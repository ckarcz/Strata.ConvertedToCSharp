/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fra
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFra = com.opengamma.strata.product.fra.ResolvedFra;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;

	/// <summary>
	/// Pricer for for forward rate agreement (FRA) trades.
	/// <para>
	/// This provides the ability to price <seealso cref="ResolvedFraTrade"/>.
	/// The trade is priced by pricing the underlying product using a forward curve for the index.
	/// </para>
	/// </summary>
	public class DiscountingFraTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFraTradePricer DEFAULT = new DiscountingFraTradePricer(DiscountingFraProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFra"/>.
	  /// </summary>
	  private readonly DiscountingFraProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedFra"/> </param>
	  public DiscountingFraTradePricer(DiscountingFraProductPricer productPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying product pricer.
	  /// </summary>
	  /// <returns> the product pricer </returns>
	  public virtual DiscountingFraProductPricer ProductPricer
	  {
		  get
		  {
			return productPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the FRA trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// This is the discounted forecast value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the trade </returns>
	  public virtual CurrencyAmount presentValue(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValue(trade.Product, provider);
	  }

	  /// <summary>
	  /// Explains the present value of the FRA product.
	  /// <para>
	  /// This returns explanatory information about the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the explanatory information </returns>
	  public virtual ExplainMap explainPresentValue(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return productPricer.explainPresentValue(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the FRA trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValueSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forecast value of the FRA trade.
	  /// <para>
	  /// The forecast value of the trade is the value on the valuation date without present value discounting.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value of the trade </returns>
	  public virtual CurrencyAmount forecastValue(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return productPricer.forecastValue(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the forecast value sensitivity of the FRA trade.
	  /// <para>
	  /// The forecast value sensitivity of the product is the sensitivity of the forecast value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the forecast value </returns>
	  public virtual PointSensitivities forecastValueSensitivity(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return productPricer.forecastValueSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par rate of the FRA trade.
	  /// <para>
	  /// The par rate is the rate for which the FRA present value is 0.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate </returns>
	  public virtual double parRate(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return productPricer.parRate(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the par rate curve sensitivity of the FRA trade.
	  /// <para>
	  /// The par rate curve sensitivity of the product is the sensitivity of the par rate to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate sensitivity </returns>
	  public virtual PointSensitivities parRateSensitivity(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return productPricer.parRateSensitivity(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the par spread of the FRA trade.
	  /// <para>
	  /// This is spread to be added to the fixed rate to have a present value of 0.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpread(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the par spread curve sensitivity of the FRA trade.
	  /// <para>
	  /// The par spread curve sensitivity of the product is the sensitivity of the par spread to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread sensitivity </returns>
	  public virtual PointSensitivities parSpreadSensitivity(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpreadSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the future cash flow of the FRA trade.
	  /// <para>
	  /// There is only one cash flow on the payment date for the FRA trade.
	  /// The expected currency amount of the cash flow is the same as <seealso cref="#forecastValue(ResolvedFraTrade, RatesProvider)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the cash flows </returns>
	  public virtual CashFlows cashFlows(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return productPricer.cashFlows(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the FRA trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFraTrade trade, RatesProvider provider)
	  {
		return MultiCurrencyAmount.of(presentValue(trade, provider));
	  }

	  /// <summary>
	  /// Calculates the current cash of the FRA trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(ResolvedFraTrade trade, RatesProvider provider)
	  {
		ResolvedFra fra = trade.Product;
		if (fra.PaymentDate.isEqual(provider.ValuationDate))
		{
		  return productPricer.presentValue(fra, provider);
		}
		return CurrencyAmount.zero(fra.Currency);
	  }

	}

}