/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.regression
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// 
	// CSOFF: JavadocMethod
	public abstract class LeastSquaresRegression
	{

	  public abstract LeastSquaresRegressionResult regress(double[][] x, double[][] weights, double[] y, bool useIntercept);

	  protected internal virtual void checkData(double[][] x, double[][] weights, double[] y)
	  {
		checkData(x, y);
		if (weights != null)
		{
		  if (weights.Length == 0)
		  {
			throw new System.ArgumentException("No data in weights array");
		  }
		  if (weights.Length != x.Length)
		  {
			throw new System.ArgumentException("Independent variable and weight arrays are not the same length");
		  }
		  int n = weights[0].Length;
		  foreach (double[] w in weights)
		  {
			if (w.Length != n)
			{
			  throw new System.ArgumentException("Need a rectangular array of weight");
			}
		  }
		}
	  }

	  protected internal virtual void checkData(double[][] x, double[] weights, double[] y)
	  {
		checkData(x, y);
		if (weights != null)
		{
		  if (weights.Length == 0)
		  {
			throw new System.ArgumentException("No data in weights array");
		  }
		  if (weights.Length != x.Length)
		  {
			throw new System.ArgumentException("Independent variable and weight arrays are not the same length");
		  }
		}
	  }

	  protected internal virtual void checkData(double[][] x, double[] y)
	  {
		if (x == null)
		{
		  throw new System.ArgumentException("Independent variable array was null");
		}
		if (y == null)
		{
		  throw new System.ArgumentException("Dependent variable array was null");
		}
		if (x.Length == 0)
		{
		  throw new System.ArgumentException("No data in independent variable array");
		}
		if (y.Length == 0)
		{
		  throw new System.ArgumentException("No data in dependent variable array");
		}
		if (x.Length != y.Length)
		{
		  throw new System.ArgumentException("Dependent and independent variable arrays are not the same length: have " + x.Length + " and " + y.Length);
		}
		int n = x[0].Length;
		foreach (double[] x1 in x)
		{
		  if (x1.Length != n)
		  {
			throw new System.ArgumentException("Need a rectangular array of independent variables");
		  }
		}
		if (y.Length <= x[0].Length)
		{
		  throw new System.ArgumentException("Insufficient data; there are " + y.Length + " variables but only " + x[0].Length + " data points");
		}
	  }

	  protected internal virtual double[][] addInterceptVariable(double[][] x, bool useIntercept)
	  {
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] result = useIntercept ? new double[x.Length][x[0].Length + 1] : new double[x.Length][x[0].Length];
		double[][] result = useIntercept ? RectangularArrays.ReturnRectangularDoubleArray(x.Length, x[0].Length + 1) : RectangularArrays.ReturnRectangularDoubleArray(x.Length, x[0].Length);
		for (int i = 0; i < x.Length; i++)
		{
		  if (useIntercept)
		  {
			result[i][0] = 1.0;
			for (int j = 1; j < x[0].Length + 1; j++)
			{
			  result[i][j] = x[i][j - 1];
			}
		  }
		  else
		  {
			for (int j = 0; j < x[0].Length; j++)
			{
			  result[i][j] = x[i][j];
			}
		  }
		}
		return result;
	  }

	  protected internal virtual double[][] convertArray(double[][] x)
	  {
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] result = new double[x.Length][x[0].Length];
		double[][] result = RectangularArrays.ReturnRectangularDoubleArray(x.Length, x[0].Length);
		for (int i = 0; i < result.Length; i++)
		{
		  for (int j = 0; j < result[0].Length; j++)
		  {
			result[i][j] = x[i][j];
		  }
		}
		return result;
	  }

	  protected internal virtual double[] convertArray(double[] x)
	  {
		double[] result = new double[x.Length];
		for (int i = 0; i < result.Length; i++)
		{
		  result[i] = x[i];
		}
		return result;
	  }

	  protected internal virtual double[] writeArrayAsVector(double[][] x)
	  {
		ArgChecker.isTrue(x[0].Length == 1, "Trying to convert matrix to vector");
		double[] result = new double[x.Length];
		for (int i = 0; i < x.Length; i++)
		{
		  result[i] = x[i][0];
		}
		return result;
	  }

	}

}