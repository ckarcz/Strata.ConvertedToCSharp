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
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using Multimap = com.google.common.collect.Multimap;
	using CharSource = com.google.common.io.CharSource;
	using Files = com.google.common.io.Files;

	/// <summary>
	/// Test <seealso cref="PropertiesFile"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PropertiesFileTest
	public class PropertiesFileTest
	{

	  private readonly string FILE1 = "" +
		  "# comment\n" +
		  "a = x\n" +
		  " \n" +
		  "; comment\n" +
		  "c = z\n" +
		  "b = y\n";
	  private readonly string FILE2 = "" +
		  "a = x\n" +
		  "a = y\n";
	  private readonly string FILE3 = "" +
		  "a=d= = x\n";
	  private const object ANOTHER_TYPE = "";

	  public virtual void test_of_noLists()
	  {
		PropertiesFile test = PropertiesFile.of(CharSource.wrap(FILE1));
		Multimap<string, string> keyValues = ImmutableListMultimap.of("a", "x", "c", "z", "b", "y");
		assertEquals(test.Properties, PropertySet.of(keyValues));
		assertEquals(test.ToString(), "{a=[x], c=[z], b=[y]}");
	  }

	  public virtual void test_of_list()
	  {
		PropertiesFile test = PropertiesFile.of(CharSource.wrap(FILE2));
		Multimap<string, string> keyValues = ImmutableListMultimap.of("a", "x", "a", "y");
		assertEquals(test.Properties, PropertySet.of(keyValues));
		assertEquals(test.ToString(), "{a=[x, y]}");
	  }

	  public virtual void test_of_escaping()
	  {
		PropertiesFile test = PropertiesFile.of(CharSource.wrap(FILE3));
		Multimap<string, string> keyValues1 = ImmutableListMultimap.of("a=d=", "x");
		assertEquals(test.Properties, PropertySet.of(keyValues1));
	  }

	  public virtual void test_of_propertyNoEquals()
	  {
		PropertiesFile test = PropertiesFile.of(CharSource.wrap("b\n"));
		Multimap<string, string> keyValues = ImmutableListMultimap.of("b", "");
		assertEquals(test.Properties, PropertySet.of(keyValues));
		assertEquals(test.ToString(), "{b=[]}");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_invalid_emptyKey()
	  public virtual void test_of_invalid_emptyKey()
	  {
		string invalid = "= y\n";
		PropertiesFile.of(CharSource.wrap(invalid));
	  }

	  public virtual void test_of_ioException()
	  {
		assertThrows(() => PropertiesFile.of(Files.asCharSource(new File("src/test/resources"), StandardCharsets.UTF_8)), typeof(UncheckedIOException));
	  }

	  public virtual void test_of_set()
	  {
		Multimap<string, string> keyValues = ImmutableListMultimap.of("a", "x", "b", "y");
		PropertiesFile test = PropertiesFile.of(PropertySet.of(keyValues));
		assertEquals(test.Properties, PropertySet.of(keyValues));
		assertEquals(test.ToString(), "{a=[x], b=[y]}");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		PropertiesFile a1 = PropertiesFile.of(CharSource.wrap(FILE1));
		PropertiesFile a2 = PropertiesFile.of(CharSource.wrap(FILE1));
		PropertiesFile b = PropertiesFile.of(CharSource.wrap(FILE2));

		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(null), false);
		assertEquals(a1.Equals(ANOTHER_TYPE), false);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	}

}