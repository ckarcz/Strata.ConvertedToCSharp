/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using DiscountingCapitalIndexedBondTradePricer = com.opengamma.strata.pricer.bond.DiscountingCapitalIndexedBondTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using CapitalIndexedBondTrade = com.opengamma.strata.product.bond.CapitalIndexedBondTrade;
	using ResolvedCapitalIndexedBondTrade = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBondTrade;

	/// <summary>
	/// Calculates pricing and risk measures for forward rate agreement (capital indexed bond) trades.
	/// <para>
	/// This provides a high-level entry point for capital indexed bond pricing and risk measures.
	/// </para>
	/// <para>
	/// Each method takes a <seealso cref="ResolvedCapitalIndexedBondTrade"/>, whereas application code will
	/// typically work with <seealso cref="CapitalIndexedBondTrade"/>. Call
	/// <seealso cref="CapitalIndexedBondTrade#resolve(com.opengamma.strata.basics.ReferenceData) CapitalIndexedBondTrade::resolve(ReferenceData)"/>
	/// to convert {@code CapitalIndexedBondTrade} to {@code ResolvedCapitalIndexedBondTrade}.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
	public class CapitalIndexedBondTradeCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly CapitalIndexedBondTradeCalculations DEFAULT = new CapitalIndexedBondTradeCalculations(DiscountingCapitalIndexedBondTradePricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedCapitalIndexedBondTrade"/>.
	  /// </summary>
	  private readonly CapitalIndexedBondMeasureCalculations calc;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// In most cases, applications should use the <seealso cref="#DEFAULT"/> instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedCapitalIndexedBondTrade"/> </param>
	  public CapitalIndexedBondTradeCalculations(DiscountingCapitalIndexedBondTradePricer tradePricer)
	  {
		this.calc = new CapitalIndexedBondMeasureCalculations(tradePricer);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="legalEntityLookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual CurrencyScenarioArray presentValue(ResolvedCapitalIndexedBondTrade trade, RatesMarketDataLookup ratesLookup, LegalEntityDiscountingMarketDataLookup legalEntityLookup, ScenarioMarketData marketData)
	  {

		return calc.presentValue(trade, ratesLookup.marketDataView(marketData), legalEntityLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="legalEntityProvider">  the market data </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider legalEntityProvider)
	  {

		return calc.presentValue(trade, ratesProvider, legalEntityProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedCapitalIndexedBondTrade, RatesMarketDataLookup, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="legalEntityLookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedCapitalIndexedBondTrade trade, RatesMarketDataLookup ratesLookup, LegalEntityDiscountingMarketDataLookup legalEntityLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedSum(trade, ratesLookup.marketDataView(marketData), legalEntityLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedCapitalIndexedBondTrade, RatesMarketDataLookup, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="legalEntityProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01CalibratedSum(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider legalEntityProvider)
	  {

		return calc.pv01CalibratedSum(trade, ratesProvider, legalEntityProvider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedCapitalIndexedBondTrade, RatesMarketDataLookup, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesLookup">  the lookup used to query the market data </param>
	  /// <param name="legalEntityLookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedCapitalIndexedBondTrade trade, RatesMarketDataLookup ratesLookup, LegalEntityDiscountingMarketDataLookup legalEntityLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedBucketed(trade, ratesLookup.marketDataView(marketData), legalEntityLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedCapitalIndexedBondTrade, RatesMarketDataLookup, LegalEntityDiscountingMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the market data </param>
	  /// <param name="legalEntityProvider">  the market data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider legalEntityProvider)
	  {

		return calc.pv01CalibratedBucketed(trade, ratesProvider, legalEntityProvider);
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
	  /// <param name="legalEntityLookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the currency exposure, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray currencyExposure(ResolvedCapitalIndexedBondTrade trade, RatesMarketDataLookup ratesLookup, LegalEntityDiscountingMarketDataLookup legalEntityLookup, ScenarioMarketData marketData)
	  {

		return calc.currencyExposure(trade, ratesLookup.marketDataView(marketData), legalEntityLookup.marketDataView(marketData));
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
	  /// <param name="legalEntityProvider">  the market data </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider legalEntityProvider)
	  {

		return calc.currencyExposure(trade, ratesProvider, legalEntityProvider);
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
	  /// <param name="legalEntityLookup">  the lookup used to query the market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the current cash, one entry per scenario </returns>
	  public virtual CurrencyScenarioArray currentCash(ResolvedCapitalIndexedBondTrade trade, RatesMarketDataLookup ratesLookup, LegalEntityDiscountingMarketDataLookup legalEntityLookup, ScenarioMarketData marketData)
	  {

		return calc.currentCash(trade, ratesLookup.marketDataView(marketData), legalEntityLookup.marketDataView(marketData));
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
	  /// <param name="legalEntityProvider">  the market data </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider legalEntityProvider)
	  {

		return calc.currentCash(trade, ratesProvider);
	  }

	}

}