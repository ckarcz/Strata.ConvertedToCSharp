/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.trade
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using IniFile = com.opengamma.strata.collect.io.IniFile;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;

	/// <summary>
	/// Test <seealso cref="TradeReportTemplateIniLoader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TradeReportTemplateIniLoaderTest
	public class TradeReportTemplateIniLoaderTest
	{

	  public virtual void test_simple_values()
	  {
		TradeReportTemplate template = parseIni("trade-report-test-simple.ini");

		TradeReportColumn productColumn = TradeReportColumn.builder().value("Product").header("Product").build();

		TradeReportColumn pvColumn = TradeReportColumn.builder().value("Measures.PresentValue").header("Present Value").build();

		assertEquals(template.Columns.Count, 2);
		assertEquals(template.Columns[0], productColumn);
		assertEquals(template.Columns[1], pvColumn);
	  }

	  public virtual void test_path()
	  {
		TradeReportTemplate template = parseIni("trade-report-test-path.ini");

		TradeReportColumn payLegCcyColumn = TradeReportColumn.builder().value("Measures.LegInitialNotional.pay.currency").header("Pay Leg Ccy").build();

		assertEquals(template.Columns.Count, 1);
		assertEquals(template.Columns[0], payLegCcyColumn);
	  }

	  public virtual void test_ignore_failures()
	  {
		TradeReportTemplate template = parseIni("trade-report-test-ignore-failures.ini");

		TradeReportColumn payLegCcyColumn = TradeReportColumn.builder().value("Measures.LegInitialNotional.pay.currency").header("Pay Leg Ccy").build();

		TradeReportColumn pvColumn = TradeReportColumn.builder().value("Measures.PresentValue").header("Present Value").ignoreFailures(true).build();

		assertEquals(template.Columns.Count, 2);
		assertEquals(template.Columns[0], payLegCcyColumn);
		assertEquals(template.Columns[1], pvColumn);
	  }

	  private TradeReportTemplate parseIni(string resourceName)
	  {
		ResourceLocator locator = ResourceLocator.of("classpath:" + resourceName);
		IniFile ini = IniFile.of(locator.CharSource);
		TradeReportTemplateIniLoader loader = new TradeReportTemplateIniLoader();
		return loader.load(ini);
	  }

	}

}