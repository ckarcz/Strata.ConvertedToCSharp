/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using MersenneTwister = com.opengamma.strata.math.impl.cern.MersenneTwister;
	using MersenneTwister64 = com.opengamma.strata.math.impl.cern.MersenneTwister64;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;
	using ProbabilityDistribution = com.opengamma.strata.math.impl.statistics.distribution.ProbabilityDistribution;

	/// <summary>
	/// Abstract test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class ParameterLimitsTransformTestCase
	public abstract class ParameterLimitsTransformTestCase
	{

	  protected internal static readonly RandomEngine RANDOM = new MersenneTwister64(MersenneTwister.DEFAULT_SEED);
	  protected internal static readonly ProbabilityDistribution<double> NORMAL = new NormalDistribution(0, 1, RANDOM);

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertRoundTrip(final ParameterLimitsTransform transform, final double modelParam)
	  protected internal virtual void assertRoundTrip(ParameterLimitsTransform transform, double modelParam)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fp = transform.transform(modelParam);
		double fp = transform.transform(modelParam);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double mp = transform.inverseTransform(fp);
		double mp = transform.inverseTransform(fp);
		assertEquals(modelParam, mp, 1e-8);
	  }

	  // reverse
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertReverseRoundTrip(final ParameterLimitsTransform transform, final double fitParam)
	  protected internal virtual void assertReverseRoundTrip(ParameterLimitsTransform transform, double fitParam)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double mp = transform.inverseTransform(fitParam);
		double mp = transform.inverseTransform(fitParam);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fp = transform.transform(mp);
		double fp = transform.transform(mp);
		assertEquals(fitParam, fp, 1e-8);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertGradientRoundTrip(final ParameterLimitsTransform transform, final double modelParam)
	  protected internal virtual void assertGradientRoundTrip(ParameterLimitsTransform transform, double modelParam)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double g = transform.transformGradient(modelParam);
		double g = transform.transformGradient(modelParam);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fp = transform.transform(modelParam);
		double fp = transform.transform(modelParam);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double gInv = transform.inverseTransformGradient(fp);
		double gInv = transform.inverseTransformGradient(fp);
		assertEquals(g, 1.0 / gInv, 1e-8);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertGradient(final ParameterLimitsTransform transform, final double modelParam)
	  protected internal virtual void assertGradient(ParameterLimitsTransform transform, double modelParam)
	  {
		const double eps = 1e-5;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double g = transform.transformGradient(modelParam);
		double g = transform.transformGradient(modelParam);
		double fdg;
		try
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double down = transform.transform(modelParam - eps);
		  double down = transform.transform(modelParam - eps);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double up = transform.transform(modelParam + eps);
		  double up = transform.transform(modelParam + eps);
		  fdg = (up - down) / 2 / eps;
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double fp = transform.transform(modelParam);
		  double fp = transform.transform(modelParam);
		  try
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double up = transform.transform(modelParam + eps);
			double up = transform.transform(modelParam + eps);
			fdg = (up - fp) / eps;
		  }
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e2)
		  catch (legalArgumentException)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double down = transform.transform(modelParam - eps);
			double down = transform.transform(modelParam - eps);
			fdg = (fp - down) / eps;
		  }
		}
		assertEquals(g, fdg, 1e-6);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertInverseGradient(final ParameterLimitsTransform transform, final double fitParam)
	  protected internal virtual void assertInverseGradient(ParameterLimitsTransform transform, double fitParam)
	  {
		const double eps = 1e-5;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double g = transform.inverseTransformGradient(fitParam);
		double g = transform.inverseTransformGradient(fitParam);
		double fdg;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double down = transform.inverseTransform(fitParam - eps);
		double down = transform.inverseTransform(fitParam - eps);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double up = transform.inverseTransform(fitParam + eps);
		double up = transform.inverseTransform(fitParam + eps);
		fdg = (up - down) / 2 / eps;

		assertEquals(g, fdg, 1e-6);
	  }

	}

}