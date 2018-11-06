/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="Attributes"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AttributesTest
	public class AttributesTest
	{

	  public virtual void test_empty()
	  {
		Attributes test = Attributes.empty();
		assertEquals(test.findAttribute(AttributeType.DESCRIPTION), null);
		assertThrows(typeof(System.ArgumentException), () => test.getAttribute(AttributeType.DESCRIPTION));

		Attributes test2 = test.withAttribute(AttributeType.NAME, "world");
		assertEquals(test2.getAttribute(AttributeType.NAME), "world");
	  }

	  public virtual void test_single()
	  {
		Attributes test = Attributes.of(AttributeType.DESCRIPTION, "hello");
		assertEquals(test.findAttribute(AttributeType.DESCRIPTION), ("hello"));
		assertEquals(test.getAttribute(AttributeType.DESCRIPTION), "hello");

		Attributes test2 = test.withAttribute(AttributeType.NAME, "world");
		assertEquals(test2.getAttribute(AttributeType.DESCRIPTION), "hello");
		assertEquals(test2.getAttribute(AttributeType.NAME), "world");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableBean test = (ImmutableBean) Attributes.of(AttributeType.DESCRIPTION, "hello");
		coverImmutableBean(test);
		ImmutableBean test2 = (ImmutableBean) Attributes.empty();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		Attributes test = Attributes.of(AttributeType.DESCRIPTION, "hello");
		assertSerialization(test);
	  }

	}

}