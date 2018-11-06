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
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ResolvedTermDepositTrade"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedTermDepositTradeTest
	public class ResolvedTermDepositTradeTest
	{

	  private static readonly LocalDate START_DATE = LocalDate.of(2015, 1, 19);
	  private static readonly LocalDate END_DATE = LocalDate.of(2015, 7, 20);
	  private static readonly double YEAR_FRACTION = ACT_365F.yearFraction(START_DATE, END_DATE);
	  private const double PRINCIPAL = 100000000d;
	  private const double RATE = 0.0250;

	  private static readonly ResolvedTermDeposit DEPOSIT = ResolvedTermDeposit.builder().currency(GBP).notional(PRINCIPAL).startDate(START_DATE).endDate(END_DATE).yearFraction(YEAR_FRACTION).rate(RATE).build();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 6, 30));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ResolvedTermDepositTrade test = ResolvedTermDepositTrade.of(TRADE_INFO, DEPOSIT);
		assertEquals(test.Product, DEPOSIT);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		ResolvedTermDepositTrade test = ResolvedTermDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		assertEquals(test.Product, DEPOSIT);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedTermDepositTrade test1 = ResolvedTermDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		coverImmutableBean(test1);
		ResolvedTermDepositTrade test2 = ResolvedTermDepositTrade.builder().product(DEPOSIT).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedTermDepositTrade test = ResolvedTermDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		assertSerialization(test);
	  }

	}

}