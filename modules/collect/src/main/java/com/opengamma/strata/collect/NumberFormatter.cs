/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{

	/// <summary>
	/// Provides the ability to parse and format numbers.
	/// <para>
	/// This exists as an alternative to <seealso cref="NumberFormat"/> and <seealso cref="DecimalFormat"/>
	/// which are not thread-safe.
	/// </para>
	/// <para>
	/// Instances of this class are immutable and thread-safe.
	/// </para>
	/// </summary>
	public sealed class NumberFormatter
	{

	  /// <summary>
	  /// The underlying format.
	  /// </summary>
	  private readonly ThreadLocal<NumberFormat> underlying;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private NumberFormatter(NumberFormat format)
	  {
		ArgChecker.notNull(format, "format");
		format.ParseIntegerOnly = false;
		this.underlying = ThreadLocal.withInitial(() => (NumberFormat) format.clone());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a standard formatter configured by grouping and decimal places.
	  /// <para>
	  /// The formatter will have the specified number of decimal places.
	  /// The integer part will be grouped if the flag is set.
	  /// The decimal part will never be grouped or truncated.
	  /// The implementation uses English locale data, which uses commas as a separator and a decimal point (dot).
	  /// Numbers will be rounded using <seealso cref="RoundingMode#HALF_EVEN"/>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="grouped">  true to group, false to not group </param>
	  /// <param name="decimalPlaces">  the minimum number of decimal places, from 0 to 9 </param>
	  /// <returns> the formatter </returns>
	  /// <exception cref="IllegalArgumentException"> if the decimal places is invalid </exception>
	  public static NumberFormatter of(bool grouped, int decimalPlaces)
	  {
		return of(grouped, decimalPlaces, decimalPlaces);
	  }

	  /// <summary>
	  /// Obtains a standard formatter configured by grouping and decimal places.
	  /// <para>
	  /// The formatter will have the specified number of decimal places.
	  /// The integer part will be grouped if the flag is set.
	  /// The decimal part will never be grouped or truncated.
	  /// The implementation uses English locale data, which uses commas as a separator and a decimal point (dot).
	  /// Numbers will be rounded using <seealso cref="RoundingMode#HALF_EVEN"/>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="grouped">  true to group, false to not group </param>
	  /// <param name="minDecimalPlaces">  the minimum number of decimal places, from 0 to 9 </param>
	  /// <param name="maxDecimalPlaces">  the minimum number of decimal places, from 0 to 9 </param>
	  /// <returns> the formatter </returns>
	  /// <exception cref="IllegalArgumentException"> if the decimal places is invalid </exception>
	  public static NumberFormatter of(bool grouped, int minDecimalPlaces, int maxDecimalPlaces)
	  {
		ArgChecker.inRangeInclusive(minDecimalPlaces, 0, 9, "minDecimalPlaces");
		ArgChecker.inRangeInclusive(maxDecimalPlaces, 0, 9, "maxDecimalPlaces");
		ArgChecker.isTrue(minDecimalPlaces <= maxDecimalPlaces, "Expected minDecimalPlaces <= maxDecimalPlaces");
		return create(grouped, minDecimalPlaces, maxDecimalPlaces);
	  }

	  // creates an instance ignoring the cache
	  private static NumberFormatter create(bool grouped, int minDecimalPlaces, int maxDecimalPlaces)
	  {
		NumberFormat format = NumberFormat.getNumberInstance(Locale.ENGLISH);
		format.GroupingUsed = grouped;
		format.MinimumIntegerDigits = 1;
		format.MinimumFractionDigits = minDecimalPlaces;
		format.MaximumFractionDigits = maxDecimalPlaces;
		return new NumberFormatter(format);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a formatter for decimal percentages configured by grouping and decimal places.
	  /// <para>
	  /// The formatter will have the specified number of decimal places.
	  /// The integer part will be grouped if the flag is set.
	  /// The decimal part will never be grouped or truncated.
	  /// The implementation uses English locale data, which uses commas as a separator and a decimal point (dot).
	  /// The formatter will suffix the output with '%'.
	  /// Numbers will be rounded using <seealso cref="RoundingMode#HALF_EVEN"/>
	  /// </para>
	  /// <para>
	  /// The number passed in must be the decimal representation of the percentage.
	  /// It will be multiplied by 100 before formatting.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="grouped">  true to group, false to not group </param>
	  /// <param name="minDecimalPlaces">  the minimum number of decimal places, from 0 to 9 </param>
	  /// <param name="maxDecimalPlaces">  the minimum number of decimal places, from 0 to 9 </param>
	  /// <returns> the formatter </returns>
	  /// <exception cref="IllegalArgumentException"> if the decimal places is invalid </exception>
	  public static NumberFormatter ofPercentage(bool grouped, int minDecimalPlaces, int maxDecimalPlaces)
	  {
		ArgChecker.inRangeInclusive(minDecimalPlaces, 0, 9, "minDecimalPlaces");
		ArgChecker.inRangeInclusive(maxDecimalPlaces, 0, 9, "maxDecimalPlaces");
		ArgChecker.isTrue(minDecimalPlaces <= maxDecimalPlaces, "Expected minDecimalPlaces <= maxDecimalPlaces");
		NumberFormat format = NumberFormat.getPercentInstance(Locale.ENGLISH);
		format.GroupingUsed = grouped;
		format.MinimumIntegerDigits = 1;
		format.MinimumFractionDigits = minDecimalPlaces;
		format.MaximumFractionDigits = maxDecimalPlaces;
		return new NumberFormatter(format);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a formatter based on a pattern in the specified locale.
	  /// </summary>
	  /// <param name="pattern">  the pattern string to use </param>
	  /// <param name="locale">  the locale to use </param>
	  /// <returns> the formatter </returns>
	  /// <exception cref="IllegalArgumentException"> if the pattern is invalid </exception>
	  /// <seealso cref= DecimalFormat </seealso>
	  public static NumberFormatter ofPattern(string pattern, Locale locale)
	  {
		ArgChecker.notNull(pattern, "pattern");
		ArgChecker.notNull(locale, "locale");
		return new NumberFormatter(new DecimalFormat(pattern, DecimalFormatSymbols.getInstance(locale)));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a number formatter for general-purpose use in the specified locale.
	  /// </summary>
	  /// <param name="locale">  the locale to use </param>
	  /// <returns> the formatter </returns>
	  /// <seealso cref= NumberFormat#getNumberInstance(Locale) </seealso>
	  public static NumberFormatter ofLocalizedNumber(Locale locale)
	  {
		ArgChecker.notNull(locale, "locale");
		return new NumberFormatter(NumberFormat.getInstance(locale));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Formats a {@code double} using this formatter.
	  /// </summary>
	  /// <param name="number">  the number to format </param>
	  /// <returns> the formatted string </returns>
	  public string format(double number)
	  {
		return underlying.get().format(number);
	  }

	  /// <summary>
	  /// Formats a {@code long} using this formatter.
	  /// </summary>
	  /// <param name="number">  the number to format </param>
	  /// <returns> the formatted string </returns>
	  public string format(long number)
	  {
		return underlying.get().format(number);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the specific string, returning a double.
	  /// </summary>
	  /// <param name="text">  the string to parse </param>
	  /// <returns> the parsed number </returns>
	  /// <exception cref="IllegalArgumentException"> if the text cannot be parsed </exception>
	  public double parse(string text)
	  {
		try
		{
		  return underlying.get().parse(text).doubleValue();
		}
		catch (ParseException ex)
		{
		  throw new System.ArgumentException(ex);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a string representation of this formatter.
	  /// </summary>
	  /// <returns> the string </returns>
	  public override string ToString()
	  {
		return underlying.ToString();
	  }

	}

}