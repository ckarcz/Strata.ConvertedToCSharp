/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.credit
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using AccrualOnDefaultFormula = com.opengamma.strata.pricer.credit.AccrualOnDefaultFormula;
	using AnalyticSpreadSensitivityCalculator = com.opengamma.strata.pricer.credit.AnalyticSpreadSensitivityCalculator;
	using CdsMarketQuoteConverter = com.opengamma.strata.pricer.credit.CdsMarketQuoteConverter;
	using CreditRatesProvider = com.opengamma.strata.pricer.credit.CreditRatesProvider;
	using IsdaCdsTradePricer = com.opengamma.strata.pricer.credit.IsdaCdsTradePricer;
	using JumpToDefault = com.opengamma.strata.pricer.credit.JumpToDefault;
	using SpreadSensitivityCalculator = com.opengamma.strata.pricer.credit.SpreadSensitivityCalculator;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedCdsTrade = com.opengamma.strata.product.credit.ResolvedCdsTrade;

	/// <summary>
	/// Multi-scenario measure calculations for CDS trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class CdsMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  internal static readonly CdsMeasureCalculations DEFAULT = new CdsMeasureCalculations(new IsdaCdsTradePricer(AccrualOnDefaultFormula.CORRECT));

	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedCdsTrade"/>.
	  /// </summary>
	  private readonly IsdaCdsTradePricer tradePricer;
	  /// <summary>
	  /// Spread sensitivity calculator.
	  /// </summary>
	  private readonly SpreadSensitivityCalculator cs01Calculator;
	  /// <summary>
	  /// Market quote converter.
	  /// </summary>
	  private readonly CdsMarketQuoteConverter converter;

	  /// <summary>
	  /// Creates an instance. 
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedCdsTrade"/> </param>
	  internal CdsMeasureCalculations(IsdaCdsTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
		this.cs01Calculator = new AnalyticSpreadSensitivityCalculator(tradePricer.AccrualOnDefaultFormula);
		this.converter = new CdsMarketQuoteConverter(tradePricer.AccrualOnDefaultFormula);
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal CurrencyScenarioArray presentValue(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => presentValue(trade, marketData.scenario(i).creditRatesProvider(), PriceType.DIRTY, refData));
	  }

	  // calculates present value for one scenario
	  internal CurrencyAmount presentValue(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, PriceType priceType, ReferenceData refData)
	  {

		return tradePricer.presentValue(trade, ratesProvider, priceType, refData);
	  }

	  //-------------------------------------------------------------------------
	  // calculates principal for all scenarios
	  internal CurrencyScenarioArray principal(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => principal(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates principal for one scenario
	  internal CurrencyAmount principal(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		return tradePricer.presentValueOnSettle(trade, ratesProvider, PriceType.CLEAN, refData);
	  }

	  //-------------------------------------------------------------------------
	  // calculates price for all scenarios
	  internal DoubleScenarioArray unitPrice(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return DoubleScenarioArray.of(marketData.ScenarioCount, i => unitPrice(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates price for one scenario
	  internal double unitPrice(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		double puf = tradePricer.price(trade, ratesProvider, PriceType.CLEAN, refData);
		return converter.cleanPriceFromPointsUpfront(puf);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated parallel IR01 for all scenarios
	  internal CurrencyScenarioArray ir01CalibratedParallel(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => ir01CalibratedParallel(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates calibrated parallel IR01 for one scenario
	  internal CurrencyAmount ir01CalibratedParallel(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueOnSettleSensitivity(trade, ratesProvider, refData);
		CurrencyParameterSensitivity irSensitivity = ratesProvider.singleDiscountCurveParameterSensitivity(pointSensitivity, trade.Product.Currency);
		return irSensitivity.total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed IR01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivity> ir01CalibratedBucketed(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => ir01CalibratedBucketed(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates calibrated bucketed IR01 for one scenario
	  internal CurrencyParameterSensitivity ir01CalibratedBucketed(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueOnSettleSensitivity(trade, ratesProvider, refData);
		CurrencyParameterSensitivity irSensitivity = ratesProvider.singleDiscountCurveParameterSensitivity(pointSensitivity, trade.Product.Currency);
		return irSensitivity.multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote parallel IR01 for all scenarios
	  internal MultiCurrencyScenarioArray ir01MarketQuoteParallel(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => ir01MarketQuoteParallel(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates market quote parallel IR01 for one scenario
	  internal MultiCurrencyAmount ir01MarketQuoteParallel(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueOnSettleSensitivity(trade, ratesProvider, refData);
		CurrencyParameterSensitivity parameterSensitivity = ratesProvider.singleDiscountCurveParameterSensitivity(pointSensitivity, trade.Product.Currency);
		CurrencyParameterSensitivities irSensitivity = MARKET_QUOTE_SENS.sensitivity(CurrencyParameterSensitivities.of(parameterSensitivity), ratesProvider);
		return irSensitivity.total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed IR01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> ir01MarketQuoteBucketed(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => ir01MarketQuoteBucketed(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates market quote bucketed IR01 for one scenario
	  internal CurrencyParameterSensitivities ir01MarketQuoteBucketed(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueOnSettleSensitivity(trade, ratesProvider, refData);
		CurrencyParameterSensitivity parameterSensitivity = ratesProvider.singleDiscountCurveParameterSensitivity(pointSensitivity, trade.Product.Currency);
		CurrencyParameterSensitivities irSensitivity = MARKET_QUOTE_SENS.sensitivity(CurrencyParameterSensitivities.of(parameterSensitivity), ratesProvider);
		return irSensitivity.multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedSum(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01CalibratedSum(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider, refData);
		return ratesProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedBucketed(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider, refData);
		return ratesProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteSum(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01MarketQuoteSum(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider, refData);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		CurrencyParameterSensitivities quoteSensitivity = MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider);
		return quoteSensitivity.total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteBucketed(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider, refData);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates parallel CS01 for all scenarios
	  internal CurrencyScenarioArray cs01Parallel(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => cs01Parallel(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates parallel CS01 for one scenario
	  internal CurrencyAmount cs01Parallel(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		return cs01Calculator.parallelCs01(trade, ratesProvider, refData);
	  }

	  //-------------------------------------------------------------------------
	  // calculates bucketed CS01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivity> cs01Bucketed(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => cs01Bucketed(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates bucketed CS01 for one scenario
	  internal CurrencyParameterSensitivity cs01Bucketed(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		return cs01Calculator.bucketedCs01(trade, ratesProvider, refData);
	  }

	  //-------------------------------------------------------------------------
	  // calculates recovery01 for all scenarios
	  internal CurrencyScenarioArray recovery01(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => recovery01(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates recovery01 for one scenario
	  internal CurrencyAmount recovery01(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		return tradePricer.recovery01OnSettle(trade, ratesProvider, refData);
	  }

	  //-------------------------------------------------------------------------
	  // calculates jump-to-default for all scenarios
	  internal ScenarioArray<JumpToDefault> jumpToDefault(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => jumpToDefault(trade, marketData.scenario(i).creditRatesProvider(), refData));
	  }

	  // calculates jump-to-default for one scenario
	  internal JumpToDefault jumpToDefault(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		return tradePricer.jumpToDefault(trade, ratesProvider, refData);
	  }

	  //-------------------------------------------------------------------------
	  // calculates expected loss for all scenarios
	  internal CurrencyScenarioArray expectedLoss(ResolvedCdsTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => expectedLoss(trade, marketData.scenario(i).creditRatesProvider()));
	  }

	  // calculates expected loss for one scenario
	  internal CurrencyAmount expectedLoss(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider)
	  {

		return tradePricer.expectedLoss(trade, ratesProvider);
	  }

	}

}