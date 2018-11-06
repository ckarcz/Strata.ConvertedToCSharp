/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.common
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ExchangeId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ExchangeIdTest
	public class ExchangeIdTest
	{

	  private const object ANOTHER_TYPE = "";

	  public virtual void test_of()
	  {
		ExchangeId test = ExchangeId.of("GB");
		assertEquals(test.Name, "GB");
		assertEquals(test.ToString(), "GB");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		ExchangeId a = ExchangeId.of("ECAG");
		ExchangeId a2 = ExchangeId.of("ECAG");
		ExchangeId b = ExchangeId.of("XLON");
		assertEquals(a.GetHashCode(), a2.GetHashCode());
		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(a2), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(ExchangeIds));
	  }

	  public virtual void test_serialization()
	  {
		ExchangeId test = ExchangeId.of("ECAG");
		assertSerialization(test);
	  }

	}

}