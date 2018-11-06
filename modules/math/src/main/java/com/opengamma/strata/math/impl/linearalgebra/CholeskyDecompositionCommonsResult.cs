/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using ArrayRealVector = org.apache.commons.math3.linear.ArrayRealVector;
	using CholeskyDecomposition = org.apache.commons.math3.linear.CholeskyDecomposition;
	using DecompositionSolver = org.apache.commons.math3.linear.DecompositionSolver;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;

	/// <summary>
	/// Wrapper for results of the Commons implementation of Cholesky decomposition (<seealso cref="CholeskyDecompositionCommons"/>).
	/// </summary>
	public class CholeskyDecompositionCommonsResult : CholeskyDecompositionResult
	{

	  private readonly double _determinant;
	  private readonly DoubleMatrix _l;
	  private readonly DoubleMatrix _lt;
	  private readonly DecompositionSolver _solver;

	  /// <summary>
	  /// Constructor. </summary>
	  /// <param name="ch"> The result of the Cholesky decomposition. </param>
	  public CholeskyDecompositionCommonsResult(CholeskyDecomposition ch)
	  {
		ArgChecker.notNull(ch, "Cholesky decomposition");
		_determinant = ch.Determinant;
		_l = CommonsMathWrapper.unwrap(ch.L);
		_lt = CommonsMathWrapper.unwrap(ch.LT);
		_solver = ch.Solver;
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

	  public virtual DoubleMatrix L
	  {
		  get
		  {
			return _l;
		  }
	  }

	  public virtual DoubleMatrix LT
	  {
		  get
		  {
			return _lt;
		  }
	  }

	  public virtual double Determinant
	  {
		  get
		  {
			return _determinant;
		  }
	  }

	}

}