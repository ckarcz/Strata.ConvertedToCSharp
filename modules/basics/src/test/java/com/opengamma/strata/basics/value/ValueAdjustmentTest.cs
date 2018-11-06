/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ValueAdjustment"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ValueAdjustmentTest
	public class ValueAdjustmentTest
	{

	  private static double TOLERANCE = 0.0001d;

	  public virtual void test_NONE()
	  {
		ValueAdjustment test = ValueAdjustment.NONE;
		assertEquals(test.ModifyingValue, 0, TOLERANCE);
		assertEquals(test.Type, ValueAdjustmentType.DELTA_AMOUNT);
		assertEquals(test.adjust(100), 100, TOLERANCE);
		assertEquals(test.ToString(), "ValueAdjustment[result = input]");
	  }

	  public virtual void test_ofReplace()
	  {
		ValueAdjustment test = ValueAdjustment.ofReplace(200);
		assertEquals(test.ModifyingValue, 200, TOLERANCE);
		assertEquals(test.Type, ValueAdjustmentType.REPLACE);
		assertEquals(test.adjust(100), 200, TOLERANCE);
		assertEquals(test.ToString(), "ValueAdjustment[result = 200.0]");
	  }

	  public virtual void test_ofDeltaAmount()
	  {
		ValueAdjustment test = ValueAdjustment.ofDeltaAmount(20);
		assertEquals(test.ModifyingValue, 20, TOLERANCE);
		assertEquals(test.Type, ValueAdjustmentType.DELTA_AMOUNT);
		assertEquals(test.adjust(100), 120, TOLERANCE);
		assertEquals(test.ToString(), "ValueAdjustment[result = input + 20.0]");
	  }

	  public virtual void test_ofDeltaMultiplier()
	  {
		ValueAdjustment test = ValueAdjustment.ofDeltaMultiplier(0.1);
		assertEquals(test.ModifyingValue, 0.1, TOLERANCE);
		assertEquals(test.Type, ValueAdjustmentType.DELTA_MULTIPLIER);
		assertEquals(test.adjust(100), 110, TOLERANCE);
		assertEquals(test.ToString(), "ValueAdjustment[result = input + input * 0.1]");
	  }

	  public virtual void test_ofMultiplier()
	  {
		ValueAdjustment test = ValueAdjustment.ofMultiplier(1.1);
		assertEquals(test.ModifyingValue, 1.1, TOLERANCE);
		assertEquals(test.Type, ValueAdjustmentType.MULTIPLIER);
		assertEquals(test.adjust(100), 110, TOLERANCE);
		assertEquals(test.ToString(), "ValueAdjustment[result = input * 1.1]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void Equals()
	  {
		ValueAdjustment a1 = ValueAdjustment.ofReplace(200);
		ValueAdjustment a2 = ValueAdjustment.ofReplace(200);
		ValueAdjustment b = ValueAdjustment.ofDeltaMultiplier(200);
		ValueAdjustment c = ValueAdjustment.ofDeltaMultiplier(0.1);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(ValueAdjustment.ofReplace(200));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(ValueAdjustment.ofReplace(200));
	  }

	}

}