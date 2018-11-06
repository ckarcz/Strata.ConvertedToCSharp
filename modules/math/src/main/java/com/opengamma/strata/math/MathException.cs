using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math
{
	/// <summary>
	/// Exception thrown by math.
	/// </summary>
	public class MathException : Exception
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public MathException() : base()
	  {
	  }

	  /// <summary>
	  /// Creates an instance based on a message.
	  /// </summary>
	  /// <param name="message">  the message, null tolerant </param>
	  public MathException(string message) : base(message)
	  {
	  }

	  /// <summary>
	  /// Creates an instance based on a message and cause.
	  /// </summary>
	  /// <param name="message">  the message, null tolerant </param>
	  /// <param name="cause">  the cause, null tolerant </param>
	  public MathException(string message, Exception cause) : base(message, cause)
	  {
	  }

	  /// <summary>
	  /// Creates an instance based on a cause.
	  /// </summary>
	  /// <param name="cause">  the cause, null tolerant </param>
	  public MathException(Exception cause) : base(cause)
	  {
	  }

	}

}