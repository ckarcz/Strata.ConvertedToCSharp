using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.marketdata
{

	using Messages = com.opengamma.strata.collect.Messages;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;

	/// <summary>
	/// Loads market data from the standard directory structure on disk.
	/// </summary>
	public class DirectoryMarketDataBuilder : ExampleMarketDataBuilder
	{

	  /// <summary>
	  /// The path to the root of the directory structure.
	  /// </summary>
	  private readonly Path rootPath;

	  /// <summary>
	  /// Constructs an instance.
	  /// </summary>
	  /// <param name="rootPath">  the path to the root of the directory structure </param>
	  public DirectoryMarketDataBuilder(Path rootPath)
	  {
		this.rootPath = rootPath;
	  }

	  //-------------------------------------------------------------------------
	  protected internal override ICollection<ResourceLocator> getAllResources(string subdirectoryName)
	  {
		File dir = rootPath.resolve(subdirectoryName).toFile();
		if (!dir.exists())
		{
		  throw new System.ArgumentException(Messages.format("Directory does not exist: {}", dir));
		}
		return java.util.dir.listFiles().Where(f => !f.Hidden).Select(ResourceLocator.ofFile).ToList();
	  }

	  protected internal override ResourceLocator getResource(string subdirectoryName, string resourceName)
	  {
		File file = rootPath.resolve(subdirectoryName).resolve(resourceName).toFile();
		if (!file.exists())
		{
		  return null;
		}
		return ResourceLocator.ofFile(file);
	  }

	  protected internal override bool subdirectoryExists(string subdirectoryName)
	  {
		File file = rootPath.resolve(subdirectoryName).toFile();
		return file.exists();
	  }

	}

}