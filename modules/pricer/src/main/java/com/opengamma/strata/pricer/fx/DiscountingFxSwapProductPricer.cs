/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{

	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxSwap = com.opengamma.strata.product.fx.ResolvedFxSwap;

	/// <summary>
	/// Pricer for foreign exchange swap transaction products.
	/// <para>
	/// This provides the ability to price an <seealso cref="ResolvedFxSwap"/>.
	/// </para>
	/// </summary>
	public class DiscountingFxSwapProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFxSwapProductPricer DEFAULT = new DiscountingFxSwapProductPricer(DiscountingFxSingleProductPricer.DEFAULT);

	  /// <summary>
	  /// Underlying single FX pricer.
	  /// </summary>
	  private readonly DiscountingFxSingleProductPricer fxPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="fxPricer">  the pricer for <seealso cref="ResolvedFxSingle"/> </param>
	  public DiscountingFxSwapProductPricer(DiscountingFxSingleProductPricer fxPricer)
	  {
		this.fxPricer = ArgChecker.notNull(fxPricer, "fxPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the FX swap product.
	  /// <para>
	  /// This discounts each payment on each leg in its own currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value in the two natural currencies </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedFxSwap swap, RatesProvider provider)
	  {
		MultiCurrencyAmount farPv = fxPricer.presentValue(swap.FarLeg, provider);
		MultiCurrencyAmount nearPv = fxPricer.presentValue(swap.NearLeg, provider);
		return nearPv.plus(farPv);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the FX swap product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedFxSwap swap, RatesProvider provider)
	  {
		PointSensitivities nearSens = fxPricer.presentValueSensitivity(swap.NearLeg, provider);
		PointSensitivities farSens = fxPricer.presentValueSensitivity(swap.FarLeg, provider);
		return nearSens.combinedWith(farSens);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par spread.
	  /// <para>
	  /// The par spread is the spread that should be added to the FX forward points to have a zero value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the spread </returns>
	  public virtual double parSpread(ResolvedFxSwap swap, RatesProvider provider)
	  {
		Payment counterPaymentNear = swap.NearLeg.CounterCurrencyPayment;
		MultiCurrencyAmount pv = presentValue(swap, provider);
		double pvCounterCcy = pv.convertedTo(counterPaymentNear.Currency, provider).Amount;
		double dfEnd = provider.discountFactor(counterPaymentNear.Currency, swap.FarLeg.PaymentDate);
		double notionalBaseCcy = swap.NearLeg.BaseCurrencyPayment.Amount;
		return -pvCounterCcy / (notionalBaseCcy * dfEnd);
	  }

	  /// <summary>
	  /// Calculates the par spread sensitivity to the curves.
	  /// <para>
	  /// The sensitivity is reported in the counter currency of the product, but is actually dimensionless.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the spread curve sensitivity </returns>
	  public virtual PointSensitivities parSpreadSensitivity(ResolvedFxSwap swap, RatesProvider provider)
	  {
		Payment counterPaymentNear = swap.NearLeg.CounterCurrencyPayment;
		MultiCurrencyAmount pv = presentValue(swap, provider);
		double pvCounterCcy = pv.convertedTo(counterPaymentNear.Currency, provider).Amount;
		double dfEnd = provider.discountFactor(counterPaymentNear.Currency, swap.FarLeg.PaymentDate);
		double notionalBaseCcy = swap.NearLeg.BaseCurrencyPayment.Amount;
		double ps = -pvCounterCcy / (notionalBaseCcy * dfEnd);
		// backward sweep
		double psBar = 1d;
		double pvCounterCcyBar = -1d / (notionalBaseCcy * dfEnd) * psBar;
		double dfEndBar = -ps / dfEnd * psBar;
		ZeroRateSensitivity ddfEnddr = provider.discountFactors(counterPaymentNear.Currency).zeroRatePointSensitivity(swap.FarLeg.PaymentDate);
		PointSensitivities result = ddfEnddr.multipliedBy(dfEndBar).build();
		PointSensitivities dpvdr = presentValueSensitivity(swap, provider);
		PointSensitivities dpvdrConverted = dpvdr.convertedTo(counterPaymentNear.Currency, provider);
		return result.combinedWith(dpvdrConverted.multipliedBy(pvCounterCcyBar));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the FX swap product.
	  /// <para>
	  /// This discounts each payment on each leg in its own currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="product">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxSwap product, RatesProvider provider)
	  {
		return presentValue(product, provider);
	  }

	  /// <summary>
	  /// Calculates the current cash of the FX swap product.
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the current cash </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedFxSwap swap, LocalDate valuationDate)
	  {
		MultiCurrencyAmount farPv = fxPricer.currentCash(swap.FarLeg, valuationDate);
		MultiCurrencyAmount nearPv = fxPricer.currentCash(swap.NearLeg, valuationDate);
		return nearPv.plus(farPv);
	  }

	}

}