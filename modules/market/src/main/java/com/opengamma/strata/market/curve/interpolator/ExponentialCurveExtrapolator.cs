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
	/// Extrapolator implementation based on a exponential function.
	/// <para>
	/// Outside the data range the function is
	/// an exponential exp(m*x) where m is such that:
	///  - on the left: exp(m * firstXValue) = firstYValue
	///  - on the right: exp(m * lastXValue) = lastYValue
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class ExponentialCurveExtrapolator : CurveExtrapolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The extrapolator name.
	  /// </summary>
	  public const string NAME = "Exponential";
	  /// <summary>
	  /// The extrapolator instance.
	  /// </summary>
	  public static readonly CurveExtrapolator INSTANCE = new ExponentialCurveExtrapolator();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private ExponentialCurveExtrapolator()
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
		internal readonly double leftGradient;
		internal readonly double rightGradient;

		internal Bound(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
		{
		  this.nodeCount = xValues.size();
		  this.firstXValue = xValues.get(0);
		  this.firstYValue = yValues.get(0);
		  this.lastXValue = xValues.get(nodeCount - 1);
		  this.lastYValue = yValues.get(nodeCount - 1);
		  // left
		  this.leftGradient = Math.Log(firstYValue) / firstXValue;
		  // right
		  this.rightGradient = Math.Log(lastYValue) / lastXValue;
		}

		//-------------------------------------------------------------------------
		public virtual double leftExtrapolate(double xValue)
		{
		  return Math.Exp(leftGradient * xValue);
		}

		public virtual double leftExtrapolateFirstDerivative(double xValue)
		{
		  return leftGradient * Math.Exp(leftGradient * xValue);
		}

		public virtual DoubleArray leftExtrapolateParameterSensitivity(double xValue)
		{
		  double ex = Math.Exp(leftGradient * xValue);
		  double[] result = new double[nodeCount];
		  result[0] = ex * xValue / (firstXValue * firstYValue);
		  return DoubleArray.ofUnsafe(result);
		}

		//-------------------------------------------------------------------------
		public virtual double rightExtrapolate(double xValue)
		{
		  return Math.Exp(rightGradient * xValue);
		}

		public virtual double rightExtrapolateFirstDerivative(double xValue)
		{
		  return rightGradient * Math.Exp(rightGradient * xValue);
		}

		public virtual DoubleArray rightExtrapolateParameterSensitivity(double xValue)
		{
		  double ex = Math.Exp(rightGradient * xValue);
		  double[] result = new double[nodeCount];
		  result[nodeCount - 1] = ex * xValue / (lastXValue * lastYValue);
		  return DoubleArray.ofUnsafe(result);
		}
	  }

	}

}