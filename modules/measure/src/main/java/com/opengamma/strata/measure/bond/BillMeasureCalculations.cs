/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using DiscountingBillTradePricer = com.opengamma.strata.pricer.bond.DiscountingBillTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedBillTrade = com.opengamma.strata.product.bond.ResolvedBillTrade;

	/// <summary>
	/// Multi-scenario measure calculations for bill trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	public class BillMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BillMeasureCalculations DEFAULT = new BillMeasureCalculations(DiscountingBillTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedBullTrade"/>.
	  /// </summary>
	  private readonly DiscountingBillTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedBillTrade"/> </param>
	  internal BillMeasureCalculations(DiscountingBillTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal virtual CurrencyScenarioArray presentValue(ResolvedBillTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => presentValue(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // present value for one scenario
	  internal virtual CurrencyAmount presentValue(ResolvedBillTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		return tradePricer.presentValue(trade, discountingProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal virtual MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedBillTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedSum(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // calibrated sum PV01 for one scenario
	  internal virtual MultiCurrencyAmount pv01CalibratedSum(ResolvedBillTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, discountingProvider);
		return discountingProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal virtual ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedBillTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedBucketed(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal virtual CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedBillTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, discountingProvider);
		return discountingProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal virtual MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedBillTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteSum(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // market quote sum PV01 for one scenario
	  internal virtual MultiCurrencyAmount pv01MarketQuoteSum(ResolvedBillTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal virtual ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedBillTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteBucketed(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal virtual CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedBillTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal virtual MultiCurrencyScenarioArray currencyExposure(ResolvedBillTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => currencyExposure(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // currency exposure for one scenario
	  internal virtual MultiCurrencyAmount currencyExposure(ResolvedBillTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		return tradePricer.currencyExposure(trade, discountingProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates current cash for all scenarios
	  internal virtual CurrencyScenarioArray currentCash(ResolvedBillTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => currentCash(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // current cash for one scenario
	  internal virtual CurrencyAmount currentCash(ResolvedBillTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		return tradePricer.currentCash(trade, discountingProvider.ValuationDate);
	  }

	}

}