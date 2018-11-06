/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Result of interpolation by piecewise polynomial containing
	/// knots: Positions of knots
	/// coefMatrix: Coefficient matrix whose i-th row vector is { a_n, a_{n-1}, ...} for the i-th interval, where a_n, a_{n-1},... are coefficients of f(x) = a_n (x-x_i)^n + a_{n-1} (x-x_i)^{n-1} + ....
	/// In multidimensional cases, coefficients for the i-th interval of the j-th spline is in (j*(i-1) + i) -th row vector.
	/// nIntervals: Number of intervals, which should be (Number of knots) - 1
	/// order: Number of coefficients in polynomial, which is equal to (polynomial degree) + 1
	/// dim: Number of splines
	/// which are in the super class, and 
	/// _coeffSense Node sensitivity of the coefficients _coeffSense[i].get(j, k) is \frac{\partial a^i_{n-j}}{\partial y_k}
	/// </summary>
	public class PiecewisePolynomialResultsWithSensitivity : PiecewisePolynomialResult
	{

	  private readonly DoubleMatrix[] _coeffSense;

	  /// 
	  /// <param name="knots">  the knots </param>
	  /// <param name="coefMatrix">  the coefMatrix </param>
	  /// <param name="order">  the order </param>
	  /// <param name="dim">  the dim </param>
	  /// <param name="coeffSense"> the sensitivity of the coefficients to the nodes (y-values) </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public PiecewisePolynomialResultsWithSensitivity(com.opengamma.strata.collect.array.DoubleArray knots, com.opengamma.strata.collect.array.DoubleMatrix coefMatrix, int order, int dim, final com.opengamma.strata.collect.array.DoubleMatrix[] coeffSense)
	  public PiecewisePolynomialResultsWithSensitivity(DoubleArray knots, DoubleMatrix coefMatrix, int order, int dim, DoubleMatrix[] coeffSense) : base(knots, coefMatrix, order, dim)
	  {
		if (dim != 1)
		{
		  throw new System.NotSupportedException();
		}
		ArgChecker.noNulls(coeffSense, "null coeffSense"); // coefficient
		_coeffSense = coeffSense;
	  }

	  /// <summary>
	  /// Access _coeffSense. </summary>
	  /// <returns> _coeffSense </returns>
	  public virtual DoubleMatrix[] CoefficientSensitivityAll
	  {
		  get
		  {
			return _coeffSense;
		  }
	  }

	  /// <summary>
	  /// Access _coeffSense for the i-th interval. </summary>
	  /// <param name="interval">  the interval </param>
	  /// <returns> _coeffSense for the i-th interval </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public com.opengamma.strata.collect.array.DoubleMatrix getCoefficientSensitivity(final int interval)
	  public virtual DoubleMatrix getCoefficientSensitivity(int interval)
	  {
		return _coeffSense[interval];
	  }

	  public override int GetHashCode()
	  {
		const int prime = 31;
		int result = base.GetHashCode();
		result = prime * result + Arrays.GetHashCode(_coeffSense);
		return result;
	  }

	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (!base.Equals(obj))
		{
		  return false;
		}
		if (!(obj is PiecewisePolynomialResultsWithSensitivity))
		{
		  return false;
		}
		PiecewisePolynomialResultsWithSensitivity other = (PiecewisePolynomialResultsWithSensitivity) obj;
		if (!Arrays.Equals(_coeffSense, other._coeffSense))
		{
		  return false;
		}
		return true;
	  }

	}

}