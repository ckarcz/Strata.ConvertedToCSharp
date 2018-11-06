using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using PiecewisePolynomialWithSensitivityFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialWithSensitivityFunction1D;

	/// <summary>
	/// Given a data set {xValues[i], yValues[i]}, interpolate {xValues[i], xValues[i] * yValues[i]} by a piecewise polynomial function. 
	/// The interpolation can be clamped at {xValuesClamped[j], xValuesClamped[j] * yValuesClamped[j]}, i.e., {xValuesClamped[j], yValuesClamped[j]}, 
	/// where the extra points can be inside or outside the data range. 
	/// By default right extrapolation is completed with a linear function, whereas default left extrapolation uses polynomial coefficients for the leftmost interval 
	/// and left linear extrapolation can be straightforwardly computed from the coefficients.
	/// This default setting is changed by adding extra node points outside the data range. 
	/// </summary>
	public class ProductPiecewisePolynomialInterpolator : PiecewisePolynomialInterpolator
	{
	  private readonly PiecewisePolynomialInterpolator _baseMethod;
	  private readonly double[] _xValuesClamped;
	  private readonly double[] _yValuesClamped;
	  private readonly bool _isClamped;
	  private static readonly PiecewisePolynomialWithSensitivityFunction1D FUNC = new PiecewisePolynomialWithSensitivityFunction1D();
	  private const double EPS = 1.0e-15;

	  /// <summary>
	  /// Construct the interpolator without clamped points. </summary>
	  /// <param name="baseMethod"> The base interpolator must not be itself </param>
	  public ProductPiecewisePolynomialInterpolator(PiecewisePolynomialInterpolator baseMethod)
	  {
		ArgChecker.notNull(baseMethod, "baseMethod");
		ArgChecker.isFalse(baseMethod is ProductPiecewisePolynomialInterpolator, "baseMethod should not be ProductPiecewisePolynomialInterpolator");
		_baseMethod = baseMethod;
		_xValuesClamped = null;
		_yValuesClamped = null;
		_isClamped = false;
	  }

	  /// <summary>
	  /// Construct the interpolator with clamped points. </summary>
	  /// <param name="baseMethod"> The base interpolator must be not be itself </param>
	  /// <param name="xValuesClamped"> X values of the clamped points </param>
	  /// <param name="yValuesClamped"> Y values of the clamped points </param>
	  public ProductPiecewisePolynomialInterpolator(PiecewisePolynomialInterpolator baseMethod, double[] xValuesClamped, double[] yValuesClamped)
	  {
		ArgChecker.notNull(baseMethod, "method");
		ArgChecker.notNull(xValuesClamped, "xValuesClamped");
		ArgChecker.notNull(yValuesClamped, "yValuesClamped");
		ArgChecker.isFalse(baseMethod is ProductPiecewisePolynomialInterpolator, "baseMethod should not be ProductPiecewisePolynomialInterpolator");
		int nExtraPoints = xValuesClamped.Length;
		ArgChecker.isTrue(yValuesClamped.Length == nExtraPoints, "xValuesClamped and yValuesClamped should be the same length");
		_baseMethod = baseMethod;
		_xValuesClamped = Arrays.copyOf(xValuesClamped, nExtraPoints);
		_yValuesClamped = Arrays.copyOf(yValuesClamped, nExtraPoints);
		_isClamped = true;
	  }

	  public override PiecewisePolynomialResult interpolate(double[] xValues, double[] yValues)
	  {
		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");
		ArgChecker.isTrue(xValues.Length == yValues.Length, "xValues length = yValues length");
		PiecewisePolynomialResult result;
		if (_isClamped)
		{
		  double[][] xyValuesAll = getDataTotal(xValues, yValues);
		  result = _baseMethod.interpolate(xyValuesAll[0], xyValuesAll[1]);
		}
		else
		{
		  double[] xyValues = getProduct(xValues, yValues);
		  result = _baseMethod.interpolate(xValues, xyValues);
		}
		return extrapolateByLinearFunction(result, xValues);
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
		PiecewisePolynomialResultsWithSensitivity result;
		if (_isClamped)
		{
		  double[][] xyValuesAll = getDataTotal(xValues, yValues);
		  result = _baseMethod.interpolateWithSensitivity(xyValuesAll[0], xyValuesAll[1]);
		}
		else
		{
		  double[] xyValues = getProduct(xValues, yValues);
		  result = _baseMethod.interpolateWithSensitivity(xValues, xyValues);
		}
		return (PiecewisePolynomialResultsWithSensitivity) extrapolateByLinearFunction(result, xValues);
	  }

	  /// <summary>
	  /// Left extrapolation by linear function unless extra node is added on the left 
	  /// </summary>
	  private PiecewisePolynomialResult extrapolateByLinearFunction(PiecewisePolynomialResult result, double[] xValues)
	  {
		int nIntervalsAll = result.NumberOfIntervals;
		double[] nodes = result.Knots.toArray();
		if (Math.Abs(xValues[xValues.Length - 1] - nodes[nIntervalsAll]) < EPS)
		{
		  double lastNodeX = nodes[nIntervalsAll];
		  double lastNodeY = FUNC.evaluate(result, lastNodeX).get(0);
		  double extraNode = 2.0 * nodes[nIntervalsAll] - nodes[nIntervalsAll - 1];
		  double extraDerivative = FUNC.differentiate(result, lastNodeX).get(0);
		  double[] newKnots = new double[nIntervalsAll + 2];
		  Array.Copy(nodes, 0, newKnots, 0, nIntervalsAll + 1);
		  newKnots[nIntervalsAll + 1] = extraNode; // dummy node, outside the data range
		  double[][] newCoefMatrix = new double[nIntervalsAll + 1][];
		  for (int i = 0; i < nIntervalsAll; ++i)
		  {
			newCoefMatrix[i] = Arrays.copyOf(result.CoefMatrix.row(i).toArray(), result.Order);
		  }
		  newCoefMatrix[nIntervalsAll] = new double[result.Order];
		  newCoefMatrix[nIntervalsAll][result.Order - 1] = lastNodeY;
		  newCoefMatrix[nIntervalsAll][result.Order - 2] = extraDerivative;
		  if (result is PiecewisePolynomialResultsWithSensitivity)
		  {
			PiecewisePolynomialResultsWithSensitivity resultCast = (PiecewisePolynomialResultsWithSensitivity) result;
			double[] extraSense = FUNC.nodeSensitivity(resultCast, lastNodeX).toArray();
			double[] extraSenseDer = FUNC.differentiateNodeSensitivity(resultCast, lastNodeX).toArray();
			DoubleMatrix[] newCoefSense = new DoubleMatrix[nIntervalsAll + 1];
			for (int i = 0; i < nIntervalsAll; ++i)
			{
			  newCoefSense[i] = resultCast.getCoefficientSensitivity(i);
			}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] extraCoefSense = new double[resultCast.Order][extraSense.Length];
			double[][] extraCoefSense = RectangularArrays.ReturnRectangularDoubleArray(resultCast.Order, extraSense.Length);
			extraCoefSense[resultCast.Order - 1] = Arrays.copyOf(extraSense, extraSense.Length);
			extraCoefSense[resultCast.Order - 2] = Arrays.copyOf(extraSenseDer, extraSenseDer.Length);
			newCoefSense[nIntervalsAll] = DoubleMatrix.copyOf(extraCoefSense);
			return new PiecewisePolynomialResultsWithSensitivity(DoubleArray.copyOf(newKnots), DoubleMatrix.copyOf(newCoefMatrix), resultCast.Order, 1, newCoefSense);
		  }
		  return new PiecewisePolynomialResult(DoubleArray.copyOf(newKnots), DoubleMatrix.copyOf(newCoefMatrix), result.Order, 1);
		}
		return result;
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
		double[] xyTotal = getProduct(xValuesTotal, yValuesTotal);
		return new double[][] {xValuesTotal, xyTotal};
	  }

	  private double[] getProduct(double[] x, double[] y)
	  {
		int n = x.Length;
		double[] xy = new double[n];
		for (int i = 0; i < n; ++i)
		{
		  xy[i] = x[i] * y[i];
		}
		return xy;
	  }
	}

}