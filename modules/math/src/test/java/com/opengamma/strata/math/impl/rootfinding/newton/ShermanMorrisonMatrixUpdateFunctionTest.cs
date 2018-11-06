/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
{

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ShermanMorrisonMatrixUpdateFunctionTest
	public class ShermanMorrisonMatrixUpdateFunctionTest
	{
	  private static readonly MatrixAlgebra ALGEBRA = new OGMatrixAlgebra();
	  private static readonly ShermanMorrisonMatrixUpdateFunction UPDATE = new ShermanMorrisonMatrixUpdateFunction(ALGEBRA);
	  private static readonly DoubleArray V = DoubleArray.of(1, 2);
	  private static readonly DoubleMatrix M = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {3, 4},
		  new double[] {5, 6}
	  });
	  private static readonly System.Func<DoubleArray, DoubleMatrix> J = new FuncAnonymousInnerClass();

	  private class FuncAnonymousInnerClass : System.Func<DoubleArray, DoubleMatrix>
	  {
		  public FuncAnonymousInnerClass()
		  {
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix apply(DoubleArray x)
		  {
			return ALGEBRA.getOuterProduct(x, x);
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull()
	  public virtual void testNull()
	  {
		new ShermanMorrisonMatrixUpdateFunction(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDeltaX()
	  public virtual void testNullDeltaX()
	  {
		UPDATE.getUpdatedMatrix(J, V, null, V, M);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDeltaY()
	  public virtual void testNullDeltaY()
	  {
		UPDATE.getUpdatedMatrix(J, V, V, null, M);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullMatrix()
	  public virtual void testNullMatrix()
	  {
		UPDATE.getUpdatedMatrix(J, V, V, V, null);
	  }
	}

}