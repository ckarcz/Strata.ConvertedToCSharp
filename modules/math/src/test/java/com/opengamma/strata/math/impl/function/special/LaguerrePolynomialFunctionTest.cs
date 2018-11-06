/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;

	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LaguerrePolynomialFunctionTest
	public class LaguerrePolynomialFunctionTest
	{
	  private static readonly DoubleFunction1D L0 = x => 1d;
	  private static readonly DoubleFunction1D L1 = x => 1 - x;
	  private static readonly DoubleFunction1D L2 = x => 0.5 * (x * x - 4 * x + 2);
	  private static readonly DoubleFunction1D L3 = x => (-x * x * x + 9 * x * x - 18 * x + 6) / 6;
	  private static readonly DoubleFunction1D L4 = x => (x * x * x * x - 16 * x * x * x + 72 * x * x - 96 * x + 24) / 24;
	  private static readonly DoubleFunction1D L5 = x => (-x * x * x * x * x + 25 * x * x * x * x - 200 * x * x * x + 600 * x * x - 600 * x + 120) / 120;
	  private static readonly DoubleFunction1D L6 = x => (x * x * x * x * x * x - 36 * x * x * x * x * x + 450 * x * x * x * x - 2400 * x * x * x + 5400 * x * x - 4320 * x + 720) / 720;

	  private static readonly DoubleFunction1D[] L = new DoubleFunction1D[] {L0, L1, L2, L3, L4, L5, L6};
	  private static readonly LaguerrePolynomialFunction LAGUERRE = new LaguerrePolynomialFunction();
	  private const double EPS = 1e-12;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testBadN1()
	  public virtual void testBadN1()
	  {
		LAGUERRE.getPolynomials(-3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testBadN2()
	  public virtual void testBadN2()
	  {
		LAGUERRE.getPolynomials(-3, 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		DoubleFunction1D[] l = LAGUERRE.getPolynomials(0);
		assertEquals(l.Length, 1);
		const double x = 1.23;
		assertEquals(l[0].applyAsDouble(x), 1, EPS);
		l = LAGUERRE.getPolynomials(1);
		assertEquals(l.Length, 2);
		assertEquals(l[1].applyAsDouble(x), 1 - x, EPS);
		for (int i = 0; i <= 6; i++)
		{
		  l = LAGUERRE.getPolynomials(i);
		  for (int j = 0; j <= i; j++)
		  {
			assertEquals(L[j].applyAsDouble(x), l[j].applyAsDouble(x), EPS);
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testAlpha1()
	  public virtual void testAlpha1()
	  {
		DoubleFunction1D[] l1, l2;
		const double x = 2.34;
		for (int i = 0; i <= 6; i++)
		{
		  l1 = LAGUERRE.getPolynomials(i, 0);
		  l2 = LAGUERRE.getPolynomials(i);
		  for (int j = 0; j <= i; j++)
		  {
			assertEquals(l1[j].applyAsDouble(x), l2[j].applyAsDouble(x), EPS);
		  }
		}
		const double alpha = 3.45;
		l1 = LAGUERRE.getPolynomials(6, alpha);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.DoubleFunction1D f0 = d -> 1d;
		DoubleFunction1D f0 = d => 1d;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.DoubleFunction1D f1 = d -> 1 + alpha - d;
		DoubleFunction1D f1 = d => 1 + alpha - d;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.DoubleFunction1D f2 = d -> d * d / 2 - (alpha + 2) * d + (alpha + 2) * (alpha + 1) / 2.0;
		DoubleFunction1D f2 = d => d * d / 2 - (alpha + 2) * d + (alpha + 2) * (alpha + 1) / 2.0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.DoubleFunction1D f3 = d -> -d * d * d / 6 + (alpha + 3) * d * d / 2 - (alpha + 2) * (alpha + 3) * d / 2 + (alpha + 1) * (alpha + 2) * (alpha + 3) / 6;
		DoubleFunction1D f3 = d => -d * d * d / 6 + (alpha + 3) * d * d / 2 - (alpha + 2) * (alpha + 3) * d / 2 + (alpha + 1) * (alpha + 2) * (alpha + 3) / 6;
		assertEquals(l1[0].applyAsDouble(x), f0.applyAsDouble(x), EPS);
		assertEquals(l1[1].applyAsDouble(x), f1.applyAsDouble(x), EPS);
		assertEquals(l1[2].applyAsDouble(x), f2.applyAsDouble(x), EPS);
		assertEquals(l1[3].applyAsDouble(x), f3.applyAsDouble(x), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testAlpha2()
	  public virtual void testAlpha2()
	  {
		const int n = 14;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.math.impl.function.DoubleFunction1D, com.opengamma.strata.math.impl.function.DoubleFunction1D>[] polynomialAndDerivative1 = LAGUERRE.getPolynomialsAndFirstDerivative(n);
		Pair<DoubleFunction1D, DoubleFunction1D>[] polynomialAndDerivative1 = LAGUERRE.getPolynomialsAndFirstDerivative(n);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.math.impl.function.DoubleFunction1D, com.opengamma.strata.math.impl.function.DoubleFunction1D>[] polynomialAndDerivative2 = LAGUERRE.getPolynomialsAndFirstDerivative(n, 0);
		Pair<DoubleFunction1D, DoubleFunction1D>[] polynomialAndDerivative2 = LAGUERRE.getPolynomialsAndFirstDerivative(n, 0);
		for (int i = 0; i < n; i++)
		{
		  assertTrue(polynomialAndDerivative1[i].First is RealPolynomialFunction1D);
		  assertTrue(polynomialAndDerivative2[i].First is RealPolynomialFunction1D);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.RealPolynomialFunction1D first = (com.opengamma.strata.math.impl.function.RealPolynomialFunction1D) polynomialAndDerivative1[i].getFirst();
		  RealPolynomialFunction1D first = (RealPolynomialFunction1D) polynomialAndDerivative1[i].First;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.RealPolynomialFunction1D second = (com.opengamma.strata.math.impl.function.RealPolynomialFunction1D) polynomialAndDerivative2[i].getFirst();
		  RealPolynomialFunction1D second = (RealPolynomialFunction1D) polynomialAndDerivative2[i].First;
		  assertEquals(first, second);
		}
	  }
	}

}