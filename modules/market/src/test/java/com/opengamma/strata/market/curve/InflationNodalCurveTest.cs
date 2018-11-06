/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// Test <seealso cref="InflationNodalCurve"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InflationNodalCurveTest
	public class InflationNodalCurveTest
	{

	  private static readonly LocalDate VAL_DATE_1 = LocalDate.of(2016, 1, 1);
	  private static readonly LocalDate VAL_DATE_2 = LocalDate.of(2016, 10, 21);

	  private static readonly DoubleArray TIMES = DoubleArray.of(9.0, 21.0, 57.0, 117.0);
	  private static readonly DoubleArray VALUES = DoubleArray.of(240.500, 245.000, 265.000, 286.000);
	  private static readonly DoubleArray VALUES2 = DoubleArray.of(243.500, 248.000, 268.000, 289.000);
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveName NAME = CurveName.of("USD-HICP");
	  private static readonly CurveMetadata METADATA = Curves.prices(NAME);
	  private static readonly InterpolatedNodalCurve CURVE_NOFIX = InterpolatedNodalCurve.of(METADATA, TIMES, VALUES, INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE2_NOFIX = InterpolatedNodalCurve.of(METADATA, TIMES, VALUES2, INTERPOLATOR);

	  private static readonly YearMonth LAST_FIX_MONTH_1 = YearMonth.of(2015, 11);
	  private static readonly YearMonth LAST_FIX_MONTH_2 = YearMonth.of(2016, 7);
	  private const double LAST_FIX_VALUE = 240.00;

	  private static readonly DoubleArray SEASONALITY_MULTIPLICATIVE = DoubleArray.of(1.002754153722096, 1.001058905136103, 1.006398754528882, 1.000862459308375, 0.998885402944655, 0.995571243121412, 1.001419845026233, 1.001663068058397, 0.999147014890734, 0.998377467899150, 0.999570726482709, 0.994346721844999);
	  private static readonly SeasonalityDefinition SEASONALITY_MULTIPLICATIVE_DEF = SeasonalityDefinition.of(SEASONALITY_MULTIPLICATIVE, ShiftType.SCALED);
	  private static readonly DoubleArray SEASONALITY_ADDITIVE = DoubleArray.of(1.0, 1.5, 1.0, -0.5, -0.5, -1.0, -1.5, 0.0, 0.5, 1.0, 1.0, -2.5);
	  private static readonly SeasonalityDefinition SEASONALITY_ADDITIVE_DEF = SeasonalityDefinition.of(SEASONALITY_ADDITIVE, ShiftType.ABSOLUTE);

	  private static readonly double NB_MONTHS_1 = YearMonth.from(VAL_DATE_1).until(LAST_FIX_MONTH_1, MONTHS);
	  private static readonly double NB_MONTHS_2 = YearMonth.from(VAL_DATE_2).until(LAST_FIX_MONTH_2, MONTHS);
	  private static readonly NodalCurve EXTENDED_CURVE_2 = CURVE_NOFIX.withNode(NB_MONTHS_2, LAST_FIX_VALUE, ParameterMetadata.empty());
	  private static readonly DoubleArray SEASONALITY_MULTIPLICATIVE_COMP_2 = seasonalityCompounded(VAL_DATE_2, LAST_FIX_MONTH_2, SEASONALITY_MULTIPLICATIVE, (v, a) => v * a);

	  private static readonly YearMonth[] TEST_MONTHS = new YearMonth[] {YearMonth.of(2016, 7), YearMonth.of(2016, 8), YearMonth.of(2016, 9), YearMonth.of(2016, 10), YearMonth.of(2017, 1), YearMonth.of(2017, 6), YearMonth.of(2017, 7), YearMonth.of(2017, 8), YearMonth.of(2018, 9), YearMonth.of(2025, 12)};

	  private const double TOLERANCE_TIME = 1.0E-10;
	  private const double TOLERANCE_VALUE = 1.0E-10;
	  private const double TOLERANCE_DELTA = 1.0E-8;

	  public virtual void of_construction_multiplicative_1()
	  {
		InflationNodalCurve curveComputed = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE_1, LAST_FIX_MONTH_1, LAST_FIX_VALUE, SEASONALITY_MULTIPLICATIVE_DEF);
		assertEquals(curveComputed.Underlying.XValues.get(0), NB_MONTHS_1, TOLERANCE_TIME);
		assertEquals(curveComputed.Underlying.YValues.get(0), LAST_FIX_VALUE, TOLERANCE_TIME);
		assertEquals(curveComputed.yValue(NB_MONTHS_1), LAST_FIX_VALUE, TOLERANCE_TIME);
	  }

	  public virtual void of_construction_multiplicative_2()
	  {
		InflationNodalCurve curveComputed = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE_2, LAST_FIX_MONTH_2, LAST_FIX_VALUE, SEASONALITY_MULTIPLICATIVE_DEF);
		assertEquals(curveComputed.Underlying.XValues.get(0), NB_MONTHS_2, TOLERANCE_TIME);
		assertEquals(curveComputed.Underlying.YValues.get(0), LAST_FIX_VALUE, TOLERANCE_TIME);
		assertEquals(curveComputed.yValue(NB_MONTHS_2), LAST_FIX_VALUE, TOLERANCE_TIME);
	  }

	  public virtual void of_construction_additive_1()
	  {
		InflationNodalCurve curveComputed = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE_1, LAST_FIX_MONTH_1, LAST_FIX_VALUE, SEASONALITY_ADDITIVE_DEF);
		assertEquals(curveComputed.Underlying.XValues.get(0), NB_MONTHS_1, TOLERANCE_TIME);
		assertEquals(curveComputed.Underlying.YValues.get(0), LAST_FIX_VALUE, TOLERANCE_TIME);
		assertEquals(curveComputed.yValue(NB_MONTHS_1), LAST_FIX_VALUE, TOLERANCE_TIME);
	  }

	  public virtual void value_multiplicative()
	  {
		InflationNodalCurve curveComputed = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE_2, LAST_FIX_MONTH_2, LAST_FIX_VALUE, SEASONALITY_MULTIPLICATIVE_DEF);
		for (int i = 1; i < TEST_MONTHS.Length; i++)
		{
		  double nbMonths = YearMonth.from(VAL_DATE_2).until(TEST_MONTHS[i], MONTHS);
		  double valueComputed = curveComputed.yValue(nbMonths);
		  int x = (int)((nbMonths + 12) % 12);
		  double valueNoAdj = EXTENDED_CURVE_2.yValue(nbMonths);
		  double adj = SEASONALITY_MULTIPLICATIVE_COMP_2.get(x);
		  double valueExpected = valueNoAdj * adj;
		  assertEquals(valueExpected, valueComputed, TOLERANCE_VALUE);
		}
	  }

	  public virtual void value_additive()
	  {
		InflationNodalCurve curveComputed = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE_2, LAST_FIX_MONTH_2, LAST_FIX_VALUE, SEASONALITY_ADDITIVE_DEF);
		for (int i = 1; i < TEST_MONTHS.Length; i++)
		{
		  double nbMonths = YearMonth.from(VAL_DATE_2).until(TEST_MONTHS[i], MONTHS);
		  double valueComputed = curveComputed.yValue(nbMonths);
		  int x = (int)((nbMonths + 12) % 12);
		  double valueNoAdj = EXTENDED_CURVE_2.yValue(nbMonths);
		  DoubleArray seasonalityAdditiveCompounded = seasonalityCompounded(VAL_DATE_2, LAST_FIX_MONTH_2, SEASONALITY_ADDITIVE, (v, a) => v + a);
		  double adj = seasonalityAdditiveCompounded.get(x);
		  double valueExpected = valueNoAdj + adj;
		  assertEquals(valueExpected, valueComputed, TOLERANCE_VALUE);
		}
	  }

	  public virtual void parameter_sensitivity_multiplicative()
	  {
		InflationNodalCurve curve = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE_2, LAST_FIX_MONTH_2, LAST_FIX_VALUE, SEASONALITY_MULTIPLICATIVE_DEF);
		double shift = 1.0E-2;
		for (int i = 1; i < TEST_MONTHS.Length; i++)
		{
		  double nbMonths = YearMonth.from(VAL_DATE_2).until(TEST_MONTHS[i], MONTHS);
		  UnitParameterSensitivity psComputed = curve.yValueParameterSensitivity(nbMonths);
		  for (int j = 0; j < TIMES.size(); j++)
		  {
			double[] valuePM = new double[2];
			for (int pm = 0; pm < 2; pm++)
			{
			  DoubleArray shiftedValues = VALUES.with(j, VALUES.get(j) + (1 - 2 * pm) * shift);
			  InterpolatedNodalCurve intCurveShifted = InterpolatedNodalCurve.of(METADATA, TIMES, shiftedValues, INTERPOLATOR);
			  InflationNodalCurve seaCurveShifted = InflationNodalCurve.of(intCurveShifted, VAL_DATE_2, LAST_FIX_MONTH_2, LAST_FIX_VALUE, SEASONALITY_MULTIPLICATIVE_DEF);
			  valuePM[pm] = seaCurveShifted.yValue(nbMonths);
			}
			assertEquals(psComputed.Sensitivity.get(j), (valuePM[0] - valuePM[1]) / (2 * shift), TOLERANCE_DELTA);
		  }
		}
	  }

	  public virtual void parameter_sensitivity_additive()
	  {
		InflationNodalCurve curve = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE_2, LAST_FIX_MONTH_2, LAST_FIX_VALUE, SEASONALITY_ADDITIVE_DEF);
		double shift = 1.0E-2;
		for (int i = 1; i < TEST_MONTHS.Length; i++)
		{
		  double nbMonths = YearMonth.from(VAL_DATE_2).until(TEST_MONTHS[i], MONTHS);
		  UnitParameterSensitivity psComputed = curve.yValueParameterSensitivity(nbMonths);
		  for (int j = 0; j < TIMES.size(); j++)
		  {
			double[] valuePM = new double[2];
			for (int pm = 0; pm < 2; pm++)
			{
			  DoubleArray shiftedValues = VALUES.with(j, VALUES.get(j) + (1 - 2 * pm) * shift);
			  InterpolatedNodalCurve intCurveShifted = InterpolatedNodalCurve.of(METADATA, TIMES, shiftedValues, INTERPOLATOR);
			  InflationNodalCurve seaCurveShifted = InflationNodalCurve.of(intCurveShifted, VAL_DATE_2, LAST_FIX_MONTH_2, LAST_FIX_VALUE, SEASONALITY_ADDITIVE_DEF);
			  valuePM[pm] = seaCurveShifted.yValue(nbMonths);
			}
			assertEquals(psComputed.Sensitivity.get(j), (valuePM[0] - valuePM[1]) / (2 * shift), TOLERANCE_DELTA);
		  }
		}
	  }

	  private static DoubleArray seasonalityCompounded(LocalDate valDate, YearMonth fixingMonth, DoubleArray seasonality, System.Func<double, double, double> adjustmentFunction)
	  {
		double nbMonths = YearMonth.from(valDate).until(fixingMonth, MONTHS);
		double[] seasonalityCompoundedArray = new double[12];
		int lastMonthIndex = fixingMonth.Month.Value - 2;
		seasonalityCompoundedArray[(int)((nbMonths + 12 + 1) % 12)] = seasonality.get((lastMonthIndex + 1) % 12);
		for (int i = 1; i < 12; i++)
		{
		  int j = (int)((nbMonths + 12 + 1 + i) % 12);
		  seasonalityCompoundedArray[j] = adjustmentFunction(seasonalityCompoundedArray[(j - 1 + 12) % 12], seasonality.get((lastMonthIndex + 1 + i) % 12));
		}
		return DoubleArray.ofUnsafe(seasonalityCompoundedArray);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		InflationNodalCurve test = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE_1, LAST_FIX_MONTH_1, LAST_FIX_VALUE, SEASONALITY_MULTIPLICATIVE_DEF);
		coverImmutableBean(test);
		InflationNodalCurve test2 = InflationNodalCurve.of(CURVE2_NOFIX, VAL_DATE_2, LAST_FIX_MONTH_2, LAST_FIX_VALUE + 1.0d, SEASONALITY_ADDITIVE_DEF);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		InflationNodalCurve test = InflationNodalCurve.of(CURVE_NOFIX, VAL_DATE_1, LAST_FIX_MONTH_1, LAST_FIX_VALUE, SEASONALITY_MULTIPLICATIVE_DEF);
		assertSerialization(test);
	  }

	}

}