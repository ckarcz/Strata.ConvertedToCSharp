/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using FxOptionVolatilities = com.opengamma.strata.pricer.fxopt.FxOptionVolatilities;

	/// <summary>
	/// Market data for FX options.
	/// <para>
	/// This interface exposes the market data necessary for pricing FX options.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface FxOptionMarketData
	{

	  /// <summary>
	  /// Gets the valuation date.
	  /// </summary>
	  /// <returns> the valuation date </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate getValuationDate()
	//  {
	//	return getMarketData().getValuationDate();
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the lookup that provides access to FX options volatilities.
	  /// </summary>
	  /// <returns> the FX options lookup </returns>
	  FxOptionMarketDataLookup Lookup {get;}

	  /// <summary>
	  /// Gets the market data.
	  /// </summary>
	  /// <returns> the market data </returns>
	  MarketData MarketData {get;}

	  /// <summary>
	  /// Returns a copy of this instance with the specified market data.
	  /// </summary>
	  /// <param name="marketData">  the market data to use </param>
	  /// <returns> a market view based on the specified data </returns>
	  FxOptionMarketData withMarketData(MarketData marketData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the volatilities for the specified currency pair.
	  /// <para>
	  /// If the currency pair is not found, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <returns> the volatilities for the currency pair </returns>
	  /// <exception cref="MarketDataNotFoundException"> if the currency pair is not found </exception>
	  FxOptionVolatilities volatilities(CurrencyPair currencyPair);

	}

}