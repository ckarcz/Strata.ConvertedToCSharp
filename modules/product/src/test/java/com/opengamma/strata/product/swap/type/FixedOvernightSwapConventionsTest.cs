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
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using OvernightIndices = com.opengamma.strata.basics.index.OvernightIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// Test <seealso cref="FixedOvernightSwapConventions"/>.
	/// <para>
	/// These tests  match the table 18.1 in the following guide:
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedOvernightSwapConventionsTest
	public class FixedOvernightSwapConventionsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "spotLag") public static Object[][] data_spot_lag()
	  public static object[][] data_spot_lag()
	  {
		return new object[][]
		{
			new object[] {FixedOvernightSwapConventions.USD_FIXED_TERM_FED_FUND_OIS, 2},
			new object[] {FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS, 2},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_TERM_EONIA_OIS, 2},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_1Y_EONIA_OIS, 2},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_TERM_SONIA_OIS, 0},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_1Y_SONIA_OIS, 0},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_TERM_TONAR_OIS, 0},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_1Y_TONAR_OIS, 2}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "spotLag") public void test_spot_lag(ImmutableFixedOvernightSwapConvention convention, int lag)
	  public virtual void test_spot_lag(ImmutableFixedOvernightSwapConvention convention, int lag)
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
			new object[] {FixedOvernightSwapConventions.USD_FIXED_TERM_FED_FUND_OIS, Frequency.TERM},
			new object[] {FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS, Frequency.P12M},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_TERM_EONIA_OIS, Frequency.TERM},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_1Y_EONIA_OIS, Frequency.P12M},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_TERM_SONIA_OIS, Frequency.TERM},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_1Y_SONIA_OIS, Frequency.P12M},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_TERM_TONAR_OIS, Frequency.TERM},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_1Y_TONAR_OIS, Frequency.P12M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "period") public void test_accrualPeriod(FixedOvernightSwapConvention convention, com.opengamma.strata.basics.schedule.Frequency frequency)
	  public virtual void test_accrualPeriod(FixedOvernightSwapConvention convention, Frequency frequency)
	  {
		assertEquals(convention.FixedLeg.AccrualFrequency, frequency);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "period") public void test_paymentPeriod(FixedOvernightSwapConvention convention, com.opengamma.strata.basics.schedule.Frequency frequency)
	  public virtual void test_paymentPeriod(FixedOvernightSwapConvention convention, Frequency frequency)
	  {
		assertEquals(convention.FixedLeg.PaymentFrequency, frequency);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "dayCount") public static Object[][] data_day_count()
	  public static object[][] data_day_count()
	  {
		return new object[][]
		{
			new object[] {FixedOvernightSwapConventions.USD_FIXED_TERM_FED_FUND_OIS, DayCounts.ACT_360},
			new object[] {FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS, DayCounts.ACT_360},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_TERM_EONIA_OIS, DayCounts.ACT_360},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_1Y_EONIA_OIS, DayCounts.ACT_360},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_TERM_SONIA_OIS, DayCounts.ACT_365F},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_1Y_SONIA_OIS, DayCounts.ACT_365F},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_TERM_TONAR_OIS, DayCounts.ACT_365F},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_1Y_TONAR_OIS, DayCounts.ACT_365F}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "dayCount") public void test_day_count(FixedOvernightSwapConvention convention, com.opengamma.strata.basics.date.DayCount dayCount)
	  public virtual void test_day_count(FixedOvernightSwapConvention convention, DayCount dayCount)
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
			new object[] {FixedOvernightSwapConventions.USD_FIXED_TERM_FED_FUND_OIS, OvernightIndices.USD_FED_FUND},
			new object[] {FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS, OvernightIndices.USD_FED_FUND},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_TERM_EONIA_OIS, OvernightIndices.EUR_EONIA},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_1Y_EONIA_OIS, OvernightIndices.EUR_EONIA},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_TERM_SONIA_OIS, OvernightIndices.GBP_SONIA},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_1Y_SONIA_OIS, OvernightIndices.GBP_SONIA},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_TERM_TONAR_OIS, OvernightIndices.JPY_TONAR},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_1Y_TONAR_OIS, OvernightIndices.JPY_TONAR}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "floatLeg") public void test_float_leg(FixedOvernightSwapConvention convention, com.opengamma.strata.basics.index.OvernightIndex floatLeg)
	  public virtual void test_float_leg(FixedOvernightSwapConvention convention, OvernightIndex floatLeg)
	  {
		assertEquals(convention.FloatingLeg.Index, floatLeg);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "dayConvention") public static Object[][] data_day_convention()
	  public static object[][] data_day_convention()
	  {
		return new object[][]
		{
			new object[] {FixedOvernightSwapConventions.USD_FIXED_TERM_FED_FUND_OIS, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_TERM_EONIA_OIS, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_1Y_EONIA_OIS, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_TERM_SONIA_OIS, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_1Y_SONIA_OIS, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_TERM_TONAR_OIS, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_1Y_TONAR_OIS, BusinessDayConventions.MODIFIED_FOLLOWING}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "dayConvention") public void test_day_convention(FixedOvernightSwapConvention convention, com.opengamma.strata.basics.date.BusinessDayConvention dayConvention)
	  public virtual void test_day_convention(FixedOvernightSwapConvention convention, BusinessDayConvention dayConvention)
	  {
		assertEquals(convention.FixedLeg.AccrualBusinessDayAdjustment.Convention, dayConvention);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "stubOn") public static Object[][] data_stub_on()
	  public static object[][] data_stub_on()
	  {
		return new object[][]
		{
			new object[] {FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS, Tenor.TENOR_18M},
			new object[] {FixedOvernightSwapConventions.EUR_FIXED_1Y_EONIA_OIS, Tenor.TENOR_18M},
			new object[] {FixedOvernightSwapConventions.GBP_FIXED_1Y_SONIA_OIS, Tenor.TENOR_18M},
			new object[] {FixedOvernightSwapConventions.JPY_FIXED_1Y_TONAR_OIS, Tenor.TENOR_18M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "stubOn") public void test_stub_overnight(FixedOvernightSwapConvention convention, com.opengamma.strata.basics.date.Tenor tenor)
	  public virtual void test_stub_overnight(FixedOvernightSwapConvention convention, Tenor tenor)
	  {
		LocalDate tradeDate = LocalDate.of(2015, 10, 20);
		SwapTrade swap = convention.createTrade(tradeDate, tenor, BuySell.BUY, 1, 0.01, REF_DATA);
		ResolvedSwap swapResolved = swap.Product.resolve(REF_DATA);
		LocalDate endDate = swapResolved.getLeg(PayReceive.PAY).get().EndDate;
		assertTrue(endDate.isAfter(tradeDate.plus(tenor).minusMonths(1)));
		assertTrue(endDate.isBefore(tradeDate.plus(tenor).plusMonths(1)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(FixedOvernightSwapConventions));
		coverPrivateConstructor(typeof(StandardFixedOvernightSwapConventions));
	  }

	}

}