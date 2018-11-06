/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.rate
{
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Market data for rates products, used for calculation across multiple scenarios.
	/// <para>
	/// This interface exposes the market data necessary for pricing rates products,
	/// such as Swaps, FRAs and FX.
	/// It uses a <seealso cref="RatesMarketDataLookup"/> to provide a view on <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface RatesScenarioMarketData
	{

	  /// <summary>
	  /// Gets the lookup that provides access to discount curves and forward curves.
	  /// </summary>
	  /// <returns> the rates lookup </returns>
	  RatesMarketDataLookup Lookup {get;}

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
	  RatesScenarioMarketData withMarketData(ScenarioMarketData marketData);

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
	  RatesMarketData scenario(int scenarioIndex);

	}

}