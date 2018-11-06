using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.MultiCurrencyAmount.toMultiCurrencyAmount;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Triple = com.opengamma.strata.collect.tuple.Triple;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;
	using RateAccrualPeriod = com.opengamma.strata.product.swap.RateAccrualPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;

	/// <summary>
	/// Pricer for for rate swap products.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedSwap"/>.
	/// The product is priced by pricing each leg.
	/// </para>
	/// </summary>
	public class DiscountingSwapProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingSwapProductPricer DEFAULT = new DiscountingSwapProductPricer(DiscountingSwapLegPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedSwapLeg"/>.
	  /// </summary>
	  private readonly DiscountingSwapLegPricer legPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="legPricer">  the pricer for <seealso cref="ResolvedSwapLeg"/> </param>
	  public DiscountingSwapProductPricer(DiscountingSwapLegPricer legPricer)
	  {
		this.legPricer = ArgChecker.notNull(legPricer, "legPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying leg pricer.
	  /// </summary>
	  /// <returns> the leg pricer </returns>
	  public virtual DiscountingSwapLegPricer LegPricer
	  {
		  get
		  {
			return legPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the swap product, converted to the specified currency.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// This is the discounted forecast value.
	  /// The result is converted to the specified currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="currency">  the currency to convert to </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the swap product in the specified currency </returns>
	  public virtual CurrencyAmount presentValue(ResolvedSwap swap, Currency currency, RatesProvider provider)
	  {
		double totalPv = 0;
		foreach (ResolvedSwapLeg leg in swap.Legs)
		{
		  double pv = legPricer.presentValueInternal(leg, provider);
		  totalPv += (pv * provider.fxRate(leg.Currency, currency));
		}
		return CurrencyAmount.of(currency, totalPv);
	  }

	  /// <summary>
	  /// Calculates the present value of the swap product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// This is the discounted forecast value.
	  /// The result is expressed using the payment currency of each leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the swap product </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedSwap swap, RatesProvider provider)
	  {
		return swapValue(provider, swap, legPricer.presentValueInternal);
	  }

	  /// <summary>
	  /// Calculates the forecast value of the swap product.
	  /// <para>
	  /// The forecast value of the product is the value on the valuation date without present value discounting.
	  /// The result is expressed using the payment currency of each leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value of the swap product </returns>
	  public virtual MultiCurrencyAmount forecastValue(ResolvedSwap swap, RatesProvider provider)
	  {
		return swapValue(provider, swap, legPricer.forecastValueInternal);
	  }

	  //-------------------------------------------------------------------------
	  // calculate present or forecast value for the swap
	  private static MultiCurrencyAmount swapValue(RatesProvider provider, ResolvedSwap swap, System.Func<ResolvedSwapLeg, RatesProvider, double> legFn)
	  {

		if (swap.CrossCurrency)
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  return swap.Legs.Select(leg => CurrencyAmount.of(leg.Currency, legFn(leg, provider))).collect(toMultiCurrencyAmount());
		}
		else
		{
		  Currency currency = swap.Legs.GetEnumerator().next().Currency;
		  double total = 0d;
		  foreach (ResolvedSwapLeg leg in swap.Legs)
		  {
			total += legFn(leg, provider);
		  }
		  return MultiCurrencyAmount.of(currency, total);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the accrued interest since the last payment.
	  /// <para>
	  /// This determines the payment period applicable at the valuation date and calculates
	  /// the accrued interest since the last payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the accrued interest of the swap product </returns>
	  public virtual MultiCurrencyAmount accruedInterest(ResolvedSwap swap, RatesProvider provider)
	  {
		MultiCurrencyAmount result = MultiCurrencyAmount.empty();
		foreach (ResolvedSwapLeg leg in swap.Legs)
		{
		  result = result.plus(legPricer.accruedInterest(leg, provider));
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the par rate for swaps with a fixed leg.
	  /// <para>
	  /// The par rate is the common rate on all payments of the fixed leg for which the total swap present value is 0.
	  /// </para>
	  /// <para>
	  /// At least one leg must be a fixed leg. The par rate will be computed with respect to the first fixed leg 
	  /// in which all the payments are fixed payments with a unique accrual period (no compounding) and no FX reset.
	  /// If the fixed leg is compounding, the par rate is computed only when the number of fixed coupon payments is 1 and 
	  /// accrual factor of each sub-period is 1 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate </returns>
	  public virtual double parRate(ResolvedSwap swap, RatesProvider provider)
	  {
		// find fixed leg
		ResolvedSwapLeg fixedLeg = this.fixedLeg(swap);
		Currency ccyFixedLeg = fixedLeg.Currency;
		// other payments (not fixed leg coupons) converted in fixed leg currency
		double otherLegsConvertedPv = 0.0;
		foreach (ResolvedSwapLeg leg in swap.Legs)
		{
		  if (leg != fixedLeg)
		  {
			double pvLocal = legPricer.presentValueInternal(leg, provider);
			otherLegsConvertedPv += (pvLocal * provider.fxRate(leg.Currency, ccyFixedLeg));
		  }
		}
		double fixedLegEventsPv = legPricer.presentValueEventsInternal(fixedLeg, provider);
		if (fixedLeg.PaymentPeriods.size() > 1)
		{ // try multiperiod par-rate
		  // PVBP
		  double pvbpFixedLeg = legPricer.pvbp(fixedLeg, provider);
		  // Par rate
		  return -(otherLegsConvertedPv + fixedLegEventsPv) / pvbpFixedLeg;
		}
		SwapPaymentPeriod firstPeriod = fixedLeg.PaymentPeriods.get(0);
		ArgChecker.isTrue(firstPeriod is RatePaymentPeriod, "PaymentPeriod must be instance of RatePaymentPeriod");
		RatePaymentPeriod payment = (RatePaymentPeriod) firstPeriod;
		if (payment.AccrualPeriods.size() == 1)
		{ // no compounding
		  // PVBP
		  double pvbpFixedLeg = legPricer.pvbp(fixedLeg, provider);
		  // Par rate
		  return -(otherLegsConvertedPv + fixedLegEventsPv) / pvbpFixedLeg;
		}
		// try Compounding
		Triple<bool, int, double> fixedCompounded = checkFixedCompounded(fixedLeg);
		ArgChecker.isTrue(fixedCompounded.First, "Swap should have a fixed leg and for one payment it should be based on compunding witout spread.");
		double notional = payment.Notional;
		double df = provider.discountFactor(ccyFixedLeg, payment.PaymentDate);
		return Math.Pow(-(otherLegsConvertedPv + fixedLegEventsPv) / (notional * df) + 1.0d, 1.0 / fixedCompounded.Second) - 1.0d;
	  }

	  /// <summary>
	  /// Computes the par spread for swaps.
	  /// <para>
	  /// The par spread is the common spread on all payments of the first leg for which the total swap present value is 0.
	  /// </para>
	  /// <para>
	  /// The par spread will be computed with respect to the first leg. For that leg, all the payments have a unique 
	  /// accrual period or multiple accrual periods with Flat compounding and no FX reset.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate </returns>
	  public virtual double parSpread(ResolvedSwap swap, RatesProvider provider)
	  {
		ResolvedSwapLeg referenceLeg = swap.Legs.get(0);
		Currency ccyReferenceLeg = referenceLeg.Currency;
		// try one payment compounding, typically for inflation swaps
		Triple<bool, int, double> fixedCompounded = checkFixedCompounded(referenceLeg);
		if (fixedCompounded.First)
		{
		  double df = provider.discountFactor(ccyReferenceLeg, referenceLeg.PaymentPeriods.get(0).PaymentDate);
		  double convertedPv = presentValue(swap, ccyReferenceLeg, provider).Amount;
		  double referenceConvertedPv = legPricer.presentValue(referenceLeg, provider).Amount;
		  double notional = ((RatePaymentPeriod) referenceLeg.PaymentPeriods.get(0)).Notional;
		  double parSpread = Math.Pow(-(convertedPv - referenceConvertedPv) / (df * notional) + 1.0d, 1.0d / fixedCompounded.Second) - (1.0d + fixedCompounded.Third);
		  return parSpread;

		}
		// In other cases, try the standard multiperiod par spread
		double convertedPv = presentValue(swap, ccyReferenceLeg, provider).Amount;
		double pvbp = legPricer.pvbp(referenceLeg, provider);
		return -convertedPv / pvbp;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the swap product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value curve sensitivity of the swap product </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(ResolvedSwap swap, RatesProvider provider)
	  {
		return swapValueSensitivity(swap, provider, legPricer.presentValueSensitivity);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the swap product converted in a given currency.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="currency">  the currency to convert to </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value curve sensitivity of the swap product converted in the given currency </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(ResolvedSwap swap, Currency currency, RatesProvider provider)
	  {
		PointSensitivityBuilder builder = PointSensitivityBuilder.none();
		foreach (ResolvedSwapLeg leg in swap.Legs)
		{
		  PointSensitivityBuilder ls = legPricer.presentValueSensitivity(leg, provider);
		  PointSensitivityBuilder lsConverted = ls.withCurrency(currency).multipliedBy(provider.fxRate(leg.Currency, currency));
		  builder = builder.combinedWith(lsConverted);
		}
		return builder;
	  }

	  /// <summary>
	  /// Calculates the forecast value sensitivity of the swap product.
	  /// <para>
	  /// The forecast value sensitivity of the product is the sensitivity of the forecast value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value curve sensitivity of the swap product </returns>
	  public virtual PointSensitivityBuilder forecastValueSensitivity(ResolvedSwap swap, RatesProvider provider)
	  {
		return swapValueSensitivity(swap, provider, legPricer.forecastValueSensitivity);
	  }

	  // calculate present or forecast value sensitivity for the swap
	  private static PointSensitivityBuilder swapValueSensitivity(ResolvedSwap swap, RatesProvider provider, System.Func<ResolvedSwapLeg, RatesProvider, PointSensitivityBuilder> legFn)
	  {

		PointSensitivityBuilder builder = PointSensitivityBuilder.none();
		foreach (ResolvedSwapLeg leg in swap.Legs)
		{
		  builder = builder.combinedWith(legFn(leg, provider));
		}
		return builder;
	  }

	  /// <summary>
	  /// Calculates the par rate curve sensitivity for a swap with a fixed leg.
	  /// <para>
	  /// The par rate is the common rate on all payments of the fixed leg for which the total swap present value is 0.
	  /// </para>
	  /// <para>
	  /// At least one leg must be a fixed leg. The par rate will be computed with respect to the first fixed leg.
	  /// All the payments in that leg should be fixed payments with a unique accrual period (no compounding) and no FX reset.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate curve sensitivity of the swap product </returns>
	  public virtual PointSensitivityBuilder parRateSensitivity(ResolvedSwap swap, RatesProvider provider)
	  {
		ResolvedSwapLeg fixedLeg = this.fixedLeg(swap);
		Currency ccyFixedLeg = fixedLeg.Currency;
		// other payments (not fixed leg coupons) converted in fixed leg currency
		double otherLegsConvertedPv = 0.0;
		foreach (ResolvedSwapLeg leg in swap.Legs)
		{
		  if (leg != fixedLeg)
		  {
			double pvLocal = legPricer.presentValueInternal(leg, provider);
			otherLegsConvertedPv += (pvLocal * provider.fxRate(leg.Currency, ccyFixedLeg));
		  }
		}
		double fixedLegEventsPv = legPricer.presentValueEventsInternal(fixedLeg, provider);
		double pvbpFixedLeg = legPricer.pvbp(fixedLeg, provider);
		// Backward sweep
		double otherLegsConvertedPvBar = -1.0d / pvbpFixedLeg;
		double fixedLegEventsPvBar = -1.0d / pvbpFixedLeg;
		double pvbpFixedLegBar = (otherLegsConvertedPv + fixedLegEventsPv) / (pvbpFixedLeg * pvbpFixedLeg);
		PointSensitivityBuilder pvbpFixedLegDr = legPricer.pvbpSensitivity(fixedLeg, provider);
		PointSensitivityBuilder fixedLegEventsPvDr = legPricer.presentValueSensitivityEventsInternal(fixedLeg, provider);
		PointSensitivityBuilder otherLegsConvertedPvDr = PointSensitivityBuilder.none();
		foreach (ResolvedSwapLeg leg in swap.Legs)
		{
		  if (leg != fixedLeg)
		  {
			PointSensitivityBuilder pvLegDr = legPricer.presentValueSensitivity(leg, provider).multipliedBy(provider.fxRate(leg.Currency, ccyFixedLeg));
			otherLegsConvertedPvDr = otherLegsConvertedPvDr.combinedWith(pvLegDr);
		  }
		}
		otherLegsConvertedPvDr = otherLegsConvertedPvDr.withCurrency(ccyFixedLeg);
		return pvbpFixedLegDr.multipliedBy(pvbpFixedLegBar).combinedWith(fixedLegEventsPvDr.multipliedBy(fixedLegEventsPvBar)).combinedWith(otherLegsConvertedPvDr.multipliedBy(otherLegsConvertedPvBar));
	  }

	  /// <summary>
	  /// Calculates the par spread curve sensitivity for a swap.
	  /// <para>
	  /// The par spread is the common spread on all payments of the first leg for which the total swap present value is 0.
	  /// </para>
	  /// <para>
	  /// The par spread is computed with respect to the first leg. For that leg, all the payments have a unique 
	  /// accrual period (no compounding) and no FX reset.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread curve sensitivity of the swap product </returns>
	  public virtual PointSensitivityBuilder parSpreadSensitivity(ResolvedSwap swap, RatesProvider provider)
	  {
		ResolvedSwapLeg referenceLeg = swap.Legs.get(0);
		Currency ccyReferenceLeg = referenceLeg.Currency;
		double convertedPv = presentValue(swap, ccyReferenceLeg, provider).Amount;
		PointSensitivityBuilder convertedPvDr = presentValueSensitivity(swap, ccyReferenceLeg, provider);
		// try one payment compounding, typically for inflation swaps
		Triple<bool, int, double> fixedCompounded = checkFixedCompounded(referenceLeg);
		if (fixedCompounded.First)
		{
		  double df = provider.discountFactor(ccyReferenceLeg, referenceLeg.PaymentPeriods.get(0).PaymentDate);
		  PointSensitivityBuilder dfDr = provider.discountFactors(ccyReferenceLeg).zeroRatePointSensitivity(referenceLeg.PaymentPeriods.get(0).PaymentDate);
		  double referenceConvertedPv = legPricer.presentValue(referenceLeg, provider).Amount;
		  PointSensitivityBuilder referenceConvertedPvDr = legPricer.presentValueSensitivity(referenceLeg, provider);
		  double notional = ((RatePaymentPeriod) referenceLeg.PaymentPeriods.get(0)).Notional;
		  PointSensitivityBuilder dParSpreadDr = convertedPvDr.combinedWith(referenceConvertedPvDr.multipliedBy(-1)).multipliedBy(-1.0d / (df * notional)).combinedWith(dfDr.multipliedBy((convertedPv - referenceConvertedPv) / (df * df * notional))).multipliedBy(1.0d / fixedCompounded.Second * Math.Pow(-(convertedPv - referenceConvertedPv) / (df * notional) + 1.0d, 1.0d / fixedCompounded.Second - 1.0d));
		  return dParSpreadDr;
		}
		double pvbp = legPricer.pvbp(referenceLeg, provider);
		// Backward sweep
		double convertedPvBar = -1d / pvbp;
		double pvbpBar = convertedPv / (pvbp * pvbp);
		PointSensitivityBuilder pvbpDr = legPricer.pvbpSensitivity(referenceLeg, provider);
		return convertedPvDr.multipliedBy(convertedPvBar).combinedWith(pvbpDr.multipliedBy(pvbpBar));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the future cash flows of the swap product.
	  /// <para>
	  /// Each expected cash flow is added to the result.
	  /// This is based on <seealso cref="#forecastValue(ResolvedSwap, RatesProvider)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the cash flow </returns>
	  public virtual CashFlows cashFlows(ResolvedSwap swap, RatesProvider provider)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return swap.Legs.Select(leg => legPricer.cashFlows(leg, provider)).Aggregate(CashFlows.NONE, CashFlows::combinedWith);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Explains the present value of the swap product.
	  /// <para>
	  /// This returns explanatory information about the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the explanatory information </returns>
	  public virtual ExplainMap explainPresentValue(ResolvedSwap swap, RatesProvider provider)
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		builder.put(ExplainKey.ENTRY_TYPE, "Swap");
		foreach (ResolvedSwapLeg leg in swap.Legs)
		{
		  builder.addListEntryWithIndex(ExplainKey.LEGS, child => legPricer.explainPresentValueInternal(leg, provider, child));
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the swap product.
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure of the swap product </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedSwap swap, RatesProvider provider)
	  {
		MultiCurrencyAmount ce = MultiCurrencyAmount.empty();
		foreach (ResolvedSwapLeg leg in swap.Legs)
		{
		  ce = ce.plus(legPricer.currencyExposure(leg, provider));
		}
		return ce;
	  }

	  /// <summary>
	  /// Calculates the current cash of the swap product.
	  /// </summary>
	  /// <param name="swap">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash of the swap product </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedSwap swap, RatesProvider provider)
	  {
		MultiCurrencyAmount ce = MultiCurrencyAmount.empty();
		foreach (ResolvedSwapLeg leg in swap.Legs)
		{
		  ce = ce.plus(legPricer.currentCash(leg, provider));
		}
		return ce;
	  }

	  //-------------------------------------------------------------------------
	  // checking that at least one leg is a fixed leg and returning the first one
	  private ResolvedSwapLeg fixedLeg(ResolvedSwap swap)
	  {
		IList<ResolvedSwapLeg> fixedLegs = swap.getLegs(SwapLegType.FIXED);
		if (fixedLegs.Count == 0)
		{
		  throw new System.ArgumentException("Swap must contain a fixed leg");
		}
		return fixedLegs[0];
	  }

	  // Checks if the leg is a fixed leg with one payment and compounding
	  // This type of leg is used in zero-coupon inflation swaps
	  // When returning a 'true' for the first element, the second element is the number of periods which are used in 
	  //   par rate/spread computation and the third element is the common fixed rate
	  private Triple<bool, int, double> checkFixedCompounded(ResolvedSwapLeg leg)
	  {
		if (leg.PaymentEvents.size() != 0)
		{
		  return Triple.of(false, 0, 0.0d); // No event
		}
		RatePaymentPeriod ratePaymentPeriod = (RatePaymentPeriod) leg.PaymentPeriods.get(0);
		if (ratePaymentPeriod.CompoundingMethod == CompoundingMethod.NONE)
		{
		  return Triple.of(false, 0, 0.0d); // Should be compounded
		}
		ImmutableList<RateAccrualPeriod> accrualPeriods = ratePaymentPeriod.AccrualPeriods;
		int nbAccrualPeriods = accrualPeriods.size();
		double fixedRate = 0;
		for (int i = 0; i < nbAccrualPeriods; i++)
		{
		  if (!(accrualPeriods.get(i).RateComputation is FixedRateComputation))
		  {
			return Triple.of(false, 0, 0.0d); // Should be fixed period
		  }
		  if ((i > 0) && (((FixedRateComputation) accrualPeriods.get(i).RateComputation).Rate != fixedRate))
		  {
			return Triple.of(false, 0, 0.0d); // All fixed rates should be the same
		  }
		  fixedRate = ((FixedRateComputation) accrualPeriods.get(i).RateComputation).Rate;
		  if (accrualPeriods.get(i).Spread != 0)
		  {
			return Triple.of(false, 0, 0.0d); // Should have no spread
		  }
		  if (accrualPeriods.get(i).Gearing != 1.0d)
		  {
			return Triple.of(false, 0, 0.0d); // Should have a gearing of 1.
		  }
		  if (accrualPeriods.get(i).YearFraction != 1.0d)
		  {
			return Triple.of(false, 0, 0.0d); // Should have a year fraction of 1.
		  }
		}
		return Triple.of(true, nbAccrualPeriods, fixedRate);
	  }

	}

}