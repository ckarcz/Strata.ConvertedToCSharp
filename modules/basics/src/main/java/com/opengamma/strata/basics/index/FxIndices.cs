/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard foreign exchange indices.
	/// <para>
	/// Each constant returns a standard definition of the specified index.
	/// </para>
	/// </summary>
	public sealed class FxIndices
	{
	  // constants are indirected via ENUM_LOOKUP to allow them to be replaced by config

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<FxIndex> ENUM_LOOKUP = ExtendedEnum.of(typeof(FxIndex));

	  /// <summary>
	  /// The FX index for conversion from EUR to CHF, as defined by the European Central Bank
	  /// "Euro foreign exchange reference rates".
	  /// </summary>
	  public static readonly FxIndex EUR_CHF_ECB = FxIndex.of("EUR/CHF-ECB");
	  /// <summary>
	  /// The FX index for conversion from EUR to GBP, as defined by the European Central Bank
	  /// "Euro foreign exchange reference rates".
	  /// </summary>
	  public static readonly FxIndex EUR_GBP_ECB = FxIndex.of("EUR/GBP-ECB");
	  /// <summary>
	  /// The FX index for conversion from EUR to JPY, as defined by the European Central Bank
	  /// "Euro foreign exchange reference rates".
	  /// </summary>
	  public static readonly FxIndex EUR_JPY_ECB = FxIndex.of("EUR/JPY-ECB");
	  /// <summary>
	  /// The FX index for conversion from EUR to USD, as defined by the European Central Bank
	  /// "Euro foreign exchange reference rates".
	  /// </summary>
	  public static readonly FxIndex EUR_USD_ECB = FxIndex.of("EUR/USD-ECB");

	  /// <summary>
	  /// The FX index for conversion from USD to CHF, as defined by the WM company
	  /// "Closing Spot rates".
	  /// </summary>
	  public static readonly FxIndex USD_CHF_WM = FxIndex.of("USD/CHF-WM");
	  /// <summary>
	  /// The FX index for conversion from GBP to USD, as defined by the WM company
	  /// "Closing Spot rates".
	  /// </summary>
	  public static readonly FxIndex GBP_USD_WM = FxIndex.of("GBP/USD-WM");
	  /// <summary>
	  /// The FX index for conversion from EUR to GBP, as defined by the WM company
	  /// "Closing Spot rates".
	  /// </summary>
	  public static readonly FxIndex EUR_USD_WM = FxIndex.of("EUR/USD-WM");
	  /// <summary>
	  /// The FX index for conversion from USD to JPY, as defined by the WM company
	  /// "Closing Spot rates".
	  /// </summary>
	  public static readonly FxIndex USD_JPY_WM = FxIndex.of("USD/JPY-WM");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FxIndices()
	  {
	  }

	}

}