/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{

	using MarketData = com.opengamma.strata.data.MarketData;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;

	/// <summary>
	/// Market data for products based on repo and issuer curves.
	/// <para>
	/// This interface exposes the market data necessary for pricing bond products,
	/// such as fixing coupon bonds, capital indexed bonds and bond futures.
	/// It uses a <seealso cref="LegalEntityDiscountingMarketDataLookup"/> to provide a view on <seealso cref="MarketData"/>.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface LegalEntityDiscountingMarketData
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
	  /// Gets the lookup that provides access to repo and issuer curves.
	  /// </summary>
	  /// <returns> the discounting lookup </returns>
	  LegalEntityDiscountingMarketDataLookup Lookup {get;}

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
	  LegalEntityDiscountingMarketData withMarketData(MarketData marketData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discounting provider.
	  /// <para>
	  /// This provides access to repo and issuer curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the discounting provider </returns>
	  LegalEntityDiscountingProvider discountingProvider();

	}

}