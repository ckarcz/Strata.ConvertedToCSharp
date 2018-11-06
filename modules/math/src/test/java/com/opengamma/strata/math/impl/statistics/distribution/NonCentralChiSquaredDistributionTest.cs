using System;

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
//ORIGINAL LINE: @Test public class NonCentralChiSquaredDistributionTest
	public class NonCentralChiSquaredDistributionTest
	{
	  private const double DOF = 3;
	  private const double NON_CENTRALITY = 1.5;
	  private static readonly NonCentralChiSquaredDistribution DIST = new NonCentralChiSquaredDistribution(DOF, NON_CENTRALITY);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeDOF()
	  public virtual void testNegativeDOF()
	  {
		new NonCentralChiSquaredDistribution(-DOF, NON_CENTRALITY);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeNonCentrality()
	  public virtual void testNegativeNonCentrality()
	  {
		new NonCentralChiSquaredDistribution(DOF, -NON_CENTRALITY);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullX()
	  public virtual void testNullX()
	  {
		DIST.getCDF(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testInverseCDF()
	  public virtual void testInverseCDF()
	  {
		DIST.getInverseCDF(0.5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testPDF()
	  public virtual void testPDF()
	  {
		DIST.getPDF(0.5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testRandom()
	  public virtual void testRandom()
	  {
		DIST.nextRandom();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(DIST.Degrees, DOF, 0);
		assertEquals(DIST.NonCentrality, NON_CENTRALITY, 0);
		assertEquals(DIST.getCDF(-100.0), 0, 0);
		assertEquals(DIST.getCDF(0.0), 0, 0);
		assertEquals(DIST.getCDF(5.0), 0.649285, 1e-6);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testObject()
	  public virtual void testObject()
	  {
		assertEquals(DIST.Degrees, DOF, 0);
		assertEquals(DIST.NonCentrality, NON_CENTRALITY, 0);
		NonCentralChiSquaredDistribution other = new NonCentralChiSquaredDistribution(DOF, NON_CENTRALITY);
		assertEquals(DIST, other);
		assertEquals(DIST.GetHashCode(), other.GetHashCode());
		other = new NonCentralChiSquaredDistribution(DOF + 1, NON_CENTRALITY);
		assertFalse(other.Equals(DIST));
		other = new NonCentralChiSquaredDistribution(DOF, NON_CENTRALITY + 1);
		assertFalse(other.Equals(DIST));
	  }

	  /// <summary>
	  /// Numbers computed from R
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testLargeValues()
	  public virtual void testLargeValues()
	  {
		double x = 123;
		double dof = 6.4;
		double nonCent = 100.34;
		NonCentralChiSquaredDistribution dist = new NonCentralChiSquaredDistribution(dof, nonCent);
		assertEquals(0.7930769, dist.getCDF(x), 1e-6);

		x = 455.038;
		dof = 12;
		nonCent = 444.44;

		dist = new NonCentralChiSquaredDistribution(dof, nonCent);
		assertEquals(0.4961805, dist.getCDF(x), 1e-6);

		x = 999400;
		dof = 500;
		nonCent = 1000000;
		dist = new NonCentralChiSquaredDistribution(dof, nonCent);
		assertEquals(0.2913029, dist.getCDF(x), 1e-6);

	  }

	  /// <summary>
	  /// Numbers computed from R
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void debugTest()
	  public virtual void debugTest()
	  {
		const double dof = 3.666;
		const double nonCentrality = 75;
		const double x = 13.89;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final NonCentralChiSquaredDistribution chiSq1 = new NonCentralChiSquaredDistribution(dof, nonCentrality);
		NonCentralChiSquaredDistribution chiSq1 = new NonCentralChiSquaredDistribution(dof, nonCentrality);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double y1 = Math.log(chiSq1.getCDF(x));
		double y1 = Math.Log(chiSq1.getCDF(x));
		assertEquals(-15.92129, y1, 1e-5);
	  }

	}

}