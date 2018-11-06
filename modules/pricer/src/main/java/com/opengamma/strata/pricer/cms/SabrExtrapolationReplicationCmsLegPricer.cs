/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.cms
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SabrSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrSwaptionVolatilities;
	using CmsLeg = com.opengamma.strata.product.cms.CmsLeg;
	using CmsPeriod = com.opengamma.strata.product.cms.CmsPeriod;
	using ResolvedCmsLeg = com.opengamma.strata.product.cms.ResolvedCmsLeg;

	/// <summary>
	/// Pricer for CMS legs by swaption replication on a SABR formula with extrapolation.
	/// <para>
	/// This function provides the ability to price <seealso cref="ResolvedCmsLeg"/>. 
	/// One must apply {@code resolved()} in order to price <seealso cref="CmsLeg"/>. 
	/// </para>
	/// </summary>
	public class SabrExtrapolationReplicationCmsLegPricer
	{

	  /// <summary>
	  /// The pricer for <seealso cref="CmsPeriod"/>.
	  /// </summary>
	  private readonly SabrExtrapolationReplicationCmsPeriodPricer cmsPeriodPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="cmsPeriodPricer">  the pricer for <seealso cref="CmsPeriod"/> </param>
	  public SabrExtrapolationReplicationCmsLegPricer(SabrExtrapolationReplicationCmsPeriodPricer cmsPeriodPricer)
	  {
		this.cmsPeriodPricer = ArgChecker.notNull(cmsPeriodPricer, "cmsPeriodPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the CMS leg.
	  /// <para>
	  /// The present value of the leg is the value on the valuation date.
	  /// The result is returned using the payment currency of the leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedCmsLeg cmsLeg, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		validate(ratesProvider, swaptionVolatilities);
		return cmsLeg.CmsPeriods.Select(cmsPeriod => cmsPeriodPricer.presentValue(cmsPeriod, ratesProvider, swaptionVolatilities)).Aggregate((c1, c2) => c1.plus(c2)).get();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Explains the present value of a CMS leg.
	  /// <para>
	  /// This returns explanatory information about the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="volatilities">  the swaption volatilities </param>
	  /// <returns> the explanatory information </returns>
	  public virtual ExplainMap explainPresentValue(ResolvedCmsLeg cmsLeg, RatesProvider provider, SabrSwaptionVolatilities volatilities)
	  {

		ExplainMapBuilder builder = ExplainMap.builder();
		builder.put(ExplainKey.ENTRY_TYPE, "CmsLeg");
		builder.put(ExplainKey.PAY_RECEIVE, cmsLeg.PayReceive);
		builder.put(ExplainKey.PAYMENT_CURRENCY, cmsLeg.Currency);
		builder.put(ExplainKey.START_DATE, cmsLeg.StartDate);
		builder.put(ExplainKey.END_DATE, cmsLeg.EndDate);
		builder.put(ExplainKey.INDEX, cmsLeg.Index);
		foreach (CmsPeriod period in cmsLeg.CmsPeriods)
		{
		  builder.addListEntry(ExplainKey.PAYMENT_PERIODS, child => cmsPeriodPricer.explainPresentValue(period, provider, volatilities, child));
		}
		builder.put(ExplainKey.PRESENT_VALUE, presentValue(cmsLeg, provider, volatilities));
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value curve sensitivity of the CMS leg.
	  /// <para>
	  /// The present value sensitivity of the leg is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRates(ResolvedCmsLeg cmsLeg, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		validate(ratesProvider, swaptionVolatilities);
		return cmsLeg.CmsPeriods.Select(cmsPeriod => cmsPeriodPricer.presentValueSensitivityRates(cmsPeriod, ratesProvider, swaptionVolatilities)).Aggregate((p1, p2) => p1.combinedWith(p2)).get();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity to the SABR model parameters.
	  /// <para>
	  /// The present value sensitivity of the leg is the sensitivity of the present value to
	  /// the SABR model parameters, alpha, beta, rho and nu.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsSabr(ResolvedCmsLeg cmsLeg, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		validate(ratesProvider, swaptionVolatilities);
		return cmsLeg.CmsPeriods.Select(cmsPeriod => cmsPeriodPricer.presentValueSensitivityModelParamsSabr(cmsPeriod, ratesProvider, swaptionVolatilities)).Aggregate(PointSensitivityBuilder.none(), PointSensitivityBuilder.combinedWith).normalize();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity to the strike value.
	  /// <para>
	  /// The present value sensitivity of the leg is the sensitivity of the present value to the strike value.
	  /// This is not relevant for CMS coupons and an exception is thrown in the underlying pricer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual double presentValueSensitivityStrike(ResolvedCmsLeg cmsLeg, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		validate(ratesProvider, swaptionVolatilities);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return cmsLeg.CmsPeriods.Select(cmsPeriod => cmsPeriodPricer.presentValueSensitivityStrike(cmsPeriod, ratesProvider, swaptionVolatilities)).collect(Collectors.summingDouble(double?.doubleValue));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the current cash of the leg.
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(ResolvedCmsLeg cmsLeg, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		validate(ratesProvider, swaptionVolatilities);
		return cmsLeg.CmsPeriods.Where(x => x.PaymentDate.Equals(ratesProvider.ValuationDate)).Select(x => cmsPeriodPricer.presentValue(x, ratesProvider, swaptionVolatilities)).Aggregate((c1, c2) => c1.plus(c2)).orElse(CurrencyAmount.zero(cmsLeg.Currency));
	  }

	  //-------------------------------------------------------------------------
	  private void validate(RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {
		ArgChecker.isTrue(swaptionVolatilities.ValuationDate.Equals(ratesProvider.ValuationDate), "volatility and rate data must be for the same date");
	  }

	}

}