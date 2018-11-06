/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_364;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365L;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365_25;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365_ACTUAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_AFB;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ICMA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_YEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.NL_365;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ONE_ONE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_360_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_360_PSA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_EPLUS_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_E_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_E_360_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360_EOM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P12M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsRuntime;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;

	/// <summary>
	/// Test <seealso cref="DayCount"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DayCountTest
	public class DayCountTest
	{

	  private static readonly LocalDate JAN_01 = LocalDate.of(2010, 1, 1);
	  private static readonly LocalDate JAN_02 = LocalDate.of(2010, 1, 2);
	  private static readonly LocalDate JUL_01 = LocalDate.of(2010, 7, 1);
	  private static readonly LocalDate JAN_01_NEXT = LocalDate.of(2011, 1, 1);

	  private const double TOLERANCE_ZERO = 0d;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "types") public static Object[][] data_types()
	  public static object[][] data_types()
	  {
		StandardDayCounts[] conv = StandardDayCounts.values();
		object[][] result = new object[conv.Length][];
		for (int i = 0; i < conv.Length; i++)
		{
		  result[i] = new object[] {conv[i]};
		}
		return result;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "types") public void test_null(DayCount type)
	  public virtual void test_null(DayCount type)
	  {
		assertThrowsRuntime(() => type.yearFraction(null, JAN_01));
		assertThrowsRuntime(() => type.yearFraction(JAN_01, null));
		assertThrowsRuntime(() => type.yearFraction(null, null));
		assertThrowsRuntime(() => type.days(null, JAN_01));
		assertThrowsRuntime(() => type.days(JAN_01, null));
		assertThrowsRuntime(() => type.days(null, null));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "types") public void test_wrongOrder(DayCount type)
	  public virtual void test_wrongOrder(DayCount type)
	  {
		assertThrowsIllegalArg(() => type.yearFraction(JAN_02, JAN_01));
		assertThrowsIllegalArg(() => type.days(JAN_02, JAN_01));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "types") public void test_same(DayCount type)
	  public virtual void test_same(DayCount type)
	  {
		if (type != ONE_ONE)
		{
		  assertEquals(type.yearFraction(JAN_02, JAN_02), 0d, TOLERANCE_ZERO);
		  assertEquals(type.days(JAN_02, JAN_02), 0);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "types") public void test_halfYear(DayCount type)
	  public virtual void test_halfYear(DayCount type)
	  {
		// sanity check to ensure that half year has fraction close to half
		if (type != ONE_ONE)
		{
		  DayCount_ScheduleInfo info = new Info(JAN_01, JAN_01_NEXT, JAN_01_NEXT, false, P12M);
		  assertEquals(type.yearFraction(JAN_01, JUL_01, info), 0.5d, 0.01d);
		  assertEquals(type.days(JAN_01, JUL_01), 182, 2);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "types") public void test_wholeYear(DayCount type)
	  public virtual void test_wholeYear(DayCount type)
	  {
		// sanity check to ensure that one year has fraction close to one
		if (type != ONE_ONE)
		{
		  DayCount_ScheduleInfo info = new Info(JAN_01, JAN_01_NEXT, JAN_01_NEXT, false, P12M);
		  assertEquals(type.yearFraction(JAN_01, JAN_01_NEXT, info), 1d, 0.02d);
		  assertEquals(type.days(JAN_01, JAN_01_NEXT), 365, 5);
		}
	  }

	  //-------------------------------------------------------------------------
	  // use flag to make it clearer when an adjustment is happening
	  private static double? SIMPLE_30_360 = Double.NaN;

	  private static int SIMPLE_30_360Days = 0;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "yearFraction") public static Object[][] data_yearFraction()
	  public static object[][] data_yearFraction()
	  {
		return new object[][]
		{
			new object[] {ONE_ONE, 2011, 12, 28, 2012, 2, 28, 1d},
			new object[] {ONE_ONE, 2011, 12, 28, 2012, 2, 29, 1d},
			new object[] {ONE_ONE, 2011, 12, 28, 2012, 3, 1, 1d},
			new object[] {ONE_ONE, 2011, 12, 28, 2016, 2, 28, 1d},
			new object[] {ONE_ONE, 2011, 12, 28, 2016, 2, 29, 1d},
			new object[] {ONE_ONE, 2011, 12, 28, 2016, 3, 1, 1d},
			new object[] {ONE_ONE, 2012, 2, 29, 2012, 3, 29, 1d},
			new object[] {ONE_ONE, 2012, 2, 29, 2012, 3, 28, 1d},
			new object[] {ONE_ONE, 2012, 3, 1, 2012, 3, 28, 1d},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2012, 2, 28, (4d / 365d + 58d / 366d)},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2012, 2, 29, (4d / 365d + 59d / 366d)},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2012, 3, 1, (4d / 365d + 60d / 366d)},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2016, 2, 28, (4d / 365d + 58d / 366d + 4)},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2016, 2, 29, (4d / 365d + 59d / 366d + 4)},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2016, 3, 1, (4d / 365d + 60d / 366d + 4)},
			new object[] {ACT_ACT_ISDA, 2012, 2, 29, 2012, 3, 29, 29d / 366d},
			new object[] {ACT_ACT_ISDA, 2012, 2, 29, 2012, 3, 28, 28d / 366d},
			new object[] {ACT_ACT_ISDA, 2012, 3, 1, 2012, 3, 28, 27d / 366d},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2012, 2, 28, (62d / 365d)},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2012, 2, 29, (63d / 365d)},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2012, 3, 1, (64d / 366d)},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2016, 2, 28, (62d / 365d) + 4},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2016, 2, 29, (63d / 365d) + 4},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2016, 3, 1, (64d / 366d) + 4},
			new object[] {ACT_ACT_AFB, 2012, 2, 28, 2012, 3, 28, 29d / 366d},
			new object[] {ACT_ACT_AFB, 2012, 2, 29, 2012, 3, 28, 28d / 366d},
			new object[] {ACT_ACT_AFB, 2012, 3, 1, 2012, 3, 28, 27d / 365d},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2012, 2, 28, (62d / 366d)},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2012, 2, 29, (63d / 366d)},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2012, 3, 1, (64d / 366d)},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2016, 2, 28, (62d / 366d) + 4},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2016, 2, 29, (63d / 366d) + 4},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2016, 3, 1, (64d / 366d) + 4},
			new object[] {ACT_ACT_YEAR, 2012, 2, 28, 2012, 3, 28, 29d / 366d},
			new object[] {ACT_ACT_YEAR, 2012, 2, 29, 2012, 3, 28, 28d / 365d},
			new object[] {ACT_ACT_YEAR, 2012, 3, 1, 2012, 3, 28, 27d / 365d},
			new object[] {ACT_ACT_YEAR, 2011, 2, 28, 2011, 3, 2, (2d / 365d)},
			new object[] {ACT_ACT_YEAR, 2011, 3, 1, 2011, 3, 2, (1d / 366d)},
			new object[] {ACT_ACT_YEAR, 2012, 2, 28, 2016, 3, 2, (3d / 366d) + 4},
			new object[] {ACT_ACT_YEAR, 2012, 2, 29, 2016, 3, 2, (2d / 365d) + 4},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2012, 2, 28, (62d / 365d)},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2012, 2, 29, (63d / 366d)},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2012, 3, 1, (64d / 366d)},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2016, 2, 28, ((62d + 366d + 365d + 365d + 365d) / 366d)},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2016, 2, 29, ((63d + 366d + 365d + 365d + 365d) / 366d)},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2016, 3, 1, ((64d + 366d + 365d + 365d + 365d) / 366d)},
			new object[] {ACT_365_ACTUAL, 2012, 2, 28, 2012, 3, 28, 29d / 366d},
			new object[] {ACT_365_ACTUAL, 2012, 2, 29, 2012, 3, 28, 28d / 365d},
			new object[] {ACT_365_ACTUAL, 2012, 3, 1, 2012, 3, 28, 27d / 365d},
			new object[] {ACT_360, 2011, 12, 28, 2012, 2, 28, (62d / 360d)},
			new object[] {ACT_360, 2011, 12, 28, 2012, 2, 29, (63d / 360d)},
			new object[] {ACT_360, 2011, 12, 28, 2012, 3, 1, (64d / 360d)},
			new object[] {ACT_360, 2011, 12, 28, 2016, 2, 28, ((62d + 366d + 365d + 365d + 365d) / 360d)},
			new object[] {ACT_360, 2011, 12, 28, 2016, 2, 29, ((63d + 366d + 365d + 365d + 365d) / 360d)},
			new object[] {ACT_360, 2011, 12, 28, 2016, 3, 1, ((64d + 366d + 365d + 365d + 365d) / 360d)},
			new object[] {ACT_360, 2012, 2, 28, 2012, 3, 28, 29d / 360d},
			new object[] {ACT_360, 2012, 2, 29, 2012, 3, 28, 28d / 360d},
			new object[] {ACT_360, 2012, 3, 1, 2012, 3, 28, 27d / 360d},
			new object[] {ACT_364, 2011, 12, 28, 2012, 2, 28, (62d / 364d)},
			new object[] {ACT_364, 2011, 12, 28, 2012, 2, 29, (63d / 364d)},
			new object[] {ACT_364, 2011, 12, 28, 2012, 3, 1, (64d / 364d)},
			new object[] {ACT_364, 2011, 12, 28, 2016, 2, 28, ((62d + 366d + 365d + 365d + 365d) / 364d)},
			new object[] {ACT_364, 2011, 12, 28, 2016, 2, 29, ((63d + 366d + 365d + 365d + 365d) / 364d)},
			new object[] {ACT_364, 2011, 12, 28, 2016, 3, 1, ((64d + 366d + 365d + 365d + 365d) / 364d)},
			new object[] {ACT_364, 2012, 2, 28, 2012, 3, 28, 29d / 364d},
			new object[] {ACT_364, 2012, 2, 29, 2012, 3, 28, 28d / 364d},
			new object[] {ACT_364, 2012, 3, 1, 2012, 3, 28, 27d / 364d},
			new object[] {ACT_365F, 2011, 12, 28, 2012, 2, 28, (62d / 365d)},
			new object[] {ACT_365F, 2011, 12, 28, 2012, 2, 29, (63d / 365d)},
			new object[] {ACT_365F, 2011, 12, 28, 2012, 3, 1, (64d / 365d)},
			new object[] {ACT_365F, 2011, 12, 28, 2016, 2, 28, ((62d + 366d + 365d + 365d + 365d) / 365d)},
			new object[] {ACT_365F, 2011, 12, 28, 2016, 2, 29, ((63d + 366d + 365d + 365d + 365d) / 365d)},
			new object[] {ACT_365F, 2011, 12, 28, 2016, 3, 1, ((64d + 366d + 365d + 365d + 365d) / 365d)},
			new object[] {ACT_365F, 2012, 2, 28, 2012, 3, 28, 29d / 365d},
			new object[] {ACT_365F, 2012, 2, 29, 2012, 3, 28, 28d / 365d},
			new object[] {ACT_365F, 2012, 3, 1, 2012, 3, 28, 27d / 365d},
			new object[] {ACT_365_25, 2011, 12, 28, 2012, 2, 28, (62d / 365.25d)},
			new object[] {ACT_365_25, 2011, 12, 28, 2012, 2, 29, (63d / 365.25d)},
			new object[] {ACT_365_25, 2011, 12, 28, 2012, 3, 1, (64d / 365.25d)},
			new object[] {ACT_365_25, 2011, 12, 28, 2016, 2, 28, ((62d + 366d + 365d + 365d + 365d) / 365.25d)},
			new object[] {ACT_365_25, 2011, 12, 28, 2016, 2, 29, ((63d + 366d + 365d + 365d + 365d) / 365.25d)},
			new object[] {ACT_365_25, 2011, 12, 28, 2016, 3, 1, ((64d + 366d + 365d + 365d + 365d) / 365.25d)},
			new object[] {ACT_365_25, 2012, 2, 28, 2012, 3, 28, 29d / 365.25d},
			new object[] {ACT_365_25, 2012, 2, 29, 2012, 3, 28, 28d / 365.25d},
			new object[] {ACT_365_25, 2012, 3, 1, 2012, 3, 28, 27d / 365.25d},
			new object[] {NL_365, 2011, 12, 28, 2012, 2, 28, (62d / 365d)},
			new object[] {NL_365, 2011, 12, 28, 2012, 2, 29, (62d / 365d)},
			new object[] {NL_365, 2011, 12, 28, 2012, 3, 1, (63d / 365d)},
			new object[] {NL_365, 2011, 12, 28, 2016, 2, 28, ((62d + 365d + 365d + 365d + 365d) / 365d)},
			new object[] {NL_365, 2011, 12, 28, 2016, 2, 29, ((62d + 365d + 365d + 365d + 365d) / 365d)},
			new object[] {NL_365, 2011, 12, 28, 2016, 3, 1, ((63d + 365d + 365d + 365d + 365d) / 365d)},
			new object[] {NL_365, 2012, 2, 28, 2012, 3, 28, 28d / 365d},
			new object[] {NL_365, 2012, 2, 29, 2012, 3, 28, 28d / 365d},
			new object[] {NL_365, 2012, 3, 1, 2012, 3, 28, 27d / 365d},
			new object[] {NL_365, 2011, 12, 1, 2012, 12, 1, 365d / 365d},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2012, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2012, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2012, 3, 1, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2016, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2016, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2016, 3, 1, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2012, 2, 28, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2012, 2, 29, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2011, 2, 28, 2012, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2011, 2, 28, 2012, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2012, 2, 29, 2016, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2012, 3, 1, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2012, 5, 30, 2013, 8, 29, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2012, 5, 29, 2013, 8, 30, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2012, 5, 30, 2013, 8, 30, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2012, 5, 29, 2013, 8, 31, SIMPLE_30_360},
			new object[] {THIRTY_360_ISDA, 2012, 5, 30, 2013, 8, 31, calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_360_ISDA, 2012, 5, 31, 2013, 8, 30, calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_360_ISDA, 2012, 5, 31, 2013, 8, 31, calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2012, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2012, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2012, 3, 1, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2016, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2016, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2016, 3, 1, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2012, 2, 28, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2012, 2, 29, 2012, 3, 28, calc360(2012, 2, 30, 2012, 3, 28)},
			new object[] {THIRTY_360_PSA, 2011, 2, 28, 2012, 2, 28, calc360(2011, 2, 30, 2012, 2, 28)},
			new object[] {THIRTY_360_PSA, 2011, 2, 28, 2012, 2, 29, calc360(2011, 2, 30, 2012, 2, 29)},
			new object[] {THIRTY_360_PSA, 2012, 2, 29, 2016, 2, 29, calc360(2012, 2, 30, 2016, 2, 29)},
			new object[] {THIRTY_360_PSA, 2012, 3, 1, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2012, 5, 30, 2013, 8, 29, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2012, 5, 29, 2013, 8, 30, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2012, 5, 30, 2013, 8, 30, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2012, 5, 29, 2013, 8, 31, SIMPLE_30_360},
			new object[] {THIRTY_360_PSA, 2012, 5, 30, 2013, 8, 31, calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_360_PSA, 2012, 5, 31, 2013, 8, 30, calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_360_PSA, 2012, 5, 31, 2013, 8, 31, calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2012, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2012, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2012, 3, 1, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2016, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2016, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2016, 3, 1, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2012, 2, 28, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2012, 2, 29, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2011, 2, 28, 2012, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2011, 2, 28, 2012, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2012, 2, 29, 2016, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2012, 3, 1, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2012, 5, 30, 2013, 8, 29, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2012, 5, 29, 2013, 8, 30, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2012, 5, 30, 2013, 8, 30, SIMPLE_30_360},
			new object[] {THIRTY_E_360, 2012, 5, 29, 2013, 8, 31, calc360(2012, 5, 29, 2013, 8, 30)},
			new object[] {THIRTY_E_360, 2012, 5, 30, 2013, 8, 31, calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_E_360, 2012, 5, 31, 2013, 8, 30, calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_E_360, 2012, 5, 31, 2013, 8, 31, calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2012, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2012, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2012, 3, 1, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2016, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2016, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2016, 3, 1, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2012, 2, 28, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2012, 2, 29, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2012, 3, 1, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2011, 2, 28, 2012, 2, 28, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2011, 2, 28, 2012, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2012, 2, 29, 2016, 2, 29, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2012, 3, 1, 2012, 3, 28, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 30, 2013, 8, 29, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 29, 2013, 8, 30, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 30, 2013, 8, 30, SIMPLE_30_360},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 29, 2013, 8, 31, calc360(2012, 5, 29, 2013, 9, 1)},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 30, 2013, 8, 31, calc360(2012, 5, 30, 2013, 9, 1)},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 31, 2013, 8, 30, calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 31, 2013, 8, 31, calc360(2012, 5, 30, 2013, 9, 1)}
		};
	  }

	  private static double calc360(int y1, int m1, int d1, int y2, int m2, int d2)
	  {
		return ((y2 - y1) * 360 + (m2 - m1) * 30 + (d2 - d1)) / 360d;
	  }

	  private static int calc360Days(int y1, int m1, int d1, int y2, int m2, int d2)
	  {
		return (y2 - y1) * 360 + (m2 - m1) * 30 + (d2 - d1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "yearFraction") public void test_yearFraction(DayCount dayCount, int y1, int m1, int d1, int y2, int m2, int d2, System.Nullable<double> value)
	  public virtual void test_yearFraction(DayCount dayCount, int y1, int m1, int d1, int y2, int m2, int d2, double? value)
	  {
		double expected = (value == SIMPLE_30_360 ? calc360(y1, m1, d1, y2, m2, d2) : value).Value;
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		assertEquals(dayCount.yearFraction(date1, date2), expected, TOLERANCE_ZERO);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "yearFraction") public void test_relativeYearFraction(DayCount dayCount, int y1, int m1, int d1, int y2, int m2, int d2, System.Nullable<double> value)
	  public virtual void test_relativeYearFraction(DayCount dayCount, int y1, int m1, int d1, int y2, int m2, int d2, double? value)
	  {
		double expected = (value == SIMPLE_30_360 ? calc360(y1, m1, d1, y2, m2, d2) : value).Value;
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		assertEquals(dayCount.relativeYearFraction(date1, date2), expected, TOLERANCE_ZERO);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "yearFraction") public void test_relativeYearFraction_reverse(DayCount dayCount, int y1, int m1, int d1, int y2, int m2, int d2, System.Nullable<double> value)
	  public virtual void test_relativeYearFraction_reverse(DayCount dayCount, int y1, int m1, int d1, int y2, int m2, int d2, double? value)
	  {
		double expected = (value == SIMPLE_30_360 ? calc360(y1, m1, d1, y2, m2, d2) : value).Value;
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		assertEquals(dayCount.relativeYearFraction(date2, date1), -expected, TOLERANCE_ZERO);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "days") public static Object[][] data_days()
	  public static object[][] data_days()
	  {
		return new object[][]
		{
			new object[] {ONE_ONE, 2011, 12, 28, 2012, 2, 28, 1},
			new object[] {ONE_ONE, 2011, 12, 28, 2012, 2, 29, 1},
			new object[] {ONE_ONE, 2011, 12, 28, 2012, 3, 1, 1},
			new object[] {ONE_ONE, 2011, 12, 28, 2016, 2, 28, 1},
			new object[] {ONE_ONE, 2011, 12, 28, 2016, 2, 29, 1},
			new object[] {ONE_ONE, 2011, 12, 28, 2016, 3, 1, 1},
			new object[] {ONE_ONE, 2012, 2, 29, 2012, 3, 29, 1},
			new object[] {ONE_ONE, 2012, 2, 29, 2012, 3, 28, 1},
			new object[] {ONE_ONE, 2012, 3, 1, 2012, 3, 28, 1},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2012, 2, 28, 62},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2012, 2, 29, 63},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2012, 3, 1, 64},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2016, 2, 28, 1523},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2016, 2, 29, 1524},
			new object[] {ACT_ACT_ISDA, 2011, 12, 28, 2016, 3, 1, 1525},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2012, 2, 28, 62},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2012, 2, 29, 63},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2012, 3, 1, 64},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2016, 2, 28, 1523},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2016, 2, 29, 1524},
			new object[] {ACT_ACT_AFB, 2011, 12, 28, 2016, 3, 1, 1525},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2012, 2, 28, 62},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2012, 2, 29, 63},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2012, 3, 1, 64},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2016, 2, 28, 1523},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2016, 2, 29, 1524},
			new object[] {ACT_ACT_YEAR, 2011, 12, 28, 2016, 3, 1, 1525},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2012, 2, 28, 62},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2012, 2, 29, 63},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2012, 3, 1, 64},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2016, 2, 28, 62 + 366 + 365 + 365 + 365},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2016, 2, 29, 63 + 366 + 365 + 365 + 365},
			new object[] {ACT_365_ACTUAL, 2011, 12, 28, 2016, 3, 1, 64 + 366 + 365 + 365 + 365},
			new object[] {ACT_365_ACTUAL, 2012, 2, 28, 2012, 3, 28, 29},
			new object[] {ACT_365_ACTUAL, 2012, 2, 29, 2012, 3, 28, 28},
			new object[] {ACT_365_ACTUAL, 2012, 3, 1, 2012, 3, 28, 27},
			new object[] {ACT_360, 2011, 12, 28, 2012, 2, 28, 62},
			new object[] {ACT_360, 2011, 12, 28, 2012, 2, 29, 63},
			new object[] {ACT_360, 2011, 12, 28, 2012, 3, 1, 64},
			new object[] {ACT_360, 2011, 12, 28, 2016, 2, 28, 62 + 366 + 365 + 365 + 365},
			new object[] {ACT_360, 2011, 12, 28, 2016, 2, 29, 63 + 366 + 365 + 365 + 365},
			new object[] {ACT_360, 2011, 12, 28, 2016, 3, 1, 64 + 366 + 365 + 365 + 365},
			new object[] {ACT_364, 2011, 12, 28, 2012, 2, 28, 62},
			new object[] {ACT_364, 2011, 12, 28, 2012, 2, 29, 63},
			new object[] {ACT_364, 2011, 12, 28, 2012, 3, 1, 64},
			new object[] {ACT_364, 2011, 12, 28, 2016, 2, 28, 62 + 366 + 365 + 365 + 365},
			new object[] {ACT_364, 2011, 12, 28, 2016, 2, 29, 63 + 366 + 365 + 365 + 365},
			new object[] {ACT_364, 2011, 12, 28, 2016, 3, 1, 64 + 366 + 365 + 365 + 365},
			new object[] {ACT_364, 2012, 2, 28, 2012, 3, 28, 29},
			new object[] {ACT_364, 2012, 2, 29, 2012, 3, 28, 28},
			new object[] {ACT_364, 2012, 3, 1, 2012, 3, 28, 27},
			new object[] {ACT_365F, 2011, 12, 28, 2012, 2, 28, 62},
			new object[] {ACT_365F, 2011, 12, 28, 2012, 2, 29, 63},
			new object[] {ACT_365F, 2011, 12, 28, 2012, 3, 1, 64},
			new object[] {ACT_365F, 2011, 12, 28, 2016, 2, 28, 62 + 366 + 365 + 365 + 365},
			new object[] {ACT_365F, 2011, 12, 28, 2016, 2, 29, 63 + 366 + 365 + 365 + 365},
			new object[] {ACT_365F, 2011, 12, 28, 2016, 3, 1, 64 + 366 + 365 + 365 + 365},
			new object[] {ACT_365F, 2012, 2, 28, 2012, 3, 28, 29},
			new object[] {ACT_365F, 2012, 2, 29, 2012, 3, 28, 28},
			new object[] {ACT_365F, 2012, 3, 1, 2012, 3, 28, 27},
			new object[] {ACT_365_25, 2011, 12, 28, 2012, 2, 28, 62},
			new object[] {ACT_365_25, 2011, 12, 28, 2012, 2, 29, 63},
			new object[] {ACT_365_25, 2011, 12, 28, 2012, 3, 1, 64},
			new object[] {ACT_365_25, 2011, 12, 28, 2016, 2, 28, 62 + 366 + 365 + 365 + 365},
			new object[] {ACT_365_25, 2011, 12, 28, 2016, 2, 29, 63 + 366 + 365 + 365 + 365},
			new object[] {ACT_365_25, 2011, 12, 28, 2016, 3, 1, 64 + 366 + 365 + 365 + 365},
			new object[] {ACT_365_25, 2012, 2, 28, 2012, 3, 28, 29},
			new object[] {ACT_365_25, 2012, 2, 29, 2012, 3, 28, 28},
			new object[] {ACT_365_25, 2012, 3, 1, 2012, 3, 28, 27},
			new object[] {NL_365, 2011, 12, 28, 2012, 2, 28, 62},
			new object[] {NL_365, 2011, 12, 28, 2012, 2, 29, 62},
			new object[] {NL_365, 2011, 12, 28, 2012, 3, 1, 63},
			new object[] {NL_365, 2011, 12, 28, 2016, 2, 28, 62 + 365 + 365 + 365 + 365},
			new object[] {NL_365, 2011, 12, 28, 2016, 2, 29, 62 + 365 + 365 + 365 + 365},
			new object[] {NL_365, 2011, 12, 28, 2016, 3, 1, 63 + 365 + 365 + 365 + 365},
			new object[] {NL_365, 2012, 2, 28, 2012, 3, 28, 28},
			new object[] {NL_365, 2012, 2, 29, 2012, 3, 28, 28},
			new object[] {NL_365, 2012, 3, 1, 2012, 3, 28, 27},
			new object[] {NL_365, 2011, 12, 1, 2012, 12, 1, 365},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2012, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2012, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2012, 3, 1, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2016, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2016, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2011, 12, 28, 2016, 3, 1, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2012, 2, 28, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2012, 2, 29, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2011, 2, 28, 2012, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2011, 2, 28, 2012, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2012, 2, 29, 2016, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2012, 3, 1, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2012, 5, 30, 2013, 8, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2012, 5, 29, 2013, 8, 30, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2012, 5, 30, 2013, 8, 30, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2012, 5, 29, 2013, 8, 31, SIMPLE_30_360Days},
			new object[] {THIRTY_360_ISDA, 2012, 5, 30, 2013, 8, 31, calc360Days(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_360_ISDA, 2012, 5, 31, 2013, 8, 30, calc360Days(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_360_ISDA, 2012, 5, 31, 2013, 8, 31, calc360Days(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2012, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2012, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2012, 3, 1, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2016, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2016, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2011, 12, 28, 2016, 3, 1, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2012, 2, 28, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2012, 2, 29, 2012, 3, 28, calc360Days(2012, 2, 30, 2012, 3, 28)},
			new object[] {THIRTY_360_PSA, 2011, 2, 28, 2012, 2, 28, calc360Days(2011, 2, 30, 2012, 2, 28)},
			new object[] {THIRTY_360_PSA, 2011, 2, 28, 2012, 2, 29, calc360Days(2011, 2, 30, 2012, 2, 29)},
			new object[] {THIRTY_360_PSA, 2012, 2, 29, 2016, 2, 29, calc360Days(2012, 2, 30, 2016, 2, 29)},
			new object[] {THIRTY_360_PSA, 2012, 3, 1, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2012, 5, 30, 2013, 8, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2012, 5, 29, 2013, 8, 30, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2012, 5, 30, 2013, 8, 30, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2012, 5, 29, 2013, 8, 31, SIMPLE_30_360Days},
			new object[] {THIRTY_360_PSA, 2012, 5, 30, 2013, 8, 31, calc360Days(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_360_PSA, 2012, 5, 31, 2013, 8, 30, calc360Days(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_360_PSA, 2012, 5, 31, 2013, 8, 31, calc360Days(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2012, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2012, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2012, 3, 1, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2016, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2016, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2011, 12, 28, 2016, 3, 1, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2012, 2, 28, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2012, 2, 29, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2011, 2, 28, 2012, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2011, 2, 28, 2012, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2012, 2, 29, 2016, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2012, 3, 1, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2012, 5, 30, 2013, 8, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2012, 5, 29, 2013, 8, 30, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2012, 5, 30, 2013, 8, 30, SIMPLE_30_360Days},
			new object[] {THIRTY_E_360, 2012, 5, 29, 2013, 8, 31, calc360Days(2012, 5, 29, 2013, 8, 30)},
			new object[] {THIRTY_E_360, 2012, 5, 30, 2013, 8, 31, calc360Days(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_E_360, 2012, 5, 31, 2013, 8, 30, calc360Days(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_E_360, 2012, 5, 31, 2013, 8, 31, calc360Days(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2012, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2012, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2012, 3, 1, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2016, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2016, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2011, 12, 28, 2016, 3, 1, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2012, 2, 28, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2012, 2, 29, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2012, 3, 1, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2011, 2, 28, 2012, 2, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2011, 2, 28, 2012, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2012, 2, 29, 2016, 2, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2012, 3, 1, 2012, 3, 28, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 30, 2013, 8, 29, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 29, 2013, 8, 30, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 30, 2013, 8, 30, SIMPLE_30_360Days},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 29, 2013, 8, 31, calc360Days(2012, 5, 29, 2013, 9, 1)},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 30, 2013, 8, 31, calc360Days(2012, 5, 30, 2013, 9, 1)},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 31, 2013, 8, 30, calc360Days(2012, 5, 30, 2013, 8, 30)},
			new object[] {THIRTY_EPLUS_360, 2012, 5, 31, 2013, 8, 31, calc360Days(2012, 5, 30, 2013, 9, 1)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "days") public void test_days(DayCount dayCount, int y1, int m1, int d1, int y2, int m2, int d2, int value)
	  public virtual void test_days(DayCount dayCount, int y1, int m1, int d1, int y2, int m2, int d2, int value)
	  {
		int expected = (value == SIMPLE_30_360Days ? calc360Days(y1, m1, d1, y2, m2, d2) : value);
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		assertEquals(dayCount.days(date1, date2), expected, TOLERANCE_ZERO);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "30U360") public static Object[][] data_30U360()
	  public static object[][] data_30U360()
	  {
		return new object[][]
		{
			new object[] {2011, 12, 28, 2012, 2, 28, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2011, 12, 28, 2012, 2, 29, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2011, 12, 28, 2012, 3, 1, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2011, 12, 28, 2016, 2, 28, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2011, 12, 28, 2016, 2, 29, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2011, 12, 28, 2016, 3, 1, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 2, 28, 2012, 3, 28, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 2, 29, 2012, 3, 28, SIMPLE_30_360, calc360(2012, 2, 30, 2012, 3, 28)},
			new object[] {2012, 2, 29, 2012, 3, 30, SIMPLE_30_360, calc360(2012, 2, 30, 2012, 3, 30)},
			new object[] {2012, 2, 29, 2012, 3, 31, SIMPLE_30_360, calc360(2012, 2, 30, 2012, 3, 30)},
			new object[] {2012, 2, 29, 2013, 2, 28, SIMPLE_30_360, calc360(2012, 2, 30, 2013, 2, 30)},
			new object[] {2011, 2, 28, 2012, 2, 28, SIMPLE_30_360, calc360(2011, 2, 30, 2012, 2, 28)},
			new object[] {2011, 2, 28, 2012, 2, 29, SIMPLE_30_360, calc360(2011, 2, 30, 2012, 2, 30)},
			new object[] {2012, 2, 29, 2016, 2, 29, SIMPLE_30_360, calc360(2012, 2, 30, 2016, 2, 30)},
			new object[] {2012, 3, 1, 2012, 3, 28, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 5, 30, 2013, 8, 29, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 5, 29, 2013, 8, 30, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 5, 30, 2013, 8, 30, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 5, 29, 2013, 8, 31, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 5, 30, 2013, 8, 31, calc360(2012, 5, 30, 2013, 8, 30), calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {2012, 5, 31, 2013, 8, 30, calc360(2012, 5, 30, 2013, 8, 30), calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {2012, 5, 31, 2013, 8, 31, calc360(2012, 5, 30, 2013, 8, 30), calc360(2012, 5, 30, 2013, 8, 30)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "30U360") public void test_yearFraction_30U360_notEom(int y1, int m1, int d1, int y2, int m2, int d2, System.Nullable<double> valueNotEOM, System.Nullable<double> valueEOM)
	  public virtual void test_yearFraction_30U360_notEom(int y1, int m1, int d1, int y2, int m2, int d2, double? valueNotEOM, double? valueEOM)
	  {
		double expected = (valueNotEOM == SIMPLE_30_360 ? calc360(y1, m1, d1, y2, m2, d2) : valueNotEOM).Value;
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		DayCount_ScheduleInfo info = new Info(false);
		assertEquals(THIRTY_U_360.yearFraction(date1, date2, info), expected, TOLERANCE_ZERO);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "30U360") public void test_yearFraction_30U360_eom(int y1, int m1, int d1, int y2, int m2, int d2, System.Nullable<double> valueNotEOM, System.Nullable<double> valueEOM)
	  public virtual void test_yearFraction_30U360_eom(int y1, int m1, int d1, int y2, int m2, int d2, double? valueNotEOM, double? valueEOM)
	  {
		double expected = (valueEOM == SIMPLE_30_360 ? calc360(y1, m1, d1, y2, m2, d2) : valueEOM).Value;
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		DayCount_ScheduleInfo info = new Info(true);
		assertEquals(THIRTY_U_360.yearFraction(date1, date2, info), expected, TOLERANCE_ZERO);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "30U360") public void test_yearFraction_30360ISDA(int y1, int m1, int d1, int y2, int m2, int d2, System.Nullable<double> valueNotEOM, System.Nullable<double> valueEOM)
	  public virtual void test_yearFraction_30360ISDA(int y1, int m1, int d1, int y2, int m2, int d2, double? valueNotEOM, double? valueEOM)
	  {
		double expected = (valueNotEOM == SIMPLE_30_360 ? calc360(y1, m1, d1, y2, m2, d2) : valueNotEOM).Value;
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		DayCount_ScheduleInfo info = new Info(true);
		assertEquals(THIRTY_360_ISDA.yearFraction(date1, date2, info), expected, TOLERANCE_ZERO);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "30U360") public void test_yearFraction_30U360EOM(int y1, int m1, int d1, int y2, int m2, int d2, System.Nullable<double> valueNotEOM, System.Nullable<double> valueEOM)
	  public virtual void test_yearFraction_30U360EOM(int y1, int m1, int d1, int y2, int m2, int d2, double? valueNotEOM, double? valueEOM)
	  {
		double expected = (valueEOM == SIMPLE_30_360 ? calc360(y1, m1, d1, y2, m2, d2) : valueEOM).Value;
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		DayCount_ScheduleInfo info = new Info(true);
		assertEquals(THIRTY_U_360_EOM.yearFraction(date1, date2, info), expected, TOLERANCE_ZERO);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "30E360ISDA") public static Object[][] data_30E360ISDA()
	  public static object[][] data_30E360ISDA()
	  {
		return new object[][]
		{
			new object[] {2011, 12, 28, 2012, 2, 28, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2011, 12, 28, 2012, 2, 29, calc360(2011, 12, 28, 2012, 2, 30), SIMPLE_30_360},
			new object[] {2011, 12, 28, 2012, 3, 1, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2011, 12, 28, 2016, 2, 28, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2011, 12, 28, 2016, 2, 29, calc360(2011, 12, 28, 2016, 2, 30), SIMPLE_30_360},
			new object[] {2011, 12, 28, 2016, 3, 1, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 2, 28, 2012, 3, 28, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 2, 29, 2012, 3, 28, calc360(2012, 2, 30, 2012, 3, 28), calc360(2012, 2, 30, 2012, 3, 28)},
			new object[] {2011, 2, 28, 2012, 2, 28, calc360(2011, 2, 30, 2012, 2, 28), calc360(2011, 2, 30, 2012, 2, 28)},
			new object[] {2011, 2, 28, 2012, 2, 29, calc360(2011, 2, 30, 2012, 2, 30), calc360(2011, 2, 30, 2012, 2, 29)},
			new object[] {2012, 2, 29, 2016, 2, 29, calc360(2012, 2, 30, 2016, 2, 30), calc360(2012, 2, 30, 2016, 2, 29)},
			new object[] {2012, 3, 1, 2012, 3, 28, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 5, 30, 2013, 8, 29, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 5, 29, 2013, 8, 30, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 5, 30, 2013, 8, 30, SIMPLE_30_360, SIMPLE_30_360},
			new object[] {2012, 5, 29, 2013, 8, 31, calc360(2012, 5, 29, 2013, 8, 30), calc360(2012, 5, 29, 2013, 8, 30)},
			new object[] {2012, 5, 30, 2013, 8, 31, calc360(2012, 5, 30, 2013, 8, 30), calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {2012, 5, 31, 2013, 8, 30, calc360(2012, 5, 30, 2013, 8, 30), calc360(2012, 5, 30, 2013, 8, 30)},
			new object[] {2012, 5, 31, 2013, 8, 31, calc360(2012, 5, 30, 2013, 8, 30), calc360(2012, 5, 30, 2013, 8, 30)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "30E360ISDA") public void test_yearFraction_30E360ISDA_notMaturity(int y1, int m1, int d1, int y2, int m2, int d2, System.Nullable<double> valueNotMaturity, System.Nullable<double> valueMaturity)
	  public virtual void test_yearFraction_30E360ISDA_notMaturity(int y1, int m1, int d1, int y2, int m2, int d2, double? valueNotMaturity, double? valueMaturity)
	  {
		double expected = (valueNotMaturity == SIMPLE_30_360 ? calc360(y1, m1, d1, y2, m2, d2) : valueNotMaturity).Value;
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		DayCount_ScheduleInfo info = new Info(false);
		assertEquals(THIRTY_E_360_ISDA.yearFraction(date1, date2, info), expected, TOLERANCE_ZERO);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "30E360ISDA") public void test_yearFraction_30E360ISDA_maturity(int y1, int m1, int d1, int y2, int m2, int d2, System.Nullable<double> valueNotMaturity, System.Nullable<double> valueMaturity)
	  public virtual void test_yearFraction_30E360ISDA_maturity(int y1, int m1, int d1, int y2, int m2, int d2, double? valueNotMaturity, double? valueMaturity)
	  {
		double expected = (valueMaturity == SIMPLE_30_360 ? calc360(y1, m1, d1, y2, m2, d2) : valueMaturity).Value;
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		DayCount_ScheduleInfo info = new Info(null, date2, null, false, P3M);
		assertEquals(THIRTY_E_360_ISDA.yearFraction(date1, date2, info), expected, TOLERANCE_ZERO);
	  }

	  //-------------------------------------------------------------------------
	  // AFB day count is poorly defined, so tests were used to identify a sensible interpretation
	  // 1) The ISDA use of "Calculation Period" is a translation of "Periode d'Application"
	  // where the original simply meant the period the day count is applied over
	  // and NOT the regular periodic schedule (ISDA's definition of "Calculation Period").
	  // 2) The ISDA "clarification" for rolling backward does not appear in the original French.
	  // The ISDA rule produce strange results (in comments below) which can be avoided.
	  // OpenGamma interprets that February 29th should only be chosen if the end date of the period
	  // is February 29th and the rolled back date is a leap year.
	  // 3) No document indicates precisely when to stop rolling back and treat the remainder as a fraction
	  // OpenGamma interprets that rolling back in whole years continues until the remainder
	  // is less than one year, and possibly zero if two dates are an exact number of years apart
	  // 4) In all cases, the rule has strange effects when interest through a period encounters
	  // February 29th and the denominator suddenly changes from 365 to 366 for the rest of the year
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "ACTACTAFB") public static Object[][] data_ACTACTAFB()
	  public static object[][] data_ACTACTAFB()
	  {
		return new object[][]
		{
			new object[] {1994, 2, 10, 1997, 6, 30, 140d / 365d + 3},
			new object[] {1994, 2, 10, 1994, 6, 30, 140d / 365d},
			new object[] {2004, 2, 10, 2005, 2, 10, 1d},
			new object[] {2004, 2, 28, 2005, 2, 28, 1d},
			new object[] {2004, 2, 29, 2005, 2, 28, 365d / 366d},
			new object[] {2004, 3, 1, 2005, 3, 1, 1d},
			new object[] {2003, 2, 28, 2005, 2, 27, 1d + (364d / 365d)},
			new object[] {2003, 2, 28, 2005, 2, 28, 2d},
			new object[] {2003, 2, 28, 2005, 3, 1, 2d + (1d / 365d)},
			new object[] {2003, 2, 28, 2008, 2, 27, 4d + (364d / 365d)},
			new object[] {2003, 2, 28, 2008, 2, 28, 5d},
			new object[] {2003, 2, 28, 2008, 2, 29, 5d},
			new object[] {2003, 2, 28, 2008, 3, 1, 5d + (1d / 365d)},
			new object[] {2004, 2, 28, 2005, 2, 27, (365d / 366d)},
			new object[] {2004, 2, 28, 2005, 2, 28, 1d},
			new object[] {2004, 2, 28, 2005, 3, 1, 1d + (2d / 366d)},
			new object[] {2004, 2, 28, 2008, 2, 27, 3d + (365d / 366d)},
			new object[] {2004, 2, 28, 2008, 2, 28, 4d},
			new object[] {2004, 2, 28, 2008, 2, 29, 4d + (1d / 365d)},
			new object[] {2004, 2, 28, 2008, 3, 1, 4d + (2d / 366d)},
			new object[] {2004, 2, 29, 2005, 2, 28, 365d / 366d},
			new object[] {2004, 2, 29, 2005, 3, 1, 1d + (1d / 366d)},
			new object[] {2004, 2, 29, 2008, 2, 27, 3d + (364d / 366d)},
			new object[] {2004, 2, 29, 2008, 2, 28, 3d + (365d / 366d)},
			new object[] {2004, 2, 29, 2008, 2, 29, 4d},
			new object[] {2004, 2, 29, 2008, 3, 1, 4d + (1d / 366d)},
			new object[] {2004, 3, 1, 2005, 2, 28, 364d / 365d},
			new object[] {2004, 3, 1, 2005, 3, 1, 1d},
			new object[] {2004, 3, 1, 2008, 2, 27, 3d + (363d / 365d)},
			new object[] {2004, 3, 1, 2008, 2, 28, 3d + (364d / 365d)},
			new object[] {2004, 3, 1, 2008, 2, 29, 3d + (364d / 365d)},
			new object[] {2004, 3, 1, 2008, 3, 1, 4d},
			new object[] {2003, 3, 1, 2005, 2, 27, 1d + (363d / 365d)},
			new object[] {2003, 3, 1, 2005, 2, 28, 1d + (364d / 365d)},
			new object[] {2003, 3, 1, 2005, 3, 1, 2d},
			new object[] {2003, 3, 1, 2008, 2, 27, 4d + (363d / 365d)},
			new object[] {2003, 3, 1, 2008, 2, 28, 4d + (364d / 365d)},
			new object[] {2003, 3, 1, 2008, 2, 29, 5d},
			new object[] {2003, 3, 1, 2008, 3, 1, 5d},
			new object[] {2004, 2, 28, 2006, 3, 1, 2d + (2d / 366d)},
			new object[] {2004, 2, 29, 2006, 3, 1, 2d + (1d / 366d)},
			new object[] {2004, 3, 1, 2006, 3, 1, 2d},
			new object[] {2005, 2, 28, 2007, 3, 1, 2d + (1d / 365d)},
			new object[] {2005, 3, 1, 2007, 3, 1, 2d},
			new object[] {2004, 2, 27, 2008, 2, 28, 4d + (1d / 365d)},
			new object[] {2004, 2, 28, 2008, 2, 28, 4d},
			new object[] {2004, 2, 29, 2008, 2, 28, 3d + (365d / 366d)},
			new object[] {2004, 3, 1, 2008, 2, 28, 3d + (364d / 365d)},
			new object[] {2006, 2, 27, 2008, 2, 28, 2d + (1d / 365d)},
			new object[] {2006, 2, 28, 2008, 2, 28, 2d},
			new object[] {2006, 3, 1, 2008, 2, 28, 1d + (364d / 365d)},
			new object[] {2004, 2, 28, 2008, 2, 29, 4d + (1d / 365d)},
			new object[] {2004, 2, 29, 2008, 2, 29, 4d},
			new object[] {2004, 3, 1, 2008, 2, 29, 3d + (364d / 365d)},
			new object[] {2006, 2, 27, 2008, 2, 29, 2d + (1d / 365d)},
			new object[] {2006, 2, 28, 2008, 2, 29, 2d},
			new object[] {2006, 3, 1, 2008, 2, 29, 1d + (364d / 365d)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "ACTACTAFB") public void test_yearFraction_ACTACTAFB(int y1, int m1, int d1, int y2, int m2, int d2, double expected)
	  public virtual void test_yearFraction_ACTACTAFB(int y1, int m1, int d1, int y2, int m2, int d2, double expected)
	  {
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		assertEquals(ACT_ACT_AFB.yearFraction(date1, date2), expected, TOLERANCE_ZERO);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "ACT365L") public static Object[][] data_ACT365L()
	  public static object[][] data_ACT365L()
	  {
		return new object[][]
		{
			new object[] {2011, 12, 28, 2012, 2, 28, P12M, 2012, 2, 28, 62d / 365d},
			new object[] {2011, 12, 28, 2012, 2, 28, P12M, 2012, 2, 29, 62d / 366d},
			new object[] {2011, 12, 28, 2012, 2, 28, P12M, 2012, 3, 1, 62d / 366d},
			new object[] {2011, 12, 28, 2012, 2, 29, P12M, 2012, 2, 29, 63d / 366d},
			new object[] {2011, 12, 28, 2012, 2, 29, P12M, 2012, 3, 1, 63d / 366d},
			new object[] {2011, 12, 28, 2012, 2, 28, P6M, 2012, 2, 28, 62d / 366d},
			new object[] {2011, 12, 28, 2012, 2, 28, P6M, 2012, 2, 29, 62d / 366d},
			new object[] {2011, 12, 28, 2012, 2, 28, P6M, 2012, 3, 1, 62d / 366d},
			new object[] {2011, 12, 28, 2012, 2, 29, P6M, 2012, 2, 29, 63d / 366d},
			new object[] {2011, 12, 28, 2012, 2, 29, P6M, 2012, 3, 1, 63d / 366d},
			new object[] {2010, 12, 28, 2011, 2, 28, P6M, 2011, 2, 28, 62d / 365d},
			new object[] {2010, 12, 28, 2011, 2, 28, P6M, 2011, 3, 1, 62d / 365d}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "ACT365L") public void test_yearFraction_ACT365L(int y1, int m1, int d1, int y2, int m2, int d2, com.opengamma.strata.basics.schedule.Frequency freq, int y3, int m3, int d3, double expected)
	  public virtual void test_yearFraction_ACT365L(int y1, int m1, int d1, int y2, int m2, int d2, Frequency freq, int y3, int m3, int d3, double expected)
	  {
		LocalDate date1 = LocalDate.of(y1, m1, d1);
		LocalDate date2 = LocalDate.of(y2, m2, d2);
		DayCount_ScheduleInfo info = new Info(null, null, LocalDate.of(y3, m3, d3), false, freq);
		assertEquals(ACT_365L.yearFraction(date1, date2, info), expected, TOLERANCE_ZERO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_actActIcma_singlePeriod()
	  {
		LocalDate start = LocalDate.of(2003, 11, 1);
		LocalDate end = LocalDate.of(2004, 5, 1);
		DayCount_ScheduleInfo info = new Info(start, end, end, true, P6M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end.minusDays(1), info), (181d / (182d * 2d)), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (182d / (182d * 2d)), TOLERANCE_ZERO);
	  }

	  public virtual void test_actActIcma_longInitialStub_eomFlagEom_short()
	  {
		// nominals, 2011-08-31 (P91D) 2011-11-30 (P91D) 2012-02-29
		LocalDate start = LocalDate.of(2011, 10, 1);
		LocalDate periodEnd = LocalDate.of(2012, 2, 29);
		LocalDate end = LocalDate.of(2011, 11, 12); // before first nominal
		DayCount_ScheduleInfo info = new Info(start, periodEnd.plus(P3M), periodEnd, true, P3M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (42d / (91d * 4d)), TOLERANCE_ZERO);
	  }

	  public virtual void test_actActIcma_longInitialStub_eomFlagEom_long()
	  {
		// nominals, 2011-08-31 (P91D) 2011-11-30 (P91D) 2012-02-29
		LocalDate start = LocalDate.of(2011, 10, 1);
		LocalDate periodEnd = LocalDate.of(2012, 2, 29);
		LocalDate end = LocalDate.of(2012, 1, 12); // after first nominal
		DayCount_ScheduleInfo info = new Info(start, periodEnd.plus(P3M), periodEnd, true, P3M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (60d / (91d * 4d)) + (43d / (91d * 4d)), TOLERANCE_ZERO);
	  }

	  public virtual void test_actActIcma_veryLongInitialStub_eomFlagEom_short()
	  {
		// nominals, 2011-05-31 (P92D) 2011-08-31 (P91D) 2011-11-30 (P91D) 2012-02-29
		LocalDate start = LocalDate.of(2011, 7, 1);
		LocalDate periodEnd = LocalDate.of(2012, 2, 29);
		LocalDate end = LocalDate.of(2011, 8, 12); // before first nominal
		DayCount_ScheduleInfo info = new Info(start, periodEnd.plus(P3M), periodEnd, true, P3M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (42d / (92d * 4d)), TOLERANCE_ZERO);
	  }

	  public virtual void test_actActIcma_veryLongInitialStub_eomFlagEom_mid()
	  {
		// nominals, 2011-05-31 (P92D) 2011-08-31 (P91D) 2011-11-30 (P91D) 2012-02-29
		LocalDate start = LocalDate.of(2011, 7, 1);
		LocalDate periodEnd = LocalDate.of(2012, 2, 29);
		LocalDate end = LocalDate.of(2011, 11, 12);
		DayCount_ScheduleInfo info = new Info(start, periodEnd.plus(P3M), periodEnd, true, P3M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (61d / (92d * 4d)) + (73d / (91d * 4d)), TOLERANCE_ZERO);
	  }

	  public virtual void test_actActIcma_longInitialStub_notEomFlagEom_short()
	  {
		// nominals, 2011-08-29 (P92D) 2011-11-29 (P92D) 2012-02-29
		LocalDate start = LocalDate.of(2011, 10, 1);
		LocalDate periodEnd = LocalDate.of(2012, 2, 29);
		LocalDate end = LocalDate.of(2011, 11, 12); // before first nominal
		DayCount_ScheduleInfo info = new Info(start, periodEnd.plus(P3M), periodEnd, false, P3M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (42d / (92d * 4d)), TOLERANCE_ZERO);
	  }

	  public virtual void test_actActIcma_longInitialStub_notEomFlagEom_long()
	  {
		// nominals, 2011-08-29 (P92D) 2011-11-29 (P92D) 2012-02-29
		LocalDate start = LocalDate.of(2011, 10, 1);
		LocalDate periodEnd = LocalDate.of(2012, 2, 29);
		LocalDate end = LocalDate.of(2012, 1, 12); // after first nominal
		DayCount_ScheduleInfo info = new Info(start, periodEnd.plus(P3M), periodEnd, false, P3M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (59d / (92d * 4d)) + (44d / (92d * 4d)), TOLERANCE_ZERO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_actActIcma_longFinalStub_eomFlagEom_short()
	  {
		// nominals, 2011-08-31 (P91D) 2011-11-30 (P91D) 2012-02-29
		LocalDate start = LocalDate.of(2011, 8, 31);
		LocalDate periodEnd = LocalDate.of(2012, 1, 31);
		LocalDate end = LocalDate.of(2011, 11, 12); // before first nominal
		DayCount_ScheduleInfo info = new Info(start.minus(P3M), periodEnd, periodEnd, true, P3M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (73d / (91d * 4d)), TOLERANCE_ZERO);
	  }

	  public virtual void test_actActIcma_longFinalStub_eomFlagEom_long()
	  {
		// nominals, 2011-08-31 (P91D) 2011-11-30 (P91D) 2012-02-29
		LocalDate start = LocalDate.of(2011, 8, 31);
		LocalDate periodEnd = LocalDate.of(2012, 1, 31);
		LocalDate end = LocalDate.of(2012, 1, 12); // after first nominal
		DayCount_ScheduleInfo info = new Info(start.minus(P3M), periodEnd, periodEnd, true, P3M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (91d / (91d * 4d)) + (43d / (91d * 4d)), TOLERANCE_ZERO);
	  }

	  public virtual void test_actActIcma_longFinalStub_notEomFlagEom_short()
	  {
		// nominals, 2012-02-29 (P90D) 2012-05-29 (P92D) 2012-08-29
		LocalDate start = LocalDate.of(2012, 2, 29);
		LocalDate periodEnd = LocalDate.of(2012, 7, 31);
		LocalDate end = LocalDate.of(2012, 4, 1); // before first nominal
		DayCount_ScheduleInfo info = new Info(start.minus(P3M), periodEnd, periodEnd, false, P3M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (32d / (90d * 4d)), TOLERANCE_ZERO);
	  }

	  public virtual void test_actActIcma_longFinalStub_notEomFlagEom_long()
	  {
		// nominals, 2012-02-29 (P90D) 2012-05-29 (P92D) 2012-08-29
		LocalDate start = LocalDate.of(2012, 2, 29);
		LocalDate periodEnd = LocalDate.of(2012, 7, 31);
		LocalDate end = LocalDate.of(2012, 6, 1); // after first nominal
		DayCount_ScheduleInfo info = new Info(start.minus(P3M), periodEnd, periodEnd, false, P3M);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (90d / (90d * 4d)) + (3d / (92d * 4d)), TOLERANCE_ZERO);
	  }

	  //-------------------------------------------------------------------------
	  // test against official examples - http://www.isda.org/c_and_a/pdf/ACT-ACT-ISDA-1999.pdf
	  // this version has an error http://www.isda.org/c_and_a/pdf/mktc1198.pdf
	  public virtual void test_actAct_isdaTestCase_normal()
	  {
		LocalDate start = LocalDate.of(2003, 11, 1);
		LocalDate end = LocalDate.of(2004, 5, 1);
		DayCount_ScheduleInfo info = new Info(start, end.plus(P6M), end, true, P6M);
		assertEquals(ACT_ACT_ISDA.yearFraction(start, end), (61d / 365d) + (121d / 366d), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (182d / (182d * 2d)), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_AFB.yearFraction(start, end), (182d / 366d), TOLERANCE_ZERO);
	  }

	  public virtual void test_actAct_isdaTestCase_shortInitialStub()
	  {
		LocalDate start = LocalDate.of(1999, 2, 1);
		LocalDate firstRegular = LocalDate.of(1999, 7, 1);
		LocalDate end = LocalDate.of(2000, 7, 1);
		DayCount_ScheduleInfo info1 = new Info(start, end.plus(P12M), firstRegular, true, P12M); // initial period
		DayCount_ScheduleInfo info2 = new Info(start, end.plus(P12M), end, true, P12M); // regular period
		assertEquals(ACT_ACT_ISDA.yearFraction(start, firstRegular), (150d / 365d), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, firstRegular, info1), (150d / (365d * 1d)), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_AFB.yearFraction(start, firstRegular), (150d / (365d)), TOLERANCE_ZERO);

		assertEquals(ACT_ACT_ISDA.yearFraction(firstRegular, end), (184d / 365d) + (182d / 366d), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_ICMA.yearFraction(firstRegular, end, info2), (366d / (366d * 1d)), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_AFB.yearFraction(firstRegular, end), (366d / 366d), TOLERANCE_ZERO);
	  }

	  public virtual void test_actAct_isdaTestCase_longInitialStub()
	  {
		LocalDate start = LocalDate.of(2002, 8, 15);
		LocalDate firstRegular = LocalDate.of(2003, 7, 15);
		LocalDate end = LocalDate.of(2004, 1, 15);
		DayCount_ScheduleInfo info1 = new Info(start, end, firstRegular, true, P6M); // initial period
		DayCount_ScheduleInfo info2 = new Info(start, end, end, true, P6M); // regular period
		assertEquals(ACT_ACT_ISDA.yearFraction(start, firstRegular), (334d / 365d), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, firstRegular, info1), (181d / (181d * 2d)) + (153d / (184d * 2d)), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_AFB.yearFraction(start, firstRegular), (334d / 365d), TOLERANCE_ZERO);
		// example is wrong in 1998 euro swap version
		assertEquals(ACT_ACT_ISDA.yearFraction(firstRegular, end), (170d / 365d) + (14d / 366d), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_ICMA.yearFraction(firstRegular, end, info2), (184d / (184d * 2d)), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_AFB.yearFraction(firstRegular, end), (184d / 365d), TOLERANCE_ZERO);
	  }

	  public virtual void test_actAct_isdaTestCase_shortFinalStub()
	  {
		LocalDate start = LocalDate.of(1999, 7, 30);
		LocalDate lastRegular = LocalDate.of(2000, 1, 30);
		LocalDate end = LocalDate.of(2000, 6, 30);
		DayCount_ScheduleInfo info1 = new Info(start, end, lastRegular, true, P6M); // regular period
		DayCount_ScheduleInfo info2 = new Info(start, end, end, true, P6M); // final period
		assertEquals(ACT_ACT_ISDA.yearFraction(start, lastRegular), (155d / 365d) + (29d / 366d), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, lastRegular, info1), (184d / (184d * 2d)), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_AFB.yearFraction(start, lastRegular), (184d / 365d), TOLERANCE_ZERO);

		assertEquals(ACT_ACT_ISDA.yearFraction(lastRegular, end), (152d / 366d), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_ICMA.yearFraction(lastRegular, end, info2), (152d / (182d * 2d)), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_AFB.yearFraction(lastRegular, end), (152d / 366d), TOLERANCE_ZERO);
	  }

	  public virtual void test_actAct_isdaTestCase_longFinalStub()
	  {
		LocalDate start = LocalDate.of(1999, 11, 30);
		LocalDate end = LocalDate.of(2000, 4, 30);
		DayCount_ScheduleInfo info = new Info(start.minus(P3M), end, end, true, P3M);
		assertEquals(ACT_ACT_ISDA.yearFraction(start, end), (32d / 365d) + (120d / 366d), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), (91d / (91d * 4d)) + (61d / (92d * 4)), TOLERANCE_ZERO);
		assertEquals(ACT_ACT_AFB.yearFraction(start, end), (152d / 366d), TOLERANCE_ZERO);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_actActYearVsIcma()
	  {
		LocalDate start = LocalDate.of(2011, 1, 1);
		for (int i = 0; i < 400; i++)
		{
		  for (int j = 0; j < 365; j++)
		  {
			LocalDate end = start.plusDays(j);
			DayCount_ScheduleInfo info = new Info(start, end, start.plusYears(1), false, P12M);
			assertEquals(ACT_ACT_ICMA.yearFraction(start, end, info), ACT_ACT_YEAR.yearFraction(start, end), TOLERANCE_ZERO);
		  }
		  start = start.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {ONE_ONE, "1/1"},
			new object[] {ACT_ACT_ISDA, "Act/Act ISDA"},
			new object[] {ACT_ACT_ICMA, "Act/Act ICMA"},
			new object[] {ACT_ACT_AFB, "Act/Act AFB"},
			new object[] {ACT_ACT_YEAR, "Act/Act Year"},
			new object[] {ACT_365_ACTUAL, "Act/365 Actual"},
			new object[] {ACT_365L, "Act/365L"},
			new object[] {ACT_360, "Act/360"},
			new object[] {ACT_364, "Act/364"},
			new object[] {ACT_365F, "Act/365F"},
			new object[] {ACT_365_25, "Act/365.25"},
			new object[] {NL_365, "NL/365"},
			new object[] {THIRTY_360_ISDA, "30/360 ISDA"},
			new object[] {THIRTY_U_360, "30U/360"},
			new object[] {THIRTY_U_360_EOM, "30U/360 EOM"},
			new object[] {THIRTY_360_PSA, "30/360 PSA"},
			new object[] {THIRTY_E_360_ISDA, "30E/360 ISDA"},
			new object[] {THIRTY_E_360, "30E/360"},
			new object[] {THIRTY_EPLUS_360, "30E+/360"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(DayCount convention, String name)
	  public virtual void test_name(DayCount convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(DayCount convention, String name)
	  public virtual void test_toString(DayCount convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(DayCount convention, String name)
	  public virtual void test_of_lookup(DayCount convention, string name)
	  {
		assertEquals(DayCount.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_lenientLookup_standardNames(DayCount convention, String name)
	  public virtual void test_lenientLookup_standardNames(DayCount convention, string name)
	  {
		assertEquals(DayCount.extendedEnum().findLenient(name.ToLower(Locale.ENGLISH)).get(), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(DayCount convention, String name)
	  public virtual void test_extendedEnum(DayCount convention, string name)
	  {
		ImmutableMap<string, DayCount> map = DayCount.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => DayCount.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsRuntime(() => DayCount.of(null));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "lenient") public static Object[][] data_lenient()
	  public static object[][] data_lenient()
	  {
		return new object[][]
		{
			new object[] {"Actual/Actual", ACT_ACT_ISDA},
			new object[] {"Act/Act", ACT_ACT_ISDA},
			new object[] {"A/A", ACT_ACT_ISDA},
			new object[] {"Actual/Actual ISDA", ACT_ACT_ISDA},
			new object[] {"A/A ISDA", ACT_ACT_ISDA},
			new object[] {"Actual/Actual ISDA", ACT_ACT_ISDA},
			new object[] {"A/A (ISDA)", ACT_ACT_ISDA},
			new object[] {"Act/Act (ISDA)", ACT_ACT_ISDA},
			new object[] {"Actual/Actual (ISDA)", ACT_ACT_ISDA},
			new object[] {"Act/Act", ACT_ACT_ISDA},
			new object[] {"Actual/Actual (Historical)", ACT_ACT_ISDA},
			new object[] {"A/A ICMA", ACT_ACT_ICMA},
			new object[] {"Actual/Actual ICMA", ACT_ACT_ICMA},
			new object[] {"A/A (ICMA)", ACT_ACT_ICMA},
			new object[] {"Act/Act (ICMA)", ACT_ACT_ICMA},
			new object[] {"Actual/Actual (ICMA)", ACT_ACT_ICMA},
			new object[] {"ISMA-99", ACT_ACT_ICMA},
			new object[] {"Actual/Actual (Bond)", ACT_ACT_ICMA},
			new object[] {"A/A AFB", ACT_ACT_AFB},
			new object[] {"Actual/Actual AFB", ACT_ACT_AFB},
			new object[] {"A/A (AFB)", ACT_ACT_AFB},
			new object[] {"Act/Act (AFB)", ACT_ACT_AFB},
			new object[] {"Actual/Actual (AFB)", ACT_ACT_AFB},
			new object[] {"Actual/Actual (Euro)", ACT_ACT_AFB},
			new object[] {"A/365 Actual", ACT_365_ACTUAL},
			new object[] {"Actual/365 Actual", ACT_365_ACTUAL},
			new object[] {"A/365 (Actual)", ACT_365_ACTUAL},
			new object[] {"Act/365 (Actual)", ACT_365_ACTUAL},
			new object[] {"Actual/365 (Actual)", ACT_365_ACTUAL},
			new object[] {"A/365A", ACT_365_ACTUAL},
			new object[] {"Act/365A", ACT_365_ACTUAL},
			new object[] {"Actual/365A", ACT_365_ACTUAL},
			new object[] {"A/365L", ACT_365L},
			new object[] {"Actual/365L", ACT_365L},
			new object[] {"A/365 Leap year", ACT_365L},
			new object[] {"Act/365 Leap year", ACT_365L},
			new object[] {"Actual/365 Leap year", ACT_365L},
			new object[] {"ISMA-Year", ACT_365L},
			new object[] {"Actual/360", ACT_360},
			new object[] {"A/360", ACT_360},
			new object[] {"French", ACT_360},
			new object[] {"Actual/364", ACT_364},
			new object[] {"A/364", ACT_364},
			new object[] {"A/365F", ACT_365F},
			new object[] {"Actual/365F", ACT_365F},
			new object[] {"A/365", ACT_365F},
			new object[] {"Act/365", ACT_365F},
			new object[] {"Actual/365", ACT_365F},
			new object[] {"Act/365 (Fixed)", ACT_365F},
			new object[] {"Actual/365 (Fixed)", ACT_365F},
			new object[] {"A/365 (Fixed)", ACT_365F},
			new object[] {"Actual/Fixed 365", ACT_365F},
			new object[] {"English", ACT_365F},
			new object[] {"A/365.25", ACT_365_25},
			new object[] {"Actual/365.25", ACT_365_25},
			new object[] {"A/NL", NL_365},
			new object[] {"Actual/NL", NL_365},
			new object[] {"NL365", NL_365},
			new object[] {"Act/365 No leap year", NL_365},
			new object[] {"30/360", THIRTY_360_ISDA},
			new object[] {"Eurobond Basis", THIRTY_E_360},
			new object[] {"30S/360", THIRTY_E_360},
			new object[] {"Special German", THIRTY_E_360},
			new object[] {"30/360 ICMA", THIRTY_E_360},
			new object[] {"30/360 (ICMA)", THIRTY_E_360},
			new object[] {"30/360 German", THIRTY_E_360_ISDA},
			new object[] {"German", THIRTY_E_360_ISDA},
			new object[] {"30/360 US", THIRTY_U_360},
			new object[] {"30/360 (US)", THIRTY_U_360},
			new object[] {"30US/360", THIRTY_U_360},
			new object[] {"360/360", THIRTY_U_360},
			new object[] {"Bond Basis", THIRTY_U_360},
			new object[] {"US", THIRTY_U_360},
			new object[] {"ISMA-30/360", THIRTY_U_360},
			new object[] {"30/360 SIA", THIRTY_U_360},
			new object[] {"30/360 (SIA)", THIRTY_U_360},
			new object[] {"BUS/252", DayCount.ofBus252(HolidayCalendarIds.BRBD)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "lenient") public void test_lenientLookup_specialNames(String name, DayCount convention)
	  public virtual void test_lenientLookup_specialNames(string name, DayCount convention)
	  {
		assertEquals(DayCount.extendedEnum().findLenient(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_relativeYearFraction_defaultMethod()
	  {
		DayCount dc = new DayCountAnonymousInnerClass(this);
		LocalDate date1 = date(2015, 6, 1);
		LocalDate date2 = date(2015, 7, 1);
		assertEquals(dc.yearFraction(date1, date2), 1, TOLERANCE_ZERO);
		assertEquals(dc.relativeYearFraction(date1, date2), 1, TOLERANCE_ZERO);
		assertEquals(dc.relativeYearFraction(date2, date1), -1, TOLERANCE_ZERO);
	  }

	  private class DayCountAnonymousInnerClass : DayCount
	  {
		  private readonly DayCountTest outerInstance;

		  public DayCountAnonymousInnerClass(DayCountTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }

		  public double yearFraction(LocalDate firstDate, LocalDate secondDate, DayCount_ScheduleInfo scheduleInfo)
		  {
			return 1;
		  }

		  public int days(LocalDate firstDate, LocalDate secondDate)
		  {
			return 1;
		  }

		  public string Name
		  {
			  get
			  {
				return "";
			  }
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_scheduleInfo()
	  {
		DayCount_ScheduleInfo test = new DayCount_ScheduleInfoAnonymousInnerClass(this);
		assertEquals(test.EndOfMonthConvention, true);
		assertThrows(() => test.StartDate, typeof(System.NotSupportedException));
		assertThrows(() => test.EndDate, typeof(System.NotSupportedException));
		assertThrows(() => test.Frequency, typeof(System.NotSupportedException));
		assertThrows(() => test.getPeriodEndDate(JAN_01), typeof(System.NotSupportedException));
	  }

	  private class DayCount_ScheduleInfoAnonymousInnerClass : DayCount_ScheduleInfo
	  {
		  private readonly DayCountTest outerInstance;

		  public DayCount_ScheduleInfoAnonymousInnerClass(DayCountTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }

	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(DayCounts));
		coverEnum(typeof(StandardDayCounts));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(ACT_364);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(DayCount), THIRTY_360_ISDA);
		assertJodaConvert(typeof(DayCount), ACT_365F);
	  }

	  //-------------------------------------------------------------------------
	  internal class Info : DayCount_ScheduleInfo
	  {
		internal readonly LocalDate start;
		internal readonly LocalDate end;
		internal readonly LocalDate periodEnd;
		internal readonly bool eom;
		internal readonly Frequency frequency;

		public Info(bool eom) : this(null, null, null, eom, null)
		{
		}

		public Info(LocalDate start, LocalDate end, LocalDate periodEnd, bool eom, Frequency frequency)
		{
		  this.start = start;
		  this.end = end;
		  this.periodEnd = periodEnd;
		  this.eom = eom;
		  this.frequency = frequency;
		}

		public override bool EndOfMonthConvention
		{
			get
			{
			  return eom;
			}
		}

		public override Frequency Frequency
		{
			get
			{
			  return frequency;
			}
		}

		public override LocalDate StartDate
		{
			get
			{
			  return start;
			}
		}

		public override LocalDate EndDate
		{
			get
			{
			  return end;
			}
		}

		public override LocalDate getPeriodEndDate(LocalDate date)
		{
		  return periodEnd;
		}
	  }

	}

}