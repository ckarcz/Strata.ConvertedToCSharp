/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.security
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MarketData = com.opengamma.strata.data.MarketData;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using Security = com.opengamma.strata.product.Security;

	/// <summary>
	/// Multi-scenario measure calculations for simple security trades and positions.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class SecurityMeasureCalculations
	{

	  // restricted constructor
	  private SecurityMeasureCalculations()
	  {
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal static CurrencyScenarioArray presentValue(Security security, double quantity, ScenarioMarketData marketData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => calculatePresentValue(security, quantity, marketData.scenario(i)));
	  }

	  //-------------------------------------------------------------------------
	  // present value for one scenario
	  private static CurrencyAmount calculatePresentValue(Security security, double quantity, MarketData marketData)
	  {

		QuoteId id = QuoteId.of(security.SecurityId.StandardId);
		double price = marketData.getValue(id);
		return security.Info.PriceInfo.calculateMonetaryAmount(quantity, price);
	  }

	}

}