/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
	using Assertions = org.assertj.core.api.Assertions;

	using Result = com.opengamma.strata.collect.result.Result;

	/// <summary>
	/// Helper class to allow custom AssertJ assertions to be
	/// accessible via the same static import as the standard
	/// assertions.
	/// <para>
	/// Prefer to statically import <seealso cref="#assertThat(Result)"/>
	/// from this class rather than <seealso cref="ResultAssert#assertThat(Result)"/>.
	/// </para>
	/// </summary>
	public class CollectProjectAssertions : Assertions
	{

	  /// <summary>
	  /// Create an {@code Assert} instance that enables
	  /// assertions on {@code Result} objects.
	  /// </summary>
	  /// <param name="result">  the result to create an {@code Assert} for </param>
	  /// <returns> an {@code Assert} instance </returns>
	  public static ResultAssert assertThat<T1>(Result<T1> result)
	  {
		return ResultAssert.assertThat(result);
	  }

	}

}