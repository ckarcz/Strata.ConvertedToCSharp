/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fx
{
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingFxSingleTradePricer = com.opengamma.strata.pricer.fx.DiscountingFxSingleTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using FxSingleTrade = com.opengamma.strata.product.fx.FxSingleTrade;
	using ResolvedFxSingleTrade = com.opengamma.strata.product.fx.ResolvedFxSingleTrade;

	/// <summary>
	/// Calculates pricing and risk measures for single FX trades.
	/// <para>
	/// This provides a high-level entry point for FX pricing and risk measures.
	/// </para>
	/// <para>
	/// Each method takes a <seealso cref="ResolvedFxSingleTrade"/>, whereas application code will
	/// typically work with <seealso cref="FxSingleTrade"/>. Call
	/// <seealso cref="FxSingleTrade#resolve(com.opengamma.strata.basics.ReferenceData) FxSingleTrade::resolve(ReferenceData)"/>
	/// to convert {@code FxSingleTrade} to {@code ResolvedFxSingleTrade}.
	/// </para>
	/// </summary>
	public class FxSingleTradeCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly FxSingleTradeCalculations DEFAULT = new FxSingleTradeCalculations(DiscountingFxSingleTradePricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxSingleTrade"/>.
	  /// </summary>
	  private readonly FxSingleMeasureCalculations calc;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// In most cases, applications should use the <seealso cref="#DEFAULT"/> instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedFxSingleTrade"/> </param>
	  public FxSingleTradeCalculations(DiscountingFxSingleTradePricer tradePricer)
	  {
		this.calc = new FxSingleMeasureCalculations(tradePricer);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray presentValue(ResolvedFxSingleTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.presentValue(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedFxSingleTrade trade, RatesProvider ratesProvider)
	  {

		return calc.presentValue(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedFxSingleTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedFxSingleTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedSum(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedFxSingleTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01CalibratedSum(ResolvedFxSingleTrade trade, RatesProvider ratesProvider)
	  {

		return calc.pv01CalibratedSum(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedFxSingleTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedFxSingleTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedBucketed(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedFxSingleTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedFxSingleTrade trade, RatesProvider ratesProvider)
	  {

		return calc.pv01CalibratedBucketed(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedFxSingleTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedFxSingleTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01MarketQuoteSum(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedFxSingleTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01MarketQuoteSum(ResolvedFxSingleTrade trade, RatesProvider ratesProvider)
	  {

		return calc.pv01MarketQuoteSum(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedFxSingleTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedFxSingleTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01MarketQuoteBucketed(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedFxSingleTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedFxSingleTrade trade, RatesProvider ratesProvider)
	  {

		return calc.pv01MarketQuoteBucketed(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates par spread across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the par spread, one entry per scenario </returns>
	  public virtual DoubleScenarioArray parSpread(ResolvedFxSingleTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.parSpread(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates par spread for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedFxSingleTrade trade, RatesProvider ratesProvider)
	  {

		return calc.parSpread(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates currency exposure across one or more scenarios.
	  /// <para>
	  /// The currency risk, expressed as the equivalent amount in each currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the currency exposure, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray currencyExposure(ResolvedFxSingleTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.currencyExposure(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates currency exposure for a single set of market data.
	  /// <para>
	  /// The currency risk, expressed as the equivalent amount in each currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxSingleTrade trade, RatesProvider ratesProvider)
	  {

		return calc.currencyExposure(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates current cash across one or more scenarios.
	  /// <para>
	  /// The sum of all cash flows paid on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the current cash, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray currentCash(ResolvedFxSingleTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.currentCash(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates current cash for a single set of market data.
	  /// <para>
	  /// The sum of all cash flows paid on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the current cash </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedFxSingleTrade trade, RatesProvider ratesProvider)
	  {

		return calc.currentCash(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forward FX rate across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the current cash, one entry per scenario </returns>
	  public virtual ScenarioArray<FxRate> forwardFxRate(ResolvedFxSingleTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.forwardFxRate(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates the forward FX rate for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the current cash </returns>
	  public virtual FxRate forwardFxRate(ResolvedFxSingleTrade trade, RatesProvider ratesProvider)
	  {

		return calc.forwardFxRate(trade, ratesProvider);
	  }

	}

}