/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;

	/// <summary>
	/// Test <seealso cref="XCcyIborIborSwapConventions"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class XCcyIborIborSwapConventionsTest
	public class XCcyIborIborSwapConventionsTest
	{

	  private static readonly HolidayCalendarId EUTA_USNY = EUTA.combinedWith(USNY);
	  private static readonly HolidayCalendarId GBLO_USNY = GBLO.combinedWith(USNY);
	  private static readonly HolidayCalendarId EUTA_GBLO = EUTA.combinedWith(GBLO);
	  private static readonly HolidayCalendarId GBLO_JPTO = GBLO.combinedWith(JPTO);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "spotLag") public static Object[][] data_spot_lag()
	  public static object[][] data_spot_lag()
	  {
		return new object[][]
		{
			new object[] {XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M, 2},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_USD_LIBOR_3M, 2},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_EUR_EURIBOR_3M, 2}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "spotLag") public void test_spot_lag(ImmutableXCcyIborIborSwapConvention convention, int lag)
	  public virtual void test_spot_lag(ImmutableXCcyIborIborSwapConvention convention, int lag)
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
			new object[] {XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M, Frequency.P3M},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_USD_LIBOR_3M, Frequency.P3M},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_EUR_EURIBOR_3M, Frequency.P3M},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_JPY_LIBOR_3M, Frequency.P3M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "period") public void test_period(XCcyIborIborSwapConvention convention, com.opengamma.strata.basics.schedule.Frequency frequency)
	  public virtual void test_period(XCcyIborIborSwapConvention convention, Frequency frequency)
	  {
		assertEquals(convention.SpreadLeg.PaymentFrequency, frequency);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "spreadLegIndex") public static Object[][] data_spread_leg()
	  public static object[][] data_spread_leg()
	  {
		return new object[][]
		{
			new object[] {XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M, IborIndices.EUR_EURIBOR_3M},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_USD_LIBOR_3M, IborIndices.GBP_LIBOR_3M},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_EUR_EURIBOR_3M, IborIndices.GBP_LIBOR_3M},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_JPY_LIBOR_3M, IborIndices.GBP_LIBOR_3M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "spreadLegIndex") public void test_float_leg(XCcyIborIborSwapConvention convention, com.opengamma.strata.basics.index.IborIndex index)
	  public virtual void test_float_leg(XCcyIborIborSwapConvention convention, IborIndex index)
	  {
		assertEquals(convention.SpreadLeg.Index, index);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "spreadLegBda") public static Object[][] data_spread_leg_bda()
	  public static object[][] data_spread_leg_bda()
	  {
		return new object[][]
		{
			new object[] {XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA_USNY)},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_USD_LIBOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_EUR_EURIBOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA_GBLO)},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_JPY_LIBOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_JPTO)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "spreadLegBda") public void test_spread_leg_bdc(XCcyIborIborSwapConvention convention, com.opengamma.strata.basics.date.BusinessDayAdjustment bda)
	  public virtual void test_spread_leg_bdc(XCcyIborIborSwapConvention convention, BusinessDayAdjustment bda)
	  {
		assertEquals(convention.SpreadLeg.AccrualBusinessDayAdjustment, bda);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "flatLegIndex") public static Object[][] data_flat_leg()
	  public static object[][] data_flat_leg()
	  {
		return new object[][]
		{
			new object[] {XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M, IborIndices.USD_LIBOR_3M},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_USD_LIBOR_3M, IborIndices.USD_LIBOR_3M},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_EUR_EURIBOR_3M, IborIndices.EUR_EURIBOR_3M},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_JPY_LIBOR_3M, IborIndices.JPY_LIBOR_3M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "flatLegIndex") public void test_flat_leg(XCcyIborIborSwapConvention convention, com.opengamma.strata.basics.index.IborIndex index)
	  public virtual void test_flat_leg(XCcyIborIborSwapConvention convention, IborIndex index)
	  {
		assertEquals(convention.FlatLeg.Index, index);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "flatLegBda") public static Object[][] data_flat_leg_bda()
	  public static object[][] data_flat_leg_bda()
	  {
		return new object[][]
		{
			new object[] {XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA_USNY)},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_USD_LIBOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_EUR_EURIBOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA_GBLO)},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_JPY_LIBOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_JPTO)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "flatLegBda") public void test_flat_leg_bdc(XCcyIborIborSwapConvention convention, com.opengamma.strata.basics.date.BusinessDayAdjustment bda)
	  public virtual void test_flat_leg_bdc(XCcyIborIborSwapConvention convention, BusinessDayAdjustment bda)
	  {
		assertEquals(convention.FlatLeg.AccrualBusinessDayAdjustment, bda);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "dayConvention") public static Object[][] data_day_convention()
	  public static object[][] data_day_convention()
	  {
		return new object[][]
		{
			new object[] {XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_USD_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_EUR_EURIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_JPY_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "dayConvention") public void test_day_convention(XCcyIborIborSwapConvention convention, com.opengamma.strata.basics.date.BusinessDayConvention dayConvention)
	  public virtual void test_day_convention(XCcyIborIborSwapConvention convention, BusinessDayConvention dayConvention)
	  {
		assertEquals(convention.SpreadLeg.AccrualBusinessDayAdjustment.Convention, dayConvention);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "notionalExchange") public static Object[][] data_notional_exchange()
	  public static object[][] data_notional_exchange()
	  {
		return new object[][]
		{
			new object[] {XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M, true},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_USD_LIBOR_3M, true},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_EUR_EURIBOR_3M, true},
			new object[] {XCcyIborIborSwapConventions.GBP_LIBOR_3M_EUR_EURIBOR_3M, true}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "notionalExchange") public void test_notional_exchange(XCcyIborIborSwapConvention convention, boolean notionalExchange)
	  public virtual void test_notional_exchange(XCcyIborIborSwapConvention convention, bool notionalExchange)
	  {
		assertEquals(convention.SpreadLeg.NotionalExchange, notionalExchange);
		assertEquals(convention.FlatLeg.NotionalExchange, notionalExchange);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(XCcyIborIborSwapConventions));
		coverPrivateConstructor(typeof(StandardXCcyIborIborSwapConventions));
	  }

	}

}