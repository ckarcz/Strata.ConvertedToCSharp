using System;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{

	using RenameHandler = org.joda.convert.RenameHandler;

	using Throwables = com.google.common.@base.Throwables;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using IniFile = com.opengamma.strata.collect.io.IniFile;
	using PropertySet = com.opengamma.strata.collect.io.PropertySet;
	using ResourceConfig = com.opengamma.strata.collect.io.ResourceConfig;

	/// <summary>
	/// Combines multiple extended enums into one lookup.
	/// <para>
	/// Each <seealso cref="ExtendedEnum"/> is kept separate to ensure fast lookup of the common case.
	/// This class uses a configuration file to determine the extended enums to combine.
	/// </para>
	/// <para>
	/// It is intended that this class is used as a helper class to load the configuration
	/// and manage the map of names to instances. It should be created and used by the author
	/// of the main abstract extended enum class, and not be application developers.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the enum </param>
	public sealed class CombinedExtendedEnum<T> where T : Named
	{

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(CombinedExtendedEnum).FullName);
	  /// <summary>
	  /// Section name used for types.
	  /// </summary>
	  private const string TYPES_SECTION = "types";

	  /// <summary>
	  /// The combined enum type.
	  /// </summary>
	  private readonly Type<T> type;
	  /// <summary>
	  /// The underlying extended enums.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final com.google.common.collect.ImmutableList<ExtendedEnum<? extends T>> children;
	  private readonly ImmutableList<ExtendedEnum<T>> children;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a combined extended enum instance.
	  /// <para>
	  /// Calling this method loads configuration files to determine which extended enums to combine.
	  /// The configuration file has the same simple name as the specified type and is a
	  /// <seealso cref="IniFile INI file"/> with the suffix '.ini'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the enum </param>
	  /// <param name="type">  the type to load </param>
	  /// <returns> the extended enum </returns>
	  public static CombinedExtendedEnum<R> of<R>(Type<R> type) where R : Named
	  {
		try
		{
		  // load all matching files
		  string name = type.Name + ".ini";
		  IniFile config = ResourceConfig.combinedIniFile(name);
		  // parse files
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableList<ExtendedEnum<? extends R>> children = parseChildren(config, type);
		  ImmutableList<ExtendedEnum<R>> children = parseChildren(config, type);
		  log.fine(() => "Loaded combined extended enum: " + name + ", providers: " + children);
		  return new CombinedExtendedEnum<R>(type, children);

		}
		catch (Exception ex)
		{
		  // logging used because this is loaded in a static variable
		  log.severe("Failed to load CombinedExtendedEnum for " + type + ": " + Throwables.getStackTraceAsString(ex));
		  // return an empty instance to avoid ExceptionInInitializerError
		  return new CombinedExtendedEnum<R>(type, ImmutableList.of());
		}
	  }

	  // parses the alternate names
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private static <R extends Named> com.google.common.collect.ImmutableList<ExtendedEnum<? extends R>> parseChildren(com.opengamma.strata.collect.io.IniFile config, Class<R> enumType)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private static ImmutableList<ExtendedEnum<R>> parseChildren<R>(IniFile config, Type<R> enumType) where R : Named
	  {

		if (!config.contains(TYPES_SECTION))
		{
		  return ImmutableList.of();
		}
		PropertySet section = config.section(TYPES_SECTION);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableList.Builder<ExtendedEnum<? extends R>> builder = com.google.common.collect.ImmutableList.builder();
		ImmutableList.Builder<ExtendedEnum<R>> builder = ImmutableList.builder();
		foreach (string key in section.keys())
		{
		  Type cls;
		  try
		  {
			cls = RenameHandler.INSTANCE.lookupType(key);
		  }
		  catch (Exception ex)
		  {
			throw new System.ArgumentException("Unable to find extended enum class: " + key, ex);
		  }
		  System.Reflection.MethodInfo method;
		  try
		  {
			method = cls.GetMethod("extendedEnum");
		  }
		  catch (Exception ex)
		  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			throw new System.ArgumentException("Unable to find extendedEnum() method on class: " + cls.FullName, ex);
		  }
		  if (!method.ReturnType.Equals(typeof(ExtendedEnum)))
		  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			throw new System.ArgumentException("Method extendedEnum() does not return ExtendedEnum on class: " + cls.FullName);
		  }
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ExtendedEnum<?> result;
		  ExtendedEnum<object> result;
		  try
		  {
			result = typeof(ExtendedEnum).cast(method.invoke(null));
		  }
		  catch (Exception ex)
		  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			throw new System.ArgumentException("Unable to call extendedEnum() method on class: " + cls.FullName, ex);
		  }
		  if (!enumType.IsAssignableFrom(result.Type))
		  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			throw new System.ArgumentException("Method extendedEnum() returned an ExtendedEnum with an incompatible type on class: " + cls.FullName);
		  }
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: builder.add((ExtendedEnum<? extends R>) result);
		  builder.add((ExtendedEnum<R>) result);
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="type">  the enum type </param>
	  /// <param name="children">  the child extended enums to delegate to </param>
	  private CombinedExtendedEnum<T1>(Type<T> type, ImmutableList<T1> children) where T1 : T
	  {

		this.type = ArgChecker.notNull(type, "type");
		this.children = ArgChecker.notNull(children, "children");
	  }

	  //-------------------------------------------------------------------------
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
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (ExtendedEnum<? extends T> child : children)
		foreach (ExtendedEnum<T> child in children)
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") java.util.Optional<T> found = (java.util.Optional<T>) child.find(name);
		  Optional<T> found = (Optional<T>) child.find(name);
		  if (found.Present)
		  {
			return found;
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
		return find(name).orElseThrow(() => new System.ArgumentException(type.Name + " name not found: " + name));
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return "CombinedExtendedEnum[" + type.Name + "]";
	  }

	}

}