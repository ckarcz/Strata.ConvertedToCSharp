using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swap
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CashFlow = com.opengamma.strata.market.amount.CashFlow;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using KnownAmountSwapPaymentPeriod = com.opengamma.strata.product.swap.KnownAmountSwapPaymentPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using SwapPaymentEvent = com.opengamma.strata.product.swap.SwapPaymentEvent;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;

	/// <summary>
	/// Pricer for for rate swap legs.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedSwapLeg"/>.
	/// The product is priced by pricing each period and event.
	/// </para>
	/// </summary>
	public class DiscountingSwapLegPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingSwapLegPricer DEFAULT = new DiscountingSwapLegPricer(SwapPaymentPeriodPricer.standard(), SwapPaymentEventPricer.standard());

	  /// <summary>
	  /// Pricer for <seealso cref="SwapPaymentPeriod"/>.
	  /// </summary>
	  private readonly SwapPaymentPeriodPricer<SwapPaymentPeriod> paymentPeriodPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="SwapPaymentEvent"/>.
	  /// </summary>
	  private readonly SwapPaymentEventPricer<SwapPaymentEvent> paymentEventPricer;

	  /* Small parameter below which the cash annuity formula is modified. */
	  private const double MIN_YIELD = 1.0E-4;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="paymentPeriodPricer">  the pricer for <seealso cref="SwapPaymentPeriod"/> </param>
	  /// <param name="paymentEventPricer">  the pricer for <seealso cref="SwapPaymentEvent"/> </param>
	  public DiscountingSwapLegPricer(SwapPaymentPeriodPricer<SwapPaymentPeriod> paymentPeriodPricer, SwapPaymentEventPricer<SwapPaymentEvent> paymentEventPricer)
	  {
		this.paymentPeriodPricer = ArgChecker.notNull(paymentPeriodPricer, "paymentPeriodPricer");
		this.paymentEventPricer = ArgChecker.notNull(paymentEventPricer, "paymentEventPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying leg pricer.
	  /// </summary>
	  /// <returns> the leg pricer </returns>
	  public virtual SwapPaymentPeriodPricer<SwapPaymentPeriod> PeriodPricer
	  {
		  get
		  {
			return paymentPeriodPricer;
		  }
	  }

	  /// <summary>
	  /// Gets the underlying leg pricer.
	  /// </summary>
	  /// <returns> the leg pricer </returns>
	  public virtual SwapPaymentEventPricer<SwapPaymentEvent> EventPricer
	  {
		  get
		  {
			return paymentEventPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the swap leg, converted to the specified currency.
	  /// <para>
	  /// The present value of the leg is the value on the valuation date.
	  /// This is the discounted forecast value.
	  /// The result is converted to the specified currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="leg">  the leg </param>
	  /// <param name="currency">  the currency to convert to </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the swap leg in the specified currency </returns>
	  public virtual CurrencyAmount presentValue(ResolvedSwapLeg leg, Currency currency, RatesProvider provider)
	  {
		double pv = presentValueInternal(leg, provider);
		return CurrencyAmount.of(currency, (pv * provider.fxRate(leg.Currency, currency)));
	  }

	  /// <summary>
	  /// Calculates the present value of the swap leg.
	  /// <para>
	  /// The present value of the leg is the value on the valuation date.
	  /// This is the discounted forecast value.
	  /// The result is returned using the payment currency of the leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="leg">  the leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the swap leg </returns>
	  public virtual CurrencyAmount presentValue(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		return CurrencyAmount.of(leg.Currency, presentValueInternal(leg, provider));
	  }

	  // calculates the present value in the currency of the swap leg
	  internal virtual double presentValueInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		return presentValuePeriodsInternal(leg, provider) + presentValueEventsInternal(leg, provider);
	  }

	  /// <summary>
	  /// Calculates the forecast value of the swap leg.
	  /// <para>
	  /// The forecast value of the leg is the value on the valuation date without present value discounting.
	  /// The result is returned using the payment currency of the leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="leg">  the leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value of the swap leg </returns>
	  public virtual CurrencyAmount forecastValue(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		return CurrencyAmount.of(leg.Currency, forecastValueInternal(leg, provider));
	  }

	  // calculates the present value in the currency of the swap leg
	  internal virtual double forecastValueInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		return forecastValuePeriodsInternal(leg, provider) + forecastValueEventsInternal(leg, provider);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the accrued interest since the last payment.
	  /// <para>
	  /// This determines the payment period applicable at the valuation date and calculates
	  /// the accrued interest since the last payment.
	  /// The result is returned using the payment currency of the leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="leg">  the leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the accrued interest of the swap leg </returns>
	  public virtual CurrencyAmount accruedInterest(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		Optional<SwapPaymentPeriod> period = leg.findPaymentPeriod(provider.ValuationDate);
		if (period.Present)
		{
		  double accruedInterest = paymentPeriodPricer.accruedInterest(period.get(), provider);
		  return CurrencyAmount.of(leg.Currency, accruedInterest);
		}
		return CurrencyAmount.zero(leg.Currency);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the Present Value of a Basis Point for a swap leg.
	  /// <para>
	  /// The Present Value of a Basis Point is the value of the leg when the rate is equal to 1.
	  /// A better name would be "Present Value of 1".
	  /// The quantity is also known as "physical annuity" or "level".
	  /// </para>
	  /// <para>
	  /// Each period must not have FX reset or compounding.
	  /// They must not be of type <seealso cref="KnownAmountSwapPaymentPeriod"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="leg">  the swap leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the Present Value of a Basis Point </returns>
	  public virtual double pvbp(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		double pvbpLeg = 0d;
		foreach (SwapPaymentPeriod period in leg.PaymentPeriods)
		{
		  pvbpLeg += paymentPeriodPricer.pvbp(period, provider);
		}
		return pvbpLeg;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the coupon equivalent of a swap leg.
	  /// <para>
	  /// The coupon equivalent is the common fixed coupon for all the periods which would
	  /// result in the same present value of the leg.
	  /// </para>
	  /// <para>
	  /// This is used in particular for swaption pricing with a model on the swap rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="leg">  the swap leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="pvbp">  the present value of a basis point </param>
	  /// <returns> the fixed coupon equivalent </returns>
	  public virtual double couponEquivalent(ResolvedSwapLeg leg, RatesProvider provider, double pvbp)
	  {
		return presentValuePeriodsInternal(leg, provider) / pvbp;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the swap leg.
	  /// <para>
	  /// The present value sensitivity of the leg is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="leg">  the leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value curve sensitivity of the swap leg </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		return legValueSensitivity(leg, provider, paymentPeriodPricer.presentValueSensitivity, paymentEventPricer.presentValueSensitivity);
	  }

	  /// <summary>
	  /// Calculates the forecast value sensitivity of the swap leg.
	  /// <para>
	  /// The forecast value sensitivity of the leg is the sensitivity of the forecast value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="leg">  the leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value curve sensitivity of the swap leg </returns>
	  public virtual PointSensitivityBuilder forecastValueSensitivity(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		return legValueSensitivity(leg, provider, paymentPeriodPricer.forecastValueSensitivity, paymentEventPricer.forecastValueSensitivity);
	  }

	  // calculate present or forecast value sensitivity for a leg
	  private PointSensitivityBuilder legValueSensitivity(ResolvedSwapLeg leg, RatesProvider provider, System.Func<SwapPaymentPeriod, RatesProvider, PointSensitivityBuilder> periodFn, System.Func<SwapPaymentEvent, RatesProvider, PointSensitivityBuilder> eventFn)
	  {

		PointSensitivityBuilder builder = PointSensitivityBuilder.none();
		foreach (SwapPaymentPeriod period in leg.PaymentPeriods)
		{
		  if (!period.PaymentDate.isBefore(provider.ValuationDate))
		  {
			builder = builder.combinedWith(periodFn(period, provider));
		  }
		}
		foreach (SwapPaymentEvent @event in leg.PaymentEvents)
		{
		  if (!@event.PaymentDate.isBefore(provider.ValuationDate))
		  {
			builder = builder.combinedWith(eventFn(@event, provider));
		  }
		}
		return builder;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the Present Value of a Basis Point curve sensitivity for a fixed swap leg.
	  /// <para>
	  /// The Present Value of a Basis Point is the value of the leg when the rate is equal to 1.
	  /// A better name would be "Present Value of 1".
	  /// The quantity is also known as "physical annuity" or "level".
	  /// </para>
	  /// <para>
	  /// Each period must not have FX reset or compounding.
	  /// They must not be of type <seealso cref="KnownAmountSwapPaymentPeriod"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixedLeg">  the swap fixed leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the Present Value of a Basis Point sensitivity to the curves </returns>
	  public virtual PointSensitivityBuilder pvbpSensitivity(ResolvedSwapLeg fixedLeg, RatesProvider provider)
	  {
		PointSensitivityBuilder builder = PointSensitivityBuilder.none();
		foreach (SwapPaymentPeriod period in fixedLeg.PaymentPeriods)
		{
		  builder = builder.combinedWith(paymentPeriodPricer.pvbpSensitivity(period, provider));
		}
		return builder;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the conventional cash annuity from a swap leg.
	  /// <para>
	  /// The computation is relevant only for standard swaps with constant notional and regular payments.
	  /// The swap leg must be a fixed leg. However, this is not checked internally.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixedLeg">  the fixed leg of the swap </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the cash annuity </returns>
	  public virtual double annuityCash(ResolvedSwapLeg fixedLeg, double yield)
	  {
		int nbFixedPeriod = fixedLeg.PaymentPeriods.size();
		SwapPaymentPeriod paymentPeriod = fixedLeg.PaymentPeriods.get(0);
		ArgChecker.isTrue(paymentPeriod is RatePaymentPeriod, "payment period should be RatePaymentPeriod");
		RatePaymentPeriod ratePaymentPeriod = (RatePaymentPeriod) paymentPeriod;
		int nbFixedPaymentYear = (int) (long)Math.Round(1d / ratePaymentPeriod.DayCount.yearFraction(ratePaymentPeriod.StartDate, ratePaymentPeriod.EndDate), MidpointRounding.AwayFromZero);
		double notional = Math.Abs(ratePaymentPeriod.Notional);
		double annuityCash = notional * this.annuityCash(nbFixedPaymentYear, nbFixedPeriod, yield);
		return annuityCash;
	  }

	  /// <summary>
	  /// Computes the conventional cash annuity for a given yield.
	  /// </summary>
	  /// <param name="nbPaymentsPerYear">  the number of payment per year </param>
	  /// <param name="nbPeriods">  the total number of periods </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the cash annuity </returns>
	  public virtual double annuityCash(int nbPaymentsPerYear, int nbPeriods, double yield)
	  {
		double tau = 1d / nbPaymentsPerYear;
		if (Math.Abs(yield) > MIN_YIELD)
		{
		  return (1d - Math.Pow(1d + yield * tau, -nbPeriods)) / yield;
		}
		double annuity = 0.0d;
		double periodFactor = 1.0d / (1.0d + yield * tau);
		double multiPeriodFactor = periodFactor;
		for (int i = 0; i < nbPeriods; i++)
		{
		  annuity += multiPeriodFactor;
		  multiPeriodFactor *= periodFactor;
		}
		annuity *= tau;
		return annuity;
	  }

	  /// <summary>
	  /// Computes the conventional cash annuity for a given yield and its first derivative with respect to the yield.
	  /// </summary>
	  /// <param name="nbPaymentsPerYear">  the number of payment per year </param>
	  /// <param name="nbPeriods">  the total number of periods </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the cash annuity and its first derivative </returns>
	  public virtual ValueDerivatives annuityCash1(int nbPaymentsPerYear, int nbPeriods, double yield)
	  {
		double tau = 1d / nbPaymentsPerYear;
		if (Math.Abs(yield) > MIN_YIELD)
		{
		  double yieldPerPeriod = yield * tau;
		  double dfEnd = Math.Pow(1d + yieldPerPeriod, -nbPeriods);
		  double annuity = (1d - dfEnd) / yield;
		  double derivative = -annuity / yield;
		  derivative += tau * nbPeriods * dfEnd / ((1d + yieldPerPeriod) * yield);
		  return ValueDerivatives.of(annuity, DoubleArray.of(derivative));
		}
		double annuity = 0.0d;
		double derivative = 0.0d;
		double periodFactor = 1.0d / (1.0d + yield * tau);
		double multiPeriodFactor = periodFactor;
		for (int i = 0; i < nbPeriods; i++)
		{
		  annuity += multiPeriodFactor;
		  multiPeriodFactor *= periodFactor;
		  derivative += -(i + 1) * multiPeriodFactor;
		}
		annuity *= tau;
		derivative *= tau * tau;
		return ValueDerivatives.of(annuity, DoubleArray.of(derivative));
	  }

	  /// <summary>
	  /// Computes the conventional cash annuity for a given yield and its first two derivatives with respect to the yield.
	  /// </summary>
	  /// <param name="nbPaymentsPerYear">  the number of payment per year </param>
	  /// <param name="nbPeriods">  the total number of periods </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the cash annuity and its first two derivatives </returns>
	  public virtual ValueDerivatives annuityCash2(int nbPaymentsPerYear, int nbPeriods, double yield)
	  {
		double tau = 1d / nbPaymentsPerYear;
		if (Math.Abs(yield) > MIN_YIELD)
		{
		  double yieldPerPeriod = yield * tau;
		  double dfEnd = Math.Pow(1d + yieldPerPeriod, -nbPeriods);
		  double annuity = (1d - dfEnd) / yield;
		  double derivative1 = -annuity / yield;
		  derivative1 += tau * nbPeriods * dfEnd / ((1d + yieldPerPeriod) * yield);
		  double derivative2 = -2 * derivative1 / yield;
		  derivative2 -= tau * tau * nbPeriods * (nbPeriods + 1) * dfEnd / ((1d + yieldPerPeriod) * (1d + yieldPerPeriod) * yield);
		  return ValueDerivatives.of(annuity, DoubleArray.of(derivative1, derivative2));
		}
		double annuity = 0.0d;
		double derivative1 = 0.0d;
		double derivative2 = 0.0d;
		double periodFactor = 1.0d / (1.0d + yield * tau);
		double multiPeriodFactor = periodFactor;
		for (int i = 0; i < nbPeriods; i++)
		{
		  annuity += multiPeriodFactor;
		  multiPeriodFactor *= periodFactor;
		  derivative1 += -(i + 1) * multiPeriodFactor;
		  derivative2 += (i + 1) * (i + 2) * multiPeriodFactor * periodFactor;
		}
		annuity *= tau;
		derivative1 *= tau * tau;
		derivative2 *= tau * tau * tau;
		return ValueDerivatives.of(annuity, DoubleArray.of(derivative1, derivative2));
	  }

	  /// <summary>
	  /// Computes the conventional cash annuity for a given yield and its first three derivatives with respect to the yield.
	  /// </summary>
	  /// <param name="nbPaymentsPerYear">  the number of payment per year </param>
	  /// <param name="nbPeriods">  the total number of periods </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the cash annuity and its first three derivatives </returns>
	  public virtual ValueDerivatives annuityCash3(int nbPaymentsPerYear, int nbPeriods, double yield)
	  {
		double tau = 1d / nbPaymentsPerYear;
		if (Math.Abs(yield) > MIN_YIELD)
		{
		  double yieldPerPeriod = yield * tau;
		  double dfEnd = Math.Pow(1d + yieldPerPeriod, -nbPeriods);
		  double annuity = (1d - dfEnd) / yield;
		  double derivative1 = -annuity / yield;
		  derivative1 += tau * nbPeriods * dfEnd / ((1d + yieldPerPeriod) * yield);
		  double derivative2 = -2 * derivative1 / yield;
		  derivative2 -= tau * tau * nbPeriods * (nbPeriods + 1) * dfEnd / ((1d + yieldPerPeriod) * (1d + yieldPerPeriod) * yield);
		  double derivative3 = -6.0d * annuity / (yield * yield * yield);
		  derivative3 += 6.0d * tau * nbPeriods / (yield * yield * yield) * dfEnd / (1d + yieldPerPeriod);
		  derivative3 += 3.0d * tau * tau * nbPeriods * (nbPeriods + 1) * dfEnd / ((1d + yieldPerPeriod) * (1d + yieldPerPeriod) * yield * yield);
		  derivative3 += tau * tau * tau * nbPeriods * (nbPeriods + 1) * (nbPeriods + 2) * dfEnd / ((1d + yieldPerPeriod) * (1d + yieldPerPeriod) * (1d + yieldPerPeriod) * yield);
		  return ValueDerivatives.of(annuity, DoubleArray.of(derivative1, derivative2, derivative3));
		}
		double annuity = 0.0d;
		double derivative1 = 0.0d;
		double derivative2 = 0.0d;
		double derivative3 = 0.0d;
		double periodFactor = 1.0d / (1.0d + yield * tau);
		double multiPeriodFactor = periodFactor;
		for (int i = 0; i < nbPeriods; i++)
		{
		  annuity += multiPeriodFactor;
		  multiPeriodFactor *= periodFactor;
		  derivative1 += -(i + 1) * multiPeriodFactor;
		  derivative2 += (i + 1) * (i + 2) * multiPeriodFactor * periodFactor;
		  derivative3 += -(i + 1) * (i + 2) * (i + 3) * multiPeriodFactor * periodFactor * periodFactor;
		}
		annuity *= tau;
		derivative1 *= tau * tau;
		derivative2 *= tau * tau * tau;
		derivative3 *= tau * tau * tau * tau;
		return ValueDerivatives.of(annuity, DoubleArray.of(derivative1, derivative2, derivative3));
	  }

	  /// <summary>
	  /// Computes the derivative of the conventional cash annuity with respect to the yield from a swap leg.
	  /// <para>
	  /// The computation is relevant only for standard swaps with constant notional and regular payments.
	  /// The swap leg must be a fixed leg. However, this is not checked internally.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixedLeg">  the fixed leg of the swap </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the cash annuity </returns>
	  public virtual ValueDerivatives annuityCashDerivative(ResolvedSwapLeg fixedLeg, double yield)
	  {
		int nbFixedPeriod = fixedLeg.PaymentPeriods.size();
		SwapPaymentPeriod paymentPeriod = fixedLeg.PaymentPeriods.get(0);
		ArgChecker.isTrue(paymentPeriod is RatePaymentPeriod, "payment period should be RatePaymentPeriod");
		RatePaymentPeriod ratePaymentPeriod = (RatePaymentPeriod) paymentPeriod;
		int nbFixedPaymentYear = (int) (long)Math.Round(1d / ratePaymentPeriod.DayCount.yearFraction(ratePaymentPeriod.StartDate, ratePaymentPeriod.EndDate), MidpointRounding.AwayFromZero);
		double notional = Math.Abs(ratePaymentPeriod.Notional);
		ValueDerivatives annuityUnit = annuityCash1(nbFixedPaymentYear, nbFixedPeriod, yield);
		return ValueDerivatives.of(annuityUnit.Value * notional, annuityUnit.Derivatives.multipliedBy(notional));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the future cash flows of the swap leg.
	  /// <para>
	  /// Each expected cash flow is added to the result.
	  /// This is based on <seealso cref="#forecastValue(ResolvedSwapLeg, RatesProvider)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="leg">  the swap leg for which the cash flows should be computed </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the cash flows </returns>
	  public virtual CashFlows cashFlows(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		CashFlows cashFlowPeriods = cashFlowPeriodsInternal(leg, provider);
		CashFlows cashFlowEvents = cashFlowEventsInternal(leg, provider);
		return cashFlowPeriods.combinedWith(cashFlowEvents);
	  }

	  //-------------------------------------------------------------------------
	  // calculates the forecast value of the events composing the leg in the currency of the swap leg
	  internal virtual double forecastValueEventsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		double total = 0d;
		foreach (SwapPaymentEvent @event in leg.PaymentEvents)
		{
		  if (!@event.PaymentDate.isBefore(provider.ValuationDate))
		  {
			total += paymentEventPricer.forecastValue(@event, provider);
		  }
		}
		return total;
	  }

	  // calculates the forecast value of the periods composing the leg in the currency of the swap leg
	  internal virtual double forecastValuePeriodsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		double total = 0d;
		foreach (SwapPaymentPeriod period in leg.PaymentPeriods)
		{
		  if (!period.PaymentDate.isBefore(provider.ValuationDate))
		  {
			total += paymentPeriodPricer.forecastValue(period, provider);
		  }
		}
		return total;
	  }

	  // calculates the present value of the events composing the leg in the currency of the swap leg
	  internal virtual double presentValueEventsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		double total = 0d;
		foreach (SwapPaymentEvent @event in leg.PaymentEvents)
		{
		  if (!@event.PaymentDate.isBefore(provider.ValuationDate))
		  {
			total += paymentEventPricer.presentValue(@event, provider);
		  }
		}
		return total;
	  }

	  // calculates the present value of the periods composing the leg in the currency of the swap leg
	  internal virtual double presentValuePeriodsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		double total = 0d;
		foreach (SwapPaymentPeriod period in leg.PaymentPeriods)
		{
		  if (!period.PaymentDate.isBefore(provider.ValuationDate))
		  {
			total += paymentPeriodPricer.presentValue(period, provider);
		  }
		}
		return total;
	  }

	  // calculates the present value curve sensitivity of the events composing the leg in the currency of the swap leg
	  internal virtual PointSensitivityBuilder presentValueSensitivityEventsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		PointSensitivityBuilder builder = PointSensitivityBuilder.none();
		foreach (SwapPaymentEvent @event in leg.PaymentEvents)
		{
		  if (!@event.PaymentDate.isBefore(provider.ValuationDate))
		  {
			builder = builder.combinedWith(paymentEventPricer.presentValueSensitivity(@event, provider));
		  }
		}
		return builder;
	  }

	  // calculates the present value curve sensitivity of the periods composing the leg in the currency of the swap leg
	  internal virtual PointSensitivityBuilder presentValueSensitivityPeriodsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		PointSensitivityBuilder builder = PointSensitivityBuilder.none();
		foreach (SwapPaymentPeriod period in leg.PaymentPeriods)
		{
		  if (!period.PaymentDate.isBefore(provider.ValuationDate))
		  {
			builder = builder.combinedWith(paymentPeriodPricer.presentValueSensitivity(period, provider));
		  }
		}
		return builder;
	  }

	  //-------------------------------------------------------------------------
	  // calculates the cash flow of the periods composing the leg in the currency of the swap leg
	  internal virtual CashFlows cashFlowPeriodsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		ImmutableList.Builder<CashFlow> builder = ImmutableList.builder();
		foreach (SwapPaymentPeriod period in leg.PaymentPeriods)
		{
		  if (!period.PaymentDate.isBefore(provider.ValuationDate))
		  {
			double forecastValue = paymentPeriodPricer.forecastValue(period, provider);
			if (forecastValue != 0d)
			{
			  Currency currency = period.Currency;
			  LocalDate paymentDate = period.PaymentDate;
			  double discountFactor = provider.discountFactor(currency, paymentDate);
			  CashFlow singleCashFlow = CashFlow.ofForecastValue(paymentDate, currency, forecastValue, discountFactor);
			  builder.add(singleCashFlow);
			}
		  }
		}
		return CashFlows.of(builder.build());
	  }

	  // calculates the cash flow of the events composing the leg in the currency of the swap leg
	  internal virtual CashFlows cashFlowEventsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		ImmutableList.Builder<CashFlow> builder = ImmutableList.builder();
		foreach (SwapPaymentEvent @event in leg.PaymentEvents)
		{
		  if (!@event.PaymentDate.isBefore(provider.ValuationDate))
		  {
			double forecastValue = paymentEventPricer.forecastValue(@event, provider);
			if (forecastValue != 0d)
			{
			  Currency currency = @event.Currency;
			  LocalDate paymentDate = @event.PaymentDate;
			  double discountFactor = provider.discountFactor(currency, paymentDate);
			  CashFlow singleCashFlow = CashFlow.ofForecastValue(paymentDate, currency, forecastValue, discountFactor);
			  builder.add(singleCashFlow);
			}
		  }
		}
		return CashFlows.of(builder.build());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Explain present value builder used to build large explain map from the individual legs.
	  /// </summary>
	  /// <param name="leg">  the swap log </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="builder">  the explain map builder which will be populated but the leg  </param>
	  internal virtual void explainPresentValueInternal(ResolvedSwapLeg leg, RatesProvider provider, ExplainMapBuilder builder)
	  {
		builder.put(ExplainKey.ENTRY_TYPE, "Leg");
		builder.put(ExplainKey.PAY_RECEIVE, leg.PayReceive);
		builder.put(ExplainKey.LEG_TYPE, leg.Type.ToString());
		foreach (SwapPaymentPeriod period in leg.PaymentPeriods)
		{
		  builder.addListEntry(ExplainKey.PAYMENT_PERIODS, child => paymentPeriodPricer.explainPresentValue(period, provider, child));
		}
		foreach (SwapPaymentEvent @event in leg.PaymentEvents)
		{
		  builder.addListEntry(ExplainKey.PAYMENT_EVENTS, child => paymentEventPricer.explainPresentValue(@event, provider, child));
		}
		builder.put(ExplainKey.FORECAST_VALUE, forecastValue(leg, provider));
		builder.put(ExplainKey.PRESENT_VALUE, presentValue(leg, provider));
	  }

	  /// <summary>
	  /// Explain present value for a swap leg.
	  /// </summary>
	  /// <param name="leg">  the swap log </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the explain PV map </returns>
	  public virtual ExplainMap explainPresentValue(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		explainPresentValueInternal(leg, provider, builder);
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the swap leg.
	  /// </summary>
	  /// <param name="leg">  the leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure of the swap leg </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		return currencyExposurePeriodsInternal(leg, provider).plus(currencyExposureEventsInternal(leg, provider));
	  }

	  private MultiCurrencyAmount currencyExposurePeriodsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		MultiCurrencyAmount total = MultiCurrencyAmount.empty();
		foreach (SwapPaymentPeriod period in leg.PaymentPeriods)
		{
		  if (!period.PaymentDate.isBefore(provider.ValuationDate))
		  {
			total = total.plus(paymentPeriodPricer.currencyExposure(period, provider));
		  }
		}
		return total;
	  }

	  private MultiCurrencyAmount currencyExposureEventsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		MultiCurrencyAmount total = MultiCurrencyAmount.empty();
		foreach (SwapPaymentEvent @event in leg.PaymentEvents)
		{
		  if (!@event.PaymentDate.isBefore(provider.ValuationDate))
		  {
			total = total.plus(paymentEventPricer.currencyExposure(@event, provider));
		  }
		}
		return total;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the current cash of the swap leg.
	  /// </summary>
	  /// <param name="leg">  the leg </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash of the swap leg </returns>
	  public virtual CurrencyAmount currentCash(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		return CurrencyAmount.of(leg.Currency, currentCashPeriodsInternal(leg, provider) + (currentCashEventsInternal(leg, provider)));
	  }

	  private double currentCashPeriodsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		double total = 0d;
		foreach (SwapPaymentPeriod period in leg.PaymentPeriods)
		{
		  if (!period.PaymentDate.isBefore(provider.ValuationDate))
		  {
			total += paymentPeriodPricer.currentCash(period, provider);
		  }
		}
		return total;
	  }

	  private double currentCashEventsInternal(ResolvedSwapLeg leg, RatesProvider provider)
	  {
		double total = 0d;
		foreach (SwapPaymentEvent @event in leg.PaymentEvents)
		{
		  if (!@event.PaymentDate.isBefore(provider.ValuationDate))
		  {
			total += paymentEventPricer.currentCash(@event, provider);
		  }
		}
		return total;
	  }
	}

}