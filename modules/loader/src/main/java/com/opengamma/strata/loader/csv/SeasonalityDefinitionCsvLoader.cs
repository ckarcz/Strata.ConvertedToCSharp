using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CharSource = com.google.common.io.CharSource;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CsvFile = com.opengamma.strata.collect.io.CsvFile;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using UnicodeBom = com.opengamma.strata.collect.io.UnicodeBom;
	using ShiftType = com.opengamma.strata.market.ShiftType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using SeasonalityDefinition = com.opengamma.strata.market.curve.SeasonalityDefinition;

	/// <summary>
	/// Loads a set of seasonality definitions into memory by reading from CSV resources.
	/// <para>
	/// The CSV file has the following header row:<br />
	/// {@code Curve Name,Shift Type,Jan-Feb,Feb-Mar,Mar-Apr,Apr-May,May-Jun,Jun-Jul,Jul-Aug,Aug-Sep,Sep-Oct,Oct-Nov,Nov-Dec,Dec-Jan}.
	/// 
	/// <ul>
	///   <li>The 'Curve Name' column is the name of the curve.
	///   <li>The 'Shift Type' column is the type of the shift, "Scaled" (multiplicative) or "Absolute" (additive).
	///   <li>The 'Jan-Feb' and similar columns are the seasonality values month-on-month.
	/// </ul>
	/// </para>
	/// <para>
	/// CSV files sometimes contain a Unicode Byte Order Mark.
	/// Callers are responsible for handling this, such as by using <seealso cref="UnicodeBom"/>.
	/// </para>
	/// </summary>
	public sealed class SeasonalityDefinitionCsvLoader
	{

	  // Column headers
	  private const string CURVE_NAME = "Curve Name";
	  private const string SHIFT_TYPE = "Shift Type";
	  private static readonly ImmutableList<string> MONTH_PAIRS = ImmutableList.of("Jan-Feb", "Feb-Mar", "Mar-Apr", "Apr-May", "May-Jun", "Jun-Jul", "Jul-Aug", "Aug-Sep", "Sep-Oct", "Oct-Nov", "Nov-Dec", "Dec-Jan");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads the seasonality definition CSV file.
	  /// </summary>
	  /// <param name="resource">  the seasonality CSV resource </param>
	  /// <returns> the map of seasonality definitions </returns>
	  public static IDictionary<CurveName, SeasonalityDefinition> loadSeasonalityDefinitions(ResourceLocator resource)
	  {
		return parseSeasonalityDefinitions(resource.CharSource);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the seasonality definition CSV file.
	  /// </summary>
	  /// <param name="charSource">  the seasonality CSV character source </param>
	  /// <returns> the map of seasonality definitions </returns>
	  public static IDictionary<CurveName, SeasonalityDefinition> parseSeasonalityDefinitions(CharSource charSource)
	  {
		ImmutableMap.Builder<CurveName, SeasonalityDefinition> builder = ImmutableMap.builder();
		CsvFile csv = CsvFile.of(charSource, true);
		foreach (CsvRow row in csv.rows())
		{
		  string curveNameStr = row.getField(CURVE_NAME);
		  string shiftTypeStr = row.getField(SHIFT_TYPE);
		  DoubleArray values = DoubleArray.of(12, i => double.Parse(row.getField(MONTH_PAIRS.get(i))));

		  CurveName curveName = CurveName.of(curveNameStr);
		  ShiftType shiftType = ShiftType.valueOf(shiftTypeStr.ToUpper(Locale.ENGLISH));
		  builder.put(curveName, SeasonalityDefinition.of(values, shiftType));
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // this class only has static methods
	  private SeasonalityDefinitionCsvLoader()
	  {
	  }

	}

}