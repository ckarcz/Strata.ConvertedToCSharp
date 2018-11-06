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
//ORIGINAL LINE: @Test public class GaussLegendreWeightAndAbscissaFunctionTest extends WeightAndAbscissaFunctionTestCase
	public class GaussLegendreWeightAndAbscissaFunctionTest : WeightAndAbscissaFunctionTestCase
	{
	  private static readonly double[] X2 = new double[] {-Math.Sqrt(3) / 3.0, Math.Sqrt(3) / 3.0};
	  private static readonly double[] W2 = new double[] {1, 1};
	  private static readonly double[] X3 = new double[] {-Math.Sqrt(15) / 5.0, 0, Math.Sqrt(15) / 5.0};
	  private static readonly double[] W3 = new double[] {5.0 / 9, 8.0 / 9, 5.0 / 9};
	  private static readonly double[] X4 = new double[] {-Math.Sqrt(525 + 70 * Math.Sqrt(30)) / 35.0, -Math.Sqrt(525 - 70 * Math.Sqrt(30)) / 35.0, Math.Sqrt(525 - 70 * Math.Sqrt(30)) / 35.0, Math.Sqrt(525 + 70 * Math.Sqrt(30)) / 35.0};
	  private static readonly double[] W4 = new double[] {(18 - Math.Sqrt(30)) / 36.0, (18 + Math.Sqrt(30)) / 36.0, (18 + Math.Sqrt(30)) / 36.0, (18 - Math.Sqrt(30)) / 36.0};
	  private static readonly double[] X5 = new double[] {-Math.Sqrt(245 + 14 * Math.Sqrt(70)) / 21.0, -Math.Sqrt(245 - 14 * Math.Sqrt(70)) / 21.0, 0, Math.Sqrt(245 - 14 * Math.Sqrt(70)) / 21.0, Math.Sqrt(245 + 14 * Math.Sqrt(70)) / 21.0};
	  private static readonly double[] W5 = new double[] {(322 - 13 * Math.Sqrt(70)) / 900.0, (322 + 13 * Math.Sqrt(70)) / 900.0, 128.0 / 225, (322 + 13 * Math.Sqrt(70)) / 900.0, (322 - 13 * Math.Sqrt(70)) / 900.0};
	  private static readonly QuadratureWeightAndAbscissaFunction F = new GaussLegendreWeightAndAbscissaFunction();

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