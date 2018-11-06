using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.smile
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using FiniteDifferenceType = com.opengamma.strata.math.impl.differentiation.FiniteDifferenceType;
	using ScalarFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.ScalarFieldFirstOrderDifferentiator;

	/// <summary>
	/// Test <seealso cref="SsviVolatilityFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SsviVolatilityFunctionTest
	public class SsviVolatilityFunctionTest
	{

	  private const double VOL_ATM = 0.20;
	  private const double RHO = -0.25;
	  private const double ETA = 0.50;
	  private static readonly SsviFormulaData DATA = SsviFormulaData.of(VOL_ATM, RHO, ETA);
	  private const double TIME_EXP = 2.5;
	  private const double FORWARD = 0.05;
	  private const int N = 10;
	  private static readonly double[] STRIKES = new double[N];
	  static SsviVolatilityFunctionTest()
	  {
		for (int i = 0; i < N; i++)
		{
		  STRIKES[i] = FORWARD - 0.03 + (i * 0.05 / N);
		}
	  }
	  private static readonly SsviVolatilityFunction SSVI_FUNCTION = SsviVolatilityFunction.DEFAULT;

	  private const double TOLERANCE_VOL = 1.0E-10;
	  private const double TOLERANCE_AD = 1.0E-6;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void volatility()
	  public virtual void volatility()
	  { // Function versus local implementation of formula
		double theta = VOL_ATM * VOL_ATM * TIME_EXP;
		double phi = ETA / Math.Sqrt(theta);
		for (int i = 0; i < N; i++)
		{
		  double k = Math.Log(STRIKES[i] / FORWARD);
		  double w = 0.5 * theta * (1.0d + RHO * phi * k + Math.Sqrt(Math.Pow(phi * k + RHO, 2) + (1.0d - RHO * RHO)));
		  double sigmaExpected = Math.Sqrt(w / TIME_EXP);
		  double sigmaComputed = SSVI_FUNCTION.volatility(FORWARD, STRIKES[i], TIME_EXP, DATA);
		  assertEquals(sigmaExpected, sigmaComputed, TOLERANCE_VOL);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void derivatives()
	  public virtual void derivatives()
	  { // AD v Finite Difference
		ScalarFieldFirstOrderDifferentiator differentiator = new ScalarFieldFirstOrderDifferentiator(FiniteDifferenceType.CENTRAL, 1.0E-5);
		for (int i = 0; i < N; i++)
		{
		  System.Func<DoubleArray, double> function = (DoubleArray x) =>
		  {
	  SsviFormulaData data = SsviFormulaData.of(x.get(3), x.get(4), x.get(5));
	  return SSVI_FUNCTION.volatility(x.get(0), x.get(1), x.get(2), data);
		  };
		  System.Func<DoubleArray, DoubleArray> d = differentiator.differentiate(function);
		  DoubleArray fd = d(DoubleArray.of(FORWARD, STRIKES[i], TIME_EXP, VOL_ATM, RHO, ETA));
		  ValueDerivatives ad = SSVI_FUNCTION.volatilityAdjoint(FORWARD, STRIKES[i], TIME_EXP, DATA);
		  for (int j = 0; j < 6; j++)
		  {
			assertEquals(fd.get(j), ad.Derivatives.get(j), TOLERANCE_AD);
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test_small_time()
	  public virtual void test_small_time()
	  {
		assertThrowsIllegalArg(() => SSVI_FUNCTION.volatility(FORWARD, STRIKES[0], 0.0, DATA));
	  }

	  public virtual void coverage()
	  {
		coverImmutableBean(SSVI_FUNCTION);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(SSVI_FUNCTION);
	  }

	}

}