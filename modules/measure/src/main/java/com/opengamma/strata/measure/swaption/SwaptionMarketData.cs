/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swaption
{

	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using SwaptionVolatilities = com.opengamma.strata.pricer.swaption.SwaptionVolatilities;

	/// <summary>
	/// Market data for swaptions.
	/// <para>
	/// This interface exposes the market data necessary for pricing a swaption.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface SwaptionMarketData
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
	  /// Gets the lookup that provides access to swaption volatilities.
	  /// </summary>
	  /// <returns> the swaption lookup </returns>
	  SwaptionMarketDataLookup Lookup {get;}

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
	  SwaptionMarketData withMarketData(MarketData marketData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the volatilities for the specified Ibor index.
	  /// <para>
	  /// If the index is not found, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Ibor index </param>
	  /// <returns> the volatilities for the index </returns>
	  /// <exception cref="MarketDataNotFoundException"> if the index is not found </exception>
	  SwaptionVolatilities volatilities(IborIndex index);

	}

}