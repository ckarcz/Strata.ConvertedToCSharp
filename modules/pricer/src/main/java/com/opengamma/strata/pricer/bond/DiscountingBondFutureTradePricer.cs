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
	using BondFutureTrade = com.opengamma.strata.product.bond.BondFutureTrade;
	using FixedCouponBond = com.opengamma.strata.product.bond.FixedCouponBond;
	using ResolvedBondFuture = com.opengamma.strata.product.bond.ResolvedBondFuture;
	using ResolvedBondFutureTrade = com.opengamma.strata.product.bond.ResolvedBondFutureTrade;

	/// <summary>
	/// Pricer implementation for bond future trades.
	/// <para>
	/// This function provides the ability to price a <seealso cref="BondFutureTrade"/>.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bond futures in the trade model, pricers and market data.
	/// This is coherent with the pricing of <seealso cref="FixedCouponBond"/>. The bond futures delivery is a bond
	/// for an amount computed from the bond future price, a conversion factor and the accrued interest.
	/// </para>
	/// </summary>
	public sealed class DiscountingBondFutureTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingBondFutureTradePricer DEFAULT = new DiscountingBondFutureTradePricer(DiscountingBondFutureProductPricer.DEFAULT);

	  /// <summary>
	  /// Underlying pricer.
	  /// </summary>
	  private readonly DiscountingBondFutureProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="BondFuture"/> </param>
	  public DiscountingBondFutureTradePricer(DiscountingBondFutureProductPricer productPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the bond future trade from the current price.
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
	  /// <param name="currentPrice">  the price on the valuation date </param>
	  /// <param name="referencePrice">  the price with respect to which the margining should be done </param>
	  /// <returns> the present value </returns>
	  internal CurrencyAmount presentValue(ResolvedBondFutureTrade trade, double currentPrice, double referencePrice)
	  {
		ResolvedBondFuture future = trade.Product;
		double priceIndex = productPricer.marginIndex(future, currentPrice);
		double referenceIndex = productPricer.marginIndex(future, referencePrice);
		double pv = (priceIndex - referenceIndex) * trade.Quantity;
		return CurrencyAmount.of(future.Currency, pv);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the bond future trade.
	  /// <para>
	  /// The price of the trade is the price on the valuation date.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bond futures. This is coherent with the pricing of <seealso cref="FixedCouponBond"/>.
	  /// For example, a price of 99.32% is represented in Strata by 0.9932.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <returns> the price of the trade, in decimal form </returns>
	  public double price(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {
		return productPricer.price(trade.Product, discountingProvider);
	  }

	  /// <summary>
	  /// Calculates the price of the bond future trade with z-spread.
	  /// <para>
	  /// The price of the trade is the price on the valuation date.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic compounded rates 
	  /// of the issuer discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodPerYear">  the number of periods per year </param>
	  /// <returns> the price of the trade, in decimal form </returns>
	  public double priceWithZSpread(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider, double zSpread, CompoundedRateType compoundedRateType, int periodPerYear)
	  {

		return productPricer.priceWithZSpread(trade.Product, discountingProvider, zSpread, compoundedRateType, periodPerYear);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the bond future trade.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <returns> the present value </returns>
	  public CurrencyAmount presentValue(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider, double lastSettlementPrice)
	  {

		double price = this.price(trade, discountingProvider);
		double referencePrice = this.referencePrice(trade, discountingProvider.ValuationDate, lastSettlementPrice);
		return presentValue(trade, price, referencePrice);
	  }

	  /// <summary>
	  /// Calculates the present value of the bond future trade with z-spread.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic compounded rates 
	  /// of the issuer discounting curve.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodPerYear">  the number of periods per year </param>
	  /// <returns> the present value </returns>
	  public CurrencyAmount presentValueWithZSpread(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider, double lastSettlementPrice, double zSpread, CompoundedRateType compoundedRateType, int periodPerYear)
	  {

		double price = priceWithZSpread(trade, discountingProvider, zSpread, compoundedRateType, periodPerYear);
		double referencePrice = this.referencePrice(trade, discountingProvider.ValuationDate, lastSettlementPrice);
		return presentValue(trade, price, referencePrice);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the bond future trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public PointSensitivities presentValueSensitivity(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		ResolvedBondFuture product = trade.Product;
		PointSensitivities priceSensi = productPricer.priceSensitivity(product, discountingProvider);
		PointSensitivities marginIndexSensi = productPricer.marginIndexSensitivity(product, priceSensi);
		return marginIndexSensi.multipliedBy(trade.Quantity);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the bond future trade with z-spread.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic compounded rates 
	  /// of the issuer discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodPerYear">  the number of periods per year </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public PointSensitivities presentValueSensitivityWithZSpread(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider, double zSpread, CompoundedRateType compoundedRateType, int periodPerYear)
	  {

		ResolvedBondFuture product = trade.Product;
		PointSensitivities priceSensi = productPricer.priceSensitivityWithZSpread(product, discountingProvider, zSpread, compoundedRateType, periodPerYear);
		PointSensitivities marginIndexSensi = productPricer.marginIndexSensitivity(product, priceSensi);
		return marginIndexSensi.multipliedBy(trade.Quantity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par spread of the bond future trade.
	  /// <para>
	  /// The par spread is defined in the following way. When the reference price (or market quote)
	  /// is increased by the par spread, the present value of the trade is zero.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <returns> the par spread </returns>
	  public double parSpread(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider, double lastSettlementPrice)
	  {

		double referencePrice = this.referencePrice(trade, discountingProvider.ValuationDate, lastSettlementPrice);
		return price(trade, discountingProvider) - referencePrice;
	  }

	  /// <summary>
	  /// Calculates the par spread of the bond future trade with z-spread.
	  /// <para>
	  /// The par spread is defined in the following way. When the reference price (or market quote)
	  /// is increased by the par spread, the present value of the trade is zero.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic compounded rates 
	  /// of the issuer discounting curve.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodPerYear">  the number of periods per year </param>
	  /// <returns> the par spread </returns>
	  public double parSpreadWithZSpread(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider, double lastSettlementPrice, double zSpread, CompoundedRateType compoundedRateType, int periodPerYear)
	  {

		double referencePrice = this.referencePrice(trade, discountingProvider.ValuationDate, lastSettlementPrice);
		return priceWithZSpread(trade, discountingProvider, zSpread, compoundedRateType, periodPerYear) - referencePrice;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par spread sensitivity of the bond future trade.
	  /// <para>
	  /// The par spread sensitivity of the trade is the sensitivity of the par spread to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <returns> the par spread curve sensitivity of the trade </returns>
	  public PointSensitivities parSpreadSensitivity(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		return productPricer.priceSensitivity(trade.Product, discountingProvider);
	  }

	  /// <summary>
	  /// Calculates the par spread sensitivity of the bond future trade with z-spread.
	  /// <para>
	  /// The par spread sensitivity of the trade is the sensitivity of the par spread to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic compounded rates 
	  /// of the issuer discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodPerYear">  the number of periods per year </param>
	  /// <returns> the par spread curve sensitivity of the trade </returns>
	  public PointSensitivities parSpreadSensitivityWithZSpread(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider, double zSpread, CompoundedRateType compoundedRateType, int periodPerYear)
	  {

		return productPricer.priceSensitivityWithZSpread(trade.Product, discountingProvider, zSpread, compoundedRateType, periodPerYear);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the bond future trade.
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <returns> the currency exposure of the bond future trade </returns>
	  public MultiCurrencyAmount currencyExposure(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider, double lastSettlementPrice)
	  {

		double price = this.price(trade, discountingProvider);
		double referencePrice = this.referencePrice(trade, discountingProvider.ValuationDate, lastSettlementPrice);
		return MultiCurrencyAmount.of(presentValue(trade, price, referencePrice));
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the bond future trade with z-spread.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic compounded rates 
	  /// of the issuer discounting curve.
	  /// </para>
	  /// <para>
	  /// This method calculates based on the difference between the model price and the
	  /// last settlement price, or the trade price if traded on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="lastSettlementPrice">  the last settlement price used for margining, in decimal form </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodPerYear">  the number of periods per year </param>
	  /// <returns> the currency exposure of the bond future trade </returns>
	  public MultiCurrencyAmount currencyExposureWithZSpread(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider, double lastSettlementPrice, double zSpread, CompoundedRateType compoundedRateType, int periodPerYear)
	  {

		double price = priceWithZSpread(trade, discountingProvider, zSpread, compoundedRateType, periodPerYear);
		double referencePrice = this.referencePrice(trade, discountingProvider.ValuationDate, lastSettlementPrice);
		return MultiCurrencyAmount.of(presentValue(trade, price, referencePrice));
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
	  private double referencePrice(ResolvedBondFutureTrade trade, LocalDate valuationDate, double lastSettlementPrice)
	  {
		ArgChecker.notNull(valuationDate, "valuationDate");
		return trade.TradedPrice.filter(tp => tp.TradeDate.Equals(valuationDate)).map(tp => tp.Price).orElse(lastSettlementPrice);
	  }

	}

}