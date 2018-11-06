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
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using SwaptionVolatilities = com.opengamma.strata.pricer.swaption.SwaptionVolatilities;
	using CmsPeriod = com.opengamma.strata.product.cms.CmsPeriod;
	using CmsPeriodType = com.opengamma.strata.product.cms.CmsPeriodType;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;

	/// <summary>
	///  Computes the price of a CMS coupon in a constant log-normal volatility set-up.
	///  <para>
	///  Reference: Brotherton-Ratcliffe, R. and Iben, B. (1997). Advanced Strategies in financial Risk Management, 
	///    Chapter Yield Curve Application of Swap Products. New York Institute of Finance.
	///  OpenGamma implementation note: Pricing of CMS by replication and other approaches, Version 2.1, May 2016.
	/// </para>
	/// </summary>
	public sealed class BlackFlatCmsPeriodPricer
	{

	  /// <summary>
	  /// Pricer for the underlying swap.
	  /// </summary>
	  private readonly DiscountingSwapProductPricer swapPricer;

	  /* Small parameter below which a value is regarded as 0. */
	  internal const double EPS = 1.0E-4;

	  /// <summary>
	  /// Obtains the pricer.
	  /// </summary>
	  /// <param name="swapPricer">  the pricer for underlying swap </param>
	  /// <returns> the pricer </returns>
	  public static BlackFlatCmsPeriodPricer of(DiscountingSwapProductPricer swapPricer)
	  {
		return new BlackFlatCmsPeriodPricer(swapPricer);
	  }

	  private BlackFlatCmsPeriodPricer(DiscountingSwapProductPricer swapPricer)
	  {
		this.swapPricer = ArgChecker.notNull(swapPricer, "swapPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value by replication in SABR framework with extrapolation on the right.
	  /// </summary>
	  /// <param name="cmsPeriod">  the CMS </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value </returns>
	  public CurrencyAmount presentValue(CmsPeriod cmsPeriod, RatesProvider provider, SwaptionVolatilities swaptionVolatilities)
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
		SwapIndex index = cmsPeriod.Index;
		ResolvedSwap swap = cmsPeriod.UnderlyingSwap;
		ResolvedSwapLeg fixedLeg = swap.getLegs(SwapLegType.FIXED).get(0);
		int nbFixedPaymentYear = (int) (long)Math.Round(1d / ((RatePaymentPeriod) fixedLeg.PaymentPeriods.get(0)).AccrualPeriods.get(0).YearFraction, MidpointRounding.AwayFromZero);
		int nbFixedPeriod = fixedLeg.PaymentPeriods.size();
		double forward = swapPricer.parRate(swap, provider);
		double tenor = swaptionVolatilities.tenor(swap.StartDate, swap.EndDate);
		double expiryTime = swaptionVolatilities.relativeTime(fixingDate.atTime(index.FixingTime).atZone(index.FixingZone));
		double volatility = swaptionVolatilities.volatility(expiryTime, tenor, forward, forward);
		ValueDerivatives annuityDerivatives = swapPricer.LegPricer.annuityCash2(nbFixedPaymentYear, nbFixedPeriod, volatility);
		double forwardAdjustment = -0.5 * forward * forward * volatility * volatility * expiryTime * annuityDerivatives.getDerivative(1) / annuityDerivatives.getDerivative(0);
		return CurrencyAmount.of(ccy, (forward + forwardAdjustment) * dfPayment * cmsPeriod.Notional * cmsPeriod.YearFraction);
	  }

	}

}