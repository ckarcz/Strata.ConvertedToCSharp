using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// An ordered pair of currencies, such as 'EUR/USD'.
	/// <para>
	/// This could be used to identify a pair of currencies for quoting rates in FX deals.
	/// See <seealso cref="FxRate"/> for the representation that contains a rate.
	/// </para>
	/// <para>
	/// It is recommended to define currencies in advance using the {@code CurrencyPair.ini} file.
	/// Standard configuration includes many commonly used currency pairs.
	/// </para>
	/// <para>
	/// Only currencies listed in configuration will be returned by <seealso cref="#getAvailablePairs()"/>.
	/// If a pair is requested that is not defined in configuration, it will still be created,
	/// however the market convention information will be generated.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class CurrencyPair
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Regular expression to parse the textual format.
	  /// Three ASCII upper case letters, a slash, and another three ASCII upper case letters.
	  /// </summary>
	  internal static readonly Pattern REGEX_FORMAT = Pattern.compile("([A-Z]{3})/([A-Z]{3})");
	  /// <summary>
	  /// The configured instances and associated rate digits.
	  /// </summary>
	  private static readonly ImmutableMap<CurrencyPair, int> CONFIGURED = CurrencyDataLoader.loadPairs();
	  /// <summary>
	  /// Ordering of each currency, used when choosing a market convention pair when there is no configuration.
	  /// The currency closer to the start of the list (with the lower ordering) is the base currency.
	  /// </summary>
	  private static readonly ImmutableMap<Currency, int> CURRENCY_ORDERING = CurrencyDataLoader.loadOrdering();

	  /// <summary>
	  /// The base currency of the pair.
	  /// In the pair 'AAA/BBB' the base is 'AAA'.
	  /// </summary>
	  private readonly Currency @base;
	  /// <summary>
	  /// The counter currency of the pair.
	  /// In the pair 'AAA/BBB' the counter is 'BBB'.
	  /// </summary>
	  private readonly Currency counter;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the set of configured currency pairs.
	  /// <para>
	  /// This contains all the currency pairs that have been defined in configuration.
	  /// Any currency pair instances that have been dynamically created are not included.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an immutable set containing all registered currency pairs </returns>
	  public static ISet<CurrencyPair> AvailablePairs
	  {
		  get
		  {
			return CONFIGURED.Keys;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from two currencies.
	  /// <para>
	  /// The first currency is the base and the second is the counter.
	  /// The two currencies may be the same.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="base">  the base currency </param>
	  /// <param name="counter">  the counter currency </param>
	  /// <returns> the currency pair </returns>
	  public static CurrencyPair of(Currency @base, Currency counter)
	  {
		ArgChecker.notNull(@base, "base");
		ArgChecker.notNull(counter, "counter");
		return new CurrencyPair(@base, counter);
	  }

	  /// <summary>
	  /// Parses a currency pair from a string with format AAA/BBB.
	  /// <para>
	  /// The parsed format is '${baseCurrency}/${counterCurrency}'.
	  /// Currency parsing is case insensitive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pairStr">  the currency pair as a string AAA/BBB </param>
	  /// <returns> the currency pair </returns>
	  /// <exception cref="IllegalArgumentException"> if the pair cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static CurrencyPair parse(String pairStr)
	  public static CurrencyPair parse(string pairStr)
	  {
		ArgChecker.notNull(pairStr, "pairStr");
		Matcher matcher = REGEX_FORMAT.matcher(pairStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException("Invalid currency pair: " + pairStr);
		}
		Currency @base = Currency.parse(matcher.group(1));
		Currency counter = Currency.parse(matcher.group(2));
		return new CurrencyPair(@base, counter);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a currency pair.
	  /// </summary>
	  /// <param name="base">  the base currency, validated not null </param>
	  /// <param name="counter">  the counter currency, validated not null </param>
	  private CurrencyPair(Currency @base, Currency counter)
	  {
		this.@base = @base;
		this.counter = counter;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the inverse currency pair.
	  /// <para>
	  /// The inverse pair has the same currencies but in reverse order.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the inverse pair </returns>
	  public CurrencyPair inverse()
	  {
		return new CurrencyPair(counter, @base);
	  }

	  /// <summary>
	  /// Checks if the currency pair contains the supplied currency as either its base or counter.
	  /// </summary>
	  /// <param name="currency">  the currency to check against the pair </param>
	  /// <returns> true if the currency is either the base or counter currency in the pair </returns>
	  public bool contains(Currency currency)
	  {
		ArgChecker.notNull(currency, "currency");
		return @base.Equals(currency) || counter.Equals(currency);
	  }

	  /// <summary>
	  /// Finds the other currency in the pair.
	  /// <para>
	  /// If the pair is AAA/BBB, then passing in AAA will return BBB, and passing in BBB will return AAA.
	  /// Passing in CCC will throw an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency to check </param>
	  /// <returns> the other currency in the pair </returns>
	  /// <exception cref="IllegalArgumentException"> if the specified currency is not one of those in the pair </exception>
	  public Currency other(Currency currency)
	  {
		ArgChecker.notNull(currency, "currency");
		if (currency.Equals(@base))
		{
		  return counter;
		}
		if (currency.Equals(counter))
		{
		  return @base;
		}
		throw new System.ArgumentException("Unable to find other currency, " + currency + " is not present in " + ToString());
	  }

	  /// <summary>
	  /// Checks if this currency pair is an identity pair.
	  /// <para>
	  /// The identity pair is one where the base and counter currency are the same..
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if this pair is an identity pair </returns>
	  public bool Identity
	  {
		  get
		  {
			return @base.Equals(counter);
		  }
	  }

	  /// <summary>
	  /// Checks if this currency pair is the inverse of the specified pair.
	  /// <para>
	  /// This could be used to check if an FX rate specified in one currency pair needs inverting.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other currency pair </param>
	  /// <returns> true if the currency is the inverse of the specified pair </returns>
	  public bool isInverse(CurrencyPair other)
	  {
		ArgChecker.notNull(other, "currencyPair");
		return @base.Equals(other.counter) && counter.Equals(other.@base);
	  }

	  /// <summary>
	  /// Finds the currency pair that is a cross between this pair and the other pair.
	  /// <para>
	  /// The cross is only returned if the two pairs contains three currencies in total,
	  /// such as AAA/BBB and BBB/CCC and neither pair is an identity such as AAA/AAA.
	  /// <ul>
	  /// <li>Given two pairs AAA/BBB and BBB/CCC the result will be AAA/CCC or CCC/AAA as per the market convention.
	  /// <li>Given two pairs AAA/BBB and CCC/DDD the result will be empty.
	  /// <li>Given two pairs AAA/AAA and AAA/BBB the result will be empty.
	  /// <li>Given two pairs AAA/BBB and AAA/BBB the result will be empty.
	  /// <li>Given two pairs AAA/AAA and AAA/AAA the result will be empty.
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other currency pair </param>
	  /// <returns> the cross currency pair, or empty if no cross currency pair can be created </returns>
	  public Optional<CurrencyPair> cross(CurrencyPair other)
	  {
		ArgChecker.notNull(other, "other");
		if (Identity || other.Identity || this.Equals(other) || this.Equals(other.inverse()))
		{
		  return null;
		}
		// AAA/BBB cross BBB/CCC
		if (counter.Equals(other.@base))
		{
		  return (of(@base, other.counter).toConventional());
		}
		// AAA/BBB cross CCC/BBB
		if (counter.Equals(other.counter))
		{
		  return (of(@base, other.@base).toConventional());
		}
		// BBB/AAA cross BBB/CCC
		if (@base.Equals(other.@base))
		{
		  return (of(counter, other.counter).toConventional());
		}
		// BBB/AAA cross CCC/BBB
		if (@base.Equals(other.counter))
		{
		  return (of(counter, other.@base).toConventional());
		}
		return null;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this currency pair is a conventional currency pair.
	  /// <para>
	  /// A market convention determines that 'EUR/USD' should be used and not 'USD/EUR'.
	  /// This knowledge is encoded in configuration for a standard set of pairs.
	  /// </para>
	  /// <para>
	  /// It is possible to create two different currency pairs from any two currencies, and it is guaranteed that
	  /// exactly one of the pairs will be the market convention pair.
	  /// </para>
	  /// <para>
	  /// If a currency pair is not explicitly listed in the configuration, a priority ordering of currencies
	  /// is used to choose base currency of the pair that is treated as conventional.
	  /// </para>
	  /// <para>
	  /// If there is no configuration available to determine which pair is the market convention, a pair will
	  /// be chosen arbitrarily but deterministically. This ensures the same pair will be chosen for any two
	  /// currencies even if the {@code CurrencyPair} instances are created independently.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if the currency pair follows the market convention, false if it does not </returns>
	  public bool Conventional
	  {
		  get
		  {
			// If the pair is in the configuration file it is a market convention pair
			if (CONFIGURED.containsKey(this))
			{
			  return true;
			}
			// Get the priorities of the currencies to determine which should be the base
			int? basePriority = CURRENCY_ORDERING.getOrDefault(@base, int.MaxValue);
			int? counterPriority = CURRENCY_ORDERING.getOrDefault(counter, int.MaxValue);
    
			// If a currency is earlier in the list it has a higher priority
			if (basePriority < counterPriority)
			{
			  return true;
			}
			else if (basePriority > counterPriority)
			{
			  return false;
			}
			// Neither currency is included in the list defining the ordering.
			// Use lexicographical ordering. It's arbitrary but consistent. This ensures two CurrencyPair instances
			// created independently for the same two currencies will always choose the same conventional pair.
			// The natural ordering of Currency is the same as the natural ordering of the currency code but
			// comparing the Currency instances is more efficient.
			// This is <= 0 so that a pair with two copies of the same currency is conventional
			return @base.CompareTo(counter) <= 0;
		  }
	  }

	  /// <summary>
	  /// Returns the market convention currency pair for the currencies in the pair.
	  /// <para>
	  /// If <seealso cref="#isConventional()"/> is {@code true} this method returns {@code this}, otherwise
	  /// it returns the <seealso cref="#inverse"/> pair.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the market convention currency pair for the currencies in the pair </returns>
	  public CurrencyPair toConventional()
	  {
		return Conventional ? this : inverse();
	  }

	  /// <summary>
	  /// Returns the set of currencies contains in the pair.
	  /// </summary>
	  /// <returns> the set of currencies, with iteration in conventional order </returns>
	  public ImmutableSet<Currency> toSet()
	  {
		if (Conventional)
		{
		  return ImmutableSet.of(@base, counter);
		}
		else
		{
		  return ImmutableSet.of(counter, @base);
		}
	  }

	  /// <summary>
	  /// Gets the number of digits in the rate.
	  /// <para>
	  /// If this rate is a conventional currency pair defined in configuration,
	  /// then the number of digits in a market FX rate quote is returned.
	  /// </para>
	  /// <para>
	  /// If the currency pair is not defined in configuration the sum of the
	  /// <seealso cref="Currency#getMinorUnitDigits() minor unit digits"/> from the two currencies is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of digits in the FX rate </returns>
	  public int RateDigits
	  {
		  get
		  {
			int? digits = CONFIGURED.get(this);
    
			if (digits != null)
			{
			  return digits.Value;
			}
			int? inverseDigits = CONFIGURED.get(inverse());
    
			if (inverseDigits != null)
			{
			  return inverseDigits.Value;
			}
			return @base.MinorUnitDigits + counter.MinorUnitDigits;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the base currency of the pair.
	  /// <para>
	  /// In the pair 'AAA/BBB' the base is 'AAA'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the base currency </returns>
	  public Currency Base
	  {
		  get
		  {
			return @base;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the counter currency of the pair.
	  /// <para>
	  /// In the pair 'AAA/BBB' the counter is 'BBB'.
	  /// </para>
	  /// <para>
	  /// The counter currency is also known as the <i>quote currency</i> or the <i>variable currency</i>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the counter currency </returns>
	  public Currency Counter
	  {
		  get
		  {
			return counter;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this currency pair equals another.
	  /// <para>
	  /// The comparison checks the two currencies.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other currency pair, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  CurrencyPair other = (CurrencyPair) obj;
		  return @base.Equals(other.@base) && counter.Equals(other.counter);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the currency.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return @base.GetHashCode() ^ counter.GetHashCode();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the formatted string version of the currency pair.
	  /// <para>
	  /// The format is '${baseCurrency}/${counterCurrency}'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the formatted string </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @ToString public String toString()
	  public override string ToString()
	  {
		return @base.Code + "/" + counter.Code;
	  }

	}

}