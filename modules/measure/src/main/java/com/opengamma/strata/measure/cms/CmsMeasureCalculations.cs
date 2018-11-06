/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.cms
{
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using SwaptionScenarioMarketData = com.opengamma.strata.measure.swaption.SwaptionScenarioMarketData;
	using SabrExtrapolationReplicationCmsLegPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsLegPricer;
	using SabrExtrapolationReplicationCmsPeriodPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsPeriodPricer;
	using SabrExtrapolationReplicationCmsProductPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsProductPricer;
	using SabrExtrapolationReplicationCmsTradePricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using SabrSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrSwaptionVolatilities;
	using SwaptionVolatilities = com.opengamma.strata.pricer.swaption.SwaptionVolatilities;
	using ResolvedCmsTrade = com.opengamma.strata.product.cms.ResolvedCmsTrade;

	/// <summary>
	/// Multi-scenario measure calculations for CMS trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class CmsMeasureCalculations
	{

	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedCmsTrade"/>.
	  /// </summary>
	  private readonly SabrExtrapolationReplicationCmsTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="cmsParams">  the CMS parameters </param>
	  internal CmsMeasureCalculations(CmsSabrExtrapolationParams cmsParams)
	  {
		SabrExtrapolationReplicationCmsPeriodPricer periodPricer = SabrExtrapolationReplicationCmsPeriodPricer.of(cmsParams.CutOffStrike, cmsParams.Mu);
		SabrExtrapolationReplicationCmsLegPricer legPricer = new SabrExtrapolationReplicationCmsLegPricer(periodPricer);
		SabrExtrapolationReplicationCmsProductPricer productPricer = new SabrExtrapolationReplicationCmsProductPricer(legPricer);
		SabrExtrapolationReplicationCmsTradePricer tradePricer = new SabrExtrapolationReplicationCmsTradePricer(productPricer);
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer function for <seealso cref="ResolvedCmsTrade"/> </param>
	  internal CmsMeasureCalculations(SabrExtrapolationReplicationCmsTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal MultiCurrencyScenarioArray presentValue(ResolvedCmsTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = cmsLegIborIndex(trade);
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => presentValue(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // present value for one scenario
	  internal MultiCurrencyAmount presentValue(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return tradePricer.presentValue(trade, ratesProvider, checkSabr(volatilities));
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01RatesCalibratedSum(ResolvedCmsTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = cmsLegIborIndex(trade);
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesCalibratedSum(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01RatesCalibratedSum(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		return ratesProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01RatesCalibratedBucketed(ResolvedCmsTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = cmsLegIborIndex(trade);
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesCalibratedBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01RatesCalibratedBucketed(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		return ratesProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01RatesMarketQuoteSum(ResolvedCmsTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = cmsLegIborIndex(trade);
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesMarketQuoteSum(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01RatesMarketQuoteSum(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01RatesMarketQuoteBucketed(ResolvedCmsTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = cmsLegIborIndex(trade);
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesMarketQuoteBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01RatesMarketQuoteBucketed(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  // point sensitivity
	  private PointSensitivities pointSensitivity(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return tradePricer.presentValueSensitivityRates(trade, ratesProvider, checkSabr(volatilities));
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedCmsTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = cmsLegIborIndex(trade);
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => currencyExposure(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return tradePricer.currencyExposure(trade, ratesProvider, checkSabr(volatilities));
	  }

	  //-------------------------------------------------------------------------
	  // calculates current cash for all scenarios
	  internal MultiCurrencyScenarioArray currentCash(ResolvedCmsTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = cmsLegIborIndex(trade);
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => currentCash(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // current cash for one scenario
	  internal MultiCurrencyAmount currentCash(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return tradePricer.currentCash(trade, ratesProvider, checkSabr(volatilities));
	  }

	  //-------------------------------------------------------------------------
	  // returns the Ibor index or the CMS leg
	  internal static IborIndex cmsLegIborIndex(ResolvedCmsTrade trade)
	  {
		return trade.Product.CmsLeg.UnderlyingIndex;
	  }

	  // checks that the volatilities are for SABR
	  private static SabrSwaptionVolatilities checkSabr(SwaptionVolatilities volatilities)
	  {
		if (volatilities is SabrSwaptionVolatilities)
		{
		  return (SabrSwaptionVolatilities) volatilities;
		}
		throw new System.ArgumentException("Swaption volatiliies for pricing CMS must be for SABR model, but was: " + volatilities.VolatilityType);
	  }

	}

}