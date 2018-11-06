using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test {@code GlobalHolidayCalendars}.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GlobalHolidayCalendarsTest
	public class GlobalHolidayCalendarsTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "easter") public static Object[][] data_easter()
		public static object[][] data_easter()
		{
		return new object[][]
		{
			new object[] {15, 4, 1900},
			new object[] {15, 4, 1900},
			new object[] {7, 4, 1901},
			new object[] {30, 3, 1902},
			new object[] {12, 4, 1903},
			new object[] {3, 4, 1904},
			new object[] {23, 4, 1905},
			new object[] {15, 4, 1906},
			new object[] {31, 3, 1907},
			new object[] {19, 4, 1908},
			new object[] {11, 4, 1909},
			new object[] {27, 3, 1910},
			new object[] {16, 4, 1911},
			new object[] {7, 4, 1912},
			new object[] {23, 3, 1913},
			new object[] {12, 4, 1914},
			new object[] {4, 4, 1915},
			new object[] {23, 4, 1916},
			new object[] {8, 4, 1917},
			new object[] {31, 3, 1918},
			new object[] {20, 4, 1919},
			new object[] {4, 4, 1920},
			new object[] {27, 3, 1921},
			new object[] {16, 4, 1922},
			new object[] {1, 4, 1923},
			new object[] {20, 4, 1924},
			new object[] {12, 4, 1925},
			new object[] {4, 4, 1926},
			new object[] {17, 4, 1927},
			new object[] {8, 4, 1928},
			new object[] {31, 3, 1929},
			new object[] {20, 4, 1930},
			new object[] {5, 4, 1931},
			new object[] {27, 3, 1932},
			new object[] {16, 4, 1933},
			new object[] {1, 4, 1934},
			new object[] {21, 4, 1935},
			new object[] {12, 4, 1936},
			new object[] {28, 3, 1937},
			new object[] {17, 4, 1938},
			new object[] {9, 4, 1939},
			new object[] {24, 3, 1940},
			new object[] {13, 4, 1941},
			new object[] {5, 4, 1942},
			new object[] {25, 4, 1943},
			new object[] {9, 4, 1944},
			new object[] {1, 4, 1945},
			new object[] {21, 4, 1946},
			new object[] {6, 4, 1947},
			new object[] {28, 3, 1948},
			new object[] {17, 4, 1949},
			new object[] {9, 4, 1950},
			new object[] {25, 3, 1951},
			new object[] {13, 4, 1952},
			new object[] {5, 4, 1953},
			new object[] {18, 4, 1954},
			new object[] {10, 4, 1955},
			new object[] {1, 4, 1956},
			new object[] {21, 4, 1957},
			new object[] {6, 4, 1958},
			new object[] {29, 3, 1959},
			new object[] {17, 4, 1960},
			new object[] {2, 4, 1961},
			new object[] {22, 4, 1962},
			new object[] {14, 4, 1963},
			new object[] {29, 3, 1964},
			new object[] {18, 4, 1965},
			new object[] {10, 4, 1966},
			new object[] {26, 3, 1967},
			new object[] {14, 4, 1968},
			new object[] {6, 4, 1969},
			new object[] {29, 3, 1970},
			new object[] {11, 4, 1971},
			new object[] {2, 4, 1972},
			new object[] {22, 4, 1973},
			new object[] {14, 4, 1974},
			new object[] {30, 3, 1975},
			new object[] {18, 4, 1976},
			new object[] {10, 4, 1977},
			new object[] {26, 3, 1978},
			new object[] {15, 4, 1979},
			new object[] {6, 4, 1980},
			new object[] {19, 4, 1981},
			new object[] {11, 4, 1982},
			new object[] {3, 4, 1983},
			new object[] {22, 4, 1984},
			new object[] {7, 4, 1985},
			new object[] {30, 3, 1986},
			new object[] {19, 4, 1987},
			new object[] {3, 4, 1988},
			new object[] {26, 3, 1989},
			new object[] {15, 4, 1990},
			new object[] {31, 3, 1991},
			new object[] {19, 4, 1992},
			new object[] {11, 4, 1993},
			new object[] {3, 4, 1994},
			new object[] {16, 4, 1995},
			new object[] {7, 4, 1996},
			new object[] {30, 3, 1997},
			new object[] {12, 4, 1998},
			new object[] {4, 4, 1999},
			new object[] {23, 4, 2000},
			new object[] {15, 4, 2001},
			new object[] {31, 3, 2002},
			new object[] {20, 4, 2003},
			new object[] {11, 4, 2004},
			new object[] {27, 3, 2005},
			new object[] {16, 4, 2006},
			new object[] {8, 4, 2007},
			new object[] {23, 3, 2008},
			new object[] {12, 4, 2009},
			new object[] {4, 4, 2010},
			new object[] {24, 4, 2011},
			new object[] {8, 4, 2012},
			new object[] {31, 3, 2013},
			new object[] {20, 4, 2014},
			new object[] {5, 4, 2015},
			new object[] {27, 3, 2016},
			new object[] {16, 4, 2017},
			new object[] {1, 4, 2018},
			new object[] {21, 4, 2019},
			new object[] {12, 4, 2020},
			new object[] {4, 4, 2021},
			new object[] {17, 4, 2022},
			new object[] {9, 4, 2023},
			new object[] {31, 3, 2024},
			new object[] {20, 4, 2025},
			new object[] {5, 4, 2026},
			new object[] {28, 3, 2027},
			new object[] {16, 4, 2028},
			new object[] {1, 4, 2029},
			new object[] {21, 4, 2030},
			new object[] {13, 4, 2031},
			new object[] {28, 3, 2032},
			new object[] {17, 4, 2033},
			new object[] {9, 4, 2034},
			new object[] {25, 3, 2035},
			new object[] {13, 4, 2036},
			new object[] {5, 4, 2037},
			new object[] {25, 4, 2038},
			new object[] {10, 4, 2039},
			new object[] {1, 4, 2040},
			new object[] {21, 4, 2041},
			new object[] {6, 4, 2042},
			new object[] {29, 3, 2043},
			new object[] {17, 4, 2044},
			new object[] {9, 4, 2045},
			new object[] {25, 3, 2046},
			new object[] {14, 4, 2047},
			new object[] {5, 4, 2048},
			new object[] {18, 4, 2049},
			new object[] {10, 4, 2050},
			new object[] {2, 4, 2051},
			new object[] {21, 4, 2052},
			new object[] {6, 4, 2053},
			new object[] {29, 3, 2054},
			new object[] {18, 4, 2055},
			new object[] {2, 4, 2056},
			new object[] {22, 4, 2057},
			new object[] {14, 4, 2058},
			new object[] {30, 3, 2059},
			new object[] {18, 4, 2060},
			new object[] {10, 4, 2061},
			new object[] {26, 3, 2062},
			new object[] {15, 4, 2063},
			new object[] {6, 4, 2064},
			new object[] {29, 3, 2065},
			new object[] {11, 4, 2066},
			new object[] {3, 4, 2067},
			new object[] {22, 4, 2068},
			new object[] {14, 4, 2069},
			new object[] {30, 3, 2070},
			new object[] {19, 4, 2071},
			new object[] {10, 4, 2072},
			new object[] {26, 3, 2073},
			new object[] {15, 4, 2074},
			new object[] {7, 4, 2075},
			new object[] {19, 4, 2076},
			new object[] {11, 4, 2077},
			new object[] {3, 4, 2078},
			new object[] {23, 4, 2079},
			new object[] {7, 4, 2080},
			new object[] {30, 3, 2081},
			new object[] {19, 4, 2082},
			new object[] {4, 4, 2083},
			new object[] {26, 3, 2084},
			new object[] {15, 4, 2085},
			new object[] {31, 3, 2086},
			new object[] {20, 4, 2087},
			new object[] {11, 4, 2088},
			new object[] {3, 4, 2089},
			new object[] {16, 4, 2090},
			new object[] {8, 4, 2091},
			new object[] {30, 3, 2092},
			new object[] {12, 4, 2093},
			new object[] {4, 4, 2094},
			new object[] {24, 4, 2095},
			new object[] {15, 4, 2096},
			new object[] {31, 3, 2097},
			new object[] {20, 4, 2098},
			new object[] {12, 4, 2099}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "easter") public void test_easter(int day, int month, int year)
	  public virtual void test_easter(int day, int month, int year)
	  {
		assertEquals(GlobalHolidayCalendars.easter(year), LocalDate.of(year, month, day));
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar GBLO = GlobalHolidayCalendars.generateLondon();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "gblo") public static Object[][] data_gblo()
	  public static object[][] data_gblo()
	  {
		return new object[][]
		{
			new object[] {1965, mds(1965, md(4, 16), md(4, 19), md(6, 7), md(8, 30), md(12, 27), md(12, 28))},
			new object[] {1966, mds(1966, md(4, 8), md(4, 11), md(5, 30), md(8, 29), md(12, 26), md(12, 27))},
			new object[] {1967, mds(1967, md(3, 24), md(3, 27), md(5, 29), md(8, 28), md(12, 25), md(12, 26))},
			new object[] {1968, mds(1968, md(4, 12), md(4, 15), md(6, 3), md(9, 2), md(12, 25), md(12, 26))},
			new object[] {1969, mds(1969, md(4, 4), md(4, 7), md(5, 26), md(9, 1), md(12, 25), md(12, 26))},
			new object[] {1970, mds(1970, md(3, 27), md(3, 30), md(5, 25), md(8, 31), md(12, 25), md(12, 28))},
			new object[] {1971, mds(1971, md(4, 9), md(4, 12), md(5, 31), md(8, 30), md(12, 27), md(12, 28))},
			new object[] {2009, mds(2009, md(1, 1), md(4, 10), md(4, 13), md(5, 4), md(5, 25), md(8, 31), md(12, 25), md(12, 28))},
			new object[] {2010, mds(2010, md(1, 1), md(4, 2), md(4, 5), md(5, 3), md(5, 31), md(8, 30), md(12, 27), md(12, 28))},
			new object[] {2012, mds(2012, md(1, 2), md(4, 6), md(4, 9), md(5, 7), md(6, 4), md(6, 5), md(8, 27), md(12, 25), md(12, 26))},
			new object[] {2013, mds(2013, md(1, 1), md(3, 29), md(4, 1), md(5, 6), md(5, 27), md(8, 26), md(12, 25), md(12, 26))},
			new object[] {2014, mds(2014, md(1, 1), md(4, 18), md(4, 21), md(5, 5), md(5, 26), md(8, 25), md(12, 25), md(12, 26))},
			new object[] {2015, mds(2015, md(1, 1), md(4, 3), md(4, 6), md(5, 4), md(5, 25), md(8, 31), md(12, 25), md(12, 28))},
			new object[] {2016, mds(2016, md(1, 1), md(3, 25), md(3, 28), md(5, 2), md(5, 30), md(8, 29), md(12, 26), md(12, 27))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "gblo") public void test_gblo(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_gblo(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(GBLO.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar FRPA = GlobalHolidayCalendars.generateParis();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "frpa") public static Object[][] data_frpa()
	  public static object[][] data_frpa()
	  {
		return new object[][]
		{
			new object[] {2003, mds(2003, md(1, 1), md(4, 18), md(4, 21), md(5, 1), md(5, 2), md(5, 8), md(5, 9), md(5, 29), md(5, 30), md(6, 9), md(7, 14), md(8, 15), md(11, 1), md(11, 10), md(11, 11), md(12, 25), md(12, 26))},
			new object[] {2004, mds(2004, md(1, 1), md(1, 2), md(4, 9), md(4, 12), md(5, 1), md(5, 8), md(5, 20), md(5, 21), md(5, 31), md(7, 14), md(8, 15), md(11, 1), md(11, 11), md(11, 12), md(12, 25), md(12, 26))},
			new object[] {2005, mds(2005, md(1, 1), md(3, 25), md(3, 28), md(5, 1), md(5, 5), md(5, 6), md(5, 8), md(7, 14), md(7, 15), md(8, 15), md(10, 31), md(11, 1), md(11, 11), md(12, 25), md(12, 26))},
			new object[] {2006, mds(2006, md(1, 1), md(4, 14), md(4, 17), md(5, 1), md(5, 8), md(5, 25), md(5, 26), md(7, 14), md(8, 14), md(8, 15), md(11, 1), md(11, 11), md(12, 25), md(12, 26))},
			new object[] {2007, mds(2007, md(1, 1), md(4, 6), md(4, 9), md(4, 30), md(5, 1), md(5, 7), md(5, 8), md(5, 17), md(5, 18), md(7, 14), md(8, 15), md(11, 1), md(11, 2), md(11, 11), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2008, mds(2008, md(1, 1), md(3, 21), md(3, 24), md(5, 1), md(5, 2), md(5, 8), md(5, 9), md(5, 12), md(5, 24), md(7, 14), md(8, 15), md(11, 1), md(11, 10), md(11, 11), md(12, 25), md(12, 26))},
			new object[] {2012, mds(2012, md(1, 1), md(4, 6), md(4, 9), md(4, 30), md(5, 1), md(5, 7), md(5, 8), md(5, 17), md(5, 18), md(5, 28), md(7, 14), md(8, 15), md(11, 1), md(11, 2), md(11, 10), md(11, 11), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2013, mds(2013, md(1, 1), md(3, 29), md(4, 1), md(5, 1), md(5, 8), md(5, 9), md(5, 10), md(5, 20), md(7, 14), md(8, 15), md(8, 16), md(11, 1), md(11, 11), md(12, 25), md(12, 26))},
			new object[] {2014, mds(2014, md(1, 1), md(4, 18), md(4, 21), md(5, 1), md(5, 2), md(5, 8), md(5, 9), md(5, 29), md(5, 30), md(6, 9), md(7, 14), md(8, 15), md(11, 1), md(11, 10), md(11, 11), md(12, 25), md(12, 26))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 2), md(4, 3), md(4, 6), md(5, 1), md(5, 8), md(5, 14), md(5, 15), md(5, 25), md(7, 13), md(7, 14), md(8, 15), md(11, 1), md(11, 11), md(12, 25), md(12, 26))},
			new object[] {2016, mds(2016, md(1, 1), md(3, 25), md(3, 28), md(5, 1), md(5, 5), md(5, 6), md(5, 8), md(5, 16), md(7, 14), md(7, 15), md(8, 15), md(10, 31), md(11, 1), md(11, 11), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "frpa") public void test_frpa(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_frpa(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(FRPA.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar DEFR = GlobalHolidayCalendars.generateFrankfurt();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "defr") public static Object[][] data_defr()
	  public static object[][] data_defr()
	  {
		return new object[][]
		{
			new object[] {2014, mds(2014, md(1, 1), md(4, 18), md(4, 21), md(5, 1), md(5, 29), md(6, 9), md(6, 19), md(10, 3), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2015, mds(2015, md(1, 1), md(4, 3), md(4, 6), md(5, 1), md(5, 14), md(5, 25), md(6, 4), md(10, 3), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2016, mds(2016, md(1, 1), md(3, 25), md(3, 28), md(5, 1), md(5, 5), md(5, 16), md(5, 26), md(10, 3), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2017, mds(2017, md(1, 1), md(4, 14), md(4, 17), md(5, 1), md(5, 25), md(6, 5), md(6, 15), md(10, 3), md(10, 31), md(12, 25), md(12, 26), md(12, 31))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "defr") public void test_defr(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_defr(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(DEFR.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar CHZU = GlobalHolidayCalendars.generateZurich();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "chzu") public static Object[][] data_chzu()
	  public static object[][] data_chzu()
	  {
		return new object[][]
		{
			new object[] {2012, mds(2012, md(1, 1), md(1, 2), md(4, 6), md(4, 9), md(5, 1), md(5, 17), md(5, 28), md(8, 1), md(12, 25), md(12, 26))},
			new object[] {2013, mds(2013, md(1, 1), md(1, 2), md(3, 29), md(4, 1), md(5, 1), md(5, 9), md(5, 20), md(8, 1), md(12, 25), md(12, 26))},
			new object[] {2014, mds(2014, md(1, 1), md(1, 2), md(4, 18), md(4, 21), md(5, 1), md(5, 29), md(6, 9), md(8, 1), md(12, 25), md(12, 26))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 2), md(4, 3), md(4, 6), md(5, 1), md(5, 14), md(5, 25), md(8, 1), md(12, 25), md(12, 26))},
			new object[] {2016, mds(2016, md(1, 1), md(1, 2), md(3, 25), md(3, 28), md(5, 1), md(5, 5), md(5, 16), md(8, 1), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "chzu") public void test_chzu(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_chzu(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(CHZU.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar EUTA = GlobalHolidayCalendars.generateEuropeanTarget();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "euta") public static Object[][] data_euta()
	  public static object[][] data_euta()
	  {
		return new object[][]
		{
			new object[] {1997, mds(1997, md(1, 1), md(12, 25))},
			new object[] {1998, mds(1998, md(1, 1), md(12, 25))},
			new object[] {1999, mds(1999, md(1, 1), md(12, 25), md(12, 31))},
			new object[] {2000, mds(2000, md(1, 1), md(4, 21), md(4, 24), md(5, 1), md(12, 25), md(12, 26))},
			new object[] {2001, mds(2001, md(1, 1), md(4, 13), md(4, 16), md(5, 1), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2002, mds(2002, md(1, 1), md(3, 29), md(4, 1), md(5, 1), md(12, 25), md(12, 26))},
			new object[] {2003, mds(2003, md(1, 1), md(4, 18), md(4, 21), md(5, 1), md(12, 25), md(12, 26))},
			new object[] {2014, mds(2014, md(1, 1), md(4, 18), md(4, 21), md(5, 1), md(12, 25), md(12, 26))},
			new object[] {2015, mds(2015, md(1, 1), md(4, 3), md(4, 6), md(5, 1), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "euta") public void test_euta(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_euta(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(EUTA.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar USGS = GlobalHolidayCalendars.generateUsGovtSecurities();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "usgs") public static Object[][] data_usgs()
	  public static object[][] data_usgs()
	  {
		return new object[][]
		{
			new object[] {1996, mds(1996, md(1, 1), md(1, 15), md(2, 19), md(4, 5), md(5, 27), md(7, 4), md(9, 2), md(10, 14), md(11, 11), md(11, 28), md(12, 25))},
			new object[] {1997, mds(1997, md(1, 1), md(1, 20), md(2, 17), md(3, 28), md(5, 26), md(7, 4), md(9, 1), md(10, 13), md(11, 11), md(11, 27), md(12, 25))},
			new object[] {1998, mds(1998, md(1, 1), md(1, 19), md(2, 16), md(4, 10), md(5, 25), md(7, 3), md(9, 7), md(10, 12), md(11, 11), md(11, 26), md(12, 25))},
			new object[] {1999, mds(1999, md(1, 1), md(1, 18), md(2, 15), md(4, 2), md(5, 31), md(7, 5), md(9, 6), md(10, 11), md(11, 11), md(11, 25), md(12, 24))},
			new object[] {2000, mds(2000, md(1, 17), md(2, 21), md(4, 21), md(5, 29), md(7, 4), md(9, 4), md(10, 9), md(11, 23), md(12, 25))},
			new object[] {2001, mds(2001, md(1, 1), md(1, 15), md(2, 19), md(4, 13), md(5, 28), md(7, 4), md(9, 3), md(10, 8), md(11, 12), md(11, 22), md(12, 25))},
			new object[] {2002, mds(2002, md(1, 1), md(1, 21), md(2, 18), md(3, 29), md(5, 27), md(7, 4), md(9, 2), md(10, 14), md(11, 11), md(11, 28), md(12, 25))},
			new object[] {2003, mds(2003, md(1, 1), md(1, 20), md(2, 17), md(4, 18), md(5, 26), md(7, 4), md(9, 1), md(10, 13), md(11, 11), md(11, 27), md(12, 25))},
			new object[] {2004, mds(2004, md(1, 1), md(1, 19), md(2, 16), md(4, 9), md(5, 31), md(7, 5), md(9, 6), md(10, 11), md(11, 11), md(11, 25), md(12, 24))},
			new object[] {2005, mds(2005, md(1, 17), md(2, 21), md(3, 25), md(5, 30), md(7, 4), md(9, 5), md(10, 10), md(11, 11), md(11, 24), md(12, 26))},
			new object[] {2006, mds(2006, md(1, 2), md(1, 16), md(2, 20), md(4, 14), md(5, 29), md(7, 4), md(9, 4), md(10, 9), md(11, 23), md(12, 25))},
			new object[] {2007, mds(2007, md(1, 1), md(1, 15), md(2, 19), md(4, 6), md(5, 28), md(7, 4), md(9, 3), md(10, 8), md(11, 12), md(11, 22), md(12, 25))},
			new object[] {2008, mds(2008, md(1, 1), md(1, 21), md(2, 18), md(3, 21), md(5, 26), md(7, 4), md(9, 1), md(10, 13), md(11, 11), md(11, 27), md(12, 25))},
			new object[] {2009, mds(2009, md(1, 1), md(1, 19), md(2, 16), md(4, 10), md(5, 25), md(7, 3), md(9, 7), md(10, 12), md(11, 11), md(11, 26), md(12, 25))},
			new object[] {2010, mds(2010, md(1, 1), md(1, 18), md(2, 15), md(4, 2), md(5, 31), md(7, 5), md(9, 6), md(10, 11), md(11, 11), md(11, 25), md(12, 24))},
			new object[] {2011, mds(2011, md(1, 17), md(2, 21), md(4, 22), md(5, 30), md(7, 4), md(9, 5), md(10, 10), md(11, 11), md(11, 24), md(12, 26))},
			new object[] {2012, mds(2012, md(1, 2), md(1, 16), md(2, 20), md(4, 6), md(5, 28), md(7, 4), md(9, 3), md(10, 8), md(10, 30), md(11, 12), md(11, 22), md(12, 25))},
			new object[] {2013, mds(2013, md(1, 1), md(1, 21), md(2, 18), md(3, 29), md(5, 27), md(7, 4), md(9, 2), md(10, 14), md(11, 11), md(11, 28), md(12, 25))},
			new object[] {2014, mds(2014, md(1, 1), md(1, 20), md(2, 17), md(4, 18), md(5, 26), md(7, 4), md(9, 1), md(10, 13), md(11, 11), md(11, 27), md(12, 25))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 19), md(2, 16), md(4, 3), md(5, 25), md(7, 3), md(9, 7), md(10, 12), md(11, 11), md(11, 26), md(12, 25))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "usgs") public void test_usgs(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_usgs(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(USGS.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar USNY = GlobalHolidayCalendars.generateUsNewYork();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "usny") public static Object[][] data_usny()
	  public static object[][] data_usny()
	  {
		return new object[][]
		{
			new object[] {2008, mds(2008, md(1, 1), md(1, 21), md(2, 18), md(5, 26), md(7, 4), md(9, 1), md(10, 13), md(11, 11), md(11, 27), md(12, 25))},
			new object[] {2009, mds(2009, md(1, 1), md(1, 19), md(2, 16), md(5, 25), md(7, 4), md(9, 7), md(10, 12), md(11, 11), md(11, 26), md(12, 25))},
			new object[] {2010, mds(2010, md(1, 1), md(1, 18), md(2, 15), md(5, 31), md(7, 5), md(9, 6), md(10, 11), md(11, 11), md(11, 25), md(12, 25))},
			new object[] {2011, mds(2011, md(1, 1), md(1, 17), md(2, 21), md(5, 30), md(7, 4), md(9, 5), md(10, 10), md(11, 11), md(11, 24), md(12, 26))},
			new object[] {2012, mds(2012, md(1, 2), md(1, 16), md(2, 20), md(5, 28), md(7, 4), md(9, 3), md(10, 8), md(11, 12), md(11, 22), md(12, 25))},
			new object[] {2013, mds(2013, md(1, 1), md(1, 21), md(2, 18), md(5, 27), md(7, 4), md(9, 2), md(10, 14), md(11, 11), md(11, 28), md(12, 25))},
			new object[] {2014, mds(2014, md(1, 1), md(1, 20), md(2, 17), md(5, 26), md(7, 4), md(9, 1), md(10, 13), md(11, 11), md(11, 27), md(12, 25))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 19), md(2, 16), md(5, 25), md(7, 4), md(9, 7), md(10, 12), md(11, 11), md(11, 26), md(12, 25))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "usny") public void test_usny(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_usny(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(USNY.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar NYFD = GlobalHolidayCalendars.generateNewYorkFed();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nyfd") public static Object[][] data_nyfd()
	  public static object[][] data_nyfd()
	  {
		return new object[][]
		{
			new object[] {2003, mds(2003, md(1, 1), md(1, 20), md(2, 17), md(5, 26), md(7, 4), md(9, 1), md(10, 13), md(11, 11), md(11, 27), md(12, 25))},
			new object[] {2004, mds(2004, md(1, 1), md(1, 19), md(2, 16), md(5, 31), md(7, 5), md(9, 6), md(10, 11), md(11, 11), md(11, 25))},
			new object[] {2005, mds(2005, md(1, 17), md(2, 21), md(5, 30), md(7, 4), md(9, 5), md(10, 10), md(11, 11), md(11, 24), md(12, 26))},
			new object[] {2006, mds(2006, md(1, 2), md(1, 16), md(2, 20), md(5, 29), md(7, 4), md(9, 4), md(10, 9), md(11, 23), md(12, 25))},
			new object[] {2007, mds(2007, md(1, 1), md(1, 15), md(2, 19), md(5, 28), md(7, 4), md(9, 3), md(10, 8), md(11, 12), md(11, 22), md(12, 25))},
			new object[] {2008, mds(2008, md(1, 1), md(1, 21), md(2, 18), md(5, 26), md(7, 4), md(9, 1), md(10, 13), md(11, 11), md(11, 27), md(12, 25))},
			new object[] {2009, mds(2009, md(1, 1), md(1, 19), md(2, 16), md(5, 25), md(9, 7), md(10, 12), md(11, 11), md(11, 26), md(12, 25))},
			new object[] {2010, mds(2010, md(1, 1), md(1, 18), md(2, 15), md(5, 31), md(7, 5), md(9, 6), md(10, 11), md(11, 11), md(11, 25))},
			new object[] {2011, mds(2011, md(1, 17), md(2, 21), md(5, 30), md(7, 4), md(9, 5), md(10, 10), md(11, 11), md(11, 24), md(12, 26))},
			new object[] {2012, mds(2012, md(1, 2), md(1, 16), md(2, 20), md(5, 28), md(7, 4), md(9, 3), md(10, 8), md(11, 12), md(11, 22), md(12, 25))},
			new object[] {2013, mds(2013, md(1, 1), md(1, 21), md(2, 18), md(5, 27), md(7, 4), md(9, 2), md(10, 14), md(11, 11), md(11, 28), md(12, 25))},
			new object[] {2014, mds(2014, md(1, 1), md(1, 20), md(2, 17), md(5, 26), md(7, 4), md(9, 1), md(10, 13), md(11, 11), md(11, 27), md(12, 25))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 19), md(2, 16), md(5, 25), md(9, 7), md(10, 12), md(11, 11), md(11, 26), md(12, 25))},
			new object[] {2016, mds(2016, md(1, 1), md(1, 18), md(2, 15), md(5, 30), md(7, 4), md(9, 5), md(10, 10), md(11, 11), md(11, 24), md(12, 26))},
			new object[] {2017, mds(2017, md(1, 2), md(1, 16), md(2, 20), md(5, 29), md(7, 4), md(9, 4), md(10, 9), md(11, 23), md(12, 25))},
			new object[] {2018, mds(2018, md(1, 1), md(1, 15), md(2, 19), md(5, 28), md(7, 4), md(9, 3), md(10, 8), md(11, 12), md(11, 22), md(12, 25))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nyfd") public void test_nyfd(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_nyfd(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(NYFD.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar NYSE = GlobalHolidayCalendars.generateNewYorkStockExchange();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nyse") public static Object[][] data_nyse()
	  public static object[][] data_nyse()
	  {
		return new object[][]
		{
			new object[] {2008, mds(2008, md(1, 1), md(1, 21), md(2, 18), md(3, 21), md(5, 26), md(7, 4), md(9, 1), md(11, 27), md(12, 25))},
			new object[] {2009, mds(2009, md(1, 1), md(1, 19), md(2, 16), md(4, 10), md(5, 25), md(7, 3), md(9, 7), md(11, 26), md(12, 25))},
			new object[] {2010, mds(2010, md(1, 1), md(1, 18), md(2, 15), md(4, 2), md(5, 31), md(7, 5), md(9, 6), md(11, 25), md(12, 24))},
			new object[] {2011, mds(2011, md(1, 1), md(1, 17), md(2, 21), md(4, 22), md(5, 30), md(7, 4), md(9, 5), md(11, 24), md(12, 26))},
			new object[] {2012, mds(2012, md(1, 2), md(1, 16), md(2, 20), md(4, 6), md(5, 28), md(7, 4), md(9, 3), md(10, 30), md(11, 22), md(12, 25))},
			new object[] {2013, mds(2013, md(1, 1), md(1, 21), md(2, 18), md(3, 29), md(5, 27), md(7, 4), md(9, 2), md(11, 28), md(12, 25))},
			new object[] {2014, mds(2014, md(1, 1), md(1, 20), md(2, 17), md(4, 18), md(5, 26), md(7, 4), md(9, 1), md(11, 27), md(12, 25))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 19), md(2, 16), md(4, 3), md(5, 25), md(7, 3), md(9, 7), md(11, 26), md(12, 25))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nyse") public void test_nyse(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_nyse(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(NYSE.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar JPTO = GlobalHolidayCalendars.generateTokyo();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "jpto") public static Object[][] data_jpto()
	  public static object[][] data_jpto()
	  {
		return new object[][]
		{
			new object[] {1999, mds(1999, md(1, 1), md(1, 2), md(1, 3), md(1, 15), md(2, 11), md(3, 22), md(4, 29), md(5, 3), md(5, 4), md(5, 5), md(7, 20), md(9, 15), md(9, 23), md(10, 11), md(11, 3), md(11, 23), md(12, 23), md(12, 31))},
			new object[] {2000, mds(2000, md(1, 1), md(1, 2), md(1, 3), md(1, 10), md(2, 11), md(3, 20), md(4, 29), md(5, 3), md(5, 4), md(5, 5), md(7, 20), md(9, 15), md(9, 23), md(10, 9), md(11, 3), md(11, 23), md(12, 23), md(12, 31))},
			new object[] {2001, mds(2001, md(1, 1), md(1, 2), md(1, 3), md(1, 8), md(2, 12), md(3, 20), md(4, 30), md(5, 3), md(5, 4), md(5, 5), md(7, 20), md(9, 15), md(9, 24), md(10, 8), md(11, 3), md(11, 23), md(12, 24), md(12, 31))},
			new object[] {2002, mds(2002, md(1, 1), md(1, 2), md(1, 3), md(1, 14), md(2, 11), md(3, 21), md(4, 29), md(5, 3), md(5, 4), md(5, 6), md(7, 20), md(9, 16), md(9, 23), md(10, 14), md(11, 4), md(11, 23), md(12, 23), md(12, 31))},
			new object[] {2003, mds(2003, md(1, 1), md(1, 2), md(1, 3), md(1, 13), md(2, 11), md(3, 21), md(4, 29), md(5, 3), md(5, 4), md(5, 5), md(7, 21), md(9, 15), md(9, 23), md(10, 13), md(11, 3), md(11, 24), md(12, 23), md(12, 31))},
			new object[] {2004, mds(2004, md(1, 1), md(1, 2), md(1, 3), md(1, 12), md(2, 11), md(3, 20), md(4, 29), md(5, 3), md(5, 4), md(5, 5), md(7, 19), md(9, 20), md(9, 23), md(10, 11), md(11, 3), md(11, 23), md(12, 23), md(12, 31))},
			new object[] {2005, mds(2005, md(1, 1), md(1, 2), md(1, 3), md(1, 10), md(2, 11), md(3, 21), md(4, 29), md(5, 3), md(5, 4), md(5, 5), md(7, 18), md(9, 19), md(9, 23), md(10, 10), md(11, 3), md(11, 23), md(12, 23), md(12, 31))},
			new object[] {2006, mds(2006, md(1, 1), md(1, 2), md(1, 3), md(1, 9), md(2, 11), md(3, 21), md(4, 29), md(5, 3), md(5, 4), md(5, 5), md(7, 17), md(9, 18), md(9, 23), md(10, 9), md(11, 3), md(11, 23), md(12, 23), md(12, 31))},
			new object[] {2011, mds(2011, md(1, 1), md(1, 2), md(1, 3), md(1, 10), md(2, 11), md(3, 21), md(4, 29), md(5, 3), md(5, 4), md(5, 5), md(7, 18), md(9, 19), md(9, 23), md(10, 10), md(11, 3), md(11, 23), md(12, 23), md(12, 31))},
			new object[] {2012, mds(2012, md(1, 1), md(1, 2), md(1, 3), md(1, 9), md(2, 11), md(3, 20), md(4, 30), md(5, 3), md(5, 4), md(5, 5), md(7, 16), md(9, 17), md(9, 22), md(10, 8), md(11, 3), md(11, 23), md(12, 24), md(12, 31))},
			new object[] {2013, mds(2013, md(1, 1), md(1, 2), md(1, 3), md(1, 14), md(2, 11), md(3, 20), md(4, 29), md(5, 3), md(5, 4), md(5, 5), md(5, 6), md(7, 15), md(9, 16), md(9, 23), md(10, 14), md(11, 4), md(11, 23), md(12, 23), md(12, 31))},
			new object[] {2014, mds(2014, md(1, 1), md(1, 2), md(1, 3), md(1, 13), md(2, 11), md(3, 21), md(4, 29), md(5, 3), md(5, 4), md(5, 5), md(5, 6), md(7, 21), md(9, 15), md(9, 23), md(10, 13), md(11, 3), md(11, 24), md(12, 23), md(12, 31))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 2), md(1, 3), md(1, 12), md(2, 11), md(3, 21), md(4, 29), md(5, 3), md(5, 4), md(5, 5), md(5, 6), md(7, 20), md(9, 21), md(9, 22), md(9, 23), md(10, 12), md(11, 3), md(11, 23), md(12, 23), md(12, 31))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "jpto") public void test_jpto(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_jpto(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(JPTO.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar AUSY = GlobalHolidayCalendars.generateSydney();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "ausy") public static Object[][] data_ausy()
	  public static object[][] data_ausy()
	  {
		return new object[][]
		{
			new object[] {2012, mds(2012, md(1, 1), md(1, 2), md(1, 26), md(4, 6), md(4, 7), md(4, 8), md(4, 9), md(4, 25), md(6, 11), md(8, 6), md(10, 1), md(12, 25), md(12, 26))},
			new object[] {2013, mds(2013, md(1, 1), md(1, 26), md(1, 28), md(3, 29), md(3, 30), md(3, 31), md(4, 1), md(4, 25), md(6, 10), md(8, 5), md(10, 7), md(12, 25), md(12, 26))},
			new object[] {2014, mds(2014, md(1, 1), md(1, 26), md(1, 27), md(4, 18), md(4, 19), md(4, 20), md(4, 21), md(4, 25), md(6, 9), md(8, 4), md(10, 6), md(12, 25), md(12, 26))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 26), md(4, 3), md(4, 4), md(4, 5), md(4, 6), md(4, 25), md(6, 8), md(8, 3), md(10, 5), md(12, 25), md(12, 26), md(12, 27), md(12, 28))},
			new object[] {2016, mds(2016, md(1, 1), md(1, 26), md(3, 25), md(3, 26), md(3, 27), md(3, 28), md(4, 25), md(6, 13), md(8, 1), md(10, 3), md(12, 25), md(12, 26), md(12, 27))},
			new object[] {2017, mds(2017, md(1, 1), md(1, 2), md(1, 26), md(4, 14), md(4, 15), md(4, 16), md(4, 17), md(4, 25), md(6, 12), md(8, 7), md(10, 2), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "ausy") public void test_ausy(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_ausy(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(AUSY.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar BRBD = GlobalHolidayCalendars.generateBrazil();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "brbd") public static Object[][] data_brbd()
	  public static object[][] data_brbd()
	  {
		// http://www.planalto.gov.br/ccivil_03/leis/2002/L10607.htm
		// fixing data
		return new object[][]
		{
			new object[] {2013, mds(2013, md(1, 1), md(2, 11), md(2, 12), md(3, 29), md(4, 21), md(5, 1), md(5, 30), md(9, 7), md(10, 12), md(11, 2), md(11, 15), md(12, 25))},
			new object[] {2014, mds(2014, md(1, 1), md(3, 3), md(3, 4), md(4, 18), md(4, 21), md(5, 1), md(6, 19), md(9, 7), md(10, 12), md(11, 2), md(11, 15), md(12, 25))},
			new object[] {2015, mds(2015, md(1, 1), md(2, 16), md(2, 17), md(4, 3), md(4, 21), md(5, 1), md(6, 4), md(9, 7), md(10, 12), md(11, 2), md(11, 15), md(12, 25))},
			new object[] {2016, mds(2016, md(1, 1), md(2, 8), md(2, 9), md(3, 25), md(4, 21), md(5, 1), md(5, 26), md(9, 7), md(10, 12), md(11, 2), md(11, 15), md(12, 25))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "brbd") public void test_brbd(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_brbd(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(BRBD.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar CAMO = GlobalHolidayCalendars.generateMontreal();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "camo") public static Object[][] data_camo()
	  public static object[][] data_camo()
	  {
		// https://www.bankofcanada.ca/about/contact-information/bank-of-canada-holiday-schedule/
		// also indicate day after new year and boxing day, but no other sources for this
		return new object[][]
		{
			new object[] {2017, mds(2017, md(1, 2), md(4, 14), md(5, 22), md(6, 26), md(7, 3), md(9, 4), md(10, 9), md(12, 25))},
			new object[] {2018, mds(2018, md(1, 1), md(3, 30), md(5, 21), md(6, 25), md(7, 2), md(9, 3), md(10, 8), md(12, 25))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "camo") public void test_camo(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_camo(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(CAMO.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar CATO = GlobalHolidayCalendars.generateToronto();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "cato") public static Object[][] data_cato()
	  public static object[][] data_cato()
	  {
		return new object[][]
		{
			new object[] {2009, mds(2009, md(1, 1), md(2, 16), md(4, 10), md(5, 18), md(7, 1), md(8, 3), md(9, 7), md(10, 12), md(11, 11), md(12, 25), md(12, 28))},
			new object[] {2010, mds(2010, md(1, 1), md(2, 15), md(4, 2), md(5, 24), md(7, 1), md(8, 2), md(9, 6), md(10, 11), md(11, 11), md(12, 27), md(12, 28))},
			new object[] {2011, mds(2011, md(1, 3), md(2, 21), md(4, 22), md(5, 23), md(7, 1), md(8, 1), md(9, 5), md(10, 10), md(11, 11), md(12, 26), md(12, 27))},
			new object[] {2012, mds(2012, md(1, 2), md(2, 20), md(4, 6), md(5, 21), md(7, 2), md(8, 6), md(9, 3), md(10, 8), md(11, 12), md(12, 25), md(12, 26))},
			new object[] {2013, mds(2013, md(1, 1), md(2, 18), md(3, 29), md(5, 20), md(7, 1), md(8, 5), md(9, 2), md(10, 14), md(11, 11), md(12, 25), md(12, 26))},
			new object[] {2014, mds(2014, md(1, 1), md(2, 17), md(4, 18), md(5, 19), md(7, 1), md(8, 4), md(9, 1), md(10, 13), md(11, 11), md(12, 25), md(12, 26))},
			new object[] {2015, mds(2015, md(1, 1), md(2, 16), md(4, 3), md(5, 18), md(7, 1), md(8, 3), md(9, 7), md(10, 12), md(11, 11), md(12, 25), md(12, 28))},
			new object[] {2016, mds(2016, md(1, 1), md(2, 15), md(3, 25), md(5, 23), md(7, 1), md(8, 1), md(9, 5), md(10, 10), md(11, 11), md(12, 26), md(12, 27))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "cato") public void test_cato(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_cato(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(CATO.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar CZPR = GlobalHolidayCalendars.generatePrague();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "czpr") public static Object[][] data_czpr()
	  public static object[][] data_czpr()
	  {
		// official data from Czech National Bank
		// https://www.cnb.cz/en/public/media_service/schedules/media_svatky.html
		return new object[][]
		{
			new object[] {2008, mds(2008, md(1, 1), md(3, 24), md(5, 1), md(5, 8), md(7, 5), md(7, 6), md(9, 28), md(10, 28), md(11, 17), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2009, mds(2009, md(1, 1), md(4, 13), md(5, 1), md(5, 8), md(7, 5), md(7, 6), md(9, 28), md(10, 28), md(11, 17), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2010, mds(2010, md(1, 1), md(4, 5), md(5, 1), md(5, 8), md(7, 5), md(7, 6), md(9, 28), md(10, 28), md(11, 17), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2011, mds(2011, md(1, 1), md(4, 25), md(5, 1), md(5, 8), md(7, 5), md(7, 6), md(9, 28), md(10, 28), md(11, 17), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2012, mds(2012, md(1, 1), md(4, 9), md(5, 1), md(5, 8), md(7, 5), md(7, 6), md(9, 28), md(10, 28), md(11, 17), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2013, mds(2013, md(1, 1), md(4, 1), md(5, 1), md(5, 8), md(7, 5), md(7, 6), md(9, 28), md(10, 28), md(11, 17), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2014, mds(2014, md(1, 1), md(4, 21), md(5, 1), md(5, 8), md(7, 5), md(7, 6), md(9, 28), md(10, 28), md(11, 17), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2015, mds(2015, md(1, 1), md(4, 6), md(5, 1), md(5, 8), md(7, 5), md(7, 6), md(9, 28), md(10, 28), md(11, 17), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2016, mds(2016, md(1, 1), md(3, 25), md(3, 28), md(5, 1), md(5, 8), md(7, 5), md(7, 6), md(9, 28), md(10, 28), md(11, 17), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2017, mds(2017, md(1, 1), md(4, 14), md(4, 17), md(5, 1), md(5, 8), md(7, 5), md(7, 6), md(9, 28), md(10, 28), md(11, 17), md(12, 24), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "czpr") public void test_czpr(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_czpr(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(CZPR.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar DKCO = GlobalHolidayCalendars.generateCopenhagen();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "dkco") public static Object[][] data_dkco()
	  public static object[][] data_dkco()
	  {
		// official data from Danish Bankers association via web archive
		return new object[][]
		{
			new object[] {2013, mds(2013, md(1, 1), md(3, 28), md(3, 29), md(4, 1), md(4, 26), md(5, 9), md(5, 10), md(5, 20), md(6, 5), md(12, 24), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2014, mds(2014, md(1, 1), md(4, 17), md(4, 18), md(4, 21), md(5, 16), md(5, 29), md(5, 30), md(6, 5), md(6, 9), md(12, 24), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2015, mds(2015, md(1, 1), md(4, 2), md(4, 3), md(4, 6), md(5, 1), md(5, 14), md(5, 15), md(5, 25), md(6, 5), md(12, 24), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2016, mds(2016, md(1, 1), md(3, 24), md(3, 25), md(3, 28), md(4, 22), md(5, 5), md(5, 6), md(5, 16), md(6, 5), md(12, 24), md(12, 25), md(12, 26), md(12, 31))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "dkco") public void test_dkco(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_dkco(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(DKCO.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar HUBU = GlobalHolidayCalendars.generateBudapest();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "hubu") public static Object[][] data_hubu()
	  public static object[][] data_hubu()
	  {
		// http://www.mnb.hu/letoltes/bubor2.xls
		// http://holidays.kayaposoft.com/public_holidays.php?year=2013&country=hun&region=#
		return new object[][]
		{
			new object[] {2012, mds(2012, md(3, 15), md(3, 16), md(4, 9), md(4, 30), md(5, 1), md(5, 28), md(8, 20), md(10, 22), md(10, 23), md(11, 1), md(11, 2), md(12, 24), md(12, 25), md(12, 26), md(12, 31)), ImmutableList.of(date(2012, 3, 24), date(2012, 5, 5), date(2012, 10, 27), date(2012, 11, 10), date(2012, 12, 15), date(2012, 12, 29))},
			new object[] {2013, mds(2013, md(1, 1), md(3, 15), md(4, 1), md(5, 1), md(5, 20), md(8, 19), md(8, 20), md(10, 23), md(11, 1), md(12, 24), md(12, 25), md(12, 26), md(12, 27)), ImmutableList.of(date(2013, 8, 24), date(2013, 12, 7), date(2013, 12, 21))},
			new object[] {2014, mds(2014, md(1, 1), md(3, 15), md(4, 21), md(5, 1), md(5, 2), md(6, 9), md(8, 20), md(10, 23), md(10, 24), md(12, 24), md(12, 25), md(12, 26)), ImmutableList.of(date(2014, 5, 10), date(2014, 10, 18))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 2), md(3, 15), md(4, 6), md(5, 1), md(5, 25), md(8, 20), md(8, 21), md(10, 23), md(12, 24), md(12, 25), md(12, 26)), ImmutableList.of(date(2015, 1, 10), date(2015, 8, 8), date(2015, 12, 12))},
			new object[] {2016, mds(2016, md(1, 1), md(3, 14), md(3, 15), md(3, 28), md(5, 1), md(5, 16), md(10, 31), md(11, 1), md(12, 24), md(12, 25), md(12, 26)), ImmutableList.of(date(2016, 3, 5), date(2016, 10, 15))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "hubu") public void test_hubu(int year, java.util.List<java.time.LocalDate> holidays, java.util.List<java.time.LocalDate> workDays)
	  public virtual void test_hubu(int year, IList<LocalDate> holidays, IList<LocalDate> workDays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = (holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY) && !workDays.Contains(date);
		  assertEquals(HUBU.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar MXMC = GlobalHolidayCalendars.generateMexicoCity();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "mxmc") public static Object[][] data_mxmc()
	  public static object[][] data_mxmc()
	  {
		// http://www.banxico.org.mx/SieInternet/consultarDirectorioInternetAction.do?accion=consultarCuadro&idCuadro=CF111&locale=en
		return new object[][]
		{
			new object[] {2012, mds(2012, md(1, 1), md(2, 6), md(3, 19), md(4, 5), md(4, 6), md(5, 1), md(9, 16), md(11, 2), md(11, 19), md(12, 12), md(12, 25))},
			new object[] {2013, mds(2013, md(1, 1), md(2, 4), md(3, 18), md(3, 28), md(3, 29), md(5, 1), md(9, 16), md(11, 2), md(11, 18), md(12, 12), md(12, 25))},
			new object[] {2014, mds(2014, md(1, 1), md(2, 3), md(3, 17), md(4, 17), md(4, 18), md(5, 1), md(9, 16), md(11, 2), md(11, 17), md(12, 12), md(12, 25))},
			new object[] {2015, mds(2015, md(1, 1), md(2, 2), md(3, 16), md(4, 2), md(4, 3), md(5, 1), md(9, 16), md(11, 2), md(11, 16), md(12, 12), md(12, 25))},
			new object[] {2016, mds(2016, md(1, 1), md(2, 1), md(3, 21), md(3, 24), md(3, 25), md(5, 1), md(9, 16), md(11, 2), md(11, 21), md(12, 12), md(12, 25))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "mxmc") public void test_mxmc(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_mxmc(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(MXMC.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar NOOS = GlobalHolidayCalendars.generateOslo();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "noos") public static Object[][] data_noos()
	  public static object[][] data_noos()
	  {
		// official data from Oslo Bors via web archive
		return new object[][]
		{
			new object[] {2009, mds(2009, md(1, 1), md(4, 9), md(4, 10), md(4, 13), md(5, 1), md(5, 21), md(6, 1), md(12, 24), md(12, 25), md(12, 31))},
			new object[] {2011, mds(2011, md(4, 21), md(4, 22), md(4, 25), md(5, 17), md(6, 2), md(6, 13), md(12, 26))},
			new object[] {2012, mds(2012, md(4, 5), md(4, 6), md(4, 9), md(5, 1), md(5, 17), md(5, 28), md(12, 24), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2013, mds(2013, md(1, 1), md(3, 28), md(3, 29), md(4, 1), md(5, 1), md(5, 9), md(5, 17), md(5, 20), md(12, 24), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2014, mds(2014, md(1, 1), md(4, 17), md(4, 18), md(4, 21), md(5, 1), md(5, 17), md(5, 29), md(6, 9), md(12, 24), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2015, mds(2015, md(1, 1), md(4, 2), md(4, 3), md(4, 6), md(5, 1), md(5, 14), md(5, 25), md(12, 24), md(12, 25), md(12, 31))},
			new object[] {2016, mds(2016, md(1, 1), md(3, 24), md(3, 25), md(3, 28), md(5, 5), md(5, 16), md(5, 17), md(12, 26))},
			new object[] {2017, mds(2017, md(4, 13), md(4, 14), md(4, 17), md(5, 1), md(5, 17), md(5, 25), md(6, 5), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "noos") public void test_noos(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_noos(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(NOOS.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar NZAU = GlobalHolidayCalendars.generateAuckland();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nzau") public static Object[][] data_nzau()
	  public static object[][] data_nzau()
	  {
		// https://www.govt.nz/browse/work/public-holidays-and-work/public-holidays-and-anniversary-dates/
		// https://www.employment.govt.nz/leave-and-holidays/public-holidays/public-holidays-and-anniversary-dates/dates-for-previous-years/
		return new object[][]
		{
			new object[] {2015, mds(2015, md(1, 1), md(1, 2), md(1, 26), md(2, 6), md(4, 3), md(4, 6), md(4, 27), md(6, 1), md(10, 26), md(12, 25), md(12, 28))},
			new object[] {2016, mds(2016, md(1, 1), md(1, 4), md(2, 1), md(2, 8), md(3, 25), md(3, 28), md(4, 25), md(6, 6), md(10, 24), md(12, 26), md(12, 27))},
			new object[] {2017, mds(2017, md(1, 2), md(1, 3), md(1, 30), md(2, 6), md(4, 14), md(4, 17), md(4, 25), md(6, 5), md(10, 23), md(12, 25), md(12, 26))},
			new object[] {2018, mds(2018, md(1, 1), md(1, 2), md(1, 29), md(2, 6), md(3, 30), md(4, 2), md(4, 25), md(6, 4), md(10, 22), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nzau") public void test_nzau(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_nzau(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(NZAU.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar NZWE = GlobalHolidayCalendars.generateWellington();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nzwe") public static Object[][] data_nzwe()
	  public static object[][] data_nzwe()
	  {
		// https://www.govt.nz/browse/work/public-holidays-and-work/public-holidays-and-anniversary-dates/
		// https://www.employment.govt.nz/leave-and-holidays/public-holidays/public-holidays-and-anniversary-dates/dates-for-previous-years/
		return new object[][]
		{
			new object[] {2015, mds(2015, md(1, 1), md(1, 2), md(1, 19), md(2, 6), md(4, 3), md(4, 6), md(4, 27), md(6, 1), md(10, 26), md(12, 25), md(12, 28))},
			new object[] {2016, mds(2016, md(1, 1), md(1, 4), md(1, 25), md(2, 8), md(3, 25), md(3, 28), md(4, 25), md(6, 6), md(10, 24), md(12, 26), md(12, 27))},
			new object[] {2017, mds(2017, md(1, 2), md(1, 3), md(1, 23), md(2, 6), md(4, 14), md(4, 17), md(4, 25), md(6, 5), md(10, 23), md(12, 25), md(12, 26))},
			new object[] {2018, mds(2018, md(1, 1), md(1, 2), md(1, 22), md(2, 6), md(3, 30), md(4, 2), md(4, 25), md(6, 4), md(10, 22), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nzwe") public void test_nzwe(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_nzwe(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(NZWE.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar NZBD = GlobalHolidayCalendars.generateNewZealand();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nzbd") public static Object[][] data_nzbd()
	  public static object[][] data_nzbd()
	  {
		// https://www.govt.nz/browse/work/public-holidays-and-work/public-holidays-and-anniversary-dates/
		// https://www.employment.govt.nz/leave-and-holidays/public-holidays/public-holidays-and-anniversary-dates/dates-for-previous-years/
		return new object[][]
		{
			new object[] {2015, mds(2015, md(1, 1), md(1, 2), md(2, 6), md(4, 3), md(4, 6), md(4, 27), md(6, 1), md(10, 26), md(12, 25), md(12, 28))},
			new object[] {2016, mds(2016, md(1, 1), md(1, 4), md(2, 8), md(3, 25), md(3, 28), md(4, 25), md(6, 6), md(10, 24), md(12, 26), md(12, 27))},
			new object[] {2017, mds(2017, md(1, 2), md(1, 3), md(2, 6), md(4, 14), md(4, 17), md(4, 25), md(6, 5), md(10, 23), md(12, 25), md(12, 26))},
			new object[] {2018, mds(2018, md(1, 1), md(1, 2), md(2, 6), md(3, 30), md(4, 2), md(4, 25), md(6, 4), md(10, 22), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nzbd") public void test_nzbd(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_nzbd(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(NZBD.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar PLWA = GlobalHolidayCalendars.generateWarsaw();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "plwa") public static Object[][] data_plwa()
	  public static object[][] data_plwa()
	  {
		// based on government law data and stock exchange holidays
		return new object[][]
		{
			new object[] {2013, mds(2013, md(1, 1), md(3, 29), md(4, 1), md(5, 1), md(5, 3), md(5, 30), md(8, 15), md(11, 1), md(11, 11), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2014, mds(2014, md(1, 1), md(1, 6), md(4, 18), md(4, 21), md(5, 1), md(6, 19), md(8, 15), md(11, 11), md(12, 24), md(12, 25), md(12, 26))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 6), md(4, 3), md(4, 6), md(5, 1), md(6, 4), md(11, 11), md(12, 24), md(12, 25), md(12, 31))},
			new object[] {2016, mds(2016, md(1, 1), md(1, 6), md(3, 25), md(3, 28), md(5, 3), md(5, 26), md(8, 15), md(11, 1), md(11, 11), md(12, 26))},
			new object[] {2017, mds(2017, md(1, 6), md(4, 14), md(4, 17), md(5, 1), md(5, 3), md(6, 15), md(8, 15), md(11, 1), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "plwa") public void test_plwa(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_plwa(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(PLWA.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar SEST = GlobalHolidayCalendars.generateStockholm();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "sest") public static Object[][] data_sest()
	  public static object[][] data_sest()
	  {
		// official data from published fixing dates
		return new object[][]
		{
			new object[] {2014, mds(2014, md(1, 1), md(1, 6), md(4, 18), md(4, 21), md(5, 1), md(5, 29), md(6, 6), md(6, 20), md(12, 24), md(12, 25), md(12, 26), md(12, 31))},
			new object[] {2015, mds(2015, md(1, 1), md(1, 6), md(4, 3), md(4, 6), md(5, 1), md(5, 14), md(6, 19), md(12, 24), md(12, 25), md(12, 31))},
			new object[] {2016, mds(2016, md(1, 1), md(1, 6), md(3, 25), md(3, 28), md(5, 5), md(6, 6), md(6, 24), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "sest") public void test_sest(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_sest(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(SEST.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static readonly HolidayCalendar ZAJO = GlobalHolidayCalendars.generateJohannesburg();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "zajo") public static Object[][] data_zajo()
	  public static object[][] data_zajo()
	  {
		// http://www.gov.za/about-sa/public-holidays
		// https://web.archive.org/web/20151230214958/http://www.gov.za/about-sa/public-holidays
		return new object[][]
		{
			new object[] {2015, mds(2015, md(1, 1), md(3, 21), md(4, 3), md(4, 6), md(4, 27), md(5, 1), md(6, 16), md(8, 10), md(9, 24), md(12, 16), md(12, 25), md(12, 26))},
			new object[] {2016, mds(2016, md(1, 1), md(3, 21), md(3, 25), md(3, 28), md(4, 27), md(5, 2), md(6, 16), md(8, 3), md(8, 9), md(9, 24), md(12, 16), md(12, 26), md(12, 27))},
			new object[] {2017, mds(2017, md(1, 1), md(1, 2), md(3, 21), md(4, 14), md(4, 17), md(4, 27), md(5, 1), md(6, 16), md(8, 9), md(9, 25), md(12, 16), md(12, 16), md(12, 25), md(12, 26))}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "zajo") public void test_zajo(int year, java.util.List<java.time.LocalDate> holidays)
	  public virtual void test_zajo(int year, IList<LocalDate> holidays)
	  {
		LocalDate date = LocalDate.of(year, 1, 1);
		int len = date.lengthOfYear();
		for (int i = 0; i < len; i++)
		{
		  bool isHoliday = holidays.Contains(date) || date.DayOfWeek == SATURDAY || date.DayOfWeek == SUNDAY;
		  assertEquals(ZAJO.isHoliday(date), isHoliday, date.ToString());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		HolidayCalendar combined = ImmutableHolidayCalendar.combined((ImmutableHolidayCalendar) JPTO, (ImmutableHolidayCalendar) USNY);
		LocalDate date = LocalDate.of(1950, 1, 1);
		while (date.Year < 2040)
		{
		  assertEquals(combined.isHoliday(date), JPTO.isHoliday(date) || USNY.isHoliday(date), "Date: " + date);
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  private static IList<LocalDate> mds(int year, params MonthDay[] monthDays)
	  {
		IList<LocalDate> holidays = new List<LocalDate>();
		foreach (MonthDay md in monthDays)
		{
		  holidays.Add(md.atYear(year));
		}
		return holidays;
	  }

	  private static MonthDay md(int month, int day)
	  {
		return MonthDay.of(month, day);
	  }

	  //-------------------------------------------------------------------------
	  public static void coverage()
	  {
		coverPrivateConstructor(typeof(GlobalHolidayCalendars));
	  }

	}

}