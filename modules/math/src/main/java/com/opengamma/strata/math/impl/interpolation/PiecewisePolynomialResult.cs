/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Result of interpolation by piecewise polynomial containing
	/// _knots: Positions of knots
	/// _coefMatrix: Coefficient matrix whose i-th row vector is { a_n, a_{n-1}, ...} for the i-th interval, where a_n, a_{n-1},... are coefficients of f(x) = a_n (x-x_i)^n + a_{n-1} (x-x_i)^{n-1} + ....
	/// In multidimensional cases, coefficients for the i-th interval of the j-th spline is in (j*(i-1) + i) -th row vector.
	/// _nIntervals: Number of intervals, which should be (Number of knots) - 1
	/// _order: Number of coefficients in polynomial, which is equal to (polynomial degree) + 1
	/// _dim: Number of splines
	/// </summary>
	public class PiecewisePolynomialResult
	{

	  private DoubleArray _knots;
	  private DoubleMatrix _coefMatrix;
	  private int _nIntervals;
	  private int _order;
	  private int _dim;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="knots">  the knots </param>
	  /// <param name="coefMatrix">  the coefMatrix </param>
	  /// <param name="order">  the order </param>
	  /// <param name="dim">  the dim </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public PiecewisePolynomialResult(final com.opengamma.strata.collect.array.DoubleArray knots, final com.opengamma.strata.collect.array.DoubleMatrix coefMatrix, final int order, final int dim)
	  public PiecewisePolynomialResult(DoubleArray knots, DoubleMatrix coefMatrix, int order, int dim)
	  {

		_knots = knots;
		_coefMatrix = coefMatrix;
		_nIntervals = knots.size() - 1;
		_order = order;
		_dim = dim;

	  }

	  /// <summary>
	  /// Access _knots. </summary>
	  /// <returns> the knots </returns>
	  public virtual DoubleArray Knots
	  {
		  get
		  {
			return _knots;
		  }
	  }

	  /// <summary>
	  /// Access _coefMatrix. </summary>
	  /// <returns> Coefficient Matrix </returns>
	  public virtual DoubleMatrix CoefMatrix
	  {
		  get
		  {
			return _coefMatrix;
		  }
	  }

	  /// <summary>
	  /// Access _nIntervals. </summary>
	  /// <returns> Number of Intervals </returns>
	  public virtual int NumberOfIntervals
	  {
		  get
		  {
			return _nIntervals;
		  }
	  }

	  /// <summary>
	  /// Access _order. </summary>
	  /// <returns> Number of coefficients in polynomial; 2 if _nIntervals=1, 3 if _nIntervals=2, 4 otherwise </returns>
	  public virtual int Order
	  {
		  get
		  {
			return _order;
		  }
	  }

	  /// <summary>
	  /// Access _dim. </summary>
	  /// <returns> Dimension of spline  </returns>
	  public virtual int Dimensions
	  {
		  get
		  {
			return _dim;
		  }
	  }

	  public override int GetHashCode()
	  {
		const int prime = 31;
		int result = 1;
		result = prime * result + _coefMatrix.GetHashCode();
		result = prime * result + _dim;
		result = prime * result + _knots.GetHashCode();
		result = prime * result + _order;
		return result;
	  }

	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj == null)
		{
		  return false;
		}
		if (!(obj is PiecewisePolynomialResult))
		{
		  return false;
		}
		PiecewisePolynomialResult other = (PiecewisePolynomialResult) obj;
		if (!_coefMatrix.Equals(other._coefMatrix))
		{
		  return false;
		}
		if (_dim != other._dim)
		{
		  return false;
		}
		if (!_knots.Equals(other._knots))
		{
		  return false;
		}
		if (_order != other._order)
		{
		  return false;
		}
		return true;
	  }

	}

}