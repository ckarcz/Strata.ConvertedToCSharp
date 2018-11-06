/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ImmutableScenarioMarketData = com.opengamma.strata.data.scenario.ImmutableScenarioMarketData;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveGroupId = com.opengamma.strata.market.curve.RatesCurveGroupId;

	/// <summary>
	/// Test <seealso cref="CurveMarketDataFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveMarketDataFunctionTest
	public class CurveMarketDataFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2011, 3, 8);
	  private static readonly CurveGroupName GROUP_NAME = CurveGroupName.of("Group");
	  private static readonly CurveGroupName GROUP_NAME2 = CurveGroupName.of("Group2");
	  private static readonly CurveName CURVE_NAME1 = CurveName.of("Name1");
	  private static readonly CurveName CURVE_NAME2 = CurveName.of("Name2");
	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");

	  //-------------------------------------------------------------------------
	  public virtual void test_singleCurve()
	  {
		Curve curve = ConstantCurve.of(CURVE_NAME1, (double) 1);
		CurveId curveId1 = CurveId.of(GROUP_NAME, CURVE_NAME1, OBS_SOURCE);
		CurveId curveId2 = CurveId.of(GROUP_NAME, CURVE_NAME2, OBS_SOURCE);
		CurveId curveId3 = CurveId.of(GROUP_NAME2, CURVE_NAME1, OBS_SOURCE);
		RatesCurveGroupId groupId = RatesCurveGroupId.of(GROUP_NAME, OBS_SOURCE);
		RatesCurveGroup curveGroup = RatesCurveGroup.of(GROUP_NAME, ImmutableMap.of(Currency.AUD, curve), ImmutableMap.of());
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(VAL_DATE).addValue(groupId, curveGroup).build();
		MarketDataConfig config = MarketDataConfig.builder().add(GROUP_NAME, RatesCurveGroupDefinition.builder().name(GROUP_NAME).build()).build();

		CurveMarketDataFunction test = new CurveMarketDataFunction();
		MarketDataRequirements reqs = test.requirements(curveId1, config);
		assertEquals(reqs.NonObservables, ImmutableSet.of(groupId));
		MarketDataBox<Curve> result = test.build(curveId1, config, marketData, REF_DATA);
		assertEquals(result, MarketDataBox.ofSingleValue(curve));
		assertThrowsIllegalArg(() => test.build(curveId2, config, marketData, REF_DATA));
		assertThrowsIllegalArg(() => test.build(curveId3, config, marketData, REF_DATA));
	  }

	  public virtual void test_multipleCurves()
	  {
		Curve curve1 = ConstantCurve.of(CURVE_NAME1, (double) 1);
		Curve curve2 = ConstantCurve.of(CURVE_NAME2, (double) 2);
		CurveId curveId1 = CurveId.of(GROUP_NAME, CURVE_NAME1);
		CurveId curveId2 = CurveId.of(GROUP_NAME, CURVE_NAME2);
		RatesCurveGroupId groupId = RatesCurveGroupId.of(GROUP_NAME);
		RatesCurveGroup curveGroup = RatesCurveGroup.of(GROUP_NAME, ImmutableMap.of(Currency.AUD, curve1, Currency.GBP, curve2), ImmutableMap.of());
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(VAL_DATE).addValue(groupId, curveGroup).build();
		MarketDataConfig config = MarketDataConfig.builder().add(GROUP_NAME, RatesCurveGroupDefinition.builder().name(GROUP_NAME).build()).build();

		CurveMarketDataFunction test = new CurveMarketDataFunction();
		MarketDataBox<Curve> result1 = test.build(curveId1, config, marketData, REF_DATA);
		assertEquals(result1, MarketDataBox.ofSingleValue(curve1));
		MarketDataBox<Curve> result2 = test.build(curveId2, config, marketData, REF_DATA);
		assertEquals(result2, MarketDataBox.ofSingleValue(curve2));
	  }

	}

}