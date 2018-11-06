using System;

/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Tests related to the repeated one-dimensional integration to integrate 2-D functions.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IntegratorRepeated2DTest
	public class IntegratorRepeated2DTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void integrate()
		public virtual void integrate()
		{

		// Test function.
		System.Func<double, double, double> f = (x1, x2) => x1 + Math.Sin(x2);

		double absTol = 1.0E-6;
		double relTol = 1.0E-6;
		int minSteps = 6;
		RungeKuttaIntegrator1D integrator1D = new RungeKuttaIntegrator1D(absTol, relTol, minSteps);
		IntegratorRepeated2D integrator2D = new IntegratorRepeated2D(integrator1D);

		double?[] lower;
		double?[] upper;
		double result, resultExpected;
		// First set of limits.
		lower = new double?[] {0.0, 1.0};
		upper = new double?[] {2.0, 10.0};
		result = integrator2D.integrate(f, lower, upper).Value;
		resultExpected = (upper[0] * upper[0] - lower[0] * lower[0]) / 2.0 * (upper[1] - lower[1]) + (upper[0] - lower[0]) * (-Math.Cos(upper[1]) + Math.Cos(lower[1]));
		assertEquals("Integration 2D - repeated 1D", resultExpected, result, 1E-8);
		// Second set of limits.
		lower = new double?[] {0.25, 5.25};
		upper = new double?[] {25.25, 35.25};
		result = integrator2D.integrate(f, lower, upper).Value;
		resultExpected = (upper[0] * upper[0] - lower[0] * lower[0]) / 2.0 * (upper[1] - lower[1]) + (upper[0] - lower[0]) * (-Math.Cos(upper[1]) + Math.Cos(lower[1]));
		assertEquals("Integration 2D - repeated 1D", resultExpected, result, 1E-6);
		}

	}

}