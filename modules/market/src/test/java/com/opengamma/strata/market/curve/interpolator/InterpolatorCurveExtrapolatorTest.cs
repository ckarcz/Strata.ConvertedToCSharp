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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="InterpolatorCurveExtrapolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InterpolatorCurveExtrapolatorTest
	public class InterpolatorCurveExtrapolatorTest
	{

	  private static readonly CurveExtrapolator INT_EXTRAPOLATOR = InterpolatorCurveExtrapolator.INSTANCE;

	  private const double TOL = 1.e-14;

	  public virtual void test_basics()
	  {
		assertEquals(INT_EXTRAPOLATOR.Name, InterpolatorCurveExtrapolator.NAME);
		assertEquals(INT_EXTRAPOLATOR.ToString(), InterpolatorCurveExtrapolator.NAME);
	  }

	  public virtual void sameIntervalsTest()
	  {
		DoubleArray xValues = DoubleArray.of(-1.0, 0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0);
		DoubleArray[] yValues = new DoubleArray[] {DoubleArray.of(1.001, 1.001, 1.001, 1.001, 1.001, 1.001, 1.001, 1.001, 1.001, 1.001), DoubleArray.of(11.0, 11.0, 8.0, 5.0, 1.001, 1.001, 5.0, 8.0, 11.0, 11.0), DoubleArray.of(1.001, 1.001, 5.0, 8.0, 9.0, 9.0, 11.0, 12.0, 18.0, 18.0)};
		int nKeys = 100;
		double[] keys = new double[nKeys];
		double interval = 0.061;
		for (int i = 0; i < nKeys; ++i)
		{
		  keys[i] = xValues.get(0) + interval * i;
		}

		CurveExtrapolator extrap = InterpolatorCurveExtrapolator.INSTANCE;
		int yDim = yValues.Length;
		for (int k = 0; k < yDim; ++k)
		{
		  BoundCurveInterpolator boundInterp = CurveInterpolators.SQUARE_LINEAR.bind(xValues, yValues[k], extrap, extrap);
		  AbstractBoundCurveInterpolator baseInterp = (AbstractBoundCurveInterpolator) boundInterp;
		  for (int j = 0; j < nKeys; ++j)
		  {
			// value
			assertEquals(boundInterp.interpolate(keys[j]), baseInterp.doInterpolate(keys[j]), TOL);
			// derivative 
			assertEquals(boundInterp.firstDerivative(keys[j]), baseInterp.doFirstDerivative(keys[j]), TOL);
			// sensitivity
			assertTrue(boundInterp.parameterSensitivity(keys[j]).equalWithTolerance(baseInterp.doParameterSensitivity(keys[j]), TOL));
		  }
		}
	  }

	  public virtual void differentIntervalsTest()
	  {
		DoubleArray xValues = DoubleArray.of(1.0328724558967068, 1.2692381049172323, 2.8611430465380905, 4.296118458251132, 7.011992052151352, 7.293354144919639, 8.557971037612713, 8.77306861567384, 10.572470371584489, 12.96945799507056);
		DoubleArray[] yValues = new DoubleArray[] {DoubleArray.of(1.1593075755231343, 2.794957672828094, 4.674733634811079, 5.517689918508841, 6.138447304104604, 6.264375977142906, 6.581666492568779, 8.378685055774037, 10.005246918325483, 10.468304334744241), DoubleArray.of(9.95780079114617, 8.733013195721913, 8.192165283188197, 6.539369493529048, 6.3868683960757515, 4.700471352238411, 4.555354921077598, 3.780781869340659, 2.299369456202763, 0.9182441378327986)};
		int nKeys = 100;
		double[] keys = new double[nKeys];
		double interval = 0.061;
		for (int i = 0; i < nKeys; ++i)
		{
		  keys[i] = xValues.get(0) + interval * i;
		}

		CurveExtrapolator extrap = InterpolatorCurveExtrapolator.INSTANCE;
		int yDim = yValues.Length;
		for (int k = 0; k < yDim; ++k)
		{
		  BoundCurveInterpolator boundInterp = CurveInterpolators.SQUARE_LINEAR.bind(xValues, yValues[k], extrap, extrap);
		  AbstractBoundCurveInterpolator baseInterp = (AbstractBoundCurveInterpolator) boundInterp;
		  for (int j = 0; j < nKeys; ++j)
		  {
			// value
			assertEquals(boundInterp.interpolate(keys[j]), baseInterp.doInterpolate(keys[j]), TOL);
			// derivative 
			assertEquals(boundInterp.firstDerivative(keys[j]), baseInterp.doFirstDerivative(keys[j]), TOL);
			// sensitivity
			assertTrue(boundInterp.parameterSensitivity(keys[j]).equalWithTolerance(baseInterp.doParameterSensitivity(keys[j]), TOL));
		  }
		}
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(INT_EXTRAPOLATOR);
	  }

	}

}