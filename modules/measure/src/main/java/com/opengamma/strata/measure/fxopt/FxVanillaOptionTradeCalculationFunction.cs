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
	using FxVanillaOption = com.opengamma.strata.product.fxopt.FxVanillaOption;
	using FxVanillaOptionTrade = com.opengamma.strata.product.fxopt.FxVanillaOptionTrade;
	using ResolvedFxVanillaOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOptionTrade;

	/// <summary>
	/// Perform calculations on an FX vanilla option trade for each of a set of scenarios.
	/// <para>
	/// This uses Black FX option volatilities, which must be specified using <seealso cref="FxOptionMarketDataLookup"/>.
	/// An instance of <seealso cref="RatesMarketDataLookup"/> must also be specified.
	/// </para>
	/// <para>
	/// Two pricing methods are available, 'Black' and 'VannaVolga'.
	/// By default, 'Black' will be used. To control the method, pass an instance of
	/// <seealso cref="FxVanillaOptionMethod"/> in the calculation parameters.
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
	public class FxVanillaOptionTradeCalculationFunction : CalculationFunction<FxVanillaOptionTrade>
	{

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, FxVanillaOptionMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, FxVanillaOptionMeasureCalculations.DEFAULT.pv01RatesCalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, FxVanillaOptionMeasureCalculations.DEFAULT.pv01RatesCalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, FxVanillaOptionMeasureCalculations.DEFAULT.pv01RatesMarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, FxVanillaOptionMeasureCalculations.DEFAULT.pv01RatesMarketQuoteBucketed).put(Measures.CURRENCY_EXPOSURE, FxVanillaOptionMeasureCalculations.DEFAULT.currencyExposure).put(Measures.CURRENT_CASH, FxVanillaOptionMeasureCalculations.DEFAULT.currentCash).put(Measures.RESOLVED_TARGET, (rt, smd, m, meth) => rt).build();

	  private static readonly ImmutableSet<Measure> MEASURES = CALCULATORS.Keys;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public FxVanillaOptionTradeCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual Type<FxVanillaOptionTrade> targetType()
	  {
		return typeof(FxVanillaOptionTrade);
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return MEASURES;
	  }

	  public override Optional<string> identifier(FxVanillaOptionTrade target)
	  {
		return target.Info.Id.map(id => id.ToString());
	  }

	  public virtual Currency naturalCurrency(FxVanillaOptionTrade trade, ReferenceData refData)
	  {
		return trade.Product.CurrencyPair.Base;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(FxVanillaOptionTrade trade, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		// extract data from product
		FxVanillaOption product = trade.Product;
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
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.product.fxopt.FxVanillaOptionTrade trade, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(FxVanillaOptionTrade trade, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// expand the trade once for all measures and all scenarios
		ResolvedFxVanillaOptionTrade resolved = trade.resolve(refData);
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		RatesScenarioMarketData ratesMarketData = ratesLookup.marketDataView(scenarioMarketData);
		FxOptionMarketDataLookup optionLookup = parameters.getParameter(typeof(FxOptionMarketDataLookup));
		FxOptionScenarioMarketData optionMarketData = optionLookup.marketDataView(scenarioMarketData);
		FxVanillaOptionMethod method = parameters.findParameter(typeof(FxVanillaOptionMethod)).orElse(FxVanillaOptionMethod.BLACK);

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
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.fxopt.ResolvedFxVanillaOptionTrade trade, com.opengamma.strata.measure.rate.RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxVanillaOptionMethod method)
	  private Result<object> calculate(Measure measure, ResolvedFxVanillaOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxVanillaOptionMethod method)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for FxVanillaOptionTrade: {}", measure);
		}
		return Result.of(() => calculator(trade, ratesMarketData, optionMarketData, method));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedFxVanillaOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxVanillaOptionMethod method);

	}

}