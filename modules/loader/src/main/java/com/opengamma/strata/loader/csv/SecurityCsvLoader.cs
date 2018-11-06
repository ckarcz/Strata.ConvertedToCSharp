/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.CONTRACT_SIZE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.CURRENCY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.EXERCISE_PRICE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.EXPIRY_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.PRICE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.PUT_CALL_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.QUANTITY_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.SECURITY_ID_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.SECURITY_ID_SCHEME_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.TICK_SIZE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.TICK_VALUE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.PositionCsvLoader.DEFAULT_SECURITY_SCHEME;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.TradeCsvLoader.BUY_SELL_FIELD;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using GenericSecurity = com.opengamma.strata.product.GenericSecurity;
	using GenericSecurityPosition = com.opengamma.strata.product.GenericSecurityPosition;
	using GenericSecurityTrade = com.opengamma.strata.product.GenericSecurityTrade;
	using Position = com.opengamma.strata.product.Position;
	using PositionInfo = com.opengamma.strata.product.PositionInfo;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using SecurityInfo = com.opengamma.strata.product.SecurityInfo;
	using SecurityPosition = com.opengamma.strata.product.SecurityPosition;
	using SecurityPriceInfo = com.opengamma.strata.product.SecurityPriceInfo;
	using SecurityQuantityTrade = com.opengamma.strata.product.SecurityQuantityTrade;
	using SecurityTrade = com.opengamma.strata.product.SecurityTrade;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// Loads security trades from CSV files.
	/// </summary>
	internal sealed class SecurityCsvLoader
	{

	  // parses a trade from the CSV row
	  internal static SecurityQuantityTrade parseTrade(CsvRow row, TradeInfo info, TradeCsvInfoResolver resolver)
	  {
		SecurityTrade trade = parseSecurityTrade(row, info, resolver);
		SecurityTrade @base = resolver.completeTrade(row, trade);

		double? tickSizeOpt = row.findValue(TICK_SIZE).map(str => LoaderUtils.parseDouble(str));
		Optional<Currency> currencyOpt = row.findValue(CURRENCY).map(str => Currency.of(str));
		double? tickValueOpt = row.findValue(TICK_VALUE).map(str => LoaderUtils.parseDouble(str));
		double contractSize = row.findValue(CONTRACT_SIZE).map(str => LoaderUtils.parseDouble(str)).orElse(1d);
		if (tickSizeOpt.HasValue && currencyOpt.Present && tickValueOpt.HasValue)
		{
		  SecurityPriceInfo priceInfo = SecurityPriceInfo.of(tickSizeOpt.Value, CurrencyAmount.of(currencyOpt.get(), tickValueOpt.Value), contractSize);
		  GenericSecurity sec = GenericSecurity.of(SecurityInfo.of(@base.SecurityId, priceInfo));
		  return GenericSecurityTrade.of(@base.Info, sec, @base.Quantity, @base.Price);
		}
		return @base;
	  }

	  // parses a SecurityTrade from the CSV row
	  private static SecurityTrade parseSecurityTrade(CsvRow row, TradeInfo info, TradeCsvInfoResolver resolver)
	  {
		string securityIdScheme = row.findValue(SECURITY_ID_SCHEME_FIELD).orElse(DEFAULT_SECURITY_SCHEME);
		string securityIdValue = row.getValue(SECURITY_ID_FIELD);
		SecurityId securityId = SecurityId.of(securityIdScheme, securityIdValue);
		double price = LoaderUtils.parseDouble(row.getValue(PRICE_FIELD));
		double quantity = parseTradeQuantity(row);
		return SecurityTrade.of(info, securityId, quantity, price);
	  }

	  // parses the trade quantity, considering the optional buy/sell field
	  private static double parseTradeQuantity(CsvRow row)
	  {
		double quantity = LoaderUtils.parseDouble(row.getValue(QUANTITY_FIELD));
		Optional<BuySell> buySellOpt = row.findValue(BUY_SELL_FIELD).map(str => LoaderUtils.parseBuySell(str));
		if (buySellOpt.Present)
		{
		  quantity = buySellOpt.get().normalize(quantity);
		}
		return quantity;
	  }

	  //-------------------------------------------------------------------------
	  // parses a position from the CSV row
	  internal static Position parsePosition(CsvRow row, PositionInfo info, PositionCsvInfoResolver resolver)
	  {
		if (row.findValue(EXPIRY_FIELD).Present)
		{
		  // etd
		  if (row.findValue(PUT_CALL_FIELD).Present || row.findValue(EXERCISE_PRICE_FIELD).Present)
		  {
			return resolver.parseEtdOptionPosition(row, info);
		  }
		  else
		  {
			return resolver.parseEtdFuturePosition(row, info);
		  }
		}
		else
		{
		  return parseNonEtdPosition(row, info, resolver);
		}
	  }

	  // parses a SecurityPosition from the CSV row, converting ETD information
	  internal static SecurityPosition parsePositionLightweight(CsvRow row, PositionInfo info, PositionCsvInfoResolver resolver)
	  {
		if (row.findValue(EXPIRY_FIELD).Present)
		{
		  // etd
		  if (row.findValue(PUT_CALL_FIELD).Present || row.findValue(EXERCISE_PRICE_FIELD).Present)
		  {
			return resolver.parseEtdOptionSecurityPosition(row, info);
		  }
		  else
		  {
			return resolver.parseEtdFutureSecurityPosition(row, info);
		  }
		}
		else
		{
		  // simple
		  return parseSecurityPosition(row, info, resolver);
		}
	  }

	  // parses the base SecurityPosition
	  internal static SecurityPosition parseSecurityPosition(CsvRow row, PositionInfo info, PositionCsvInfoResolver resolver)
	  {
		string securityIdScheme = row.findValue(SECURITY_ID_SCHEME_FIELD).orElse(DEFAULT_SECURITY_SCHEME);
		string securityIdValue = row.getValue(SECURITY_ID_FIELD);
		SecurityId securityId = SecurityId.of(securityIdScheme, securityIdValue);
		DoublesPair quantity = CsvLoaderUtils.parseQuantity(row);
		SecurityPosition position = SecurityPosition.ofLongShort(info, securityId, quantity.First, quantity.Second);
		return resolver.completePosition(row, position);
	  }

	  // parses the additional GenericSecurityPosition information
	  internal static Position parseNonEtdPosition(CsvRow row, PositionInfo info, PositionCsvInfoResolver resolver)
	  {
		SecurityPosition @base = parseSecurityPosition(row, info, resolver);
		double? tickSizeOpt = row.findValue(TICK_SIZE).map(str => LoaderUtils.parseDouble(str));
		Optional<Currency> currencyOpt = row.findValue(CURRENCY).map(str => Currency.of(str));
		double? tickValueOpt = row.findValue(TICK_VALUE).map(str => LoaderUtils.parseDouble(str));
		double contractSize = row.findValue(CONTRACT_SIZE).map(str => LoaderUtils.parseDouble(str)).orElse(1d);
		if (tickSizeOpt.HasValue && currencyOpt.Present && tickValueOpt.HasValue)
		{
		  SecurityPriceInfo priceInfo = SecurityPriceInfo.of(tickSizeOpt.Value, CurrencyAmount.of(currencyOpt.get(), tickValueOpt.Value), contractSize);
		  GenericSecurity sec = GenericSecurity.of(SecurityInfo.of(@base.SecurityId, priceInfo));
		  return GenericSecurityPosition.ofLongShort(@base.Info, sec, @base.LongQuantity, @base.ShortQuantity);
		}
		return @base;
	  }

	  //-------------------------------------------------------------------------
	  // Restricted constructor.
	  private SecurityCsvLoader()
	  {
	  }

	}

}