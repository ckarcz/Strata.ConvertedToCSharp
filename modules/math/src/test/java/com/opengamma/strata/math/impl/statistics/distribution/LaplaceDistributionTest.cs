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

	using MeanCalculator = com.opengamma.strata.math.impl.statistics.descriptive.MeanCalculator;
	using MedianCalculator = com.opengamma.strata.math.impl.statistics.descriptive.MedianCalculator;
	using SampleFisherKurtosisCalculator = com.opengamma.strata.math.impl.statistics.descriptive.SampleFisherKurtosisCalculator;
	using SampleSkewnessCalculator = com.opengamma.strata.math.impl.statistics.descriptive.SampleSkewnessCalculator;
	using SampleVarianceCalculator = com.opengamma.strata.math.impl.statistics.descriptive.SampleVarianceCalculator;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LaplaceDistributionTest extends ProbabilityDistributionTestCase
	public class LaplaceDistributionTest : ProbabilityDistributionTestCase
	{
	  private const double MU = 0.7;
	  private const double B = 0.5;
	  private static readonly LaplaceDistribution LAPLACE = new LaplaceDistribution(MU, B, ENGINE);
	  private static readonly double[] DATA;
	  private const double EPS1 = 0.05;
	  static LaplaceDistributionTest()
	  {
		const int n = 1000000;
		DATA = new double[n];
		for (int i = 0; i < n; i++)
		{
		  DATA[i] = LAPLACE.nextRandom();
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeBDistribution()
	  public virtual void testNegativeBDistribution()
	  {
		new LaplaceDistribution(1, -0.4);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullEngine()
	  public virtual void testNullEngine()
	  {
		new LaplaceDistribution(0, 1, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testInverseCDFWithLow()
	  public virtual void testInverseCDFWithLow()
	  {
		LAPLACE.getInverseCDF(-0.45);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testInverseCDFWithHigh()
	  public virtual void testInverseCDFWithHigh()
	  {
		LAPLACE.getInverseCDF(6.7);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testObject()
	  public virtual void testObject()
	  {
		assertEquals(LAPLACE.B, B, 0);
		assertEquals(LAPLACE.Mu, MU, 0);
		LaplaceDistribution other = new LaplaceDistribution(MU, B);
		assertEquals(LAPLACE, other);
		assertEquals(LAPLACE.GetHashCode(), other.GetHashCode());
		other = new LaplaceDistribution(MU + 1, B);
		assertFalse(LAPLACE.Equals(other));
		other = new LaplaceDistribution(MU, B + 1);
		assertFalse(LAPLACE.Equals(other));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertCDFWithNull(LAPLACE);
		assertPDFWithNull(LAPLACE);
		assertInverseCDFWithNull(LAPLACE);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double mean = new com.opengamma.strata.math.impl.statistics.descriptive.MeanCalculator().apply(DATA);
		double mean = (new MeanCalculator()).apply(DATA).Value;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double median = new com.opengamma.strata.math.impl.statistics.descriptive.MedianCalculator().apply(DATA);
		double median = (new MedianCalculator()).apply(DATA).Value;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double variance = new com.opengamma.strata.math.impl.statistics.descriptive.SampleVarianceCalculator().apply(DATA);
		double variance = (new SampleVarianceCalculator()).apply(DATA).Value;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double skew = new com.opengamma.strata.math.impl.statistics.descriptive.SampleSkewnessCalculator().apply(DATA);
		double skew = (new SampleSkewnessCalculator()).apply(DATA).Value;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double kurtosis = new com.opengamma.strata.math.impl.statistics.descriptive.SampleFisherKurtosisCalculator().apply(DATA);
		double kurtosis = (new SampleFisherKurtosisCalculator()).apply(DATA).Value;
		assertEquals(mean, MU, EPS1);
		assertEquals(median, MU, EPS1);
		assertEquals(variance, 2 * B * B, EPS1);
		assertEquals(skew, 0, EPS1);
		assertEquals(kurtosis, 3, EPS1);
	  }
	}

}