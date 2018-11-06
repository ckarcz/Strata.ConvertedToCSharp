/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.swap
{
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwapPaymentEventPricer = com.opengamma.strata.pricer.swap.SwapPaymentEventPricer;
	using FxResetNotionalExchange = com.opengamma.strata.product.swap.FxResetNotionalExchange;
	using NotionalExchange = com.opengamma.strata.product.swap.NotionalExchange;
	using SwapPaymentEvent = com.opengamma.strata.product.swap.SwapPaymentEvent;

	/// <summary>
	/// Pricer implementation for payment events using multiple dispatch.
	/// <para>
	/// Dispatches the request to the correct implementation.
	/// </para>
	/// </summary>
	public class DispatchingSwapPaymentEventPricer : SwapPaymentEventPricer<SwapPaymentEvent>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DispatchingSwapPaymentEventPricer DEFAULT = new DispatchingSwapPaymentEventPricer(DiscountingNotionalExchangePricer.DEFAULT, DiscountingFxResetNotionalExchangePricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="NotionalExchange"/>.
	  /// </summary>
	  private readonly SwapPaymentEventPricer<NotionalExchange> notionalExchangePricer;
	  /// <summary>
	  /// Pricer for <seealso cref="FxResetNotionalExchange"/>.
	  /// </summary>
	  private readonly SwapPaymentEventPricer<FxResetNotionalExchange> fxResetNotionalExchangePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="notionalExchangePricer">  the pricer for <seealso cref="NotionalExchange"/> </param>
	  /// <param name="fxResetNotionalExchangePricer">  the pricer for <seealso cref="FxResetNotionalExchange"/> </param>
	  public DispatchingSwapPaymentEventPricer(SwapPaymentEventPricer<NotionalExchange> notionalExchangePricer, SwapPaymentEventPricer<FxResetNotionalExchange> fxResetNotionalExchangePricer)
	  {
		this.notionalExchangePricer = ArgChecker.notNull(notionalExchangePricer, "notionalExchangePricer");
		this.fxResetNotionalExchangePricer = ArgChecker.notNull(fxResetNotionalExchangePricer, "fxResetNotionalExchangePricer");
	  }

	  //-------------------------------------------------------------------------
	  public virtual double presentValue(SwapPaymentEvent paymentEvent, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentEvent is NotionalExchange)
		{
		  return notionalExchangePricer.presentValue((NotionalExchange) paymentEvent, provider);
		}
		else if (paymentEvent is FxResetNotionalExchange)
		{
		  return fxResetNotionalExchangePricer.presentValue((FxResetNotionalExchange) paymentEvent, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentEvent type: " + paymentEvent.GetType().Name);
		}
	  }

	  public virtual PointSensitivityBuilder presentValueSensitivity(SwapPaymentEvent paymentEvent, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentEvent is NotionalExchange)
		{
		  return notionalExchangePricer.presentValueSensitivity((NotionalExchange) paymentEvent, provider);
		}
		else if (paymentEvent is FxResetNotionalExchange)
		{
		  return fxResetNotionalExchangePricer.presentValueSensitivity((FxResetNotionalExchange) paymentEvent, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentEvent type: " + paymentEvent.GetType().Name);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual double forecastValue(SwapPaymentEvent paymentEvent, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentEvent is NotionalExchange)
		{
		  return notionalExchangePricer.forecastValue((NotionalExchange) paymentEvent, provider);
		}
		else if (paymentEvent is FxResetNotionalExchange)
		{
		  return fxResetNotionalExchangePricer.forecastValue((FxResetNotionalExchange) paymentEvent, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentEvent type: " + paymentEvent.GetType().Name);
		}
	  }

	  public virtual PointSensitivityBuilder forecastValueSensitivity(SwapPaymentEvent paymentEvent, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentEvent is NotionalExchange)
		{
		  return notionalExchangePricer.forecastValueSensitivity((NotionalExchange) paymentEvent, provider);
		}
		else if (paymentEvent is FxResetNotionalExchange)
		{
		  return fxResetNotionalExchangePricer.forecastValueSensitivity((FxResetNotionalExchange) paymentEvent, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentEvent type: " + paymentEvent.GetType().Name);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void explainPresentValue(SwapPaymentEvent paymentEvent, RatesProvider provider, ExplainMapBuilder builder)
	  {
		// dispatch by runtime type
		if (paymentEvent is NotionalExchange)
		{
		  notionalExchangePricer.explainPresentValue((NotionalExchange) paymentEvent, provider, builder);
		}
		else if (paymentEvent is FxResetNotionalExchange)
		{
		  fxResetNotionalExchangePricer.explainPresentValue((FxResetNotionalExchange) paymentEvent, provider, builder);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentEvent type: " + paymentEvent.GetType().Name);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual MultiCurrencyAmount currencyExposure(SwapPaymentEvent paymentEvent, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentEvent is NotionalExchange)
		{
		  return notionalExchangePricer.currencyExposure((NotionalExchange) paymentEvent, provider);
		}
		else if (paymentEvent is FxResetNotionalExchange)
		{
		  return fxResetNotionalExchangePricer.currencyExposure((FxResetNotionalExchange) paymentEvent, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentEvent type: " + paymentEvent.GetType().Name);
		}
	  }

	  public virtual double currentCash(SwapPaymentEvent paymentEvent, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentEvent is NotionalExchange)
		{
		  return notionalExchangePricer.currentCash((NotionalExchange) paymentEvent, provider);
		}
		else if (paymentEvent is FxResetNotionalExchange)
		{
		  return fxResetNotionalExchangePricer.currentCash((FxResetNotionalExchange) paymentEvent, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentEvent type: " + paymentEvent.GetType().Name);
		}
	  }

	}

}