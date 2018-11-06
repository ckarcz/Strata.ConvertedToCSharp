/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using ResolvedSwaptionTrade = com.opengamma.strata.product.swaption.ResolvedSwaptionTrade;

	/// <summary>
	/// Pricer for swaptions handling physical and cash par yield settlement based on volatilities.
	/// <para>
	/// The swap underlying the swaption must have a fixed leg on which the forward rate is computed.
	/// The underlying swap must be single currency.
	/// </para>
	/// <para>
	/// The volatility parameters are not adjusted for the underlying swap convention.
	/// </para>
	/// <para>
	/// The present value and sensitivities of the premium, if in the future, are also taken into account.
	/// </para>
	/// </summary>
	public class VolatilitySwaptionTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly VolatilitySwaptionTradePricer DEFAULT = new VolatilitySwaptionTradePricer(VolatilitySwaptionProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedSwaption"/>.
	  /// </summary>
	  private readonly VolatilitySwaptionProductPricer productPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedSwaption"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public VolatilitySwaptionTradePricer(VolatilitySwaptionProductPricer productPricer, DiscountingPaymentPricer paymentPricer)
	  {

		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the swaption trade.
	  /// <para>
	  /// The result is expressed using the currency of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the swaption trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		// product
		ResolvedSwaption product = trade.Product;
		CurrencyAmount pvProduct = productPricer.presentValue(product, ratesProvider, swaptionVolatilities);
		// premium
		Payment premium = trade.Premium;
		CurrencyAmount pvPremium = paymentPricer.presentValue(premium, ratesProvider);
		// total
		return pvProduct.plus(pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the swaption to the rate curves.
	  /// <para>
	  /// The present value sensitivity is computed in a "sticky strike" style, i.e. the sensitivity to the 
	  /// curve nodes with the volatility at the swaption strike unchanged. This sensitivity does not include a potential 
	  /// change of volatility due to the implicit change of forward rate or moneyness.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the swaption trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the rate curves </returns>
	  public virtual PointSensitivities presentValueSensitivityRatesStickyStrike(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		// product
		ResolvedSwaption product = trade.Product;
		PointSensitivityBuilder pvcsProduct = productPricer.presentValueSensitivityRatesStickyStrike(product, ratesProvider, swaptionVolatilities);
		// premium
		Payment premium = trade.Premium;
		PointSensitivityBuilder pvcsPremium = paymentPricer.presentValueSensitivity(premium, ratesProvider);
		// total
		return pvcsProduct.combinedWith(pvcsPremium).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity to the implied volatility of the swaption trade.
	  /// <para>
	  /// The sensitivity to the implied volatility is also called vega.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the swaption trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the implied volatility </returns>
	  public virtual PointSensitivities presentValueSensitivityModelParamsVolatility(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		ResolvedSwaption product = trade.Product;
		SwaptionSensitivity pointSens = productPricer.presentValueSensitivityModelParamsVolatility(product, ratesProvider, swaptionVolatilities);
		return PointSensitivities.of(pointSens);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the currency exposure of the swaption trade.
	  /// </summary>
	  /// <param name="trade">  the swaption trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		return MultiCurrencyAmount.of(presentValue(trade, ratesProvider, swaptionVolatilities));
	  }

	  /// <summary>
	  /// Calculates the current cash of the swaption trade.
	  /// <para>
	  /// Only the premium is contributing to the current cash for non-cash settle swaptions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the swaption trade </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the current cash amount </returns>
	  public virtual CurrencyAmount currentCash(ResolvedSwaptionTrade trade, LocalDate valuationDate)
	  {
		Payment premium = trade.Premium;
		if (premium.Date.Equals(valuationDate))
		{
		  return CurrencyAmount.of(premium.Currency, premium.Amount);
		}
		return CurrencyAmount.of(premium.Currency, 0d);
	  }

	}

}