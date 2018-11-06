using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;

	/// <summary>
	/// Test <seealso cref="PointShifts"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PointShiftsTest
	public class PointShiftsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private const string TNR_1W = "1W";
	  private const string TNR_1M = "1M";
	  private const string TNR_3M = "3M";
	  private const string TNR_6M = "6M";
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LOG_LINEAR;

	  // TODO test volatilities with surface, SABR params

	  public virtual void absolute()
	  {
		IList<LabelDateParameterMetadata> nodeMetadata = ImmutableList.of(LabelDateParameterMetadata.of(date(2011, 3, 8), TNR_1M), LabelDateParameterMetadata.of(date(2011, 5, 8), TNR_3M), LabelDateParameterMetadata.of(date(2011, 8, 8), TNR_6M));

		// This should create 4 scenarios. Scenario zero has no shifts and scenario 3 doesn't have shifts on all nodes
		PointShifts shift = PointShifts.builder(ShiftType.ABSOLUTE).addShift(1, TNR_1W, 0.1).addShift(1, TNR_1M, 0.2).addShift(1, TNR_3M, 0.3).addShift(2, TNR_1M, 0.4).addShift(2, TNR_3M, 0.5).addShift(2, TNR_6M, 0.6).addShift(3, TNR_3M, 0.7).build();

		Curve curve = InterpolatedNodalCurve.of(Curves.zeroRates(CurveName.of("curve"), DayCounts.ACT_365F, nodeMetadata), DoubleArray.of(1, 2, 3), DoubleArray.of(5, 6, 7), INTERPOLATOR);

		MarketDataBox<ParameterizedData> shiftedCurveBox = shift.applyTo(MarketDataBox.ofSingleValue(curve), REF_DATA);

		Curve scenario1Curve = InterpolatedNodalCurve.of(Curves.zeroRates(CurveName.of("curve"), DayCounts.ACT_365F, nodeMetadata), DoubleArray.of(1, 2, 3), DoubleArray.of(5.2, 6.3, 7), INTERPOLATOR);

		Curve scenario2Curve = InterpolatedNodalCurve.of(Curves.zeroRates(CurveName.of("curve"), DayCounts.ACT_365F, nodeMetadata), DoubleArray.of(1, 2, 3), DoubleArray.of(5.4, 6.5, 7.6), INTERPOLATOR);

		Curve scenario3Curve = InterpolatedNodalCurve.of(Curves.zeroRates(CurveName.of("curve"), DayCounts.ACT_365F, nodeMetadata), DoubleArray.of(1, 2, 3), DoubleArray.of(5, 6.7, 7), INTERPOLATOR);

		// Scenario zero has no perturbations so the expected curve is the same as the input
		IList<Curve> expectedCurves = ImmutableList.of(curve, scenario1Curve, scenario2Curve, scenario3Curve);

		for (int scenarioIndex = 0; scenarioIndex < 4; scenarioIndex++)
		{
		  // Check every point from 0 to 4 in steps of 0.1 is the same on the bumped curve and the expected curve
		  for (int xIndex = 0; xIndex <= 40; xIndex++)
		  {
			double xValue = xIndex * 0.1;
			Curve expectedCurve = expectedCurves[scenarioIndex];
			Curve shiftedCurve = (Curve) shiftedCurveBox.getValue(scenarioIndex);
			double shiftedY = shiftedCurve.yValue(xValue);
			double expectedY = expectedCurve.yValue(xValue);
			assertThat(shiftedY).overridingErrorMessage("Curve differed in scenario %d at x value %f, expected %f, actual %f", scenarioIndex, xValue, expectedY, shiftedY).isEqualTo(expectedY);
		  }
		}
	  }

	  public virtual void relative()
	  {
		IList<LabelDateParameterMetadata> nodeMetadata = ImmutableList.of(LabelDateParameterMetadata.of(date(2011, 3, 8), TNR_1M), LabelDateParameterMetadata.of(date(2011, 5, 8), TNR_3M), LabelDateParameterMetadata.of(date(2011, 8, 8), TNR_6M));

		// This should create 4 scenarios. Scenario zero has no shifts and scenario 3 doesn't have shifts on all nodes
		PointShifts shift = PointShifts.builder(ShiftType.RELATIVE).addShift(1, TNR_1W, 0.1).addShift(1, TNR_1M, 0.2).addShift(1, TNR_3M, 0.3).addShift(2, TNR_1M, 0.4).addShift(2, TNR_3M, 0.5).addShift(2, TNR_6M, 0.6).addShift(3, TNR_3M, 0.7).build();

		Curve curve = InterpolatedNodalCurve.of(Curves.zeroRates(CurveName.of("curve"), DayCounts.ACT_365F, nodeMetadata), DoubleArray.of(1, 2, 3), DoubleArray.of(5, 6, 7), INTERPOLATOR);

		MarketDataBox<ParameterizedData> shiftedCurveBox = shift.applyTo(MarketDataBox.ofSingleValue(curve), REF_DATA);

		Curve scenario1Curve = InterpolatedNodalCurve.of(Curves.zeroRates(CurveName.of("curve"), DayCounts.ACT_365F, nodeMetadata), DoubleArray.of(1, 2, 3), DoubleArray.of(6, 7.8, 7), INTERPOLATOR);

		Curve scenario2Curve = InterpolatedNodalCurve.of(Curves.zeroRates(CurveName.of("curve"), DayCounts.ACT_365F, nodeMetadata), DoubleArray.of(1, 2, 3), DoubleArray.of(7, 9, 11.2), INTERPOLATOR);

		Curve scenario3Curve = InterpolatedNodalCurve.of(Curves.zeroRates(CurveName.of("curve"), DayCounts.ACT_365F, nodeMetadata), DoubleArray.of(1, 2, 3), DoubleArray.of(5, 10.2, 7), INTERPOLATOR);

		// Scenario zero has no perturbations so the expected curve is the same as the input
		IList<Curve> expectedCurves = ImmutableList.of(curve, scenario1Curve, scenario2Curve, scenario3Curve);

		for (int scenarioIndex = 0; scenarioIndex < 4; scenarioIndex++)
		{
		  // Check every point from 0 to 4 in steps of 0.1 is the same on the bumped curve and the expected curve
		  for (int xIndex = 0; xIndex <= 40; xIndex++)
		  {
			double xValue = xIndex * 0.1;
			Curve expectedCurve = expectedCurves[scenarioIndex];
			Curve shiftedCurve = (Curve) shiftedCurveBox.getValue(scenarioIndex);
			double shiftedY = shiftedCurve.yValue(xValue);
			double expectedY = expectedCurve.yValue(xValue);
			assertThat(shiftedY).overridingErrorMessage("Curve differed in scenario %d at x value %f, expected %f, actual %f", scenarioIndex, xValue, expectedY, shiftedY).isEqualTo(expectedY);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		PointShifts test = PointShifts.builder(ShiftType.RELATIVE).addShift(0, Tenor.TENOR_1W, 0.1).addShift(0, Tenor.TENOR_1M, 0.2).addShift(0, Tenor.TENOR_3M, 0.3).build();
		coverImmutableBean(test);
		PointShifts test2 = PointShifts.builder(ShiftType.ABSOLUTE).addShift(0, Tenor.TENOR_1M, 0.2).addShift(0, Tenor.TENOR_3M, 0.3).build();
		coverBeanEquals(test, test2);
	  }

	}

}