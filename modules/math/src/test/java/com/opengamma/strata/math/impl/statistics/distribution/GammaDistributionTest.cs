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
//ORIGINAL LINE: @Test public class GammaDistributionTest extends ProbabilityDistributionTestCase
	public class GammaDistributionTest : ProbabilityDistributionTestCase
	{
	  private const double K = 1;
	  private const double THETA = 0.5;
	  private static readonly GammaDistribution DIST = new GammaDistribution(K, THETA, ENGINE);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeK1()
	  public virtual void testNegativeK1()
	  {
		new GammaDistribution(-1, 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeK2()
	  public virtual void testNegativeK2()
	  {
		new GammaDistribution(-1, 1, ENGINE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeTheta1()
	  public virtual void testNegativeTheta1()
	  {
		new GammaDistribution(1, -1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeTheta2()
	  public virtual void testNegativeTheta2()
	  {
		new GammaDistribution(1, -1, ENGINE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullEngine()
	  public virtual void testNullEngine()
	  {
		new GammaDistribution(1, 1, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertCDFWithNull(DIST);
		assertPDFWithNull(DIST);
		assertEquals(K, DIST.K, 0);
		assertEquals(THETA, DIST.Theta, 0);
		GammaDistribution other = new GammaDistribution(K, THETA, ENGINE);
		assertEquals(DIST, other);
		assertEquals(DIST.GetHashCode(), other.GetHashCode());
		other = new GammaDistribution(K, THETA);
		assertEquals(DIST, other);
		assertEquals(DIST.GetHashCode(), other.GetHashCode());
		other = new GammaDistribution(K + 1, THETA);
		assertFalse(other.Equals(DIST));
		other = new GammaDistribution(K, THETA + 1);
		assertFalse(other.Equals(DIST));
	  }
	}

}