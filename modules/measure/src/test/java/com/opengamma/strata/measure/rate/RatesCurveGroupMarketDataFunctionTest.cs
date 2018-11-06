using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.offset;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using RateIndex = com.opengamma.strata.basics.index.RateIndex;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ImmutableScenarioMarketData = com.opengamma.strata.data.scenario.ImmutableScenarioMarketData;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ValueType = com.opengamma.strata.market.ValueType;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurveDefinition = com.opengamma.strata.market.curve.InterpolatedNodalCurveDefinition;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveGroupId = com.opengamma.strata.market.curve.RatesCurveGroupId;
	using RatesCurveInputs = com.opengamma.strata.market.curve.RatesCurveInputs;
	using RatesCurveInputsId = com.opengamma.strata.market.curve.RatesCurveInputsId;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using FixedIborSwapCurveNode = com.opengamma.strata.market.curve.node.FixedIborSwapCurveNode;
	using FraCurveNode = com.opengamma.strata.market.curve.node.FraCurveNode;
	using FxSwapCurveNode = com.opengamma.strata.market.curve.node.FxSwapCurveNode;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using DiscountingFraTradePricer = com.opengamma.strata.pricer.fra.DiscountingFraTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapTradePricer = com.opengamma.strata.pricer.swap.DiscountingSwapTradePricer;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using FxSwapConventions = com.opengamma.strata.product.fx.type.FxSwapConventions;
	using FxSwapTemplate = com.opengamma.strata.product.fx.type.FxSwapTemplate;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;

	/// <summary>
	/// Test <seealso cref="RatesCurveGroupMarketDataFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesCurveGroupMarketDataFunctionTest
	public class RatesCurveGroupMarketDataFunctionTest
	{

	  /// <summary>
	  /// The calibrator. </summary>
	  private static readonly RatesCurveCalibrator CALIBRATOR = RatesCurveCalibrator.standard();
	  /// <summary>
	  /// The maximum allowable PV when round-tripping an instrument used to calibrate a curve. </summary>
	  private const double PV_TOLERANCE = 5e-10;
	  /// <summary>
	  /// The reference data. </summary>
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  /// <summary>
	  /// Tests calibration a curve containing FRAs and pricing the curve instruments using the curve.
	  /// </summary>
	  public virtual void roundTripFra()
	  {
		InterpolatedNodalCurveDefinition curveDefn = CurveTestUtils.fraCurveDefinition();

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<FraCurveNode> nodes = curveDefn.Nodes.Select(typeof(FraCurveNode).cast).collect(toImmutableList());

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.data.MarketDataId<?>> keys = nodes.stream().map(CurveTestUtils::key).collect(toImmutableList());
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<MarketDataId<object>> keys = nodes.Select(CurveTestUtils.key).collect(toImmutableList());
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, double> inputData = com.google.common.collect.ImmutableMap.builder<com.opengamma.strata.data.MarketDataId<?>, double>().put(keys.get(0), 0.003).put(keys.get(1), 0.0033).put(keys.get(2), 0.0037).put(keys.get(3), 0.0054).put(keys.get(4), 0.007).put(keys.get(5), 0.0091).put(keys.get(6), 0.0134).build();
		IDictionary<MarketDataId<object>, double> inputData = ImmutableMap.builder<MarketDataId<object>, double>().put(keys[0], 0.003).put(keys[1], 0.0033).put(keys[2], 0.0037).put(keys[3], 0.0054).put(keys[4], 0.007).put(keys[5], 0.0091).put(keys[6], 0.0134).build();

		CurveGroupName groupName = CurveGroupName.of("Curve Group");
		CurveName curveName = CurveName.of("FRA Curve");
		RatesCurveInputs curveInputs = RatesCurveInputs.of(inputData, DefaultCurveMetadata.of(curveName));

		RatesCurveGroupDefinition groupDefn = RatesCurveGroupDefinition.builder().name(groupName).addCurve(curveDefn, Currency.USD, IborIndices.USD_LIBOR_3M).build();

		RatesCurveGroupMarketDataFunction function = new RatesCurveGroupMarketDataFunction();
		LocalDate valuationDate = date(2011, 3, 8);
		ScenarioMarketData inputMarketData = ImmutableScenarioMarketData.builder(valuationDate).addValue(RatesCurveInputsId.of(groupName, curveName, ObservableSource.NONE), curveInputs).build();
		MarketDataBox<RatesCurveGroup> curveGroup = function.buildCurveGroup(groupDefn, CALIBRATOR, inputMarketData, REF_DATA, ObservableSource.NONE);

		Curve curve = curveGroup.SingleValue.findDiscountCurve(Currency.USD).get();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> marketDataMap = com.google.common.collect.ImmutableMap.builder<com.opengamma.strata.data.MarketDataId<?>, Object>().putAll(inputData).put(com.opengamma.strata.market.curve.CurveId.of(groupName, curveName), curve).build();
		IDictionary<MarketDataId<object>, object> marketDataMap = ImmutableMap.builder<MarketDataId<object>, object>().putAll(inputData).put(CurveId.of(groupName, curveName), curve).build();

		MarketData marketData = ImmutableMarketData.of(valuationDate, marketDataMap);
		TestMarketDataMap scenarioMarketData = new TestMarketDataMap(valuationDate, marketDataMap, ImmutableMap.of());
		RatesMarketDataLookup lookup = RatesMarketDataLookup.of(groupDefn);
		RatesProvider ratesProvider = lookup.ratesProvider(scenarioMarketData.scenario(0));

		// The PV should be zero for an instrument used to build the curve
		nodes.ForEach(node => checkFraPvIsZero(node, ratesProvider, marketData));
	  }

	  public virtual void roundTripFraAndFixedFloatSwap()
	  {
		CurveGroupName groupName = CurveGroupName.of("Curve Group");
		InterpolatedNodalCurveDefinition curveDefn = CurveTestUtils.fraSwapCurveDefinition();
		CurveName curveName = curveDefn.Name;
		IList<CurveNode> nodes = curveDefn.Nodes;

		RatesCurveGroupDefinition groupDefn = RatesCurveGroupDefinition.builder().name(groupName).addCurve(curveDefn, Currency.USD, IborIndices.USD_LIBOR_3M).build();

		RatesCurveGroupMarketDataFunction function = new RatesCurveGroupMarketDataFunction();
		LocalDate valuationDate = date(2011, 3, 8);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, double> inputData = com.google.common.collect.ImmutableMap.builder<com.opengamma.strata.data.MarketDataId<?>, double>().put(CurveTestUtils.key(nodes.get(0)), 0.0037).put(CurveTestUtils.key(nodes.get(1)), 0.0054).put(CurveTestUtils.key(nodes.get(2)), 0.005).put(CurveTestUtils.key(nodes.get(3)), 0.0087).put(CurveTestUtils.key(nodes.get(4)), 0.012).build();
		IDictionary<MarketDataId<object>, double> inputData = ImmutableMap.builder<MarketDataId<object>, double>().put(CurveTestUtils.key(nodes[0]), 0.0037).put(CurveTestUtils.key(nodes[1]), 0.0054).put(CurveTestUtils.key(nodes[2]), 0.005).put(CurveTestUtils.key(nodes[3]), 0.0087).put(CurveTestUtils.key(nodes[4]), 0.012).build();

		RatesCurveInputs curveInputs = RatesCurveInputs.of(inputData, DefaultCurveMetadata.of(curveName));
		ScenarioMarketData inputMarketData = ImmutableScenarioMarketData.builder(valuationDate).addValue(RatesCurveInputsId.of(groupName, curveName, ObservableSource.NONE), curveInputs).build();

		MarketDataBox<RatesCurveGroup> curveGroup = function.buildCurveGroup(groupDefn, CALIBRATOR, inputMarketData, REF_DATA, ObservableSource.NONE);
		Curve curve = curveGroup.SingleValue.findDiscountCurve(Currency.USD).get();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> marketDataMap = com.google.common.collect.ImmutableMap.builder<com.opengamma.strata.data.MarketDataId<?>, Object>().putAll(inputData).put(com.opengamma.strata.market.curve.CurveId.of(groupName, curveName), curve).build();
		IDictionary<MarketDataId<object>, object> marketDataMap = ImmutableMap.builder<MarketDataId<object>, object>().putAll(inputData).put(CurveId.of(groupName, curveName), curve).build();
		MarketData marketData = ImmutableMarketData.of(valuationDate, marketDataMap);
		TestMarketDataMap scenarioMarketData = new TestMarketDataMap(valuationDate, marketDataMap, ImmutableMap.of());
		RatesMarketDataLookup lookup = RatesMarketDataLookup.of(groupDefn);
		RatesProvider ratesProvider = lookup.ratesProvider(scenarioMarketData.scenario(0));

		checkFraPvIsZero((FraCurveNode) nodes[0], ratesProvider, marketData);
		checkFraPvIsZero((FraCurveNode) nodes[1], ratesProvider, marketData);
		checkSwapPvIsZero((FixedIborSwapCurveNode) nodes[2], ratesProvider, marketData);
		checkSwapPvIsZero((FixedIborSwapCurveNode) nodes[3], ratesProvider, marketData);
		checkSwapPvIsZero((FixedIborSwapCurveNode) nodes[4], ratesProvider, marketData);
	  }

	  /// <summary>
	  /// Tests that par rates and ibor index are required for curves.
	  /// </summary>
	  public virtual void requirements()
	  {
		FraCurveNode node1x4 = CurveTestUtils.fraNode(1, "foo");
		FraCurveNode node2x5 = CurveTestUtils.fraNode(2, "foo");
		IList<CurveNode> nodes = ImmutableList.of(node1x4, node2x5);
		CurveGroupName groupName = CurveGroupName.of("Curve Group");
		CurveName curveName = CurveName.of("FRA Curve");
		ObservableSource obsSource = ObservableSource.of("Vendor");

		InterpolatedNodalCurveDefinition curveDefn = InterpolatedNodalCurveDefinition.builder().name(curveName).nodes(nodes).interpolator(CurveInterpolators.DOUBLE_QUADRATIC).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();

		RateIndex ibor = IborIndices.USD_LIBOR_3M;
		RatesCurveGroupDefinition groupDefn = RatesCurveGroupDefinition.builder().name(groupName).addCurve(curveDefn, Currency.USD, ibor).build();

		MarketDataConfig marketDataConfig = MarketDataConfig.builder().add(groupName, groupDefn).build();

		RatesCurveGroupMarketDataFunction function = new RatesCurveGroupMarketDataFunction();
		RatesCurveGroupId curveGroupId = RatesCurveGroupId.of(groupName, obsSource);
		MarketDataRequirements requirements = function.requirements(curveGroupId, marketDataConfig);

		assertThat(requirements.NonObservables).contains(RatesCurveInputsId.of(groupName, curveName, obsSource));
		assertThat(requirements.TimeSeries.contains(IndexQuoteId.of(ibor)));
	  }

	  public virtual void metadata()
	  {
		CurveGroupName groupName = CurveGroupName.of("Curve Group");

		InterpolatedNodalCurveDefinition fraCurveDefn = CurveTestUtils.fraCurveDefinition();
		IList<CurveNode> fraNodes = fraCurveDefn.Nodes;

		RatesCurveGroupDefinition groupDefn = RatesCurveGroupDefinition.builder().name(groupName).addForwardCurve(fraCurveDefn, IborIndices.USD_LIBOR_3M).build();

		MarketDataConfig marketDataConfig = MarketDataConfig.builder().add(groupName, groupDefn).build();

		RatesCurveGroupId curveGroupId = RatesCurveGroupId.of(groupName);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, double> fraInputData = com.google.common.collect.ImmutableMap.builder<com.opengamma.strata.data.MarketDataId<?>, double>().put(CurveTestUtils.key(fraNodes.get(0)), 0.003).put(CurveTestUtils.key(fraNodes.get(1)), 0.0033).put(CurveTestUtils.key(fraNodes.get(2)), 0.0037).put(CurveTestUtils.key(fraNodes.get(3)), 0.0054).put(CurveTestUtils.key(fraNodes.get(4)), 0.007).put(CurveTestUtils.key(fraNodes.get(5)), 0.0091).put(CurveTestUtils.key(fraNodes.get(6)), 0.0134).build();
		IDictionary<MarketDataId<object>, double> fraInputData = ImmutableMap.builder<MarketDataId<object>, double>().put(CurveTestUtils.key(fraNodes[0]), 0.003).put(CurveTestUtils.key(fraNodes[1]), 0.0033).put(CurveTestUtils.key(fraNodes[2]), 0.0037).put(CurveTestUtils.key(fraNodes[3]), 0.0054).put(CurveTestUtils.key(fraNodes[4]), 0.007).put(CurveTestUtils.key(fraNodes[5]), 0.0091).put(CurveTestUtils.key(fraNodes[6]), 0.0134).build();

		LocalDate valuationDate = date(2011, 3, 8);
		RatesCurveInputs fraCurveInputs = RatesCurveInputs.of(fraInputData, fraCurveDefn.metadata(valuationDate, REF_DATA));
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(valuationDate).addValue(RatesCurveInputsId.of(groupName, fraCurveDefn.Name, ObservableSource.NONE), fraCurveInputs).build();

		RatesCurveGroupMarketDataFunction function = new RatesCurveGroupMarketDataFunction();
		MarketDataBox<RatesCurveGroup> curveGroup = function.build(curveGroupId, marketDataConfig, marketData, REF_DATA);

		// Check the FRA curve identifiers are the expected tenors
		Curve forwardCurve = curveGroup.SingleValue.findForwardCurve(IborIndices.USD_LIBOR_3M).get();
		IList<ParameterMetadata> forwardMetadata = forwardCurve.Metadata.ParameterMetadata.get();

//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<object> forwardTenors = forwardMetadata.Select(ParameterMetadata::getIdentifier).collect(toImmutableList());

		IList<Tenor> expectedForwardTenors = ImmutableList.of(Tenor.TENOR_4M, Tenor.TENOR_5M, Tenor.TENOR_6M, Tenor.TENOR_9M, Tenor.TENOR_12M, Tenor.ofMonths(15), Tenor.ofMonths(21));

		assertThat(forwardTenors).isEqualTo(expectedForwardTenors);

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<ParameterMetadata> expectedForwardMetadata = fraNodes.Select(node => node.metadata(valuationDate, REF_DATA)).collect(toImmutableList());

		assertThat(forwardMetadata).isEqualTo(expectedForwardMetadata);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void duplicateInputDataKeys()
	  {
		FxSwapTemplate template1 = FxSwapTemplate.of(Period.ofMonths(1), FxSwapConventions.EUR_USD);
		FxSwapTemplate template2 = FxSwapTemplate.of(Period.ofMonths(2), FxSwapConventions.EUR_USD);
		QuoteId pointsKey1a = QuoteId.of(StandardId.of("test", "1a"));
		QuoteId pointsKey1b = QuoteId.of(StandardId.of("test", "1b"));
		QuoteId pointsKey2a = QuoteId.of(StandardId.of("test", "2a"));
		QuoteId pointsKey2b = QuoteId.of(StandardId.of("test", "2b"));
		FxSwapCurveNode node1a = FxSwapCurveNode.of(template1, pointsKey1a);
		FxSwapCurveNode node1b = FxSwapCurveNode.of(template2, pointsKey1b);
		FxSwapCurveNode node2 = FxSwapCurveNode.of(template1, pointsKey2a);
		FxSwapCurveNode node2b = FxSwapCurveNode.of(template2, pointsKey2b);
		CurveName curveName1 = CurveName.of("curve1");
		InterpolatedNodalCurveDefinition curve1 = InterpolatedNodalCurveDefinition.builder().name(curveName1).nodes(node1a, node1b).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_360).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.LINEAR).extrapolatorRight(CurveExtrapolators.LINEAR).build();
		CurveName curveName2 = CurveName.of("curve2");
		InterpolatedNodalCurveDefinition curve2 = InterpolatedNodalCurveDefinition.builder().name(curveName2).nodes(node2, node2b).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(ACT_360).interpolator(CurveInterpolators.LINEAR).extrapolatorLeft(CurveExtrapolators.LINEAR).extrapolatorRight(CurveExtrapolators.LINEAR).build();
		CurveGroupName curveGroupName = CurveGroupName.of("group");
		RatesCurveGroupDefinition groupDefinition = RatesCurveGroupDefinition.builder().name(curveGroupName).addDiscountCurve(curve1, Currency.EUR).addDiscountCurve(curve2, Currency.USD).build();

		RatesCurveGroupMarketDataFunction fn = new RatesCurveGroupMarketDataFunction();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> marketDataMap1 = com.google.common.collect.ImmutableMap.of(com.opengamma.strata.data.FxRateId.of(com.opengamma.strata.basics.currency.Currency.EUR, com.opengamma.strata.basics.currency.Currency.USD), com.opengamma.strata.basics.currency.FxRate.of(com.opengamma.strata.basics.currency.Currency.EUR, com.opengamma.strata.basics.currency.Currency.USD, 1.01), pointsKey1a, 0.1d, pointsKey1b, 0.2d);
		IDictionary<MarketDataId<object>, object> marketDataMap1 = ImmutableMap.of(FxRateId.of(Currency.EUR, Currency.USD), FxRate.of(Currency.EUR, Currency.USD, 1.01), pointsKey1a, 0.1d, pointsKey1b, 0.2d);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> marketDataMap2 = com.google.common.collect.ImmutableMap.of(com.opengamma.strata.data.FxRateId.of(com.opengamma.strata.basics.currency.Currency.EUR, com.opengamma.strata.basics.currency.Currency.USD), com.opengamma.strata.basics.currency.FxRate.of(com.opengamma.strata.basics.currency.Currency.EUR, com.opengamma.strata.basics.currency.Currency.USD, 1.01), pointsKey2a, 0.1d, pointsKey2b, 0.2d);
		IDictionary<MarketDataId<object>, object> marketDataMap2 = ImmutableMap.of(FxRateId.of(Currency.EUR, Currency.USD), FxRate.of(Currency.EUR, Currency.USD, 1.01), pointsKey2a, 0.1d, pointsKey2b, 0.2d);
		RatesCurveInputs curveInputs1 = RatesCurveInputs.of(marketDataMap1, DefaultCurveMetadata.of("curve1"));
		RatesCurveInputs curveInputs2 = RatesCurveInputs.of(marketDataMap2, DefaultCurveMetadata.of("curve2"));
		ImmutableScenarioMarketData marketData = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addValue(RatesCurveInputsId.of(curveGroupName, curveName1, ObservableSource.NONE), curveInputs1).addValue(RatesCurveInputsId.of(curveGroupName, curveName2, ObservableSource.NONE), curveInputs2).build();
		fn.buildCurveGroup(groupDefinition, CALIBRATOR, marketData, REF_DATA, ObservableSource.NONE);

		// This has a duplicate key with a different value which should fail
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> badMarketDataMap = com.google.common.collect.ImmutableMap.of(com.opengamma.strata.data.FxRateId.of(com.opengamma.strata.basics.currency.Currency.EUR, com.opengamma.strata.basics.currency.Currency.USD), com.opengamma.strata.basics.currency.FxRate.of(com.opengamma.strata.basics.currency.Currency.EUR, com.opengamma.strata.basics.currency.Currency.USD, 1.02), pointsKey2a, 0.2d);
		IDictionary<MarketDataId<object>, object> badMarketDataMap = ImmutableMap.of(FxRateId.of(Currency.EUR, Currency.USD), FxRate.of(Currency.EUR, Currency.USD, 1.02), pointsKey2a, 0.2d);
		RatesCurveInputs badCurveInputs = RatesCurveInputs.of(badMarketDataMap, DefaultCurveMetadata.of("curve2"));
		ScenarioMarketData badMarketData = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addValue(RatesCurveInputsId.of(curveGroupName, curveName1, ObservableSource.NONE), curveInputs1).addValue(RatesCurveInputsId.of(curveGroupName, curveName2, ObservableSource.NONE), badCurveInputs).build();
		string msg = "Multiple unequal values found for identifier .*\\. Values: .* and .*";
		assertThrowsIllegalArg(() => fn.buildCurveGroup(groupDefinition, CALIBRATOR, badMarketData, REF_DATA, ObservableSource.NONE), msg);
	  }

	  //-----------------------------------------------------------------------------------------------------------

	  private void checkFraPvIsZero(FraCurveNode node, RatesProvider ratesProvider, MarketData marketDataMap)
	  {

		ResolvedFraTrade trade = node.resolvedTrade(1d, marketDataMap, REF_DATA);
		CurrencyAmount currencyAmount = DiscountingFraTradePricer.DEFAULT.presentValue(trade, ratesProvider);
		double pv = currencyAmount.Amount;
		assertThat(pv).isCloseTo(0, offset(PV_TOLERANCE));
	  }

	  private void checkSwapPvIsZero(FixedIborSwapCurveNode node, RatesProvider ratesProvider, MarketData marketDataMap)
	  {

		ResolvedSwapTrade trade = node.resolvedTrade(1d, marketDataMap, REF_DATA);
		MultiCurrencyAmount amount = DiscountingSwapTradePricer.DEFAULT.presentValue(trade, ratesProvider);
		double pv = amount.getAmount(Currency.USD).Amount;
		assertThat(pv).isCloseTo(0, offset(PV_TOLERANCE));
	  }

	}

}