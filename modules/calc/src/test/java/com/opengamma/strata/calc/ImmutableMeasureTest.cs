/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;

	using Test = org.testng.annotations.Test;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ImmutableMeasureTest
	public class ImmutableMeasureTest
	{

	  /// <summary>
	  /// Tests that measure names are validated
	  /// </summary>
	  public virtual void namePattern()
	  {
		assertThrows(() => ImmutableMeasure.of(null), typeof(System.ArgumentException));
		assertThrows(() => ImmutableMeasure.of(""), typeof(System.ArgumentException));
		assertThrows(() => ImmutableMeasure.of("Foo Bar"), typeof(System.ArgumentException), ".*must only contain the characters.*");
		assertThrows(() => ImmutableMeasure.of("Foo_Bar"), typeof(System.ArgumentException), ".*must only contain the characters.*");
		assertThrows(() => ImmutableMeasure.of("FooBar!"), typeof(System.ArgumentException), ".*must only contain the characters.*");

		// These should execute without throwing an exception
		ImmutableMeasure.of("FooBar");
		ImmutableMeasure.of("Foo-Bar");
		ImmutableMeasure.of("123");
		ImmutableMeasure.of("FooBar123");
	  }
	}

}