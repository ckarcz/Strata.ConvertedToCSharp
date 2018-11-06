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
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class JacobianDirectionFunctionTest
	public class JacobianDirectionFunctionTest
	{

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.math.linearalgebra.Decomposition<?> SV = com.opengamma.strata.math.impl.linearalgebra.DecompositionFactory.SV_COMMONS;
	  private static readonly Decomposition<object> SV = DecompositionFactory.SV_COMMONS;
	  private static readonly JacobianDirectionFunction F = new JacobianDirectionFunction(SV);
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
		new JacobianDirectionFunction(null);
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

	  public virtual void test()
	  {
		double eps = 1e-9;
		DoubleArray direction = F.getDirection(M, Y);
		assertEquals(direction.get(0), 1.0 / X0, eps);
		assertEquals(direction.get(1), 1.0 / X1, eps);
		assertEquals(direction.get(2), 1.0 / X2, eps);
	  }

	}

}