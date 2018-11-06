/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
	using Result = com.opengamma.strata.collect.result.Result;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ObservableId = com.opengamma.strata.data.ObservableId;

	/// <summary>
	/// A provider of time-series.
	/// <para>
	/// This plugin point allows a market data supplier of time-series to be provided.
	/// </para>
	/// </summary>
	public interface TimeSeriesProvider
	{

	  /// <summary>
	  /// Returns a time-series provider that is unable to source any time-series.
	  /// <para>
	  /// All requests for a time-series will return a failure.
	  /// This is used to validate that no time-series have been requested that were not
	  /// already supplied in the input to the market data factory.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the time-series provider </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static TimeSeriesProvider none()
	//  {
	//	return NoTimeSeriesProvider.INSTANCE;
	//  }

	  /// <summary>
	  /// Returns a time-series provider that returns an empty time-series for any ID.
	  /// <para>
	  /// All requests for a time-series will succeed, returning an empty time-series.
	  /// This is used for those cases where time-series are considered optional.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the time-series provider </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static TimeSeriesProvider empty()
	//  {
	//	return EmptyTimeSeriesProvider.INSTANCE;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Provides the time-series for the specified identifier.
	  /// <para>
	  /// The implementation will provide a time-series for the identifier, returning
	  /// a failure if unable to do so.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="identifier">  the market data identifier to find </param>
	  /// <returns> the time-series of market data for the specified identifier </returns>
	  Result<LocalDateDoubleTimeSeries> provideTimeSeries(ObservableId identifier);

	}

}