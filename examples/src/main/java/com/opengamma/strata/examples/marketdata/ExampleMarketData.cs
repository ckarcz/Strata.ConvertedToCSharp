/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.marketdata
{
	/// <summary>
	/// Contains utilities for using example market data.
	/// </summary>
	public sealed class ExampleMarketData
	{

	  /// <summary>
	  /// Root resource directory of the built-in example market data
	  /// </summary>
	  private const string EXAMPLE_MARKET_DATA_ROOT = "example-marketdata";

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private ExampleMarketData()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets a market data builder for the built-in example market data.
	  /// </summary>
	  /// <returns> the market data builder </returns>
	  public static ExampleMarketDataBuilder builder()
	  {
		return ExampleMarketDataBuilder.ofResource(EXAMPLE_MARKET_DATA_ROOT);
	  }

	}

}