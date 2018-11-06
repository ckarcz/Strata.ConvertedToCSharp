using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A market convention for credit default swap trades.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableCdsConvention implements CdsConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableCdsConvention : CdsConvention, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final String name;
		private readonly string name;
	  /// <summary>
	  /// The currency of the CDS.
	  /// <para>
	  /// The amounts of the notional are expressed in terms of this currency.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
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
	  /// The periodic frequency of payments.
	  /// <para>
	  /// Regular payments will be made at the specified periodic frequency.
	  /// This also defines the accrual periodic frequency.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.Frequency paymentFrequency;
	  private readonly Frequency paymentFrequency;
	  /// <summary>
	  /// The business day adjustment to apply to payment schedule dates.
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
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;
	  /// <summary>
	  /// The business day adjustment to apply to the start date, optional with defaulting getter.
	  /// <para>
	  /// The start date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert the start date to a valid business day.
	  /// </para>
	  /// <para>
	  /// This will default to the {@code businessDayAdjustment} if not specified.
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
	  /// This will default to the 'None' if not specified.
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
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.StubConvention stubConvention;
	  private readonly StubConvention stubConvention;
	  /// <summary>
	  /// The convention defining how to roll dates, optional with defaulting getter.
	  /// <para>
	  /// The schedule periods are determined at the high level by repeatedly adding
	  /// the frequency to the start date, or subtracting it from the end date.
	  /// The roll convention provides the detailed rule to adjust the day-of-month or day-of-week.
	  /// </para>
	  /// <para>
	  /// This will default to 'Day20' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.RollConvention rollConvention;
	  private readonly RollConvention rollConvention;
	  /// <summary>
	  /// The payment on default.
	  /// <para>
	  /// Whether the accrued premium is paid in the event of a default.
	  /// </para>
	  /// <para>
	  /// This will default to 'accrued premium' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.credit.PaymentOnDefault paymentOnDefault;
	  private readonly PaymentOnDefault paymentOnDefault;
	  /// <summary>
	  /// The protection start of the day.
	  /// <para>
	  /// When the protection starts on the start date.
	  /// </para>
	  /// <para>
	  /// This will default to 'beginning of the start day' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.credit.ProtectionStartOfDay protectionStart;
	  private readonly ProtectionStartOfDay protectionStart;
	  /// <summary>
	  /// The number of days between valuation date and step-in date.
	  /// <para>
	  /// The step-in date is also called protection effective date. 
	  /// </para>
	  /// <para>
	  /// This will default to '1 calendar day' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment stepinDateOffset;
	  private readonly DaysAdjustment stepinDateOffset;
	  /// <summary>
	  /// The number of days between valuation date and settlement date.
	  /// <para>
	  /// It is usually 3 business days for standardised CDS contracts.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DaysAdjustment settlementDateOffset;
	  private readonly DaysAdjustment settlementDateOffset;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a convention based on the specified parameters.
	  /// </summary>
	  /// <param name="name">  the name of the convention </param>
	  /// <param name="currency">  the currency </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="paymentFrequency">  the payment frequency </param>
	  /// <param name="businessDayAdjustment">  the business day adjustment </param>
	  /// <param name="settlementDateOffset">  the settlement date offset </param>
	  /// <returns> the CDS convention </returns>
	  public static ImmutableCdsConvention of(string name, Currency currency, DayCount dayCount, Frequency paymentFrequency, BusinessDayAdjustment businessDayAdjustment, DaysAdjustment settlementDateOffset)
	  {

		return ImmutableCdsConvention.builder().name(name).currency(currency).dayCount(dayCount).paymentFrequency(paymentFrequency).businessDayAdjustment(businessDayAdjustment).settlementDateOffset(settlementDateOffset).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.stubConvention_Renamed == null)
		{
		  builder.stubConvention_Renamed = StubConvention.SMART_INITIAL;
		}
		if (builder.paymentOnDefault_Renamed == null)
		{
		  builder.paymentOnDefault_Renamed = PaymentOnDefault.ACCRUED_PREMIUM;
		}
		if (builder.rollConvention_Renamed == null)
		{
		  builder.rollConvention_Renamed = RollConventions.DAY_20;
		}
		if (builder.protectionStart_Renamed == null)
		{
		  builder.protectionStart_Renamed = ProtectionStartOfDay.BEGINNING;
		}
		if (builder.stepinDateOffset_Renamed == null)
		{
		  builder.stepinDateOffset_Renamed = DaysAdjustment.ofCalendarDays(1);
		}
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
	  /// This will default to the {@code businessDayAdjustment} if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date business day adjustment, not null </returns>
	  public BusinessDayAdjustment StartDateBusinessDayAdjustment
	  {
		  get
		  {
			return startDateBusinessDayAdjustment != null ? startDateBusinessDayAdjustment : businessDayAdjustment;
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
	  /// This will default to the 'None' if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date business day adjustment, not null </returns>
	  public BusinessDayAdjustment EndDateBusinessDayAdjustment
	  {
		  get
		  {
			return endDateBusinessDayAdjustment != null ? endDateBusinessDayAdjustment : BusinessDayAdjustment.NONE;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public CdsTrade toTrade(StandardId legalEntityId, TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double fixedRate)
	  {

		Cds product = Cds.builder().legalEntityId(legalEntityId).paymentSchedule(PeriodicSchedule.builder().startDate(startDate).endDate(endDate).frequency(paymentFrequency).businessDayAdjustment(businessDayAdjustment).startDateBusinessDayAdjustment(StartDateBusinessDayAdjustment).endDateBusinessDayAdjustment(EndDateBusinessDayAdjustment).stubConvention(stubConvention).rollConvention(rollConvention).build()).buySell(buySell).currency(currency).dayCount(dayCount).notional(notional).fixedRate(fixedRate).paymentOnDefault(paymentOnDefault).protectionStart(protectionStart).stepinDateOffset(stepinDateOffset).settlementDateOffset(settlementDateOffset).build();
		return CdsTrade.builder().info(tradeInfo).product(product).build();
	  }

	  public CdsTrade toTrade(StandardId legalEntityId, TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double fixedRate, AdjustablePayment upfrontFee)
	  {

		Cds product = Cds.builder().legalEntityId(legalEntityId).paymentSchedule(PeriodicSchedule.builder().startDate(startDate).endDate(endDate).frequency(paymentFrequency).businessDayAdjustment(businessDayAdjustment).startDateBusinessDayAdjustment(StartDateBusinessDayAdjustment).endDateBusinessDayAdjustment(EndDateBusinessDayAdjustment).stubConvention(stubConvention).rollConvention(rollConvention).build()).buySell(buySell).currency(currency).dayCount(dayCount).notional(notional).fixedRate(fixedRate).paymentOnDefault(paymentOnDefault).protectionStart(protectionStart).stepinDateOffset(stepinDateOffset).settlementDateOffset(settlementDateOffset).build();
		return CdsTrade.builder().info(tradeInfo).product(product).upfrontFee(upfrontFee).build();
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return Name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableCdsConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableCdsConvention.Meta meta()
	  {
		return ImmutableCdsConvention.Meta.INSTANCE;
	  }

	  static ImmutableCdsConvention()
	  {
		MetaBean.register(ImmutableCdsConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableCdsConvention.Builder builder()
	  {
		return new ImmutableCdsConvention.Builder();
	  }

	  private ImmutableCdsConvention(string name, Currency currency, DayCount dayCount, Frequency paymentFrequency, BusinessDayAdjustment businessDayAdjustment, BusinessDayAdjustment startDateBusinessDayAdjustment, BusinessDayAdjustment endDateBusinessDayAdjustment, StubConvention stubConvention, RollConvention rollConvention, PaymentOnDefault paymentOnDefault, ProtectionStartOfDay protectionStart, DaysAdjustment stepinDateOffset, DaysAdjustment settlementDateOffset)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(paymentFrequency, "paymentFrequency");
		JodaBeanUtils.notNull(businessDayAdjustment, "businessDayAdjustment");
		JodaBeanUtils.notNull(stubConvention, "stubConvention");
		JodaBeanUtils.notNull(rollConvention, "rollConvention");
		JodaBeanUtils.notNull(paymentOnDefault, "paymentOnDefault");
		JodaBeanUtils.notNull(protectionStart, "protectionStart");
		JodaBeanUtils.notNull(stepinDateOffset, "stepinDateOffset");
		JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		this.name = name;
		this.currency = currency;
		this.dayCount = dayCount;
		this.paymentFrequency = paymentFrequency;
		this.businessDayAdjustment = businessDayAdjustment;
		this.startDateBusinessDayAdjustment = startDateBusinessDayAdjustment;
		this.endDateBusinessDayAdjustment = endDateBusinessDayAdjustment;
		this.stubConvention = stubConvention;
		this.rollConvention = rollConvention;
		this.paymentOnDefault = paymentOnDefault;
		this.protectionStart = protectionStart;
		this.stepinDateOffset = stepinDateOffset;
		this.settlementDateOffset = settlementDateOffset;
	  }

	  public override ImmutableCdsConvention.Meta metaBean()
	  {
		return ImmutableCdsConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the convention name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the CDS.
	  /// <para>
	  /// The amounts of the notional are expressed in terms of this currency.
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
	  /// Gets the periodic frequency of payments.
	  /// <para>
	  /// Regular payments will be made at the specified periodic frequency.
	  /// This also defines the accrual periodic frequency.
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
	  /// Gets the business day adjustment to apply to payment schedule dates.
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
	  public BusinessDayAdjustment BusinessDayAdjustment
	  {
		  get
		  {
			return businessDayAdjustment;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the convention defining how to handle stubs, optional with defaulting getter.
	  /// <para>
	  /// The stub convention is used during schedule construction to determine whether the irregular
	  /// remaining period occurs at the start or end of the schedule.
	  /// It also determines whether the irregular period is shorter or longer than the regular period.
	  /// </para>
	  /// <para>
	  /// This will default to 'SmartInitial' if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public StubConvention StubConvention
	  {
		  get
		  {
			return stubConvention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the convention defining how to roll dates, optional with defaulting getter.
	  /// <para>
	  /// The schedule periods are determined at the high level by repeatedly adding
	  /// the frequency to the start date, or subtracting it from the end date.
	  /// The roll convention provides the detailed rule to adjust the day-of-month or day-of-week.
	  /// </para>
	  /// <para>
	  /// This will default to 'Day20' if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public RollConvention RollConvention
	  {
		  get
		  {
			return rollConvention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment on default.
	  /// <para>
	  /// Whether the accrued premium is paid in the event of a default.
	  /// </para>
	  /// <para>
	  /// This will default to 'accrued premium' if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PaymentOnDefault PaymentOnDefault
	  {
		  get
		  {
			return paymentOnDefault;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the protection start of the day.
	  /// <para>
	  /// When the protection starts on the start date.
	  /// </para>
	  /// <para>
	  /// This will default to 'beginning of the start day' if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ProtectionStartOfDay ProtectionStart
	  {
		  get
		  {
			return protectionStart;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of days between valuation date and step-in date.
	  /// <para>
	  /// The step-in date is also called protection effective date.
	  /// </para>
	  /// <para>
	  /// This will default to '1 calendar day' if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment StepinDateOffset
	  {
		  get
		  {
			return stepinDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of days between valuation date and settlement date.
	  /// <para>
	  /// It is usually 3 business days for standardised CDS contracts.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment SettlementDateOffset
	  {
		  get
		  {
			return settlementDateOffset;
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
		  ImmutableCdsConvention other = (ImmutableCdsConvention) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(paymentFrequency, other.paymentFrequency) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment) && JodaBeanUtils.equal(startDateBusinessDayAdjustment, other.startDateBusinessDayAdjustment) && JodaBeanUtils.equal(endDateBusinessDayAdjustment, other.endDateBusinessDayAdjustment) && JodaBeanUtils.equal(stubConvention, other.stubConvention) && JodaBeanUtils.equal(rollConvention, other.rollConvention) && JodaBeanUtils.equal(paymentOnDefault, other.paymentOnDefault) && JodaBeanUtils.equal(protectionStart, other.protectionStart) && JodaBeanUtils.equal(stepinDateOffset, other.stepinDateOffset) && JodaBeanUtils.equal(settlementDateOffset, other.settlementDateOffset);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentFrequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDateBusinessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDateBusinessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(stubConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rollConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentOnDefault);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(protectionStart);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(stepinDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementDateOffset);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableCdsConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableCdsConvention), typeof(string));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(ImmutableCdsConvention), typeof(Currency));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(ImmutableCdsConvention), typeof(DayCount));
			  paymentFrequency_Renamed = DirectMetaProperty.ofImmutable(this, "paymentFrequency", typeof(ImmutableCdsConvention), typeof(Frequency));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(ImmutableCdsConvention), typeof(BusinessDayAdjustment));
			  startDateBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "startDateBusinessDayAdjustment", typeof(ImmutableCdsConvention), typeof(BusinessDayAdjustment));
			  endDateBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "endDateBusinessDayAdjustment", typeof(ImmutableCdsConvention), typeof(BusinessDayAdjustment));
			  stubConvention_Renamed = DirectMetaProperty.ofImmutable(this, "stubConvention", typeof(ImmutableCdsConvention), typeof(StubConvention));
			  rollConvention_Renamed = DirectMetaProperty.ofImmutable(this, "rollConvention", typeof(ImmutableCdsConvention), typeof(RollConvention));
			  paymentOnDefault_Renamed = DirectMetaProperty.ofImmutable(this, "paymentOnDefault", typeof(ImmutableCdsConvention), typeof(PaymentOnDefault));
			  protectionStart_Renamed = DirectMetaProperty.ofImmutable(this, "protectionStart", typeof(ImmutableCdsConvention), typeof(ProtectionStartOfDay));
			  stepinDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "stepinDateOffset", typeof(ImmutableCdsConvention), typeof(DaysAdjustment));
			  settlementDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "settlementDateOffset", typeof(ImmutableCdsConvention), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "currency", "dayCount", "paymentFrequency", "businessDayAdjustment", "startDateBusinessDayAdjustment", "endDateBusinessDayAdjustment", "stubConvention", "rollConvention", "paymentOnDefault", "protectionStart", "stepinDateOffset", "settlementDateOffset");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> name_Renamed;
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
		/// The meta-property for the {@code paymentOnDefault} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PaymentOnDefault> paymentOnDefault_Renamed;
		/// <summary>
		/// The meta-property for the {@code protectionStart} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ProtectionStartOfDay> protectionStart_Renamed;
		/// <summary>
		/// The meta-property for the {@code stepinDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> stepinDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code settlementDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> settlementDateOffset_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "currency", "dayCount", "paymentFrequency", "businessDayAdjustment", "startDateBusinessDayAdjustment", "endDateBusinessDayAdjustment", "stubConvention", "rollConvention", "paymentOnDefault", "protectionStart", "stepinDateOffset", "settlementDateOffset");
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
			case 3373707: // name
			  return name_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 863656438: // paymentFrequency
			  return paymentFrequency_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 429197561: // startDateBusinessDayAdjustment
			  return startDateBusinessDayAdjustment_Renamed;
			case -734327136: // endDateBusinessDayAdjustment
			  return endDateBusinessDayAdjustment_Renamed;
			case -31408449: // stubConvention
			  return stubConvention_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			case -480203780: // paymentOnDefault
			  return paymentOnDefault_Renamed;
			case 2103482633: // protectionStart
			  return protectionStart_Renamed;
			case 852621746: // stepinDateOffset
			  return stepinDateOffset_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableCdsConvention.Builder builder()
		{
		  return new ImmutableCdsConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableCdsConvention);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> name()
		{
		  return name_Renamed;
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
		/// The meta-property for the {@code paymentOnDefault} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PaymentOnDefault> paymentOnDefault()
		{
		  return paymentOnDefault_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code protectionStart} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ProtectionStartOfDay> protectionStart()
		{
		  return protectionStart_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code stepinDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> stepinDateOffset()
		{
		  return stepinDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code settlementDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> settlementDateOffset()
		{
		  return settlementDateOffset_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((ImmutableCdsConvention) bean).Name;
			case 575402001: // currency
			  return ((ImmutableCdsConvention) bean).Currency;
			case 1905311443: // dayCount
			  return ((ImmutableCdsConvention) bean).DayCount;
			case 863656438: // paymentFrequency
			  return ((ImmutableCdsConvention) bean).PaymentFrequency;
			case -1065319863: // businessDayAdjustment
			  return ((ImmutableCdsConvention) bean).BusinessDayAdjustment;
			case 429197561: // startDateBusinessDayAdjustment
			  return ((ImmutableCdsConvention) bean).startDateBusinessDayAdjustment;
			case -734327136: // endDateBusinessDayAdjustment
			  return ((ImmutableCdsConvention) bean).endDateBusinessDayAdjustment;
			case -31408449: // stubConvention
			  return ((ImmutableCdsConvention) bean).StubConvention;
			case -10223666: // rollConvention
			  return ((ImmutableCdsConvention) bean).RollConvention;
			case -480203780: // paymentOnDefault
			  return ((ImmutableCdsConvention) bean).PaymentOnDefault;
			case 2103482633: // protectionStart
			  return ((ImmutableCdsConvention) bean).ProtectionStart;
			case 852621746: // stepinDateOffset
			  return ((ImmutableCdsConvention) bean).StepinDateOffset;
			case 135924714: // settlementDateOffset
			  return ((ImmutableCdsConvention) bean).SettlementDateOffset;
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
	  /// The bean-builder for {@code ImmutableCdsConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableCdsConvention>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency paymentFrequency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment startDateBusinessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment endDateBusinessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal StubConvention stubConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal RollConvention rollConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PaymentOnDefault paymentOnDefault_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ProtectionStartOfDay protectionStart_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment stepinDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment settlementDateOffset_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ImmutableCdsConvention beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.paymentFrequency_Renamed = beanToCopy.PaymentFrequency;
		  this.businessDayAdjustment_Renamed = beanToCopy.BusinessDayAdjustment;
		  this.startDateBusinessDayAdjustment_Renamed = beanToCopy.startDateBusinessDayAdjustment;
		  this.endDateBusinessDayAdjustment_Renamed = beanToCopy.endDateBusinessDayAdjustment;
		  this.stubConvention_Renamed = beanToCopy.StubConvention;
		  this.rollConvention_Renamed = beanToCopy.RollConvention;
		  this.paymentOnDefault_Renamed = beanToCopy.PaymentOnDefault;
		  this.protectionStart_Renamed = beanToCopy.ProtectionStart;
		  this.stepinDateOffset_Renamed = beanToCopy.StepinDateOffset;
		  this.settlementDateOffset_Renamed = beanToCopy.SettlementDateOffset;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 863656438: // paymentFrequency
			  return paymentFrequency_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 429197561: // startDateBusinessDayAdjustment
			  return startDateBusinessDayAdjustment_Renamed;
			case -734327136: // endDateBusinessDayAdjustment
			  return endDateBusinessDayAdjustment_Renamed;
			case -31408449: // stubConvention
			  return stubConvention_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			case -480203780: // paymentOnDefault
			  return paymentOnDefault_Renamed;
			case 2103482633: // protectionStart
			  return protectionStart_Renamed;
			case 852621746: // stepinDateOffset
			  return stepinDateOffset_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name_Renamed = (string) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 863656438: // paymentFrequency
			  this.paymentFrequency_Renamed = (Frequency) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
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
			case -480203780: // paymentOnDefault
			  this.paymentOnDefault_Renamed = (PaymentOnDefault) newValue;
			  break;
			case 2103482633: // protectionStart
			  this.protectionStart_Renamed = (ProtectionStartOfDay) newValue;
			  break;
			case 852621746: // stepinDateOffset
			  this.stepinDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 135924714: // settlementDateOffset
			  this.settlementDateOffset_Renamed = (DaysAdjustment) newValue;
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

		public override ImmutableCdsConvention build()
		{
		  preBuild(this);
		  return new ImmutableCdsConvention(name_Renamed, currency_Renamed, dayCount_Renamed, paymentFrequency_Renamed, businessDayAdjustment_Renamed, startDateBusinessDayAdjustment_Renamed, endDateBusinessDayAdjustment_Renamed, stubConvention_Renamed, rollConvention_Renamed, paymentOnDefault_Renamed, protectionStart_Renamed, stepinDateOffset_Renamed, settlementDateOffset_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the convention name. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the currency of the CDS.
		/// <para>
		/// The amounts of the notional are expressed in terms of this currency.
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
		/// Sets the periodic frequency of payments.
		/// <para>
		/// Regular payments will be made at the specified periodic frequency.
		/// This also defines the accrual periodic frequency.
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
		/// Sets the business day adjustment to apply to payment schedule dates.
		/// <para>
		/// Each date in the calculated schedule is determined without taking into account weekends and holidays.
		/// The adjustment specified here is used to convert those dates to valid business days.
		/// </para>
		/// <para>
		/// The start date and end date may have their own business day adjustment rules.
		/// If those are not present, then this adjustment is used instead.
		/// </para>
		/// </summary>
		/// <param name="businessDayAdjustment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder businessDayAdjustment(BusinessDayAdjustment businessDayAdjustment)
		{
		  JodaBeanUtils.notNull(businessDayAdjustment, "businessDayAdjustment");
		  this.businessDayAdjustment_Renamed = businessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to the start date, optional with defaulting getter.
		/// <para>
		/// The start date property is an unadjusted date and as such might be a weekend or holiday.
		/// The adjustment specified here is used to convert the start date to a valid business day.
		/// </para>
		/// <para>
		/// This will default to the {@code businessDayAdjustment} if not specified.
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
		/// This will default to the 'None' if not specified.
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
		/// <param name="stubConvention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder stubConvention(StubConvention stubConvention)
		{
		  JodaBeanUtils.notNull(stubConvention, "stubConvention");
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
		/// This will default to 'Day20' if not specified.
		/// </para>
		/// </summary>
		/// <param name="rollConvention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rollConvention(RollConvention rollConvention)
		{
		  JodaBeanUtils.notNull(rollConvention, "rollConvention");
		  this.rollConvention_Renamed = rollConvention;
		  return this;
		}

		/// <summary>
		/// Sets the payment on default.
		/// <para>
		/// Whether the accrued premium is paid in the event of a default.
		/// </para>
		/// <para>
		/// This will default to 'accrued premium' if not specified.
		/// </para>
		/// </summary>
		/// <param name="paymentOnDefault">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentOnDefault(PaymentOnDefault paymentOnDefault)
		{
		  JodaBeanUtils.notNull(paymentOnDefault, "paymentOnDefault");
		  this.paymentOnDefault_Renamed = paymentOnDefault;
		  return this;
		}

		/// <summary>
		/// Sets the protection start of the day.
		/// <para>
		/// When the protection starts on the start date.
		/// </para>
		/// <para>
		/// This will default to 'beginning of the start day' if not specified.
		/// </para>
		/// </summary>
		/// <param name="protectionStart">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder protectionStart(ProtectionStartOfDay protectionStart)
		{
		  JodaBeanUtils.notNull(protectionStart, "protectionStart");
		  this.protectionStart_Renamed = protectionStart;
		  return this;
		}

		/// <summary>
		/// Sets the number of days between valuation date and step-in date.
		/// <para>
		/// The step-in date is also called protection effective date.
		/// </para>
		/// <para>
		/// This will default to '1 calendar day' if not specified.
		/// </para>
		/// </summary>
		/// <param name="stepinDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder stepinDateOffset(DaysAdjustment stepinDateOffset)
		{
		  JodaBeanUtils.notNull(stepinDateOffset, "stepinDateOffset");
		  this.stepinDateOffset_Renamed = stepinDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the number of days between valuation date and settlement date.
		/// <para>
		/// It is usually 3 business days for standardised CDS contracts.
		/// </para>
		/// </summary>
		/// <param name="settlementDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder settlementDateOffset(DaysAdjustment settlementDateOffset)
		{
		  JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		  this.settlementDateOffset_Renamed = settlementDateOffset;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(448);
		  buf.Append("ImmutableCdsConvention.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentFrequency").Append('=').Append(JodaBeanUtils.ToString(paymentFrequency_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("startDateBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(startDateBusinessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("endDateBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(endDateBusinessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("stubConvention").Append('=').Append(JodaBeanUtils.ToString(stubConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("rollConvention").Append('=').Append(JodaBeanUtils.ToString(rollConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentOnDefault").Append('=').Append(JodaBeanUtils.ToString(paymentOnDefault_Renamed)).Append(',').Append(' ');
		  buf.Append("protectionStart").Append('=').Append(JodaBeanUtils.ToString(protectionStart_Renamed)).Append(',').Append(' ');
		  buf.Append("stepinDateOffset").Append('=').Append(JodaBeanUtils.ToString(stepinDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("settlementDateOffset").Append('=').Append(JodaBeanUtils.ToString(settlementDateOffset_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------

	}

}