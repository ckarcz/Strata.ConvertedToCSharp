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
	using FieldName = com.opengamma.strata.data.FieldName;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DiscountingBondFutureTradePricer = com.opengamma.strata.pricer.bond.DiscountingBondFutureTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedBondFutureTrade = com.opengamma.strata.product.bond.ResolvedBondFutureTrade;

	/// <summary>
	/// Multi-scenario measure calculations for Bond Future trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class BondFutureMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BondFutureMeasureCalculations DEFAULT = new BondFutureMeasureCalculations(DiscountingBondFutureTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedBondFutureTrade"/>.
	  /// </summary>
	  private readonly DiscountingBondFutureTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedBondFutureTrade"/> </param>
	  internal BondFutureMeasureCalculations(DiscountingBondFutureTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal CurrencyScenarioArray presentValue(ResolvedBondFutureTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => presentValue(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // present value for one scenario
	  internal CurrencyAmount presentValue(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		// mark to model
		double settlementPrice = this.settlementPrice(trade, discountingProvider);
		return tradePricer.presentValue(trade, discountingProvider, settlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedBondFutureTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedSum(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01CalibratedSum(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, discountingProvider);
		return discountingProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedBondFutureTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedBucketed(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, discountingProvider);
		return discountingProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedBondFutureTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteSum(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01MarketQuoteSum(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedBondFutureTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteBucketed(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates par spread for all scenarios
	  internal DoubleScenarioArray parSpread(ResolvedBondFutureTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return DoubleScenarioArray.of(marketData.ScenarioCount, i => parSpread(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // par spread for one scenario
	  internal double parSpread(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		double settlementPrice = this.settlementPrice(trade, discountingProvider);
		return tradePricer.parSpread(trade, discountingProvider, settlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  // calculates unit price for all scenarios
	  internal DoubleScenarioArray unitPrice(ResolvedBondFutureTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return DoubleScenarioArray.of(marketData.ScenarioCount, i => unitPrice(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // unit price for one scenario
	  internal double unitPrice(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		// mark to model
		return tradePricer.price(trade, discountingProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedBondFutureTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => currencyExposure(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		double settlementPrice = this.settlementPrice(trade, discountingProvider);
		return tradePricer.currencyExposure(trade, discountingProvider, settlementPrice);
	  }

	  //-------------------------------------------------------------------------
	  // gets the settlement price
	  private double settlementPrice(ResolvedBondFutureTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {
		StandardId standardId = trade.Product.SecurityId.StandardId;
		QuoteId id = QuoteId.of(standardId, FieldName.SETTLEMENT_PRICE);
		return discountingProvider.data(id) / 100; // convert market quote to value needed
	  }

	}

}