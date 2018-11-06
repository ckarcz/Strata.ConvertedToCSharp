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
	public class InverseJacobianDirectionFunction : NewtonRootFinderDirectionFunction
	{

	  private readonly MatrixAlgebra _algebra;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="algebra">  the matrix </param>
	  public InverseJacobianDirectionFunction(MatrixAlgebra algebra)
	  {
		ArgChecker.notNull(algebra, "algebra");
		_algebra = algebra;
	  }

	  public virtual DoubleArray getDirection(DoubleMatrix estimate, DoubleArray y)
	  {
		ArgChecker.notNull(estimate, "estimate");
		ArgChecker.notNull(y, "y");
		return (DoubleArray) _algebra.multiply(estimate, y);
	  }

	}

}