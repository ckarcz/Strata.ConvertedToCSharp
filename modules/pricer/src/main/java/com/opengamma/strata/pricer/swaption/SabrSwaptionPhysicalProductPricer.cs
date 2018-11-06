using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.ALPHA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.BETA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.NU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.RHO;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;

	/// <summary>
	/// Pricer for swaption with physical settlement in SABR model on the swap rate.
	/// <para>
	/// The swap underlying the swaption must have a fixed leg on which the forward rate is computed.
	/// The underlying swap must be single currency.
	/// </para>
	/// <para>
	/// The volatility parameters are not adjusted for the underlying swap convention.
	/// The volatilities from the provider are taken as such.
	/// </para>
	/// <para>
	/// The value of the swaption after expiry is 0. For a swaption which already expired, negative number is returned by 
	/// the method, <seealso cref="SabrSwaptionVolatilities#relativeTime(ZonedDateTime)"/>.
	/// </para>
	/// </summary>
	public class SabrSwaptionPhysicalProductPricer : VolatilitySwaptionPhysicalProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly SabrSwaptionPhysicalProductPricer DEFAULT = new SabrSwaptionPhysicalProductPricer(DiscountingSwapProductPricer.DEFAULT);

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="swapPricer">  the pricer for <seealso cref="Swap"/> </param>
	  public SabrSwaptionPhysicalProductPricer(DiscountingSwapProductPricer swapPricer) : base(swapPricer)
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the swaption product to the rate curves.
	  /// <para>
	  /// The present value sensitivity is computed in a "sticky model parameter" style, i.e. the sensitivity to the 
	  /// curve nodes with the SABR model parameters unchanged. This sensitivity does not include a potential 
	  /// re-calibration of the model parameters to the raw market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the rate curves </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRatesStickyModel(ResolvedSwaption swaption, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		validate(swaption, ratesProvider, swaptionVolatilities);
		ZonedDateTime expiryDateTime = swaption.Expiry;
		double expiry = swaptionVolatilities.relativeTime(expiryDateTime);
		ResolvedSwap underlying = swaption.Underlying;
		ResolvedSwapLeg fixedLeg = this.fixedLeg(underlying);
		if (expiry < 0d)
		{ // Option has expired already
		  return PointSensitivityBuilder.none();
		}
		double forward = SwapPricer.parRate(underlying, ratesProvider);
		double pvbp = SwapPricer.LegPricer.pvbp(fixedLeg, ratesProvider);
		double strike = SwapPricer.LegPricer.couponEquivalent(fixedLeg, ratesProvider, pvbp);
		double tenor = swaptionVolatilities.tenor(fixedLeg.StartDate, fixedLeg.EndDate);
		double shift = swaptionVolatilities.shift(expiry, tenor);
		ValueDerivatives volatilityAdj = swaptionVolatilities.volatilityAdjoint(expiry, tenor, strike, forward);
		bool isCall = fixedLeg.PayReceive.Pay;
		// Payer at strike is exercise when rate > strike, i.e. call on rate
		// Backward sweep
		PointSensitivityBuilder pvbpDr = SwapPricer.LegPricer.pvbpSensitivity(fixedLeg, ratesProvider);
		PointSensitivityBuilder forwardDr = SwapPricer.parRateSensitivity(underlying, ratesProvider);
		double shiftedForward = forward + shift;
		double shiftedStrike = strike + shift;
		double price = BlackFormulaRepository.price(shiftedForward, shiftedStrike, expiry, volatilityAdj.Value, isCall);
		double delta = BlackFormulaRepository.delta(shiftedForward, shiftedStrike, expiry, volatilityAdj.Value, isCall);
		double vega = BlackFormulaRepository.vega(shiftedForward, shiftedStrike, expiry, volatilityAdj.Value);
		double sign = swaption.LongShort.sign();
		return pvbpDr.multipliedBy(price * sign * Math.Sign(pvbp)).combinedWith(forwardDr.multipliedBy((delta + vega * volatilityAdj.getDerivative(0)) * Math.Abs(pvbp) * sign));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity to the SABR model parameters of the swaption product.
	  /// <para>
	  /// The sensitivity of the present value to the SABR model parameters, alpha, beta, rho and nu.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the SABR model parameters </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsSabr(ResolvedSwaption swaption, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		validate(swaption, ratesProvider, swaptionVolatilities);
		double expiry = swaptionVolatilities.relativeTime(swaption.Expiry);
		ResolvedSwap underlying = swaption.Underlying;
		ResolvedSwapLeg fixedLeg = this.fixedLeg(underlying);
		double tenor = swaptionVolatilities.tenor(fixedLeg.StartDate, fixedLeg.EndDate);
		double shift = swaptionVolatilities.shift(expiry, tenor);
		double pvbp = SwapPricer.LegPricer.pvbp(fixedLeg, ratesProvider);
		double strike = SwapPricer.LegPricer.couponEquivalent(fixedLeg, ratesProvider, pvbp);
		if (expiry < 0d)
		{ // Option has expired already
		  return PointSensitivityBuilder.none();
		}
		double forward = SwapPricer.parRate(underlying, ratesProvider);
		double volatility = swaptionVolatilities.volatility(expiry, tenor, strike, forward);
		DoubleArray derivative = swaptionVolatilities.volatilityAdjoint(expiry, tenor, strike, forward).Derivatives;
		// Backward sweep
		double vega = Math.Abs(pvbp) * BlackFormulaRepository.vega(forward + shift, strike + shift, expiry, volatility) * swaption.LongShort.sign();
		// sensitivities
		Currency ccy = fixedLeg.Currency;
		SwaptionVolatilitiesName name = swaptionVolatilities.Name;
		return PointSensitivityBuilder.of(SwaptionSabrSensitivity.of(name, expiry, tenor, ALPHA, ccy, vega * derivative.get(2)), SwaptionSabrSensitivity.of(name, expiry, tenor, BETA, ccy, vega * derivative.get(3)), SwaptionSabrSensitivity.of(name, expiry, tenor, RHO, ccy, vega * derivative.get(4)), SwaptionSabrSensitivity.of(name, expiry, tenor, NU, ccy, vega * derivative.get(5)));
	  }

	}

}