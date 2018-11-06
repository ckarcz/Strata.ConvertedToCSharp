/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using BondFuture = com.opengamma.strata.product.bond.BondFuture;
	using ResolvedBondFuture = com.opengamma.strata.product.bond.ResolvedBondFuture;
	using ResolvedBondFutureOption = com.opengamma.strata.product.bond.ResolvedBondFutureOption;
	using ResolvedBondFutureOptionTrade = com.opengamma.strata.product.bond.ResolvedBondFutureOptionTrade;

	/// <summary>
	/// Pricer implementation for bond future option.
	/// <para>
	/// The bond future option is priced based on Black model.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bond futures options in the trade model, pricers and market data.
	/// This is coherent with the pricing of <seealso cref="BondFuture"/>.
	/// </para>
	/// </summary>
	public sealed class BlackBondFutureOptionMarginedTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BlackBondFutureOptionMarginedTradePricer DEFAULT = new BlackBondFutureOptionMarginedTradePricer(BlackBondFutureOptionMarginedProductPricer.DEFAULT);

	  /// <summary>
	  /// Underlying option pricer.
	  /// </summary>
	  private readonly BlackBondFutureOptionMarginedProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedBondFutureOption"/> </param>
	  public BlackBondFutureOptionMarginedTradePricer(BlackBondFutureOptionMarginedProductPricer productPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the bond future option trade.
	  /// <para>
	  /// The price of the trade is the price on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public double price(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		return productPricer.price(trade.Product, discountingProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the bond future option trade from the current option price.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// The current price is specified, not calculated.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the specified current price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="valuationDate">  the valuation date; required to asses if the trade or last closing price should be used </param>
	  /// <param name="currentOptionPrice">  the option price on the valuation date </param>
	  /// <param name="lastOptionSettlementPrice">  the last settlement price used for margining for the option, in decimal form </param>
	  /// <returns> the present value </returns>
	  public CurrencyAmount presentValue(ResolvedBondFutureOptionTrade trade, LocalDate valuationDate, double currentOptionPrice, double lastOptionSettlementPrice)
	  {

		ResolvedBondFutureOption option = trade.Product;
		double referencePrice = this.referencePrice(trade, valuationDate, lastOptionSettlementPrice);
		double priceIndex = productPricer.marginIndex(option, currentOptionPrice);
		double referenceIndex = productPricer.marginIndex(option, referencePrice);
		double pv = (priceIndex - referenceIndex) * trade.Quantity;
		return CurrencyAmount.of(option.UnderlyingFuture.Currency, pv);
	  }

	  /// <summary>
	  /// Calculates the present value of the bond future option trade.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// The current price is calculated using the volatility model.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="lastOptionSettlementPrice">  the last settlement price used for margining for the option, in decimal form </param>
	  /// <returns> the present value </returns>
	  public CurrencyAmount presentValue(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities, double lastOptionSettlementPrice)
	  {

		double price = this.price(trade, discountingProvider, volatilities);
		return presentValue(trade, discountingProvider.ValuationDate, price, lastOptionSettlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the bond future option trade from the underlying future price.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// The current price is calculated using the volatility model with a known future price.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <param name="lastOptionSettlementPrice">  the last settlement price used for margining for the option, in decimal form </param>
	  /// <returns> the present value </returns>
	  public CurrencyAmount presentValue(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities, double futurePrice, double lastOptionSettlementPrice)
	  {

		double optionPrice = productPricer.price(trade.Product, discountingProvider, volatilities, futurePrice);
		return presentValue(trade, discountingProvider.ValuationDate, optionPrice, lastOptionSettlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the bond future option trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public PointSensitivities presentValueSensitivityRates(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		ResolvedBondFutureOption product = trade.Product;
		PointSensitivities priceSensi = productPricer.priceSensitivity(product, discountingProvider, volatilities);
		PointSensitivities marginIndexSensi = productPricer.marginIndexSensitivity(product, priceSensi);
		return marginIndexSensi.multipliedBy(trade.Quantity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value sensitivity to the Black volatility used in the pricing.
	  /// <para>
	  /// The result is a single sensitivity to the volatility used.
	  /// The volatility is associated with the expiry/delay/strike/future price key combination.
	  /// </para>
	  /// <para>
	  /// This calculates the underlying future price using the future pricer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOptionTrade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price sensitivity </returns>
	  public BondFutureOptionSensitivity presentValueSensitivityModelParamsVolatility(ResolvedBondFutureOptionTrade futureOptionTrade, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities)
	  {

		ResolvedBondFuture future = futureOptionTrade.Product.UnderlyingFuture;
		double futurePrice = productPricer.FuturePricer.price(future, discountingProvider);
		return presentValueSensitivityModelParamsVolatility(futureOptionTrade, discountingProvider, volatilities, futurePrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value sensitivity to the Black volatility used in the pricing
	  /// based on the price of the underlying future.
	  /// <para>
	  /// The result is a single sensitivity to the volatility used.
	  /// The volatility is associated with the expiry/delay/strike/future price key combination.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOptionTrade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <returns> the price sensitivity </returns>
	  public BondFutureOptionSensitivity presentValueSensitivityModelParamsVolatility(ResolvedBondFutureOptionTrade futureOptionTrade, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities, double futurePrice)
	  {

		ResolvedBondFutureOption product = futureOptionTrade.Product;
		BondFutureOptionSensitivity priceSensitivity = productPricer.priceSensitivityModelParamsVolatility(product, discountingProvider, volatilities, futurePrice);
		double factor = productPricer.marginIndex(product, 1) * futureOptionTrade.Quantity;
		return priceSensitivity.multipliedBy(factor);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the bond future option trade.
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="lastOptionSettlementPrice">  the last settlement price used for margining for the option, in decimal form </param>
	  /// <returns> the currency exposure of the bond future option trade </returns>
	  public MultiCurrencyAmount currencyExposure(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities, double lastOptionSettlementPrice)
	  {

		double price = this.price(trade, discountingProvider, volatilities);
		return currencyExposure(trade, discountingProvider.ValuationDate, price, lastOptionSettlementPrice);
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the bond future option trade from the current option price.
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="valuationDate">  the valuation date; required to asses if the trade or last closing price should be used </param>
	  /// <param name="currentOptionPrice">  the option price on the valuation date </param>
	  /// <param name="lastOptionSettlementPrice">  the last settlement price used for margining for the option, in decimal form </param>
	  /// <returns> the currency exposure of the bond future option trade </returns>
	  public MultiCurrencyAmount currencyExposure(ResolvedBondFutureOptionTrade trade, LocalDate valuationDate, double currentOptionPrice, double lastOptionSettlementPrice)
	  {

		return MultiCurrencyAmount.of(presentValue(trade, valuationDate, currentOptionPrice, lastOptionSettlementPrice));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the reference price for the trade.
	  /// <para>
	  /// If the valuation date equals the trade date, then the reference price is the trade price.
	  /// Otherwise, the reference price is the last settlement price used for margining.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="valuationDate">  the date for which the reference price should be calculated </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <returns> the reference price, in decimal form </returns>
	  private double referencePrice(ResolvedBondFutureOptionTrade trade, LocalDate valuationDate, double lastSettlementPrice)
	  {
		ArgChecker.notNull(valuationDate, "valuationDate");
		return trade.TradedPrice.filter(tp => tp.TradeDate.Equals(valuationDate)).map(tp => tp.Price).orElse(lastSettlementPrice);
	  }

	}

}