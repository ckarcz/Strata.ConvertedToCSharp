using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using TestHelper = com.opengamma.strata.collect.TestHelper;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;
	using KnockType = com.opengamma.strata.product.option.KnockType;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Test <seealso cref="BlackOneTouchCashPriceFormulaRepository"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackOneTouchCashPriceFormulaRepositoryTest
	public class BlackOneTouchCashPriceFormulaRepositoryTest
	{
	  private static readonly ZonedDateTime REFERENCE_DATE = TestHelper.dateUtc(2011, 7, 1);
	  private static readonly ZonedDateTime EXPIRY_DATE = TestHelper.dateUtc(2015, 1, 2);
	  private static readonly double EXPIRY_TIME = DayCounts.ACT_ACT_ISDA.relativeYearFraction(REFERENCE_DATE.toLocalDate(), EXPIRY_DATE.toLocalDate());
	  private static readonly SimpleConstantContinuousBarrier BARRIER_DOWN_IN = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, 90);
	  private static readonly SimpleConstantContinuousBarrier BARRIER_DOWN_OUT = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, 90);
	  private static readonly SimpleConstantContinuousBarrier BARRIER_UP_IN = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, 110);
	  private static readonly SimpleConstantContinuousBarrier BARRIER_UP_OUT = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, 110);
	  private static readonly SimpleConstantContinuousBarrier[] BARRIERS = new SimpleConstantContinuousBarrier[] {BARRIER_UP_IN, BARRIER_UP_OUT, BARRIER_DOWN_IN, BARRIER_DOWN_OUT};
	  private const double SPOT = 105;
	  private const double RATE_DOM = 0.05; // Domestic rate
	  private const double RATE_FOR = 0.02; // Foreign rate
	  private static readonly double COST_OF_CARRY = RATE_DOM - RATE_FOR; // Domestic - Foreign rate
	  private const double VOLATILITY = 0.20;
	  private static readonly double DF_DOM = Math.Exp(-RATE_DOM * EXPIRY_TIME);

	  private const double TOL = 1.0e-14;
	  private const double EPS_FD = 1.0e-6;
	  private static readonly BlackOneTouchCashPriceFormulaRepository PRICER = new BlackOneTouchCashPriceFormulaRepository();

	  /// <summary>
	  /// standard in-out parity holds if r=0.
	  /// </summary>
	  public virtual void inOutParity()
	  {
		double upIn = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, 0d, VOLATILITY, BARRIER_UP_IN);
		double upOut = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, 0d, VOLATILITY, BARRIER_UP_OUT);
		double downIn = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, 0d, VOLATILITY, BARRIER_DOWN_IN);
		double downOut = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, 0d, VOLATILITY, BARRIER_DOWN_OUT);
		assertRelative(upIn + upOut, 1d);
		assertRelative(downIn + downOut, 1d);
	  }

	  /// <summary>
	  /// Upper barrier level is very high.
	  /// </summary>
	  public virtual void largeBarrierTest()
	  {
		SimpleConstantContinuousBarrier @in = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, 1.0e4);
		SimpleConstantContinuousBarrier @out = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, 1.0e4);
		double upIn = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, @in);
		double upOut = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, @out);
		assertRelative(upIn, 0d);
		assertRelative(upOut, DF_DOM);
	  }

	  /// <summary>
	  /// Lower barrier level is very small.
	  /// </summary>
	  public virtual void smallBarrierTest()
	  {
		SimpleConstantContinuousBarrier @in = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, 0.1d);
		SimpleConstantContinuousBarrier @out = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, 0.1d);
		double dwIn = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, @in);
		double dwOut = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, @out);
		assertRelative(dwIn, 0d);
		assertRelative(dwOut, DF_DOM);
	  }

	  /// <summary>
	  /// Greeks against finite difference approximation.
	  /// </summary>
	  public virtual void greekfdTest()
	  {
		foreach (SimpleConstantContinuousBarrier barrier in BARRIERS)
		{
		  ValueDerivatives computed = PRICER.priceAdjoint(SPOT, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, barrier);
		  double spotUp = PRICER.price(SPOT + EPS_FD, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, barrier);
		  double spotDw = PRICER.price(SPOT - EPS_FD, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, barrier);
		  double rateUp = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM + EPS_FD, VOLATILITY, barrier);
		  double rateDw = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM - EPS_FD, VOLATILITY, barrier);
		  double costUp = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY + EPS_FD, RATE_DOM, VOLATILITY, barrier);
		  double costDw = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY - EPS_FD, RATE_DOM, VOLATILITY, barrier);
		  double volUp = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY + EPS_FD, barrier);
		  double volDw = PRICER.price(SPOT, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY - EPS_FD, barrier);
		  double timeUp = PRICER.price(SPOT, EXPIRY_TIME + EPS_FD, COST_OF_CARRY, RATE_DOM, VOLATILITY, barrier);
		  double timeDw = PRICER.price(SPOT, EXPIRY_TIME - EPS_FD, COST_OF_CARRY, RATE_DOM, VOLATILITY, barrier);
		  ValueDerivatives spotUp1 = PRICER.priceAdjoint(SPOT + EPS_FD, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, barrier);
		  ValueDerivatives spotDw1 = PRICER.priceAdjoint(SPOT - EPS_FD, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, barrier);
		  assertEquals(computed.getDerivative(0), 0.5 * (spotUp - spotDw) / EPS_FD, EPS_FD);
		  assertEquals(computed.getDerivative(1), 0.5 * (rateUp - rateDw) / EPS_FD, EPS_FD);
		  assertEquals(computed.getDerivative(2), 0.5 * (costUp - costDw) / EPS_FD, EPS_FD);
		  assertEquals(computed.getDerivative(3), 0.5 * (volUp - volDw) / EPS_FD, EPS_FD);
		  assertEquals(computed.getDerivative(4), 0.5 * (timeUp - timeDw) / EPS_FD, EPS_FD);
		  assertEquals(computed.getDerivative(5), 0.5 * (spotUp1.getDerivative(0) - spotDw1.getDerivative(0)) / EPS_FD, EPS_FD);
		}
	  }

	  /// <summary>
	  /// smoothly connected to limiting cases.
	  /// </summary>
	  public virtual void smallsigmaTTest()
	  {
		foreach (SimpleConstantContinuousBarrier barrier in BARRIERS)
		{
		  double volUp = 2.0e-3;
		  double volDw = 1.0e-3;
		  double time = 1.0e-2;
		  double optUp = PRICER.price(SPOT, time, COST_OF_CARRY, RATE_DOM, volUp, barrier);
		  double optDw = PRICER.price(SPOT, time, COST_OF_CARRY, RATE_DOM, volDw, barrier);
		  assertRelative(optUp, optDw);
		  ValueDerivatives optUpAdj = PRICER.priceAdjoint(SPOT, time, COST_OF_CARRY, RATE_DOM, volUp, barrier);
		  ValueDerivatives optDwAdj = PRICER.priceAdjoint(SPOT, time, COST_OF_CARRY, RATE_DOM, volDw, barrier);
		  assertRelative(optUpAdj.Value, optDwAdj.Value);
		  for (int i = 0; i < 6; ++i)
		  {
			assertRelative(optUpAdj.getDerivative(i), optDwAdj.getDerivative(i));
		  }
		}
	  }

	  /// <summary>
	  /// Barrier event has occured already.
	  /// </summary>
	  public virtual void illegalBarrierLevelTest()
	  {
		assertThrowsIllegalArg(() => PRICER.price(BARRIER_UP_IN.BarrierLevel + 0.1, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, BARRIER_UP_IN));
		assertThrowsIllegalArg(() => PRICER.price(BARRIER_DOWN_OUT.BarrierLevel - 0.1, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, BARRIER_DOWN_OUT));
		assertThrowsIllegalArg(() => PRICER.priceAdjoint(BARRIER_UP_IN.BarrierLevel + 0.1, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, BARRIER_UP_IN));
		assertThrowsIllegalArg(() => PRICER.priceAdjoint(BARRIER_DOWN_OUT.BarrierLevel - 0.1, EXPIRY_TIME, COST_OF_CARRY, RATE_DOM, VOLATILITY, BARRIER_DOWN_OUT));
	  }

	  //-------------------------------------------------------------------------
	  private void assertRelative(double val1, double val2)
	  {
		assertEquals(val1, val2, Math.Max(Math.Abs(val2), 1d) * TOL);
	  }
	}

}