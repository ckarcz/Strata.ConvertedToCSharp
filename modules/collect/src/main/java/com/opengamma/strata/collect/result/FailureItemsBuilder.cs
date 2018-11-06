using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
{

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// A builder for a list of failure items.
	/// <para>
	/// This provides a builder to create <seealso cref="FailureItems"/>.
	/// </para>
	/// </summary>
	public sealed class FailureItemsBuilder
	{

	  /// <summary>
	  /// The mutable list of failures.
	  /// </summary>
	  private readonly ImmutableList.Builder<FailureItem> listBuilder = ImmutableList.builder();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  internal FailureItemsBuilder()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a failure to the list.
	  /// </summary>
	  /// <param name="failure">  the failure to add </param>
	  /// <returns> this, for chaining </returns>
	  public FailureItemsBuilder addFailure(FailureItem failure)
	  {
		listBuilder.add(failure);
		return this;
	  }

	  /// <summary>
	  /// Adds a list of failures to the list.
	  /// </summary>
	  /// <param name="failures">  the failures to add </param>
	  /// <returns> this, for chaining </returns>
	  public FailureItemsBuilder addAllFailures(IList<FailureItem> failures)
	  {
		listBuilder.addAll(failures);
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the resulting instance.
	  /// </summary>
	  /// <returns> the result </returns>
	  public FailureItems build()
	  {
		return FailureItems.of(listBuilder.build());
	  }

	}

}