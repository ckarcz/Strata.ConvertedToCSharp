using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.credit
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationFunction = com.opengamma.strata.calc.runner.CalculationFunction;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CdsIndex = com.opengamma.strata.product.credit.CdsIndex;
	using CdsIndexTrade = com.opengamma.strata.product.credit.CdsIndexTrade;
	using ResolvedCdsIndexTrade = com.opengamma.strata.product.credit.ResolvedCdsIndexTrade;

	/// <summary>
	/// Perform calculations on a single {@code CdsIndexTrade} for each of a set of scenarios.
	/// <para>
	/// An instance of <seealso cref="CreditRatesMarketDataLookup"/> must be specified.
	/// The supported built-in measures are:
	/// <ul>
	///   <li><seealso cref="Measures#PRESENT_VALUE Present value"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_SUM PV01 calibrated sum on rate curves"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_BUCKETED PV01 calibrated bucketed on rate curves"/>
	///   <li><seealso cref="Measures#PV01_MARKET_QUOTE_SUM PV01 market quote sum on rate curves"/>
	///   <li><seealso cref="Measures#PV01_MARKET_QUOTE_BUCKETED PV01 market quote bucketed on rate curves"/>
	///   <li><seealso cref="Measures#UNIT_PRICE Unit price"/>
	///   <li><seealso cref="CreditMeasures#PRINCIPAL principal"/>
	///   <li><seealso cref="CreditMeasures#IR01_CALIBRATED_PARALLEL IR01 calibrated parallel"/>
	///   <li><seealso cref="CreditMeasures#IR01_CALIBRATED_BUCKETED IR01 calibrated bucketed"/>
	///   <li><seealso cref="CreditMeasures#IR01_MARKET_QUOTE_PARALLEL IR01 market quote parallel"/>
	///   <li><seealso cref="CreditMeasures#IR01_MARKET_QUOTE_BUCKETED IR01 market quote bucketed"/>
	///   <li><seealso cref="CreditMeasures#CS01_PARALLEL CS01 parallel"/>
	///   <li><seealso cref="CreditMeasures#CS01_BUCKETED CS01 bucketed"/>
	///   <li><seealso cref="CreditMeasures#RECOVERY01 recovery01"/>
	///   <li><seealso cref="CreditMeasures#JUMP_TO_DEFAULT jump to default"/>
	///   <li><seealso cref="CreditMeasures#EXPECTED_LOSS expected loss"/>
	/// </ul>
	/// </para>
	/// <para>
	/// The "natural" currency is the currency of the CDS index, which is limited to be single-currency.
	/// </para>
	/// </summary>
	public class CdsIndexTradeCalculationFunction : CalculationFunction<CdsIndexTrade>
	{

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, CdsIndexMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, CdsIndexMeasureCalculations.DEFAULT.pv01CalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, CdsIndexMeasureCalculations.DEFAULT.pv01CalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, CdsIndexMeasureCalculations.DEFAULT.pv01MarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, CdsIndexMeasureCalculations.DEFAULT.pv01MarketQuoteBucketed).put(Measures.UNIT_PRICE, CdsIndexMeasureCalculations.DEFAULT.unitPrice).put(CreditMeasures.PRINCIPAL, CdsIndexMeasureCalculations.DEFAULT.principal).put(CreditMeasures.IR01_CALIBRATED_PARALLEL, CdsIndexMeasureCalculations.DEFAULT.ir01CalibratedParallel).put(CreditMeasures.IR01_CALIBRATED_BUCKETED, CdsIndexMeasureCalculations.DEFAULT.ir01CalibratedBucketed).put(CreditMeasures.IR01_MARKET_QUOTE_PARALLEL, CdsIndexMeasureCalculations.DEFAULT.ir01MarketQuoteParallel).put(CreditMeasures.IR01_MARKET_QUOTE_BUCKETED, CdsIndexMeasureCalculations.DEFAULT.ir01MarketQuoteBucketed).put(CreditMeasures.CS01_PARALLEL, CdsIndexMeasureCalculations.DEFAULT.cs01Parallel).put(CreditMeasures.CS01_BUCKETED, CdsIndexMeasureCalculations.DEFAULT.cs01Bucketed).put(CreditMeasures.RECOVERY01, CdsIndexMeasureCalculations.DEFAULT.recovery01).put(CreditMeasures.JUMP_TO_DEFAULT, CdsIndexMeasureCalculations.DEFAULT.jumpToDefault).put(CreditMeasures.EXPECTED_LOSS, CdsIndexMeasureCalculations.DEFAULT.expectedLoss).put(Measures.RESOLVED_TARGET, (rt, smd, rd) => rt).build();

	  private static readonly ImmutableSet<Measure> MEASURES = CALCULATORS.Keys;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public CdsIndexTradeCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual Type<CdsIndexTrade> targetType()
	  {
		return typeof(CdsIndexTrade);
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return MEASURES;
	  }

	  public override Optional<string> identifier(CdsIndexTrade target)
	  {
		return target.Info.Id.map(id => id.ToString());
	  }

	  public virtual Currency naturalCurrency(CdsIndexTrade trade, ReferenceData refData)
	  {
		return trade.Product.Currency;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(CdsIndexTrade trade, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		// extract data from product
		CdsIndex product = trade.Product;
		StandardId legalEntityId = product.CdsIndexId;
		Currency currency = product.Currency;

		// use lookup to build requirements
		CreditRatesMarketDataLookup lookup = parameters.getParameter(typeof(CreditRatesMarketDataLookup));
		return lookup.requirements(legalEntityId, currency);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.product.credit.CdsIndexTrade trade, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(CdsIndexTrade trade, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// resolve the trade once for all measures and all scenarios
		ResolvedCdsIndexTrade resolved = trade.resolve(refData);

		// use lookup to query market data
		CreditRatesMarketDataLookup ledLookup = parameters.getParameter(typeof(CreditRatesMarketDataLookup));
		CreditRatesScenarioMarketData marketData = ledLookup.marketDataView(scenarioMarketData);

		// loop around measures, calculating all scenarios for one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = new java.util.HashMap<>();
		IDictionary<Measure, Result<object>> results = new Dictionary<Measure, Result<object>>();
		foreach (Measure measure in measures)
		{
		  results[measure] = calculate(measure, resolved, marketData, refData);
		}
		return results;
	  }

	  // calculate one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.credit.ResolvedCdsIndexTrade trade, CreditRatesScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
	  private Result<object> calculate(Measure measure, ResolvedCdsIndexTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for CdsIndexTrade: {}", measure);
		}
		return Result.of(() => calculator(trade, marketData, refData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedCdsIndexTrade trade, CreditRatesScenarioMarketData marketData, ReferenceData refData);

	}

}