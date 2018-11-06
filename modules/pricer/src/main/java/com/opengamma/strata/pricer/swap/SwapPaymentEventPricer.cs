/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swap
{
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DispatchingSwapPaymentEventPricer = com.opengamma.strata.pricer.impl.swap.DispatchingSwapPaymentEventPricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwapPaymentEvent = com.opengamma.strata.product.swap.SwapPaymentEvent;

	/// <summary>
	/// Pricer for payment events.
	/// <para>
	/// This function provides the ability to price a <seealso cref="SwapPaymentEvent"/>.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe functions.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of event </param>
	public interface SwapPaymentEventPricer<T> where T : com.opengamma.strata.product.swap.SwapPaymentEvent
	{

	  /// <summary>
	  /// Returns the standard instance of the function.
	  /// <para>
	  /// Use this method to avoid a direct dependency on the implementation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the payment event pricer </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static SwapPaymentEventPricer<com.opengamma.strata.product.swap.SwapPaymentEvent> standard()
	//  {
	//	return DispatchingSwapPaymentEventPricer.DEFAULT;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of a single payment event.
	  /// <para>
	  /// The amount is expressed in the currency of the event.
	  /// This returns the value of the event with discounting.
	  /// </para>
	  /// <para>
	  /// The payment date of the event should not be in the past.
	  /// The result of this method for payment dates in the past is undefined.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="event">  the event </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the event </returns>
	  double presentValue(T @event, RatesProvider provider);

	  /// <summary>
	  /// Calculates the present value sensitivity of a single payment event.
	  /// <para>
	  /// The present value sensitivity of the event is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="event">  the event </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value curve sensitivity of the event </returns>
	  PointSensitivityBuilder presentValueSensitivity(T @event, RatesProvider provider);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forecast value of a single payment event.
	  /// <para>
	  /// The amount is expressed in the currency of the event.
	  /// This returns the value of the event without discounting.
	  /// </para>
	  /// <para>
	  /// The payment date of the event should not be in the past.
	  /// The result of this method for payment dates in the past is undefined.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="event">  the event </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value of the event </returns>
	  double forecastValue(T @event, RatesProvider provider);

	  /// <summary>
	  /// Calculates the forecast value sensitivity of a single payment event.
	  /// <para>
	  /// The forecast value sensitivity of the event is the sensitivity of the forecast value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="event">  the event </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value curve sensitivity of the event </returns>
	  PointSensitivityBuilder forecastValueSensitivity(T @event, RatesProvider provider);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Explains the present value of a single payment event.
	  /// <para>
	  /// This adds information to the <seealso cref="ExplainMapBuilder"/> to aid understanding of the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="event">  the event </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="builder">  the builder to populate </param>
	  void explainPresentValue(T @event, RatesProvider provider, ExplainMapBuilder builder);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of a single payment event.
	  /// </summary>
	  /// <param name="event">  the event </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  MultiCurrencyAmount currencyExposure(T @event, RatesProvider provider);

	  /// <summary>
	  /// Calculates the current cash of a single payment event.
	  /// </summary>
	  /// <param name="event">  the event </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash </returns>
	  double currentCash(T @event, RatesProvider provider);
	}

}