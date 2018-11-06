using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{

	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using Splitter = com.google.common.@base.Splitter;
	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// An amount of a currency.
	/// <para>
	/// This class represents a {@code double} amount associated with a currency.
	/// It is specifically named "CurrencyAmount" and not "Money" to indicate that
	/// it simply holds a currency and an amount. By contrast, naming it "Money"
	/// would imply it was a suitable choice for accounting purposes, which it is not.
	/// This design approach has been chosen primarily for performance reasons.
	/// Using a {@code BigDecimal} is markedly slower.
	/// See the <seealso cref="Money"/> class for the alternative that uses <seealso cref="BigDecimal"/>.
	/// </para>
	/// <para>
	/// A {@code double} is a 64 bit floating point value suitable for most calculations.
	/// Floating point maths is
	/// <a href="http://docs.oracle.com/cd/E19957-01/806-3568/ncg_goldberg.html">inexact</a>
	/// due to the conflict between binary and decimal arithmetic.
	/// As such, there is the potential for data loss at the margins.
	/// For example, adding the {@code double} values {@code 0.1d} and {@code 0.2d}
	/// results in {@code 0.30000000000000004} rather than {@code 0.3}.
	/// As can be seen, the level of error is small, hence providing this class is
	/// used appropriately, the use of {@code double} is acceptable.
	/// For example, using this class to provide a meaningful result type after
	/// calculations have completed would be an appropriate use.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class CurrencyAmount : FxConvertible<CurrencyAmount>, IComparable<CurrencyAmount>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The currency.
	  /// <para>
	  /// For example, in the value 'GBP 12.34' the currency is 'GBP'.
	  /// </para>
	  /// </summary>
	  private readonly Currency currency;
	  /// <summary>
	  /// The amount of the currency.
	  /// <para>
	  /// For example, in the value 'GBP 12.34' the amount is 12.34.
	  /// </para>
	  /// </summary>
	  private readonly double amount;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a zero amount instance of {@code CurrencyAmount} for the specified currency.
	  /// </summary>
	  /// <param name="currency">  the currency the amount is in </param>
	  /// <returns> the zero amount instance </returns>
	  public static CurrencyAmount zero(Currency currency)
	  {
		return of(currency, 0d);
	  }

	  /// <summary>
	  /// Obtains an instance of {@code CurrencyAmount} for the specified currency and amount.
	  /// </summary>
	  /// <param name="currency">  the currency the amount is in </param>
	  /// <param name="amount">  the amount of the currency to represent </param>
	  /// <returns> the currency amount </returns>
	  public static CurrencyAmount of(Currency currency, double amount)
	  {
		return new CurrencyAmount(currency, amount);
	  }

	  /// <summary>
	  /// Obtains an instance of {@code CurrencyAmount} for the specified ISO-4217
	  /// three letter currency code and amount.
	  /// <para>
	  /// A currency is uniquely identified by ISO-4217 three letter code.
	  /// This method creates the currency if it is not known.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyCode">  the three letter currency code, ASCII and upper case </param>
	  /// <param name="amount">  the amount of the currency to represent </param>
	  /// <returns> the currency amount </returns>
	  /// <exception cref="IllegalArgumentException"> if the currency code is invalid </exception>
	  public static CurrencyAmount of(string currencyCode, double amount)
	  {
		return of(Currency.of(currencyCode), amount);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the string to produce a {@code CurrencyAmount}.
	  /// <para>
	  /// This parses the {@code toString} format of '${currency} ${amount}'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountStr">  the amount string </param>
	  /// <returns> the currency amount </returns>
	  /// <exception cref="IllegalArgumentException"> if the amount cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static CurrencyAmount parse(String amountStr)
	  public static CurrencyAmount parse(string amountStr)
	  {
		ArgChecker.notNull(amountStr, "amountStr");
		IList<string> split = Splitter.on(' ').splitToList(amountStr);
		if (split.Count != 2)
		{
		  throw new System.ArgumentException("Unable to parse amount, invalid format: " + amountStr);
		}
		try
		{
		  Currency cur = Currency.parse(split[0]);
		  double amount = double.Parse(split[1]);
		  return new CurrencyAmount(cur, amount);
		}
		catch (Exception ex)
		{
		  throw new System.ArgumentException("Unable to parse amount: " + amountStr, ex);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="amount">  the amount </param>
	  private CurrencyAmount(Currency currency, double amount)
	  {
		this.currency = ArgChecker.notNull(currency, "currency");
		this.amount = amount;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency.
	  /// <para>
	  /// For example, in the value 'GBP 12.34' the currency is 'GBP'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  /// <summary>
	  /// Gets the amount of the currency.
	  /// <para>
	  /// For example, in the value 'GBP 12.34' the amount is 12.34.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the amount </returns>
	  public double Amount
	  {
		  get
		  {
			return amount;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this {@code CurrencyAmount} with the specified amount added.
	  /// <para>
	  /// This adds the specified amount to this monetary amount, returning a new object.
	  /// The addition simply uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountToAdd">  the amount to add, in the same currency </param>
	  /// <returns> an amount based on this with the specified amount added </returns>
	  /// <exception cref="IllegalArgumentException"> if the currencies are not equal </exception>
	  public CurrencyAmount plus(CurrencyAmount amountToAdd)
	  {
		ArgChecker.notNull(amountToAdd, "amountToAdd");
		ArgChecker.isTrue(amountToAdd.Currency.Equals(currency), "Unable to add amounts in different currencies");
		return plus(amountToAdd.Amount);
	  }

	  /// <summary>
	  /// Returns a copy of this {@code CurrencyAmount} with the specified amount added.
	  /// <para>
	  /// This adds the specified amount to this monetary amount, returning a new object.
	  /// The addition simply uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountToAdd">  the amount to add </param>
	  /// <returns> an amount based on this with the specified amount added </returns>
	  public CurrencyAmount plus(double amountToAdd)
	  {
		return new CurrencyAmount(currency, amount + amountToAdd);
	  }

	  /// <summary>
	  /// Returns a copy of this {@code CurrencyAmount} with the specified amount subtracted.
	  /// <para>
	  /// This subtracts the specified amount to this monetary amount, returning a new object.
	  /// The addition simply uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountToSubtract">  the amount to subtract, in the same currency </param>
	  /// <returns> an amount based on this with the specified amount subtracted </returns>
	  /// <exception cref="IllegalArgumentException"> if the currencies are not equal </exception>
	  public CurrencyAmount minus(CurrencyAmount amountToSubtract)
	  {
		ArgChecker.notNull(amountToSubtract, "amountToSubtract");
		ArgChecker.isTrue(amountToSubtract.Currency.Equals(currency), "Unable to subtract amounts in different currencies");
		return minus(amountToSubtract.Amount);
	  }

	  /// <summary>
	  /// Returns a copy of this {@code CurrencyAmount} with the specified amount subtracted.
	  /// <para>
	  /// This subtracts the specified amount to this monetary amount, returning a new object.
	  /// The addition simply uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountToSubtract">  the amount to subtract </param>
	  /// <returns> an amount based on this with the specified amount subtracted </returns>
	  public CurrencyAmount minus(double amountToSubtract)
	  {
		return new CurrencyAmount(currency, amount - amountToSubtract);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this {@code CurrencyAmount} with the amount multiplied.
	  /// <para>
	  /// This takes this amount and multiplies it by the specified value.
	  /// The multiplication simply uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valueToMultiplyBy">  the scalar amount to multiply by </param>
	  /// <returns> an amount based on this with the amount multiplied </returns>
	  public CurrencyAmount multipliedBy(double valueToMultiplyBy)
	  {
		return new CurrencyAmount(currency, amount * valueToMultiplyBy);
	  }

	  /// <summary>
	  /// Applies an operation to the amount.
	  /// <para>
	  /// This is generally used to apply a mathematical operation to the amount.
	  /// For example, the operator could multiply the amount by a constant, or take the inverse.
	  /// <pre>
	  ///   multiplied = base.mapAmount(value -> (value &lt; 0 ? 0 : value * 3));
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  the operator to be applied to the amount </param>
	  /// <returns> a copy of this amount with the mapping applied to the original amount </returns>
	  public CurrencyAmount mapAmount(System.Func<double, double> mapper)
	  {
		ArgChecker.notNull(mapper, "mapper");
		return new CurrencyAmount(currency, mapper(amount));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this {@code CurrencyAmount} with the amount negated.
	  /// <para>
	  /// This takes this amount and negates it. If the amount is 0.0 or -0.0 the negated amount is 0.0.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an amount based on this with the amount negated </returns>
	  public CurrencyAmount negated()
	  {
		// Zero is treated as a special case to avoid creating -0.0 which produces surprising equality behaviour
		return new CurrencyAmount(currency, amount == 0d ? 0d : -amount);
	  }

	  /// <summary>
	  /// Returns a copy of this {@code CurrencyAmount} with a positive amount.
	  /// <para>
	  /// The result of this method will always be positive, where the amount is equal to {@code Math.abs(amount)}.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a currency amount based on this where the amount is positive </returns>
	  public CurrencyAmount positive()
	  {
		return amount < 0 ? negated() : this;
	  }

	  /// <summary>
	  /// Returns a copy of this {@code CurrencyAmount} with a negative amount.
	  /// <para>
	  /// The result of this method will always be negative, equal to {@code -Math.abs(amount)}.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a currency amount based on this where the amount is negative </returns>
	  public CurrencyAmount negative()
	  {
		return amount > 0 ? negated() : this;
	  }

	  //-------------------------------------------------------------------------

	  /// <summary>
	  /// Converts the current instance of <seealso cref="CurrencyAmount"/> to the equivalent <seealso cref="Money"/> instance.
	  /// This will result into loss of precision in the amount, since <seealso cref="Money"/> is storing the amount
	  /// rounded to the currency specification.
	  /// </summary>
	  /// <returns> The newly created instance of <seealso cref="Money"/>. </returns>
	  public Money toMoney()
	  {
		return Money.of(this.Currency, this.Amount);
	  }

	  /// <summary>
	  /// Converts this amount to an equivalent amount the specified currency.
	  /// <para>
	  /// The result will be expressed in terms of the given currency, converting
	  /// using the specified FX rate.
	  /// </para>
	  /// <para>
	  /// For example, if this represents 'GBP 100' and this method is called with
	  /// arguments {@code (USD, 1.6)} then the result will be 'USD 160'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="fxRate">  the FX rate from this currency to the result currency </param>
	  /// <returns> the converted instance, which should be expressed in the specified currency </returns>
	  /// <exception cref="IllegalArgumentException"> if the FX is not 1 when no conversion is required </exception>
	  public CurrencyAmount convertedTo(Currency resultCurrency, double fxRate)
	  {
		if (currency.Equals(resultCurrency))
		{
		  if (DoubleMath.fuzzyEquals(fxRate, 1d, 1e-8))
		  {
			return this;
		  }
		  throw new System.ArgumentException("FX rate must be 1 when no conversion required");
		}
		return CurrencyAmount.of(resultCurrency, amount * fxRate);
	  }

	  /// <summary>
	  /// Converts this amount to an equivalent amount in the specified currency.
	  /// <para>
	  /// The result will be expressed in terms of the given currency.
	  /// If conversion is needed, the provider will be used to supply the FX rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the converted instance, in the specified currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  public CurrencyAmount convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		if (currency.Equals(resultCurrency))
		{
		  return this;
		}
		double converted = rateProvider.convert(amount, currency, resultCurrency);
		return CurrencyAmount.of(resultCurrency, converted);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this currency amount to another.
	  /// <para>
	  /// This compares currencies alphabetically, then by amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other amount </param>
	  /// <returns> negative if less, zero if equal, positive if greater </returns>
	  public int CompareTo(CurrencyAmount other)
	  {
		return ComparisonChain.start().compare(currency, other.currency).compare(amount, other.amount).result();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this currency amount equals another.
	  /// </summary>
	  /// <param name="obj">  the other amount, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  CurrencyAmount other = (CurrencyAmount) obj;
		  return currency.Equals(other.currency) && JodaBeanUtils.equal(amount, other.amount);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the currency.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return currency.GetHashCode() * 31 + JodaBeanUtils.GetHashCode(amount);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the amount as a string.
	  /// <para>
	  /// The format is the currency code, followed by a space, followed by the
	  /// amount: '${currency} ${amount}'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency amount </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @ToString public String toString()
	  public override string ToString()
	  {
		return currency + " " + (DoubleMath.isMathematicalInteger(amount) ? Convert.ToString((long) amount) : Convert.ToString(amount));
	  }

	}

}