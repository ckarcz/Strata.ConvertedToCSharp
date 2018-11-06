using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader
{


	using CharMatcher = com.google.common.@base.CharMatcher;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Contains utilities for loading market data from input files.
	/// </summary>
	public sealed class LoaderUtils
	{

	  /// <summary>
	  /// Default scheme for trades.
	  /// </summary>
	  public const string DEFAULT_TRADE_SCHEME = "OG-Trade";
	  /// <summary>
	  /// Default scheme for positions.
	  /// </summary>
	  public const string DEFAULT_POSITION_SCHEME = "OG-Position";
	  /// <summary>
	  /// Default scheme for securities.
	  /// </summary>
	  public const string DEFAULT_SECURITY_SCHEME = "OG-Security";

	  // date formats
	  private static readonly DateTimeFormatter D_M_YEAR_SLASH = new DateTimeFormatterBuilder().appendPattern("d/M/").parseLenient().appendValueReduced(ChronoField.YEAR_OF_ERA, 2, 2, 2000).toFormatter(Locale.ENGLISH);
	  private static readonly DateTimeFormatter YYYY_M_D_SLASH = DateTimeFormatter.ofPattern("yyyy/M/d", Locale.ENGLISH);
	  private static readonly DateTimeFormatter YYYY_MM_DD_DASH = DateTimeFormatter.ofPattern("yyyy-MM-dd", Locale.ENGLISH);
	  private static readonly DateTimeFormatter YYYYMMDD = DateTimeFormatter.ofPattern("yyyyMMdd", Locale.ENGLISH);
	  private static readonly DateTimeFormatter D_MMM_YEAR_DASH = new DateTimeFormatterBuilder().parseCaseInsensitive().appendPattern("d-MMM-").parseLenient().appendValueReduced(ChronoField.YEAR_OF_ERA, 2, 2, 2000).toFormatter(Locale.ENGLISH);
	  private static readonly DateTimeFormatter D_MMM_YEAR_NODASH = new DateTimeFormatterBuilder().parseCaseInsensitive().appendPattern("dMMM").parseLenient().appendValueReduced(ChronoField.YEAR_OF_ERA, 2, 2, 2000).toFormatter(Locale.ENGLISH);
	  // year-month formats
	  private static readonly DateTimeFormatter YYYY_MM_DASH = DateTimeFormatter.ofPattern("yyyy-MM", Locale.ENGLISH);
	  private static readonly DateTimeFormatter YYYYMM = DateTimeFormatter.ofPattern("yyyyMM", Locale.ENGLISH);
	  private static readonly DateTimeFormatter MMM_YEAR = new DateTimeFormatterBuilder().parseCaseInsensitive().appendPattern("MMM").optionalStart().appendLiteral('-').optionalEnd().parseLenient().appendValueReduced(ChronoField.YEAR_OF_ERA, 2, 2, 2000).toFormatter(Locale.ENGLISH);
	  // time formats
	  private static readonly DateTimeFormatter HH_MM_SS_COLON = new DateTimeFormatterBuilder().appendValue(HOUR_OF_DAY, 1, 2, SignStyle.NEVER).optionalStart().appendLiteral(':').appendValue(MINUTE_OF_HOUR, 2).optionalStart().appendLiteral(':').appendValue(SECOND_OF_MINUTE, 2).optionalStart().appendFraction(NANO_OF_SECOND, 0, 9, true).toFormatter(Locale.ENGLISH).withResolverStyle(ResolverStyle.STRICT);
	  // match a currency
	  private static readonly CharMatcher CURRENCY_MATCHER = CharMatcher.inRange('A', 'Z');

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Attempts to locate a rate index by reference name.
	  /// <para>
	  /// This utility searches <seealso cref="IborIndex"/>, <seealso cref="OvernightIndex"/>, <seealso cref="FxIndex"/>
	  /// and <seealso cref="PriceIndex"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reference">  the reference name </param>
	  /// <returns> the resolved rate index </returns>
	  public static Index findIndex(string reference)
	  {
		return Index.of(reference);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses a boolean from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// True is parsed as 'TRUE', 'T', 'YES', 'Y'.
	  /// False is parsed as 'FALSE', 'F', 'NO', 'N'.
	  /// Other strings are rejected.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static bool parseBoolean(string str)
	  {
		switch (str.ToUpper(Locale.ENGLISH))
		{
		  case "TRUE":
		  case "T":
		  case "YES":
		  case "Y":
			return true;
		  case "FALSE":
		  case "F":
		  case "NO":
		  case "N":
			return false;
		  default:
			throw new System.ArgumentException("Unknown BuySell value, must be 'True' or 'False' but was '" + str + "'; " + "parser is case insensitive and also accepts 'T', 'Yes', 'Y', 'F', 'No' and 'N'");
		}
	  }

	  /// <summary>
	  /// Parses an integer from the input string.
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="NumberFormatException"> if the string cannot be parsed </exception>
	  public static int parseInteger(string str)
	  {
		try
		{
		  return int.Parse(str);
		}
		catch (System.FormatException ex)
		{
		  System.FormatException nfex = new System.FormatException("Unable to parse integer from '" + str + "'");
		  nfex.initCause(ex);
		  throw nfex;
		}
	  }

	  /// <summary>
	  /// Parses a double from the input string.
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="NumberFormatException"> if the string cannot be parsed </exception>
	  public static double parseDouble(string str)
	  {
		try
		{
		  return (new decimal(str)).doubleValue();
		}
		catch (System.FormatException ex)
		{
		  System.FormatException nfex = new System.FormatException("Unable to parse double from '" + str + "'");
		  nfex.initCause(ex);
		  throw nfex;
		}
	  }

	  /// <summary>
	  /// Parses a double from the input string, converting it from a percentage to a decimal values.
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="NumberFormatException"> if the string cannot be parsed </exception>
	  public static double parseDoublePercent(string str)
	  {
		try
		{
		  return (new decimal(str)).movePointLeft(2).doubleValue();
		}
		catch (System.FormatException ex)
		{
		  System.FormatException nfex = new System.FormatException("Unable to parse percentage from '" + str + "'");
		  nfex.initCause(ex);
		  throw nfex;
		}
	  }

	  /// <summary>
	  /// Parses a date from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// It accepts formats 'yyyy-MM-dd', 'yyyyMMdd', 'yyyy/M/d', 'd/M/yyyy', 'd-MMM-yyyy' or 'dMMMyyyy'.
	  /// Some formats also accept two-digits years (use is not recommended): 'd/M/yy', 'd-MMM-yy' or 'dMMMyy'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static LocalDate parseDate(string str)
	  {
		try
		{
		  // yyyy-MM-dd
		  if (str.Length == 10 && str[4] == '-' && str[7] == '-')
		  {
			return LocalDate.parse(str, YYYY_MM_DD_DASH);
		  }
		  // yyyy/M/d
		  if (str.Length >= 8 && str[4] == '/')
		  {
			return LocalDate.parse(str, YYYY_M_D_SLASH);
		  }
		  // d/M/yy
		  // d/M/yyyy
		  if (str.Length >= 6 && (str[1] == '/' || str[2] == '/'))
		  {
			return LocalDate.parse(str, D_M_YEAR_SLASH);
		  }
		  // d-MMM-yy
		  // d-MMM-yyyy
		  if (str.Length >= 8 && (str[1] == '-' || str[2] == '-'))
		  {
			return LocalDate.parse(str, D_MMM_YEAR_DASH);
		  }
		  // yyyyMMdd
		  if (str.Length == 8 && char.IsDigit(str[2]))
		  {
			return LocalDate.parse(str, YYYYMMDD);
		  }
		  // dMMMyy
		  // dMMMyyyy
		  return LocalDate.parse(str, D_MMM_YEAR_NODASH);

		}
		catch (DateTimeParseException)
		{
		  throw new System.ArgumentException("Unknown date format, must be formatted as 'yyyy-MM-dd', 'yyyyMMdd', 'yyyy/M/d', 'd/M/yyyy', " + "'d-MMM-yyyy', 'dMMMyyyy', 'd/M/yy', 'd-MMM-yy' or 'dMMMyy' but was: " + str);
		}
	  }

	  /// <summary>
	  /// Parses a year-month from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// It accepts formats 'yyyy-MM', 'yyyyMM', 'MMM-yyyy' or 'MMMyyyy'.
	  /// Some formats also accept two-digits years (use is not recommended): 'MMM-yy' or 'MMMyy'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static YearMonth parseYearMonth(string str)
	  {
		try
		{
		  // yyyy-MM
		  if (str.Length == 7 && str[4] == '-')
		  {
			return YearMonth.parse(str, YYYY_MM_DASH);
		  }
		  // MMM-yy
		  // MMM-yyyy
		  // MMMyy
		  // MMMyyyy
		  if (str.Length >= 5 && !char.IsDigit(str[0]))
		  {
			return YearMonth.parse(str, MMM_YEAR);
		  }
		  // d/M/yyyy - handle Excel converting YearMonth to date
		  if (str.Length >= 8 && (str[1] == '/' || str[2] == '/'))
		  {
			LocalDate date = LocalDate.parse(str, D_M_YEAR_SLASH);
			if (date.DayOfMonth == 1)
			{
			  return YearMonth.of(date.Year, date.Month);
			}
			throw new System.ArgumentException("Found Excel-style date but day-of-month was not set to 1:" + str);
		  }
		  // yyyyMM
		  return YearMonth.parse(str, YYYYMM);

		}
		catch (DateTimeParseException)
		{
		  throw new System.ArgumentException("Unknown date format, must be formatted as 'yyyy-MM', 'yyyyMM', " + "'MMM-yyyy', 'MMMyyyy', 'MMM-yy' or 'MMMyy' but was: " + str);
		}
	  }

	  /// <summary>
	  /// Parses time from the input string.
	  /// <para>
	  /// It accepts formats 'HH[:mm[:ss.SSS]]'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static LocalTime parseTime(string str)
	  {
		try
		{
		  return LocalTime.parse(str, HH_MM_SS_COLON);

		}
		catch (DateTimeParseException)
		{
		  throw new System.ArgumentException("Unknown time format, must be formatted as 'HH', 'HH:mm', 'HH:mm:ss' or 'HH:mm:ss.SSS' but was: " + str);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses a period from the input string.
	  /// <para>
	  /// It accepts the same formats as <seealso cref="Period"/>, but the "P" at the start is optional.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static Period parsePeriod(string str)
	  {
		try
		{
		  string prefixed = str.StartsWith("P", StringComparison.Ordinal) ? str : "P" + str;
		  return Period.parse(prefixed);

		}
		catch (DateTimeParseException)
		{
		  throw new System.ArgumentException("Unknown period format: " + str);
		}
	  }

	  /// <summary>
	  /// Parses a tenor from the input string.
	  /// <para>
	  /// A tenor cannot be zero or negative.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static Tenor parseTenor(string str)
	  {
		try
		{
		  return Tenor.parse(str);

		}
		catch (DateTimeParseException)
		{
		  throw new System.ArgumentException("Unknown tenor format: " + str);
		}
	  }

	  /// <summary>
	  /// Tries to parse a tenor from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse, may be null </param>
	  /// <returns> the parsed tenor, empty if unable to parse </returns>
	  public static Optional<Tenor> tryParseTenor(string str)
	  {
		if (!string.ReferenceEquals(str, null) && str.Length > 1)
		{
		  try
		  {
			return Tenor.parse(str);
		  }
		  catch (Exception)
		  {
			// ignore
		  }
		}
		return null;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses currency from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed currency </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static Currency parseCurrency(string str)
	  {
		try
		{
		  return Currency.parse(str);
		}
		catch (Exception)
		{
		  throw new System.ArgumentException("Unknown Currency, must be 3 letter ISO-4217 format but was '" + str + "'");
		}
	  }

	  /// <summary>
	  /// Tries to parse a currency from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse, may be null </param>
	  /// <returns> the parsed currency, empty if unable to parse </returns>
	  public static Optional<Currency> tryParseCurrency(string str)
	  {
		if (!string.ReferenceEquals(str, null) && str.Length == 3 && CURRENCY_MATCHER.matchesAllOf(str))
		{
		  try
		  {
			return Currency.parse(str);
		  }
		  catch (Exception)
		  {
			// ignore
		  }
		}
		return null;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses day count from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// It leniently handles a variety of known variants of day counts.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static DayCount parseDayCount(string str)
	  {
		return DayCount.extendedEnum().findLenient(str).orElseThrow(() => new System.ArgumentException("Unknown DayCount value, must be one of " + DayCount.extendedEnum().lookupAllNormalized().Keys + " but was '" + str + "'"));
	  }

	  /// <summary>
	  /// Parses business day convention from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// It leniently handles a variety of known variants of business day conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static BusinessDayConvention parseBusinessDayConvention(string str)
	  {
		return BusinessDayConvention.extendedEnum().findLenient(str).orElseThrow(() => new System.ArgumentException("Unknown BusinessDayConvention value, must be one of " + BusinessDayConvention.extendedEnum().lookupAllNormalized().Keys + " but was '" + str + "'"));
	  }

	  /// <summary>
	  /// Parses roll convention from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// It leniently handles a variety of known variants of roll conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static RollConvention parseRollConvention(string str)
	  {
		return RollConvention.extendedEnum().findLenient(str).orElseThrow(() => new System.ArgumentException("Unknown RollConvention value, must be one of " + RollConvention.extendedEnum().lookupAllNormalized().Keys + " but was '" + str + "'"));
	  }

	  /// <summary>
	  /// Parses buy/sell from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// Buy is parsed as 'BUY', 'B'.
	  /// Sell is parsed as 'SELL', 'S'.
	  /// Other strings are rejected.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static BuySell parseBuySell(string str)
	  {
		switch (str.ToUpper(Locale.ENGLISH))
		{
		  case "BUY":
		  case "B":
			return BuySell.BUY;
		  case "SELL":
		  case "S":
			return BuySell.SELL;
		  default:
			throw new System.ArgumentException("Unknown BuySell value, must be 'Buy' or 'Sell' but was '" + str + "'; " + "parser is case insensitive and also accepts 'B' and 'S'");
		}
	  }

	  /// <summary>
	  /// Parses pay/receive from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// Pay is parsed as 'PAY', 'P'.
	  /// Receive is parsed as 'RECEIVE', 'REC', 'R'.
	  /// Other strings are rejected.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static PayReceive parsePayReceive(string str)
	  {
		switch (str.ToUpper(Locale.ENGLISH))
		{
		  case "PAY":
		  case "P":
			return PayReceive.PAY;
		  case "RECEIVE":
		  case "REC":
		  case "R":
			return PayReceive.RECEIVE;
		  default:
			throw new System.ArgumentException("Unknown PayReceive value, must be 'Pay' or 'Receive' but was '" + str + "'; " + "parser is case insensitive and also accepts 'P', 'Rec' and 'R'");
		}
	  }

	  /// <summary>
	  /// Parses put/call from the input string.
	  /// <para>
	  /// Parsing is case insensitive.
	  /// Put is parsed as 'PUT', 'P'.
	  /// Call is parsed as 'CALL', 'C'.
	  /// Other strings are rejected.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the parsed value </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static PutCall parsePutCall(string str)
	  {
		switch (str.ToUpper(Locale.ENGLISH))
		{
		  case "PUT":
		  case "P":
			return PutCall.PUT;
		  case "CALL":
		  case "C":
			return PutCall.CALL;
		  default:
			throw new System.ArgumentException("Unknown PutCall value, must be 'Put' or 'Call' but was '" + str + "'; " + "parser is case insensitive and also accepts 'P' and 'C'");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private LoaderUtils()
	  {
	  }

	}

}