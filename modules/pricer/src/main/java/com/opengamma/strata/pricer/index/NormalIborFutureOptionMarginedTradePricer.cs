/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborFutureOption = com.opengamma.strata.product.index.IborFutureOption;
	using ResolvedIborFuture = com.opengamma.strata.product.index.ResolvedIborFuture;
	using ResolvedIborFutureOption = com.opengamma.strata.product.index.ResolvedIborFutureOption;
	using ResolvedIborFutureOptionTrade = com.opengamma.strata.product.index.ResolvedIborFutureOptionTrade;
	using FutureOptionPremiumStyle = com.opengamma.strata.product.option.FutureOptionPremiumStyle;

	/// <summary>
	/// Pricer implementation for Ibor future option.
	/// <para>
	/// This provides the ability to price an Ibor future option.
	/// The option must be based on <seealso cref="FutureOptionPremiumStyle#DAILY_MARGIN daily margin"/>.
	/// 
	/// <h4>Price</h4>
	/// The price of an Ibor future option is based on the price of the underlying future, the volatility
	/// and the time to expiry. The price of the at-the-money option tends to zero as expiry approaches.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Ibor future options in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, an option price of 0.2 is related to a futures price of 99.32 that implies an
	/// interest rate of 0.68%. Strata represents the price of the future as 0.9932 and thus
	/// represents the price of the option as 0.002.
	/// </para>
	/// </summary>
	public sealed class NormalIborFutureOptionMarginedTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly NormalIborFutureOptionMarginedTradePricer DEFAULT = new NormalIborFutureOptionMarginedTradePricer(NormalIborFutureOptionMarginedProductPricer.DEFAULT);

	  /// <summary>
	  /// Underlying option pricer.
	  /// </summary>
	  private readonly NormalIborFutureOptionMarginedProductPricer futureOptionPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="futureOptionPricer">  the pricer for <seealso cref="IborFutureOption"/> </param>
	  public NormalIborFutureOptionMarginedTradePricer(NormalIborFutureOptionMarginedProductPricer futureOptionPricer)
	  {
		this.futureOptionPricer = ArgChecker.notNull(futureOptionPricer, "futureOptionPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the Ibor future option trade.
	  /// <para>
	  /// The price of the trade is the price on the valuation date.
	  /// The price is calculated using the volatility model.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public double price(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities)
	  {

		return futureOptionPricer.price(trade.Product, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor future option trade from the current option price.
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
	  /// <param name="currentOptionPrice">  the current price for the option, in decimal form </param>
	  /// <param name="lastOptionSettlementPrice">  the last settlement price used for margining for the option, in decimal form </param>
	  /// <returns> the present value </returns>
	  public CurrencyAmount presentValue(ResolvedIborFutureOptionTrade trade, LocalDate valuationDate, double currentOptionPrice, double lastOptionSettlementPrice)
	  {

		ResolvedIborFutureOption option = trade.Product;
		double referencePrice = this.referencePrice(trade, valuationDate, lastOptionSettlementPrice);
		double priceIndex = futureOptionPricer.marginIndex(option, currentOptionPrice);
		double referenceIndex = futureOptionPricer.marginIndex(option, referencePrice);
		double pv = (priceIndex - referenceIndex) * trade.Quantity;
		return CurrencyAmount.of(option.UnderlyingFuture.Currency, pv);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor future option trade.
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
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="lastOptionSettlementPrice">  the last settlement price used for margining for the option, in decimal form </param>
	  /// <returns> the present value </returns>
	  public CurrencyAmount presentValue(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities, double lastOptionSettlementPrice)
	  {

		double price = this.price(trade, ratesProvider, volatilities);
		return presentValue(trade, ratesProvider.ValuationDate, price, lastOptionSettlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor future option trade from the underlying future price.
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
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future, in decimal form </param>
	  /// <param name="lastOptionSettlementPrice">  the last settlement price used for margining for the option, in decimal form </param>
	  /// <returns> the present value </returns>
	  public CurrencyAmount presentValue(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities, double futurePrice, double lastOptionSettlementPrice)
	  {

		double optionPrice = futureOptionPricer.price(trade.Product, ratesProvider, volatilities, futurePrice);
		return presentValue(trade, ratesProvider.ValuationDate, optionPrice, lastOptionSettlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the Ibor future option trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public PointSensitivities presentValueSensitivityRates(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities)
	  {

		ResolvedIborFutureOption product = trade.Product;
		PointSensitivities priceSensi = futureOptionPricer.priceSensitivityRatesStickyStrike(product, ratesProvider, volatilities);
		PointSensitivities marginIndexSensi = futureOptionPricer.marginIndexSensitivity(product, priceSensi);
		return marginIndexSensi.multipliedBy(trade.Quantity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value sensitivity to the normal volatility used in the pricing.
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
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price sensitivity </returns>
	  public IborFutureOptionSensitivity presentValueSensitivityModelParamsVolatility(ResolvedIborFutureOptionTrade futureOptionTrade, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities)
	  {

		ResolvedIborFuture future = futureOptionTrade.Product.UnderlyingFuture;
		double futurePrice = futureOptionPricer.FuturePricer.price(future, ratesProvider);
		return presentValueSensitivityModelParamsVolatility(futureOptionTrade, ratesProvider, volatilities, futurePrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value sensitivity to the normal volatility used in the pricing
	  /// based on the price of the underlying future.
	  /// <para>
	  /// The result is a single sensitivity to the volatility used.
	  /// The volatility is associated with the expiry/delay/strike/future price key combination.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOptionTrade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future, in decimal form </param>
	  /// <returns> the price sensitivity </returns>
	  public IborFutureOptionSensitivity presentValueSensitivityModelParamsVolatility(ResolvedIborFutureOptionTrade futureOptionTrade, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities, double futurePrice)
	  {

		ResolvedIborFutureOption product = futureOptionTrade.Product;
		IborFutureOptionSensitivity priceSensitivity = futureOptionPricer.priceSensitivityModelParamsVolatility(product, ratesProvider, volatilities, futurePrice);
		double factor = futureOptionPricer.marginIndex(product, 1) * futureOptionTrade.Quantity;
		return priceSensitivity.multipliedBy(factor);
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
	  private double referencePrice(ResolvedIborFutureOptionTrade trade, LocalDate valuationDate, double lastSettlementPrice)
	  {
		ArgChecker.notNull(valuationDate, "valuationDate");
		return trade.TradedPrice.filter(tp => tp.TradeDate.Equals(valuationDate)).map(tp => tp.Price).orElse(lastSettlementPrice);
	  }

	}

}