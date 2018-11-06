using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{

	/// <summary>
	/// Interface for anything the provides a vector function which depends on some extraneous data.
	/// </summary>
	/// @param <T> the type of extraneous data </param>
	/// <seealso cref= VectorFunction </seealso>
	public interface VectorFunctionProvider<T>
	{

	  /// <summary>
	  /// Produces a vector function that maps from some 'model' parameters to values at the sample points.
	  /// </summary>
	  /// <param name="samplePoints">  the list of sample points </param>
	  /// <returns> a <seealso cref="VectorFunction"/> </returns>
	  VectorFunction from(IList<T> samplePoints);

	  /// <summary>
	  /// Produces a vector function that maps from some 'model' parameters to values at the sample points.
	  /// </summary>
	  /// <param name="samplePoints"> the array of sample points </param>
	  /// <returns> a <seealso cref="VectorFunction"/> </returns>
	  VectorFunction from(T[] samplePoints);

	}

}