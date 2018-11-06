/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// Test <seealso cref="FixedIborSwapConventions"/>.
	/// <para>
	/// These tests  match the table 18.1 in the following guide:
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedIborSwapConventionsTest
	public class FixedIborSwapConventionsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "spotLag") public static Object[][] data_spot_lag()
	  public static object[][] data_spot_lag()
	  {
		return new object[][]
		{
			new object[] {FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M, 2},
			new object[] {FixedIborSwapConventions.USD_FIXED_1Y_LIBOR_3M, 2},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M, 2},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M, 2},
			new object[] {FixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M, 0},
			new object[] {FixedIborSwapConventions.GBP_FIXED_6M_LIBOR_6M, 0},
			new object[] {FixedIborSwapConventions.GBP_FIXED_3M_LIBOR_3M, 0},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_TIBORJ_3M, 2},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_LIBOR_6M, 2},
			new object[] {FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_3M, 2},
			new object[] {FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_6M, 2}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "spotLag") public void test_spot_lag(ImmutableFixedIborSwapConvention convention, int lag)
	  public virtual void test_spot_lag(ImmutableFixedIborSwapConvention convention, int lag)
	  {
		assertEquals(convention.SpotDateOffset.Days, lag);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "period") public static Object[][] data_period()
	  public static object[][] data_period()
	  {
		return new object[][]
		{
			new object[] {FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M, Frequency.P6M},
			new object[] {FixedIborSwapConventions.USD_FIXED_1Y_LIBOR_3M, Frequency.P12M},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M, Frequency.P12M},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M, Frequency.P12M},
			new object[] {FixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M, Frequency.P12M},
			new object[] {FixedIborSwapConventions.GBP_FIXED_6M_LIBOR_6M, Frequency.P6M},
			new object[] {FixedIborSwapConventions.GBP_FIXED_3M_LIBOR_3M, Frequency.P3M},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_TIBORJ_3M, Frequency.P6M},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_LIBOR_6M, Frequency.P6M},
			new object[] {FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_3M, Frequency.P12M},
			new object[] {FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_6M, Frequency.P12M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "period") public void test_period(FixedIborSwapConvention convention, com.opengamma.strata.basics.schedule.Frequency frequency)
	  public virtual void test_period(FixedIborSwapConvention convention, Frequency frequency)
	  {
		assertEquals(convention.FixedLeg.AccrualFrequency, frequency);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "dayCount") public static Object[][] data_day_count()
	  public static object[][] data_day_count()
	  {
		return new object[][]
		{
			new object[] {FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M, DayCounts.THIRTY_U_360},
			new object[] {FixedIborSwapConventions.USD_FIXED_1Y_LIBOR_3M, DayCounts.ACT_360},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M, DayCounts.THIRTY_U_360},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M, DayCounts.THIRTY_U_360},
			new object[] {FixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M, DayCounts.ACT_365F},
			new object[] {FixedIborSwapConventions.GBP_FIXED_6M_LIBOR_6M, DayCounts.ACT_365F},
			new object[] {FixedIborSwapConventions.GBP_FIXED_3M_LIBOR_3M, DayCounts.ACT_365F},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_TIBORJ_3M, DayCounts.ACT_365F},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_LIBOR_6M, DayCounts.ACT_365F},
			new object[] {FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_3M, DayCounts.THIRTY_U_360},
			new object[] {FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_6M, DayCounts.THIRTY_U_360}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "dayCount") public void test_day_count(FixedIborSwapConvention convention, com.opengamma.strata.basics.date.DayCount dayCount)
	  public virtual void test_day_count(FixedIborSwapConvention convention, DayCount dayCount)
	  {
		assertEquals(convention.FixedLeg.DayCount, dayCount);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "floatLeg") public static Object[][] data_float_leg()
	  public static object[][] data_float_leg()
	  {
		return new object[][]
		{
			new object[] {FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M, IborIndices.USD_LIBOR_3M},
			new object[] {FixedIborSwapConventions.USD_FIXED_1Y_LIBOR_3M, IborIndices.USD_LIBOR_3M},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M, IborIndices.EUR_EURIBOR_3M},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M, IborIndices.EUR_EURIBOR_6M},
			new object[] {FixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M, IborIndices.GBP_LIBOR_3M},
			new object[] {FixedIborSwapConventions.GBP_FIXED_6M_LIBOR_6M, IborIndices.GBP_LIBOR_6M},
			new object[] {FixedIborSwapConventions.GBP_FIXED_3M_LIBOR_3M, IborIndices.GBP_LIBOR_3M},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_TIBORJ_3M, IborIndices.JPY_TIBOR_JAPAN_3M},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_LIBOR_6M, IborIndices.JPY_LIBOR_6M},
			new object[] {FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_3M, IborIndices.CHF_LIBOR_3M},
			new object[] {FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_6M, IborIndices.CHF_LIBOR_6M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "floatLeg") public void test_float_leg(FixedIborSwapConvention convention, com.opengamma.strata.basics.index.IborIndex floatLeg)
	  public virtual void test_float_leg(FixedIborSwapConvention convention, IborIndex floatLeg)
	  {
		assertEquals(convention.FloatingLeg.Index, floatLeg);
	  }

	  // For vanilla swaps the holidays calendars on the fixed leg should be
	  // consistent with the maturity calendars on the floating leg
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "floatLeg") public void test_holiday_calendars_match(FixedIborSwapConvention convention, com.opengamma.strata.basics.index.IborIndex floatLeg)
	  public virtual void test_holiday_calendars_match(FixedIborSwapConvention convention, IborIndex floatLeg)
	  {
		assertEquals(convention.FixedLeg.AccrualBusinessDayAdjustment.Calendar, floatLeg.MaturityDateOffset.Adjustment.Calendar);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "dayConvention") public static Object[][] data_day_convention()
	  public static object[][] data_day_convention()
	  {
		return new object[][]
		{
			new object[] {FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedIborSwapConventions.USD_FIXED_1Y_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedIborSwapConventions.GBP_FIXED_6M_LIBOR_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedIborSwapConventions.GBP_FIXED_3M_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_TIBORJ_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_LIBOR_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_6M, BusinessDayConventions.MODIFIED_FOLLOWING}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "dayConvention") public void test_day_convention(FixedIborSwapConvention convention, com.opengamma.strata.basics.date.BusinessDayConvention dayConvention)
	  public virtual void test_day_convention(FixedIborSwapConvention convention, BusinessDayConvention dayConvention)
	  {
		assertEquals(convention.FixedLeg.AccrualBusinessDayAdjustment.Convention, dayConvention);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "stubIbor") public static Object[][] data_stub_ibor()
	  public static object[][] data_stub_ibor()
	  {
		return new object[][]
		{
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M, Tenor.TENOR_18M},
			new object[] {FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M, Tenor.TENOR_18M},
			new object[] {FixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M, Tenor.TENOR_18M},
			new object[] {FixedIborSwapConventions.GBP_FIXED_6M_LIBOR_6M, Tenor.TENOR_9M},
			new object[] {FixedIborSwapConventions.GBP_FIXED_3M_LIBOR_3M, Tenor.TENOR_10M},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_TIBORJ_3M, Tenor.TENOR_9M},
			new object[] {FixedIborSwapConventions.JPY_FIXED_6M_TIBORJ_3M, Tenor.TENOR_9M},
			new object[] {FixedIborSwapConventions.USD_FIXED_1Y_LIBOR_3M, Tenor.TENOR_18M},
			new object[] {FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M, Tenor.TENOR_9M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "stubIbor") public void test_stub_ibor(FixedIborSwapConvention convention, com.opengamma.strata.basics.date.Tenor tenor)
	  public virtual void test_stub_ibor(FixedIborSwapConvention convention, Tenor tenor)
	  {
		LocalDate tradeDate = LocalDate.of(2015, 10, 20);
		SwapTrade swap = convention.createTrade(tradeDate, tenor, BuySell.BUY, 1, 0.01, REF_DATA);
		ResolvedSwap swapResolved = swap.Product.resolve(REF_DATA);
		LocalDate endDate = swapResolved.getLeg(PayReceive.PAY).get().EndDate;
		assertTrue(endDate.isAfter(tradeDate.plus(tenor).minusMonths(1)));
		assertTrue(endDate.isBefore(tradeDate.plus(tenor).plusMonths(1)));
	  }

	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(FixedIborSwapConventions));
		coverPrivateConstructor(typeof(StandardFixedIborSwapConventions));
	  }

	}

}