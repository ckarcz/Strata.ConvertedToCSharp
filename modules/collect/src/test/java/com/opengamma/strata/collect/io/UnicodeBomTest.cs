using System;
using System.IO;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertUtilityClass;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ByteSource = com.google.common.io.ByteSource;
	using CharSource = com.google.common.io.CharSource;
	using CharStreams = com.google.common.io.CharStreams;

	/// <summary>
	/// Test <seealso cref="UnicodeBom"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class UnicodeBomTest
	public class UnicodeBomTest
	{

	  private const sbyte X_00 = (sbyte)'\u0000';
	  private static readonly sbyte X_FE = unchecked((sbyte) 0xFE);
	  private static readonly sbyte X_FF = unchecked((sbyte) 0xFF);

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toString_noBomUtf8() throws java.io.IOException
	  public virtual void test_toString_noBomUtf8()
	  {
		sbyte[] bytes = new sbyte[] {(sbyte)'H', (sbyte)'e', (sbyte)'l', (sbyte)'l', (sbyte)'o'};
		string str = UnicodeBom.ToString(bytes);
		assertEquals(str, "Hello");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toString_bomUtf8() throws java.io.IOException
	  public virtual void test_toString_bomUtf8()
	  {
		sbyte[] bytes = new sbyte[] {unchecked((sbyte) 0xEF), unchecked((sbyte) 0xBB), unchecked((sbyte) 0xBF), (sbyte)'H', (sbyte)'e', (sbyte)'l', (sbyte)'l', (sbyte)'o'};
		string str = UnicodeBom.ToString(bytes);
		assertEquals(str, "Hello");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toString_bomUtf16BE() throws java.io.IOException
	  public virtual void test_toString_bomUtf16BE()
	  {
		sbyte[] bytes = new sbyte[] {X_FE, X_FF, X_00, (sbyte)'H', X_00, (sbyte)'e', X_00, (sbyte)'l', X_00, (sbyte)'l', X_00, (sbyte)'o'};
		string str = UnicodeBom.ToString(bytes);
		assertEquals(str, "Hello");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toString_bomUtf16LE() throws java.io.IOException
	  public virtual void test_toString_bomUtf16LE()
	  {
		sbyte[] bytes = new sbyte[] {X_FF, X_FE, (sbyte)'H', X_00, (sbyte)'e', X_00, (sbyte)'l', X_00, (sbyte)'l', X_00, (sbyte)'o', X_00};
		string str = UnicodeBom.ToString(bytes);
		assertEquals(str, "Hello");
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toCharSource_noBomUtf8() throws java.io.IOException
	  public virtual void test_toCharSource_noBomUtf8()
	  {
		sbyte[] bytes = new sbyte[] {(sbyte)'H', (sbyte)'e', (sbyte)'l', (sbyte)'l', (sbyte)'o'};
		ByteSource byteSource = ByteSource.wrap(bytes);
		CharSource charSource = UnicodeBom.toCharSource(byteSource);
		string str = charSource.read();
		assertEquals(str, "Hello");
		assertEquals(charSource.asByteSource(StandardCharsets.UTF_8), byteSource);
		assertEquals(charSource.ToString().StartsWith("UnicodeBom", StringComparison.Ordinal), true);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toReader_noBomUtf8() throws java.io.IOException
	  public virtual void test_toReader_noBomUtf8()
	  {
		sbyte[] bytes = new sbyte[] {(sbyte)'H', (sbyte)'e', (sbyte)'l', (sbyte)'l', (sbyte)'o'};
		Reader reader = UnicodeBom.toReader(new MemoryStream(bytes));
		string str = CharStreams.ToString(reader);
		assertEquals(str, "Hello");
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toReader_bomUtf8() throws java.io.IOException
	  public virtual void test_toReader_bomUtf8()
	  {
		sbyte[] bytes = new sbyte[] {unchecked((sbyte) 0xEF), unchecked((sbyte) 0xBB), unchecked((sbyte) 0xBF), (sbyte)'H', (sbyte)'e', (sbyte)'l', (sbyte)'l', (sbyte)'o'};
		Reader reader = UnicodeBom.toReader(new MemoryStream(bytes));
		string str = CharStreams.ToString(reader);
		assertEquals(str, "Hello");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toReader_bomUtf16BE() throws java.io.IOException
	  public virtual void test_toReader_bomUtf16BE()
	  {
		sbyte[] bytes = new sbyte[] {X_FE, X_FF, X_00, (sbyte)'H', X_00, (sbyte)'e', X_00, (sbyte)'l', X_00, (sbyte)'l', X_00, (sbyte)'o'};
		Reader reader = UnicodeBom.toReader(new MemoryStream(bytes));
		string str = CharStreams.ToString(reader);
		assertEquals(str, "Hello");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toReader_bomUtf16BE_short() throws java.io.IOException
	  public virtual void test_toReader_bomUtf16BE_short()
	  {
		sbyte[] bytes = new sbyte[] {X_FE, X_FF};
		Reader reader = UnicodeBom.toReader(new MemoryStream(bytes));
		string str = CharStreams.ToString(reader);
		assertEquals(str, "");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toReader_almostBomUtf16BE() throws java.io.IOException
	  public virtual void test_toReader_almostBomUtf16BE()
	  {
		sbyte[] bytes = new sbyte[] {X_FE, X_00};
		Reader reader = UnicodeBom.toReader(new MemoryStream(bytes));
		string str = CharStreams.ToString(reader);
		assertEquals(str, StringHelper.NewString(bytes, StandardCharsets.UTF_8));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toReader_bomUtf16LE() throws java.io.IOException
	  public virtual void test_toReader_bomUtf16LE()
	  {
		sbyte[] bytes = new sbyte[] {X_FF, X_FE, (sbyte)'H', X_00, (sbyte)'e', X_00, (sbyte)'l', X_00, (sbyte)'l', X_00, (sbyte)'o', X_00};
		Reader reader = UnicodeBom.toReader(new MemoryStream(bytes));
		string str = CharStreams.ToString(reader);
		assertEquals(str, "Hello");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toReader_bomUtf16LE_short() throws java.io.IOException
	  public virtual void test_toReader_bomUtf16LE_short()
	  {
		sbyte[] bytes = new sbyte[] {X_FF, X_FE};
		Reader reader = UnicodeBom.toReader(new MemoryStream(bytes));
		string str = CharStreams.ToString(reader);
		assertEquals(str, "");
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toReader_almostBomUtf16LE() throws java.io.IOException
	  public virtual void test_toReader_almostBomUtf16LE()
	  {
		sbyte[] bytes = new sbyte[] {X_FF, X_00};
		Reader reader = UnicodeBom.toReader(new MemoryStream(bytes));
		string str = CharStreams.ToString(reader);
		assertEquals(str, StringHelper.NewString(bytes, StandardCharsets.UTF_8));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_toReader_notBomUtf16LE() throws java.io.IOException
	  public virtual void test_toReader_notBomUtf16LE()
	  {
		sbyte[] bytes = new sbyte[] {X_00, X_FE, (sbyte)'M', (sbyte)'P'};
		Reader reader = UnicodeBom.toReader(new MemoryStream(bytes));
		string str = CharStreams.ToString(reader);
		assertEquals(str, StringHelper.NewString(bytes, StandardCharsets.UTF_8));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_validUtilityClass()
	  {
		assertUtilityClass(typeof(UnicodeBom));
	  }

	}

}