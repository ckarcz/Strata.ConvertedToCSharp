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
	/// Extrapolator implementation that returns the y-value of the first or last node.
	/// <para>
	/// When left extrapolating, the y-value of the first node is returned.
	/// When right extrapolating, the y-value of the last node is returned.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class FlatCurveExtrapolator : CurveExtrapolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The extrapolator name.
	  /// </summary>
	  public const string NAME = "Flat";
	  /// <summary>
	  /// The extrapolator instance.
	  /// </summary>
	  public static readonly CurveExtrapolator INSTANCE = new FlatCurveExtrapolator();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FlatCurveExtrapolator()
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
		return new Bound(xValues, yValues);
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
		internal readonly double firstYValue;
		internal readonly double lastYValue;
		internal readonly DoubleArray leftSensitivity;
		internal readonly DoubleArray rightSensitivity;

		internal Bound(DoubleArray xValues, DoubleArray yValues)
		{
		  this.nodeCount = xValues.size();
		  this.firstYValue = yValues.get(0);
		  this.lastYValue = yValues.get(nodeCount - 1);
		  double[] left = new double[nodeCount];
		  left[0] = 1d;
		  this.leftSensitivity = DoubleArray.ofUnsafe(left);
		  double[] right = new double[nodeCount];
		  right[nodeCount - 1] = 1d;
		  this.rightSensitivity = DoubleArray.ofUnsafe(right);
		}

		//-------------------------------------------------------------------------
		public virtual double leftExtrapolate(double xValue)
		{
		  return firstYValue;
		}

		public virtual double leftExtrapolateFirstDerivative(double xValue)
		{
		  return 0d;
		}

		public virtual DoubleArray leftExtrapolateParameterSensitivity(double xValue)
		{
		  return leftSensitivity;
		}

		//-------------------------------------------------------------------------
		public virtual double rightExtrapolate(double xValue)
		{
		  return lastYValue;
		}

		public virtual double rightExtrapolateFirstDerivative(double xValue)
		{
		  return 0d;
		}

		public virtual DoubleArray rightExtrapolateParameterSensitivity(double xValue)
		{
		  return rightSensitivity;
		}
	  }

	}

}