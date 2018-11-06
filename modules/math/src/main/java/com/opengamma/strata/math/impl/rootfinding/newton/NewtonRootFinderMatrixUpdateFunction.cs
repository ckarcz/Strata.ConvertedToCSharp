/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// 
	//CSOFF: JavadocMethod
	public interface NewtonRootFinderMatrixUpdateFunction
	{

	  // TODO might be better to pass in NewtonVectorRootFinder.DataBundle as many of these arguments are not used.
	  DoubleMatrix getUpdatedMatrix(System.Func<DoubleArray, DoubleMatrix> jacobianFunction, DoubleArray x, DoubleArray deltaX, DoubleArray deltaY, DoubleMatrix matrix);

	}

}