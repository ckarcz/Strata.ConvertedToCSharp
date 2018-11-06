using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{

	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using Dom = com.opengamma.strata.basics.schedule.DayRollConventions.Dom;
	using Dow = com.opengamma.strata.basics.schedule.DayRollConventions.Dow;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard roll conventions.
	/// <para>
	/// The purpose of this convention is to define how to roll dates when building a schedule.
	/// The standard approach to building a schedule is based on unadjusted dates, which do not
	/// have a <seealso cref="BusinessDayConvention business day convention"/> applied.
	/// To get the next date in the schedule, take the base date and the
	/// <seealso cref="Frequency periodic frequency"/>. Once this date is calculated,
	/// the roll convention is applied to produce the next schedule date.
	/// </para>
	/// <para>
	/// In most cases the specific values for day-of-month and day-of-week are not needed.
	/// A one month periodic frequency will naturally select the same day-of-month as the
	/// input date, thus the day-of-month does not need to be additionally specified.
	/// </para>
	/// </summary>
	public sealed class RollConventions
	{
	  // constants are indirected via ENUM_LOOKUP to allow them to be replaced by config

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<RollConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(RollConvention));

	  /// <summary>
	  /// The 'None' roll convention.
	  /// <para>
	  /// The input date will not be adjusted.
	  /// </para>
	  /// <para>
	  /// When calculating a schedule, there will be no further adjustment after the
	  /// periodic frequency is added or subtracted.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention NONE = RollConvention.of(StandardRollConventions.NONE.Name);
	  /// <summary>
	  /// The 'EOM' roll convention which adjusts the date to the end of the month.
	  /// <para>
	  /// The input date will be adjusted ensure it is the last valid day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention EOM = RollConvention.of(StandardRollConventions.EOM.Name);
	  /// <summary>
	  /// The 'IMM' roll convention which adjusts the date to the third Wednesday.
	  /// <para>
	  /// The input date will be adjusted ensure it is the third Wednesday of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention IMM = RollConvention.of(StandardRollConventions.IMM.Name);
	  /// <summary>
	  /// The 'IMMCAD' roll convention which adjusts the date two days before the third Wednesday.
	  /// <para>
	  /// The input date will be adjusted ensure it is two GBLO business days before the third Wednesday of the month.
	  /// The date is further adjusted earlier by a combination of the CATO and CAMO calendars.
	  /// Standard reference data is used to resolve the holiday calendars.
	  /// (Note that all current GBLO, CATO and CAMO holiday dates will not impact the result.)
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention IMMCAD = RollConvention.of(StandardRollConventions.IMMCAD.Name);
	  /// <summary>
	  /// The 'IMMAUD' roll convention which adjusts the date to the Thursday before the second Friday.
	  /// <para>
	  /// The input date will be adjusted ensure it is the Thursday before the second Friday of the month.
	  /// Standard reference data is used to resolve the AUSY holiday calendar to subtract the day.
	  /// (Note that all current AUSY holiday dates will not impact the result.)
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention IMMAUD = RollConvention.of(StandardRollConventions.IMMAUD.Name);
	  /// <summary>
	  /// The 'IMMNZD' roll convention which adjusts the date to the first Wednesday
	  /// on or after the ninth day of the month.
	  /// <para>
	  /// The input date will be adjusted to the ninth day of the month, and then it will
	  /// be adjusted to be a Wednesday. If the ninth is a Wednesday, then that is returned.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention IMMNZD = RollConvention.of(StandardRollConventions.IMMNZD.Name);
	  /// <summary>
	  /// The 'SFE' roll convention which adjusts the date to the second Friday.
	  /// <para>
	  /// The input date will be adjusted ensure it is the second Friday of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention SFE = RollConvention.of(StandardRollConventions.SFE.Name);
	  /// <summary>
	  /// The 'TBILL' roll convention which adjusts the date to next Monday.
	  /// <para>
	  /// The input date will be adjusted ensure it is the next Monday.
	  /// The USNY holiday calendar is used in case the Monday is a holiday.
	  /// Standard reference data is used to resolve the holiday calendar.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention TBILL = RollConvention.of(StandardRollConventions.TBILL.Name);

	  /// <summary>
	  /// The 'Day1' roll convention which adjusts the date to day-of-month 1.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 1st day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_1 = Dom.of(1);
	  /// <summary>
	  /// The 'Day2' roll convention which adjusts the date to day-of-month 2.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 2nd day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_2 = Dom.of(2);
	  /// <summary>
	  /// The 'Day3' roll convention which adjusts the date to day-of-month 3.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 3rd day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_3 = Dom.of(3);
	  /// <summary>
	  /// The 'Day4' roll convention which adjusts the date to day-of-month 4.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 4th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_4 = Dom.of(4);
	  /// <summary>
	  /// The 'Day5' roll convention which adjusts the date to day-of-month 5.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 5th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_5 = Dom.of(5);
	  /// <summary>
	  /// The 'Day6' roll convention which adjusts the date to day-of-month 6.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 6th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_6 = Dom.of(6);
	  /// <summary>
	  /// The 'Day7' roll convention which adjusts the date to day-of-month 7.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 7th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_7 = Dom.of(7);
	  /// <summary>
	  /// The 'Day8' roll convention which adjusts the date to day-of-month 8.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 8th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_8 = Dom.of(8);
	  /// <summary>
	  /// The 'Day9' roll convention which adjusts the date to day-of-month 9.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 9th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_9 = Dom.of(9);
	  /// <summary>
	  /// The 'Day10' roll convention which adjusts the date to day-of-month 10.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 10th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_10 = Dom.of(10);
	  /// <summary>
	  /// The 'Day11' roll convention which adjusts the date to day-of-month 11.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 11th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_11 = Dom.of(11);
	  /// <summary>
	  /// The 'Day12' roll convention which adjusts the date to day-of-month 12.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 12th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_12 = Dom.of(12);
	  /// <summary>
	  /// The 'Day13' roll convention which adjusts the date to day-of-month 13
	  /// <para>
	  /// The input date will be adjusted ensure it is the 13th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_13 = Dom.of(13);
	  /// <summary>
	  /// The 'Day14' roll convention which adjusts the date to day-of-month 14.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 14th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_14 = Dom.of(14);
	  /// <summary>
	  /// The 'Day15' roll convention which adjusts the date to day-of-month 15.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 15th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_15 = Dom.of(15);
	  /// <summary>
	  /// The 'Day16' roll convention which adjusts the date to day-of-month 16.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 16th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_16 = Dom.of(16);
	  /// <summary>
	  /// The 'Day17' roll convention which adjusts the date to day-of-month 17.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 17th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_17 = Dom.of(17);
	  /// <summary>
	  /// The 'Day18' roll convention which adjusts the date to day-of-month 18.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 18th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_18 = Dom.of(18);
	  /// <summary>
	  /// The 'Day19' roll convention which adjusts the date to day-of-month 19.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 19th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_19 = Dom.of(19);
	  /// <summary>
	  /// The 'Day20' roll convention which adjusts the date to day-of-month 20.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 20th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_20 = Dom.of(20);
	  /// <summary>
	  /// The 'Day21' roll convention which adjusts the date to day-of-month 21.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 21st day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_21 = Dom.of(21);
	  /// <summary>
	  /// The 'Day22' roll convention which adjusts the date to day-of-month 22.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 22nd day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_22 = Dom.of(22);
	  /// <summary>
	  /// The 'Day23' roll convention which adjusts the date to day-of-month 23.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 23rd day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_23 = Dom.of(23);
	  /// <summary>
	  /// The 'Day24' roll convention which adjusts the date to day-of-month 24.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 24th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_24 = Dom.of(24);
	  /// <summary>
	  /// The 'Day25' roll convention which adjusts the date to day-of-month 25.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 25th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_25 = Dom.of(25);
	  /// <summary>
	  /// The 'Day26' roll convention which adjusts the date to day-of-month 26.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 26th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_26 = Dom.of(26);
	  /// <summary>
	  /// The 'Day27' roll convention which adjusts the date to day-of-month 27.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 27th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_27 = Dom.of(27);
	  /// <summary>
	  /// The 'Day28' roll convention which adjusts the date to day-of-month 28.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 28th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_28 = Dom.of(28);
	  /// <summary>
	  /// The 'Day29' roll convention which adjusts the date to day-of-month 29.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 29th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_29 = Dom.of(29);
	  /// <summary>
	  /// The 'Day30' roll convention which adjusts the date to day-of-month 30.
	  /// <para>
	  /// The input date will be adjusted ensure it is the 30th day of the month.
	  /// The year and month of the result date will be the same as the input date.
	  /// </para>
	  /// <para>
	  /// This convention is intended for use with periods that are a multiple of months.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_30 = Dom.of(30);

	  /// <summary>
	  /// The 'DayMon' roll convention which adjusts the date to be Monday.
	  /// <para>
	  /// The input date will be adjusted ensure it is a Monday.
	  /// This convention is intended for use with periods that are a multiple of weeks.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_MON = Dow.of(DayOfWeek.Monday);
	  /// <summary>
	  /// The 'DayTue' roll convention which adjusts the date to be Tuesday.
	  /// <para>
	  /// The input date will be adjusted ensure it is a Tuesday.
	  /// This convention is intended for use with periods that are a multiple of weeks.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_TUE = Dow.of(DayOfWeek.Tuesday);
	  /// <summary>
	  /// The 'DayWed' roll convention which adjusts the date to be Wednesday.
	  /// <para>
	  /// The input date will be adjusted ensure it is a Wednesday.
	  /// This convention is intended for use with periods that are a multiple of weeks.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_WED = Dow.of(DayOfWeek.Wednesday);
	  /// <summary>
	  /// The 'DayThu' roll convention which adjusts the date to be Thursday.
	  /// <para>
	  /// The input date will be adjusted ensure it is a Thursday.
	  /// This convention is intended for use with periods that are a multiple of weeks.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_THU = Dow.of(DayOfWeek.Thursday);
	  /// <summary>
	  /// The 'DayFri' roll convention which adjusts the date to be Friday.
	  /// <para>
	  /// The input date will be adjusted ensure it is a Friday.
	  /// This convention is intended for use with periods that are a multiple of weeks.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_FRI = Dow.of(DayOfWeek.Friday);
	  /// <summary>
	  /// The 'DaySat' roll convention which adjusts the date to be Saturday.
	  /// <para>
	  /// The input date will be adjusted ensure it is a Saturday.
	  /// This convention is intended for use with periods that are a multiple of weeks.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_SAT = Dow.of(DayOfWeek.Saturday);
	  /// <summary>
	  /// The 'DaySun' roll convention which adjusts the date to be Sunday.
	  /// <para>
	  /// The input date will be adjusted ensure it is a Sunday.
	  /// This convention is intended for use with periods that are a multiple of weeks.
	  /// </para>
	  /// </summary>
	  public static readonly RollConvention DAY_SUN = Dow.of(DayOfWeek.Sunday);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private RollConventions()
	  {
	  }

	}

}