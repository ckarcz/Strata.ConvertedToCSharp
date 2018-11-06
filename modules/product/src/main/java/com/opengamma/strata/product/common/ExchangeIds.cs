/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.common
{
	/// <summary>
	/// Identifiers for common exchanges.
	/// <para>
	/// The identifier names are ISO Market Identifier Codes (MICs).
	/// </para>
	/// </summary>
	public sealed class ExchangeIds
	{

	  /// <summary>
	  /// Eurex Clearing AG. </summary>
	  public static readonly ExchangeId ECAG = ExchangeId.of("ECAG");

	  /// <summary>
	  /// Chicago Mercantile Exchange (CME). </summary>
	  public static readonly ExchangeId XCME = ExchangeId.of("XCME");

	  /// <summary>
	  /// Chicago Board of Trade (CBOT). </summary>
	  public static readonly ExchangeId XCBT = ExchangeId.of("XCBT");

	  /// <summary>
	  /// Chicago Board Options Exchange. </summary>
	  public static readonly ExchangeId XCBO = ExchangeId.of("XCBO");

	  /// <summary>
	  /// New York Mercantile Exchange (NYMEX). </summary>
	  public static readonly ExchangeId XNYM = ExchangeId.of("XNYM");

	  /// <summary>
	  /// Commodities Exchange Center (COMEX). </summary>
	  public static readonly ExchangeId XCEC = ExchangeId.of("XCEC");

	  /// <summary>
	  /// ICE Futures Europe - Equity Products Division. </summary>
	  public static readonly ExchangeId IFLO = ExchangeId.of("IFLO");

	  /// <summary>
	  /// ICE Futures Europe - Financial Products Division. </summary>
	  public static readonly ExchangeId IFLL = ExchangeId.of("IFLL");

	  /// <summary>
	  /// ICE Futures Europe - European Utilities Division. </summary>
	  public static readonly ExchangeId IFUT = ExchangeId.of("IFUT");

	  /// <summary>
	  /// ICE Futures Europe - Agricultural Products Division. </summary>
	  public static readonly ExchangeId IFLX = ExchangeId.of("IFLX");

	  /// <summary>
	  /// ICE Futures Europe - Oil and Refined Products Division. </summary>
	  public static readonly ExchangeId IFEN = ExchangeId.of("IFEN");

	  /// <summary>
	  /// ICE Futures U.S. </summary>
	  public static readonly ExchangeId IFUS = ExchangeId.of("IFUS");

	  /// <summary>
	  /// Osaka Exchange. </summary>
	  public static readonly ExchangeId XOSE = ExchangeId.of("XOSE");

	  /// <summary>
	  /// Hong Kong Exchanges And Clearing Ltd. </summary>
	  public static readonly ExchangeId XHKG = ExchangeId.of("XHKG");

	  /// <summary>
	  /// Hong Kong Futures Exchange Ltd. </summary>
	  public static readonly ExchangeId XHKF = ExchangeId.of("XHKF");

	  /// <summary>
	  /// Singapore Exchange Ltd. </summary>
	  public static readonly ExchangeId XSES = ExchangeId.of("XSES");

	  /// <summary>
	  /// Australian Securities Exchange. </summary>
	  public static readonly ExchangeId XASX = ExchangeId.of("XASX");

	  /// <summary>
	  /// ASX - Trade24 (formerly Sydney Futures Exchange). </summary>
	  public static readonly ExchangeId XSFE = ExchangeId.of("XSFE");

	  /// <summary>
	  /// New Zealand Futures & Options. </summary>
	  public static readonly ExchangeId NZFX = ExchangeId.of("NZFX");

	  /// <summary>
	  /// Warsaw Stock Exchange. </summary>
	  public static readonly ExchangeId XWAR = ExchangeId.of("XWAR");

	  /// <summary>
	  /// Johannesburg Stock Exchange. </summary>
	  public static readonly ExchangeId XJSE = ExchangeId.of("XJSE");

	  /// <summary>
	  /// JSE - Equity Derivatives Market. </summary>
	  public static readonly ExchangeId XSAF = ExchangeId.of("XSAF");

	  /// <summary>
	  /// Mercado Español de Futuros Financiero (MEFF). </summary>
	  public static readonly ExchangeId XMRV = ExchangeId.of("XMRV");

	  /// <summary>
	  /// Bursa Malaysia. </summary>
	  public static readonly ExchangeId XKLS = ExchangeId.of("XKLS");

	  /// <summary>
	  /// London Metal Exchange. </summary>
	  public static readonly ExchangeId XLME = ExchangeId.of("XLME");

	  /// <summary>
	  /// Minneapolis Grain Exchange. </summary>
	  public static readonly ExchangeId XMGE = ExchangeId.of("XMGE");

	  /// <summary>
	  /// The Montreal Exchange. </summary>
	  public static readonly ExchangeId XMOD = ExchangeId.of("XMOD");

	  /// <summary>
	  /// Tokyo Financial Exchange. </summary>
	  public static readonly ExchangeId XTFF = ExchangeId.of("XTFF");

	  /// <summary>
	  /// Tokyo Commodity Exchange. </summary>
	  public static readonly ExchangeId XTKT = ExchangeId.of("XTKT");

	  /// <summary>
	  /// Tokyo Stock Exchange. </summary>
	  public static readonly ExchangeId XTKS = ExchangeId.of("XTKS");

	  /// <summary>
	  /// Borsa Istanbul Exchange </summary>
	  public static readonly ExchangeId XFNO = ExchangeId.of("XFNO");
	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private ExchangeIds()
	  {
	  }

	}

}