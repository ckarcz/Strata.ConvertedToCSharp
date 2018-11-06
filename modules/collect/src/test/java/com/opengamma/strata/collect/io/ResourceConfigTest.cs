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
//	import static com.opengamma.strata.collect.TestHelper.caputureSystemErr;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Multimap = com.google.common.collect.Multimap;

	/// <summary>
	/// Test <seealso cref="ResourceConfig"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResourceConfigTest
	public class ResourceConfigTest
	{

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_orderedResources() throws Exception
	  public virtual void test_orderedResources()
	  {
		IList<ResourceLocator> list = ResourceConfig.orderedResources("TestFile.txt");
		assertEquals(list.Count, 1);
		ResourceLocator test = list[0];
		assertEquals(test.Locator.StartsWith("classpath", StringComparison.Ordinal), true);
		assertEquals(test.Locator.EndsWith("com/opengamma/strata/config/base/TestFile.txt", StringComparison.Ordinal), true);
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.ToString().StartsWith("classpath", StringComparison.Ordinal), true);
		assertEquals(test.ToString().EndsWith("com/opengamma/strata/config/base/TestFile.txt", StringComparison.Ordinal), true);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_orderedResources_notFound() throws Exception
	  public virtual void test_orderedResources_notFound()
	  {
		string captured = caputureSystemErr(() => assertThrows(typeof(System.InvalidOperationException), () => ResourceConfig.orderedResources("NotFound.txt")));
		assertTrue(captured.Contains("No resource files found on the classpath"));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ofChained_chainNextFileTrue()
	  {
		IniFile test = ResourceConfig.combinedIniFile("TestChain1.ini");
		Multimap<string, string> keyValues1 = ImmutableListMultimap.of("a", "z", "b", "y");
		Multimap<string, string> keyValues2 = ImmutableListMultimap.of("m", "n");
		assertEquals(test.asMap(), ImmutableMap.of("one", PropertySet.of(keyValues1), "two", PropertySet.of(keyValues2)));
	  }

	  public virtual void test_ofChained_chainNextFileFalse()
	  {
		IniFile test = ResourceConfig.combinedIniFile("TestChain2.ini");
		Multimap<string, string> keyValues1 = ImmutableListMultimap.of("a", "z");
		Multimap<string, string> keyValues2 = ImmutableListMultimap.of("m", "n");
		assertEquals(test.asMap(), ImmutableMap.of("one", PropertySet.of(keyValues1), "two", PropertySet.of(keyValues2)));
	  }

	  public virtual void test_ofChained_chainToNowhere()
	  {
		IniFile test = ResourceConfig.combinedIniFile("TestChain3.ini");
		Multimap<string, string> keyValues1 = ImmutableListMultimap.of("a", "x", "b", "y");
		assertEquals(test.asMap(), ImmutableMap.of("one", PropertySet.of(keyValues1)));
	  }

	  public virtual void test_ofChained_autoChain()
	  {
		IniFile test = ResourceConfig.combinedIniFile("TestChain4.ini");
		Multimap<string, string> keyValues1 = ImmutableListMultimap.of("a", "z", "b", "y");
		Multimap<string, string> keyValues2 = ImmutableListMultimap.of("m", "n");
		assertEquals(test.asMap(), ImmutableMap.of("one", PropertySet.of(keyValues1), "two", PropertySet.of(keyValues2)));
	  }

	  public virtual void test_ofChained_chainRemoveSections()
	  {
		IniFile test = ResourceConfig.combinedIniFile("TestChain5.ini");
		Multimap<string, string> keyValues1 = ImmutableListMultimap.of("a", "a");
		Multimap<string, string> keyValues2 = ImmutableListMultimap.of("m", "n", "o", "z");
		assertEquals(test.asMap(), ImmutableMap.of("one", PropertySet.of(keyValues1), "two", PropertySet.of(keyValues2)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(ResourceConfig));
	  }

	}

}