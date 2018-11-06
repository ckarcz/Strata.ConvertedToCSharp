/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;

	/// <summary>
	/// Test <seealso cref="DiscountFactors"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountFactorsTest
	public class DiscountFactorsTest
	{

	  private static readonly LocalDate DATE_VAL = date(2015, 6, 4);

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly CurveName NAME = CurveName.of("TestCurve");
	  private static readonly InterpolatedNodalCurve CURVE_DF = InterpolatedNodalCurve.of(Curves.discountFactors(NAME, ACT_365F), DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE_ZERO = InterpolatedNodalCurve.of(Curves.zeroRates(NAME, ACT_365F), DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
	  private static readonly CurveMetadata META_ZERO_PERIODIC = DefaultCurveMetadata.builder().curveName(NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).addInfo(CurveInfoType.COMPOUNDING_PER_YEAR, 2).build();
	  private static readonly InterpolatedNodalCurve CURVE_ZERO_PERIODIC = InterpolatedNodalCurve.of(META_ZERO_PERIODIC, DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);
	  private static readonly InterpolatedNodalCurve CURVE_PRICES = InterpolatedNodalCurve.of(Curves.prices(NAME), DoubleArray.of(0, 10), DoubleArray.of(1, 2), INTERPOLATOR);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_discountFactors()
	  {
		DiscountFactors test = DiscountFactors.of(GBP, DATE_VAL, CURVE_DF);
		assertEquals(test is SimpleDiscountFactors, true);
		assertEquals(test.Currency, GBP);
		assertEquals(test.ValuationDate, DATE_VAL);
	  }

	  public virtual void test_of_zeroRate()
	  {
		DiscountFactors test = DiscountFactors.of(GBP, DATE_VAL, CURVE_ZERO);
		assertEquals(test is ZeroRateDiscountFactors, true);
		assertEquals(test.Currency, GBP);
		assertEquals(test.ValuationDate, DATE_VAL);
	  }

	  public virtual void test_of_zeroRatePeriodic()
	  {
		DiscountFactors test = DiscountFactors.of(GBP, DATE_VAL, CURVE_ZERO_PERIODIC);
		assertEquals(test is ZeroRatePeriodicDiscountFactors, true);
		assertEquals(test.Currency, GBP);
		assertEquals(test.ValuationDate, DATE_VAL);
	  }

	  public virtual void test_of_prices()
	  {
		assertThrowsIllegalArg(() => DiscountFactors.of(GBP, DATE_VAL, CURVE_PRICES));
	  }

	}

}