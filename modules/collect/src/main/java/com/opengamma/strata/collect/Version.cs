/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
	using PropertiesFile = com.opengamma.strata.collect.io.PropertiesFile;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;

	/// <summary>
	/// Provides access to the version of Strata.
	/// </summary>
	public sealed class Version
	{

	  /// <summary>
	  /// The version, which will be populated by the Maven build.
	  /// </summary>
	  private static readonly string VERSION = PropertiesFile.of(ResourceLocator.ofClasspath(typeof(Version), "version.properties").CharSource).Properties.value("version");

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private Version()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the version of Strata.
	  /// </summary>
	  /// <returns> the version </returns>
	  public static string VersionString
	  {
		  get
		  {
			return VERSION;
		  }
	  }

	}

}