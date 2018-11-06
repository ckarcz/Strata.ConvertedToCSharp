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

	/// 
	public class NewtonDefaultUpdateFunction : NewtonRootFinderMatrixUpdateFunction
	{

	  public virtual DoubleMatrix getUpdatedMatrix(System.Func<DoubleArray, DoubleMatrix> jacobianFunction, DoubleArray x, DoubleArray deltaX, DoubleArray deltaY, DoubleMatrix matrix)
	  {

		ArgChecker.notNull(jacobianFunction, "jacobianFunction");
		ArgChecker.notNull(x, "x");
		return jacobianFunction(x);
	  }

	}

}