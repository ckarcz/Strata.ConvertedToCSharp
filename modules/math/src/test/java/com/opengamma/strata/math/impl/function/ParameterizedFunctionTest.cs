using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ScalarFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.ScalarFieldFirstOrderDifferentiator;
	using ScalarFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.ScalarFirstOrderDifferentiator;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParameterizedFunctionTest
	public class ParameterizedFunctionTest
	{

	  private static ParameterizedFunction<double, double[], double> ARRAY_PARAMS = new ParameterizedFunctionAnonymousInnerClass();

	  private class ParameterizedFunctionAnonymousInnerClass : ParameterizedFunction<double, double[], double>
	  {
		  public ParameterizedFunctionAnonymousInnerClass()
		  {
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> evaluate(final System.Nullable<double> x, final double[] a)
		  public override double? evaluate(double? x, double[] a)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = a.length;
			int n = a.Length;
			double sum = 0.0;
			for (int i = n - 1; i > 0; i--)
			{
			  sum += a[i];
			  sum *= x.Value;
			}
			sum += a[0];
			return sum;
		  }

		  public override int NumberOfParameters
		  {
			  get
			  {
				return 0;
			  }
		  }
	  }

	  private static ParameterizedFunction<double, DoubleArray, double> VECTOR_PARAMS = new ParameterizedFunctionAnonymousInnerClass2();

	  private class ParameterizedFunctionAnonymousInnerClass2 : ParameterizedFunction<double, DoubleArray, double>
	  {
		  public ParameterizedFunctionAnonymousInnerClass2()
		  {
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> evaluate(final System.Nullable<double> x, final com.opengamma.strata.collect.array.DoubleArray a)
		  public override double? evaluate(double? x, DoubleArray a)
		  {
			ArgChecker.notNull(a, "parameters");
			if (a.size() != 2)
			{
			  throw new System.ArgumentException("wrong number of parameters");
			}
			return a.get(0) * Math.Sin(a.get(1) * x);
		  }

		  public override int NumberOfParameters
		  {
			  get
			  {
				return 0;
			  }
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testCubic()
	  public virtual void testCubic()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] parms = new double[] {3.0, -1.0, 1.0, 1.0 };
		double[] parms = new double[] {3.0, -1.0, 1.0, 1.0};
		assertEquals(13.0, ARRAY_PARAMS.evaluate(2.0, parms), 0.0);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> func = ARRAY_PARAMS.asFunctionOfArguments(parms);
		System.Func<double, double> func = ARRAY_PARAMS.asFunctionOfArguments(parms);
		assertEquals(4.0, func(-1.0), 0.0);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double[], double> param_func = ARRAY_PARAMS.asFunctionOfParameters(0.0);
		System.Func<double[], double> param_func = ARRAY_PARAMS.asFunctionOfParameters(0.0);
		assertEquals(10.0, param_func(new double[] {10, 312, 423, 534}), 0.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testSin()
	  public virtual void testSin()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray parms = com.opengamma.strata.collect.array.DoubleArray.of(-1.0, 0.5);
		DoubleArray parms = DoubleArray.of(-1.0, 0.5);
		assertEquals(-Math.Sin(1.0), VECTOR_PARAMS.evaluate(2.0, parms), 0.0);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> func = VECTOR_PARAMS.asFunctionOfArguments(parms);
		System.Func<double, double> func = VECTOR_PARAMS.asFunctionOfArguments(parms);
		assertEquals(1.0, func(-Math.PI), 0.0);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.differentiation.ScalarFirstOrderDifferentiator diff = new com.opengamma.strata.math.impl.differentiation.ScalarFirstOrderDifferentiator();
		ScalarFirstOrderDifferentiator diff = new ScalarFirstOrderDifferentiator();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> grad = diff.differentiate(func);
		System.Func<double, double> grad = diff.differentiate(func);
		assertEquals(-0.5, grad(0.0), 1e-8);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double> params_func = VECTOR_PARAMS.asFunctionOfParameters(1.0);
		System.Func<DoubleArray, double> params_func = VECTOR_PARAMS.asFunctionOfParameters(1.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.differentiation.ScalarFieldFirstOrderDifferentiator vdiff = new com.opengamma.strata.math.impl.differentiation.ScalarFieldFirstOrderDifferentiator();
		ScalarFieldFirstOrderDifferentiator vdiff = new ScalarFieldFirstOrderDifferentiator();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleArray> vgrad = vdiff.differentiate(params_func);
		System.Func<DoubleArray, DoubleArray> vgrad = vdiff.differentiate(params_func);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray res = vgrad.apply(com.opengamma.strata.collect.array.DoubleArray.of(Math.PI, 0));
		DoubleArray res = vgrad(DoubleArray.of(Math.PI, 0));
		assertEquals(0.0, res.get(0), 1e-8);
		assertEquals(Math.PI, res.get(1), 1e-8);
	  }
	}

}