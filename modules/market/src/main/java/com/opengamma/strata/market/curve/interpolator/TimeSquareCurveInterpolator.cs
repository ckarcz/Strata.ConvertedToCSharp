using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Time square interpolator.
	/// <para>
	/// The interpolation is linear on x y^2. The interpolator is used for interpolation on integrated variance for options.
	/// All values of y must be positive.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class TimeSquareCurveInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "TimeSquare";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly CurveInterpolator INSTANCE = new TimeSquareCurveInterpolator();
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
	  private TimeSquareCurveInterpolator()
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
		  ArgChecker.isTrue(xValue > 0, "Value should be stricly positive");
		  // x-value is less than the x-value of the last node (lowerIndex < intervalCount)
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  double x1 = xValues[lowerIndex];
		  double y1 = yValues[lowerIndex];
		  if (lowerIndex == dataSize - 1)
		  {
			return y1;
		  }

		  int higherIndex = lowerIndex + 1;
		  double x2 = xValues[higherIndex];
		  double y2 = yValues[higherIndex];
		  double w = (x2 - xValue) / (x2 - x1);
		  double xy21 = x1 * y1 * y1;
		  double xy22 = x2 * y2 * y2;
		  double xy2 = w * xy21 + (1 - w) * xy22;
		  return Math.Sqrt(xy2 / xValue);
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  ArgChecker.isTrue(xValue > 0, "Value should be stricly positive");
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
		  int higherIndex = index + 1;
		  double x2 = xValues[higherIndex];
		  double y2 = yValues[higherIndex];
		  if ((y1 < EPS) || (y2 < EPS))
		  {
			throw new System.NotSupportedException("node sensitivity not implemented when one node is 0 value");
		  }

		  double w = (x2 - xValue) / (x2 - x1);
		  double xy21 = x1 * y1 * y1;
		  double xy22 = x2 * y2 * y2;
		  double xy2 = w * xy21 + (1 - w) * xy22;
		  return 0.5 * (-Math.Sqrt(xy2 / xValue) + (-xy21 + xy22) / (x2 - x1) / Math.Sqrt(xy2 / xValue)) / xValue;
		}

		protected internal override DoubleArray doParameterSensitivity(double xValue)
		{
		  double[] resultSensitivity = new double[dataSize];
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  double x1 = xValues[lowerIndex];
		  double y1 = yValues[lowerIndex];
		  // check if x-value is at the last node
		  if (lowerIndex == dataSize - 1)
		  {
			resultSensitivity[dataSize - 1] = 1.0;
			return DoubleArray.ofUnsafe(resultSensitivity);
		  }

		  int higherIndex = lowerIndex + 1;
		  double x2 = xValues[higherIndex];
		  double y2 = yValues[higherIndex];
		  if ((y1 < EPS) || (y2 < EPS))
		  {
			throw new System.NotSupportedException("node sensitivity not implemented when one node is 0 value");
		  }

		  double w = (x2 - xValue) / (x2 - x1);
		  double xy21 = x1 * y1 * y1;
		  double xy22 = x2 * y2 * y2;
		  double xy2 = w * xy21 + (1 - w) * xy22;
		  double resultValue = Math.Sqrt(xy2 / xValue);
		  double resultValueBar = 1.0;
		  double xy2Bar = 0.5 / resultValue / xValue * resultValueBar;
		  double xy21Bar = w * xy2Bar;
		  double xy22Bar = (1 - w) * xy2Bar;
		  double y2Bar = 2 * x2 * y2 * xy22Bar;
		  double y1Bar = 2 * x1 * y1 * xy21Bar;
		  resultSensitivity[lowerIndex] = y1Bar;
		  resultSensitivity[lowerIndex + 1] = y2Bar;
		  return DoubleArray.ofUnsafe(resultSensitivity);
		}

		public override BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{

		  return new Bound(this, extrapolatorLeft, extrapolatorRight);
		}
	  }

	}

}