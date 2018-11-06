using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationFunction = com.opengamma.strata.calc.runner.CalculationFunction;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using FxSingleBarrierOption = com.opengamma.strata.product.fxopt.FxSingleBarrierOption;
	using FxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.FxSingleBarrierOptionTrade;
	using ResolvedFxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOptionTrade;

	/// <summary>
	/// Perform calculations on an FX single barrier option trade for each of a set of scenarios.
	/// <para>
	/// This uses Black FX option volatilities, which must be specified using <seealso cref="FxOptionMarketDataLookup"/>.
	/// An instance of <seealso cref="RatesMarketDataLookup"/> must also be specified.
	/// </para>
	/// <para>
	/// Two pricing methods are available, 'Black' and 'TrinomialTree'.
	/// By default, 'Black' will be used. To control the method, pass an instance of
	/// <seealso cref="FxSingleBarrierOptionMethod"/> in the calculation parameters.
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
	/// The "natural" currency is the market convention base currency of the underlying FX.
	/// </para>
	/// </summary>
	public class FxSingleBarrierOptionTradeCalculationFunction : CalculationFunction<FxSingleBarrierOptionTrade>
	{

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, FxSingleBarrierOptionMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, FxSingleBarrierOptionMeasureCalculations.DEFAULT.pv01RatesCalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, FxSingleBarrierOptionMeasureCalculations.DEFAULT.pv01RatesCalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, FxSingleBarrierOptionMeasureCalculations.DEFAULT.pv01RatesMarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, FxSingleBarrierOptionMeasureCalculations.DEFAULT.pv01RatesMarketQuoteBucketed).put(Measures.CURRENCY_EXPOSURE, FxSingleBarrierOptionMeasureCalculations.DEFAULT.currencyExposure).put(Measures.CURRENT_CASH, FxSingleBarrierOptionMeasureCalculations.DEFAULT.currentCash).put(Measures.RESOLVED_TARGET, (rt, smd, m, meth) => rt).build();

	  private static readonly ImmutableSet<Measure> MEASURES = CALCULATORS.Keys;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public FxSingleBarrierOptionTradeCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual Type<FxSingleBarrierOptionTrade> targetType()
	  {
		return typeof(FxSingleBarrierOptionTrade);
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return MEASURES;
	  }

	  public override Optional<string> identifier(FxSingleBarrierOptionTrade target)
	  {
		return target.Info.Id.map(id => id.ToString());
	  }

	  public virtual Currency naturalCurrency(FxSingleBarrierOptionTrade trade, ReferenceData refData)
	  {
		return trade.Product.CurrencyPair.Base;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(FxSingleBarrierOptionTrade trade, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		// extract data from product
		FxSingleBarrierOption product = trade.Product;
		CurrencyPair currencyPair = product.CurrencyPair;

		// use lookup to build requirements
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		FunctionRequirements ratesReqs = ratesLookup.requirements(ImmutableSet.of(currencyPair.Base, currencyPair.Counter));
		FxOptionMarketDataLookup optionLookup = parameters.getParameter(typeof(FxOptionMarketDataLookup));
		FunctionRequirements optionReqs = optionLookup.requirements(currencyPair);
		return ratesReqs.combinedWith(optionReqs);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.product.fxopt.FxSingleBarrierOptionTrade trade, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(FxSingleBarrierOptionTrade trade, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// expand the trade once for all measures and all scenarios
		ResolvedFxSingleBarrierOptionTrade resolved = trade.resolve(refData);
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		RatesScenarioMarketData ratesMarketData = ratesLookup.marketDataView(scenarioMarketData);
		FxOptionMarketDataLookup optionLookup = parameters.getParameter(typeof(FxOptionMarketDataLookup));
		FxOptionScenarioMarketData optionMarketData = optionLookup.marketDataView(scenarioMarketData);
		FxSingleBarrierOptionMethod method = parameters.findParameter(typeof(FxSingleBarrierOptionMethod)).orElse(FxSingleBarrierOptionMethod.BLACK);

		// loop around measures, calculating all scenarios for one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = new java.util.HashMap<>();
		IDictionary<Measure, Result<object>> results = new Dictionary<Measure, Result<object>>();
		foreach (Measure measure in measures)
		{
		  results[measure] = calculate(measure, resolved, ratesMarketData, optionMarketData, method);
		}
		return results;
	  }

	  // calculate one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOptionTrade trade, com.opengamma.strata.measure.rate.RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxSingleBarrierOptionMethod method)
	  private Result<object> calculate(Measure measure, ResolvedFxSingleBarrierOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxSingleBarrierOptionMethod method)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for FxSingleBarrierOptionTrade: {}", measure);
		}
		return Result.of(() => calculator(trade, ratesMarketData, optionMarketData, method));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedFxSingleBarrierOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxSingleBarrierOptionMethod method);

	}

}