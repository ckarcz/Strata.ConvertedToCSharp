using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.capfloor
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationFunction = com.opengamma.strata.calc.runner.CalculationFunction;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using IborCapFloor = com.opengamma.strata.product.capfloor.IborCapFloor;
	using IborCapFloorTrade = com.opengamma.strata.product.capfloor.IborCapFloorTrade;
	using ResolvedIborCapFloorTrade = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorTrade;

	/// <summary>
	/// Perform calculations on a single {@code IborCapFloorTrade} for each of a set of scenarios.
	/// <para>
	/// This uses Black, Normal or SABR cap/floor volatilities,
	/// which must be specified using <seealso cref="IborCapFloorMarketDataLookup"/>.
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
	/// </ul>
	/// </para>
	/// <para>
	/// The "natural" currency is determined from the cap/floor leg.
	/// </para>
	/// </summary>
	public class IborCapFloorTradeCalculationFunction : CalculationFunction<IborCapFloorTrade>
	{

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, IborCapFloorMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, IborCapFloorMeasureCalculations.DEFAULT.pv01RatesCalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, IborCapFloorMeasureCalculations.DEFAULT.pv01RatesCalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, IborCapFloorMeasureCalculations.DEFAULT.pv01RatesMarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, IborCapFloorMeasureCalculations.DEFAULT.pv01RatesMarketQuoteBucketed).put(Measures.CURRENCY_EXPOSURE, IborCapFloorMeasureCalculations.DEFAULT.currencyExposure).put(Measures.CURRENT_CASH, IborCapFloorMeasureCalculations.DEFAULT.currentCash).build();

	  private static readonly ImmutableSet<Measure> MEASURES = CALCULATORS.Keys;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public IborCapFloorTradeCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual Type<IborCapFloorTrade> targetType()
	  {
		return typeof(IborCapFloorTrade);
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return MEASURES;
	  }

	  public override Optional<string> identifier(IborCapFloorTrade target)
	  {
		return target.Info.Id.map(id => id.ToString());
	  }

	  public virtual Currency naturalCurrency(IborCapFloorTrade trade, ReferenceData refData)
	  {
		return trade.Product.CapFloorLeg.Currency;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(IborCapFloorTrade trade, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		// extract data from product
		IborCapFloor product = trade.Product;
		ISet<Currency> currencies = product.allPaymentCurrencies();
		ISet<Index> indices = product.allIndices();

		// use lookup to build requirements
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		FunctionRequirements ratesReqs = ratesLookup.requirements(currencies, indices);
		IborCapFloorMarketDataLookup capFloorLookup = parameters.getParameter(typeof(IborCapFloorMarketDataLookup));
		FunctionRequirements capFloorReqs = capFloorLookup.requirements(product.CapFloorLeg.Index);
		return ratesReqs.combinedWith(capFloorReqs);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.product.capfloor.IborCapFloorTrade trade, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(IborCapFloorTrade trade, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// expand the trade once for all measures and all scenarios
		ResolvedIborCapFloorTrade resolved = trade.resolve(refData);
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		RatesScenarioMarketData ratesMarketData = ratesLookup.marketDataView(scenarioMarketData);
		IborCapFloorMarketDataLookup capFloorLookup = parameters.getParameter(typeof(IborCapFloorMarketDataLookup));
		IborCapFloorScenarioMarketData capFloorMarketData = capFloorLookup.marketDataView(scenarioMarketData);

		// loop around measures, calculating all scenarios for one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = new java.util.HashMap<>();
		IDictionary<Measure, Result<object>> results = new Dictionary<Measure, Result<object>>();
		foreach (Measure measure in measures)
		{
		  results[measure] = calculate(measure, resolved, ratesMarketData, capFloorMarketData);
		}
		return results;
	  }

	  // calculate one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.capfloor.ResolvedIborCapFloorTrade trade, com.opengamma.strata.measure.rate.RatesScenarioMarketData ratesMarketData, IborCapFloorScenarioMarketData capFloorMarketData)
	  private Result<object> calculate(Measure measure, ResolvedIborCapFloorTrade trade, RatesScenarioMarketData ratesMarketData, IborCapFloorScenarioMarketData capFloorMarketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for IborCapFloorTrade: {}", measure);
		}
		return Result.of(() => calculator(trade, ratesMarketData, capFloorMarketData));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @FunctionalInterface interface SingleMeasureCalculation
	  delegate ScenarioArray<object> SingleMeasureCalculation(ResolvedIborCapFloorTrade trade, RatesScenarioMarketData ratesMarketData, IborCapFloorScenarioMarketData capFloorMarketData);

	}

}