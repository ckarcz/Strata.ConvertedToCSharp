/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Market standard Fixed-Overnight swap conventions.
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	public sealed class FixedOvernightSwapConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<FixedOvernightSwapConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(FixedOvernightSwapConvention));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'USD-FIXED-TERM-FED-FUND-OIS' swap convention.
	  /// <para>
	  /// USD fixed vs Fed Fund OIS swap for terms less than or equal to one year.
	  /// Both legs pay once at the end and use day count 'Act/360'.
	  /// The spot date offset is 2 days and the payment date offset is 2 days.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention USD_FIXED_TERM_FED_FUND_OIS = FixedOvernightSwapConvention.of(StandardFixedOvernightSwapConventions.USD_FIXED_TERM_FED_FUND_OIS.Name);

	  /// <summary>
	  /// The 'USD-FIXED-1Y-FED-FUND-OIS' swap convention.
	  /// <para>
	  /// USD fixed vs Fed Fund OIS swap for terms greater than one year.
	  /// Both legs pay annually and use day count 'Act/360'.
	  /// The spot date offset is 2 days and the payment date offset is 2 days.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention USD_FIXED_1Y_FED_FUND_OIS = FixedOvernightSwapConvention.of(StandardFixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'USD-FIXED-1Y-SOFR-OIS' swap convention.
	  /// <para>
	  /// USD fixed vs SOFR OIS swap for terms greater than one year.
	  /// Both legs pay annually and use day count 'Act/360'.
	  /// The spot date offset is 2 days and the payment date offset is 2 days.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention USD_FIXED_1Y_SOFR_OIS = FixedOvernightSwapConvention.of(StandardFixedOvernightSwapConventions.USD_FIXED_1Y_SOFR_OIS.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'EUR-FIXED-TERM-EONIA-OIS' swap convention.
	  /// <para>
	  /// EUR fixed vs EONIA OIS swap for terms less than or equal to one year.
	  /// Both legs pay once at the end and use day count 'Act/360'.
	  /// The spot date offset is 2 days and the payment date offset is 1 day.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention EUR_FIXED_TERM_EONIA_OIS = FixedOvernightSwapConvention.of(StandardFixedOvernightSwapConventions.EUR_FIXED_TERM_EONIA_OIS.Name);

	  /// <summary>
	  /// The 'EUR-FIXED-1Y-EONIA_OIS' swap convention.
	  /// <para>
	  /// EUR fixed vs EONIA OIS swap for terms greater than one year.
	  /// Both legs pay annually and use day count 'Act/360'.
	  /// The spot date offset is 2 days and the payment date offset is 1 day.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention EUR_FIXED_1Y_EONIA_OIS = FixedOvernightSwapConvention.of(StandardFixedOvernightSwapConventions.EUR_FIXED_1Y_EONIA_OIS.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'GBP-FIXED-TERM-SONIA-OIS' swap convention.
	  /// <para>
	  /// GBP fixed vs SONIA OIS swap for terms less than or equal to one year.
	  /// Both legs pay once at the end and use day count 'Act/365F'.
	  /// The spot date offset is 0 days and there is no payment date offset.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention GBP_FIXED_TERM_SONIA_OIS = FixedOvernightSwapConvention.of(StandardFixedOvernightSwapConventions.GBP_FIXED_TERM_SONIA_OIS.Name);

	  /// <summary>
	  /// The 'GBP-FIXED-1Y-SONIA-OIS' swap convention.
	  /// <para>
	  /// GBP fixed vs SONIA OIS swap for terms greater than one year.
	  /// Both legs pay annually and use day count 'Act/365F'.
	  /// The spot date offset is 0 days and there is no payment date offset.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention GBP_FIXED_1Y_SONIA_OIS = FixedOvernightSwapConvention.of(StandardFixedOvernightSwapConventions.GBP_FIXED_1Y_SONIA_OIS.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'JPY_FIXED_TERM_TONAR-OIS' swap convention.
	  /// <para>
	  /// JPY fixed vs TONAR OIS swap for terms less than or equal to one year.
	  /// Both legs pay once at the end and use day count 'Act/365F'.
	  /// The spot date offset is 2 days and there is no payment date offset.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention JPY_FIXED_TERM_TONAR_OIS = FixedOvernightSwapConvention.of(StandardFixedOvernightSwapConventions.JPY_FIXED_TERM_TONAR_OIS.Name);

	  /// <summary>
	  /// The 'JPY-FIXED-1Y-TONAR-OIS' swap convention.
	  /// <para>
	  /// JPY fixed vs TONAR OIS swap for terms greater than one year.
	  /// Both legs pay annually and use day count 'Act/365F'.
	  /// The spot date offset is 2 days and there is no payment date offset.
	  /// </para>
	  /// </summary>
	  public static readonly FixedOvernightSwapConvention JPY_FIXED_1Y_TONAR_OIS = FixedOvernightSwapConvention.of(StandardFixedOvernightSwapConventions.JPY_FIXED_1Y_TONAR_OIS.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FixedOvernightSwapConventions()
	  {
	  }

	}

}