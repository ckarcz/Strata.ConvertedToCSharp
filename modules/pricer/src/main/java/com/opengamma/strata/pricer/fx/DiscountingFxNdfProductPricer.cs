/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxNdf = com.opengamma.strata.product.fx.ResolvedFxNdf;

	/// <summary>
	/// Pricer for FX non-deliverable forward (NDF) products.
	/// <para>
	/// This provides the ability to price an <seealso cref="ResolvedFxNdf"/>.
	/// The product is priced using forward curves for the currency pair.
	/// </para>
	/// </summary>
	public class DiscountingFxNdfProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFxNdfProductPricer DEFAULT = new DiscountingFxNdfProductPricer();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public DiscountingFxNdfProductPricer()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the NDF product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// The present value is returned in the settlement currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ndf">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the product in the settlement currency </returns>
	  public virtual CurrencyAmount presentValue(ResolvedFxNdf ndf, RatesProvider provider)
	  {
		Currency ccySettle = ndf.SettlementCurrency;
		if (provider.ValuationDate.isAfter(ndf.PaymentDate))
		{
		  return CurrencyAmount.zero(ccySettle);
		}
		Currency ccyOther = ndf.NonDeliverableCurrency;
		CurrencyAmount notionalSettle = ndf.SettlementCurrencyNotional;
		double agreedRate = ndf.AgreedFxRate.fxRate(ccySettle, ccyOther);
		double forwardRate = provider.fxIndexRates(ndf.Index).rate(ndf.Observation, ccySettle);
		double dfSettle = provider.discountFactor(ccySettle, ndf.PaymentDate);
		return notionalSettle.multipliedBy(dfSettle * (1d - agreedRate / forwardRate));
	  }

	  /// <summary>
	  /// Calculates the present value curve sensitivity of the NDF product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ndf">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedFxNdf ndf, RatesProvider provider)
	  {
		if (provider.ValuationDate.isAfter(ndf.PaymentDate))
		{
		  return PointSensitivities.empty();
		}
		Currency ccySettle = ndf.SettlementCurrency;
		Currency ccyOther = ndf.NonDeliverableCurrency;
		double notionalSettle = ndf.SettlementNotional;
		double agreedRate = ndf.AgreedFxRate.fxRate(ccySettle, ccyOther);
		double forwardRate = provider.fxIndexRates(ndf.Index).rate(ndf.Observation, ccySettle);
		double dfSettle = provider.discountFactor(ccySettle, ndf.PaymentDate);
		double ratio = agreedRate / forwardRate;
		double dscBar = (1d - ratio) * notionalSettle;
		PointSensitivityBuilder sensiDsc = provider.discountFactors(ccySettle).zeroRatePointSensitivity(ndf.PaymentDate).multipliedBy(dscBar);
		double forwardRateBar = dfSettle * notionalSettle * ratio / forwardRate;
		PointSensitivityBuilder sensiFx = provider.fxIndexRates(ndf.Index).ratePointSensitivity(ndf.Observation, ccySettle).withCurrency(ccySettle).multipliedBy(forwardRateBar);
		return sensiDsc.combinedWith(sensiFx).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure by discounting each payment in its own currency.
	  /// </summary>
	  /// <param name="ndf">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxNdf ndf, RatesProvider provider)
	  {
		if (provider.ValuationDate.isAfter(ndf.PaymentDate))
		{
		  return MultiCurrencyAmount.empty();
		}
		Currency ccySettle = ndf.SettlementCurrency;
		CurrencyAmount notionalSettle = ndf.SettlementCurrencyNotional;
		double dfSettle = provider.discountFactor(ccySettle, ndf.PaymentDate);
		Currency ccyOther = ndf.NonDeliverableCurrency;
		double agreedRate = ndf.AgreedFxRate.fxRate(ccySettle, ccyOther);
		double dfOther = provider.discountFactor(ccyOther, ndf.PaymentDate);
		return MultiCurrencyAmount.of(notionalSettle.multipliedBy(dfSettle)).plus(CurrencyAmount.of(ccyOther, -notionalSettle.Amount * agreedRate * dfOther));
	  }

	  /// <summary>
	  /// Calculates the current cash of the NDF product.
	  /// </summary>
	  /// <param name="ndf">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash of the product in the settlement currency </returns>
	  public virtual CurrencyAmount currentCash(ResolvedFxNdf ndf, RatesProvider provider)
	  {
		Currency ccySettle = ndf.SettlementCurrency;
		if (provider.ValuationDate.isEqual(ndf.PaymentDate))
		{
		  Currency ccyOther = ndf.NonDeliverableCurrency;
		  CurrencyAmount notionalSettle = ndf.SettlementCurrencyNotional;
		  double agreedRate = ndf.AgreedFxRate.fxRate(ccySettle, ccyOther);
		  double rate = provider.fxIndexRates(ndf.Index).rate(ndf.Observation, ccySettle);
		  return notionalSettle.multipliedBy(1d - agreedRate / rate);
		}
		return CurrencyAmount.zero(ccySettle);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forward exchange rate.
	  /// </summary>
	  /// <param name="ndf">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forward rate </returns>
	  public virtual FxRate forwardFxRate(ResolvedFxNdf ndf, RatesProvider provider)
	  {
		Currency ccySettle = ndf.SettlementCurrency;
		Currency ccyOther = ndf.NonDeliverableCurrency;
		double forwardRate = provider.fxIndexRates(ndf.Index).rate(ndf.Observation, ccySettle);
		return FxRate.of(ccySettle, ccyOther, forwardRate);
	  }

	}

}