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
	using DerivedProperty = org.joda.beans.gen.DerivedProperty;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Index = com.opengamma.strata.basics.index.Index;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// A rate swap leg defined using a parameterized schedule and calculation.
	/// <para>
	/// This defines a single swap leg paying a rate, such as an interest rate.
	/// The rate may be fixed or floating, see <seealso cref="FixedRateCalculation"/>,
	/// <seealso cref="IborRateCalculation"/> and <seealso cref="OvernightRateCalculation"/>.
	/// </para>
	/// <para>
	/// Interest is calculated based on <i>accrual periods</i> which follow a regular schedule
	/// with optional initial and final stubs. Coupon payments are based on <i>payment periods</i>
	/// which are typically the same as the accrual periods.
	/// If the payment period is longer than the accrual period then compounding may apply.
	/// The schedule of periods is defined using <seealso cref="PeriodicSchedule"/>, <seealso cref="PaymentSchedule"/>,
	/// <seealso cref="NotionalSchedule"/> and <seealso cref="ResetSchedule"/>.
	/// </para>
	/// <para>
	/// If the schedule needs to be manually specified, or there are other unusual calculation
	/// rules then the <seealso cref="RatePeriodSwapLeg"/> class should be used instead.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class RateCalculationSwapLeg implements SwapLeg, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RateCalculationSwapLeg : SwapLeg, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.common.PayReceive payReceive;
		private readonly PayReceive payReceive;
	  /// <summary>
	  /// The accrual schedule.
	  /// <para>
	  /// This is used to define the accrual periods.
	  /// These are used directly or indirectly to determine other dates in the swap.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.PeriodicSchedule accrualSchedule;
	  private readonly PeriodicSchedule accrualSchedule;
	  /// <summary>
	  /// The payment schedule.
	  /// <para>
	  /// This is used to define the payment periods, including any compounding.
	  /// The payment period dates are based on the accrual schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final PaymentSchedule paymentSchedule;
	  private readonly PaymentSchedule paymentSchedule;
	  /// <summary>
	  /// The notional schedule.
	  /// <para>
	  /// The notional amount schedule, which can vary during the lifetime of the swap.
	  /// In most cases, the notional amount is not exchanged, with only the net difference being exchanged.
	  /// However, in certain cases, initial, final or intermediate amounts are exchanged.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final NotionalSchedule notionalSchedule;
	  private readonly NotionalSchedule notionalSchedule;
	  /// <summary>
	  /// The interest rate accrual calculation.
	  /// <para>
	  /// Different kinds of swap leg are determined by the subclass used here.
	  /// See <seealso cref="FixedRateCalculation"/>, <seealso cref="IborRateCalculation"/> and <seealso cref="OvernightRateCalculation"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final RateCalculation calculation;
	  private readonly RateCalculation calculation;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public SwapLegType getType()
	  public SwapLegType Type
	  {
		  get
		  {
			return calculation.Type;
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public com.opengamma.strata.basics.date.AdjustableDate getStartDate()
	  public AdjustableDate StartDate
	  {
		  get
		  {
			return accrualSchedule.calculatedStartDate();
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public com.opengamma.strata.basics.date.AdjustableDate getEndDate()
	  public AdjustableDate EndDate
	  {
		  get
		  {
			return accrualSchedule.calculatedEndDate();
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public com.opengamma.strata.basics.currency.Currency getCurrency()
	  public Currency Currency
	  {
		  get
		  {
			return notionalSchedule.Currency;
		  }
	  }

	  public void collectCurrencies(ImmutableSet.Builder<Currency> builder)
	  {
		builder.add(Currency);
		calculation.collectCurrencies(builder);
		notionalSchedule.FxReset.ifPresent(fxReset => builder.add(fxReset.ReferenceCurrency));
	  }

	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		calculation.collectIndices(builder);
		notionalSchedule.FxReset.ifPresent(fxReset => builder.add(fxReset.Index));
	  }

	  /// <summary>
	  /// Converts this swap leg to the equivalent {@code ResolvedSwapLeg}.
	  /// <para>
	  /// An <seealso cref="ResolvedSwapLeg"/> represents the same data as this leg, but with
	  /// a complete schedule of dates defined using <seealso cref="RatePaymentPeriod"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the equivalent resolved swap leg </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid swap schedule or definition </exception>
	  public ResolvedSwapLeg resolve(ReferenceData refData)
	  {
		DayCount dayCount = calculation.DayCount;
		Schedule resolvedAccruals = accrualSchedule.createSchedule(refData);
		Schedule resolvedPayments = paymentSchedule.createSchedule(resolvedAccruals, refData);
		IList<RateAccrualPeriod> accrualPeriods = calculation.createAccrualPeriods(resolvedAccruals, resolvedPayments, refData);
		IList<NotionalPaymentPeriod> payPeriods = paymentSchedule.createPaymentPeriods(resolvedAccruals, resolvedPayments, accrualPeriods, dayCount, notionalSchedule, payReceive, refData);
		LocalDate startDate = accrualPeriods[0].StartDate;
		ImmutableList<SwapPaymentEvent> payEvents = notionalSchedule.createEvents(payPeriods, startDate, refData);
		return new ResolvedSwapLeg(Type, payReceive, payPeriods, payEvents, Currency);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RateCalculationSwapLeg}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RateCalculationSwapLeg.Meta meta()
	  {
		return RateCalculationSwapLeg.Meta.INSTANCE;
	  }

	  static RateCalculationSwapLeg()
	  {
		MetaBean.register(RateCalculationSwapLeg.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static RateCalculationSwapLeg.Builder builder()
	  {
		return new RateCalculationSwapLeg.Builder();
	  }

	  private RateCalculationSwapLeg(PayReceive payReceive, PeriodicSchedule accrualSchedule, PaymentSchedule paymentSchedule, NotionalSchedule notionalSchedule, RateCalculation calculation)
	  {
		JodaBeanUtils.notNull(payReceive, "payReceive");
		JodaBeanUtils.notNull(accrualSchedule, "accrualSchedule");
		JodaBeanUtils.notNull(paymentSchedule, "paymentSchedule");
		JodaBeanUtils.notNull(notionalSchedule, "notionalSchedule");
		JodaBeanUtils.notNull(calculation, "calculation");
		this.payReceive = payReceive;
		this.accrualSchedule = accrualSchedule;
		this.paymentSchedule = paymentSchedule;
		this.notionalSchedule = notionalSchedule;
		this.calculation = calculation;
	  }

	  public override RateCalculationSwapLeg.Meta metaBean()
	  {
		return RateCalculationSwapLeg.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the leg is pay or receive.
	  /// <para>
	  /// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
	  /// A value of 'Receive' implies that the resulting amount is received from the counterparty.
	  /// Note that negative interest rates can result in a payment in the opposite
	  /// direction to that implied by this indicator.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PayReceive PayReceive
	  {
		  get
		  {
			return payReceive;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual schedule.
	  /// <para>
	  /// This is used to define the accrual periods.
	  /// These are used directly or indirectly to determine other dates in the swap.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PeriodicSchedule AccrualSchedule
	  {
		  get
		  {
			return accrualSchedule;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment schedule.
	  /// <para>
	  /// This is used to define the payment periods, including any compounding.
	  /// The payment period dates are based on the accrual schedule.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PaymentSchedule PaymentSchedule
	  {
		  get
		  {
			return paymentSchedule;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional schedule.
	  /// <para>
	  /// The notional amount schedule, which can vary during the lifetime of the swap.
	  /// In most cases, the notional amount is not exchanged, with only the net difference being exchanged.
	  /// However, in certain cases, initial, final or intermediate amounts are exchanged.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public NotionalSchedule NotionalSchedule
	  {
		  get
		  {
			return notionalSchedule;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the interest rate accrual calculation.
	  /// <para>
	  /// Different kinds of swap leg are determined by the subclass used here.
	  /// See <seealso cref="FixedRateCalculation"/>, <seealso cref="IborRateCalculation"/> and <seealso cref="OvernightRateCalculation"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public RateCalculation Calculation
	  {
		  get
		  {
			return calculation;
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
		  RateCalculationSwapLeg other = (RateCalculationSwapLeg) obj;
		  return JodaBeanUtils.equal(payReceive, other.payReceive) && JodaBeanUtils.equal(accrualSchedule, other.accrualSchedule) && JodaBeanUtils.equal(paymentSchedule, other.paymentSchedule) && JodaBeanUtils.equal(notionalSchedule, other.notionalSchedule) && JodaBeanUtils.equal(calculation, other.calculation);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payReceive);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualSchedule);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentSchedule);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notionalSchedule);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(calculation);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("RateCalculationSwapLeg{");
		buf.Append("payReceive").Append('=').Append(payReceive).Append(',').Append(' ');
		buf.Append("accrualSchedule").Append('=').Append(accrualSchedule).Append(',').Append(' ');
		buf.Append("paymentSchedule").Append('=').Append(paymentSchedule).Append(',').Append(' ');
		buf.Append("notionalSchedule").Append('=').Append(notionalSchedule).Append(',').Append(' ');
		buf.Append("calculation").Append('=').Append(JodaBeanUtils.ToString(calculation));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RateCalculationSwapLeg}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  payReceive_Renamed = DirectMetaProperty.ofImmutable(this, "payReceive", typeof(RateCalculationSwapLeg), typeof(PayReceive));
			  accrualSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "accrualSchedule", typeof(RateCalculationSwapLeg), typeof(PeriodicSchedule));
			  paymentSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "paymentSchedule", typeof(RateCalculationSwapLeg), typeof(PaymentSchedule));
			  notionalSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "notionalSchedule", typeof(RateCalculationSwapLeg), typeof(NotionalSchedule));
			  calculation_Renamed = DirectMetaProperty.ofImmutable(this, "calculation", typeof(RateCalculationSwapLeg), typeof(RateCalculation));
			  type_Renamed = DirectMetaProperty.ofDerived(this, "type", typeof(RateCalculationSwapLeg), typeof(SwapLegType));
			  startDate_Renamed = DirectMetaProperty.ofDerived(this, "startDate", typeof(RateCalculationSwapLeg), typeof(AdjustableDate));
			  endDate_Renamed = DirectMetaProperty.ofDerived(this, "endDate", typeof(RateCalculationSwapLeg), typeof(AdjustableDate));
			  currency_Renamed = DirectMetaProperty.ofDerived(this, "currency", typeof(RateCalculationSwapLeg), typeof(Currency));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "payReceive", "accrualSchedule", "paymentSchedule", "notionalSchedule", "calculation", "type", "startDate", "endDate", "currency");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code payReceive} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PayReceive> payReceive_Renamed;
		/// <summary>
		/// The meta-property for the {@code accrualSchedule} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PeriodicSchedule> accrualSchedule_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentSchedule} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PaymentSchedule> paymentSchedule_Renamed;
		/// <summary>
		/// The meta-property for the {@code notionalSchedule} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<NotionalSchedule> notionalSchedule_Renamed;
		/// <summary>
		/// The meta-property for the {@code calculation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<RateCalculation> calculation_Renamed;
		/// <summary>
		/// The meta-property for the {@code type} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwapLegType> type_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustableDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustableDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "payReceive", "accrualSchedule", "paymentSchedule", "notionalSchedule", "calculation", "type", "startDate", "endDate", "currency");
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
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case 304659814: // accrualSchedule
			  return accrualSchedule_Renamed;
			case -1499086147: // paymentSchedule
			  return paymentSchedule_Renamed;
			case 1447860727: // notionalSchedule
			  return notionalSchedule_Renamed;
			case -934682935: // calculation
			  return calculation_Renamed;
			case 3575610: // type
			  return type_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override RateCalculationSwapLeg.Builder builder()
		{
		  return new RateCalculationSwapLeg.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RateCalculationSwapLeg);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code payReceive} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PayReceive> payReceive()
		{
		  return payReceive_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code accrualSchedule} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PeriodicSchedule> accrualSchedule()
		{
		  return accrualSchedule_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentSchedule} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PaymentSchedule> paymentSchedule()
		{
		  return paymentSchedule_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code notionalSchedule} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<NotionalSchedule> notionalSchedule()
		{
		  return notionalSchedule_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code calculation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<RateCalculation> calculation()
		{
		  return calculation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code type} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwapLegType> type()
		{
		  return type_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustableDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustableDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  return ((RateCalculationSwapLeg) bean).PayReceive;
			case 304659814: // accrualSchedule
			  return ((RateCalculationSwapLeg) bean).AccrualSchedule;
			case -1499086147: // paymentSchedule
			  return ((RateCalculationSwapLeg) bean).PaymentSchedule;
			case 1447860727: // notionalSchedule
			  return ((RateCalculationSwapLeg) bean).NotionalSchedule;
			case -934682935: // calculation
			  return ((RateCalculationSwapLeg) bean).Calculation;
			case 3575610: // type
			  return ((RateCalculationSwapLeg) bean).Type;
			case -2129778896: // startDate
			  return ((RateCalculationSwapLeg) bean).StartDate;
			case -1607727319: // endDate
			  return ((RateCalculationSwapLeg) bean).EndDate;
			case 575402001: // currency
			  return ((RateCalculationSwapLeg) bean).Currency;
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
	  /// The bean-builder for {@code RateCalculationSwapLeg}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<RateCalculationSwapLeg>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PayReceive payReceive_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PeriodicSchedule accrualSchedule_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PaymentSchedule paymentSchedule_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal NotionalSchedule notionalSchedule_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal RateCalculation calculation_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(RateCalculationSwapLeg beanToCopy)
		{
		  this.payReceive_Renamed = beanToCopy.PayReceive;
		  this.accrualSchedule_Renamed = beanToCopy.AccrualSchedule;
		  this.paymentSchedule_Renamed = beanToCopy.PaymentSchedule;
		  this.notionalSchedule_Renamed = beanToCopy.NotionalSchedule;
		  this.calculation_Renamed = beanToCopy.Calculation;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case 304659814: // accrualSchedule
			  return accrualSchedule_Renamed;
			case -1499086147: // paymentSchedule
			  return paymentSchedule_Renamed;
			case 1447860727: // notionalSchedule
			  return notionalSchedule_Renamed;
			case -934682935: // calculation
			  return calculation_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  this.payReceive_Renamed = (PayReceive) newValue;
			  break;
			case 304659814: // accrualSchedule
			  this.accrualSchedule_Renamed = (PeriodicSchedule) newValue;
			  break;
			case -1499086147: // paymentSchedule
			  this.paymentSchedule_Renamed = (PaymentSchedule) newValue;
			  break;
			case 1447860727: // notionalSchedule
			  this.notionalSchedule_Renamed = (NotionalSchedule) newValue;
			  break;
			case -934682935: // calculation
			  this.calculation_Renamed = (RateCalculation) newValue;
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

		public override RateCalculationSwapLeg build()
		{
		  return new RateCalculationSwapLeg(payReceive_Renamed, accrualSchedule_Renamed, paymentSchedule_Renamed, notionalSchedule_Renamed, calculation_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the leg is pay or receive.
		/// <para>
		/// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
		/// A value of 'Receive' implies that the resulting amount is received from the counterparty.
		/// Note that negative interest rates can result in a payment in the opposite
		/// direction to that implied by this indicator.
		/// </para>
		/// </summary>
		/// <param name="payReceive">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder payReceive(PayReceive payReceive)
		{
		  JodaBeanUtils.notNull(payReceive, "payReceive");
		  this.payReceive_Renamed = payReceive;
		  return this;
		}

		/// <summary>
		/// Sets the accrual schedule.
		/// <para>
		/// This is used to define the accrual periods.
		/// These are used directly or indirectly to determine other dates in the swap.
		/// </para>
		/// </summary>
		/// <param name="accrualSchedule">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualSchedule(PeriodicSchedule accrualSchedule)
		{
		  JodaBeanUtils.notNull(accrualSchedule, "accrualSchedule");
		  this.accrualSchedule_Renamed = accrualSchedule;
		  return this;
		}

		/// <summary>
		/// Sets the payment schedule.
		/// <para>
		/// This is used to define the payment periods, including any compounding.
		/// The payment period dates are based on the accrual schedule.
		/// </para>
		/// </summary>
		/// <param name="paymentSchedule">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentSchedule(PaymentSchedule paymentSchedule)
		{
		  JodaBeanUtils.notNull(paymentSchedule, "paymentSchedule");
		  this.paymentSchedule_Renamed = paymentSchedule;
		  return this;
		}

		/// <summary>
		/// Sets the notional schedule.
		/// <para>
		/// The notional amount schedule, which can vary during the lifetime of the swap.
		/// In most cases, the notional amount is not exchanged, with only the net difference being exchanged.
		/// However, in certain cases, initial, final or intermediate amounts are exchanged.
		/// </para>
		/// </summary>
		/// <param name="notionalSchedule">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notionalSchedule(NotionalSchedule notionalSchedule)
		{
		  JodaBeanUtils.notNull(notionalSchedule, "notionalSchedule");
		  this.notionalSchedule_Renamed = notionalSchedule;
		  return this;
		}

		/// <summary>
		/// Sets the interest rate accrual calculation.
		/// <para>
		/// Different kinds of swap leg are determined by the subclass used here.
		/// See <seealso cref="FixedRateCalculation"/>, <seealso cref="IborRateCalculation"/> and <seealso cref="OvernightRateCalculation"/>.
		/// </para>
		/// </summary>
		/// <param name="calculation">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder calculation(RateCalculation calculation)
		{
		  JodaBeanUtils.notNull(calculation, "calculation");
		  this.calculation_Renamed = calculation;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("RateCalculationSwapLeg.Builder{");
		  buf.Append("payReceive").Append('=').Append(JodaBeanUtils.ToString(payReceive_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualSchedule").Append('=').Append(JodaBeanUtils.ToString(accrualSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentSchedule").Append('=').Append(JodaBeanUtils.ToString(paymentSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("notionalSchedule").Append('=').Append(JodaBeanUtils.ToString(notionalSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("calculation").Append('=').Append(JodaBeanUtils.ToString(calculation_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}