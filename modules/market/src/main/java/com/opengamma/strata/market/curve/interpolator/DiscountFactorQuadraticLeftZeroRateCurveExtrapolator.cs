using System;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using Supplier = com.google.common.@base.Supplier;
	using Suppliers = com.google.common.@base.Suppliers;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Extrapolator implementation that is designed for extrapolating a zero rate curve for the near end.
	/// <para>
	/// The extrapolation is completed by applying a quadratic extrapolant on the discount
	/// factor, where the point (0,1) is inserted and
	/// the first derivative value is assumed to be continuous at the first x-value.
	/// </para>
	/// </summary>
	[Serializable]
	internal class DiscountFactorQuadraticLeftZeroRateCurveExtrapolator : CurveExtrapolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The extrapolator name.
	  /// </summary>
	  public const string NAME = "DiscountFactorQuadraticLeftZeroRate";
	  /// <summary>
	  /// The extrapolator instance.
	  /// </summary>
	  public static readonly CurveExtrapolator INSTANCE = new DiscountFactorQuadraticLeftZeroRateCurveExtrapolator();
	  /// <summary>
	  /// The epsilon value.
	  /// </summary>
	  private const double EPS = 1e-8;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private DiscountFactorQuadraticLeftZeroRateCurveExtrapolator()
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
		internal readonly double firstXValue;
		internal readonly double firstYValue;
		internal readonly double firstYGradient;
		internal readonly double firstDfValue;
		internal readonly double eps;
		internal readonly double leftQuadCoef;
		internal readonly double leftLinCoef;
		internal readonly Supplier<DoubleArray> leftSens;

		internal Bound(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
		{
		  this.nodeCount = xValues.size();
		  this.firstXValue = xValues.get(0);
		  this.firstYValue = yValues.get(0);
		  this.firstDfValue = Math.Exp(-firstXValue * firstYValue);
		  this.firstYGradient = interpolator.firstDerivative(firstXValue);
		  double gradient = -firstYValue * firstDfValue - firstXValue * firstDfValue * firstYGradient;
		  this.eps = EPS * (xValues.get(nodeCount - 1) - firstXValue);
		  this.leftQuadCoef = gradient / firstXValue - (firstDfValue - 1d) / firstXValue / firstXValue;
		  this.leftLinCoef = -gradient + 2d * (firstDfValue - 1d) / firstXValue;
		  this.leftSens = Suppliers.memoize(() => interpolator.parameterSensitivity(firstXValue + eps));
		}

		//-------------------------------------------------------------------------
		public virtual double leftExtrapolate(double xValue)
		{
		  if (firstXValue == 0d)
		  {
			throw new System.ArgumentException("The trivial point at x = 0 is already included");
		  }
		  if (Math.Abs(xValue) < eps)
		  {
			return -leftLinCoef - (leftQuadCoef - 0.5d * leftLinCoef * leftLinCoef) * xValue - xValue * xValue * (leftLinCoef * leftLinCoef * leftLinCoef / 3d - leftQuadCoef * leftLinCoef);
		  }
		  double df = leftQuadCoef * xValue * xValue + leftLinCoef * xValue + 1d;
		  return -Math.Log(df) / xValue;
		}

		public virtual double leftExtrapolateFirstDerivative(double xValue)
		{
		  if (firstXValue == 0d)
		  {
			throw new System.ArgumentException("The trivial point at x = 0 is already included");
		  }
		  if (Math.Abs(xValue) < eps)
		  {
			return -leftQuadCoef + 0.5d * leftLinCoef * leftLinCoef - xValue * (2d * leftLinCoef * leftLinCoef * leftLinCoef / 3d - 2d * leftQuadCoef * leftLinCoef);
		  }
		  double gradDf = 2d * leftQuadCoef * xValue + leftLinCoef;
		  double df = leftQuadCoef * xValue * xValue + leftLinCoef * xValue + 1d;
		  return Math.Log(df) / Math.Pow(xValue, 2) - gradDf / (df * xValue);
		}

		public virtual DoubleArray leftExtrapolateParameterSensitivity(double xValue)
		{
		  if (firstXValue == 0d)
		  {
			throw new System.ArgumentException("The trivial point at x = 0 is already included");
		  }
		  double[] sensiZero = leftSens.get().toArray();
		  double xQuad = xValue * xValue;
		  if (Math.Abs(xValue) < eps)
		  {
			double coef1 = -(leftLinCoef * leftLinCoef - leftQuadCoef) * xQuad + leftLinCoef * xValue - 1d;
			double coef2 = -xValue + leftLinCoef * xQuad;
			double factor = firstDfValue * (firstXValue * coef1 - coef2);
			for (int i = 1; i < nodeCount; i++)
			{
			  sensiZero[i] *= factor / eps;
			}
			sensiZero[0] = (sensiZero[0] - 1d) * factor / eps;
			sensiZero[0] += -firstDfValue * coef1 * (1d + firstXValue * firstYValue + firstXValue * firstXValue * firstYGradient) + firstDfValue * (firstYValue + firstXValue * firstYGradient) * coef2;
			return DoubleArray.ofUnsafe(sensiZero);
		  }
		  double df = leftQuadCoef * xQuad + leftLinCoef * xValue + 1d;
		  double factor = xQuad / firstXValue - xValue;
		  for (int i = 1; i < nodeCount; i++)
		  {
			sensiZero[i] *= factor / eps;
		  }
		  sensiZero[0] = (sensiZero[0] - 1d) * factor / eps;
		  sensiZero[0] += xValue / firstXValue - (xQuad / firstXValue - xValue) * (firstYValue + firstXValue * firstYGradient);
		  return DoubleArray.ofUnsafe(sensiZero).multipliedBy(firstXValue * firstDfValue / (xValue * df));
		}

		//-------------------------------------------------------------------------
		public virtual double rightExtrapolate(double xValue)
		{
		  throw new System.ArgumentException("QuadraticLeftZeroRateCurveExtrapolator cannot be used for right extrapolation");
		}

		public virtual double rightExtrapolateFirstDerivative(double xValue)
		{
		  throw new System.ArgumentException("QuadraticLeftZeroRateCurveExtrapolator cannot be used for right extrapolation");
		}

		public virtual DoubleArray rightExtrapolateParameterSensitivity(double xValue)
		{
		  throw new System.ArgumentException("QuadraticLeftZeroRateCurveExtrapolator cannot be used for right extrapolation");
		}
	  }

	}

}