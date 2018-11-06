using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.random
{

	/// <summary>
	/// Generator of random numbers.
	/// </summary>
	public interface RandomNumberGenerator
	{

	  /// <summary>
	  /// Gets an array of random numbers.
	  /// </summary>
	  /// <param name="size">  the size of the resulting array </param>
	  /// <returns> the array of random numbers </returns>
	  double[] getVector(int size);

	  /// <summary>
	  /// Gets a list of random number arrays.
	  /// </summary>
	  /// <param name="arraySize">  the size of each resulting array </param>
	  /// <param name="listSize">  the size of the list </param>
	  /// <returns> the list of random number arrays </returns>
	  IList<double[]> getVectors(int arraySize, int listSize);

	}

}