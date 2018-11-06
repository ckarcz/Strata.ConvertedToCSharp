/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxNdf = com.opengamma.strata.product.fx.ResolvedFxNdf;
	using ResolvedFxNdfTrade = com.opengamma.strata.product.fx.ResolvedFxNdfTrade;

	/// <summary>
	/// Pricer for FX non-deliverable forward (NDF) trades.
	/// <para>
	/// This provides the ability to price an <seealso cref="ResolvedFxNdfTrade"/>.
	/// The product is priced using forward curves for the currency pair.
	/// </para>
	/// </summary>
	public class DiscountingFxNdfTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFxNdfTradePricer DEFAULT = new DiscountingFxNdfTradePricer(DiscountingFxNdfProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxNdf"/>.
	  /// </summary>
	  private readonly DiscountingFxNdfProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedFxNdf"/> </param>
	  public DiscountingFxNdfTradePricer(DiscountingFxNdfProductPricer productPricer)
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
	  public virtual CurrencyAmount presentValue(ResolvedFxNdfTrade trade, RatesProvider provider)
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
	  public virtual PointSensitivities presentValueSensitivity(ResolvedFxNdfTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValueSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure by discounting each payment in its own currency.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxNdfTrade trade, RatesProvider provider)
	  {
		return productPricer.currencyExposure(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the current cash of the trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash of the trade in the settlement currency </returns>
	  public virtual CurrencyAmount currentCash(ResolvedFxNdfTrade trade, RatesProvider provider)
	  {
		return productPricer.currentCash(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forward exchange rate.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forward rate </returns>
	  public virtual FxRate forwardFxRate(ResolvedFxNdfTrade trade, RatesProvider provider)
	  {
		return productPricer.forwardFxRate(trade.Product, provider);
	  }

	}

}