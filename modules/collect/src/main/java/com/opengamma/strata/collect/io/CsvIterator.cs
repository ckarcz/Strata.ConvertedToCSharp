using System;
using System.Collections.Generic;
using System.IO;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using PeekingIterator = com.google.common.collect.PeekingIterator;
	using CharSource = com.google.common.io.CharSource;

	/// <summary>
	/// Iterator over the rows of a CSV file.
	/// <para>
	/// Provides the ability to iterate over a CSV file together with the ability to parse it from a <seealso cref="CharSource"/>.
	/// The separator may be specified, allowing TSV files (tab-separated) and other similar formats to be parsed.
	/// See <seealso cref="CsvFile"/> for more details of the CSV format.
	/// </para>
	/// <para>
	/// This class processes the CSV file row-by-row.
	/// To load the entire CSV file into memory, use <seealso cref="CsvFile"/>.
	/// </para>
	/// <para>
	/// This class must be used in a try-with-resources block to ensure that the underlying CSV file is closed:
	/// <pre>
	///  try (CsvIterator csvIterator = CsvIterator.of(source, true)) {
	///    // use the CsvIterator
	///  }
	/// </pre>
	/// One way to use the iterable is with the for-each loop, using a lambda to adapt {@code Iterator} to {@code Iterable}:
	/// <pre>
	///  try (CsvIterator csvIterator = CsvIterator.of(source, true)) {
	///    for (CsvRow row : () -&gt; csvIterator) {
	///      // process the row
	///    }
	///  }
	/// </pre>
	/// This class also allows the headers to be obtained without reading the whole CSV file:
	/// <pre>
	///  try (CsvIterator csvIterator = CsvIterator.of(source, true)) {
	///    ImmutableList{@literal <String>} headers = csvIterator.headers();
	///  }
	/// </pre>
	/// </para>
	/// </summary>
	public sealed class CsvIterator : AutoCloseable, PeekingIterator<CsvRow>
	{

	  /// <summary>
	  /// The buffered reader.
	  /// </summary>
	  private readonly StreamReader reader;
	  /// <summary>
	  /// The separator
	  /// </summary>
	  private readonly char separator;
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
	  /// The next row.
	  /// </summary>
	  private CsvRow nextRow;
	  /// <summary>
	  /// The current line number in the source file.
	  /// </summary>
	  private int currentLineNumber;

	  //------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the specified source as a CSV file, using a comma as the separator.
	  /// <para>
	  /// This method opens the CSV file for reading.
	  /// The caller is responsible for closing it by calling <seealso cref="#close()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="source">  the CSV file resource </param>
	  /// <param name="headerRow">  whether the source has a header row, an empty source must still contain the header </param>
	  /// <returns> the CSV file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static CsvIterator of(CharSource source, bool headerRow)
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
	  /// This method opens the CSV file for reading.
	  /// The caller is responsible for closing it by calling <seealso cref="#close()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="source">  the file resource </param>
	  /// <param name="headerRow">  whether the source has a header row, an empty source must still contain the header </param>
	  /// <param name="separator">  the separator used to separate each field, typically a comma, but a tab is sometimes used </param>
	  /// <returns> the CSV file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static CsvIterator of(CharSource source, bool headerRow, char separator)
	  {
		ArgChecker.notNull(source, "source");
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("resource") java.io.BufferedReader reader = com.opengamma.strata.collect.Unchecked.wrap(() -> source.openBufferedStream());
		StreamReader reader = Unchecked.wrap(() => source.openBufferedStream());
		return create(reader, headerRow, separator);
	  }

	  /// <summary>
	  /// Parses the specified reader as a CSV file, using a comma as the separator.
	  /// <para>
	  /// This factory method allows the separator to be controlled.
	  /// For example, a tab-separated file is very similar to a CSV file, the only difference is the separator.
	  /// </para>
	  /// <para>
	  /// The caller is responsible for closing the reader, such as by calling <seealso cref="#close()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reader">  the file reader </param>
	  /// <param name="headerRow">  whether the source has a header row, an empty source must still contain the header </param>
	  /// <returns> the CSV file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static CsvIterator of(Reader reader, bool headerRow)
	  {
		return of(reader, headerRow, ',');
	  }

	  /// <summary>
	  /// Parses the specified reader as a CSV file where the separator is specified and might not be a comma.
	  /// <para>
	  /// This factory method allows the separator to be controlled.
	  /// For example, a tab-separated file is very similar to a CSV file, the only difference is the separator.
	  /// </para>
	  /// <para>
	  /// The caller is responsible for closing the reader, such as by calling <seealso cref="#close()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reader">  the file reader </param>
	  /// <param name="headerRow">  whether the source has a header row, an empty source must still contain the header </param>
	  /// <param name="separator">  the separator used to separate each field, typically a comma, but a tab is sometimes used </param>
	  /// <returns> the CSV file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static CsvIterator of(Reader reader, bool headerRow, char separator)
	  {
		ArgChecker.notNull(reader, "reader");
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("resource") java.io.BufferedReader breader = reader instanceof java.io.BufferedReader ? (java.io.BufferedReader) reader : new java.io.BufferedReader(reader);
		StreamReader breader = reader is StreamReader ? (StreamReader) reader : new StreamReader(reader);
		return create(breader, headerRow, separator);
	  }

	  // create the iterator
	  private static CsvIterator create(StreamReader breader, bool headerRow, char separator)
	  {
		try
		{
		  if (!headerRow)
		  {
			return new CsvIterator(breader, separator, ImmutableList.of(), ImmutableMap.of(), 0);
		  }
		  string line = breader.ReadLine();
		  int lineNumber = 1;
		  while (!string.ReferenceEquals(line, null))
		  {
			ImmutableList<string> headers = CsvFile.parseLine(line, lineNumber, separator);
			if (!headers.Empty)
			{
			  return new CsvIterator(breader, separator, headers, CsvFile.buildSearchHeaders(headers), lineNumber);
			}
			line = breader.ReadLine();
			lineNumber++;
		  }
		  throw new System.ArgumentException("Could not read header row from empty CSV file");

		}
		catch (Exception ex)
		{
		  try
		  {
			breader.Close();
		  }
		  catch (IOException ex2)
		  {
			ex.addSuppressed(ex2);
		  }
		  throw ex;

		}
		catch (IOException ex)
		{
		  try
		  {
			breader.Close();
		  }
		  catch (IOException ex2)
		  {
			ex.addSuppressed(ex2);
		  }
		  throw new UncheckedIOException(ex);
		}
	  }

	  //------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  /// <param name="reader">  the buffered reader </param>
	  /// <param name="headers">  the header row </param>
	  /// <param name="searchHeaders">  the search headers </param>
	  private CsvIterator(StreamReader reader, char separator, ImmutableList<string> headers, ImmutableMap<string, int> searchHeaders, int currentLineNumber)
	  {

		this.reader = reader;
		this.separator = separator;
		this.headers_Renamed = headers;
		this.searchHeaders = searchHeaders;
		this.currentLineNumber = currentLineNumber;
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

	  /// <summary>
	  /// Returns a stream that wraps this iterator.
	  /// <para>
	  /// The stream will process any remaining rows in the CSV file.
	  /// As such, it is recommended that callers should use this method or the iterator methods and not both.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the stream wrapping this iterator </returns>
	  public Stream<CsvRow> asStream()
	  {
		Spliterator<CsvRow> spliterator = Spliterators.spliteratorUnknownSize(this, Spliterator.ORDERED | Spliterator.NONNULL);
		return StreamSupport.stream(spliterator, false);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks whether there is another row in the CSV file.
	  /// </summary>
	  /// <returns> true if there is another row, false if not </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public override bool hasNext()
	  {
		if (nextRow != null)
		{
		  return true;
		}
		else
		{
		  string line = null;
		  while (!string.ReferenceEquals((line = Unchecked.wrap(() => reader.ReadLine())), null))
		  {
			currentLineNumber++;
			ImmutableList<string> fields = CsvFile.parseLine(line, currentLineNumber, separator);
			if (!fields.Empty)
			{
			  nextRow = new CsvRow(headers_Renamed, searchHeaders, currentLineNumber, fields);
			  return true;
			}
		  }
		  return false;
		}
	  }

	  /// <summary>
	  /// Peeks the next row from the CSV file without changing the iteration position.
	  /// </summary>
	  /// <returns> the peeked row </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  /// <exception cref="NoSuchElementException"> if the end of file has been reached </exception>
	  public override CsvRow peek()
	  {
		if (nextRow != null || hasNext())
		{
		  return nextRow;
		}
		else
		{
		  throw new NoSuchElementException("CsvIterator has reached the end of the file");
		}
	  }

	  /// <summary>
	  /// Returns the next row from the CSV file.
	  /// </summary>
	  /// <returns> the next row </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  /// <exception cref="NoSuchElementException"> if the end of file has been reached </exception>
	  public override CsvRow next()
	  {
		if (nextRow != null || hasNext())
		{
		  CsvRow row = nextRow;
		  nextRow = null;
		  return row;
		}
		else
		{
		  throw new NoSuchElementException("CsvIterator has reached the end of the file");
		}
	  }

	  /// <summary>
	  /// Returns the next batch of rows from the CSV file.
	  /// <para>
	  /// This will return up to the specified number of rows from the file at the current iteration point.
	  /// An empty list is returned if there are no more rows.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="count">  the number of rows to try and get, negative returns an empty list </param>
	  /// <returns> the next batch of rows, up to the number requested </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public IList<CsvRow> nextBatch(int count)
	  {
		IList<CsvRow> rows = new List<CsvRow>();
		for (int i = 0; i < count; i++)
		{
		  if (hasNext())
		  {
			rows.Add(next());
		  }
		}
		return rows;
	  }

	  /// <summary>
	  /// Returns the next batch of rows from the CSV file using a predicate to determine the rows.
	  /// <para>
	  /// This is useful for CSV files where information is grouped with an identifier or key.
	  /// For example, a variable notional trade file might have one row for the trade followed by
	  /// multiple rows for the variable aspects, all grouped by a common trade identifier.
	  /// In general, callers should peek or read the first row and use information within it to
	  /// create the selector:
	  /// <pre>
	  ///  while (it.hasNext()) {
	  ///    CsvRow first = it.peek();
	  ///    String id = first.getValue("ID");
	  ///    List&lt;CsvRow&gt; batch = it.nextBatch(row -&gt; row.getValue("ID").equals(id));
	  ///    // process batch
	  ///  }
	  /// </pre>
	  /// This will return a batch of rows where the selector returns true for the row.
	  /// An empty list is returned if the selector returns false for the first row.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="selector">  selects whether a row is part of the batch or part of the next batch </param>
	  /// <returns> the next batch of rows, as determined by the selector </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public IList<CsvRow> nextBatch(System.Predicate<CsvRow> selector)
	  {
		IList<CsvRow> rows = new List<CsvRow>();
		while (hasNext() && selector(peek()))
		{
		  rows.Add(next());
		}
		return rows;
	  }

	  /// <summary>
	  /// Throws an exception as remove is not supported.
	  /// </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override void remove()
	  {
		throw new System.NotSupportedException("CsvIterator does not support remove()");
	  }

	  /// <summary>
	  /// Closes the underlying reader.
	  /// </summary>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  public override void close()
	  {
		Unchecked.wrap(() => reader.Close());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a string describing the CSV iterator.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		return "CsvIterator" + headers_Renamed.ToString();
	  }

	}

}