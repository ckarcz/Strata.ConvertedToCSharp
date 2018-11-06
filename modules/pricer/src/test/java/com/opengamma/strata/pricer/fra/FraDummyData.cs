/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.SELL;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using Fra = com.opengamma.strata.product.fra.Fra;
	using FraDiscountingMethod = com.opengamma.strata.product.fra.FraDiscountingMethod;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;

	/// <summary>
	/// Basic dummy objects used when the data within is not important.
	/// </summary>
	public class FraDummyData
	{

	  /// <summary>
	  /// The notional.
	  /// </summary>
	  public const double NOTIONAL = 1_000_000d;

	  /// <summary>
	  /// Fra, default discounting method.
	  /// </summary>
	  public static readonly Fra FRA = Fra.builder().buySell(BUY).notional(NOTIONAL).startDate(date(2014, 9, 12)).endDate(date(2014, 12, 12)).index(GBP_LIBOR_3M).fixedRate(0.0125).currency(Currency.GBP).build();

	  /// <summary>
	  /// Fra, AFMA discounting method.
	  /// </summary>
	  public static readonly Fra FRA_AFMA = Fra.builder().buySell(SELL).notional(NOTIONAL).startDate(date(2014, 9, 12)).endDate(date(2014, 12, 12)).index(GBP_LIBOR_3M).fixedRate(0.0125).currency(Currency.GBP).discounting(FraDiscountingMethod.AFMA).build();

	  /// <summary>
	  /// Fra, NONE discounting method.
	  /// </summary>
	  public static readonly Fra FRA_NONE = Fra.builder().buySell(BUY).notional(NOTIONAL).startDate(date(2014, 9, 12)).endDate(date(2014, 12, 12)).index(GBP_LIBOR_3M).fixedRate(0.0125).currency(Currency.GBP).discounting(FraDiscountingMethod.NONE).build();

	  /// <summary>
	  /// Fra trade.
	  /// </summary>
	  public static readonly FraTrade FRA_TRADE = FraTrade.builder().info(TradeInfo.builder().tradeDate(date(2014, 6, 30)).build()).product(FRA).build();

	}

}