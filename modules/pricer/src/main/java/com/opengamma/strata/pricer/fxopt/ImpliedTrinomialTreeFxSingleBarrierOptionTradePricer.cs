/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingleBarrierOption = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOption;
	using ResolvedFxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOptionTrade;

	/// <summary>
	/// Pricer for FX barrier option trades under implied trinomial tree.
	/// <para>
	/// This function provides the ability to price an <seealso cref="ResolvedFxSingleBarrierOptionTrade"/>.
	/// </para>
	/// </summary>
	public class ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer DEFAULT = new ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer(ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedFxSingleBarrierOption"/>.
	  /// </summary>
	  private readonly ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer productPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedFxSingleBarrierOption"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public ImpliedTrinomialTreeFxSingleBarrierOptionTradePricer(ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer productPricer, DiscountingPaymentPricer paymentPricer)
	  {
		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the FX barrier option trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// </para>
	  /// <para>
	  /// The trinomial tree is first calibrated to Black volatilities, 
	  /// then the price is computed based on the calibrated tree.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value of the trade </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ResolvedFxSingleBarrierOption product = trade.Product;
		CurrencyAmount pvProduct = productPricer.presentValue(product, ratesProvider, volatilities);
		Payment premium = trade.Premium;
		CurrencyAmount pvPremium = paymentPricer.presentValue(premium, ratesProvider);
		return MultiCurrencyAmount.of(pvProduct, pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the FX barrier option trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The sensitivity is computed by bump and re-price, returning <seealso cref="CurrencyParameterSensitivities"/>,
	  /// not <seealso cref="PointSensitivities"/>.
	  /// </para>
	  /// <para>
	  /// The trinomial tree is first calibrated to Black volatilities, 
	  /// then the price is computed based on the calibrated tree.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value curve sensitivity of the trade </returns>
	  public virtual CurrencyParameterSensitivities presentValueSensitivityRates(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ResolvedFxSingleBarrierOption product = trade.Product;
		CurrencyParameterSensitivities sensProduct = productPricer.presentValueSensitivityRates(product, ratesProvider, volatilities);
		Payment premium = trade.Premium;
		PointSensitivityBuilder pvcsPremium = paymentPricer.presentValueSensitivity(premium, ratesProvider);
		CurrencyParameterSensitivities sensPremium = ratesProvider.parameterSensitivity(pvcsPremium.build());
		return sensProduct.combinedWith(sensPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the FX barrier option trade.
	  /// <para>
	  /// The trinomial tree is first calibrated to Black volatilities, 
	  /// then the price is computed based on the calibrated tree.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxSingleBarrierOptionTrade trade, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		Payment premium = trade.Premium;
		CurrencyAmount pvPremium = paymentPricer.presentValue(premium, ratesProvider);
		ResolvedFxSingleBarrierOption product = trade.Product;
		return productPricer.currencyExposure(product, ratesProvider, volatilities).plus(pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the current of the FX barrier option trade.
	  /// </summary>
	  /// <param name="trade">  the option trade </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the current cash amount </returns>
	  public virtual CurrencyAmount currentCash(ResolvedFxSingleBarrierOptionTrade trade, LocalDate valuationDate)
	  {
		Payment premium = trade.Premium;
		if (premium.Date.Equals(valuationDate))
		{
		  return CurrencyAmount.of(premium.Currency, premium.Amount);
		}
		return CurrencyAmount.of(premium.Currency, 0d);
	  }

	}

}