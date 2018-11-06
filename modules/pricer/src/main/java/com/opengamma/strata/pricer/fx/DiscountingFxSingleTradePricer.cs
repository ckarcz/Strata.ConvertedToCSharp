/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxSingleTrade = com.opengamma.strata.product.fx.ResolvedFxSingleTrade;

	/// <summary>
	/// Pricer for foreign exchange transaction trades.
	/// <para>
	/// This provides the ability to price an <seealso cref="ResolvedFxSingleTrade"/>.
	/// </para>
	/// </summary>
	public class DiscountingFxSingleTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFxSingleTradePricer DEFAULT = new DiscountingFxSingleTradePricer(DiscountingFxSingleProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxSingle"/>.
	  /// </summary>
	  private readonly DiscountingFxSingleProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedFxSingle"/> </param>
	  public DiscountingFxSingleTradePricer(DiscountingFxSingleProductPricer productPricer)
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
	  public virtual MultiCurrencyAmount presentValue(ResolvedFxSingleTrade trade, RatesProvider provider)
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
	  public virtual PointSensitivities presentValueSensitivity(ResolvedFxSingleTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValueSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par spread.
	  /// <para>
	  /// This is the spread that should be added to the FX points to have a zero value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the spread </returns>
	  public virtual double parSpread(ResolvedFxSingleTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpread(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure by discounting each payment in its own currency.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxSingleTrade trade, RatesProvider provider)
	  {
		return productPricer.currencyExposure(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the current cash of the trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash of the trade in the settlement currency </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedFxSingleTrade trade, RatesProvider provider)
	  {
		return productPricer.currentCash(trade.Product, provider.ValuationDate);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forward exchange rate.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forward rate </returns>
	  public virtual FxRate forwardFxRate(ResolvedFxSingleTrade trade, RatesProvider provider)
	  {
		return productPricer.forwardFxRate(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the forward exchange rate point sensitivity.
	  /// <para>
	  /// The returned value is based on the direction of the FX product.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity </returns>
	  public virtual PointSensitivities forwardFxRatePointSensitivity(ResolvedFxSingleTrade trade, RatesProvider provider)
	  {
		return productPricer.forwardFxRatePointSensitivity(trade.Product, provider).build();
	  }

	  /// <summary>
	  /// Calculates the sensitivity of the forward exchange rate to the spot rate.
	  /// <para>
	  /// The returned value is based on the direction of the FX product.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the sensitivity to spot </returns>
	  public virtual double forwardFxRateSpotSensitivity(ResolvedFxSingleTrade trade, RatesProvider provider)
	  {
		return productPricer.forwardFxRateSpotSensitivity(trade.Product, provider);
	  }

	}

}