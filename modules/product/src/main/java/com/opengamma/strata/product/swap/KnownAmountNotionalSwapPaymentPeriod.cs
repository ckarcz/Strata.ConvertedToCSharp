using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A period within a swap that results in a known amount.
	/// <para>
	/// A swap leg consists of one or more periods that result in a payment.
	/// The standard class, <seealso cref="RatePaymentPeriod"/>, represents a payment period calculated
	/// from a fixed or floating rate. By contrast, this class represents a period
	/// where the amount of the payment is known and fixed.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class KnownAmountNotionalSwapPaymentPeriod implements NotionalPaymentPeriod, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class KnownAmountNotionalSwapPaymentPeriod : NotionalPaymentPeriod, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Payment payment;
		private readonly Payment payment;
	  /// <summary>
	  /// The start date of the payment period.
	  /// <para>
	  /// This is the first date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date of the payment period.
	  /// <para>
	  /// This is the last date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The unadjusted start date.
	  /// <para>
	  /// The start date before any business day adjustment is applied.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the start date if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate unadjustedStartDate;
	  private readonly LocalDate unadjustedStartDate;
	  /// <summary>
	  /// The unadjusted end date.
	  /// <para>
	  /// The end date before any business day adjustment is applied.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the end date if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate unadjustedEndDate;
	  private readonly LocalDate unadjustedEndDate;
	  /// <summary>
	  /// The notional amount, positive if receiving, negative if paying.
	  /// <para>
	  /// The notional amount applicable during the period.
	  /// The currency of the notional is typically the same as {@code currency}.
	  /// However, if FX reset applies, the currency will differ.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final com.opengamma.strata.basics.currency.CurrencyAmount notionalAmount;
	  private readonly CurrencyAmount notionalAmount;
	  /// <summary>
	  /// The FX reset definition, optional.
	  /// <para>
	  /// This property is used when the defined amount of the notional is specified in
	  /// a currency other than the currency of the swap leg. When this occurs, the notional
	  /// amount has to be converted using an FX rate to the swap leg currency.
	  /// </para>
	  /// <para>
	  /// The FX reset definition must be valid. The payment currency and the currency
	  /// of the notional must differ, and the currency pair must be that of the observation.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional", overrideGet = true) private final com.opengamma.strata.basics.index.FxIndexObservation fxResetObservation;
	  private readonly FxIndexObservation fxResetObservation;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a payment, schedule period and notional.
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="period">  the schedule period </param>
	  /// <param name="notional">  the notional </param>
	  /// <returns> the period </returns>
	  public static KnownAmountNotionalSwapPaymentPeriod of(Payment payment, SchedulePeriod period, CurrencyAmount notional)
	  {

		return KnownAmountNotionalSwapPaymentPeriod.builder().payment(payment).startDate(period.StartDate).endDate(period.EndDate).unadjustedStartDate(period.UnadjustedStartDate).unadjustedEndDate(period.UnadjustedEndDate).notionalAmount(notional).build();
	  }

	  /// <summary>
	  /// Obtains an instance based on a payment, schedule period, notional and FX reset.
	  /// </summary>
	  /// <param name="payment">  the payment </param>
	  /// <param name="period">  the schedule period </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="fxResetObservation">  the FX reset observation </param>
	  /// <returns> the period </returns>
	  public static KnownAmountNotionalSwapPaymentPeriod of(Payment payment, SchedulePeriod period, CurrencyAmount notional, FxIndexObservation fxResetObservation)
	  {

		return KnownAmountNotionalSwapPaymentPeriod.builder().payment(payment).startDate(period.StartDate).endDate(period.EndDate).unadjustedStartDate(period.UnadjustedStartDate).unadjustedEndDate(period.UnadjustedEndDate).notionalAmount(notional).fxResetObservation(fxResetObservation).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.unadjustedStartDate_Renamed == null && builder.startDate_Renamed != null)
		{
		  builder.unadjustedStartDate_Renamed = builder.startDate_Renamed;
		}
		if (builder.unadjustedEndDate_Renamed == null && builder.endDate_Renamed != null)
		{
		  builder.unadjustedEndDate_Renamed = builder.endDate_Renamed;
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		// check for unadjusted must be after firstNonNull
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
		ArgChecker.inOrderNotEqual(unadjustedStartDate, unadjustedEndDate, "unadjustedStartDate", "unadjustedEndDate");
		Currency payCcy = payment.Currency;
		Currency notionalCcy = notionalAmount.Currency;
		if (fxResetObservation != null)
		{
		  if (notionalCcy.Equals(payCcy))
		  {
			throw new System.ArgumentException(Messages.format("Payment currency {} must not equal notional currency {} when FX reset applies", payCcy, notionalCcy));
		  }
		  if (!fxResetObservation.Index.CurrencyPair.contains(payCcy))
		  {
			throw new System.ArgumentException(Messages.format("Payment currency {} must be one of those in the FxReset index {}", payCcy, fxResetObservation.Index));
		  }
		  if (!fxResetObservation.Index.CurrencyPair.contains(notionalCcy))
		  {
			throw new System.ArgumentException(Messages.format("Notional currency {} must be one of those in the FxReset index {}", notionalCcy, fxResetObservation.Index));
		  }
		}
		else
		{
		  if (!notionalCcy.Equals(payCcy))
		  {
			throw new System.ArgumentException(Messages.format("Payment currency {} must equal notional currency {}", payCcy, notionalCcy));
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate PaymentDate
	  {
		  get
		  {
			return payment.Date;
		  }
	  }

	  /// <summary>
	  /// Gets the primary currency of the payment period.
	  /// <para>
	  /// This is the currency of the swap leg and the currency that interest calculation is made in.
	  /// </para>
	  /// <para>
	  /// The amounts of the notional are usually expressed in terms of this currency,
	  /// however they can be converted from amounts in a different currency.
	  /// See the optional {@code fxReset} property.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the primary currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return payment.Currency;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public KnownAmountNotionalSwapPaymentPeriod adjustPaymentDate(TemporalAdjuster adjuster)
	  {
		Payment adjusted = payment.adjustDate(adjuster);
		return adjusted == payment ? this : toBuilder().payment(adjusted).build();
	  }

	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		FxResetObservation.ifPresent(fxReset => builder.add(fxReset.Index));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code KnownAmountNotionalSwapPaymentPeriod}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static KnownAmountNotionalSwapPaymentPeriod.Meta meta()
	  {
		return KnownAmountNotionalSwapPaymentPeriod.Meta.INSTANCE;
	  }

	  static KnownAmountNotionalSwapPaymentPeriod()
	  {
		MetaBean.register(KnownAmountNotionalSwapPaymentPeriod.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static KnownAmountNotionalSwapPaymentPeriod.Builder builder()
	  {
		return new KnownAmountNotionalSwapPaymentPeriod.Builder();
	  }

	  private KnownAmountNotionalSwapPaymentPeriod(Payment payment, LocalDate startDate, LocalDate endDate, LocalDate unadjustedStartDate, LocalDate unadjustedEndDate, CurrencyAmount notionalAmount, FxIndexObservation fxResetObservation)
	  {
		JodaBeanUtils.notNull(payment, "payment");
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		JodaBeanUtils.notNull(unadjustedStartDate, "unadjustedStartDate");
		JodaBeanUtils.notNull(unadjustedEndDate, "unadjustedEndDate");
		this.payment = payment;
		this.startDate = startDate;
		this.endDate = endDate;
		this.unadjustedStartDate = unadjustedStartDate;
		this.unadjustedEndDate = unadjustedEndDate;
		this.notionalAmount = notionalAmount;
		this.fxResetObservation = fxResetObservation;
		validate();
	  }

	  public override KnownAmountNotionalSwapPaymentPeriod.Meta metaBean()
	  {
		return KnownAmountNotionalSwapPaymentPeriod.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment.
	  /// <para>
	  /// This includes the payment date and amount.
	  /// If the schedule adjusts for business days, then the date is the adjusted date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Payment Payment
	  {
		  get
		  {
			return payment;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of the payment period.
	  /// <para>
	  /// This is the first date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the end date of the payment period.
	  /// <para>
	  /// This is the last date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return endDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the unadjusted start date.
	  /// <para>
	  /// The start date before any business day adjustment is applied.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the start date if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate UnadjustedStartDate
	  {
		  get
		  {
			return unadjustedStartDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the unadjusted end date.
	  /// <para>
	  /// The end date before any business day adjustment is applied.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the end date if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate UnadjustedEndDate
	  {
		  get
		  {
			return unadjustedEndDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional amount, positive if receiving, negative if paying.
	  /// <para>
	  /// The notional amount applicable during the period.
	  /// The currency of the notional is typically the same as {@code currency}.
	  /// However, if FX reset applies, the currency will differ.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public CurrencyAmount NotionalAmount
	  {
		  get
		  {
			return notionalAmount;
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
	  /// The FX reset definition must be valid. The payment currency and the currency
	  /// of the notional must differ, and the currency pair must be that of the observation.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<FxIndexObservation> FxResetObservation
	  {
		  get
		  {
			return Optional.ofNullable(fxResetObservation);
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
		  KnownAmountNotionalSwapPaymentPeriod other = (KnownAmountNotionalSwapPaymentPeriod) obj;
		  return JodaBeanUtils.equal(payment, other.payment) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(unadjustedStartDate, other.unadjustedStartDate) && JodaBeanUtils.equal(unadjustedEndDate, other.unadjustedEndDate) && JodaBeanUtils.equal(notionalAmount, other.notionalAmount) && JodaBeanUtils.equal(fxResetObservation, other.fxResetObservation);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedStartDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedEndDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notionalAmount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fxResetObservation);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("KnownAmountNotionalSwapPaymentPeriod{");
		buf.Append("payment").Append('=').Append(payment).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("unadjustedStartDate").Append('=').Append(unadjustedStartDate).Append(',').Append(' ');
		buf.Append("unadjustedEndDate").Append('=').Append(unadjustedEndDate).Append(',').Append(' ');
		buf.Append("notionalAmount").Append('=').Append(notionalAmount).Append(',').Append(' ');
		buf.Append("fxResetObservation").Append('=').Append(JodaBeanUtils.ToString(fxResetObservation));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code KnownAmountNotionalSwapPaymentPeriod}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  payment_Renamed = DirectMetaProperty.ofImmutable(this, "payment", typeof(KnownAmountNotionalSwapPaymentPeriod), typeof(Payment));
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(KnownAmountNotionalSwapPaymentPeriod), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(KnownAmountNotionalSwapPaymentPeriod), typeof(LocalDate));
			  unadjustedStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedStartDate", typeof(KnownAmountNotionalSwapPaymentPeriod), typeof(LocalDate));
			  unadjustedEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedEndDate", typeof(KnownAmountNotionalSwapPaymentPeriod), typeof(LocalDate));
			  notionalAmount_Renamed = DirectMetaProperty.ofImmutable(this, "notionalAmount", typeof(KnownAmountNotionalSwapPaymentPeriod), typeof(CurrencyAmount));
			  fxResetObservation_Renamed = DirectMetaProperty.ofImmutable(this, "fxResetObservation", typeof(KnownAmountNotionalSwapPaymentPeriod), typeof(FxIndexObservation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "payment", "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "notionalAmount", "fxResetObservation");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code payment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Payment> payment_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code unadjustedStartDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> unadjustedStartDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code unadjustedEndDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> unadjustedEndDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code notionalAmount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmount> notionalAmount_Renamed;
		/// <summary>
		/// The meta-property for the {@code fxResetObservation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxIndexObservation> fxResetObservation_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "payment", "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "notionalAmount", "fxResetObservation");
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
			case -786681338: // payment
			  return payment_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 1457691881: // unadjustedStartDate
			  return unadjustedStartDate_Renamed;
			case 31758114: // unadjustedEndDate
			  return unadjustedEndDate_Renamed;
			case -902123592: // notionalAmount
			  return notionalAmount_Renamed;
			case 1301329999: // fxResetObservation
			  return fxResetObservation_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override KnownAmountNotionalSwapPaymentPeriod.Builder builder()
		{
		  return new KnownAmountNotionalSwapPaymentPeriod.Builder();
		}

		public override Type beanType()
		{
		  return typeof(KnownAmountNotionalSwapPaymentPeriod);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code payment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Payment> payment()
		{
		  return payment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code unadjustedStartDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> unadjustedStartDate()
		{
		  return unadjustedStartDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code unadjustedEndDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> unadjustedEndDate()
		{
		  return unadjustedEndDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code notionalAmount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmount> notionalAmount()
		{
		  return notionalAmount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fxResetObservation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxIndexObservation> fxResetObservation()
		{
		  return fxResetObservation_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -786681338: // payment
			  return ((KnownAmountNotionalSwapPaymentPeriod) bean).Payment;
			case -2129778896: // startDate
			  return ((KnownAmountNotionalSwapPaymentPeriod) bean).StartDate;
			case -1607727319: // endDate
			  return ((KnownAmountNotionalSwapPaymentPeriod) bean).EndDate;
			case 1457691881: // unadjustedStartDate
			  return ((KnownAmountNotionalSwapPaymentPeriod) bean).UnadjustedStartDate;
			case 31758114: // unadjustedEndDate
			  return ((KnownAmountNotionalSwapPaymentPeriod) bean).UnadjustedEndDate;
			case -902123592: // notionalAmount
			  return ((KnownAmountNotionalSwapPaymentPeriod) bean).NotionalAmount;
			case 1301329999: // fxResetObservation
			  return ((KnownAmountNotionalSwapPaymentPeriod) bean).fxResetObservation;
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
	  /// The bean-builder for {@code KnownAmountNotionalSwapPaymentPeriod}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<KnownAmountNotionalSwapPaymentPeriod>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Payment payment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate unadjustedStartDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate unadjustedEndDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyAmount notionalAmount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxIndexObservation fxResetObservation_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(KnownAmountNotionalSwapPaymentPeriod beanToCopy)
		{
		  this.payment_Renamed = beanToCopy.Payment;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.unadjustedStartDate_Renamed = beanToCopy.UnadjustedStartDate;
		  this.unadjustedEndDate_Renamed = beanToCopy.UnadjustedEndDate;
		  this.notionalAmount_Renamed = beanToCopy.NotionalAmount;
		  this.fxResetObservation_Renamed = beanToCopy.fxResetObservation;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -786681338: // payment
			  return payment_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 1457691881: // unadjustedStartDate
			  return unadjustedStartDate_Renamed;
			case 31758114: // unadjustedEndDate
			  return unadjustedEndDate_Renamed;
			case -902123592: // notionalAmount
			  return notionalAmount_Renamed;
			case 1301329999: // fxResetObservation
			  return fxResetObservation_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -786681338: // payment
			  this.payment_Renamed = (Payment) newValue;
			  break;
			case -2129778896: // startDate
			  this.startDate_Renamed = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (LocalDate) newValue;
			  break;
			case 1457691881: // unadjustedStartDate
			  this.unadjustedStartDate_Renamed = (LocalDate) newValue;
			  break;
			case 31758114: // unadjustedEndDate
			  this.unadjustedEndDate_Renamed = (LocalDate) newValue;
			  break;
			case -902123592: // notionalAmount
			  this.notionalAmount_Renamed = (CurrencyAmount) newValue;
			  break;
			case 1301329999: // fxResetObservation
			  this.fxResetObservation_Renamed = (FxIndexObservation) newValue;
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

		public override KnownAmountNotionalSwapPaymentPeriod build()
		{
		  preBuild(this);
		  return new KnownAmountNotionalSwapPaymentPeriod(payment_Renamed, startDate_Renamed, endDate_Renamed, unadjustedStartDate_Renamed, unadjustedEndDate_Renamed, notionalAmount_Renamed, fxResetObservation_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the payment.
		/// <para>
		/// This includes the payment date and amount.
		/// If the schedule adjusts for business days, then the date is the adjusted date.
		/// </para>
		/// </summary>
		/// <param name="payment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder payment(Payment payment)
		{
		  JodaBeanUtils.notNull(payment, "payment");
		  this.payment_Renamed = payment;
		  return this;
		}

		/// <summary>
		/// Sets the start date of the payment period.
		/// <para>
		/// This is the first date in the period.
		/// If the schedule adjusts for business days, then this is the adjusted date.
		/// </para>
		/// </summary>
		/// <param name="startDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDate(LocalDate startDate)
		{
		  JodaBeanUtils.notNull(startDate, "startDate");
		  this.startDate_Renamed = startDate;
		  return this;
		}

		/// <summary>
		/// Sets the end date of the payment period.
		/// <para>
		/// This is the last date in the period.
		/// If the schedule adjusts for business days, then this is the adjusted date.
		/// </para>
		/// </summary>
		/// <param name="endDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDate(LocalDate endDate)
		{
		  JodaBeanUtils.notNull(endDate, "endDate");
		  this.endDate_Renamed = endDate;
		  return this;
		}

		/// <summary>
		/// Sets the unadjusted start date.
		/// <para>
		/// The start date before any business day adjustment is applied.
		/// </para>
		/// <para>
		/// When building, this will default to the start date if not specified.
		/// </para>
		/// </summary>
		/// <param name="unadjustedStartDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder unadjustedStartDate(LocalDate unadjustedStartDate)
		{
		  JodaBeanUtils.notNull(unadjustedStartDate, "unadjustedStartDate");
		  this.unadjustedStartDate_Renamed = unadjustedStartDate;
		  return this;
		}

		/// <summary>
		/// Sets the unadjusted end date.
		/// <para>
		/// The end date before any business day adjustment is applied.
		/// </para>
		/// <para>
		/// When building, this will default to the end date if not specified.
		/// </para>
		/// </summary>
		/// <param name="unadjustedEndDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder unadjustedEndDate(LocalDate unadjustedEndDate)
		{
		  JodaBeanUtils.notNull(unadjustedEndDate, "unadjustedEndDate");
		  this.unadjustedEndDate_Renamed = unadjustedEndDate;
		  return this;
		}

		/// <summary>
		/// Sets the notional amount, positive if receiving, negative if paying.
		/// <para>
		/// The notional amount applicable during the period.
		/// The currency of the notional is typically the same as {@code currency}.
		/// However, if FX reset applies, the currency will differ.
		/// </para>
		/// </summary>
		/// <param name="notionalAmount">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notionalAmount(CurrencyAmount notionalAmount)
		{
		  this.notionalAmount_Renamed = notionalAmount;
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
		/// The FX reset definition must be valid. The payment currency and the currency
		/// of the notional must differ, and the currency pair must be that of the observation.
		/// </para>
		/// </summary>
		/// <param name="fxResetObservation">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fxResetObservation(FxIndexObservation fxResetObservation)
		{
		  this.fxResetObservation_Renamed = fxResetObservation;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("KnownAmountNotionalSwapPaymentPeriod.Builder{");
		  buf.Append("payment").Append('=').Append(JodaBeanUtils.ToString(payment_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedStartDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedStartDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedEndDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedEndDate_Renamed)).Append(',').Append(' ');
		  buf.Append("notionalAmount").Append('=').Append(JodaBeanUtils.ToString(notionalAmount_Renamed)).Append(',').Append(' ');
		  buf.Append("fxResetObservation").Append('=').Append(JodaBeanUtils.ToString(fxResetObservation_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}