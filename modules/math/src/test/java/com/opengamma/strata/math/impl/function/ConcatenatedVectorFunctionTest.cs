using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using VectorFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.VectorFieldFirstOrderDifferentiator;
	using AssertMatrix = com.opengamma.strata.math.impl.util.AssertMatrix;

	/// <summary>
	/// Create a few <seealso cref="VectorFunction"/> (as anonymous inner classes) and check they concatenate correctly 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ConcatenatedVectorFunctionTest
	public class ConcatenatedVectorFunctionTest
	{

	  private static readonly VectorFieldFirstOrderDifferentiator DIFF = new VectorFieldFirstOrderDifferentiator();

	  private const int NUM_FUNCS = 3;
	  private static readonly VectorFunction[] F = new VectorFunction[NUM_FUNCS];

	  private static readonly DoubleArray[] X = new DoubleArray[3];
	  private static readonly DoubleArray[] Y_EXP = new DoubleArray[3];
	  private static readonly DoubleMatrix[] JAC_EXP = new DoubleMatrix[3];

	  static ConcatenatedVectorFunctionTest()
	  {
		F[0] = new VectorFunctionAnonymousInnerClass();

		F[1] = new VectorFunctionAnonymousInnerClass2();

		F[2] = new VectorFunctionAnonymousInnerClass3();

		X[0] = DoubleArray.of(-2.0, 2.0);
		X[1] = DoubleArray.of(1.0, 2.0);
		X[2] = DoubleArray.of(Math.PI);

		Y_EXP[0] = DoubleArray.of(2.0);
		Y_EXP[1] = DoubleArray.of(2.0, 4.0);
		Y_EXP[2] = DoubleArray.of(Math.PI, 0.0);
		JAC_EXP[0] = DoubleMatrix.of(1, 2, 1d, 2d);
		JAC_EXP[1] = DoubleMatrix.of(2, 2, 2d, 1d, 0d, 4d);
		JAC_EXP[2] = DoubleMatrix.of(2, 1, 1d, -1d);
	  }

	  private class VectorFunctionAnonymousInnerClass : VectorFunction
	  {
		  public VectorFunctionAnonymousInnerClass()
		  {
		  }


		  public override DoubleArray apply(DoubleArray x)
		  {
			return DoubleArray.filled(1, x.get(0) + 2 * x.get(1));
		  }

		  public override DoubleMatrix calculateJacobian(DoubleArray x)
		  {
			return DoubleMatrix.of(1, 2, 1d, 2d);
		  }

		  public override int LengthOfDomain
		  {
			  get
			  {
				return 2;
			  }
		  }

		  public override int LengthOfRange
		  {
			  get
			  {
				return 1;
			  }
		  }
	  }

	  private class VectorFunctionAnonymousInnerClass2 : VectorFunction
	  {
		  public VectorFunctionAnonymousInnerClass2()
		  {
		  }


		  public override DoubleArray apply(DoubleArray x)
		  {
			double x1 = x.get(0);
			double x2 = x.get(1);
			double y1 = x1 * x2;
			double y2 = x2 * x2;
			return DoubleArray.of(y1, y2);
		  }

		  public override DoubleMatrix calculateJacobian(DoubleArray x)
		  {
			double x1 = x.get(0);
			double x2 = x.get(1);
			double j11 = x2;
			double j12 = x1;
			double j21 = 0.0;
			double j22 = 2 * x2;
			return DoubleMatrix.of(2, 2, j11, j12, j21, j22);
		  }

		  public override int LengthOfDomain
		  {
			  get
			  {
				return 2;
			  }
		  }

		  public override int LengthOfRange
		  {
			  get
			  {
				return 2;
			  }
		  }
	  }

	  private class VectorFunctionAnonymousInnerClass3 : VectorFunction
	  {
		  public VectorFunctionAnonymousInnerClass3()
		  {
		  }


		  public override DoubleArray apply(DoubleArray x)
		  {
			double x1 = x.get(0);
			double y1 = x1;
			double y2 = Math.Sin(x1);
			return DoubleArray.of(y1, y2);
		  }

		  public override DoubleMatrix calculateJacobian(DoubleArray x)
		  {
			double x1 = x.get(0);
			double j11 = 1.0;
			double j21 = Math.Cos(x1);
			return DoubleMatrix.of(2, 1, j11, j21);
		  }

		  public override int LengthOfDomain
		  {
			  get
			  {
				return 1;
			  }
		  }

		  public override int LengthOfRange
		  {
			  get
			  {
				return 2;
			  }
		  }
	  }

	  /// <summary>
	  /// /check individual functions first
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void functionsTest()
	  public virtual void functionsTest()
	  {

		for (int i = 0; i < 3; i++)
		{
		  DoubleArray y = F[i].apply(X[i]);
		  DoubleMatrix jac = F[i].calculateJacobian(X[i]);
		  AssertMatrix.assertEqualsVectors(Y_EXP[i], y, 1e-15);
		  AssertMatrix.assertEqualsMatrix(JAC_EXP[i], jac, 1e-15);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void conCatTest()
	  public virtual void conCatTest()
	  {
		DoubleArray cx = X[0].concat(X[1]).concat(X[2]);
		DoubleArray cyExp = Y_EXP[0].concat(Y_EXP[1]).concat(Y_EXP[2]);
		ConcatenatedVectorFunction cf = new ConcatenatedVectorFunction(F);
		DoubleArray cy = cf.apply(cx);
		AssertMatrix.assertEqualsVectors(cyExp, cy, 1e-15);

		DoubleMatrix cJac = cf.calculateJacobian(cx);
		DoubleMatrix fdJac = DIFF.differentiate(cf).apply(cx);
		AssertMatrix.assertEqualsMatrix(fdJac, cJac, 1e-10);
	  }

	}

}