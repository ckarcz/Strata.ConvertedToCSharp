﻿using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using Supplier = com.google.common.@base.Supplier;
	using Suppliers = com.google.common.@base.Suppliers;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using FunctionUtils = com.opengamma.strata.math.impl.FunctionUtils;
	using NaturalSplineInterpolator = com.opengamma.strata.math.impl.interpolation.NaturalSplineInterpolator;
	using NonnegativityPreservingCubicSplineInterpolator = com.opengamma.strata.math.impl.interpolation.NonnegativityPreservingCubicSplineInterpolator;
	using PiecewisePolynomialInterpolator = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialInterpolator;
	using PiecewisePolynomialResult = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResult;
	using PiecewisePolynomialResultsWithSensitivity = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;

	/// <summary>
	/// Natural spline interpolator with non-negativity filter.
	/// </summary>
	[Serializable]
	internal sealed class NaturalSplineNonnegativityCubicCurveInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "NaturalSplineNonnegativityCubic";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly CurveInterpolator INSTANCE = new NaturalSplineNonnegativityCubicCurveInterpolator();

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// Underlying matrix algebra.
	  /// </summary>
	  private static readonly MatrixAlgebra MA = new OGMatrixAlgebra();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private NaturalSplineNonnegativityCubicCurveInterpolator()
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
		internal readonly PiecewisePolynomialResult poly;
		internal readonly Supplier<PiecewisePolynomialResultsWithSensitivity> polySens;

		internal Bound(DoubleArray xValues, DoubleArray yValues) : base(xValues, yValues)
		{
		  this.xValues = xValues.toArrayUnsafe();
		  this.yValues = yValues.toArrayUnsafe();
		  PiecewisePolynomialInterpolator underlying = new NonnegativityPreservingCubicSplineInterpolator(new NaturalSplineInterpolator());
		  this.poly = underlying.interpolate(xValues.toArray(), yValues.toArray());
		  this.polySens = Suppliers.memoize(() => underlying.interpolateWithSensitivity(xValues.toArray(), yValues.toArray()));
		}

		internal Bound(Bound @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight) : base(@base, extrapolatorLeft, extrapolatorRight)
		{
		  this.xValues = @base.xValues;
		  this.yValues = @base.yValues;
		  this.poly = @base.poly;
		  this.polySens = @base.polySens;
		}

		//-------------------------------------------------------------------------
		internal static double evaluate(double xValue, DoubleArray knots, DoubleMatrix coefMatrix, int dimensions)
		{

		  // check for 1 less interval than knots 
		  int lowerBound = FunctionUtils.getLowerBoundIndex(knots, xValue);
		  int indicator = lowerBound == knots.size() - 1 ? lowerBound - 1 : lowerBound;
		  DoubleArray coefs = coefMatrix.row(dimensions * indicator);
		  return getValue(coefs.toArrayUnsafe(), xValue, knots.get(indicator));
		}

		internal static double differentiate(double xValue, DoubleArray knots, DoubleMatrix coefMatrix, int dimensions, int nCoefs, int numberOfIntervals)
		{

		  int rowCount = dimensions * numberOfIntervals;
		  int colCount = nCoefs - 1;
		  DoubleMatrix coef = DoubleMatrix.of(rowCount, colCount, (i, j) => coefMatrix.get(i, j) * (nCoefs - j - 1));
		  return evaluate(xValue, knots, coef, dimensions);
		}

		/// <param name="coefs">  {a_n,a_{n-1},...} of f(x) = a_n x^{n} + a_{n-1} x^{n-1} + .... </param>
		/// <param name="x">  the x </param>
		/// <param name="leftknot">  the knot specifying underlying interpolation function </param>
		/// <returns> the value of the underlying interpolation function at the value of x </returns>
		internal static double getValue(double[] coefs, double x, double leftknot)
		{
		  int nCoefs = coefs.Length;
		  double s = x - leftknot;
		  double res = coefs[0];
		  for (int i = 1; i < nCoefs; i++)
		  {
			res *= s;
			res += coefs[i];
		  }
		  return res;
		}

		//-------------------------------------------------------------------------
		protected internal override double doInterpolate(double xValue)
		{
		  return evaluate(xValue, poly.Knots, poly.CoefMatrix, poly.Dimensions);
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  int nCoefs = poly.Order;
		  int numberOfIntervals = poly.NumberOfIntervals;
		  return differentiate(xValue, poly.Knots, poly.CoefMatrix, poly.Dimensions, nCoefs, numberOfIntervals);
		}

		protected internal override DoubleArray doParameterSensitivity(double xValue)
		{
		  int interval = FunctionUtils.getLowerBoundIndex(poly.Knots, xValue);
		  if (interval == poly.Knots.size() - 1)
		  {
			interval--; // there is 1 less interval than knots
		  }
		  DoubleMatrix coefficientSensitivity = polySens.get().getCoefficientSensitivity(interval);
		  int nCoefs = coefficientSensitivity.rowCount();
		  double s = xValue - poly.Knots.get(interval);
		  DoubleArray res = coefficientSensitivity.row(0);
		  for (int i = 1; i < nCoefs; i++)
		  {
			res = (DoubleArray) MA.scale(res, s);
			res = (DoubleArray) MA.add(res, coefficientSensitivity.row(i));
		  }
		  return res;
		}

		public override BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{

		  return new Bound(this, extrapolatorLeft, extrapolatorRight);
		}
	  }

	}

}