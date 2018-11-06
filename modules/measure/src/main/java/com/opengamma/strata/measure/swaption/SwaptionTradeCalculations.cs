/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swaption
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SabrSwaptionTradePricer = com.opengamma.strata.pricer.swaption.SabrSwaptionTradePricer;
	using SwaptionVolatilities = com.opengamma.strata.pricer.swaption.SwaptionVolatilities;
	using VolatilitySwaptionTradePricer = com.opengamma.strata.pricer.swaption.VolatilitySwaptionTradePricer;
	using ResolvedSwaptionTrade = com.opengamma.strata.product.swaption.ResolvedSwaptionTrade;
	using SwaptionTrade = com.opengamma.strata.product.swaption.SwaptionTrade;

	/// <summary>
	/// Calculates pricing and risk measures for swaption trades.
	/// <para>
	/// This provides a high-level entry point for swaption pricing and risk measures.
	/// </para>
	/// <para>
	/// Each method takes a <seealso cref="ResolvedSwaptionTrade"/>, whereas application code will
	/// typically work with <seealso cref="SwaptionTrade"/>. Call
	/// <seealso cref="SwaptionTrade#resolve(com.opengamma.strata.basics.ReferenceData) SwaptionTrade::resolve(ReferenceData)"/>
	/// to convert {@code SwaptionTrade} to {@code ResolvedSwaptionTrade}.
	/// </para>
	/// </summary>
	public class SwaptionTradeCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly SwaptionTradeCalculations DEFAULT = new SwaptionTradeCalculations(VolatilitySwaptionTradePricer.DEFAULT, SabrSwaptionTradePricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedSwaptionTrade"/>.
	  /// </summary>
	  private readonly SwaptionMeasureCalculations calc;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// In most cases, applications should use the <seealso cref="#DEFAULT"/> instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedSwaptionTrade"/> </param>
	  /// <param name="sabrTradePricer">  the pricer for <seealso cref="ResolvedSwaptionTrade"/> SABR </param>
	  public SwaptionTradeCalculations(VolatilitySwaptionTradePricer tradePricer, SabrSwaptionTradePricer sabrTradePricer)
	  {
		this.calc = new SwaptionMeasureCalculations(tradePricer, sabrTradePricer);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="swaptionLookup">  the lookup used to query the swaption market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual CurrencyScenarioArray presentValue(ResolvedSwaptionTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
	  {

		return calc.presentValue(trade, ratesLookup.marketDataView(marketData), swaptionLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the swaption volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return calc.presentValue(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="swaptionLookup">  the lookup used to query the swaption market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01RatesCalibratedSum(ResolvedSwaptionTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01RatesCalibratedSum(trade, ratesLookup.marketDataView(marketData), swaptionLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01RatesCalibratedSum(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return calc.pv01RatesCalibratedSum(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="swaptionLookup">  the lookup used to query the swaption market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01RatesCalibratedBucketed(ResolvedSwaptionTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01RatesCalibratedBucketed(trade, ratesLookup.marketDataView(marketData), swaptionLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01RatesCalibratedBucketed(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return calc.pv01RatesCalibratedBucketed(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in
	  /// the market quotes used to calibrate the curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="swaptionLookup">  the lookup used to query the swaption market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01RatesMarketQuoteSum(ResolvedSwaptionTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01RatesMarketQuoteSum(trade, ratesLookup.marketDataView(marketData), swaptionLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in
	  /// the market quotes used to calibrate the curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01RatesMarketQuoteSum(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return calc.pv01RatesMarketQuoteSum(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in
	  /// the market quotes used to calibrate the curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="swaptionLookup">  the lookup used to query the swaption market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01RatesMarketQuoteBucketed(ResolvedSwaptionTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01RatesMarketQuoteBucketed(trade, ratesLookup.marketDataView(marketData), swaptionLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in
	  /// the market quotes used to calibrate the curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01RatesMarketQuoteBucketed(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return calc.pv01RatesMarketQuoteBucketed(trade, ratesProvider, volatilities);
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
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="swaptionLookup">  the lookup used to query the swaption market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the currency exposure, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray currencyExposure(ResolvedSwaptionTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
	  {

		return calc.currencyExposure(trade, ratesLookup.marketDataView(marketData), swaptionLookup.marketDataView(marketData));
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
	  /// <param name="volatilities">  the swaption volatilities </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return calc.currencyExposure(trade, ratesProvider, volatilities);
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
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="swaptionLookup">  the lookup used to query the swaption market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the current cash, one entry per scenario </returns>
	  public virtual CurrencyScenarioArray currentCash(ResolvedSwaptionTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
	  {

		return calc.currentCash(trade, ratesLookup.marketDataView(marketData), swaptionLookup.marketDataView(marketData));
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
	  /// <param name="volatilities">  the swaption volatilities </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return calc.currentCash(trade, ratesProvider.ValuationDate);
	  }

	}

}