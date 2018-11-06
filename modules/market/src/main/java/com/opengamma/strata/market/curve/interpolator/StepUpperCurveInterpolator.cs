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
	/// Interpolator implementation that uses upper step interpolation.
	/// <para>
	/// The interpolated value at <i>x</i> s.t. <i>x<sub>1</sub> < x =< x<sub>2</sub></i> is the value at <i>x<sub>2</sub></i>. 
	/// The flat extrapolation is implemented outside the data range.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class StepUpperCurveInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The small parameter.
	  /// <para>
	  /// A value will be treated as 0 if its magnitude is smaller than this parameter.
	  /// </para>
	  /// </summary>
	  private const double EPS = 1.0e-12;
	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "StepUpper";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly CurveInterpolator INSTANCE = new StepUpperCurveInterpolator();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StepUpperCurveInterpolator()
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

	  //-------------------------------------------------------------------------
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
		internal readonly int maxIndex;

		internal Bound(DoubleArray xValues, DoubleArray yValues) : base(xValues, yValues)
		{
		  this.xValues = xValues.toArrayUnsafe();
		  this.yValues = yValues.toArrayUnsafe();
		  this.maxIndex = xValues.size() - 1;
		}

		internal Bound(Bound @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight) : base(@base, extrapolatorLeft, extrapolatorRight)
		{
		  this.xValues = @base.xValues;
		  this.yValues = @base.yValues;
		  this.maxIndex = @base.maxIndex;
		}

		//-------------------------------------------------------------------------
		protected internal override double doInterpolate(double xValue)
		{
		  int upperIndex = getUpperBoundIndex(xValue);
		  return yValues[upperIndex];
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  return 0d;
		}

		protected internal override DoubleArray doParameterSensitivity(double xValue)
		{
		  double[] result = new double[yValues.Length];
		  int upperIndex = getUpperBoundIndex(xValue);
		  result[upperIndex] = 1d;
		  return DoubleArray.ofUnsafe(result);
		}

		public override BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{

		  return new Bound(this, extrapolatorLeft, extrapolatorRight);
		}

		internal virtual int getUpperBoundIndex(double xValue)
		{
		  if (xValue <= xValues[0] + EPS)
		  {
			return 0;
		  }
		  if (xValue >= xValues[maxIndex - 1] + EPS)
		  {
			return maxIndex;
		  }
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  if (Math.Abs(xValues[lowerIndex] - xValue) < EPS)
		  {
			return lowerIndex;
		  }
		  return lowerIndex + 1;
		}
	  }

	}

}