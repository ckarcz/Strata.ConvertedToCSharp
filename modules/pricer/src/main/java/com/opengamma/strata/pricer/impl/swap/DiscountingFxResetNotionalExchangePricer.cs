/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.swap
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using FxIndexRates = com.opengamma.strata.pricer.fx.FxIndexRates;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwapPaymentEventPricer = com.opengamma.strata.pricer.swap.SwapPaymentEventPricer;
	using FxResetNotionalExchange = com.opengamma.strata.product.swap.FxResetNotionalExchange;

	/// <summary>
	/// Pricer implementation for the exchange of FX reset notionals.
	/// <para>
	/// The FX reset notional exchange is priced by discounting the value of the exchange.
	/// The value of the exchange is calculated by performing an FX conversion on the amount.
	/// </para>
	/// </summary>
	public class DiscountingFxResetNotionalExchangePricer : SwapPaymentEventPricer<FxResetNotionalExchange>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFxResetNotionalExchangePricer DEFAULT = new DiscountingFxResetNotionalExchangePricer();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public DiscountingFxResetNotionalExchangePricer()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double presentValue(FxResetNotionalExchange @event, RatesProvider provider)
	  {
		// forecastValue * discountFactor
		double df = provider.discountFactor(@event.Currency, @event.PaymentDate);
		return forecastValue(@event, provider) * df;
	  }

	  public virtual PointSensitivityBuilder presentValueSensitivity(FxResetNotionalExchange @event, RatesProvider provider)
	  {
		DiscountFactors discountFactors = provider.discountFactors(@event.Currency);
		PointSensitivityBuilder sensiDsc = discountFactors.zeroRatePointSensitivity(@event.PaymentDate);
		sensiDsc = sensiDsc.multipliedBy(forecastValue(@event, provider));
		PointSensitivityBuilder sensiFx = forecastValueSensitivity(@event, provider);
		sensiFx = sensiFx.multipliedBy(discountFactors.discountFactor(@event.PaymentDate));
		return sensiDsc.combinedWith(sensiFx);
	  }

	  //-------------------------------------------------------------------------
	  public virtual double forecastValue(FxResetNotionalExchange @event, RatesProvider provider)
	  {
		// notional * fxRate
		return @event.Notional * fxRate(@event, provider);
	  }

	  // obtains the FX rate
	  private double fxRate(FxResetNotionalExchange @event, RatesProvider provider)
	  {
		FxIndexRates rates = provider.fxIndexRates(@event.Observation.Index);
		return rates.rate(@event.Observation, @event.ReferenceCurrency);
	  }

	  public virtual PointSensitivityBuilder forecastValueSensitivity(FxResetNotionalExchange @event, RatesProvider provider)
	  {
		FxIndexRates rates = provider.fxIndexRates(@event.Observation.Index);
		return rates.ratePointSensitivity(@event.Observation, @event.ReferenceCurrency).multipliedBy(@event.Notional);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void explainPresentValue(FxResetNotionalExchange @event, RatesProvider provider, ExplainMapBuilder builder)
	  {
		Currency currency = @event.Currency;
		LocalDate paymentDate = @event.PaymentDate;

		builder.put(ExplainKey.ENTRY_TYPE, "FxResetNotionalExchange");
		builder.put(ExplainKey.PAYMENT_DATE, paymentDate);
		builder.put(ExplainKey.PAYMENT_CURRENCY, currency);
		builder.put(ExplainKey.TRADE_NOTIONAL, @event.NotionalAmount);
		if (paymentDate.isBefore(provider.ValuationDate))
		{
		  builder.put(ExplainKey.COMPLETED, true);
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.zero(currency));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.zero(currency));
		}
		else
		{
		  builder.addListEntry(ExplainKey.OBSERVATIONS, child =>
		  {
		  child.put(ExplainKey.ENTRY_TYPE, "FxObservation");
		  child.put(ExplainKey.INDEX, @event.Observation.Index);
		  child.put(ExplainKey.FIXING_DATE, @event.Observation.FixingDate);
		  child.put(ExplainKey.INDEX_VALUE, fxRate(@event, provider));
		  });
		  builder.put(ExplainKey.DISCOUNT_FACTOR, provider.discountFactor(currency, paymentDate));
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.of(currency, forecastValue(@event, provider)));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.of(currency, presentValue(@event, provider)));
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual MultiCurrencyAmount currencyExposure(FxResetNotionalExchange @event, RatesProvider provider)
	  {
		LocalDate fixingDate = @event.Observation.FixingDate;
		FxIndexRates rates = provider.fxIndexRates(@event.Observation.Index);
		double df = provider.discountFactor(@event.Currency, @event.PaymentDate);
		if (!fixingDate.isAfter(provider.ValuationDate) && rates.Fixings.get(fixingDate).HasValue)
		{
		  double fxRate = rates.rate(@event.Observation, @event.ReferenceCurrency);
		  return MultiCurrencyAmount.of(CurrencyAmount.of(@event.Currency, @event.Notional * df * fxRate));
		}
		LocalDate maturityDate = @event.Observation.MaturityDate;
		double fxRateSpotSensitivity = rates.FxForwardRates.rateFxSpotSensitivity(@event.ReferenceCurrency, maturityDate);
		return MultiCurrencyAmount.of(CurrencyAmount.of(@event.ReferenceCurrency, @event.Notional * df * fxRateSpotSensitivity));
	  }

	  public virtual double currentCash(FxResetNotionalExchange @event, RatesProvider provider)
	  {
		if (provider.ValuationDate.isEqual(@event.PaymentDate))
		{
		  return forecastValue(@event, provider);
		}
		return 0d;
	  }

	}

}