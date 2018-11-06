/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using IntArray = com.opengamma.strata.collect.array.IntArray;

	/// <summary>
	/// Test <seealso cref="StepUpperCurveInterpolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class StepUpperCurveInterpolatorTest
	public class StepUpperCurveInterpolatorTest
	{

	  private static readonly CurveInterpolator STEP_UPPER_INTERPOLATOR = StepUpperCurveInterpolator.INSTANCE;
	  private static readonly CurveExtrapolator FLAT_EXTRAPOLATOR = CurveExtrapolators.FLAT;

	  private const double TOL = 1.e-12;
	  private const double SMALL = 1.e-13;
	  private static readonly DoubleArray X_DATA = DoubleArray.of(0.0, 0.4, 1.0, 1.8, 2.8, 5.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 3.1, 2.0, 7.0, 2.0);
	  private static readonly int SIZE = X_DATA.size();
	  private static readonly DoubleArray X_TEST = DoubleArray.of(-1.0, SMALL, SMALL * 100d, 0.4, 1.1, 2.3, 2.8 + SMALL, 6.0);
	  private static readonly IntArray INDEX_TEST = IntArray.of(0, 0, 1, 1, 3, 4, 4, 5);

	  public virtual void test_basics()
	  {
		assertEquals(STEP_UPPER_INTERPOLATOR.Name, StepUpperCurveInterpolator.NAME);
		assertEquals(STEP_UPPER_INTERPOLATOR.ToString(), StepUpperCurveInterpolator.NAME);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_interpolation()
	  {
		BoundCurveInterpolator bci = STEP_UPPER_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		for (int i = 0; i < X_DATA.size(); i++)
		{
		  assertEquals(bci.interpolate(X_DATA.get(i)), Y_DATA.get(i), TOL);
		}
		for (int i = 0; i < X_TEST.size(); i++)
		{
		  assertEquals(bci.interpolate(X_TEST.get(i)), Y_DATA.get(INDEX_TEST.get(i)), TOL);
		}
	  }

	  public virtual void test_firstDerivative()
	  {
		BoundCurveInterpolator bci = STEP_UPPER_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		for (int i = 0; i < X_DATA.size(); i++)
		{
		  assertEquals(bci.firstDerivative(X_DATA.get(i)), 0d, TOL);
		}
		for (int i = 0; i < X_TEST.size(); i++)
		{
		  assertEquals(bci.firstDerivative(X_TEST.get(i)), 0d, TOL);
		}
	  }

	  public virtual void test_parameterSensitivity()
	  {
		BoundCurveInterpolator bci = STEP_UPPER_INTERPOLATOR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		for (int i = 0; i < X_DATA.size(); i++)
		{
		  assertTrue(DoubleArrayMath.fuzzyEquals(bci.parameterSensitivity(X_DATA.get(i)).toArray(), DoubleArray.filled(SIZE).with(i, 1d).toArray(), TOL));
		}
		for (int i = 0; i < X_TEST.size(); i++)
		{
		  assertTrue(DoubleArrayMath.fuzzyEquals(bci.parameterSensitivity(X_TEST.get(i)).toArray(), DoubleArray.filled(SIZE).with(INDEX_TEST.get(i), 1d).toArray(), TOL));
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(STEP_UPPER_INTERPOLATOR);
	  }

	}

}