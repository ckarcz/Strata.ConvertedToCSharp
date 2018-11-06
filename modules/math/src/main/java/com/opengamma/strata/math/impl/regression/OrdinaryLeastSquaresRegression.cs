using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.regression
{
	using TDistribution = org.apache.commons.math3.distribution.TDistribution;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;

	/// 
	//CSOFF: JavadocMethod
	public class OrdinaryLeastSquaresRegression : LeastSquaresRegression
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(OrdinaryLeastSquaresRegression));
	  private CommonsMatrixAlgebra _algebra = new CommonsMatrixAlgebra();

	  public override LeastSquaresRegressionResult regress(double[][] x, double[][] weights, double[] y, bool useIntercept)
	  {
		if (weights != null)
		{
		  log.info("Weights were provided for OLS regression: they will be ignored");
		}
		return regress(x, y, useIntercept);
	  }

	  public virtual LeastSquaresRegressionResult regress(double[][] x, double[] y, bool useIntercept)
	  {
		checkData(x, y);
		double[][] indep = addInterceptVariable(x, useIntercept);
		double[] dep = new double[y.Length];
		for (int i = 0; i < y.Length; i++)
		{
		  dep[i] = y[i];
		}
		DoubleMatrix matrix = DoubleMatrix.copyOf(indep);
		DoubleArray vector = DoubleArray.copyOf(dep);
		DoubleMatrix transpose = _algebra.getTranspose(matrix);
		DoubleMatrix betasVector = (DoubleMatrix) _algebra.multiply(_algebra.multiply(_algebra.getInverse(_algebra.multiply(transpose, matrix)), transpose), vector);
		double[] yModel = base.writeArrayAsVector(((DoubleMatrix) _algebra.multiply(matrix, betasVector)).toArray());
		double[] betas = base.writeArrayAsVector(betasVector.toArray());
		return getResultWithStatistics(x, y, betas, yModel, transpose, matrix, useIntercept);
	  }

	  private LeastSquaresRegressionResult getResultWithStatistics(double[][] x, double[] y, double[] betas, double[] yModel, DoubleMatrix transpose, DoubleMatrix matrix, bool useIntercept)
	  {

		double yMean = 0.0;
		foreach (double y1 in y)
		{
		  yMean += y1;
		}
		yMean /= y.Length;
		double totalSumOfSquares = 0.0;
		double errorSumOfSquares = 0.0;
		int n = x.Length;
		int k = betas.Length;
		double[] residuals = new double[n];
		double[] stdErrorBetas = new double[k];
		double[] tStats = new double[k];
		double[] pValues = new double[k];
		for (int i = 0; i < n; i++)
		{
		  totalSumOfSquares += (y[i] - yMean) * (y[i] - yMean);
		  residuals[i] = y[i] - yModel[i];
		  errorSumOfSquares += residuals[i] * residuals[i];
		}
		double regressionSumOfSquares = totalSumOfSquares - errorSumOfSquares;
		double[][] covarianceBetas = convertArray(_algebra.getInverse(_algebra.multiply(transpose, matrix)).toArray());
		double rSquared = regressionSumOfSquares / totalSumOfSquares;
		double adjustedRSquared = 1.0 - (1 - rSquared) * (n - 1.0) / (n - k);
		double meanSquareError = errorSumOfSquares / (n - k);
		TDistribution studentT = new TDistribution(n - k);
		for (int i = 0; i < k; i++)
		{
		  stdErrorBetas[i] = Math.Sqrt(meanSquareError * covarianceBetas[i][i]);
		  tStats[i] = betas[i] / stdErrorBetas[i];
		  pValues[i] = 1 - studentT.cumulativeProbability(Math.Abs(tStats[i]));
		}
		return new LeastSquaresRegressionResult(betas, residuals, meanSquareError, stdErrorBetas, rSquared, adjustedRSquared, tStats, pValues, useIntercept);
	  }

	}

}