/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.option
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
	/// Test <seealso cref="SimpleStrike"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SimpleStrikeTest
	public class SimpleStrikeTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		SimpleStrike test = SimpleStrike.of(0.6d);
		assertEquals(test.Type, StrikeType.STRIKE);
		assertEquals(test.Value, 0.6d, 0d);
		assertEquals(test.Label, "Strike=0.6");
		assertEquals(test.withValue(0.2d), SimpleStrike.of(0.2d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SimpleStrike test = SimpleStrike.of(0.6d);
		coverImmutableBean(test);
		SimpleStrike test2 = SimpleStrike.of(0.2d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		SimpleStrike test = SimpleStrike.of(0.6d);
		assertSerialization(test);
	  }

	}

}