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
	/// Test <seealso cref="MoneynessStrike"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MoneynessStrikeTest
	public class MoneynessStrikeTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		MoneynessStrike test = MoneynessStrike.of(0.6d);
		assertEquals(test.Type, StrikeType.MONEYNESS);
		assertEquals(test.Value, 0.6d, 0d);
		assertEquals(test.Label, "Moneyness=0.6");
		assertEquals(test.withValue(0.2d), MoneynessStrike.of(0.2d));
	  }

	  public virtual void test_ofStrikeAndForward()
	  {
		MoneynessStrike test = MoneynessStrike.ofStrikeAndForward(0.6d, 1.2d);
		assertEquals(test.Type, StrikeType.MONEYNESS);
		assertEquals(test.Value, 0.5d, 0d);
		assertEquals(test.Label, "Moneyness=0.5");
		assertEquals(test.withValue(0.2d), MoneynessStrike.of(0.2d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		MoneynessStrike test = MoneynessStrike.of(0.6d);
		coverImmutableBean(test);
		MoneynessStrike test2 = MoneynessStrike.of(0.2d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		MoneynessStrike test = MoneynessStrike.of(0.6d);
		assertSerialization(test);
	  }

	}

}