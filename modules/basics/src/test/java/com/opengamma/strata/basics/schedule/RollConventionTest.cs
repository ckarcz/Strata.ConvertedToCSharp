using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1D;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_2;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_30;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_THU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.EOM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.IMM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.IMMAUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.IMMCAD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.IMMNZD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.NONE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.SFE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.TBILL;
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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using CaseFormat = com.google.common.@base.CaseFormat;
	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Test <seealso cref="RollConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RollConventionTest
	public class RollConventionTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "types") public static Object[][] data_types()
		public static object[][] data_types()
		{
		RollConvention[] conv = StandardRollConventions.values();
		object[][] result = new object[conv.Length][];
		for (int i = 0; i < conv.Length; i++)
		{
		  result[i] = new object[] {conv[i]};
		}
		return result;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "types") public void test_null(RollConvention type)
	  public virtual void test_null(RollConvention type)
	  {
		assertThrowsIllegalArg(() => type.adjust(null));
		assertThrowsIllegalArg(() => type.matches(null));
		assertThrowsIllegalArg(() => type.next(date(2014, JULY, 1), null));
		assertThrowsIllegalArg(() => type.next(null, P3M));
		assertThrowsIllegalArg(() => type.previous(date(2014, JULY, 1), null));
		assertThrowsIllegalArg(() => type.previous(null, P3M));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_noAdjust()
	  {
		LocalDate date = date(2014, AUGUST, 17);
		assertEquals(NONE.adjust(date), date);
		assertEquals(NONE.matches(date), true);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "adjust") public static Object[][] data_adjust()
	  public static object[][] data_adjust()
	  {
		return new object[][]
		{
			new object[] {EOM, date(2014, AUGUST, 1), date(2014, AUGUST, 31)},
			new object[] {EOM, date(2014, AUGUST, 30), date(2014, AUGUST, 31)},
			new object[] {EOM, date(2014, SEPTEMBER, 1), date(2014, SEPTEMBER, 30)},
			new object[] {EOM, date(2014, SEPTEMBER, 30), date(2014, SEPTEMBER, 30)},
			new object[] {EOM, date(2014, FEBRUARY, 1), date(2014, FEBRUARY, 28)},
			new object[] {IMM, date(2014, AUGUST, 1), date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, AUGUST, 6), date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, AUGUST, 19), date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, AUGUST, 20), date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, AUGUST, 21), date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, AUGUST, 31), date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, SEPTEMBER, 1), date(2014, SEPTEMBER, 17)},
			new object[] {IMMCAD, date(2014, AUGUST, 1), date(2014, AUGUST, 18)},
			new object[] {IMMCAD, date(2014, AUGUST, 6), date(2014, AUGUST, 18)},
			new object[] {IMMCAD, date(2014, AUGUST, 7), date(2014, AUGUST, 18)},
			new object[] {IMMCAD, date(2014, AUGUST, 8), date(2014, AUGUST, 18)},
			new object[] {IMMCAD, date(2014, AUGUST, 31), date(2014, AUGUST, 18)},
			new object[] {IMMCAD, date(2014, SEPTEMBER, 1), date(2014, SEPTEMBER, 15)},
			new object[] {IMMAUD, date(2014, AUGUST, 1), date(2014, AUGUST, 7)},
			new object[] {IMMAUD, date(2014, AUGUST, 6), date(2014, AUGUST, 7)},
			new object[] {IMMAUD, date(2014, AUGUST, 7), date(2014, AUGUST, 7)},
			new object[] {IMMAUD, date(2014, AUGUST, 8), date(2014, AUGUST, 7)},
			new object[] {IMMAUD, date(2014, AUGUST, 31), date(2014, AUGUST, 7)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 1), date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, OCTOBER, 1), date(2014, OCTOBER, 9)},
			new object[] {IMMAUD, date(2014, NOVEMBER, 1), date(2014, NOVEMBER, 13)},
			new object[] {IMMNZD, date(2014, AUGUST, 1), date(2014, AUGUST, 13)},
			new object[] {IMMNZD, date(2014, AUGUST, 6), date(2014, AUGUST, 13)},
			new object[] {IMMNZD, date(2014, AUGUST, 12), date(2014, AUGUST, 13)},
			new object[] {IMMNZD, date(2014, AUGUST, 13), date(2014, AUGUST, 13)},
			new object[] {IMMNZD, date(2014, AUGUST, 14), date(2014, AUGUST, 13)},
			new object[] {IMMNZD, date(2014, AUGUST, 31), date(2014, AUGUST, 13)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 1), date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, OCTOBER, 1), date(2014, OCTOBER, 15)},
			new object[] {IMMNZD, date(2014, NOVEMBER, 1), date(2014, NOVEMBER, 12)},
			new object[] {SFE, date(2014, AUGUST, 1), date(2014, AUGUST, 8)},
			new object[] {SFE, date(2014, AUGUST, 6), date(2014, AUGUST, 8)},
			new object[] {SFE, date(2014, AUGUST, 7), date(2014, AUGUST, 8)},
			new object[] {SFE, date(2014, AUGUST, 8), date(2014, AUGUST, 8)},
			new object[] {SFE, date(2014, AUGUST, 31), date(2014, AUGUST, 8)},
			new object[] {SFE, date(2014, SEPTEMBER, 1), date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, OCTOBER, 1), date(2014, OCTOBER, 10)},
			new object[] {SFE, date(2014, NOVEMBER, 1), date(2014, NOVEMBER, 14)},
			new object[] {TBILL, date(2014, AUGUST, 1), date(2014, AUGUST, 4)},
			new object[] {TBILL, date(2014, AUGUST, 2), date(2014, AUGUST, 4)},
			new object[] {TBILL, date(2014, AUGUST, 3), date(2014, AUGUST, 4)},
			new object[] {TBILL, date(2014, AUGUST, 4), date(2014, AUGUST, 4)},
			new object[] {TBILL, date(2014, AUGUST, 5), date(2014, AUGUST, 11)},
			new object[] {TBILL, date(2014, AUGUST, 7), date(2014, AUGUST, 11)},
			new object[] {TBILL, date(2018, AUGUST, 31), date(2018, SEPTEMBER, 4)},
			new object[] {TBILL, date(2018, SEPTEMBER, 1), date(2018, SEPTEMBER, 4)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "adjust") public void test_adjust(RollConvention conv, java.time.LocalDate input, java.time.LocalDate expected)
	  public virtual void test_adjust(RollConvention conv, LocalDate input, LocalDate expected)
	  {
		assertEquals(conv.adjust(input), expected);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "matches") public static Object[][] data_matches()
	  public static object[][] data_matches()
	  {
		return new object[][]
		{
			new object[] {EOM, date(2014, AUGUST, 1), false},
			new object[] {EOM, date(2014, AUGUST, 30), false},
			new object[] {EOM, date(2014, AUGUST, 31), true},
			new object[] {EOM, date(2014, SEPTEMBER, 1), false},
			new object[] {EOM, date(2014, SEPTEMBER, 30), true},
			new object[] {IMM, date(2014, SEPTEMBER, 16), false},
			new object[] {IMM, date(2014, SEPTEMBER, 17), true},
			new object[] {IMM, date(2014, SEPTEMBER, 18), false},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 10), false},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 11), true},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 12), false},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 9), false},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 10), true},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 11), false},
			new object[] {SFE, date(2014, SEPTEMBER, 11), false},
			new object[] {SFE, date(2014, SEPTEMBER, 12), true},
			new object[] {SFE, date(2014, SEPTEMBER, 13), false}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "matches") public void test_matches(RollConvention conv, java.time.LocalDate input, boolean expected)
	  public virtual void test_matches(RollConvention conv, LocalDate input, bool expected)
	  {
		assertEquals(conv.matches(input), expected);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "next") public static Object[][] data_next()
	  public static object[][] data_next()
	  {
		return new object[][]
		{
			new object[] {EOM, date(2014, AUGUST, 1), P1M, date(2014, SEPTEMBER, 30)},
			new object[] {EOM, date(2014, AUGUST, 30), P1M, date(2014, SEPTEMBER, 30)},
			new object[] {EOM, date(2014, AUGUST, 31), P1M, date(2014, SEPTEMBER, 30)},
			new object[] {EOM, date(2014, SEPTEMBER, 1), P1M, date(2014, OCTOBER, 31)},
			new object[] {EOM, date(2014, SEPTEMBER, 30), P1M, date(2014, OCTOBER, 31)},
			new object[] {EOM, date(2014, JANUARY, 1), P1M, date(2014, FEBRUARY, 28)},
			new object[] {EOM, date(2014, FEBRUARY, 1), P1M, date(2014, MARCH, 31)},
			new object[] {EOM, date(2014, AUGUST, 1), P3M, date(2014, NOVEMBER, 30)},
			new object[] {EOM, date(2014, AUGUST, 1), P1D, date(2014, AUGUST, 31)},
			new object[] {EOM, date(2014, AUGUST, 30), P1D, date(2014, AUGUST, 31)},
			new object[] {EOM, date(2014, AUGUST, 31), P1D, date(2014, SEPTEMBER, 30)},
			new object[] {EOM, date(2014, JANUARY, 1), P1D, date(2014, JANUARY, 31)},
			new object[] {EOM, date(2014, JANUARY, 31), P1D, date(2014, FEBRUARY, 28)},
			new object[] {EOM, date(2014, FEBRUARY, 1), P1D, date(2014, FEBRUARY, 28)},
			new object[] {IMM, date(2014, AUGUST, 1), P1M, date(2014, SEPTEMBER, 17)},
			new object[] {IMM, date(2014, AUGUST, 31), P1M, date(2014, SEPTEMBER, 17)},
			new object[] {IMM, date(2014, SEPTEMBER, 1), P1M, date(2014, OCTOBER, 15)},
			new object[] {IMM, date(2014, SEPTEMBER, 30), P1M, date(2014, OCTOBER, 15)},
			new object[] {IMM, date(2014, AUGUST, 1), P1D, date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, AUGUST, 19), P1D, date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, AUGUST, 20), P1D, date(2014, SEPTEMBER, 17)},
			new object[] {IMM, date(2014, AUGUST, 31), P1D, date(2014, SEPTEMBER, 17)},
			new object[] {IMM, date(2014, SEPTEMBER, 1), P1D, date(2014, SEPTEMBER, 17)},
			new object[] {IMM, date(2014, SEPTEMBER, 16), P1D, date(2014, SEPTEMBER, 17)},
			new object[] {IMM, date(2014, SEPTEMBER, 17), P1D, date(2014, OCTOBER, 15)},
			new object[] {IMM, date(2014, SEPTEMBER, 30), P1D, date(2014, OCTOBER, 15)},
			new object[] {IMMAUD, date(2014, AUGUST, 1), P1M, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, AUGUST, 31), P1M, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 1), P1M, date(2014, OCTOBER, 9)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 30), P1M, date(2014, OCTOBER, 9)},
			new object[] {IMMAUD, date(2014, AUGUST, 1), P1D, date(2014, AUGUST, 7)},
			new object[] {IMMAUD, date(2014, AUGUST, 6), P1D, date(2014, AUGUST, 7)},
			new object[] {IMMAUD, date(2014, AUGUST, 7), P1D, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, AUGUST, 31), P1D, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 1), P1D, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 10), P1D, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 11), P1D, date(2014, OCTOBER, 9)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 30), P1D, date(2014, OCTOBER, 9)},
			new object[] {IMMNZD, date(2014, AUGUST, 1), P1M, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, AUGUST, 31), P1M, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 1), P1M, date(2014, OCTOBER, 15)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 30), P1M, date(2014, OCTOBER, 15)},
			new object[] {IMMNZD, date(2014, AUGUST, 1), P1D, date(2014, AUGUST, 13)},
			new object[] {IMMNZD, date(2014, AUGUST, 12), P1D, date(2014, AUGUST, 13)},
			new object[] {IMMNZD, date(2014, AUGUST, 13), P1D, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, AUGUST, 31), P1D, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 1), P1D, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 9), P1D, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 10), P1D, date(2014, OCTOBER, 15)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 30), P1D, date(2014, OCTOBER, 15)},
			new object[] {SFE, date(2014, AUGUST, 1), P1M, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, AUGUST, 31), P1M, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, SEPTEMBER, 1), P1M, date(2014, OCTOBER, 10)},
			new object[] {SFE, date(2014, SEPTEMBER, 30), P1M, date(2014, OCTOBER, 10)},
			new object[] {SFE, date(2014, AUGUST, 1), P1D, date(2014, AUGUST, 8)},
			new object[] {SFE, date(2014, AUGUST, 7), P1D, date(2014, AUGUST, 8)},
			new object[] {SFE, date(2014, AUGUST, 8), P1D, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, AUGUST, 31), P1D, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, SEPTEMBER, 1), P1D, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, SEPTEMBER, 11), P1D, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, SEPTEMBER, 12), P1D, date(2014, OCTOBER, 10)},
			new object[] {SFE, date(2014, SEPTEMBER, 30), P1D, date(2014, OCTOBER, 10)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "next") public void test_next(RollConvention conv, java.time.LocalDate input, Frequency freq, java.time.LocalDate expected)
	  public virtual void test_next(RollConvention conv, LocalDate input, Frequency freq, LocalDate expected)
	  {
		assertEquals(conv.next(input, freq), expected);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "previous") public static Object[][] data_previous()
	  public static object[][] data_previous()
	  {
		return new object[][]
		{
			new object[] {EOM, date(2014, OCTOBER, 1), P1M, date(2014, SEPTEMBER, 30)},
			new object[] {EOM, date(2014, OCTOBER, 31), P1M, date(2014, SEPTEMBER, 30)},
			new object[] {EOM, date(2014, NOVEMBER, 1), P1M, date(2014, OCTOBER, 31)},
			new object[] {EOM, date(2014, NOVEMBER, 30), P1M, date(2014, OCTOBER, 31)},
			new object[] {EOM, date(2014, MARCH, 1), P1M, date(2014, FEBRUARY, 28)},
			new object[] {EOM, date(2014, APRIL, 1), P1M, date(2014, MARCH, 31)},
			new object[] {EOM, date(2014, NOVEMBER, 1), P3M, date(2014, AUGUST, 31)},
			new object[] {EOM, date(2014, OCTOBER, 1), P1D, date(2014, SEPTEMBER, 30)},
			new object[] {EOM, date(2014, OCTOBER, 30), P1D, date(2014, SEPTEMBER, 30)},
			new object[] {IMM, date(2014, OCTOBER, 1), P1M, date(2014, SEPTEMBER, 17)},
			new object[] {IMM, date(2014, OCTOBER, 31), P1M, date(2014, SEPTEMBER, 17)},
			new object[] {IMM, date(2014, NOVEMBER, 1), P1M, date(2014, OCTOBER, 15)},
			new object[] {IMM, date(2014, NOVEMBER, 30), P1M, date(2014, OCTOBER, 15)},
			new object[] {IMM, date(2014, AUGUST, 1), P1D, date(2014, JULY, 16)},
			new object[] {IMM, date(2014, AUGUST, 20), P1D, date(2014, JULY, 16)},
			new object[] {IMM, date(2014, AUGUST, 21), P1D, date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, AUGUST, 31), P1D, date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, SEPTEMBER, 1), P1D, date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, SEPTEMBER, 17), P1D, date(2014, AUGUST, 20)},
			new object[] {IMM, date(2014, SEPTEMBER, 18), P1D, date(2014, SEPTEMBER, 17)},
			new object[] {IMM, date(2014, SEPTEMBER, 30), P1D, date(2014, SEPTEMBER, 17)},
			new object[] {IMMAUD, date(2014, OCTOBER, 1), P1M, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, OCTOBER, 31), P1M, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, NOVEMBER, 1), P1M, date(2014, OCTOBER, 9)},
			new object[] {IMMAUD, date(2014, NOVEMBER, 30), P1M, date(2014, OCTOBER, 9)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 1), P1D, date(2014, AUGUST, 7)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 11), P1D, date(2014, AUGUST, 7)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 12), P1D, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, SEPTEMBER, 30), P1D, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, OCTOBER, 1), P1D, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, OCTOBER, 9), P1D, date(2014, SEPTEMBER, 11)},
			new object[] {IMMAUD, date(2014, OCTOBER, 10), P1D, date(2014, OCTOBER, 9)},
			new object[] {IMMAUD, date(2014, OCTOBER, 30), P1D, date(2014, OCTOBER, 9)},
			new object[] {IMMNZD, date(2014, OCTOBER, 1), P1M, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, OCTOBER, 31), P1M, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, NOVEMBER, 1), P1M, date(2014, OCTOBER, 15)},
			new object[] {IMMNZD, date(2014, NOVEMBER, 30), P1M, date(2014, OCTOBER, 15)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 1), P1D, date(2014, AUGUST, 13)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 10), P1D, date(2014, AUGUST, 13)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 11), P1D, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, SEPTEMBER, 30), P1D, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, OCTOBER, 1), P1D, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, OCTOBER, 15), P1D, date(2014, SEPTEMBER, 10)},
			new object[] {IMMNZD, date(2014, OCTOBER, 16), P1D, date(2014, OCTOBER, 15)},
			new object[] {IMMNZD, date(2014, OCTOBER, 30), P1D, date(2014, OCTOBER, 15)},
			new object[] {SFE, date(2014, OCTOBER, 1), P1M, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, OCTOBER, 31), P1M, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, NOVEMBER, 1), P1M, date(2014, OCTOBER, 10)},
			new object[] {SFE, date(2014, NOVEMBER, 30), P1M, date(2014, OCTOBER, 10)},
			new object[] {SFE, date(2014, SEPTEMBER, 1), P1D, date(2014, AUGUST, 8)},
			new object[] {SFE, date(2014, SEPTEMBER, 12), P1D, date(2014, AUGUST, 8)},
			new object[] {SFE, date(2014, SEPTEMBER, 13), P1D, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, SEPTEMBER, 30), P1D, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, OCTOBER, 1), P1D, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, OCTOBER, 10), P1D, date(2014, SEPTEMBER, 12)},
			new object[] {SFE, date(2014, OCTOBER, 11), P1D, date(2014, OCTOBER, 10)},
			new object[] {SFE, date(2014, OCTOBER, 30), P1D, date(2014, OCTOBER, 10)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "previous") public void test_previous(RollConvention conv, java.time.LocalDate input, Frequency freq, java.time.LocalDate expected)
	  public virtual void test_previous(RollConvention conv, LocalDate input, Frequency freq, LocalDate expected)
	  {
		assertEquals(conv.previous(input, freq), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_dayOfMonth_constants()
	  {
		assertEquals(RollConventions.DAY_1.adjust(date(2014, JULY, 30)), date(2014, JULY, 1));
		assertEquals(RollConventions.DAY_2.adjust(date(2014, JULY, 30)), date(2014, JULY, 2));
		assertEquals(RollConventions.DAY_3.adjust(date(2014, JULY, 30)), date(2014, JULY, 3));
		assertEquals(RollConventions.DAY_4.adjust(date(2014, JULY, 30)), date(2014, JULY, 4));
		assertEquals(RollConventions.DAY_5.adjust(date(2014, JULY, 30)), date(2014, JULY, 5));
		assertEquals(RollConventions.DAY_6.adjust(date(2014, JULY, 30)), date(2014, JULY, 6));
		assertEquals(RollConventions.DAY_7.adjust(date(2014, JULY, 30)), date(2014, JULY, 7));
		assertEquals(RollConventions.DAY_8.adjust(date(2014, JULY, 30)), date(2014, JULY, 8));
		assertEquals(RollConventions.DAY_9.adjust(date(2014, JULY, 30)), date(2014, JULY, 9));
		assertEquals(RollConventions.DAY_10.adjust(date(2014, JULY, 30)), date(2014, JULY, 10));
		assertEquals(RollConventions.DAY_11.adjust(date(2014, JULY, 30)), date(2014, JULY, 11));
		assertEquals(RollConventions.DAY_12.adjust(date(2014, JULY, 30)), date(2014, JULY, 12));
		assertEquals(RollConventions.DAY_13.adjust(date(2014, JULY, 30)), date(2014, JULY, 13));
		assertEquals(RollConventions.DAY_14.adjust(date(2014, JULY, 30)), date(2014, JULY, 14));
		assertEquals(RollConventions.DAY_15.adjust(date(2014, JULY, 30)), date(2014, JULY, 15));
		assertEquals(RollConventions.DAY_16.adjust(date(2014, JULY, 30)), date(2014, JULY, 16));
		assertEquals(RollConventions.DAY_17.adjust(date(2014, JULY, 30)), date(2014, JULY, 17));
		assertEquals(RollConventions.DAY_18.adjust(date(2014, JULY, 30)), date(2014, JULY, 18));
		assertEquals(RollConventions.DAY_19.adjust(date(2014, JULY, 30)), date(2014, JULY, 19));
		assertEquals(RollConventions.DAY_20.adjust(date(2014, JULY, 30)), date(2014, JULY, 20));
		assertEquals(RollConventions.DAY_21.adjust(date(2014, JULY, 30)), date(2014, JULY, 21));
		assertEquals(RollConventions.DAY_22.adjust(date(2014, JULY, 30)), date(2014, JULY, 22));
		assertEquals(RollConventions.DAY_23.adjust(date(2014, JULY, 30)), date(2014, JULY, 23));
		assertEquals(RollConventions.DAY_24.adjust(date(2014, JULY, 30)), date(2014, JULY, 24));
		assertEquals(RollConventions.DAY_25.adjust(date(2014, JULY, 30)), date(2014, JULY, 25));
		assertEquals(RollConventions.DAY_26.adjust(date(2014, JULY, 30)), date(2014, JULY, 26));
		assertEquals(RollConventions.DAY_27.adjust(date(2014, JULY, 30)), date(2014, JULY, 27));
		assertEquals(RollConventions.DAY_28.adjust(date(2014, JULY, 30)), date(2014, JULY, 28));
		assertEquals(RollConventions.DAY_29.adjust(date(2014, JULY, 30)), date(2014, JULY, 29));
		assertEquals(RollConventions.DAY_30.adjust(date(2014, JULY, 30)), date(2014, JULY, 30));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ofDayOfMonth()
	  {
		for (int i = 1; i < 30; i++)
		{
		  RollConvention test = RollConvention.ofDayOfMonth(i);
		  assertEquals(test.adjust(date(2014, JULY, 1)), date(2014, JULY, i));
		  assertEquals(test.Name, "Day" + i);
		  assertEquals(test.ToString(), "Day" + i);
		  assertSame(RollConvention.of(test.Name), test);
		  assertSame(RollConvention.of("DAY" + i), test);
		}
	  }

	  public virtual void test_ofDayOfMonth_31()
	  {
		assertEquals(RollConvention.ofDayOfMonth(31), EOM);
	  }

	  public virtual void test_ofDayOfMonth_invalid()
	  {
		assertThrowsIllegalArg(() => RollConvention.ofDayOfMonth(0));
		assertThrowsIllegalArg(() => RollConvention.ofDayOfMonth(32));
	  }

	  public virtual void test_ofDayOfMonth_adjust_Day29()
	  {
		assertEquals(RollConvention.ofDayOfMonth(29).adjust(date(2014, FEBRUARY, 2)), date(2014, FEBRUARY, 28));
		assertEquals(RollConvention.ofDayOfMonth(29).adjust(date(2016, FEBRUARY, 2)), date(2016, FEBRUARY, 29));
	  }

	  public virtual void test_ofDayOfMonth_adjust_Day30()
	  {
		assertEquals(RollConvention.ofDayOfMonth(30).adjust(date(2014, FEBRUARY, 2)), date(2014, FEBRUARY, 28));
		assertEquals(RollConvention.ofDayOfMonth(30).adjust(date(2016, FEBRUARY, 2)), date(2016, FEBRUARY, 29));
	  }

	  public virtual void test_ofDayOfMonth_matches_Day29()
	  {
		assertEquals(RollConvention.ofDayOfMonth(29).matches(date(2016, JANUARY, 30)), false);
		assertEquals(RollConvention.ofDayOfMonth(29).matches(date(2016, JANUARY, 29)), true);
		assertEquals(RollConvention.ofDayOfMonth(29).matches(date(2016, JANUARY, 30)), false);

		assertEquals(RollConvention.ofDayOfMonth(29).matches(date(2016, FEBRUARY, 28)), false);
		assertEquals(RollConvention.ofDayOfMonth(29).matches(date(2016, FEBRUARY, 29)), true);

		assertEquals(RollConvention.ofDayOfMonth(29).matches(date(2015, FEBRUARY, 27)), false);
		assertEquals(RollConvention.ofDayOfMonth(29).matches(date(2015, FEBRUARY, 28)), true);
	  }

	  public virtual void test_ofDayOfMonth_matches_Day30()
	  {
		assertEquals(RollConvention.ofDayOfMonth(30).matches(date(2016, JANUARY, 29)), false);
		assertEquals(RollConvention.ofDayOfMonth(30).matches(date(2016, JANUARY, 30)), true);
		assertEquals(RollConvention.ofDayOfMonth(30).matches(date(2016, JANUARY, 31)), false);

		assertEquals(RollConvention.ofDayOfMonth(30).matches(date(2016, FEBRUARY, 28)), false);
		assertEquals(RollConvention.ofDayOfMonth(30).matches(date(2016, FEBRUARY, 29)), true);

		assertEquals(RollConvention.ofDayOfMonth(30).matches(date(2015, FEBRUARY, 27)), false);
		assertEquals(RollConvention.ofDayOfMonth(30).matches(date(2015, FEBRUARY, 28)), true);
	  }

	  public virtual void test_ofDayOfMonth_next_oneMonth()
	  {
		for (int start = 1; start <= 5; start++)
		{
		  for (int i = 1; i <= 30; i++)
		  {
			RollConvention test = RollConvention.ofDayOfMonth(i);
			LocalDate expected = date(2014, AUGUST, i);
			assertEquals(test.next(date(2014, JULY, start), P1M), expected);
		  }
		}
	  }

	  public virtual void test_ofDayOfMonth_next_oneDay()
	  {
		for (int start = 1; start <= 5; start++)
		{
		  for (int i = 1; i <= 30; i++)
		  {
			RollConvention test = RollConvention.ofDayOfMonth(i);
			LocalDate expected = date(2014, JULY, i);
			if (i <= start)
			{
			  expected = expected.plusMonths(1);
			}
			assertEquals(test.next(date(2014, JULY, start), P1D), expected);
		  }
		}
	  }

	  public virtual void test_ofDayOfMonth_previous_oneMonth()
	  {
		for (int start = 1; start <= 5; start++)
		{
		  for (int i = 1; i <= 30; i++)
		  {
			RollConvention test = RollConvention.ofDayOfMonth(i);
			LocalDate expected = date(2014, JUNE, i);
			assertEquals(test.previous(date(2014, JULY, start), P1M), expected);
		  }
		}
	  }

	  public virtual void test_ofDayOfMonth_previous_oneDay()
	  {
		for (int start = 1; start <= 5; start++)
		{
		  for (int i = 1; i <= 30; i++)
		  {
			RollConvention test = RollConvention.ofDayOfMonth(i);
			LocalDate expected = date(2014, JULY, i);
			if (i >= start)
			{
			  expected = expected.minusMonths(1);
			}
			assertEquals(test.previous(date(2014, JULY, start), P1D), expected);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_dayOfWeek_constants()
	  {
		assertEquals(RollConventions.DAY_MON.adjust(date(2014, AUGUST, 11)), date(2014, AUGUST, 11));
		assertEquals(RollConventions.DAY_TUE.adjust(date(2014, AUGUST, 11)), date(2014, AUGUST, 12));
		assertEquals(RollConventions.DAY_WED.adjust(date(2014, AUGUST, 11)), date(2014, AUGUST, 13));
		assertEquals(RollConventions.DAY_THU.adjust(date(2014, AUGUST, 11)), date(2014, AUGUST, 14));
		assertEquals(RollConventions.DAY_FRI.adjust(date(2014, AUGUST, 11)), date(2014, AUGUST, 15));
		assertEquals(RollConventions.DAY_SAT.adjust(date(2014, AUGUST, 11)), date(2014, AUGUST, 16));
		assertEquals(RollConventions.DAY_SUN.adjust(date(2014, AUGUST, 11)), date(2014, AUGUST, 17));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ofDayOfWeek()
	  {
		foreach (DayOfWeek dow in DayOfWeek.values())
		{
		  RollConvention test = RollConvention.ofDayOfWeek(dow);
		  assertEquals(test.Name, "Day" + CaseFormat.UPPER_UNDERSCORE.converterTo(CaseFormat.UPPER_CAMEL).convert(dow.ToString()).substring(0, 3));
		  assertEquals(test.ToString(), "Day" + CaseFormat.UPPER_UNDERSCORE.converterTo(CaseFormat.UPPER_CAMEL).convert(dow.ToString()).substring(0, 3));
		  assertSame(RollConvention.of(test.Name), test);
		  assertSame(RollConvention.of("DAY" + dow.ToString().Substring(0, 3)), test);
		}
	  }

	  public virtual void test_ofDayOfWeek_adjust()
	  {
		foreach (DayOfWeek dow in DayOfWeek.values())
		{
		  RollConvention test = RollConvention.ofDayOfWeek(dow);
		  assertEquals(test.adjust(date(2014, AUGUST, 14)), date(2014, AUGUST, 14).with(TemporalAdjusters.nextOrSame(dow)));
		}
	  }

	  public virtual void test_ofDayOfWeek_matches()
	  {
		assertEquals(RollConvention.ofDayOfWeek(TUESDAY).matches(date(2014, SEPTEMBER, 1)), false);
		assertEquals(RollConvention.ofDayOfWeek(TUESDAY).matches(date(2014, SEPTEMBER, 2)), true);
		assertEquals(RollConvention.ofDayOfWeek(TUESDAY).matches(date(2014, SEPTEMBER, 3)), false);
	  }

	  public virtual void test_ofDayOfWeek_next_oneMonth()
	  {
		foreach (DayOfWeek dow in DayOfWeek.values())
		{
		  RollConvention test = RollConvention.ofDayOfWeek(dow);
		  assertEquals(test.next(date(2014, AUGUST, 14), P1W), date(2014, AUGUST, 21).with(TemporalAdjusters.nextOrSame(dow)));
		}
	  }

	  public virtual void test_ofDayOfWeek_next_oneDay()
	  {
		foreach (DayOfWeek dow in DayOfWeek.values())
		{
		  RollConvention test = RollConvention.ofDayOfWeek(dow);
		  assertEquals(test.next(date(2014, AUGUST, 14), P1D), date(2014, AUGUST, 15).with(TemporalAdjusters.nextOrSame(dow)));
		}
	  }

	  public virtual void test_ofDayOfWeek_previous_oneMonth()
	  {
		foreach (DayOfWeek dow in DayOfWeek.values())
		{
		  RollConvention test = RollConvention.ofDayOfWeek(dow);
		  assertEquals(test.previous(date(2014, AUGUST, 14), P1W), date(2014, AUGUST, 7).with(TemporalAdjusters.previousOrSame(dow)));
		}
	  }

	  public virtual void test_ofDayOfWeek_previous_oneDay()
	  {
		foreach (DayOfWeek dow in DayOfWeek.values())
		{
		  RollConvention test = RollConvention.ofDayOfWeek(dow);
		  assertEquals(test.previous(date(2014, AUGUST, 14), P1D), date(2014, AUGUST, 13).with(TemporalAdjusters.previousOrSame(dow)));
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {NONE, "None"},
			new object[] {EOM, "EOM"},
			new object[] {IMM, "IMM"},
			new object[] {IMMAUD, "IMMAUD"},
			new object[] {IMMNZD, "IMMNZD"},
			new object[] {SFE, "SFE"},
			new object[] {DAY_2, "Day2"},
			new object[] {DAY_THU, "DayThu"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(RollConvention convention, String name)
	  public virtual void test_name(RollConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(RollConvention convention, String name)
	  public virtual void test_toString(RollConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(RollConvention convention, String name)
	  public virtual void test_of_lookup(RollConvention convention, string name)
	  {
		assertEquals(RollConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_lenientLookup_standardNames(RollConvention convention, String name)
	  public virtual void test_lenientLookup_standardNames(RollConvention convention, string name)
	  {
		assertEquals(RollConvention.extendedEnum().findLenient(name.ToLower(Locale.ENGLISH)).get(), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(RollConvention convention, String name)
	  public virtual void test_extendedEnum(RollConvention convention, string name)
	  {
		ImmutableMap<string, RollConvention> map = RollConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => RollConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => RollConvention.of(null));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "lenient") public static Object[][] data_lenient()
	  public static object[][] data_lenient()
	  {
		return new object[][]
		{
			new object[] {"2", DAY_2},
			new object[] {"30", DAY_30},
			new object[] {"31", EOM},
			new object[] {"THU", DAY_THU}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "lenient") public void test_lenientLookup_specialNames(String name, RollConvention convention)
	  public virtual void test_lenientLookup_specialNames(string name, RollConvention convention)
	  {
		assertEquals(RollConvention.extendedEnum().findLenient(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		RollConvention a = RollConventions.EOM;
		RollConvention b = RollConventions.DAY_1;
		RollConvention c = RollConventions.DAY_WED;

		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(c), false);

		assertEquals(b.Equals(a), false);
		assertEquals(b.Equals(b), true);
		assertEquals(b.Equals(c), false);

		assertEquals(c.Equals(a), false);
		assertEquals(c.Equals(b), false);
		assertEquals(c.Equals(c), true);

		assertEquals(a.GetHashCode(), a.GetHashCode());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(RollConventions));
		coverEnum(typeof(StandardRollConventions));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(EOM);
		assertSerialization(DAY_2);
		assertSerialization(DAY_THU);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(RollConvention), NONE);
		assertJodaConvert(typeof(RollConvention), EOM);
	  }

	}

}