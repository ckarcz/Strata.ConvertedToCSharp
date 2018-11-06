/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.cms
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using SwaptionMarketDataLookup = com.opengamma.strata.measure.swaption.SwaptionMarketDataLookup;
	using SabrExtrapolationReplicationCmsLegPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsLegPricer;
	using SabrExtrapolationReplicationCmsPeriodPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsPeriodPricer;
	using SabrExtrapolationReplicationCmsProductPricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsProductPricer;
	using SabrExtrapolationReplicationCmsTradePricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SabrSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrSwaptionVolatilities;
	using ResolvedCmsTrade = com.opengamma.strata.product.cms.ResolvedCmsTrade;

	/// <summary>
	/// Test <seealso cref="CmsTradeCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CmsTradeCalculationsTest
	public class CmsTradeCalculationsTest
	{

	  private static readonly ResolvedCmsTrade RTRADE = CmsTradeCalculationFunctionTest.RTRADE;
	  private static readonly RatesMarketDataLookup RATES_LOOKUP = CmsTradeCalculationFunctionTest.RATES_LOOKUP;
	  private static readonly SwaptionMarketDataLookup SWAPTION_LOOKUP = CmsTradeCalculationFunctionTest.SWAPTION_LOOKUP;
	  private static readonly CmsSabrExtrapolationParams CMS_MODEL = CmsTradeCalculationFunctionTest.CMS_MODEL;
	  private static readonly SabrSwaptionVolatilities VOLS = CmsTradeCalculationFunctionTest.VOLS;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		ScenarioMarketData md = CmsTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		SabrExtrapolationReplicationCmsTradePricer pricer = new SabrExtrapolationReplicationCmsTradePricer(new SabrExtrapolationReplicationCmsProductPricer(new SabrExtrapolationReplicationCmsLegPricer(SabrExtrapolationReplicationCmsPeriodPricer.of(CMS_MODEL.CutOffStrike, CMS_MODEL.Mu))));
		MultiCurrencyAmount expectedPv = pricer.presentValue(RTRADE, provider, VOLS);
		MultiCurrencyAmount expectedCurrencyExposure = pricer.currencyExposure(RTRADE, provider, VOLS);
		MultiCurrencyAmount expectedCurrentCash = pricer.currentCash(RTRADE, provider, VOLS);

		CmsTradeCalculations calcs = CmsTradeCalculations.of(CMS_MODEL);
		assertEquals(calcs.presentValue(RTRADE, RATES_LOOKUP, SWAPTION_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv)));
		assertEquals(calcs.currencyExposure(RTRADE, RATES_LOOKUP, SWAPTION_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrencyExposure)));
		assertEquals(calcs.currentCash(RTRADE, RATES_LOOKUP, SWAPTION_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedCurrentCash)));
	  }

	  public virtual void test_pv01()
	  {
		ScenarioMarketData md = CmsTradeCalculationFunctionTest.marketData();
		RatesProvider provider = RATES_LOOKUP.marketDataView(md.scenario(0)).ratesProvider();
		SabrExtrapolationReplicationCmsTradePricer pricer = new SabrExtrapolationReplicationCmsTradePricer(new SabrExtrapolationReplicationCmsProductPricer(new SabrExtrapolationReplicationCmsLegPricer(SabrExtrapolationReplicationCmsPeriodPricer.of(CMS_MODEL.CutOffStrike, CMS_MODEL.Mu))));
		PointSensitivities pvPointSens = pricer.presentValueSensitivityRates(RTRADE, provider, VOLS);
		CurrencyParameterSensitivities pvParamSens = provider.parameterSensitivity(pvPointSens);
		MultiCurrencyAmount expectedPv01Cal = pvParamSens.total().multipliedBy(1e-4);
		CurrencyParameterSensitivities expectedPv01CalBucketed = pvParamSens.multipliedBy(1e-4);

		CmsTradeCalculations calcs = CmsTradeCalculations.of(CMS_MODEL);
		assertEquals(calcs.pv01RatesCalibratedSum(RTRADE, RATES_LOOKUP, SWAPTION_LOOKUP, md), MultiCurrencyScenarioArray.of(ImmutableList.of(expectedPv01Cal)));
		assertEquals(calcs.pv01RatesCalibratedBucketed(RTRADE, RATES_LOOKUP, SWAPTION_LOOKUP, md), ScenarioArray.of(ImmutableList.of(expectedPv01CalBucketed)));
	  }

	}

}