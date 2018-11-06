using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.fpml
{
	/// <summary>
	/// Exception thrown when parsing FpML.
	/// </summary>
	public sealed class FpmlParseException : Exception
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance based on a message.
	  /// </summary>
	  /// <param name="message">  the message, null tolerant </param>
	  public FpmlParseException(string message) : base(message)
	  {
	  }

	}

}