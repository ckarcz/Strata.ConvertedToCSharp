/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BivariateNormalDistributionTest
	public class BivariateNormalDistributionTest
	{
	  private static readonly ProbabilityDistribution<double[]> DIST = new BivariateNormalDistribution();
	  private const double EPS = 1e-8;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullCDF()
	  public virtual void testNullCDF()
	  {
		DIST.getCDF(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testInsufficientLengthCDF()
	  public virtual void testInsufficientLengthCDF()
	  {
		DIST.getCDF(new double[] {2, 1});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testExcessiveLengthCDF()
	  public virtual void testExcessiveLengthCDF()
	  {
		DIST.getCDF(new double[] {2, 1, 4, 5});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testHighCorrelation()
	  public virtual void testHighCorrelation()
	  {
		DIST.getCDF(new double[] {1.0, 1.0, 3.0});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testLowCorrelation()
	  public virtual void testLowCorrelation()
	  {
		DIST.getCDF(new double[] {1.0, 1.0, -3.0});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(DIST.getCDF(new double[] {double.PositiveInfinity, GlobalRandom.NextDouble, GlobalRandom.NextDouble}), 1, 0);
		assertEquals(DIST.getCDF(new double[] {GlobalRandom.NextDouble, double.PositiveInfinity, GlobalRandom.NextDouble}), 1, 0);
		assertEquals(DIST.getCDF(new double[] {double.NegativeInfinity, GlobalRandom.NextDouble, GlobalRandom.NextDouble}), 0, 0);
		assertEquals(DIST.getCDF(new double[] {GlobalRandom.NextDouble, double.NegativeInfinity, GlobalRandom.NextDouble}), 0, 0);
		assertEquals(DIST.getCDF(new double[] {0.0, 0.0, 0.0}), 0.25, EPS);
		assertEquals(DIST.getCDF(new double[] {0.0, 0.0, -0.5}), 1.0 / 6, EPS);
		assertEquals(DIST.getCDF(new double[] {0.0, 0.0, 0.5}), 1.0 / 3, EPS);

		assertEquals(DIST.getCDF(new double[] {0.0, -0.5, 0.0}), 0.1542687694, EPS);
		assertEquals(DIST.getCDF(new double[] {0.0, -0.5, -0.5}), 0.0816597607, EPS);
		assertEquals(DIST.getCDF(new double[] {0.0, -0.5, 0.5}), 0.2268777781, EPS);

		assertEquals(DIST.getCDF(new double[] {0.0, 0.5, 0.0}), 0.3457312306, EPS);
		assertEquals(DIST.getCDF(new double[] {0.0, 0.5, -0.5}), 0.2731222219, EPS);
		assertEquals(DIST.getCDF(new double[] {0.0, 0.5, 0.5}), 0.4183402393, EPS);

		assertEquals(DIST.getCDF(new double[] {-0.5, 0.0, 0.0}), 0.1542687694, EPS);
		assertEquals(DIST.getCDF(new double[] {-0.5, 0.0, -0.5}), 0.0816597607, EPS);
		assertEquals(DIST.getCDF(new double[] {-0.5, 0.0, 0.5}), 0.2268777781, EPS);

		assertEquals(DIST.getCDF(new double[] {-0.5, -0.5, 0.0}), 0.0951954128, EPS);
		assertEquals(DIST.getCDF(new double[] {-0.5, -0.5, -0.5}), 0.0362981865, EPS);
		assertEquals(DIST.getCDF(new double[] {-0.5, -0.5, 0.5}), 0.1633195213, EPS);

		assertEquals(DIST.getCDF(new double[] {-0.5, 0.5, 0.0}), 0.2133421259, EPS);
		assertEquals(DIST.getCDF(new double[] {-0.5, 0.5, -0.5}), 0.1452180174, EPS);
		assertEquals(DIST.getCDF(new double[] {-0.5, 0.5, 0.5}), 0.2722393522, EPS);

		assertEquals(DIST.getCDF(new double[] {0.5, 0.0, 0.0}), 0.3457312306, EPS);
		assertEquals(DIST.getCDF(new double[] {0.5, 0.0, -0.5}), 0.2731222219, EPS);
		assertEquals(DIST.getCDF(new double[] {0.5, 0.0, 0.5}), 0.4183402393, EPS);

		assertEquals(DIST.getCDF(new double[] {0.5, -0.5, 0.0}), 0.2133421259, EPS);
		assertEquals(DIST.getCDF(new double[] {0.5, -0.5, -0.5}), 0.1452180174, EPS);
		assertEquals(DIST.getCDF(new double[] {0.5, -0.5, 0.5}), 0.2722393522, EPS);

		assertEquals(DIST.getCDF(new double[] {0.5, 0.5, 0.0}), 0.4781203354, EPS);
		assertEquals(DIST.getCDF(new double[] {0.5, 0.5, -0.5}), 0.4192231090, EPS);
		assertEquals(DIST.getCDF(new double[] {0.0, -1.0, -1.0}), 0.00000000, EPS);
	  }
	}

}