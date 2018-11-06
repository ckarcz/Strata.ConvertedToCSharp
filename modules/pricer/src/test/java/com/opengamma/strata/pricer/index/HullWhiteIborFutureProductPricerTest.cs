/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using HullWhiteOneFactorPiecewiseConstantParameters = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParameters;
	using HullWhiteOneFactorPiecewiseConstantParametersProvider = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParametersProvider;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using ResolvedIborFuture = com.opengamma.strata.product.index.ResolvedIborFuture;

	/// <summary>
	/// Test <seealso cref="HullWhiteIborFutureProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class HullWhiteIborFutureProductPricerTest
	public class HullWhiteIborFutureProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VALUATION = LocalDate.of(2011, 5, 12);
	  private static readonly HullWhiteOneFactorPiecewiseConstantParametersProvider HW_PROVIDER = HullWhiteIborFutureDataSet.createHullWhiteProvider(VALUATION);
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = HullWhiteIborFutureDataSet.createRatesProvider(VALUATION);
	  private static readonly ResolvedIborFuture FUTURE = HullWhiteIborFutureDataSet.IBOR_FUTURE.resolve(REF_DATA);

	  private const double TOL = 1.0e-13;
	  private const double TOL_FD = 1.0e-6;
	  private static readonly HullWhiteIborFutureProductPricer PRICER = HullWhiteIborFutureProductPricer.DEFAULT;
	  private static readonly DiscountingIborFutureProductPricer PRICER_DSC = DiscountingIborFutureProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(TOL_FD);

	  public virtual void test_price()
	  {
		double computed = PRICER.price(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		LocalDate start = FUTURE.IborRate.Observation.EffectiveDate;
		LocalDate end = FUTURE.IborRate.Observation.MaturityDate;
		double fixingYearFraction = FUTURE.IborRate.Observation.YearFraction;
		double convexity = HW_PROVIDER.futuresConvexityFactor(FUTURE.LastTradeDate, start, end);
		double forward = RATE_PROVIDER.iborIndexRates(EUR_EURIBOR_3M).rate(FUTURE.IborRate.Observation);
		double expected = 1d - convexity * forward + (1d - convexity) / fixingYearFraction;
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_parRate()
	  {
		double computed = PRICER.parRate(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		double price = PRICER.price(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		assertEquals(computed, 1d - price, TOL);
	  }

	  public virtual void test_convexityAdjustment()
	  {
		double computed = PRICER.convexityAdjustment(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		double priceHw = PRICER.price(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		double priceDsc = PRICER_DSC.price(FUTURE, RATE_PROVIDER); // no convexity adjustment
		assertEquals(priceDsc + computed, priceHw, TOL);
	  }

	  public virtual void test_priceSensitivity()
	  {
		PointSensitivities point = PRICER.priceSensitivityRates(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATE_PROVIDER, (p) => CurrencyAmount.of(EUR, PRICER.price(FUTURE, (p), HW_PROVIDER)));
		assertTrue(computed.equalWithTolerance(expected, TOL_FD));
	  }

	  public virtual void test_priceSensitivityHullWhiteParameter()
	  {
		DoubleArray computed = PRICER.priceSensitivityModelParamsHullWhite(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		DoubleArray vols = HW_PROVIDER.Parameters.Volatility;
		int size = vols.size();
		double[] expected = new double[size];
		for (int i = 0; i < size; ++i)
		{
		  double[] volsUp = vols.toArray();
		  double[] volsDw = vols.toArray();
		  volsUp[i] += TOL_FD;
		  volsDw[i] -= TOL_FD;
		  HullWhiteOneFactorPiecewiseConstantParameters paramsUp = HullWhiteOneFactorPiecewiseConstantParameters.of(HW_PROVIDER.Parameters.MeanReversion, DoubleArray.copyOf(volsUp), HW_PROVIDER.Parameters.VolatilityTime.subArray(1, size));
		  HullWhiteOneFactorPiecewiseConstantParameters paramsDw = HullWhiteOneFactorPiecewiseConstantParameters.of(HW_PROVIDER.Parameters.MeanReversion, DoubleArray.copyOf(volsDw), HW_PROVIDER.Parameters.VolatilityTime.subArray(1, size));
		  HullWhiteOneFactorPiecewiseConstantParametersProvider provUp = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(paramsUp, HW_PROVIDER.DayCount, HW_PROVIDER.ValuationDateTime);
		  HullWhiteOneFactorPiecewiseConstantParametersProvider provDw = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(paramsDw, HW_PROVIDER.DayCount, HW_PROVIDER.ValuationDateTime);
		  double priceUp = PRICER.price(FUTURE, RATE_PROVIDER, provUp);
		  double priceDw = PRICER.price(FUTURE, RATE_PROVIDER, provDw);
		  expected[i] = 0.5 * (priceUp - priceDw) / TOL_FD;
		}
		assertTrue(DoubleArrayMath.fuzzyEquals(computed.toArray(), expected, TOL_FD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void regression_value()
	  {
		double price = PRICER.price(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		assertEquals(price, 0.9802338355115904, TOL);
		double parRate = PRICER.parRate(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		assertEquals(parRate, 0.01976616448840962, TOL);
		double adjustment = PRICER.convexityAdjustment(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		assertEquals(adjustment, -1.3766374738599652E-4, TOL);
	  }

	  public virtual void regression_sensitivity()
	  {
		PointSensitivities point = PRICER.priceSensitivityRates(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point);
		double[] expected = new double[] {0.0, 0.0, 0.9514709785770106, -1.9399920741192112, 0.0, 0.0, 0.0, 0.0};
		assertEquals(computed.size(), 1);
		assertTrue(DoubleArrayMath.fuzzyEquals(computed.getSensitivity(HullWhiteIborFutureDataSet.FWD3_NAME, EUR).Sensitivity.toArray(), expected, TOL));
	  }
	}

}