using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using Splitter = com.google.common.@base.Splitter;
	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Provides access to configuration files.
	/// <para>
	/// A standard approach to configuration is provided by this class.
	/// Any configuration information provided by this library can be overridden or added to by applications.
	/// </para>
	/// <para>
	/// By default, there are three groups of recognized configuration directories:
	/// <ul>
	/// <li>base
	/// <li>library
	/// <li>application
	/// </ul>
	/// </para>
	/// <para>
	/// Each group consists of ten directories using a numeric suffix:
	/// <ul>
	/// <li>{@code com/opengamma/strata/config/base}
	/// <li>{@code com/opengamma/strata/config/base1}
	/// <li>{@code com/opengamma/strata/config/base2}
	/// <li>...
	/// <li>{@code com/opengamma/strata/config/base9}
	/// <li>{@code com/opengamma/strata/config/library}
	/// <li>{@code com/opengamma/strata/config/library1}
	/// <li>...
	/// <li>{@code com/opengamma/strata/config/library9}
	/// <li>{@code com/opengamma/strata/config/application}
	/// <li>{@code com/opengamma/strata/config/application1}
	/// <li>...
	/// <li>{@code com/opengamma/strata/config/application9}
	/// </ul>
	/// These form a complete set of thirty directories that are searched for configuration.
	/// </para>
	/// <para>
	/// The search strategy looks for the same file name in each of the thirty directories.
	/// All the files that are found are then merged, with directories lower down the list taking priorty.
	/// Thus, any configuration file in the 'application9' directory will override the same file
	/// in the 'appication1' directory, which will override the same file in the 'library' group,
	/// which will further override the same file in the 'base' group.
	/// </para>
	/// <para>
	/// The 'base' group is reserved for Strata.
	/// The 'library' group is reserved for libraries built directly on Strata.
	/// </para>
	/// <para>
	/// The set of configuration directories can be changed using the system property
	/// 'com.opengamma.strata.config.directories'.
	/// This must be a comma separated list, such as 'base,base1,base2,override,application'.
	/// </para>
	/// <para>
	/// In general, the configuration managed by this class will be in INI format.
	/// The <seealso cref="#combinedIniFile(String)"/> method is the main entry point, returning a single
	/// INI file merged from all available configuration files.
	/// </para>
	/// </summary>
	public sealed class ResourceConfig
	{

	  /// <summary>
	  /// The logger.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
	  private static readonly Logger log = Logger.getLogger(typeof(ResourceConfig).FullName);
	  /// <summary>
	  /// The package/folder location for the configuration.
	  /// </summary>
	  private const string CONFIG_PACKAGE = "META-INF/com/opengamma/strata/config/";
	  /// <summary>
	  /// The default set of directories to query configuration files in.
	  /// </summary>
	  private static readonly ImmutableList<string> DEFAULT_DIRS = ImmutableList.of("base", "base1", "base2", "base3", "base4", "base5", "base6", "base7", "base8", "base9", "library", "library1", "library2", "library3", "library4", "library5", "library6", "library7", "library8", "library9", "application", "application1", "application2", "application3", "application4", "application5", "application6", "application7", "application8", "application9");
	  /// <summary>
	  /// The system property defining the comma separated list of groups.
	  /// </summary>
	  public const string RESOURCE_DIRS_PROPERTY = "com.opengamma.strata.config.directories";
	  /// <summary>
	  /// The resource groups.
	  /// Always falls back to the known set in case of error.
	  /// </summary>
	  private static readonly ImmutableList<string> RESOURCE_DIRS;
	  static ResourceConfig()
	  {
		IList<string> dirs = DEFAULT_DIRS;
		string property = null;
		try
		{
		  property = System.getProperty(RESOURCE_DIRS_PROPERTY);
		}
		catch (Exception ex)
		{
		  log.warning("Unable to access system property: " + ex.ToString());
		}
		if (!string.ReferenceEquals(property, null) && property.Length > 0)
		{
		  try
		  {
			dirs = Splitter.on(',').trimResults().splitToList(property);
		  }
		  catch (Exception ex)
		  {
			log.warning("Invalid system property: " + property + ": " + ex.ToString());
		  }
		  foreach (string dir in dirs)
		  {
			if (!dir.matches("[A-Za-z0-9-]+"))
			{
			  log.warning("Invalid system property directory, must match regex [A-Za-z0-9-]+: " + dir);
			}
		  }
		}
		log.config("Using directories: " + dirs);
		RESOURCE_DIRS = ImmutableList.copyOf(dirs);
	  }
	  /// <summary>
	  /// INI section name used for chaining.
	  /// </summary>
	  private const string CHAIN_SECTION = "chain";
	  /// <summary>
	  /// INI property name used for chaining.
	  /// </summary>
	  private const string CHAIN_NEXT = "chainNextFile";
	  /// <summary>
	  /// INI property name used for removing sections.
	  /// </summary>
	  private const string CHAIN_REMOVE = "chainRemoveSections";

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a combined INI file formed by merging INI files with the specified name.
	  /// <para>
	  /// This finds the all files with the specified name in the configuration directories.
	  /// Each file is loaded, with the result being formed by merging the files into one.
	  /// See <seealso cref="#combinedIniFile(List)"/> for more details on the merge process.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resourceName">  the resource name </param>
	  /// <returns> the resource locators </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalStateException"> if there is a configuration error </exception>
	  public static IniFile combinedIniFile(string resourceName)
	  {
		ArgChecker.notNull(resourceName, "resourceName");
		return ResourceConfig.combinedIniFile(ResourceConfig.orderedResources(resourceName));
	  }

	  /// <summary>
	  /// Returns a combined INI file formed by merging the specified INI files.
	  /// <para>
	  /// The result of this method is formed by merging the specified files together.
	  /// The files are combined in order forming a chain.
	  /// The first file in the list has the lowest priority.
	  /// The last file in the list has the highest priority.
	  /// </para>
	  /// <para>
	  /// The algorithm starts with all the sections and properties from the highest priority file.
	  /// It then adds any sections or properties from subsequent files that are not already present.
	  /// </para>
	  /// <para>
	  /// The algorithm can be controlled by providing a '[chain]' section.
	  /// Within the 'chain' section, if 'chainNextFile' is 'false', then processing stops,
	  /// and lower priority files are ignored. If the 'chainRemoveSections' property is specified,
	  /// the listed sections are ignored from the files lower in the chain.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resources">  the INI file resources to read </param>
	  /// <returns> the combined chained INI file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO error occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the configuration is invalid </exception>
	  public static IniFile combinedIniFile(IList<ResourceLocator> resources)
	  {
		ArgChecker.notNull(resources, "resources");
		IDictionary<string, PropertySet> sectionMap = new LinkedHashMap<string, PropertySet>();
		foreach (ResourceLocator resource in resources)
		{
		  IniFile file = IniFile.of(resource.CharSource);
		  if (file.contains(CHAIN_SECTION))
		  {
			PropertySet chainSection = file.section(CHAIN_SECTION);
			// remove everything from lower priority files if not chaining
			if (chainSection.contains(CHAIN_NEXT) && bool.Parse(chainSection.value(CHAIN_NEXT)) == false)
			{
			  sectionMap.Clear();
			}
			else
			{
			  // remove sections from lower priority files
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the java.util.Collection 'removeAll' method:
			  sectionMap.Keys.removeAll(chainSection.valueList(CHAIN_REMOVE));
			}
		  }
		  // add entries, replacing existing data
		  foreach (string sectionName in file.asMap().Keys)
		  {
			if (!sectionName.Equals(CHAIN_SECTION))
			{
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
			  sectionMap.merge(sectionName, file.section(sectionName), PropertySet::overrideWith);
			}
		  }
		}
		return IniFile.of(sectionMap);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an ordered list of resource locators.
	  /// <para>
	  /// This finds the all files with the specified name in the configuration directories.
	  /// The result is ordered from the lowest priority (base) file to the highest priority (application) file.
	  /// The result will always contain at least one file, but it may contain more than one.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resourceName">  the resource name </param>
	  /// <returns> the resource locators </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalStateException"> if there is a configuration error </exception>
	  public static IList<ResourceLocator> orderedResources(string resourceName)
	  {
		ArgChecker.notNull(resourceName, "resourceName");
		return Unchecked.wrap(() => orderedResources0(resourceName));
	  }

	  // find the list of resources
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private static java.util.List<ResourceLocator> orderedResources0(String classpathResourceName) throws java.io.IOException
	  private static IList<ResourceLocator> orderedResources0(string classpathResourceName)
	  {
		ClassLoader classLoader = ResourceLocator.classLoader();
		IList<string> names = new List<string>();
		IList<ResourceLocator> result = new List<ResourceLocator>();
		foreach (string dir in RESOURCE_DIRS)
		{
		  string name = CONFIG_PACKAGE + dir + "/" + classpathResourceName;
		  names.Add(name);
		  IList<URL> urls = Collections.list(classLoader.getResources(name));
		  switch (urls.Count)
		  {
			case 0:
			  continue;
			case 1:
			  result.Add(ResourceLocator.ofClasspathUrl(urls[0]));
			  break;
			default:
			  // handle case where Strata is on the classpath more than once
			  // only accept this if the data being read is the same in all URLs
			  ResourceLocator baseResource = ResourceLocator.ofClasspathUrl(urls[0]);
			  for (int i = 1; i < urls.Count; i++)
			  {
				ResourceLocator otherResource = ResourceLocator.ofClasspathUrl(urls[i]);
				if (!baseResource.ByteSource.contentEquals(otherResource.ByteSource))
				{
				  log.severe("More than one file found on the classpath: " + name + ": " + urls);
				  throw new System.InvalidOperationException("More than one file found on the classpath: " + name + ": " + urls);
				}
			  }
			  result.Add(baseResource);
			  break;
		  }
		}
		if (result.Count == 0)
		{
		  log.severe("No resource files found on the classpath: " + names);
		  throw new System.InvalidOperationException("No files found on the classpath: " + names);
		}
		log.config(() => "Resources found: " + result);
		return result;
	  }

	  //-------------------------------------------------------------------------
	  private ResourceConfig()
	  {
	  }

	}

}