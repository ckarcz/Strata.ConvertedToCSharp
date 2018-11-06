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
	/// Interpolator implementation that returns the linearly interpolated value.
	/// <para>
	/// The interpolated value of the function <i>y</i> at <i>x</i> between two data points
	/// <i>(x<sub>1</sub>, y<sub>1</sub>)</i> and <i>(x<sub>2</sub>, y<sub>2</sub>)</i> is given by:<br>
	/// <i>y = y<sub>1</sub> + (x - x<sub>1</sub>) * (y<sub>2</sub> - y<sub>1</sub>)
	/// / (x<sub>2</sub> - x<sub>1</sub>)</i>.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class LinearCurveInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "Linear";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly CurveInterpolator INSTANCE = new LinearCurveInterpolator();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private LinearCurveInterpolator()
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
		internal readonly double[] gradients;

		internal Bound(DoubleArray xValues, DoubleArray yValues) : base(xValues, yValues)
		{
		  this.xValues = xValues.toArrayUnsafe();
		  this.yValues = yValues.toArrayUnsafe();
		  this.intervalCount = xValues.size() - 1;
		  this.gradients = new double[intervalCount];
		  for (int i = 0; i < intervalCount; i++)
		  {
			double x1 = xValues.get(i);
			double y1 = yValues.get(i);
			double x2 = xValues.get(i + 1);
			double y2 = yValues.get(i + 1);
			double gradient = (y2 - y1) / (x2 - x1);
			this.gradients[i] = gradient;
		  }
		}

		internal Bound(Bound @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight) : base(@base, extrapolatorLeft, extrapolatorRight)
		{
		  this.xValues = @base.xValues;
		  this.yValues = @base.yValues;
		  this.intervalCount = @base.intervalCount;
		  this.gradients = @base.gradients;
		}

		//-------------------------------------------------------------------------
		protected internal override double doInterpolate(double xValue)
		{
		  // x-value is less than the x-value of the last node (lowerIndex < intervalCount)
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  double x1 = xValues[lowerIndex];
		  double y1 = yValues[lowerIndex];
		  return y1 + (xValue - x1) * gradients[lowerIndex];
		}

		protected internal override double doInterpolateFromExtrapolator(double xValue)
		{
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  // check if x-value is at the last node
		  if (lowerIndex == intervalCount)
		  {
			// if value is at last node, calculate the gradient from the previous interval
			lowerIndex--;
		  }
		  double x1 = xValues[lowerIndex];
		  double y1 = yValues[lowerIndex];
		  return y1 + (xValue - x1) * gradients[lowerIndex];
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  // check if x-value is at the last node
		  if (lowerIndex == intervalCount)
		  {
			// if value is at last node, calculate the gradient from the previous interval
			lowerIndex--;
		  }
		  return gradients[lowerIndex];
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
			double dx = x2 - x1;
			double a = (x2 - xValue) / dx;
			result[lowerIndex] = a;
			result[lowerIndex + 1] = 1 - a;
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