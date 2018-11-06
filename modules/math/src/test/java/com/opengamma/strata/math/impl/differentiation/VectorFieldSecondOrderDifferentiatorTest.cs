using System;

/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class VectorFieldSecondOrderDifferentiatorTest
	public class VectorFieldSecondOrderDifferentiatorTest
	{

	  private static System.Func<DoubleArray, DoubleArray> FUNC = (DoubleArray x) =>
	  {

  double a = x.get(0);
  double theta = x.get(1);
  double c1 = Math.Cos(theta);
  return DoubleArray.of(a * c1 * c1, a * (1 - c1 * c1));
	  };

	  private static System.Func<DoubleArray, bool> DOMAIN = (DoubleArray x) =>
	  {

  double a = x.get(0);
  double theta = x.get(1);
  if (a <= 0)
  {
	return false;
  }
  if (theta < 0.0 || theta > Math.PI)
  {
	return false;
  }
  return true;
	  };

	  private static System.Func<DoubleArray, DoubleMatrix> DW1 = (DoubleArray x) =>
	  {
  double a = x.get(0);
  double theta = x.get(1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] temp = new double[2][2];
  double[][] temp = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
  double c1 = Math.Cos(theta);
  double s1 = Math.Sin(theta);
  temp[0][0] = 0.0;
  temp[1][1] = 2 * a * (1 - 2 * c1 * c1);
  temp[0][1] = -2 * s1 * c1;
  temp[1][0] = temp[0][1];
  return DoubleMatrix.copyOf(temp);
	  };

	  private static System.Func<DoubleArray, DoubleMatrix> DW2 = (DoubleArray x) =>
	  {
  double a = x.get(0);
  double theta = x.get(1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] temp = new double[2][2];
  double[][] temp = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
  double c1 = Math.Cos(theta);
  double s1 = Math.Sin(theta);
  temp[0][0] = 0.0;
  temp[1][1] = 2 * a * (2 * c1 * c1 - 1);
  temp[0][1] = 2 * s1 * c1;
  temp[1][0] = temp[0][1];
  return DoubleMatrix.copyOf(temp);
	  };

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		double a = 2.3;
		double theta = 0.34;
		DoubleArray x = DoubleArray.of(a, theta);

		VectorFieldSecondOrderDifferentiator fd = new VectorFieldSecondOrderDifferentiator();
		System.Func<DoubleArray, DoubleMatrix[]> fdFuncs = fd.differentiate(FUNC);
		DoubleMatrix[] fdValues = fdFuncs(x);

		DoubleMatrix t1 = DW1.apply(x);
		DoubleMatrix t2 = DW2.apply(x);
		for (int i = 0; i < 2; i++)
		{
		  for (int j = 0; j < 2; j++)
		  {
			assertEquals("first observation " + i + " " + j, t1.get(i, j), fdValues[0].get(i, j), 1e-6);
			assertEquals("second observation " + i + " " + j, t2.get(i, j), fdValues[1].get(i, j), 1e-6);
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void outsideDomainTest()
	  public virtual void outsideDomainTest()
	  {
		VectorFieldSecondOrderDifferentiator fd = new VectorFieldSecondOrderDifferentiator();
		System.Func<DoubleArray, DoubleMatrix[]> fdFuncs = fd.differentiate(FUNC, DOMAIN);
		fdFuncs(DoubleArray.of(-1.0, 0.3));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void domainTest()
	  public virtual void domainTest()
	  {

		DoubleArray[] x = new DoubleArray[4];
		x[0] = DoubleArray.of(2.3, 0.34);
		x[1] = DoubleArray.of(1e-8, 1.45);
		x[2] = DoubleArray.of(1.2, 0.0);
		x[3] = DoubleArray.of(1.2, Math.PI);

		VectorFieldSecondOrderDifferentiator fd = new VectorFieldSecondOrderDifferentiator();
		System.Func<DoubleArray, DoubleMatrix[]> fdFuncs = fd.differentiate(FUNC, DOMAIN);

		for (int k = 0; k < 4; k++)
		{
		  DoubleMatrix[] fdValues = fdFuncs(x[k]);
		  DoubleMatrix t1 = DW1.apply(x[k]);
		  DoubleMatrix t2 = DW2.apply(x[k]);
		  for (int i = 0; i < 2; i++)
		  {
			for (int j = 0; j < 2; j++)
			{
			  assertEquals("first observation " + i + " " + j, t1.get(i, j), fdValues[0].get(i, j), 1e-6);
			  assertEquals("second observation " + i + " " + j, t2.get(i, j), fdValues[1].get(i, j), 1e-6);
			}
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void tes_differentiateNoCrosst()
	  public virtual void tes_differentiateNoCrosst()
	  {
		System.Func<DoubleArray, DoubleArray> func = (DoubleArray x) =>
		{
	double x1 = x.get(0);
	double x2 = x.get(1);
	return DoubleArray.of(x1 * x1 * x1 + x2 * x2, Math.Exp(1.5 * x1), Math.Log(2d * x1) * Math.Sin(x2));
		};
		double[][] keys = new double[][]
		{
			new double[] {1.5, -2.1},
			new double[] {2.3445, 0.5}
		};
		System.Func<DoubleArray, DoubleMatrix> funcExp = (DoubleArray x) =>
		{
	double x1 = x.get(0);
	double x2 = x.get(1);
	double[][] mat = new double[][]
	{
		new double[] {6d * x1, 2.25 * Math.Exp(1.5 * x1), -Math.Sin(x2) / x1 / x1},
		new double[] {2d, 0d, -Math.Log(2d * x1) * Math.Sin(x2)}
	};
	return DoubleMatrix.ofUnsafe(mat);
		};
		VectorFieldSecondOrderDifferentiator fd = new VectorFieldSecondOrderDifferentiator();
		System.Func<DoubleArray, DoubleMatrix> fdFuncs = fd.differentiateNoCross(func);
		foreach (double[] key in keys)
		{
		  DoubleMatrix cmp = fdFuncs(DoubleArray.ofUnsafe(key));
		  DoubleMatrix exp = funcExp(DoubleArray.ofUnsafe(key));
		  assertTrue(DoubleArrayMath.fuzzyEquals(cmp.column(0).toArray(), exp.row(0).toArray(), 1.0e-5));
		  assertTrue(DoubleArrayMath.fuzzyEquals(cmp.column(1).toArray(), exp.row(1).toArray(), 1.0e-5));
		}
	  }

	}

}