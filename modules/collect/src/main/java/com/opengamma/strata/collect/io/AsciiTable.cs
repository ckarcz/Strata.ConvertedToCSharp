using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using Strings = com.google.common.@base.Strings;

	/// <summary>
	/// An ASCII table generator.
	/// <para>
	/// Provides the ability to generate a simple ASCII table, typically used on the command line.
	/// All data is provided as strings, with formatting the responsibility of the caller.
	/// </para>
	/// </summary>
	public sealed class AsciiTable
	{

	  /// <summary>
	  /// Line separator.
	  /// </summary>
	  private static readonly string LINE_SEPARATOR = Environment.NewLine;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Generates the ASCII table.
	  /// <para>
	  /// The caller specifies the headers for each column and the alignment to use,
	  /// plus the list of lists representing the data. All data is provided as strings,
	  /// with formatting the responsibility of the caller.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="headers">  the table headers </param>
	  /// <param name="alignments">  the table alignments, must match the size of the headers </param>
	  /// <param name="cells">  the table cells, outer list of rows, inner list of columns </param>
	  /// <returns> the table </returns>
	  /// <exception cref="IllegalArgumentException"> if the number of columns specified is inconsistent </exception>
	  public static string generate<T1>(IList<string> headers, IList<AsciiTableAlignment> alignments, IList<T1> cells) where T1 : IList<string>
	  {

		int colCount = alignments.Count;
		int rowCount = cells.Count;
		ArgChecker.isTrue(headers.Count == colCount, "Number of headers {} must match number of alignments {}", headers.Count, colCount);

		// find max length of each column
		int[] colLengths = new int[colCount];
		for (int colIdx = 0; colIdx < colCount; colIdx++)
		{
		  colLengths[colIdx] = headers[colIdx].Length;
		}
		for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
		{
		  IList<string> row = cells[rowIdx];
		  ArgChecker.isTrue(row.Count == colCount, "Table of cells has incorrect number of columns {} in row {}", row.Count, rowIdx);
		  for (int colIdx = 0; colIdx < colCount; colIdx++)
		  {
			colLengths[colIdx] = Math.Max(colLengths[colIdx], row[colIdx].Length);
		  }
		}
		int colTotalLength = 3; // allow for last vertical separator and windows line separator
		for (int colIdx = 0; colIdx < colCount; colIdx++)
		{
		  colTotalLength += colLengths[colIdx] + 3; // each column has two spaces and a vertical separator
		}

		// write table
		StringBuilder buf = new StringBuilder((rowCount + 3) * colTotalLength);
		writeSeparatorLine(buf, colLengths);
		writeDataLine(buf, colLengths, alignments, headers);
		writeSeparatorLine(buf, colLengths);
		for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
		{
		  writeDataLine(buf, colLengths, alignments, cells[rowIdx]);
		}
		writeSeparatorLine(buf, colLengths);
		return buf.ToString();
	  }

	  // write a separator line
	  private static void writeSeparatorLine(StringBuilder buf, int[] colLengths)
	  {
		for (int colIdx = 0; colIdx < colLengths.Length; colIdx++)
		{
		  buf.Append('+');
		  for (int i = 0; i < colLengths[colIdx] + 2; i++)
		  {
			buf.Append('-');
		  }
		}
		buf.Append('+').Append(LINE_SEPARATOR);
	  }

	  // write a data line
	  private static void writeDataLine(StringBuilder buf, int[] colLengths, IList<AsciiTableAlignment> alignments, IList<string> values)
	  {

		for (int colIdx = 0; colIdx < colLengths.Length; colIdx++)
		{
		  string value = Strings.nullToEmpty(values[colIdx]);
		  buf.Append('|').Append(' ').Append(formatValue(buf, colLengths[colIdx], alignments[colIdx], value)).Append(' ');
		}
		buf.Append('|').Append(LINE_SEPARATOR);
	  }

	  // writes a data item
	  private static string formatValue(StringBuilder buf, int colLength, AsciiTableAlignment alignment, string value)
	  {

		if (alignment == AsciiTableAlignment.RIGHT)
		{
		  return Strings.padStart(value, colLength, ' ');
		}
		else
		{
		  return Strings.padEnd(value, colLength, ' ');
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private AsciiTable()
	  {
	  }

	}

}