using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CharSource = com.google.common.io.CharSource;
	using CharStreams = com.google.common.io.CharStreams;

	/// <summary>
	/// A CSV file.
	/// <para>
	/// Represents a CSV file together with the ability to parse it from a <seealso cref="CharSource"/>.
	/// The separator may be specified, allowing TSV files (tab-separated) and other similar formats to be parsed.
	/// </para>
	/// <para>
	/// This class loads the entire CSV file into memory.
	/// To process the CSV file row-by-row, use <seealso cref="CsvIterator"/>.
	/// </para>
	/// <para>
	/// The CSV file format is a general-purpose comma-separated value format.
	/// The format is parsed line-by-line, with lines separated by CR, LF or CRLF.
	/// Each line can contain one or more fields.
	/// Each field is separated by a comma character ({@literal ,}) or tab.
	/// Any field may be quoted using a double quote at the start and end.
	/// A quoted field may additionally be prefixed by an equals sign.
	/// The content of a quoted field may include commas and additional double quotes.
	/// Two adjacent double quotes in a quoted field will be replaced by a single double quote.
	/// Quoted fields are not trimmed. Non-quoted fields are trimmed.
	/// </para>
	/// <para>
	/// The first line may be treated as a header row.
	/// The header row is accessed separately from the data rows.
	/// </para>
	/// <para>
	/// Blank lines are ignored.
	/// Lines may be commented with has '#' or semicolon ';'.
	/// </para>
	/// </summary>
	public sealed class CsvFile
	{

	  /// <summary>
	  /// The header row, ordered as the headers appear in the file.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly ImmutableList<string> headers_Renamed;
	  /// <summary>
	  /// The header map, transformed for case-insensitive searching.
	  /// </summary>
	  private readonly ImmutableMap<string, int> searchHeaders;
	  /// <summary>
	  /// The data rows in the CSV file.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly ImmutableList<CsvRow> rows_Renamed;

	  //------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the specified source as a CSV file, using a comma as the separator.
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="source">  the CSV file resource </param>
	  /// <param name="headerRow">  whether the source has a header row, an empty source must still contain the header </param>
	  /// <returns> the CSV file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static CsvFile of(CharSource source, bool headerRow)
	  {
		return of(source, headerRow, ',');
	  }

	  /// <summary>
	  /// Parses the specified source as a CSV file where the separator is specified and might not be a comma.
	  /// <para>
	  /// This overload allows the separator to be controlled.
	  /// For example, a tab-separated file is very similar to a CSV file, the only difference is the separator.
	  /// </para>
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="source">  the file resource </param>
	  /// <param name="headerRow">  whether the source has a header row, an empty source must still contain the header </param>
	  /// <param name="separator">  the separator used to separate each field, typically a comma, but a tab is sometimes used </param>
	  /// <returns> the CSV file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static CsvFile of(CharSource source, bool headerRow, char separator)
	  {
		ArgChecker.notNull(source, "source");
		IList<string> lines = Unchecked.wrap(() => source.readLines());
		return create(lines, headerRow, separator);
	  }

	  /// <summary>
	  /// Parses the specified reader as a CSV file, using a comma as the separator.
	  /// <para>
	  /// This factory method takes a <seealso cref="Reader"/>.
	  /// Callers are encouraged to use <seealso cref="CharSource"/> instead of {@code Reader}
	  /// as it allows the resource to be safely managed.
	  /// </para>
	  /// <para>
	  /// This factory method allows the separator to be controlled.
	  /// For example, a tab-separated file is very similar to a CSV file, the only difference is the separator.
	  /// </para>
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reader">  the file resource </param>
	  /// <param name="headerRow">  whether the source has a header row, an empty source must still contain the header </param>
	  /// <returns> the CSV file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static CsvFile of(Reader reader, bool headerRow)
	  {
		return of(reader, headerRow, ',');
	  }

	  /// <summary>
	  /// Parses the specified reader as a CSV file where the separator is specified and might not be a comma.
	  /// <para>
	  /// This factory method takes a <seealso cref="Reader"/>.
	  /// Callers are encouraged to use <seealso cref="CharSource"/> instead of {@code Reader}
	  /// as it allows the resource to be safely managed.
	  /// </para>
	  /// <para>
	  /// This factory method allows the separator to be controlled.
	  /// For example, a tab-separated file is very similar to a CSV file, the only difference is the separator.
	  /// </para>
	  /// <para>
	  /// CSV files sometimes contain a Unicode Byte Order Mark.
	  /// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reader">  the file resource </param>
	  /// <param name="headerRow">  whether the source has a header row, an empty source must still contain the header </param>
	  /// <param name="separator">  the separator used to separate each field, typically a comma, but a tab is sometimes used </param>
	  /// <returns> the CSV file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static CsvFile of(Reader reader, bool headerRow, char separator)
	  {
		ArgChecker.notNull(reader, "source");
		IList<string> lines = Unchecked.wrap(() => CharStreams.readLines(reader));
		return create(lines, headerRow, separator);
	  }

	  // creates the file
	  private static CsvFile create(IList<string> lines, bool headerRow, char separator)
	  {
		if (headerRow)
		{
		  for (int i = 0; i < lines.Count; i++)
		  {
			ImmutableList<string> headers = parseLine(lines[i], i + 1, separator);
			if (!headers.Empty)
			{
			  ImmutableMap<string, int> searchHeaders = buildSearchHeaders(headers);
			  return parseAll(lines, i + 1, separator, headers, searchHeaders);
			}
		  }
		  throw new System.ArgumentException("Could not read header row from empty CSV file");
		}
		return parseAll(lines, 0, separator, ImmutableList.of(), ImmutableMap.of());
	  }

	  //------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a list of headers and rows.
	  /// <para>
	  /// The headers may be an empty list.
	  /// All the rows must contain a list of the same size, matching the header if present.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="headers">  the headers, empty if no headers </param>
	  /// <param name="rows">  the data rows </param>
	  /// <returns> the CSV file </returns>
	  /// <exception cref="IllegalArgumentException"> if the rows do not match the headers </exception>
	  public static CsvFile of<T1>(IList<string> headers, IList<T1> rows) where T1 : IList<string>
	  {
		ArgChecker.notNull(headers, "headers");
		ArgChecker.notNull(rows, "rows");
		int size = (headers.Count == 0 && rows.Count > 0 ? rows[0].size() : headers.Count);
		if (rows.Where(row => row.size() != size).First().Present)
		{
		  throw new System.ArgumentException("Invalid data rows, each row must have same columns as header row");
		}
		ImmutableList<string> copiedHeaders = ImmutableList.copyOf(headers);
		ImmutableMap<string, int> searchHeaders = buildSearchHeaders(copiedHeaders);
		ImmutableList.Builder<CsvRow> csvRows = ImmutableList.builder();
		int firstLine = copiedHeaders.Empty ? 1 : 2;
		for (int i = 0; i < rows.Count; i++)
		{
		  csvRows.add(new CsvRow(copiedHeaders, searchHeaders, i + firstLine, ImmutableList.copyOf(rows[i])));
		}
		return new CsvFile(copiedHeaders, searchHeaders, csvRows.build());
	  }

	  //------------------------------------------------------------------------
	  // parses the CSV file format
	  private static CsvFile parseAll(IList<string> lines, int lineIndex, char separator, ImmutableList<string> headers, ImmutableMap<string, int> searchHeaders)
	  {

		ImmutableList.Builder<CsvRow> rows = ImmutableList.builder();
		for (int i = lineIndex; i < lines.Count; i++)
		{
		  ImmutableList<string> fields = parseLine(lines[i], i + 1, separator);
		  if (!fields.Empty)
		  {
			rows.add(new CsvRow(headers, searchHeaders, i + 1, fields));
		  }
		}
		return new CsvFile(headers, searchHeaders, rows.build());
	  }

	  // parse a single line
	  internal static ImmutableList<string> parseLine(string line, int lineNumber, char separator)
	  {
		if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal) || line.StartsWith(";", StringComparison.Ordinal))
		{
		  return ImmutableList.of();
		}
		ImmutableList.Builder<string> builder = ImmutableList.builder();
		int start = 0;
		string terminated = line + separator;
		int nextSeparator = terminated.IndexOf(separator, start);
		while (nextSeparator >= 0)
		{
		  string possible = terminated.Substring(start, nextSeparator - start).Trim();
		  // handle convention where ="xxx" means xxx
		  if (possible.StartsWith("=\"", StringComparison.Ordinal))
		  {
			start++;
			possible = possible.Substring(1);
		  }
		  // handle quoting where "xxx""yyy" means xxx"yyy
		  if (possible.StartsWith("\"", StringComparison.Ordinal))
		  {
			while (true)
			{
			  if (possible.Substring(1).Replace("\"\"", "").EndsWith("\"", StringComparison.Ordinal))
			  {
				possible = possible.Substring(1, (possible.Length - 1) - 1).Replace("\"\"", "\"");
				break;
			  }
			  else
			  {
				nextSeparator = terminated.IndexOf(separator, nextSeparator + 1);
				if (nextSeparator < 0)
				{
				  throw new System.ArgumentException("Mismatched quotes in CSV on line " + lineNumber);
				}
				possible = terminated.Substring(start, nextSeparator - start).Trim();
			  }
			}
		  }
		  builder.add(possible);
		  start = nextSeparator + 1;
		  nextSeparator = terminated.IndexOf(separator, start);
		}
		ImmutableList<string> fields = builder.build();
		if (!hasContent(fields))
		{
		  return ImmutableList.of();
		}
		return fields;
	  }

	  // determines whether there is any content on a line
	  // this handles lines that contain separators but nothing else
	  private static bool hasContent(ImmutableList<string> fields)
	  {
		foreach (string field in fields)
		{
		  if (field.Trim().Length > 0)
		  {
			return true;
		  }
		}
		return false;
	  }

	  // build the search headers
	  internal static ImmutableMap<string, int> buildSearchHeaders(ImmutableList<string> headers)
	  {
		// need to allow duplicate headers and only store the first instance
		IDictionary<string, int> searchHeaders = new Dictionary<string, int>();
		for (int i = 0; i < headers.size(); i++)
		{
		  string searchHeader = headers.get(i).ToLower(Locale.ENGLISH);
		  if (!searchHeaders.ContainsKey(searchHeader)) searchHeaders.Add(searchHeader, i);
		}
		return ImmutableMap.copyOf(searchHeaders);
	  }

	  //------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  /// <param name="headers">  the header row </param>
	  /// <param name="searchHeaders">  the headers transformed for searching </param>
	  /// <param name="rows">  the data rows </param>
	  private CsvFile(ImmutableList<string> headers, ImmutableMap<string, int> searchHeaders, ImmutableList<CsvRow> rows)
	  {

		this.headers_Renamed = headers;
		this.searchHeaders = searchHeaders;
		this.rows_Renamed = rows;
	  }

	  //------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the header row.
	  /// <para>
	  /// If there is no header row, an empty list is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the header row </returns>
	  public ImmutableList<string> headers()
	  {
		return headers_Renamed;
	  }

	  /// <summary>
	  /// Gets all data rows in the file.
	  /// </summary>
	  /// <returns> the data rows </returns>
	  public ImmutableList<CsvRow> rows()
	  {
		return rows_Renamed;
	  }

	  /// <summary>
	  /// Gets the number of data rows.
	  /// </summary>
	  /// <returns> the number of data rows </returns>
	  public int rowCount()
	  {
		return rows_Renamed.size();
	  }

	  /// <summary>
	  /// Gets a single row.
	  /// </summary>
	  /// <param name="index">  the row index, zero-based </param>
	  /// <returns> the row </returns>
	  public CsvRow row(int index)
	  {
		return rows_Renamed.get(index);
	  }

	  /// <summary>
	  /// Checks if the header is known.
	  /// <para>
	  /// Matching is case insensitive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="header">  the column header to match </param>
	  /// <returns> true if the header is known </returns>
	  public bool containsHeader(string header)
	  {
		return searchHeaders.containsKey(header.ToLower(Locale.ENGLISH));
	  }

	  /// <summary>
	  /// Checks if the header pattern is known.
	  /// <para>
	  /// Matching is case insensitive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="headerPattern">  the header pattern to match </param>
	  /// <returns> true if the header is known </returns>
	  public bool containsHeader(Pattern headerPattern)
	  {
		for (int i = 0; i < headers_Renamed.size(); i++)
		{
		  if (headerPattern.matcher(headers_Renamed.get(i)).matches())
		  {
			return true;
		  }
		}
		return false;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this CSV file equals another.
	  /// <para>
	  /// The comparison checks the content.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other file, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is CsvFile)
		{
		  CsvFile other = (CsvFile) obj;
		  return headers_Renamed.Equals(other.headers_Renamed) && rows_Renamed.Equals(other.rows_Renamed);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the CSV file.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return headers_Renamed.GetHashCode() ^ rows_Renamed.GetHashCode();
	  }

	  /// <summary>
	  /// Returns a string describing the CSV file.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		return "CsvFile" + headers_Renamed.ToString();
	  }

	}

}