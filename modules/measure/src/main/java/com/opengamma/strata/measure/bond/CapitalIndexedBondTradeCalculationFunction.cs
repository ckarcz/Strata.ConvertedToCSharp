using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationFunction = com.opengamma.strata.calc.runner.CalculationFunction;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecuritizedProductPortfolioItem = com.opengamma.strata.product.SecuritizedProductPortfolioItem;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using CapitalIndexedBond = com.opengamma.strata.product.bond.CapitalIndexedBond;
	using CapitalIndexedBondPosition = com.opengamma.strata.product.bond.CapitalIndexedBondPosition;
	using CapitalIndexedBondTrade = com.opengamma.strata.product.bond.CapitalIndexedBondTrade;
	using ResolvedCapitalIndexedBondTrade = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBondTrade;

	/// <summary>
	/// Perform calculations on a single {@code CapitalIndexedBondTrade} or {@code CapitalIndexedBondPosition}
	/// for each of a set of scenarios.
	/// <para>
	/// This uses the standard discounting calculation method.
	/// An instance of <seealso cref="LegalEntityDiscountingMarketDataLookup"/> must be specified.
	/// The supported built-in measures are:
	/// <ul>
	///   <li><seealso cref="Measures#PRESENT_VALUE Present value"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_SUM PV01 calibrated sum"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_BUCKETED PV01 calibrated bucketed"/>
	///   <li><seealso cref="Measures#CURRENCY_EXPOSURE Currency exposure"/>
	///   <li><seealso cref="Measures#CURRENT_CASH Current cash"/>
	///   <li><seealso cref="Measures#RESOLVED_TARGET Resolved trade"/>
	/// </ul>
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the trade or position type </param>
	public class CapitalIndexedBondTradeCalculationFunction<T> : CalculationFunction<T> where T : com.opengamma.strata.product.SecuritizedProductPortfolioItem<com.opengamma.strata.product.bond.CapitalIndexedBond>, com.opengamma.strata.basics.Resolvable<com.opengamma.strata.product.bond.ResolvedCapitalIndexedBondTrade>
	{

	  /// <summary>
	  /// The trade instance
	  /// </summary>
	  public static readonly CapitalIndexedBondTradeCalculationFunction<CapitalIndexedBondTrade> TRADE = new CapitalIndexedBondTradeCalculationFunction<CapitalIndexedBondTrade>(typeof(CapitalIndexedBondTrade));
	  /// <summary>
	  /// The position instance
	  /// </summary>
	  public static readonly CapitalIndexedBondTradeCalculationFunction<CapitalIndexedBondPosition> POSITION = new CapitalIndexedBondTradeCalculationFunction<CapitalIndexedBondPosition>(typeof(CapitalIndexedBondPosition));

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, CapitalIndexedBondMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, CapitalIndexedBondMeasureCalculations.DEFAULT.pv01CalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, CapitalIndexedBondMeasureCalculations.DEFAULT.pv01CalibratedBucketed).put(Measures.CURRENCY_EXPOSURE, CapitalIndexedBondMeasureCalculations.DEFAULT.currencyExposure).put(Measures.CURRENT_CASH, CapitalIndexedBondMeasureCalculations.DEFAULT.currentCash).put(Measures.RESOLVED_TARGET, (rt, smd1, smd2) => rt).build();

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
	  private CapitalIndexedBondTradeCalculationFunction(Type<T> targetType)
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
		CapitalIndexedBond product = target.Product;
		Currency currency = product.Currency;
		SecurityId securityId = product.SecurityId;
		LegalEntityId legalEntityId = product.LegalEntityId;

		// use lookup to build requirements
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		FunctionRequirements ratesReqs = ratesLookup.requirements(ImmutableSet.of(), ImmutableSet.of(product.RateCalculation.Index));
		LegalEntityDiscountingMarketDataLookup ledLookup = parameters.getParameter(typeof(LegalEntityDiscountingMarketDataLookup));
		FunctionRequirements ledReqs = ledLookup.requirements(securityId, legalEntityId, currency);
		return ratesReqs.combinedWith(ledReqs);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(T target, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(T target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// resolve the trade once for all measures and all scenarios
		ResolvedCapitalIndexedBondTrade resolved = target.resolve(refData);

		// use lookup to query market data
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		RatesScenarioMarketData ratesMarketData = ratesLookup.marketDataView(scenarioMarketData);
		LegalEntityDiscountingMarketDataLookup ledLookup = parameters.getParameter(typeof(LegalEntityDiscountingMarketDataLookup));
		LegalEntityDiscountingScenarioMarketData legalEntityMarketData = ledLookup.marketDataView(scenarioMarketData);

		// loop around measures, calculating all scenarios for one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = new java.util.HashMap<>();
		IDictionary<Measure, Result<object>> results = new Dictionary<Measure, Result<object>>();
		foreach (Measure measure in measures)
		{
		  results[measure] = calculate(measure, resolved, ratesMarketData, legalEntityMarketData);
		}
		return results;
	  }

	  // calculate one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.bond.ResolvedCapitalIndexedBondTrade resolved, com.opengamma.strata.measure.rate.RatesScenarioMarketData ratesMarketData, LegalEntityDiscountingScenarioMarketData legalEntityMarketData)
	  private Result<object> calculate(Measure measure, ResolvedCapitalIndexedBondTrade resolved, RatesScenarioMarketData ratesMarketData, LegalEntityDiscountingScenarioMarketData legalEntityMarketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for CapitalIndexedBond: {}", measure);
		}
		return Result.of(() => calculator(resolved, ratesMarketData, legalEntityMarketData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedCapitalIndexedBondTrade resolved, RatesScenarioMarketData ratesMarketData, LegalEntityDiscountingScenarioMarketData legalEntityMarketData);

	}

}