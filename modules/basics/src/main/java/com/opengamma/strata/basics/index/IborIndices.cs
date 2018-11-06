/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard Ibor indices.
	/// <para>
	/// Each constant returns a standard definition of the specified index.
	/// </para>
	/// <para>
	/// If a floating rate has a constant here, then it is fully supported by Strata
	/// with example holiday calendar data.
	/// </para>
	/// </summary>
	public sealed class IborIndices
	{
	  // constants are indirected via ENUM_LOOKUP to allow them to be replaced by config

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<IborIndex> ENUM_LOOKUP = ExtendedEnum.of(typeof(IborIndex));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week LIBOR index for GBP.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex GBP_LIBOR_1W = IborIndex.of("GBP-LIBOR-1W");
	  /// <summary>
	  /// The 1 month LIBOR index for GBP.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex GBP_LIBOR_1M = IborIndex.of("GBP-LIBOR-1M");
	  /// <summary>
	  /// The 2 month LIBOR index for GBP.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex GBP_LIBOR_2M = IborIndex.of("GBP-LIBOR-2M");
	  /// <summary>
	  /// The 3 month LIBOR index for GBP.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex GBP_LIBOR_3M = IborIndex.of("GBP-LIBOR-3M");
	  /// <summary>
	  /// The 6 month LIBOR index for GBP.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex GBP_LIBOR_6M = IborIndex.of("GBP-LIBOR-6M");
	  /// <summary>
	  /// The 12 month LIBOR index for GBP.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex GBP_LIBOR_12M = IborIndex.of("GBP-LIBOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week LIBOR index for CHF.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CHF_LIBOR_1W = IborIndex.of("CHF-LIBOR-1W");
	  /// <summary>
	  /// The 1 month LIBOR index for CHF.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CHF_LIBOR_1M = IborIndex.of("CHF-LIBOR-1M");
	  /// <summary>
	  /// The 2 month LIBOR index for CHF.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CHF_LIBOR_2M = IborIndex.of("CHF-LIBOR-2M");
	  /// <summary>
	  /// The 3 month LIBOR index for CHF.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CHF_LIBOR_3M = IborIndex.of("CHF-LIBOR-3M");
	  /// <summary>
	  /// The 6 month LIBOR index for CHF.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CHF_LIBOR_6M = IborIndex.of("CHF-LIBOR-6M");
	  /// <summary>
	  /// The 12 month LIBOR index for CHF.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CHF_LIBOR_12M = IborIndex.of("CHF-LIBOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week LIBOR index for EUR.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_LIBOR_1W = IborIndex.of("EUR-LIBOR-1W");
	  /// <summary>
	  /// The 1 month LIBOR index for EUR.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_LIBOR_1M = IborIndex.of("EUR-LIBOR-1M");
	  /// <summary>
	  /// The 2 month LIBOR index for EUR.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_LIBOR_2M = IborIndex.of("EUR-LIBOR-2M");
	  /// <summary>
	  /// The 3 month LIBOR index for EUR.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_LIBOR_3M = IborIndex.of("EUR-LIBOR-3M");
	  /// <summary>
	  /// The 6 month LIBOR index for EUR.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_LIBOR_6M = IborIndex.of("EUR-LIBOR-6M");
	  /// <summary>
	  /// The 12 month LIBOR index for EUR.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_LIBOR_12M = IborIndex.of("EUR-LIBOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week LIBOR index for JPY.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_LIBOR_1W = IborIndex.of("JPY-LIBOR-1W");
	  /// <summary>
	  /// The 1 month LIBOR index for JPY.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_LIBOR_1M = IborIndex.of("JPY-LIBOR-1M");
	  /// <summary>
	  /// The 2 month LIBOR index for JPY.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_LIBOR_2M = IborIndex.of("JPY-LIBOR-2M");
	  /// <summary>
	  /// The 3 month LIBOR index for JPY.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_LIBOR_3M = IborIndex.of("JPY-LIBOR-3M");
	  /// <summary>
	  /// The 6 month LIBOR index for JPY.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_LIBOR_6M = IborIndex.of("JPY-LIBOR-6M");
	  /// <summary>
	  /// The 12 month LIBOR index for JPY.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_LIBOR_12M = IborIndex.of("JPY-LIBOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week LIBOR index for USD.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex USD_LIBOR_1W = IborIndex.of("USD-LIBOR-1W");
	  /// <summary>
	  /// The 1 month LIBOR index for USD.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex USD_LIBOR_1M = IborIndex.of("USD-LIBOR-1M");
	  /// <summary>
	  /// The 2 month LIBOR index for USD.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex USD_LIBOR_2M = IborIndex.of("USD-LIBOR-2M");
	  /// <summary>
	  /// The 3 month LIBOR index for USD.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex USD_LIBOR_3M = IborIndex.of("USD-LIBOR-3M");
	  /// <summary>
	  /// The 6 month LIBOR index for USD.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex USD_LIBOR_6M = IborIndex.of("USD-LIBOR-6M");
	  /// <summary>
	  /// The 12 month LIBOR index for USD.
	  /// <para>
	  /// The "London Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex USD_LIBOR_12M = IborIndex.of("USD-LIBOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week EURIBOR index.
	  /// <para>
	  /// The "Euro Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_EURIBOR_1W = IborIndex.of("EUR-EURIBOR-1W");
	  /// <summary>
	  /// The 2 week EURIBOR index.
	  /// <para>
	  /// The "Euro Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_EURIBOR_2W = IborIndex.of("EUR-EURIBOR-2W");
	  /// <summary>
	  /// The 1 month EURIBOR index.
	  /// <para>
	  /// The "Euro Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_EURIBOR_1M = IborIndex.of("EUR-EURIBOR-1M");
	  /// <summary>
	  /// The 2 month EURIBOR index.
	  /// <para>
	  /// The "Euro Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_EURIBOR_2M = IborIndex.of("EUR-EURIBOR-2M");
	  /// <summary>
	  /// The 3 month EURIBOR index.
	  /// <para>
	  /// The "Euro Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_EURIBOR_3M = IborIndex.of("EUR-EURIBOR-3M");
	  /// <summary>
	  /// The 6 month EURIBOR index.
	  /// <para>
	  /// The "Euro Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_EURIBOR_6M = IborIndex.of("EUR-EURIBOR-6M");
	  /// <summary>
	  /// The 9 month EURIBOR index.
	  /// <para>
	  /// The "Euro Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_EURIBOR_9M = IborIndex.of("EUR-EURIBOR-9M");
	  /// <summary>
	  /// The 12 month EURIBOR index.
	  /// <para>
	  /// The "Euro Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex EUR_EURIBOR_12M = IborIndex.of("EUR-EURIBOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week TIBOR (Japan) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", unsecured call market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_JAPAN_1W = IborIndex.of("JPY-TIBOR-JAPAN-1W");
	  /// <summary>
	  /// The 1 month TIBOR (Japan) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", unsecured call market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_JAPAN_1M = IborIndex.of("JPY-TIBOR-JAPAN-1M");
	  /// <summary>
	  /// The 2 month TIBOR (Japan) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", unsecured call market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_JAPAN_2M = IborIndex.of("JPY-TIBOR-JAPAN-2M");
	  /// <summary>
	  /// The 3 month TIBOR (Japan) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", unsecured call market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_JAPAN_3M = IborIndex.of("JPY-TIBOR-JAPAN-3M");
	  /// <summary>
	  /// The 6 month TIBOR (Japan) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", unsecured call market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_JAPAN_6M = IborIndex.of("JPY-TIBOR-JAPAN-6M");
	  /// <summary>
	  /// The 12 month TIBOR (Japan) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", unsecured call market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_JAPAN_12M = IborIndex.of("JPY-TIBOR-JAPAN-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week TIBOR (Euroyen) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", Japan offshore market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_EUROYEN_1W = IborIndex.of("JPY-TIBOR-EUROYEN-1W");
	  /// <summary>
	  /// The 1 month TIBOR (Euroyen) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", Japan offshore market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_EUROYEN_1M = IborIndex.of("JPY-TIBOR-EUROYEN-1M");
	  /// <summary>
	  /// The 2 month TIBOR (Euroyen) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", Japan offshore market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_EUROYEN_2M = IborIndex.of("JPY-TIBOR-EUROYEN-2M");
	  /// <summary>
	  /// The 3 month TIBOR (Euroyen) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", Japan offshore market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_EUROYEN_3M = IborIndex.of("JPY-TIBOR-EUROYEN-3M");
	  /// <summary>
	  /// The 6 month TIBOR (Euroyen) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", Japan offshore market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_EUROYEN_6M = IborIndex.of("JPY-TIBOR-EUROYEN-6M");
	  /// <summary>
	  /// The 12 month TIBOR (Euroyen) index.
	  /// <para>
	  /// The "Tokyo Interbank Offered Rate", Japan offshore market.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex JPY_TIBOR_EUROYEN_12M = IborIndex.of("JPY-TIBOR-EUROYEN-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 month BBSW index.
	  /// <para>
	  /// The AFMA Australian Bank Bill Short Term Rate.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex AUD_BBSW_1M = IborIndex.of("AUD-BBSW-1M");
	  /// <summary>
	  /// The 2 month BBSW index.
	  /// <para>
	  /// The AFMA Australian Bank Bill Short Term Rate.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex AUD_BBSW_2M = IborIndex.of("AUD-BBSW-2M");
	  /// <summary>
	  /// The 3 month BBSW index.
	  /// <para>
	  /// The AFMA Australian Bank Bill Short Term Rate.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex AUD_BBSW_3M = IborIndex.of("AUD-BBSW-3M");
	  /// <summary>
	  /// The 4 month BBSW index.
	  /// <para>
	  /// The AFMA Australian Bank Bill Short Term Rate.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex AUD_BBSW_4M = IborIndex.of("AUD-BBSW-4M");
	  /// <summary>
	  /// The 5 month BBSW index.
	  /// <para>
	  /// The AFMA Australian Bank Bill Short Term Rate.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex AUD_BBSW_5M = IborIndex.of("AUD-BBSW-5M");
	  /// <summary>
	  /// The 6 month BBSW index.
	  /// <para>
	  /// The AFMA Australian Bank Bill Short Term Rate.
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex AUD_BBSW_6M = IborIndex.of("AUD-BBSW-6M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 month CDOR index.
	  /// <para>
	  /// The "Canadian Dollar Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CAD_CDOR_1M = IborIndex.of("CAD-CDOR-1M");
	  /// <summary>
	  /// The 2 month CDOR index.
	  /// <para>
	  /// The "Canadian Dollar Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CAD_CDOR_2M = IborIndex.of("CAD-CDOR-2M");
	  /// <summary>
	  /// The 3 month CDOR index.
	  /// <para>
	  /// The "Canadian Dollar Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CAD_CDOR_3M = IborIndex.of("CAD-CDOR-3M");
	  /// <summary>
	  /// The 6 month CDOR index.
	  /// <para>
	  /// The "Canadian Dollar Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CAD_CDOR_6M = IborIndex.of("CAD-CDOR-6M");
	  /// <summary>
	  /// The 12 month CDOR index.
	  /// <para>
	  /// The "Canadian Dollar Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CAD_CDOR_12M = IborIndex.of("CAD-CDOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week PRIBOR index.
	  /// <para>
	  /// The "Prague Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CZK_PRIBOR_1W = IborIndex.of("CZK-PRIBOR-1W");
	  /// <summary>
	  /// The 2 week PRIBOR index.
	  /// <para>
	  /// The "Prague Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CZK_PRIBOR_2W = IborIndex.of("CZK-PRIBOR-2W");
	  /// <summary>
	  /// The 1 month PRIBOR index.
	  /// <para>
	  /// The "Prague Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CZK_PRIBOR_1M = IborIndex.of("CZK-PRIBOR-1M");
	  /// <summary>
	  /// The 2 month PRIBOR index.
	  /// <para>
	  /// The "Prague Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CZK_PRIBOR_2M = IborIndex.of("CZK-PRIBOR-2M");
	  /// <summary>
	  /// The 3 month PRIBOR index.
	  /// <para>
	  /// The "Prague Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CZK_PRIBOR_3M = IborIndex.of("CZK-PRIBOR-3M");
	  /// <summary>
	  /// The 6 month PRIBOR index.
	  /// <para>
	  /// The "Prague Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CZK_PRIBOR_6M = IborIndex.of("CZK-PRIBOR-6M");
	  /// <summary>
	  /// The 9 month PRIBOR index.
	  /// <para>
	  /// The "Prague Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CZK_PRIBOR_9M = IborIndex.of("CZK-PRIBOR-9M");
	  /// <summary>
	  /// The 12 month PRIBOR index.
	  /// <para>
	  /// The "Prague Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex CZK_PRIBOR_12M = IborIndex.of("CZK-PRIBOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week CIBOR index.
	  /// <para>
	  /// The "Copenhagen Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex DKK_CIBOR_1W = IborIndex.of("DKK-CIBOR-1W");
	  /// <summary>
	  /// The 2 week CIBOR index.
	  /// <para>
	  /// The "Copenhagen Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex DKK_CIBOR_2W = IborIndex.of("DKK-CIBOR-2W");
	  /// <summary>
	  /// The 1 month CIBOR index.
	  /// <para>
	  /// The "Copenhagen Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex DKK_CIBOR_1M = IborIndex.of("DKK-CIBOR-1M");
	  /// <summary>
	  /// The 2 month CIBOR index.
	  /// <para>
	  /// The "Copenhagen Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex DKK_CIBOR_2M = IborIndex.of("DKK-CIBOR-2M");
	  /// <summary>
	  /// The 3 month CIBOR index.
	  /// <para>
	  /// The "Copenhagen Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex DKK_CIBOR_3M = IborIndex.of("DKK-CIBOR-3M");
	  /// <summary>
	  /// The 6 month CIBOR index.
	  /// <para>
	  /// The "Copenhagen Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex DKK_CIBOR_6M = IborIndex.of("DKK-CIBOR-6M");
	  /// <summary>
	  /// The 9 month CIBOR index.
	  /// <para>
	  /// The "Copenhagen Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex DKK_CIBOR_9M = IborIndex.of("DKK-CIBOR-9M");
	  /// <summary>
	  /// The 12 month CIBOR index.
	  /// <para>
	  /// The "Copenhagen Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex DKK_CIBOR_12M = IborIndex.of("DKK-CIBOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week BUBOR index.
	  /// <para>
	  /// The "Budapest Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex HUF_BUBOR_1W = IborIndex.of("HUF-BUBOR-1W");
	  /// <summary>
	  /// The 2 week BUBOR index.
	  /// <para>
	  /// The "Budapest Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex HUF_BUBOR_2W = IborIndex.of("HUF-BUBOR-2W");
	  /// <summary>
	  /// The 1 month BUBOR index.
	  /// <para>
	  /// The "Budapest Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex HUF_BUBOR_1M = IborIndex.of("HUF-BUBOR-1M");
	  /// <summary>
	  /// The 2 month BUBOR index.
	  /// <para>
	  /// The "Budapest Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex HUF_BUBOR_2M = IborIndex.of("HUF-BUBOR-2M");
	  /// <summary>
	  /// The 3 month BUBOR index.
	  /// <para>
	  /// The "Budapest Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex HUF_BUBOR_3M = IborIndex.of("HUF-BUBOR-3M");
	  /// <summary>
	  /// The 6 month BUBOR index.
	  /// <para>
	  /// The "Budapest Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex HUF_BUBOR_6M = IborIndex.of("HUF-BUBOR-6M");
	  /// <summary>
	  /// The 9 month BUBOR index.
	  /// <para>
	  /// The "Budapest Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex HUF_BUBOR_9M = IborIndex.of("HUF-BUBOR-9M");
	  /// <summary>
	  /// The 12 month BUBOR index.
	  /// <para>
	  /// The "Budapest Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex HUF_BUBOR_12M = IborIndex.of("HUF-BUBOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 4 week TIIE index.
	  /// <para>
	  /// The "Interbank Equilibrium Interest Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex MXN_TIIE_4W = IborIndex.of("MXN-TIIE-4W");
	  /// <summary>
	  /// The 13 week TIIE index.
	  /// <para>
	  /// The "Interbank Equilibrium Interest Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex MXN_TIIE_13W = IborIndex.of("MXN-TIIE-13W");
	  /// <summary>
	  /// The 26 week TIIE index.
	  /// <para>
	  /// The "Interbank Equilibrium Interest Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex MXN_TIIE_26W = IborIndex.of("MXN-TIIE-26W");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week NIBOR index.
	  /// <para>
	  /// The "Norwegian Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NOK_NIBOR_1W = IborIndex.of("NOK-NIBOR-1W");
	  /// <summary>
	  /// The 1 month NIBOR index.
	  /// <para>
	  /// The "Norwegian Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NOK_NIBOR_1M = IborIndex.of("NOK-NIBOR-1M");
	  /// <summary>
	  /// The 2 month NIBOR index.
	  /// <para>
	  /// The "Norwegian Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NOK_NIBOR_2M = IborIndex.of("NOK-NIBOR-2M");
	  /// <summary>
	  /// The 3 month NIBOR index.
	  /// <para>
	  /// The "Norwegian Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NOK_NIBOR_3M = IborIndex.of("NOK-NIBOR-3M");
	  /// <summary>
	  /// The 6 month NIBOR index.
	  /// <para>
	  /// The "Norwegian Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NOK_NIBOR_6M = IborIndex.of("NOK-NIBOR-6M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 month BKBM index.
	  /// <para>
	  /// The "New Zealand Bank Bill Benchmark Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NZD_BKBM_1M = IborIndex.of("NZD-BKBM-1M");
	  /// <summary>
	  /// The 2 month BKBM index.
	  /// <para>
	  /// The "New Zealand Bank Bill Benchmark Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NZD_BKBM_2M = IborIndex.of("NZD-BKBM-2M");
	  /// <summary>
	  /// The 3 month BKBM index.
	  /// <para>
	  /// The "New Zealand Bank Bill Benchmark Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NZD_BKBM_3M = IborIndex.of("NZD-BKBM-3M");
	  /// <summary>
	  /// The 4 month BKBM index.
	  /// <para>
	  /// The "New Zealand Bank Bill Benchmark Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NZD_BKBM_4M = IborIndex.of("NZD-BKBM-4M");
	  /// <summary>
	  /// The 5 month BKBM index.
	  /// <para>
	  /// The "New Zealand Bank Bill Benchmark Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NZD_BKBM_5M = IborIndex.of("NZD-BKBM-5M");
	  /// <summary>
	  /// The 6 month BKBM index.
	  /// <para>
	  /// The "New Zealand Bank Bill Benchmark Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex NZD_BKBM_6M = IborIndex.of("NZD-BKBM-6M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week WIBOR index.
	  /// <para>
	  /// The "Polish Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex PLN_WIBOR_1W = IborIndex.of("PLN-WIBOR-1W");
	  /// <summary>
	  /// The 1 month WIBOR index.
	  /// <para>
	  /// The "Polish Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex PLN_WIBOR_1M = IborIndex.of("PLN-WIBOR-1M");
	  /// <summary>
	  /// The 3 month WIBOR index.
	  /// <para>
	  /// The "Polish Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex PLN_WIBOR_3M = IborIndex.of("PLN-WIBOR-3M");
	  /// <summary>
	  /// The 6 month WIBOR index.
	  /// <para>
	  /// The "Polish Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex PLN_WIBOR_6M = IborIndex.of("PLN-WIBOR-6M");
	  /// <summary>
	  /// The 12 month WIBOR index.
	  /// <para>
	  /// The "Polish Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex PLN_WIBOR_12M = IborIndex.of("PLN-WIBOR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 week STIBOR index.
	  /// <para>
	  /// The "Swedish Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex SEK_STIBOR_1W = IborIndex.of("SEK-STIBOR-1W");
	  /// <summary>
	  /// The 1 month STIBOR index.
	  /// <para>
	  /// The "Swedish Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex SEK_STIBOR_1M = IborIndex.of("SEK-STIBOR-1M");
	  /// <summary>
	  /// The 2 month STIBOR index.
	  /// <para>
	  /// The "Swedish Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex SEK_STIBOR_2M = IborIndex.of("SEK-STIBOR-2M");
	  /// <summary>
	  /// The 3 month STIBOR index.
	  /// <para>
	  /// The "Swedish Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex SEK_STIBOR_3M = IborIndex.of("SEK-STIBOR-3M");
	  /// <summary>
	  /// The 6 month STIBOR index.
	  /// <para>
	  /// The "Swedish Interbank Offered Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex SEK_STIBOR_6M = IborIndex.of("SEK-STIBOR-6M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 1 month JIBAR index.
	  /// <para>
	  /// The "Johannnesburg Interbank Average Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex ZAR_JIBAR_1M = IborIndex.of("ZAR-JIBAR-1M");
	  /// <summary>
	  /// The 3 month JIBAR index.
	  /// <para>
	  /// The "Johannnesburg Interbank Average Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex ZAR_JIBAR_3M = IborIndex.of("ZAR-JIBAR-3M");
	  /// <summary>
	  /// The 6 month JIBAR index.
	  /// <para>
	  /// The "Johannnesburg Interbank Average Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex ZAR_JIBAR_6M = IborIndex.of("ZAR-JIBAR-6M");
	  /// <summary>
	  /// The 12 month JIBAR index.
	  /// <para>
	  /// The "Johannnesburg Interbank Average Rate".
	  /// </para>
	  /// </summary>
	  public static readonly IborIndex ZAR_JIBAR_12M = IborIndex.of("ZAR-JIBAR-12M");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private IborIndices()
	  {
	  }

	}

}