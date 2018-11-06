using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swap
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
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Perform calculations on a single {@code SwapTrade} for each of a set of scenarios.
	/// <para>
	/// This uses the standard discounting calculation method.
	/// An instance of <seealso cref="RatesMarketDataLookup"/> must be specified.
	/// The supported built-in measures are:
	/// <ul>
	///   <li><seealso cref="Measures#PRESENT_VALUE Present value"/>
	///   <li><seealso cref="Measures#EXPLAIN_PRESENT_VALUE Explain present value"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_SUM PV01 calibrated sum"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_BUCKETED PV01 calibrated bucketed"/>
	///   <li><seealso cref="Measures#PV01_MARKET_QUOTE_SUM PV01 market quote sum"/>
	///   <li><seealso cref="Measures#PV01_MARKET_QUOTE_BUCKETED PV01 market quote bucketed"/>
	///   <li><seealso cref="Measures#PAR_RATE Par rate"/>
	///   <li><seealso cref="Measures#PAR_SPREAD Par spread"/>
	///   <li><seealso cref="Measures#CASH_FLOWS Cash flows"/>
	///   <li><seealso cref="Measures#ACCRUED_INTEREST Accrued interest"/>
	///   <li><seealso cref="Measures#LEG_INITIAL_NOTIONAL Leg initial notional"/>
	///   <li><seealso cref="Measures#LEG_PRESENT_VALUE Leg present value"/>
	///   <li><seealso cref="Measures#CURRENCY_EXPOSURE Currency exposure"/>
	///   <li><seealso cref="Measures#CURRENT_CASH Current cash"/>
	///   <li><seealso cref="Measures#RESOLVED_TARGET Resolved trade"/>
	///   <li><seealso cref="AdvancedMeasures#PV01_SEMI_PARALLEL_GAMMA_BUCKETED PV01 semi-parallel gamma bucketed"/>
	///   <li><seealso cref="AdvancedMeasures#PV01_SINGLE_NODE_GAMMA_BUCKETED PV01 single node gamma bucketed"/>
	/// </ul>
	/// </para>
	/// <para>
	/// The "natural" currency is the currency of the swaption, which is limited to be single-currency.
	/// </para>
	/// </summary>
	public class SwapTradeCalculationFunction : CalculationFunction<SwapTrade>
	{

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, SwapMeasureCalculations.DEFAULT.presentValue).put(Measures.EXPLAIN_PRESENT_VALUE, SwapMeasureCalculations.DEFAULT.explainPresentValue).put(Measures.PV01_CALIBRATED_SUM, SwapMeasureCalculations.DEFAULT.pv01CalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, SwapMeasureCalculations.DEFAULT.pv01CalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, SwapMeasureCalculations.DEFAULT.pv01MarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, SwapMeasureCalculations.DEFAULT.pv01MarketQuoteBucketed).put(Measures.PAR_RATE, SwapMeasureCalculations.DEFAULT.parRate).put(Measures.PAR_SPREAD, SwapMeasureCalculations.DEFAULT.parSpread).put(Measures.CASH_FLOWS, SwapMeasureCalculations.DEFAULT.cashFlows).put(Measures.ACCRUED_INTEREST, SwapMeasureCalculations.DEFAULT.accruedInterest).put(Measures.LEG_INITIAL_NOTIONAL, SwapMeasureCalculations.DEFAULT.legInitialNotional).put(Measures.LEG_PRESENT_VALUE, SwapMeasureCalculations.DEFAULT.legPresentValue).put(Measures.CURRENCY_EXPOSURE, SwapMeasureCalculations.DEFAULT.currencyExposure).put(Measures.CURRENT_CASH, SwapMeasureCalculations.DEFAULT.currentCash).put(Measures.RESOLVED_TARGET, (rt, smd) => rt).put(AdvancedMeasures.PV01_SEMI_PARALLEL_GAMMA_BUCKETED, SwapMeasureCalculations.DEFAULT.pv01SemiParallelGammaBucketed).put(AdvancedMeasures.PV01_SINGLE_NODE_GAMMA_BUCKETED, SwapMeasureCalculations.DEFAULT.pv01SingleNodeGammaBucketed).build();

	  private static readonly ImmutableSet<Measure> MEASURES = CALCULATORS.Keys;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public SwapTradeCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual Type<SwapTrade> targetType()
	  {
		return typeof(SwapTrade);
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return MEASURES;
	  }

	  public override Optional<string> identifier(SwapTrade target)
	  {
		return target.Info.Id.map(id => id.ToString());
	  }

	  public virtual Currency naturalCurrency(SwapTrade trade, ReferenceData refData)
	  {
		return trade.Product.Legs.get(0).Currency;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(SwapTrade trade, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		// extract data from product
		Swap product = trade.Product;
		ImmutableSet<Currency> currencies = product.allPaymentCurrencies();

		// use lookup to build requirements
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		return ratesLookup.requirements(currencies, product.allIndices());
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.product.swap.SwapTrade trade, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(SwapTrade trade, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// resolve the trade once for all measures and all scenarios
		ResolvedSwapTrade resolved = trade.resolve(refData);

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
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.swap.ResolvedSwapTrade trade, com.opengamma.strata.measure.rate.RatesScenarioMarketData marketData)
	  private Result<object> calculate(Measure measure, ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for SwapTrade: {}", measure);
		}
		return Result.of(() => calculator(trade, marketData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedSwapTrade trade, RatesScenarioMarketData marketData);

	}

}