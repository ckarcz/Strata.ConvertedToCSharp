/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PercentileCalculatorTest
	public class PercentileCalculatorTest
	{

	  private static readonly PercentileCalculator CALCULATOR = new PercentileCalculator(0.1);
	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private const int N = 100;
	  private static readonly double[] X = new double[N];

	  static PercentileCalculatorTest()
	  {
		for (int i = 0; i < N; i++)
		{
		  X[i] = RANDOM.NextDouble();
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testHighPercentile()
	  public virtual void testHighPercentile()
	  {
		new PercentileCalculator(1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testLowPercentile()
	  public virtual void testLowPercentile()
	  {
		new PercentileCalculator(0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSetHighPercentile()
	  public virtual void testSetHighPercentile()
	  {
		CALCULATOR.Percentile = 1;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSetLowPercentile()
	  public virtual void testSetLowPercentile()
	  {
		CALCULATOR.Percentile = 0;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullArray()
	  public virtual void testNullArray()
	  {
		CALCULATOR.apply((double[]) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testEmptyArray()
	  public virtual void testEmptyArray()
	  {
		CALCULATOR.apply(new double[0]);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testExtremes()
	  public virtual void testExtremes()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] y = java.util.Arrays.copyOf(X, X.length);
		double[] y = Arrays.copyOf(X, X.Length);
		Arrays.sort(y);
		CALCULATOR.Percentile = 1e-15;
		assertEquals(CALCULATOR.apply(X), y[0], 0);
		CALCULATOR.Percentile = 1 - 1e-15;
		assertEquals(CALCULATOR.apply(X), y[N - 1], 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertResult(X, 10);
		assertResult(X, 99);
		assertResult(X, 50);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertResult(final double[] x, final int percentile)
	  private void assertResult(double[] x, int percentile)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] copy = java.util.Arrays.copyOf(x, N);
		double[] copy = Arrays.copyOf(x, N);
		Arrays.sort(copy);
		int count = 0;
		CALCULATOR.Percentile = ((double) percentile) / N;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = CALCULATOR.apply(x);
		double value = CALCULATOR.apply(x).Value;
		while (copy[count++] < value)
		{
		  //intended
		}
		assertEquals(count - 1, percentile);
	  }
	}

}