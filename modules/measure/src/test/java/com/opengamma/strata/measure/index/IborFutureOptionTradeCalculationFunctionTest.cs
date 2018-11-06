using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Result = com.opengamma.strata.collect.result.Result;
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using MoneynessType = com.opengamma.strata.market.model.MoneynessType;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using IborFutureDummyData = com.opengamma.strata.pricer.index.IborFutureDummyData;
	using IborFutureOptionVolatilitiesId = com.opengamma.strata.pricer.index.IborFutureOptionVolatilitiesId;
	using NormalIborFutureOptionExpirySimpleMoneynessVolatilities = com.opengamma.strata.pricer.index.NormalIborFutureOptionExpirySimpleMoneynessVolatilities;
	using NormalIborFutureOptionMarginedTradePricer = com.opengamma.strata.pricer.index.NormalIborFutureOptionMarginedTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using IborFutureOption = com.opengamma.strata.product.index.IborFutureOption;
	using IborFutureOptionTrade = com.opengamma.strata.product.index.IborFutureOptionTrade;
	using ResolvedIborFutureOptionTrade = com.opengamma.strata.product.index.ResolvedIborFutureOptionTrade;

	/// <summary>
	/// Test <seealso cref="IborFutureOptionTradeCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFutureOptionTradeCalculationFunctionTest
	public class IborFutureOptionTradeCalculationFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly DoubleArray TIMES = DoubleArray.of(0.25, 0.25, 0.25, 0.25, 0.5, 0.5, 0.5, 0.5, 1, 1, 1, 1);
	  private static readonly DoubleArray MONEYNESS_PRICES = DoubleArray.of(-0.02, -0.01, 0, 0.01, -0.02, -0.01, 0, 0.01, -0.02, -0.01, 0, 0.01);
	  private static readonly DoubleArray NORMAL_VOL_PRICES = DoubleArray.of(0.01, 0.011, 0.012, 0.010, 0.011, 0.012, 0.013, 0.012, 0.012, 0.013, 0.014, 0.014);
	  private static readonly InterpolatedNodalSurface PARAMETERS_PRICE = InterpolatedNodalSurface.of(Surfaces.normalVolatilityByExpirySimpleMoneyness("Price", ACT_365F, MoneynessType.PRICE), TIMES, MONEYNESS_PRICES, NORMAL_VOL_PRICES, INTERPOLATOR_2D);

	  private static readonly LocalDate VAL_DATE = date(2015, 2, 17);
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId LONDON_ZONE = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(LONDON_ZONE);

	  private static readonly NormalIborFutureOptionExpirySimpleMoneynessVolatilities VOL_SIMPLE_MONEY_PRICE = NormalIborFutureOptionExpirySimpleMoneynessVolatilities.of(GBP_LIBOR_2M, VAL_DATE_TIME, PARAMETERS_PRICE);

	  private static readonly IborFutureOption OPTION = IborFutureDummyData.IBOR_FUTURE_OPTION_2;
	  private static readonly LocalDate TRADE_DATE = date(2015, 2, 16);
	  private const long OPTION_QUANTITY = 12345;
	  private const double TRADE_PRICE = 0.0100;
	  private const double SETTLEMENT_PRICE = 0.0120;
	  private static readonly IborFutureOptionTrade TRADE = IborFutureOptionTrade.builder().info(TradeInfo.builder().tradeDate(TRADE_DATE).build()).product(OPTION).quantity(OPTION_QUANTITY).price(TRADE_PRICE).build();

	  private static readonly Currency CURRENCY = Currency.GBP;
	  private static readonly IborIndex INDEX = GBP_LIBOR_2M;

	  private static readonly CurveId DISCOUNT_CURVE_ID = CurveId.of("Default", "Discount");
	  private static readonly CurveId FORWARD_CURVE_ID = CurveId.of("Default", "Forward");
	  private static readonly IborFutureOptionVolatilitiesId VOL_ID = IborFutureOptionVolatilitiesId.of("IborFutureOptionVols.Normal.USD");
	  private static readonly QuoteId QUOTE_ID_OPTION = QuoteId.of(TRADE.SecurityId.StandardId, FieldName.SETTLEMENT_PRICE);
	  internal static readonly RatesMarketDataLookup RATES_LOOKUP = RatesMarketDataLookup.of(ImmutableMap.of(CURRENCY, DISCOUNT_CURVE_ID), ImmutableMap.of(INDEX, FORWARD_CURVE_ID));
	  internal static readonly IborFutureOptionMarketDataLookup OPTION_LOOKUP = IborFutureOptionMarketDataLookup.of(INDEX, VOL_ID);
	  private static readonly CalculationParameters PARAMS = CalculationParameters.of(RATES_LOOKUP, OPTION_LOOKUP);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		IborFutureOptionTradeCalculationFunction<IborFutureOptionTrade> function = IborFutureOptionTradeCalculationFunction.TRADE;
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(TRADE, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(DISCOUNT_CURVE_ID, FORWARD_CURVE_ID, VOL_ID, QUOTE_ID_OPTION));
		assertThat(reqs.TimeSeriesRequirements).isEqualTo(ImmutableSet.of(IndexQuoteId.of(INDEX)));
		assertThat(function.naturalCurrency(TRADE, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_simpleMeasures()
	  {
		IborFutureOptionTradeCalculationFunction<IborFutureOptionTrade> function = IborFutureOptionTradeCalculationFunction.TRADE;
		ScenarioMarketData md = marketData();
		RatesProvider provider = RATES_LOOKUP.ratesProvider(md.scenario(0));
		NormalIborFutureOptionMarginedTradePricer pricer = NormalIborFutureOptionMarginedTradePricer.DEFAULT;
		ResolvedIborFutureOptionTrade resolved = TRADE.resolve(REF_DATA);
		CurrencyAmount expectedPv = pricer.presentValue(resolved, provider, VOL_SIMPLE_MONEY_PRICE, SETTLEMENT_PRICE);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE, Measures.RESOLVED_TARGET);
		assertThat(function.calculate(TRADE, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv)))).containsEntry(Measures.RESOLVED_TARGET, Result.success(resolved));
	  }

	  //-------------------------------------------------------------------------
	  internal static ScenarioMarketData marketData()
	  {
		Curve curve = ConstantCurve.of(Curves.discountFactors("Test", ACT_360), 0.99);
		TestMarketDataMap md = new TestMarketDataMap(VAL_DATE, ImmutableMap.of(DISCOUNT_CURVE_ID, curve, FORWARD_CURVE_ID, curve, VOL_ID, VOL_SIMPLE_MONEY_PRICE, QUOTE_ID_OPTION, SETTLEMENT_PRICE), ImmutableMap.of());
		return md;
	  }

	}

}