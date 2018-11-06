using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using NormalFormulaRepository = com.opengamma.strata.pricer.impl.option.NormalFormulaRepository;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;

	/// <summary>
	/// Pricer for swaption with par yield curve method of cash settlement in a normal model on the swap rate.
	/// <para>
	/// The swap underlying the swaption must have a fixed leg on which the forward rate is computed.
	/// The underlying swap must be single currency.
	/// </para>
	/// <para>
	/// The volatility parameters are not adjusted for the underlying swap convention.
	/// </para>
	/// <para>
	/// The value of the swaption after expiry is 0.
	/// For a swaption which already expired, negative number is returned by 
	/// <seealso cref="SwaptionVolatilities#relativeTime(ZonedDateTime)"/>.
	/// </para>
	/// </summary>
	public class NormalSwaptionCashParYieldProductPricer : VolatilitySwaptionCashParYieldProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly NormalSwaptionCashParYieldProductPricer DEFAULT = new NormalSwaptionCashParYieldProductPricer(DiscountingSwapProductPricer.DEFAULT);

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="swapPricer">  the pricer for <seealso cref="Swap"/> </param>
	  public NormalSwaptionCashParYieldProductPricer(DiscountingSwapProductPricer swapPricer) : base(swapPricer)
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the implied normal volatility from the present value of a swaption.
	  /// <para>
	  /// The guess volatility for the start of the root-finding process is 1%.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="dayCount">  the day-count used to estimate the time between valuation date and swaption expiry </param>
	  /// <param name="presentValue">  the present value of the swaption product </param>
	  /// <returns> the implied volatility associated with the present value </returns>
	  public virtual double impliedVolatilityFromPresentValue(ResolvedSwaption swaption, RatesProvider ratesProvider, DayCount dayCount, double presentValue)
	  {

		double sign = swaption.LongShort.sign();
		ArgChecker.isTrue(presentValue * sign > 0, "Present value sign must be in line with the option Long/Short flag ");
		validateSwaption(swaption);
		LocalDate valuationDate = ratesProvider.ValuationDate;
		LocalDate expiryDate = swaption.ExpiryDate;
		ArgChecker.isTrue(expiryDate.isAfter(valuationDate), "Expiry must be after valuation date to compute an implied volatility");
		double expiry = dayCount.yearFraction(valuationDate, expiryDate);
		ResolvedSwap underlying = swaption.Underlying;
		ResolvedSwapLeg fixedLeg = this.fixedLeg(underlying);
		double forward = SwapPricer.parRate(underlying, ratesProvider);
		double numeraire = calculateNumeraire(swaption, fixedLeg, forward, ratesProvider);
		double strike = calculateStrike(fixedLeg);
		PutCall putCall = PutCall.ofPut(fixedLeg.PayReceive.Receive);
		return NormalFormulaRepository.impliedVolatility(Math.Abs(presentValue), forward, strike, expiry, 0.01, numeraire, putCall);
	  }

	}

}