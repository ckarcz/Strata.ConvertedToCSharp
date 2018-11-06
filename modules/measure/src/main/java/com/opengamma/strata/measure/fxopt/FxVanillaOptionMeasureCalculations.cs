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
	using BlackFxOptionSmileVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionSmileVolatilities;
	using BlackFxOptionVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionVolatilities;
	using BlackFxVanillaOptionTradePricer = com.opengamma.strata.pricer.fxopt.BlackFxVanillaOptionTradePricer;
	using FxOptionVolatilities = com.opengamma.strata.pricer.fxopt.FxOptionVolatilities;
	using VannaVolgaFxVanillaOptionTradePricer = com.opengamma.strata.pricer.fxopt.VannaVolgaFxVanillaOptionTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedFxVanillaOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOptionTrade;

	/// <summary>
	/// Multi-scenario measure calculations for FX vanilla option trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class FxVanillaOptionMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly FxVanillaOptionMeasureCalculations DEFAULT = new FxVanillaOptionMeasureCalculations(BlackFxVanillaOptionTradePricer.DEFAULT, VannaVolgaFxVanillaOptionTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxVanillaOptionTrade"/>.
	  /// </summary>
	  private readonly BlackFxVanillaOptionTradePricer blackPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxVanillaOptionTrade"/>.
	  /// </summary>
	  private readonly VannaVolgaFxVanillaOptionTradePricer vannaVolgaPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="blackPricer">  the pricer for <seealso cref="ResolvedFxVanillaOptionTrade"/> using Black </param>
	  /// <param name="vannaVolgaPricer">  the pricer for <seealso cref="ResolvedFxVanillaOptionTrade"/> using Vanna-Volga </param>
	  internal FxVanillaOptionMeasureCalculations(BlackFxVanillaOptionTradePricer blackPricer, VannaVolgaFxVanillaOptionTradePricer vannaVolgaPricer)
	  {
		this.blackPricer = ArgChecker.notNull(blackPricer, "blackPricer");
		this.vannaVolgaPricer = ArgChecker.notNull(vannaVolgaPricer, "vannaVolgaPricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal MultiCurrencyScenarioArray presentValue(ResolvedFxVanillaOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxVanillaOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => presentValue(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // present value for one scenario
	  internal MultiCurrencyAmount presentValue(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		if (method == FxVanillaOptionMethod.VANNA_VOLGA)
		{
		  return vannaVolgaPricer.presentValue(trade, ratesProvider, checkVannaVolgaVolatilities(volatilities));
		}
		else
		{
		  return blackPricer.presentValue(trade, ratesProvider, checkBlackVolatilities(volatilities));
		}
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01RatesCalibratedSum(ResolvedFxVanillaOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxVanillaOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesCalibratedSum(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01RatesCalibratedSum(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		CurrencyParameterSensitivities paramSens = pointSensitivity(trade, ratesProvider, volatilities, method);
		return paramSens.total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01RatesCalibratedBucketed(ResolvedFxVanillaOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxVanillaOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesCalibratedBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01RatesCalibratedBucketed(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		CurrencyParameterSensitivities paramSens = pointSensitivity(trade, ratesProvider, volatilities, method);
		return paramSens.multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01RatesMarketQuoteSum(ResolvedFxVanillaOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxVanillaOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesMarketQuoteSum(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01RatesMarketQuoteSum(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		CurrencyParameterSensitivities paramSens = pointSensitivity(trade, ratesProvider, volatilities, method);
		return MARKET_QUOTE_SENS.sensitivity(paramSens, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01RatesMarketQuoteBucketed(ResolvedFxVanillaOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxVanillaOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesMarketQuoteBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01RatesMarketQuoteBucketed(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		CurrencyParameterSensitivities paramSens = pointSensitivity(trade, ratesProvider, volatilities, method);
		return MARKET_QUOTE_SENS.sensitivity(paramSens, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  // point sensitivity
	  private CurrencyParameterSensitivities pointSensitivity(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		PointSensitivities pointSens;
		if (method == FxVanillaOptionMethod.VANNA_VOLGA)
		{
		  pointSens = vannaVolgaPricer.presentValueSensitivityRatesStickyStrike(trade, ratesProvider, checkVannaVolgaVolatilities(volatilities));
		}
		else
		{
		  pointSens = blackPricer.presentValueSensitivityRatesStickyStrike(trade, ratesProvider, checkBlackVolatilities(volatilities));
		}
		return ratesProvider.parameterSensitivity(pointSens);
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedFxVanillaOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxVanillaOptionMethod method)
	  {

		CurrencyPair currencyPair = trade.Product.CurrencyPair;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => currencyExposure(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(currencyPair), method));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		if (method == FxVanillaOptionMethod.VANNA_VOLGA)
		{
		  return vannaVolgaPricer.currencyExposure(trade, ratesProvider, checkVannaVolgaVolatilities(volatilities));
		}
		else
		{
		  return blackPricer.currencyExposure(trade, ratesProvider, checkBlackVolatilities(volatilities));
		}
	  }

	  //-------------------------------------------------------------------------
	  // calculates current cash for all scenarios
	  internal CurrencyScenarioArray currentCash(ResolvedFxVanillaOptionTrade trade, RatesScenarioMarketData ratesMarketData, FxOptionScenarioMarketData optionMarketData, FxVanillaOptionMethod method)
	  {

		return CurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => currentCash(trade, ratesMarketData.scenario(i).ValuationDate, method));
	  }

	  // current cash for one scenario
	  internal CurrencyAmount currentCash(ResolvedFxVanillaOptionTrade trade, LocalDate valuationDate, FxVanillaOptionMethod method)
	  {

		if (method == FxVanillaOptionMethod.VANNA_VOLGA)
		{
		  return vannaVolgaPricer.currentCash(trade, valuationDate);
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
		throw new System.ArgumentException("FX vanilla option Black pricing requires BlackFxOptionVolatilities");
	  }

	  // ensures that the volatilities are correct
	  private BlackFxOptionSmileVolatilities checkVannaVolgaVolatilities(FxOptionVolatilities volatilities)
	  {
		if (volatilities is BlackFxOptionSmileVolatilities)
		{
		  return (BlackFxOptionSmileVolatilities) volatilities;
		}
		throw new System.ArgumentException("FX vanilla option Vanna Volga pricing requires BlackFxOptionSmileVolatilities");
	  }

	}

}