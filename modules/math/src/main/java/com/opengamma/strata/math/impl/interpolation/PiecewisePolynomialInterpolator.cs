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
	/// Abstract class for interpolations based on piecewise polynomial functions .
	/// </summary>
	public abstract class PiecewisePolynomialInterpolator
	{

	  /// <summary>
	  /// Interpolate.
	  /// </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <returns> <seealso cref="PiecewisePolynomialResult"/> containing knots, coefficients of piecewise polynomials, number of intervals, degree of polynomials, dimension of spline </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public abstract PiecewisePolynomialResult interpolate(final double[] xValues, final double[] yValues);
	  public abstract PiecewisePolynomialResult interpolate(double[] xValues, double[] yValues);

	  /// <summary>
	  /// Interpolate.
	  /// </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValuesMatrix"> Y values of data </param>
	  /// <returns> Coefficient matrix whose i-th row vector is {a_n, a_{n-1}, ... } of f(x) = a_n * (x-x_i)^n + a_{n-1} * (x-x_i)^{n-1} +... for the i-th interval </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public abstract PiecewisePolynomialResult interpolate(final double[] xValues, final double[][] yValuesMatrix);
	  public abstract PiecewisePolynomialResult interpolate(double[] xValues, double[][] yValuesMatrix);

	  /// <summary>
	  /// Interpolate.
	  /// </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <param name="xKey">  the key </param>
	  /// <returns> value of the underlying cubic spline function at the value of x </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public double interpolate(final double[] xValues, final double[] yValues, final double xKey)
	  public virtual double interpolate(double[] xValues, double[] yValues, double xKey)
	  {

		ArgChecker.isFalse(double.IsNaN(xKey), "xKey containing NaN");
		ArgChecker.isFalse(double.IsInfinity(xKey), "xKey containing Infinity");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult result = this.interpolate(xValues, yValues);
		PiecewisePolynomialResult result = this.interpolate(xValues, yValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray knots = result.getKnots();
		DoubleArray knots = result.Knots;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nKnots = knots.size();
		int nKnots = knots.size();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefMatrix = result.getCoefMatrix();
		DoubleMatrix coefMatrix = result.CoefMatrix;

		double res = 0.0;

		int indicator = 0;
		if (xKey < knots.get(1))
		{
		  indicator = 0;
		}
		else
		{
		  for (int i = 1; i < nKnots - 1; ++i)
		  {
			if (knots.get(i) <= xKey)
			{
			  indicator = i;
			}
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray coefs = coefMatrix.row(indicator);
		DoubleArray coefs = coefMatrix.row(indicator);
		res = getValue(coefs, xKey, knots.get(indicator));
		ArgChecker.isFalse(double.IsInfinity(res), "Too large input");
		ArgChecker.isFalse(double.IsNaN(res), "Too large input");

		return res;
	  }

	  /// <summary>
	  /// Interpolate.
	  /// </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <param name="xKeys">  the keys </param>
	  /// <returns> Values of the underlying cubic spline function at the values of x </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public com.opengamma.strata.collect.array.DoubleArray interpolate(final double[] xValues, final double[] yValues, final double[] xKeys)
	  public virtual DoubleArray interpolate(double[] xValues, double[] yValues, double[] xKeys)
	  {
		ArgChecker.notNull(xKeys, "xKeys");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int keyLength = xKeys.length;
		int keyLength = xKeys.Length;
		for (int i = 0; i < keyLength; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xKeys[i]), "xKeys containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xKeys[i]), "xKeys containing Infinity");
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PiecewisePolynomialResult result = this.interpolate(xValues, yValues);
		PiecewisePolynomialResult result = this.interpolate(xValues, yValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray knots = result.getKnots();
		DoubleArray knots = result.Knots;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nKnots = knots.size();
		int nKnots = knots.size();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefMatrix = result.getCoefMatrix();
		DoubleMatrix coefMatrix = result.CoefMatrix;

		double[] res = new double[keyLength];

		for (int j = 0; j < keyLength; ++j)
		{
		  int indicator = 0;
		  if (xKeys[j] < knots.get(1))
		  {
			indicator = 0;
		  }
		  else
		  {
			for (int i = 1; i < nKnots - 1; ++i)
			{
			  if (knots.get(i) <= xKeys[j])
			  {
				indicator = i;
			  }
			}
		  }
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray coefs = coefMatrix.row(indicator);
		  DoubleArray coefs = coefMatrix.row(indicator);
		  res[j] = getValue(coefs, xKeys[j], knots.get(indicator));
		  ArgChecker.isFalse(double.IsInfinity(res[j]), "Too large input");
		  ArgChecker.isFalse(double.IsNaN(res[j]), "Too large input");
		}

		return DoubleArray.copyOf(res);
	  }

	  /// <summary>
	  /// Interpolate.
	  /// </summary>
	  /// <param name="xValues">  the values </param>
	  /// <param name="yValues">  the values </param>
	  /// <param name="xMatrix">  the matrix </param>
	  /// <returns> Values of the underlying cubic spline function at the values of x </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public com.opengamma.strata.collect.array.DoubleMatrix interpolate(final double[] xValues, final double[] yValues, final double[][] xMatrix)
	  public virtual DoubleMatrix interpolate(double[] xValues, double[] yValues, double[][] xMatrix)
	  {
		ArgChecker.notNull(xMatrix, "xMatrix");

		DoubleMatrix matrix = DoubleMatrix.copyOf(xMatrix);
		return DoubleMatrix.ofArrayObjects(xMatrix.Length, xMatrix[0].Length, i => interpolate(xValues, yValues, matrix.rowArray(i)));

	  }

	  /// <summary>
	  /// Interpolate.
	  /// </summary>
	  /// <param name="xValues">  the values </param>
	  /// <param name="yValuesMatrix">  the matrix </param>
	  /// <param name="x">  the x </param>
	  /// <returns> Values of the underlying cubic spline functions interpolating {yValuesMatrix.RowVectors} at the value of x </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public com.opengamma.strata.collect.array.DoubleArray interpolate(final double[] xValues, final double[][] yValuesMatrix, final double x)
	  public virtual DoubleArray interpolate(double[] xValues, double[][] yValuesMatrix, double x)
	  {
		DoubleMatrix matrix = DoubleMatrix.copyOf(yValuesMatrix);
		return DoubleArray.of(matrix.rowCount(), i => interpolate(xValues, matrix.rowArray(i), x));
	  }

	  /// <summary>
	  /// Interpolate.
	  /// </summary>
	  /// <param name="xValues">  the values </param>
	  /// <param name="yValuesMatrix">  the matrix </param>
	  /// <param name="x">  the s </param>
	  /// <returns> Values of the underlying cubic spline functions interpolating {yValuesMatrix.RowVectors} at the values of x </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public com.opengamma.strata.collect.array.DoubleMatrix interpolate(final double[] xValues, final double[][] yValuesMatrix, final double[] x)
	  public virtual DoubleMatrix interpolate(double[] xValues, double[][] yValuesMatrix, double[] x)
	  {
		ArgChecker.notNull(x, "x");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix matrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(yValuesMatrix);
		DoubleMatrix matrix = DoubleMatrix.copyOf(yValuesMatrix);
		return DoubleMatrix.ofArrayObjects(yValuesMatrix.Length, x.Length, i => interpolate(xValues, matrix.rowArray(i), x));
	  }

	  /// <summary>
	  /// Interpolate.
	  /// </summary>
	  /// <param name="xValues">  the values </param>
	  /// <param name="yValuesMatrix">  the matrix </param>
	  /// <param name="xMatrix">  the matrix </param>
	  /// <returns> Values of the underlying cubic spline functions interpolating {yValuesMatrix.RowVectors} at the values of xMatrix </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public com.opengamma.strata.collect.array.DoubleMatrix[] interpolate(final double[] xValues, final double[][] yValuesMatrix, final double[][] xMatrix)
	  public virtual DoubleMatrix[] interpolate(double[] xValues, double[][] yValuesMatrix, double[][] xMatrix)
	  {
		ArgChecker.notNull(xMatrix, "xMatrix");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int keyColumn = xMatrix[0].length;
		int keyColumn = xMatrix[0].Length;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix matrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(xMatrix);
		DoubleMatrix matrix = DoubleMatrix.copyOf(xMatrix);

		DoubleMatrix[] resMatrix2D = new DoubleMatrix[keyColumn];

		for (int i = 0; i < keyColumn; ++i)
		{
		  resMatrix2D[i] = interpolate(xValues, yValuesMatrix, matrix.columnArray(i));
		}

		return resMatrix2D;
	  }

	  /// <summary>
	  /// Derive interpolant on {xValues_i, yValues_i} and (yValues) node sensitivity. </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <returns> <seealso cref="PiecewisePolynomialResultsWithSensitivity"/> </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public abstract PiecewisePolynomialResultsWithSensitivity interpolateWithSensitivity(final double[] xValues, final double[] yValues);
	  public abstract PiecewisePolynomialResultsWithSensitivity interpolateWithSensitivity(double[] xValues, double[] yValues);

	  /// <summary>
	  /// Hyman filter modifies derivative values at knot points which are initially computed by a "primary" interpolator. </summary>
	  /// <returns> The primary interpolator for Hyman filter, interpolation method itself for other interpolators </returns>
	  public virtual PiecewisePolynomialInterpolator PrimaryMethod
	  {
		  get
		  {
			return this;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <param name="coefs">  {a_n,a_{n-1},...} of f(x) = a_n x^{n} + a_{n-1} x^{n-1} + .... </param>
	  /// <param name="x">  the x </param>
	  /// <param name="leftknot">  the knot specifying underlying interpolation function </param>
	  /// <returns> the value of the underlying interpolation function at the value of x </returns>
	  protected internal virtual double getValue(DoubleArray coefs, double x, double leftknot)
	  {
		// needs to delegate as method is protected
		return getValue(coefs.toArrayUnsafe(), x, leftknot);
	  }

	  /// <param name="coefs">  {a_n,a_{n-1},...} of f(x) = a_n x^{n} + a_{n-1} x^{n-1} + .... </param>
	  /// <param name="x">  the x </param>
	  /// <param name="leftknot">  the knot specifying underlying interpolation function </param>
	  /// <returns> the value of the underlying interpolation function at the value of x </returns>
	  protected internal virtual double getValue(double[] coefs, double x, double leftknot)
	  {
		int nCoefs = coefs.Length;
		double s = x - leftknot;
		double res = coefs[0];
		for (int i = 1; i < nCoefs; i++)
		{
		  res *= s;
		  res += coefs[i];
		}
		return res;
	  }

	}

}