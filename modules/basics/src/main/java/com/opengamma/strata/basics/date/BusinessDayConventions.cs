/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard business day conventions.
	/// <para>
	/// The purpose of each convention is to define how to handle non-business days.
	/// When processing dates in finance, it is typically intended that non-business days,
	/// such as weekends and holidays, are converted to a nearby valid business day.
	/// The convention, in conjunction with a <seealso cref="HolidayCalendar holiday calendar"/>,
	/// defines exactly how the adjustment should be made.
	/// </para>
	/// </summary>
	public sealed class BusinessDayConventions
	{
	  // constants are indirected via ENUM_LOOKUP to allow them to be replaced by config

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<BusinessDayConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(BusinessDayConvention));

	  /// <summary>
	  /// The 'NoAdjust' convention which makes no adjustment.
	  /// <para>
	  /// The input date will not be adjusted even if it is not a business day.
	  /// </para>
	  /// </summary>
	  public static readonly BusinessDayConvention NO_ADJUST = BusinessDayConvention.of(StandardBusinessDayConventions.NO_ADJUST.Name);
	  /// <summary>
	  /// The 'Following' convention which adjusts to the next business day.
	  /// <para>
	  /// If the input date is not a business day then the date is adjusted.
	  /// The adjusted date is the next business day.
	  /// </para>
	  /// </summary>
	  public static readonly BusinessDayConvention FOLLOWING = BusinessDayConvention.of(StandardBusinessDayConventions.FOLLOWING.Name);
	  /// <summary>
	  /// The 'ModifiedFollowing' convention which adjusts to the next business day without crossing month end.
	  /// <para>
	  /// If the input date is not a business day then the date is adjusted.
	  /// The adjusted date is the next business day unless that day is in a different
	  /// calendar month, in which case the previous business day is returned.
	  /// </para>
	  /// </summary>
	  public static readonly BusinessDayConvention MODIFIED_FOLLOWING = BusinessDayConvention.of(StandardBusinessDayConventions.MODIFIED_FOLLOWING.Name);
	  /// <summary>
	  /// The 'ModifiedFollowingBiMonthly' convention which adjusts to the next business day without
	  /// crossing mid-month or month end.
	  /// <para>
	  /// If the input date is not a business day then the date is adjusted.
	  /// The month is divided into two parts, the first half, the 1st to 15th and the 16th onwards.
	  /// The adjusted date is the next business day unless that day is in a different half-month,
	  /// in which case the previous business day is returned.
	  /// </para>
	  /// </summary>
	  public static readonly BusinessDayConvention MODIFIED_FOLLOWING_BI_MONTHLY = BusinessDayConvention.of(StandardBusinessDayConventions.MODIFIED_FOLLOWING_BI_MONTHLY.Name);
	  /// <summary>
	  /// The 'Preceding' convention which adjusts to the previous business day.
	  /// <para>
	  /// If the input date is not a business day then the date is adjusted.
	  /// The adjusted date is the previous business day.
	  /// </para>
	  /// </summary>
	  public static readonly BusinessDayConvention PRECEDING = BusinessDayConvention.of(StandardBusinessDayConventions.PRECEDING.Name);
	  /// <summary>
	  /// The 'ModifiedPreceding' convention which adjusts to the previous business day without crossing month start.
	  /// <para>
	  /// If the input date is not a business day then the date is adjusted.
	  /// The adjusted date is the previous business day unless that day is in a different
	  /// calendar month, in which case the next business day is returned.
	  /// </para>
	  /// </summary>
	  public static readonly BusinessDayConvention MODIFIED_PRECEDING = BusinessDayConvention.of(StandardBusinessDayConventions.MODIFIED_PRECEDING.Name);
	  /// <summary>
	  /// The 'Nearest' convention which adjusts Sunday and Monday forward, and other days backward.
	  /// <para>
	  /// If the input date is not a business day then the date is adjusted.
	  /// If the input is Sunday or Monday then the next business day is returned.
	  /// Otherwise the previous business day is returned.
	  /// </para>
	  /// <para>
	  /// Note that despite the name, the algorithm may not return the business day that is actually nearest.
	  /// </para>
	  /// </summary>
	  public static readonly BusinessDayConvention NEAREST = BusinessDayConvention.of(StandardBusinessDayConventions.NEAREST.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private BusinessDayConventions()
	  {
	  }

	}

}