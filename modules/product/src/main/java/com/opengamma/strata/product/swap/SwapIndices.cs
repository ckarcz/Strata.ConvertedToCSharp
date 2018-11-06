/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard swap indices.
	/// <para>
	/// Each constant returns a standard definition of the specified index.
	/// </para>
	/// </summary>
	public sealed class SwapIndices
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<SwapIndex> ENUM_LOOKUP = ExtendedEnum.of(typeof(SwapIndex));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// USD Rates 1100 for tenor of 1 year.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_1Y = SwapIndex.of("USD-LIBOR-1100-1Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 2 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_2Y = SwapIndex.of("USD-LIBOR-1100-2Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 3 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_3Y = SwapIndex.of("USD-LIBOR-1100-3Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 4 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_4Y = SwapIndex.of("USD-LIBOR-1100-4Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 5 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_5Y = SwapIndex.of("USD-LIBOR-1100-5Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 6 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_6Y = SwapIndex.of("USD-LIBOR-1100-6Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 7 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_7Y = SwapIndex.of("USD-LIBOR-1100-7Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 8 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_8Y = SwapIndex.of("USD-LIBOR-1100-8Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 9 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_9Y = SwapIndex.of("USD-LIBOR-1100-9Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 10 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_10Y = SwapIndex.of("USD-LIBOR-1100-10Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 15 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_15Y = SwapIndex.of("USD-LIBOR-1100-15Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 20 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_20Y = SwapIndex.of("USD-LIBOR-1100-20Y");
	  /// <summary>
	  /// USD Rates 1100 for tenor of 30 years.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1100_30Y = SwapIndex.of("USD-LIBOR-1100-30Y");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// USD Rates 1500 for tenor of 1 year.
	  /// </summary>
	  public static readonly SwapIndex USD_LIBOR_1500_1Y = SwapIndex.of("USD-LIBOR-1500-1Y");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 1 year.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_1Y = SwapIndex.of("EUR-EURIBOR-1100-1Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 2 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_2Y = SwapIndex.of("EUR-EURIBOR-1100-2Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 3 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_3Y = SwapIndex.of("EUR-EURIBOR-1100-3Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 4 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_4Y = SwapIndex.of("EUR-EURIBOR-1100-4Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 5 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_5Y = SwapIndex.of("EUR-EURIBOR-1100-5Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 6 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_6Y = SwapIndex.of("EUR-EURIBOR-1100-6Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 7 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_7Y = SwapIndex.of("EUR-EURIBOR-1100-7Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 8 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_8Y = SwapIndex.of("EUR-EURIBOR-1100-8Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 9 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_9Y = SwapIndex.of("EUR-EURIBOR-1100-9Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 10 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_10Y = SwapIndex.of("EUR-EURIBOR-1100-10Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 12 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_12Y = SwapIndex.of("EUR-EURIBOR-1100-12Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 15 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_15Y = SwapIndex.of("EUR-EURIBOR-1100-15Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 20 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_20Y = SwapIndex.of("EUR-EURIBOR-1100-20Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 25 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_25Y = SwapIndex.of("EUR-EURIBOR-1100-25Y");
	  /// <summary>
	  /// EUR Rates 1100 for tenor of 30 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1100_30Y = SwapIndex.of("EUR-EURIBOR-1100-30Y");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 1 year.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_1Y = SwapIndex.of("EUR-EURIBOR-1200-1Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 2 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_2Y = SwapIndex.of("EUR-EURIBOR-1200-2Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 3 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_3Y = SwapIndex.of("EUR-EURIBOR-1200-3Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 4 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_4Y = SwapIndex.of("EUR-EURIBOR-1200-4Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 5 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_5Y = SwapIndex.of("EUR-EURIBOR-1200-5Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 6 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_6Y = SwapIndex.of("EUR-EURIBOR-1200-6Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 7 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_7Y = SwapIndex.of("EUR-EURIBOR-1200-7Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 8 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_8Y = SwapIndex.of("EUR-EURIBOR-1200-8Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 9 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_9Y = SwapIndex.of("EUR-EURIBOR-1200-9Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 10 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_10Y = SwapIndex.of("EUR-EURIBOR-1200-10Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 12 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_12Y = SwapIndex.of("EUR-EURIBOR-1200-12Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 15 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_15Y = SwapIndex.of("EUR-EURIBOR-1200-15Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 20 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_20Y = SwapIndex.of("EUR-EURIBOR-1200-20Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 25 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_25Y = SwapIndex.of("EUR-EURIBOR-1200-25Y");
	  /// <summary>
	  /// EUR Rates 1200 for tenor of 30 years.
	  /// </summary>
	  public static readonly SwapIndex EUR_EURIBOR_1200_30Y = SwapIndex.of("EUR-EURIBOR-1200-30Y");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 1 year.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_1Y = SwapIndex.of("GBP-LIBOR-1100-1Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 2 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_2Y = SwapIndex.of("GBP-LIBOR-1100-2Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 3 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_3Y = SwapIndex.of("GBP-LIBOR-1100-3Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 4 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_4Y = SwapIndex.of("GBP-LIBOR-1100-4Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 5 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_5Y = SwapIndex.of("GBP-LIBOR-1100-5Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 6 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_6Y = SwapIndex.of("GBP-LIBOR-1100-6Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 7 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_7Y = SwapIndex.of("GBP-LIBOR-1100-7Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 8 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_8Y = SwapIndex.of("GBP-LIBOR-1100-8Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 9 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_9Y = SwapIndex.of("GBP-LIBOR-1100-9Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 10 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_10Y = SwapIndex.of("GBP-LIBOR-1100-10Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 12 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_12Y = SwapIndex.of("GBP-LIBOR-1100-12Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 15 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_15Y = SwapIndex.of("GBP-LIBOR-1100-15Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 20 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_20Y = SwapIndex.of("GBP-LIBOR-1100-20Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 25 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_25Y = SwapIndex.of("GBP-LIBOR-1100-25Y");
	  /// <summary>
	  /// GBP Rates 1100 for tenor of 30 years.
	  /// </summary>
	  public static readonly SwapIndex GBP_LIBOR_1100_30Y = SwapIndex.of("GBP-LIBOR-1100-30Y");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private SwapIndices()
	  {
	  }
	}

}