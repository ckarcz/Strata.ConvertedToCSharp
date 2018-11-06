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
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;

	/// <summary>
	/// Test <seealso cref="ResolvedCds"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedCdsTradeTest
	public class ResolvedCdsTradeTest
	{
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABC");
	  private const double COUPON = 0.05;
	  private const double NOTIONAL = 1.0e9;
	  private static readonly LocalDate START_DATE = LocalDate.of(2013, 12, 20);
	  private static readonly LocalDate END_DATE = LocalDate.of(2024, 9, 20);

	  private static readonly ResolvedCds PRODUCT = Cds.of(BUY, LEGAL_ENTITY, USD, NOTIONAL, START_DATE, END_DATE, P3M, CALENDAR, COUPON).resolve(REF_DATA);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(LocalDate.of(2014, 1, 9));
	  private static readonly Payment UPFRONT = Payment.of(USD, NOTIONAL, LocalDate.of(2014, 1, 12));

	  public virtual void test_builder_full()
	  {
		ResolvedCdsTrade test = ResolvedCdsTrade.builder().product(PRODUCT).info(TRADE_INFO).upfrontFee(UPFRONT).build();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.UpfrontFee.get(), UPFRONT);
	  }

	  public virtual void test_builder_min()
	  {
		ResolvedCdsTrade test = ResolvedCdsTrade.builder().product(PRODUCT).info(TRADE_INFO).build();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertFalse(test.UpfrontFee.Present);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedCdsTrade test1 = ResolvedCdsTrade.builder().product(PRODUCT).upfrontFee(UPFRONT).info(TRADE_INFO).build();
		coverImmutableBean(test1);
		ResolvedCds product = Cds.of(BUY, LEGAL_ENTITY, USD, 1.e9, START_DATE, END_DATE, Frequency.P6M, SAT_SUN, 0.067).resolve(REF_DATA);
		ResolvedCdsTrade test2 = ResolvedCdsTrade.builder().product(product).info(TradeInfo.empty()).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedCdsTrade test = ResolvedCdsTrade.builder().product(PRODUCT).upfrontFee(UPFRONT).info(TRADE_INFO).build();
		assertSerialization(test);
	  }

	}

}