/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;

	/// <summary>
	/// Test <seealso cref="XCcyIborIborSwapConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class XCcyIborIborSwapConventionTest
	public class XCcyIborIborSwapConventionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly HolidayCalendarId EUTA_USNY = EUTA.combinedWith(USNY);

	  private const string NAME = "EUR-EURIBOR-3M-USD-LIBOR-3M";
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly CurrencyPair EUR_USD = CurrencyPair.of(Currency.EUR, Currency.USD);
	  private const double FX_EUR_USD = 1.15d;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, EUTA_USNY);
	  private static readonly DaysAdjustment NEXT_SAME_BUS_DAY = DaysAdjustment.ofCalendarDays(0, BDA_FOLLOW);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, EUTA_USNY);
	  private static readonly DaysAdjustment PLUS_ONE_DAY = DaysAdjustment.ofBusinessDays(1, EUTA_USNY);

	  private static readonly IborRateSwapLegConvention EUR3M = IborRateSwapLegConvention.builder().index(IborIndices.EUR_EURIBOR_3M).accrualBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA_USNY)).build();
	  private static readonly IborRateSwapLegConvention USD3M = IborRateSwapLegConvention.builder().index(IborIndices.USD_LIBOR_3M).accrualBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA_USNY)).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ImmutableXCcyIborIborSwapConvention test = ImmutableXCcyIborIborSwapConvention.of(NAME, EUR3M, USD3M);
		assertEquals(test.Name, NAME);
		assertEquals(test.SpreadLeg, EUR3M);
		assertEquals(test.FlatLeg, USD3M);
		assertEquals(test.SpotDateOffset, EUR3M.Index.EffectiveDateOffset);
		assertEquals(test.CurrencyPair, EUR_USD);
	  }

	  public virtual void test_of_spotDateOffset()
	  {
		ImmutableXCcyIborIborSwapConvention test = ImmutableXCcyIborIborSwapConvention.of(NAME, EUR3M, USD3M, PLUS_ONE_DAY);
		assertEquals(test.Name, NAME);
		assertEquals(test.SpreadLeg, EUR3M);
		assertEquals(test.FlatLeg, USD3M);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
		assertEquals(test.CurrencyPair, EUR_USD);
	  }

	  public virtual void test_builder()
	  {
		ImmutableXCcyIborIborSwapConvention test = ImmutableXCcyIborIborSwapConvention.builder().name(NAME).spreadLeg(EUR3M).flatLeg(USD3M).spotDateOffset(PLUS_ONE_DAY).build();
		assertEquals(test.Name, NAME);
		assertEquals(test.SpreadLeg, EUR3M);
		assertEquals(test.FlatLeg, USD3M);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
		assertEquals(test.CurrencyPair, EUR_USD);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toTrade_tenor()
	  {
		ImmutableXCcyIborIborSwapConvention @base = ImmutableXCcyIborIborSwapConvention.of(NAME, EUR3M, USD3M, PLUS_TWO_DAYS);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 5, 7);
		LocalDate endDate = date(2025, 5, 7);
		SwapTrade test = @base.createTrade(tradeDate, TENOR_10Y, BUY, NOTIONAL_2M, NOTIONAL_2M * FX_EUR_USD, 0.25d, REF_DATA);
		Swap expected = Swap.of(EUR3M.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), USD3M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M * FX_EUR_USD));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_periodTenor()
	  {
		ImmutableXCcyIborIborSwapConvention @base = ImmutableXCcyIborIborSwapConvention.of(NAME, EUR3M, USD3M, PLUS_TWO_DAYS);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 7);
		LocalDate endDate = date(2025, 8, 7);
		SwapTrade test = @base.createTrade(tradeDate, Period.ofMonths(3), TENOR_10Y, BUY, NOTIONAL_2M, NOTIONAL_2M * FX_EUR_USD, 0.25d, REF_DATA);
		Swap expected = Swap.of(EUR3M.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), USD3M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M * FX_EUR_USD));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_dates()
	  {
		ImmutableXCcyIborIborSwapConvention @base = ImmutableXCcyIborIborSwapConvention.of(NAME, EUR3M, USD3M, PLUS_TWO_DAYS);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 5);
		LocalDate endDate = date(2015, 11, 5);
		SwapTrade test = @base.toTrade(tradeDate, startDate, endDate, BUY, NOTIONAL_2M, NOTIONAL_2M * FX_EUR_USD, 0.25d);
		Swap expected = Swap.of(EUR3M.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), USD3M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M * FX_EUR_USD));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {IborIborSwapConventions.USD_LIBOR_1M_LIBOR_3M, "USD-LIBOR-1M-LIBOR-3M"},
			new object[] {IborIborSwapConventions.USD_LIBOR_3M_LIBOR_6M, "USD-LIBOR-3M-LIBOR-6M"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(IborIborSwapConvention convention, String name)
	  public virtual void test_name(IborIborSwapConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(IborIborSwapConvention convention, String name)
	  public virtual void test_toString(IborIborSwapConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(IborIborSwapConvention convention, String name)
	  public virtual void test_of_lookup(IborIborSwapConvention convention, string name)
	  {
		assertEquals(IborIborSwapConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(IborIborSwapConvention convention, String name)
	  public virtual void test_extendedEnum(IborIborSwapConvention convention, string name)
	  {
		IborIborSwapConvention.of(name); // ensures map is populated
		ImmutableMap<string, IborIborSwapConvention> map = IborIborSwapConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => IborIborSwapConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => IborIborSwapConvention.of((string) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableXCcyIborIborSwapConvention test = ImmutableXCcyIborIborSwapConvention.of(NAME, EUR3M, USD3M, PLUS_TWO_DAYS);
		coverImmutableBean(test);
		ImmutableXCcyIborIborSwapConvention test2 = ImmutableXCcyIborIborSwapConvention.of("XXX", USD3M, EUR3M, NEXT_SAME_BUS_DAY);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ImmutableXCcyIborIborSwapConvention test = ImmutableXCcyIborIborSwapConvention.of(NAME, EUR3M, USD3M, PLUS_TWO_DAYS);
		assertSerialization(test);
	  }

	}

}