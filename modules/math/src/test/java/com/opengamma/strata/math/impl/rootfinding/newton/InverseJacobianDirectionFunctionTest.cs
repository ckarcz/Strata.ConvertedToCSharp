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
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InverseJacobianDirectionFunctionTest
	public class InverseJacobianDirectionFunctionTest
	{
	  private static readonly MatrixAlgebra ALGEBRA = new OGMatrixAlgebra();
	  private static readonly InverseJacobianDirectionFunction F = new InverseJacobianDirectionFunction(ALGEBRA);
	  private const double X0 = 2.4;
	  private const double X1 = 7.6;
	  private const double X2 = 4.5;
	  private static readonly DoubleMatrix M = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {X0, 0, 0},
		  new double[] {0, X1, 0},
		  new double[] {0, 0, X2}
	  });
	  private static readonly DoubleArray Y = DoubleArray.of(1, 1, 1);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull()
	  public virtual void testNull()
	  {
		new InverseJacobianDirectionFunction(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullEstimate()
	  public virtual void testNullEstimate()
	  {
		F.getDirection(null, Y);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullY()
	  public virtual void testNullY()
	  {
		F.getDirection(M, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		double eps = 1e-9;
		DoubleArray direction = F.getDirection(M, Y);
		assertEquals(direction.get(0), X0, eps);
		assertEquals(direction.get(1), X1, eps);
		assertEquals(direction.get(2), X2, eps);
	  }
	}

}