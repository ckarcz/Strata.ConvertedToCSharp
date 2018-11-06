using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.rate
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using Index = com.opengamma.strata.basics.index.Index;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using FxRateLookup = com.opengamma.strata.calc.runner.FxRateLookup;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveGroupEntry = com.opengamma.strata.market.curve.RatesCurveGroupEntry;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// The lookup that provides access to rates in market data.
	/// <para>
	/// The rates market lookup provides access to discount curves and forward curves.
	/// This includes Ibor index rates, Overnight index rates, Price index rates,
	/// FX rates and discounting.
	/// </para>
	/// <para>
	/// The lookup implements <seealso cref="CalculationParameter"/> and is used by passing it
	/// as an argument to <seealso cref="CalculationRules"/>. It provides the link between the
	/// data that the function needs and the data that is available in <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface RatesMarketDataLookup : FxRateLookup, CalculationParameter
	{

	  /// <summary>
	  /// Obtains an instance based on a map of discount and forward curve identifiers.
	  /// <para>
	  /// The discount and forward curves refer to the curve identifier.
	  /// The curves themselves are provided in <seealso cref="ScenarioMarketData"/>
	  /// using <seealso cref="CurveId"/> as the identifier.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="discountCurveIds">  the discount curve identifiers, keyed by currency </param>
	  /// <param name="forwardCurveIds">  the forward curves identifiers, keyed by index </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RatesMarketDataLookup of(java.util.Map<com.opengamma.strata.basics.currency.Currency, com.opengamma.strata.market.curve.CurveId> discountCurveIds, java.util.Map<com.opengamma.strata.basics.index.Index, com.opengamma.strata.market.curve.CurveId> forwardCurveIds)
	//  {
	//
	//	return DefaultRatesMarketDataLookup.of(discountCurveIds, forwardCurveIds, ObservableSource.NONE, FxRateLookup.ofRates());
	//  }

	  /// <summary>
	  /// Obtains an instance based on a map of discount and forward curve identifiers,
	  /// specifying the source of FX rates.
	  /// <para>
	  /// The discount and forward curves refer to the curve identifier.
	  /// The curves themselves are provided in <seealso cref="ScenarioMarketData"/>
	  /// using <seealso cref="CurveId"/> as the identifier.
	  /// The source of market data is rarely needed, as most applications use only one
	  /// underlying data source.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="discountCurveIds">  the discount curve identifiers, keyed by currency </param>
	  /// <param name="forwardCurveIds">  the forward curves identifiers, keyed by index </param>
	  /// <param name="obsSource">  the source of market data for quotes and other observable market data </param>
	  /// <param name="fxLookup">  the lookup used to obtain FX rates </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RatesMarketDataLookup of(java.util.Map<com.opengamma.strata.basics.currency.Currency, com.opengamma.strata.market.curve.CurveId> discountCurveIds, java.util.Map<com.opengamma.strata.basics.index.Index, com.opengamma.strata.market.curve.CurveId> forwardCurveIds, com.opengamma.strata.data.ObservableSource obsSource, com.opengamma.strata.calc.runner.FxRateLookup fxLookup)
	//  {
	//
	//	return DefaultRatesMarketDataLookup.of(discountCurveIds, forwardCurveIds, obsSource, fxLookup);
	//  }

	  /// <summary>
	  /// Obtains an instance based on a group of discount and forward curves.
	  /// <para>
	  /// The discount and forward curves refer to the curve name.
	  /// The curves themselves are provided in <seealso cref="ScenarioMarketData"/>
	  /// using <seealso cref="CurveId"/> as the identifier.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupName">  the curve group name </param>
	  /// <param name="discountCurves">  the discount curves, keyed by currency </param>
	  /// <param name="forwardCurves">  the forward curves, keyed by index </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RatesMarketDataLookup of(com.opengamma.strata.market.curve.CurveGroupName groupName, java.util.Map<com.opengamma.strata.basics.currency.Currency, com.opengamma.strata.market.curve.CurveName> discountCurves, java.util.Map<JavaToDotNetGenericWildcard extends com.opengamma.strata.basics.index.Index, com.opengamma.strata.market.curve.CurveName> forwardCurves)
	//  {
	//
	//	Map<Currency, CurveId> discountCurveIds = MapStream.of(discountCurves).mapValues(c -> CurveId.of(groupName, c)).toMap();
	//	Map<? extends Index, CurveId> forwardCurveIds = MapStream.of(forwardCurves).mapValues(c -> CurveId.of(groupName, c)).toMap();
	//	return DefaultRatesMarketDataLookup.of(discountCurveIds, forwardCurveIds, ObservableSource.NONE, FxRateLookup.ofRates());
	//  }

	  /// <summary>
	  /// Obtains an instance based on a curve group.
	  /// <para>
	  /// The discount curves and forward curves from the group are extracted and used to build the lookup.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveGroup">  the curve group to base the lookup on </param>
	  /// <returns> the rates lookup based on the specified group </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RatesMarketDataLookup of(com.opengamma.strata.market.curve.RatesCurveGroup curveGroup)
	//  {
	//	CurveGroupName groupName = curveGroup.getName();
	//	Map<Currency, CurveId> discountCurves = MapStream.of(curveGroup.getDiscountCurves()).mapValues(c -> CurveId.of(groupName, c.getName())).toMap();
	//	Map<Index, CurveId> forwardCurves = MapStream.of(curveGroup.getForwardCurves()).mapValues(c -> CurveId.of(groupName, c.getName())).toMap();
	//	return DefaultRatesMarketDataLookup.of(discountCurves, forwardCurves, ObservableSource.NONE, FxRateLookup.ofRates());
	//  }

	  /// <summary>
	  /// Obtains an instance based on a curve group definition.
	  /// <para>
	  /// The discount curves and forward curves from the group are extracted and used to build the lookup.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveGroupDefinition">  the curve group to base the lookup on </param>
	  /// <returns> the rates lookup based on the specified group </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RatesMarketDataLookup of(com.opengamma.strata.market.curve.RatesCurveGroupDefinition curveGroupDefinition)
	//  {
	//	CurveGroupName groupName = curveGroupDefinition.getName();
	//	Map<Currency, CurveId> discountCurves = new HashMap<>();
	//	Map<Index, CurveId> forwardCurves = new HashMap<>();
	//	for (RatesCurveGroupEntry entry : curveGroupDefinition.getEntries())
	//	{
	//	  CurveId curveId = CurveId.of(groupName, entry.getCurveName());
	//	  entry.getDiscountCurrencies().forEach(ccy -> discountCurves.put(ccy, curveId));
	//	  entry.getIndices().forEach(idx -> forwardCurves.put(idx, curveId));
	//	}
	//	return DefaultRatesMarketDataLookup.of(discountCurves, forwardCurves, ObservableSource.NONE, FxRateLookup.ofRates());
	//  }

	  /// <summary>
	  /// Obtains an instance based on a curve group definition.
	  /// <para>
	  /// The discount curves and forward curves from the group are extracted and used to build the lookup.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveGroupDefinition">  the curve group to base the lookup on </param>
	  /// <param name="observableSource">  the source of market data for quotes and other observable market data </param>
	  /// <param name="fxLookup">  the lookup used to obtain FX rates </param>
	  /// <returns> the rates lookup based on the specified group </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RatesMarketDataLookup of(com.opengamma.strata.market.curve.RatesCurveGroupDefinition curveGroupDefinition, com.opengamma.strata.data.ObservableSource observableSource, com.opengamma.strata.calc.runner.FxRateLookup fxLookup)
	//  {
	//
	//	CurveGroupName groupName = curveGroupDefinition.getName();
	//	Map<Currency, CurveId> discountCurves = new HashMap<>();
	//	Map<Index, CurveId> forwardCurves = new HashMap<>();
	//	for (RatesCurveGroupEntry entry : curveGroupDefinition.getEntries())
	//	{
	//	  CurveId curveId = CurveId.of(groupName, entry.getCurveName());
	//	  entry.getDiscountCurrencies().forEach(ccy -> discountCurves.put(ccy, curveId));
	//	  entry.getIndices().forEach(idx -> forwardCurves.put(idx, curveId));
	//	}
	//	return DefaultRatesMarketDataLookup.of(discountCurves, forwardCurves, observableSource, fxLookup);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type that the lookup will be queried by.
	  /// <para>
	  /// This returns {@code RatesMarketLookup.class}.
	  /// When querying parameters using <seealso cref="CalculationParameters#findParameter(Class)"/>,
	  /// {@code RatesMarketLookup.class} must be passed in to find the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of the parameter implementation </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Class queryType()
	//  {
	//	return RatesMarketDataLookup.class;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the set of currencies that discount factors are provided for.
	  /// </summary>
	  /// <returns> the set of discount curve currencies </returns>
	  ImmutableSet<Currency> DiscountCurrencies {get;}

	  /// <summary>
	  /// Gets the identifiers used to obtain the discount factors for the specified currency.
	  /// <para>
	  /// In most cases, the identifier will refer to a curve.
	  /// If the currency is not found, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency for which identifiers are required </param>
	  /// <returns> the set of market data identifiers </returns>
	  /// <exception cref="IllegalArgumentException"> if the currency is not found </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getDiscountMarketDataIds(com.opengamma.strata.basics.currency.Currency currency);
	  ImmutableSet<MarketDataId<object>> getDiscountMarketDataIds(Currency currency);

	  /// <summary>
	  /// Gets the set of indices that forward rates are provided for.
	  /// </summary>
	  /// <returns> the set of forward curve indices </returns>
	  ImmutableSet<Index> ForwardIndices {get;}

	  /// <summary>
	  /// Gets the identifiers used to obtain the forward rates for the specified index.
	  /// <para>
	  /// In most cases, the identifier will refer to a curve.
	  /// If the index is not found, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index for which identifiers are required </param>
	  /// <returns> the set of market data identifiers </returns>
	  /// <exception cref="IllegalArgumentException"> if the index is not found </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getForwardMarketDataIds(com.opengamma.strata.basics.index.Index index);
	  ImmutableSet<MarketDataId<object>> getForwardMarketDataIds(Index index);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates market data requirements for the specified currencies.
	  /// <para>
	  /// This is used when discount factors are required, but forward curves are not.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencies">  the currencies, for which discount factors will be needed </param>
	  /// <returns> the requirements </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to create requirements </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.calc.runner.FunctionRequirements requirements(java.util.Set<com.opengamma.strata.basics.currency.Currency> currencies)
	//  {
	//	return requirements(currencies, ImmutableSet.of());
	//  }

	  /// <summary>
	  /// Creates market data requirements for the specified currency and indices.
	  /// </summary>
	  /// <param name="currency">  the currency, for which discount factors are needed </param>
	  /// <param name="indices">  the indices, for which forward curves and time-series will be needed </param>
	  /// <returns> the requirements </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to create requirements </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.calc.runner.FunctionRequirements requirements(com.opengamma.strata.basics.currency.Currency currency, com.opengamma.strata.basics.index.Index... indices)
	//  {
	//	return requirements(ImmutableSet.of(currency), ImmutableSet.copyOf(indices));
	//  }

	  /// <summary>
	  /// Creates market data requirements for the specified currencies and indices.
	  /// </summary>
	  /// <param name="currencies">  the currencies, for which discount factors will be needed </param>
	  /// <param name="indices">  the indices, for which forward curves and time-series will be needed </param>
	  /// <returns> the requirements </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to create requirements </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract com.opengamma.strata.calc.runner.FunctionRequirements requirements(java.util.Set<com.opengamma.strata.basics.currency.Currency> currencies, java.util.Set<? extends com.opengamma.strata.basics.index.Index> indices);
	  FunctionRequirements requirements<T1>(ISet<Currency> currencies, ISet<T1> indices);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a filtered view of the complete set of market data.
	  /// <para>
	  /// This method returns an instance that binds the lookup to the market data.
	  /// The input is <seealso cref="ScenarioMarketData"/>, which contains market data for all scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for all scenarios </param>
	  /// <returns> the filtered market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default RatesScenarioMarketData marketDataView(com.opengamma.strata.data.scenario.ScenarioMarketData marketData)
	//  {
	//	return DefaultRatesScenarioMarketData.of(this, marketData);
	//  }

	  /// <summary>
	  /// Obtains a filtered view of the complete set of market data.
	  /// <para>
	  /// This method returns an instance that binds the lookup to the market data.
	  /// The input is <seealso cref="MarketData"/>, which contains market data for one scenario.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the filtered market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default RatesMarketData marketDataView(com.opengamma.strata.data.MarketData marketData)
	//  {
	//	return DefaultRatesMarketData.of(this, marketData);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a rates provider based on the specified market data.
	  /// <para>
	  /// This provides a <seealso cref="RatesProvider"/> suitable for pricing a rates product.
	  /// Although this method can be used directly, it is typically invoked indirectly
	  /// via <seealso cref="RatesMarketData"/>:
	  /// <pre>
	  ///  // bind the baseData to this lookup
	  ///  RatesMarketData view = lookup.marketView(baseData);
	  /// 
	  ///  // pass around RatesMarketData within the function to use in pricing
	  ///  RatesProvider provider = view.ratesProvider();
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the rates provider </returns>
	  RatesProvider ratesProvider(MarketData marketData);

	  /// <summary>
	  /// Obtains an FX rate provider based on the specified market data.
	  /// <para>
	  /// This provides an <seealso cref="FxRateProvider"/> suitable for obtaining FX rates.
	  /// Although this method can be used directly, it is typically invoked indirectly
	  /// via <seealso cref="RatesMarketData"/>:
	  /// <pre>
	  ///  // bind the baseData to this lookup
	  ///  RatesMarketData view = lookup.marketView(baseData);
	  /// 
	  ///  // pass around RatesMarketData within the function to use in pricing
	  ///  RatesProvider provider = view.fxRateProvider();
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the FX rate provider </returns>
	  FxRateProvider fxRateProvider(MarketData marketData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the observable source.
	  /// </summary>
	  /// <returns> the observable source </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.data.ObservableSource getObservableSource()
	//  {
	//	return ObservableSource.NONE;
	//  }

	  /// <summary>
	  /// Gets the underlying FX lookup.
	  /// </summary>
	  /// <returns> the underlying FX lookup </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.calc.runner.FxRateLookup getFxRateLookup()
	//  {
	//	return this;
	//  }

	}

}