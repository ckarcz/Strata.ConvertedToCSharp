/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using OvernightIndices = com.opengamma.strata.basics.index.OvernightIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// Test <seealso cref="OvernightIborSwapConventions"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightIborSwapConventionsTest
	public class OvernightIborSwapConventionsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "spotLag") public static Object[][] data_spot_lag()
	  public static object[][] data_spot_lag()
	  {
		return new object[][]
		{
			new object[] {OvernightIborSwapConventions.USD_FED_FUND_AA_LIBOR_3M, 2},
			new object[] {OvernightIborSwapConventions.GBP_SONIA_OIS_1Y_LIBOR_3M, 0}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "spotLag") public void test_spot_lag(ImmutableOvernightIborSwapConvention convention, int lag)
	  public virtual void test_spot_lag(ImmutableOvernightIborSwapConvention convention, int lag)
	  {
		assertEquals(convention.SpotDateOffset.Days, lag);
		assertEquals(convention.SpotDateOffset, convention.IborLeg.Index.EffectiveDateOffset);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "periodOn") public static Object[][] data_period_on()
	  public static object[][] data_period_on()
	  {
		return new object[][]
		{
			new object[] {OvernightIborSwapConventions.USD_FED_FUND_AA_LIBOR_3M, Frequency.P3M},
			new object[] {OvernightIborSwapConventions.GBP_SONIA_OIS_1Y_LIBOR_3M, Frequency.P12M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "periodOn") public void test_accrualPeriod_on(OvernightIborSwapConvention convention, com.opengamma.strata.basics.schedule.Frequency frequency)
	  public virtual void test_accrualPeriod_on(OvernightIborSwapConvention convention, Frequency frequency)
	  {
		assertEquals(convention.OvernightLeg.AccrualFrequency, frequency);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "periodOn") public void test_paymentPeriod_on(OvernightIborSwapConvention convention, com.opengamma.strata.basics.schedule.Frequency frequency)
	  public virtual void test_paymentPeriod_on(OvernightIborSwapConvention convention, Frequency frequency)
	  {
		assertEquals(convention.OvernightLeg.PaymentFrequency, frequency);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "periodIbor") public static Object[][] data_period_ibor()
	  public static object[][] data_period_ibor()
	  {
		return new object[][]
		{
			new object[] {OvernightIborSwapConventions.USD_FED_FUND_AA_LIBOR_3M, Frequency.P3M},
			new object[] {OvernightIborSwapConventions.GBP_SONIA_OIS_1Y_LIBOR_3M, Frequency.P3M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "periodIbor") public void test_accrualPeriod_ibor(OvernightIborSwapConvention convention, com.opengamma.strata.basics.schedule.Frequency frequency)
	  public virtual void test_accrualPeriod_ibor(OvernightIborSwapConvention convention, Frequency frequency)
	  {
		assertEquals(convention.IborLeg.AccrualFrequency, frequency);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "periodIbor") public void test_paymentPeriod_ibor(OvernightIborSwapConvention convention, com.opengamma.strata.basics.schedule.Frequency frequency)
	  public virtual void test_paymentPeriod_ibor(OvernightIborSwapConvention convention, Frequency frequency)
	  {
		assertEquals(convention.IborLeg.PaymentFrequency, frequency);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "dayCount") public static Object[][] data_day_count()
	  public static object[][] data_day_count()
	  {
		return new object[][]
		{
			new object[] {OvernightIborSwapConventions.USD_FED_FUND_AA_LIBOR_3M, DayCounts.ACT_360},
			new object[] {OvernightIborSwapConventions.GBP_SONIA_OIS_1Y_LIBOR_3M, DayCounts.ACT_365F}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "dayCount") public void test_day_count(OvernightIborSwapConvention convention, com.opengamma.strata.basics.date.DayCount dayCount)
	  public virtual void test_day_count(OvernightIborSwapConvention convention, DayCount dayCount)
	  {
		assertEquals(convention.OvernightLeg.DayCount, dayCount);
		assertEquals(convention.IborLeg.DayCount, dayCount);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "onLeg") public static Object[][] data_float_leg()
	  public static object[][] data_float_leg()
	  {
		return new object[][]
		{
			new object[] {OvernightIborSwapConventions.USD_FED_FUND_AA_LIBOR_3M, OvernightIndices.USD_FED_FUND},
			new object[] {OvernightIborSwapConventions.GBP_SONIA_OIS_1Y_LIBOR_3M, OvernightIndices.GBP_SONIA}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "onLeg") public void test_float_leg(OvernightIborSwapConvention convention, com.opengamma.strata.basics.index.OvernightIndex floatLeg)
	  public virtual void test_float_leg(OvernightIborSwapConvention convention, OvernightIndex floatLeg)
	  {
		assertEquals(convention.OvernightLeg.Index, floatLeg);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "iborLeg") public static Object[][] data_ibor_leg()
	  public static object[][] data_ibor_leg()
	  {
		return new object[][]
		{
			new object[] {OvernightIborSwapConventions.USD_FED_FUND_AA_LIBOR_3M, IborIndices.USD_LIBOR_3M},
			new object[] {OvernightIborSwapConventions.GBP_SONIA_OIS_1Y_LIBOR_3M, IborIndices.GBP_LIBOR_3M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "iborLeg") public void test_ibor_leg(OvernightIborSwapConvention convention, com.opengamma.strata.basics.index.IborIndex iborLeg)
	  public virtual void test_ibor_leg(OvernightIborSwapConvention convention, IborIndex iborLeg)
	  {
		assertEquals(convention.IborLeg.Index, iborLeg);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "dayConvention") public static Object[][] data_day_convention()
	  public static object[][] data_day_convention()
	  {
		return new object[][]
		{
			new object[] {OvernightIborSwapConventions.USD_FED_FUND_AA_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {OvernightIborSwapConventions.GBP_SONIA_OIS_1Y_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "dayConvention") public void test_day_convention(OvernightIborSwapConvention convention, com.opengamma.strata.basics.date.BusinessDayConvention dayConvention)
	  public virtual void test_day_convention(OvernightIborSwapConvention convention, BusinessDayConvention dayConvention)
	  {
		assertEquals(convention.OvernightLeg.AccrualBusinessDayAdjustment.Convention, dayConvention);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "stubOn") public static Object[][] data_stub_on()
	  public static object[][] data_stub_on()
	  {
		return new object[][]
		{
			new object[] {OvernightIborSwapConventions.USD_FED_FUND_AA_LIBOR_3M, Tenor.TENOR_4M},
			new object[] {OvernightIborSwapConventions.GBP_SONIA_OIS_1Y_LIBOR_3M, Tenor.of(Period.ofMonths(13))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "stubOn") public void test_stub_overnight(OvernightIborSwapConvention convention, com.opengamma.strata.basics.date.Tenor tenor)
	  public virtual void test_stub_overnight(OvernightIborSwapConvention convention, Tenor tenor)
	  {
		LocalDate tradeDate = LocalDate.of(2015, 10, 20);
		SwapTrade swap = convention.createTrade(tradeDate, tenor, BuySell.BUY, 1, 0.01, REF_DATA);
		ResolvedSwap swapResolved = swap.Product.resolve(REF_DATA);
		LocalDate endDate = swapResolved.getLeg(PayReceive.PAY).get().EndDate;
		assertTrue(endDate.isAfter(tradeDate.plus(tenor).minusDays(7)));
		assertTrue(endDate.isBefore(tradeDate.plus(tenor).plusDays(7)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(OvernightIborSwapConventions));
		coverPrivateConstructor(typeof(StandardOvernightIborSwapConventions));
	  }

	}

}