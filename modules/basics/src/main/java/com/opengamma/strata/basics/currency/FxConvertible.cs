/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
	/// <summary>
	/// Defines a standard mechanism for converting an object representing one or more
	/// monetary amounts to a single currency.
	/// <para>
	/// The single method allows a monetary object to be converted to a similar object
	/// expressed in terms of the specified currency.
	/// The conversion is permitted to return a different type.
	/// </para>
	/// <para>
	/// For example, the <seealso cref="MultiCurrencyAmount"/> class implements this interface
	/// and returns a <seealso cref="CurrencyAmount"/> instance.
	/// </para>
	/// <para>
	/// Implementations do not have to be immutable, but calls to the method must be thread-safe.
	/// 
	/// </para>
	/// </summary>
	/// @param <R>  the result type expressed in a single currency </param>
	public interface FxConvertible<R>
	{

	  /// <summary>
	  /// Converts this instance to an equivalent amount in the specified currency.
	  /// <para>
	  /// The result, which may be of a different type, will be expressed in terms of the given currency.
	  /// Any FX conversion that is required will use rates from the provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the converted instance, which should be expressed in the specified currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  R convertedTo(Currency resultCurrency, FxRateProvider rateProvider);

	}

}