/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Market standard cross-currency Ibor-Ibor swap conventions.
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	public sealed class XCcyIborIborSwapConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<XCcyIborIborSwapConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(XCcyIborIborSwapConvention));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'EUR-EURIBOR-3M-USD-LIBOR-3M' swap convention.
	  /// <para>
	  /// EUR EURIBOR 3M v USD LIBOR 3M.
	  /// The spread is on the EUR leg.
	  /// </para>
	  /// </summary>
	  public static readonly XCcyIborIborSwapConvention EUR_EURIBOR_3M_USD_LIBOR_3M = XCcyIborIborSwapConvention.of(StandardXCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M.Name);

	  /// <summary>
	  /// The 'GBP-LIBOR-3M-USD-LIBOR-3M' swap convention.
	  /// <para>
	  /// GBP LIBOR 3M v USD LIBOR 3M.
	  /// The spread is on the GBP leg.
	  /// </para>
	  /// </summary>
	  public static readonly XCcyIborIborSwapConvention GBP_LIBOR_3M_USD_LIBOR_3M = XCcyIborIborSwapConvention.of(StandardXCcyIborIborSwapConventions.GBP_LIBOR_3M_USD_LIBOR_3M.Name);

	  /// <summary>
	  /// The 'GBP-LIBOR-3M-EUR-EURIBOR-3M' swap convention.
	  /// <para>
	  /// GBP LIBOR 3M v EUR EURIBOR 3M.
	  /// The spread is on the GBP leg.
	  /// </para>
	  /// </summary>
	  public static readonly XCcyIborIborSwapConvention GBP_LIBOR_3M_EUR_EURIBOR_3M = XCcyIborIborSwapConvention.of(StandardXCcyIborIborSwapConventions.GBP_LIBOR_3M_EUR_EURIBOR_3M.Name);

	  /// <summary>
	  /// The 'GBP-LIBOR-3M-JPY-LIBOR-3M' swap convention.
	  /// <para>
	  /// GBP LIBOR 3M v JPY LIBOR 3M.
	  /// The spread is on the GBP leg.
	  /// </para>
	  /// </summary>
	  public static readonly XCcyIborIborSwapConvention GBP_LIBOR_3M_JPY_LIBOR_3M = XCcyIborIborSwapConvention.of(StandardXCcyIborIborSwapConventions.GBP_LIBOR_3M_JPY_LIBOR_3M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private XCcyIborIborSwapConventions()
	  {
	  }

	}

}