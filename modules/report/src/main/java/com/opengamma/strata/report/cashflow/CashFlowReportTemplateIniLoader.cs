/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.cashflow
{
	using IniFile = com.opengamma.strata.collect.io.IniFile;

	/// <summary>
	/// Loads a cash flow report template from the standard INI file format.
	/// </summary>
	public class CashFlowReportTemplateIniLoader : ReportTemplateIniLoader<CashFlowReportTemplate>
	{

	  /// <summary>
	  /// The report type.
	  /// </summary>
	  private const string REPORT_TYPE = "cashflow";

	  public virtual string ReportType
	  {
		  get
		  {
			return REPORT_TYPE;
		  }
	  }

	  public virtual CashFlowReportTemplate load(IniFile iniFile)
	  {
		return new CashFlowReportTemplate();
	  }

	}

}