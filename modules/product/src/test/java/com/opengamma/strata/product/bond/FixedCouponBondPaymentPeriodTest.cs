/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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

	/// <summary>
	/// Test <seealso cref="FixedCouponBondPaymentPeriod"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedCouponBondPaymentPeriodTest
	public class FixedCouponBondPaymentPeriodTest
	{

	  private static readonly LocalDate START = LocalDate.of(2015, 2, 2);
	  private static readonly LocalDate END = LocalDate.of(2015, 8, 2);
	  private static readonly LocalDate START_ADJUSTED = LocalDate.of(2015, 2, 2);
	  private static readonly LocalDate END_ADJUSTED = LocalDate.of(2015, 8, 3);
	  private static readonly LocalDate DETACHMENT_DATE = LocalDate.of(2015, 7, 27);
	  private const double FIXED_RATE = 0.025;
	  private const double NOTIONAL = 1.0e7;
	  private const double YEAR_FRACTION = 0.5;

	  public virtual void test_of()
	  {
		FixedCouponBondPaymentPeriod test = FixedCouponBondPaymentPeriod.builder().currency(USD).startDate(START_ADJUSTED).unadjustedStartDate(START).endDate(END_ADJUSTED).unadjustedEndDate(END).detachmentDate(DETACHMENT_DATE).notional(NOTIONAL).fixedRate(FIXED_RATE).yearFraction(YEAR_FRACTION).build();
		assertEquals(test.Currency, USD);
		assertEquals(test.UnadjustedStartDate, START);
		assertEquals(test.StartDate, START_ADJUSTED);
		assertEquals(test.UnadjustedEndDate, END);
		assertEquals(test.EndDate, END_ADJUSTED);
		assertEquals(test.PaymentDate, END_ADJUSTED);
		assertEquals(test.DetachmentDate, DETACHMENT_DATE);
		assertEquals(test.FixedRate, FIXED_RATE);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.YearFraction, YEAR_FRACTION);
		assertEquals(test.hasExCouponPeriod(), true);

		// the object is not changed
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(2))), test);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(test.Currency, USD);
		assertEquals(test.UnadjustedStartDate, START);
		assertEquals(test.StartDate, START_ADJUSTED);
		assertEquals(test.UnadjustedEndDate, END);
		assertEquals(test.EndDate, END_ADJUSTED);
		assertEquals(test.PaymentDate, END_ADJUSTED);
		assertEquals(test.DetachmentDate, DETACHMENT_DATE);
		assertEquals(test.FixedRate, FIXED_RATE);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.YearFraction, YEAR_FRACTION);
		assertEquals(test.hasExCouponPeriod(), true);
	  }

	  public virtual void test_of_noExCoupon()
	  {
		FixedCouponBondPaymentPeriod test = FixedCouponBondPaymentPeriod.builder().currency(USD).startDate(START_ADJUSTED).unadjustedStartDate(START).endDate(END_ADJUSTED).unadjustedEndDate(END).detachmentDate(END_ADJUSTED).notional(NOTIONAL).fixedRate(FIXED_RATE).yearFraction(YEAR_FRACTION).build();
		assertEquals(test.hasExCouponPeriod(), false);
	  }

	  public virtual void test_of_wrongDates()
	  {
		assertThrowsIllegalArg(() => FixedCouponBondPaymentPeriod.builder().currency(USD).startDate(START_ADJUSTED).unadjustedStartDate(START).endDate(LocalDate.of(2015, 2, 3)).unadjustedEndDate(LocalDate.of(2015, 2, 2)).notional(NOTIONAL).fixedRate(FIXED_RATE).yearFraction(YEAR_FRACTION).build());
		assertThrowsIllegalArg(() => FixedCouponBondPaymentPeriod.builder().currency(USD).startDate(LocalDate.of(2015, 8, 3)).unadjustedStartDate(LocalDate.of(2015, 8, 2)).endDate(LocalDate.of(2015, 8, 3)).unadjustedEndDate(LocalDate.of(2015, 8, 3)).notional(NOTIONAL).fixedRate(FIXED_RATE).yearFraction(YEAR_FRACTION).build());
		assertThrowsIllegalArg(() => FixedCouponBondPaymentPeriod.builder().currency(USD).startDate(START_ADJUSTED).unadjustedStartDate(START).endDate(END_ADJUSTED).unadjustedEndDate(END).detachmentDate(LocalDate.of(2015, 8, 6)).notional(NOTIONAL).fixedRate(FIXED_RATE).yearFraction(YEAR_FRACTION).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_contains()
	  {
		FixedCouponBondPaymentPeriod test = FixedCouponBondPaymentPeriod.builder().currency(USD).startDate(START_ADJUSTED).unadjustedStartDate(START).endDate(END_ADJUSTED).unadjustedEndDate(END).detachmentDate(DETACHMENT_DATE).notional(NOTIONAL).fixedRate(FIXED_RATE).yearFraction(YEAR_FRACTION).build();
		assertEquals(test.contains(START.minusDays(1)), false);
		assertEquals(test.contains(START), true);
		assertEquals(test.contains(START.plusDays(1)), true);
		assertEquals(test.contains(END.minusDays(1)), true);
		assertEquals(test.contains(END), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FixedCouponBondPaymentPeriod test1 = FixedCouponBondPaymentPeriod.builder().currency(USD).startDate(START_ADJUSTED).unadjustedStartDate(START).endDate(END_ADJUSTED).unadjustedEndDate(END).detachmentDate(DETACHMENT_DATE).notional(NOTIONAL).fixedRate(FIXED_RATE).yearFraction(YEAR_FRACTION).build();
		coverImmutableBean(test1);
		FixedCouponBondPaymentPeriod test2 = FixedCouponBondPaymentPeriod.builder().currency(GBP).startDate(LocalDate.of(2014, 3, 4)).unadjustedStartDate(LocalDate.of(2014, 3, 2)).endDate(LocalDate.of(2015, 3, 4)).unadjustedEndDate(LocalDate.of(2015, 3, 3)).notional(1.0e8).fixedRate(0.005).yearFraction(1d).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		FixedCouponBondPaymentPeriod test = FixedCouponBondPaymentPeriod.builder().currency(USD).startDate(START_ADJUSTED).unadjustedStartDate(START).endDate(END_ADJUSTED).unadjustedEndDate(END).detachmentDate(DETACHMENT_DATE).notional(NOTIONAL).fixedRate(FIXED_RATE).yearFraction(YEAR_FRACTION).build();
		assertSerialization(test);
	  }

	}

}