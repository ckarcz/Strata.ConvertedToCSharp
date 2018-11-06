using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.trade
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Messages = com.opengamma.strata.collect.Messages;
	using Result = com.opengamma.strata.collect.result.Result;
	using FormatCategory = com.opengamma.strata.report.framework.format.FormatCategory;
	using FormatSettings = com.opengamma.strata.report.framework.format.FormatSettings;
	using ReportFormatter = com.opengamma.strata.report.framework.format.ReportFormatter;
	using ReportOutputFormat = com.opengamma.strata.report.framework.format.ReportOutputFormat;
	using ValueFormatters = com.opengamma.strata.report.framework.format.ValueFormatters;

	/// <summary>
	/// Formatter for trade reports.
	/// </summary>
	public sealed class TradeReportFormatter : ReportFormatter<TradeReport>
	{

	  /// <summary>
	  /// The single shared instance of this report formatter.
	  /// </summary>
	  public static readonly TradeReportFormatter INSTANCE = new TradeReportFormatter();

	  // restricted constructor
	  private TradeReportFormatter() : base(FormatSettings.of(FormatCategory.TEXT, ValueFormatters.UNSUPPORTED))
	  {
	  }

	  //-------------------------------------------------------------------------
	  protected internal override IList<Type> getColumnTypes(TradeReport report)
	  {
		return IntStream.range(0, report.ColumnCount).mapToObj(columnIndex => columnType(report, columnIndex)).collect(toImmutableList());
	  }

	  // TODO This would be unnecessary if measures had a data type
	  /// <summary>
	  /// Returns the data type for the values in a column of a trade report.
	  /// <para>
	  /// The results in the column are examined and the type of the first successful value is returned. If all values
	  /// are failures then {@code Object.class} is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="report">  a trade report </param>
	  /// <param name="columnIndex">  the index of a column in the report </param>
	  /// <returns> the data type of the values in the column or {@code Object.class} if all results are failures </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes"}) private static Class columnType(TradeReport report, int columnIndex)
	  private static Type columnType(TradeReport report, int columnIndex)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return report.Data.rowKeySet().Select(rowIndex => report.Data.get(rowIndex, columnIndex)).Where(Result::isSuccess).Select(Result::getValue).Select(object.getClass).First().orElse((Type) typeof(object)); // raw type needed for Eclipse
	  }

	  protected internal override string formatData(TradeReport report, int rowIdx, int colIdx, ReportOutputFormat format)
	  {
		TradeReportColumn templateColumn = report.Columns.get(colIdx);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = report.getData().get(rowIdx, colIdx);
		Result<object> result = report.Data.get(rowIdx, colIdx);

		if (result.Failure)
		{
		  return templateColumn.IgnoreFailures ? "" : Messages.format("FAIL: {}", result.Failure.Message);
		}
		object value = result.Value;
		return formatValue(value, format);
	  }

	}

}