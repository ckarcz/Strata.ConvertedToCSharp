/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.matrix
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertNull;

	using Test = org.testng.annotations.Test;

	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Matrix = com.opengamma.strata.collect.array.Matrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MatrixAlgebraFactoryTest
	public class MatrixAlgebraFactoryTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testBadName()
		public virtual void testBadName()
		{
		MatrixAlgebraFactory.getMatrixAlgebra("X");
		}

	  public virtual void testBadClass()
	  {
		assertNull(MatrixAlgebraFactory.getMatrixAlgebraName(new MatrixAlgebraAnonymousInnerClass(this)));
	  }

	  private class MatrixAlgebraAnonymousInnerClass : MatrixAlgebra
	  {
		  private readonly MatrixAlgebraFactoryTest outerInstance;

		  public MatrixAlgebraAnonymousInnerClass(MatrixAlgebraFactoryTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


		  public override double getCondition(Matrix m)
		  {
			return 0;
		  }

		  public override double getDeterminant(Matrix m)
		  {
			return 0;
		  }

		  public override double getInnerProduct(Matrix m1, Matrix m2)
		  {
			return 0;
		  }

		  public override DoubleMatrix getInverse(Matrix m)
		  {
			return null;
		  }

		  public override double getNorm1(Matrix m)
		  {
			return 0;
		  }

		  public override double getNorm2(Matrix m)
		  {
			return 0;
		  }

		  public override double getNormInfinity(Matrix m)
		  {
			return 0;
		  }

		  public override DoubleMatrix getOuterProduct(Matrix m1, Matrix m2)
		  {
			return null;
		  }

		  public override DoubleMatrix getPower(Matrix m, int p)
		  {
			return null;
		  }

		  public override DoubleMatrix getPower(Matrix m, double p)
		  {
			return null;
		  }

		  public override double getTrace(Matrix m)
		  {
			return 0;
		  }

		  public override DoubleMatrix getTranspose(Matrix m)
		  {
			return null;
		  }

		  public override Matrix multiply(Matrix m1, Matrix m2)
		  {
			return null;
		  }

	  }

	  public virtual void test()
	  {
		assertEquals(MatrixAlgebraFactory.getMatrixAlgebra(MatrixAlgebraFactory.COMMONS), MatrixAlgebraFactory.COMMONS_ALGEBRA);
		assertEquals(MatrixAlgebraFactory.getMatrixAlgebra(MatrixAlgebraFactory.OG), MatrixAlgebraFactory.OG_ALGEBRA);
		assertEquals(MatrixAlgebraFactory.getMatrixAlgebraName(MatrixAlgebraFactory.COMMONS_ALGEBRA), MatrixAlgebraFactory.COMMONS);
		assertEquals(MatrixAlgebraFactory.getMatrixAlgebraName(MatrixAlgebraFactory.OG_ALGEBRA), MatrixAlgebraFactory.OG);
	  }

	}

}