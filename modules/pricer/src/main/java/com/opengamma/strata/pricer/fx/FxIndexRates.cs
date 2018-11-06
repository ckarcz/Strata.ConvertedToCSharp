/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataView = com.opengamma.strata.market.MarketDataView;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Provides access to rates for an FX index.
	/// <para>
	/// This provides historic and forward rates for a single <seealso cref="FxIndex"/>, such as 'EUR/GBP-ECB'.
	/// An FX rate is the conversion rate between two currencies. An FX index is the rate
	/// as published by a specific organization, typically at a well-known time-of-day.
	/// </para>
	/// </summary>
	public interface FxIndexRates : MarketDataView, ParameterizedData
	{

	  /// <summary>
	  /// Gets the FX index.
	  /// <para>
	  /// The index that the rates are for.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the FX index </returns>
	  FxIndex Index {get;}

	  /// <summary>
	  /// Gets the time-series of fixings for the index.
	  /// <para>
	  /// The time-series contains historic fixings of the index.
	  /// It may be empty if the data is not available.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the time-series fixings </returns>
	  LocalDateDoubleTimeSeries Fixings {get;}

	  /// <summary>
	  /// Gets the underlying FX forward rates.
	  /// </summary>
	  /// <returns> the FX forward rates </returns>
	  FxForwardRates FxForwardRates {get;}

	  FxIndexRates withParameter(int parameterIndex, double newValue);

	  FxIndexRates withPerturbation(ParameterPerturbation perturbation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the historic or forward rate at the specified fixing date.
	  /// <para>
	  /// The rate of the FX index varies over time.
	  /// This method obtains the actual or estimated rate for the fixing date.
	  /// </para>
	  /// <para>
	  /// This retrieves the actual rate if the fixing date is before the valuation date,
	  /// or the estimated rate if the fixing date is after the valuation date.
	  /// If the fixing date equals the valuation date, then the best available rate is returned.
	  /// </para>
	  /// <para>
	  /// The index defines the conversion rate for a specific currency pair.
	  /// This method specifies which of the two currencies in the index is to be treated
	  /// as the base currency for the purposes of the returned rate.
	  /// If the specified base currency equals the base currency of the index, then
	  /// the rate is simply returned. If the specified base currency equals the counter currency
	  /// of the index, then the inverse rate is returned.
	  /// As such, an amount in the specified base currency can be directly multiplied by the
	  /// returned FX rate to perform FX conversion.
	  /// </para>
	  /// <para>
	  /// To convert an amount in the specified base currency to the other currency,
	  /// multiply it by the returned FX rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <param name="baseCurrency">  the base currency that the rate should be expressed against </param>
	  /// <returns> the rate of the index, either historic or forward </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double rate(FxIndexObservation observation, Currency baseCurrency);

	  /// <summary>
	  /// Calculates the point sensitivity of the historic or forward rate at the specified fixing date.
	  /// <para>
	  /// This returns a sensitivity instance referring to the points that were queried in the market data.
	  /// If a time-series was used, then there is no sensitivity.
	  /// The sensitivity refers to the result of <seealso cref="#rate(FxIndexObservation, Currency)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <param name="baseCurrency">  the base currency that the rate should be expressed against </param>
	  /// <returns> the point sensitivity of the rate </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  PointSensitivityBuilder ratePointSensitivity(FxIndexObservation observation, Currency baseCurrency);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the parameter sensitivity from the point sensitivity.
	  /// <para>
	  /// This is used to convert a single point sensitivity to parameter sensitivity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivity">  the point sensitivity to convert </param>
	  /// <returns> the parameter sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  CurrencyParameterSensitivities parameterSensitivity(FxIndexSensitivity pointSensitivity);

	  /// <summary>
	  /// Calculates the currency exposure from the point sensitivity.
	  /// <para>
	  /// This is used to convert a single point sensitivity to currency exposure.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivity">  the point sensitivity to convert </param>
	  /// <returns> the currency exposure </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  MultiCurrencyAmount currencyExposure(FxIndexSensitivity pointSensitivity);

	}

}