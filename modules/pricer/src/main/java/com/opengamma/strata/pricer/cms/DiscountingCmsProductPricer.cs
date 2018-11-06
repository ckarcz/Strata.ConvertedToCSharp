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
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DiscountingCmsPeriodPricer = com.opengamma.strata.pricer.impl.cms.DiscountingCmsPeriodPricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using ResolvedCms = com.opengamma.strata.product.cms.ResolvedCms;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	///  Computes the price of a CMS product by simple forward estimation.
	///  <para>
	///  This is an overly simplistic approach to CMS coupon pricer. It is provided only for testing and comparison 
	///  purposes. It is not recommended to use this for valuation or risk management purposes.
	/// </para>
	/// </summary>
	public class DiscountingCmsProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingCmsProductPricer DEFAULT = new DiscountingCmsProductPricer(DiscountingSwapProductPricer.DEFAULT);

	  /// <summary>
	  /// The pricer for <seealso cref="SwapLeg"/>. </summary>
	  private readonly DiscountingSwapProductPricer swapPricer;
	  /// <summary>
	  /// The pricer for <seealso cref="CmsLeg"/>. </summary>
	  private readonly DiscountingCmsLegPricer cmsLegPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="swapPricer">  the pricer for <seealso cref="Swap"/> </param>
	  public DiscountingCmsProductPricer(DiscountingSwapProductPricer swapPricer)
	  {
		this.swapPricer = ArgChecker.notNull(swapPricer, "swapPricer");
		this.cmsLegPricer = new DiscountingCmsLegPricer(new DiscountingCmsPeriodPricer(swapPricer));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the CMS product by simple forward estimation.
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the present value </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedCms cms, RatesProvider ratesProvider)
	  {

		CurrencyAmount pvCmsLeg = cmsLegPricer.presentValue(cms.CmsLeg, ratesProvider);
		if (!cms.PayLeg.Present)
		{
		  return MultiCurrencyAmount.of(pvCmsLeg);
		}
		CurrencyAmount pvPayLeg = swapPricer.LegPricer.presentValue(cms.PayLeg.get(), ratesProvider);
		return MultiCurrencyAmount.of(pvCmsLeg).plus(pvPayLeg);
	  }

	  /// <summary>
	  /// Calculates the present value curve sensitivity of the CMS product by simple forward estimation.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(ResolvedCms cms, RatesProvider ratesProvider)
	  {

		PointSensitivityBuilder pvSensiCmsLeg = cmsLegPricer.presentValueSensitivity(cms.CmsLeg, ratesProvider);
		if (!cms.PayLeg.Present)
		{
		  return pvSensiCmsLeg;
		}
		PointSensitivityBuilder pvSensiPayLeg = swapPricer.LegPricer.presentValueSensitivity(cms.PayLeg.get(), ratesProvider);
		return pvSensiCmsLeg.combinedWith(pvSensiPayLeg);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the product.
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedCms cms, RatesProvider ratesProvider)
	  {

		return presentValue(cms, ratesProvider);
	  }

	  /// <summary>
	  /// Calculates the current cash of the product.
	  /// </summary>
	  /// <param name="cms">  the CMS product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the current cash </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedCms cms, RatesProvider ratesProvider)
	  {

		CurrencyAmount ccCmsLeg = cmsLegPricer.currentCash(cms.CmsLeg, ratesProvider);
		if (!cms.PayLeg.Present)
		{
		  return MultiCurrencyAmount.of(ccCmsLeg);
		}
		CurrencyAmount ccPayLeg = swapPricer.LegPricer.currentCash(cms.PayLeg.get(), ratesProvider);
		return MultiCurrencyAmount.of(ccPayLeg).plus(ccCmsLeg);
	  }

	}

}