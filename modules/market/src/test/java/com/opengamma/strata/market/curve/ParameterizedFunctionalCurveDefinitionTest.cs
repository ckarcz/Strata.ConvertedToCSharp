using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1M;
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
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorParameterMetadata = com.opengamma.strata.market.param.TenorParameterMetadata;

	/// <summary>
	/// Test <seealso cref="ParameterizedFunctionalCurveDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParameterizedFunctionalCurveDefinitionTest
	public class ParameterizedFunctionalCurveDefinitionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2015, 9, 9);
	  private static readonly CurveName CURVE_NAME = CurveName.of("Test");
	  private static readonly QuoteId TICKER = QuoteId.of(StandardId.of("OG", "Ticker"));
	  private static readonly ImmutableList<DummyFraCurveNode> NODES = ImmutableList.of(DummyFraCurveNode.of(Period.ofMonths(1), GBP_LIBOR_1M, TICKER), DummyFraCurveNode.of(Period.ofMonths(3), GBP_LIBOR_1M, TICKER));
	  private static readonly ImmutableList<DummyFraCurveNode> NODES2 = ImmutableList.of(DummyFraCurveNode.of(Period.ofMonths(1), GBP_LIBOR_1M, TICKER), DummyFraCurveNode.of(Period.ofMonths(2), GBP_LIBOR_1M, TICKER));
	  private static readonly CurveNodeDateOrder DROP_THIS_2D = CurveNodeDateOrder.of(2, DROP_THIS);
	  private static readonly CurveNodeDateOrder DROP_OTHER_2D = CurveNodeDateOrder.of(2, DROP_OTHER);
	  private static readonly CurveNodeDateOrder EXCEPTION_2D = CurveNodeDateOrder.of(2, EXCEPTION);

	  private static readonly IList<double> INITIAL_PARAMS = DoubleArray.of(1.0, 1.0, 1.0).toList();
	  private static readonly ImmutableList<ParameterMetadata> PARAM_METADATA;
	  static ParameterizedFunctionalCurveDefinitionTest()
	  {
		TenorParameterMetadata param1 = TenorParameterMetadata.of(Tenor.TENOR_1Y);
		TenorParameterMetadata param2 = TenorParameterMetadata.of(Tenor.TENOR_5Y);
		TenorParameterMetadata param3 = TenorParameterMetadata.of(Tenor.TENOR_10Y);
		PARAM_METADATA = ImmutableList.of(param1, param2, param3);
	  }

	  private static readonly System.Func<DoubleArray, double, double> VALUE_FUNCTION = (DoubleArray t, double? u) =>
	  {
	  return t.get(0) + Math.Sin(t.get(1) + t.get(2) * u);
	  };
	  private static readonly System.Func<DoubleArray, double, double> DERIVATIVE_FUNCTION = (DoubleArray t, double? u) =>
	  {
	  return t.get(2) * Math.Cos(t.get(1) + t.get(2) * u);
	  };
	  private static readonly System.Func<DoubleArray, double, DoubleArray> SENSITIVITY_FUNCTION = (DoubleArray t, double? u) =>
	  {
	  return DoubleArray.of(1d, Math.Cos(t.get(1) + t.get(2) * u), u * Math.Cos(t.get(1) + t.get(2) * u));
	  };

	  public virtual void test_builder()
	  {
		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(NODES).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertEquals(test.Name, CURVE_NAME);
		assertEquals(test.XValueType, ValueType.YEAR_FRACTION);
		assertEquals(test.YValueType, ValueType.ZERO_RATE);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Nodes, NODES);
		assertEquals(test.ValueFunction, VALUE_FUNCTION);
		assertEquals(test.DerivativeFunction, DERIVATIVE_FUNCTION);
		assertEquals(test.SensitivityFunction, SENSITIVITY_FUNCTION);
		assertEquals(test.InitialGuess, INITIAL_PARAMS);
		assertEquals(test.ParameterCount, 3);
		assertEquals(test.ParameterMetadata, PARAM_METADATA);
	  }

	  public virtual void test_builder_noParamMetadata()
	  {
		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(NODES).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertEquals(test.Name, CURVE_NAME);
		assertEquals(test.XValueType, ValueType.YEAR_FRACTION);
		assertEquals(test.YValueType, ValueType.ZERO_RATE);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Nodes, NODES);
		assertEquals(test.ValueFunction, VALUE_FUNCTION);
		assertEquals(test.DerivativeFunction, DERIVATIVE_FUNCTION);
		assertEquals(test.SensitivityFunction, SENSITIVITY_FUNCTION);
		assertEquals(test.InitialGuess, INITIAL_PARAMS);
		assertEquals(test.ParameterCount, 3);
		assertEquals(test.ParameterMetadata, ParameterMetadata.listOfEmpty(3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filtered_dropThis_atStart()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(3), GBP_LIBOR_1M, TICKER, DROP_THIS_2D);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(4), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(nodes).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node2, node3));
	  }

	  public virtual void test_filtered_dropOther_atStart()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(3), GBP_LIBOR_1M, TICKER, DROP_OTHER_2D);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(4), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(nodes).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node1, node3));
	  }

	  public virtual void test_filtered_exception_atStart()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(3), GBP_LIBOR_1M, TICKER, EXCEPTION_2D);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(4), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(nodes).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertThrowsIllegalArg(() => test.filtered(VAL_DATE, REF_DATA), "Curve node dates clash.*");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filtered_dropThis_middle()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(3), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(4), GBP_LIBOR_1M, TICKER, DROP_THIS_2D);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(nodes).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node1, node3));
	  }

	  public virtual void test_filtered_dropOther_middle()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(3), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(4), GBP_LIBOR_1M, TICKER, DROP_OTHER_2D);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(nodes).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node2, node3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filtered_dropThis_atEnd()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(5), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(10), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER, DROP_THIS_2D);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(nodes).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node1, node2));
	  }

	  public virtual void test_filtered_dropOther_atEnd()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(5), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(10), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER, DROP_OTHER_2D);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(nodes).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node1, node3));
	  }

	  public virtual void test_filtered_exception_atEnd()
	  {
		DummyFraCurveNode node1 = DummyFraCurveNode.of(Period.ofDays(5), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node2 = DummyFraCurveNode.of(Period.ofDays(10), GBP_LIBOR_1M, TICKER);
		DummyFraCurveNode node3 = DummyFraCurveNode.of(Period.ofDays(11), GBP_LIBOR_1M, TICKER, EXCEPTION_2D);
		ImmutableList<DummyFraCurveNode> nodes = ImmutableList.of(node1, node2, node3);

		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(nodes).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
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

		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(nodes).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertEquals(test.filtered(VAL_DATE, REF_DATA).Nodes, ImmutableList.of(node1, node4, node6));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_metadata()
	  {
		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(NODES).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		DefaultCurveMetadata expected = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).parameterMetadata(PARAM_METADATA).build();
		assertEquals(test.metadata(VAL_DATE, REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_curve()
	  {
		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(NODES).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		DefaultCurveMetadata metadata = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).parameterMetadata(PARAM_METADATA).build();
		DoubleArray parameters = DoubleArray.of(1d, 1.5d, -0.5d);
		ParameterizedFunctionalCurve expected = ParameterizedFunctionalCurve.builder().metadata(metadata).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).parameters(parameters).build();
		assertEquals(test.curve(VAL_DATE, metadata, parameters), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toCurveParameterSize()
	  {
		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(NODES).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		assertEquals(test.toCurveParameterSize(), CurveParameterSize.of(CURVE_NAME, INITIAL_PARAMS.Count));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ParameterizedFunctionalCurveDefinition test = ParameterizedFunctionalCurveDefinition.builder().dayCount(ACT_365F).valueFunction(VALUE_FUNCTION).derivativeFunction(DERIVATIVE_FUNCTION).sensitivityFunction(SENSITIVITY_FUNCTION).initialGuess(INITIAL_PARAMS).name(CURVE_NAME).nodes(NODES).parameterMetadata(PARAM_METADATA).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
		coverImmutableBean(test);
		ImmutableList<double> initial = ImmutableList.of(12d);
		System.Func<DoubleArray, double, double> value = (DoubleArray t, double? u) =>
		{
		return t.get(0) * u;
		};
		System.Func<DoubleArray, double, double> deriv = (DoubleArray t, double? u) =>
		{
		return t.get(0);
		};
		System.Func<DoubleArray, double, DoubleArray> sensi = (DoubleArray t, double? u) =>
		{
		return DoubleArray.of(u);
		};
		ParameterizedFunctionalCurveDefinition test2 = ParameterizedFunctionalCurveDefinition.builder().dayCount(DayCounts.ACT_365L).valueFunction(value).derivativeFunction(deriv).sensitivityFunction(sensi).initialGuess(initial).name(CURVE_NAME).nodes(NODES2).xValueType(ValueType.MONTHS).yValueType(ValueType.DISCOUNT_FACTOR).build();
		coverBeanEquals(test, test2);
	  }

	}

}