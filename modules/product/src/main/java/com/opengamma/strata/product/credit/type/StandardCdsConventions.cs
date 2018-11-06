/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
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
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;

	/// <summary>
	/// Standardized credit default swap conventions.
	/// </summary>
	internal sealed class StandardCdsConventions
	{

	  private static readonly HolidayCalendarId GBLO_USNY = GBLO.combinedWith(USNY);
	  private static readonly HolidayCalendarId GBLO_USNY_JPTO = JPTO.combinedWith(GBLO_USNY);
	  private static readonly HolidayCalendarId GBLO_EUTA = GBLO.combinedWith(EUTA);

	  /// <summary>
	  /// USD-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'USNY'.
	  /// </para>
	  /// </summary>
	  public static readonly ImmutableCdsConvention USD_STANDARD = ImmutableCdsConvention.of("USD-STANDARD", USD, ACT_360, P3M, BusinessDayAdjustment.of(FOLLOWING, USNY), DaysAdjustment.ofBusinessDays(3, USNY));

	  /// <summary>
	  /// EUR-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'EUTA'.
	  /// </para>
	  /// </summary>
	  public static readonly ImmutableCdsConvention EUR_STANDARD = ImmutableCdsConvention.of("EUR-STANDARD", EUR, ACT_360, P3M, BusinessDayAdjustment.of(FOLLOWING, EUTA), DaysAdjustment.ofBusinessDays(3, EUTA));

	  /// <summary>
	  /// EUR-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'EUTA' and 'GBLO'.
	  /// </para>
	  /// </summary>
	  public static readonly ImmutableCdsConvention EUR_GB_STANDARD = ImmutableCdsConvention.of("EUR-GB-STANDARD", EUR, ACT_360, P3M, BusinessDayAdjustment.of(FOLLOWING, GBLO_EUTA), DaysAdjustment.ofBusinessDays(3, GBLO_EUTA));

	  /// <summary>
	  /// GBP-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'GBLO'.
	  /// </para>
	  /// </summary>
	  public static readonly ImmutableCdsConvention GBP_STANDARD = ImmutableCdsConvention.of("GBP-STANDARD", GBP, ACT_360, P3M, BusinessDayAdjustment.of(FOLLOWING, GBLO), DaysAdjustment.ofBusinessDays(3, GBLO));

	  /// <summary>
	  /// GBP-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'GBLO' and 'USNY'.
	  /// </para>
	  /// </summary>
	  public static readonly ImmutableCdsConvention GBP_US_STANDARD = ImmutableCdsConvention.of("GBP-US-STANDARD", GBP, ACT_360, P3M, BusinessDayAdjustment.of(FOLLOWING, GBLO_USNY), DaysAdjustment.ofBusinessDays(3, GBLO_USNY));

	  /// <summary>
	  /// JPY-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'JPTO'.
	  /// </para>
	  /// </summary>
	  public static readonly ImmutableCdsConvention JPY_STANDARD = ImmutableCdsConvention.of("JPY-STANDARD", JPY, ACT_360, P3M, BusinessDayAdjustment.of(FOLLOWING, JPTO), DaysAdjustment.ofBusinessDays(3, JPTO));

	  /// <summary>
	  /// JPY-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'JPTO', 'USNY' and 'GBLO'.
	  /// </para>
	  /// </summary>
	  public static readonly ImmutableCdsConvention JPY_US_GB_STANDARD = ImmutableCdsConvention.of("JPY-US-GB-STANDARD", JPY, ACT_360, P3M, BusinessDayAdjustment.of(FOLLOWING, GBLO_USNY_JPTO), DaysAdjustment.ofBusinessDays(3, GBLO_USNY_JPTO));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardCdsConventions()
	  {
	  }

	}

}