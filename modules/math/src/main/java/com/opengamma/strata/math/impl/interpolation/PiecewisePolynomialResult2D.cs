using System.Collections.Generic;

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
	/// Result of 2D interpolation.
	/// <para>
	/// Result by piecewise polynomial f(x0,x1) containing
	/// _knots0: Positions of knots in x0 direction
	/// _knots1: Positions of knots in x1 direction
	/// _coefMatrix: Coefficient matrix whose (i,j) element is a DoubleMatrix containing coefficients for the square, _knots0_i < x0 < _knots0_{i+1}, _knots1_j < x1 < _knots1_{j+1},
	/// Each DoubleMatrix is c_ij where f(x0,x1) = sum_{i=0}^{order0-1} sum_{j=0}^{order1-1} coefMat_{ij} (x0-knots0_i)^{order0-1-i} (x1-knots1_j)^{order0-1-j}
	/// _nIntervals: Number of intervals in x0 direction and x1 direction, respectively, which should be (Number of knots) - 1
	/// _order: Number of coefficients in polynomial in terms of x0 and x1, respectively, which is equal to (polynomial degree) + 1
	/// </para>
	/// </summary>
	public class PiecewisePolynomialResult2D
	{

	  private readonly DoubleArray _knots0;
	  private readonly DoubleArray _knots1;
	  private readonly DoubleMatrix[][] _coefMatrix;
	  private readonly int[] _nIntervals;
	  private readonly int[] _order;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="knots0"> The knots in the x0 direction </param>
	  /// <param name="knots1"> The knots in the x1 direction </param>
	  /// <param name="coefMatrix"> The coefficient matrix </param>
	  /// <param name="order"> The order of the polynomial </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public PiecewisePolynomialResult2D(final com.opengamma.strata.collect.array.DoubleArray knots0, final com.opengamma.strata.collect.array.DoubleArray knots1, final com.opengamma.strata.collect.array.DoubleMatrix[][] coefMatrix, final int[] order)
	  public PiecewisePolynomialResult2D(DoubleArray knots0, DoubleArray knots1, DoubleMatrix[][] coefMatrix, int[] order)
	  {

		_knots0 = knots0;
		_knots1 = knots1;
		_coefMatrix = coefMatrix;
		_nIntervals = new int[2];
		_nIntervals[0] = knots0.size() - 1;
		_nIntervals[1] = knots1.size() - 1;
		_order = order;
	  }

	  /// <summary>
	  /// Access _knots0 and _knots1. </summary>
	  /// <returns> _knots0 and _knots1 contained in a ArrayList </returns>
	  public virtual List<DoubleArray> Knots2D
	  {
		  get
		  {
	//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
	//ORIGINAL LINE: final java.util.ArrayList<com.opengamma.strata.collect.array.DoubleArray> res = new java.util.ArrayList<>();
			List<DoubleArray> res = new List<DoubleArray>();
			res.Add(_knots0);
			res.Add(_knots1);
    
			return res;
		  }
	  }

	  /// <summary>
	  /// Access _knots0. </summary>
	  /// <returns> _knots0 </returns>
	  public virtual DoubleArray Knots0
	  {
		  get
		  {
			return _knots0;
		  }
	  }

	  /// <summary>
	  /// Access _knots1. </summary>
	  /// <returns> knots1 </returns>
	  public virtual DoubleArray Knots1
	  {
		  get
		  {
			return _knots1;
		  }
	  }

	  /// <summary>
	  /// Access _coefMatrix. </summary>
	  /// <returns> _coefMatrix </returns>
	  public virtual DoubleMatrix[][] Coefs
	  {
		  get
		  {
			return _coefMatrix;
		  }
	  }

	  /// <summary>
	  /// Access _nIntervals. </summary>
	  /// <returns> _nIntervals </returns>
	  public virtual int[] NumberOfIntervals
	  {
		  get
		  {
			return _nIntervals;
		  }
	  }

	  /// <summary>
	  /// Access _order. </summary>
	  /// <returns> _order </returns>
	  public virtual int[] Order
	  {
		  get
		  {
			return _order;
		  }
	  }

	}

}