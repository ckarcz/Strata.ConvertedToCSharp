/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using ArrayRealVector = org.apache.commons.math3.linear.ArrayRealVector;
	using DecompositionSolver = org.apache.commons.math3.linear.DecompositionSolver;
	using LUDecomposition = org.apache.commons.math3.linear.LUDecomposition;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;

	/// <summary>
	/// Wrapper for results of the Commons implementation of LU decomposition (<seealso cref="LUDecompositionCommons"/>).
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public class LUDecompositionCommonsResult : LUDecompositionResult
	{
	  private readonly double _determinant;
	  private readonly DoubleMatrix _l;
	  private readonly DoubleMatrix _p;
	  private readonly int[] _pivot;
	  private readonly DecompositionSolver _solver;
	  private readonly DoubleMatrix _u;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="lu"> The result of the LU decomposition, not null. $\mathbf{L}$ cannot be singular. </param>
	  public LUDecompositionCommonsResult(LUDecomposition lu)
	  {
		ArgChecker.notNull(lu, "LU decomposition");
		ArgChecker.notNull(lu.L, "Matrix is singular; could not perform LU decomposition");
		_determinant = lu.Determinant;
		_l = CommonsMathWrapper.unwrap(lu.L);
		_p = CommonsMathWrapper.unwrap(lu.P);
		_pivot = lu.Pivot;
		_solver = lu.Solver;
		_u = CommonsMathWrapper.unwrap(lu.U);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double Determinant
	  {
		  get
		  {
			return _determinant;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleMatrix L
	  {
		  get
		  {
			return _l;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleMatrix P
	  {
		  get
		  {
			return _p;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual int[] Pivot
	  {
		  get
		  {
			return _pivot;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleMatrix U
	  {
		  get
		  {
			return _u;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleArray solve(DoubleArray b)
	  {
		ArgChecker.notNull(b, "b");
		return CommonsMathWrapper.unwrap(_solver.solve(CommonsMathWrapper.wrap(b)));
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double[] solve(double[] b)
	  {
		ArgChecker.notNull(b, "b");
		return _solver.solve(new ArrayRealVector(b)).toArray();
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleMatrix solve(DoubleMatrix b)
	  {
		ArgChecker.notNull(b, "b");
		return CommonsMathWrapper.unwrap(_solver.solve(CommonsMathWrapper.wrap(b)));
	  }

	}

}