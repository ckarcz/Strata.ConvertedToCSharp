using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using CharMatcher = com.google.common.@base.CharMatcher;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A unit of currency.
	/// <para>
	/// This class represents a unit of currency such as the British Pound, Euro or US Dollar.
	/// The currency is represented by a three letter code, intended to be ISO-4217.
	/// </para>
	/// <para>
	/// It is recommended to define currencies in advance using the {@code Currency.ini} file.
	/// Standard configuration includes many commonly used currencies.
	/// </para>
	/// <para>
	/// Only currencies listed in configuration will be returned by <seealso cref="#getAvailableCurrencies()"/>.
	/// If a currency is requested that is not defined in configuration, it will still be created,
	/// however it will have the default value of zero for the minor units and 'USD' for
	/// the triangulation currency.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class Currency : IComparable<Currency>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The matcher for the code.
	  /// </summary>
	  internal static readonly CharMatcher CODE_MATCHER = CharMatcher.inRange('A', 'Z');
	  /// <summary>
	  /// The configured instances.
	  /// </summary>
	  private static readonly ImmutableMap<string, Currency> CONFIGURED = CurrencyDataLoader.loadCurrencies(false);
	  /// <summary>
	  /// A cache of dynamically created instances, initialized with some historic currencies.
	  /// </summary>
	  private static readonly ConcurrentMap<string, Currency> DYNAMIC = new ConcurrentDictionary<string, Currency>(CurrencyDataLoader.loadCurrencies(true));

	  // a selection of commonly traded, stable currencies
	  /// <summary>
	  /// The currency 'USD' - United States Dollar.
	  /// </summary>
	  public static readonly Currency USD = of("USD");
	  /// <summary>
	  /// The currency 'EUR' - Euro.
	  /// </summary>
	  public static readonly Currency EUR = of("EUR");
	  /// <summary>
	  /// The currency 'JPY' - Japanese Yen.
	  /// </summary>
	  public static readonly Currency JPY = of("JPY");
	  /// <summary>
	  /// The currency 'GBP' - British pound.
	  /// </summary>
	  public static readonly Currency GBP = of("GBP");
	  /// <summary>
	  /// The currency 'CHF' - Swiss Franc.
	  /// </summary>
	  public static readonly Currency CHF = of("CHF");
	  /// <summary>
	  /// The currency 'AUD' - Australian Dollar.
	  /// </summary>
	  public static readonly Currency AUD = of("AUD");
	  /// <summary>
	  /// The currency 'CAD' - Canadian Dollar.
	  /// </summary>
	  public static readonly Currency CAD = of("CAD");
	  /// <summary>
	  /// The currency 'NZD' - New Zealand Dollar.
	  /// </summary>
	  public static readonly Currency NZD = of("NZD");

	  // a selection of other currencies
	  /// <summary>
	  /// The currency 'AED' - UAE Dirham.
	  /// </summary>
	  public static readonly Currency AED = of("AED");
	  /// <summary>
	  /// The currency 'ARS' - Argentine Peso.
	  /// </summary>
	  public static readonly Currency ARS = of("ARS");
	  /// <summary>
	  /// The currency 'BGN' - Bulgarian Lev.
	  /// </summary>
	  public static readonly Currency BGN = of("BGN");
	  /// <summary>
	  /// The currency 'BHD' - Bahraini Dinar.
	  /// </summary>
	  public static readonly Currency BHD = of("BHD");
	  /// <summary>
	  /// The currency 'BRL' - Brazilian Real.
	  /// </summary>
	  public static readonly Currency BRL = of("BRL");
	  /// <summary>
	  /// The currency 'CLP' - Chilean Peso.
	  /// </summary>
	  public static readonly Currency CLP = of("CLP");
	  /// <summary>
	  /// The currency 'CNY' - Chinese Yuan.
	  /// </summary>
	  public static readonly Currency CNY = of("CNY");
	  /// <summary>
	  /// The currency 'COP' - Colombian Peso.
	  /// </summary>
	  public static readonly Currency COP = of("COP");
	  /// <summary>
	  /// The currency 'CZK' - Czeck Krona.
	  /// </summary>
	  public static readonly Currency CZK = of("CZK");
	  /// <summary>
	  /// The currency 'DKK' - Danish Krone.
	  /// </summary>
	  public static readonly Currency DKK = of("DKK");
	  /// <summary>
	  /// The currency 'EGP' - Egyptian Pound.
	  /// </summary>
	  public static readonly Currency EGP = of("EGP");
	  /// <summary>
	  /// The currency 'HKD' - Hong Kong Dollar.
	  /// </summary>
	  public static readonly Currency HKD = of("HKD");
	  /// <summary>
	  /// The currency 'HRK' - Croatian Kuna.
	  /// </summary>
	  public static readonly Currency HRK = of("HRK");
	  /// <summary>
	  /// The currency 'HUF' = Hugarian Forint.
	  /// </summary>
	  public static readonly Currency HUF = of("HUF");
	  /// <summary>
	  /// The currency 'IDR' = Indonesian Rupiah.
	  /// </summary>
	  public static readonly Currency IDR = of("IDR");
	  /// <summary>
	  /// The currency 'ILS' = Israeli Shekel.
	  /// </summary>
	  public static readonly Currency ILS = of("ILS");
	  /// <summary>
	  /// The currency 'INR' = Indian Rupee.
	  /// </summary>
	  public static readonly Currency INR = of("INR");
	  /// <summary>
	  /// The currency 'ISK' = Icelandic Krone.
	  /// </summary>
	  public static readonly Currency ISK = of("ISK");
	  /// <summary>
	  /// The currency 'KRW' = South Korean Won.
	  /// </summary>
	  public static readonly Currency KRW = of("KRW");
	  /// <summary>
	  /// The currency 'MXN' - Mexican Peso.
	  /// </summary>
	  public static readonly Currency MXN = of("MXN");
	  /// <summary>
	  /// The currency 'MYR' - Malaysian Ringgit.
	  /// </summary>
	  public static readonly Currency MYR = of("MYR");
	  /// <summary>
	  /// The currency 'NOK' - Norwegian Krone.
	  /// </summary>
	  public static readonly Currency NOK = of("NOK");
	  /// <summary>
	  /// The currency 'PEN' - Peruvian Nuevo Sol.
	  /// </summary>
	  public static readonly Currency PEN = of("PEN");
	  /// <summary>
	  /// The currency 'PHP' - Philippine Peso.
	  /// </summary>
	  public static readonly Currency PHP = of("PHP");
	  /// <summary>
	  /// The currency 'PKR' - Pakistani Rupee.
	  /// </summary>
	  public static readonly Currency PKR = of("PKR");
	  /// <summary>
	  /// The currency 'PLN' - Polish Zloty.
	  /// </summary>
	  public static readonly Currency PLN = of("PLN");
	  /// <summary>
	  /// The currency 'RON' - Romanian New Leu.
	  /// </summary>
	  public static readonly Currency RON = of("RON");
	  /// <summary>
	  /// The currency 'RUB' - Russian Ruble.
	  /// </summary>
	  public static readonly Currency RUB = of("RUB");
	  /// <summary>
	  /// The currency 'SAR' - Saudi Riyal.
	  /// </summary>
	  public static readonly Currency SAR = of("SAR");
	  /// <summary>
	  /// The currency 'SEK' - Swedish Krona.
	  /// </summary>
	  public static readonly Currency SEK = of("SEK");
	  /// <summary>
	  /// The currency 'SGD' - Singapore Dollar.
	  /// </summary>
	  public static readonly Currency SGD = of("SGD");
	  /// <summary>
	  /// The currency 'THB' - Thai Baht.
	  /// </summary>
	  public static readonly Currency THB = of("THB");
	  /// <summary>
	  /// The currency 'TRY' - Turkish Lira.
	  /// </summary>
	  public static readonly Currency TRY = of("TRY");
	  /// <summary>
	  /// The currency 'TWD' - New Taiwan Dollar.
	  /// </summary>
	  public static readonly Currency TWD = of("TWD");
	  /// <summary>
	  /// The currency 'UAH' - Ukrainian Hryvnia.
	  /// </summary>
	  public static readonly Currency UAH = of("UAH");
	  /// <summary>
	  /// The currency 'ZAR' - South African Rand.
	  /// </summary>
	  public static readonly Currency ZAR = of("ZAR");

	  // special cases
	  /// <summary>
	  /// The currency 'XXX' - No applicable currency.
	  /// </summary>
	  public static readonly Currency XXX = of("XXX");
	  /// <summary>
	  /// The currency 'XAG' - Silver (troy ounce).
	  /// </summary>
	  public static readonly Currency XAG = of("XAG");
	  /// <summary>
	  /// The currency 'XAU' - Gold (troy ounce).
	  /// </summary>
	  public static readonly Currency XAU = of("XAU");
	  /// <summary>
	  /// The currency 'XPD' - Paladium (troy ounce).
	  /// </summary>
	  public static readonly Currency XPD = of("XPD");
	  /// <summary>
	  /// The currency 'XPT' - Platinum (troy ounce).
	  /// </summary>
	  public static readonly Currency XPT = of("XPT");

	  /// <summary>
	  /// The currency code.
	  /// </summary>
	  private readonly string code;
	  /// <summary>
	  /// The number of fraction digits, such as 2 for cents in the dollar.
	  /// </summary>
	  [NonSerialized]
	  private readonly int minorUnitDigits;
	  /// <summary>
	  /// The triangulation currency.
	  /// Due to initialization ordering, cannot guarantee that USD/EUR is loaded first, so this must be a string.
	  /// </summary>
	  [NonSerialized]
	  private readonly string triangulationCurrency;
	  /// <summary>
	  /// The cached hash code.
	  /// </summary>
	  [NonSerialized]
	  private readonly int cachedHashCode;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the set of configured currencies.
	  /// <para>
	  /// This contains all the currencies that have been defined in configuration.
	  /// Any currency instances that have been dynamically created are not included.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an immutable set containing all registered currencies </returns>
	  public static ISet<Currency> AvailableCurrencies
	  {
		  get
		  {
			return ImmutableSet.copyOf(CONFIGURED.values());
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance for the specified ISO-4217 three letter currency code.
	  /// <para>
	  /// A currency is uniquely identified by ISO-4217 three letter code.
	  /// Currencies should be defined in configuration before they can be used.
	  /// If the requested currency is not defined in configuration, it will still be created,
	  /// however it will have the default value of zero for the minor units and 'USD' for
	  /// the triangulation currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyCode">  the three letter currency code, ASCII and upper case </param>
	  /// <returns> the singleton instance </returns>
	  /// <exception cref="IllegalArgumentException"> if the currency code is invalid </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static Currency of(String currencyCode)
	  public static Currency of(string currencyCode)
	  {
		ArgChecker.notNull(currencyCode, "currencyCode");
		Currency currency = CONFIGURED.get(currencyCode);
		if (currency == null)
		{
		  return addCode(currencyCode);
		}
		return currency;
	  }

	  // add code
	  private static Currency addCode(string currencyCode)
	  {
		ArgChecker.matches(CODE_MATCHER, 3, 3, currencyCode, "currencyCode", "[A-Z][A-Z][A-Z]");
		return DYNAMIC.computeIfAbsent(currencyCode, code => new Currency(code, 0, "USD"));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses a string to obtain a {@code Currency}.
	  /// <para>
	  /// The parse is identical to <seealso cref="#of(String)"/> except that it will convert
	  /// letters to upper case first.
	  /// If the requested currency is not defined in configuration, it will still be created,
	  /// however it will have the default value of zero for the minor units and 'USD' for
	  /// the triangulation currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyCode">  the three letter currency code, ASCII </param>
	  /// <returns> the singleton instance </returns>
	  /// <exception cref="IllegalArgumentException"> if the currency code is invalid </exception>
	  public static Currency parse(string currencyCode)
	  {
		ArgChecker.notNull(currencyCode, "currencyCode");
		return of(currencyCode.ToUpper(Locale.ENGLISH));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor, called only by {@code CurrencyProperties}.
	  /// </summary>
	  /// <param name="code">  the three letter currency code, validated </param>
	  /// <param name="fractionDigits">  the number of fraction digits, validated </param>
	  /// <param name="triangulationCurrency">  the triangulation currency </param>
	  internal Currency(string code, int fractionDigits, string triangulationCurrency)
	  {
		this.code = code;
		this.minorUnitDigits = fractionDigits;
		this.triangulationCurrency = triangulationCurrency;
		// total universe is (26 * 26 * 26) codes, which can provide a unique hash code
		this.cachedHashCode = ((code[0] - 64) << 16) + ((code[1] - 64) << 8) + (code[2] - 64);
	  }

	  /// <summary>
	  /// Ensure singleton on deserialization.
	  /// </summary>
	  /// <returns> the singleton </returns>
	  private object readResolve()
	  {
		return Currency.of(code);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the three letter ISO code.
	  /// </summary>
	  /// <returns> the three letter ISO code </returns>
	  public string Code
	  {
		  get
		  {
			return code;
		  }
	  }

	  /// <summary>
	  /// Gets the number of digits in the minor unit.
	  /// <para>
	  /// For example, 'USD' will return 2, indicating that there are two digits,
	  /// corresponding to cents in the dollar.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of fraction digits </returns>
	  public int MinorUnitDigits
	  {
		  get
		  {
			return minorUnitDigits;
		  }
	  }

	  /// <summary>
	  /// Gets the preferred triangulation currency.
	  /// <para>
	  /// When obtaining a market quote for a currency, the triangulation currency
	  /// is used if no direct rate can be found.
	  /// For example, there is no direct rate for 'CZK/SGD'. Instead 'CZK' might be defined to
	  /// triangulate via 'EUR' and 'SGD' with 'USD'. Since the three rates, 'CZK/EUR', 'EUR/USD'
	  /// and 'USD/SGD' can be obtained, a rate can be determined for 'CZK/SGD'.
	  /// Note that most currencies triangulate via 'USD'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the triangulation currency </returns>
	  public Currency TriangulationCurrency
	  {
		  get
		  {
			return Currency.of(triangulationCurrency);
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Rounds the specified amount according to the minor units.
	  /// <para>
	  /// For example, 'USD' has 2 minor digits, so 63.347 will be rounded to 63.35.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to round </param>
	  /// <returns> the rounded amount </returns>
	  public double roundMinorUnits(double amount)
	  {
		return roundMinorUnits(decimal.valueOf(amount));
	  }

	  /// <summary>
	  /// Rounds the specified amount according to the minor units.
	  /// <para>
	  /// For example, 'USD' has 2 minor digits, so 63.347 will be rounded to 63.35.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to round </param>
	  /// <returns> the rounded amount </returns>
	  public decimal roundMinorUnits(decimal amount)
	  {
		return amount.setScale(minorUnitDigits, RoundingMode.HALF_UP);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this currency to another.
	  /// <para>
	  /// The comparison sorts alphabetically by the three letter currency code.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other currency </param>
	  /// <returns> negative if less, zero if equal, positive if greater </returns>
	  public int CompareTo(Currency other)
	  {
		// hash code is unique and ordered so can be used for compareTo
		return cachedHashCode - other.cachedHashCode;
	  }

	  /// <summary>
	  /// Checks if this currency equals another currency.
	  /// <para>
	  /// The comparison checks the three letter currency code.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other currency, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is Currency)
		{
		  return Equals((Currency) obj);
		}
		return false;
	  }

	  // called by CurrencyAmount
	  internal bool Equals(Currency other)
	  {
		// hash code is unique so can be used for equals
		return other.cachedHashCode == cachedHashCode;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the currency.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return cachedHashCode;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a string representation of the currency, which is the three letter code.
	  /// </summary>
	  /// <returns> the three letter currency code </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @ToString public String toString()
	  public override string ToString()
	  {
		return code;
	  }

	}

}