/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;

	/// <summary>
	/// Market standard FX swap conventions.
	/// </summary>
	public sealed class StandardFxSwapConventions
	{

	  // Join calendar with the main currencies
	  private static readonly HolidayCalendarId EUTA_USNY = EUTA.combinedWith(USNY);
	  private static readonly HolidayCalendarId GBLO_EUTA = GBLO.combinedWith(EUTA);
	  private static readonly HolidayCalendarId GBLO_USNY = GBLO.combinedWith(USNY);
	  private static readonly HolidayCalendarId GBLO_JPTO = GBLO.combinedWith(JPTO);

	  /// <summary>
	  /// EUR/USD convention with 2 days spot date.
	  /// </summary>
	  public static readonly FxSwapConvention EUR_USD = ImmutableFxSwapConvention.of(CurrencyPair.of(EUR, USD), DaysAdjustment.ofBusinessDays(2, EUTA_USNY), BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, EUTA_USNY));

	  /// <summary>
	  /// EUR/GBP convention with 2 days spot date.
	  /// </summary>
	  public static readonly FxSwapConvention EUR_GBP = ImmutableFxSwapConvention.of(CurrencyPair.of(EUR, GBP), DaysAdjustment.ofBusinessDays(2, GBLO_EUTA), BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, GBLO_EUTA));

	  /// <summary>
	  /// GBP/USD convention with 2 days spot date.
	  /// </summary>
	  public static readonly FxSwapConvention GBP_USD = ImmutableFxSwapConvention.of(CurrencyPair.of(GBP, USD), DaysAdjustment.ofBusinessDays(2, GBLO_USNY), BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, GBLO_USNY));

	  /// <summary>
	  /// GBP/JPY convention with 2 days spot date.
	  /// </summary>
	  public static readonly FxSwapConvention GBP_JPY = ImmutableFxSwapConvention.of(CurrencyPair.of(GBP, JPY), DaysAdjustment.ofBusinessDays(2, GBLO_JPTO), BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, GBLO_JPTO));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardFxSwapConventions()
	  {
	  }

	}

}