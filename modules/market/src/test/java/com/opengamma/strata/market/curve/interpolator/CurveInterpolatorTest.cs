/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.DOUBLE_QUADRATIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LOG_LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LOG_NATURAL_SPLINE_DISCOUNT_FACTOR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LOG_NATURAL_SPLINE_MONOTONE_CUBIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.NATURAL_CUBIC_SPLINE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.NATURAL_SPLINE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.NATURAL_SPLINE_NONNEGATIVITY_CUBIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.PRODUCT_NATURAL_SPLINE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.SQUARE_LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.TIME_SQUARE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotNull;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="CurveInterpolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveInterpolatorTest
	public class CurveInterpolatorTest
	{

	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {LINEAR, "Linear"},
			new object[] {LOG_LINEAR, "LogLinear"},
			new object[] {SQUARE_LINEAR, "SquareLinear"},
			new object[] {DOUBLE_QUADRATIC, "DoubleQuadratic"},
			new object[] {TIME_SQUARE, "TimeSquare"},
			new object[] {LOG_NATURAL_SPLINE_MONOTONE_CUBIC, "LogNaturalSplineMonotoneCubic"},
			new object[] {LOG_NATURAL_SPLINE_DISCOUNT_FACTOR, "LogNaturalSplineDiscountFactor"},
			new object[] {NATURAL_CUBIC_SPLINE, "NaturalCubicSpline"},
			new object[] {NATURAL_SPLINE, "NaturalSpline"},
			new object[] {NATURAL_SPLINE_NONNEGATIVITY_CUBIC, "NaturalSplineNonnegativityCubic"},
			new object[] {PRODUCT_NATURAL_SPLINE, "ProductNaturalSpline"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(CurveInterpolator convention, String name)
	  public virtual void test_name(CurveInterpolator convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(CurveInterpolator convention, String name)
	  public virtual void test_toString(CurveInterpolator convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(CurveInterpolator convention, String name)
	  public virtual void test_of_lookup(CurveInterpolator convention, string name)
	  {
		assertEquals(CurveInterpolator.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(CurveInterpolator convention, String name)
	  public virtual void test_extendedEnum(CurveInterpolator convention, string name)
	  {
		ImmutableMap<string, CurveInterpolator> map = CurveInterpolator.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => CurveInterpolator.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => CurveInterpolator.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_bind()
	  {
		DoubleArray xValues = DoubleArray.of(1, 2, 3);
		DoubleArray yValues = DoubleArray.of(2, 4, 5);
		BoundCurveInterpolator bound = LINEAR.bind(xValues, yValues, CurveExtrapolators.FLAT, CurveExtrapolators.FLAT);
		assertEquals(bound.interpolate(0.5), 2d, 0d);
		assertEquals(bound.interpolate(1), 2d, 0d);
		assertEquals(bound.interpolate(1.5), 3d, 0d);
		assertEquals(bound.interpolate(2), 4d, 0d);
		assertEquals(bound.interpolate(2.5), 4.5d, 0d);
		assertEquals(bound.interpolate(3), 5d, 0d);
		assertEquals(bound.interpolate(3.5), 5d, 0d);
		// coverage
		assertEquals(bound.parameterSensitivity(0.5).size(), 3);
		assertEquals(bound.parameterSensitivity(2).size(), 3);
		assertEquals(bound.parameterSensitivity(3.5).size(), 3);
		assertEquals(bound.firstDerivative(0.5), 0d, 0d);
		assertTrue(bound.firstDerivative(2) != 0d);
		assertEquals(bound.firstDerivative(3.5), 0d, 0d);
		assertNotNull(bound.ToString());
	  }

	  public virtual void test_lowerBound()
	  {
		// bad input, but still produces good output
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(0.0d, new double[] {1, 2, 3}), 0);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(0.5d, new double[] {1, 2, 3}), 0);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(0.9999d, new double[] {1, 2, 3}), 0);
		// good input
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(1.0d, new double[] {1, 2, 3}), 0);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(1.0001d, new double[] {1, 2, 3}), 0);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(1.9999d, new double[] {1, 2, 3}), 0);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(2.0d, new double[] {1, 2, 3}), 1);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(2.0001d, new double[] {1, 2, 3}), 1);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(2.9999d, new double[] {1, 2, 3}), 1);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(3.0d, new double[] {1, 2, 3}), 2);
		// bad input, but still produces good output
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(3.0001d, new double[] {1, 2, 3}), 2);
		// check zero
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(-1.0d, new double[] {-1, 0, 1}), 0);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(-0.9999d, new double[] {-1, 0, 1}), 0);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(-0.0001d, new double[] {-1, 0, 1}), 0);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(-0.0d, new double[] {-1, 0, 1}), 1);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(0.0d, new double[] {-1, 0, 1}), 1);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(1.0d, new double[] {-1, 0, 1}), 2);
		assertEquals(AbstractBoundCurveInterpolator.lowerBoundIndex(1.5d, new double[] {-1, 0, 1}), 2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(CurveInterpolators));
		coverPrivateConstructor(typeof(StandardCurveInterpolators));
		assertFalse(LINEAR.Equals(null));
		assertFalse(LINEAR.Equals(ANOTHER_TYPE));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(LINEAR);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(CurveInterpolator), LINEAR);
		assertJodaConvert(typeof(CurveInterpolator), LOG_LINEAR);
	  }

	}

}