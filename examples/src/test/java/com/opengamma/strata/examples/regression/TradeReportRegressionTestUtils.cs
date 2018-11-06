using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.regression
{

	using Assert = org.testng.Assert;

	using Strings = com.google.common.@base.Strings;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Utility class for trade report regression tests.
	/// </summary>
	public sealed class TradeReportRegressionTestUtils
	{

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private TradeReportRegressionTestUtils()
	  {
	  }

	  public static void assertAsciiTableEquals(string actual, string expected)
	  {
		IList<string> actualLines = toLines(actual);
		IList<string> expectedLines = toLines(expected);

		int maxLines = Math.Max(actualLines.Count, expectedLines.Count);

		for (int i = 0; i < maxLines; i++)
		{
		  if (i >= actualLines.Count)
		  {
			string expectedLine = expectedLines[i];
			Assert.fail(Messages.format("No more results but expected:\n{}", expectedLine));
		  }
		  if (i >= expectedLines.Count)
		  {
			string actualLine = actualLines[i];
			Assert.fail(Messages.format("Expected end of results but got:\n{}", actualLine));
		  }
		  string actualLine = actualLines[i];
		  string expectedLine = expectedLines[i];
		  if (!actualLine.Equals(expectedLine))
		  {
			if (isDataRow(expectedLine) && isDataRow(actualLine))
			{
			  IList<string> actualCells = toCells(actualLine);
			  IList<string> expectedCells = toCells(expectedLine);
			  Assert.assertEquals(actualCells, expectedCells, "Mismatch at line " + i);
			}
			else
			{
			  Assert.fail(Messages.format("Mismatch at line {}:\n" + "Expected:\n" + "{}\n" + "Got:\n" + "{}\n" + "Expected table:\n" + "{}\n" + "Actual table:\n" + "{}", i, expectedLine, actualLine, expected, actual));
			}
		  }
		}
	  }

	  private static IList<string> toLines(string asciiTable)
	  {
		return Arrays.asList(asciiTable.Split("\\r?\\n", true)).Where(line => !Strings.nullToEmpty(line).Trim().Empty).ToList();
	  }

	  private static bool isDataRow(string asciiLine)
	  {
		return asciiLine.Contains("|");
	  }

	  private static IList<string> toCells(string asciiLine)
	  {
		return Arrays.asList(asciiLine.Split("\\|", true));
	  }

	}

}