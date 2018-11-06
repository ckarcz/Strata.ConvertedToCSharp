using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.security
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
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using GenericSecurityTrade = com.opengamma.strata.product.GenericSecurityTrade;
	using Security = com.opengamma.strata.product.Security;

	/// <summary>
	/// Perform calculations on a single {@code GenericSecurityTrade} for each of a set of scenarios.
	/// <para>
	/// The supported built-in measures are:
	/// <ul>
	///   <li><seealso cref="Measures#PRESENT_VALUE Present value"/>
	/// </ul>
	/// </para>
	/// </summary>
	public class GenericSecurityTradeCalculationFunction : CalculationFunction<GenericSecurityTrade>
	{

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, SecurityMeasureCalculations.presentValue).build();

	  private static readonly ImmutableSet<Measure> MEASURES = CALCULATORS.Keys;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public GenericSecurityTradeCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual Type<GenericSecurityTrade> targetType()
	  {
		return typeof(GenericSecurityTrade);
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return MEASURES;
	  }

	  public override Optional<string> identifier(GenericSecurityTrade target)
	  {
		return target.Info.Id.map(id => id.ToString());
	  }

	  public virtual Currency naturalCurrency(GenericSecurityTrade trade, ReferenceData refData)
	  {
		return trade.Currency;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(GenericSecurityTrade trade, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		QuoteId id = QuoteId.of(trade.SecurityId.StandardId);

		return FunctionRequirements.builder().valueRequirements(ImmutableSet.of(id)).outputCurrencies(trade.Currency).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.product.GenericSecurityTrade trade, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(GenericSecurityTrade trade, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// loop around measures, calculating all scenarios for one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = new java.util.HashMap<>();
		IDictionary<Measure, Result<object>> results = new Dictionary<Measure, Result<object>>();
		foreach (Measure measure in measures)
		{
		  results[measure] = calculate(measure, trade, scenarioMarketData);
		}
		return results;
	  }

	  // calculate one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.GenericSecurityTrade trade, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData)
	  private Result<object> calculate(Measure measure, GenericSecurityTrade trade, ScenarioMarketData scenarioMarketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for GenericSecurityTrade: {}", measure);
		}
		return Result.of(() => calculator(trade.Security, trade.Quantity, scenarioMarketData));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @FunctionalInterface interface SingleMeasureCalculation
	  delegate ScenarioArray<object> SingleMeasureCalculation(Security security, double quantity, ScenarioMarketData marketData);

	}

}