using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CharSource = com.google.common.io.CharSource;

	/// <summary>
	/// An INI file.
	/// <para>
	/// Represents an INI file together with the ability to parse it from a <seealso cref="CharSource"/>.
	/// </para>
	/// <para>
	/// The INI file format used here is deliberately simple.
	/// There are two elements - key-value pairs and sections.
	/// </para>
	/// <para>
	/// The basic element is a key-value pair.
	/// The key is separated from the value using the '=' symbol.
	/// The string ' = ' is searched for before '=' to allow an equals sign to be present
	/// in the key, which implies that this string cannot be in either the key or the value.
	/// Duplicate keys are allowed.
	/// For example 'key = value'.
	/// The equals sign and value may be omitted, in which case the value is an empty string.
	/// </para>
	/// <para>
	/// All properties are grouped into named sections.
	/// The section name occurs on a line by itself surrounded by square brackets.
	/// Duplicate section names are not allowed.
	/// For example '[section]'.
	/// </para>
	/// <para>
	/// Keys, values and section names are trimmed.
	/// Blank lines are ignored.
	/// Whole line comments begin with hash '#' or semicolon ';'.
	/// No escape format is available.
	/// Lookup is case sensitive.
	/// </para>
	/// <para>
	/// This example explains the format:
	/// <pre>
	///  # line comment
	///  [foo]
	///  key = value
	/// 
	///  [bar]
	///  key = value
	///  month = January
	/// </pre>
	/// </para>
	/// <para>
	/// The aim of this class is to parse the basic format.
	/// Interpolation of variables is not supported.
	/// </para>
	/// </summary>
	public sealed class IniFile
	{

	  /// <summary>
	  /// The INI sections.
	  /// </summary>
	  private readonly ImmutableMap<string, PropertySet> sectionMap;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the specified source as an INI file.
	  /// <para>
	  /// This parses the specified character source expecting an INI file format.
	  /// The resulting instance can be queried for each section in the file.
	  /// </para>
	  /// <para>
	  /// INI files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="source">  the INI file resource </param>
	  /// <returns> the INI file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static IniFile of(CharSource source)
	  {
		ArgChecker.notNull(source, "source");
		ImmutableList<string> lines = Unchecked.wrap(() => source.readLines());
		ImmutableMap<string, ImmutableListMultimap<string, string>> parsedIni = parse(lines);
		ImmutableMap.Builder<string, PropertySet> builder = ImmutableMap.builder();
		parsedIni.forEach((sectionName, sectionData) => builder.put(sectionName, PropertySet.of(sectionData)));
		return new IniFile(builder.build());
	  }

	  //-------------------------------------------------------------------------
	  // parses the INI file format
	  private static ImmutableMap<string, ImmutableListMultimap<string, string>> parse(ImmutableList<string> lines)
	  {
		// cannot use ArrayListMultiMap as it does not retain the order of the keys
		// whereas ImmutableListMultimap does retain the order of the keys
		IDictionary<string, ImmutableListMultimap.Builder<string, string>> ini = new LinkedHashMap<string, ImmutableListMultimap.Builder<string, string>>();
		ImmutableListMultimap.Builder<string, string> currentSection = null;
		int lineNum = 0;
		foreach (string line in lines)
		{
		  lineNum++;
		  line = line.Trim();
		  if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal) || line.StartsWith(";", StringComparison.Ordinal))
		  {
			continue;
		  }
		  if (line.StartsWith("[", StringComparison.Ordinal) && line.EndsWith("]", StringComparison.Ordinal))
		  {
			string sectionName = line.Substring(1, (line.Length - 1) - 1).Trim();
			if (ini.ContainsKey(sectionName))
			{
			  throw new System.ArgumentException("Invalid INI file, duplicate section not allowed, line " + lineNum);
			}
			currentSection = ImmutableListMultimap.builder();
			ini[sectionName] = currentSection;

		  }
		  else if (currentSection == null)
		  {
			throw new System.ArgumentException("Invalid INI file, properties must be within a [section], line " + lineNum);

		  }
		  else
		  {
			int equalsPosition = line.IndexOf(" = ", StringComparison.Ordinal);
			equalsPosition = equalsPosition < 0 ? line.IndexOf('=') : equalsPosition + 1;
			string key = (equalsPosition < 0 ? line.Trim() : line.Substring(0, equalsPosition).Trim());
			string value = (equalsPosition < 0 ? "" : line.Substring(equalsPosition + 1).Trim());
			if (key.Length == 0)
			{
			  throw new System.ArgumentException("Invalid INI file, empty key, line " + lineNum);
			}
			currentSection.put(key, value);
		  }
		}
		return MapStream.of(ini).mapValues(b => b.build()).toMap();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance, specifying the map of section to properties.
	  /// </summary>
	  /// <param name="sectionMap">  the map of sections </param>
	  /// <returns> the INI file </returns>
	  public static IniFile of(IDictionary<string, PropertySet> sectionMap)
	  {
		return new IniFile(ImmutableMap.copyOf(sectionMap));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  /// <param name="sectionMap">  the sections </param>
	  private IniFile(ImmutableMap<string, PropertySet> sectionMap)
	  {
		this.sectionMap = sectionMap;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the set of sections of this INI file.
	  /// </summary>
	  /// <returns> the set of sections </returns>
	  public ImmutableSet<string> sections()
	  {
		return sectionMap.Keys;
	  }

	  /// <summary>
	  /// Returns the INI file as a map.
	  /// <para>
	  /// The iteration order of the map matches that of the original file.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the INI file sections </returns>
	  public ImmutableMap<string, PropertySet> asMap()
	  {
		return sectionMap;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this INI file contains the specified section.
	  /// </summary>
	  /// <param name="name">  the section name </param>
	  /// <returns> true if the section exists </returns>
	  public bool contains(string name)
	  {
		ArgChecker.notNull(name, "name");
		return sectionMap.containsKey(name);
	  }

	  /// <summary>
	  /// Gets a single section of this INI file.
	  /// <para>
	  /// This returns the section associated with the specified name.
	  /// If the section does not exist an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the section name </param>
	  /// <returns> the INI file section </returns>
	  /// <exception cref="IllegalArgumentException"> if the section does not exist </exception>
	  public PropertySet section(string name)
	  {
		ArgChecker.notNull(name, "name");
		if (contains(name) == false)
		{
		  throw new System.ArgumentException("Unknown INI file section: " + name);
		}
		return sectionMap.get(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this INI file equals another.
	  /// <para>
	  /// The comparison checks the content.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other file, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is IniFile)
		{
		  return sectionMap.Equals(((IniFile) obj).sectionMap);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the INI file.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return sectionMap.GetHashCode();
	  }

	  /// <summary>
	  /// Returns a string describing the INI file.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		return sectionMap.ToString();
	  }

	}

}