/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.credit
{

	using MarketData = com.opengamma.strata.data.MarketData;
	using CreditRatesProvider = com.opengamma.strata.pricer.credit.CreditRatesProvider;

	/// <summary>
	/// Market data for credit products.
	/// <para>
	/// This interface exposes the market data necessary for pricing credit products.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface CreditRatesMarketData
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
	  /// Gets the lookup that provides access to credit, discount and recovery rate curves.
	  /// </summary>
	  /// <returns> the lookup </returns>
	  CreditRatesMarketDataLookup Lookup {get;}

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
	  CreditRatesMarketData withMarketData(MarketData marketData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the credit rates provider.
	  /// <para>
	  /// This provides access to credit, discount and recovery rate curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the credit rates provider </returns>
	  CreditRatesProvider creditRatesProvider();

	}

}