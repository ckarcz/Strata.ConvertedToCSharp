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
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.DIRECTION_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.FX_RATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.NOTIONAL_FIELD;


	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Messages = com.opengamma.strata.collect.Messages;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;
	using FxSingleTrade = com.opengamma.strata.product.fx.FxSingleTrade;

	/// <summary>
	/// Loads FX trades (spot or forward) from CSV files.
	/// </summary>
	internal class FxSingleTradeCsvLoader
	{

	  private const string PAYMENT_DATE_FIELD = "Payment Date";
	  private const string PAYMENT_DATE_CNV_FIELD = "Payment Date Convention"; // Optional
	  private const string PAYMENT_DATE_CAL_FIELD = "Payment Date Calendar"; // Optional

	  private static readonly string LEG_1_DIRECTION_FIELD = "Leg 1 " + DIRECTION_FIELD;
	  private static readonly string LEG_1_PAYMENT_DATE_FIELD = "Leg 1 " + PAYMENT_DATE_FIELD;
	  private static readonly string LEG_1_CURRENCY_FIELD = "Leg 1 " + CURRENCY_FIELD;
	  private static readonly string LEG_1_NOTIONAL_FIELD = "Leg 1 " + NOTIONAL_FIELD;

	  private static readonly string LEG_2_DIRECTION_FIELD = "Leg 2 " + DIRECTION_FIELD;
	  private static readonly string LEG_2_PAYMENT_DATE_FIELD = "Leg 2 " + PAYMENT_DATE_FIELD;
	  private static readonly string LEG_2_CURRENCY_FIELD = "Leg 2 " + CURRENCY_FIELD;
	  private static readonly string LEG_2_NOTIONAL_FIELD = "Leg 2 " + NOTIONAL_FIELD;

	  /// <summary>
	  /// Parses the data from a CSV row.
	  /// </summary>
	  /// <param name="row">  the CSV row object </param>
	  /// <param name="info">  the trade info object </param>
	  /// <param name="resolver">  the resolver used to parse additional information. This is not currently used in this method. </param>
	  /// <returns> the parsed trade, as an instance of <seealso cref="FxSingleTrade"/> </returns>
	  internal static FxSingleTrade parse(CsvRow row, TradeInfo info, TradeCsvInfoResolver resolver)
	  {
		FxSingleTrade trade = parseRow(row, info);
		return resolver.completeTrade(row, trade);
	  }

	  // parses the trade
	  private static FxSingleTrade parseRow(CsvRow row, TradeInfo info)
	  {
		if (row.findValue(CONVENTION_FIELD).Present || row.findValue(BUY_SELL_FIELD).Present)
		{
		  return parseConvention(row, info);
		}
		else
		{
		  return parseFull(row, info);
		}
	  }

	  // convention-based
	  // ideally we'd use the trade date plus "period to start" to get the spot/payment date
	  // but we don't have all the data and it gets complicated in places like TRY, RUB and AED
	  private static FxSingleTrade parseConvention(CsvRow row, TradeInfo info)
	  {
		CurrencyPair pair = CurrencyPair.parse(row.getValue(CONVENTION_FIELD));
		BuySell buySell = LoaderUtils.parseBuySell(row.getValue(BUY_SELL_FIELD));
		Currency currency = Currency.parse(row.getValue(CURRENCY_FIELD));
		double notional = LoaderUtils.parseDouble(row.getValue(NOTIONAL_FIELD));
		double fxRate = LoaderUtils.parseDouble(row.getValue(FX_RATE_FIELD));
		LocalDate paymentDate = LoaderUtils.parseDate(row.getValue(PAYMENT_DATE_FIELD));
		Optional<BusinessDayAdjustment> paymentAdj = parsePaymentDateAdjustment(row);

		CurrencyAmount amount = CurrencyAmount.of(currency, buySell.normalize(notional));
		FxSingle fx = paymentAdj.map(adj => FxSingle.of(amount, FxRate.of(pair, fxRate), paymentDate, adj)).orElseGet(() => FxSingle.of(amount, FxRate.of(pair, fxRate), paymentDate));
		return FxSingleTrade.of(info, fx);
	  }

	  // parse full definition
	  private static FxSingleTrade parseFull(CsvRow row, TradeInfo info)
	  {
		FxSingle fx = parseFxSingle(row, "");
		return FxSingleTrade.of(info, fx);
	  }

	  // parse an FxSingle
	  internal static FxSingle parseFxSingle(CsvRow row, string prefix)
	  {
		PayReceive direction1 = LoaderUtils.parsePayReceive(row.getValue(prefix + LEG_1_DIRECTION_FIELD));
		Currency currency1 = Currency.of(row.getValue(prefix + LEG_1_CURRENCY_FIELD));
		double notional1 = LoaderUtils.parseDouble(row.getValue(prefix + LEG_1_NOTIONAL_FIELD));
		LocalDate paymentDate1 = row.findValue(prefix + LEG_1_PAYMENT_DATE_FIELD).map(str => LoaderUtils.parseDate(str)).orElseGet(() => LoaderUtils.parseDate(row.getValue(prefix + PAYMENT_DATE_FIELD)));
		PayReceive direction2 = LoaderUtils.parsePayReceive(row.getValue(prefix + LEG_2_DIRECTION_FIELD));
		Currency currency2 = Currency.of(row.getValue(prefix + LEG_2_CURRENCY_FIELD));
		double notional2 = LoaderUtils.parseDouble(row.getValue(prefix + LEG_2_NOTIONAL_FIELD));
		LocalDate paymentDate2 = row.findValue(prefix + LEG_2_PAYMENT_DATE_FIELD).map(str => LoaderUtils.parseDate(str)).orElseGet(() => LoaderUtils.parseDate(row.getValue(prefix + PAYMENT_DATE_FIELD)));
		Optional<BusinessDayAdjustment> paymentAdj = parsePaymentDateAdjustment(row);
		if (direction1.Equals(direction2))
		{
		  throw new System.ArgumentException(Messages.format("FxSingle legs must not have the same direction: {}, {}", direction1.ToString(), direction2.ToString()));
		}
		Payment payment1 = Payment.of(currency1, direction1.normalize(notional1), paymentDate1);
		Payment payment2 = Payment.of(currency2, direction2.normalize(notional2), paymentDate2);
		return paymentAdj.map(adj => FxSingle.of(payment1, payment2, adj)).orElseGet(() => FxSingle.of(payment1, payment2));
	  }

	  // parses the payment date adjustment, which consists of two linked optional fields
	  internal static Optional<BusinessDayAdjustment> parsePaymentDateAdjustment(CsvRow row)
	  {
		Optional<BusinessDayAdjustment> paymentAdj = null;
		Optional<string> paymentDateCnv = row.findValue(PAYMENT_DATE_CNV_FIELD); // Optional field with Business day adjustment
		if (paymentDateCnv.Present)
		{
		  BusinessDayConvention bdCnv = LoaderUtils.parseBusinessDayConvention(paymentDateCnv.get());
		  if (!bdCnv.Equals(BusinessDayConventions.NO_ADJUST))
		  {
			Optional<string> paymentDateCalOpt = row.findValue(PAYMENT_DATE_CAL_FIELD);
			if (paymentDateCalOpt.Present)
			{
			  paymentAdj = BusinessDayAdjustment.of(LoaderUtils.parseBusinessDayConvention(paymentDateCnv.get()), HolidayCalendarId.of(paymentDateCalOpt.get()));
			}
		  }
		}
		return paymentAdj;
	  }

	}

}