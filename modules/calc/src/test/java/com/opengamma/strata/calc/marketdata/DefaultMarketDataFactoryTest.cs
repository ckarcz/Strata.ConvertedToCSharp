using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using FieldName = com.opengamma.strata.data.FieldName;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ScenarioPerturbation = com.opengamma.strata.data.scenario.ScenarioPerturbation;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DefaultMarketDataFactoryTest
	public class DefaultMarketDataFactoryTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly MarketDataConfig MARKET_DATA_CONFIG = MarketDataConfig.empty();

	  /// <summary>
	  /// Tests building time series from requirements.
	  /// </summary>
	  public virtual void buildTimeSeries()
	  {
		TestObservableId id1 = TestObservableId.of("1");
		TestObservableId id2 = TestObservableId.of("2");
		LocalDateDoubleTimeSeries timeSeries1 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1).put(date(2011, 3, 9), 2).put(date(2011, 3, 10), 3).build();
		LocalDateDoubleTimeSeries timeSeries2 = LocalDateDoubleTimeSeries.builder().put(date(2012, 4, 8), 10).put(date(2012, 4, 9), 20).put(date(2012, 4, 10), 30).build();
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeries = ImmutableMap.of(id1, timeSeries1, id2, timeSeries2);
		MarketDataFactory factory = MarketDataFactory.of(ObservableDataProvider.none(), new TestTimeSeriesProvider(timeSeries));

		MarketDataRequirements requirements = MarketDataRequirements.builder().addTimeSeries(id1, id2).build();
		MarketData suppliedData = MarketData.empty(date(2011, 3, 8));
		BuiltMarketData marketData = factory.create(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA);
		assertThat(marketData.getTimeSeries(id1)).isEqualTo(timeSeries1);
		assertThat(marketData.getTimeSeries(id2)).isEqualTo(timeSeries2);
		assertThat(marketData.TimeSeriesIds).isEqualTo(ImmutableSet.of(id1, id2));
	  }

	  /// <summary>
	  /// Tests non-observable market data values supplied by the user are included in the results.
	  /// </summary>
	  public virtual void buildSuppliedNonObservableValues()
	  {
		TestId id1 = new TestId("1");
		TestId id2 = new TestId("2");
		MarketData suppliedData = ImmutableMarketData.builder(date(2011, 3, 8)).addValue(id1, "foo").addValue(id2, "bar").build();
		MarketDataFactory factory = MarketDataFactory.of(ObservableDataProvider.none(), new TestTimeSeriesProvider(ImmutableMap.of()));
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();
		BuiltMarketData marketData = factory.create(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA);
		assertThat(marketData.getValue(id1)).isEqualTo("foo");
		assertThat(marketData.getValue(id2)).isEqualTo("bar");
	  }

	  /// <summary>
	  /// Tests building single values using market data functions.
	  /// </summary>
	  public virtual void buildNonObservableValues()
	  {
		ObservableId idA = new TestIdA("1");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> idC = new TestIdC("1");
		MarketDataId<object> idC = new TestIdC("1");
		LocalDateDoubleTimeSeries timeSeries = LocalDateDoubleTimeSeries.builder().put(date(2012, 4, 8), 10).put(date(2012, 4, 9), 20).put(date(2012, 4, 10), 30).build();

		MarketData suppliedData = ImmutableMarketData.builder(date(2011, 3, 8)).addTimeSeries(idA, timeSeries).build();
		MarketDataFactory factory = MarketDataFactory.of(ObservableDataProvider.none(), new TestTimeSeriesProvider(ImmutableMap.of()), new TestMarketDataFunctionC());

		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(idC).build();
		BuiltMarketData marketData = factory.create(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA);
		assertThat(marketData.getValue(idC)).isEqualTo(new TestMarketDataC(timeSeries));
	  }

	  /// <summary>
	  /// Tests building observable market data values.
	  /// </summary>
	  public virtual void buildObservableValues()
	  {
		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), new TestTimeSeriesProvider(ImmutableMap.of()));

		MarketData suppliedData = MarketData.empty(date(2011, 3, 8));
		TestObservableId id1 = TestObservableId.of(StandardId.of("reqs", "a"));
		TestObservableId id2 = TestObservableId.of(StandardId.of("reqs", "b"));
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();
		BuiltMarketData marketData = factory.create(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA);
		assertThat(marketData.getValue(id1)).isEqualTo(1d);
		assertThat(marketData.getValue(id2)).isEqualTo(2d);
	  }

	  /// <summary>
	  /// Tests observable market data values supplied by the user are included in the results.
	  /// </summary>
	  public virtual void buildSuppliedObservableValues()
	  {
		MarketDataFactory factory = MarketDataFactory.of(ObservableDataProvider.none(), new TestTimeSeriesProvider(ImmutableMap.of()));

		TestObservableId id1 = TestObservableId.of("a");
		TestObservableId id2 = TestObservableId.of("b");

		MarketData suppliedData = ImmutableMarketData.builder(date(2011, 3, 8)).addValue(id1, 1d).addValue(id2, 2d).build();
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();
		BuiltMarketData marketData = factory.create(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA);
		assertThat(marketData.getValue(id1)).isEqualTo(1d);
		assertThat(marketData.getValue(id2)).isEqualTo(2d);
	  }

	  /// <summary>
	  /// Tests building market data that depends on other market data.
	  /// </summary>
	  public virtual void buildDataFromOtherData()
	  {
		TestMarketDataFunctionB builderB = new TestMarketDataFunctionB(this);
		TestMarketDataFunctionC builderC = new TestMarketDataFunctionC();

		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(new TestIdB("1"), new TestIdB("2")).build();

		LocalDateDoubleTimeSeries timeSeries1 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1).put(date(2011, 3, 9), 2).put(date(2011, 3, 10), 3).build();

		LocalDateDoubleTimeSeries timeSeries2 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 10).put(date(2011, 3, 9), 20).put(date(2011, 3, 10), 30).build();

		IDictionary<TestIdA, LocalDateDoubleTimeSeries> timeSeriesMap = ImmutableMap.of(new TestIdA("1"), timeSeries1, new TestIdA("2"), timeSeries2);

		TimeSeriesProvider timeSeriesProvider = new TestTimeSeriesProvider(timeSeriesMap);

		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), timeSeriesProvider, builderB, builderC);

		MarketData suppliedData = MarketData.empty(date(2011, 3, 8));
		BuiltMarketData marketData = factory.create(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA);

		assertThat(marketData.ValueFailures).Empty;
		assertThat(marketData.TimeSeriesFailures).Empty;

		TestMarketDataB marketDataB1 = marketData.getValue(new TestIdB("1"));
		TestMarketDataB marketDataB2 = marketData.getValue(new TestIdB("2"));

		TestMarketDataB expectedB1 = new TestMarketDataB(1, new TestMarketDataC(timeSeries1));
		TestMarketDataB expectedB2 = new TestMarketDataB(2, new TestMarketDataC(timeSeries2));

		assertThat(marketDataB1).isEqualTo(expectedB1);
		assertThat(marketDataB2).isEqualTo(expectedB2);
	  }

	  /// <summary>
	  /// Tests building market data that depends on other market data that is supplied by the user.
	  /// 
	  /// This tests that supplied data is included in scenario data if it is not in the requirements but it is
	  /// needed to build data that is in the requirements.
	  /// 
	  /// For example, par rates are required to build curves but are not used directly by functions so the
	  /// requirements will not contain par rates IDs. The requirements contain curve IDs and the curve
	  /// building function will declare that it requires par rates.
	  /// </summary>
	  public virtual void buildDataFromSuppliedData()
	  {
		TestMarketDataFunctionB builderB = new TestMarketDataFunctionB(this);
		TestMarketDataFunctionC builderC = new TestMarketDataFunctionC();

		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(new TestIdB("1"), new TestIdB("2")).build();

		LocalDateDoubleTimeSeries timeSeries1 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1).put(date(2011, 3, 9), 2).put(date(2011, 3, 10), 3).build();

		LocalDateDoubleTimeSeries timeSeries2 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 10).put(date(2011, 3, 9), 20).put(date(2011, 3, 10), 30).build();

		TestIdA idA1 = new TestIdA("1");
		TestIdA idA2 = new TestIdA("2");

		MarketData suppliedData = ImmutableMarketData.builder(date(2011, 3, 8)).addTimeSeries(idA1, timeSeries1).addTimeSeries(idA2, timeSeries2).addValue(idA1, 1d).addValue(idA2, 2d).build();

		MarketDataFactory factory = MarketDataFactory.of(ObservableDataProvider.none(), TimeSeriesProvider.none(), builderB, builderC);

		BuiltMarketData marketData = factory.create(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA);

		assertThat(marketData.ValueFailures).Empty;
		assertThat(marketData.TimeSeriesFailures).Empty;

		TestMarketDataB marketDataB1 = marketData.getValue(new TestIdB("1"));
		TestMarketDataB marketDataB2 = marketData.getValue(new TestIdB("2"));

		TestMarketDataB expectedB1 = new TestMarketDataB(1, new TestMarketDataC(timeSeries1));
		TestMarketDataB expectedB2 = new TestMarketDataB(2, new TestMarketDataC(timeSeries2));

		assertThat(marketDataB1).isEqualTo(expectedB1);
		assertThat(marketDataB2).isEqualTo(expectedB2);
	  }

	  /// <summary>
	  /// Tests an exception is thrown when there is no builder for an ID type.
	  /// </summary>
	  public virtual void noMarketDataBuilderAvailable()
	  {
		TestIdB idB1 = new TestIdB("1");
		TestIdB idB2 = new TestIdB("2");
		TestMarketDataFunctionB builder = new TestMarketDataFunctionB(this);

		// Market data B depends on market data C so these requirements should cause instances of C to be built.
		// There is no market data function for building instances of C so this should cause failures.
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(idB1, idB2).build();

		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), new TestTimeSeriesProvider(ImmutableMap.of()), builder);

		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).build();
		assertThrows(() => factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, ScenarioDefinition.empty()), typeof(System.InvalidOperationException), "No market data function available for market data ID of type.*");
	  }

	  /// <summary>
	  /// Tests building a result and keeping the intermediate values.
	  /// </summary>
	  public virtual void buildWithIntermediateValues()
	  {
		TestMarketDataFunctionB builderB = new TestMarketDataFunctionB(this);
		TestMarketDataFunctionC builderC = new TestMarketDataFunctionC();

		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(new TestIdB("1"), new TestIdB("2")).build();

		LocalDateDoubleTimeSeries timeSeries1 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1).put(date(2011, 3, 9), 2).put(date(2011, 3, 10), 3).build();

		LocalDateDoubleTimeSeries timeSeries2 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 10).put(date(2011, 3, 9), 20).put(date(2011, 3, 10), 30).build();

		IDictionary<TestIdA, LocalDateDoubleTimeSeries> timeSeriesMap = ImmutableMap.of(new TestIdA("1"), timeSeries1, new TestIdA("2"), timeSeries2);

		TimeSeriesProvider timeSeriesProvider = new TestTimeSeriesProvider(timeSeriesMap);

		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), timeSeriesProvider, builderB, builderC);

		MarketData suppliedData = MarketData.empty(date(2011, 3, 8));
		BuiltMarketData marketData = factory.create(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA);

		assertThat(marketData.ValueFailures).Empty;
		assertThat(marketData.TimeSeriesFailures).Empty;

		TestMarketDataC expectedC1 = new TestMarketDataC(timeSeries1);
		TestMarketDataC expectedC2 = new TestMarketDataC(timeSeries2);
		TestMarketDataB expectedB1 = new TestMarketDataB(1, expectedC1);
		TestMarketDataB expectedB2 = new TestMarketDataB(2, expectedC2);

		// Check the values in the requirements are present
		assertThat(marketData.getValue(new TestIdB("1"))).isEqualTo(expectedB1);
		assertThat(marketData.getValue(new TestIdB("2"))).isEqualTo(expectedB2);

		// Check the intermediate values are present
		assertThat(marketData.getValue(new TestIdA("1"))).isEqualTo(1d);
		assertThat(marketData.getValue(new TestIdA("2"))).isEqualTo(2d);
		assertThat(marketData.getValue(new TestIdC("1"))).isEqualTo(expectedC1);
		assertThat(marketData.getValue(new TestIdC("2"))).isEqualTo(expectedC2);
	  }

	  /// <summary>
	  /// Tests building multiple observable values for scenarios where the values aren't perturbed.
	  /// </summary>
	  public virtual void buildObservableScenarioValues()
	  {
		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), new TestTimeSeriesProvider(ImmutableMap.of()));

		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).build();
		TestObservableId id1 = TestObservableId.of(StandardId.of("reqs", "a"));
		TestObservableId id2 = TestObservableId.of(StandardId.of("reqs", "b"));
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();
		// This mapping doesn't perturb any data but it causes three scenarios to be built
		PerturbationMapping<double> mapping = PerturbationMapping.of(new FalseFilter<double>(typeof(TestObservableId)), new AbsoluteDoubleShift(1, 2, 3));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);
		assertThat(marketData.getValue(id1)).isEqualTo(MarketDataBox.ofSingleValue(1d));
		assertThat(marketData.getValue(id2)).isEqualTo(MarketDataBox.ofSingleValue(2d));
	  }

	  /// <summary>
	  /// Tests observable values supplied by the user are included in the results when they aren't perturbed
	  /// </summary>
	  public virtual void buildSuppliedObservableScenarioValues()
	  {
		MarketDataFactory factory = MarketDataFactory.of(ObservableDataProvider.none(), new TestTimeSeriesProvider(ImmutableMap.of()));
		TestObservableId id1 = TestObservableId.of(StandardId.of("reqs", "a"));
		TestObservableId id2 = TestObservableId.of(StandardId.of("reqs", "b"));
		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).addValue(id1, 1d).addValue(id2, 2d).build();
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();
		// This mapping doesn't perturb any data but it causes three scenarios to be built
		PerturbationMapping<double> mapping = PerturbationMapping.of(new FalseFilter<double>(typeof(TestObservableId)), new AbsoluteDoubleShift(1, 2, 3));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);

		assertThat(marketData.getValue(id1)).isEqualTo(MarketDataBox.ofSingleValue(1d));
		assertThat(marketData.getValue(id2)).isEqualTo(MarketDataBox.ofSingleValue(2d));
	  }

	  /// <summary>
	  /// Test that time series from the supplied data are copied to the scenario data.
	  /// </summary>
	  public virtual void buildSuppliedTimeSeries()
	  {
		MarketDataFactory factory = MarketDataFactory.of(ObservableDataProvider.none(), new TestTimeSeriesProvider(ImmutableMap.of()));

		TestObservableId id1 = TestObservableId.of(StandardId.of("reqs", "a"));
		TestObservableId id2 = TestObservableId.of(StandardId.of("reqs", "b"));

		LocalDateDoubleTimeSeries timeSeries1 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1).put(date(2011, 3, 9), 2).put(date(2011, 3, 10), 3).build();

		LocalDateDoubleTimeSeries timeSeries2 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 10).put(date(2011, 3, 9), 20).put(date(2011, 3, 10), 30).build();

		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).addTimeSeries(id1, timeSeries1).addTimeSeries(id2, timeSeries2).build();

		MarketDataRequirements requirements = MarketDataRequirements.builder().addTimeSeries(id1, id2).build();
		// This mapping doesn't perturb any data but it causes three scenarios to be built
		PerturbationMapping<double> mapping = PerturbationMapping.of(new FalseFilter<double>(typeof(TestObservableId)), new AbsoluteDoubleShift(1, 2, 3));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);

		assertThat(marketData.getTimeSeries(id1)).isEqualTo(timeSeries1);
		assertThat(marketData.getTimeSeries(id2)).isEqualTo(timeSeries2);
	  }

	  public virtual void perturbObservableValues()
	  {
		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), new TestTimeSeriesProvider(ImmutableMap.of()));

		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).build();
		TestObservableId id1 = TestObservableId.of(StandardId.of("reqs", "a"));
		TestObservableId id2 = TestObservableId.of(StandardId.of("reqs", "b"));
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();
		PerturbationMapping<double> mapping = PerturbationMapping.of(new ExactIdFilter<double>(id1), new AbsoluteDoubleShift(1, 2, 3));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);

		assertThat(marketData.getValue(id1)).isEqualTo(MarketDataBox.ofScenarioValues(2d, 3d, 4d));
		assertThat(marketData.getValue(id2)).isEqualTo(MarketDataBox.ofSingleValue(2d));
	  }

	  /// <summary>
	  /// Tests that observable data is only perturbed once, even if there are two applicable perturbation mappings.
	  /// </summary>
	  public virtual void observableDataOnlyPerturbedOnce()
	  {
		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), new TestTimeSeriesProvider(ImmutableMap.of()));

		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).build();
		TestObservableId id1 = TestObservableId.of(StandardId.of("reqs", "a"));
		TestObservableId id2 = TestObservableId.of(StandardId.of("reqs", "b"));
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();
		PerturbationMapping<double> mapping1 = PerturbationMapping.of(new ExactIdFilter<double>(id2), new RelativeDoubleShift(0.1, 0.2, 0.3));
		PerturbationMapping<double> mapping2 = PerturbationMapping.of(new ExactIdFilter<double>(id2), new AbsoluteDoubleShift(1, 2, 3));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping1, mapping2));
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);

		assertThat(marketData.getValue(id1)).isEqualTo(MarketDataBox.ofSingleValue(1d));
		assertThat(marketData.getValue(id2)).isEqualTo(MarketDataBox.ofScenarioValues(2.2d, 2.4d, 2.6d));
	  }

	  /// <summary>
	  /// Tests building multiple values of non-observable market data for multiple scenarios. The data isn't perturbed.
	  /// </summary>
	  public virtual void buildNonObservableScenarioValues()
	  {
		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), new TestTimeSeriesProvider(ImmutableMap.of()), new NonObservableMarketDataFunction());

		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).build();
		NonObservableId id1 = new NonObservableId("a");
		NonObservableId id2 = new NonObservableId("b");
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();

		// This mapping doesn't perturb any data but it causes three scenarios to be built
		PerturbationMapping<string> mapping = PerturbationMapping.of(new FalseFilter<string>(typeof(NonObservableId)), new StringAppender("", "", ""));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);

		MarketDataBox<string> box1 = marketData.getValue(id1);
		assertThat(box1.getValue(0)).isEqualTo("1.0");
		assertThat(box1.getValue(1)).isEqualTo("1.0");
		assertThat(box1.getValue(2)).isEqualTo("1.0");

		MarketDataBox<string> box2 = marketData.getValue(id2);
		assertThat(box2.getValue(0)).isEqualTo("2.0");
		assertThat(box2.getValue(1)).isEqualTo("2.0");
		assertThat(box2.getValue(2)).isEqualTo("2.0");
	  }

	  /// <summary>
	  /// Tests non-observable values supplied by the user are included in the results when they aren't perturbed
	  /// </summary>
	  public virtual void buildSuppliedNonObservableScenarioValues()
	  {
		MarketDataFactory factory = MarketDataFactory.of(ObservableDataProvider.none(), new TestTimeSeriesProvider(ImmutableMap.of()));
		NonObservableId id1 = new NonObservableId("a");
		NonObservableId id2 = new NonObservableId("b");
		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).addValue(id1, "value1").addValue(id2, "value2").build();
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();
		// This mapping doesn't perturb any data but it causes three scenarios to be built
		PerturbationMapping<string> mapping = PerturbationMapping.of(new FalseFilter<string>(typeof(NonObservableId)), new StringAppender("", "", ""));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);

		assertThat(marketData.getValue(id1)).isEqualTo(MarketDataBox.ofSingleValue("value1"));
		assertThat(marketData.getValue(id2)).isEqualTo(MarketDataBox.ofSingleValue("value2"));
	  }

	  /// <summary>
	  /// Tests building scenario data from values that are supplied by the user but aren't directly required
	  /// by the functions.
	  /// 
	  /// For example, par rates are required to build curves but are not used directly by functions so the
	  /// requirements will not contain par rates IDs. The requirements contain curve IDs and the curve
	  /// building function will declare that it requires par rates.
	  /// </summary>
	  public virtual void buildScenarioValuesFromSuppliedData()
	  {
		TestMarketDataFunctionB builderB = new TestMarketDataFunctionB(this);
		TestMarketDataFunctionC builderC = new TestMarketDataFunctionC();
		TestIdB idB1 = new TestIdB("1");
		TestIdB idB2 = new TestIdB("2");

		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(idB1, idB2).build();

		LocalDateDoubleTimeSeries timeSeries1 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1).put(date(2011, 3, 9), 2).put(date(2011, 3, 10), 3).build();

		LocalDateDoubleTimeSeries timeSeries2 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 10).put(date(2011, 3, 9), 20).put(date(2011, 3, 10), 30).build();

		TestIdA idA1 = new TestIdA("1");
		TestIdA idA2 = new TestIdA("2");

		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).addTimeSeries(idA1, timeSeries1).addTimeSeries(idA2, timeSeries2).addValue(idA1, 1d).addValue(idA2, 2d).build();

		MarketDataFactory marketDataFactory = MarketDataFactory.of(ObservableDataProvider.none(), TimeSeriesProvider.none(), builderB, builderC);

		PerturbationMapping<double> aMapping = PerturbationMapping.of(new ExactIdFilter<double>(new TestIdA("2")), new RelativeDoubleShift(0.2, 0.3, 0.4));

		PerturbationMapping<TestMarketDataC> cMapping = PerturbationMapping.of(new ExactIdFilter<TestMarketDataC>(new TestIdC("1")), new TestCPerturbation(1.1, 1.2, 1.3));

		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(aMapping, cMapping);
		BuiltScenarioMarketData marketData = marketDataFactory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);

		assertThat(marketData.ValueFailures).Empty;
		assertThat(marketData.TimeSeriesFailures).Empty;

		MarketDataBox<TestMarketDataB> marketDataB1 = marketData.getValue(idB1);
		MarketDataBox<TestMarketDataB> marketDataB2 = marketData.getValue(idB2);

		MarketDataBox<TestMarketDataB> expectedB1 = MarketDataBox.ofScenarioValues(new TestMarketDataB(1, new TestMarketDataC(timeSeries1.mapValues(v => v * 1.1))), new TestMarketDataB(1, new TestMarketDataC(timeSeries1.mapValues(v => v * 1.2))), new TestMarketDataB(1, new TestMarketDataC(timeSeries1.mapValues(v => v * 1.3))));

		MarketDataBox<TestMarketDataB> expectedB2 = MarketDataBox.ofScenarioValues(new TestMarketDataB(2.4, new TestMarketDataC(timeSeries2)), new TestMarketDataB(2.6, new TestMarketDataC(timeSeries2)), new TestMarketDataB(2.8, new TestMarketDataC(timeSeries2)));

		assertThat(marketDataB1).isEqualTo(expectedB1);
		assertThat(marketDataB2).isEqualTo(expectedB2);
	  }

	  /// <summary>
	  /// Tests that perturbations are applied to non-observable market data.
	  /// </summary>
	  public virtual void perturbNonObservableValues()
	  {
		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), new TestTimeSeriesProvider(ImmutableMap.of()), new NonObservableMarketDataFunction());
		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).build();

		NonObservableId id1 = new NonObservableId("a");
		NonObservableId id2 = new NonObservableId("b");
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();

		PerturbationMapping<string> mapping = PerturbationMapping.of(new ExactIdFilter<string>(id1), new StringAppender("foo", "bar", "baz"));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);

		assertThat(marketData.getValue(id1)).isEqualTo(MarketDataBox.ofScenarioValues("1.0foo", "1.0bar", "1.0baz"));
		assertThat(marketData.getValue(id2)).isEqualTo(MarketDataBox.ofSingleValue("2.0"));
	  }

	  /// <summary>
	  /// Tests that non-observable data is only perturbed once, even if there are two applicable perturbation mappings.
	  /// </summary>
	  public virtual void nonObservableDataOnlyPerturbedOnce()
	  {
		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), new TestTimeSeriesProvider(ImmutableMap.of()), new NonObservableMarketDataFunction());
		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).build();

		NonObservableId id1 = new NonObservableId("a");
		NonObservableId id2 = new NonObservableId("b");
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();

		PerturbationMapping<string> mapping1 = PerturbationMapping.of(new ExactIdFilter<string>(id1), new StringAppender("FOO", "BAR", "BAZ"));
		PerturbationMapping<string> mapping2 = PerturbationMapping.of(new ExactIdFilter<string>(id1), new StringAppender("foo", "bar", "baz"));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping1, mapping2));
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);

		assertThat(marketData.getValue(id1)).isEqualTo(MarketDataBox.ofScenarioValues("1.0FOO", "1.0BAR", "1.0BAZ"));
		assertThat(marketData.getValue(id2)).isEqualTo(MarketDataBox.ofSingleValue("2.0"));
	  }

	  /// <summary>
	  /// Tests that observable data built from observable values see the effects of the perturbations.
	  /// </summary>
	  public virtual void nonObservableDataBuiltFromPerturbedObservableData()
	  {
		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), new TestTimeSeriesProvider(ImmutableMap.of()), new NonObservableMarketDataFunction());
		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).build();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> id1 = new NonObservableId("a");
		MarketDataId<object> id1 = new NonObservableId("a");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> id2 = new NonObservableId("b");
		MarketDataId<object> id2 = new NonObservableId("b");
		TestObservableId quoteId = TestObservableId.of(StandardId.of("reqs", "b"));
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();

		PerturbationMapping<double> mapping = PerturbationMapping.of(new ExactIdFilter<double>(quoteId), new RelativeDoubleShift(0.1, 0.2, 0.3));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);

		assertThat(marketData.getValue(id1)).isEqualTo(MarketDataBox.ofSingleValue("1.0"));
		assertThat(marketData.getValue(id2)).isEqualTo(MarketDataBox.ofScenarioValues("2.2", "2.4", "2.6"));
	  }

	  /// <summary>
	  /// Tests that an exception is thrown when building observable market data for scenarios where there is no
	  /// market data function.
	  /// </summary>
	  public virtual void nonObservableScenarioDataWithMissingBuilder()
	  {
		MarketDataFactory factory = MarketDataFactory.of(new TestObservableDataProvider(), new TestTimeSeriesProvider(ImmutableMap.of()));
		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).build();

		NonObservableId id1 = new NonObservableId("a");
		NonObservableId id2 = new NonObservableId("b");
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id1, id2).build();

		// This mapping doesn't perturb any data but it causes three scenarios to be built
		PerturbationMapping<string> mapping = PerturbationMapping.of(new FalseFilter<string>(typeof(NonObservableId)), new StringAppender("", "", ""));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));

		assertThrows(() => factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition), typeof(System.InvalidOperationException), "No market data function available for market data ID of type.*");

	  }

	  /// <summary>
	  /// Tests that perturbations are applied to observable data supplied by the user.
	  /// </summary>
	  public virtual void perturbSuppliedNonObservableData()
	  {
		MarketDataFactory factory = MarketDataFactory.of(ObservableDataProvider.none(), new TestTimeSeriesProvider(ImmutableMap.of()));
		NonObservableId id = new NonObservableId("a");
		PerturbationMapping<string> mapping = PerturbationMapping.of(new ExactIdFilter<string>(id), new StringAppender("Foo", "Bar", "Baz"));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));
		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).addValue(id, "value").build();
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id).build();
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);
		MarketDataBox<string> values = marketData.getValue(id);
		MarketDataBox<string> expectedValues = MarketDataBox.ofScenarioValues("valueFoo", "valueBar", "valueBaz");
		assertThat(values).isEqualTo(expectedValues);
	  }

	  /// <summary>
	  /// Tests that perturbations are applied to non-observable data supplied by the user.
	  /// </summary>
	  public virtual void perturbSuppliedObservableData()
	  {
		MarketDataFactory factory = MarketDataFactory.of(ObservableDataProvider.none(), new TestTimeSeriesProvider(ImmutableMap.of()));
		TestObservableId id = TestObservableId.of(StandardId.of("reqs", "a"));
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(id).build();
		PerturbationMapping<double> mapping = PerturbationMapping.of(new ExactIdFilter<double>(id), new RelativeDoubleShift(0.1, 0.2, 0.3));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(ImmutableList.of(mapping));
		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).addValue(id, 2d).build();
		BuiltScenarioMarketData marketData = factory.createMultiScenario(requirements, MARKET_DATA_CONFIG, suppliedData, REF_DATA, scenarioDefinition);
		MarketDataBox<double> values = marketData.getValue(id);
		MarketDataBox<double> expectedValues = MarketDataBox.ofScenarioValues(2.2, 2.4, 2.6);
		assertThat(values).isEqualTo(expectedValues);
	  }

	  /// <summary>
	  /// Tests ObservableDataProvider.none(), which is never normally be invoked.
	  /// </summary>
	  public virtual void coverage_ObservableDataProvider_none()
	  {
		TestObservableId id = TestObservableId.of(StandardId.of("reqs", "a"));
		ObservableDataProvider test = ObservableDataProvider.none();
		IDictionary<ObservableId, Result<double>> result = test.provideObservableData(ImmutableSet.of(id));
		assertThat(result).containsOnlyKeys(id);
		assertThat(result[id].Failure).True;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Simple time series provider backed by a map.
	  /// </summary>
	  private sealed class TestTimeSeriesProvider : TimeSeriesProvider
	  {

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<? extends com.opengamma.strata.data.ObservableId, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries> timeSeries;
		internal readonly IDictionary<ObservableId, LocalDateDoubleTimeSeries> timeSeries;

		internal TestTimeSeriesProvider<T1>(IDictionary<T1> timeSeries) where T1 : com.opengamma.strata.data.ObservableId
		{
		  this.timeSeries = timeSeries;
		}

		public Result<LocalDateDoubleTimeSeries> provideTimeSeries(ObservableId id)
		{
		  LocalDateDoubleTimeSeries series = timeSeries[id];
		  return Result.ofNullable(series, FailureReason.MISSING_DATA, "No time series found for ID {}", id);
		}
	  }

	  /// <summary>
	  /// Builds observable data by parsing the value of the standard ID.
	  /// </summary>
	  private sealed class TestObservableDataProvider : ObservableDataProvider
	  {

		// demonstrates provider that maps identifiers
		internal readonly IDictionary<ObservableId, ObservableId> idMap = ImmutableMap.of(TestObservableId.of(StandardId.of("reqs", "a")), TestObservableId.of(StandardId.of("vendor", "1")), TestObservableId.of(StandardId.of("reqs", "b")), TestObservableId.of(StandardId.of("vendor", "2")));

		public IDictionary<ObservableId, Result<double>> provideObservableData<T1>(ISet<T1> requirements) where T1 : com.opengamma.strata.data.ObservableId
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  return requirements.collect(toImmutableMap(id => id, id => buildResult(idMap.getOrDefault(id, id))));
		}

		internal Result<double> buildResult(ObservableId id)
		{
		  return Result.success(double.Parse(id.StandardId.Value));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test ID A.
	  /// </summary>
	  private sealed class TestIdA : ObservableId
	  {

		internal readonly StandardId id;

		internal TestIdA(string id)
		{
		  this.id = StandardId.of("test", id);
		}

		public StandardId StandardId
		{
			get
			{
			  return id;
			}
		}

		public FieldName FieldName
		{
			get
			{
			  return FieldName.MARKET_VALUE;
			}
		}

		public ObservableSource ObservableSource
		{
			get
			{
			  return ObservableSource.NONE;
			}
		}

		public ObservableId withObservableSource(ObservableSource obsSource)
		{
		  return this;
		}

		public override bool Equals(object o)
		{
		  if (this == o)
		  {
			return true;
		  }
		  if (o == null || this.GetType() != o.GetType())
		  {
			return false;
		  }
		  TestIdA id = (TestIdA) o;
		  return Objects.Equals(this.id, id.id);
		}

		public override int GetHashCode()
		{
		  return Objects.hash(id);
		}

		public override string ToString()
		{
		  return "TestIdA [id=" + id + "]";
		}
	  }

	  /// <summary>
	  /// Test ID B.
	  /// </summary>
	  private sealed class TestIdB : MarketDataId<TestMarketDataB>
	  {

		internal readonly string str;

		internal TestIdB(string str)
		{
		  this.str = str;
		}

		public Type<TestMarketDataB> MarketDataType
		{
			get
			{
			  return typeof(TestMarketDataB);
			}
		}

		public override bool Equals(object o)
		{
		  if (this == o)
		  {
			return true;
		  }
		  if (o == null || this.GetType() != o.GetType())
		  {
			return false;
		  }
		  TestIdB id = (TestIdB) o;
		  return Objects.Equals(str, id.str);
		}

		public override int GetHashCode()
		{
		  return Objects.hash(str);
		}

		public override string ToString()
		{
		  return "TestIdB [str='" + str + "']";
		}
	  }

	  private sealed class TestIdC : MarketDataId<TestMarketDataC>
	  {

		internal readonly string str;

		internal TestIdC(string str)
		{
		  this.str = str;
		}

		public Type<TestMarketDataC> MarketDataType
		{
			get
			{
			  return typeof(TestMarketDataC);
			}
		}

		public override bool Equals(object o)
		{
		  if (this == o)
		  {
			return true;
		  }
		  if (o == null || this.GetType() != o.GetType())
		  {
			return false;
		  }
		  TestIdC id = (TestIdC) o;
		  return Objects.Equals(str, id.str);
		}

		public override int GetHashCode()
		{
		  return Objects.hash(str);
		}

		public override string ToString()
		{
		  return "TestIdC [str='" + str + "']";
		}
	  }

	  /// <summary>
	  /// Test market data B.
	  /// </summary>
	  private sealed class TestMarketDataB
	  {

		internal readonly double value;

		internal readonly TestMarketDataC marketData;

		internal TestMarketDataB(double value, TestMarketDataC marketData)
		{
		  this.value = value;
		  this.marketData = marketData;
		}

		public override bool Equals(object o)
		{
		  if (this == o)
		  {
			return true;
		  }
		  if (o == null || this.GetType() != o.GetType())
		  {
			return false;
		  }
		  TestMarketDataB that = (TestMarketDataB) o;
		  return Objects.Equals(value, that.value) && Objects.Equals(marketData, that.marketData);
		}

		public override int GetHashCode()
		{
		  return Objects.hash(value, marketData);
		}
	  }

	  /// <summary>
	  /// Function for building TestMarketDataB.
	  /// Requires a value with ID TestIdA(id.str) and TestMarketDataC with ID TestIdC(id.str)
	  /// </summary>
	  private sealed class TestMarketDataFunctionB : MarketDataFunction<TestMarketDataB, TestIdB>
	  {
		  private readonly DefaultMarketDataFactoryTest outerInstance;

		  public TestMarketDataFunctionB(DefaultMarketDataFactoryTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


		public MarketDataRequirements requirements(TestIdB id, MarketDataConfig marketDataConfig)
		{
		  return MarketDataRequirements.builder().addValues(new TestIdA(id.str), new TestIdC(id.str)).build();
		}

		public MarketDataBox<TestMarketDataB> build(TestIdB id, MarketDataConfig marketDataConfig, ScenarioMarketData marketData, ReferenceData refData)
		{

		  TestIdA idA = new TestIdA(id.str);
		  TestIdC idC = new TestIdC(id.str);
		  MarketDataBox<double> valueA = marketData.getValue(idA);
		  MarketDataBox<TestMarketDataC> marketDataC = marketData.getValue(idC);
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		  return valueA.combineWith(marketDataC, TestMarketDataB::new);
		}

		public Type<TestIdB> MarketDataIdType
		{
			get
			{
			  return typeof(TestIdB);
			}
		}
	  }

	  /// <summary>
	  /// Test market data C.
	  /// </summary>
	  private sealed class TestMarketDataC
	  {

		internal readonly LocalDateDoubleTimeSeries timeSeries;

		internal TestMarketDataC(LocalDateDoubleTimeSeries timeSeries)
		{
		  this.timeSeries = timeSeries;
		}

		public override bool Equals(object o)
		{
		  if (this == o)
		  {
			return true;
		  }
		  if (o == null || this.GetType() != o.GetType())
		  {
			return false;
		  }
		  TestMarketDataC that = (TestMarketDataC) o;
		  return Objects.Equals(timeSeries, that.timeSeries);
		}

		public override int GetHashCode()
		{
		  return Objects.hash(timeSeries);
		}
	  }

	  /// <summary>
	  /// Function for building TestMarketDataC.
	  /// Requires a time series with ID TestIdA(id.str)
	  /// </summary>
	  private sealed class TestMarketDataFunctionC : MarketDataFunction<TestMarketDataC, TestIdC>
	  {

		public MarketDataRequirements requirements(TestIdC id, MarketDataConfig marketDataConfig)
		{
		  return MarketDataRequirements.builder().addTimeSeries(new TestIdA(id.str)).build();
		}

		public MarketDataBox<TestMarketDataC> build(TestIdC id, MarketDataConfig marketDataConfig, ScenarioMarketData marketData, ReferenceData refData)
		{

		  LocalDateDoubleTimeSeries timeSeries = marketData.getTimeSeries(new TestIdA(id.str));
		  return MarketDataBox.ofSingleValue(new TestMarketDataC(timeSeries));
		}

		public Type<TestIdC> MarketDataIdType
		{
			get
			{
			  return typeof(TestIdC);
			}
		}
	  }

	  /// <summary>
	  /// Market data filter that doesn't match any market data.
	  /// </summary>
	  private sealed class FalseFilter<T, I> : MarketDataFilter<T, I> where I : com.opengamma.strata.data.MarketDataId<T>
	  {

		internal readonly Type idType;

		internal FalseFilter(Type idType)
		{
		  this.idType = idType;
		}

		public bool matches(I marketDataId, MarketDataBox<T> marketData, ReferenceData refData)
		{
		  return false;
		}

		public Type MarketDataIdType
		{
			get
			{
			  return idType;
			}
		}
	  }

	  /// <summary>
	  /// Perturbation that applies a shift to a double value.
	  /// </summary>
	  private sealed class AbsoluteDoubleShift : ScenarioPerturbation<double>
	  {

		internal readonly double[] shiftAmount;

		internal AbsoluteDoubleShift(params double[] shiftAmount)
		{
		  this.shiftAmount = shiftAmount;
		}

		public MarketDataBox<double> applyTo(MarketDataBox<double> marketData, ReferenceData refData)
		{
		  return marketData.mapWithIndex(ScenarioCount, (value, scenarioIndex) => value + shiftAmount[scenarioIndex]);
		}

		public int ScenarioCount
		{
			get
			{
			  return shiftAmount.Length;
			}
		}

		public Type<double> MarketDataType
		{
			get
			{
			  return typeof(Double);
			}
		}
	  }

	  /// <summary>
	  /// Perturbation that applies a shift to a double value.
	  /// </summary>
	  private sealed class RelativeDoubleShift : ScenarioPerturbation<double>
	  {

		internal readonly double[] shiftAmounts;

		internal RelativeDoubleShift(params double[] shiftAmounts)
		{
		  this.shiftAmounts = shiftAmounts;
		}

		public MarketDataBox<double> applyTo(MarketDataBox<double> marketData, ReferenceData refData)
		{
		  return marketData.mapWithIndex(ScenarioCount, (value, scenarioIndex) => value * (1 + shiftAmounts[scenarioIndex]));
		}

		public int ScenarioCount
		{
			get
			{
			  return shiftAmounts.Length;
			}
		}

		public Type<double> MarketDataType
		{
			get
			{
			  return typeof(Double);
			}
		}
	  }

	  /// <summary>
	  /// Market data filter that matches an ID exactly.
	  /// </summary>
	  private sealed class ExactIdFilter<T, I> : MarketDataFilter<T, I> where I : com.opengamma.strata.data.MarketDataId<T>
	  {

		internal readonly I id;

		internal ExactIdFilter(I id)
		{
		  this.id = id;
		}

		public bool matches(I marketDataId, MarketDataBox<T> marketData, ReferenceData refData)
		{
		  return id.Equals(marketDataId);
		}

		public Type MarketDataIdType
		{
			get
			{
			  return id.GetType();
			}
		}
	  }

	  /// <summary>
	  /// Market data ID for a piece of non-observable market data that is a string.
	  /// </summary>
	  private sealed class NonObservableId : MarketDataId<string>
	  {

		internal readonly string str;

		internal NonObservableId(string str)
		{
		  this.str = str;
		}

		public Type<string> MarketDataType
		{
			get
			{
			  return typeof(string);
			}
		}

		public override string ToString()
		{
		  return "NonObservableId [str='" + str + "']";
		}
	  }

	  /// <summary>
	  /// Market data function that builds a piece of non-observable market data (a string).
	  /// </summary>
	  private sealed class NonObservableMarketDataFunction : MarketDataFunction<string, NonObservableId>
	  {

		public MarketDataRequirements requirements(NonObservableId id, MarketDataConfig marketDataConfig)
		{
		  return MarketDataRequirements.builder().addValues(TestObservableId.of(StandardId.of("reqs", id.str))).build();
		}

		public MarketDataBox<string> build(NonObservableId id, MarketDataConfig marketDataConfig, ScenarioMarketData marketData, ReferenceData refData)
		{

		  MarketDataBox<double> value = marketData.getValue(TestObservableId.of(StandardId.of("reqs", id.str)));
		  return value.map(v => Convert.ToString(v));
		}

		public Type<NonObservableId> MarketDataIdType
		{
			get
			{
			  return typeof(NonObservableId);
			}
		}
	  }

	  /// <summary>
	  /// A perturbation which perturbs a string by appending another string to it.
	  /// </summary>
	  private sealed class StringAppender : ScenarioPerturbation<string>
	  {

		internal readonly string[] str;

		public StringAppender(params string[] str)
		{
		  this.str = str;
		}

		public MarketDataBox<string> applyTo(MarketDataBox<string> marketData, ReferenceData refData)
		{
		  return marketData.mapWithIndex(ScenarioCount, (value, scenarioIndex) => value + str[scenarioIndex]);
		}

		public int ScenarioCount
		{
			get
			{
			  return str.Length;
			}
		}

		public Type<string> MarketDataType
		{
			get
			{
			  return typeof(string);
			}
		}
	  }

	  /// <summary>
	  /// Perturbation that perturbs TestMarketDataC by scaling its time series.
	  /// </summary>
	  private sealed class TestCPerturbation : ScenarioPerturbation<TestMarketDataC>
	  {

		internal readonly double[] scaleFactors;

		internal TestCPerturbation(params double[] scaleFactors)
		{
		  this.scaleFactors = scaleFactors;
		}

		public MarketDataBox<TestMarketDataC> applyTo(MarketDataBox<TestMarketDataC> marketData, ReferenceData refData)
		{
		  return marketData.mapWithIndex(ScenarioCount, this.perturb);
		}

		internal TestMarketDataC perturb(TestMarketDataC data, int scenarioIndex)
		{
		  LocalDateDoubleTimeSeries perturbedTimeSeries = data.timeSeries.mapValues(v => v * scaleFactors[scenarioIndex]);
		  return new TestMarketDataC(perturbedTimeSeries);
		}

		public int ScenarioCount
		{
			get
			{
			  return scaleFactors.Length;
			}
		}

		public Type<TestMarketDataC> MarketDataType
		{
			get
			{
			  return typeof(TestMarketDataC);
			}
		}
	  }
	}

}