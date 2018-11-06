/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Assert = org.testng.Assert;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AverageCalculatorTest
	public class AverageCalculatorTest
	{
	  private static readonly double[] DATA = new double[] {1.0, 1.0, 3.0, 2.5, 5.7, 3.7, 5.7, 5.7, -4.0, 9.0};
	  private static readonly System.Func<double[], double> MEAN = new MeanCalculator();
	  private static readonly System.Func<double[], double> MEDIAN = new MedianCalculator();
	  private static readonly System.Func<double[], double> MODE = new ModeCalculator();
	  private const double EPS = 1e-15;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNull()
	  public virtual void testNull()
	  {
		assertNull(MEAN);
		assertNull(MEDIAN);
		assertNull(MODE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testEmpty()
	  public virtual void testEmpty()
	  {
		assertEmpty(MEAN);
		assertEmpty(MEDIAN);
		assertEmpty(MODE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testSingleValue()
	  public virtual void testSingleValue()
	  {
		double value = 3.0;
		double[] x = new double[] {value};
		assertEquals(value, MEAN.apply(x), EPS);
		assertEquals(value, MEDIAN.apply(x), EPS);
		assertEquals(value, MODE.apply(x), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testMean()
	  public virtual void testMean()
	  {
		assertEquals(MEAN.apply(DATA), 3.33, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testMedian()
	  public virtual void testMedian()
	  {
		assertEquals(MEDIAN.apply(DATA), 3.35, EPS);
		double[] x = Arrays.copyOf(DATA, DATA.Length - 1);
		assertEquals(MEDIAN.apply(x), 3, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testMode()
	  public virtual void testMode()
	  {
		double[] x = new double[] {1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0};
		try
		{
		  MODE.apply(x);
		  Assert.fail();
		}
		catch (MathException)
		{
		  // Expected
		}
		assertEquals(MODE.apply(DATA), 5.7, EPS);
	  }

	  private void assertNull(System.Func<double[], double> calculator)
	  {
		try
		{
		  calculator(null);
		  Assert.fail();
		}
		catch (System.ArgumentException)
		{
		  // Expected
		}
	  }

	  private void assertEmpty(System.Func<double[], double> calculator)
	  {
		double[] x = new double[0];
		try
		{
		  calculator(x);
		  Assert.fail();
		}
		catch (System.ArgumentException)
		{
		  // Expected
		}
	  }

	}

}