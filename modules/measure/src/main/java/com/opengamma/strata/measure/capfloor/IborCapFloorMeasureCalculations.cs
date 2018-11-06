/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.capfloor
{
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using IborCapletFloorletVolatilities = com.opengamma.strata.pricer.capfloor.IborCapletFloorletVolatilities;
	using VolatilityIborCapFloorTradePricer = com.opengamma.strata.pricer.capfloor.VolatilityIborCapFloorTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedIborCapFloorTrade = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorTrade;

	/// <summary>
	/// Multi-scenario measure calculations for Ibor cap/floor trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class IborCapFloorMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly IborCapFloorMeasureCalculations DEFAULT = new IborCapFloorMeasureCalculations(VolatilityIborCapFloorTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedIborCapFloorTrade"/>.
	  /// </summary>
	  private readonly VolatilityIborCapFloorTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedIborCapFloorTrade"/> </param>
	  internal IborCapFloorMeasureCalculations(VolatilityIborCapFloorTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal MultiCurrencyScenarioArray presentValue(ResolvedIborCapFloorTrade trade, RatesScenarioMarketData ratesMarketData, IborCapFloorScenarioMarketData capFloorMarketData)
	  {

		IborIndex index = trade.Product.CapFloorLeg.Index;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => presentValue(trade, ratesMarketData.scenario(i).ratesProvider(), capFloorMarketData.scenario(i).volatilities(index)));
	  }

	  // present value for one scenario
	  internal MultiCurrencyAmount presentValue(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		return tradePricer.presentValue(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01RatesCalibratedSum(ResolvedIborCapFloorTrade trade, RatesScenarioMarketData ratesMarketData, IborCapFloorScenarioMarketData capFloorMarketData)
	  {

		IborIndex index = trade.Product.CapFloorLeg.Index;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesCalibratedSum(trade, ratesMarketData.scenario(i).ratesProvider(), capFloorMarketData.scenario(i).volatilities(index)));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01RatesCalibratedSum(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		return ratesProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01RatesCalibratedBucketed(ResolvedIborCapFloorTrade trade, RatesScenarioMarketData ratesMarketData, IborCapFloorScenarioMarketData capFloorMarketData)
	  {

		IborIndex index = trade.Product.CapFloorLeg.Index;
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesCalibratedBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), capFloorMarketData.scenario(i).volatilities(index)));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01RatesCalibratedBucketed(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		return ratesProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01RatesMarketQuoteSum(ResolvedIborCapFloorTrade trade, RatesScenarioMarketData ratesMarketData, IborCapFloorScenarioMarketData capFloorMarketData)
	  {

		IborIndex index = trade.Product.CapFloorLeg.Index;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesMarketQuoteSum(trade, ratesMarketData.scenario(i).ratesProvider(), capFloorMarketData.scenario(i).volatilities(index)));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01RatesMarketQuoteSum(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01RatesMarketQuoteBucketed(ResolvedIborCapFloorTrade trade, RatesScenarioMarketData ratesMarketData, IborCapFloorScenarioMarketData capFloorMarketData)
	  {

		IborIndex index = trade.Product.CapFloorLeg.Index;
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesMarketQuoteBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), capFloorMarketData.scenario(i).volatilities(index)));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01RatesMarketQuoteBucketed(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  // point sensitivity
	  private PointSensitivities pointSensitivity(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		return tradePricer.presentValueSensitivityRates(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedIborCapFloorTrade trade, RatesScenarioMarketData ratesMarketData, IborCapFloorScenarioMarketData capFloorMarketData)
	  {

		IborIndex index = trade.Product.CapFloorLeg.Index;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => currencyExposure(trade, ratesMarketData.scenario(i).ratesProvider(), capFloorMarketData.scenario(i).volatilities(index)));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		return tradePricer.currencyExposure(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  // calculates current cash for all scenarios
	  internal MultiCurrencyScenarioArray currentCash(ResolvedIborCapFloorTrade trade, RatesScenarioMarketData ratesMarketData, IborCapFloorScenarioMarketData capFloorMarketData)
	  {

		IborIndex index = trade.Product.CapFloorLeg.Index;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => currentCash(trade, ratesMarketData.scenario(i).ratesProvider(), capFloorMarketData.scenario(i).volatilities(index)));
	  }

	  // current cash for one scenario
	  internal MultiCurrencyAmount currentCash(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		return tradePricer.currentCash(trade, ratesProvider, volatilities);
	  }

	}

}