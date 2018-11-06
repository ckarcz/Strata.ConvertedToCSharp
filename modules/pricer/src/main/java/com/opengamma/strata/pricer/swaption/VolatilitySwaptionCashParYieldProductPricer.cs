using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using SettlementType = com.opengamma.strata.product.common.SettlementType;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;
	using CashSwaptionSettlement = com.opengamma.strata.product.swaption.CashSwaptionSettlement;
	using CashSwaptionSettlementMethod = com.opengamma.strata.product.swaption.CashSwaptionSettlementMethod;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;

	/// <summary>
	/// Pricer for swaption with par yield curve method of cash settlement based on volatilities.
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
	public class VolatilitySwaptionCashParYieldProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly VolatilitySwaptionCashParYieldProductPricer DEFAULT = new VolatilitySwaptionCashParYieldProductPricer(DiscountingSwapProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="SwapProduct"/>. 
	  /// </summary>
	  private readonly DiscountingSwapProductPricer swapPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="swapPricer">  the pricer for <seealso cref="Swap"/> </param>
	  public VolatilitySwaptionCashParYieldProductPricer(DiscountingSwapProductPricer swapPricer)
	  {
		this.swapPricer = ArgChecker.notNull(swapPricer, "swapPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the swap pricer.
	  /// </summary>
	  /// <returns> the swap pricer </returns>
	  protected internal virtual DiscountingSwapProductPricer SwapPricer
	  {
		  get
		  {
			return swapPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the swaption.
	  /// <para>
	  /// The result is expressed using the currency of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		validate(swaption, ratesProvider, swaptionVolatilities);
		double expiry = swaptionVolatilities.relativeTime(swaption.Expiry);
		ResolvedSwap underlying = swaption.Underlying;
		ResolvedSwapLeg fixedLeg = this.fixedLeg(underlying);
		if (expiry < 0d)
		{ // Option has expired already
		  return CurrencyAmount.of(fixedLeg.Currency, 0d);
		}
		double forward = swapPricer.parRate(underlying, ratesProvider);
		double numeraire = calculateNumeraire(swaption, fixedLeg, forward, ratesProvider);
		double strike = calculateStrike(fixedLeg);
		double tenor = swaptionVolatilities.tenor(fixedLeg.StartDate, fixedLeg.EndDate);
		double volatility = swaptionVolatilities.volatility(expiry, tenor, strike, forward);
		PutCall putCall = PutCall.ofPut(fixedLeg.PayReceive.Receive);
		double price = numeraire * swaptionVolatilities.price(expiry, tenor, putCall, strike, forward, volatility);
		return CurrencyAmount.of(fixedLeg.Currency, price * swaption.LongShort.sign());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the currency exposure of the swaption.
	  /// <para>
	  /// This is equivalent to the present value of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		return MultiCurrencyAmount.of(presentValue(swaption, ratesProvider, swaptionVolatilities));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the implied volatility of the swaption.
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the implied volatility associated with the swaption </returns>
	  public virtual double impliedVolatility(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		validate(swaption, ratesProvider, swaptionVolatilities);
		double expiry = swaptionVolatilities.relativeTime(swaption.Expiry);
		ResolvedSwap underlying = swaption.Underlying;
		ResolvedSwapLeg fixedLeg = this.fixedLeg(underlying);
		ArgChecker.isTrue(expiry >= 0d, "Option must be before expiry to compute an implied volatility");
		double forward = SwapPricer.parRate(underlying, ratesProvider);
		double strike = calculateStrike(fixedLeg);
		double tenor = swaptionVolatilities.tenor(fixedLeg.StartDate, fixedLeg.EndDate);
		return swaptionVolatilities.volatility(expiry, tenor, strike, forward);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value delta of the swaption.
	  /// <para>
	  /// The present value delta is given by {@code pvbp * priceDelta} where {@code priceDelta}
	  /// is the first derivative of the price with respect to forward.
	  /// </para>
	  /// <para>
	  /// The result is expressed using the currency of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the present value delta of the swaption </returns>
	  public virtual CurrencyAmount presentValueDelta(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		validate(swaption, ratesProvider, swaptionVolatilities);
		double expiry = swaptionVolatilities.relativeTime(swaption.Expiry);
		ResolvedSwap underlying = swaption.Underlying;
		ResolvedSwapLeg fixedLeg = this.fixedLeg(underlying);
		if (expiry < 0d)
		{ // Option has expired already
		  return CurrencyAmount.of(fixedLeg.Currency, 0d);
		}
		double forward = SwapPricer.parRate(underlying, ratesProvider);
		double numeraire = calculateNumeraire(swaption, fixedLeg, forward, ratesProvider);
		double strike = calculateStrike(fixedLeg);
		double tenor = swaptionVolatilities.tenor(fixedLeg.StartDate, fixedLeg.EndDate);
		double volatility = swaptionVolatilities.volatility(expiry, tenor, strike, forward);
		PutCall putCall = PutCall.ofPut(fixedLeg.PayReceive.Receive);
		double delta = numeraire * swaptionVolatilities.priceDelta(expiry, tenor, putCall, strike, forward, volatility);
		return CurrencyAmount.of(fixedLeg.Currency, delta * swaption.LongShort.sign());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value gamma of the swaption.
	  /// <para>
	  /// The present value gamma is given by {@code pvbp * priceGamma} where {@code priceGamma}
	  /// is the second derivative of the price with respect to forward.
	  /// </para>
	  /// <para>
	  /// The result is expressed using the currency of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the present value gamma of the swaption </returns>
	  public virtual CurrencyAmount presentValueGamma(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		validate(swaption, ratesProvider, swaptionVolatilities);
		double expiry = swaptionVolatilities.relativeTime(swaption.Expiry);
		ResolvedSwap underlying = swaption.Underlying;
		ResolvedSwapLeg fixedLeg = this.fixedLeg(underlying);
		if (expiry < 0d)
		{ // Option has expired already
		  return CurrencyAmount.of(fixedLeg.Currency, 0d);
		}
		double forward = SwapPricer.parRate(underlying, ratesProvider);
		double numeraire = calculateNumeraire(swaption, fixedLeg, forward, ratesProvider);
		double strike = calculateStrike(fixedLeg);
		double tenor = swaptionVolatilities.tenor(fixedLeg.StartDate, fixedLeg.EndDate);
		double volatility = swaptionVolatilities.volatility(expiry, tenor, strike, forward);
		PutCall putCall = PutCall.ofPut(fixedLeg.PayReceive.Receive);
		double gamma = numeraire * swaptionVolatilities.priceGamma(expiry, tenor, putCall, strike, forward, volatility);
		return CurrencyAmount.of(fixedLeg.Currency, gamma * swaption.LongShort.sign());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the swaption.
	  /// <para>
	  /// The present value theta is given by {@code pvbp * priceTheta} where {@code priceTheta}
	  /// is the minus of the price sensitivity to {@code timeToExpiry}.
	  /// </para>
	  /// <para>
	  /// The result is expressed using the currency of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the present value theta of the swaption </returns>
	  public virtual CurrencyAmount presentValueTheta(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		validate(swaption, ratesProvider, swaptionVolatilities);
		double expiry = swaptionVolatilities.relativeTime(swaption.Expiry);
		ResolvedSwap underlying = swaption.Underlying;
		ResolvedSwapLeg fixedLeg = this.fixedLeg(underlying);
		if (expiry < 0d)
		{ // Option has expired already
		  return CurrencyAmount.of(fixedLeg.Currency, 0d);
		}
		double forward = SwapPricer.parRate(underlying, ratesProvider);
		double numeraire = calculateNumeraire(swaption, fixedLeg, forward, ratesProvider);
		double strike = calculateStrike(fixedLeg);
		double tenor = swaptionVolatilities.tenor(fixedLeg.StartDate, fixedLeg.EndDate);
		double volatility = swaptionVolatilities.volatility(expiry, tenor, strike, forward);
		PutCall putCall = PutCall.ofPut(fixedLeg.PayReceive.Receive);
		double theta = numeraire * swaptionVolatilities.priceTheta(expiry, tenor, putCall, strike, forward, volatility);
		return CurrencyAmount.of(fixedLeg.Currency, theta * swaption.LongShort.sign());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the swaption to the rate curves.
	  /// <para>
	  /// The present value sensitivity is computed in a "sticky strike" style, i.e. the sensitivity to the 
	  /// curve nodes with the volatility at the swaption strike unchanged. This sensitivity does not include a potential 
	  /// change of volatility due to the implicit change of forward rate or moneyness.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the rate curves </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRatesStickyStrike(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		validate(swaption, ratesProvider, swaptionVolatilities);
		double expiry = swaptionVolatilities.relativeTime(swaption.Expiry);
		ResolvedSwap underlying = swaption.Underlying;
		ResolvedSwapLeg fixedLeg = this.fixedLeg(underlying);
		if (expiry < 0d)
		{ // Option has expired already
		  return PointSensitivityBuilder.none();
		}
		double forward = SwapPricer.parRate(underlying, ratesProvider);
		ValueDerivatives annuityDerivative = SwapPricer.LegPricer.annuityCashDerivative(fixedLeg, forward);
		double annuityCash = annuityDerivative.Value;
		double annuityCashDr = annuityDerivative.getDerivative(0);
		LocalDate settlementDate = ((CashSwaptionSettlement) swaption.SwaptionSettlement).SettlementDate;
		double discountSettle = ratesProvider.discountFactor(fixedLeg.Currency, settlementDate);
		double strike = calculateStrike(fixedLeg);
		double tenor = swaptionVolatilities.tenor(fixedLeg.StartDate, fixedLeg.EndDate);
		double volatility = swaptionVolatilities.volatility(expiry, tenor, strike, forward);
		PutCall putCall = PutCall.ofPut(fixedLeg.PayReceive.Receive);
		double price = swaptionVolatilities.price(expiry, tenor, putCall, strike, forward, volatility);
		double delta = swaptionVolatilities.priceDelta(expiry, tenor, putCall, strike, forward, volatility);
		// Backward sweep
		PointSensitivityBuilder forwardSensi = SwapPricer.parRateSensitivity(underlying, ratesProvider);
		PointSensitivityBuilder discountSettleSensi = ratesProvider.discountFactors(fixedLeg.Currency).zeroRatePointSensitivity(settlementDate);
		double sign = swaption.LongShort.sign();
		return forwardSensi.multipliedBy(sign * discountSettle * (annuityCash * delta + annuityCashDr * price)).combinedWith(discountSettleSensi.multipliedBy(sign * annuityCash * price));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity to the implied volatility of the swaption.
	  /// <para>
	  /// The sensitivity to the implied volatility is also called vega.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the volatility </returns>
	  public virtual SwaptionSensitivity presentValueSensitivityModelParamsVolatility(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		validate(swaption, ratesProvider, swaptionVolatilities);
		double expiry = swaptionVolatilities.relativeTime(swaption.Expiry);
		ResolvedSwap underlying = swaption.Underlying;
		ResolvedSwapLeg fixedLeg = this.fixedLeg(underlying);
		double tenor = swaptionVolatilities.tenor(fixedLeg.StartDate, fixedLeg.EndDate);
		double strike = calculateStrike(fixedLeg);
		if (expiry < 0d)
		{ // Option has expired already
		  return SwaptionSensitivity.of(swaptionVolatilities.Name, expiry, tenor, strike, 0d, fixedLeg.Currency, 0d);
		}
		double forward = SwapPricer.parRate(underlying, ratesProvider);
		double numeraire = calculateNumeraire(swaption, fixedLeg, forward, ratesProvider);
		double volatility = swaptionVolatilities.volatility(expiry, tenor, strike, forward);
		PutCall putCall = PutCall.ofPut(fixedLeg.PayReceive.Receive);
		double vega = numeraire * swaptionVolatilities.priceVega(expiry, tenor, putCall, strike, forward, volatility);
		return SwaptionSensitivity.of(swaptionVolatilities.Name, expiry, tenor, strike, forward, fixedLeg.Currency, vega * swaption.LongShort.sign());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the numeraire, used to multiply the results.
	  /// </summary>
	  /// <param name="swaption">  the swap </param>
	  /// <param name="fixedLeg">  the fixed leg </param>
	  /// <param name="forward">  the forward rate </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the numeraire </returns>
	  protected internal virtual double calculateNumeraire(ResolvedSwaption swaption, ResolvedSwapLeg fixedLeg, double forward, RatesProvider ratesProvider)
	  {

		double annuityCash = swapPricer.LegPricer.annuityCash(fixedLeg, forward);
		CashSwaptionSettlement cashSettlement = (CashSwaptionSettlement) swaption.SwaptionSettlement;
		double discountSettle = ratesProvider.discountFactor(fixedLeg.Currency, cashSettlement.SettlementDate);
		return Math.Abs(annuityCash * discountSettle);
	  }

	  /// <summary>
	  /// Calculates the strike.
	  /// </summary>
	  /// <param name="fixedLeg">  the fixed leg </param>
	  /// <returns> the strike </returns>
	  protected internal virtual double calculateStrike(ResolvedSwapLeg fixedLeg)
	  {
		SwapPaymentPeriod paymentPeriod = fixedLeg.PaymentPeriods.get(0);
		ArgChecker.isTrue(paymentPeriod is RatePaymentPeriod, "Payment period must be RatePaymentPeriod");
		RatePaymentPeriod ratePaymentPeriod = (RatePaymentPeriod) paymentPeriod;
		// compounding is caught when par rate is computed
		RateComputation rateComputation = ratePaymentPeriod.AccrualPeriods.get(0).RateComputation;
		ArgChecker.isTrue(rateComputation is FixedRateComputation, "Swap leg must be fixed leg");
		return ((FixedRateComputation) rateComputation).Rate;
	  }

	  /// <summary>
	  /// Checks that there is exactly one fixed leg and returns it.
	  /// </summary>
	  /// <param name="swap">  the swap </param>
	  /// <returns> the fixed leg </returns>
	  protected internal virtual ResolvedSwapLeg fixedLeg(ResolvedSwap swap)
	  {
		ArgChecker.isFalse(swap.CrossCurrency, "Swap must be single currency");
		// find fixed leg
		IList<ResolvedSwapLeg> fixedLegs = swap.getLegs(SwapLegType.FIXED);
		if (fixedLegs.Count == 0)
		{
		  throw new System.ArgumentException("Swap must contain a fixed leg");
		}
		return fixedLegs[0];
	  }

	  /// <summary>
	  /// Validates that the rates and volatilities providers are coherent
	  /// and that the swaption is single currency cash par-yield.
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  protected internal virtual void validate(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		ArgChecker.isTrue(swaptionVolatilities.ValuationDate.Equals(ratesProvider.ValuationDate), "Volatility and rate data must be for the same date");
		validateSwaption(swaption);
	  }

	  /// <summary>
	  /// Validates that the swaption is single currency cash par-yield.
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  protected internal virtual void validateSwaption(ResolvedSwaption swaption)
	  {
		ArgChecker.isFalse(swaption.Underlying.CrossCurrency, "Underlying swap must be single currency");
		ArgChecker.isTrue(swaption.SwaptionSettlement.SettlementType.Equals(SettlementType.CASH), "Swaption must be cash settlement");
		CashSwaptionSettlement cashSettle = (CashSwaptionSettlement) swaption.SwaptionSettlement;
		ArgChecker.isTrue(cashSettle.Method.Equals(CashSwaptionSettlementMethod.PAR_YIELD), "Cash settlement method must be par yield");
	  }

	}

}