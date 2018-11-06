/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using DiscountingCapitalIndexedBondTradePricer = com.opengamma.strata.pricer.bond.DiscountingCapitalIndexedBondTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedCapitalIndexedBondTrade = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBondTrade;

	/// <summary>
	/// Multi-scenario measure calculations for capital indexed bond trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class CapitalIndexedBondMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly CapitalIndexedBondMeasureCalculations DEFAULT = new CapitalIndexedBondMeasureCalculations(DiscountingCapitalIndexedBondTradePricer.DEFAULT);
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedCapitalIndexedBondTrade"/>.
	  /// </summary>
	  private readonly DiscountingCapitalIndexedBondTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedCapitalIndexedBondTrade"/> </param>
	  internal CapitalIndexedBondMeasureCalculations(DiscountingCapitalIndexedBondTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal CurrencyScenarioArray presentValue(ResolvedCapitalIndexedBondTrade trade, RatesScenarioMarketData ratesMarketData, LegalEntityDiscountingScenarioMarketData legalEntityMarketData)
	  {

		return CurrencyScenarioArray.of(legalEntityMarketData.ScenarioCount, i => presentValue(trade, ratesMarketData.scenario(i).ratesProvider(), legalEntityMarketData.scenario(i).discountingProvider()));
	  }

	  // present value for one scenario
	  internal CurrencyAmount presentValue(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		return tradePricer.presentValue(trade, ratesProvider, discountingProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedCapitalIndexedBondTrade trade, RatesScenarioMarketData ratesMarketData, LegalEntityDiscountingScenarioMarketData legalEntityMarketData)
	  {

		return MultiCurrencyScenarioArray.of(legalEntityMarketData.ScenarioCount, i => pv01CalibratedSum(trade, ratesMarketData.scenario(i).ratesProvider(), legalEntityMarketData.scenario(i).discountingProvider()));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01CalibratedSum(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider, discountingProvider);
		return discountingProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedCapitalIndexedBondTrade trade, RatesScenarioMarketData ratesMarketData, LegalEntityDiscountingScenarioMarketData legalEntityMarketData)
	  {

		return ScenarioArray.of(legalEntityMarketData.ScenarioCount, i => pv01CalibratedBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), legalEntityMarketData.scenario(i).discountingProvider()));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider, discountingProvider);
		return discountingProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedCapitalIndexedBondTrade trade, RatesScenarioMarketData ratesMarketData, LegalEntityDiscountingScenarioMarketData legalEntityMarketData)
	  {

		return MultiCurrencyScenarioArray.of(legalEntityMarketData.ScenarioCount, i => currencyExposure(trade, ratesMarketData.scenario(i).ratesProvider(), legalEntityMarketData.scenario(i).discountingProvider()));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		return tradePricer.currencyExposure(trade, ratesProvider, discountingProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates current cash for all scenarios
	  internal CurrencyScenarioArray currentCash(ResolvedCapitalIndexedBondTrade trade, RatesScenarioMarketData ratesMarketData, LegalEntityDiscountingScenarioMarketData legalEntityMarketData)
	  {

		return CurrencyScenarioArray.of(legalEntityMarketData.ScenarioCount, i => currentCash(trade, ratesMarketData.scenario(i).ratesProvider()));
	  }

	  // current cash for one scenario
	  internal CurrencyAmount currentCash(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.currentCash(trade, ratesProvider);
	  }

	}

}