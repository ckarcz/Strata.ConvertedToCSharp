using System;
using System.Collections.Generic;
using System.Reflection;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{

	using RenameHandler = org.joda.convert.RenameHandler;

	using Throwables = com.google.common.@base.Throwables;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using IniFile = com.opengamma.strata.collect.io.IniFile;
	using PropertySet = com.opengamma.strata.collect.io.PropertySet;
	using ResourceConfig = com.opengamma.strata.collect.io.ResourceConfig;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Manager for extended enums controlled by code or configuration.
	/// <para>
	/// The standard Java {@code Enum} is a fixed set of constants defined at compile time.
	/// In many scenarios this can be too limiting and this class provides an alternative.
	/// </para>
	/// <para>
	/// An INI configuration file is used to define the set of named instances.
	/// For more information on the process of loading the configuration file, see <seealso cref="ResourceConfig"/>.
	/// </para>
	/// <para>
	/// The named instances are loaded via provider classes.
	/// A provider class is either an implementation of <seealso cref="NamedLookup"/> or a class
	/// providing {@code public static final} enum constants.
	/// </para>
	/// <para>
	/// The configuration file also supports the notion of alternate names (aliases).
	/// This allows many different names to be used to lookup the same instance.
	/// </para>
	/// <para>
	/// Three sections control the loading of additional information.
	/// </para>
	/// <para>
	/// The 'providers' section contains a number of properties, one for each provider.
	/// The key is the full class name of the provider.
	/// The value is 'constants', 'lookup' or 'instance', and is used to obtain a <seealso cref="NamedLookup"/> instance.
	/// A 'constants' provider must contain public static constants of the correct type,
	/// which will be reflectively located and wrapped in a {@code NamedLookup}.
	/// A 'lookup' provider must implement <seealso cref="NamedLookup"/> and have a no-args constructor.
	/// An 'instance' provider must have a static variable named "INSTANCE" of type <seealso cref="NamedLookup"/>.
	/// </para>
	/// <para>
	/// The 'alternates' section contains a number of properties, one for each alternate name.
	/// The key is the alternate name, the value is the standard name.
	/// Alternate names are used when looking up an extended enum.
	/// </para>
	/// <para>
	/// The 'externals' sections contains a number of properties intended to allow external enum names to be mapped.
	/// Unlike 'alternates', which are always included, 'externals' are only included when requested.
	/// There may be multiple external <i>groups</i> to handle different external providers of data.
	/// For example, the mapping used by FpML may differ from that used by Bloomberg.
	/// </para>
	/// <para>
	/// Each 'externals' section has a name of the form 'externals.Foo', where 'Foo' is the name of the group.
	/// Each property line in the section is of the same format as the 'alternates' section.
	/// It maps the external name to the standard name.
	/// </para>
	/// <para>
	/// It is intended that this class is used as a helper class to load the configuration
	/// and manage the map of names to instances. It should be created and used by the author
	/// of the main abstract extended enum class, and not be application developers.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the enum </param>
	public sealed class ExtendedEnum<T> where T : Named
	{

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(ExtendedEnum).FullName);
	  /// <summary>
	  /// Section name used for providers.
	  /// </summary>
	  private const string PROVIDERS_SECTION = "providers";
	  /// <summary>
	  /// Section name used for alternates.
	  /// </summary>
	  private const string ALTERNATES_SECTION = "alternates";
	  /// <summary>
	  /// Section name used for externals.
	  /// </summary>
	  private const string EXTERNALS_SECTION = "externals.";
	  /// <summary>
	  /// Section name used for lenient patterns.
	  /// </summary>
	  private const string LENIENT_PATTERNS_SECTION = "lenientPatterns";

	  /// <summary>
	  /// The enum type.
	  /// </summary>
	  private readonly Type<T> type;
	  /// <summary>
	  /// The lookup functions defining the standard names.
	  /// </summary>
	  private readonly ImmutableList<NamedLookup<T>> lookups;
	  /// <summary>
	  /// The map of alternate names.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly ImmutableMap<string, string> alternateNames_Renamed;
	  /// <summary>
	  /// The map of external names, keyed by the group name.
	  /// The first map holds groups of external names.
	  /// The inner map holds the mapping from external name to our name.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly ImmutableMap<string, ImmutableMap<string, string>> externalNames_Renamed;
	  /// <summary>
	  /// The list of regex patterns for lenient lookup.
	  /// </summary>
	  private readonly ImmutableList<Pair<Pattern, string>> lenientRegex;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an extended enum instance.
	  /// <para>
	  /// Calling this method loads configuration files to determine the extended enum values.
	  /// The configuration file has the same simple name as the specified type and is a
	  /// <seealso cref="IniFile INI file"/> with the suffix '.ini'.
	  /// See class-level documentation for more information.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the enum </param>
	  /// <param name="type">  the type to load </param>
	  /// <returns> the extended enum </returns>
	  public static ExtendedEnum<R> of<R>(Type<R> type) where R : Named
	  {
		try
		{
		  // load all matching files
		  string name = type.Name + ".ini";
		  IniFile config = ResourceConfig.combinedIniFile(name);
		  // parse files
		  ImmutableList<NamedLookup<R>> lookups = parseProviders(config, type);
		  ImmutableMap<string, string> alternateNames = parseAlternates(config);
		  ImmutableMap<string, ImmutableMap<string, string>> externalNames = parseExternals(config);
		  ImmutableList<Pair<Pattern, string>> lenientRegex = parseLenientPatterns(config);
		  log.fine(() => "Loaded extended enum: " + name + ", providers: " + lookups);
		  return new ExtendedEnum<R>(type, lookups, alternateNames, externalNames, lenientRegex);

		}
		catch (Exception ex)
		{
		  // logging used because this is loaded in a static variable
		  log.severe("Failed to load ExtendedEnum for " + type + ": " + Throwables.getStackTraceAsString(ex));
		  // return an empty instance to avoid ExceptionInInitializerError
		  return new ExtendedEnum<R>(type, ImmutableList.of(), ImmutableMap.of(), ImmutableMap.of(), ImmutableList.of());
		}
	  }

	  // parses the alternate names
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private static <R extends Named> com.google.common.collect.ImmutableList<NamedLookup<R>> parseProviders(com.opengamma.strata.collect.io.IniFile config, Class<R> enumType)
	  private static ImmutableList<NamedLookup<R>> parseProviders<R>(IniFile config, Type<R> enumType) where R : Named
	  {

		if (!config.contains(PROVIDERS_SECTION))
		{
		  return ImmutableList.of();
		}
		PropertySet section = config.section(PROVIDERS_SECTION);
		ImmutableList.Builder<NamedLookup<R>> builder = ImmutableList.builder();
		foreach (string key in section.keys())
		{
		  Type cls;
		  try
		  {
			cls = RenameHandler.INSTANCE.lookupType(key);
		  }
		  catch (Exception ex)
		  {
			throw new System.ArgumentException("Unable to find enum provider class: " + key, ex);
		  }
		  string value = section.value(key);
		  if (value.Equals("constants"))
		  {
			// extract public static final constants
			builder.add(parseConstants(enumType, cls));

		  }
		  else if (value.Equals("lookup"))
		  {
			// class is a named lookup
			if (!cls.IsAssignableFrom(typeof(NamedLookup)))
			{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			  throw new System.ArgumentException("Enum provider class must implement NamedLookup " + cls.FullName);
			}
			try
			{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: Constructor<?> cons = cls.getDeclaredConstructor();
			  System.Reflection.ConstructorInfo<object> cons = cls.DeclaredConstructor;
			  if (!Modifier.isPublic(cls.Modifiers))
			  {
				cons.Accessible = true;
			  }
			  builder.add((NamedLookup<R>) cons.newInstance());
			}
			catch (Exception ex)
			{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			  throw new System.ArgumentException("Invalid enum provider constructor: new " + cls.FullName + "()", ex);
			}

		  }
		  else if (value.Equals("instance"))
		  {
			// class has a named lookup INSTANCE static field
			try
			{
			  System.Reflection.FieldInfo field = cls.getDeclaredField("INSTANCE");
			  if (!Modifier.isStatic(field.Modifiers) || !field.Type.IsAssignableFrom(typeof(NamedLookup)))
			  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
				throw new System.ArgumentException("Invalid enum provider instance: " + cls.FullName + ".INSTANCE");
			  }
			  if (!Modifier.isPublic(cls.Modifiers) || !Modifier.isPublic(field.Modifiers))
			  {
				field.Accessible = true;
			  }
			  builder.add((NamedLookup<R>) field.get(null));
			}
			catch (Exception ex)
			{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			  throw new System.ArgumentException("Invalid enum provider instance: " + cls.FullName + ".INSTANCE", ex);
			}

		  }
		  else
		  {
			throw new System.ArgumentException("Provider value must be either 'constants' or 'lookup'");
		  }
		}
		return builder.build();
	  }

	  // parses the public static final constants
	  private static NamedLookup<R> parseConstants<R>(Type<R> enumType, Type constantsType) where R : Named
	  {
		System.Reflection.FieldInfo[] fields = constantsType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
		IDictionary<string, R> instances = new Dictionary<string, R>();
		foreach (System.Reflection.FieldInfo field in fields)
		{
		  if (Modifier.isPublic(field.Modifiers) && Modifier.isStatic(field.Modifiers) && Modifier.isFinal(field.Modifiers) && enumType.IsAssignableFrom(field.Type))
		  {
			if (Modifier.isPublic(constantsType.Modifiers) == false)
			{
			  field.Accessible = true;
			}
			try
			{
			  R instance = enumType.cast(field.get(null));
			  if (!instances.ContainsKey(instance.Name)) instances.Add(instance.Name, instance);
			  if (!instances.ContainsKey(instance.Name.ToUpper(Locale.ENGLISH))) instances.Add(instance.Name.ToUpper(Locale.ENGLISH), instance);
			}
			catch (Exception ex)
			{
			  throw new System.ArgumentException("Unable to query field: " + field, ex);
			}
		  }
		}
		ImmutableMap<string, R> constants = ImmutableMap.copyOf(instances);
		return new NamedLookupAnonymousInnerClass(constants);
	  }

	  private class NamedLookupAnonymousInnerClass : NamedLookup<R>
	  {
		  private ImmutableMap<string, R> constants;

		  public NamedLookupAnonymousInnerClass(ImmutableMap<string, R> constants)
		  {
			  this.constants = constants;
		  }

		  public ImmutableMap<string, R> lookupAll()
		  {
			return constants;
		  }
	  }

	  // parses the alternate names
	  private static ImmutableMap<string, string> parseAlternates(IniFile config)
	  {
		if (!config.contains(ALTERNATES_SECTION))
		{
		  return ImmutableMap.of();
		}
		IDictionary<string, string> alternates = new Dictionary<string, string>();
		foreach (KeyValuePair<string, string> entry in config.section(ALTERNATES_SECTION).asMap().entrySet())
		{
		  alternates[entry.Key] = entry.Value;
		  if (!alternates.ContainsKey(entry.Key.ToUpper(Locale.ENGLISH))) alternates.Add(entry.Key.ToUpper(Locale.ENGLISH), entry.Value);
		}
		return ImmutableMap.copyOf(alternates);
	  }

	  // parses the external names
	  private static ImmutableMap<string, ImmutableMap<string, string>> parseExternals(IniFile config)
	  {
		ImmutableMap.Builder<string, ImmutableMap<string, string>> builder = ImmutableMap.builder();
		foreach (string sectionName in config.sections())
		{
		  if (sectionName.StartsWith(EXTERNALS_SECTION, StringComparison.Ordinal))
		  {
			string group = sectionName.Substring(EXTERNALS_SECTION.Length);
			builder.put(group, config.section(sectionName).asMap());
		  }
		}
		return builder.build();
	  }

	  // parses the lenient patterns
	  private static ImmutableList<Pair<Pattern, string>> parseLenientPatterns(IniFile config)
	  {
		if (!config.contains(LENIENT_PATTERNS_SECTION))
		{
		  return ImmutableList.of();
		}
		IList<Pair<Pattern, string>> alternates = new List<Pair<Pattern, string>>();
		foreach (KeyValuePair<string, string> entry in config.section(LENIENT_PATTERNS_SECTION).asMap().entrySet())
		{
		  alternates.Add(Pair.of(Pattern.compile(entry.Key, Pattern.CASE_INSENSITIVE), entry.Value));
		}
		return ImmutableList.copyOf(alternates);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="type">  the enum type </param>
	  /// <param name="lookups">  the lookup functions to find instances </param>
	  /// <param name="alternateNames">  the map of alternate name to standard name </param>
	  /// <param name="externalNames">  the map of external name groups </param>
	  private ExtendedEnum(Type<T> type, ImmutableList<NamedLookup<T>> lookups, ImmutableMap<string, string> alternateNames, ImmutableMap<string, ImmutableMap<string, string>> externalNames, ImmutableList<Pair<Pattern, string>> lenientRegex)
	  {

		this.type = ArgChecker.notNull(type, "type");
		this.lookups = ArgChecker.notNull(lookups, "lookups");
		this.alternateNames_Renamed = ArgChecker.notNull(alternateNames, "alternateNames");
		this.externalNames_Renamed = ArgChecker.notNull(externalNames, "externalNames");
		this.lenientRegex = ArgChecker.notNull(lenientRegex, "lenientRegex");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the enum type.
	  /// </summary>
	  /// <returns> the enum type </returns>
	  public Type<T> Type
	  {
		  get
		  {
			return type;
		  }
	  }

	  /// <summary>
	  /// Finds an instance by name.
	  /// <para>
	  /// This finds the instance matching the specified name.
	  /// Instances may have alternate names (aliases), thus the returned instance
	  /// may have a name other than that requested.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the enum name to return </param>
	  /// <returns> the named enum </returns>
	  public Optional<T> find(string name)
	  {
		string standardName = alternateNames_Renamed.getOrDefault(name, name);
		foreach (NamedLookup<T> lookup in lookups)
		{
		  T instance = lookup.lookup(standardName);
		  if (instance != null)
		  {
			return instance;
		  }
		}
		return null;
	  }

	  /// <summary>
	  /// Looks up an instance by name.
	  /// <para>
	  /// This finds the instance matching the specified name.
	  /// Instances may have alternate names (aliases), thus the returned instance
	  /// may have a name other than that requested.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the enum name to return </param>
	  /// <returns> the named enum </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not found </exception>
	  public T lookup(string name)
	  {
		string standardName = alternateNames_Renamed.getOrDefault(name, name);
		foreach (NamedLookup<T> lookup in lookups)
		{
		  T instance = lookup.lookup(standardName);
		  if (instance != null)
		  {
			return instance;
		  }
		}
		throw new System.ArgumentException(type.Name + " name not found: " + name);
	  }

	  /// <summary>
	  /// Looks up an instance by name and type.
	  /// <para>
	  /// This finds the instance matching the specified name, ensuring it is of the specified type.
	  /// Instances may have alternate names (aliases), thus the returned instance
	  /// may have a name other than that requested.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <S>  the enum subtype </param>
	  /// <param name="subtype">  the enum subtype to match </param>
	  /// <param name="name">  the enum name to return </param>
	  /// <returns> the named enum </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not found or has the wrong type </exception>
	  public S lookup<S>(string name, Type<S> subtype) where S : T
	  {
		T result = lookup(name);
		if (!subtype.IsInstanceOfType(result))
		{
		  throw new System.ArgumentException(type.Name + " name found but did not match expected type: " + name);
		}
		return subtype.cast(result);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the map of known instances by name.
	  /// <para>
	  /// This method returns all known instances.
	  /// It is permitted for an enum provider implementation to return an empty map,
	  /// thus the map may not be complete.
	  /// The map may include instances keyed under an alternate name, such as names
	  /// in upper case, however it will not include the base set of
	  /// <seealso cref="#alternateNames() alternate names"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the map of enum instance by name </returns>
	  public ImmutableMap<string, T> lookupAll()
	  {
		IDictionary<string, T> map = new Dictionary<string, T>();
		foreach (NamedLookup<T> lookup in lookups)
		{
		  IDictionary<string, T> lookupMap = lookup.lookupAll();
		  foreach (KeyValuePair<string, T> entry in lookupMap.SetOfKeyValuePairs())
		  {
			if (!map.ContainsKey(entry.Key)) map.Add(entry.Key, entry.Value);
		  }
		}
		return ImmutableMap.copyOf(map);
	  }

	  /// <summary>
	  /// Returns the map of known instances by normalized name.
	  /// <para>
	  /// This method returns all known instances, keyed by the normalized name.
	  /// This is equivalent to the result of <seealso cref="#lookupAll()"/> adjusted such
	  /// that each entry is keyed by the result of <seealso cref="Named#getName()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the map of enum instance by name </returns>
	  public ImmutableMap<string, T> lookupAllNormalized()
	  {
		// add values that are keyed under the normalized name
		// keep values keyed under a non-normalized name
		IDictionary<string, T> result = new Dictionary<string, T>();
		IDictionary<string, T> others = new Dictionary<string, T>();
		foreach (KeyValuePair<string, T> entry in lookupAll().entrySet())
		{
		  string normalizedName = entry.Value.Name;
		  if (entry.Key.Equals(normalizedName))
		  {
			result[normalizedName] = entry.Value;
		  }
		  else
		  {
			others[normalizedName] = entry.Value;
		  }
		}
		// include any values that are only keyed under a non-normalized name
		others.Values.forEach(v => result.putIfAbsent(v.Name, v));
		return ImmutableMap.copyOf(result);
	  }

	  /// <summary>
	  /// Returns the complete map of alternate name to standard name.
	  /// <para>
	  /// The map is keyed by the alternate name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the map of alternate names </returns>
	  public ImmutableMap<string, string> alternateNames()
	  {
		return alternateNames_Renamed;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the set of groups that have external names defined.
	  /// <para>
	  /// External names are used to map names used by external systems to the standard name used here.
	  /// There can be multiple groups of mappings to external systems,
	  /// For example, the mapping used by FpML may differ from that used by Bloomberg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of groups that have external names </returns>
	  public ImmutableSet<string> externalNameGroups()
	  {
		return externalNames_Renamed.Keys;
	  }

	  /// <summary>
	  /// Returns the mapping of external names to standard names for a group.
	  /// <para>
	  /// External names are used to map names used by external systems to the standard name used here.
	  /// There can be multiple groups of mappings to external systems,
	  /// For example, the mapping used by FpML may differ from that used by Bloomberg.
	  /// </para>
	  /// <para>
	  /// The result provides mapping between the external name and the standard name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="group">  the group name to find external names for </param>
	  /// <returns> the map of external names for the group </returns>
	  /// <exception cref="IllegalArgumentException"> if the group is not found </exception>
	  public ExternalEnumNames<T> externalNames(string group)
	  {
		ImmutableMap<string, string> externals = externalNames_Renamed.get(group);
		if (externals == null)
		{
		  throw new System.ArgumentException(type.Name + " group not found: " + group);
		}
		return new ExternalEnumNames<T>(this, group, externals);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Looks up an instance by name leniently.
	  /// <para>
	  /// This finds the instance matching the specified name using a lenient lookup strategy.
	  /// An extended enum may include additional configuration defining how lenient search occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the enum name to return </param>
	  /// <returns> the named enum </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not found </exception>
	  public Optional<T> findLenient(string name)
	  {
		Optional<T> alreadyValid = find(name);
		if (alreadyValid.Present)
		{
		  return alreadyValid;
		}
		string current = name.ToUpper(Locale.ENGLISH);
		foreach (Pair<Pattern, string> pair in lenientRegex)
		{
		  Matcher matcher = pair.First.matcher(current);
		  if (matcher.matches())
		  {
			current = matcher.replaceFirst(pair.Second);
		  }
		}
		return find(current);
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return "ExtendedEnum[" + type.Name + "]";
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Maps names used by external systems to the standard name used here.
	  /// <para>
	  /// A frequent problem in parsing external file formats is converting enum values.
	  /// This class provides a suitable mapping, allowing multiple external names to map to one standard name.
	  /// </para>
	  /// <para>
	  /// A single instance represents the mapping for a single external group.
	  /// This allows the mapping for different groups to differ.
	  /// For example, the mapping used by FpML may differ from that used by Bloomberg.
	  /// </para>
	  /// <para>
	  /// Instances of this class are configured via INI files and provided via <seealso cref="ExtendedEnum"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the enum </param>
	  public sealed class ExternalEnumNames<T> where T : Named
	  {

		internal ExtendedEnum<T> extendedEnum;
		internal string group;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ImmutableMap<string, string> externalNames_Renamed;

		internal ExternalEnumNames(ExtendedEnum<T> extendedEnum, string group, ImmutableMap<string, string> externalNames)
		{
		  this.extendedEnum = extendedEnum;
		  this.group = group;
		  this.externalNames_Renamed = externalNames;
		}

		/// <summary>
		/// Looks up an instance by name.
		/// <para>
		/// This finds the instance matching the specified name.
		/// Instances may have alternate names (aliases), thus the returned instance
		/// may have a name other than that requested.
		/// 
		/// </para>
		/// </summary>
		/// <param name="name">  the enum name to return </param>
		/// <returns> the named enum </returns>
		/// <exception cref="IllegalArgumentException"> if the name is not found </exception>
		public T lookup(string name)
		{
		  string standardName = externalNames_Renamed.getOrDefault(name, name);
		  try
		  {
			return extendedEnum.lookup(standardName);
		  }
		  catch (System.ArgumentException)
		  {
			throw new System.ArgumentException(Messages.format("{}:{} unable to find external name: {}", extendedEnum.type.Name, group, name));
		  }
		}

		/// <summary>
		/// Looks up an instance by name and type.
		/// <para>
		/// This finds the instance matching the specified name, ensuring it is of the specified type.
		/// Instances may have alternate names (aliases), thus the returned instance
		/// may have a name other than that requested.
		/// 
		/// </para>
		/// </summary>
		/// @param <S>  the enum subtype </param>
		/// <param name="subtype">  the enum subtype to match </param>
		/// <param name="name">  the enum name to return </param>
		/// <returns> the named enum </returns>
		/// <exception cref="IllegalArgumentException"> if the name is not found or has the wrong type </exception>
		public S lookup<S>(string name, Type<S> subtype) where S : T
		{
		  T result = lookup(name);
		  if (!subtype.IsInstanceOfType(result))
		  {
			throw new System.ArgumentException(Messages.format("{}:{} external name found but did not match expected type: {}", extendedEnum.type.Name, group, name));
		  }
		  return subtype.cast(result);
		}

		/// <summary>
		/// Returns the complete map of external name to standard name.
		/// <para>
		/// The map is keyed by the external name.
		/// 
		/// </para>
		/// </summary>
		/// <returns> the map of external names </returns>
		public ImmutableMap<string, string> externalNames()
		{
		  return externalNames_Renamed;
		}

		/// <summary>
		/// Looks up the external name given a standard enum instance.
		/// <para>
		/// This searches the map of external names and returns the first matching entry
		/// that maps to the given standard name.
		/// 
		/// </para>
		/// </summary>
		/// <param name="namedEnum">  the named enum to find an external name for </param>
		/// <returns> the external name </returns>
		/// <exception cref="IllegalArgumentException"> if there is no external name </exception>
		public string reverseLookup(T namedEnum)
		{
		  string name = namedEnum.Name;
		  foreach (KeyValuePair<string, string> entry in externalNames_Renamed.entrySet())
		  {
			if (entry.Value.Equals(name))
			{
			  return entry.Key;
			}
		  }
		  throw new System.ArgumentException(Messages.format("{}:{} external name not found for standard name: {}", extendedEnum.type.Name, group, name));
		}

		//-------------------------------------------------------------------------
		public override string ToString()
		{
		  return "ExternalEnumNames[" + extendedEnum.type.Name + ":" + group + "]";
		}
	  }

	}

}