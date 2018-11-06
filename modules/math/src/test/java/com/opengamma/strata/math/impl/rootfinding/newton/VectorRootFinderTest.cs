using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory.OG_ALGEBRA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class VectorRootFinderTest
	public abstract class VectorRootFinderTest
	{
	  internal const double EPS = 1e-6;
	  internal const double TOLERANCE = 1e-8;
	  internal const int MAXSTEPS = 100;
	  internal static readonly System.Func<DoubleArray, DoubleArray> LINEAR = (final DoubleArray x) =>
	  {

  double[] data = x.toArray();
  if (data.Length != 2)
  {
	throw new System.ArgumentException("This test is for 2-d vector only");
  }
  return DoubleArray.of(data[0] + data[1], 2 * data[0] - data[1] - 3.0);
	  };
	  internal static readonly System.Func<DoubleArray, DoubleArray> FUNCTION2D = (final DoubleArray x) =>
	  {

  double[] data = x.toArray();
  if (data.Length != 2)
  {
	throw new System.ArgumentException("This test is for 2-d vector only");
  }
  return DoubleArray.of(data[1] * Math.Exp(data[0]) - Math.E, data[0] * data[0] + data[1] * data[1] - 2.0);
	  };
	  internal static readonly System.Func<DoubleArray, DoubleMatrix> JACOBIAN2D = (final DoubleArray x) =>
	  {

  if (x.size() != 2)
  {
	throw new System.ArgumentException("This test is for 2-d vector only");
  }
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[2][2];
  double[][] res = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
  double temp = Math.Exp(x.get(0));

  res[0][0] = x.get(1) * temp;
  res[0][1] = temp;
  for (int i = 0; i < 2; i++)
  {
	res[1][i] = 2 * x.get(i);
  }

  return DoubleMatrix.copyOf(res);

	  };

	  internal static readonly System.Func<DoubleArray, DoubleArray> FUNCTION3D = (final DoubleArray x) =>
	  {

  if (x.size() != 3)
  {
	throw new System.ArgumentException("This test is for 3-d vector only");
  }
  return DoubleArray.of(Math.Exp(x.get(0) + x.get(1)) + x.get(2) - Math.E + 1.0, x.get(2) * Math.Exp(x.get(0) - x.get(1)) + Math.E, OG_ALGEBRA.getInnerProduct(x, x) - 2.0);
	  };
	  internal static readonly System.Func<DoubleArray, DoubleMatrix> JACOBIAN3D = (final DoubleArray x) =>
	  {

  if (x.size() != 3)
  {
	throw new System.ArgumentException("This test is for 3-d vector only");
  }
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[3][3];
  double[][] res = RectangularArrays.ReturnRectangularDoubleArray(3, 3);
  double temp1 = Math.Exp(x.get(0) + x.get(1));
  double temp2 = Math.Exp(x.get(0) - x.get(1));
  res[0][0] = res[0][1] = temp1;
  res[0][2] = 1.0;
  res[1][0] = x.get(2) * temp2;
  res[1][1] = -x.get(2) * temp2;
  res[1][2] = temp2;
  for (int i = 0; i < 3; i++)
  {
	res[2][i] = 2 * x.get(i);
  }

  return DoubleMatrix.copyOf(res);

	  };

	  internal static readonly double[] TIME_GRID = new double[] {0.25, 0.5, 1.0, 1.5, 2.0, 3.0, 5.0, 7.0, 10.0, 15.0, 20.0, 25.0, 30.0};
	  internal static readonly System.Func<double, double> DUMMY_YIELD_CURVE = new FuncAnonymousInnerClass();

	  private class FuncAnonymousInnerClass : System.Func<double, double>
	  {
		  public FuncAnonymousInnerClass()
		  {
		  }


		  private const double a = -0.03;
		  private const double b = 0.02;
		  private const double c = 0.5;
		  private const double d = 0.05;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> apply(final System.Nullable<double> x)
		  public override double? apply(double? x)
		  {
			return Math.Exp(-x * ((a + b * x) * Math.Exp(-c * x) + d));
		  }
	  }
	  internal static readonly System.Func<DoubleArray, DoubleArray> SWAP_RATES = new FuncAnonymousInnerClass2();

	  private class FuncAnonymousInnerClass2 : System.Func<DoubleArray, DoubleArray>
	  {
		  public FuncAnonymousInnerClass2()
		  {
		  }


		  private readonly int n = TIME_GRID.Length;
		  private double[] _swapRates = null;

		  private void calculateSwapRates()
		  {
			if (_swapRates != null)
			{
			  return;
			}
			_swapRates = new double[n];
			double acc = 0.0;
			double pi;
			for (int i = 0; i < n; i++)
			{
			  pi = DUMMY_YIELD_CURVE.apply(TIME_GRID[i]);
			  acc += (TIME_GRID[i] - (i == 0 ? 0.0 : TIME_GRID[i - 1])) * pi;
			  _swapRates[i] = (1.0 - pi) / acc;
			}
		  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleArray apply(final com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleArray apply(DoubleArray x)
		  {
			calculateSwapRates();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yield = x.toArray();
			double[] yield = x.toArray();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] diff = new double[n];
			double[] diff = new double[n];
			double pi;
			double acc = 0.0;
			for (int i = 0; i < n; i++)
			{
			  pi = Math.Exp(-yield[i] * TIME_GRID[i]);
			  acc += (TIME_GRID[i] - (i == 0 ? 0.0 : TIME_GRID[i - 1])) * pi;
			  diff[i] = (1.0 - pi) / acc - _swapRates[i];
			}
			return DoubleArray.copyOf(diff);
		  }

	  }
	  private static readonly VectorRootFinder DUMMY = new VectorRootFinderAnonymousInnerClass();

	  private class VectorRootFinderAnonymousInnerClass : VectorRootFinder
	  {
		  public VectorRootFinderAnonymousInnerClass()
		  {
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleArray getRoot(final System.Func<com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleArray> function, final com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleArray getRoot(System.Func<DoubleArray, DoubleArray> function, DoubleArray x)
		  {
			checkInputs(function, x);
			return null;
		  }

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		DUMMY.getRoot(null, DoubleArray.EMPTY);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullVector()
	  public virtual void testNullVector()
	  {
		DUMMY.getRoot(LINEAR, (DoubleArray) null);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertLinear(final com.opengamma.strata.math.impl.rootfinding.VectorRootFinder rootFinder, final double eps)
	  protected internal virtual void assertLinear(VectorRootFinder rootFinder, double eps)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x0 = com.opengamma.strata.collect.array.DoubleArray.of(0.0, 0.0);
		DoubleArray x0 = DoubleArray.of(0.0, 0.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x1 = rootFinder.getRoot(LINEAR, x0);
		DoubleArray x1 = rootFinder.getRoot(LINEAR, x0);
		assertEquals(1.0, x1.get(0), eps);
		assertEquals(-1.0, x1.get(1), eps);
	  }

	  // Note: at the root (1,1) the Jacobian is singular which leads to very slow convergence and is why
	  // we switch to using SVD rather than the default LU
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertFunction2D(final BaseNewtonVectorRootFinder rootFinder, final double eps)
	  protected internal virtual void assertFunction2D(BaseNewtonVectorRootFinder rootFinder, double eps)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x0 = com.opengamma.strata.collect.array.DoubleArray.of(-0.0, 0.0);
		DoubleArray x0 = DoubleArray.of(-0.0, 0.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x1 = rootFinder.findRoot(FUNCTION2D, JACOBIAN2D, x0);
		DoubleArray x1 = rootFinder.findRoot(FUNCTION2D, JACOBIAN2D, x0);
		assertEquals(1.0, x1.get(0), eps);
		assertEquals(1.0, x1.get(1), eps);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertFunction3D(final BaseNewtonVectorRootFinder rootFinder, final double eps)
	  protected internal virtual void assertFunction3D(BaseNewtonVectorRootFinder rootFinder, double eps)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x0 = com.opengamma.strata.collect.array.DoubleArray.of(0.8, 0.2, -0.7);
		DoubleArray x0 = DoubleArray.of(0.8, 0.2, -0.7);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x1 = rootFinder.findRoot(FUNCTION3D, JACOBIAN3D, x0);
		DoubleArray x1 = rootFinder.findRoot(FUNCTION3D, JACOBIAN3D, x0);
		assertEquals(1.0, x1.get(0), eps);
		assertEquals(0.0, x1.get(1), eps);
		assertEquals(-1.0, x1.get(2), eps);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertYieldCurveBootstrap(final com.opengamma.strata.math.impl.rootfinding.VectorRootFinder rootFinder, final double eps)
	  protected internal virtual void assertYieldCurveBootstrap(VectorRootFinder rootFinder, double eps)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = TIME_GRID.length;
		int n = TIME_GRID.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] flatCurve = new double[n];
		double[] flatCurve = new double[n];
		for (int i = 0; i < n; i++)
		{
		  flatCurve[i] = 0.05;
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x0 = com.opengamma.strata.collect.array.DoubleArray.copyOf(flatCurve);
		DoubleArray x0 = DoubleArray.copyOf(flatCurve);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x1 = rootFinder.getRoot(SWAP_RATES, x0);
		DoubleArray x1 = rootFinder.getRoot(SWAP_RATES, x0);
		for (int i = 0; i < n; i++)
		{
		  assertEquals(-Math.Log(DUMMY_YIELD_CURVE.apply(TIME_GRID[i])) / TIME_GRID[i], x1.get(i), eps);
		}
	  }

	}

}