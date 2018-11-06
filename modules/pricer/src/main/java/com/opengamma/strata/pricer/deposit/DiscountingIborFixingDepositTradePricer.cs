/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.deposit
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedIborFixingDeposit = com.opengamma.strata.product.deposit.ResolvedIborFixingDeposit;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;

	/// <summary>
	/// The methods associated to the pricing of Ibor fixing deposit trades by discounting.
	/// <para>
	/// This provides the ability to price <seealso cref="ResolvedIborFixingDepositTrade"/>.
	/// These trades are synthetic trades which are used for curve calibration purposes.
	/// They should not be used as actual trades.
	/// </para>
	/// </summary>
	public class DiscountingIborFixingDepositTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingIborFixingDepositTradePricer DEFAULT = new DiscountingIborFixingDepositTradePricer(DiscountingIborFixingDepositProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedIborFixingDeposit"/>.
	  /// </summary>
	  private readonly DiscountingIborFixingDepositProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedIborFixingDeposit"/> </param>
	  public DiscountingIborFixingDepositTradePricer(DiscountingIborFixingDepositProductPricer productPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor fixing deposit trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedIborFixingDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValue(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the Ibor fixing deposit trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedIborFixingDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.presentValueSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the deposit fair rate given the start and end time and the accrual factor.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate </returns>
	  public virtual double parRate(ResolvedIborFixingDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.parRate(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the deposit fair rate sensitivity to the curves.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate curve sensitivity </returns>
	  public virtual PointSensitivities parRateSensitivity(ResolvedIborFixingDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.parRateSensitivity(trade.Product, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the spread to be added to the deposit rate to have a zero present value.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedIborFixingDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpread(trade.Product, provider);
	  }

	  /// <summary>
	  /// Calculates the par spread curve sensitivity.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread curve sensitivity </returns>
	  public virtual PointSensitivities parSpreadSensitivity(ResolvedIborFixingDepositTrade trade, RatesProvider provider)
	  {
		return productPricer.parSpreadSensitivity(trade.Product, provider);
	  }

	}

}