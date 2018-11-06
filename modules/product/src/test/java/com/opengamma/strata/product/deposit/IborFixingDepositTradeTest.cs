/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
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
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// Test <seealso cref="IborFixingDepositTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFixingDepositTradeTest
	public class IborFixingDepositTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly IborFixingDeposit DEPOSIT = IborFixingDeposit.builder().buySell(BuySell.BUY).notional(100000000d).startDate(LocalDate.of(2015, 1, 19)).endDate(LocalDate.of(2015, 7, 19)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).index(GBP_LIBOR_6M).fixedRate(0.0250).build();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2015, 1, 15));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		IborFixingDepositTrade test = IborFixingDepositTrade.of(TRADE_INFO, DEPOSIT);
		assertEquals(test.Product, DEPOSIT);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		IborFixingDepositTrade test = IborFixingDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		assertEquals(test.Product, DEPOSIT);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		IborFixingDepositTrade test = IborFixingDepositTrade.of(TRADE_INFO, DEPOSIT);
		assertEquals(test.resolve(REF_DATA).Info, TRADE_INFO);
		assertEquals(test.resolve(REF_DATA).Product, DEPOSIT.resolve(REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborFixingDepositTrade test1 = IborFixingDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		coverImmutableBean(test1);
		IborFixingDepositTrade test2 = IborFixingDepositTrade.builder().product(DEPOSIT).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborFixingDepositTrade test = IborFixingDepositTrade.builder().product(DEPOSIT).info(TRADE_INFO).build();
		assertSerialization(test);
	  }

	}

}