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
//ORIGINAL LINE: @Test public class ChiSquareDistributionTest extends ProbabilityDistributionTestCase
	public class ChiSquareDistributionTest : ProbabilityDistributionTestCase
	{
	  private static readonly double[] X = new double[] {1.9, 5.8, 9.0, 15.5, 39};
	  private static readonly double[] DOF = new double[] {3, 6, 7, 16, 28};
	  private static readonly double[] Q = new double[] {0.59342, 0.44596, 0.25266, 0.48837, 0.08092};
	  private static readonly ChiSquareDistribution DIST = new ChiSquareDistribution(1, ENGINE);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeDOF1()
	  public virtual void testNegativeDOF1()
	  {
		new ChiSquareDistribution(-2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeDOF2()
	  public virtual void testNegativeDOF2()
	  {
		new ChiSquareDistribution(-2, ENGINE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullEngine()
	  public virtual void testNullEngine()
	  {
		new ChiSquareDistribution(2, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testObject()
	  public virtual void testObject()
	  {
		assertEquals(1, DIST.DegreesOfFreedom, 0);
		ChiSquareDistribution other = new ChiSquareDistribution(1);
		assertEquals(DIST, other);
		assertEquals(DIST.GetHashCode(), other.GetHashCode());
		other = new ChiSquareDistribution(1, ENGINE);
		assertEquals(DIST, other);
		assertEquals(DIST.GetHashCode(), other.GetHashCode());
		other = new ChiSquareDistribution(2);
		assertFalse(other.Equals(DIST));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertCDFWithNull(DIST);
		assertPDFWithNull(DIST);
		assertInverseCDFWithNull(DIST);
		ChiSquareDistribution dist;
		for (int i = 0; i < 5; i++)
		{
		  dist = new ChiSquareDistribution(DOF[i], ENGINE);
		  assertEquals(1 - dist.getCDF(X[i]), Q[i], EPS);
		  assertEquals(dist.getInverseCDF(dist.getCDF(X[i])), X[i], EPS);
		}
	  }
	}

}