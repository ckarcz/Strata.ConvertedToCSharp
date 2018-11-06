/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Tests <seealso cref="RatesCurveCalibrator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesCurveCalibratorTest
	public class RatesCurveCalibratorTest
	{

	  public virtual void test_toString()
	  {
		assertThat(RatesCurveCalibrator.standard().ToString()).isEqualTo("CurveCalibrator[ParSpread]");
	  }

	}

}