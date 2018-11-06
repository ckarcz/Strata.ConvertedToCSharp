/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.timeseries
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test LocalDateDoublePoint.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LocalDateDoublePointTest
	public class LocalDateDoublePointTest
	{

	  private static readonly LocalDate DATE_2012_06_29 = LocalDate.of(2012, 6, 29);
	  private static readonly LocalDate DATE_2012_06_30 = LocalDate.of(2012, 6, 30);
	  private static readonly LocalDate DATE_2012_07_01 = LocalDate.of(2012, 7, 1);
	  private const double TOLERANCE = 0.00001d;
	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		LocalDateDoublePoint test = LocalDateDoublePoint.of(DATE_2012_06_30, 1d);
		assertEquals(test.Date, DATE_2012_06_30);
		assertEquals(test.Value, 1d, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_nullDate()
	  public virtual void test_of_nullDate()
	  {
		LocalDateDoublePoint.of(null, 1d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withDate()
	  {
		LocalDateDoublePoint @base = LocalDateDoublePoint.of(DATE_2012_06_30, 1d);
		LocalDateDoublePoint test = @base.withDate(DATE_2012_06_29);
		assertEquals(test.Date, DATE_2012_06_29);
		assertEquals(test.Value, 1d, TOLERANCE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_withDate_nullDate()
	  public virtual void test_withDate_nullDate()
	  {
		LocalDateDoublePoint @base = LocalDateDoublePoint.of(DATE_2012_06_30, 1d);
		@base.withDate(null);
	  }

	  public virtual void test_withValue()
	  {
		LocalDateDoublePoint @base = LocalDateDoublePoint.of(DATE_2012_06_30, 1d);
		LocalDateDoublePoint test = @base.withValue(2d);
		assertEquals(test.Date, DATE_2012_06_30);
		assertEquals(test.Value, 2d, TOLERANCE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		LocalDateDoublePoint a = LocalDateDoublePoint.of(DATE_2012_06_29, 1d);
		LocalDateDoublePoint b = LocalDateDoublePoint.of(DATE_2012_06_30, 1d);
		LocalDateDoublePoint c = LocalDateDoublePoint.of(DATE_2012_07_01, 1d);

		assertTrue(a.CompareTo(a) == 0);
		assertTrue(a.CompareTo(b) < 0);
		assertTrue(a.CompareTo(c) < 0);
		assertTrue(b.CompareTo(a) > 0);
		assertTrue(b.CompareTo(b) == 0);
		assertTrue(b.CompareTo(c) < 0);
		assertTrue(c.CompareTo(a) > 0);
		assertTrue(c.CompareTo(b) > 0);
		assertTrue(c.CompareTo(c) == 0);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode_differentDates()
	  {
		LocalDateDoublePoint a1 = LocalDateDoublePoint.of(DATE_2012_06_29, 1d);
		LocalDateDoublePoint a2 = LocalDateDoublePoint.of(DATE_2012_06_29, 1d);
		LocalDateDoublePoint b = LocalDateDoublePoint.of(DATE_2012_06_30, 1d);
		LocalDateDoublePoint c = LocalDateDoublePoint.of(DATE_2012_07_01, 1d);

		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);
		assertEquals(a1.GetHashCode(), a1.GetHashCode());
	  }

	  public virtual void test_equalsHashCode_differentValues()
	  {
		LocalDateDoublePoint a1 = LocalDateDoublePoint.of(DATE_2012_06_29, 1d);
		LocalDateDoublePoint a2 = LocalDateDoublePoint.of(DATE_2012_06_29, 1d);
		LocalDateDoublePoint b = LocalDateDoublePoint.of(DATE_2012_06_29, 2d);
		LocalDateDoublePoint c = LocalDateDoublePoint.of(DATE_2012_06_29, 3d);

		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);
		assertEquals(a1.GetHashCode(), a1.GetHashCode());
	  }

	  public virtual void test_equalsBad()
	  {
		LocalDateDoublePoint a = LocalDateDoublePoint.of(DATE_2012_06_29, 1d);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
		assertEquals(a.Equals(null), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toString()
	  {
		LocalDateDoublePoint test = LocalDateDoublePoint.of(DATE_2012_06_29, 1d);
		assertEquals(test.ToString(), "(2012-06-29=1.0)");
	  }

	}

}