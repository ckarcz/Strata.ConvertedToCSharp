using System.Collections.Generic;
using System.Text;

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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ByteSource = com.google.common.io.ByteSource;
	using Files = com.google.common.io.Files;

	/// <summary>
	/// Test <seealso cref="XmlFile"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class XmlFileTest
	public class XmlFileTest
	{

	  private static readonly string SAMPLE = "" +
		  "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
		  "<!--Leading comment-->" +
		  "<base>" +
		  " <test key=\"value\" og=\"strata\">" +
		  "  <leaf1>l<![CDATA[e]]>af</leaf1>" +
		  "  <leaf2>a<!-- comment ignored --></leaf2>" +
		  "  <leaf2>b</leaf2>" +
		  " </test>" +
		  "</base>";
	  private static readonly string SAMPLE_MISMATCHED_TAGS = "" +
		  "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
		  "<base>" +
		  " <test>" +
		  " </foo>" +
		  "</base>";
	  private static readonly string SAMPLE_BAD_END = "" +
		  "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
		  "<base>" +
		  " <test>" +
		  " </foo>";
	  private static readonly string SAMPLE_NAMESPACE = "" +
		  "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
		  "<base xmlns=\"https://opengamma.com/test\" xmlns:h=\"http://www.w3.org/TR/html4/\">" +
		  " <h:p>Some text</h:p>" +
		  " <leaf1 h:foo='bla' og='strata'>leaf</leaf1>" +
		  "</base>";

	  private static readonly IDictionary<string, string> ATTR_MAP_EMPTY = ImmutableMap.of();
	  private static readonly IDictionary<string, string> ATTR_MAP = ImmutableMap.of("key", "value", "og", "strata");
	  private static readonly XmlElement LEAF1 = XmlElement.ofContent("leaf1", ATTR_MAP_EMPTY, "leaf");
	  private static readonly XmlElement LEAF2A = XmlElement.ofContent("leaf2", ATTR_MAP_EMPTY, "a");
	  private static readonly XmlElement LEAF2B = XmlElement.ofContent("leaf2", ATTR_MAP_EMPTY, "b");
	  private static readonly IList<XmlElement> CHILD_LIST_MULTI = ImmutableList.of(LEAF1, LEAF2A, LEAF2B);
	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
	  public virtual void test_of_ByteSource()
	  {
		ByteSource source = ByteSource.wrap(SAMPLE.GetBytes(Encoding.UTF8));
		XmlFile test = XmlFile.of(source);
		XmlElement root = test.Root;
		assertEquals(root.Name, "base");
		assertEquals(root.Attributes, ATTR_MAP_EMPTY);
		assertEquals(root.Content, "");
		assertEquals(root.Children.size(), 1);
		XmlElement child = root.getChild(0);
		assertEquals(child, XmlElement.ofChildren("test", ATTR_MAP, CHILD_LIST_MULTI));
		assertEquals(test.References, ImmutableMap.of());
	  }

	  public virtual void test_of_ByteSource_namespace()
	  {
		ByteSource source = ByteSource.wrap(SAMPLE_NAMESPACE.GetBytes(Encoding.UTF8));
		XmlFile test = XmlFile.of(source);
		XmlElement root = test.Root;
		assertEquals(root.Name, "base");
		assertEquals(root.Attributes, ImmutableMap.of());
		assertEquals(root.Content, "");
		assertEquals(root.Children.size(), 2);
		XmlElement child1 = root.getChild(0);
		assertEquals(child1.Name, "p");
		assertEquals(child1.Content, "Some text");
		assertEquals(child1.Attributes, ImmutableMap.of());
		XmlElement child2 = root.getChild(1);
		assertEquals(child2.Name, "leaf1");
		assertEquals(child2.Content, "leaf");
		assertEquals(child2.Attributes, ImmutableMap.of("foo", "bla", "og", "strata"));
		assertEquals(test.References, ImmutableMap.of());
	  }

	  public virtual void test_of_ByteSource_mismatchedTags()
	  {
		ByteSource source = ByteSource.wrap(SAMPLE_MISMATCHED_TAGS.GetBytes(Encoding.UTF8));
		assertThrowsIllegalArg(() => XmlFile.of(source));
	  }

	  public virtual void test_of_ByteSource_badEnd()
	  {
		ByteSource source = ByteSource.wrap(SAMPLE_BAD_END.GetBytes(Encoding.UTF8));
		assertThrowsIllegalArg(() => XmlFile.of(source));
	  }

	  public virtual void test_of_ByteSource_ioException()
	  {
		ByteSource source = Files.asByteSource(new File("/oh-dear-no-such-file"));
		assertThrows(() => XmlFile.of(source), typeof(UncheckedIOException));
	  }

	  public virtual void test_of_ByteSource_parsedReferences()
	  {
		ByteSource source = ByteSource.wrap(SAMPLE.GetBytes(Encoding.UTF8));
		XmlFile test = XmlFile.of(source, "key");
		XmlElement root = test.Root;
		assertEquals(root.Name, "base");
		assertEquals(root.Attributes, ATTR_MAP_EMPTY);
		assertEquals(root.Content, "");
		assertEquals(root.Children.size(), 1);
		XmlElement child = root.getChild(0);
		assertEquals(child, XmlElement.ofChildren("test", ATTR_MAP, CHILD_LIST_MULTI));
		assertEquals(test.References, ImmutableMap.of("value", root.getChild(0)));
	  }

	  public virtual void test_of_ByteSource_parsedReferences_ioException()
	  {
		ByteSource source = Files.asByteSource(new File("/oh-dear-no-such-file"));
		assertThrows(() => XmlFile.of(source, "key"), typeof(UncheckedIOException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCodeToString()
	  {
		ByteSource source = ByteSource.wrap(SAMPLE.GetBytes(Encoding.UTF8));
		XmlFile test = XmlFile.of(source);
		XmlFile test2 = XmlFile.of(source);
		assertFalse(test.Equals(null));
		assertFalse(test.Equals(ANOTHER_TYPE));
		assertEquals(test, test);
		assertEquals(test, test2);
		assertEquals(test.GetHashCode(), test2.GetHashCode());
		assertEquals(test.ToString(), test2.ToString());
	  }

	}

}