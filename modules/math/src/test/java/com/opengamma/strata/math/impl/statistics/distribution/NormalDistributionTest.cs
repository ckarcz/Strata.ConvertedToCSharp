/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertFalse;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalDistributionTest extends ProbabilityDistributionTestCase
	public class NormalDistributionTest : ProbabilityDistributionTestCase
	{
	  private static readonly ProbabilityDistribution<double> NORMAL = new NormalDistribution(0, 1, ENGINE);
	  private static readonly double[] X = new double[] {0, 0.1, 0.4, 0.8, 1, 1.32, 1.78, 2, 2.36, 2.88, 3, 3.5, 4, 4.5, 5};
	  private static readonly double[] P = new double[] {0.50000, 0.53982, 0.65542, 0.78814, 0.84134, 0.90658, 0.96246, 0.97724, 0.99086, 0.99801, 0.99865, 0.99976, 0.99996, 0.99999, 0.99999};
	  private static readonly double[] Z = new double[] {0.39894, 0.39695, 0.36827, 0.28969, 0.24197, 0.16693, 0.08182, 0.05399, 0.02463, 0.00630, 4.43184e-3, 8.72682e-4, 1.3383e-4, 1.59837e-5, 1.48671e-6};

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeSigmaDistribution()
	  public virtual void testNegativeSigmaDistribution()
	  {
		new NormalDistribution(1, -0.4);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullEngine()
	  public virtual void testNullEngine()
	  {
		new NormalDistribution(0, 1, null);
	  }

	  public virtual void test()
	  {
		assertCDF(P, X, NORMAL);
		assertPDF(Z, X, NORMAL);
		assertInverseCDF(X, NORMAL);
	  }

	  public virtual void testRoundTrip()
	  {
		int n = 29;
		for (int i = 0; i < n; i++)
		{
		  double x = -7.0 + 0.5 * i;
		  double p = NORMAL.getCDF(x);
		  double xStar = NORMAL.getInverseCDF(p);
		  assertEquals(x, xStar, 1e-5);
		}
	  }

	  public virtual void testObject()
	  {
		NormalDistribution other = new NormalDistribution(0, 1, ENGINE);
		assertEquals(NORMAL, other);
		assertEquals(NORMAL.GetHashCode(), other.GetHashCode());
		other = new NormalDistribution(0, 1);
		assertEquals(NORMAL, other);
		assertEquals(NORMAL.GetHashCode(), other.GetHashCode());
		other = new NormalDistribution(0.1, 1, ENGINE);
		assertFalse(NORMAL.Equals(other));
		other = new NormalDistribution(0, 1.1, ENGINE);
		assertFalse(NORMAL.Equals(other));
	  }
	}

}