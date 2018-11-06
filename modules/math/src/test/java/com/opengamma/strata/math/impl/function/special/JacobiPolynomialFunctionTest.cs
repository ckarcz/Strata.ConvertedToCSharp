/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class JacobiPolynomialFunctionTest
	public class JacobiPolynomialFunctionTest
	{
	  private const double ALPHA = 0.12;
	  private const double BETA = 0.34;
	  private static readonly DoubleFunction1D P0 = x => 1d;
	  private static readonly DoubleFunction1D P1 = x => 0.5 * (2 * (ALPHA + 1) + (ALPHA + BETA + 2) * (x - 1));
	  private static readonly DoubleFunction1D P2 = x => 0.125 * (4 * (ALPHA + 1) * (ALPHA + 2) + 4 * (ALPHA + BETA + 3) * (ALPHA + 2) * (x - 1) + (ALPHA + BETA + 3) * (ALPHA + BETA + 4) * (x - 1) * (x - 1));
	  private static readonly DoubleFunction1D[] P = new DoubleFunction1D[] {P0, P1, P2};
	  private static readonly JacobiPolynomialFunction JACOBI = new JacobiPolynomialFunction();
	  private const double EPS = 1e-9;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testNoAlphaBeta()
	  public virtual void testNoAlphaBeta()
	  {
		JACOBI.getPolynomials(3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeN()
	  public virtual void testNegativeN()
	  {
		JACOBI.getPolynomials(-3, ALPHA, BETA);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testGetPolynomials()
	  public virtual void testGetPolynomials()
	  {
		JACOBI.getPolynomialsAndFirstDerivative(3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		DoubleFunction1D[] p = JACOBI.getPolynomials(0, ALPHA, BETA);
		assertEquals(p.Length, 1);
		const double x = 1.23;
		assertEquals(p[0].applyAsDouble(x), 1, EPS);
		for (int i = 0; i <= 2; i++)
		{
		  p = JACOBI.getPolynomials(i, ALPHA, BETA);
		  for (int j = 0; j <= i; j++)
		  {
			assertEquals(P[j].applyAsDouble(x), p[j].applyAsDouble(x), EPS);
		  }
		}
	  }
	}

}