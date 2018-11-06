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
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.CURRENCY_FIELD;
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
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.NOTIONAL_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.START_DATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.TENOR_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.TRADE_DATE_FIELD;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using TermDeposit = com.opengamma.strata.product.deposit.TermDeposit;
	using TermDepositTrade = com.opengamma.strata.product.deposit.TermDepositTrade;
	using TermDepositConvention = com.opengamma.strata.product.deposit.type.TermDepositConvention;

	/// <summary>
	/// Loads TermDeposit trades from CSV files.
	/// </summary>
	internal sealed class TermDepositTradeCsvLoader
	{

	  /// <summary>
	  /// Parses from the CSV row.
	  /// </summary>
	  /// <param name="row">  the CSV row </param>
	  /// <param name="info">  the trade info </param>
	  /// <param name="resolver">  the resolver used to parse additional information </param>
	  /// <returns> the parsed trade </returns>
	  internal static TermDepositTrade parse(CsvRow row, TradeInfo info, TradeCsvInfoResolver resolver)
	  {
		TermDepositTrade trade = parseRow(row, info, resolver);
		return resolver.completeTrade(row, trade);
	  }

	  // parse the row to a trade
	  private static TermDepositTrade parseRow(CsvRow row, TradeInfo info, TradeCsvInfoResolver resolver)
	  {
		BuySell buySell = LoaderUtils.parseBuySell(row.getValue(BUY_SELL_FIELD));
		double notional = LoaderUtils.parseDouble(row.getValue(NOTIONAL_FIELD));
		double fixedRate = LoaderUtils.parseDoublePercent(row.getValue(FIXED_RATE_FIELD));
		Optional<TermDepositConvention> conventionOpt = row.findValue(CONVENTION_FIELD).map(s => TermDepositConvention.of(s));
		Optional<Period> tenorOpt = row.findValue(TENOR_FIELD).map(s => LoaderUtils.parseTenor(s).Period);
		Optional<LocalDate> startDateOpt = row.findValue(START_DATE_FIELD).map(s => LoaderUtils.parseDate(s));
		Optional<LocalDate> endDateOpt = row.findValue(END_DATE_FIELD).map(s => LoaderUtils.parseDate(s));
		Optional<Currency> currencyOpt = row.findValue(CURRENCY_FIELD).map(s => Currency.parse(s));
		Optional<DayCount> dayCountOpt = row.findValue(DAY_COUNT_FIELD).map(s => LoaderUtils.parseDayCount(s));
		BusinessDayConvention dateCnv = row.findValue(DATE_ADJ_CNV_FIELD).map(s => LoaderUtils.parseBusinessDayConvention(s)).orElse(BusinessDayConventions.MODIFIED_FOLLOWING);
		Optional<HolidayCalendarId> dateCalOpt = row.findValue(DATE_ADJ_CAL_FIELD).map(s => HolidayCalendarId.of(s));

		// use convention if available
		if (conventionOpt.Present)
		{
		  if (currencyOpt.Present || dayCountOpt.Present)
		  {
			throw new System.ArgumentException("TermDeposit trade had invalid combination of fields. When '" + CONVENTION_FIELD + "' is present these fields must not be present: " + ImmutableList.of(CURRENCY_FIELD, DAY_COUNT_FIELD));
		  }
		  TermDepositConvention convention = conventionOpt.get();
		  // explicit dates take precedence over relative ones
		  if (startDateOpt.Present && endDateOpt.Present)
		  {
			if (tenorOpt.Present)
			{
			  throw new System.ArgumentException("TermDeposit trade had invalid combination of fields. When these fields are found " + ImmutableList.of(CONVENTION_FIELD, START_DATE_FIELD, END_DATE_FIELD) + " then these fields must not be present " + ImmutableList.of(TENOR_FIELD));
			}
			LocalDate startDate = startDateOpt.get();
			LocalDate endDate = endDateOpt.get();
			TermDepositTrade trade = convention.toTrade(info, startDate, endDate, buySell, notional, fixedRate);
			return adjustTrade(trade, dateCnv, dateCalOpt);
		  }
		  // relative dates
		  if (tenorOpt.Present && info.TradeDate.Present)
		  {
			if (startDateOpt.Present || endDateOpt.Present)
			{
			  throw new System.ArgumentException("TermDeposit trade had invalid combination of fields. When these fields are found " + ImmutableList.of(CONVENTION_FIELD, TENOR_FIELD, TRADE_DATE_FIELD) + " then these fields must not be present " + ImmutableList.of(START_DATE_FIELD, END_DATE_FIELD));
			}
			LocalDate tradeDate = info.TradeDate.get();
			Period periodToStart = tenorOpt.get();
			TermDepositTrade trade = convention.createTrade(tradeDate, periodToStart, buySell, notional, fixedRate, resolver.ReferenceData);
			trade = trade.toBuilder().info(info).build();
			return adjustTrade(trade, dateCnv, dateCalOpt);
		  }

		}
		else if (startDateOpt.Present && endDateOpt.Present && currencyOpt.Present && dayCountOpt.Present)
		{
		  LocalDate startDate = startDateOpt.get();
		  LocalDate endDate = endDateOpt.get();
		  Currency currency = currencyOpt.get();
		  DayCount dayCount = dayCountOpt.get();
		  TermDeposit.Builder builder = TermDeposit.builder().buySell(buySell).currency(currency).notional(notional).startDate(startDate).endDate(endDate).dayCount(dayCount).rate(fixedRate);
		  TermDepositTrade trade = TermDepositTrade.of(info, builder.build());
		  return adjustTrade(trade, dateCnv, dateCalOpt);
		}
		// no match
		throw new System.ArgumentException("TermDeposit trade had invalid combination of fields. These fields are mandatory:" + ImmutableList.of(BUY_SELL_FIELD, NOTIONAL_FIELD, FIXED_RATE_FIELD) + " and one of these combinations is mandatory: " + ImmutableList.of(CONVENTION_FIELD, TRADE_DATE_FIELD, TENOR_FIELD) + " or " + ImmutableList.of(CONVENTION_FIELD, START_DATE_FIELD, END_DATE_FIELD) + " or " + ImmutableList.of(START_DATE_FIELD, END_DATE_FIELD, CURRENCY_FIELD, DAY_COUNT_FIELD));
	  }

	  // adjust trade based on additional fields specified
	  private static TermDepositTrade adjustTrade(TermDepositTrade trade, BusinessDayConvention dateCnv, Optional<HolidayCalendarId> dateCalOpt)
	  {

		if (!dateCalOpt.Present)
		{
		  return trade;
		}
		TermDeposit.Builder builder = trade.Product.toBuilder();
		dateCalOpt.ifPresent(cal => builder.businessDayAdjustment(BusinessDayAdjustment.of(dateCnv, cal)));
		return trade.toBuilder().product(builder.build()).build();
	  }

	  //-------------------------------------------------------------------------
	  // Restricted constructor.
	  private TermDepositTradeCsvLoader()
	  {
	  }

	}

}