/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DispatchingRateComputationFn = com.opengamma.strata.pricer.impl.rate.DispatchingRateComputationFn;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using OvernightFuture = com.opengamma.strata.product.index.OvernightFuture;
	using ResolvedOvernightFuture = com.opengamma.strata.product.index.ResolvedOvernightFuture;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;

	/// <summary>
	/// Test <seealso cref="DiscountingOvernightFutureProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingOvernightFutureProductPricerTest
	public class DiscountingOvernightFutureProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VALUATION = LocalDate.of(2018, 7, 12);
	  private const double NOTIONAL = 5_000_000d;
	  private static readonly double ACCRUAL_FACTOR = TENOR_1M.Period.toTotalMonths() / 12.0;
	  private static readonly LocalDate LAST_TRADE_DATE = date(2018, 9, 28);
	  private static readonly LocalDate START_DATE = date(2018, 9, 1);
	  private static readonly LocalDate END_DATE = date(2018, 9, 30);
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(5);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "OnFuture");
	  private static readonly ResolvedOvernightFuture FUTURE = OvernightFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).startDate(START_DATE).endDate(END_DATE).lastTradeDate(LAST_TRADE_DATE).index(USD_FED_FUND).accrualMethod(OvernightAccrualMethod.AVERAGED_DAILY).rounding(ROUNDING).build().resolve(REF_DATA);

	  private static readonly DoubleArray TIME = DoubleArray.of(0.02, 0.08, 0.25, 0.5);
	  private static readonly DoubleArray RATE = DoubleArray.of(0.01, 0.015, 0.008, 0.005);
	  private static readonly Curve CURVE = InterpolatedNodalCurve.of(Curves.zeroRates("FED-FUND", DayCounts.ACT_365F), TIME, RATE, CurveInterpolators.NATURAL_SPLINE);
	  private static readonly RatesProvider RATES_PROVIDER = ImmutableRatesProvider.builder(VALUATION).indexCurve(USD_FED_FUND, CURVE).build();

	  private const double TOL = 1.0e-14;
	  private const double EPS = 1.0e-6;
	  private static readonly DiscountingOvernightFutureProductPricer PRICER = DiscountingOvernightFutureProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CALC = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  //------------------------------------------------------------------------- 
	  public virtual void test_marginIndex()
	  {
		double notional = FUTURE.Notional;
		double accrualFactor = FUTURE.AccrualFactor;
		double price = 0.99;
		double marginIndexExpected = price * notional * accrualFactor;
		double marginIndexComputed = PRICER.marginIndex(FUTURE, price);
		assertEquals(marginIndexComputed, marginIndexExpected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_marginIndexSensitivity()
	  {
		double notional = FUTURE.Notional;
		double accrualFactor = FUTURE.AccrualFactor;
		PointSensitivities priceSensitivity = PRICER.priceSensitivity(FUTURE, RATES_PROVIDER);
		PointSensitivities sensiComputed = PRICER.marginIndexSensitivity(FUTURE, priceSensitivity);
		assertTrue(sensiComputed.equalWithTolerance(priceSensitivity.multipliedBy(accrualFactor * notional), TOL * notional));
	  }

	  //------------------------------------------------------------------------- 
	  public virtual void test_price()
	  {
		double computed = PRICER.price(FUTURE, RATES_PROVIDER);
		double rate = DispatchingRateComputationFn.DEFAULT.rate(FUTURE.OvernightRate, START_DATE, END_DATE, RATES_PROVIDER);
		double expected = 1d - rate;
		assertEquals(computed, expected, TOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_priceSensitivity()
	  {
		PointSensitivities points = PRICER.priceSensitivity(FUTURE, RATES_PROVIDER);
		CurrencyParameterSensitivities computed = RATES_PROVIDER.parameterSensitivity(points);
		CurrencyParameterSensitivities expected = FD_CALC.sensitivity(RATES_PROVIDER, r => CurrencyAmount.of(USD, PRICER.price(FUTURE, r)));
		assertTrue(computed.equalWithTolerance(expected, EPS));
	  }

	}

}