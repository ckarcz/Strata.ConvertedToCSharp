/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.EUR_EONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.JPY_TONAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_SOFR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P12M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.TERM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.OvernightAccrualMethod.COMPOUNDED;

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;

	/// <summary>
	/// Market standard Fixed-Overnight swap conventions.
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	internal sealed class StandardFixedOvernightSwapConventions
	{

	  /// <summary>
	  /// USD fixed vs Fed Fund OIS swap for terms less than or equal to one year.
	  /// <para>
	  /// Both legs pay once at the end and use day count 'Act/360'.
	  /// The spot date offset is 2 days and the payment date offset is 2 days.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention USD_FIXED_TERM_FED_FUND_OIS = makeConvention("USD-FIXED-TERM-FED-FUND-OIS", USD_FED_FUND, ACT_360, TERM, 2, 2);

	  /// <summary>
	  /// USD fixed vs Fed Fund OIS swap for terms greater than one year.
	  /// <para>
	  /// Both legs pay annually and use day count 'Act/360'.
	  /// The spot date offset is 2 days and the payment date offset is 2 days.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention USD_FIXED_1Y_FED_FUND_OIS = makeConvention("USD-FIXED-1Y-FED-FUND-OIS", USD_FED_FUND, ACT_360, P12M, 2, 2);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// USD fixed vs SOFR OIS swap for terms greater than one year.
	  /// <para>
	  /// Both legs pay annually and use day count 'Act/360'.
	  /// The spot date offset is 2 days and the payment date offset is 2 days.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention USD_FIXED_1Y_SOFR_OIS = makeConvention("USD-FIXED-1Y-SOFR-OIS", USD_SOFR, ACT_360, P12M, 2, 2);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// EUR fixed vs EONIA OIS swap for terms less than or equal to one year.
	  /// <para>
	  /// Both legs pay once at the end and use day count 'Act/360'.
	  /// The spot date offset is 2 days and the payment date offset is 1 day.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention EUR_FIXED_TERM_EONIA_OIS = makeConvention("EUR-FIXED-TERM-EONIA-OIS", EUR_EONIA, ACT_360, TERM, 1, 2);

	  /// <summary>
	  /// EUR fixed vs EONIA OIS swap for terms greater than one year.
	  /// <para>
	  /// Both legs pay annually and use day count 'Act/360'.
	  /// The spot date offset is 2 days and the payment date offset is 1 day.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention EUR_FIXED_1Y_EONIA_OIS = makeConvention("EUR-FIXED-1Y-EONIA-OIS", EUR_EONIA, ACT_360, P12M, 1, 2);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// GBP fixed vs SONIA OIS swap for terms less than or equal to one year.
	  /// <para>
	  /// Both legs pay once at the end and use day count 'Act/365F'.
	  /// The spot date offset is 0 days and there is no payment date offset.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention GBP_FIXED_TERM_SONIA_OIS = makeConvention("GBP-FIXED-TERM-SONIA-OIS", GBP_SONIA, ACT_365F, TERM, 0, 0);

	  /// <summary>
	  /// GBP fixed vs SONIA OIS swap for terms greater than one year.
	  /// <para>
	  /// Both legs pay annually and use day count 'Act/365F'.
	  /// The spot date offset is 0 days and there is no payment date offset.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention GBP_FIXED_1Y_SONIA_OIS = makeConvention("GBP-FIXED-1Y-SONIA-OIS", GBP_SONIA, ACT_365F, P12M, 0, 0);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// JPY fixed vs TONAR OIS swap for terms less than or equal to one year.
	  /// <para>
	  /// Both legs pay once at the end and use day count 'Act/365F'.
	  /// The spot date offset is 2 days and there is no payment date offset.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention JPY_FIXED_TERM_TONAR_OIS = makeConvention("JPY-FIXED-TERM-TONAR-OIS", JPY_TONAR, ACT_365F, TERM, 0, 0);

	  /// <summary>
	  /// JPY fixed vs TONAR OIS swap for terms greater than one year.
	  /// <para>
	  /// Both legs pay annually and use day count 'Act/365F'.
	  /// The spot date offset is 2 days and there is no payment date offset.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention JPY_FIXED_1Y_TONAR_OIS = makeConvention("JPY-FIXED-1Y-TONAR-OIS", JPY_TONAR, ACT_365F, P12M, 0, 2);

	  //-------------------------------------------------------------------------
	  // build conventions
	  private static FixedOvernightSwapConvention makeConvention(string name, OvernightIndex index, DayCount dayCount, Frequency frequency, int paymentLag, int spotLag)
	  {

		HolidayCalendarId calendar = index.FixingCalendar;
		DaysAdjustment paymentDateOffset = DaysAdjustment.ofBusinessDays(paymentLag, calendar);
		DaysAdjustment spotDateOffset = DaysAdjustment.ofBusinessDays(spotLag, calendar);
		return ImmutableFixedOvernightSwapConvention.of(name, FixedRateSwapLegConvention.builder().currency(index.Currency).dayCount(dayCount).accrualFrequency(frequency).accrualBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, calendar)).paymentFrequency(frequency).paymentDateOffset(paymentDateOffset).stubConvention(StubConvention.SMART_INITIAL).build(), OvernightRateSwapLegConvention.builder().index(index).accrualMethod(COMPOUNDED).accrualFrequency(frequency).paymentFrequency(frequency).paymentDateOffset(paymentDateOffset).stubConvention(StubConvention.SMART_INITIAL).build(), spotDateOffset);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardFixedOvernightSwapConventions()
	  {
	  }

	}

}