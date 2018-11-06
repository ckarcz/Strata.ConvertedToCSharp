/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ShiftType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ShiftTypeTest
	public class ShiftTypeTest
	{

	  public virtual void test_applyShift()
	  {
		assertEquals(ShiftType.ABSOLUTE.applyShift(2, 0.1), 2.1);
		assertEquals(ShiftType.RELATIVE.applyShift(2, 0.1), 2.2);
		assertEquals(ShiftType.SCALED.applyShift(2, 1.1), 2.2);
	  }

	  public virtual void test_toValueAdjustment()
	  {
		assertEquals(ShiftType.ABSOLUTE.toValueAdjustment(0.1).adjust(2), 2.1);
		assertEquals(ShiftType.RELATIVE.toValueAdjustment(0.1).adjust(2), 2.2);
		assertEquals(ShiftType.SCALED.toValueAdjustment(1.1).adjust(2), 2.2);
	  }

	  public virtual void test_computeShift()
	  {
		double tol = 1.0e-15;
		double @base = 2.0;
		double shifted = 2.1;
		assertEquals(ShiftType.ABSOLUTE.computeShift(@base, shifted), 0.1, tol);
		assertEquals(ShiftType.RELATIVE.computeShift(@base, shifted), 0.05, tol);
		assertEquals(ShiftType.SCALED.computeShift(@base, shifted), 1.05, tol);
		assertEquals(ShiftType.ABSOLUTE.applyShift(@base, ShiftType.ABSOLUTE.computeShift(@base, shifted)), shifted, tol);
		assertEquals(ShiftType.RELATIVE.applyShift(@base, ShiftType.RELATIVE.computeShift(@base, shifted)), shifted, tol);
		assertEquals(ShiftType.SCALED.applyShift(@base, ShiftType.SCALED.computeShift(@base, shifted)), shifted, tol);
	  }

	  public virtual void test_name()
	  {
		assertEquals(ShiftType.ABSOLUTE.name(), "ABSOLUTE");
		assertEquals(ShiftType.RELATIVE.name(), "RELATIVE");
		assertEquals(ShiftType.SCALED.name(), "SCALED");
	  }

	  public virtual void test_toString()
	  {
		assertEquals(ShiftType.ABSOLUTE.ToString(), "Absolute");
		assertEquals(ShiftType.RELATIVE.ToString(), "Relative");
		assertEquals(ShiftType.SCALED.ToString(), "Scaled");
	  }

	  public virtual void coverage()
	  {
		coverEnum(typeof(ShiftType));
	  }

	}

}