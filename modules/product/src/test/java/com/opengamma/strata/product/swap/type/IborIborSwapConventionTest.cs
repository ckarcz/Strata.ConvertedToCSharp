/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_6M;
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
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;

	/// <summary>
	/// Test <seealso cref="IborIborSwapConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborIborSwapConventionTest
	public class IborIborSwapConventionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly DaysAdjustment PLUS_ONE_DAY = DaysAdjustment.ofBusinessDays(1, GBLO);

	  private const string NAME = "USD-Swap";
	  private static readonly IborRateSwapLegConvention IBOR1M = IborRateSwapLegConvention.of(USD_LIBOR_1M);
	  private static readonly IborRateSwapLegConvention IBOR3M = IborRateSwapLegConvention.of(USD_LIBOR_3M);
	  private static readonly IborRateSwapLegConvention IBOR6M = IborRateSwapLegConvention.of(USD_LIBOR_6M);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ImmutableIborIborSwapConvention test = ImmutableIborIborSwapConvention.of(NAME, IBOR3M, IBOR6M);
		assertEquals(test.Name, NAME);
		assertEquals(test.SpreadLeg, IBOR3M);
		assertEquals(test.FlatLeg, IBOR6M);
		assertEquals(test.SpotDateOffset, USD_LIBOR_3M.EffectiveDateOffset);
	  }

	  public virtual void test_of_spotDateOffset()
	  {
		ImmutableIborIborSwapConvention test = ImmutableIborIborSwapConvention.of(NAME, IBOR1M, IBOR3M, PLUS_ONE_DAY);
		assertEquals(test.Name, NAME);
		assertEquals(test.SpreadLeg, IBOR1M);
		assertEquals(test.FlatLeg, IBOR3M);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
	  }

	  public virtual void test_builder()
	  {
		ImmutableIborIborSwapConvention test = ImmutableIborIborSwapConvention.builder().name(NAME).spreadLeg(IBOR1M).flatLeg(IBOR3M).spotDateOffset(PLUS_ONE_DAY).build();
		assertEquals(test.Name, NAME);
		assertEquals(test.SpreadLeg, IBOR1M);
		assertEquals(test.FlatLeg, IBOR3M);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toTrade_tenor()
	  {
		IborIborSwapConvention @base = ImmutableIborIborSwapConvention.of(NAME, IBOR3M, IBOR6M);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 5, 7);
		LocalDate endDate = date(2025, 5, 7);
		SwapTrade test = @base.createTrade(tradeDate, TENOR_10Y, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(IBOR3M.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR6M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_periodTenor()
	  {
		IborIborSwapConvention @base = ImmutableIborIborSwapConvention.of(NAME, IBOR3M, IBOR6M);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 7);
		LocalDate endDate = date(2025, 8, 7);
		SwapTrade test = @base.createTrade(tradeDate, Period.ofMonths(3), TENOR_10Y, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(IBOR3M.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR6M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_dates()
	  {
		IborIborSwapConvention @base = ImmutableIborIborSwapConvention.of(NAME, IBOR3M, IBOR6M);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 5);
		LocalDate endDate = date(2015, 11, 5);
		SwapTrade test = @base.toTrade(tradeDate, startDate, endDate, BUY, NOTIONAL_2M, 0.25d);
		Swap expected = Swap.of(IBOR3M.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR6M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
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
		ImmutableIborIborSwapConvention test = ImmutableIborIborSwapConvention.of(NAME, IBOR3M, IBOR6M);
		coverImmutableBean(test);
		ImmutableIborIborSwapConvention test2 = ImmutableIborIborSwapConvention.of(NAME, IBOR1M, IBOR6M);
		coverBeanEquals(test, test2);
		ImmutableIborIborSwapConvention test3 = ImmutableIborIborSwapConvention.of(NAME, IBOR1M, IBOR3M);
		coverBeanEquals(test, test3);
	  }

	  public virtual void test_serialization()
	  {
		IborIborSwapConvention test = ImmutableIborIborSwapConvention.of(NAME, IBOR3M, IBOR6M);
		assertSerialization(test);
	  }

	}

}