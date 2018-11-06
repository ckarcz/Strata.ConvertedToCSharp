/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="SimpleCreditCurveCalibrator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SimpleCreditCurveCalibratorTest extends IsdaCompliantCreditCurveCalibratorBase
	public class SimpleCreditCurveCalibratorTest : IsdaCompliantCreditCurveCalibratorBase
	{

	  // calibrators
	  private static readonly SimpleCreditCurveCalibrator BUILDER_ISDA = new SimpleCreditCurveCalibrator(AccrualOnDefaultFormula.ORIGINAL_ISDA);
	  private static readonly SimpleCreditCurveCalibrator BUILDER_MARKIT = new SimpleCreditCurveCalibrator(AccrualOnDefaultFormula.MARKIT_FIX);

	  private const double TOL = 1e-14;

	  public virtual void regression_consistency_test()
	  {
		testCalibrationAgainstISDA(BUILDER_ISDA, ACT_365F, EUR, TOL);
		testCalibrationAgainstISDA(BUILDER_MARKIT, ACT_365F, EUR, TOL);
	  }

	}

}