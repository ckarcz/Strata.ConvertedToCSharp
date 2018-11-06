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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="QuadraticLeftCurveExtrapolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class QuadraticLeftCurveExtrapolatorTest
	public class QuadraticLeftCurveExtrapolatorTest
	{

	  private static readonly CurveExtrapolator QL_EXTRAPOLATOR = QuadraticLeftCurveExtrapolator.INSTANCE;

	  private static readonly DoubleArray X_DATA = DoubleArray.of(0.3, 0.4, 1.0, 1.8, 2.8, 5.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 3.1, 2.0, 7.0, 2.0);

	  private const double EPS = 1.e-7;
	  private const double TOL = 1.e-12;

	  public virtual void test_basics()
	  {
		assertEquals(QL_EXTRAPOLATOR.Name, QuadraticLeftCurveExtrapolator.NAME);
		assertEquals(QL_EXTRAPOLATOR.ToString(), QuadraticLeftCurveExtrapolator.NAME);
	  }

	  public virtual void sameIntervalsTest()
	  {
		DoubleArray xValues = DoubleArray.of(1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0);
		DoubleArray[] yValues = new DoubleArray[] {DoubleArray.of(1.001, 1.001, 1.001, 1.001, 1.001, 1.001, 1.001, 1.001), DoubleArray.of(11.0, 8.0, 5.0, 1.001, 1.001, 5.0, 8.0, 11.0), DoubleArray.of(1.001, 1.001, 5.0, 9.0, 9.0, 12.0, 18.0, 18.0)};
		int nData = xValues.size();
		int nKeys = 100 * nData;
		double[] xKeys = new double[nKeys];
		double xMin = 0.0;
		double xMax = xValues.get(nData - 1) + 2.0;
		double step = (xMax - xMin) / nKeys;
		for (int i = 0; i < nKeys; ++i)
		{
		  xKeys[i] = xMin + step * i;
		}

		CurveExtrapolator extrap = QuadraticLeftCurveExtrapolator.INSTANCE;

		int yDim = yValues.Length;
		for (int k = 0; k < yDim; ++k)
		{
		  BoundCurveInterpolator bci = CurveInterpolators.LOG_NATURAL_SPLINE_MONOTONE_CUBIC.bind(xValues, yValues[k], extrap, CurveExtrapolators.LOG_LINEAR);

		  // Check C0 continuity
		  assertEquals(bci.interpolate(xValues.get(0) - 1.e-14), bci.interpolate(xValues.get(0)), TOL);

		  // Check C1 continuity
		  assertEquals(bci.firstDerivative(xValues.get(0) - TOL), bci.firstDerivative(xValues.get(0)), TOL * 1.e2);

		  // Test sensitivity
		  double[] yValues1Up = yValues[k].toArray();
		  double[] yValues1Dw = yValues[k].toArray();
		  for (int j = 0; j < nData; ++j)
		  {
			yValues1Up[j] = yValues[k].get(j) * (1.0 + EPS);
			yValues1Dw[j] = yValues[k].get(j) * (1.0 - EPS);
			BoundCurveInterpolator bciUp = CurveInterpolators.LOG_NATURAL_SPLINE_MONOTONE_CUBIC.bind(xValues, DoubleArray.ofUnsafe(yValues1Up), extrap, CurveExtrapolators.LOG_LINEAR);
			BoundCurveInterpolator bciDw = CurveInterpolators.LOG_NATURAL_SPLINE_MONOTONE_CUBIC.bind(xValues, DoubleArray.ofUnsafe(yValues1Dw), extrap, CurveExtrapolators.LOG_LINEAR);
			for (int i = 0; i < nKeys; ++i)
			{
			  double res1 = 0.5 * (bciUp.interpolate(xKeys[i]) - bciDw.interpolate(xKeys[i])) / EPS / yValues[k].get(j);
			  assertEquals(res1, bci.parameterSensitivity(xKeys[i]).get(j), Math.Max(Math.Abs(yValues[k].get(j)) * EPS, EPS) * 1.e2); //because gradient is NOT exact
			}
			yValues1Up[j] = yValues[k].get(j);
			yValues1Dw[j] = yValues[k].get(j);
		  }
		}
	  }

	  public virtual void differentIntervalsTest()
	  {
		DoubleArray xValues = DoubleArray.of(1.0328724558967068, 1.2692381049172323, 2.8611430465380905, 4.296118458251132, 7.011992052151352, 7.293354144919639, 8.557971037612713, 8.77306861567384, 10.572470371584489, 12.96945799507056);
		DoubleArray[] yValues = new DoubleArray[] {DoubleArray.of(1.1593075755231343, 2.794957672828094, 4.674733634811079, 5.517689918508841, 6.138447304104604, 6.264375977142906, 6.581666492568779, 8.378685055774037, 10.005246918325483, 10.468304334744241), DoubleArray.of(9.95780079114617, 8.733013195721913, 8.192165283188197, 6.539369493529048, 6.3868683960757515, 4.700471352238411, 4.555354921077598, 3.780781869340659, 2.299369456202763, 0.9182441378327986)};
		int nData = xValues.size();
		int nKeys = 100 * nData;
		double[] xKeys = new double[nKeys];
		double xMin = 0.0;
		double xMax = xValues.get(nData - 1) + 2.0;
		double step = (xMax - xMin) / nKeys;
		for (int i = 0; i < nKeys; ++i)
		{
		  xKeys[i] = xMin + step * i;
		}

		CurveExtrapolator extrap = QuadraticLeftCurveExtrapolator.INSTANCE;

		int yDim = yValues.Length;
		for (int k = 0; k < yDim; ++k)
		{
		  BoundCurveInterpolator bci = CurveInterpolators.LOG_NATURAL_SPLINE_MONOTONE_CUBIC.bind(xValues, yValues[k], extrap, CurveExtrapolators.LOG_LINEAR);

		  // Check C0 continuity
		  assertEquals(bci.interpolate(xValues.get(0) - 1.e-14), bci.interpolate(xValues.get(0)), TOL);

		  // Check C1 continuity
		  assertEquals(bci.firstDerivative(xValues.get(0) - TOL), bci.firstDerivative(xValues.get(0)), TOL * 1.e2);

		  // Test sensitivity
		  double[] yValues1Up = yValues[k].toArray();
		  double[] yValues1Dw = yValues[k].toArray();
		  for (int j = 0; j < nData; ++j)
		  {
			yValues1Up[j] = yValues[k].get(j) * (1.0 + EPS);
			yValues1Dw[j] = yValues[k].get(j) * (1.0 - EPS);
			BoundCurveInterpolator bciUp = CurveInterpolators.LOG_NATURAL_SPLINE_MONOTONE_CUBIC.bind(xValues, DoubleArray.ofUnsafe(yValues1Up), extrap, CurveExtrapolators.LOG_LINEAR);
			BoundCurveInterpolator bciDw = CurveInterpolators.LOG_NATURAL_SPLINE_MONOTONE_CUBIC.bind(xValues, DoubleArray.ofUnsafe(yValues1Dw), extrap, CurveExtrapolators.LOG_LINEAR);
			for (int i = 0; i < nKeys; ++i)
			{
			  double res1 = 0.5 * (bciUp.interpolate(xKeys[i]) - bciDw.interpolate(xKeys[i])) / EPS / yValues[k].get(j);
			  assertEquals(res1, bci.parameterSensitivity(xKeys[i]).get(j), Math.Max(Math.Abs(yValues[k].get(j)) * EPS, EPS) * 1.e2); //because gradient is NOT exact
			}
			yValues1Up[j] = yValues[k].get(j);
			yValues1Dw[j] = yValues[k].get(j);
		  }
		}
	  }

	  public virtual void test_noRight()
	  {
		BoundCurveInterpolator bci = CurveInterpolators.LINEAR.bind(X_DATA, Y_DATA, QL_EXTRAPOLATOR, QL_EXTRAPOLATOR);
		assertThrowsIllegalArg(() => bci.interpolate(10d));
		assertThrowsIllegalArg(() => bci.firstDerivative(10d));
		assertThrowsIllegalArg(() => bci.parameterSensitivity(10d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(QL_EXTRAPOLATOR);
	  }

	}

}