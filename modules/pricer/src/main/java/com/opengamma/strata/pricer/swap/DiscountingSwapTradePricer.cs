/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swap
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;

	/// <summary>
	/// Pricer for for rate swap trades.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedSwapTrade"/>.
	/// The product is priced by pricing the product.
	/// </para>
	/// </summary>
	public class DiscountingSwapTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingSwapTradePricer DEFAULT = new DiscountingSwapTradePricer(DiscountingSwapProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedSwap"/>.
	  /// </summary>
	  private readonly DiscountingSwapProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedSwap"/> </param>
	  public DiscountingSwapTradePricer(DiscountingSwapProductPricer productPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying product pricer.
	  /// </summary>
	  /// <returns> the product pricer </returns>
	  public virtual DiscountingSwapProductPricer ProductPricer
	  {
		  get
		  {
			return productPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the swap trade, converted to the specified currency.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// This is the discounted forecast value.
	  /// The result is converted to the specified currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="currency">  the currency to convert to </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the swap trade in the specified currency </returns>
	  public virtual CurrencyAmount presentValue(ResolvedSwapTrade trade, Currency currency, RatesProvider provider)
	  {
		return productPricer.presentValue(trade.Product, currency, provider);
	  }

	  /// <summary>
	  /// Calculates the present value of the swap trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// This is the discounted forecast value.
	  /// The result is expressed using the payment currency of each leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the swap trade </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValue(trade.Product, provider);
	  }

	  /// <summary>
	  /// Explains the present value of the swap trade.
	  /// <para>
	  /// This returns explanatory information about the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the explanatory information </returns>
	  public virtual ExplainMap explainPresentValue(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.explainPresentValue(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the swap trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value curve sensitivity of the swap trade </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValueSensitivity(trade.Product, provider).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forecast value of the swap trade.
	  /// <para>
	  /// The forecast value of the trade is the value on the valuation date without present value discounting.
	  /// The result is expressed using the payment currency of each leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value of the swap trade </returns>
	  public virtual MultiCurrencyAmount forecastValue(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.forecastValue(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the forecast value sensitivity of the swap trade.
	  /// <para>
	  /// The forecast value sensitivity of the trade is the sensitivity of the forecast value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value curve sensitivity of the swap trade </returns>
	  public virtual PointSensitivities forecastValueSensitivity(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.forecastValueSensitivity(trade.Product, provider).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par rate of the swap trade.
	  /// <para>
	  /// The par rate is the rate for which the swap present value is 0.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate </returns>
	  public virtual double parRate(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.parRate(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the par rate curve sensitivity of the swap trade.
	  /// <para>
	  /// The par rate curve sensitivity of the product is the sensitivity of the par rate to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate sensitivity </returns>
	  public virtual PointSensitivities parRateSensitivity(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.parRateSensitivity(trade.Product, provider).build();
	  }

	  /// <summary>
	  /// Calculates the par spread of the swap trade.
	  /// <para>
	  /// This is spread to be added to the fixed rate to have a present value of 0.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpread(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the par spread curve sensitivity of the swap trade.
	  /// <para>
	  /// The par spread curve sensitivity of the product is the sensitivity of the par spread to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread sensitivity </returns>
	  public virtual PointSensitivities parSpreadSensitivity(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpreadSensitivity(trade.Product, provider).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the future cash flows of the swap trade.
	  /// <para>
	  /// Each expected cash flow is added to the result.
	  /// This is based on <seealso cref="#forecastValue(ResolvedSwapTrade, RatesProvider)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the cash flows </returns>
	  public virtual CashFlows cashFlows(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.cashFlows(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the accrued interest since the last payment.
	  /// <para>
	  /// This determines the payment period applicable at the valuation date and calculates
	  /// the accrued interest since the last payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the accrued interest </returns>
	  public virtual MultiCurrencyAmount accruedInterest(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.accruedInterest(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the swap trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure of the swap trade </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.currencyExposure(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the current cash of the swap trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash of the swap trade </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedSwapTrade trade, RatesProvider provider)
	  {
		return productPricer.currentCash(trade.Product, provider);
	  }

	}

}