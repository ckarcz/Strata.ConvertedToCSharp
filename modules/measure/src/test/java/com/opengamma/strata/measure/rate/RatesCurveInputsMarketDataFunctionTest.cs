using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ImmutableScenarioMarketData = com.opengamma.strata.data.scenario.ImmutableScenarioMarketData;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using InterpolatedNodalCurveDefinition = com.opengamma.strata.market.curve.InterpolatedNodalCurveDefinition;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveInputs = com.opengamma.strata.market.curve.RatesCurveInputs;
	using RatesCurveInputsId = com.opengamma.strata.market.curve.RatesCurveInputsId;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using FraCurveNode = com.opengamma.strata.market.curve.node.FraCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using FraTemplate = com.opengamma.strata.product.fra.type.FraTemplate;

	/// <summary>
	/// Test <seealso cref="RatesCurveInputsMarketDataFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesCurveInputsMarketDataFunctionTest
	public class RatesCurveInputsMarketDataFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2011, 3, 8);

	  /// <summary>
	  /// Test that the curve node requirements are extracted and returned.
	  /// </summary>
	  public virtual void requirements()
	  {
		FraCurveNode node1x4 = fraNode(1, "a");
		FraCurveNode node2x5 = fraNode(2, "b");
		FraCurveNode node3x6 = fraNode(3, "c");

		InterpolatedNodalCurveDefinition curve = InterpolatedNodalCurveDefinition.builder().name(CurveName.of("curve")).interpolator(CurveInterpolators.DOUBLE_QUADRATIC).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).nodes(node1x4, node2x5, node3x6).build();

		RatesCurveGroupDefinition groupDefn = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("curve group")).addDiscountCurve(curve, Currency.USD).build();

		MarketDataConfig marketDataConfig = MarketDataConfig.builder().add(groupDefn.Name, groupDefn).build();

		RatesCurveInputsMarketDataFunction marketDataFunction = new RatesCurveInputsMarketDataFunction();
		RatesCurveInputsId curveInputsId = RatesCurveInputsId.of(groupDefn.Name, curve.Name, ObservableSource.NONE);
		MarketDataRequirements requirements = marketDataFunction.requirements(curveInputsId, marketDataConfig);

		assertThat(requirements.Observables).contains(QuoteId.of(StandardId.of("test", "a"))).contains(QuoteId.of(StandardId.of("test", "b"))).contains(QuoteId.of(StandardId.of("test", "c")));
	  }

	  /// <summary>
	  /// Test that an exception is thrown if there is no curve group configuration corresponding to the ID
	  /// </summary>
	  public virtual void requirementsMissingGroupConfig()
	  {
		RatesCurveInputsMarketDataFunction marketDataFunction = new RatesCurveInputsMarketDataFunction();
		RatesCurveInputsId curveInputsId = RatesCurveInputsId.of(CurveGroupName.of("curve group"), CurveName.of("curve"), ObservableSource.NONE);
		assertThrowsIllegalArg(() => marketDataFunction.requirements(curveInputsId, MarketDataConfig.empty()));
	  }

	  /// <summary>
	  /// Test that requirements are empty if the curve group config exists but not the curve
	  /// </summary>
	  public virtual void requirementsMissingCurveDefinition()
	  {
		RatesCurveInputsMarketDataFunction marketDataFunction = new RatesCurveInputsMarketDataFunction();
		RatesCurveInputsId curveInputsId = RatesCurveInputsId.of(CurveGroupName.of("curve group"), CurveName.of("curve"), ObservableSource.NONE);
		RatesCurveGroupDefinition groupDefn = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("curve group")).build();
		MarketDataConfig marketDataConfig = MarketDataConfig.builder().add(groupDefn.Name, groupDefn).build();
		MarketDataRequirements requirements = marketDataFunction.requirements(curveInputsId, marketDataConfig);
		assertThat(requirements.Observables).Empty;
	  }

	  /// <summary>
	  /// Test that inputs are correctly built from market data.
	  /// </summary>
	  public virtual void build()
	  {
		FraCurveNode node1x4 = fraNode(1, "a");
		FraCurveNode node2x5 = fraNode(2, "b");
		FraCurveNode node3x6 = fraNode(3, "c");

		InterpolatedNodalCurveDefinition curveDefn = InterpolatedNodalCurveDefinition.builder().name(CurveName.of("curve")).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(DayCounts.ACT_ACT_ISDA).interpolator(CurveInterpolators.DOUBLE_QUADRATIC).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).nodes(node1x4, node2x5, node3x6).build();

		RatesCurveGroupDefinition groupDefn = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("curve group")).addDiscountCurve(curveDefn, Currency.USD).build();

		MarketDataConfig marketDataConfig = MarketDataConfig.builder().add(groupDefn.Name, groupDefn).build();

		QuoteId idA = QuoteId.of(StandardId.of("test", "a"));
		QuoteId idB = QuoteId.of(StandardId.of("test", "b"));
		QuoteId idC = QuoteId.of(StandardId.of("test", "c"));

		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(VAL_DATE).addValue(idA, 1d).addValue(idB, 2d).addValue(idC, 3d).build();

		RatesCurveInputsMarketDataFunction marketDataFunction = new RatesCurveInputsMarketDataFunction();
		RatesCurveInputsId curveInputsId = RatesCurveInputsId.of(groupDefn.Name, curveDefn.Name, ObservableSource.NONE);
		MarketDataBox<RatesCurveInputs> result = marketDataFunction.build(curveInputsId, marketDataConfig, marketData, REF_DATA);

		RatesCurveInputs curveInputs = result.SingleValue;
		assertThat(curveInputs.MarketData.get(idA)).isEqualTo(1d);
		assertThat(curveInputs.MarketData.get(idB)).isEqualTo(2d);
		assertThat(curveInputs.MarketData.get(idC)).isEqualTo(3d);

		IList<ParameterMetadata> expectedMetadata = ImmutableList.of(node1x4.metadata(VAL_DATE, REF_DATA), node2x5.metadata(VAL_DATE, REF_DATA), node3x6.metadata(VAL_DATE, REF_DATA));
		assertThat(curveInputs.CurveMetadata.ParameterMetadata).hasValue(expectedMetadata);
	  }

	  /// <summary>
	  /// Test that a failure is returned if there is no config for the curve group.
	  /// </summary>
	  public virtual void buildMissingGroupConfig()
	  {
		RatesCurveInputsMarketDataFunction marketDataFunction = new RatesCurveInputsMarketDataFunction();
		RatesCurveInputsId curveInputsId = RatesCurveInputsId.of(CurveGroupName.of("curve group"), CurveName.of("curve"), ObservableSource.NONE);
		ScenarioMarketData emptyData = ScenarioMarketData.empty();
		assertThrows(() => marketDataFunction.build(curveInputsId, MarketDataConfig.empty(), emptyData, REF_DATA), typeof(System.ArgumentException), "No configuration found for type .*");
	  }

	  /// <summary>
	  /// Test that a failure is returned if there is config for the curve group but it doesn't contain the named curve.
	  /// </summary>
	  public virtual void buildMissingCurveDefinition()
	  {
		RatesCurveInputsMarketDataFunction marketDataFunction = new RatesCurveInputsMarketDataFunction();
		RatesCurveInputsId curveInputsId = RatesCurveInputsId.of(CurveGroupName.of("curve group"), CurveName.of("curve"), ObservableSource.NONE);
		RatesCurveGroupDefinition groupDefn = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("curve group")).build();
		MarketDataConfig marketDataConfig = MarketDataConfig.builder().add(groupDefn.Name, groupDefn).build();
		ScenarioMarketData emptyData = ScenarioMarketData.empty();

		assertThrows(() => marketDataFunction.build(curveInputsId, marketDataConfig, emptyData, REF_DATA), typeof(System.ArgumentException), "No curve named .*");
	  }

	  /// <summary>
	  /// Test that a failure is returned if the observable data isn't available.
	  /// </summary>
	  public virtual void buildMissingMarketData()
	  {
		FraCurveNode node1x4 = fraNode(1, "a");
		FraCurveNode node2x5 = fraNode(2, "b");
		FraCurveNode node3x6 = fraNode(3, "c");

		InterpolatedNodalCurveDefinition curve = InterpolatedNodalCurveDefinition.builder().name(CurveName.of("curve")).interpolator(CurveInterpolators.DOUBLE_QUADRATIC).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).nodes(node1x4, node2x5, node3x6).build();

		RatesCurveGroupDefinition groupDefn = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("curve group")).addDiscountCurve(curve, Currency.USD).build();

		MarketDataConfig marketDataConfig = MarketDataConfig.builder().add(groupDefn.Name, groupDefn).build();

		ScenarioMarketData emptyData = ScenarioMarketData.of(1, date(2016, 6, 30), ImmutableMap.of(), ImmutableMap.of());

		RatesCurveInputsMarketDataFunction marketDataFunction = new RatesCurveInputsMarketDataFunction();
		RatesCurveInputsId curveInputsId = RatesCurveInputsId.of(groupDefn.Name, curve.Name, ObservableSource.NONE);

		assertThrows(() => marketDataFunction.build(curveInputsId, marketDataConfig, emptyData, REF_DATA), typeof(MarketDataNotFoundException));
	  }

	  //-------------------------------------------------------------------------
	  private static FraCurveNode fraNode(int startTenor, string marketDataId)
	  {
		Period periodToStart = Period.ofMonths(startTenor);
		FraTemplate template = FraTemplate.of(periodToStart, IborIndices.USD_LIBOR_3M);
		return FraCurveNode.of(template, QuoteId.of(StandardId.of("test", marketDataId)));
	  }

	}

}