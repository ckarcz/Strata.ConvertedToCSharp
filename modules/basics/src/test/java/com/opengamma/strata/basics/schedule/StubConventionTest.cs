/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P2W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.TERM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_14;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_16;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_30;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_SAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_TUE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.EOM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.BOTH;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.LONG_FINAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.LONG_INITIAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.NONE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.SHORT_FINAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.SHORT_INITIAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.SMART_FINAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.StubConvention.SMART_INITIAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="StubConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class StubConventionTest
	public class StubConventionTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "types") public static Object[][] data_types()
		public static object[][] data_types()
		{
		StubConvention[] conv = StubConvention.values();
		object[][] result = new object[conv.Length][];
		for (int i = 0; i < conv.Length; i++)
		{
		  result[i] = new object[] {conv[i]};
		}
		return result;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "types") public void test_null(StubConvention type)
	  public virtual void test_null(StubConvention type)
	  {
		assertThrowsIllegalArg(() => type.toRollConvention(null, date(2014, JULY, 1), Frequency.P3M, true));
		assertThrowsIllegalArg(() => type.toRollConvention(date(2014, JULY, 1), null, Frequency.P3M, true));
		assertThrowsIllegalArg(() => type.toRollConvention(date(2014, JULY, 1), date(2014, OCTOBER, 1), null, true));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "roll") public static Object[][] data_roll()
	  public static object[][] data_roll()
	  {
		return new object[][]
		{
			new object[] {NONE, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, false, DAY_14},
			new object[] {NONE, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, true, DAY_14},
			new object[] {NONE, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, false, DAY_TUE},
			new object[] {NONE, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, true, DAY_TUE},
			new object[] {NONE, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, false, RollConventions.NONE},
			new object[] {NONE, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, true, RollConventions.NONE},
			new object[] {NONE, date(2014, JANUARY, 31), date(2014, APRIL, 30), P1M, true, RollConventions.EOM},
			new object[] {NONE, date(2014, APRIL, 30), date(2014, AUGUST, 31), P1M, true, RollConventions.EOM},
			new object[] {NONE, date(2014, APRIL, 30), date(2014, FEBRUARY, 28), P1M, true, RollConventions.EOM},
			new object[] {NONE, date(2016, FEBRUARY, 29), date(2019, FEBRUARY, 28), P6M, true, RollConventions.EOM},
			new object[] {NONE, date(2015, FEBRUARY, 28), date(2016, FEBRUARY, 29), P6M, true, RollConventions.EOM},
			new object[] {NONE, date(2015, APRIL, 30), date(2016, FEBRUARY, 29), P1M, true, RollConventions.EOM},
			new object[] {NONE, date(2016, MARCH, 31), date(2017, MARCH, 27), P6M, true, RollConventions.EOM},
			new object[] {NONE, date(2016, MARCH, 16), date(2016, MARCH, 31), P6M, true, RollConvention.ofDayOfMonth(16)},
			new object[] {NONE, date(2016, MARCH, 16), date(2017, MARCH, 31), P6M, true, RollConventions.EOM},
			new object[] {SHORT_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, false, DAY_16},
			new object[] {SHORT_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, true, DAY_16},
			new object[] {SHORT_INITIAL, date(2014, JANUARY, 14), date(2014, JUNE, 30), P1M, false, DAY_30},
			new object[] {SHORT_INITIAL, date(2014, JANUARY, 14), date(2014, JUNE, 30), P1M, true, EOM},
			new object[] {SHORT_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, true, DAY_SAT},
			new object[] {SHORT_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, true, DAY_SAT},
			new object[] {SHORT_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, false, RollConventions.NONE},
			new object[] {SHORT_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, true, RollConventions.NONE},
			new object[] {LONG_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, false, DAY_16},
			new object[] {LONG_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, true, DAY_16},
			new object[] {LONG_INITIAL, date(2014, JANUARY, 14), date(2014, JUNE, 30), P1M, false, DAY_30},
			new object[] {LONG_INITIAL, date(2014, JANUARY, 14), date(2014, JUNE, 30), P1M, true, EOM},
			new object[] {LONG_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, false, DAY_SAT},
			new object[] {LONG_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, true, DAY_SAT},
			new object[] {LONG_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, false, RollConventions.NONE},
			new object[] {LONG_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, true, RollConventions.NONE},
			new object[] {SMART_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, false, DAY_16},
			new object[] {SMART_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, true, DAY_16},
			new object[] {SMART_INITIAL, date(2014, JANUARY, 14), date(2014, JUNE, 30), P1M, false, DAY_30},
			new object[] {SMART_INITIAL, date(2014, JANUARY, 14), date(2014, JUNE, 30), P1M, true, EOM},
			new object[] {SMART_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, false, DAY_SAT},
			new object[] {SMART_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, true, DAY_SAT},
			new object[] {SMART_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, false, RollConventions.NONE},
			new object[] {SMART_INITIAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, true, RollConventions.NONE},
			new object[] {SHORT_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, false, DAY_14},
			new object[] {SHORT_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, true, DAY_14},
			new object[] {SHORT_FINAL, date(2014, JUNE, 30), date(2014, AUGUST, 16), P1M, false, DAY_30},
			new object[] {SHORT_FINAL, date(2014, JUNE, 30), date(2014, AUGUST, 16), P1M, true, EOM},
			new object[] {SHORT_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, false, DAY_TUE},
			new object[] {SHORT_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, true, DAY_TUE},
			new object[] {SHORT_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, false, RollConventions.NONE},
			new object[] {SHORT_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, true, RollConventions.NONE},
			new object[] {LONG_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, false, DAY_14},
			new object[] {LONG_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, true, DAY_14},
			new object[] {LONG_FINAL, date(2014, JUNE, 30), date(2014, AUGUST, 16), P1M, false, DAY_30},
			new object[] {LONG_FINAL, date(2014, JUNE, 30), date(2014, AUGUST, 16), P1M, true, EOM},
			new object[] {LONG_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, false, DAY_TUE},
			new object[] {LONG_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, true, DAY_TUE},
			new object[] {LONG_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, false, RollConventions.NONE},
			new object[] {LONG_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, true, RollConventions.NONE},
			new object[] {SMART_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, false, DAY_14},
			new object[] {SMART_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, true, DAY_14},
			new object[] {SMART_FINAL, date(2014, JUNE, 30), date(2014, AUGUST, 16), P1M, false, DAY_30},
			new object[] {SMART_FINAL, date(2014, JUNE, 30), date(2014, AUGUST, 16), P1M, true, EOM},
			new object[] {SMART_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, false, DAY_TUE},
			new object[] {SMART_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P2W, true, DAY_TUE},
			new object[] {SMART_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, false, RollConventions.NONE},
			new object[] {SMART_FINAL, date(2014, JANUARY, 14), date(2014, AUGUST, 16), TERM, true, RollConventions.NONE},
			new object[] {BOTH, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, false, DAY_14},
			new object[] {BOTH, date(2014, JANUARY, 14), date(2014, AUGUST, 16), P1M, true, DAY_14}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "roll") public void test_toRollConvention(StubConvention conv, java.time.LocalDate start, java.time.LocalDate end, Frequency freq, boolean eom, RollConvention expected)
	  public virtual void test_toRollConvention(StubConvention conv, LocalDate start, LocalDate end, Frequency freq, bool eom, RollConvention expected)
	  {
		assertEquals(conv.toRollConvention(start, end, freq, eom), expected);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "implicit") public static Object[][] data_implicit()
	  public static object[][] data_implicit()
	  {
		return new object[][]
		{
			new object[] {NONE, false, false, NONE},
			new object[] {NONE, true, false, null},
			new object[] {NONE, false, true, null},
			new object[] {NONE, true, true, null},
			new object[] {SHORT_INITIAL, false, false, SHORT_INITIAL},
			new object[] {SHORT_INITIAL, true, false, NONE},
			new object[] {SHORT_INITIAL, false, true, null},
			new object[] {SHORT_INITIAL, true, true, null},
			new object[] {LONG_INITIAL, false, false, LONG_INITIAL},
			new object[] {LONG_INITIAL, true, false, NONE},
			new object[] {LONG_INITIAL, false, true, null},
			new object[] {LONG_INITIAL, true, true, null},
			new object[] {SMART_INITIAL, false, false, SMART_INITIAL},
			new object[] {SMART_INITIAL, true, false, NONE},
			new object[] {SMART_INITIAL, false, true, null},
			new object[] {SMART_INITIAL, true, true, null},
			new object[] {SHORT_FINAL, false, false, SHORT_FINAL},
			new object[] {SHORT_FINAL, true, false, null},
			new object[] {SHORT_FINAL, false, true, NONE},
			new object[] {SHORT_FINAL, true, true, null},
			new object[] {LONG_FINAL, false, false, LONG_FINAL},
			new object[] {LONG_FINAL, true, false, null},
			new object[] {LONG_FINAL, false, true, NONE},
			new object[] {LONG_FINAL, true, true, null},
			new object[] {SMART_FINAL, false, false, SMART_FINAL},
			new object[] {SMART_FINAL, true, false, null},
			new object[] {SMART_FINAL, false, true, NONE},
			new object[] {SMART_FINAL, true, true, null},
			new object[] {BOTH, false, false, null},
			new object[] {BOTH, true, false, null},
			new object[] {BOTH, false, true, null},
			new object[] {BOTH, true, true, NONE}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "implicit") public void test_toImplicit(StubConvention conv, boolean initialStub, boolean finalStub, StubConvention expected)
	  public virtual void test_toImplicit(StubConvention conv, bool initialStub, bool finalStub, StubConvention expected)
	  {
		if (expected == null)
		{
		  assertThrowsIllegalArg(() => conv.toImplicit(null, initialStub, finalStub));
		}
		else
		{
		  assertEquals(conv.toImplicit(null, initialStub, finalStub), expected);
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "isStubLong") public static Object[][] data_isStubLong()
	  public static object[][] data_isStubLong()
	  {
		return new object[][]
		{
			new object[] {NONE, date(2018, 6, 1), date(2018, 6, 8), false},
			new object[] {SHORT_INITIAL, date(2018, 6, 1), date(2018, 6, 8), false},
			new object[] {LONG_INITIAL, date(2018, 6, 1), date(2018, 6, 8), true},
			new object[] {SHORT_FINAL, date(2018, 6, 1), date(2018, 6, 8), false},
			new object[] {LONG_FINAL, date(2018, 6, 1), date(2018, 6, 8), true},
			new object[] {BOTH, date(2018, 6, 1), date(2018, 6, 8), false},
			new object[] {SMART_INITIAL, date(2018, 6, 1), date(2018, 6, 2), true},
			new object[] {SMART_INITIAL, date(2018, 6, 1), date(2018, 6, 7), true},
			new object[] {SMART_INITIAL, date(2018, 6, 1), date(2018, 6, 8), false},
			new object[] {SMART_INITIAL, date(2018, 6, 1), date(2018, 6, 9), false},
			new object[] {SMART_FINAL, date(2018, 6, 1), date(2018, 6, 2), true},
			new object[] {SMART_FINAL, date(2018, 6, 1), date(2018, 6, 7), true},
			new object[] {SMART_FINAL, date(2018, 6, 1), date(2018, 6, 8), false},
			new object[] {SMART_FINAL, date(2018, 6, 1), date(2018, 6, 9), false}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "isStubLong") public void test_isStubLong(StubConvention conv, java.time.LocalDate date1, java.time.LocalDate date2, System.Nullable<bool> expected)
	  public virtual void test_isStubLong(StubConvention conv, LocalDate date1, LocalDate date2, bool? expected)
	  {
		if (expected == null)
		{
		  assertThrowsIllegalArg(() => conv.isStubLong(date1, date2));
		}
		else
		{
		  assertEquals(conv.isStubLong(date1, date2), expected.Value);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_NONE()
	  {
		assertEquals(NONE.CalculateForwards, true);
		assertEquals(NONE.CalculateBackwards, false);
		assertEquals(NONE.Long, false);
		assertEquals(NONE.Short, false);
		assertEquals(NONE.Smart, false);
	  }

	  public virtual void test_SHORT_INITIAL()
	  {
		assertEquals(SHORT_INITIAL.CalculateForwards, false);
		assertEquals(SHORT_INITIAL.CalculateBackwards, true);
		assertEquals(SHORT_INITIAL.Long, false);
		assertEquals(SHORT_INITIAL.Short, true);
		assertEquals(SHORT_INITIAL.Smart, false);
	  }

	  public virtual void test_LONG_INITIAL()
	  {
		assertEquals(LONG_INITIAL.CalculateForwards, false);
		assertEquals(LONG_INITIAL.CalculateBackwards, true);
		assertEquals(LONG_INITIAL.Long, true);
		assertEquals(LONG_INITIAL.Short, false);
		assertEquals(LONG_INITIAL.Smart, false);
	  }

	  public virtual void test_SMART_INITIAL()
	  {
		assertEquals(SMART_INITIAL.CalculateForwards, false);
		assertEquals(SMART_INITIAL.CalculateBackwards, true);
		assertEquals(SMART_INITIAL.Long, false);
		assertEquals(SMART_INITIAL.Short, false);
		assertEquals(SMART_INITIAL.Smart, true);
	  }

	  public virtual void test_SHORT_FINAL()
	  {
		assertEquals(SHORT_FINAL.CalculateForwards, true);
		assertEquals(SHORT_FINAL.CalculateBackwards, false);
		assertEquals(SHORT_FINAL.Long, false);
		assertEquals(SHORT_FINAL.Short, true);
		assertEquals(SHORT_FINAL.Smart, false);
	  }

	  public virtual void test_LONG_FINAL()
	  {
		assertEquals(LONG_FINAL.CalculateForwards, true);
		assertEquals(LONG_FINAL.CalculateBackwards, false);
		assertEquals(LONG_FINAL.Long, true);
		assertEquals(LONG_FINAL.Short, false);
		assertEquals(LONG_FINAL.Smart, false);
	  }

	  public virtual void test_SMART_FINAL()
	  {
		assertEquals(SMART_FINAL.CalculateForwards, true);
		assertEquals(SMART_FINAL.CalculateBackwards, false);
		assertEquals(SMART_FINAL.Long, false);
		assertEquals(SMART_FINAL.Short, false);
		assertEquals(SMART_FINAL.Smart, true);
	  }

	  public virtual void test_BOTH()
	  {
		assertEquals(BOTH.CalculateForwards, false);
		assertEquals(BOTH.CalculateBackwards, false);
		assertEquals(BOTH.Long, false);
		assertEquals(BOTH.Short, false);
		assertEquals(BOTH.Smart, false);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {NONE, "None"},
			new object[] {SHORT_INITIAL, "ShortInitial"},
			new object[] {LONG_INITIAL, "LongInitial"},
			new object[] {SMART_INITIAL, "SmartInitial"},
			new object[] {SHORT_FINAL, "ShortFinal"},
			new object[] {LONG_FINAL, "LongFinal"},
			new object[] {SMART_FINAL, "SmartFinal"},
			new object[] {BOTH, "Both"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(StubConvention convention, String name)
	  public virtual void test_toString(StubConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(StubConvention convention, String name)
	  public virtual void test_of_lookup(StubConvention convention, string name)
	  {
		assertEquals(StubConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(StubConvention convention, String name)
	  public virtual void test_of_lookupUpperCase(StubConvention convention, string name)
	  {
		assertEquals(StubConvention.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(StubConvention convention, String name)
	  public virtual void test_of_lookupLowerCase(StubConvention convention, string name)
	  {
		assertEquals(StubConvention.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => StubConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => StubConvention.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(StubConvention));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(NONE);
		assertSerialization(SHORT_FINAL);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(StubConvention), NONE);
		assertJodaConvert(typeof(StubConvention), SHORT_FINAL);
	  }

	}

}