/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report
{
	using IniFile = com.opengamma.strata.collect.io.IniFile;

	/// <summary>
	/// Marker interface for report templates.
	/// </summary>
	public interface ReportTemplate
	{

	  /// <summary>
	  /// Loads a report template from an ini file.
	  /// </summary>
	  /// <param name="iniFile">  the ini file containing the definition of a report template </param>
	  /// <returns> the template defined in the ini file </returns>
	  /// <exception cref="RuntimeException"> if the ini file cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ReportTemplate load(com.opengamma.strata.collect.io.IniFile iniFile)
	//  {
	//	return MasterReportTemplateIniLoader.load(iniFile);
	//  }

	}

}