using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{


	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using BracketRoot = com.opengamma.strata.math.impl.rootfinding.BracketRoot;
	using BrentSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.BrentSingleRootFinder;
	using RealSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.RealSingleRootFinder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using Security = com.opengamma.strata.product.Security;
	using CapitalIndexedBondPaymentPeriod = com.opengamma.strata.product.bond.CapitalIndexedBondPaymentPeriod;
	using CapitalIndexedBondYieldConvention = com.opengamma.strata.product.bond.CapitalIndexedBondYieldConvention;
	using ResolvedCapitalIndexedBond = com.opengamma.strata.product.bond.ResolvedCapitalIndexedBond;
	using InflationEndInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationEndInterpolatedRateComputation;
	using InflationEndMonthRateComputation = com.opengamma.strata.product.rate.InflationEndMonthRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Pricer for capital indexed bond products.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedCapitalIndexedBond"/>.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
	public class DiscountingCapitalIndexedBondProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingCapitalIndexedBondProductPricer DEFAULT = new DiscountingCapitalIndexedBondProductPricer(DiscountingCapitalIndexedBondPaymentPeriodPricer.DEFAULT);
	  /// <summary>
	  /// The root finder.
	  /// </summary>
	  private static readonly RealSingleRootFinder ROOT_FINDER = new BrentSingleRootFinder();
	  /// <summary>
	  /// Brackets a root.
	  /// </summary>
	  private static readonly BracketRoot ROOT_BRACKETER = new BracketRoot();
	  /// <summary>
	  /// Small parameter used in finite difference approximation.
	  /// </summary>
	  private const double FD_EPS = 1.0e-5;
	  /// <summary>
	  /// Pricer for <seealso cref="CapitalIndexedBondPaymentPeriod"/>.
	  /// </summary>
	  private readonly DiscountingCapitalIndexedBondPaymentPeriodPricer periodPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="periodPricer">  the pricer for <seealso cref="CapitalIndexedBondPaymentPeriod"/>. </param>
	  public DiscountingCapitalIndexedBondProductPricer(DiscountingCapitalIndexedBondPaymentPeriodPricer periodPricer)
	  {
		this.periodPricer = ArgChecker.notNull(periodPricer, "periodPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the period pricer.
	  /// </summary>
	  /// <returns> the period pricer </returns>
	  public virtual DiscountingCapitalIndexedBondPaymentPeriodPricer PeriodPricer
	  {
		  get
		  {
			return periodPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the bond.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// The result is expressed using the payment currency of the bond.
	  /// </para>
	  /// <para>
	  /// Coupon payments of the product are considered based on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		validate(ratesProvider, discountingProvider);
		return presentValue(bond, ratesProvider, discountingProvider, ratesProvider.ValuationDate);
	  }

	  // calculate the present value
	  internal virtual CurrencyAmount presentValue(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, LocalDate referenceDate)
	  {

		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bond, discountingProvider);
		double pvNominal = periodPricer.presentValue(bond.NominalPayment, ratesProvider, issuerDf);
		double pvCoupon = 0d;
		foreach (CapitalIndexedBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if ((bond.hasExCouponPeriod() && period.DetachmentDate.isAfter(referenceDate)) || (!bond.hasExCouponPeriod() && period.PaymentDate.isAfter(referenceDate)))
		  {
			pvCoupon += periodPricer.presentValue(period, ratesProvider, issuerDf);
		  }
		}
		return CurrencyAmount.of(bond.Currency, pvCoupon + pvNominal);
	  }

	  /// <summary>
	  /// Calculates the present value of the bond product with z-spread.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// The result is expressed using the payment currency of the bond.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or
	  /// periodic compounded rates of the issuer discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value of the bond product </returns>
	  public virtual CurrencyAmount presentValueWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		validate(ratesProvider, discountingProvider);
		return presentValueWithZSpread(bond, ratesProvider, discountingProvider, ratesProvider.ValuationDate, zSpread, compoundedRateType, periodsPerYear);
	  }

	  // calculate the present value
	  internal virtual CurrencyAmount presentValueWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, LocalDate referenceDate, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bond, discountingProvider);
		double pvNominal = periodPricer.presentValueWithZSpread(bond.NominalPayment, ratesProvider, issuerDf, zSpread, compoundedRateType, periodsPerYear);
		double pvCoupon = 0d;
		foreach (CapitalIndexedBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if ((bond.hasExCouponPeriod() && period.DetachmentDate.isAfter(referenceDate)) || (!bond.hasExCouponPeriod() && period.PaymentDate.isAfter(referenceDate)))
		  {
			pvCoupon += periodPricer.presentValueWithZSpread(period, ratesProvider, issuerDf, zSpread, compoundedRateType, periodsPerYear);
		  }
		}
		return CurrencyAmount.of(bond.Currency, pvCoupon + pvNominal);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the bond product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <returns> the present value curve sensitivity of the product </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {

		validate(ratesProvider, discountingProvider);
		return presentValueSensitivity(bond, ratesProvider, discountingProvider, ratesProvider.ValuationDate);
	  }

	  // calculate the present value sensitivity
	  internal virtual PointSensitivityBuilder presentValueSensitivity(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, LocalDate referenceDate)
	  {

		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bond, discountingProvider);
		PointSensitivityBuilder pointNominal = periodPricer.presentValueSensitivity(bond.NominalPayment, ratesProvider, issuerDf);
		PointSensitivityBuilder pointCoupon = PointSensitivityBuilder.none();
		foreach (CapitalIndexedBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if ((bond.hasExCouponPeriod() && period.DetachmentDate.isAfter(referenceDate)) || (!bond.hasExCouponPeriod() && period.PaymentDate.isAfter(referenceDate)))
		  {
			pointCoupon = pointCoupon.combinedWith(periodPricer.presentValueSensitivity(period, ratesProvider, issuerDf));
		  }
		}
		return pointNominal.combinedWith(pointCoupon);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the bond product with z-spread.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or
	  /// periodic compounded rates of the issuer discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value curve sensitivity of the product </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		validate(ratesProvider, discountingProvider);
		return presentValueSensitivityWithZSpread(bond, ratesProvider, discountingProvider, ratesProvider.ValuationDate, zSpread, compoundedRateType, periodsPerYear);
	  }

	  // calculate the present value sensitivity
	  internal virtual PointSensitivityBuilder presentValueSensitivityWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, LocalDate referenceDate, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bond, discountingProvider);
		PointSensitivityBuilder pointNominal = periodPricer.presentValueSensitivityWithZSpread(bond.NominalPayment, ratesProvider, issuerDf, zSpread, compoundedRateType, periodsPerYear);
		PointSensitivityBuilder pointCoupon = PointSensitivityBuilder.none();
		foreach (CapitalIndexedBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if ((bond.hasExCouponPeriod() && period.DetachmentDate.isAfter(referenceDate)) || (!bond.hasExCouponPeriod() && period.PaymentDate.isAfter(referenceDate)))
		  {
			pointCoupon = pointCoupon.combinedWith(periodPricer.presentValueSensitivityWithZSpread(period, ratesProvider, issuerDf, zSpread, compoundedRateType, periodsPerYear));
		  }
		}
		return pointNominal.combinedWith(pointCoupon);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the bond product.
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <returns> the currency exposure of the product  </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, LocalDate referenceDate)
	  {

		return MultiCurrencyAmount.of(presentValue(bond, ratesProvider, discountingProvider, referenceDate));
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the bond product with z-spread.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or
	  /// periodic compounded rates of the issuer discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the currency exposure of the product  </returns>
	  public virtual MultiCurrencyAmount currencyExposureWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, LocalDate referenceDate, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		return MultiCurrencyAmount.of(presentValueWithZSpread(bond, ratesProvider, discountingProvider, referenceDate, zSpread, compoundedRateType, periodsPerYear));
	  }

	  /// <summary>
	  /// Calculates the current cash of the bond product.
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <returns> the current cash of the product  </returns>
	  public virtual CurrencyAmount currentCash(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate)
	  {

		LocalDate valuationDate = ratesProvider.ValuationDate;
		Currency currency = bond.Currency;
		CurrencyAmount currentCash = CurrencyAmount.zero(currency);
		if (settlementDate.isBefore(valuationDate))
		{
		  double cashCoupon = bond.hasExCouponPeriod() ? 0d : currentCashPayment(bond, ratesProvider, valuationDate);
		  CapitalIndexedBondPaymentPeriod nominal = bond.NominalPayment;
		  double cashNominal = nominal.PaymentDate.isEqual(valuationDate) ? periodPricer.forecastValue(nominal, ratesProvider) : 0d;
		  currentCash = currentCash.plus(CurrencyAmount.of(currency, cashCoupon + cashNominal));
		}
		return currentCash;
	  }

	  private double currentCashPayment(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate valuationDate)
	  {

		double cash = 0d;
		foreach (CapitalIndexedBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if (period.PaymentDate.isEqual(valuationDate))
		  {
			cash += periodPricer.forecastValue(period, ratesProvider);
		  }
		}
		return cash;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the dirty price of the bond security.
	  /// <para>
	  /// The bond is represented as <seealso cref="Security"/> where standard ID of the bond is stored.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bonds. For example, a price of 99.32% is represented in Strata by 0.9932.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <returns> the dirty price of the bond security </returns>
	  public virtual double dirtyNominalPriceFromCurves(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = bond.calculateSettlementDateFromValuation(ratesProvider.ValuationDate, refData);
		return dirtyNominalPriceFromCurves(bond, ratesProvider, discountingProvider, settlementDate);
	  }

	  // calculate the dirty price
	  internal virtual double dirtyNominalPriceFromCurves(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, LocalDate settlementDate)
	  {

		CurrencyAmount pv = presentValue(bond, ratesProvider, discountingProvider, settlementDate);
		RepoCurveDiscountFactors repoDf = repoCurveDf(bond, discountingProvider);
		double df = repoDf.discountFactor(settlementDate);
		double notional = bond.Notional;
		return pv.Amount / (df * notional);
	  }

	  /// <summary>
	  /// Calculates the dirty price of the bond security with z-spread.
	  /// <para>
	  /// The bond is represented as <seealso cref="Security"/> where standard ID of the bond is stored.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the dirty price of the bond security </returns>
	  public virtual double dirtyNominalPriceFromCurvesWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = bond.calculateSettlementDateFromValuation(ratesProvider.ValuationDate, refData);
		return dirtyNominalPriceFromCurvesWithZSpread(bond, ratesProvider, discountingProvider, settlementDate, zSpread, compoundedRateType, periodsPerYear);
	  }

	  // calculate the dirty price
	  internal virtual double dirtyNominalPriceFromCurvesWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, LocalDate settlementDate, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		CurrencyAmount pv = presentValueWithZSpread(bond, ratesProvider, discountingProvider, settlementDate, zSpread, compoundedRateType, periodsPerYear);
		RepoCurveDiscountFactors repoDf = repoCurveDf(bond, discountingProvider);
		double df = repoDf.discountFactor(settlementDate);
		double notional = bond.Notional;
		return pv.Amount / (df * notional);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the dirty price sensitivity of the bond security.
	  /// <para>
	  /// The dirty price sensitivity of the security is the sensitivity of the dirty price value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <returns> the dirty price curve sensitivity of the security </returns>
	  public virtual PointSensitivityBuilder dirtyNominalPriceSensitivity(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = bond.calculateSettlementDateFromValuation(ratesProvider.ValuationDate, refData);
		return dirtyNominalPriceSensitivity(bond, ratesProvider, discountingProvider, settlementDate);
	  }

	  // calculate the dirty price sensitivity
	  internal virtual PointSensitivityBuilder dirtyNominalPriceSensitivity(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, LocalDate settlementDate)
	  {

		double notional = bond.Notional;
		CurrencyAmount pv = presentValue(bond, ratesProvider, discountingProvider, settlementDate);
		RepoCurveDiscountFactors repoDf = repoCurveDf(bond, discountingProvider);
		double df = repoDf.discountFactor(settlementDate);
		PointSensitivityBuilder pvSensi = presentValueSensitivity(bond, ratesProvider, discountingProvider, settlementDate).multipliedBy(1d / (df * notional));
		RepoCurveZeroRateSensitivity dfSensi = repoDf.zeroRatePointSensitivity(settlementDate).multipliedBy(-pv.Amount / (df * df * notional));
		return pvSensi.combinedWith(dfSensi);
	  }

	  /// <summary>
	  /// Calculates the dirty price sensitivity of the bond security with z-spread.
	  /// <para>
	  /// The dirty price sensitivity of the security is the sensitivity of the dirty price value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the dirty price curve sensitivity of the security </returns>
	  public virtual PointSensitivityBuilder dirtyNominalPriceSensitivityWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = bond.calculateSettlementDateFromValuation(ratesProvider.ValuationDate, refData);
		return dirtyNominalPriceSensitivityWithZSpread(bond, ratesProvider, discountingProvider, settlementDate, zSpread, compoundedRateType, periodsPerYear);
	  }

	  // calculate the dirty price sensitivity
	  internal virtual PointSensitivityBuilder dirtyNominalPriceSensitivityWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, LocalDate settlementDate, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		double notional = bond.Notional;
		CurrencyAmount pv = presentValueWithZSpread(bond, ratesProvider, discountingProvider, settlementDate, zSpread, compoundedRateType, periodsPerYear);
		RepoCurveDiscountFactors repoDf = repoCurveDf(bond, discountingProvider);
		double df = repoDf.discountFactor(settlementDate);
		PointSensitivityBuilder pvSensi = presentValueSensitivityWithZSpread(bond, ratesProvider, discountingProvider, settlementDate, zSpread, compoundedRateType, periodsPerYear).multipliedBy(1d / (df * notional));
		RepoCurveZeroRateSensitivity dfSensi = repoDf.zeroRatePointSensitivity(settlementDate).multipliedBy(-pv.Amount / df / df / notional);
		return pvSensi.combinedWith(dfSensi);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the dirty price from the conventional real yield.
	  /// <para>
	  /// The resulting dirty price is real price or nominal price depending on the yield convention.
	  /// </para>
	  /// <para>
	  /// The input yield and output are expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the dirty price of the product  </returns>
	  public virtual double dirtyPriceFromRealYield(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double yield)
	  {

		ArgChecker.isTrue(settlementDate.isBefore(bond.UnadjustedEndDate), "settlement date must be before end date");
		int periodIndex = bond.findPeriodIndex(settlementDate).orElseThrow(() => new System.ArgumentException("Date outside range of bond"));
		CapitalIndexedBondPaymentPeriod period = bond.PeriodicPayments.get(periodIndex);
		int nbCoupon = bond.PeriodicPayments.size() - periodIndex;
		double couponPerYear = bond.Frequency.eventsPerYear();
		CapitalIndexedBondYieldConvention yieldConvention = bond.YieldConvention;
		if (yieldConvention.Equals(CapitalIndexedBondYieldConvention.US_IL_REAL))
		{
		  double pvAtFirstCoupon;
		  double cpnRate = bond.PeriodicPayments.get(0).RealCoupon;
		  if (Math.Abs(yield) > 1.0E-8)
		  {
			double factorOnPeriod = 1d + yield / couponPerYear;
			double vn = Math.Pow(factorOnPeriod, 1 - nbCoupon);
			pvAtFirstCoupon = cpnRate * couponPerYear / yield * (factorOnPeriod - vn) + vn;
		  }
		  else
		  {
			pvAtFirstCoupon = cpnRate * nbCoupon + 1d;
		  }
		  return pvAtFirstCoupon / (1d + factorToNextCoupon(bond, settlementDate) * yield / couponPerYear);
		}

		double realRate = period.RealCoupon;
		double firstYearFraction = bond.yearFraction(period.UnadjustedStartDate, period.UnadjustedEndDate);
		double v = 1d / (1d + yield / couponPerYear);
		double rs = ratioPeriodToNextCoupon(period, settlementDate);
		if (yieldConvention.Equals(CapitalIndexedBondYieldConvention.GB_IL_FLOAT))
		{
		  RateComputation obs = period.RateComputation;
		  LocalDateDoubleTimeSeries ts = ratesProvider.priceIndexValues(bond.RateCalculation.Index).Fixings;
		  YearMonth lastKnownFixingMonth = YearMonth.from(ts.LatestDate);
		  double indexRatio = ts.LatestValue / bond.FirstIndexValue;
		  YearMonth endFixingMonth = null;
		  if (obs is InflationEndInterpolatedRateComputation)
		  {
			endFixingMonth = ((InflationEndInterpolatedRateComputation) obs).EndSecondObservation.FixingMonth;
		  }
		  else if (obs is InflationEndMonthRateComputation)
		  {
			endFixingMonth = ((InflationEndMonthRateComputation) obs).EndObservation.FixingMonth;
		  }
		  else
		  {
			throw new System.ArgumentException("The rate observation " + obs.ToString() + " is not supported.");
		  }
		  double nbMonth = Math.Abs(MONTHS.between(endFixingMonth, lastKnownFixingMonth));
		  double u = Math.Sqrt(1d / 1.03);
		  double a = indexRatio * Math.Pow(u, nbMonth / 6d);
		  if (nbCoupon == 1)
		  {
			return (realRate + 1d) * a / u * Math.Pow(u * v, rs);
		  }
		  else
		  {
			double firstCashFlow = firstYearFraction * realRate * indexRatio * couponPerYear;
			CapitalIndexedBondPaymentPeriod secondPeriod = bond.PeriodicPayments.get(periodIndex + 1);
			double secondYearFraction = bond.yearFraction(secondPeriod.UnadjustedStartDate, secondPeriod.UnadjustedEndDate);
			double secondCashFlow = secondYearFraction * realRate * indexRatio * couponPerYear;
			double vn = Math.Pow(v, nbCoupon - 1);
			double pvAtFirstCoupon = firstCashFlow + secondCashFlow * u * v + a * realRate * v * v * (1d - vn / v) / (1d - v) + a * vn;
			return pvAtFirstCoupon * Math.Pow(u * v, rs);
		  }
		}
		if (yieldConvention.Equals(CapitalIndexedBondYieldConvention.GB_IL_BOND))
		{
		  double indexRatio = this.indexRatio(bond, ratesProvider, settlementDate);
		  double firstCashFlow = realRate * indexRatio * firstYearFraction * couponPerYear;
		  if (nbCoupon == 1)
		  {
			return Math.Pow(v, rs) * (firstCashFlow + 1d);
		  }
		  else
		  {
			CapitalIndexedBondPaymentPeriod secondPeriod = bond.PeriodicPayments.get(periodIndex + 1);
			double secondYearFraction = bond.yearFraction(secondPeriod.UnadjustedStartDate, secondPeriod.UnadjustedEndDate);
			double secondCashFlow = realRate * indexRatio * secondYearFraction * couponPerYear;
			double vn = Math.Pow(v, nbCoupon - 1);
			double pvAtFirstCoupon = firstCashFlow + secondCashFlow * v + realRate * v * v * (1d - vn / v) / (1d - v) + vn;
			return pvAtFirstCoupon * Math.Pow(v, rs);
		  }
		}
		if (yieldConvention.Equals(CapitalIndexedBondYieldConvention.JP_IL_SIMPLE))
		{
		  LocalDate maturityDate = bond.EndDate;
		  double maturity = bond.yearFraction(settlementDate, maturityDate);
		  double cleanPrice = (1d + realRate * couponPerYear * maturity) / (1d + yield * maturity);
		  return dirtyRealPriceFromCleanRealPrice(bond, settlementDate, cleanPrice);
		}
		if (yieldConvention.Equals(CapitalIndexedBondYieldConvention.JP_IL_COMPOUND))
		{
		  double pvAtFirstCoupon = 0d;
		  for (int loopcpn = 0; loopcpn < nbCoupon; loopcpn++)
		  {
			CapitalIndexedBondPaymentPeriod paymentPeriod = bond.PeriodicPayments.get(loopcpn + periodIndex);
			pvAtFirstCoupon += paymentPeriod.RealCoupon * Math.Pow(v, loopcpn);
		  }
		  pvAtFirstCoupon += Math.Pow(v, nbCoupon - 1);
		  double factorToNext = factorToNextCoupon(bond, settlementDate);
		  return pvAtFirstCoupon * Math.Pow(v, factorToNext);
		}
		throw new System.ArgumentException("The convention " + bond.YieldConvention.ToString() + " is not supported.");
	  }

	  /// <summary>
	  /// Computes the clean price from the conventional real yield.
	  /// <para>
	  /// The resulting clean price is real price or nominal price depending on the yield convention.
	  /// </para>
	  /// <para>
	  /// The input yield and output are expressed in fraction.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bonds. For example, a price of 99.32% is represented in Strata by 0.9932.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the clean price of the product  </returns>
	  public virtual double cleanPriceFromRealYield(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double yield)
	  {

		double dirtyPrice = dirtyPriceFromRealYield(bond, ratesProvider, settlementDate, yield);
		if (bond.YieldConvention.Equals(CapitalIndexedBondYieldConvention.GB_IL_FLOAT))
		{
		  return cleanNominalPriceFromDirtyNominalPrice(bond, ratesProvider, settlementDate, dirtyPrice);
		}
		return cleanRealPriceFromDirtyRealPrice(bond, settlementDate, dirtyPrice);
	  }

	  /// <summary>
	  /// Computes the conventional real yield from the dirty price.
	  /// <para>
	  /// The input dirty price should be real price or nominal price depending on the yield convention. This is coherent to  
	  /// the implementation of <seealso cref="#dirtyPriceFromRealYield(ResolvedCapitalIndexedBond, RatesProvider, LocalDate, double)"/>.
	  /// </para>
	  /// <para>
	  /// The input price and output are expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="dirtyPrice">  the bond dirty price </param>
	  /// <returns> the yield of the product  </returns>
	  public virtual double realYieldFromDirtyPrice(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double dirtyPrice)
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> priceResidual = new java.util.function.Function<double, double>()
		System.Func<double, double> priceResidual = (double? y) =>
		{
	return dirtyPriceFromRealYield(bond, ratesProvider, settlementDate, y.Value) - dirtyPrice;
		};
		double[] range = ROOT_BRACKETER.getBracketedPoints(priceResidual, -0.05, 0.10);
		double yield = ROOT_FINDER.getRoot(priceResidual, range[0], range[1]).Value;
		return yield;
	  }

	  /// <summary>
	  /// Computes the conventional real yield from the curves.
	  /// <para>
	  /// The yield is in the bill yield convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <returns> the yield of the product  </returns>
	  public virtual double realYieldFromCurves(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = bond.calculateSettlementDateFromValuation(ratesProvider.ValuationDate, refData);
		double dirtyPrice;
		if (bond.YieldConvention.Equals(CapitalIndexedBondYieldConvention.GB_IL_FLOAT))
		{
		  dirtyPrice = dirtyNominalPriceFromCurves(bond, ratesProvider, discountingProvider, settlementDate);
		}
		else
		{
		  double dirtyNominalPrice = dirtyNominalPriceFromCurves(bond, ratesProvider, discountingProvider, settlementDate);
		  dirtyPrice = realPriceFromNominalPrice(bond, ratesProvider, settlementDate, dirtyNominalPrice);
		}
		return realYieldFromDirtyPrice(bond, ratesProvider, settlementDate, dirtyPrice);
	  }

	  /// <summary>
	  /// Computes the dirty price from the standard yield.
	  /// <para>
	  /// The input yield and output are expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the standard yield </param>
	  /// <returns> the dirty price of the product  </returns>
	  public virtual double dirtyPriceFromStandardYield(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double yield)
	  {

		int nbCoupon = bond.PeriodicPayments.size();
		double couponPerYear = bond.Frequency.eventsPerYear();
		double factorOnPeriod = 1d + yield / couponPerYear;
		double pvAtFirstCoupon = 0d;
		int pow = 0;
		double factorToNext = factorToNextCoupon(bond, settlementDate);
		for (int loopcpn = 0; loopcpn < nbCoupon; loopcpn++)
		{
		  CapitalIndexedBondPaymentPeriod period = bond.PeriodicPayments.get(loopcpn);
		  if ((bond.hasExCouponPeriod() && !settlementDate.isAfter(period.DetachmentDate)) || (!bond.hasExCouponPeriod() && period.PaymentDate.isAfter(settlementDate)))
		  {
			pvAtFirstCoupon += period.RealCoupon / Math.Pow(factorOnPeriod, pow);
			++pow;
		  }
		}
		pvAtFirstCoupon += 1d / Math.Pow(factorOnPeriod, pow - 1);
		return pvAtFirstCoupon * Math.Pow(factorOnPeriod, -factorToNext);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the modified duration from the conventional real yield using finite difference approximation.
	  /// <para>
	  /// The modified duration is defined as the minus of the first derivative of clean price with respect to yield, 
	  /// divided by the clean price.
	  /// </para>
	  /// <para>
	  /// The clean price here is real price or nominal price depending on the yield convention. This is coherent to 
	  /// the implementation of <seealso cref="#dirtyPriceFromRealYield(ResolvedCapitalIndexedBond, RatesProvider, LocalDate, double)"/>.
	  /// </para>
	  /// <para>
	  /// The input yield and output are expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the modified duration of the product  </returns>
	  public virtual double modifiedDurationFromRealYieldFiniteDifference(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double yield)
	  {

		double price = cleanPriceFromRealYield(bond, ratesProvider, settlementDate, yield);
		double priceplus = cleanPriceFromRealYield(bond, ratesProvider, settlementDate, yield + FD_EPS);
		double priceminus = cleanPriceFromRealYield(bond, ratesProvider, settlementDate, yield - FD_EPS);
		return -0.5 * (priceplus - priceminus) / (price * FD_EPS);
	  }

	  /// <summary>
	  /// Calculates the convexity from the conventional real yield using finite difference approximation.
	  /// <para>
	  /// The convexity is defined as the second derivative of clean price with respect to yield, divided by the clean price.
	  /// </para>
	  /// <para>
	  /// The clean price here is real price or nominal price depending on the yield convention. This is coherent to 
	  /// the implementation of <seealso cref="#dirtyPriceFromRealYield(ResolvedCapitalIndexedBond, RatesProvider, LocalDate, double)"/>.
	  /// </para>
	  /// <para>
	  /// The input yield and output are expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the covexity of the product  </returns>
	  public virtual double convexityFromRealYieldFiniteDifference(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double yield)
	  {

		double price = cleanPriceFromRealYield(bond, ratesProvider, settlementDate, yield);
		double priceplus = cleanPriceFromRealYield(bond, ratesProvider, settlementDate, yield + FD_EPS);
		double priceminus = cleanPriceFromRealYield(bond, ratesProvider, settlementDate, yield - FD_EPS);
		return (priceplus - 2 * price + priceminus) / (price * FD_EPS * FD_EPS);
	  }

	  /// <summary>
	  /// Computes the modified duration from the standard yield.
	  /// <para>
	  /// The modified duration is defined as the minus of the first derivative of dirty price with respect to yield, 
	  /// divided by the dirty price.
	  /// </para>
	  /// <para>
	  /// The input yield and output are expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the standard yield </param>
	  /// <returns> the modified duration of the product  </returns>
	  public virtual double modifiedDurationFromStandardYield(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double yield)
	  {

		int nbCoupon = bond.PeriodicPayments.size();
		double couponPerYear = bond.Frequency.eventsPerYear();
		double factorOnPeriod = 1d + yield / couponPerYear;
		double mdAtFirstCoupon = 0d;
		double pvAtFirstCoupon = 0d;
		int pow = 0;
		double factorToNext = factorToNextCoupon(bond, settlementDate);
		for (int loopcpn = 0; loopcpn < nbCoupon; loopcpn++)
		{
		  CapitalIndexedBondPaymentPeriod period = bond.PeriodicPayments.get(loopcpn);
		  if ((bond.hasExCouponPeriod() && !settlementDate.isAfter(period.DetachmentDate)) || (!bond.hasExCouponPeriod() && period.PaymentDate.isAfter(settlementDate)))
		  {
			mdAtFirstCoupon += period.RealCoupon / Math.Pow(factorOnPeriod, pow + 1) * (pow + factorToNext) / couponPerYear;
			pvAtFirstCoupon += period.RealCoupon / Math.Pow(factorOnPeriod, pow);
			++pow;
		  }
		}
		mdAtFirstCoupon += (pow - 1d + factorToNext) / (couponPerYear * Math.Pow(factorOnPeriod, pow));
		pvAtFirstCoupon += 1d / Math.Pow(factorOnPeriod, pow - 1);
		double dp = pvAtFirstCoupon * Math.Pow(factorOnPeriod, -factorToNext);
		double md = mdAtFirstCoupon * Math.Pow(factorOnPeriod, -factorToNext) / dp;
		return md;
	  }

	  /// <summary>
	  /// Computes the covexity from the standard yield.
	  /// <para>
	  /// The convexity is defined as the second derivative of dirty price with respect to yield, divided by the dirty price.
	  /// </para>
	  /// <para>
	  /// The input yield and output are expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the standard yield </param>
	  /// <returns> the convexity of the product  </returns>
	  public virtual double convexityFromStandardYield(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double yield)
	  {

		int nbCoupon = bond.PeriodicPayments.size();
		double couponPerYear = bond.Frequency.eventsPerYear();
		double factorOnPeriod = 1d + yield / couponPerYear;
		double cvAtFirstCoupon = 0;
		double pvAtFirstCoupon = 0;
		int pow = 0;
		double factorToNext = factorToNextCoupon(bond, settlementDate);
		for (int loopcpn = 0; loopcpn < nbCoupon; loopcpn++)
		{
		  CapitalIndexedBondPaymentPeriod period = bond.PeriodicPayments.get(loopcpn);
		  if ((bond.hasExCouponPeriod() && !settlementDate.isAfter(period.DetachmentDate)) || (!bond.hasExCouponPeriod() && period.PaymentDate.isAfter(settlementDate)))
		  {
			cvAtFirstCoupon += period.RealCoupon * (pow + factorToNext) * (pow + factorToNext + 1d) / (Math.Pow(factorOnPeriod, pow + 2) * couponPerYear * couponPerYear);
			pvAtFirstCoupon += period.RealCoupon / Math.Pow(factorOnPeriod, pow);
			++pow;
		  }
		}
		cvAtFirstCoupon += (pow - 1d + factorToNext) * (pow + factorToNext) / (Math.Pow(factorOnPeriod, pow + 1) * couponPerYear * couponPerYear);
		pvAtFirstCoupon += 1d / Math.Pow(factorOnPeriod, pow - 1);
		double pv = pvAtFirstCoupon * Math.Pow(factorOnPeriod, -factorToNext);
		double cv = cvAtFirstCoupon * Math.Pow(factorOnPeriod, -factorToNext) / pv;
		return cv;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the dirty real price of the bond from its settlement date and clean real price.
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="cleanPrice">  the clean real price </param>
	  /// <returns> the price of the bond product </returns>
	  public virtual double dirtyRealPriceFromCleanRealPrice(ResolvedCapitalIndexedBond bond, LocalDate settlementDate, double cleanPrice)
	  {

		double notional = bond.Notional;
		return cleanPrice + bond.accruedInterest(settlementDate) / notional;
	  }

	  /// <summary>
	  /// Calculates the clean real price of the bond from its settlement date and dirty real price.
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="dirtyPrice">  the dirty real price </param>
	  /// <returns> the price of the bond product </returns>
	  public virtual double cleanRealPriceFromDirtyRealPrice(ResolvedCapitalIndexedBond bond, LocalDate settlementDate, double dirtyPrice)
	  {

		double notional = bond.Notional;
		return dirtyPrice - bond.accruedInterest(settlementDate) / notional;
	  }

	  /// <summary>
	  /// Calculates the dirty nominal price of the bond from its settlement date and clean nominal price.
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="cleanPrice">  the clean nominal price </param>
	  /// <returns> the price of the bond product </returns>
	  public virtual double dirtyNominalPriceFromCleanNominalPrice(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double cleanPrice)
	  {

		double notional = bond.Notional;
		double indexRatio = this.indexRatio(bond, ratesProvider, settlementDate);
		return cleanPrice + bond.accruedInterest(settlementDate) / notional * indexRatio;
	  }

	  /// <summary>
	  /// Calculates the clean nominal price of the bond from its settlement date and dirty nominal price.
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="dirtyPrice">  the dirty nominal price </param>
	  /// <returns> the price of the bond product </returns>
	  public virtual double cleanNominalPriceFromDirtyNominalPrice(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double dirtyPrice)
	  {

		double notional = bond.Notional;
		double indexRatio = this.indexRatio(bond, ratesProvider, settlementDate);
		return dirtyPrice - bond.accruedInterest(settlementDate) / notional * indexRatio;
	  }

	  /// <summary>
	  /// Calculates the real price of the bond from its settlement date and nominal price.
	  /// <para>
	  /// The input and output prices are both clean or dirty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="nominalPrice">  the nominal price </param>
	  /// <returns> the price of the bond product </returns>
	  public virtual double realPriceFromNominalPrice(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double nominalPrice)
	  {

		double indexRatio = this.indexRatio(bond, ratesProvider, settlementDate);
		return nominalPrice / indexRatio;
	  }

	  /// <summary>
	  /// Calculates the nominal price of the bond from its settlement date and real price.
	  /// <para>
	  /// The input and output prices are both clean or dirty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="realPrice">  the real price </param>
	  /// <returns> the price of the bond product </returns>
	  public virtual double nominalPriceFromRealPrice(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate, double realPrice)
	  {

		double indexRatio = this.indexRatio(bond, ratesProvider, settlementDate);
		return realPrice * indexRatio;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the z-spread of the bond from curves and clean price.
	  /// <para>
	  /// The input clean price is real price or nominal price depending on the yield convention.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve associated to the bond (Issuer Entity)
	  /// to match the present value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="cleanPrice">  the clean price </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the z-spread of the bond security </returns>
	  public virtual double zSpreadFromCurvesAndCleanPrice(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData, double cleanPrice, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = bond.calculateSettlementDateFromValuation(ratesProvider.ValuationDate, refData);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> residual = new java.util.function.Function<double, double>()
		System.Func<double, double> residual = (double? z) =>
		{
	double dirtyPrice = dirtyNominalPriceFromCurvesWithZSpread(bond, ratesProvider, discountingProvider, settlementDate, z.Value, compoundedRateType, periodsPerYear);
	if (bond.YieldConvention.Equals(CapitalIndexedBondYieldConvention.GB_IL_FLOAT))
	{
	  return cleanNominalPriceFromDirtyNominalPrice(bond, ratesProvider, settlementDate, dirtyPrice) - cleanPrice;
	}
	double dirtyRealPrice = realPriceFromNominalPrice(bond, ratesProvider, settlementDate, dirtyPrice);
	return cleanRealPriceFromDirtyRealPrice(bond, settlementDate, dirtyRealPrice) - cleanPrice;
		};
		double[] range = ROOT_BRACKETER.getBracketedPoints(residual, -0.5, 0.5); // Starting range is [-1%, 1%]
		return ROOT_FINDER.getRoot(residual, range[0], range[1]).Value;
	  }

	  /// <summary>
	  /// Calculates the z-spread of the bond from curves and present value.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve associated to the bond (Issuer Entity)
	  /// to match the present value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="discountingProvider">  the discount factors provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="presentValue">  the present value </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the z-spread of the bond product </returns>
	  public virtual double zSpreadFromCurvesAndPv(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider, ReferenceData refData, CurrencyAmount presentValue, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		validate(ratesProvider, discountingProvider);
		LocalDate settlementDate = bond.calculateSettlementDateFromValuation(ratesProvider.ValuationDate, refData);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> residual = new java.util.function.Function<double, double>()
		System.Func<double, double> residual = (double? z) =>
		{
	return presentValueWithZSpread(bond, ratesProvider, discountingProvider, settlementDate, z.Value, compoundedRateType, periodsPerYear).Amount - presentValue.Amount;
		};
		double[] range = ROOT_BRACKETER.getBracketedPoints(residual, -0.5, 0.5); // Starting range is [-1%, 1%]
		return ROOT_FINDER.getRoot(residual, range[0], range[1]).Value;
	  }

	  //-------------------------------------------------------------------------
	  private double ratioPeriodToNextCoupon(CapitalIndexedBondPaymentPeriod bond, LocalDate settlementDate)
	  {
		double nbDayToSpot = DAYS.between(settlementDate, bond.UnadjustedEndDate);
		double nbDaysPeriod = DAYS.between(bond.UnadjustedStartDate, bond.UnadjustedEndDate);
		return nbDayToSpot / nbDaysPeriod;
	  }

	  private double factorToNextCoupon(ResolvedCapitalIndexedBond bond, LocalDate settlementDate)
	  {
		if (bond.UnadjustedStartDate.isAfter(settlementDate))
		{
		  return 0d;
		}
		int periodIndex = bond.findPeriodIndex(settlementDate).orElseThrow(() => new System.ArgumentException("Date outside range of bond"));
		CapitalIndexedBondPaymentPeriod period = bond.PeriodicPayments.get(periodIndex);
		LocalDate previousAccrualDate = period.UnadjustedStartDate;
		double factorSpot = bond.yearFraction(previousAccrualDate, settlementDate);
		double factorPeriod = bond.yearFraction(previousAccrualDate, period.UnadjustedEndDate);
		return (factorPeriod - factorSpot) / factorPeriod;
	  }

	  internal virtual double indexRatio(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate)
	  {
		LocalDate endReferenceDate = settlementDate.isBefore(ratesProvider.ValuationDate) ? ratesProvider.ValuationDate : settlementDate;
		RateComputation modifiedComputation = bond.RateCalculation.createRateComputation(endReferenceDate);
		return 1d + periodPricer.RateComputationFn.rate(modifiedComputation, bond.UnadjustedStartDate, bond.UnadjustedEndDate, ratesProvider);
	  }

	  internal virtual PointSensitivityBuilder indexRatioSensitivity(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, LocalDate settlementDate)
	  {

		LocalDate endReferenceDate = settlementDate.isBefore(ratesProvider.ValuationDate) ? ratesProvider.ValuationDate : settlementDate;
		RateComputation modifiedComputation = bond.RateCalculation.createRateComputation(endReferenceDate);
		return periodPricer.RateComputationFn.rateSensitivity(modifiedComputation, bond.UnadjustedStartDate, bond.UnadjustedEndDate, ratesProvider);
	  }

	  private void validate(RatesProvider ratesProvider, LegalEntityDiscountingProvider discountingProvider)
	  {
		ArgChecker.isTrue(ratesProvider.ValuationDate.isEqual(discountingProvider.ValuationDate), "the rates providers should be for the same date");
	  }

	  //-------------------------------------------------------------------------
	  // compute pv of coupon payment(s) s.t. referenceDate1 < coupon <= referenceDate2
	  internal virtual double presentValueCoupon(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, IssuerCurveDiscountFactors discountFactors, LocalDate referenceDate1, LocalDate referenceDate2)
	  {

		double pvDiff = 0d;
		foreach (CapitalIndexedBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if (period.DetachmentDate.isAfter(referenceDate1) && !period.DetachmentDate.isAfter(referenceDate2))
		  {
			pvDiff += periodPricer.presentValue(period, ratesProvider, discountFactors);
		  }
		}
		return pvDiff;
	  }

	  // compute pv of coupon payment(s) s.t. referenceDate1 < coupon <= referenceDate2
	  internal virtual double presentValueCouponWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, IssuerCurveDiscountFactors discountFactors, LocalDate referenceDate1, LocalDate referenceDate2, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		double pvDiff = 0d;
		foreach (CapitalIndexedBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if (period.DetachmentDate.isAfter(referenceDate1) && !period.DetachmentDate.isAfter(referenceDate2))
		  {
			pvDiff += periodPricer.presentValueWithZSpread(period, ratesProvider, discountFactors, zSpread, compoundedRateType, periodsPerYear);
		  }
		}
		return pvDiff;
	  }

	  internal virtual PointSensitivityBuilder presentValueSensitivityCoupon(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, IssuerCurveDiscountFactors discountFactors, LocalDate referenceDate1, LocalDate referenceDate2)
	  {

		PointSensitivityBuilder pvSensiDiff = PointSensitivityBuilder.none();
		foreach (CapitalIndexedBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if (period.DetachmentDate.isAfter(referenceDate1) && !period.DetachmentDate.isAfter(referenceDate2))
		  {
			pvSensiDiff = pvSensiDiff.combinedWith(periodPricer.presentValueSensitivity(period, ratesProvider, discountFactors));
		  }
		}
		return pvSensiDiff;
	  }

	  // compute pv sensitivity of coupon payment(s) s.t. referenceDate1 < coupon <= referenceDate2
	  internal virtual PointSensitivityBuilder presentValueSensitivityCouponWithZSpread(ResolvedCapitalIndexedBond bond, RatesProvider ratesProvider, IssuerCurveDiscountFactors discountFactors, LocalDate referenceDate1, LocalDate referenceDate2, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		PointSensitivityBuilder pvSensiDiff = PointSensitivityBuilder.none();
		foreach (CapitalIndexedBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if (period.DetachmentDate.isAfter(referenceDate1) && !period.DetachmentDate.isAfter(referenceDate2))
		  {
			pvSensiDiff = pvSensiDiff.combinedWith(periodPricer.presentValueSensitivityWithZSpread(period, ratesProvider, discountFactors, zSpread, compoundedRateType, periodsPerYear));
		  }
		}
		return pvSensiDiff;
	  }

	  //-------------------------------------------------------------------------
	  // extracts the repo curve discount factors for the bond
	  internal static RepoCurveDiscountFactors repoCurveDf(ResolvedCapitalIndexedBond bond, LegalEntityDiscountingProvider provider)
	  {
		return provider.repoCurveDiscountFactors(bond.SecurityId, bond.LegalEntityId, bond.Currency);
	  }

	  // extracts the issuer curve discount factors for the bond
	  internal static IssuerCurveDiscountFactors issuerCurveDf(ResolvedCapitalIndexedBond bond, LegalEntityDiscountingProvider provider)
	  {
		return provider.issuerCurveDiscountFactors(bond.LegalEntityId, bond.Currency);
	  }

	}

}