/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;

	/// 
	//CSOFF: JavadocMethod
	public class ShermanMorrisonMatrixUpdateFunction : NewtonRootFinderMatrixUpdateFunction
	{

	  private readonly MatrixAlgebra _algebra;

	  public ShermanMorrisonMatrixUpdateFunction(MatrixAlgebra algebra)
	  {
		ArgChecker.notNull(algebra, "algebra");
		_algebra = algebra;
	  }

	  public virtual DoubleMatrix getUpdatedMatrix(System.Func<DoubleArray, DoubleMatrix> g, DoubleArray x, DoubleArray deltaX, DoubleArray deltaY, DoubleMatrix matrix)
	  {

		ArgChecker.notNull(deltaX, "deltaX");
		ArgChecker.notNull(deltaY, "deltaY");
		ArgChecker.notNull(matrix, "matrix");
		DoubleArray v1 = (DoubleArray) _algebra.multiply(deltaX, matrix);
		double length = _algebra.getInnerProduct(v1, deltaY);
		if (length == 0)
		{
		  return matrix;
		}
		v1 = (DoubleArray) _algebra.scale(v1, 1.0 / length);
		DoubleArray v2 = (DoubleArray) _algebra.subtract(deltaX, _algebra.multiply(matrix, deltaY));
		DoubleMatrix m = _algebra.getOuterProduct(v2, v1);
		return (DoubleMatrix) _algebra.add(matrix, m);
	  }

	}

}