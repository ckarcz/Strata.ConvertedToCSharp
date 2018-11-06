using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;

	/// <summary>
	/// A provider of data used for pricing.
	/// <para>
	/// This provides the valuation date, FX rates and discount factors,
	/// Sensitivity for discount factors is also available.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface BaseProvider : FxRateProvider
	{

	  /// <summary>
	  /// Gets the valuation date.
	  /// <para>
	  /// The raw data in this provider is calibrated for this date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the valuation date </returns>
	  LocalDate ValuationDate {get;}

	  /// <summary>
	  /// Gets the set of currencies that discount factors are provided for.
	  /// </summary>
	  /// <returns> the set of discount curve currencies </returns>
	  ISet<Currency> DiscountCurrencies {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets market data of a specific type.
	  /// <para>
	  /// This is a general purpose mechanism to obtain market data.
	  /// In general, it is desirable to pass the specific market data needed for pricing into
	  /// the pricing method. However, in some cases, notably swaps, this is not feasible.
	  /// It is strongly recommended to clearly state on pricing methods what data is required.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the data associated with the key </returns>
	  /// <exception cref="IllegalArgumentException"> if the data is not available </exception>
	  T data<T>(MarketDataId<T> id);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the FX rate for the specified currency pair on the valuation date.
	  /// <para>
	  /// The rate returned is the rate from the base currency to the counter currency
	  /// as defined by this formula: {@code (1 * baseCurrency = fxRate * counterCurrency)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseCurrency">  the base currency, to convert from </param>
	  /// <param name="counterCurrency">  the counter currency, to convert to </param>
	  /// <returns> the current FX rate for the currency pair </returns>
	  /// <exception cref="IllegalArgumentException"> if the rate is not available </exception>
	  double fxRate(Currency baseCurrency, Currency counterCurrency);

	  /// <summary>
	  /// Gets the FX rate for the specified currency pair on the valuation date.
	  /// <para>
	  /// The rate returned is the rate from the base currency to the counter currency
	  /// as defined by this formula: {@code (1 * baseCurrency = fxRate * counterCurrency)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the ordered currency pair defining the rate required </param>
	  /// <returns> the current FX rate for the currency pair </returns>
	  /// <exception cref="IllegalArgumentException"> if the rate is not available </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double fxRate(com.opengamma.strata.basics.currency.CurrencyPair currencyPair)
	//  {
	//	return fxRate(currencyPair.getBase(), currencyPair.getCounter());
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discount factors for a currency.
	  /// <para>
	  /// The discount factor represents the time value of money for the specified currency
	  /// when comparing the valuation date to the specified date.
	  /// </para>
	  /// <para>
	  /// If the valuation date is on or after the specified date, the discount factor is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency to get the discount factors for </param>
	  /// <returns> the discount factors for the specified currency </returns>
	  /// <exception cref="IllegalArgumentException"> if the discount factors are not available </exception>
	  DiscountFactors discountFactors(Currency currency);

	  /// <summary>
	  /// Gets the discount factor applicable for a currency.
	  /// <para>
	  /// The discount factor represents the time value of money for the specified currency
	  /// when comparing the valuation date to the specified date.
	  /// </para>
	  /// <para>
	  /// If the valuation date is on or after the specified date, the discount factor is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency to get the discount factor for </param>
	  /// <param name="date">  the date to discount to </param>
	  /// <returns> the discount factor </returns>
	  /// <exception cref="IllegalArgumentException"> if the discount factors are not available </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double discountFactor(com.opengamma.strata.basics.currency.Currency currency, java.time.LocalDate date)
	//  {
	//	return discountFactors(currency).discountFactor(date);
	//  }

	}

}