/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Assert = org.testng.Assert;
	using Test = org.testng.annotations.Test;

	using MersenneTwister = com.opengamma.strata.math.impl.cern.MersenneTwister;
	using MersenneTwister64 = com.opengamma.strata.math.impl.cern.MersenneTwister64;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;

	/// <summary>
	/// Abstract test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class ProbabilityDistributionTestCase
	public abstract class ProbabilityDistributionTestCase
	{

	  protected internal const double EPS = 1e-5;
	  protected internal static readonly RandomEngine ENGINE = new MersenneTwister64(MersenneTwister.DEFAULT_SEED);

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertCDF(final double[] p, final double[] x, final ProbabilityDistribution<double> dist)
	  protected internal virtual void assertCDF(double[] p, double[] x, ProbabilityDistribution<double> dist)
	  {
		assertCDFWithNull(dist);
		for (int i = 0; i < p.Length; i++)
		{
		  assertEquals(dist.getCDF(x[i]), p[i], EPS);
		}
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertPDF(final double[] z, final double[] x, final ProbabilityDistribution<double> dist)
	  protected internal virtual void assertPDF(double[] z, double[] x, ProbabilityDistribution<double> dist)
	  {
		assertPDFWithNull(dist);
		for (int i = 0; i < z.Length; i++)
		{
		  assertEquals(dist.getPDF(x[i]), z[i], EPS);
		}
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertInverseCDF(final double[] x, final ProbabilityDistribution<double> dist)
	  protected internal virtual void assertInverseCDF(double[] x, ProbabilityDistribution<double> dist)
	  {
		assertInverseCDFWithNull(dist);
		foreach (double d in x)
		{
		  assertEquals(dist.getInverseCDF(dist.getCDF(d)), d, EPS);
		}
		try
		{
		  dist.getInverseCDF(3.4);
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
		  dist.getInverseCDF(-0.2);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertInverseCDFWithNull(final ProbabilityDistribution<double> dist)
	  protected internal virtual void assertInverseCDFWithNull(ProbabilityDistribution<double> dist)
	  {
		try
		{
		  dist.getInverseCDF(null);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertPDFWithNull(final ProbabilityDistribution<double> dist)
	  protected internal virtual void assertPDFWithNull(ProbabilityDistribution<double> dist)
	  {
		try
		{
		  dist.getPDF(null);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertCDFWithNull(final ProbabilityDistribution<double> dist)
	  protected internal virtual void assertCDFWithNull(ProbabilityDistribution<double> dist)
	  {
		try
		{
		  dist.getCDF(null);
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