/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Market data for bond future options, used for calculation across multiple scenarios.
	/// <para>
	/// This interface exposes the market data necessary for pricing bond future options.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface BondFutureOptionScenarioMarketData
	{

	  /// <summary>
	  /// Gets the lookup that provides access to bond future volatilities.
	  /// </summary>
	  /// <returns> the bond future options lookup </returns>
	  BondFutureOptionMarketDataLookup Lookup {get;}

	  /// <summary>
	  /// Gets the market data.
	  /// </summary>
	  /// <returns> the market data </returns>
	  ScenarioMarketData MarketData {get;}

	  /// <summary>
	  /// Returns a copy of this instance with the specified market data.
	  /// </summary>
	  /// <param name="marketData">  the market data to use </param>
	  /// <returns> a market view based on the specified data </returns>
	  BondFutureOptionScenarioMarketData withMarketData(ScenarioMarketData marketData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of scenarios.
	  /// </summary>
	  /// <returns> the number of scenarios </returns>
	  int ScenarioCount {get;}

	  /// <summary>
	  /// Returns market data for a single scenario.
	  /// <para>
	  /// This returns a view of the market data for the specified scenario.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scenarioIndex">  the scenario index </param>
	  /// <returns> the market data for the specified scenario </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the scenario index is invalid </exception>
	  BondFutureOptionMarketData scenario(int scenarioIndex);

	}

}