using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
	/// <summary>
	/// An identifier for a unique item of market data.
	/// <para>
	/// The market data system can locate market data using implementations of this interface.
	/// Implementations can identify any piece of market data.
	/// This includes observable values, such as the quoted market value of a security, and derived
	/// values, such as a volatility surface or a discounting curve.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the market data this identifier refers to </param>
	public interface MarketDataId<T>
	{

	  /// <summary>
	  /// Gets the type of data this identifier refers to.
	  /// </summary>
	  /// <returns> the type of the market data this identifier refers to </returns>
	  Type<T> MarketDataType {get;}

	}

}