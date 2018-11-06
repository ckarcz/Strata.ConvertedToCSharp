using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.tuple
{

	/// <summary>
	/// Base interface for all tuple types.
	/// <para>
	/// An ordered list of elements of a fixed size, where each element can have a different type.
	/// </para>
	/// <para>
	/// All implementations must be final, immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface Tuple
	{

	  /// <summary>
	  /// Gets the number of elements held by this tuple.
	  /// <para>
	  /// Each tuple type has a fixed size, returned by this method.
	  /// For example, <seealso cref="Pair"/> returns 2.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the size of the tuple </returns>
	  int size();

	  /// <summary>
	  /// Gets the elements from this tuple as a list.
	  /// <para>
	  /// The list contains each element in the tuple in order.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the elements as a list </returns>
	  IList<object> elements();

	}

}