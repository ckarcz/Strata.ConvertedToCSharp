/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CHF;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.CHZU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P12M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;

	/// <summary>
	/// Market standard Fixed-Ibor swap conventions.
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	internal sealed class StandardFixedIborSwapConventions
	{

	  // GBLO+USNY calendar
	  private static readonly HolidayCalendarId GBLO_USNY = GBLO.combinedWith(USNY);
	  // GBLO+CHZU calendar
	  private static readonly HolidayCalendarId GBLO_CHZU = GBLO.combinedWith(CHZU);
	  // GBLO+JPTO calendar
	  private static readonly HolidayCalendarId GBLO_JPTO = GBLO.combinedWith(JPTO);

	  /// <summary>
	  /// USD(NY) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays every 6 months with day count '30U/360'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention USD_FIXED_6M_LIBOR_3M = ImmutableFixedIborSwapConvention.of("USD-FIXED-6M-LIBOR-3M", FixedRateSwapLegConvention.of(USD, THIRTY_U_360, P6M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)), IborRateSwapLegConvention.of(IborIndices.USD_LIBOR_3M));

	  /// <summary>
	  /// USD(London) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays yearly with day count 'Act/360'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention USD_FIXED_1Y_LIBOR_3M = ImmutableFixedIborSwapConvention.of("USD-FIXED-1Y-LIBOR-3M", FixedRateSwapLegConvention.of(USD, ACT_360, P12M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_USNY)), IborRateSwapLegConvention.of(IborIndices.USD_LIBOR_3M));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// EUR(1Y) vanilla fixed vs Euribor 3M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention EUR_FIXED_1Y_EURIBOR_3M = ImmutableFixedIborSwapConvention.of("EUR-FIXED-1Y-EURIBOR-3M", FixedRateSwapLegConvention.of(EUR, THIRTY_U_360, P12M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)), IborRateSwapLegConvention.of(IborIndices.EUR_EURIBOR_3M));

	  /// <summary>
	  /// EUR(>1Y) vanilla fixed vs Euribor 6M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention EUR_FIXED_1Y_EURIBOR_6M = ImmutableFixedIborSwapConvention.of("EUR-FIXED-1Y-EURIBOR-6M", FixedRateSwapLegConvention.of(EUR, THIRTY_U_360, P12M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)), IborRateSwapLegConvention.of(IborIndices.EUR_EURIBOR_6M));

	  /// <summary>
	  /// EUR(1Y) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention EUR_FIXED_1Y_LIBOR_3M = ImmutableFixedIborSwapConvention.of("EUR-FIXED-1Y-LIBOR-3M", FixedRateSwapLegConvention.of(EUR, THIRTY_U_360, P12M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)), IborRateSwapLegConvention.of(IborIndices.EUR_LIBOR_3M));

	  /// <summary>
	  /// EUR(>1Y) vanilla fixed vs LIBOR 6M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention EUR_FIXED_1Y_LIBOR_6M = ImmutableFixedIborSwapConvention.of("EUR-FIXED-1Y-LIBOR-6M", FixedRateSwapLegConvention.of(EUR, THIRTY_U_360, P12M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)), IborRateSwapLegConvention.of(IborIndices.EUR_LIBOR_6M));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// GBP(1Y) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays yearly with day count 'Act/365F'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention GBP_FIXED_1Y_LIBOR_3M = ImmutableFixedIborSwapConvention.of("GBP-FIXED-1Y-LIBOR-3M", FixedRateSwapLegConvention.of(GBP, ACT_365F, P12M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)), IborRateSwapLegConvention.of(IborIndices.GBP_LIBOR_3M));

	  /// <summary>
	  /// GBP(>1Y) vanilla fixed vs LIBOR 6M swap.
	  /// The fixed leg pays every 6 months with day count 'Act/365F'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention GBP_FIXED_6M_LIBOR_6M = ImmutableFixedIborSwapConvention.of("GBP-FIXED-6M-LIBOR-6M", FixedRateSwapLegConvention.of(GBP, ACT_365F, P6M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)), IborRateSwapLegConvention.of(IborIndices.GBP_LIBOR_6M));

	  /// <summary>
	  /// GBP(>1Y) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays every 3 months with day count 'Act/365F'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention GBP_FIXED_3M_LIBOR_3M = ImmutableFixedIborSwapConvention.of("GBP-FIXED-3M-LIBOR-3M", FixedRateSwapLegConvention.of(GBP, ACT_365F, P3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)), IborRateSwapLegConvention.of(IborIndices.GBP_LIBOR_3M));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// CHF(1Y) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention CHF_FIXED_1Y_LIBOR_3M = ImmutableFixedIborSwapConvention.of("CHF-FIXED-1Y-LIBOR-3M", FixedRateSwapLegConvention.of(CHF, THIRTY_U_360, P12M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_CHZU)), IborRateSwapLegConvention.of(IborIndices.CHF_LIBOR_3M));

	  /// <summary>
	  /// CHF(>1Y) vanilla fixed vs LIBOR 6M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention CHF_FIXED_1Y_LIBOR_6M = ImmutableFixedIborSwapConvention.of("CHF-FIXED-1Y-LIBOR-6M", FixedRateSwapLegConvention.of(CHF, THIRTY_U_360, P12M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_CHZU)), IborRateSwapLegConvention.of(IborIndices.CHF_LIBOR_6M));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// JPY(Tibor) vanilla fixed vs Tibor 3M swap.
	  /// The fixed leg pays every 6 months with day count 'Act/365F'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention JPY_FIXED_6M_TIBORJ_3M = ImmutableFixedIborSwapConvention.of("JPY-FIXED-6M-TIBOR-JAPAN-3M", FixedRateSwapLegConvention.of(JPY, ACT_365F, P6M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, JPTO)), IborRateSwapLegConvention.of(IborIndices.JPY_TIBOR_JAPAN_3M));

	  /// <summary>
	  /// JPY(LIBOR) vanilla fixed vs LIBOR 6M swap.
	  /// The fixed leg pays every 6 months with day count 'Act/365F'.
	  /// </summary>
	  public static readonly FixedIborSwapConvention JPY_FIXED_6M_LIBOR_6M = ImmutableFixedIborSwapConvention.of("JPY-FIXED-6M-LIBOR-6M", FixedRateSwapLegConvention.of(JPY, ACT_365F, P6M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO_JPTO)), IborRateSwapLegConvention.of(IborIndices.JPY_LIBOR_6M));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardFixedIborSwapConventions()
	  {
	  }

	}

}