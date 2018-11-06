using System;

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
	/// Test <seealso cref="LogMoneynessStrike"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LogMoneynessStrikeTest
	public class LogMoneynessStrikeTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		LogMoneynessStrike test = LogMoneynessStrike.of(0.6d);
		assertEquals(test.Type, StrikeType.LOG_MONEYNESS);
		assertEquals(test.Value, 0.6d, 0d);
		assertEquals(test.Label, "LogMoneyness=0.6");
		assertEquals(test.withValue(0.2d), LogMoneynessStrike.of(0.2d));
	  }

	  public virtual void test_ofStrikeAndForward()
	  {
		LogMoneynessStrike test = LogMoneynessStrike.ofStrikeAndForward(0.6d, 1.2d);
		assertEquals(test.Type, StrikeType.LOG_MONEYNESS);
		assertEquals(test.Value, Math.Log(0.5d), 0d);
		assertEquals(test.Label, "LogMoneyness=" + Math.Log(0.5d));
		assertEquals(test.withValue(0.2d), LogMoneynessStrike.of(0.2d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		LogMoneynessStrike test = LogMoneynessStrike.of(0.6d);
		coverImmutableBean(test);
		LogMoneynessStrike test2 = LogMoneynessStrike.of(0.2d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		LogMoneynessStrike test = LogMoneynessStrike.of(0.6d);
		assertSerialization(test);
	  }

	}

}