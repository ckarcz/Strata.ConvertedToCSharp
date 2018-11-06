/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Market standard Fixed-Ibor swap conventions.
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	public sealed class FixedIborSwapConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<FixedIborSwapConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(FixedIborSwapConvention));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'USD-FIXED-6M-LIBOR-3M' swap convention.
	  /// <para>
	  /// USD(NY) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays every 6 months with day count '30U/360'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention USD_FIXED_6M_LIBOR_3M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.Name);

	  /// <summary>
	  /// The 'USD-FIXED-1Y-LIBOR-3M' swap convention.
	  /// <para>
	  /// USD(London) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays yearly with day count 'Act/360'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention USD_FIXED_1Y_LIBOR_3M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.USD_FIXED_1Y_LIBOR_3M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'EUR-FIXED-1Y-EURIBOR-3M' swap convention.
	  /// <para>
	  /// EUR(1Y) vanilla fixed vs Euribor 3M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention EUR_FIXED_1Y_EURIBOR_3M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M.Name);

	  /// <summary>
	  /// The 'EUR-FIXED-1Y-EURIBOR-6M' swap convention.
	  /// <para>
	  /// EUR(>1Y) vanilla fixed vs Euribor 6M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention EUR_FIXED_1Y_EURIBOR_6M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'EUR-FIXED-1Y-LIBOR-3M' swap convention.
	  /// <para>
	  /// EUR(1Y) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention EUR_FIXED_1Y_LIBOR_3M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.EUR_FIXED_1Y_LIBOR_3M.Name);

	  /// <summary>
	  /// The 'EUR-FIXED-1Y-LIBOR-6M' swap convention.
	  /// <para>
	  /// EUR(>1Y) vanilla fixed vs LIBOR 6M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention EUR_FIXED_1Y_LIBOR_6M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.EUR_FIXED_1Y_LIBOR_6M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'GBP-FIXED-1Y-LIBOR-3M' swap convention.
	  /// <para>
	  /// GBP(1Y) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays yearly with day count 'Act/365F'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention GBP_FIXED_1Y_LIBOR_3M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M.Name);

	  /// <summary>
	  /// The 'GBP-FIXED-6M-LIBOR-6M' swap convention.
	  /// <para>
	  /// GBP(>1Y) vanilla fixed vs LIBOR 6M swap.
	  /// The fixed leg pays every 6 months with day count 'Act/365F'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention GBP_FIXED_6M_LIBOR_6M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.GBP_FIXED_6M_LIBOR_6M.Name);

	  /// <summary>
	  /// The 'GBP-FIXED-3M-LIBOR-3M' swap convention.
	  /// <para>
	  /// GBP(>1Y) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays every 3 months with day count 'Act/365F'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention GBP_FIXED_3M_LIBOR_3M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.GBP_FIXED_3M_LIBOR_3M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'CHF-FIXED-1Y-LIBOR-3M' swap convention.
	  /// <para>
	  /// CHF(1Y) vanilla fixed vs LIBOR 3M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention CHF_FIXED_1Y_LIBOR_3M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_3M.Name);

	  /// <summary>
	  /// The 'CHF-FIXED-1Y-LIBOR-6M' swap convention.
	  /// <para>
	  /// CHF(>1Y) vanilla fixed vs LIBOR 6M swap.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention CHF_FIXED_1Y_LIBOR_6M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_6M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'JPY-FIXED-6M-TIBOR-JAPAN-3M' swap convention.
	  /// <para>
	  /// JPY(Tibor) vanilla fixed vs Tibor 3M swap.
	  /// The fixed leg pays every 6 months with day count 'Act/365F'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention JPY_FIXED_6M_TIBORJ_3M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.JPY_FIXED_6M_TIBORJ_3M.Name);

	  /// <summary>
	  /// The 'JPY-FIXED-6M-LIBOR-6M' swap convention.
	  /// <para>
	  /// JPY(LIBOR) vanilla fixed vs LIBOR 6M swap.
	  /// The fixed leg pays every 6 months with day count 'Act/365F'.
	  /// </para>
	  /// </summary>
	  public static readonly FixedIborSwapConvention JPY_FIXED_6M_LIBOR_6M = FixedIborSwapConvention.of(StandardFixedIborSwapConventions.JPY_FIXED_6M_LIBOR_6M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FixedIborSwapConventions()
	  {
	  }

	}

}