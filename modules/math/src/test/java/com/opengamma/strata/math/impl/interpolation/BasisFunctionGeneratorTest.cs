using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;


	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BasisFunctionGeneratorTest
	public class BasisFunctionGeneratorTest
	{
	  private static readonly BasisFunctionGenerator GENERATOR = new BasisFunctionGenerator();
	  private static readonly double[] KNOTS;

	  static BasisFunctionGeneratorTest()
	  {
		const int n = 10;
		KNOTS = new double[n + 1];
		for (int i = 0; i < n + 1; i++)
		{
		  KNOTS[i] = 0 + i * 1.0;
		}

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testFunctionIndexOutOfRange1()
	  public virtual void testFunctionIndexOutOfRange1()
	  {
		BasisFunctionKnots k = BasisFunctionKnots.fromKnots(KNOTS, 2);
		GENERATOR.generate(k, -1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testFunctionIndexOutOfRange2()
	  public virtual void testFunctionIndexOutOfRange2()
	  {
		BasisFunctionKnots k = BasisFunctionKnots.fromKnots(KNOTS, 5);
		int nS = k.NumSplines;
		GENERATOR.generate(k, nS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testZeroOrder()
	  public virtual void testZeroOrder()
	  {
		BasisFunctionKnots knots = BasisFunctionKnots.fromInternalKnots(KNOTS, 0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> func = GENERATOR.generate(knots, 4);
		System.Func<double, double> func = GENERATOR.generate(knots, 4);
		assertEquals(0.0, func(3.5), 0.0);
		assertEquals(1.0, func(4.78), 0.0);
		assertEquals(1.0, func(4.0), 0.0);
		assertEquals(0.0, func(5.0), 0.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testFirstOrder()
	  public virtual void testFirstOrder()
	  {
		BasisFunctionKnots knots = BasisFunctionKnots.fromInternalKnots(KNOTS, 1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> func = GENERATOR.generate(knots, 3);
		System.Func<double, double> func = GENERATOR.generate(knots, 3);
		assertEquals(0.0, func(1.76), 0.0);
		assertEquals(1.0, func(3.0), 0.0);
		assertEquals(0, func(4.0), 0.0);
		assertEquals(0.5, func(2.5), 0.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testSecondOrder()
	  public virtual void testSecondOrder()
	  {
		BasisFunctionKnots knots = BasisFunctionKnots.fromInternalKnots(KNOTS, 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> func = GENERATOR.generate(knots, 3);
		System.Func<double, double> func = GENERATOR.generate(knots, 3);
		assertEquals(0.0, func(0.76), 0.0);
		assertEquals(0.125, func(1.5), 0.0);
		assertEquals(0.5, func(2.0), 0.0);
		assertEquals(0.75, func(2.5), 0.0);
		assertEquals(0.0, func(4.0), 0.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testThirdOrder()
	  public virtual void testThirdOrder()
	  {
		BasisFunctionKnots knots = BasisFunctionKnots.fromInternalKnots(KNOTS, 3);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> func = GENERATOR.generate(knots, 3);
		System.Func<double, double> func = GENERATOR.generate(knots, 3);
		assertEquals(0.0, func(-0.1), 0.0);
		assertEquals(1.0 / 6.0, func(1.0), 0.0);
		assertEquals(2.0 / 3.0, func(2.0), 0.0);
		assertEquals(1 / 48.0, func(3.5), 0.0);
		assertEquals(0.0, func(4.0), 0.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testTwoD()
	  public virtual void testTwoD()
	  {

		BasisFunctionKnots knots1 = BasisFunctionKnots.fromInternalKnots(KNOTS, 2);
		BasisFunctionKnots knots2 = BasisFunctionKnots.fromInternalKnots(KNOTS, 3);
		IList<System.Func<double[], double>> set = GENERATOR.generateSet(new BasisFunctionKnots[] {knots1, knots2});

		//pick of one of the basis functions for testing 
		int index = FunctionUtils.toTensorIndex(new int[] {3, 3}, new int[] {knots1.NumSplines, knots2.NumSplines});
		System.Func<double[], double> func = set[index];
		assertEquals(1.0 / 3.0, func(new double[] {2.0, 2.0}), 0.0);
		assertEquals(1.0 / 2.0, func(new double[] {2.5, 2.0}), 0.0);
		assertEquals(1.0 / 8.0 / 48.0, func(new double[] {1.5, 3.5}), 0.0);
		assertEquals(0.0, func(new double[] {4.0, 2.5}), 0.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testThreeD()
	  public virtual void testThreeD()
	  {
		BasisFunctionKnots knots1 = BasisFunctionKnots.fromInternalKnots(KNOTS, 2);
		BasisFunctionKnots knots2 = BasisFunctionKnots.fromInternalKnots(KNOTS, 3);
		BasisFunctionKnots knots3 = BasisFunctionKnots.fromInternalKnots(KNOTS, 1);
		IList<System.Func<double[], double>> set = GENERATOR.generateSet(new BasisFunctionKnots[] {knots1, knots2, knots3});

		//pick of one of the basis functions for testing 
		int index = FunctionUtils.toTensorIndex(new int[] {3, 3, 3}, new int[] {knots1.NumSplines, knots2.NumSplines, knots3.NumSplines});
		System.Func<double[], double> func = set[index];
		assertEquals(1.0 / 3.0, func(new double[] {2.0, 2.0, 3.0}), 0.0);
	  }

	}

}