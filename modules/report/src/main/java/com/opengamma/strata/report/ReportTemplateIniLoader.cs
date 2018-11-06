/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report
{
	using IniFile = com.opengamma.strata.collect.io.IniFile;

	/// <summary>
	/// Loads a report template from an ini-based file format.
	/// </summary>
	/// @param <T>  the report template type </param>
	public interface ReportTemplateIniLoader<T> where T : ReportTemplate
	{

	  /// <summary>
	  /// The settings section name. </summary>

	  /// <summary>
	  /// The report type property name, in the settings section. </summary>

	  /// <summary>
	  /// Gets the type of report handled by this loader.
	  /// </summary>
	  /// <returns> the type of report handled by this loader </returns>
	  string ReportType {get;}

	  /// <summary>
	  /// Loads the report template.
	  /// </summary>
	  /// <param name="iniFile">  the ini file to load </param>
	  /// <returns> the loaded report template object </returns>
	  T load(IniFile iniFile);

	}

	public static class ReportTemplateIniLoader_Fields
	{
	  public const string SETTINGS_SECTION = "settings";
	  public const string SETTINGS_REPORT_TYPE = "reportType";
	}

}