using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.PCHIP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.measure.Measures.PRESENT_VALUE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.SHORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using OvernightIndices = com.opengamma.strata.basics.index.OvernightIndices;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationRunner = com.opengamma.strata.calc.CalculationRunner;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataFilter = com.opengamma.strata.calc.marketdata.MarketDataFilter;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using PerturbationMapping = com.opengamma.strata.calc.marketdata.PerturbationMapping;
	using ScenarioDefinition = com.opengamma.strata.calc.marketdata.ScenarioDefinition;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using ImmutableScenarioMarketData = com.opengamma.strata.data.scenario.ImmutableScenarioMarketData;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ScenarioPerturbation = com.opengamma.strata.data.scenario.ScenarioPerturbation;
	using GenericDoubleShifts = com.opengamma.strata.market.GenericDoubleShifts;
	using ShiftType = com.opengamma.strata.market.ShiftType;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveGroupEntry = com.opengamma.strata.market.curve.RatesCurveGroupEntry;
	using RatesCurveGroupId = com.opengamma.strata.market.curve.RatesCurveGroupId;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using InterpolatedNodalCurveDefinition = com.opengamma.strata.market.curve.InterpolatedNodalCurveDefinition;
	using FixedOvernightSwapCurveNode = com.opengamma.strata.market.curve.node.FixedOvernightSwapCurveNode;
	using FxSwapCurveNode = com.opengamma.strata.market.curve.node.FxSwapCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DeltaStrike = com.opengamma.strata.market.option.DeltaStrike;
	using SimpleStrike = com.opengamma.strata.market.option.SimpleStrike;
	using Strike = com.opengamma.strata.market.option.Strike;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using PointShifts = com.opengamma.strata.market.param.PointShifts;
	using PointShiftsBuilder = com.opengamma.strata.market.param.PointShiftsBuilder;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using RatesCurveCalibrator = com.opengamma.strata.pricer.curve.RatesCurveCalibrator;
	using BlackFxOptionSmileVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionSmileVolatilities;
	using BlackFxOptionSurfaceVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionSurfaceVolatilities;
	using BlackFxVanillaOptionTradePricer = com.opengamma.strata.pricer.fxopt.BlackFxVanillaOptionTradePricer;
	using FxOptionVolatilitiesId = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesId;
	using FxOptionVolatilitiesName = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName;
	using InterpolatedStrikeSmileDeltaTermStructure = com.opengamma.strata.pricer.fxopt.InterpolatedStrikeSmileDeltaTermStructure;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;
	using FxSwapConventions = com.opengamma.strata.product.fx.type.FxSwapConventions;
	using FxSwapTemplate = com.opengamma.strata.product.fx.type.FxSwapTemplate;
	using FxVanillaOption = com.opengamma.strata.product.fxopt.FxVanillaOption;
	using FxVanillaOptionTrade = com.opengamma.strata.product.fxopt.FxVanillaOptionTrade;
	using FixedOvernightSwapConventions = com.opengamma.strata.product.swap.type.FixedOvernightSwapConventions;
	using FixedOvernightSwapTemplate = com.opengamma.strata.product.swap.type.FixedOvernightSwapTemplate;

	/// <summary>
	/// Test {@code FxOptionVolatilitiesMarketDataFunction}.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxOptionVolatilitiesMarketDataFunctionTest
	public class FxOptionVolatilitiesMarketDataFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2017, 2, 15);
	  private static readonly LocalTime VALUATION_TIME = LocalTime.NOON;
	  private static readonly LocalTime VALUATION_TIME_1 = LocalTime.MIDNIGHT;
	  private static readonly ZoneId ZONE = ZoneId.of("Europe/London");
	  private static readonly CurrencyPair GBP_USD = CurrencyPair.of(GBP, USD);
	  private static readonly HolidayCalendarId NY_LO = USNY.combinedWith(GBLO);
	  private static readonly DaysAdjustment SPOT_OFFSET = DaysAdjustment.NONE;
	  private static readonly BusinessDayAdjustment BDA = BusinessDayAdjustment.of(FOLLOWING, NY_LO);

	  private static readonly IList<Tenor> VOL_TENORS = ImmutableList.of(Tenor.TENOR_3M, Tenor.TENOR_6M);
	  private static readonly IList<Strike> STRIKES = ImmutableList.of(DeltaStrike.of(0.5), DeltaStrike.of(0.1), DeltaStrike.of(0.1), DeltaStrike.of(0.25), DeltaStrike.of(0.25));
	  private static readonly IList<ValueType> VALUE_TYPES = ImmutableList.of(ValueType.BLACK_VOLATILITY, ValueType.RISK_REVERSAL, ValueType.STRANGLE, ValueType.RISK_REVERSAL, ValueType.STRANGLE);
	  private static readonly double[][] VOL_QUOTES = new double[][]
	  {
		  new double[] {0.18, -0.011, 0.031, -0.006, 0.011},
		  new double[] {0.14, -0.012, 0.032, -0.007, 0.012}
	  };
	  private static readonly double[][] VOL_QUOTES_1 = new double[][]
	  {
		  new double[] {0.24, -0.021, 0.043, -0.013, 0.021},
		  new double[] {0.11, -0.022, 0.042, -0.017, 0.022}
	  };
	  private static readonly IList<double> USD_QUOTES = ImmutableList.of(1.1E-5, 1.1E-5, 1.12E-5, 1.39E-5, 2.1E-5, 3.0E-5, 5.0E-5, 7.02E-5, 1.01E-4, 1.22E-4, 1.41E-4, 1.85E-4);
	  private static readonly IList<double> USD_QUOTES_1 = ImmutableList.of(1.2E-5, 1.2E-5, 1.22E-5, 1.49E-5, 2.2E-5, 3.1E-5, 5.1E-5, 7.12E-5, 1.11E-4, 1.32E-4, 1.51E-4, 1.95E-4);
	  private static readonly IList<double> GBP_QUOTES = ImmutableList.of(-3.53E-4, -7.02E-4, -0.00101, -0.00204, -0.0026, -0.00252, -8.0E-4);
	  private static readonly IList<double> GBP_QUOTES_1 = ImmutableList.of(-2.53E-4, -6.02E-4, -0.00001, -0.00104, -0.0016, -0.00152, -7.0E-4);
	  private static readonly IList<Tenor> USD_TENORS = ImmutableList.of(Tenor.TENOR_1M, Tenor.TENOR_2M, Tenor.TENOR_3M, Tenor.TENOR_6M, Tenor.TENOR_9M, Tenor.TENOR_1Y, Tenor.TENOR_18M, Tenor.TENOR_2Y, Tenor.TENOR_3Y, Tenor.TENOR_4Y, Tenor.TENOR_5Y, Tenor.TENOR_10Y);
	  private static readonly IList<Period> GBP_PERIODS = ImmutableList.of(Period.ofMonths(1), Period.ofMonths(2), Period.ofMonths(3), Period.ofMonths(6), Period.ofMonths(9), Period.ofYears(1), Period.ofMonths(18));
	  private static readonly FxRate FX = FxRate.of(GBP_USD, 1.53);
	  private static readonly FxRate FX_1 = FxRate.of(GBP_USD, 1.43);
	  private static readonly ImmutableMap<FxRateId, FxRate> MARKET_FX_QUOTES = ImmutableMap.of(FxRateId.of(GBP_USD), FX);
	  private static readonly ImmutableMap<FxRateId, MarketDataBox<FxRate>> SCENARIO_MARKET_FX_QUOTES = ImmutableMap.of(FxRateId.of(GBP_USD), MarketDataBox.ofScenarioValues(FX, FX_1));

	  private static readonly ImmutableList<FxOptionVolatilitiesNode> VOL_NODES;
	  private static readonly ImmutableList<FxSwapCurveNode> GBP_NODES;
	  private static readonly ImmutableList<FixedOvernightSwapCurveNode> USD_NODES;
	  private static readonly ImmutableMap<QuoteId, double> MARKET_QUOTES;
	  private static readonly ImmutableMap<QuoteId, MarketDataBox<double>> SCENARIO_MARKET_QUOTES;
	  static FxOptionVolatilitiesMarketDataFunctionTest()
	  {
		ImmutableList.Builder<FxOptionVolatilitiesNode> volNodeBuilder = ImmutableList.builder();
		ImmutableMap.Builder<QuoteId, double> marketQuoteBuilder = ImmutableMap.builder();
		ImmutableMap.Builder<QuoteId, MarketDataBox<double>> scenarioMarketQuoteBuilder = ImmutableMap.builder();
		ImmutableList.Builder<FixedOvernightSwapCurveNode> usdNodeBuilder = ImmutableList.builder();
		ImmutableList.Builder<FxSwapCurveNode> gbpNodeBuilder = ImmutableList.builder();
		for (int i = 0; i < VOL_TENORS.Count; ++i)
		{
		  for (int j = 0; j < STRIKES.Count; ++j)
		  {
			QuoteId quoteId = QuoteId.of(StandardId.of("OG", VOL_TENORS[i].ToString() + "_" + STRIKES[j].Label + "_" + VALUE_TYPES[j].ToString()));
			volNodeBuilder.add(FxOptionVolatilitiesNode.of(GBP_USD, SPOT_OFFSET, BDA, VALUE_TYPES[j], quoteId, VOL_TENORS[i], STRIKES[j]));
			marketQuoteBuilder.put(quoteId, VOL_QUOTES[i][j]);
			scenarioMarketQuoteBuilder.put(quoteId, MarketDataBox.ofScenarioValues(VOL_QUOTES[i][j], VOL_QUOTES_1[i][j]));
		  }
		}
		for (int i = 0; i < USD_QUOTES.Count; ++i)
		{
		  QuoteId quoteId = QuoteId.of(StandardId.of("OG", USD.ToString() + "-OIS-" + USD_TENORS[i].ToString()));
		  usdNodeBuilder.add(FixedOvernightSwapCurveNode.of(FixedOvernightSwapTemplate.of(USD_TENORS[i], FixedOvernightSwapConventions.USD_FIXED_TERM_FED_FUND_OIS), quoteId));
		  marketQuoteBuilder.put(quoteId, USD_QUOTES[i]);
		  scenarioMarketQuoteBuilder.put(quoteId, MarketDataBox.ofScenarioValues(USD_QUOTES[i], USD_QUOTES_1[i]));
		}
		for (int i = 0; i < GBP_QUOTES.Count; ++i)
		{
		  QuoteId quoteId = QuoteId.of(StandardId.of("OG", GBP_USD.ToString() + "-FX-" + GBP_PERIODS[i].ToString()));
		  gbpNodeBuilder.add(FxSwapCurveNode.of(FxSwapTemplate.of(GBP_PERIODS[i], FxSwapConventions.GBP_USD), quoteId));
		  marketQuoteBuilder.put(quoteId, GBP_QUOTES[i]);
		  scenarioMarketQuoteBuilder.put(quoteId, MarketDataBox.ofScenarioValues(GBP_QUOTES[i], GBP_QUOTES_1[i]));
		}
		VOL_NODES = volNodeBuilder.build();
		USD_NODES = usdNodeBuilder.build();
		GBP_NODES = gbpNodeBuilder.build();
		MARKET_QUOTES = marketQuoteBuilder.build();
		SCENARIO_MARKET_QUOTES = scenarioMarketQuoteBuilder.build();
		IList<double> expiry = VOL_TENORS.Select(t => ACT_365F.relativeYearFraction(VALUATION_DATE, BDA.adjust(SPOT_OFFSET.adjust(VALUATION_DATE, REF_DATA).plus(t), REF_DATA))).ToList();
		int nSmiles = expiry.Count;
		double[] atm = new double[nSmiles];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] rr = new double[nSmiles][2];
		double[][] rr = RectangularArrays.ReturnRectangularDoubleArray(nSmiles, 2);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] str = new double[nSmiles][2];
		double[][] str = RectangularArrays.ReturnRectangularDoubleArray(nSmiles, 2);
		for (int i = 0; i < nSmiles; ++i)
		{
		  atm[i] = VOL_QUOTES[i][0];
		  rr[i][0] = VOL_QUOTES[i][1];
		  rr[i][1] = VOL_QUOTES[i][3];
		  str[i][0] = VOL_QUOTES[i][2];
		  str[i][1] = VOL_QUOTES[i][4];
		}
		InterpolatedStrikeSmileDeltaTermStructure term = InterpolatedStrikeSmileDeltaTermStructure.of(DoubleArray.copyOf(expiry), DoubleArray.of(0.1, 0.25), DoubleArray.copyOf(atm), DoubleMatrix.copyOf(rr), DoubleMatrix.copyOf(str), ACT_365F, LINEAR, FLAT, FLAT, PCHIP, FLAT, FLAT);
		EXP_VOLS = BlackFxOptionSmileVolatilities.of(VOL_NAME, GBP_USD, VALUATION_DATE.atTime(VALUATION_TIME).atZone(ZONE), term);
		for (int i = 0; i < nSmiles; ++i)
		{
		  atm[i] = VOL_QUOTES_1[i][0];
		  rr[i][0] = VOL_QUOTES_1[i][1];
		  rr[i][1] = VOL_QUOTES_1[i][3];
		  str[i][0] = VOL_QUOTES_1[i][2];
		  str[i][1] = VOL_QUOTES_1[i][4];
		}
		InterpolatedStrikeSmileDeltaTermStructure term1 = InterpolatedStrikeSmileDeltaTermStructure.of(DoubleArray.copyOf(expiry), DoubleArray.of(0.1, 0.25), DoubleArray.copyOf(atm), DoubleMatrix.copyOf(rr), DoubleMatrix.copyOf(str), ACT_365F, LINEAR, FLAT, FLAT, PCHIP, FLAT, FLAT);
		EXP_VOLS_1 = BlackFxOptionSmileVolatilities.of(VOL_NAME, GBP_USD, VALUATION_DATE_1.atTime(VALUATION_TIME_1).atZone(ZONE), term1);
		ImmutableList.Builder<FxOptionVolatilitiesNode> nodeBuilder = ImmutableList.builder();
		ImmutableMap.Builder<QuoteId, double> quoteBuilder = ImmutableMap.builder();
		for (int i = 0; i < SURFACE_TENORS.Count; ++i)
		{
		  for (int j = 0; j < SURFACE_STRIKES.Count; ++j)
		  {
			QuoteId quoteId = QuoteId.of(StandardId.of("OG", GBP_USD.ToString() + "_" + SURFACE_TENORS[i].ToString() + "_" + SURFACE_STRIKES[j]));
			quoteBuilder.put(quoteId, SURFACE_VOL_QUOTES[i][j]);
			nodeBuilder.add(FxOptionVolatilitiesNode.of(GBP_USD, SPOT_OFFSET, BDA, ValueType.BLACK_VOLATILITY, quoteId, SURFACE_TENORS[i], SimpleStrike.of(SURFACE_STRIKES[j])));
		  }
		}
		SURFACE_NODES = nodeBuilder.build();
		SURFACE_QUOTES = quoteBuilder.build();
		IList<double> expiry = new List<double>();
		IList<double> strike = new List<double>();
		IList<double> vols = new List<double>();
		for (int i = 0; i < SURFACE_TENORS.Count; ++i)
		{
		  for (int j = 0; j < SURFACE_STRIKES.Count; ++j)
		  {
			double yearFraction = ACT_365F.relativeYearFraction(VALUATION_DATE, BDA.adjust(SPOT_OFFSET.adjust(VALUATION_DATE, REF_DATA).plus(SURFACE_TENORS[i]), REF_DATA));
			expiry.Add(yearFraction);
			strike.Add(SURFACE_STRIKES[j]);
			vols.Add(SURFACE_VOL_QUOTES[i][j]);
		  }
		}
		SurfaceInterpolator interp = GridSurfaceInterpolator.of(LINEAR, PCHIP);
		InterpolatedNodalSurface surface = InterpolatedNodalSurface.ofUnsorted(Surfaces.blackVolatilityByExpiryStrike(VOL_NAME.Name, ACT_365F), DoubleArray.copyOf(expiry), DoubleArray.copyOf(strike), DoubleArray.copyOf(vols), interp);
		SURFACE_EXP_VOLS = BlackFxOptionSurfaceVolatilities.of(VOL_NAME, GBP_USD, VALUATION_DATE.atTime(VALUATION_TIME).atZone(ZONE), surface);
	  }
	  private static readonly ImmutableMarketData MARKET_DATA = ImmutableMarketData.builder(VALUATION_DATE).addValueMap(MARKET_QUOTES).addValueMap(MARKET_FX_QUOTES).build();
	  private static readonly LocalDate VALUATION_DATE_1 = VALUATION_DATE.plusDays(7);
	  private static readonly MarketDataBox<LocalDate> VALUATION_DATES = MarketDataBox.ofScenarioValues(VALUATION_DATE, VALUATION_DATE_1);
	  private static readonly ImmutableScenarioMarketData SCENARIO_MARKET_DATA = ImmutableScenarioMarketData.builder(VALUATION_DATES).addBoxMap(SCENARIO_MARKET_QUOTES).addBoxMap(SCENARIO_MARKET_FX_QUOTES).build();

	  private static readonly FxOptionVolatilitiesName VOL_NAME = FxOptionVolatilitiesName.of(GBP_USD.ToString() + "_VOL");
	  private static readonly FxOptionVolatilitiesId VOL_ID = FxOptionVolatilitiesId.of(VOL_NAME);
	  private static readonly FxOptionVolatilitiesDefinition VOL_DEFINITION = FxOptionVolatilitiesDefinition.of(BlackFxOptionSmileVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(GBP_USD).dayCount(ACT_365F).nodes(VOL_NODES).timeInterpolator(LINEAR).strikeInterpolator(PCHIP).build());
	  private static readonly ValuationZoneTimeDefinition ZT_DEFINITION = ValuationZoneTimeDefinition.of(VALUATION_TIME, ZONE);
	  private static readonly ValuationZoneTimeDefinition SCENARIO_ZT_DEFINITION = ValuationZoneTimeDefinition.of(VALUATION_TIME, ZONE, VALUATION_TIME, VALUATION_TIME_1);

	  private static readonly CurveGroupName CURVE_GROUP_NAME = CurveGroupName.of("USD-DSCONOIS-GBP-DSCFX");
	  private static readonly CurveName USD_CURVE_NAME = CurveName.of("USD-DSCON-OIS");
	  private static readonly CurveDefinition USD_CURVE_DEFINITION = InterpolatedNodalCurveDefinition.builder().dayCount(ACT_365F).interpolator(LINEAR).extrapolatorLeft(FLAT).extrapolatorRight(FLAT).name(USD_CURVE_NAME).nodes(USD_NODES).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
	  private static readonly RatesCurveGroupEntry USD_ENTRY = RatesCurveGroupEntry.builder().curveName(USD_CURVE_NAME).discountCurrencies(USD).indices(OvernightIndices.USD_FED_FUND).build();
	  private static readonly CurveName GBP_CURVE_NAME = CurveName.of("GBP-DSC-FX");
	  private static readonly CurveDefinition GBP_CURVE_DEFINITION = InterpolatedNodalCurveDefinition.builder().dayCount(ACT_365F).interpolator(LINEAR).extrapolatorLeft(FLAT).extrapolatorRight(FLAT).name(GBP_CURVE_NAME).nodes(GBP_NODES).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).build();
	  private static readonly RatesCurveGroupEntry GBP_ENTRY = RatesCurveGroupEntry.builder().curveName(GBP_CURVE_NAME).discountCurrencies(GBP).build();
	  private static readonly RatesCurveGroupDefinition CURVE_GROUP_DEFINITION = RatesCurveGroupDefinition.of(CURVE_GROUP_NAME, ImmutableList.of(GBP_ENTRY, USD_ENTRY), ImmutableList.of(GBP_CURVE_DEFINITION, USD_CURVE_DEFINITION));

	  private static readonly MarketDataConfig CONFIG = MarketDataConfig.builder().add(CURVE_GROUP_NAME, CURVE_GROUP_DEFINITION).add(VOL_ID.Name.Name, VOL_DEFINITION).addDefault(ZT_DEFINITION).build();
	  private static readonly MarketDataConfig SCENARIO_CONFIG = MarketDataConfig.builder().add(CURVE_GROUP_NAME, CURVE_GROUP_DEFINITION).add(VOL_ID.Name.Name, VOL_DEFINITION).addDefault(SCENARIO_ZT_DEFINITION).build();

	  private static readonly ZonedDateTime EXPIRY = ZonedDateTime.of(2018, 5, 9, 13, 10, 0, 0, ZONE);
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2018, 5, 13);
	  private const double NOTIONAL = 1.0e6;
	  private static readonly CurrencyAmount GBP_AMOUNT = CurrencyAmount.of(GBP, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT = CurrencyAmount.of(USD, -NOTIONAL * 1.3d);
	  private static readonly FxSingle FX_PRODUCT = FxSingle.of(GBP_AMOUNT, USD_AMOUNT, PAYMENT_DATE);
	  private static readonly FxVanillaOption OPTION_PRODUCT = FxVanillaOption.builder().longShort(SHORT).expiryDate(EXPIRY.toLocalDate()).expiryTime(EXPIRY.toLocalTime()).expiryZone(ZONE).underlying(FX_PRODUCT).build();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(VALUATION_DATE).build();
	  private static readonly LocalDate CASH_SETTLE_DATE = VALUATION_DATE.plusDays(1);
	  private static readonly AdjustablePayment PREMIUM = AdjustablePayment.of(GBP, NOTIONAL * 0.1, CASH_SETTLE_DATE);
	  private static readonly FxVanillaOptionTrade OPTION_TRADE = FxVanillaOptionTrade.builder().premium(PREMIUM).product(OPTION_PRODUCT).info(TRADE_INFO).build();
	  private static readonly IList<CalculationTarget> TARGETS = ImmutableList.of(OPTION_TRADE);

	  private static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(CURVE_GROUP_DEFINITION);
	  private static readonly FxOptionMarketDataLookup VOL_LOOKUP = FxOptionMarketDataLookup.of(GBP_USD, VOL_ID);
	  private static readonly CalculationRules RULES = CalculationRules.of(StandardComponents.calculationFunctions(), USD, RATES_LOOKUP, VOL_LOOKUP);
	  private static readonly IList<Column> COLUMN = ImmutableList.of(Column.of(PRESENT_VALUE));
	  private static readonly MarketDataRequirements REQUIREMENTS = MarketDataRequirements.of(RULES, TARGETS, COLUMN, REF_DATA);
	  private static readonly CalculationRunner CALC_RUNNER = CalculationRunner.ofMultiThreaded();

	  private static readonly RatesCurveCalibrator CURVE_CALIBRATOR = RatesCurveCalibrator.standard();
	  private static readonly RatesProvider EXP_RATES = CURVE_CALIBRATOR.calibrate(CURVE_GROUP_DEFINITION, MARKET_DATA, REF_DATA);
	  private static readonly RatesProvider EXP_RATES_1 = CURVE_CALIBRATOR.calibrate(CURVE_GROUP_DEFINITION, SCENARIO_MARKET_DATA.scenario(1), REF_DATA);
	  private static readonly BlackFxOptionSmileVolatilities EXP_VOLS;
	  private static readonly BlackFxOptionSmileVolatilities EXP_VOLS_1;
	  private static readonly BlackFxVanillaOptionTradePricer PRICER = BlackFxVanillaOptionTradePricer.DEFAULT;

	  public virtual void test_singleMarketData()
	  {
		MarketData marketDataCalibrated = StandardComponents.marketDataFactory().create(REQUIREMENTS, CONFIG, MARKET_DATA, REF_DATA);
		Results results = CALC_RUNNER.calculate(RULES, TARGETS, COLUMN, marketDataCalibrated, REF_DATA);
		CurrencyAmount computed = results.get(0, 0, typeof(CurrencyAmount)).Value;
		CurrencyAmount expected = PRICER.presentValue(OPTION_TRADE.resolve(REF_DATA), EXP_RATES, EXP_VOLS).convertedTo(USD, EXP_RATES);
		assertEquals(computed, expected);
	  }

	  public virtual void test_scenarioMarketData()
	  {
		ScenarioMarketData marketDataCalibrated = StandardComponents.marketDataFactory().createMultiScenario(REQUIREMENTS, SCENARIO_CONFIG, SCENARIO_MARKET_DATA, REF_DATA, ScenarioDefinition.empty());
		Results results = CALC_RUNNER.calculateMultiScenario(RULES, TARGETS, COLUMN, marketDataCalibrated, REF_DATA);
		CurrencyScenarioArray pvs = results.get(0, 0, typeof(CurrencyScenarioArray)).Value;
		CurrencyAmount pv0 = PRICER.presentValue(OPTION_TRADE.resolve(REF_DATA), EXP_RATES, EXP_VOLS).convertedTo(USD, EXP_RATES);
		CurrencyAmount pv1 = PRICER.presentValue(OPTION_TRADE.resolve(REF_DATA), EXP_RATES_1, EXP_VOLS_1).convertedTo(USD, EXP_RATES_1);
		assertEquals(pvs.get(0), pv0);
		assertEquals(pvs.get(1), pv1);
	  }

	  public virtual void test_quote_secenarioDefinition()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.calc.marketdata.PerturbationMapping<?>> perturbationMapping = new java.util.ArrayList<>();
		IList<PerturbationMapping<object>> perturbationMapping = new List<PerturbationMapping<object>>();
		int nScenarios = 3;
		foreach (KeyValuePair<QuoteId, double> entry in MARKET_QUOTES.entrySet())
		{
		  DoubleArray shifts = DoubleArray.of(nScenarios, n => Math.Pow(0.9, n));
		  ScenarioPerturbation<double> perturb = GenericDoubleShifts.of(ShiftType.SCALED, shifts);
		  perturbationMapping.Add(PerturbationMapping.of(MarketDataFilter.ofId(entry.Key), perturb));
		}
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(perturbationMapping);
		ScenarioMarketData marketDataCalibrated = StandardComponents.marketDataFactory().createMultiScenario(REQUIREMENTS, SCENARIO_CONFIG, MARKET_DATA, REF_DATA, scenarioDefinition);
		Results results = CALC_RUNNER.calculateMultiScenario(RULES, TARGETS, COLUMN, marketDataCalibrated, REF_DATA);
		CurrencyScenarioArray pvs = results.get(0, 0, typeof(CurrencyScenarioArray)).Value;
		for (int i = 0; i < nScenarios; ++i)
		{
		  ImmutableMap.Builder<QuoteId, double> builder = ImmutableMap.builder();
		  foreach (KeyValuePair<QuoteId, double> entry in MARKET_QUOTES.entrySet())
		  {
			builder.put(entry.Key, entry.Value * Math.Pow(0.9, i));
		  }
		  ImmutableMarketData shiftedMarketData = ImmutableMarketData.builder(VALUATION_DATE).addValueMap(builder.build()).addValueMap(MARKET_FX_QUOTES).build();
		  MarketData shiftedMarketDataCalibrated = StandardComponents.marketDataFactory().create(REQUIREMENTS, CONFIG, shiftedMarketData, REF_DATA);
		  Results shiftedResults = CALC_RUNNER.calculate(RULES, TARGETS, COLUMN, shiftedMarketDataCalibrated, REF_DATA);
		  CurrencyAmount pv = shiftedResults.get(0, 0, typeof(CurrencyAmount)).Value;
		  assertEquals(pvs.get(i), pv);
		}
	  }

	  public virtual void test_parameter_secenarioDefinition()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.calc.marketdata.PerturbationMapping<?>> perturbationMapping = new java.util.ArrayList<>();
		IList<PerturbationMapping<object>> perturbationMapping = new List<PerturbationMapping<object>>();
		int nVolParams = EXP_VOLS.ParameterCount;
		int nScenarios = 3;
		PointShiftsBuilder builder = PointShifts.builder(ShiftType.SCALED);
		for (int i = 0; i < nVolParams; ++i)
		{
		  object id = EXP_VOLS.getParameterMetadata(i).Identifier;
		  for (int j = 0; j < nScenarios; ++j)
		  {
			builder.addShift(j, id, Math.Pow(0.9, j));
		  }
		}
		ScenarioPerturbation<ParameterizedData> perturb = builder.build();
		perturbationMapping.Add(PerturbationMapping.of(MarketDataFilter.ofId(VOL_ID), perturb));
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(perturbationMapping);
		ScenarioMarketData marketDataCalibrated = StandardComponents.marketDataFactory().createMultiScenario(REQUIREMENTS, SCENARIO_CONFIG, MARKET_DATA, REF_DATA, scenarioDefinition);
		Results results = CALC_RUNNER.calculateMultiScenario(RULES, TARGETS, COLUMN, marketDataCalibrated, REF_DATA);
		CurrencyScenarioArray pvs = results.get(0, 0, typeof(CurrencyScenarioArray)).Value;
		for (int i = 0; i < nScenarios; ++i)
		{
		  int index = i;
		  BlackFxOptionSmileVolatilities shiftedSmile = EXP_VOLS.withPerturbation((j, v, m) => Math.Pow(0.9, index) * v);
		  CurrencyAmount pv = PRICER.presentValue(OPTION_TRADE.resolve(REF_DATA), EXP_RATES, shiftedSmile).convertedTo(USD, EXP_RATES);
		  assertEquals(pvs.get(i), pv);
		}
	  }

	  public virtual void test_builtData()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.calc.marketdata.PerturbationMapping<?>> perturbationMapping = new java.util.ArrayList<>();
		IList<PerturbationMapping<object>> perturbationMapping = new List<PerturbationMapping<object>>();
		int nScenarios = 3;
		foreach (KeyValuePair<QuoteId, double> entry in MARKET_QUOTES.entrySet())
		{
		  DoubleArray shifts = DoubleArray.of(nScenarios, n => Math.Pow(0.9, n));
		  ScenarioPerturbation<double> perturb = GenericDoubleShifts.of(ShiftType.SCALED, shifts);
		  perturbationMapping.Add(PerturbationMapping.of(MarketDataFilter.ofId(entry.Key), perturb));
		}
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(perturbationMapping);
		ImmutableMarketData dataWithSurface = ImmutableMarketData.builder(VALUATION_DATE).addValueMap(MARKET_QUOTES).addValueMap(MARKET_FX_QUOTES).addValue(VOL_ID, EXP_VOLS).addValue(RatesCurveGroupId.of(CURVE_GROUP_NAME), RatesCurveGroup.ofCurves(CURVE_GROUP_DEFINITION, EXP_RATES.toImmutableRatesProvider().DiscountCurves.values())).build();
		ScenarioMarketData marketDataCalibrated = StandardComponents.marketDataFactory().createMultiScenario(REQUIREMENTS, SCENARIO_CONFIG, dataWithSurface, REF_DATA, scenarioDefinition);
		Results results = CALC_RUNNER.calculateMultiScenario(RULES, TARGETS, COLUMN, marketDataCalibrated, REF_DATA);
		CurrencyScenarioArray computed = results.get(0, 0, typeof(CurrencyScenarioArray)).Value;
		CurrencyAmount expected = PRICER.presentValue(OPTION_TRADE.resolve(REF_DATA), EXP_RATES, EXP_VOLS).convertedTo(USD, EXP_RATES);
		// dependency graph is absent, thus scenarios are not created
		assertTrue(computed.ScenarioCount == 1);
		assertEquals(computed.get(0), expected);
	  }

	  private static readonly IList<Tenor> SURFACE_TENORS = ImmutableList.of(Tenor.TENOR_3M, Tenor.TENOR_6M, Tenor.TENOR_1Y);
	  private static readonly IList<double> SURFACE_STRIKES = ImmutableList.of(1.35, 1.5, 1.65, 1.7);
	  private static readonly double[][] SURFACE_VOL_QUOTES = new double[][]
	  {
		  new double[] {0.19, 0.15, 0.13, 0.14},
		  new double[] {0.14, 0.11, 0.09, 0.09},
		  new double[] {0.11, 0.09, 0.07, 0.07}
	  };
	  private static readonly ImmutableList<FxOptionVolatilitiesNode> SURFACE_NODES;
	  private static readonly ImmutableMap<QuoteId, double> SURFACE_QUOTES;
	  private static readonly MarketData SURFACE_MARKET_DATA = MARKET_DATA.combinedWith(MarketData.of(VALUATION_DATE, SURFACE_QUOTES));
	  private static readonly FxOptionVolatilitiesDefinition VOL_DEFINITION_SURFACE = FxOptionVolatilitiesDefinition.of(BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(GBP_USD).dayCount(ACT_365F).nodes(SURFACE_NODES).timeInterpolator(LINEAR).timeExtrapolatorLeft(FLAT).timeExtrapolatorRight(FLAT).strikeInterpolator(PCHIP).strikeExtrapolatorLeft(FLAT).strikeExtrapolatorLeft(FLAT).build());
	  private static readonly MarketDataConfig SURFACE_CONFIG = MarketDataConfig.builder().add(CURVE_GROUP_NAME, CURVE_GROUP_DEFINITION).add(VOL_ID.Name.Name, VOL_DEFINITION_SURFACE).addDefault(ZT_DEFINITION).build();
	  private static readonly BlackFxOptionSurfaceVolatilities SURFACE_EXP_VOLS;

	  public virtual void test_surface()
	  {
		MarketData marketDataCalibrated = StandardComponents.marketDataFactory().create(REQUIREMENTS, SURFACE_CONFIG, SURFACE_MARKET_DATA, REF_DATA);
		Results results = CALC_RUNNER.calculate(RULES, TARGETS, COLUMN, marketDataCalibrated, REF_DATA);
		CurrencyAmount computed = results.get(0, 0, typeof(CurrencyAmount)).Value;
		CurrencyAmount expected = PRICER.presentValue(OPTION_TRADE.resolve(REF_DATA), EXP_RATES, SURFACE_EXP_VOLS).convertedTo(USD, EXP_RATES);
		assertEquals(computed, expected);
	  }

	}

}