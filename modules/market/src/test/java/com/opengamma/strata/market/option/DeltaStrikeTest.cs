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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="DeltaStrike"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DeltaStrikeTest
	public class DeltaStrikeTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		DeltaStrike test = DeltaStrike.of(0.6d);
		assertEquals(test.Type, StrikeType.DELTA);
		assertEquals(test.Value, 0.6d, 0d);
		assertEquals(test.Label, "Delta=0.6");
		assertEquals(test.withValue(0.2d), DeltaStrike.of(0.2d));
	  }

	  public virtual void test_of_invalid()
	  {
		assertThrowsIllegalArg(() => DeltaStrike.of(-0.001d));
		assertThrowsIllegalArg(() => DeltaStrike.of(1.0001d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DeltaStrike test = DeltaStrike.of(0.6d);
		coverImmutableBean(test);
		DeltaStrike test2 = DeltaStrike.of(0.2d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		DeltaStrike test = DeltaStrike.of(0.6d);
		assertSerialization(test);
	  }

	}

}