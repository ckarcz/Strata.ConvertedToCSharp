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
	using IborFutureOptionVolatilities = com.opengamma.strata.pricer.index.IborFutureOptionVolatilities;
	using NormalIborFutureOptionMarginedTradePricer = com.opengamma.strata.pricer.index.NormalIborFutureOptionMarginedTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborFutureOptionTrade = com.opengamma.strata.product.index.IborFutureOptionTrade;
	using ResolvedIborFutureOptionTrade = com.opengamma.strata.product.index.ResolvedIborFutureOptionTrade;

	/// <summary>
	/// Calculates pricing and risk measures for trades in an option contract based on an Ibor index future.
	/// <para>
	/// This provides a high-level entry point for option pricing and risk measures.
	/// </para>
	/// <para>
	/// Each method takes a <seealso cref="ResolvedIborFutureOptionTrade"/>, whereas application code will
	/// typically work with <seealso cref="IborFutureOptionTrade"/>. Call
	/// <seealso cref="IborFutureOptionTrade#resolve(com.opengamma.strata.basics.ReferenceData) IborFutureOptionTrade::resolve(ReferenceData)"/>
	/// to convert {@code IborFutureOptionTrade} to {@code ResolvedIborFutureOptionTrade}.
	/// 
	/// <h4>Price</h4>
	/// The price of an Ibor future option is based on the price of the underlying future, the volatility
	/// and the time to expiry. The price of the at-the-money option tends to zero as expiry approaches.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Ibor future options in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, an option price of 0.2 is related to a futures price of 99.32 that implies an
	/// interest rate of 0.68%. Strata represents the price of the future as 0.9932 and thus
	/// represents the price of the option as 0.002.
	/// </para>
	/// </summary>
	public class IborFutureOptionTradeCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly IborFutureOptionTradeCalculations DEFAULT = new IborFutureOptionTradeCalculations(NormalIborFutureOptionMarginedTradePricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedIborFutureOptionTrade"/>.
	  /// </summary>
	  private readonly IborFutureOptionMeasureCalculations calc;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// In most cases, applications should use the <seealso cref="#DEFAULT"/> instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedIborFutureOptionTrade"/> </param>
	  public IborFutureOptionTradeCalculations(NormalIborFutureOptionMarginedTradePricer tradePricer)
	  {
		this.calc = new IborFutureOptionMeasureCalculations(tradePricer);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the rates market data </param>
	  /// <param name="optionLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual CurrencyScenarioArray presentValue(ResolvedIborFutureOptionTrade trade, RatesMarketDataLookup ratesLookup, IborFutureOptionMarketDataLookup optionLookup, ScenarioMarketData marketData)
	  {

		return calc.presentValue(trade, ratesLookup.marketDataView(marketData), optionLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		return calc.presentValue(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureOptionTrade, RatesMarketDataLookup, IborFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the rates market data </param>
	  /// <param name="optionLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedIborFutureOptionTrade trade, RatesMarketDataLookup ratesLookup, IborFutureOptionMarketDataLookup optionLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedSum(trade, ratesLookup.marketDataView(marketData), optionLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureOptionTrade, RatesMarketDataLookup, IborFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01CalibratedSum(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		return calc.pv01CalibratedSum(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureOptionTrade, RatesMarketDataLookup, IborFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the rates market data </param>
	  /// <param name="optionLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedIborFutureOptionTrade trade, RatesMarketDataLookup ratesLookup, IborFutureOptionMarketDataLookup optionLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedBucketed(trade, ratesLookup.marketDataView(marketData), optionLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureOptionTrade, RatesMarketDataLookup, IborFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		return calc.pv01CalibratedBucketed(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureOptionTrade, RatesMarketDataLookup, IborFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the rates market data </param>
	  /// <param name="optionLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedIborFutureOptionTrade trade, RatesMarketDataLookup ratesLookup, IborFutureOptionMarketDataLookup optionLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01MarketQuoteSum(trade, ratesLookup.marketDataView(marketData), optionLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureOptionTrade, RatesMarketDataLookup, IborFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01MarketQuoteSum(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		return calc.pv01MarketQuoteSum(trade, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureOptionTrade, RatesMarketDataLookup, IborFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the rates market data </param>
	  /// <param name="optionLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedIborFutureOptionTrade trade, RatesMarketDataLookup ratesLookup, IborFutureOptionMarketDataLookup optionLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01MarketQuoteBucketed(trade, ratesLookup.marketDataView(marketData), optionLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedIborFutureOptionTrade, RatesMarketDataLookup, IborFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		return calc.pv01MarketQuoteBucketed(trade, ratesProvider, volatilities);
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
	  /// <param name="ratesLookup">  the lookup used to query the rates market data </param>
	  /// <param name="optionLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual DoubleScenarioArray unitPrice(ResolvedIborFutureOptionTrade trade, RatesMarketDataLookup ratesLookup, IborFutureOptionMarketDataLookup optionLookup, ScenarioMarketData marketData)
	  {

		return calc.unitPrice(trade, ratesLookup.marketDataView(marketData), optionLookup.marketDataView(marketData));
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
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual double unitPrice(ResolvedIborFutureOptionTrade trade, RatesProvider ratesProvider, IborFutureOptionVolatilities volatilities)
	  {

		return calc.unitPrice(trade, ratesProvider, volatilities);
	  }

	}

}