using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.cms
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.ALPHA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.BETA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.NU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.RHO;


	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using MathException = com.opengamma.strata.math.MathException;
	using RungeKuttaIntegrator1D = com.opengamma.strata.math.impl.integration.RungeKuttaIntegrator1D;
	using SabrExtrapolationRightFunction = com.opengamma.strata.pricer.impl.option.SabrExtrapolationRightFunction;
	using SabrFormulaData = com.opengamma.strata.pricer.impl.volatility.smile.SabrFormulaData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using SabrSwaptionVolatilities = com.opengamma.strata.pricer.swaption.SabrSwaptionVolatilities;
	using SwaptionSabrSensitivity = com.opengamma.strata.pricer.swaption.SwaptionSabrSensitivity;
	using SwaptionVolatilitiesName = com.opengamma.strata.pricer.swaption.SwaptionVolatilitiesName;
	using CmsPeriod = com.opengamma.strata.product.cms.CmsPeriod;
	using CmsPeriodType = com.opengamma.strata.product.cms.CmsPeriodType;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;

	/// <summary>
	///  Computes the price of a CMS coupon/caplet/floorlet by swaption replication on a shifted SABR formula with extrapolation.
	///  <para>
	///  The extrapolation is done on call prices above a certain strike. See <seealso cref="SabrExtrapolationRightFunction"/> for
	///  more details on the extrapolation method.
	/// </para>
	///  <para>
	///  The replication requires numerical integration. This is completed by <seealso cref="RungeKuttaIntegrator1D"/>.
	/// </para>
	///  <para>
	///  The consistency between {@code RatesProvider} and {@code SabrParametersSwaptionVolatilities} is not checked in this 
	///  class, but validated only once in <seealso cref="SabrExtrapolationReplicationCmsLegPricer"/>.
	/// </para>
	///  <para>
	///  Reference: Hagan, P. S. (2003). Convexity conundrums: Pricing CMS swaps, caps, and floors.
	///  Wilmott Magazine, March, pages 38--44.
	///  OpenGamma implementation note: Replication pricing for linear and TEC format CMS, Version 1.2, March 2011.
	///  OpenGamma implementation note for the extrapolation: Smile extrapolation, version 1.2, May 2011.
	/// </para>
	/// </summary>
	public sealed class SabrExtrapolationReplicationCmsPeriodPricer
	{

	  /// <summary>
	  /// Logger.
	  /// </summary>
	  private static readonly Logger log = LoggerFactory.getLogger(typeof(SabrExtrapolationReplicationCmsPeriodPricer));

	  /// <summary>
	  /// The minimal number of iterations for the numerical integration.
	  /// </summary>
	  private const int NUM_ITER = 10;
	  /// <summary>
	  /// The relative tolerance for the numerical integration in PV computation. </summary>
	  private const double REL_TOL = 1.0e-10;
	  /// <summary>
	  /// The absolute tolerance for the numerical integration in PV computation.
	  /// The numerical integration stops when the difference between two steps is below the absolute tolerance
	  /// plus the relative tolerance multiplied by the value.
	  /// </summary>
	  private const double ABS_TOL = 1.0e-8;
	  /// <summary>
	  /// The relative tolerance for the numerical integration in sensitivity computation.
	  /// </summary>
	  private const double REL_TOL_STRIKE = 1e-5;
	  /// <summary>
	  /// The relative tolerance for the numerical integration in sensitivity computation.
	  /// </summary>
	  private const double REL_TOL_VEGA = 1e-3;
	  /// <summary>
	  /// The maximum iteration count.
	  /// </summary>
	  private const int MAX_COUNT = 10;
	  /// <summary>
	  /// Shift from zero bound for floor.
	  /// To avoid numerical instability of the SABR function around 0. Shift by 0.01 bps.
	  /// </summary>
	  private const double ZERO_SHIFT = 1e-6;
	  /// <summary>
	  /// The minimal time for which the convexity adjustment is computed. The time is less than a day.
	  /// For expiry below that value, the forward rate is used for present value.
	  /// </summary>
	  private const double MIN_TIME = 1e-4;

	  /// <summary>
	  /// Pricer for the underlying swap.
	  /// </summary>
	  private readonly DiscountingSwapProductPricer swapPricer;
	  /// <summary>
	  /// The cut-off strike.
	  /// <para>
	  /// The smile is extrapolated above that level.
	  /// </para>
	  /// </summary>
	  private readonly double cutOffStrike;
	  /// <summary>
	  /// The tail thickness parameter.
	  /// <para>
	  /// This must be greater than 0 in order to ensure that the call price converges to 0 for infinite strike.
	  /// </para>
	  /// </summary>
	  private readonly double mu;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the pricer.
	  /// </summary>
	  /// <param name="swapPricer">  the pricer for underlying swap </param>
	  /// <param name="cutOffStrike">  the cut-off strike value </param>
	  /// <param name="mu">  the tail thickness </param>
	  /// <returns> the pricer </returns>
	  public static SabrExtrapolationReplicationCmsPeriodPricer of(DiscountingSwapProductPricer swapPricer, double cutOffStrike, double mu)
	  {

		return new SabrExtrapolationReplicationCmsPeriodPricer(swapPricer, cutOffStrike, mu);
	  }

	  /// <summary>
	  /// Obtains the pricer with default swap pricer.
	  /// </summary>
	  /// <param name="cutOffStrike">  the cut-off strike value </param>
	  /// <param name="mu">  the tail thickness </param>
	  /// <returns> the pricer </returns>
	  public static SabrExtrapolationReplicationCmsPeriodPricer of(double cutOffStrike, double mu)
	  {
		return of(DiscountingSwapProductPricer.DEFAULT, cutOffStrike, mu);
	  }

	  private SabrExtrapolationReplicationCmsPeriodPricer(DiscountingSwapProductPricer swapPricer, double cutOffStrike, double mu)
	  {

		this.swapPricer = ArgChecker.notNull(swapPricer, "swapPricer");
		this.cutOffStrike = cutOffStrike;
		this.mu = ArgChecker.notNegativeOrZero(mu, "mu");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value by replication in SABR framework with extrapolation on the right.
	  /// </summary>
	  /// <param name="cmsPeriod">  the CMS </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value </returns>
	  public CurrencyAmount presentValue(CmsPeriod cmsPeriod, RatesProvider provider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		Currency ccy = cmsPeriod.Currency;
		if (provider.ValuationDate.isAfter(cmsPeriod.PaymentDate))
		{
		  return CurrencyAmount.zero(ccy);
		}
		SwapIndex index = cmsPeriod.Index;
		ResolvedSwap swap = cmsPeriod.UnderlyingSwap;
		double dfPayment = provider.discountFactor(ccy, cmsPeriod.PaymentDate);
		ZonedDateTime valuationDate = swaptionVolatilities.ValuationDateTime;
		LocalDate fixingDate = cmsPeriod.FixingDate;
		double expiryTime = swaptionVolatilities.relativeTime(fixingDate.atTime(index.FixingTime).atZone(index.FixingZone));
		double tenor = swaptionVolatilities.tenor(swap.StartDate, swap.EndDate);
		double shift = swaptionVolatilities.shift(expiryTime, tenor);
		double strikeCpn = cmsPeriod.CmsPeriodType.Equals(CmsPeriodType.COUPON) ? -shift : cmsPeriod.Strike;
		if (!fixingDate.isAfter(valuationDate.toLocalDate()))
		{
		  double? fixedRate = provider.timeSeries(cmsPeriod.Index).get(fixingDate);
		  if (fixedRate.HasValue)
		  {
			double payoff = payOff(cmsPeriod.CmsPeriodType, strikeCpn, fixedRate.Value);
			return CurrencyAmount.of(ccy, dfPayment * payoff * cmsPeriod.Notional * cmsPeriod.YearFraction);
		  }
		  else if (fixingDate.isBefore(valuationDate.toLocalDate()))
		  {
			throw new System.ArgumentException(Messages.format("Unable to get fixing for {} on date {}, no time-series supplied", cmsPeriod.Index, fixingDate));
		  }
		}
		double forward = swapPricer.parRate(swap, provider);
		if (expiryTime < MIN_TIME)
		{
		  double payoff = payOff(cmsPeriod.CmsPeriodType, strikeCpn, forward);
		  return CurrencyAmount.of(ccy, dfPayment * payoff * cmsPeriod.Notional * cmsPeriod.YearFraction);
		}
		double eta = index.Template.Convention.FixedLeg.DayCount.relativeYearFraction(cmsPeriod.PaymentDate, swap.StartDate);
		CmsIntegrantProvider intProv = new CmsIntegrantProvider(this, cmsPeriod, swap, swaptionVolatilities, forward, strikeCpn, expiryTime, tenor, cutOffStrike, eta);
		double factor = dfPayment / intProv.h(forward) * intProv.g(forward);
		double strikePart = factor * intProv.k(strikeCpn) * intProv.bs(strikeCpn);
		RungeKuttaIntegrator1D integrator = new RungeKuttaIntegrator1D(ABS_TOL, REL_TOL, NUM_ITER);
		double integralPart = 0d;
		System.Func<double, double> integrant = intProv.integrant();
		try
		{
		  if (intProv.PutCall.Call)
		  {
			integralPart = dfPayment * integrateCall(integrator, integrant, swaptionVolatilities, forward, strikeCpn, expiryTime, tenor);
		  }
		  else
		  {
			integralPart = -dfPayment * integrator.integrate(integrant, -shift + ZERO_SHIFT, strikeCpn);
		  }
		}
		catch (Exception e)
		{
		  throw new MathException(e);
		}
		double priceCMS = (strikePart + integralPart);
		if (cmsPeriod.CmsPeriodType.Equals(CmsPeriodType.COUPON))
		{
		  priceCMS -= dfPayment * shift;
		}
		priceCMS *= cmsPeriod.Notional * cmsPeriod.YearFraction;
		return CurrencyAmount.of(ccy, priceCMS);
	  }

	  /// <summary>
	  /// Computes the adjusted forward rate for a CMS coupon.
	  /// <para>
	  /// The adjusted forward rate, is the number such that, multiplied by the notional, the year fraction and the payment
	  /// date discount factor, it produces the present value. In other terms, it is the number which used in the same
	  /// formula used for Ibor coupon pricing will provide the correct present value.
	  /// </para>
	  /// <para>
	  /// For period already fixed, this number will be equal to the swap index fixing.
	  /// </para>
	  /// <para>
	  /// For cap or floor the result is the adjusted forward rate for the coupon equivalent to the cap/floor, 
	  /// i.e. the coupon with the same dates and index but with no cap or floor strike.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsPeriod">  the CMS period, which should be of the type <seealso cref="CmsPeriodType#COUPON"/> </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the adjusted forward rate </returns>
	  public double adjustedForwardRate(CmsPeriod cmsPeriod, RatesProvider provider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		CmsPeriod coupon = cmsPeriod.toCouponEquivalent();
		Currency ccy = cmsPeriod.Currency;
		double dfPayment = provider.discountFactor(ccy, coupon.PaymentDate);
		double pv = presentValue(coupon, provider, swaptionVolatilities).Amount;
		return pv / (coupon.Notional * coupon.YearFraction * dfPayment);
	  }

	  /// <summary>
	  /// Computes the adjustment to the forward rate for a CMS coupon.
	  /// <para>
	  /// The adjustment to the forward rate, is the quantity that need to be added to the forward rate to obtain the 
	  /// adjusted forward rate. The adjusted forward rate is the number which used in the same formula used for 
	  /// Ibor coupon pricing (forward * notional * accrual factor * discount factor) will provide the correct present value.
	  /// </para>
	  /// <para>
	  /// For cap or floor the result is the adjustment to the forward rate for the coupon equivalent to the cap/floor, 
	  /// i.e. the coupon with the same dates and index but with no cap or floor strike.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsPeriod">  the CMS period, which should be of the type <seealso cref="CmsPeriodType#COUPON"/> </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the adjusted forward rate </returns>
	  public double adjustmentToForwardRate(CmsPeriod cmsPeriod, RatesProvider provider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		CmsPeriod coupon = cmsPeriod.toCouponEquivalent();
		double adjustedForwardRate = this.adjustedForwardRate(coupon, provider, swaptionVolatilities);
		double forward = swapPricer.parRate(coupon.UnderlyingSwap, provider);
		return adjustedForwardRate - forward;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value curve sensitivity by replication in SABR framework with extrapolation on the right.
	  /// </summary>
	  /// <param name="cmsPeriod">  the CMS </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public PointSensitivityBuilder presentValueSensitivityRates(CmsPeriod cmsPeriod, RatesProvider provider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		Currency ccy = cmsPeriod.Currency;
		if (provider.ValuationDate.isAfter(cmsPeriod.PaymentDate))
		{
		  return PointSensitivityBuilder.none();
		}
		SwapIndex index = cmsPeriod.Index;
		ResolvedSwap swap = cmsPeriod.UnderlyingSwap;
		double dfPayment = provider.discountFactor(ccy, cmsPeriod.PaymentDate);
		ZonedDateTime valuationDate = swaptionVolatilities.ValuationDateTime;
		LocalDate fixingDate = cmsPeriod.FixingDate;
		double expiryTime = swaptionVolatilities.relativeTime(fixingDate.atTime(index.FixingTime).atZone(index.FixingZone));
		double tenor = swaptionVolatilities.tenor(swap.StartDate, swap.EndDate);
		double shift = swaptionVolatilities.shift(expiryTime, tenor);
		double strikeCpn = cmsPeriod.CmsPeriodType.Equals(CmsPeriodType.COUPON) ? -shift : cmsPeriod.Strike;
		if (!fixingDate.isAfter(valuationDate.toLocalDate()))
		{
		  double? fixedRate = provider.timeSeries(cmsPeriod.Index).get(fixingDate);
		  if (fixedRate.HasValue)
		  {
			double payoff = payOff(cmsPeriod.CmsPeriodType, strikeCpn, fixedRate.Value);
			return provider.discountFactors(ccy).zeroRatePointSensitivity(cmsPeriod.PaymentDate).multipliedBy(payoff * cmsPeriod.Notional * cmsPeriod.YearFraction);
		  }
		  else if (fixingDate.isBefore(valuationDate.toLocalDate()))
		  {
			throw new System.ArgumentException(Messages.format("Unable to get fixing for {} on date {}, no time-series supplied", cmsPeriod.Index, fixingDate));
		  }
		}
		double forward = swapPricer.parRate(swap, provider);
		double eta = index.Template.Convention.FixedLeg.DayCount.relativeYearFraction(cmsPeriod.PaymentDate, swap.StartDate);
		CmsDeltaIntegrantProvider intProv = new CmsDeltaIntegrantProvider(this, cmsPeriod, swap, swaptionVolatilities, forward, strikeCpn, expiryTime, tenor, cutOffStrike, eta);
		RungeKuttaIntegrator1D integrator = new RungeKuttaIntegrator1D(ABS_TOL, REL_TOL, NUM_ITER);
		double[] bs = intProv.bsbsp(strikeCpn);
		double[] n = intProv.Nnp;
		double strikePartPrice = intProv.k(strikeCpn) * n[0] * bs[0];
		double integralPartPrice = 0d;
		double integralPart = 0d;
		System.Func<double, double> integrant = intProv.integrant();
		System.Func<double, double> integrantDelta = intProv.integrantDelta();
		try
		{
		  if (intProv.PutCall.Call)
		  {
			integralPartPrice = integrateCall(integrator, integrant, swaptionVolatilities, forward, strikeCpn, expiryTime, tenor);
			integralPart = dfPayment * integrateCall(integrator, integrantDelta, swaptionVolatilities, forward, strikeCpn, expiryTime, tenor);
		  }
		  else
		  {
			integralPartPrice = -integrator.integrate(integrant, -shift + ZERO_SHIFT, strikeCpn).Value;
			integralPart = -dfPayment * integrator.integrate(integrantDelta, -shift, strikeCpn);
		  }
		}
		catch (Exception e)
		{
		  throw new MathException(e);
		}
		double deltaPD = strikePartPrice + integralPartPrice;
		if (cmsPeriod.CmsPeriodType.Equals(CmsPeriodType.COUPON))
		{
		  deltaPD -= shift;
		}
		deltaPD *= cmsPeriod.Notional * cmsPeriod.YearFraction;
		double strikePart = dfPayment * intProv.k(strikeCpn) * (n[1] * bs[0] + n[0] * bs[1]);
		double deltaFwd = (strikePart + integralPart) * cmsPeriod.Notional * cmsPeriod.YearFraction;
		PointSensitivityBuilder sensiFwd = swapPricer.parRateSensitivity(swap, provider).multipliedBy(deltaFwd);
		PointSensitivityBuilder sensiDf = provider.discountFactors(ccy).zeroRatePointSensitivity(cmsPeriod.PaymentDate).multipliedBy(deltaPD);
		return sensiFwd.combinedWith(sensiDf);
	  }

	  /// <summary>
	  /// Computes the present value sensitivity to SABR parameters by replication in SABR framework with extrapolation on the right.
	  /// </summary>
	  /// <param name="cmsPeriod">  the CMS </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public PointSensitivityBuilder presentValueSensitivityModelParamsSabr(CmsPeriod cmsPeriod, RatesProvider provider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		Currency ccy = cmsPeriod.Currency;
		SwapIndex index = cmsPeriod.Index;
		ResolvedSwap swap = cmsPeriod.UnderlyingSwap;
		double dfPayment = provider.discountFactor(ccy, cmsPeriod.PaymentDate);
		ZonedDateTime valuationDate = swaptionVolatilities.ValuationDateTime;
		LocalDate fixingDate = cmsPeriod.FixingDate;
		ZonedDateTime expiryDate = fixingDate.atTime(index.FixingTime).atZone(index.FixingZone);
		double tenor = swaptionVolatilities.tenor(swap.StartDate, swap.EndDate);
		if (provider.ValuationDate.isAfter(cmsPeriod.PaymentDate))
		{
		  return PointSensitivityBuilder.none();
		}
		if (!fixingDate.isAfter(valuationDate.toLocalDate()))
		{
		  double? fixedRate = provider.timeSeries(cmsPeriod.Index).get(fixingDate);
		  if (fixedRate.HasValue)
		  {
			return PointSensitivityBuilder.none();
		  }
		  else if (fixingDate.isBefore(valuationDate.toLocalDate()))
		  {
			throw new System.ArgumentException(Messages.format("Unable to get fixing for {} on date {}, no time-series supplied", cmsPeriod.Index, fixingDate));
		  }
		}
		double expiryTime = swaptionVolatilities.relativeTime(expiryDate);
		double shift = swaptionVolatilities.shift(expiryTime, tenor);
		double strikeCpn = cmsPeriod.CmsPeriodType.Equals(CmsPeriodType.COUPON) ? -shift : cmsPeriod.Strike;
		double forward = swapPricer.parRate(swap, provider);
		double eta = index.Template.Convention.FixedLeg.DayCount.relativeYearFraction(cmsPeriod.PaymentDate, swap.StartDate);
		CmsIntegrantProvider intProv = new CmsIntegrantProvider(this, cmsPeriod, swap, swaptionVolatilities, forward, strikeCpn, expiryTime, tenor, cutOffStrike, eta);
		double factor = dfPayment / intProv.h(forward) * intProv.g(forward);
		double factor2 = factor * intProv.k(strikeCpn);
		double[] strikePartPrice = intProv.SabrExtrapolation.priceAdjointSabr(Math.Max(0d, strikeCpn + shift), intProv.PutCall).Derivatives.multipliedBy(factor2).toArray();
		RungeKuttaIntegrator1D integrator = new RungeKuttaIntegrator1D(ABS_TOL, REL_TOL_VEGA, NUM_ITER);
		double[] totalSensi = new double[4];
		for (int loopparameter = 0; loopparameter < 4; loopparameter++)
		{
		  double integralPart = 0d;
		  System.Func<double, double> integrant = intProv.integrantVega(loopparameter);
		  try
		  {
			if (intProv.PutCall.Call)
			{
			  integralPart = dfPayment * integrateCall(integrator, integrant, swaptionVolatilities, forward, strikeCpn, expiryTime, tenor);
			}
			else
			{
			  integralPart = -dfPayment * integrator.integrate(integrant, -shift + ZERO_SHIFT, strikeCpn);
			}
		  }
		  catch (Exception e)
		  {
			throw new Exception(e);
		  }
		  totalSensi[loopparameter] = (strikePartPrice[loopparameter] + integralPart) * cmsPeriod.Notional * cmsPeriod.YearFraction;
		}
		SwaptionVolatilitiesName name = swaptionVolatilities.Name;
		return PointSensitivityBuilder.of(SwaptionSabrSensitivity.of(name, expiryTime, tenor, ALPHA, ccy, totalSensi[0]), SwaptionSabrSensitivity.of(name, expiryTime, tenor, BETA, ccy, totalSensi[1]), SwaptionSabrSensitivity.of(name, expiryTime, tenor, RHO, ccy, totalSensi[2]), SwaptionSabrSensitivity.of(name, expiryTime, tenor, NU, ccy, totalSensi[3]));
	  }

	  /// <summary>
	  /// Computes the present value sensitivity to strike by replication in SABR framework with extrapolation on the right.
	  /// </summary>
	  /// <param name="cmsPeriod">  the CMS </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the swaption volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public double presentValueSensitivityStrike(CmsPeriod cmsPeriod, RatesProvider provider, SabrSwaptionVolatilities swaptionVolatilities)
	  {

		ArgChecker.isFalse(cmsPeriod.CmsPeriodType.Equals(CmsPeriodType.COUPON), "presentValueSensitivityStrike is not relevant for CMS coupon");
		Currency ccy = cmsPeriod.Currency;
		SwapIndex index = cmsPeriod.Index;
		if (provider.ValuationDate.isAfter(cmsPeriod.PaymentDate))
		{
		  return 0d;
		}
		ResolvedSwap swap = cmsPeriod.UnderlyingSwap;
		double dfPayment = provider.discountFactor(ccy, cmsPeriod.PaymentDate);
		ZonedDateTime valuationDate = swaptionVolatilities.ValuationDateTime;
		LocalDate fixingDate = cmsPeriod.FixingDate;
		double tenor = swaptionVolatilities.tenor(swap.StartDate, swap.EndDate);
		ZonedDateTime expiryDate = fixingDate.atTime(index.FixingTime).atZone(index.FixingZone);
		double expiryTime = swaptionVolatilities.relativeTime(expiryDate);
		double strike = cmsPeriod.Strike;
		double shift = swaptionVolatilities.shift(expiryTime, tenor);
		if (!fixingDate.isAfter(valuationDate.toLocalDate()))
		{
		  double? fixedRate = provider.timeSeries(cmsPeriod.Index).get(fixingDate);
		  if (fixedRate.HasValue)
		  {
			double payoff = 0d;
			switch (cmsPeriod.CmsPeriodType)
			{
			  case CAPLET:
				payoff = fixedRate.Value >= strike ? -1d : 0d;
				break;
			  case FLOORLET:
				payoff = fixedRate.Value < strike ? 1d : 0d;
				break;
			  default:
				throw new System.ArgumentException("unsupported CMS type");
			}
			return payoff * cmsPeriod.Notional * cmsPeriod.YearFraction * dfPayment;
		  }
		  else if (fixingDate.isBefore(valuationDate.toLocalDate()))
		  {
			throw new System.ArgumentException(Messages.format("Unable to get fixing for {} on date {}, no time-series supplied", cmsPeriod.Index, fixingDate));
		  }
		}
		double forward = swapPricer.parRate(swap, provider);
		double eta = index.Template.Convention.FixedLeg.DayCount.relativeYearFraction(cmsPeriod.PaymentDate, swap.StartDate);
		CmsIntegrantProvider intProv = new CmsIntegrantProvider(this, cmsPeriod, swap, swaptionVolatilities, forward, strike, expiryTime, tenor, cutOffStrike, eta);
		double factor = dfPayment * intProv.g(forward) / intProv.h(forward);
		RungeKuttaIntegrator1D integrator = new RungeKuttaIntegrator1D(ABS_TOL, REL_TOL_STRIKE, NUM_ITER);
		double[] kpkpp = intProv.kpkpp(strike);
		double firstPart;
		double thirdPart;
		System.Func<double, double> integrant = intProv.integrantDualDelta();
		if (intProv.PutCall.Call)
		{
		  firstPart = -kpkpp[0] * intProv.bs(strike);
		  thirdPart = integrateCall(integrator, integrant, swaptionVolatilities, forward, strike, expiryTime, tenor);
		}
		else
		{
		  firstPart = -kpkpp[0] * intProv.bs(strike);
		  thirdPart = -integrator.integrate(integrant, -shift + ZERO_SHIFT, strike).Value;
		}
		double secondPart = intProv.k(strike) * intProv.SabrExtrapolation.priceDerivativeStrike(strike + shift, intProv.PutCall);
		return cmsPeriod.Notional * cmsPeriod.YearFraction * factor * (firstPart + secondPart + thirdPart);
	  }

	  private double payOff(CmsPeriodType cmsPeriodType, double strikeCpn, double? fixedRate)
	  {
		double payoff = 0d;
		switch (cmsPeriodType.innerEnumValue)
		{
		  case CmsPeriodType.InnerEnum.CAPLET:
			payoff = Math.Max(fixedRate - strikeCpn, 0d);
			break;
		  case CmsPeriodType.InnerEnum.FLOORLET:
			payoff = Math.Max(strikeCpn - fixedRate, 0d);
			break;
		  case CmsPeriodType.InnerEnum.COUPON:
			payoff = fixedRate.Value;
			break;
		  default:
			throw new System.ArgumentException("unsupported CMS type");
		}
		return payoff;
	  }

	  private double integrateCall(RungeKuttaIntegrator1D integrator, System.Func<double, double> integrant, SabrSwaptionVolatilities swaptionVolatilities, double forward, double strike, double expiryTime, double tenor)
	  {

		double res;
		double vol = swaptionVolatilities.volatility(expiryTime, tenor, forward, forward);
		double upper0 = Math.Max(forward * Math.Exp(6d * vol * Math.Sqrt(expiryTime)), Math.Max(cutOffStrike, 2d * strike)); // To ensure that the integral covers a good part of the smile
		double upper = Math.Min(upper0, 1d); // To ensure that we don't miss the meaningful part
		res = integrator.integrate(integrant, strike, upper).Value;
		double reminder = integrant(upper) * upper;
		double error = reminder / res;
		int count = 0;
		while (Math.Abs(error) > integrator.RelativeTolerance && count < MAX_COUNT)
		{
		  res += integrator.integrate(integrant, upper, 2d * upper).Value;
		  upper *= 2d;
		  reminder = integrant(upper) * upper;
		  error = reminder / res;
		  ++count;
		  if (count == MAX_COUNT)
		  {
			log.info("Maximum iteration count, " + MAX_COUNT + ", has been reached. Relative error is greater than " + integrator.RelativeTolerance);
		  }
		}
		return res;
	  }

	  /// <summary>
	  /// Explains the present value of the CMS period.
	  /// <para>
	  /// This returns explanatory information about the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <param name="builder">  the builder to populate </param>
	  public void explainPresentValue(CmsPeriod period, RatesProvider ratesProvider, SabrSwaptionVolatilities swaptionVolatilities, ExplainMapBuilder builder)
	  {

		string type = period.CmsPeriodType.ToString();
		Currency ccy = period.Currency;
		LocalDate paymentDate = period.PaymentDate;
		builder.put(ExplainKey.ENTRY_TYPE, "Cms" + type + "Period");
		builder.put(ExplainKey.STRIKE_VALUE, period.Strike);
		builder.put(ExplainKey.NOTIONAL, CurrencyAmount.of(ccy, period.Notional));
		builder.put(ExplainKey.PAYMENT_DATE, period.PaymentDate);
		builder.put(ExplainKey.DISCOUNT_FACTOR, ratesProvider.discountFactor(ccy, paymentDate));
		builder.put(ExplainKey.START_DATE, period.StartDate);
		builder.put(ExplainKey.END_DATE, period.EndDate);
		builder.put(ExplainKey.FIXING_DATE, period.FixingDate);
		builder.put(ExplainKey.ACCRUAL_YEAR_FRACTION, period.YearFraction);
		builder.put(ExplainKey.PRESENT_VALUE, presentValue(period, ratesProvider, swaptionVolatilities));
		builder.put(ExplainKey.FORWARD_RATE, swapPricer.parRate(period.UnderlyingSwap, ratesProvider));
		builder.put(ExplainKey.CONVEXITY_ADJUSTED_RATE, adjustedForwardRate(period, ratesProvider, swaptionVolatilities));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Inner class to implement the integration used in price replication.
	  /// </summary>
	  private class CmsIntegrantProvider
	  {
		  private readonly SabrExtrapolationReplicationCmsPeriodPricer outerInstance;

		/* Small parameter below which a value is regarded as 0. */
		protected internal const double EPS = 1.0E-4;
		internal readonly int nbFixedPeriod;
		internal readonly int nbFixedPaymentYear;
		internal readonly double tau;
		internal readonly double eta;
		internal readonly double strike;
		internal readonly double shift;
		internal readonly double factor;
		internal readonly SabrExtrapolationRightFunction sabrExtrapolation;
		internal readonly PutCall putCall;
		internal readonly double[] g0;

		/// <summary>
		/// Gets the tau field.
		/// </summary>
		/// <returns> the tau </returns>
		public virtual double Tau
		{
			get
			{
			  return tau;
			}
		}

		/// <summary>
		/// Gets the eta field.
		/// </summary>
		/// <returns> the eta </returns>
		public virtual double Eta
		{
			get
			{
			  return eta;
			}
		}

		/// <summary>
		/// Gets the putCall field.
		/// </summary>
		/// <returns> the putCall </returns>
		public virtual PutCall PutCall
		{
			get
			{
			  return putCall;
			}
		}

		/// <summary>
		/// Gets the strike field.
		/// </summary>
		/// <returns> the strike </returns>
		protected internal virtual double Strike
		{
			get
			{
			  return strike;
			}
		}

		/// <summary>
		/// Gets the shift field.
		/// </summary>
		/// <returns> the shift </returns>
		protected internal virtual double Shift
		{
			get
			{
			  return shift;
			}
		}

		/// <summary>
		/// Gets the sabrExtrapolation field.
		/// </summary>
		/// <returns> the sabrExtrapolation </returns>
		public virtual SabrExtrapolationRightFunction SabrExtrapolation
		{
			get
			{
			  return sabrExtrapolation;
			}
		}

		public CmsIntegrantProvider(SabrExtrapolationReplicationCmsPeriodPricer outerInstance, CmsPeriod cmsPeriod, ResolvedSwap swap, SabrSwaptionVolatilities swaptionVolatilities, double forward, double strike, double timeToExpiry, double tenor, double cutOffStrike, double eta)
		{
			this.outerInstance = outerInstance;

		  ResolvedSwapLeg fixedLeg = swap.getLegs(SwapLegType.FIXED).get(0);
		  this.nbFixedPeriod = fixedLeg.PaymentPeriods.size();
		  this.nbFixedPaymentYear = (int) (long)Math.Round(1d / ((RatePaymentPeriod) fixedLeg.PaymentPeriods.get(0)).AccrualPeriods.get(0).YearFraction, MidpointRounding.AwayFromZero);
		  this.tau = 1d / nbFixedPaymentYear;
		  this.eta = eta;
		  SabrFormulaData sabrPoint = SabrFormulaData.of(swaptionVolatilities.alpha(timeToExpiry, tenor), swaptionVolatilities.beta(timeToExpiry, tenor), swaptionVolatilities.rho(timeToExpiry, tenor), swaptionVolatilities.nu(timeToExpiry, tenor));
		  this.shift = swaptionVolatilities.shift(timeToExpiry, tenor);
		  this.sabrExtrapolation = SabrExtrapolationRightFunction.of(forward + shift, timeToExpiry, sabrPoint, cutOffStrike + shift, outerInstance.mu);
		  this.putCall = cmsPeriod.CmsPeriodType.Equals(CmsPeriodType.FLOORLET) ? PutCall.PUT : PutCall.CALL;
		  this.strike = strike;
		  this.factor = g(forward) / h(forward);
		  this.g0 = new double[4];
		  g0[0] = nbFixedPeriod * tau;
		  g0[1] = -0.5 * nbFixedPeriod * (nbFixedPeriod + 1.0d) * tau * tau;
		  g0[2] = -2.0d / 3.0d * g0[1] * (nbFixedPeriod + 2.0d) * tau;
		  g0[3] = -3.0d / 4.0d * g0[2] * (nbFixedPeriod + 2.0d) * tau;
		}

		/// <summary>
		/// Obtains the integrant used in price replication.
		/// </summary>
		/// <returns> the integrant </returns>
		internal virtual System.Func<double, double> integrant()
		{
		  return (double? x) =>
		  {
	  double[] kD = kpkpp(x.Value);
	  // Implementation note: kD[0] contains the first derivative of k; kD[1] the second derivative of k.
	  return factor * (kD[1] * (x - strike) + 2d * kD[0]) * bs(x.Value);
		  };
		}

		/// <summary>
		/// Obtains the integrant sensitivity to the i-th SABR parameter.
		/// </summary>
		/// <param name="i">  the index of SABR parameters </param>
		/// <returns> the vega integrant </returns>
		internal virtual System.Func<double, double> integrantVega(int i)
		{
		  return (double? x) =>
		  {
	  double[] kD = kpkpp(x.Value);
	  // Implementation note: kD[0] contains the first derivative of k; kD[1] the second derivative of k.
	  double xShifted = Math.Max(x + shift, 0d); // handle tiny but negative number
	  DoubleArray priceDerivativeSabr = SabrExtrapolation.priceAdjointSabr(xShifted, putCall).Derivatives;
	  return priceDerivativeSabr.get(i) * (factor * (kD[1] * (x - strike) + 2d * kD[0]));
		  };
		}

		/// <summary>
		/// Obtains the integrant sensitivity to strike.
		/// </summary>
		/// <returns> the dual delta integrant </returns>
		internal virtual System.Func<double, double> integrantDualDelta()
		{
		  return (double? x) =>
		  {
	  double[] kD = kpkpp(x.Value);
	  // Implementation note: kD[0] contains the first derivative of k; kD[1] the second derivative of k.
	  return -kD[1] * bs(x.Value);
		  };
		}

		/// <summary>
		/// The approximation of the discount factor as function of the swap rate.
		/// </summary>
		/// <param name="x">  the swap rate. </param>
		/// <returns> the discount factor. </returns>
		internal virtual double h(double x)
		{
		  return Math.Pow(1d + tau * x, eta);
		}

		/// <summary>
		/// The cash annuity.
		/// </summary>
		/// <param name="x">  the swap rate. </param>
		/// <returns> the annuity. </returns>
		internal virtual double g(double x)
		{
		  if (Math.Abs(x) >= EPS)
		  {
			double periodFactor = 1d + x / nbFixedPaymentYear;
			double nPeriodDiscount = Math.Pow(periodFactor, -nbFixedPeriod);
			return (1d - nPeriodDiscount) / x;
		  }
		  // Special case when x ~ 0: expansion of g around 0
		  return g0[0] + g0[1] * x + 0.5 * g0[2] * x * x + g0[3] * x * x * x / 6.0d;
		}

		/// <summary>
		/// The cash annuity.
		/// </summary>
		/// <param name="x">  the swap rate. </param>
		/// <returns> the annuity. </returns>
		internal virtual double[] ggpgpp(double x)
		{
		  if (Math.Abs(x) >= EPS)
		  {
			double periodFactor = 1d + x / nbFixedPaymentYear;
			double nPeriodDiscount = Math.Pow(periodFactor, -nbFixedPeriod);
			double[] ggpgpp = new double[3];
			ggpgpp[0] = (1d - nPeriodDiscount) / x;
			ggpgpp[1] = -ggpgpp[0] / x + nbFixedPeriod * nPeriodDiscount / (x * nbFixedPaymentYear * periodFactor);
			ggpgpp[2] = 2d / (x * x) * ggpgpp[0] - 2d * nbFixedPeriod * nPeriodDiscount / (x * x * nbFixedPaymentYear * periodFactor) - (nbFixedPeriod + 1d) * nbFixedPeriod * nPeriodDiscount / (x * nbFixedPaymentYear * nbFixedPaymentYear * periodFactor * periodFactor);
			return ggpgpp;
		  }
		  // Special case when x ~ 0: expansion of g around 0
		  return new double[] {g0[0] + g0[1] * x + 0.5 * g0[2] * x * x + g0[3] * x * x * x / 6.0d, g0[1] + g0[2] * x + 0.5 * g0[3] * x * x, g0[2] + g0[3] * x};
		}

		/// <summary>
		/// The factor used in the strike part and in the integration of the replication.
		/// </summary>
		/// <param name="x">  the swap rate. </param>
		/// <returns> the factor. </returns>
		internal virtual double k(double x)
		{
		  double g = this.g(x);
		  double h = Math.Pow(1.0 + tau * x, eta);
		  return h / g;
		}

		/// <summary>
		/// The first and second derivative of the function k.
		/// <para>
		/// The first element is the first derivative and the second element is second derivative.
		/// 
		/// </para>
		/// </summary>
		/// <param name="x">  the swap rate. </param>
		/// <returns> the derivatives </returns>
		protected internal virtual double[] kpkpp(double x)
		{
		  double periodFactor = 1d + x / nbFixedPaymentYear;
		  /// <summary>
		  /// The value of the annuity and its first and second derivative.
		  /// </summary>
		  double[] ggpgpp = this.ggpgpp(x);
		  double h = Math.Pow(1d + tau * x, eta);
		  double hp = eta * tau * h / periodFactor;
		  double hpp = (eta - 1d) * tau * hp / periodFactor;
		  double kp = hp / ggpgpp[0] - h * ggpgpp[1] / (ggpgpp[0] * ggpgpp[0]);
		  double kpp = hpp / ggpgpp[0] - 2d * hp * ggpgpp[1] / (ggpgpp[0] * ggpgpp[0]) - h * (ggpgpp[2] / (ggpgpp[0] * ggpgpp[0]) - 2d * (ggpgpp[1] * ggpgpp[1]) / (ggpgpp[0] * ggpgpp[0] * ggpgpp[0]));
		  return new double[] {kp, kpp};
		}

		/// <summary>
		/// The Black price with numeraire 1 as function of the strike.
		/// </summary>
		/// <param name="strike">  the strike. </param>
		/// <returns> the Black prcie. </returns>
		internal virtual double bs(double strike)
		{
		  double strikeShifted = Math.Max(strike + Shift, 0d); // handle tiny but negative number
		  return sabrExtrapolation.price(strikeShifted, putCall);
		}
	  }

	  /// <summary>
	  /// Inner class to implement the integration used for delta calculation.
	  /// </summary>
	  private class CmsDeltaIntegrantProvider : CmsIntegrantProvider
	  {
		  private readonly SabrExtrapolationReplicationCmsPeriodPricer outerInstance;


//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal readonly double[] nnp_Renamed;

		public CmsDeltaIntegrantProvider(SabrExtrapolationReplicationCmsPeriodPricer outerInstance, CmsPeriod cmsPeriod, ResolvedSwap swap, SabrSwaptionVolatilities swaptionVolatilities, double forward, double strike, double timeToExpiry, double tenor, double cutOffStrike, double eta) : base(outerInstance, cmsPeriod, swap, swaptionVolatilities, forward, strike, timeToExpiry, tenor, cutOffStrike, eta)
		{
			this.outerInstance = outerInstance;
		  this.nnp_Renamed = nnp(forward);
		}

		/// <summary>
		/// Gets the nnp field.
		/// </summary>
		/// <returns> the nnp </returns>
		public virtual double[] Nnp
		{
			get
			{
			  return nnp_Renamed;
			}
		}

		/// <summary>
		/// Obtains the integrant sensitivity to forward.
		/// </summary>
		/// <returns> the delta integrant </returns>
		internal virtual System.Func<double, double> integrantDelta()
		{
		  return (double? x) =>
		  {
	  double[] kD = kpkpp(x.Value);
	  // Implementation note: kD[0] contains the first derivative of k; kD[1] the second derivative of k.
	  double[] bs = bsbsp(x.Value);
	  return (kD[1] * (x - Strike) + 2d * kD[0]) * (nnp_Renamed[1] * bs[0] + nnp_Renamed[0] * bs[1]);
		  };
		}

		/// <summary>
		/// The Black price and its derivative with respect to the forward.
		/// </summary>
		/// <param name="strike">  the strike. </param>
		/// <returns> the Black price and its derivative. </returns>
		internal virtual double[] bsbsp(double strike)
		{
		  double[] result = new double[2];
		  double strikeShifted = Math.Max(strike + Shift, 0d); // handle tiny but negative number
		  result[0] = SabrExtrapolation.price(strikeShifted, PutCall);
		  result[1] = SabrExtrapolation.priceDerivativeForward(strikeShifted, PutCall);
		  return result;
		}

		internal virtual double[] nnp(double x)
		{
		  double[] result = new double[2];
		  double[] ggpgpp = this.ggpgpp(x);
		  double[] hhp = this.hhp(x);
		  result[0] = ggpgpp[0] / hhp[0];
		  result[1] = ggpgpp[1] / hhp[0] - ggpgpp[0] * hhp[1] / (hhp[0] * hhp[0]);
		  return result;
		}

		internal virtual double[] hhp(double x)
		{
		  double[] result = new double[2];
		  result[0] = Math.Pow(1d + Tau * x, Eta);
		  result[1] = Eta * Tau * result[0] / (1d + x * Tau);
		  return result;
		}
	  }

	}

}