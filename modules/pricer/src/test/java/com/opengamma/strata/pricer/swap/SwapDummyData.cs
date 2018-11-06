/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_RPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.PriceIndexCalculationMethod.MONTHLY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.FIXED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.IBOR;


	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using FxIndices = com.opengamma.strata.basics.index.FxIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using CompoundingMethod = com.opengamma.strata.product.swap.CompoundingMethod;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using FxReset = com.opengamma.strata.product.swap.FxReset;
	using FxResetNotionalExchange = com.opengamma.strata.product.swap.FxResetNotionalExchange;
	using InflationRateCalculation = com.opengamma.strata.product.swap.InflationRateCalculation;
	using KnownAmountSwapLeg = com.opengamma.strata.product.swap.KnownAmountSwapLeg;
	using NotionalExchange = com.opengamma.strata.product.swap.NotionalExchange;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateAccrualPeriod = com.opengamma.strata.product.swap.RateAccrualPeriod;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using FixedOvernightSwapConventions = com.opengamma.strata.product.swap.type.FixedOvernightSwapConventions;

	/// <summary>
	/// Basic dummy objects used when the data within is not important.
	/// </summary>
	public sealed class SwapDummyData
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  /// <summary>
	  /// The notional.
	  /// </summary>
	  public const double NOTIONAL = 1_000_000d;
	  /// <summary>
	  /// NotionalExchange (receive - GBP).
	  /// </summary>
	  public static readonly NotionalExchange NOTIONAL_EXCHANGE_REC_GBP = NotionalExchange.of(CurrencyAmount.of(Currency.GBP, NOTIONAL), date(2014, 7, 1));
	  /// <summary>
	  /// NotionalExchange (pay - GBP).
	  /// </summary>
	  public static readonly NotionalExchange NOTIONAL_EXCHANGE_PAY_GBP = NotionalExchange.of(CurrencyAmount.of(Currency.GBP, -NOTIONAL), date(2014, 7, 1));
	  /// <summary>
	  /// NotionalExchange (pay - USD).
	  /// </summary>
	  public static readonly NotionalExchange NOTIONAL_EXCHANGE_PAY_USD = NotionalExchange.of(CurrencyAmount.of(Currency.USD, -1.5d * NOTIONAL), date(2014, 7, 1));
	  /// <summary>
	  /// NotionalExchange.
	  /// </summary>
	  public static readonly FxResetNotionalExchange FX_RESET_NOTIONAL_EXCHANGE_REC_USD = FxResetNotionalExchange.of(CurrencyAmount.of(Currency.USD, NOTIONAL), date(2014, 7, 1), FxIndexObservation.of(FxIndices.GBP_USD_WM, date(2014, 7, 1), REF_DATA));

	  public static readonly FxResetNotionalExchange FX_RESET_NOTIONAL_EXCHANGE_PAY_GBP = FxResetNotionalExchange.of(CurrencyAmount.of(Currency.GBP, -NOTIONAL), date(2014, 7, 1), FxIndexObservation.of(FxIndices.GBP_USD_WM, date(2014, 7, 1), REF_DATA));

	  /// <summary>
	  /// IborRateComputation.
	  /// </summary>
	  public static readonly IborRateComputation IBOR_RATE_COMP = IborRateComputation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA);
	  /// <summary>
	  /// RateAccuralPeriod (ibor).
	  /// </summary>
	  public static readonly RateAccrualPeriod IBOR_RATE_ACCRUAL_PERIOD = RateAccrualPeriod.builder().startDate(date(2014, 7, 2)).endDate(date(2014, 10, 2)).rateComputation(IBOR_RATE_COMP).yearFraction(0.25d).build();
	  /// <summary>
	  /// RateAccuralPeriod (ibor).
	  /// </summary>
	  public static readonly RateAccrualPeriod IBOR_RATE_ACCRUAL_PERIOD_2 = RateAccrualPeriod.builder().startDate(date(2014, 10, 2)).endDate(date(2015, 1, 2)).rateComputation(IborRateComputation.of(GBP_LIBOR_3M, date(2014, 9, 30), REF_DATA)).yearFraction(0.25d).build();
	  /// <summary>
	  /// RatePaymentPeriod (ibor).
	  /// </summary>
	  public static readonly RatePaymentPeriod IBOR_RATE_PAYMENT_PERIOD_REC_GBP = RatePaymentPeriod.builder().paymentDate(date(2014, 10, 6)).accrualPeriods(IBOR_RATE_ACCRUAL_PERIOD).dayCount(ACT_365F).currency(Currency.GBP).notional(NOTIONAL).build();
	  /// <summary>
	  /// RatePaymentPeriod (ibor).
	  /// </summary>
	  public static readonly RatePaymentPeriod IBOR_RATE_PAYMENT_PERIOD_REC_GBP_2 = RatePaymentPeriod.builder().paymentDate(date(2015, 1, 4)).accrualPeriods(IBOR_RATE_ACCRUAL_PERIOD_2).dayCount(ACT_365F).currency(Currency.GBP).notional(NOTIONAL).build();
	  /// <summary>
	  /// ResolvedSwapLeg (ibor).
	  /// </summary>
	  public static readonly ResolvedSwapLeg IBOR_SWAP_LEG_REC_GBP = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(IBOR_RATE_PAYMENT_PERIOD_REC_GBP).paymentEvents(NOTIONAL_EXCHANGE_REC_GBP).build();
	  /// <summary>
	  /// ResolvedSwapLeg (ibor).
	  /// </summary>
	  public static readonly ResolvedSwapLeg IBOR_SWAP_LEG_REC_GBP_MULTI = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(IBOR_RATE_PAYMENT_PERIOD_REC_GBP, IBOR_RATE_PAYMENT_PERIOD_REC_GBP_2).paymentEvents(NOTIONAL_EXCHANGE_REC_GBP).build();
	  /// <summary>
	  /// ResolvedSwapLeg (known amount).
	  /// </summary>
	  public static readonly ResolvedSwapLeg KNOWN_AMOUNT_SWAP_LEG = KnownAmountSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2014, 4, 2)).endDate(date(2014, 10, 2)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build()).currency(GBP).amount(ValueSchedule.of(1000)).build().resolve(REF_DATA);

	  /// <summary>
	  /// FixedRateComputation.
	  /// </summary>
	  public static readonly FixedRateComputation FIXED_RATE_COMP = FixedRateComputation.of(0.0123d);
	  /// <summary>
	  /// RateAccuralPeriod (fixed).
	  /// </summary>
	  public static readonly RateAccrualPeriod FIXED_RATE_ACCRUAL_PERIOD = RateAccrualPeriod.builder().startDate(date(2014, 7, 2)).endDate(date(2014, 10, 2)).rateComputation(FIXED_RATE_COMP).yearFraction(0.25d).build();
	  /// <summary>
	  /// RateAccuralPeriod (fixed).
	  /// </summary>
	  public static readonly RateAccrualPeriod FIXED_RATE_ACCRUAL_PERIOD_2 = RateAccrualPeriod.builder().startDate(date(2014, 10, 2)).endDate(date(2015, 1, 2)).rateComputation(FIXED_RATE_COMP).yearFraction(0.25d).build();
	  /// <summary>
	  /// RatePaymentPeriod (fixed - receiver).
	  /// </summary>
	  public static readonly RatePaymentPeriod FIXED_RATE_PAYMENT_PERIOD_REC_GBP = RatePaymentPeriod.builder().paymentDate(date(2014, 10, 6)).accrualPeriods(FIXED_RATE_ACCRUAL_PERIOD).dayCount(ACT_365F).currency(Currency.GBP).notional(NOTIONAL).build();
	  /// <summary>
	  /// RatePaymentPeriod (fixed - receiver).
	  /// </summary>
	  public static readonly RatePaymentPeriod FIXED_RATE_PAYMENT_PERIOD_CMP_NONE_REC_GBP = RatePaymentPeriod.builder().paymentDate(date(2015, 1, 2)).accrualPeriods(FIXED_RATE_ACCRUAL_PERIOD, FIXED_RATE_ACCRUAL_PERIOD_2).dayCount(ACT_365F).currency(Currency.GBP).compoundingMethod(CompoundingMethod.NONE).notional(NOTIONAL).build();
	  /// <summary>
	  /// RatePaymentPeriod (fixed - receiver).
	  /// </summary>
	  public static readonly RatePaymentPeriod FIXED_RATE_PAYMENT_PERIOD_CMP_FLAT_REC_GBP = RatePaymentPeriod.builder().paymentDate(date(2015, 1, 2)).accrualPeriods(FIXED_RATE_ACCRUAL_PERIOD, FIXED_RATE_ACCRUAL_PERIOD_2).dayCount(ACT_365F).currency(Currency.GBP).compoundingMethod(CompoundingMethod.FLAT).notional(NOTIONAL).build();
	  /// <summary>
	  /// RatePaymentPeriod (fixed - payer).
	  /// </summary>
	  public static readonly RatePaymentPeriod FIXED_RATE_PAYMENT_PERIOD_PAY_GBP = RatePaymentPeriod.builder().paymentDate(date(2014, 10, 4)).accrualPeriods(FIXED_RATE_ACCRUAL_PERIOD).dayCount(ACT_365F).currency(Currency.GBP).notional(-NOTIONAL).build();
	  /// <summary>
	  /// RatePaymentPeriod (fixed - payer).
	  /// </summary>
	  public static readonly RatePaymentPeriod FIXED_RATE_PAYMENT_FX_RESET_PERIOD_PAY_GBP = RatePaymentPeriod.builder().paymentDate(date(2014, 10, 4)).accrualPeriods(FIXED_RATE_ACCRUAL_PERIOD).dayCount(ACT_365F).currency(Currency.GBP).notional(-NOTIONAL).fxReset(FxReset.of(FxIndexObservation.of(FxIndices.GBP_USD_WM, date(2014, 7, 2), REF_DATA), Currency.USD)).build();
	  /// <summary>
	  /// ResolvedSwapLeg (GBP - fixed - receiver).
	  /// </summary>
	  public static readonly ResolvedSwapLeg FIXED_SWAP_LEG_REC = ResolvedSwapLeg.builder().type(FIXED).payReceive(RECEIVE).paymentPeriods(FIXED_RATE_PAYMENT_PERIOD_REC_GBP).paymentEvents(NOTIONAL_EXCHANGE_REC_GBP).build();
	  /// <summary>
	  /// ResolvedSwapLeg (GBP - fixed - payer).
	  /// </summary>
	  public static readonly ResolvedSwapLeg FIXED_SWAP_LEG_PAY = ResolvedSwapLeg.builder().type(FIXED).payReceive(PAY).paymentPeriods(FIXED_RATE_PAYMENT_PERIOD_PAY_GBP).paymentEvents(NOTIONAL_EXCHANGE_PAY_GBP).build();
	  /// <summary>
	  /// RatePaymentPeriod (USD - fixed - receiver).
	  /// </summary>
	  public static readonly RatePaymentPeriod FIXED_RATE_PAYMENT_PERIOD_REC_USD = RatePaymentPeriod.builder().paymentDate(date(2014, 10, 4)).accrualPeriods(FIXED_RATE_ACCRUAL_PERIOD).dayCount(ACT_365F).currency(Currency.USD).notional(NOTIONAL).build();
	  /// <summary>
	  /// RatePaymentPeriod (USD - fixed - receiver).
	  /// </summary>
	  public static readonly RatePaymentPeriod FIXED_RATE_PAYMENT_PERIOD_PAY_USD = RatePaymentPeriod.builder().paymentDate(date(2014, 10, 4)).accrualPeriods(FIXED_RATE_ACCRUAL_PERIOD).dayCount(ACT_365F).currency(Currency.USD).notional(-NOTIONAL).build();
	  /// <summary>
	  /// RatePaymentPeriod (USD - fixed - receiver).
	  /// </summary>
	  public static readonly RatePaymentPeriod FIXED_RATE_PAYMENT_PERIOD_PAY_USD_2 = RatePaymentPeriod.builder().paymentDate(date(2015, 1, 4)).accrualPeriods(FIXED_RATE_ACCRUAL_PERIOD_2).dayCount(ACT_365F).currency(Currency.USD).notional(-NOTIONAL).build();
	  /// <summary>
	  /// ResolvedSwapLeg  (USD - fixed - receiver).
	  /// </summary>
	  public static readonly ResolvedSwapLeg FIXED_SWAP_LEG_REC_USD = ResolvedSwapLeg.builder().type(FIXED).payReceive(RECEIVE).paymentPeriods(FIXED_RATE_PAYMENT_PERIOD_REC_USD).build();
	  /// <summary>
	  /// ResolvedSwapLeg  (USD - fixed - receiver).
	  /// </summary>
	  public static readonly ResolvedSwapLeg FIXED_SWAP_LEG_PAY_USD = ResolvedSwapLeg.builder().type(FIXED).payReceive(PAY).paymentPeriods(FIXED_RATE_PAYMENT_PERIOD_PAY_USD).paymentEvents(NOTIONAL_EXCHANGE_PAY_USD).build();
	  /// <summary>
	  /// ResolvedSwapLeg  (USD - fixed - receiver - FX reset).
	  /// </summary>
	  public static readonly ResolvedSwapLeg FIXED_FX_RESET_SWAP_LEG_PAY_GBP = ResolvedSwapLeg.builder().type(FIXED).payReceive(PAY).paymentPeriods(FIXED_RATE_PAYMENT_FX_RESET_PERIOD_PAY_GBP).paymentEvents(FX_RESET_NOTIONAL_EXCHANGE_REC_USD).build();
	  /// <summary>
	  /// ResolvedSwapLeg  (GBP - fixed - receiver - compounding).
	  /// </summary>
	  public static readonly ResolvedSwapLeg FIXED_CMP_NONE_SWAP_LEG_PAY_GBP = ResolvedSwapLeg.builder().type(FIXED).payReceive(PAY).paymentPeriods(FIXED_RATE_PAYMENT_PERIOD_CMP_NONE_REC_GBP).build();
	  /// <summary>
	  /// ResolvedSwapLeg  (GBP - fixed - receiver - compounding).
	  /// </summary>
	  public static readonly ResolvedSwapLeg FIXED_CMP_FLAT_SWAP_LEG_PAY_GBP = ResolvedSwapLeg.builder().type(FIXED).payReceive(PAY).paymentPeriods(FIXED_RATE_PAYMENT_PERIOD_CMP_FLAT_REC_GBP).build();
	  /// <summary>
	  /// ResolvedSwapLeg (fixed).
	  /// </summary>
	  public static readonly ResolvedSwapLeg FIXED_RATECALC_SWAP_LEG = RateCalculationSwapLeg.builder().payReceive(PayReceive.RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2014, 4, 2)).endDate(date(2014, 10, 2)).frequency(Frequency.P3M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.P3M).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build()).notionalSchedule(NotionalSchedule.of(Currency.GBP, NOTIONAL)).calculation(FixedRateCalculation.of(0.0123d, DayCounts.ACT_365F)).build().resolve(REF_DATA);
	  /// <summary>
	  /// ResolvedSwapLeg (inflation)
	  /// </summary>
	  public static readonly ResolvedSwapLeg INFLATION_MONTHLY_SWAP_LEG_REC_GBP = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(date(2014, 6, 9)).endDate(date(2019, 6, 9)).frequency(Frequency.ofYears(5)).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.ofYears(5)).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build()).calculation(InflationRateCalculation.builder().index(GB_RPI).indexCalculationMethod(MONTHLY).lag(Period.ofMonths(3)).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL)).build().resolve(REF_DATA);
	  /// <summary>
	  /// ResolvedSwapLeg fixed rate.
	  /// </summary>
	  public const double INFLATION_FIXED_SWAP_LEG_PAY_GBP_FIXED_RATE = 0.0358d;
	  /// <summary>
	  /// ResolvedSwapLeg (fixed - to be used as a counterpart of INFLATION_MONTHLY_SWAP_LEG_REC_GBP)
	  /// </summary>
	  public static readonly ResolvedSwapLeg INFLATION_FIXED_SWAP_LEG_PAY_GBP = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(date(2014, 6, 9)).endDate(date(2019, 6, 9)).frequency(Frequency.P12M).businessDayAdjustment(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.ofYears(5)).paymentDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).compoundingMethod(CompoundingMethod.STRAIGHT).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL)).calculation(FixedRateCalculation.builder().rate(ValueSchedule.of(INFLATION_FIXED_SWAP_LEG_PAY_GBP_FIXED_RATE)).dayCount(DayCounts.ONE_ONE).build()).build().resolve(REF_DATA);

	  /// <summary>
	  /// Single currency swap.
	  /// </summary>
	  public static readonly ResolvedSwap SWAP = ResolvedSwap.of(IBOR_SWAP_LEG_REC_GBP, FIXED_SWAP_LEG_PAY);
	  /// <summary>
	  /// OvernightIndexedSwap.
	  /// </summary>
	  public static readonly ResolvedSwap OIS = FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS.createTrade(LocalDate.of(2017, 6, 28), Tenor.TENOR_1Y, BuySell.BUY, 1_000_000, 0.01, REF_DATA).Product.resolve(REF_DATA);
	  /// <summary>
	  /// Cross currency swap.
	  /// </summary>
	  public static readonly ResolvedSwap SWAP_CROSS_CURRENCY = ResolvedSwap.of(IBOR_SWAP_LEG_REC_GBP, FIXED_SWAP_LEG_PAY_USD);

	  /// <summary>
	  /// Inflation Swap.
	  /// </summary>
	  public static readonly ResolvedSwap SWAP_INFLATION = ResolvedSwap.builder().legs(INFLATION_MONTHLY_SWAP_LEG_REC_GBP, INFLATION_FIXED_SWAP_LEG_PAY_GBP).build();

	  /// <summary>
	  /// Swap trade.
	  /// </summary>
	  public static readonly ResolvedSwapTrade SWAP_TRADE = ResolvedSwapTrade.builder().info(TradeInfo.builder().tradeDate(date(2014, 6, 30)).build()).product(SWAP).build();

	  /// <summary>
	  /// Swap trade.
	  /// </summary>
	  public static readonly ResolvedSwapTrade SWAP_TRADE_CROSS_CURRENCY = ResolvedSwapTrade.builder().info(TradeInfo.builder().tradeDate(date(2014, 6, 30)).build()).product(SWAP_CROSS_CURRENCY).build();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private SwapDummyData()
	  {
	  }

	}

}