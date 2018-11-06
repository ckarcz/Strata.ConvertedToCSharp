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
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
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
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;

	/// <summary>
	/// Test <seealso cref="CdsIndexTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsIndexTradeTest
	public class CdsIndexTradeTest
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
	  private static readonly CdsIndex PRODUCT = CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, START_DATE, END_DATE, P3M, SAT_SUN, COUPON);
	  private static readonly LocalDate TRADE_DATE = LocalDate.of(2014, 1, 9);
	  private static readonly LocalDate SETTLE_DATE = SETTLE_DAY_ADJ.adjust(TRADE_DATE, REF_DATA);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(TRADE_DATE).settlementDate(SETTLE_DATE).build();
	  private static readonly AdjustablePayment UPFRONT = AdjustablePayment.of(USD, NOTIONAL, AdjustableDate.of(SETTLE_DATE, BusinessDayAdjustment.of(FOLLOWING, SAT_SUN)));

	  public virtual void test_full_builder()
	  {
		CdsIndexTrade test = sut();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.UpfrontFee.get(), UPFRONT);
	  }

	  public virtual void test_min_builder()
	  {
		CdsIndexTrade test = CdsIndexTrade.builder().product(PRODUCT).info(TRADE_INFO).build();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertFalse(test.UpfrontFee.Present);
	  }

	  public virtual void test_full_resolve()
	  {
		ResolvedCdsIndexTrade test = sut().resolve(REF_DATA);
		assertEquals(test.Product, PRODUCT.resolve(REF_DATA));
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.UpfrontFee.get(), UPFRONT.resolve(REF_DATA));
	  }

	  public virtual void test_min_resolve()
	  {
		ResolvedCdsIndexTrade test = CdsIndexTrade.builder().product(PRODUCT).info(TRADE_INFO).build().resolve(REF_DATA);
		assertEquals(test.Product, PRODUCT.resolve(REF_DATA));
		assertEquals(test.Info, TRADE_INFO);
		assertFalse(test.UpfrontFee.Present);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		CdsIndexTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.CDS_INDEX).currencies(Currency.USD).description("10Y9M Buy USD 1000mm AA-INDEX / 5% : 20Dec13-20Sep24").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CdsIndexTrade test1 = sut();
		coverImmutableBean(test1);
		CdsIndex product = CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, 1.e9, START_DATE, END_DATE, P6M, SAT_SUN, 0.067);
		CdsIndexTrade test2 = CdsIndexTrade.builder().product(product).info(TradeInfo.empty()).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		CdsIndexTrade test = sut();
		assertSerialization(test);
	  }

	  //-------------------------------------------------------------------------
	  internal virtual CdsIndexTrade sut()
	  {
		return CdsIndexTrade.builder().product(PRODUCT).upfrontFee(UPFRONT).info(TRADE_INFO).build();
	  }

	}

}