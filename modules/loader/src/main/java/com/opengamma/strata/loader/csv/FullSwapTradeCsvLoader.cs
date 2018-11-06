using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.CURRENCY_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.DATE_ADJ_CAL_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.DATE_ADJ_CNV_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.DAY_COUNT_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.DIRECTION_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.END_DATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.FIXED_RATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.INDEX_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.NOTIONAL_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.START_DATE_FIELD;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using Iterables = com.google.common.collect.Iterables;
	using Ints = com.google.common.primitives.Ints;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using FloatingRateIndex = com.opengamma.strata.basics.index.FloatingRateIndex;
	using FloatingRateName = com.opengamma.strata.basics.index.FloatingRateName;
	using FloatingRateType = com.opengamma.strata.basics.index.FloatingRateType;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using FixedRateStubCalculation = com.opengamma.strata.product.swap.FixedRateStubCalculation;
	using FixingRelativeTo = com.opengamma.strata.product.swap.FixingRelativeTo;
	using FxResetCalculation = com.opengamma.strata.product.swap.FxResetCalculation;
	using FxResetFixingRelativeTo = com.opengamma.strata.product.swap.FxResetFixingRelativeTo;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using IborRateResetMethod = com.opengamma.strata.product.swap.IborRateResetMethod;
	using IborRateStubCalculation = com.opengamma.strata.product.swap.IborRateStubCalculation;
	using InflationRateCalculation = com.opengamma.strata.product.swap.InflationRateCalculation;
	using NegativeRateMethod = com.opengamma.strata.product.swap.NegativeRateMethod;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;
	using OvernightRateCalculation = com.opengamma.strata.product.swap.OvernightRateCalculation;
	using PaymentRelativeTo = com.opengamma.strata.product.swap.PaymentRelativeTo;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using PriceIndexCalculationMethod = com.opengamma.strata.product.swap.PriceIndexCalculationMethod;
	using RateCalculation = com.opengamma.strata.product.swap.RateCalculation;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResetSchedule = com.opengamma.strata.product.swap.ResetSchedule;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Loads Swap trades from CSV files.
	/// </summary>
	internal sealed class FullSwapTradeCsvLoader
	{

	  // CSV column headers
	  private const string FREQUENCY_FIELD = "Frequency";
	  private const string START_DATE_CNV_FIELD = "Start Date Convention";
	  private const string START_DATE_CAL_FIELD = "Start Date Calendar";
	  private const string END_DATE_CNV_FIELD = "End Date Convention";
	  private const string END_DATE_CAL_FIELD = "End Date Calendar";
	  private const string ROLL_CONVENTION_FIELD = "Roll Convention";
	  private const string STUB_CONVENTION_FIELD = "Stub Convention";
	  private const string FIRST_REGULAR_START_DATE_FIELD = "First Regular Start Date";
	  private const string LAST_REGULAR_END_DATE_FIELD = "Last Regular End Date";
	  private const string OVERRIDE_START_DATE_FIELD = "Override Start Date";
	  private const string OVERRIDE_START_DATE_CNV_FIELD = "Override Start Date Convention";
	  private const string OVERRIDE_START_DATE_CAL_FIELD = "Override Start Date Calendar";

	  private const string PAYMENT_FREQUENCY_FIELD = "Payment Frequency";
	  private const string PAYMENT_RELATIVE_TO_FIELD = "Payment Relative To";
	  private const string PAYMENT_OFFSET_DAYS_FIELD = "Payment Offset Days";
	  private const string PAYMENT_OFFSET_CAL_FIELD = "Payment Offset Calendar";
	  private const string PAYMENT_OFFSET_ADJ_CNV_FIELD = "Payment Offset Adjustment Convention";
	  private const string PAYMENT_OFFSET_ADJ_CAL_FIELD = "Payment Offset Adjustment Calendar";
	  private const string COMPOUNDING_METHOD_FIELD = "Compounding Method";
	  private const string PAYMENT_FIRST_REGULAR_START_DATE_FIELD = "Payment First Regular Start Date";
	  private const string PAYMENT_LAST_REGULAR_END_DATE_FIELD = "Payment Last Regular End Date";

	  private const string NOTIONAL_CURRENCY_FIELD = "Notional Currency";
	  private const string NOTIONAL_INITIAL_EXCHANGE_FIELD = "Notional Initial Exchange";
	  private const string NOTIONAL_INTERMEDIATE_EXCHANGE_FIELD = "Notional Intermediate Exchange";
	  private const string NOTIONAL_FINAL_EXCHANGE_FIELD = "Notional Final Exchange";
	  private const string FX_RESET_INDEX_FIELD = "FX Reset Index";
	  private const string FX_RESET_RELATIVE_TO_FIELD = "FX Reset Relative To";
	  private const string FX_RESET_OFFSET_DAYS_FIELD = "FX Reset Offset Days";
	  private const string FX_RESET_OFFSET_CAL_FIELD = "FX Reset Offset Calendar";
	  private const string FX_RESET_OFFSET_ADJ_CNV_FIELD = "FX Reset Offset Adjustment Convention";
	  private const string FX_RESET_OFFSET_ADJ_CAL_FIELD = "FX Reset Offset Adjustment Calendar";

	  private const string INITIAL_STUB_RATE_FIELD = "Initial Stub Rate";
	  private const string INITIAL_STUB_AMOUNT_FIELD = "Initial Stub Amount";
	  private const string INITIAL_STUB_INDEX_FIELD = "Initial Stub Index";
	  private const string INITIAL_STUB_INTERPOLATED_INDEX_FIELD = "Initial Stub Interpolated Index";
	  private const string FINAL_STUB_RATE_FIELD = "Final Stub Rate";
	  private const string FINAL_STUB_AMOUNT_FIELD = "Final Stub Amount";
	  private const string FINAL_STUB_INDEX_FIELD = "Final Stub Index";
	  private const string FINAL_STUB_INTERPOLATED_INDEX_FIELD = "Final Stub Interpolated Index";
	  private const string RESET_FREQUENCY_FIELD = "Reset Frequency";
	  private const string RESET_DATE_CNV_FIELD = "Reset Date Convention";
	  private const string RESET_DATE_CAL_FIELD = "Reset Date Calendar";
	  private const string RESET_METHOD_FIELD = "Reset Method";
	  private const string FIXING_RELATIVE_TO_FIELD = "Fixing Relative To";
	  private const string FIXING_OFFSET_DAYS_FIELD = "Fixing Offset Days";
	  private const string FIXING_OFFSET_CAL_FIELD = "Fixing Offset Calendar";
	  private const string FIXING_OFFSET_ADJ_CNV_FIELD = "Fixing Offset Adjustment Convention";
	  private const string FIXING_OFFSET_ADJ_CAL_FIELD = "Fixing Offset Adjustment Calendar";
	  private const string NEGATIVE_RATE_METHOD_FIELD = "Negative Rate Method";
	  private const string FIRST_RATE_FIELD = "First Rate";
	  private const string ACCRUAL_METHOD_FIELD = "Accrual Method";
	  private const string RATE_CUT_OFF_DAYS_FIELD = "Rate Cut Off Days";
	  private const string INFLATION_LAG_FIELD = "Inflation Lag";
	  private const string INFLATION_METHOD_FIELD = "Inflation Method";
	  private const string INFLATION_FIRST_INDEX_VALUE_FIELD = "Inflation First Index Value";

	  private const string GEARING_FIELD = "Gearing";
	  private const string SPREAD_FIELD = "Spread";

	  /// <summary>
	  /// Parses from the CSV row.
	  /// </summary>
	  /// <param name="row">  the CSV row </param>
	  /// <param name="info">  the trade info </param>
	  /// <returns> the parsed trade </returns>
	  internal static SwapTrade parse(CsvRow row, TradeInfo info)
	  {
		// parse any number of legs by looking for 'Leg n Pay Receive'
		// this finds the index for each leg, using null for fixed legs
		IList<FloatingRateIndex> indices = new List<FloatingRateIndex>();
		ISet<DayCount> dayCounts = new LinkedHashSet<DayCount>();
		bool missingDayCount = false;
		string legPrefix = "Leg 1 ";
		Optional<string> payReceiveOpt = getValue(row, legPrefix, DIRECTION_FIELD);
		int i = 1;
		while (payReceiveOpt.Present)
		{
		  // parse this leg, capturing the day count for floating legs
		  FloatingRateIndex index = parseIndex(row, legPrefix);
		  indices.Add(index);
		  if (index != null)
		  {
			dayCounts.Add(index.DefaultFixedLegDayCount);
		  }
		  // defaulting only triggered if a fixed leg actually has a missing day count
		  if (index == null && !findValue(row, legPrefix, DAY_COUNT_FIELD).Present)
		  {
			missingDayCount = true;
		  }
		  // check if there is another leg
		  i++;
		  legPrefix = "Leg " + i + " ";
		  payReceiveOpt = findValue(row, legPrefix, DIRECTION_FIELD);
		}
		// determine the default day count for the fixed leg (only if there is a fixed leg)
		DayCount defaultFixedLegDayCount = null;
		if (missingDayCount)
		{
		  if (dayCounts.Count != 1)
		  {
			throw new System.ArgumentException("Invalid swap definition, day count must be defined on each fixed leg");
		  }
		  defaultFixedLegDayCount = Iterables.getOnlyElement(dayCounts);
		}
		// parse fully now we know the number of legs and the default fixed leg day count
		IList<SwapLeg> legs = parseLegs(row, indices, defaultFixedLegDayCount);
		Swap swap = Swap.of(legs);
		return SwapTrade.of(info, swap);
	  }

	  //-------------------------------------------------------------------------
	  // parse the index and default fixed leg day count
	  private static FloatingRateIndex parseIndex(CsvRow row, string leg)
	  {
		Optional<string> fixedRateOpt = findValue(row, leg, FIXED_RATE_FIELD);
		Optional<string> indexOpt = findValue(row, leg, INDEX_FIELD);
		if (fixedRateOpt.Present)
		{
		  if (indexOpt.Present)
		  {
			throw new System.ArgumentException("Swap leg must not define both '" + leg + FIXED_RATE_FIELD + "' and  '" + leg + INDEX_FIELD + "'");
		  }
		  return null;
		}
		if (!indexOpt.Present)
		{
		  throw new System.ArgumentException("Swap leg must define either '" + leg + FIXED_RATE_FIELD + "' or  '" + leg + INDEX_FIELD + "'");
		}
		// use FloatingRateName to identify Ibor vs other
		string indexStr = indexOpt.get();
		FloatingRateName frn = FloatingRateName.parse(indexStr);
		if (frn.Type == FloatingRateType.IBOR)
		{
		  // re-parse Ibor using tenor, which ensures tenor picked up from indexStr if present
		  Frequency freq = Frequency.parse(getValue(row, leg, FREQUENCY_FIELD));
		  Tenor iborTenor = freq.Term ? frn.DefaultTenor : Tenor.of(freq.Period);
		  return FloatingRateIndex.parse(indexStr, iborTenor);
		}
		return frn.toFloatingRateIndex();
	  }

	  // parses all the legs
	  private static IList<SwapLeg> parseLegs(CsvRow row, IList<FloatingRateIndex> indices, DayCount defaultFixedLegDayCount)
	  {
		IList<SwapLeg> legs = new List<SwapLeg>();
		for (int i = 0; i < indices.Count; i++)
		{
		  string legPrefix = "Leg " + (i + 1) + " ";
		  legs.Add(parseLeg(row, legPrefix, indices[i], defaultFixedLegDayCount));
		}
		return legs;
	  }

	  // parse a single leg
	  private static RateCalculationSwapLeg parseLeg(CsvRow row, string leg, FloatingRateIndex index, DayCount defaultFixedLegDayCount)
	  {

		PayReceive payReceive = LoaderUtils.parsePayReceive(getValue(row, leg, DIRECTION_FIELD));
		PeriodicSchedule accrualSch = parseAccrualSchedule(row, leg);
		PaymentSchedule paymentSch = parsePaymentSchedule(row, leg, accrualSch.Frequency);
		NotionalSchedule notionalSch = parseNotionalSchedule(row, leg);
		RateCalculation calc = parseRateCalculation(row, leg, index, defaultFixedLegDayCount, accrualSch.BusinessDayAdjustment, notionalSch.Currency);

		return RateCalculationSwapLeg.builder().payReceive(payReceive).accrualSchedule(accrualSch).paymentSchedule(paymentSch).notionalSchedule(notionalSch).calculation(calc).build();
	  }

	  //-------------------------------------------------------------------------
	  // accrual schedule
	  private static PeriodicSchedule parseAccrualSchedule(CsvRow row, string leg)
	  {
		PeriodicSchedule.Builder builder = PeriodicSchedule.builder();
		// basics
		builder.startDate(LoaderUtils.parseDate(getValueWithFallback(row, leg, START_DATE_FIELD)));
		builder.endDate(LoaderUtils.parseDate(getValueWithFallback(row, leg, END_DATE_FIELD)));
		builder.frequency(Frequency.parse(getValue(row, leg, FREQUENCY_FIELD)));
		// adjustments
		BusinessDayAdjustment dateAdj = parseBusinessDayAdjustment(row, leg, DATE_ADJ_CNV_FIELD, DATE_ADJ_CAL_FIELD).orElse(BusinessDayAdjustment.NONE);
		Optional<BusinessDayAdjustment> startDateAdj = parseBusinessDayAdjustment(row, leg, START_DATE_CNV_FIELD, START_DATE_CAL_FIELD);
		Optional<BusinessDayAdjustment> endDateAdj = parseBusinessDayAdjustment(row, leg, END_DATE_CNV_FIELD, END_DATE_CAL_FIELD);
		builder.businessDayAdjustment(dateAdj);
		if (startDateAdj.Present && !startDateAdj.get().Equals(dateAdj))
		{
		  builder.startDateBusinessDayAdjustment(startDateAdj.get());
		}
		if (endDateAdj.Present && !endDateAdj.get().Equals(dateAdj))
		{
		  builder.endDateBusinessDayAdjustment(endDateAdj.get());
		}
		// optionals
		builder.stubConvention(findValueWithFallback(row, leg, STUB_CONVENTION_FIELD).map(s => StubConvention.of(s)).orElse(StubConvention.SMART_INITIAL));
		findValue(row, leg, ROLL_CONVENTION_FIELD).map(s => LoaderUtils.parseRollConvention(s)).ifPresent(v => builder.rollConvention(v));
		findValue(row, leg, FIRST_REGULAR_START_DATE_FIELD).map(s => LoaderUtils.parseDate(s)).ifPresent(v => builder.firstRegularStartDate(v));
		findValue(row, leg, LAST_REGULAR_END_DATE_FIELD).map(s => LoaderUtils.parseDate(s)).ifPresent(v => builder.lastRegularEndDate(v));
		parseAdjustableDate(row, leg, OVERRIDE_START_DATE_FIELD, OVERRIDE_START_DATE_CNV_FIELD, OVERRIDE_START_DATE_CAL_FIELD).ifPresent(d => builder.overrideStartDate(d));
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // payment schedule
	  private static PaymentSchedule parsePaymentSchedule(CsvRow row, string leg, Frequency accrualFrequency)
	  {
		PaymentSchedule.Builder builder = PaymentSchedule.builder();
		// basics
		builder.paymentFrequency(findValue(row, leg, PAYMENT_FREQUENCY_FIELD).map(s => Frequency.parse(s)).orElse(accrualFrequency));
		Optional<DaysAdjustment> offsetOpt = parseDaysAdjustment(row, leg, PAYMENT_OFFSET_DAYS_FIELD, PAYMENT_OFFSET_CAL_FIELD, PAYMENT_OFFSET_ADJ_CNV_FIELD, PAYMENT_OFFSET_ADJ_CAL_FIELD);
		builder.paymentDateOffset(offsetOpt.orElse(DaysAdjustment.NONE));
		// optionals
		findValue(row, leg, PAYMENT_RELATIVE_TO_FIELD).map(s => PaymentRelativeTo.of(s)).ifPresent(v => builder.paymentRelativeTo(v));
		findValue(row, leg, COMPOUNDING_METHOD_FIELD).map(s => CompoundingMethod.of(s)).ifPresent(v => builder.compoundingMethod(v));
		findValue(row, leg, PAYMENT_FIRST_REGULAR_START_DATE_FIELD).map(s => LoaderUtils.parseDate(s)).ifPresent(v => builder.firstRegularStartDate(v));
		findValue(row, leg, PAYMENT_LAST_REGULAR_END_DATE_FIELD).map(s => LoaderUtils.parseDate(s)).ifPresent(v => builder.lastRegularEndDate(v));
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // notional schedule
	  private static NotionalSchedule parseNotionalSchedule(CsvRow row, string leg)
	  {
		NotionalSchedule.Builder builder = NotionalSchedule.builder();
		// basics
		Currency currency = Currency.of(getValueWithFallback(row, leg, CURRENCY_FIELD));
		builder.currency(currency);
		builder.amount(ValueSchedule.of(LoaderUtils.parseDouble(getValueWithFallback(row, leg, NOTIONAL_FIELD))));
		// fx reset
		Optional<FxIndex> fxIndexOpt = findValue(row, leg, FX_RESET_INDEX_FIELD).map(s => FxIndex.of(s));
		Optional<Currency> notionalCurrencyOpt = findValue(row, leg, NOTIONAL_CURRENCY_FIELD).map(s => Currency.of(s));
		Optional<FxResetFixingRelativeTo> fxFixingRelativeToOpt = findValue(row, leg, FX_RESET_RELATIVE_TO_FIELD).map(s => FxResetFixingRelativeTo.of(s));
		Optional<DaysAdjustment> fxResetAdjOpt = parseDaysAdjustment(row, leg, FX_RESET_OFFSET_DAYS_FIELD, FX_RESET_OFFSET_CAL_FIELD, FX_RESET_OFFSET_ADJ_CNV_FIELD, FX_RESET_OFFSET_ADJ_CAL_FIELD);
		if (fxIndexOpt.Present)
		{
		  FxIndex fxIndex = fxIndexOpt.get();
		  FxResetCalculation.Builder fxResetBuilder = FxResetCalculation.builder();
		  fxResetBuilder.index(fxIndex);
		  fxResetBuilder.referenceCurrency(notionalCurrencyOpt.orElse(fxIndex.CurrencyPair.other(currency)));
		  fxFixingRelativeToOpt.ifPresent(v => fxResetBuilder.fixingRelativeTo(v));
		  fxResetAdjOpt.ifPresent(v => fxResetBuilder.fixingDateOffset(v));
		  builder.fxReset(fxResetBuilder.build());
		}
		else if (notionalCurrencyOpt.Present || fxFixingRelativeToOpt.Present || fxResetAdjOpt.Present)
		{
		  throw new System.ArgumentException("Swap trade FX Reset must define field '" + leg + FX_RESET_INDEX_FIELD + "'");
		}
		// optionals
		findValue(row, leg, NOTIONAL_INITIAL_EXCHANGE_FIELD).map(s => LoaderUtils.parseBoolean(s)).ifPresent(v => builder.initialExchange(v));
		findValue(row, leg, NOTIONAL_INTERMEDIATE_EXCHANGE_FIELD).map(s => LoaderUtils.parseBoolean(s)).ifPresent(v => builder.intermediateExchange(v));
		findValue(row, leg, NOTIONAL_FINAL_EXCHANGE_FIELD).map(s => LoaderUtils.parseBoolean(s)).ifPresent(v => builder.finalExchange(v));
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // rate calculation
	  private static RateCalculation parseRateCalculation(CsvRow row, string leg, FloatingRateIndex index, DayCount defaultFixedLegDayCount, BusinessDayAdjustment bda, Currency currency)
	  {

		if (index is IborIndex)
		{
		  return parseIborRateCalculation(row, leg, (IborIndex) index, bda, currency);

		}
		else if (index is OvernightIndex)
		{
		  Optional<FloatingRateName> frnOpt = FloatingRateName.extendedEnum().find(getValue(row, leg, INDEX_FIELD));
		  if (frnOpt.Present)
		  {
			FloatingRateName frn = frnOpt.get();
			if (frn.Type == FloatingRateType.OVERNIGHT_AVERAGED)
			{
			  return parseOvernightRateCalculation(row, leg, (OvernightIndex) index, OvernightAccrualMethod.AVERAGED);
			}
		  }
		  return parseOvernightRateCalculation(row, leg, (OvernightIndex) index, OvernightAccrualMethod.COMPOUNDED);

		}
		else if (index is PriceIndex)
		{
		  return parseInflationRateCalculation(row, leg, (PriceIndex) index, currency);

		}
		else
		{
		  return parseFixedRateCalculation(row, leg, currency, defaultFixedLegDayCount);
		}
	  }

	  //-------------------------------------------------------------------------
	  // fixed rate calculation
	  private static RateCalculation parseFixedRateCalculation(CsvRow row, string leg, Currency currency, DayCount defaultFixedLegDayCount)
	  {

		FixedRateCalculation.Builder builder = FixedRateCalculation.builder();
		// basics
		double fixedRate = LoaderUtils.parseDoublePercent(getValue(row, leg, FIXED_RATE_FIELD));
		DayCount dayCount = findValue(row, leg, DAY_COUNT_FIELD).map(s => LoaderUtils.parseDayCount(s)).orElse(defaultFixedLegDayCount);
		if (dayCount == null)
		{
		  throw new System.ArgumentException("Swap leg must define day count using '" + leg + DAY_COUNT_FIELD + "'");
		}
		builder.dayCount(dayCount);
		builder.rate(ValueSchedule.of(fixedRate));
		// initial stub
		double? initialStubRateOpt = findValue(row, leg, INITIAL_STUB_RATE_FIELD).map(s => LoaderUtils.parseDoublePercent(s));
		double? initialStubAmountOpt = findValue(row, leg, INITIAL_STUB_AMOUNT_FIELD).map(s => LoaderUtils.parseDouble(s));
		if (initialStubRateOpt.HasValue && initialStubAmountOpt.HasValue)
		{
		  throw new System.ArgumentException("Swap leg must not define both '" + leg + INITIAL_STUB_RATE_FIELD + "' and  '" + leg + INITIAL_STUB_AMOUNT_FIELD + "'");
		}
		initialStubRateOpt.ifPresent(v => builder.initialStub(FixedRateStubCalculation.ofFixedRate(v)));
		initialStubAmountOpt.ifPresent(v => builder.initialStub(FixedRateStubCalculation.ofKnownAmount(CurrencyAmount.of(currency, v))));
		// final stub
		double? finalStubRateOpt = findValue(row, leg, FINAL_STUB_RATE_FIELD).map(s => LoaderUtils.parseDoublePercent(s));
		double? finalStubAmountOpt = findValue(row, leg, FINAL_STUB_AMOUNT_FIELD).map(s => LoaderUtils.parseDouble(s));
		if (finalStubRateOpt.HasValue && finalStubAmountOpt.HasValue)
		{
		  throw new System.ArgumentException("Swap leg must not define both '" + leg + FINAL_STUB_RATE_FIELD + "' and  '" + leg + FINAL_STUB_AMOUNT_FIELD + "'");
		}
		finalStubRateOpt.ifPresent(v => builder.finalStub(FixedRateStubCalculation.ofFixedRate(v)));
		finalStubAmountOpt.ifPresent(v => builder.finalStub(FixedRateStubCalculation.ofKnownAmount(CurrencyAmount.of(currency, v))));
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // ibor rate calculation
	  private static RateCalculation parseIborRateCalculation(CsvRow row, string leg, IborIndex iborIndex, BusinessDayAdjustment bda, Currency currency)
	  {

		IborRateCalculation.Builder builder = IborRateCalculation.builder();
		// basics
		builder.index(iborIndex);
		// reset
		Optional<Frequency> resetFrequencyOpt = findValue(row, leg, RESET_FREQUENCY_FIELD).map(v => Frequency.parse(v));
		IborRateResetMethod resetMethod = findValue(row, leg, RESET_METHOD_FIELD).map(v => IborRateResetMethod.of(v)).orElse(IborRateResetMethod.WEIGHTED);
		BusinessDayAdjustment resetDateAdj = parseBusinessDayAdjustment(row, leg, RESET_DATE_CNV_FIELD, RESET_DATE_CAL_FIELD).orElse(bda);
		resetFrequencyOpt.ifPresent(freq => builder.resetPeriods(ResetSchedule.builder().resetFrequency(freq).resetMethod(resetMethod).businessDayAdjustment(resetDateAdj).build()));
		// optionals, no ability to set firstFixingDateOffset
		findValue(row, leg, DAY_COUNT_FIELD).map(s => LoaderUtils.parseDayCount(s)).ifPresent(v => builder.dayCount(v));
		findValue(row, leg, FIXING_RELATIVE_TO_FIELD).map(s => FixingRelativeTo.of(s)).ifPresent(v => builder.fixingRelativeTo(v));
		Optional<DaysAdjustment> fixingAdjOpt = parseDaysAdjustment(row, leg, FIXING_OFFSET_DAYS_FIELD, FIXING_OFFSET_CAL_FIELD, FIXING_OFFSET_ADJ_CNV_FIELD, FIXING_OFFSET_ADJ_CAL_FIELD);
		fixingAdjOpt.ifPresent(v => builder.fixingDateOffset(v));
		findValue(row, leg, NEGATIVE_RATE_METHOD_FIELD).map(s => NegativeRateMethod.of(s)).ifPresent(v => builder.negativeRateMethod(v));
		findValue(row, leg, FIRST_RATE_FIELD).map(s => LoaderUtils.parseDoublePercent(s)).ifPresent(v => builder.firstRate(v));
		findValue(row, leg, GEARING_FIELD).map(s => LoaderUtils.parseDouble(s)).ifPresent(v => builder.gearing(ValueSchedule.of(v)));
		findValue(row, leg, SPREAD_FIELD).map(s => LoaderUtils.parseDoublePercent(s)).ifPresent(v => builder.spread(ValueSchedule.of(v)));
		// initial stub
		Optional<IborRateStubCalculation> initialStub = parseIborStub(row, leg, currency, builder, INITIAL_STUB_RATE_FIELD, INITIAL_STUB_AMOUNT_FIELD, INITIAL_STUB_INDEX_FIELD, INITIAL_STUB_INTERPOLATED_INDEX_FIELD);
		initialStub.ifPresent(stub => builder.initialStub(stub));
		// final stub
		Optional<IborRateStubCalculation> finalStub = parseIborStub(row, leg, currency, builder, FINAL_STUB_RATE_FIELD, FINAL_STUB_AMOUNT_FIELD, FINAL_STUB_INDEX_FIELD, FINAL_STUB_INTERPOLATED_INDEX_FIELD);
		finalStub.ifPresent(stub => builder.finalStub(stub));
		return builder.build();
	  }

	  // an Ibor stub
	  private static Optional<IborRateStubCalculation> parseIborStub(CsvRow row, string leg, Currency currency, IborRateCalculation.Builder builder, string rateField, string amountField, string indexField, string interpolatedField)
	  {

		double? stubRateOpt = findValue(row, leg, rateField).map(s => LoaderUtils.parseDoublePercent(s));
		double? stubAmountOpt = findValue(row, leg, amountField).map(s => LoaderUtils.parseDouble(s));
		Optional<IborIndex> stubIndexOpt = findValue(row, leg, indexField).map(s => IborIndex.of(s));
		Optional<IborIndex> stubIndex2Opt = findValue(row, leg, interpolatedField).map(s => IborIndex.of(s));
		if (stubRateOpt.HasValue && !stubAmountOpt.HasValue && !stubIndexOpt.Present && !stubIndex2Opt.Present)
		{
		  return IborRateStubCalculation.ofFixedRate(stubRateOpt.Value);

		}
		else if (!stubRateOpt.HasValue && stubAmountOpt.HasValue && !stubIndexOpt.Present && !stubIndex2Opt.Present)
		{
		  return IborRateStubCalculation.ofKnownAmount(CurrencyAmount.of(currency, stubAmountOpt.Value));

		}
		else if (!stubRateOpt.HasValue && !stubAmountOpt.HasValue && stubIndexOpt.Present)
		{
		  if (stubIndex2Opt.Present)
		  {
			return IborRateStubCalculation.ofIborInterpolatedRate(stubIndexOpt.get(), stubIndex2Opt.get());
		  }
		  else
		  {
			return IborRateStubCalculation.ofIborRate(stubIndexOpt.get());
		  }
		}
		else if (stubRateOpt.HasValue || stubAmountOpt.HasValue || stubIndexOpt.Present || stubIndex2Opt.Present)
		{
		  throw new System.ArgumentException("Swap leg must define only one of the following fields " + ImmutableList.of(leg + rateField, leg + amountField, leg + indexField) + ", and '" + leg + interpolatedField + "' is only allowed with '" + leg + indexField + "'");
		}
		return null;
	  }

	  //-------------------------------------------------------------------------
	  // overnight rate calculation
	  private static RateCalculation parseOvernightRateCalculation(CsvRow row, string leg, OvernightIndex overnightIndex, OvernightAccrualMethod accrualMethod)
	  {

		OvernightRateCalculation.Builder builder = OvernightRateCalculation.builder();
		// basics
		builder.index(overnightIndex);
		builder.accrualMethod(findValue(row, leg, ACCRUAL_METHOD_FIELD).map(s => OvernightAccrualMethod.of(s)).orElse(accrualMethod));
		// optionals
		findValue(row, leg, DAY_COUNT_FIELD).map(s => LoaderUtils.parseDayCount(s)).ifPresent(v => builder.dayCount(v));
		findValue(row, leg, RATE_CUT_OFF_DAYS_FIELD).map(s => Convert.ToInt32(s)).ifPresent(v => builder.rateCutOffDays(v));
		findValue(row, leg, NEGATIVE_RATE_METHOD_FIELD).map(s => NegativeRateMethod.of(s)).ifPresent(v => builder.negativeRateMethod(v));
		findValue(row, leg, GEARING_FIELD).map(s => LoaderUtils.parseDouble(s)).ifPresent(v => builder.gearing(ValueSchedule.of(v)));
		findValue(row, leg, SPREAD_FIELD).map(s => LoaderUtils.parseDoublePercent(s)).ifPresent(v => builder.spread(ValueSchedule.of(v)));
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // inflation rate calculation
	  private static RateCalculation parseInflationRateCalculation(CsvRow row, string leg, PriceIndex priceIndex, Currency currency)
	  {
		InflationRateCalculation.Builder builder = InflationRateCalculation.builder();
		// basics
		builder.index(priceIndex);
		builder.lag(parseInflationLag(findValue(row, leg, INFLATION_LAG_FIELD), currency));
		builder.indexCalculationMethod(parseInflationMethod(findValue(row, leg, INFLATION_METHOD_FIELD), currency));
		// optionals
		findValue(row, leg, INFLATION_FIRST_INDEX_VALUE_FIELD).map(s => LoaderUtils.parseDouble(s)).ifPresent(v => builder.firstIndexValue(v));
		findValue(row, leg, GEARING_FIELD).map(s => LoaderUtils.parseDouble(s)).ifPresent(v => builder.gearing(ValueSchedule.of(v)));
		return builder.build();
	  }

	  // parse inflation lag with convention defaults
	  private static Period parseInflationLag(Optional<string> strOpt, Currency currency)
	  {
		if (!strOpt.Present)
		{
		  if (Currency.GBP.Equals(currency))
		  {
			return Period.ofMonths(2);
		  }
		  return Period.ofMonths(3);
		}
		string str = strOpt.get();
		int? months = Ints.tryParse(str);
		if (months != null)
		{
		  return Period.ofMonths(months);
		}
		return Tenor.parse(str).Period;
	  }

	  // parse inflation method with convention defaults
	  private static PriceIndexCalculationMethod parseInflationMethod(Optional<string> strOpt, Currency currency)
	  {
		if (!strOpt.Present)
		{
		  if (Currency.JPY.Equals(currency))
		  {
			return PriceIndexCalculationMethod.INTERPOLATED_JAPAN;
		  }
		  else if (Currency.USD.Equals(currency))
		  {
			return PriceIndexCalculationMethod.INTERPOLATED;
		  }
		  return PriceIndexCalculationMethod.MONTHLY;
		}
		return PriceIndexCalculationMethod.of(strOpt.get());
	  }

	  //-------------------------------------------------------------------------
	  // days adjustment, defaulting business day convention
	  private static Optional<BusinessDayAdjustment> parseBusinessDayAdjustment(CsvRow row, string leg, string cnvField, string calField)
	  {

		BusinessDayConvention dateCnv = findValue(row, leg, cnvField).map(s => LoaderUtils.parseBusinessDayConvention(s)).orElse(BusinessDayConventions.MODIFIED_FOLLOWING);
		return findValue(row, leg, calField).map(s => HolidayCalendarId.of(s)).map(cal => BusinessDayAdjustment.of(dateCnv, cal));
	  }

	  // days adjustment, defaulting calendar and adjustment
	  private static Optional<DaysAdjustment> parseDaysAdjustment(CsvRow row, string leg, string daysField, string daysCalField, string cnvField, string calField)
	  {

		int? daysOpt = findValue(row, leg, daysField).map(s => Convert.ToInt32(s));
		HolidayCalendarId cal = findValue(row, leg, daysCalField).map(s => HolidayCalendarId.of(s)).orElse(HolidayCalendarIds.NO_HOLIDAYS);
		BusinessDayAdjustment bda = parseBusinessDayAdjustment(row, leg, cnvField, calField).orElse(BusinessDayAdjustment.NONE);
		if (!daysOpt.HasValue)
		{
		  return null;
		}
		return (DaysAdjustment.builder().days(daysOpt.Value).calendar(cal).adjustment(bda).build());
	  }

	  // adjustable date, defaulting business day convention and holiday calendar
	  private static Optional<AdjustableDate> parseAdjustableDate(CsvRow row, string leg, string dateField, string cnvField, string calField)
	  {

		Optional<LocalDate> dateOpt = findValue(row, leg, dateField).map(s => LoaderUtils.parseDate(s));
		if (!dateOpt.Present)
		{
		  return null;
		}
		BusinessDayConvention dateCnv = findValue(row, leg, cnvField).map(s => LoaderUtils.parseBusinessDayConvention(s)).orElse(BusinessDayConventions.MODIFIED_FOLLOWING);
		HolidayCalendarId cal = findValue(row, leg, calField).map(s => HolidayCalendarId.of(s)).orElse(HolidayCalendarIds.NO_HOLIDAYS);
		return AdjustableDate.of(dateOpt.get(), BusinessDayAdjustment.of(dateCnv, cal));
	  }

	  //-------------------------------------------------------------------------
	  // gets value from CSV
	  private static string getValue(CsvRow row, string leg, string field)
	  {
		return findValue(row, leg, field).orElseThrow(() => new System.ArgumentException("Swap leg must define field: '" + leg + field + "'"));
	  }

	  // gets value from CSV
	  private static string getValueWithFallback(CsvRow row, string leg, string field)
	  {
		return findValueWithFallback(row, leg, field).orElseThrow(() => new System.ArgumentException("Swap leg must define field: '" + leg + field + "' or '" + field + "'"));
	  }

	  // finds value from CSV
	  private static Optional<string> findValue(CsvRow row, string leg, string field)
	  {
		return row.findValue(leg + field);
	  }

	  // finds value from CSV
	  private static Optional<string> findValueWithFallback(CsvRow row, string leg, string field)
	  {
		return Guavate.firstNonEmpty(row.findValue(leg + field), row.findValue(field));
	  }

	  //-------------------------------------------------------------------------
	  // Restricted constructor.
	  private FullSwapTradeCsvLoader()
	  {
	  }

	}

}