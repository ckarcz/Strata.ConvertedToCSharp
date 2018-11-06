using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
{

	/// <summary>
	/// An exception thrown when a failure <seealso cref="Result"/> is encountered and the failure can't be handled.
	/// </summary>
	public class FailureException : Exception
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The details of the failure. </summary>
	  private readonly Failure failure;

	  /// <summary>
	  /// Returns an exception wrapping a failure that couldn't be handled.
	  /// </summary>
	  /// <param name="failure">  a failure that couldn't be handled </param>
	  public FailureException(Failure failure) : base(failure.Message)
	  {
		this.failure = ArgChecker.notNull(failure, "failure");
	  }

	  /// <summary>
	  /// Returns the details of the failure.
	  /// </summary>
	  /// <returns> the details of the failure </returns>
	  public virtual Failure Failure
	  {
		  get
		  {
			return failure;
		  }
	  }

	}

}