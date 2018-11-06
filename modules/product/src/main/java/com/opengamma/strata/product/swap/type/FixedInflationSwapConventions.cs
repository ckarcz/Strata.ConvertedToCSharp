/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Fixed-Inflation swap conventions.
	/// </summary>
	public sealed class FixedInflationSwapConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<FixedInflationSwapConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(FixedInflationSwapConvention));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// GBP vanilla fixed vs UK HCIP swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention GBP_FIXED_ZC_GB_HCIP = FixedInflationSwapConvention.of(StandardFixedInflationSwapConventions.GBP_FIXED_ZC_GB_HCIP.Name);

	  /// <summary>
	  /// GBP vanilla fixed vs UK RPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention GBP_FIXED_ZC_GB_RPI = FixedInflationSwapConvention.of(StandardFixedInflationSwapConventions.GBP_FIXED_ZC_GB_RPI.Name);

	  /// <summary>
	  /// GBP vanilla fixed vs UK RPIX swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention GBP_FIXED_ZC_GB_RPIX = FixedInflationSwapConvention.of(StandardFixedInflationSwapConventions.GBP_FIXED_ZC_GB_RPIX.Name);

	  /// <summary>
	  /// CHF vanilla fixed vs Switzerland CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention CHF_FIXED_ZC_CH_CPI = FixedInflationSwapConvention.of(StandardFixedInflationSwapConventions.CHF_FIXED_ZC_CH_CPI.Name);

	  /// <summary>
	  /// Euro vanilla fixed vs Europe CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention EUR_FIXED_ZC_EU_AI_CPI = FixedInflationSwapConvention.of(StandardFixedInflationSwapConventions.EUR_FIXED_ZC_EU_AI_CPI.Name);

	  /// <summary>
	  /// Euro vanilla fixed vs Europe (Excluding Tobacco) CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention EUR_FIXED_ZC_EU_EXT_CPI = FixedInflationSwapConvention.of(StandardFixedInflationSwapConventions.EUR_FIXED_ZC_EU_EXT_CPI.Name);

	  /// <summary>
	  /// JPY vanilla fixed vs Japan (Excluding Fresh Food) CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention JPY_FIXED_ZC_JP_CPI = FixedInflationSwapConvention.of(StandardFixedInflationSwapConventions.JPY_FIXED_ZC_JP_CPI.Name);

	  /// <summary>
	  /// USD(NY) vanilla fixed vs US Urban consumers CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention USD_FIXED_ZC_US_CPI = FixedInflationSwapConvention.of(StandardFixedInflationSwapConventions.USD_FIXED_ZC_US_CPI.Name);

	  /// <summary>
	  /// Euro vanilla fixed vs France CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention EUR_FIXED_ZC_FR_CPI = FixedInflationSwapConvention.of(StandardFixedInflationSwapConventions.EUR_FIXED_ZC_FR_CPI.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FixedInflationSwapConventions()
	  {
	  }

	}

}