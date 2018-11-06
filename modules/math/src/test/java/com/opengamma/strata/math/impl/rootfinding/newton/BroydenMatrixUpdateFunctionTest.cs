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

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BroydenMatrixUpdateFunctionTest
	public class BroydenMatrixUpdateFunctionTest
	{
	  private static readonly BroydenMatrixUpdateFunction UPDATE = new BroydenMatrixUpdateFunction();
	  private static readonly DoubleArray V = DoubleArray.of(1, 2);
	  private static readonly DoubleMatrix M = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {3, 4},
		  new double[] {5, 6}
	  });
	  private static readonly System.Func<DoubleArray, DoubleMatrix> J = (final DoubleArray x) =>
	  {
  return M;
	  };

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