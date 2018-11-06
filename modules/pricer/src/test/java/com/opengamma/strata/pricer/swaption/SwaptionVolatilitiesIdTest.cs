/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
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
	/// Test <seealso cref="SwaptionVolatilitiesId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwaptionVolatilitiesIdTest
	public class SwaptionVolatilitiesIdTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		SwaptionVolatilitiesId test = SwaptionVolatilitiesId.of("Foo");
		assertEquals(test.Name, SwaptionVolatilitiesName.of("Foo"));
		assertEquals(test.MarketDataType, typeof(SwaptionVolatilities));
		assertEquals(test.MarketDataName, SwaptionVolatilitiesName.of("Foo"));
		assertEquals(test.ToString(), "SwaptionVolatilitiesId:Foo");
	  }

	  public virtual void test_of_object()
	  {
		SwaptionVolatilitiesId test = SwaptionVolatilitiesId.of(SwaptionVolatilitiesName.of("Foo"));
		assertEquals(test.Name, SwaptionVolatilitiesName.of("Foo"));
		assertEquals(test.MarketDataType, typeof(SwaptionVolatilities));
		assertEquals(test.MarketDataName, SwaptionVolatilitiesName.of("Foo"));
		assertEquals(test.ToString(), "SwaptionVolatilitiesId:Foo");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SwaptionVolatilitiesId test = SwaptionVolatilitiesId.of("Foo");
		coverImmutableBean(test);
		SwaptionVolatilitiesId test2 = SwaptionVolatilitiesId.of("Bar");
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		SwaptionVolatilitiesId test = SwaptionVolatilitiesId.of("Foo");
		assertSerialization(test);
	  }

	}

}