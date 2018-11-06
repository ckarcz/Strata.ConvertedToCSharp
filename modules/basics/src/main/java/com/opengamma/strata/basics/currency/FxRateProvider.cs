/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
	/// <summary>
	/// A provider of FX rates.
	/// <para>
	/// This provides the ability to obtain an FX rate. The interface does not mandate when the
	/// rate applies, however it typically represents the current rate.
	/// </para>
	/// <para>
	/// One possible implementation is <seealso cref="FxMatrix"/>.
	/// </para>
	/// <para>
	/// Implementations do not have to be immutable, but calls to the methods must be thread-safe.
	/// </para>
	/// </summary>
	public interface FxRateProvider
	{

	  /// <summary>
	  /// Converts an amount in a currency to an amount in a different currency using this rate.
	  /// <para>
	  /// The currencies must both be included in the currency pair of this rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  an amount in {@code fromCurrency} </param>
	  /// <param name="fromCurrency">  the currency of the amount </param>
	  /// <param name="toCurrency">  the currency into which the amount should be converted </param>
	  /// <returns> the amount converted into {@code toCurrency} </returns>
	  /// <exception cref="IllegalArgumentException"> if either of the currencies aren't included in the currency pair of this rate </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double convert(double amount, Currency fromCurrency, Currency toCurrency)
	//  {
	//	return amount * fxRate(fromCurrency, toCurrency);
	//  }

	  /// <summary>
	  /// Gets the FX rate for the specified currency pair.
	  /// <para>
	  /// The rate returned is the rate from the base currency to the counter currency
	  /// as defined by this formula: {@code (1 * baseCurrency = fxRate * counterCurrency)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseCurrency">  the base currency, to convert from </param>
	  /// <param name="counterCurrency">  the counter currency, to convert to </param>
	  /// <returns> the FX rate for the currency pair </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  double fxRate(Currency baseCurrency, Currency counterCurrency);

	  /// <summary>
	  /// Gets the FX rate for the specified currency pair.
	  /// <para>
	  /// The rate returned is the rate from the base currency to the counter currency
	  /// as defined by this formula: {@code (1 * baseCurrency = fxRate * counterCurrency)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the ordered currency pair defining the rate required </param>
	  /// <returns> the FX rate for the currency pair </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double fxRate(CurrencyPair currencyPair)
	//  {
	//	return fxRate(currencyPair.getBase(), currencyPair.getCounter());
	//  }

	}

}