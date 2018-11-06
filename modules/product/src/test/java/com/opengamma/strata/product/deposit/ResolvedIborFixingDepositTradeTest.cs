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
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test <seealso cref="ResolvedIborFixingDepositTrade"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedIborFixingDepositTradeTest
	public class ResolvedIborFixingDepositTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate FIXING_DATE = LocalDate.of(2015, 1, 19);
	  private static readonly LocalDate START_DATE = LocalDate.of(2015, 1, 19);
	  private static readonly LocalDate END_DATE = LocalDate.of(2015, 7, 20);
	  private static readonly double YEAR_FRACTION = ACT_365F.yearFraction(START_DATE, END_DATE);
	  private static readonly IborRateComputation RATE_COMP = IborRateComputation.of(GBP_LIBOR_6M, FIXING_DATE, REF_DATA);
	  private const double NOTIONAL = 100000000d;
	  private const double RATE = 0.0250;

	  private static readonly ResolvedIborFixingDeposit DEPOSIT = ResolvedIborFixingDeposit.builder().currency(GBP).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).yearFraction(YEAR_FRACTION).floatingRate(RATE_COMP).fixedRate(RATE).build();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 6, 30));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ResolvedIborFixingDepositTrade test = ResolvedIborFixingDepositTrade.of(TRADE_INFO, DEPOSIT);
		assertEquals(test.Product, DEPOSIT);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		ResolvedIborFixingDepositTrade test = ResolvedIborFixingDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		assertEquals(test.Product, DEPOSIT);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedIborFixingDepositTrade test1 = ResolvedIborFixingDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		coverImmutableBean(test1);
		ResolvedIborFixingDepositTrade test2 = ResolvedIborFixingDepositTrade.builder().product(DEPOSIT).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedIborFixingDepositTrade test = ResolvedIborFixingDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		assertSerialization(test);
	  }

	}

}