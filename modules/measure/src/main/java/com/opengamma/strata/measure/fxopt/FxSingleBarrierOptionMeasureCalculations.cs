/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using BlackFxOptionVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionVolatilities;
	using BlackFxSingleBarrierOptionTradePricer = com.opengamma.strata.pricer.fxopt.BlackFxSingleBarrierOptionTradePricer;
	using FxOptionVolatilities = com.opengamma.strata.pricer.fxopt.FxOptionVolatilities;
	using ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer = com.opengamma.strata.pricer.fxopt.ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedFxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOptionTrade;

	/// <summary>
	/// Multi-scenario measure calculations for FX single barrier option trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class FxSingleBarrierOptionMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly FxSingleBarrierOptionMeasureCalculations DEFAULT = new FxSingleBarrierOptionMeasureCalculations(BlackFxSingleBarrierOptionTradePricer.DEFAULT, ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxSingleBarrierOptionTrade"/>.
	  /// </summary>
	  private readonly BlackFxSingleBarrierOptionTradePricer blackPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxSingleBarrierOptionTrade"/>.
	  /// </summary>
	  private readonly ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer trinomialTreePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="blackPricer">  the pricer for <seealso cref="ResolvedFxSingleBarrierOptionTrade"/> </param>
	  /// <param name="trinomialTreePricer">  the pricer for <seealso cref="ResolvedFxSingleBarrierOptionTrade"/> SABR </param>
	  internal FxSingleBarrierOptionMeasureCalculations(BlackFxSingleBarrierOptionTradePricer blackPricer, ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer trinomialTreePricer)
	  {
		this.blackPricer = ArgChecker.notNull(blackPricer, "blackPricer");
		this.trinomialTreePricer = ArgChecker.notNull(trinomialTreePricer, "trinomialTreePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal MultiCurrencyScenarioArray presentValue(ResolvedFxSingleBarrierOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxSingleBarrierOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => presentValue(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // present value for one scenario
	  internal MultiCurrencyAmount presentValue(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxSingleBarrierOptionMethod method)
	  {

		if (method == FxSingleBarrierOptionMethod.TRINOMIAL_TREE)
		{
		  return trinomialTreePricer.presentValue(trade, ratesProvider, checkTrinomialTreeVolatilities(volatilities));
		}
		else
		{
		  return blackPricer.presentValue(trade, ratesProvider, checkBlackVolatilities(volatilities));
		}
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01RatesCalibratedSum(ResolvedFxSingleBarrierOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxSingleBarrierOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesCalibratedSum(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01RatesCalibratedSum(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxSingleBarrierOptionMethod method)
	  {

		CurrencyParameterSensitivities paramSens = parameterSensitivities(trade, ratesProvider, volatilities, method);
		return paramSens.total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01RatesCalibratedBucketed(ResolvedFxSingleBarrierOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxSingleBarrierOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesCalibratedBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01RatesCalibratedBucketed(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxSingleBarrierOptionMethod method)
	  {

		CurrencyParameterSensitivities paramSens = parameterSensitivities(trade, ratesProvider, volatilities, method);
		return paramSens.multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01RatesMarketQuoteSum(ResolvedFxSingleBarrierOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxSingleBarrierOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesMarketQuoteSum(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01RatesMarketQuoteSum(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxSingleBarrierOptionMethod method)
	  {

		CurrencyParameterSensitivities paramSens = parameterSensitivities(trade, ratesProvider, volatilities, method);
		return MARKET_QUOTE_SENS.sensitivity(paramSens, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01RatesMarketQuoteBucketed(ResolvedFxSingleBarrierOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxSingleBarrierOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesMarketQuoteBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01RatesMarketQuoteBucketed(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxSingleBarrierOptionMethod method)
	  {

		CurrencyParameterSensitivities paramSens = parameterSensitivities(trade, ratesProvider, volatilities, method);
		return MARKET_QUOTE_SENS.sensitivity(paramSens, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  // point sensitivity
	  private CurrencyParameterSensitivities parameterSensitivities(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxSingleBarrierOptionMethod method)
	  {

		if (method == FxSingleBarrierOptionMethod.TRINOMIAL_TREE)
		{
		  return trinomialTreePricer.presentValueSensitivityRates(trade, ratesProvider, checkTrinomialTreeVolatilities(volatilities));
		}
		else
		{
		  PointSensitivities pointSens = blackPricer.presentValueSensitivityRatesStickyStrike(trade, ratesProvider, checkBlackVolatilities(volatilities));
		  return ratesProvider.parameterSensitivity(pointSens);
		}
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedFxSingleBarrierOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxSingleBarrierOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => currencyExposure(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxSingleBarrierOptionMethod method)
	  {

		if (method == FxSingleBarrierOptionMethod.TRINOMIAL_TREE)
		{
		  return trinomialTreePricer.currencyExposure(trade, ratesProvider, checkTrinomialTreeVolatilities(volatilities));
		}
		else
		{
		  return blackPricer.currencyExposure(trade, ratesProvider, checkBlackVolatilities(volatilities));
		}
	  }

	  //-------------------------------------------------------------------------
	  // calculates current cash for all scenarios
	  internal CurrencyScenarioArray currentCash(ResolvedFxSingleBarrierOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxSingleBarrierOptionMethod method)
	  {

		return CurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => currentCash(trade, ratesMarketData.scenario(i).ValuationDate, method));
	  }

	  // current cash for one scenario
	  internal CurrencyAmount currentCash(ResolvedFxSingleBarrierOptionTrade trade, LocalDate valuationDate, FxSingleBarrierOptionMethod method)
	  {

		if (method == FxSingleBarrierOptionMethod.TRINOMIAL_TREE)
		{
		  return trinomialTreePricer.currentCash(trade, valuationDate);
		}
		else
		{
		  return blackPricer.currentCash(trade, valuationDate);
		}
	  }

	  //-------------------------------------------------------------------------
	  // ensures that the volatilities are correct
	  private BlackFxOptionVolatilities checkBlackVolatilities(FxOptionVolatilities volatilities)
	  {
		if (volatilities is BlackFxOptionVolatilities)
		{
		  return (BlackFxOptionVolatilities) volatilities;
		}
		throw new System.ArgumentException("FX single barrier option Black pricing requires BlackFxOptionVolatilities");
	  }

	  // ensures that the volatilities are correct
	  private BlackFxOptionVolatilities checkTrinomialTreeVolatilities(FxOptionVolatilities volatilities)
	  {
		if (volatilities is BlackFxOptionVolatilities)
		{
		  return (BlackFxOptionVolatilities) volatilities;
		}
		throw new System.ArgumentException("FX single barrier option Trinomial Tree pricing requires BlackFxOptionVolatilities");
	  }

	}

}