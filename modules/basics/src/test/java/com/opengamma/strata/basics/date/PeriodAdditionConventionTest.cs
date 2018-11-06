/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.PeriodAdditionConventions.LAST_BUSINESS_DAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.PeriodAdditionConventions.LAST_DAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.PeriodAdditionConventions.NONE;
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
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Test <seealso cref="PeriodAdditionConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PeriodAdditionConventionTest
	public class PeriodAdditionConventionTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "types") public static Object[][] data_types()
		public static object[][] data_types()
		{
		StandardPeriodAdditionConventions[] conv = StandardPeriodAdditionConventions.values();
		object[][] result = new object[conv.Length][];
		for (int i = 0; i < conv.Length; i++)
		{
		  result[i] = new object[] {conv[i]};
		}
		return result;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "types") public void test_null(PeriodAdditionConvention type)
	  public virtual void test_null(PeriodAdditionConvention type)
	  {
		assertThrowsIllegalArg(() => type.adjust(null, Period.ofMonths(3), HolidayCalendars.NO_HOLIDAYS));
		assertThrowsIllegalArg(() => type.adjust(date(2014, 7, 11), null, HolidayCalendars.NO_HOLIDAYS));
		assertThrowsIllegalArg(() => type.adjust(date(2014, 7, 11), Period.ofMonths(3), null));
		assertThrowsIllegalArg(() => type.adjust(null, null, null));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "convention") public static Object[][] data_convention()
	  public static object[][] data_convention()
	  {
		return new object[][]
		{
			new object[] {NONE, date(2014, 7, 11), 1, date(2014, 8, 11)},
			new object[] {NONE, date(2014, 7, 31), 1, date(2014, 8, 31)},
			new object[] {NONE, date(2014, 6, 30), 2, date(2014, 8, 30)},
			new object[] {LAST_DAY, date(2014, 7, 11), 1, date(2014, 8, 11)},
			new object[] {LAST_DAY, date(2014, 7, 31), 1, date(2014, 8, 31)},
			new object[] {LAST_DAY, date(2014, 6, 30), 2, date(2014, 8, 31)},
			new object[] {LAST_BUSINESS_DAY, date(2014, 7, 11), 1, date(2014, 8, 11)},
			new object[] {LAST_BUSINESS_DAY, date(2014, 7, 31), 1, date(2014, 8, 29)},
			new object[] {LAST_BUSINESS_DAY, date(2014, 6, 30), 2, date(2014, 8, 29)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "convention") public void test_convention(PeriodAdditionConvention convention, java.time.LocalDate input, int months, java.time.LocalDate expected)
	  public virtual void test_convention(PeriodAdditionConvention convention, LocalDate input, int months, LocalDate expected)
	  {
		assertEquals(convention.adjust(input, Period.ofMonths(months), HolidayCalendars.SAT_SUN), expected);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {NONE, "None"},
			new object[] {LAST_DAY, "LastDay"},
			new object[] {LAST_BUSINESS_DAY, "LastBusinessDay"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(PeriodAdditionConvention convention, String name)
	  public virtual void test_name(PeriodAdditionConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(PeriodAdditionConvention convention, String name)
	  public virtual void test_toString(PeriodAdditionConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(PeriodAdditionConvention convention, String name)
	  public virtual void test_of_lookup(PeriodAdditionConvention convention, string name)
	  {
		assertEquals(PeriodAdditionConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(PeriodAdditionConvention convention, String name)
	  public virtual void test_extendedEnum(PeriodAdditionConvention convention, string name)
	  {
		ImmutableMap<string, PeriodAdditionConvention> map = PeriodAdditionConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => PeriodAdditionConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => PeriodAdditionConvention.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(PeriodAdditionConventions));
		coverEnum(typeof(StandardPeriodAdditionConventions));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(LAST_DAY);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(PeriodAdditionConvention), NONE);
		assertJodaConvert(typeof(PeriodAdditionConvention), LAST_BUSINESS_DAY);
	  }

	}

}