/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.swap
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwapPaymentEventPricer = com.opengamma.strata.pricer.swap.SwapPaymentEventPricer;
	using NotionalExchange = com.opengamma.strata.product.swap.NotionalExchange;

	/// <summary>
	/// Pricer implementation for the exchange of notionals.
	/// <para>
	/// The notional exchange is priced by discounting the value of the exchange.
	/// </para>
	/// </summary>
	public class DiscountingNotionalExchangePricer : SwapPaymentEventPricer<NotionalExchange>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingNotionalExchangePricer DEFAULT = new DiscountingNotionalExchangePricer(DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public DiscountingNotionalExchangePricer(DiscountingPaymentPricer paymentPricer)
	  {
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  //-------------------------------------------------------------------------
	  public virtual double presentValue(NotionalExchange @event, RatesProvider provider)
	  {
		return paymentPricer.presentValueAmount(@event.Payment, provider);
	  }

	  public virtual PointSensitivityBuilder presentValueSensitivity(NotionalExchange @event, RatesProvider provider)
	  {
		return paymentPricer.presentValueSensitivity(@event.Payment, provider);
	  }

	  //-------------------------------------------------------------------------
	  public virtual double forecastValue(NotionalExchange @event, RatesProvider provider)
	  {
		return paymentPricer.forecastValueAmount(@event.Payment, provider);
	  }

	  public virtual PointSensitivityBuilder forecastValueSensitivity(NotionalExchange @event, RatesProvider provider)
	  {
		return PointSensitivityBuilder.none();
	  }

	  //-------------------------------------------------------------------------
	  public virtual void explainPresentValue(NotionalExchange @event, RatesProvider provider, ExplainMapBuilder builder)
	  {
		Currency currency = @event.Currency;
		LocalDate paymentDate = @event.PaymentDate;

		builder.put(ExplainKey.ENTRY_TYPE, "NotionalExchange");
		builder.put(ExplainKey.PAYMENT_DATE, paymentDate);
		builder.put(ExplainKey.PAYMENT_CURRENCY, currency);
		builder.put(ExplainKey.TRADE_NOTIONAL, @event.PaymentAmount);
		if (paymentDate.isBefore(provider.ValuationDate))
		{
		  builder.put(ExplainKey.COMPLETED, true);
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.zero(currency));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.zero(currency));
		}
		else
		{
		  builder.put(ExplainKey.DISCOUNT_FACTOR, provider.discountFactor(currency, paymentDate));
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.of(currency, forecastValue(@event, provider)));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.of(currency, presentValue(@event, provider)));
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual MultiCurrencyAmount currencyExposure(NotionalExchange @event, RatesProvider provider)
	  {
		return paymentPricer.currencyExposure(@event.Payment, provider);
	  }

	  public virtual double currentCash(NotionalExchange @event, RatesProvider provider)
	  {
		return paymentPricer.currentCash(@event.Payment, provider).Amount;
	  }

	}

}