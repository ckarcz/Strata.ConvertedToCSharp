/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;

	/// <summary>
	/// Provides the ability for objects to be automatically currency converted.
	/// <para>
	/// The interface is intended to operate with multi-scenario objects.
	/// As such, the supplied market data is scenario aware, and the FX rate used to convert
	/// each value may be different.
	/// </para>
	/// <para>
	/// For example, the object implementing this interface might hold 100 instances of
	/// <seealso cref="CurrencyAmount"/>, one for each scenario. When invoked, the <seealso cref="ScenarioFxRateProvider"/>
	/// will be used to convert each of the 100 amounts, with each conversion potentially
	/// having a different FX rate.
	/// </para>
	/// <para>
	/// This is the multi-scenario version of <seealso cref="FxConvertible"/>.
	/// 
	/// </para>
	/// </summary>
	/// @param <R>  the type of the currency converted result </param>
	public interface ScenarioFxConvertible<R>
	{

	  /// <summary>
	  /// Converts this instance to an equivalent amount in the specified currency.
	  /// <para>
	  /// The result, which may be of a different type, will be expressed in terms of the given currency.
	  /// Any FX conversion that is required will use rates from the provider.
	  /// </para>
	  /// <para>
	  /// Any object that is not a currency amount will be left unchanged.
	  /// The number of scenarios of this instance must match the number of scenarios of the specified provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the multi-scenario provider of FX rates </param>
	  /// <returns> the converted instance, which should be expressed in the specified currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  R convertedTo(Currency resultCurrency, ScenarioFxRateProvider rateProvider);

	}

}