/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ResolvedBill = com.opengamma.strata.product.bond.ResolvedBill;
	using ResolvedBillTrade = com.opengamma.strata.product.bond.ResolvedBillTrade;

	/// <summary>
	/// Pricer for bill trades.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedBillTrade"/>.
	/// </para>
	/// </summary>
	public class DiscountingBillTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingBillTradePricer DEFAULT = new DiscountingBillTradePricer(DiscountingBillProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedBill"/>.
	  /// </summary>
	  private readonly DiscountingBillProductPricer productPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedBill"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public DiscountingBillTradePricer(DiscountingBillProductPricer productPricer, DiscountingPaymentPricer paymentPricer)
	  {

		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  /// <summary>
	  /// Calculates the present value of a bill trade.
	  /// <para>
	  /// If the settlement details are provided, the present value is the sum of the underlying product's present value
	  /// multiplied by the quantity and the present value of the settlement payment if still due at the valuation date. 
	  /// If not it is the underlying product's present value multiplied by the quantity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedBillTrade trade, LegalEntityDiscountingProvider provider)
	  {
		if (provider.ValuationDate.isAfter(trade.Product.Notional.Date))
		{
		  return CurrencyAmount.of(trade.Product.Currency, 0.0d);
		}
		CurrencyAmount pvProduct = productPricer.presentValue(trade.Product, provider).multipliedBy(trade.Quantity);
		if (trade.Settlement.Present)
		{
		  RepoCurveDiscountFactors repoDf = DiscountingBillProductPricer.repoCurveDf(trade.Product, provider);
		  CurrencyAmount pvSettle = paymentPricer.presentValue(trade.Settlement.get(), repoDf.DiscountFactors);
		  return pvProduct.plus(pvSettle);
		}
		return pvProduct;
	  }

	  /// <summary>
	  /// Calculates the present value of a bill trade with z-spread.
	  /// <para>
	  /// If the settlement details are provided, the present value is the sum of the underlying product's present value
	  /// multiplied by the quantity and the present value of the settlement payment if still due at the valuation date. 
	  /// If not it is the underlying product's present value multiplied by the quantity.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic compounded rates of 
	  /// the issuer discounting curve. The z-spread is applied only on the legal entity curve, not on the repo curve used
	  /// for the settlement amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValueWithZSpread(ResolvedBillTrade trade, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		if (provider.ValuationDate.isAfter(trade.Product.Notional.Date))
		{
		  return CurrencyAmount.of(trade.Product.Currency, 0.0d);
		}
		CurrencyAmount pvProduct = productPricer.presentValueWithZSpread(trade.Product, provider, zSpread, compoundedRateType, periodsPerYear).multipliedBy(trade.Quantity);
		if (trade.Settlement.Present)
		{
		  RepoCurveDiscountFactors repoDf = DiscountingBillProductPricer.repoCurveDf(trade.Product, provider);
		  CurrencyAmount pvSettle = paymentPricer.presentValue(trade.Settlement.get(), repoDf.DiscountFactors);
		  return pvProduct.plus(pvSettle);
		}
		return pvProduct;
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of a bill trade.
	  /// <para>
	  /// If the settlement details are provided, the sensitivity is the sum of the underlying product's sensitivity
	  /// multiplied by the quantity and the sensitivity of the settlement payment if still due at the valuation date. 
	  /// If not it is the underlying product's sensitivity multiplied by the quantity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedBillTrade trade, LegalEntityDiscountingProvider provider)
	  {
		if (provider.ValuationDate.isAfter(trade.Product.Notional.Date))
		{
		  return PointSensitivities.empty();
		}
		PointSensitivities sensiProduct = productPricer.presentValueSensitivity(trade.Product, provider).multipliedBy(trade.Quantity);
		if (!trade.Settlement.Present)
		{
		  return sensiProduct;
		}
		Payment settlement = trade.Settlement.get();
		RepoCurveDiscountFactors repoDf = DiscountingBillProductPricer.repoCurveDf(trade.Product, provider);
		PointSensitivities sensiSettle = presentValueSensitivitySettlement(settlement, repoDf);
		return sensiProduct.combinedWith(sensiSettle);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of a bill trade with z-spread.
	  /// <para>
	  /// If the settlement details are provided, the sensitivity is the sum of the underlying product's sensitivity
	  /// multiplied by the quantity and the sensitivity of the settlement payment if still due at the valuation date. 
	  /// If not it is the underlying product's sensitivity multiplied by the quantity.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic compounded rates of 
	  /// the issuer discounting curve. The z-spread is applied only on the legal entity curve, not on the repo curve used
	  /// for the settlement amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueSensitivityWithZSpread(ResolvedBillTrade trade, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		if (provider.ValuationDate.isAfter(trade.Product.Notional.Date))
		{
		  return PointSensitivities.empty();
		}
		PointSensitivities sensiProduct = productPricer.presentValueSensitivityWithZSpread(trade.Product, provider, zSpread, compoundedRateType, periodsPerYear).multipliedBy(trade.Quantity);
		if (!trade.Settlement.Present)
		{
		  return sensiProduct;
		}
		Payment settlement = trade.Settlement.get();
		RepoCurveDiscountFactors repoDf = DiscountingBillProductPricer.repoCurveDf(trade.Product, provider);
		PointSensitivities sensiSettle = presentValueSensitivitySettlement(settlement, repoDf);
		return sensiProduct.combinedWith(sensiSettle);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of a bill trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedBillTrade trade, LegalEntityDiscountingProvider provider)
	  {
		return MultiCurrencyAmount.of(presentValue(trade, provider));
	  }

	  /// <summary>
	  /// Calculates the currency exposure of a bill trade with z-spread.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposureWithZSpread(ResolvedBillTrade trade, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		return MultiCurrencyAmount.of(presentValueWithZSpread(trade, provider, zSpread, compoundedRateType, periodsPerYear));
	  }

	  /// <summary>
	  /// Calculates the current cash of a bill trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the current cash amount </returns>
	  public virtual CurrencyAmount currentCash(ResolvedBillTrade trade, LocalDate valuationDate)
	  {
		if (trade.Product.Notional.Date.Equals(valuationDate))
		{
		  return trade.Product.Notional.Value.multipliedBy(trade.Quantity);
		}
		if (trade.Settlement.Present && trade.Settlement.get().Date.Equals(valuationDate))
		{
		  return trade.Settlement.get().Value;
		}
		return CurrencyAmount.zero(trade.Product.Currency);
	  }

	  //-------------------------------------------------------------------------
	  private PointSensitivities presentValueSensitivitySettlement(Payment settlement, RepoCurveDiscountFactors repoDf)
	  {

		PointSensitivityBuilder pointSettle = paymentPricer.presentValueSensitivity(settlement, repoDf.DiscountFactors);
		if (pointSettle is ZeroRateSensitivity)
		{
		  return RepoCurveZeroRateSensitivity.of((ZeroRateSensitivity) pointSettle, repoDf.RepoGroup).build();
		}
		return pointSettle.build(); // NoPointSensitivity
	  }

	}

}