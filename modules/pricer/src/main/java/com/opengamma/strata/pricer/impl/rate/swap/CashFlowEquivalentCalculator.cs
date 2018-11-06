using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate.swap
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using NotionalExchange = com.opengamma.strata.product.swap.NotionalExchange;
	using RateAccrualPeriod = com.opengamma.strata.product.swap.RateAccrualPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;

	/// <summary>
	/// Computes cash flow equivalent of products.
	/// <para>
	/// Reference: Henrard, M. The Irony in the derivatives discounting Part II: the crisis. Wilmott Journal, 2010, 2, 301-316.
	/// </para>
	/// </summary>
	public sealed class CashFlowEquivalentCalculator
	{

	  /// <summary>
	  /// Computes cash flow equivalent of swap.
	  /// <para>
	  /// The swap should be a fix-for-Ibor swap without compounding, and its swap legs
	  /// should not involve {@code PaymentEvent}.
	  /// </para>
	  /// <para>
	  /// The return type is {@code ResolvedSwapLeg} in which individual payments are
	  /// represented in terms of {@code NotionalExchange}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the swap product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the cash flow equivalent </returns>
	  public static ResolvedSwapLeg cashFlowEquivalentSwap(ResolvedSwap swap, RatesProvider ratesProvider)
	  {
		validateSwap(swap);
		ResolvedSwapLeg cfFixed = cashFlowEquivalentFixedLeg(swap.getLegs(SwapLegType.FIXED).get(0), ratesProvider);
		ResolvedSwapLeg cfIbor = cashFlowEquivalentIborLeg(swap.getLegs(SwapLegType.IBOR).get(0), ratesProvider);
		ResolvedSwapLeg leg = ResolvedSwapLeg.builder().paymentEvents(Stream.concat(cfFixed.PaymentEvents.stream(), cfIbor.PaymentEvents.stream()).collect(Collectors.toList())).payReceive(PayReceive.RECEIVE).type(SwapLegType.OTHER).build();
		return leg;
	  }

	  /// <summary>
	  /// Computes cash flow equivalent of Ibor leg.
	  /// <para>
	  /// The return type is {@code ResolvedSwapLeg} in which individual payments are
	  /// represented in terms of {@code NotionalExchange}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="iborLeg">  the Ibor leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the cash flow equivalent </returns>
	  public static ResolvedSwapLeg cashFlowEquivalentIborLeg(ResolvedSwapLeg iborLeg, RatesProvider ratesProvider)
	  {
		ArgChecker.isTrue(iborLeg.Type.Equals(SwapLegType.IBOR), "Leg type should be IBOR");
		ArgChecker.isTrue(iborLeg.PaymentEvents.Empty, "PaymentEvent should be empty");
		IList<NotionalExchange> paymentEvents = new List<NotionalExchange>();
		foreach (SwapPaymentPeriod paymentPeriod in iborLeg.PaymentPeriods)
		{
		  ArgChecker.isTrue(paymentPeriod is RatePaymentPeriod, "rate payment should be RatePaymentPeriod");
		  RatePaymentPeriod ratePaymentPeriod = (RatePaymentPeriod) paymentPeriod;
		  ArgChecker.isTrue(ratePaymentPeriod.AccrualPeriods.size() == 1, "rate payment should not be compounding");
		  RateAccrualPeriod rateAccrualPeriod = ratePaymentPeriod.AccrualPeriods.get(0);
		  CurrencyAmount notional = ratePaymentPeriod.NotionalAmount;
		  LocalDate paymentDate = ratePaymentPeriod.PaymentDate;
		  IborIndexObservation obs = ((IborRateComputation) rateAccrualPeriod.RateComputation).Observation;
		  IborIndex index = obs.Index;
		  LocalDate fixingStartDate = obs.EffectiveDate;
		  double fixingYearFraction = obs.YearFraction;
		  double beta = (1d + fixingYearFraction * ratesProvider.iborIndexRates(index).rate(obs)) * ratesProvider.discountFactor(paymentPeriod.Currency, paymentPeriod.PaymentDate) / ratesProvider.discountFactor(paymentPeriod.Currency, fixingStartDate);
		  double ycRatio = rateAccrualPeriod.YearFraction / fixingYearFraction;
		  NotionalExchange payStart = NotionalExchange.of(notional.multipliedBy(beta * ycRatio), fixingStartDate);
		  NotionalExchange payEnd = NotionalExchange.of(notional.multipliedBy(-ycRatio), paymentDate);
		  paymentEvents.Add(payStart);
		  paymentEvents.Add(payEnd);
		}
		ResolvedSwapLeg leg = ResolvedSwapLeg.builder().paymentEvents(paymentEvents).payReceive(PayReceive.RECEIVE).type(SwapLegType.OTHER).build();
		return leg;
	  }

	  /// <summary>
	  /// Computes cash flow equivalent of fixed leg.
	  /// <para>
	  /// The return type is {@code ResolvedSwapLeg} in which individual payments are
	  /// represented in terms of {@code NotionalExchange}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixedLeg">  the fixed leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the cash flow equivalent </returns>
	  public static ResolvedSwapLeg cashFlowEquivalentFixedLeg(ResolvedSwapLeg fixedLeg, RatesProvider ratesProvider)
	  {
		ArgChecker.isTrue(fixedLeg.Type.Equals(SwapLegType.FIXED), "Leg type should be FIXED");
		ArgChecker.isTrue(fixedLeg.PaymentEvents.Empty, "PaymentEvent should be empty");
		IList<NotionalExchange> paymentEvents = new List<NotionalExchange>();
		foreach (SwapPaymentPeriod paymentPeriod in fixedLeg.PaymentPeriods)
		{
		  ArgChecker.isTrue(paymentPeriod is RatePaymentPeriod, "rate payment should be RatePaymentPeriod");
		  RatePaymentPeriod ratePaymentPeriod = (RatePaymentPeriod) paymentPeriod;
		  ArgChecker.isTrue(ratePaymentPeriod.AccrualPeriods.size() == 1, "rate payment should not be compounding");
		  RateAccrualPeriod rateAccrualPeriod = ratePaymentPeriod.AccrualPeriods.get(0);
		  double factor = rateAccrualPeriod.YearFraction * ((FixedRateComputation) rateAccrualPeriod.RateComputation).Rate;
		  CurrencyAmount notional = ratePaymentPeriod.NotionalAmount.multipliedBy(factor);
		  LocalDate paymentDate = ratePaymentPeriod.PaymentDate;
		  NotionalExchange pay = NotionalExchange.of(notional, paymentDate);
		  paymentEvents.Add(pay);
		}
		ResolvedSwapLeg leg = ResolvedSwapLeg.builder().paymentEvents(paymentEvents).payReceive(PayReceive.RECEIVE).type(SwapLegType.OTHER).build();
		return leg;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes cash flow equivalent and sensitivity of swap.
	  /// <para>
	  /// The swap should be a fix-for-Ibor swap without compounding, and its swap legs should not involve {@code PaymentEvent}.
	  /// </para>
	  /// <para>
	  /// The return type is a map of {@code NotionalExchange} and {@code PointSensitivityBuilder}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the swap product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the cash flow equivalent and sensitivity </returns>
	  public static ImmutableMap<Payment, PointSensitivityBuilder> cashFlowEquivalentAndSensitivitySwap(ResolvedSwap swap, RatesProvider ratesProvider)
	  {

		validateSwap(swap);
		ImmutableMap<Payment, PointSensitivityBuilder> mapFixed = cashFlowEquivalentAndSensitivityFixedLeg(swap.getLegs(SwapLegType.FIXED).get(0), ratesProvider);
		ImmutableMap<Payment, PointSensitivityBuilder> mapIbor = cashFlowEquivalentAndSensitivityIborLeg(swap.getLegs(SwapLegType.IBOR).get(0), ratesProvider);
		return ImmutableMap.builder<Payment, PointSensitivityBuilder>().putAll(mapFixed).putAll(mapIbor).build();
	  }

	  /// <summary>
	  /// Computes cash flow equivalent and sensitivity of Ibor leg.
	  /// <para>
	  /// The return type is a map of {@code NotionalExchange} and {@code PointSensitivityBuilder}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="iborLeg">  the Ibor leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the cash flow equivalent and sensitivity </returns>
	  public static ImmutableMap<Payment, PointSensitivityBuilder> cashFlowEquivalentAndSensitivityIborLeg(ResolvedSwapLeg iborLeg, RatesProvider ratesProvider)
	  {

		ArgChecker.isTrue(iborLeg.Type.Equals(SwapLegType.IBOR), "Leg type should be IBOR");
		ArgChecker.isTrue(iborLeg.PaymentEvents.Empty, "PaymentEvent should be empty");
		IDictionary<Payment, PointSensitivityBuilder> res = new Dictionary<Payment, PointSensitivityBuilder>();
		foreach (SwapPaymentPeriod paymentPeriod in iborLeg.PaymentPeriods)
		{
		  ArgChecker.isTrue(paymentPeriod is RatePaymentPeriod, "rate payment should be RatePaymentPeriod");
		  RatePaymentPeriod ratePaymentPeriod = (RatePaymentPeriod) paymentPeriod;
		  ArgChecker.isTrue(ratePaymentPeriod.AccrualPeriods.size() == 1, "rate payment should not be compounding");
		  RateAccrualPeriod rateAccrualPeriod = ratePaymentPeriod.AccrualPeriods.get(0);
		  CurrencyAmount notional = ratePaymentPeriod.NotionalAmount;
		  LocalDate paymentDate = ratePaymentPeriod.PaymentDate;
		  IborIndexObservation obs = ((IborRateComputation) rateAccrualPeriod.RateComputation).Observation;
		  IborIndex index = obs.Index;
		  LocalDate fixingStartDate = obs.EffectiveDate;
		  double fixingYearFraction = obs.YearFraction;

		  double factorIndex = (1d + fixingYearFraction * ratesProvider.iborIndexRates(index).rate(obs));
		  double dfPayment = ratesProvider.discountFactor(paymentPeriod.Currency, paymentPeriod.PaymentDate);
		  double dfStart = ratesProvider.discountFactor(paymentPeriod.Currency, fixingStartDate);
		  double beta = factorIndex * dfPayment / dfStart;
		  double ycRatio = rateAccrualPeriod.YearFraction / fixingYearFraction;
		  Payment payStart = Payment.of(notional.multipliedBy(beta * ycRatio), fixingStartDate);
		  Payment payEnd = Payment.of(notional.multipliedBy(-ycRatio), paymentDate);
		  double factor = ycRatio * notional.Amount / dfStart;

		  PointSensitivityBuilder factorIndexSensi = ratesProvider.iborIndexRates(index).ratePointSensitivity(obs).multipliedBy(fixingYearFraction * dfPayment * factor);
		  PointSensitivityBuilder dfPaymentSensitivity = ratesProvider.discountFactors(paymentPeriod.Currency).zeroRatePointSensitivity(paymentPeriod.PaymentDate).multipliedBy(factorIndex * factor);
		  PointSensitivityBuilder dfStartSensitivity = ratesProvider.discountFactors(paymentPeriod.Currency).zeroRatePointSensitivity(fixingStartDate).multipliedBy(-factorIndex * dfPayment * factor / dfStart);
		  res[payStart] = factorIndexSensi.combinedWith(dfPaymentSensitivity).combinedWith(dfStartSensitivity);
		  res[payEnd] = PointSensitivityBuilder.none();
		}
		return ImmutableMap.copyOf(res);
	  }

	  /// <summary>
	  /// Computes cash flow equivalent and sensitivity of fixed leg.
	  /// <para>
	  /// The return type is a map of {@code NotionalExchange} and {@code PointSensitivityBuilder}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixedLeg">  the fixed leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the cash flow equivalent and sensitivity </returns>
	  public static ImmutableMap<Payment, PointSensitivityBuilder> cashFlowEquivalentAndSensitivityFixedLeg(ResolvedSwapLeg fixedLeg, RatesProvider ratesProvider)
	  {

		ArgChecker.isTrue(fixedLeg.Type.Equals(SwapLegType.FIXED), "Leg type should be FIXED");
		ArgChecker.isTrue(fixedLeg.PaymentEvents.Empty, "PaymentEvent should be empty");
		IDictionary<Payment, PointSensitivityBuilder> res = new Dictionary<Payment, PointSensitivityBuilder>();
		foreach (SwapPaymentPeriod paymentPeriod in fixedLeg.PaymentPeriods)
		{
		  ArgChecker.isTrue(paymentPeriod is RatePaymentPeriod, "rate payment should be RatePaymentPeriod");
		  RatePaymentPeriod ratePaymentPeriod = (RatePaymentPeriod) paymentPeriod;
		  ArgChecker.isTrue(ratePaymentPeriod.AccrualPeriods.size() == 1, "rate payment should not be compounding");
		  RateAccrualPeriod rateAccrualPeriod = ratePaymentPeriod.AccrualPeriods.get(0);
		  double factor = rateAccrualPeriod.YearFraction * ((FixedRateComputation) rateAccrualPeriod.RateComputation).Rate;
		  CurrencyAmount notional = ratePaymentPeriod.NotionalAmount.multipliedBy(factor);
		  LocalDate paymentDate = ratePaymentPeriod.PaymentDate;
		  Payment pay = Payment.of(notional, paymentDate);
		  res[pay] = PointSensitivityBuilder.none();
		}
		return ImmutableMap.copyOf(res);
	  }

	  //-------------------------------------------------------------------------
	  private static void validateSwap(ResolvedSwap swap)
	  {
		ArgChecker.isTrue(swap.Legs.size() == 2, "swap should have 2 legs");
		ArgChecker.isTrue(swap.getLegs(SwapLegType.FIXED).size() == 1, "swap should have unique fixed leg");
		ArgChecker.isTrue(swap.getLegs(SwapLegType.IBOR).size() == 1, "swap should have unique Ibor leg");
	  }

	}

}