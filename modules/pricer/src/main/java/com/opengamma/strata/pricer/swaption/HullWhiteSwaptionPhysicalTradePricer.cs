/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using HullWhiteOneFactorPiecewiseConstantParametersProvider = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParametersProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;
	using ResolvedSwaptionTrade = com.opengamma.strata.product.swaption.ResolvedSwaptionTrade;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;

	/// <summary>
	/// Pricer for swaption with physical settlement in Hull-White one factor model with piecewise constant volatility.
	/// <para>
	/// Reference: Henrard, M. "The Irony in the derivatives discounting Part II: the crisis", Wilmott Journal, 2010, 2, 301-316
	/// </para>
	/// </summary>
	public class HullWhiteSwaptionPhysicalTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly HullWhiteSwaptionPhysicalTradePricer DEFAULT = new HullWhiteSwaptionPhysicalTradePricer();

	  /// <summary>
	  /// Pricer for <seealso cref="Swaption"/>.  
	  /// </summary>
	  private static readonly HullWhiteSwaptionPhysicalProductPricer PRICER_PRODUCT = HullWhiteSwaptionPhysicalProductPricer.DEFAULT;
	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/> which is used to described the premium.
	  /// </summary>
	  private static readonly DiscountingPaymentPricer PRICER_PREMIUM = DiscountingPaymentPricer.DEFAULT;

	  /// <summary>
	  /// Calculates the present value of the swaption trade.
	  /// <para>
	  /// The result is expressed using the currency of the swapion.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the swaption trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter trade </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		ResolvedSwaption product = trade.Product;
		CurrencyAmount pvProduct = PRICER_PRODUCT.presentValue(product, ratesProvider, hwProvider);
		Payment premium = trade.Premium;
		CurrencyAmount pvPremium = PRICER_PREMIUM.presentValue(premium, ratesProvider);
		return pvProduct.plus(pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the currency exposure of the swaption trade.
	  /// </summary>
	  /// <param name="trade">  the swaption trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		return MultiCurrencyAmount.of(presentValue(trade, ratesProvider, hwProvider));
	  }

	  /// <summary>
	  /// Calculates the current cash of the swaption trade.
	  /// <para>
	  /// Only the premium is contributing to the current cash for non-cash settle swaptions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the swaption trade </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the current cash amount </returns>
	  public virtual CurrencyAmount currentCash(ResolvedSwaptionTrade trade, LocalDate valuationDate)
	  {
		Payment premium = trade.Premium;
		if (premium.Date.Equals(valuationDate))
		{
		  return CurrencyAmount.of(premium.Currency, premium.Amount);
		}
		return CurrencyAmount.of(premium.Currency, 0.0);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the swaption product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the swaption trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the point sensitivity to the rate curves </returns>
	  public virtual PointSensitivities presentValueSensitivityRates(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		ResolvedSwaption product = trade.Product;
		PointSensitivityBuilder pvcsProduct = PRICER_PRODUCT.presentValueSensitivityRates(product, ratesProvider, hwProvider);
		Payment premium = trade.Premium;
		PointSensitivityBuilder pvcsPremium = PRICER_PREMIUM.presentValueSensitivity(premium, ratesProvider);
		return pvcsProduct.combinedWith(pvcsPremium).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity piecewise constant volatility parameters of the Hull-White model.
	  /// </summary>
	  /// <param name="trade">  the swaption trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the present value Hull-White model parameter sensitivity of the swaption trade </returns>
	  public virtual DoubleArray presentValueSensitivityModelParamsHullWhite(ResolvedSwaptionTrade trade, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		ResolvedSwaption product = trade.Product;
		return PRICER_PRODUCT.presentValueSensitivityModelParamsHullWhite(product, ratesProvider, hwProvider);
	  }

	}

}