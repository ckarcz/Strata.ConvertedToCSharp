using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// A market data function creates items of market data for a set of market data IDs.
	/// <para>
	/// A function implementation produces a single type of market data and consumes a single type of market data ID.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the market data built by this class </param>
	/// @param <I>  the type of the market data ID handled by this class </param>
	public interface MarketDataFunction<T, I>
	{

	  /// <summary>
	  /// Returns requirements representing the data needed to build the item of market data identified by the ID.
	  /// </summary>
	  /// <param name="id">  an ID identifying an item of market data </param>
	  /// <param name="marketDataConfig">  configuration specifying how market data values should be built </param>
	  /// <returns> requirements representing the data needed to build the item of market data identified by the ID </returns>
	  MarketDataRequirements requirements(I id, MarketDataConfig marketDataConfig);

	  /// <summary>
	  /// Builds and returns the market data identified by the ID.
	  /// <para>
	  /// If the data cannot be built the result contains details of the problem.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  ID of the market data that should be built </param>
	  /// <param name="marketDataConfig">  configuration specifying how the market data should be built </param>
	  /// <param name="marketData">  a set of market data including any data required to build the requested data </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> built market data, or details of the problems that prevented building </returns>
	  MarketDataBox<T> build(I id, MarketDataConfig marketDataConfig, ScenarioMarketData marketData, ReferenceData refData);

	  /// <summary>
	  /// Returns the type of market data ID this function can handle.
	  /// </summary>
	  /// <returns> the type of market data ID this function can handle </returns>
	  Type<I> MarketDataIdType {get;}
	}

}