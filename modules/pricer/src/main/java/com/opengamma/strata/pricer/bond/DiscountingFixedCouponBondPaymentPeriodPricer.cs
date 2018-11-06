/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using FixedCouponBondPaymentPeriod = com.opengamma.strata.product.bond.FixedCouponBondPaymentPeriod;

	/// <summary>
	/// Pricer implementation for bond payment periods based on a fixed coupon.
	/// <para>
	/// This pricer performs discounting of the fixed coupon payment.
	/// </para>
	/// </summary>
	public class DiscountingFixedCouponBondPaymentPeriodPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFixedCouponBondPaymentPeriodPricer DEFAULT = new DiscountingFixedCouponBondPaymentPeriodPricer();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public DiscountingFixedCouponBondPaymentPeriodPricer()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of a single fixed coupon payment period.
	  /// <para>
	  /// The amount is expressed in the currency of the period.
	  /// This returns the value of the period with discounting.
	  /// </para>
	  /// <para>
	  /// The payment date of the period should not be in the past.
	  /// The result of this method for payment dates in the past is undefined.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="discountFactors">  the discount factor provider </param>
	  /// <returns> the present value of the period </returns>
	  public virtual double presentValue(FixedCouponBondPaymentPeriod period, IssuerCurveDiscountFactors discountFactors)
	  {

		if (period.PaymentDate.isBefore(discountFactors.ValuationDate))
		{
		  return 0d;
		}
		double df = discountFactors.discountFactor(period.PaymentDate);
		return period.FixedRate * period.Notional * period.YearFraction * df;
	  }

	  /// <summary>
	  /// Calculates the present value of a single fixed coupon payment period with z-spread.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// </para>
	  /// <para>
	  /// The amount is expressed in the currency of the period.
	  /// This returns the value of the period with discounting.
	  /// </para>
	  /// <para>
	  /// The payment date of the period should not be in the past.
	  /// The result of this method for payment dates in the past is undefined.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="discountFactors">  the discount factor provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value of the period </returns>
	  public virtual double presentValueWithSpread(FixedCouponBondPaymentPeriod period, IssuerCurveDiscountFactors discountFactors, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		if (period.PaymentDate.isBefore(discountFactors.ValuationDate))
		{
		  return 0d;
		}
		double df = discountFactors.DiscountFactors.discountFactorWithSpread(period.PaymentDate, zSpread, compoundedRateType, periodsPerYear);
		return period.FixedRate * period.Notional * period.YearFraction * df;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forecast value of a single fixed coupon payment period.
	  /// <para>
	  /// The amount is expressed in the currency of the period.
	  /// This returns the value of the period with discounting.
	  /// </para>
	  /// <para>
	  /// The payment date of the period should not be in the past.
	  /// The result of this method for payment dates in the past is undefined.
	  /// </para>
	  /// <para>
	  /// The forecast value is z-spread independent.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="discountFactors">  the discount factor provider </param>
	  /// <returns> the present value of the period </returns>
	  public virtual double forecastValue(FixedCouponBondPaymentPeriod period, IssuerCurveDiscountFactors discountFactors)
	  {

		if (period.PaymentDate.isBefore(discountFactors.ValuationDate))
		{
		  return 0d;
		}
		return period.FixedRate * period.Notional * period.YearFraction;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of a single fixed coupon payment period.
	  /// <para>
	  /// The present value sensitivity of the period is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="discountFactors">  the discount factor provider </param>
	  /// <returns> the present value curve sensitivity of the period </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(FixedCouponBondPaymentPeriod period, IssuerCurveDiscountFactors discountFactors)
	  {

		if (period.PaymentDate.isBefore(discountFactors.ValuationDate))
		{
		  return PointSensitivityBuilder.none();
		}
		IssuerCurveZeroRateSensitivity dscSensi = discountFactors.zeroRatePointSensitivity(period.PaymentDate);
		return dscSensi.multipliedBy(period.FixedRate * period.Notional * period.YearFraction);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of a single fixed coupon payment period with z-spread.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// </para>
	  /// <para>
	  /// The present value sensitivity of the period is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="discountFactors">  the discount factor provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value curve sensitivity of the period </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityWithSpread(FixedCouponBondPaymentPeriod period, IssuerCurveDiscountFactors discountFactors, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		if (period.PaymentDate.isBefore(discountFactors.ValuationDate))
		{
		  return PointSensitivityBuilder.none();
		}
		ZeroRateSensitivity zeroSensi = discountFactors.DiscountFactors.zeroRatePointSensitivityWithSpread(period.PaymentDate, zSpread, compoundedRateType, periodsPerYear);
		IssuerCurveZeroRateSensitivity dscSensi = IssuerCurveZeroRateSensitivity.of(zeroSensi, discountFactors.LegalEntityGroup);
		return dscSensi.multipliedBy(period.FixedRate * period.Notional * period.YearFraction);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forecast value sensitivity of a single fixed coupon payment period.
	  /// <para>
	  /// The forecast value sensitivity of the period is the sensitivity of the forecast value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The forecast value sensitivity is zero and z-spread independent for the fixed payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="discountFactors">  the discount factor provider </param>
	  /// <returns> the forecast value curve sensitivity of the period </returns>
	  public virtual PointSensitivityBuilder forecastValueSensitivity(FixedCouponBondPaymentPeriod period, IssuerCurveDiscountFactors discountFactors)
	  {

		return PointSensitivityBuilder.none();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Explains the present value of a single fixed coupon payment period.
	  /// <para>
	  /// This adds information to the <seealso cref="ExplainMapBuilder"/> to aid understanding of the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="discountFactors">  the discount factor provider </param>
	  /// <param name="builder">  the builder to populate </param>
	  public virtual void explainPresentValue(FixedCouponBondPaymentPeriod period, IssuerCurveDiscountFactors discountFactors, ExplainMapBuilder builder)
	  {

		Currency currency = period.Currency;
		LocalDate paymentDate = period.PaymentDate;
		explainBasics(period, builder, currency, paymentDate);
		if (paymentDate.isBefore(discountFactors.ValuationDate))
		{
		  builder.put(ExplainKey.COMPLETED, true);
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.zero(currency));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.zero(currency));
		}
		else
		{
		  builder.put(ExplainKey.DISCOUNT_FACTOR, discountFactors.discountFactor(paymentDate));
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.of(currency, forecastValue(period, discountFactors)));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.of(currency, presentValue(period, discountFactors)));
		}
	  }

	  /// <summary>
	  /// Explains the present value of a single fixed coupon payment period with z-spread.
	  /// <para>
	  /// This adds information to the <seealso cref="ExplainMapBuilder"/> to aid understanding of the calculation.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="discountFactors">  the discount factor provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <param name="builder">  the builder to populate </param>
	  public virtual void explainPresentValueWithSpread(FixedCouponBondPaymentPeriod period, IssuerCurveDiscountFactors discountFactors, ExplainMapBuilder builder, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		Currency currency = period.Currency;
		LocalDate paymentDate = period.PaymentDate;
		explainBasics(period, builder, currency, paymentDate);
		if (paymentDate.isBefore(discountFactors.ValuationDate))
		{
		  builder.put(ExplainKey.COMPLETED, true);
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.zero(currency));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.zero(currency));
		}
		else
		{
		  builder.put(ExplainKey.DISCOUNT_FACTOR, discountFactors.DiscountFactors.discountFactorWithSpread(paymentDate, zSpread, compoundedRateType, periodsPerYear));
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.of(currency, forecastValue(period, discountFactors)));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.of(currency, presentValueWithSpread(period, discountFactors, zSpread, compoundedRateType, periodsPerYear)));
		}
	  }

	  // common parts of explain
	  private void explainBasics(FixedCouponBondPaymentPeriod period, ExplainMapBuilder builder, Currency currency, LocalDate paymentDate)
	  {
		builder.put(ExplainKey.ENTRY_TYPE, "FixedCouponBondPaymentPeriod");
		builder.put(ExplainKey.PAYMENT_DATE, paymentDate);
		builder.put(ExplainKey.PAYMENT_CURRENCY, currency);
		builder.put(ExplainKey.START_DATE, period.StartDate);
		builder.put(ExplainKey.UNADJUSTED_START_DATE, period.UnadjustedStartDate);
		builder.put(ExplainKey.END_DATE, period.EndDate);
		builder.put(ExplainKey.UNADJUSTED_END_DATE, period.UnadjustedEndDate);
		builder.put(ExplainKey.ACCRUAL_YEAR_FRACTION, period.YearFraction);
		builder.put(ExplainKey.DAYS, (int) DAYS.between(period.StartDate, period.EndDate));
	  }

	}

}