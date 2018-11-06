using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
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
	using IborFutureOption = com.opengamma.strata.product.index.IborFutureOption;
	using IborFutureOptionPosition = com.opengamma.strata.product.index.IborFutureOptionPosition;
	using IborFutureOptionTrade = com.opengamma.strata.product.index.IborFutureOptionTrade;
	using ResolvedIborFutureOptionTrade = com.opengamma.strata.product.index.ResolvedIborFutureOptionTrade;

	/// <summary>
	/// Perform calculations on a single {@code IborFutureOptionTrade} or {@code IborFutureOptionPosition}
	/// for each of a set of scenarios.
	/// <para>
	/// This uses Normal pricing.
	/// An instance of <seealso cref="RatesMarketDataLookup"/> and <seealso cref="IborFutureOptionMarketDataLookup"/> must be specified.
	/// The supported built-in measures are:
	/// <ul>
	///   <li><seealso cref="Measures#PRESENT_VALUE Present value"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_SUM PV01 calibrated sum"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_BUCKETED PV01 calibrated bucketed"/>
	///   <li><seealso cref="Measures#PV01_MARKET_QUOTE_SUM PV01 market quote sum"/>
	///   <li><seealso cref="Measures#PV01_MARKET_QUOTE_BUCKETED PV01 market quote bucketed"/>
	///   <li><seealso cref="Measures#UNIT_PRICE Unit price"/>
	///   <li><seealso cref="Measures#RESOLVED_TARGET Resolved trade"/>
	/// </ul>
	/// 
	/// <h4>Price</h4>
	/// The price of an Ibor future option is based on the price of the underlying future, the volatility
	/// and the time to expiry. The price of the at-the-money option tends to zero as expiry approaches.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Ibor future options in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, an option price of 0.2 is related to a futures price of 99.32 that implies an
	/// interest rate of 0.68%. Strata represents the price of the future as 0.9932 and thus
	/// represents the price of the option as 0.002.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the trade or position type </param>
	public class IborFutureOptionTradeCalculationFunction<T> : CalculationFunction<T> where T : com.opengamma.strata.product.SecuritizedProductPortfolioItem<com.opengamma.strata.product.index.IborFutureOption>, com.opengamma.strata.basics.Resolvable<com.opengamma.strata.product.index.ResolvedIborFutureOptionTrade>
	{

	  /// <summary>
	  /// The trade instance
	  /// </summary>
	  public static readonly IborFutureOptionTradeCalculationFunction<IborFutureOptionTrade> TRADE = new IborFutureOptionTradeCalculationFunction<IborFutureOptionTrade>(typeof(IborFutureOptionTrade));
	  /// <summary>
	  /// The position instance
	  /// </summary>
	  public static readonly IborFutureOptionTradeCalculationFunction<IborFutureOptionPosition> POSITION = new IborFutureOptionTradeCalculationFunction<IborFutureOptionPosition>(typeof(IborFutureOptionPosition));

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, IborFutureOptionMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, IborFutureOptionMeasureCalculations.DEFAULT.pv01CalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, IborFutureOptionMeasureCalculations.DEFAULT.pv01CalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, IborFutureOptionMeasureCalculations.DEFAULT.pv01MarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, IborFutureOptionMeasureCalculations.DEFAULT.pv01MarketQuoteBucketed).put(Measures.UNIT_PRICE, IborFutureOptionMeasureCalculations.DEFAULT.unitPrice).put(Measures.RESOLVED_TARGET, (rt, smd, m) => rt).build();

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
	  private IborFutureOptionTradeCalculationFunction(Type<T> targetType)
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
		IborFutureOption option = target.Product;
		QuoteId optionQuoteId = QuoteId.of(option.SecurityId.StandardId, FieldName.SETTLEMENT_PRICE);
		Currency currency = option.Currency;
		IborIndex index = option.Index;

		// use lookup to build requirements
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		FunctionRequirements ratesReqs = ratesLookup.requirements(currency, index);
		IborFutureOptionMarketDataLookup optionLookup = parameters.getParameter(typeof(IborFutureOptionMarketDataLookup));
		FunctionRequirements optionReqs = optionLookup.requirements(index);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> valueReqs = com.google.common.collect.ImmutableSet.builder<com.opengamma.strata.data.MarketDataId<?>>().add(optionQuoteId).addAll(ratesReqs.getValueRequirements()).addAll(optionReqs.getValueRequirements()).build();
		ImmutableSet<MarketDataId<object>> valueReqs = ImmutableSet.builder<MarketDataId<object>>().add(optionQuoteId).addAll(ratesReqs.ValueRequirements).addAll(optionReqs.ValueRequirements).build();
		return ratesReqs.toBuilder().valueRequirements(valueReqs).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(T target, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(T target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// resolve the trade once for all measures and all scenarios
		ResolvedIborFutureOptionTrade resolved = target.resolve(refData);

		// use lookup to query market data
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		RatesScenarioMarketData ratesMarketData = ratesLookup.marketDataView(scenarioMarketData);
		IborFutureOptionMarketDataLookup optionLookup = parameters.getParameter(typeof(IborFutureOptionMarketDataLookup));
		IborFutureOptionScenarioMarketData optionMarketData = optionLookup.marketDataView(scenarioMarketData);

		// loop around measures, calculating all scenarios for one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = new java.util.HashMap<>();
		IDictionary<Measure, Result<object>> results = new Dictionary<Measure, Result<object>>();
		foreach (Measure measure in measures)
		{
		  results[measure] = calculate(measure, resolved, ratesMarketData, optionMarketData);
		}
		return results;
	  }

	  // calculate one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.index.ResolvedIborFutureOptionTrade resolved, com.opengamma.strata.measure.rate.RatesScenarioMarketData ratesMarketData, IborFutureOptionScenarioMarketData optionMarketData)
	  private Result<object> calculate(Measure measure, ResolvedIborFutureOptionTrade resolved, RatesScenarioMarketData ratesMarketData, IborFutureOptionScenarioMarketData optionMarketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for IborFutureOption: {}", measure);
		}
		return Result.of(() => calculator(resolved, ratesMarketData, optionMarketData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedIborFutureOptionTrade resolved, RatesScenarioMarketData ratesMarketData, IborFutureOptionScenarioMarketData optionMarketData);

	}

}