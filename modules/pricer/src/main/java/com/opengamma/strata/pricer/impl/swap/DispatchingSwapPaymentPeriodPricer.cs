/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.swap
{
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwapPaymentPeriodPricer = com.opengamma.strata.pricer.swap.SwapPaymentPeriodPricer;
	using KnownAmountSwapPaymentPeriod = com.opengamma.strata.product.swap.KnownAmountSwapPaymentPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;

	/// <summary>
	/// Pricer implementation for payment periods using multiple dispatch.
	/// <para>
	/// Dispatches the request to the correct implementation.
	/// </para>
	/// </summary>
	public class DispatchingSwapPaymentPeriodPricer : SwapPaymentPeriodPricer<SwapPaymentPeriod>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DispatchingSwapPaymentPeriodPricer DEFAULT = new DispatchingSwapPaymentPeriodPricer(DiscountingRatePaymentPeriodPricer.DEFAULT, DiscountingKnownAmountPaymentPeriodPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="RatePaymentPeriod"/>.
	  /// </summary>
	  private readonly SwapPaymentPeriodPricer<RatePaymentPeriod> ratePaymentPeriodPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="KnownAmountSwapPaymentPeriod"/>.
	  /// </summary>
	  private readonly SwapPaymentPeriodPricer<KnownAmountSwapPaymentPeriod> knownAmountPaymentPeriodPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="ratePaymentPeriodPricer">  the pricer for <seealso cref="RatePaymentPeriod"/> </param>
	  /// <param name="knownAmountPaymentPeriodPricer">  the pricer for <seealso cref="KnownAmountSwapPaymentPeriod"/> </param>
	  public DispatchingSwapPaymentPeriodPricer(SwapPaymentPeriodPricer<RatePaymentPeriod> ratePaymentPeriodPricer, SwapPaymentPeriodPricer<KnownAmountSwapPaymentPeriod> knownAmountPaymentPeriodPricer)
	  {
		this.ratePaymentPeriodPricer = ArgChecker.notNull(ratePaymentPeriodPricer, "ratePaymentPeriodPricer");
		this.knownAmountPaymentPeriodPricer = ArgChecker.notNull(knownAmountPaymentPeriodPricer, "knownAmountPaymentPeriodPricer");
	  }

	  //-------------------------------------------------------------------------
	  public virtual double presentValue(SwapPaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentPeriod is RatePaymentPeriod)
		{
		  return ratePaymentPeriodPricer.presentValue((RatePaymentPeriod) paymentPeriod, provider);
		}
		else if (paymentPeriod is KnownAmountSwapPaymentPeriod)
		{
		  return knownAmountPaymentPeriodPricer.presentValue((KnownAmountSwapPaymentPeriod) paymentPeriod, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentPeriod type: " + paymentPeriod.GetType().Name);
		}
	  }

	  public virtual PointSensitivityBuilder presentValueSensitivity(SwapPaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentPeriod is RatePaymentPeriod)
		{
		  return ratePaymentPeriodPricer.presentValueSensitivity((RatePaymentPeriod) paymentPeriod, provider);
		}
		else if (paymentPeriod is KnownAmountSwapPaymentPeriod)
		{
		  return knownAmountPaymentPeriodPricer.presentValueSensitivity((KnownAmountSwapPaymentPeriod) paymentPeriod, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentPeriod type: " + paymentPeriod.GetType().Name);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual double forecastValue(SwapPaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentPeriod is RatePaymentPeriod)
		{
		  return ratePaymentPeriodPricer.forecastValue((RatePaymentPeriod) paymentPeriod, provider);
		}
		else if (paymentPeriod is KnownAmountSwapPaymentPeriod)
		{
		  return knownAmountPaymentPeriodPricer.forecastValue((KnownAmountSwapPaymentPeriod) paymentPeriod, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentPeriod type: " + paymentPeriod.GetType().Name);
		}
	  }

	  public virtual PointSensitivityBuilder forecastValueSensitivity(SwapPaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentPeriod is RatePaymentPeriod)
		{
		  return ratePaymentPeriodPricer.forecastValueSensitivity((RatePaymentPeriod) paymentPeriod, provider);
		}
		else if (paymentPeriod is KnownAmountSwapPaymentPeriod)
		{
		  return knownAmountPaymentPeriodPricer.forecastValueSensitivity((KnownAmountSwapPaymentPeriod) paymentPeriod, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentPeriod type: " + paymentPeriod.GetType().Name);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual double pvbp(SwapPaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentPeriod is RatePaymentPeriod)
		{
		  return ratePaymentPeriodPricer.pvbp((RatePaymentPeriod) paymentPeriod, provider);
		}
		else if (paymentPeriod is KnownAmountSwapPaymentPeriod)
		{
		  return knownAmountPaymentPeriodPricer.pvbp((KnownAmountSwapPaymentPeriod) paymentPeriod, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentPeriod type: " + paymentPeriod.GetType().Name);
		}
	  }

	  public virtual PointSensitivityBuilder pvbpSensitivity(SwapPaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentPeriod is RatePaymentPeriod)
		{
		  return ratePaymentPeriodPricer.pvbpSensitivity((RatePaymentPeriod) paymentPeriod, provider);
		}
		else if (paymentPeriod is KnownAmountSwapPaymentPeriod)
		{
		  return knownAmountPaymentPeriodPricer.pvbpSensitivity((KnownAmountSwapPaymentPeriod) paymentPeriod, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentPeriod type: " + paymentPeriod.GetType().Name);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual double accruedInterest(SwapPaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentPeriod is RatePaymentPeriod)
		{
		  return ratePaymentPeriodPricer.accruedInterest((RatePaymentPeriod) paymentPeriod, provider);
		}
		else if (paymentPeriod is KnownAmountSwapPaymentPeriod)
		{
		  return knownAmountPaymentPeriodPricer.accruedInterest((KnownAmountSwapPaymentPeriod) paymentPeriod, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentPeriod type: " + paymentPeriod.GetType().Name);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void explainPresentValue(SwapPaymentPeriod paymentPeriod, RatesProvider provider, ExplainMapBuilder builder)
	  {
		// dispatch by runtime type
		if (paymentPeriod is RatePaymentPeriod)
		{
		  ratePaymentPeriodPricer.explainPresentValue((RatePaymentPeriod) paymentPeriod, provider, builder);
		}
		else if (paymentPeriod is KnownAmountSwapPaymentPeriod)
		{
		  knownAmountPaymentPeriodPricer.explainPresentValue((KnownAmountSwapPaymentPeriod) paymentPeriod, provider, builder);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentEvent type: " + paymentPeriod.GetType().Name);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual MultiCurrencyAmount currencyExposure(SwapPaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentPeriod is RatePaymentPeriod)
		{
		  return ratePaymentPeriodPricer.currencyExposure((RatePaymentPeriod) paymentPeriod, provider);
		}
		else if (paymentPeriod is KnownAmountSwapPaymentPeriod)
		{
		  return knownAmountPaymentPeriodPricer.currencyExposure((KnownAmountSwapPaymentPeriod) paymentPeriod, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentEvent type: " + paymentPeriod.GetType().Name);
		}
	  }

	  public virtual double currentCash(SwapPaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		// dispatch by runtime type
		if (paymentPeriod is RatePaymentPeriod)
		{
		  return ratePaymentPeriodPricer.currentCash((RatePaymentPeriod) paymentPeriod, provider);
		}
		else if (paymentPeriod is KnownAmountSwapPaymentPeriod)
		{
		  return knownAmountPaymentPeriodPricer.currentCash((KnownAmountSwapPaymentPeriod) paymentPeriod, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown PaymentEvent type: " + paymentPeriod.GetType().Name);
		}
	  }

	}

}