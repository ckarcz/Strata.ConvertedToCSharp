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
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using VectorFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.VectorFieldFirstOrderDifferentiator;
	using AssertMatrix = com.opengamma.strata.math.impl.util.AssertMatrix;

	/// <summary>
	/// Test simple a simple function a * Math.sinh(b * x)
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParameterizedCurveVectorFunctionTest
	public class ParameterizedCurveVectorFunctionTest
	{

	  private static readonly ParameterizedCurve s_PCurve;

	  static ParameterizedCurveVectorFunctionTest()
	  {
		s_PCurve = new ParameterizedCurveAnonymousInnerClass();
	  }

	  private class ParameterizedCurveAnonymousInnerClass : ParameterizedCurve
	  {
		  public ParameterizedCurveAnonymousInnerClass()
		  {
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> evaluate(final System.Nullable<double> x, final com.opengamma.strata.collect.array.DoubleArray parameters)
		  public override double? evaluate(double? x, DoubleArray parameters)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a = parameters.get(0);
			double a = parameters.get(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double b = parameters.get(1);
			double b = parameters.get(1);
			return a * Math.Sinh(b * x);
		  }

		  public override int NumberOfParameters
		  {
			  get
			  {
				return 2;
			  }
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParameterizedCurveVectorFunctionProvider pro = new ParameterizedCurveVectorFunctionProvider(s_PCurve);
		ParameterizedCurveVectorFunctionProvider pro = new ParameterizedCurveVectorFunctionProvider(s_PCurve);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] points = new double[] {-1.0, 0.0, 1.0 };
		double[] points = new double[] {-1.0, 0.0, 1.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final VectorFunction f = pro.from(points);
		VectorFunction f = pro.from(points);
		assertEquals(2, f.LengthOfDomain);
		assertEquals(3, f.LengthOfRange);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x = com.opengamma.strata.collect.array.DoubleArray.of(0.5, 2.0);
		DoubleArray x = DoubleArray.of(0.5, 2.0); //the parameters a & b
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray y = f.apply(x);
		DoubleArray y = f.apply(x);
		assertEquals(0.5 * Math.Sinh(-2.0), y.get(0), 1e-14);
		assertEquals(0.0, y.get(1), 1e-14);
		assertEquals(0.5 * Math.Sinh(2.0), y.get(2), 1e-14);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix jac = f.calculateJacobian(x);
		DoubleMatrix jac = f.calculateJacobian(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix fdJac = (new com.opengamma.strata.math.impl.differentiation.VectorFieldFirstOrderDifferentiator().differentiate(f)).apply(x);
		DoubleMatrix fdJac = ((new VectorFieldFirstOrderDifferentiator()).differentiate(f)).apply(x);
		AssertMatrix.assertEqualsMatrix(fdJac, jac, 1e-9);
	  }
	}

}