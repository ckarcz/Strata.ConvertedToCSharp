using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Iterables = com.google.common.collect.Iterables;
	using Sets = com.google.common.collect.Sets;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using LegAmount = com.opengamma.strata.market.amount.LegAmount;
	using LegAmounts = com.opengamma.strata.market.amount.LegAmounts;
	using SwapLegAmount = com.opengamma.strata.market.amount.SwapLegAmount;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using CrossGammaParameterSensitivities = com.opengamma.strata.market.param.CrossGammaParameterSensitivities;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesMarketData = com.opengamma.strata.measure.rate.RatesMarketData;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using CurveGammaCalculator = com.opengamma.strata.pricer.sensitivity.CurveGammaCalculator;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using DiscountingSwapTradePricer = com.opengamma.strata.pricer.swap.DiscountingSwapTradePricer;
	using NotionalPaymentPeriod = com.opengamma.strata.product.swap.NotionalPaymentPeriod;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;

	/// <summary>
	/// Multi-scenario measure calculations for Swap trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class SwapMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly SwapMeasureCalculations DEFAULT = new SwapMeasureCalculations(DiscountingSwapTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// The cross gamma sensitivity calculator.
	  /// </summary>
	  private static readonly CurveGammaCalculator CROSS_GAMMA = CurveGammaCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;
	  /// <summary>
	  /// Special marker value used in place of null.
	  /// </summary>
	  private static CurrencyAmount NOT_FOUND = CurrencyAmount.zero(Currency.XXX);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedSwapTrade"/>.
	  /// </summary>
	  private readonly DiscountingSwapTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedSwapTrade"/> </param>
	  internal SwapMeasureCalculations(DiscountingSwapTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal MultiCurrencyScenarioArray presentValue(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => presentValue(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // present value for one scenario
	  internal MultiCurrencyAmount presentValue(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.presentValue(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates explain present value for all scenarios
	  internal ScenarioArray<ExplainMap> explainPresentValue(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => explainPresentValue(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // explain present value for one scenario
	  internal ExplainMap explainPresentValue(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.explainPresentValue(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedSum(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01CalibratedSum(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		return ratesProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedBucketed(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		return ratesProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteSum(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01MarketQuoteSum(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteBucketed(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates semi-parallel gamma PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01SemiParallelGammaBucketed(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01SemiParallelGammaBucketed(trade, marketData.scenario(i)));
	  }

	  // semi-parallel gamma PV01 for one scenario
	  private CurrencyParameterSensitivities pv01SemiParallelGammaBucketed(ResolvedSwapTrade trade, RatesMarketData marketData)
	  {

		// find the curve identifiers and resolve to a single curve
		ResolvedSwap product = trade.Product;
		if (product.CrossCurrency)
		{
		  throw new System.ArgumentException("Implementation only supports a single curve, but swap is cross-currency");
		}
		Currency currency = product.Legs.get(0).Currency;
		ISet<Index> indices = product.allIndices();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> discountIds = marketData.getLookup().getDiscountMarketDataIds(currency);
		ImmutableSet<MarketDataId<object>> discountIds = marketData.Lookup.getDiscountMarketDataIds(currency);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> forwardIds = indices.stream().flatMap(idx -> marketData.getLookup().getForwardMarketDataIds(idx).stream()).collect(toImmutableSet());
		ImmutableSet<MarketDataId<object>> forwardIds = indices.stream().flatMap(idx => marketData.Lookup.getForwardMarketDataIds(idx).stream()).collect(toImmutableSet());
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<com.opengamma.strata.data.MarketDataId<?>> allIds = com.google.common.collect.Sets.union(discountIds, forwardIds);
		ISet<MarketDataId<object>> allIds = Sets.union(discountIds, forwardIds);
		if (allIds.Count != 1)
		{
		  throw new System.ArgumentException(Messages.format("Implementation only supports a single curve, but lookup refers to more than one: {}", allIds));
		}
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> singleId = allIds.iterator().next();
		MarketDataId<object> singleId = allIds.GetEnumerator().next();
		if (!(singleId is CurveId))
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException(Messages.format("Implementation only supports a single curve, but lookup does not refer to a curve: {} {}", singleId.GetType().FullName, singleId));
		}
		CurveId curveId = (CurveId) singleId;
		Curve curve = marketData.MarketData.getValue(curveId);

		// calculate gamma
		CurrencyParameterSensitivity gamma = CurveGammaCalculator.DEFAULT.calculateSemiParallelGamma(curve, currency, c => calculateCurveSensitivity(trade, marketData, curveId, c));
		return CurrencyParameterSensitivities.of(gamma).multipliedBy(ONE_BASIS_POINT * ONE_BASIS_POINT);
	  }

	  // calculates the sensitivity
	  private CurrencyParameterSensitivity calculateCurveSensitivity(ResolvedSwapTrade trade, RatesMarketData marketData, CurveId curveId, Curve bumpedCurve)
	  {

		MarketData bumpedMarketData = marketData.MarketData.withValue(curveId, bumpedCurve);
		RatesProvider bumpedRatesProvider = marketData.withMarketData(bumpedMarketData).ratesProvider();
		PointSensitivities pointSensitivities = tradePricer.presentValueSensitivity(trade, bumpedRatesProvider);
		CurrencyParameterSensitivities paramSensitivities = bumpedRatesProvider.parameterSensitivity(pointSensitivities);
		return Iterables.getOnlyElement(paramSensitivities.Sensitivities);
	  }

	  //-------------------------------------------------------------------------
	  // calculates single-node gamma PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01SingleNodeGammaBucketed(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01SingleNodeGammaBucketed(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // single-node gamma PV01 for one scenario
	  private CurrencyParameterSensitivities pv01SingleNodeGammaBucketed(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		CrossGammaParameterSensitivities crossGamma = CROSS_GAMMA.calculateCrossGammaIntraCurve(ratesProvider, p => p.parameterSensitivity(tradePricer.presentValueSensitivity(trade, p)));
		return crossGamma.diagonal().multipliedBy(ONE_BASIS_POINT * ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates par rate for all scenarios
	  internal DoubleScenarioArray parRate(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return DoubleScenarioArray.of(marketData.ScenarioCount, i => parRate(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // par rate for one scenario
	  internal double parRate(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.parRate(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates par spread for all scenarios
	  internal DoubleScenarioArray parSpread(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return DoubleScenarioArray.of(marketData.ScenarioCount, i => parSpread(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // par spread for one scenario
	  internal double parSpread(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.parSpread(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates cash flows for all scenarios
	  internal ScenarioArray<CashFlows> cashFlows(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => cashFlows(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // cash flows for one scenario
	  internal CashFlows cashFlows(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.cashFlows(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates accrued interest for all scenarios
	  internal MultiCurrencyScenarioArray accruedInterest(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => accruedInterest(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // current cash for one scenario
	  internal MultiCurrencyAmount accruedInterest(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.accruedInterest(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates leg initial notional for all scenarios
	  internal ScenarioArray<LegAmounts> legInitialNotional(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		LegAmounts legInitialNotional = this.legInitialNotional(trade);
		return ScenarioArray.ofSingleValue(marketData.ScenarioCount, legInitialNotional);
	  }

	  // leg initial notional, which is the same for all scenarios
	  // package-scoped for testing
	  internal LegAmounts legInitialNotional(ResolvedSwapTrade trade)
	  {
		IList<Pair<ResolvedSwapLeg, CurrencyAmount>> notionals = trade.Product.Legs.Select(leg => Pair.of(leg, buildLegNotional(leg))).ToList();
		CurrencyAmount firstNotional = notionals.Where(pair => pair.Second != NOT_FOUND).Select(pair => pair.Second).First().orElseThrow(() => new System.ArgumentException("No notional found on any swap leg"));
		notionals = notionals.Select(pair => pair.Second != NOT_FOUND ? pair : Pair.of(pair.First, firstNotional)).ToList();
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableList<LegAmount> legAmounts = notionals.Select(pair => SwapLegAmount.of(pair.First, pair.Second)).collect(toImmutableList());
		return LegAmounts.of(legAmounts);
	  }

	  // find the notional
	  private CurrencyAmount buildLegNotional(ResolvedSwapLeg leg)
	  {
		// check for NotionalPaymentPeriod
		SwapPaymentPeriod firstPaymentPeriod = leg.PaymentPeriods.get(0);
		if (firstPaymentPeriod is NotionalPaymentPeriod)
		{
		  NotionalPaymentPeriod pp = (NotionalPaymentPeriod) firstPaymentPeriod;
		  return pp.NotionalAmount.positive();
		}
		return NOT_FOUND;
	  }

	  //-------------------------------------------------------------------------
	  // calculates leg present value for all scenarios
	  internal ScenarioArray<LegAmounts> legPresentValue(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => legPresentValue(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // leg present value for one scenario
	  internal LegAmounts legPresentValue(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		IList<LegAmount> legAmounts = trade.Product.Legs.Select(leg => legAmount(leg, ratesProvider)).ToList();
		return LegAmounts.of(legAmounts);
	  }

	  // present value for a leg
	  private SwapLegAmount legAmount(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		CurrencyAmount amount = tradePricer.ProductPricer.LegPricer.presentValue(leg, provider);
		return SwapLegAmount.of(leg, amount);
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => currencyExposure(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.currencyExposure(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates current cash for all scenarios
	  internal MultiCurrencyScenarioArray currentCash(ResolvedSwapTrade trade, RatesScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => currentCash(trade, marketData.scenario(i).ratesProvider()));
	  }

	  // current cash for one scenario
	  internal MultiCurrencyAmount currentCash(ResolvedSwapTrade trade, RatesProvider ratesProvider)
	  {

		return tradePricer.currentCash(trade, ratesProvider);
	  }

	}

}