/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSwap = com.opengamma.strata.product.fx.ResolvedFxSwap;
	using ResolvedFxSwapTrade = com.opengamma.strata.product.fx.ResolvedFxSwapTrade;

	/// <summary>
	/// Pricer for foreign exchange swap transaction trades.
	/// <para>
	/// This provides the ability to price an <seealso cref="ResolvedFxSwapTrade"/>.
	/// </para>
	/// </summary>
	public class DiscountingFxSwapTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFxSwapTradePricer DEFAULT = new DiscountingFxSwapTradePricer(DiscountingFxSwapProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxSwap"/>.
	  /// </summary>
	  private readonly DiscountingFxSwapProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedFxSwap"/> </param>
	  public DiscountingFxSwapTradePricer(DiscountingFxSwapProductPricer productPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// The present value is returned in the settlement currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the trade in the settlement currency </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedFxSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValue(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the present value curve sensitivity of the trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedFxSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValueSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par spread.
	  /// <para>
	  /// The par spread is the spread that should be added to the FX forward points to have a zero value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the spread </returns>
	  public virtual double parSpread(ResolvedFxSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpread(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the par spread sensitivity to the curves.
	  /// <para>
	  /// The sensitivity is reported in the counter currency of the product, but is actually dimensionless.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the spread curve sensitivity </returns>
	  public virtual PointSensitivities parSpreadSensitivity(ResolvedFxSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpreadSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure by discounting each payment in its own currency.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.currencyExposure(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the current cash of the trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash of the trade in the settlement currency </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedFxSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.currentCash(trade.Product, provider.ValuationDate);
	  }

	}

}