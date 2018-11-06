/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard Floating rate names.
	/// <para>
	/// Each constant refers to a standard definition of the specified index.
	/// </para>
	/// <para>
	/// If a floating rate has a constant here, then it is fully supported by Strata
	/// with example holiday calendar data.
	/// </para>
	/// </summary>
	public sealed class FloatingRateNames
	{
	  // constants are indirected via ENUM_LOOKUP to allow them to be replaced by config

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<FloatingRateName> ENUM_LOOKUP = ExtendedEnum.of(typeof(FloatingRateName));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Constant for GBP-LIBOR.
	  /// </summary>
	  public static readonly FloatingRateName GBP_LIBOR = FloatingRateName.of("GBP-LIBOR");
	  /// <summary>
	  /// Constant for USD-LIBOR.
	  /// </summary>
	  public static readonly FloatingRateName USD_LIBOR = FloatingRateName.of("USD-LIBOR");
	  /// <summary>
	  /// Constant for CHF-LIBOR.
	  /// </summary>
	  public static readonly FloatingRateName CHF_LIBOR = FloatingRateName.of("CHF-LIBOR");
	  /// <summary>
	  /// Constant for EUR-LIBOR.
	  /// </summary>
	  public static readonly FloatingRateName EUR_LIBOR = FloatingRateName.of("EUR-LIBOR");
	  /// <summary>
	  /// Constant for JPY-LIBOR.
	  /// </summary>
	  public static readonly FloatingRateName JPY_LIBOR = FloatingRateName.of("JPY-LIBOR");
	  /// <summary>
	  /// Constant for EUR-EURIBOR.
	  /// </summary>
	  public static readonly FloatingRateName EUR_EURIBOR = FloatingRateName.of("EUR-EURIBOR");
	  /// <summary>
	  /// Constant for AUD-BBSW.
	  /// </summary>
	  public static readonly FloatingRateName AUD_BBSW = FloatingRateName.of("AUD-BBSW");
	  /// <summary>
	  /// Constant for CAD-CDOR.
	  /// </summary>
	  public static readonly FloatingRateName CAD_CDOR = FloatingRateName.of("CAD-CDOR");
	  /// <summary>
	  /// Constant for CZK-PRIBOR.
	  /// </summary>
	  public static readonly FloatingRateName CZK_PRIBOR = FloatingRateName.of("CZK-PRIBOR");
	  /// <summary>
	  /// Constant for DKK-CIBOR.
	  /// </summary>
	  public static readonly FloatingRateName DKK_CIBOR = FloatingRateName.of("DKK-CIBOR");
	  /// <summary>
	  /// Constant for HUF-BUBOR.
	  /// </summary>
	  public static readonly FloatingRateName HUF_BUBOR = FloatingRateName.of("HUF-BUBOR");
	  /// <summary>
	  /// Constant for MXN-TIIE.
	  /// </summary>
	  public static readonly FloatingRateName MXN_TIIE = FloatingRateName.of("MXN-TIIE");
	  /// <summary>
	  /// Constant for NOK-NIBOR.
	  /// </summary>
	  public static readonly FloatingRateName NOK_NIBOR = FloatingRateName.of("NOK-NIBOR");
	  /// <summary>
	  /// Constant for NZD-BKBM.
	  /// </summary>
	  public static readonly FloatingRateName NZD_BKBM = FloatingRateName.of("NZD-BKBM");
	  /// <summary>
	  /// Constant for PLN-WIBOR.
	  /// </summary>
	  public static readonly FloatingRateName PLN_WIBOR = FloatingRateName.of("PLN-WIBOR");
	  /// <summary>
	  /// Constant for SEK-STIBOR.
	  /// </summary>
	  public static readonly FloatingRateName SEK_STIBOR = FloatingRateName.of("SEK-STIBOR");
	  /// <summary>
	  /// Constant for ZAR-JIBAR.
	  /// </summary>
	  public static readonly FloatingRateName ZAR_JIBAR = FloatingRateName.of("ZAR-JIBAR");

	  /// <summary>
	  /// Constant for GBP-SONIA Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName GBP_SONIA = FloatingRateName.of("GBP-SONIA");
	  /// <summary>
	  /// Constant for USD-FED-FUND Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName USD_FED_FUND = FloatingRateName.of("USD-FED-FUND");
	  /// <summary>
	  /// Constant for USD-SOFR Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName USD_SOFR = FloatingRateName.of("USD-SOFR");
	  /// <summary>
	  /// Constant for CHF-SARON Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName CHF_SARON = FloatingRateName.of("CHF-SARON");
	  /// <summary>
	  /// Constant for CHF-TOIS Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName CHF_TOIS = FloatingRateName.of("CHF-TOIS");
	  /// <summary>
	  /// Constant for EUR-EONIA Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName EUR_EONIA = FloatingRateName.of("EUR-EONIA");
	  /// <summary>
	  /// Constant for JPY-TONAR Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName JPY_TONAR = FloatingRateName.of("JPY-TONAR");
	  /// <summary>
	  /// Constant for AUD-AONIA Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName AUD_AONIA = FloatingRateName.of("AUD-AONIA");
	  /// <summary>
	  /// Constant for BRL-CDI Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName BRL_CDI = FloatingRateName.of("BRL-CDI");
	  /// <summary>
	  /// Constant for CAD-CORRA Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName CAD_CORRA = FloatingRateName.of("CAD-CORRA");
	  /// <summary>
	  /// Constant for DKK-TNR Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName DKK_TNR = FloatingRateName.of("DKK-TNR");
	  /// <summary>
	  /// Constant for NOK-NOWA Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName NOK_NOWA = FloatingRateName.of("NOK-NOWA");
	  /// <summary>
	  /// Constant for PLN-POLONIA Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName PLN_POLONIA = FloatingRateName.of("PLN-POLONIA");
	  /// <summary>
	  /// Constant for SEK-SIOR Overnight index.
	  /// </summary>
	  public static readonly FloatingRateName SEK_SIOR = FloatingRateName.of("SEK-SIOR");

	  /// <summary>
	  /// Constant for USD-FED-FUND Overnight index using averaging.
	  /// </summary>
	  public static readonly FloatingRateName USD_FED_FUND_AVG = FloatingRateName.of("USD-FED-FUND-AVG");

	  /// <summary>
	  /// Constant for GB-RPI Price index.
	  /// </summary>
	  public static readonly FloatingRateName GB_RPI = FloatingRateName.of("GB-RPI");
	  /// <summary>
	  /// Constant for EU-EXT-CPI Price index.
	  /// </summary>
	  public static readonly FloatingRateName EU_EXT_CPI = FloatingRateName.of("EU-EXT-CPI");
	  /// <summary>
	  /// Constant for US-CPI-U Price index.
	  /// </summary>
	  public static readonly FloatingRateName US_CPI_U = FloatingRateName.of("US-CPI-U");
	  /// <summary>
	  /// Constant for FR-EXT-CPI Price index.
	  /// </summary>
	  public static readonly FloatingRateName FR_EXT_CPI = FloatingRateName.of("FR-EXT-CPI");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FloatingRateNames()
	  {
	  }

	}

}