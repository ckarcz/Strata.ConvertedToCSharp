/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
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


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Multimap = com.google.common.collect.Multimap;
	using CharSource = com.google.common.io.CharSource;
	using Files = com.google.common.io.Files;

	/// <summary>
	/// Test <seealso cref="IniFile"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IniFileTest
	public class IniFileTest
	{

	  private readonly string INI1 = "" +
		  "# comment\n" +
		  "[section]\n" +
		  "c = x\n" +
		  "b = y\n" +
		  "a = z\n" +
		  "\n" +
		  "; comment\n" +
		  "[name]\n" +
		  "a = m\n" +
		  "b = n\n";
	  private readonly string INI2 = "" +
		  "[section]\n" +
		  "a = x\n" +
		  "b = y\n";
	  private readonly string INI3 = "" +
		  "[section]\n" +
		  "a = x\n" +
		  "a = y\n";
	  private readonly string INI4 = "" +
		  "[section]\n" +
		  "a=d= = x\n";
	  private const object ANOTHER_TYPE = "";

	  public virtual void test_of_noLists()
	  {
		IniFile test = IniFile.of(CharSource.wrap(INI1));
		Multimap<string, string> keyValues1 = ImmutableListMultimap.of("c", "x", "b", "y", "a", "z");
		Multimap<string, string> keyValues2 = ImmutableListMultimap.of("a", "m", "b", "n");
		assertEquals(test.asMap(), ImmutableMap.of("section", PropertySet.of(keyValues1), "name", PropertySet.of(keyValues2)));

		assertEquals(test.contains("section"), true);
		assertEquals(test.section("section"), PropertySet.of(keyValues1));
		assertEquals(test.section("section").contains("c"), true);
		assertEquals(test.section("section").value("c"), "x");
		assertEquals(test.section("section").valueList("c"), ImmutableList.of("x"));
		assertEquals(test.section("section").contains("b"), true);
		assertEquals(test.section("section").value("b"), "y");
		assertEquals(test.section("section").valueList("b"), ImmutableList.of("y"));
		assertEquals(test.section("section").contains("a"), true);
		assertEquals(test.section("section").value("a"), "z");
		assertEquals(test.section("section").valueList("a"), ImmutableList.of("z"));
		assertEquals(test.section("section").contains("d"), false);
		// order must be retained
		assertEquals(ImmutableList.copyOf(test.section("section").keys()), ImmutableList.of("c", "b", "a"));
		assertEquals(test.section("section").asMultimap(), ImmutableListMultimap.of("c", "x", "b", "y", "a", "z"));

		assertEquals(test.contains("name"), true);
		assertEquals(test.section("name"), PropertySet.of(keyValues2));
		assertEquals(test.section("name").contains("a"), true);
		assertEquals(test.section("name").value("a"), "m");
		assertEquals(test.section("name").valueList("a"), ImmutableList.of("m"));
		assertEquals(test.section("name").contains("b"), true);
		assertEquals(test.section("name").value("b"), "n");
		assertEquals(test.section("name").valueList("b"), ImmutableList.of("n"));
		assertEquals(test.section("name").contains("c"), false);
		assertEquals(ImmutableList.copyOf(test.section("name").keys()), ImmutableList.of("a", "b"));
		assertEquals(test.section("name").asMultimap(), ImmutableListMultimap.of("a", "m", "b", "n"));

		assertEquals(test.contains("unknown"), false);
		assertThrowsIllegalArg(() => test.section("unknown"));
		assertEquals(test.section("section").valueList("unknown"), ImmutableList.of());
		assertThrowsIllegalArg(() => test.section("section").value("unknown"));
		assertEquals(test.ToString(), "{section={c=[x], b=[y], a=[z]}, name={a=[m], b=[n]}}");
	  }

	  public virtual void test_of_list()
	  {
		IniFile test = IniFile.of(CharSource.wrap(INI3));
		Multimap<string, string> keyValues1 = ImmutableListMultimap.of("a", "x", "a", "y");
		assertEquals(test.asMap(), ImmutableMap.of("section", PropertySet.of(keyValues1)));

		assertEquals(test.section("section"), PropertySet.of(keyValues1));
		assertEquals(test.section("section").contains("a"), true);
		assertThrowsIllegalArg(() => test.section("section").value("a"));
		assertEquals(test.section("section").valueList("a"), ImmutableList.of("x", "y"));
		assertEquals(test.section("section").contains("b"), false);
		assertEquals(test.section("section").keys(), ImmutableSet.of("a"));
		assertEquals(test.section("section").asMultimap(), ImmutableListMultimap.of("a", "x", "a", "y"));
		assertEquals(test.ToString(), "{section={a=[x, y]}}");
	  }

	  public virtual void test_of_escaping()
	  {
		IniFile test = IniFile.of(CharSource.wrap(INI4));
		Multimap<string, string> keyValues1 = ImmutableListMultimap.of("a=d=", "x");
		assertEquals(test.asMap(), ImmutableMap.of("section", PropertySet.of(keyValues1)));
	  }

	  public virtual void test_of_propertyNoEquals()
	  {
		IniFile test = IniFile.of(CharSource.wrap("[section]\na\n"));
		Multimap<string, string> keyValues1 = ImmutableListMultimap.of("a", "");
		assertEquals(test.asMap(), ImmutableMap.of("section", PropertySet.of(keyValues1)));

		assertEquals(test.section("section"), PropertySet.of(keyValues1));
		assertEquals(test.section("section").contains("a"), true);
		assertEquals(test.section("section").valueList("a"), ImmutableList.of(""));
		assertEquals(test.section("section").contains("b"), false);
		assertEquals(test.section("section").keys(), ImmutableSet.of("a"));
		assertEquals(test.section("section").asMultimap(), ImmutableListMultimap.of("a", ""));
		assertEquals(test.ToString(), "{section={a=[]}}");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_invalid_propertyAtStart()
	  public virtual void test_of_invalid_propertyAtStart()
	  {
		string invalid = "a = x\n";
		IniFile.of(CharSource.wrap(invalid));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_invalid_badSection()
	  public virtual void test_of_invalid_badSection()
	  {
		string invalid = "" +
			"[section\n" +
			"b\n";
		IniFile.of(CharSource.wrap(invalid));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_invalid_duplicateSection()
	  public virtual void test_of_invalid_duplicateSection()
	  {
		string invalid = "" +
			"[section]\n" +
			"a = y\n" +
			"[section]\n" +
			"b = y\n";
		IniFile.of(CharSource.wrap(invalid));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_invalid_emptyKey()
	  public virtual void test_of_invalid_emptyKey()
	  {
		string invalid = "" +
			"[section]\n" +
			"= y\n";
		IniFile.of(CharSource.wrap(invalid));
	  }

	  public virtual void test_of_ioException()
	  {
		assertThrows(() => IniFile.of(Files.asCharSource(new File("src/test/resources"), StandardCharsets.UTF_8)), typeof(UncheckedIOException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		IniFile a1 = IniFile.of(CharSource.wrap(INI1));
		IniFile a2 = IniFile.of(CharSource.wrap(INI1));
		IniFile b = IniFile.of(CharSource.wrap(INI2));

		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(null), false);
		assertEquals(a1.Equals(ANOTHER_TYPE), false);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void test_equalsHashCode_section()
	  {
		IniFile a1 = IniFile.of(CharSource.wrap(INI1));
		IniFile a2 = IniFile.of(CharSource.wrap(INI1));
		IniFile b = IniFile.of(CharSource.wrap(INI2));

		assertEquals(a1.section("name").Equals(a1.section("name")), true);
		assertEquals(a1.section("name").Equals(a2.section("name")), true);
		assertEquals(a1.section("name").Equals(b.section("section")), false);
		assertEquals(a1.section("name").Equals(null), false);
		assertEquals(a1.section("name").Equals(ANOTHER_TYPE), false);
		assertEquals(a1.section("name").GetHashCode(), a2.section("name").GetHashCode());
	  }

	}

}