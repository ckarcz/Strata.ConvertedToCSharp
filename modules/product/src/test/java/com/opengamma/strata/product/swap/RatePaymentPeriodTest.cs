/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
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
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatePaymentPeriodTest
	public class RatePaymentPeriodTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_2014_03_30 = date(2014, 3, 30);
	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);
	  private static readonly LocalDate DATE_2014_09_30 = date(2014, 9, 30);
	  private static readonly LocalDate DATE_2014_10_01 = date(2014, 10, 1);
	  private static readonly IborRateComputation GBP_LIBOR_3M_2014_03_28 = IborRateComputation.of(GBP_LIBOR_3M, date(2014, 3, 28), REF_DATA);
	  private static readonly IborRateComputation GBP_LIBOR_3M_2014_06_28 = IborRateComputation.of(GBP_LIBOR_3M, date(2014, 6, 28), REF_DATA);
	  private static readonly FxReset FX_RESET_USD = FxReset.of(FxIndexObservation.of(GBP_USD_WM, date(2014, 3, 28), REF_DATA), USD);
	  private static readonly RateAccrualPeriod RAP1 = RateAccrualPeriod.builder().startDate(DATE_2014_03_30).endDate(DATE_2014_06_30).yearFraction(0.25d).rateComputation(GBP_LIBOR_3M_2014_03_28).build();
	  private static readonly RateAccrualPeriod RAP2 = RateAccrualPeriod.builder().startDate(DATE_2014_06_30).endDate(DATE_2014_09_30).yearFraction(0.25d).rateComputation(GBP_LIBOR_3M_2014_06_28).build();

	  public virtual void test_builder_oneAccrualPeriod()
	  {
		RatePaymentPeriod test = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP2).dayCount(ACT_365F).currency(GBP).notional(1000d).compoundingMethod(CompoundingMethod.STRAIGHT).build();
		assertEquals(test.StartDate, DATE_2014_06_30);
		assertEquals(test.EndDate, DATE_2014_09_30);
		assertEquals(test.PaymentDate, DATE_2014_10_01);
		assertEquals(test.AccrualPeriods, ImmutableList.of(RAP2));
		assertEquals(test.Currency, GBP);
		assertEquals(test.FxReset, null);
		assertEquals(test.Notional, 1000d, 0d);
		assertEquals(test.NotionalAmount, CurrencyAmount.of(GBP, 1000d));
		assertEquals(test.CompoundingMethod, CompoundingMethod.STRAIGHT);
		assertEquals(test.CompoundingApplicable, false);
	  }

	  public virtual void test_builder_twoAccrualPeriods()
	  {
		RatePaymentPeriod test = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1, RAP2).dayCount(ACT_365F).currency(GBP).notional(1000d).compoundingMethod(CompoundingMethod.STRAIGHT).build();
		assertEquals(test.StartDate, DATE_2014_03_30);
		assertEquals(test.EndDate, DATE_2014_09_30);
		assertEquals(test.PaymentDate, DATE_2014_10_01);
		assertEquals(test.AccrualPeriods, ImmutableList.of(RAP1, RAP2));
		assertEquals(test.Currency, GBP);
		assertEquals(test.FxReset, null);
		assertEquals(test.Notional, 1000d, 0d);
		assertEquals(test.CompoundingMethod, CompoundingMethod.STRAIGHT);
		assertEquals(test.CompoundingApplicable, true);
	  }

	  public virtual void test_builder_twoAccrualPeriods_compoundingDefaultedToNone_fxReset()
	  {
		RatePaymentPeriod test = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1, RAP2).dayCount(ACT_365F).currency(GBP).fxReset(FX_RESET_USD).notional(1000d).compoundingMethod(CompoundingMethod.NONE).build();
		assertEquals(test.StartDate, DATE_2014_03_30);
		assertEquals(test.EndDate, DATE_2014_09_30);
		assertEquals(test.PaymentDate, DATE_2014_10_01);
		assertEquals(test.AccrualPeriods, ImmutableList.of(RAP1, RAP2));
		assertEquals(test.Currency, GBP);
		assertEquals(test.FxReset, FX_RESET_USD);
		assertEquals(test.Notional, 1000d, 0d);
		assertEquals(test.NotionalAmount, CurrencyAmount.of(USD, 1000d));
		assertEquals(test.CompoundingApplicable, false);
	  }

	  public virtual void test_builder_badFxReset()
	  {
		assertThrowsIllegalArg(() => RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1, RAP2).dayCount(ACT_365F).currency(USD).fxReset(FX_RESET_USD).notional(1000d).compoundingMethod(CompoundingMethod.NONE).build());
		assertThrowsIllegalArg(() => RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1, RAP2).dayCount(ACT_365F).currency(EUR).fxReset(FX_RESET_USD).notional(1000d).compoundingMethod(CompoundingMethod.NONE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_adjustPaymentDate()
	  {
		RatePaymentPeriod test = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP2).dayCount(ACT_365F).currency(GBP).notional(1000d).compoundingMethod(CompoundingMethod.STRAIGHT).build();
		RatePaymentPeriod expected = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01.plusDays(2)).accrualPeriods(RAP2).dayCount(ACT_365F).currency(GBP).notional(1000d).compoundingMethod(CompoundingMethod.STRAIGHT).build();
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(0))), test);
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(2))), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices_simple()
	  {
		RatePaymentPeriod test = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP2).dayCount(ACT_365F).currency(GBP).notional(1000d).compoundingMethod(CompoundingMethod.STRAIGHT).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_3M));
	  }

	  public virtual void test_collectIndices_fxReset()
	  {
		RatePaymentPeriod test = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP2).dayCount(ACT_365F).currency(GBP).notional(1000d).fxReset(FX_RESET_USD).compoundingMethod(CompoundingMethod.STRAIGHT).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_3M, GBP_USD_WM));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RatePaymentPeriod test = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1, RAP2).dayCount(ACT_365F).currency(GBP).fxReset(FX_RESET_USD).notional(1000d).compoundingMethod(CompoundingMethod.STRAIGHT).build();
		coverImmutableBean(test);
		RatePaymentPeriod test2 = RatePaymentPeriod.builder().paymentDate(DATE_2014_09_30).accrualPeriods(RAP1).dayCount(ACT_360).currency(USD).notional(2000d).compoundingMethod(CompoundingMethod.NONE).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		RatePaymentPeriod test = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP1, RAP2).dayCount(ACT_365F).currency(GBP).notional(1000d).compoundingMethod(CompoundingMethod.STRAIGHT).build();
		assertSerialization(test);
	  }

	}

}