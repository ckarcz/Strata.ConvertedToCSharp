using System.IO;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report
{

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Represents a business report.
	/// <para>
	/// A report is a transformation of calculation engine results for a specific purpose, for example
	/// a trade report on a list of trades, or a cashflow report on a single trade.
	/// </para>
	/// <para>
	/// The report physically represents a table of data, with column headers.
	/// </para>
	/// </summary>
	public interface Report
	{

	  /// <summary>
	  /// Gets the valuation date of the results driving the report.
	  /// </summary>
	  /// <returns> the valuation date </returns>
	  LocalDate ValuationDate {get;}

	  /// <summary>
	  /// Gets the instant at which the report was run, which is independent of the valuation date.
	  /// </summary>
	  /// <returns> the run instant </returns>
	  Instant RunInstant {get;}

	  /// <summary>
	  /// Gets the number of rows in the report table.
	  /// </summary>
	  /// <returns> the number of rows in the report table </returns>
	  int RowCount {get;}

	  /// <summary>
	  /// Gets the report column headers.
	  /// </summary>
	  /// <returns> the column headers </returns>
	  ImmutableList<string> ColumnHeaders {get;}

	  /// <summary>
	  /// Writes this report out in a CSV format.
	  /// </summary>
	  /// <param name="out">  the output stream to write to </param>
	  void writeCsv(Stream @out);

	  /// <summary>
	  /// Writes this report out as an ASCII table.
	  /// </summary>
	  /// <param name="out">  the output stream to write to </param>
	  void writeAsciiTable(Stream @out);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of columns in the report table.
	  /// </summary>
	  /// <returns> the number of columns in the report table </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default int getColumnCount()
	//  {
	//	return getColumnHeaders().size();
	//  }

	  /// <summary>
	  /// Gets this report as an ASCII table string.
	  /// </summary>
	  /// <returns> the ASCII table string </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default String toAsciiTableString()
	//  {
	//	ByteArrayOutputStream os = new ByteArrayOutputStream();
	//	writeAsciiTable(os);
	//	return new String(os.toByteArray(), StandardCharsets.UTF_8);
	//  }

	}

}