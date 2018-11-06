/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CHF;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ONE_ONE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.CHZU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.FRPA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using PriceIndices = com.opengamma.strata.basics.index.PriceIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;

	/// <summary>
	/// Fixed-Inflation swap conventions.
	/// </summary>
	internal sealed class StandardFixedInflationSwapConventions
	{

	  /// <summary>
	  /// Three month lag.
	  /// </summary>
	  private static readonly Period LAG_3M = Period.ofMonths(3);
	  /// <summary>
	  /// Two month lag.
	  /// </summary>
	  private static readonly Period LAG_2M = Period.ofMonths(2);

	  /// <summary>
	  /// GBP vanilla fixed vs UK HCIP swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention GBP_FIXED_ZC_GB_HCIP = ImmutableFixedInflationSwapConvention.of("GBP-FIXED-ZC-GB-HCIP", fixedLegZcConvention(GBP, GBLO), InflationRateSwapLegConvention.of(PriceIndices.GB_HICP, LAG_2M, PriceIndexCalculationMethod.MONTHLY, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)), DaysAdjustment.ofBusinessDays(2, GBLO));

	  /// <summary>
	  /// GBP vanilla fixed vs UK RPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention GBP_FIXED_ZC_GB_RPI = ImmutableFixedInflationSwapConvention.of("GBP-FIXED-ZC-GB-RPI", fixedLegZcConvention(GBP, GBLO), InflationRateSwapLegConvention.of(PriceIndices.GB_RPI, LAG_2M, PriceIndexCalculationMethod.MONTHLY, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)), DaysAdjustment.ofBusinessDays(2, GBLO));

	  /// <summary>
	  /// GBP vanilla fixed vs UK RPIX swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention GBP_FIXED_ZC_GB_RPIX = ImmutableFixedInflationSwapConvention.of("GBP-FIXED-ZC-GB-RPIX", fixedLegZcConvention(GBP, GBLO), InflationRateSwapLegConvention.of(PriceIndices.GB_RPIX, LAG_2M, PriceIndexCalculationMethod.MONTHLY, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)), DaysAdjustment.ofBusinessDays(2, GBLO));

	  /// <summary>
	  /// CHF vanilla fixed vs Switzerland CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention CHF_FIXED_ZC_CH_CPI = ImmutableFixedInflationSwapConvention.of("CHF-FIXED-ZC-CH-CPI", fixedLegZcConvention(CHF, CHZU), InflationRateSwapLegConvention.of(PriceIndices.CH_CPI, LAG_3M, PriceIndexCalculationMethod.MONTHLY, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, CHZU)), DaysAdjustment.ofBusinessDays(2, CHZU));

	  /// <summary>
	  /// EUR vanilla fixed vs Europe CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention EUR_FIXED_ZC_EU_AI_CPI = ImmutableFixedInflationSwapConvention.of("EUR-FIXED-ZC-EU-AI-CPI", fixedLegZcConvention(EUR, EUTA), InflationRateSwapLegConvention.of(PriceIndices.EU_AI_CPI, LAG_3M, PriceIndexCalculationMethod.MONTHLY, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)), DaysAdjustment.ofBusinessDays(2, EUTA));

	  /// <summary>
	  /// EUR vanilla fixed vs Europe (Excluding Tobacco) CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention EUR_FIXED_ZC_EU_EXT_CPI = ImmutableFixedInflationSwapConvention.of("EUR-FIXED-ZC-EU-EXT-CPI", fixedLegZcConvention(EUR, EUTA), InflationRateSwapLegConvention.of(PriceIndices.EU_EXT_CPI, LAG_3M, PriceIndexCalculationMethod.MONTHLY, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)), DaysAdjustment.ofBusinessDays(2, EUTA));

	  /// <summary>
	  /// JPY vanilla fixed vs Japan (Excluding Fresh Food) CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention JPY_FIXED_ZC_JP_CPI = ImmutableFixedInflationSwapConvention.of("JPY-FIXED-ZC-JP-CPI", fixedLegZcConvention(JPY, JPTO), InflationRateSwapLegConvention.of(PriceIndices.JP_CPI_EXF, LAG_3M, PriceIndexCalculationMethod.INTERPOLATED_JAPAN, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, JPTO)), DaysAdjustment.ofBusinessDays(2, JPTO));

	  /// <summary>
	  /// USD vanilla fixed vs US Urban consumers CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention USD_FIXED_ZC_US_CPI = ImmutableFixedInflationSwapConvention.of("USD-FIXED-ZC-US-CPI", fixedLegZcConvention(USD, USNY), InflationRateSwapLegConvention.of(PriceIndices.US_CPI_U, LAG_3M, PriceIndexCalculationMethod.INTERPOLATED, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, USNY)), DaysAdjustment.ofBusinessDays(2, USNY));

	  /// <summary>
	  /// EUR vanilla fixed vs France CPI swap.
	  /// Both legs are zero-coupon; the fixed rate is compounded.
	  /// </summary>
	  public static readonly FixedInflationSwapConvention EUR_FIXED_ZC_FR_CPI = ImmutableFixedInflationSwapConvention.of("EUR-FIXED-ZC-FR-CPI", fixedLegZcConvention(EUR, EUTA), InflationRateSwapLegConvention.of(PriceIndices.FR_EXT_CPI, LAG_3M, PriceIndexCalculationMethod.MONTHLY, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, FRPA)), DaysAdjustment.ofBusinessDays(2, EUTA));

	  // Create a zero-coupon fixed leg convention
	  private static FixedRateSwapLegConvention fixedLegZcConvention(Currency ccy, HolidayCalendarId cal)
	  {
		return FixedRateSwapLegConvention.builder().paymentFrequency(Frequency.TERM).accrualFrequency(Frequency.P12M).accrualBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, cal)).startDateBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, cal)).endDateBusinessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, cal)).compoundingMethod(CompoundingMethod.STRAIGHT).dayCount(ONE_ONE).currency(ccy).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardFixedInflationSwapConventions()
	  {
	  }

	}

}