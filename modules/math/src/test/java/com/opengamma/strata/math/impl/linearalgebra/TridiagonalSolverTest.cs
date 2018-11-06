/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.linearalgebra.TridiagonalSolver.solvTriDag;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MersenneTwister = com.opengamma.strata.math.impl.cern.MersenneTwister;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;
	using ProbabilityDistribution = com.opengamma.strata.math.impl.statistics.distribution.ProbabilityDistribution;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TridiagonalSolverTest
	public class TridiagonalSolverTest
	{

	  private static MatrixAlgebra MA = new OGMatrixAlgebra();
	  private static ProbabilityDistribution<double> RANDOM = new NormalDistribution(0, 1, new MersenneTwister(123));

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		const int n = 97;
		double[] a = new double[n - 1];
		double[] b = new double[n];
		double[] c = new double[n - 1];
		double[] x = new double[n];

		for (int ii = 0; ii < n; ii++)
		{
		  b[ii] = RANDOM.nextRandom();
		  x[ii] = RANDOM.nextRandom();
		  if (ii < n - 1)
		  {
			a[ii] = RANDOM.nextRandom();
			c[ii] = RANDOM.nextRandom();
		  }
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final TridiagonalMatrix m = new TridiagonalMatrix(b, a, c);
		TridiagonalMatrix m = new TridiagonalMatrix(b, a, c);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray xVec = com.opengamma.strata.collect.array.DoubleArray.copyOf(x);
		DoubleArray xVec = DoubleArray.copyOf(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray yVec = (com.opengamma.strata.collect.array.DoubleArray) MA.multiply(m, xVec);
		DoubleArray yVec = (DoubleArray) MA.multiply(m, xVec);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xSolv = solvTriDag(m, yVec).toArray();
		double[] xSolv = solvTriDag(m, yVec).toArray();

		for (int i = 0; i < n; i++)
		{
		  assertEquals(x[i], xSolv[i], 1e-9);
		}

		DoubleArray resi = (DoubleArray) MA.subtract(MA.multiply(m, DoubleArray.copyOf(xSolv)), yVec);
		double err = MA.getNorm2(resi);
		assertEquals(0.0, err, 1e-14);

	  }

	}

}