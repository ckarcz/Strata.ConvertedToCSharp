/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertUtilityClass;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="DateAdjusters"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DateAdjustersTest
	public class DateAdjustersTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nextLeapDay") public static Object[][] data_nextLeapDay()
		public static object[][] data_nextLeapDay()
		{
		return new object[][]
		{
			new object[] {2000, 1, 1, 2000},
			new object[] {2000, 2, 1, 2000},
			new object[] {2000, 2, 28, 2000},
			new object[] {2000, 2, 29, 2004},
			new object[] {2000, 3, 1, 2004},
			new object[] {2009, 1, 1, 2012},
			new object[] {2009, 2, 1, 2012},
			new object[] {2009, 2, 28, 2012},
			new object[] {2009, 3, 1, 2012},
			new object[] {2010, 1, 1, 2012},
			new object[] {2010, 2, 1, 2012},
			new object[] {2010, 2, 28, 2012},
			new object[] {2010, 3, 1, 2012},
			new object[] {2012, 1, 1, 2012},
			new object[] {2012, 2, 1, 2012},
			new object[] {2012, 2, 28, 2012},
			new object[] {2012, 2, 29, 2016},
			new object[] {2012, 3, 1, 2016},
			new object[] {2013, 1, 1, 2016},
			new object[] {2013, 2, 1, 2016},
			new object[] {2013, 2, 28, 2016},
			new object[] {2013, 3, 1, 2016},
			new object[] {2014, 1, 1, 2016},
			new object[] {2014, 2, 1, 2016},
			new object[] {2014, 2, 28, 2016},
			new object[] {2014, 3, 1, 2016},
			new object[] {2015, 1, 1, 2016},
			new object[] {2015, 2, 1, 2016},
			new object[] {2015, 2, 28, 2016},
			new object[] {2015, 3, 1, 2016},
			new object[] {2016, 1, 1, 2016},
			new object[] {2016, 2, 1, 2016},
			new object[] {2016, 2, 28, 2016},
			new object[] {2016, 2, 29, 2020},
			new object[] {2016, 3, 1, 2020},
			new object[] {2017, 1, 1, 2020},
			new object[] {2096, 1, 1, 2096},
			new object[] {2096, 2, 1, 2096},
			new object[] {2096, 2, 28, 2096},
			new object[] {2096, 2, 29, 2104},
			new object[] {2096, 3, 1, 2104},
			new object[] {2100, 1, 1, 2104},
			new object[] {2100, 2, 1, 2104},
			new object[] {2100, 2, 28, 2104},
			new object[] {2100, 3, 1, 2104}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nextLeapDay") public void test_nextLeapDay_LocalDate(int year, int month, int day, int expectedYear)
	  public virtual void test_nextLeapDay_LocalDate(int year, int month, int day, int expectedYear)
	  {
		LocalDate date = LocalDate.of(year, month, day);
		LocalDate test = DateAdjusters.nextLeapDay().adjust(date);
		assertEquals(test.Year, expectedYear);
		assertEquals(test.MonthValue, 2);
		assertEquals(test.DayOfMonth, 29);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nextLeapDay") public void test_nextLeapDay_Temporal(int year, int month, int day, int expectedYear)
	  public virtual void test_nextLeapDay_Temporal(int year, int month, int day, int expectedYear)
	  {
		LocalDate date = LocalDate.of(year, month, day);
		LocalDate test = (LocalDate) DateAdjusters.nextLeapDay().adjustInto(date);
		assertEquals(test.Year, expectedYear);
		assertEquals(test.MonthValue, 2);
		assertEquals(test.DayOfMonth, 29);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nextLeapDay") public void test_nextOrSameLeapDay_LocalDate(int year, int month, int day, int expectedYear)
	  public virtual void test_nextOrSameLeapDay_LocalDate(int year, int month, int day, int expectedYear)
	  {
		LocalDate date = LocalDate.of(year, month, day);
		LocalDate test = DateAdjusters.nextOrSameLeapDay().adjust(date);
		if (month == 2 && day == 29)
		{
		  assertEquals(test, date);
		}
		else
		{
		  assertEquals(test.Year, expectedYear);
		  assertEquals(test.MonthValue, 2);
		  assertEquals(test.DayOfMonth, 29);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nextLeapDay") public void test_nextOrSameLeapDay_Temporal(int year, int month, int day, int expectedYear)
	  public virtual void test_nextOrSameLeapDay_Temporal(int year, int month, int day, int expectedYear)
	  {
		LocalDate date = LocalDate.of(year, month, day);
		LocalDate test = (LocalDate) DateAdjusters.nextOrSameLeapDay().adjustInto(date);
		if (month == 2 && day == 29)
		{
		  assertEquals(test, date);
		}
		else
		{
		  assertEquals(test.Year, expectedYear);
		  assertEquals(test.MonthValue, 2);
		  assertEquals(test.DayOfMonth, 29);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		assertUtilityClass(typeof(DateAdjusters));
	  }

	}

}