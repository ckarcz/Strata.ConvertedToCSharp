/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	public sealed class OvernightIborSwapConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<OvernightIborSwapConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(OvernightIborSwapConvention));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'USD-FED-FUND-AA-LIBOR-3M' swap convention.
	  /// <para>
	  /// USD Fed Fund Arithmetic Average 3M v Libor 3M swap.
	  /// Both legs use day count 'Act/360'.
	  /// The spot date offset is 2 days, the rate cut-off period is 2 days.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIborSwapConvention USD_FED_FUND_AA_LIBOR_3M = OvernightIborSwapConvention.of(StandardOvernightIborSwapConventions.USD_FED_FUND_AA_LIBOR_3M.Name);

	  /// <summary>
	  /// The 'GBP-SONIA-OIS-1Y-LIBOR-3M' swap convention.
	  /// <para>
	  /// GBP Sonia compounded 1Y v LIBOR 3M .
	  /// Both legs use day count 'Act/365F'.
	  /// The spot date offset is 0 days and payment offset is 0 days.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIborSwapConvention GBP_SONIA_OIS_1Y_LIBOR_3M = OvernightIborSwapConvention.of(StandardOvernightIborSwapConventions.GBP_SONIA_OIS_1Y_LIBOR_3M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private OvernightIborSwapConventions()
	  {
	  }

	}

}