using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Throwables = com.google.common.@base.Throwables;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using IniFile = com.opengamma.strata.collect.io.IniFile;
	using PropertySet = com.opengamma.strata.collect.io.PropertySet;
	using ResourceConfig = com.opengamma.strata.collect.io.ResourceConfig;

	/// <summary>
	/// Internal loader of currency and currency pair data.
	/// <para>
	/// This loads configuration files for <seealso cref="Currency"/> and <seealso cref="CurrencyPair"/>.
	/// </para>
	/// </summary>
	internal sealed class CurrencyDataLoader
	{

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(CurrencyDataLoader).FullName);
	  /// <summary>
	  /// INI file for currency data.
	  /// </summary>
	  private const string CURRENCY_INI = "Currency.ini";
	  /// <summary>
	  /// INI file for currency pair data.
	  /// </summary>
	  private const string PAIR_INI = "CurrencyPair.ini";
	  /// <summary>
	  /// INI file containing a list of general currency data.
	  /// This in includes a list of currencies in priority order used to choose the base currency of the market
	  /// convention pair for pairs that aren't configured in currency-pair.ini.
	  /// </summary>
	  private const string CURRENCY_DATA_INI = "CurrencyData.ini";

	  // restricted constructor
	  private CurrencyDataLoader()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads the available currencies.
	  /// </summary>
	  /// <param name="loadHistoric">  whether to load the historic or active currencies </param>
	  /// <returns> the map of known currencies </returns>
	  internal static ImmutableMap<string, Currency> loadCurrencies(bool loadHistoric)
	  {
		try
		{
		  IniFile ini = ResourceConfig.combinedIniFile(ResourceConfig.orderedResources(CURRENCY_INI));
		  return parseCurrencies(ini, loadHistoric);

		}
		catch (Exception ex)
		{
		  // logging used because this is loaded in a static variable
		  log.severe(Throwables.getStackTraceAsString(ex));
		  // return an empty instance to avoid ExceptionInInitializerError
		  return ImmutableMap.of();
		}
	  }

	  // parse currency info
	  private static ImmutableMap<string, Currency> parseCurrencies(IniFile ini, bool loadHistoric)
	  {
		ImmutableMap.Builder<string, Currency> builder = ImmutableMap.builder();
		foreach (KeyValuePair<string, PropertySet> entry in ini.asMap().entrySet())
		{
		  string currencyCode = entry.Key;
		  if (currencyCode.Length == 3 && Currency.CODE_MATCHER.matchesAllOf(currencyCode))
		  {
			PropertySet properties = entry.Value;
			bool isHistoric = (properties.keys().contains("historic") && bool.Parse(properties.value("historic")));
			if (isHistoric == loadHistoric)
			{
			  int minorUnits = int.Parse(properties.value("minorUnitDigits"));
			  string triangulationCurrency = properties.value("triangulationCurrency");
			  builder.put(currencyCode, new Currency(currencyCode, minorUnits, triangulationCurrency));
			}
		  }
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads the available currency pairs.
	  /// </summary>
	  /// <returns> the map of known currency pairs, where the value is the number of digits in the rate </returns>
	  internal static ImmutableMap<CurrencyPair, int> loadPairs()
	  {
		try
		{
		  IniFile ini = ResourceConfig.combinedIniFile(ResourceConfig.orderedResources(PAIR_INI));
		  return parsePairs(ini);

		}
		catch (Exception ex)
		{
		  // logging used because this is loaded in a static variable
		  log.severe(Throwables.getStackTraceAsString(ex));
		  // return an empty instance to avoid ExceptionInInitializerError
		  return ImmutableMap.of();
		}
	  }

	  // parse pair info
	  private static ImmutableMap<CurrencyPair, int> parsePairs(IniFile ini)
	  {
		ImmutableMap.Builder<CurrencyPair, int> builder = ImmutableMap.builder();
		foreach (KeyValuePair<string, PropertySet> entry in ini.asMap().entrySet())
		{
		  string pairStr = entry.Key;
		  if (CurrencyPair.REGEX_FORMAT.matcher(pairStr).matches())
		  {
			CurrencyPair pair = CurrencyPair.parse(pairStr);
			PropertySet properties = entry.Value;
			int? rateDigits = int.Parse(properties.value("rateDigits"));
			builder.put(pair, rateDigits);
		  }
		}
		return builder.build();
	  }

	  /// <summary>
	  /// Loads the priority order of currencies, used to determine the base currency of the market convention pair
	  /// for pairs that aren't explicitly configured.
	  /// </summary>
	  /// <returns> a map of currency to order </returns>
	  internal static ImmutableMap<Currency, int> loadOrdering()
	  {
		try
		{
		  IniFile ini = ResourceConfig.combinedIniFile(ResourceConfig.orderedResources(CURRENCY_DATA_INI));
		  PropertySet section = ini.section("marketConventionPriority");
		  string list = section.value("ordering");
		  // The currency ordering is defined as a comma-separated list
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  IList<Currency> currencies = java.util.list.Split(",", true).Select(string.Trim).Select(Currency.of).collect(toImmutableList());

		  ImmutableMap.Builder<Currency, int> orderBuilder = ImmutableMap.builder();

		  for (int i = 0; i < currencies.Count; i++)
		  {
			orderBuilder.put(currencies[i], i + 1);
		  }
		  return orderBuilder.build();
		}
		catch (Exception ex)
		{
		  // logging used because this is loaded in a static variable
		  log.severe(Throwables.getStackTraceAsString(ex));
		  // return an empty instance to avoid ExceptionInInitializerError
		  return ImmutableMap.of();
		}
	  }
	}

}