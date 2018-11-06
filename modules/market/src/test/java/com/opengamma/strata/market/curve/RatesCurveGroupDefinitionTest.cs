using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_364;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FloatingRateNames.GBP_LIBOR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_RPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.CurveNodeClashAction.DROP_THIS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedInflationSwapConventions.GBP_FIXED_ZC_GB_RPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Index = com.opengamma.strata.basics.index.Index;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using FixedInflationSwapCurveNode = com.opengamma.strata.market.curve.node.FixedInflationSwapCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using Trade = com.opengamma.strata.product.Trade;
	using FixedInflationSwapTemplate = com.opengamma.strata.product.swap.type.FixedInflationSwapTemplate;

	/// <summary>
	/// Test <seealso cref="RatesCurveGroupDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesCurveGroupDefinitionTest
	public class RatesCurveGroupDefinitionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly ObservableId GBP_LIBOR_1M_ID = QuoteId.of(StandardId.of("OG", "Ticker1"));
	  private static readonly ObservableId GBP_LIBOR_3M_ID = QuoteId.of(StandardId.of("OG", "Ticker3"));
	  private static readonly DummyFraCurveNode NODE1 = DummyFraCurveNode.of(Period.ofMonths(1), GBP_LIBOR_1M, GBP_LIBOR_1M_ID);
	  private static readonly DummyFraCurveNode NODE2 = DummyFraCurveNode.of(Period.ofMonths(3), GBP_LIBOR_3M, GBP_LIBOR_3M_ID);
	  private static readonly FixedInflationSwapCurveNode NODE_I1 = FixedInflationSwapCurveNode.of(FixedInflationSwapTemplate.of(Tenor.TENOR_5Y, GBP_FIXED_ZC_GB_RPI), GBP_LIBOR_1M_ID);
	  private static readonly FixedInflationSwapCurveNode NODE_I2 = FixedInflationSwapCurveNode.of(FixedInflationSwapTemplate.of(Tenor.TENOR_10Y, GBP_FIXED_ZC_GB_RPI), GBP_LIBOR_1M_ID);
	  private static readonly CurveNodeDateOrder DROP_THIS_2D = CurveNodeDateOrder.of(2, DROP_THIS);
	  private static readonly CurveName CURVE_NAME1 = CurveName.of("Test");
	  private static readonly CurveName CURVE_NAME2 = CurveName.of("Test2");
	  private static readonly CurveName CURVE_NAME_I = CurveName.of("Test-CPI");
	  private static readonly InterpolatedNodalCurveDefinition CURVE_DEFN1 = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME1).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(ImmutableList.of(NODE1, NODE2)).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
	  private static readonly InterpolatedNodalCurveDefinition CURVE_DEFN1B = CURVE_DEFN1.toBuilder().dayCount(ACT_364).build();
	  private static readonly InterpolatedNodalCurveDefinition CURVE_DEFN2 = CURVE_DEFN1.toBuilder().name(CURVE_NAME2).build();
	  private static readonly InterpolatedNodalCurveDefinition CURVE_DEFN_I = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME_I).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.PRICE_INDEX).nodes(ImmutableList.of(NODE_I1, NODE_I2)).interpolator(CurveInterpolators.LOG_LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
	  private static readonly RatesCurveGroupEntry ENTRY1 = RatesCurveGroupEntry.builder().curveName(CURVE_NAME1).discountCurrencies(GBP).indices(GBP_LIBOR_1W, GBP_SONIA).build();
	  private static readonly RatesCurveGroupEntry ENTRY2 = RatesCurveGroupEntry.builder().curveName(CURVE_NAME2).indices(GBP_LIBOR_1M, GBP_LIBOR_3M).build();
	  private static readonly RatesCurveGroupEntry ENTRY3 = RatesCurveGroupEntry.builder().curveName(CURVE_NAME1).discountCurrencies(GBP).indices(GBP_LIBOR_1M, GBP_LIBOR_3M).build();
	  private static readonly DoubleArray SEASONALITY_ADDITIVE = DoubleArray.of(1.0, 1.5, 1.0, -0.5, -0.5, -1.0, -1.5, 0.0, 0.5, 1.0, 1.0, -2.5);
	  private static readonly SeasonalityDefinition SEASONALITY_ADDITIVE_DEF = SeasonalityDefinition.of(SEASONALITY_ADDITIVE, ShiftType.SCALED);

	  public virtual void test_builder1()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addDiscountCurve(CURVE_DEFN1, GBP).addForwardCurve(CURVE_DEFN1, GBP_SONIA).addForwardCurve(CURVE_DEFN1, GBP_LIBOR_1W).addForwardCurve(CURVE_DEFN2, GBP_LIBOR_1M, GBP_LIBOR_3M).build();
		assertEquals(test.Name, CurveGroupName.of("Test"));
		assertEquals(test.Entries, ImmutableList.of(ENTRY1, ENTRY2));
		assertEquals(test.findDiscountCurveName(GBP), CURVE_NAME1);
		assertEquals(test.findDiscountCurveName(USD), null);
		assertEquals(test.findForwardCurveName(GBP_LIBOR_1W), CURVE_NAME1);
		assertEquals(test.findForwardCurveName(GBP_LIBOR_1M), CURVE_NAME2);
		assertEquals(test.findForwardCurveName(GBP_LIBOR_6M), null);
		assertEquals(test.findForwardCurveNames(GBP_LIBOR), ImmutableSet.of(CURVE_NAME1, CURVE_NAME2));
		assertEquals(test.findEntry(CurveName.of("Test")), ENTRY1);
		assertEquals(test.findEntry(CurveName.of("Test2")), ENTRY2);
		assertEquals(test.findEntry(CurveName.of("Rubbish")), null);
		assertEquals(test.findCurveDefinition(CurveName.of("Test")), CURVE_DEFN1);
		assertEquals(test.findCurveDefinition(CurveName.of("Test2")), CURVE_DEFN2);
		assertEquals(test.findCurveDefinition(CurveName.of("Rubbish")), null);
	  }

	  public virtual void test_builder2()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).build();
		assertEquals(test.Name, CurveGroupName.of("Test"));
		assertEquals(test.Entries, ImmutableList.of(ENTRY3));
		assertEquals(test.findEntry(CurveName.of("Test")), ENTRY3);
		assertEquals(test.findEntry(CurveName.of("Test2")), null);
		assertEquals(test.findEntry(CurveName.of("Rubbish")), null);
		assertEquals(test.findCurveDefinition(CurveName.of("Test")), CURVE_DEFN1);
		assertEquals(test.findCurveDefinition(CurveName.of("Test2")), null);
		assertEquals(test.findCurveDefinition(CurveName.of("Rubbish")), null);
	  }

	  public virtual void test_builder_seasonality()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).addSeasonality(CURVE_NAME_I, SEASONALITY_ADDITIVE_DEF).build();
		assertEquals(test.Name, CurveGroupName.of("Test"));
		assertEquals(test.Entries, ImmutableList.of(ENTRY3));
		assertEquals(test.findEntry(CurveName.of("Test")), ENTRY3);
		assertEquals(test.findEntry(CurveName.of("Test2")), null);
		assertEquals(test.findEntry(CurveName.of("Rubbish")), null);
		assertEquals(test.findCurveDefinition(CurveName.of("Test")), CURVE_DEFN1);
		assertEquals(test.findCurveDefinition(CurveName.of("Test2")), null);
		assertEquals(test.findCurveDefinition(CurveName.of("Rubbish")), null);
		ImmutableMap<CurveName, SeasonalityDefinition> seasonMap = test.SeasonalityDefinitions;
		assertTrue(seasonMap.size() == 1);
		assertEquals(seasonMap.get(CURVE_NAME_I), SEASONALITY_ADDITIVE_DEF);
	  }

	  public virtual void test_builder3()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addDiscountCurve(CURVE_NAME1, GBP).addForwardCurve(CURVE_NAME1, GBP_SONIA).addForwardCurve(CURVE_NAME1, GBP_LIBOR_1W).addForwardCurve(CURVE_NAME2, GBP_LIBOR_1M, GBP_LIBOR_3M).build();
		assertEquals(test.Name, CurveGroupName.of("Test"));
		assertEquals(test.Entries, ImmutableList.of(ENTRY1, ENTRY2));
		assertEquals(test.findEntry(CurveName.of("Test")), ENTRY1);
		assertEquals(test.findEntry(CurveName.of("Test2")), ENTRY2);
		assertEquals(test.findEntry(CurveName.of("Rubbish")), null);
	  }

	  public virtual void test_builder4()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_NAME1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).build();
		assertEquals(test.Name, CurveGroupName.of("Test"));
		assertEquals(test.Entries, ImmutableList.of(ENTRY3));
		assertEquals(test.findEntry(CurveName.of("Test")), ENTRY3);
		assertEquals(test.findEntry(CurveName.of("Test2")), null);
		assertEquals(test.findEntry(CurveName.of("Rubbish")), null);
	  }

	  public virtual void test_missingEntries()
	  {
		assertThrowsIllegalArg(() => RatesCurveGroupDefinition.of(CurveGroupName.of("group"), ImmutableList.of(ENTRY1), ImmutableList.of(CURVE_DEFN1, CURVE_DEFN2)), "An entry must be provided .* \\[Test2\\]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filtered()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(5), GBP_LIBOR_1M, GBP_LIBOR_1M_ID);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(10), GBP_LIBOR_1M, GBP_LIBOR_1M_ID);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, GBP_LIBOR_1M_ID, DROP_THIS_2D);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);
		LocalDate valuationDate = date(2015, 6, 30);

		InterpolatedNodalCurveDefinition curveDefn = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME1).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(nodes).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(curveDefn, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).build();
		RatesCurveGroupDefinition expected = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(curveDefn.filtered(valuationDate, REF_DATA), GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).build();

		assertEquals(test.filtered(valuationDate, REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_metadata()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).build();

		LocalDate valuationDate = date(2015, 6, 30);
		CurveMetadata meta = CURVE_DEFN1.metadata(valuationDate, REF_DATA);
		assertEquals(test.metadata(valuationDate, REF_DATA), ImmutableList.of(meta));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_tradesInitialGuesses()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).build();

		MarketData marketData = ImmutableMarketData.of(date(2015, 6, 30), ImmutableMap.of(GBP_LIBOR_1M_ID, 0.5d, GBP_LIBOR_3M_ID, 1.5d));
		Trade trade1 = NODE1.trade(1d, marketData, REF_DATA);
		Trade trade2 = NODE2.trade(1d, marketData, REF_DATA);
		assertEquals(test.TotalParameterCount, 2);
		assertEquals(test.resolvedTrades(marketData, REF_DATA), ImmutableList.of(trade1, trade2));
		assertEquals(test.initialGuesses(marketData), ImmutableList.of(0.5d, 1.5d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_bind()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).addForwardCurve(CURVE_DEFN_I, GB_RPI).addSeasonality(CURVE_NAME_I, SEASONALITY_ADDITIVE_DEF).build();
		LocalDate valuationDate = LocalDate.of(2015, 11, 10);
		LocalDate lastFixingDate = LocalDate.of(2015, 10, 31);
		LocalDate otherFixingDate = LocalDate.of(2015, 9, 30);
		double lastFixingValue = 234.56;
		IDictionary<Index, LocalDateDoubleTimeSeries> map = ImmutableMap.of(GB_RPI, LocalDateDoubleTimeSeries.builder().put(lastFixingDate, 234.56).put(otherFixingDate, lastFixingValue - 1).build());
		RatesCurveGroupDefinition testBound = test.bindTimeSeries(valuationDate, map);
		IList<CurveDefinition> list = testBound.CurveDefinitions;
		assertEquals(list.Count, 2);
		assertTrue(list[0] is InterpolatedNodalCurveDefinition);
		assertTrue(list[1] is InflationNodalCurveDefinition);
		InflationNodalCurveDefinition seasonDef = (InflationNodalCurveDefinition) list[1];
		assertEquals(seasonDef.CurveWithoutFixingDefinition, CURVE_DEFN_I);
		assertEquals(seasonDef.LastFixingMonth, YearMonth.from(lastFixingDate));
		assertEquals(seasonDef.LastFixingValue, lastFixingValue);
		assertEquals(seasonDef.Name, CURVE_NAME_I);
		assertEquals(seasonDef.SeasonalityDefinition, SEASONALITY_ADDITIVE_DEF);
		assertEquals(seasonDef.YValueType, ValueType.PRICE_INDEX);
	  }

	  public virtual void test_bind_after_last_fixing()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).addForwardCurve(CURVE_DEFN_I, GB_RPI).addSeasonality(CURVE_NAME_I, SEASONALITY_ADDITIVE_DEF).build();
		LocalDate valuationDate = LocalDate.of(2015, 10, 15);
		LocalDate lastFixingDate = LocalDate.of(2015, 10, 31);
		LocalDate otherFixingDate = LocalDate.of(2015, 9, 30);
		LocalDate other2FixingDate = LocalDate.of(2015, 8, 31);
		double lastFixingValue = 234.56;
		IDictionary<Index, LocalDateDoubleTimeSeries> map = ImmutableMap.of(GB_RPI, LocalDateDoubleTimeSeries.builder().put(lastFixingDate, lastFixingValue).put(otherFixingDate, lastFixingValue - 1.0).put(other2FixingDate, lastFixingValue - 2.0).build());
		RatesCurveGroupDefinition testBound = test.bindTimeSeries(valuationDate, map);
		IList<CurveDefinition> list = testBound.CurveDefinitions;
		assertEquals(list.Count, 2);
		assertTrue(list[0] is InterpolatedNodalCurveDefinition);
		assertTrue(list[1] is InflationNodalCurveDefinition);
		InflationNodalCurveDefinition seasonDef = (InflationNodalCurveDefinition) list[1];
		assertEquals(seasonDef.CurveWithoutFixingDefinition, CURVE_DEFN_I);
		assertEquals(seasonDef.LastFixingMonth, YearMonth.from(otherFixingDate));
		assertEquals(seasonDef.LastFixingValue, lastFixingValue - 1.0);
		assertEquals(seasonDef.Name, CURVE_NAME_I);
		assertEquals(seasonDef.SeasonalityDefinition, SEASONALITY_ADDITIVE_DEF);
		assertEquals(seasonDef.YValueType, ValueType.PRICE_INDEX);
	  }

	  public virtual void test_bind_no_seasonality()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).addForwardCurve(CURVE_DEFN_I, GB_RPI).build();
		LocalDate valuationDate = LocalDate.of(2015, 11, 10);
		LocalDate lastFixingDate = LocalDate.of(2015, 10, 31);
		LocalDate otherFixingDate = LocalDate.of(2015, 9, 30);
		double lastFixingValue = 234.56;
		IDictionary<Index, LocalDateDoubleTimeSeries> map = ImmutableMap.of(GB_RPI, LocalDateDoubleTimeSeries.builder().put(lastFixingDate, 234.56).put(otherFixingDate, lastFixingValue - 1).build());
		RatesCurveGroupDefinition testBound = test.bindTimeSeries(valuationDate, map);
		IList<CurveDefinition> list = testBound.CurveDefinitions;
		assertEquals(list.Count, 2);
		assertTrue(list[0] is InterpolatedNodalCurveDefinition);
		assertTrue(list[1] is InflationNodalCurveDefinition);
		InflationNodalCurveDefinition seasonDef = (InflationNodalCurveDefinition) list[1];
		assertEquals(seasonDef.CurveWithoutFixingDefinition, CURVE_DEFN_I);
		assertEquals(seasonDef.LastFixingMonth, YearMonth.from(lastFixingDate));
		assertEquals(seasonDef.LastFixingValue, lastFixingValue);
		assertEquals(seasonDef.Name, CURVE_NAME_I);
		assertEquals(seasonDef.YValueType, ValueType.PRICE_INDEX);
		// Check the default
		assertTrue(seasonDef.SeasonalityDefinition.SeasonalityMonthOnMonth.equalWithTolerance(DoubleArray.filled(12, 1d), 1.0E-10));
		assertEquals(seasonDef.SeasonalityDefinition.AdjustmentType, ShiftType.SCALED);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith_sameCurveNames()
	  {
		RatesCurveGroupDefinition base1 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).addForwardCurve(CURVE_DEFN_I, GB_RPI).build();
		RatesCurveGroupDefinition base2 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("TestX")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_6M).build();
		RatesCurveGroupDefinition expected = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M, GBP_LIBOR_6M).addForwardCurve(CURVE_DEFN_I, GB_RPI).build();
		assertEquals(base1.combinedWith(base2), expected);
	  }

	  public virtual void test_combinedWith_differentCurveNames()
	  {
		RatesCurveGroupDefinition base1 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).addForwardCurve(CURVE_DEFN_I, GB_RPI).build();
		RatesCurveGroupDefinition base2 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("TestX")).addForwardCurve(CURVE_DEFN2, GBP_LIBOR_6M).build();
		RatesCurveGroupDefinition expected = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).addForwardCurve(CURVE_DEFN_I, GB_RPI).addForwardCurve(CURVE_DEFN2, GBP_LIBOR_6M).build();
		assertEquals(base1.combinedWith(base2), expected);
	  }

	  public virtual void test_combinedWith_sameCurveNamesClash()
	  {
		RatesCurveGroupDefinition base1 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addCurve(CURVE_DEFN1, GBP, GBP_LIBOR_1M, GBP_LIBOR_3M).addForwardCurve(CURVE_DEFN_I, GB_RPI).build();
		RatesCurveGroupDefinition base2 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("TestX")).addCurve(CURVE_DEFN1B, GBP, GBP_LIBOR_6M).build();
		assertThrowsIllegalArg(() => base1.combinedWith(base2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addDiscountCurve(CURVE_DEFN1, GBP).build();
		coverImmutableBean(test);
		RatesCurveGroupDefinition test2 = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test2")).addForwardCurve(CURVE_DEFN2, GBP_LIBOR_1M).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addDiscountCurve(CURVE_DEFN1, GBP).build();
		assertSerialization(test);
	  }

	  public virtual void test_withName()
	  {
		RatesCurveGroupDefinition test = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("Test")).addDiscountCurve(CURVE_DEFN1, GBP).build();
		RatesCurveGroupDefinition expected = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("NewName")).addDiscountCurve(CURVE_DEFN1, GBP).build();
		RatesCurveGroupDefinition withNewName = test.withName(CurveGroupName.of("NewName"));
		assertEquals(withNewName, expected);
	  }
	}

}