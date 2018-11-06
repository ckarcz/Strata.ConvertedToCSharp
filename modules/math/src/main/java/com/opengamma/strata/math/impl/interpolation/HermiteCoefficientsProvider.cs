using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Hermite interpolation is determined if one specifies first derivatives for a cubic
	/// interpolant and first and second derivatives for a quintic interpolant.
	/// </summary>
	public class HermiteCoefficientsProvider
	{

	  /// <param name="values"> (yValues_i) </param>
	  /// <param name="intervals"> (xValues_{i+1} - xValues_{i}) </param>
	  /// <param name="slopes"> (yValues_{i+1} - yValues_{i})/(xValues_{i+1} - xValues_{i}) </param>
	  /// <param name="first"> First derivatives at xValues_i </param>
	  /// <returns> Coefficient matrix whose i-th row vector is { a_n, a_{n-1}, ...} for the i-th interval,
	  ///   where a_n, a_{n-1},... are coefficients of f(x) = a_n (x-x_i)^n + a_{n-1} (x-x_i)^{n-1} + .... with n=3 </returns>
	  public virtual double[][] solve(double[] values, double[] intervals, double[] slopes, double[] first)
	  {
		int nInt = intervals.Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nInt][4];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nInt, 4);
		for (int i = 0; i < nInt; ++i)
		{
		  Arrays.fill(res[i], 0.0);
		}

		for (int i = 0; i < nInt; ++i)
		{
		  res[i][3] = values[i];
		  res[i][2] = first[i];
		  res[i][1] = (3.0 * slopes[i] - first[i + 1] - 2.0 * first[i]) / intervals[i];
		  res[i][0] = -(2.0 * slopes[i] - first[i + 1] - first[i]) / intervals[i] / intervals[i];
		}

		return res;
	  }

	  /// <param name="values"> Y values of data </param>
	  /// <param name="intervals"> (xValues_{i+1} - xValues_{i}) </param>
	  /// <param name="slopes"> (yValues_{i+1} - yValues_{i})/(xValues_{i+1} - xValues_{i}) </param>
	  /// <param name="slopeSensitivity"> Derivative values of slope with respect to yValues </param>
	  /// <param name="firstWithSensitivity"> First derivative values at xValues_i and their yValues dependencies </param>
	  /// <returns> Coefficient matrix and its node dependencies </returns>
	  public virtual DoubleMatrix[] solveWithSensitivity(double[] values, double[] intervals, double[] slopes, double[][] slopeSensitivity, DoubleArray[] firstWithSensitivity)
	  {

		int nData = values.Length;
		double[] first = firstWithSensitivity[0].toArray();
		DoubleMatrix[] res = new DoubleMatrix[nData];

		double[][] coef = solve(values, intervals, slopes, first);
		res[0] = DoubleMatrix.copyOf(coef);

		for (int i = 0; i < nData - 1; ++i)
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] coefSense = new double[4][nData];
		  double[][] coefSense = RectangularArrays.ReturnRectangularDoubleArray(4, nData);
		  Arrays.fill(coefSense[3], 0.0);
		  coefSense[3][i] = 1.0;
		  for (int k = 0; k < nData; ++k)
		  {
			coefSense[0][k] = -(2.0 * slopeSensitivity[i][k] - firstWithSensitivity[i + 2].get(k) - firstWithSensitivity[i + 1].get(k)) / intervals[i] / intervals[i];
			coefSense[1][k] = (3.0 * slopeSensitivity[i][k] - firstWithSensitivity[i + 2].get(k) - 2.0 * firstWithSensitivity[i + 1].get(k)) / intervals[i];
			coefSense[2][k] = firstWithSensitivity[i + 1].get(k);
		  }
		  res[i + 1] = DoubleMatrix.copyOf(coefSense);
		}

		return res;
	  }

	  /// <param name="values"> (yValues_i) </param>
	  /// <param name="intervals"> (xValues_{i+1} - xValues_{i}) </param>
	  /// <param name="slopes"> (yValues_{i+1} - yValues_{i})/(xValues_{i+1} - xValues_{i}) </param>
	  /// <param name="first"> First derivatives at xValues_i </param>
	  /// <param name="second"> Second derivatives at xValues_i </param>
	  /// <returns> Coefficient matrix whose i-th row vector is { a_n, a_{n-1}, ...} for the i-th interval,
	  ///   where a_n, a_{n-1},... are coefficients of f(x) = a_n (x-x_i)^n + a_{n-1} (x-x_i)^{n-1} + .... with n=5 </returns>
	  public virtual double[][] solve(double[] values, double[] intervals, double[] slopes, double[] first, double[] second)
	  {
		int nInt = intervals.Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nInt][6];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nInt, 6);
		for (int i = 0; i < nInt; ++i)
		{
		  Arrays.fill(res[i], 0.0);
		}

		for (int i = 0; i < nInt; ++i)
		{
		  res[i][5] = values[i];
		  res[i][4] = first[i];
		  res[i][3] = 0.5 * second[i];
		  res[i][2] = 0.5 * (second[i + 1] - 3.0 * second[i]) / intervals[i] + 2.0 * (5.0 * slopes[i] - 3.0 * first[i] - 2.0 * first[i + 1]) / intervals[i] / intervals[i];
		  res[i][1] = 0.5 * (3.0 * second[i] - 2.0 * second[i + 1]) / intervals[i] / intervals[i] + (8.0 * first[i] + 7.0 * first[i + 1] - 15.0 * slopes[i]) / intervals[i] / intervals[i] / intervals[i];
		  res[i][0] = 0.5 * (second[i + 1] - second[i]) / intervals[i] / intervals[i] / intervals[i] + 3.0 * (2.0 * slopes[i] - first[i + 1] - first[i]) / intervals[i] / intervals[i] / intervals[i] / intervals[i];
		}

		return res;
	  }

	  /// 
	  /// <param name="values"> (yValues_i) </param>
	  /// <param name="intervals"> (xValues_{i+1} - xValues_{i}) </param>
	  /// <param name="slopes"> (yValues_{i+1} - yValues_{i})/(xValues_{i+1} - xValues_{i}) </param>
	  /// <param name="slopeSensitivity"> Derivative values of slope with respect to yValues </param>
	  /// <param name="firstWithSensitivity"> First derivative values at xValues_i and their yValues dependencies </param>
	  /// <param name="secondWithSensitivity"> Second derivative values at xValues_i and their yValues dependencies </param>
	  /// <returns> Coefficient matrix and its node dependencies </returns>
	  public virtual DoubleMatrix[] solveWithSensitivity(double[] values, double[] intervals, double[] slopes, double[][] slopeSensitivity, DoubleArray[] firstWithSensitivity, DoubleArray[] secondWithSensitivity)
	  {

		int nData = values.Length;
		double[] first = firstWithSensitivity[0].toArray();
		double[] second = secondWithSensitivity[0].toArray();
		DoubleMatrix[] res = new DoubleMatrix[nData];

		double[][] coef = solve(values, intervals, slopes, first, second);
		res[0] = DoubleMatrix.copyOf(coef);

		for (int i = 0; i < nData - 1; ++i)
		{
		  double interval = intervals[i];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] coefSense = new double[6][nData];
		  double[][] coefSense = RectangularArrays.ReturnRectangularDoubleArray(6, nData);
		  Arrays.fill(coefSense[5], 0.0);
		  coefSense[5][i] = 1.0;
		  for (int k = 0; k < nData; ++k)
		  {
			double cs0b = 2d * slopeSensitivity[i][k] - firstWithSensitivity[i + 2].get(k) - firstWithSensitivity[i + 1].get(k);
			double cs0a = secondWithSensitivity[i + 2].get(k) - secondWithSensitivity[i + 1].get(k);
			coefSense[0][k] = 0.5 * cs0a / interval / interval / interval + 3d * cs0b / interval / interval / interval / interval;

			double cs1a = 3d * secondWithSensitivity[i + 1].get(k) - 2d * secondWithSensitivity[i + 2].get(k);
			double cs1b = 8d * firstWithSensitivity[i + 1].get(k) + 7d * firstWithSensitivity[i + 2].get(k) - 15d * slopeSensitivity[i][k];
			coefSense[1][k] = 0.5 * cs1a / interval / interval + cs1b / interval / interval / interval;

			double cs2a = secondWithSensitivity[i + 2].get(k) - 3d * secondWithSensitivity[i + 1].get(k);
			double cs2b = 5d * slopeSensitivity[i][k] - 3d * firstWithSensitivity[i + 1].get(k) - 2d * firstWithSensitivity[i + 2].get(k);
			coefSense[2][k] = 0.5 * cs2a / interval + 2.0 * cs2b / interval / interval;

			coefSense[3][k] = 0.5 * secondWithSensitivity[i + 1].get(k);
			coefSense[4][k] = firstWithSensitivity[i + 1].get(k);
		  }
		  res[i + 1] = DoubleMatrix.copyOf(coefSense);
		}

		return res;
	  }

	  /// <param name="xValues"> The x values </param>
	  /// <returns> Intervals of xValues, ( xValues_{i+1} - xValues_i ) </returns>
	  public virtual double[] intervalsCalculator(double[] xValues)
	  {

		int nDataPts = xValues.Length;
		double[] intervals = new double[nDataPts - 1];

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  intervals[i] = xValues[i + 1] - xValues[i];
		}

		return intervals;
	  }

	  /// <param name="yValues"> Y values of data </param>
	  /// <param name="intervals"> Intervals of x data </param>
	  /// <returns> ( yValues_{i+1} - yValues_i )/( xValues_{i+1} - xValues_i ) </returns>
	  public virtual double[] slopesCalculator(double[] yValues, double[] intervals)
	  {

		int nDataPts = yValues.Length;
		double[] slopes = new double[nDataPts - 1];

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  slopes[i] = (yValues[i + 1] - yValues[i]) / intervals[i];
		}

		return slopes;
	  }

	  /// <summary>
	  /// Derivative values of slopes_i with respect to yValues_j, s_{ij}. </summary>
	  /// <param name="intervals"> Intervals of x data </param>
	  /// <returns> The matrix s_{ij} </returns>
	  public virtual double[][] slopeSensitivityCalculator(double[] intervals)
	  {
		int nDataPts = intervals.Length + 1;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nDataPts - 1][nDataPts];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nDataPts - 1, nDataPts);

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  Arrays.fill(res[i], 0.0);
		  res[i][i] = -1.0 / intervals[i];
		  res[i][i + 1] = 1.0 / intervals[i];
		}
		return res;
	  }

	  /// <param name="ints1"> The first interval </param>
	  /// <param name="ints2"> The second interval </param>
	  /// <param name="slope1"> The first gradient </param>
	  /// <param name="slope2"> The second gradient </param>
	  /// <returns> Value of derivative at each endpoint </returns>
	  public virtual double endpointDerivatives(double ints1, double ints2, double slope1, double slope2)
	  {
		double val = (2.0 * ints1 + ints2) * slope1 / (ints1 + ints2) - ints1 * slope2 / (ints1 + ints2);

		if (Math.Sign(val) != Math.Sign(slope1))
		{
		  return 0.0;
		}
		if (Math.Sign(slope1) != Math.Sign(slope2) && Math.Abs(val) > 3.0 * Math.Abs(slope1))
		{
		  return 3.0 * slope1;
		}
		return val;
	  }
	}

}