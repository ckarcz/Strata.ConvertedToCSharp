/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;

	/// <summary>
	/// A period over which interest is accrued with a single payment calculated using a notional.
	/// <para>
	/// This is a single payment period within a swap leg.
	/// The amount of the payment is defined by implementations of this interface.
	/// It is typically based on a rate of interest.
	/// </para>
	/// <para>
	/// This interface imposes few restrictions on the payment periods.
	/// It extends <seealso cref="SwapPaymentPeriod"/> to require that the period is based on a notional amount.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// </para>
	/// </summary>
	public interface NotionalPaymentPeriod : SwapPaymentPeriod
	{

	  /// <summary>
	  /// The notional amount, positive if receiving, negative if paying.
	  /// <para>
	  /// This is the notional amount applicable during the period.
	  /// The currency may differ from that returned by <seealso cref="#getCurrency()"/>,
	  /// for example if the swap contains an FX reset.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the notional amount of the period </returns>
	  CurrencyAmount NotionalAmount {get;}

	  /// <summary>
	  /// Gets the FX reset observation, optional.
	  /// <para>
	  /// This property is used when the defined amount of the notional is specified in
	  /// a currency other than the currency of the swap leg. When this occurs, the notional
	  /// amount has to be converted using an FX rate to the swap leg currency.
	  /// </para>
	  /// <para>
	  /// The FX reset definition must be valid. The currency of the period and the currency
	  /// of the notional must differ, and the currency pair must be that of the observation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the optional FX reset observation </returns>
	  Optional<FxIndexObservation> FxResetObservation {get;}

	}

}