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

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// Test <seealso cref="PortfolioItemInfo"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PortfolioItemInfoTest
	public class PortfolioItemInfoTest
	{

	  private static readonly StandardId ID = StandardId.of("OG-Test", "123");
	  private static readonly StandardId ID2 = StandardId.of("OG-Test", "321");

	  public virtual void test_withers()
	  {
		PortfolioItemInfo test = PortfolioItemInfo.empty().withId(ID).withAttribute(AttributeType.DESCRIPTION, "A");
		assertEquals(test.Id, ID);
		assertEquals(test.AttributeTypes, ImmutableSet.of(AttributeType.DESCRIPTION));
		assertEquals(test.getAttribute(AttributeType.DESCRIPTION), "A");
		assertEquals(test.findAttribute(AttributeType.DESCRIPTION), ("A"));
		assertThrows(typeof(System.ArgumentException), () => test.getAttribute(AttributeType.NAME));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ItemInfo test = ItemInfo.empty().withId(ID).withAttribute(AttributeType.DESCRIPTION, "A");
		coverImmutableBean(test);
		ItemInfo test2 = ItemInfo.empty().withId(ID2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ItemInfo test = ItemInfo.empty().withId(ID).withAttribute(AttributeType.DESCRIPTION, "A");
		assertSerialization(test);
	  }

	}

}