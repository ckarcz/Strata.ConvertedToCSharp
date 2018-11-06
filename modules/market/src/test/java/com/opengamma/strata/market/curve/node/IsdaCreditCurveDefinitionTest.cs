/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.node
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_15Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_5Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;

	/// <summary>
	/// Test {@code IsdaCreditCurveDefinition}.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IsdaCreditCurveDefinitionTest
	public class IsdaCreditCurveDefinitionTest
	{

	  private static readonly CurveName NAME = CurveName.of("TestCurve");
	  private static readonly LocalDate CURVE_VALUATION_DATE = LocalDate.of(2014, 2, 3);
	  private static readonly ImmutableList<IsdaCreditCurveNode> NODES = ImmutableList.of(SwapIsdaCreditCurveNode.of(QuoteId.of(StandardId.of("OG", "swap1Y")), DaysAdjustment.NONE, BusinessDayAdjustment.NONE, TENOR_1Y, ACT_360, P3M), SwapIsdaCreditCurveNode.of(QuoteId.of(StandardId.of("OG", "swap5Y")), DaysAdjustment.NONE, BusinessDayAdjustment.NONE, TENOR_5Y, ACT_360, P3M), SwapIsdaCreditCurveNode.of(QuoteId.of(StandardId.of("OG", "swap15Y")), DaysAdjustment.NONE, BusinessDayAdjustment.NONE, TENOR_15Y, ACT_360, P3M));

	  public virtual void test_of()
	  {
		IsdaCreditCurveDefinition test = IsdaCreditCurveDefinition.of(NAME, USD, CURVE_VALUATION_DATE, ACT_ACT_ISDA, NODES, true, false);
		assertEquals(test.Currency, USD);
		assertEquals(test.CurveNodes, NODES);
		assertEquals(test.CurveValuationDate, CURVE_VALUATION_DATE);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.ComputeJacobian, true);
		assertEquals(test.StoreNodeTrade, false);
		DoubleArray time = DoubleArray.of(1, 2, 3);
		DoubleArray rate = DoubleArray.of(0.01, 0.014, 0.02);
		InterpolatedNodalCurve expectedCurve = InterpolatedNodalCurve.of(Curves.zeroRates(NAME, ACT_ACT_ISDA), time, rate, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
		assertEquals(test.curve(time, rate), expectedCurve);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IsdaCreditCurveDefinition test1 = IsdaCreditCurveDefinition.of(NAME, USD, CURVE_VALUATION_DATE, ACT_ACT_ISDA, NODES, true, true);
		coverImmutableBean(test1);
		IsdaCreditCurveDefinition test2 = IsdaCreditCurveDefinition.of(CurveName.of("TestCurve1"), EUR, CURVE_VALUATION_DATE.plusDays(1), ACT_365F, NODES.subList(0, 2), false, false);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		IsdaCreditCurveDefinition test = IsdaCreditCurveDefinition.of(NAME, USD, CURVE_VALUATION_DATE, ACT_ACT_ISDA, NODES, true, true);
		assertSerialization(test);
	  }

	}

}