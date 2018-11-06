/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory.OG_ALGEBRA;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Matrix = com.opengamma.strata.collect.array.Matrix;

	/// 
	public class BroydenMatrixUpdateFunction : NewtonRootFinderMatrixUpdateFunction
	{

	  public virtual DoubleMatrix getUpdatedMatrix(System.Func<DoubleArray, DoubleMatrix> j, DoubleArray x, DoubleArray deltaX, DoubleArray deltaY, DoubleMatrix matrix)
	  {

		ArgChecker.notNull(deltaX, "deltaX");
		ArgChecker.notNull(deltaY, "deltaY");
		ArgChecker.notNull(matrix, "matrix");
		double length2 = OG_ALGEBRA.getInnerProduct(deltaX, deltaX);
		if (length2 == 0.0)
		{
		  return matrix;
		}
		Matrix temp = OG_ALGEBRA.subtract(deltaY, OG_ALGEBRA.multiply(matrix, deltaX));
		temp = OG_ALGEBRA.scale(temp, 1.0 / length2);
		return (DoubleMatrix) OG_ALGEBRA.add(matrix, OG_ALGEBRA.getOuterProduct(temp, deltaX));
	  }

	}

}