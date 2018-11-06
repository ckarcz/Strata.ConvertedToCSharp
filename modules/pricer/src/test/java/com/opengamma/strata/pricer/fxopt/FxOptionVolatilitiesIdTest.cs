/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
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
	/// Test <seealso cref="FxOptionVolatilitiesId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxOptionVolatilitiesIdTest
	public class FxOptionVolatilitiesIdTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		FxOptionVolatilitiesId test = FxOptionVolatilitiesId.of("Foo");
		assertEquals(test.Name, FxOptionVolatilitiesName.of("Foo"));
		assertEquals(test.MarketDataType, typeof(FxOptionVolatilities));
		assertEquals(test.MarketDataName, FxOptionVolatilitiesName.of("Foo"));
	  }

	  public virtual void test_of_object()
	  {
		FxOptionVolatilitiesId test = FxOptionVolatilitiesId.of(FxOptionVolatilitiesName.of("Foo"));
		assertEquals(test.Name, FxOptionVolatilitiesName.of("Foo"));
		assertEquals(test.MarketDataType, typeof(FxOptionVolatilities));
		assertEquals(test.MarketDataName, FxOptionVolatilitiesName.of("Foo"));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxOptionVolatilitiesId test = FxOptionVolatilitiesId.of("Foo");
		coverImmutableBean(test);
		FxOptionVolatilitiesId test2 = FxOptionVolatilitiesId.of("Bar");
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxOptionVolatilitiesId test = FxOptionVolatilitiesId.of("Foo");
		assertSerialization(test);
	  }

	}

}