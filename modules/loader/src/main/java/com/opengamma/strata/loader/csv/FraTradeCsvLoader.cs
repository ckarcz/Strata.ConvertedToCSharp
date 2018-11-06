/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.BUY_SELL_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.CONVENTION_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.DATE_ADJ_CAL_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.DATE_ADJ_CNV_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.DAY_COUNT_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.END_DATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.FIXED_RATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.INDEX_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.INTERPOLATED_INDEX_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.NOTIONAL_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.PERIOD_TO_START_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.START_DATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.TRADE_DATE_FIELD;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using Fra = com.opengamma.strata.product.fra.Fra;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;
	using FraConvention = com.opengamma.strata.product.fra.type.FraConvention;

	/// <summary>
	/// Loads FRA trades from CSV files.
	/// </summary>
	internal sealed class FraTradeCsvLoader
	{

	  /// <summary>
	  /// Parses from the CSV row.
	  /// </summary>
	  /// <param name="row">  the CSV row </param>
	  /// <param name="info">  the trade info </param>
	  /// <param name="resolver">  the resolver used to parse additional information </param>
	  /// <returns> the parsed trade </returns>
	  internal static FraTrade parse(CsvRow row, TradeInfo info, TradeCsvInfoResolver resolver)
	  {
		FraTrade trade = parseRow(row, info, resolver);
		return resolver.completeTrade(row, trade);
	  }

	  // parse the row to a trade
	  private static FraTrade parseRow(CsvRow row, TradeInfo info, TradeCsvInfoResolver resolver)
	  {
		BuySell buySell = LoaderUtils.parseBuySell(row.getValue(BUY_SELL_FIELD));
		double notional = LoaderUtils.parseDouble(row.getValue(NOTIONAL_FIELD));
		double fixedRate = LoaderUtils.parseDoublePercent(row.getValue(FIXED_RATE_FIELD));
		Optional<FraConvention> conventionOpt = row.findValue(CONVENTION_FIELD).map(s => FraConvention.of(s));
		Optional<Period> periodToStartOpt = row.findValue(PERIOD_TO_START_FIELD).map(s => LoaderUtils.parsePeriod(s));
		Optional<LocalDate> startDateOpt = row.findValue(START_DATE_FIELD).map(s => LoaderUtils.parseDate(s));
		Optional<LocalDate> endDateOpt = row.findValue(END_DATE_FIELD).map(s => LoaderUtils.parseDate(s));
		Optional<IborIndex> indexOpt = row.findValue(INDEX_FIELD).map(s => IborIndex.of(s));
		Optional<IborIndex> interpolatedOpt = row.findValue(INTERPOLATED_INDEX_FIELD).map(s => IborIndex.of(s));
		Optional<DayCount> dayCountOpt = row.findValue(DAY_COUNT_FIELD).map(s => LoaderUtils.parseDayCount(s));
		BusinessDayConvention dateCnv = row.findValue(DATE_ADJ_CNV_FIELD).map(s => LoaderUtils.parseBusinessDayConvention(s)).orElse(BusinessDayConventions.MODIFIED_FOLLOWING);
		Optional<HolidayCalendarId> dateCalOpt = row.findValue(DATE_ADJ_CAL_FIELD).map(s => HolidayCalendarId.of(s));
		// not parsing paymentDate, fixingDateOffset, discounting

		// use convention if available
		if (conventionOpt.Present)
		{
		  if (indexOpt.Present || interpolatedOpt.Present || dayCountOpt.Present)
		  {
			throw new System.ArgumentException("Fra trade had invalid combination of fields. When '" + CONVENTION_FIELD + "' is present these fields must not be present: " + ImmutableList.of(INDEX_FIELD, INTERPOLATED_INDEX_FIELD, DAY_COUNT_FIELD));
		  }
		  FraConvention convention = conventionOpt.get();
		  // explicit dates take precedence over relative ones
		  if (startDateOpt.Present && endDateOpt.Present)
		  {
			if (periodToStartOpt.Present)
			{
			  throw new System.ArgumentException("Fra trade had invalid combination of fields. When these fields are found " + ImmutableList.of(CONVENTION_FIELD, START_DATE_FIELD, END_DATE_FIELD) + " then these fields must not be present " + ImmutableList.of(PERIOD_TO_START_FIELD));
			}
			LocalDate startDate = startDateOpt.get();
			LocalDate endDate = endDateOpt.get();
			// NOTE: payment date assumed to be the start date
			FraTrade trade = convention.toTrade(info, startDate, endDate, startDate, buySell, notional, fixedRate);
			return adjustTrade(trade, dateCnv, dateCalOpt);
		  }
		  // relative dates
		  if (periodToStartOpt.Present && info.TradeDate.Present)
		  {
			if (startDateOpt.Present || endDateOpt.Present)
			{
			  throw new System.ArgumentException("Fra trade had invalid combination of fields. When these fields are found " + ImmutableList.of(CONVENTION_FIELD, PERIOD_TO_START_FIELD, TRADE_DATE_FIELD) + " then these fields must not be present " + ImmutableList.of(START_DATE_FIELD, END_DATE_FIELD));
			}
			LocalDate tradeDate = info.TradeDate.get();
			Period periodToStart = periodToStartOpt.get();
			FraTrade trade = convention.createTrade(tradeDate, periodToStart, buySell, notional, fixedRate, resolver.ReferenceData);
			trade = trade.toBuilder().info(info).build();
			return adjustTrade(trade, dateCnv, dateCalOpt);
		  }

		}
		else if (startDateOpt.Present && endDateOpt.Present && indexOpt.Present)
		{
		  LocalDate startDate = startDateOpt.get();
		  LocalDate endDate = endDateOpt.get();
		  IborIndex index = indexOpt.get();
		  Fra.Builder builder = Fra.builder().buySell(buySell).notional(notional).startDate(startDate).endDate(endDate).fixedRate(fixedRate).index(index);
		  interpolatedOpt.ifPresent(interpolated => builder.indexInterpolated(interpolated));
		  dayCountOpt.ifPresent(dayCount => builder.dayCount(dayCount));
		  return adjustTrade(FraTrade.of(info, builder.build()), dateCnv, dateCalOpt);
		}
		// no match
		throw new System.ArgumentException("Fra trade had invalid combination of fields. These fields are mandatory:" + ImmutableList.of(BUY_SELL_FIELD, NOTIONAL_FIELD, FIXED_RATE_FIELD) + " and one of these combinations is mandatory: " + ImmutableList.of(CONVENTION_FIELD, TRADE_DATE_FIELD, PERIOD_TO_START_FIELD) + " or " + ImmutableList.of(CONVENTION_FIELD, START_DATE_FIELD, END_DATE_FIELD) + " or " + ImmutableList.of(START_DATE_FIELD, END_DATE_FIELD, INDEX_FIELD));
	  }

	  // adjust trade based on additional fields specified
	  private static FraTrade adjustTrade(FraTrade trade, BusinessDayConvention dateCnv, Optional<HolidayCalendarId> dateCalOpt)
	  {

		if (!dateCalOpt.Present)
		{
		  return trade;
		}
		Fra.Builder builder = trade.Product.toBuilder();
		dateCalOpt.ifPresent(cal => builder.businessDayAdjustment(BusinessDayAdjustment.of(dateCnv, cal)));
		return trade.toBuilder().product(builder.build()).build();
	  }

	  //-------------------------------------------------------------------------
	  // Restricted constructor.
	  private FraTradeCsvLoader()
	  {
	  }

	}

}