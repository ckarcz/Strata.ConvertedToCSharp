using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.leastsquare
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.interpolation.PenaltyMatrixGenerator.getPenaltyMatrix;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;

	/// <summary>
	/// Test <seealso cref="NonLinearLeastSquareWithPenalty"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NonLinearLeastSquareWithPenaltyTest
	public class NonLinearLeastSquareWithPenaltyTest
	{

	  private static readonly MatrixAlgebra MA = new CommonsMatrixAlgebra();

	  private static NonLinearLeastSquareWithPenalty NLLSWP = new NonLinearLeastSquareWithPenalty();
	  internal static int N_SWAPS = 8;

	  public virtual void linearTest()
	  {
		bool print = false;
		if (print)
		{
		  Console.WriteLine("NonLinearLeastSquareWithPenaltyTest.linearTest");
		}
		int nWeights = 20;
		int diffOrder = 2;
		double lambda = 100.0;
		DoubleMatrix penalty = (DoubleMatrix) MA.scale(getPenaltyMatrix(nWeights, diffOrder), lambda);
		int[] onIndex = new int[] {1, 4, 11, 12, 15, 17};
		double[] obs = new double[] {0, 1.0, 1.0, 1.0, 0.0, 0.0};
		int n = onIndex.Length;

		System.Func<DoubleArray, DoubleArray> func = (DoubleArray x) =>
		{

	return DoubleArray.of(n, i => x.get(onIndex[i]));
		};

		System.Func<DoubleArray, DoubleMatrix> jac = (DoubleArray x) =>
		{

	return DoubleMatrix.of(n, nWeights, (i, j) => j == onIndex[i] ? 1d : 0d);
		};

		Well44497b random = new Well44497b(0L);
		DoubleArray start = DoubleArray.of(nWeights, i => random.NextDouble());

		LeastSquareWithPenaltyResults lsRes = NLLSWP.solve(DoubleArray.copyOf(obs), DoubleArray.filled(n, 0.01), func, jac, start, penalty);
		if (print)
		{
		  Console.WriteLine("chi2: " + lsRes.ChiSq);
		  Console.WriteLine(lsRes.FitParameters);
		}
		for (int i = 0; i < n; i++)
		{
		  assertEquals(obs[i], lsRes.FitParameters.get(onIndex[i]), 0.01);
		}
		double expPen = 20.87912357454752;
		assertEquals(expPen, lsRes.Penalty, 1e-9);
	  }

	}

}