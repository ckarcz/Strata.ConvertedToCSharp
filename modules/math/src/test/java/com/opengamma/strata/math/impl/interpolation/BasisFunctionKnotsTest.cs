/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;
	using ArrayAsserts = org.testng.@internal.junit.ArrayAsserts;

	/// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BasisFunctionKnotsTest
	public class BasisFunctionKnotsTest
	{

	  private static readonly double[] KNOTS;
	  private static readonly double[] WRONG_ORDER_KNOTS;

	  static BasisFunctionKnotsTest()
	  {
		const int n = 10;
		KNOTS = new double[n + 1];

		for (int i = 0; i < n + 1; i++)
		{
		  KNOTS[i] = 0 + i * 1.0;
		}
		WRONG_ORDER_KNOTS = KNOTS.Clone();
		double a = WRONG_ORDER_KNOTS[6];
		WRONG_ORDER_KNOTS[6] = WRONG_ORDER_KNOTS[4];
		WRONG_ORDER_KNOTS[4] = a;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullKnots()
	  public virtual void testNullKnots()
	  {
		BasisFunctionKnots.fromKnots(null, 2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullInternalKnots()
	  public virtual void testNullInternalKnots()
	  {
		BasisFunctionKnots.fromInternalKnots(null, 2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegDegree()
	  public virtual void testNegDegree()
	  {
		BasisFunctionKnots.fromKnots(KNOTS, -1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegDegree2()
	  public virtual void testNegDegree2()
	  {
		BasisFunctionKnots.fromInternalKnots(KNOTS, -1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testWrongOrderUniform()
	  public virtual void testWrongOrderUniform()
	  {
		BasisFunctionKnots.fromUniform(2.0, 1.0, 10, 3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testWrongOrderKnots()
	  public virtual void testWrongOrderKnots()
	  {
		BasisFunctionKnots.fromKnots(WRONG_ORDER_KNOTS, 3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testWrongOrderInternalKnots()
	  public virtual void testWrongOrderInternalKnots()
	  {
		BasisFunctionKnots.fromInternalKnots(WRONG_ORDER_KNOTS, 3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDegreeToHigh1()
	  public virtual void testDegreeToHigh1()
	  {
		BasisFunctionKnots.fromUniform(0.0, 10.0, 11, 11);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDegreeToHigh2()
	  public virtual void testDegreeToHigh2()
	  {
		BasisFunctionKnots.fromInternalKnots(KNOTS, 11);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDegreeToHigh3()
	  public virtual void testDegreeToHigh3()
	  {
		BasisFunctionKnots.fromKnots(KNOTS, 11);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testUniform()
	  public virtual void testUniform()
	  {
		BasisFunctionKnots knots = BasisFunctionKnots.fromUniform(1.0, 2.0, 10, 3);
		assertEquals(3, knots.Degree);
		assertEquals(16, knots.NumKnots);
		assertEquals(12, knots.NumSplines);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testInternalKnots()
	  public virtual void testInternalKnots()
	  {
		BasisFunctionKnots knots = BasisFunctionKnots.fromInternalKnots(KNOTS, 2);
		assertEquals(2, knots.Degree);
		assertEquals(15, knots.NumKnots);
		assertEquals(12, knots.NumSplines);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testKnots()
	  public virtual void testKnots()
	  {
		BasisFunctionKnots knots = BasisFunctionKnots.fromKnots(KNOTS, 3);
		assertEquals(3, knots.Degree);
		assertEquals(11, knots.NumKnots);
		assertEquals(7, knots.NumSplines);
		ArrayAsserts.assertArrayEquals(KNOTS, knots.Knots, 1e-15);
	  }

	}

}