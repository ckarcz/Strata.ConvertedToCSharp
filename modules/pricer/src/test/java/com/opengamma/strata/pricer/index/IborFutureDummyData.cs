/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;


	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using IborFuture = com.opengamma.strata.product.index.IborFuture;
	using IborFutureOption = com.opengamma.strata.product.index.IborFutureOption;
	using IborFutureOptionTrade = com.opengamma.strata.product.index.IborFutureOptionTrade;
	using IborFutureTrade = com.opengamma.strata.product.index.IborFutureTrade;
	using FutureOptionPremiumStyle = com.opengamma.strata.product.option.FutureOptionPremiumStyle;

	/// <summary>
	/// Ibor future data.
	/// </summary>
	public class IborFutureDummyData
	{

	  private const double NOTIONAL = 100_000d;
	  private static readonly double ACCRUAL_FACTOR_2M = TENOR_2M.Period.toTotalMonths() / 12.0;
	  private static readonly LocalDate LAST_TRADE_DATE = date(2015, 6, 17);
	  private const int ROUNDING = 4;
	  private static readonly LocalDate TRADE_DATE = date(2015, 2, 17);
	  private const long FUTURE_QUANTITY = 35;
	  private const double FUTURE_INITIAL_PRICE = 1.015;
	  private static readonly SecurityId FUTURE_ID = SecurityId.of("OG-Ticker", "Future");

	  private static readonly LocalDate EXPIRY_DATE = date(2015, 5, 20);
	  private const double STRIKE_PRICE = 1.075;
	  private const double STRIKE_PRICE_2 = 0.99;
	  private const long OPTION_QUANTITY = 65L;
	  private const double OPTION_INITIAL_PRICE = 0.065;
	  private static readonly SecurityId OPTION_ID = SecurityId.of("OG-Ticker", "Option");
	  private static readonly SecurityId OPTION_ID2 = SecurityId.of("OG-Ticker", "Option2");

	  /// <summary>
	  /// An IborFuture.
	  /// </summary>
	  public static readonly IborFuture IBOR_FUTURE = IborFuture.builder().securityId(FUTURE_ID).currency(GBP).notional(NOTIONAL).lastTradeDate(LAST_TRADE_DATE).index(GBP_LIBOR_2M).accrualFactor(ACCRUAL_FACTOR_2M).rounding(Rounding.ofDecimalPlaces(ROUNDING)).build();

	  /// <summary>
	  /// An IborFutureTrade.
	  /// </summary>
	  public static readonly IborFutureTrade IBOR_FUTURE_TRADE = IborFutureTrade.builder().info(TradeInfo.builder().tradeDate(TRADE_DATE).build()).product(IBOR_FUTURE).quantity(FUTURE_QUANTITY).price(FUTURE_INITIAL_PRICE).build();

	  /// <summary>
	  /// An IborFutureOption.
	  /// </summary>
	  public static readonly IborFutureOption IBOR_FUTURE_OPTION = IborFutureOption.builder().securityId(OPTION_ID).putCall(PutCall.CALL).strikePrice(STRIKE_PRICE).expiryDate(EXPIRY_DATE).expiryTime(LocalTime.of(11, 0)).expiryZone(ZoneId.of("Europe/London")).premiumStyle(FutureOptionPremiumStyle.DAILY_MARGIN).underlyingFuture(IBOR_FUTURE).build();

	  /// <summary>
	  /// An IborFutureOption.
	  /// </summary>
	  public static readonly IborFutureOption IBOR_FUTURE_OPTION_2 = IborFutureOption.builder().securityId(OPTION_ID2).putCall(PutCall.CALL).strikePrice(STRIKE_PRICE_2).expiryDate(EXPIRY_DATE).expiryTime(LocalTime.of(11, 0)).expiryZone(ZoneId.of("Europe/London")).premiumStyle(FutureOptionPremiumStyle.DAILY_MARGIN).underlyingFuture(IBOR_FUTURE).build();

	  /// <summary>
	  /// An IborFutureOptionTrade.
	  /// </summary>
	  public static readonly IborFutureOptionTrade IBOR_FUTURE_OPTION_TRADE = IborFutureOptionTrade.builder().info(TradeInfo.builder().tradeDate(TRADE_DATE).build()).product(IBOR_FUTURE_OPTION).quantity(OPTION_QUANTITY).price(OPTION_INITIAL_PRICE).build();

	}

}