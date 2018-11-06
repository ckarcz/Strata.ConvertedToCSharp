/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.GBP_USD_WM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_RPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;

	using Bean = org.joda.beans.Bean;
	using JodaBeanSer = org.joda.beans.ser.JodaBeanSer;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using DiscountFxForwardRates = com.opengamma.strata.pricer.fx.DiscountFxForwardRates;

	/// <summary>
	/// Test <seealso cref="ImmutableRatesProvider"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ImmutableRatesProviderTest
	public class ImmutableRatesProviderTest
	{

	  private static readonly LocalDate PREV_DATE = LocalDate.of(2014, 6, 27);
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2014, 6, 30);
	  private const double FX_GBP_USD = 1.6d;
	  private static readonly FxMatrix FX_MATRIX = FxMatrix.of(GBP, USD, FX_GBP_USD);
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;

	  private const double GBP_DSC = 0.99d;
	  private const double USD_DSC = 0.95d;
	  private static readonly Curve DISCOUNT_CURVE_GBP = ConstantCurve.of(Curves.zeroRates("GBP-Discount", ACT_ACT_ISDA), GBP_DSC);
	  private static readonly Curve DISCOUNT_CURVE_USD = ConstantCurve.of(Curves.zeroRates("USD-Discount", ACT_ACT_ISDA), USD_DSC);
	  private static readonly Curve USD_LIBOR_CURVE = ConstantCurve.of(Curves.zeroRates("USD-Discount", ACT_ACT_ISDA), 0.96d);
	  private static readonly Curve FED_FUND_CURVE = ConstantCurve.of(Curves.zeroRates("USD-Discount", ACT_ACT_ISDA), 0.97d);
	  private static readonly Curve GBPRI_CURVE = InterpolatedNodalCurve.of(Curves.prices("GB-RPI"), DoubleArray.of(1d, 10d), DoubleArray.of(252d, 252d), INTERPOLATOR);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.of(PREV_DATE, 0.62d);
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).timeSeries(GBP_USD_WM, ts).build();
		assertEquals(test.ValuationDate, VAL_DATE);
		assertEquals(ImmutableRatesProvider.meta().timeSeries().get(test), ImmutableMap.of(GBP_USD_WM, ts));
		assertSame(test.toImmutableRatesProvider(), test);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_discountFactors()
	  {
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		assertEquals(test.discountFactors(GBP).Currency, GBP);
	  }

	  public virtual void test_discountFactors_notKnown()
	  {
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).build();
		assertThrowsIllegalArg(() => test.discountFactors(GBP));
		assertThrowsIllegalArg(() => test.discountFactor(GBP, LocalDate.of(2014, 7, 30)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_fxRate_separate()
	  {
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).fxRateProvider(FX_MATRIX).build();
		assertEquals(test.fxRate(USD, GBP), 1 / FX_GBP_USD, 0d);
		assertEquals(test.fxRate(USD, USD), 1d, 0d);
	  }

	  public virtual void test_fxRate_pair()
	  {
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).fxRateProvider(FX_MATRIX).build();
		assertEquals(test.fxRate(CurrencyPair.of(USD, GBP)), 1 / FX_GBP_USD, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_fxIndexRates()
	  {
		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.of(VAL_DATE, 0.62d);
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).fxRateProvider(FX_MATRIX).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).timeSeries(GBP_USD_WM, ts).build();
		assertEquals(test.fxIndexRates(GBP_USD_WM).Index, GBP_USD_WM);
		assertEquals(test.fxIndexRates(GBP_USD_WM).Fixings, ts);
		assertEquals(test.TimeSeriesIndices, ImmutableSet.of(GBP_USD_WM));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_fxForwardRates()
	  {
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).fxRateProvider(FX_MATRIX).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		DiscountFxForwardRates res = (DiscountFxForwardRates) test.fxForwardRates(CurrencyPair.of(GBP, USD));
		assertEquals(res.BaseCurrencyDiscountFactors, ZeroRateDiscountFactors.of(GBP, VAL_DATE, DISCOUNT_CURVE_GBP));
		assertEquals(res.CounterCurrencyDiscountFactors, ZeroRateDiscountFactors.of(USD, VAL_DATE, DISCOUNT_CURVE_USD));
		assertEquals(res.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(res.FxRateProvider, FX_MATRIX);
		assertEquals(res.ValuationDate, VAL_DATE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_iborIndexRates()
	  {
		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.of(VAL_DATE, 0.62d);
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).iborIndexCurve(USD_LIBOR_3M, USD_LIBOR_CURVE).timeSeries(USD_LIBOR_3M, ts).build();
		assertEquals(test.iborIndexRates(USD_LIBOR_3M).Index, USD_LIBOR_3M);
		assertEquals(test.iborIndexRates(USD_LIBOR_3M).Fixings, ts);
		assertEquals(test.IborIndices, ImmutableSet.of(USD_LIBOR_3M));
		assertEquals(test.TimeSeriesIndices, ImmutableSet.of(USD_LIBOR_3M));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_overnightIndexRates()
	  {
		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.of(VAL_DATE, 0.62d);
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).overnightIndexCurve(USD_FED_FUND, FED_FUND_CURVE).timeSeries(USD_FED_FUND, ts).build();
		assertEquals(test.overnightIndexRates(USD_FED_FUND).Index, USD_FED_FUND);
		assertEquals(test.overnightIndexRates(USD_FED_FUND).Fixings, ts);
		assertEquals(test.OvernightIndices, ImmutableSet.of(USD_FED_FUND));
		assertEquals(test.TimeSeriesIndices, ImmutableSet.of(USD_FED_FUND));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_priceIndexValues()
	  {
		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.of(VAL_DATE, 0.62d);
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).priceIndexCurve(GB_RPI, GBPRI_CURVE).timeSeries(GB_RPI, ts).build();
		assertEquals(test.priceIndexValues(GB_RPI).Index, GB_RPI);
		assertEquals(test.priceIndexValues(GB_RPI).Fixings, ts);
		assertEquals(test.PriceIndices, ImmutableSet.of(GB_RPI));
		assertEquals(test.TimeSeriesIndices, ImmutableSet.of(GB_RPI));
	  }

	  public virtual void test_priceIndexValues_notKnown()
	  {
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).build();
		assertThrowsIllegalArg(() => test.priceIndexValues(GB_RPI));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_getCurves()
	  {
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		assertEquals(test.Curves.Count, 2);
		assertEquals(test.Curves[DISCOUNT_CURVE_GBP.Name], DISCOUNT_CURVE_GBP);
		assertEquals(test.Curves[DISCOUNT_CURVE_USD.Name], DISCOUNT_CURVE_USD);
	  }

	  public virtual void test_getCurves_withGroup()
	  {
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(GBP, DISCOUNT_CURVE_GBP).discountCurve(USD, DISCOUNT_CURVE_USD).build();
		CurveGroupName group = CurveGroupName.of("GRP");
		assertEquals(test.getCurves(group).Count, 2);
		assertEquals(test.getCurves(group)[CurveId.of(group, DISCOUNT_CURVE_GBP.Name)], DISCOUNT_CURVE_GBP);
		assertEquals(test.getCurves(group)[CurveId.of(group, DISCOUNT_CURVE_USD.Name)], DISCOUNT_CURVE_USD);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableRatesProvider test = ImmutableRatesProvider.builder(VAL_DATE).build();
		coverImmutableBean(test);
		ImmutableRatesProvider test2 = ImmutableRatesProvider.builder(LocalDate.of(2014, 6, 27)).discountCurve(GBP, DISCOUNT_CURVE_GBP).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void testSerializeDeserialize()
	  {
		cycleBean(ImmutableRatesProvider.builder(VAL_DATE).build());
	  }

	  private void cycleBean(Bean bean)
	  {
		JodaBeanSer ser = JodaBeanSer.COMPACT;
		string result = ser.xmlWriter().write(bean);
		Bean cycled = ser.xmlReader().read(result);
		assertThat(cycled).isEqualTo(bean);
	  }


	}

}