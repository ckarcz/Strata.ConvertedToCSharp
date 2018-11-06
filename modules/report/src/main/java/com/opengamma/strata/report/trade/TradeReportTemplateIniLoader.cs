using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.trade
{

	using IniFile = com.opengamma.strata.collect.io.IniFile;
	using PropertySet = com.opengamma.strata.collect.io.PropertySet;

	/// <summary>
	/// Loads a trade report template from the standard INI file format.
	/// <para>
	/// In a trade report template, the sections in the INI file (other than the special settings
	/// section) correspond to the columns in the report, in the order they are declared.
	/// </para>
	/// <para>
	/// Each section can specify the following properties:
	/// <ul>
	/// <li>value - identifies the value to display in the column's cells
	/// <li>ignoreFailures - optional boolean flag to disable failure messages in this column
	/// </ul>
	/// </para>
	/// </summary>
	public class TradeReportTemplateIniLoader : ReportTemplateIniLoader<TradeReportTemplate>
	{

	  /// <summary>
	  /// The report type.
	  /// </summary>
	  private const string REPORT_TYPE = "trade";
	  /// <summary>
	  /// The value property name.
	  /// </summary>
	  private const string VALUE_PROPERTY = "value";
	  /// <summary>
	  /// The ignore-failures property name.
	  /// </summary>
	  private const string IGNORE_FAILURES_PROPERTY = "ignoreFailures";

	  //-------------------------------------------------------------------------
	  public virtual string ReportType
	  {
		  get
		  {
			return REPORT_TYPE;
		  }
	  }

	  public virtual TradeReportTemplate load(IniFile iniFile)
	  {
		IList<TradeReportColumn> reportColumns = new List<TradeReportColumn>();
		foreach (string columnName in iniFile.sections())
		{
		  if (columnName.ToLower(Locale.ENGLISH).Equals(com.opengamma.strata.report.ReportTemplateIniLoader_Fields.SETTINGS_SECTION))
		  {
			continue;
		  }
		  PropertySet properties = iniFile.section(columnName);
		  reportColumns.Add(parseColumn(columnName, properties));
		}
		return TradeReportTemplate.builder().columns(reportColumns).build();
	  }

	  private TradeReportColumn parseColumn(string columnName, PropertySet properties)
	  {
		TradeReportColumn.Builder columnBuilder = TradeReportColumn.builder();
		columnBuilder.header(columnName);

		if (properties.contains(VALUE_PROPERTY))
		{
		  columnBuilder.value(properties.value(VALUE_PROPERTY));
		}
		if (properties.contains(IGNORE_FAILURES_PROPERTY))
		{
		  string ignoreFailuresValue = properties.value(IGNORE_FAILURES_PROPERTY);
		  bool ignoresFailure = Convert.ToBoolean(ignoreFailuresValue);
		  columnBuilder.ignoreFailures(ignoresFailure);
		}
		return columnBuilder.build();
	  }

	}

}