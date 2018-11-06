/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P12M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.OvernightAccrualMethod.AVERAGED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.OvernightAccrualMethod.COMPOUNDED;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;

	/// <summary>
	/// Market standard Fixed-Overnight swap conventions.
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	internal sealed class StandardOvernightIborSwapConventions
	{

	  /// <summary>
	  /// USD Fed Fund AA v LIBOR 3M swap .
	  /// <para>
	  /// Both legs use day count 'Act/360'.
	  /// The spot date offset is 2 days and the cut-off period is 2 days.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIborSwapConvention USD_FED_FUND_AA_LIBOR_3M = makeConvention("USD-FED-FUND-AA-LIBOR-3M", USD_FED_FUND, USD_LIBOR_3M, ACT_360, P3M, 0, 2, AVERAGED);

	  /// <summary>
	  /// GBP Sonia compounded 1Y v LIBOR 3M .
	  /// <para>
	  /// Both legs use day count 'Act/365F'.
	  /// The spot date offset is 0 days and payment offset is 0 days.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIborSwapConvention GBP_SONIA_OIS_1Y_LIBOR_3M = makeConvention("GBP-SONIA-OIS-1Y-LIBOR-3M", GBP_SONIA, GBP_LIBOR_3M, ACT_365F, P12M, 0, 0, COMPOUNDED);

	  //-------------------------------------------------------------------------
	  // build conventions
	  private static OvernightIborSwapConvention makeConvention(string name, OvernightIndex onIndex, IborIndex iborIndex, DayCount dayCount, Frequency frequency, int paymentLag, int cutOffDays, OvernightAccrualMethod accrual)
	  {

		HolidayCalendarId calendarOn = onIndex.FixingCalendar;
		DaysAdjustment paymentDateOffset = DaysAdjustment.ofBusinessDays(paymentLag, calendarOn);
		return ImmutableOvernightIborSwapConvention.of(name, OvernightRateSwapLegConvention.builder().index(onIndex).accrualMethod(accrual).accrualFrequency(frequency).paymentFrequency(frequency).paymentDateOffset(paymentDateOffset).stubConvention(StubConvention.SMART_INITIAL).rateCutOffDays(cutOffDays).build(), IborRateSwapLegConvention.of(iborIndex));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardOvernightIborSwapConventions()
	  {
	  }

	}

}