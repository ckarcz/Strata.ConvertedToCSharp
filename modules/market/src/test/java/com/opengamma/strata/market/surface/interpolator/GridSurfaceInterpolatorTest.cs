/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface.interpolator
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.EXPONENTIAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.LOG_LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.DOUBLE_QUADRATIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="GridSurfaceInterpolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GridSurfaceInterpolatorTest
	public class GridSurfaceInterpolatorTest
	{

	  private static readonly DoubleArray X_DATA = DoubleArray.of(0.0, 0.0, 0.0, 1.0, 1.0, 1.0, 2.0, 2.0, 2.0, 3.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 5.0, 3.0, 4.0, 5.0, 3.0, 4.0, 5.0, 4.0);
	  private static readonly DoubleArray Z_DATA = DoubleArray.of(3.0, 5.0, 3.1, 2.0, 4.0, 3.0, 1.5, 4.5, 2.5, 5.7);
	  // where x= 0.0, y=3.4 -> z=3.8
	  // where x= 1.0, y=3.4 -> z=2.8
	  // x= 0.2 -> z=3.6
	  //
	  // where x= 1.0, y=4.1 -> z=3.9
	  // where x= 2.0, y=4.1 -> z=4.3
	  // x= 1.3 -> z=3.9 + 0.4 * 0.3
	  //
	  // where x= 2.0, y=4.5 -> z=3.5
	  // where x= 3.0, y=4.5 -> z=5.7
	  // x= 2.5 -> z=3.5 + 2.2 * 0.5
	  private static readonly DoubleArray X_TEST = DoubleArray.of(0.2, 1.3, 2.5);
	  private static readonly DoubleArray Y_TEST = DoubleArray.of(3.4, 4.1, 4.5);
	  private static readonly DoubleArray Z_TEST = DoubleArray.of(3.6, 3.9 + (0.4 * 0.3), 3.5 + (2.2 * 0.5));
	  private const double TOL = 1.e-12;

	  //-------------------------------------------------------------------------
	  public virtual void test_of2()
	  {
		GridSurfaceInterpolator test = GridSurfaceInterpolator.of(LINEAR, LINEAR);
		assertEquals(test.XInterpolator, LINEAR);
		assertEquals(test.XExtrapolatorLeft, FLAT);
		assertEquals(test.XExtrapolatorRight, FLAT);
		assertEquals(test.YInterpolator, LINEAR);
		assertEquals(test.YExtrapolatorLeft, FLAT);
		assertEquals(test.YExtrapolatorRight, FLAT);
	  }

	  public virtual void test_of4()
	  {
		GridSurfaceInterpolator test = GridSurfaceInterpolator.of(LINEAR, EXPONENTIAL, LINEAR, EXPONENTIAL);
		assertEquals(test.XInterpolator, LINEAR);
		assertEquals(test.XExtrapolatorLeft, EXPONENTIAL);
		assertEquals(test.XExtrapolatorRight, EXPONENTIAL);
		assertEquals(test.YInterpolator, LINEAR);
		assertEquals(test.YExtrapolatorLeft, EXPONENTIAL);
		assertEquals(test.YExtrapolatorRight, EXPONENTIAL);
	  }

	  public virtual void test_of6()
	  {
		GridSurfaceInterpolator test = GridSurfaceInterpolator.of(LINEAR, EXPONENTIAL, EXPONENTIAL, LINEAR, EXPONENTIAL, EXPONENTIAL);
		assertEquals(test.XInterpolator, LINEAR);
		assertEquals(test.XExtrapolatorLeft, EXPONENTIAL);
		assertEquals(test.XExtrapolatorRight, EXPONENTIAL);
		assertEquals(test.YInterpolator, LINEAR);
		assertEquals(test.YExtrapolatorLeft, EXPONENTIAL);
		assertEquals(test.YExtrapolatorRight, EXPONENTIAL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_bind_invalidXValues()
	  {
		GridSurfaceInterpolator test = GridSurfaceInterpolator.of(LINEAR, LINEAR);
		assertThrowsIllegalArg(() => test.bind(DoubleArray.of(1d, 1d), DoubleArray.of(1d, 2d), DoubleArray.of(1d, 1d)));
	  }

	  public virtual void test_bind_invalidOrder()
	  {
		GridSurfaceInterpolator test = GridSurfaceInterpolator.of(LINEAR, LINEAR);
		assertThrowsIllegalArg(() => test.bind(DoubleArray.of(1d, 1d, 0d, 0d), DoubleArray.of(1d, 2d, 1d, 2d), DoubleArray.of(1d, 1d, 1d, 1d)));
		assertThrowsIllegalArg(() => test.bind(DoubleArray.of(1d, 1d, 2d, 2d), DoubleArray.of(1d, 0d, 1d, 0d), DoubleArray.of(1d, 1d, 1d, 1d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_interpolation()
	  {
		GridSurfaceInterpolator test = GridSurfaceInterpolator.of(LINEAR, FLAT, FLAT, LINEAR, FLAT, FLAT);
		BoundSurfaceInterpolator bci = test.bind(X_DATA, Y_DATA, Z_DATA);
		for (int i = 0; i < X_DATA.size(); i++)
		{
		  assertEquals(bci.interpolate(X_DATA.get(i), Y_DATA.get(i)), Z_DATA.get(i), TOL);
		}
		for (int i = 0; i < X_TEST.size(); i++)
		{
		  assertEquals(bci.interpolate(X_TEST.get(i), Y_TEST.get(i)), Z_TEST.get(i), TOL);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		GridSurfaceInterpolator test = GridSurfaceInterpolator.of(LINEAR, FLAT, FLAT, LINEAR, FLAT, FLAT);
		coverImmutableBean(test);
		GridSurfaceInterpolator test2 = GridSurfaceInterpolator.of(DOUBLE_QUADRATIC, LOG_LINEAR, LOG_LINEAR, DOUBLE_QUADRATIC, LOG_LINEAR, LOG_LINEAR);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		GridSurfaceInterpolator test = GridSurfaceInterpolator.of(LINEAR, FLAT, FLAT, LINEAR, FLAT, FLAT);
		assertSerialization(test);
	  }

	}

}