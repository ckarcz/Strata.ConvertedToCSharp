using System;
using System.Collections.Generic;

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
	using MersenneTwister = com.opengamma.strata.math.impl.cern.MersenneTwister;
	using MersenneTwister64 = com.opengamma.strata.math.impl.cern.MersenneTwister64;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;
	using BasisFunctionAggregation = com.opengamma.strata.math.impl.interpolation.BasisFunctionAggregation;
	using BasisFunctionGenerator = com.opengamma.strata.math.impl.interpolation.BasisFunctionGenerator;
	using BasisFunctionKnots = com.opengamma.strata.math.impl.interpolation.BasisFunctionKnots;
	using PSplineFitter = com.opengamma.strata.math.impl.interpolation.PSplineFitter;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GeneralizedLeastSquareTest
	public class GeneralizedLeastSquareTest
	{
	  private static bool PRINT = false;

	  protected internal static readonly RandomEngine RANDOM = new MersenneTwister64(MersenneTwister.DEFAULT_SEED);
	  private static readonly NormalDistribution NORMAL = new NormalDistribution(0, 1.0, RANDOM);
	  private static readonly double[] WEIGHTS = new double[] {1.0, -0.5, 2.0, 0.23, 1.45};
	  private static readonly double?[] X;
	  private static readonly double[] Y;
	  private static readonly double[] SIGMA;
	  private static readonly IList<DoubleArray> X_TRIG;
	  private static readonly IList<double> Y_TRIG;
	  private static readonly IList<double> SIGMA_TRIG;
	  private static readonly IList<double> SIGMA_COS_EXP;
	  private static readonly IList<double[]> X_SIN_EXP;
	  private static readonly IList<double> Y_SIN_EXP;
	  private static readonly IList<System.Func<double, double>> SIN_FUNCTIONS;
	  private static readonly System.Func<double, double> TEST_FUNCTION;
	  private static readonly IList<System.Func<double, double>> BASIS_FUNCTIONS;
	  private static readonly IList<System.Func<double[], double>> BASIS_FUNCTIONS_2D;
	  private static System.Func<double[], double> SIN_EXP_FUNCTION;

	  private static readonly IList<System.Func<DoubleArray, double>> VECTOR_TRIG_FUNCTIONS;
	  private static readonly System.Func<DoubleArray, double> VECTOR_TEST_FUNCTION;

	  static GeneralizedLeastSquareTest()
	  {
		SIN_FUNCTIONS = new List<>();
		for (int i = 0; i < WEIGHTS.Length; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int k = i;
		  int k = i;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> func = new java.util.function.Function<double, double>()
		  System.Func<double, double> func = (final double? x) =>
		  {

	  return Math.Sin((2 * k + 1) * x);
		  };
		  SIN_FUNCTIONS.Add(func);
		}
		TEST_FUNCTION = new BasisFunctionAggregation<>(SIN_FUNCTIONS, WEIGHTS);

		VECTOR_TRIG_FUNCTIONS = new List<>();
		for (int i = 0; i < WEIGHTS.Length; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int k = i;
		  int k = i;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double> func = new java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double>()
		  System.Func<DoubleArray, double> func = (final DoubleArray x) =>
		  {
	  ArgChecker.isTrue(x.size() == 2);
	  return Math.Sin((2 * k + 1) * x.get(0)) * Math.Cos((2 * k + 1) * x.get(1));
		  };
		  VECTOR_TRIG_FUNCTIONS.Add(func);
		}
		VECTOR_TEST_FUNCTION = new BasisFunctionAggregation<>(VECTOR_TRIG_FUNCTIONS, WEIGHTS);

		SIN_EXP_FUNCTION = (final double[] x) =>
		{

	return Math.Sin(Math.PI * x[0] / 10.0) * Math.Exp(-x[1] / 5.0);
		};

		const int n = 10;

		X = new double?[n];
		Y = new double[n];
		SIGMA = new double[n];
		X_TRIG = new List<>();
		Y_TRIG = new List<>();
		SIGMA_TRIG = new List<>();
		for (int i = 0; i < n; i++)
		{
		  X[i] = i / 5.0;
		  Y[i] = TEST_FUNCTION.apply(X[i]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] temp = new double[2];
		  double[] temp = new double[2];
		  temp[0] = 2.0 * RANDOM.NextDouble();
		  temp[1] = 2.0 * RANDOM.NextDouble();
		  X_TRIG.Add(DoubleArray.copyOf(temp));
		  Y_TRIG.Add(VECTOR_TEST_FUNCTION.apply(X_TRIG[i]));
		  SIGMA[i] = 0.01;
		  SIGMA_TRIG.Add(0.01);
		}

		SIGMA_COS_EXP = new List<>();
		X_SIN_EXP = new List<>();
		Y_SIN_EXP = new List<>();
		for (int i = 0; i < 20; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] temp = new double[2];
		  double[] temp = new double[2];
		  temp[0] = 10.0 * RANDOM.NextDouble();
		  temp[1] = 10.0 * RANDOM.NextDouble();
		  X_SIN_EXP.Add(temp);
		  Y_SIN_EXP.Add(SIN_EXP_FUNCTION.apply(X_SIN_EXP[i]));
		  SIGMA_COS_EXP.Add(0.01);
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.BasisFunctionGenerator generator = new com.opengamma.strata.math.impl.interpolation.BasisFunctionGenerator();
		BasisFunctionGenerator generator = new BasisFunctionGenerator();
		BASIS_FUNCTIONS = generator.generateSet(BasisFunctionKnots.fromUniform(0.0, 2.0, 20, 3));
		BasisFunctionKnots[] knots = new BasisFunctionKnots[2];
		knots[0] = BasisFunctionKnots.fromUniform(0, 10, 10, 3);
		knots[1] = BasisFunctionKnots.fromUniform(0, 10, 10, 3);
		BASIS_FUNCTIONS_2D = generator.generateSet(knots);
	  }

	  public virtual void testPerfectFit()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquare gls = new GeneralizedLeastSquare();
		GeneralizedLeastSquare gls = new GeneralizedLeastSquare();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquareResults results = gls.solve(X, Y, SIGMA, SIN_FUNCTIONS);
		LeastSquareResults results = gls.solve(X, Y, SIGMA, SIN_FUNCTIONS);
		assertEquals(0.0, results.ChiSq, 1e-8);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray w = results.getFitParameters();
		DoubleArray w = results.FitParameters;
		for (int i = 0; i < WEIGHTS.Length; i++)
		{
		  assertEquals(WEIGHTS[i], w.get(i), 1e-8);
		}
	  }

	  public virtual void testPerfectFitVector()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquare gls = new GeneralizedLeastSquare();
		GeneralizedLeastSquare gls = new GeneralizedLeastSquare();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquareResults results = gls.solve(X_TRIG, Y_TRIG, SIGMA_TRIG, VECTOR_TRIG_FUNCTIONS);
		LeastSquareResults results = gls.solve(X_TRIG, Y_TRIG, SIGMA_TRIG, VECTOR_TRIG_FUNCTIONS);
		assertEquals(0.0, results.ChiSq, 1e-8);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray w = results.getFitParameters();
		DoubleArray w = results.FitParameters;
		for (int i = 0; i < WEIGHTS.Length; i++)
		{
		  assertEquals(WEIGHTS[i], w.get(i), 1e-8);
		}
	  }

	  public virtual void testFit()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquare gls = new GeneralizedLeastSquare();
		GeneralizedLeastSquare gls = new GeneralizedLeastSquare();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] y = new double[Y.length];
		double[] y = new double[Y.Length];
		for (int i = 0; i < Y.Length; i++)
		{
		  y[i] = Y[i] + SIGMA[i] * NORMAL.nextRandom();
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquareResults results = gls.solve(X, y, SIGMA, SIN_FUNCTIONS);
		LeastSquareResults results = gls.solve(X, y, SIGMA, SIN_FUNCTIONS);
		assertTrue(results.ChiSq < 3 * Y.Length);

	  }

	  public virtual void testBSplineFit()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquare gls = new GeneralizedLeastSquare();
		GeneralizedLeastSquare gls = new GeneralizedLeastSquare();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquareResults results = gls.solve(X, Y, SIGMA, BASIS_FUNCTIONS);
		LeastSquareResults results = gls.solve(X, Y, SIGMA, BASIS_FUNCTIONS);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> spline = new com.opengamma.strata.math.impl.interpolation.BasisFunctionAggregation<>(BASIS_FUNCTIONS, results.getFitParameters().toArray());
		System.Func<double, double> spline = new BasisFunctionAggregation<double, double>(BASIS_FUNCTIONS, results.FitParameters.toArray());
		assertEquals(0.0, results.ChiSq, 1e-12);
		assertEquals(-0.023605293, spline(0.5), 1e-8);

		if (PRINT)
		{
		  Console.WriteLine("Chi^2:\t" + results.ChiSq);
		  Console.WriteLine("weights:\t" + results.FitParameters);

		  for (int i = 0; i < 101; i++)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = 0 + i * 2.0 / 100.0;
			double x = 0 + i * 2.0 / 100.0;
			Console.WriteLine(x + "\t" + spline(x));
		  }
		  for (int i = 0; i < X.Length; i++)
		  {
			Console.WriteLine(X[i] + "\t" + Y[i]);
		  }
		}
	  }

	  public virtual void testBSplineFit2D()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquare gls = new GeneralizedLeastSquare();
		GeneralizedLeastSquare gls = new GeneralizedLeastSquare();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquareResults results = gls.solve(X_SIN_EXP, Y_SIN_EXP, SIGMA_COS_EXP, BASIS_FUNCTIONS_2D);
		LeastSquareResults results = gls.solve(X_SIN_EXP, Y_SIN_EXP, SIGMA_COS_EXP, BASIS_FUNCTIONS_2D);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double[], double> spline = new com.opengamma.strata.math.impl.interpolation.BasisFunctionAggregation<>(BASIS_FUNCTIONS_2D, results.getFitParameters().toArray());
		System.Func<double[], double> spline = new BasisFunctionAggregation<double[], double>(BASIS_FUNCTIONS_2D, results.FitParameters.toArray());
		assertEquals(0.0, results.ChiSq, 1e-16);
		assertEquals(0.05161579, spline(new double[] {4, 3}), 1e-8);

		/*
		 * Print out function for debugging
		 */
		if (PRINT)
		{
		  Console.WriteLine("Chi^2:\t" + results.ChiSq);
		  Console.WriteLine("weights:\t" + results.FitParameters);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] x = new double[2];
		  double[] x = new double[2];

		  for (int i = 0; i < 101; i++)
		  {
			x[0] = 0 + i * 10.0 / 100.0;
			Console.Write("\t" + x[0]);
		  }
		  Console.Write("\n");
		  for (int i = 0; i < 101; i++)
		  {
			x[0] = -0.0 + i * 10 / 100.0;
			Console.Write(x[0]);
			for (int j = 0; j < 101; j++)
			{
			  x[1] = -0.0 + j * 10.0 / 100.0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double y = spline.apply(x);
			  double y = spline(x);
			  Console.Write("\t" + y);
			}
			Console.Write("\n");
		  }
		}
	  }

	  public virtual void testPSplineFit()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquare gls = new GeneralizedLeastSquare();
		GeneralizedLeastSquare gls = new GeneralizedLeastSquare();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquareResults<double> results = gls.solve(X, Y, SIGMA, BASIS_FUNCTIONS, 1000.0, 2);
		GeneralizedLeastSquareResults<double> results = gls.solve(X, Y, SIGMA, BASIS_FUNCTIONS, 1000.0, 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> spline = results.getFunction();
		System.Func<double, double> spline = results.Function;
		assertEquals(2225.7, results.ChiSq, 1e-1);
		assertEquals(-0.758963811327287, spline(1.1), 1e-8);

		/*
		 * Print out function for debugging
		 */
		if (PRINT)
		{
		  Console.WriteLine("Chi^2:\t" + results.ChiSq);
		  Console.WriteLine("weights:\t" + results.FitParameters);

		  for (int i = 0; i < 101; i++)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = 0 + i * 2.0 / 100.0;
			double x = 0 + i * 2.0 / 100.0;
			Console.WriteLine(x + "\t" + spline(x));
		  }
		  for (int i = 0; i < X.Length; i++)
		  {
			Console.WriteLine(X[i] + "\t" + Y[i]);
		  }
		}
	  }

	  public virtual void testPSplineFit2()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.BasisFunctionGenerator generator = new com.opengamma.strata.math.impl.interpolation.BasisFunctionGenerator();
		BasisFunctionGenerator generator = new BasisFunctionGenerator();
		IList<System.Func<double, double>> basisFuncs = generator.generateSet(BasisFunctionKnots.fromUniform(0, 12, 100, 3));
		IList<System.Func<double, double>> basisFuncsLog = generator.generateSet(BasisFunctionKnots.fromUniform(-5, 3, 100, 3));

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquare gls = new GeneralizedLeastSquare();
		GeneralizedLeastSquare gls = new GeneralizedLeastSquare();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xData = new double[] {7.0 / 365, 14 / 365.0, 21 / 365.0, 1 / 12.0, 3 / 12.0, 0.5, 0.75, 1, 5, 10 };
		double[] xData = new double[] {7.0 / 365, 14 / 365.0, 21 / 365.0, 1 / 12.0, 3 / 12.0, 0.5, 0.75, 1, 5, 10};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yData = new double[] {0.972452371, 0.749039802, 0.759792085, 0.714206462, 0.604446956, 0.517955313, 0.474807307, 0.443532132, 0.2404755, 0.197128583};
		double[] yData = new double[] {0.972452371, 0.749039802, 0.759792085, 0.714206462, 0.604446956, 0.517955313, 0.474807307, 0.443532132, 0.2404755, 0.197128583};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = xData.length;
		int n = xData.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] lnX = new double[n];
		double[] lnX = new double[n];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yData2 = new double[n];
		double[] yData2 = new double[n];
		for (int i = 0; i < n; i++)
		{
		  lnX[i] = Math.Log(xData[i]);
		  yData2[i] = yData[i] * yData[i] * xData[i];
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] sigma = new double[n];
		double[] sigma = new double[n];
		Arrays.fill(sigma, 0.01);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquareResults<double> results = gls.solve(xData, yData, sigma, basisFuncs, 1000.0, 2);
		GeneralizedLeastSquareResults<double> results = gls.solve(xData, yData, sigma, basisFuncs, 1000.0, 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> spline = results.getFunction();
		System.Func<double, double> spline = results.Function;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquareResults<double> resultsLog = gls.solve(lnX, yData, sigma, basisFuncsLog, 1000.0, 2);
		GeneralizedLeastSquareResults<double> resultsLog = gls.solve(lnX, yData, sigma, basisFuncsLog, 1000.0, 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> splineLog = resultsLog.getFunction();
		System.Func<double, double> splineLog = resultsLog.Function;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquareResults<double> resultsVar = gls.solve(xData, yData2, sigma, basisFuncs, 1000.0, 2);
		GeneralizedLeastSquareResults<double> resultsVar = gls.solve(xData, yData2, sigma, basisFuncs, 1000.0, 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> splineVar = resultsVar.getFunction();
		System.Func<double, double> splineVar = resultsVar.Function;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquareResults<double> resultsVarLog = gls.solve(lnX, yData2, sigma, basisFuncsLog, 1000.0, 2);
		GeneralizedLeastSquareResults<double> resultsVarLog = gls.solve(lnX, yData2, sigma, basisFuncsLog, 1000.0, 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> splineVarLog = resultsVarLog.getFunction();
		System.Func<double, double> splineVarLog = resultsVarLog.Function;

		if (PRINT)
		{
		  Console.WriteLine("Chi^2:\t" + results.ChiSq);
		  Console.WriteLine("weights:\t" + results.FitParameters);

		  for (int i = 0; i < 101; i++)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double logX = -5 + 8 * i / 100.0;
			double logX = -5 + 8 * i / 100.0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = Math.exp(logX);
			double x = Math.Exp(logX);
			Console.WriteLine(x + "\t" + +logX + "\t" + spline(x) + "\t" + splineLog(logX) + "\t" + splineVar(x) + "\t" + splineVarLog(logX));
		  }
		  for (int i = 0; i < n; i++)
		  {
			Console.WriteLine(lnX[i] + "\t" + yData[i]);
		  }
		}

	  }

	  public virtual void testPSplineFit2D()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.interpolation.PSplineFitter psf = new com.opengamma.strata.math.impl.interpolation.PSplineFitter();
		PSplineFitter psf = new PSplineFitter();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GeneralizedLeastSquareResults<double[]> results = psf.solve(X_SIN_EXP, Y_SIN_EXP, SIGMA_COS_EXP, new double[] {0.0, 0.0 }, new double[] {10.0, 10.0 }, new int[] {10, 10 }, new int[] {3, 3 }, new double[] {0.001, 0.001 }, new int[] {3, 3 });
		GeneralizedLeastSquareResults<double[]> results = psf.solve(X_SIN_EXP, Y_SIN_EXP, SIGMA_COS_EXP, new double[] {0.0, 0.0}, new double[] {10.0, 10.0}, new int[] {10, 10}, new int[] {3, 3}, new double[] {0.001, 0.001}, new int[] {3, 3});

		assertEquals(0.0, results.ChiSq, 1e-9);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double[], double> spline = results.getFunction();
		System.Func<double[], double> spline = results.Function;
		assertEquals(0.5333876489112092, spline(new double[] {4, 3}), 1e-8);

		/*
		 * Print out function for debugging
		 */
		if (PRINT)
		{
		  Console.WriteLine("Chi^2:\t" + results.ChiSq);
		  Console.WriteLine("weights:\t" + results.FitParameters);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] x = new double[2];
		  double[] x = new double[2];

		  for (int i = 0; i < 101; i++)
		  {
			x[0] = 0 + i * 10.0 / 100.0;
			Console.Write("\t" + x[0]);
		  }
		  Console.Write("\n");
		  for (int i = 0; i < 101; i++)
		  {
			x[0] = -0.0 + i * 10 / 100.0;
			Console.Write(x[0]);
			for (int j = 0; j < 101; j++)
			{
			  x[1] = -0.0 + j * 10.0 / 100.0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double y = spline.apply(x);
			  double y = spline(x);
			  Console.Write("\t" + y);
			}
			Console.Write("\n");
		  }
		}
	  }
	}

}