using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using AssertMatrix = com.opengamma.strata.math.impl.util.AssertMatrix;

	/// <summary>
	/// Construct a curve a + b*x + c*x^2 (where a, b, and c are the parameters), then make some VectorFunctions
	/// that sample the curve at some values of x, thus providing a mapping from the model parameters to the curve
	/// value at the sample positions. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DoublesVectorFunctionProviderTest
	public class DoublesVectorFunctionProviderTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
		public virtual void test()
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParameterizedCurve curve = new ParameterizedCurve()
		ParameterizedCurve curve = new ParameterizedCurveAnonymousInnerClass(this);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final DoublesVectorFunctionProvider pro = new DoublesVectorFunctionProvider()
		DoublesVectorFunctionProvider pro = new DoublesVectorFunctionProviderAnonymousInnerClass(this, curve);

		//a = -2, b = 1, c = 0.5
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray parms = com.opengamma.strata.collect.array.DoubleArray.of(-2.0, 1.0, 0.5);
		DoubleArray parms = DoubleArray.of(-2.0, 1.0, 0.5);

		//sample the curve at x = -1, 0, and 1 
		VectorFunction f = pro.from(new double?[] {-1.0, 0.0, 1.0});
		DoubleArray y = f.apply(parms);
		AssertMatrix.assertEqualsVectors(DoubleArray.of(-2.5, -2.0, -0.5), y, 1e-15);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<double> l = new java.util.ArrayList<>(3);
		IList<double> l = new List<double>(3);
		l.Add(0.0);
		l.Add(2.0);
		l.Add(4.0);
		f = pro.from(l);
		y = f.apply(parms);
		AssertMatrix.assertEqualsVectors(DoubleArray.of(-2.0, 2.0, 10.0), y, 1e-15);
		}

	  private class ParameterizedCurveAnonymousInnerClass : ParameterizedCurve
	  {
		  private readonly DoublesVectorFunctionProviderTest outerInstance;

		  public ParameterizedCurveAnonymousInnerClass(DoublesVectorFunctionProviderTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> evaluate(final System.Nullable<double> x, final com.opengamma.strata.collect.array.DoubleArray parameters)
		  public override double? evaluate(double? x, DoubleArray parameters)
		  {
			return parameters.get(0) + parameters.get(1) * x + parameters.get(2) * x * x;
		  }

		  public override int NumberOfParameters
		  {
			  get
			  {
				return 3;
			  }
		  }
	  }

	  private class DoublesVectorFunctionProviderAnonymousInnerClass : DoublesVectorFunctionProvider
	  {
		  private readonly DoublesVectorFunctionProviderTest outerInstance;

		  private com.opengamma.strata.math.impl.function.ParameterizedCurve curve;

		  public DoublesVectorFunctionProviderAnonymousInnerClass(DoublesVectorFunctionProviderTest outerInstance, com.opengamma.strata.math.impl.function.ParameterizedCurve curve)
		  {
			  this.outerInstance = outerInstance;
			  this.curve = curve;
		  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public VectorFunction from(final double[] x)
		  public override VectorFunction from(double[] x)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParameterizedCurveVectorFunction vf = new ParameterizedCurveVectorFunction(x, curve);
			ParameterizedCurveVectorFunction vf = new ParameterizedCurveVectorFunction(x, curve);
			return vf;
		  }
	  }

	}

}