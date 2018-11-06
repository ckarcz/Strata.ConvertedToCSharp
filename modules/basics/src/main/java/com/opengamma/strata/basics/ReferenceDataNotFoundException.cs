using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{
	/// <summary>
	/// Exception thrown if reference data cannot be found.
	/// </summary>
	public class ReferenceDataNotFoundException : Exception
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates the exception passing the exception message.
	  /// </summary>
	  /// <param name="message">  the exception message, null tolerant </param>
	  public ReferenceDataNotFoundException(string message) : base(message)
	  {
	  }

	}

}