/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GaussLaguerreWeightAndAbscissaFunctionTest extends WeightAndAbscissaFunctionTestCase
	public class GaussLaguerreWeightAndAbscissaFunctionTest : WeightAndAbscissaFunctionTestCase
	{
	  private static readonly double[] X2 = new double[] {0.585786, 3.41421};
	  private static readonly double[] W2 = new double[] {0.853553, 0.146447};
	  private static readonly double[] X3 = new double[] {0.415775, 2.29428, 6.28995};
	  private static readonly double[] W3 = new double[] {0.711093, 0.278518, 0.0103893};
	  private static readonly double[] X4 = new double[] {0.322548, 1.74576, 4.53662, 9.39507};
	  private static readonly double[] W4 = new double[] {0.603154, 0.357419, 0.0388879, 0.000539295};
	  private static readonly double[] X5 = new double[] {0.26356, 1.4134, 3.59643, 7.08581, 12.6408};
	  private static readonly double[] W5 = new double[] {0.521756, 0.398667, 0.0759424, 0.00361176, 0.00002337};
	  private static readonly QuadratureWeightAndAbscissaFunction F = new GaussLaguerreWeightAndAbscissaFunction(0);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertResults(F.generate(2), X2, W2);
		assertResults(F.generate(3), X3, W3);
		assertResults(F.generate(4), X4, W4);
		assertResults(F.generate(5), X5, W5);
	  }

	  protected internal override QuadratureWeightAndAbscissaFunction Function
	  {
		  get
		  {
			return F;
		  }
	  }
	}

}