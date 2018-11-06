/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.regression
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;

	/// 
	public class GeneralizedLeastSquaresRegression : LeastSquaresRegression
	{

	  private static CommonsMatrixAlgebra ALGEBRA = new CommonsMatrixAlgebra();

	  public override LeastSquaresRegressionResult regress(double[][] x, double[][] weights, double[] y, bool useIntercept)
	  {
		if (weights == null)
		{
		  throw new System.ArgumentException("Cannot perform GLS regression without an array of weights");
		}
		checkData(x, weights, y);
		double[][] dep = addInterceptVariable(x, useIntercept);
		double[] indep = new double[y.Length];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] wArray = new double[y.Length][y.Length];
		double[][] wArray = RectangularArrays.ReturnRectangularDoubleArray(y.Length, y.Length);
		for (int i = 0; i < y.Length; i++)
		{
		  indep[i] = y[i];
		  for (int j = 0; j < y.Length; j++)
		  {
			wArray[i][j] = weights[i][j];
		  }
		}
		DoubleMatrix matrix = DoubleMatrix.copyOf(dep);
		DoubleArray vector = DoubleArray.copyOf(indep);
		DoubleMatrix w = DoubleMatrix.copyOf(wArray);
		DoubleMatrix transpose = ALGEBRA.getTranspose(matrix);
		DoubleMatrix betasVector = (DoubleMatrix) ALGEBRA.multiply(ALGEBRA.multiply(ALGEBRA.multiply(ALGEBRA.getInverse(ALGEBRA.multiply(transpose, ALGEBRA.multiply(w, matrix))), transpose), w), vector);
		double[] yModel = base.writeArrayAsVector(((DoubleMatrix) ALGEBRA.multiply(matrix, betasVector)).toArray());
		double[] betas = base.writeArrayAsVector(betasVector.toArray());
		return getResultWithStatistics(x, y, betas, yModel, useIntercept);
	  }

	  private LeastSquaresRegressionResult getResultWithStatistics(double[][] x, double[] y, double[] betas, double[] yModel, bool useIntercept)
	  {

		int n = x.Length;
		double[] residuals = new double[n];
		for (int i = 0; i < n; i++)
		{
		  residuals[i] = y[i] - yModel[i];
		}
		return new WeightedLeastSquaresRegressionResult(betas, residuals, 0.0, null, 0.0, 0.0, null, null, useIntercept);
	  }

	}

}