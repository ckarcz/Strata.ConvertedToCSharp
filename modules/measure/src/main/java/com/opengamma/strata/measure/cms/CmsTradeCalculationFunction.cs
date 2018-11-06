using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.cms
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationFunction = com.opengamma.strata.calc.runner.CalculationFunction;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using SwaptionMarketDataLookup = com.opengamma.strata.measure.swaption.SwaptionMarketDataLookup;
	using SwaptionScenarioMarketData = com.opengamma.strata.measure.swaption.SwaptionScenarioMarketData;
	using Cms = com.opengamma.strata.product.cms.Cms;
	using CmsTrade = com.opengamma.strata.product.cms.CmsTrade;
	using ResolvedCmsTrade = com.opengamma.strata.product.cms.ResolvedCmsTrade;

	/// <summary>
	/// Perform calculations on a single {@code CmsTrade} for each of a set of scenarios.
	/// <para>
	/// This uses SABR swaption volatilities, which must be specified using <seealso cref="SwaptionMarketDataLookup"/>.
	/// An instance of <seealso cref="RatesMarketDataLookup"/> and <seealso cref="CmsSabrExtrapolationParams"/> must also be specified.
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
	/// The "natural" currency is determined from the CMS leg.
	/// </para>
	/// </summary>
	public class CmsTradeCalculationFunction : CalculationFunction<CmsTrade>
	{

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, CmsMeasureCalculations::presentValue).put(Measures.PV01_CALIBRATED_SUM, CmsMeasureCalculations::pv01RatesCalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, CmsMeasureCalculations::pv01RatesCalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, CmsMeasureCalculations::pv01RatesMarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, CmsMeasureCalculations::pv01RatesMarketQuoteBucketed).put(Measures.CURRENCY_EXPOSURE, CmsMeasureCalculations::currencyExposure).put(Measures.CURRENT_CASH, CmsMeasureCalculations::currentCash).put(Measures.RESOLVED_TARGET, (c, rt, smd, m) => rt).build();

	  private static readonly ImmutableSet<Measure> MEASURES = CALCULATORS.Keys;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public CmsTradeCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual Type<CmsTrade> targetType()
	  {
		return typeof(CmsTrade);
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return MEASURES;
	  }

	  public override Optional<string> identifier(CmsTrade target)
	  {
		return target.Info.Id.map(id => id.ToString());
	  }

	  public virtual Currency naturalCurrency(CmsTrade trade, ReferenceData refData)
	  {
		return trade.Product.CmsLeg.Currency;
	  }

	  //-------------------------------------------------------------------------
	  public virtual FunctionRequirements requirements(CmsTrade trade, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		// extract data from product
		Cms product = trade.Product;
		ISet<Currency> currencies = product.allPaymentCurrencies();
		IborIndex cmsIndex = trade.Product.CmsLeg.UnderlyingIndex;
		ISet<Index> payIndices = trade.Product.allRateIndices();
		ISet<Index> indices = ImmutableSet.builder<Index>().add(cmsIndex).addAll(payIndices).build();

		// use lookup to build requirements
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		FunctionRequirements ratesReqs = ratesLookup.requirements(currencies, indices);
		SwaptionMarketDataLookup swaptionLookup = parameters.getParameter(typeof(SwaptionMarketDataLookup));
		FunctionRequirements swaptionReqs = swaptionLookup.requirements(cmsIndex);
		return ratesReqs.combinedWith(swaptionReqs);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.product.cms.CmsTrade trade, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(CmsTrade trade, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// expand the trade once for all measures and all scenarios
		ResolvedCmsTrade resolved = trade.resolve(refData);
		RatesMarketDataLookup ratesLookup = parameters.getParameter(typeof(RatesMarketDataLookup));
		RatesScenarioMarketData ratesMarketData = ratesLookup.marketDataView(scenarioMarketData);
		SwaptionMarketDataLookup swaptionLookup = parameters.getParameter(typeof(SwaptionMarketDataLookup));
		SwaptionScenarioMarketData swaptionMarketData = swaptionLookup.marketDataView(scenarioMarketData);
		CmsSabrExtrapolationParams cmsParams = parameters.getParameter(typeof(CmsSabrExtrapolationParams));
		CmsMeasureCalculations calculations = new CmsMeasureCalculations(cmsParams);

		// loop around measures, calculating all scenarios for one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = new java.util.HashMap<>();
		IDictionary<Measure, Result<object>> results = new Dictionary<Measure, Result<object>>();
		foreach (Measure measure in measures)
		{
		  results[measure] = calculate(measure, resolved, calculations, ratesMarketData, swaptionMarketData);
		}
		return results;
	  }

	  // calculate one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.cms.ResolvedCmsTrade trade, CmsMeasureCalculations calculations, com.opengamma.strata.measure.rate.RatesScenarioMarketData ratesMarketData, com.opengamma.strata.measure.swaption.SwaptionScenarioMarketData swaptionMarketData)
	  private Result<object> calculate(Measure measure, ResolvedCmsTrade trade, CmsMeasureCalculations calculations, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for SwaptionTrade: {}", measure);
		}
		return Result.of(() => calculator(calculations, trade, ratesMarketData, swaptionMarketData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(CmsMeasureCalculations calculations, ResolvedCmsTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData);

	}

}