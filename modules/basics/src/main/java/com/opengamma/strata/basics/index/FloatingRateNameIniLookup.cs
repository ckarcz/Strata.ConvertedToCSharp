using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{

	using Throwables = com.google.common.@base.Throwables;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IniFile = com.opengamma.strata.collect.io.IniFile;
	using PropertySet = com.opengamma.strata.collect.io.PropertySet;
	using ResourceConfig = com.opengamma.strata.collect.io.ResourceConfig;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Loads standard floating rate names from INI.
	/// </summary>
	internal sealed class FloatingRateNameIniLookup : NamedLookup<FloatingRateName>
	{

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(FloatingRateNameIniLookup).FullName);
	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly FloatingRateNameIniLookup INSTANCE = new FloatingRateNameIniLookup();

	  /// <summary>
	  /// INI file for floating rate names.
	  /// </summary>
	  private const string FLOATING_RATE_NAME_INI = "FloatingRateNameData.ini";

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FloatingRateNameIniLookup()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public IDictionary<string, FloatingRateName> lookupAll()
	  {
		return Loader.INSTANCE.names;
	  }

	  // finds a default
	  internal FloatingRateName defaultIborIndex(Currency currency)
	  {
		FloatingRateName frname = Loader.INSTANCE.iborDefaults.get(currency);
		if (frname == null)
		{
		  throw new System.ArgumentException("No default Ibor index for currency " + currency);
		}
		return frname;
	  }

	  // finds a default
	  internal FloatingRateName defaultOvernightIndex(Currency currency)
	  {
		FloatingRateName frname = Loader.INSTANCE.overnightDefaults.get(currency);
		if (frname == null)
		{
		  throw new System.ArgumentException("No default Overnight index for currency " + currency);
		}
		return frname;
	  }

	  //-------------------------------------------------------------------------
	  internal class Loader
	  {
		/// <summary>
		/// Instance. </summary>
		internal static readonly Loader INSTANCE = new Loader();

		/// <summary>
		/// The cache by name. </summary>
		internal readonly ImmutableMap<string, FloatingRateName> names;
		/// <summary>
		/// The Ibor defaults by currency. </summary>
		internal readonly ImmutableMap<Currency, FloatingRateName> iborDefaults;
		/// <summary>
		/// The Overnight defaults by currency. </summary>
		internal readonly ImmutableMap<Currency, FloatingRateName> overnightDefaults;

		//-------------------------------------------------------------------------
		internal Loader()
		{
		  ImmutableMap<string, FloatingRateName> names = ImmutableMap.of();
		  ImmutableMap<Currency, FloatingRateName> iborDefaults = ImmutableMap.of();
		  ImmutableMap<Currency, FloatingRateName> overnightDefaults = ImmutableMap.of();
		  try
		  {
			IniFile ini = ResourceConfig.combinedIniFile(FLOATING_RATE_NAME_INI);
			names = parseIndices(ini);
			iborDefaults = parseIborDefaults(ini, names);
			overnightDefaults = parseOvernightDefaults(ini, names);

		  }
		  catch (Exception ex)
		  {
			// logging used because this is loaded in a static variable
			log.severe(Throwables.getStackTraceAsString(ex));
			// return an empty instance to avoid ExceptionInInitializerError
		  }
		  this.names = names;
		  this.iborDefaults = iborDefaults;
		  this.overnightDefaults = overnightDefaults;
		}

		// parse the config file FloatingRateName.ini
		internal static ImmutableMap<string, FloatingRateName> parseIndices(IniFile ini)
		{
		  Dictionary<string, ImmutableFloatingRateName> map = new Dictionary<string, ImmutableFloatingRateName>();
		  parseSection(ini.section("ibor"), "-", FloatingRateType.IBOR, map);
		  parseFixingDateOffset(ini.section("iborFixingDateOffset"), map);
		  parseSection(ini.section("overnightCompounded"), "", FloatingRateType.OVERNIGHT_COMPOUNDED, map);
		  parseSection(ini.section("overnightAveraged"), "", FloatingRateType.OVERNIGHT_AVERAGED, map);
		  parseSection(ini.section("price"), "", FloatingRateType.PRICE, map);
		  return ImmutableMap.copyOf(map);
		}

		// parse a single section
		internal static void parseSection(PropertySet section, string indexNameSuffix, FloatingRateType type, Dictionary<string, ImmutableFloatingRateName> mutableMap)
		{

		  // find our names from the RHS of the key/value pairs
		  foreach (string key in section.keys())
		  {
			ImmutableFloatingRateName name = ImmutableFloatingRateName.of(key, section.value(key) + indexNameSuffix, type);
			mutableMap[key] = name;
			if (!mutableMap.ContainsKey(key.ToUpper(Locale.ENGLISH))) mutableMap.Add(key.ToUpper(Locale.ENGLISH), name);
		  }
		}

		// parse the fixing date offset section
		internal static void parseFixingDateOffset(PropertySet section, Dictionary<string, ImmutableFloatingRateName> mutableMap)
		{
		  // find our names from the RHS of the key/value pairs
		  foreach (string key in section.keys())
		  {
			int? days = int.Parse(section.value(key));
			ImmutableFloatingRateName name = mutableMap[key.ToUpper(Locale.ENGLISH)];
			ImmutableFloatingRateName updated = name.toBuilder().fixingDateOffsetDays(days).build();
			mutableMap[key.ToUpper(Locale.ENGLISH)] = updated;
		  }
		}

		//-------------------------------------------------------------------------
		// load currency defaults
		internal static ImmutableMap<Currency, FloatingRateName> parseIborDefaults(IniFile ini, ImmutableMap<string, FloatingRateName> names)
		{

		  ImmutableMap.Builder<Currency, FloatingRateName> map = ImmutableMap.builder();
		  PropertySet section = ini.section("currencyDefaultIbor");
		  foreach (string key in section.keys())
		  {
			FloatingRateName frname = names.get(section.value(key));
			if (frname == null)
			{
			  throw new System.ArgumentException("Invalid default Ibor index for currency " + key);
			}
			map.put(Currency.of(key), frname);
		  }
		  return map.build();
		}

		//-------------------------------------------------------------------------
		// load currency defaults
		internal static ImmutableMap<Currency, FloatingRateName> parseOvernightDefaults(IniFile ini, ImmutableMap<string, FloatingRateName> names)
		{

		  ImmutableMap.Builder<Currency, FloatingRateName> map = ImmutableMap.builder();
		  PropertySet section = ini.section("currencyDefaultOvernight");
		  foreach (string key in section.keys())
		  {
			FloatingRateName frname = names.get(section.value(key));
			if (frname == null)
			{
			  throw new System.ArgumentException("Invalid default Overnight index for currency " + key);
			}
			map.put(Currency.of(key), frname);
		  }
		  return map.build();
		}
	  }
	}

}