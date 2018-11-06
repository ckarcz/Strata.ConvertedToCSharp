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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;

	/// <summary>
	/// Test <seealso cref="CdsTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsTradeTest
	public class CdsTradeTest
	{
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABC");
	  private const double COUPON = 0.05;
	  private const double NOTIONAL = 1.0e9;
	  private static readonly LocalDate START_DATE = LocalDate.of(2013, 12, 20);
	  private static readonly LocalDate END_DATE = LocalDate.of(2024, 9, 20);

	  private static readonly Cds PRODUCT = Cds.of(BUY, LEGAL_ENTITY, USD, NOTIONAL, START_DATE, END_DATE, P3M, CALENDAR, COUPON);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(LocalDate.of(2014, 1, 9));
	  private static readonly AdjustablePayment UPFRONT = AdjustablePayment.of(USD, NOTIONAL, LocalDate.of(2014, 1, 12));

	  public virtual void test_full_builder()
	  {
		CdsTrade test = sut();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.UpfrontFee.get(), UPFRONT);
	  }

	  public virtual void test_min_builder()
	  {
		CdsTrade test = CdsTrade.builder().product(PRODUCT).info(TRADE_INFO).build();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertFalse(test.UpfrontFee.Present);
	  }

	  public virtual void test_full_resolve()
	  {
		ResolvedCdsTrade test = sut().resolve(REF_DATA);
		assertEquals(test.Product, PRODUCT.resolve(REF_DATA));
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.UpfrontFee.get(), UPFRONT.resolve(REF_DATA));
	  }

	  public virtual void test_min_resolve()
	  {
		ResolvedCdsTrade test = CdsTrade.builder().product(PRODUCT).info(TRADE_INFO).build().resolve(REF_DATA);
		assertEquals(test.Product, PRODUCT.resolve(REF_DATA));
		assertEquals(test.Info, TRADE_INFO);
		assertFalse(test.UpfrontFee.Present);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		CdsTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.CDS).currencies(Currency.USD).description("10Y9M Buy USD 1000mm ABC / 5% : 20Dec13-20Sep24").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CdsTrade test1 = sut();
		coverImmutableBean(test1);
		Cds product = Cds.of(BUY, LEGAL_ENTITY, USD, 1.e9, START_DATE, END_DATE, P3M, SAT_SUN, 0.067);
		CdsTrade test2 = CdsTrade.builder().product(product).info(TradeInfo.empty()).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		CdsTrade test = sut();
		assertSerialization(test);
	  }

	  //-------------------------------------------------------------------------
	  internal virtual CdsTrade sut()
	  {
		return CdsTrade.builder().product(PRODUCT).upfrontFee(UPFRONT).info(TRADE_INFO).build();
	  }

	}

}