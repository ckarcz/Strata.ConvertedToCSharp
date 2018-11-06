using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// Test <seealso cref="AddFixedCurve"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AddFixedCurveTest
	public class AddFixedCurveTest
	{

	  private const string NAME_FIXED = "FixedCurve";
	  private const string NAME_SPREAD = "SpreadCurve";
	  private static readonly CurveName FIXED_CURVE_NAME = CurveName.of(NAME_FIXED);
	  private static readonly CurveName SPREAD_CURVE_NAME = CurveName.of(NAME_SPREAD);
	  private static readonly CurveMetadata METADATA_FIXED = Curves.zeroRates(FIXED_CURVE_NAME, ACT_365F);
	  private const string LABEL_1 = "Node1";
	  private const string LABEL_2 = "Node2";
	  private const string LABEL_3 = "Node3";
	  private static readonly IList<ParameterMetadata> PARAM_METADATA_SPREAD = new List<ParameterMetadata>();
	  static AddFixedCurveTest()
	  {
		PARAM_METADATA_SPREAD.Add(LabelDateParameterMetadata.of(LocalDate.of(2015, 1, 1), LABEL_1));
		PARAM_METADATA_SPREAD.Add(LabelDateParameterMetadata.of(LocalDate.of(2015, 2, 1), LABEL_2));
		PARAM_METADATA_SPREAD.Add(LabelDateParameterMetadata.of(LocalDate.of(2015, 3, 1), LABEL_3));
	  }
	  private static readonly CurveMetadata METADATA_SPREAD = DefaultCurveMetadata.builder().curveName(SPREAD_CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).parameterMetadata(PARAM_METADATA_SPREAD).build();

	  private static readonly DoubleArray XVALUES_FIXED = DoubleArray.of(1d, 2d, 3d, 4d);
	  private static readonly DoubleArray YVALUES_FIXED = DoubleArray.of(0.05d, 0.07d, 0.08d, 0.09d);
	  private static readonly DoubleArray XVALUES_SPREAD = DoubleArray.of(1.5d, 2.5d, 4.5d);
	  private static readonly DoubleArray YVALUES_SPREAD = DoubleArray.of(0.04d, 0.045d, 0.05d);
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly double[] X_SAMPLE = new double[] {0.5d, 1.0d, 1.5d, 1.75d, 10.0d};
	  private static readonly int NB_X_SAMPLE = X_SAMPLE.Length;

	  private static readonly InterpolatedNodalCurve FIXED_CURVE = InterpolatedNodalCurve.of(METADATA_FIXED, XVALUES_FIXED, YVALUES_FIXED, INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve SPREAD_CURVE = InterpolatedNodalCurve.of(METADATA_SPREAD, XVALUES_SPREAD, YVALUES_SPREAD, INTERPOLATOR);

	  private static readonly AddFixedCurve ADD_FIXED_CURVE = AddFixedCurve.of(FIXED_CURVE, SPREAD_CURVE);

	  private const double TOLERANCE_Y = 1.0E-10;

	  public virtual void test_invalid()
	  {
		// null fixed
		assertThrowsIllegalArg(() => AddFixedCurve.of(null, SPREAD_CURVE));
		// null spread
		assertThrowsIllegalArg(() => AddFixedCurve.of(FIXED_CURVE, null));
	  }

	  public virtual void getter()
	  {
		assertEquals(ADD_FIXED_CURVE.Metadata, METADATA_SPREAD);
		assertEquals(ADD_FIXED_CURVE.ParameterCount, XVALUES_SPREAD.size());
		assertEquals(ADD_FIXED_CURVE.getParameter(0), ADD_FIXED_CURVE.SpreadCurve.getParameter(0));
		assertEquals(ADD_FIXED_CURVE.getParameterMetadata(0), ADD_FIXED_CURVE.SpreadCurve.getParameterMetadata(0));
		assertEquals(ADD_FIXED_CURVE.withParameter(0, 9d), AddFixedCurve.of(FIXED_CURVE, SPREAD_CURVE.withParameter(0, 9d)));
		assertEquals(ADD_FIXED_CURVE.withPerturbation((i, v, m) => v + 1d), AddFixedCurve.of(FIXED_CURVE, SPREAD_CURVE.withPerturbation((i, v, m) => v + 1d)));
		assertEquals(ADD_FIXED_CURVE.withMetadata(METADATA_FIXED), AddFixedCurve.of(FIXED_CURVE, SPREAD_CURVE.withMetadata(METADATA_FIXED)));
	  }

	  public virtual void yValue()
	  {
		for (int i = 0; i < NB_X_SAMPLE; i++)
		{
		  double yComputed = ADD_FIXED_CURVE.yValue(X_SAMPLE[i]);
		  double yExpected = FIXED_CURVE.yValue(X_SAMPLE[i]) + SPREAD_CURVE.yValue(X_SAMPLE[i]);
		  assertEquals(yComputed, yExpected, TOLERANCE_Y);
		}
	  }

	  public virtual void firstDerivative()
	  {
		for (int i = 0; i < NB_X_SAMPLE; i++)
		{
		  double dComputed = ADD_FIXED_CURVE.firstDerivative(X_SAMPLE[i]);
		  double dExpected = FIXED_CURVE.firstDerivative(X_SAMPLE[i]) + SPREAD_CURVE.firstDerivative(X_SAMPLE[i]);
		  assertEquals(dComputed, dExpected, TOLERANCE_Y);
		}
	  }

	  public virtual void yParameterSensitivity()
	  {
		for (int i = 0; i < X_SAMPLE.Length; i++)
		{
		  UnitParameterSensitivity dComputed = ADD_FIXED_CURVE.yValueParameterSensitivity(X_SAMPLE[i]);
		  UnitParameterSensitivity dExpected = SPREAD_CURVE.yValueParameterSensitivity(X_SAMPLE[i]);
		  assertTrue(dComputed.compareKey(dExpected) == 0);
		  assertTrue(dComputed.Sensitivity.equalWithTolerance(dExpected.Sensitivity, TOLERANCE_Y));
		}
	  }

	  public virtual void underlyingCurve()
	  {
		assertEquals(ADD_FIXED_CURVE.split(), ImmutableList.of(FIXED_CURVE, SPREAD_CURVE));
		CurveMetadata metadata = DefaultCurveMetadata.builder().curveName(CurveName.of("newCurve")).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).parameterMetadata(PARAM_METADATA_SPREAD).build();
		InterpolatedNodalCurve newCurve = InterpolatedNodalCurve.of(metadata, XVALUES_SPREAD, YVALUES_SPREAD, INTERPOLATOR);
		assertEquals(ADD_FIXED_CURVE.withUnderlyingCurve(0, newCurve), AddFixedCurve.of(newCurve, SPREAD_CURVE));
		assertEquals(ADD_FIXED_CURVE.withUnderlyingCurve(1, newCurve), AddFixedCurve.of(FIXED_CURVE, newCurve));
		assertThrowsIllegalArg(() => ADD_FIXED_CURVE.withUnderlyingCurve(2, newCurve));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(ADD_FIXED_CURVE);
		coverBeanEquals(ADD_FIXED_CURVE, AddFixedCurve.of(SPREAD_CURVE, FIXED_CURVE));
	  }

	}

}