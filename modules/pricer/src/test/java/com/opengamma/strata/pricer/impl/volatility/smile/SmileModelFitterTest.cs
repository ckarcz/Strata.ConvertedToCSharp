using System;
using System.Collections;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.smile
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Logger = org.slf4j.Logger;
	using Test = org.testng.annotations.Test;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MersenneTwister = com.opengamma.strata.math.impl.cern.MersenneTwister;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;
	using VectorFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.VectorFieldFirstOrderDifferentiator;
	using LeastSquareResults = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResults;
	using LeastSquareResultsWithTransform = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResultsWithTransform;

	/// <summary>
	/// Test case for smile model fitters.
	/// </summary>
	/// @param <T> the smile model data </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class SmileModelFitterTest<T extends SmileModelData>
	public abstract class SmileModelFitterTest<T> where T : SmileModelData
	{

	  protected internal static double TIME_TO_EXPIRY = 7.0;
	  protected internal static double F = 0.03;
	  private static RandomEngine UNIFORM = new MersenneTwister();
	  protected internal static double[] STRIKES = new double[] {0.005, 0.01, 0.02, 0.03, 0.04, 0.05, 0.07, 0.1};

	  protected internal double[] _cleanVols;
	  protected internal double[] _noisyVols;
	  protected internal double[] _errors;
	  protected internal VolatilityFunctionProvider<T> _model;
	  protected internal SmileModelFitter<T> _fitter;
	  protected internal SmileModelFitter<T> _nosiyFitter;
	  protected internal double _chiSqEps = 1e-6;
	  protected internal double _paramValueEps = 1e-6;

	  internal abstract Logger getlogger();

	  internal abstract VolatilityFunctionProvider<T> Model {get;}

	  internal abstract T ModelData {get;}

	  internal abstract SmileModelFitter<T> getFitter(double forward, double[] strikes, double timeToExpiry, double[] impliedVols, double[] error, VolatilityFunctionProvider<T> model);

	  internal abstract double[][] StartValues {get;}

	  internal abstract double[] RandomStartValues {get;}

	  internal abstract BitArray[] FixedValues {get;}

	  public SmileModelFitterTest()
	  {
		VolatilityFunctionProvider<T> model = Model;
		T data = ModelData;
		int n = STRIKES.Length;
		_noisyVols = new double[n];
		_errors = new double[n];
		_cleanVols = new double[n];
		Arrays.fill(_errors, 1e-4);
		for (int i = 0; i < n; i++)
		{
		  _cleanVols[i] = model.volatility(F, STRIKES[i], TIME_TO_EXPIRY, data);
		  _noisyVols[i] = _cleanVols[i] + UNIFORM.NextDouble() * _errors[i];
		}
		_fitter = getFitter(F, STRIKES, TIME_TO_EXPIRY, _cleanVols, _errors, model);
		_nosiyFitter = getFitter(F, STRIKES, TIME_TO_EXPIRY, _noisyVols, _errors, model);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") public void testExactFit()
	  public virtual void testExactFit()
	  {
		double[][] start = StartValues;
		BitArray[] @fixed = FixedValues;
		int nStartPoints = start.Length;
		ArgChecker.isTrue(@fixed.Length == nStartPoints);
		for (int trys = 0; trys < nStartPoints; trys++)
		{
		  LeastSquareResultsWithTransform results = _fitter.solve(DoubleArray.copyOf(start[trys]), @fixed[trys]);
		  DoubleArray res = results.ModelParameters;
		  assertEquals(0.0, results.ChiSq, _chiSqEps);
		  int n = res.size();
		  T data = ModelData;
		  assertEquals(data.NumberOfParameters, n);
		  for (int i = 0; i < n; i++)
		  {
			assertEquals(data.getParameter(i), res.get(i), _paramValueEps);
		  }
		}
	  }

	  public virtual void testNoisyFit()
	  {
		double[][] start = StartValues;
		BitArray[] @fixed = FixedValues;
		int nStartPoints = start.Length;
		ArgChecker.isTrue(@fixed.Length == nStartPoints);
		for (int trys = 0; trys < nStartPoints; trys++)
		{
		  LeastSquareResultsWithTransform results = _nosiyFitter.solve(DoubleArray.copyOf(start[trys]), @fixed[trys]);
		  DoubleArray res = results.ModelParameters;
		  double eps = 1e-2;
		  assertTrue(results.ChiSq < 7);
		  int n = res.size();
		  T data = ModelData;
		  assertEquals(data.NumberOfParameters, n);
		  for (int i = 0; i < n; i++)
		  {
			assertEquals(data.getParameter(i), res.get(i), eps);
		  }
		}
	  }

	  public virtual void timeTest()
	  {
		long start = 0;
		int hotspotWarmupCycles = 200;
		int benchmarkCycles = 1000;
		int nStarts = StartValues.Length;
		for (int i = 0; i < hotspotWarmupCycles; i++)
		{
		  testNoisyFit();
		}
		start = System.nanoTime();
		for (int i = 0; i < benchmarkCycles; i++)
		{
		  testNoisyFit();
		}
		long time = System.nanoTime() - start;
		getlogger().info("time per fit: " + ((double) time) / benchmarkCycles / nStarts + "ms");
	  }

	  public virtual void horribleMarketDataTest()
	  {
		double forward = 0.0059875;
		double[] strikes = new double[] {0.0012499999999999734, 0.0024999999999999467, 0.003750000000000031, 0.0050000000000000044, 0.006249999999999978, 0.007499999999999951, 0.008750000000000036, 0.010000000000000009, 0.011249999999999982, 0.012499999999999956, 0.01375000000000004, 0.015000000000000013, 0.016249999999999987, 0.01749999999999996, 0.018750000000000044, 0.020000000000000018, 0.02124999999999999, 0.022499999999999964, 0.02375000000000005, 0.025000000000000022, 0.026249999999999996, 0.02749999999999997, 0.028750000000000053, 0.030000000000000027};
		double expiry = 0.09041095890410959;
		double[] vols = new double[] {2.7100433855959642, 1.5506135190088546, 0.9083977239618538, 0.738416513934868, 0.8806973450124451, 1.0906290439592792, 1.2461975189027226, 1.496275983572826, 1.5885915338673156, 1.4842142974195722, 1.7667347426399058, 1.4550288621444052, 1.0651798188736166, 1.143318270172714, 1.216215092528441, 1.2845258218014657, 1.3488224665755535, 1.9259326343836376, 1.9868728791190922, 2.0441767092857317, 2.0982583238541026, 2.1494622372820675, 2.198020785622251, 2.244237863291375};
		int n = strikes.Length;
		double[] errors = new double[n];
		Arrays.fill(errors, 0.01); //1% error
		SmileModelFitter<T> fitter = getFitter(forward, strikes, expiry, vols, errors, Model);
		LeastSquareResults best = null;
		BitArray @fixed = new BitArray();
		for (int i = 0; i < 5; i++)
		{
		  double[] start = RandomStartValues;

		  //   int nStartPoints = start.length;
		  LeastSquareResults lsRes = fitter.solve(DoubleArray.copyOf(start), @fixed);
		  if (best == null)
		  {
			best = lsRes;
		  }
		  else
		  {
			if (lsRes.ChiSq < best.ChiSq)
			{
			  best = lsRes;
			}
		  }
		}
		if (best != null)
		{
		  assertTrue(best.ChiSq < 24000); //average error 31.6% - not a good fit, but the data is horrible
		}
	  }

	  public virtual void testJacobian()
	  {
		T data = ModelData;
		int n = data.NumberOfParameters;
		double[] temp = new double[n];
		for (int i = 0; i < n; i++)
		{
		  temp[i] = data.getParameter(i);
		}
		DoubleArray x = DoubleArray.copyOf(temp);

		testJacobian(x);
	  }

	  // random test to be turned off
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(enabled = false) public void testRandomJacobian()
	  public virtual void testRandomJacobian()
	  {
		for (int i = 0; i < 10; i++)
		{
		  double[] temp = RandomStartValues;
		  DoubleArray x = DoubleArray.copyOf(temp);
		  try
		  {
			testJacobian(x);
		  }
		  catch (AssertionError e)
		  {
			Console.WriteLine("Jacobian test failed at " + x.ToString());
			throw e;
		  }
		}
	  }

	  private void testJacobian(DoubleArray x)
	  {
		int n = x.size();
		System.Func<DoubleArray, DoubleArray> func = _fitter.ModelValueFunction;
		System.Func<DoubleArray, DoubleMatrix> jacFunc = _fitter.ModelJacobianFunction;
		VectorFieldFirstOrderDifferentiator differ = new VectorFieldFirstOrderDifferentiator();
		System.Func<DoubleArray, DoubleMatrix> jacFuncFD = differ.differentiate(func);
		DoubleMatrix jac = jacFunc(x);
		DoubleMatrix jacFD = jacFuncFD(x);
		int rows = jacFD.rowCount();
		int cols = jacFD.columnCount();

		assertEquals(_cleanVols.Length, rows);
		assertEquals(n, cols);
		assertEquals(rows, jac.rowCount());
		assertEquals(cols, jac.columnCount());
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < cols; j++)
		  {
			assertEquals(jacFD.get(i, j), jac.get(i, j), 2e-2);
		  }
		}
	  }

	}

}