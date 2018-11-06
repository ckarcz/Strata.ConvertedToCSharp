/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.dsf
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using Dsf = com.opengamma.strata.product.dsf.Dsf;
	using ResolvedDsf = com.opengamma.strata.product.dsf.ResolvedDsf;
	using ResolvedDsfTrade = com.opengamma.strata.product.dsf.ResolvedDsfTrade;

	/// <summary>
	/// Pricer implementation for Deliverable Swap Futures (DSFs).
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedDsfTrade"/>.
	/// 
	/// <h4>Price</h4>
	/// The price of a DSF is based on the present value (NPV) of the underlying swap on the delivery date.
	/// For example, a price of 100.182 represents a present value of $100,182.00, if the notional is $100,000.
	/// This price can also be viewed as a percentage present value - {@code (100 + percentPv)}, or 0.182% in this example.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for DSFs in the trade model, pricers and market data.
	/// The decimal price is based on the decimal multiplier equivalent to the implied percentage.
	/// Thus the market price of 100.182 is represented in Strata by 1.00182.
	/// </para>
	/// </summary>
	public class DiscountingDsfTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingDsfTradePricer DEFAULT = new DiscountingDsfTradePricer(DiscountingDsfProductPricer.DEFAULT);

	  /// <summary>
	  /// Underlying pricer.
	  /// </summary>
	  private readonly DiscountingDsfProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="Dsf"/> </param>
	  public DiscountingDsfTradePricer(DiscountingDsfProductPricer productPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the underlying deliverable swap futures product.
	  /// <para>
	  /// The price of the trade is the price on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the price of the trade, in decimal form </returns>
	  public virtual double price(ResolvedDsfTrade trade, RatesProvider ratesProvider)
	  {
		return productPricer.price(trade.Product, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price sensitivity of the deliverable swap futures product.
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of the price to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the price curve sensitivity of the trade </returns>
	  public virtual PointSensitivities priceSensitivity(ResolvedDsfTrade trade, RatesProvider ratesProvider)
	  {
		return productPricer.priceSensitivity(trade.Product, ratesProvider);
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
	  private double referencePrice(ResolvedDsfTrade trade, LocalDate valuationDate, double lastSettlementPrice)
	  {
		ArgChecker.notNull(valuationDate, "valuationDate");
		return trade.TradedPrice.filter(tp => tp.TradeDate.Equals(valuationDate)).map(tp => tp.Price).orElse(lastSettlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the deliverable swap futures trade from the current price.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// The current price is specified, not calculated.
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
	  private CurrencyAmount presentValue(ResolvedDsfTrade trade, double currentPrice, double referencePrice)
	  {

		ResolvedDsf future = trade.Product;
		double priceIndex = productPricer.marginIndex(future, currentPrice);
		double referenceIndex = productPricer.marginIndex(future, referencePrice);
		double pv = (priceIndex - referenceIndex) * trade.Quantity;
		return CurrencyAmount.of(future.Currency, pv);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the deliverable swap futures trade.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// The current price is calculated using the discounting model.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedDsfTrade trade, RatesProvider ratesProvider, double lastSettlementPrice)
	  {

		double price = this.price(trade, ratesProvider);
		double referencePrice = this.referencePrice(trade, ratesProvider.ValuationDate, lastSettlementPrice);
		return presentValue(trade, price, referencePrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the deliverable swap futures trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedDsfTrade trade, RatesProvider ratesProvider)
	  {
		ResolvedDsf product = trade.Product;
		PointSensitivities priceSensi = productPricer.priceSensitivity(product, ratesProvider);
		PointSensitivities marginIndexSensi = productPricer.marginIndexSensitivity(product, priceSensi);
		return marginIndexSensi.multipliedBy(trade.Quantity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the deliverable swap futures trade.
	  /// <para>
	  /// Since the deliverable swap futures is based on a single currency, the trade is exposed to only this currency.
	  /// The current price is calculated using the discounting model.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <returns> the currency exposure of the trade </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedDsfTrade trade, RatesProvider ratesProvider, double lastSettlementPrice)
	  {

		return MultiCurrencyAmount.of(presentValue(trade, ratesProvider, lastSettlementPrice));
	  }

	}

}