using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Abstract class for interpolations based on 2d piecewise polynomial functions .
	/// </summary>
	public abstract class PiecewisePolynomialInterpolator2D
	{

	  /// <summary>
	  /// Given a set of data points (x0Values_i, x1Values_j, yValues_{ij}), 2d spline interpolation
	  /// is returned such that f(x0Values_i, x1Values_j) = yValues_{ij}. </summary>
	  /// <param name="x0Values">  the values </param>
	  /// <param name="x1Values">  the values </param>
	  /// <param name="yValues">  the values </param>
	  /// <returns> <seealso cref="PiecewisePolynomialResult2D"/> containing positions of knots in x0 direction,
	  ///   positions of knots in x1 direction, coefficients of interpolant, 
	  ///   number of intervals in x0 direction, number of intervals in x1 direction, order of polynomial function </returns>
	  public abstract PiecewisePolynomialResult2D interpolate(double[] x0Values, double[] x1Values, double[][] yValues);

	  /// <param name="x0Values">  the values </param>
	  /// <param name="x1Values">  the values </param>
	  /// <param name="yValues">  the values </param>
	  /// <param name="x0Keys">  the keys </param>
	  /// <param name="x1Keys">  the keys </param>
	  /// <returns> Values of 2D interpolant at (x0Key_i, x1Keys_j)  </returns>
	  public virtual DoubleMatrix interpolate(double[] x0Values, double[] x1Values, double[][] yValues, double[] x0Keys, double[] x1Keys)
	  {

		ArgChecker.notNull(x0Keys, "x0Keys");
		ArgChecker.notNull(x1Keys, "x1Keys");

		int n0Keys = x0Keys.Length;
		int n1Keys = x1Keys.Length;

		for (int i = 0; i < n0Keys; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(x0Keys[i]), "x0Keys containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(x0Keys[i]), "x0Keys containing Infinity");
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(x1Keys[i]), "x1Keys containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(x1Keys[i]), "x1Keys containing Infinity");
		}

		PiecewisePolynomialResult2D result = this.interpolate(x0Values, x1Values, yValues);

		DoubleArray knots0 = result.Knots0;
		DoubleArray knots1 = result.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[n0Keys][n1Keys];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(n0Keys, n1Keys);

		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
			int ind0 = 0;
			int ind1 = 0;

			for (int k = 1; k < nKnots0 - 1; ++k)
			{
			  if (x0Keys[i] >= knots0.get(k))
			  {
				ind0 = k;
			  }
			}
			for (int k = 1; k < nKnots1 - 1; ++k)
			{
			  if (x1Keys[j] >= knots1.get(k))
			  {
				ind1 = k;
			  }
			}
			res[i][j] = getValue(result.Coefs[ind0][ind1], x0Keys[i], x1Keys[j], knots0.get(ind0), knots1.get(ind1));
			ArgChecker.isFalse(double.IsInfinity(res[i][j]), "Too large input");
			ArgChecker.isFalse(double.IsNaN(res[i][j]), "Too large input");
		  }
		}

		return DoubleMatrix.copyOf(res);
	  }

	  /// <param name="x0Values">  the values </param>
	  /// <param name="x1Values">  the values </param>
	  /// <param name="yValues">  the values </param>
	  /// <param name="x0Key">  the key </param>
	  /// <param name="x1Key">  the key </param>
	  /// <returns> Value of 2D interpolant at (x0Key, x1Key)  </returns>
	  public virtual double interpolate(double[] x0Values, double[] x1Values, double[][] yValues, double x0Key, double x1Key)
	  {

		PiecewisePolynomialResult2D result = this.interpolate(x0Values, x1Values, yValues);
		ArgChecker.isFalse(double.IsNaN(x0Key), "x0Key containing NaN");
		ArgChecker.isFalse(double.IsInfinity(x0Key), "x0Key containing Infinity");
		ArgChecker.isFalse(double.IsNaN(x1Key), "x1Key containing NaN");
		ArgChecker.isFalse(double.IsInfinity(x1Key), "x1Key containing Infinity");

		DoubleArray knots0 = result.Knots0;
		DoubleArray knots1 = result.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();

		int ind0 = 0;
		int ind1 = 0;

		for (int k = 1; k < nKnots0 - 1; ++k)
		{
		  if (x0Key >= knots0.get(k))
		  {
			ind0 = k;
		  }
		}

		for (int i = 1; i < nKnots1 - 1; ++i)
		{
		  if (x1Key >= knots1.get(i))
		  {
			ind1 = i;
		  }
		}
		double res = getValue(result.Coefs[ind0][ind1], x0Key, x1Key, knots0.get(ind0), knots1.get(ind1));

		ArgChecker.isFalse(double.IsInfinity(res), "Too large input");
		ArgChecker.isFalse(double.IsNaN(res), "Too large input");

		return res;
	  }

	  /// <param name="coefMat">  the coefMat </param>
	  /// <param name="x0">  the x0 </param>
	  /// <param name="x1">  the x1 </param>
	  /// <param name="leftKnot0">  the leftKnot0 </param>
	  /// <param name="leftKnot1">  the leftKnot1 </param>
	  /// <returns> sum_{i=0}^{order0-1} sum_{j=0}^{order1-1} coefMat_{ij} (x0-leftKnots0)^{order0-1-i} (x1-leftKnots1)^{order0-1-j} </returns>
	  protected internal virtual double getValue(DoubleMatrix coefMat, double x0, double x1, double leftKnot0, double leftKnot1)
	  {

		int order0 = coefMat.rowCount();
		int order1 = coefMat.columnCount();
		double x0Mod = x0 - leftKnot0;
		double x1Mod = x1 - leftKnot1;
		double res = 0.0;

		for (int i = 0; i < order0; ++i)
		{
		  for (int j = 0; j < order1; ++j)
		  {
			res += coefMat.get(order0 - i - 1, order1 - j - 1) * Math.Pow(x0Mod, i) * Math.Pow(x1Mod, j);
		  }
		}

		return res;
	  }

	}

}