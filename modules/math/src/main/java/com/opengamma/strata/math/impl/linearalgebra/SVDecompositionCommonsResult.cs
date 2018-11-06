/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using ArrayRealVector = org.apache.commons.math3.linear.ArrayRealVector;
	using DecompositionSolver = org.apache.commons.math3.linear.DecompositionSolver;
	using SingularValueDecomposition = org.apache.commons.math3.linear.SingularValueDecomposition;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;

	/// <summary>
	/// Wrapper for results of the Commons implementation of singular value decomposition <seealso cref="SVDecompositionCommons"/>.
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public class SVDecompositionCommonsResult : SVDecompositionResult
	{

	  private readonly double _condition;
	  private readonly double _norm;
	  private readonly int _rank;
	  private readonly DoubleMatrix _s;
	  private readonly double[] _singularValues;
	  private readonly DoubleMatrix _u;
	  private readonly DoubleMatrix _v;
	  private readonly DoubleMatrix _uTranspose;
	  private readonly DoubleMatrix _vTranspose;
	  private readonly DecompositionSolver _solver;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="svd"> The result of the SV decomposition, not null </param>
	  public SVDecompositionCommonsResult(SingularValueDecomposition svd)
	  {
		ArgChecker.notNull(svd, "svd");
		_condition = svd.ConditionNumber;
		_norm = svd.Norm;
		_rank = svd.Rank;
		_s = CommonsMathWrapper.unwrap(svd.S);
		_singularValues = svd.SingularValues;
		_u = CommonsMathWrapper.unwrap(svd.U);
		_uTranspose = CommonsMathWrapper.unwrap(svd.UT);
		_v = CommonsMathWrapper.unwrap(svd.V);
		_vTranspose = CommonsMathWrapper.unwrap(svd.VT);
		_solver = svd.Solver;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double ConditionNumber
	  {
		  get
		  {
			return _condition;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double Norm
	  {
		  get
		  {
			return _norm;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual int Rank
	  {
		  get
		  {
			return _rank;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleMatrix S
	  {
		  get
		  {
			return _s;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double[] SingularValues
	  {
		  get
		  {
			return _singularValues;
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
	  public virtual DoubleMatrix UT
	  {
		  get
		  {
			return _uTranspose;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleMatrix V
	  {
		  get
		  {
			return _v;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleMatrix VT
	  {
		  get
		  {
			return _vTranspose;
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