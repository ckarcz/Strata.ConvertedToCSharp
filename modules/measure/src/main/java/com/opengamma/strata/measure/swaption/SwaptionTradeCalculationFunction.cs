using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swaption
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationFunction = com.opengamma.strata.calc.runner.CalculationFunction;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using ResolvedSwaptionTrade = com.opengamma.strata.product.swaption.ResolvedSwaptionTrade;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;
	using SwaptionTrade = com.opengamma.strata.product.swaption.SwaptionTrade;

	/// <summary>
	/// Perform calculations on a single {@code SwaptionTrade} for each of a set of scenarios.
	/// <para>
	/// This uses Black, Normal or SABR swaption volatilities,
	/// which must be specified using <seealso cref="SwaptionMarketDataLookup"/>.
	/// An instance of <seealso cref="RatesMarketDataLookup"/> must also be specified.
	/// </para>
	/// <para>
	/// The supported built-in measures are:
	/// <ul>
	///   <li><seealso cref="Measures#PRESENT_VALUE Present value"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_SUM PV01 calibrated sum on rate curves"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_BUCKETED PV01 calibrated bucketed on rate curves"/>
	///   <li><seealso cref="Measures#PV01_MARKET_QUOTE_SUM PV01 market quote sum on rate curves"/>
	///   <li><seealso cref="Measures#PV01_MARKET_QUOTE_BUCKETED PV01 market quote bucketed on rate curves"/>
	///   <li><seealso cref="Measures#CURRENCY_EXPOSURE Currency exposure"/>
	///   <li><seealso cref="Measures#CURRENT_CASH Current cash"/>
	///   <li><seealso cref="Measures#RESOLVED_TARGET Resolved trade"/>
	/// </ul>
	/// </para>
	/// <para>
	/// The "natural" currency is determined from the first swap leg.
	/// </para>
	/// </summary>
	public class SwaptionTradeCalculationFunction : CalculationFunction<SwaptionTrade>
	{

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, SwaptionMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, SwaptionMeasureCalculations.DEFAULT.pv01RatesCalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, SwaptionMeasureCalculations.DEFAULT.pv01RatesCalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, SwaptionMeasureCalculations.DEFAULT.pv01RatesMarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, SwaptionMeasureCalculations.DEFAULT.pv01RatesMarketQuoteBucketed).put(Measures.CURRENCY_EXPOSURE, SwaptionMeasureCalculations.DEFAULT.currencyExposure).put(Measures.CURRENT_CASH, SwaptionMeasureCalculations.DEFAULT.currentCash).put(Measures.RESOLVED_TARGET, (rt, smd, m) => rt).build();

	  private static readonly ImmutableSet<Measure> MEASURES = CALCULATORS.Keys;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public SwaptionTradeCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual Type<SwaptionTrade> targetType()
	  {
		return typeof(SwaptionTrade);
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return MEASURES;
	  }

	  public override Optional<string> identifier(SwaptionTrade target)
	  {
		return target.Info.Id.map(id => id.ToString());
	  }

	  public virtual Currency naturalCurrency(SwaptionTrade trade, ReferenceData refData)
	  {
		return trade.Product.Currency;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(SwaptionTrade trade, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		// extract data from product
		Swaption product = trade.Product;
		Currency currency = product.Currency;
		IborIndex index = product.Index;

		// use lookup to build requirements
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		FunctionRequirements ratesReqs = ratesLookup.requirements(currency, index);
		SwaptionMarketDataLookup swaptionLookup = parameters.getParameter(typeof(SwaptionMarketDataLookup));
		FunctionRequirements swaptionReqs = swaptionLookup.requirements(index);
		return ratesReqs.combinedWith(swaptionReqs);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.product.swaption.SwaptionTrade trade, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(SwaptionTrade trade, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// expand the trade once for all measures and all scenarios
		ResolvedSwaptionTrade resolved = trade.resolve(refData);
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		RatesScenarioMarketData ratesMarketData = ratesLookup.marketDataView(scenarioMarketData);
		SwaptionMarketDataLookup swaptionLookup = parameters.getParameter(typeof(SwaptionMarketDataLookup));
		SwaptionScenarioMarketData swaptionMarketData = swaptionLookup.marketDataView(scenarioMarketData);

		// loop around measures, calculating all scenarios for one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = new java.util.HashMap<>();
		IDictionary<Measure, Result<object>> results = new Dictionary<Measure, Result<object>>();
		foreach (Measure measure in measures)
		{
		  results[measure] = calculate(measure, resolved, ratesMarketData, swaptionMarketData);
		}
		return results;
	  }

	  // calculate one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.swaption.ResolvedSwaptionTrade trade, com.opengamma.strata.measure.rate.RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  private Result<object> calculate(Measure measure, ResolvedSwaptionTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for SwaptionTrade: {}", measure);
		}
		return Result.of(() => calculator(trade, ratesMarketData, swaptionMarketData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedSwaptionTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData);

	}

}