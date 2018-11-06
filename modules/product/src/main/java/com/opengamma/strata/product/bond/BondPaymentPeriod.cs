/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// A period over which interest is accrued with a single payment.
	/// <para>
	/// A single payment period within a swap leg.
	/// The amount of the payment is defined by implementations of this interface.
	/// It is typically based on a rate of interest.
	/// </para>
	/// <para>
	/// This interface imposes few restrictions on the payment periods.
	/// The period must have a payment date, currency and period dates.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// </para>
	/// </summary>
	public interface BondPaymentPeriod
	{

	  /// <summary>
	  /// Gets the date that the payment is made.
	  /// <para>
	  /// Each payment period has a single payment date.
	  /// This date has been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the payment date of the period </returns>
	  LocalDate PaymentDate {get;}

	  /// <summary>
	  /// Gets the currency of the payment resulting from the period.
	  /// <para>
	  /// This is the currency of the generated payment.
	  /// A period has a single currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency of the period </returns>
	  Currency Currency {get;}

	  /// <summary>
	  /// Gets the start date of the period.
	  /// <para>
	  /// This is the first accrual date in the period.
	  /// This date has been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date of the period </returns>
	  LocalDate StartDate {get;}

	  /// <summary>
	  /// Gets the end date of the period.
	  /// <para>
	  /// This is the last accrual date in the period.
	  /// This date has been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date of the period </returns>
	  LocalDate EndDate {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Collects all the indices referred to by this period.
	  /// <para>
	  /// A period will typically refer to at least one index, such as 'GBP-LIBOR-3M'.
	  /// Each index that is referred to must be added to the specified builder.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="builder">  the builder to use </param>
	  void collectIndices(ImmutableSet.Builder<Index> builder);

	  /// <summary>
	  /// Adjusts the payment date using the rules of the specified adjuster.
	  /// <para>
	  /// The adjuster is typically an instance of <seealso cref="BusinessDayAdjustment"/>.
	  /// Implementations must return a new instance unless they are immutable and no change occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="adjuster">  the adjuster to apply to the payment date </param>
	  /// <returns> the adjusted payment event </returns>
	  BondPaymentPeriod adjustPaymentDate(TemporalAdjuster adjuster);

	}

}