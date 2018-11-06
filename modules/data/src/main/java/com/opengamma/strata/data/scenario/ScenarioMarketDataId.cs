using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{

	/// <summary>
	/// Market data identifier used by functions that need access to objects containing market data for multiple scenarios.
	/// <para>
	/// Many functions are written to calculate a single value at a time, for example a single present value for a trade.
	/// These functions need to consume single values of market data, for example a curve or a quoted price.
	/// </para>
	/// <para>
	/// However, it may be more efficient in some cases to calculate values for all scenarios at the same time.
	/// To do this efficiently it may be helpful to package the market data into a structure that is suitable for
	/// bulk calculations. Implementations of this interface allow these values to be requested.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of data referred to by the identifier </param>
	/// @param <U>  the type of the multi-scenario data </param>
	public interface ScenarioMarketDataId<T, U> where U : ScenarioArray<T>
	{

	  /// <summary>
	  /// Gets the market data identifier of the market data value.
	  /// </summary>
	  /// <returns> the market data identifier of the market data value </returns>
	  MarketDataId<T> MarketDataId {get;}

	  /// <summary>
	  /// Gets the type of the object containing the market data for all scenarios.
	  /// </summary>
	  /// <returns> the type of the object containing the market data for all scenarios </returns>
	  Type<U> ScenarioMarketDataType {get;}

	  /// <summary>
	  /// Creates an instance of the scenario market data object from a box containing data of the same underlying
	  /// type.
	  /// <para>
	  /// There are many possible ways to store scenario market data for a data type. For example, if the single
	  /// values are doubles, the scenario value might simply be a {@code List<Double>} or it might be a wrapper
	  /// class that stores the values more efficiently in a {@code double[]}.
	  /// </para>
	  /// <para>
	  /// This method allows a scenario value of the required type to be created from a different type of
	  /// scenario value or from a single value.
	  /// </para>
	  /// <para>
	  /// Normally this method will not be used. It is assumed the required scenario values will be created by the
	  /// perturbations that create scenario data. However there is no mechanism in the market data system to guarantee
	  /// that scenario values of a particular type are available. If they are not, this method creates them on demand.
	  /// </para>
	  /// <para>
	  /// Values returned from this method might be cached in the market data containers for efficiency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataBox">  a market data box containing single market data values of the same type as the
	  ///   scenario value identified by this key </param>
	  /// <param name="scenarioCount">  the number of scenarios for which data is required in the returned value </param>
	  /// <returns> an object containing market data for multiple scenarios built from the data in the market data box </returns>
	  U createScenarioValue(MarketDataBox<T> marketDataBox, int scenarioCount);

	}

}