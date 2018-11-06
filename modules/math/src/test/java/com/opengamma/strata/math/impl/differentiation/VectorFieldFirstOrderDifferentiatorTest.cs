using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class VectorFieldFirstOrderDifferentiatorTest
	public class VectorFieldFirstOrderDifferentiatorTest
	{

	  private static readonly System.Func<DoubleArray, DoubleArray> F = (final DoubleArray x) =>
	  {

  double x1 = x.get(0);
  double x2 = x.get(1);
  return DoubleArray.of(x1 * x1 + 2 * x2 * x2 - x1 * x2 + x1 * Math.Cos(x2) - x2 * Math.Sin(x1), 2 * x1 * x2 * Math.Cos(x1 * x2) - x1 * Math.Sin(x1) - x2 * Math.Cos(x2));
	  };

	  private static readonly System.Func<DoubleArray, DoubleArray> F2 = (final DoubleArray x) =>
	  {

  double x1 = x.get(0);
  double x2 = x.get(1);
  return DoubleArray.of(x1 * x1 + 2 * x2 * x2 - x1 * x2 + x1 * Math.Cos(x2) - x2 * Math.Sin(x1), 2 * x1 * x2 * Math.Cos(x1 * x2) - x1 * Math.Sin(x1) - x2 * Math.Cos(x2), x1 - x2);
	  };

	  private static readonly System.Func<DoubleArray, DoubleMatrix> G = (final DoubleArray x) =>
	  {

  double x1 = x.get(0);
  double x2 = x.get(1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] jac = new double[2][2];
  double[][] jac = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
  jac[0][0] = 2 * x1 - x2 + Math.Cos(x2) - x2 * Math.Cos(x1);
  jac[0][1] = 4 * x2 - x1 - x1 * Math.Sin(x2) - Math.Sin(x1);
  jac[1][0] = 2 * x2 * Math.Cos(x1 * x2) - 2 * x1 * x2 * x2 * Math.Sin(x1 * x2) - Math.Sin(x1) - x1 * Math.Cos(x1);
  jac[1][1] = 2 * x1 * Math.Cos(x1 * x2) - 2 * x1 * x1 * x2 * Math.Sin(x1 * x2) - Math.Cos(x2) + x2 * Math.Sin(x2);
  return DoubleMatrix.copyOf(jac);
	  };

	  private static readonly System.Func<DoubleArray, DoubleMatrix> G2 = (final DoubleArray x) =>
	  {

  double x1 = x.get(0);
  double x2 = x.get(1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] jac = new double[3][2];
  double[][] jac = RectangularArrays.ReturnRectangularDoubleArray(3, 2);
  jac[0][0] = 2 * x1 - x2 + Math.Cos(x2) - x2 * Math.Cos(x1);
  jac[0][1] = 4 * x2 - x1 - x1 * Math.Sin(x2) - Math.Sin(x1);
  jac[1][0] = 2 * x2 * Math.Cos(x1 * x2) - 2 * x1 * x2 * x2 * Math.Sin(x1 * x2) - Math.Sin(x1) - x1 * Math.Cos(x1);
  jac[1][1] = 2 * x1 * Math.Cos(x1 * x2) - 2 * x1 * x1 * x2 * Math.Sin(x1 * x2) - Math.Cos(x2) + x2 * Math.Sin(x2);
  jac[2][0] = 1;
  jac[2][1] = -1;
  return DoubleMatrix.copyOf(jac);
	  };

	  private static readonly System.Func<DoubleArray, bool> DOMAIN = (final DoubleArray x) =>
	  {

  double x1 = x.get(0);
  double x2 = x.get(1);
  if (x1 < 0 || x1 > Math.PI || x2 < 0 || x2 > Math.PI)
  {
	return false;
  }
  return true;

	  };

	  private const double EPS = 1e-5;
	  private static readonly VectorFieldFirstOrderDifferentiator FORWARD = new VectorFieldFirstOrderDifferentiator(FiniteDifferenceType.FORWARD, EPS);
	  private static readonly VectorFieldFirstOrderDifferentiator CENTRAL = new VectorFieldFirstOrderDifferentiator(FiniteDifferenceType.CENTRAL, EPS);
	  private static readonly VectorFieldFirstOrderDifferentiator BACKWARD = new VectorFieldFirstOrderDifferentiator(FiniteDifferenceType.BACKWARD, EPS);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDifferenceType()
	  public virtual void testNullDifferenceType()
	  {
		new ScalarFirstOrderDifferentiator(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		CENTRAL.differentiate((System.Func<DoubleArray, DoubleArray>) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x = com.opengamma.strata.collect.array.DoubleArray.of(.2245, -1.2344);
		DoubleArray x = DoubleArray.of(.2245, -1.2344);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix anJac = G.apply(x);
		DoubleMatrix anJac = G.apply(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix fdFwdJac = FORWARD.differentiate(F).apply(x);
		DoubleMatrix fdFwdJac = FORWARD.differentiate(F).apply(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix fdCentGrad = CENTRAL.differentiate(F).apply(x);
		DoubleMatrix fdCentGrad = CENTRAL.differentiate(F).apply(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix fdBackGrad = BACKWARD.differentiate(F).apply(x);
		DoubleMatrix fdBackGrad = BACKWARD.differentiate(F).apply(x);

		for (int i = 0; i < 2; i++)
		{
		  for (int j = 0; j < 2; j++)
		  {
			assertEquals(fdFwdJac.get(i, j), anJac.get(i, j), 10 * EPS);
			assertEquals(fdCentGrad.get(i, j), anJac.get(i, j), 10 * EPS * EPS);
			assertEquals(fdBackGrad.get(i, j), anJac.get(i, j), 10 * EPS);
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test2()
	  public virtual void test2()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x = com.opengamma.strata.collect.array.DoubleArray.of(1.3423, 0.235);
		DoubleArray x = DoubleArray.of(1.3423, 0.235);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix anJac = G2.apply(x);
		DoubleMatrix anJac = G2.apply(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix fdFwdJac = FORWARD.differentiate(F2).apply(x);
		DoubleMatrix fdFwdJac = FORWARD.differentiate(F2).apply(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix fdCentGrad = CENTRAL.differentiate(F2).apply(x);
		DoubleMatrix fdCentGrad = CENTRAL.differentiate(F2).apply(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix fdBackGrad = BACKWARD.differentiate(F2).apply(x);
		DoubleMatrix fdBackGrad = BACKWARD.differentiate(F2).apply(x);

		for (int i = 0; i < 3; i++)
		{
		  for (int j = 0; j < 2; j++)
		  {
			assertEquals(fdFwdJac.get(i, j), anJac.get(i, j), 10 * EPS);
			assertEquals(fdCentGrad.get(i, j), anJac.get(i, j), 10 * EPS * EPS);
			assertEquals(fdBackGrad.get(i, j), anJac.get(i, j), 10 * EPS);
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void outsideDomainTest()
	  public virtual void outsideDomainTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleMatrix> fdJacFunc = CENTRAL.differentiate(F2, DOMAIN);
		System.Func<DoubleArray, DoubleMatrix> fdJacFunc = CENTRAL.differentiate(F2, DOMAIN);
		fdJacFunc(DoubleArray.of(2.3, 3.2));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testDomain()
	  public virtual void testDomain()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] x = new com.opengamma.strata.collect.array.DoubleArray[7];
		DoubleArray[] x = new DoubleArray[7];

		x[0] = DoubleArray.of(1.3423, 0.235);
		x[1] = DoubleArray.of(0.0, 1.235);
		x[2] = DoubleArray.of(Math.PI, 3.1);
		x[3] = DoubleArray.of(2.3, 0.0);
		x[4] = DoubleArray.of(2.3, Math.PI);
		x[5] = DoubleArray.of(0.0, 0.0);
		x[6] = DoubleArray.of(Math.PI, Math.PI);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleMatrix> fdJacFunc = CENTRAL.differentiate(F2, DOMAIN);
		System.Func<DoubleArray, DoubleMatrix> fdJacFunc = CENTRAL.differentiate(F2, DOMAIN);

		for (int k = 0; k < 7; k++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix anJac = G2.apply(x[k]);
		  DoubleMatrix anJac = G2.apply(x[k]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix fdJac = fdJacFunc.apply(x[k]);
		  DoubleMatrix fdJac = fdJacFunc(x[k]);

		  for (int i = 0; i < 3; i++)
		  {
			for (int j = 0; j < 2; j++)
			{
			  assertEquals("set " + k, anJac.get(i, j), fdJac.get(i, j), 1e-8);
			}
		  }
		}
	  }

	}

}