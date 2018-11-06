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
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwapPaymentPeriodPricer = com.opengamma.strata.pricer.swap.SwapPaymentPeriodPricer;
	using KnownAmountSwapPaymentPeriod = com.opengamma.strata.product.swap.KnownAmountSwapPaymentPeriod;

	/// <summary>
	/// Pricer implementation for swap payment periods based on a known amount.
	/// <para>
	/// This pricer performs discounting of the known amount.
	/// </para>
	/// </summary>
	public class DiscountingKnownAmountPaymentPeriodPricer : SwapPaymentPeriodPricer<KnownAmountSwapPaymentPeriod>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingKnownAmountPaymentPeriodPricer DEFAULT = new DiscountingKnownAmountPaymentPeriodPricer(DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Payment pricer.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="paymentPricer">  the payment pricer </param>
	  public DiscountingKnownAmountPaymentPeriodPricer(DiscountingPaymentPricer paymentPricer)
	  {
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  //-------------------------------------------------------------------------
	  public virtual double presentValue(KnownAmountSwapPaymentPeriod period, RatesProvider provider)
	  {
		return paymentPricer.presentValueAmount(period.Payment, provider);
	  }

	  public virtual double forecastValue(KnownAmountSwapPaymentPeriod period, RatesProvider provider)
	  {
		return paymentPricer.forecastValueAmount(period.Payment, provider);
	  }

	  public virtual double accruedInterest(KnownAmountSwapPaymentPeriod period, RatesProvider provider)
	  {
		// no day count available, so return the simple day-based fraction
		LocalDate valDate = provider.ValuationDate;
		if (valDate.compareTo(period.StartDate) <= 0 || valDate.compareTo(period.EndDate) > 0)
		{
		  return 0d;
		}
		double fv = forecastValue(period, provider);
		double totalDays = period.StartDate.until(period.EndDate, DAYS);
		double partialDays = period.StartDate.until(valDate, DAYS);
		return fv * (partialDays / totalDays);
	  }

	  public virtual double pvbp(KnownAmountSwapPaymentPeriod period, RatesProvider provider)
	  {
		throw new System.NotSupportedException("Unable to calculate PVBP for KnownAmountPaymentPeriod");
	  }

	  //-------------------------------------------------------------------------
	  public virtual PointSensitivityBuilder presentValueSensitivity(KnownAmountSwapPaymentPeriod period, RatesProvider provider)
	  {
		return paymentPricer.presentValueSensitivity(period.Payment, provider);
	  }

	  public virtual PointSensitivityBuilder forecastValueSensitivity(KnownAmountSwapPaymentPeriod period, RatesProvider provider)
	  {
		return PointSensitivityBuilder.none();
	  }

	  public virtual PointSensitivityBuilder pvbpSensitivity(KnownAmountSwapPaymentPeriod period, RatesProvider provider)
	  {
		throw new System.NotSupportedException("Unable to calculate PVBP for KnownAmountPaymentPeriod");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void explainPresentValue(KnownAmountSwapPaymentPeriod period, RatesProvider provider, ExplainMapBuilder builder)
	  {
		Currency currency = period.Currency;
		LocalDate paymentDate = period.PaymentDate;

		builder.put(ExplainKey.ENTRY_TYPE, "KnownAmountPaymentPeriod");
		builder.put(ExplainKey.PAYMENT_DATE, paymentDate);
		builder.put(ExplainKey.PAYMENT_CURRENCY, currency);
		builder.put(ExplainKey.START_DATE, period.StartDate);
		builder.put(ExplainKey.UNADJUSTED_START_DATE, period.UnadjustedStartDate);
		builder.put(ExplainKey.END_DATE, period.EndDate);
		builder.put(ExplainKey.UNADJUSTED_END_DATE, period.UnadjustedEndDate);
		builder.put(ExplainKey.DAYS, (int) DAYS.between(period.StartDate, period.EndDate));
		if (paymentDate.isBefore(provider.ValuationDate))
		{
		  builder.put(ExplainKey.COMPLETED, true);
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.zero(currency));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.zero(currency));
		}
		else
		{
		  builder.put(ExplainKey.DISCOUNT_FACTOR, provider.discountFactor(currency, paymentDate));
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.of(currency, forecastValue(period, provider)));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.of(currency, presentValue(period, provider)));
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual MultiCurrencyAmount currencyExposure(KnownAmountSwapPaymentPeriod period, RatesProvider provider)
	  {
		return MultiCurrencyAmount.of(CurrencyAmount.of(period.Currency, presentValue(period, provider)));
	  }

	  public virtual double currentCash(KnownAmountSwapPaymentPeriod period, RatesProvider provider)
	  {
		if (provider.ValuationDate.isEqual(period.PaymentDate))
		{
		  return forecastValue(period, provider);
		}
		return 0d;
	  }
	}

}