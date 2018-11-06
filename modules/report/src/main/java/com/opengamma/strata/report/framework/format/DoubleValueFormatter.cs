using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{

	/// <summary>
	/// Formatter for double amounts.
	/// </summary>
	internal sealed class DoubleValueFormatter : ValueFormatter<double>
	{

	  /// <summary>
	  /// The single shared instance of this formatter.
	  /// </summary>
	  internal static readonly DoubleValueFormatter INSTANCE = new DoubleValueFormatter();

	  /// <summary>
	  /// The decimal format.
	  /// </summary>
	  private static readonly DecimalFormat FULL_AMOUNT_FORMAT = new DecimalFormat("#.##########", new DecimalFormatSymbols(Locale.ENGLISH));
	  /// <summary>
	  /// The format cache.
	  /// </summary>
	  private readonly IDictionary<int, DecimalFormat> displayFormatCache = new Dictionary<int, DecimalFormat>();

	  // restricted constructor
	  private DoubleValueFormatter()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public string formatForCsv(double? amount)
	  {
		return FULL_AMOUNT_FORMAT.format(amount.Value);
	  }

	  public string formatForDisplay(double? @object)
	  {
		return formatForDisplay(@object.Value, 2);
	  }

	  /// <summary>
	  /// Formats a double value for display, using the specified number of decimal places.
	  /// </summary>
	  /// <param name="amount">  the amount </param>
	  /// <param name="decimalPlaces">  the number of decimal places to display </param>
	  /// <returns> the formatted amount </returns>
	  public string formatForDisplay(double amount, int decimalPlaces)
	  {
		DecimalFormat format = getDecimalPlacesFormat(decimalPlaces);
		return format.format(amount);
	  }

	  //-------------------------------------------------------------------------
	  private DecimalFormat getDecimalPlacesFormat(int decimalPlaces)
	  {
		if (!displayFormatCache.ContainsKey(decimalPlaces))
		{
		  DecimalFormat format = new DecimalFormat("#,##0;(#,##0)", new DecimalFormatSymbols(Locale.ENGLISH));
		  format.MinimumFractionDigits = decimalPlaces;
		  format.MaximumFractionDigits = decimalPlaces;
		  displayFormatCache[decimalPlaces] = format;
		  return format;
		}
		return displayFormatCache[decimalPlaces];
	  }

	}

}