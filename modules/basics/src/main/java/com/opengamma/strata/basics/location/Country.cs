using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.location
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using CharMatcher = com.google.common.@base.CharMatcher;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A country or territory.
	/// <para>
	/// This class represents a country or territory that it is useful to identify.
	/// Any two letter code may be used, however it is intended to use codes based on ISO-3166-1 alpha-2.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class Country : IComparable<Country>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// A cache of instances.
	  /// </summary>
	  private static readonly ConcurrentMap<string, Country> CACHE = new ConcurrentDictionary<string, Country>();
	  /// <summary>
	  /// The matcher for the code.
	  /// </summary>
	  private static readonly CharMatcher CODE_MATCHER = CharMatcher.inRange('A', 'Z');

	  // selected countries of Europe
	  /// <summary>
	  /// The region of 'EU' - Europe (special status in ISO-3166).
	  /// </summary>
	  public static readonly Country EU = of("EU");
	  /// <summary>
	  /// The country 'AT' - Austria.
	  /// </summary>
	  public static readonly Country AT = of("AT");
	  /// <summary>
	  /// The country 'BE' - Belgium.
	  /// </summary>
	  public static readonly Country BE = of("BE");
	  /// <summary>
	  /// The country 'CH' - Switzerland.
	  /// </summary>
	  public static readonly Country CH = of("CH");
	  /// <summary>
	  /// The currency 'CZ' - Czech Republic.
	  /// </summary>
	  public static readonly Country CZ = of("CZ");
	  /// <summary>
	  /// The country 'DE' - Germany.
	  /// </summary>
	  public static readonly Country DE = of("DE");
	  /// <summary>
	  /// The country 'DK' - Denmark.
	  /// </summary>
	  public static readonly Country DK = of("DK");
	  /// <summary>
	  /// The currency 'ES' - Spain.
	  /// </summary>
	  public static readonly Country ES = of("ES");
	  /// <summary>
	  /// The currency 'FI' - Finland.
	  /// </summary>
	  public static readonly Country FI = of("FI");
	  /// <summary>
	  /// The currency 'FR' - France.
	  /// </summary>
	  public static readonly Country FR = of("FR");
	  /// <summary>
	  /// The country 'GB' - United Kingdom.
	  /// </summary>
	  public static readonly Country GB = of("GB");
	  /// <summary>
	  /// The country 'GR' - Greece.
	  /// </summary>
	  public static readonly Country GR = of("GR");
	  /// <summary>
	  /// The currency 'HU' = Hungary.
	  /// </summary>
	  public static readonly Country HU = of("HU");
	  /// <summary>
	  /// The currency 'IE' - Ireland.
	  /// </summary>
	  public static readonly Country IE = of("IE");
	  /// <summary>
	  /// The currency 'IS' - Iceland.
	  /// </summary>
	  public static readonly Country IS = of("IS");
	  /// <summary>
	  /// The currency 'IT' - Italy.
	  /// </summary>
	  public static readonly Country IT = of("IT");
	  /// <summary>
	  /// The currency 'LU' - Luxembourg.
	  /// </summary>
	  public static readonly Country LU = of("LU");
	  /// <summary>
	  /// The currency 'NL' - Netherlands.
	  /// </summary>
	  public static readonly Country NL = of("NL");
	  /// <summary>
	  /// The currency 'NO' - Norway.
	  /// </summary>
	  public static readonly Country NO = of("NO");
	  /// <summary>
	  /// The currency 'PL' = Poland.
	  /// </summary>
	  public static readonly Country PL = of("PL");
	  /// <summary>
	  /// The currency 'PT' - Portugal.
	  /// </summary>
	  public static readonly Country PT = of("PT");
	  /// <summary>
	  /// The currency 'SE' - Sweden.
	  /// </summary>
	  public static readonly Country SE = of("SE");
	  /// <summary>
	  /// The currency 'SK' - Slovakia.
	  /// </summary>
	  public static readonly Country SK = of("SK");
	  /// <summary>
	  /// The country 'TR' - Turkey.
	  /// </summary>
	  public static readonly Country TR = of("TR");

	  // selected countries of the Americas
	  /// <summary>
	  /// The country 'AR' - Argentina.
	  /// </summary>
	  public static readonly Country AR = of("AR");
	  /// <summary>
	  /// The country 'BR' - Brazil.
	  /// </summary>
	  public static readonly Country BR = of("BR");
	  /// <summary>
	  /// The country 'CA' - Canada.
	  /// </summary>
	  public static readonly Country CA = of("CA");
	  /// <summary>
	  /// The country 'CL' - Chile.
	  /// </summary>
	  public static readonly Country CL = of("CL");
	  /// <summary>
	  /// The country 'MX' - Mexico.
	  /// </summary>
	  public static readonly Country MX = of("MX");
	  /// <summary>
	  /// The country 'US' - United States.
	  /// </summary>
	  public static readonly Country US = of("US");

	  // selected countries of the Rest of the World
	  /// <summary>
	  /// The country 'AU' - Australia.
	  /// </summary>
	  public static readonly Country AU = of("AU");
	  /// <summary>
	  /// The country 'CN' - China.
	  /// </summary>
	  public static readonly Country CN = of("CN");
	  /// <summary>
	  /// The currency 'EG' - Egypt.
	  /// </summary>
	  public static readonly Country EG = of("EG");
	  /// <summary>
	  /// The currency 'HK' - Hong Kong.
	  /// </summary>
	  public static readonly Country HK = of("HK");
	  /// <summary>
	  /// The country 'ID' - Indonesia.
	  /// </summary>
	  public static readonly Country ID = of("ID");
	  /// <summary>
	  /// The country 'IL' - Israel.
	  /// </summary>
	  public static readonly Country IL = of("IL");
	  /// <summary>
	  /// The country 'IN' - India.
	  /// </summary>
	  public static readonly Country IN = of("IN");
	  /// <summary>
	  /// The country 'JP' - Japan.
	  /// </summary>
	  public static readonly Country JP = of("JP");
	  /// <summary>
	  /// The country 'KR' - South Korea.
	  /// </summary>
	  public static readonly Country KR = of("KR");
	  /// <summary>
	  /// The country 'MY' - Malaysia.
	  /// </summary>
	  public static readonly Country MY = of("MY");
	  /// <summary>
	  /// The country 'NZ' - New Zealand.
	  /// </summary>
	  public static readonly Country NZ = of("NZ");
	  /// <summary>
	  /// The currency 'RU' = Russia.
	  /// </summary>
	  public static readonly Country RU = of("RU");
	  /// <summary>
	  /// The country 'SA' - Saudi Arabia.
	  /// </summary>
	  public static readonly Country SA = of("SA");
	  /// <summary>
	  /// The country 'SG' - Singapore.
	  /// </summary>
	  public static readonly Country SG = of("SG");
	  /// <summary>
	  /// The country 'TH' - Thailand.
	  /// </summary>
	  public static readonly Country TH = of("TH");
	  /// <summary>
	  /// The country 'ZA' - South Africa.
	  /// </summary>
	  public static readonly Country ZA = of("ZA");

	  /// <summary>
	  /// The country code.
	  /// </summary>
	  private readonly string code;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the set of available countries.
	  /// <para>
	  /// This contains all the countries that have been defined at the point
	  /// that the method is called.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an immutable set containing all registered countries </returns>
	  public static ISet<Country> AvailableCountries
	  {
		  get
		  {
			return ImmutableSet.copyOf(CACHE.values());
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified ISO-3166-1 alpha-2
	  /// two letter country code dynamically creating a country if necessary.
	  /// <para>
	  /// A country is uniquely identified by ISO-3166-1 alpha-2 two letter code.
	  /// This method creates the country if it is not known.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="countryCode">  the two letter country code, ASCII and upper case </param>
	  /// <returns> the singleton instance </returns>
	  /// <exception cref="IllegalArgumentException"> if the country code is invalid </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static Country of(String countryCode)
	  public static Country of(string countryCode)
	  {
		ArgChecker.notNull(countryCode, "countryCode");
		return CACHE.computeIfAbsent(countryCode, c => addCode(c));
	  }

	  // add code
	  private static Country addCode(string countryCode)
	  {
		ArgChecker.matches(CODE_MATCHER, 2, 2, countryCode, "countryCode", "[A-Z][A-Z]");
		return new Country(countryCode);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses a string to obtain a {@code Country}.
	  /// <para>
	  /// The parse is identical to <seealso cref="#of(String)"/> except that it will convert
	  /// letters to upper case first.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="countryCode">  the two letter country code, ASCII </param>
	  /// <returns> the singleton instance </returns>
	  /// <exception cref="IllegalArgumentException"> if the country code is invalid </exception>
	  public static Country parse(string countryCode)
	  {
		ArgChecker.notNull(countryCode, "countryCode");
		return of(countryCode.ToUpper(Locale.ENGLISH));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  /// <param name="code">  the two letter country code </param>
	  private Country(string code)
	  {
		this.code = code;
	  }

	  /// <summary>
	  /// Ensure singleton on deserialization.
	  /// </summary>
	  /// <returns> the singleton </returns>
	  private object readResolve()
	  {
		return Country.of(code);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the two letter ISO code.
	  /// </summary>
	  /// <returns> the two letter ISO code </returns>
	  public string Code
	  {
		  get
		  {
			return code;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this country to another.
	  /// <para>
	  /// The comparison sorts alphabetically by the two letter country code.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other country </param>
	  /// <returns> negative if less, zero if equal, positive if greater </returns>
	  public int CompareTo(Country other)
	  {
		return code.CompareTo(other.code);
	  }

	  /// <summary>
	  /// Checks if this country equals another country.
	  /// <para>
	  /// The comparison checks the two letter country code.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other country, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj == null || this.GetType() != obj.GetType())
		{
		  return false;
		}
		Country other = (Country) obj;
		return code.Equals(other.code);
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the country.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return code.GetHashCode();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a string representation of the country, which is the two letter code.
	  /// </summary>
	  /// <returns> the two letter country code </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @ToString public String toString()
	  public override string ToString()
	  {
		return code;
	  }

	}

}