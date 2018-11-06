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
	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;
	using WeightingFunction = com.opengamma.strata.math.impl.interpolation.WeightingFunction;
	using WeightingFunctions = com.opengamma.strata.math.impl.interpolation.WeightingFunctions;

	/// <summary>
	/// Interpolator implementation that uses double quadratic interpolation.
	/// <para>
	/// This uses linear weighting.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class DoubleQuadraticCurveInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "DoubleQuadratic";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly CurveInterpolator INSTANCE = new DoubleQuadraticCurveInterpolator();
	  /// <summary>
	  /// The weighting function.
	  /// </summary>
	  private static readonly WeightingFunction WEIGHT_FUNCTION = WeightingFunctions.LINEAR;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private DoubleQuadraticCurveInterpolator()
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

	  //-----------------------------------------------------------------------
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
		internal readonly int intervalCount;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal readonly RealPolynomialFunction1D[] quadratics_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal readonly Supplier<RealPolynomialFunction1D[]> quadraticsFirstDerivative_Renamed;

		internal Bound(DoubleArray xValues, DoubleArray yValues) : base(xValues, yValues)
		{
		  this.xValues = xValues.toArrayUnsafe();
		  this.yValues = yValues.toArrayUnsafe();
		  this.intervalCount = xValues.size() - 1;
		  this.quadratics_Renamed = quadratics(this.xValues, this.yValues, this.intervalCount);
		  this.quadraticsFirstDerivative_Renamed = Suppliers.memoize(() => quadraticsFirstDerivative(this.xValues, this.yValues, this.intervalCount));
		}

		internal Bound(Bound @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight) : base(@base, extrapolatorLeft, extrapolatorRight)
		{
		  this.xValues = @base.xValues;
		  this.yValues = @base.yValues;
		  this.intervalCount = @base.intervalCount;
		  this.quadratics_Renamed = @base.quadratics_Renamed;
		  this.quadraticsFirstDerivative_Renamed = @base.quadraticsFirstDerivative_Renamed;
		}

		//-------------------------------------------------------------------------
		internal static RealPolynomialFunction1D[] quadratics(double[] x, double[] y, int intervalCount)
		{
		  if (intervalCount == 1)
		  {
			double a = y[1];
			double b = (y[1] - y[0]) / (x[1] - x[0]);
			return new RealPolynomialFunction1D[] {new RealPolynomialFunction1D(a, b)};
		  }
		  RealPolynomialFunction1D[] quadratic = new RealPolynomialFunction1D[intervalCount - 1];
		  for (int i = 1; i < intervalCount; i++)
		  {
			quadratic[i - 1] = Bound.quadratic(x, y, i);
		  }
		  return quadratic;
		}

		internal static RealPolynomialFunction1D quadratic(double[] x, double[] y, int index)
		{
		  double a = y[index];
		  double dx1 = x[index] - x[index - 1];
		  double dx2 = x[index + 1] - x[index];
		  double dy1 = y[index] - y[index - 1];
		  double dy2 = y[index + 1] - y[index];
		  double b = (dx1 * dy2 / dx2 + dx2 * dy1 / dx1) / (dx1 + dx2);
		  double c = (dy2 / dx2 - dy1 / dx1) / (dx1 + dx2);
		  return new RealPolynomialFunction1D(new double[] {a, b, c});
		}

		internal static RealPolynomialFunction1D[] quadraticsFirstDerivative(double[] x, double[] y, int intervalCount)
		{
		  if (intervalCount == 1)
		  {
			double b = (y[1] - y[0]) / (x[1] - x[0]);
			return new RealPolynomialFunction1D[] {new RealPolynomialFunction1D(b)};
		  }
		  else
		  {
			RealPolynomialFunction1D[] quadraticFirstDerivative = new RealPolynomialFunction1D[intervalCount - 1];
			for (int i = 1; i < intervalCount; i++)
			{
			  quadraticFirstDerivative[i - 1] = Bound.quadraticFirstDerivative(x, y, i);
			}
			return quadraticFirstDerivative;
		  }
		}

		internal static RealPolynomialFunction1D quadraticFirstDerivative(double[] x, double[] y, int index)
		{
		  double dx1 = x[index] - x[index - 1];
		  double dx2 = x[index + 1] - x[index];
		  double dy1 = y[index] - y[index - 1];
		  double dy2 = y[index + 1] - y[index];
		  double b = (dx1 * dy2 / dx2 + dx2 * dy1 / dx1) / (dx1 + dx2);
		  double c = (dy2 / dx2 - dy1 / dx1) / (dx1 + dx2);
		  return new RealPolynomialFunction1D(new double[] {b, 2.0 * c});
		}

		//-------------------------------------------------------------------------
		protected internal override double doInterpolate(double xValue)
		{
		  // x-value is less than the x-value of the last node (lowerIndex < intervalCount)
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  int higherIndex = lowerIndex + 1;
		  // at start of curve
		  if (lowerIndex == 0)
		  {
			RealPolynomialFunction1D quadratic = quadratics_Renamed[0];
			double x = xValue - xValues[1];
			return quadratic.applyAsDouble(x);
		  }
		  // at end of curve
		  if (higherIndex == intervalCount)
		  {
			RealPolynomialFunction1D quadratic = quadratics_Renamed[intervalCount - 2];
			double x = xValue - xValues[intervalCount - 1];
			return quadratic.applyAsDouble(x);
		  }
		  // normal case
		  RealPolynomialFunction1D quadratic1 = quadratics_Renamed[lowerIndex - 1];
		  RealPolynomialFunction1D quadratic2 = quadratics_Renamed[higherIndex - 1];
		  double w = WEIGHT_FUNCTION.getWeight((xValues[higherIndex] - xValue) / (xValues[higherIndex] - xValues[lowerIndex]));
		  return w * quadratic1.applyAsDouble(xValue - xValues[lowerIndex]) + (1 - w) * quadratic2.applyAsDouble(xValue - xValues[higherIndex]);
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  int higherIndex = lowerIndex + 1;
		  RealPolynomialFunction1D[] quadFirstDerivative = quadraticsFirstDerivative_Renamed.get();
		  // at start of curve, or only one interval
		  if (lowerIndex == 0 || intervalCount == 1)
		  {
			RealPolynomialFunction1D quadraticFirstDerivative = quadFirstDerivative[0];
			double x = xValue - xValues[1];
			return quadraticFirstDerivative.applyAsDouble(x);
		  }
		  // at end of curve
		  if (higherIndex >= intervalCount)
		  {
			RealPolynomialFunction1D quadraticFirstDerivative = quadFirstDerivative[intervalCount - 2];
			double x = xValue - xValues[intervalCount - 1];
			return quadraticFirstDerivative.applyAsDouble(x);
		  }
		  RealPolynomialFunction1D quadratic1 = quadratics_Renamed[lowerIndex - 1];
		  RealPolynomialFunction1D quadratic2 = quadratics_Renamed[higherIndex - 1];
		  RealPolynomialFunction1D quadratic1FirstDerivative = quadFirstDerivative[lowerIndex - 1];
		  RealPolynomialFunction1D quadratic2FirstDerivative = quadFirstDerivative[higherIndex - 1];
		  double w = WEIGHT_FUNCTION.getWeight((xValues[higherIndex] - xValue) / (xValues[higherIndex] - xValues[lowerIndex]));
		  return w * quadratic1FirstDerivative.applyAsDouble(xValue - xValues[lowerIndex]) + (1 - w) * quadratic2FirstDerivative.applyAsDouble(xValue - xValues[higherIndex]) + (quadratic2.applyAsDouble(xValue - xValues[higherIndex]) - quadratic1.applyAsDouble(xValue - xValues[lowerIndex])) / (xValues[higherIndex] - xValues[lowerIndex]);
		}

		protected internal override DoubleArray doParameterSensitivity(double xValue)
		{
		  int lowerIndex = lowerBoundIndex(xValue, xValues);
		  int higherIndex = lowerIndex + 1;
		  int n = xValues.Length;
		  double[] result = new double[n];
		  // at start of curve
		  if (lowerIndex == 0)
		  {
			double[] temp = quadraticSensitivities(xValues, xValue, 1);
			result[0] = temp[0];
			result[1] = temp[1];
			result[2] = temp[2];
			return DoubleArray.ofUnsafe(result);
		  }
		  // at end of curve
		  if (higherIndex == intervalCount)
		  {
			double[] temp = quadraticSensitivities(xValues, xValue, n - 2);
			result[n - 3] = temp[0];
			result[n - 2] = temp[1];
			result[n - 1] = temp[2];
			return DoubleArray.ofUnsafe(result);
		  }
		  // at last node
		  if (lowerIndex == intervalCount)
		  {
			result[n - 1] = 1;
			return DoubleArray.ofUnsafe(result);
		  }
		  double[] temp1 = quadraticSensitivities(xValues, xValue, lowerIndex);
		  double[] temp2 = quadraticSensitivities(xValues, xValue, higherIndex);
		  double w = WEIGHT_FUNCTION.getWeight((xValues[higherIndex] - xValue) / (xValues[higherIndex] - xValues[lowerIndex]));
		  result[lowerIndex - 1] = w * temp1[0];
		  result[lowerIndex] = w * temp1[1] + (1 - w) * temp2[0];
		  result[higherIndex] = w * temp1[2] + (1 - w) * temp2[1];
		  result[higherIndex + 1] = (1 - w) * temp2[2];
		  return DoubleArray.ofUnsafe(result);
		}

		internal static double[] quadraticSensitivities(double[] xValues, double x, int i)
		{
		  double[] result = new double[3];
		  double deltaX = x - xValues[i];
		  double h1 = xValues[i] - xValues[i - 1];
		  double h2 = xValues[i + 1] - xValues[i];
		  result[0] = deltaX * (deltaX - h2) / h1 / (h1 + h2);
		  result[1] = 1 + deltaX * (h2 - h1 - deltaX) / h1 / h2;
		  result[2] = deltaX * (h1 + deltaX) / (h1 + h2) / h2;
		  return result;
		}

		public override BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{

		  return new Bound(this, extrapolatorLeft, extrapolatorRight);
		}
	  }

	}

}