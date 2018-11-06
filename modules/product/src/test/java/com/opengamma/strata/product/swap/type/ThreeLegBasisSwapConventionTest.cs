/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_12M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
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
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;

	/// <summary>
	/// Test <seealso cref="ThreeLegBasisSwapConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ThreeLegBasisSwapConventionTest
	public class ThreeLegBasisSwapConventionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, EUTA);
	  private static readonly DaysAdjustment PLUS_ONE_DAY = DaysAdjustment.ofBusinessDays(1, EUTA);

	  private const string NAME = "EUR-Swap";
	  private static readonly FixedRateSwapLegConvention FIXED = FixedRateSwapLegConvention.of(EUR, THIRTY_U_360, P12M, BDA_FOLLOW);
	  private static readonly IborRateSwapLegConvention IBOR3M = IborRateSwapLegConvention.of(EUR_EURIBOR_3M);
	  private static readonly IborRateSwapLegConvention IBOR6M = IborRateSwapLegConvention.of(EUR_EURIBOR_6M);
	  private static readonly IborRateSwapLegConvention IBOR12M = IborRateSwapLegConvention.of(EUR_EURIBOR_12M);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ImmutableThreeLegBasisSwapConvention test = ImmutableThreeLegBasisSwapConvention.of(NAME, FIXED, IBOR6M, IBOR12M);
		assertEquals(test.Name, NAME);
		assertEquals(test.SpreadLeg, FIXED);
		assertEquals(test.SpreadFloatingLeg, IBOR6M);
		assertEquals(test.FlatFloatingLeg, IBOR12M);
		assertEquals(test.SpotDateOffset, EUR_EURIBOR_6M.EffectiveDateOffset);
	  }

	  public virtual void test_of_spotDateOffset()
	  {
		ImmutableThreeLegBasisSwapConvention test = ImmutableThreeLegBasisSwapConvention.of(NAME, FIXED, IBOR6M, IBOR12M, PLUS_ONE_DAY);
		assertEquals(test.Name, NAME);
		assertEquals(test.SpreadLeg, FIXED);
		assertEquals(test.SpreadFloatingLeg, IBOR6M);
		assertEquals(test.FlatFloatingLeg, IBOR12M);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
	  }

	  public virtual void test_builder()
	  {
		ImmutableThreeLegBasisSwapConvention test = ImmutableThreeLegBasisSwapConvention.builder().name(NAME).spreadLeg(FIXED).spreadFloatingLeg(IBOR6M).flatFloatingLeg(IBOR12M).spotDateOffset(PLUS_ONE_DAY).build();
		assertEquals(test.Name, NAME);
		assertEquals(test.SpreadLeg, FIXED);
		assertEquals(test.SpreadFloatingLeg, IBOR6M);
		assertEquals(test.FlatFloatingLeg, IBOR12M);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toTrade_tenor()
	  {
		ThreeLegBasisSwapConvention @base = ImmutableThreeLegBasisSwapConvention.of(NAME, FIXED, IBOR6M, IBOR12M);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 5, 7);
		LocalDate endDate = date(2025, 5, 7);
		SwapTrade test = @base.createTrade(tradeDate, TENOR_10Y, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(FIXED.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR6M.toLeg(startDate, endDate, PAY, NOTIONAL_2M), IBOR12M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_periodTenor()
	  {
		ThreeLegBasisSwapConvention @base = ImmutableThreeLegBasisSwapConvention.of(NAME, FIXED, IBOR6M, IBOR12M);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 7);
		LocalDate endDate = date(2025, 8, 7);
		SwapTrade test = @base.createTrade(tradeDate, Period.ofMonths(3), TENOR_10Y, BUY, NOTIONAL_2M, 0.25d, REF_DATA);
		Swap expected = Swap.of(FIXED.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR6M.toLeg(startDate, endDate, PAY, NOTIONAL_2M), IBOR12M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
	  }

	  public virtual void test_toTrade_dates()
	  {
		ThreeLegBasisSwapConvention @base = ImmutableThreeLegBasisSwapConvention.of(NAME, FIXED, IBOR6M, IBOR12M);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 5);
		LocalDate endDate = date(2015, 11, 5);
		SwapTrade test = @base.toTrade(tradeDate, startDate, endDate, BUY, NOTIONAL_2M, 0.25d);
		Swap expected = Swap.of(FIXED.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), IBOR6M.toLeg(startDate, endDate, PAY, NOTIONAL_2M), IBOR12M.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
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
			new object[] {ThreeLegBasisSwapConventions.EUR_FIXED_1Y_EURIBOR_3M_EURIBOR_6M, "EUR-FIXED-1Y-EURIBOR-3M-EURIBOR-6M"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(ThreeLegBasisSwapConvention convention, String name)
	  public virtual void test_name(ThreeLegBasisSwapConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(ThreeLegBasisSwapConvention convention, String name)
	  public virtual void test_toString(ThreeLegBasisSwapConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(ThreeLegBasisSwapConvention convention, String name)
	  public virtual void test_of_lookup(ThreeLegBasisSwapConvention convention, string name)
	  {
		assertEquals(ThreeLegBasisSwapConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(ThreeLegBasisSwapConvention convention, String name)
	  public virtual void test_extendedEnum(ThreeLegBasisSwapConvention convention, string name)
	  {
		ThreeLegBasisSwapConvention.of(name); // ensures map is populated
		ImmutableMap<string, ThreeLegBasisSwapConvention> map = ThreeLegBasisSwapConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => ThreeLegBasisSwapConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => ThreeLegBasisSwapConvention.of((string) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableThreeLegBasisSwapConvention test = ImmutableThreeLegBasisSwapConvention.of(NAME, FIXED, IBOR6M, IBOR12M);
		coverImmutableBean(test);
		ImmutableThreeLegBasisSwapConvention test2 = ImmutableThreeLegBasisSwapConvention.of("swap", FIXED, IBOR3M, IBOR6M);
		coverBeanEquals(test, test2);
		ImmutableThreeLegBasisSwapConvention test3 = ImmutableThreeLegBasisSwapConvention.of(NAME, FIXED, IBOR3M, IBOR12M);
		coverBeanEquals(test, test3);
	  }

	  public virtual void test_serialization()
	  {
		ThreeLegBasisSwapConvention test = ImmutableThreeLegBasisSwapConvention.of(NAME, FIXED, IBOR6M, IBOR12M);
		assertSerialization(test);
	  }

	}

}