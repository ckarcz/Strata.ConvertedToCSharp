/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_HICP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.US_CPI_U;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using PriceIndexObservation = com.opengamma.strata.basics.index.PriceIndexObservation;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using LocalDateDoubleTimeSeriesBuilder = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeriesBuilder;
	using ShiftType = com.opengamma.strata.market.ShiftType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InflationNodalCurve = com.opengamma.strata.market.curve.InflationNodalCurve;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using SeasonalityDefinition = com.opengamma.strata.market.curve.SeasonalityDefinition;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using UnitParameterSensitivities = com.opengamma.strata.market.param.UnitParameterSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Tests <seealso cref="SimplePriceIndexValues"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SimplePriceIndexValuesTest
	public class SimplePriceIndexValuesTest
	{

	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 5, 3);
	  private static readonly LocalDate VAL_DATE_2 = LocalDate.of(2015, 2, 6);
	  private static readonly YearMonth VAL_MONTH = YearMonth.of(2015, 5);

	  // USD HICP, CPURNSA Index
	  private static readonly double[] USCPI_VALUES = new double[] {211.143, 212.193, 212.709, 213.240, 213.856, 215.693, 215.351, 215.834, 215.969, 216.177, 216.330, 215.949, 216.687, 216.741, 217.631, 218.009, 218.178, 217.965, 218.011, 218.312, 218.439, 218.711, 218.803, 219.179, 220.223, 221.309, 223.467, 224.906, 225.964, 225.722, 225.922, 226.545, 226.889, 226.421, 226.230, 225.672, 226.655, 227.663, 229.392, 230.085, 229.815, 229.478, 229.104, 230.379, 231.407, 231.317, 230.221, 229.601, 230.280, 232.166, 232.773, 232.531, 232.945, 233.504, 233.596, 233.877, 234.149, 233.546, 233.069, 233.049, 233.916, 234.781, 236.293, 237.072, 237.900, 238.343, 238.250, 237.852, 238.031, 237.433, 236.151, 234.812, 233.707, 234.722, 236.119};
	  private static readonly LocalDate USCPI_START_DATE = LocalDate.of(2009, 1, 31);
	  private static readonly LocalDateDoubleTimeSeries USCPI_TS;
	  static SimplePriceIndexValuesTest()
	  {
		LocalDateDoubleTimeSeriesBuilder builder = LocalDateDoubleTimeSeries.builder();
		for (int i = 0; i < USCPI_VALUES.Length; i++)
		{
		  builder.put(USCPI_START_DATE.plusMonths(i), USCPI_VALUES[i]);
		}
		USCPI_TS = builder.build();
		for (int i = 0; i < TEST_MONTHS.Length; i++)
		{
		  TEST_OBS[i] = PriceIndexObservation.of(US_CPI_U, TEST_MONTHS[i]);
		}
	  }

	  private static readonly DoubleArray TIMES = DoubleArray.of(9.0, 21.0, 57.0, 117.0);
	  private static readonly DoubleArray VALUES = DoubleArray.of(240.500, 245.000, 265.000, 286.000);
	  private static readonly DoubleArray VALUES2 = DoubleArray.of(243.500, 248.000, 268.000, 289.000);
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveName NAME = CurveName.of("USD-HICP");
	  private static readonly CurveMetadata METADATA = Curves.prices(NAME);
	  private static readonly InterpolatedNodalCurve CURVE_NOFIX = InterpolatedNodalCurve.of(METADATA, TIMES, VALUES, INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE2_NOFIX = InterpolatedNodalCurve.of(METADATA, TIMES, VALUES2, INTERPOLATOR);
	  private static readonly DoubleArray SEASONALITY_MULTIPLICATIVE = DoubleArray.of(1.002754153722096, 1.001058905136103, 1.006398754528882, 1.000862459308375, 0.998885402944655, 0.995571243121412, 1.001419845026233, 1.001663068058397, 0.999147014890734, 0.998377467899150, 0.999570726482709, 0.994346721844999);
	  private static readonly SeasonalityDefinition SEASONALITY_DEF = SeasonalityDefinition.of(SEASONALITY_MULTIPLICATIVE, ShiftType.SCALED);
	  private static readonly InflationNodalCurve CURVE_INFL = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE, YearMonth.from(USCPI_TS.LatestDate), USCPI_TS.LatestValue, SEASONALITY_DEF);
	  private static readonly InflationNodalCurve CURVE_INFL2 = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE_2, YearMonth.of(2014, 12), USCPI_TS.get(LocalDate.of(2014, 12, 31)).Value, SEASONALITY_DEF);
	  private static readonly SimplePriceIndexValues INSTANCE = SimplePriceIndexValues.of(US_CPI_U, VAL_DATE, CURVE_INFL, USCPI_TS);
	  private static readonly SimplePriceIndexValues INSTANCE_WITH_FUTFIXING = SimplePriceIndexValues.of(US_CPI_U, VAL_DATE_2, CURVE_INFL2, USCPI_TS);

	  private static readonly YearMonth[] TEST_MONTHS = new YearMonth[] {YearMonth.of(2015, 1), YearMonth.of(2015, 2), YearMonth.of(2015, 5), YearMonth.of(2016, 5), YearMonth.of(2016, 6), YearMonth.of(2024, 12)};
	  private static readonly PriceIndexObservation[] TEST_OBS = new PriceIndexObservation[TEST_MONTHS.Length];
	  private const double TOLERANCE_VALUE = 1.0E-10;
	  private const double TOLERANCE_DELTA = 1.0E-6;

	  //-------------------------------------------------------------------------
	  public virtual void test_of_noSeasonality()
	  {
		SimplePriceIndexValues test = SimplePriceIndexValues.of(US_CPI_U, VAL_DATE, CURVE_NOFIX, USCPI_TS);
		assertEquals(test.Index, US_CPI_U);
		assertEquals(test.ValuationDate, VAL_DATE);
		assertEquals(test.Curve, CURVE_NOFIX);
		assertEquals(test.ParameterCount, CURVE_NOFIX.ParameterCount);
		assertEquals(test.getParameter(0), CURVE_NOFIX.getParameter(0));
		assertEquals(test.getParameterMetadata(0), CURVE_NOFIX.getParameterMetadata(0));
		assertEquals(test.withParameter(0, 1d).Curve, CURVE_NOFIX.withParameter(0, 1d));
		assertEquals(test.withPerturbation((i, v, m) => v + 1d).Curve, CURVE_NOFIX.withPerturbation((i, v, m) => v + 1d));
		assertEquals(test.findData(CURVE_NOFIX.Name), CURVE_NOFIX);
		assertEquals(test.findData(CurveName.of("Rubbish")), null);
		// check PriceIndexValues
		PriceIndexValues test2 = PriceIndexValues.of(US_CPI_U, VAL_DATE, CURVE_NOFIX, USCPI_TS);
		assertEquals(test, test2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_valuePointSensitivity_fixing()
	  {
		SimplePriceIndexValues test = SimplePriceIndexValues.of(US_CPI_U, VAL_DATE, CURVE_NOFIX, USCPI_TS);
		PriceIndexObservation obs = PriceIndexObservation.of(US_CPI_U, VAL_MONTH.minusMonths(3));
		assertEquals(test.valuePointSensitivity(obs), PointSensitivityBuilder.none());
	  }

	  public virtual void test_valuePointSensitivity_forward()
	  {
		YearMonth month = VAL_MONTH.plusMonths(3);
		SimplePriceIndexValues test = SimplePriceIndexValues.of(US_CPI_U, VAL_DATE, CURVE_NOFIX, USCPI_TS);
		PriceIndexObservation obs = PriceIndexObservation.of(US_CPI_U, month);
		InflationRateSensitivity expected = InflationRateSensitivity.of(obs, 1d);
		assertEquals(test.valuePointSensitivity(obs), expected);
	  }

	  //-------------------------------------------------------------------------
	  // proper end-to-end tests are elsewhere
	  public virtual void test_parameterSensitivity()
	  {
		SimplePriceIndexValues test = SimplePriceIndexValues.of(US_CPI_U, VAL_DATE, CURVE_NOFIX, USCPI_TS);
		InflationRateSensitivity point = InflationRateSensitivity.of(PriceIndexObservation.of(US_CPI_U, VAL_MONTH.plusMonths(3)), 1d);
		assertEquals(test.parameterSensitivity(point).size(), 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createParameterSensitivity()
	  {
		SimplePriceIndexValues test = SimplePriceIndexValues.of(US_CPI_U, VAL_DATE, CURVE_NOFIX, USCPI_TS);
		DoubleArray sensitivities = DoubleArray.of(0.12, 0.15, 0.16, 0.17);
		CurrencyParameterSensitivities sens = test.createParameterSensitivity(USD, sensitivities);
		assertEquals(sens.Sensitivities.get(0), CURVE_NOFIX.createParameterSensitivity(USD, sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurve()
	  {
		SimplePriceIndexValues test = SimplePriceIndexValues.of(US_CPI_U, VAL_DATE, CURVE_NOFIX, USCPI_TS).withCurve(CURVE2_NOFIX);
		assertEquals(test.Curve, CURVE2_NOFIX);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parameter_count()
	  {
		assertEquals(INSTANCE.ParameterCount, CURVE_NOFIX.ParameterCount);
	  }

	  public virtual void test_parameter()
	  {
		assertEquals(INSTANCE.getParameter(2), CURVE_NOFIX.getParameter(2));
	  }

	  public virtual void test_parameter_metadata()
	  {
		assertEquals(INSTANCE.getParameterMetadata(2), CURVE_NOFIX.getParameterMetadata(2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_value()
	  {
		for (int i = 0; i < TEST_MONTHS.Length; i++)
		{
		  double valueComputed = INSTANCE.value(TEST_OBS[i]);
		  YearMonth fixingMonth = TEST_OBS[i].FixingMonth;
		  double valueExpected;
		  if (USCPI_TS.containsDate(fixingMonth.atEndOfMonth()))
		  {
			valueExpected = USCPI_TS.get(fixingMonth.atEndOfMonth()).Value;
		  }
		  else
		  {
			double x = YearMonth.from(VAL_DATE).until(fixingMonth, MONTHS);
			valueExpected = CURVE_INFL.yValue(x);
		  }
		  assertEquals(valueComputed, valueExpected, TOLERANCE_VALUE, "test " + i);
		}
	  }

	  /* Test values when a fixing in the futures in present in the TS. */
	  public virtual void test_value_futfixing()
	  {
		for (int i = 0; i < TEST_MONTHS.Length; i++)
		{
		  double valueComputed = INSTANCE_WITH_FUTFIXING.value(TEST_OBS[i]);
		  YearMonth fixingMonth = TEST_OBS[i].FixingMonth;
		  double valueExpected;
		  if (fixingMonth.isBefore(YearMonth.from(VAL_DATE_2)) && USCPI_TS.containsDate(fixingMonth.atEndOfMonth()))
		  {
			valueExpected = USCPI_TS.get(fixingMonth.atEndOfMonth()).Value;
		  }
		  else
		  {
			double x = YearMonth.from(VAL_DATE_2).until(fixingMonth, MONTHS);
			valueExpected = CURVE_INFL2.yValue(x);
		  }
		  assertEquals(valueComputed, valueExpected, TOLERANCE_VALUE, "test " + i);
		}
	  }

	  public virtual void test_value_pts_sensitivity()
	  {
		for (int i = 0; i < TEST_MONTHS.Length; i++)
		{
		  PointSensitivityBuilder ptsComputed = INSTANCE.valuePointSensitivity(TEST_OBS[i]);
		  YearMonth fixingMonth = TEST_OBS[i].FixingMonth;
		  PointSensitivityBuilder ptsExpected;
		  if (USCPI_TS.containsDate(fixingMonth.atEndOfMonth()))
		  {
			ptsExpected = PointSensitivityBuilder.none();
		  }
		  else
		  {
			ptsExpected = InflationRateSensitivity.of(TEST_OBS[i], 1d);
		  }
		  assertTrue(ptsComputed.build().equalWithTolerance(ptsExpected.build(), TOLERANCE_VALUE), "test " + i);
		}
	  }

	  public virtual void test_value_pts_sensitivity_futfixing()
	  {
		for (int i = 0; i < TEST_MONTHS.Length; i++)
		{
		  PointSensitivityBuilder ptsComputed = INSTANCE_WITH_FUTFIXING.valuePointSensitivity(TEST_OBS[i]);
		  YearMonth fixingMonth = TEST_OBS[i].FixingMonth;
		  PointSensitivityBuilder ptsExpected;
		  if (fixingMonth.isBefore(YearMonth.from(VAL_DATE_2)) && USCPI_TS.containsDate(fixingMonth.atEndOfMonth()))
		  {
			ptsExpected = PointSensitivityBuilder.none();
		  }
		  else
		  {
			ptsExpected = InflationRateSensitivity.of(TEST_OBS[i], 1d);
		  }
		  assertTrue(ptsComputed.build().equalWithTolerance(ptsExpected.build(), TOLERANCE_VALUE), "test " + i);
		}
	  }

	  public virtual void test_value_parameter_sensitivity()
	  {
		for (int i = 0; i < TEST_MONTHS.Length; i++)
		{
		  YearMonth fixingMonth = TEST_OBS[i].FixingMonth;
		  if (!USCPI_TS.containsDate(fixingMonth.atEndOfMonth()))
		  {
			InflationRateSensitivity ptsExpected = (InflationRateSensitivity) InflationRateSensitivity.of(TEST_OBS[i], 1d);
			CurrencyParameterSensitivities psComputed = INSTANCE.parameterSensitivity(ptsExpected);
			double x = YearMonth.from(VAL_DATE).until(fixingMonth, MONTHS);
			UnitParameterSensitivities sens1 = UnitParameterSensitivities.of(CURVE_INFL.yValueParameterSensitivity(x));
			CurrencyParameterSensitivities psExpected = sens1.multipliedBy(ptsExpected.Currency, ptsExpected.Sensitivity);
			assertTrue(psComputed.equalWithTolerance(psExpected, TOLERANCE_DELTA), "test " + i);
		  }
		}
	  }

	  public virtual void test_value_parameter_sensitivity_futfixing()
	  {
		for (int i = 0; i < TEST_MONTHS.Length; i++)
		{
		  YearMonth fixingMonth = TEST_OBS[i].FixingMonth;
		  if (!fixingMonth.isBefore(YearMonth.from(VAL_DATE_2)) && !USCPI_TS.containsDate(fixingMonth.atEndOfMonth()))
		  {
			InflationRateSensitivity ptsExpected = (InflationRateSensitivity) InflationRateSensitivity.of(TEST_OBS[i], 1d);
			CurrencyParameterSensitivities psComputed = INSTANCE_WITH_FUTFIXING.parameterSensitivity(ptsExpected);
			double x = YearMonth.from(VAL_DATE_2).until(fixingMonth, MONTHS);
			UnitParameterSensitivities sens1 = UnitParameterSensitivities.of(CURVE_INFL2.yValueParameterSensitivity(x));
			CurrencyParameterSensitivities psExpected = sens1.multipliedBy(ptsExpected.Currency, ptsExpected.Sensitivity);
			assertTrue(psComputed.equalWithTolerance(psExpected, TOLERANCE_DELTA), "test " + i);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SimplePriceIndexValues instance1 = SimplePriceIndexValues.of(US_CPI_U, VAL_DATE, CURVE_NOFIX, USCPI_TS);
		coverImmutableBean(instance1);
		SimplePriceIndexValues test2 = SimplePriceIndexValues.of(GB_HICP, VAL_DATE.plusMonths(1), CURVE_NOFIX, LocalDateDoubleTimeSeries.of(VAL_MONTH.minusMonths(2).atEndOfMonth(), 100d));
		coverBeanEquals(instance1, test2);
	  }

	}

}