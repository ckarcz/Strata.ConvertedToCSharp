/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Market standard three leg basis swap conventions.
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	public sealed class ThreeLegBasisSwapConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<ThreeLegBasisSwapConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(ThreeLegBasisSwapConvention));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'EUR-FIXED-1Y-EURIBOR-3M-EURIBOR-6M' swap convention.
	  /// <para>
	  /// EUR three leg basis swap of fixed, Euribor 3M and Euribor 6M.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </para>
	  /// </summary>
	  public static readonly ThreeLegBasisSwapConvention EUR_FIXED_1Y_EURIBOR_3M_EURIBOR_6M = ThreeLegBasisSwapConvention.of(StandardThreeLegBasisSwapConventions.EUR_FIXED_1Y_EURIBOR_3M_EURIBOR_6M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private ThreeLegBasisSwapConventions()
	  {
	  }

	}

}