/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swaption
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using SabrSwaptionTradePricer = com.opengamma.strata.pricer.swaption.SabrSwaptionTradePricer;
	using SabrSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrSwaptionVolatilities;
	using SwaptionVolatilities = com.opengamma.strata.pricer.swaption.SwaptionVolatilities;
	using VolatilitySwaptionTradePricer = com.opengamma.strata.pricer.swaption.VolatilitySwaptionTradePricer;
	using ResolvedSwaptionTrade = com.opengamma.strata.product.swaption.ResolvedSwaptionTrade;

	/// <summary>
	/// Multi-scenario measure calculations for Swaption trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class SwaptionMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly SwaptionMeasureCalculations DEFAULT = new SwaptionMeasureCalculations(VolatilitySwaptionTradePricer.DEFAULT, SabrSwaptionTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedSwaptionTrade"/>.
	  /// </summary>
	  private readonly VolatilitySwaptionTradePricer tradePricer;
	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedSwaptionTrade"/>.
	  /// </summary>
	  private readonly SabrSwaptionTradePricer sabrTradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedSwaptionTrade"/> </param>
	  /// <param name="sabrTradePricer">  the pricer for <seealso cref="ResolvedSwaptionTrade"/> SABR </param>
	  internal SwaptionMeasureCalculations(VolatilitySwaptionTradePricer tradePricer, SabrSwaptionTradePricer sabrTradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
		this.sabrTradePricer = ArgChecker.notNull(sabrTradePricer, "sabrTradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal CurrencyScenarioArray presentValue(ResolvedSwaptionTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = trade.Product.Index;
		return CurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => presentValue(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // present value for one scenario
	  internal CurrencyAmount presentValue(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return tradePricer.presentValue(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01RatesCalibratedSum(ResolvedSwaptionTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = trade.Product.Index;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesCalibratedSum(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01RatesCalibratedSum(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		return ratesProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01RatesCalibratedBucketed(ResolvedSwaptionTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = trade.Product.Index;
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesCalibratedBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01RatesCalibratedBucketed(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		return ratesProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01RatesMarketQuoteSum(ResolvedSwaptionTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = trade.Product.Index;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesMarketQuoteSum(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01RatesMarketQuoteSum(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01RatesMarketQuoteBucketed(ResolvedSwaptionTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = trade.Product.Index;
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01RatesMarketQuoteBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01RatesMarketQuoteBucketed(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		PointSensitivities pointSensitivity = this.pointSensitivity(trade, ratesProvider, volatilities);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  // point sensitivity
	  private PointSensitivities pointSensitivity(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		if (volatilities is SabrSwaptionVolatilities)
		{
		  return sabrTradePricer.presentValueSensitivityRatesStickyModel(trade, ratesProvider, (SabrSwaptionVolatilities) volatilities);
		}
		return tradePricer.presentValueSensitivityRatesStickyStrike(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedSwaptionTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		IborIndex index = trade.Product.Index;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => currencyExposure(trade, ratesMarketData.scenario(i).ratesProvider(), swaptionMarketData.scenario(i).volatilities(index)));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return tradePricer.currencyExposure(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  // calculates current cash for all scenarios
	  internal CurrencyScenarioArray currentCash(ResolvedSwaptionTrade trade, RatesScenarioMarketData ratesMarketData, SwaptionScenarioMarketData swaptionMarketData)
	  {

		return CurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => currentCash(trade, ratesMarketData.scenario(i).ValuationDate));
	  }

	  // current cash for one scenario
	  internal CurrencyAmount currentCash(ResolvedSwaptionTrade trade, LocalDate valuationDate)
	  {

		return tradePricer.currentCash(trade, valuationDate);
	  }

	}

}