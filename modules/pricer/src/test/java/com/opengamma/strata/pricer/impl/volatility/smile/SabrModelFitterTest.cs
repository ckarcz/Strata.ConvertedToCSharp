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
	using LoggerFactory = org.slf4j.LoggerFactory;
	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MersenneTwister = com.opengamma.strata.math.impl.cern.MersenneTwister;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;
	using NonLinearParameterTransforms = com.opengamma.strata.math.impl.minimization.NonLinearParameterTransforms;
	using LeastSquareResultsWithTransform = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResultsWithTransform;


	/// <summary>
	/// Test <seealso cref="SabrModelFitter"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrModelFitterTest extends SmileModelFitterTest<SabrFormulaData>
	public class SabrModelFitterTest : SmileModelFitterTest<SabrFormulaData>
	{

	  private static double ALPHA = 0.05;
	  private static double BETA = 0.5;
	  private static double RHO = -0.3;
	  private static double NU = 0.2;
	  private static Logger log = LoggerFactory.getLogger(typeof(SabrModelFitterTest));
	  private static RandomEngine RANDOM = new MersenneTwister();

	  internal SabrModelFitterTest()
	  {
		_chiSqEps = 1e-10;
	  }

	  internal override VolatilityFunctionProvider<SabrFormulaData> Model
	  {
		  get
		  {
			return SabrHaganVolatilityFunctionProvider.DEFAULT;
		  }
	  }

	  internal override SabrFormulaData ModelData
	  {
		  get
		  {
			return SabrFormulaData.of(ALPHA, BETA, RHO, NU);
		  }
	  }

	  internal override SmileModelFitter<SabrFormulaData> getFitter(double forward, double[] strikes, double timeToExpiry, double[] impliedVols, double[] error, VolatilityFunctionProvider<SabrFormulaData> model)
	  {
		return new SabrModelFitter(forward, DoubleArray.copyOf(strikes), timeToExpiry, DoubleArray.copyOf(impliedVols), DoubleArray.copyOf(error), model);
	  }

	  internal override double[][] StartValues
	  {
		  get
		  {
			return new double[][]
			{
				new double[] {0.1, 0.7, 0.0, 0.3},
				new double[] {0.01, 0.95, 0.9, 0.4},
				new double[] {0.01, 0.5, -0.7, 0.6}
			};
		  }
	  }

	  internal override Logger getlogger()
	  {
		return log;
	  }

	  internal override BitArray[] FixedValues
	  {
		  get
		  {
			BitArray[] @fixed = new BitArray[3];
			@fixed[0] = new BitArray();
			@fixed[1] = new BitArray();
			@fixed[2] = new BitArray();
			@fixed[2].Set(1, true);
			return @fixed;
		  }
	  }

	  internal override double[] RandomStartValues
	  {
		  get
		  {
			double alpha = 0.1 + 0.4 * RANDOM.NextDouble();
			double beta = RANDOM.NextDouble();
			double rho = 2 * RANDOM.NextDouble() - 1;
			double nu = 1.5 * RANDOM.NextDouble();
			return new double[] {alpha, beta, rho, nu};
		  }
	  }

	  public virtual void testExactFitOddStart()
	  {
		double[] start = new double[] {0.01, 0.99, 0.9, 0.4};
		LeastSquareResultsWithTransform results = _fitter.solve(DoubleArray.copyOf(start));
		double[] res = results.ModelParameters.toArray();
		double eps = 1e-6;
		assertEquals(ALPHA, res[0], eps);
		assertEquals(BETA, res[1], eps);
		assertEquals(RHO, res[2], eps);
		assertEquals(NU, res[3], eps);
		assertEquals(0.0, results.ChiSq, eps);
	  }

	  public virtual void testExactFitWithTransform()
	  {
		double[] start = new double[] {0.01, 0.99, 0.9, 0.4};
		NonLinearParameterTransforms transf = _fitter.getTransform(DoubleArray.copyOf(start));
		LeastSquareResultsWithTransform results = _fitter.solve(DoubleArray.copyOf(start), transf);
		double[] res = results.ModelParameters.toArray();
		double eps = 1e-6;
		assertEquals(ALPHA, res[0], eps);
		assertEquals(BETA, res[1], eps);
		assertEquals(RHO, res[2], eps);
		assertEquals(NU, res[3], eps);
		assertEquals(0.0, results.ChiSq, eps);
	  }

	  public virtual void testExactFitWithFixedBeta()
	  {
		DoubleArray start = DoubleArray.of(0.1, 0.5, 0.0, 0.3);
		BitArray @fixed = new BitArray();
		@fixed.Set(1, true);
		LeastSquareResultsWithTransform results = _fitter.solve(start, @fixed);
		double[] res = results.ModelParameters.toArray();
		double eps = 1e-6;
		assertEquals(ALPHA, res[0], eps);
		assertEquals(BETA, res[1], eps);
		assertEquals(RHO, res[2], eps);
		assertEquals(NU, res[3], eps);
		assertEquals(0.0, results.ChiSq, eps);

		// sensitivity to data
		DoubleMatrix sensitivity = results.ModelParameterSensitivityToData;
		double shiftFd = 1.0E-5;
		for (int i = 0; i < _cleanVols.Length; i++)
		{
		  double[] volBumpedP = _cleanVols.Clone();
		  volBumpedP[i] += shiftFd;
		  SabrModelFitter fitterP = new SabrModelFitter(F, DoubleArray.copyOf(STRIKES), TIME_TO_EXPIRY, DoubleArray.copyOf(volBumpedP), DoubleArray.copyOf(_errors), Model);
		  LeastSquareResultsWithTransform resultsBumpedP = fitterP.solve(start, @fixed);
		  DoubleArray parameterBumpedP = resultsBumpedP.ModelParameters;
		  double[] volBumpedM = _cleanVols.Clone();
		  volBumpedM[i] -= shiftFd;
		  SabrModelFitter fitterM = new SabrModelFitter(F, DoubleArray.copyOf(STRIKES), TIME_TO_EXPIRY, DoubleArray.copyOf(volBumpedM), DoubleArray.copyOf(_errors), Model);
		  LeastSquareResultsWithTransform resultsBumpedM = fitterM.solve(start, @fixed);
		  DoubleArray parameterBumpedM = resultsBumpedM.ModelParameters;
		  DoubleArray sensitivityColumnFd = parameterBumpedP.minus(parameterBumpedM).dividedBy(2 * shiftFd);
		  assertTrue(sensitivityColumnFd.equalWithTolerance(sensitivity.column(i), 1.0E-6));
		}
	  }

	  public virtual void testNoisyFitWithFixedBeta()
	  {
		DoubleArray start = DoubleArray.of(0.1, 0.5, 0.0, 0.3);
		BitArray @fixed = new BitArray();
		@fixed.Set(1, true);
		LeastSquareResultsWithTransform results = _nosiyFitter.solve(start, @fixed);
		double[] res = results.ModelParameters.toArray();
		double eps = 1e-2;
		assertEquals(ALPHA, res[0], eps);
		assertEquals(BETA, res[1], eps);
		assertEquals(RHO, res[2], eps);
		assertEquals(NU, res[3], eps);
		assertEquals(0.0, results.ChiSq, 10.0d);

		// sensitivity to data
		DoubleMatrix sensitivity = results.ModelParameterSensitivityToData;
		double shiftFd = 1.0E-5;
		for (int i = 0; i < _cleanVols.Length; i++)
		{
		  double[] volBumpedP = _noisyVols.Clone();
		  volBumpedP[i] += shiftFd;
		  SabrModelFitter fitterP = new SabrModelFitter(F, DoubleArray.copyOf(STRIKES), TIME_TO_EXPIRY, DoubleArray.copyOf(volBumpedP), DoubleArray.copyOf(_errors), Model);
		  LeastSquareResultsWithTransform resultsBumpedP = fitterP.solve(start, @fixed);
		  DoubleArray parameterBumpedP = resultsBumpedP.ModelParameters;
		  double[] volBumpedM = _noisyVols.Clone();
		  volBumpedM[i] -= shiftFd;
		  SabrModelFitter fitterM = new SabrModelFitter(F, DoubleArray.copyOf(STRIKES), TIME_TO_EXPIRY, DoubleArray.copyOf(volBumpedM), DoubleArray.copyOf(_errors), Model);
		  LeastSquareResultsWithTransform resultsBumpedM = fitterM.solve(start, @fixed);
		  DoubleArray parameterBumpedM = resultsBumpedM.ModelParameters;
		  DoubleArray sensitivityColumnFd = parameterBumpedP.minus(parameterBumpedM).dividedBy(2 * shiftFd);
		  assertTrue(sensitivityColumnFd.equalWithTolerance(sensitivity.column(i), 1.0E-2));
		}
	  }

	}

}