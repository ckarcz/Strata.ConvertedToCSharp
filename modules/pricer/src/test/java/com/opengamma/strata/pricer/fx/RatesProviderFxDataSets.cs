/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// Sets of market data used in FX tests.
	/// </summary>
	public class RatesProviderFxDataSets
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  /// <summary>
	  /// Wednesday. </summary>
	  public static readonly LocalDate VAL_DATE_2014_01_22 = RatesProviderDataSets.VAL_DATE_2014_01_22;

	  private static readonly Currency KRW = Currency.of("KRW");
	  private const double EUR_USD = 1.40;
	  private const double USD_KRW = 1111.11;
	  private const double GBP_USD = 1.50;
	  private static readonly FxMatrix FX_MATRIX = FxMatrix.builder().addRate(EUR, USD, EUR_USD).addRate(KRW, USD, 1.0 / USD_KRW).addRate(GBP, USD, GBP_USD).build();

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly DoubleArray USD_DSC_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0);
	  private static readonly DoubleArray USD_DSC_RATE = DoubleArray.of(0.0100, 0.0120, 0.0120, 0.0140, 0.0140);
	  private static readonly DoubleArray USD_DSC_RATE_FLAT = DoubleArray.of(0.0110, 0.0110, 0.0110, 0.0110, 0.0110);
	  private static readonly CurveMetadata USD_DSC_METADATA = Curves.zeroRates("USD Dsc", ACT_360);
	  private static readonly InterpolatedNodalCurve USD_DSC = InterpolatedNodalCurve.of(USD_DSC_METADATA, USD_DSC_TIME, USD_DSC_RATE, INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve USD_DSC_FLAT = InterpolatedNodalCurve.of(USD_DSC_METADATA, USD_DSC_TIME, USD_DSC_RATE_FLAT, INTERPOLATOR);
	  private static readonly CurveMetadata USD_DSC_METADATA_ISDA = Curves.zeroRates("USD Dsc", ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve USD_DSC_ISDA = InterpolatedNodalCurve.of(USD_DSC_METADATA_ISDA, USD_DSC_TIME, USD_DSC_RATE, INTERPOLATOR);

	  private static readonly DoubleArray EUR_DSC_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0);
	  private static readonly DoubleArray EUR_DSC_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0150);
	  private static readonly DoubleArray EUR_DSC_RATE_FLAT = DoubleArray.of(0.0150, 0.0150, 0.0150, 0.0150, 0.0150);
	  private static readonly CurveMetadata EUR_DSC_METADATA = Curves.zeroRates("EUR Dsc", ACT_360);
	  private static readonly InterpolatedNodalCurve EUR_DSC = InterpolatedNodalCurve.of(EUR_DSC_METADATA, EUR_DSC_TIME, EUR_DSC_RATE, INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve EUR_DSC_FLAT = InterpolatedNodalCurve.of(EUR_DSC_METADATA, EUR_DSC_TIME, EUR_DSC_RATE_FLAT, INTERPOLATOR);
	  private static readonly CurveMetadata EUR_DSC_METADATA_ISDA = Curves.zeroRates("EUR Dsc", ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve EUR_DSC_ISDA = InterpolatedNodalCurve.of(EUR_DSC_METADATA_ISDA, EUR_DSC_TIME, EUR_DSC_RATE, INTERPOLATOR);

	  private static readonly DoubleArray GBP_DSC_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0);
	  private static readonly DoubleArray GBP_DSC_RATE = DoubleArray.of(0.0160, 0.0135, 0.0160, 0.0185, 0.0160);
	  private static readonly CurveMetadata GBP_DSC_METADATA = Curves.zeroRates("GBP Dsc", ACT_360);
	  private static readonly InterpolatedNodalCurve GBP_DSC = InterpolatedNodalCurve.of(GBP_DSC_METADATA, GBP_DSC_TIME, GBP_DSC_RATE, INTERPOLATOR);

	  private static readonly DoubleArray KRW_DSC_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0);
	  private static readonly DoubleArray KRW_DSC_RATE = DoubleArray.of(0.0350, 0.0325, 0.0350, 0.0375, 0.0350);
	  private static readonly CurveMetadata KRW_DSC_METADATA = Curves.zeroRates("KRW Dsc", ACT_360);
	  private static readonly InterpolatedNodalCurve KRW_DSC = InterpolatedNodalCurve.of(KRW_DSC_METADATA, KRW_DSC_TIME, KRW_DSC_RATE, INTERPOLATOR);

	  /// <summary>
	  /// Create a yield curve bundle with three curves.
	  /// One called "Discounting EUR" with a constant rate of 2.50%, one called "Discounting USD"
	  /// with a constant rate of 1.00% and one called "Discounting GBP" with a constant rate of 2.00%;
	  /// "Discounting KRW" with a constant rate of 3.21%;
	  /// </summary>
	  /// <returns> the provider </returns>
	  public static RatesProvider createProvider()
	  {
		return ImmutableRatesProvider.builder(VAL_DATE_2014_01_22).discountCurve(EUR, EUR_DSC).discountCurve(USD, USD_DSC).discountCurve(GBP, GBP_DSC).discountCurve(KRW, KRW_DSC).fxRateProvider(FX_MATRIX).build();
	  }

	  /// <summary>
	  /// Create a yield curve bundle with three curves.
	  /// One called "Discounting EUR" with a constant rate of 2.50%, one called "Discounting USD"
	  /// with a constant rate of 1.00% and one called "Discounting GBP" with a constant rate of 2.00%;
	  /// "Discounting KRW" with a constant rate of 3.21%;
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="fxIndex">  the FX index </param>
	  /// <param name="spotRate">  the spot rate for the index </param>
	  /// <returns> the provider </returns>
	  public static RatesProvider createProvider(LocalDate valuationDate, FxIndex fxIndex, double spotRate)
	  {
		return ImmutableRatesProvider.builder(valuationDate).discountCurve(EUR, EUR_DSC).discountCurve(USD, USD_DSC).discountCurve(GBP, GBP_DSC).discountCurve(KRW, KRW_DSC).fxRateProvider(FX_MATRIX).timeSeries(fxIndex, LocalDateDoubleTimeSeries.of(fxIndex.calculateFixingFromMaturity(valuationDate, REF_DATA), spotRate)).build();
	  }

	  /// <summary>
	  /// Creates rates provider for EUR, USD with FX matrix.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the rates provider </returns>
	  public static ImmutableRatesProvider createProviderEURUSD(LocalDate valuationDate)
	  {
		FxMatrix fxMatrix = FxMatrix.builder().addRate(USD, EUR, 1.0d / EUR_USD).build();
		return ImmutableRatesProvider.builder(valuationDate).discountCurve(EUR, EUR_DSC).discountCurve(USD, USD_DSC).fxRateProvider(fxMatrix).build();
	  }

	  /// <summary>
	  /// Creates rates provider for EUR, USD with FX matrix.
	  /// <para>
	  /// The discount curves are based on the day count convention, ACT/ACT ISDA.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the rates provider </returns>
	  public static ImmutableRatesProvider createProviderEurUsdActActIsda(LocalDate valuationDate)
	  {
		FxMatrix fxMatrix = FxMatrix.builder().addRate(USD, EUR, 1.0d / EUR_USD).build();
		return ImmutableRatesProvider.builder(valuationDate).discountCurve(EUR, EUR_DSC_ISDA).discountCurve(USD, USD_DSC_ISDA).fxRateProvider(fxMatrix).build();
	  }

	  /// <summary>
	  /// Creates rates provider for EUR, USD with FX matrix.
	  /// <para>
	  /// The discount curves are flat.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the rates provider </returns>
	  public static ImmutableRatesProvider createProviderEurUsdFlat(LocalDate valuationDate)
	  {
		FxMatrix fxMatrix = FxMatrix.builder().addRate(USD, EUR, 1.0d / EUR_USD).build();
		return ImmutableRatesProvider.builder(valuationDate).discountCurve(EUR, EUR_DSC_FLAT).discountCurve(USD, USD_DSC_FLAT).fxRateProvider(fxMatrix).build();
	  }

	  /// <summary>
	  /// Get the curve name of the curve for a given currency.
	  /// </summary>
	  /// <param name="currency"> the currency </param>
	  /// <returns> the curve name </returns>
	  public static CurveName getCurveName(Currency currency)
	  {
		if (currency.Equals(EUR))
		{
		  return EUR_DSC.Name;
		}
		if (currency.Equals(USD))
		{
		  return USD_DSC.Name;
		}
		if (currency.Equals(GBP))
		{
		  return GBP_DSC.Name;
		}
		throw new System.ArgumentException();
	  }

	  /// <summary>
	  /// Gets the FX matrix.
	  /// </summary>
	  /// <returns> the FX matrix </returns>
	  public static FxMatrix fxMatrix()
	  {
		return FX_MATRIX;
	  }

	}

}