using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using Splitter = com.google.common.@base.Splitter;
	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// An amount of a currency, rounded to match the currency specifications.
	/// <para>
	/// This class is similar to <seealso cref="CurrencyAmount"/>, but only exposes the rounded amounts.
	/// The rounding is done using <seealso cref="BigDecimal"/>, as BigDecimal.ROUND_HALF_UP. Given this operation,
	/// it should be assumed that the numbers are an approximation, and not an exact figure.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// </para>
	/// </summary>
	[Serializable]
	public class Money : FxConvertible<Money>, IComparable<Money>
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
	  private readonly decimal amount;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of {@code Money} for the specified <seealso cref="CurrencyAmount"/>.
	  /// </summary>
	  /// <param name="currencyAmount">  the instance of <seealso cref="CurrencyAmount"/> wrapping the currency and amount. </param>
	  /// <returns> the currency amount </returns>
	  public static Money of(CurrencyAmount currencyAmount)
	  {
		return new Money(currencyAmount.Currency, decimal.valueOf(currencyAmount.Amount));
	  }

	  /// <summary>
	  /// Obtains an instance of {@code Money} for the specified currency and amount.
	  /// </summary>
	  /// <param name="currency">  the currency the amount is in </param>
	  /// <param name="amount">  the amount of the currency to represent </param>
	  /// <returns> the currency amount </returns>
	  public static Money of(Currency currency, double amount)
	  {
		return new Money(currency, decimal.valueOf(amount));
	  }

	  /// <summary>
	  /// Obtains an instance of {@code Money} for the specified currency and amount.
	  /// </summary>
	  /// <param name="currency">  the currency the amount is in </param>
	  /// <param name="amount">  the amount of the currency to represent, as an instance of <seealso cref="BigDecimal"/> </param>
	  /// <returns> the currency amount </returns>
	  public static Money of(Currency currency, decimal amount)
	  {
		return new Money(currency, amount);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the string to produce a <seealso cref="Money"/>.
	  /// <para>
	  /// This parses the {@code toString} format of '${currency} ${amount}'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountStr">  the amount string </param>
	  /// <returns> the currency amount </returns>
	  /// <exception cref="IllegalArgumentException"> if the amount cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static Money parse(String amountStr)
	  public static Money parse(string amountStr)
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
		  return new Money(cur, new decimal(split[1]));
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
	  private Money(Currency currency, decimal amount)
	  {
		ArgChecker.notNull(currency, "currency");
		ArgChecker.notNull(amount, "amount");
		this.currency = currency;
		this.amount = currency.roundMinorUnits(amount);
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
	  public virtual Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  /// <summary>
	  /// Gets the amount of the currency as an instance of <seealso cref="BigDecimal"/>.
	  /// <para>
	  /// The amount will be rounded to the currency specifications.
	  /// </para>
	  /// <para>
	  /// For example, in the value 'GBP 12.34' the amount is 12.34.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the amount </returns>
	  public virtual decimal Amount
	  {
		  get
		  {
			return amount;
		  }
	  }

	  //-------------------------------------------------------------------------
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
	  public virtual Money convertedTo(Currency resultCurrency, decimal fxRate)
	  {
		if (currency.Equals(resultCurrency))
		{
		  if (DoubleMath.fuzzyEquals(fxRate.doubleValue(), 1d, 1e-8))
		  {
			return this;
		  }
		  throw new System.ArgumentException("FX rate must be 1 when no conversion required");
		}
		return Money.of(resultCurrency, amount * fxRate);
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
	  public virtual Money convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		if (currency.Equals(resultCurrency))
		{
		  return this;
		}
		double converted = rateProvider.convert(amount.doubleValue(), currency, resultCurrency);
		return Money.of(resultCurrency, converted);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this money to another.
	  /// <para>
	  /// This compares currencies alphabetically, then by amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other amount </param>
	  /// <returns> negative if less, zero if equal, positive if greater </returns>
	  public virtual int CompareTo(Money other)
	  {
		return ComparisonChain.start().compare(currency, other.currency).compare(amount, other.amount).result();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this money equals another.
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
		  Money other = (Money) obj;
		  return currency.Equals(other.currency) && amount.Equals(other.amount);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the currency.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return currency.GetHashCode() * 31 + amount.GetHashCode();
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
		return currency + " " + amount.ToString();
	  }

	}

}