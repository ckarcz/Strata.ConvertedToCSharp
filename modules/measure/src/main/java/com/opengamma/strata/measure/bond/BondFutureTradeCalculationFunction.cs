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
	using BondFuturePosition = com.opengamma.strata.product.bond.BondFuturePosition;
	using BondFutureTrade = com.opengamma.strata.product.bond.BondFutureTrade;
	using FixedCouponBond = com.opengamma.strata.product.bond.FixedCouponBond;
	using ResolvedBondFutureTrade = com.opengamma.strata.product.bond.ResolvedBondFutureTrade;

	/// <summary>
	/// Perform calculations on a single {@code BondFutureTrade} or {@code BondFuturePosition}
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
	///   <li><seealso cref="Measures#PAR_SPREAD Par spread"/>
	///   <li><seealso cref="Measures#CURRENCY_EXPOSURE Currency exposure"/>
	///   <li><seealso cref="Measures#RESOLVED_TARGET Resolved trade"/>
	/// </ul>
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bond futures in the trade model, pricers and market data.
	/// This is coherent with the pricing of <seealso cref="FixedCouponBond"/>. The bond futures delivery is a bond
	/// for an amount computed from the bond future price, a conversion factor and the accrued interest.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the trade or position type </param>
	public class BondFutureTradeCalculationFunction<T> : CalculationFunction<T> where T : com.opengamma.strata.product.SecuritizedProductPortfolioItem<com.opengamma.strata.product.bond.BondFuture>, com.opengamma.strata.basics.Resolvable<com.opengamma.strata.product.bond.ResolvedBondFutureTrade>
	{

	  /// <summary>
	  /// The trade instance
	  /// </summary>
	  public static readonly BondFutureTradeCalculationFunction<BondFutureTrade> TRADE = new BondFutureTradeCalculationFunction<BondFutureTrade>(typeof(BondFutureTrade));
	  /// <summary>
	  /// The position instance
	  /// </summary>
	  public static readonly BondFutureTradeCalculationFunction<BondFuturePosition> POSITION = new BondFutureTradeCalculationFunction<BondFuturePosition>(typeof(BondFuturePosition));

	  /// <summary>
	  /// The calculations by measure.
	  /// </summary>
	  private static readonly ImmutableMap<Measure, SingleMeasureCalculation> CALCULATORS = ImmutableMap.builder<Measure, SingleMeasureCalculation>().put(Measures.PRESENT_VALUE, BondFutureMeasureCalculations.DEFAULT.presentValue).put(Measures.PV01_CALIBRATED_SUM, BondFutureMeasureCalculations.DEFAULT.pv01CalibratedSum).put(Measures.PV01_CALIBRATED_BUCKETED, BondFutureMeasureCalculations.DEFAULT.pv01CalibratedBucketed).put(Measures.PV01_MARKET_QUOTE_SUM, BondFutureMeasureCalculations.DEFAULT.pv01MarketQuoteSum).put(Measures.PV01_MARKET_QUOTE_BUCKETED, BondFutureMeasureCalculations.DEFAULT.pv01MarketQuoteBucketed).put(Measures.UNIT_PRICE, BondFutureMeasureCalculations.DEFAULT.unitPrice).put(Measures.PAR_SPREAD, BondFutureMeasureCalculations.DEFAULT.parSpread).put(Measures.CURRENCY_EXPOSURE, BondFutureMeasureCalculations.DEFAULT.currencyExposure).put(Measures.RESOLVED_TARGET, (rt, smd) => rt).build();

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
	  private BondFutureTradeCalculationFunction(Type<T> targetType)
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
		BondFuture product = target.Product;
		QuoteId quoteId = QuoteId.of(product.SecurityId.StandardId, FieldName.SETTLEMENT_PRICE);
		Currency currency = product.Currency;

		// use lookup to build requirements
		FunctionRequirements freqs = FunctionRequirements.builder().valueRequirements(quoteId).outputCurrencies(currency).build();
		LegalEntityDiscountingMarketDataLookup ledLookup = parameters.getParameter(typeof(LegalEntityDiscountingMarketDataLookup));
		foreach (FixedCouponBond bond in product.DeliveryBasket)
		{
		  freqs = freqs.combinedWith(ledLookup.requirements(bond.SecurityId, bond.LegalEntityId, bond.Currency));
		}
		return freqs;
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(T target, java.util.Set<com.opengamma.strata.calc.Measure> measures, com.opengamma.strata.calc.runner.CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData scenarioMarketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(T target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData scenarioMarketData, ReferenceData refData)
	  {

		// resolve the trade once for all measures and all scenarios
		ResolvedBondFutureTrade resolved = target.resolve(refData);

		// use lookup to query market data
		LegalEntityDiscountingMarketDataLookup ledLookup = parameters.getParameter(typeof(LegalEntityDiscountingMarketDataLookup));
		LegalEntityDiscountingScenarioMarketData marketData = ledLookup.marketDataView(scenarioMarketData);

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
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculate(com.opengamma.strata.calc.Measure measure, com.opengamma.strata.product.bond.ResolvedBondFutureTrade resolved, LegalEntityDiscountingScenarioMarketData marketData)
	  private Result<object> calculate(Measure measure, ResolvedBondFutureTrade resolved, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		SingleMeasureCalculation calculator = CALCULATORS.get(measure);
		if (calculator == null)
		{
		  return Result.failure(FailureReason.UNSUPPORTED, "Unsupported measure for BondFuture: {}", measure);
		}
		return Result.of(() => calculator(resolved, marketData));
	  }

	  //-------------------------------------------------------------------------
	  delegate object SingleMeasureCalculation(ResolvedBondFutureTrade resolved, LegalEntityDiscountingScenarioMarketData marketData);

	}

}