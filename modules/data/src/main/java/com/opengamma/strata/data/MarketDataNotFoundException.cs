/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
	/// <summary>
	/// Exception thrown if market data cannot be found.
	/// </summary>
	public class MarketDataNotFoundException : System.ArgumentException
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates the exception passing the exception message.
	  /// </summary>
	  /// <param name="message">  the exception message, null tolerant </param>
	  public MarketDataNotFoundException(string message) : base(message)
	  {
	  }

	}

}