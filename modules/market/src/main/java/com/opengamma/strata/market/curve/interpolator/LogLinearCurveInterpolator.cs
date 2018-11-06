using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Interpolator implementation that returns the log linearly interpolated value.
	/// <para>
	/// The interpolated value of the function <i>y</i> at <i>x</i> between two data points
	/// <i>(x<sub>1</sub>, y<sub>1</sub>)</i> and <i>(x<sub>2</sub>, y<sub>2</sub>)</i> is given by:<br>
	/// <i>y = y<sub>1</sub> (y<sub>2</sub> / y<sub>1</sub>) ^ ((x - x<sub>1</sub>) /
	/// (x<sub>2</sub> - x<sub>1</sub>))</i><br>
	/// It is the equivalent of performing a linear interpolation on a data set after
	/// taking the logarithm of the y-values.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class LogLinearCurveInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "LogLinear";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly CurveInterpolator INSTANCE = new LogLinearCurveInterpolator();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private LogLinearCurveInterpolator()
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
		internal readonly int intervalCount;

		internal Bound(DoubleArray xValues, DoubleArray yValues) : base(xValues, yValues)
		{
		  this.xValues = xValues.toArrayUnsafe();
		  this.yValues = yValues.toArrayUnsafe();
		  this.intervalCount = xValues.size() - 1;
		}

		internal Bound(Bound @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight) : base(@base, extrapolatorLeft, extrapolatorRight)
		{
		  this.xValues = @base.xValues;
		  this.yValues = @base.yValues;
		  this.intervalCount = @base.intervalCount;
		}

		//-------------------------------------------------------------------------
		protected internal override double doInterpolate(double xValue)
		{
		  // x-value is less than the x-value of the last node (lowerIndex < intervalCount)
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  double x1 = xValues[lowerIndex];
		  double x2 = xValues[lowerIndex + 1];
		  double y1 = yValues[lowerIndex];
		  double y2 = yValues[lowerIndex + 1];
		  return Math.Pow(y2 / y1, (xValue - x1) / (x2 - x1)) * y1;
		}

		protected internal override double doInterpolateFromExtrapolator(double xValue)
		{
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  // check if x-value is at the last node
		  if (lowerIndex == intervalCount)
		  {
			// if value is at last node, calculate using the previous interval
			lowerIndex--;
		  }
		  double x1 = xValues[lowerIndex];
		  double x2 = xValues[lowerIndex + 1];
		  double y1 = yValues[lowerIndex];
		  double y2 = yValues[lowerIndex + 1];
		  return Math.Pow(y2 / y1, (xValue - x1) / (x2 - x1)) * y1;
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  // check if x-value is at the last node
		  if (lowerIndex == intervalCount)
		  {
			// if value is at last node, calculate the gradient from the previous interval
			double x1 = xValues[lowerIndex - 1];
			double x2 = xValues[lowerIndex];
			double y1 = yValues[lowerIndex - 1];
			double y2 = yValues[lowerIndex];
			return y2 * Math.Log(y2 / y1) / (x2 - x1);
		  }
		  double x1 = xValues[lowerIndex];
		  double x2 = xValues[lowerIndex + 1];
		  double y1 = yValues[lowerIndex];
		  double y2 = yValues[lowerIndex + 1];
		  double yDiv = y2 / y1;
		  double xDiff = (x2 - x1);
		  return Math.Pow(yDiv, (xValue - x1) / xDiff) * y1 * Math.Log(yDiv) / xDiff;
		}

		protected internal override DoubleArray doParameterSensitivity(double xValue)
		{
		  double[] result = new double[yValues.Length];
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  // check if x-value is at the last node
		  if (lowerIndex == intervalCount)
		  {
			// sensitivity is entirely to the last node
			result[intervalCount] = 1d;
		  }
		  else
		  {
			double x1 = xValues[lowerIndex];
			double x2 = xValues[lowerIndex + 1];
			double y1 = yValues[lowerIndex];
			double y2 = yValues[lowerIndex + 1];
			double diffInv = 1.0 / (x2 - x1);
			double x1diffInv = (xValue - x1) * diffInv;
			double x2diffInv = (x2 - xValue) * diffInv;
			double yDiv = y1 / y2;
			result[lowerIndex] = Math.Pow(yDiv, -x1diffInv) * x2diffInv;
			result[lowerIndex + 1] = Math.Pow(yDiv, x2diffInv) * x1diffInv;
		  }
		  return DoubleArray.ofUnsafe(result);
		}

		public override BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{

		  return new Bound(this, extrapolatorLeft, extrapolatorRight);
		}
	  }

	}

}