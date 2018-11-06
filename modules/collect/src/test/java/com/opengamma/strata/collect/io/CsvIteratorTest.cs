using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="CsvIterator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CsvIteratorTest
	public class CsvIteratorTest
	{

	  private readonly string CSV1 = "" +
		  "h1,h2\n" +
		  "r11,r12\n" +
		  "r21,r22";

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
		  "# Comment about the file\n" +
		  "h1,h2\n" +
		  "r1,r2\n";

	  private readonly string CSV5GROUPED = "" +
		  "id,value\n" +
		  "1,a\n" +
		  "1,b\n" +
		  "1,c\n" +
		  "2,mm\n" +
		  "3,yyy\n" +
		  "3,zzz";

	  //-------------------------------------------------------------------------
	  public virtual void test_of_ioException()
	  {
		assertThrows(() => CsvIterator.of(Files.asCharSource(new File("src/test/resources"), StandardCharsets.UTF_8), false), typeof(UncheckedIOException));
	  }

	  public virtual void test_of_empty_no_header()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(""), false))
		{
		  assertEquals(csvFile.headers().size(), 0);
		  assertEquals(csvFile.containsHeader("a"), false);
		  assertEquals(csvFile.hasNext(), false);
		  assertEquals(csvFile.hasNext(), false);
		  assertThrows(() => csvFile.peek(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.next(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.next(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.remove(), typeof(System.NotSupportedException));
		}
	  }

	  public virtual void test_of_empty_with_header()
	  {
		assertThrowsIllegalArg(() => CsvIterator.of(CharSource.wrap(""), true));
	  }

	  public virtual void test_of_simple_no_header()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV1), false))
		{
		  assertEquals(csvFile.headers().size(), 0);
		  assertEquals(csvFile.hasNext(), true);
		  assertEquals(csvFile.hasNext(), true);
		  CsvRow peeked = csvFile.peek();
		  CsvRow row0 = csvFile.next();
		  assertEquals(row0, peeked);
		  assertEquals(row0.headers().size(), 0);
		  assertEquals(row0.lineNumber(), 1);
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "h1");
		  assertEquals(row0.field(1), "h2");
		  CsvRow row1 = csvFile.next();
		  assertEquals(row1.headers().size(), 0);
		  assertEquals(row1.lineNumber(), 2);
		  assertEquals(row1.fieldCount(), 2);
		  assertEquals(row1.field(0), "r11");
		  assertEquals(row1.field(1), "r12");
		  CsvRow row2 = csvFile.next();
		  assertEquals(row2.headers().size(), 0);
		  assertEquals(row2.lineNumber(), 3);
		  assertEquals(row2.fieldCount(), 2);
		  assertEquals(row2.field(0), "r21");
		  assertEquals(row2.field(1), "r22");
		  assertEquals(csvFile.hasNext(), false);
		  assertThrows(() => csvFile.peek(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.peek(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.next(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.next(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.remove(), typeof(System.NotSupportedException));
		}
	  }

	  public virtual void test_of_simple_no_header_tabs()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV1T), false, '\t'))
		{
		  assertEquals(csvFile.headers().size(), 0);
		  CsvRow row0 = csvFile.next();
		  assertEquals(row0.headers().size(), 0);
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "h1");
		  assertEquals(row0.field(1), "h2");
		  assertEquals(csvFile.hasNext(), true);
		  CsvRow row1 = csvFile.next();
		  assertEquals(row1.headers().size(), 0);
		  assertEquals(row1.fieldCount(), 2);
		  assertEquals(row1.field(0), "r11");
		  assertEquals(row1.field(1), "r12");
		  assertEquals(csvFile.hasNext(), true);
		  CsvRow row2 = csvFile.next();
		  assertEquals(row2.headers().size(), 0);
		  assertEquals(row2.fieldCount(), 2);
		  assertEquals(row2.field(0), "r21");
		  assertEquals(row2.field(1), "r22");
		  assertEquals(csvFile.hasNext(), false);
		  assertThrows(() => csvFile.next(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.next(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.remove(), typeof(System.NotSupportedException));
		}
	  }

	  public virtual void test_of_simple_with_header()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV1), true))
		{
		  ImmutableList<string> headers = csvFile.headers();
		  assertEquals(headers.size(), 2);
		  assertEquals(csvFile.containsHeader("h1"), true);
		  assertEquals(csvFile.containsHeader("h2"), true);
		  assertEquals(csvFile.containsHeader("a"), false);
		  assertEquals(csvFile.containsHeader(Pattern.compile("h.")), true);
		  assertEquals(csvFile.containsHeader(Pattern.compile("a")), false);
		  assertEquals(headers.get(0), "h1");
		  assertEquals(headers.get(1), "h2");
		  CsvRow peeked = csvFile.peek();
		  CsvRow row0 = csvFile.next();
		  assertEquals(row0, peeked);
		  assertEquals(row0.headers(), headers);
		  assertEquals(row0.lineNumber(), 2);
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "r11");
		  assertEquals(row0.field(1), "r12");
		  CsvRow row1 = csvFile.next();
		  assertEquals(row1.headers(), headers);
		  assertEquals(row1.lineNumber(), 3);
		  assertEquals(row1.fieldCount(), 2);
		  assertEquals(row1.field(0), "r21");
		  assertEquals(row1.field(1), "r22");
		  assertEquals(csvFile.hasNext(), false);
		  assertThrows(() => csvFile.peek(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.peek(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.next(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.next(), typeof(NoSuchElementException));
		  assertThrows(() => csvFile.remove(), typeof(System.NotSupportedException));
		}
	  }

	  public virtual void test_of_comment_blank_no_header()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV2), false))
		{
		  assertEquals(csvFile.headers().size(), 0);
		  assertEquals(csvFile.hasNext(), true);
		  CsvRow row0 = csvFile.next();
		  assertEquals(row0.lineNumber(), 1);
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "h1");
		  assertEquals(row0.field(1), "h2");
		  CsvRow row1 = csvFile.next();
		  assertEquals(row1.lineNumber(), 5);
		  assertEquals(row1.fieldCount(), 2);
		  assertEquals(row1.field(0), "r21");
		  assertEquals(row1.field(1), "r22");
		  assertEquals(csvFile.hasNext(), false);
		}
	  }

	  public virtual void test_of_comment_blank_with_header()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV2), true))
		{
		  ImmutableList<string> headers = csvFile.headers();
		  assertEquals(headers.size(), 2);
		  assertEquals(headers.get(0), "h1");
		  assertEquals(headers.get(1), "h2");
		  assertEquals(csvFile.hasNext(), true);
		  CsvRow row0 = csvFile.next();
		  assertEquals(row0.lineNumber(), 5);
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "r21");
		  assertEquals(row0.field(1), "r22");
		  assertEquals(csvFile.hasNext(), false);
		}
	  }

	  public virtual void test_of_blank_row()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV3), false))
		{
		  assertEquals(csvFile.hasNext(), true);
		  CsvRow row0 = csvFile.next();
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "r11");
		  assertEquals(row0.field(1), "r12");
		  CsvRow row1 = csvFile.next();
		  assertEquals(row1.fieldCount(), 2);
		  assertEquals(row1.field(0), "r21");
		  assertEquals(row1.field(1), "r22");
		  assertEquals(csvFile.hasNext(), false);
		}
	  }

	  public virtual void test_of_headerComment()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV4), true))
		{
		  assertEquals(csvFile.hasNext(), true);
		  CsvRow row0 = csvFile.next();
		  assertEquals(row0.lineNumber(), 3);
		  assertEquals(csvFile.headers().size(), 2);
		  assertEquals(csvFile.headers().get(0), "h1");
		  assertEquals(csvFile.headers().get(1), "h2");
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "r1");
		  assertEquals(row0.field(1), "r2");
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_empty_no_header_reader()
	  {
		using (CsvIterator csvFile = CsvIterator.of(new StringReader(""), false, ','))
		{
		  assertEquals(csvFile.headers().size(), 0);
		  assertEquals(csvFile.hasNext(), false);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_nextBatch1()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV1), true))
		{
		  ImmutableList<string> headers = csvFile.headers();
		  assertEquals(headers.size(), 2);
		  assertEquals(headers.get(0), "h1");
		  assertEquals(headers.get(1), "h2");
		  IList<CsvRow> a = csvFile.nextBatch(0);
		  assertEquals(a.Count, 0);
		  IList<CsvRow> b = csvFile.nextBatch(1);
		  assertEquals(b.Count, 1);
		  CsvRow row0 = b[0];
		  assertEquals(row0.headers(), headers);
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "r11");
		  assertEquals(row0.field(1), "r12");
		  IList<CsvRow> c = csvFile.nextBatch(2);
		  assertEquals(c.Count, 1);
		  CsvRow row1 = c[0];
		  assertEquals(row1.headers(), headers);
		  assertEquals(row1.fieldCount(), 2);
		  assertEquals(row1.field(0), "r21");
		  assertEquals(row1.field(1), "r22");
		  IList<CsvRow> d = csvFile.nextBatch(2);
		  assertEquals(d.Count, 0);
		  assertEquals(csvFile.hasNext(), false);
		}
	  }

	  public virtual void test_nextBatch2()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV1), true))
		{
		  ImmutableList<string> headers = csvFile.headers();
		  assertEquals(headers.size(), 2);
		  assertEquals(headers.get(0), "h1");
		  assertEquals(headers.get(1), "h2");
		  IList<CsvRow> a = csvFile.nextBatch(3);
		  assertEquals(a.Count, 2);
		  CsvRow row0 = a[0];
		  assertEquals(row0.headers(), headers);
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "r11");
		  assertEquals(row0.field(1), "r12");
		  CsvRow row1 = a[1];
		  assertEquals(row1.headers(), headers);
		  assertEquals(row1.fieldCount(), 2);
		  assertEquals(row1.field(0), "r21");
		  assertEquals(row1.field(1), "r22");
		  IList<CsvRow> d = csvFile.nextBatch(2);
		  assertEquals(d.Count, 0);
		  assertEquals(csvFile.hasNext(), false);
		  assertEquals(csvFile.hasNext(), false);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void nextBatch_predicate()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV5GROUPED), true))
		{
		  ImmutableList<string> headers = csvFile.headers();
		  assertEquals(headers.size(), 2);
		  assertEquals(headers.get(0), "id");
		  assertEquals(headers.get(1), "value");
		  int batches = 0;
		  int total = 0;
		  while (csvFile.hasNext())
		  {
			CsvRow first = csvFile.peek();
			string id = first.getValue("id");
			IList<CsvRow> batch = csvFile.nextBatch(row => row.getValue("id").Equals(id));
			assertEquals(batch.Select(row => row.getValue("id")).Distinct().Count(), 1);
			batches++;
			total += batch.Count;
		  }
		  assertEquals(batches, 3);
		  assertEquals(total, 6);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_asStream_empty_no_header()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(""), false))
		{
		  assertEquals(csvFile.asStream().collect(toList()).size(), 0);
		}
	  }

	  public virtual void test_asStream_simple_no_header()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV1), false))
		{
		  assertEquals(csvFile.headers().size(), 0);
		  IList<CsvRow> rows = csvFile.asStream().collect(toList());
		  assertEquals(csvFile.hasNext(), false);
		  assertEquals(rows.Count, 3);
		  CsvRow row0 = rows[0];
		  assertEquals(row0.headers().size(), 0);
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "h1");
		  assertEquals(row0.field(1), "h2");
		  CsvRow row1 = rows[1];
		  assertEquals(row1.headers().size(), 0);
		  assertEquals(row1.fieldCount(), 2);
		  assertEquals(row1.field(0), "r11");
		  assertEquals(row1.field(1), "r12");
		  CsvRow row2 = rows[2];
		  assertEquals(row2.headers().size(), 0);
		  assertEquals(row2.fieldCount(), 2);
		  assertEquals(row2.field(0), "r21");
		  assertEquals(row2.field(1), "r22");
		}
	  }

	  public virtual void test_asStream_simple_with_header()
	  {
		using (CsvIterator csvFile = CsvIterator.of(CharSource.wrap(CSV1), true))
		{
		  ImmutableList<string> headers = csvFile.headers();
		  assertEquals(headers.size(), 2);
		  assertEquals(headers.get(0), "h1");
		  assertEquals(headers.get(1), "h2");
		  IList<CsvRow> rows = csvFile.asStream().collect(toList());
		  assertEquals(csvFile.hasNext(), false);
		  assertEquals(rows.Count, 2);
		  CsvRow row0 = rows[0];
		  assertEquals(row0.headers(), headers);
		  assertEquals(row0.fieldCount(), 2);
		  assertEquals(row0.field(0), "r11");
		  assertEquals(row0.field(1), "r12");
		  CsvRow row1 = rows[1];
		  assertEquals(row1.headers(), headers);
		  assertEquals(row1.fieldCount(), 2);
		  assertEquals(row1.field(0), "r21");
		  assertEquals(row1.field(1), "r22");
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toString()
	  {
		using (CsvIterator test = CsvIterator.of(CharSource.wrap(CSV1), true))
		{
		  assertNotNull(test.ToString());
		}
	  }

	}

}