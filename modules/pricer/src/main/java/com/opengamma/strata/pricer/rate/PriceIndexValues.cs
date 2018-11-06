/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using PriceIndexObservation = com.opengamma.strata.basics.index.PriceIndexObservation;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataView = com.opengamma.strata.market.MarketDataView;
	using ValueType = com.opengamma.strata.market.ValueType;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Provides access to the values of a price index.
	/// <para>
	/// This provides historic and forward values for a single <seealso cref="PriceIndex"/>, such as 'US-CPI-U'.
	/// This is typically used in inflation products.
	/// </para>
	/// </summary>
	public interface PriceIndexValues : MarketDataView, ParameterizedData
	{

	  /// <summary>
	  /// Obtains an instance from a curve and time-series of fixings.
	  /// <para>
	  /// The only supported implementation at present is <seealso cref="SimplePriceIndexValues"/>.
	  /// The curve must have x-values of <seealso cref="ValueType#MONTHS months"/>.
	  /// The y-values must be <seealso cref="ValueType#PRICE_INDEX price index values"/>.
	  /// The fixings time-series must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="valuationDate">  the valuation date for which the curve is valid </param>
	  /// <param name="forwardCurve">  the forward curve </param>
	  /// <param name="fixings">  the time-series of fixings </param>
	  /// <returns> the price index values </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static PriceIndexValues of(com.opengamma.strata.basics.index.PriceIndex index, java.time.LocalDate valuationDate, com.opengamma.strata.market.curve.Curve forwardCurve, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries fixings)
	//  {
	//
	//	if (forwardCurve instanceof NodalCurve)
	//	{
	//	  return SimplePriceIndexValues.of(index, valuationDate, (NodalCurve) forwardCurve, fixings);
	//	}
	//	throw new IllegalArgumentException(Messages.format("Unknown curve type for PriceIndexValues, must be 'NodalCurve' but was '{}'", forwardCurve.getClass().getName()));
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Price index.
	  /// <para>
	  /// The index that the rates are for.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the Price index </returns>
	  PriceIndex Index {get;}

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

	  //-------------------------------------------------------------------------
	  PriceIndexValues withParameter(int parameterIndex, double newValue);

	  PriceIndexValues withPerturbation(ParameterPerturbation perturbation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the historic or forward rate at the specified fixing month.
	  /// <para>
	  /// The rate of the Price index, such as 'US-CPI-U', varies over time.
	  /// This method obtains the actual or estimated rate for the month.
	  /// </para>
	  /// <para>
	  /// This retrieves the actual rate if the fixing month is before the valuation month,
	  /// or the estimated rate if the fixing month is after the valuation month.
	  /// If the month equals the valuation month, then the best available value is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing month </param>
	  /// <returns> the value of the index, either historic or forward </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double value(PriceIndexObservation observation);

	  /// <summary>
	  /// Calculates the point sensitivity of the historic or forward value at the specified fixing month.
	  /// <para>
	  /// This returns a sensitivity instance referring to the points that were queried in the market data.
	  /// If a time-series was used, then there is no sensitivity.
	  /// The sensitivity refers to the result of <seealso cref="#value(PriceIndexObservation)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing month </param>
	  /// <returns> the point sensitivity of the value </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  PointSensitivityBuilder valuePointSensitivity(PriceIndexObservation observation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the parameter sensitivity from the point sensitivity.
	  /// <para>
	  /// This is used to convert a single point sensitivity to parameter sensitivity.
	  /// The calculation typically involves multiplying the point and unit sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivity">  the point sensitivity to convert </param>
	  /// <returns> the parameter sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  CurrencyParameterSensitivities parameterSensitivity(InflationRateSensitivity pointSensitivity);

	  /// <summary>
	  /// Creates the parameter sensitivity when the sensitivity values are known.
	  /// <para>
	  /// In most cases, <seealso cref="#parameterSensitivity(InflationRateSensitivity)"/> should be used and manipulated.
	  /// However, it can be useful to create parameter sensitivity from pre-computed sensitivity values.
	  /// </para>
	  /// <para>
	  /// There will typically be one <seealso cref="CurrencyParameterSensitivity"/> for each underlying data
	  /// structure, such as a curve. For example, if the values are based on a single forward
	  /// curve, then there will be one {@code CurrencyParameterSensitivity} in the result.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="sensitivities">  the sensitivity values, which must match the parameter count </param>
	  /// <returns> the parameter sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  CurrencyParameterSensitivities createParameterSensitivity(Currency currency, DoubleArray sensitivities);

	}

}