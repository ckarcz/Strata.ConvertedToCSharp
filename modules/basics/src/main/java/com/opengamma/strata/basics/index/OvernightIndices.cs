/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard Overnight rate indices.
	/// <para>
	/// Each constant returns a standard definition of the specified index.
	/// </para>
	/// <para>
	/// If a floating rate has a constant here, then it is fully supported by Strata
	/// with example holiday calendar data.
	/// </para>
	/// </summary>
	public sealed class OvernightIndices
	{
	  // constants are indirected via ENUM_LOOKUP to allow them to be replaced by config

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<OvernightIndex> ENUM_LOOKUP = ExtendedEnum.of(typeof(OvernightIndex));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The SONIA index for GBP.
	  /// <para>
	  /// The Sterling Overnight Index Average (SONIA) index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex GBP_SONIA = OvernightIndex.of("GBP-SONIA");
	  /// <summary>
	  /// The SARON index for CHF.
	  /// <para>
	  /// The Swiss Average Overnight Rate (SARON) index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex CHF_SARON = OvernightIndex.of("CHF-SARON");
	  /// <summary>
	  /// The TOIS index for CHF.
	  /// <para>
	  /// The Tomorrow/Next Overnight Indexed Swaps (TOIS) index, which is a "Tomorrow/Next" index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex CHF_TOIS = OvernightIndex.of("CHF-TOIS");
	  /// <summary>
	  /// The EONIA index for EUR.
	  /// <para>
	  /// The Euro OverNight Index Average (EONIA) index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex EUR_EONIA = OvernightIndex.of("EUR-EONIA");
	  /// <summary>
	  /// The TONAR index for JPY.
	  /// <para>
	  /// The Tokyo Overnight Average Rate (TONAR) index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex JPY_TONAR = OvernightIndex.of("JPY-TONAR");
	  /// <summary>
	  /// The Fed Fund index for USD.
	  /// <para>
	  /// The Federal Funds Rate index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex USD_FED_FUND = OvernightIndex.of("USD-FED-FUND");
	  /// <summary>
	  /// The SOFR index for USD.
	  /// <para>
	  /// The Secured Overnight Financing Rate (SOFR) index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex USD_SOFR = OvernightIndex.of("USD-SOFR");
	  /// <summary>
	  /// The AONIA index for AUD.
	  /// <para>
	  /// AONIA is an "Overnight" index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex AUD_AONIA = OvernightIndex.of("AUD-AONIA");
	  /// <summary>
	  /// The CDI index for BRL.
	  /// <para>
	  /// The "Brazil Certificates of Interbank Deposit" index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex BRL_CDI = OvernightIndex.of("BRL-CDI");
	  /// <summary>
	  /// The CORRA index for CAD.
	  /// <para>
	  /// The "Canadian Overnight Repo Rate Average" index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex CAD_CORRA = OvernightIndex.of("CAD-CORRA");
	  /// <summary>
	  /// The TN index for DKK.
	  /// <para>
	  /// The "Tomorrow/Next-renten" index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex DKK_TNR = OvernightIndex.of("DKK-TNR");
	  /// <summary>
	  /// The NOWA index for NOK.
	  /// <para>
	  /// The "Norwegian Overnight Weighted Average" index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex NOK_NOWA = OvernightIndex.of("NOK-NOWA");
	  /// <summary>
	  /// The NZIONA index for NZD.
	  /// <para>
	  /// The "New Zealand Overnight" index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex NZD_NZIONA = OvernightIndex.of("NZD-NZIONA");
	  /// <summary>
	  /// The PLONIA index for PLN.
	  /// <para>
	  /// The "Polish Overnight" index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex PLN_POLONIA = OvernightIndex.of("PLN-POLONIA");
	  /// <summary>
	  /// The SIOR index for SEK.
	  /// <para>
	  /// The "STIBOR T/N" index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex SEK_SIOR = OvernightIndex.of("SEK-SIOR");
	  /// <summary>
	  /// The SABOR index for ZAR.
	  /// <para>
	  /// The "South African Benchmark Overnight Rate" index.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightIndex ZAR_SABOR = OvernightIndex.of("ZAR-SABOR");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private OvernightIndices()
	  {
	  }

	}

}