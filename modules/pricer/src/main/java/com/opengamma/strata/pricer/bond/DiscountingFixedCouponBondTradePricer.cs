/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using FixedCouponBondPaymentPeriod = com.opengamma.strata.product.bond.FixedCouponBondPaymentPeriod;
	using ResolvedFixedCouponBond = com.opengamma.strata.product.bond.ResolvedFixedCouponBond;
	using ResolvedFixedCouponBondSettlement = com.opengamma.strata.product.bond.ResolvedFixedCouponBondSettlement;
	using ResolvedFixedCouponBondTrade = com.opengamma.strata.product.bond.ResolvedFixedCouponBondTrade;

	/// <summary>
	/// Pricer for fixed coupon bond trades.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedFixedCouponBondTrade"/>.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
	public class DiscountingFixedCouponBondTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFixedCouponBondTradePricer DEFAULT = new DiscountingFixedCouponBondTradePricer(DiscountingFixedCouponBondProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFixedCouponBond"/>.
	  /// </summary>
	  private readonly DiscountingFixedCouponBondProductPricer productPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedFixedCouponBond"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public DiscountingFixedCouponBondTradePricer(DiscountingFixedCouponBondProductPricer productPricer, DiscountingPaymentPricer paymentPricer)
	  {

		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the fixed coupon bond trade.
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
	  /// <param name="provider">  the discounting provider </param>
	  /// <returns> the present value of the fixed coupon bond trade </returns>
	  public virtual CurrencyAmount presentValue(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider)
	  {
		LocalDate settlementDate = this.settlementDate(trade, provider.ValuationDate);
		CurrencyAmount pvProduct = productPricer.presentValue(trade.Product, provider, settlementDate);
		return presentValueFromProductPresentValue(trade, provider, pvProduct);
	  }

	  /// <summary>
	  /// Calculates the present value of the fixed coupon bond trade with z-spread.
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
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value of the fixed coupon bond trade </returns>
	  public virtual CurrencyAmount presentValueWithZSpread(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		LocalDate settlementDate = this.settlementDate(trade, provider.ValuationDate);
		CurrencyAmount pvProduct = productPricer.presentValueWithZSpread(trade.Product, provider, zSpread, compoundedRateType, periodsPerYear, settlementDate);
		return presentValueFromProductPresentValue(trade, provider, pvProduct);
	  }

	  private CurrencyAmount presentValueFromProductPresentValue(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider, CurrencyAmount productPresentValue)
	  {

		CurrencyAmount pvProduct = productPresentValue.multipliedBy(trade.Quantity);
		CurrencyAmount pvPayment = presentValuePayment(trade, provider);
		return pvProduct.plus(pvPayment);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the fixed coupon bond trade from the clean price of the underlying product.
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
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="cleanPrice">  the clean price </param>
	  /// <returns> the present value of the fixed coupon bond trade </returns>
	  public virtual CurrencyAmount presentValueFromCleanPrice(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider, ReferenceData refData, double cleanPrice)
	  {

		ResolvedFixedCouponBond product = trade.Product;
		LocalDate standardSettlementDate = this.standardSettlementDate(product, provider, refData);
		LocalDate tradeSettlementDate = settlementDate(trade, provider.ValuationDate);
		Currency currency = product.Currency;
		RepoCurveDiscountFactors repoDf = DiscountingFixedCouponBondProductPricer.repoCurveDf(product, provider);
		double df = repoDf.discountFactor(standardSettlementDate);
		double pvStandard = (cleanPrice * product.Notional + productPricer.accruedInterest(product, standardSettlementDate)) * df;
		if (standardSettlementDate.isEqual(tradeSettlementDate))
		{
		  return presentValueFromProductPresentValue(trade, provider, CurrencyAmount.of(currency, pvStandard));
		}
		// check coupon payment between two settlement dates
		IssuerCurveDiscountFactors issuerDf = DiscountingFixedCouponBondProductPricer.issuerCurveDf(product, provider);
		double pvDiff = 0d;
		if (standardSettlementDate.isAfter(tradeSettlementDate))
		{
		  pvDiff = productPricer.presentValueCoupon(product, issuerDf, tradeSettlementDate, standardSettlementDate);
		}
		else
		{
		  pvDiff = -productPricer.presentValueCoupon(product, issuerDf, standardSettlementDate, tradeSettlementDate);
		}
		return presentValueFromProductPresentValue(trade, provider, CurrencyAmount.of(currency, pvStandard + pvDiff));
	  }

	  /// <summary>
	  /// Calculates the present value of the fixed coupon bond trade with z-spread from the
	  /// clean price of the underlying product.
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
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="cleanPrice">  the clean price </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value of the fixed coupon bond trade </returns>
	  public virtual CurrencyAmount presentValueFromCleanPriceWithZSpread(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider, ReferenceData refData, double cleanPrice, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		ResolvedFixedCouponBond product = trade.Product;
		LocalDate standardSettlementDate = this.standardSettlementDate(product, provider, refData);
		LocalDate tradeSettlementDate = settlementDate(trade, provider.ValuationDate);
		Currency currency = product.Currency;
		RepoCurveDiscountFactors repoDf = DiscountingFixedCouponBondProductPricer.repoCurveDf(product, provider);
		double df = repoDf.discountFactor(standardSettlementDate);
		double pvStandard = (cleanPrice * product.Notional + productPricer.accruedInterest(product, standardSettlementDate)) * df;
		if (standardSettlementDate.isEqual(tradeSettlementDate))
		{
		  return presentValueFromProductPresentValue(trade, provider, CurrencyAmount.of(currency, pvStandard));
		}
		// check coupon payment between two settlement dates
		IssuerCurveDiscountFactors issuerDf = DiscountingFixedCouponBondProductPricer.issuerCurveDf(product, provider);
		double pvDiff = 0d;
		if (standardSettlementDate.isAfter(tradeSettlementDate))
		{
		  pvDiff = productPricer.presentValueCouponWithZSpread(product, issuerDf, tradeSettlementDate, standardSettlementDate, zSpread, compoundedRateType, periodsPerYear);
		}
		else
		{
		  pvDiff = -productPricer.presentValueCouponWithZSpread(product, issuerDf, standardSettlementDate, tradeSettlementDate, zSpread, compoundedRateType, periodsPerYear);
		}
		return presentValueFromProductPresentValue(trade, provider, CurrencyAmount.of(currency, pvStandard + pvDiff));
	  }

	  // calculates the settlement date using the offset from the valuation date
	  private LocalDate standardSettlementDate(ResolvedFixedCouponBond product, LegalEntityDiscountingProvider provider, ReferenceData refData)
	  {

		return product.SettlementDateOffset.adjust(provider.ValuationDate, refData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the fixed coupon bond trade.
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
	  /// <param name="provider">  the discounting provider </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider)
	  {

		LocalDate settlementDate = this.settlementDate(trade, provider.ValuationDate);
		PointSensitivityBuilder sensiProduct = productPricer.presentValueSensitivity(trade.Product, provider, settlementDate);
		return presentValueSensitivityFromProductPresentValueSensitivity(trade, provider, sensiProduct).build();
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the fixed coupon bond trade with z-spread.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
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
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public virtual PointSensitivities presentValueSensitivityWithZSpread(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		LocalDate settlementDate = this.settlementDate(trade, provider.ValuationDate);
		PointSensitivityBuilder sensiProduct = productPricer.presentValueSensitivityWithZSpread(trade.Product, provider, zSpread, compoundedRateType, periodsPerYear, settlementDate);
		return presentValueSensitivityFromProductPresentValueSensitivity(trade, provider, sensiProduct).build();
	  }

	  private PointSensitivityBuilder presentValueSensitivityFromProductPresentValueSensitivity(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider, PointSensitivityBuilder productPresnetValueSensitivity)
	  {

		PointSensitivityBuilder sensiProduct = productPresnetValueSensitivity.multipliedBy(trade.Quantity);
		PointSensitivityBuilder sensiPayment = presentValueSensitivityPayment(trade, provider);
		return sensiProduct.combinedWith(sensiPayment);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the fixed coupon bond trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <returns> the currency exposure of the fixed coupon bond trade </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider)
	  {

		return MultiCurrencyAmount.of(presentValue(trade, provider));
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the fixed coupon bond trade with z-spread.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the currency exposure of the fixed coupon bond trade </returns>
	  public virtual MultiCurrencyAmount currencyExposureWithZSpread(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		return MultiCurrencyAmount.of(presentValueWithZSpread(trade, provider, zSpread, compoundedRateType, periodsPerYear));
	  }

	  /// <summary>
	  /// Calculates the current cash of the fixed coupon bond trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the current cash amount </returns>
	  public virtual CurrencyAmount currentCash(ResolvedFixedCouponBondTrade trade, LocalDate valuationDate)
	  {
		Payment upfrontPayment = this.upfrontPayment(trade);
		Currency currency = upfrontPayment.Currency; // assumes single currency is involved in trade
		CurrencyAmount currentCash = CurrencyAmount.zero(currency);
		if (upfrontPayment.Date.Equals(valuationDate))
		{
		  currentCash = currentCash.plus(upfrontPayment.Value);
		}
		if (trade.Settlement.Present)
		{
		  LocalDate settlementDate = trade.Settlement.get().SettlementDate;
		  ResolvedFixedCouponBond product = trade.Product;
		  if (!settlementDate.isAfter(valuationDate))
		  {
			double cashCoupon = product.hasExCouponPeriod() ? 0d : currentCashCouponPayment(product, valuationDate);
			Payment payment = product.NominalPayment;
			double cashNominal = payment.Date.isEqual(valuationDate) ? payment.Amount : 0d;
			currentCash = currentCash.plus(CurrencyAmount.of(currency, (cashCoupon + cashNominal) * trade.Quantity));
		  }
		}
		return currentCash;
	  }

	  private double currentCashCouponPayment(ResolvedFixedCouponBond product, LocalDate referenceDate)
	  {
		double cash = 0d;
		foreach (FixedCouponBondPaymentPeriod period in product.PeriodicPayments)
		{
		  if (period.PaymentDate.isEqual(referenceDate))
		  {
			cash += period.FixedRate * period.Notional * period.YearFraction;
		  }
		}
		return cash;
	  }

	  //-------------------------------------------------------------------------
	  private CurrencyAmount presentValuePayment(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider)
	  {
		RepoCurveDiscountFactors repoDf = DiscountingFixedCouponBondProductPricer.repoCurveDf(trade.Product, provider);
		Payment upfrontPayment = this.upfrontPayment(trade);
		return paymentPricer.presentValue(upfrontPayment, repoDf.DiscountFactors);
	  }

	  private PointSensitivityBuilder presentValueSensitivityPayment(ResolvedFixedCouponBondTrade trade, LegalEntityDiscountingProvider provider)
	  {

		RepoCurveDiscountFactors repoDf = DiscountingFixedCouponBondProductPricer.repoCurveDf(trade.Product, provider);
		Payment upfrontPayment = this.upfrontPayment(trade);
		PointSensitivityBuilder pt = paymentPricer.presentValueSensitivity(upfrontPayment, repoDf.DiscountFactors);
		if (pt is ZeroRateSensitivity)
		{
		  return RepoCurveZeroRateSensitivity.of((ZeroRateSensitivity) pt, repoDf.RepoGroup);
		}
		return pt; // NoPointSensitivity
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the payment that was made for the trade.
	  /// <para>
	  /// This is the payment that was made on the settlement date, based on the quantity and clean price.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <returns> the payment that was made </returns>
	  public virtual Payment upfrontPayment(ResolvedFixedCouponBondTrade trade)
	  {
		ResolvedFixedCouponBond product = trade.Product;
		Currency currency = product.Currency;
		if (!trade.Settlement.Present)
		{
		  return Payment.of(CurrencyAmount.zero(currency), product.StartDate); // date doesn't matter as it is zero
		}
		// payment is based on the dirty price
		ResolvedFixedCouponBondSettlement settlement = trade.Settlement.get();
		LocalDate settlementDate = settlement.SettlementDate;
		double cleanPrice = settlement.Price;
		double dirtyPrice = productPricer.dirtyPriceFromCleanPrice(product, settlementDate, cleanPrice);
		// calculate payment
		double quantity = trade.Quantity;
		double notional = product.Notional;
		return Payment.of(CurrencyAmount.of(currency, -quantity * notional * dirtyPrice), settlementDate);
	  }

	  //-------------------------------------------------------------------------
	  // calculate the settlement date
	  private LocalDate settlementDate(ResolvedFixedCouponBondTrade trade, LocalDate valuationDate)
	  {
		return trade.Settlement.map(settle => settle.SettlementDate).orElse(valuationDate);
	  }

	}

}