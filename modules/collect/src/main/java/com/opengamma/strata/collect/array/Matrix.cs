/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.array
{
	/// <summary>
	/// Base interface for all matrix types.
	/// <para>
	/// An n-dimensional matrix of elements of a fixed size.
	/// A 1-dimensional matrix is typically known as an array.
	/// </para>
	/// </summary>
	public interface Matrix
	{

	  /// <summary>
	  /// Gets the number of dimensions of the matrix.
	  /// <para>
	  /// Each matrix type has a fixed number of dimensions, returned by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the dimensions of the matrix </returns>
	  int dimensions();

	  /// <summary>
	  /// Gets the size of the matrix.
	  /// <para>
	  /// This is the total number of elements in the matrix.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the size of the matrix </returns>
	  int size();

	}

}