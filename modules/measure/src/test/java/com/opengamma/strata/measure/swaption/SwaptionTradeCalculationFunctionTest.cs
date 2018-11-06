using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.LONG;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Result = com.opengamma.strata.collect.result.Result;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using NormalSwaptionExpiryTenorVolatilities = com.opengamma.strata.pricer.swaption.NormalSwaptionExpiryTenorVolatilities;
	using NormalSwaptionTradePricer = com.opengamma.strata.pricer.swaption.NormalSwaptionTradePricer;
	using SwaptionNormalVolatilityDataSets = com.opengamma.strata.pricer.swaption.SwaptionNormalVolatilityDataSets;
	using SwaptionVolatilitiesId = com.opengamma.strata.pricer.swaption.SwaptionVolatilitiesId;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;
	using PhysicalSwaptionSettlement = com.opengamma.strata.product.swaption.PhysicalSwaptionSettlement;
	using ResolvedSwaptionTrade = com.opengamma.strata.product.swaption.ResolvedSwaptionTrade;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;
	using SwaptionSettlement = com.opengamma.strata.product.swaption.SwaptionSettlement;
	using SwaptionTrade = com.opengamma.strata.product.swaption.SwaptionTrade;

	/// <summary>
	/// Test <seealso cref="SwaptionTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwaptionTradeCalculationFunctionTest
	public class SwaptionTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double FIXED_RATE = 0.015;
	  private const double NOTIONAL = 100000000d;
	  private static readonly Swap SWAP = FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.createTrade(LocalDate.of(2014, 6, 12), Tenor.TENOR_10Y, BuySell.BUY, NOTIONAL, FIXED_RATE, REF_DATA).Product;
	  private static readonly BusinessDayAdjustment ADJUSTMENT = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, GBLO.combinedWith(USNY));
	  private static readonly LocalDate EXPIRY_DATE = LocalDate.of(2014, 6, 14);
	  private static readonly AdjustableDate ADJUSTABLE_EXPIRY_DATE = AdjustableDate.of(EXPIRY_DATE, ADJUSTMENT);
	  private static readonly LocalTime EXPIRY_TIME = LocalTime.of(11, 0);
	  private static readonly ZoneId ZONE = ZoneId.of("Z");
	  private static readonly SwaptionSettlement PHYSICAL_SETTLE = PhysicalSwaptionSettlement.DEFAULT;
	  private static readonly Swaption SWAPTION = Swaption.builder().expiryDate(ADJUSTABLE_EXPIRY_DATE).expiryTime(EXPIRY_TIME).expiryZone(ZONE).longShort(LONG).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP).build();
	  private static readonly AdjustablePayment PREMIUM = AdjustablePayment.of(CurrencyAmount.of(Currency.USD, -3150000d), LocalDate.of(2014, 3, 17));
	  public static readonly SwaptionTrade TRADE = SwaptionTrade.builder().premium(PREMIUM).product(SWAPTION).build();
	  public static readonly ResolvedSwaptionTrade RTRADE = TRADE.resolve(REF_DATA);
	  private static readonly Currency CURRENCY = Currency.USD;
	  private static readonly IborIndex INDEX = IborIndices.USD_LIBOR_3M;

	  public static readonly NormalSwaptionExpiryTenorVolatilities NORMAL_VOL_SWAPTION_PROVIDER_USD = SwaptionNormalVolatilityDataSets.NORMAL_SWAPTION_VOLS_USD_STD;
	  private static readonly CurveId DISCOUNT_CURVE_ID = CurveId.of("Default", "Discount");
	  private static readonly CurveId FORWARD_CURVE_ID = CurveId.of("Default", "Forward");
	  private static readonly SwaptionVolatilitiesId VOL_ID = SwaptionVolatilitiesId.of("SwaptionVols.Normal.USD");
	  internal static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(CURRENCY, DISCOUNT_CURVE_ID), ImmutableMap.of(INDEX, FORWARD_CURVE_ID));
	  internal static readonly SwaptionMarketDataLookup SWAPTION_LOOKUP = SwaptionMarketDataLookup.of(INDEX, VOL_ID);
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP, SWAPTION_LOOKUP);
	  private static readonly LocalDate VAL_DATE = NORMAL_VOL_SWAPTION_PROVIDER_USD.ValuationDate;

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		SwaptionTradeCalculationFunction function = new SwaptionTradeCalculationFunction();
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(DISCOUNT_CURVE_ID, FORWARD_CURVE_ID, VOL_ID));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of(IndexQuoteId.of(INDEX)));
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		SwaptionTradeCalculationFunction function = new SwaptionTradeCalculationFunction();
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		NormalSwaptionTradePricer pricer = NormalSwaptionTradePricer.DEFAULT;
		ResolvedSwaptionTrade resolved = TRADE.resolve(REF_DATA);
		CurrencyAmount expectedPv = pricer.presentValue(resolved, provider, NORMAL_VOL_SWAPTION_PROVIDER_USD);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(RTRADE));
	  }

	  //-------------------------------------------------------------------------
	  internal static ScenarioMarketData marketData()
	  {
		Curve curve = ConstantCurve.of(Curves.discountFactors("Test", ACT_360), 0.99);
		TestMarketDataMap md = new TestMarketDataMap(VAL_DATE, ImmutableMap.of(DISCOUNT_CURVE_ID, curve, FORWARD_CURVE_ID, curve, VOL_ID, NORMAL_VOL_SWAPTION_PROVIDER_USD), ImmutableMap.of());
		return md;
	  }

	}

}