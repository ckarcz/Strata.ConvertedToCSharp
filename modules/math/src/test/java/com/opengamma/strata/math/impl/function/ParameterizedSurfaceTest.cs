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
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;

	/// <summary>
	/// Set up a simple parameterised surface (based on the function a * Math.sin(b * x + c * y) + Math.cos(y), where a, b, & c are the parameters)
	/// and check the finite difference sensitivity (the default behaviour of getYParameterSensitivity) agrees with the analytic 
	/// calculation for a range of points along the curve.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParameterizedSurfaceTest
	public class ParameterizedSurfaceTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
		public virtual void test()
		{

		/// <summary>
		/// Take the form $y = a\sin(bx + cy) + cos(y)$
		/// </summary>
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParameterizedSurface testSurface = new ParameterizedSurface()
		ParameterizedSurface testSurface = new ParameterizedSurfaceAnonymousInnerClass(this);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParameterizedFunction<com.opengamma.strata.collect.tuple.DoublesPair, com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleArray> parmSense = new ParameterizedFunction<com.opengamma.strata.collect.tuple.DoublesPair, com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleArray>()
		ParameterizedFunction<DoublesPair, DoubleArray, DoubleArray> parmSense = new ParameterizedFunctionAnonymousInnerClass(this);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray params = com.opengamma.strata.collect.array.DoubleArray.of(0.7, -0.3, 1.2);
		DoubleArray @params = DoubleArray.of(0.7, -0.3, 1.2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<com.opengamma.strata.collect.tuple.DoublesPair, com.opengamma.strata.collect.array.DoubleArray> paramsSenseFD = testSurface.getZParameterSensitivity(params);
		System.Func<DoublesPair, DoubleArray> paramsSenseFD = testSurface.getZParameterSensitivity(@params);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<com.opengamma.strata.collect.tuple.DoublesPair, com.opengamma.strata.collect.array.DoubleArray> paramsSenseAnal = parmSense.asFunctionOfArguments(params);
		System.Func<DoublesPair, DoubleArray> paramsSenseAnal = parmSense.asFunctionOfArguments(@params);

		for (int i = 0; i < 20; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = Math.PI * (-0.5 + i / 19.0);
		  double x = Math.PI * (-0.5 + i / 19.0);
		  for (int j = 0; j < 20; j++)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double y = Math.PI * (-0.5 + j / 19.0);
			double y = Math.PI * (-0.5 + j / 19.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.tuple.DoublesPair xy = com.opengamma.strata.collect.tuple.DoublesPair.of(x, y);
			DoublesPair xy = DoublesPair.of(x, y);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray s1 = paramsSenseAnal.apply(xy);
			DoubleArray s1 = paramsSenseAnal(xy);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray s2 = paramsSenseFD.apply(xy);
			DoubleArray s2 = paramsSenseFD(xy);
			for (int k = 0; k < 3; k++)
			{
			  assertEquals(s1.get(k), s2.get(k), 1e-10);
			}
		  }
		}

		}

	  private class ParameterizedSurfaceAnonymousInnerClass : ParameterizedSurface
	  {
		  private readonly ParameterizedSurfaceTest outerInstance;

		  public ParameterizedSurfaceAnonymousInnerClass(ParameterizedSurfaceTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> evaluate(final com.opengamma.strata.collect.tuple.DoublesPair xy, final com.opengamma.strata.collect.array.DoubleArray parameters)
		  public override double? evaluate(DoublesPair xy, DoubleArray parameters)
		  {
			assertEquals(3, parameters.size());
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a = parameters.get(0);
			double a = parameters.get(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double b = parameters.get(1);
			double b = parameters.get(1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double c = parameters.get(2);
			double c = parameters.get(2);
			return a * Math.Sin(b * xy.First + c * xy.Second) + Math.Cos(xy.Second);
		  }

		  public override int NumberOfParameters
		  {
			  get
			  {
				return 3;
			  }
		  }
	  }

	  private class ParameterizedFunctionAnonymousInnerClass : ParameterizedFunction<DoublesPair, DoubleArray, DoubleArray>
	  {
		  private readonly ParameterizedSurfaceTest outerInstance;

		  public ParameterizedFunctionAnonymousInnerClass(ParameterizedSurfaceTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleArray evaluate(final com.opengamma.strata.collect.tuple.DoublesPair xy, final com.opengamma.strata.collect.array.DoubleArray parameters)
		  public override DoubleArray evaluate(DoublesPair xy, DoubleArray parameters)
		  {
			double a = parameters.get(0);
			double b = parameters.get(1);
			double c = parameters.get(2);
			DoubleArray res = DoubleArray.of(Math.Sin(b * xy.First + c * xy.Second), xy.First * a * Math.Cos(b * xy.First + c * xy.Second), xy.Second * a * Math.Cos(b * xy.First + c * xy.Second));
			return res;
		  }

		  public override int NumberOfParameters
		  {
			  get
			  {
				return 3;
			  }
		  }
	  }
	}

}