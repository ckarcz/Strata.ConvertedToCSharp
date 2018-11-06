using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.sensitivity
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ImmutableLegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.ImmutableLegalEntityDiscountingProvider;
	using CreditDiscountFactors = com.opengamma.strata.pricer.credit.CreditDiscountFactors;
	using CreditRatesProvider = com.opengamma.strata.pricer.credit.CreditRatesProvider;
	using ImmutableCreditRatesProvider = com.opengamma.strata.pricer.credit.ImmutableCreditRatesProvider;
	using IsdaCreditDiscountFactors = com.opengamma.strata.pricer.credit.IsdaCreditDiscountFactors;
	using LegalEntitySurvivalProbabilities = com.opengamma.strata.pricer.credit.LegalEntitySurvivalProbabilities;
	using CreditRatesProviderDataSets = com.opengamma.strata.pricer.datasets.CreditRatesProviderDataSets;
	using LegalEntityDiscountingProviderDataSets = com.opengamma.strata.pricer.datasets.LegalEntityDiscountingProviderDataSets;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;

	/// <summary>
	/// Tests <seealso cref="RatesFiniteDifferenceSensitivityCalculator"/>.
	/// </summary>
	public class RatesFiniteDifferenceSensitivityCalculatorTest
	{

	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CALCULATOR = RatesFiniteDifferenceSensitivityCalculator.DEFAULT;

	  private const double TOLERANCE_DELTA = 1.0E-8;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void sensitivity_single_curve()
	  public virtual void sensitivity_single_curve()
	  {
		CurrencyParameterSensitivities sensiComputed = FD_CALCULATOR.sensitivity(RatesProviderDataSets.SINGLE_USD, this.fn);
		DoubleArray times = RatesProviderDataSets.TIMES_1;
		assertEquals(sensiComputed.size(), 1);
		DoubleArray s = sensiComputed.Sensitivities.get(0).Sensitivity;
		assertEquals(s.size(), times.size());
		for (int i = 0; i < times.size(); i++)
		{
		  assertEquals(s.get(i), times.get(i) * 4.0d, TOLERANCE_DELTA);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void sensitivity_multi_curve()
	  public virtual void sensitivity_multi_curve()
	  {
		CurrencyParameterSensitivities sensiComputed = FD_CALCULATOR.sensitivity(RatesProviderDataSets.MULTI_CPI_USD, this.fn);
		DoubleArray times1 = RatesProviderDataSets.TIMES_1;
		DoubleArray times2 = RatesProviderDataSets.TIMES_2;
		DoubleArray times3 = RatesProviderDataSets.TIMES_3;
		DoubleArray times4 = RatesProviderDataSets.TIMES_4;
		assertEquals(sensiComputed.size(), 4);
		DoubleArray s1 = sensiComputed.getSensitivity(RatesProviderDataSets.USD_DSC_NAME, USD).Sensitivity;
		assertEquals(s1.size(), times1.size());
		for (int i = 0; i < times1.size(); i++)
		{
		  assertEquals(times1.get(i) * 2.0d, s1.get(i), TOLERANCE_DELTA);
		}
		DoubleArray s2 = sensiComputed.getSensitivity(RatesProviderDataSets.USD_L3_NAME, USD).Sensitivity;
		assertEquals(s2.size(), times2.size());
		for (int i = 0; i < times2.size(); i++)
		{
		  assertEquals(times2.get(i), s2.get(i), TOLERANCE_DELTA);
		}
		DoubleArray s3 = sensiComputed.getSensitivity(RatesProviderDataSets.USD_L6_NAME, USD).Sensitivity;
		assertEquals(s3.size(), times3.size());
		for (int i = 0; i < times3.size(); i++)
		{
		  assertEquals(times3.get(i), s3.get(i), TOLERANCE_DELTA);
		}
		DoubleArray s4 = sensiComputed.getSensitivity(RatesProviderDataSets.USD_CPI_NAME, USD).Sensitivity;
		assertEquals(s4.size(), times4.size());
		for (int i = 0; i < times4.size(); i++)
		{
		  assertEquals(times4.get(i), s4.get(i), TOLERANCE_DELTA);
		}
	  }

	  // private function for testing. Returns the sum of rates multiplied by time
	  private CurrencyAmount fn(ImmutableRatesProvider provider)
	  {
		double result = 0.0;
		// Currency
		ImmutableMap<Currency, Curve> mapCurrency = provider.DiscountCurves;
		foreach (KeyValuePair<Currency, Curve> entry in mapCurrency.entrySet())
		{
		  InterpolatedNodalCurve curveInt = checkInterpolated(entry.Value);
		  result += sumProduct(curveInt);
		}
		// Index
		ImmutableMap<Index, Curve> mapIndex = provider.IndexCurves;
		foreach (KeyValuePair<Index, Curve> entry in mapIndex.entrySet())
		{
		  InterpolatedNodalCurve curveInt = checkInterpolated(entry.Value);
		  result += sumProduct(curveInt);
		}
		return CurrencyAmount.of(USD, result);
	  }

	  // compute the sum of the product of times and rates
	  private double sumProduct(NodalCurve curveInt)
	  {
		double result = 0.0;
		DoubleArray x = curveInt.XValues;
		DoubleArray y = curveInt.YValues;
		int nbNodePoint = x.size();
		for (int i = 0; i < nbNodePoint; i++)
		{
		  result += x.get(i) * y.get(i);
		}
		return result;
	  }

	  // check that the curve is InterpolatedNodalCurve
	  private InterpolatedNodalCurve checkInterpolated(Curve curve)
	  {
		ArgChecker.isTrue(curve is InterpolatedNodalCurve, "Curve should be a InterpolatedNodalCurve");
		return (InterpolatedNodalCurve) curve;
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void sensitivity_legalEntity_Zero()
	  public virtual void sensitivity_legalEntity_Zero()
	  {
		CurrencyParameterSensitivities sensiComputed = FD_CALCULATOR.sensitivity(LegalEntityDiscountingProviderDataSets.ISSUER_REPO_ZERO, this.fn);
		DoubleArray timeIssuer = LegalEntityDiscountingProviderDataSets.ISSUER_TIME_USD;
		DoubleArray timesRepo = LegalEntityDiscountingProviderDataSets.REPO_TIME_USD;
		assertEquals(sensiComputed.size(), 2);
		DoubleArray sensiIssuer = sensiComputed.getSensitivity(LegalEntityDiscountingProviderDataSets.META_ZERO_ISSUER_USD.CurveName, USD).Sensitivity;
		assertEquals(sensiIssuer.size(), timeIssuer.size());
		for (int i = 0; i < timeIssuer.size(); i++)
		{
		  assertEquals(timeIssuer.get(i), sensiIssuer.get(i), TOLERANCE_DELTA);
		}
		DoubleArray sensiRepo = sensiComputed.getSensitivity(LegalEntityDiscountingProviderDataSets.META_ZERO_REPO_USD.CurveName, USD).Sensitivity;
		assertEquals(sensiRepo.size(), timesRepo.size());
		for (int i = 0; i < timesRepo.size(); i++)
		{
		  assertEquals(timesRepo.get(i), sensiRepo.get(i), TOLERANCE_DELTA);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void sensitivity_legalEntity_Simple()
	  public virtual void sensitivity_legalEntity_Simple()
	  {
		CurrencyParameterSensitivities sensiComputed = FD_CALCULATOR.sensitivity(LegalEntityDiscountingProviderDataSets.ISSUER_REPO_SIMPLE, this.fn);
		DoubleArray timeIssuer = LegalEntityDiscountingProviderDataSets.ISSUER_TIME_USD;
		DoubleArray timesRepo = LegalEntityDiscountingProviderDataSets.REPO_TIME_USD;
		assertEquals(sensiComputed.size(), 2);
		DoubleArray sensiIssuer = sensiComputed.getSensitivity(LegalEntityDiscountingProviderDataSets.META_SIMPLE_ISSUER_USD.CurveName, USD).Sensitivity;
		assertEquals(sensiIssuer.size(), timeIssuer.size());
		for (int i = 0; i < timeIssuer.size(); i++)
		{
		  assertEquals(timeIssuer.get(i), sensiIssuer.get(i), TOLERANCE_DELTA);
		}
		DoubleArray sensiRepo = sensiComputed.getSensitivity(LegalEntityDiscountingProviderDataSets.META_SIMPLE_REPO_USD.CurveName, USD).Sensitivity;
		assertEquals(sensiRepo.size(), timesRepo.size());
		for (int i = 0; i < timesRepo.size(); i++)
		{
		  assertEquals(timesRepo.get(i), sensiRepo.get(i), TOLERANCE_DELTA);
		}
	  }

	  // private function for testing. Returns the sum of rates multiplied by time
	  private CurrencyAmount fn(ImmutableLegalEntityDiscountingProvider provider)
	  {
		double result = 0.0;
		// issuer curve
		ImmutableMap<Pair<LegalEntityGroup, Currency>, DiscountFactors> mapLegal = provider.metaBean().issuerCurves().get(provider);
		foreach (KeyValuePair<Pair<LegalEntityGroup, Currency>, DiscountFactors> entry in mapLegal.entrySet())
		{
		  InterpolatedNodalCurve curveInt = checkInterpolated(checkDiscountFactors(entry.Value));
		  result += sumProduct(curveInt);
		}
		// repo curve
		ImmutableMap<Pair<RepoGroup, Currency>, DiscountFactors> mapRepo = provider.metaBean().repoCurves().get(provider);
		foreach (KeyValuePair<Pair<RepoGroup, Currency>, DiscountFactors> entry in mapRepo.entrySet())
		{
		  InterpolatedNodalCurve curveInt = checkInterpolated(checkDiscountFactors(entry.Value));
		  result += sumProduct(curveInt);
		}
		return CurrencyAmount.of(USD, result);
	  }

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

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void sensitivity_credit_isda()
	  public virtual void sensitivity_credit_isda()
	  {
		LocalDate valuationDate = LocalDate.of(2014, 1, 3);
		CreditRatesProvider rates = CreditRatesProviderDataSets.createCreditRatesProvider(valuationDate);
		CurrencyParameterSensitivities sensiComputed = FD_CALCULATOR.sensitivity(rates, this.creditFunction);
		IList<IsdaCreditDiscountFactors> curves = CreditRatesProviderDataSets.getAllDiscountFactors(valuationDate);
		assertEquals(sensiComputed.size(), curves.Count);
		foreach (IsdaCreditDiscountFactors curve in curves)
		{
		  DoubleArray time = curve.ParameterKeys;
		  DoubleArray sensiValueComputed = sensiComputed.getSensitivity(curve.Curve.Name, USD).Sensitivity;
		  assertEquals(sensiValueComputed.size(), time.size());
		  for (int i = 0; i < time.size(); i++)
		  {
			assertEquals(time.get(i), sensiValueComputed.get(i), TOLERANCE_DELTA);
		  }
		}
	  }

	  // private function for testing. Returns the sum of rates multiplied by time
	  private CurrencyAmount creditFunction(ImmutableCreditRatesProvider provider)
	  {
		double result = 0.0;
		// credit curve
		ImmutableMap<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities> mapCredit = provider.metaBean().creditCurves().get(provider);
		foreach (KeyValuePair<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities> entry in mapCredit.entrySet())
		{
		  InterpolatedNodalCurve curveInt = checkInterpolated(checkDiscountFactors(entry.Value.SurvivalProbabilities.toDiscountFactors()));
		  result += sumProduct(curveInt);
		}
		// repo curve
		ImmutableMap<Currency, CreditDiscountFactors> mapDiscount = provider.metaBean().discountCurves().get(provider);
		foreach (KeyValuePair<Currency, CreditDiscountFactors> entry in mapDiscount.entrySet())
		{
		  InterpolatedNodalCurve curveInt = checkInterpolated(checkDiscountFactors(entry.Value.toDiscountFactors()));
		  result += sumProduct(curveInt);
		}
		return CurrencyAmount.of(USD, result);
	  }

	}

}