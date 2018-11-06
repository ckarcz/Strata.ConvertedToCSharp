using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using PiecewisePolynomialResult2D = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResult2D;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PiecewisePolynomialFunction2DTest
	public class PiecewisePolynomialFunction2DTest
	{

	  private const double EPS = 1e-14;
	  private const double INF = 1.0 / 0.0;

	  private static readonly DoubleArray knots0 = DoubleArray.of(1.0, 2.0, 3.0, 4.0);
	  private static readonly DoubleArray knots1 = DoubleArray.of(2.0, 3.0, 4.0);

	  private const int nKnots0 = 4;
	  private const int nKnots1 = 3;
	  private static DoubleMatrix[][] coefs;
	  static PiecewisePolynomialFunction2DTest()
	  {
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: coefs = new DoubleMatrix[nKnots0 - 1][nKnots1 - 1];
		coefs = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0 - 1, nKnots1 - 1);
		coefs[0][0] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1d, 0d, 0d, 0d},
			new double[] {0d, 0d, 0d, 0d},
			new double[] {0d, 0d, 0d, 0d},
			new double[] {0d, 0d, 0d, 0d},
			new double[] {0d, 0d, 0d, 0d}
		});
		coefs[1][0] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1d, 0d, 0d, 0d},
			new double[] {4d, 0d, 0d, 0d},
			new double[] {6d, 0d, 0d, 0d},
			new double[] {4d, 0d, 0d, 0d},
			new double[] {1d, 0d, 0d, 0d}
		});
		coefs[2][0] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1d, 0d, 0d, 0d},
			new double[] {8d, 0d, 0d, 0d},
			new double[] {24d, 0d, 0d, 0d},
			new double[] {32d, 0d, 0d, 0d},
			new double[] {16d, 0d, 0d, 0d}
		});
		coefs[0][1] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1d, 3d, 3d, 1d},
			new double[] {0d, 0d, 0d, 0d},
			new double[] {0d, 0d, 0d, 0d},
			new double[] {0d, 0d, 0d, 0d},
			new double[] {0d, 0d, 0d, 0d}
		});
		coefs[1][1] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1d, 3d, 3d, 1d},
			new double[] {4.0 * 1d, 4.0 * 3d, 4.0 * 3d, 4.0 * 1d},
			new double[] {6.0 * 1d, 6.0 * 3d, 6.0 * 3d, 6.0 * 1d},
			new double[] {4.0 * 1d, 4.0 * 3d, 4.0 * 3d, 4.0 * 1d},
			new double[] {1d, 3d, 3d, 1d}
		});
		coefs[2][1] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1d, 3d, 3d, 1d},
			new double[] {8.0 * 1d, 8.0 * 3d, 8.0 * 3d, 8.0 * 1d},
			new double[] {24.0 * 1d, 24.0 * 3d, 24.0 * 3d, 24.0 * 1d},
			new double[] {32.0 * 1d, 32.0 * 3d, 32.0 * 3d, 32.0 * 1d},
			new double[] {16.0 * 1d, 16.0 * 3d, 16.0 * 3d, 16.0 * 1d}
		});
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: coefsConst = new DoubleMatrix[nKnots0 - 1][nKnots1 - 1];
		coefsConst = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0 - 1, nKnots1 - 1);
		coefsConst[0][0] = DoubleMatrix.of(1, 1, 4d);
		coefsConst[1][0] = DoubleMatrix.of(1, 1, 4d);
		coefsConst[2][0] = DoubleMatrix.of(1, 1, 4d);
		coefsConst[0][1] = DoubleMatrix.of(1, 1, 4d);
		coefsConst[1][1] = DoubleMatrix.of(1, 1, 4d);
		coefsConst[2][1] = DoubleMatrix.of(1, 1, 4d);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: coefsLin = new DoubleMatrix[nKnots0 - 1][nKnots1 - 1];
		coefsLin = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0 - 1, nKnots1 - 1);
		coefsLin[0][0] = DoubleMatrix.of(2, 2, 1d, 2d, 2d, 4d);
		coefsLin[1][0] = DoubleMatrix.of(2, 2, 1d, 2d, 2d, 4d);
		coefsLin[2][0] = DoubleMatrix.of(2, 2, 1d, 2d, 2d, 4d);
		coefsLin[0][1] = DoubleMatrix.of(2, 2, 1d, 3d, 3d, 9d);
		coefsLin[1][1] = DoubleMatrix.of(2, 2, 1d, 3d, 3d, 9d);
		coefsLin[2][1] = DoubleMatrix.of(2, 2, 1d, 3d, 3d, 9d);
	  }

	  private static DoubleMatrix[][] coefsConst;

	  private static DoubleMatrix[][] coefsLin;

	  /// <summary>
	  /// Sample function is f(x,y) = (x-1)^4 * (y-2)^3
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void sampleFunctionTest()
	  public virtual void sampleFunctionTest()
	  {

		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}

		/*
		 * "Evaluate" test
		 */
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] valuesExp = new double[n0Keys][n1Keys];
		double[][] valuesExp = RectangularArrays.ReturnRectangularDoubleArray(n0Keys, n1Keys);
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
			valuesExp[i][j] = Math.Pow(x0Keys[i] - 1.0, 4.0) * Math.Pow(x1Keys[j] - 2.0, 3.0);
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] values = function.evaluate(result, x0Keys, x1Keys).toArray();
		double[][] values = function.evaluate(result, x0Keys, x1Keys).toArray();
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesExp[i][j] == 0.0 ? 1.0 : Math.abs(valuesExp[i][j]);
			double @ref = valuesExp[i][j] == 0.0 ? 1.0 : Math.Abs(valuesExp[i][j]);
			assertEquals(values[i][j], valuesExp[i][j], @ref * EPS);
		  }
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.evaluate(result, x0Keys[1], x1Keys[1]);
		  double value = function.evaluate(result, x0Keys[1], x1Keys[1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesExp[1][1] == 0.0 ? 1.0 : Math.abs(valuesExp[1][1]);
		  double @ref = valuesExp[1][1] == 0.0 ? 1.0 : Math.Abs(valuesExp[1][1]);
		  assertEquals(value, valuesExp[1][1], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.evaluate(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
		  double value = function.evaluate(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesExp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.abs(valuesExp[n0Keys - 2][n1Keys - 2]);
		  double @ref = valuesExp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.Abs(valuesExp[n0Keys - 2][n1Keys - 2]);
		  assertEquals(value, valuesExp[n0Keys - 2][n1Keys - 2], @ref * EPS);
		}

		/*
		 * First derivative test
		 */
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] valuesDiffX0Exp = new double[n0Keys][n1Keys];
		double[][] valuesDiffX0Exp = RectangularArrays.ReturnRectangularDoubleArray(n0Keys, n1Keys);
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
			valuesDiffX0Exp[i][j] = 4.0 * Math.Pow(x0Keys[i] - 1.0, 3.0) * Math.Pow(x1Keys[j] - 2.0, 3.0);
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] valuesDiffX0 = function.differentiateX0(result, x0Keys, x1Keys).toArray();
		double[][] valuesDiffX0 = function.differentiateX0(result, x0Keys, x1Keys).toArray();
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffX0Exp[i][j] == 0.0 ? 1.0 : Math.abs(valuesDiffX0Exp[i][j]);
			double @ref = valuesDiffX0Exp[i][j] == 0.0 ? 1.0 : Math.Abs(valuesDiffX0Exp[i][j]);
			assertEquals(valuesDiffX0[i][j], valuesDiffX0Exp[i][j], @ref * EPS);
		  }
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.differentiateX0(result, x0Keys[1], x1Keys[1]);
		  double value = function.differentiateX0(result, x0Keys[1], x1Keys[1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffX0Exp[1][1] == 0.0 ? 1.0 : Math.abs(valuesDiffX0Exp[1][1]);
		  double @ref = valuesDiffX0Exp[1][1] == 0.0 ? 1.0 : Math.Abs(valuesDiffX0Exp[1][1]);
		  assertEquals(value, valuesDiffX0Exp[1][1], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.differentiateX0(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
		  double value = function.differentiateX0(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffX0Exp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.abs(valuesDiffX0Exp[n0Keys - 2][n1Keys - 2]);
		  double @ref = valuesDiffX0Exp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.Abs(valuesDiffX0Exp[n0Keys - 2][n1Keys - 2]);
		  assertEquals(value, valuesDiffX0Exp[n0Keys - 2][n1Keys - 2], @ref * EPS);
		}

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] valuesDiffX1Exp = new double[n0Keys][n1Keys];
		double[][] valuesDiffX1Exp = RectangularArrays.ReturnRectangularDoubleArray(n0Keys, n1Keys);
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
			valuesDiffX1Exp[i][j] = Math.Pow(x0Keys[i] - 1.0, 4.0) * Math.Pow(x1Keys[j] - 2.0, 2.0) * 3.0;
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] valuesDiffX1 = function.differentiateX1(result, x0Keys, x1Keys).toArray();
		double[][] valuesDiffX1 = function.differentiateX1(result, x0Keys, x1Keys).toArray();
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffX1Exp[i][j] == 0.0 ? 1.0 : Math.abs(valuesDiffX1Exp[i][j]);
			double @ref = valuesDiffX1Exp[i][j] == 0.0 ? 1.0 : Math.Abs(valuesDiffX1Exp[i][j]);
			assertEquals(valuesDiffX1[i][j], valuesDiffX1Exp[i][j], @ref * EPS);
		  }
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.differentiateX1(result, x0Keys[1], x1Keys[1]);
		  double value = function.differentiateX1(result, x0Keys[1], x1Keys[1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffX1Exp[1][1] == 0.0 ? 1.0 : Math.abs(valuesDiffX1Exp[1][1]);
		  double @ref = valuesDiffX1Exp[1][1] == 0.0 ? 1.0 : Math.Abs(valuesDiffX1Exp[1][1]);
		  assertEquals(value, valuesDiffX1Exp[1][1], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.differentiateX1(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
		  double value = function.differentiateX1(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffX1Exp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.abs(valuesDiffX1Exp[n0Keys - 2][n1Keys - 2]);
		  double @ref = valuesDiffX1Exp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.Abs(valuesDiffX1Exp[n0Keys - 2][n1Keys - 2]);
		  assertEquals(value, valuesDiffX1Exp[n0Keys - 2][n1Keys - 2], @ref * EPS);
		}

		/*
		 * Second derivative test
		 */
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] valuesDiffCrossExp = new double[n0Keys][n1Keys];
		double[][] valuesDiffCrossExp = RectangularArrays.ReturnRectangularDoubleArray(n0Keys, n1Keys);
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
			valuesDiffCrossExp[i][j] = 4.0 * Math.Pow(x0Keys[i] - 1.0, 3.0) * 3.0 * Math.Pow(x1Keys[j] - 2.0, 2.0);
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] valuesDiffCross = function.differentiateCross(result, x0Keys, x1Keys).toArray();
		double[][] valuesDiffCross = function.differentiateCross(result, x0Keys, x1Keys).toArray();
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffCrossExp[i][j] == 0.0 ? 1.0 : Math.abs(valuesDiffCrossExp[i][j]);
			double @ref = valuesDiffCrossExp[i][j] == 0.0 ? 1.0 : Math.Abs(valuesDiffCrossExp[i][j]);
			assertEquals(valuesDiffCross[i][j], valuesDiffCrossExp[i][j], @ref * EPS);
		  }
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.differentiateCross(result, x0Keys[1], x1Keys[1]);
		  double value = function.differentiateCross(result, x0Keys[1], x1Keys[1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffCrossExp[1][1] == 0.0 ? 1.0 : Math.abs(valuesDiffCrossExp[1][1]);
		  double @ref = valuesDiffCrossExp[1][1] == 0.0 ? 1.0 : Math.Abs(valuesDiffCrossExp[1][1]);
		  assertEquals(value, valuesDiffCrossExp[1][1], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.differentiateCross(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
		  double value = function.differentiateCross(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffCrossExp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.abs(valuesDiffCrossExp[n0Keys - 2][n1Keys - 2]);
		  double @ref = valuesDiffCrossExp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.Abs(valuesDiffCrossExp[n0Keys - 2][n1Keys - 2]);
		  assertEquals(value, valuesDiffCrossExp[n0Keys - 2][n1Keys - 2], @ref * EPS);
		}

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] valuesDiffTwiceX0Exp = new double[n0Keys][n1Keys];
		double[][] valuesDiffTwiceX0Exp = RectangularArrays.ReturnRectangularDoubleArray(n0Keys, n1Keys);
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
			valuesDiffTwiceX0Exp[i][j] = 4.0 * 3.0 * Math.Pow(x0Keys[i] - 1.0, 2.0) * Math.Pow(x1Keys[j] - 2.0, 3.0);
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] valuesDiffTwiceX0 = function.differentiateTwiceX0(result, x0Keys, x1Keys).toArray();
		double[][] valuesDiffTwiceX0 = function.differentiateTwiceX0(result, x0Keys, x1Keys).toArray();
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffTwiceX0Exp[i][j] == 0.0 ? 1.0 : Math.abs(valuesDiffTwiceX0Exp[i][j]);
			double @ref = valuesDiffTwiceX0Exp[i][j] == 0.0 ? 1.0 : Math.Abs(valuesDiffTwiceX0Exp[i][j]);
			assertEquals(valuesDiffTwiceX0[i][j], valuesDiffTwiceX0Exp[i][j], @ref * EPS);
		  }
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.differentiateTwiceX0(result, x0Keys[1], x1Keys[1]);
		  double value = function.differentiateTwiceX0(result, x0Keys[1], x1Keys[1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffTwiceX0Exp[1][1] == 0.0 ? 1.0 : Math.abs(valuesDiffTwiceX0Exp[1][1]);
		  double @ref = valuesDiffTwiceX0Exp[1][1] == 0.0 ? 1.0 : Math.Abs(valuesDiffTwiceX0Exp[1][1]);
		  assertEquals(value, valuesDiffTwiceX0Exp[1][1], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.differentiateTwiceX0(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
		  double value = function.differentiateTwiceX0(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffTwiceX0Exp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.abs(valuesDiffTwiceX0Exp[n0Keys - 2][n1Keys - 2]);
		  double @ref = valuesDiffTwiceX0Exp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.Abs(valuesDiffTwiceX0Exp[n0Keys - 2][n1Keys - 2]);
		  assertEquals(value, valuesDiffTwiceX0Exp[n0Keys - 2][n1Keys - 2], @ref * EPS);
		}

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] valuesDiffTwiceX1Exp = new double[n0Keys][n1Keys];
		double[][] valuesDiffTwiceX1Exp = RectangularArrays.ReturnRectangularDoubleArray(n0Keys, n1Keys);
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
			valuesDiffTwiceX1Exp[i][j] = Math.Pow(x0Keys[i] - 1.0, 4.0) * Math.Pow(x1Keys[j] - 2.0, 1.0) * 3.0 * 2.0;
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] valuesDiffTwiceX1 = function.differentiateTwiceX1(result, x0Keys, x1Keys).toArray();
		double[][] valuesDiffTwiceX1 = function.differentiateTwiceX1(result, x0Keys, x1Keys).toArray();
		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffTwiceX1Exp[i][j] == 0.0 ? 1.0 : Math.abs(valuesDiffTwiceX1Exp[i][j]);
			double @ref = valuesDiffTwiceX1Exp[i][j] == 0.0 ? 1.0 : Math.Abs(valuesDiffTwiceX1Exp[i][j]);
			assertEquals(valuesDiffTwiceX1[i][j], valuesDiffTwiceX1Exp[i][j], @ref * EPS);
		  }
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.differentiateTwiceX1(result, x0Keys[1], x1Keys[1]);
		  double value = function.differentiateTwiceX1(result, x0Keys[1], x1Keys[1]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffTwiceX1Exp[1][1] == 0.0 ? 1.0 : Math.abs(valuesDiffTwiceX1Exp[1][1]);
		  double @ref = valuesDiffTwiceX1Exp[1][1] == 0.0 ? 1.0 : Math.Abs(valuesDiffTwiceX1Exp[1][1]);
		  assertEquals(value, valuesDiffTwiceX1Exp[1][1], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double value = function.differentiateTwiceX1(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
		  double value = function.differentiateTwiceX1(result, x0Keys[n0Keys - 2], x1Keys[n1Keys - 2]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesDiffTwiceX1Exp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.abs(valuesDiffTwiceX1Exp[n0Keys - 2][n1Keys - 2]);
		  double @ref = valuesDiffTwiceX1Exp[n0Keys - 2][n1Keys - 2] == 0.0 ? 1.0 : Math.Abs(valuesDiffTwiceX1Exp[n0Keys - 2][n1Keys - 2]);
		  assertEquals(value, valuesDiffTwiceX1Exp[n0Keys - 2][n1Keys - 2], @ref * EPS);
		}
	  }

	  /*
	   * PiecewisePolynomialResult2D is null
	   */
	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpEvaluateTest()
	  public virtual void nullPpEvaluateTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.evaluate(result, x0Keys[1], x1Keys[1]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpEvaluateMultiTest()
	  public virtual void nullPpEvaluateMultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.evaluate(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpDiffX0Test()
	  public virtual void nullPpDiffX0Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.differentiateX0(result, x0Keys[1], x1Keys[1]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpDiffX0MultiTest()
	  public virtual void nullPpDiffX0MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.differentiateX0(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpDiffX1Test()
	  public virtual void nullPpDiffX1Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.differentiateX1(result, x0Keys[1], x1Keys[1]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpDiffX1MultiTest()
	  public virtual void nullPpDiffX1MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.differentiateX1(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpDiffCrossTest()
	  public virtual void nullPpDiffCrossTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.differentiateCross(result, x0Keys[1], x1Keys[1]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpDiffCrossMultiTest()
	  public virtual void nullPpDiffCrossMultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.differentiateCross(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpDiffTwiceX0Test()
	  public virtual void nullPpDiffTwiceX0Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.differentiateTwiceX0(result, x0Keys[1], x1Keys[1]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpDiffTwiceX0MultiTest()
	  public virtual void nullPpDiffTwiceX0MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.differentiateTwiceX0(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpDiffTwiceX1Test()
	  public virtual void nullPpDiffTwiceX1Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.differentiateTwiceX1(result, x0Keys[1], x1Keys[1]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullPpDiffTwiceX1MultiTest()
	  public virtual void nullPpDiffTwiceX1MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		result = null;
		function.differentiateTwiceX1(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infX0Test()
	  public virtual void infX0Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		x0Keys[2] = INF;
		function.evaluate(result, x0Keys[2], x1Keys[2]);
	  }

	  /*
	   * Input contains NaN or infinity
	   */
	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanX0Test()
	  public virtual void nanX0Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		x0Keys[3] = Double.NaN;
		function.evaluate(result, x0Keys[3], x1Keys[3]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infX1Test()
	  public virtual void infX1Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		x1Keys[2] = INF;
		function.evaluate(result, x0Keys[2], x1Keys[2]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanX1Test()
	  public virtual void nanX1Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		x1Keys[3] = Double.NaN;
		function.evaluate(result, x0Keys[3], x1Keys[3]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infX0MultiTest()
	  public virtual void infX0MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		x0Keys[2] = INF;
		function.evaluate(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanX0MultiTest()
	  public virtual void nanX0MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		x0Keys[3] = Double.NaN;
		function.evaluate(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infX1MultiTest()
	  public virtual void infX1MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		x1Keys[2] = INF;
		function.evaluate(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nanX1MultiTest()
	  public virtual void nanX1MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefs, new int[] {5, 4});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		x1Keys[3] = Double.NaN;
		function.evaluate(result, x0Keys, x1Keys);
	  }

	  /*
	   * Polynomial degree is too low
	   */
	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void constDiffX0Test()
	  public virtual void constDiffX0Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsConst, new int[] {1, 1});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateX0(result, x0Keys[0], x1Keys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void constDiffX0MultiTest()
	  public virtual void constDiffX0MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsConst, new int[] {1, 1});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateX0(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void constDiffX1Test()
	  public virtual void constDiffX1Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsConst, new int[] {1, 1});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateX1(result, x0Keys[0], x1Keys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void constDiffX1MultiTest()
	  public virtual void constDiffX1MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsConst, new int[] {1, 1});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateX1(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void linearDiffTwiceX0Test()
	  public virtual void linearDiffTwiceX0Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsLin, new int[] {2, 2});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateTwiceX0(result, x0Keys[0], x1Keys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void linearDiffTwiceX0MultiTest()
	  public virtual void linearDiffTwiceX0MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsLin, new int[] {2, 2});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateTwiceX0(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void linearDiffTwiceX1Test()
	  public virtual void linearDiffTwiceX1Test()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsLin, new int[] {2, 2});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateTwiceX1(result, x0Keys[0], x1Keys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void linearDiffTwiceX1MultiTest()
	  public virtual void linearDiffTwiceX1MultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsLin, new int[] {2, 2});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateTwiceX1(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void constDiffCrossTest()
	  public virtual void constDiffCrossTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsConst, new int[] {1, 1});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateCross(result, x0Keys[0], x1Keys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void linConstDiffCrossTest()
	  public virtual void linConstDiffCrossTest()
	  {

		DoubleMatrix[][] coefsLinConst;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: coefsLinConst = new DoubleMatrix[nKnots0 - 1][nKnots1 - 1];
		coefsLinConst = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0 - 1, nKnots1 - 1);
		coefsLinConst[0][0] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {2.0 * 2.0}
		});
		coefsLinConst[1][0] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {2.0 * 2.0}
		});
		coefsLinConst[2][0] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {2.0 * 2.0}
		});
		coefsLinConst[0][1] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {3.0 * 2.0}
		});
		coefsLinConst[1][1] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {3.0 * 2.0}
		});
		coefsLinConst[2][1] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {3.0 * 2.0}
		});

		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsLinConst, new int[] {2, 1});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateCross(result, x0Keys[0], x1Keys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void constDiffCrossMultiTest()
	  public virtual void constDiffCrossMultiTest()
	  {
		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsConst, new int[] {1, 1});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateCross(result, x0Keys, x1Keys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void linConstDiffCrossMultiTest()
	  public virtual void linConstDiffCrossMultiTest()
	  {

		DoubleMatrix[][] coefsLinConst;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: coefsLinConst = new DoubleMatrix[nKnots0 - 1][nKnots1 - 1];
		coefsLinConst = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0 - 1, nKnots1 - 1);
		coefsLinConst[0][0] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {2.0 * 2.0}
		});
		coefsLinConst[1][0] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {2.0 * 2.0}
		});
		coefsLinConst[2][0] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {2.0 * 2.0}
		});
		coefsLinConst[0][1] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {3.0 * 2.0}
		});
		coefsLinConst[1][1] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {3.0 * 2.0}
		});
		coefsLinConst[2][1] = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {2.0},
			new double[] {3.0 * 2.0}
		});

		PiecewisePolynomialResult2D result = new PiecewisePolynomialResult2D(knots0, knots1, coefsLinConst, new int[] {2, 1});
		PiecewisePolynomialFunction2D function = new PiecewisePolynomialFunction2D();

		const int n0Keys = 21;
		const int n1Keys = 31;
		double[] x0Keys = new double[n0Keys];
		double[] x1Keys = new double[n1Keys];
		for (int i = 0; i < n0Keys; ++i)
		{
		  x0Keys[i] = 0.0 + 4.0 / (n0Keys - 1) * i;
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  x1Keys[i] = 1.0 + 3.0 / (n1Keys - 1) * i;
		}
		function.differentiateCross(result, x0Keys, x1Keys);
	  }

	}

}