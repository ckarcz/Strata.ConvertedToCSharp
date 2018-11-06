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
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using BlackBondFutureOptionMarginedTradePricer = com.opengamma.strata.pricer.bond.BlackBondFutureOptionMarginedTradePricer;
	using BondFutureVolatilities = com.opengamma.strata.pricer.bond.BondFutureVolatilities;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using BondFuture = com.opengamma.strata.product.bond.BondFuture;
	using BondFutureOptionTrade = com.opengamma.strata.product.bond.BondFutureOptionTrade;
	using ResolvedBondFutureOptionTrade = com.opengamma.strata.product.bond.ResolvedBondFutureOptionTrade;

	/// <summary>
	/// Calculates pricing and risk measures for trades in an option contract based on an bond future.
	/// <para>
	/// This provides a high-level entry point for option pricing and risk measures.
	/// </para>
	/// <para>
	/// Each method takes a <seealso cref="ResolvedBondFutureOptionTrade"/>, whereas application code will
	/// typically work with <seealso cref="BondFutureOptionTrade"/>. Call
	/// <seealso cref="BondFutureOptionTrade#resolve(com.opengamma.strata.basics.ReferenceData) BondFutureOptionTrade::resolve(ReferenceData)"/>
	/// to convert {@code BondFutureOptionTrade} to {@code ResolvedBondFutureOptionTrade}.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bond futures options in the trade model, pricers and market data.
	/// This is coherent with the pricing of <seealso cref="BondFuture"/>.
	/// </para>
	/// </summary>
	public class BondFutureOptionTradeCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BondFutureOptionTradeCalculations DEFAULT = new BondFutureOptionTradeCalculations(BlackBondFutureOptionMarginedTradePricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedBondFutureOptionTrade"/>.
	  /// </summary>
	  private readonly BondFutureOptionMeasureCalculations calc;

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// In most cases, applications should use the <seealso cref="#DEFAULT"/> instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedBondFutureOptionTrade"/> </param>
	  public BondFutureOptionTradeCalculations(BlackBondFutureOptionMarginedTradePricer tradePricer)
	  {
		this.calc = new BondFutureOptionMeasureCalculations(tradePricer);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value across one or more scenarios.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="legalEntityLookup">  the lookup used to query the rates market data </param>
	  /// <param name="volsLookup">  the lookup used to query the volatility market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual CurrencyScenarioArray presentValue(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingMarketDataLookup legalEntityLookup, BondFutureOptionMarketDataLookup volsLookup, ScenarioMarketData marketData)
	  {

		return calc.presentValue(trade, legalEntityLookup.marketDataView(marketData), volsLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value for a single set of market data.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the market data </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		return calc.presentValue(trade, discountingProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBondFutureOptionTrade, LegalEntityDiscountingMarketDataLookup, BondFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="legalEntityLookup">  the lookup used to query the rates market data </param>
	  /// <param name="volsLookup">  the lookup used to query the volatility market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingMarketDataLookup legalEntityLookup, BondFutureOptionMarketDataLookup volsLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedSum(trade, legalEntityLookup.marketDataView(marketData), volsLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBondFutureOptionTrade, LegalEntityDiscountingMarketDataLookup, BondFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the market data </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual MultiCurrencyAmount pv01CalibratedSum(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		return calc.pv01CalibratedSum(trade, discountingProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates present value sensitivity across one or more scenarios.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBondFutureOptionTrade, LegalEntityDiscountingMarketDataLookup, BondFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="legalEntityLookup">  the lookup used to query the rates market data </param>
	  /// <param name="volsLookup">  the lookup used to query the volatility market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value sensitivity, one entry per scenario </returns>
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingMarketDataLookup legalEntityLookup, BondFutureOptionMarketDataLookup volsLookup, ScenarioMarketData marketData)
	  {

		return calc.pv01CalibratedBucketed(trade, legalEntityLookup.marketDataView(marketData), volsLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates present value sensitivity for a single set of market data.
	  /// <para>
	  /// This is the sensitivity of
	  /// <seealso cref="#presentValue(ResolvedBondFutureOptionTrade, LegalEntityDiscountingMarketDataLookup, BondFutureOptionMarketDataLookup, ScenarioMarketData) present value"/>
	  /// to a one basis point shift in the calibrated curves.
	  /// The result is provided for each affected curve and currency, bucketed by curve node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the market data </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		return calc.pv01CalibratedBucketed(trade, discountingProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates unit price across one or more scenarios.
	  /// <para>
	  /// This is the price of a single unit of the security.
	  /// 
	  /// <h4>Price</h4>
	  /// Strata uses <i>decimal prices</i> for bond futures options in the trade model, pricers and market data.
	  /// This is coherent with the pricing of <seealso cref="BondFuture"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="legalEntityLookup">  the lookup used to query the rates market data </param>
	  /// <param name="volsLookup">  the lookup used to query the volatility market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the present value, one entry per scenario </returns>
	  public virtual DoubleScenarioArray unitPrice(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingMarketDataLookup legalEntityLookup, BondFutureOptionMarketDataLookup volsLookup, ScenarioMarketData marketData)
	  {

		return calc.unitPrice(trade, legalEntityLookup.marketDataView(marketData), volsLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates unit price for a single set of market data.
	  /// <para>
	  /// This is the price of a single unit of the security.
	  /// 
	  /// <h4>Price</h4>
	  /// Strata uses <i>decimal prices</i> for bond futures options in the trade model, pricers and market data.
	  /// This is coherent with the pricing of <seealso cref="BondFuture"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the market data </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual double unitPrice(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		return calc.unitPrice(trade, discountingProvider, volatilities);
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
	  /// <param name="legalEntityLookup">  the lookup used to query the rates market data </param>
	  /// <param name="volsLookup">  the lookup used to query the volatility market data </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the currency exposure, one entry per scenario </returns>
	  public virtual MultiCurrencyScenarioArray currencyExposure(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingMarketDataLookup legalEntityLookup, BondFutureOptionMarketDataLookup volsLookup, ScenarioMarketData marketData)
	  {

		return calc.currencyExposure(trade, legalEntityLookup.marketDataView(marketData), volsLookup.marketDataView(marketData));
	  }

	  /// <summary>
	  /// Calculates currency exposure for a single set of market data.
	  /// <para>
	  /// The currency risk, expressed as the equivalent amount in each currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="discountingProvider">  the market data </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		return calc.currencyExposure(trade, discountingProvider, volatilities);
	  }

	}

}