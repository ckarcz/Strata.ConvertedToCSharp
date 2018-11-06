/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P12M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1D;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.TERM;
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
	/// Tests for the frequency class.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FrequencyTest
	public class FrequencyTest
	{

	  private const object ANOTHER_TYPE = "";

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "create") public static Object[][] data_create()
	  public static object[][] data_create()
	  {
		return new object[][]
		{
			new object[] {Frequency.ofDays(1), Period.ofDays(1), "P1D"},
			new object[] {Frequency.ofDays(2), Period.ofDays(2), "P2D"},
			new object[] {Frequency.ofDays(6), Period.ofDays(6), "P6D"},
			new object[] {Frequency.ofDays(7), Period.ofDays(7), "P1W"},
			new object[] {Frequency.ofDays(91), Period.ofDays(91), "P13W"},
			new object[] {Frequency.ofWeeks(1), Period.ofDays(7), "P1W"},
			new object[] {Frequency.ofWeeks(3), Period.ofDays(21), "P3W"},
			new object[] {Frequency.ofMonths(8), Period.ofMonths(8), "P8M"},
			new object[] {Frequency.ofMonths(12), Period.ofMonths(12), "P12M"},
			new object[] {Frequency.ofMonths(18), Period.ofMonths(18), "P18M"},
			new object[] {Frequency.ofMonths(24), Period.ofMonths(24), "P24M"},
			new object[] {Frequency.ofMonths(30), Period.ofMonths(30), "P30M"},
			new object[] {Frequency.ofYears(1), Period.ofYears(1), "P1Y"},
			new object[] {Frequency.ofYears(2), Period.ofYears(2), "P2Y"},
			new object[] {Frequency.of(Period.of(1, 2, 3)), Period.of(1, 2, 3), "P1Y2M3D"},
			new object[] {Frequency.P1D, Period.ofDays(1), "P1D"},
			new object[] {Frequency.P1W, Period.ofWeeks(1), "P1W"},
			new object[] {Frequency.P2W, Period.ofWeeks(2), "P2W"},
			new object[] {Frequency.P4W, Period.ofWeeks(4), "P4W"},
			new object[] {Frequency.P13W, Period.ofWeeks(13), "P13W"},
			new object[] {Frequency.P26W, Period.ofWeeks(26), "P26W"},
			new object[] {Frequency.P52W, Period.ofWeeks(52), "P52W"},
			new object[] {Frequency.P1M, Period.ofMonths(1), "P1M"},
			new object[] {Frequency.P2M, Period.ofMonths(2), "P2M"},
			new object[] {Frequency.P3M, Period.ofMonths(3), "P3M"},
			new object[] {Frequency.P4M, Period.ofMonths(4), "P4M"},
			new object[] {Frequency.P6M, Period.ofMonths(6), "P6M"},
			new object[] {Frequency.P12M, Period.ofMonths(12), "P12M"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "create") public void test_of_int(Frequency test, java.time.Period period, String toString)
	  public virtual void test_of_int(Frequency test, Period period, string toString)
	  {
		assertEquals(test.Period, period);
		assertEquals(test.ToString(), toString);
		assertEquals(test.Term, false);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "create") public void test_of_Period(Frequency test, java.time.Period period, String toString)
	  public virtual void test_of_Period(Frequency test, Period period, string toString)
	  {
		assertEquals(Frequency.of(period), test);
		assertEquals(Frequency.of(period).Period, period);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "create") public void test_parse(Frequency test, java.time.Period period, String toString)
	  public virtual void test_parse(Frequency test, Period period, string toString)
	  {
		assertEquals(Frequency.parse(toString), test);
		assertEquals(Frequency.parse(toString).Period, period);
	  }

	  public virtual void test_term()
	  {
		assertEquals(TERM.Period, Period.ofYears(10_000));
		assertEquals(TERM.Term, true);
		assertEquals(TERM.ToString(), "Term");
		assertEquals(Frequency.parse("Term"), TERM);
		assertEquals(Frequency.parse("0T"), TERM);
		assertEquals(Frequency.parse("1T"), TERM);
	  }

	  public virtual void test_of_notZero()
	  {
		assertThrowsIllegalArg(() => Frequency.of(Period.ofDays(0)));
		assertThrowsIllegalArg(() => Frequency.ofDays(0));
		assertThrowsIllegalArg(() => Frequency.ofWeeks(0));
		assertThrowsIllegalArg(() => Frequency.ofMonths(0));
		assertThrowsIllegalArg(() => Frequency.ofYears(0));
	  }

	  public virtual void test_of_notNegative()
	  {
		assertThrowsIllegalArg(() => Frequency.of(Period.ofDays(-1)));
		assertThrowsIllegalArg(() => Frequency.of(Period.ofMonths(-1)));
		assertThrowsIllegalArg(() => Frequency.of(Period.of(0, -1, -1)));
		assertThrowsIllegalArg(() => Frequency.of(Period.of(0, -1, 1)));
		assertThrowsIllegalArg(() => Frequency.of(Period.of(0, 1, -1)));
		assertThrowsIllegalArg(() => Frequency.ofDays(-1));
		assertThrowsIllegalArg(() => Frequency.ofWeeks(-1));
		assertThrowsIllegalArg(() => Frequency.ofMonths(-1));
		assertThrowsIllegalArg(() => Frequency.ofYears(-1));
	  }

	  public virtual void test_of_tooBig()
	  {
		assertThrowsIllegalArg(() => Frequency.of(Period.ofMonths(12001)));
		assertThrowsIllegalArg(() => Frequency.of(Period.ofMonths(int.MaxValue)));

		assertThrowsIllegalArg(() => Frequency.of(Period.ofYears(1001)));
		assertThrowsIllegalArg(() => Frequency.of(Period.ofYears(int.MaxValue)));

		assertThrowsIllegalArg(() => Frequency.ofMonths(12001), "Months must not exceed 12,000");
		assertThrowsIllegalArg(() => Frequency.ofMonths(int.MaxValue));

		assertThrowsIllegalArg(() => Frequency.ofYears(1001), "Years must not exceed 1,000");
		assertThrowsIllegalArg(() => Frequency.ofYears(int.MaxValue));

		assertThrowsIllegalArg(() => Frequency.of(Period.of(10000, 0, 1)));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "ofMonths") public static Object[][] data_ofMonths()
	  public static object[][] data_ofMonths()
	  {
		return new object[][]
		{
			new object[] {1, Period.ofMonths(1), "P1M"},
			new object[] {2, Period.ofMonths(2), "P2M"},
			new object[] {3, Period.ofMonths(3), "P3M"},
			new object[] {4, Period.ofMonths(4), "P4M"},
			new object[] {6, Period.ofMonths(6), "P6M"},
			new object[] {12, Period.ofMonths(12), "P12M"},
			new object[] {20, Period.ofMonths(20), "P20M"},
			new object[] {24, Period.ofMonths(24), "P24M"},
			new object[] {30, Period.ofMonths(30), "P30M"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "ofMonths") public void test_ofMonths(int months, java.time.Period normalized, String str)
	  public virtual void test_ofMonths(int months, Period normalized, string str)
	  {
		assertEquals(Frequency.ofMonths(months).Period, normalized);
		assertEquals(Frequency.ofMonths(months).ToString(), str);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "ofYears") public static Object[][] data_ofYears()
	  public static object[][] data_ofYears()
	  {
		return new object[][]
		{
			new object[] {1, Period.ofYears(1), "P1Y"},
			new object[] {2, Period.ofYears(2), "P2Y"},
			new object[] {3, Period.ofYears(3), "P3Y"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "ofYears") public void test_ofYears(int years, java.time.Period normalized, String str)
	  public virtual void test_ofYears(int years, Period normalized, string str)
	  {
		assertEquals(Frequency.ofYears(years).Period, normalized);
		assertEquals(Frequency.ofYears(years).ToString(), str);
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
			new object[] {Period.ofMonths(12), Period.ofYears(1)},
			new object[] {Period.ofYears(1), Period.ofYears(1)},
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
		assertEquals(Frequency.of(period).normalized().Period, normalized);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "based") public static Object[][] data_based()
	  public static object[][] data_based()
	  {
		return new object[][]
		{
			new object[] {Frequency.ofDays(1), false, false, false},
			new object[] {Frequency.ofDays(2), false, false, false},
			new object[] {Frequency.ofDays(6), false, false, false},
			new object[] {Frequency.ofDays(7), true, false, false},
			new object[] {Frequency.ofWeeks(1), true, false, false},
			new object[] {Frequency.ofWeeks(3), true, false, false},
			new object[] {Frequency.ofMonths(1), false, true, false},
			new object[] {Frequency.ofMonths(3), false, true, false},
			new object[] {Frequency.ofMonths(12), false, true, true},
			new object[] {Frequency.ofYears(1), false, true, true},
			new object[] {Frequency.ofYears(3), false, true, false},
			new object[] {Frequency.of(Period.of(1, 2, 3)), false, false, false},
			new object[] {Frequency.TERM, false, false, false}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "based") public void test_isWeekBased(Frequency test, boolean weekBased, boolean monthBased, boolean annual)
	  public virtual void test_isWeekBased(Frequency test, bool weekBased, bool monthBased, bool annual)
	  {
		assertEquals(test.WeekBased, weekBased);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "based") public void test_isMonthBased(Frequency test, boolean weekBased, boolean monthBased, boolean annual)
	  public virtual void test_isMonthBased(Frequency test, bool weekBased, bool monthBased, bool annual)
	  {
		assertEquals(test.MonthBased, monthBased);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "based") public void test_isAnnual(Frequency test, boolean weekBased, boolean monthBased, boolean annual)
	  public virtual void test_isAnnual(Frequency test, bool weekBased, bool monthBased, bool annual)
	  {
		assertEquals(test.Annual, annual);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "events") public static Object[][] data_events()
	  public static object[][] data_events()
	  {
		return new object[][]
		{
			new object[] {Frequency.P1D, 364},
			new object[] {Frequency.P1W, 52},
			new object[] {Frequency.P2W, 26},
			new object[] {Frequency.P4W, 13},
			new object[] {Frequency.P13W, 4},
			new object[] {Frequency.P26W, 2},
			new object[] {Frequency.P52W, 1},
			new object[] {Frequency.P1M, 12},
			new object[] {Frequency.P2M, 6},
			new object[] {Frequency.P3M, 4},
			new object[] {Frequency.P4M, 3},
			new object[] {Frequency.P6M, 2},
			new object[] {Frequency.P12M, 1},
			new object[] {Frequency.TERM, 0}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "events") public void test_eventsPerYear(Frequency test, int expected)
	  public virtual void test_eventsPerYear(Frequency test, int expected)
	  {
		assertEquals(test.eventsPerYear(), expected);
	  }

	  public virtual void test_eventsPerYear_bad()
	  {
		assertThrowsIllegalArg(() => Frequency.ofDays(3).eventsPerYear());
		assertThrowsIllegalArg(() => Frequency.ofWeeks(3).eventsPerYear());
		assertThrowsIllegalArg(() => Frequency.ofWeeks(104).eventsPerYear());
		assertThrowsIllegalArg(() => Frequency.ofMonths(5).eventsPerYear());
		assertThrowsIllegalArg(() => Frequency.ofMonths(24).eventsPerYear());
		assertThrowsIllegalArg(() => Frequency.of(Period.of(2, 2, 2)).eventsPerYear());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "events") public void test_eventsPerYearEstimate(Frequency test, int expected)
	  public virtual void test_eventsPerYearEstimate(Frequency test, int expected)
	  {
		assertEquals(test.eventsPerYearEstimate(), expected, 1e-8);
	  }

	  public virtual void test_eventsPerYearEstimate_bad()
	  {
		assertEquals(Frequency.ofDays(3).eventsPerYearEstimate(), 364d / 3, 1e-8);
		assertEquals(Frequency.ofWeeks(3).eventsPerYearEstimate(), 364d / 21, 1e-8);
		assertEquals(Frequency.ofWeeks(104).eventsPerYearEstimate(), 364d / 728, 1e-8);
		assertEquals(Frequency.ofMonths(5).eventsPerYearEstimate(), 12d / 5, 1e-8);
		assertEquals(Frequency.ofMonths(22).eventsPerYearEstimate(), 12d / 22, 1e-8);
		assertEquals(Frequency.ofMonths(24).eventsPerYearEstimate(), 12d / 24, 1e-8);
		assertEquals(Frequency.ofYears(2).eventsPerYearEstimate(), 0.5d, 1e-8);
		assertEquals(Frequency.of(Period.of(10, 0, 1)).eventsPerYearEstimate(), 0.1d, 1e-3);
		assertEquals(Frequency.of(Period.of(5, 0, 95)).eventsPerYearEstimate(), 0.19d, 1e-3);
		assertEquals(Frequency.of(Period.of(5, 0, 97)).eventsPerYearEstimate(), 0.19d, 1e-3);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "exactDivide") public static Object[][] data_exactDivide()
	  public static object[][] data_exactDivide()
	  {
		return new object[][]
		{
			new object[] {Frequency.P1D, Frequency.P1D, 1},
			new object[] {Frequency.P1W, Frequency.P1D, 7},
			new object[] {Frequency.P2W, Frequency.P1D, 14},
			new object[] {Frequency.P1W, Frequency.P1W, 1},
			new object[] {Frequency.P2W, Frequency.P1W, 2},
			new object[] {Frequency.ofWeeks(3), Frequency.P1W, 3},
			new object[] {Frequency.P4W, Frequency.P1W, 4},
			new object[] {Frequency.P13W, Frequency.P1W, 13},
			new object[] {Frequency.P26W, Frequency.P1W, 26},
			new object[] {Frequency.P26W, Frequency.P2W, 13},
			new object[] {Frequency.P52W, Frequency.P1W, 52},
			new object[] {Frequency.P52W, Frequency.P2W, 26},
			new object[] {Frequency.P1M, Frequency.P1M, 1},
			new object[] {Frequency.P2M, Frequency.P1M, 2},
			new object[] {Frequency.P3M, Frequency.P1M, 3},
			new object[] {Frequency.P4M, Frequency.P1M, 4},
			new object[] {Frequency.P6M, Frequency.P1M, 6},
			new object[] {Frequency.P6M, Frequency.P2M, 3},
			new object[] {Frequency.P12M, Frequency.P1M, 12},
			new object[] {Frequency.P12M, Frequency.P2M, 6},
			new object[] {Frequency.ofYears(1), Frequency.P6M, 2},
			new object[] {Frequency.ofYears(1), Frequency.P3M, 4},
			new object[] {Frequency.ofYears(2), Frequency.P6M, 4}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "exactDivide") public void test_exactDivide(Frequency test, Frequency other, int expected)
	  public virtual void test_exactDivide(Frequency test, Frequency other, int expected)
	  {
		assertEquals(test.exactDivide(other), expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "exactDivide") public void test_exactDivide_reverse(Frequency test, Frequency other, int expected)
	  public virtual void test_exactDivide_reverse(Frequency test, Frequency other, int expected)
	  {
		if (!test.Equals(other))
		{
		  assertThrowsIllegalArg(() => other.exactDivide(test));
		}
	  }

	  public virtual void test_exactDivide_bad()
	  {
		assertThrowsIllegalArg(() => Frequency.ofDays(5).exactDivide(Frequency.ofDays(2)));
		assertThrowsIllegalArg(() => Frequency.ofMonths(5).exactDivide(Frequency.ofMonths(2)));
		assertThrowsIllegalArg(() => Frequency.P1M.exactDivide(Frequency.P1W));
		assertThrowsIllegalArg(() => Frequency.P1W.exactDivide(Frequency.P1M));
		assertThrowsIllegalArg(() => Frequency.TERM.exactDivide(Frequency.P1W));
		assertThrowsIllegalArg(() => Frequency.P12M.exactDivide(Frequency.TERM));
		assertThrowsIllegalArg(() => Frequency.ofYears(1).exactDivide(Frequency.P1W));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parse_String_roundTrip()
	  {
		assertEquals(Frequency.parse(P6M.ToString()), P6M);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "parseGood") public static Object[][] data_parseGood()
	  public static object[][] data_parseGood()
	  {
		return new object[][]
		{
			new object[] {"1D", Frequency.ofDays(1)},
			new object[] {"2D", Frequency.ofDays(2)},
			new object[] {"91D", Frequency.ofDays(91)},
			new object[] {"2W", Frequency.ofWeeks(2)},
			new object[] {"6W", Frequency.ofWeeks(6)},
			new object[] {"2M", Frequency.ofMonths(2)},
			new object[] {"12M", Frequency.ofMonths(12)},
			new object[] {"1Y", Frequency.ofYears(1)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseGood") public void test_parse_String_good_noP(String input, Frequency expected)
	  public virtual void test_parse_String_good_noP(string input, Frequency expected)
	  {
		assertEquals(Frequency.parse(input), expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseGood") public void test_parse_String_good_withP(String input, Frequency expected)
	  public virtual void test_parse_String_good_withP(string input, Frequency expected)
	  {
		assertEquals(Frequency.parse("P" + input), expected);
	  }

	  public virtual void test_parse_String_term()
	  {
		assertEquals(Frequency.parse("Term"), Frequency.TERM);
		assertEquals(Frequency.parse("TERM"), Frequency.TERM);
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
			new object[] {"-2D"},
			new object[] {"PTerm"},
			new object[] {null}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "parseBad", expectedExceptions = IllegalArgumentException.class) public void test_parse_String_bad(String input)
	  public virtual void test_parse_String_bad(string input)
	  {
		Frequency.parse(input);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_addTo()
	  {
		assertEquals(P1D.addTo(LocalDate.of(2014, 6, 30)), LocalDate.of(2014, 7, 1));
		assertEquals(P1W.addTo(OffsetDateTime.of(2014, 6, 30, 0, 0, 0, 0, ZoneOffset.UTC)), OffsetDateTime.of(2014, 7, 7, 0, 0, 0, 0, ZoneOffset.UTC));
	  }

	  public virtual void test_subtractFrom()
	  {
		assertEquals(P1D.subtractFrom(LocalDate.of(2014, 6, 30)), LocalDate.of(2014, 6, 29));
		assertEquals(P1W.subtractFrom(OffsetDateTime.of(2014, 6, 30, 0, 0, 0, 0, ZoneOffset.UTC)), OffsetDateTime.of(2014, 6, 23, 0, 0, 0, 0, ZoneOffset.UTC));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_temporalAmount()
	  {
		assertEquals(P3M.Units, ImmutableList.of(YEARS, MONTHS, DAYS));
		assertEquals(P3M.get(MONTHS), 3);
		assertEquals(LocalDate.of(2014, 6, 30).plus(P1W), LocalDate.of(2014, 7, 7));
		assertEquals(LocalDate.of(2014, 6, 30).minus(P1W), LocalDate.of(2014, 6, 23));
		assertThrows(() => P3M.get(CENTURIES), typeof(UnsupportedTemporalTypeException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals_hashCode()
	  {
		Frequency a1 = P1D;
		Frequency a2 = Frequency.ofDays(1);
		Frequency b = P3M;
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
		assertEquals(P3M.Equals(null), false);
		assertEquals(P3M.Equals(ANOTHER_TYPE), false);
		assertEquals(P3M.Equals(new object()), false);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(P1D);
		assertSerialization(P3M);
		assertSerialization(P12M);
		assertSerialization(TERM);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(Frequency), P1D);
		assertJodaConvert(typeof(Frequency), P3M);
		assertJodaConvert(typeof(Frequency), P12M);
		assertJodaConvert(typeof(Frequency), TERM);
	  }

	}

}