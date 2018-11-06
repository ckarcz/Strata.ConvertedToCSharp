using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Test <seealso cref="XmlElement"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class XmlElementTest
	public class XmlElementTest
	{

	  private static readonly IDictionary<string, string> ATTR_MAP_EMPTY = ImmutableMap.of();
	  private static readonly IDictionary<string, string> ATTR_MAP = ImmutableMap.of("key", "value", "og", "strata");
	  private static readonly XmlElement LEAF1 = XmlElement.ofContent("leaf1", ATTR_MAP_EMPTY, "leaf");
	  private static readonly XmlElement LEAF2A = XmlElement.ofContent("leaf2", ATTR_MAP_EMPTY, "a");
	  private static readonly XmlElement LEAF2B = XmlElement.ofContent("leaf2", ATTR_MAP_EMPTY, "b");
	  private static readonly IList<XmlElement> CHILD_LIST_EMPTY = ImmutableList.of();
	  private static readonly IList<XmlElement> CHILD_LIST_ONE = ImmutableList.of(LEAF1);
	  private static readonly IList<XmlElement> CHILD_LIST_MULTI = ImmutableList.of(LEAF1, LEAF2A, LEAF2B);

	  //-------------------------------------------------------------------------
	  public virtual void test_ofChildren_empty()
	  {
		XmlElement test = XmlElement.ofChildren("test", CHILD_LIST_EMPTY);
		assertEquals(test.Name, "test");
		assertEquals(test.Attributes, ATTR_MAP_EMPTY);
		assertEquals(test.hasContent(), false);
		assertEquals(test.Content, "");
		assertEquals(test.Children, CHILD_LIST_EMPTY);
		assertThrowsIllegalArg(() => test.getAttribute("notFound"));
		assertThrows(() => test.getChild(0), typeof(System.IndexOutOfRangeException));
		assertThrowsIllegalArg(() => test.getChild("notFound"));
		assertEquals(test.findChild("notFound"), null);
		assertEquals(test.getChildren("notFound"), ImmutableList.of());
		assertEquals(test.ToString(), "<test></test>");
	  }

	  public virtual void test_ofChildren_one()
	  {
		XmlElement test = XmlElement.ofChildren("test", ATTR_MAP, CHILD_LIST_ONE);
		assertEquals(test.Name, "test");
		assertEquals(test.Attributes, ATTR_MAP);
		assertEquals(test.hasContent(), false);
		assertEquals(test.Content, "");
		assertEquals(test.Children, CHILD_LIST_ONE);
		assertEquals(test.getAttribute("key"), "value");
		assertEquals(test.findAttribute("key"), ("value"));
		assertEquals(test.findAttribute("none"), null);
		assertEquals(test.getChild(0), LEAF1);
		assertEquals(test.getChild("leaf1"), LEAF1);
		assertEquals(test.findChild("leaf1"), LEAF1);
		assertEquals(test.getChildren("leaf1"), ImmutableList.of(LEAF1));
		assertEquals(test.ToString(), "<test key=\"value\" og=\"strata\">" + Environment.NewLine + " <leaf1 ... />" + Environment.NewLine + "</test>");
	  }

	  public virtual void test_ofChildren_multi()
	  {
		XmlElement test = XmlElement.ofChildren("test", ATTR_MAP, CHILD_LIST_MULTI);
		assertEquals(test.Name, "test");
		assertEquals(test.Attributes, ATTR_MAP);
		assertEquals(test.getAttribute("key"), "value");
		assertEquals(test.hasContent(), false);
		assertEquals(test.Content, "");
		assertEquals(test.Children, CHILD_LIST_MULTI);
		assertEquals(test.getAttribute("key"), "value");
		assertEquals(test.getChild(0), LEAF1);
		assertEquals(test.getChild(1), LEAF2A);
		assertEquals(test.getChild(2), LEAF2B);
		assertEquals(test.getChild("leaf1"), LEAF1);
		assertThrowsIllegalArg(() => test.getChild("leaf2"));
		assertEquals(test.findChild("leaf1"), LEAF1);
		assertThrowsIllegalArg(() => test.findChild("leaf2"));
		assertEquals(test.getChildren("leaf1"), ImmutableList.of(LEAF1));
		assertEquals(test.getChildren("leaf2"), ImmutableList.of(LEAF2A, LEAF2B));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ofContent()
	  {
		XmlElement test = XmlElement.ofContent("test", ATTR_MAP_EMPTY, "hello");
		assertEquals(test.Name, "test");
		assertEquals(test.Attributes, ATTR_MAP_EMPTY);
		assertEquals(test.hasContent(), true);
		assertEquals(test.Content, "hello");
		assertEquals(test.Children, CHILD_LIST_EMPTY);
		assertThrowsIllegalArg(() => test.getAttribute("notFound"));
		assertThrows(() => test.getChild(0), typeof(System.IndexOutOfRangeException));
		assertThrowsIllegalArg(() => test.getChild("notFound"));
		assertEquals(test.findChild("notFound"), null);
		assertEquals(test.getChildren("notFound"), ImmutableList.of());
		assertEquals(test.ToString(), "<test>hello</test>");
	  }

	  public virtual void test_ofContent_empty()
	  {
		XmlElement test = XmlElement.ofContent("test", "");
		assertEquals(test.Name, "test");
		assertEquals(test.Attributes, ATTR_MAP_EMPTY);
		assertEquals(test.hasContent(), false);
		assertEquals(test.Content, "");
		assertEquals(test.Children, CHILD_LIST_EMPTY);
		assertThrowsIllegalArg(() => test.getAttribute("notFound"));
		assertThrows(() => test.getChild(0), typeof(System.IndexOutOfRangeException));
		assertThrowsIllegalArg(() => test.getChild("notFound"));
		assertEquals(test.findChild("notFound"), null);
		assertEquals(test.getChildren("notFound"), ImmutableList.of());
		assertEquals(test.ToString(), "<test></test>");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		XmlElement test = XmlElement.ofChildren("test", ATTR_MAP, CHILD_LIST_MULTI);
		coverImmutableBean(test);
		XmlElement test2 = XmlElement.ofChildren("test2", ATTR_MAP_EMPTY, CHILD_LIST_EMPTY);
		coverBeanEquals(test, test2);
		XmlElement test3 = XmlElement.ofContent("test3", ATTR_MAP_EMPTY, "content");
		coverBeanEquals(test2, test3);
		coverBeanEquals(test, test3);
	  }

	}

}