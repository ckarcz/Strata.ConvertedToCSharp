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
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using NamedMarketDataId = com.opengamma.strata.data.NamedMarketDataId;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioPerturbation = com.opengamma.strata.data.scenario.ScenarioPerturbation;

	/// <summary>
	/// Encapsulates a rule or set of rules to decide whether a perturbation applies to a piece of market data.
	/// <para>
	/// A <seealso cref="ScenarioPerturbation"/> encapsulates a specific change to a piece of market data, such as a parallel shift.
	/// An implementation of this filter interface defines when the perturbation should be used.
	/// For example, a filter could apply to all yield curves whose currency is USD, or quoted prices
	/// of equity securities in the energy sector.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the market data handled by the filter </param>
	/// @param <I>  the type of the market data ID handled by the filter </param>
	public interface MarketDataFilter<T, I> where I : com.opengamma.strata.data.MarketDataId<T>
	{

	  /// <summary>
	  /// Obtains a filter that matches any value with the specified identifier type.
	  /// </summary>
	  /// @param <T>  the type of market data handled by the filter </param>
	  /// <param name="type">  the type that is matched by this filter </param>
	  /// <returns> a filter matching the specified type </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> MarketDataFilter<T, com.opengamma.strata.data.MarketDataId<T>> ofIdType(Class type)
	//  {
	//	return new IdTypeFilter<T>(type);
	//  }

	  /// <summary>
	  /// Obtains a filter that matches the specified identifier.
	  /// </summary>
	  /// @param <T>  the type of market data handled by the filter </param>
	  /// <param name="id">  the identifier that is matched by this filter </param>
	  /// <returns> a filter matching the specified identifier </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> MarketDataFilter<T, com.opengamma.strata.data.MarketDataId<T>> ofId(com.opengamma.strata.data.MarketDataId<T> id)
	//  {
	//	return new IdFilter<T>(id);
	//  }

	  /// <summary>
	  /// Obtains a filter that matches the specified name.
	  /// </summary>
	  /// @param <T>  the type of market data handled by the filter </param>
	  /// <param name="name">  the name that is matched by this filter </param>
	  /// <returns> a filter matching the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> MarketDataFilter<T, com.opengamma.strata.data.NamedMarketDataId<T>> ofName(com.opengamma.strata.data.MarketDataName<T> name)
	//  {
	//	return new NameFilter<T>(name);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the type of market data ID handled by this filter.
	  /// <para>
	  /// This should correspond to the type parameter {@code I}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of market data ID handled by this filter </returns>
	  Type MarketDataIdType {get;}

	  /// <summary>
	  /// Applies the filter to a market data ID and the corresponding market data value
	  /// and returns true if the filter matches.
	  /// </summary>
	  /// <param name="marketDataId">  the ID of a piece of market data </param>
	  /// <param name="marketData">  the market data value </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> true if the filter matches </returns>
	  bool matches(I marketDataId, MarketDataBox<T> marketData, ReferenceData refData);

	}

}