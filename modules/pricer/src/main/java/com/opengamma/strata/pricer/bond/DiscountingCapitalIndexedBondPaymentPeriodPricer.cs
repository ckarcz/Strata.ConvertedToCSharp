/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using CapitalIndexedBondPaymentPeriod = com.opengamma.strata.product.bond.CapitalIndexedBondPaymentPeriod;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Pricer implementation for bond payment periods based on a capital indexed coupon.
	/// <para>
	/// This pricer performs discounting of <seealso cref="CapitalIndexedBondPaymentPeriod"/>.
	/// </para>
	/// </summary>
	public class DiscountingCapitalIndexedBondPaymentPeriodPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingCapitalIndexedBondPaymentPeriodPricer DEFAULT = new DiscountingCapitalIndexedBondPaymentPeriodPricer(RateComputationFn.standard());
	  /// <summary>
	  /// Rate observation.
	  /// </summary>
	  private readonly RateComputationFn<RateComputation> rateComputationFn;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="rateComputationFn">  the rate computation function </param>
	  public DiscountingCapitalIndexedBondPaymentPeriodPricer(RateComputationFn<RateComputation> rateComputationFn)
	  {
		this.rateComputationFn = ArgChecker.notNull(rateComputationFn, "rateComputationFn");
	  }

	  /// <summary>
	  /// Obtains the rate computation function.
	  /// </summary>
	  /// <returns> the rate computation function </returns>
	  public virtual RateComputationFn<RateComputation> RateComputationFn
	  {
		  get
		  {
			return rateComputationFn;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of a single payment period.
	  /// <para>
	  /// This returns the value of the period with discounting.
	  /// If the payment date of the period is in the past, zero is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="issuerDiscountFactors">  the discount factor provider </param>
	  /// <returns> the present value of the period </returns>
	  public virtual double presentValue(CapitalIndexedBondPaymentPeriod period, RatesProvider ratesProvider, IssuerCurveDiscountFactors issuerDiscountFactors)
	  {

		double df = issuerDiscountFactors.discountFactor(period.PaymentDate);
		return df * forecastValue(period, ratesProvider);
	  }

	  /// <summary>
	  /// Calculates the present value of a single payment period with z-spread.
	  /// <para>
	  /// This returns the value of the period with discounting.
	  /// If the payment date of the period is in the past, zero is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="issuerDiscountFactors">  the discount factor provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value of the period </returns>
	  public virtual double presentValueWithZSpread(CapitalIndexedBondPaymentPeriod period, RatesProvider ratesProvider, IssuerCurveDiscountFactors issuerDiscountFactors, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		double df = issuerDiscountFactors.DiscountFactors.discountFactorWithSpread(period.PaymentDate, zSpread, compoundedRateType, periodsPerYear);
		return df * forecastValue(period, ratesProvider);
	  }

	  /// <summary>
	  /// Calculates the forecast value of a single payment period.
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <returns> the forecast value of the period  </returns>
	  public virtual double forecastValue(CapitalIndexedBondPaymentPeriod period, RatesProvider ratesProvider)
	  {
		if (period.PaymentDate.isBefore(ratesProvider.ValuationDate))
		{
		  return 0d;
		}
		double rate = rateComputationFn.rate(period.RateComputation, period.StartDate, period.EndDate, ratesProvider);
		return period.Notional * period.RealCoupon * (rate + 1d);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of a single payment period.
	  /// <para>
	  /// The present value sensitivity of the period is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="issuerDiscountFactors">  the discount factor provider </param>
	  /// <returns> the present value curve sensitivity of the period </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(CapitalIndexedBondPaymentPeriod period, RatesProvider ratesProvider, IssuerCurveDiscountFactors issuerDiscountFactors)
	  {

		if (period.PaymentDate.isBefore(ratesProvider.ValuationDate))
		{
		  return PointSensitivityBuilder.none();
		}
		double rate = rateComputationFn.rate(period.RateComputation, period.StartDate, period.EndDate, ratesProvider);
		PointSensitivityBuilder rateSensi = rateComputationFn.rateSensitivity(period.RateComputation, period.StartDate, period.EndDate, ratesProvider);
		double df = issuerDiscountFactors.discountFactor(period.PaymentDate);
		PointSensitivityBuilder dfSensi = issuerDiscountFactors.zeroRatePointSensitivity(period.PaymentDate);
		double factor = period.Notional * period.RealCoupon;
		return rateSensi.multipliedBy(df * factor).combinedWith(dfSensi.multipliedBy((rate + 1d) * factor));
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of a single payment period with z-spread.
	  /// <para>
	  /// The present value sensitivity of the period is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="issuerDiscountFactors">  the discount factor provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value curve sensitivity of the period </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityWithZSpread(CapitalIndexedBondPaymentPeriod period, RatesProvider ratesProvider, IssuerCurveDiscountFactors issuerDiscountFactors, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		if (period.PaymentDate.isBefore(ratesProvider.ValuationDate))
		{
		  return PointSensitivityBuilder.none();
		}
		double rate = rateComputationFn.rate(period.RateComputation, period.StartDate, period.EndDate, ratesProvider);
		PointSensitivityBuilder rateSensi = rateComputationFn.rateSensitivity(period.RateComputation, period.StartDate, period.EndDate, ratesProvider);
		double df = issuerDiscountFactors.DiscountFactors.discountFactorWithSpread(period.PaymentDate, zSpread, compoundedRateType, periodsPerYear);
		ZeroRateSensitivity zeroSensi = issuerDiscountFactors.DiscountFactors.zeroRatePointSensitivityWithSpread(period.PaymentDate, zSpread, compoundedRateType, periodsPerYear);
		IssuerCurveZeroRateSensitivity dfSensi = IssuerCurveZeroRateSensitivity.of(zeroSensi, issuerDiscountFactors.LegalEntityGroup);
		double factor = period.Notional * period.RealCoupon;
		return rateSensi.multipliedBy(df * factor).combinedWith(dfSensi.multipliedBy((rate + 1d) * factor));
	  }

	  /// <summary>
	  /// Calculates the forecast value sensitivity of a single payment period.
	  /// <para>
	  /// The forecast value sensitivity of the period is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <returns> the forecast value sensitivity of the period  </returns>
	  public virtual PointSensitivityBuilder forecastValueSensitivity(CapitalIndexedBondPaymentPeriod period, RatesProvider ratesProvider)
	  {

		if (period.PaymentDate.isBefore(ratesProvider.ValuationDate))
		{
		  return PointSensitivityBuilder.none();
		}
		PointSensitivityBuilder rateSensi = rateComputationFn.rateSensitivity(period.RateComputation, period.StartDate, period.EndDate, ratesProvider);
		return rateSensi.multipliedBy(period.Notional * period.RealCoupon);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Explains the present value of a single payment period.
	  /// <para>
	  /// This adds information to the <seealso cref="ExplainMapBuilder"/> to aid understanding of the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="issuerDiscountFactors">  the discount factor provider </param>
	  /// <param name="builder">  the builder to populate </param>
	  public virtual void explainPresentValue(CapitalIndexedBondPaymentPeriod period, RatesProvider ratesProvider, IssuerCurveDiscountFactors issuerDiscountFactors, ExplainMapBuilder builder)
	  {

		Currency currency = period.Currency;
		LocalDate paymentDate = period.PaymentDate;
		builder.put(ExplainKey.ENTRY_TYPE, "CapitalIndexedBondPaymentPeriod");
		builder.put(ExplainKey.PAYMENT_DATE, paymentDate);
		builder.put(ExplainKey.PAYMENT_CURRENCY, currency);
		builder.put(ExplainKey.START_DATE, period.StartDate);
		builder.put(ExplainKey.UNADJUSTED_START_DATE, period.UnadjustedStartDate);
		builder.put(ExplainKey.END_DATE, period.EndDate);
		builder.put(ExplainKey.UNADJUSTED_END_DATE, period.UnadjustedEndDate);
		builder.put(ExplainKey.DAYS, (int) DAYS.between(period.UnadjustedStartDate, period.UnadjustedEndDate));
		if (paymentDate.isBefore(ratesProvider.ValuationDate))
		{
		  builder.put(ExplainKey.COMPLETED, true);
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.zero(currency));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.zero(currency));
		}
		else
		{
		  builder.put(ExplainKey.DISCOUNT_FACTOR, issuerDiscountFactors.discountFactor(paymentDate));
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.of(currency, forecastValue(period, ratesProvider)));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.of(currency, presentValue(period, ratesProvider, issuerDiscountFactors)));
		}
	  }

	  /// <summary>
	  /// Explains the present value of a single payment period with z-spread.
	  /// <para>
	  /// This adds information to the <seealso cref="ExplainMapBuilder"/> to aid understanding of the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to price </param>
	  /// <param name="ratesProvider">  the rates provider, used to determine price index values </param>
	  /// <param name="issuerDiscountFactors">  the discount factor provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <param name="builder">  the builder to populate </param>
	  public virtual void explainPresentValueWithZSpread(CapitalIndexedBondPaymentPeriod period, RatesProvider ratesProvider, IssuerCurveDiscountFactors issuerDiscountFactors, ExplainMapBuilder builder, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		Currency currency = period.Currency;
		LocalDate paymentDate = period.PaymentDate;
		builder.put(ExplainKey.ENTRY_TYPE, "CapitalIndexedBondPaymentPeriod");
		builder.put(ExplainKey.PAYMENT_DATE, paymentDate);
		builder.put(ExplainKey.PAYMENT_CURRENCY, currency);
		builder.put(ExplainKey.START_DATE, period.StartDate);
		builder.put(ExplainKey.UNADJUSTED_START_DATE, period.UnadjustedStartDate);
		builder.put(ExplainKey.END_DATE, period.EndDate);
		builder.put(ExplainKey.UNADJUSTED_END_DATE, period.UnadjustedEndDate);
		builder.put(ExplainKey.DAYS, (int) DAYS.between(period.UnadjustedStartDate, period.UnadjustedEndDate));
		if (paymentDate.isBefore(ratesProvider.ValuationDate))
		{
		  builder.put(ExplainKey.COMPLETED, true);
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.zero(currency));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.zero(currency));
		}
		else
		{
		  builder.put(ExplainKey.DISCOUNT_FACTOR, issuerDiscountFactors.discountFactor(paymentDate));
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.of(currency, forecastValue(period, ratesProvider)));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.of(currency, presentValueWithZSpread(period, ratesProvider, issuerDiscountFactors, zSpread, compoundedRateType, periodsPerYear)));
		}
	  }

	}

}