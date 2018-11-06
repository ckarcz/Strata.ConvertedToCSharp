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
	/// Implementation of a time-series provider which returns an empty time series for any ID.
	/// <para>
	/// This is useful when calculations might require a time series and therefore request it but the
	/// user knows that in the current case the time series data won't be used.
	/// </para>
	/// </summary>
	internal class EmptyTimeSeriesProvider : TimeSeriesProvider
	{

	  /// <summary>
	  /// The single, shared instance of this class. </summary>
	  internal static readonly EmptyTimeSeriesProvider INSTANCE = new EmptyTimeSeriesProvider();

	  public virtual Result<LocalDateDoubleTimeSeries> provideTimeSeries(ObservableId id)
	  {
		return Result.success(LocalDateDoubleTimeSeries.empty());
	  }

	}

}