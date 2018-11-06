using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.cashflow
{

	using FormatCategory = com.opengamma.strata.report.framework.format.FormatCategory;
	using FormatSettings = com.opengamma.strata.report.framework.format.FormatSettings;
	using ReportFormatter = com.opengamma.strata.report.framework.format.ReportFormatter;
	using ReportOutputFormat = com.opengamma.strata.report.framework.format.ReportOutputFormat;
	using ValueFormatters = com.opengamma.strata.report.framework.format.ValueFormatters;

	/// <summary>
	/// Formatter for cash flow reports.
	/// </summary>
	public sealed class CashFlowReportFormatter : ReportFormatter<CashFlowReport>
	{

	  /// <summary>
	  /// The single shared instance of this report formatter.
	  /// </summary>
	  public static readonly CashFlowReportFormatter INSTANCE = new CashFlowReportFormatter();

	  // restricted constructor
	  private CashFlowReportFormatter() : base(FormatSettings.of(FormatCategory.TEXT, ValueFormatters.TO_STRING))
	  {
	  }

	  //-------------------------------------------------------------------------
	  protected internal override IList<Type> getColumnTypes(CashFlowReport report)
	  {
		return Collections.nCopies(report.ColumnCount, typeof(object));
	  }

	  protected internal override string formatData(CashFlowReport report, int rowIdx, int colIdx, ReportOutputFormat format)
	  {
		object value = report.Data.get(rowIdx, colIdx);
		return formatValue(value, format);
	  }

	}

}