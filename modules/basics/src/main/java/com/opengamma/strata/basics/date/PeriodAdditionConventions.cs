/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard period addition conventions.
	/// <para>
	/// The purpose of each convention is to define how to handle the addition of a period.
	/// The default implementations include two different end-of-month rules.
	/// The convention is generally only applicable for month-based periods.
	/// </para>
	/// </summary>
	public sealed class PeriodAdditionConventions
	{
	  // constants are indirected via ENUM_LOOKUP to allow them to be replaced by config

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<PeriodAdditionConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(PeriodAdditionConvention));

	  /// <summary>
	  /// No specific rule applies.
	  /// <para>
	  /// Given a date, the specified period is added using standard date arithmetic.
	  /// The business day adjustment is applied to produce the final result.
	  /// </para>
	  /// <para>
	  /// For example, adding a period of 1 month to June 30th will result in July 30th.
	  /// </para>
	  /// </summary>
	  public static readonly PeriodAdditionConvention NONE = PeriodAdditionConvention.of(StandardPeriodAdditionConventions.NONE.Name);
	  /// <summary>
	  /// Convention applying a last day of month rule, <i>ignoring business days</i>.
	  /// <para>
	  /// Given a date, the specified period is added using standard date arithmetic,
	  /// shifting to the end-of-month if the base date is the last day of the month.
	  /// The business day adjustment is applied to produce the final result.
	  /// Note that this rule is based on the last day of the month, not the last business day of the month.
	  /// </para>
	  /// <para>
	  /// For example, adding a period of 1 month to June 30th will result in July 31st.
	  /// </para>
	  /// </summary>
	  public static readonly PeriodAdditionConvention LAST_DAY = PeriodAdditionConvention.of(StandardPeriodAdditionConventions.LAST_DAY.Name);
	  /// <summary>
	  /// Convention applying a last <i>business</i> day of month rule.
	  /// <para>
	  /// Given a date, the specified period is added using standard date arithmetic,
	  /// shifting to the last business day of the month if the base date is the
	  /// last business day of the month.
	  /// The business day adjustment is applied to produce the final result.
	  /// </para>
	  /// <para>
	  /// For example, adding a period of 1 month to June 29th will result in July 31st
	  /// assuming that June 30th is not a valid business day and July 31st is.
	  /// </para>
	  /// </summary>
	  public static readonly PeriodAdditionConvention LAST_BUSINESS_DAY = PeriodAdditionConvention.of(StandardPeriodAdditionConventions.LAST_BUSINESS_DAY.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private PeriodAdditionConventions()
	  {
	  }

	}

}