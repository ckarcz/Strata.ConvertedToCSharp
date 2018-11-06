/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataId = com.opengamma.strata.basics.ReferenceDataId;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using Resolvable = com.opengamma.strata.basics.Resolvable;

	/// <summary>
	/// A trade that can to be resolved using reference data.
	/// <para>
	/// Resolvable trades are the primary definition of a trade that applications work with.
	/// They are resolved when necessary for use with pricers, locking in specific reference data.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the resolved trade </param>
	public interface ResolvableTrade<T> : Trade, Resolvable<T> where T : ResolvedTrade
	{

	  /// <summary>
	  /// Resolves this trade using the specified reference data.
	  /// <para>
	  /// This converts this trade to the equivalent resolved form.
	  /// All <seealso cref="ReferenceDataId"/> identifiers in this instance will be resolved.
	  /// The resulting <seealso cref="ResolvedTrade"/> is optimized for pricing.
	  /// </para>
	  /// <para>
	  /// Resolved objects may be bound to data that changes over time, such as holiday calendars.
	  /// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	  /// Care must be taken when placing the resolved form in a cache or persistence layer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the resolved trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid definition </exception>
	  T resolve(ReferenceData refData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  ResolvableTrade<T> withInfo(TradeInfo info);

	}

}