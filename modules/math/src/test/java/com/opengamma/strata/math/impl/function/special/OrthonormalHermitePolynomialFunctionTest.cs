using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using CombinatoricsUtils = org.apache.commons.math3.util.CombinatoricsUtils;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OrthonormalHermitePolynomialFunctionTest
	public class OrthonormalHermitePolynomialFunctionTest
	{
	  private static readonly HermitePolynomialFunction HERMITE = new HermitePolynomialFunction();
	  private static readonly OrthonormalHermitePolynomialFunction ORTHONORMAL = new OrthonormalHermitePolynomialFunction();
	  private const double EPS = 1e-9;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testBadN()
	  public virtual void testBadN()
	  {
		ORTHONORMAL.getPolynomials(-3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		const int n = 15;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.DoubleFunction1D[] f1 = HERMITE.getPolynomials(n);
		DoubleFunction1D[] f1 = HERMITE.getPolynomials(n);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.DoubleFunction1D[] f2 = ORTHONORMAL.getPolynomials(n);
		DoubleFunction1D[] f2 = ORTHONORMAL.getPolynomials(n);
		const double x = 3.4;
		for (int i = 0; i < f1.Length; i++)
		{
		  assertEquals(f1[i].applyAsDouble(x) / Math.Sqrt(CombinatoricsUtils.factorialDouble(i) * Math.Pow(2, i) * Math.Sqrt(Math.PI)), f2[i].applyAsDouble(x), EPS);
		}
	  }

	}

}