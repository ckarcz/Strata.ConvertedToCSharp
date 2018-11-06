/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IncompleteBetaFunctionTest
	public class IncompleteBetaFunctionTest
	{

	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private const double EPS = 1e-9;
	  private const double A = 0.4;
	  private const double B = 0.2;
	  private const int MAX_ITER = 10000;
	  private static readonly System.Func<double, double> BETA = new IncompleteBetaFunction(A, B);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeA1()
	  public virtual void testNegativeA1()
	  {
		new IncompleteBetaFunction(-A, B);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeA2()
	  public virtual void testNegativeA2()
	  {
		new IncompleteBetaFunction(-A, B, EPS, MAX_ITER);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeB1()
	  public virtual void testNegativeB1()
	  {
		new IncompleteBetaFunction(A, -B);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeB2()
	  public virtual void testNegativeB2()
	  {
		new IncompleteBetaFunction(A, -B, EPS, MAX_ITER);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeEps()
	  public virtual void testNegativeEps()
	  {
		new IncompleteBetaFunction(A, B, -EPS, MAX_ITER);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeIter()
	  public virtual void testNegativeIter()
	  {
		new IncompleteBetaFunction(A, B, EPS, -MAX_ITER);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testLow()
	  public virtual void testLow()
	  {
		BETA.apply(-0.3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testHigh()
	  public virtual void testHigh()
	  {
		BETA.apply(1.5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a = RANDOM.nextDouble();
		double a = RANDOM.NextDouble();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double b = RANDOM.nextDouble();
		double b = RANDOM.NextDouble();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = RANDOM.nextDouble();
		double x = RANDOM.NextDouble();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> f1 = new IncompleteBetaFunction(a, b);
		System.Func<double, double> f1 = new IncompleteBetaFunction(a, b);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> f2 = new IncompleteBetaFunction(b, a);
		System.Func<double, double> f2 = new IncompleteBetaFunction(b, a);
		assertEquals(f1(0.0), 0, EPS);
		assertEquals(f1(1.0), 1, EPS);
		assertEquals(f1(x), 1 - f2(1 - x), EPS);
	  }
	}

}