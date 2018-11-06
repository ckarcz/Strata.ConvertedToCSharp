using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.sensitivity
{

	using MetaProperty = org.joda.beans.MetaProperty;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ImmutableLegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.ImmutableLegalEntityDiscountingProvider;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using CreditDiscountFactors = com.opengamma.strata.pricer.credit.CreditDiscountFactors;
	using CreditRatesProvider = com.opengamma.strata.pricer.credit.CreditRatesProvider;
	using ImmutableCreditRatesProvider = com.opengamma.strata.pricer.credit.ImmutableCreditRatesProvider;
	using IsdaCreditDiscountFactors = com.opengamma.strata.pricer.credit.IsdaCreditDiscountFactors;
	using LegalEntitySurvivalProbabilities = com.opengamma.strata.pricer.credit.LegalEntitySurvivalProbabilities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// Computes the curve parameter sensitivity by finite difference.
	/// <para>
	/// This is based on an <seealso cref="ImmutableRatesProvider"/>, <seealso cref="LegalEntityDiscountingProvider"/> or <seealso cref="CreditRatesProvider"/>.
	/// The sensitivities are calculated by finite difference.
	/// </para>
	/// </summary>
	public class RatesFiniteDifferenceSensitivityCalculator
	{

	  /// <summary>
	  /// Default implementation. The shift is one basis point (0.0001).
	  /// </summary>
	  public static readonly RatesFiniteDifferenceSensitivityCalculator DEFAULT = new RatesFiniteDifferenceSensitivityCalculator(1.0E-4);

	  /// <summary>
	  /// The shift used for finite difference.
	  /// </summary>
	  private readonly double shift;

	  /// <summary>
	  /// Create an instance of the finite difference calculator.
	  /// </summary>
	  /// <param name="shift">  the shift used in the finite difference computation </param>
	  public RatesFiniteDifferenceSensitivityCalculator(double shift)
	  {
		this.shift = shift;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the first order sensitivities of a function of a RatesProvider to a double by finite difference.
	  /// <para>
	  /// The finite difference is computed by forward type.
	  /// The function should return a value in the same currency for any rate provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="valueFn">  the function from a rate provider to a currency amount for which the sensitivity should be computed </param>
	  /// <returns> the curve sensitivity </returns>
	  public virtual CurrencyParameterSensitivities sensitivity(RatesProvider provider, System.Func<ImmutableRatesProvider, CurrencyAmount> valueFn)
	  {

		ImmutableRatesProvider immProv = provider.toImmutableRatesProvider();
		CurrencyAmount valueInit = valueFn(immProv);
		CurrencyParameterSensitivities discounting = sensitivity(immProv, immProv.DiscountCurves, (@base, bumped) => @base.toBuilder().discountCurves(bumped).build(), valueFn, valueInit);
		CurrencyParameterSensitivities forward = sensitivity(immProv, immProv.IndexCurves, (@base, bumped) => @base.toBuilder().indexCurves(bumped).build(), valueFn, valueInit);
		return discounting.combinedWith(forward);
	  }

	  // computes the sensitivity with respect to the curves
	  private CurrencyParameterSensitivities sensitivity<T>(ImmutableRatesProvider provider, IDictionary<T, Curve> baseCurves, System.Func<ImmutableRatesProvider, IDictionary<T, Curve>, ImmutableRatesProvider> storeBumpedFn, System.Func<ImmutableRatesProvider, CurrencyAmount> valueFn, CurrencyAmount valueInit)
	  {

		CurrencyParameterSensitivities result = CurrencyParameterSensitivities.empty();
		foreach (KeyValuePair<T, Curve> entry in baseCurves.SetOfKeyValuePairs())
		{
		  Curve curve = entry.Value;
		  DoubleArray sensitivity = DoubleArray.of(curve.ParameterCount, i =>
		  {
		  Curve dscBumped = curve.withParameter(i, curve.getParameter(i) + shift);
		  IDictionary<T, Curve> mapBumped = new Dictionary<T, Curve>(baseCurves);
		  mapBumped[entry.Key] = dscBumped;
		  ImmutableRatesProvider providerDscBumped = storeBumpedFn(provider, mapBumped);
		  return (valueFn(providerDscBumped).Amount - valueInit.Amount) / shift;
		  });
		  result = result.combinedWith(curve.createParameterSensitivity(valueInit.Currency, sensitivity));
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the first order sensitivities of a function of a LegalEntityDiscountingProvider to a double by finite difference.
	  /// <para>
	  /// The finite difference is computed by forward type.
	  /// The function should return a value in the same currency for any rates provider of LegalEntityDiscountingProvider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="valueFn">  the function from a rate provider to a currency amount for which the sensitivity should be computed </param>
	  /// <returns> the curve sensitivity </returns>
	  public virtual CurrencyParameterSensitivities sensitivity(LegalEntityDiscountingProvider provider, System.Func<ImmutableLegalEntityDiscountingProvider, CurrencyAmount> valueFn)
	  {

		ImmutableLegalEntityDiscountingProvider immProv = provider.toImmutableLegalEntityDiscountingProvider();
		CurrencyAmount valueInit = valueFn(immProv);
		CurrencyParameterSensitivities discounting = sensitivity(immProv, valueFn, ImmutableLegalEntityDiscountingProvider.meta().repoCurves(), valueInit);
		CurrencyParameterSensitivities forward = sensitivity(immProv, valueFn, ImmutableLegalEntityDiscountingProvider.meta().issuerCurves(), valueInit);
		return discounting.combinedWith(forward);
	  }

	  private CurrencyParameterSensitivities sensitivity<T>(ImmutableLegalEntityDiscountingProvider provider, System.Func<ImmutableLegalEntityDiscountingProvider, CurrencyAmount> valueFn, MetaProperty<ImmutableMap<Pair<T, Currency>, DiscountFactors>> metaProperty, CurrencyAmount valueInit)
	  {

		ImmutableMap<Pair<T, Currency>, DiscountFactors> baseCurves = metaProperty.get(provider);
		CurrencyParameterSensitivities result = CurrencyParameterSensitivities.empty();
		foreach (Pair<T, Currency> key in baseCurves.Keys)
		{
		  DiscountFactors discountFactors = baseCurves.get(key);
		  Curve curve = checkDiscountFactors(discountFactors);
		  int paramCount = curve.ParameterCount;
		  double[] sensitivity = new double[paramCount];
		  for (int i = 0; i < paramCount; i++)
		  {
			Curve dscBumped = curve.withParameter(i, curve.getParameter(i) + shift);
			IDictionary<Pair<T, Currency>, DiscountFactors> mapBumped = new Dictionary<Pair<T, Currency>, DiscountFactors>(baseCurves);
			mapBumped[key] = createDiscountFactors(discountFactors, dscBumped);
			ImmutableLegalEntityDiscountingProvider providerDscBumped = provider.toBuilder().set(metaProperty, mapBumped).build();
			sensitivity[i] = (valueFn(providerDscBumped).Amount - valueInit.Amount) / shift;
		  }
		  result = result.combinedWith(curve.createParameterSensitivity(valueInit.Currency, DoubleArray.copyOf(sensitivity)));
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the first order sensitivities of a function of a {@code CreditRatesProvider} to a double by finite difference.
	  /// <para>
	  /// The finite difference is computed by forward type.
	  /// The function should return a value in the same currency for any rates provider of {@code CreditRatesProvider}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="valueFn">  the function from a rate provider to a currency amount for which the sensitivity should be computed </param>
	  /// <returns> the curve sensitivity </returns>
	  public virtual CurrencyParameterSensitivities sensitivity(CreditRatesProvider provider, System.Func<ImmutableCreditRatesProvider, CurrencyAmount> valueFn)
	  {

		ImmutableCreditRatesProvider immutableProvider = provider.toImmutableCreditRatesProvider();
		CurrencyAmount valueInit = valueFn(immutableProvider);
		CurrencyParameterSensitivities discounting = sensitivityDiscountCurve(immutableProvider, valueFn, ImmutableCreditRatesProvider.meta().discountCurves(), valueInit);
		CurrencyParameterSensitivities credit = sensitivityCreidtCurve(immutableProvider, valueFn, ImmutableCreditRatesProvider.meta().creditCurves(), valueInit);
		return discounting.combinedWith(credit);
	  }

	  private CurrencyParameterSensitivities sensitivityDiscountCurve<T>(ImmutableCreditRatesProvider provider, System.Func<ImmutableCreditRatesProvider, CurrencyAmount> valueFn, MetaProperty<ImmutableMap<T, CreditDiscountFactors>> metaProperty, CurrencyAmount valueInit)
	  {

		ImmutableMap<T, CreditDiscountFactors> baseCurves = metaProperty.get(provider);
		CurrencyParameterSensitivities result = CurrencyParameterSensitivities.empty();
		foreach (T key in baseCurves.Keys)
		{
		  CreditDiscountFactors creditDiscountFactors = baseCurves.get(key);
		  DiscountFactors discountFactors = creditDiscountFactors.toDiscountFactors();
		  Curve curve = checkDiscountFactors(discountFactors);
		  int paramCount = curve.ParameterCount;
		  double[] sensitivity = new double[paramCount];
		  for (int i = 0; i < paramCount; i++)
		  {
			Curve dscBumped = curve.withParameter(i, curve.getParameter(i) + shift);
			IDictionary<T, CreditDiscountFactors> mapBumped = new Dictionary<T, CreditDiscountFactors>(baseCurves);
			mapBumped[key] = createCreditDiscountFactors(creditDiscountFactors, dscBumped);
			ImmutableCreditRatesProvider providerDscBumped = provider.toBuilder().set(metaProperty, mapBumped).build();
			sensitivity[i] = (valueFn(providerDscBumped).Amount - valueInit.Amount) / shift;
		  }
		  result = result.combinedWith(curve.createParameterSensitivity(valueInit.Currency, DoubleArray.copyOf(sensitivity)));
		}
		return result;
	  }

	  private CurrencyParameterSensitivities sensitivityCreidtCurve<T>(ImmutableCreditRatesProvider provider, System.Func<ImmutableCreditRatesProvider, CurrencyAmount> valueFn, MetaProperty<ImmutableMap<T, LegalEntitySurvivalProbabilities>> metaProperty, CurrencyAmount valueInit)
	  {

		ImmutableMap<T, LegalEntitySurvivalProbabilities> baseCurves = metaProperty.get(provider);
		CurrencyParameterSensitivities result = CurrencyParameterSensitivities.empty();
		foreach (T key in baseCurves.Keys)
		{
		  LegalEntitySurvivalProbabilities credit = baseCurves.get(key);
		  CreditDiscountFactors creditDiscountFactors = credit.SurvivalProbabilities;
		  DiscountFactors discountFactors = creditDiscountFactors.toDiscountFactors();
		  Curve curve = checkDiscountFactors(discountFactors);
		  int paramCount = curve.ParameterCount;
		  double[] sensitivity = new double[paramCount];
		  for (int i = 0; i < paramCount; i++)
		  {
			Curve dscBumped = curve.withParameter(i, curve.getParameter(i) + shift);
			IDictionary<T, LegalEntitySurvivalProbabilities> mapBumped = new Dictionary<T, LegalEntitySurvivalProbabilities>(baseCurves);
			mapBumped[key] = LegalEntitySurvivalProbabilities.of(credit.LegalEntityId, createCreditDiscountFactors(creditDiscountFactors, dscBumped));
			ImmutableCreditRatesProvider providerDscBumped = provider.toBuilder().set(metaProperty, mapBumped).build();
			sensitivity[i] = (valueFn(providerDscBumped).Amount - valueInit.Amount) / shift;
		  }
		  result = result.combinedWith(curve.createParameterSensitivity(valueInit.Currency, DoubleArray.copyOf(sensitivity)));
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  // check that the discountFactors is ZeroRateDiscountFactors or SimpleDiscountFactors
	  private Curve checkDiscountFactors(DiscountFactors discountFactors)
	  {
		if (discountFactors is ZeroRateDiscountFactors)
		{
		  return ((ZeroRateDiscountFactors) discountFactors).Curve;
		}
		else if (discountFactors is SimpleDiscountFactors)
		{
		  return ((SimpleDiscountFactors) discountFactors).Curve;
		}
		throw new System.ArgumentException("Not supported");
	  }

	  // return correct instance of DiscountFactors
	  private DiscountFactors createDiscountFactors(DiscountFactors originalDsc, Curve bumpedCurve)
	  {
		if (originalDsc is ZeroRateDiscountFactors)
		{
		  return ZeroRateDiscountFactors.of(originalDsc.Currency, originalDsc.ValuationDate, bumpedCurve);
		}
		else if (originalDsc is SimpleDiscountFactors)
		{
		  return SimpleDiscountFactors.of(originalDsc.Currency, originalDsc.ValuationDate, bumpedCurve);
		}
		throw new System.ArgumentException("Not supported");
	  }

	  // return correct instance of CreditDiscountFactors
	  private CreditDiscountFactors createCreditDiscountFactors(CreditDiscountFactors originalDsc, Curve bumpedCurve)
	  {
		if (originalDsc is IsdaCreditDiscountFactors && bumpedCurve is NodalCurve)
		{
		  IsdaCreditDiscountFactors isdaDsc = (IsdaCreditDiscountFactors) originalDsc;
		  return isdaDsc.withCurve((NodalCurve) bumpedCurve);
		}
		throw new System.ArgumentException("Not supported");
	  }

	}

}