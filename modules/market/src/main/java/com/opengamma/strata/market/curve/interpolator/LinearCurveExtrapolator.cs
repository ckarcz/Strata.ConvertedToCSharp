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
	/// Extrapolator implementation that returns a value linearly from the gradient at the first or last node.
	/// </summary>
	[Serializable]
	internal sealed class LinearCurveExtrapolator : CurveExtrapolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The extrapolator name.
	  /// </summary>
	  public const string NAME = "Linear";
	  /// <summary>
	  /// The extrapolator instance.
	  /// </summary>
	  public static readonly CurveExtrapolator INSTANCE = new LinearCurveExtrapolator();
	  /// <summary>
	  /// The epsilon value.
	  /// </summary>
	  private const double EPS = 1e-8;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private LinearCurveExtrapolator()
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

	  public BoundCurveExtrapolator bind(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
	  {
		return new Bound(xValues, yValues, interpolator);
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return NAME;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Bound extrapolator.
	  /// </summary>
	  internal class Bound : BoundCurveExtrapolator
	  {
		internal readonly int nodeCount;
		internal readonly double firstXValue;
		internal readonly double firstYValue;
		internal readonly double lastXValue;
		internal readonly double lastYValue;
		internal readonly double eps;
		internal readonly double leftGradient;
		internal readonly DoubleArray leftSens;
		internal readonly double rightGradient;
		internal readonly DoubleArray rightSens;

		internal Bound(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
		{
		  this.nodeCount = xValues.size();
		  this.firstXValue = xValues.get(0);
		  this.firstYValue = yValues.get(0);
		  this.lastXValue = xValues.get(nodeCount - 1);
		  this.lastYValue = yValues.get(nodeCount - 1);
		  this.eps = EPS * (lastXValue - firstXValue);
		  // left
		  this.leftGradient = (interpolator.interpolate(firstXValue + eps) - firstYValue) / eps;
		  this.leftSens = interpolator.parameterSensitivity(firstXValue + eps);
		  // right
		  this.rightGradient = (lastYValue - interpolator.interpolate(lastXValue - eps)) / eps;
		  this.rightSens = interpolator.parameterSensitivity(lastXValue - eps);
		}

		//-------------------------------------------------------------------------
		public virtual double leftExtrapolate(double xValue)
		{
		  return firstYValue + (xValue - firstXValue) * leftGradient;
		}

		public virtual double leftExtrapolateFirstDerivative(double xValue)
		{
		  return leftGradient;
		}

		public virtual DoubleArray leftExtrapolateParameterSensitivity(double xValue)
		{
		  double[] result = leftSens.toArray();
		  int n = result.Length;
		  for (int i = 1; i < n; i++)
		  {
			result[i] = result[i] * (xValue - firstXValue) / eps;
		  }
		  result[0] = 1 + (result[0] - 1) * (xValue - firstXValue) / eps;
		  return DoubleArray.ofUnsafe(result);
		}

		//-------------------------------------------------------------------------
		public virtual double rightExtrapolate(double xValue)
		{
		  return lastYValue + (xValue - lastXValue) * rightGradient;
		}

		public virtual double rightExtrapolateFirstDerivative(double xValue)
		{
		  return rightGradient;
		}

		public virtual DoubleArray rightExtrapolateParameterSensitivity(double xValue)
		{
		  double[] result = rightSens.toArray();
		  int n = result.Length;
		  for (int i = 0; i < n - 1; i++)
		  {
			result[i] = -result[i] * (xValue - lastXValue) / eps;
		  }
		  result[n - 1] = 1 + (1 - result[n - 1]) * (xValue - lastXValue) / eps;
		  return DoubleArray.ofUnsafe(result);
		}
	  }

	}

}