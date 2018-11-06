/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING_BI_MONTHLY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.NEAREST;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.NO_ADJUST;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Test <seealso cref="BusinessDayConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BusinessDayConventionTest
	public class BusinessDayConventionTest
	{

	  private static readonly LocalDate FRI_2014_07_11 = LocalDate.of(2014, 7, 11);
	  private static readonly LocalDate SAT_2014_07_12 = LocalDate.of(2014, 7, 12);
	  private static readonly LocalDate SUN_2014_07_13 = LocalDate.of(2014, 7, 13);
	  private static readonly LocalDate MON_2014_07_14 = LocalDate.of(2014, 7, 14);
	  private static readonly LocalDate TUE_2014_07_15 = LocalDate.of(2014, 7, 15);

	  private static readonly LocalDate FRI_2014_08_29 = LocalDate.of(2014, 8, 29);
	  private static readonly LocalDate SAT_2014_08_30 = LocalDate.of(2014, 8, 30);
	  private static readonly LocalDate SUN_2014_08_31 = LocalDate.of(2014, 8, 31);
	  private static readonly LocalDate MON_2014_09_01 = LocalDate.of(2014, 9, 1);

	  private static readonly LocalDate FRI_2014_10_31 = LocalDate.of(2014, 10, 31);
	  private static readonly LocalDate SAT_2014_11_01 = LocalDate.of(2014, 11, 1);
	  private static readonly LocalDate SUN_2014_11_02 = LocalDate.of(2014, 11, 2);
	  private static readonly LocalDate MON_2014_11_03 = LocalDate.of(2014, 11, 3);

	  private static readonly LocalDate FRI_2014_11_14 = LocalDate.of(2014, 11, 14);
	  private static readonly LocalDate SAT_2014_11_15 = LocalDate.of(2014, 11, 15);
	  private static readonly LocalDate SUN_2014_11_16 = LocalDate.of(2014, 11, 16);
	  private static readonly LocalDate MON_2014_11_17 = LocalDate.of(2014, 11, 17);

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "convention") public static Object[][] data_convention()
	  public static object[][] data_convention()
	  {
		return new object[][]
		{
			new object[] {NO_ADJUST, FRI_2014_07_11, FRI_2014_07_11},
			new object[] {NO_ADJUST, SAT_2014_07_12, SAT_2014_07_12},
			new object[] {NO_ADJUST, SUN_2014_07_13, SUN_2014_07_13},
			new object[] {NO_ADJUST, MON_2014_07_14, MON_2014_07_14},
			new object[] {FOLLOWING, FRI_2014_07_11, FRI_2014_07_11},
			new object[] {FOLLOWING, SAT_2014_07_12, MON_2014_07_14},
			new object[] {FOLLOWING, SUN_2014_07_13, MON_2014_07_14},
			new object[] {FOLLOWING, MON_2014_07_14, MON_2014_07_14},
			new object[] {FOLLOWING, FRI_2014_08_29, FRI_2014_08_29},
			new object[] {FOLLOWING, SAT_2014_08_30, MON_2014_09_01},
			new object[] {FOLLOWING, SUN_2014_08_31, MON_2014_09_01},
			new object[] {FOLLOWING, MON_2014_09_01, MON_2014_09_01},
			new object[] {FOLLOWING, FRI_2014_10_31, FRI_2014_10_31},
			new object[] {FOLLOWING, SAT_2014_11_01, MON_2014_11_03},
			new object[] {FOLLOWING, SUN_2014_11_02, MON_2014_11_03},
			new object[] {FOLLOWING, MON_2014_11_03, MON_2014_11_03},
			new object[] {MODIFIED_FOLLOWING, FRI_2014_07_11, FRI_2014_07_11},
			new object[] {MODIFIED_FOLLOWING, SAT_2014_07_12, MON_2014_07_14},
			new object[] {MODIFIED_FOLLOWING, SUN_2014_07_13, MON_2014_07_14},
			new object[] {MODIFIED_FOLLOWING, MON_2014_07_14, MON_2014_07_14},
			new object[] {MODIFIED_FOLLOWING, FRI_2014_08_29, FRI_2014_08_29},
			new object[] {MODIFIED_FOLLOWING, SAT_2014_08_30, FRI_2014_08_29},
			new object[] {MODIFIED_FOLLOWING, SUN_2014_08_31, FRI_2014_08_29},
			new object[] {MODIFIED_FOLLOWING, MON_2014_09_01, MON_2014_09_01},
			new object[] {MODIFIED_FOLLOWING, FRI_2014_10_31, FRI_2014_10_31},
			new object[] {MODIFIED_FOLLOWING, SAT_2014_11_01, MON_2014_11_03},
			new object[] {MODIFIED_FOLLOWING, SUN_2014_11_02, MON_2014_11_03},
			new object[] {MODIFIED_FOLLOWING, MON_2014_11_03, MON_2014_11_03},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, FRI_2014_07_11, FRI_2014_07_11},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, SAT_2014_07_12, MON_2014_07_14},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, SUN_2014_07_13, MON_2014_07_14},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, MON_2014_07_14, MON_2014_07_14},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, FRI_2014_08_29, FRI_2014_08_29},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, SAT_2014_08_30, FRI_2014_08_29},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, SUN_2014_08_31, FRI_2014_08_29},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, MON_2014_09_01, MON_2014_09_01},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, FRI_2014_10_31, FRI_2014_10_31},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, SAT_2014_11_01, MON_2014_11_03},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, SUN_2014_11_02, MON_2014_11_03},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, MON_2014_11_03, MON_2014_11_03},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, FRI_2014_11_14, FRI_2014_11_14},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, SAT_2014_11_15, FRI_2014_11_14},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, SUN_2014_11_16, MON_2014_11_17},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, MON_2014_11_17, MON_2014_11_17},
			new object[] {PRECEDING, FRI_2014_07_11, FRI_2014_07_11},
			new object[] {PRECEDING, SAT_2014_07_12, FRI_2014_07_11},
			new object[] {PRECEDING, SUN_2014_07_13, FRI_2014_07_11},
			new object[] {PRECEDING, MON_2014_07_14, MON_2014_07_14},
			new object[] {PRECEDING, FRI_2014_08_29, FRI_2014_08_29},
			new object[] {PRECEDING, SAT_2014_08_30, FRI_2014_08_29},
			new object[] {PRECEDING, SUN_2014_08_31, FRI_2014_08_29},
			new object[] {PRECEDING, MON_2014_09_01, MON_2014_09_01},
			new object[] {PRECEDING, FRI_2014_10_31, FRI_2014_10_31},
			new object[] {PRECEDING, SAT_2014_11_01, FRI_2014_10_31},
			new object[] {PRECEDING, SUN_2014_11_02, FRI_2014_10_31},
			new object[] {PRECEDING, MON_2014_11_03, MON_2014_11_03},
			new object[] {MODIFIED_PRECEDING, FRI_2014_07_11, FRI_2014_07_11},
			new object[] {MODIFIED_PRECEDING, SAT_2014_07_12, FRI_2014_07_11},
			new object[] {MODIFIED_PRECEDING, SUN_2014_07_13, FRI_2014_07_11},
			new object[] {MODIFIED_PRECEDING, MON_2014_07_14, MON_2014_07_14},
			new object[] {MODIFIED_PRECEDING, FRI_2014_08_29, FRI_2014_08_29},
			new object[] {MODIFIED_PRECEDING, SAT_2014_08_30, FRI_2014_08_29},
			new object[] {MODIFIED_PRECEDING, SUN_2014_08_31, FRI_2014_08_29},
			new object[] {MODIFIED_PRECEDING, MON_2014_09_01, MON_2014_09_01},
			new object[] {MODIFIED_PRECEDING, FRI_2014_10_31, FRI_2014_10_31},
			new object[] {MODIFIED_PRECEDING, SAT_2014_11_01, MON_2014_11_03},
			new object[] {MODIFIED_PRECEDING, SUN_2014_11_02, MON_2014_11_03},
			new object[] {MODIFIED_PRECEDING, MON_2014_11_03, MON_2014_11_03},
			new object[] {NEAREST, FRI_2014_07_11, FRI_2014_07_11},
			new object[] {NEAREST, SAT_2014_07_12, FRI_2014_07_11},
			new object[] {NEAREST, SUN_2014_07_13, MON_2014_07_14},
			new object[] {NEAREST, MON_2014_07_14, MON_2014_07_14}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "convention") public void test_convention(BusinessDayConvention convention, java.time.LocalDate input, java.time.LocalDate expected)
	  public virtual void test_convention(BusinessDayConvention convention, LocalDate input, LocalDate expected)
	  {
		assertEquals(convention.adjust(input, HolidayCalendars.SAT_SUN), expected);
	  }

	  public virtual void test_nearest()
	  {
		HolidayCalendar cal = ImmutableHolidayCalendar.of(HolidayCalendarId.of("Test"), ImmutableList.of(MON_2014_07_14), SATURDAY, SUNDAY);
		assertEquals(NEAREST.adjust(FRI_2014_07_11, cal), FRI_2014_07_11);
		assertEquals(NEAREST.adjust(SAT_2014_07_12, cal), FRI_2014_07_11);
		assertEquals(NEAREST.adjust(SUN_2014_07_13, cal), TUE_2014_07_15);
		assertEquals(NEAREST.adjust(MON_2014_07_14, cal), TUE_2014_07_15);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {NO_ADJUST, "NoAdjust"},
			new object[] {FOLLOWING, "Following"},
			new object[] {MODIFIED_FOLLOWING, "ModifiedFollowing"},
			new object[] {MODIFIED_FOLLOWING_BI_MONTHLY, "ModifiedFollowingBiMonthly"},
			new object[] {PRECEDING, "Preceding"},
			new object[] {MODIFIED_PRECEDING, "ModifiedPreceding"},
			new object[] {NEAREST, "Nearest"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(BusinessDayConvention convention, String name)
	  public virtual void test_name(BusinessDayConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(BusinessDayConvention convention, String name)
	  public virtual void test_toString(BusinessDayConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(BusinessDayConvention convention, String name)
	  public virtual void test_of_lookup(BusinessDayConvention convention, string name)
	  {
		assertEquals(BusinessDayConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_lenientLookup_standardNames(BusinessDayConvention convention, String name)
	  public virtual void test_lenientLookup_standardNames(BusinessDayConvention convention, string name)
	  {
		assertEquals(BusinessDayConvention.extendedEnum().findLenient(name.ToLower(Locale.ENGLISH)).get(), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(BusinessDayConvention convention, String name)
	  public virtual void test_extendedEnum(BusinessDayConvention convention, string name)
	  {
		ImmutableMap<string, BusinessDayConvention> map = BusinessDayConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => BusinessDayConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => BusinessDayConvention.of(null));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "lenient") public static Object[][] data_lenient()
	  public static object[][] data_lenient()
	  {
		return new object[][]
		{
			new object[] {"F", FOLLOWING},
			new object[] {"M", MODIFIED_FOLLOWING},
			new object[] {"MF", MODIFIED_FOLLOWING},
			new object[] {"P", PRECEDING},
			new object[] {"MP", MODIFIED_PRECEDING},
			new object[] {"Modified", MODIFIED_FOLLOWING},
			new object[] {"Mod", MODIFIED_FOLLOWING},
			new object[] {"Modified Following", MODIFIED_FOLLOWING},
			new object[] {"ModifiedFollowing", MODIFIED_FOLLOWING},
			new object[] {"Mod Following", MODIFIED_FOLLOWING},
			new object[] {"ModFollowing", MODIFIED_FOLLOWING},
			new object[] {"Modified Preceding", MODIFIED_PRECEDING},
			new object[] {"ModifiedPreceding", MODIFIED_PRECEDING},
			new object[] {"Mod Preceding", MODIFIED_PRECEDING},
			new object[] {"ModPreceding", MODIFIED_PRECEDING},
			new object[] {"None", NO_ADJUST}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "lenient") public void test_lenientLookup_specialNames(String name, BusinessDayConvention convention)
	  public virtual void test_lenientLookup_specialNames(string name, BusinessDayConvention convention)
	  {
		assertEquals(BusinessDayConvention.extendedEnum().findLenient(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(BusinessDayConventions));
		coverEnum(typeof(StandardBusinessDayConventions));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(NO_ADJUST);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(BusinessDayConvention), NO_ADJUST);
		assertJodaConvert(typeof(BusinessDayConvention), MODIFIED_FOLLOWING);
	  }

	}

}