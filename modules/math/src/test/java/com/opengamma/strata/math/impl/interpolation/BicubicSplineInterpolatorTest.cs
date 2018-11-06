using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory.OG_ALGEBRA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using PiecewisePolynomialFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D;
	using PiecewisePolynomialFunction2D = com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction2D;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BicubicSplineInterpolatorTest
	public class BicubicSplineInterpolatorTest
	{

	  private const double EPS = 1e-12;
	  private const double INF = 1.0 / 0.0;

	  /// 
	  public virtual void linearTest()
	  {
		double[] x0Values = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] x1Values = new double[] {-1.0, 0.0, 1.0, 2.0, 3.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n0Data = x0Values.length;
		int n0Data = x0Values.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n1Data = x1Values.length;
		int n1Data = x1Values.Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] yValues = new double[n0Data][n1Data];
		double[][] yValues = RectangularArrays.ReturnRectangularDoubleArray(n0Data, n1Data);

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
			yValues[i][j] = (x0Values[i] + 2.0) * (x1Values[j] + 5.0);
		  }
		}

		CubicSplineInterpolator method = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator2D interp = new BicubicSplineInterpolator(new CubicSplineInterpolator[] {method, method});
		PiecewisePolynomialResult2D result = interp.interpolate(x0Values, x1Values, yValues);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n0IntExp = n0Data - 1;
		int n0IntExp = n0Data - 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n1IntExp = n1Data - 1;
		int n1IntExp = n1Data - 1;
		const int orderExp = 4;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] coefsExp = new DoubleMatrix[n0Data - 1][n1Data - 1];
		DoubleMatrix[][] coefsExp = RectangularArrays.ReturnRectangularDoubleMatrixArray(n0Data - 1, n1Data - 1);
		for (int i = 0; i < n0Data - 1; ++i)
		{
		  for (int j = 0; j < n1Data - 1; ++j)
		  {
			coefsExp[i][j] = DoubleMatrix.ofUnsafe(new double[][]
			{
				new double[] {0.0, 0.0, 0.0, 0.0},
				new double[] {0.0, 0.0, 0.0, 0.0},
				new double[] {0.0, 0.0, 1.0, (5.0 + x1Values[j])},
				new double[] {0.0, 0.0, (2.0 + x0Values[i]), (2.0 + x0Values[i]) * (5.0 + x1Values[j])}
			});
		  }
		}

		assertEquals(result.NumberOfIntervals[0], n0IntExp);
		assertEquals(result.NumberOfIntervals[1], n1IntExp);
		assertEquals(result.Order[0], orderExp);
		assertEquals(result.Order[1], orderExp);

		const int n0Keys = 51;
		const int n1Keys = 61;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 5.0 * i / (n0Keys - 1);
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = -2.0 + 6.0 * i / (n1Keys - 1);
		}

		for (int i = 0; i < n0Data; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(x0Values[i]) == 0.0 ? 1.0 : Math.abs(x0Values[i]);
		  double @ref = Math.Abs(x0Values[i]) == 0.0 ? 1.0 : Math.Abs(x0Values[i]);
		  assertEquals(result.Knots0.get(i), x0Values[i], @ref * EPS);
		  assertEquals(result.Knots2D[0].get(i), x0Values[i], @ref * EPS);
		}
		for (int i = 0; i < n1Data; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(x1Values[i]) == 0.0 ? 1.0 : Math.abs(x1Values[i]);
		  double @ref = Math.Abs(x1Values[i]) == 0.0 ? 1.0 : Math.Abs(x1Values[i]);
		  assertEquals(result.Knots1.get(i), x1Values[i], @ref * EPS);
		  assertEquals(result.Knots2D[1].get(i), x1Values[i], @ref * EPS);
		}
		for (int i = 0; i < n0Data - 1; ++i)
		{
		  for (int j = 0; j < n1Data - 1; ++j)
		  {
			for (int k = 0; k < orderExp; ++k)
			{
			  for (int l = 0; l < orderExp; ++l)
			  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(coefsExp[i][j].get(k, l)) == 0.0 ? 1.0 : Math.abs(coefsExp[i][j].get(k, l));
				double @ref = Math.Abs(coefsExp[i][j].get(k, l)) == 0.0 ? 1.0 : Math.Abs(coefsExp[i][j].get(k, l));
				assertEquals(result.Coefs[i][j].get(k, l), coefsExp[i][j].get(k, l), @ref * EPS);
			  }
			}
		  }
		}

		DoubleMatrix resValues = interp.interpolate(x0Values, x1Values, yValues, x0Keys, x1Keys);

		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double expVal = (x0Keys[i] + 2.0) * (x1Keys[j] + 5.0);
			double expVal = (x0Keys[i] + 2.0) * (x1Keys[j] + 5.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(expVal) == 0.0 ? 1.0 : Math.abs(expVal);
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resValues.get(i, j), expVal, @ref * EPS);
		  }
		}
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double expVal = (x0Keys[i] + 2.0) * (x1Keys[j] + 5.0);
			double expVal = (x0Keys[i] + 2.0) * (x1Keys[j] + 5.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(expVal) == 0.0 ? 1.0 : Math.abs(expVal);
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resValues.get(i, j), expVal, @ref * EPS);
		  }
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double expVal = (x0Keys[1] + 2.0) * (x1Keys[2] + 5.0);
		  double expVal = (x0Keys[1] + 2.0) * (x1Keys[2] + 5.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(expVal) == 0.0 ? 1.0 : Math.abs(expVal);
		  double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
		  assertEquals(interp.interpolate(x0Values, x1Values, yValues, x0Keys[1], x1Keys[2]), expVal, @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double expVal = (x0Keys[23] + 2.0) * (x1Keys[20] + 5.0);
		  double expVal = (x0Keys[23] + 2.0) * (x1Keys[20] + 5.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(expVal) == 0.0 ? 1.0 : Math.abs(expVal);
		  double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
		  assertEquals(interp.interpolate(x0Values, x1Values, yValues, x0Keys[23], x1Keys[20]), expVal, @ref * EPS);
		}

	  }

	  /// <summary>
	  /// f(x0,x1) = ( x0 - 1.5)^2 * (x1  - 2.)^2
	  /// </summary>
	  public virtual void quadraticTest()
	  {
		double[] x0Values = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] x1Values = new double[] {-1.0, 0.0, 1.0, 2.0, 3.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n0Data = x0Values.length;
		int n0Data = x0Values.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n1Data = x1Values.length;
		int n1Data = x1Values.Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] yValues = new double[n0Data][n1Data];
		double[][] yValues = RectangularArrays.ReturnRectangularDoubleArray(n0Data, n1Data);

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
			yValues[i][j] = (x0Values[i] - 1.5) * (x0Values[i] - 1.5) * (x1Values[j] - 2.0) * (x1Values[j] - 2.0);
		  }
		}

		CubicSplineInterpolator method = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator2D interp = new BicubicSplineInterpolator(method);
		PiecewisePolynomialResult2D result = interp.interpolate(x0Values, x1Values, yValues);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n0IntExp = n0Data - 1;
		int n0IntExp = n0Data - 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n1IntExp = n1Data - 1;
		int n1IntExp = n1Data - 1;
		const int orderExp = 4;

		const int n0Keys = 51;
		const int n1Keys = 61;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 5.0 * i / (n0Keys - 1);
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = -2.0 + 6.0 * i / (n1Keys - 1);
		}

		assertEquals(result.NumberOfIntervals[0], n0IntExp);
		assertEquals(result.NumberOfIntervals[1], n1IntExp);
		assertEquals(result.Order[0], orderExp);
		assertEquals(result.Order[1], orderExp);

		for (int i = 0; i < n0Data; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(x0Values[i]) == 0.0 ? 1.0 : Math.abs(x0Values[i]);
		  double @ref = Math.Abs(x0Values[i]) == 0.0 ? 1.0 : Math.Abs(x0Values[i]);
		  assertEquals(result.Knots0.get(i), x0Values[i], @ref * EPS);
		  assertEquals(result.Knots2D[0].get(i), x0Values[i], @ref * EPS);
		}
		for (int i = 0; i < n1Data; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(x1Values[i]) == 0.0 ? 1.0 : Math.abs(x1Values[i]);
		  double @ref = Math.Abs(x1Values[i]) == 0.0 ? 1.0 : Math.Abs(x1Values[i]);
		  assertEquals(result.Knots1.get(i), x1Values[i], @ref * EPS);
		  assertEquals(result.Knots2D[1].get(i), x1Values[i], @ref * EPS);
		}

		for (int i = 0; i < n0Data - 1; ++i)
		{
		  for (int j = 0; j < n1Data - 1; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(yValues[i][j]) == 0.0 ? 1.0 : Math.abs(yValues[i][j]);
			double @ref = Math.Abs(yValues[i][j]) == 0.0 ? 1.0 : Math.Abs(yValues[i][j]);
			assertEquals(result.Coefs[i][j].get(orderExp - 1, orderExp - 1), yValues[i][j], @ref * EPS);
		  }
		}

		DoubleMatrix resValues = interp.interpolate(x0Values, x1Values, yValues, x0Values, x1Values);
		PiecewisePolynomialFunction2D func2D = new PiecewisePolynomialFunction2D();
		DoubleMatrix resDiffX0 = func2D.differentiateX0(result, x0Values, x1Values);
		DoubleMatrix resDiffX1 = func2D.differentiateX1(result, x0Values, x1Values);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D func1D = new com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D();
		PiecewisePolynomialFunction1D func1D = new PiecewisePolynomialFunction1D();
		DoubleMatrix expDiffX0 = func1D.differentiate(method.interpolate(x0Values, OG_ALGEBRA.getTranspose(DoubleMatrix.copyOf(yValues)).toArray()), x0Values);
		DoubleMatrix expDiffX1 = func1D.differentiate(method.interpolate(x1Values, yValues), x1Values);

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double expVal = expDiffX1.get(i, j);
			double expVal = expDiffX1.get(i, j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(expVal) == 0.0 ? 1.0 : Math.abs(expVal);
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resDiffX1.get(i, j), expVal, @ref * EPS);
		  }
		}

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double expVal = expDiffX0.get(j, i);
			double expVal = expDiffX0.get(j, i);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(expVal) == 0.0 ? 1.0 : Math.abs(expVal);
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resDiffX0.get(i, j), expVal, @ref * EPS);
		  }
		}

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double expVal = yValues[i][j];
			double expVal = yValues[i][j];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(expVal) == 0.0 ? 1.0 : Math.abs(expVal);
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resValues.get(i, j), expVal, @ref * EPS);
		  }
		}

	  }

	  /// <summary>
	  /// f(x0,x1) = ( x0 - 1.)^3 * (x1  + 14./13.)^3
	  /// </summary>
	  public virtual void cubicTest()
	  {
		double[] x0Values = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] x1Values = new double[] {-1.0, 0.0, 1.0, 2.0, 3.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n0Data = x0Values.length;
		int n0Data = x0Values.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n1Data = x1Values.length;
		int n1Data = x1Values.Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] yValues = new double[n0Data][n1Data];
		double[][] yValues = RectangularArrays.ReturnRectangularDoubleArray(n0Data, n1Data);

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
			yValues[i][j] = (x0Values[i] - 1.0) * (x0Values[i] - 1.0) * (x0Values[i] - 1.0) * (x1Values[j] + 14.0 / 13.0) * (x1Values[j] + 14.0 / 13.0) * (x1Values[j] + 14.0 / 13.0);
		  }
		}

		CubicSplineInterpolator method = new CubicSplineInterpolator();
		PiecewisePolynomialInterpolator2D interp = new BicubicSplineInterpolator(method);
		PiecewisePolynomialResult2D result = interp.interpolate(x0Values, x1Values, yValues);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n0IntExp = n0Data - 1;
		int n0IntExp = n0Data - 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n1IntExp = n1Data - 1;
		int n1IntExp = n1Data - 1;
		const int orderExp = 4;

		const int n0Keys = 51;
		const int n1Keys = 61;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 5.0 * i / (n0Keys - 1);
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = -2.0 + 6.0 * i / (n1Keys - 1);
		}

		assertEquals(result.NumberOfIntervals[0], n0IntExp);
		assertEquals(result.NumberOfIntervals[1], n1IntExp);
		assertEquals(result.Order[0], orderExp);
		assertEquals(result.Order[1], orderExp);

		for (int i = 0; i < n0Data; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(x0Values[i]) == 0.0 ? 1.0 : Math.abs(x0Values[i]);
		  double @ref = Math.Abs(x0Values[i]) == 0.0 ? 1.0 : Math.Abs(x0Values[i]);
		  assertEquals(result.Knots0.get(i), x0Values[i], @ref * EPS);
		  assertEquals(result.Knots2D[0].get(i), x0Values[i], @ref * EPS);
		}
		for (int i = 0; i < n1Data; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(x1Values[i]) == 0.0 ? 1.0 : Math.abs(x1Values[i]);
		  double @ref = Math.Abs(x1Values[i]) == 0.0 ? 1.0 : Math.Abs(x1Values[i]);
		  assertEquals(result.Knots1.get(i), x1Values[i], @ref * EPS);
		  assertEquals(result.Knots2D[1].get(i), x1Values[i], @ref * EPS);
		}

		for (int i = 0; i < n0Data - 1; ++i)
		{
		  for (int j = 0; j < n1Data - 1; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(yValues[i][j]) == 0.0 ? 1.0 : Math.abs(yValues[i][j]);
			double @ref = Math.Abs(yValues[i][j]) == 0.0 ? 1.0 : Math.Abs(yValues[i][j]);
			assertEquals(result.Coefs[i][j].get(orderExp - 1, orderExp - 1), yValues[i][j], @ref * EPS);
		  }
		}

		DoubleMatrix resValues = interp.interpolate(x0Values, x1Values, yValues, x0Values, x1Values);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction2D func2D = new com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction2D();
		PiecewisePolynomialFunction2D func2D = new PiecewisePolynomialFunction2D();
		DoubleMatrix resDiffX0 = func2D.differentiateX0(result, x0Values, x1Values);
		DoubleMatrix resDiffX1 = func2D.differentiateX1(result, x0Values, x1Values);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D func1D = new com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D();
		PiecewisePolynomialFunction1D func1D = new PiecewisePolynomialFunction1D();
		DoubleMatrix expDiffX0 = func1D.differentiate(method.interpolate(x0Values, OG_ALGEBRA.getTranspose(DoubleMatrix.copyOf(yValues)).toArray()), x0Values);
		DoubleMatrix expDiffX1 = func1D.differentiate(method.interpolate(x1Values, yValues), x1Values);

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double expVal = expDiffX1.get(i, j);
			double expVal = expDiffX1.get(i, j);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(expVal) == 0.0 ? 1.0 : Math.abs(expVal);
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resDiffX1.get(i, j), expVal, @ref * EPS);
		  }
		}

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double expVal = expDiffX0.get(j, i);
			double expVal = expDiffX0.get(j, i);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(expVal) == 0.0 ? 1.0 : Math.abs(expVal);
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resDiffX0.get(i, j), expVal, @ref * EPS);
		  }
		}

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double expVal = yValues[i][j];
			double expVal = yValues[i][j];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(expVal) == 0.0 ? 1.0 : Math.abs(expVal);
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resValues.get(i, j), expVal, @ref * EPS);
		  }
		}

	  }

	  /// 
	  public virtual void crossDerivativeTest()
	  {
		double[] x0Values = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] x1Values = new double[] {-1.0, 0.0, 1.0, 2.0, 3.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n0Data = x0Values.length;
		int n0Data = x0Values.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n1Data = x1Values.length;
		int n1Data = x1Values.Length;
		double[][] yValues = new double[][]
		{
			new double[] {1.0, -1.0, 0.0, 1.0, 0.0},
			new double[] {1.0, -1.0, 0.0, 1.0, -2.0},
			new double[] {1.0, -2.0, 0.0, -2.0, -2.0},
			new double[] {-1.0, -1.0, -2.0, -2.0, -1.0}
		};

		NaturalSplineInterpolator method = new NaturalSplineInterpolator();
		PiecewisePolynomialInterpolator2D interp = new BicubicSplineInterpolator(method);
		PiecewisePolynomialResult2D result = interp.interpolate(x0Values, x1Values, yValues);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n0IntExp = n0Data - 1;
		int n0IntExp = n0Data - 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n1IntExp = n1Data - 1;
		int n1IntExp = n1Data - 1;
		const int orderExp = 4;

		const int n0Keys = 51;
		const int n1Keys = 61;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 5.0 * i / (n0Keys - 1);
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = -2.0 + 6.0 * i / (n1Keys - 1);
		}

		assertEquals(result.NumberOfIntervals[0], n0IntExp);
		assertEquals(result.NumberOfIntervals[1], n1IntExp);
		assertEquals(result.Order[0], orderExp);
		assertEquals(result.Order[1], orderExp);

		for (int i = 0; i < n0Data; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(x0Values[i]) == 0.0 ? 1.0 : Math.abs(x0Values[i]);
		  double @ref = Math.Abs(x0Values[i]) == 0.0 ? 1.0 : Math.Abs(x0Values[i]);
		  assertEquals(result.Knots0.get(i), x0Values[i], @ref * EPS);
		  assertEquals(result.Knots2D[0].get(i), x0Values[i], @ref * EPS);
		}
		for (int i = 0; i < n1Data; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = Math.abs(x1Values[i]) == 0.0 ? 1.0 : Math.abs(x1Values[i]);
		  double @ref = Math.Abs(x1Values[i]) == 0.0 ? 1.0 : Math.Abs(x1Values[i]);
		  assertEquals(result.Knots1.get(i), x1Values[i], @ref * EPS);
		  assertEquals(result.Knots2D[1].get(i), x1Values[i], @ref * EPS);
		}

		for (int i = 0; i < n0Data - 1; ++i)
		{
		  for (int j = 0; j < n1Data - 1; ++j)
		  {
			double @ref = Math.Abs(yValues[i][j]) == 0.0 ? 1.0 : Math.Abs(yValues[i][j]);
			assertEquals(result.Coefs[i][j].get(orderExp - 1, orderExp - 1), yValues[i][j], @ref * EPS);
		  }
		}

		DoubleMatrix resValues = interp.interpolate(x0Values, x1Values, yValues, x0Values, x1Values);
		PiecewisePolynomialFunction2D func2D = new PiecewisePolynomialFunction2D();
		DoubleMatrix resDiffX0 = func2D.differentiateX0(result, x0Values, x1Values);
		DoubleMatrix resDiffX1 = func2D.differentiateX1(result, x0Values, x1Values);

		PiecewisePolynomialFunction1D func1D = new PiecewisePolynomialFunction1D();
		DoubleMatrix expDiffX0 = func1D.differentiate(method.interpolate(x0Values, OG_ALGEBRA.getTranspose(DoubleMatrix.copyOf(yValues)).toArray()), x0Values);
		DoubleMatrix expDiffX1 = func1D.differentiate(method.interpolate(x1Values, yValues), x1Values);

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
			double expVal = expDiffX1.get(i, j);
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resDiffX1.get(i, j), expVal, @ref * EPS);
		  }
		}

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
			double expVal = expDiffX0.get(j, i);
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resDiffX0.get(i, j), expVal, @ref * EPS);
		  }
		}

		for (int i = 0; i < n0Data; ++i)
		{
		  for (int j = 0; j < n1Data; ++j)
		  {
			double expVal = yValues[i][j];
			double @ref = Math.Abs(expVal) == 0.0 ? 1.0 : Math.Abs(expVal);
			assertEquals(resValues.get(i, j), expVal, @ref * EPS);
		  }
		}

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullx0Test()
	  public virtual void nullx0Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};
		x0Values = null;

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullx1Test()
	  public virtual void nullx1Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};
		x1Values = null;

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullyTest()
	  public virtual void nullyTest()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};
		yValues = null;

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void wrongLengthx0Test()
	  public virtual void wrongLengthx0Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void wrongLengthx1Test()
	  public virtual void wrongLengthx1Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void shortx0Test()
	  public virtual void shortx0Test()
	  {
		double[] x0Values = new double[] {1.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void shortx1Test()
	  public virtual void shortx1Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0},
			new double[] {-1.0},
			new double[] {2.0},
			new double[] {5.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infX0Test()
	  public virtual void infX0Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, INF};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanX0Test()
	  public virtual void nanX0Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, Double.NaN};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infX1Test()
	  public virtual void infX1Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, INF};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanX1Test()
	  public virtual void nanX1Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, Double.NaN};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infYTest()
	  public virtual void infYTest()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, INF},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanYTest()
	  public virtual void nanYTest()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, Double.NaN}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void coincideX0Test()
	  public virtual void coincideX0Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 1.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void coincideX1Test()
	  public virtual void coincideX1Test()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, 1.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void notTwoMethodsTest()
	  public virtual void notTwoMethodsTest()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.0, 2.0, 4.0},
			new double[] {-1.0, 2.0, -4.0},
			new double[] {2.0, 3.0, 4.0},
			new double[] {5.0, 2.0, 1.0}
		};

		BicubicSplineInterpolator interp = new BicubicSplineInterpolator(new PiecewisePolynomialInterpolator[] {new CubicSplineInterpolator()});
		interp.interpolate(x0Values, x1Values, yValues);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void notKnotRevoveredTests()
	  public virtual void notKnotRevoveredTests()
	  {
		double[] x0Values = new double[] {0.0, 1.0, 2.0, 3.0};
		double[] x1Values = new double[] {0.0, 1.0, 2.0};
		double[][] yValues = new double[][]
		{
			new double[] {1.e-20, 3.e-120, 5.e120},
			new double[] {2.e-20, 3.e-120, 4.e-120},
			new double[] {1.e-20, 1.e-120, 1.e-20},
			new double[] {4.e-120, 3.e-20, 2.e-20}
		};

		BicubicSplineInterpolator intp = new BicubicSplineInterpolator(new CubicSplineInterpolator());
		intp.interpolate(x0Values, x1Values, yValues);
	  }

	}

}