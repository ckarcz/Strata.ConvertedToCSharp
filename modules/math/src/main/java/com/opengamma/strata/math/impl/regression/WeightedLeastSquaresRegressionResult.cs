/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.regression
{
	/// 
	//CSOFF: JavadocMethod
	public class WeightedLeastSquaresRegressionResult : LeastSquaresRegressionResult
	{

	  public WeightedLeastSquaresRegressionResult(LeastSquaresRegressionResult result) : base(result)
	  {
	  }

	  public WeightedLeastSquaresRegressionResult(double[] betas, double[] residuals, double meanSquareError, double[] standardErrorOfBeta, double rSquared, double rSquaredAdjusted, double[] tStats, double[] pValues, bool hasIntercept) : base(betas, residuals, meanSquareError, standardErrorOfBeta, rSquared, rSquaredAdjusted, tStats, pValues, hasIntercept)
	  {

	  }

	  public virtual double getWeightedPredictedValue(double[] x, double[] w)
	  {
		if (x == null)
		{
		  throw new System.ArgumentException("Variable array was null");
		}
		if (w == null)
		{
		  throw new System.ArgumentException("Weight array was null");
		}
		double[] betas = Betas;
		if (hasIntercept() && x.Length != betas.Length - 1 || x.Length != betas.Length)
		{
		  throw new System.ArgumentException("Number of variables did not match number used in regression");
		}
		if (x.Length != w.Length)
		{
		  throw new System.ArgumentException("Number of weights did not match number of variables");
		}
		double sum = 0;
		for (int i = 0; i < (hasIntercept() ? x.Length + 1 : x.Length); i++)
		{
		  if (hasIntercept())
		  {
			if (i == 0)
			{
			  sum += betas[0];
			}
			else
			{
			  sum += betas[i] * x[i - 1] * w[i - 1];
			}
		  }
		  else
		  {
			sum += betas[i] * x[i] * w[i];
		  }
		}
		return sum;
	  }
	}

}