using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.cms
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapLegPricer = com.opengamma.strata.pricer.swap.DiscountingSwapLegPricer;
	using SabrSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrSwaptionVolatilities;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using ResolvedCms = com.opengamma.strata.product.cms.ResolvedCms;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// Pricer for CMS products by swaption replication on a SABR formula with extrapolation.
	/// <para>
	/// This function provides the ability to price <seealso cref="ResolvedCms"/>. 
	/// </para>
	/// </summary>
	public class SabrExtrapolationReplicationCmsProductPricer
	{

	  /// <summary>
	  /// The pricer for <seealso cref="CmsLeg"/>.
	  /// </summary>
	  private readonly SabrExtrapolationReplicationCmsLegPricer cmsLegPricer;
	  /// <summary>
	  /// The pricer for <seealso cref="SwapLeg"/>.
	  /// </summary>
	  private readonly DiscountingSwapLegPricer payLegPricer;

	  /// <summary>
	  /// Creates an instance using the default pay leg pricer.
	  /// </summary>
	  /// <param name="cmsLegPricer">  the pricer for <seealso cref="CmsLeg"/> </param>
	  public SabrExtrapolationReplicationCmsProductPricer(SabrExtrapolationReplicationCmsLegPricer cmsLegPricer) : this(cmsLegPricer, DiscountingSwapLegPricer.DEFAULT)
	  {

	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="cmsLegPricer">  the pricer for <seealso cref="CmsLeg"/> </param>
	  /// <param name="payLegPricer">  the pricer for <seealso cref="SwapLeg"/> </param>
	  public SabrExtrapolationReplicationCmsProductPricer(SabrExtrapolationReplicationCmsLegPricer cmsLegPricer, DiscountingSwapLegPricer payLegPricer)
	  {

		this.cmsLegPricer = ArgChecker.notNull(cmsLegPricer, "cmsLegPricer");
		this.payLegPricer = ArgChecker.notNull(payLegPricer, "payLegPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the CMS product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// </para>
	  /// <para>
	  /// CMS leg and pay leg are typically in the same currency. Thus the present value is expressed as a 
	  /// single currency amount in most cases.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedCms cms, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		CurrencyAmount pvCmsLeg = cmsLegPricer.presentValue(cms.CmsLeg, ratesProvider, swaptionVolatilities);
		if (!cms.PayLeg.Present)
		{
		  return MultiCurrencyAmount.of(pvCmsLeg);
		}
		CurrencyAmount pvPayLeg = payLegPricer.presentValue(cms.PayLeg.get(), ratesProvider);
		return MultiCurrencyAmount.of(pvCmsLeg).plus(pvPayLeg);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Explains the present value of the CMS product.
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
		ExplainMapBuilder builder = ExplainMap.builder();
		builder.put(ExplainKey.ENTRY_TYPE, "CmsSwap");
		IList<ExplainMap> legsExplain = new List<ExplainMap>();
		legsExplain.Add(cmsLegPricer.explainPresentValue(cms.CmsLeg, ratesProvider, swaptionVolatilities));
		if (cms.PayLeg.Present)
		{
		  legsExplain.Add(payLegPricer.explainPresentValue(cms.PayLeg.get(), ratesProvider));
		}
		builder.put(ExplainKey.LEGS, legsExplain);
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value curve sensitivity of the CMS product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRates(ResolvedCms cms, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		PointSensitivityBuilder pvSensiCmsLeg = cmsLegPricer.presentValueSensitivityRates(cms.CmsLeg, ratesProvider, swaptionVolatilities);
		if (!cms.PayLeg.Present)
		{
		  return pvSensiCmsLeg;
		}
		PointSensitivityBuilder pvSensiPayLeg = payLegPricer.presentValueSensitivity(cms.PayLeg.get(), ratesProvider);
		return pvSensiCmsLeg.combinedWith(pvSensiPayLeg);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity to the SABR model parameters.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value
	  /// to the SABR model parameters, alpha, beta, rho and nu.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsSabr(ResolvedCms cms, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		return cmsLegPricer.presentValueSensitivityModelParamsSabr(cms.CmsLeg, ratesProvider, swaptionVolatilities);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity to the strike value.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to the strike value.
	  /// This is not relevant for CMS coupons and an exception is thrown in the underlying pricer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual double presentValueSensitivityStrike(ResolvedCms cms, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		return cmsLegPricer.presentValueSensitivityStrike(cms.CmsLeg, ratesProvider, swaptionVolatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the product.
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedCms cms, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		CurrencyAmount ceCmsLeg = cmsLegPricer.presentValue(cms.CmsLeg, ratesProvider, swaptionVolatilities);
		if (!cms.PayLeg.Present)
		{
		  return MultiCurrencyAmount.of(ceCmsLeg);
		}
		MultiCurrencyAmount cePayLeg = payLegPricer.currencyExposure(cms.PayLeg.get(), ratesProvider);
		return cePayLeg.plus(ceCmsLeg);
	  }

	  /// <summary>
	  /// Calculates the current cash of the product.
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the current cash </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedCms cms, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		CurrencyAmount ccCmsLeg = cmsLegPricer.currentCash(cms.CmsLeg, ratesProvider, swaptionVolatilities);
		if (!cms.PayLeg.Present)
		{
		  return MultiCurrencyAmount.of(ccCmsLeg);
		}
		CurrencyAmount ccPayLeg = payLegPricer.currentCash(cms.PayLeg.get(), ratesProvider);
		return MultiCurrencyAmount.of(ccPayLeg).plus(ccCmsLeg);
	  }

	}

}