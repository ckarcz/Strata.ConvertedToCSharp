using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.linearalgebra.DecompositionFactory.SV_COMMONS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory.OG_ALGEBRA;


	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using ValueType = com.opengamma.strata.market.ValueType;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using DoubleRangeLimitTransform = com.opengamma.strata.math.impl.minimization.DoubleRangeLimitTransform;
	using NonLinearTransformFunction = com.opengamma.strata.math.impl.minimization.NonLinearTransformFunction;
	using ParameterLimitsTransform = com.opengamma.strata.math.impl.minimization.ParameterLimitsTransform;
	using ParameterLimitsTransform_LimitType = com.opengamma.strata.math.impl.minimization.ParameterLimitsTransform_LimitType;
	using SingleRangeLimitTransform = com.opengamma.strata.math.impl.minimization.SingleRangeLimitTransform;
	using UncoupledParameterTransforms = com.opengamma.strata.math.impl.minimization.UncoupledParameterTransforms;
	using LeastSquareResults = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResults;
	using LeastSquareResultsWithTransform = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResultsWithTransform;
	using NonLinearLeastSquare = com.opengamma.strata.math.impl.statistics.leastsquare.NonLinearLeastSquare;
	using SabrParameters = com.opengamma.strata.pricer.model.SabrParameters;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Caplet volatilities calibration to cap volatilities based on SABR model.
	/// <para>
	/// The SABR model parameters are computed by bootstrapping along the expiry time dimension. 
	/// The result is a complete set of curves for the SABR parameters spanned by the expiry time.  
	/// The position of the node points on the resultant curves corresponds to market cap expiries, 
	/// and are interpolated by a local interpolation scheme. 
	/// See <seealso cref="SabrIborCapletFloorletVolatilityBootstrapDefinition"/> for detail.
	/// </para>
	/// <para>
	/// The calibration to SABR is computed once the option volatility date is converted to prices. Thus we should note that 
	/// the error values in {@code RawOptionData} are applied in the price space rather than the volatility space.
	/// </para>
	/// </summary>
	public class SabrIborCapletFloorletVolatilityBootstrapper : IborCapletFloorletVolatilityCalibrator
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly SabrIborCapletFloorletVolatilityBootstrapper DEFAULT = of(VolatilityIborCapFloorLegPricer.DEFAULT, SabrIborCapletFloorletPeriodPricer.DEFAULT, 1.0e-10, ReferenceData.standard());

	  /// <summary>
	  /// Transformation for SABR parameters.
	  /// </summary>
	  private static readonly ParameterLimitsTransform[] TRANSFORMS;
	  /// <summary>
	  /// SABR parameter range. 
	  /// </summary>
	  private const double RHO_LIMIT = 0.999;
	  static SabrIborCapletFloorletVolatilityBootstrapper()
	  {
		TRANSFORMS = new ParameterLimitsTransform[4];
		TRANSFORMS[0] = new SingleRangeLimitTransform(0, ParameterLimitsTransform_LimitType.GREATER_THAN); // alpha > 0
		TRANSFORMS[1] = new DoubleRangeLimitTransform(0.0, 1.0); // 0 <= beta <= 1
		TRANSFORMS[2] = new DoubleRangeLimitTransform(-RHO_LIMIT, RHO_LIMIT); // -1 <= rho <= 1
		TRANSFORMS[3] = new DoubleRangeLimitTransform(0.001d, 2.50d);
		// nu > 0  and limit on Nu to avoid numerical instability in formula for large nu.
	  }

	  /// <summary>
	  /// The nonlinear least square solver.
	  /// </summary>
	  private readonly NonLinearLeastSquare solver;
	  /// <summary>
	  /// SABR pricer for caplet/floorlet.
	  /// </summary>
	  private readonly SabrIborCapletFloorletPeriodPricer sabrPeriodPricer;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance. 
	  /// <para>
	  /// The epsilon is the parameter used in <seealso cref="NonLinearLeastSquare"/>, where the iteration stops when certain 
	  /// quantities are smaller than this parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pricer">  the cap/floor pricer to convert quoted volatilities to prices </param>
	  /// <param name="sabrPeriodPricer">  the SABR pricer </param>
	  /// <param name="epsilon">  the epsilon parameter </param>
	  /// <param name="referenceData">  the reference data </param>
	  /// <returns> the instance </returns>
	  public static SabrIborCapletFloorletVolatilityBootstrapper of(VolatilityIborCapFloorLegPricer pricer, SabrIborCapletFloorletPeriodPricer sabrPeriodPricer, double epsilon, ReferenceData referenceData)
	  {

		NonLinearLeastSquare solver = new NonLinearLeastSquare(SV_COMMONS, OG_ALGEBRA, epsilon);
		return new SabrIborCapletFloorletVolatilityBootstrapper(pricer, sabrPeriodPricer, solver, referenceData);
	  }

	  // private constructor
	  private SabrIborCapletFloorletVolatilityBootstrapper(VolatilityIborCapFloorLegPricer pricer, SabrIborCapletFloorletPeriodPricer sabrPeriodPricer, NonLinearLeastSquare solver, ReferenceData referenceData) : base(pricer, referenceData)
	  {

		this.sabrPeriodPricer = ArgChecker.notNull(sabrPeriodPricer, "sabrPeriodPricer");
		this.solver = ArgChecker.notNull(solver, "solver");
	  }

	  //-------------------------------------------------------------------------
	  public override IborCapletFloorletVolatilityCalibrationResult calibrate(IborCapletFloorletVolatilityDefinition definition, ZonedDateTime calibrationDateTime, RawOptionData capFloorData, RatesProvider ratesProvider)
	  {

		ArgChecker.isTrue(ratesProvider.ValuationDate.Equals(calibrationDateTime.toLocalDate()), "valuationDate of ratesProvider should be coherent to calibrationDateTime");
		ArgChecker.isTrue(definition is SabrIborCapletFloorletVolatilityBootstrapDefinition, "definition should be SabrIborCapletFloorletVolatilityBootstrapDefinition");
		SabrIborCapletFloorletVolatilityBootstrapDefinition bsDefinition = (SabrIborCapletFloorletVolatilityBootstrapDefinition) definition;
		IborIndex index = bsDefinition.Index;
		LocalDate calibrationDate = calibrationDateTime.toLocalDate();
		LocalDate baseDate = index.EffectiveDateOffset.adjust(calibrationDate, ReferenceData);
		LocalDate startDate = baseDate.plus(index.Tenor);
		System.Func<Surface, IborCapletFloorletVolatilities> volatilitiesFunction = this.volatilitiesFunction(bsDefinition, calibrationDateTime, capFloorData);
		SurfaceMetadata metaData = bsDefinition.createMetadata(capFloorData);
		IList<Period> expiries = capFloorData.Expiries;
		int nExpiries = expiries.Count;
		DoubleArray strikes = capFloorData.Strikes;
		DoubleMatrix errorsMatrix = capFloorData.Error.orElse(DoubleMatrix.filled(nExpiries, strikes.size(), 1d));
		IList<double> timeList = new List<double>();
		IList<double> strikeList = new List<double>();
		IList<double> volList = new List<double>();
		IList<ResolvedIborCapFloorLeg> capList = new List<ResolvedIborCapFloorLeg>();
		IList<double> priceList = new List<double>();
		IList<double> errorList = new List<double>();
		int[] startIndex = new int[nExpiries + 1];
		for (int i = 0; i < nExpiries; ++i)
		{
		  LocalDate endDate = baseDate.plus(expiries[i]);
		  DoubleArray volatilityData = capFloorData.Data.row(i);
		  DoubleArray errors = errorsMatrix.row(i);
		  reduceRawData(bsDefinition, ratesProvider, strikes, volatilityData, errors, startDate, endDate, metaData, volatilitiesFunction, timeList, strikeList, volList, capList, priceList, errorList);
		  startIndex[i + 1] = volList.Count;
		  ArgChecker.isTrue(startIndex[i + 1] > startIndex[i], "no valid option data for {}", expiries[i]);
		}

		IList<CurveMetadata> metadataList = bsDefinition.createSabrParameterMetadata();
		DoubleArray timeToExpiries = DoubleArray.of(nExpiries, i => timeList[startIndex[i]]);

		BitArray @fixed = new BitArray();
		bool betaFix = false;
		Curve betaCurve;
		Curve rhoCurve;
		if (bsDefinition.BetaCurve.Present)
		{
		  betaFix = true;
		  @fixed.Set(1, true);
		  betaCurve = bsDefinition.BetaCurve.get();
		  rhoCurve = InterpolatedNodalCurve.of(metadataList[2], timeToExpiries, DoubleArray.filled(nExpiries), bsDefinition.Interpolator, bsDefinition.ExtrapolatorLeft, bsDefinition.ExtrapolatorRight);
		}
		else
		{
		  @fixed.Set(2, true);
		  betaCurve = InterpolatedNodalCurve.of(metadataList[1], timeToExpiries, DoubleArray.filled(nExpiries), bsDefinition.Interpolator, bsDefinition.ExtrapolatorLeft, bsDefinition.ExtrapolatorRight);
		  rhoCurve = bsDefinition.RhoCurve.get();
		}
		InterpolatedNodalCurve alphaCurve = InterpolatedNodalCurve.of(metadataList[0], timeToExpiries, DoubleArray.filled(nExpiries), bsDefinition.Interpolator, bsDefinition.ExtrapolatorLeft, bsDefinition.ExtrapolatorRight);
		InterpolatedNodalCurve nuCurve = InterpolatedNodalCurve.of(metadataList[3], timeToExpiries, DoubleArray.filled(nExpiries), bsDefinition.Interpolator, bsDefinition.ExtrapolatorLeft, bsDefinition.ExtrapolatorRight);
		Curve shiftCurve = bsDefinition.ShiftCurve;
		SabrParameters sabrParams = SabrParameters.of(alphaCurve, betaCurve, rhoCurve, nuCurve, shiftCurve, bsDefinition.SabrVolatilityFormula);
		SabrParametersIborCapletFloorletVolatilities vols = SabrParametersIborCapletFloorletVolatilities.of(bsDefinition.Name, index, calibrationDateTime, sabrParams);
		double totalChiSq = 0d;
		ZonedDateTime prevExpiry = calibrationDateTime.minusDays(1L); // included if calibrationDateTime == fixingDateTime
		for (int i = 0; i < nExpiries; ++i)
		{
		  DoubleArray start = computeInitialValues(ratesProvider, betaCurve, shiftCurve, timeList, volList, capList, startIndex, i, betaFix, capFloorData.DataType);
		  UncoupledParameterTransforms transform = new UncoupledParameterTransforms(start, TRANSFORMS, @fixed);
		  int nCaplets = startIndex[i + 1] - startIndex[i];
		  int currentStart = startIndex[i];
		  System.Func<DoubleArray, DoubleArray> valueFunction = createPriceFunction(ratesProvider, vols, prevExpiry, capList, priceList, startIndex, nExpiries, i, nCaplets, betaFix);
		  System.Func<DoubleArray, DoubleMatrix> jacobianFunction = createJacobianFunction(ratesProvider, vols, prevExpiry, capList, priceList, index.Currency, startIndex, nExpiries, i, nCaplets, betaFix);
		  NonLinearTransformFunction transFunc = new NonLinearTransformFunction(valueFunction, jacobianFunction, transform);
		  DoubleArray adjustedPrices = this.adjustedPrices(ratesProvider, vols, prevExpiry, capList, priceList, startIndex, i, nCaplets);
		  DoubleArray errors = DoubleArray.of(nCaplets, n => errorList[currentStart + n]);
		  LeastSquareResults res = solver.solve(adjustedPrices, errors, transFunc.FittingFunction, transFunc.FittingJacobian, transform.transform(start));
		  LeastSquareResultsWithTransform resTransform = new LeastSquareResultsWithTransform(res, transform);
		  vols = updateParameters(vols, nExpiries, i, betaFix, resTransform.ModelParameters);
		  totalChiSq += res.ChiSq;
		  prevExpiry = capList[startIndex[i + 1] - 1].FinalFixingDateTime;
		}
		return IborCapletFloorletVolatilityCalibrationResult.ofLeastSquare(vols, totalChiSq);
	  }

	  //-------------------------------------------------------------------------
	  // computes initial guess for each time step
	  private DoubleArray computeInitialValues(RatesProvider ratesProvider, Curve betaCurve, Curve shiftCurve, IList<double> timeList, IList<double> volList, IList<ResolvedIborCapFloorLeg> capList, int[] startIndex, int postion, bool betaFixed, ValueType valueType)
	  {

		IList<double> vols = volList.subList(startIndex[postion], startIndex[postion + 1]);
		ResolvedIborCapFloorLeg cap = capList[startIndex[postion]];
		double fwd = ratesProvider.iborIndexRates(cap.Index).rate(cap.FinalPeriod.IborRate.Observation);
		double shift = shiftCurve.yValue(timeList[startIndex[postion]]);
		double factor = valueType.Equals(ValueType.BLACK_VOLATILITY) ? 1d : 1d / (fwd + shift);
		IList<double> volsEquiv = vols.Select(v => v * factor).ToList();
		double nuFirst;
		double betaInitial = betaFixed ? betaCurve.yValue(timeList[startIndex[postion]]) : 0.5d;
		double alphaInitial = DoubleArray.copyOf(volsEquiv).min() * Math.Pow(fwd, 1d - betaInitial);
		if (alphaInitial == volsEquiv[0] || alphaInitial == volsEquiv[volsEquiv.Count - 1])
		{
		  nuFirst = 0.1d;
		  alphaInitial *= 0.95d;
		}
		else
		{
		  nuFirst = 1d;
		}
		return DoubleArray.of(alphaInitial, betaInitial, -0.5 * betaInitial + 0.5 * (1d - betaInitial), nuFirst);
	  }

	  // price function
	  private System.Func<DoubleArray, DoubleArray> createPriceFunction(RatesProvider ratesProvider, SabrParametersIborCapletFloorletVolatilities volatilities, ZonedDateTime prevExpiry, IList<ResolvedIborCapFloorLeg> capList, IList<double> priceList, int[] startIndex, int nExpiries, int timeIndex, int nCaplets, bool betaFixed)
	  {

		int currentStart = startIndex[timeIndex];
		System.Func<DoubleArray, DoubleArray> priceFunction = (DoubleArray x) =>
		{
	SabrParametersIborCapletFloorletVolatilities volsNew = updateParameters(volatilities, nExpiries, timeIndex, betaFixed, x);
	return DoubleArray.of(nCaplets, n => capList[currentStart + n].CapletFloorletPeriods.Where(p => p.FixingDateTime.isAfter(prevExpiry)).Select(p => sabrPeriodPricer.presentValue(p, ratesProvider, volsNew).Amount).Sum() / priceList[currentStart + n]);
		};
		return priceFunction;
	  }

	  // node sensitivity function
	  private System.Func<DoubleArray, DoubleMatrix> createJacobianFunction(RatesProvider ratesProvider, SabrParametersIborCapletFloorletVolatilities volatilities, ZonedDateTime prevExpiry, IList<ResolvedIborCapFloorLeg> capList, IList<double> priceList, Currency currency, int[] startIndex, int nExpiries, int timeIndex, int nCaplets, bool betaFixed)
	  {

		Curve alphaCurve = volatilities.Parameters.AlphaCurve;
		Curve betaCurve = volatilities.Parameters.BetaCurve;
		Curve rhoCurve = volatilities.Parameters.RhoCurve;
		Curve nuCurve = volatilities.Parameters.NuCurve;
		int currentStart = startIndex[timeIndex];
		System.Func<DoubleArray, DoubleMatrix> jacobianFunction = (DoubleArray x) =>
		{
	SabrParametersIborCapletFloorletVolatilities volsNew = updateParameters(volatilities, nExpiries, timeIndex, betaFixed, x);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] jacobian = new double[nCaplets][4];
	double[][] jacobian = RectangularArrays.ReturnRectangularDoubleArray(nCaplets, 4);
	for (int i = 0; i < nCaplets; ++i)
	{
	  PointSensitivities point = capList[currentStart + i].CapletFloorletPeriods.Where(p => p.FixingDateTime.isAfter(prevExpiry)).Select(p => sabrPeriodPricer.presentValueSensitivityModelParamsSabr(p, ratesProvider, volsNew)).Aggregate((c1, c2) => c1.combinedWith(c2)).get().build();
	  double targetPrice = priceList[currentStart + i];
	  CurrencyParameterSensitivities sensi = volsNew.parameterSensitivity(point);
	  jacobian[i][0] = sensi.getSensitivity(alphaCurve.Name, currency).Sensitivity.get(timeIndex) / targetPrice;
	  if (betaFixed)
	  {
		jacobian[i][1] = 0d;
		jacobian[i][2] = sensi.getSensitivity(rhoCurve.Name, currency).Sensitivity.get(timeIndex) / targetPrice;

	  }
	  else
	  {
		jacobian[i][1] = sensi.getSensitivity(betaCurve.Name, currency).Sensitivity.get(timeIndex) / targetPrice;
		jacobian[i][2] = 0d;

	  }
	  jacobian[i][3] = sensi.getSensitivity(nuCurve.Name, currency).Sensitivity.get(timeIndex) / targetPrice;
	}
	return DoubleMatrix.ofUnsafe(jacobian);
		};
		return jacobianFunction;
	  }

	  // update vols
	  private SabrParametersIborCapletFloorletVolatilities updateParameters(SabrParametersIborCapletFloorletVolatilities volatilities, int nExpiries, int timeIndex, bool betaFixed, DoubleArray newParameters)
	  {

		int nBetaParams = volatilities.Parameters.BetaCurve.ParameterCount;
		int nRhoParams = volatilities.Parameters.RhoCurve.ParameterCount;
		SabrParametersIborCapletFloorletVolatilities newVols = volatilities.withParameter(timeIndex, newParameters.get(0)).withParameter(timeIndex + nExpiries + nBetaParams + nRhoParams, newParameters.get(3));
		if (betaFixed)
		{
		  newVols = newVols.withParameter(timeIndex + nExpiries + nBetaParams, newParameters.get(2));
		  return newVols;
		}
		newVols = newVols.withParameter(timeIndex + nExpiries, newParameters.get(1));
		return newVols;
	  }

	  // sum of caplet prices which are not fixed
	  private DoubleArray adjustedPrices(RatesProvider ratesProvider, IborCapletFloorletVolatilities vols, ZonedDateTime prevExpiry, IList<ResolvedIborCapFloorLeg> capList, IList<double> priceList, int[] startIndex, int timeIndex, int nCaplets)
	  {

		if (timeIndex == 0)
		{
		  return DoubleArray.filled(nCaplets, 1d);
		}
		int currentStart = startIndex[timeIndex];
		return DoubleArray.of(nCaplets, n => (priceList[currentStart + n] - capList[currentStart + n].CapletFloorletPeriods.Where(p => !p.FixingDateTime.isAfter(prevExpiry)).Select(p => sabrPeriodPricer.presentValue(p, ratesProvider, vols).Amount).Sum()) / priceList[currentStart + n]);
	  }

	}

}