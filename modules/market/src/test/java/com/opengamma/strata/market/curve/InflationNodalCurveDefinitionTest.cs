/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedInflationSwapConventions.GBP_FIXED_ZC_GB_RPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using FixedInflationSwapCurveNode = com.opengamma.strata.market.curve.node.FixedInflationSwapCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using FixedInflationSwapTemplate = com.opengamma.strata.product.swap.type.FixedInflationSwapTemplate;

	/// <summary>
	/// Test <seealso cref="InflationNodalCurveDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InflationNodalCurveDefinitionTest
	public class InflationNodalCurveDefinitionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 9, 9);

	  private static readonly CurveName CURVE_NAME = CurveName.of("Test");
	  private static readonly QuoteId TICKER = QuoteId.of(StandardId.of("OG", "Ticker"));
	  private static readonly ImmutableList<FixedInflationSwapCurveNode> NODES = ImmutableList.of(FixedInflationSwapCurveNode.of(FixedInflationSwapTemplate.of(Tenor.TENOR_1Y, GBP_FIXED_ZC_GB_RPI), TICKER), FixedInflationSwapCurveNode.of(FixedInflationSwapTemplate.of(Tenor.TENOR_2Y, GBP_FIXED_ZC_GB_RPI), TICKER));
	  private static readonly InterpolatedNodalCurveDefinition UNDERLYING_DEF = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.PRICE_INDEX).dayCount(ACT_365F).nodes(NODES).interpolator(CurveInterpolators.LOG_LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
	  private static readonly DoubleArray SEASONALITY_ADDITIVE = DoubleArray.of(1.0, 1.5, 1.0, -0.5, -0.5, -1.0, -1.5, 0.0, 0.5, 1.0, 1.0, -2.5);
	  private static readonly SeasonalityDefinition SEASONALITY_DEF = SeasonalityDefinition.of(SEASONALITY_ADDITIVE, ShiftType.ABSOLUTE);
	  private static readonly YearMonth LAST_FIX_MONTH = YearMonth.of(2015, 7);
	  private const double LAST_FIX_VALUE = 240.0d;

	  public virtual void test_builder()
	  {
		InflationNodalCurveDefinition test = new InflationNodalCurveDefinition(UNDERLYING_DEF, LAST_FIX_MONTH, LAST_FIX_VALUE, SEASONALITY_DEF);
		assertEquals(test.CurveWithoutFixingDefinition, UNDERLYING_DEF);
		assertEquals(test.LastFixingMonth, LAST_FIX_MONTH);
		assertEquals(test.LastFixingValue, LAST_FIX_VALUE);
		assertEquals(test.SeasonalityDefinition, SEASONALITY_DEF);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_metadata()
	  {
		InflationNodalCurveDefinition test = new InflationNodalCurveDefinition(UNDERLYING_DEF, LAST_FIX_MONTH, LAST_FIX_VALUE, SEASONALITY_DEF);
		DefaultCurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.PRICE_INDEX).dayCount(ACT_365F).parameterMetadata(NODES.get(0).metadata(VAL_DATE, REF_DATA), NODES.get(1).metadata(VAL_DATE, REF_DATA)).build();
		assertEquals(test.metadata(VAL_DATE, REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_curve()
	  {
		InflationNodalCurveDefinition test = new InflationNodalCurveDefinition(UNDERLYING_DEF, LAST_FIX_MONTH, LAST_FIX_VALUE, SEASONALITY_DEF);
		DefaultCurveMetadata metadata = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.PRICE_INDEX).dayCount(ACT_365F).parameterMetadata(NODES.get(0).metadata(VAL_DATE, REF_DATA), NODES.get(1).metadata(VAL_DATE, REF_DATA)).build();
		LocalDate date0 = NODES.get(0).date(VAL_DATE, REF_DATA);
		LocalDate date1 = NODES.get(1).date(VAL_DATE, REF_DATA);
		DoubleArray param = DoubleArray.of(250.0d, 260.0d);
		InterpolatedNodalCurve expectedUnderlying = InterpolatedNodalCurve.builder().metadata(metadata).xValues(DoubleArray.of(ACT_365F.yearFraction(VAL_DATE, date0), ACT_365F.yearFraction(VAL_DATE, date1))).yValues(param).interpolator(CurveInterpolators.LOG_LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		InflationNodalCurve expected = InflationNodalCurve.of(expectedUnderlying, VAL_DATE, LAST_FIX_MONTH, LAST_FIX_VALUE, SEASONALITY_DEF);
		assertEquals(test.curve(VAL_DATE, metadata, param), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toCurveParameterSize()
	  {
		InflationNodalCurveDefinition test = new InflationNodalCurveDefinition(UNDERLYING_DEF, LAST_FIX_MONTH, LAST_FIX_VALUE, SEASONALITY_DEF);
		assertEquals(test.toCurveParameterSize(), CurveParameterSize.of(CURVE_NAME, NODES.size()));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		InflationNodalCurveDefinition test = new InflationNodalCurveDefinition(UNDERLYING_DEF, LAST_FIX_MONTH, LAST_FIX_VALUE, SEASONALITY_DEF);
		coverImmutableBean(test);
		InterpolatedNodalCurveDefinition underlyingDef2 = InterpolatedNodalCurveDefinition.builder().name(CurveName.of("foo")).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(NODES).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		SeasonalityDefinition seasonalityDef2 = SeasonalityDefinition.of(SEASONALITY_ADDITIVE, ShiftType.SCALED);
		InflationNodalCurveDefinition test2 = new InflationNodalCurveDefinition(underlyingDef2, LAST_FIX_MONTH.plus(Period.ofMonths(1)), LAST_FIX_VALUE + 1.0d, seasonalityDef2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		InflationNodalCurveDefinition test = new InflationNodalCurveDefinition(UNDERLYING_DEF, LAST_FIX_MONTH, LAST_FIX_VALUE, SEASONALITY_DEF);
		assertSerialization(test);
	  }

	}

}