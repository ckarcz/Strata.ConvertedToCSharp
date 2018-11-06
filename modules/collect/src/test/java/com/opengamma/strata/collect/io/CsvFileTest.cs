using System.Collections.Generic;

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
//	import static org.testng.Assert.assertNotNull;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CharSource = com.google.common.io.CharSource;
	using Files = com.google.common.io.Files;

	/// <summary>
	/// Test <seealso cref="CsvFile"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CsvFileTest
	public class CsvFileTest
	{

	  private const object ANOTHER_TYPE = "";

	  private readonly string CSV1 = "" +
		  "h1,h2\n" +
		  "r11,r12\n" +
		  "r21,r22\n" +
		  "r31,";

	  private readonly string CSV1T = "" +
		  "h1\th2\n" +
		  "r11\tr12\n" +
		  "r21\tr22";

	  private readonly string CSV2 = "" +
		  "h1,h2\n" +
		  "#r11,r12\n" +
		  ";r11,r12\n" +
		  "\n" +
		  "r21,r22\n";

	  private readonly string CSV3 = "" +
		  "r11,r12\n" +
		  ",\n" +
		  "r21,r22\n";

	  private readonly string CSV4 = "" +
		  "\"alpha\",\"be, \"\"at\"\", one\"\n" +
		  "\"alpha\"\",\"be\"\"\", \"\"at\"\", one\"\n" +
		  "r21,\" r22 \"\n";

	  private readonly string CSV4B = "" +
		  "=\"alpha\",=\"be, \"\"at\"\", one\"\n" +
		  "=\"alpha\"\",\"be\"\"\", =\"\"at\"\", one\"\n" +
		  "r21,=\" r22 \"\n";

	  private readonly string CSV5 = "" +
		  "a,b,c,b,c\n" +
		  "aa,b1,c1,b2,c2\n";

	  private readonly string CSV6 = "" +
		  "a,b,c\n" +
		  "r11\n" +
		  "r21,r22";

	  private readonly string CSV7 = "" +
		  "# Comment about the file\n" +
		  "h1,h2\n" +
		  "r1,r2\n";

	  //-------------------------------------------------------------------------
	  public virtual void test_of_ioException()
	  {
		assertThrows(() => CsvFile.of(Files.asCharSource(new File("src/test/resources"), StandardCharsets.UTF_8), false), typeof(UncheckedIOException));
	  }

	  public virtual void test_of_empty_no_header()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(""), false);
		assertEquals(csvFile.headers().size(), 0);
		assertEquals(csvFile.rowCount(), 0);
		assertEquals(csvFile.containsHeader("Foo"), false);
		assertEquals(csvFile.containsHeader(Pattern.compile("Foo")), false);
	  }

	  public virtual void test_of_empty_with_header()
	  {
		assertThrowsIllegalArg(() => CsvFile.of(CharSource.wrap(""), true));
	  }

	  public virtual void test_of_simple_no_header()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV1), false);
		assertEquals(csvFile.headers().size(), 0);
		assertEquals(csvFile.rowCount(), 4);
		assertEquals(csvFile.containsHeader("Foo"), false);
		assertEquals(csvFile.containsHeader(Pattern.compile("Foo")), false);
		assertEquals(csvFile.row(0).lineNumber(), 1);
		assertEquals(csvFile.row(1).lineNumber(), 2);
		assertEquals(csvFile.row(2).lineNumber(), 3);
		assertEquals(csvFile.row(3).lineNumber(), 4);

		assertEquals(csvFile.row(0).headers().size(), 0);
		assertEquals(csvFile.row(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).field(0), "h1");
		assertEquals(csvFile.row(0).field(1), "h2");
		assertEquals(csvFile.row(1).headers().size(), 0);
		assertEquals(csvFile.row(1).fieldCount(), 2);
		assertEquals(csvFile.row(1).field(0), "r11");
		assertEquals(csvFile.row(1).field(1), "r12");
		assertEquals(csvFile.row(2).headers().size(), 0);
		assertEquals(csvFile.row(2).fieldCount(), 2);
		assertEquals(csvFile.row(2).field(0), "r21");
		assertEquals(csvFile.row(2).field(1), "r22");

		assertEquals(csvFile.row(0).subRow(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).subRow(1).fieldCount(), 1);
		assertEquals(csvFile.row(0).subRow(2).fieldCount(), 0);
	  }

	  public virtual void test_of_simple_no_header_tabs()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV1T), false, '\t');
		assertEquals(csvFile.headers().size(), 0);
		assertEquals(csvFile.containsHeader("Foo"), false);
		assertEquals(csvFile.containsHeader(Pattern.compile("Foo")), false);
		assertEquals(csvFile.rowCount(), 3);
		assertEquals(csvFile.row(0).lineNumber(), 1);
		assertEquals(csvFile.row(1).lineNumber(), 2);
		assertEquals(csvFile.row(2).lineNumber(), 3);

		assertEquals(csvFile.row(0).headers().size(), 0);
		assertEquals(csvFile.row(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).field(0), "h1");
		assertEquals(csvFile.row(0).field(1), "h2");
		assertEquals(csvFile.row(1).headers().size(), 0);
		assertEquals(csvFile.row(1).fieldCount(), 2);
		assertEquals(csvFile.row(1).field(0), "r11");
		assertEquals(csvFile.row(1).field(1), "r12");
		assertEquals(csvFile.row(2).headers().size(), 0);
		assertEquals(csvFile.row(2).fieldCount(), 2);
		assertEquals(csvFile.row(2).field(0), "r21");
		assertEquals(csvFile.row(2).field(1), "r22");
	  }

	  public virtual void test_of_simple_with_header()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV1), true);
		assertEquals(csvFile.containsHeader("Foo"), false);
		assertEquals(csvFile.containsHeader("h1"), true);
		assertEquals(csvFile.containsHeader(Pattern.compile("Foo")), false);
		assertEquals(csvFile.containsHeader(Pattern.compile("h[0-9]")), true);
		ImmutableList<string> headers = csvFile.headers();
		assertEquals(headers.size(), 2);
		assertEquals(headers.get(0), "h1");
		assertEquals(headers.get(1), "h2");
		assertEquals(csvFile.rowCount(), 3);
		assertEquals(csvFile.row(0).lineNumber(), 2);
		assertEquals(csvFile.row(1).lineNumber(), 3);
		assertEquals(csvFile.row(2).lineNumber(), 4);

		assertEquals(csvFile.row(0).headers(), headers);
		assertEquals(csvFile.row(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).field(0), "r11");
		assertEquals(csvFile.row(0).field(1), "r12");
		assertEquals(csvFile.row(1).headers(), headers);
		assertEquals(csvFile.row(1).fieldCount(), 2);
		assertEquals(csvFile.row(1).field(0), "r21");
		assertEquals(csvFile.row(1).field(1), "r22");

		assertEquals(csvFile.row(0).getField("h1"), "r11");
		assertEquals(csvFile.row(0).getField("h2"), "r12");
		assertEquals(csvFile.row(1).getField("h1"), "r21");
		assertEquals(csvFile.row(1).getField("h2"), "r22");
		assertThrowsIllegalArg(() => csvFile.row(0).getField("zzz"));

		assertEquals(csvFile.row(0).getValue("h1"), "r11");
		assertEquals(csvFile.row(0).getValue("h2"), "r12");
		assertEquals(csvFile.row(1).getValue("h1"), "r21");
		assertEquals(csvFile.row(1).getValue("h2"), "r22");
		assertThrowsIllegalArg(() => csvFile.row(0).getValue("zzz"));
		assertThrowsIllegalArg(() => csvFile.row(2).getValue("h2"));

		assertEquals(csvFile.row(0).findField("h1"), ("r11"));
		assertEquals(csvFile.row(0).findField("h2"), ("r12"));
		assertEquals(csvFile.row(1).findField("h1"), ("r21"));
		assertEquals(csvFile.row(1).findField("h2"), ("r22"));
		assertEquals(csvFile.row(0).findField("zzz"), null);

		assertEquals(csvFile.row(0).findValue("h1"), ("r11"));
		assertEquals(csvFile.row(0).findValue("h2"), ("r12"));
		assertEquals(csvFile.row(1).findValue("h1"), ("r21"));
		assertEquals(csvFile.row(1).findValue("h2"), ("r22"));
		assertEquals(csvFile.row(0).findValue("zzz"), null);
		assertEquals(csvFile.row(2).findValue("h2"), null);

		assertEquals(csvFile.row(0).getField(Pattern.compile("h[13]")), "r11");
		assertEquals(csvFile.row(0).getField(Pattern.compile("h[24]")), "r12");
		assertThrowsIllegalArg(() => csvFile.row(0).getField(Pattern.compile("zzz")));

		assertEquals(csvFile.row(0).getValue(Pattern.compile("h[13]")), "r11");
		assertEquals(csvFile.row(0).getValue(Pattern.compile("h[24]")), "r12");
		assertThrowsIllegalArg(() => csvFile.row(0).getValue(Pattern.compile("zzz")));
		assertThrowsIllegalArg(() => csvFile.row(2).getValue(Pattern.compile("h2")));

		assertEquals(csvFile.row(0).findField(Pattern.compile("h[13]")), ("r11"));
		assertEquals(csvFile.row(0).findField(Pattern.compile("h[24]")), ("r12"));
		assertEquals(csvFile.row(0).findField(Pattern.compile("zzz")), null);

		assertEquals(csvFile.row(0).findValue(Pattern.compile("h[13]")), ("r11"));
		assertEquals(csvFile.row(0).findValue(Pattern.compile("h[24]")), ("r12"));
		assertEquals(csvFile.row(0).findValue(Pattern.compile("zzz")), null);
		assertEquals(csvFile.row(2).findValue(Pattern.compile("h2")), null);

		assertEquals(csvFile.row(0).subRow(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).subRow(1).fieldCount(), 1);
		assertEquals(csvFile.row(0).subRow(2).fieldCount(), 0);

		assertEquals(csvFile.row(0).subRow(0, 0).fieldCount(), 0);
		assertEquals(csvFile.row(0).subRow(0, 1).fieldCount(), 1);
		assertEquals(csvFile.row(0).subRow(2, 2).fieldCount(), 0);
	  }

	  public virtual void test_of_duplicate_headers()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV5), true);
		assertEquals(csvFile.headers(), ImmutableList.of("a", "b", "c", "b", "c"));
		assertEquals(csvFile.containsHeader("Foo"), false);
		assertEquals(csvFile.containsHeader("a"), true);
		assertEquals(csvFile.row(0).getField("a"), "aa");
		assertEquals(csvFile.row(0).getField("b"), "b1");
		assertEquals(csvFile.row(0).getField("c"), "c1");

		assertEquals(csvFile.row(0).subRow(1, 3).getField("b"), "b1");
		assertEquals(csvFile.row(0).subRow(1, 3).getField("c"), "c1");
		assertEquals(csvFile.row(0).subRow(3).getField("b"), "b2");
		assertEquals(csvFile.row(0).subRow(3).getField("c"), "c2");
	  }

	  public virtual void test_of_short_data_row()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV6), true);
		assertEquals(csvFile.headers(), ImmutableList.of("a", "b", "c"));
		assertEquals(csvFile.row(0).getField("a"), "r11");
		assertEquals(csvFile.row(0).getField("b"), "");
		assertEquals(csvFile.row(0).getField("c"), "");
		assertEquals(csvFile.row(0).field(0), "r11");
		assertEquals(csvFile.row(0).field(1), "");
		assertEquals(csvFile.row(0).field(2), "");
		assertThrows(() => csvFile.row(0).field(4), typeof(System.IndexOutOfRangeException));

		assertEquals(csvFile.row(1).getField("a"), "r21");
		assertEquals(csvFile.row(1).getField("b"), "r22");
		assertEquals(csvFile.row(1).getField("c"), "");
	  }

	  public virtual void test_of_comment_blank_no_header()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV2), false);
		assertEquals(csvFile.headers().size(), 0);
		assertEquals(csvFile.rowCount(), 2);
		assertEquals(csvFile.row(0).lineNumber(), 1);
		assertEquals(csvFile.row(1).lineNumber(), 5);

		assertEquals(csvFile.row(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).field(0), "h1");
		assertEquals(csvFile.row(0).field(1), "h2");
		assertEquals(csvFile.row(1).fieldCount(), 2);
		assertEquals(csvFile.row(1).field(0), "r21");
		assertEquals(csvFile.row(1).field(1), "r22");
	  }

	  public virtual void test_of_comment_blank_with_header()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV2), true);
		ImmutableList<string> headers = csvFile.headers();
		assertEquals(headers.size(), 2);
		assertEquals(headers.get(0), "h1");
		assertEquals(headers.get(1), "h2");
		assertEquals(csvFile.rows().size(), 1);
		assertEquals(csvFile.rowCount(), 1);
		assertEquals(csvFile.row(0).lineNumber(), 5);

		assertEquals(csvFile.row(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).field(0), "r21");
		assertEquals(csvFile.row(0).field(1), "r22");
		assertEquals(csvFile.rows().get(0), csvFile.row(0));
	  }

	  public virtual void test_of_simple_no_header_access_by_field()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV1), false);
		assertEquals(csvFile.row(0).findField("h1"), null);
		assertThrowsIllegalArg(() => csvFile.row(0).getField("h1"));
	  }

	  public virtual void test_of_simple_with_header_access_by_invalid_field()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV1), true);
		assertEquals(csvFile.row(0).findField("h3"), null);
		assertThrowsIllegalArg(() => csvFile.row(0).getField("h3"));
	  }

	  public virtual void test_of_blank_row()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV3), false);
		assertEquals(csvFile.rowCount(), 2);
		assertEquals(csvFile.row(0).lineNumber(), 1);
		assertEquals(csvFile.row(1).lineNumber(), 3);

		assertEquals(csvFile.row(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).field(0), "r11");
		assertEquals(csvFile.row(0).field(1), "r12");
		assertEquals(csvFile.row(1).fieldCount(), 2);
		assertEquals(csvFile.row(1).field(0), "r21");
		assertEquals(csvFile.row(1).field(1), "r22");
	  }

	  public virtual void test_of_headerComment()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV7), true);
		assertEquals(csvFile.rowCount(), 1);
		assertEquals(csvFile.row(0).lineNumber(), 3);

		assertEquals(csvFile.headers().size(), 2);
		assertEquals(csvFile.headers().get(0), "h1");
		assertEquals(csvFile.headers().get(1), "h2");
		assertEquals(csvFile.row(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).field(0), "r1");
		assertEquals(csvFile.row(0).field(1), "r2");
	  }

	  public virtual void test_of_quoting()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV4), false);
		assertEquals(csvFile.rowCount(), 3);
		assertEquals(csvFile.row(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).field(0), "alpha");
		assertEquals(csvFile.row(0).field(1), "be, \"at\", one");
		assertEquals(csvFile.row(1).fieldCount(), 2);
		assertEquals(csvFile.row(1).field(0), "alpha\",\"be\"");
		assertEquals(csvFile.row(1).field(1), "\"at\", one");
		assertEquals(csvFile.row(2).fieldCount(), 2);
		assertEquals(csvFile.row(2).field(0), "r21");
		assertEquals(csvFile.row(2).field(1), " r22 ");
	  }

	  public virtual void test_of_quotingWithEquals()
	  {
		CsvFile csvFile = CsvFile.of(CharSource.wrap(CSV4B), false);
		assertEquals(csvFile.rowCount(), 3);
		assertEquals(csvFile.row(0).fieldCount(), 2);
		assertEquals(csvFile.row(0).field(0), "alpha");
		assertEquals(csvFile.row(0).field(1), "be, \"at\", one");
		assertEquals(csvFile.row(1).fieldCount(), 2);
		assertEquals(csvFile.row(1).field(0), "alpha\",\"be\"");
		assertEquals(csvFile.row(1).field(1), "\"at\", one");
		assertEquals(csvFile.row(2).fieldCount(), 2);
		assertEquals(csvFile.row(2).field(0), "r21");
		assertEquals(csvFile.row(2).field(1), " r22 ");
	  }

	  public virtual void test_of_quoting_mismatched()
	  {
		assertThrowsIllegalArg(() => CsvFile.of(CharSource.wrap("\"alpha"), false), "Mismatched quotes in CSV on line 1");
		assertThrowsIllegalArg(() => CsvFile.of(CharSource.wrap("\"al\"pha"), false));
		assertThrowsIllegalArg(() => CsvFile.of(CharSource.wrap("\"al\"\"pha"), false));
		assertThrowsIllegalArg(() => CsvFile.of(CharSource.wrap("\"al,pha"), false));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_lists_noHeader()
	  {
		IList<string> headers = Collections.emptyList();
		IList<IList<string>> rows = Arrays.asList(Arrays.asList("a", "x"), Arrays.asList("b", "y"));
		CsvFile csvFile = CsvFile.of(headers, rows);
		assertEquals(csvFile.headers(), headers);
		assertEquals(csvFile.rows().size(), 2);
		assertEquals(csvFile.row(0).fields(), Arrays.asList("a", "x"));
		assertEquals(csvFile.row(1).fields(), Arrays.asList("b", "y"));
	  }

	  public virtual void test_of_lists_noHeaderNoRows()
	  {
		IList<string> headers = Collections.emptyList();
		IList<IList<string>> rows = Collections.emptyList();
		CsvFile csvFile = CsvFile.of(headers, rows);
		assertEquals(csvFile.headers(), headers);
		assertEquals(csvFile.rows(), rows);
	  }

	  public virtual void test_of_lists_header()
	  {
		IList<string> headers = Arrays.asList("1", "2");
		IList<IList<string>> rows = Arrays.asList(Arrays.asList("a", "x"), Arrays.asList("b", "y"));
		CsvFile csvFile = CsvFile.of(headers, rows);
		assertEquals(csvFile.headers(), headers);
		assertEquals(csvFile.rows().size(), 2);
		assertEquals(csvFile.row(0).fields(), Arrays.asList("a", "x"));
		assertEquals(csvFile.row(1).fields(), Arrays.asList("b", "y"));
	  }

	  public virtual void test_of_lists_sizeMismatch()
	  {
		IList<string> headers = Arrays.asList("1", "2");
		IList<IList<string>> rows = Arrays.asList(Arrays.asList("a", "x"), Arrays.asList("b"));
		assertThrowsIllegalArg(() => CsvFile.of(headers, rows));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_empty_no_header_reader()
	  {
		CsvFile csvFile = CsvFile.of(new StringReader(""), false, ',');
		assertEquals(csvFile.headers().size(), 0);
		assertEquals(csvFile.rowCount(), 0);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCodeToString()
	  {
		CsvFile a1 = CsvFile.of(CharSource.wrap(CSV1), true);
		CsvFile a2 = CsvFile.of(CharSource.wrap(CSV1), true);
		CsvFile b = CsvFile.of(CharSource.wrap(CSV2), true);
		CsvFile c = CsvFile.of(CharSource.wrap(CSV3), false);
		// file
		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);
		assertEquals(a1.Equals(null), false);
		assertEquals(a1.Equals(ANOTHER_TYPE), false);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
		assertNotNull(a1.ToString());
		// row
		assertEquals(a1.row(0).Equals(a1.row(0)), true);
		assertEquals(a1.row(0).Equals(a2.row(0)), true);
		assertEquals(a1.row(0).Equals(b.row(0)), false);
		assertEquals(c.row(0).Equals(c.row(1)), false);
		assertEquals(a1.row(0).Equals(ANOTHER_TYPE), false);
		assertEquals(a1.row(0).Equals(null), false);
		assertEquals(a1.row(0).GetHashCode(), a2.row(0).GetHashCode());
		assertNotNull(a1.row(0).ToString());
	  }

	}

}