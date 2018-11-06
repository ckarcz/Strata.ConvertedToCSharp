/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.deposit
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedTermDeposit = com.opengamma.strata.product.deposit.ResolvedTermDeposit;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;

	/// <summary>
	/// The methods associated to the pricing of term deposit by discounting.
	/// <para>
	/// This provides the ability to price <seealso cref="ResolvedTermDeposit"/>.
	/// </para>
	/// </summary>
	public class DiscountingTermDepositTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingTermDepositTradePricer DEFAULT = new DiscountingTermDepositTradePricer(DiscountingTermDepositProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedTermDeposit"/>.
	  /// </summary>
	  private readonly DiscountingTermDepositProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedTermDeposit"/> </param>
	  public DiscountingTermDepositTradePricer(DiscountingTermDepositProductPricer productPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value by discounting the final cash flow (nominal + interest)
	  /// and the initial payment (initial amount).
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedTermDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValue(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity by discounting the final cash flow (nominal + interest)
	  /// and the initial payment (initial amount).
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedTermDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValueSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the deposit fair rate given the start and end time and the accrual factor.
	  /// <para>
	  /// When the deposit has already started the number may not be meaningful as the remaining period
	  /// is not in line with the accrual factor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate </returns>
	  public virtual double parRate(ResolvedTermDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.parRate(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the par rate curve sensitivity.
	  /// <para>
	  /// The calculation is based on both of initial and final payments.
	  /// Thus the number resulting may not be meaningful when deposit has already started and only the final
	  /// payment remains (no initial payment).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate curve sensitivity </returns>
	  public virtual PointSensitivities parRateSensitivity(ResolvedTermDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.parRateSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the spread to be added to the deposit rate to have a zero present value.
	  /// <para>
	  /// The calculation is based on both the initial and final payments.
	  /// Thus the resulting number may not be meaningful when deposit has already started and only the final
	  /// payment remains (no initial payment).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedTermDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpread(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the par spread curve sensitivity.
	  /// <para>
	  /// The calculation is based on both of initial and final payments.
	  /// Thus the number resulting may not be meaningful when deposit has already started and only the final
	  /// payment remains (no initial payment).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread curve sensitivity </returns>
	  public virtual PointSensitivities parSpreadSensitivity(ResolvedTermDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpreadSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedTermDepositTrade trade, RatesProvider provider)
	  {
		return MultiCurrencyAmount.of(presentValue(trade, provider));
	  }

	  /// <summary>
	  /// Calculates the current cash.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(ResolvedTermDepositTrade trade, RatesProvider provider)
	  {
		ResolvedTermDeposit product = trade.Product;
		if (product.StartDate.isEqual(provider.ValuationDate))
		{
		  return CurrencyAmount.of(product.Currency, -product.Notional);
		}
		if (product.EndDate.isEqual(provider.ValuationDate))
		{
		  return CurrencyAmount.of(product.Currency, product.Notional + product.Interest);
		}
		return CurrencyAmount.zero(product.Currency);
	  }

	}

}