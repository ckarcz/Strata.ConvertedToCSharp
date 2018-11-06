using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Helper class to hold the knots and polynomial degree that specify a set of basis functions.
	/// </summary>
	public sealed class BasisFunctionKnots
	{

	  private readonly double[] _knots;
	  private readonly int _degree;
	  private readonly int _nSplines;

	  /// <summary>
	  /// Generate knots uniformly in the range xa and xb and knots outside this range to support the basis functions on
	  /// the edge of the range. </summary>
	  /// <param name="xa"> start of the range </param>
	  /// <param name="xb"> end of the range </param>
	  /// <param name="nKnots"> number of knots in the range (internal knots) </param>
	  /// <param name="degree"> the polynomial degree of the basis functions (this will determine how many external knots are required) </param>
	  /// <returns> a BasisFunctionKnots instance </returns>
	  public static BasisFunctionKnots fromUniform(double xa, double xb, int nKnots, int degree)
	  {
		ArgChecker.isTrue(xb > xa, "Require xb > xa, values are xa = {}, xb = {}", xa, xb);
		ArgChecker.notNegative(degree, "degree");
		ArgChecker.isTrue(nKnots - degree > 0, "Require at least {} knots for degree {}, only given {}", degree + 1, degree, nKnots);

		int nTotalKnots = nKnots + 2 * degree; // this is the total number of knots, including those outside the range
		int nSplines = nKnots + degree - 1;
		double[] knots = new double[nTotalKnots];
		double dx = (xb - xa) / (nKnots - 1);

		// knots to the left and right of the range
		for (int i = 0; i < degree; i++)
		{
		  knots[i] = (i - degree) * dx + xa;
		  knots[degree + nKnots + i] = xb + dx * (i + 1);
		}
		// knots in the main range
		for (int i = 0; i < nKnots - 1; i++)
		{
		  knots[i + degree] = xa + i * dx;
		}
		knots[nKnots + degree - 1] = xb;
		return new BasisFunctionKnots(knots, degree, nSplines);
	  }

	  /// <summary>
	  /// Generate a set of knots capable of supporting the given degree of basis functions. The given knots are used inside
	  /// the range, with knots generated outside this range to support the basis functions on the edge of the range </summary>
	  /// <param name="internalKnots"> the internal knots. The start of the range is the first knot and the end is the last. </param>
	  /// <param name="degree"> the polynomial degree of the basis functions (this will determine how many external knots are required) </param>
	  /// <returns> a BasisFunctionKnots instance </returns>
	  public static BasisFunctionKnots fromInternalKnots(double[] internalKnots, int degree)
	  {
		ArgChecker.notEmpty(internalKnots, "knots");
		ArgChecker.notNegative(degree, "degree");
		int nInterKnots = internalKnots.Length;
		ArgChecker.isTrue(nInterKnots - degree > 0, "Require at least {} knots for degree {}, only given {}", degree + 1, degree, nInterKnots);

		// check knots are ascending
		for (int i = 1; i < nInterKnots; i++)
		{
		  ArgChecker.isTrue(internalKnots[i] - internalKnots[i - 1] > 0, "knots are not ascending");
		}

		int nSplines = nInterKnots + degree - 1;

		int nTotalKnots = nInterKnots + 2 * degree; // add in extra knots outside the range to handle basis functions on the edge
		double[] knots = new double[nTotalKnots];

		double dxa = internalKnots[1] - internalKnots[0];
		double dxb = internalKnots[nInterKnots - 1] - internalKnots[nInterKnots - 2];
		// knots to the left and right of the range
		for (int i = 0; i < degree; i++)
		{
		  knots[i] = (i - degree) * dxa + internalKnots[0];
		  knots[degree + nInterKnots + i] = internalKnots[nInterKnots - 1] + dxb * (i + 1);
		}
		// knots in the main range
		Array.Copy(internalKnots, 0, knots, degree, nInterKnots);
		return new BasisFunctionKnots(knots, degree, nSplines);
	  }

	  /// <summary>
	  /// Generate a set of knots capable of supporting the given degree of basis functions. All the knots, including those
	  /// outside the range must be supplied - the first and last degree knots are outside the range (e.g. for degree = 2, the
	  /// first and last two knots are out side the range and exist to support the basis functions on the edge of the range. </summary>
	  /// <param name="knots"> The total set of knots - must be strictly acceding </param>
	  /// <param name="degree"> the polynomial degree of the basis functions </param>
	  /// <returns> a BasisFunctionKnots instance </returns>
	  public static BasisFunctionKnots fromKnots(double[] knots, int degree)
	  {
		ArgChecker.notEmpty(knots, "knots");
		ArgChecker.notNegative(degree, "degree");
		int nKnots = knots.Length;
		ArgChecker.isTrue(nKnots - 3 * degree > 0, "Require at least {} knots for degree {}, only given {}", 3 * degree + 1, degree, nKnots);

		// check knots are ascending
		for (int i = 1; i < nKnots; i++)
		{
		  ArgChecker.isTrue(knots[i] - knots[i - 1] > 0, "knots are not ascending");
		}
		int nSplines = nKnots - degree - 1;
		return new BasisFunctionKnots(knots, degree, nSplines);
	  }

	  private BasisFunctionKnots(double[] knots, int degree, int nSplines)
	  {
		_knots = knots;
		_degree = degree;
		_nSplines = nSplines;
	  }

	  /// <summary>
	  /// Get the full set of knots. </summary>
	  /// <returns> the knots </returns>
	  public double[] Knots
	  {
		  get
		  {
			return _knots.Clone();
		  }
	  }

	  /// <summary>
	  /// The number of knots. </summary>
	  /// <returns> number of knots </returns>
	  public int NumKnots
	  {
		  get
		  {
			return _knots.Length;
		  }
	  }

	  /// <summary>
	  /// the polynomial degree of the basis functions. </summary>
	  /// <returns> the degree </returns>
	  public int Degree
	  {
		  get
		  {
			return _degree;
		  }
	  }

	  /// <summary>
	  /// The number of basis splines of the degree this set of knots will support. </summary>
	  /// <returns> number of splines </returns>
	  public int NumSplines
	  {
		  get
		  {
			return _nSplines;
		  }
	  }
	}

}