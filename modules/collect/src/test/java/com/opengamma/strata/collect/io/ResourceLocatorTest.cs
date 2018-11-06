using System;

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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Resources = com.google.common.io.Resources;

	/// <summary>
	/// Test <seealso cref="ResourceLocator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResourceLocatorTest
	public class ResourceLocatorTest
	{

	  private const object ANOTHER_TYPE = "";

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_of_filePrefixed() throws Exception
	  public virtual void test_of_filePrefixed()
	  {
		ResourceLocator test = ResourceLocator.of("file:src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
		assertEquals(test.Locator, "file:src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.ToString(), "file:src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_of_fileNoPrefix() throws Exception
	  public virtual void test_of_fileNoPrefix()
	  {
		ResourceLocator test = ResourceLocator.of("src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
		assertEquals(test.Locator, "file:src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.ToString(), "file:src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_of_classpath() throws Exception
	  public virtual void test_of_classpath()
	  {
		ResourceLocator test = ResourceLocator.of("classpath:com/opengamma/strata/collect/io/TestFile.txt");
		assertTrue(test.Locator.StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.Locator.EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertTrue(test.ToString().StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.ToString().EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_of_invalid() throws Exception
	  public virtual void test_of_invalid()
	  {
		assertThrowsIllegalArg(() => ResourceLocator.of("classpath:http:https:file:/foobar.txt"));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_ofFile() throws Exception
	  public virtual void test_ofFile()
	  {
		File file = new File("src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
		ResourceLocator test = ResourceLocator.ofFile(file);
		assertEquals(test.Locator, "file:src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.ToString(), "file:src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_ofPath() throws Exception
	  public virtual void test_ofPath()
	  {
		Path path = Paths.get("src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
		ResourceLocator test = ResourceLocator.ofPath(path);
		assertEquals(test.Locator.Replace('\\', '/'), "file:src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.ToString().Replace('\\', '/'), "file:src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_ofPath_zipFile() throws Exception
	  public virtual void test_ofPath_zipFile()
	  {
		Path path = Paths.get("src/test/resources/com/opengamma/strata/collect/io/TestFile.zip");
		ResourceLocator test = ResourceLocator.ofPath(path);
		assertEquals(test.Locator.Replace('\\', '/'), "file:src/test/resources/com/opengamma/strata/collect/io/TestFile.zip");
		sbyte[] read = test.ByteSource.read();
		assertEquals(read[0], 80); // these are the standard header of a zip file
		assertEquals(read[1], 75);
		assertEquals(read[2], 3);
		assertEquals(read[3], 4);
		assertEquals(test.ToString().Replace('\\', '/'), "file:src/test/resources/com/opengamma/strata/collect/io/TestFile.zip");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_ofPath_fileInZipFile() throws Exception
	  public virtual void test_ofPath_fileInZipFile()
	  {
		Path zip = Paths.get("src/test/resources/com/opengamma/strata/collect/io/TestFile.zip");
		using (FileSystem fs = FileSystems.newFileSystem(zip, null))
		{
		  Path path = fs.getPath("TestFile.txt").toAbsolutePath();
		  ResourceLocator test = ResourceLocator.ofPath(path);
		  string locator = test.Locator;
		  assertTrue(locator.StartsWith("url:jar:file:", StringComparison.Ordinal));
		  assertTrue(locator.EndsWith("src/test/resources/com/opengamma/strata/collect/io/TestFile.zip!/TestFile.txt", StringComparison.Ordinal));
		  assertEquals(test.ByteSource.read()[0], 'H');
		  assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		  assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		  assertEquals(test.ToString(), locator);
		}
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_ofUrl() throws Exception
	  public virtual void test_ofUrl()
	  {
		File file = new File("src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
		URL url = file.toURI().toURL();
		ResourceLocator test = ResourceLocator.ofUrl(url);
		string locator = test.Locator;
		assertTrue(locator.StartsWith("url:file:", StringComparison.Ordinal));
		assertTrue(locator.EndsWith("src/test/resources/com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.ToString(), locator);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_ofClasspath_absolute() throws Exception
	  public virtual void test_ofClasspath_absolute()
	  {
		ResourceLocator test = ResourceLocator.ofClasspath("/com/opengamma/strata/collect/io/TestFile.txt");
		assertTrue(test.Locator.StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.Locator.EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertTrue(test.ToString().StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.ToString().EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_ofClasspath_relativeConvertedToAbsolute() throws Exception
	  public virtual void test_ofClasspath_relativeConvertedToAbsolute()
	  {
		ResourceLocator test = ResourceLocator.ofClasspath("com/opengamma/strata/collect/io/TestFile.txt");
		assertTrue(test.Locator.StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.Locator.EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertTrue(test.ToString().StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.ToString().EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_ofClasspath_withClass_absolute() throws Exception
	  public virtual void test_ofClasspath_withClass_absolute()
	  {
		ResourceLocator test = ResourceLocator.ofClasspath(typeof(ResourceLocator), "/com/opengamma/strata/collect/io/TestFile.txt");
		assertTrue(test.Locator.StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.Locator.EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertTrue(test.ToString().StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.ToString().EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_ofClasspath_withClass_relative() throws Exception
	  public virtual void test_ofClasspath_withClass_relative()
	  {
		ResourceLocator test = ResourceLocator.ofClasspath(typeof(ResourceLocator), "TestFile.txt");
		assertTrue(test.Locator.StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.Locator.EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertTrue(test.ToString().StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.ToString().EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_ofClasspathUrl() throws Exception
	  public virtual void test_ofClasspathUrl()
	  {
		URL url = Resources.getResource("com/opengamma/strata/collect/io/TestFile.txt");
		ResourceLocator test = ResourceLocator.ofClasspathUrl(url);
		assertTrue(test.Locator.StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.Locator.EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
		assertEquals(test.ByteSource.read()[0], 'H');
		assertEquals(test.CharSource.readLines(), ImmutableList.of("HelloWorld"));
		assertEquals(test.getCharSource(StandardCharsets.UTF_8).readLines(), ImmutableList.of("HelloWorld"));
		assertTrue(test.ToString().StartsWith("classpath", StringComparison.Ordinal));
		assertTrue(test.ToString().EndsWith("com/opengamma/strata/collect/io/TestFile.txt", StringComparison.Ordinal));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_equalsHashCode() throws Exception
	  public virtual void test_equalsHashCode()
	  {
		File file1 = new File("src/test/resources/com/opengamma/strata/collect/io/TestFile.txt");
		File file2 = new File("src/test/resources/com/opengamma/strata/collect/io/Other.txt");
		ResourceLocator a1 = ResourceLocator.ofFile(file1);
		ResourceLocator a2 = ResourceLocator.ofFile(file1);
		ResourceLocator b = ResourceLocator.ofFile(file2);

		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(null), false);
		assertEquals(a1.Equals(ANOTHER_TYPE), false);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	}

}