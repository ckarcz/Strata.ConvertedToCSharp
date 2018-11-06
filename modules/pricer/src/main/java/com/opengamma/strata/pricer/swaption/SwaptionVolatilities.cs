/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using MarketDataView = com.opengamma.strata.market.MarketDataView;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;

	/// <summary>
	/// Volatilities for pricing swaptions.
	/// <para>
	/// This provides access to the volatilities for various pricing models, such as normal, Black and SABR.
	/// The price and derivatives are also made available.
	/// </para>
	/// </summary>
	public interface SwaptionVolatilities : MarketDataView, ParameterizedData
	{

	  /// <summary>
	  /// Gets the name of these volatilities.
	  /// </summary>
	  /// <returns> the name </returns>
	  SwaptionVolatilitiesName Name {get;}

	  /// <summary>
	  /// Gets the convention of the swap for which the data is valid.
	  /// </summary>
	  /// <returns> the convention </returns>
	  FixedIborSwapConvention Convention {get;}

	  /// <summary>
	  /// Gets the type of volatility returned by the <seealso cref="SwaptionVolatilities#volatility"/> method.
	  /// </summary>
	  /// <returns> the type </returns>
	  ValueType VolatilityType {get;}

	  /// <summary>
	  /// Gets the valuation date.
	  /// <para>
	  /// The volatilities are calibrated for this date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the valuation date </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate getValuationDate()
	//  {
	//	return getValuationDateTime().toLocalDate();
	//  }

	  /// <summary>
	  /// Gets the valuation date-time.
	  /// <para>
	  /// The volatilities are calibrated for this date-time.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the valuation date-time </returns>
	  ZonedDateTime ValuationDateTime {get;}

	  SwaptionVolatilities withParameter(int parameterIndex, double newValue);

	  SwaptionVolatilities withPerturbation(ParameterPerturbation perturbation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the volatility at the specified expiry.
	  /// <para>
	  /// This relies on tenor supplied by <seealso cref="#tenor(LocalDate, LocalDate)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiryDateTime">  the option expiry </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <param name="strike">  the option strike rate </param>
	  /// <param name="forward">  the forward rate of the underlying swap </param>
	  /// <returns> the volatility </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double volatility(java.time.ZonedDateTime expiryDateTime, double tenor, double strike, double forward)
	//  {
	//	return volatility(relativeTime(expiryDateTime), tenor, strike, forward);
	//  }

	  /// <summary>
	  /// Calculates the volatility at the specified expiry.
	  /// <para>
	  /// This relies on expiry supplied by <seealso cref="#relativeTime(ZonedDateTime)"/>.
	  /// This relies on tenor supplied by <seealso cref="#tenor(LocalDate, LocalDate)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <param name="strike">  the option strike rate </param>
	  /// <param name="forward">  the forward rate of the underlying swap </param>
	  /// <returns> the volatility </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double volatility(double expiry, double tenor, double strike, double forward);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the parameter sensitivity.
	  /// <para>
	  /// This computes the <seealso cref="CurrencyParameterSensitivities"/> associated with the <seealso cref="PointSensitivities"/>.
	  /// This corresponds to the projection of the point sensitivity to the internal parameters representation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivities">  the point sensitivities </param>
	  /// <returns> the sensitivity to the underlying parameters </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.CurrencyParameterSensitivities parameterSensitivity(com.opengamma.strata.market.sensitivity.PointSensitivity... pointSensitivities)
	//  {
	//	return parameterSensitivity(PointSensitivities.of(pointSensitivities));
	//  }

	  /// <summary>
	  /// Calculates the parameter sensitivity.
	  /// <para>
	  /// This computes the <seealso cref="CurrencyParameterSensitivities"/> associated with the <seealso cref="PointSensitivities"/>.
	  /// This corresponds to the projection of the point sensitivity to the internal parameters representation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivities">  the point sensitivities </param>
	  /// <returns> the sensitivity to the underlying parameters </returns>
	  CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price.
	  /// <para>
	  /// This relies on expiry supplied by <seealso cref="#relativeTime(ZonedDateTime)"/>.
	  /// This relies on tenor supplied by <seealso cref="#tenor(LocalDate, LocalDate)"/>.
	  /// This relies on volatility supplied by <seealso cref="#volatility(double, double, double, double)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <param name="putCall">  whether the option is put or call </param>
	  /// <param name="strike">  the option strike rate </param>
	  /// <param name="forward">  the forward rate of the underlying swap </param>
	  /// <param name="volatility">  the volatility </param>
	  /// <returns> the price </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double price(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility);

	  /// <summary>
	  /// Calculates the price delta.
	  /// <para>
	  /// This is the forward driftless delta.
	  /// </para>
	  /// <para>
	  /// This relies on expiry supplied by <seealso cref="#relativeTime(ZonedDateTime)"/>.
	  /// This relies on tenor supplied by <seealso cref="#tenor(LocalDate, LocalDate)"/>.
	  /// This relies on volatility supplied by <seealso cref="#volatility(double, double, double, double)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <param name="putCall">  whether the option is put or call </param>
	  /// <param name="strike">  the option strike rate </param>
	  /// <param name="forward">  the forward rate of the underlying swap </param>
	  /// <param name="volatility">  the volatility </param>
	  /// <returns> the delta </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double priceDelta(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility);

	  /// <summary>
	  /// Calculates the price gamma.
	  /// <para>
	  /// This is the second order sensitivity of the forward option value to the forward.
	  /// </para>
	  /// <para>
	  /// This relies on expiry supplied by <seealso cref="#relativeTime(ZonedDateTime)"/>.
	  /// This relies on tenor supplied by <seealso cref="#tenor(LocalDate, LocalDate)"/>.
	  /// This relies on volatility supplied by <seealso cref="#volatility(double, double, double, double)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <param name="putCall">  whether the option is put or call </param>
	  /// <param name="strike">  the option strike rate </param>
	  /// <param name="forward">  the forward rate of the underlying swap </param>
	  /// <param name="volatility">  the volatility </param>
	  /// <returns> the gamma </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double priceGamma(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility);

	  /// <summary>
	  /// Calculates the price theta.
	  /// <para>
	  /// This is the driftless sensitivity of the present value to a change in time to maturity.
	  /// </para>
	  /// <para>
	  /// This relies on expiry supplied by <seealso cref="#relativeTime(ZonedDateTime)"/>.
	  /// This relies on tenor supplied by <seealso cref="#tenor(LocalDate, LocalDate)"/>.
	  /// This relies on volatility supplied by <seealso cref="#volatility(double, double, double, double)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <param name="putCall">  whether the option is put or call </param>
	  /// <param name="strike">  the option strike rate </param>
	  /// <param name="forward">  the forward rate of the underlying swap </param>
	  /// <param name="volatility">  the volatility </param>
	  /// <returns> the theta </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double priceTheta(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility);

	  /// <summary>
	  /// Calculates the price vega.
	  /// <para>
	  /// This is the sensitivity of the option forward price to the implied volatility.
	  /// </para>
	  /// <para>
	  /// This relies on expiry supplied by <seealso cref="#relativeTime(ZonedDateTime)"/>.
	  /// This relies on tenor supplied by <seealso cref="#tenor(LocalDate, LocalDate)"/>.
	  /// This relies on volatility supplied by <seealso cref="#volatility(double, double, double, double)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <param name="putCall">  whether the option is put or call </param>
	  /// <param name="strike">  the option strike rate </param>
	  /// <param name="forward">  the forward rate of the underlying swap </param>
	  /// <param name="volatility">  the volatility </param>
	  /// <returns> the vega </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double priceVega(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts a time and date to a relative year fraction.
	  /// <para>
	  /// When the date is after the valuation date (and potentially time), the returned number is negative.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dateTime">  the date-time to find the relative year fraction of </param>
	  /// <returns> the relative year fraction </returns>
	  double relativeTime(ZonedDateTime dateTime);

	  /// <summary>
	  /// Calculates the tenor of the swap based on its start date and end date.
	  /// </summary>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <returns> the tenor </returns>
	  double tenor(LocalDate startDate, LocalDate endDate);

	}

}