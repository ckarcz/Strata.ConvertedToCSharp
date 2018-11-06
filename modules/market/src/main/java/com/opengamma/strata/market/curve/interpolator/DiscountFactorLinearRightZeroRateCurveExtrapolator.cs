using System;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Extrapolator implementation that is designed for extrapolating a zero rate curve for the far end.
	/// <para>
	/// The linear interpolation is applied discount factor values converted from the input zero rates. 
	/// The gradient of the linear function is determined so that the first derivative of the discount 
	/// factor is continuous at the last node.
	/// </para>
	/// </summary>
	[Serializable]
	internal class DiscountFactorLinearRightZeroRateCurveExtrapolator : CurveExtrapolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The extrapolator name.
	  /// </summary>
	  public const string NAME = "DiscountFactorLinearRightZeroRateCurve";
	  /// <summary>
	  /// The extrapolator instance.
	  /// </summary>
	  public static readonly CurveExtrapolator INSTANCE = new DiscountFactorLinearRightZeroRateCurveExtrapolator();
	  /// <summary>
	  /// The epsilon value.
	  /// </summary>
	  private const double EPS = 1e-8;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private DiscountFactorLinearRightZeroRateCurveExtrapolator()
	  {
	  }

	  // resolve instance
	  private object readResolve()
	  {
		return INSTANCE;
	  }

	  //-------------------------------------------------------------------------
	  public virtual string Name
	  {
		  get
		  {
			return NAME;
		  }
	  }

	  public virtual BoundCurveExtrapolator bind(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
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
		internal readonly double lastXValue;
		internal readonly double lastYValue;
		internal readonly double lastDf;
		internal readonly double eps;
		internal readonly double rightYGradient;
		internal readonly DoubleArray rightYSens;

		internal readonly double coef1;
		internal readonly double coef0;

		internal Bound(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
		{
		  this.nodeCount = xValues.size();
		  this.lastXValue = xValues.get(nodeCount - 1);
		  this.lastYValue = yValues.get(nodeCount - 1);
		  this.lastDf = Math.Exp(-lastXValue * lastYValue);
		  this.eps = EPS * (lastXValue - xValues.get(0));
		  this.rightYGradient = (lastYValue - interpolator.interpolate(lastXValue - eps)) / eps;
		  this.rightYSens = interpolator.parameterSensitivity(lastXValue - eps).multipliedBy(-1d);
		  this.coef1 = -lastYValue * lastDf - lastXValue * lastDf * rightYGradient;
		  this.coef0 = lastDf - coef1 * lastXValue;
		}

		//-------------------------------------------------------------------------
		public virtual double leftExtrapolate(double xValue)
		{
		  throw new System.ArgumentException("DiscountFactorLinearRightZeroRateCurveExtrapolator cannot be used for left extrapolation");
		}

		public virtual double leftExtrapolateFirstDerivative(double xValue)
		{
		  throw new System.ArgumentException("DiscountFactorLinearRightZeroRateCurveExtrapolator cannot be used for left extrapolation");
		}

		public virtual DoubleArray leftExtrapolateParameterSensitivity(double xValue)
		{
		  throw new System.ArgumentException("DiscountFactorLinearRightZeroRateCurveExtrapolator cannot be used for left extrapolation");
		}

		//-------------------------------------------------------------------------
		public virtual double rightExtrapolate(double xValue)
		{
		  if (lastXValue <= 0d)
		  {
			throw new System.ArgumentException("X value of the right endpoint must be positive");
		  }
		  return -Math.Log(coef1 * xValue + coef0) / xValue;
		}

		public virtual double rightExtrapolateFirstDerivative(double xValue)
		{
		  if (lastXValue <= 0d)
		  {
			throw new System.ArgumentException("X value of the right endpoint must be positive");
		  }
		  double df = coef1 * xValue + coef0;
		  double value = -Math.Log(df) / xValue;
		  return -(value + coef1 / df) / xValue;
		}

		public virtual DoubleArray rightExtrapolateParameterSensitivity(double xValue)
		{
		  if (lastXValue <= 0d)
		  {
			throw new System.ArgumentException("X value of the right endpoint must be positive");
		  }
		  double df = coef1 * xValue + coef0;
		  double[] result = rightYSens.toArray();
		  double factor = xValue - lastXValue;
		  int minusOne = nodeCount - 1;
		  for (int i = 0; i < minusOne; i++)
		  {
			result[i] *= factor / eps;
		  }
		  result[minusOne] = (1d + result[minusOne]) * factor / eps;
		  result[minusOne] += (1d / lastXValue - lastYValue - lastXValue * rightYGradient) * xValue + lastXValue * lastYValue + lastXValue * lastXValue * rightYGradient;
		  return DoubleArray.ofUnsafe(result).multipliedBy(lastXValue * lastDf / (xValue * df));
		}
	  }

	}

}