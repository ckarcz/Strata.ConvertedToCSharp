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
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using LegalEntityDiscountingProviderDataSets = com.opengamma.strata.pricer.datasets.LegalEntityDiscountingProviderDataSets;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using ResolvedBondFuture = com.opengamma.strata.product.bond.ResolvedBondFuture;
	using ResolvedFixedCouponBond = com.opengamma.strata.product.bond.ResolvedFixedCouponBond;

	/// <summary>
	/// Test <seealso cref="DiscountingBondFutureProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingBondFutureProductPricerTest
	public class DiscountingBondFutureProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // product
	  private static readonly ResolvedBondFuture FUTURE_PRODUCT = BondDataSets.FUTURE_PRODUCT_USD.resolve(REF_DATA);
	  private static readonly ResolvedFixedCouponBond BOND = BondDataSets.BOND_USD[0].resolve(REF_DATA);
	  private static readonly double?[] CONVERSION_FACTOR = BondDataSets.CONVERSION_FACTOR_USD.Clone();
	  // curves
	  private static readonly LegalEntityDiscountingProvider PROVIDER = LegalEntityDiscountingProviderDataSets.ISSUER_REPO_ZERO;
	  private static readonly CurveMetadata METADATA_ISSUER = LegalEntityDiscountingProviderDataSets.META_ZERO_ISSUER_USD;
	  private static readonly CurveMetadata METADATA_REPO = LegalEntityDiscountingProviderDataSets.META_ZERO_REPO_USD;
	  // parameters
	  private const double Z_SPREAD = 0.0075;
	  private const int PERIOD_PER_YEAR = 4;
	  private const double TOL = 1.0e-12;
	  private const double EPS = 1.0e-6;
	  // pricer
	  private static readonly DiscountingBondFutureProductPricer FUTURE_PRICER = DiscountingBondFutureProductPricer.DEFAULT;
	  private static readonly DiscountingFixedCouponBondProductPricer BOND_PRICER = DiscountingFixedCouponBondProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  //-------------------------------------------------------------------------
	  public virtual void test_price()
	  {
		double computed = FUTURE_PRICER.price(FUTURE_PRODUCT, PROVIDER);
		double dirtyPrice = BOND_PRICER.dirtyPriceFromCurves(BOND, PROVIDER, FUTURE_PRODUCT.LastDeliveryDate);
		double expected = BOND_PRICER.cleanPriceFromDirtyPrice(BOND, FUTURE_PRODUCT.LastDeliveryDate, dirtyPrice) / CONVERSION_FACTOR[0];
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_priceWithZSpread_continuous()
	  {
		double computed = FUTURE_PRICER.priceWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, CONTINUOUS, 0);
		double dirtyPrice = BOND_PRICER.dirtyPriceFromCurvesWithZSpread(BOND, PROVIDER, Z_SPREAD, CONTINUOUS, 0, FUTURE_PRODUCT.LastDeliveryDate);
		double expected = BOND_PRICER.cleanPriceFromDirtyPrice(BOND, FUTURE_PRODUCT.LastDeliveryDate, dirtyPrice) / CONVERSION_FACTOR[0];
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_priceWithZSpread_periodic()
	  {
		double computed = FUTURE_PRICER.priceWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		double dirtyPrice = BOND_PRICER.dirtyPriceFromCurvesWithZSpread(BOND, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR, FUTURE_PRODUCT.LastDeliveryDate);
		double expected = BOND_PRICER.cleanPriceFromDirtyPrice(BOND, FUTURE_PRODUCT.LastDeliveryDate, dirtyPrice) / CONVERSION_FACTOR[0];
		assertEquals(computed, expected, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_priceSensitivity()
	  {
		PointSensitivities point = FUTURE_PRICER.priceSensitivity(FUTURE_PRODUCT, PROVIDER);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(PROVIDER, (p) => CurrencyAmount.of(USD, FUTURE_PRICER.price(FUTURE_PRODUCT, (p))));
		assertTrue(computed.equalWithTolerance(expected, EPS * 10.0));
	  }

	  public virtual void test_priceSensitivityWithZSpread_continuous()
	  {
		PointSensitivities point = FUTURE_PRICER.priceSensitivityWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, CONTINUOUS, 0);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(PROVIDER, (p) => CurrencyAmount.of(USD, FUTURE_PRICER.priceWithZSpread(FUTURE_PRODUCT, (p), Z_SPREAD, CONTINUOUS, 0)));
		assertTrue(computed.equalWithTolerance(expected, EPS * 10.0));
	  }

	  public virtual void test_priceSensitivityWithZSpread_periodic()
	  {
		PointSensitivities point = FUTURE_PRICER.priceSensitivityWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(PROVIDER, (p) => CurrencyAmount.of(USD, FUTURE_PRICER.priceWithZSpread(FUTURE_PRODUCT, (p), Z_SPREAD, PERIODIC, PERIOD_PER_YEAR)));
		assertTrue(computed.equalWithTolerance(expected, EPS * 10.0));
	  }

	  //-------------------------------------------------------------------------
	  // regression to 2.x
	  public virtual void regression()
	  {
		double price = FUTURE_PRICER.price(FUTURE_PRODUCT, PROVIDER);
		assertEquals(price, 1.2106928633440506, TOL);
		PointSensitivities point = FUTURE_PRICER.priceSensitivity(FUTURE_PRODUCT, PROVIDER);
		CurrencyParameterSensitivities test = PROVIDER.parameterSensitivity(point);

		DoubleArray expectedIssuer = DoubleArray.of(-3.940585873921608E-4, -0.004161527192990392, -0.014331606019672717, -1.0229665443857998, -4.220553063715371, 0);
		DoubleArray actualIssuer = test.getSensitivity(METADATA_ISSUER.CurveName, USD).Sensitivity;
		assertTrue(actualIssuer.equalWithTolerance(expectedIssuer, TOL));

		DoubleArray expectedRepo = DoubleArray.of(0.14752541809405412, 0.20907575809356016, 0.0, 0.0, 0.0, 0.0);
		DoubleArray actualRepo = test.getSensitivity(METADATA_REPO.CurveName, USD).Sensitivity;
		assertTrue(actualRepo.equalWithTolerance(expectedRepo, TOL));
	  }

	  public virtual void regression_withZSpread_continuous()
	  {
		double price = FUTURE_PRICER.priceWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, CONTINUOUS, 0);
		assertEquals(price, 1.1718691843665354, TOL);
	   // curve parameter sensitivity is not supported for continuous z-spread in 2.x.
	  }

	  public virtual void regression_withZSpread_periodic()
	  {
		double price = FUTURE_PRICER.priceWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		assertEquals(price, 1.1720190529653407, TOL);
		PointSensitivities point = FUTURE_PRICER.priceSensitivityWithZSpread(FUTURE_PRODUCT, PROVIDER, Z_SPREAD, PERIODIC, PERIOD_PER_YEAR);
		CurrencyParameterSensitivities test = PROVIDER.parameterSensitivity(point);

		DoubleArray expectedIssuer = DoubleArray.of(-3.9201229100932256E-4, -0.0041367134351306374, -0.014173323438217467, -0.9886444827927878, -4.07533109609094, 0);
		DoubleArray actualIssuer = test.getSensitivity(METADATA_ISSUER.CurveName, USD).Sensitivity;
		assertTrue(actualIssuer.equalWithTolerance(expectedIssuer, TOL));

		DoubleArray expectedRepo = DoubleArray.of(0.1428352116441475, 0.20242871054203687, 0.0, 0.0, 0.0, 0.0);
		DoubleArray actualRepo = test.getSensitivity(METADATA_REPO.CurveName, USD).Sensitivity;
		assertTrue(actualRepo.equalWithTolerance(expectedRepo, TOL));
	  }

	}

}