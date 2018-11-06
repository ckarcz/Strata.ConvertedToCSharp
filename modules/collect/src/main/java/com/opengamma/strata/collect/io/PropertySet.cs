using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using MoreObjects = com.google.common.@base.MoreObjects;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Multimap = com.google.common.collect.Multimap;

	/// <summary>
	/// A map of key-value properties.
	/// <para>
	/// This class represents a map of key to value.
	/// Multiple values may be associated with each key.
	/// </para>
	/// <para>
	/// This class is generally created by reading an INI or properties file.
	/// See <seealso cref="IniFile"/> and <seealso cref="PropertiesFile"/>.
	/// </para>
	/// </summary>
	public sealed class PropertySet
	{
	  // this class is common between IniFile and PropertiesFile

	  /// <summary>
	  /// The empty instance.
	  /// </summary>
	  private static readonly PropertySet EMPTY = new PropertySet(ImmutableListMultimap.of());

	  /// <summary>
	  /// The key-value pairs.
	  /// </summary>
	  private readonly ImmutableListMultimap<string, string> keyValueMap;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an empty property set.
	  /// <para>
	  /// The result contains no properties.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an empty property set </returns>
	  public static PropertySet empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an instance from a map.
	  /// <para>
	  /// The returned instance will have one value for each key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="keyValues">  the key-values to create the instance with </param>
	  /// <returns> the property set </returns>
	  public static PropertySet of(IDictionary<string, string> keyValues)
	  {
		ArgChecker.notNull(keyValues, "keyValues");
		ImmutableListMultimap.Builder<string, string> builder = ImmutableListMultimap.builder();
		foreach (KeyValuePair<string, string> entry in keyValues.SetOfKeyValuePairs())
		{
		  builder.put(entry);
		}
		return new PropertySet(builder.build());
	  }

	  /// <summary>
	  /// Obtains an instance from a map allowing for multiple values for each key.
	  /// <para>
	  /// The returned instance may have more than one value for each key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="keyValues">  the key-values to create the instance with </param>
	  /// <returns> the property set </returns>
	  public static PropertySet of(Multimap<string, string> keyValues)
	  {
		ArgChecker.notNull(keyValues, "keyValues");
		return new PropertySet(ImmutableListMultimap.copyOf(keyValues));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  /// <param name="keyValues">  the key-value pairs </param>
	  private PropertySet(ImmutableListMultimap<string, string> keyValues)
	  {
		this.keyValueMap = keyValues;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the set of keys of this property set.
	  /// <para>
	  /// The iteration order of the map matches that of the input data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of keys </returns>
	  public ImmutableSet<string> keys()
	  {
		return ImmutableSet.copyOf(keyValueMap.Keys);
	  }

	  /// <summary>
	  /// Returns the property set as a multimap.
	  /// <para>
	  /// The iteration order of the map matches that of the input data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the key-value map </returns>
	  public ImmutableListMultimap<string, string> asMultimap()
	  {
		return keyValueMap;
	  }

	  /// <summary>
	  /// Returns the property set as a map, throwing an exception if any key has multiple values.
	  /// <para>
	  /// The iteration order of the map matches that of the input data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the key-value map </returns>
	  public ImmutableMap<string, string> asMap()
	  {
		ImmutableMap.Builder<string, string> builder = ImmutableMap.builder();
		foreach (string key in keys())
		{
		  builder.put(key, value(key));
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this property set is empty.
	  /// </summary>
	  /// <returns> true if the set is empty </returns>
	  public bool Empty
	  {
		  get
		  {
			return keyValueMap.Empty;
		  }
	  }

	  /// <summary>
	  /// Checks if this property set contains the specified key.
	  /// </summary>
	  /// <param name="key">  the key name </param>
	  /// <returns> true if the key exists </returns>
	  public bool contains(string key)
	  {
		ArgChecker.notNull(key, "key");
		return keyValueMap.containsKey(key);
	  }

	  /// <summary>
	  /// Gets a single value from this property set.
	  /// <para>
	  /// This returns the value associated with the specified key.
	  /// If more than one value, or no value, is associated with the key an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="key">  the key name </param>
	  /// <returns> the value </returns>
	  /// <exception cref="IllegalArgumentException"> if the key does not exist, or if more than one value is associated </exception>
	  public string value(string key)
	  {
		ArgChecker.notNull(key, "key");
		ImmutableList<string> values = keyValueMap.get(key);
		if (values.size() == 0)
		{
		  throw new System.ArgumentException("Unknown key: " + key);
		}
		if (values.size() > 1)
		{
		  throw new System.ArgumentException("Multiple values for key: " + key);
		}
		return values.get(0);
	  }

	  /// <summary>
	  /// Gets the list of values associated with the specified key.
	  /// <para>
	  /// A key-values instance may contain multiple values for each key.
	  /// This method returns that list of values.
	  /// The iteration order of the map matches that of the input data.
	  /// The returned list may be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="key">  the key name </param>
	  /// <returns> the list of values associated with the key </returns>
	  public ImmutableList<string> valueList(string key)
	  {
		ArgChecker.notNull(key, "key");
		return MoreObjects.firstNonNull(keyValueMap.get(key), ImmutableList.of<string>());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this property set with another.
	  /// <para>
	  /// This property set takes precedence.
	  /// Any order of any additional keys will be retained, with those keys located after the base set of keys.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other property set </param>
	  /// <returns> the combined property set </returns>
	  public PropertySet combinedWith(PropertySet other)
	  {
		ArgChecker.notNull(other, "other");
		if (other.Empty)
		{
		  return this;
		}
		if (Empty)
		{
		  return other;
		}
		// cannot use ArrayListMultiMap as it does not retain the order of the keys
		// whereas ImmutableListMultimap does retain the order of the keys
		ImmutableListMultimap.Builder<string, string> map = ImmutableListMultimap.builder();
		map.putAll(this.keyValueMap);
		foreach (string key in other.keyValueMap.Keys)
		{
		  if (!this.contains(key))
		  {
			map.putAll(key, other.valueList(key));
		  }
		}
		return new PropertySet(map.build());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Overrides this property set with another.
	  /// <para>
	  /// The specified property set takes precedence.
	  /// The order of any existing keys will be retained, with the value replaced.
	  /// Any order of any additional keys will be retained, with those keys located after the base set of keys.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other property set </param>
	  /// <returns> the combined property set </returns>
	  public PropertySet overrideWith(PropertySet other)
	  {
		ArgChecker.notNull(other, "other");
		if (other.Empty)
		{
		  return this;
		}
		if (Empty)
		{
		  return other;
		}
		// cannot use ArrayListMultiMap as it does not retain the order of the keys
		// whereas ImmutableListMultimap does retain the order of the keys
		ImmutableListMultimap.Builder<string, string> map = ImmutableListMultimap.builder();
		foreach (string key in this.keyValueMap.Keys)
		{
		  if (other.contains(key))
		  {
			map.putAll(key, other.valueList(key));
		  }
		  else
		  {
			map.putAll(key, this.valueList(key));
		  }
		}
		foreach (string key in other.keyValueMap.Keys)
		{
		  if (!this.contains(key))
		  {
			map.putAll(key, other.valueList(key));
		  }
		}
		return new PropertySet(map.build());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this property set equals another.
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
		if (obj is PropertySet)
		{
		  return keyValueMap.Equals(((PropertySet) obj).keyValueMap);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the property set.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return keyValueMap.GetHashCode();
	  }

	  /// <summary>
	  /// Returns a string describing the property set.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		return keyValueMap.ToString();
	  }

	}

}