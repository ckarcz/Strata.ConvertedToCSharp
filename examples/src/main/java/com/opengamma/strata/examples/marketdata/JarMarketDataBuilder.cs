using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.marketdata
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Messages = com.opengamma.strata.collect.Messages;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;

	/// <summary>
	/// Loads market data from the standard directory structure embedded within a JAR file.
	/// </summary>
	public class JarMarketDataBuilder : ExampleMarketDataBuilder
	{

	  /// <summary>
	  /// The JAR file containing the expected structure of resources.
	  /// </summary>
	  private readonly File jarFile;
	  /// <summary>
	  /// The root path to the resources within the JAR file.
	  /// </summary>
	  private readonly string rootPath;
	  /// <summary>
	  /// A cache of JAR entries under the root path.
	  /// </summary>
	  private readonly ImmutableSet<string> entries;

	  /// <summary>
	  /// Constructs an instance.
	  /// </summary>
	  /// <param name="jarFile">  the JAR file containing the expected structure of resources </param>
	  /// <param name="rootPath">  the root path to the resources within the JAR file </param>
	  public JarMarketDataBuilder(File jarFile, string rootPath)
	  {
		// classpath resources are forward-slash separated
		string jarRoot = rootPath.StartsWith("/", StringComparison.Ordinal) ? rootPath.Substring(1) : rootPath;
		if (!jarRoot.EndsWith("/", StringComparison.Ordinal))
		{
		  jarRoot += "/";
		}
		this.jarFile = jarFile;
		this.rootPath = jarRoot;
		this.entries = getEntries(jarFile, rootPath);
	  }

	  //-------------------------------------------------------------------------
	  protected internal override ICollection<ResourceLocator> getAllResources(string subdirectoryName)
	  {
		string resolvedSubdirectory = subdirectoryName + "/";
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return entries.Where(e => e.StartsWith(resolvedSubdirectory) && !e.Equals(resolvedSubdirectory)).Select(e => getEntryLocator(rootPath + e)).collect(Collectors.toSet());
	  }

	  protected internal override ResourceLocator getResource(string subdirectoryName, string resourceName)
	  {
		string fullLocation = string.format(Locale.ENGLISH, "%s%s/%s", rootPath, subdirectoryName, resourceName);
		try
		{
				using (JarFile jar = new JarFile(jarFile))
				{
			  JarEntry entry = jar.getJarEntry(fullLocation);
			  if (entry == null)
			  {
				return null;
			  }
			  return getEntryLocator(entry.Name);
				}
		}
		catch (Exception e)
		{
		  throw new System.ArgumentException(Messages.format("Error loading resource from JAR file: {}", jarFile), e);
		}
	  }

	  protected internal override bool subdirectoryExists(string subdirectoryName)
	  {
		// classpath resources are forward-slash separated
		string resolvedName = subdirectoryName.StartsWith("/", StringComparison.Ordinal) ? subdirectoryName.Substring(1) : subdirectoryName;
		if (!resolvedName.EndsWith("/", StringComparison.Ordinal))
		{
		  resolvedName += "/";
		}
		return entries.contains(resolvedName);
	  }

	  //-------------------------------------------------------------------------
	  // Gets the resource locator corresponding to a given entry
	  private ResourceLocator getEntryLocator(string entryName)
	  {
		return ResourceLocator.ofClasspath(entryName);
	  }

	  private static ImmutableSet<string> getEntries(File jarFile, string rootPath)
	  {
		ImmutableSet.Builder<string> builder = ImmutableSet.builder();
		try
		{
				using (JarFile jar = new JarFile(jarFile))
				{
			  IEnumerator<JarEntry> jarEntries = jar.entries();
			  while (jarEntries.MoveNext())
			  {
				JarEntry entry = jarEntries.Current;
				string entryName = entry.Name;
				if (entryName.StartsWith(rootPath, StringComparison.Ordinal) && !entryName.Equals(rootPath))
				{
				  string relativeEntryPath = entryName.Substring(rootPath.Length + 1);
				  if (relativeEntryPath.Trim().Length > 0)
				  {
					builder.add(relativeEntryPath);
				  }
				}
			  }
				}
		}
		catch (Exception e)
		{
		  throw new System.ArgumentException(Messages.format("Error scanning entries in JAR file: {}", jarFile), e);
		}
		return builder.build();
	  }

	}

}