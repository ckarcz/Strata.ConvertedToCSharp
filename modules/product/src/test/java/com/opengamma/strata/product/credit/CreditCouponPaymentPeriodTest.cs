/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Test <seealso cref="CreditCouponPaymentPeriod"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CreditCouponPaymentPeriodTest
	public class CreditCouponPaymentPeriodTest
	{
	  private const double COUPON = 0.05;
	  private const double NOTIONAL = 1.0e9;
	  private static readonly LocalDate START_DATE = LocalDate.of(2013, 12, 20);
	  private static readonly LocalDate END_DATE = LocalDate.of(2014, 3, 20);
	  private static readonly LocalDate EFF_START_DATE = LocalDate.of(2013, 12, 19);
	  private static readonly LocalDate EFF_END_DATE = LocalDate.of(2014, 3, 19);
	  private const double YEAR_FRACTION = 0.256;

	  public virtual void test_builder()
	  {
		CreditCouponPaymentPeriod test = CreditCouponPaymentPeriod.builder().currency(USD).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).effectiveStartDate(EFF_START_DATE).effectiveEndDate(EFF_END_DATE).paymentDate(END_DATE).fixedRate(COUPON).yearFraction(YEAR_FRACTION).build();
		assertEquals(test.Currency, USD);
		assertEquals(test.EffectiveStartDate, EFF_START_DATE);
		assertEquals(test.EffectiveEndDate, EFF_END_DATE);
		assertEquals(test.StartDate, START_DATE);
		assertEquals(test.EndDate, END_DATE);
		assertEquals(test.FixedRate, COUPON);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.PaymentDate, END_DATE);
		assertEquals(test.YearFraction, YEAR_FRACTION);
	  }

	  public virtual void test_contains()
	  {
		CreditCouponPaymentPeriod test = CreditCouponPaymentPeriod.builder().currency(USD).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).effectiveStartDate(EFF_START_DATE).effectiveEndDate(EFF_END_DATE).paymentDate(END_DATE).fixedRate(COUPON).yearFraction(YEAR_FRACTION).build();
		assertTrue(test.contains(START_DATE));
		assertTrue(test.contains(START_DATE.plusMonths(1)));
		assertFalse(test.contains(END_DATE));
		assertFalse(test.contains(START_DATE.minusDays(1)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CreditCouponPaymentPeriod test1 = CreditCouponPaymentPeriod.builder().currency(USD).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).effectiveStartDate(EFF_START_DATE).effectiveEndDate(EFF_END_DATE).paymentDate(END_DATE).fixedRate(COUPON).yearFraction(YEAR_FRACTION).build();
		coverImmutableBean(test1);
		CreditCouponPaymentPeriod test2 = CreditCouponPaymentPeriod.builder().currency(Currency.JPY).notional(5.0e6).startDate(START_DATE.minusDays(7)).endDate(END_DATE.minusDays(7)).effectiveStartDate(EFF_START_DATE.minusDays(7)).effectiveEndDate(EFF_END_DATE.minusDays(7)).paymentDate(END_DATE.minusDays(7)).fixedRate(0.01).yearFraction(0.25).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		CreditCouponPaymentPeriod test = CreditCouponPaymentPeriod.builder().currency(USD).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).effectiveStartDate(EFF_START_DATE).effectiveEndDate(EFF_END_DATE).paymentDate(END_DATE).fixedRate(COUPON).yearFraction(YEAR_FRACTION).build();
		assertSerialization(test);
	  }

	}

}