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
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Iterables = com.google.common.collect.Iterables;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// A resolved swap leg, with dates calculated ready for pricing.
	/// <para>
	/// A swap is a financial instrument that represents the exchange of streams of payments.
	/// The swap is formed of legs, where each leg typically represents the obligations
	/// of the seller or buyer of the swap.
	/// </para>
	/// <para>
	/// This class defines a single swap leg in the form of a list of payment periods.
	/// Each payment period typically consists of one or more accrual periods.
	/// </para>
	/// <para>
	/// Any combination of payment and accrual periods is supported in the data model,
	/// however there is no guarantee that exotic combinations will price sensibly.
	/// </para>
	/// <para>
	/// All periods and events must be in the same currency.
	/// </para>
	/// <para>
	/// A {@code ResolvedSwapLeg} contains information based on holiday calendars.
	/// If a holiday calendar changes, the adjusted dates may no longer be correct.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedSwapLeg implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedSwapLeg : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final SwapLegType type;
		private readonly SwapLegType type;
	  /// <summary>
	  /// Whether the leg is pay or receive.
	  /// <para>
	  /// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
	  /// A value of 'Receive' implies that the resulting amount is received from the counterparty.
	  /// Note that negative interest rates can result in a payment in the opposite
	  /// direction to that implied by this indicator.
	  /// </para>
	  /// <para>
	  /// The value of this flag should match the signs of the payment period notionals.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.PayReceive payReceive;
	  private readonly PayReceive payReceive;
	  /// <summary>
	  /// The payment periods that combine to form the swap leg.
	  /// <para>
	  /// Each payment period represents part of the life-time of the leg.
	  /// In most cases, the periods do not overlap. However, since each payment period
	  /// is essentially independent the data model allows overlapping periods.
	  /// </para>
	  /// <para>
	  /// The start date and end date of the leg are determined from the first and last period.
	  /// As such, the periods should be sorted.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", builderType = "List<? extends SwapPaymentPeriod>") private final com.google.common.collect.ImmutableList<SwapPaymentPeriod> paymentPeriods;
	  private readonly ImmutableList<SwapPaymentPeriod> paymentPeriods;
	  /// <summary>
	  /// The payment events that are associated with the swap leg.
	  /// <para>
	  /// Payment events include notional exchange and fees.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "List<? extends SwapPaymentEvent>") private final com.google.common.collect.ImmutableList<SwapPaymentEvent> paymentEvents;
	  private readonly ImmutableList<SwapPaymentEvent> paymentEvents;
	  /// <summary>
	  /// The currency of the leg.
	  /// </summary>
	  [NonSerialized]
	  private readonly Currency currency; // not a property, derived and cached from input data

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private ResolvedSwapLeg(SwapLegType type, com.opengamma.strata.product.common.PayReceive payReceive, java.util.List<? extends SwapPaymentPeriod> paymentPeriods, java.util.List<? extends SwapPaymentEvent> paymentEvents)
	  private ResolvedSwapLeg<T1, T2>(SwapLegType type, PayReceive payReceive, IList<T1> paymentPeriods, IList<T2> paymentEvents) where T1 : SwapPaymentPeriod where T2 : SwapPaymentEvent
	  {

		this.type = ArgChecker.notNull(type, "type");
		this.payReceive = ArgChecker.notNull(payReceive, "payReceive");
		this.paymentPeriods = ImmutableList.copyOf(paymentPeriods);
		this.paymentEvents = ImmutableList.copyOf(paymentEvents);
		// determine and validate currency, with explicit error message
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		Stream<Currency> periodCurrencies = paymentPeriods.Select(SwapPaymentPeriod::getCurrency);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		Stream<Currency> eventCurrencies = paymentEvents.Select(SwapPaymentEvent::getCurrency);
		ISet<Currency> currencies = Stream.concat(periodCurrencies, eventCurrencies).collect(Collectors.toSet());
		if (currencies.Count > 1)
		{
		  throw new System.ArgumentException("Swap leg must have a single currency, found: " + currencies);
		}
		this.currency = Iterables.getOnlyElement(currencies);
	  }

	  // trusted constructor
	  internal ResolvedSwapLeg<T1, T2>(SwapLegType type, PayReceive payReceive, IList<T1> paymentPeriods, IList<T2> paymentEvents, Currency currency) where T1 : SwapPaymentPeriod where T2 : SwapPaymentEvent
	  {

		this.type = type;
		this.payReceive = payReceive;
		this.paymentPeriods = ImmutableList.copyOf(paymentPeriods);
		this.paymentEvents = ImmutableList.copyOf(paymentEvents);
		this.currency = currency;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new ResolvedSwapLeg(type, payReceive, paymentPeriods, paymentEvents);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual start date of the leg.
	  /// <para>
	  /// This is the first accrual date in the leg, often known as the effective date.
	  /// This date has typically been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date of the leg </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return paymentPeriods.get(0).StartDate;
		  }
	  }

	  /// <summary>
	  /// Gets the accrual end date of the leg.
	  /// <para>
	  /// This is the last accrual date in the leg, often known as the termination date.
	  /// This date has typically been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date of the leg </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return paymentPeriods.get(paymentPeriods.size() - 1).EndDate;
		  }
	  }

	  /// <summary>
	  /// Gets the primary currency of the swap leg.
	  /// <para>
	  /// Any currency associated with FX reset is not included.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the payment period applicable for the specified accrual date.
	  /// <para>
	  /// Each payment period contains one or more accrual periods.
	  /// This method finds the matching accrual period and returns the payment period that holds it.
	  /// Periods are considered to contain the end date but not the start date
	  /// If no accrual period contains the date, an empty optional is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to find </param>
	  /// <returns> the payment period applicable at the date </returns>
	  public Optional<SwapPaymentPeriod> findPaymentPeriod(LocalDate date)
	  {
		return paymentPeriods.Where(period => period.StartDate.compareTo(date) < 0 && date.compareTo(period.EndDate) <= 0).First();
	  }

	  /// <summary>
	  /// Collects all the indices referred to by this leg.
	  /// <para>
	  /// A swap leg will typically refer to at least one index, such as 'GBP-LIBOR-3M'.
	  /// Each index that is referred to must be added to the specified builder.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="builder">  the builder to use </param>
	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		paymentPeriods.ForEach(period => period.collectIndices(builder));
	  }

	  /// <summary>
	  /// Finds the notional on the specified date.
	  /// <para>
	  /// If the date falls before the start, the initial notional will be returned.
	  /// If the date falls after the end, the final notional will be returned.
	  /// </para>
	  /// <para>
	  /// An empty optional is returned if the leg has no notional, for example if the payment amount
	  /// is known and explicitly specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date on which the notional is required </param>
	  /// <returns> the notional on the specified date, if available </returns>
	  public Optional<CurrencyAmount> findNotional(LocalDate date)
	  {
		SwapPaymentPeriod paymentPeriod;

		if (!date.isAfter(paymentPeriods.get(0).StartDate))
		{
		  // Use the first payment period if the date is before the start
		  paymentPeriod = paymentPeriods.get(0);
		}
		else if (date.isAfter(paymentPeriods.get(paymentPeriods.size() - 1).EndDate))
		{
		  // Use the last payment period if the date is after the end
		  paymentPeriod = paymentPeriods.get(paymentPeriods.size() - 1);
		}
		else
		{
		  paymentPeriod = findPaymentPeriod(date).get();
		}
		if (!(paymentPeriod is NotionalPaymentPeriod))
		{
		  return null;
		}
		return (((NotionalPaymentPeriod) paymentPeriod).NotionalAmount);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedSwapLeg}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedSwapLeg.Meta meta()
	  {
		return ResolvedSwapLeg.Meta.INSTANCE;
	  }

	  static ResolvedSwapLeg()
	  {
		MetaBean.register(ResolvedSwapLeg.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedSwapLeg.Builder builder()
	  {
		return new ResolvedSwapLeg.Builder();
	  }

	  public override ResolvedSwapLeg.Meta metaBean()
	  {
		return ResolvedSwapLeg.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of the leg, such as Fixed or Ibor.
	  /// <para>
	  /// This provides a high level categorization of the swap leg.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SwapLegType Type
	  {
		  get
		  {
			return type;
		  }
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
	  /// <para>
	  /// The value of this flag should match the signs of the payment period notionals.
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
	  /// Gets the payment periods that combine to form the swap leg.
	  /// <para>
	  /// Each payment period represents part of the life-time of the leg.
	  /// In most cases, the periods do not overlap. However, since each payment period
	  /// is essentially independent the data model allows overlapping periods.
	  /// </para>
	  /// <para>
	  /// The start date and end date of the leg are determined from the first and last period.
	  /// As such, the periods should be sorted.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<SwapPaymentPeriod> PaymentPeriods
	  {
		  get
		  {
			return paymentPeriods;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment events that are associated with the swap leg.
	  /// <para>
	  /// Payment events include notional exchange and fees.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<SwapPaymentEvent> PaymentEvents
	  {
		  get
		  {
			return paymentEvents;
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
		  ResolvedSwapLeg other = (ResolvedSwapLeg) obj;
		  return JodaBeanUtils.equal(type, other.type) && JodaBeanUtils.equal(payReceive, other.payReceive) && JodaBeanUtils.equal(paymentPeriods, other.paymentPeriods) && JodaBeanUtils.equal(paymentEvents, other.paymentEvents);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(type);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payReceive);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentPeriods);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentEvents);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("ResolvedSwapLeg{");
		buf.Append("type").Append('=').Append(type).Append(',').Append(' ');
		buf.Append("payReceive").Append('=').Append(payReceive).Append(',').Append(' ');
		buf.Append("paymentPeriods").Append('=').Append(paymentPeriods).Append(',').Append(' ');
		buf.Append("paymentEvents").Append('=').Append(JodaBeanUtils.ToString(paymentEvents));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedSwapLeg}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  type_Renamed = DirectMetaProperty.ofImmutable(this, "type", typeof(ResolvedSwapLeg), typeof(SwapLegType));
			  payReceive_Renamed = DirectMetaProperty.ofImmutable(this, "payReceive", typeof(ResolvedSwapLeg), typeof(PayReceive));
			  paymentPeriods_Renamed = DirectMetaProperty.ofImmutable(this, "paymentPeriods", typeof(ResolvedSwapLeg), (Type) typeof(ImmutableList));
			  paymentEvents_Renamed = DirectMetaProperty.ofImmutable(this, "paymentEvents", typeof(ResolvedSwapLeg), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "type", "payReceive", "paymentPeriods", "paymentEvents");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code type} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwapLegType> type_Renamed;
		/// <summary>
		/// The meta-property for the {@code payReceive} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PayReceive> payReceive_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentPeriods} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<SwapPaymentPeriod>> paymentPeriods = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "paymentPeriods", ResolvedSwapLeg.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<SwapPaymentPeriod>> paymentPeriods_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentEvents} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<SwapPaymentEvent>> paymentEvents = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "paymentEvents", ResolvedSwapLeg.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<SwapPaymentEvent>> paymentEvents_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "type", "payReceive", "paymentPeriods", "paymentEvents");
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
			case 3575610: // type
			  return type_Renamed;
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case -1674414612: // paymentPeriods
			  return paymentPeriods_Renamed;
			case 1031856831: // paymentEvents
			  return paymentEvents_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedSwapLeg.Builder builder()
		{
		  return new ResolvedSwapLeg.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedSwapLeg);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code type} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwapLegType> type()
		{
		  return type_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code payReceive} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PayReceive> payReceive()
		{
		  return payReceive_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentPeriods} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<SwapPaymentPeriod>> paymentPeriods()
		{
		  return paymentPeriods_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentEvents} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<SwapPaymentEvent>> paymentEvents()
		{
		  return paymentEvents_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  return ((ResolvedSwapLeg) bean).Type;
			case -885469925: // payReceive
			  return ((ResolvedSwapLeg) bean).PayReceive;
			case -1674414612: // paymentPeriods
			  return ((ResolvedSwapLeg) bean).PaymentPeriods;
			case 1031856831: // paymentEvents
			  return ((ResolvedSwapLeg) bean).PaymentEvents;
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
	  /// The bean-builder for {@code ResolvedSwapLeg}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedSwapLeg>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SwapLegType type_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PayReceive payReceive_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends SwapPaymentPeriod> paymentPeriods = com.google.common.collect.ImmutableList.of();
		internal IList<SwapPaymentPeriod> paymentPeriods_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends SwapPaymentEvent> paymentEvents = com.google.common.collect.ImmutableList.of();
		internal IList<SwapPaymentEvent> paymentEvents_Renamed = ImmutableList.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedSwapLeg beanToCopy)
		{
		  this.type_Renamed = beanToCopy.Type;
		  this.payReceive_Renamed = beanToCopy.PayReceive;
		  this.paymentPeriods_Renamed = beanToCopy.PaymentPeriods;
		  this.paymentEvents_Renamed = beanToCopy.PaymentEvents;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  return type_Renamed;
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case -1674414612: // paymentPeriods
			  return paymentPeriods_Renamed;
			case 1031856831: // paymentEvents
			  return paymentEvents_Renamed;
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
			case 3575610: // type
			  this.type_Renamed = (SwapLegType) newValue;
			  break;
			case -885469925: // payReceive
			  this.payReceive_Renamed = (PayReceive) newValue;
			  break;
			case -1674414612: // paymentPeriods
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.paymentPeriods = (java.util.List<? extends SwapPaymentPeriod>) newValue;
			  this.paymentPeriods_Renamed = (IList<SwapPaymentPeriod>) newValue;
			  break;
			case 1031856831: // paymentEvents
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.paymentEvents = (java.util.List<? extends SwapPaymentEvent>) newValue;
			  this.paymentEvents_Renamed = (IList<SwapPaymentEvent>) newValue;
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

		public override ResolvedSwapLeg build()
		{
		  return new ResolvedSwapLeg(type_Renamed, payReceive_Renamed, paymentPeriods_Renamed, paymentEvents_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the type of the leg, such as Fixed or Ibor.
		/// <para>
		/// This provides a high level categorization of the swap leg.
		/// </para>
		/// </summary>
		/// <param name="type">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder type(SwapLegType type)
		{
		  JodaBeanUtils.notNull(type, "type");
		  this.type_Renamed = type;
		  return this;
		}

		/// <summary>
		/// Sets whether the leg is pay or receive.
		/// <para>
		/// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
		/// A value of 'Receive' implies that the resulting amount is received from the counterparty.
		/// Note that negative interest rates can result in a payment in the opposite
		/// direction to that implied by this indicator.
		/// </para>
		/// <para>
		/// The value of this flag should match the signs of the payment period notionals.
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
		/// Sets the payment periods that combine to form the swap leg.
		/// <para>
		/// Each payment period represents part of the life-time of the leg.
		/// In most cases, the periods do not overlap. However, since each payment period
		/// is essentially independent the data model allows overlapping periods.
		/// </para>
		/// <para>
		/// The start date and end date of the leg are determined from the first and last period.
		/// As such, the periods should be sorted.
		/// </para>
		/// </summary>
		/// <param name="paymentPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentPeriods<T1>(IList<T1> paymentPeriods) where T1 : SwapPaymentPeriod
		{
		  JodaBeanUtils.notEmpty(paymentPeriods, "paymentPeriods");
		  this.paymentPeriods_Renamed = paymentPeriods;
		  return this;
		}

		/// <summary>
		/// Sets the {@code paymentPeriods} property in the builder
		/// from an array of objects. </summary>
		/// <param name="paymentPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentPeriods(params SwapPaymentPeriod[] paymentPeriods)
		{
		  return this.paymentPeriods(ImmutableList.copyOf(paymentPeriods));
		}

		/// <summary>
		/// Sets the payment events that are associated with the swap leg.
		/// <para>
		/// Payment events include notional exchange and fees.
		/// </para>
		/// </summary>
		/// <param name="paymentEvents">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentEvents<T1>(IList<T1> paymentEvents) where T1 : SwapPaymentEvent
		{
		  JodaBeanUtils.notNull(paymentEvents, "paymentEvents");
		  this.paymentEvents_Renamed = paymentEvents;
		  return this;
		}

		/// <summary>
		/// Sets the {@code paymentEvents} property in the builder
		/// from an array of objects. </summary>
		/// <param name="paymentEvents">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentEvents(params SwapPaymentEvent[] paymentEvents)
		{
		  return this.paymentEvents(ImmutableList.copyOf(paymentEvents));
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ResolvedSwapLeg.Builder{");
		  buf.Append("type").Append('=').Append(JodaBeanUtils.ToString(type_Renamed)).Append(',').Append(' ');
		  buf.Append("payReceive").Append('=').Append(JodaBeanUtils.ToString(payReceive_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentPeriods").Append('=').Append(JodaBeanUtils.ToString(paymentPeriods_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentEvents").Append('=').Append(JodaBeanUtils.ToString(paymentEvents_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}