/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1M;
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
//	import static com.opengamma.strata.market.curve.CurveNodeClashAction.DROP_OTHER;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.CurveNodeClashAction.DROP_THIS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.CurveNodeClashAction.EXCEPTION;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;

	/// <summary>
	/// Test <seealso cref="InterpolatedNodalCurveDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InterpolatedNodalCurveDefinitionTest
	public class InterpolatedNodalCurveDefinitionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 9, 9);
	  private static readonly LocalDate DATE1 = GBLO.resolve(REF_DATA).nextOrSame(VAL_DATE.plusMonths(2));
	  private static readonly LocalDate DATE2 = GBLO.resolve(REF_DATA).nextOrSame(VAL_DATE.plusMonths(4));
	  private static readonly CurveName CURVE_NAME = CurveName.of("Test");
	  private static readonly QuoteId TICKER = QuoteId.of(StandardId.of("OG", "Ticker"));
	  private static readonly ImmutableList<DummyFraCurveNode> NODES = ImmutableList.of(DummyFraCurveNode.of(Period.ofMonths(1), GBP_LIBOR_1M, TICKER), DummyFraCurveNode.of(Period.ofMonths(3), GBP_LIBOR_1M, TICKER));
	  private static readonly ImmutableList<DummyFraCurveNode> NODES2 = ImmutableList.of(DummyFraCurveNode.of(Period.ofMonths(1), GBP_LIBOR_1M, TICKER), DummyFraCurveNode.of(Period.ofMonths(2), GBP_LIBOR_1M, TICKER));
	  private static readonly CurveNodeDateOrder DROP_THIS_2D = CurveNodeDateOrder.of(2, DROP_THIS);
	  private static readonly CurveNodeDateOrder DROP_OTHER_2D = CurveNodeDateOrder.of(2, DROP_OTHER);
	  private static readonly CurveNodeDateOrder EXCEPTION_2D = CurveNodeDateOrder.of(2, EXCEPTION);

	  public virtual void test_builder()
	  {
		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(NODES).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertEquals(test.Name, CURVE_NAME);
		assertEquals(test.XValueType, ValueType.YEAR_FRACTION);
		assertEquals(test.YValueType, ValueType.ZERO_RATE);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Nodes, NODES);
		assertEquals(test.Interpolator, CurveInterpolators.LINEAR);
		assertEquals(test.ExtrapolatorLeft, CurveExtrapolators.FLAT);
		assertEquals(test.ExtrapolatorRight, CurveExtrapolators.FLAT);
		assertEquals(test.ParameterCount, 2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filtered_dropThis_atStart()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(3), GBP_LIBOR_1M, TICKER, DROP_THIS_2D);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(4), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(nodes).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node2, node3));
	  }

	  public virtual void test_filtered_dropOther_atStart()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(3), GBP_LIBOR_1M, TICKER, DROP_OTHER_2D);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(4), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(nodes).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node1, node3));
	  }

	  public virtual void test_filtered_exception_atStart()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(3), GBP_LIBOR_1M, TICKER, EXCEPTION_2D);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(4), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(nodes).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertThrowsIllegalArg(() => test.filtered(VAL_DATE, REF_DATA), "Curve node dates clash.*");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filtered_dropThis_middle()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(3), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(4), GBP_LIBOR_1M, TICKER, DROP_THIS_2D);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(nodes).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node1, node3));
	  }

	  public virtual void test_filtered_dropOther_middle()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(3), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(4), GBP_LIBOR_1M, TICKER, DROP_OTHER_2D);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(nodes).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node2, node3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filtered_dropThis_atEnd()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(5), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(10), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER, DROP_THIS_2D);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(nodes).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node1, node2));
	  }

	  public virtual void test_filtered_dropOther_atEnd()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(5), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(10), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER, DROP_OTHER_2D);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(nodes).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node1, node3));
	  }

	  public virtual void test_filtered_exception_atEnd()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(5), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(10), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER, EXCEPTION_2D);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(nodes).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertThrowsIllegalArg(() => test.filtered(VAL_DATE, REF_DATA), "Curve node dates clash.*");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filtered_dropOther_multiple()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(5), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(10), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node4 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER, DROP_OTHER_2D);
		DummyFraCurveNode node5 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node6 = DummyFraCurveNode.of(Period.ofDays(15), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3, node4, node5, node6);

		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(nodes).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node1, node4, node6));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_metadata()
	  {
		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(NODES).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		DefaultCurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).parameterMetadata(NODES.get(0).metadata(VAL_DATE, REF_DATA), NODES.get(1).metadata(VAL_DATE, REF_DATA)).build();
		assertEquals(test.metadata(VAL_DATE, REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_curve()
	  {
		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(NODES).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		DefaultCurveMetadata metadata = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).parameterMetadata(NODES.get(0).metadata(VAL_DATE, REF_DATA), NODES.get(1).metadata(VAL_DATE, REF_DATA)).build();
		InterpolatedNodalCurve expected = InterpolatedNodalCurve.builder().metadata(metadata).xValues(DoubleArray.of(ACT_365F.yearFraction(VAL_DATE, DATE1), ACT_365F.yearFraction(VAL_DATE, DATE2))).yValues(DoubleArray.of(1d, 1.5d)).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertEquals(test.curve(VAL_DATE, metadata, DoubleArray.of(1d, 1.5d)), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toCurveParameterSize()
	  {
		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(NODES).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertEquals(test.toCurveParameterSize(), CurveParameterSize.of(CURVE_NAME, NODES.size()));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(NODES).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		coverImmutableBean(test);
		InterpolatedNodalCurveDefinition test2 = InterpolatedNodalCurveDefinition.builder().name(CurveName.of("Foo")).nodes(NODES2).interpolator(CurveInterpolators.LOG_LINEAR).extrapolatorLeft(CurveExtrapolators.LOG_LINEAR).extrapolatorRight(CurveExtrapolators.LOG_LINEAR).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		InterpolatedNodalCurveDefinition test = InterpolatedNodalCurveDefinition.builder().name(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).nodes(NODES).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
		assertSerialization(test);
	  }

	}

}