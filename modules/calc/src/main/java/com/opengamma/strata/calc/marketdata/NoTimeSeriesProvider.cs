/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ObservableId = com.opengamma.strata.data.ObservableId;

	/// <summary>
	/// Implementation of a time-series provider which always returns missing data failures.
	/// <para>
	/// This is designed to be used when it is not necessary to source time-series on-demand,
	/// for example because all required market data is expected to be present in a snapshot.
	/// </para>
	/// </summary>
	internal class NoTimeSeriesProvider : TimeSeriesProvider
	{

	  /// <summary>
	  /// The single, shared instance of this class. </summary>
	  internal static readonly NoTimeSeriesProvider INSTANCE = new NoTimeSeriesProvider();

	  public virtual Result<LocalDateDoubleTimeSeries> provideTimeSeries(ObservableId id)
	  {
		return Result.failure(FailureReason.MISSING_DATA, "No time-series provider configured, unable to provide time-series for '{}'", id);
	  }

	}

}