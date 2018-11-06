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
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedIborFixingDeposit = com.opengamma.strata.product.deposit.ResolvedIborFixingDeposit;

	/// <summary>
	/// The methods associated to the pricing of Ibor fixing deposit by discounting.
	/// <para>
	/// This provides the ability to price <seealso cref="ResolvedIborFixingDeposit"/>. Those products are synthetic deposits
	/// which are used for curve calibration purposes; they should not be used as actual trades.
	/// </para>
	/// </summary>
	public class DiscountingIborFixingDepositProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingIborFixingDepositProductPricer DEFAULT = new DiscountingIborFixingDepositProductPricer();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public DiscountingIborFixingDepositProductPricer()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor fixing deposit product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedIborFixingDeposit deposit, RatesProvider provider)
	  {
		Currency currency = deposit.Currency;
		if (provider.ValuationDate.isAfter(deposit.EndDate))
		{
		  return CurrencyAmount.of(currency, 0.0d);
		}
		double forwardRate = this.forwardRate(deposit, provider);
		double discountFactor = provider.discountFactor(currency, deposit.EndDate);
		double fv = deposit.Notional * deposit.YearFraction * (deposit.FixedRate - forwardRate);
		double pv = discountFactor * fv;
		return CurrencyAmount.of(currency, pv);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the Ibor fixing product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedIborFixingDeposit deposit, RatesProvider provider)
	  {
		double forwardRate = this.forwardRate(deposit, provider);
		DiscountFactors discountFactors = provider.discountFactors(deposit.Currency);
		double discountFactor = discountFactors.discountFactor(deposit.EndDate);
		// sensitivity
		PointSensitivityBuilder sensiFwd = forwardRateSensitivity(deposit, provider).multipliedBy(-discountFactor * deposit.Notional * deposit.YearFraction);
		PointSensitivityBuilder sensiDsc = discountFactors.zeroRatePointSensitivity(deposit.EndDate).multipliedBy(deposit.Notional * deposit.YearFraction * (deposit.FixedRate - forwardRate));
		return sensiFwd.combinedWith(sensiDsc).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the deposit fair rate given the start and end time and the accrual factor.
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate </returns>
	  public virtual double parRate(ResolvedIborFixingDeposit deposit, RatesProvider provider)
	  {
		return forwardRate(deposit, provider);
	  }

	  /// <summary>
	  /// Calculates the deposit fair rate sensitivity to the curves.
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate curve sensitivity </returns>
	  public virtual PointSensitivities parRateSensitivity(ResolvedIborFixingDeposit deposit, RatesProvider provider)
	  {
		return forwardRateSensitivity(deposit, provider).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the spread to be added to the deposit rate to have a zero present value.
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedIborFixingDeposit deposit, RatesProvider provider)
	  {
		return forwardRate(deposit, provider) - deposit.FixedRate;
	  }

	  /// <summary>
	  /// Calculates the par spread curve sensitivity.
	  /// </summary>
	  /// <param name="deposit">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread curve sensitivity </returns>
	  public virtual PointSensitivities parSpreadSensitivity(ResolvedIborFixingDeposit deposit, RatesProvider provider)
	  {
		return forwardRateSensitivity(deposit, provider).build();
	  }

	  //-------------------------------------------------------------------------
	  // query the forward rate
	  private double forwardRate(ResolvedIborFixingDeposit product, RatesProvider provider)
	  {
		IborIndexRates rates = provider.iborIndexRates(product.FloatingRate.Index);
		// The IborFixingDeposit are fictitious instruments to anchor the beginning of the IborIndex forward curve.
		// By using the 'rateIgnoringTimeSeries' method (instead of 'rate') we ensure that only the forward curve is involved.
		return rates.rateIgnoringFixings(product.FloatingRate.Observation);
	  }

	  // query the forward rate sensitivity
	  private PointSensitivityBuilder forwardRateSensitivity(ResolvedIborFixingDeposit product, RatesProvider provider)
	  {
		IborIndexRates rates = provider.iborIndexRates(product.FloatingRate.Index);
		return rates.rateIgnoringFixingsPointSensitivity(product.FloatingRate.Observation);
	  }

	}

}