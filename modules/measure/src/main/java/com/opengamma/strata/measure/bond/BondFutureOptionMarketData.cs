/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{

	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using BondFutureVolatilities = com.opengamma.strata.pricer.bond.BondFutureVolatilities;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// Market data for bond future options.
	/// <para>
	/// This interface exposes the market data necessary for pricing bond future options.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface BondFutureOptionMarketData
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
	  /// Gets the lookup that provides access to bond future volatilities.
	  /// </summary>
	  /// <returns> the bond future options lookup </returns>
	  BondFutureOptionMarketDataLookup Lookup {get;}

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
	  BondFutureOptionMarketData withMarketData(MarketData marketData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the volatilities for the specified security ID.
	  /// <para>
	  /// If the security ID is not found, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="securityId">  the security ID </param>
	  /// <returns> the volatilities for the security ID </returns>
	  /// <exception cref="MarketDataNotFoundException"> if the security ID is not found </exception>
	  BondFutureVolatilities volatilities(SecurityId securityId);

	}

}