using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using Unchecked = com.opengamma.strata.collect.Unchecked;
	using AsciiTable = com.opengamma.strata.collect.io.AsciiTable;
	using AsciiTableAlignment = com.opengamma.strata.collect.io.AsciiTableAlignment;
	using CsvOutput = com.opengamma.strata.collect.io.CsvOutput;

	/// <summary>
	/// Common base class for formatting reports into ASCII tables or CSV format.
	/// </summary>
	/// @param <R>  the report type </param>
	public abstract class ReportFormatter<R> where R : com.opengamma.strata.report.Report
	{

	  /// <summary>
	  /// The default format settings, used if there are no settings for a data type.
	  /// </summary>
	  private readonly FormatSettings<object> defaultSettings;
	  /// <summary>
	  /// The format settings provider.
	  /// </summary>
	  private readonly FormatSettingsProvider formatSettingsProvider = FormatSettingsProvider.INSTANCE;

	  /// <summary>
	  /// Creates a new formatter with a set of default format settings.
	  /// </summary>
	  /// <param name="defaultSettings">  default format settings, used if there are no settings for a data type. </param>
	  protected internal ReportFormatter(FormatSettings<object> defaultSettings)
	  {
		this.defaultSettings = defaultSettings;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Outputs the report table in CSV format.
	  /// </summary>
	  /// <param name="report">  the report </param>
	  /// <param name="out">  the output stream to write to </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("resource") public void writeCsv(R report, java.io.OutputStream out)
	  public virtual void writeCsv(R report, Stream @out)
	  {
		StreamWriter outputWriter = new StreamWriter(@out, Encoding.UTF8);
		CsvOutput csvOut = CsvOutput.safe(outputWriter);
		csvOut.writeLine(report.ColumnHeaders);
		IntStream.range(0, report.RowCount).mapToObj(rowIdx => formatRow(report, rowIdx, ReportOutputFormat.CSV)).forEach(csvOut.writeLine);
		Unchecked.wrap(outputWriter.flush);
	  }

	  /// <summary>
	  /// Outputs the report as an ASCII table.
	  /// </summary>
	  /// <param name="report">  the report </param>
	  /// <param name="out">  the output stream to write to </param>
	  public virtual void writeAsciiTable(R report, Stream @out)
	  {
		IList<Type> columnTypes = getColumnTypes(report);
		IList<AsciiTableAlignment> alignments = IntStream.range(0, columnTypes.Count).mapToObj(i => calculateAlignment(columnTypes[i])).collect(toImmutableList());
		IList<string> headers = report.ColumnHeaders;
		ImmutableList<ImmutableList<string>> cells = formatAsciiTable(report);
		string asciiTable = AsciiTable.generate(headers, alignments, cells);
		PrintWriter pw = new PrintWriter(new StreamWriter(@out, Encoding.UTF8));
		pw.println(asciiTable);
		pw.flush();
	  }

	  // calculates the alignment to use
	  private AsciiTableAlignment calculateAlignment(Type columnType)
	  {
		FormatSettings<object> formatSettings = formatSettingsProvider.settings(columnType, defaultSettings);
		bool isNumeric = formatSettings.Category == FormatCategory.NUMERIC || formatSettings.Category == FormatCategory.DATE;
		return isNumeric ? AsciiTableAlignment.RIGHT : AsciiTableAlignment.LEFT;
	  }

	  // formats the ASCII table
	  private ImmutableList<ImmutableList<string>> formatAsciiTable(R report)
	  {
		ImmutableList.Builder<ImmutableList<string>> table = ImmutableList.builder();
		for (int rowIdx = 0; rowIdx < report.RowCount; rowIdx++)
		{
		  table.add(formatRow(report, rowIdx, ReportOutputFormat.ASCII_TABLE));
		}
		return table.build();
	  }

	  // formats a single row
	  private ImmutableList<string> formatRow(R report, int rowIdx, ReportOutputFormat format)
	  {
		ImmutableList.Builder<string> tableRow = ImmutableList.builder();
		for (int colIdx = 0; colIdx < report.ColumnCount; colIdx++)
		{
		  tableRow.add(formatData(report, rowIdx, colIdx, format));
		}
		return tableRow.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of the data in each report column.
	  /// <para>
	  /// If every value in a column is a failure the type will be {@code Object.class}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="report">  the report </param>
	  /// <returns> a list of column types </returns>
	  protected internal abstract IList<Type> getColumnTypes(R report);

	  /// <summary>
	  /// Formats a piece of data for display.
	  /// </summary>
	  /// <param name="report"> the report containing the data </param>
	  /// <param name="rowIdx">  the row index of the data </param>
	  /// <param name="colIdx">  the column index of the data </param>
	  /// <param name="format">  the report output format </param>
	  /// <returns> the formatted data </returns>
	  protected internal abstract string formatData(R report, int rowIdx, int colIdx, ReportOutputFormat format);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Formats a value into a string.
	  /// </summary>
	  /// <param name="value">  the value </param>
	  /// <param name="format">  the format that controls how the value is formatted </param>
	  /// <returns> the formatted value </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") protected String formatValue(Object value, ReportOutputFormat format)
	  protected internal virtual string formatValue(object value, ReportOutputFormat format)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: Object formatValue = value instanceof java.util.Optional ? ((java.util.Optional<?>) value).orElse(null) : value;
		object formatValue = value is Optional ? ((Optional<object>) value).orElse(null) : value;

		if (formatValue == null)
		{
		  return "";
		}
		FormatSettings<object> formatSettings = formatSettingsProvider.settings(formatValue.GetType(), defaultSettings);
		ValueFormatter<object> formatter = formatSettings.Formatter;

		return format == ReportOutputFormat.CSV ? formatter.formatForCsv(formatValue) : formatter.formatForDisplay(formatValue);
	  }

	}

}