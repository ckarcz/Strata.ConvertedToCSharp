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
	using BlackScholesFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackScholesFormulaRepository;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Test <seealso cref="EuropeanVanillaOptionFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EuropeanVanillaOptionFunctionTest
	public class EuropeanVanillaOptionFunctionTest
	{

	  private const double STRIKE = 130d;
	  private const double TIME_TO_EXPIRY = 0.257;
	  private const int NUM = 35;

	  public virtual void test_of()
	  {
		EuropeanVanillaOptionFunction test = EuropeanVanillaOptionFunction.of(STRIKE, TIME_TO_EXPIRY, PutCall.PUT, NUM);
		assertEquals(test.Sign, -1d);
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.TimeToExpiry, TIME_TO_EXPIRY);
		assertEquals(test.NumberOfSteps, NUM);
	  }

	  public virtual void test_optionPrice()
	  {
		double tol = 1.0e-12;
		EuropeanVanillaOptionFunction test = EuropeanVanillaOptionFunction.of(STRIKE, TIME_TO_EXPIRY, PutCall.PUT, NUM);
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
		for (int i = 0; i < expectedSize; ++i)
		{
		  double price = spot * Math.Pow(u, 0.5 * i) * Math.Pow(d, NUM - 0.5 * i);
		  double expectedPayoff = Math.Max(STRIKE - price, 0d);
		  assertEquals(computedPayoff.get(i), expectedPayoff, tol);
		}
		// test getNextOptionValues
		double df = 0.92;
		int n = 2;
		DoubleArray values = DoubleArray.of(1.4, 0.9, 0.1, 0.05, 0.0, 0.0, 0.0);
		DoubleArray computedNextValues = test.getNextOptionValues(df, up, mp, dp, values, spot, d, m, n);
		DoubleArray expectedNextValues = DoubleArray.of(df * (1.4 * dp + 0.9 * mp + 0.1 * up), df * (0.9 * dp + 0.1 * mp + 0.05 * up), df * (0.1 * dp + 0.05 * mp), df * 0.05 * dp, 0.0);
		assertTrue(DoubleArrayMath.fuzzyEquals(computedNextValues.toArray(), expectedNextValues.toArray(), tol));
	  }

	  private static readonly TrinomialTree TRINOMIAL_TREE = new TrinomialTree();
	  private const double SPOT = 105.0;
	  private static readonly double[] STRIKES = new double[] {81.0, 97.0, 105.0, 105.1, 114.0, 128.0};
	  private const double TIME = 1.25;
	  private static readonly double[] INTERESTS = new double[] {-0.01, 0.0, 0.05};
	  private static readonly double[] VOLS = new double[] {0.05, 0.1, 0.5};
	  private static readonly double[] DIVIDENDS = new double[] {0.0, 0.02};

	  public virtual void test_trinomialTree()
	  {
		int nSteps = 135;
		LatticeSpecification[] lattices = new LatticeSpecification[]
		{
			new CoxRossRubinsteinLatticeSpecification(),
			new TrigeorgisLatticeSpecification()
		};
		double tol = 5.0e-3;
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
				  OptionFunction function = EuropeanVanillaOptionFunction.of(strike, TIME, PutCall.ofPut(!isCall), nSteps);
				  double exact = BlackScholesFormulaRepository.price(SPOT, strike, TIME, vol, interest, interest - dividend, isCall);
				  foreach (LatticeSpecification lattice in lattices)
				  {
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

}