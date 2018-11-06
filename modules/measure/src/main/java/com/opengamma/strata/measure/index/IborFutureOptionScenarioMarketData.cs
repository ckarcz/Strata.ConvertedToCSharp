/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.index
{
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Market data for Ibor future options, used for calculation across multiple scenarios.
	/// <para>
	/// This interface exposes the market data necessary for pricing an Ibor future option.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface IborFutureOptionScenarioMarketData
	{

	  /// <summary>
	  /// Gets the lookup that provides access to Ibor future option volatilities.
	  /// </summary>
	  /// <returns> the Ibor future option lookup </returns>
	  IborFutureOptionMarketDataLookup Lookup {get;}

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
	  IborFutureOptionScenarioMarketData withMarketData(ScenarioMarketData marketData);

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
	  IborFutureOptionMarketData scenario(int scenarioIndex);

	}

}