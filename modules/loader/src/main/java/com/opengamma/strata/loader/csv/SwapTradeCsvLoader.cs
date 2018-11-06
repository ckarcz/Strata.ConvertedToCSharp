using System.Collections.Generic;

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
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.DIRECTION_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.END_DATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.FIXED_RATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.FX_RATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.NOTIONAL_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.PERIOD_TO_START_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.START_DATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.TENOR_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.TRADE_DATE_FIELD;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using SingleCurrencySwapConvention = com.opengamma.strata.product.swap.type.SingleCurrencySwapConvention;
	using XCcyIborIborSwapConvention = com.opengamma.strata.product.swap.type.XCcyIborIborSwapConvention;

	/// <summary>
	/// Loads Swap trades from CSV files.
	/// </summary>
	internal sealed class SwapTradeCsvLoader
	{

	  // CSV column headers
	  private const string ROLL_CONVENTION_FIELD = "Roll Convention";
	  private const string STUB_CONVENTION_FIELD = "Stub Convention";
	  private const string FIRST_REGULAR_START_DATE_FIELD = "First Regular Start Date";
	  private const string LAST_REGULAR_END_DATE_FIELD = "Last Regular End Date";

	  /// <summary>
	  /// Parses from the CSV row.
	  /// </summary>
	  /// <param name="row">  the CSV row </param>
	  /// <param name="info">  the trade info </param>
	  /// <param name="resolver">  the resolver used to parse additional information </param>
	  /// <returns> the parsed trade </returns>
	  internal static SwapTrade parse(CsvRow row, IList<CsvRow> variableRows, TradeInfo info, TradeCsvInfoResolver resolver)
	  {
		SwapTrade trade = parseRow(row, info, resolver);
		trade = parseVariableNotional(trade, variableRows);
		trade = parseVariableRates(trade, variableRows);
		return resolver.completeTrade(row, trade);
	  }

	  // variable notional
	  private static SwapTrade parseVariableNotional(SwapTrade trade, IList<CsvRow> variableRows)
	  {
		// parse notionals
		ImmutableList.Builder<ValueStep> stepBuilder = ImmutableList.builder();
		foreach (CsvRow row in variableRows)
		{
		  LocalDate date = LoaderUtils.parseDate(row.getValue(START_DATE_FIELD));
		  row.findValue(NOTIONAL_FIELD).map(str => LoaderUtils.parseDouble(str)).ifPresent(notional => stepBuilder.add(ValueStep.of(date, ValueAdjustment.ofReplace(notional))));
		}
		ImmutableList<ValueStep> varNotionals = stepBuilder.build();
		if (varNotionals.Empty)
		{
		  return trade;
		}
		// adjust the trade, inserting the variable notionals
		ImmutableList.Builder<SwapLeg> legBuilder = ImmutableList.builder();
		foreach (SwapLeg swapLeg in trade.Product.Legs)
		{
		  RateCalculationSwapLeg leg = (RateCalculationSwapLeg) swapLeg;
		  NotionalSchedule notionalSchedule = leg.NotionalSchedule.toBuilder().amount(ValueSchedule.of(leg.NotionalSchedule.Amount.InitialValue, varNotionals)).build();
		  legBuilder.add(leg.toBuilder().notionalSchedule(notionalSchedule).build());
		}
		return replaceLegs(trade, legBuilder.build());
	  }

	  // variable fixed rate
	  private static SwapTrade parseVariableRates(SwapTrade trade, IList<CsvRow> variableRows)
	  {
		ImmutableList.Builder<ValueStep> stepBuilder = ImmutableList.builder();
		foreach (CsvRow row in variableRows)
		{
		  LocalDate date = LoaderUtils.parseDate(row.getValue(START_DATE_FIELD));
		  row.findValue(FIXED_RATE_FIELD).map(str => LoaderUtils.parseDoublePercent(str)).ifPresent(fixedRate => stepBuilder.add(ValueStep.of(date, ValueAdjustment.ofReplace(fixedRate))));
		}
		ImmutableList<ValueStep> varRates = stepBuilder.build();
		if (varRates.Empty)
		{
		  return trade;
		}
		// adjust the trade, inserting the variable rates
		ImmutableList.Builder<SwapLeg> legBuilder = ImmutableList.builder();
		foreach (SwapLeg swapLeg in trade.Product.Legs)
		{
		  RateCalculationSwapLeg leg = (RateCalculationSwapLeg) swapLeg;
		  if (leg.Calculation is FixedRateCalculation)
		  {
			FixedRateCalculation baseCalc = (FixedRateCalculation) leg.Calculation;
			FixedRateCalculation calc = baseCalc.toBuilder().rate(ValueSchedule.of(baseCalc.Rate.InitialValue, varRates)).build();
			legBuilder.add(leg.toBuilder().calculation(calc).build());
		  }
		  else
		  {
			legBuilder.add(leg);
		  }
		}
		return replaceLegs(trade, legBuilder.build());
	  }

	  // parse the row to a trade
	  private static SwapTrade parseRow(CsvRow row, TradeInfo info, TradeCsvInfoResolver resolver)
	  {
		Optional<string> conventionOpt = row.findValue(CONVENTION_FIELD);
		if (conventionOpt.Present)
		{
		  return parseWithConvention(row, info, resolver, conventionOpt.get());
		}
		else
		{
		  Optional<string> payReceive = row.findValue("Leg 1 " + DIRECTION_FIELD);
		  if (payReceive.Present)
		  {
			return FullSwapTradeCsvLoader.parse(row, info);
		  }
		  throw new System.ArgumentException("Swap trade had invalid combination of fields. Must include either '" + CONVENTION_FIELD + "' or '" + "Leg 1 " + DIRECTION_FIELD + "'");
		}
	  }

	  // parse a trade based on a convention
	  internal static SwapTrade parseWithConvention(CsvRow row, TradeInfo info, TradeCsvInfoResolver resolver, string conventionStr)
	  {
		BuySell buySell = LoaderUtils.parseBuySell(row.getValue(BUY_SELL_FIELD));
		double notional = LoaderUtils.parseDouble(row.getValue(NOTIONAL_FIELD));
		double fixedRate = LoaderUtils.parseDoublePercent(row.getValue(FIXED_RATE_FIELD));
		Optional<Period> periodToStartOpt = row.findValue(PERIOD_TO_START_FIELD).map(s => LoaderUtils.parsePeriod(s));
		Optional<Tenor> tenorOpt = row.findValue(TENOR_FIELD).map(s => LoaderUtils.parseTenor(s));
		Optional<LocalDate> startDateOpt = row.findValue(START_DATE_FIELD).map(s => LoaderUtils.parseDate(s));
		Optional<LocalDate> endDateOpt = row.findValue(END_DATE_FIELD).map(s => LoaderUtils.parseDate(s));
		Optional<RollConvention> rollCnvOpt = row.findValue(ROLL_CONVENTION_FIELD).map(s => LoaderUtils.parseRollConvention(s));
		Optional<StubConvention> stubCnvOpt = row.findValue(STUB_CONVENTION_FIELD).map(s => StubConvention.of(s));
		Optional<LocalDate> firstRegStartDateOpt = row.findValue(FIRST_REGULAR_START_DATE_FIELD).map(s => LoaderUtils.parseDate(s));
		Optional<LocalDate> lastRegEndDateOpt = row.findValue(LAST_REGULAR_END_DATE_FIELD).map(s => LoaderUtils.parseDate(s));
		BusinessDayConvention dateCnv = row.findValue(DATE_ADJ_CNV_FIELD).map(s => LoaderUtils.parseBusinessDayConvention(s)).orElse(BusinessDayConventions.MODIFIED_FOLLOWING);
		Optional<HolidayCalendarId> dateCalOpt = row.findValue(DATE_ADJ_CAL_FIELD).map(s => HolidayCalendarId.of(s));
		double? fxRateOpt = row.findValue(FX_RATE_FIELD).map(str => LoaderUtils.parseDouble(str));

		// explicit dates take precedence over relative ones
		if (startDateOpt.Present && endDateOpt.Present)
		{
		  if (periodToStartOpt.Present || tenorOpt.Present)
		  {
			throw new System.ArgumentException("Swap trade had invalid combination of fields. When these fields are found " + ImmutableList.of(CONVENTION_FIELD, START_DATE_FIELD, END_DATE_FIELD) + " then these fields must not be present " + ImmutableList.of(PERIOD_TO_START_FIELD, TENOR_FIELD));
		  }
		  LocalDate startDate = startDateOpt.get();
		  LocalDate endDate = endDateOpt.get();
		  SwapTrade trade = createSwap(info, conventionStr, startDate, endDate, buySell, notional, fixedRate, fxRateOpt);
		  return adjustTrade(trade, rollCnvOpt, stubCnvOpt, firstRegStartDateOpt, lastRegEndDateOpt, dateCnv, dateCalOpt);
		}

		// start date + tenor
		if (startDateOpt.Present && tenorOpt.Present)
		{
		  if (periodToStartOpt.Present || endDateOpt.Present)
		  {
			throw new System.ArgumentException("Swap trade had invalid combination of fields. When these fields are found " + ImmutableList.of(CONVENTION_FIELD, START_DATE_FIELD, TENOR_FIELD) + " then these fields must not be present " + ImmutableList.of(PERIOD_TO_START_FIELD, END_DATE_FIELD));
		  }
		  LocalDate startDate = startDateOpt.get();
		  Tenor tenor = tenorOpt.get();
		  LocalDate endDate = startDate.plus(tenor);
		  SwapTrade trade = createSwap(info, conventionStr, startDate, endDate, buySell, notional, fixedRate, fxRateOpt);
		  return adjustTrade(trade, rollCnvOpt, stubCnvOpt, firstRegStartDateOpt, lastRegEndDateOpt, dateCnv, dateCalOpt);
		}

		// relative dates
		if (periodToStartOpt.Present && tenorOpt.Present && info.TradeDate.Present)
		{
		  if (startDateOpt.Present || endDateOpt.Present)
		  {
			throw new System.ArgumentException("Swap trade had invalid combination of fields. When these fields are found " + ImmutableList.of(CONVENTION_FIELD, PERIOD_TO_START_FIELD, TENOR_FIELD, TRADE_DATE_FIELD) + " then these fields must not be present " + ImmutableList.of(START_DATE_FIELD, END_DATE_FIELD));
		  }
		  LocalDate tradeDate = info.TradeDate.get();
		  Period periodToStart = periodToStartOpt.get();
		  Tenor tenor = tenorOpt.get();
		  if (fxRateOpt.HasValue)
		  {
			XCcyIborIborSwapConvention convention = XCcyIborIborSwapConvention.of(conventionStr);
			double notionalFlat = notional * fxRateOpt.Value;
			SwapTrade trade = convention.createTrade(tradeDate, periodToStart, tenor, buySell, notional, notionalFlat, fixedRate, resolver.ReferenceData);
			trade = trade.toBuilder().info(info).build();
			return adjustTrade(trade, rollCnvOpt, stubCnvOpt, firstRegStartDateOpt, lastRegEndDateOpt, dateCnv, dateCalOpt);
		  }
		  else
		  {
			SingleCurrencySwapConvention convention = SingleCurrencySwapConvention.of(conventionStr);
			SwapTrade trade = convention.createTrade(tradeDate, periodToStart, tenor, buySell, notional, fixedRate, resolver.ReferenceData);
			trade = trade.toBuilder().info(info).build();
			return adjustTrade(trade, rollCnvOpt, stubCnvOpt, firstRegStartDateOpt, lastRegEndDateOpt, dateCnv, dateCalOpt);
		  }
		}

		// no match
		throw new System.ArgumentException("Swap trade had invalid combination of fields. These fields are mandatory:" + ImmutableList.of(BUY_SELL_FIELD, NOTIONAL_FIELD, FIXED_RATE_FIELD) + " and one of these combinations is mandatory: " + ImmutableList.of(CONVENTION_FIELD, TRADE_DATE_FIELD, PERIOD_TO_START_FIELD, TENOR_FIELD) + " or " + ImmutableList.of(CONVENTION_FIELD, START_DATE_FIELD, TENOR_FIELD) + " or " + ImmutableList.of(CONVENTION_FIELD, START_DATE_FIELD, END_DATE_FIELD));
	  }

	  // create a swap from known start/end dates
	  private static SwapTrade createSwap(TradeInfo info, string conventionStr, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double fixedRate, double? fxRateOpt)
	  {

		if (fxRateOpt.HasValue)
		{
		  XCcyIborIborSwapConvention convention = XCcyIborIborSwapConvention.of(conventionStr);
		  double notionalFlat = notional * fxRateOpt.Value;
		  return convention.toTrade(info, startDate, endDate, buySell, notional, notionalFlat, fixedRate);
		}
		else
		{
		  SingleCurrencySwapConvention convention = SingleCurrencySwapConvention.of(conventionStr);
		  return convention.toTrade(info, startDate, endDate, buySell, notional, fixedRate);
		}
	  }

	  // adjust trade based on additional fields specified
	  private static SwapTrade adjustTrade(SwapTrade trade, Optional<RollConvention> rollConventionOpt, Optional<StubConvention> stubConventionOpt, Optional<LocalDate> firstRegularStartDateOpt, Optional<LocalDate> lastRegEndDateOpt, BusinessDayConvention dateCnv, Optional<HolidayCalendarId> dateCalOpt)
	  {

		if (!rollConventionOpt.Present && !stubConventionOpt.Present && !firstRegularStartDateOpt.Present && !lastRegEndDateOpt.Present && !dateCalOpt.Present)
		{
		  return trade;
		}
		ImmutableList.Builder<SwapLeg> legBuilder = ImmutableList.builder();
		foreach (SwapLeg leg in trade.Product.Legs)
		{
		  RateCalculationSwapLeg swapLeg = (RateCalculationSwapLeg) leg;
		  PeriodicSchedule.Builder scheduleBuilder = swapLeg.AccrualSchedule.toBuilder();
		  rollConventionOpt.ifPresent(rc => scheduleBuilder.rollConvention(rc));
		  stubConventionOpt.ifPresent(sc => scheduleBuilder.stubConvention(sc));
		  firstRegularStartDateOpt.ifPresent(date => scheduleBuilder.firstRegularStartDate(date));
		  lastRegEndDateOpt.ifPresent(date => scheduleBuilder.lastRegularEndDate(date));
		  dateCalOpt.ifPresent(cal => scheduleBuilder.businessDayAdjustment(BusinessDayAdjustment.of(dateCnv, cal)));
		  legBuilder.add(swapLeg.toBuilder().accrualSchedule(scheduleBuilder.build()).build());
		}
		return replaceLegs(trade, legBuilder.build());
	  }

	  // replace the legs
	  private static SwapTrade replaceLegs(SwapTrade trade, ImmutableList<SwapLeg> legs)
	  {
		return trade.toBuilder().product(trade.Product.toBuilder().legs(legs).build()).build();
	  }

	  //-------------------------------------------------------------------------
	  // Restricted constructor.
	  private SwapTradeCsvLoader()
	  {
	  }

	}

}