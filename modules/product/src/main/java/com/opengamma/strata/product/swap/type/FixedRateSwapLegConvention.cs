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
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// A market convention for the fixed leg of rate swap trades.
	/// <para>
	/// This defines the market convention for a fixed leg in a specific currency.
	/// </para>
	/// <para>
	/// Some fields are mandatory, others are optional, providing the ability to override
	/// The getters will default any missing information on the fly, avoiding both null and <seealso cref="Optional"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FixedRateSwapLegConvention implements SwapLegConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FixedRateSwapLegConvention : SwapLegConvention, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
		private readonly Currency currency;
	  /// <summary>
	  /// The day count convention applicable.
	  /// <para>
	  /// This is used to convert schedule period dates to a numerical value.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The periodic frequency of accrual.
	  /// <para>
	  /// Interest will be accrued over periods at the specified periodic frequency, such as every 3 months.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.Frequency accrualFrequency;
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
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.BusinessDayAdjustment accrualBusinessDayAdjustment;
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

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a convention based on the specified parameters.
	  /// <para>
	  /// The standard market convention for a fixed rate leg is based on these parameters,
	  /// with the stub convention set to 'SmartInitial'.
	  /// Use the <seealso cref="#builder() builder"/> for unusual conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency of the leg </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="accrualFrequency">  the accrual frequency </param>
	  /// <param name="accrualBusinessDayAdjustment">  the accrual business day adjustment </param>
	  /// <returns> the convention </returns>
	  public static FixedRateSwapLegConvention of(Currency currency, DayCount dayCount, Frequency accrualFrequency, BusinessDayAdjustment accrualBusinessDayAdjustment)
	  {

		return FixedRateSwapLegConvention.builder().currency(currency).dayCount(dayCount).accrualFrequency(accrualFrequency).accrualBusinessDayAdjustment(accrualBusinessDayAdjustment).stubConvention(StubConvention.SMART_INITIAL).build();
	  }

	  //-------------------------------------------------------------------------
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
			return startDateBusinessDayAdjustment != null ? startDateBusinessDayAdjustment : accrualBusinessDayAdjustment;
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
			return endDateBusinessDayAdjustment != null ? endDateBusinessDayAdjustment : accrualBusinessDayAdjustment;
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
			return paymentFrequency != null ? paymentFrequency : accrualFrequency;
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
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <returns> the leg </returns>
	  public RateCalculationSwapLeg toLeg(LocalDate startDate, LocalDate endDate, PayReceive payReceive, double notional, double fixedRate)
	  {

		return RateCalculationSwapLeg.builder().payReceive(payReceive).accrualSchedule(PeriodicSchedule.builder().startDate(startDate).endDate(endDate).frequency(accrualFrequency).businessDayAdjustment(accrualBusinessDayAdjustment).startDateBusinessDayAdjustment(startDateBusinessDayAdjustment).endDateBusinessDayAdjustment(endDateBusinessDayAdjustment).stubConvention(stubConvention).rollConvention(rollConvention).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(PaymentFrequency).paymentDateOffset(PaymentDateOffset).compoundingMethod(CompoundingMethod).build()).notionalSchedule(NotionalSchedule.of(currency, notional)).calculation(FixedRateCalculation.of(fixedRate, dayCount)).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedRateSwapLegConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FixedRateSwapLegConvention.Meta meta()
	  {
		return FixedRateSwapLegConvention.Meta.INSTANCE;
	  }

	  static FixedRateSwapLegConvention()
	  {
		MetaBean.register(FixedRateSwapLegConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FixedRateSwapLegConvention.Builder builder()
	  {
		return new FixedRateSwapLegConvention.Builder();
	  }

	  private FixedRateSwapLegConvention(Currency currency, DayCount dayCount, Frequency accrualFrequency, BusinessDayAdjustment accrualBusinessDayAdjustment, BusinessDayAdjustment startDateBusinessDayAdjustment, BusinessDayAdjustment endDateBusinessDayAdjustment, StubConvention stubConvention, RollConvention rollConvention, Frequency paymentFrequency, DaysAdjustment paymentDateOffset, CompoundingMethod compoundingMethod)
	  {
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(accrualFrequency, "accrualFrequency");
		JodaBeanUtils.notNull(accrualBusinessDayAdjustment, "accrualBusinessDayAdjustment");
		this.currency = currency;
		this.dayCount = dayCount;
		this.accrualFrequency = accrualFrequency;
		this.accrualBusinessDayAdjustment = accrualBusinessDayAdjustment;
		this.startDateBusinessDayAdjustment = startDateBusinessDayAdjustment;
		this.endDateBusinessDayAdjustment = endDateBusinessDayAdjustment;
		this.stubConvention = stubConvention;
		this.rollConvention = rollConvention;
		this.paymentFrequency = paymentFrequency;
		this.paymentDateOffset = paymentDateOffset;
		this.compoundingMethod = compoundingMethod;
	  }

	  public override FixedRateSwapLegConvention.Meta metaBean()
	  {
		return FixedRateSwapLegConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the leg currency.
	  /// <para>
	  /// This is the currency of the swap leg and the currency that payment is made in.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention applicable.
	  /// <para>
	  /// This is used to convert schedule period dates to a numerical value.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the periodic frequency of accrual.
	  /// <para>
	  /// Interest will be accrued over periods at the specified periodic frequency, such as every 3 months.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Frequency AccrualFrequency
	  {
		  get
		  {
			return accrualFrequency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the business day adjustment to apply to accrual schedule dates.
	  /// <para>
	  /// Each date in the calculated schedule is determined without taking into account weekends and holidays.
	  /// The adjustment specified here is used to convert those dates to valid business days.
	  /// </para>
	  /// <para>
	  /// The start date and end date may have their own business day adjustment rules.
	  /// If those are not present, then this adjustment is used instead.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BusinessDayAdjustment AccrualBusinessDayAdjustment
	  {
		  get
		  {
			return accrualBusinessDayAdjustment;
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
		  FixedRateSwapLegConvention other = (FixedRateSwapLegConvention) obj;
		  return JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(accrualFrequency, other.accrualFrequency) && JodaBeanUtils.equal(accrualBusinessDayAdjustment, other.accrualBusinessDayAdjustment) && JodaBeanUtils.equal(startDateBusinessDayAdjustment, other.startDateBusinessDayAdjustment) && JodaBeanUtils.equal(endDateBusinessDayAdjustment, other.endDateBusinessDayAdjustment) && JodaBeanUtils.equal(stubConvention, other.stubConvention) && JodaBeanUtils.equal(rollConvention, other.rollConvention) && JodaBeanUtils.equal(paymentFrequency, other.paymentFrequency) && JodaBeanUtils.equal(paymentDateOffset, other.paymentDateOffset) && JodaBeanUtils.equal(compoundingMethod, other.compoundingMethod);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualFrequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualBusinessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDateBusinessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDateBusinessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(stubConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rollConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentFrequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(compoundingMethod);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(384);
		buf.Append("FixedRateSwapLegConvention{");
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("accrualFrequency").Append('=').Append(accrualFrequency).Append(',').Append(' ');
		buf.Append("accrualBusinessDayAdjustment").Append('=').Append(accrualBusinessDayAdjustment).Append(',').Append(' ');
		buf.Append("startDateBusinessDayAdjustment").Append('=').Append(startDateBusinessDayAdjustment).Append(',').Append(' ');
		buf.Append("endDateBusinessDayAdjustment").Append('=').Append(endDateBusinessDayAdjustment).Append(',').Append(' ');
		buf.Append("stubConvention").Append('=').Append(stubConvention).Append(',').Append(' ');
		buf.Append("rollConvention").Append('=').Append(rollConvention).Append(',').Append(' ');
		buf.Append("paymentFrequency").Append('=').Append(paymentFrequency).Append(',').Append(' ');
		buf.Append("paymentDateOffset").Append('=').Append(paymentDateOffset).Append(',').Append(' ');
		buf.Append("compoundingMethod").Append('=').Append(JodaBeanUtils.ToString(compoundingMethod));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedRateSwapLegConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(FixedRateSwapLegConvention), typeof(Currency));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(FixedRateSwapLegConvention), typeof(DayCount));
			  accrualFrequency_Renamed = DirectMetaProperty.ofImmutable(this, "accrualFrequency", typeof(FixedRateSwapLegConvention), typeof(Frequency));
			  accrualBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "accrualBusinessDayAdjustment", typeof(FixedRateSwapLegConvention), typeof(BusinessDayAdjustment));
			  startDateBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "startDateBusinessDayAdjustment", typeof(FixedRateSwapLegConvention), typeof(BusinessDayAdjustment));
			  endDateBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "endDateBusinessDayAdjustment", typeof(FixedRateSwapLegConvention), typeof(BusinessDayAdjustment));
			  stubConvention_Renamed = DirectMetaProperty.ofImmutable(this, "stubConvention", typeof(FixedRateSwapLegConvention), typeof(StubConvention));
			  rollConvention_Renamed = DirectMetaProperty.ofImmutable(this, "rollConvention", typeof(FixedRateSwapLegConvention), typeof(RollConvention));
			  paymentFrequency_Renamed = DirectMetaProperty.ofImmutable(this, "paymentFrequency", typeof(FixedRateSwapLegConvention), typeof(Frequency));
			  paymentDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDateOffset", typeof(FixedRateSwapLegConvention), typeof(DaysAdjustment));
			  compoundingMethod_Renamed = DirectMetaProperty.ofImmutable(this, "compoundingMethod", typeof(FixedRateSwapLegConvention), typeof(CompoundingMethod));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currency", "dayCount", "accrualFrequency", "accrualBusinessDayAdjustment", "startDateBusinessDayAdjustment", "endDateBusinessDayAdjustment", "stubConvention", "rollConvention", "paymentFrequency", "paymentDateOffset", "compoundingMethod");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

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
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currency", "dayCount", "accrualFrequency", "accrualBusinessDayAdjustment", "startDateBusinessDayAdjustment", "endDateBusinessDayAdjustment", "stubConvention", "rollConvention", "paymentFrequency", "paymentDateOffset", "compoundingMethod");
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
			case 863656438: // paymentFrequency
			  return paymentFrequency_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case -1376171496: // compoundingMethod
			  return compoundingMethod_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FixedRateSwapLegConvention.Builder builder()
		{
		  return new FixedRateSwapLegConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FixedRateSwapLegConvention);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
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

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return ((FixedRateSwapLegConvention) bean).Currency;
			case 1905311443: // dayCount
			  return ((FixedRateSwapLegConvention) bean).DayCount;
			case 945206381: // accrualFrequency
			  return ((FixedRateSwapLegConvention) bean).AccrualFrequency;
			case 896049114: // accrualBusinessDayAdjustment
			  return ((FixedRateSwapLegConvention) bean).AccrualBusinessDayAdjustment;
			case 429197561: // startDateBusinessDayAdjustment
			  return ((FixedRateSwapLegConvention) bean).startDateBusinessDayAdjustment;
			case -734327136: // endDateBusinessDayAdjustment
			  return ((FixedRateSwapLegConvention) bean).endDateBusinessDayAdjustment;
			case -31408449: // stubConvention
			  return ((FixedRateSwapLegConvention) bean).stubConvention;
			case -10223666: // rollConvention
			  return ((FixedRateSwapLegConvention) bean).rollConvention;
			case 863656438: // paymentFrequency
			  return ((FixedRateSwapLegConvention) bean).paymentFrequency;
			case -716438393: // paymentDateOffset
			  return ((FixedRateSwapLegConvention) bean).paymentDateOffset;
			case -1376171496: // compoundingMethod
			  return ((FixedRateSwapLegConvention) bean).compoundingMethod;
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
	  /// The bean-builder for {@code FixedRateSwapLegConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FixedRateSwapLegConvention>
	  {

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
		internal Frequency paymentFrequency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment paymentDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CompoundingMethod compoundingMethod_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(FixedRateSwapLegConvention beanToCopy)
		{
		  this.currency_Renamed = beanToCopy.Currency;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.accrualFrequency_Renamed = beanToCopy.AccrualFrequency;
		  this.accrualBusinessDayAdjustment_Renamed = beanToCopy.AccrualBusinessDayAdjustment;
		  this.startDateBusinessDayAdjustment_Renamed = beanToCopy.startDateBusinessDayAdjustment;
		  this.endDateBusinessDayAdjustment_Renamed = beanToCopy.endDateBusinessDayAdjustment;
		  this.stubConvention_Renamed = beanToCopy.stubConvention;
		  this.rollConvention_Renamed = beanToCopy.rollConvention;
		  this.paymentFrequency_Renamed = beanToCopy.paymentFrequency;
		  this.paymentDateOffset_Renamed = beanToCopy.paymentDateOffset;
		  this.compoundingMethod_Renamed = beanToCopy.compoundingMethod;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
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
			case 863656438: // paymentFrequency
			  return paymentFrequency_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case -1376171496: // compoundingMethod
			  return compoundingMethod_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
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
			case 863656438: // paymentFrequency
			  this.paymentFrequency_Renamed = (Frequency) newValue;
			  break;
			case -716438393: // paymentDateOffset
			  this.paymentDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case -1376171496: // compoundingMethod
			  this.compoundingMethod_Renamed = (CompoundingMethod) newValue;
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

		public override FixedRateSwapLegConvention build()
		{
		  return new FixedRateSwapLegConvention(currency_Renamed, dayCount_Renamed, accrualFrequency_Renamed, accrualBusinessDayAdjustment_Renamed, startDateBusinessDayAdjustment_Renamed, endDateBusinessDayAdjustment_Renamed, stubConvention_Renamed, rollConvention_Renamed, paymentFrequency_Renamed, paymentDateOffset_Renamed, compoundingMethod_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the leg currency.
		/// <para>
		/// This is the currency of the swap leg and the currency that payment is made in.
		/// </para>
		/// </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention applicable.
		/// <para>
		/// This is used to convert schedule period dates to a numerical value.
		/// </para>
		/// </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the periodic frequency of accrual.
		/// <para>
		/// Interest will be accrued over periods at the specified periodic frequency, such as every 3 months.
		/// </para>
		/// </summary>
		/// <param name="accrualFrequency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualFrequency(Frequency accrualFrequency)
		{
		  JodaBeanUtils.notNull(accrualFrequency, "accrualFrequency");
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
		/// </summary>
		/// <param name="accrualBusinessDayAdjustment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualBusinessDayAdjustment(BusinessDayAdjustment accrualBusinessDayAdjustment)
		{
		  JodaBeanUtils.notNull(accrualBusinessDayAdjustment, "accrualBusinessDayAdjustment");
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(384);
		  buf.Append("FixedRateSwapLegConvention.Builder{");
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualFrequency").Append('=').Append(JodaBeanUtils.ToString(accrualFrequency_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(accrualBusinessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("startDateBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(startDateBusinessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("endDateBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(endDateBusinessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("stubConvention").Append('=').Append(JodaBeanUtils.ToString(stubConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("rollConvention").Append('=').Append(JodaBeanUtils.ToString(rollConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentFrequency").Append('=').Append(JodaBeanUtils.ToString(paymentFrequency_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDateOffset").Append('=').Append(JodaBeanUtils.ToString(paymentDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("compoundingMethod").Append('=').Append(JodaBeanUtils.ToString(compoundingMethod_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}