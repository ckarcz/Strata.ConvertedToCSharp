/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;

	/// <summary>
	/// A payment event, where a single payment is made between two counterparties.
	/// <para>
	/// Implementations of this interface represent a single payment event.
	/// Causes include exchange of notional amounts and fees.
	/// </para>
	/// <para>
	/// This interface imposes few restrictions on the payment events.
	/// The event must have a payment date and currency.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// </para>
	/// </summary>
	public interface SwapPaymentEvent
	{

	  /// <summary>
	  /// Gets the date that the payment is made.
	  /// <para>
	  /// Each payment event has a single payment date.
	  /// This date has been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the payment date </returns>
	  LocalDate PaymentDate {get;}

	  /// <summary>
	  /// Gets the currency of the payment resulting from the event.
	  /// <para>
	  /// This is the currency of the generated payment.
	  /// An event has a single currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency of the payment </returns>
	  Currency Currency {get;}

	  //-------------------------------------------------------------------------
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
	  SwapPaymentEvent adjustPaymentDate(TemporalAdjuster adjuster);

	}

}