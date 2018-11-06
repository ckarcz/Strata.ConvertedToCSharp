/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;


	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using EtdOptionType = com.opengamma.strata.product.etd.EtdOptionType;
	using EtdSettlementType = com.opengamma.strata.product.etd.EtdSettlementType;
	using EtdType = com.opengamma.strata.product.etd.EtdType;
	using EtdVariant = com.opengamma.strata.product.etd.EtdVariant;

	/// <summary>
	/// CSV information resolver helper.
	/// <para>
	/// This simplifies implementations of <seealso cref="TradeCsvInfoResolver"/> and <seealso cref="PositionCsvInfoResolver"/>.
	/// </para>
	/// </summary>
	public sealed class CsvLoaderUtils
	{

	  /// <summary>
	  /// The column name for the security ID scheme/symbology.
	  /// </summary>
	  public const string SECURITY_ID_SCHEME_FIELD = "Security Id Scheme";
	  /// <summary>
	  /// The column name for the security ID.
	  /// </summary>
	  public const string SECURITY_ID_FIELD = "Security Id";
	  /// <summary>
	  /// The column name for the exchange.
	  /// </summary>
	  public const string EXCHANGE_FIELD = "Exchange";
	  /// <summary>
	  /// The column name for the contract code.
	  /// </summary>
	  public const string CONTRACT_CODE_FIELD = "Contract Code";
	  /// <summary>
	  /// The column name for the long quantity.
	  /// </summary>
	  public const string LONG_QUANTITY_FIELD = "Long Quantity";
	  /// <summary>
	  /// The column name for the short quantity.
	  /// </summary>
	  public const string SHORT_QUANTITY_FIELD = "Short Quantity";
	  /// <summary>
	  /// The column name for the quantity.
	  /// </summary>
	  public const string QUANTITY_FIELD = "Quantity";
	  /// <summary>
	  /// The column name for the price.
	  /// </summary>
	  public const string PRICE_FIELD = "Price";
	  /// <summary>
	  /// The column name for the expiry month/year.
	  /// </summary>
	  public const string EXPIRY_FIELD = "Expiry";
	  /// <summary>
	  /// The column name for the expiry week.
	  /// </summary>
	  public const string EXPIRY_WEEK_FIELD = "Expiry Week";
	  /// <summary>
	  /// The column name for the expiry day.
	  /// </summary>
	  public const string EXPIRY_DAY_FIELD = "Expiry Day";
	  /// <summary>
	  /// The column name for the settlement type.
	  /// </summary>
	  public const string SETTLEMENT_TYPE_FIELD = "Settlement Type";
	  /// <summary>
	  /// The column name for the exercise style.
	  /// </summary>
	  public const string EXERCISE_STYLE_FIELD = "Exercise Style";
	  /// <summary>
	  /// The column name for the option version.
	  /// </summary>
	  public const string VERSION_FIELD = "Version";
	  /// <summary>
	  /// The column name for the put/call flag.
	  /// </summary>
	  public const string PUT_CALL_FIELD = "Put Call";
	  /// <summary>
	  /// The column name for the option strike price.
	  /// </summary>
	  public const string EXERCISE_PRICE_FIELD = "Exercise Price";
	  /// <summary>
	  /// The column name for the underlying expiry month/year.
	  /// </summary>
	  public const string UNDERLYING_EXPIRY_FIELD = "Underlying Expiry";
	  /// <summary>
	  /// The column name for the currency.
	  /// </summary>
	  public const string CURRENCY = "Currency";
	  /// <summary>
	  /// The column name for the tick size.
	  /// </summary>
	  public const string TICK_SIZE = "Tick Size";
	  /// <summary>
	  /// The column name for the tick value.
	  /// </summary>
	  public const string TICK_VALUE = "Tick Value";
	  /// <summary>
	  /// The column name for the contract size.
	  /// </summary>
	  public const string CONTRACT_SIZE = "Contract Size";

	  /// <summary>
	  /// Default version used as an option might not specify a version number.
	  /// </summary>
	  public const int DEFAULT_OPTION_VERSION_NUMBER = 0;
	  /// <summary>
	  /// Lookup settlement by code.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
	  public static readonly ImmutableMap<string, EtdSettlementType> SETTLEMENT_BY_CODE = Stream.of(EtdSettlementType.values()).collect(toImmutableMap(EtdSettlementType::getCode));

	  // Restricted constructor.
	  private CsvLoaderUtils()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the year-month and variant.
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="type">  the ETD type </param>
	  /// <returns> the expiry year-month and variant </returns>
	  /// <exception cref="IllegalArgumentException"> if the row cannot be parsed </exception>
	  public static Pair<YearMonth, EtdVariant> parseEtdVariant(CsvRow row, EtdType type)
	  {
		YearMonth yearMonth = LoaderUtils.parseYearMonth(row.getValue(EXPIRY_FIELD));
		int week = row.findValue(EXPIRY_WEEK_FIELD).map(s => LoaderUtils.parseInteger(s)).orElse(0);
		int day = row.findValue(EXPIRY_DAY_FIELD).map(s => LoaderUtils.parseInteger(s)).orElse(0);
		Optional<EtdSettlementType> settleType = row.findValue(SETTLEMENT_TYPE_FIELD).map(s => parseEtdSettlementType(s));
		Optional<EtdOptionType> optionType = row.findValue(EXERCISE_STYLE_FIELD).map(s => parseEtdOptionType(s));
		// check valid combinations
		if (!settleType.Present)
		{
		  if (day == 0)
		  {
			if (week == 0)
			{
			  return Pair.of(yearMonth, EtdVariant.ofMonthly());
			}
			else
			{
			  return Pair.of(yearMonth, EtdVariant.ofWeekly(week));
			}
		  }
		  else
		  {
			if (week == 0)
			{
			  return Pair.of(yearMonth, EtdVariant.ofDaily(day));
			}
			else
			{
			  throw new System.ArgumentException("ETD date columns conflict, cannot set both expiry day and expiry week");
			}
		  }
		}
		else
		{
		  if (day == 0)
		  {
			throw new System.ArgumentException("ETD date columns conflict, must set expiry day for Flex " + type);
		  }
		  if (week != 0)
		  {
			throw new System.ArgumentException("ETD date columns conflict, cannot set expiry week for Flex " + type);
		  }
		  if (type == EtdType.FUTURE)
		  {
			return Pair.of(yearMonth, EtdVariant.ofFlexFuture(day, settleType.get()));
		  }
		  else
		  {
			if (!optionType.Present)
			{
			  throw new System.ArgumentException("ETD option type not found for Flex Option");
			}
			return Pair.of(yearMonth, EtdVariant.ofFlexOption(day, settleType.get(), optionType.get()));
		  }
		}
	  }

	  /// <summary>
	  /// Parses the ETD settlement type from the short code or full name.
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the settlement type </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static EtdSettlementType parseEtdSettlementType(string str)
	  {
		string upper = str.ToUpper(Locale.ENGLISH);
		EtdSettlementType fromCode = SETTLEMENT_BY_CODE.get(upper);
		return fromCode != null ? fromCode : EtdSettlementType.of(str);
	  }

	  /// <summary>
	  /// Parses the ETD option type from the short code or full name.
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the option type </returns>
	  /// <exception cref="IllegalArgumentException"> if the string cannot be parsed </exception>
	  public static EtdOptionType parseEtdOptionType(string str)
	  {
		switch (str.ToUpper(Locale.ENGLISH))
		{
		  case "AMERICAN":
		  case "A":
			return EtdOptionType.AMERICAN;
		  case "EUROPEAN":
		  case "E":
			return EtdOptionType.EUROPEAN;
		  default:
			throw new System.ArgumentException("Unknown EtdOptionType value, must be 'American' or 'European' but was '" + str + "'; " + "parser is case insensitive and also accepts 'A' and 'E'");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the quantity.
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <returns> the quantity, long first, short second </returns>
	  /// <exception cref="IllegalArgumentException"> if the row cannot be parsed </exception>
	  public static DoublesPair parseQuantity(CsvRow row)
	  {
		double? quantityOpt = row.findValue(QUANTITY_FIELD).map(s => LoaderUtils.parseDouble(s));
		if (quantityOpt.HasValue)
		{
		  double quantity = quantityOpt.Value;
		  return DoublesPair.of(quantity >= 0 ? quantity : 0, quantity >= 0 ? 0 : -quantity);
		}
		double? longQuantityOpt = row.findValue(LONG_QUANTITY_FIELD).map(s => LoaderUtils.parseDouble(s));
		double? shortQuantityOpt = row.findValue(SHORT_QUANTITY_FIELD).map(s => LoaderUtils.parseDouble(s));
		if (!longQuantityOpt.HasValue && !shortQuantityOpt.HasValue)
		{
		  throw new System.ArgumentException(Messages.format("Security must contain a quantity column, either '{}' or '{}' and '{}'", QUANTITY_FIELD, LONG_QUANTITY_FIELD, SHORT_QUANTITY_FIELD));
		}
		double longQuantity = ArgChecker.notNegative(longQuantityOpt.GetValueOrDefault(0d), LONG_QUANTITY_FIELD);
		double shortQuantity = ArgChecker.notNegative(shortQuantityOpt.GetValueOrDefault(0d), SHORT_QUANTITY_FIELD);
		return DoublesPair.of(longQuantity, shortQuantity);
	  }

	}

}