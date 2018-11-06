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
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using BondFutureVolatilities = com.opengamma.strata.pricer.bond.BondFutureVolatilities;
	using BondFutureVolatilitiesId = com.opengamma.strata.pricer.bond.BondFutureVolatilitiesId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// The lookup that provides access to bond future volatilities in market data.
	/// <para>
	/// The bond future option market lookup provides access to the volatilities used to price bond future options.
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
	public interface BondFutureOptionMarketDataLookup : CalculationParameter
	{

	  /// <summary>
	  /// Obtains an instance based on a single mapping from security ID to volatility identifier.
	  /// <para>
	  /// The lookup provides volatilities for the specified security ID.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="securityId">  the security ID </param>
	  /// <param name="volatilityId">  the volatility identifier </param>
	  /// <returns> the bond future options lookup containing the specified mapping </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static BondFutureOptionMarketDataLookup of(com.opengamma.strata.product.SecurityId securityId, com.opengamma.strata.pricer.bond.BondFutureVolatilitiesId volatilityId)
	//  {
	//	return DefaultBondFutureOptionMarketDataLookup.of(ImmutableMap.of(securityId, volatilityId));
	//  }

	  /// <summary>
	  /// Obtains an instance based on a map of volatility identifiers.
	  /// <para>
	  /// The map is used to specify the appropriate volatilities to use for each security ID.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatilityIds">  the volatility identifiers, keyed by security ID </param>
	  /// <returns> the bond future options lookup containing the specified volatilities </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static BondFutureOptionMarketDataLookup of(java.util.Map<com.opengamma.strata.product.SecurityId, com.opengamma.strata.pricer.bond.BondFutureVolatilitiesId> volatilityIds)
	//  {
	//	return DefaultBondFutureOptionMarketDataLookup.of(volatilityIds);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type that the lookup will be queried by.
	  /// <para>
	  /// This returns {@code BondFutureOptionMarketLookup.class}.
	  /// When querying parameters using <seealso cref="CalculationParameters#findParameter(Class)"/>,
	  /// {@code BondFutureOptionMarketLookup.class} must be passed in to find the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of the parameter implementation </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Class queryType()
	//  {
	//	return BondFutureOptionMarketDataLookup.class;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the set of security IDs that volatilities are provided for.
	  /// </summary>
	  /// <returns> the set of security IDs </returns>
	  ImmutableSet<SecurityId> VolatilitySecurityIds {get;}

	  /// <summary>
	  /// Gets the identifiers used to obtain the volatilities for the specified security ID.
	  /// <para>
	  /// The result will typically refer to a surface or cube.
	  /// If the security ID is not found, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="securityId">  the security ID for which identifiers are required </param>
	  /// <returns> the set of market data identifiers </returns>
	  /// <exception cref="IllegalArgumentException"> if the security ID is not found </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getVolatilityIds(com.opengamma.strata.product.SecurityId securityId);
	  ImmutableSet<MarketDataId<object>> getVolatilityIds(SecurityId securityId);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates market data requirements for the specified security IDs.
	  /// </summary>
	  /// <param name="securityIds">  the security IDs, for which volatilities are required </param>
	  /// <returns> the requirements </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.calc.runner.FunctionRequirements requirements(com.opengamma.strata.product.SecurityId... securityIds)
	//  {
	//	return requirements(ImmutableSet.copyOf(securityIds));
	//  }

	  /// <summary>
	  /// Creates market data requirements for the specified security IDs.
	  /// </summary>
	  /// <param name="securityIds">  the security IDs, for which volatilities are required </param>
	  /// <returns> the requirements </returns>
	  FunctionRequirements requirements(ISet<SecurityId> securityIds);

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
//	  public default BondFutureOptionScenarioMarketData marketDataView(com.opengamma.strata.data.scenario.ScenarioMarketData marketData)
	//  {
	//	return DefaultBondFutureOptionScenarioMarketData.of(this, marketData);
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
//	  public default BondFutureOptionMarketData marketDataView(com.opengamma.strata.data.MarketData marketData)
	//  {
	//	return DefaultBondFutureOptionMarketData.of(this, marketData);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains bond future volatilities based on the specified market data.
	  /// <para>
	  /// This provides <seealso cref="BondFutureVolatilities"/> suitable for pricing bond future options.
	  /// Although this method can be used directly, it is typically invoked indirectly
	  /// via <seealso cref="BondFutureOptionMarketData"/>:
	  /// <pre>
	  ///  // bind the baseData to this lookup
	  ///  BondFutureOptionMarketData view = lookup.marketDataView(baseData);
	  /// 
	  ///  // pas around BondFutureOptionMarketData within the function to use in pricing
	  ///  BondFutureVolatilities vols = view.volatilities(securityId);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="securityId">  the security ID </param>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the volatilities </returns>
	  /// <exception cref="MarketDataNotFoundException"> if the security ID is not found </exception>
	  BondFutureVolatilities volatilities(SecurityId securityId, MarketData marketData);

	}

}