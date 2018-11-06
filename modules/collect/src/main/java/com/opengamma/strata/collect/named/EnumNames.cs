using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{

	using CaseFormat = com.google.common.@base.CaseFormat;
	using ImmutableSortedMap = com.google.common.collect.ImmutableSortedMap;
	using ImmutableSortedSet = com.google.common.collect.ImmutableSortedSet;

	/// <summary>
	/// Helper that allows enum names to be created and parsed.
	/// </summary>
	/// @param <T>  the type of the enum </param>
	public sealed class EnumNames<T> where T : Enum<T>, NamedEnum
	{

	  /// <summary>
	  /// Parsing map.
	  /// </summary>
	  private readonly ImmutableSortedMap<string, T> parseMap;
	  /// <summary>
	  /// Formatted forms.
	  /// </summary>
	  private readonly ImmutableSortedSet<string> formattedSet;
	  /// <summary>
	  /// Format map (mutable, but treated as immutable).
	  /// </summary>
	  private readonly Dictionary<T, string> formatMap;
	  /// <summary>
	  /// Class of the enum.
	  /// </summary>
	  private readonly Type<T> enumType;

	  /// <summary>
	  /// Creates an instance deriving the formatted string from the enum constant name.
	  /// </summary>
	  /// @param <T>  the type of the enum </param>
	  /// <param name="enumType">  the type of the enum </param>
	  /// <returns> the names instance </returns>
	  public static EnumNames<T> of<T>(Type<T> enumType) where T : Enum<T>, NamedEnum
	  {
		return new EnumNames<T>(enumType, false);
	  }

	  /// <summary>
	  /// Creates an instance where the {@code toString} method is written manually.
	  /// <para>
	  /// The {@code toString} method is called to extract the correct formatted string.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the enum </param>
	  /// <param name="enumType">  the type of the enum </param>
	  /// <returns> the names instance </returns>
	  public static EnumNames<T> ofManualToString<T>(Type<T> enumType) where T : Enum<T>, NamedEnum
	  {
		return new EnumNames<T>(enumType, true);
	  }

	  // restricted constructor
	  private EnumNames(Type<T> enumType, bool manualToString)
	  {
		this.enumType = ArgChecker.notNull(enumType, "enumType");
		SortedDictionary<string, T> map = new SortedDictionary<string, T>();
		SortedSet<string> formattedSet = new SortedSet<string>();
		Dictionary<T, string> formatMap = new Dictionary<T, string>(enumType);
		foreach (T value in enumType.EnumConstants)
		{
		  string formatted = manualToString ? value.ToString() : CaseFormat.UPPER_UNDERSCORE.to(CaseFormat.UPPER_CAMEL, value.name());
		  map[value.name()] = value;
		  map[value.name().ToUpper(Locale.ENGLISH)] = value;
		  map[value.name().ToLower(Locale.ENGLISH)] = value;
		  map[formatted] = value;
		  map[formatted.ToUpper(Locale.ENGLISH)] = value;
		  map[formatted.ToLower(Locale.ENGLISH)] = value;
		  formattedSet.Add(formatted);
		  formatMap[value] = formatted;
		}
		this.parseMap = ImmutableSortedMap.copyOf(map);
		this.formattedSet = ImmutableSortedSet.copyOf(formattedSet);
		this.formatMap = formatMap;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a standard Strata mixed case name from an enum-style constant.
	  /// </summary>
	  /// <param name="value">  the enum value to convert </param>
	  /// <returns> the converted name </returns>
	  public string format(T value)
	  {
		// this should never return null
		return formatMap[value];
	  }

	  /// <summary>
	  /// Parses the standard external name for an enum.
	  /// </summary>
	  /// <param name="name">  the external name </param>
	  /// <returns> the enum value </returns>
	  public T parse(string name)
	  {
		ArgChecker.notNull(name, "name");
		T value = parseMap.get(name);
		if (value == null)
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException(Messages.format("Unknown enum name '{}' for type {}, valid values are {}", name, enumType.FullName, formattedSet));
		}
		return value;
	  }

	}

}