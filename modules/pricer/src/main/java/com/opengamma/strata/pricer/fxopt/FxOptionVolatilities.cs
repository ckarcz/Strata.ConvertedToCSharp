/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using MarketDataView = com.opengamma.strata.market.MarketDataView;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Volatilities for pricing FX options.
	/// <para>
	/// This provides access to the volatilities for pricing models, such as Black.
	/// </para>
	/// </summary>
	public interface FxOptionVolatilities : MarketDataView, ParameterizedData
	{

	  /// <summary>
	  /// Gets the name of these volatilities.
	  /// </summary>
	  /// <returns> the name </returns>
	  FxOptionVolatilitiesName Name {get;}

	  /// <summary>
	  /// Gets the currency pair for which the data is valid.
	  /// </summary>
	  /// <returns> the currency pai </returns>
	  CurrencyPair CurrencyPair {get;}

	  /// <summary>
	  /// Gets the type of volatility returned by the <seealso cref="FxOptionVolatilities#volatility"/> method.
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

	  FxOptionVolatilities withParameter(int parameterIndex, double newValue);

	  FxOptionVolatilities withPerturbation(ParameterPerturbation perturbation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the volatility at the specified expiry.
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="expiryDateTime">  the option expiry </param>
	  /// <param name="strike">  the option strike rate </param>
	  /// <param name="forward">  the forward rate </param>
	  /// <returns> the volatility </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double volatility(com.opengamma.strata.basics.currency.CurrencyPair currencyPair, java.time.ZonedDateTime expiryDateTime, double strike, double forward)
	//  {
	//
	//	return volatility(currencyPair, relativeTime(expiryDateTime), strike, forward);
	//  }

	  /// <summary>
	  /// Calculates the volatility at the specified expiry.
	  /// <para>
	  /// This relies on expiry supplied by <seealso cref="#relativeTime(ZonedDateTime)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="strike">  the option strike rate </param>
	  /// <param name="forward">  the forward rate </param>
	  /// <returns> the volatility </returns>
	  double volatility(CurrencyPair currencyPair, double expiry, double strike, double forward);

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
	  /// This relies on volatility supplied by <seealso cref="#volatility(CurrencyPair, double, double, double)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="putCall">  whether the option is put or call </param>
	  /// <param name="strike">  the option strike rate </param>
	  /// <param name="forward">  the forward rate </param>
	  /// <param name="volatility">  the volatility </param>
	  /// <returns> the price </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double price(double expiry, PutCall putCall, double strike, double forward, double volatility);

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