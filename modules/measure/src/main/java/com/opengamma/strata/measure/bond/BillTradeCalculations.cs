/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using DiscountingBillTradePricer = com.opengamma.strata.pricer.bond.DiscountingBillTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using BillTrade = com.opengamma.strata.product.bond.BillTrade;
	using ResolvedBillTrade = com.opengamma.strata.product.bond.ResolvedBillTrade;

	/// <summary>
	/// Calculates pricing and risk measures for bill trades.
	/// <para>
	/// This provides a high-level entry point for bill pricing and risk measures.
	/// </para>
	/// <para>
	/// Each method takes a <seealso cref="ResolvedBillTrade"/>, whereas application code will
	/// typically work with <seealso cref="BillTrade"/>. Call
	/// <seealso cref="BillTrade#resolve(com.opengamma.strata.basics.ReferenceData) BillTrade::resolve(ReferenceData)"/>
	/// to convert {@code BillTrade} to {@code ResolvedBillTrade}.
	/// 
	/// <h4>Price and yield</h4>
	/// Strata uses <i>decimal</i> yields and prices for bills in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932 and a yield of 1.32% is represented by 0.0132.
	/// </para>
	/// </summary>
	public class BillTradeCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BillTradeCalculations DEFAULT = new BillTradeCalculations(DiscountingBillTradePricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedBillTrade"/>.
	  /// </summary>
	  private readonly BillMeasureCalculations calc;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// In most cases, applications should use the <seealso cref="#DEFAULT"/> instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedBillTrade"/> </param>
	  public BillTradeCalculations(DiscountingBillTradePricer tradePricer)
	  {
		this.calc = new BillMeasureCalculations(tradePricer);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual CurrencyScenarioArray presentValue(ResolvedBillTrade trade, LegalEntityDiscountingMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.presentValue(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedBillTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		return calc.presentValue(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBillTrade, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedBillTrade trade, LegalEntityDiscountingMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedSum(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBillTrade, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01CalibratedSum(ResolvedBillTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		return calc.pv01CalibratedSum(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBillTrade, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedBillTrade trade, LegalEntityDiscountingMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedBucketed(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBillTrade, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedBillTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		return calc.pv01CalibratedBucketed(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBillTrade, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedBillTrade trade, LegalEntityDiscountingMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01MarketQuoteSum(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBillTrade, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01MarketQuoteSum(ResolvedBillTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		return calc.pv01MarketQuoteSum(trade, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBillTrade, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="lookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedBillTrade trade, LegalEntityDiscountingMarketDataLookup lookup, ScenarioMarketData marketData)
	  {

		return calc.pv01MarketQuoteBucketed(trade, lookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBillTrade, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the market quotes used to calibrate the curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedBillTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		return calc.pv01MarketQuoteBucketed(trade, ratesProvider);
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
	  public virtual MultiCurrencyScenarioArray currencyExposure(ResolvedBillTrade trade, LegalEntityDiscountingMarketDataLookup lookup, ScenarioMarketData marketData)
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
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedBillTrade trade, LegalEntityDiscountingProvider ratesProvider)
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
	  public virtual CurrencyScenarioArray currentCash(ResolvedBillTrade trade, LegalEntityDiscountingMarketDataLookup lookup, ScenarioMarketData marketData)
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
	  public virtual CurrencyAmount currentCash(ResolvedBillTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		return calc.currentCash(trade, ratesProvider);
	  }

	}

}