/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Market standard FX swap conventions.
	/// </summary>
	public sealed class FxSwapConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<FxSwapConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(FxSwapConvention));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The "EUR/USD" FX Swap convention.
	  /// <para>
	  /// EUR/USD convention with 2 days spot date.
	  /// </para>
	  /// </summary>
	  public static readonly FxSwapConvention EUR_USD = FxSwapConvention.of(StandardFxSwapConventions.EUR_USD.Name);

	  /// <summary>
	  /// The "EUR/GBP" FX Swap convention.
	  /// <para>
	  /// EUR/GBP convention with 2 days spot date.
	  /// </para>
	  /// </summary>
	  public static readonly FxSwapConvention EUR_GBP = FxSwapConvention.of(StandardFxSwapConventions.EUR_GBP.Name);

	  /// <summary>
	  /// The "GBP/USD" FX Swap convention.
	  /// <para>
	  /// GBP/USD convention with 2 days spot date.
	  /// </para>
	  /// </summary>
	  public static readonly FxSwapConvention GBP_USD = FxSwapConvention.of(StandardFxSwapConventions.GBP_USD.Name);

	  /// <summary>
	  /// The "GBP/JPY" FX Swap convention.
	  /// <para>
	  /// GBP/JPY convention with 2 days spot date.
	  /// </para>
	  /// </summary>
	  public static readonly FxSwapConvention GBP_JPY = FxSwapConvention.of(StandardFxSwapConventions.GBP_JPY.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FxSwapConventions()
	  {
	  }

	}

}