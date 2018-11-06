/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.swap
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using FxIndexRates = com.opengamma.strata.pricer.fx.FxIndexRates;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwapPaymentPeriodPricer = com.opengamma.strata.pricer.swap.SwapPaymentPeriodPricer;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;
	using FxReset = com.opengamma.strata.product.swap.FxReset;
	using RateAccrualPeriod = com.opengamma.strata.product.swap.RateAccrualPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;

	/// <summary>
	/// Pricer implementation for swap payment periods based on a rate.
	/// <para>
	/// The value of a payment period is calculated by combining the value of each accrual period.
	/// Where necessary, the accrual periods are compounded.
	/// </para>
	/// </summary>
	public class DiscountingRatePaymentPeriodPricer : SwapPaymentPeriodPricer<RatePaymentPeriod>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingRatePaymentPeriodPricer DEFAULT = new DiscountingRatePaymentPeriodPricer(RateComputationFn.standard());

	  /// <summary>
	  /// Rate computation.
	  /// </summary>
	  private readonly RateComputationFn<RateComputation> rateComputationFn;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="rateComputationFn">  the rate computation function </param>
	  public DiscountingRatePaymentPeriodPricer(RateComputationFn<RateComputation> rateComputationFn)
	  {
		this.rateComputationFn = ArgChecker.notNull(rateComputationFn, "rateComputationFn");
	  }

	  //-------------------------------------------------------------------------
	  public virtual double presentValue(RatePaymentPeriod period, RatesProvider provider)
	  {
		// forecastValue * discountFactor
		double df = provider.discountFactor(period.Currency, period.PaymentDate);
		return forecastValue(period, provider) * df;
	  }

	  public virtual double forecastValue(RatePaymentPeriod period, RatesProvider provider)
	  {
		// notional * fxRate
		// fxRate is 1 if no FX conversion
		double notional = period.Notional * fxRate(period, provider);
		return accrualWithNotional(period, notional, provider);
	  }

	  public virtual double pvbp(RatePaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		ArgChecker.isTrue(!paymentPeriod.FxReset.Present, "FX reset is not supported");
		int accPeriodCount = paymentPeriod.AccrualPeriods.size();
		ArgChecker.isTrue(accPeriodCount == 1 || paymentPeriod.CompoundingMethod.Equals(CompoundingMethod.FLAT), "Only one accrued period or Flat compounding supported");
		// no compounding
		if (accPeriodCount == 1)
		{
		  RateAccrualPeriod accrualPeriod = paymentPeriod.AccrualPeriods.get(0);
		  double df = provider.discountFactor(paymentPeriod.Currency, paymentPeriod.PaymentDate);
		  return df * accrualPeriod.YearFraction * paymentPeriod.Notional;
		}
		else
		{
		  // Flat compounding
		  switch (paymentPeriod.CompoundingMethod)
		  {
			case FLAT:
			  return pvbpCompoundedFlat(paymentPeriod, provider);
			default:
			  throw new System.NotSupportedException("PVBP not implemented yet for non FLAT compounding");
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual double accruedInterest(RatePaymentPeriod period, RatesProvider provider)
	  {
		LocalDate valDate = provider.ValuationDate;
		if (valDate.compareTo(period.StartDate) <= 0 || valDate.compareTo(period.EndDate) > 0)
		{
		  return 0d;
		}
		ImmutableList.Builder<RateAccrualPeriod> truncated = ImmutableList.builder();
		foreach (RateAccrualPeriod rap in period.AccrualPeriods)
		{
		  if (valDate.compareTo(rap.EndDate) > 0)
		  {
			truncated.add(rap);
		  }
		  else
		  {
			truncated.add(rap.toBuilder().endDate(provider.ValuationDate).unadjustedEndDate(provider.ValuationDate).yearFraction(period.DayCount.yearFraction(rap.StartDate, provider.ValuationDate)).build());
			break;
		  }
		}
		RatePaymentPeriod adjustedPaymentPeriod = period.toBuilder().accrualPeriods(truncated.build()).build();
		return forecastValue(adjustedPaymentPeriod, provider);
	  }

	  //-------------------------------------------------------------------------
	  // resolve the FX rate from the FX reset, returning an FX rate of 1 if not applicable
	  private double fxRate(RatePaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		// inefficient to use Optional.orElse because double primitive type would be boxed
		if (paymentPeriod.FxReset.Present)
		{
		  FxReset fxReset = paymentPeriod.FxReset.get();
		  FxIndexRates rates = provider.fxIndexRates(fxReset.Observation.Index);
		  return rates.rate(fxReset.Observation, fxReset.ReferenceCurrency);
		}
		else
		{
		  return 1d;
		}
	  }

	  private double accrualWithNotional(RatePaymentPeriod period, double notional, RatesProvider provider)
	  {
		// handle simple case and more complex compounding for whole payment period
		if (period.AccrualPeriods.size() == 1)
		{
		  RateAccrualPeriod accrualPeriod = period.AccrualPeriods.get(0);
		  return unitNotionalAccrual(accrualPeriod, accrualPeriod.Spread, provider) * notional;
		}
		return accrueCompounded(period, notional, provider);
	  }

	  // calculate the accrual for a unit notional
	  private double unitNotionalAccrual(RateAccrualPeriod accrualPeriod, double spread, RatesProvider provider)
	  {
		double rawRate = this.rawRate(accrualPeriod, provider);
		return unitNotionalAccrualRaw(accrualPeriod, rawRate, spread);
	  }

	  // calculate the accrual for a unit notional from the raw rate
	  private double unitNotionalAccrualRaw(RateAccrualPeriod accrualPeriod, double rawRate, double spread)
	  {
		double treatedRate = rawRate * accrualPeriod.Gearing + spread;
		return accrualPeriod.NegativeRateMethod.adjust(treatedRate * accrualPeriod.YearFraction);
	  }

	  // finds the raw rate for the accrual period
	  // the raw rate is the rate before gearing, spread and negative checks are applied
	  private double rawRate(RateAccrualPeriod accrualPeriod, RatesProvider provider)
	  {
		return rateComputationFn.rate(accrualPeriod.RateComputation, accrualPeriod.StartDate, accrualPeriod.EndDate, provider);
	  }

	  //-------------------------------------------------------------------------
	  // apply compounding
	  private double accrueCompounded(RatePaymentPeriod paymentPeriod, double notional, RatesProvider provider)
	  {
		switch (paymentPeriod.CompoundingMethod)
		{
		  case STRAIGHT:
			return compoundedStraight(paymentPeriod, notional, provider);
		  case FLAT:
			return compoundedFlat(paymentPeriod, notional, provider);
		  case SPREAD_EXCLUSIVE:
			return compoundedSpreadExclusive(paymentPeriod, notional, provider);
		  case NONE:
		  default:
			return compoundingNone(paymentPeriod, notional, provider);
		}
	  }

	  // straight compounding
	  private double compoundedStraight(RatePaymentPeriod paymentPeriod, double notional, RatesProvider provider)
	  {
		double notionalAccrued = notional;
		foreach (RateAccrualPeriod accrualPeriod in paymentPeriod.AccrualPeriods)
		{
		  double investFactor = 1 + unitNotionalAccrual(accrualPeriod, accrualPeriod.Spread, provider);
		  notionalAccrued *= investFactor;
		}
		return (notionalAccrued - notional);
	  }

	  // flat compounding
	  private double compoundedFlat(RatePaymentPeriod paymentPeriod, double notional, RatesProvider provider)
	  {
		double cpaAccumulated = 0d;
		foreach (RateAccrualPeriod accrualPeriod in paymentPeriod.AccrualPeriods)
		{
		  double rate = rawRate(accrualPeriod, provider);
		  cpaAccumulated += cpaAccumulated * unitNotionalAccrualRaw(accrualPeriod, rate, 0) + unitNotionalAccrualRaw(accrualPeriod, rate, accrualPeriod.Spread);
		}
		return cpaAccumulated * notional;
	  }

	  // spread exclusive compounding
	  private double compoundedSpreadExclusive(RatePaymentPeriod paymentPeriod, double notional, RatesProvider provider)
	  {
		double notionalAccrued = notional;
		double spreadAccrued = 0;
		foreach (RateAccrualPeriod accrualPeriod in paymentPeriod.AccrualPeriods)
		{
		  double investFactor = 1 + unitNotionalAccrual(accrualPeriod, 0, provider);
		  notionalAccrued *= investFactor;
		  spreadAccrued += notional * accrualPeriod.Spread * accrualPeriod.YearFraction;
		}
		return (notionalAccrued - notional + spreadAccrued);
	  }

	  // no compounding, just sum each accrual period
	  private double compoundingNone(RatePaymentPeriod paymentPeriod, double notional, RatesProvider provider)
	  {
		return paymentPeriod.AccrualPeriods.Select(accrualPeriod => unitNotionalAccrual(accrualPeriod, accrualPeriod.Spread, provider) * notional).Sum();
	  }

	  //-------------------------------------------------------------------------
	  public virtual PointSensitivityBuilder presentValueSensitivity(RatePaymentPeriod period, RatesProvider provider)
	  {
		Currency ccy = period.Currency;
		DiscountFactors discountFactors = provider.discountFactors(ccy);
		LocalDate paymentDate = period.PaymentDate;
		double df = discountFactors.discountFactor(paymentDate);
		PointSensitivityBuilder forecastSensitivity = forecastValueSensitivity(period, provider);
		forecastSensitivity = forecastSensitivity.multipliedBy(df);
		double forecastValue = this.forecastValue(period, provider);
		PointSensitivityBuilder dscSensitivity = discountFactors.zeroRatePointSensitivity(paymentDate);
		dscSensitivity = dscSensitivity.multipliedBy(forecastValue);
		return forecastSensitivity.combinedWith(dscSensitivity);
	  }

	  public virtual PointSensitivityBuilder forecastValueSensitivity(RatePaymentPeriod period, RatesProvider provider)
	  {
		// historic payments have zero sensi
		if (period.PaymentDate.isBefore(provider.ValuationDate))
		{
		  return PointSensitivityBuilder.none();
		}
		PointSensitivityBuilder sensiFx = fxRateSensitivity(period, provider);
		double accrual = accrualWithNotional(period, period.Notional, provider);
		sensiFx = sensiFx.multipliedBy(accrual);
		PointSensitivityBuilder sensiAccrual = PointSensitivityBuilder.none();
		if (period.CompoundingApplicable)
		{
		  sensiAccrual = accrueCompoundedSensitivity(period, provider);
		}
		else
		{
		  sensiAccrual = unitNotionalSensitivityNoCompounding(period, provider);
		}
		double notional = period.Notional * fxRate(period, provider);
		sensiAccrual = sensiAccrual.multipliedBy(notional);
		return sensiFx.combinedWith(sensiAccrual);
	  }

	  public virtual PointSensitivityBuilder pvbpSensitivity(RatePaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		ArgChecker.isTrue(!paymentPeriod.FxReset.Present, "FX reset is not supported");
		int accPeriodCount = paymentPeriod.AccrualPeriods.size();
		ArgChecker.isTrue(accPeriodCount == 1 || paymentPeriod.CompoundingMethod.Equals(CompoundingMethod.FLAT), "Only one accrued period or Flat compounding supported");
		// no compounding
		if (accPeriodCount == 1)
		{
		  RateAccrualPeriod accrualPeriod = paymentPeriod.AccrualPeriods.get(0);
		  DiscountFactors discountFactors = provider.discountFactors(paymentPeriod.Currency);
		  return discountFactors.zeroRatePointSensitivity(paymentPeriod.PaymentDate).multipliedBy(accrualPeriod.YearFraction * paymentPeriod.Notional);
		}
		else
		{
		  // Flat compounding
		  switch (paymentPeriod.CompoundingMethod)
		  {
			case FLAT:
			  return pvbpSensitivtyCompoundedFlat(paymentPeriod, provider);
			default:
			  throw new System.NotSupportedException("PVBP not implemented yet for non FLAT compounding");
		  }
		}
	  }

	  // resolve the FX rate sensitivity from the FX reset
	  private PointSensitivityBuilder fxRateSensitivity(RatePaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		if (paymentPeriod.FxReset.Present)
		{
		  FxReset fxReset = paymentPeriod.FxReset.get();
		  FxIndexRates rates = provider.fxIndexRates(fxReset.Observation.Index);
		  return rates.ratePointSensitivity(fxReset.Observation, fxReset.ReferenceCurrency);
		}
		return PointSensitivityBuilder.none();
	  }

	  // computes the sensitivity of the payment period to the rate observations (not to the discount factors)
	  private PointSensitivityBuilder unitNotionalSensitivityNoCompounding(RatePaymentPeriod period, RatesProvider provider)
	  {
		Currency ccy = period.Currency;
		PointSensitivityBuilder sensi = PointSensitivityBuilder.none();
		foreach (RateAccrualPeriod accrualPeriod in period.AccrualPeriods)
		{
		  sensi = sensi.combinedWith(unitNotionalSensitivityAccrual(accrualPeriod, ccy, provider));
		}
		return sensi;
	  }

	  // computes the sensitivity of the accrual period to the rate observations (not to discount factors)
	  private PointSensitivityBuilder unitNotionalSensitivityAccrual(RateAccrualPeriod period, Currency ccy, RatesProvider provider)
	  {

		PointSensitivityBuilder sensi = rateComputationFn.rateSensitivity(period.RateComputation, period.StartDate, period.EndDate, provider);
		return sensi.multipliedBy(period.Gearing * period.YearFraction);
	  }

	  //-------------------------------------------------------------------------
	  // apply compounding - sensitivity
	  private PointSensitivityBuilder accrueCompoundedSensitivity(RatePaymentPeriod paymentPeriod, RatesProvider provider)
	  {

		switch (paymentPeriod.CompoundingMethod)
		{
		  case STRAIGHT:
			return compoundedStraightSensitivity(paymentPeriod, provider);
		  case FLAT:
			return compoundedFlatSensitivity(paymentPeriod, provider);
		  case SPREAD_EXCLUSIVE:
			return compoundedSpreadExclusiveSensitivity(paymentPeriod, provider);
		  default:
			return unitNotionalSensitivityNoCompounding(paymentPeriod, provider);
		}
	  }

	  // straight compounding
	  private PointSensitivityBuilder compoundedStraightSensitivity(RatePaymentPeriod paymentPeriod, RatesProvider provider)
	  {

		double notionalAccrued = 1d;
		Currency ccy = paymentPeriod.Currency;
		PointSensitivityBuilder sensi = PointSensitivityBuilder.none();
		foreach (RateAccrualPeriod accrualPeriod in paymentPeriod.AccrualPeriods)
		{
		  double investFactor = 1d + unitNotionalAccrual(accrualPeriod, accrualPeriod.Spread, provider);
		  notionalAccrued *= investFactor;
		  PointSensitivityBuilder investFactorSensi = unitNotionalSensitivityAccrual(accrualPeriod, ccy, provider).multipliedBy(1d / investFactor);
		  sensi = sensi.combinedWith(investFactorSensi);
		}
		return sensi.multipliedBy(notionalAccrued);
	  }

	  // flat compounding
	  private PointSensitivityBuilder compoundedFlatSensitivity(RatePaymentPeriod paymentPeriod, RatesProvider provider)
	  {

		double cpaAccumulated = 0d;
		Currency ccy = paymentPeriod.Currency;
		PointSensitivityBuilder sensiAccumulated = PointSensitivityBuilder.none();
		foreach (RateAccrualPeriod accrualPeriod in paymentPeriod.AccrualPeriods)
		{
		  double rate = rawRate(accrualPeriod, provider);
		  double accrualZeroSpread = unitNotionalAccrualRaw(accrualPeriod, rate, 0);
		  PointSensitivityBuilder sensiCp = sensiAccumulated.cloned();
		  sensiCp = sensiCp.multipliedBy(accrualZeroSpread);
		  PointSensitivityBuilder sensi2 = unitNotionalSensitivityAccrual(accrualPeriod, ccy, provider).multipliedBy(1d + cpaAccumulated);
		  cpaAccumulated += cpaAccumulated * accrualZeroSpread + unitNotionalAccrualRaw(accrualPeriod, rate, accrualPeriod.Spread);
		  sensiCp = sensiCp.combinedWith(sensi2);
		  sensiAccumulated = sensiAccumulated.combinedWith(sensiCp).normalize();
		}
		return sensiAccumulated;
	  }

	  // spread exclusive compounding
	  private PointSensitivityBuilder compoundedSpreadExclusiveSensitivity(RatePaymentPeriod paymentPeriod, RatesProvider provider)
	  {

		double notionalAccrued = 1d;
		Currency ccy = paymentPeriod.Currency;
		PointSensitivityBuilder sensi = PointSensitivityBuilder.none();
		foreach (RateAccrualPeriod accrualPeriod in paymentPeriod.AccrualPeriods)
		{
		  double investFactor = 1 + unitNotionalAccrual(accrualPeriod, 0, provider);
		  notionalAccrued *= investFactor;
		  PointSensitivityBuilder investFactorSensi = unitNotionalSensitivityAccrual(accrualPeriod, ccy, provider).multipliedBy(1d / investFactor);
		  sensi = sensi.combinedWith(investFactorSensi);
		}
		return sensi.multipliedBy(notionalAccrued);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void explainPresentValue(RatePaymentPeriod paymentPeriod, RatesProvider provider, ExplainMapBuilder builder)
	  {
		Currency currency = paymentPeriod.Currency;
		LocalDate paymentDate = paymentPeriod.PaymentDate;

		double fxRate = this.fxRate(paymentPeriod, provider);
		double notional = paymentPeriod.Notional * fxRate;
		builder.put(ExplainKey.ENTRY_TYPE, "RatePaymentPeriod");
		builder.put(ExplainKey.PAYMENT_DATE, paymentDate);
		builder.put(ExplainKey.PAYMENT_CURRENCY, currency);
		builder.put(ExplainKey.NOTIONAL, CurrencyAmount.of(currency, notional));
		builder.put(ExplainKey.TRADE_NOTIONAL, paymentPeriod.NotionalAmount);
		if (paymentDate.isBefore(provider.ValuationDate))
		{
		  builder.put(ExplainKey.COMPLETED, true);
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.zero(currency));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.zero(currency));
		}
		else
		{
		  paymentPeriod.FxReset.ifPresent(fxReset =>
		  {
		  builder.addListEntry(ExplainKey.OBSERVATIONS, child =>
		  {
			  child.put(ExplainKey.ENTRY_TYPE, "FxObservation");
			  child.put(ExplainKey.INDEX, fxReset.Observation.Index);
			  child.put(ExplainKey.FIXING_DATE, fxReset.Observation.FixingDate);
			  child.put(ExplainKey.INDEX_VALUE, fxRate);
		  });
		  });
		  foreach (RateAccrualPeriod accrualPeriod in paymentPeriod.AccrualPeriods)
		  {
			builder.addListEntry(ExplainKey.ACCRUAL_PERIODS, child => explainPresentValue(accrualPeriod, paymentPeriod.DayCount, currency, notional, provider, child));
		  }
		  builder.put(ExplainKey.COMPOUNDING, paymentPeriod.CompoundingMethod);
		  builder.put(ExplainKey.DISCOUNT_FACTOR, provider.discountFactor(currency, paymentDate));
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.of(currency, forecastValue(paymentPeriod, provider)));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.of(currency, presentValue(paymentPeriod, provider)));
		}
	  }

	  // explain PV for an accrual period, ignoring compounding
	  private void explainPresentValue(RateAccrualPeriod accrualPeriod, DayCount dayCount, Currency currency, double notional, RatesProvider provider, ExplainMapBuilder builder)
	  {

		double rawRate = rateComputationFn.explainRate(accrualPeriod.RateComputation, accrualPeriod.StartDate, accrualPeriod.EndDate, provider, builder);
		double payOffRate = rawRate * accrualPeriod.Gearing + accrualPeriod.Spread;
		double ua = unitNotionalAccrual(accrualPeriod, accrualPeriod.Spread, provider);

		// Note that the forecast value is not published since this is potentially misleading when
		// compounding is being applied, and when it isn't then it's the same as the forecast
		// value of the payment period.

		builder.put(ExplainKey.ENTRY_TYPE, "AccrualPeriod");
		builder.put(ExplainKey.START_DATE, accrualPeriod.StartDate);
		builder.put(ExplainKey.UNADJUSTED_START_DATE, accrualPeriod.UnadjustedStartDate);
		builder.put(ExplainKey.END_DATE, accrualPeriod.EndDate);
		builder.put(ExplainKey.UNADJUSTED_END_DATE, accrualPeriod.UnadjustedEndDate);
		builder.put(ExplainKey.ACCRUAL_YEAR_FRACTION, accrualPeriod.YearFraction);
		builder.put(ExplainKey.ACCRUAL_DAYS, dayCount.days(accrualPeriod.StartDate, accrualPeriod.EndDate));
		builder.put(ExplainKey.DAYS, (int) DAYS.between(accrualPeriod.StartDate, accrualPeriod.EndDate));
		builder.put(ExplainKey.GEARING, accrualPeriod.Gearing);
		builder.put(ExplainKey.SPREAD, accrualPeriod.Spread);
		builder.put(ExplainKey.PAY_OFF_RATE, accrualPeriod.NegativeRateMethod.adjust(payOffRate));
		builder.put(ExplainKey.UNIT_AMOUNT, ua);
	  }

	  //-------------------------------------------------------------------------
	  public virtual MultiCurrencyAmount currencyExposure(RatePaymentPeriod period, RatesProvider provider)
	  {
		double df = provider.discountFactor(period.Currency, period.PaymentDate);
		if (period.FxReset.Present)
		{
		  FxReset fxReset = period.FxReset.get();
		  LocalDate fixingDate = fxReset.Observation.FixingDate;
		  FxIndexRates rates = provider.fxIndexRates(fxReset.Observation.Index);
		  if (!fixingDate.isAfter(provider.ValuationDate) && rates.Fixings.get(fixingDate).HasValue)
		  {
			double fxRate = rates.rate(fxReset.Observation, fxReset.ReferenceCurrency);
			return MultiCurrencyAmount.of(period.Currency, accrualWithNotional(period, period.Notional * fxRate * df, provider));
		  }
		  double fxRateSpotSensitivity = rates.FxForwardRates.rateFxSpotSensitivity(fxReset.ReferenceCurrency, fxReset.Observation.MaturityDate);
		  return MultiCurrencyAmount.of(fxReset.ReferenceCurrency, accrualWithNotional(period, period.Notional * fxRateSpotSensitivity * df, provider));
		}
		return MultiCurrencyAmount.of(period.Currency, accrualWithNotional(period, period.Notional * df, provider));
	  }

	  public virtual double currentCash(RatePaymentPeriod period, RatesProvider provider)
	  {
		if (provider.ValuationDate.isEqual(period.PaymentDate))
		{
		  return forecastValue(period, provider);
		}
		return 0d;
	  }

	  //-------------------------------------------------------------------------
	  // sensitivity to the spread for a payment period with FLAT compounding type
	  private double pvbpCompoundedFlat(RatePaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		int nbCmp = paymentPeriod.AccrualPeriods.size();
		double[] rate = paymentPeriod.AccrualPeriods.Select(ap => rawRate(ap, provider)).ToArray();
		double df = provider.discountFactor(paymentPeriod.Currency, paymentPeriod.PaymentDate);
		double rBar = 1.0;
		double[] cpaAccumulatedBar = new double[nbCmp + 1];
		cpaAccumulatedBar[nbCmp] = paymentPeriod.Notional * df * rBar;
		double spreadBar = 0.0d;
		for (int j = nbCmp - 1; j >= 0; j--)
		{
		  cpaAccumulatedBar[j] = (1.0d + paymentPeriod.AccrualPeriods.get(j).YearFraction * rate[j] * paymentPeriod.AccrualPeriods.get(j).Gearing) * cpaAccumulatedBar[j + 1];
		  spreadBar += paymentPeriod.AccrualPeriods.get(j).YearFraction * cpaAccumulatedBar[j + 1];
		}
		return spreadBar;
	  }

	  // sensitivity to the spread for a payment period with FLAT compounding type
	  private PointSensitivityBuilder pvbpSensitivtyCompoundedFlat(RatePaymentPeriod paymentPeriod, RatesProvider provider)
	  {
		Currency ccy = paymentPeriod.Currency;
		int nbCmp = paymentPeriod.AccrualPeriods.size();
		double[] rate = paymentPeriod.AccrualPeriods.Select(ap => rawRate(ap, provider)).ToArray();
		double df = provider.discountFactor(ccy, paymentPeriod.PaymentDate);
		double rB1 = 1.0;
		double[] cpaAccumulatedB1 = new double[nbCmp + 1];
		cpaAccumulatedB1[nbCmp] = paymentPeriod.Notional * df * rB1;
		for (int j = nbCmp - 1; j >= 0; j--)
		{
		  RateAccrualPeriod accrualPeriod = paymentPeriod.AccrualPeriods.get(j);
		  cpaAccumulatedB1[j] = (1.0d + accrualPeriod.YearFraction * rate[j] * accrualPeriod.Gearing) * cpaAccumulatedB1[j + 1];
		}
		// backward sweep
		double pvbpB2 = 1.0d;
		double[] cpaAccumulatedB1B2 = new double[nbCmp + 1];
		double[] rateB2 = new double[nbCmp];
		for (int j = 0; j < nbCmp; j++)
		{
		  RateAccrualPeriod accrualPeriod = paymentPeriod.AccrualPeriods.get(j);
		  cpaAccumulatedB1B2[j + 1] += accrualPeriod.YearFraction * pvbpB2;
		  cpaAccumulatedB1B2[j + 1] += (1.0d + accrualPeriod.YearFraction * rate[j] * accrualPeriod.Gearing) * cpaAccumulatedB1B2[j];
		  rateB2[j] += accrualPeriod.YearFraction * accrualPeriod.Gearing * cpaAccumulatedB1[j + 1] * cpaAccumulatedB1B2[j];
		}
		double dfB2 = paymentPeriod.Notional * rB1 * cpaAccumulatedB1B2[nbCmp];
		PointSensitivityBuilder dfdr = provider.discountFactors(ccy).zeroRatePointSensitivity(paymentPeriod.PaymentDate);
		PointSensitivityBuilder pvbpdr = dfdr.multipliedBy(dfB2);
		for (int j = 0; j < nbCmp; j++)
		{
		  RateAccrualPeriod accrualPeriod = paymentPeriod.AccrualPeriods.get(j);
		  pvbpdr = pvbpdr.combinedWith(rateComputationFn.rateSensitivity(accrualPeriod.RateComputation, accrualPeriod.StartDate, accrualPeriod.EndDate, provider).multipliedBy(rateB2[j]));
		}
		return pvbpdr;
	  }

	}

}