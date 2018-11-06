/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.GBP_USD_WM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.FIXED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.IBOR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatePeriodSwapLegTest
	public class RatePeriodSwapLegTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_2014_06_28 = date(2014, 6, 28);
	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);
	  private static readonly LocalDate DATE_2014_09_28 = date(2014, 9, 28);
	  private static readonly LocalDate DATE_2014_09_30 = date(2014, 9, 30);
	  private static readonly LocalDate DATE_2014_10_01 = date(2014, 10, 1);
	  private static readonly LocalDate DATE_2014_12_30 = date(2014, 12, 30);
	  private static readonly LocalDate DATE_2014_01_02 = date(2014, 1, 2);
	  private static readonly IborRateComputation GBPLIBOR3M_2014_06_28 = IborRateComputation.of(GBP_LIBOR_3M, DATE_2014_06_28, REF_DATA);
	  private static readonly IborRateComputation GBPLIBOR3M_2014_09_28 = IborRateComputation.of(GBP_LIBOR_3M, DATE_2014_09_28, REF_DATA);
	  private static readonly NotionalExchange NOTIONAL_EXCHANGE = NotionalExchange.of(CurrencyAmount.of(GBP, 2000d), DATE_2014_10_01);
	  private static readonly RateAccrualPeriod RAP1 = RateAccrualPeriod.builder().startDate(DATE_2014_06_30).endDate(DATE_2014_09_30).yearFraction(0.25d).rateComputation(GBPLIBOR3M_2014_06_28).build();
	  private static readonly RateAccrualPeriod RAP2 = RateAccrualPeriod.builder().startDate(DATE_2014_09_30).endDate(DATE_2014_12_30).yearFraction(0.25d).rateComputation(GBPLIBOR3M_2014_09_28).build();
	  private static readonly RatePaymentPeriod RPP1 = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1).dayCount(ACT_365F).currency(GBP).notional(5000d).build();
	  private static readonly RatePaymentPeriod RPP1_FXRESET = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1).dayCount(ACT_365F).currency(GBP).fxReset(FxReset.of(FxIndexObservation.of(GBP_USD_WM, DATE_2014_06_28, REF_DATA), USD)).notional(8000d).build();
	  private static readonly RatePaymentPeriod RPP2 = RatePaymentPeriod.builder().paymentDate(DATE_2014_01_02).accrualPeriods(RAP2).dayCount(ACT_365F).currency(GBP).notional(6000d).build();
	  private static readonly RatePaymentPeriod RPP3 = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1).dayCount(ACT_365F).currency(USD).notional(6000d).build();
	  private static readonly BusinessDayAdjustment FOLLOWING_GBLO = BusinessDayAdjustment.of(FOLLOWING, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).initialExchange(true).intermediateExchange(true).finalExchange(true).paymentEvents(NOTIONAL_EXCHANGE).paymentBusinessDayAdjustment(FOLLOWING_GBLO).build();
		assertEquals(test.Type, IBOR);
		assertEquals(test.PayReceive, RECEIVE);
		assertEquals(test.StartDate, AdjustableDate.of(DATE_2014_06_30));
		assertEquals(test.EndDate, AdjustableDate.of(DATE_2014_09_30));
		assertEquals(test.Currency, GBP);
		assertEquals(test.PaymentPeriods, ImmutableList.of(RPP1));
		assertEquals(test.PaymentEvents, ImmutableList.of(NOTIONAL_EXCHANGE));
		assertEquals(test.InitialExchange, true);
		assertEquals(test.IntermediateExchange, true);
		assertEquals(test.FinalExchange, true);
		assertEquals(test.PaymentBusinessDayAdjustment, FOLLOWING_GBLO);
	  }

	  public virtual void test_builder_defaults()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).build();
		assertEquals(test.PayReceive, RECEIVE);
		assertEquals(test.StartDate, AdjustableDate.of(DATE_2014_06_30));
		assertEquals(test.EndDate, AdjustableDate.of(DATE_2014_09_30));
		assertEquals(test.Currency, GBP);
		assertEquals(test.PaymentPeriods, ImmutableList.of(RPP1));
		assertEquals(test.PaymentEvents, ImmutableList.of());
		assertEquals(test.InitialExchange, false);
		assertEquals(test.IntermediateExchange, false);
		assertEquals(test.FinalExchange, false);
		assertEquals(test.PaymentBusinessDayAdjustment, BusinessDayAdjustment.NONE);
	  }

	  public virtual void test_builder_invalidMixedCurrency()
	  {
		assertThrowsIllegalArg(() => RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP3).paymentEvents(NOTIONAL_EXCHANGE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_3M));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP));
	  }

	  public virtual void test_collectIndices_fxReset()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1_FXRESET).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_3M, GBP_USD_WM));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP, USD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NOTIONAL_EXCHANGE).build();
		ResolvedSwapLeg expected = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NOTIONAL_EXCHANGE).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_createNotionalExchange()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).initialExchange(true).intermediateExchange(true).finalExchange(true).build();
		ResolvedSwapLeg expected = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NotionalExchange.of(CurrencyAmount.of(GBP, -5000d), DATE_2014_06_30), NotionalExchange.of(CurrencyAmount.of(GBP, 5000d), DATE_2014_10_01)).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_fxResetNotionalExchange()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1_FXRESET, RPP2).initialExchange(true).intermediateExchange(true).finalExchange(true).build();
		FxResetNotionalExchange ne1a = FxResetNotionalExchange.of(CurrencyAmount.of(USD, -8000d), DATE_2014_06_30, FxIndexObservation.of(GBP_USD_WM, DATE_2014_06_28, REF_DATA));
		FxResetNotionalExchange ne1b = FxResetNotionalExchange.of(CurrencyAmount.of(USD, 8000d), DATE_2014_10_01, FxIndexObservation.of(GBP_USD_WM, DATE_2014_06_28, REF_DATA));
		NotionalExchange ne2a = NotionalExchange.of(CurrencyAmount.of(GBP, -6000d), DATE_2014_10_01);
		NotionalExchange ne2b = NotionalExchange.of(CurrencyAmount.of(GBP, 6000d), DATE_2014_01_02);
		ResolvedSwapLeg expected = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1_FXRESET, RPP2).paymentEvents(ne1a, ne1b, ne2a, ne2b).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_FxResetOmitIntermediateNotionalExchange()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1_FXRESET).initialExchange(true).intermediateExchange(false).finalExchange(true).build();

		FxResetNotionalExchange initialExchange = FxResetNotionalExchange.of(CurrencyAmount.of(USD, -8000d), DATE_2014_06_30, FxIndexObservation.of(GBP_USD_WM, DATE_2014_06_28, REF_DATA));
		FxResetNotionalExchange finalExchange = FxResetNotionalExchange.of(CurrencyAmount.of(USD, 8000d), DATE_2014_10_01, FxIndexObservation.of(GBP_USD_WM, DATE_2014_06_28, REF_DATA));

		ResolvedSwapLeg expected = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1_FXRESET).paymentEvents(initialExchange, finalExchange).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_FxResetOmitInitialNotionalExchange()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(PAY).paymentPeriods(RPP1_FXRESET).initialExchange(false).intermediateExchange(true).finalExchange(true).build();

		FxResetNotionalExchange finalExchange = FxResetNotionalExchange.of(CurrencyAmount.of(USD, 8000d), DATE_2014_10_01, FxIndexObservation.of(GBP_USD_WM, DATE_2014_06_28, REF_DATA));

		ResolvedSwapLeg expected = ResolvedSwapLeg.builder().type(IBOR).payReceive(PAY).paymentPeriods(RPP1_FXRESET).paymentEvents(finalExchange).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_createNotionalExchange_noInitial()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).initialExchange(false).intermediateExchange(true).finalExchange(true).build();
		ResolvedSwapLeg expected = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NotionalExchange.of(CurrencyAmount.of(GBP, 5000d), DATE_2014_10_01)).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_createNotionalExchange_initialOnly()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).initialExchange(true).intermediateExchange(false).finalExchange(false).build();
		ResolvedSwapLeg expected = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NotionalExchange.of(CurrencyAmount.of(GBP, -5000d), DATE_2014_06_30)).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_createNotionalExchange_finalOnly()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).initialExchange(false).intermediateExchange(false).finalExchange(true).build();
		ResolvedSwapLeg expected = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NotionalExchange.of(CurrencyAmount.of(GBP, 5000d), DATE_2014_10_01)).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NOTIONAL_EXCHANGE).paymentBusinessDayAdjustment(FOLLOWING_GBLO).initialExchange(true).intermediateExchange(true).finalExchange(true).build();
		coverImmutableBean(test);
		RatePeriodSwapLeg test2 = RatePeriodSwapLeg.builder().type(FIXED).payReceive(PAY).paymentPeriods(RPP2).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		RatePeriodSwapLeg test = RatePeriodSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP1).paymentEvents(NOTIONAL_EXCHANGE).build();
		assertSerialization(test);
	  }

	}

}