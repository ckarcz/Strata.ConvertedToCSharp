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
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A period over which a rate of interest is paid.
	/// <para>
	/// A swap leg consists of one or more periods that are the basis of accrual.
	/// The payment period is formed from one or more accrual periods which
	/// detail the type of interest to be accrued, fixed or floating.
	/// </para>
	/// <para>
	/// This class specifies the data necessary to calculate the value of the period.
	/// Any combination of accrual periods is supported in the data model, however
	/// there is no guarantee that exotic combinations will price sensibly.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(constructorScope = "package") public final class RatePaymentPeriod implements NotionalPaymentPeriod, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RatePaymentPeriod : NotionalPaymentPeriod, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate paymentDate;
		private readonly LocalDate paymentDate;
	  /// <summary>
	  /// The accrual periods that combine to form the payment period.
	  /// <para>
	  /// Each accrual period includes the applicable dates and details of how to observe the rate.
	  /// In most cases, there will be one accrual period.
	  /// If there is more than one accrual period then compounding may apply.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableList<RateAccrualPeriod> accrualPeriods;
	  private readonly ImmutableList<RateAccrualPeriod> accrualPeriods;
	  /// <summary>
	  /// The day count convention.
	  /// <para>
	  /// Each accrual period contains a year fraction calculated using this day count.
	  /// This day count is used when there is a need to perform further calculations.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The primary currency of the payment period.
	  /// <para>
	  /// This is the currency of the swap leg and the currency that interest calculation is made in.
	  /// </para>
	  /// <para>
	  /// The amounts of the notional are usually expressed in terms of this currency,
	  /// however they can be converted from amounts in a different currency.
	  /// See the optional {@code fxReset} property.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The FX reset definition, optional.
	  /// <para>
	  /// This property is used when the defined amount of the notional is specified in
	  /// a currency other than the currency of the swap leg. When this occurs, the notional
	  /// amount has to be converted using an FX rate to the swap leg currency.
	  /// </para>
	  /// <para>
	  /// The FX reset definition must be valid. It must have a reference currency that is
	  /// different to that of this period, and the currency of this period must be
	  /// one of those defined by the FX reset index.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final FxReset fxReset;
	  private readonly FxReset fxReset;
	  /// <summary>
	  /// The notional amount, positive if receiving, negative if paying.
	  /// <para>
	  /// The notional amount applicable during the period.
	  /// The currency of the notional is specified by {@code currency} unless there
	  /// is the {@code fxReset} property is present.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The compounding method to use when there is more than one accrual period, default is 'None'.
	  /// <para>
	  /// Compounding is used when combining accrual periods.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CompoundingMethod compoundingMethod;
	  private readonly CompoundingMethod compoundingMethod;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.compoundingMethod(CompoundingMethod.NONE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (fxReset != null)
		{
		  Currency notionalCcy = fxReset.ReferenceCurrency;
		  if (fxReset.ReferenceCurrency.Equals(currency))
		  {
			throw new System.ArgumentException(Messages.format("Payment currency {} must not equal notional currency {} when FX reset applies", currency, notionalCcy));
		  }
		  if (!fxReset.Index.CurrencyPair.contains(currency))
		  {
			throw new System.ArgumentException(Messages.format("Payment currency {} must be one of those in the FxReset index {}", currency, fxReset.Index));
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual start date of the period.
	  /// <para>
	  /// This is the first accrual date in the period.
	  /// This date has typically been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date of the period </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return accrualPeriods.get(0).StartDate;
		  }
	  }

	  /// <summary>
	  /// Gets the accrual end date of the period.
	  /// <para>
	  /// This is the last accrual date in the period.
	  /// This date has typically been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date of the period </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return accrualPeriods.get(accrualPeriods.size() - 1).EndDate;
		  }
	  }

	  /// <summary>
	  /// Gets the notional amount, positive if receiving, negative if paying.
	  /// <para>
	  /// This is the notional amount applicable during the period.
	  /// The currency of the notional is specified by {@code currency} unless there
	  /// is the {@code fxReset} property is present.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the notional as a {@code CurrencyAmount} </returns>
	  public CurrencyAmount NotionalAmount
	  {
		  get
		  {
			if (fxReset != null)
			{
			  return CurrencyAmount.of(fxReset.ReferenceCurrency, notional);
			}
			return CurrencyAmount.of(currency, notional);
		  }
	  }

	  public Optional<FxIndexObservation> FxResetObservation
	  {
		  get
		  {
			return FxReset.map(fxr => fxr.Observation);
		  }
	  }

	  /// <summary>
	  /// Checks whether compounding applies.
	  /// <para>
	  /// Compounding applies if there is more than one accrual period and the
	  /// compounding method is not 'None'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if compounding applies </returns>
	  public bool CompoundingApplicable
	  {
		  get
		  {
			return accrualPeriods.size() > 1 && compoundingMethod != CompoundingMethod.NONE;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public RatePaymentPeriod adjustPaymentDate(TemporalAdjuster adjuster)
	  {
		LocalDate adjusted = paymentDate.with(adjuster);
		return adjusted.Equals(paymentDate) ? this : toBuilder().paymentDate(adjusted).build();
	  }

	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		accrualPeriods.ForEach(accrual => accrual.RateComputation.collectIndices(builder));
		FxReset.ifPresent(fxReset => builder.add(fxReset.Index));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RatePaymentPeriod}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RatePaymentPeriod.Meta meta()
	  {
		return RatePaymentPeriod.Meta.INSTANCE;
	  }

	  static RatePaymentPeriod()
	  {
		MetaBean.register(RatePaymentPeriod.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static RatePaymentPeriod.Builder builder()
	  {
		return new RatePaymentPeriod.Builder();
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="paymentDate">  the value of the property, not null </param>
	  /// <param name="accrualPeriods">  the value of the property, not empty </param>
	  /// <param name="dayCount">  the value of the property, not null </param>
	  /// <param name="currency">  the value of the property, not null </param>
	  /// <param name="fxReset">  the value of the property </param>
	  /// <param name="notional">  the value of the property </param>
	  /// <param name="compoundingMethod">  the value of the property, not null </param>
	  internal RatePaymentPeriod(LocalDate paymentDate, IList<RateAccrualPeriod> accrualPeriods, DayCount dayCount, Currency currency, FxReset fxReset, double notional, CompoundingMethod compoundingMethod)
	  {
		JodaBeanUtils.notNull(paymentDate, "paymentDate");
		JodaBeanUtils.notEmpty(accrualPeriods, "accrualPeriods");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(compoundingMethod, "compoundingMethod");
		this.paymentDate = paymentDate;
		this.accrualPeriods = ImmutableList.copyOf(accrualPeriods);
		this.dayCount = dayCount;
		this.currency = currency;
		this.fxReset = fxReset;
		this.notional = notional;
		this.compoundingMethod = compoundingMethod;
		validate();
	  }

	  public override RatePaymentPeriod.Meta metaBean()
	  {
		return RatePaymentPeriod.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date that payment occurs.
	  /// <para>
	  /// The date that payment is made for the accrual periods.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate PaymentDate
	  {
		  get
		  {
			return paymentDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual periods that combine to form the payment period.
	  /// <para>
	  /// Each accrual period includes the applicable dates and details of how to observe the rate.
	  /// In most cases, there will be one accrual period.
	  /// If there is more than one accrual period then compounding may apply.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<RateAccrualPeriod> AccrualPeriods
	  {
		  get
		  {
			return accrualPeriods;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention.
	  /// <para>
	  /// Each accrual period contains a year fraction calculated using this day count.
	  /// This day count is used when there is a need to perform further calculations.
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
	  /// Gets the primary currency of the payment period.
	  /// <para>
	  /// This is the currency of the swap leg and the currency that interest calculation is made in.
	  /// </para>
	  /// <para>
	  /// The amounts of the notional are usually expressed in terms of this currency,
	  /// however they can be converted from amounts in a different currency.
	  /// See the optional {@code fxReset} property.
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
	  /// Gets the FX reset definition, optional.
	  /// <para>
	  /// This property is used when the defined amount of the notional is specified in
	  /// a currency other than the currency of the swap leg. When this occurs, the notional
	  /// amount has to be converted using an FX rate to the swap leg currency.
	  /// </para>
	  /// <para>
	  /// The FX reset definition must be valid. It must have a reference currency that is
	  /// different to that of this period, and the currency of this period must be
	  /// one of those defined by the FX reset index.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<FxReset> FxReset
	  {
		  get
		  {
			return Optional.ofNullable(fxReset);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional amount, positive if receiving, negative if paying.
	  /// <para>
	  /// The notional amount applicable during the period.
	  /// The currency of the notional is specified by {@code currency} unless there
	  /// is the {@code fxReset} property is present.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Notional
	  {
		  get
		  {
			return notional;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the compounding method to use when there is more than one accrual period, default is 'None'.
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
		  RatePaymentPeriod other = (RatePaymentPeriod) obj;
		  return JodaBeanUtils.equal(paymentDate, other.paymentDate) && JodaBeanUtils.equal(accrualPeriods, other.accrualPeriods) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(fxReset, other.fxReset) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(compoundingMethod, other.compoundingMethod);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualPeriods);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fxReset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(compoundingMethod);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("RatePaymentPeriod{");
		buf.Append("paymentDate").Append('=').Append(paymentDate).Append(',').Append(' ');
		buf.Append("accrualPeriods").Append('=').Append(accrualPeriods).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("fxReset").Append('=').Append(fxReset).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("compoundingMethod").Append('=').Append(JodaBeanUtils.ToString(compoundingMethod));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RatePaymentPeriod}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  paymentDate_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDate", typeof(RatePaymentPeriod), typeof(LocalDate));
			  accrualPeriods_Renamed = DirectMetaProperty.ofImmutable(this, "accrualPeriods", typeof(RatePaymentPeriod), (Type) typeof(ImmutableList));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(RatePaymentPeriod), typeof(DayCount));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(RatePaymentPeriod), typeof(Currency));
			  fxReset_Renamed = DirectMetaProperty.ofImmutable(this, "fxReset", typeof(RatePaymentPeriod), typeof(FxReset));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(RatePaymentPeriod), Double.TYPE);
			  compoundingMethod_Renamed = DirectMetaProperty.ofImmutable(this, "compoundingMethod", typeof(RatePaymentPeriod), typeof(CompoundingMethod));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "paymentDate", "accrualPeriods", "dayCount", "currency", "fxReset", "notional", "compoundingMethod");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code paymentDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> paymentDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code accrualPeriods} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<RateAccrualPeriod>> accrualPeriods = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "accrualPeriods", RatePaymentPeriod.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<RateAccrualPeriod>> accrualPeriods_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code fxReset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxReset> fxReset_Renamed;
		/// <summary>
		/// The meta-property for the {@code notional} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> notional_Renamed;
		/// <summary>
		/// The meta-property for the {@code compoundingMethod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CompoundingMethod> compoundingMethod_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "paymentDate", "accrualPeriods", "dayCount", "currency", "fxReset", "notional", "compoundingMethod");
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
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case -92208605: // accrualPeriods
			  return accrualPeriods_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case -449555555: // fxReset
			  return fxReset_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1376171496: // compoundingMethod
			  return compoundingMethod_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override RatePaymentPeriod.Builder builder()
		{
		  return new RatePaymentPeriod.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RatePaymentPeriod);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code paymentDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> paymentDate()
		{
		  return paymentDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code accrualPeriods} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<RateAccrualPeriod>> accrualPeriods()
		{
		  return accrualPeriods_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fxReset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxReset> fxReset()
		{
		  return fxReset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code notional} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> notional()
		{
		  return notional_Renamed;
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
			case -1540873516: // paymentDate
			  return ((RatePaymentPeriod) bean).PaymentDate;
			case -92208605: // accrualPeriods
			  return ((RatePaymentPeriod) bean).AccrualPeriods;
			case 1905311443: // dayCount
			  return ((RatePaymentPeriod) bean).DayCount;
			case 575402001: // currency
			  return ((RatePaymentPeriod) bean).Currency;
			case -449555555: // fxReset
			  return ((RatePaymentPeriod) bean).fxReset;
			case 1585636160: // notional
			  return ((RatePaymentPeriod) bean).Notional;
			case -1376171496: // compoundingMethod
			  return ((RatePaymentPeriod) bean).CompoundingMethod;
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
	  /// The bean-builder for {@code RatePaymentPeriod}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<RatePaymentPeriod>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate paymentDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<RateAccrualPeriod> accrualPeriods_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxReset fxReset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CompoundingMethod compoundingMethod_Renamed;

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
		internal Builder(RatePaymentPeriod beanToCopy)
		{
		  this.paymentDate_Renamed = beanToCopy.PaymentDate;
		  this.accrualPeriods_Renamed = beanToCopy.AccrualPeriods;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.fxReset_Renamed = beanToCopy.fxReset;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.compoundingMethod_Renamed = beanToCopy.CompoundingMethod;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case -92208605: // accrualPeriods
			  return accrualPeriods_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case -449555555: // fxReset
			  return fxReset_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1376171496: // compoundingMethod
			  return compoundingMethod_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1540873516: // paymentDate
			  this.paymentDate_Renamed = (LocalDate) newValue;
			  break;
			case -92208605: // accrualPeriods
			  this.accrualPeriods_Renamed = (IList<RateAccrualPeriod>) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case -449555555: // fxReset
			  this.fxReset_Renamed = (FxReset) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
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

		public override RatePaymentPeriod build()
		{
		  return new RatePaymentPeriod(paymentDate_Renamed, accrualPeriods_Renamed, dayCount_Renamed, currency_Renamed, fxReset_Renamed, notional_Renamed, compoundingMethod_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the date that payment occurs.
		/// <para>
		/// The date that payment is made for the accrual periods.
		/// If the schedule adjusts for business days, then this is the adjusted date.
		/// </para>
		/// </summary>
		/// <param name="paymentDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentDate(LocalDate paymentDate)
		{
		  JodaBeanUtils.notNull(paymentDate, "paymentDate");
		  this.paymentDate_Renamed = paymentDate;
		  return this;
		}

		/// <summary>
		/// Sets the accrual periods that combine to form the payment period.
		/// <para>
		/// Each accrual period includes the applicable dates and details of how to observe the rate.
		/// In most cases, there will be one accrual period.
		/// If there is more than one accrual period then compounding may apply.
		/// </para>
		/// </summary>
		/// <param name="accrualPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualPeriods(IList<RateAccrualPeriod> accrualPeriods)
		{
		  JodaBeanUtils.notEmpty(accrualPeriods, "accrualPeriods");
		  this.accrualPeriods_Renamed = accrualPeriods;
		  return this;
		}

		/// <summary>
		/// Sets the {@code accrualPeriods} property in the builder
		/// from an array of objects. </summary>
		/// <param name="accrualPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualPeriods(params RateAccrualPeriod[] accrualPeriods)
		{
		  return this.accrualPeriods(ImmutableList.copyOf(accrualPeriods));
		}

		/// <summary>
		/// Sets the day count convention.
		/// <para>
		/// Each accrual period contains a year fraction calculated using this day count.
		/// This day count is used when there is a need to perform further calculations.
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
		/// Sets the primary currency of the payment period.
		/// <para>
		/// This is the currency of the swap leg and the currency that interest calculation is made in.
		/// </para>
		/// <para>
		/// The amounts of the notional are usually expressed in terms of this currency,
		/// however they can be converted from amounts in a different currency.
		/// See the optional {@code fxReset} property.
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
		/// Sets the FX reset definition, optional.
		/// <para>
		/// This property is used when the defined amount of the notional is specified in
		/// a currency other than the currency of the swap leg. When this occurs, the notional
		/// amount has to be converted using an FX rate to the swap leg currency.
		/// </para>
		/// <para>
		/// The FX reset definition must be valid. It must have a reference currency that is
		/// different to that of this period, and the currency of this period must be
		/// one of those defined by the FX reset index.
		/// </para>
		/// </summary>
		/// <param name="fxReset">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fxReset(FxReset fxReset)
		{
		  this.fxReset_Renamed = fxReset;
		  return this;
		}

		/// <summary>
		/// Sets the notional amount, positive if receiving, negative if paying.
		/// <para>
		/// The notional amount applicable during the period.
		/// The currency of the notional is specified by {@code currency} unless there
		/// is the {@code fxReset} property is present.
		/// </para>
		/// </summary>
		/// <param name="notional">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notional(double notional)
		{
		  this.notional_Renamed = notional;
		  return this;
		}

		/// <summary>
		/// Sets the compounding method to use when there is more than one accrual period, default is 'None'.
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("RatePaymentPeriod.Builder{");
		  buf.Append("paymentDate").Append('=').Append(JodaBeanUtils.ToString(paymentDate_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualPeriods").Append('=').Append(JodaBeanUtils.ToString(accrualPeriods_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("fxReset").Append('=').Append(JodaBeanUtils.ToString(fxReset_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("compoundingMethod").Append('=').Append(JodaBeanUtils.ToString(compoundingMethod_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}