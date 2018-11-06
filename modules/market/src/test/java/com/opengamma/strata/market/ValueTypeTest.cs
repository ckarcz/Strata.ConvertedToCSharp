/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ValueType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ValueTypeTest
	public class ValueTypeTest
	{

	  public virtual void test_validation()
	  {
		assertThrows(() => ValueType.of(null), typeof(System.ArgumentException));
		assertThrows(() => ValueType.of(""), typeof(System.ArgumentException));
		assertThrows(() => ValueType.of("Foo Bar"), typeof(System.ArgumentException), ".*must only contain the characters.*");
		assertThrows(() => ValueType.of("Foo_Bar"), typeof(System.ArgumentException), ".*must only contain the characters.*");
		assertThrows(() => ValueType.of("FooBar!"), typeof(System.ArgumentException), ".*must only contain the characters.*");

		// these should execute without throwing an exception
		ValueType.of("FooBar");
		ValueType.of("Foo-Bar");
		ValueType.of("123");
		ValueType.of("FooBar123");
	  }

	  //-----------------------------------------------------------------------
	  public virtual void checkEquals()
	  {
		ValueType test = ValueType.of("Foo");
		test.checkEquals(test, "Error");
		assertThrowsIllegalArg(() => test.checkEquals(ValueType.PRICE_INDEX, "Error"));
	  }

	  //-----------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ValueType test = ValueType.of("Foo");
		assertEquals(test.ToString(), "Foo");
		assertSerialization(test);
		assertJodaConvert(typeof(ValueType), test);
	  }

	}

}