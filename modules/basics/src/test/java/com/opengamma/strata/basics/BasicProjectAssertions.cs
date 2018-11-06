/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CollectProjectAssertions = com.opengamma.strata.collect.CollectProjectAssertions;

	/// <summary>
	/// Helper class to allow custom AssertJ assertions to be
	/// accessible via the same static import as the standard
	/// assertions.
	/// <para>
	/// Prefer to statically import <seealso cref="#assertThat(CurrencyAmount)"/>
	/// from this class rather than <seealso cref="CurrencyAmountAssert#assertThat(CurrencyAmount)"/>.
	/// </para>
	/// </summary>
	public class BasicProjectAssertions : CollectProjectAssertions
	{

	  /// <summary>
	  /// Create an {@code Assert} instance that enables
	  /// assertions on {@code CurrencyAmount} objects.
	  /// </summary>
	  /// <param name="amount">  the amount to create an {@code Assert} for </param>
	  /// <returns> an {@code Assert} instance </returns>
	  public static CurrencyAmountAssert assertThat(CurrencyAmount amount)
	  {
		return CurrencyAmountAssert.assertThat(amount);
	  }
	}

}