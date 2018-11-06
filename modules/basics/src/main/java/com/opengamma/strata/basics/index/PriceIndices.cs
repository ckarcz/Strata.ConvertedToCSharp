/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard price indices.
	/// <para>
	/// Each constant returns a standard definition of the specified index.
	/// </para>
	/// </summary>
	public sealed class PriceIndices
	{
	  // constants are indirected via ENUM_LOOKUP to allow them to be replaced by config

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<PriceIndex> ENUM_LOOKUP = ExtendedEnum.of(typeof(PriceIndex));

	  /// <summary>
	  /// The harmonized consumer price index for the United Kingdom,
	  /// "Non-revised Harmonised Index of Consumer Prices".
	  /// </summary>
	  public static readonly PriceIndex GB_HICP = PriceIndex.of("GB-HICP");
	  /// <summary>
	  /// The retail price index for the United Kingdom,
	  /// "Non-revised Retail Price Index All Items in the United Kingdom".
	  /// </summary>
	  public static readonly PriceIndex GB_RPI = PriceIndex.of("GB-RPI");
	  /// <summary>
	  /// The retail price index for the United Kingdom excluding mortgage interest payments,
	  /// "Non-revised Retail Price Index Excluding Mortgage Interest Payments in the United Kingdom".
	  /// </summary>
	  public static readonly PriceIndex GB_RPIX = PriceIndex.of("GB-RPIX");
	  /// <summary>
	  /// The consumer price index for Switzerland,
	  /// "Non-revised Consumer Price Index".
	  /// </summary>
	  public static readonly PriceIndex CH_CPI = PriceIndex.of("CH-CPI");
	  /// <summary>
	  /// The consumer price index for Europe,
	  /// "Non-revised Harmonised Index of Consumer Prices All Items".
	  /// </summary>
	  public static readonly PriceIndex EU_AI_CPI = PriceIndex.of("EU-AI-CPI");
	  /// <summary>
	  /// The consumer price index for Europe,
	  /// "Non-revised Harmonised Index of Consumer Prices Excluding Tobacco".
	  /// </summary>
	  public static readonly PriceIndex EU_EXT_CPI = PriceIndex.of("EU-EXT-CPI");
	  /// <summary>
	  /// The consumer price index for Japan excluding fresh food,
	  /// "Non-revised Consumer Price Index Nationwide General Excluding Fresh Food".
	  /// </summary>
	  public static readonly PriceIndex JP_CPI_EXF = PriceIndex.of("JP-CPI-EXF");
	  /// <summary>
	  /// The consumer price index for US Urban consumers,
	  /// "Non-revised index of Consumer Prices for All Urban Consumers (CPI-U) before seasonal adjustment".
	  /// </summary>
	  public static readonly PriceIndex US_CPI_U = PriceIndex.of("US-CPI-U");
	  /// <summary>
	  /// The consumer price index for France,
	  /// "Non-revised Harmonised Index of Consumer Prices Excluding Tobacco".
	  /// </summary>
	  public static readonly PriceIndex FR_EXT_CPI = PriceIndex.of("FR-EXT-CPI");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private PriceIndices()
	  {
	  }

	}

}