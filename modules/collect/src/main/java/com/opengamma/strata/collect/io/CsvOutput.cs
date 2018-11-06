using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	/// <summary>
	/// Outputs a CSV formatted file.
	/// <para>
	/// Provides a simple tool for writing a CSV file.
	/// </para>
	/// <para>
	/// Each line in the CSV file will consist of comma separated entries.
	/// Each entry may be quoted using a double quote.
	/// If an entry contains a double quote, comma or trimmable whitespace, it will be quoted.
	/// If an entry starts with '=' or '@', it will be quoted.
	/// Two double quotes will be used to escape a double quote.
	/// </para>
	/// <para>
	/// There are two modes of output.
	/// Standard mode provides the encoding described above which is accepted by most CSV parsers.
	/// Safe mode provides extra encoding to protect unsafe content from being run as a script in tools like Excel.
	/// </para>
	/// <para>
	/// Instances of this class contain mutable state.
	/// A new instance must be created for each file to be output.
	/// </para>
	/// </summary>
	public sealed class CsvOutput
	{

	  // regex for a number
	  private const string DIGITS = "([0-9]+)";
	  private static readonly string EXPONENT = "[eE][+-]?" + DIGITS;
	  private static readonly Pattern FP_REGEX = Pattern.compile("(" + DIGITS + "(\\.)?(" + DIGITS + "?)(" + EXPONENT + ")?)|" + "(\\.(" + DIGITS + ")(" + EXPONENT + ")?)");
	  private const string COMMA = ",";
	  private static readonly string NEW_LINE = Environment.NewLine;

	  /// <summary>
	  /// The header row, ordered as the headers appear in the file.
	  /// </summary>
	  private readonly Appendable underlying;
	  /// <summary>
	  /// The new line string.
	  /// </summary>
	  private readonly string newLine;
	  /// <summary>
	  /// The line item separator.
	  /// </summary>
	  private readonly string separator;
	  /// <summary>
	  /// Whether expressions should be safely encoded.
	  /// </summary>
	  private readonly bool safeExpressions;
	  /// <summary>
	  /// Whether the writer is currently at the start of a line.
	  /// </summary>
	  private bool lineStarted;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance, using the system default line separator and using a comma separator.
	  /// <para>
	  /// See the standard quoting rules in the class-level documentation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="underlying">  the destination to write to </param>
	  /// <returns> the CSV outputter </returns>
	  public static CsvOutput standard(Appendable underlying)
	  {
		return new CsvOutput(underlying, NEW_LINE, COMMA, false);
	  }

	  /// <summary>
	  /// Creates an instance, allowing the new line character to be controlled and using a comma separator.
	  /// <para>
	  /// See the standard quoting rules in the class-level documentation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="underlying">  the destination to write to </param>
	  /// <param name="newLine">  the new line string </param>
	  /// <returns> the CSV outputter </returns>
	  public static CsvOutput standard(Appendable underlying, string newLine)
	  {
		return new CsvOutput(underlying, newLine, COMMA, false);
	  }

	  /// <summary>
	  /// Creates an instance, allowing the new line character to be controlled, specifying the separator.
	  /// <para>
	  /// See the standard quoting rules in the class-level documentation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="underlying">  the destination to write to </param>
	  /// <param name="newLine">  the new line string </param>
	  /// <param name="separator">  the separator used to separate each field, typically a comma, but a tab is sometimes used </param>
	  /// <returns> the CSV outputter </returns>
	  public static CsvOutput standard(Appendable underlying, string newLine, string separator)
	  {
		return new CsvOutput(underlying, newLine, separator, false);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance, using the system default line separator and using a comma separator.
	  /// <para>
	  /// This applies the standard quoting rules from the class-level documentation, plus an additional rule.
	  /// If an entry starts with an expression character, '=', '@', '+' or '-', the entry
	  /// will be quoted and the quote section will be preceeded by equals.
	  /// Thus, the string '=Foo' will be written as '="=Foo"'.
	  /// This avoids the string being treated as an expression by tools like Excel.
	  /// Simple numbers are not quoted.
	  /// Thus, the number '-1234' will still be written as '-1234'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="underlying">  the destination to write to </param>
	  /// <returns> the CSV outputter </returns>
	  public static CsvOutput safe(Appendable underlying)
	  {
		return new CsvOutput(underlying, NEW_LINE, COMMA, true);
	  }

	  /// <summary>
	  /// Creates an instance, allowing the new line character to be controlled and using a comma separator.
	  /// <para>
	  /// This applies the standard quoting rules from the class-level documentation, plus an additional rule.
	  /// If an entry starts with an expression character, '=', '@', '+' or '-', the entry
	  /// will be quoted and the quote section will be preceeded by equals.
	  /// Thus, the string '=Foo' will be written as '="=Foo"'.
	  /// This avoids the string being treated as an expression by tools like Excel.
	  /// Simple numbers are not quoted.
	  /// Thus, the number '-1234' will still be written as '-1234'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="underlying">  the destination to write to </param>
	  /// <param name="newLine">  the new line string </param>
	  /// <returns> the CSV outputter </returns>
	  public static CsvOutput safe(Appendable underlying, string newLine)
	  {
		return new CsvOutput(underlying, newLine, COMMA, true);
	  }

	  /// <summary>
	  /// Creates an instance, allowing the new line character to be controlled, specifying the separator.
	  /// <para>
	  /// This applies the standard quoting rules from the class-level documentation, plus an additional rule.
	  /// If an entry starts with an expression character, '=', '@', '+' or '-', the entry
	  /// will be quoted and the quote section will be preceeded by equals.
	  /// Thus, the string '=Foo' will be written as '="=Foo"'.
	  /// This avoids the string being treated as an expression by tools like Excel.
	  /// Simple numbers are not quoted.
	  /// Thus, the number '-1234' will still be written as '-1234'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="underlying">  the destination to write to </param>
	  /// <param name="newLine">  the new line string </param>
	  /// <param name="separator">  the separator used to separate each field, typically a comma, but a tab is sometimes used </param>
	  /// <returns> the CSV outputter </returns>
	  public static CsvOutput safe(Appendable underlying, string newLine, string separator)
	  {
		return new CsvOutput(underlying, newLine, separator, true);
	  }

	  //-------------------------------------------------------------------------
	  // creates an instance
	  private CsvOutput(Appendable underlying, string newLine, string separator, bool safeExpressions)
	  {
		this.underlying = ArgChecker.notNull(underlying, "underlying");
		this.newLine = newLine;
		this.separator = separator;
		this.safeExpressions = safeExpressions;
	  }

	  //------------------------------------------------------------------------
	  /// <summary>
	  /// Writes multiple CSV lines to the underlying.
	  /// <para>
	  /// The boolean flag controls whether each entry is always quoted or only quoted when necessary.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="lines">  the lines to write </param>
	  /// <param name="alwaysQuote">  when true, each column will be quoted, when false, quoting is selective </param>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  public void writeLines<T1>(IEnumerable<T1> lines, bool alwaysQuote) where T1 : IList<string>
	  {
		ArgChecker.notNull(lines, "lines");
		foreach (IList<string> line in lines)
		{
		  writeLine(line, alwaysQuote);
		}
	  }

	  /// <summary>
	  /// Writes a single CSV line to the underlying, only quoting if needed.
	  /// <para>
	  /// This can be used as a method reference from a {@code Stream} pipeline from
	  /// <seealso cref="Stream#forEachOrdered(Consumer)"/>.
	  /// </para>
	  /// <para>
	  /// This method writes each cell in the specified list to the underlying, followed by
	  /// a new line character.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="line">  the line to write </param>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  public void writeLine(IList<string> line)
	  {
		writeLine(line, false);
	  }

	  /// <summary>
	  /// Writes a single CSV line to the underlying.
	  /// <para>
	  /// The boolean flag controls whether each entry is always quoted or only quoted when necessary.
	  /// </para>
	  /// <para>
	  /// This method writes each cell in the specified list to the underlying, followed by
	  /// a new line character.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="line">  the line to write </param>
	  /// <param name="alwaysQuote">  when true, each column will be quoted, when false, quoting is selective </param>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  public void writeLine(IList<string> line, bool alwaysQuote)
	  {
		ArgChecker.notNull(line, "line");
		foreach (string cell in line)
		{
		  writeCell(cell, alwaysQuote);
		}
		writeNewLine();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Writes a single cell to the current line, only quoting if needed.
	  /// <para>
	  /// When using this method, either <seealso cref="#writeNewLine()"/> or one of the {@code writeLine}
	  /// methods must be called at the end of the line.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cell">  the cell to write </param>
	  /// <returns> this, for method chaining </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  public CsvOutput writeCell(string cell)
	  {
		writeCell(cell, false);
		return this;
	  }

	  /// <summary>
	  /// Writes a single cell to the current line.
	  /// <para>
	  /// The boolean flag controls whether each entry is always quoted or only quoted when necessary.
	  /// </para>
	  /// <para>
	  /// When using this method, either <seealso cref="#writeNewLine()"/> or one of the {@code writeLine}
	  /// methods must be called at the end of the line.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cell">  the cell to write </param>
	  /// <param name="alwaysQuote">  when true, the cell will be quoted, when false, quoting is selective </param>
	  /// <returns> this, for method chaining </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  public CsvOutput writeCell(string cell, bool alwaysQuote)
	  {
		try
		{
		  if (lineStarted)
		  {
			underlying.append(separator);
		  }
		  if (alwaysQuote || isQuotingRequired(cell))
		  {
			outputQuotedCell(cell);
		  }
		  else
		  {
			underlying.append(cell);
		  }
		  lineStarted = true;
		}
		catch (IOException ex)
		{
		  throw new UncheckedIOException(ex);
		}
		return this;
	  }

	  /// <summary>
	  /// Writes a new line character.
	  /// </summary>
	  /// <returns> this, for method chaining </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  public CsvOutput writeNewLine()
	  {
		try
		{
		  underlying.append(newLine);
		}
		catch (IOException ex)
		{
		  throw new UncheckedIOException(ex);
		}
		lineStarted = false;
		return this;
	  }

	  //-------------------------------------------------------------------------
	  // quoting is required if entry contains quote, comma, trimmable whitespace, or starts with an expression character
	  private bool isQuotingRequired(string cell)
	  {
		return cell.IndexOf('"') >= 0 || cell.IndexOf(',') >= 0 || cell.Trim().Length != cell.Length || isExpressionPrefix(cell);
	  }

	  // checks if quoting should be applied
	  private bool isExpressionPrefix(string cell)
	  {
		if (cell.Length == 0)
		{
		  return false;
		}
		char first = cell[0];
		if (first == '=' || first == '@')
		{
		  return true;
		}
		if (safeExpressions && (first == '+' || first == '-'))
		{
		  return !FP_REGEX.matcher(cell.Substring(1)).matches();
		}
		return false;
	  }

	  // quotes the entry
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private void outputQuotedCell(String cell) throws java.io.IOException
	  private void outputQuotedCell(string cell)
	  {
		if (safeExpressions && isExpressionPrefix(cell))
		{
		  underlying.append('=');
		}
		underlying.append('"');
		underlying.append(cell.Replace("\"", "\"\""));
		underlying.append('"');
	  }

	}

}