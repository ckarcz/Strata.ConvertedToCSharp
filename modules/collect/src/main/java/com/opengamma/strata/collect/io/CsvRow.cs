using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// A row in a CSV file.
	/// <para>
	/// Represents a single row in a CSV file, accessed via <seealso cref="CsvFile"/>.
	/// Each row object provides access to the data in the row by field index.
	/// If the CSV file has headers, the headers can also be used to lookup the fields.
	/// </para>
	/// </summary>
	public sealed class CsvRow
	{
	  // some methods have been inlined/simplified for startup/performance reasons

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
	  /// The fields in the row.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly ImmutableList<string> fields_Renamed;
	  /// <summary>
	  /// The line number in the source file.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly int lineNumber_Renamed;

	  //------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance, specifying the headers and row.
	  /// <para>
	  /// See <seealso cref="CsvFile"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="headers">  the headers </param>
	  /// <param name="fields">  the fields </param>
	  private CsvRow(ImmutableList<string> headers, int lineNumber, ImmutableList<string> fields)
	  {
		this.headers_Renamed = headers;
		// need to allow duplicate headers and only store the first instance
		IDictionary<string, int> searchHeaders = new Dictionary<string, int>();
		for (int i = 0; i < headers.size(); i++)
		{
		  string searchHeader = headers.get(i).ToLower(Locale.ENGLISH);
		  if (!searchHeaders.ContainsKey(searchHeader)) searchHeaders.Add(searchHeader, i);
		}
		this.searchHeaders = ImmutableMap.copyOf(searchHeaders);
		this.lineNumber_Renamed = lineNumber;
		this.fields_Renamed = fields;
	  }

	  /// <summary>
	  /// Creates an instance, specifying the headers and row.
	  /// <para>
	  /// See <seealso cref="CsvFile"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="headers">  the headers </param>
	  /// <param name="searchHeaders">  the search headers </param>
	  /// <param name="fields">  the fields </param>
	  internal CsvRow(ImmutableList<string> headers, ImmutableMap<string, int> searchHeaders, int lineNumber, ImmutableList<string> fields)
	  {

		this.headers_Renamed = headers;
		this.searchHeaders = searchHeaders;
		this.lineNumber_Renamed = lineNumber;
		this.fields_Renamed = fields;
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
	  /// Gets the line number in the source file.
	  /// </summary>
	  /// <returns> the line number </returns>
	  public int lineNumber()
	  {
		return lineNumber_Renamed;
	  }

	  /// <summary>
	  /// Gets all fields in the row.
	  /// </summary>
	  /// <returns> the fields </returns>
	  public ImmutableList<string> fields()
	  {
		return fields_Renamed;
	  }

	  /// <summary>
	  /// Gets the number of fields.
	  /// <para>
	  /// This will never be less than the number of headers.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of fields </returns>
	  public int fieldCount()
	  {
		return Math.Max(fields_Renamed.size(), headers_Renamed.size());
	  }

	  /// <summary>
	  /// Gets the specified field.
	  /// </summary>
	  /// <param name="index">  the field index </param>
	  /// <returns> the field </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the field index is invalid </exception>
	  public string field(int index)
	  {
		if (index >= fields_Renamed.size() && index < headers_Renamed.size())
		{
		  return "";
		}
		return fields_Renamed.get(index);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets a single field value from the row by header.
	  /// <para>
	  /// This returns the value of the first column where the header matches the specified header.
	  /// Matching is case insensitive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="header">  the column header </param>
	  /// <returns> the trimmed field value </returns>
	  /// <exception cref="IllegalArgumentException"> if the header is not found </exception>
	  public string getField(string header)
	  {
		int? index = findIndex(header);
		if (index == null)
		{
		  throw new System.ArgumentException("Header not found: '" + header + "'");
		}
		return field(index.Value);
	  }

	  /// <summary>
	  /// Gets a single field value from the row by header.
	  /// <para>
	  /// This returns the value of the first column where the header matches the specified header.
	  /// Matching is case insensitive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="header">  the column header </param>
	  /// <returns> the trimmed field value, empty if not found </returns>
	  public Optional<string> findField(string header)
	  {
		int? index = findIndex(header);
		return index == null ? null : field(index.Value);
	  }

	  // finds the index of the specified header
	  private int? findIndex(string header)
	  {
		return searchHeaders.get(header.ToLower(Locale.ENGLISH));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets a single field value from the row by header pattern.
	  /// <para>
	  /// This returns the value of the first column where the header matches the specified header pattern.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="headerPattern">  the header pattern to match </param>
	  /// <returns> the trimmed field value, empty </returns>
	  /// <exception cref="IllegalArgumentException"> if the header is not found </exception>
	  public string getField(Pattern headerPattern)
	  {
		for (int i = 0; i < headers_Renamed.size(); i++)
		{
		  if (headerPattern.matcher(headers_Renamed.get(i)).matches())
		  {
			return field(i);
		  }
		}
		throw new System.ArgumentException("Header pattern not found: '" + headerPattern + "'");
	  }

	  /// <summary>
	  /// Gets a single field value from the row by header pattern.
	  /// <para>
	  /// This returns the value of the first column where the header matches the specified header pattern.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="headerPattern">  the header pattern to match </param>
	  /// <returns> the trimmed field value, empty if not found </returns>
	  public Optional<string> findField(Pattern headerPattern)
	  {
		for (int i = 0; i < headers_Renamed.size(); i++)
		{
		  if (headerPattern.matcher(headers_Renamed.get(i)).matches())
		  {
			return field(i);
		  }
		}
		return null;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets a single field value from the row by header
	  /// <para>
	  /// This returns the value of the first column where the header matches the specified header.
	  /// If the header is not found or the value found is an empty string, then an IllegalArgumentException is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="header"> the column header </param>
	  /// <returns> the trimmed field value, empty </returns>
	  /// <exception cref="IllegalArgumentException"> if the header is not found or if the value in the field is empty. </exception>
	  public string getValue(string header)
	  {
		string value = getField(header);
		if (value.Length == 0)
		{
		  throw new System.ArgumentException("No value was found for field: '" + header + "'");
		}
		return value;
	  }

	  /// <summary>
	  /// Gets a single value from the row by header.
	  /// <para>
	  /// This returns the value of the first column where the header matches the specified header pattern.
	  /// If the value is an empty string, then an empty optional is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="header"> the column header </param>
	  /// <returns> the trimmed field value, empty </returns>
	  public Optional<string> findValue(string header)
	  {
		return findField(header).filter(str => !str.Empty);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets a single field value from the row by header pattern
	  /// <para>
	  /// This returns the value of the first column where the header matches the specified header pattern.
	  /// If the header is not found or the value found is an empty string, then an IllegalArgumentException is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="headerPattern"> the header pattern to match </param>
	  /// <returns> the trimmed field value </returns>
	  /// <exception cref="IllegalArgumentException"> if the header is not found or if the value in the field is empty. </exception>
	  public string getValue(Pattern headerPattern)
	  {
		string value = getField(headerPattern);
		if (value.Length == 0)
		{
		  throw new System.ArgumentException("No value was found for header pattern: '" + headerPattern + "'");
		}
		return value;
	  }

	  /// <summary>
	  /// Gets a single value from the row by header pattern.
	  /// <para>
	  /// This returns the value of the first column where the header matches the specified header pattern.
	  /// If the value is an empty string, then an empty optional is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="headerPattern"> the header pattern to match </param>
	  /// <returns> the trimmed field value, empty </returns>
	  public Optional<string> findValue(Pattern headerPattern)
	  {
		return findField(headerPattern).filter(str => !str.Empty);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a sub-row, containing a selection of fields by index.
	  /// <para>
	  /// All fields after the specified index are included.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startInclusive">  the start index, zero-based, inclusive </param>
	  /// <returns> the sub row </returns>
	  public CsvRow subRow(int startInclusive)
	  {
		return subRow(startInclusive, fields_Renamed.size());
	  }

	  /// <summary>
	  /// Obtains a sub-row, containing a selection of fields by index.
	  /// </summary>
	  /// <param name="startInclusive">  the start index, zero-based, inclusive </param>
	  /// <param name="endExclusive">  the end index, zero-based, exclusive </param>
	  /// <returns> the sub row </returns>
	  public CsvRow subRow(int startInclusive, int endExclusive)
	  {
		return new CsvRow(headers_Renamed.subList(Math.Min(startInclusive, headers_Renamed.size()), Math.Min(endExclusive, headers_Renamed.size())), lineNumber_Renamed, fields_Renamed.subList(startInclusive, endExclusive));
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
		if (obj is CsvRow)
		{
		  CsvRow other = (CsvRow) obj;
		  return headers_Renamed.Equals(other.headers_Renamed) && fields_Renamed.Equals(other.fields_Renamed);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the CSV file.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return headers_Renamed.GetHashCode() ^ fields_Renamed.GetHashCode();
	  }

	  /// <summary>
	  /// Returns a string describing the CSV file.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		return "CsvRow" + fields_Renamed.ToString();
	  }

	}

}