/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;

	/// <summary>
	/// Test <seealso cref="CurveParallelShifts"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveParallelShiftsTest
	public class CurveParallelShiftsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  public virtual void test_absolute()
	  {
		CurveParallelShifts test = CurveParallelShifts.absolute(1d, 2d, 4d);

		Curve baseCurve = InterpolatedNodalCurve.of(Curves.zeroRates("curve", DayCounts.ACT_365F), DoubleArray.of(1, 2, 3), DoubleArray.of(5, 6, 7), CurveInterpolators.LOG_LINEAR);

		MarketDataBox<Curve> shiftedCurveBox = test.applyTo(MarketDataBox.ofSingleValue(baseCurve), REF_DATA);

		assertEquals(shiftedCurveBox.getValue(0), ParallelShiftedCurve.absolute(baseCurve, 1d));
		assertEquals(shiftedCurveBox.getValue(1), ParallelShiftedCurve.absolute(baseCurve, 2d));
		assertEquals(shiftedCurveBox.getValue(2), ParallelShiftedCurve.absolute(baseCurve, 4d));
	  }

	  public virtual void test_relative()
	  {
		CurveParallelShifts test = CurveParallelShifts.relative(0.1d, 0.2d, 0.4d);

		Curve baseCurve = InterpolatedNodalCurve.of(Curves.zeroRates("curve", DayCounts.ACT_365F), DoubleArray.of(1, 2, 3), DoubleArray.of(5, 6, 7), CurveInterpolators.LOG_LINEAR);

		MarketDataBox<Curve> shiftedCurveBox = test.applyTo(MarketDataBox.ofSingleValue(baseCurve), REF_DATA);

		assertEquals(shiftedCurveBox.getValue(0), ParallelShiftedCurve.relative(baseCurve, 0.1d));
		assertEquals(shiftedCurveBox.getValue(1), ParallelShiftedCurve.relative(baseCurve, 0.2d));
		assertEquals(shiftedCurveBox.getValue(2), ParallelShiftedCurve.relative(baseCurve, 0.4d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CurveParallelShifts test = CurveParallelShifts.absolute(1d, 2d, 4d);
		coverImmutableBean(test);
		CurveParallelShifts test2 = CurveParallelShifts.relative(2d, 3d, 4d);
		coverBeanEquals(test, test2);
	  }

	}

}