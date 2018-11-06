/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataFactory = com.opengamma.strata.calc.marketdata.MarketDataFactory;
	using MarketDataFilter = com.opengamma.strata.calc.marketdata.MarketDataFilter;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using ObservableDataProvider = com.opengamma.strata.calc.marketdata.ObservableDataProvider;
	using PerturbationMapping = com.opengamma.strata.calc.marketdata.PerturbationMapping;
	using ScenarioDefinition = com.opengamma.strata.calc.marketdata.ScenarioDefinition;
	using TimeSeriesProvider = com.opengamma.strata.calc.marketdata.TimeSeriesProvider;
	using TestHelper = com.opengamma.strata.collect.TestHelper;
	using ImmutableScenarioMarketData = com.opengamma.strata.data.scenario.ImmutableScenarioMarketData;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveParallelShifts = com.opengamma.strata.market.curve.CurveParallelShifts;

	/// <summary>
	/// Test usage of <seealso cref="CurveParallelShifts"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveParallelShiftsUsageTest
	public class CurveParallelShiftsUsageTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  public virtual void absoluteScenarios()
	  {
		CurveName curveName = CurveName.of("curveName");
		CurveGroupName curveGroupName = CurveGroupName.of("curveGroupName");
		Curve curve = ConstantCurve.of(curveName, 2);
		PerturbationMapping<Curve> mapping = PerturbationMapping.of(MarketDataFilter.ofName(curveName), CurveParallelShifts.absolute(0.1, 0.2, 0.3));
		CurveId curveId = CurveId.of(curveGroupName, curveName);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(TestHelper.date(2011, 3, 8)).addValue(curveId, curve).build();
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(mapping);
		MarketDataFactory marketDataFactory = MarketDataFactory.of(mock(typeof(ObservableDataProvider)), mock(typeof(TimeSeriesProvider)));
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(curveId).build();
		ScenarioMarketData scenarioData = marketDataFactory.createMultiScenario(requirements, MarketDataConfig.empty(), marketData, REF_DATA, scenarioDefinition);
		MarketDataBox<Curve> curves = scenarioData.getValue(curveId);
		assertThat(curves.ScenarioCount).isEqualTo(3);
		checkCurveValues(curves.getValue(0), 2.1);
		checkCurveValues(curves.getValue(1), 2.2);
		checkCurveValues(curves.getValue(2), 2.3);
	  }

	  // It's not possible to do an equality test on the curves because shifting them wraps them in a different type
	  private void checkCurveValues(Curve curve, double expectedValue)
	  {
		for (int i = 0; i < 10; i++)
		{
		  assertThat(curve.yValue((double) i)).isEqualTo(expectedValue);
		}
	  }

	}

}