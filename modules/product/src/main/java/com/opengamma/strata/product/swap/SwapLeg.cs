/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataId = com.opengamma.strata.basics.ReferenceDataId;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using Index = com.opengamma.strata.basics.index.Index;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// A single leg of a swap.
	/// <para>
	/// A swap leg is one element of a <seealso cref="Swap"/>.
	/// In most cases, a swap has two legs, one expressing the obligations of the seller
	/// and one expressing the obligations of the buyer. However, it is possible to
	/// represent more complex swaps, with one, three or more legs.
	/// </para>
	/// <para>
	/// This interface imposes few restrictions on the swap leg. A leg must have a start and
	/// end date, where the start date can be before or after the date that the swap is traded.
	/// A single swap leg must produce payments in a single currency.
	/// </para>
	/// <para>
	/// In most cases, a swap will consist of a list of payment periods, but this is not
	/// required by this interface. The <seealso cref="ResolvedSwapLeg"/> class, which this leg can
	/// be converted to, does define the swap in terms of payment periods.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// </para>
	/// </summary>
	public interface SwapLeg : Resolvable<ResolvedSwapLeg>
	{

	  /// <summary>
	  /// Gets the type of the leg, such as Fixed or Ibor.
	  /// <para>
	  /// This provides a high level categorization of the swap leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the leg type </returns>
	  SwapLegType Type {get;}

	  /// <summary>
	  /// Gets whether the leg is pay or receive.
	  /// <para>
	  /// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
	  /// A value of 'Receive' implies that the resulting amount is received from the counterparty.
	  /// Note that negative interest rates can result in a payment in the opposite
	  /// direction to that implied by this indicator.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the pay receive flag </returns>
	  PayReceive PayReceive {get;}

	  /// <summary>
	  /// Gets the accrual start date of the leg.
	  /// <para>
	  /// This is the first accrual date in the leg, often known as the effective date.
	  /// </para>
	  /// <para>
	  /// Defined as the effective date by the 2006 ISDA definitions article 3.2.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date of the leg </returns>
	  AdjustableDate StartDate {get;}

	  /// <summary>
	  /// Gets the accrual end date of the leg.
	  /// <para>
	  /// This is the last accrual date in the leg, often known as the termination date.
	  /// </para>
	  /// <para>
	  /// Defined as the termination date by the 2006 ISDA definitions article 3.3.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date of the leg </returns>
	  AdjustableDate EndDate {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment currency of the leg.
	  /// <para>
	  /// A swap leg has a single payment currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the payment currency of the leg </returns>
	  Currency Currency {get;}

	  /// <summary>
	  /// Returns the set of currencies referred to by the leg.
	  /// <para>
	  /// This returns the complete set of currencies for the leg, not just the payment currencies.
	  /// For example, if there is an FX reset, then this set contains both the currency of the
	  /// payment and the currency of the notional.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of currencies referred to by this leg </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.currency.Currency> allCurrencies()
	//  {
	//	ImmutableSet.Builder<Currency> builder = ImmutableSet.builder();
	//	collectCurrencies(builder);
	//	return builder.build();
	//  }

	  /// <summary>
	  /// Collects all the currencies referred to by this leg.
	  /// <para>
	  /// This collects the complete set of currencies for the leg, not just the payment currencies.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="builder">  the builder to populate </param>
	  void collectCurrencies(ImmutableSet.Builder<Currency> builder);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the set of indices referred to by the leg.
	  /// <para>
	  /// A leg will typically refer to at least one index, such as 'GBP-LIBOR-3M'.
	  /// Calling this method will return the complete list of indices, including
	  /// any associated with FX reset.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of indices referred to by this leg </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.index.Index> allIndices()
	//  {
	//	ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
	//	collectIndices(builder);
	//	return builder.build();
	//  }

	  /// <summary>
	  /// Collects all the indices referred to by this leg.
	  /// <para>
	  /// A swap leg will typically refer to at least one index, such as 'GBP-LIBOR-3M'.
	  /// Each index that is referred to must be added to the specified builder.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="builder">  the builder to populate </param>
	  void collectIndices(ImmutableSet.Builder<Index> builder);

	  /// <summary>
	  /// Resolves this swap leg using the specified reference data.
	  /// <para>
	  /// This converts the swap leg to the equivalent resolved form.
	  /// All <seealso cref="ReferenceDataId"/> identifiers in this instance will be resolved.
	  /// The resolved form will typically be a type that is optimized for pricing.
	  /// </para>
	  /// <para>
	  /// Resolved objects may be bound to data that changes over time, such as holiday calendars.
	  /// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	  /// Care must be taken when placing the resolved form in a cache or persistence layer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the resolved instance </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid definition </exception>
	  ResolvedSwapLeg resolve(ReferenceData refData);

	}

}