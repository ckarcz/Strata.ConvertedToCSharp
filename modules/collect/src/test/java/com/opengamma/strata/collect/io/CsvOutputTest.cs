using System;
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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;


	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="CsvOutput"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CsvOutputTest
	public class CsvOutputTest
	{

	  private static readonly string LINE_SEP = Environment.NewLine;
	  private const string LINE_ITEM_SEP_COMMA = ",";
	  private const string LINE_ITEM_SEP_TAB = "\t";

	  //-------------------------------------------------------------------------
	  public virtual void test_standard_writeLines_alwaysQuote()
	  {
		IList<IList<string>> rows = Arrays.asList(Arrays.asList("a", "x"), Arrays.asList("b", "y"));
		StringBuilder buf = new StringBuilder();
		CsvOutput.standard(buf, "\n").writeLines(rows, true);
		assertEquals(buf.ToString(), "\"a\",\"x\"\n\"b\",\"y\"\n");
	  }

	  public virtual void test_standard_writeLines_selectiveQuote_commaAndQuote()
	  {
		IList<IList<string>> rows = Arrays.asList(Arrays.asList("a", "1,000"), Arrays.asList("b\"c", "y"));
		StringBuilder buf = new StringBuilder();
		CsvOutput.standard(buf, "\n", LINE_ITEM_SEP_COMMA).writeLines(rows, false);
		assertEquals(buf.ToString(), "a,\"1,000\"\n\"b\"\"c\",y\n");
	  }

	  public virtual void test_standard_writeLines_selectiveQuote_trimmable()
	  {
		IList<IList<string>> rows = Arrays.asList(Arrays.asList("a", " x"), Arrays.asList("b ", "y"));
		StringBuilder buf = new StringBuilder();
		CsvOutput.standard(buf, "\n", LINE_ITEM_SEP_COMMA).writeLines(rows, false);
		assertEquals(buf.ToString(), "a,\" x\"\n\"b \",y\n");
	  }

	  public virtual void test_standard_writeLines_systemNewLine()
	  {
		IList<IList<string>> rows = Arrays.asList(Arrays.asList("a", "x"), Arrays.asList("b", "y"));
		StringBuilder buf = new StringBuilder();
		CsvOutput.standard(buf).writeLines(rows, false);
		assertEquals(buf.ToString(), "a,x" + LINE_SEP + "b,y" + LINE_SEP);
	  }

	  public virtual void test_standard_writeLine_selectiveQuote()
	  {
		StringBuilder buf = new StringBuilder();
		CsvOutput.standard(buf, "\n", LINE_ITEM_SEP_COMMA).writeLine(Arrays.asList("a", "1,000"));
		assertEquals(buf.ToString(), "a,\"1,000\"\n");
	  }

	  public virtual void test_standard_writeLines_tab_separated()
	  {
		StringBuilder buf = new StringBuilder();
		CsvOutput.standard(buf, "\n", LINE_ITEM_SEP_TAB).writeLine(Arrays.asList("a", "1,000"));
		assertEquals(buf.ToString(), "a\t\"1,000\"\n");
	  }

	  public virtual void test_standard_expressionPrefix()
	  {
		StringBuilder buf = new StringBuilder();
		CsvOutput.standard(buf, "\n").writeLine(Arrays.asList("=cmd", "+cmd", "-cmd", "@cmd", ""));
		assertEquals(buf.ToString(), "\"=cmd\",+cmd,-cmd,\"@cmd\",\n");
	  }

	  public virtual void test_standard_expressionPrefixNumbers()
	  {
		StringBuilder buf = new StringBuilder();
		CsvOutput.standard(buf, "\n").writeLine(Arrays.asList("+8", "-7", "+8-7", "-7+8", "NaN", "-Infinity"));
		assertEquals(buf.ToString(), "+8,-7,+8-7,-7+8,NaN,-Infinity\n");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_safe_writeLines_systemNewLine()
	  {
		IList<IList<string>> rows = Arrays.asList(Arrays.asList("a", "=x"), Arrays.asList("b", "y"));
		StringBuilder buf = new StringBuilder();
		CsvOutput.safe(buf).writeLines(rows, false);
		assertEquals(buf.ToString(), "a,=\"=x\"" + LINE_SEP + "b,y" + LINE_SEP);
	  }

	  public virtual void test_safe_expressionPrefix()
	  {
		StringBuilder buf = new StringBuilder();
		CsvOutput.safe(buf, "\n").writeLine(Arrays.asList("=cmd", "+cmd", "-cmd", "@cmd"));
		assertEquals(buf.ToString(), "=\"=cmd\",=\"+cmd\",=\"-cmd\",=\"@cmd\"\n");
	  }

	  public virtual void test_safe_expressionPrefixNumbers()
	  {
		StringBuilder buf = new StringBuilder();
		CsvOutput.safe(buf, "\n", LINE_ITEM_SEP_COMMA).writeLine(Arrays.asList("+8", "-7", "+8-7", "-7+8", "NaN", "-Infinity"));
		assertEquals(buf.ToString(), "+8,-7,=\"+8-7\",=\"-7+8\",NaN,=\"-Infinity\"\n");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_writeCell()
	  {
		StringBuilder buf = new StringBuilder();
		CsvOutput.standard(buf, "\n").writeCell("a").writeCell("x").writeNewLine().writeCell("b", true).writeCell("y", true).writeNewLine();
		assertEquals(buf.ToString(), "a,x\n\"b\",\"y\"\n");
	  }

	  public virtual void test_mixed()
	  {
		IList<string> row = Arrays.asList("x", "y");
		StringBuilder buf = new StringBuilder();
		CsvOutput.standard(buf, "\n").writeCell("a").writeCell("b").writeLine(row);
		assertEquals(buf.ToString(), "a,b,x,y\n");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_exception()
	  {
		Appendable throwingAppendable = new AppendableAnonymousInnerClass(this);

		CsvOutput output = CsvOutput.standard(throwingAppendable, "\n");
		assertThrows(typeof(UncheckedIOException), () => output.writeCell("a"));
		assertThrows(typeof(UncheckedIOException), () => output.writeNewLine());
	  }

	  private class AppendableAnonymousInnerClass : Appendable
	  {
		  private readonly CsvOutputTest outerInstance;

		  public AppendableAnonymousInnerClass(CsvOutputTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public Appendable append(CharSequence csq, int start, int end) throws java.io.IOException
		  public override Appendable append(CharSequence csq, int start, int end)
		  {
			throw new IOException();
		  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public Appendable append(char c) throws java.io.IOException
		  public override Appendable append(char c)
		  {
			throw new IOException();
		  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public Appendable append(CharSequence csq) throws java.io.IOException
		  public override Appendable append(CharSequence csq)
		  {
			throw new IOException();
		  }
	  }

	}

}