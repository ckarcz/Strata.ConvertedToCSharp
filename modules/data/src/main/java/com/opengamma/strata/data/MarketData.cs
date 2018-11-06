using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{

	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Messages = com.opengamma.strata.collect.Messages;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// Provides access to market data, such as curves, surfaces and time-series.
	/// <para>
	/// Market data is looked up using subclasses of <seealso cref="MarketDataId"/>.
	/// All data is valid for a single date, defined by <seealso cref="#getValuationDate()"/>.
	/// When performing calculations with scenarios, only the data of a single scenario is accessible.
	/// </para>
	/// <para>
	/// The standard implementation is <seealso cref="ImmutableMarketData"/>.
	/// </para>
	/// </summary>
	public interface MarketData
	{

	  /// <summary>
	  /// Obtains an instance from a valuation date and map of values.
	  /// <para>
	  /// Each entry in the map is a single piece of market data, keyed by the matching identifier.
	  /// For example, an <seealso cref="FxRate"/> can be looked up using an <seealso cref="FxRateId"/>.
	  /// The caller must ensure that the each entry in the map corresponds with the parameterized
	  /// type on the identifier.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date of the market data </param>
	  /// <param name="values">  the market data values </param>
	  /// <returns> the market data instance containing the values in the map </returns>
	  /// <exception cref="ClassCastException"> if a value does not match the parameterized type associated with the identifier </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static MarketData of(java.time.LocalDate valuationDate, java.util.Map<JavaToDotNetGenericWildcard extends MarketDataId<JavaToDotNetGenericWildcard>, JavaToDotNetGenericWildcard> values)
	//  {
	//	return ImmutableMarketData.of(valuationDate, values);
	//  }

	  /// <summary>
	  /// Obtains an instance from a valuation date, map of values and time-series.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date of the market data </param>
	  /// <param name="values">  the market data values </param>
	  /// <param name="timeSeries">  the time-series </param>
	  /// <returns> the market data instance containing the values in the map and the time-series </returns>
	  /// <exception cref="ClassCastException"> if a value does not match the parameterized type associated with the identifier </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static MarketData of(java.time.LocalDate valuationDate, java.util.Map<JavaToDotNetGenericWildcard extends MarketDataId<JavaToDotNetGenericWildcard>, JavaToDotNetGenericWildcard> values, java.util.Map<JavaToDotNetGenericWildcard extends ObservableId, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries> timeSeries)
	//  {
	//
	//	return ImmutableMarketData.builder(valuationDate).values(values).timeSeries(timeSeries).build();
	//  }

	  /// <summary>
	  /// Obtains an instance containing no market data.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date of the market data </param>
	  /// <returns> empty market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static MarketData empty(java.time.LocalDate valuationDate)
	//  {
	//	return ImmutableMarketData.builder(valuationDate).build();
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date of the market data.
	  /// <para>
	  /// All values accessible through this interface have the same valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the valuation date </returns>
	  LocalDate ValuationDate {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this market data contains a value for the specified identifier.
	  /// </summary>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> true if the market data contains a value for the identifier </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean containsValue(MarketDataId<JavaToDotNetGenericWildcard> id)
	//  {
	//	return findValue(id).isPresent();
	//  }

	  /// <summary>
	  /// Gets the market data value associated with the specified identifier.
	  /// <para>
	  /// If this market data instance contains the identifier, the value will be returned.
	  /// Otherwise, an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the market data value </returns>
	  /// <exception cref="MarketDataNotFoundException"> if the identifier is not found </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T> T getValue(MarketDataId<T> id)
	//  {
	//	return findValue(id).orElseThrow(() -> new MarketDataNotFoundException(Messages.format("Market data not found for '{}' of type '{}'", id, id.getClass().getSimpleName())));
	//  }

	  /// <summary>
	  /// Finds the market data value associated with the specified identifier.
	  /// <para>
	  /// If this market data instance contains the identifier, the value will be returned.
	  /// Otherwise, an empty optional will be returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the market data value, empty if not found </returns>
	  Optional<T> findValue<T>(MarketDataId<T> id);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data identifiers.
	  /// </summary>
	  /// <returns> the set of market data identifiers </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract java.util.Set<MarketDataId<?>> getIds();
	  ISet<MarketDataId<object>> Ids {get;}

	  /// <summary>
	  /// Finds the market data identifiers associated with the specified name.
	  /// <para>
	  /// This returns the unique identifiers that refer to the specified name.
	  /// There may be more than one identifier associated with a name as the name is not unique.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="name">  the name to find </param>
	  /// <returns> the set of market data identifiers, empty if name not found </returns>
	  ISet<MarketDataId<T>> findIds<T>(MarketDataName<T> name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time-series identifiers.
	  /// </summary>
	  /// <returns> the set of observable identifiers </returns>
	  ISet<ObservableId> TimeSeriesIds {get;}

	  /// <summary>
	  /// Gets the time-series identified by the specified identifier, empty if not found.
	  /// </summary>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the time-series, empty if no time-series found </returns>
	  LocalDateDoubleTimeSeries getTimeSeries(ObservableId id);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this market data with another.
	  /// <para>
	  /// The result combines both sets of market data.
	  /// Values are taken from this set of market data if available, otherwise they are taken
	  /// from the other set.
	  /// </para>
	  /// <para>
	  /// The valuation dates of the sets of market data must be the same.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other market data </param>
	  /// <returns> the combined market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default MarketData combinedWith(MarketData other)
	//  {
	//	return new CombinedMarketData(this, other);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this market data with the specified value.
	  /// <para>
	  /// When the result is queried for the specified identifier, the specified value will be returned.
	  /// </para>
	  /// <para>
	  /// For example, this method could be used to replace a curve with a bumped curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="id">  the identifier to find </param>
	  /// <param name="value">  the value to associate with the identifier </param>
	  /// <returns> the derived market data with the specified identifier and value </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T> MarketData withValue(MarketDataId<T> id, T value)
	//  {
	//	return ExtendedMarketData.of(id, value, this);
	//  }

	}

}