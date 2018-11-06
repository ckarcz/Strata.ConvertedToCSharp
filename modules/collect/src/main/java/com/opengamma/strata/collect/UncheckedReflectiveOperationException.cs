using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
	/// <summary>
	/// An unchecked reflection exception.
	/// <para>
	/// This is used by <seealso cref="Unchecked"/> to wrap instances of <seealso cref="ReflectiveOperationException"/>.
	/// </para>
	/// </summary>
	public sealed class UncheckedReflectiveOperationException : Exception
	{

	  /// <summary>
	  /// Serialization version </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance that wraps the underlying exception.
	  /// </summary>
	  /// <param name="ex">  the underlying exception, null tolerant </param>
	  public UncheckedReflectiveOperationException(ReflectiveOperationException ex) : base(ex)
	  {
	  }

	}

}