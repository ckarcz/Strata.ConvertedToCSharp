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

	using Assert = org.testng.Assert;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GeneralizedExtremeValueDistributionTest extends ProbabilityDistributionTestCase
	public class GeneralizedExtremeValueDistributionTest : ProbabilityDistributionTestCase
	{
	  private const double MU = 1.5;
	  private const double SIGMA = 0.6;
	  private const double KSI = 0.7;
	  private static readonly GeneralizedExtremeValueDistribution DIST = new GeneralizedExtremeValueDistribution(MU, SIGMA, KSI);
	  private const double LARGE_X = 1e10;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testBadConstructor()
	  public virtual void testBadConstructor()
	  {
		new GeneralizedExtremeValueDistribution(MU, -SIGMA, KSI);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testBadInputs()
	  public virtual void testBadInputs()
	  {
		assertCDFWithNull(DIST);
		assertPDFWithNull(DIST);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testObject()
	  public virtual void testObject()
	  {
		assertEquals(MU, DIST.Mu, 0);
		assertEquals(SIGMA, DIST.Sigma, 0);
		assertEquals(KSI, DIST.Ksi, 0);
		GeneralizedExtremeValueDistribution other = new GeneralizedExtremeValueDistribution(MU, SIGMA, KSI);
		assertEquals(DIST, other);
		assertEquals(DIST.GetHashCode(), other.GetHashCode());
		other = new GeneralizedExtremeValueDistribution(MU + 1, SIGMA, KSI);
		assertFalse(other.Equals(DIST));
		other = new GeneralizedExtremeValueDistribution(MU, SIGMA + 1, KSI);
		assertFalse(other.Equals(DIST));
		other = new GeneralizedExtremeValueDistribution(MU, SIGMA, KSI + 1);
		assertFalse(other.Equals(DIST));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testSupport()
	  public virtual void testSupport()
	  {
		ProbabilityDistribution<double> dist = new GeneralizedExtremeValueDistribution(MU, SIGMA, KSI);
		double limit = MU - SIGMA / KSI;
		assertLimit(dist, limit - EPS);
		assertEquals(dist.getCDF(limit + EPS), 0, EPS);
		assertEquals(dist.getCDF(LARGE_X), 1, EPS);
		dist = new GeneralizedExtremeValueDistribution(MU, SIGMA, -KSI);
		limit = MU + SIGMA / KSI;
		assertLimit(dist, limit + EPS);
		assertEquals(dist.getCDF(-LARGE_X), 0, EPS);
		assertEquals(dist.getCDF(limit - EPS), 1, EPS);
		dist = new GeneralizedExtremeValueDistribution(MU, SIGMA, 0);
		assertEquals(dist.getCDF(-LARGE_X), 0, EPS);
		assertEquals(dist.getCDF(LARGE_X), 1, EPS);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertLimit(final ProbabilityDistribution<double> dist, final double limit)
	  private void assertLimit(ProbabilityDistribution<double> dist, double limit)
	  {
		try
		{
		  dist.getCDF(limit);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		try
		{
		  dist.getPDF(limit);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
	  }
	}

}