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

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class StudentTDistributionTest extends ProbabilityDistributionTestCase
	public class StudentTDistributionTest : ProbabilityDistributionTestCase
	{

	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private static readonly double[] X = new double[] {0.32492, 0.270722, 0.717558, 1.372184, 1.36343, 1.770933, 2.13145, 2.55238, 2.80734, 3.6896};
	  private static readonly double[] DOF = new double[] {1, 4, 6, 10, 11, 13, 15, 18, 23, 27};
	  private static readonly double[] P = new double[] {0.6, 0.6, 0.75, 0.9, 0.9, 0.95, 0.975, 0.99, 0.995, 0.9995};

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeDOF1()
	  public virtual void testNegativeDOF1()
	  {
		new StudentTDistribution(-2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeDOF2()
	  public virtual void testNegativeDOF2()
	  {
		new StudentTDistribution(-2, ENGINE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullEngine()
	  public virtual void testNullEngine()
	  {
		new StudentTDistribution(2, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		ProbabilityDistribution<double> dist = new StudentTDistribution(1, ENGINE);
		assertCDFWithNull(dist);
		assertPDFWithNull(dist);
		assertInverseCDF(X, dist);
		for (int i = 0; i < 10; i++)
		{
		  dist = new StudentTDistribution(DOF[i], ENGINE);
		  assertEquals(P[i], dist.getCDF(X[i]), EPS);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNormal()
	  public virtual void testNormal()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ProbabilityDistribution<double> highDOF = new StudentTDistribution(1000000, ENGINE);
		ProbabilityDistribution<double> highDOF = new StudentTDistribution(1000000, ENGINE);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ProbabilityDistribution<double> normal = new NormalDistribution(0, 1, ENGINE);
		ProbabilityDistribution<double> normal = new NormalDistribution(0, 1, ENGINE);
		const double eps = 1e-4;
		double x;
		for (int i = 0; i < 100; i++)
		{
		  x = RANDOM.NextDouble();
		  assertEquals(highDOF.getCDF(x), normal.getCDF(x), eps);
		  assertEquals(highDOF.getPDF(x), normal.getPDF(x), eps);
		  assertEquals(highDOF.getInverseCDF(x), normal.getInverseCDF(x), eps);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testObject()
	  public virtual void testObject()
	  {
		const double dof = 2.4;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final StudentTDistribution dist = new StudentTDistribution(dof, ENGINE);
		StudentTDistribution dist = new StudentTDistribution(dof, ENGINE);
		StudentTDistribution other = new StudentTDistribution(dof, ENGINE);
		assertEquals(dist, other);
		assertEquals(dist.GetHashCode(), other.GetHashCode());
		other = new StudentTDistribution(dof);
		assertEquals(dist, other);
		assertEquals(dist.GetHashCode(), other.GetHashCode());
		other = new StudentTDistribution(dof + 1, ENGINE);
		assertFalse(dist.Equals(other));
	  }
	}

}