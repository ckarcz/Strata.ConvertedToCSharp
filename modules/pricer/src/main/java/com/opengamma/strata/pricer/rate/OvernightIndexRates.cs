/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataView = com.opengamma.strata.market.MarketDataView;
	using ValueType = com.opengamma.strata.market.ValueType;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Provides access to rates for an Overnight index.
	/// <para>
	/// This provides historic and forward rates for a single <seealso cref="OvernightIndex"/>, such as 'EUR-EONIA'.
	/// </para>
	/// </summary>
	public interface OvernightIndexRates : MarketDataView, ParameterizedData
	{

	  /// <summary>
	  /// Obtains an instance from a forward curve, with an empty time-series of fixings.
	  /// <para>
	  /// The curve is specified by an instance of <seealso cref="Curve"/>, such as <seealso cref="InterpolatedNodalCurve"/>.
	  /// The curve must have x-values of <seealso cref="ValueType#YEAR_FRACTION year fractions"/> with
	  /// the day count specified. The y-values must be <seealso cref="ValueType#ZERO_RATE zero rates"/>
	  /// or <seealso cref="ValueType#DISCOUNT_FACTOR discount factors"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="valuationDate">  the valuation date for which the curve is valid </param>
	  /// <param name="forwardCurve">  the forward curve </param>
	  /// <returns> the rates view </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static OvernightIndexRates of(com.opengamma.strata.basics.index.OvernightIndex index, java.time.LocalDate valuationDate, com.opengamma.strata.market.curve.Curve forwardCurve)
	//  {
	//
	//	return of(index, valuationDate, forwardCurve, LocalDateDoubleTimeSeries.empty());
	//  }

	  /// <summary>
	  /// Obtains an instance from a curve and time-series of fixings.
	  /// <para>
	  /// The curve is specified by an instance of <seealso cref="Curve"/>, such as <seealso cref="InterpolatedNodalCurve"/>.
	  /// The curve must have x-values of <seealso cref="ValueType#YEAR_FRACTION year fractions"/> with
	  /// the day count specified. The y-values must be <seealso cref="ValueType#ZERO_RATE zero rates"/>
	  /// or <seealso cref="ValueType#DISCOUNT_FACTOR discount factors"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="valuationDate">  the valuation date for which the curve is valid </param>
	  /// <param name="forwardCurve">  the forward curve </param>
	  /// <param name="fixings">  the time-series of fixings </param>
	  /// <returns> the rates view </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static OvernightIndexRates of(com.opengamma.strata.basics.index.OvernightIndex index, java.time.LocalDate valuationDate, com.opengamma.strata.market.curve.Curve forwardCurve, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries fixings)
	//  {
	//
	//	DiscountFactors discountFactors = DiscountFactors.of(index.getCurrency(), valuationDate, forwardCurve);
	//	return DiscountOvernightIndexRates.of(index, discountFactors, fixings);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Overnight index.
	  /// <para>
	  /// The index that the rates are for.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the Overnight index </returns>
	  OvernightIndex Index {get;}

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
	  OvernightIndexRates withParameter(int parameterIndex, double newValue);

	  OvernightIndexRates withPerturbation(ParameterPerturbation perturbation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the historic or forward rate at the specified fixing date.
	  /// <para>
	  /// The rate of the Overnight index, such as 'EUR-EONIA', varies over time.
	  /// This method obtains the actual or estimated rate for the fixing date.
	  /// </para>
	  /// <para>
	  /// This retrieves the actual rate if the fixing date is before the valuation date,
	  /// or the estimated rate if the fixing date is after the valuation date.
	  /// If the fixing date equals the valuation date, then the best available rate is returned.
	  /// The reference period for the underlying deposit is computed from the index conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <returns> the rate of the index, either historic or forward </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double rate(OvernightIndexObservation observation);

	  /// <summary>
	  /// Ignores the time-series of fixings to get the forward rate at the specified
	  /// fixing date, used in rare and special cases. In most cases callers should use
	  /// <seealso cref="#rate(OvernightIndexObservation) rate(OvernightIndexObservation)"/>.
	  /// <para>
	  /// An instance of {@code OvernightIndexRates} is typically based on a forward curve and a historic time-series.
	  /// The {@code rate(LocalDate)} method uses either the curve or time-series, depending on whether the
	  /// fixing date is before or after the valuation date. This method only queries the forward curve,
	  /// totally ignoring the time-series, which is needed for rare and special cases only.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <returns> the rate of the index ignoring the time-series of fixings </returns>
	  double rateIgnoringFixings(OvernightIndexObservation observation);

	  /// <summary>
	  /// Calculates the point sensitivity of the historic or forward rate at the specified fixing date.
	  /// <para>
	  /// This returns a sensitivity instance referring to the points that were queried in the market data.
	  /// If a time-series was used, then there is no sensitivity.
	  /// Otherwise, the sensitivity has the value 1.
	  /// The sensitivity refers to the result of <seealso cref="#rate(OvernightIndexObservation)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <returns> the point sensitivity of the rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  PointSensitivityBuilder ratePointSensitivity(OvernightIndexObservation observation);

	  /// <summary>
	  /// Ignores the time-series of fixings to get the forward rate point sensitivity at the
	  /// specified fixing date, used in rare and special cases. In most cases callers should use
	  /// <seealso cref="#ratePointSensitivity(OvernightIndexObservation) ratePointSensitivity(OvernightIndexObservation)"/>.
	  /// <para>
	  /// An instance of {@code OvernightIndexRates} is typically based on a forward curve and a historic time-series.
	  /// The {@code ratePointSensitivity(LocalDate)} method uses either the curve or time-series, depending on whether the
	  /// fixing date is before or after the valuation date. This method only queries the forward curve,
	  /// totally ignoring the time-series, which is needed for rare and special cases only.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <returns> the point sensitivity of the rate ignoring the time-series of fixings </returns>
	  PointSensitivityBuilder rateIgnoringFixingsPointSensitivity(OvernightIndexObservation observation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the historic or forward rate at the specified fixing period.
	  /// <para>
	  /// The start date should be on or after the valuation date. The end date should be after the start date.
	  /// </para>
	  /// <para>
	  /// This computes the forward rate in the simple simply compounded convention of the index between two given date.
	  /// This is used mainly to speed-up computation by computing the rate on a longer period instead of each individual 
	  /// overnight rate. When data related to the overnight index rate are stored based on the fixing date and not
	  /// the start and end date of the period, the call may return an {@code IllegalArgumentException}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDateObservation">  the rate observation for the start of the period </param>
	  /// <param name="endDate">  the end or maturity date of the period on which the rate is computed </param>
	  /// <returns> the simply compounded rate associated to the period for the index </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double periodRate(OvernightIndexObservation startDateObservation, LocalDate endDate);

	  /// <summary>
	  /// Calculates the point sensitivity of the historic or forward rate at the specified fixing period.
	  /// <para>
	  /// This returns a sensitivity instance referring to the points that were queried in the market data.
	  /// The sensitivity refers to the result of <seealso cref="#periodRate(OvernightIndexObservation, LocalDate)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDateObservation">  the rate observation for the start of the period </param>
	  /// <param name="endDate">  the end or maturity date of the period on which the rate is computed </param>
	  /// <returns> the point sensitivity of the rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  PointSensitivityBuilder periodRatePointSensitivity(OvernightIndexObservation startDateObservation, LocalDate endDate);

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
	  CurrencyParameterSensitivities parameterSensitivity(OvernightRateSensitivity pointSensitivity);

	  /// <summary>
	  /// Creates the parameter sensitivity when the sensitivity values are known.
	  /// <para>
	  /// In most cases, <seealso cref="#parameterSensitivity(OvernightRateSensitivity)"/> should be used and manipulated.
	  /// However, it can be useful to create parameter sensitivity from pre-computed sensitivity values.
	  /// </para>
	  /// <para>
	  /// There will typically be one <seealso cref="CurrencyParameterSensitivity"/> for each underlying data
	  /// structure, such as a curve. For example, if the rates are based on a single forward
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