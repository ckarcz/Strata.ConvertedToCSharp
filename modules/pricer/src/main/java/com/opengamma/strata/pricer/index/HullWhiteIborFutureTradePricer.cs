/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using HullWhiteOneFactorPiecewiseConstantParametersProvider = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParametersProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborFuture = com.opengamma.strata.product.index.IborFuture;
	using IborFutureTrade = com.opengamma.strata.product.index.IborFutureTrade;
	using ResolvedIborFuture = com.opengamma.strata.product.index.ResolvedIborFuture;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;

	/// <summary>
	/// Pricer for for Ibor future trades.
	/// <para>
	/// This function provides the ability to price a <seealso cref="IborFutureTrade"/> based on
	/// Hull-White one-factor model with piecewise constant volatility.
	/// </para>
	/// <para> 
	/// Reference: Henrard M., Eurodollar Futures and Options: Convexity Adjustment in HJM One-Factor Model. March 2005.
	/// Available at <a href="http://ssrn.com/abstract=682343">http://ssrn.com/abstract=682343</a>
	/// 
	/// <h4>Price</h4>
	/// The price of an Ibor future is based on the interest rate of the underlying index.
	/// It is defined as {@code (100 - percentRate)}.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
	public class HullWhiteIborFutureTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly HullWhiteIborFutureTradePricer DEFAULT = new HullWhiteIborFutureTradePricer(HullWhiteIborFutureProductPricer.DEFAULT);

	  /// <summary>
	  /// Underlying pricer.
	  /// </summary>
	  private readonly HullWhiteIborFutureProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="IborFuture"/> </param>
	  public HullWhiteIborFutureTradePricer(HullWhiteIborFutureProductPricer productPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the Ibor future trade.
	  /// <para>
	  /// The price of the trade is the price on the valuation date.
	  /// The price is calculated using the Hull-White model.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the price of the trade, in decimal form </returns>
	  public virtual double price(ResolvedIborFutureTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		return productPricer.price(trade.Product, ratesProvider, hwProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price sensitivity of the Ibor future product.
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of the price to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public virtual PointSensitivities priceSensitivityRates(ResolvedIborFutureTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		return productPricer.priceSensitivityRates(trade.Product, ratesProvider, hwProvider);
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
	  private double referencePrice(ResolvedIborFutureTrade trade, LocalDate valuationDate, double lastSettlementPrice)
	  {
		ArgChecker.notNull(valuationDate, "valuationDate");
		return trade.TradedPrice.filter(tp => tp.TradeDate.Equals(valuationDate)).map(tp => tp.Price).orElse(lastSettlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor future trade from the current price.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// </para>
	  /// <para>
	  /// The calculation is performed against a reference price. The reference price
	  /// must be the last settlement price used for margining, except on the trade date,
	  /// when it must be the trade price.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="currentPrice">  the current price, in decimal form </param>
	  /// <param name="referencePrice">  the reference price to margin against, typically the last settlement price, in decimal form </param>
	  /// <returns> the present value </returns>
	  internal virtual CurrencyAmount presentValue(ResolvedIborFutureTrade trade, double currentPrice, double referencePrice)
	  {
		ResolvedIborFuture future = trade.Product;
		double priceIndex = productPricer.marginIndex(future, currentPrice);
		double referenceIndex = productPricer.marginIndex(future, referencePrice);
		double pv = (priceIndex - referenceIndex) * trade.Quantity;
		return CurrencyAmount.of(future.Currency, pv);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor future trade.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// The current price is calculated using the Hull-White model.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedIborFutureTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider, double lastSettlementPrice)
	  {

		double referencePrice = this.referencePrice(trade, ratesProvider.ValuationDate, lastSettlementPrice);
		double price = this.price(trade, ratesProvider, hwProvider);
		return presentValue(trade, price, referencePrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the Ibor future trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public virtual PointSensitivities presentValueSensitivityRates(ResolvedIborFutureTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		ResolvedIborFuture product = trade.Product;
		PointSensitivities priceSensi = productPricer.priceSensitivityRates(product, ratesProvider, hwProvider);
		PointSensitivities marginIndexSensi = productPricer.marginIndexSensitivity(product, priceSensi);
		return marginIndexSensi.multipliedBy(trade.Quantity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity to piecewise constant volatility parameters of the Hull-White model.
	  /// </summary>
	  /// <param name="trade">  the trade to price </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the present value parameter sensitivity of the trade </returns>
	  public virtual DoubleArray presentValueSensitivityModelParamsHullWhite(ResolvedIborFutureTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		ResolvedIborFuture product = trade.Product;
		DoubleArray hwSensi = productPricer.priceSensitivityModelParamsHullWhite(product, ratesProvider, hwProvider);
		hwSensi = hwSensi.multipliedBy(product.Notional * product.AccrualFactor * trade.Quantity);
		return hwSensi;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par spread of the Ibor future trade.
	  /// <para>
	  /// The par spread is defined in the following way. When the reference price (or market quote)
	  /// is increased by the par spread, the present value of the trade is zero.
	  /// The current price is calculated using the Hull-White model.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <returns> the par spread. </returns>
	  public virtual double parSpread(ResolvedIborFutureTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider, double lastSettlementPrice)
	  {

		double referencePrice = this.referencePrice(trade, ratesProvider.ValuationDate, lastSettlementPrice);
		return price(trade, ratesProvider, hwProvider) - referencePrice;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par spread sensitivity of the Ibor future trade.
	  /// <para>
	  /// The par spread sensitivity of the trade is the sensitivity of the par spread to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the par spread curve sensitivity of the trade </returns>
	  public virtual PointSensitivities parSpreadSensitivityRates(ResolvedIborFutureTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		return productPricer.priceSensitivityRates(trade.Product, ratesProvider, hwProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the Ibor future trade.
	  /// <para>
	  /// Since the Ibor future is based on a single currency, the trade is exposed to only this currency.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <returns> the currency exposure of the trade </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedIborFutureTrade trade, RatesProvider provider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider, double lastSettlementPrice)
	  {

		return MultiCurrencyAmount.of(presentValue(trade, provider, hwProvider, lastSettlementPrice));
	  }

	}

}