using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_12M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_15M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_18M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1D;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_21M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2D;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_35Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3D;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_40Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_45Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_4M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_4Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_50Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_6W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Tests for the tenor class.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TenorTest
	public class TenorTest
	{

	  private const object ANOTHER_TYPE = "";

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "ofPeriod") public static Object[][] data_ofPeriod()
	  public static object[][] data_ofPeriod()
	  {
		return new object[][]
		{
			new object[] {Period.ofDays(1), Period.ofDays(1), "1D"},
			new object[] {Period.ofDays(7), Period.ofDays(7), "1W"},
			new object[] {Period.ofDays(10), Period.ofDays(10), "10D"},
			new object[] {Period.ofWeeks(2), Period.ofDays(14), "2W"},
			new object[] {Period.ofMonths(1), Period.ofMonths(1), "1M"},
			new object[] {Period.ofMonths(2), Period.ofMonths(2), "2M"},
			new object[] {Period.ofMonths(12), Period.ofMonths(12), "12M"},
			new object[] {Period.ofYears(1), Period.ofYears(1), "1Y"},
			new object[] {Period.ofMonths(20), Period.ofMonths(20), "20M"},
			new object[] {Period.ofMonths(24), Period.ofMonths(24), "24M"},
			new object[] {Period.ofYears(2), Period.ofYears(2), "2Y"},
			new object[] {Period.ofMonths(30), Period.ofMonths(30), "30M"},
			new object[] {Period.of(2, 6, 0), Period.of(2, 6, 0), "2Y6M"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "ofPeriod") public void test_ofPeriod(java.time.Period period, java.time.Period stored, String str)
	  public virtual void test_ofPeriod(Period period, Period stored, string str)
	  {
		assertEquals(Tenor.of(period).Period, stored);
		assertEquals(Tenor.of(period).ToString(), str);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "ofMonths") public static Object[][] data_ofMonths()
	  public static object[][] data_ofMonths()
	  {
		return new object[][]
		{
			new object[] {1, Period.ofMonths(1), "1M"},
			new object[] {2, Period.ofMonths(2), "2M"},
			new object[] {12, Period.ofMonths(12), "12M"},
			new object[] {20, Period.ofMonths(20), "20M"},
			new object[] {24, Period.ofMonths(24), "24M"},
			new object[] {30, Period.ofMonths(30), "30M"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "ofMonths") public void test_ofMonths(int months, java.time.Period stored, String str)
	  public virtual void test_ofMonths(int months, Period stored, string str)
	  {
		assertEquals(Tenor.ofMonths(months).Period, stored);
		assertEquals(Tenor.ofMonths(months).ToString(), str);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "ofYears") public static Object[][] data_ofYears()
	  public static object[][] data_ofYears()
	  {
		return new object[][]
		{
			new object[] {1, Period.ofYears(1), "1Y"},
			new object[] {2, Period.ofYears(2), "2Y"},
			new object[] {3, Period.ofYears(3), "3Y"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "ofYears") public void test_ofYears(int years, java.time.Period stored, String str)
	  public virtual void test_ofYears(int years, Period stored, string str)
	  {
		assertEquals(Tenor.ofYears(years).Period, stored);
		assertEquals(Tenor.ofYears(years).ToString(), str);
	  }

	  public virtual void test_of_int()
	  {
		assertEquals(Tenor.ofDays(1), TENOR_1D);
		assertEquals(Tenor.ofDays(7), TENOR_1W);
		assertEquals(Tenor.ofWeeks(2), TENOR_2W);
		assertEquals(Tenor.ofMonths(1), TENOR_1M);
		assertEquals(Tenor.ofMonths(15), TENOR_15M);
		assertEquals(Tenor.ofMonths(18), TENOR_18M);
		assertEquals(Tenor.ofMonths(21), TENOR_21M);
		assertEquals(Tenor.ofYears(1), TENOR_1Y);
		assertEquals(Tenor.ofYears(35), TENOR_35Y);
		assertEquals(Tenor.ofYears(40), TENOR_40Y);
		assertEquals(Tenor.ofYears(45), TENOR_45Y);
		assertEquals(Tenor.ofYears(50), TENOR_50Y);
	  }

	  public virtual void test_of_notZero()
	  {
		assertThrowsIllegalArg(() => Tenor.of(Period.ofDays(0)));
		assertThrowsIllegalArg(() => Tenor.ofDays(0));
		assertThrowsIllegalArg(() => Tenor.ofWeeks(0));
		assertThrowsIllegalArg(() => Tenor.ofMonths(0));
		assertThrowsIllegalArg(() => Tenor.ofYears(0));
	  }

	  public virtual void test_of_notNegative()
	  {
		assertThrowsIllegalArg(() => Tenor.of(Period.ofDays(-1)));
		assertThrowsIllegalArg(() => Tenor.ofDays(-1));
		assertThrowsIllegalArg(() => Tenor.ofWeeks(-1));
		assertThrowsIllegalArg(() => Tenor.ofMonths(-1));
		assertThrowsIllegalArg(() => Tenor.ofYears(-1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parse_String_roundTrip()
	  {
		assertEquals(Tenor.parse(TENOR_10M.ToString()), TENOR_10M);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseGood") public static Object[][] data_parseGood()
	  public static object[][] data_parseGood()
	  {
		return new object[][]
		{
			new object[] {"2D", TENOR_2D},
			new object[] {"2W", TENOR_2W},
			new object[] {"6W", TENOR_6W},
			new object[] {"2M", TENOR_2M},
			new object[] {"12M", TENOR_12M},
			new object[] {"1Y", TENOR_1Y},
			new object[] {"2Y", TENOR_2Y}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseGood") public void test_parse_String_good_noP(String input, Tenor expected)
	  public virtual void test_parse_String_good_noP(string input, Tenor expected)
	  {
		assertEquals(Tenor.parse(input), expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseGood") public void test_parse_String_good_withP(String input, Tenor expected)
	  public virtual void test_parse_String_good_withP(string input, Tenor expected)
	  {
		assertEquals(Tenor.parse("P" + input), expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseBad") public static Object[][] data_parseBad()
	  public static object[][] data_parseBad()
	  {
		return new object[][]
		{
			new object[] {""},
			new object[] {"2"},
			new object[] {"2K"},
			new object[] {"-2D"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseBad", expectedExceptions = IllegalArgumentException.class) public void test_parse_String_bad(String input)
	  public virtual void test_parse_String_bad(string input)
	  {
		Tenor.parse(input);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_getPeriod()
	  {
		assertEquals(TENOR_3D.Period, Period.ofDays(3));
		assertEquals(TENOR_3W.Period, Period.ofDays(21));
		assertEquals(TENOR_3M.Period, Period.ofMonths(3));
		assertEquals(TENOR_3Y.Period, Period.ofYears(3));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "normalized") public static Object[][] data_normalized()
	  public static object[][] data_normalized()
	  {
		return new object[][]
		{
			new object[] {Period.ofDays(1), Period.ofDays(1)},
			new object[] {Period.ofDays(7), Period.ofDays(7)},
			new object[] {Period.ofDays(10), Period.ofDays(10)},
			new object[] {Period.ofWeeks(2), Period.ofDays(14)},
			new object[] {Period.ofMonths(1), Period.ofMonths(1)},
			new object[] {Period.ofMonths(2), Period.ofMonths(2)},
			new object[] {Period.ofMonths(12), Period.ofMonths(12)},
			new object[] {Period.ofYears(1), Period.ofMonths(12)},
			new object[] {Period.ofMonths(20), Period.of(1, 8, 0)},
			new object[] {Period.ofMonths(24), Period.ofYears(2)},
			new object[] {Period.ofYears(2), Period.ofYears(2)},
			new object[] {Period.ofMonths(30), Period.of(2, 6, 0)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "normalized") public void test_normalized(java.time.Period period, java.time.Period normalized)
	  public virtual void test_normalized(Period period, Period normalized)
	  {
		assertEquals(Tenor.of(period).normalized().Period, normalized);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "based") public static Object[][] data_based()
	  public static object[][] data_based()
	  {
		return new object[][]
		{
			new object[] {Tenor.ofDays(1), false, false},
			new object[] {Tenor.ofDays(2), false, false},
			new object[] {Tenor.ofDays(6), false, false},
			new object[] {Tenor.ofDays(7), true, false},
			new object[] {Tenor.ofWeeks(1), true, false},
			new object[] {Tenor.ofWeeks(3), true, false},
			new object[] {Tenor.ofMonths(1), false, true},
			new object[] {Tenor.ofMonths(3), false, true},
			new object[] {Tenor.ofYears(1), false, true},
			new object[] {Tenor.ofYears(3), false, true},
			new object[] {Tenor.of(Period.of(1, 2, 3)), false, false}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "based") public void test_isWeekBased(Tenor test, boolean weekBased, boolean monthBased)
	  public virtual void test_isWeekBased(Tenor test, bool weekBased, bool monthBased)
	  {
		assertEquals(test.WeekBased, weekBased);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "based") public void test_isMonthBased(Tenor test, boolean weekBased, boolean monthBased)
	  public virtual void test_isMonthBased(Tenor test, bool weekBased, bool monthBased)
	  {
		assertEquals(test.MonthBased, monthBased);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_addTo()
	  {
		assertEquals(TENOR_3D.addTo(LocalDate.of(2014, 6, 30)), LocalDate.of(2014, 7, 3));
		assertEquals(TENOR_1W.addTo(OffsetDateTime.of(2014, 6, 30, 0, 0, 0, 0, ZoneOffset.UTC)), OffsetDateTime.of(2014, 7, 7, 0, 0, 0, 0, ZoneOffset.UTC));
	  }

	  public virtual void test_subtractFrom()
	  {
		assertEquals(TENOR_3D.subtractFrom(LocalDate.of(2014, 6, 30)), LocalDate.of(2014, 6, 27));
		assertEquals(TENOR_1W.subtractFrom(OffsetDateTime.of(2014, 6, 30, 0, 0, 0, 0, ZoneOffset.UTC)), OffsetDateTime.of(2014, 6, 23, 0, 0, 0, 0, ZoneOffset.UTC));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_temporalAmount()
	  {
		assertEquals(TENOR_3D.Units, ImmutableList.of(YEARS, MONTHS, DAYS));
		assertEquals(TENOR_3D.get(DAYS), 3);
		assertEquals(LocalDate.of(2014, 6, 30).plus(TENOR_1W), LocalDate.of(2014, 7, 7));
		assertEquals(LocalDate.of(2014, 6, 30).minus(TENOR_1W), LocalDate.of(2014, 6, 23));
		assertThrows(() => TENOR_10M.get(CENTURIES), typeof(UnsupportedTemporalTypeException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compare()
	  {
		IList<Tenor> tenors = ImmutableList.of(Tenor.ofDays(1), Tenor.ofDays(3), Tenor.ofDays(7), Tenor.ofWeeks(2), Tenor.ofWeeks(4), Tenor.ofDays(30), Tenor.ofMonths(1), Tenor.ofDays(31), Tenor.of(Period.of(0, 1, 1)), Tenor.ofDays(60), Tenor.ofMonths(2), Tenor.ofDays(61), Tenor.ofDays(91), Tenor.ofMonths(3), Tenor.ofDays(92), Tenor.ofDays(182), Tenor.ofMonths(6), Tenor.ofDays(183), Tenor.ofDays(365), Tenor.ofYears(1), Tenor.ofDays(366));

		IList<Tenor> test = new List<Tenor>(tenors);
		Collections.shuffle(test);
		test.Sort();
		assertEquals(test, tenors);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals_hashCode()
	  {
		Tenor a1 = TENOR_3D;
		Tenor a2 = Tenor.ofDays(3);
		Tenor b = TENOR_4M;
		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(a2), true);

		assertEquals(a2.Equals(a1), true);
		assertEquals(a2.Equals(a2), true);
		assertEquals(a2.Equals(b), false);

		assertEquals(b.Equals(a1), false);
		assertEquals(b.Equals(a2), false);
		assertEquals(b.Equals(b), true);

		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void test_equals_bad()
	  {
		assertEquals(TENOR_3D.Equals(null), false);
		assertEquals(TENOR_3D.Equals(ANOTHER_TYPE), false);
		assertEquals(TENOR_3D.Equals(new object()), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toString()
	  {
		assertEquals(TENOR_3D.ToString(), "3D");
		assertEquals(TENOR_2W.ToString(), "2W");
		assertEquals(TENOR_4M.ToString(), "4M");
		assertEquals(TENOR_12M.ToString(), "12M");
		assertEquals(TENOR_1Y.ToString(), "1Y");
		assertEquals(TENOR_18M.ToString(), "18M");
		assertEquals(TENOR_4Y.ToString(), "4Y");
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(TENOR_3D);
		assertSerialization(TENOR_4M);
		assertSerialization(TENOR_3Y);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(Tenor), TENOR_3D);
		assertJodaConvert(typeof(Tenor), TENOR_4M);
		assertJodaConvert(typeof(Tenor), TENOR_3Y);
	  }

	}

}