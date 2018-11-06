using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.cms
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using CmsPeriod = com.opengamma.strata.product.cms.CmsPeriod;
	using CmsPeriodType = com.opengamma.strata.product.cms.CmsPeriodType;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;

	/// <summary>
	///  Computes the price of a CMS coupon by simple forward estimation.
	///  <para>
	///  This is an overly simplistic approach to CMS coupon pricer. It is provided only for testing and comparison 
	///  purposes. It is not recommended to use this for valuation or risk management purposes.
	/// </para>
	/// </summary>
	public class DiscountingCmsPeriodPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingCmsPeriodPricer DEFAULT = new DiscountingCmsPeriodPricer(DiscountingSwapProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for the underlying swap.
	  /// </summary>
	  private readonly DiscountingSwapProductPricer swapPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="swapPricer">  the pricer for <seealso cref="ResolvedSwap"/> </param>
	  public DiscountingCmsPeriodPricer(DiscountingSwapProductPricer swapPricer)
	  {
		this.swapPricer = ArgChecker.notNull(swapPricer, "legPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value of CMS coupon by simple forward rate estimation.
	  /// </summary>
	  /// <param name="cmsPeriod">  the CMS </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(CmsPeriod cmsPeriod, RatesProvider provider)
	  {

		Currency ccy = cmsPeriod.Currency;
		LocalDate valuationDate = provider.ValuationDate;
		if (valuationDate.isAfter(cmsPeriod.PaymentDate))
		{
		  return CurrencyAmount.zero(ccy);
		}
		LocalDate fixingDate = cmsPeriod.FixingDate;
		double dfPayment = provider.discountFactor(ccy, cmsPeriod.PaymentDate);
		if (!fixingDate.isAfter(valuationDate))
		{ // Using fixing
		  double? fixedRate = provider.timeSeries(cmsPeriod.Index).get(fixingDate);
		  if (fixedRate.HasValue)
		  {
			double payoff = 0d;
			switch (cmsPeriod.CmsPeriodType)
			{
			  case CAPLET:
				payoff = Math.Max(fixedRate.Value - cmsPeriod.Strike, 0d);
				break;
			  case FLOORLET:
				payoff = Math.Max(cmsPeriod.Strike - fixedRate.Value, 0d);
				break;
			  case COUPON:
				payoff = fixedRate.Value;
				break;
			  default:
				throw new System.ArgumentException("unsupported CMS type");
			}
			return CurrencyAmount.of(ccy, payoff * dfPayment * cmsPeriod.Notional * cmsPeriod.YearFraction);
		  }
		  else if (fixingDate.isBefore(valuationDate))
		  {
			throw new System.ArgumentException(Messages.format("Unable to get fixing for {} on date {}, no time-series supplied", cmsPeriod.Index, fixingDate));
		  }
		}
		if (!cmsPeriod.CmsPeriodType.Equals(CmsPeriodType.COUPON))
		{
		  throw new System.ArgumentException("Unable to price cap or floor in this pricer");
		}
		// Using forward
		ResolvedSwap swap = cmsPeriod.UnderlyingSwap;
		double forward = swapPricer.parRate(swap, provider);
		return CurrencyAmount.of(ccy, forward * dfPayment * cmsPeriod.Notional * cmsPeriod.YearFraction);
	  }

	  /// <summary>
	  /// Computes the forward rate associated to the swap underlying the CMS period.
	  /// <para>
	  /// Returns a value only if the period has not fixed yet. If the fixing date is on or before the valuation date,
	  /// an <seealso cref="IllegalArgumentException"/> is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsPeriod">  the CMS </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forward rate </returns>
	  public virtual double forwardRate(CmsPeriod cmsPeriod, RatesProvider provider)
	  {

		LocalDate fixingDate = cmsPeriod.FixingDate;
		LocalDate valuationDate = provider.ValuationDate;
		if (!fixingDate.isAfter(valuationDate))
		{ // Using fixing
		  throw new System.ArgumentException("Forward rate is availaible only for valuation date after the fixing date");
		}
		ResolvedSwap swap = cmsPeriod.UnderlyingSwap;
		return swapPricer.parRate(swap, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value curve sensitivity by simple forward rate estimation.
	  /// </summary>
	  /// <param name="cmsPeriod">  the CMS </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(CmsPeriod cmsPeriod, RatesProvider provider)
	  {

		Currency ccy = cmsPeriod.Currency;
		LocalDate valuationDate = provider.ValuationDate;
		if (valuationDate.isAfter(cmsPeriod.PaymentDate))
		{
		  return PointSensitivityBuilder.none();
		}
		LocalDate fixingDate = cmsPeriod.FixingDate;
		double dfPayment = provider.discountFactor(ccy, cmsPeriod.PaymentDate);
		if (!fixingDate.isAfter(valuationDate))
		{ // Using fixing
		  double? fixedRate = provider.timeSeries(cmsPeriod.Index).get(fixingDate);
		  if (fixedRate.HasValue)
		  {
			double payoff = 0d;
			switch (cmsPeriod.CmsPeriodType)
			{
			  case CAPLET:
				payoff = Math.Max(fixedRate.Value - cmsPeriod.Strike, 0d);
				break;
			  case FLOORLET:
				payoff = Math.Max(cmsPeriod.Strike - fixedRate.Value, 0d);
				break;
			  case COUPON:
				payoff = fixedRate.Value;
				break;
			  default:
				throw new System.ArgumentException("unsupported CMS type");
			}
			return provider.discountFactors(ccy).zeroRatePointSensitivity(cmsPeriod.PaymentDate).multipliedBy(payoff * cmsPeriod.Notional * cmsPeriod.YearFraction);
		  }
		  else if (fixingDate.isBefore(valuationDate))
		  {
			throw new System.ArgumentException(Messages.format("Unable to get fixing for {} on date {}, no time-series supplied", cmsPeriod.Index, fixingDate));
		  }
		}
		if (!cmsPeriod.CmsPeriodType.Equals(CmsPeriodType.COUPON))
		{
		  throw new System.ArgumentException("Unable to price cap or floor in this pricer");
		}
		// Using forward
		ResolvedSwap swap = cmsPeriod.UnderlyingSwap;
		ZeroRateSensitivity dfPaymentdr = provider.discountFactors(ccy).zeroRatePointSensitivity(cmsPeriod.PaymentDate);
		double forward = swapPricer.parRate(swap, provider);
		PointSensitivityBuilder forwardSensi = swapPricer.parRateSensitivity(swap, provider);
		return forwardSensi.multipliedBy(dfPayment).combinedWith(dfPaymentdr.multipliedBy(forward)).multipliedBy(cmsPeriod.Notional * cmsPeriod.YearFraction);
	  }

	}

}