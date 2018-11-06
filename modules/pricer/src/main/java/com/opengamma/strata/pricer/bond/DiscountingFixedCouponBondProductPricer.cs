using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.FixedCouponBondYieldConvention.DE_BONDS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.FixedCouponBondYieldConvention.GB_BUMP_DMO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.FixedCouponBondYieldConvention.JP_SIMPLE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.FixedCouponBondYieldConvention.US_STREET;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using BracketRoot = com.opengamma.strata.math.impl.rootfinding.BracketRoot;
	using BrentSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.BrentSingleRootFinder;
	using RealSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.RealSingleRootFinder;
	using Security = com.opengamma.strata.product.Security;
	using FixedCouponBondPaymentPeriod = com.opengamma.strata.product.bond.FixedCouponBondPaymentPeriod;
	using FixedCouponBondYieldConvention = com.opengamma.strata.product.bond.FixedCouponBondYieldConvention;
	using ResolvedFixedCouponBond = com.opengamma.strata.product.bond.ResolvedFixedCouponBond;

	/// <summary>
	/// Pricer for fixed coupon bond products.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedFixedCouponBond"/>.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
	public class DiscountingFixedCouponBondProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFixedCouponBondProductPricer DEFAULT = new DiscountingFixedCouponBondProductPricer(DiscountingFixedCouponBondPaymentPeriodPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// The root finder.
	  /// </summary>
	  private static readonly RealSingleRootFinder ROOT_FINDER = new BrentSingleRootFinder();
	  /// <summary>
	  /// Brackets a root.
	  /// </summary>
	  private static readonly BracketRoot ROOT_BRACKETER = new BracketRoot();

	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer nominalPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="FixedCouponBondPaymentPeriod"/>.
	  /// </summary>
	  private readonly DiscountingFixedCouponBondPaymentPeriodPricer periodPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="periodPricer">  the pricer for <seealso cref="FixedCouponBondPaymentPeriod"/> </param>
	  /// <param name="nominalPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public DiscountingFixedCouponBondProductPricer(DiscountingFixedCouponBondPaymentPeriodPricer periodPricer, DiscountingPaymentPricer nominalPricer)
	  {

		this.nominalPricer = ArgChecker.notNull(nominalPricer, "nominalPricer");
		this.periodPricer = ArgChecker.notNull(periodPricer, "periodPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the fixed coupon bond product.
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
	  /// <param name="provider">  the discounting provider </param>
	  /// <returns> the present value of the fixed coupon bond product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider)
	  {
		return presentValue(bond, provider, provider.ValuationDate);
	  }

	  // calculate the present value
	  internal virtual CurrencyAmount presentValue(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, LocalDate referenceDate)
	  {

		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bond, provider);
		CurrencyAmount pvNominal = nominalPricer.presentValue(bond.NominalPayment, issuerDf.DiscountFactors);
		CurrencyAmount pvCoupon = presentValueCoupon(bond, issuerDf, referenceDate);
		return pvNominal.plus(pvCoupon);
	  }

	  /// <summary>
	  /// Calculates the present value of the fixed coupon bond product with z-spread.
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
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value of the fixed coupon bond product </returns>
	  public virtual CurrencyAmount presentValueWithZSpread(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		return presentValueWithZSpread(bond, provider, zSpread, compoundedRateType, periodsPerYear, provider.ValuationDate);
	  }

	  // calculate the present value
	  internal virtual CurrencyAmount presentValueWithZSpread(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear, LocalDate referenceDate)
	  {

		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bond, provider);
		CurrencyAmount pvNominal = nominalPricer.presentValueWithSpread(bond.NominalPayment, issuerDf.DiscountFactors, zSpread, compoundedRateType, periodsPerYear);
		CurrencyAmount pvCoupon = presentValueCouponFromZSpread(bond, issuerDf, zSpread, compoundedRateType, periodsPerYear, referenceDate);
		return pvNominal.plus(pvCoupon);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the dirty price of the fixed coupon bond.
	  /// <para>
	  /// The fixed coupon bond is represented as <seealso cref="Security"/> where standard ID of the bond is stored.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bonds. For example, a price of 99.32% is represented in Strata by 0.9932.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <returns> the dirty price of the fixed coupon bond security </returns>
	  public virtual double dirtyPriceFromCurves(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, ReferenceData refData)
	  {

		LocalDate settlementDate = bond.SettlementDateOffset.adjust(provider.ValuationDate, refData);
		return dirtyPriceFromCurves(bond, provider, settlementDate);
	  }

	  /// <summary>
	  /// Calculates the dirty price of the fixed coupon bond under the specified settlement date.
	  /// <para>
	  /// The fixed coupon bond is represented as <seealso cref="Security"/> where standard ID of the bond is stored.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <returns> the dirty price of the fixed coupon bond security </returns>
	  public virtual double dirtyPriceFromCurves(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, LocalDate settlementDate)
	  {

		CurrencyAmount pv = presentValue(bond, provider, settlementDate);
		RepoCurveDiscountFactors repoDf = repoCurveDf(bond, provider);
		double df = repoDf.discountFactor(settlementDate);
		double notional = bond.Notional;
		return pv.Amount / df / notional;
	  }

	  /// <summary>
	  /// Calculates the dirty price of the fixed coupon bond with z-spread.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// </para>
	  /// <para>
	  /// The fixed coupon bond is represented as <seealso cref="Security"/> where standard ID of the bond is stored.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the dirty price of the fixed coupon bond security </returns>
	  public virtual double dirtyPriceFromCurvesWithZSpread(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, ReferenceData refData, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		LocalDate settlementDate = bond.SettlementDateOffset.adjust(provider.ValuationDate, refData);
		return dirtyPriceFromCurvesWithZSpread(bond, provider, zSpread, compoundedRateType, periodsPerYear, settlementDate);
	  }

	  /// <summary>
	  /// Calculates the dirty price of the fixed coupon bond under the specified settlement date with z-spread.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// </para>
	  /// <para>
	  /// The fixed coupon bond is represented as <seealso cref="Security"/> where standard ID of the bond is stored.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <returns> the dirty price of the fixed coupon bond security </returns>
	  public virtual double dirtyPriceFromCurvesWithZSpread(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear, LocalDate settlementDate)
	  {

		CurrencyAmount pv = presentValueWithZSpread(bond, provider, zSpread, compoundedRateType, periodsPerYear, settlementDate);
		RepoCurveDiscountFactors repoDf = repoCurveDf(bond, provider);
		double df = repoDf.discountFactor(settlementDate);
		double notional = bond.Notional;
		return pv.Amount / df / notional;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the dirty price of the fixed coupon bond from its settlement date and clean price.
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="cleanPrice">  the clean price </param>
	  /// <returns> the present value of the fixed coupon bond product </returns>
	  public virtual double dirtyPriceFromCleanPrice(ResolvedFixedCouponBond bond, LocalDate settlementDate, double cleanPrice)
	  {
		double notional = bond.Notional;
		double accruedInterest = this.accruedInterest(bond, settlementDate);
		return cleanPrice + accruedInterest / notional;
	  }

	  /// <summary>
	  /// Calculates the clean price of the fixed coupon bond from its settlement date and dirty price.
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bonds. For example, a price of 99.32% is represented in Strata by 0.9932.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="dirtyPrice">  the dirty price </param>
	  /// <returns> the present value of the fixed coupon bond product </returns>
	  public virtual double cleanPriceFromDirtyPrice(ResolvedFixedCouponBond bond, LocalDate settlementDate, double dirtyPrice)
	  {
		double notional = bond.Notional;
		double accruedInterest = this.accruedInterest(bond, settlementDate);
		return dirtyPrice - accruedInterest / notional;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the z-spread of the fixed coupon bond from curves and dirty price.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve associated to the bond (Issuer Entity)
	  /// to match the dirty price.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="dirtyPrice">  the dirtyPrice </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the z-spread of the fixed coupon bond security </returns>
	  public virtual double zSpreadFromCurvesAndDirtyPrice(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, ReferenceData refData, double dirtyPrice, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> residual = new java.util.function.Function<double, double>()
		System.Func<double, double> residual = (final double? z) =>
		{
	return dirtyPriceFromCurvesWithZSpread(bond, provider, refData, z, compoundedRateType, periodsPerYear) - dirtyPrice;
		};
		double[] range = ROOT_BRACKETER.getBracketedPoints(residual, -0.01, 0.01); // Starting range is [-1%, 1%]
		return ROOT_FINDER.getRoot(residual, range[0], range[1]).Value;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the fixed coupon bond product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <returns> the present value curve sensitivity of the product </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider)
	  {

		return presentValueSensitivity(bond, provider, provider.ValuationDate);
	  }

	  // calculate the present value sensitivity
	  internal virtual PointSensitivityBuilder presentValueSensitivity(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, LocalDate referenceDate)
	  {

		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bond, provider);
		PointSensitivityBuilder pvNominal = presentValueSensitivityNominal(bond, issuerDf);
		PointSensitivityBuilder pvCoupon = presentValueSensitivityCoupon(bond, issuerDf, referenceDate);
		return pvNominal.combinedWith(pvCoupon);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the fixed coupon bond with z-spread.
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
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value curve sensitivity of the product </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityWithZSpread(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		return presentValueSensitivityWithZSpread(bond, provider, zSpread, compoundedRateType, periodsPerYear, provider.ValuationDate);
	  }

	  // calculate the present value sensitivity
	  internal virtual PointSensitivityBuilder presentValueSensitivityWithZSpread(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear, LocalDate referenceDate)
	  {

		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bond, provider);
		PointSensitivityBuilder pvNominal = presentValueSensitivityNominalFromZSpread(bond, issuerDf, zSpread, compoundedRateType, periodsPerYear);
		PointSensitivityBuilder pvCoupon = presentValueSensitivityCouponFromZSpread(bond, issuerDf, zSpread, compoundedRateType, periodsPerYear, referenceDate);
		return pvNominal.combinedWith(pvCoupon);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the dirty price sensitivity of the fixed coupon bond product.
	  /// <para>
	  /// The dirty price sensitivity of the security is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <returns> the dirty price value curve sensitivity of the security </returns>
	  public virtual PointSensitivityBuilder dirtyPriceSensitivity(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, ReferenceData refData)
	  {

		LocalDate settlementDate = bond.SettlementDateOffset.adjust(provider.ValuationDate, refData);
		return dirtyPriceSensitivity(bond, provider, settlementDate);
	  }

	  // calculate the dirty price sensitivity
	  internal virtual PointSensitivityBuilder dirtyPriceSensitivity(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, LocalDate referenceDate)
	  {

		RepoCurveDiscountFactors repoDf = repoCurveDf(bond, provider);
		double df = repoDf.discountFactor(referenceDate);
		CurrencyAmount pv = presentValue(bond, provider);
		double notional = bond.Notional;
		PointSensitivityBuilder pvSensi = presentValueSensitivity(bond, provider).multipliedBy(1d / df / notional);
		RepoCurveZeroRateSensitivity dfSensi = repoDf.zeroRatePointSensitivity(referenceDate).multipliedBy(-pv.Amount / df / df / notional);
		return pvSensi.combinedWith(dfSensi);
	  }

	  /// <summary>
	  /// Calculates the dirty price sensitivity of the fixed coupon bond with z-spread.
	  /// <para>
	  /// The dirty price sensitivity of the security is the sensitivity of the present value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="refData">  the reference data used to calculate the settlement date </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the dirty price curve sensitivity of the security </returns>
	  public virtual PointSensitivityBuilder dirtyPriceSensitivityWithZspread(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, ReferenceData refData, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		LocalDate settlementDate = bond.SettlementDateOffset.adjust(provider.ValuationDate, refData);
		return dirtyPriceSensitivityWithZspread(bond, provider, zSpread, compoundedRateType, periodsPerYear, settlementDate);
	  }

	  // calculate the dirty price sensitivity
	  internal virtual PointSensitivityBuilder dirtyPriceSensitivityWithZspread(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear, LocalDate referenceDate)
	  {

		RepoCurveDiscountFactors repoDf = repoCurveDf(bond, provider);
		double df = repoDf.discountFactor(referenceDate);
		CurrencyAmount pv = presentValueWithZSpread(bond, provider, zSpread, compoundedRateType, periodsPerYear);
		double notional = bond.Notional;
		PointSensitivityBuilder pvSensi = presentValueSensitivityWithZSpread(bond, provider, zSpread, compoundedRateType, periodsPerYear).multipliedBy(1d / df / notional);
		RepoCurveZeroRateSensitivity dfSensi = repoDf.zeroRatePointSensitivity(referenceDate).multipliedBy(-pv.Amount / df / df / notional);
		return pvSensi.combinedWith(dfSensi);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the accrued interest of the fixed coupon bond with the specified settlement date.
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <returns> the accrued interest of the product  </returns>
	  public virtual double accruedInterest(ResolvedFixedCouponBond bond, LocalDate settlementDate)
	  {
		double notional = bond.Notional;
		return accruedYearFraction(bond, settlementDate) * bond.FixedRate * notional;
	  }

	  /// <summary>
	  /// Calculates the accrued year fraction of the fixed coupon bond with the specified settlement date.
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <returns> the accrued year fraction of the product  </returns>
	  public virtual double accruedYearFraction(ResolvedFixedCouponBond bond, LocalDate settlementDate)
	  {
		if (bond.UnadjustedStartDate.isAfter(settlementDate))
		{
		  return 0d;
		}
		FixedCouponBondPaymentPeriod period = bond.findPeriod(settlementDate).orElseThrow(() => new System.ArgumentException("Date outside range of bond"));
		LocalDate previousAccrualDate = period.UnadjustedStartDate;
		double accruedYearFraction = bond.yearFraction(previousAccrualDate, settlementDate);
		double result = 0d;
		if (settlementDate.isAfter(period.DetachmentDate))
		{
		  result = accruedYearFraction - period.YearFraction;
		}
		else
		{
		  result = accruedYearFraction;
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the dirty price of the fixed coupon bond from yield.
	  /// <para>
	  /// The yield must be fractional.
	  /// The dirty price is computed for <seealso cref="FixedCouponBondYieldConvention"/>, and the result is expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the dirty price of the product  </returns>
	  public virtual double dirtyPriceFromYield(ResolvedFixedCouponBond bond, LocalDate settlementDate, double yield)
	  {
		ImmutableList<FixedCouponBondPaymentPeriod> payments = bond.PeriodicPayments;
		int nCoupon = payments.size() - couponIndex(payments, settlementDate);
		FixedCouponBondYieldConvention yieldConv = bond.YieldConvention;
		if (nCoupon == 1)
		{
		  if (yieldConv.Equals(US_STREET) || yieldConv.Equals(DE_BONDS))
		  {
			FixedCouponBondPaymentPeriod payment = payments.get(payments.size() - 1);
			return (1d + payment.FixedRate * payment.YearFraction) / (1d + factorToNextCoupon(bond, settlementDate) * yield / ((double) bond.Frequency.eventsPerYear()));
		  }
		}
		if ((yieldConv.Equals(US_STREET)) || (yieldConv.Equals(GB_BUMP_DMO)) || (yieldConv.Equals(DE_BONDS)))
		{
		  return dirtyPriceFromYieldStandard(bond, settlementDate, yield);
		}
		if (yieldConv.Equals(JP_SIMPLE))
		{
		  LocalDate maturityDate = bond.UnadjustedEndDate;
		  if (settlementDate.isAfter(maturityDate))
		  {
			return 0d;
		  }
		  double maturity = bond.DayCount.relativeYearFraction(settlementDate, maturityDate);
		  double cleanPrice = (1d + bond.FixedRate * maturity) / (1d + yield * maturity);
		  return dirtyPriceFromCleanPrice(bond, settlementDate, cleanPrice);
		}
		throw new System.NotSupportedException("The convention " + yieldConv.name() + " is not supported.");
	  }

	  private double dirtyPriceFromYieldStandard(ResolvedFixedCouponBond bond, LocalDate settlementDate, double yield)
	  {

		int nbCoupon = bond.PeriodicPayments.size();
		double factorOnPeriod = 1 + yield / ((double) bond.Frequency.eventsPerYear());
		double fixedRate = bond.FixedRate;
		double pvAtFirstCoupon = 0;
		int pow = 0;
		for (int loopcpn = 0; loopcpn < nbCoupon; loopcpn++)
		{
		  FixedCouponBondPaymentPeriod period = bond.PeriodicPayments.get(loopcpn);
		  if ((period.hasExCouponPeriod() && !settlementDate.isAfter(period.DetachmentDate)) || (!period.hasExCouponPeriod() && period.PaymentDate.isAfter(settlementDate)))
		  {
			pvAtFirstCoupon += fixedRate * period.YearFraction / Math.Pow(factorOnPeriod, pow);
			++pow;
		  }
		}
		pvAtFirstCoupon += 1d / Math.Pow(factorOnPeriod, pow - 1);
		return pvAtFirstCoupon * Math.Pow(factorOnPeriod, -factorToNextCoupon(bond, settlementDate));
	  }

	  /// <summary>
	  /// Calculates the yield of the fixed coupon bond product from dirty price.
	  /// <para>
	  /// The dirty price must be fractional.
	  /// If the analytic formula is not available, the yield is computed by solving
	  /// a root-finding problem with <seealso cref="#dirtyPriceFromYield(ResolvedFixedCouponBond, LocalDate, double)"/>.  
	  /// The result is also expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="dirtyPrice">  the dirty price </param>
	  /// <returns> the yield of the product  </returns>
	  public virtual double yieldFromDirtyPrice(ResolvedFixedCouponBond bond, LocalDate settlementDate, double dirtyPrice)
	  {
		if (bond.YieldConvention.Equals(JP_SIMPLE))
		{
		  double cleanPrice = cleanPriceFromDirtyPrice(bond, settlementDate, dirtyPrice);
		  LocalDate maturityDate = bond.UnadjustedEndDate;
		  double maturity = bond.DayCount.relativeYearFraction(settlementDate, maturityDate);
		  return (bond.FixedRate + (1d - cleanPrice) / maturity) / cleanPrice;
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> priceResidual = new java.util.function.Function<double, double>()
		System.Func<double, double> priceResidual = (final double? y) =>
		{
	return dirtyPriceFromYield(bond, settlementDate, y) - dirtyPrice;
		};
		double[] range = ROOT_BRACKETER.getBracketedPoints(priceResidual, 0.00, 0.20);
		double yield = ROOT_FINDER.getRoot(priceResidual, range[0], range[1]).Value;
		return yield;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the modified duration of the fixed coupon bond product from yield.
	  /// <para>
	  /// The modified duration is defined as the minus of the first derivative of dirty price
	  /// with respect to yield, divided by the dirty price.
	  /// </para>
	  /// <para>
	  /// The input yield must be fractional. The dirty price and its derivative are
	  /// computed for <seealso cref="FixedCouponBondYieldConvention"/>, and the result is expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the modified duration of the product  </returns>
	  public virtual double modifiedDurationFromYield(ResolvedFixedCouponBond bond, LocalDate settlementDate, double yield)
	  {
		ImmutableList<FixedCouponBondPaymentPeriod> payments = bond.PeriodicPayments;
		int nCoupon = payments.size() - couponIndex(payments, settlementDate);
		FixedCouponBondYieldConvention yieldConv = bond.YieldConvention;
		if (nCoupon == 1)
		{
		  if (yieldConv.Equals(US_STREET) || yieldConv.Equals(DE_BONDS))
		  {
			double couponPerYear = bond.Frequency.eventsPerYear();
			double factor = factorToNextCoupon(bond, settlementDate);
			return factor / couponPerYear / (1d + factor * yield / couponPerYear);
		  }
		}
		if (yieldConv.Equals(US_STREET) || yieldConv.Equals(GB_BUMP_DMO) || yieldConv.Equals(DE_BONDS))
		{
		  return modifiedDurationFromYieldStandard(bond, settlementDate, yield);
		}
		if (yieldConv.Equals(JP_SIMPLE))
		{
		  LocalDate maturityDate = bond.UnadjustedEndDate;
		  if (settlementDate.isAfter(maturityDate))
		  {
			return 0d;
		  }
		  double maturity = bond.DayCount.relativeYearFraction(settlementDate, maturityDate);
		  double num = 1d + bond.FixedRate * maturity;
		  double den = 1d + yield * maturity;
		  double dirtyPrice = dirtyPriceFromCleanPrice(bond, settlementDate, num / den);
		  return num * maturity / den / den / dirtyPrice;
		}
		throw new System.NotSupportedException("The convention " + yieldConv.name() + " is not supported.");
	  }

	  private double modifiedDurationFromYieldStandard(ResolvedFixedCouponBond bond, LocalDate settlementDate, double yield)
	  {

		int nbCoupon = bond.PeriodicPayments.size();
		double couponPerYear = bond.Frequency.eventsPerYear();
		double factorToNextCoupon = this.factorToNextCoupon(bond, settlementDate);
		double factorOnPeriod = 1 + yield / couponPerYear;
		double nominal = bond.Notional;
		double fixedRate = bond.FixedRate;
		double mdAtFirstCoupon = 0d;
		double pvAtFirstCoupon = 0d;
		int pow = 0;
		for (int loopcpn = 0; loopcpn < nbCoupon; loopcpn++)
		{
		  FixedCouponBondPaymentPeriod period = bond.PeriodicPayments.get(loopcpn);
		  if ((period.hasExCouponPeriod() && !settlementDate.isAfter(period.DetachmentDate)) || (!period.hasExCouponPeriod() && period.PaymentDate.isAfter(settlementDate)))
		  {
			mdAtFirstCoupon += period.YearFraction / Math.Pow(factorOnPeriod, pow + 1) * (pow + factorToNextCoupon) / couponPerYear;
			pvAtFirstCoupon += period.YearFraction / Math.Pow(factorOnPeriod, pow);
			++pow;
		  }
		}
		mdAtFirstCoupon *= fixedRate * nominal;
		pvAtFirstCoupon *= fixedRate * nominal;
		mdAtFirstCoupon += nominal / Math.Pow(factorOnPeriod, pow) * (pow - 1 + factorToNextCoupon) / couponPerYear;
		pvAtFirstCoupon += nominal / Math.Pow(factorOnPeriod, pow - 1);
		double md = mdAtFirstCoupon / pvAtFirstCoupon;
		return md;
	  }

	  /// <summary>
	  /// Calculates the Macaulay duration of the fixed coupon bond product from yield.
	  /// <para>
	  /// Macaulay defined an alternative way of weighting the future cash flows.
	  /// </para>
	  /// <para>
	  /// The input yield must be fractional. The dirty price and its derivative are
	  /// computed for <seealso cref="FixedCouponBondYieldConvention"/>, and the result is expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the modified duration of the product  </returns>
	  public virtual double macaulayDurationFromYield(ResolvedFixedCouponBond bond, LocalDate settlementDate, double yield)
	  {
		ImmutableList<FixedCouponBondPaymentPeriod> payments = bond.PeriodicPayments;
		int nCoupon = payments.size() - couponIndex(payments, settlementDate);
		FixedCouponBondYieldConvention yieldConv = bond.YieldConvention;
		if ((yieldConv.Equals(US_STREET)) && (nCoupon == 1))
		{
		  return factorToNextCoupon(bond, settlementDate) / bond.Frequency.eventsPerYear();
		}
		if ((yieldConv.Equals(US_STREET)) || (yieldConv.Equals(GB_BUMP_DMO)) || (yieldConv.Equals(DE_BONDS)))
		{
		  return modifiedDurationFromYield(bond, settlementDate, yield) * (1d + yield / bond.Frequency.eventsPerYear());
		}
		throw new System.NotSupportedException("The convention " + yieldConv.name() + " is not supported.");
	  }

	  /// <summary>
	  /// Calculates the convexity of the fixed coupon bond product from yield.
	  /// <para>
	  /// The convexity is defined as the second derivative of dirty price with respect
	  /// to yield, divided by the dirty price.
	  /// </para>
	  /// <para>
	  /// The input yield must be fractional. The dirty price and its derivative are
	  /// computed for <seealso cref="FixedCouponBondYieldConvention"/>, and the result is expressed in fraction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bond">  the product </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="yield">  the yield </param>
	  /// <returns> the convexity of the product  </returns>
	  public virtual double convexityFromYield(ResolvedFixedCouponBond bond, LocalDate settlementDate, double yield)
	  {
		ImmutableList<FixedCouponBondPaymentPeriod> payments = bond.PeriodicPayments;
		int nCoupon = payments.size() - couponIndex(payments, settlementDate);
		FixedCouponBondYieldConvention yieldConv = bond.YieldConvention;
		if (nCoupon == 1)
		{
		  if (yieldConv.Equals(US_STREET) || yieldConv.Equals(DE_BONDS))
		  {
			double couponPerYear = bond.Frequency.eventsPerYear();
			double factorToNextCoupon = this.factorToNextCoupon(bond, settlementDate);
			double timeToPay = factorToNextCoupon / couponPerYear;
			double disc = (1d + factorToNextCoupon * yield / couponPerYear);
			return 2d * timeToPay * timeToPay / (disc * disc);
		  }
		}
		if (yieldConv.Equals(US_STREET) || yieldConv.Equals(GB_BUMP_DMO) || yieldConv.Equals(DE_BONDS))
		{
		  return convexityFromYieldStandard(bond, settlementDate, yield);
		}
		if (yieldConv.Equals(JP_SIMPLE))
		{
		  LocalDate maturityDate = bond.UnadjustedEndDate;
		  if (settlementDate.isAfter(maturityDate))
		  {
			return 0d;
		  }
		  double maturity = bond.DayCount.relativeYearFraction(settlementDate, maturityDate);
		  double num = 1d + bond.FixedRate * maturity;
		  double den = 1d + yield * maturity;
		  double dirtyPrice = dirtyPriceFromCleanPrice(bond, settlementDate, num / den);
		  return 2d * num * Math.Pow(maturity, 2) * Math.Pow(den, -3) / dirtyPrice;
		}
		throw new System.NotSupportedException("The convention " + yieldConv.name() + " is not supported.");
	  }

	  // assumes notional and coupon rate are constant across the payments.
	  private double convexityFromYieldStandard(ResolvedFixedCouponBond bond, LocalDate settlementDate, double yield)
	  {

		int nbCoupon = bond.PeriodicPayments.size();
		double couponPerYear = bond.Frequency.eventsPerYear();
		double factorToNextCoupon = this.factorToNextCoupon(bond, settlementDate);
		double factorOnPeriod = 1 + yield / couponPerYear;
		double nominal = bond.Notional;
		double fixedRate = bond.FixedRate;
		double cvAtFirstCoupon = 0;
		double pvAtFirstCoupon = 0;
		int pow = 0;
		for (int loopcpn = 0; loopcpn < nbCoupon; loopcpn++)
		{
		  FixedCouponBondPaymentPeriod period = bond.PeriodicPayments.get(loopcpn);
		  if ((period.hasExCouponPeriod() && !settlementDate.isAfter(period.DetachmentDate)) || (!period.hasExCouponPeriod() && period.PaymentDate.isAfter(settlementDate)))
		  {
			cvAtFirstCoupon += period.YearFraction / Math.Pow(factorOnPeriod, pow + 2) * (pow + factorToNextCoupon) * (pow + factorToNextCoupon + 1);
			pvAtFirstCoupon += period.YearFraction / Math.Pow(factorOnPeriod, pow);
			++pow;
		  }
		}
		cvAtFirstCoupon *= fixedRate * nominal / (couponPerYear * couponPerYear);
		pvAtFirstCoupon *= fixedRate * nominal;
		cvAtFirstCoupon += nominal / Math.Pow(factorOnPeriod, pow + 1) * (pow - 1 + factorToNextCoupon) * (pow + factorToNextCoupon) / (couponPerYear * couponPerYear);
		pvAtFirstCoupon += nominal / Math.Pow(factorOnPeriod, pow - 1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double pv = pvAtFirstCoupon * Math.pow(factorOnPeriod, -factorToNextCoupon);
		double pv = pvAtFirstCoupon * Math.Pow(factorOnPeriod, -factorToNextCoupon);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double cv = cvAtFirstCoupon * Math.pow(factorOnPeriod, -factorToNextCoupon) / pv;
		double cv = cvAtFirstCoupon * Math.Pow(factorOnPeriod, -factorToNextCoupon) / pv;
		return cv;
	  }

	  //-------------------------------------------------------------------------
	  private double factorToNextCoupon(ResolvedFixedCouponBond bond, LocalDate settlementDate)
	  {
		if (bond.PeriodicPayments.get(0).StartDate.isAfter(settlementDate))
		{
		  return 0d;
		}
		int couponIndex = this.couponIndex(bond.PeriodicPayments, settlementDate);
		double factorSpot = accruedYearFraction(bond, settlementDate);
		double factorPeriod = bond.PeriodicPayments.get(couponIndex).YearFraction;
		return (factorPeriod - factorSpot) / factorPeriod;
	  }

	  private int couponIndex(ImmutableList<FixedCouponBondPaymentPeriod> list, LocalDate date)
	  {
		int nbCoupon = list.size();
		int couponIndex = 0;
		for (int loopcpn = 0; loopcpn < nbCoupon; ++loopcpn)
		{
		  if (list.get(loopcpn).EndDate.isAfter(date))
		  {
			couponIndex = loopcpn;
			break;
		  }
		}
		return couponIndex;
	  }

	  //-------------------------------------------------------------------------
	  private CurrencyAmount presentValueCoupon(ResolvedFixedCouponBond bond, IssuerCurveDiscountFactors discountFactors, LocalDate referenceDate)
	  {

		double total = 0d;
		foreach (FixedCouponBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if (period.DetachmentDate.isAfter(referenceDate))
		  {
			total += periodPricer.presentValue(period, discountFactors);
		  }
		}
		return CurrencyAmount.of(bond.Currency, total);
	  }

	  private CurrencyAmount presentValueCouponFromZSpread(ResolvedFixedCouponBond bond, IssuerCurveDiscountFactors discountFactors, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear, LocalDate referenceDate)
	  {

		double total = 0d;
		foreach (FixedCouponBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if (period.DetachmentDate.isAfter(referenceDate))
		  {
			total += periodPricer.presentValueWithSpread(period, discountFactors, zSpread, compoundedRateType, periodsPerYear);
		  }
		}
		return CurrencyAmount.of(bond.Currency, total);
	  }

	  //-------------------------------------------------------------------------
	  private PointSensitivityBuilder presentValueSensitivityCoupon(ResolvedFixedCouponBond bond, IssuerCurveDiscountFactors discountFactors, LocalDate referenceDate)
	  {

		PointSensitivityBuilder builder = PointSensitivityBuilder.none();
		foreach (FixedCouponBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if (period.DetachmentDate.isAfter(referenceDate))
		  {
			builder = builder.combinedWith(periodPricer.presentValueSensitivity(period, discountFactors));
		  }
		}
		return builder;
	  }

	  private PointSensitivityBuilder presentValueSensitivityCouponFromZSpread(ResolvedFixedCouponBond bond, IssuerCurveDiscountFactors discountFactors, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear, LocalDate referenceDate)
	  {

		PointSensitivityBuilder builder = PointSensitivityBuilder.none();
		foreach (FixedCouponBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if (period.DetachmentDate.isAfter(referenceDate))
		  {
			builder = builder.combinedWith(periodPricer.presentValueSensitivityWithSpread(period, discountFactors, zSpread, compoundedRateType, periodsPerYear));
		  }
		}
		return builder;
	  }

	  private PointSensitivityBuilder presentValueSensitivityNominal(ResolvedFixedCouponBond bond, IssuerCurveDiscountFactors discountFactors)
	  {

		Payment nominal = bond.NominalPayment;
		PointSensitivityBuilder pt = nominalPricer.presentValueSensitivity(nominal, discountFactors.DiscountFactors);
		if (pt is ZeroRateSensitivity)
		{
		  return IssuerCurveZeroRateSensitivity.of((ZeroRateSensitivity) pt, discountFactors.LegalEntityGroup);
		}
		return pt; // NoPointSensitivity
	  }

	  private PointSensitivityBuilder presentValueSensitivityNominalFromZSpread(ResolvedFixedCouponBond bond, IssuerCurveDiscountFactors discountFactors, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		Payment nominal = bond.NominalPayment;
		PointSensitivityBuilder pt = nominalPricer.presentValueSensitivityWithSpread(nominal, discountFactors.DiscountFactors, zSpread, compoundedRateType, periodsPerYear);
		if (pt is ZeroRateSensitivity)
		{
		  return IssuerCurveZeroRateSensitivity.of((ZeroRateSensitivity) pt, discountFactors.LegalEntityGroup);
		}
		return pt; // NoPointSensitivity
	  }

	  //-------------------------------------------------------------------------
	  // compute pv of coupon payment(s) s.t. referenceDate1 < coupon <= referenceDate2
	  internal virtual double presentValueCoupon(ResolvedFixedCouponBond bond, IssuerCurveDiscountFactors discountFactors, LocalDate referenceDate1, LocalDate referenceDate2)
	  {

		double pvDiff = 0d;
		foreach (FixedCouponBondPaymentPeriod period in bond.PeriodicPayments)
		{
		  if (period.DetachmentDate.isAfter(referenceDate1) && !period.DetachmentDate.isAfter(referenceDate2))
		  {
			pvDiff += periodPricer.presentValue(period, discountFactors);
		  }
		}
		return pvDiff;
	  }

	  // compute pv of coupon payment(s) s.t. referenceDate1 < coupon <= referenceDate2
	  internal virtual double presentValueCouponWithZSpread(ResolvedFixedCouponBond expanded, IssuerCurveDiscountFactors discountFactors, LocalDate referenceDate1, LocalDate referenceDate2, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		double pvDiff = 0d;
		foreach (FixedCouponBondPaymentPeriod period in expanded.PeriodicPayments)
		{
		  if (period.DetachmentDate.isAfter(referenceDate1) && !period.DetachmentDate.isAfter(referenceDate2))
		  {
			pvDiff += periodPricer.presentValueWithSpread(period, discountFactors, zSpread, compoundedRateType, periodsPerYear);
		  }
		}
		return pvDiff;
	  }

	  //-------------------------------------------------------------------------
	  // extracts the repo curve discount factors for the bond
	  internal static RepoCurveDiscountFactors repoCurveDf(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider)
	  {
		return provider.repoCurveDiscountFactors(bond.SecurityId, bond.LegalEntityId, bond.Currency);
	  }

	  // extracts the issuer curve discount factors for the bond
	  internal static IssuerCurveDiscountFactors issuerCurveDf(ResolvedFixedCouponBond bond, LegalEntityDiscountingProvider provider)
	  {
		return provider.issuerCurveDiscountFactors(bond.LegalEntityId, bond.Currency);
	  }

	}

}