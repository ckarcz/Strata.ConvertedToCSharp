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
	/// Extrapolator implementation.
	/// <para>
	/// The extrapolant is {@code exp(f(x))} where {@code f(x)} is a linear function
	/// which is smoothly connected with a log-interpolator {@code exp(F(x))}.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class LogLinearCurveExtrapolator : CurveExtrapolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The extrapolator name.
	  /// </summary>
	  public const string NAME = "LogLinear";
	  /// <summary>
	  /// The extrapolator instance.
	  /// </summary>
	  public static readonly CurveExtrapolator INSTANCE = new LogLinearCurveExtrapolator();
	  /// <summary>
	  /// The epsilon value.
	  /// </summary>
	  private const double EPS = 1e-8;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private LogLinearCurveExtrapolator()
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
		internal readonly double firstYValueLog;
		internal readonly double lastXValue;
		internal readonly double lastYValue;
		internal readonly double lastYValueLog;
		internal readonly double eps;
		internal readonly double leftGradient;
		internal readonly double leftResValueInterpolator;
		internal readonly DoubleArray leftSens;
		internal readonly double rightGradient;
		internal readonly double rightResValueInterpolator;
		internal readonly DoubleArray rightSens;

		internal Bound(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
		{
		  this.nodeCount = xValues.size();
		  this.firstXValue = xValues.get(0);
		  this.firstYValue = yValues.get(0);
		  this.firstYValueLog = Math.Log(firstYValue);
		  this.lastXValue = xValues.get(nodeCount - 1);
		  this.lastYValue = yValues.get(nodeCount - 1);
		  this.lastYValueLog = Math.Log(lastYValue);
		  this.eps = EPS * (lastXValue - firstXValue);
		  // left
		  this.leftGradient = interpolator.firstDerivative(firstXValue) / interpolator.interpolate(firstXValue);
		  this.leftResValueInterpolator = interpolator.interpolate(firstXValue + eps);
		  this.leftSens = interpolator.parameterSensitivity(firstXValue + eps);
		  // right
		  this.rightGradient = interpolator.firstDerivative(lastXValue) / interpolator.interpolate(lastXValue);
		  this.rightResValueInterpolator = interpolator.interpolate(lastXValue - eps);
		  this.rightSens = interpolator.parameterSensitivity(lastXValue - eps);
		}

		//-------------------------------------------------------------------------
		public virtual double leftExtrapolate(double xValue)
		{
		  return Math.Exp(firstYValueLog + (xValue - firstXValue) * leftGradient);
		}

		public virtual double leftExtrapolateFirstDerivative(double xValue)
		{
		  return leftGradient * Math.Exp(firstYValueLog + (xValue - firstXValue) * leftGradient);
		}

		public virtual DoubleArray leftExtrapolateParameterSensitivity(double xValue)
		{
		  double[] result = leftSens.toArray();
		  double resValueExtrapolator = leftExtrapolate(xValue);
		  double factor1 = (xValue - firstXValue) / eps;
		  double factor2 = factor1 * resValueExtrapolator / leftResValueInterpolator;
		  int n = result.Length;
		  for (int i = 1; i < n; i++)
		  {
			result[i] *= factor2;
		  }
		  result[0] = result[0] * factor2 + (1d - factor1) * resValueExtrapolator / firstYValue;
		  return DoubleArray.ofUnsafe(result);
		}

		//-------------------------------------------------------------------------
		public virtual double rightExtrapolate(double xValue)
		{
		  return Math.Exp(lastYValueLog + (xValue - lastXValue) * rightGradient);
		}

		public virtual double rightExtrapolateFirstDerivative(double xValue)
		{
		  return rightGradient * Math.Exp(lastYValueLog + (xValue - lastXValue) * rightGradient);
		}

		public virtual DoubleArray rightExtrapolateParameterSensitivity(double xValue)
		{
		  double[] result = rightSens.toArray();
		  double resValueExtrapolator = rightExtrapolate(xValue);
		  double factor1 = (xValue - lastXValue) / eps;
		  double factor2 = factor1 * resValueExtrapolator / rightResValueInterpolator;
		  int n = result.Length;
		  for (int i = 0; i < n - 1; i++)
		  {
			result[i] *= -factor2;
		  }
		  result[n - 1] = (1d + factor1) * resValueExtrapolator / lastYValue - result[n - 1] * factor2;
		  return DoubleArray.ofUnsafe(result);
		}
	  }

	}

}