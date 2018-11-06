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
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingleBarrierOption = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOption;
	using ResolvedFxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOptionTrade;

	/// <summary>
	/// Pricer for FX barrier option trades in Black-Scholes world.
	/// <para>
	/// This function provides the ability to price an <seealso cref="ResolvedFxSingleBarrierOptionTrade"/>.
	/// </para>
	/// </summary>
	public class BlackFxSingleBarrierOptionTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BlackFxSingleBarrierOptionTradePricer DEFAULT = new BlackFxSingleBarrierOptionTradePricer(BlackFxSingleBarrierOptionProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxSingleBarrierOption"/>.
	  /// </summary>
	  private readonly BlackFxSingleBarrierOptionProductPricer productPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedFxSingleBarrierOption"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public BlackFxSingleBarrierOptionTradePricer(BlackFxSingleBarrierOptionProductPricer productPricer, DiscountingPaymentPricer paymentPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the FX barrier option trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value of the trade </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ResolvedFxSingleBarrierOption product = trade.Product;
		CurrencyAmount pvProduct = productPricer.presentValue(product, ratesProvider, volatilities);
		Payment premium = trade.Premium;
		CurrencyAmount pvPremium = paymentPricer.presentValue(premium, ratesProvider);
		return MultiCurrencyAmount.of(pvProduct, pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the FX barrier option trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The volatility is fixed in this sensitivity computation, i.e., sticky-strike.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public virtual PointSensitivities presentValueSensitivityRatesStickyStrike(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ResolvedFxSingleBarrierOption product = trade.Product;
		PointSensitivityBuilder pvcsProduct = productPricer.presentValueSensitivityRatesStickyStrike(product, ratesProvider, volatilities);
		Payment premium = trade.Premium;
		PointSensitivityBuilder pvcsPremium = paymentPricer.presentValueSensitivity(premium, ratesProvider);
		return pvcsProduct.combinedWith(pvcsPremium).build();
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
	  public virtual PointSensitivities presentValueSensitivityModelParamsVolatility(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ResolvedFxSingleBarrierOption product = trade.Product;
		return productPricer.presentValueSensitivityModelParamsVolatility(product, ratesProvider, volatilities).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the FX barrier option trade.
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		Payment premium = trade.Premium;
		CurrencyAmount pvPremium = paymentPricer.presentValue(premium, ratesProvider);
		ResolvedFxSingleBarrierOption product = trade.Product;
		return productPricer.currencyExposure(product, ratesProvider, volatilities).plus(pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the current of the FX barrier option trade.
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the current cash amount </returns>
	  public virtual CurrencyAmount currentCash(ResolvedFxSingleBarrierOptionTrade trade, LocalDate valuationDate)
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