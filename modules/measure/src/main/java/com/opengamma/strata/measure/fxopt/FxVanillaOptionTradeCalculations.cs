/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using BlackFxVanillaOptionTradePricer = com.opengamma.strata.pricer.fxopt.BlackFxVanillaOptionTradePricer;
	using FxOptionVolatilities = com.opengamma.strata.pricer.fxopt.FxOptionVolatilities;
	using VannaVolgaFxVanillaOptionTradePricer = com.opengamma.strata.pricer.fxopt.VannaVolgaFxVanillaOptionTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using FxVanillaOptionTrade = com.opengamma.strata.product.fxopt.FxVanillaOptionTrade;
	using ResolvedFxVanillaOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOptionTrade;

	/// <summary>
	/// Calculates pricing and risk measures for FX vanilla option trades.
	/// <para>
	/// This provides a high-level entry point for FX vanilla option pricing and risk measures.
	/// Pricing is performed using the Black method.
	/// </para>
	/// <para>
	/// Each method takes a <seealso cref="ResolvedFxVanillaOptionTrade"/>, whereas application code will
	/// typically work with <seealso cref="FxVanillaOptionTrade"/>. Call
	/// <seealso cref="FxVanillaOptionTrade#resolve(com.opengamma.strata.basics.ReferenceData) FxVanillaOptionTrade::resolve(ReferenceData)"/>
	/// to convert {@code FxVanillaOptionTrade} to {@code ResolvedFxVanillaOptionTrade}.
	/// </para>
	/// </summary>
	public class FxVanillaOptionTradeCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly FxVanillaOptionTradeCalculations DEFAULT = new FxVanillaOptionTradeCalculations(BlackFxVanillaOptionTradePricer.DEFAULT, VannaVolgaFxVanillaOptionTradePricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxVanillaOptionTrade"/>.
	  /// </summary>
	  private readonly FxVanillaOptionMeasureCalculations calc;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// In most cases, applications should use the <seealso cref="#DEFAULT"/> instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="blackPricer">  the pricer for <seealso cref="ResolvedFxVanillaOptionTrade"/> using Black </param>
	  /// <param name="vannaVolgaPricer">  the pricer for <seealso cref="ResolvedFxVanillaOptionTrade"/> using Vanna-Volga </param>
	  public FxVanillaOptionTradeCalculations(BlackFxVanillaOptionTradePricer blackPricer, VannaVolgaFxVanillaOptionTradePricer vannaVolgaPricer)
	  {
		this.calc = new FxVanillaOptionMeasureCalculations(blackPricer, vannaVolgaPricer);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="fxLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray presentValue(ResolvedFxVanillaOptionTrade trade, RatesMarketDataLookup ratesLookup, FxOptionMarketDataLookup fxLookup, ScenarioMarketData marketData, FxVanillaOptionMethod method)
	  {

		return calc.presentValue(trade, ratesLookup.marketDataView(marketData), fxLookup.marketDataView(marketData), method);
	  }

	  /// <summary>
	  /// Calculates present value for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the present value </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		return calc.presentValue(trade, ratesProvider, volatilities, method);
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
	  /// <param name="fxLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01RatesCalibratedSum(ResolvedFxVanillaOptionTrade trade, RatesMarketDataLookup ratesLookup, FxOptionMarketDataLookup fxLookup, ScenarioMarketData marketData, FxVanillaOptionMethod method)
	  {

		return calc.pv01RatesCalibratedSum(trade, ratesLookup.marketDataView(marketData), fxLookup.marketDataView(marketData), method);
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
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01RatesCalibratedSum(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		return calc.pv01RatesCalibratedSum(trade, ratesProvider, volatilities, method);
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
	  /// <param name="fxLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01RatesCalibratedBucketed(ResolvedFxVanillaOptionTrade trade, RatesMarketDataLookup ratesLookup, FxOptionMarketDataLookup fxLookup, ScenarioMarketData marketData, FxVanillaOptionMethod method)
	  {

		return calc.pv01RatesCalibratedBucketed(trade, ratesLookup.marketDataView(marketData), fxLookup.marketDataView(marketData), method);
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
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01RatesCalibratedBucketed(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		return calc.pv01RatesCalibratedBucketed(trade, ratesProvider, volatilities, method);
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
	  /// <param name="fxLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01RatesMarketQuoteSum(ResolvedFxVanillaOptionTrade trade, RatesMarketDataLookup ratesLookup, FxOptionMarketDataLookup fxLookup, ScenarioMarketData marketData, FxVanillaOptionMethod method)
	  {

		return calc.pv01RatesMarketQuoteSum(trade, ratesLookup.marketDataView(marketData), fxLookup.marketDataView(marketData), method);
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
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01RatesMarketQuoteSum(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		return calc.pv01RatesMarketQuoteSum(trade, ratesProvider, volatilities, method);
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
	  /// <param name="fxLookup">  the lookup used to query the option market data </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01RatesMarketQuoteBucketed(ResolvedFxVanillaOptionTrade trade, RatesMarketDataLookup ratesLookup, FxOptionMarketDataLookup fxLookup, ScenarioMarketData marketData, FxVanillaOptionMethod method)
	  {

		return calc.pv01RatesMarketQuoteBucketed(trade, ratesLookup.marketDataView(marketData), fxLookup.marketDataView(marketData), method);
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
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01RatesMarketQuoteBucketed(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		return calc.pv01RatesMarketQuoteBucketed(trade, ratesProvider, volatilities, method);
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
	  /// <param name="fxLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the currency exposure, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray currencyExposure(ResolvedFxVanillaOptionTrade trade, RatesMarketDataLookup ratesLookup, FxOptionMarketDataLookup fxLookup, ScenarioMarketData marketData, FxVanillaOptionMethod method)
	  {

		return calc.currencyExposure(trade, ratesLookup.marketDataView(marketData), fxLookup.marketDataView(marketData), method);
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
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		return calc.currencyExposure(trade, ratesProvider, volatilities, method);
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
	  /// <param name="fxLookup">  the lookup used to query the option market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the current cash, one entry per scenario </returns>
	  public virtual CurrencyScenarioArray currentCash(ResolvedFxVanillaOptionTrade trade, RatesMarketDataLookup ratesLookup, FxOptionMarketDataLookup fxLookup, ScenarioMarketData marketData, FxVanillaOptionMethod method)
	  {

		return calc.currentCash(trade, ratesLookup.marketDataView(marketData), fxLookup.marketDataView(marketData), method);
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
	  /// <param name="volatilities">  the option volatilities </param>
	  /// <param name="method">  the pricing method </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(ResolvedFxVanillaOptionTrade trade, RatesProvider ratesProvider, FxOptionVolatilities volatilities, FxVanillaOptionMethod method)
	  {

		return calc.currentCash(trade, ratesProvider.ValuationDate, method);
	  }

	}

}