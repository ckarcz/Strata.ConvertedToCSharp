using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// An exchange of notionals between two counterparties where FX reset applies.
	/// <para>
	/// In most swaps, the notional amount is not exchanged, with only the interest being exchanged.
	/// However, in the case of an FX reset swap, the notional is exchanged.
	/// The swap contract will define a notional, which may vary over time, in one currency
	/// however payments are defined to occur in a different currency.
	/// An FX conversion is used to convert the amount.
	/// </para>
	/// <para>
	/// For example, a swap may have a notional of GBP 1,000,000 but be paid in USD.
	/// At the start of the first swap period, there is a notional exchange at the prevailing
	/// FX rate, say of USD 1,520,000. At the end of the first swap period, that amount is repaid
	/// and the new FX rate is used to determine the exchange for the second period, say of USD 1,610,000.
	/// In general, only the net difference due to FX will be exchanged at intermediate swap period boundaries.
	/// </para>
	/// <para>
	/// The reference currency is the currency in which the notional is actually defined.
	/// ISDA refers to the payment currency as the <i>variable currency</i> and the reference
	/// currency as the <i>constant currency</i>.
	/// An FX reset swap is also known as a <i>Mark-to-market currency swap</i>.
	/// </para>
	/// <para>
	/// Defined by the 2006 ISDA definitions article 10.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FxResetNotionalExchange implements SwapPaymentEvent, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxResetNotionalExchange : SwapPaymentEvent, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyAmount notionalAmount;
		private readonly CurrencyAmount notionalAmount;
	  /// <summary>
	  /// The date that the payment is made.
	  /// <para>
	  /// Each payment event has a single payment date.
	  /// This date has been adjusted to be a valid business day.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate paymentDate;
	  private readonly LocalDate paymentDate;
	  /// <summary>
	  /// The FX index observation.
	  /// <para>
	  /// This defines the observation of the index used to obtain the FX reset rate.
	  /// </para>
	  /// <para>
	  /// An FX index is a daily rate of exchange between two currencies.
	  /// Note that the order of the currencies in the index does not matter, as the
	  /// conversion direction is fully defined by the currency of the reference amount.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.FxIndexObservation observation;
	  private readonly FxIndexObservation observation;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the amount, date and FX index observation.
	  /// </summary>
	  /// <param name="notionalAmount">  the notional amount that will be FX converted </param>
	  /// <param name="paymentDate">  the date that the payment is made </param>
	  /// <param name="observation">  the FX observation to perform </param>
	  /// <returns> the FX reset notional exchange </returns>
	  public static FxResetNotionalExchange of(CurrencyAmount notionalAmount, LocalDate paymentDate, FxIndexObservation observation)
	  {

		return new FxResetNotionalExchange(notionalAmount, paymentDate, observation);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		FxIndex index = observation.Index;
		if (!index.CurrencyPair.contains(notionalAmount.Currency))
		{
		  throw new System.ArgumentException(Messages.format("Reference currency {} must be one of those in the FxIndex {}", notionalAmount.Currency, index));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment currency.
	  /// <para>
	  /// This returns the currency that the payment is made in.
	  /// ISDA refers to this as the <i>variable currency</i>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the payment currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			FxIndex index = observation.Index;
			Currency indexBase = index.CurrencyPair.Base;
			Currency indexCounter = index.CurrencyPair.Counter;
			return (ReferenceCurrency.Equals(indexBase) ? indexCounter : indexBase);
		  }
	  }

	  /// <summary>
	  /// Gets the reference currency, as defined in the contract.
	  /// <para>
	  /// This is the currency of notional amount as defined in the contract.
	  /// The notional will be converted from this currency to the payment currency using the specified index.
	  /// ISDA refers to this as the <i>constant currency</i>.
	  /// </para>
	  /// <para>
	  /// The reference currency must be one of the two currencies of the index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the reference currency, as defined in the contract </returns>
	  public Currency ReferenceCurrency
	  {
		  get
		  {
			return notionalAmount.Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the amount of the notional.
	  /// <para>
	  /// See <seealso cref="#getNotionalAmount()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the amount of the notional </returns>
	  public double Notional
	  {
		  get
		  {
			return NotionalAmount.Amount;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public FxResetNotionalExchange adjustPaymentDate(TemporalAdjuster adjuster)
	  {
		LocalDate adjusted = paymentDate.with(adjuster);
		return adjusted.Equals(paymentDate) ? this : new FxResetNotionalExchange(notionalAmount, adjusted, observation);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxResetNotionalExchange}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxResetNotionalExchange.Meta meta()
	  {
		return FxResetNotionalExchange.Meta.INSTANCE;
	  }

	  static FxResetNotionalExchange()
	  {
		MetaBean.register(FxResetNotionalExchange.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FxResetNotionalExchange(CurrencyAmount notionalAmount, LocalDate paymentDate, FxIndexObservation observation)
	  {
		JodaBeanUtils.notNull(notionalAmount, "notionalAmount");
		JodaBeanUtils.notNull(paymentDate, "paymentDate");
		JodaBeanUtils.notNull(observation, "observation");
		this.notionalAmount = notionalAmount;
		this.paymentDate = paymentDate;
		this.observation = observation;
		validate();
	  }

	  public override FxResetNotionalExchange.Meta metaBean()
	  {
		return FxResetNotionalExchange.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional amount, positive if receiving, negative if paying.
	  /// <para>
	  /// The notional amount applicable during the period.
	  /// The currency of the notional is specified by {@code referenceCurrency} but will
	  /// be paid after FX conversion using the index.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyAmount NotionalAmount
	  {
		  get
		  {
			return notionalAmount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date that the payment is made.
	  /// <para>
	  /// Each payment event has a single payment date.
	  /// This date has been adjusted to be a valid business day.
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
	  /// Gets the FX index observation.
	  /// <para>
	  /// This defines the observation of the index used to obtain the FX reset rate.
	  /// </para>
	  /// <para>
	  /// An FX index is a daily rate of exchange between two currencies.
	  /// Note that the order of the currencies in the index does not matter, as the
	  /// conversion direction is fully defined by the currency of the reference amount.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxIndexObservation Observation
	  {
		  get
		  {
			return observation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  FxResetNotionalExchange other = (FxResetNotionalExchange) obj;
		  return JodaBeanUtils.equal(notionalAmount, other.notionalAmount) && JodaBeanUtils.equal(paymentDate, other.paymentDate) && JodaBeanUtils.equal(observation, other.observation);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notionalAmount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observation);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("FxResetNotionalExchange{");
		buf.Append("notionalAmount").Append('=').Append(notionalAmount).Append(',').Append(' ');
		buf.Append("paymentDate").Append('=').Append(paymentDate).Append(',').Append(' ');
		buf.Append("observation").Append('=').Append(JodaBeanUtils.ToString(observation));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxResetNotionalExchange}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  notionalAmount_Renamed = DirectMetaProperty.ofImmutable(this, "notionalAmount", typeof(FxResetNotionalExchange), typeof(CurrencyAmount));
			  paymentDate_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDate", typeof(FxResetNotionalExchange), typeof(LocalDate));
			  observation_Renamed = DirectMetaProperty.ofImmutable(this, "observation", typeof(FxResetNotionalExchange), typeof(FxIndexObservation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "notionalAmount", "paymentDate", "observation");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code notionalAmount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmount> notionalAmount_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> paymentDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code observation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxIndexObservation> observation_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "notionalAmount", "paymentDate", "observation");
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
			case -902123592: // notionalAmount
			  return notionalAmount_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case 122345516: // observation
			  return observation_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxResetNotionalExchange> builder()
		public override BeanBuilder<FxResetNotionalExchange> builder()
		{
		  return new FxResetNotionalExchange.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxResetNotionalExchange);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code notionalAmount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmount> notionalAmount()
		{
		  return notionalAmount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> paymentDate()
		{
		  return paymentDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code observation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxIndexObservation> observation()
		{
		  return observation_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -902123592: // notionalAmount
			  return ((FxResetNotionalExchange) bean).NotionalAmount;
			case -1540873516: // paymentDate
			  return ((FxResetNotionalExchange) bean).PaymentDate;
			case 122345516: // observation
			  return ((FxResetNotionalExchange) bean).Observation;
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
	  /// The bean-builder for {@code FxResetNotionalExchange}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxResetNotionalExchange>
	  {

		internal CurrencyAmount notionalAmount;
		internal LocalDate paymentDate;
		internal FxIndexObservation observation;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -902123592: // notionalAmount
			  return notionalAmount;
			case -1540873516: // paymentDate
			  return paymentDate;
			case 122345516: // observation
			  return observation;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -902123592: // notionalAmount
			  this.notionalAmount = (CurrencyAmount) newValue;
			  break;
			case -1540873516: // paymentDate
			  this.paymentDate = (LocalDate) newValue;
			  break;
			case 122345516: // observation
			  this.observation = (FxIndexObservation) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FxResetNotionalExchange build()
		{
		  return new FxResetNotionalExchange(notionalAmount, paymentDate, observation);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("FxResetNotionalExchange.Builder{");
		  buf.Append("notionalAmount").Append('=').Append(JodaBeanUtils.ToString(notionalAmount)).Append(',').Append(' ');
		  buf.Append("paymentDate").Append('=').Append(JodaBeanUtils.ToString(paymentDate)).Append(',').Append(' ');
		  buf.Append("observation").Append('=').Append(JodaBeanUtils.ToString(observation));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}