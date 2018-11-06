/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using BlackBondFutureOptionMarginedTradePricer = com.opengamma.strata.pricer.bond.BlackBondFutureOptionMarginedTradePricer;
	using BlackBondFutureVolatilities = com.opengamma.strata.pricer.bond.BlackBondFutureVolatilities;
	using BondFutureVolatilities = com.opengamma.strata.pricer.bond.BondFutureVolatilities;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using ResolvedBondFutureOptionTrade = com.opengamma.strata.product.bond.ResolvedBondFutureOptionTrade;

	/// <summary>
	/// Multi-scenario measure calculations for Bond Future Option trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class BondFutureOptionMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BondFutureOptionMeasureCalculations DEFAULT = new BondFutureOptionMeasureCalculations(BlackBondFutureOptionMarginedTradePricer.DEFAULT);
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedBondFutureOptionTrade"/>.
	  /// </summary>
	  private readonly BlackBondFutureOptionMarginedTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedBondFutureOptionTrade"/> </param>
	  internal BondFutureOptionMeasureCalculations(BlackBondFutureOptionMarginedTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal CurrencyScenarioArray presentValue(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingScenarioMarketData legalEntityMarketData, BondFutureOptionScenarioMarketData optionMarketData)
	  {

		SecurityId securityId = trade.Product.UnderlyingFuture.SecurityId;
		return CurrencyScenarioArray.of(legalEntityMarketData.ScenarioCount, i => presentValue(trade, legalEntityMarketData.scenario(i).discountingProvider(), optionMarketData.scenario(i).volatilities(securityId)));
	  }

	  // present value for one scenario
	  internal CurrencyAmount presentValue(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		// mark to model
		double settlementPrice = this.settlementPrice(trade, discountingProvider);
		BlackBondFutureVolatilities normalVols = checkBlackVols(volatilities);
		return tradePricer.presentValue(trade, discountingProvider, normalVols, settlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingScenarioMarketData legalEntityMarketData, BondFutureOptionScenarioMarketData optionMarketData)
	  {

		SecurityId securityId = trade.Product.UnderlyingFuture.SecurityId;
		return MultiCurrencyScenarioArray.of(legalEntityMarketData.ScenarioCount, i => pv01CalibratedSum(trade, legalEntityMarketData.scenario(i).discountingProvider(), optionMarketData.scenario(i).volatilities(securityId)));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01CalibratedSum(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		BlackBondFutureVolatilities normalVols = checkBlackVols(volatilities);
		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivityRates(trade, discountingProvider, normalVols);
		return discountingProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingScenarioMarketData legalEntityMarketData, BondFutureOptionScenarioMarketData optionMarketData)
	  {

		SecurityId securityId = trade.Product.UnderlyingFuture.SecurityId;
		return ScenarioArray.of(legalEntityMarketData.ScenarioCount, i => pv01CalibratedBucketed(trade, legalEntityMarketData.scenario(i).discountingProvider(), optionMarketData.scenario(i).volatilities(securityId)));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		BlackBondFutureVolatilities normalVols = checkBlackVols(volatilities);
		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivityRates(trade, discountingProvider, normalVols);
		return discountingProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates unit price for all scenarios
	  internal DoubleScenarioArray unitPrice(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingScenarioMarketData legalEntityMarketData, BondFutureOptionScenarioMarketData optionMarketData)
	  {

		SecurityId securityId = trade.Product.UnderlyingFuture.SecurityId;
		return DoubleScenarioArray.of(legalEntityMarketData.ScenarioCount, i => unitPrice(trade, legalEntityMarketData.scenario(i).discountingProvider(), optionMarketData.scenario(i).volatilities(securityId)));
	  }

	  // unit price for one scenario
	  internal double unitPrice(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		// mark to model
		BlackBondFutureVolatilities normalVols = checkBlackVols(volatilities);
		return tradePricer.price(trade, discountingProvider, normalVols);
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingScenarioMarketData legalEntityMarketData, BondFutureOptionScenarioMarketData optionMarketData)
	  {

		SecurityId securityId = trade.Product.UnderlyingFuture.SecurityId;
		return MultiCurrencyScenarioArray.of(legalEntityMarketData.ScenarioCount, i => currencyExposure(trade, legalEntityMarketData.scenario(i).discountingProvider(), optionMarketData.scenario(i).volatilities(securityId)));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		double settlementPrice = this.settlementPrice(trade, discountingProvider);
		return tradePricer.currencyExposure(trade, discountingProvider, volatilities, settlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  // gets the settlement price
	  private double settlementPrice(ResolvedBondFutureOptionTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {
		StandardId standardId = trade.Product.SecurityId.StandardId;
		QuoteId id = QuoteId.of(standardId, FieldName.SETTLEMENT_PRICE);
		return discountingProvider.data(id);
	  }

	  // ensure volatilities are Black
	  private BlackBondFutureVolatilities checkBlackVols(BondFutureVolatilities volatilities)
	  {
		if (volatilities is BlackBondFutureVolatilities)
		{
		  return (BlackBondFutureVolatilities) volatilities;
		}
		throw new System.ArgumentException(Messages.format("Bond future option only supports Black volatilities, but was '{}'", volatilities.VolatilityType));
	  }

	}

}