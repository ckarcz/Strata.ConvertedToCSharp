using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.matrix
{
	using LUDecomposition = org.apache.commons.math3.linear.LUDecomposition;
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;
	using RealVector = org.apache.commons.math3.linear.RealVector;
	using SingularValueDecomposition = org.apache.commons.math3.linear.SingularValueDecomposition;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Matrix = com.opengamma.strata.collect.array.Matrix;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;

	/// <summary>
	/// Provides matrix algebra by using the <a href = "http://commons.apache.org/math/api-2.1/index.html">Commons library</a>. 
	/// </summary>
	public class CommonsMatrixAlgebra : MatrixAlgebra
	{

	  public override double getCondition(Matrix m)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleMatrix)
		{
		  RealMatrix temp = CommonsMathWrapper.wrap((DoubleMatrix) m);
		  SingularValueDecomposition svd = new SingularValueDecomposition(temp);
		  return svd.ConditionNumber;
		}
		throw new System.ArgumentException("Can only find condition number of DoubleMatrix; have " + m.GetType());
	  }

	  public override double getDeterminant(Matrix m)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleMatrix)
		{
		  RealMatrix temp = CommonsMathWrapper.wrap((DoubleMatrix) m);
		  LUDecomposition lud = new LUDecomposition(temp);
		  return lud.Determinant;
		}
		throw new System.ArgumentException("Can only find determinant of DoubleMatrix; have " + m.GetType());
	  }

	  public override double getInnerProduct(Matrix m1, Matrix m2)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		if (m1 is DoubleArray && m2 is DoubleArray)
		{
		  RealVector t1 = CommonsMathWrapper.wrap((DoubleArray) m1);
		  RealVector t2 = CommonsMathWrapper.wrap((DoubleArray) m2);
		  return t1.dotProduct(t2);
		}
		throw new System.ArgumentException("Can only find inner product of DoubleArray; have " + m1.GetType() + " and " + m2.GetType());
	  }

	  public override DoubleMatrix getInverse(Matrix m)
	  {
		ArgChecker.notNull(m, "matrix was null");
		if (m is DoubleMatrix)
		{
		  RealMatrix temp = CommonsMathWrapper.wrap((DoubleMatrix) m);
		  SingularValueDecomposition sv = new SingularValueDecomposition(temp);
		  RealMatrix inv = sv.Solver.Inverse;
		  return CommonsMathWrapper.unwrap(inv);
		}
		throw new System.ArgumentException("Can only find inverse of DoubleMatrix; have " + m.GetType());
	  }

	  public override double getNorm1(Matrix m)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleArray)
		{
		  RealVector temp = CommonsMathWrapper.wrap((DoubleArray) m);
		  return temp.L1Norm;
		}
		else if (m is DoubleMatrix)
		{
		  RealMatrix temp = CommonsMathWrapper.wrap((DoubleMatrix) m);
		  // TODO find if commons implements this anywhere, so we are not doing it
		  // by hand
		  double max = 0.0;
		  for (int col = temp.ColumnDimension - 1; col >= 0; col--)
		  {
			max = Math.Max(max, temp.getColumnVector(col).L1Norm);
		  }
		  return max;

		}
		throw new System.ArgumentException("Can only find norm1 of DoubleMatrix; have " + m.GetType());
	  }

	  public override double getNorm2(Matrix m)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleArray)
		{
		  RealVector temp = CommonsMathWrapper.wrap((DoubleArray) m);
		  return temp.Norm;
		}
		else if (m is DoubleMatrix)
		{
		  RealMatrix temp = CommonsMathWrapper.wrap((DoubleMatrix) m);
		  SingularValueDecomposition svd = new SingularValueDecomposition(temp);
		  return svd.Norm;
		}
		throw new System.ArgumentException("Can only find norm2 of DoubleMatrix; have " + m.GetType());
	  }

	  public override double getNormInfinity(Matrix m)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleArray)
		{
		  RealVector temp = CommonsMathWrapper.wrap((DoubleArray) m);
		  return temp.LInfNorm;
		}
		else if (m is DoubleMatrix)
		{
		  RealMatrix temp = CommonsMathWrapper.wrap((DoubleMatrix) m);
		  //REVIEW Commons getNorm() is wrong - it returns the column norm
		  // TODO find if commons implements this anywhere, so we are not doing it
		  // by hand
		  double max = 0.0;
		  for (int row = temp.RowDimension - 1; row >= 0; row--)
		  {
			max = Math.Max(max, temp.getRowVector(row).L1Norm);
		  }
		  return max;
		}
		throw new System.ArgumentException("Can only find normInfinity of DoubleMatrix; have " + m.GetType());
	  }

	  public override DoubleMatrix getOuterProduct(Matrix m1, Matrix m2)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		if (m1 is DoubleArray && m2 is DoubleArray)
		{
		  RealVector t1 = CommonsMathWrapper.wrap((DoubleArray) m1);
		  RealVector t2 = CommonsMathWrapper.wrap((DoubleArray) m2);
		  return CommonsMathWrapper.unwrap(t1.outerProduct(t2));
		}
		throw new System.ArgumentException("Can only find outer product of DoubleArray; have " + m1.GetType() + " and " + m2.GetType());
	  }

	  public override DoubleMatrix getPower(Matrix m, int p)
	  {
		ArgChecker.notNull(m, "m");
		RealMatrix temp;
		if (m is DoubleMatrix)
		{
		  temp = CommonsMathWrapper.wrap((DoubleMatrix) m);
		}
		else
		{
		  throw new System.ArgumentException("Can only find powers of DoubleMatrix; have " + m.GetType());
		}
		return CommonsMathWrapper.unwrap(temp.power(p));
	  }

	  /// <summary>
	  /// Returns a real matrix raised to some real power 
	  /// Currently this method is limited to symmetric matrices only as Commons Math does not
	  /// support the diagonalization of asymmetric matrices.
	  /// </summary>
	  /// <param name="m"> The <strong>symmetric</strong> matrix to take the power of. </param>
	  /// <param name="p"> The power to raise to matrix to </param>
	  /// <returns> The result </returns>
	  public override DoubleMatrix getPower(Matrix m, double p)
	  {
		throw new System.NotSupportedException();
	  }

	  public override double getTrace(Matrix m)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleMatrix)
		{
		  RealMatrix temp = CommonsMathWrapper.wrap((DoubleMatrix) m);
		  return temp.Trace;
		}
		throw new System.ArgumentException("Can only find trace of DoubleMatrix; have " + m.GetType());
	  }

	  public override DoubleMatrix getTranspose(Matrix m)
	  {
		ArgChecker.notNull(m, "m");
		if (m is DoubleMatrix)
		{
		  RealMatrix temp = CommonsMathWrapper.wrap((DoubleMatrix) m);
		  return CommonsMathWrapper.unwrap(temp.transpose());
		}
		throw new System.ArgumentException("Can only find transpose of DoubleMatrix; have " + m.GetType());
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// The following combinations of input matrices m1 and m2 are allowed:
	  /// <ul>
	  /// <li> m1 = 2-D matrix, m2 = 2-D matrix, returns $\mathbf{C} = \mathbf{AB}$
	  /// <li> m1 = 2-D matrix, m2 = 1-D matrix, returns $\mathbf{C} = \mathbf{A}b$
	  /// </ul>
	  /// </summary>
	  public override Matrix multiply(Matrix m1, Matrix m2)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		ArgChecker.isTrue(!(m1 is DoubleArray), "Cannot have 1D matrix as first argument");
		if (m1 is DoubleMatrix)
		{
		  RealMatrix t1 = CommonsMathWrapper.wrap((DoubleMatrix) m1);
		  RealMatrix t2;
		  if (m2 is DoubleArray)
		  {
			t2 = CommonsMathWrapper.wrapAsMatrix((DoubleArray) m2);
		  }
		  else if (m2 is DoubleMatrix)
		  {
			t2 = CommonsMathWrapper.wrap((DoubleMatrix) m2);
		  }
		  else
		  {
			throw new System.ArgumentException("Can only have 1D or 2D matrix as second argument");
		  }
		  return CommonsMathWrapper.unwrap(t1.multiply(t2));
		}
		throw new System.ArgumentException("Can only multiply 2D and 1D matrices");
	  }

	}

}