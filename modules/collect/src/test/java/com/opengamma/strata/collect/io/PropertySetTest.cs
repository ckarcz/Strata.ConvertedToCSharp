using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableMultimap = com.google.common.collect.ImmutableMultimap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Multimap = com.google.common.collect.Multimap;

	/// <summary>
	/// Test <seealso cref="PropertySet"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PropertySetTest
	public class PropertySetTest
	{

	  private const object ANOTHER_TYPE = "";

	  public virtual void test_empty()
	  {
		PropertySet test = PropertySet.empty();

		assertEquals(test.Empty, true);
		assertEquals(test.contains("unknown"), false);
		assertEquals(test.valueList("unknown"), ImmutableList.of());
		assertThrowsIllegalArg(() => test.value("unknown"));
		assertEquals(test.ToString(), "{}");
	  }

	  public virtual void test_of_map()
	  {
		IDictionary<string, string> keyValues = ImmutableMap.of("a", "x", "b", "y");
		PropertySet test = PropertySet.of(keyValues);

		assertEquals(test.Empty, false);
		assertEquals(test.contains("a"), true);
		assertEquals(test.value("a"), "x");
		assertEquals(test.valueList("a"), ImmutableList.of("x"));
		assertEquals(test.contains("b"), true);
		assertEquals(test.value("b"), "y");
		assertEquals(test.valueList("b"), ImmutableList.of("y"));
		assertEquals(test.contains("c"), false);
		assertEquals(test.keys(), ImmutableSet.of("a", "b"));
		assertEquals(test.asMap(), ImmutableMap.of("a", "x", "b", "y"));
		assertEquals(test.asMultimap(), ImmutableListMultimap.of("a", "x", "b", "y"));
		assertEquals(test.valueList("unknown"), ImmutableSet.of());

		assertThrowsIllegalArg(() => test.value("unknown"));
		assertEquals(test.ToString(), "{a=[x], b=[y]}");
	  }

	  public virtual void test_of_multimap()
	  {
		Multimap<string, string> keyValues = ImmutableMultimap.of("a", "x", "a", "y", "b", "z");
		PropertySet test = PropertySet.of(keyValues);

		assertEquals(test.Empty, false);
		assertEquals(test.contains("a"), true);
		assertThrowsIllegalArg(() => test.value("a"));
		assertEquals(test.valueList("a"), ImmutableList.of("x", "y"));
		assertEquals(test.contains("b"), true);
		assertEquals(test.value("b"), "z");
		assertEquals(test.valueList("b"), ImmutableList.of("z"));
		assertEquals(test.contains("c"), false);
		assertEquals(test.keys(), ImmutableSet.of("a", "b"));
		assertEquals(test.asMultimap(), ImmutableListMultimap.of("a", "x", "a", "y", "b", "z"));
		assertEquals(test.valueList("unknown"), ImmutableSet.of());

		assertThrowsIllegalArg(() => test.asMap());
		assertThrowsIllegalArg(() => test.value("unknown"));
		assertEquals(test.ToString(), "{a=[x, y], b=[z]}");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		PropertySet @base = PropertySet.of(ImmutableListMultimap.of("a", "x", "a", "y", "c", "z"));
		PropertySet other = PropertySet.of(ImmutableListMultimap.of("a", "aa", "b", "bb", "d", "dd"));
		PropertySet expected = PropertySet.of(ImmutableListMultimap.of("a", "x", "a", "y", "c", "z", "b", "bb", "d", "dd"));
		assertEquals(@base.combinedWith(other), expected);
	  }

	  public virtual void test_combinedWith_emptyBase()
	  {
		PropertySet @base = PropertySet.of(ImmutableListMultimap.of("a", "x", "a", "y", "b", "y", "c", "z"));
		assertEquals(@base.combinedWith(PropertySet.empty()), @base);
	  }

	  public virtual void test_combinedWith_emptyOther()
	  {
		PropertySet @base = PropertySet.of(ImmutableListMultimap.of("a", "x", "a", "y", "b", "y", "c", "z"));
		assertEquals(PropertySet.empty().combinedWith(@base), @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_overrideWith()
	  {
		PropertySet @base = PropertySet.of(ImmutableListMultimap.of("a", "x", "a", "y", "b", "y", "c", "z"));
		PropertySet other = PropertySet.of(ImmutableListMultimap.of("a", "aa", "c", "cc", "d", "dd", "e", "ee"));
		PropertySet expected = PropertySet.of(ImmutableListMultimap.of("a", "aa", "b", "y", "c", "cc", "d", "dd", "e", "ee"));
		assertEquals(@base.overrideWith(other), expected);
	  }

	  public virtual void test_overrideWith_emptyBase()
	  {
		PropertySet @base = PropertySet.of(ImmutableListMultimap.of("a", "x", "a", "y", "b", "y", "c", "z"));
		assertEquals(@base.overrideWith(PropertySet.empty()), @base);
	  }

	  public virtual void test_overrideWith_emptyOther()
	  {
		PropertySet @base = PropertySet.of(ImmutableListMultimap.of("a", "x", "a", "y", "b", "y", "c", "z"));
		assertEquals(PropertySet.empty().overrideWith(@base), @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		IDictionary<string, string> keyValues = ImmutableMap.of("a", "x", "b", "y");
		PropertySet a1 = PropertySet.of(keyValues);
		PropertySet a2 = PropertySet.of(keyValues);
		PropertySet b = PropertySet.of(ImmutableMap.of("a", "x", "b", "z"));

		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(null), false);
		assertEquals(a1.Equals(ANOTHER_TYPE), false);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	}

}