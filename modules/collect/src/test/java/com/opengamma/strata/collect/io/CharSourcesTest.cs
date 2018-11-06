using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using Charsets = com.google.common.@base.Charsets;
	using CharSource = com.google.common.io.CharSource;

	/// <summary>
	/// Tests <seealso cref="CharSources"/>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CharSourcesTest
	public class CharSourcesTest
	{

	  private readonly string fileName = "src/test/resources/com/opengamma/strata/collect/io/utf16le.txt";

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testPrivateConstructor() throws Exception
	  public virtual void testPrivateConstructor()
	  {
		TestHelper.coverPrivateConstructor(typeof(CharSources));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testOfFileName() throws Exception
	  public virtual void testOfFileName()
	  {
		CharSource charSource = CharSources.ofFileName(fileName);
		assertEquals(charSource.readFirstLine(), "H\u0000e\u0000l\u0000l\u0000o\u0000");
		assertEquals(charSource.length(), 10);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testOfFileNameWithCharset() throws Exception
	  public virtual void testOfFileNameWithCharset()
	  {
		CharSource charSource = CharSources.ofFileName(fileName, Charsets.UTF_16LE);
		assertEquals(charSource.readFirstLine(), "Hello");
		assertEquals(charSource.length(), 5);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testOfFile() throws Exception
	  public virtual void testOfFile()
	  {
		CharSource charSource = CharSources.ofFile(new File(fileName));
		assertEquals(charSource.readFirstLine(), "H\u0000e\u0000l\u0000l\u0000o\u0000");
		assertEquals(charSource.length(), 10);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testOfFileWithCharset() throws Exception
	  public virtual void testOfFileWithCharset()
	  {
		CharSource charSource = CharSources.ofFile(new File(fileName), Charsets.UTF_16LE);
		assertEquals(charSource.readFirstLine(), "Hello");
		assertEquals(charSource.length(), 5);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testOfPath() throws Exception
	  public virtual void testOfPath()
	  {
		CharSource charSource = CharSources.ofPath(Paths.get(fileName));
		assertEquals(charSource.readFirstLine(), "H\u0000e\u0000l\u0000l\u0000o\u0000");
		assertEquals(charSource.length(), 10);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testOfPathWithCharset() throws Exception
	  public virtual void testOfPathWithCharset()
	  {
		CharSource charSource = CharSources.ofPath(Paths.get(fileName), Charsets.UTF_16LE);
		assertEquals(charSource.readFirstLine(), "Hello");
		assertEquals(charSource.length(), 5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testOfUrl() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testOfUrl()
	  {
		string fullPathToFile = "file:///" + System.getProperty("user.dir") + "/" + fileName;
		URL url = new URL(fullPathToFile);
		CharSource charSource = CharSources.ofUrl(url);
		assertEquals(charSource.readFirstLine(), "H\u0000e\u0000l\u0000l\u0000o\u0000");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testOfUrlWithCharset() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testOfUrlWithCharset()
	  {
		string fullPathToFile = "file:///" + System.getProperty("user.dir") + "/" + fileName;
		URL url = new URL(fullPathToFile);
		CharSource charSource = CharSources.ofUrl(url, Charsets.UTF_16LE);
		assertEquals(charSource.readFirstLine(), "Hello");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testOfContentString() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testOfContentString()
	  {
		CharSource charSource = CharSources.ofContent("H\u0000e\u0000l\u0000l\u0000o\u0000");
		assertEquals(charSource.readFirstLine(), "H\u0000e\u0000l\u0000l\u0000o\u0000");
		assertEquals(charSource.length(), 10);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testOfContentByteArray() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testOfContentByteArray()
	  {
		sbyte[] inputText = "H\u0000e\u0000l\u0000l\u0000o\u0000".GetBytes(Encoding.UTF8);
		CharSource charSource = CharSources.ofContent(inputText);
		assertEquals(charSource.readFirstLine(), "H\u0000e\u0000l\u0000l\u0000o\u0000");
		assertEquals(charSource.length(), 10);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testOfContentByteArrayWithCharset() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void testOfContentByteArrayWithCharset()
	  {
		sbyte[] inputText = "H\u0000e\u0000l\u0000l\u0000o\u0000".GetBytes(Encoding.UTF8);
		CharSource charSource = CharSources.ofContent(inputText, Charsets.UTF_16LE);
		assertEquals(charSource.readFirstLine(), "Hello");
		assertEquals(charSource.length(), 5);
	  }
	}

}