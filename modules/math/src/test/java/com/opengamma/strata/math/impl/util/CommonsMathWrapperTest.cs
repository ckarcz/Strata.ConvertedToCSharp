/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.util
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.@internal.junit.ArrayAsserts.assertArrayEquals;

	using UnivariateFunction = org.apache.commons.math3.analysis.UnivariateFunction;
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;
	using RealVector = org.apache.commons.math3.linear.RealVector;
	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test <seealso cref="CommonsMathWrapper"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CommonsMathWrapperTest
	public class CommonsMathWrapperTest
	{

	  private static readonly DoubleArray OG_VECTOR = DoubleArray.of(1, 2, 3);
	  private static readonly DoubleMatrix OG_MATRIX = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {1, 2, 3},
		  new double[] {4, 5, 6},
		  new double[] {7, 8, 9}
	  });
	  private static readonly System.Func<double, double> OG_FUNCTION_1D = (final double? x) =>
	  {
  return x * x + 7 * x + 12;

	  };

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull1DMatrix()
	  public virtual void testNull1DMatrix()
	  {
		CommonsMathWrapper.wrap((DoubleArray) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullVector()
	  public virtual void testNullVector()
	  {
		CommonsMathWrapper.unwrap((RealVector) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull1DFunction()
	  public virtual void testNull1DFunction()
	  {
		CommonsMathWrapper.wrapUnivariate((System.Func<double, double>) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullMatrix()
	  public virtual void testNullMatrix()
	  {
		CommonsMathWrapper.wrap((DoubleMatrix) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullRealMatrix()
	  public virtual void testNullRealMatrix()
	  {
		CommonsMathWrapper.unwrap((RealMatrix) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testVector()
	  public virtual void testVector()
	  {
		RealVector commons = CommonsMathWrapper.wrap(OG_VECTOR);
		assertEquals(CommonsMathWrapper.unwrap(commons), OG_VECTOR);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testVectorAsMatrix()
	  public virtual void testVectorAsMatrix()
	  {
		RealMatrix commons = CommonsMathWrapper.wrapAsMatrix(OG_VECTOR);
		double[][] data = commons.Data;
		assertEquals(data.Length, OG_VECTOR.size());
		assertEquals(data[0].Length, 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test1DFunction()
	  public virtual void test1DFunction()
	  {
		UnivariateFunction commons = CommonsMathWrapper.wrapUnivariate(OG_FUNCTION_1D);
		for (int i = 0; i < 100; i++)
		{
		  assertEquals(OG_FUNCTION_1D.apply((double) i), commons.value(i), 1e-15);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testMatrix()
	  public virtual void testMatrix()
	  {
		RealMatrix commons = CommonsMathWrapper.wrap(OG_MATRIX);
		double[][] unwrapped = CommonsMathWrapper.unwrap(commons).toArray();
		double[][] ogData = OG_MATRIX.toArray();
		int n = unwrapped.Length;
		assertEquals(n, ogData.Length);
		for (int i = 0; i < n; i++)
		{
		  assertArrayEquals(unwrapped[i], ogData[i], 1e-15);
		}
	  }

	}

}