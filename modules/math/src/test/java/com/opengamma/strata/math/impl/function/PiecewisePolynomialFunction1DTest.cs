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

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using PiecewisePolynomialResult = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResult;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PiecewisePolynomialFunction1DTest
	public class PiecewisePolynomialFunction1DTest
	{

	  private const double EPS = 1e-14;
	  private const double INF = 1.0 / 0.0;
	  private static readonly DoubleArray X_VALUES = DoubleArray.of(1, 2, 3, 4);

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void evaluateAllTest()
	  public virtual void evaluateAllTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1.0, -3.0, 3.0, -1 }, {0.0, 5.0, -20.0, 20 }, {1.0, 0.0, 0.0, 0.0 }, {0.0, 5.0, -10.0, 5 }, {1.0, 3.0, 3.0, 1.0 }, {0.0, 5.0, 0.0, 0.0 } });
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKeys = new double[][] { {-2, 1, 2, 2.5 }, {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0 } };
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][][] valuesExp = new double[][][] { { {-64.0, -1.0, 0.0, 1.0 / 8.0 }, {-1.0 / 8.0, 1.0 / 27.0, 3375.0 / 7.0 / 7.0 / 7.0, 27.0 } }, { {125.0, 20.0, 5.0, 5.0 / 4.0 }, {45.0 / 4.0, 20.0 / 9.0, 2240.0 / 7.0 / 7.0 / 7.0, 20.0 } } };
		double[][][] valuesExp = new double[][][]
		{
			new double[][]
			{
				new double[] {-64.0, -1.0, 0.0, 1.0 / 8.0},
				new double[] {-1.0 / 8.0, 1.0 / 27.0, 3375.0 / 7.0 / 7.0 / 7.0, 27.0}
			},
			new double[][]
			{
				new double[] {125.0, 20.0, 5.0, 5.0 / 4.0},
				new double[] {45.0 / 4.0, 20.0 / 9.0, 2240.0 / 7.0 / 7.0 / 7.0, 20.0}
			}
		};
		const int dim = 2;
		const int nCoefs = 4;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int keyLength = xKeys[0].length;
		int keyLength = xKeys[0].Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int keyDim = xKeys.length;
		int keyDim = xKeys.Length;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] valuesResMat = function.evaluate(pp, xKeys);
		DoubleMatrix[] valuesResMat = function.evaluate(pp, xKeys);
		for (int i = 0; i < dim; ++i)
		{
		  for (int k = 0; k < keyDim; ++k)
		  {
			for (int j = 0; j < keyLength; ++j)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesExp[i][k][j] == 0.0 ? 1.0 : Math.abs(valuesExp[i][k][j]);
			  double @ref = valuesExp[i][k][j] == 0.0 ? 1.0 : Math.Abs(valuesExp[i][k][j]);
			  assertEquals(valuesResMat[i].get(k, j), valuesExp[i][k][j], @ref * EPS);
			}
		  }
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix valuesRes = function.evaluate(pp, xKeys[0]);
		DoubleMatrix valuesRes = function.evaluate(pp, xKeys[0]);
		for (int i = 0; i < dim; ++i)
		{
		  for (int j = 0; j < keyLength; ++j)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesExp[i][0][j] == 0.0 ? 1.0 : Math.abs(valuesExp[i][0][j]);
			double @ref = valuesExp[i][0][j] == 0.0 ? 1.0 : Math.Abs(valuesExp[i][0][j]);
			assertEquals(valuesRes.get(i, j), valuesExp[i][0][j], @ref * EPS);
		  }
		}

		DoubleArray valuesResVec = function.evaluate(pp, xKeys[0][0]);
		for (int i = 0; i < dim; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesExp[i][0][0] == 0.0 ? 1.0 : Math.abs(valuesExp[i][0][0]);
		  double @ref = valuesExp[i][0][0] == 0.0 ? 1.0 : Math.Abs(valuesExp[i][0][0]);
		  assertEquals(valuesResVec.get(i), valuesExp[i][0][0], @ref * EPS);
		}

		valuesResVec = function.evaluate(pp, xKeys[0][3]);
		for (int i = 0; i < dim; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesExp[i][0][3] == 0.0 ? 1.0 : Math.abs(valuesExp[i][0][3]);
		  double @ref = valuesExp[i][0][3] == 0.0 ? 1.0 : Math.Abs(valuesExp[i][0][3]);
		  assertEquals(valuesResVec.get(i), valuesExp[i][0][3], @ref * EPS);
		}

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void linearAllTest()
	  public virtual void linearAllTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray knots = com.opengamma.strata.collect.array.DoubleArray.of(1d, 4d);
		DoubleArray knots = DoubleArray.of(1d, 4d);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] {{0.0, 1.0, 1.0 } });
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.0, 1.0, 1.0}
		});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xKeys = new double[] {-2, 1.0, 2.5, 4.0 };
		double[] xKeys = new double[] {-2, 1.0, 2.5, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] initials = new double[] {-0.5, 1.0, 2.5, 5.0 };
		double[] initials = new double[] {-0.5, 1.0, 2.5, 5.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nKeys = xKeys.length;
		int nKeys = xKeys.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nInit = initials.length;
		int nInit = initials.Length;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] valuesExp = new double[] {-2, 1, 2.5, 4.0 };
		double[] valuesExp = new double[] {-2, 1, 2.5, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] integrateExp = new double[nInit][nKeys];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] integrateExp = new double[nInit][nKeys];
		double[][] integrateExp = RectangularArrays.ReturnRectangularDoubleArray(nInit, nKeys);
		for (int i = 0; i < nInit; ++i)
		{
		  for (int j = 0; j < nKeys; ++j)
		  {
			integrateExp[i][j] = 0.5 * (xKeys[j] * xKeys[j] - initials[i] * initials[i]);
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] differentiateExp = new double[] {1.0, 1.0, 1.0, 1.0 };
		double[] differentiateExp = new double[] {1.0, 1.0, 1.0, 1.0};

		PiecewisePolynomialResult result = new PiecewisePolynomialResult(knots, coefsMatrix, 3, 1);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray values = function.evaluate(result, xKeys).row(0);
		DoubleArray values = function.evaluate(result, xKeys).row(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray differentiate = function.differentiate(result, xKeys).row(0);
		DoubleArray differentiate = function.differentiate(result, xKeys).row(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] integrate = new double[nInit][nKeys];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] integrate = new double[nInit][nKeys];
		double[][] integrate = RectangularArrays.ReturnRectangularDoubleArray(nInit, nKeys);
		for (int i = 0; i < nInit; ++i)
		{
		  for (int j = 0; j < nKeys; ++j)
		  {
			integrate[i][j] = function.integrate(result, initials[i], xKeys).get(j);
		  }
		}

		for (int i = 0; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesExp[i] == 0.0 ? 1.0 : Math.abs(valuesExp[i]);
		  double @ref = valuesExp[i] == 0.0 ? 1.0 : Math.Abs(valuesExp[i]);
		  assertEquals(values.get(i), valuesExp[i], @ref * EPS);
		}

		for (int i = 0; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = differentiateExp[i] == 0.0 ? 1.0 : Math.abs(differentiateExp[i]);
		  double @ref = differentiateExp[i] == 0.0 ? 1.0 : Math.Abs(differentiateExp[i]);
		  assertEquals(differentiate.get(i), differentiateExp[i], @ref * EPS);
		}

		for (int j = 0; j < nInit; ++j)
		{
		  for (int i = 0; i < nKeys; ++i)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = integrateExp[j][i] == 0.0 ? 1.0 : Math.abs(integrateExp[j][i]);
			double @ref = integrateExp[j][i] == 0.0 ? 1.0 : Math.Abs(integrateExp[j][i]);
			assertEquals(integrate[j][i], integrateExp[j][i], @ref * EPS);
		  }
		}
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void quadraticAllTest()
	  public virtual void quadraticAllTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray knots = com.opengamma.strata.collect.array.DoubleArray.of(1d, 3d);
		DoubleArray knots = DoubleArray.of(1d, 3d);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] {{-1.0, 2.0, 1.0 } });
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {-1.0, 2.0, 1.0}
		});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xKeys = new double[] {-2, 1, 2.5, 4.0 };
		double[] xKeys = new double[] {-2, 1, 2.5, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] initials = new double[] {-0.5, 1.0, 2.5, 5.0 };
		double[] initials = new double[] {-0.5, 1.0, 2.5, 5.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nKeys = xKeys.length;
		int nKeys = xKeys.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nInit = initials.length;
		int nInit = initials.Length;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] valuesExp = new double[] {-14.0, 1.0, 7.0 / 4.0, -2.0 };
		double[] valuesExp = new double[] {-14.0, 1.0, 7.0 / 4.0, -2.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] integrateExp = new double[nInit][nKeys];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] integrateExp = new double[nInit][nKeys];
		double[][] integrateExp = RectangularArrays.ReturnRectangularDoubleArray(nInit, nKeys);
		for (int i = 0; i < nInit; ++i)
		{
		  for (int j = 0; j < nKeys; ++j)
		  {
			integrateExp[i][j] = -1.0 / 3.0 * (xKeys[j] - initials[i]) * (xKeys[j] * xKeys[j] + initials[i] * initials[i] - 6.0 * xKeys[j] - 6.0 * initials[i] + 6.0 + initials[i] * xKeys[j]);
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] differentiateExp = new double[nKeys];
		double[] differentiateExp = new double[nKeys];
		for (int j = 0; j < nKeys; ++j)
		{
		  differentiateExp[j] = -2.0 * (xKeys[j] - 1) + 2.0;
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] differentiateTwiceExp = new double[nKeys];
		double[] differentiateTwiceExp = new double[nKeys];
		for (int j = 0; j < nKeys; ++j)
		{
		  differentiateTwiceExp[j] = -2.0;
		}

		PiecewisePolynomialResult result = new PiecewisePolynomialResult(knots, coefsMatrix, 3, 1);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray values = function.evaluate(result, xKeys).row(0);
		DoubleArray values = function.evaluate(result, xKeys).row(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray differentiate = function.differentiate(result, xKeys).row(0);
		DoubleArray differentiate = function.differentiate(result, xKeys).row(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray differentiateTwice = function.differentiateTwice(result, xKeys).row(0);
		DoubleArray differentiateTwice = function.differentiateTwice(result, xKeys).row(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] integrate = new double[nInit][nKeys];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] integrate = new double[nInit][nKeys];
		double[][] integrate = RectangularArrays.ReturnRectangularDoubleArray(nInit, nKeys);
		for (int i = 0; i < nInit; ++i)
		{
		  for (int j = 0; j < nKeys; ++j)
		  {
			integrate[i][j] = function.integrate(result, initials[i], xKeys).get(j);
		  }
		}

		for (int i = 0; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = valuesExp[i] == 0.0 ? 1.0 : Math.abs(valuesExp[i]);
		  double @ref = valuesExp[i] == 0.0 ? 1.0 : Math.Abs(valuesExp[i]);
		  assertEquals(values.get(i), valuesExp[i], @ref * EPS);
		}

		for (int i = 0; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = differentiateExp[i] == 0.0 ? 1.0 : Math.abs(differentiateExp[i]);
		  double @ref = differentiateExp[i] == 0.0 ? 1.0 : Math.Abs(differentiateExp[i]);
		  assertEquals(differentiate.get(i), differentiateExp[i], @ref * EPS);
		}

		for (int i = 0; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = differentiateTwiceExp[i] == 0.0 ? 1.0 : Math.abs(differentiateTwiceExp[i]);
		  double @ref = differentiateTwiceExp[i] == 0.0 ? 1.0 : Math.Abs(differentiateTwiceExp[i]);
		  assertEquals(differentiateTwice.get(i), differentiateTwiceExp[i], @ref * EPS);
		}

		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = differentiateTwiceExp[1] == 0.0 ? 1.0 : Math.abs(differentiateTwiceExp[1]);
		  double @ref = differentiateTwiceExp[1] == 0.0 ? 1.0 : Math.Abs(differentiateTwiceExp[1]);
		  assertEquals(differentiateTwice.get(1), differentiateTwiceExp[1], @ref * EPS);
		}

		for (int j = 0; j < nInit; ++j)
		{
		  for (int i = 0; i < nKeys; ++i)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = integrateExp[j][i] == 0.0 ? 1.0 : Math.abs(integrateExp[j][i]);
			double @ref = integrateExp[j][i] == 0.0 ? 1.0 : Math.Abs(integrateExp[j][i]);
			assertEquals(integrate[j][i], integrateExp[j][i], @ref * EPS);
		  }
		}

	  }

	  /// <summary>
	  /// Sample function is f(x) = (x-1)^4
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void GeneralIntegrateDifferentiateTest()
	  public virtual void GeneralIntegrateDifferentiateTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefMat = new double[][] { {1.0, 0.0, 0.0, 0.0, 0.0 }, {1.0, 4.0, 6.0, 4.0, 1.0 }, {1.0, 8.0, 24.0, 32.0, 16.0 } };
		double[][] coefMat = new double[][]
		{
			new double[] {1.0, 0.0, 0.0, 0.0, 0.0},
			new double[] {1.0, 4.0, 6.0, 4.0, 1.0},
			new double[] {1.0, 8.0, 24.0, 32.0, 16.0}
		};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xKeys = new double[] {-2, 1, 2.5, 4.0 };
		double[] xKeys = new double[] {-2, 1, 2.5, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] initials = new double[] {1.0, 2.5, 23.0 / 7.0, 7.0 };
		double[] initials = new double[] {1.0, 2.5, 23.0 / 7.0, 7.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nKeys = xKeys.length;
		int nKeys = xKeys.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nInit = initials.length;
		int nInit = initials.Length;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] integrateExp = new double[nInit][nKeys];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] integrateExp = new double[nInit][nKeys];
		double[][] integrateExp = RectangularArrays.ReturnRectangularDoubleArray(nInit, nKeys);
		for (int i = 0; i < nInit; ++i)
		{
		  for (int j = 0; j < nKeys; ++j)
		  {
			integrateExp[i][j] = Math.Pow(xKeys[j] - 1.0, 5.0) / 5.0 - Math.Pow(initials[i] - 1.0, 5.0) / 5.0;
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] differentiateExp = new double[] {-108.0, 0.0, 27.0 / 2.0, 108.0 };
		double[] differentiateExp = new double[] {-108.0, 0.0, 27.0 / 2.0, 108.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] differentiateTwiceExp = new double[nKeys];
		double[] differentiateTwiceExp = new double[nKeys];
		for (int i = 0; i < nKeys; ++i)
		{
		  differentiateTwiceExp[i] = 12.0 * (xKeys[i] - 1.0) * (xKeys[i] - 1.0);
		}

		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();
		PiecewisePolynomialResult result = new PiecewisePolynomialResult(X_VALUES, DoubleMatrix.copyOf(coefMat), 5, 1);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray differentiate = function.differentiate(result, xKeys).row(0);
		DoubleArray differentiate = function.differentiate(result, xKeys).row(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray differentiateTwice = function.differentiateTwice(result, xKeys).row(0);
		DoubleArray differentiateTwice = function.differentiateTwice(result, xKeys).row(0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] integrate = new double[nInit][nKeys];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] integrate = new double[nInit][nKeys];
		double[][] integrate = RectangularArrays.ReturnRectangularDoubleArray(nInit, nKeys);
		for (int i = 0; i < nInit; ++i)
		{
		  for (int j = 0; j < nKeys; ++j)
		  {
			integrate[i][j] = function.integrate(result, initials[i], xKeys).get(j);
		  }
		}

		for (int i = 0; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = differentiateExp[i] == 0.0 ? 1.0 : Math.abs(differentiateExp[i]);
		  double @ref = differentiateExp[i] == 0.0 ? 1.0 : Math.Abs(differentiateExp[i]);
		  assertEquals(differentiate.get(i), differentiateExp[i], @ref * EPS);
		}
		for (int i = 0; i < nKeys; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = differentiateTwiceExp[i] == 0.0 ? 1.0 : Math.abs(differentiateTwiceExp[i]);
		  double @ref = differentiateTwiceExp[i] == 0.0 ? 1.0 : Math.Abs(differentiateTwiceExp[i]);
		  assertEquals(differentiateTwice.get(i), differentiateTwiceExp[i], @ref * EPS);
		}

		for (int j = 0; j < nInit; ++j)
		{
		  for (int i = 0; i < nKeys; ++i)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = integrateExp[j][i] == 0.0 ? 1.0 : Math.abs(integrateExp[j][i]);
			double @ref = integrateExp[j][i] == 0.0 ? 1.0 : Math.Abs(integrateExp[j][i]);
			assertEquals(integrate[j][i], integrateExp[j][i], @ref * EPS);
		  }
		}

		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = differentiateExp[0] == 0.0 ? 1.0 : Math.abs(differentiateExp[0]);
		  double @ref = differentiateExp[0] == 0.0 ? 1.0 : Math.Abs(differentiateExp[0]);
		  assertEquals(function.differentiate(result, xKeys[0]).get(0), differentiateExp[0], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = differentiateExp[3] == 0.0 ? 1.0 : Math.abs(differentiateExp[3]);
		  double @ref = differentiateExp[3] == 0.0 ? 1.0 : Math.Abs(differentiateExp[3]);
		  assertEquals(function.differentiate(result, xKeys[3]).get(0), differentiateExp[3], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = differentiateTwiceExp[0] == 0.0 ? 1.0 : Math.abs(differentiateTwiceExp[0]);
		  double @ref = differentiateTwiceExp[0] == 0.0 ? 1.0 : Math.Abs(differentiateTwiceExp[0]);
		  assertEquals(function.differentiateTwice(result, xKeys[0]).get(0), differentiateTwiceExp[0], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = differentiateTwiceExp[3] == 0.0 ? 1.0 : Math.abs(differentiateTwiceExp[3]);
		  double @ref = differentiateTwiceExp[3] == 0.0 ? 1.0 : Math.Abs(differentiateTwiceExp[3]);
		  assertEquals(function.differentiateTwice(result, xKeys[3]).get(0), differentiateTwiceExp[3], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = integrateExp[0][0] == 0.0 ? 1.0 : Math.abs(integrateExp[0][0]);
		  double @ref = integrateExp[0][0] == 0.0 ? 1.0 : Math.Abs(integrateExp[0][0]);
		  assertEquals(function.integrate(result, initials[0], xKeys[0]), integrateExp[0][0], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = integrateExp[0][3] == 0.0 ? 1.0 : Math.abs(integrateExp[0][3]);
		  double @ref = integrateExp[0][3] == 0.0 ? 1.0 : Math.Abs(integrateExp[0][3]);
		  assertEquals(function.integrate(result, initials[0], xKeys[3]), integrateExp[0][3], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = integrateExp[3][0] == 0.0 ? 1.0 : Math.abs(integrateExp[3][0]);
		  double @ref = integrateExp[3][0] == 0.0 ? 1.0 : Math.Abs(integrateExp[3][0]);
		  assertEquals(function.integrate(result, initials[3], xKeys[0]), integrateExp[3][0], @ref * EPS);
		}
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ref = integrateExp[1][0] == 0.0 ? 1.0 : Math.abs(integrateExp[1][0]);
		  double @ref = integrateExp[1][0] == 0.0 ? 1.0 : Math.Abs(integrateExp[1][0]);
		  assertEquals(function.integrate(result, initials[1], xKeys[0]), integrateExp[1][0], @ref * EPS);
		}
	  }

	  /// <summary>
	  /// Consistency with evaluate and differentiate.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void evaluateAndDifferentiateTest()
	  public virtual void evaluateAndDifferentiateTest()
	  {
		double[][][] coefsMatrix = new double[][][]
		{
			new double[][]
			{
				new double[] {1.0, -3.0, 3.0, -1},
				new double[] {1.0, 0.0, 0.0, 0.0},
				new double[] {1.0, 3.0, 3.0, 1.0}
			},
			new double[][]
			{
				new double[] {0.0, 5.0, -20.0, 20},
				new double[] {0.0, 5.0, -10.0, 5},
				new double[] {0.0, 5.0, 0.0, 0.0}
			}
		};
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		int dim = 2;
		int nCoefs = 4;
		int keyLength = xKeys[0].Length;
		PiecewisePolynomialResult[] pp = new PiecewisePolynomialResult[dim];
		for (int i = 0; i < dim; ++i)
		{
		  pp[i] = new PiecewisePolynomialResult(X_VALUES, DoubleMatrix.ofUnsafe(coefsMatrix[i]), nCoefs, 1);
		}
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();
		for (int i = 0; i < dim; ++i)
		{
		  for (int j = 0; j < keyLength; ++j)
		  {
			ValueDerivatives computed = function.evaluateAndDifferentiate(pp[i], xKeys[i][j]);
			double value = function.evaluate(pp[i], xKeys[i][j]).get(0);
			double deriv = function.differentiate(pp[i], xKeys[i][j]).get(0);
			assertEquals(computed.Value, value, EPS);
			assertEquals(computed.Derivatives.size(), 1);
			assertEquals(computed.getDerivative(0), deriv, EPS);
		  }
		}
	  }

	  /// <summary>
	  /// Error tests below
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullpEvaluateTest()
	  public virtual void nullpEvaluateTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1.0, -3.0, 3.0, -1 }, {0.0, 5.0, -20.0, 20 }, {1.0, 0.0, 0.0, 0.0 }, {0.0, 5.0, -10.0, 5 }, {1.0, 3.0, 3.0, 1.0 }, {0.0, 5.0, 0.0, 0.0 } });
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKeys = new double[][] { {-2, 1, 2, 2.5 }, {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0 } };
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		pp = null;
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys[0][0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullpEvaluateMultiTest()
	  public virtual void nullpEvaluateMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1.0, -3.0, 3.0, -1 }, {0.0, 5.0, -20.0, 20 }, {1.0, 0.0, 0.0, 0.0 }, {0.0, 5.0, -10.0, 5 }, {1.0, 3.0, 3.0, 1.0 }, {0.0, 5.0, 0.0, 0.0 } });
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKeys = new double[][] { {-2, 1, 2, 2.5 }, {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0 } };
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		pp = null;
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullpEvaluateMatrixTest()
	  public virtual void nullpEvaluateMatrixTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1.0, -3.0, 3.0, -1 }, {0.0, 5.0, -20.0, 20 }, {1.0, 0.0, 0.0, 0.0 }, {0.0, 5.0, -10.0, 5 }, {1.0, 3.0, 3.0, 1.0 }, {0.0, 5.0, 0.0, 0.0 } });
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKeys = new double[][] { {-2, 1, 2, 2.5 }, {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0 } };
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		pp = null;
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullpIntegrateTest()
	  public virtual void nullpIntegrateTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1.0, -3.0, 3.0, -1 }, {1.0, 0.0, 0.0, 0.0 }, {1.0, 3.0, 3.0, 1.0 } });
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {1.0, 3.0, 3.0, 1.0}
		});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKeys = new double[][] { {-2, 1, 2, 2.5 }, {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0 } };
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 1;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		pp = null;
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.integrate(pp, 1.0, xKeys[0][0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullpIntegrateMultiTest()
	  public virtual void nullpIntegrateMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1.0, -3.0, 3.0, -1 }, {1.0, 0.0, 0.0, 0.0 }, {1.0, 3.0, 3.0, 1.0 } });
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {1.0, 3.0, 3.0, 1.0}
		});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKeys = new double[][] { {-2, 1, 2, 2.5 }, {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0 } };
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 1;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		pp = null;
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.integrate(pp, 1.0, xKeys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullpDifferentiateTest()
	  public virtual void nullpDifferentiateTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1.0, -3.0, 3.0, -1 }, {0.0, 5.0, -20.0, 20 }, {1.0, 0.0, 0.0, 0.0 }, {0.0, 5.0, -10.0, 5 }, {1.0, 3.0, 3.0, 1.0 }, {0.0, 5.0, 0.0, 0.0 } });
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKeys = new double[][] { {-2, 1, 2, 2.5 }, {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0 } };
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		pp = null;
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.differentiate(pp, xKeys[0][0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullpDifferentiateMultiTest()
	  public virtual void nullpDifferentiateMultiTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix coefsMatrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(new double[][] { {1.0, -3.0, 3.0, -1 }, {0.0, 5.0, -20.0, 20 }, {1.0, 0.0, 0.0, 0.0 }, {0.0, 5.0, -10.0, 5 }, {1.0, 3.0, 3.0, 1.0 }, {0.0, 5.0, 0.0, 0.0 } });
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] xKeys = new double[][] { {-2, 1, 2, 2.5 }, {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0 } };
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		pp = null;
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.differentiate(pp, xKeys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullxEvaluateTest()
	  public virtual void nullxEvaluateTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[] xKeys = new double[] {-2, 1, 2, 2.5};
		const int dim = 2;
		const int nCoefs = 4;

		xKeys = null;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullxEvaluateMatrixTest()
	  public virtual void nullxEvaluateMatrixTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 2;
		const int nCoefs = 4;

		xKeys = null;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullxIntTest()
	  public virtual void nullxIntTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {1.0, 3.0, 3.0, 1.0}
		});
		double[] xKeys = new double[] {-2, 1, 2, 2.5};
		const int dim = 1;
		const int nCoefs = 4;

		xKeys = null;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.integrate(pp, 1.0, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullxDiffTest()
	  public virtual void nullxDiffTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[] xKeys = new double[] {-2, 1, 2, 2.5};
		const int dim = 2;
		const int nCoefs = 4;

		xKeys = null;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.differentiate(pp, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infxEvaluateTest()
	  public virtual void infxEvaluateTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[][] xKeys = new double[][]
		{
			new double[] {INF, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys[0][0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infxEvaluateMultiTest()
	  public virtual void infxEvaluateMultiTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, INF, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infxEvaluateMatrixTest()
	  public virtual void infxEvaluateMatrixTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, INF}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infxIntTest()
	  public virtual void infxIntTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {1.0, 3.0, 3.0, 1.0}
		});
		double[][] xKeys = new double[][]
		{
			new double[] {INF, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 1;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.integrate(pp, 1.0, xKeys[0][0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void infxIntMultiTest()
	  public virtual void infxIntMultiTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {1.0, 3.0, 3.0, 1.0}
		});
		double[] xKeys = new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, INF};
		const int dim = 1;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.integrate(pp, 1.0, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNxEvaluateTest()
	  public virtual void NaNxEvaluateTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[][] xKeys = new double[][]
		{
			new double[] {Double.NaN, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys[0][0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNxEvaluateMultiTest()
	  public virtual void NaNxEvaluateMultiTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, Double.NaN, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNxEvaluateMatrixTest()
	  public virtual void NaNxEvaluateMatrixTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[][] xKeys = new double[][]
		{
			new double[] {-2, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, Double.NaN}
		};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.evaluate(pp, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNxIntTest()
	  public virtual void NaNxIntTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {1.0, 3.0, 3.0, 1.0}
		});
		double[][] xKeys = new double[][]
		{
			new double[] {Double.NaN, 1, 2, 2.5},
			new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, 5.0}
		};
		const int dim = 1;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.integrate(pp, 1.0, xKeys[0][0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNxIntMultiTest()
	  public virtual void NaNxIntMultiTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {1.0, 3.0, 3.0, 1.0}
		});
		double[] xKeys = new double[] {1.5, 7.0 / 3.0, 29.0 / 7.0, Double.NaN};
		const int dim = 1;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.integrate(pp, 1.0, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullDimIntTest()
	  public virtual void nullDimIntTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[] xKeys = new double[] {-2, 1, 2, 2.5};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.integrate(pp, 1.0, xKeys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void nullDimIntMultiTest()
	  public virtual void nullDimIntMultiTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		double[] xKeys = new double[] {-2, 1, 2, 2.5};
		const int dim = 2;
		const int nCoefs = 4;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.integrate(pp, 1.0, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void constFuncDiffTest()
	  public virtual void constFuncDiffTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {-1},
			new double[] {20},
			new double[] {0.0},
			new double[] {5},
			new double[] {1.0},
			new double[] {0.0}
		});
		double[] xKeys = new double[] {-2, 1, 2, 2.5};
		const int dim = 2;
		const int nCoefs = 1;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.differentiate(pp, xKeys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void constFuncDiffMultiTest()
	  public virtual void constFuncDiffMultiTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {-1},
			new double[] {20},
			new double[] {0.0},
			new double[] {5},
			new double[] {1.0},
			new double[] {0.0}
		});
		double[] xKeys = new double[] {-2, 1, 2, 2.5};
		const int dim = 2;
		const int nCoefs = 1;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.differentiate(pp, xKeys);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void linearFuncDiffTwiceTest()
	  public virtual void linearFuncDiffTwiceTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0},
			new double[] {0.0, 5.0},
			new double[] {1.0, 0.0},
			new double[] {0.0, 5.0},
			new double[] {1.0, 3.0},
			new double[] {0.0, 5.0}
		});
		double[] xKeys = new double[] {-2, 1, 2, 2.5};
		const int dim = 2;
		const int nCoefs = 2;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.differentiateTwice(pp, xKeys[0]);
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void linearFuncDiffTwiceMultiTest()
	  public virtual void linearFuncDiffTwiceMultiTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0},
			new double[] {0.0, 5.0},
			new double[] {1.0, 0.0},
			new double[] {0.0, 5.0},
			new double[] {1.0, 3.0},
			new double[] {0.0, 5.0}
		});
		double[] xKeys = new double[] {-2, 1, 2, 2.5};
		const int dim = 2;
		const int nCoefs = 2;

		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();

		function.differentiateTwice(pp, xKeys);
	  }

	  /// <summary>
	  /// dim must be 1 for evaluateAndDifferentiate.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void dimFailTest()
	  public virtual void dimFailTest()
	  {
		DoubleMatrix coefsMatrix = DoubleMatrix.copyOf(new double[][]
		{
			new double[] {1.0, -3.0, 3.0, -1},
			new double[] {0.0, 5.0, -20.0, 20},
			new double[] {1.0, 0.0, 0.0, 0.0},
			new double[] {0.0, 5.0, -10.0, 5},
			new double[] {1.0, 3.0, 3.0, 1.0},
			new double[] {0.0, 5.0, 0.0, 0.0}
		});
		int dim = 2;
		int nCoefs = 4;
		PiecewisePolynomialResult pp = new PiecewisePolynomialResult(X_VALUES, coefsMatrix, nCoefs, dim);
		PiecewisePolynomialFunction1D function = new PiecewisePolynomialFunction1D();
		function.evaluateAndDifferentiate(pp, 1.5);
	  }

	}

}