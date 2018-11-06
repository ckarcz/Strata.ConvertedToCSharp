using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using Supplier = com.google.common.@base.Supplier;
	using Suppliers = com.google.common.@base.Suppliers;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Extrapolator implementation that returns a value linearly in terms of {@code (x[i], x[i] * y[i])}. 
	/// <para>
	/// The gradient of the extrapolation is obtained from the gradient of the interpolated curve on {@code (x[i], x[i] * y[i])}  
	/// at the first/last node.
	/// </para>
	/// <para>
	/// The extrapolation is ambiguous at x=0. Thus the following rule applies: 
	/// The x value of the first node must be strictly negative for the left extrapolation, whereas the x value of 
	/// the last node must be strictly positive for the right extrapolation.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class ProductLinearCurveExtrapolator : CurveExtrapolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The extrapolator name.
	  /// </summary>
	  public const string NAME = "ProductLinear";
	  /// <summary>
	  /// The extrapolator instance.
	  /// </summary>
	  public static readonly CurveExtrapolator INSTANCE = new ProductLinearCurveExtrapolator();
	  /// <summary>
	  /// The epsilon value.
	  /// </summary>
	  private const double EPS = 1e-8;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private ProductLinearCurveExtrapolator()
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
		internal readonly double lastXValue;
		internal readonly double lastYValue;
		internal readonly double eps;
		internal readonly double lastGradient;
		internal readonly Supplier<DoubleArray> lastSens;
		internal readonly Supplier<DoubleArray> lastGradSens;

		internal readonly double firstXValue;
		internal readonly double firstYValue;
		internal readonly double firstGradient;
		internal readonly Supplier<DoubleArray> firstSens;
		internal readonly Supplier<DoubleArray> firstGradSens;

		internal Bound(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
		{
		  this.nodeCount = xValues.size();
		  this.firstXValue = xValues.get(0);
		  this.firstYValue = yValues.get(0);
		  this.lastXValue = xValues.get(nodeCount - 1);
		  this.lastYValue = yValues.get(nodeCount - 1);
		  this.eps = EPS * (lastXValue - firstXValue);
		  // left
		  this.firstGradient = interpolator.firstDerivative(firstXValue);
		  this.firstSens = Suppliers.memoize(() => interpolator.parameterSensitivity(firstXValue));
		  this.firstGradSens = Suppliers.memoize(() => interpolator.parameterSensitivity(firstXValue + eps).minus(firstSens.get()).dividedBy(eps));
		  // right
		  this.lastGradient = interpolator.firstDerivative(lastXValue);
		  this.lastSens = Suppliers.memoize(() => interpolator.parameterSensitivity(lastXValue));
		  this.lastGradSens = Suppliers.memoize(() => lastSens.get().minus(interpolator.parameterSensitivity(lastXValue - eps)).dividedBy(eps));
		}

		//-------------------------------------------------------------------------
		public virtual double leftExtrapolate(double xValue)
		{
		  ArgChecker.isTrue(firstXValue < -EPS, "the first x value must be negative for left extrapolation");
		  return firstGradient * firstXValue * (1d - firstXValue / xValue) + firstYValue;
		}

		public virtual double leftExtrapolateFirstDerivative(double xValue)
		{
		  ArgChecker.isTrue(firstXValue < -EPS, "the first x value must be negative for left extrapolation");
		  return firstGradient * Math.Pow(firstXValue / xValue, 2);
		}

		public virtual DoubleArray leftExtrapolateParameterSensitivity(double xValue)
		{
		  ArgChecker.isTrue(firstXValue < -EPS, "the first x value must be negative for left extrapolation");
		  double factor = (1d - firstXValue / xValue) * firstXValue;
		  return firstGradSens.get().multipliedBy(factor).plus(firstSens.get());
		}

		//-------------------------------------------------------------------------
		public virtual double rightExtrapolate(double xValue)
		{
		  ArgChecker.isTrue(lastXValue > EPS, "the last x value must be positive for right extrapolation");
		  return lastGradient * lastXValue * (1d - lastXValue / xValue) + lastYValue;
		}

		public virtual double rightExtrapolateFirstDerivative(double xValue)
		{
		  ArgChecker.isTrue(lastXValue > EPS, "the last x value must be positive for right extrapolation");
		  return lastGradient * Math.Pow(lastXValue / xValue, 2);
		}

		public virtual DoubleArray rightExtrapolateParameterSensitivity(double xValue)
		{
		  ArgChecker.isTrue(lastXValue > EPS, "the last x value must be positive for right extrapolation");
		  double factor = (1d - lastXValue / xValue) * lastXValue;
		  return lastGradSens.get().multipliedBy(factor).plus(lastSens.get());
		}
	  }

	}

}