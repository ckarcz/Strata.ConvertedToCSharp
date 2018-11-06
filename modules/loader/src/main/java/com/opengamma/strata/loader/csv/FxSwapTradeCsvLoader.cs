/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.FX_RATE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.NOTIONAL_FIELD;


	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;
	using FxSingleTrade = com.opengamma.strata.product.fx.FxSingleTrade;
	using FxSwap = com.opengamma.strata.product.fx.FxSwap;
	using FxSwapTrade = com.opengamma.strata.product.fx.FxSwapTrade;

	/// <summary>
	/// Loads FX swap trades from CSV files.
	/// </summary>
	internal class FxSwapTradeCsvLoader
	{

	  private const string PAYMENT_DATE_FIELD = "Payment Date";
	  private const string FAR_FX_RATE_DATE_FIELD = "Far FX Rate";
	  private const string FAR_PAYMENT_DATE_FIELD = "Far Payment Date";

	  /// <summary>
	  /// Parses the data from a CSV row.
	  /// </summary>
	  /// <param name="row">  the CSV row object </param>
	  /// <param name="info">  the trade info object </param>
	  /// <param name="resolver">  the resolver used to parse additional information. This is not currently used in this method. </param>
	  /// <returns> the parsed trade, as an instance of <seealso cref="FxSingleTrade"/> </returns>
	  internal static FxSwapTrade parse(CsvRow row, TradeInfo info, TradeCsvInfoResolver resolver)
	  {
		FxSwapTrade trade = parseRow(row, info);
		return resolver.completeTrade(row, trade);
	  }

	  // parses the trade
	  private static FxSwapTrade parseRow(CsvRow row, TradeInfo info)
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
	  private static FxSwapTrade parseConvention(CsvRow row, TradeInfo info)
	  {
		CurrencyPair pair = CurrencyPair.parse(row.getValue(CONVENTION_FIELD));
		BuySell buySell = LoaderUtils.parseBuySell(row.getValue(BUY_SELL_FIELD));
		Currency currency = Currency.parse(row.getValue(CURRENCY_FIELD));
		double notional = LoaderUtils.parseDouble(row.getValue(NOTIONAL_FIELD));
		double nearFxRate = LoaderUtils.parseDouble(row.getValue(FX_RATE_FIELD));
		double farFxRate = LoaderUtils.parseDouble(row.getValue(FAR_FX_RATE_DATE_FIELD));
		LocalDate nearPaymentDate = LoaderUtils.parseDate(row.getValue(PAYMENT_DATE_FIELD));
		LocalDate farPaymentDate = LoaderUtils.parseDate(row.getValue(FAR_PAYMENT_DATE_FIELD));
		Optional<BusinessDayAdjustment> paymentAdj = FxSingleTradeCsvLoader.parsePaymentDateAdjustment(row);

		CurrencyAmount amount = CurrencyAmount.of(currency, buySell.normalize(notional));
		FxRate nearRate = FxRate.of(pair, nearFxRate);
		FxRate farRate = FxRate.of(pair, farFxRate);
		FxSwap fx = paymentAdj.map(adj => FxSwap.of(amount, nearRate, nearPaymentDate, farRate, farPaymentDate, adj)).orElseGet(() => FxSwap.of(amount, nearRate, nearPaymentDate, farRate, farPaymentDate));
		return FxSwapTrade.of(info, fx);
	  }

	  // parse full definition
	  private static FxSwapTrade parseFull(CsvRow row, TradeInfo info)
	  {
		FxSingle nearFx = FxSingleTradeCsvLoader.parseFxSingle(row, "");
		FxSingle farFx = FxSingleTradeCsvLoader.parseFxSingle(row, "Far ");
		return FxSwapTrade.of(info, FxSwap.of(nearFx, farFx));
	  }

	}

}