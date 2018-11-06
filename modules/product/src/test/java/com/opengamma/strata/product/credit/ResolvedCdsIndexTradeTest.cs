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
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;

	/// <summary>
	/// Test <seealso cref="ResolvedCdsIndexTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedCdsIndexTradeTest
	public class ResolvedCdsIndexTradeTest
	{
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly DaysAdjustment SETTLE_DAY_ADJ = DaysAdjustment.ofBusinessDays(3, CALENDAR);
	  private static readonly StandardId INDEX_ID = StandardId.of("OG", "AA-INDEX");
	  private static readonly ImmutableList<StandardId> LEGAL_ENTITIES = ImmutableList.of(StandardId.of("OG", "ABC1"), StandardId.of("OG", "ABC2"), StandardId.of("OG", "ABC3"), StandardId.of("OG", "ABC4"));
	  private const double COUPON = 0.05;
	  private const double NOTIONAL = 1.0e9;
	  private static readonly LocalDate START_DATE = LocalDate.of(2013, 12, 20);
	  private static readonly LocalDate END_DATE = LocalDate.of(2024, 9, 20);
	  private static readonly ResolvedCdsIndex PRODUCT = CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, START_DATE, END_DATE, P3M, SAT_SUN, COUPON).resolve(REF_DATA);
	  private static readonly LocalDate TRADE_DATE = LocalDate.of(2014, 1, 9);
	  private static readonly LocalDate SETTLE_DATE = SETTLE_DAY_ADJ.adjust(TRADE_DATE, REF_DATA);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(TRADE_DATE).settlementDate(SETTLE_DATE).build();
	  private static readonly Payment UPFRONT = Payment.of(USD, NOTIONAL, SETTLE_DATE);

	  public virtual void test_builder_full()
	  {
		ResolvedCdsIndexTrade test = ResolvedCdsIndexTrade.builder().product(PRODUCT).info(TRADE_INFO).upfrontFee(UPFRONT).build();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.UpfrontFee.get(), UPFRONT);

		ResolvedCdsTrade singleName = test.toSingleNameCds();
		assertEquals(singleName.Product, PRODUCT.toSingleNameCds());
		assertEquals(singleName.Info, TRADE_INFO);
		assertEquals(singleName.UpfrontFee.get(), UPFRONT);
	  }

	  public virtual void test_builder_min()
	  {
		ResolvedCdsIndexTrade test = ResolvedCdsIndexTrade.builder().product(PRODUCT).info(TRADE_INFO).build();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertFalse(test.UpfrontFee.Present);

		ResolvedCdsTrade singleName = test.toSingleNameCds();
		assertEquals(singleName.Product, PRODUCT.toSingleNameCds());
		assertEquals(singleName.Info, TRADE_INFO);
		assertFalse(singleName.UpfrontFee.Present);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedCdsIndexTrade test1 = ResolvedCdsIndexTrade.builder().product(PRODUCT).upfrontFee(UPFRONT).info(TRADE_INFO).build();
		coverImmutableBean(test1);
		ResolvedCdsIndex product = CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, 1.e9, START_DATE, END_DATE, P6M, SAT_SUN, 0.067).resolve(REF_DATA);
		ResolvedCdsIndexTrade test2 = ResolvedCdsIndexTrade.builder().product(product).info(TradeInfo.empty()).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedCdsIndexTrade test = ResolvedCdsIndexTrade.builder().product(PRODUCT).upfrontFee(UPFRONT).info(TRADE_INFO).build();
		assertSerialization(test);
	  }

	}

}