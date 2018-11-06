using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;
	using ProbabilityDistribution = com.opengamma.strata.math.impl.statistics.distribution.ProbabilityDistribution;
	using CashFlowEquivalentCalculator = com.opengamma.strata.pricer.impl.rate.swap.CashFlowEquivalentCalculator;
	using HullWhiteOneFactorPiecewiseConstantParametersProvider = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParametersProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SettlementType = com.opengamma.strata.product.common.SettlementType;
	using NotionalExchange = com.opengamma.strata.product.swap.NotionalExchange;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;

	/// <summary>
	/// Pricer for swaption with physical settlement in Hull-White one factor model with piecewise constant volatility.
	/// <para>
	/// Reference: Henrard, M. "The Irony in the derivatives discounting Part II: the crisis", Wilmott Journal, 2010, 2, 301-316
	/// </para>
	/// </summary>
	public class HullWhiteSwaptionPhysicalProductPricer
	{

	  /// <summary>
	  /// Normal distribution function.
	  /// </summary>
	  private static readonly ProbabilityDistribution<double> NORMAL = new NormalDistribution(0, 1);

	  /// <summary>
	  /// The small parameter.
	  /// </summary>
	  private const double SMALL = 1.0e-9;

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly HullWhiteSwaptionPhysicalProductPricer DEFAULT = new HullWhiteSwaptionPhysicalProductPricer(DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public HullWhiteSwaptionPhysicalProductPricer(DiscountingPaymentPricer paymentPricer)
	  {
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  /// <summary>
	  /// Calculates the present value of the swaption product.
	  /// <para>
	  /// The result is expressed using the currency of the swapion.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedSwaption swaption, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		validate(swaption, ratesProvider, hwProvider);
		ResolvedSwap swap = swaption.Underlying;
		LocalDate expiryDate = swaption.ExpiryDate;
		if (expiryDate.isBefore(ratesProvider.ValuationDate))
		{ // Option has expired already
		  return CurrencyAmount.of(swap.Legs.get(0).Currency, 0d);
		}
		ResolvedSwapLeg cashFlowEquiv = CashFlowEquivalentCalculator.cashFlowEquivalentSwap(swap, ratesProvider);
		int nPayments = cashFlowEquiv.PaymentEvents.size();
		double[] alpha = new double[nPayments];
		double[] discountedCashFlow = new double[nPayments];
		for (int loopcf = 0; loopcf < nPayments; loopcf++)
		{
		  NotionalExchange payment = (NotionalExchange) cashFlowEquiv.PaymentEvents.get(loopcf);
		  LocalDate maturityDate = payment.PaymentDate;
		  alpha[loopcf] = hwProvider.alpha(ratesProvider.ValuationDate, expiryDate, expiryDate, maturityDate);
		  discountedCashFlow[loopcf] = paymentPricer.presentValueAmount(payment.Payment, ratesProvider);
		}
		double omega = (swap.getLegs(SwapLegType.FIXED).get(0).PayReceive.Pay ? -1d : 1d);
		double kappa = computeKappa(hwProvider, discountedCashFlow, alpha, omega);
		double pv = 0.0;
		for (int loopcf = 0; loopcf < nPayments; loopcf++)
		{
		  pv += discountedCashFlow[loopcf] * NORMAL.getCDF(omega * (kappa + alpha[loopcf]));
		}
		return CurrencyAmount.of(cashFlowEquiv.Currency, pv * (swaption.LongShort.Long ? 1d : -1d));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the swaption product.
	  /// </summary>
	  /// <param name="swaption">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedSwaption swaption, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		return MultiCurrencyAmount.of(presentValue(swaption, ratesProvider, hwProvider));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the swaption product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the point sensitivity to the rate curves </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRates(ResolvedSwaption swaption, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		validate(swaption, ratesProvider, hwProvider);
		ResolvedSwap swap = swaption.Underlying;
		LocalDate expiryDate = swaption.ExpiryDate;
		if (expiryDate.isBefore(ratesProvider.ValuationDate))
		{ // Option has expired already
		  return PointSensitivityBuilder.none();
		}
		ImmutableMap<Payment, PointSensitivityBuilder> cashFlowEquivSensi = CashFlowEquivalentCalculator.cashFlowEquivalentAndSensitivitySwap(swap, ratesProvider);
		ImmutableList<Payment> list = cashFlowEquivSensi.Keys.asList();
		ImmutableList<PointSensitivityBuilder> listSensi = cashFlowEquivSensi.values().asList();
		int nPayments = list.size();
		double[] alpha = new double[nPayments];
		double[] discountedCashFlow = new double[nPayments];
		for (int loopcf = 0; loopcf < nPayments; loopcf++)
		{
		  Payment payment = list.get(loopcf);
		  alpha[loopcf] = hwProvider.alpha(ratesProvider.ValuationDate, expiryDate, expiryDate, payment.Date);
		  discountedCashFlow[loopcf] = paymentPricer.presentValueAmount(payment, ratesProvider);
		}
		double omega = (swap.getLegs(SwapLegType.FIXED).get(0).PayReceive.Pay ? -1d : 1d);
		double kappa = computeKappa(hwProvider, discountedCashFlow, alpha, omega);
		PointSensitivityBuilder point = PointSensitivityBuilder.none();
		for (int loopcf = 0; loopcf < nPayments; loopcf++)
		{
		  Payment payment = list.get(loopcf);
		  double cdf = NORMAL.getCDF(omega * (kappa + alpha[loopcf]));
		  point = point.combinedWith(paymentPricer.presentValueSensitivity(payment, ratesProvider).multipliedBy(cdf));
		  if (!listSensi.get(loopcf).Equals(PointSensitivityBuilder.none()))
		  {
			point = point.combinedWith(listSensi.get(loopcf).multipliedBy(cdf * ratesProvider.discountFactor(payment.Currency, payment.Date)));
		  }
		}
		return swaption.LongShort.Long ? point : point.multipliedBy(-1d);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity to piecewise constant volatility parameters of the Hull-White model.
	  /// </summary>
	  /// <param name="swaption">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the present value Hull-White model parameter sensitivity of the swaption product </returns>
	  public virtual DoubleArray presentValueSensitivityModelParamsHullWhite(ResolvedSwaption swaption, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		validate(swaption, ratesProvider, hwProvider);
		ResolvedSwap swap = swaption.Underlying;
		LocalDate expiryDate = swaption.ExpiryDate;
		if (expiryDate.isBefore(ratesProvider.ValuationDate))
		{ // Option has expired already
		  return DoubleArray.EMPTY;
		}
		ResolvedSwapLeg cashFlowEquiv = CashFlowEquivalentCalculator.cashFlowEquivalentSwap(swap, ratesProvider);
		int nPayments = cashFlowEquiv.PaymentEvents.size();
		double[] alpha = new double[nPayments];
		double[][] alphaAdjoint = new double[nPayments][];
		double[] discountedCashFlow = new double[nPayments];
		for (int loopcf = 0; loopcf < nPayments; loopcf++)
		{
		  NotionalExchange payment = (NotionalExchange) cashFlowEquiv.PaymentEvents.get(loopcf);
		  ValueDerivatives valueDeriv = hwProvider.alphaAdjoint(ratesProvider.ValuationDate, expiryDate, expiryDate, payment.PaymentDate);
		  alpha[loopcf] = valueDeriv.Value;
		  alphaAdjoint[loopcf] = valueDeriv.Derivatives.toArray();
		  discountedCashFlow[loopcf] = paymentPricer.presentValueAmount(payment.Payment, ratesProvider);
		}
		double omega = (swap.getLegs(SwapLegType.FIXED).get(0).PayReceive.Pay ? -1d : 1d);
		double kappa = computeKappa(hwProvider, discountedCashFlow, alpha, omega);
		int nParams = alphaAdjoint[0].Length;
		if (Math.Abs(kappa) > 1d / SMALL)
		{ // decays exponentially
		  return DoubleArray.filled(nParams);
		}
		double[] pvSensi = new double[nParams];
		double sign = (swaption.LongShort.Long ? 1d : -1d);
		for (int i = 0; i < nParams; ++i)
		{
		  for (int loopcf = 0; loopcf < nPayments; loopcf++)
		  {
			pvSensi[i] += sign * discountedCashFlow[loopcf] * NORMAL.getPDF(omega * (kappa + alpha[loopcf])) * omega * alphaAdjoint[loopcf][i];
		  }
		}
		return DoubleArray.ofUnsafe(pvSensi);
	  }

	  //-------------------------------------------------------------------------
	  // validate that the rates and volatilities providers are coherent
	  private void validate(ResolvedSwaption swaption, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {
		ArgChecker.isTrue(hwProvider.ValuationDateTime.toLocalDate().Equals(ratesProvider.ValuationDate), "Hull-White model data and rate data should be for the same date");
		ArgChecker.isFalse(swaption.Underlying.CrossCurrency, "underlying swap should be single currency");
		ArgChecker.isTrue(swaption.SwaptionSettlement.SettlementType.Equals(SettlementType.PHYSICAL), "swaption should be physical settlement");
	  }

	  // handling short time to expiry
	  private double computeKappa(HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider, double[] discountedCashFlow, double[] alpha, double omega)
	  {
		double kappa = 0d;
		if (DoubleArrayMath.fuzzyEqualsZero(alpha, SMALL))
		{ // threshold coherent to rootfinder in kappa computation
		  double totalPv = DoubleArrayMath.sum(discountedCashFlow);
		  kappa = totalPv * omega > 0d ? double.PositiveInfinity : double.NegativeInfinity;
		}
		else
		{
		  kappa = hwProvider.Model.kappa(DoubleArray.ofUnsafe(discountedCashFlow), DoubleArray.ofUnsafe(alpha));
		}
		return kappa;
	  }

	}

}