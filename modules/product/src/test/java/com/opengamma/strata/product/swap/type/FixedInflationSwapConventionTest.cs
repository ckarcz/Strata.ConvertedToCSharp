/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ONE_ONE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_HICP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_RPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_RPIX;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
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
//	import static com.opengamma.strata.product.swap.PriceIndexCalculationMethod.MONTHLY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;

	/// <summary>
	/// Test <seealso cref="FixedInflationSwapConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedInflationSwapConventionTest
	public class FixedInflationSwapConventionTest
	{

	  private static readonly Period LAG_3M = Period.ofMonths(3);
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);
	  private static readonly DaysAdjustment PLUS_ONE_DAY = DaysAdjustment.ofBusinessDays(1, GBLO);

	  private const string NAME = "GBP-Swap";
	  private static readonly FixedRateSwapLegConvention FIXED = fixedLegZcConvention(GBP, GBLO);
	  private static readonly FixedRateSwapLegConvention FIXED2 = FixedRateSwapLegConvention.of(GBP, ACT_365F, P3M, BDA_MOD_FOLLOW);
	  private static readonly InflationRateSwapLegConvention INFL = InflationRateSwapLegConvention.of(GB_HICP, LAG_3M, MONTHLY, BDA_MOD_FOLLOW);
	  private static readonly InflationRateSwapLegConvention INFL2 = InflationRateSwapLegConvention.of(GB_RPI, LAG_3M, MONTHLY, BDA_MOD_FOLLOW);
	  private static readonly InflationRateSwapLegConvention INFL3 = InflationRateSwapLegConvention.of(GB_RPIX, LAG_3M, MONTHLY, BDA_MOD_FOLLOW);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ImmutableFixedInflationSwapConvention test = ImmutableFixedInflationSwapConvention.of(NAME, FIXED, INFL, PLUS_ONE_DAY);
		assertEquals(test.Name, NAME);
		assertEquals(test.FixedLeg, FIXED);
		assertEquals(test.FloatingLeg, INFL);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
	  }

	  public virtual void test_of_spotDateOffset()
	  {
		ImmutableFixedInflationSwapConvention test = ImmutableFixedInflationSwapConvention.of(NAME, FIXED, INFL, PLUS_ONE_DAY);
		assertEquals(test.Name, NAME);
		assertEquals(test.FixedLeg, FIXED);
		assertEquals(test.FloatingLeg, INFL);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
	  }

	  public virtual void test_builder()
	  {
		ImmutableFixedInflationSwapConvention test = ImmutableFixedInflationSwapConvention.builder().name(NAME).fixedLeg(FIXED).floatingLeg(INFL).spotDateOffset(PLUS_ONE_DAY).build();
		assertEquals(test.Name, NAME);
		assertEquals(test.FixedLeg, FIXED);
		assertEquals(test.FloatingLeg, INFL);
		assertEquals(test.SpotDateOffset, PLUS_ONE_DAY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toTrade_dates()
	  {
		ImmutableFixedInflationSwapConvention @base = ImmutableFixedInflationSwapConvention.of(NAME, FIXED, INFL, PLUS_ONE_DAY);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate = date(2015, 8, 5);
		LocalDate endDate = date(2017, 8, 5);
		SwapTrade test = @base.toTrade(tradeDate, startDate, endDate, BUY, NOTIONAL_2M, 0.25d);
		Swap expected = Swap.of(FIXED.toLeg(startDate, endDate, PAY, NOTIONAL_2M, 0.25d), INFL.toLeg(startDate, endDate, RECEIVE, NOTIONAL_2M));
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
			new object[] {FixedInflationSwapConventions.GBP_FIXED_ZC_GB_HCIP, "GBP-FIXED-ZC-GB-HCIP"},
			new object[] {FixedInflationSwapConventions.USD_FIXED_ZC_US_CPI, "USD-FIXED-ZC-US-CPI"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(FixedInflationSwapConvention convention, String name)
	  public virtual void test_name(FixedInflationSwapConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(FixedInflationSwapConvention convention, String name)
	  public virtual void test_toString(FixedInflationSwapConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(FixedInflationSwapConvention convention, String name)
	  public virtual void test_of_lookup(FixedInflationSwapConvention convention, string name)
	  {
		assertEquals(FixedInflationSwapConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(FixedInflationSwapConvention convention, String name)
	  public virtual void test_extendedEnum(FixedInflationSwapConvention convention, string name)
	  {
		FixedInflationSwapConvention.of(name); // ensures map is populated
		ImmutableMap<string, FixedInflationSwapConvention> map = FixedInflationSwapConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => FixedInflationSwapConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => FixedInflationSwapConvention.of((string) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableFixedInflationSwapConvention test = ImmutableFixedInflationSwapConvention.of(NAME, FIXED, INFL, PLUS_ONE_DAY);
		coverImmutableBean(test);
		ImmutableFixedInflationSwapConvention test2 = ImmutableFixedInflationSwapConvention.of(NAME, FIXED2, INFL2, PLUS_ONE_DAY);
		coverBeanEquals(test, test2);
		ImmutableFixedInflationSwapConvention test3 = ImmutableFixedInflationSwapConvention.of(NAME, FIXED, INFL3, PLUS_ONE_DAY);
		coverBeanEquals(test, test3);
	  }

	  public virtual void test_serialization()
	  {
		FixedInflationSwapConvention test = ImmutableFixedInflationSwapConvention.of(NAME, FIXED, INFL, PLUS_ONE_DAY);
		assertSerialization(test);
	  }

	  // Create a zero-coupon fixed leg convention
	  private static FixedRateSwapLegConvention fixedLegZcConvention(Currency ccy, HolidayCalendarId cal)
	  {
		return FixedRateSwapLegConvention.builder().paymentFrequency(Frequency.TERM).accrualFrequency(Frequency.P12M).accrualBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, cal)).startDateBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, cal)).endDateBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, cal)).compoundingMethod(CompoundingMethod.STRAIGHT).dayCount(ONE_ONE).currency(ccy).build();
	  }

	}

}