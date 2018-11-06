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
	public interface NewtonRootFinderDirectionFunction
	{

	  DoubleArray getDirection(DoubleMatrix estimate, DoubleArray y);

	}

}