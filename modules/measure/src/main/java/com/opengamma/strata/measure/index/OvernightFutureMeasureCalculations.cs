/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.index
{
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using DiscountingOvernightFutureTradePricer = com.opengamma.strata.pricer.index.DiscountingOvernightFutureTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedOvernightFutureTrade = com.opengamma.strata.product.index.ResolvedOvernightFutureTrade;

	/// <summary>
	/// Multi-scenario measure calculations for Overnight rate Future trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class OvernightFutureMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly OvernightFutureMeasureCalculations DEFAULT = new OvernightFutureMeasureCalculations(DiscountingOvernightFutureTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedOvernightFutureTrade"/>.
	  /// </summary>
	  private readonly DiscountingOvernightFutureTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedOvernightFutureTrade"/> </param>
	  internal OvernightFutureMeasureCalculations(DiscountingOvernightFutureTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal CurrencyScenarioArray presentValue(ResolvedOvernightFutureTrade trade, RatesScenarioMarketData marketData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => presentValue(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // present value for one scenario
	  internal CurrencyAmount presentValue(ResolvedOvernightFutureTrade trade, RatesProvider ratesProvider)
	  {

		// mark to model
		double settlementPrice = this.settlementPrice(trade, ratesProvider);
		return tradePricer.presentValue(trade, ratesProvider, settlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedOvernightFutureTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedSum(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01CalibratedSum(ResolvedOvernightFutureTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		return ratesProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedOvernightFutureTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedBucketed(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedOvernightFutureTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		return ratesProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedOvernightFutureTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteSum(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01MarketQuoteSum(ResolvedOvernightFutureTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedOvernightFutureTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteBucketed(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedOvernightFutureTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates par spread for all scenarios
	  internal DoubleScenarioArray parSpread(ResolvedOvernightFutureTrade trade, RatesScenarioMarketData marketData)
	  {

		return DoubleScenarioArray.of(marketData.ScenarioCount, i => parSpread(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // par spread for one scenario
	  internal double parSpread(ResolvedOvernightFutureTrade trade, RatesProvider ratesProvider)
	  {

		double settlementPrice = this.settlementPrice(trade, ratesProvider);
		return tradePricer.parSpread(trade, ratesProvider, settlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  // calculates unit price for all scenarios
	  internal DoubleScenarioArray unitPrice(ResolvedOvernightFutureTrade trade, RatesScenarioMarketData marketData)
	  {

		return DoubleScenarioArray.of(marketData.ScenarioCount, i => unitPrice(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // unit price for one scenario
	  internal double unitPrice(ResolvedOvernightFutureTrade trade, RatesProvider ratesProvider)
	  {

		// mark to model
		return tradePricer.price(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // gets the settlement price
	  private double settlementPrice(ResolvedOvernightFutureTrade trade, RatesProvider ratesProvider)
	  {
		StandardId standardId = trade.Product.SecurityId.StandardId;
		QuoteId id = QuoteId.of(standardId, FieldName.SETTLEMENT_PRICE);
		return ratesProvider.data(id) / 100; // convert market quote to value needed
	  }

	}

}