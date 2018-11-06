/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.capfloor
{
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using IborCapletFloorletVolatilities = com.opengamma.strata.pricer.capfloor.IborCapletFloorletVolatilities;
	using VolatilityIborCapFloorTradePricer = com.opengamma.strata.pricer.capfloor.VolatilityIborCapFloorTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborCapFloorTrade = com.opengamma.strata.product.capfloor.IborCapFloorTrade;
	using ResolvedIborCapFloorTrade = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorTrade;

	/// <summary>
	/// Calculates pricing and risk measures for cap/floor trades.
	/// <para>
	/// This provides a high-level entry point for cap/floor pricing and risk measures.
	/// </para>
	/// <para>
	/// Each method takes a <seealso cref="ResolvedIborCapFloorTrade"/>, whereas application code will
	/// typically work with <seealso cref="IborCapFloorTrade"/>. Call
	/// <seealso cref="IborCapFloorTrade#resolve(com.opengamma.strata.basics.ReferenceData) CapFloorTrade::resolve(ReferenceData)"/>
	/// to convert {@code CapFloorTrade} to {@code ResolvedIborCapFloorTrade}.
	/// </para>
	/// </summary>
	public class IborCapFloorTradeCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly IborCapFloorTradeCalculations DEFAULT = new IborCapFloorTradeCalculations(VolatilityIborCapFloorTradePricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedIborCapFloorTrade"/>.
	  /// </summary>
	  private readonly IborCapFloorMeasureCalculations calc;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// In most cases, applications should use the <seealso cref="#DEFAULT"/> instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedIborCapFloorTrade"/> </param>
	  public IborCapFloorTradeCalculations(VolatilityIborCapFloorTradePricer tradePricer)
	  {
		this.calc = new IborCapFloorMeasureCalculations(tradePricer);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="capFloorLookup">  the lookup used to query the cap/floor market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray presentValue(ResolvedIborCapFloorTrade trade, RatesMarketDataLookup ratesLookup, IborCapFloorMarketDataLookup capFloorLookup, ScenarioMarketData marketData)
	  {

		return calc.presentValue(trade, ratesLookup.marketDataView(marketData), capFloorLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the cap/floor volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
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
	  /// <param name="capFloorLookup">  the lookup used to query the cap/floor market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01RatesCalibratedSum(ResolvedIborCapFloorTrade trade, RatesMarketDataLookup ratesLookup, IborCapFloorMarketDataLookup capFloorLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01RatesCalibratedSum(trade, ratesLookup.marketDataView(marketData), capFloorLookup.marketDataView(marketData));
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
	  /// <param name="volatilities">  the cap/floor volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01RatesCalibratedSum(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
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
	  /// <param name="capFloorLookup">  the lookup used to query the cap/floor market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01RatesCalibratedBucketed(ResolvedIborCapFloorTrade trade, RatesMarketDataLookup ratesLookup, IborCapFloorMarketDataLookup capFloorLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01RatesCalibratedBucketed(trade, ratesLookup.marketDataView(marketData), capFloorLookup.marketDataView(marketData));
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
	  /// <param name="volatilities">  the cap/floor volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01RatesCalibratedBucketed(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
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
	  /// <param name="capFloorLookup">  the lookup used to query the cap/floor market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01RatesMarketQuoteSum(ResolvedIborCapFloorTrade trade, RatesMarketDataLookup ratesLookup, IborCapFloorMarketDataLookup capFloorLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01RatesMarketQuoteSum(trade, ratesLookup.marketDataView(marketData), capFloorLookup.marketDataView(marketData));
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
	  /// <param name="volatilities">  the cap/floor volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01RatesMarketQuoteSum(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
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
	  /// <param name="capFloorLookup">  the lookup used to query the cap/floor market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01RatesMarketQuoteBucketed(ResolvedIborCapFloorTrade trade, RatesMarketDataLookup ratesLookup, IborCapFloorMarketDataLookup capFloorLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01RatesMarketQuoteBucketed(trade, ratesLookup.marketDataView(marketData), capFloorLookup.marketDataView(marketData));
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
	  /// <param name="volatilities">  the cap/floor volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01RatesMarketQuoteBucketed(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
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
	  /// <param name="capFloorLookup">  the lookup used to query the cap/floor market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the currency exposure, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray currencyExposure(ResolvedIborCapFloorTrade trade, RatesMarketDataLookup ratesLookup, IborCapFloorMarketDataLookup capFloorLookup, ScenarioMarketData marketData)
	  {

		return calc.currencyExposure(trade, ratesLookup.marketDataView(marketData), capFloorLookup.marketDataView(marketData));
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
	  /// <param name="volatilities">  the cap/floor volatilities </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
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
	  /// <param name="capFloorLookup">  the lookup used to query the cap/floor market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the current cash, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray currentCash(ResolvedIborCapFloorTrade trade, RatesMarketDataLookup ratesLookup, IborCapFloorMarketDataLookup capFloorLookup, ScenarioMarketData marketData)
	  {

		return calc.currentCash(trade, ratesLookup.marketDataView(marketData), capFloorLookup.marketDataView(marketData));
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
	  /// <param name="volatilities">  the cap/floor volatilities </param>
	  /// <returns> the current cash </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		return calc.currentCash(trade, ratesProvider, volatilities);
	  }

	}

}