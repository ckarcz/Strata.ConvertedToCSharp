/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.index
{
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesScenarioMarketData = com.opengamma.strata.measure.rate.RatesScenarioMarketData;
	using IborFutureOptionVolatilities = com.opengamma.strata.pricer.index.IborFutureOptionVolatilities;
	using NormalIborFutureOptionMarginedTradePricer = com.opengamma.strata.pricer.index.NormalIborFutureOptionMarginedTradePricer;
	using NormalIborFutureOptionVolatilities = com.opengamma.strata.pricer.index.NormalIborFutureOptionVolatilities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedIborFutureOptionTrade = com.opengamma.strata.product.index.ResolvedIborFutureOptionTrade;

	/// <summary>
	/// Multi-scenario measure calculations for Ibor Future Option trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class IborFutureOptionMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly IborFutureOptionMeasureCalculations DEFAULT = new IborFutureOptionMeasureCalculations(NormalIborFutureOptionMarginedTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedIborFutureOptionTrade"/>.
	  /// </summary>
	  private readonly NormalIborFutureOptionMarginedTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedIborFutureOptionTrade"/> </param>
	  internal IborFutureOptionMeasureCalculations(NormalIborFutureOptionMarginedTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal CurrencyScenarioArray presentValue(ResolvedIborFutureOptionTrade trade, RatesScenarioMarketData ratesMarketData, IborFutureOptionScenarioMarketData optionMarketData)
	  {

		IborIndex index = trade.Product.UnderlyingFuture.Index;
		return CurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => presentValue(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(index)));
	  }

	  // present value for one scenario
	  internal CurrencyAmount presentValue(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		// mark to model
		double settlementPrice = this.settlementPrice(trade, ratesProvider);
		NormalIborFutureOptionVolatilities normalVols = checkNormalVols(volatilities);
		return tradePricer.presentValue(trade, ratesProvider, normalVols, settlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedIborFutureOptionTrade trade, RatesScenarioMarketData ratesMarketData, IborFutureOptionScenarioMarketData optionMarketData)
	  {

		IborIndex index = trade.Product.UnderlyingFuture.Index;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01CalibratedSum(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(index)));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01CalibratedSum(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		NormalIborFutureOptionVolatilities normalVols = checkNormalVols(volatilities);
		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivityRates(trade, ratesProvider, normalVols);
		return ratesProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedIborFutureOptionTrade trade, RatesScenarioMarketData ratesMarketData, IborFutureOptionScenarioMarketData optionMarketData)
	  {

		IborIndex index = trade.Product.UnderlyingFuture.Index;
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01CalibratedBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(index)));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		NormalIborFutureOptionVolatilities normalVols = checkNormalVols(volatilities);
		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivityRates(trade, ratesProvider, normalVols);
		return ratesProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedIborFutureOptionTrade trade, RatesScenarioMarketData ratesMarketData, IborFutureOptionScenarioMarketData optionMarketData)
	  {

		IborIndex index = trade.Product.UnderlyingFuture.Index;
		return MultiCurrencyScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01MarketQuoteSum(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(index)));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01MarketQuoteSum(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		NormalIborFutureOptionVolatilities normalVols = checkNormalVols(volatilities);
		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivityRates(trade, ratesProvider, normalVols);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedIborFutureOptionTrade trade, RatesScenarioMarketData ratesMarketData, IborFutureOptionScenarioMarketData optionMarketData)
	  {

		IborIndex index = trade.Product.UnderlyingFuture.Index;
		return ScenarioArray.of(ratesMarketData.ScenarioCount, i => pv01MarketQuoteBucketed(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(index)));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		NormalIborFutureOptionVolatilities normalVols = checkNormalVols(volatilities);
		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivityRates(trade, ratesProvider, normalVols);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates unit price for all scenarios
	  internal DoubleScenarioArray unitPrice(ResolvedIborFutureOptionTrade trade, RatesScenarioMarketData ratesMarketData, IborFutureOptionScenarioMarketData optionMarketData)
	  {

		IborIndex index = trade.Product.UnderlyingFuture.Index;
		return DoubleScenarioArray.of(ratesMarketData.ScenarioCount, i => unitPrice(trade, ratesMarketData.scenario(i).ratesProvider(), optionMarketData.scenario(i).volatilities(index)));
	  }

	  // unit price for one scenario
	  internal double unitPrice(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		// mark to model
		NormalIborFutureOptionVolatilities normalVols = checkNormalVols(volatilities);
		return tradePricer.price(trade, ratesProvider, normalVols);
	  }

	  //-------------------------------------------------------------------------
	  // gets the settlement price
	  private double settlementPrice(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider)
	  {
		StandardId standardId = trade.Product.SecurityId.StandardId;
		QuoteId id = QuoteId.of(standardId, FieldName.SETTLEMENT_PRICE);
		return ratesProvider.data(id);
	  }

	  // ensure volatilities are Normal
	  private NormalIborFutureOptionVolatilities checkNormalVols(IborFutureOptionVolatilities volatilities)
	  {
		if (volatilities is NormalIborFutureOptionVolatilities)
		{
		  return (NormalIborFutureOptionVolatilities) volatilities;
		}
		throw new System.ArgumentException(Messages.format("Ibor future option only supports Normal volatilities, but was '{}'", volatilities.VolatilityType));
	  }

	}

}