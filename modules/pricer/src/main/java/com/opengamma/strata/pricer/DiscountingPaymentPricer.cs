/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using CashFlow = com.opengamma.strata.market.amount.CashFlow;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Pricer for simple payments.
	/// <para>
	/// This function provides the ability to price an <seealso cref="Payment"/>.
	/// </para>
	/// </summary>
	public class DiscountingPaymentPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingPaymentPricer DEFAULT = new DiscountingPaymentPricer();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public DiscountingPaymentPricer()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value of the payment by discounting.
	  /// <para>
	  /// The present value is zero if the payment date is before the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(Payment payment, BaseProvider provider)
	  {
		// duplicated code to avoid looking up in the provider when not necessary
		if (provider.ValuationDate.isAfter(payment.Date))
		{
		  return CurrencyAmount.zero(payment.Currency);
		}
		double df = provider.discountFactor(payment.Currency, payment.Date);
		return payment.Value.multipliedBy(df);
	  }

	  /// <summary>
	  /// Computes the present value of the payment by discounting.
	  /// <para>
	  /// The present value is zero if the payment date is before the valuation date.
	  /// </para>
	  /// <para>
	  /// The specified discount factors should be for the payment currency, however this is not validated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="discountFactors">  the discount factors to price against </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(Payment payment, DiscountFactors discountFactors)
	  {
		if (discountFactors.ValuationDate.isAfter(payment.Date))
		{
		  return CurrencyAmount.zero(payment.Currency);
		}
		return payment.Value.multipliedBy(discountFactors.discountFactor(payment.Date));
	  }

	  /// <summary>
	  /// Computes the present value of the payment by discounting.
	  /// <para>
	  /// The present value is zero if the payment date is before the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the present value </returns>
	  public virtual double presentValueAmount(Payment payment, BaseProvider provider)
	  {
		// duplicated code to avoid looking up in the provider when not necessary
		if (provider.ValuationDate.isAfter(payment.Date))
		{
		  return 0d;
		}
		double df = provider.discountFactor(payment.Currency, payment.Date);
		return payment.Amount * df;
	  }

	  /// <summary>
	  /// Computes the present value of the payment with z-spread by discounting.
	  /// <para>
	  /// The present value is zero if the payment date is before the valuation date.
	  /// </para>
	  /// <para>
	  /// The specified discount factors should be for the payment currency, however this is not validated.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="discountFactors">  the discount factors to price against </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValueWithSpread(Payment payment, DiscountFactors discountFactors, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		if (discountFactors.ValuationDate.isAfter(payment.Date))
		{
		  return CurrencyAmount.zero(payment.Currency);
		}
		double df = discountFactors.discountFactorWithSpread(payment.Date, zSpread, compoundedRateType, periodsPerYear);
		return payment.Value.multipliedBy(df);
	  }

	  /// <summary>
	  /// Explains the present value of the payment.
	  /// <para>
	  /// This returns explanatory information about the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the explanatory information </returns>
	  public virtual ExplainMap explainPresentValue(Payment payment, BaseProvider provider)
	  {
		Currency currency = payment.Currency;
		LocalDate paymentDate = payment.Date;

		ExplainMapBuilder builder = ExplainMap.builder();
		builder.put(ExplainKey.ENTRY_TYPE, "Payment");
		builder.put(ExplainKey.PAYMENT_DATE, paymentDate);
		builder.put(ExplainKey.PAYMENT_CURRENCY, currency);
		if (paymentDate.isBefore(provider.ValuationDate))
		{
		  builder.put(ExplainKey.COMPLETED, true);
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.zero(currency));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.zero(currency));
		}
		else
		{
		  builder.put(ExplainKey.DISCOUNT_FACTOR, provider.discountFactor(currency, paymentDate));
		  builder.put(ExplainKey.FORECAST_VALUE, forecastValue(payment, provider));
		  builder.put(ExplainKey.PRESENT_VALUE, presentValue(payment, provider));
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compute the present value curve sensitivity of the payment.
	  /// <para>
	  /// The present value sensitivity of the payment is the sensitivity of the
	  /// present value to the discount factor curve.
	  /// There is no sensitivity if the payment date is before the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(Payment payment, BaseProvider provider)
	  {
		// duplicated code to avoid looking up in the provider when not necessary
		if (provider.ValuationDate.isAfter(payment.Date))
		{
		  return PointSensitivityBuilder.none();
		}
		DiscountFactors discountFactors = provider.discountFactors(payment.Currency);
		return discountFactors.zeroRatePointSensitivity(payment.Date).multipliedBy(payment.Amount);
	  }

	  /// <summary>
	  /// Compute the present value curve sensitivity of the payment.
	  /// <para>
	  /// The present value sensitivity of the payment is the sensitivity of the
	  /// present value to the discount factor curve.
	  /// There is no sensitivity if the payment date is before the valuation date.
	  /// </para>
	  /// <para>
	  /// The specified discount factors should be for the payment currency, however this is not validated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="discountFactors">  the discount factors to price against </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(Payment payment, DiscountFactors discountFactors)
	  {
		if (discountFactors.ValuationDate.isAfter(payment.Date))
		{
		  return PointSensitivityBuilder.none();
		}
		return discountFactors.zeroRatePointSensitivity(payment.Date).multipliedBy(payment.Amount);
	  }

	  /// <summary>
	  /// Compute the present value curve sensitivity of the payment with z-spread.
	  /// <para>
	  /// The present value sensitivity of the payment is the sensitivity of the
	  /// present value to the discount factor curve.
	  /// There is no sensitivity if the payment date is before the valuation date.
	  /// </para>
	  /// <para>
	  /// The specified discount factors should be for the payment currency, however this is not validated.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="discountFactors">  the discount factors to price against </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityWithSpread(Payment payment, DiscountFactors discountFactors, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		if (discountFactors.ValuationDate.isAfter(payment.Date))
		{
		  return PointSensitivityBuilder.none();
		}
		ZeroRateSensitivity sensi = discountFactors.zeroRatePointSensitivityWithSpread(payment.Date, zSpread, compoundedRateType, periodsPerYear);
		return sensi.multipliedBy(payment.Amount);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the forecast value of the payment.
	  /// <para>
	  /// The present value is zero if the payment date is before the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the forecast value </returns>
	  public virtual CurrencyAmount forecastValue(Payment payment, BaseProvider provider)
	  {
		if (provider.ValuationDate.isAfter(payment.Date))
		{
		  return CurrencyAmount.zero(payment.Currency);
		}
		return payment.Value;
	  }

	  /// <summary>
	  /// Computes the forecast value of the payment.
	  /// <para>
	  /// The present value is zero if the payment date is before the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the forecast value </returns>
	  public virtual double forecastValueAmount(Payment payment, BaseProvider provider)
	  {
		if (provider.ValuationDate.isAfter(payment.Date))
		{
		  return 0d;
		}
		return payment.Amount;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the future cash flow of the payment.
	  /// <para>
	  /// The cash flow is returned, empty if the payment has already occurred.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the cash flow, empty if the payment has occurred </returns>
	  public virtual CashFlows cashFlows(Payment payment, BaseProvider provider)
	  {
		if (provider.ValuationDate.isAfter(payment.Date))
		{
		  return CashFlows.NONE;
		}
		double df = provider.discountFactor(payment.Currency, payment.Date);
		CashFlow flow = CashFlow.ofForecastValue(payment.Date, payment.Currency, payment.Amount, df);
		return CashFlows.of(flow);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure.
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(Payment payment, BaseProvider provider)
	  {
		return MultiCurrencyAmount.of(presentValue(payment, provider));
	  }

	  /// <summary>
	  /// Calculates the current cash.
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(Payment payment, BaseProvider provider)
	  {
		if (payment.Date.isEqual(provider.ValuationDate))
		{
		  return payment.Value;
		}
		return CurrencyAmount.zero(payment.Currency);
	  }

	}

}