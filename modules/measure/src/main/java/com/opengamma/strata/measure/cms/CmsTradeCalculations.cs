/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.cms
{
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;
	using SwaptionMarketDataLookup = com.opengamma.strata.measure.swaption.SwaptionMarketDataLookup;
	using SabrExtrapolationReplicationCmsTradePricer = com.opengamma.strata.pricer.cms.SabrExtrapolationReplicationCmsTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwaptionVolatilities = com.opengamma.strata.pricer.swaption.SwaptionVolatilities;
	using CmsTrade = com.opengamma.strata.product.cms.CmsTrade;
	using ResolvedCmsTrade = com.opengamma.strata.product.cms.ResolvedCmsTrade;

	/// <summary>
	/// Calculates pricing and risk measures for constant maturity swap (CMS) trades.
	/// <para>
	/// This provides a high-level entry point for CMS pricing and risk measures.
	/// CMS pricing uses swaption volatilities with the SABR model.
	/// Additional model parameters must be specified using <seealso cref="CmsSabrExtrapolationParams"/>.
	/// </para>
	/// <para>
	/// Each method takes a <seealso cref="ResolvedCmsTrade"/>, whereas application code will
	/// typically work with <seealso cref="CmsTrade"/>. Call
	/// <seealso cref="CmsTrade#resolve(com.opengamma.strata.basics.ReferenceData) CmsTrade::resolve(ReferenceData)"/>
	/// to convert {@code CmsTrade} to {@code ResolvedCmsTrade}.
	/// </para>
	/// </summary>
	public class CmsTradeCalculations
	{

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedCmsTrade"/>.
	  /// </summary>
	  private readonly CmsMeasureCalculations calc;

	  /// <summary>
	  /// Obtains an instance specifying the SABR extrapolation parameters.
	  /// </summary>
	  /// <param name="cmsParams">  the parameters for SABR pricing of CMS </param>
	  /// <returns> the trade calculations </returns>
	  public static CmsTradeCalculations of(CmsSabrExtrapolationParams cmsParams)
	  {
		return new CmsTradeCalculations(cmsParams);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance specifying the SABR extrapolation parameters.
	  /// </summary>
	  /// <param name="cmsParams">  the parameters for SABR pricing of CMS </param>
	  private CmsTradeCalculations(CmsSabrExtrapolationParams cmsParams)
	  {
		this.calc = new CmsMeasureCalculations(cmsParams);
	  }

	  /// <summary>
	  /// Creates an instance specifying the SABR pricer.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedCmsTrade"/> </param>
	  public CmsTradeCalculations(SabrExtrapolationReplicationCmsTradePricer tradePricer)
	  {
		this.calc = new CmsMeasureCalculations(tradePricer);
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
	  public virtual MultiCurrencyScenarioArray presentValue(ResolvedCmsTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
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
	  public virtual MultiCurrencyAmount presentValue(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
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
	  public virtual MultiCurrencyScenarioArray pv01RatesCalibratedSum(ResolvedCmsTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
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
	  public virtual MultiCurrencyAmount pv01RatesCalibratedSum(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
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
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01RatesCalibratedBucketed(ResolvedCmsTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
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
	  public virtual CurrencyParameterSensitivities pv01RatesCalibratedBucketed(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
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
	  public virtual MultiCurrencyScenarioArray pv01RatesMarketQuoteSum(ResolvedCmsTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
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
	  public virtual MultiCurrencyAmount pv01RatesMarketQuoteSum(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
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
	  public virtual ScenarioArray<CurrencyParameterSensitivities> pv01RatesMarketQuoteBucketed(ResolvedCmsTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
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
	  public virtual CurrencyParameterSensitivities pv01RatesMarketQuoteBucketed(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
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
	  public virtual MultiCurrencyScenarioArray currencyExposure(ResolvedCmsTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
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
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
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
	  public virtual MultiCurrencyScenarioArray currentCash(ResolvedCmsTrade trade, RatesMarketDataLookup ratesLookup, SwaptionMarketDataLookup swaptionLookup, ScenarioMarketData marketData)
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
	  public virtual MultiCurrencyAmount currentCash(ResolvedCmsTrade trade, RatesProvider ratesProvider, SwaptionVolatilities volatilities)
	  {

		return calc.currentCash(trade, ratesProvider, volatilities);
	  }

	}

}