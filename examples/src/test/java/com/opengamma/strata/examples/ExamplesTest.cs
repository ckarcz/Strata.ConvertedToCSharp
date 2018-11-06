using System.IO;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.caputureSystemOut;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using JodaBeanSer = org.joda.beans.ser.JodaBeanSer;
	using Test = org.testng.annotations.Test;

	using CalibrationCheckExample = com.opengamma.strata.examples.finance.CalibrationCheckExample;
	using CalibrationEur3CheckExample = com.opengamma.strata.examples.finance.CalibrationEur3CheckExample;
	using CalibrationSimpleForwardCheckExample = com.opengamma.strata.examples.finance.CalibrationSimpleForwardCheckExample;
	using CalibrationUsdCpiExample = com.opengamma.strata.examples.finance.CalibrationUsdCpiExample;
	using CalibrationUsdFfsExample = com.opengamma.strata.examples.finance.CalibrationUsdFfsExample;
	using CalibrationXCcyCheckExample = com.opengamma.strata.examples.finance.CalibrationXCcyCheckExample;
	using CurveScenarioExample = com.opengamma.strata.examples.finance.CurveScenarioExample;
	using HistoricalScenarioExample = com.opengamma.strata.examples.finance.HistoricalScenarioExample;
	using SabrSwaptionCubeCalibrationExample = com.opengamma.strata.examples.finance.SabrSwaptionCubeCalibrationExample;
	using SabrSwaptionCubePvRiskExample = com.opengamma.strata.examples.finance.SabrSwaptionCubePvRiskExample;
	using SwapPricingCcpExample = com.opengamma.strata.examples.finance.SwapPricingCcpExample;
	using SwapPricingWithCalibrationExample = com.opengamma.strata.examples.finance.SwapPricingWithCalibrationExample;
	using SwapTradeExample = com.opengamma.strata.examples.finance.SwapTradeExample;
	using ReportRunnerTool = com.opengamma.strata.examples.report.ReportRunnerTool;
	using TradeList = com.opengamma.strata.examples.report.TradeList;

	/// <summary>
	/// Test examples do not throw exceptions.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ExamplesTest
	public class ExamplesTest
	{

	  private static readonly string[] NO_ARGS = new string[0];

	  //-------------------------------------------------------------------------
	  public virtual void test_dsfPricing_standalone()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => DsfPricingExample.main(NO_ARGS)));
	  }

	  public virtual void test_dsfPricing_tool()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => ReportRunnerTool.main(toolArgs("dsf"))));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_fraPricing_standalone()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => FraPricingExample.main(NO_ARGS)));
	  }

	  public virtual void test_fraPricing_tool()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => ReportRunnerTool.main(toolArgs("fra"))));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_fxPricing_standalone()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => FxPricingExample.main(NO_ARGS)));
	  }

	  public virtual void test_fxPricing_tool()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => ReportRunnerTool.main(toolArgs("fx"))));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_genericSecurityPricing_standalone()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => GenericSecurityPricingExample.main(NO_ARGS)));
	  }

	  public virtual void test_genericSecurityPricing_tool()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => ReportRunnerTool.main(toolArgs("security"))));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_iborFuturePricing_standalone()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => StirFuturePricingExample.main(NO_ARGS)));
	  }

	  public virtual void test_iborFuturePricing_tool()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => ReportRunnerTool.main(toolArgs("stir-future"))));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_swapPricing_standalone()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => SwapPricingExample.main(NO_ARGS)));
	  }

	  public virtual void test_swapPricing_tool()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => ReportRunnerTool.main(toolArgs("swap"))));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_swapPricingCcp_standalone()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => SwapPricingCcpExample.main(NO_ARGS)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_swapPricingWithCalibration_standalone()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => SwapPricingWithCalibrationExample.main(NO_ARGS)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_termDepositPricing_standalone()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => TermDepositPricingExample.main(NO_ARGS)));
	  }

	  public virtual void test_termDepositPricing_tool()
	  {
		assertValidCapturedAsciiTable(caputureSystemOut(() => ReportRunnerTool.main(toolArgs("term-deposit"))));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_curveScenario()
	  {
		string captured = caputureSystemOut(() => CurveScenarioExample.main(NO_ARGS));
		assertTrue(captured.Contains("PV01"));
		assertValidCaptured(captured);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_historicalScenario()
	  {
		string captured = caputureSystemOut(() => HistoricalScenarioExample.main(NO_ARGS));
		assertTrue(captured.Contains("Base PV"));
		assertValidCaptured(captured);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_swapTrade()
	  {
		string captured = caputureSystemOut(() => SwapTradeExample.main(NO_ARGS));
		assertTrue(captured.Contains("<product>"));
		assertValidCaptured(captured);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_portfolios() throws Exception
	  public virtual void test_portfolios()
	  {
		File baseDir = new File("src/main/resources/example-portfolios");
		assertTrue(baseDir.exists());
		foreach (File file in baseDir.listFiles(f => f.Name.EndsWith(".xml")))
		{
		  using (FileStream @in = new FileStream(file, FileMode.Open, FileAccess.Read))
		  {
			TradeList tradeList = JodaBeanSer.COMPACT.xmlReader().read(@in, typeof(TradeList));
			assertTrue(tradeList.Trades.size() > 0);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_calibration() throws Exception
	  public virtual void test_calibration()
	  {
		string captured = caputureSystemOut(() => CalibrationCheckExample.main(NO_ARGS));
		assertTrue(captured.Contains("Checked PV for all instruments used in the calibration set are near to zero"));
		assertFalse(captured.Contains("ERROR"));
		assertFalse(captured.Contains("Exception"));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_calibration_xccy() throws Exception
	  public virtual void test_calibration_xccy()
	  {
		string captured = caputureSystemOut(() => CalibrationXCcyCheckExample.main(NO_ARGS));
		assertTrue(captured.Contains("Checked PV for all instruments used in the calibration set are near to zero"));
		assertFalse(captured.Contains("ERROR"));
		assertFalse(captured.Contains("Exception"));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_calibration_eur3() throws Exception
	  public virtual void test_calibration_eur3()
	  {
		string captured = caputureSystemOut(() => CalibrationEur3CheckExample.main(NO_ARGS));
		assertTrue(captured.Contains("Checked PV for all instruments used in the calibration set are near to zero"));
		assertFalse(captured.Contains("ERROR"));
		assertFalse(captured.Contains("Exception"));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_calibration_simple_forward() throws Exception
	  public virtual void test_calibration_simple_forward()
	  {
		string captured = caputureSystemOut(() => CalibrationSimpleForwardCheckExample.main(NO_ARGS));
		assertTrue(captured.Contains("Checked PV for all instruments used in the calibration set are near to zero"));
		assertFalse(captured.Contains("ERROR"));
		assertFalse(captured.Contains("Exception"));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_calibration_usd_ffs() throws Exception
	  public virtual void test_calibration_usd_ffs()
	  {
		string captured = caputureSystemOut(() => CalibrationUsdFfsExample.main(NO_ARGS));
		assertTrue(captured.Contains("Checked PV for all instruments used in the calibration set are near to zero"));
		assertFalse(captured.Contains("ERROR"));
		assertFalse(captured.Contains("Exception"));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_calibration_cpi() throws Exception
	  public virtual void test_calibration_cpi()
	  {
		string captured = caputureSystemOut(() => CalibrationUsdCpiExample.main(NO_ARGS));
		assertTrue(captured.Contains("Checked PV for all instruments used in the calibration set are near to zero"));
		assertFalse(captured.Contains("ERROR"));
		assertFalse(captured.Contains("Exception"));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_sabr_swaption_calibration() throws Exception
	  public virtual void test_sabr_swaption_calibration()
	  {
		string captured = caputureSystemOut(() => SabrSwaptionCubeCalibrationExample.main(NO_ARGS));
		assertTrue(captured.Contains("End calibration"));
		assertFalse(captured.Contains("ERROR"));
		assertFalse(captured.Contains("Exception"));
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_sabr_swaption_calibration_pv_risk() throws Exception
	  public virtual void test_sabr_swaption_calibration_pv_risk()
	  {
		string captured = caputureSystemOut(() => SabrSwaptionCubePvRiskExample.main(NO_ARGS));
		assertTrue(captured.Contains("PV and risk time"));
		assertFalse(captured.Contains("ERROR"));
		assertFalse(captured.Contains("Exception"));
	  }

	  //-------------------------------------------------------------------------
	  private string[] toolArgs(string name)
	  {
		return new string[] {"-p", "src/main/resources/example-portfolios/" + name + "-portfolio.xml", "-t", "src/main/resources/example-reports/" + name + "-report-template.ini", "-d", "2014-01-22"};
	  }

	  private void assertValidCapturedAsciiTable(string captured)
	  {
		assertTrue(captured.Contains("+------"), captured);
		assertValidCaptured(captured);
	  }

	  private void assertValidCaptured(string captured)
	  {
		assertFalse(captured.Contains("ERROR"), captured);
		assertFalse(captured.Contains("FAIL"), captured);
		assertFalse(captured.Contains("Exception"), captured);
		assertFalse(captured.Contains("drill down"), captured);
	  }

	}

}