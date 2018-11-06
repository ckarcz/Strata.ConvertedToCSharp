/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EigenvaluePolynomialRootFinderTest
	public class EigenvaluePolynomialRootFinderTest
	{

	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private static readonly Polynomial1DRootFinder<double> FINDER = new EigenvaluePolynomialRootFinder();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull()
	  public virtual void testNull()
	  {
		FINDER.getRoots(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] r = new double[] {-RANDOM.nextDouble(), -RANDOM.nextDouble(), RANDOM.nextDouble(), RANDOM.nextDouble() };
		double[] r = new double[] {-RANDOM.NextDouble(), -RANDOM.NextDouble(), RANDOM.NextDouble(), RANDOM.NextDouble()};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a0 = r[0] * r[1] * r[2] * r[3];
		double a0 = r[0] * r[1] * r[2] * r[3];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a1 = r[0] * r[1] * r[2] + r[0] * r[1] * r[3] + r[0] * r[2] * r[3] + r[1] * r[2] * r[3];
		double a1 = r[0] * r[1] * r[2] + r[0] * r[1] * r[3] + r[0] * r[2] * r[3] + r[1] * r[2] * r[3];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a2 = r[0] * r[1] + r[0] * r[2] + r[0] * r[3] + r[1] * r[2] + r[1] * r[3] + r[2] * r[3];
		double a2 = r[0] * r[1] + r[0] * r[2] + r[0] * r[3] + r[1] * r[2] + r[1] * r[3] + r[2] * r[3];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a3 = r[0] + r[1] + r[2] + r[3];
		double a3 = r[0] + r[1] + r[2] + r[3];
		const double a4 = 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.RealPolynomialFunction1D f = new com.opengamma.strata.math.impl.function.RealPolynomialFunction1D(new double[] {a0, a1, a2, a3, a4 });
		RealPolynomialFunction1D f = new RealPolynomialFunction1D(new double[] {a0, a1, a2, a3, a4});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final System.Nullable<double>[] roots = FINDER.getRoots(f);
		double?[] roots = FINDER.getRoots(f);
		Arrays.sort(roots);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] expected = new double[r.length];
		double[] expected = new double[r.Length];
		for (int i = 0; i < r.Length; i++)
		{
		  expected[i] = -r[i];
		}
		Arrays.sort(expected);
		for (int i = 0; i < roots.Length; i++)
		{
		  assertEquals(roots[i], expected[i], 1e-12);
		}
	  }
	}

}