/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.deposit
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedTermDeposit = com.opengamma.strata.product.deposit.ResolvedTermDeposit;

	/// <summary>
	/// The methods associated to the pricing of term deposit by discounting.
	/// <para>
	/// This provides the ability to price <seealso cref="ResolvedTermDeposit"/>.
	/// </para>
	/// </summary>
	public class DiscountingTermDepositProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingTermDepositProductPricer DEFAULT = new DiscountingTermDepositProductPricer();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public DiscountingTermDepositProductPricer()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value by discounting the final cash flow (nominal + interest)
	  /// and the initial payment (initial amount).
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedTermDeposit deposit, RatesProvider provider)
	  {
		Currency currency = deposit.Currency;
		if (provider.ValuationDate.isAfter(deposit.EndDate))
		{
		  return CurrencyAmount.of(currency, 0.0d);
		}
		DiscountFactors discountFactors = provider.discountFactors(currency);
		double dfStart = discountFactors.discountFactor(deposit.StartDate);
		double dfEnd = discountFactors.discountFactor(deposit.EndDate);
		double pvStart = initialAmount(deposit, provider) * dfStart;
		double pvEnd = (deposit.Notional + deposit.Interest) * dfEnd;
		double pv = pvEnd - pvStart;
		return CurrencyAmount.of(currency, pv);
	  }

	  // the initial amount is the same as the principal, but zero if the start date has passed
	  // the caller must negate the result of this method if required
	  private double initialAmount(ResolvedTermDeposit deposit, RatesProvider provider)
	  {
		return provider.ValuationDate.isAfter(deposit.StartDate) ? 0d : deposit.Notional;
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity by discounting the final cash flow (nominal + interest)
	  /// and the initial payment (initial amount).
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedTermDeposit deposit, RatesProvider provider)
	  {
		Currency currency = deposit.Currency;
		// backward sweep
		double dfEndBar = deposit.Notional + deposit.Interest;
		double dfStartBar = -initialAmount(deposit, provider);
		// sensitivity
		DiscountFactors discountFactors = provider.discountFactors(currency);
		PointSensitivityBuilder sensStart = discountFactors.zeroRatePointSensitivity(deposit.StartDate).multipliedBy(dfStartBar);
		PointSensitivityBuilder sensEnd = discountFactors.zeroRatePointSensitivity(deposit.EndDate).multipliedBy(dfEndBar);
		return sensStart.combinedWith(sensEnd).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the deposit fair rate given the start and end time and the accrual factor.
	  /// <para>
	  /// When the deposit has already started the number may not be meaningful as the remaining period
	  /// is not in line with the accrual factor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate </returns>
	  public virtual double parRate(ResolvedTermDeposit deposit, RatesProvider provider)
	  {
		Currency currency = deposit.Currency;
		DiscountFactors discountFactors = provider.discountFactors(currency);
		double dfStart = discountFactors.discountFactor(deposit.StartDate);
		double dfEnd = discountFactors.discountFactor(deposit.EndDate);
		double accrualFactor = deposit.YearFraction;
		return (dfStart / dfEnd - 1d) / accrualFactor;
	  }

	  /// <summary>
	  /// Calculates the par rate curve sensitivity.
	  /// <para>
	  /// The calculation is based on both of initial and final payments.
	  /// Thus the number resulting may not be meaningful when deposit has already started and only the final
	  /// payment remains (no initial payment).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate curve sensitivity </returns>
	  public virtual PointSensitivities parRateSensitivity(ResolvedTermDeposit deposit, RatesProvider provider)
	  {
		return parSpreadSensitivity(deposit, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the spread to be added to the deposit rate to have a zero present value.
	  /// <para>
	  /// The calculation is based on both the initial and final payments.
	  /// Thus the resulting number may not be meaningful when deposit has already started and only the final
	  /// payment remains (no initial payment).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedTermDeposit deposit, RatesProvider provider)
	  {
		double parRate = this.parRate(deposit, provider);
		return parRate - deposit.Rate;
	  }

	  /// <summary>
	  /// Calculates the par spread curve sensitivity.
	  /// <para>
	  /// The calculation is based on both of initial and final payments.
	  /// Thus the number resulting may not be meaningful when deposit has already started and only the final
	  /// payment remains (no initial payment).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread curve sensitivity </returns>
	  public virtual PointSensitivities parSpreadSensitivity(ResolvedTermDeposit deposit, RatesProvider provider)
	  {
		Currency currency = deposit.Currency;
		double accrualFactorInv = 1d / deposit.YearFraction;
		double dfStart = provider.discountFactor(currency, deposit.StartDate);
		double dfEndInv = 1d / provider.discountFactor(currency, deposit.EndDate);
		DiscountFactors discountFactors = provider.discountFactors(currency);
		PointSensitivityBuilder sensStart = discountFactors.zeroRatePointSensitivity(deposit.StartDate).multipliedBy(dfEndInv * accrualFactorInv);
		PointSensitivityBuilder sensEnd = discountFactors.zeroRatePointSensitivity(deposit.EndDate).multipliedBy(-dfStart * dfEndInv * dfEndInv * accrualFactorInv);
		return sensStart.combinedWith(sensEnd).build();
	  }

	}

}