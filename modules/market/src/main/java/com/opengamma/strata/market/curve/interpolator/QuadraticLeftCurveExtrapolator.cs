using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using Supplier = com.google.common.@base.Supplier;
	using Suppliers = com.google.common.@base.Suppliers;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Extrapolator implementation that is designed for extrapolating a discount factor where the
	/// trivial point (0,1) is NOT involved in the data.
	/// The extrapolation is completed by applying a quadratic extrapolant on the discount
	/// factor (not log of the discount factor), where the point (0,1) is inserted and
	/// the first derivative value is assumed to be continuous at the first x-value.
	/// </summary>
	[Serializable]
	internal sealed class QuadraticLeftCurveExtrapolator : CurveExtrapolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The extrapolator name.
	  /// </summary>
	  public const string NAME = "QuadraticLeft";
	  /// <summary>
	  /// The extrapolator instance.
	  /// </summary>
	  public static readonly CurveExtrapolator INSTANCE = new QuadraticLeftCurveExtrapolator();
	  /// <summary>
	  /// The epsilon value.
	  /// </summary>
	  private const double EPS = 1e-8;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private QuadraticLeftCurveExtrapolator()
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
		internal readonly double eps;
		internal readonly double leftQuadCoef;
		internal readonly double leftLinCoef;
		internal readonly Supplier<DoubleArray> leftSens;

		internal Bound(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
		{
		  this.nodeCount = xValues.size();
		  this.firstXValue = xValues.get(0);
		  this.firstYValue = yValues.get(0);
		  this.lastXValue = xValues.get(nodeCount - 1);
		  double gradient = interpolator.firstDerivative(firstXValue);
		  this.eps = EPS * (lastXValue - firstXValue);
		  this.leftQuadCoef = gradient / firstXValue - (firstYValue - 1d) / firstXValue / firstXValue;
		  this.leftLinCoef = -gradient + 2d * (firstYValue - 1d) / firstXValue;
		  this.leftSens = Suppliers.memoize(() => interpolator.parameterSensitivity(firstXValue + eps));
		}

		//-------------------------------------------------------------------------
		public virtual double leftExtrapolate(double xValue)
		{
		  if (firstXValue == 0d)
		  {
			throw new System.ArgumentException("The trivial point at x = 0 is already included");
		  }
		  return leftQuadCoef * xValue * xValue + leftLinCoef * xValue + 1d;
		}

		public virtual double leftExtrapolateFirstDerivative(double xValue)
		{
		  if (firstXValue == 0d)
		  {
			throw new System.ArgumentException("The trivial point at x = 0 is already included");
		  }
		  return 2d * leftQuadCoef * xValue + leftLinCoef;
		}

		public virtual DoubleArray leftExtrapolateParameterSensitivity(double xValue)
		{
		  if (firstXValue == 0d)
		  {
			throw new System.ArgumentException("The trivial point at x = 0 is already included");
		  }
		  double[] result = leftSens.get().toArray();
		  for (int i = 1; i < nodeCount; i++)
		  {
			double tmp = result[i] * xValue / eps;
			result[i] = tmp / firstXValue * xValue - tmp;
		  }
		  double tmp = (result[0] - 1d) / eps;
		  result[0] = (tmp / firstXValue - 1d / firstXValue / firstXValue) * xValue * xValue + (2d / firstXValue - tmp) * xValue;
		  return DoubleArray.ofUnsafe(result);
		}

		//-------------------------------------------------------------------------
		public virtual double rightExtrapolate(double xValue)
		{
		  throw new System.ArgumentException("QuadraticLeft extrapolator cannot be used for right extrapolation");
		}

		public virtual double rightExtrapolateFirstDerivative(double xValue)
		{
		  throw new System.ArgumentException("QuadraticLeft extrapolator cannot be used for right extrapolation");
		}

		public virtual DoubleArray rightExtrapolateParameterSensitivity(double xValue)
		{
		  throw new System.ArgumentException("QuadraticLeft extrapolator cannot be used for right extrapolation");
		}
	  }

	}

}