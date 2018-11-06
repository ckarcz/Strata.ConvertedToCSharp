/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// Defines a mechanism for computing a rate.
	/// <para>
	/// A floating rate can be observed in a number of ways, including from one index,
	/// interpolating two indices, averaging an index on specific dates, overnight compounding
	/// and overnight averaging.
	/// </para>
	/// <para>
	/// Each implementation contains the necessary information to compute the rate.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// </para>
	/// </summary>
	public interface RateComputation
	{

	  /// <summary>
	  /// Collects all the indices referred to by this computation.
	  /// <para>
	  /// A computation will typically refer to one index, such as 'GBP-LIBOR-3M'.
	  /// Each index that is referred to must be added to the specified builder.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="builder">  the builder to use </param>
	  void collectIndices(ImmutableSet.Builder<Index> builder);

	}

}