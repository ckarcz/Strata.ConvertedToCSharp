/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
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

	/// <summary>
	/// Test <seealso cref="ResolvedTermDeposit"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedTermDepositTest
	public class ResolvedTermDepositTest
	{

	  private static readonly LocalDate START_DATE = LocalDate.of(2015, 1, 19);
	  private static readonly LocalDate END_DATE = LocalDate.of(2015, 7, 20);
	  private static readonly double YEAR_FRACTION = ACT_365F.yearFraction(START_DATE, END_DATE);
	  private const double PRINCIPAL = 100000000d;
	  private const double RATE = 0.0250;
	  private const double EPS = 1.0e-14;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedTermDeposit test = ResolvedTermDeposit.builder().currency(GBP).notional(PRINCIPAL).startDate(START_DATE).endDate(END_DATE).yearFraction(YEAR_FRACTION).rate(RATE).build();
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, PRINCIPAL);
		assertEquals(test.StartDate, START_DATE);
		assertEquals(test.EndDate, END_DATE);
		assertEquals(test.YearFraction, YEAR_FRACTION);
		assertEquals(test.Rate, RATE);
		assertEquals(test.Interest, RATE * YEAR_FRACTION * PRINCIPAL, PRINCIPAL * EPS);
	  }

	  public virtual void test_builder_wrongDates()
	  {
		assertThrowsIllegalArg(() => ResolvedTermDeposit.builder().currency(GBP).notional(PRINCIPAL).startDate(START_DATE).endDate(LocalDate.of(2013, 1, 22)).yearFraction(YEAR_FRACTION).rate(RATE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedTermDeposit test1 = ResolvedTermDeposit.builder().currency(GBP).notional(PRINCIPAL).startDate(START_DATE).endDate(END_DATE).yearFraction(YEAR_FRACTION).rate(RATE).build();
		coverImmutableBean(test1);
		ResolvedTermDeposit test2 = ResolvedTermDeposit.builder().currency(GBP).notional(-50000000).startDate(START_DATE).endDate(END_DATE).yearFraction(YEAR_FRACTION).rate(0.0145).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedTermDeposit test = ResolvedTermDeposit.builder().currency(GBP).notional(PRINCIPAL).startDate(START_DATE).endDate(END_DATE).yearFraction(YEAR_FRACTION).rate(RATE).build();
		assertSerialization(test);
	  }

	}

}