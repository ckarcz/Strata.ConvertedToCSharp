/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.cms
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DiscountingCmsPeriodPricer = com.opengamma.strata.pricer.impl.cms.DiscountingCmsPeriodPricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using CmsPeriod = com.opengamma.strata.product.cms.CmsPeriod;
	using ResolvedCmsLeg = com.opengamma.strata.product.cms.ResolvedCmsLeg;

	/// <summary>
	/// Pricer for CMS legs by simple forward estimation.
	/// <para>
	/// This is an overly simplistic approach to CMS coupon pricer. It is provided only for testing and comparison 
	/// purposes. It is not recommended to use this for valuation or risk management purposes.
	/// </para>
	/// <para>
	/// This function provides the ability to price <seealso cref="ResolvedCmsLeg"/>. 
	/// One must apply {@code resolved()} in order to price <seealso cref="CmsLeg"/>. 
	/// </para>
	/// </summary>
	public class DiscountingCmsLegPricer
	{

	  /// <summary>
	  /// The pricer for <seealso cref="CmsPeriod"/>.
	  /// </summary>
	  private readonly DiscountingCmsPeriodPricer cmsPeriodPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="cmsPeriodPricer">  the pricer for <seealso cref="CmsPeriod"/> </param>
	  public DiscountingCmsLegPricer(DiscountingCmsPeriodPricer cmsPeriodPricer)
	  {
		this.cmsPeriodPricer = ArgChecker.notNull(cmsPeriodPricer, "cmsPeriodPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value of CMS leg by simple forward rate estimation.
	  /// <para>
	  /// The result is returned using the payment currency of the leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedCmsLeg cmsLeg, RatesProvider ratesProvider)
	  {

		return cmsLeg.CmsPeriods.Select(cmsPeriod => cmsPeriodPricer.presentValue(cmsPeriod, ratesProvider)).Aggregate((c1, c2) => c1.plus(c2)).get();
	  }

	  /// <summary>
	  /// Calculates the present value curve sensitivity of the CMS leg by simple forward rate estimation.
	  /// <para>
	  /// The present value sensitivity of the leg is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(ResolvedCmsLeg cmsLeg, RatesProvider ratesProvider)
	  {

		return cmsLeg.CmsPeriods.Select(cmsPeriod => cmsPeriodPricer.presentValueSensitivity(cmsPeriod, ratesProvider)).Aggregate((p1, p2) => p1.combinedWith(p2)).get();
	  }

	  /// <summary>
	  /// Calculates the current cash of the leg.
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(ResolvedCmsLeg cmsLeg, RatesProvider ratesProvider)
	  {

		return cmsLeg.CmsPeriods.Where(x => x.PaymentDate.Equals(ratesProvider.ValuationDate)).Select(x => cmsPeriodPricer.presentValue(x, ratesProvider)).Aggregate((c1, c2) => c1.plus(c2)).orElse(CurrencyAmount.zero(cmsLeg.Currency));
	  }

	}

}