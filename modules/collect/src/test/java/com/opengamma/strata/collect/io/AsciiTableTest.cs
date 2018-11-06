using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test <seealso cref=" AsciiTable"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AsciiTableTest
	public class AsciiTableTest
	{

	  private static readonly string LINE_SEPARATOR = Environment.NewLine;

	  public virtual void test_generate_padData()
	  {
		IList<AsciiTableAlignment> alignments = ImmutableList.of(AsciiTableAlignment.LEFT, AsciiTableAlignment.RIGHT);
		IList<string> headers = ImmutableList.of("Alpha", "Beta");
		IList<IList<string>> cells = ImmutableList.of(ImmutableList.of("12", "23"), ImmutableList.of("12345", ""));
		string test = AsciiTable.generate(headers, alignments, cells);
		string expected = "" +
			"+-------+------+" + LINE_SEPARATOR +
			"| Alpha | Beta |" + LINE_SEPARATOR +
			"+-------+------+" + LINE_SEPARATOR +
			"| 12    |   23 |" + LINE_SEPARATOR +
			"| 12345 |      |" + LINE_SEPARATOR +
			"+-------+------+" + LINE_SEPARATOR;
		assertEquals(test, expected);
	  }

	  public virtual void test_generate_padHeader()
	  {
		IList<AsciiTableAlignment> alignments = ImmutableList.of(AsciiTableAlignment.LEFT, AsciiTableAlignment.RIGHT);
		IList<string> headers = ImmutableList.of("A", "B");
		IList<IList<string>> cells = ImmutableList.of(ImmutableList.of("12", "23"), ImmutableList.of("12345", ""));
		string test = AsciiTable.generate(headers, alignments, cells);
		string expected = "" +
			"+-------+----+" + LINE_SEPARATOR +
			"| A     |  B |" + LINE_SEPARATOR +
			"+-------+----+" + LINE_SEPARATOR +
			"| 12    | 23 |" + LINE_SEPARATOR +
			"| 12345 |    |" + LINE_SEPARATOR +
			"+-------+----+" + LINE_SEPARATOR;
		assertEquals(test, expected);
	  }

	}

}