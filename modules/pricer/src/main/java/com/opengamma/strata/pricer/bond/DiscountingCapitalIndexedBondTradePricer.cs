/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using BondPaymentPeriod = com.opengamma.strata.product.bond.BondPaymentPeriod;
	using CapitalIndexedBondPaymentPeriod = com.opengamma.strata.product.bond.CapitalIndexedBondPaymentPeriod;
	using CapitalIndexedBondYieldConvention = com.opengamma.strata.product.bond.CapitalIndexedBondYieldConvention;
	using KnownAmountBondPaymentPeriod = com.opengamma.strata.product.bond.KnownAmountBondPaymentPeriod;
	using ResolvedCapitalIndexedBond = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBond;
	using ResolvedCapitalIndexedBondSettlement = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBondSettlement;
	using ResolvedCapitalIndexedBondTrade = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBondTrade;

	/// <summary>
	/// Pricer for for capital index bond trades.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedCapitalIndexedBondTrade"/>.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
	public class DiscountingCapitalIndexedBondTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingCapitalIndexedBondTradePricer DEFAULT = new DiscountingCapitalIndexedBondTradePricer(DiscountingCapitalIndexedBondProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedCapitalIndexedBond"/>.
	  /// </summary>
	  private readonly DiscountingCapitalIndexedBondProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  pricer for <seealso cref="ResolvedCapitalIndexedBond"/> </param>
	  public DiscountingCapitalIndexedBondTradePricer(DiscountingCapitalIndexedBondProductPricer productPricer)
	  {

		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the bond trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// The result is expressed using the payment currency of the bond.
	  /// </para>
	  /// <para>
	  /// Coupon payments of the underlying product are considered based on the settlement date of the trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <returns> the present value of the bond trade </returns>
	  public virtual CurrencyAmount presentValue(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = this.settlementDate(trade, ratesProvider.ValuationDate);
		CurrencyAmount pvProduct = productPricer.presentValue(trade.Product, ratesProvider, discountingProvider, settlementDate);
		return presentValueFromProductPresentValue(trade, ratesProvider, discountingProvider, pvProduct);
	  }

	  /// <summary>
	  /// Calculates the present value of the bond trade with z-spread.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// The result is expressed using the payment currency of the bond.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// </para>
	  /// <para>
	  /// Coupon payments of the underlying product are considered based on the settlement date of the trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value of the bond trade </returns>
	  public virtual CurrencyAmount presentValueWithZSpread(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = this.settlementDate(trade, ratesProvider.ValuationDate);
		CurrencyAmount pvProduct = productPricer.presentValueWithZSpread(trade.Product, ratesProvider, discountingProvider, settlementDate, zSpread, compoundedRateType, periodsPerYear);
		return presentValueFromProductPresentValue(trade, ratesProvider, discountingProvider, pvProduct);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the bond trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// Coupon payments of the underlying product are considered based on the settlement date of the trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <returns> the present value sensitivity of the bond trade </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = this.settlementDate(trade, ratesProvider.ValuationDate);
		PointSensitivityBuilder productSensi = productPricer.presentValueSensitivity(trade.Product, ratesProvider, discountingProvider, settlementDate);
		return presentValueSensitivityFromProductPresentValueSensitivity(trade, ratesProvider, discountingProvider, productSensi).build();
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the bond trade with z-spread.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// Coupon payments of the underlying product are considered based on the settlement date of the trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value sensitivity of the bond trade </returns>
	  public virtual PointSensitivities presentValueSensitivityWithZSpread(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = this.settlementDate(trade, ratesProvider.ValuationDate);
		PointSensitivityBuilder productSensi = productPricer.presentValueSensitivityWithZSpread(trade.Product, ratesProvider, discountingProvider, settlementDate, zSpread, compoundedRateType, periodsPerYear);
		return presentValueSensitivityFromProductPresentValueSensitivity(trade, ratesProvider, discountingProvider, productSensi).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the bond trade from the clean price.
	  /// <para>
	  /// Since the sign of the settlement notional is opposite to that of the product, negative amount will be returned 
	  /// for positive quantity of trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="cleanRealPrice">  the clean real price </param>
	  /// <returns> the present value of the settlement </returns>
	  public virtual CurrencyAmount presentValueFromCleanPrice(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData, double cleanRealPrice)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate valuationDate = ratesProvider.ValuationDate;
		ResolvedCapitalIndexedBond bond = trade.Product;
		LocalDate standardSettlementDate = bond.calculateSettlementDateFromValuation(valuationDate, refData);
		LocalDate tradeSettlementDate = settlementDate(trade, valuationDate);
		RepoCurveDiscountFactors repoDf = DiscountingCapitalIndexedBondProductPricer.repoCurveDf(bond, discountingProvider);
		double df = repoDf.discountFactor(standardSettlementDate);
		CurrencyAmount pvStandard = forecastValueStandardFromCleanPrice(bond, ratesProvider, standardSettlementDate, cleanRealPrice).multipliedBy(df);
		if (standardSettlementDate.isEqual(tradeSettlementDate))
		{
		  return presentValueFromProductPresentValue(trade, ratesProvider, discountingProvider, pvStandard);
		}
		// check coupon payment between two settlement dates
		IssuerCurveDiscountFactors issuerDf = DiscountingCapitalIndexedBondProductPricer.issuerCurveDf(bond, discountingProvider);
		double pvDiff = 0d;
		if (standardSettlementDate.isAfter(tradeSettlementDate))
		{
		  pvDiff = -productPricer.presentValueCoupon(bond, ratesProvider, issuerDf, tradeSettlementDate, standardSettlementDate);
		}
		else
		{
		  pvDiff = productPricer.presentValueCoupon(bond, ratesProvider, issuerDf, standardSettlementDate, tradeSettlementDate);
		}
		return presentValueFromProductPresentValue(trade, ratesProvider, discountingProvider, pvStandard.plus(pvDiff));
	  }

	  /// <summary>
	  /// Calculates the present value of the settlement of the bond trade from the clean price with z-spread.
	  /// <para>
	  /// Since the sign of the settlement notional is opposite to that of the product, negative amount will be returned 
	  /// for positive quantity of trade.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <param name="cleanRealPrice">  the clean real price </param>
	  /// <returns> the present value of the settlement </returns>
	  public virtual CurrencyAmount presentValueFromCleanPriceWithZSpread(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData, double cleanRealPrice, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate valuationDate = ratesProvider.ValuationDate;
		ResolvedCapitalIndexedBond bond = trade.Product;
		LocalDate standardSettlementDate = bond.calculateSettlementDateFromValuation(valuationDate, refData);
		LocalDate tradeSettlementDate = settlementDate(trade, valuationDate);
		RepoCurveDiscountFactors repoDf = DiscountingCapitalIndexedBondProductPricer.repoCurveDf(bond, discountingProvider);
		double df = repoDf.discountFactor(standardSettlementDate);
		CurrencyAmount pvStandard = forecastValueStandardFromCleanPrice(bond, ratesProvider, standardSettlementDate, cleanRealPrice).multipliedBy(df);
		if (standardSettlementDate.isEqual(tradeSettlementDate))
		{
		  return presentValueFromProductPresentValue(trade, ratesProvider, discountingProvider, pvStandard);
		}
		// check coupon payment between two settlement dates
		IssuerCurveDiscountFactors issuerDf = DiscountingCapitalIndexedBondProductPricer.issuerCurveDf(bond, discountingProvider);
		double pvDiff = 0d;
		if (standardSettlementDate.isAfter(tradeSettlementDate))
		{
		  pvDiff = -productPricer.presentValueCouponWithZSpread(bond, ratesProvider, issuerDf, tradeSettlementDate, standardSettlementDate, zSpread, compoundedRateType, periodsPerYear);
		}
		else
		{
		  pvDiff = productPricer.presentValueCouponWithZSpread(bond, ratesProvider, issuerDf, standardSettlementDate, tradeSettlementDate, zSpread, compoundedRateType, periodsPerYear);
		}
		return presentValueFromProductPresentValue(trade, ratesProvider, discountingProvider, pvStandard.plus(pvDiff));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the settlement of the bond trade from the real clean price.
	  /// <para>
	  /// The present value sensitivity of the settlement is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="cleanRealPrice">  the clean real price </param>
	  /// <returns> the present value sensitivity of the settlement </returns>
	  public virtual PointSensitivities presentValueSensitivityFromCleanPrice(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData, double cleanRealPrice)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate valuationDate = ratesProvider.ValuationDate;
		ResolvedCapitalIndexedBond bond = trade.Product;
		LocalDate standardSettlementDate = bond.calculateSettlementDateFromValuation(valuationDate, refData);
		LocalDate tradeSettlementDate = settlementDate(trade, valuationDate);
		RepoCurveDiscountFactors repoDf = DiscountingCapitalIndexedBondProductPricer.repoCurveDf(bond, discountingProvider);
		double df = repoDf.discountFactor(standardSettlementDate);
		PointSensitivityBuilder dfSensi = repoDf.zeroRatePointSensitivity(standardSettlementDate);
		PointSensitivityBuilder pvSensiStandard = forecastValueSensitivityStandardFromCleanPrice(bond, ratesProvider, standardSettlementDate, cleanRealPrice).multipliedBy(df).combinedWith(dfSensi.multipliedBy(forecastValueStandardFromCleanPrice(bond, ratesProvider, standardSettlementDate, cleanRealPrice).Amount));
		if (standardSettlementDate.isEqual(tradeSettlementDate))
		{
		  return presentValueSensitivityFromProductPresentValueSensitivity(trade, ratesProvider, discountingProvider, pvSensiStandard).build();
		}
		// check coupon payment between two settlement dates
		IssuerCurveDiscountFactors issuerDf = DiscountingCapitalIndexedBondProductPricer.issuerCurveDf(bond, discountingProvider);
		PointSensitivityBuilder pvSensiDiff = PointSensitivityBuilder.none();
		if (standardSettlementDate.isAfter(tradeSettlementDate))
		{
		  pvSensiDiff = pvSensiDiff.combinedWith(productPricer.presentValueSensitivityCoupon(bond, ratesProvider, issuerDf, tradeSettlementDate, standardSettlementDate).multipliedBy(-1d));
		}
		else
		{
		  pvSensiDiff = pvSensiDiff.combinedWith(productPricer.presentValueSensitivityCoupon(bond, ratesProvider, issuerDf, standardSettlementDate, tradeSettlementDate));
		}
		return presentValueSensitivityFromProductPresentValueSensitivity(trade, ratesProvider, discountingProvider, pvSensiStandard.combinedWith(pvSensiDiff)).build();
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the settlement of the bond trade from the real clean price 
	  /// with z-spread.
	  /// <para>
	  /// The present value sensitivity of the settlement is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <param name="cleanRealPrice">  the clean real price </param>
	  /// <returns> the present value sensitivity of the settlement </returns>
	  public virtual PointSensitivities presentValueSensitivityFromCleanPriceWithZSpread(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData, double cleanRealPrice, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate valuationDate = ratesProvider.ValuationDate;
		ResolvedCapitalIndexedBond bond = trade.Product;
		LocalDate standardSettlementDate = bond.calculateSettlementDateFromValuation(valuationDate, refData);
		LocalDate tradeSettlementDate = settlementDate(trade, valuationDate);
		RepoCurveDiscountFactors repoDf = DiscountingCapitalIndexedBondProductPricer.repoCurveDf(bond, discountingProvider);
		double df = repoDf.discountFactor(standardSettlementDate);
		PointSensitivityBuilder dfSensi = repoDf.zeroRatePointSensitivity(standardSettlementDate);
		PointSensitivityBuilder pvSensiStandard = forecastValueSensitivityStandardFromCleanPrice(bond, ratesProvider, standardSettlementDate, cleanRealPrice).multipliedBy(df).combinedWith(dfSensi.multipliedBy(forecastValueStandardFromCleanPrice(bond, ratesProvider, standardSettlementDate, cleanRealPrice).Amount));
		if (standardSettlementDate.isEqual(tradeSettlementDate))
		{
		  return presentValueSensitivityFromProductPresentValueSensitivity(trade, ratesProvider, discountingProvider, pvSensiStandard).build();
		}
		// check coupon payment between two settlement dates
		IssuerCurveDiscountFactors issuerDf = DiscountingCapitalIndexedBondProductPricer.issuerCurveDf(bond, discountingProvider);
		PointSensitivityBuilder pvSensiDiff = PointSensitivityBuilder.none();
		if (standardSettlementDate.isAfter(tradeSettlementDate))
		{
		  pvSensiDiff = pvSensiDiff.combinedWith(productPricer.presentValueSensitivityCouponWithZSpread(bond, ratesProvider, issuerDf, tradeSettlementDate, standardSettlementDate, zSpread, compoundedRateType, periodsPerYear).multipliedBy(-1d));
		}
		else
		{
		  pvSensiDiff = pvSensiDiff.combinedWith(productPricer.presentValueSensitivityCouponWithZSpread(bond, ratesProvider, issuerDf, standardSettlementDate, tradeSettlementDate, zSpread, compoundedRateType, periodsPerYear));
		}
		return presentValueSensitivityFromProductPresentValueSensitivity(trade, ratesProvider, discountingProvider, pvSensiStandard.combinedWith(pvSensiDiff)).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the bond trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="cleanRealPrice">  the clean real price </param>
	  /// <returns> the currency exposure of the trade </returns>
	  public virtual MultiCurrencyAmount currencyExposureFromCleanPrice(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData, double cleanRealPrice)
	  {

		CurrencyAmount pv = presentValueFromCleanPrice(trade, ratesProvider, discountingProvider, refData, cleanRealPrice);
		return MultiCurrencyAmount.of(pv);
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the bond trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <returns> the currency exposure of the trade </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		CurrencyAmount pv = presentValue(trade, ratesProvider, discountingProvider);
		return MultiCurrencyAmount.of(pv);
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the bond trade with z-spread.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <param name="cleanRealPrice">  the clean real price </param>
	  /// <returns> the currency exposure of the trade </returns>
	  public virtual MultiCurrencyAmount currencyExposureFromCleanPriceWithZSpread(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData, double cleanRealPrice, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		CurrencyAmount pv = presentValueFromCleanPriceWithZSpread(trade, ratesProvider, discountingProvider, refData, cleanRealPrice, zSpread, compoundedRateType, periodsPerYear);
		return MultiCurrencyAmount.of(pv);
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the bond trade with z-spread.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the currency exposure of the trade </returns>
	  public virtual MultiCurrencyAmount currencyExposureWithZSpread(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		CurrencyAmount pv = presentValueWithZSpread(trade, ratesProvider, discountingProvider, zSpread, compoundedRateType, periodsPerYear);
		return MultiCurrencyAmount.of(pv);
	  }

	  /// <summary>
	  /// Calculates the current cash of the bond trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider)
	  {

		LocalDate valuationDate = ratesProvider.ValuationDate;
		LocalDate settlementDate = this.settlementDate(trade, valuationDate);
		CurrencyAmount cashProduct = productPricer.currentCash(trade.Product, ratesProvider, settlementDate);
		if (!trade.Settlement.Present)
		{
		  return cashProduct;
		}
		BondPaymentPeriod settlePeriod = trade.Settlement.get().Payment;
		double cashSettle = settlePeriod.PaymentDate.isEqual(valuationDate) ? netAmount(trade, ratesProvider).Amount : 0d;
		return cashProduct.plus(cashSettle);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the net amount of the settlement of the bond trade.
	  /// <para>
	  /// Since the sign of the settlement notional is opposite to that of the product, negative amount will be returned 
	  /// for positive quantity of trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <returns> the net amount </returns>
	  public virtual CurrencyAmount netAmount(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider)
	  {

		if (!trade.Settlement.Present)
		{
		  // position has no settlement, thus it has no value
		  return CurrencyAmount.zero(trade.Product.Currency);
		}
		BondPaymentPeriod settlePeriod = trade.Settlement.get().Payment;
		if (settlePeriod is KnownAmountBondPaymentPeriod)
		{
		  Payment payment = ((KnownAmountBondPaymentPeriod) settlePeriod).Payment;
		  return payment.Value;
		}
		else if (settlePeriod is CapitalIndexedBondPaymentPeriod)
		{
		  CapitalIndexedBondPaymentPeriod casted = (CapitalIndexedBondPaymentPeriod) settlePeriod;
		  double netAmount = productPricer.PeriodPricer.forecastValue(casted, ratesProvider);
		  return CurrencyAmount.of(casted.Currency, netAmount);
		}
		throw new System.NotSupportedException("unsupported settlement type");
	  }

	  //-------------------------------------------------------------------------
	  private CurrencyAmount presentValueSettlement(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		if (!trade.Settlement.Present)
		{
		  // position has no settlement, thus it has no value
		  return CurrencyAmount.zero(trade.Product.Currency);
		}
		BondPaymentPeriod settlePeriod = trade.Settlement.get().Payment;
		ResolvedCapitalIndexedBond product = trade.Product;
		CurrencyAmount netAmount = this.netAmount(trade, ratesProvider);
		RepoCurveDiscountFactors repoDf = DiscountingCapitalIndexedBondProductPricer.repoCurveDf(product, discountingProvider);
		return netAmount.multipliedBy(repoDf.discountFactor(settlePeriod.PaymentDate));
	  }

	  private CurrencyAmount presentValueFromProductPresentValue(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, CurrencyAmount productPresentValue)
	  {

		CurrencyAmount pvProduct = productPresentValue.multipliedBy(trade.Quantity);
		CurrencyAmount pvPayment = presentValueSettlement(trade, ratesProvider, discountingProvider);
		return pvProduct.plus(pvPayment);
	  }

	  internal virtual CurrencyAmount forecastValueStandardFromCleanPrice(ResolvedCapitalIndexedBond product, RatesProvider ratesProvider, LocalDate standardSettlementDate, double realCleanPrice)
	  {

		double notional = product.Notional;
		double netAmountReal = realCleanPrice * notional + product.accruedInterest(standardSettlementDate);
		double indexRatio = product.YieldConvention.Equals(CapitalIndexedBondYieldConvention.GB_IL_FLOAT) ? 1d : productPricer.indexRatio(product, ratesProvider, standardSettlementDate);
		return CurrencyAmount.of(product.Currency, indexRatio * netAmountReal);
	  }

	  //-------------------------------------------------------------------------
	  // the sensitivity of the product plus settlement
	  private PointSensitivityBuilder presentValueSensitivityFromProductPresentValueSensitivity(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, PointSensitivityBuilder productPresnetValueSensitivity)
	  {

		PointSensitivityBuilder sensiProduct = productPresnetValueSensitivity.multipliedBy(trade.Quantity);
		PointSensitivityBuilder sensiPayment = presentValueSensitivitySettlement(trade, ratesProvider, discountingProvider);
		return sensiProduct.combinedWith(sensiPayment);
	  }

	  // the sensitivity of the present value of the settlement
	  private PointSensitivityBuilder presentValueSensitivitySettlement(ResolvedCapitalIndexedBondTrade trade, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		if (!trade.Settlement.Present)
		{
		  // position has no settlement, thus it has no sensitivity
		  return PointSensitivityBuilder.none();
		}
		ResolvedCapitalIndexedBondSettlement settlement = trade.Settlement.get();
		BondPaymentPeriod settlePeriod = settlement.Payment;
		ResolvedCapitalIndexedBond product = trade.Product;
		RepoCurveDiscountFactors repoDf = DiscountingCapitalIndexedBondProductPricer.repoCurveDf(product, discountingProvider);
		double df = repoDf.discountFactor(settlePeriod.PaymentDate);
		double netAmount = this.netAmount(trade, ratesProvider).Amount;
		PointSensitivityBuilder dfSensi = repoDf.zeroRatePointSensitivity(settlePeriod.PaymentDate).multipliedBy(netAmount);
		PointSensitivityBuilder naSensi = netAmountSensitivity(settlement, ratesProvider).multipliedBy(df);
		return dfSensi.combinedWith(naSensi);
	  }

	  // the sensitivity of the net amount of the settlement
	  private PointSensitivityBuilder netAmountSensitivity(ResolvedCapitalIndexedBondSettlement settlement, RatesProvider ratesProvider)
	  {

		BondPaymentPeriod settlePeriod = settlement.Payment;
		if (settlePeriod is KnownAmountBondPaymentPeriod)
		{
		  return PointSensitivityBuilder.none();
		}
		else if (settlePeriod is CapitalIndexedBondPaymentPeriod)
		{
		  CapitalIndexedBondPaymentPeriod casted = (CapitalIndexedBondPaymentPeriod) settlePeriod;
		  return productPricer.PeriodPricer.forecastValueSensitivity(casted, ratesProvider);
		}
		throw new System.NotSupportedException("unsupported settlement type");
	  }

	  // the sensitivity of the forecast value given the clean price
	  internal virtual PointSensitivityBuilder forecastValueSensitivityStandardFromCleanPrice(ResolvedCapitalIndexedBond product, RatesProvider ratesProvider, LocalDate standardSettlementDate, double realCleanPrice)
	  {

		if (product.YieldConvention.Equals(CapitalIndexedBondYieldConvention.GB_IL_FLOAT))
		{
		  return PointSensitivityBuilder.none();
		}
		double notional = product.Notional;
		double netAmountReal = realCleanPrice * notional + product.accruedInterest(standardSettlementDate);
		PointSensitivityBuilder indexRatioSensi = productPricer.indexRatioSensitivity(product, ratesProvider, standardSettlementDate);
		return indexRatioSensi.multipliedBy(netAmountReal);
	  }

	  //-------------------------------------------------------------------------
	  // calculate the settlement date
	  private LocalDate settlementDate(ResolvedCapitalIndexedBondTrade trade, LocalDate valuationDate)
	  {
		return trade.Settlement.map(settle => settle.SettlementDate).orElse(valuationDate);
	  }

	  private void validate(RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {
		ArgChecker.isTrue(ratesProvider.ValuationDate.isEqual(discountingProvider.ValuationDate), "the rates providers should be for the same date");
	  }

	}

}