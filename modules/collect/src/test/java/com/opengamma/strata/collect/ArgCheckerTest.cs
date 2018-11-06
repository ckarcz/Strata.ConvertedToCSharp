using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertUtilityClass;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CharMatcher = com.google.common.@base.CharMatcher;
	using ImmutableSortedMap = com.google.common.collect.ImmutableSortedMap;

	/// <summary>
	/// Test ArgChecker.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ArgCheckerTest
	public class ArgCheckerTest
	{

	  public virtual void test_isTrue_simple_ok()
	  {
		ArgChecker.isTrue(true);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_isTrue_simple_false()
	  public virtual void test_isTrue_simple_false()
	  {
		ArgChecker.isTrue(false);
	  }

	  public virtual void test_isTrue_ok()
	  {
		ArgChecker.isTrue(true, "Message");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "Message") public void test_isTrue_false()
	  public virtual void test_isTrue_false()
	  {
		ArgChecker.isTrue(false, "Message");
	  }

	  public virtual void test_isTrue_ok_args()
	  {
		ArgChecker.isTrue(true, "Message {} {} {}", "A", 2, 3d);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "Message A 2 3.0") public void test_isTrue_false_args()
	  public virtual void test_isTrue_false_args()
	  {
		ArgChecker.isTrue(false, "Message {} {} {}", "A", 2, 3d);
	  }

	  public virtual void test_isTrue_ok_longArg()
	  {
		ArgChecker.isTrue(true, "Message {}", 3L);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "Message 3") public void test_isTrue_false_longArg()
	  public virtual void test_isTrue_false_longArg()
	  {
		ArgChecker.isTrue(false, "Message {}", 3L);
	  }

	  public virtual void test_isTrue_ok_doubleArg()
	  {
		ArgChecker.isTrue(true, "Message {}", 3d);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "Message 3.0") public void test_isTrue_false_doubleArg()
	  public virtual void test_isTrue_false_doubleArg()
	  {
		ArgChecker.isTrue(false, "Message {}", 3d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_isFalse_ok()
	  {
		ArgChecker.isFalse(false, "Message");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "Message") public void test_isFalse_true()
	  public virtual void test_isFalse_true()
	  {
		ArgChecker.isFalse(true, "Message");
	  }

	  public virtual void test_isFalse_ok_args()
	  {
		ArgChecker.isFalse(false, "Message {} {} {}", "A", 2.0, 3, true);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "Message A 2 3.0") public void test_isFalse_ok_args_true()
	  public virtual void test_isFalse_ok_args_true()
	  {
		ArgChecker.isFalse(true, "Message {} {} {}", "A", 2, 3.0);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notNull_ok()
	  {
		assertEquals(ArgChecker.notNull("OG", "name"), "OG");
		assertEquals(ArgChecker.notNull(1, "name"), Convert.ToInt32(1));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*") public void test_notNull_null()
	  public virtual void test_notNull_null()
	  {
		ArgChecker.notNull(null, "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notNullItem_noText_ok()
	  {
		assertEquals(ArgChecker.notNullItem("OG"), "OG");
		assertEquals(ArgChecker.notNullItem(1), Convert.ToInt32(1));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_notNullItem_noText_null()
	  public virtual void test_notNullItem_noText_null()
	  {
		ArgChecker.notNullItem(null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_matches_String_ok()
	  {
		assertEquals(ArgChecker.matches(Pattern.compile("[A-Z]+"), "OG", "name"), "OG");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'pattern'.*") public void test_matches_String_nullPattern()
	  public virtual void test_matches_String_nullPattern()
	  {
		ArgChecker.matches(null, "", "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*") public void test_matches_String_nullString()
	  public virtual void test_matches_String_nullString()
	  {
		ArgChecker.matches(Pattern.compile("[A-Z]+"), null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*") public void test_matches_String_empty()
	  public virtual void test_matches_String_empty()
	  {
		ArgChecker.matches(Pattern.compile("[A-Z]+"), "", "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*'123'.*") public void test_matches_String_noMatch()
	  public virtual void test_matches_String_noMatch()
	  {
		ArgChecker.matches(Pattern.compile("[A-Z]+"), "123", "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_matches_CharMatcher_String_ok()
	  {
		assertEquals(ArgChecker.matches(CharMatcher.inRange('A', 'Z'), 1, int.MaxValue, "OG", "name", "[A-Z]+"), "OG");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*") public void test_matches_CharMatcher_String_tooShort()
	  public virtual void test_matches_CharMatcher_String_tooShort()
	  {
		ArgChecker.matches(CharMatcher.inRange('A', 'Z'), 1, 2, "", "name", "[A-Z]+");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*") public void test_matches_CharMatcher_String_tooLong()
	  public virtual void test_matches_CharMatcher_String_tooLong()
	  {
		ArgChecker.matches(CharMatcher.inRange('A', 'Z'), 1, 2, "abc", "name", "[A-Z]+");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'pattern'.*") public void test_matches_CharMatcher_String_nullMatcher()
	  public virtual void test_matches_CharMatcher_String_nullMatcher()
	  {
		ArgChecker.matches(null, 1, int.MaxValue, "", "name", "[A-Z]+");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*") public void test_matches_CharMatcher_String_nullString()
	  public virtual void test_matches_CharMatcher_String_nullString()
	  {
		ArgChecker.matches(CharMatcher.inRange('A', 'Z'), 1, 2, null, "name", "[A-Z]+");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*'123'.*") public void test_matches_CharMatcher_String_noMatch()
	  public virtual void test_matches_CharMatcher_String_noMatch()
	  {
		ArgChecker.matches(CharMatcher.inRange('A', 'Z'), 1, int.MaxValue, "123", "name", "[A-Z]+");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notBlank_String_ok()
	  {
		assertEquals(ArgChecker.notBlank("OG", "name"), "OG");
	  }

	  public virtual void test_notBlank_String_ok_notTrimmed()
	  {
		assertEquals(ArgChecker.notBlank(" OG ", "name"), " OG ");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*") public void test_notBlank_String_null()
	  public virtual void test_notBlank_String_null()
	  {
		ArgChecker.notBlank(null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*") public void test_notBlank_String_empty()
	  public virtual void test_notBlank_String_empty()
	  {
		ArgChecker.notBlank("", "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*") public void test_notBlank_String_spaces()
	  public virtual void test_notBlank_String_spaces()
	  {
		ArgChecker.notBlank("  ", "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notEmpty_String_ok()
	  {
		assertEquals(ArgChecker.notEmpty("OG", "name"), "OG");
		assertEquals(ArgChecker.notEmpty(" ", "name"), " ");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_notEmpty_String_null()
	  public virtual void test_notEmpty_String_null()
	  {
		ArgChecker.notEmpty((string) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*empty.*") public void test_notEmpty_String_empty()
	  public virtual void test_notEmpty_String_empty()
	  {
		ArgChecker.notEmpty("", "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notEmpty_Array_ok()
	  {
		object[] expected = new object[] {"Element"};
		object[] result = ArgChecker.notEmpty(expected, "name");
		assertEquals(result, expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_notEmpty_Array_null()
	  public virtual void test_notEmpty_Array_null()
	  {
		ArgChecker.notEmpty((object[]) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*array.*'name'.*empty.*") public void test_notEmpty_Array_empty()
	  public virtual void test_notEmpty_Array_empty()
	  {
		ArgChecker.notEmpty(new object[] {}, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_notEmpty_2DArray_null()
	  public virtual void test_notEmpty_2DArray_null()
	  {
		ArgChecker.notEmpty((object[][]) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*array.*'name'.*empty.*") public void test_notEmpty_2DArray_empty()
	  public virtual void test_notEmpty_2DArray_empty()
	  {
		ArgChecker.notEmpty(new object[0][], "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notEmpty_intArray_ok()
	  {
		int[] expected = new int[] {6};
		int[] result = ArgChecker.notEmpty(expected, "name");
		assertEquals(result, expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_notEmpty_intArray_null()
	  public virtual void test_notEmpty_intArray_null()
	  {
		ArgChecker.notEmpty((int[]) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*array.*'name'.*empty.*") public void test_notEmpty_intArray_empty()
	  public virtual void test_notEmpty_intArray_empty()
	  {
		ArgChecker.notEmpty(new int[0], "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notEmpty_longArray_ok()
	  {
		long[] expected = new long[] {6L};
		long[] result = ArgChecker.notEmpty(expected, "name");
		assertEquals(result, expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_notEmpty_longArray_null()
	  public virtual void test_notEmpty_longArray_null()
	  {
		ArgChecker.notEmpty((long[]) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*array.*'name'.*empty.*") public void test_notEmpty_longArray_empty()
	  public virtual void test_notEmpty_longArray_empty()
	  {
		ArgChecker.notEmpty(new long[0], "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notEmpty_doubleArray_ok()
	  {
		double[] expected = new double[] {6.0d};
		double[] result = ArgChecker.notEmpty(expected, "name");
		assertEquals(result, expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_notEmpty_doubleArray_null()
	  public virtual void test_notEmpty_doubleArray_null()
	  {
		ArgChecker.notEmpty((double[]) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*array.*'name'.*empty.*") public void test_notEmpty_doubleArray_empty()
	  public virtual void test_notEmpty_doubleArray_empty()
	  {
		ArgChecker.notEmpty(new double[0], "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notEmpty_Iterable_ok()
	  {
		IEnumerable<string> expected = Arrays.asList("Element");
		IEnumerable<string> result = ArgChecker.notEmpty((IEnumerable<string>) expected, "name");
		assertEquals(result, expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_notEmpty_Iterable_null()
	  public virtual void test_notEmpty_Iterable_null()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ArgChecker.notEmpty((Iterable<?>) null, "name");
		ArgChecker.notEmpty((IEnumerable<object>) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*iterable.*'name'.*empty.*") public void test_notEmpty_Iterable_empty()
	  public virtual void test_notEmpty_Iterable_empty()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ArgChecker.notEmpty((Iterable<?>) java.util.Collections.emptyList(), "name");
		ArgChecker.notEmpty((IEnumerable<object>) Collections.emptyList(), "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notEmpty_Collection_ok()
	  {
		IList<string> expected = Arrays.asList("Element");
		IList<string> result = ArgChecker.notEmpty(expected, "name");
		assertEquals(result, expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_notEmpty_Collection_null()
	  public virtual void test_notEmpty_Collection_null()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ArgChecker.notEmpty((java.util.Collection<?>) null, "name");
		ArgChecker.notEmpty((ICollection<object>) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*collection.*'name'.*empty.*") public void test_notEmpty_Collection_empty()
	  public virtual void test_notEmpty_Collection_empty()
	  {
		ArgChecker.notEmpty(Collections.emptyList(), "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notEmpty_Map_ok()
	  {
		SortedDictionary<string, string> expected = ImmutableSortedMap.of("Element", "Element");
		SortedDictionary<string, string> result = ArgChecker.notEmpty(expected, "name");
		assertEquals(result, expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_notEmpty_Map_null()
	  public virtual void test_notEmpty_Map_null()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ArgChecker.notEmpty((java.util.Map<?, ?>) null, "name");
		ArgChecker.notEmpty((IDictionary<object, ?>) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*map.*'name'.*empty.*") public void test_notEmpty_Map_empty()
	  public virtual void test_notEmpty_Map_empty()
	  {
		ArgChecker.notEmpty(Collections.emptyMap(), "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_noNulls_Array_ok()
	  {
		string[] expected = new string[] {"Element"};
		string[] result = ArgChecker.noNulls(expected, "name");
		assertEquals(result, expected);
	  }

	  public virtual void test_noNulls_Array_ok_empty()
	  {
		object[] array = new object[] {};
		ArgChecker.noNulls(array, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_noNulls_Array_null()
	  public virtual void test_noNulls_Array_null()
	  {
		ArgChecker.noNulls((object[]) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*array.*'name'.*null.*") public void test_noNulls_Array_nullElement()
	  public virtual void test_noNulls_Array_nullElement()
	  {
		ArgChecker.noNulls(new object[] {null}, "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_noNulls_Iterable_ok()
	  {
		IList<string> expected = Arrays.asList("Element");
		IList<string> result = ArgChecker.noNulls(expected, "name");
		assertEquals(result, expected);
	  }

	  public virtual void test_noNulls_Iterable_ok_empty()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: Iterable<?> coll = java.util.Arrays.asList();
		IEnumerable<object> coll = Arrays.asList();
		ArgChecker.noNulls(coll, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_noNulls_Iterable_null()
	  public virtual void test_noNulls_Iterable_null()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ArgChecker.noNulls((Iterable<?>) null, "name");
		ArgChecker.noNulls((IEnumerable<object>) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*iterable.*'name'.*null.*") public void test_noNulls_Iterable_nullElement()
	  public virtual void test_noNulls_Iterable_nullElement()
	  {
		ArgChecker.noNulls(Arrays.asList((object) null), "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_noNulls_Map_ok()
	  {
		ImmutableSortedMap<string, string> expected = ImmutableSortedMap.of("A", "B");
		ImmutableSortedMap<string, string> result = ArgChecker.noNulls(expected, "name");
		assertEquals(result, expected);
	  }

	  public virtual void test_noNulls_Map_ok_empty()
	  {
		IDictionary<object, object> map = new Dictionary<object, object>();
		ArgChecker.noNulls(map, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*null.*") public void test_noNulls_Map_null()
	  public virtual void test_noNulls_Map_null()
	  {
		ArgChecker.noNulls((IDictionary<object, object>) null, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*map.*'name'.*null.*") public void test_noNulls_Map_nullKey()
	  public virtual void test_noNulls_Map_nullKey()
	  {
		IDictionary<object, object> map = new Dictionary<object, object>();
		map["A"] = "B";
		map[null] = "Z";
		ArgChecker.noNulls(map, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*map.*'name'.*null.*") public void test_noNulls_Map_nullValue()
	  public virtual void test_noNulls_Map_nullValue()
	  {
		IDictionary<object, object> map = new Dictionary<object, object>();
		map["A"] = "B";
		map["Z"] = null;
		ArgChecker.noNulls(map, "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notNegative_int_ok()
	  {
		assertEquals(ArgChecker.notNegative(0, "name"), 0);
		assertEquals(ArgChecker.notNegative(1, "name"), 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*negative.*") public void test_notNegative_int_negative()
	  public virtual void test_notNegative_int_negative()
	  {
		ArgChecker.notNegative(-1, "name");
	  }

	  public virtual void test_notNegative_long_ok()
	  {
		assertEquals(ArgChecker.notNegative(0L, "name"), 0L);
		assertEquals(ArgChecker.notNegative(1L, "name"), 1L);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*negative.*") public void test_notNegative_long_negative()
	  public virtual void test_notNegative_long_negative()
	  {
		ArgChecker.notNegative(-1L, "name");
	  }

	  public virtual void test_notNegative_double_ok()
	  {
		assertEquals(ArgChecker.notNegative(0d, "name"), 0d, 0.0001d);
		assertEquals(ArgChecker.notNegative(1d, "name"), 1d, 0.0001d);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*negative.*") public void test_notNegative_double_negative()
	  public virtual void test_notNegative_double_negative()
	  {
		ArgChecker.notNegative(-1.0d, "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notNegativeOrZero_int_ok()
	  {
		assertEquals(ArgChecker.notNegativeOrZero(1, "name"), 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*negative.*zero.*") public void test_notNegativeOrZero_int_zero()
	  public virtual void test_notNegativeOrZero_int_zero()
	  {
		ArgChecker.notNegativeOrZero(0, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*negative.*zero.*") public void test_notNegativeOrZero_int_negative()
	  public virtual void test_notNegativeOrZero_int_negative()
	  {
		ArgChecker.notNegativeOrZero(-1, "name");
	  }

	  public virtual void test_notNegativeOrZero_long_ok()
	  {
		assertEquals(ArgChecker.notNegativeOrZero(1L, "name"), 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*negative.*zero.*") public void test_notNegativeOrZero_long_zero()
	  public virtual void test_notNegativeOrZero_long_zero()
	  {
		ArgChecker.notNegativeOrZero(0L, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*negative.*zero.*") public void test_notNegativeOrZero_long_negative()
	  public virtual void test_notNegativeOrZero_long_negative()
	  {
		ArgChecker.notNegativeOrZero(-1L, "name");
	  }

	  public virtual void test_notNegativeOrZero_double_ok()
	  {
		assertEquals(ArgChecker.notNegativeOrZero(1d, "name"), 1d, 0.0001d);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*negative.*zero.*") public void test_notNegativeOrZero_double_zero()
	  public virtual void test_notNegativeOrZero_double_zero()
	  {
		ArgChecker.notNegativeOrZero(0.0d, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*negative.*zero.*") public void test_notNegativeOrZero_double_negative()
	  public virtual void test_notNegativeOrZero_double_negative()
	  {
		ArgChecker.notNegativeOrZero(-1.0d, "name");
	  }

	  public virtual void test_notNegativeOrZero_double_eps_ok()
	  {
		assertEquals(ArgChecker.notNegativeOrZero(1d, 0.0001d, "name"), 1d, 0.0001d);
		assertEquals(ArgChecker.notNegativeOrZero(0.1d, 0.0001d, "name"), 0.1d, 0.0001d);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*zero.*") public void test_notNegativeOrZero_double_eps_zero()
	  public virtual void test_notNegativeOrZero_double_eps_zero()
	  {
		ArgChecker.notNegativeOrZero(0.0000001d, 0.0001d, "name");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*greater.*zero.*") public void test_notNegativeOrZero_double_eps_negative()
	  public virtual void test_notNegativeOrZero_double_eps_negative()
	  {
		ArgChecker.notNegativeOrZero(-1.0d, 0.0001d, "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notZero_double_ok()
	  {
		assertEquals(ArgChecker.notZero(1d, "name"), 1d, 0.0001d);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*zero.*") public void test_notZero_double_zero()
	  public virtual void test_notZero_double_zero()
	  {
		ArgChecker.notZero(0d, "name");
	  }

	  public virtual void test_notZero_double_negative()
	  {
		ArgChecker.notZero(-1d, "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_notZero_double_tolerance_ok()
	  {
		assertEquals(ArgChecker.notZero(1d, 0.1d, "name"), 1d, 0.0001d);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*'name'.*zero.*") public void test_notZero_double_tolerance_zero()
	  public virtual void test_notZero_double_tolerance_zero()
	  {
		ArgChecker.notZero(0d, 0.1d, "name");
	  }

	  public virtual void test_notZero_double_tolerance_negative()
	  {
		ArgChecker.notZero(-1d, 0.1d, "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_double_inRange()
	  {
		double low = 0d;
		double mid = 0.5d;
		double high = 1d;
		double small = 0.00000000001d;
		assertEquals(ArgChecker.inRange(mid, low, high, "name"), mid);
		assertEquals(ArgChecker.inRange(low, low, high, "name"), low);
		assertEquals(ArgChecker.inRange(high - small, low, high, "name"), high - small);

		assertEquals(ArgChecker.inRangeInclusive(mid, low, high, "name"), mid);
		assertEquals(ArgChecker.inRangeInclusive(low, low, high, "name"), low);
		assertEquals(ArgChecker.inRangeInclusive(high, low, high, "name"), high);

		assertEquals(ArgChecker.inRangeExclusive(mid, low, high, "name"), mid);
		assertEquals(ArgChecker.inRangeExclusive(small, low, high, "name"), small);
		assertEquals(ArgChecker.inRangeExclusive(high - small, low, high, "name"), high - small);
	  }

	  public virtual void test_double_inRange_outOfRange()
	  {
		double low = 0d;
		double high = 1d;
		double small = 0.00000000001d;
		assertThrowsIllegalArg(() => ArgChecker.inRange(low - small, low, high, "name"));
		assertThrowsIllegalArg(() => ArgChecker.inRange(high, low, high, "name"));

		assertThrowsIllegalArg(() => ArgChecker.inRangeInclusive(low - small, low, high, "name"));
		assertThrowsIllegalArg(() => ArgChecker.inRangeInclusive(high + small, low, high, "name"));

		assertThrowsIllegalArg(() => ArgChecker.inRangeExclusive(low, low, high, "name"));
		assertThrowsIllegalArg(() => ArgChecker.inRangeExclusive(high, low, high, "name"));
	  }

	  public virtual void test_int_inRange()
	  {
		int low = 0;
		int mid = 1;
		int high = 2;
		assertEquals(ArgChecker.inRange(mid, low, high, "name"), mid);
		assertEquals(ArgChecker.inRange(low, low, high, "name"), low);

		assertEquals(ArgChecker.inRangeInclusive(mid, low, high, "name"), mid);
		assertEquals(ArgChecker.inRangeInclusive(low, low, high, "name"), low);
		assertEquals(ArgChecker.inRangeInclusive(high, low, high, "name"), high);

		assertEquals(ArgChecker.inRangeExclusive(mid, low, high, "name"), mid);
	  }

	  public virtual void test_int_inRange_outOfRange()
	  {
		int low = 0;
		int high = 1;
		assertThrowsIllegalArg(() => ArgChecker.inRange(low - 1, low, high, "name"));
		assertThrowsIllegalArg(() => ArgChecker.inRange(high, low, high, "name"));

		assertThrowsIllegalArg(() => ArgChecker.inRangeInclusive(low - 1, low, high, "name"));
		assertThrowsIllegalArg(() => ArgChecker.inRangeInclusive(high + 1, low, high, "name"));

		assertThrowsIllegalArg(() => ArgChecker.inRangeExclusive(low, low, high, "name"));
		assertThrowsIllegalArg(() => ArgChecker.inRangeExclusive(high, low, high, "name"));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*array.*'name'.*empty.*") public void testNotEmptyLongArray()
	  public virtual void testNotEmptyLongArray()
	  {
		ArgChecker.notEmpty(new double[0], "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_inOrderNotEqual_true()
	  {
		LocalDate a = LocalDate.of(2011, 7, 2);
		LocalDate b = LocalDate.of(2011, 7, 3);
		ArgChecker.inOrderNotEqual(a, b, "a", "b");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*a.* [<] .*b.*") public void test_inOrderNotEqual_false_invalidOrder()
	  public virtual void test_inOrderNotEqual_false_invalidOrder()
	  {
		LocalDate a = LocalDate.of(2011, 7, 2);
		LocalDate b = LocalDate.of(2011, 7, 3);
		ArgChecker.inOrderNotEqual(b, a, "a", "b");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*a.* [<] .*b.*") public void test_inOrderNotEqual_false_equal()
	  public virtual void test_inOrderNotEqual_false_equal()
	  {
		LocalDate a = LocalDate.of(2011, 7, 3);
		ArgChecker.inOrderNotEqual(a, a, "a", "b");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_inOrderOrEqual_true()
	  {
		LocalDate a = LocalDate.of(2011, 7, 2);
		LocalDate b = LocalDate.of(2011, 7, 3);
		ArgChecker.inOrderOrEqual(a, b, "a", "b");
		ArgChecker.inOrderOrEqual(a, a, "a", "b");
		ArgChecker.inOrderOrEqual(b, b, "a", "b");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = ".*a.* [<][=] .*b.*") public void test_inOrderOrEqual_false()
	  public virtual void test_inOrderOrEqual_false()
	  {
		LocalDate a = LocalDate.of(2011, 7, 3);
		LocalDate b = LocalDate.of(2011, 7, 2);
		ArgChecker.inOrderOrEqual(a, b, "a", "b");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_validUtilityClass()
	  {
		assertUtilityClass(typeof(ArgChecker));
	  }

	}

}