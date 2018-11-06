/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using ArrayRealVector = org.apache.commons.math3.linear.ArrayRealVector;
	using DecompositionSolver = org.apache.commons.math3.linear.DecompositionSolver;
	using QRDecomposition = org.apache.commons.math3.linear.QRDecomposition;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;

	/// <summary>
	/// Wrapper for results of the Commons implementation of QR Decomposition (<seealso cref="QRDecompositionCommons"/>).
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public class QRDecompositionCommonsResult : QRDecompositionResult
	{

	  private readonly DoubleMatrix _q;
	  private readonly DoubleMatrix _r;
	  private readonly DoubleMatrix _qTranspose;
	  private readonly DecompositionSolver _solver;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="qr"> The result of the QR decomposition, not null </param>
	  public QRDecompositionCommonsResult(QRDecomposition qr)
	  {
		ArgChecker.notNull(qr, "qr");
		_q = CommonsMathWrapper.unwrap(qr.Q);
		_r = CommonsMathWrapper.unwrap(qr.R);
		_qTranspose = _q.transpose();
		_solver = qr.Solver;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleMatrix Q
	  {
		  get
		  {
			return _q;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleMatrix QT
	  {
		  get
		  {
			return _qTranspose;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual DoubleMatrix R
	  {
		  get
		  {
			return _r;
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