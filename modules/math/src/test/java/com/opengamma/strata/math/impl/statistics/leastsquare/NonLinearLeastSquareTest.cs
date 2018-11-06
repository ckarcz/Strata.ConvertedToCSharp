using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.leastsquare
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MersenneTwister = com.opengamma.strata.math.impl.cern.MersenneTwister;
	using MersenneTwister64 = com.opengamma.strata.math.impl.cern.MersenneTwister64;
	using ParameterizedFunction = com.opengamma.strata.math.impl.function.ParameterizedFunction;
	using LUDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.LUDecompositionCommons;
	using LUDecompositionResult = com.opengamma.strata.math.impl.linearalgebra.LUDecompositionResult;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NonLinearLeastSquareTest
	public class NonLinearLeastSquareTest
	{
	  private static readonly NormalDistribution NORMAL = new NormalDistribution(0, 1.0, new MersenneTwister64(MersenneTwister.DEFAULT_SEED));
	  private static readonly DoubleArray X;
	  private static readonly DoubleArray Y;
	  private static readonly DoubleArray SIGMA;
	  private static readonly NonLinearLeastSquare LS;

	  private static readonly System.Func<double, double> TARGET = (final double? x) =>
	  {

  return Math.Sin(x);
	  };

	  private static readonly System.Func<DoubleArray, DoubleArray> FUNCTION = new FuncAnonymousInnerClass();

	  private class FuncAnonymousInnerClass : System.Func<DoubleArray, DoubleArray>
	  {
		  public FuncAnonymousInnerClass()
		  {
		  }


//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleArray apply(final com.opengamma.strata.collect.array.DoubleArray a)
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
		  public override DoubleArray apply(DoubleArray a)
		  {
			ArgChecker.isTrue(a.size() == 4, "four parameters");
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = X.size();
			int n = X.size();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] res = new double[n];
			double[] res = new double[n];
			for (int i = 0; i < n; i++)
			{
			  res[i] = a.get(0) * Math.Sin(a.get(1) * X.get(i) + a.get(2)) + a.get(3);
			}
			return DoubleArray.copyOf(res);
		  }
	  }

	  private static readonly ParameterizedFunction<double, DoubleArray, double> PARAM_FUNCTION = new ParameterizedFunctionAnonymousInnerClass();

	  private class ParameterizedFunctionAnonymousInnerClass : ParameterizedFunction<double, DoubleArray, double>
	  {
		  public ParameterizedFunctionAnonymousInnerClass()
		  {
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> evaluate(final System.Nullable<double> x, final com.opengamma.strata.collect.array.DoubleArray a)
		  public override double? evaluate(double? x, DoubleArray a)
		  {
			ArgChecker.isTrue(a.size() == NumberOfParameters, "four parameters");
			return a.get(0) * Math.Sin(a.get(1) * x + a.get(2)) + a.get(3);
		  }

		  public override int NumberOfParameters
		  {
			  get
			  {
				return 4;
			  }
		  }
	  }

	  private static readonly ParameterizedFunction<double, DoubleArray, DoubleArray> PARAM_GRAD = new ParameterizedFunctionAnonymousInnerClass2();

	  private class ParameterizedFunctionAnonymousInnerClass2 : ParameterizedFunction<double, DoubleArray, DoubleArray>
	  {
		  public ParameterizedFunctionAnonymousInnerClass2()
		  {
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleArray evaluate(final System.Nullable<double> x, final com.opengamma.strata.collect.array.DoubleArray a)
		  public override DoubleArray evaluate(double? x, DoubleArray a)
		  {
			ArgChecker.isTrue(a.size() == NumberOfParameters, "four parameters");
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double temp1 = Math.sin(a.get(1) * x + a.get(2));
			double temp1 = Math.Sin(a.get(1) * x + a.get(2));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double temp2 = Math.cos(a.get(1) * x + a.get(2));
			double temp2 = Math.Cos(a.get(1) * x + a.get(2));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] res = new double[4];
				double[] res = new double[4];
				res[0] = temp1;
				res[2] = a.get(0) * temp2;
				res[1] = x * res[2];
				res[3] = 1.0;
				return DoubleArray.copyOf(res);
		  }

		  public override int NumberOfParameters
		  {
			  get
			  {
				return 4;
			  }
		  }
	  }

	  private static readonly System.Func<DoubleArray, DoubleMatrix> GRAD = new FuncAnonymousInnerClass2();

	  private class FuncAnonymousInnerClass2 : System.Func<DoubleArray, DoubleMatrix>
	  {
		  public FuncAnonymousInnerClass2()
		  {
		  }


//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix apply(final com.opengamma.strata.collect.array.DoubleArray a)
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
		  public override DoubleMatrix apply(DoubleArray a)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = X.size();
			int n = X.size();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int m = a.size();
			int m = a.size();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] res = new double[n][m];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[n][m];
			double[][] res = RectangularArrays.ReturnRectangularDoubleArray(n, m);
			for (int i = 0; i < n; i++)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray temp = PARAM_GRAD.evaluate(X.get(i), a);
			  DoubleArray temp = PARAM_GRAD.evaluate(X.get(i), a);
			  ArgChecker.isTrue(m == temp.size());
			  for (int j = 0; j < m; j++)
			  {
				res[i][j] = temp.get(j);
			  }
			}
			return DoubleMatrix.copyOf(res);
		  }
	  }

	  static NonLinearLeastSquareTest()
	  {
		X = DoubleArray.of(20, i => -Math.PI + i * Math.PI / 10);
		Y = DoubleArray.of(20, i => TARGET.apply(X.get(i)));
		SIGMA = DoubleArray.of(20, i => 0.1 * Math.Exp(Math.Abs(X.get(i)) / Math.PI));
		LS = new NonLinearLeastSquare();
	  }

	  public virtual void solveExactTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(1.2, 0.8, -0.2, -0.3);
		DoubleArray start = DoubleArray.of(1.2, 0.8, -0.2, -0.3);
		LeastSquareResults result = LS.solve(X, Y, SIGMA, PARAM_FUNCTION, PARAM_GRAD, start);
		assertEquals(0.0, result.ChiSq, 1e-8);
		assertEquals(1.0, result.FitParameters.get(0), 1e-8);
		assertEquals(1.0, result.FitParameters.get(1), 1e-8);
		assertEquals(0.0, result.FitParameters.get(2), 1e-8);
		assertEquals(0.0, result.FitParameters.get(3), 1e-8);
		result = LS.solve(X, Y, SIGMA.get(0), PARAM_FUNCTION, PARAM_GRAD, start);
		assertEquals(0.0, result.ChiSq, 1e-8);
		assertEquals(1.0, result.FitParameters.get(0), 1e-8);
		assertEquals(1.0, result.FitParameters.get(1), 1e-8);
		assertEquals(0.0, result.FitParameters.get(2), 1e-8);
		assertEquals(0.0, result.FitParameters.get(3), 1e-8);
		result = LS.solve(X, Y, PARAM_FUNCTION, PARAM_GRAD, start);
		assertEquals(0.0, result.ChiSq, 1e-8);
		assertEquals(1.0, result.FitParameters.get(0), 1e-8);
		assertEquals(1.0, result.FitParameters.get(1), 1e-8);
		assertEquals(0.0, result.FitParameters.get(2), 1e-8);
		assertEquals(0.0, result.FitParameters.get(3), 1e-8);
	  }

	  public virtual void solveExactTest2()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(0.2, 1.8, 0.2, 0.3);
		DoubleArray start = DoubleArray.of(0.2, 1.8, 0.2, 0.3);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquareResults result = LS.solve(Y, SIGMA, FUNCTION, start);
		LeastSquareResults result = LS.solve(Y, SIGMA, FUNCTION, start);
		assertEquals(0.0, result.ChiSq, 1e-8);
		assertEquals(1.0, result.FitParameters.get(0), 1e-8);
		assertEquals(1.0, result.FitParameters.get(1), 1e-8);
		assertEquals(0.0, result.FitParameters.get(2), 1e-8);
		assertEquals(0.0, result.FitParameters.get(3), 1e-8);
	  }

	  public virtual void solveExactWithoutGradientTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(1.2, 0.8, -0.2, -0.3);
		DoubleArray start = DoubleArray.of(1.2, 0.8, -0.2, -0.3);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final NonLinearLeastSquare ls = new NonLinearLeastSquare();
		NonLinearLeastSquare ls = new NonLinearLeastSquare();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquareResults result = ls.solve(X, Y, SIGMA, PARAM_FUNCTION, start);
		LeastSquareResults result = ls.solve(X, Y, SIGMA, PARAM_FUNCTION, start);
		assertEquals(0.0, result.ChiSq, 1e-8);
		assertEquals(1.0, result.FitParameters.get(0), 1e-8);
		assertEquals(1.0, result.FitParameters.get(1), 1e-8);
		assertEquals(0.0, result.FitParameters.get(2), 1e-8);
		assertEquals(0.0, result.FitParameters.get(3), 1e-8);
	  }

	  public virtual void solveRandomNoiseTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.matrix.MatrixAlgebra ma = new com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra();
		MatrixAlgebra ma = new OGMatrixAlgebra();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] y = new double[20];
		double[] y = new double[20];
		for (int i = 0; i < 20; i++)
		{
		  y[i] = Y.get(i) + SIGMA.get(i) * NORMAL.nextRandom();
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(0.7, 1.4, 0.2, -0.3);
		DoubleArray start = DoubleArray.of(0.7, 1.4, 0.2, -0.3);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final NonLinearLeastSquare ls = new NonLinearLeastSquare();
		NonLinearLeastSquare ls = new NonLinearLeastSquare();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquareResults res = ls.solve(X, com.opengamma.strata.collect.array.DoubleArray.copyOf(y), SIGMA, PARAM_FUNCTION, PARAM_GRAD, start);
		LeastSquareResults res = ls.solve(X, DoubleArray.copyOf(y), SIGMA, PARAM_FUNCTION, PARAM_GRAD, start);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double chiSqDoF = res.getChiSq() / 16;
		double chiSqDoF = res.ChiSq / 16;
		assertTrue(chiSqDoF > 0.25);
		assertTrue(chiSqDoF < 3.0);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray trueValues = com.opengamma.strata.collect.array.DoubleArray.of(1, 1, 0, 0);
		DoubleArray trueValues = DoubleArray.of(1, 1, 0, 0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray delta = (com.opengamma.strata.collect.array.DoubleArray) ma.subtract(res.getFitParameters(), trueValues);
		DoubleArray delta = (DoubleArray) ma.subtract(res.FitParameters, trueValues);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.linearalgebra.LUDecompositionCommons decmp = new com.opengamma.strata.math.impl.linearalgebra.LUDecompositionCommons();
		LUDecompositionCommons decmp = new LUDecompositionCommons();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.linearalgebra.LUDecompositionResult decmpRes = decmp.apply(res.getCovariance());
		LUDecompositionResult decmpRes = decmp.apply(res.Covariance);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix invCovariance = decmpRes.solve(com.opengamma.strata.collect.array.DoubleMatrix.identity(4));
		DoubleMatrix invCovariance = decmpRes.solve(DoubleMatrix.identity(4));

		double z = ma.getInnerProduct(delta, ma.multiply(invCovariance, delta));
		z = Math.Sqrt(z);

		assertTrue(z < 3.0);

	  }

	  public virtual void smallPertubationTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.matrix.MatrixAlgebra ma = new com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra();
		MatrixAlgebra ma = new OGMatrixAlgebra();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] dy = new double[20];
		double[] dy = new double[20];
		for (int i = 0; i < 20; i++)
		{
		  dy[i] = 0.1 * SIGMA.get(i) * NORMAL.nextRandom();
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray deltaY = com.opengamma.strata.collect.array.DoubleArray.copyOf(dy);
		DoubleArray deltaY = DoubleArray.copyOf(dy);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray solution = com.opengamma.strata.collect.array.DoubleArray.of(1.0, 1.0, 0.0, 0.0);
		DoubleArray solution = DoubleArray.of(1.0, 1.0, 0.0, 0.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final NonLinearLeastSquare ls = new NonLinearLeastSquare();
		NonLinearLeastSquare ls = new NonLinearLeastSquare();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix res = ls.calInverseJacobian(SIGMA, FUNCTION, GRAD, solution);
		DoubleMatrix res = ls.calInverseJacobian(SIGMA, FUNCTION, GRAD, solution);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray deltaParms = (com.opengamma.strata.collect.array.DoubleArray) ma.multiply(res, deltaY);
		DoubleArray deltaParms = (DoubleArray) ma.multiply(res, deltaY);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray y = (com.opengamma.strata.collect.array.DoubleArray) ma.add(Y, deltaY);
		DoubleArray y = (DoubleArray) ma.add(Y, deltaY);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquareResults lsRes = ls.solve(X, y, SIGMA, PARAM_FUNCTION, PARAM_GRAD, solution);
		LeastSquareResults lsRes = ls.solve(X, y, SIGMA, PARAM_FUNCTION, PARAM_GRAD, solution);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray trueDeltaParms = (com.opengamma.strata.collect.array.DoubleArray) ma.subtract(lsRes.getFitParameters(), solution);
		DoubleArray trueDeltaParms = (DoubleArray) ma.subtract(lsRes.FitParameters, solution);

		assertEquals(trueDeltaParms.get(0), deltaParms.get(0), 5e-5);
		assertEquals(trueDeltaParms.get(1), deltaParms.get(1), 5e-5);
		assertEquals(trueDeltaParms.get(2), deltaParms.get(2), 5e-5);
		assertEquals(trueDeltaParms.get(3), deltaParms.get(3), 5e-5);
	  }

	}

}