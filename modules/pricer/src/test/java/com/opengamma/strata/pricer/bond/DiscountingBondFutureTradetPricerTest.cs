/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.CompoundedRateType.CONTINUOUS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.CompoundedRateType.PERIODIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using LegalEntityDiscountingProviderDataSets = com.opengamma.strata.pricer.datasets.LegalEntityDiscountingProviderDataSets;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using ResolvedBondFuture = com.opengamma.strata.product.bond.ResolvedBondFuture;
	using ResolvedBondFutureTrade = com.opengamma.strata.product.bond.ResolvedBondFutureTrade;

	/// <summary>
	/// Test <seealso cref="DiscountingBondFutureTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingBondFutureTradetPricerTest
	public class DiscountingBondFutureTradetPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // product and trade
	  private static readonly ResolvedBondFuture FUTURE_PRODUCT = BondDataSets.FUTURE_PRODUCT_USD.resolve(REF_DATA);
	  private static readonly ResolvedBondFutureTrade FUTURE_TRADE = BondDataSets.FUTURE_TRADE_USD.resolve(REF_DATA);
	  private const double SETTLE_PRICE = BondDataSets.SETTLE_PRICE_USD;
	  private const double NOTIONAL = BondDataSets.NOTIONAL_USD;
	  private const long QUANTITY = BondDataSets.QUANTITY_USD;
	  // curves
	  private static readonly LegalEntityDiscountingProvider PROVIDER = LegalEntityDiscountingProviderDataSets.ISSUER_REPO_ZERO;
	  private static readonly CurveMetadata METADATA_ISSUER = LegalEntityDiscountingProviderDataSets.META_ZERO_ISSUER_USD;
	  private static readonly CurveMetadata METADATA_REPO = LegalEntityDiscountingProviderDataSets.META_ZERO_REPO_USD;
	  // parameters
	  private const double Z_SPREAD = 0.0075;
	  private const int PERIOD_PER_YEAR = 4;
	  private const double TOL = 1.0e-12;
	  private const double EPS = 1.0e-6;
	  // pricers
	  private static readonly DiscountingBondFutureTradePricer TRADE_PRICER = DiscountingBondFutureTradePricer.DEFAULT;
	  private static readonly DiscountingBondFutureProductPricer PRODUCT_PRICER = DiscountingBondFutureProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  public virtual void test_price()
	  {
		double computed = TRADE_PRICER.price(FUTURE_TRADE, PROVIDER);
		double expected = PRODUCT_PRICER.price(FUTURE_PRODUCT, PROVIDER);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_priceWithZSpread_continuous()
	  {
		double computed = TRADE_PRICER.priceWithZSpread(FUTURE_TRADE, PROVIDER, Z_SPREAD, CONTINUOUS, 0);
		double expected = PRODUCT_PRICER.priceWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, CONTINUOUS, 0);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_priceWithZSpread_periodic()
	  {
		double computed = TRADE_PRICER.priceWithZSpread(FUTURE_TRADE, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		double expected = PRODUCT_PRICER.priceWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_presentValue()
	  {
		CurrencyAmount computed = TRADE_PRICER.presentValue(FUTURE_TRADE, PROVIDER, SETTLE_PRICE);
		double expected = (PRODUCT_PRICER.price(FUTURE_PRODUCT, PROVIDER) - SETTLE_PRICE) * NOTIONAL * QUANTITY;
		assertEquals(computed.Currency, USD);
		assertEquals(computed.Amount, expected, TOL * NOTIONAL * QUANTITY);
	  }

	  public virtual void test_presentValueWithZSpread_continuous()
	  {
		CurrencyAmount computed = TRADE_PRICER.presentValueWithZSpread(FUTURE_TRADE, PROVIDER, SETTLE_PRICE, Z_SPREAD, CONTINUOUS, 0);
		double expected = (PRODUCT_PRICER.priceWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, CONTINUOUS, 0) - SETTLE_PRICE) * NOTIONAL * QUANTITY;
		assertEquals(computed.Currency, USD);
		assertEquals(computed.Amount, expected, TOL * NOTIONAL * QUANTITY);
	  }

	  public virtual void test_presentValueWithZSpread_periodic()
	  {
		CurrencyAmount computed = TRADE_PRICER.presentValueWithZSpread(FUTURE_TRADE, PROVIDER, SETTLE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		double expected = NOTIONAL * QUANTITY * (PRODUCT_PRICER.priceWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR) - SETTLE_PRICE);
		assertEquals(computed.Currency, USD);
		assertEquals(computed.Amount, expected, TOL * NOTIONAL * QUANTITY);
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities point = TRADE_PRICER.presentValueSensitivity(FUTURE_TRADE, PROVIDER);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(PROVIDER, (p) => TRADE_PRICER.presentValue(FUTURE_TRADE, (p), SETTLE_PRICE));
		assertTrue(computed.equalWithTolerance(expected, 10.0 * EPS * NOTIONAL * QUANTITY));
	  }

	  public virtual void test_presentValueSensitivityWithZSpread_continuous()
	  {
		PointSensitivities point = TRADE_PRICER.presentValueSensitivityWithZSpread(FUTURE_TRADE, PROVIDER, Z_SPREAD, CONTINUOUS, 0);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(PROVIDER, (p) => TRADE_PRICER.presentValueWithZSpread(FUTURE_TRADE, (p), SETTLE_PRICE, Z_SPREAD, CONTINUOUS, 0));
		assertTrue(computed.equalWithTolerance(expected, 10.0 * EPS * NOTIONAL * QUANTITY));
	  }

	  public virtual void test_presentValueSensitivityWithZSpread_periodic()
	  {
		PointSensitivities point = TRADE_PRICER.presentValueSensitivityWithZSpread(FUTURE_TRADE, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(PROVIDER, (p) => TRADE_PRICER.presentValueWithZSpread(FUTURE_TRADE, (p), SETTLE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR));
		assertTrue(computed.equalWithTolerance(expected, 10.0 * EPS * NOTIONAL * QUANTITY));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpread()
	  {
		double computed = TRADE_PRICER.parSpread(FUTURE_TRADE, PROVIDER, SETTLE_PRICE);
		double expected = PRODUCT_PRICER.price(FUTURE_PRODUCT, PROVIDER) - SETTLE_PRICE;
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_parSpreadWithZSpread_continuous()
	  {
		double computed = TRADE_PRICER.parSpreadWithZSpread(FUTURE_TRADE, PROVIDER, SETTLE_PRICE, Z_SPREAD, CONTINUOUS, 0);
		double expected = PRODUCT_PRICER.priceWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, CONTINUOUS, 0) - SETTLE_PRICE;
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_parSpreadWithZSpread_periodic()
	  {
		double computed = TRADE_PRICER.parSpreadWithZSpread(FUTURE_TRADE, PROVIDER, SETTLE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		double expected = PRODUCT_PRICER.priceWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR) - SETTLE_PRICE;
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_parSpreadSensitivity()
	  {
		PointSensitivities point = TRADE_PRICER.parSpreadSensitivity(FUTURE_TRADE, PROVIDER);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(PROVIDER, (p) => CurrencyAmount.of(USD, TRADE_PRICER.parSpread(FUTURE_TRADE, (p), SETTLE_PRICE)));
		assertTrue(computed.equalWithTolerance(expected, 10.0 * EPS * NOTIONAL * QUANTITY));
	  }

	  public virtual void test_parSpreadSensitivityWithZSpread_continuous()
	  {
		PointSensitivities point = TRADE_PRICER.parSpreadSensitivityWithZSpread(FUTURE_TRADE, PROVIDER, Z_SPREAD, CONTINUOUS, 0);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(PROVIDER, (p) => CurrencyAmount.of(USD, TRADE_PRICER.parSpreadWithZSpread(FUTURE_TRADE, (p), SETTLE_PRICE, Z_SPREAD, CONTINUOUS, 0)));
		assertTrue(computed.equalWithTolerance(expected, 10.0 * EPS * NOTIONAL * QUANTITY));
	  }

	  public virtual void test_parSpreadSensitivityWithZSpread_periodic()
	  {
		PointSensitivities point = TRADE_PRICER.parSpreadSensitivityWithZSpread(FUTURE_TRADE, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(PROVIDER, (p) => CurrencyAmount.of(USD, TRADE_PRICER.parSpreadWithZSpread(FUTURE_TRADE, (p), SETTLE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)));
		assertTrue(computed.equalWithTolerance(expected, 10.0 * EPS * NOTIONAL * QUANTITY));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		MultiCurrencyAmount ceComputed = TRADE_PRICER.currencyExposure(FUTURE_TRADE, PROVIDER, SETTLE_PRICE);
		CurrencyAmount pv = TRADE_PRICER.presentValue(FUTURE_TRADE, PROVIDER, SETTLE_PRICE);
		assertEquals(ceComputed, MultiCurrencyAmount.of(pv));
	  }

	  public virtual void test_currencyExposureWithZSpread()
	  {
		MultiCurrencyAmount ceComputed = TRADE_PRICER.currencyExposureWithZSpread(FUTURE_TRADE, PROVIDER, SETTLE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyAmount pv = TRADE_PRICER.presentValueWithZSpread(FUTURE_TRADE, PROVIDER, SETTLE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		assertEquals(ceComputed, MultiCurrencyAmount.of(pv));
	  }

	  //-------------------------------------------------------------------------
	  // regression to 2.x
	  public virtual void regression()
	  {
		CurrencyAmount pv = TRADE_PRICER.presentValue(FUTURE_TRADE, PROVIDER, SETTLE_PRICE);
		assertEquals(pv.Amount, -2937800.66334416, EPS * NOTIONAL * QUANTITY);
		PointSensitivities pvPoint = TRADE_PRICER.presentValueSensitivity(FUTURE_TRADE, PROVIDER);
		CurrencyParameterSensitivities test = PROVIDER.parameterSensitivity(pvPoint);

		DoubleArray expectedIssuer = DoubleArray.of(-48626.82968419264, -513532.4556150143, -1768520.182827613, -1.262340715772077E8, -5.208162480624767E8, 0);
		DoubleArray actualIssuer = test.getSensitivity(METADATA_ISSUER.CurveName, USD).Sensitivity;
		assertTrue(actualIssuer.equalWithTolerance(expectedIssuer, TOL * NOTIONAL * QUANTITY));

		DoubleArray expectedRepo = DoubleArray.of(1.8204636592806276E7, 2.5799948548745323E7, 0.0, 0.0, 0.0, 0.0);
		DoubleArray actualRepo = test.getSensitivity(METADATA_REPO.CurveName, USD).Sensitivity;
		assertTrue(actualRepo.equalWithTolerance(expectedRepo, TOL * NOTIONAL * QUANTITY));
	  }

	  public virtual void regression_withZSpread_continuous()
	  {
		CurrencyAmount pv = TRADE_PRICER.presentValueWithZSpread(FUTURE_TRADE, PROVIDER, SETTLE_PRICE, Z_SPREAD, CONTINUOUS, 0);
		assertEquals(pv.Amount, -7728642.649169521, EPS * NOTIONAL * QUANTITY);
	  }

	  public virtual void regression_withZSpread_periodic()
	  {
		CurrencyAmount pv = TRADE_PRICER.presentValueWithZSpread(FUTURE_TRADE, PROVIDER, SETTLE_PRICE, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		assertEquals(pv.Amount, -7710148.864076961, EPS * NOTIONAL * QUANTITY);
		PointSensitivities pvPoint = TRADE_PRICER.presentValueSensitivityWithZSpread(FUTURE_TRADE, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyParameterSensitivities test = PROVIDER.parameterSensitivity(pvPoint);

		DoubleArray expectedIssuer = DoubleArray.of(-48374.31671055041, -510470.43789512076, -1748988.1122760356, -1.2199872917663E8, -5.0289585725762194E8, 0);
		DoubleArray actualIssuer = test.getSensitivity(METADATA_ISSUER.CurveName, USD).Sensitivity;
		assertTrue(actualIssuer.equalWithTolerance(expectedIssuer, TOL * NOTIONAL * QUANTITY));

		DoubleArray expectedRepo = DoubleArray.of(1.7625865116887797E7, 2.497970288088735E7, 0.0, 0.0, 0.0, 0.0);
		DoubleArray actualRepo = test.getSensitivity(METADATA_REPO.CurveName, USD).Sensitivity;
		assertTrue(actualRepo.equalWithTolerance(expectedRepo, TOL * NOTIONAL * QUANTITY));
	  }

	}

}