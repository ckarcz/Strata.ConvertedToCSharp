/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P12M;

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;

	/// <summary>
	/// Market standard three leg basis swap conventions.
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	internal sealed class StandardThreeLegBasisSwapConventions
	{

	  /// <summary>
	  /// EUR three leg basis swap of fixed, Euribor 3M and Euribor 6M.
	  /// The fixed leg pays yearly with day count '30U/360'.
	  /// </summary>
	  public static readonly ThreeLegBasisSwapConvention EUR_FIXED_1Y_EURIBOR_3M_EURIBOR_6M = ImmutableThreeLegBasisSwapConvention.of("EUR-FIXED-1Y-EURIBOR-3M-EURIBOR-6M", FixedRateSwapLegConvention.of(EUR, THIRTY_U_360, P12M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)), IborRateSwapLegConvention.of(IborIndices.EUR_EURIBOR_3M), IborRateSwapLegConvention.of(IborIndices.EUR_EURIBOR_6M));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardThreeLegBasisSwapConventions()
	  {
	  }

	}

}