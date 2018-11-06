/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swap
{
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DispatchingSwapPaymentPeriodPricer = com.opengamma.strata.pricer.impl.swap.DispatchingSwapPaymentPeriodPricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;

	/// <summary>
	/// Pricer for payment periods.
	/// <para>
	/// This function provides the ability to price a <seealso cref="SwapPaymentPeriod"/>.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe functions.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of period </param>
	public interface SwapPaymentPeriodPricer<T> where T : com.opengamma.strata.product.swap.SwapPaymentPeriod
	{

	  /// <summary>
	  /// Returns the standard instance of the function.
	  /// <para>
	  /// Use this method to avoid a direct dependency on the implementation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the payment period pricer </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static SwapPaymentPeriodPricer<com.opengamma.strata.product.swap.SwapPaymentPeriod> standard()
	//  {
	//	return DispatchingSwapPaymentPeriodPricer.DEFAULT;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of a single payment period.
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
	  /// <param name="period">  the period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the period </returns>
	  double presentValue(T period, RatesProvider provider);

	  /// <summary>
	  /// Calculates the present value sensitivity of a single payment period.
	  /// <para>
	  /// The present value sensitivity of the period is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value curve sensitivity of the period </returns>
	  PointSensitivityBuilder presentValueSensitivity(T period, RatesProvider provider);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forecast value of a single payment period.
	  /// <para>
	  /// The amount is expressed in the currency of the period.
	  /// This returns the value of the period without discounting.
	  /// </para>
	  /// <para>
	  /// The payment date of the period should not be in the past.
	  /// The result of this method for payment dates in the past is undefined.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value of the period </returns>
	  double forecastValue(T period, RatesProvider provider);

	  /// <summary>
	  /// Calculates the forecast value sensitivity of a single payment period.
	  /// <para>
	  /// The forecast value sensitivity of the period is the sensitivity of the forecast value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value curve sensitivity of the period </returns>
	  PointSensitivityBuilder forecastValueSensitivity(T period, RatesProvider provider);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of a basis point of a period.
	  /// <para>
	  /// This calculate the amount by which, to the first order, the period present value
	  /// changes for a change of the rate defining the payment period. For known amount
	  /// payments for which there is rate, the value is 0. In absence of compounding on
	  /// the period, this measure is equivalent to the traditional PVBP.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of a basis point </returns>
	  double pvbp(T period, RatesProvider provider);

	  /// <summary>
	  /// Calculates the present value of a basis point sensitivity of a single payment period.
	  /// <para>
	  /// This calculate the sensitivity of the present value of a basis point (pvbp) quantity
	  /// to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of a basis point sensitivity </returns>
	  PointSensitivityBuilder pvbpSensitivity(T period, RatesProvider provider);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the accrued interest since the last payment.
	  /// <para>
	  /// This calculates the interest that has accrued between the start of the period
	  /// and the valuation date. Discounting is not applied.
	  /// The amount is expressed in the currency of the period.
	  /// It is intended that this method is called only with the period where the
	  /// valuation date is after the start date and before or equal to the end date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the accrued interest of the period </returns>
	  double accruedInterest(T period, RatesProvider provider);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Explains the present value of a single payment period.
	  /// <para>
	  /// This adds information to the <seealso cref="ExplainMapBuilder"/> to aid understanding of the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="builder">  the builder to populate </param>
	  void explainPresentValue(T period, RatesProvider provider, ExplainMapBuilder builder);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of a single payment period.
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the currency exposure </returns>
	  MultiCurrencyAmount currencyExposure(T period, RatesProvider provider);

	  /// <summary>
	  /// Calculates the current cash of a single payment period.
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the current cash </returns>
	  double currentCash(T period, RatesProvider provider);
	}

}