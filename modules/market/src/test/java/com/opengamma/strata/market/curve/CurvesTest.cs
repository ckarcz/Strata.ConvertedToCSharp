using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Test <seealso cref="Curves"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurvesTest
	public class CurvesTest
	{

	  private const string NAME = "Foo";
	  private static readonly CurveName CURVE_NAME = CurveName.of(NAME);
	  private static readonly IList<ParameterMetadata> PARAMS = ImmutableList.of();

	  //-------------------------------------------------------------------------
	  public virtual void zeroRates_string()
	  {
		CurveMetadata test = Curves.zeroRates(NAME, ACT_360);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void zeroRates_curveName()
	  {
		CurveMetadata test = Curves.zeroRates(CURVE_NAME, ACT_360);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void zeroRates_curveNameParams()
	  {
		CurveMetadata test = Curves.zeroRates(CURVE_NAME, ACT_360, PARAMS);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_360).parameterMetadata(PARAMS).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void forwardRates_string()
	  {
		CurveMetadata test = Curves.forwardRates(NAME, ACT_360);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.FORWARD_RATE).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void forwardRates_curveName()
	  {
		CurveMetadata test = Curves.forwardRates(CURVE_NAME, ACT_360);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.FORWARD_RATE).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void forwardRates_curveNameParams()
	  {
		CurveMetadata test = Curves.forwardRates(CURVE_NAME, ACT_360, PARAMS);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.FORWARD_RATE).dayCount(ACT_360).parameterMetadata(PARAMS).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void discountFactors_string()
	  {
		CurveMetadata test = Curves.discountFactors(NAME, ACT_360);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void discountFactors_curveName()
	  {
		CurveMetadata test = Curves.discountFactors(CURVE_NAME, ACT_360);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void discountFactors_curveNameParams()
	  {
		CurveMetadata test = Curves.discountFactors(CURVE_NAME, ACT_360, PARAMS);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(ACT_360).parameterMetadata(PARAMS).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void prices_string()
	  {
		CurveMetadata test = Curves.prices(NAME);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.MONTHS).yValueType(ValueType.PRICE_INDEX).build();
		assertEquals(test, expected);
	  }

	  public virtual void prices_curveName()
	  {
		CurveMetadata test = Curves.prices(CURVE_NAME);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.MONTHS).yValueType(ValueType.PRICE_INDEX).build();
		assertEquals(test, expected);
	  }

	  public virtual void prices_curveNameParams()
	  {
		CurveMetadata test = Curves.prices(CURVE_NAME, PARAMS);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.MONTHS).yValueType(ValueType.PRICE_INDEX).parameterMetadata(PARAMS).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void blackVolatilityByExpiry_string()
	  {
		CurveMetadata test = Curves.blackVolatilityByExpiry(NAME, ACT_360);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void blackVolatilityByExpiry_curveName()
	  {
		CurveMetadata test = Curves.blackVolatilityByExpiry(CURVE_NAME, ACT_360);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void blackVolatilityByExpiry_curveNameParams()
	  {
		CurveMetadata test = Curves.blackVolatilityByExpiry(CURVE_NAME, ACT_360, PARAMS);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_360).parameterMetadata(PARAMS).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void recoveryRates_string()
	  {
		CurveMetadata test = Curves.recoveryRates(NAME, ACT_360);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.RECOVERY_RATE).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void recoveryRates_curveName()
	  {
		CurveMetadata test = Curves.recoveryRates(CURVE_NAME, ACT_360);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.RECOVERY_RATE).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void recoveryRates_curveNameParams()
	  {
		CurveMetadata test = Curves.recoveryRates(CURVE_NAME, ACT_360, PARAMS);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.RECOVERY_RATE).dayCount(ACT_360).parameterMetadata(PARAMS).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void sabrParameterByExpiry_string()
	  {
		CurveMetadata test = Curves.sabrParameterByExpiry(NAME, ACT_365F, ValueType.SABR_ALPHA);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.SABR_ALPHA).dayCount(ACT_365F).build();
		assertEquals(test, expected);
	  }

	  public virtual void sabrParameterByExpiry_curveName()
	  {
		CurveMetadata test = Curves.sabrParameterByExpiry(CURVE_NAME, ACT_365F, ValueType.SABR_BETA);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.SABR_BETA).dayCount(ACT_365F).build();
		assertEquals(test, expected);
	  }

	  public virtual void sabrParameterByExpiry_curveNameParams()
	  {
		CurveMetadata test = Curves.sabrParameterByExpiry(CURVE_NAME, ACT_365F, ValueType.SABR_NU, PARAMS);
		CurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.SABR_NU).dayCount(ACT_365F).parameterMetadata(PARAMS).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(Curves));
	  }

	}

}