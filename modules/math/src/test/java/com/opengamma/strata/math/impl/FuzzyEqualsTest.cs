/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */

namespace com.opengamma.strata.math.impl
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;

	// NOTE: this is from OG-Maths

	/// <summary>
	/// Tests for values being equal allowing for a level of floating point fuzz
	/// Based on the OG-Maths C++ fuzzy equals test code.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FuzzyEqualsTest
	public class FuzzyEqualsTest
	{

	  internal virtual double NaN
	  {
		  get
		  {
			return Double.longBitsToDouble(0x7FF1010101010101L);
		  }
	  }

	  internal virtual double PosInf
	  {
		  get
		  {
			return Double.longBitsToDouble(0x7FF0000000000000L);
		  }
	  }

	  internal virtual double NegInf
	  {
		  get
		  {
			return Double.longBitsToDouble(0xFFF0000000000000L);
		  }
	  }

	  internal virtual double NegZero
	  {
		  get
		  {
			return Double.longBitsToDouble(8000000000000000L);
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void EqualsTest_SingleValueFuzzyEqualsDouble()
	  public virtual void EqualsTest_SingleValueFuzzyEqualsDouble()
	  {
		double NaN = NaN;
		double pinf = PosInf;
		double ninf = NegInf;
		double neg0 = NegZero;

		// NaN branch
		assertFalse(FuzzyEquals.SingleValueFuzzyEquals(NaN, NaN));
		assertFalse(FuzzyEquals.SingleValueFuzzyEquals(NaN, 1));
		assertFalse(FuzzyEquals.SingleValueFuzzyEquals(1, NaN));

		// Inf branches
		assertTrue(FuzzyEquals.SingleValueFuzzyEquals(pinf, pinf));
		assertTrue(FuzzyEquals.SingleValueFuzzyEquals(ninf, ninf));
		assertFalse(FuzzyEquals.SingleValueFuzzyEquals(pinf, ninf));
		assertFalse(FuzzyEquals.SingleValueFuzzyEquals(ninf, pinf));
		assertFalse(FuzzyEquals.SingleValueFuzzyEquals(pinf, double.MaxValue));
		assertFalse(FuzzyEquals.SingleValueFuzzyEquals(ninf, -double.MaxValue));

		// val 0 branches
		assertTrue(FuzzyEquals.SingleValueFuzzyEquals(0.e0, 0.e0));
		assertTrue(FuzzyEquals.SingleValueFuzzyEquals(0.e0, neg0));
		assertTrue(FuzzyEquals.SingleValueFuzzyEquals(neg0, 0.e0));
		assertTrue(FuzzyEquals.SingleValueFuzzyEquals(neg0, neg0));

		// same value as it trips the return true on "difference less than abs tol" branch
		assertTrue(FuzzyEquals.SingleValueFuzzyEquals(FuzzyEquals.Eps, 2.e0 * FuzzyEquals.Eps));

		// same value as it trips the return true on "difference less than relative error" branch
		assertTrue(FuzzyEquals.SingleValueFuzzyEquals(1.e308, 9.99999999999999e0 * 1.e307));

		// fail, just plain different
		assertFalse(FuzzyEquals.SingleValueFuzzyEquals(1.e0, 2.e0));

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void EqualsTest_ArrayFuzzyEqualsDouble()
	  public virtual void EqualsTest_ArrayFuzzyEqualsDouble()
	  {

		double[] data = new double[] {1.0e0, 2.0e0, 3.0e0, 4.0e0};
		double[] same = new double[] {1.0e0, 2.0e0, 3.0e0, 4.0e0};
		double[] diff = new double[] {-1.0e0, 2.0e0, 3.0e0, 4.0e0};
		double[] lendiff = new double[] {-1.0e0, 2.0e0, 3.0e0};

		assertFalse(FuzzyEquals.ArrayFuzzyEquals(data, lendiff));
		assertFalse(FuzzyEquals.ArrayFuzzyEquals(data, diff));
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(data, same));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void EqualsTest_ArrayOfArraysFuzzyEqualsDouble()
	  public virtual void EqualsTest_ArrayOfArraysFuzzyEqualsDouble()
	  {

		double[][] data = new double[][]
		{
			new double[] {1.0e0, 2.0e0, 3.0e0, 4.0e0},
			new double[] {5.e0, 6.e0, 7.e0, 8.e0},
			new double[] {9.e0, 10.e0, 11.e0, 12.e0}
		};
		double[][] same = new double[][]
		{
			new double[] {1.0e0, 2.0e0, 3.0e0, 4.0e0},
			new double[] {5.e0, 6.e0, 7.e0, 8.e0},
			new double[] {9.e0, 10.e0, 11.e0, 12.e0}
		};
		double[][] diffvalue = new double[][]
		{
			new double[] {-1.0e0, 2.0e0, 3.0e0, 4.0e0},
			new double[] {5.e0, 6.e0, 7.e0, 8.e0},
			new double[] {9.e0, 10.e0, 11.e0, 12.e0}
		};
		double[][] diffrowlen = new double[][]
		{
			new double[] {1.0e0, 2.0e0, 3.0e0, 4.0e0},
			new double[] {5.e0, 6.e0, 7.e0},
			new double[] {9.e0, 10.e0, 11.e0, 12.e0}
		};
		double[][] diffrowcount = new double[][]
		{
			new double[] {1.0e0, 2.0e0, 3.0e0, 4.0e0},
			new double[] {5.e0, 6.e0, 7.e0, 8.e0}
		};

		assertFalse(FuzzyEquals.ArrayFuzzyEquals(data, diffvalue));
		assertFalse(FuzzyEquals.ArrayFuzzyEquals(data, diffrowlen));
		assertFalse(FuzzyEquals.ArrayFuzzyEquals(data, diffrowcount));
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(data, same));

		assertFalse(FuzzyEquals.ArrayFuzzyEquals(data, diffvalue, FuzzyEquals.DefaultTolerance, FuzzyEquals.DefaultTolerance));
		assertFalse(FuzzyEquals.ArrayFuzzyEquals(data, diffrowlen, FuzzyEquals.DefaultTolerance, FuzzyEquals.DefaultTolerance));
		assertFalse(FuzzyEquals.ArrayFuzzyEquals(data, diffrowcount, FuzzyEquals.DefaultTolerance, FuzzyEquals.DefaultTolerance));
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(data, same, FuzzyEquals.DefaultTolerance, FuzzyEquals.DefaultTolerance));

		// same value as it trips the return true on "difference less than abs tol" branch
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(new double[][]
		{
			new double[] {FuzzyEquals.Eps}
		},
		new double[][]
		{
			new double[] {2.e0 * FuzzyEquals.Eps}
		},
		FuzzyEquals.DefaultTolerance, FuzzyEquals.DefaultTolerance));

		// same value as it trips the return true on "difference less than relative error" branch
		assertTrue(FuzzyEquals.ArrayFuzzyEquals(new double[][]
		{
			new double[] {1.e308}
		},
		new double[][]
		{
			new double[] {9.99999999999999e0 * 1.e307}
		},
		FuzzyEquals.DefaultTolerance, FuzzyEquals.DefaultTolerance));

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void EqualsTest_CheckEPSIsAppropriatelySmall()
	  public virtual void EqualsTest_CheckEPSIsAppropriatelySmall()
	  {
		assertTrue(FuzzyEquals.Eps < 5e-16);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void EqualsTest_CheckDefaultToleranceAppropriatelySmall()
	  public virtual void EqualsTest_CheckDefaultToleranceAppropriatelySmall()
	  {
		assertTrue(FuzzyEquals.Eps < 10 * 5e-16);
	  }

	}

}