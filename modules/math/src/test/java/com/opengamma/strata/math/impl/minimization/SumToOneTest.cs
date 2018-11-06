using System;

/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using VectorFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.VectorFieldFirstOrderDifferentiator;
	using DecompositionFactory = com.opengamma.strata.math.impl.linearalgebra.DecompositionFactory;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using LeastSquareResults = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResults;
	using NonLinearLeastSquare = com.opengamma.strata.math.impl.statistics.leastsquare.NonLinearLeastSquare;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SumToOneTest
	public class SumToOneTest
	{

	  private static readonly MatrixAlgebra MA = new OGMatrixAlgebra();
	  private static readonly NonLinearLeastSquare SOLVER = new NonLinearLeastSquare(DecompositionFactory.SV_COMMONS, MA, 1e-9);
	  private static readonly VectorFieldFirstOrderDifferentiator DIFFER = new VectorFieldFirstOrderDifferentiator();
	  private static readonly Well44497b RANDOM = new Well44497b(0L);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void setTest()
	  public virtual void setTest()
	  {
		int n = 7;
		int[][] sets = SumToOne.getSet(n);
		assertEquals(n, sets.Length);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void setTest2()
	  public virtual void setTest2()
	  {
		int n = 13;
		int[][] sets = SumToOne.getSet(n);
		assertEquals(n, sets.Length);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void transformTest()
	  public virtual void transformTest()
	  {
		for (int n = 2; n < 13; n++)
		{
		  double[] from = new double[n - 1];
		  for (int j = 0; j < n - 1; j++)
		  {
			from[j] = RANDOM.NextDouble() * Math.PI / 2;
		  }
		  SumToOne trans = new SumToOne(n);
		  DoubleArray to = trans.transform(DoubleArray.copyOf(from));
		  assertEquals(n, to.size());
		  double sum = 0;
		  for (int i = 0; i < n; i++)
		  {
			sum += to.get(i);
		  }
		  assertEquals("vector length " + n, 1.0, sum, 1e-9);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void inverseTransformTest()
	  public virtual void inverseTransformTest()
	  {
		for (int n = 2; n < 13; n++)
		{
		  double[] theta = new double[n - 1];
		  for (int j = 0; j < n - 1; j++)
		  {
			theta[j] = RANDOM.NextDouble() * Math.PI / 2;
		  }
		  SumToOne trans = new SumToOne(n);
		  DoubleArray w = trans.transform(DoubleArray.copyOf(theta));

		  DoubleArray theta2 = trans.inverseTransform(w);
		  for (int j = 0; j < n - 1; j++)
		  {
			assertEquals("element " + j + ", of vector length " + n, theta[j], theta2.get(j), 1e-9);
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void solverTest()
	  public virtual void solverTest()
	  {
		double[] w = new double[] {0.01, 0.5, 0.3, 0.19};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = w.length;
		int n = w.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final SumToOne trans = new SumToOne(n);
		SumToOne trans = new SumToOne(n);
		System.Func<DoubleArray, DoubleArray> func = (DoubleArray theta) =>
		{

	return trans.transform(theta);
		};

		DoubleArray sigma = DoubleArray.filled(n, 1e-4);
		DoubleArray start = DoubleArray.filled(n - 1, 0.8);

		LeastSquareResults res = SOLVER.solve(DoubleArray.copyOf(w), sigma, func, start);
		assertEquals("chi sqr", 0.0, res.ChiSq, 1e-9);
		double[] fit = res.FitParameters.toArray();
		double[] expected = trans.inverseTransform(w);
		for (int i = 0; i < n - 1; i++)
		{
		  //put the fit result back in the range 0 - pi/2
		  double x = fit[i];
		  if (x < 0)
		  {
			x = -x;
		  }
		  if (x > Math.PI / 2)
		  {
			int p = (int)(x / Math.PI);
			x -= p * Math.PI;
			if (x > Math.PI / 2)
			{
			  x = -x + Math.PI;
			}
		  }

		  assertEquals(expected[i], x, 1e-9);
		}

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void solverTest2()
	  public virtual void solverTest2()
	  {
		double[] w = new double[] {3.0, 4.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = w.length;
		int n = w.Length;
		System.Func<DoubleArray, DoubleArray> func = (DoubleArray x) =>
		{

	double a = x.get(0);
	double theta = x.get(1);
	double c1 = Math.Cos(theta);
	return DoubleArray.of(a * c1 * c1, a * (1 - c1 * c1));
		};

		DoubleArray sigma = DoubleArray.filled(n, 1e-4);
		DoubleArray start = DoubleArray.of(0.0, 0.8);

		LeastSquareResults res = SOLVER.solve(DoubleArray.copyOf(w), sigma, func, start);
		assertEquals("chi sqr", 0.0, res.ChiSq, 1e-9);
		double[] fit = res.FitParameters.toArray();
		assertEquals(7.0, fit[0], 1e-9);
		assertEquals(Math.Atan(Math.Sqrt(4 / 3.0)), fit[1], 1e-9);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void jacobianTest()
	  public virtual void jacobianTest()
	  {
		const int n = 5;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final SumToOne trans = new SumToOne(n);
		SumToOne trans = new SumToOne(n);
		System.Func<DoubleArray, DoubleArray> func = (DoubleArray theta) =>
		{
	return trans.transform(theta);
		};

		System.Func<DoubleArray, DoubleMatrix> jacFunc = (DoubleArray theta) =>
		{
	return trans.jacobian(theta);
		};

		System.Func<DoubleArray, DoubleMatrix> fdJacFunc = DIFFER.differentiate(func);

		for (int tries = 0; tries < 10; tries++)
		{
		  DoubleArray vTheta = DoubleArray.of(n - 1, i => RANDOM.NextDouble());
		  DoubleMatrix jac = jacFunc(vTheta);
		  DoubleMatrix fdJac = fdJacFunc(vTheta);
		  for (int j = 0; j < n - 1; j++)
		  {
			double sum = 0.0;
			for (int i = 0; i < n; i++)
			{
			  sum += jac.get(i, j);
			  assertEquals("element " + i + " " + j, fdJac.get(i, j), jac.get(i, j), 1e-6);
			}
			assertEquals("wrong sum of sensitivities", 0.0, sum, 1e-15);
		  }

		}
	  }
	}

}