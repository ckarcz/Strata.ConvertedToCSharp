/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{

	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using MarketDataView = com.opengamma.strata.market.MarketDataView;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;

	/// <summary>
	/// Volatilities for pricing Ibor futures.
	/// <para>
	/// This provides access to the volatilities for various pricing models, such as normal.
	/// </para>
	/// </summary>
	public interface IborFutureOptionVolatilities : MarketDataView, ParameterizedData
	{

	  /// <summary>
	  /// Gets the name of these volatilities.
	  /// </summary>
	  /// <returns> the name </returns>
	  IborFutureOptionVolatilitiesName Name {get;}

	  /// <summary>
	  /// Gets the index of the underlying future for which the data is valid.
	  /// </summary>
	  /// <returns> the index </returns>
	  IborIndex Index {get;}

	  /// <summary>
	  /// Gets the type of volatility returned by the <seealso cref="IborFutureOptionVolatilities#volatility"/> method.
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

	  IborFutureOptionVolatilities withParameter(int parameterIndex, double newValue);

	  IborFutureOptionVolatilities withPerturbation(ParameterPerturbation perturbation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the volatility at the specified expiry.
	  /// </summary>
	  /// <param name="expiryDateTime">  the option expiry </param>
	  /// <param name="fixingDate">  the underlying future fixing date </param>
	  /// <param name="strikePrice">  the option strike price </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <returns> the volatility </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double volatility(java.time.ZonedDateTime expiryDateTime, java.time.LocalDate fixingDate, double strikePrice, double futurePrice)
	//  {
	//
	//	return volatility(relativeTime(expiryDateTime), fixingDate, strikePrice, futurePrice);
	//  }

	  /// <summary>
	  /// Calculates the volatility at the specified expiry.
	  /// <para>
	  /// This relies on expiry supplied by <seealso cref="#relativeTime(ZonedDateTime)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="fixingDate">  the underlying future fixing date </param>
	  /// <param name="strikePrice">  the option strike price </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <returns> the volatility </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double volatility(double expiry, LocalDate fixingDate, double strikePrice, double futurePrice);

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
	  /// Converts a time and date to a relative year fraction. 
	  /// <para>
	  /// When the date is after the valuation date (and potentially time), the returned number is negative.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dateTime">  the date-time to find the relative year fraction of </param>
	  /// <returns> the relative year fraction </returns>
	  double relativeTime(ZonedDateTime dateTime);

	}

}