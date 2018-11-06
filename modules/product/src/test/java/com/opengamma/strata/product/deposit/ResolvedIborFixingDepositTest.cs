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
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test <seealso cref="ResolvedTermDeposit"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedIborFixingDepositTest
	public class ResolvedIborFixingDepositTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate FIXING_DATE = LocalDate.of(2015, 1, 19);
	  private static readonly LocalDate START_DATE = LocalDate.of(2015, 1, 19);
	  private static readonly LocalDate END_DATE = LocalDate.of(2015, 7, 20);
	  private static readonly double YEAR_FRACTION = ACT_365F.yearFraction(START_DATE, END_DATE);
	  private static readonly IborRateComputation RATE_COMP = IborRateComputation.of(GBP_LIBOR_6M, FIXING_DATE, REF_DATA);
	  private const double NOTIONAL = 100000000d;
	  private const double RATE = 0.0250;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedIborFixingDeposit test = ResolvedIborFixingDeposit.builder().currency(GBP).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).yearFraction(YEAR_FRACTION).floatingRate(RATE_COMP).fixedRate(RATE).build();
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.StartDate, START_DATE);
		assertEquals(test.EndDate, END_DATE);
		assertEquals(test.YearFraction, YEAR_FRACTION);
		assertEquals(test.FloatingRate, RATE_COMP);
		assertEquals(test.FixedRate, RATE);
	  }

	  public virtual void test_builder_wrongDates()
	  {
		assertThrowsIllegalArg(() => ResolvedIborFixingDeposit.builder().currency(GBP).notional(NOTIONAL).startDate(LocalDate.of(2015, 8, 20)).endDate(END_DATE).yearFraction(YEAR_FRACTION).floatingRate(RATE_COMP).fixedRate(RATE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedIborFixingDeposit test1 = ResolvedIborFixingDeposit.builder().currency(GBP).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).yearFraction(YEAR_FRACTION).floatingRate(RATE_COMP).fixedRate(RATE).build();
		coverImmutableBean(test1);
		ResolvedIborFixingDeposit test2 = ResolvedIborFixingDeposit.builder().currency(GBP).notional(-100000000d).startDate(START_DATE).endDate(LocalDate.of(2015, 4, 20)).yearFraction(0.25).floatingRate(IborRateComputation.of(GBP_LIBOR_3M, FIXING_DATE, REF_DATA)).fixedRate(0.0375).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedIborFixingDeposit test = ResolvedIborFixingDeposit.builder().currency(GBP).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).yearFraction(YEAR_FRACTION).floatingRate(RATE_COMP).fixedRate(RATE).build();
		assertSerialization(test);
	  }

	}

}