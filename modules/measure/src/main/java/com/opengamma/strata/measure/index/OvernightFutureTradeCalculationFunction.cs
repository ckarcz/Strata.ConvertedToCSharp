using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.index
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
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
	using OvernightFuture = com.opengamma.strata.product.index.OvernightFuture;
	using OvernightFuturePosition = com.opengamma.strata.product.index.OvernightFuturePosition;
	using OvernightFutureTrade = com.opengamma.strata.product.index.OvernightFutureTrade;
	using ResolvedOvernightFutureTrade = com.opengamma.strata.product.index.ResolvedOvernightFutureTrade;

	/// <summary>
	/// Perform calculations on a single {@code OvernightFutureTrade} for each of a set of scenarios.
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
	///   <li><seealso cref="Measures#PAR_SPREAD Par spread"/>
	///   <li><seealso cref="Measures#RESOLVED_TARGET Resolved trade"/>
	/// </ul>
	/// 
	/// <h4>Price</h4>
	/// The price of an Overnight rate future is based on the interest rate of the underlying index.
	/// It is defined as {@code (100 - percentRate)}.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Overnight rate futures in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the trade or position type </param>
	public class OvernightFutureTradeCalculationFunction<T> : CalculationFunction<T> where T : com.opengamma.strata.product.SecuritizedProductPortfolioItem<com.opengamma.strata.product.index.OvernightFuture>, com.opengamma.strata.basics.Resolvable<com.opengamma.strata.product.index.ResolvedOvernightFutureTrade>
	{

	  /// <summary>
	  /// The trade instance
	  /// </summary>
	  public static readonly OvernightFutureTradeCalculationFunction<OvernightFutureTrade> TRADE = new OvernightFutureTradeCalculationFunction<OvernightFutureTrade>(typeof(OvernightFutureTrade));
	  /// <summary>
	  /// The position instance
	  /// </summary>
	  public static readonly OvernightFutureTradeCalculationFunction<OvernightFuturePosition> POSITION = new OvernightFutureTradeCalculationFunction<OvernightFuturePosition>(typeof(OvernightFuturePosition));

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, OvernightFutureMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, OvernightFutureMeasureCalculations.DEFAULT.pv01CalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, OvernightFutureMeasureCalculations.DEFAULT.pv01CalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, OvernightFutureMeasureCalculations.DEFAULT.pv01MarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, OvernightFutureMeasureCalculations.DEFAULT.pv01MarketQuoteBucketed).put(Measures.UNIT_PRICE, OvernightFutureMeasureCalculations.DEFAULT.unitPrice).put(Measures.PAR_SPREAD, OvernightFutureMeasureCalculations.DEFAULT.parSpread).put(Measures.RESOLVED_TARGET, (rt, smd) => rt).build();

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
	  public OvernightFutureTradeCalculationFunction(Type<T> targetType)
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
		return target.Product.Currency;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(T target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		// extract data from product
		OvernightFuture product = target.Product;
		QuoteId quoteId = QuoteId.of(target.Product.SecurityId.StandardId, FieldName.SETTLEMENT_PRICE);
		OvernightIndex index = product.Index;

		// use lookup to build requirements
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		FunctionRequirements ratesReqs = ratesLookup.requirements(ImmutableSet.of(), ImmutableSet.of(index));
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
		ResolvedOvernightFutureTrade resolved = target.resolve(refData);

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
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.index.ResolvedOvernightFutureTrade resolved, com.opengamma.strata.measure.rate.RatesScenarioMarketData marketData)
	  private Result<object> calculate(Measure measure, ResolvedOvernightFutureTrade resolved, RatesScenarioMarketData marketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for OvernightFutureTrade: {}", measure);
		}
		return Result.of(() => calculator(resolved, marketData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedOvernightFutureTrade resolved, RatesScenarioMarketData marketData);

	}

}