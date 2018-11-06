using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;

	/// <summary>
	/// Piecewise polynomial interpolator clamped at specified points. 
	/// <para>
	/// The clamped points are regarded as 'normal' data points in the interpolation result, i.e., 
	/// {@code PiecewisePolynomialResult} or {@code PiecewisePolynomialResultsWithSensitivity}.  
	/// A consequence of this is, for example, that the coefficient sensitivities involve the sensitivities to clamped points.
	/// </para>
	/// </summary>
	public class ClampedPiecewisePolynomialInterpolator : PiecewisePolynomialInterpolator
	{
	  private readonly PiecewisePolynomialInterpolator _baseMethod;
	  private readonly double[] _xValuesClamped;
	  private readonly double[] _yValuesClamped;

	  /// <summary>
	  /// Construct the interpolator with clamped points.
	  /// </summary>
	  /// <param name="baseMethod"> The base interpolator must be not be itself </param>
	  /// <param name="xValuesClamped"> X values of the clamped points </param>
	  /// <param name="yValuesClamped"> Y values of the clamped points </param>
	  public ClampedPiecewisePolynomialInterpolator(PiecewisePolynomialInterpolator baseMethod, double[] xValuesClamped, double[] yValuesClamped)
	  {
		ArgChecker.notNull(baseMethod, "method");
		ArgChecker.notEmpty(xValuesClamped, "xValuesClamped");
		ArgChecker.notEmpty(yValuesClamped, "yValuesClamped");
		ArgChecker.isFalse(baseMethod is ProductPiecewisePolynomialInterpolator, "baseMethod should not be ProductPiecewisePolynomialInterpolator");
		int nExtraPoints = xValuesClamped.Length;
		ArgChecker.isTrue(yValuesClamped.Length == nExtraPoints, "xValuesClamped and yValuesClamped should be the same length");
		_baseMethod = baseMethod;
		_xValuesClamped = Arrays.copyOf(xValuesClamped, nExtraPoints);
		_yValuesClamped = Arrays.copyOf(yValuesClamped, nExtraPoints);
	  }

	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[] yValues)
	  {
		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");
		ArgChecker.isTrue(xValues.Length == yValues.Length, "xValues length = yValues length");
		double[][] xyValuesAll = getDataTotal(xValues, yValues);
		return _baseMethod.interpolate(xyValuesAll[0], xyValuesAll[1]);
	  }

	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[][] yValuesMatrix)
	  {
		throw new System.NotSupportedException("Use 1D interpolation method");
	  }

	  public override PiecewisePolynomialResultsWithSensitivity interpolateWithSensitivity(double[] xValues, double[] yValues)
	  {
		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");
		ArgChecker.isTrue(xValues.Length == yValues.Length, "xValues length = yValues length");
		double[][] xyValuesAll = getDataTotal(xValues, yValues);
		return _baseMethod.interpolateWithSensitivity(xyValuesAll[0], xyValuesAll[1]);
	  }

	  public override PiecewisePolynomialInterpolator PrimaryMethod
	  {
		  get
		  {
			return _baseMethod;
		  }
	  }

	  private double[][] getDataTotal(double[] xData, double[] yData)
	  {
		int nExtraPoints = _xValuesClamped.Length;
		int nData = xData.Length;
		int nTotal = nExtraPoints + nData;
		double[] xValuesTotal = new double[nTotal];
		double[] yValuesTotal = new double[nTotal];
		Array.Copy(xData, 0, xValuesTotal, 0, nData);
		Array.Copy(yData, 0, yValuesTotal, 0, nData);
		Array.Copy(_xValuesClamped, 0, xValuesTotal, nData, nExtraPoints);
		Array.Copy(_yValuesClamped, 0, yValuesTotal, nData, nExtraPoints);
		DoubleArrayMath.sortPairs(xValuesTotal, yValuesTotal);
		return new double[][] {xValuesTotal, yValuesTotal};
	  }

	}

}