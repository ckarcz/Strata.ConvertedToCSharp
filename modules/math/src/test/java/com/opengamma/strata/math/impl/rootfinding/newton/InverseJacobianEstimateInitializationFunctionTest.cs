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
	using DecompositionFactory = com.opengamma.strata.math.impl.linearalgebra.DecompositionFactory;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InverseJacobianEstimateInitializationFunctionTest
	public class InverseJacobianEstimateInitializationFunctionTest
	{

	  private static readonly MatrixAlgebra ALGEBRA = new CommonsMatrixAlgebra();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.math.linearalgebra.Decomposition<?> SV = com.opengamma.strata.math.impl.linearalgebra.DecompositionFactory.SV_COMMONS;
	  private static readonly Decomposition<object> SV = DecompositionFactory.SV_COMMONS;
	  private static readonly InverseJacobianEstimateInitializationFunction ESTIMATE = new InverseJacobianEstimateInitializationFunction(SV);
	  private static readonly System.Func<DoubleArray, DoubleMatrix> J = (DoubleArray v) =>
	  {

  double[] x = v.toArray();
  return DoubleMatrix.copyOf(new double[][]
  {
	  new double[] {x[0] * x[0], x[0] * x[1]},
	  new double[] {x[0] - x[1], x[1] * x[1]}
  });

	  };
	  private static readonly DoubleArray X = DoubleArray.of(3, 4);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDecomposition()
	  public virtual void testNullDecomposition()
	  {
		new InverseJacobianEstimateInitializationFunction(null);
	  }

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

	  public virtual void test()
	  {
		DoubleMatrix m1 = ESTIMATE.getInitializedMatrix(J, X);
		DoubleMatrix m2 = J.apply(X);
		DoubleMatrix m3 = (DoubleMatrix)(ALGEBRA.multiply(m1, m2));
		DoubleMatrix identity = DoubleMatrix.identity(2);
		for (int i = 0; i < 2; i++)
		{
		  for (int j = 0; j < 2; j++)
		  {
			assertEquals(m3.get(i, j), identity.get(i, j), 1e-6);
		  }
		}
	  }

	}

}