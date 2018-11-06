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
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using IborFutureOptionVolatilities = com.opengamma.strata.pricer.index.IborFutureOptionVolatilities;
	using IborFutureOptionVolatilitiesId = com.opengamma.strata.pricer.index.IborFutureOptionVolatilitiesId;

	/// <summary>
	/// The lookup that provides access to Ibor future option volatilities in market data.
	/// <para>
	/// The Ibor future option market lookup provides access to the volatilities used to price Ibor future options.
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
	public interface IborFutureOptionMarketDataLookup : CalculationParameter
	{

	  /// <summary>
	  /// Obtains an instance based on a single mapping from index to volatility identifier.
	  /// <para>
	  /// The lookup provides volatilities for the specified index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="volatilityId">  the volatility identifier </param>
	  /// <returns> the Ibor future option lookup containing the specified mapping </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static IborFutureOptionMarketDataLookup of(com.opengamma.strata.basics.index.IborIndex index, com.opengamma.strata.pricer.index.IborFutureOptionVolatilitiesId volatilityId)
	//  {
	//	return DefaultIborFutureOptionMarketDataLookup.of(ImmutableMap.of(index, volatilityId));
	//  }

	  /// <summary>
	  /// Obtains an instance based on a map of volatility identifiers.
	  /// <para>
	  /// The map is used to specify the appropriate volatilities to use for each index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatilityIds">  the volatility identifiers, keyed by index </param>
	  /// <returns> the Ibor future option lookup containing the specified volatilities </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static IborFutureOptionMarketDataLookup of(java.util.Map<com.opengamma.strata.basics.index.IborIndex, com.opengamma.strata.pricer.index.IborFutureOptionVolatilitiesId> volatilityIds)
	//  {
	//	return DefaultIborFutureOptionMarketDataLookup.of(volatilityIds);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type that the lookup will be queried by.
	  /// <para>
	  /// This returns {@code IborFutureOptionMarketLookup.class}.
	  /// When querying parameters using <seealso cref="CalculationParameters#findParameter(Class)"/>,
	  /// {@code IborFutureOptionMarketLookup.class} must be passed in to find the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of the parameter implementation </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Class queryType()
	//  {
	//	return IborFutureOptionMarketDataLookup.class;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the set of indices that volatilities are provided for.
	  /// </summary>
	  /// <returns> the set of indices </returns>
	  ImmutableSet<IborIndex> VolatilityIndices {get;}

	  /// <summary>
	  /// Gets the identifiers used to obtain the volatilities for the specified currency.
	  /// <para>
	  /// The result will typically refer to a surface or cube.
	  /// If the index is not found, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index for which identifiers are required </param>
	  /// <returns> the set of market data identifiers </returns>
	  /// <exception cref="IllegalArgumentException"> if the index is not found </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getVolatilityIds(com.opengamma.strata.basics.index.IborIndex index);
	  ImmutableSet<MarketDataId<object>> getVolatilityIds(IborIndex index);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates market data requirements for the specified indices.
	  /// </summary>
	  /// <param name="indices">  the indices, for which volatilities are required </param>
	  /// <returns> the requirements </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.calc.runner.FunctionRequirements requirements(com.opengamma.strata.basics.index.IborIndex... indices)
	//  {
	//	return requirements(ImmutableSet.copyOf(indices));
	//  }

	  /// <summary>
	  /// Creates market data requirements for the specified indices.
	  /// </summary>
	  /// <param name="indices">  the indices, for which volatilities are required </param>
	  /// <returns> the requirements </returns>
	  FunctionRequirements requirements(ISet<IborIndex> indices);

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
//	  public default IborFutureOptionScenarioMarketData marketDataView(com.opengamma.strata.data.scenario.ScenarioMarketData marketData)
	//  {
	//	return DefaultIborFutureOptionScenarioMarketData.of(this, marketData);
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
//	  public default IborFutureOptionMarketData marketDataView(com.opengamma.strata.data.MarketData marketData)
	//  {
	//	return DefaultIborFutureOptionMarketData.of(this, marketData);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains Ibor future option volatilities based on the specified market data.
	  /// <para>
	  /// This provides <seealso cref="IborFutureOptionVolatilities"/> suitable for pricing an Ibor future option.
	  /// Although this method can be used directly, it is typically invoked indirectly
	  /// via <seealso cref="IborFutureOptionMarketData"/>:
	  /// <pre>
	  ///  // bind the baseData to this lookup
	  ///  IborFutureOptionMarketData view = lookup.marketDataView(baseData);
	  /// 
	  ///  // pass around IborFutureOptionMarketData within the function to use in pricing
	  ///  IborFutureOptionVolatilities vols = view.volatilities(index);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the volatilities </returns>
	  /// <exception cref="MarketDataNotFoundException"> if the index is not found </exception>
	  IborFutureOptionVolatilities volatilities(IborIndex index, MarketData marketData);

	}

}