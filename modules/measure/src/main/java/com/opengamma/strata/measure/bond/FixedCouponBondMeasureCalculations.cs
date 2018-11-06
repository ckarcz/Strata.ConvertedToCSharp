/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.bond
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DiscountingFixedCouponBondTradePricer = com.opengamma.strata.pricer.bond.DiscountingFixedCouponBondTradePricer;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using ResolvedFixedCouponBondTrade = com.opengamma.strata.product.bond.ResolvedFixedCouponBondTrade;

	/// <summary>
	/// Multi-scenario measure calculations for fixed coupon bond trades.
	/// <para>
	/// Each method corresponds to a measure, typically calculated by one or more calls to the pricer.
	/// </para>
	/// </summary>
	internal sealed class FixedCouponBondMeasureCalculations
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly FixedCouponBondMeasureCalculations DEFAULT = new FixedCouponBondMeasureCalculations(DiscountingFixedCouponBondTradePricer.DEFAULT);
	  /// <summary>
	  /// The market quote sensitivity calculator.
	  /// </summary>
	  private static readonly MarketQuoteSensitivityCalculator MARKET_QUOTE_SENS = MarketQuoteSensitivityCalculator.DEFAULT;
	  /// <summary>
	  /// One basis point, expressed as a {@code double}.
	  /// </summary>
	  private const double ONE_BASIS_POINT = 1e-4;

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFixedCouponBondTrade"/>.
	  /// </summary>
	  private readonly DiscountingFixedCouponBondTradePricer tradePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tradePricer">  the pricer for <seealso cref="ResolvedFixedCouponBondTrade"/> </param>
	  internal FixedCouponBondMeasureCalculations(DiscountingFixedCouponBondTradePricer tradePricer)
	  {
		this.tradePricer = ArgChecker.notNull(tradePricer, "tradePricer");
	  }

	  //-------------------------------------------------------------------------
	  // calculates present value for all scenarios
	  internal CurrencyScenarioArray presentValue(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => presentValue(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // present value for one scenario
	  internal CurrencyAmount presentValue(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		return tradePricer.presentValue(trade, discountingProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01CalibratedSum(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedSum(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // calibrated sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01CalibratedSum(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, discountingProvider);
		return discountingProvider.parameterSensitivity(pointSensitivity).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates calibrated bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01CalibratedBucketed(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01CalibratedBucketed(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // calibrated bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01CalibratedBucketed(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, discountingProvider);
		return discountingProvider.parameterSensitivity(pointSensitivity).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote sum PV01 for all scenarios
	  internal MultiCurrencyScenarioArray pv01MarketQuoteSum(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteSum(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // market quote sum PV01 for one scenario
	  internal MultiCurrencyAmount pv01MarketQuoteSum(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).total().multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates market quote bucketed PV01 for all scenarios
	  internal ScenarioArray<CurrencyParameterSensitivities> pv01MarketQuoteBucketed(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return ScenarioArray.of(marketData.ScenarioCount, i => pv01MarketQuoteBucketed(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // market quote bucketed PV01 for one scenario
	  internal CurrencyParameterSensitivities pv01MarketQuoteBucketed(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider ratesProvider)
	  {

		PointSensitivities pointSensitivity = tradePricer.presentValueSensitivity(trade, ratesProvider);
		CurrencyParameterSensitivities parameterSensitivity = ratesProvider.parameterSensitivity(pointSensitivity);
		return MARKET_QUOTE_SENS.sensitivity(parameterSensitivity, ratesProvider).multipliedBy(ONE_BASIS_POINT);
	  }

	  //-------------------------------------------------------------------------
	  // calculates currency exposure for all scenarios
	  internal MultiCurrencyScenarioArray currencyExposure(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return MultiCurrencyScenarioArray.of(marketData.ScenarioCount, i => currencyExposure(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // currency exposure for one scenario
	  internal MultiCurrencyAmount currencyExposure(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		return tradePricer.currencyExposure(trade, discountingProvider);
	  }

	  //-------------------------------------------------------------------------
	  // calculates current cash for all scenarios
	  internal CurrencyScenarioArray currentCash(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingScenarioMarketData marketData)
	  {

		return CurrencyScenarioArray.of(marketData.ScenarioCount, i => currentCash(trade, marketData.scenario(i).discountingProvider()));
	  }

	  // current cash for one scenario
	  internal CurrencyAmount currentCash(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider discountingProvider)
	  {

		return tradePricer.currentCash(trade, discountingProvider.ValuationDate);
	  }

	}

}