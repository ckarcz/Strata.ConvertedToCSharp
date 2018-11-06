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
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SabrSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrSwaptionVolatilities;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using ResolvedCms = com.opengamma.strata.product.cms.ResolvedCms;
	using ResolvedCmsTrade = com.opengamma.strata.product.cms.ResolvedCmsTrade;

	/// <summary>
	/// Pricer for CMS trade by swaption replication on a SABR formula with extrapolation.
	/// <para>
	/// This function provides the ability to price <seealso cref="ResolvedCmsTrade"/>. 
	/// </para>
	/// </summary>
	public class SabrExtrapolationReplicationCmsTradePricer
	{

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedCms"/>.
	  /// </summary>
	  private readonly SabrExtrapolationReplicationCmsProductPricer productPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance using the default payment pricer.
	  /// </summary>
	  /// <param name="cmsProductPricer">  the pricer for <seealso cref="CmsLeg"/> </param>
	  public SabrExtrapolationReplicationCmsTradePricer(SabrExtrapolationReplicationCmsProductPricer cmsProductPricer) : this(cmsProductPricer, DiscountingPaymentPricer.DEFAULT)
	  {

	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="cmsProductPricer">  the pricer for <seealso cref="ResolvedCms"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public SabrExtrapolationReplicationCmsTradePricer(SabrExtrapolationReplicationCmsProductPricer cmsProductPricer, DiscountingPaymentPricer paymentPricer)
	  {

		this.productPricer = ArgChecker.notNull(cmsProductPricer, "cmsProductPricer");
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the CMS trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the CMS trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedCmsTrade trade, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		MultiCurrencyAmount pvCms = productPricer.presentValue(trade.Product, ratesProvider, swaptionVolatilities);
		if (!trade.Premium.Present)
		{
		  return pvCms;
		}
		CurrencyAmount pvPremium = paymentPricer.presentValue(trade.Premium.get(), ratesProvider);
		return pvCms.plus(pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Explains the present value of the CMS trade.
	  /// <para>
	  /// This returns explanatory information about the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the explain PV map </returns>
	  public virtual ExplainMap explainPresentValue(ResolvedCms cms, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		return productPricer.explainPresentValue(cms, ratesProvider, swaptionVolatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value curve sensitivity of the CMS trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the CMS trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueSensitivityRates(ResolvedCmsTrade trade, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		PointSensitivityBuilder pvSensiCms = productPricer.presentValueSensitivityRates(trade.Product, ratesProvider, swaptionVolatilities);
		if (!trade.Premium.Present)
		{
		  return pvSensiCms.build();
		}
		PointSensitivityBuilder pvSensiPremium = paymentPricer.presentValueSensitivity(trade.Premium.get(), ratesProvider);
		return pvSensiCms.combinedWith(pvSensiPremium).build();
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity to the SABR model parameters.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to the SABR model parameters, 
	  /// alpha, beta, rho and nu.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the CMS trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueSensitivityModelParamsSabr(ResolvedCmsTrade trade, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		return productPricer.presentValueSensitivityModelParamsSabr(trade.Product, ratesProvider, swaptionVolatilities).build();
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity to the strike value.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to the strike value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the CMS trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual double presentValueSensitivityStrike(ResolvedCmsTrade trade, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		return productPricer.presentValueSensitivityStrike(trade.Product, ratesProvider, swaptionVolatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the trade.
	  /// </summary>
	  /// <param name="trade">  the CMS trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedCmsTrade trade, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		MultiCurrencyAmount ceCms = productPricer.currencyExposure(trade.Product, ratesProvider, swaptionVolatilities);
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
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the current cash </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedCmsTrade trade, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		MultiCurrencyAmount ccCms = productPricer.currentCash(trade.Product, ratesProvider, swaptionVolatilities);
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