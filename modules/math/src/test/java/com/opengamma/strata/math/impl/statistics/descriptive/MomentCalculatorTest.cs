using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Assert = org.testng.Assert;
	using Test = org.testng.annotations.Test;

	using MersenneTwister = com.opengamma.strata.math.impl.cern.MersenneTwister;
	using MersenneTwister64 = com.opengamma.strata.math.impl.cern.MersenneTwister64;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;
	using ChiSquareDistribution = com.opengamma.strata.math.impl.statistics.distribution.ChiSquareDistribution;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;
	using ProbabilityDistribution = com.opengamma.strata.math.impl.statistics.distribution.ProbabilityDistribution;
	using StudentTDistribution = com.opengamma.strata.math.impl.statistics.distribution.StudentTDistribution;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MomentCalculatorTest
	public class MomentCalculatorTest
	{

	  private const double STD = 2.0;
	  private const double DOF = 10;
	  private static readonly System.Func<double[], double> SAMPLE_VARIANCE = new SampleVarianceCalculator();
	  private static readonly System.Func<double[], double> POPULATION_VARIANCE = new PopulationVarianceCalculator();
	  private static readonly System.Func<double[], double> SAMPLE_STD = new SampleStandardDeviationCalculator();
	  private static readonly System.Func<double[], double> POPULATION_STD = new PopulationStandardDeviationCalculator();
	  private static readonly System.Func<double[], double> SAMPLE_SKEWNESS = new SampleSkewnessCalculator();
	  private static readonly System.Func<double[], double> SAMPLE_FISHER_KURTOSIS = new SampleFisherKurtosisCalculator();
	  private static readonly RandomEngine ENGINE = new MersenneTwister64(MersenneTwister.DEFAULT_SEED);
	  private static readonly ProbabilityDistribution<double> NORMAL = new NormalDistribution(0, STD, ENGINE);
	  private static readonly ProbabilityDistribution<double> STUDENT_T = new StudentTDistribution(DOF, ENGINE);
	  private static readonly ProbabilityDistribution<double> CHI_SQ = new ChiSquareDistribution(DOF, ENGINE);
	  private static readonly double[] NORMAL_DATA = new double[500000];
	  private static readonly double[] STUDENT_T_DATA = new double[500000];
	  private static readonly double[] CHI_SQ_DATA = new double[500000];
	  private const double EPS = 0.1;
	  static MomentCalculatorTest()
	  {
		for (int i = 0; i < 500000; i++)
		{
		  NORMAL_DATA[i] = NORMAL.nextRandom();
		  STUDENT_T_DATA[i] = STUDENT_T.nextRandom();
		  CHI_SQ_DATA[i] = CHI_SQ.nextRandom();
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNull()
	  public virtual void testNull()
	  {
		assertNullArg(SAMPLE_VARIANCE);
		assertNullArg(SAMPLE_STD);
		assertNullArg(POPULATION_VARIANCE);
		assertNullArg(POPULATION_STD);
		assertNullArg(SAMPLE_SKEWNESS);
		assertNullArg(SAMPLE_FISHER_KURTOSIS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testInsufficientData()
	  public virtual void testInsufficientData()
	  {
		assertInsufficientData(SAMPLE_VARIANCE);
		assertInsufficientData(SAMPLE_STD);
		assertInsufficientData(POPULATION_VARIANCE);
		assertInsufficientData(POPULATION_STD);
		assertInsufficientData(SAMPLE_SKEWNESS);
		assertInsufficientData(SAMPLE_FISHER_KURTOSIS);
	  }

	  private void assertNullArg(System.Func<double[], double> f)
	  {
		try
		{
		  f((double[]) null);
		  Assert.fail();
		}
		catch (System.ArgumentException)
		{
		  // Expected
		}
	  }

	  private void assertInsufficientData(System.Func<double[], double> f)
	  {
		try
		{
		  f(new double[] {1.0});
		  Assert.fail();
		}
		catch (System.ArgumentException)
		{
		  // Expected
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNormal()
	  public virtual void testNormal()
	  {
		assertEquals(SAMPLE_VARIANCE.apply(NORMAL_DATA), STD * STD, EPS);
		assertEquals(POPULATION_VARIANCE.apply(NORMAL_DATA), STD * STD, EPS);
		assertEquals(SAMPLE_STD.apply(NORMAL_DATA), STD, EPS);
		assertEquals(POPULATION_STD.apply(NORMAL_DATA), STD, EPS);
		assertEquals(SAMPLE_SKEWNESS.apply(NORMAL_DATA), 0.0, EPS);
		assertEquals(SAMPLE_FISHER_KURTOSIS.apply(NORMAL_DATA), 0.0, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testStudentT()
	  public virtual void testStudentT()
	  {
		double variance = DOF / (DOF - 2);
		assertEquals(SAMPLE_VARIANCE.apply(STUDENT_T_DATA), variance, EPS);
		assertEquals(POPULATION_VARIANCE.apply(STUDENT_T_DATA), variance, EPS);
		assertEquals(SAMPLE_STD.apply(STUDENT_T_DATA), Math.Sqrt(variance), EPS);
		assertEquals(POPULATION_STD.apply(STUDENT_T_DATA), Math.Sqrt(variance), EPS);
		assertEquals(SAMPLE_SKEWNESS.apply(STUDENT_T_DATA), 0.0, EPS);
		assertEquals(SAMPLE_FISHER_KURTOSIS.apply(STUDENT_T_DATA), 6 / (DOF - 4), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testChiSq()
	  public virtual void testChiSq()
	  {
		double variance = 2 * DOF;
		assertEquals(SAMPLE_VARIANCE.apply(CHI_SQ_DATA), variance, EPS);
		assertEquals(POPULATION_VARIANCE.apply(CHI_SQ_DATA), variance, EPS);
		assertEquals(SAMPLE_STD.apply(CHI_SQ_DATA), Math.Sqrt(variance), EPS);
		assertEquals(POPULATION_STD.apply(CHI_SQ_DATA), Math.Sqrt(variance), EPS);
		assertEquals(SAMPLE_SKEWNESS.apply(CHI_SQ_DATA), Math.Sqrt(8 / DOF), EPS);
		assertEquals(SAMPLE_FISHER_KURTOSIS.apply(CHI_SQ_DATA), 12 / DOF, EPS);
	  }

	}

}