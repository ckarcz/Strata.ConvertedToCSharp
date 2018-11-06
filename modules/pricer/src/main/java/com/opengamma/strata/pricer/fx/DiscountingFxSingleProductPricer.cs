/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;

	/// <summary>
	/// Pricer for foreign exchange transaction products.
	/// <para>
	/// This provides the ability to price an <seealso cref="ResolvedFxSingle"/>.
	/// </para>
	/// </summary>
	public class DiscountingFxSingleProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFxSingleProductPricer DEFAULT = new DiscountingFxSingleProductPricer(DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public DiscountingFxSingleProductPricer(DiscountingPaymentPricer paymentPricer)
	  {
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the FX product by discounting each payment in its own currency.
	  /// </summary>
	  /// <param name="fx">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value in the two natural currencies </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedFxSingle fx, RatesProvider provider)
	  {
		if (provider.ValuationDate.isAfter(fx.PaymentDate))
		{
		  return MultiCurrencyAmount.empty();
		}
		CurrencyAmount pv1 = paymentPricer.presentValue(fx.BaseCurrencyPayment, provider);
		CurrencyAmount pv2 = paymentPricer.presentValue(fx.CounterCurrencyPayment, provider);
		return MultiCurrencyAmount.of(pv1, pv2);
	  }

	  /// <summary>
	  /// Calculates the present value curve sensitivity of the FX product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fx">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedFxSingle fx, RatesProvider provider)
	  {
		if (provider.ValuationDate.isAfter(fx.PaymentDate))
		{
		  return PointSensitivities.empty();
		}
		PointSensitivityBuilder pvcs1 = paymentPricer.presentValueSensitivity(fx.BaseCurrencyPayment, provider);
		PointSensitivityBuilder pvcs2 = paymentPricer.presentValueSensitivity(fx.CounterCurrencyPayment, provider);
		return pvcs1.combinedWith(pvcs2).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par spread.
	  /// <para>
	  /// This is the spread that should be added to the FX points to have a zero value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fx">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the spread </returns>
	  public virtual double parSpread(ResolvedFxSingle fx, RatesProvider provider)
	  {
		Payment basePayment = fx.BaseCurrencyPayment;
		Payment counterPayment = fx.CounterCurrencyPayment;
		MultiCurrencyAmount pv = presentValue(fx, provider);
		double pvCounterCcy = pv.convertedTo(counterPayment.Currency, provider).Amount;
		double dfEnd = provider.discountFactor(counterPayment.Currency, fx.PaymentDate);
		double notionalBaseCcy = basePayment.Amount;
		return pvCounterCcy / (notionalBaseCcy * dfEnd);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure by discounting each payment in its own currency.
	  /// </summary>
	  /// <param name="product">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxSingle product, RatesProvider provider)
	  {
		return presentValue(product, provider);
	  }

	  /// <summary>
	  /// Calculates the current cash.
	  /// </summary>
	  /// <param name="fx">  the product </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the current cash </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedFxSingle fx, LocalDate valuationDate)
	  {
		if (valuationDate.isEqual(fx.PaymentDate))
		{
		  return MultiCurrencyAmount.of(fx.BaseCurrencyPayment.Value, fx.CounterCurrencyPayment.Value);
		}
		return MultiCurrencyAmount.empty();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forward exchange rate.
	  /// </summary>
	  /// <param name="fx">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forward rate </returns>
	  public virtual FxRate forwardFxRate(ResolvedFxSingle fx, RatesProvider provider)
	  {
		FxForwardRates fxForwardRates = provider.fxForwardRates(fx.CurrencyPair);
		Payment basePayment = fx.BaseCurrencyPayment;
		Payment counterPayment = fx.CounterCurrencyPayment;
		double forwardRate = fxForwardRates.rate(basePayment.Currency, fx.PaymentDate);
		return FxRate.of(basePayment.Currency, counterPayment.Currency, forwardRate);
	  }

	  /// <summary>
	  /// Calculates the forward exchange rate point sensitivity.
	  /// <para>
	  /// The returned value is based on the direction of the FX product.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fx">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity </returns>
	  public virtual PointSensitivityBuilder forwardFxRatePointSensitivity(ResolvedFxSingle fx, RatesProvider provider)
	  {
		FxForwardRates fxForwardRates = provider.fxForwardRates(fx.CurrencyPair);
		PointSensitivityBuilder forwardFxRatePointSensitivity = fxForwardRates.ratePointSensitivity(fx.ReceiveCurrencyAmount.Currency, fx.PaymentDate);
		return forwardFxRatePointSensitivity;
	  }

	  /// <summary>
	  /// Calculates the sensitivity of the forward exchange rate to the spot rate.
	  /// <para>
	  /// The returned value is based on the direction of the FX product.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fx">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the sensitivity to spot </returns>
	  public virtual double forwardFxRateSpotSensitivity(ResolvedFxSingle fx, RatesProvider provider)
	  {
		FxForwardRates fxForwardRates = provider.fxForwardRates(fx.CurrencyPair);
		double forwardRateSpotSensitivity = fxForwardRates.rateFxSpotSensitivity(fx.ReceiveCurrencyAmount.Currency, fx.PaymentDate);
		return forwardRateSpotSensitivity;
	  }

	}

}