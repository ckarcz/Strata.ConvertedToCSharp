/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.marketdata
{

	using IniFile = com.opengamma.strata.collect.io.IniFile;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using TradeReportTemplate = com.opengamma.strata.report.trade.TradeReportTemplate;

	/// <summary>
	/// Contains utilities for working with data in the examples environment.
	/// </summary>
	public sealed class ExampleData
	{

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private ExampleData()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads a golden copy of expected results from a text file.
	  /// </summary>
	  /// <param name="name">  the name of the results </param>
	  /// <returns> the loaded results </returns>
	  public static string loadExpectedResults(string name)
	  {
		string classpathResourceName = string.format(Locale.ENGLISH, "classpath:goldencopy/%s.txt", name);
		ResourceLocator resourceLocator = ResourceLocator.of(classpathResourceName);
		try
		{
		  return resourceLocator.CharSource.read().Trim();
		}
		catch (IOException ex)
		{
		  throw new UncheckedIOException(name, ex);
		}
	  }

	  /// <summary>
	  /// Loads a trade report template from the standard INI format.
	  /// </summary>
	  /// <param name="templateName">  the name of the template </param>
	  /// <returns> the loaded report template </returns>
	  public static TradeReportTemplate loadTradeReportTemplate(string templateName)
	  {
		string resourceName = string.format(Locale.ENGLISH, "classpath:example-reports/%s.ini", templateName);
		ResourceLocator resourceLocator = ResourceLocator.of(resourceName);
		IniFile ini = IniFile.of(resourceLocator.CharSource);
		return TradeReportTemplate.load(ini);
	  }

	}

}