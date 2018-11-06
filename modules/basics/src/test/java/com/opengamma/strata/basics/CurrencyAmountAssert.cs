/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{
	using AbstractAssert = org.assertj.core.api.AbstractAssert;
	using Assertions = org.assertj.core.api.Assertions;
	using Offset = org.assertj.core.data.Offset;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;

	/// <summary>
	/// An assert helper that provides useful AssertJ assertion
	/// methods for <seealso cref="CurrencyAmount"/> instances.
	/// <para>
	/// These allow {code CurrencyAmount}s to be inspected in tests in the
	/// same fluent style as other basic classes.
	/// </para>
	/// <para>
	/// So the following:
	/// <pre>
	///   CurrencyAmount result = someMethodCall();
	///   assertEquals(result.getCurrency(), USD);
	///   assertEquals(result.getAmount(), 123.45, 1e-6);
	/// </pre>
	/// can be replaced with:
	/// <pre>
	///   CurrencyAmount result = someMethodCall();
	///   assertThat(result)
	///     .hasCurrency(USD)
	///     .hasAmount(123.45, within(1e-6));
	/// </pre>
	/// or:
	/// <pre>
	///   CurrencyAmount result = someMethodCall();
	///   CurrencyAmount expected = CurrencyAmount.of(USD, 123.45);
	///   assertThat(result).isEqualTo(expected, within(1e-6));
	/// </pre>
	/// </para>
	/// <para>
	/// In order to be able to use a statically imported assertThat()
	/// method for both {@code CurrencyAmount} and other types, statically
	/// import <seealso cref="BasicProjectAssertions#assertThat(CurrencyAmount)"/>
	/// rather than this class.
	/// </para>
	/// </summary>
	public class CurrencyAmountAssert : AbstractAssert<CurrencyAmountAssert, CurrencyAmount>
	{

	  /// <summary>
	  /// Private constructor, use <seealso cref="#assertThat(CurrencyAmount)"/> to
	  /// construct an instance.
	  /// </summary>
	  /// <param name="actual"> the amount to create an {@code Assert} for </param>
	  private CurrencyAmountAssert(CurrencyAmount actual) : base(actual, typeof(CurrencyAmountAssert))
	  {
	  }

	  /// <summary>
	  /// Create an {@code Assert} instance for the supplied {@code CurrencyAmount}.
	  /// </summary>
	  /// <param name="amount">  the amount to create an {@code Assert} for </param>
	  /// <returns> an {@code Assert} instance </returns>
	  public static CurrencyAmountAssert assertThat(CurrencyAmount amount)
	  {
		return new CurrencyAmountAssert(amount);
	  }

	  /// <summary>
	  /// Assert that the {@code CurrencyAmount} is of the expected currency.
	  /// </summary>
	  /// <param name="ccy">  the expected currency </param>
	  /// <returns> this if the currency matches the expectation, else
	  ///   throw an {@code AssertionError} </returns>
	  public virtual CurrencyAmountAssert hasCurrency(Currency ccy)
	  {
		NotNull;
		if (!actual.Currency.Equals(ccy))
		{
		  failWithMessage("Expected CurrencyAmount with currency: <%s> but was: <%s>", ccy, actual.Currency);
		}
		return this;
	  }

	  /// <summary>
	  /// Assert that the {@code CurrencyAmount} is for the expected amount.
	  /// </summary>
	  /// <param name="expectedAmount">  the expected amount </param>
	  /// <returns> this if the amount matches the expectation, else
	  ///   throw an {@code AssertionError} </returns>
	  public virtual CurrencyAmountAssert hasAmount(double expectedAmount)
	  {
		NotNull;
		Assertions.assertThat(actual.Amount).isEqualTo(expectedAmount);
		return this;
	  }

	  /// <summary>
	  /// Assert that the {@code CurrencyAmount} is within range
	  /// of an expected amount.
	  /// </summary>
	  /// <param name="expectedAmount">  the expected amount </param>
	  /// <param name="tolerance">  the tolerance to use </param>
	  /// <returns> this if the amount matches the expectation, else
	  ///   throw an {@code AssertionError} </returns>
	  public virtual CurrencyAmountAssert hasAmount(double expectedAmount, Offset<double> tolerance)
	  {
		NotNull;
		Assertions.assertThat(actual.Amount).isEqualTo(expectedAmount, tolerance);
		return this;
	  }

	  /// <summary>
	  /// Assert that the {@code CurrencyAmount} has the same currency as
	  /// the supplied {@code CurrencyAmount} and that the amount is within
	  /// range of the supplied {@code CurrencyAmount}'s amount.
	  /// </summary>
	  /// <param name="expected">  the expected {@code CurrencyAmount} </param>
	  /// <param name="tolerance">  the tolerance to use </param>
	  /// <returns> this if the amount matches the expectation, else
	  ///   throw an {@code AssertionError} </returns>
	  public virtual CurrencyAmountAssert isEqualTo(CurrencyAmount expected, Offset<double> tolerance)
	  {
		NotNull;
		hasCurrency(expected.Currency);
		hasAmount(expected.Amount, tolerance);
		return this;
	  }

	}

}