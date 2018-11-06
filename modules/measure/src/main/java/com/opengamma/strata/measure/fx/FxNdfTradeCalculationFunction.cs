using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fx
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationFunction = com.opengamma.strata.calc.runner.CalculationFunction;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using FxNdf = com.opengamma.strata.product.fx.FxNdf;
	using FxNdfTrade = com.opengamma.strata.product.fx.FxNdfTrade;
	using ResolvedFxNdfTrade = com.opengamma.strata.product.fx.ResolvedFxNdfTrade;

	/// <summary>
	/// Perform calculations on a single {@code FxNdfTrade} for each of a set of scenarios.
	/// <para>
	/// This uses the standard discounting calculation method.
	/// An instance of <seealso cref="RatesMarketDataLookup"/> must be specified.
	/// The supported built-in measures are:
	/// <ul>
	///   <li><seealso cref="Measures#PRESENT_VALUE Present value"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_SUM PV01 calibrated sum"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_BUCKETED PV01 calibrated bucketed"/>
	///   <li><seealso cref="Measures#PV01_MARKET_QUOTE_SUM PV01 market quote sum"/>
	///   <li><seealso cref="Measures#PV01_MARKET_QUOTE_BUCKETED PV01 market quote bucketed"/>
	///   <li><seealso cref="Measures#CURRENCY_EXPOSURE Currency exposure"/>
	///   <li><seealso cref="Measures#CURRENT_CASH Current cash"/>
	///   <li><seealso cref="Measures#RESOLVED_TARGET Resolved trade"/>
	///   <li><seealso cref="Measures#FORWARD_FX_RATE Forward FX rate"/>
	/// </ul>
	/// </para>
	/// <para>
	/// The "natural" currency is the settlement currency of the trade.
	/// </para>
	/// </summary>
	public class FxNdfTradeCalculationFunction : CalculationFunction<FxNdfTrade>
	{

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, FxNdfMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, FxNdfMeasureCalculations.DEFAULT.pv01CalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, FxNdfMeasureCalculations.DEFAULT.pv01CalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, FxNdfMeasureCalculations.DEFAULT.pv01MarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, FxNdfMeasureCalculations.DEFAULT.pv01MarketQuoteBucketed).put(Measures.CURRENCY_EXPOSURE, FxNdfMeasureCalculations.DEFAULT.currencyExposure).put(Measures.CURRENT_CASH, FxNdfMeasureCalculations.DEFAULT.currentCash).put(Measures.FORWARD_FX_RATE, FxNdfMeasureCalculations.DEFAULT.forwardFxRate).put(Measures.RESOLVED_TARGET, (rt, smd) => rt).build();

	  private static readonly ImmutableSet<Measure> MEASURES = CALCULATORS.Keys;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public FxNdfTradeCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual Type<FxNdfTrade> targetType()
	  {
		return typeof(FxNdfTrade);
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return MEASURES;
	  }

	  public override Optional<string> identifier(FxNdfTrade target)
	  {
		return target.Info.Id.map(id => id.ToString());
	  }

	  public virtual Currency naturalCurrency(FxNdfTrade trade, ReferenceData refData)
	  {
		return trade.Product.SettlementCurrency;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(FxNdfTrade trade, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		// extract data from product
		FxNdf fx = trade.Product;
		Currency settleCurrency = fx.SettlementCurrency;
		Currency otherCurrency = fx.NonDeliverableCurrency;
		ImmutableSet<Currency> currencies = ImmutableSet.of(settleCurrency, otherCurrency);

		// use lookup to build requirements
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		return ratesLookup.requirements(currencies);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.product.fx.FxNdfTrade trade, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(FxNdfTrade trade, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// resolve the trade once for all measures and all scenarios
		ResolvedFxNdfTrade resolved = trade.resolve(refData);

		// use lookup to query market data
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		RatesScenarioMarketData marketData = ratesLookup.marketDataView(scenarioMarketData);

		// loop around measures, calculating all scenarios for one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = new java.util.HashMap<>();
		IDictionary<Measure, Result<object>> results = new Dictionary<Measure, Result<object>>();
		foreach (Measure measure in measures)
		{
		  results[measure] = calculate(measure, resolved, marketData);
		}
		return results;
	  }

	  // calculate one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.fx.ResolvedFxNdfTrade trade, com.opengamma.strata.measure.rate.RatesScenarioMarketData marketData)
	  private Result<object> calculate(Measure measure, ResolvedFxNdfTrade trade, RatesScenarioMarketData marketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for FxNdfTrade: {}", measure);
		}
		return Result.of(() => calculator(trade, marketData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedFxNdfTrade trade, RatesScenarioMarketData marketData);

	}

}