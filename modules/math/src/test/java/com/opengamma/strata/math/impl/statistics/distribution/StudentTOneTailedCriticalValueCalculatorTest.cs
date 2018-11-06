/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class StudentTOneTailedCriticalValueCalculatorTest
	public class StudentTOneTailedCriticalValueCalculatorTest
	{

	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private const double NU = 3;
	  private static readonly System.Func<double, double> F = new StudentTOneTailedCriticalValueCalculator(NU);
	  private static readonly ProbabilityDistribution<double> T = new StudentTDistribution(NU);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNu1()
	  public virtual void testNu1()
	  {
		new StudentTOneTailedCriticalValueCalculator(-3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNu2()
	  public virtual void testNu2()
	  {
		new StudentTOneTailedCriticalValueCalculator(-3, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testEngine()
	  public virtual void testEngine()
	  {
		new StudentTDistribution(3, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull()
	  public virtual void testNull()
	  {
		F.apply((double?) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegative()
	  public virtual void testNegative()
	  {
		F.apply(-4.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		double x;
		const double eps = 1e-5;
		for (int i = 0; i < 100; i++)
		{
		  x = RANDOM.NextDouble();
		  assertEquals(x, F.apply(T.getCDF(x)), eps);
		}
	  }
	}

}