using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.capfloor
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
	using IborCapletFloorletVolatilities = com.opengamma.strata.pricer.capfloor.IborCapletFloorletVolatilities;
	using IborCapletFloorletVolatilitiesId = com.opengamma.strata.pricer.capfloor.IborCapletFloorletVolatilitiesId;

	/// <summary>
	/// The lookup that provides access to cap/floor volatilities in market data.
	/// <para>
	/// The cap/floor market lookup provides access to the volatilities used to price cap/floors.
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
	public interface IborCapFloorMarketDataLookup : CalculationParameter
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
	  /// <returns> the cap/floor lookup containing the specified mapping </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static IborCapFloorMarketDataLookup of(com.opengamma.strata.basics.index.IborIndex index, com.opengamma.strata.pricer.capfloor.IborCapletFloorletVolatilitiesId volatilityId)
	//  {
	//	return DefaultIborCapFloorMarketDataLookup.of(ImmutableMap.of(index, volatilityId));
	//  }

	  /// <summary>
	  /// Obtains an instance based on a map of volatility identifiers.
	  /// <para>
	  /// The map is used to specify the appropriate volatilities to use for each index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatilityIds">  the volatility identifiers, keyed by index </param>
	  /// <returns> the cap/floor lookup containing the specified volatilities </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static IborCapFloorMarketDataLookup of(java.util.Map<com.opengamma.strata.basics.index.IborIndex, com.opengamma.strata.pricer.capfloor.IborCapletFloorletVolatilitiesId> volatilityIds)
	//  {
	//	return DefaultIborCapFloorMarketDataLookup.of(volatilityIds);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type that the lookup will be queried by.
	  /// <para>
	  /// This returns {@code CapFloorMarketLookup.class}.
	  /// When querying parameters using <seealso cref="CalculationParameters#findParameter(Class)"/>,
	  /// {@code CapFloorMarketLookup.class} must be passed in to find the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of the parameter implementation </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Class queryType()
	//  {
	//	return IborCapFloorMarketDataLookup.class;
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
//	  public default IborCapFloorScenarioMarketData marketDataView(com.opengamma.strata.data.scenario.ScenarioMarketData marketData)
	//  {
	//	return DefaultIborCapFloorScenarioMarketData.of(this, marketData);
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
//	  public default IborCapFloorMarketData marketDataView(com.opengamma.strata.data.MarketData marketData)
	//  {
	//	return DefaultIborCapFloorMarketData.of(this, marketData);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains cap/floor volatilities based on the specified market data.
	  /// <para>
	  /// This provides <seealso cref="IborCapletFloorletVolatilities"/> suitable for pricing a cap/floor.
	  /// Although this method can be used directly, it is typically invoked indirectly
	  /// via <seealso cref="IborCapFloorMarketData"/>:
	  /// <pre>
	  ///  // bind the baseData to this lookup
	  ///  IborCapFloorMarketData view = lookup.marketDataView(baseData);
	  /// 
	  ///  // pass around CapFloorMarketData within the function to use in pricing
	  ///  IborCapletFloorletVolatilities vols = view.volatilities(index);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the volatilities </returns>
	  /// <exception cref="MarketDataNotFoundException"> if the index is not found </exception>
	  IborCapletFloorletVolatilities volatilities(IborIndex index, MarketData marketData);

	}

}