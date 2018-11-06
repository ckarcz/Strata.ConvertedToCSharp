/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.payment
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using CrossGammaParameterSensitivities = com.opengamma.strata.market.param.CrossGammaParameterSensitivities;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using DiscountingBulletPaymentTradePricer = com.opengamma.strata.pricer.payment.DiscountingBulletPaymentTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using CurveGammaCalculator = com.opengamma.strata.pricer.sensitivity.CurveGammaCalculator;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedBulletPaymentTrade = com.opengamma.strata.product.payment.ResolvedBulletPaymentTrade;

	/// <summary>
	/// Multi-scenario measure calculations for Bullet Payment trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class BulletPaymentMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BulletPaymentMeasureCalculations DEFAULT = new BulletPaymentMeasureCalculations(DiscountingBulletPaymentTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// The cross gamma sensitivity calculator.
	  /// </summary>
	  private static readonly CurveGammaCalculator CROSS_GAMMA = CurveGammaCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedBulletPaymentTrade"/>.
	  /// </summary>
	  private readonly DiscountingBulletPaymentTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedBulletPaymentTrade"/> </param>
	  internal BulletPaymentMeasureCalculations(DiscountingBulletPaymentTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal CurrencyScenarioArray presentValue(ResolvedBulletPaymentTrade trade, RatesScenarioMarketData marketData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => presentValue(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // present value for one scenario
	  internal CurrencyAmount presentValue(ResolvedBulletPaymentTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.presentValue(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates explain present value for all scenarios
	  internal ScenarioArray<ExplainMap> explainPresentValue(ResolvedBulletPaymentTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => explainPresentValue(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // explain present value for one scenario
	  internal ExplainMap explainPresentValue(ResolvedBulletPaymentTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.explainPresentValue(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedBulletPaymentTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedSum(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01CalibratedSum(ResolvedBulletPaymentTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		return ratesProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedBulletPaymentTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedBucketed(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedBulletPaymentTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		return ratesProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedBulletPaymentTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteSum(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01MarketQuoteSum(ResolvedBulletPaymentTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedBulletPaymentTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteBucketed(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedBulletPaymentTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates single-node gamma PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01SingleNodeGammaBucketed(ResolvedBulletPaymentTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01SingleNodeGammaBucketed(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // single-node gamma PV01 for one scenario
	  private CurrencyParameterSensitivities pv01SingleNodeGammaBucketed(ResolvedBulletPaymentTrade trade, RatesProvider ratesProvider)
	  {

		CrossGammaParameterSensitivities crossGamma = CROSS_GAMMA.calculateCrossGammaIntraCurve(ratesProvider, p => p.parameterSensitivity(tradePricer.presentValueSensitivity(trade, p)));
		return crossGamma.diagonal().multipliedBy(ONE_BASIS_POINT * ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates cash flows for all scenarios
	  internal ScenarioArray<CashFlows> cashFlows(ResolvedBulletPaymentTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => cashFlows(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // cash flows for one scenario
	  internal CashFlows cashFlows(ResolvedBulletPaymentTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.cashFlows(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedBulletPaymentTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => currencyExposure(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedBulletPaymentTrade trade, RatesProvider ratesProvider)
	  {

		return MultiCurrencyAmount.of(tradePricer.currencyExposure(trade, ratesProvider));
	  }

	  //-------------------------------------------------------------------------
	  // calculates current cash for all scenarios
	  internal CurrencyScenarioArray currentCash(ResolvedBulletPaymentTrade trade, RatesScenarioMarketData marketData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => currentCash(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // current cash for one scenario
	  internal CurrencyAmount currentCash(ResolvedBulletPaymentTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.currentCash(trade, ratesProvider);
	  }

	}

}