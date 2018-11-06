/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.cashflow
{
	using IniFile = com.opengamma.strata.collect.io.IniFile;

	/// <summary>
	/// Marker for a cash flow report template.
	/// <para>
	/// Cash flow reports are currently parameterless so this class contains no fields.
	/// </para>
	/// </summary>
	public class CashFlowReportTemplate : ReportTemplate
	{

	  /// <summary>
	  /// Creates a trade report template by reading a template definition in an ini file.
	  /// </summary>
	  /// <param name="ini">  the ini file containing the definition of the template </param>
	  /// <returns> a trade report template built from the definition in the ini file </returns>
	  public static CashFlowReportTemplate load(IniFile ini)
	  {
		CashFlowReportTemplateIniLoader loader = new CashFlowReportTemplateIniLoader();
		return loader.load(ini);
	  }

	}

}