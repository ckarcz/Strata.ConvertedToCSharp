/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.cms
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedCms = com.opengamma.strata.product.cms.ResolvedCms;
	using ResolvedCmsTrade = com.opengamma.strata.product.cms.ResolvedCmsTrade;
	using Swap = com.opengamma.strata.product.swap.Swap;

	/// <summary>
	/// Pricer for CMS trade by simple forward estimation.
	///  <para>
	///  This is an overly simplistic approach to CMS coupon pricer. It is provided only for testing and comparison 
	///  purposes. It is not recommended to use this for valuation or risk management purposes.
	/// </para>
	/// </summary>
	public class DiscountingCmsTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingCmsTradePricer DEFAULT = new DiscountingCmsTradePricer(DiscountingSwapProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedCms"/>.
	  /// </summary>
	  private readonly DiscountingCmsProductPricer productPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="swapPricer">  the pricer for <seealso cref="Swap"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public DiscountingCmsTradePricer(DiscountingSwapProductPricer swapPricer, DiscountingPaymentPricer paymentPricer)
	  {
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
		this.productPricer = new DiscountingCmsProductPricer(swapPricer);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the CMS trade by simple forward estimation.
	  /// </summary>
	  /// <param name="trade">  the CMS trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the present value </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedCmsTrade trade, RatesProvider ratesProvider)
	  {

		MultiCurrencyAmount pvCms = productPricer.presentValue(trade.Product, ratesProvider);
		if (!trade.Premium.Present)
		{
		  return pvCms;
		}
		CurrencyAmount pvPremium = paymentPricer.presentValue(trade.Premium.get(), ratesProvider);
		return pvCms.plus(pvPremium);
	  }

	  /// <summary>
	  /// Calculates the present value curve sensitivity of the CMS trade by simple forward estimation.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the CMS trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedCmsTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivityBuilder pvSensiCms = productPricer.presentValueSensitivity(trade.Product, ratesProvider);
		if (!trade.Premium.Present)
		{
		  return pvSensiCms.build();
		}
		PointSensitivityBuilder pvSensiPremium = paymentPricer.presentValueSensitivity(trade.Premium.get(), ratesProvider);
		return pvSensiCms.combinedWith(pvSensiPremium).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the trade.
	  /// </summary>
	  /// <param name="trade">  the CMS trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedCmsTrade trade, RatesProvider ratesProvider)
	  {

		MultiCurrencyAmount ceCms = productPricer.currencyExposure(trade.Product, ratesProvider);
		if (!trade.Premium.Present)
		{
		  return ceCms;
		}
		CurrencyAmount pvPremium = paymentPricer.presentValue(trade.Premium.get(), ratesProvider);
		return ceCms.plus(pvPremium);
	  }

	  /// <summary>
	  /// Calculates the current cash of the trade.
	  /// </summary>
	  /// <param name="trade">  the CMS trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the current cash </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedCmsTrade trade, RatesProvider ratesProvider)
	  {

		MultiCurrencyAmount ccCms = productPricer.currentCash(trade.Product, ratesProvider);
		if (!trade.Premium.Present)
		{
		  return ccCms;
		}
		Payment premium = trade.Premium.get();
		if (premium.Date.Equals(ratesProvider.ValuationDate))
		{
		  ccCms = ccCms.plus(premium.Currency, premium.Amount);
		}
		return ccCms;
	  }

	}

}