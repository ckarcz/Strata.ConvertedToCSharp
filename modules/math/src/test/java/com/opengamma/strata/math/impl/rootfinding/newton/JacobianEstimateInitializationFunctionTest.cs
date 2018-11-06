/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
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
//ORIGINAL LINE: @Test public class JacobianEstimateInitializationFunctionTest
	public class JacobianEstimateInitializationFunctionTest
	{

	  private static readonly JacobianEstimateInitializationFunction ESTIMATE = new JacobianEstimateInitializationFunction();
	  private static readonly System.Func<DoubleArray, DoubleMatrix> J = (DoubleArray v) =>
	  {
  double[] x = v.toArray();
  return DoubleMatrix.copyOf(new double[][]
  {
	  new double[] {x[0] * x[0], x[0] * x[1]},
	  new double[] {x[0] - x[1], x[1] * x[1]}
  });
	  };

	  private static readonly DoubleArray X = DoubleArray.of(1, 2);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		ESTIMATE.getInitializedMatrix(null, X);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullVector()
	  public virtual void testNullVector()
	  {
		ESTIMATE.getInitializedMatrix(J, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		DoubleMatrix m1 = ESTIMATE.getInitializedMatrix(J, X);
		DoubleMatrix m2 = J.apply(X);
		for (int i = 0; i < 2; i++)
		{
		  for (int j = 0; j < 2; j++)
		  {
			assertEquals(m1.get(i, j), m2.get(i, j), 1e-9);
		  }
		}
	  }
	}

}