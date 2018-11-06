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

	using Assert = org.testng.Assert;
	using Test = org.testng.annotations.Test;

	using MeanCalculator = com.opengamma.strata.math.impl.statistics.descriptive.MeanCalculator;
	using MedianCalculator = com.opengamma.strata.math.impl.statistics.descriptive.MedianCalculator;
	using PopulationVarianceCalculator = com.opengamma.strata.math.impl.statistics.descriptive.PopulationVarianceCalculator;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GeneralizedParetoDistributionTest extends ProbabilityDistributionTestCase
	public class GeneralizedParetoDistributionTest : ProbabilityDistributionTestCase
	{
	  private const double MU = 0.4;
	  private const double SIGMA = 1.4;
	  private const double KSI = 0.2;
	  private static readonly GeneralizedParetoDistribution DIST = new GeneralizedParetoDistribution(MU, SIGMA, KSI, ENGINE);
	  private const double LARGE_X = 1e20;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testBadSigma()
	  public virtual void testBadSigma()
	  {
		new GeneralizedParetoDistribution(MU, -SIGMA, KSI);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testZeroKsi()
	  public virtual void testZeroKsi()
	  {
		new GeneralizedParetoDistribution(MU, SIGMA, 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullEngine()
	  public virtual void testNullEngine()
	  {
		new GeneralizedParetoDistribution(MU, SIGMA, KSI, null);
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
		assertEquals(KSI, DIST.Ksi, 0);
		assertEquals(MU, DIST.Mu, 0);
		assertEquals(SIGMA, DIST.Sigma, 0);
		GeneralizedParetoDistribution other = new GeneralizedParetoDistribution(MU, SIGMA, KSI, ENGINE);
		assertEquals(DIST, other);
		assertEquals(DIST.GetHashCode(), other.GetHashCode());
		other = new GeneralizedParetoDistribution(MU, SIGMA, KSI);
		assertEquals(DIST, other);
		assertEquals(DIST.GetHashCode(), other.GetHashCode());
		other = new GeneralizedParetoDistribution(MU + 1, SIGMA, KSI);
		assertFalse(other.Equals(DIST));
		other = new GeneralizedParetoDistribution(MU, SIGMA + 1, KSI);
		assertFalse(other.Equals(DIST));
		other = new GeneralizedParetoDistribution(MU, SIGMA, KSI + 1);
		assertFalse(other.Equals(DIST));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testSupport()
	  public virtual void testSupport()
	  {
		ProbabilityDistribution<double> dist = new GeneralizedParetoDistribution(MU, SIGMA, KSI, ENGINE);
		assertLimit(dist, MU - EPS);
		assertEquals(dist.getCDF(MU + EPS), 0, EPS);
		assertEquals(dist.getCDF(LARGE_X), 1, EPS);
		dist = new GeneralizedParetoDistribution(MU, SIGMA, -KSI);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double limit = MU + SIGMA / KSI;
		double limit = MU + SIGMA / KSI;
		assertLimit(dist, MU - EPS);
		assertLimit(dist, limit + EPS);
		assertEquals(dist.getCDF(MU + EPS), 0, EPS);
		assertEquals(dist.getCDF(limit - 1e-15), 1, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testDistribution()
	  public virtual void testDistribution()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double[], double> meanCalculator = new com.opengamma.strata.math.impl.statistics.descriptive.MeanCalculator();
		System.Func<double[], double> meanCalculator = new MeanCalculator();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double[], double> medianCalculator = new com.opengamma.strata.math.impl.statistics.descriptive.MedianCalculator();
		System.Func<double[], double> medianCalculator = new MedianCalculator();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double[], double> varianceCalculator = new com.opengamma.strata.math.impl.statistics.descriptive.PopulationVarianceCalculator();
		System.Func<double[], double> varianceCalculator = new PopulationVarianceCalculator();
		const int n = 1000000;
		const double eps = 0.1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] data = new double[n];
		double[] data = new double[n];
		for (int i = 0; i < n; i++)
		{
		  data[i] = DIST.nextRandom();
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double mean = MU + SIGMA / (1 - KSI);
		double mean = MU + SIGMA / (1 - KSI);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double median = MU + SIGMA * (Math.pow(2, KSI) - 1) / KSI;
		double median = MU + SIGMA * (Math.Pow(2, KSI) - 1) / KSI;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double variance = SIGMA * SIGMA / ((1 - KSI) * (1 - KSI) * (1 - 2 * KSI));
		double variance = SIGMA * SIGMA / ((1 - KSI) * (1 - KSI) * (1 - 2 * KSI));
		assertEquals(meanCalculator(data), mean, eps);
		assertEquals(medianCalculator(data), median, eps);
		assertEquals(varianceCalculator(data), variance, eps);
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