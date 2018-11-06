/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_RPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.US_CPI_U;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Index = com.opengamma.strata.basics.index.Index;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using InflationEndInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationEndInterpolatedRateComputation;
	using InflationEndMonthRateComputation = com.opengamma.strata.product.rate.InflationEndMonthRateComputation;

	/// <summary>
	/// Test <seealso cref="CapitalIndexedBondPaymentPeriod"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CapitalIndexedBondPaymentPeriodTest
	public class CapitalIndexedBondPaymentPeriodTest
	{

	  private static readonly LocalDate START_UNADJ = LocalDate.of(2008, 1, 13);
	  private static readonly LocalDate END_UNADJ = LocalDate.of(2008, 7, 13);
	  private static readonly LocalDate START = LocalDate.of(2008, 1, 14);
	  private static readonly LocalDate END = LocalDate.of(2008, 7, 14);
	  private static readonly YearMonth REF_END = YearMonth.of(2008, 4);
	  private const double NOTIONAL = 10_000_000d;
	  private const double REAL_COUPON = 0.01d;
	  private static readonly LocalDate DETACHMENT = LocalDate.of(2008, 1, 11);
	  private const double START_INDEX = 198.475;
	  private static readonly InflationEndInterpolatedRateComputation COMPUTE_INTERP = InflationEndInterpolatedRateComputation.of(US_CPI_U, START_INDEX, REF_END, 0.25);
	  private static readonly InflationEndMonthRateComputation COMPUTE_MONTH = InflationEndMonthRateComputation.of(US_CPI_U, START_INDEX, REF_END);

	  public virtual void test_builder_full()
	  {
		CapitalIndexedBondPaymentPeriod test = CapitalIndexedBondPaymentPeriod.builder().currency(USD).notional(NOTIONAL).detachmentDate(DETACHMENT).startDate(START).endDate(END).unadjustedStartDate(START_UNADJ).unadjustedEndDate(END_UNADJ).rateComputation(COMPUTE_INTERP).realCoupon(REAL_COUPON).build();
		assertEquals(test.Currency, USD);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.DetachmentDate, DETACHMENT);
		assertEquals(test.StartDate, START);
		assertEquals(test.EndDate, END);
		assertEquals(test.UnadjustedStartDate, START_UNADJ);
		assertEquals(test.UnadjustedEndDate, END_UNADJ);
		assertEquals(test.RateComputation, COMPUTE_INTERP);
		assertEquals(test.RealCoupon, REAL_COUPON);
	  }

	  public virtual void test_builder_min()
	  {
		CapitalIndexedBondPaymentPeriod test = CapitalIndexedBondPaymentPeriod.builder().currency(USD).notional(NOTIONAL).startDate(START).endDate(END).rateComputation(COMPUTE_MONTH).realCoupon(REAL_COUPON).build();
		assertEquals(test.Currency, USD);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.DetachmentDate, END);
		assertEquals(test.StartDate, START);
		assertEquals(test.EndDate, END);
		assertEquals(test.UnadjustedStartDate, START);
		assertEquals(test.UnadjustedEndDate, END);
		assertEquals(test.RateComputation, COMPUTE_MONTH);
		assertEquals(test.RealCoupon, REAL_COUPON);
	  }

	  public virtual void test_builder_fail()
	  {
		// not inflation rate observation
		FixedRateComputation fixedRate = FixedRateComputation.of(0.01);
		assertThrowsIllegalArg(() => CapitalIndexedBondPaymentPeriod.builder().currency(USD).notional(NOTIONAL).detachmentDate(DETACHMENT).startDate(START).endDate(END).unadjustedStartDate(START_UNADJ).unadjustedEndDate(END_UNADJ).rateComputation(fixedRate).realCoupon(REAL_COUPON).build());
		// wrong start date and end date
		assertThrowsIllegalArg(() => CapitalIndexedBondPaymentPeriod.builder().currency(USD).notional(NOTIONAL).detachmentDate(DETACHMENT).startDate(END.plusDays(1)).endDate(END).unadjustedStartDate(START_UNADJ).unadjustedEndDate(END_UNADJ).rateComputation(COMPUTE_INTERP).realCoupon(REAL_COUPON).build());
		// wrong unadjusted start date and unadjusted end date
		assertThrowsIllegalArg(() => CapitalIndexedBondPaymentPeriod.builder().currency(USD).notional(NOTIONAL).detachmentDate(DETACHMENT).startDate(START).endDate(END).unadjustedStartDate(START_UNADJ).unadjustedEndDate(START_UNADJ.minusWeeks(1)).rateComputation(COMPUTE_INTERP).realCoupon(REAL_COUPON).build());
	  }

	  public virtual void test_methods()
	  {
		CapitalIndexedBondPaymentPeriod test = CapitalIndexedBondPaymentPeriod.builder().currency(USD).notional(NOTIONAL).detachmentDate(DETACHMENT).startDate(START).endDate(END).unadjustedStartDate(START_UNADJ).unadjustedEndDate(END_UNADJ).rateComputation(COMPUTE_INTERP).realCoupon(REAL_COUPON).build();
		assertEquals(test.PaymentDate, END);
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(2))), test);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		ImmutableSet<Index> set = builder.build();
		assertEquals(set.size(), 1);
		assertEquals(set.asList().get(0), US_CPI_U);

		LocalDate bondStart = LocalDate.of(2003, 1, 13);
		LocalDate bondStartUnadj = LocalDate.of(2003, 1, 12);
		CapitalIndexedBondPaymentPeriod expected = CapitalIndexedBondPaymentPeriod.builder().currency(USD).notional(NOTIONAL).detachmentDate(END).startDate(bondStart).endDate(END).unadjustedStartDate(bondStartUnadj).unadjustedEndDate(END_UNADJ).rateComputation(COMPUTE_INTERP).realCoupon(1d).build();
		assertEquals(test.withUnitCoupon(bondStart, bondStartUnadj), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CapitalIndexedBondPaymentPeriod test1 = CapitalIndexedBondPaymentPeriod.builder().currency(USD).notional(NOTIONAL).detachmentDate(DETACHMENT).startDate(START).endDate(END).unadjustedStartDate(START_UNADJ).unadjustedEndDate(END_UNADJ).rateComputation(COMPUTE_INTERP).realCoupon(REAL_COUPON).build();
		coverImmutableBean(test1);
		CapitalIndexedBondPaymentPeriod test2 = CapitalIndexedBondPaymentPeriod.builder().currency(GBP).notional(5.0e6).startDate(LocalDate.of(2008, 1, 15)).endDate(LocalDate.of(2008, 7, 15)).rateComputation(InflationEndMonthRateComputation.of(GB_RPI, 155.32, REF_END)).realCoupon(1d).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		CapitalIndexedBondPaymentPeriod test = CapitalIndexedBondPaymentPeriod.builder().currency(USD).notional(NOTIONAL).detachmentDate(DETACHMENT).startDate(START).endDate(END).unadjustedStartDate(START_UNADJ).unadjustedEndDate(END_UNADJ).rateComputation(COMPUTE_INTERP).realCoupon(REAL_COUPON).build();
		assertSerialization(test);
	  }

	}

}