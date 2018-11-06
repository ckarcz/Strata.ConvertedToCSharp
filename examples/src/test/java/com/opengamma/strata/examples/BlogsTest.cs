/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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

	using Test = org.testng.annotations.Test;

	using CalibrationPV01Example = com.opengamma.strata.examples.blog.multicurve1.CalibrationPV01Example;
	using CalibrationPVPerformanceExample = com.opengamma.strata.examples.blog.multicurve1.CalibrationPVPerformanceExample;

	/// <summary>
	/// Test blog related examples do not throw exceptions.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlogsTest
	public class BlogsTest
	{

	  private static readonly string[] NO_ARGS = new string[0];

	  //-------------------------------------------------------------------------
	  public virtual void test_multicurve1_pv01()
	  {
		string captured = caputureSystemOut(() => CalibrationPV01Example.main(NO_ARGS));
		assertTrue(captured.Contains("Calibration and export finished"));
		assertValidCaptured(captured);
	  }

	  public virtual void test_multicurve1_perf()
	  {
		string captured = caputureSystemOut(() => CalibrationPVPerformanceExample.main(NO_ARGS));
		assertTrue(captured.Contains("Performance estimate for curve calibration"));
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