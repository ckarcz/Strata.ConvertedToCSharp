using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.tree
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using BlackBarrierPriceFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackBarrierPriceFormulaRepository;
	using BlackOneTouchCashPriceFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackOneTouchCashPriceFormulaRepository;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;
	using KnockType = com.opengamma.strata.product.option.KnockType;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Test <seealso cref="ConstantContinuousSingleBarrierKnockoutFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ConstantContinuousSingleBarrierKnockoutFunctionTest
	public class ConstantContinuousSingleBarrierKnockoutFunctionTest
	{

	  private const double STRIKE = 130d;
	  private const double TIME_TO_EXPIRY = 0.257;
	  private const int NUM = 35;
	  private const double BARRIER = 140d;
	  private const double REBATE_AMOUNT = 5d;
	  private static readonly DoubleArray REBATE = DoubleArray.of(NUM + 1, i => REBATE_AMOUNT);

	  public virtual void test_of()
	  {
		ConstantContinuousSingleBarrierKnockoutFunction test = ConstantContinuousSingleBarrierKnockoutFunction.of(STRIKE, TIME_TO_EXPIRY, PutCall.PUT, NUM, BarrierType.UP, BARRIER, REBATE);
		assertEquals(test.Sign, -1d);
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.TimeToExpiry, TIME_TO_EXPIRY);
		assertEquals(test.NumberOfSteps, NUM);
		assertEquals(test.BarrierLevel, BARRIER);
		assertEquals(test.getBarrierLevel(23), BARRIER);
		assertEquals(test.BarrierType, BarrierType.UP);
		assertEquals(test.Rebate, REBATE);
		assertEquals(test.getRebate(14), REBATE_AMOUNT);
	  }

	  public virtual void test_optionPrice_up()
	  {
		double tol = 1.0e-12;
		ConstantContinuousSingleBarrierKnockoutFunction test = ConstantContinuousSingleBarrierKnockoutFunction.of(STRIKE, TIME_TO_EXPIRY, PutCall.PUT, NUM, BarrierType.UP, BARRIER, REBATE);
		double spot = 130d;
		double u = 1.05;
		double d = 0.98;
		double m = Math.Sqrt(u * d);
		double up = 0.29;
		double dp = 0.25;
		double mp = 1d - up - dp;
		// test getPayoffAtExpiryTrinomial
		DoubleArray computedPayoff = test.getPayoffAtExpiryTrinomial(spot, d, m);
		int expectedSize = 2 * NUM + 1;
		assertEquals(computedPayoff.size(), expectedSize);
		double[] price = new double[expectedSize];
		for (int i = 0; i < expectedSize; ++i)
		{
		  price[i] = spot * Math.Pow(u, 0.5 * i) * Math.Pow(d, NUM - 0.5 * i);
		}
		for (int i = 0; i < expectedSize; ++i)
		{
		  double expectedPayoff = price[i] < BARRIER ? Math.Max(STRIKE - price[i], 0d) : REBATE_AMOUNT;
		  if (i != expectedSize - 1 && price[i] < BARRIER && price[i + 1] > BARRIER)
		  {
			expectedPayoff = 0.5 * ((BARRIER - price[i]) * expectedPayoff + (price[i + 1] - BARRIER) * REBATE_AMOUNT) / (price[i + 1] - price[i]) + 0.5 * expectedPayoff;
		  }
		  assertEquals(computedPayoff.get(i), expectedPayoff, tol);
		}
		// test getNextOptionValues
		double df = 0.92;
		int n = 2;
		DoubleArray values = DoubleArray.of(1.4, 0.9, 0.1, 0.05, 0.0, 0.0, 0.0);
		DoubleArray computedNextValues = test.getNextOptionValues(df, up, mp, dp, values, spot, d, m, n);
		double tmp = df * 0.05 * dp;
		DoubleArray expectedNextValues = DoubleArray.of(df * (1.4 * dp + 0.9 * mp + 0.1 * up), df * (0.9 * dp + 0.1 * mp + 0.05 * up), df * (0.1 * dp + 0.05 * mp), 0.5 * ((BARRIER / spot - u * m) * tmp + (u * u - BARRIER / spot) * REBATE_AMOUNT) / (u * u - u * m) + 0.5 * tmp, REBATE_AMOUNT);
		assertTrue(DoubleArrayMath.fuzzyEquals(computedNextValues.toArray(), expectedNextValues.toArray(), tol));
	  }

	  public virtual void test_optionPrice_down()
	  {
		double tol = 1.0e-12;
		double barrier = 97d;
		ConstantContinuousSingleBarrierKnockoutFunction test = ConstantContinuousSingleBarrierKnockoutFunction.of(STRIKE, TIME_TO_EXPIRY, PutCall.CALL, NUM, BarrierType.DOWN, barrier, REBATE);
		double spot = 100d;
		double u = 1.05;
		double d = 0.98;
		double m = Math.Sqrt(u * d);
		double up = 0.29;
		double dp = 0.25;
		double mp = 1d - up - dp;
		// test getPayoffAtExpiryTrinomial
		DoubleArray computedPayoff = test.getPayoffAtExpiryTrinomial(spot, d, m);
		int expectedSize = 2 * NUM + 1;
		assertEquals(computedPayoff.size(), expectedSize);
		double[] price = new double[expectedSize];
		for (int i = 0; i < expectedSize; ++i)
		{
		  price[i] = spot * Math.Pow(u, 0.5 * i) * Math.Pow(d, NUM - 0.5 * i);
		}
		for (int i = 0; i < expectedSize; ++i)
		{
		  double expectedPayoff = price[i] > barrier ? Math.Max(price[i] - STRIKE, 0d) : REBATE_AMOUNT;
		  if (i != 0 && price[i - 1] < barrier && price[i] > barrier)
		  {
			expectedPayoff = 0.5 * (expectedPayoff * (price[i] - barrier) + REBATE_AMOUNT * (barrier - price[i - 1])) / (price[i] - price[i - 1]) + 0.5 * expectedPayoff;
		  }
		  assertEquals(computedPayoff.get(i), expectedPayoff, tol);
		}
		// test getNextOptionValues
		double df = 0.92;
		int n = 2;
		DoubleArray values = DoubleArray.of(1.4, 0.9, 0.1, 0.05, 0.0, 0.0, 0.0);
		DoubleArray computedNextValues = test.getNextOptionValues(df, up, mp, dp, values, spot, d, m, n);
		double tmp = df * (0.9 * dp + 0.1 * mp + 0.05 * up);
		DoubleArray expectedNextValues = DoubleArray.of(REBATE_AMOUNT, 0.5 * (tmp * (m * d - barrier / spot) + REBATE_AMOUNT * (barrier / spot - d * d)) / (m * d - d * d) + 0.5 * tmp, df * (0.1 * dp + 0.05 * mp), df * 0.05 * dp, 0.0);
		assertTrue(DoubleArrayMath.fuzzyEquals(computedNextValues.toArray(), expectedNextValues.toArray(), tol));
	  }

	  private static readonly TrinomialTree TRINOMIAL_TREE = new TrinomialTree();
	  private const double SPOT = 105.0;
	  private static readonly double[] STRIKES = new double[] {81d, 97d, 105d, 105.1, 114d, 128d};
	  private const double TIME = 1.25;
	  private static readonly double[] INTERESTS = new double[] {-0.01, 0.0, 0.05};
	  private static readonly double[] VOLS = new double[] {0.05, 0.1, 0.25};
	  private static readonly double[] DIVIDENDS = new double[] {0.0, 0.02};

	  private static readonly BlackBarrierPriceFormulaRepository BARRIER_PRICER = new BlackBarrierPriceFormulaRepository();
	  private static readonly BlackOneTouchCashPriceFormulaRepository REBATE_PRICER = new BlackOneTouchCashPriceFormulaRepository();

	  public virtual void test_trinomialTree_up()
	  {
		int nSteps = 133;
		LatticeSpecification lattice = new CoxRossRubinsteinLatticeSpecification();
		DoubleArray rebate = DoubleArray.of(nSteps + 1, i => REBATE_AMOUNT);
		double barrierLevel = 135d;
		double tol = 1.0e-2;
		foreach (bool isCall in new bool[] {true, false})
		{
		  foreach (double strike in STRIKES)
		  {
			foreach (double interest in INTERESTS)
			{
			  foreach (double vol in VOLS)
			  {
				foreach (double dividend in DIVIDENDS)
				{
				  OptionFunction function = ConstantContinuousSingleBarrierKnockoutFunction.of(strike, TIME, PutCall.ofPut(!isCall), nSteps, BarrierType.UP, barrierLevel, rebate);
				  SimpleConstantContinuousBarrier barrier = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, barrierLevel);
				  double exact = REBATE_AMOUNT * REBATE_PRICER.price(SPOT, TIME, interest - dividend, interest, vol, barrier.inverseKnockType()) + BARRIER_PRICER.price(SPOT, strike, TIME, interest - dividend, interest, vol, isCall, barrier);
				  double computed = TRINOMIAL_TREE.optionPrice(function, lattice, SPOT, vol, interest, dividend);
				  assertEquals(computed, exact, Math.Max(exact, 1d) * tol);
				}
			  }
			}
		  }
		}
	  }

	  public virtual void test_trinomialTree_down()
	  {
		int nSteps = 133;
		LatticeSpecification lattice = new CoxRossRubinsteinLatticeSpecification();
		DoubleArray rebate = DoubleArray.of(nSteps + 1, i => REBATE_AMOUNT);
		double barrierLevel = 76d;
		double tol = 1.0e-2;
		foreach (bool isCall in new bool[] {true, false})
		{
		  foreach (double strike in STRIKES)
		  {
			foreach (double interest in INTERESTS)
			{
			  foreach (double vol in VOLS)
			  {
				foreach (double dividend in DIVIDENDS)
				{
				  OptionFunction function = ConstantContinuousSingleBarrierKnockoutFunction.of(strike, TIME, PutCall.ofPut(!isCall), nSteps, BarrierType.DOWN, barrierLevel, rebate);
				  SimpleConstantContinuousBarrier barrier = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, barrierLevel);
				  double exact = REBATE_AMOUNT * REBATE_PRICER.price(SPOT, TIME, interest - dividend, interest, vol, barrier.inverseKnockType()) + BARRIER_PRICER.price(SPOT, strike, TIME, interest - dividend, interest, vol, isCall, barrier);
				  double computed = TRINOMIAL_TREE.optionPrice(function, lattice, SPOT, vol, interest, dividend);
				  assertEquals(computed, exact, Math.Max(exact, 1d) * tol);
				}
			  }
			}
		  }
		}
	  }

	}

}