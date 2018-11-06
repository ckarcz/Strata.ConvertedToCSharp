using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// A market convention for the floating leg of rate swap trades based on an Ibor index.
	/// <para>
	/// This defines the market convention for a floating leg based on the observed value
	/// of an Ibor index such as 'GBP-LIBOR-3M' or 'EUR-EURIBOR-1M'.
	/// In most cases, the index contains sufficient information to fully define the convention.
	/// As such, no other fields need to be specified when creating an instance.
	/// The getters will default any missing information on the fly, avoiding both null and <seealso cref="Optional"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class IborRateSwapLegConvention implements SwapLegConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborRateSwapLegConvention : SwapLegConvention, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.IborIndex index;
		private readonly IborIndex index;

	  /// <summary>
	  /// The leg currency, optional with defaulting getter.
	  /// <para>
	  /// This is the currency of the swap leg and the currency that payment is made in.
	  /// The data model permits this currency to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the currency of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The day count convention applicable, optional with defaulting getter.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// The data model permits the day count to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the day count of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The periodic frequency of accrual.
	  /// <para>
	  /// Interest will be accrued over periods at the specified periodic frequency, such as every 3 months.
	  /// </para>
	  /// <para>
	  /// This will default to the tenor of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.schedule.Frequency accrualFrequency;
	  private readonly Frequency accrualFrequency;
	  /// <summary>
	  /// The business day adjustment to apply to accrual schedule dates.
	  /// <para>
	  /// Each date in the calculated schedule is determined without taking into account weekends and holidays.
	  /// The adjustment specified here is used to convert those dates to valid business days.
	  /// </para>
	  /// <para>
	  /// The start date and end date may have their own business day adjustment rules.
	  /// If those are not present, then this adjustment is used instead.
	  /// </para>
	  /// <para>
	  /// This will default to 'ModifiedFollowing' using the index fixing calendar if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.BusinessDayAdjustment accrualBusinessDayAdjustment;
	  private readonly BusinessDayAdjustment accrualBusinessDayAdjustment;
	  /// <summary>
	  /// The business day adjustment to apply to the start date, optional with defaulting getter.
	  /// <para>
	  /// The start date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert the start date to a valid business day.
	  /// </para>
	  /// <para>
	  /// This will default to the {@code accrualDatesBusinessDayAdjustment} if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.BusinessDayAdjustment startDateBusinessDayAdjustment;
	  private readonly BusinessDayAdjustment startDateBusinessDayAdjustment;
	  /// <summary>
	  /// The business day adjustment to apply to the end date, optional with defaulting getter.
	  /// <para>
	  /// The end date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert the end date to a valid business day.
	  /// </para>
	  /// <para>
	  /// This will default to the {@code accrualDatesBusinessDayAdjustment} if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.BusinessDayAdjustment endDateBusinessDayAdjustment;
	  private readonly BusinessDayAdjustment endDateBusinessDayAdjustment;
	  /// <summary>
	  /// The convention defining how to handle stubs, optional with defaulting getter.
	  /// <para>
	  /// The stub convention is used during schedule construction to determine whether the irregular
	  /// remaining period occurs at the start or end of the schedule.
	  /// It also determines whether the irregular period is shorter or longer than the regular period.
	  /// </para>
	  /// <para>
	  /// This will default to 'SmartInitial' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.schedule.StubConvention stubConvention;
	  private readonly StubConvention stubConvention;
	  /// <summary>
	  /// The convention defining how to roll dates, optional with defaulting getter.
	  /// <para>
	  /// The schedule periods are determined at the high level by repeatedly adding
	  /// the frequency to the start date, or subtracting it from the end date.
	  /// The roll convention provides the detailed rule to adjust the day-of-month or day-of-week.
	  /// </para>
	  /// <para>
	  /// This will default to 'None' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.schedule.RollConvention rollConvention;
	  private readonly RollConvention rollConvention;
	  /// <summary>
	  /// The base date that each fixing is made relative to, optional with defaulting getter.
	  /// <para>
	  /// The fixing date is relative to either the start or end of each reset period.
	  /// </para>
	  /// <para>
	  /// Note that in most cases, the reset frequency matches the accrual frequency
	  /// and thus there is only one fixing for the accrual period.
	  /// </para>
	  /// <para>
	  /// This will default to 'PeriodStart' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.product.swap.FixingRelativeTo fixingRelativeTo;
	  private readonly FixingRelativeTo fixingRelativeTo;
	  /// <summary>
	  /// The offset of the fixing date from each adjusted reset date.
	  /// <para>
	  /// The offset is applied to the base date specified by {@code fixingRelativeTo}.
	  /// The offset is typically a negative number of business days.
	  /// The data model permits the offset to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the fixing date offset of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.DaysAdjustment fixingDateOffset;
	  private readonly DaysAdjustment fixingDateOffset;
	  /// <summary>
	  /// The periodic frequency of payments, optional with defaulting getter.
	  /// <para>
	  /// Regular payments will be made at the specified periodic frequency.
	  /// The frequency must be the same as, or a multiple of, the accrual periodic frequency.
	  /// </para>
	  /// <para>
	  /// Compounding applies if the payment frequency does not equal the accrual frequency.
	  /// </para>
	  /// <para>
	  /// This will default to the accrual frequency if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.schedule.Frequency paymentFrequency;
	  private readonly Frequency paymentFrequency;
	  /// <summary>
	  /// The offset of payment from the base date, optional with defaulting getter.
	  /// <para>
	  /// The offset is applied to the unadjusted date specified by {@code paymentRelativeTo}.
	  /// Offset can be based on calendar days or business days.
	  /// </para>
	  /// <para>
	  /// This will default to 'None' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.DaysAdjustment paymentDateOffset;
	  private readonly DaysAdjustment paymentDateOffset;
	  /// <summary>
	  /// The compounding method to use when there is more than one accrual period
	  /// in each payment period, optional with defaulting getter.
	  /// <para>
	  /// Compounding is used when combining accrual periods.
	  /// </para>
	  /// <para>
	  /// This will default to 'None' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.product.swap.CompoundingMethod compoundingMethod;
	  private readonly CompoundingMethod compoundingMethod;
	  /// <summary>
	  /// The flag indicating whether to exchange the notional.
	  /// <para>
	  /// If 'true', the notional there is both an initial exchange and a final exchange of notional.
	  /// </para>
	  /// <para>
	  /// This will default to 'false' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final boolean notionalExchange;
	  private readonly bool notionalExchange;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a convention based on the specified index.
	  /// <para>
	  /// The standard market convention for an Ibor rate leg is based on the index,
	  /// with the stub convention set to 'SmartInitial'.
	  /// Use the <seealso cref="#builder() builder"/> for unusual conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index, the market convention values are extracted from the index </param>
	  /// <returns> the convention </returns>
	  public static IborRateSwapLegConvention of(IborIndex index)
	  {
		return IborRateSwapLegConvention.builder().index(index).stubConvention(StubConvention.SMART_INITIAL).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the leg currency, optional with defaulting getter.
	  /// <para>
	  /// This is the currency of the swap leg and the currency that payment is made in.
	  /// The data model permits this currency to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the currency of the index if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date business day adjustment, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency != null ? currency : index.Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the day count convention applicable,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// The data model permits the day count to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the day count of the index if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the day count, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount != null ? dayCount : index.DayCount;
		  }
	  }

	  /// <summary>
	  /// Gets the periodic frequency of accrual.
	  /// <para>
	  /// Interest will be accrued over periods at the specified periodic frequency, such as every 3 months.
	  /// </para>
	  /// <para>
	  /// This will default to the tenor of the index if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the accrual frequency, not null </returns>
	  public Frequency AccrualFrequency
	  {
		  get
		  {
			return accrualFrequency != null ? accrualFrequency : Frequency.of(index.Tenor.Period);
		  }
	  }

	  /// <summary>
	  /// Gets the business day adjustment to apply to accrual schedule dates,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// Each date in the calculated schedule is determined without taking into account weekends and holidays.
	  /// The adjustment specified here is used to convert those dates to valid business days.
	  /// The start date and end date have their own business day adjustment rules.
	  /// </para>
	  /// <para>
	  /// This will default to 'ModifiedFollowing' using the index fixing calendar if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the business day adjustment, not null </returns>
	  public BusinessDayAdjustment AccrualBusinessDayAdjustment
	  {
		  get
		  {
			return accrualBusinessDayAdjustment != null ? accrualBusinessDayAdjustment : BusinessDayAdjustment.of(MODIFIED_FOLLOWING, index.FixingCalendar);
		  }
	  }

	  /// <summary>
	  /// Gets the business day adjustment to apply to the start date,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// The start date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert the start date to a valid business day.
	  /// </para>
	  /// <para>
	  /// This will default to the {@code accrualDatesBusinessDayAdjustment} if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date business day adjustment, not null </returns>
	  public BusinessDayAdjustment StartDateBusinessDayAdjustment
	  {
		  get
		  {
			return startDateBusinessDayAdjustment != null ? startDateBusinessDayAdjustment : AccrualBusinessDayAdjustment;
		  }
	  }

	  /// <summary>
	  /// Gets the business day adjustment to apply to the end date,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// The end date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert the end date to a valid business day.
	  /// </para>
	  /// <para>
	  /// This will default to the {@code accrualDatesBusinessDayAdjustment} if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date business day adjustment, not null </returns>
	  public BusinessDayAdjustment EndDateBusinessDayAdjustment
	  {
		  get
		  {
			return endDateBusinessDayAdjustment != null ? endDateBusinessDayAdjustment : AccrualBusinessDayAdjustment;
		  }
	  }

	  /// <summary>
	  /// Gets the convention defining how to handle stubs,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// The stub convention is used during schedule construction to determine whether the irregular
	  /// remaining period occurs at the start or end of the schedule.
	  /// It also determines whether the irregular period is shorter or longer than the regular period.
	  /// </para>
	  /// <para>
	  /// This will default to 'SmartInitial' if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the stub convention, not null </returns>
	  public StubConvention StubConvention
	  {
		  get
		  {
			return stubConvention != null ? stubConvention : StubConvention.SMART_INITIAL;
		  }
	  }

	  /// <summary>
	  /// Gets the convention defining how to roll dates,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// The schedule periods are determined at the high level by repeatedly adding
	  /// the frequency to the start date, or subtracting it from the end date.
	  /// The roll convention provides the detailed rule to adjust the day-of-month or day-of-week.
	  /// </para>
	  /// <para>
	  /// This will default to 'EOM' if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the roll convention, not null </returns>
	  public RollConvention RollConvention
	  {
		  get
		  {
			return rollConvention != null ? rollConvention : RollConventions.EOM;
		  }
	  }

	  /// <summary>
	  /// Gets the base date that each fixing is made relative to, optional with defaulting getter.
	  /// <para>
	  /// The fixing date is relative to either the start or end of each reset period.
	  /// </para>
	  /// <para>
	  /// Note that in most cases, the reset frequency matches the accrual frequency
	  /// and thus there is only one fixing for the accrual period.
	  /// </para>
	  /// <para>
	  /// This will default to 'PeriodStart' if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the fixing relative to, not null </returns>
	  public FixingRelativeTo FixingRelativeTo
	  {
		  get
		  {
			return fixingRelativeTo != null ? fixingRelativeTo : FixingRelativeTo.PERIOD_START;
		  }
	  }

	  /// <summary>
	  /// The offset of the fixing date from each adjusted reset date,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// The offset is applied to the base date specified by {@code fixingRelativeTo}.
	  /// The offset is typically a negative number of business days.
	  /// The data model permits the offset to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the fixing date offset of the index if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the fixing date offset, not null </returns>
	  public DaysAdjustment FixingDateOffset
	  {
		  get
		  {
			return fixingDateOffset != null ? fixingDateOffset : index.FixingDateOffset;
		  }
	  }

	  /// <summary>
	  /// Gets the periodic frequency of payments,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// Regular payments will be made at the specified periodic frequency.
	  /// The frequency must be the same as, or a multiple of, the accrual periodic frequency.
	  /// </para>
	  /// <para>
	  /// Compounding applies if the payment frequency does not equal the accrual frequency.
	  /// </para>
	  /// <para>
	  /// This will default to the accrual frequency if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the payment frequency, not null </returns>
	  public Frequency PaymentFrequency
	  {
		  get
		  {
			return paymentFrequency != null ? paymentFrequency : AccrualFrequency;
		  }
	  }

	  /// <summary>
	  /// Gets the offset of payment from the base date,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// The offset is applied to the unadjusted date specified by {@code paymentRelativeTo}.
	  /// Offset can be based on calendar days or business days.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the payment date offset, not null </returns>
	  public DaysAdjustment PaymentDateOffset
	  {
		  get
		  {
			return paymentDateOffset != null ? paymentDateOffset : DaysAdjustment.NONE;
		  }
	  }

	  /// <summary>
	  /// Gets the compounding method to use when there is more than one accrual period
	  /// in each payment period, providing a default result if no override specified.
	  /// <para>
	  /// Compounding is used when combining accrual periods.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the compounding method, not null </returns>
	  public CompoundingMethod CompoundingMethod
	  {
		  get
		  {
			return compoundingMethod != null ? compoundingMethod : CompoundingMethod.NONE;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a leg based on this convention.
	  /// <para>
	  /// This returns a leg based on the specified date.
	  /// The notional is unsigned, with pay/receive determining the direction of the leg.
	  /// If the leg is 'Pay', the fixed rate is paid to the counterparty.
	  /// If the leg is 'Receive', the fixed rate is received from the counterparty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="payReceive">  determines if the leg is to be paid or received </param>
	  /// <param name="notional">  the notional </param>
	  /// <returns> the leg </returns>
	  public RateCalculationSwapLeg toLeg(LocalDate startDate, LocalDate endDate, PayReceive payReceive, double notional)
	  {

		return toLeg(startDate, endDate, payReceive, notional, 0d);
	  }

	  /// <summary>
	  /// Creates a leg based on this convention.
	  /// <para>
	  /// This returns a leg based on the specified date.
	  /// The notional is unsigned, with pay/receive determining the direction of the leg.
	  /// If the leg is 'Pay', the fixed rate is paid to the counterparty.
	  /// If the leg is 'Receive', the fixed rate is received from the counterparty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="payReceive">  determines if the leg is to be paid or received </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="spread">  the spread to apply </param>
	  /// <returns> the leg </returns>
	  public RateCalculationSwapLeg toLeg(LocalDate startDate, LocalDate endDate, PayReceive payReceive, double notional, double spread)
	  {

		return RateCalculationSwapLeg.builder().payReceive(payReceive).accrualSchedule(PeriodicSchedule.builder().startDate(startDate).endDate(endDate).frequency(AccrualFrequency).businessDayAdjustment(AccrualBusinessDayAdjustment).startDateBusinessDayAdjustment(startDateBusinessDayAdjustment).endDateBusinessDayAdjustment(endDateBusinessDayAdjustment).stubConvention(stubConvention).rollConvention(rollConvention).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(PaymentFrequency).paymentDateOffset(PaymentDateOffset).compoundingMethod(CompoundingMethod).build()).notionalSchedule(NotionalSchedule.builder().currency(Currency).finalExchange(notionalExchange).initialExchange(notionalExchange).amount(ValueSchedule.of(notional)).build()).calculation(IborRateCalculation.builder().index(index).dayCount(DayCount).fixingRelativeTo(FixingRelativeTo).fixingDateOffset(FixingDateOffset).spread(spread != 0 ? ValueSchedule.of(spread) : null).build()).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborRateSwapLegConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborRateSwapLegConvention.Meta meta()
	  {
		return IborRateSwapLegConvention.Meta.INSTANCE;
	  }

	  static IborRateSwapLegConvention()
	  {
		MetaBean.register(IborRateSwapLegConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static IborRateSwapLegConvention.Builder builder()
	  {
		return new IborRateSwapLegConvention.Builder();
	  }

	  private IborRateSwapLegConvention(IborIndex index, Currency currency, DayCount dayCount, Frequency accrualFrequency, BusinessDayAdjustment accrualBusinessDayAdjustment, BusinessDayAdjustment startDateBusinessDayAdjustment, BusinessDayAdjustment endDateBusinessDayAdjustment, StubConvention stubConvention, RollConvention rollConvention, FixingRelativeTo fixingRelativeTo, DaysAdjustment fixingDateOffset, Frequency paymentFrequency, DaysAdjustment paymentDateOffset, CompoundingMethod compoundingMethod, bool notionalExchange)
	  {
		JodaBeanUtils.notNull(index, "index");
		this.index = index;
		this.currency = currency;
		this.dayCount = dayCount;
		this.accrualFrequency = accrualFrequency;
		this.accrualBusinessDayAdjustment = accrualBusinessDayAdjustment;
		this.startDateBusinessDayAdjustment = startDateBusinessDayAdjustment;
		this.endDateBusinessDayAdjustment = endDateBusinessDayAdjustment;
		this.stubConvention = stubConvention;
		this.rollConvention = rollConvention;
		this.fixingRelativeTo = fixingRelativeTo;
		this.fixingDateOffset = fixingDateOffset;
		this.paymentFrequency = paymentFrequency;
		this.paymentDateOffset = paymentDateOffset;
		this.compoundingMethod = compoundingMethod;
		this.notionalExchange = notionalExchange;
	  }

	  public override IborRateSwapLegConvention.Meta metaBean()
	  {
		return IborRateSwapLegConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index.
	  /// <para>
	  /// The floating rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the flag indicating whether to exchange the notional.
	  /// <para>
	  /// If 'true', the notional there is both an initial exchange and a final exchange of notional.
	  /// </para>
	  /// <para>
	  /// This will default to 'false' if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public bool NotionalExchange
	  {
		  get
		  {
			return notionalExchange;
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
		  IborRateSwapLegConvention other = (IborRateSwapLegConvention) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(accrualFrequency, other.accrualFrequency) && JodaBeanUtils.equal(accrualBusinessDayAdjustment, other.accrualBusinessDayAdjustment) && JodaBeanUtils.equal(startDateBusinessDayAdjustment, other.startDateBusinessDayAdjustment) && JodaBeanUtils.equal(endDateBusinessDayAdjustment, other.endDateBusinessDayAdjustment) && JodaBeanUtils.equal(stubConvention, other.stubConvention) && JodaBeanUtils.equal(rollConvention, other.rollConvention) && JodaBeanUtils.equal(fixingRelativeTo, other.fixingRelativeTo) && JodaBeanUtils.equal(fixingDateOffset, other.fixingDateOffset) && JodaBeanUtils.equal(paymentFrequency, other.paymentFrequency) && JodaBeanUtils.equal(paymentDateOffset, other.paymentDateOffset) && JodaBeanUtils.equal(compoundingMethod, other.compoundingMethod) && (notionalExchange == other.notionalExchange);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualFrequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualBusinessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDateBusinessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDateBusinessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(stubConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rollConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingRelativeTo);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentFrequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(compoundingMethod);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notionalExchange);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(512);
		buf.Append("IborRateSwapLegConvention{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("accrualFrequency").Append('=').Append(accrualFrequency).Append(',').Append(' ');
		buf.Append("accrualBusinessDayAdjustment").Append('=').Append(accrualBusinessDayAdjustment).Append(',').Append(' ');
		buf.Append("startDateBusinessDayAdjustment").Append('=').Append(startDateBusinessDayAdjustment).Append(',').Append(' ');
		buf.Append("endDateBusinessDayAdjustment").Append('=').Append(endDateBusinessDayAdjustment).Append(',').Append(' ');
		buf.Append("stubConvention").Append('=').Append(stubConvention).Append(',').Append(' ');
		buf.Append("rollConvention").Append('=').Append(rollConvention).Append(',').Append(' ');
		buf.Append("fixingRelativeTo").Append('=').Append(fixingRelativeTo).Append(',').Append(' ');
		buf.Append("fixingDateOffset").Append('=').Append(fixingDateOffset).Append(',').Append(' ');
		buf.Append("paymentFrequency").Append('=').Append(paymentFrequency).Append(',').Append(' ');
		buf.Append("paymentDateOffset").Append('=').Append(paymentDateOffset).Append(',').Append(' ');
		buf.Append("compoundingMethod").Append('=').Append(compoundingMethod).Append(',').Append(' ');
		buf.Append("notionalExchange").Append('=').Append(JodaBeanUtils.ToString(notionalExchange));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborRateSwapLegConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(IborRateSwapLegConvention), typeof(IborIndex));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(IborRateSwapLegConvention), typeof(Currency));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(IborRateSwapLegConvention), typeof(DayCount));
			  accrualFrequency_Renamed = DirectMetaProperty.ofImmutable(this, "accrualFrequency", typeof(IborRateSwapLegConvention), typeof(Frequency));
			  accrualBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "accrualBusinessDayAdjustment", typeof(IborRateSwapLegConvention), typeof(BusinessDayAdjustment));
			  startDateBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "startDateBusinessDayAdjustment", typeof(IborRateSwapLegConvention), typeof(BusinessDayAdjustment));
			  endDateBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "endDateBusinessDayAdjustment", typeof(IborRateSwapLegConvention), typeof(BusinessDayAdjustment));
			  stubConvention_Renamed = DirectMetaProperty.ofImmutable(this, "stubConvention", typeof(IborRateSwapLegConvention), typeof(StubConvention));
			  rollConvention_Renamed = DirectMetaProperty.ofImmutable(this, "rollConvention", typeof(IborRateSwapLegConvention), typeof(RollConvention));
			  fixingRelativeTo_Renamed = DirectMetaProperty.ofImmutable(this, "fixingRelativeTo", typeof(IborRateSwapLegConvention), typeof(FixingRelativeTo));
			  fixingDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDateOffset", typeof(IborRateSwapLegConvention), typeof(DaysAdjustment));
			  paymentFrequency_Renamed = DirectMetaProperty.ofImmutable(this, "paymentFrequency", typeof(IborRateSwapLegConvention), typeof(Frequency));
			  paymentDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDateOffset", typeof(IborRateSwapLegConvention), typeof(DaysAdjustment));
			  compoundingMethod_Renamed = DirectMetaProperty.ofImmutable(this, "compoundingMethod", typeof(IborRateSwapLegConvention), typeof(CompoundingMethod));
			  notionalExchange_Renamed = DirectMetaProperty.ofImmutable(this, "notionalExchange", typeof(IborRateSwapLegConvention), Boolean.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "currency", "dayCount", "accrualFrequency", "accrualBusinessDayAdjustment", "startDateBusinessDayAdjustment", "endDateBusinessDayAdjustment", "stubConvention", "rollConvention", "fixingRelativeTo", "fixingDateOffset", "paymentFrequency", "paymentDateOffset", "compoundingMethod", "notionalExchange");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code accrualFrequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> accrualFrequency_Renamed;
		/// <summary>
		/// The meta-property for the {@code accrualBusinessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> accrualBusinessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDateBusinessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> startDateBusinessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDateBusinessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> endDateBusinessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code stubConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<StubConvention> stubConvention_Renamed;
		/// <summary>
		/// The meta-property for the {@code rollConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<RollConvention> rollConvention_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingRelativeTo} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixingRelativeTo> fixingRelativeTo_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> fixingDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentFrequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> paymentFrequency_Renamed;
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
		/// The meta-property for the {@code notionalExchange} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> notionalExchange_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "currency", "dayCount", "accrualFrequency", "accrualBusinessDayAdjustment", "startDateBusinessDayAdjustment", "endDateBusinessDayAdjustment", "stubConvention", "rollConvention", "fixingRelativeTo", "fixingDateOffset", "paymentFrequency", "paymentDateOffset", "compoundingMethod", "notionalExchange");
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
			case 100346066: // index
			  return index_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 945206381: // accrualFrequency
			  return accrualFrequency_Renamed;
			case 896049114: // accrualBusinessDayAdjustment
			  return accrualBusinessDayAdjustment_Renamed;
			case 429197561: // startDateBusinessDayAdjustment
			  return startDateBusinessDayAdjustment_Renamed;
			case -734327136: // endDateBusinessDayAdjustment
			  return endDateBusinessDayAdjustment_Renamed;
			case -31408449: // stubConvention
			  return stubConvention_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			case 232554996: // fixingRelativeTo
			  return fixingRelativeTo_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 863656438: // paymentFrequency
			  return paymentFrequency_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case -1376171496: // compoundingMethod
			  return compoundingMethod_Renamed;
			case -159410813: // notionalExchange
			  return notionalExchange_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override IborRateSwapLegConvention.Builder builder()
		{
		  return new IborRateSwapLegConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborRateSwapLegConvention);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code accrualFrequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> accrualFrequency()
		{
		  return accrualFrequency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code accrualBusinessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> accrualBusinessDayAdjustment()
		{
		  return accrualBusinessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDateBusinessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> startDateBusinessDayAdjustment()
		{
		  return startDateBusinessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDateBusinessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> endDateBusinessDayAdjustment()
		{
		  return endDateBusinessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code stubConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<StubConvention> stubConvention()
		{
		  return stubConvention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rollConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<RollConvention> rollConvention()
		{
		  return rollConvention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingRelativeTo} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixingRelativeTo> fixingRelativeTo()
		{
		  return fixingRelativeTo_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> fixingDateOffset()
		{
		  return fixingDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentFrequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> paymentFrequency()
		{
		  return paymentFrequency_Renamed;
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
		/// The meta-property for the {@code notionalExchange} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> notionalExchange()
		{
		  return notionalExchange_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((IborRateSwapLegConvention) bean).Index;
			case 575402001: // currency
			  return ((IborRateSwapLegConvention) bean).currency;
			case 1905311443: // dayCount
			  return ((IborRateSwapLegConvention) bean).dayCount;
			case 945206381: // accrualFrequency
			  return ((IborRateSwapLegConvention) bean).accrualFrequency;
			case 896049114: // accrualBusinessDayAdjustment
			  return ((IborRateSwapLegConvention) bean).accrualBusinessDayAdjustment;
			case 429197561: // startDateBusinessDayAdjustment
			  return ((IborRateSwapLegConvention) bean).startDateBusinessDayAdjustment;
			case -734327136: // endDateBusinessDayAdjustment
			  return ((IborRateSwapLegConvention) bean).endDateBusinessDayAdjustment;
			case -31408449: // stubConvention
			  return ((IborRateSwapLegConvention) bean).stubConvention;
			case -10223666: // rollConvention
			  return ((IborRateSwapLegConvention) bean).rollConvention;
			case 232554996: // fixingRelativeTo
			  return ((IborRateSwapLegConvention) bean).fixingRelativeTo;
			case 873743726: // fixingDateOffset
			  return ((IborRateSwapLegConvention) bean).fixingDateOffset;
			case 863656438: // paymentFrequency
			  return ((IborRateSwapLegConvention) bean).paymentFrequency;
			case -716438393: // paymentDateOffset
			  return ((IborRateSwapLegConvention) bean).paymentDateOffset;
			case -1376171496: // compoundingMethod
			  return ((IborRateSwapLegConvention) bean).compoundingMethod;
			case -159410813: // notionalExchange
			  return ((IborRateSwapLegConvention) bean).NotionalExchange;
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
	  /// The bean-builder for {@code IborRateSwapLegConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<IborRateSwapLegConvention>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency accrualFrequency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment accrualBusinessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment startDateBusinessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment endDateBusinessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal StubConvention stubConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal RollConvention rollConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixingRelativeTo fixingRelativeTo_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment fixingDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency paymentFrequency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment paymentDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CompoundingMethod compoundingMethod_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool notionalExchange_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(IborRateSwapLegConvention beanToCopy)
		{
		  this.index_Renamed = beanToCopy.Index;
		  this.currency_Renamed = beanToCopy.currency;
		  this.dayCount_Renamed = beanToCopy.dayCount;
		  this.accrualFrequency_Renamed = beanToCopy.accrualFrequency;
		  this.accrualBusinessDayAdjustment_Renamed = beanToCopy.accrualBusinessDayAdjustment;
		  this.startDateBusinessDayAdjustment_Renamed = beanToCopy.startDateBusinessDayAdjustment;
		  this.endDateBusinessDayAdjustment_Renamed = beanToCopy.endDateBusinessDayAdjustment;
		  this.stubConvention_Renamed = beanToCopy.stubConvention;
		  this.rollConvention_Renamed = beanToCopy.rollConvention;
		  this.fixingRelativeTo_Renamed = beanToCopy.fixingRelativeTo;
		  this.fixingDateOffset_Renamed = beanToCopy.fixingDateOffset;
		  this.paymentFrequency_Renamed = beanToCopy.paymentFrequency;
		  this.paymentDateOffset_Renamed = beanToCopy.paymentDateOffset;
		  this.compoundingMethod_Renamed = beanToCopy.compoundingMethod;
		  this.notionalExchange_Renamed = beanToCopy.NotionalExchange;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return index_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 945206381: // accrualFrequency
			  return accrualFrequency_Renamed;
			case 896049114: // accrualBusinessDayAdjustment
			  return accrualBusinessDayAdjustment_Renamed;
			case 429197561: // startDateBusinessDayAdjustment
			  return startDateBusinessDayAdjustment_Renamed;
			case -734327136: // endDateBusinessDayAdjustment
			  return endDateBusinessDayAdjustment_Renamed;
			case -31408449: // stubConvention
			  return stubConvention_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			case 232554996: // fixingRelativeTo
			  return fixingRelativeTo_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 863656438: // paymentFrequency
			  return paymentFrequency_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case -1376171496: // compoundingMethod
			  return compoundingMethod_Renamed;
			case -159410813: // notionalExchange
			  return notionalExchange_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index_Renamed = (IborIndex) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 945206381: // accrualFrequency
			  this.accrualFrequency_Renamed = (Frequency) newValue;
			  break;
			case 896049114: // accrualBusinessDayAdjustment
			  this.accrualBusinessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case 429197561: // startDateBusinessDayAdjustment
			  this.startDateBusinessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case -734327136: // endDateBusinessDayAdjustment
			  this.endDateBusinessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case -31408449: // stubConvention
			  this.stubConvention_Renamed = (StubConvention) newValue;
			  break;
			case -10223666: // rollConvention
			  this.rollConvention_Renamed = (RollConvention) newValue;
			  break;
			case 232554996: // fixingRelativeTo
			  this.fixingRelativeTo_Renamed = (FixingRelativeTo) newValue;
			  break;
			case 873743726: // fixingDateOffset
			  this.fixingDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 863656438: // paymentFrequency
			  this.paymentFrequency_Renamed = (Frequency) newValue;
			  break;
			case -716438393: // paymentDateOffset
			  this.paymentDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case -1376171496: // compoundingMethod
			  this.compoundingMethod_Renamed = (CompoundingMethod) newValue;
			  break;
			case -159410813: // notionalExchange
			  this.notionalExchange_Renamed = (bool?) newValue.Value;
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

		public override IborRateSwapLegConvention build()
		{
		  return new IborRateSwapLegConvention(index_Renamed, currency_Renamed, dayCount_Renamed, accrualFrequency_Renamed, accrualBusinessDayAdjustment_Renamed, startDateBusinessDayAdjustment_Renamed, endDateBusinessDayAdjustment_Renamed, stubConvention_Renamed, rollConvention_Renamed, fixingRelativeTo_Renamed, fixingDateOffset_Renamed, paymentFrequency_Renamed, paymentDateOffset_Renamed, compoundingMethod_Renamed, notionalExchange_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the Ibor index.
		/// <para>
		/// The floating rate to be paid is based on this index
		/// It will be a well known market index such as 'GBP-LIBOR-3M'.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(IborIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the leg currency, optional with defaulting getter.
		/// <para>
		/// This is the currency of the swap leg and the currency that payment is made in.
		/// The data model permits this currency to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// <para>
		/// This will default to the currency of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="currency">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention applicable, optional with defaulting getter.
		/// <para>
		/// This is used to convert dates to a numerical value.
		/// The data model permits the day count to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// <para>
		/// This will default to the day count of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="dayCount">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the periodic frequency of accrual.
		/// <para>
		/// Interest will be accrued over periods at the specified periodic frequency, such as every 3 months.
		/// </para>
		/// <para>
		/// This will default to the tenor of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="accrualFrequency">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualFrequency(Frequency accrualFrequency)
		{
		  this.accrualFrequency_Renamed = accrualFrequency;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to accrual schedule dates.
		/// <para>
		/// Each date in the calculated schedule is determined without taking into account weekends and holidays.
		/// The adjustment specified here is used to convert those dates to valid business days.
		/// </para>
		/// <para>
		/// The start date and end date may have their own business day adjustment rules.
		/// If those are not present, then this adjustment is used instead.
		/// </para>
		/// <para>
		/// This will default to 'ModifiedFollowing' using the index fixing calendar if not specified.
		/// </para>
		/// </summary>
		/// <param name="accrualBusinessDayAdjustment">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualBusinessDayAdjustment(BusinessDayAdjustment accrualBusinessDayAdjustment)
		{
		  this.accrualBusinessDayAdjustment_Renamed = accrualBusinessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to the start date, optional with defaulting getter.
		/// <para>
		/// The start date property is an unadjusted date and as such might be a weekend or holiday.
		/// The adjustment specified here is used to convert the start date to a valid business day.
		/// </para>
		/// <para>
		/// This will default to the {@code accrualDatesBusinessDayAdjustment} if not specified.
		/// </para>
		/// </summary>
		/// <param name="startDateBusinessDayAdjustment">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDateBusinessDayAdjustment(BusinessDayAdjustment startDateBusinessDayAdjustment)
		{
		  this.startDateBusinessDayAdjustment_Renamed = startDateBusinessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to the end date, optional with defaulting getter.
		/// <para>
		/// The end date property is an unadjusted date and as such might be a weekend or holiday.
		/// The adjustment specified here is used to convert the end date to a valid business day.
		/// </para>
		/// <para>
		/// This will default to the {@code accrualDatesBusinessDayAdjustment} if not specified.
		/// </para>
		/// </summary>
		/// <param name="endDateBusinessDayAdjustment">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDateBusinessDayAdjustment(BusinessDayAdjustment endDateBusinessDayAdjustment)
		{
		  this.endDateBusinessDayAdjustment_Renamed = endDateBusinessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the convention defining how to handle stubs, optional with defaulting getter.
		/// <para>
		/// The stub convention is used during schedule construction to determine whether the irregular
		/// remaining period occurs at the start or end of the schedule.
		/// It also determines whether the irregular period is shorter or longer than the regular period.
		/// </para>
		/// <para>
		/// This will default to 'SmartInitial' if not specified.
		/// </para>
		/// </summary>
		/// <param name="stubConvention">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder stubConvention(StubConvention stubConvention)
		{
		  this.stubConvention_Renamed = stubConvention;
		  return this;
		}

		/// <summary>
		/// Sets the convention defining how to roll dates, optional with defaulting getter.
		/// <para>
		/// The schedule periods are determined at the high level by repeatedly adding
		/// the frequency to the start date, or subtracting it from the end date.
		/// The roll convention provides the detailed rule to adjust the day-of-month or day-of-week.
		/// </para>
		/// <para>
		/// This will default to 'None' if not specified.
		/// </para>
		/// </summary>
		/// <param name="rollConvention">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rollConvention(RollConvention rollConvention)
		{
		  this.rollConvention_Renamed = rollConvention;
		  return this;
		}

		/// <summary>
		/// Sets the base date that each fixing is made relative to, optional with defaulting getter.
		/// <para>
		/// The fixing date is relative to either the start or end of each reset period.
		/// </para>
		/// <para>
		/// Note that in most cases, the reset frequency matches the accrual frequency
		/// and thus there is only one fixing for the accrual period.
		/// </para>
		/// <para>
		/// This will default to 'PeriodStart' if not specified.
		/// </para>
		/// </summary>
		/// <param name="fixingRelativeTo">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingRelativeTo(FixingRelativeTo fixingRelativeTo)
		{
		  this.fixingRelativeTo_Renamed = fixingRelativeTo;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the fixing date from each adjusted reset date.
		/// <para>
		/// The offset is applied to the base date specified by {@code fixingRelativeTo}.
		/// The offset is typically a negative number of business days.
		/// The data model permits the offset to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// <para>
		/// This will default to the fixing date offset of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="fixingDateOffset">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingDateOffset(DaysAdjustment fixingDateOffset)
		{
		  this.fixingDateOffset_Renamed = fixingDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the periodic frequency of payments, optional with defaulting getter.
		/// <para>
		/// Regular payments will be made at the specified periodic frequency.
		/// The frequency must be the same as, or a multiple of, the accrual periodic frequency.
		/// </para>
		/// <para>
		/// Compounding applies if the payment frequency does not equal the accrual frequency.
		/// </para>
		/// <para>
		/// This will default to the accrual frequency if not specified.
		/// </para>
		/// </summary>
		/// <param name="paymentFrequency">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentFrequency(Frequency paymentFrequency)
		{
		  this.paymentFrequency_Renamed = paymentFrequency;
		  return this;
		}

		/// <summary>
		/// Sets the offset of payment from the base date, optional with defaulting getter.
		/// <para>
		/// The offset is applied to the unadjusted date specified by {@code paymentRelativeTo}.
		/// Offset can be based on calendar days or business days.
		/// </para>
		/// <para>
		/// This will default to 'None' if not specified.
		/// </para>
		/// </summary>
		/// <param name="paymentDateOffset">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentDateOffset(DaysAdjustment paymentDateOffset)
		{
		  this.paymentDateOffset_Renamed = paymentDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the compounding method to use when there is more than one accrual period
		/// in each payment period, optional with defaulting getter.
		/// <para>
		/// Compounding is used when combining accrual periods.
		/// </para>
		/// <para>
		/// This will default to 'None' if not specified.
		/// </para>
		/// </summary>
		/// <param name="compoundingMethod">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder compoundingMethod(CompoundingMethod compoundingMethod)
		{
		  this.compoundingMethod_Renamed = compoundingMethod;
		  return this;
		}

		/// <summary>
		/// Sets the flag indicating whether to exchange the notional.
		/// <para>
		/// If 'true', the notional there is both an initial exchange and a final exchange of notional.
		/// </para>
		/// <para>
		/// This will default to 'false' if not specified.
		/// </para>
		/// </summary>
		/// <param name="notionalExchange">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notionalExchange(bool notionalExchange)
		{
		  this.notionalExchange_Renamed = notionalExchange;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(512);
		  buf.Append("IborRateSwapLegConvention.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualFrequency").Append('=').Append(JodaBeanUtils.ToString(accrualFrequency_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(accrualBusinessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("startDateBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(startDateBusinessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("endDateBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(endDateBusinessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("stubConvention").Append('=').Append(JodaBeanUtils.ToString(stubConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("rollConvention").Append('=').Append(JodaBeanUtils.ToString(rollConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingRelativeTo").Append('=').Append(JodaBeanUtils.ToString(fixingRelativeTo_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDateOffset").Append('=').Append(JodaBeanUtils.ToString(fixingDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentFrequency").Append('=').Append(JodaBeanUtils.ToString(paymentFrequency_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDateOffset").Append('=').Append(JodaBeanUtils.ToString(paymentDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("compoundingMethod").Append('=').Append(JodaBeanUtils.ToString(compoundingMethod_Renamed)).Append(',').Append(' ');
		  buf.Append("notionalExchange").Append('=').Append(JodaBeanUtils.ToString(notionalExchange_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}