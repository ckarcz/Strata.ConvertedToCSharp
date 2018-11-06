using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.sensitivity
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using Doubles = com.google.common.primitives.Doubles;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using RateIndex = com.opengamma.strata.basics.index.RateIndex;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using ParallelShiftedCurve = com.opengamma.strata.market.curve.ParallelShiftedCurve;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CrossGammaParameterSensitivities = com.opengamma.strata.market.param.CrossGammaParameterSensitivities;
	using CrossGammaParameterSensitivity = com.opengamma.strata.market.param.CrossGammaParameterSensitivity;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using FiniteDifferenceType = com.opengamma.strata.math.impl.differentiation.FiniteDifferenceType;
	using VectorFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.VectorFieldFirstOrderDifferentiator;
	using ImmutableLegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.ImmutableLegalEntityDiscountingProvider;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// Computes the gamma-related values for the rates curve parameters.
	/// <para>
	/// By default the gamma is computed using a one basis-point shift and a forward finite difference.
	/// The results themselves are not scaled (they represent the second order derivative).
	/// </para>
	/// <para>
	/// Reference: Interest Rate Cross-gamma for Single and Multiple Curves. OpenGamma quantitative research 15, July 14
	/// </para>
	/// </summary>
	public sealed class CurveGammaCalculator
	{

	  /// <summary>
	  /// Default implementation. Finite difference is forward and the shift is one basis point (0.0001).
	  /// </summary>
	  public static readonly CurveGammaCalculator DEFAULT = new CurveGammaCalculator(FiniteDifferenceType.FORWARD, 1e-4);

	  /// <summary>
	  /// The first order finite difference calculator.
	  /// </summary>
	  private readonly VectorFieldFirstOrderDifferentiator fd;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of the finite difference calculator using forward differencing.
	  /// </summary>
	  /// <param name="shift">  the shift to be applied to the curves </param>
	  /// <returns> the calculator </returns>
	  public static CurveGammaCalculator ofForwardDifference(double shift)
	  {
		return new CurveGammaCalculator(FiniteDifferenceType.FORWARD, shift);
	  }

	  /// <summary>
	  /// Obtains an instance of the finite difference calculator using central differencing.
	  /// </summary>
	  /// <param name="shift">  the shift to be applied to the curves </param>
	  /// <returns> the calculator </returns>
	  public static CurveGammaCalculator ofCentralDifference(double shift)
	  {
		return new CurveGammaCalculator(FiniteDifferenceType.CENTRAL, shift);
	  }

	  /// <summary>
	  /// Obtains an instance of the finite difference calculator using backward differencing.
	  /// </summary>
	  /// <param name="shift">  the shift to be applied to the curves </param>
	  /// <returns> the calculator </returns>
	  public static CurveGammaCalculator ofBackwardDifference(double shift)
	  {
		return new CurveGammaCalculator(FiniteDifferenceType.BACKWARD, shift);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Create an instance of the finite difference calculator.
	  /// </summary>
	  /// <param name="fdType">  the finite difference type </param>
	  /// <param name="shift">  the shift to be applied to the curves </param>
	  private CurveGammaCalculator(FiniteDifferenceType fdType, double shift)
	  {
		this.fd = new VectorFieldFirstOrderDifferentiator(fdType, shift);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes intra-curve cross gamma by applying finite difference method to curve delta.
	  /// <para>
	  /// This computes the intra-curve cross gamma, i.e., the second order sensitivities to individual curves. 
	  /// Thus the sensitivity of a curve delta to another curve is not produced.
	  /// </para>
	  /// <para>
	  /// The sensitivities are computed for discount curves, and forward curves for {@code RateIndex} and {@code PriceIndex}. 
	  /// This implementation works only for single currency trades. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="sensitivitiesFn">  the sensitivity function </param>
	  /// <returns> the cross gamma </returns>
	  public CrossGammaParameterSensitivities calculateCrossGammaIntraCurve(RatesProvider ratesProvider, System.Func<ImmutableRatesProvider, CurrencyParameterSensitivities> sensitivitiesFn)
	  {

		ImmutableRatesProvider immProv = ratesProvider.toImmutableRatesProvider();
		CurrencyParameterSensitivities baseDelta = sensitivitiesFn(immProv); // used to check target sensitivity exits
		CrossGammaParameterSensitivities result = CrossGammaParameterSensitivities.empty();
		// discount curve
		foreach (KeyValuePair<Currency, Curve> entry in immProv.DiscountCurves.entrySet())
		{
		  Currency currency = entry.Key;
		  Curve curve = entry.Value;
		  if (baseDelta.findSensitivity(curve.Name, currency).Present)
		  {
			CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(curve, currency, c => immProv.toBuilder().discountCurve(currency, c).build(), sensitivitiesFn);
			result = result.combinedWith(gammaSingle);
		  }
		  else if (curve.split().size() > 1)
		  {
			ImmutableList<Curve> curves = curve.split();
			int nCurves = curves.size();
			for (int i = 0; i < nCurves; ++i)
			{
			  int currentIndex = i;
			  Curve underlyingCurve = curves.get(currentIndex);
			  if (baseDelta.findSensitivity(underlyingCurve.Name, currency).Present)
			  {
				CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(underlyingCurve, currency, c => immProv.toBuilder().discountCurve(currency, curve.withUnderlyingCurve(currentIndex, c)).build(), sensitivitiesFn);
				result = result.combinedWith(gammaSingle);
			  }
			}
		  }
		}
		// forward curve
		foreach (KeyValuePair<Index, Curve> entry in immProv.IndexCurves.entrySet())
		{
		  Index index = entry.Key;
		  if (index is RateIndex || index is PriceIndex)
		  {
			Currency currency = getCurrency(index);
			Curve curve = entry.Value;
			if (baseDelta.findSensitivity(curve.Name, currency).Present)
			{
			  CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(curve, currency, c => immProv.toBuilder().indexCurve(index, c).build(), sensitivitiesFn);
			  result = result.combinedWith(gammaSingle);
			}
			else if (curve.split().size() > 1)
			{
			  ImmutableList<Curve> curves = curve.split();
			  int nCurves = curves.size();
			  for (int i = 0; i < nCurves; ++i)
			  {
				int currentIndex = i;
				Curve underlyingCurve = curves.get(currentIndex);
				if (baseDelta.findSensitivity(underlyingCurve.Name, currency).Present)
				{
				  CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(underlyingCurve, currency, c => immProv.toBuilder().indexCurve(index, curve.withUnderlyingCurve(currentIndex, c)).build(), sensitivitiesFn);
				  result = result.combinedWith(gammaSingle);
				}
			  }
			}
		  }
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes intra-curve cross gamma for bond curves by applying finite difference method to curve delta.
	  /// <para>
	  /// This computes the intra-curve cross gamma, i.e., the second order sensitivities to individual curves. 
	  /// Thus the sensitivity of a curve delta to another curve is not produced.
	  /// </para>
	  /// <para>
	  /// The underlying instruments must be single-currency, i.e., the curve currency must be the same as the sensitivity currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="sensitivitiesFn">  the sensitivity function </param>
	  /// <returns> the cross gamma </returns>
	  public CrossGammaParameterSensitivities calculateCrossGammaIntraCurve(LegalEntityDiscountingProvider ratesProvider, System.Func<ImmutableLegalEntityDiscountingProvider, CurrencyParameterSensitivities> sensitivitiesFn)
	  {

		LocalDate valuationDate = ratesProvider.ValuationDate;
		ImmutableLegalEntityDiscountingProvider immProv = ratesProvider.toImmutableLegalEntityDiscountingProvider();
		CurrencyParameterSensitivities baseDelta = sensitivitiesFn(immProv); // used to check target sensitivity exits
		CrossGammaParameterSensitivities result = CrossGammaParameterSensitivities.empty();
		// issuer curve
		foreach (KeyValuePair<Pair<LegalEntityGroup, Currency>, DiscountFactors> entry in immProv.IssuerCurves.entrySet())
		{
		  Pair<LegalEntityGroup, Currency> legCcy = entry.Key;
		  Currency currency = legCcy.Second;
		  Curve curve = getCurve(entry.Value);
		  CurveName curveName = curve.Name;
		  if (baseDelta.findSensitivity(curveName, currency).Present)
		  {
			CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(curveName, curve, currency, c => replaceIssuerCurve(immProv, legCcy, DiscountFactors.of(currency, valuationDate, c)), sensitivitiesFn);
			result = result.combinedWith(gammaSingle);
		  }
		  else
		  {
			ImmutableList<Curve> curves = curve.split();
			int nCurves = curves.size();
			if (nCurves > 1)
			{
			  for (int i = 0; i < nCurves; ++i)
			  {
				int currentIndex = i;
				Curve underlyingCurve = curves.get(currentIndex);
				CurveName underlyingCurveName = underlyingCurve.Name;
				if (baseDelta.findSensitivity(underlyingCurveName, currency).Present)
				{
				  CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(underlyingCurveName, underlyingCurve, currency, c => replaceIssuerCurve(immProv, legCcy, DiscountFactors.of(currency, valuationDate, curve.withUnderlyingCurve(currentIndex, c))), sensitivitiesFn);
				  result = result.combinedWith(gammaSingle);
				}
			  }
			}
		  }
		}
		// repo curve
		foreach (KeyValuePair<Pair<RepoGroup, Currency>, DiscountFactors> entry in immProv.RepoCurves.entrySet())
		{
		  Pair<RepoGroup, Currency> rgCcy = entry.Key;
		  Currency currency = rgCcy.Second;
		  Curve curve = getCurve(entry.Value);
		  CurveName curveName = curve.Name;
		  if (baseDelta.findSensitivity(curveName, currency).Present)
		  {
			CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(curveName, curve, currency, c => replaceRepoCurve(immProv, rgCcy, DiscountFactors.of(currency, valuationDate, c)), sensitivitiesFn);
			result = result.combinedWith(gammaSingle);
		  }
		  else
		  {
			ImmutableList<Curve> curves = curve.split();
			int nCurves = curves.size();
			if (nCurves > 1)
			{
			  for (int i = 0; i < nCurves; ++i)
			  {
				int currentIndex = i;
				Curve underlyingCurve = curves.get(currentIndex);
				CurveName underlyingCurveName = underlyingCurve.Name;
				if (baseDelta.findSensitivity(underlyingCurveName, rgCcy.Second).Present)
				{
				  CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(underlyingCurveName, underlyingCurve, currency, c => replaceRepoCurve(immProv, rgCcy, DiscountFactors.of(currency, valuationDate, curve.withUnderlyingCurve(currentIndex, c))), sensitivitiesFn);
				  result = result.combinedWith(gammaSingle);
				}
			  }
			}
		  }
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes cross-curve gamma by applying finite difference method to curve delta.
	  /// <para>
	  /// This computes the cross-curve gamma, i.e., the second order sensitivities to full curves. 
	  /// Thus the sensitivities of curve delta to other curves are produced.
	  /// </para>
	  /// <para>
	  /// The sensitivities are computed for discount curves, and forward curves for {@code RateIndex} and {@code PriceIndex}. 
	  /// This implementation works only for single currency trades. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="sensitivitiesFn">  the sensitivity function </param>
	  /// <returns> the cross gamma </returns>
	  public CrossGammaParameterSensitivities calculateCrossGammaCrossCurve(RatesProvider ratesProvider, System.Func<ImmutableRatesProvider, CurrencyParameterSensitivities> sensitivitiesFn)
	  {

		ImmutableRatesProvider immProv = ratesProvider.toImmutableRatesProvider();
		CurrencyParameterSensitivities baseDelta = sensitivitiesFn(immProv); // used to check target sensitivity exits.
		CrossGammaParameterSensitivities result = CrossGammaParameterSensitivities.empty();
		foreach (CurrencyParameterSensitivity baseDeltaSingle in baseDelta.Sensitivities)
		{
		  CrossGammaParameterSensitivities resultInner = CrossGammaParameterSensitivities.empty();
		  // discount curve
		  foreach (KeyValuePair<Currency, Curve> entry in immProv.DiscountCurves.entrySet())
		  {
			Currency currency = entry.Key;
			Curve curve = entry.Value;
			if (baseDelta.findSensitivity(curve.Name, currency).Present)
			{
			  CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(baseDeltaSingle, curve, c => immProv.toBuilder().discountCurve(currency, c).build(), sensitivitiesFn);
			  resultInner = resultInner.combinedWith(gammaSingle);
			}
			else if (curve.split().size() > 1)
			{
			  ImmutableList<Curve> curves = curve.split();
			  int nCurves = curves.size();
			  for (int i = 0; i < nCurves; ++i)
			  {
				int currentIndex = i;
				Curve underlyingCurve = curves.get(currentIndex);
				if (baseDelta.findSensitivity(underlyingCurve.Name, currency).Present)
				{
				  CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(baseDeltaSingle, underlyingCurve, c => immProv.toBuilder().discountCurve(currency, curve.withUnderlyingCurve(currentIndex, c)).build(), sensitivitiesFn);
				  resultInner = resultInner.combinedWith(gammaSingle);
				}
			  }
			}
		  }
		  // forward curve
		  foreach (KeyValuePair<Index, Curve> entry in immProv.IndexCurves.entrySet())
		  {
			Index index = entry.Key;
			if (index is RateIndex || index is PriceIndex)
			{
			  Currency currency = getCurrency(index);
			  Curve curve = entry.Value;
			  if (baseDelta.findSensitivity(curve.Name, currency).Present)
			  {
				CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(baseDeltaSingle, curve, c => immProv.toBuilder().indexCurve(index, c).build(), sensitivitiesFn);
				resultInner = resultInner.combinedWith(gammaSingle);
			  }
			  else if (curve.split().size() > 1)
			  {
				ImmutableList<Curve> curves = curve.split();
				int nCurves = curves.size();
				for (int i = 0; i < nCurves; ++i)
				{
				  int currentIndex = i;
				  Curve underlyingCurve = curves.get(currentIndex);
				  if (baseDelta.findSensitivity(underlyingCurve.Name, currency).Present)
				  {
					CrossGammaParameterSensitivity gammaSingle = computeGammaForCurve(baseDeltaSingle, underlyingCurve, c => immProv.toBuilder().indexCurve(index, curve.withUnderlyingCurve(currentIndex, c)).build(), sensitivitiesFn);
					resultInner = resultInner.combinedWith(gammaSingle);
				  }
				}
			  }
			}
		  }
		  result = result.combinedWith(combineSensitivities(baseDeltaSingle, resultInner));
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  private Currency getCurrency(Index index)
	  {
		if (index is RateIndex)
		{
		  return ((RateIndex) index).Currency;
		}
		else if (index is PriceIndex)
		{
		  return ((PriceIndex) index).Currency;
		}
		throw new System.ArgumentException("unsupported index");
	  }

	  // compute the second order sensitivity to Curve
	  internal CrossGammaParameterSensitivity computeGammaForCurve(Curve curve, Currency sensitivityCurrency, System.Func<Curve, ImmutableRatesProvider> ratesProviderFn, System.Func<ImmutableRatesProvider, CurrencyParameterSensitivities> sensitivitiesFn)
	  {

		System.Func<DoubleArray, DoubleArray> function = (DoubleArray t) =>
		{
	Curve newCurve = replaceParameters(curve, t);
	ImmutableRatesProvider newRates = ratesProviderFn(newCurve);
	CurrencyParameterSensitivities sensiMulti = sensitivitiesFn(newRates);
	return sensiMulti.getSensitivity(newCurve.Name, sensitivityCurrency).Sensitivity;
		};
		int nParams = curve.ParameterCount;
		DoubleMatrix sensi = fd.differentiate(function).apply(DoubleArray.of(nParams, n => curve.getParameter(n)));
		IList<ParameterMetadata> metadata = IntStream.range(0, nParams).mapToObj(i => curve.getParameterMetadata(i)).collect(toImmutableList());
		return CrossGammaParameterSensitivity.of(curve.Name, metadata, sensitivityCurrency, sensi);
	  }

	  // computes the sensitivity of baseDeltaSingle to Curve
	  internal CrossGammaParameterSensitivity computeGammaForCurve(CurrencyParameterSensitivity baseDeltaSingle, Curve curve, System.Func<Curve, ImmutableRatesProvider> ratesProviderFn, System.Func<ImmutableRatesProvider, CurrencyParameterSensitivities> sensitivitiesFn)
	  {

		System.Func<DoubleArray, DoubleArray> function = (DoubleArray t) =>
		{
	Curve newCurve = replaceParameters(curve, t);
	ImmutableRatesProvider newRates = ratesProviderFn(newCurve);
	CurrencyParameterSensitivities sensiMulti = sensitivitiesFn(newRates);
	return sensiMulti.getSensitivity(baseDeltaSingle.MarketDataName, baseDeltaSingle.Currency).Sensitivity;
		};
		int nParams = curve.ParameterCount;
		DoubleMatrix sensi = fd.differentiate(function).apply(DoubleArray.of(nParams, n => curve.getParameter(n)));
		IList<ParameterMetadata> metadata = IntStream.range(0, nParams).mapToObj(i => curve.getParameterMetadata(i)).collect(toImmutableList());
		return CrossGammaParameterSensitivity.of(baseDeltaSingle.MarketDataName, baseDeltaSingle.ParameterMetadata, curve.Name, metadata, baseDeltaSingle.Currency, sensi);
	  }

	  private CrossGammaParameterSensitivity combineSensitivities(CurrencyParameterSensitivity baseDeltaSingle, CrossGammaParameterSensitivities blockCrossGamma)
	  {

		double[][] valuesTotal = new double[baseDeltaSingle.ParameterCount][];
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.data.MarketDataName<?>, java.util.List<? extends com.opengamma.strata.market.param.ParameterMetadata>>> order = new java.util.ArrayList<>();
		IList<Pair<MarketDataName<object>, IList<ParameterMetadata>>> order = new List<Pair<MarketDataName<object>, IList<ParameterMetadata>>>();
		for (int i = 0; i < baseDeltaSingle.ParameterCount; ++i)
		{
		  List<double> innerList = new List<double>();
		  foreach (CrossGammaParameterSensitivity gammaSingle in blockCrossGamma.Sensitivities)
		  {
			innerList.AddRange(gammaSingle.Sensitivity.row(i).toList());
			if (i == 0)
			{
			  order.Add(gammaSingle.Order.get(0));
			}
		  }
		  valuesTotal[i] = Doubles.toArray(innerList);
		}
		return CrossGammaParameterSensitivity.of(baseDeltaSingle.MarketDataName, baseDeltaSingle.ParameterMetadata, order, baseDeltaSingle.Currency, DoubleMatrix.ofUnsafe(valuesTotal));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the "sum-of-column gamma" or "semi-parallel gamma" for a sensitivity function.
	  /// <para>
	  /// This implementation supports a single <seealso cref="Curve"/> on the zero-coupon rates.
	  /// By default the gamma is computed using a one basis-point shift and a forward finite difference.
	  /// The results themselves are not scaled (they represent the second order derivative).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curve">  the single curve to be bumped </param>
	  /// <param name="curveCurrency">  the currency of the curve and resulting sensitivity </param>
	  /// <param name="sensitivitiesFn">  the function to convert the bumped curve to parameter sensitivities </param>
	  /// <returns> the "sum-of-columns" or "semi-parallel" gamma vector </returns>
	  public CurrencyParameterSensitivity calculateSemiParallelGamma(Curve curve, Currency curveCurrency, System.Func<Curve, CurrencyParameterSensitivity> sensitivitiesFn)
	  {

		Delta deltaShift = new Delta(curve, sensitivitiesFn);
		System.Func<DoubleArray, DoubleMatrix> gammaFn = fd.differentiate(deltaShift);
		DoubleArray gamma = gammaFn(DoubleArray.filled(1)).column(0);
		return curve.createParameterSensitivity(curveCurrency, gamma);
	  }

	  //-------------------------------------------------------------------------
	  private Curve replaceParameters(Curve curve, DoubleArray newParameters)
	  {
		return curve.withPerturbation((i, v, m) => newParameters.get(i));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Inner class to compute the delta for a given parallel shift of the curve.
	  /// </summary>
	  internal class Delta : System.Func<DoubleArray, DoubleArray>
	  {
		internal readonly Curve curve;
		internal readonly System.Func<Curve, CurrencyParameterSensitivity> sensitivitiesFn;

		internal Delta(Curve curve, System.Func<Curve, CurrencyParameterSensitivity> sensitivitiesFn)
		{
		  this.curve = curve;
		  this.sensitivitiesFn = sensitivitiesFn;
		}

		public override DoubleArray apply(DoubleArray s)
		{
		  double shift = s.get(0);
		  Curve curveBumped = ParallelShiftedCurve.absolute(curve, shift);
		  CurrencyParameterSensitivity pts = sensitivitiesFn.apply(curveBumped);
		  return pts.Sensitivity;
		}
	  }

	  //-------------------------------------------------------------------------
	  private Curve getCurve(DiscountFactors discountFactors)
	  {
		if (discountFactors is SimpleDiscountFactors)
		{
		  return ((SimpleDiscountFactors) discountFactors).Curve;
		}
		if (discountFactors is ZeroRateDiscountFactors)
		{
		  return ((ZeroRateDiscountFactors) discountFactors).Curve;
		}
		if (discountFactors is ZeroRatePeriodicDiscountFactors)
		{
		  return ((ZeroRatePeriodicDiscountFactors) discountFactors).Curve;
		}
		throw new System.ArgumentException("Unsupported DiscountFactors type");
	  }

	  private CrossGammaParameterSensitivity computeGammaForCurve(CurveName curveName, Curve curve, Currency sensitivityCurrency, System.Func<Curve, ImmutableLegalEntityDiscountingProvider> ratesProviderFn, System.Func<ImmutableLegalEntityDiscountingProvider, CurrencyParameterSensitivities> sensitivitiesFn)
	  {

		System.Func<DoubleArray, DoubleArray> function = (DoubleArray t) =>
		{
	Curve newCurve = curve.withPerturbation((i, v, m) => t.get(i));
	ImmutableLegalEntityDiscountingProvider newRates = ratesProviderFn(newCurve);
	CurrencyParameterSensitivities sensiMulti = sensitivitiesFn(newRates);
	return sensiMulti.getSensitivity(curveName, sensitivityCurrency).Sensitivity;
		};
		int nParams = curve.ParameterCount;
		DoubleMatrix sensi = fd.differentiate(function).apply(DoubleArray.of(nParams, n => curve.getParameter(n)));
		IList<ParameterMetadata> metadata = IntStream.range(0, nParams).mapToObj(i => curve.getParameterMetadata(i)).collect(toImmutableList());
		return CrossGammaParameterSensitivity.of(curveName, metadata, sensitivityCurrency, sensi);
	  }

	  private ImmutableLegalEntityDiscountingProvider replaceIssuerCurve(ImmutableLegalEntityDiscountingProvider ratesProvider, Pair<LegalEntityGroup, Currency> legCcy, DiscountFactors discountFactors)
	  {

		IDictionary<Pair<LegalEntityGroup, Currency>, DiscountFactors> curves = new Dictionary<Pair<LegalEntityGroup, Currency>, DiscountFactors>();
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		curves.putAll(ratesProvider.IssuerCurves);
		curves[legCcy] = discountFactors;

		return ratesProvider.toBuilder().issuerCurves(curves).build();
	  }

	  private ImmutableLegalEntityDiscountingProvider replaceRepoCurve(ImmutableLegalEntityDiscountingProvider ratesProvider, Pair<RepoGroup, Currency> rgCcy, DiscountFactors discountFactors)
	  {

		IDictionary<Pair<RepoGroup, Currency>, DiscountFactors> curves = new Dictionary<Pair<RepoGroup, Currency>, DiscountFactors>();
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		curves.putAll(ratesProvider.RepoCurves);
		curves[rgCcy] = discountFactors;

		return ratesProvider.toBuilder().repoCurves(curves).build();
	  }

	}

}