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
	using FieldName = com.opengamma.strata.data.FieldName;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using SecuritizedProductPortfolioItem = com.opengamma.strata.product.SecuritizedProductPortfolioItem;
	using BondFuture = com.opengamma.strata.product.bond.BondFuture;
	using BondFutureOption = com.opengamma.strata.product.bond.BondFutureOption;
	using BondFutureOptionPosition = com.opengamma.strata.product.bond.BondFutureOptionPosition;
	using BondFutureOptionTrade = com.opengamma.strata.product.bond.BondFutureOptionTrade;
	using FixedCouponBond = com.opengamma.strata.product.bond.FixedCouponBond;
	using ResolvedBondFutureOptionTrade = com.opengamma.strata.product.bond.ResolvedBondFutureOptionTrade;

	/// <summary>
	/// Perform calculations on a single {@code BondFutureOptionTrade} or {@code BondFutureOptionPosition}
	/// for each of a set of scenarios.
	/// <para>
	/// This uses Black pricing.
	/// An instance of <seealso cref="RatesMarketDataLookup"/> and <seealso cref="BondFutureOptionMarketDataLookup"/> must be specified.
	/// The supported built-in measures are:
	/// <ul>
	///   <li><seealso cref="Measures#PRESENT_VALUE Present value"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_SUM PV01 calibrated sum"/>
	///   <li><seealso cref="Measures#PV01_CALIBRATED_BUCKETED PV01 calibrated bucketed"/>
	///   <li><seealso cref="Measures#UNIT_PRICE Unit price"/>
	///   <li><seealso cref="Measures#CURRENCY_EXPOSURE Currency exposure"/>
	///   <li><seealso cref="Measures#RESOLVED_TARGET Resolved trade"/>
	/// </ul>
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bond futures options in the trade model, pricers and market data.
	/// This is coherent with the pricing of <seealso cref="BondFuture"/>.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the trade or position type </param>
	public class BondFutureOptionTradeCalculationFunction<T> : CalculationFunction<T> where T : com.opengamma.strata.product.SecuritizedProductPortfolioItem<com.opengamma.strata.product.bond.BondFutureOption>, com.opengamma.strata.basics.Resolvable<com.opengamma.strata.product.bond.ResolvedBondFutureOptionTrade>
	{

	  /// <summary>
	  /// The trade instance
	  /// </summary>
	  public static readonly BondFutureOptionTradeCalculationFunction<BondFutureOptionTrade> TRADE = new BondFutureOptionTradeCalculationFunction<BondFutureOptionTrade>(typeof(BondFutureOptionTrade));
	  /// <summary>
	  /// The position instance
	  /// </summary>
	  public static readonly BondFutureOptionTradeCalculationFunction<BondFutureOptionPosition> POSITION = new BondFutureOptionTradeCalculationFunction<BondFutureOptionPosition>(typeof(BondFutureOptionPosition));

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, BondFutureOptionMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, BondFutureOptionMeasureCalculations.DEFAULT.pv01CalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, BondFutureOptionMeasureCalculations.DEFAULT.pv01CalibratedBucketed).put(Measures.UNIT_PRICE, BondFutureOptionMeasureCalculations.DEFAULT.unitPrice).put(Measures.CURRENCY_EXPOSURE, BondFutureOptionMeasureCalculations.DEFAULT.currencyExposure).put(Measures.RESOLVED_TARGET, (rt, smd, m) => rt).build();

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
	  private BondFutureOptionTradeCalculationFunction(Type<T> targetType)
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
		BondFutureOption option = target.Product;
		BondFuture future = option.UnderlyingFuture;

		// use lookup to build requirements
		QuoteId optionQuoteId = QuoteId.of(option.SecurityId.StandardId, FieldName.SETTLEMENT_PRICE);
		FunctionRequirements freqs = FunctionRequirements.builder().valueRequirements(optionQuoteId).outputCurrencies(future.Currency, option.Currency).build();
		LegalEntityDiscountingMarketDataLookup ledLookup = parameters.getParameter(typeof(LegalEntityDiscountingMarketDataLookup));
		foreach (FixedCouponBond bond in future.DeliveryBasket)
		{
		  freqs = freqs.combinedWith(ledLookup.requirements(bond.SecurityId, bond.LegalEntityId, bond.Currency));
		}
		BondFutureOptionMarketDataLookup optionLookup = parameters.getParameter(typeof(BondFutureOptionMarketDataLookup));
		FunctionRequirements optionReqs = optionLookup.requirements(future.SecurityId);
		return freqs.combinedWith(optionReqs);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(T target, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(T target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// resolve the trade once for all measures and all scenarios
		ResolvedBondFutureOptionTrade resolved = target.resolve(refData);

		// use lookup to query market data
		LegalEntityDiscountingMarketDataLookup ledLookup = parameters.getParameter(typeof(LegalEntityDiscountingMarketDataLookup));
		LegalEntityDiscountingScenarioMarketData ledMarketData = ledLookup.marketDataView(scenarioMarketData);
		BondFutureOptionMarketDataLookup optionLookup = parameters.getParameter(typeof(BondFutureOptionMarketDataLookup));
		BondFutureOptionScenarioMarketData optionMarketData = optionLookup.marketDataView(scenarioMarketData);

		// loop around measures, calculating all scenarios for one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = new java.util.HashMap<>();
		IDictionary<Measure, Result<object>> results = new Dictionary<Measure, Result<object>>();
		foreach (Measure measure in measures)
		{
		  results[measure] = calculate(measure, resolved, ledMarketData, optionMarketData);
		}
		return results;
	  }

	  // calculate one measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.bond.ResolvedBondFutureOptionTrade resolved, LegalEntityDiscountingScenarioMarketData ratesMarketData, BondFutureOptionScenarioMarketData optionMarketData)
	  private Result<object> calculate(Measure measure, ResolvedBondFutureOptionTrade resolved, LegalEntityDiscountingScenarioMarketData ratesMarketData, BondFutureOptionScenarioMarketData optionMarketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for BondFutureOption: {}", measure);
		}
		return Result.of(() => calculator(resolved, ratesMarketData, optionMarketData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedBondFutureOptionTrade resolved, LegalEntityDiscountingScenarioMarketData ratesMarketData, BondFutureOptionScenarioMarketData optionMarketData);

	}

}