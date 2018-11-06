using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="ExponentialCurveExtrapolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ExponentialCurveExtrapolatorTest
	public class ExponentialCurveExtrapolatorTest
	{

	  private static readonly CurveExtrapolator EXP_EXTRAPOLATOR = ExponentialCurveExtrapolator.INSTANCE;

	  private static readonly DoubleArray X_DATA = DoubleArray.of(0.01, 0.4, 1.0, 1.8, 2.8, 5.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 3.1, 2.0, 7.0, 2.0);
	  private const double TOLERANCE_VALUE = 1.0E-10;
	  private const double TOLERANCE_SENSI = 1.0E-5;

	  public virtual void test_basics()
	  {
		assertEquals(EXP_EXTRAPOLATOR.Name, ExponentialCurveExtrapolator.NAME);
		assertEquals(EXP_EXTRAPOLATOR.ToString(), ExponentialCurveExtrapolator.NAME);
	  }

	  public virtual void value()
	  {
		BoundCurveInterpolator bci = CurveInterpolators.LINEAR.bind(X_DATA, Y_DATA, EXP_EXTRAPOLATOR, EXP_EXTRAPOLATOR);

		double mLeft = Math.Log(Y_DATA.get(0)) / X_DATA.get(0);
		double mRight = Math.Log(Y_DATA.get(X_DATA.size() - 1)) / X_DATA.get(X_DATA.size() - 1);
		assertEquals(bci.interpolate(0.0), 1d, TOLERANCE_VALUE);
		assertEquals(bci.interpolate(-0.2), Math.Exp(mLeft * -0.2), TOLERANCE_VALUE);
		assertEquals(bci.interpolate(6.0), Math.Exp(mRight * 6.0), TOLERANCE_VALUE);
	  }

	  public virtual void sensitivity1()
	  {
		BoundCurveInterpolator bci = CurveInterpolators.LINEAR.bind(X_DATA, Y_DATA, EXP_EXTRAPOLATOR, EXP_EXTRAPOLATOR);

		double shift = 1e-8;
		double value = 0d;
		double[] yDataShifted = Y_DATA.toArray();
		yDataShifted[0] += shift;
		BoundCurveInterpolator bciShifted1 = CurveInterpolators.LINEAR.bind(X_DATA, DoubleArray.ofUnsafe(yDataShifted), EXP_EXTRAPOLATOR, EXP_EXTRAPOLATOR);
		assertEquals(bci.parameterSensitivity(value).get(0), (bciShifted1.interpolate(value) - bci.interpolate(value)) / shift, TOLERANCE_SENSI);
	  }

	  public virtual void sensitivity2()
	  {
		BoundCurveInterpolator bci = CurveInterpolators.LINEAR.bind(X_DATA, Y_DATA, EXP_EXTRAPOLATOR, EXP_EXTRAPOLATOR);

		double shift = 1e-8;
		double value = -0.2;
		double[] yDataShifted = Y_DATA.toArray();
		yDataShifted[0] += shift;
		BoundCurveInterpolator bciShifted = CurveInterpolators.LINEAR.bind(X_DATA, DoubleArray.ofUnsafe(yDataShifted), EXP_EXTRAPOLATOR, EXP_EXTRAPOLATOR);
		assertEquals(bci.parameterSensitivity(value).get(0), (bciShifted.interpolate(value) - bci.interpolate(value)) / shift, TOLERANCE_SENSI);
	  }

	  public virtual void sensitivity3()
	  {
		BoundCurveInterpolator bci = CurveInterpolators.LINEAR.bind(X_DATA, Y_DATA, EXP_EXTRAPOLATOR, EXP_EXTRAPOLATOR);

		double shift = 1e-8;
		double value = 6d;
		double[] yDataShifted = Y_DATA.toArray();
		yDataShifted[Y_DATA.size() - 1] += shift;
		BoundCurveInterpolator bciShifted = CurveInterpolators.LINEAR.bind(X_DATA, DoubleArray.ofUnsafe(yDataShifted), EXP_EXTRAPOLATOR, EXP_EXTRAPOLATOR);
		assertEquals(bci.parameterSensitivity(value).get(Y_DATA.size() - 1), (bciShifted.interpolate(value) - bci.interpolate(value)) / shift, TOLERANCE_SENSI);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(EXP_EXTRAPOLATOR);
	  }

	}

}