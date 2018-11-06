using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.regression
{
	using TDistribution = org.apache.commons.math3.distribution.TDistribution;
	using Array2DRowRealMatrix = org.apache.commons.math3.linear.Array2DRowRealMatrix;
	using DiagonalMatrix = org.apache.commons.math3.linear.DiagonalMatrix;
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;

	/// 
	//CSOFF: JavadocMethod
	public class WeightedLeastSquaresRegression : LeastSquaresRegression
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(WeightedLeastSquaresRegression));
	  private static CommonsMatrixAlgebra ALGEBRA = new CommonsMatrixAlgebra();

	  public override LeastSquaresRegressionResult regress(double[][] x, double[][] weights, double[] y, bool useIntercept)
	  {
		if (weights == null)
		{
		  throw new System.ArgumentException("Cannot perform WLS regression without an array of weights");
		}
		checkData(x, weights, y);
		log.info("Have a two-dimensional array for what should be a one-dimensional array of weights. " + "The weights used in this regression will be the diagonal elements only");
		double[] w = new double[weights.Length];
		for (int i = 0; i < w.Length; i++)
		{
		  w[i] = weights[i][i];
		}
		return regress(x, w, y, useIntercept);
	  }

	  public override LeastSquaresRegressionResult regress(double[][] x, double[] weights, double[] y, bool useIntercept)
	  {
		if (weights == null)
		{
		  throw new System.ArgumentException("Cannot perform WLS regression without an array of weights");
		}
		checkData(x, weights, y);
		double[][] dep = addInterceptVariable(x, useIntercept);
		double[] w = new double[weights.Length];
		for (int i = 0; i < y.Length; i++)
		{
		  w[i] = weights[i];
		}
		DoubleMatrix matrix = DoubleMatrix.copyOf(dep);
		DoubleArray vector = DoubleArray.copyOf(y);
		RealMatrix wDiag = new DiagonalMatrix(w);
		DoubleMatrix transpose = ALGEBRA.getTranspose(matrix);

		DoubleMatrix wDiagTimesMatrix = DoubleMatrix.ofUnsafe(wDiag.multiply(new Array2DRowRealMatrix(matrix.toArrayUnsafe())).Data);
		DoubleMatrix tmp = (DoubleMatrix) ALGEBRA.multiply(ALGEBRA.getInverse(ALGEBRA.multiply(transpose, wDiagTimesMatrix)), transpose);

		DoubleMatrix wTmpTimesDiag = DoubleMatrix.copyOf(wDiag.preMultiply(new Array2DRowRealMatrix(tmp.toArrayUnsafe())).Data);
		DoubleMatrix betasVector = (DoubleMatrix) ALGEBRA.multiply(wTmpTimesDiag, vector);
		double[] yModel = base.writeArrayAsVector(((DoubleMatrix) ALGEBRA.multiply(matrix, betasVector)).toArray());
		double[] betas = base.writeArrayAsVector(betasVector.toArray());
		return getResultWithStatistics(x, convertArray(wDiag.Data), y, betas, yModel, transpose, matrix, useIntercept);
	  }

	  private LeastSquaresRegressionResult getResultWithStatistics(double[][] x, double[][] w, double[] y, double[] betas, double[] yModel, DoubleMatrix transpose, DoubleMatrix matrix, bool useIntercept)
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
		double[] standardErrorsOfBeta = new double[k];
		double[] tStats = new double[k];
		double[] pValues = new double[k];
		for (int i = 0; i < n; i++)
		{
		  totalSumOfSquares += w[i][i] * (y[i] - yMean) * (y[i] - yMean);
		  residuals[i] = y[i] - yModel[i];
		  errorSumOfSquares += w[i][i] * residuals[i] * residuals[i];
		}
		double regressionSumOfSquares = totalSumOfSquares - errorSumOfSquares;
		double[][] covarianceBetas = convertArray(ALGEBRA.getInverse(ALGEBRA.multiply(transpose, matrix)).toArray());
		double rSquared = regressionSumOfSquares / totalSumOfSquares;
		double adjustedRSquared = 1.0 - (1 - rSquared) * (n - 1) / (n - k);
		double meanSquareError = errorSumOfSquares / (n - k);
		TDistribution studentT = new TDistribution(n - k);
		for (int i = 0; i < k; i++)
		{
		  standardErrorsOfBeta[i] = Math.Sqrt(meanSquareError * covarianceBetas[i][i]);
		  tStats[i] = betas[i] / standardErrorsOfBeta[i];
		  pValues[i] = 1 - studentT.cumulativeProbability(Math.Abs(tStats[i]));
		}
		return new WeightedLeastSquaresRegressionResult(betas, residuals, meanSquareError, standardErrorsOfBeta, rSquared, adjustedRSquared, tStats, pValues, useIntercept);
	  }
	}

}