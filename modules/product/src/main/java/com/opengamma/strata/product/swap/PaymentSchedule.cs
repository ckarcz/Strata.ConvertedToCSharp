using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using ScheduleException = com.opengamma.strata.basics.schedule.ScheduleException;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// Defines the schedule of payment dates relative to the accrual periods.
	/// <para>
	/// This defines the data necessary to create a schedule of payment periods.
	/// Each payment period contains one or more accrual periods.
	/// If a payment period contains more than one accrual period then the compounding
	/// method will be used to combine the amounts.
	/// </para>
	/// <para>
	/// This class defines payment periods using a periodic frequency.
	/// The frequency must match or be a multiple of the accrual periodic frequency.
	/// </para>
	/// <para>
	/// If the payment frequency is 'Term' then there is only one payment.
	/// As such, a 'Term' payment frequency causes stubs to be treated solely as accrual periods.
	/// In all other cases, stubs are treated as payment periods in their own right.
	/// </para>
	/// <para>
	/// When applying the frequency, it is converted into an integer value, representing the
	/// number of accrual periods per payment period. The accrual periods are allocated by rolling
	/// forwards or backwards, applying the same direction as accrual schedule generation.
	/// </para>
	/// <para>
	/// A different business day adjustment may be specified for the payment schedule to that
	/// used for the accrual schedule. When resolving the swap, the adjustment will be applied
	/// as part of the process that creates the payment date. Note that the start and end dates
	/// of the payment period, as defined by the payment schedule, cannot be observed on the
	/// resulting <seealso cref="RatePaymentPeriod"/> instance.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class PaymentSchedule implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class PaymentSchedule : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.Frequency paymentFrequency;
		private readonly Frequency paymentFrequency;
	  /// <summary>
	  /// The business day adjustment to apply, optional.
	  /// <para>
	  /// Each date in the calculated schedule is determined relative to the accrual schedule.
	  /// Normally, the accrual schedule is adjusted ensuring each date is not a holiday.
	  /// As such, there is typically no reason to adjust the date before applying the payment date offset.
	  /// </para>
	  /// <para>
	  /// If the accrual dates are unadjusted, or for some other reason, it may be
	  /// desirable to adjust the schedule dates before applying the payment date offset.
	  /// This optional property allows that to happen.
	  /// Note that the payment date offset itself provides the ability to adjust dates
	  /// after the offset is applied.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;
	  /// <summary>
	  /// The base date that each payment is made relative to, defaulted to 'PeriodEnd'.
	  /// <para>
	  /// The payment date is relative to either the start or end of the payment period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final PaymentRelativeTo paymentRelativeTo;
	  private readonly PaymentRelativeTo paymentRelativeTo;
	  /// <summary>
	  /// The offset of payment from the base calculation period date.
	  /// <para>
	  /// The offset is applied to the unadjusted date specified by {@code paymentRelativeTo}.
	  /// Offset can be based on calendar days or business days.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment paymentDateOffset;
	  private readonly DaysAdjustment paymentDateOffset;
	  /// <summary>
	  /// The compounding method to use when there is more than one accrual period, defaulted to 'None'.
	  /// <para>
	  /// Compounding is used when combining accrual periods.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CompoundingMethod compoundingMethod;
	  private readonly CompoundingMethod compoundingMethod;
	  /// <summary>
	  /// The optional start date of the first regular payment schedule period, which is the end date of the initial stub.
	  /// <para>
	  /// This is used to identify the boundary date between the initial stub and the first regular period.
	  /// In most cases there is no need to specify this as it can be worked out from other information.
	  /// It must be used when there is a need to produce a payment schedule with an initial stub that combines
	  /// an initial stub from the accrual schedule with the first regular period of the accrual schedule.
	  /// </para>
	  /// <para>
	  /// This is an unadjusted date, and as such it might not be a valid business day.
	  /// It must equal one of the unadjusted dates on the accrual schedule.
	  /// </para>
	  /// <para>
	  /// If <seealso cref="#getPaymentRelativeTo() paymentRelativeTo"/> is 'PeriodEnd' then this field
	  /// corresponds to {@code firstPaymentDate} in FpML.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.LocalDate firstRegularStartDate;
	  private readonly LocalDate firstRegularStartDate;
	  /// <summary>
	  /// The optional end date of the last regular payment schedule period, which is the start date of the final stub.
	  /// <para>
	  /// This is used to identify the boundary date between the last regular period and the final stub.
	  /// In most cases there is no need to specify this as it can be worked out from other information.
	  /// It must be used when there is a need to produce a payment schedule with a final stub that combines
	  /// a final stub from the accrual schedule with the last regular period of the accrual schedule.
	  /// </para>
	  /// <para>
	  /// This is used to identify the boundary date between the last regular schedule period and the final stub.
	  /// </para>
	  /// <para>
	  /// This is an unadjusted date, and as such it might not be a valid business day.
	  /// This date must be after 'firstPaymentDate'.
	  /// It must equal one of the unadjusted dates on the accrual schedule.
	  /// </para>
	  /// <para>
	  /// If <seealso cref="#getPaymentRelativeTo() paymentRelativeTo"/> is 'PeriodEnd' then this field
	  /// corresponds to {@code lastRegularPaymentDate} in FpML.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.LocalDate lastRegularEndDate;
	  private readonly LocalDate lastRegularEndDate;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (firstRegularStartDate != null && lastRegularEndDate != null)
		{
		  ArgChecker.inOrderNotEqual(firstRegularStartDate, lastRegularEndDate, "firstPaymentDate", "lastRegularPaymentDate");
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.paymentRelativeTo(PaymentRelativeTo.PERIOD_END);
		builder.compoundingMethod(CompoundingMethod.NONE);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the payment schedule based on the accrual schedule.
	  /// <para>
	  /// If the payment frequency matches the accrual frequency, or if there is
	  /// only one period in the accrual schedule, the input schedule is returned.
	  /// </para>
	  /// <para>
	  /// Only the regular part of the accrual schedule is grouped into payment periods.
	  /// Any initial or final stub will be returned unaltered in the new schedule.
	  /// </para>
	  /// <para>
	  /// The grouping is determined by rolling forwards or backwards through the regular accrual periods
	  /// Rolling is backwards if there is an initial stub, otherwise rolling is forwards.
	  /// Grouping involves merging the existing accrual periods, thus the roll convention
	  /// of the accrual periods is implicitly applied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="accrualSchedule">  the accrual schedule </param>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the payment schedule </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="IllegalArgumentException"> if the accrual frequency does not divide evenly into the payment frequency </exception>
	  public Schedule createSchedule(Schedule accrualSchedule, ReferenceData refData)
	  {
		// payment frequency of Term absorbs everything
		if (paymentFrequency.Equals(Frequency.TERM))
		{
		  if (firstRegularStartDate != null && !firstRegularStartDate.Equals(accrualSchedule.UnadjustedStartDate) && !firstRegularStartDate.Equals(accrualSchedule.StartDate))
		  {
			throw new ScheduleException("Unable to create schedule for frequency 'Term' when firstRegularStartDate != startDate");
		  }
		  if (lastRegularEndDate != null && !lastRegularEndDate.Equals(accrualSchedule.UnadjustedEndDate) && !lastRegularEndDate.Equals(accrualSchedule.EndDate))
		  {
			throw new ScheduleException("Unable to create schedule for frequency 'Term' when lastRegularEndDate != endDate");
		  }
		  return accrualSchedule.mergeToTerm();
		}
		// derive schedule, retaining stubs as payment periods
		int accrualPeriodsPerPayment = paymentFrequency.exactDivide(accrualSchedule.Frequency);
		Schedule paySchedule;
		if (firstRegularStartDate != null && lastRegularEndDate != null)
		{
		  paySchedule = accrualSchedule.merge(accrualPeriodsPerPayment, firstRegularStartDate, lastRegularEndDate);

		}
		else if (firstRegularStartDate != null || lastRegularEndDate != null)
		{
		  LocalDate firstRegular = firstRegularStartDate != null ? firstRegularStartDate : accrualSchedule.InitialStub.map(stub => stub.UnadjustedEndDate).orElse(accrualSchedule.UnadjustedStartDate);
		  LocalDate lastRegular = lastRegularEndDate != null ? lastRegularEndDate : accrualSchedule.FinalStub.map(stub => stub.UnadjustedStartDate).orElse(accrualSchedule.UnadjustedEndDate);
		  paySchedule = accrualSchedule.merge(accrualPeriodsPerPayment, firstRegular, lastRegular);

		}
		else
		{
		  bool rollForwards = !accrualSchedule.InitialStub.Present;
		  paySchedule = accrualSchedule.mergeRegular(accrualPeriodsPerPayment, rollForwards);
		}
		// adjust for business days
		if (businessDayAdjustment != null)
		{
		  return paySchedule.toAdjusted(businessDayAdjustment.resolve(refData));
		}
		return paySchedule;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the list of payment periods from the list of accrual periods.
	  /// <para>
	  /// This applies the payment schedule.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="accrualSchedule">  the accrual schedule </param>
	  /// <param name="paymentSchedule">  the payment schedule </param>
	  /// <param name="accrualPeriods">  the list of accrual periods </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="notionalSchedule">  the schedule of notionals </param>
	  /// <param name="payReceive">  the pay-receive flag </param>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the list of payment periods </returns>
	  internal ImmutableList<NotionalPaymentPeriod> createPaymentPeriods(Schedule accrualSchedule, Schedule paymentSchedule, IList<RateAccrualPeriod> accrualPeriods, DayCount dayCount, NotionalSchedule notionalSchedule, PayReceive payReceive, ReferenceData refData)
	  {

		DoubleArray notionals = notionalSchedule.Amount.resolveValues(paymentSchedule);
		// resolve against reference data once
		DateAdjuster paymentDateAdjuster = paymentDateOffset.resolve(refData);
		System.Func<int, SchedulePeriod, Optional<FxReset>> fxResetFn = notionalSchedule.FxReset.map(calc => calc.resolve(refData)).orElse((i, p) => null);

		// build up payment periods using schedule
		ImmutableList.Builder<NotionalPaymentPeriod> paymentPeriods = ImmutableList.builder();
		System.Func<int, CurrencyAmount> notionalFunction = getNotionalSupplierFunction(notionalSchedule, notionals, payReceive);

		// compare using == as Schedule.mergeRegular() will return same schedule
		if (accrualSchedule == paymentSchedule)
		{
		  // same schedule means one accrual period per payment period
		  for (int index = 0; index < paymentSchedule.size(); index++)
		  {
			SchedulePeriod period = paymentSchedule.getPeriod(index);
			CurrencyAmount notional = notionalFunction(index);
			ImmutableList<RateAccrualPeriod> paymentAccrualPeriods = ImmutableList.of(accrualPeriods[index]);
			paymentPeriods.add(createPaymentPeriod(index, period, paymentAccrualPeriods, paymentDateAdjuster, fxResetFn, dayCount, notional));
		  }
		}
		else
		{
		  // multiple accrual periods per payment period, or accrual/payment schedules differ
		  int accrualIndex = 0;
		  for (int paymentIndex = 0; paymentIndex < paymentSchedule.size(); paymentIndex++)
		  {
			SchedulePeriod payPeriod = paymentSchedule.getPeriod(paymentIndex);
			CurrencyAmount notional = notionalFunction(paymentIndex);
			int accrualStartIndex = accrualIndex;
			RateAccrualPeriod accrual = accrualPeriods[accrualIndex];
			while (accrual.UnadjustedEndDate.isBefore(payPeriod.UnadjustedEndDate))
			{
			  accrual = accrualPeriods[++accrualIndex];
			}
			IList<RateAccrualPeriod> paymentAccrualPeriods = accrualPeriods.subList(accrualStartIndex, accrualIndex + 1);
			paymentPeriods.add(createPaymentPeriod(paymentIndex, payPeriod, paymentAccrualPeriods, paymentDateAdjuster, fxResetFn, dayCount, notional));
			accrualIndex++;
		  }
		}
		return paymentPeriods.build();
	  }

	  //Returns a function which takes a payment period index and returns the notional for the index
	  private System.Func<int, CurrencyAmount> getNotionalSupplierFunction(NotionalSchedule notionalSchedule, DoubleArray notionals, PayReceive payReceive)
	  {

		bool hasInitialFxNotional = notionalSchedule.FxReset.Present && notionalSchedule.FxReset.get().InitialNotionalValue.Present;

		return index =>
		{
	  if (hasInitialFxNotional && index == 0)
	  {
		FxResetCalculation fxReset = notionalSchedule.FxReset.get();
		//If Fx reset leg with fixed initial notional then return the fixed amount in the payment currency
		double notional = payReceive.normalize(fxReset.InitialNotionalValue.Value);
		Currency currency = fxReset.Index.CurrencyPair.other(fxReset.ReferenceCurrency);
		return CurrencyAmount.of(currency, notional);
	  }
	  else
	  {
		double notional = payReceive.normalize(notionals.get(index));
		return CurrencyAmount.of(notionalSchedule.Currency, notional);
	  }
		};
	  }

	  // create the payment period
	  private NotionalPaymentPeriod createPaymentPeriod(int paymentPeriodIndex, SchedulePeriod paymentPeriod, IList<RateAccrualPeriod> periods, DateAdjuster paymentDateAdjuster, System.Func<int, SchedulePeriod, Optional<FxReset>> fxResetFn, DayCount dayCount, CurrencyAmount notional)
	  {

		// FpML cash flow example 3 shows payment offset calculated from adjusted accrual date (not unadjusted)
		LocalDate paymentDate = paymentDateAdjuster.adjust(paymentRelativeTo.selectBaseDate(paymentPeriod));

		// extract FX reset information
		FxReset fxReset = fxResetFn(paymentPeriodIndex, paymentPeriod).orElse(null);

		// handle special case where amount is known
		if (periods.Count == 1 && periods[0].RateComputation is KnownAmountRateComputation)
		{
		  CurrencyAmount amount = ((KnownAmountRateComputation) periods[0].RateComputation).Amount;
		  Payment payment = Payment.of(amount, paymentDate);
		  if (fxReset != null)
		  {
			CurrencyAmount notionalAmount = CurrencyAmount.of(fxReset.ReferenceCurrency, amount.Amount);
			return KnownAmountNotionalSwapPaymentPeriod.of(payment, paymentPeriod, notionalAmount, fxReset.Observation);
		  }
		  else
		  {
			return KnownAmountNotionalSwapPaymentPeriod.of(payment, paymentPeriod, notional);
		  }
		}
		// rate based computation
		return new RatePaymentPeriod(paymentDate, periods, dayCount, notional.Currency, fxReset, notional.Amount, compoundingMethod);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code PaymentSchedule}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static PaymentSchedule.Meta meta()
	  {
		return PaymentSchedule.Meta.INSTANCE;
	  }

	  static PaymentSchedule()
	  {
		MetaBean.register(PaymentSchedule.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static PaymentSchedule.Builder builder()
	  {
		return new PaymentSchedule.Builder();
	  }

	  private PaymentSchedule(Frequency paymentFrequency, BusinessDayAdjustment businessDayAdjustment, PaymentRelativeTo paymentRelativeTo, DaysAdjustment paymentDateOffset, CompoundingMethod compoundingMethod, LocalDate firstRegularStartDate, LocalDate lastRegularEndDate)
	  {
		JodaBeanUtils.notNull(paymentFrequency, "paymentFrequency");
		JodaBeanUtils.notNull(paymentRelativeTo, "paymentRelativeTo");
		JodaBeanUtils.notNull(paymentDateOffset, "paymentDateOffset");
		JodaBeanUtils.notNull(compoundingMethod, "compoundingMethod");
		this.paymentFrequency = paymentFrequency;
		this.businessDayAdjustment = businessDayAdjustment;
		this.paymentRelativeTo = paymentRelativeTo;
		this.paymentDateOffset = paymentDateOffset;
		this.compoundingMethod = compoundingMethod;
		this.firstRegularStartDate = firstRegularStartDate;
		this.lastRegularEndDate = lastRegularEndDate;
		validate();
	  }

	  public override PaymentSchedule.Meta metaBean()
	  {
		return PaymentSchedule.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the periodic frequency of payments.
	  /// <para>
	  /// Regular payments will be made at the specified periodic frequency.
	  /// The frequency must be the same as, or a multiple of, the accrual periodic frequency.
	  /// </para>
	  /// <para>
	  /// Compounding applies if the payment frequency does not equal the accrual frequency.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Frequency PaymentFrequency
	  {
		  get
		  {
			return paymentFrequency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the business day adjustment to apply, optional.
	  /// <para>
	  /// Each date in the calculated schedule is determined relative to the accrual schedule.
	  /// Normally, the accrual schedule is adjusted ensuring each date is not a holiday.
	  /// As such, there is typically no reason to adjust the date before applying the payment date offset.
	  /// </para>
	  /// <para>
	  /// If the accrual dates are unadjusted, or for some other reason, it may be
	  /// desirable to adjust the schedule dates before applying the payment date offset.
	  /// This optional property allows that to happen.
	  /// Note that the payment date offset itself provides the ability to adjust dates
	  /// after the offset is applied.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<BusinessDayAdjustment> BusinessDayAdjustment
	  {
		  get
		  {
			return Optional.ofNullable(businessDayAdjustment);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the base date that each payment is made relative to, defaulted to 'PeriodEnd'.
	  /// <para>
	  /// The payment date is relative to either the start or end of the payment period.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PaymentRelativeTo PaymentRelativeTo
	  {
		  get
		  {
			return paymentRelativeTo;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of payment from the base calculation period date.
	  /// <para>
	  /// The offset is applied to the unadjusted date specified by {@code paymentRelativeTo}.
	  /// Offset can be based on calendar days or business days.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment PaymentDateOffset
	  {
		  get
		  {
			return paymentDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the compounding method to use when there is more than one accrual period, defaulted to 'None'.
	  /// <para>
	  /// Compounding is used when combining accrual periods.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CompoundingMethod CompoundingMethod
	  {
		  get
		  {
			return compoundingMethod;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional start date of the first regular payment schedule period, which is the end date of the initial stub.
	  /// <para>
	  /// This is used to identify the boundary date between the initial stub and the first regular period.
	  /// In most cases there is no need to specify this as it can be worked out from other information.
	  /// It must be used when there is a need to produce a payment schedule with an initial stub that combines
	  /// an initial stub from the accrual schedule with the first regular period of the accrual schedule.
	  /// </para>
	  /// <para>
	  /// This is an unadjusted date, and as such it might not be a valid business day.
	  /// It must equal one of the unadjusted dates on the accrual schedule.
	  /// </para>
	  /// <para>
	  /// If <seealso cref="#getPaymentRelativeTo() paymentRelativeTo"/> is 'PeriodEnd' then this field
	  /// corresponds to {@code firstPaymentDate} in FpML.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<LocalDate> FirstRegularStartDate
	  {
		  get
		  {
			return Optional.ofNullable(firstRegularStartDate);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional end date of the last regular payment schedule period, which is the start date of the final stub.
	  /// <para>
	  /// This is used to identify the boundary date between the last regular period and the final stub.
	  /// In most cases there is no need to specify this as it can be worked out from other information.
	  /// It must be used when there is a need to produce a payment schedule with a final stub that combines
	  /// a final stub from the accrual schedule with the last regular period of the accrual schedule.
	  /// </para>
	  /// <para>
	  /// This is used to identify the boundary date between the last regular schedule period and the final stub.
	  /// </para>
	  /// <para>
	  /// This is an unadjusted date, and as such it might not be a valid business day.
	  /// This date must be after 'firstPaymentDate'.
	  /// It must equal one of the unadjusted dates on the accrual schedule.
	  /// </para>
	  /// <para>
	  /// If <seealso cref="#getPaymentRelativeTo() paymentRelativeTo"/> is 'PeriodEnd' then this field
	  /// corresponds to {@code lastRegularPaymentDate} in FpML.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<LocalDate> LastRegularEndDate
	  {
		  get
		  {
			return Optional.ofNullable(lastRegularEndDate);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  PaymentSchedule other = (PaymentSchedule) obj;
		  return JodaBeanUtils.equal(paymentFrequency, other.paymentFrequency) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment) && JodaBeanUtils.equal(paymentRelativeTo, other.paymentRelativeTo) && JodaBeanUtils.equal(paymentDateOffset, other.paymentDateOffset) && JodaBeanUtils.equal(compoundingMethod, other.compoundingMethod) && JodaBeanUtils.equal(firstRegularStartDate, other.firstRegularStartDate) && JodaBeanUtils.equal(lastRegularEndDate, other.lastRegularEndDate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentFrequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentRelativeTo);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(compoundingMethod);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(firstRegularStartDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastRegularEndDate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("PaymentSchedule{");
		buf.Append("paymentFrequency").Append('=').Append(paymentFrequency).Append(',').Append(' ');
		buf.Append("businessDayAdjustment").Append('=').Append(businessDayAdjustment).Append(',').Append(' ');
		buf.Append("paymentRelativeTo").Append('=').Append(paymentRelativeTo).Append(',').Append(' ');
		buf.Append("paymentDateOffset").Append('=').Append(paymentDateOffset).Append(',').Append(' ');
		buf.Append("compoundingMethod").Append('=').Append(compoundingMethod).Append(',').Append(' ');
		buf.Append("firstRegularStartDate").Append('=').Append(firstRegularStartDate).Append(',').Append(' ');
		buf.Append("lastRegularEndDate").Append('=').Append(JodaBeanUtils.ToString(lastRegularEndDate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code PaymentSchedule}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  paymentFrequency_Renamed = DirectMetaProperty.ofImmutable(this, "paymentFrequency", typeof(PaymentSchedule), typeof(Frequency));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(PaymentSchedule), typeof(BusinessDayAdjustment));
			  paymentRelativeTo_Renamed = DirectMetaProperty.ofImmutable(this, "paymentRelativeTo", typeof(PaymentSchedule), typeof(PaymentRelativeTo));
			  paymentDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDateOffset", typeof(PaymentSchedule), typeof(DaysAdjustment));
			  compoundingMethod_Renamed = DirectMetaProperty.ofImmutable(this, "compoundingMethod", typeof(PaymentSchedule), typeof(CompoundingMethod));
			  firstRegularStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "firstRegularStartDate", typeof(PaymentSchedule), typeof(LocalDate));
			  lastRegularEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastRegularEndDate", typeof(PaymentSchedule), typeof(LocalDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "paymentFrequency", "businessDayAdjustment", "paymentRelativeTo", "paymentDateOffset", "compoundingMethod", "firstRegularStartDate", "lastRegularEndDate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code paymentFrequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> paymentFrequency_Renamed;
		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> businessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentRelativeTo} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PaymentRelativeTo> paymentRelativeTo_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> paymentDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code compoundingMethod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CompoundingMethod> compoundingMethod_Renamed;
		/// <summary>
		/// The meta-property for the {@code firstRegularStartDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> firstRegularStartDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code lastRegularEndDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> lastRegularEndDate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "paymentFrequency", "businessDayAdjustment", "paymentRelativeTo", "paymentDateOffset", "compoundingMethod", "firstRegularStartDate", "lastRegularEndDate");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 863656438: // paymentFrequency
			  return paymentFrequency_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case -1357627123: // paymentRelativeTo
			  return paymentRelativeTo_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case -1376171496: // compoundingMethod
			  return compoundingMethod_Renamed;
			case 2011803076: // firstRegularStartDate
			  return firstRegularStartDate_Renamed;
			case -1540679645: // lastRegularEndDate
			  return lastRegularEndDate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override PaymentSchedule.Builder builder()
		{
		  return new PaymentSchedule.Builder();
		}

		public override Type beanType()
		{
		  return typeof(PaymentSchedule);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code paymentFrequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> paymentFrequency()
		{
		  return paymentFrequency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> businessDayAdjustment()
		{
		  return businessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentRelativeTo} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PaymentRelativeTo> paymentRelativeTo()
		{
		  return paymentRelativeTo_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> paymentDateOffset()
		{
		  return paymentDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code compoundingMethod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CompoundingMethod> compoundingMethod()
		{
		  return compoundingMethod_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code firstRegularStartDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> firstRegularStartDate()
		{
		  return firstRegularStartDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lastRegularEndDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> lastRegularEndDate()
		{
		  return lastRegularEndDate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 863656438: // paymentFrequency
			  return ((PaymentSchedule) bean).PaymentFrequency;
			case -1065319863: // businessDayAdjustment
			  return ((PaymentSchedule) bean).businessDayAdjustment;
			case -1357627123: // paymentRelativeTo
			  return ((PaymentSchedule) bean).PaymentRelativeTo;
			case -716438393: // paymentDateOffset
			  return ((PaymentSchedule) bean).PaymentDateOffset;
			case -1376171496: // compoundingMethod
			  return ((PaymentSchedule) bean).CompoundingMethod;
			case 2011803076: // firstRegularStartDate
			  return ((PaymentSchedule) bean).firstRegularStartDate;
			case -1540679645: // lastRegularEndDate
			  return ((PaymentSchedule) bean).lastRegularEndDate;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code PaymentSchedule}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<PaymentSchedule>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency paymentFrequency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PaymentRelativeTo paymentRelativeTo_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment paymentDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CompoundingMethod compoundingMethod_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate firstRegularStartDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate lastRegularEndDate_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(PaymentSchedule beanToCopy)
		{
		  this.paymentFrequency_Renamed = beanToCopy.PaymentFrequency;
		  this.businessDayAdjustment_Renamed = beanToCopy.businessDayAdjustment;
		  this.paymentRelativeTo_Renamed = beanToCopy.PaymentRelativeTo;
		  this.paymentDateOffset_Renamed = beanToCopy.PaymentDateOffset;
		  this.compoundingMethod_Renamed = beanToCopy.CompoundingMethod;
		  this.firstRegularStartDate_Renamed = beanToCopy.firstRegularStartDate;
		  this.lastRegularEndDate_Renamed = beanToCopy.lastRegularEndDate;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 863656438: // paymentFrequency
			  return paymentFrequency_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case -1357627123: // paymentRelativeTo
			  return paymentRelativeTo_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case -1376171496: // compoundingMethod
			  return compoundingMethod_Renamed;
			case 2011803076: // firstRegularStartDate
			  return firstRegularStartDate_Renamed;
			case -1540679645: // lastRegularEndDate
			  return lastRegularEndDate_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 863656438: // paymentFrequency
			  this.paymentFrequency_Renamed = (Frequency) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case -1357627123: // paymentRelativeTo
			  this.paymentRelativeTo_Renamed = (PaymentRelativeTo) newValue;
			  break;
			case -716438393: // paymentDateOffset
			  this.paymentDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case -1376171496: // compoundingMethod
			  this.compoundingMethod_Renamed = (CompoundingMethod) newValue;
			  break;
			case 2011803076: // firstRegularStartDate
			  this.firstRegularStartDate_Renamed = (LocalDate) newValue;
			  break;
			case -1540679645: // lastRegularEndDate
			  this.lastRegularEndDate_Renamed = (LocalDate) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override PaymentSchedule build()
		{
		  return new PaymentSchedule(paymentFrequency_Renamed, businessDayAdjustment_Renamed, paymentRelativeTo_Renamed, paymentDateOffset_Renamed, compoundingMethod_Renamed, firstRegularStartDate_Renamed, lastRegularEndDate_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the periodic frequency of payments.
		/// <para>
		/// Regular payments will be made at the specified periodic frequency.
		/// The frequency must be the same as, or a multiple of, the accrual periodic frequency.
		/// </para>
		/// <para>
		/// Compounding applies if the payment frequency does not equal the accrual frequency.
		/// </para>
		/// </summary>
		/// <param name="paymentFrequency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentFrequency(Frequency paymentFrequency)
		{
		  JodaBeanUtils.notNull(paymentFrequency, "paymentFrequency");
		  this.paymentFrequency_Renamed = paymentFrequency;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply, optional.
		/// <para>
		/// Each date in the calculated schedule is determined relative to the accrual schedule.
		/// Normally, the accrual schedule is adjusted ensuring each date is not a holiday.
		/// As such, there is typically no reason to adjust the date before applying the payment date offset.
		/// </para>
		/// <para>
		/// If the accrual dates are unadjusted, or for some other reason, it may be
		/// desirable to adjust the schedule dates before applying the payment date offset.
		/// This optional property allows that to happen.
		/// Note that the payment date offset itself provides the ability to adjust dates
		/// after the offset is applied.
		/// </para>
		/// </summary>
		/// <param name="businessDayAdjustment">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder businessDayAdjustment(BusinessDayAdjustment businessDayAdjustment)
		{
		  this.businessDayAdjustment_Renamed = businessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the base date that each payment is made relative to, defaulted to 'PeriodEnd'.
		/// <para>
		/// The payment date is relative to either the start or end of the payment period.
		/// </para>
		/// </summary>
		/// <param name="paymentRelativeTo">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentRelativeTo(PaymentRelativeTo paymentRelativeTo)
		{
		  JodaBeanUtils.notNull(paymentRelativeTo, "paymentRelativeTo");
		  this.paymentRelativeTo_Renamed = paymentRelativeTo;
		  return this;
		}

		/// <summary>
		/// Sets the offset of payment from the base calculation period date.
		/// <para>
		/// The offset is applied to the unadjusted date specified by {@code paymentRelativeTo}.
		/// Offset can be based on calendar days or business days.
		/// </para>
		/// </summary>
		/// <param name="paymentDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentDateOffset(DaysAdjustment paymentDateOffset)
		{
		  JodaBeanUtils.notNull(paymentDateOffset, "paymentDateOffset");
		  this.paymentDateOffset_Renamed = paymentDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the compounding method to use when there is more than one accrual period, defaulted to 'None'.
		/// <para>
		/// Compounding is used when combining accrual periods.
		/// </para>
		/// </summary>
		/// <param name="compoundingMethod">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder compoundingMethod(CompoundingMethod compoundingMethod)
		{
		  JodaBeanUtils.notNull(compoundingMethod, "compoundingMethod");
		  this.compoundingMethod_Renamed = compoundingMethod;
		  return this;
		}

		/// <summary>
		/// Sets the optional start date of the first regular payment schedule period, which is the end date of the initial stub.
		/// <para>
		/// This is used to identify the boundary date between the initial stub and the first regular period.
		/// In most cases there is no need to specify this as it can be worked out from other information.
		/// It must be used when there is a need to produce a payment schedule with an initial stub that combines
		/// an initial stub from the accrual schedule with the first regular period of the accrual schedule.
		/// </para>
		/// <para>
		/// This is an unadjusted date, and as such it might not be a valid business day.
		/// It must equal one of the unadjusted dates on the accrual schedule.
		/// </para>
		/// <para>
		/// If <seealso cref="#getPaymentRelativeTo() paymentRelativeTo"/> is 'PeriodEnd' then this field
		/// corresponds to {@code firstPaymentDate} in FpML.
		/// </para>
		/// </summary>
		/// <param name="firstRegularStartDate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder firstRegularStartDate(LocalDate firstRegularStartDate)
		{
		  this.firstRegularStartDate_Renamed = firstRegularStartDate;
		  return this;
		}

		/// <summary>
		/// Sets the optional end date of the last regular payment schedule period, which is the start date of the final stub.
		/// <para>
		/// This is used to identify the boundary date between the last regular period and the final stub.
		/// In most cases there is no need to specify this as it can be worked out from other information.
		/// It must be used when there is a need to produce a payment schedule with a final stub that combines
		/// a final stub from the accrual schedule with the last regular period of the accrual schedule.
		/// </para>
		/// <para>
		/// This is used to identify the boundary date between the last regular schedule period and the final stub.
		/// </para>
		/// <para>
		/// This is an unadjusted date, and as such it might not be a valid business day.
		/// This date must be after 'firstPaymentDate'.
		/// It must equal one of the unadjusted dates on the accrual schedule.
		/// </para>
		/// <para>
		/// If <seealso cref="#getPaymentRelativeTo() paymentRelativeTo"/> is 'PeriodEnd' then this field
		/// corresponds to {@code lastRegularPaymentDate} in FpML.
		/// </para>
		/// </summary>
		/// <param name="lastRegularEndDate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lastRegularEndDate(LocalDate lastRegularEndDate)
		{
		  this.lastRegularEndDate_Renamed = lastRegularEndDate;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("PaymentSchedule.Builder{");
		  buf.Append("paymentFrequency").Append('=').Append(JodaBeanUtils.ToString(paymentFrequency_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentRelativeTo").Append('=').Append(JodaBeanUtils.ToString(paymentRelativeTo_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDateOffset").Append('=').Append(JodaBeanUtils.ToString(paymentDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("compoundingMethod").Append('=').Append(JodaBeanUtils.ToString(compoundingMethod_Renamed)).Append(',').Append(' ');
		  buf.Append("firstRegularStartDate").Append('=').Append(JodaBeanUtils.ToString(firstRegularStartDate_Renamed)).Append(',').Append(' ');
		  buf.Append("lastRegularEndDate").Append('=').Append(JodaBeanUtils.ToString(lastRegularEndDate_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}