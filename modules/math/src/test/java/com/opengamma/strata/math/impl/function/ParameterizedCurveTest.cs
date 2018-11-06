using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Set up a simple parameterised curve
	/// (based on the function a * Math.sin(b * x) + c, where a, b, & c are the parameters)
	/// and check the finite difference sensitivity (the default behaviour of getYParameterSensitivity)
	/// agrees with the analytic calculation for a range of points along the curve.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParameterizedCurveTest
	public class ParameterizedCurveTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
		public virtual void test()
		{
		/// <summary>
		/// Take the form $y = a\sin(bx) + c$
		/// </summary>
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParameterizedCurve testCurve = new ParameterizedCurve()
		ParameterizedCurve testCurve = new ParameterizedCurveAnonymousInnerClass(this);

		ParameterizedFunction<double, DoubleArray, DoubleArray> parmSense = new ParameterizedFunctionAnonymousInnerClass(this);

		DoubleArray @params = DoubleArray.of(0.7, -0.3, 1.2);
		System.Func<double, DoubleArray> paramsSenseFD = testCurve.getYParameterSensitivity(@params);
		System.Func<double, DoubleArray> paramsSenseAnal = parmSense.asFunctionOfArguments(@params);

		for (int i = 0; i < 20; i++)
		{
		  double x = Math.PI * (-0.5 + i / 19.0);
		  DoubleArray s1 = paramsSenseAnal(x);
		  DoubleArray s2 = paramsSenseFD(x);
		  for (int j = 0; j < 3; j++)
		  {
			assertEquals(s1.get(j), s2.get(j), 1e-10);
		  }
		}

		}

	  private class ParameterizedCurveAnonymousInnerClass : ParameterizedCurve
	  {
		  private readonly ParameterizedCurveTest outerInstance;

		  public ParameterizedCurveAnonymousInnerClass(ParameterizedCurveTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> evaluate(final System.Nullable<double> x, final com.opengamma.strata.collect.array.DoubleArray parameters)
		  public override double? evaluate(double? x, DoubleArray parameters)
		  {
			assertEquals(3, parameters.size());
			double a = parameters.get(0);
			double b = parameters.get(1);
			double c = parameters.get(2);
			return a * Math.Sin(b * x) + c;
		  }

		  public override int NumberOfParameters
		  {
			  get
			  {
				return 3;
			  }
		  }

	  }

	  private class ParameterizedFunctionAnonymousInnerClass : ParameterizedFunction<double, DoubleArray, DoubleArray>
	  {
		  private readonly ParameterizedCurveTest outerInstance;

		  public ParameterizedFunctionAnonymousInnerClass(ParameterizedCurveTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


		  public override DoubleArray evaluate(double? x, DoubleArray parameters)
		  {
			double a = parameters.get(0);
			double b = parameters.get(1);
			DoubleArray res = DoubleArray.of(Math.Sin(b * x), x * a * Math.Cos(b * x), 1.0);
			return res;
		  }

		  public override int NumberOfParameters
		  {
			  get
			  {
				return 0;
			  }
		  }
	  }

	}

}