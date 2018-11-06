/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
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
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;

	/// <summary>
	/// Test <seealso cref="FixedIborSwapConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedIborSwapConventionTest
	public class FixedIborSwapConventionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, GBLO);
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);
	  private static readonly DaysAdjustment PLUS_ONE_DAY = DaysAdjustment.ofBusinessDays(1, GBLO);

	  private const string NAME = "USD-Swap";
	  private static readonly FixedRateSwapLegConvention FIXED = FixedRateSwapLegConvention.of(USD, ACT_360, P6M, BDA_FOLLOW);
	  private static readonly FixedRateSwapLegConvention FIXED2 = FixedRateSwapLegConvention.of(GBP, ACT_365F, P3M, BDA_MOD_FOLLOW);
	  private static readonly IborRateSwapLegConvention IBOR = IborRateSwapLegConvention.of(USD_LIBOR_3M);
	  private static readonly IborRateSwapLegConvention IBOR2 = IborRateSwapLegConvention.of(GBP_LIBOR_3M);
	  private static readonly IborRateSwapLegConvention IBOR3 = IborRateSwapLegConvention.of(USD_LIBOR_6M);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ImmutableFixedIborSwapConvention test = ImmutableFixedIborSwapConvention.of(NAME, FIXED, IBOR);
		assertEquals(test.Name, NAME);
		assertEquals(test.FixedLeg, FIXED);
		assertEquals(test.FloatingLeg, IBOR);
		assertEquals(test.SpotDateOffset, USD_LIBOR_3M.EffectiveDateOffset);
	  }

	  public virtual void test_of_spotDateOffset()
	  {
		ImmutableFixedIborSwapConvention test = ImmutableFixedIborSwapConvention.of(NAME, FIXED, IBOR, PLUS_ONE_DAY);
		assertEquals(test.Name, NAME);
		assertEquals(test.FixedLeg, FIXED);
		assertEquals(test.FloatingLeg, IBOR);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
	  }

	  public virtual void test_builder()
	  {
		ImmutableFixedIborSwapConvention test = ImmutableFixedIborSwapConvention.builder().name(NAME).fixedLeg(FIXED).floatingLeg(IBOR).spotDateOffset(PLUS_ONE_DAY).build();
		assertEquals(test.Name, NAME);
		assertEquals(test.FixedLeg, FIXED);
		assertEquals(test.FloatingLeg, IBOR);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toTrade_tenor()
	  {
		FixedIborSwapConvention @base = ImmutableFixedIborSwapConvention.of(NAME, FIXED, IBOR);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 5, 7);
		LocalDate endDate = date(2025, 5, 7);
		SwapTrade test = @base.createTrade(tradeDate, TENOR_10Y, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(FIXED.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_periodTenor()
	  {
		FixedIborSwapConvention @base = ImmutableFixedIborSwapConvention.of(NAME, FIXED, IBOR);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 7);
		LocalDate endDate = date(2025, 8, 7);
		SwapTrade test = @base.createTrade(tradeDate, Period.ofMonths(3), TENOR_10Y, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(FIXED.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_dates()
	  {
		FixedIborSwapConvention @base = ImmutableFixedIborSwapConvention.of(NAME, FIXED, IBOR);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 5);
		LocalDate endDate = date(2015, 11, 5);
		SwapTrade test = @base.toTrade(tradeDate, startDate, endDate, BUY, NOTIONAL_2M, 0.25d);
		Swap expected = Swap.of(FIXED.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
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
			new object[] {FixedIborSwapConventions.USD_FIXED_1Y_LIBOR_3M, "USD-FIXED-1Y-LIBOR-3M"},
			new object[] {FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M, "USD-FIXED-6M-LIBOR-3M"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(FixedIborSwapConvention convention, String name)
	  public virtual void test_name(FixedIborSwapConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(FixedIborSwapConvention convention, String name)
	  public virtual void test_toString(FixedIborSwapConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(FixedIborSwapConvention convention, String name)
	  public virtual void test_of_lookup(FixedIborSwapConvention convention, string name)
	  {
		assertEquals(FixedIborSwapConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(FixedIborSwapConvention convention, String name)
	  public virtual void test_extendedEnum(FixedIborSwapConvention convention, string name)
	  {
		FixedIborSwapConvention.of(name); // ensures map is populated
		ImmutableMap<string, FixedIborSwapConvention> map = FixedIborSwapConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => FixedIborSwapConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => FixedIborSwapConvention.of((string) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableFixedIborSwapConvention test = ImmutableFixedIborSwapConvention.of(NAME, FIXED, IBOR);
		coverImmutableBean(test);
		ImmutableFixedIborSwapConvention test2 = ImmutableFixedIborSwapConvention.of("GBP-Swap", FIXED2, IBOR2);
		coverBeanEquals(test, test2);
		ImmutableFixedIborSwapConvention test3 = ImmutableFixedIborSwapConvention.of(NAME, FIXED, IBOR3);
		coverBeanEquals(test, test3);
	  }

	  public virtual void test_serialization()
	  {
		FixedIborSwapConvention test = ImmutableFixedIborSwapConvention.of(NAME, FIXED, IBOR);
		assertSerialization(test);
	  }

	}

}