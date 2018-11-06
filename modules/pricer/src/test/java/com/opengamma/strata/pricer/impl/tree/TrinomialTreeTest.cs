using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.tree
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using RecombiningTrinomialTreeData = com.opengamma.strata.pricer.fxopt.RecombiningTrinomialTreeData;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Test <seealso cref="TrinomialTree"/>.
	/// <para>
	/// Further tests are done for implementations of {@code OptionFunction}. See their test classes.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TrinomialTreeTest
	public class TrinomialTreeTest
	{

	  private static readonly TrinomialTree TRINOMIAL_TREE = new TrinomialTree();
	  private const double SPOT = 105.0;
	  private static readonly double[] STRIKES = new double[] {81.0, 97.0, 105.0, 105.1, 114.0, 128.0};
	  private const double TIME = 1.25;
	  private static readonly double[] INTERESTS = new double[] {-0.01, 0.0, 0.05};
	  private static readonly double[] VOLS = new double[] {0.05, 0.1, 0.5};
	  private static readonly double[] DIVIDENDS = new double[] {0.0, 0.02};

	  /// <summary>
	  /// Test consistency between price methods, and Greek via finite difference.
	  /// </summary>
	  public virtual void test_trinomialTree()
	  {
		int nSteps = 135;
		double dt = TIME / nSteps;
		LatticeSpecification lattice = new CoxRossRubinsteinLatticeSpecification();
		double fdEps = 1.0e-4;
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
				  double[] @params = lattice.getParametersTrinomial(vol, interest - dividend, dt).toArray();
				  DoubleArray time = DoubleArray.of(nSteps + 1, i => dt * i);
				  DoubleArray df = DoubleArray.of(nSteps, i => Math.Exp(-interest * dt));
				  double[][] stateValue = new double[nSteps + 1][];
				  stateValue[0] = new double[] {SPOT};
				  IList<DoubleMatrix> prob = new List<DoubleMatrix>();
				  double[] probs = new double[] {@params[5], @params[4], @params[3]};
				  for (int i = 0; i < nSteps; ++i)
				  {
					int index = i;
					stateValue[i + 1] = DoubleArray.of(2 * i + 3, j => SPOT * Math.Pow(@params[2], index + 1 - j) * Math.Pow(@params[1], j)).toArray();
					double[][] probMatrix = new double[2 * i + 1][];
					Arrays.fill(probMatrix, probs);
					prob.Add(DoubleMatrix.ofUnsafe(probMatrix));
				  }
				  RecombiningTrinomialTreeData treeData = RecombiningTrinomialTreeData.of(DoubleMatrix.ofUnsafe(stateValue), prob, df, time);
				  double priceData = TRINOMIAL_TREE.optionPrice(function, treeData);
				  double priceParams = TRINOMIAL_TREE.optionPrice(function, lattice, SPOT, vol, interest, dividend);
				  assertEquals(priceData, priceParams);
				  ValueDerivatives priceDeriv = TRINOMIAL_TREE.optionPriceAdjoint(function, treeData);
				  assertEquals(priceDeriv.Value, priceData);
				  double priceUp = TRINOMIAL_TREE.optionPrice(function, lattice, SPOT + fdEps, vol, interest, dividend);
				  double priceDw = TRINOMIAL_TREE.optionPrice(function, lattice, SPOT - fdEps, vol, interest, dividend);
				  double fdDelta = 0.5 * (priceUp - priceDw) / fdEps;
				  assertEquals(priceDeriv.getDerivative(0), fdDelta, 3.0e-2);
				}
			  }
			}
		  }
		}
	  }

	}

}