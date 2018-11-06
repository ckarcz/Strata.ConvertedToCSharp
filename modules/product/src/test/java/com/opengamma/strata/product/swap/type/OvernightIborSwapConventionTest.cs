/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P12M;
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
//	import static com.opengamma.strata.product.swap.OvernightAccrualMethod.AVERAGED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// Test <seealso cref="OvernightIborSwapConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightIborSwapConventionTest
	public class OvernightIborSwapConventionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly DaysAdjustment PLUS_ONE_DAY = DaysAdjustment.ofBusinessDays(1, GBLO);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, GBLO);

	  private const string NAME = "USD-FF";
	  private static readonly OvernightRateSwapLegConvention FFUND_LEG = OvernightRateSwapLegConvention.builder().index(USD_FED_FUND).accrualMethod(AVERAGED).accrualFrequency(Frequency.P3M).paymentFrequency(Frequency.P3M).stubConvention(StubConvention.SMART_INITIAL).rateCutOffDays(2).build();
	  private static readonly OvernightRateSwapLegConvention FFUND_LEG2 = OvernightRateSwapLegConvention.of(USD_FED_FUND, P12M, 3);
	  private static readonly OvernightRateSwapLegConvention FLOATING_LEG2 = OvernightRateSwapLegConvention.of(GBP_SONIA, P12M, 0);
	  private static readonly IborRateSwapLegConvention USD_LIBOR_3M_LEG = IborRateSwapLegConvention.of(USD_LIBOR_3M);
	  private static readonly IborRateSwapLegConvention GBP_LIBOR_3M_LEG = IborRateSwapLegConvention.of(GBP_LIBOR_3M);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ImmutableOvernightIborSwapConvention test = ImmutableOvernightIborSwapConvention.of(NAME, FFUND_LEG, USD_LIBOR_3M_LEG, PLUS_TWO_DAYS);
		assertEquals(test.Name, NAME);
		assertEquals(test.OvernightLeg, FFUND_LEG);
		assertEquals(test.IborLeg, USD_LIBOR_3M_LEG);
		assertEquals(test.SpotDateOffset, PLUS_TWO_DAYS);
	  }

	  public virtual void test_builder()
	  {
		ImmutableOvernightIborSwapConvention test = ImmutableOvernightIborSwapConvention.builder().name(NAME).overnightLeg(FFUND_LEG).iborLeg(USD_LIBOR_3M_LEG).spotDateOffset(PLUS_ONE_DAY).build();
		assertEquals(test.Name, NAME);
		assertEquals(test.OvernightLeg, FFUND_LEG);
		assertEquals(test.IborLeg, USD_LIBOR_3M_LEG);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toTrade_tenor()
	  {
		OvernightIborSwapConvention @base = ImmutableOvernightIborSwapConvention.of(NAME, FFUND_LEG, USD_LIBOR_3M_LEG, PLUS_TWO_DAYS);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 5, 7);
		LocalDate endDate = date(2025, 5, 7);
		SwapTrade test = @base.createTrade(tradeDate, TENOR_10Y, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(FFUND_LEG.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), USD_LIBOR_3M_LEG.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_periodTenor()
	  {
		OvernightIborSwapConvention @base = ImmutableOvernightIborSwapConvention.of(NAME, FFUND_LEG, USD_LIBOR_3M_LEG, PLUS_TWO_DAYS);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 7);
		LocalDate endDate = date(2025, 8, 7);
		SwapTrade test = @base.createTrade(tradeDate, Period.ofMonths(3), TENOR_10Y, BuySell.SELL, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(FFUND_LEG.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M, 0.25d), USD_LIBOR_3M_LEG.toLeg(startDate, endDate, PAY, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_dates()
	  {
		OvernightIborSwapConvention @base = ImmutableOvernightIborSwapConvention.of(NAME, FFUND_LEG, USD_LIBOR_3M_LEG, PLUS_TWO_DAYS);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 5);
		LocalDate endDate = date(2015, 11, 5);
		SwapTrade test = @base.toTrade(tradeDate, startDate, endDate, BUY, NOTIONAL_2M, 0.25d);
		Swap expected = Swap.of(FFUND_LEG.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), USD_LIBOR_3M_LEG.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
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
			new object[] {OvernightIborSwapConventions.USD_FED_FUND_AA_LIBOR_3M, "USD-FED-FUND-AA-LIBOR-3M"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(OvernightIborSwapConvention convention, String name)
	  public virtual void test_name(OvernightIborSwapConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(OvernightIborSwapConvention convention, String name)
	  public virtual void test_toString(OvernightIborSwapConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(OvernightIborSwapConvention convention, String name)
	  public virtual void test_of_lookup(OvernightIborSwapConvention convention, string name)
	  {
		assertEquals(OvernightIborSwapConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(OvernightIborSwapConvention convention, String name)
	  public virtual void test_extendedEnum(OvernightIborSwapConvention convention, string name)
	  {
		OvernightIborSwapConvention.of(name); // ensures map is populated
		ImmutableMap<string, OvernightIborSwapConvention> map = OvernightIborSwapConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => OvernightIborSwapConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => OvernightIborSwapConvention.of((string) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableOvernightIborSwapConvention test = ImmutableOvernightIborSwapConvention.of(NAME, FFUND_LEG, USD_LIBOR_3M_LEG, PLUS_TWO_DAYS);
		coverImmutableBean(test);
		ImmutableOvernightIborSwapConvention test2 = ImmutableOvernightIborSwapConvention.of("GBP-Swap", FLOATING_LEG2, GBP_LIBOR_3M_LEG, PLUS_ONE_DAY);
		coverBeanEquals(test, test2);
		ImmutableOvernightIborSwapConvention test3 = ImmutableOvernightIborSwapConvention.of("USD-Swap2", FFUND_LEG2, USD_LIBOR_3M_LEG, PLUS_ONE_DAY);
		coverBeanEquals(test, test3);
	  }

	  public virtual void test_serialization()
	  {
		ImmutableOvernightIborSwapConvention test = ImmutableOvernightIborSwapConvention.of(NAME, FFUND_LEG, USD_LIBOR_3M_LEG, PLUS_TWO_DAYS);
		assertSerialization(test);
	  }

	}

}