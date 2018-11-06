using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.dsf
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using ResolvedDsf = com.opengamma.strata.product.dsf.ResolvedDsf;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// Test <seealso cref="DiscountingDsfProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingDsfProductPricerTest
	public class DiscountingDsfProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // curves
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2012, 9, 20);
	  private static readonly DoubleArray USD_DSC_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  private static readonly DoubleArray USD_DSC_RATE = DoubleArray.of(0.0100, 0.0120, 0.0120, 0.0140, 0.0140, 0.0140);
	  private static readonly CurveName USD_DSC_NAME = CurveName.of("USD Dsc");
	  private static readonly CurveMetadata USD_DSC_METADATA = Curves.zeroRates(USD_DSC_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve USD_DSC = InterpolatedNodalCurve.of(USD_DSC_METADATA, USD_DSC_TIME, USD_DSC_RATE, INTERPOLATOR);
	  private static readonly DoubleArray USD_FWD3_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  private static readonly DoubleArray USD_FWD3_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0150, 0.0150);
	  private static readonly CurveName USD_FWD3_NAME = CurveName.of("USD LIBOR 3M");
	  private static readonly CurveMetadata USD_FWD3_METADATA = Curves.zeroRates(USD_FWD3_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve USD_FWD3 = InterpolatedNodalCurve.of(USD_FWD3_METADATA, USD_FWD3_TIME, USD_FWD3_RATE, INTERPOLATOR);
	  private static readonly ImmutableRatesProvider PROVIDER = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(USD, USD_DSC).iborIndexCurve(USD_LIBOR_3M, USD_FWD3).build();
	  // underlying swap
	  private static readonly NotionalSchedule UNIT_NOTIONAL = NotionalSchedule.of(USD, 1d);
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly BusinessDayAdjustment BDA_MF = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, CALENDAR);
	  private static readonly BusinessDayAdjustment BDA_P = BusinessDayAdjustment.of(PRECEDING, CALENDAR);
	  private static readonly LocalDate START = LocalDate.of(2012, 12, 19);
	  private static readonly LocalDate END = START.plusYears(10);
	  private const double RATE = 0.0175;
	  private static readonly SwapLeg FIXED_LEG = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(START).endDate(END).frequency(P6M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_FINAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P6M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(UNIT_NOTIONAL).calculation(FixedRateCalculation.builder().dayCount(THIRTY_U_360).rate(ValueSchedule.of(RATE)).build()).build();
	  private static readonly SwapLeg IBOR_LEG = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(START).endDate(END).frequency(P3M).businessDayAdjustment(BDA_MF).stubConvention(StubConvention.SHORT_FINAL).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(UNIT_NOTIONAL).calculation(IborRateCalculation.builder().index(USD_LIBOR_3M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CALENDAR, BDA_P)).build()).build();
	  private static readonly Swap SWAP = Swap.of(FIXED_LEG, IBOR_LEG);
	  private static readonly ResolvedSwap RSWAP = SWAP.resolve(REF_DATA);

	  // deliverable swap future
	  private static readonly LocalDate LAST_TRADE = LocalDate.of(2012, 12, 17);
	  private static readonly LocalDate DELIVERY = LocalDate.of(2012, 12, 19);
	  private const double NOTIONAL = 100000;
	  private static readonly ResolvedDsf FUTURE = ResolvedDsf.builder().securityId(SecurityId.of("OG-Test", "DSF")).deliveryDate(DELIVERY).lastTradeDate(LAST_TRADE).notional(NOTIONAL).underlyingSwap(SWAP.resolve(REF_DATA)).build();
	  // calculators
	  private const double TOL = 1.0e-13;
	  private const double EPS = 1.0e-6;
	  private static readonly DiscountingDsfProductPricer PRICER = DiscountingDsfProductPricer.DEFAULT;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator FD_CAL = new RatesFiniteDifferenceSensitivityCalculator(EPS);

	  //-------------------------------------------------------------------------
	  public virtual void test_price()
	  {
		double computed = PRICER.price(FUTURE, PROVIDER);
		double pvSwap = PRICER.SwapPricer.presentValue(RSWAP, PROVIDER).getAmount(USD).Amount;
		double yc = ACT_ACT_ISDA.relativeYearFraction(VAL_DATE, DELIVERY);
		double df = Math.Exp(-USD_DSC.yValue(yc) * yc);
		double expected = 1d + pvSwap / df;
		assertEquals(computed, expected, TOL);
	  }

	  public virtual void test_priceSensitivity()
	  {
		PointSensitivities point = PRICER.priceSensitivity(FUTURE, PROVIDER);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = FD_CAL.sensitivity(PROVIDER, (p) => CurrencyAmount.of(USD, PRICER.price(FUTURE, (p))));
		assertTrue(computed.equalWithTolerance(expected, 10d * EPS));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void regression()
	  {
		double price = PRICER.price(FUTURE, PROVIDER);
		assertEquals(price, 1.022245377054993, TOL); // 2.x
	  }

	}

}