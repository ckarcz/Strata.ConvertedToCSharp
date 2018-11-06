/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.rate
{

	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using MarketData = com.opengamma.strata.data.MarketData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// Market data for rates products.
	/// <para>
	/// This interface exposes the market data necessary for pricing rates products,
	/// such as Swaps, FRAs and FX.
	/// It uses a <seealso cref="RatesMarketDataLookup"/> to provide a view on <seealso cref="MarketData"/>.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface RatesMarketData
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
	  /// Gets the lookup that provides access to discount curves and forward curves.
	  /// </summary>
	  /// <returns> the rates lookup </returns>
	  RatesMarketDataLookup Lookup {get;}

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
	  RatesMarketData withMarketData(MarketData marketData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rates provider.
	  /// <para>
	  /// This provides access to discount curves, forward curves and FX.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the rates provider </returns>
	  RatesProvider ratesProvider();

	  /// <summary>
	  /// Gets the FX rate provider.
	  /// <para>
	  /// This provides access to FX rates.
	  /// By default, this returns the rates provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the rates provider </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.currency.FxRateProvider fxRateProvider()
	//  {
	//	return ratesProvider();
	//  }

	}

}