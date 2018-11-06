using System;
using System.Collections;

/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using VectorFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.VectorFieldFirstOrderDifferentiator;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NonLinearTransformFunctionTest
	public class NonLinearTransformFunctionTest
	{

	  private static readonly ParameterLimitsTransform[] NULL_TRANSFORMS;
	  private static readonly ParameterLimitsTransform[] TRANSFORMS;

	  private static readonly System.Func<DoubleArray, DoubleArray> FUNCTION = (DoubleArray x) =>
	  {
  ArgChecker.isTrue(x.size() == 2);
  double x1 = x.get(0);
  double x2 = x.get(1);
  return DoubleArray.of(Math.Sin(x1) * Math.Cos(x2), Math.Sin(x1) * Math.Sin(x2), Math.Cos(x1));
	  };

	  private static readonly System.Func<DoubleArray, DoubleMatrix> JACOBIAN = (DoubleArray x) =>
	  {
  ArgChecker.isTrue(x.size() == 2);
  double x1 = x.get(0);
  double x2 = x.get(1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] y = new double[3][2];
  double[][] y = RectangularArrays.ReturnRectangularDoubleArray(3, 2);
  y[0][0] = Math.Cos(x1) * Math.Cos(x2);
  y[0][1] = -Math.Sin(x1) * Math.Sin(x2);
  y[1][0] = Math.Cos(x1) * Math.Sin(x2);
  y[1][1] = Math.Sin(x1) * Math.Cos(x2);
  y[2][0] = -Math.Sin(x1);
  y[2][1] = 0;
  return DoubleMatrix.copyOf(y);
	  };

	  static NonLinearTransformFunctionTest()
	  {
		NULL_TRANSFORMS = new ParameterLimitsTransform[2];
		NULL_TRANSFORMS[0] = new NullTransform();
		NULL_TRANSFORMS[1] = new NullTransform();

		TRANSFORMS = new ParameterLimitsTransform[2];
		TRANSFORMS[0] = new DoubleRangeLimitTransform(0, Math.PI);
		TRANSFORMS[1] = new SingleRangeLimitTransform(0, ParameterLimitsTransform_LimitType.GREATER_THAN);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNullTransform()
	  public virtual void testNullTransform()
	  {
		BitArray @fixed = new BitArray();
		@fixed.Set(0, true);
		DoubleArray start = DoubleArray.of(Math.PI / 4, 1);
		UncoupledParameterTransforms transforms = new UncoupledParameterTransforms(start, NULL_TRANSFORMS, @fixed);
		NonLinearTransformFunction transFunc = new NonLinearTransformFunction(FUNCTION, JACOBIAN, transforms);
		System.Func<DoubleArray, DoubleArray> func = transFunc.FittingFunction;
		System.Func<DoubleArray, DoubleMatrix> jacFunc = transFunc.FittingJacobian;

		DoubleArray x = DoubleArray.of(0.5);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double rootHalf = Math.sqrt(0.5);
		double rootHalf = Math.Sqrt(0.5);
		DoubleArray y = func(x);
		assertEquals(3, y.size());
		assertEquals(rootHalf * Math.Cos(0.5), y.get(0), 1e-9);
		assertEquals(rootHalf * Math.Sin(0.5), y.get(1), 1e-9);
		assertEquals(rootHalf, y.get(2), 1e-9);

		DoubleMatrix jac = jacFunc(x);
		assertEquals(3, jac.rowCount());
		assertEquals(1, jac.columnCount());
		assertEquals(-rootHalf * Math.Sin(0.5), jac.get(0, 0), 1e-9);
		assertEquals(rootHalf * Math.Cos(0.5), jac.get(1, 0), 1e-9);
		assertEquals(0, jac.get(2, 0), 1e-9);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNonLinearTransform()
	  public virtual void testNonLinearTransform()
	  {
		BitArray @fixed = new BitArray();
		DoubleArray start = DoubleArray.filled(2);
		UncoupledParameterTransforms transforms = new UncoupledParameterTransforms(start, TRANSFORMS, @fixed);
		NonLinearTransformFunction transFunc = new NonLinearTransformFunction(FUNCTION, JACOBIAN, transforms);
		System.Func<DoubleArray, DoubleArray> func = transFunc.FittingFunction;
		System.Func<DoubleArray, DoubleMatrix> jacFunc = transFunc.FittingJacobian;

		VectorFieldFirstOrderDifferentiator diff = new VectorFieldFirstOrderDifferentiator();
		System.Func<DoubleArray, DoubleMatrix> jacFuncFD = diff.differentiate(func);

		DoubleArray testPoint = DoubleArray.of(4.5, -2.1);
		DoubleMatrix jac = jacFunc(testPoint);
		DoubleMatrix jacFD = jacFuncFD(testPoint);
		assertEquals(3, jac.rowCount());
		assertEquals(2, jac.columnCount());

		for (int i = 0; i < 3; i++)
		{
		  for (int j = 0; j < 2; j++)
		  {
			assertEquals(jacFD.get(i, j), jac.get(i, j), 1e-6);
		  }
		}
	  }
	}

}