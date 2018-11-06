using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GeometricMeanCalculatorTest
	public class GeometricMeanCalculatorTest
	{
	  private static readonly System.Func<double[], double> ARITHMETIC = new MeanCalculator();
	  private static readonly System.Func<double[], double> GEOMETRIC = new GeometricMeanCalculator();
	  private const int N = 100;
	  private static readonly double[] FLAT = new double[N];
	  private static readonly double[] X = new double[N];
	  private static readonly double[] LN_X = new double[N];

	  static GeometricMeanCalculatorTest()
	  {
		for (int i = 0; i < N; i++)
		{
		  FLAT[i] = 2;
		  X[i] = GlobalRandom.NextDouble;
		  LN_X[i] = Math.Log(X[i]);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullArray()
	  public virtual void testNullArray()
	  {
		GEOMETRIC.apply(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testEmptyArray()
	  public virtual void testEmptyArray()
	  {
		GEOMETRIC.apply(new double[0]);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(GEOMETRIC.apply(FLAT), 2, 0);
		assertEquals(GEOMETRIC.apply(X), Math.Exp(ARITHMETIC.apply(LN_X)), 1e-15);
	  }
	}

}