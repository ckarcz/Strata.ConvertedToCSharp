/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.option
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="SimpleConstantContinuousBarrier"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SimpleConstantContinuousBarrierTest
	public class SimpleConstantContinuousBarrierTest
	{

	  public virtual void test_of()
	  {
		double level = 1.5;
		SimpleConstantContinuousBarrier test = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, level);
		assertEquals(test.BarrierLevel, level);
		assertEquals(test.getBarrierLevel(LocalDate.of(2015, 1, 21)), level);
		assertEquals(test.BarrierType, BarrierType.DOWN);
		assertEquals(test.KnockType, KnockType.KNOCK_IN);
	  }

	  public virtual void test_inverseKnockType()
	  {
		double level = 1.5;
		SimpleConstantContinuousBarrier @base = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, level);
		SimpleConstantContinuousBarrier test = @base.inverseKnockType();
		SimpleConstantContinuousBarrier expected = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_OUT, level);
		assertEquals(test, expected);
		assertEquals(test.inverseKnockType(), @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SimpleConstantContinuousBarrier test1 = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, 1.5);
		SimpleConstantContinuousBarrier test2 = SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_OUT, 2.1);
		coverImmutableBean(test1);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		SimpleConstantContinuousBarrier test = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, 1.5);
		assertSerialization(test);
	  }

	}

}