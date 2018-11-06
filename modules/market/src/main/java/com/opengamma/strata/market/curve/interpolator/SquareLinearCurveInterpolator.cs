using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// The interpolation is linear on y^2. The interpolator is used for interpolation on variance for options.
	/// All values of y must be positive.
	/// </summary>
	[Serializable]
	internal sealed class SquareLinearCurveInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "SquareLinear";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly CurveInterpolator INSTANCE = new SquareLinearCurveInterpolator();
	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// Level below which the value is consider to be 0.
	  /// </summary>
	  private const double EPS = 1.0E-10;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private SquareLinearCurveInterpolator()
	  {
	  }

	  // resolve instance
	  private object readResolve()
	  {
		return INSTANCE;
	  }

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return NAME;
		  }
	  }

	  public BoundCurveInterpolator bind(DoubleArray xValues, DoubleArray yValues)
	  {
		return new Bound(xValues, yValues);
	  }

	  //-----------------------------------------------------------------------
	  public override string ToString()
	  {
		return NAME;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Bound interpolator.
	  /// </summary>
	  internal class Bound : AbstractBoundCurveInterpolator
	  {
		internal readonly double[] xValues;
		internal readonly double[] yValues;
		internal readonly int dataSize;

		internal Bound(DoubleArray xValues, DoubleArray yValues) : base(xValues, yValues)
		{
		  this.xValues = xValues.toArrayUnsafe();
		  this.yValues = yValues.toArrayUnsafe();
		  this.dataSize = xValues.size();
		}

		internal Bound(Bound @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight) : base(@base, extrapolatorLeft, extrapolatorRight)
		{
		  this.xValues = @base.xValues;
		  this.yValues = @base.yValues;
		  this.dataSize = xValues.Length;
		}

		//-------------------------------------------------------------------------
		protected internal override double doInterpolate(double xValue)
		{
		  // x-value is less than the x-value of the last node (lowerIndex < intervalCount)
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  double x1 = xValues[lowerIndex];
		  double y1 = yValues[lowerIndex];

		  int higherIndex = lowerIndex + 1;
		  double x2 = xValues[higherIndex];
		  double y2 = yValues[higherIndex];

		  double w = (x2 - xValue) / (x2 - x1);
		  double y21 = y1 * y1;
		  double y22 = y2 * y2;
		  double ySq = w * y21 + (1.0 - w) * y22;
		  return Math.Sqrt(ySq);
		}

		protected internal override double doInterpolateFromExtrapolator(double xValue)
		{
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  // check if x-value is at the last node
		  if (lowerIndex == dataSize - 1)
		  {
			// if value is at last node, calculate the gradient from the previous interval
			lowerIndex--;
		  }
		  double x1 = xValues[lowerIndex];
		  double y1 = yValues[lowerIndex];

		  int higherIndex = lowerIndex + 1;
		  double x2 = xValues[higherIndex];
		  double y2 = yValues[higherIndex];

		  double w = (x2 - xValue) / (x2 - x1);
		  double y21 = y1 * y1;
		  double y22 = y2 * y2;
		  double ySq = w * y21 + (1.0 - w) * y22;
		  return Math.Sqrt(ySq);
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  int index;
		  // check if x-value is at the last node
		  if (lowerIndex == dataSize - 1)
		  {
			index = dataSize - 2;
		  }
		  else
		  {
			index = lowerIndex;
		  }

		  double x1 = xValues[index];
		  double y1 = yValues[index];
		  double x2 = xValues[index + 1];
		  double y2 = yValues[index + 1];

		  if ((y1 < EPS) && (y2 >= EPS) && (xValue - x1) < EPS)
		  { // On one vertex with value 0, other vertex not 0
			throw new System.ArgumentException("ask for first derivative on a value without derivative; value " + xValue + " is close to vertex " + x1 + " and value at vertex is " + y1);
		  }
		  if ((y2 < EPS) && (y1 >= EPS) && (x2 - xValue) < EPS)
		  { // On one vertex with value 0, other vertex not 0
			throw new System.ArgumentException("ask for first derivative on a value without derivative; value " + xValue + " is close to vertex " + x2 + " and value at vertex is " + y2);
		  }
		  if ((y1 < EPS) && (y2 < EPS))
		  { // Both vertices have 0 value, return 0.
			return 0.0;
		  }

		  double w = (x2 - xValue) / (x2 - x1);
		  double y21 = y1 * y1;
		  double y22 = y2 * y2;
		  double ySq = w * y21 + (1.0 - w) * y22;
		  return 0.5 * (y22 - y21) / (x2 - x1) / Math.Sqrt(ySq);
		}

		protected internal override DoubleArray doParameterSensitivity(double xValue)
		{
		  double[] result = new double[dataSize];

		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  double x1 = xValues[lowerIndex];
		  double y1 = yValues[lowerIndex];
		  // check if x-value is at the last node
		  if (lowerIndex == dataSize - 1)
		  {
			result[dataSize - 1] = 1.0;
			return DoubleArray.ofUnsafe(result);
		  }

		  int higherIndex = lowerIndex + 1;
		  double x2 = xValues[higherIndex];
		  double y2 = yValues[higherIndex];
		  if ((xValue - x1) < EPS)
		  { // On or very close to Vertex 1
			result[lowerIndex] = 1.0d;
			return DoubleArray.ofUnsafe(result);
		  }
		  if ((x2 - xValue) < EPS)
		  { // On or very close to Vertex 2
			result[lowerIndex + 1] = 1.0d;
			return DoubleArray.ofUnsafe(result);
		  }
		  double w2 = (x2 - xValue) / (x2 - x1);
		  if ((y2 < EPS) && (y1 < EPS))
		  { // Both values very close to 0
			result[lowerIndex] = Math.Sqrt(w2);
			result[lowerIndex + 1] = Math.Sqrt(1.0d - w2);
			return DoubleArray.ofUnsafe(result);
		  }

		  double y21 = y1 * y1;
		  double y22 = y2 * y2;
		  double ySq = w2 * y21 + (1.0 - w2) * y22;
		  // Backward
		  double ySqBar = 0.5 / Math.Sqrt(ySq);
		  double y22Bar = (1.0 - w2) * ySqBar;
		  double y21Bar = w2 * ySqBar;
		  double y1Bar = 2 * y1 * y21Bar;
		  double y2Bar = 2 * y2 * y22Bar;
		  result[lowerIndex] = y1Bar;
		  result[lowerIndex + 1] = y2Bar;

		  return DoubleArray.ofUnsafe(result);
		}

		public override BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{

		  return new Bound(this, extrapolatorLeft, extrapolatorRight);
		}
	  }

	}

}