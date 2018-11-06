/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.index
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingIborFutureTradePricer = com.opengamma.strata.pricer.index.DiscountingIborFutureTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborFutureTrade = com.opengamma.strata.product.index.IborFutureTrade;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;

	/// <summary>
	/// Calculates pricing and risk measures for trades in a futures contract based on an Ibor index.
	/// <para>
	/// This provides a high-level entry point for future pricing and risk measures.
	/// </para>
	/// <para>
	/// Each method takes a <seealso cref="ResolvedIborFutureTrade"/>, whereas application code will
	/// typically work with <seealso cref="IborFutureTrade"/>. Call
	/// <seealso cref="IborFutureTrade#resolve(com.opengamma.strata.basics.ReferenceData) IborFutureTrade::resolve(ReferenceData)"/>
	/// to convert {@code IborFutureTrade} to {@code ResolvedIborFutureTrade}.
	/// 
	/// <h4>Price</h4>
	/// The price of an Ibor future is based on the interest rate of the underlying index.
	/// It is defined as {@code (100 - percentRate)}.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
	public class IborFutureTradeCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly IborFutureTradeCalculations DEFAULT = new IborFutureTradeCalculations(DiscountingIborFutureTradePricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedIborFutureTrade"/>.
	  /// </summary>
	  private readonly IborFutureMeasureCalculations calc;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// In most cases, applications should use the <seealso cref="#DEFAULT"/> instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedIborFutureTrade"/> </param>
	  public IborFutureTradeCalculations(DiscountingIborFutureTradePricer tradePricer)
	  {
		this.calc = new IborFutureMeasureCalculations(tradePricer);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual CurrencyScenarioArray presentValue(ResolvedIborFutureTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.presentValue(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedIborFutureTrade trade, RatesProvider ratesProvider)
	  {

		return calc.presentValue(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedIborFutureTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedSum(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01CalibratedSum(ResolvedIborFutureTrade trade, RatesProvider ratesProvider)
	  {

		return calc.pv01CalibratedSum(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedIborFutureTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedBucketed(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedIborFutureTrade trade, RatesProvider ratesProvider)
	  {

		return calc.pv01CalibratedBucketed(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedIborFutureTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01MarketQuoteSum(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01MarketQuoteSum(ResolvedIborFutureTrade trade, RatesProvider ratesProvider)
	  {

		return calc.pv01MarketQuoteSum(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedIborFutureTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01MarketQuoteBucketed(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureTrade, RatesMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedIborFutureTrade trade, RatesProvider ratesProvider)
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
	  public virtual DoubleScenarioArray parSpread(ResolvedIborFutureTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.parSpread(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates par spread for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedIborFutureTrade trade, RatesProvider ratesProvider)
	  {

		return calc.parSpread(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates unit price across one or more scenarios.
	  /// <para>
	  /// This is the price of a single unit of the security.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
	  /// The decimal price is based on the decimal rate equivalent to the percentage.
	  /// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual DoubleScenarioArray unitPrice(ResolvedIborFutureTrade trade, RatesMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.unitPrice(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates unit price for a single set of market data.
	  /// <para>
	  /// This is the price of a single unit of the security.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
	  /// The decimal price is based on the decimal rate equivalent to the percentage.
	  /// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value </returns>
	  public virtual double unitPrice(ResolvedIborFutureTrade trade, RatesProvider ratesProvider)
	  {

		return calc.unitPrice(trade, ratesProvider);
	  }

	}

}