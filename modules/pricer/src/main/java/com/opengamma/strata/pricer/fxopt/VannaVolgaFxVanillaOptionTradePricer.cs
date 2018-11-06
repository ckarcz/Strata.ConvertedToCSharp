/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingleBarrierOption = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOption;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;
	using ResolvedFxVanillaOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOptionTrade;

	/// <summary>
	/// Pricer for FX vanilla option trades with a Vanna-Volga method.
	/// <para>
	/// The volatilities are expressed using {@code BlackFxOptionSmileVolatilities}. 
	/// Each smile of the term structure consists of 3 data points, where the middle point corresponds to ATM volatility.
	/// </para>
	/// </summary>
	public class VannaVolgaFxVanillaOptionTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly VannaVolgaFxVanillaOptionTradePricer DEFAULT = new VannaVolgaFxVanillaOptionTradePricer(VannaVolgaFxVanillaOptionProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxSingleBarrierOption"/>.
	  /// </summary>
	  private readonly VannaVolgaFxVanillaOptionProductPricer productPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedFxVanillaOption"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public VannaVolgaFxVanillaOptionTradePricer(VannaVolgaFxVanillaOptionProductPricer productPricer, DiscountingPaymentPricer paymentPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the FX vanilla option trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value of the trade </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionSmileVolatilities volatilities)
	  {

		ResolvedFxVanillaOption product = trade.Product;
		CurrencyAmount pvProduct = productPricer.presentValue(product, ratesProvider, volatilities);
		Payment premium = trade.Premium;
		CurrencyAmount pvPremium = paymentPricer.presentValue(premium, ratesProvider);
		return MultiCurrencyAmount.of(pvProduct, pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the FX vanilla option trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The volatility is fixed in this sensitivity computation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public virtual PointSensitivities presentValueSensitivityRatesStickyStrike(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionSmileVolatilities volatilities)
	  {

		ResolvedFxVanillaOption product = trade.Product;
		PointSensitivities pvcsProduct = productPricer.presentValueSensitivityRatesStickyStrike(product, ratesProvider, volatilities).build();
		Payment premium = trade.Premium;
		PointSensitivities pvcsPremium = paymentPricer.presentValueSensitivity(premium, ratesProvider).build();
		return pvcsProduct.combinedWith(pvcsPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value sensitivity to the black volatility used in the pricing.
	  /// <para>
	  /// The result is a single sensitivity to the volatility used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueSensitivityModelParamsVolatility(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionSmileVolatilities volatilities)
	  {

		ResolvedFxVanillaOption product = trade.Product;
		return productPricer.presentValueSensitivityModelParamsVolatility(product, ratesProvider, volatilities).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the FX vanilla option trade.
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionSmileVolatilities volatilities)
	  {

		Payment premium = trade.Premium;
		CurrencyAmount pvPremium = paymentPricer.presentValue(premium, ratesProvider);
		ResolvedFxVanillaOption product = trade.Product;
		return productPricer.currencyExposure(product, ratesProvider, volatilities).plus(pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the current of the FX vanilla option trade.
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the current cash amount </returns>
	  public virtual CurrencyAmount currentCash(ResolvedFxVanillaOptionTrade trade, LocalDate valuationDate)
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