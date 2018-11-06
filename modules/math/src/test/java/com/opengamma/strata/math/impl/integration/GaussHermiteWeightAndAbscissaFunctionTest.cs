using System;

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
//ORIGINAL LINE: @Test public class GaussHermiteWeightAndAbscissaFunctionTest extends WeightAndAbscissaFunctionTestCase
	public class GaussHermiteWeightAndAbscissaFunctionTest : WeightAndAbscissaFunctionTestCase
	{
	  private static readonly double SQRT_PI = Math.Sqrt(Math.PI);
	  private static readonly double DENOM1 = 4 * (3 - Math.Sqrt(6));
	  private static readonly double DENOM2 = 4 * (3 + Math.Sqrt(6));
	  private static readonly double[] X2 = new double[] {-Math.Sqrt(2) / 2.0, Math.Sqrt(2) / 2.0};
	  private static readonly double[] W2 = new double[] {SQRT_PI / 2.0, SQRT_PI / 2.0};
	  private static readonly double[] X3 = new double[] {-Math.Sqrt(6) / 2.0, 0, Math.Sqrt(6) / 2.0};
	  private static readonly double[] W3 = new double[] {SQRT_PI / 6.0, 2 * SQRT_PI / 3.0, SQRT_PI / 6.0};
	  private static readonly double[] X4 = new double[] {-Math.Sqrt((3 + Math.Sqrt(6)) / 2.0), -Math.Sqrt((3 - Math.Sqrt(6)) / 2.0), Math.Sqrt((3 - Math.Sqrt(6)) / 2.0), Math.Sqrt((3 + Math.Sqrt(6)) / 2.0)};
	  private static readonly double[] W4 = new double[] {SQRT_PI / DENOM2, SQRT_PI / DENOM1, SQRT_PI / DENOM1, SQRT_PI / DENOM2};
	  private static readonly QuadratureWeightAndAbscissaFunction F = new GaussHermiteWeightAndAbscissaFunction();

	  protected internal override QuadratureWeightAndAbscissaFunction Function
	  {
		  get
		  {
			return F;
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertResults(F.generate(2), X2, W2);
		assertResults(F.generate(3), X3, W3);
		assertResults(F.generate(4), X4, W4);
	  }
	}

}