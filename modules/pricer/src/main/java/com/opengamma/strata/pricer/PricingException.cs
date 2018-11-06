using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer
{
	/// <summary>
	/// Exception thrown when pricing fails.
	/// </summary>
	public sealed class PricingException : Exception
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance based on a message.
	  /// </summary>
	  /// <param name="message">  the message, null tolerant </param>
	  public PricingException(string message) : base(message)
	  {
	  }

	  /// <summary>
	  /// Creates an instance based on a message and cause.
	  /// </summary>
	  /// <param name="message">  the message, null tolerant </param>
	  /// <param name="cause">  the cause, null tolerant </param>
	  public PricingException(string message, Exception cause) : base(message, cause)
	  {
	  }

	}

}