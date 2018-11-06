using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using CharSource = com.google.common.io.CharSource;

	/// <summary>
	/// A properties file.
	/// <para>
	/// Represents a properties file together with the ability to parse it from a <seealso cref="CharSource"/>.
	/// This is similar to <seealso cref="Properties"/> but with a simpler format and without extending <seealso cref="Map"/>.
	/// Duplicate keys are allowed and handled.
	/// </para>
	/// <para>
	/// The properties file format used here is deliberately simple.
	/// There is only one element - key-value pairs.
	/// </para>
	/// <para>
	/// The key is separated from the value using the '=' symbol.
	/// The string ' = ' is searched for before '=' to allow an equals sign to be present
	/// in the key, which implies that this string cannot be in either the key or the value.
	/// Duplicate keys are allowed.
	/// For example 'key = value'.
	/// The equals sign and value may be omitted, in which case the value is an empty string.
	/// </para>
	/// <para>
	/// Keys and values are trimmed.
	/// Blank lines are ignored.
	/// Whole line comments begin with hash '#' or semicolon ';'.
	/// No escape format is available.
	/// Lookup is case sensitive.
	/// </para>
	/// <para>
	/// This example explains the format:
	/// <pre>
	///  # line comment
	///  key = value
	///  month = January
	/// </pre>
	/// </para>
	/// <para>
	/// The aim of this class is to parse the basic format.
	/// Interpolation of variables is not supported.
	/// </para>
	/// </summary>
	public sealed class PropertiesFile
	{

	  /// <summary>
	  /// The key-value pairs.
	  /// </summary>
	  private readonly PropertySet keyValueMap;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the specified source as a properties file.
	  /// <para>
	  /// This parses the specified character source expecting a properties file format.
	  /// The resulting instance can be queried for each key and value.
	  /// </para>
	  /// <para>
	  /// Properties files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="source">  the properties file resource </param>
	  /// <returns> the properties file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static PropertiesFile of(CharSource source)
	  {
		ArgChecker.notNull(source, "source");
		ImmutableList<string> lines = Unchecked.wrap(() => source.readLines());
		PropertySet keyValues = parse(lines);
		return new PropertiesFile(keyValues);
	  }

	  // parses the properties file format
	  private static PropertySet parse(ImmutableList<string> lines)
	  {
		// cannot use ArrayListMultiMap as it does not retain the order of the keys
		// whereas ImmutableListMultimap does retain the order of the keys
		ImmutableListMultimap.Builder<string, string> parsed = ImmutableListMultimap.builder();
		int lineNum = 0;
		foreach (string line in lines)
		{
		  lineNum++;
		  line = line.Trim();
		  if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal) || line.StartsWith(";", StringComparison.Ordinal))
		  {
			continue;
		  }
		  int equalsPosition = line.IndexOf(" = ", StringComparison.Ordinal);
		  equalsPosition = equalsPosition < 0 ? line.IndexOf('=') : equalsPosition + 1;
		  string key = (equalsPosition < 0 ? line.Trim() : line.Substring(0, equalsPosition).Trim());
		  string value = (equalsPosition < 0 ? "" : line.Substring(equalsPosition + 1).Trim());
		  if (key.Length == 0)
		  {
			throw new System.ArgumentException("Invalid properties file, empty key, line " + lineNum);
		  }
		  parsed.put(key, value);
		}
		return PropertySet.of(parsed.build());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a key-value property set.
	  /// </summary>
	  /// <param name="keyValueMap">  the key-value property set </param>
	  /// <returns> the properties file </returns>
	  public static PropertiesFile of(PropertySet keyValueMap)
	  {
		ArgChecker.notNull(keyValueMap, "keyValueMap");
		return new PropertiesFile(keyValueMap);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  /// <param name="keyValueMap">  the values </param>
	  private PropertiesFile(PropertySet keyValueMap)
	  {
		this.keyValueMap = keyValueMap;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets all the key-value properties of this file.
	  /// <para>
	  /// The map of key-value properties is exposed by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the key-value properties </returns>
	  public PropertySet Properties
	  {
		  get
		  {
			return keyValueMap;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this file equals another.
	  /// <para>
	  /// The comparison checks the content.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other section, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is PropertiesFile)
		{
		  return keyValueMap.Equals(((PropertiesFile) obj).keyValueMap);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the file.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return keyValueMap.GetHashCode();
	  }

	  /// <summary>
	  /// Returns a string describing the file.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		return keyValueMap.ToString();
	  }

	}

}