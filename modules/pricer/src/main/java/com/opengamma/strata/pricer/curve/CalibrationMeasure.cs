using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;

	/// <summary>
	/// Provides access to the measures needed to perform curve calibration for a single type of trade.
	/// <para>
	/// The most commonly used measures are par spread and converted present value.
	/// </para>
	/// <para>
	/// See <seealso cref="TradeCalibrationMeasure"/> for constants defining measures for common trade types.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the trade type </param>
	public interface CalibrationMeasure<T> where T : com.opengamma.strata.product.ResolvedTrade
	{

	  /// <summary>
	  /// Gets the trade type of the calibrator.
	  /// </summary>
	  /// <returns> the trade type </returns>
	  Type<T> TradeType {get;}

	  /// <summary>
	  /// Calculates the value, such as par spread.
	  /// <para>
	  /// The value must be calculated using the specified rates provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the sensitivity </returns>
	  /// <exception cref="IllegalArgumentException"> if the trade cannot be valued </exception>
	  double value(T trade, RatesProvider provider);

	  /// <summary>
	  /// Calculates the parameter sensitivities that relate to the value.
	  /// <para>
	  /// The sensitivities must be calculated using the specified rates provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the sensitivity </returns>
	  /// <exception cref="IllegalArgumentException"> if the trade cannot be valued </exception>
	  CurrencyParameterSensitivities sensitivities(T trade, RatesProvider provider);

	}

}