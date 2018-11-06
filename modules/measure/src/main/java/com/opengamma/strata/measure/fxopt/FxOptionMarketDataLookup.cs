using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using FxOptionVolatilities = com.opengamma.strata.pricer.fxopt.FxOptionVolatilities;
	using FxOptionVolatilitiesId = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesId;

	/// <summary>
	/// The lookup that provides access to FX options volatilities in market data.
	/// <para>
	/// The FX options market lookup provides access to the volatilities used to price FX options.
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
	public interface FxOptionMarketDataLookup : CalculationParameter
	{

	  /// <summary>
	  /// Obtains an instance based on a single mapping from currency pair to volatility identifier.
	  /// <para>
	  /// The lookup provides volatilities for the specified currency pair.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="volatilityId">  the volatility identifier </param>
	  /// <returns> the FX options lookup containing the specified mapping </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FxOptionMarketDataLookup of(com.opengamma.strata.basics.currency.CurrencyPair currencyPair, com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesId volatilityId)
	//  {
	//	return DefaultFxOptionMarketDataLookup.of(ImmutableMap.of(currencyPair, volatilityId));
	//  }

	  /// <summary>
	  /// Obtains an instance based on a map of volatility identifiers.
	  /// <para>
	  /// The map is used to specify the appropriate volatilities to use for each currency pair.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatilityIds">  the volatility identifiers, keyed by currency pair </param>
	  /// <returns> the FX options lookup containing the specified volatilities </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FxOptionMarketDataLookup of(java.util.Map<com.opengamma.strata.basics.currency.CurrencyPair, com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesId> volatilityIds)
	//  {
	//	return DefaultFxOptionMarketDataLookup.of(volatilityIds);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type that the lookup will be queried by.
	  /// <para>
	  /// This returns {@code FxOptionMarketLookup.class}.
	  /// When querying parameters using <seealso cref="CalculationParameters#findParameter(Class)"/>,
	  /// {@code FxOptionMarketLookup.class} must be passed in to find the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of the parameter implementation </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Class queryType()
	//  {
	//	return FxOptionMarketDataLookup.class;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the set of currency pairs that volatilities are provided for.
	  /// </summary>
	  /// <returns> the set of currency pairs </returns>
	  ImmutableSet<CurrencyPair> VolatilityCurrencyPairs {get;}

	  /// <summary>
	  /// Gets the identifiers used to obtain the volatilities for the specified currency pair.
	  /// <para>
	  /// The result will typically refer to a surface or cube.
	  /// If the currency pair is not found, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair for which identifiers are required </param>
	  /// <returns> the set of market data identifiers </returns>
	  /// <exception cref="IllegalArgumentException"> if the currency pair is not found </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getVolatilityIds(com.opengamma.strata.basics.currency.CurrencyPair currencyPair);
	  ImmutableSet<MarketDataId<object>> getVolatilityIds(CurrencyPair currencyPair);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates market data requirements for the specified currency pairs.
	  /// </summary>
	  /// <param name="currencyPairs">  the currency pairs, for which volatilities are required </param>
	  /// <returns> the requirements </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.calc.runner.FunctionRequirements requirements(com.opengamma.strata.basics.currency.CurrencyPair... currencyPairs)
	//  {
	//	return requirements(ImmutableSet.copyOf(currencyPairs));
	//  }

	  /// <summary>
	  /// Creates market data requirements for the specified currency pairs.
	  /// </summary>
	  /// <param name="currencyPairs">  the currency pairs, for which volatilities are required </param>
	  /// <returns> the requirements </returns>
	  FunctionRequirements requirements(ISet<CurrencyPair> currencyPairs);

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
//	  public default FxOptionScenarioMarketData marketDataView(com.opengamma.strata.data.scenario.ScenarioMarketData marketData)
	//  {
	//	return DefaultFxOptionScenarioMarketData.of(this, marketData);
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
//	  public default FxOptionMarketData marketDataView(com.opengamma.strata.data.MarketData marketData)
	//  {
	//	return DefaultFxOptionMarketData.of(this, marketData);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains FX options volatilities based on the specified market data.
	  /// <para>
	  /// This provides <seealso cref="FxOptionVolatilities"/> suitable for pricing FX options.
	  /// Although this method can be used directly, it is typically invoked indirectly
	  /// via <seealso cref="FxOptionMarketData"/>:
	  /// <pre>
	  ///  // bind the baseData to this lookup
	  ///  FxOptionMarketData view = lookup.marketDataView(baseData);
	  /// 
	  ///  // pas around FxOptionMarketData within the function to use in pricing
	  ///  FxOptionVolatilities vols = view.volatilities(currencyPair);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the volatilities </returns>
	  /// <exception cref="MarketDataNotFoundException"> if the currency pair is not found </exception>
	  FxOptionVolatilities volatilities(CurrencyPair currencyPair, MarketData marketData);

	}

}