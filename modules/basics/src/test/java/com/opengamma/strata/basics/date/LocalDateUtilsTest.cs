/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertUtilityClass;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LocalDateUtilsTest
	public class LocalDateUtilsTest
	{

	  public virtual void test_dayOfYear()
	  {
		LocalDate date = LocalDate.of(2012, 1, 1);
		for (int i = 0; i < 366 * 4; i++)
		{
		  assertEquals(LocalDateUtils.doy(date), date.DayOfYear);
		  date = date.plusDays(1);
		}
	  }

	  public virtual void test_plusDays0()
	  {
		LocalDate date = LocalDate.of(2012, 1, 1);
		for (int i = 0; i < 366 * 4; i++)
		{
		  assertEquals(LocalDateUtils.plusDays(date, 0), date.plusDays(0));
		  date = date.plusDays(1);
		}
	  }

	  public virtual void test_plusDays1()
	  {
		LocalDate date = LocalDate.of(2012, 1, 1);
		for (int i = 0; i < 366 * 4; i++)
		{
		  assertEquals(LocalDateUtils.plusDays(date, 1), date.plusDays(1));
		  date = date.plusDays(1);
		}
	  }

	  public virtual void test_plusDays3()
	  {
		LocalDate date = LocalDate.of(2012, 1, 1);
		for (int i = 0; i < 366 * 4; i++)
		{
		  assertEquals(LocalDateUtils.plusDays(date, 3), date.plusDays(3));
		  date = date.plusDays(1);
		}
	  }

	  public virtual void test_plusDays99()
	  {
		LocalDate date = LocalDate.of(2012, 1, 1);
		for (int i = 0; i < 366 * 4; i++)
		{
		  assertEquals(LocalDateUtils.plusDays(date, 99), date.plusDays(99));
		  date = date.plusDays(1);
		}
	  }

	  public virtual void test_plusDaysM1()
	  {
		LocalDate date = LocalDate.of(2012, 1, 1);
		for (int i = 0; i < 366 * 4; i++)
		{
		  assertEquals(LocalDateUtils.plusDays(date, -1), date.plusDays(-1));
		  date = date.plusDays(1);
		}
	  }

	  public virtual void test_daysBetween()
	  {
		LocalDate @base = LocalDate.of(2012, 1, 1);
		LocalDate date = @base;
		for (int i = 0; i < 366 * 8; i++)
		{
		  assertEquals(LocalDateUtils.daysBetween(@base, date), date.toEpochDay() - @base.toEpochDay());
		  date = date.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		assertUtilityClass(typeof(LocalDateUtils));
	  }

	}

}