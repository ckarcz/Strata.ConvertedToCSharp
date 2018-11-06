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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using HullWhiteOneFactorPiecewiseConstantParameters = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParameters;
	using HullWhiteOneFactorPiecewiseConstantParametersProvider = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParametersProvider;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using ResolvedIborFuture = com.opengamma.strata.product.index.ResolvedIborFuture;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;

	/// <summary>
	/// Test <seealso cref="HullWhiteIborFutureTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class HullWhiteIborFutureTradePricerTest
	public class HullWhiteIborFutureTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VALUATION = LocalDate.of(2011, 5, 12);
	  private static readonly HullWhiteOneFactorPiecewiseConstantParametersProvider HW_PROVIDER = HullWhiteIborFutureDataSet.createHullWhiteProvider(VALUATION);
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = HullWhiteIborFutureDataSet.createRatesProvider(VALUATION);
	  private static readonly ResolvedIborFutureTrade FUTURE_TRADE = HullWhiteIborFutureDataSet.IBOR_FUTURE_TRADE.resolve(REF_DATA);
	  private static readonly ResolvedIborFuture FUTURE = FUTURE_TRADE.Product;
	  private const double LAST_PRICE = HullWhiteIborFutureDataSet.LAST_MARGIN_PRICE;
	  private const double NOTIONAL = HullWhiteIborFutureDataSet.NOTIONAL;
	  private const long QUANTITY = HullWhiteIborFutureDataSet.QUANTITY;

	  private const double TOL = 1.0e-13;
	  private const double TOL_FD = 1.0e-6;
	  private static readonly HullWhiteIborFutureTradePricer PRICER = HullWhiteIborFutureTradePricer.DEFAULT;
	  private static readonly HullWhiteIborFutureProductPricer PRICER_PRODUCT = HullWhiteIborFutureProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(TOL_FD);

	  public virtual void test_price()
	  {
		double computed = PRICER.price(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER);
		double expected = PRICER_PRODUCT.price(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_presentValue()
	  {
		CurrencyAmount computed = PRICER.presentValue(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER, LAST_PRICE);
		double price = PRICER_PRODUCT.price(FUTURE, RATE_PROVIDER, HW_PROVIDER);
		double expected = (price - LAST_PRICE) * FUTURE.AccrualFactor * NOTIONAL * QUANTITY;
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, expected, TOL * NOTIONAL * QUANTITY);
	  }

	  public virtual void test_parSpread()
	  {
		double computed = PRICER.parSpread(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER, LAST_PRICE);
		CurrencyAmount pv = PRICER.presentValue(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER, LAST_PRICE + computed);
		assertEquals(pv.Amount, 0d, TOL * NOTIONAL * QUANTITY);
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityRates(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER);
		CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATE_PROVIDER, p => PRICER.presentValue(FUTURE_TRADE, p, HW_PROVIDER, LAST_PRICE));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * TOL_FD));
	  }

	  public virtual void test_presentValueSensitivityHullWhiteParameter()
	  {
		DoubleArray computed = PRICER.presentValueSensitivityModelParamsHullWhite(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER);
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
		  double priceUp = PRICER.presentValue(FUTURE_TRADE, RATE_PROVIDER, provUp, LAST_PRICE).Amount;
		  double priceDw = PRICER.presentValue(FUTURE_TRADE, RATE_PROVIDER, provDw, LAST_PRICE).Amount;
		  expected[i] = 0.5 * (priceUp - priceDw) / TOL_FD;
		}
		assertTrue(DoubleArrayMath.fuzzyEquals(computed.toArray(), expected, NOTIONAL * QUANTITY * TOL_FD));
	  }

	  public virtual void test_parSpreadSensitivity()
	  {
		PointSensitivities point = PRICER.parSpreadSensitivityRates(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER);
		CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(RATE_PROVIDER, p => CurrencyAmount.of(EUR, PRICER.parSpread(FUTURE_TRADE, p, HW_PROVIDER, LAST_PRICE)));
		assertTrue(computed.equalWithTolerance(expected, NOTIONAL * QUANTITY * TOL_FD));
	  }

	  public virtual void test_currencyExposure()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityRates(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER);
		MultiCurrencyAmount expected = RATE_PROVIDER.currencyExposure(point).plus(PRICER.presentValue(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER, LAST_PRICE));
		MultiCurrencyAmount computed = PRICER.currencyExposure(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER, LAST_PRICE);
		assertEquals(computed.size(), 1);
		assertEquals(computed.getAmount(EUR).Amount, expected.getAmount(EUR).Amount, NOTIONAL * QUANTITY * TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void regression_pv()
	  {
		CurrencyAmount pv = PRICER.presentValue(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER, LAST_PRICE);
		assertEquals(pv.Amount, 23383.551159035414, NOTIONAL * QUANTITY * TOL);
	  }

	  public virtual void regression_pvSensi()
	  {
		PointSensitivities point = PRICER.presentValueSensitivityRates(FUTURE_TRADE, RATE_PROVIDER, HW_PROVIDER);
		CurrencyParameterSensitivities computed = RATE_PROVIDER.parameterSensitivity(point);
		double[] expected = new double[] {0.0, 0.0, 9.514709785770103E7, -1.939992074119211E8, 0.0, 0.0, 0.0, 0.0};
		assertEquals(computed.size(), 1);
		assertTrue(DoubleArrayMath.fuzzyEquals(computed.getSensitivity(HullWhiteIborFutureDataSet.FWD3_NAME, EUR).Sensitivity.toArray(), expected, NOTIONAL * QUANTITY * TOL));
	  }
	}

}