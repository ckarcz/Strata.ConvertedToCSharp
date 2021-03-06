﻿using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.dsf
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationFunction = com.opengamma.strata.calc.runner.CalculationFunction;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using FieldName = com.opengamma.strata.data.FieldName;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using SecuritizedProductPortfolioItem = com.opengamma.strata.product.SecuritizedProductPortfolioItem;
	using Dsf = com.opengamma.strata.product.dsf.Dsf;
	using DsfPosition = com.opengamma.strata.product.dsf.DsfPosition;
	using DsfTrade = com.opengamma.strata.product.dsf.DsfTrade;
	using ResolvedDsfTrade = com.opengamma.strata.product.dsf.ResolvedDsfTrade;

	/// <summary>
	/// Perform calculations on a single {@code DsfTrade} or {@code DsfPosition}
	/// for each of a set of scenarios.
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
	///   <li><seealso cref="Measures#UNIT_PRICE Unit price"/>
	///   <li><seealso cref="Measures#CURRENCY_EXPOSURE Currency exposure"/>
	///   <li><seealso cref="Measures#RESOLVED_TARGET Resolved trade"/>
	/// </ul>
	/// </para>
	/// <para>
	/// The "natural" currency is the currency of the swap leg that is received.
	/// 
	/// <h4>Price</h4>
	/// The price of a DSF is based on the present value (NPV) of the underlying swap on the delivery date.
	/// For example, a price of 100.182 represents a present value of $100,182.00, if the notional is $100,000.
	/// This price can also be viewed as a percentage present value - {@code (100 + percentPv)}, or 0.182% in this example.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for DSFs in the trade model, pricers and market data.
	/// The decimal price is based on the decimal multiplier equivalent to the implied percentage.
	/// Thus the market price of 100.182 is represented in Strata by 1.00182.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the trade or position type </param>
	public class DsfTradeCalculationFunction<T> : CalculationFunction<T> where T : com.opengamma.strata.product.SecuritizedProductPortfolioItem<com.opengamma.strata.product.dsf.Dsf>, com.opengamma.strata.basics.Resolvable<com.opengamma.strata.product.dsf.ResolvedDsfTrade>
	{

	  /// <summary>
	  /// The trade instance
	  /// </summary>
	  public static readonly DsfTradeCalculationFunction<DsfTrade> TRADE = new DsfTradeCalculationFunction<DsfTrade>(typeof(DsfTrade));
	  /// <summary>
	  /// The position instance
	  /// </summary>
	  public static readonly DsfTradeCalculationFunction<DsfPosition> POSITION = new DsfTradeCalculationFunction<DsfPosition>(typeof(DsfPosition));

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, DsfMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, DsfMeasureCalculations.DEFAULT.pv01CalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, DsfMeasureCalculations.DEFAULT.pv01CalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, DsfMeasureCalculations.DEFAULT.pv01MarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, DsfMeasureCalculations.DEFAULT.pv01MarketQuoteBucketed).put(Measures.UNIT_PRICE, DsfMeasureCalculations.DEFAULT.unitPrice).put(Measures.CURRENCY_EXPOSURE, DsfMeasureCalculations.DEFAULT.currencyExposure).put(Measures.RESOLVED_TARGET, (rt, smd) => rt).build();

	  private static readonly ImmutableSet<Measure> MEASURES = CALCULATORS.Keys;

	  /// <summary>
	  /// The trade or position type.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly Type<T> targetType_Renamed;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="targetType">  the trade or position type </param>
	  private DsfTradeCalculationFunction(Type<T> targetType)
	  {
		this.targetType_Renamed = ArgChecker.notNull(targetType, "targetType");
	  }

	  //-------------------------------------------------------------------------
	  public virtual Type<T> targetType()
	  {
		return targetType_Renamed;
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return MEASURES;
	  }

	  public override Optional<string> identifier(T target)
	  {
		return target.Info.Id.map(id => id.ToString());
	  }

	  public virtual Currency naturalCurrency(T target, ReferenceData refData)
	  {
		return target.Currency;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(T target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		// extract data from product
		Dsf product = target.Product;
		QuoteId quoteId = QuoteId.of(target.SecurityId.StandardId, FieldName.SETTLEMENT_PRICE);
		ImmutableSet<Index> indices = product.UnderlyingSwap.allIndices();
		ImmutableSet<Currency> currencies = ImmutableSet.of(product.Currency);

		// use lookup to build requirements
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		FunctionRequirements ratesReqs = ratesLookup.requirements(currencies, indices);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> valueReqs = com.google.common.collect.ImmutableSet.builder<com.opengamma.strata.data.MarketDataId<?>>().add(quoteId).addAll(ratesReqs.getValueRequirements()).build();
		ImmutableSet<MarketDataId<object>> valueReqs = ImmutableSet.builder<MarketDataId<object>>().add(quoteId).addAll(ratesReqs.ValueRequirements).build();
		return ratesReqs.toBuilder().valueRequirements(valueReqs).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(T target, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(T target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// resolve the trade once for all measures and all scenarios
		ResolvedDsfTrade resolved = target.resolve(refData);

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
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.dsf.ResolvedDsfTrade resolved, com.opengamma.strata.measure.rate.RatesScenarioMarketData marketData)
	  private Result<object> calculate(Measure measure, ResolvedDsfTrade resolved, RatesScenarioMarketData marketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for Dsf: {}", measure);
		}
		return Result.of(() => calculator(resolved, marketData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedDsfTrade resolved, RatesScenarioMarketData marketData);

	}

}