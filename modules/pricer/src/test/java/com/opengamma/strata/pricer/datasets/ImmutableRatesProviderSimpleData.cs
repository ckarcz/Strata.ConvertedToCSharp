/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.datasets
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;

	/// <summary>
	/// Simple instances of ImmutableRateProvider to be used in tests.
	/// </summary>
	public class ImmutableRatesProviderSimpleData
	{

	  public static readonly LocalDate VAL_DATE = LocalDate.of(2014, 1, 16);

	  public static readonly ImmutableRatesProvider IMM_PROV_EUR_NOFIX;
	  public static readonly ImmutableRatesProvider IMM_PROV_EUR_FIX;
	  static ImmutableRatesProviderSimpleData()
	  {
		CurveInterpolator interp = CurveInterpolators.DOUBLE_QUADRATIC;
		DoubleArray time_eur = DoubleArray.of(0.0, 0.1, 0.25, 0.5, 0.75, 1.0, 2.0);
		DoubleArray rate_eur = DoubleArray.of(0.0160, 0.0165, 0.0155, 0.0155, 0.0155, 0.0150, 0.0140);
		InterpolatedNodalCurve dscCurve = InterpolatedNodalCurve.of(Curves.zeroRates("EUR-Discount", ACT_365F), time_eur, rate_eur, interp);
		DoubleArray time_index = DoubleArray.of(0.0, 0.25, 0.5, 1.0);
		DoubleArray rate_index = DoubleArray.of(0.0180, 0.0180, 0.0175, 0.0165);
		InterpolatedNodalCurve indexCurve = InterpolatedNodalCurve.of(Curves.zeroRates("EUR-EURIBOR6M", ACT_365F), time_index, rate_index, interp);
		IMM_PROV_EUR_NOFIX = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(EUR, dscCurve).iborIndexCurve(EUR_EURIBOR_6M, indexCurve).build();
		LocalDateDoubleTimeSeries tsE6 = LocalDateDoubleTimeSeries.builder().put(VAL_DATE, 0.012345).build();
		IMM_PROV_EUR_FIX = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(EUR, dscCurve).iborIndexCurve(EUR_EURIBOR_6M, indexCurve, tsE6).build();
	  }

	}

}