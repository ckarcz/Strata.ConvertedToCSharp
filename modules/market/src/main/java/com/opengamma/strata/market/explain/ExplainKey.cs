using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.explain
{

	using FromString = org.joda.convert.FromString;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Index = com.opengamma.strata.basics.index.Index;
	using TypedString = com.opengamma.strata.collect.TypedString;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using IborInterpolatedRateComputation = com.opengamma.strata.product.rate.IborInterpolatedRateComputation;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;

	/// <summary>
	/// A key for the map of explanatory values.
	/// <para>
	/// This key is used with <seealso cref="ExplainMap"/> to create a loosely defined data structure
	/// that allows an explanation of a calculation to be represented.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the object associated with the key </param>
	[Serializable]
	public sealed class ExplainKey<T> : TypedString<ExplainKey<T>>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The index of this entry within the parent.
	  /// For example, this could be used to represent the index of the leg within the swap,
	  /// or the index of the payment period within the leg.
	  /// </summary>
	  public static readonly ExplainKey<int> ENTRY_INDEX = of("EntryIndex");
	  /// <summary>
	  /// The type of this entry.
	  /// For example, this could be used to distinguish between a swap leg, swap payment period and a FRA.
	  /// </summary>
	  public static readonly ExplainKey<string> ENTRY_TYPE = of("EntryType");

	  /// <summary>
	  /// The list of legs.
	  /// </summary>
	  public static readonly ExplainKey<IList<ExplainMap>> LEGS = of("Legs");
	  /// <summary>
	  /// The list of payment events.
	  /// </summary>
	  public static readonly ExplainKey<IList<ExplainMap>> PAYMENT_EVENTS = of("PaymentEvents");
	  /// <summary>
	  /// The list of payment periods.
	  /// </summary>
	  public static readonly ExplainKey<IList<ExplainMap>> PAYMENT_PERIODS = of("PaymentPeriods");
	  /// <summary>
	  /// The list of accrual periods.
	  /// </summary>
	  public static readonly ExplainKey<IList<ExplainMap>> ACCRUAL_PERIODS = of("AccrualPeriods");
	  /// <summary>
	  /// The list of reset periods.
	  /// </summary>
	  public static readonly ExplainKey<IList<ExplainMap>> RESET_PERIODS = of("ResetPeriods");
	  /// <summary>
	  /// The list of rate observations.
	  /// </summary>
	  public static readonly ExplainKey<IList<ExplainMap>> OBSERVATIONS = of("Observations");

	  /// <summary>
	  /// The present value.
	  /// </summary>
	  public static readonly ExplainKey<CurrencyAmount> PRESENT_VALUE = of("PresentValue");
	  /// <summary>
	  /// The forecast value.
	  /// </summary>
	  public static readonly ExplainKey<CurrencyAmount> FORECAST_VALUE = of("ForecastValue");
	  /// <summary>
	  /// The flag to indicate that the period has completed.
	  /// For example, a swap payment period that has already paid would have this set to true.
	  /// This will generally never be set to false.
	  /// </summary>
	  public static readonly ExplainKey<bool> COMPLETED = of("Completed");

	  /// <summary>
	  /// The currency of the payment.
	  /// </summary>
	  public static readonly ExplainKey<Currency> PAYMENT_CURRENCY = of("PaymentCurrency");
	  /// <summary>
	  /// Whether the entry is being paid or received.
	  /// </summary>
	  public static readonly ExplainKey<PayReceive> PAY_RECEIVE = of("PayReceive");
	  /// <summary>
	  /// An indication of the pay-off formula that applies to the leg.
	  /// For example, this could be used to distinguish between fixed, overnight, IBOR and inflation.
	  /// </summary>
	  public static readonly ExplainKey<string> LEG_TYPE = of("LegType");
	  /// <summary>
	  /// The effective notional, which may be converted from the contract notional in the case of FX reset.
	  /// </summary>
	  public static readonly ExplainKey<CurrencyAmount> NOTIONAL = of("Notional");
	  /// <summary>
	  /// The notional, as defined in the trade.
	  /// This is the notional in the trade, which may be converted to the actual notional by FX reset.
	  /// </summary>
	  public static readonly ExplainKey<CurrencyAmount> TRADE_NOTIONAL = of("TradeNotional");

	  /// <summary>
	  /// The payment date, adjusted to be a valid business day if necessary.
	  /// </summary>
	  public static readonly ExplainKey<LocalDate> PAYMENT_DATE = of("PaymentDate");
	  /// <summary>
	  /// The payment date, before any business day adjustment.
	  /// </summary>
	  public static readonly ExplainKey<LocalDate> UNADJUSTED_PAYMENT_DATE = of("PaymentDate");

	  /// <summary>
	  /// The accrual start date, adjusted to be a valid business day if necessary.
	  /// </summary>
	  public static readonly ExplainKey<LocalDate> START_DATE = of("StartDate");
	  /// <summary>
	  /// The accrual start date, before any business day adjustment.
	  /// </summary>
	  public static readonly ExplainKey<LocalDate> UNADJUSTED_START_DATE = of("UnadjustedStartDate");
	  /// <summary>
	  /// The accrual end date, adjusted to be a valid business day if necessary.
	  /// </summary>
	  public static readonly ExplainKey<LocalDate> END_DATE = of("EndDate");
	  /// <summary>
	  /// The accrual end date, before any business day adjustment.
	  /// </summary>
	  public static readonly ExplainKey<LocalDate> UNADJUSTED_END_DATE = of("UnadjustedEndDate");

	  /// <summary>
	  /// The day count used to calculate the year fraction.
	  /// </summary>
	  public static readonly ExplainKey<DayCount> ACCRUAL_DAY_COUNT = of("AccrualDayCount");
	  /// <summary>
	  /// The year fraction between the start and end dates.
	  /// </summary>
	  public static readonly ExplainKey<double> ACCRUAL_YEAR_FRACTION = of("AccrualYearFraction");
	  /// <summary>
	  /// The number of accrual days between the start and end dates.
	  /// </summary>
	  public static readonly ExplainKey<int> ACCRUAL_DAYS = of("AccrualDays");
	  /// <summary>
	  /// The actual number of days between the start and end dates.
	  /// </summary>
	  public static readonly ExplainKey<int> DAYS = of("Days");

	  /// <summary>
	  /// The discount factor, typically derived from a curve.
	  /// </summary>
	  public static readonly ExplainKey<double> DISCOUNT_FACTOR = of("DiscountFactor");
	  /// <summary>
	  /// The fixed rate, as defined in the contract.
	  /// </summary>
	  public static readonly ExplainKey<double> FIXED_RATE = of("FixedRate");

	  /// <summary>
	  /// The observed index, such as an Ibor or Overnight index.
	  /// </summary>
	  public static readonly ExplainKey<Index> INDEX = of("Index");
	  /// <summary>
	  /// The start date used to calculate the forward rate.
	  /// </summary>
	  public static readonly ExplainKey<LocalDate> FORWARD_RATE_START_DATE = of("ForwardRateStartDate");
	  /// <summary>
	  /// The end date used to calculate the forward rate.
	  /// </summary>
	  public static readonly ExplainKey<LocalDate> FORWARD_RATE_END_DATE = of("ForwardRateEndDate");
	  /// <summary>
	  /// The fixing date.
	  /// </summary>
	  public static readonly ExplainKey<LocalDate> FIXING_DATE = of("FixingDate");
	  /// <summary>
	  /// The observed index value, typically derived from a curve.
	  /// This may be known exactly if the fixing has occurred.
	  /// </summary>
	  public static readonly ExplainKey<double> INDEX_VALUE = of("IndexValue");
	  /// <summary>
	  /// The flag to indicate that the that the observed value is from a fixing time-series.
	  /// This will generally never be set to false.
	  /// </summary>
	  public static readonly ExplainKey<bool> FROM_FIXING_SERIES = of("FromFixingSeries");
	  /// <summary>
	  /// The weight of this observation.
	  /// Weighting applies when averaging more than one observation to produce the final rate.
	  /// </summary>
	  public static readonly ExplainKey<double> WEIGHT = of("Weight");
	  /// <summary>
	  /// The combined rate, including weighting.
	  /// This rate differs from the observed rate if there is more than one fixing involved.
	  /// For example, <seealso cref="IborInterpolatedRateComputation"/> has two observed rates
	  /// which are combined to create this rate.
	  /// </summary>
	  public static readonly ExplainKey<double> COMBINED_RATE = of("CombinedRate");
	  /// <summary>
	  /// The spread, added to the forward rate.
	  /// </summary>
	  public static readonly ExplainKey<double> SPREAD = of("Spread");
	  /// <summary>
	  /// The gearing, that the rate is multiplied by.
	  /// </summary>
	  public static readonly ExplainKey<double> GEARING = of("Gearing");
	  /// <summary>
	  /// The pay-off rate, which includes adjustments like weighting, spread and gearing.
	  /// </summary>
	  public static readonly ExplainKey<double> PAY_OFF_RATE = of("PayOffRate");
	  /// <summary>
	  /// The unit amount.
	  /// This is typically the rate multiplied by the year fraction, before multiplication by the notional.
	  /// </summary>
	  public static readonly ExplainKey<double> UNIT_AMOUNT = of("UnitAmount");
	  /// <summary>
	  /// The method of compounding.
	  /// </summary>
	  public static readonly ExplainKey<CompoundingMethod> COMPOUNDING = of("CompoundingMethod");
	  /// <summary>
	  /// The strike value.
	  /// </summary>
	  public static readonly ExplainKey<double> STRIKE_VALUE = of("StrikeValue");
	  /// <summary>
	  /// The convexity adjusted rate.
	  /// </summary>
	  public static readonly ExplainKey<double> CONVEXITY_ADJUSTED_RATE = of("ConvexityAdjustedRate");
	  /// <summary>
	  /// The forward rate.
	  /// </summary>
	  public static readonly ExplainKey<double> FORWARD_RATE = of("ForwardRate");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Field names may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the inferred type of the key </param>
	  /// <param name="name">  the name of the field </param>
	  /// <returns> a field with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static <R> ExplainKey<R> of(String name)
	  public static ExplainKey<R> of<R>(string name)
	  {
		return new ExplainKey<R>(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name of the field </param>
	  private ExplainKey(string name) : base(name)
	  {
	  }

	}

}