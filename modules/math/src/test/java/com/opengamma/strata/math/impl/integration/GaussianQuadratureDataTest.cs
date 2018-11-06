/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.@internal.junit.ArrayAsserts.assertArrayEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GaussianQuadratureDataTest
	public class GaussianQuadratureDataTest
	{
	  private static readonly double[] X = new double[] {1, 2, 3, 4};
	  private static readonly double[] W = new double[] {6, 7, 8, 9};
	  private static readonly GaussianQuadratureData F = new GaussianQuadratureData(X, W);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullAbscissas()
	  public virtual void testNullAbscissas()
	  {
		new GaussianQuadratureData(null, W);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullWeights()
	  public virtual void testNullWeights()
	  {
		new GaussianQuadratureData(X, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testWrongLength()
	  public virtual void testWrongLength()
	  {
		new GaussianQuadratureData(X, new double[] {1, 2, 3});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		GaussianQuadratureData other = new GaussianQuadratureData(X, W);
		assertEquals(F, other);
		assertEquals(F.GetHashCode(), other.GetHashCode());
		other = new GaussianQuadratureData(W, W);
		assertFalse(F.Equals(other));
		other = new GaussianQuadratureData(X, X);
		assertFalse(F.Equals(other));
		assertArrayEquals(F.Abscissas, X, 0);
		assertArrayEquals(F.Weights, W, 0);
	  }
	}

}