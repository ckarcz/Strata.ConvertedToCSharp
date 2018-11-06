using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Extrapolator implementation.
	/// <para>
	/// Extrapolator that does no extrapolation itself and delegates to the interpolator for all operations.
	/// </para>
	/// <para>
	/// This extrapolator is used in place of a null extrapolator which allows the extrapolators to be non-null
	/// and makes for simpler and cleaner code where the extrapolators are used.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class InterpolatorCurveExtrapolator : CurveExtrapolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "Interpolator";
	  /// <summary>
	  /// The extrapolator instance.
	  /// </summary>
	  public static readonly CurveExtrapolator INSTANCE = new InterpolatorCurveExtrapolator();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private InterpolatorCurveExtrapolator()
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
		internal readonly AbstractBoundCurveInterpolator interpolator;

		internal Bound(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
		{
		  ArgChecker.isTrue(interpolator is AbstractBoundCurveInterpolator);
		  this.interpolator = (AbstractBoundCurveInterpolator) interpolator;
		}

		//-------------------------------------------------------------------------
		public virtual double leftExtrapolate(double xValue)
		{
		  return interpolator.doInterpolateFromExtrapolator(xValue);
		}

		public virtual double leftExtrapolateFirstDerivative(double xValue)
		{
		  return interpolator.doFirstDerivative(xValue);
		}

		public virtual DoubleArray leftExtrapolateParameterSensitivity(double xValue)
		{
		  return interpolator.doParameterSensitivity(xValue);
		}

		//-------------------------------------------------------------------------
		public virtual double rightExtrapolate(double xValue)
		{
		  return interpolator.doInterpolateFromExtrapolator(xValue);
		}

		public virtual double rightExtrapolateFirstDerivative(double xValue)
		{
		  return interpolator.doFirstDerivative(xValue);
		}

		public virtual DoubleArray rightExtrapolateParameterSensitivity(double xValue)
		{
		  return interpolator.doParameterSensitivity(xValue);
		}
	  }

	}

}