/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
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

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// Test <seealso cref="PositionInfo"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PositionInfoTest
	public class PositionInfoTest
	{

	  private static readonly StandardId ID = StandardId.of("OG-Test", "123");
	  private static readonly StandardId ID2 = StandardId.of("OG-Test", "321");

	  public virtual void test_builder()
	  {
		PositionInfo test = PositionInfo.builder().id(ID).build();
		assertEquals(test.Id, ID);
		assertEquals(test.AttributeTypes, ImmutableSet.of());
		assertEquals(test.Attributes, ImmutableMap.of());
		assertThrowsIllegalArg(() => test.getAttribute(AttributeType.DESCRIPTION));
		assertEquals(test.findAttribute(AttributeType.DESCRIPTION), null);
	  }

	  public virtual void test_builder_withers()
	  {
		PositionInfo test = PositionInfo.builder().build().withId(ID).withAttribute(AttributeType.DESCRIPTION, "A");
		assertEquals(test.Id, ID);
		assertEquals(test.AttributeTypes, ImmutableSet.of(AttributeType.DESCRIPTION));
		assertEquals(test.Attributes, ImmutableMap.of(AttributeType.DESCRIPTION, "A"));
		assertEquals(test.getAttribute(AttributeType.DESCRIPTION), "A");
		assertEquals(test.findAttribute(AttributeType.DESCRIPTION), ("A"));
	  }

	  public virtual void test_toBuilder()
	  {
		PositionInfo test = PositionInfo.builder().id(ID).build().toBuilder().id(ID2).build();
		assertEquals(test.Id, ID2);
		assertEquals(test.Attributes, ImmutableMap.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		PositionInfo test = PositionInfo.builder().id(ID).addAttribute(AttributeType.DESCRIPTION, "A").build();
		coverImmutableBean(test);
		PositionInfo test2 = PositionInfo.builder().id(ID2).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		PositionInfo test = PositionInfo.builder().id(ID).addAttribute(AttributeType.DESCRIPTION, "A").build();
		assertSerialization(test);
	  }

	}

}