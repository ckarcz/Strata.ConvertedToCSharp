/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Standardized credit default swap conventions.
	/// </summary>
	public sealed class CdsConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<CdsConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(CdsConvention));

	  /// <summary>
	  /// USD-dominated standardized credit default swap.
	  /// </summary>
	  public static readonly CdsConvention USD_STANDARD = CdsConvention.of(StandardCdsConventions.USD_STANDARD.Name);

	  /// <summary>
	  /// EUR-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'EUTA'.
	  /// </para>
	  /// </summary>
	  public static readonly CdsConvention EUR_STANDARD = CdsConvention.of(StandardCdsConventions.EUR_STANDARD.Name);

	  /// <summary>
	  /// EUR-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'EUTA' and 'GBLO'.
	  /// </para>
	  /// </summary>
	  public static readonly CdsConvention EUR_GB_STANDARD = CdsConvention.of(StandardCdsConventions.EUR_GB_STANDARD.Name);

	  /// <summary>
	  /// GBP-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'GBLO'.
	  /// </para>
	  /// </summary>
	  public static readonly CdsConvention GBP_STANDARD = CdsConvention.of(StandardCdsConventions.GBP_STANDARD.Name);

	  /// <summary>
	  /// GBP-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'GBLO' and 'USNY'.
	  /// </para>
	  /// </summary>
	  public static readonly CdsConvention GBP_US_STANDARD = CdsConvention.of(StandardCdsConventions.GBP_US_STANDARD.Name);

	  /// <summary>
	  /// JPY-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'JPTO'.
	  /// </para>
	  /// </summary>
	  public static readonly CdsConvention JPY_STANDARD = CdsConvention.of(StandardCdsConventions.JPY_STANDARD.Name);

	  /// <summary>
	  /// JPY-dominated standardized credit default swap.
	  /// <para>
	  /// The payment dates are calculated with 'JPTO', 'USNY' and 'GBLO'.
	  /// </para>
	  /// </summary>
	  public static readonly CdsConvention JPY_US_GB_STANDARD = CdsConvention.of(StandardCdsConventions.JPY_US_GB_STANDARD.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private CdsConventions()
	  {
	  }

	}

}