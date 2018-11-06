using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;

	/// <summary>
	/// A Non-Deliverable Forward (NDF), resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="FxNdf"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedFxNdf} from a {@code FxNdf}
	/// using <seealso cref="FxNdf#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedFxNdf} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedFxNdf implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedFxNdf : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyAmount settlementCurrencyNotional;
		private readonly CurrencyAmount settlementCurrencyNotional;
	  /// <summary>
	  /// The FX rate agreed for the value date at the inception of the trade.
	  /// <para>
	  /// The settlement amount is based on the difference between this rate and the
	  /// rate observed on the fixing date using the {@code index}.
	  /// </para>
	  /// <para>
	  /// The forward is between the two currencies defined by the rate.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.FxRate agreedFxRate;
	  private readonly FxRate agreedFxRate;
	  /// <summary>
	  /// The FX index observation.
	  /// <para>
	  /// This defines the observation of the index used to settle the trade.
	  /// The value of the trade is based on the difference between the actual rate and the agreed rate.
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
	  /// <summary>
	  /// The date that the forward settles.
	  /// <para>
	  /// On this date, the settlement amount will be exchanged.
	  /// This date should be a valid business day.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate paymentDate;
	  private readonly LocalDate paymentDate;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		CurrencyPair pair = observation.Index.CurrencyPair;
		if (!pair.contains(settlementCurrencyNotional.Currency))
		{
		  throw new System.ArgumentException("FxIndex and settlement notional currency are incompatible");
		}
		if (!(pair.Equals(agreedFxRate.Pair) || pair.isInverse(agreedFxRate.Pair)))
		{
		  throw new System.ArgumentException("FxIndex and agreed FX rate are incompatible");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the FX index.
	  /// </summary>
	  /// <returns> the FX index </returns>
	  public FxIndex Index
	  {
		  get
		  {
			return observation.Index;
		  }
	  }

	  /// <summary>
	  /// Gets the settlement currency.
	  /// </summary>
	  /// <returns> the currency that is to be settled </returns>
	  public Currency SettlementCurrency
	  {
		  get
		  {
			return settlementCurrencyNotional.Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the settlement notional.
	  /// <para>
	  /// Returns the signed notional amount that is to be settled in the settlement currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the notional </returns>
	  public double SettlementNotional
	  {
		  get
		  {
			return settlementCurrencyNotional.Amount;
		  }
	  }

	  /// <summary>
	  /// Gets the non-deliverable currency.
	  /// <para>
	  /// Returns the currency that is not the settlement currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency that is not to be settled </returns>
	  public Currency NonDeliverableCurrency
	  {
		  get
		  {
			FxIndex index = Index;
			return index.CurrencyPair.Base.Equals(SettlementCurrency) ? index.CurrencyPair.Counter : index.CurrencyPair.Base;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFxNdf}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedFxNdf.Meta meta()
	  {
		return ResolvedFxNdf.Meta.INSTANCE;
	  }

	  static ResolvedFxNdf()
	  {
		MetaBean.register(ResolvedFxNdf.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedFxNdf.Builder builder()
	  {
		return new ResolvedFxNdf.Builder();
	  }

	  private ResolvedFxNdf(CurrencyAmount settlementCurrencyNotional, FxRate agreedFxRate, FxIndexObservation observation, LocalDate paymentDate)
	  {
		JodaBeanUtils.notNull(settlementCurrencyNotional, "settlementCurrencyNotional");
		JodaBeanUtils.notNull(agreedFxRate, "agreedFxRate");
		JodaBeanUtils.notNull(observation, "observation");
		JodaBeanUtils.notNull(paymentDate, "paymentDate");
		this.settlementCurrencyNotional = settlementCurrencyNotional;
		this.agreedFxRate = agreedFxRate;
		this.observation = observation;
		this.paymentDate = paymentDate;
		validate();
	  }

	  public override ResolvedFxNdf.Meta metaBean()
	  {
		return ResolvedFxNdf.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional amount in the settlement currency, positive if receiving, negative if paying.
	  /// <para>
	  /// The amount is signed.
	  /// A positive amount indicates the payment is to be received.
	  /// A negative amount indicates the payment is to be paid.
	  /// </para>
	  /// <para>
	  /// This must be specified in one of the two currencies of the forward.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyAmount SettlementCurrencyNotional
	  {
		  get
		  {
			return settlementCurrencyNotional;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the FX rate agreed for the value date at the inception of the trade.
	  /// <para>
	  /// The settlement amount is based on the difference between this rate and the
	  /// rate observed on the fixing date using the {@code index}.
	  /// </para>
	  /// <para>
	  /// The forward is between the two currencies defined by the rate.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxRate AgreedFxRate
	  {
		  get
		  {
			return agreedFxRate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the FX index observation.
	  /// <para>
	  /// This defines the observation of the index used to settle the trade.
	  /// The value of the trade is based on the difference between the actual rate and the agreed rate.
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
	  /// <summary>
	  /// Gets the date that the forward settles.
	  /// <para>
	  /// On this date, the settlement amount will be exchanged.
	  /// This date should be a valid business day.
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
		  ResolvedFxNdf other = (ResolvedFxNdf) obj;
		  return JodaBeanUtils.equal(settlementCurrencyNotional, other.settlementCurrencyNotional) && JodaBeanUtils.equal(agreedFxRate, other.agreedFxRate) && JodaBeanUtils.equal(observation, other.observation) && JodaBeanUtils.equal(paymentDate, other.paymentDate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementCurrencyNotional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(agreedFxRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("ResolvedFxNdf{");
		buf.Append("settlementCurrencyNotional").Append('=').Append(settlementCurrencyNotional).Append(',').Append(' ');
		buf.Append("agreedFxRate").Append('=').Append(agreedFxRate).Append(',').Append(' ');
		buf.Append("observation").Append('=').Append(observation).Append(',').Append(' ');
		buf.Append("paymentDate").Append('=').Append(JodaBeanUtils.ToString(paymentDate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFxNdf}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  settlementCurrencyNotional_Renamed = DirectMetaProperty.ofImmutable(this, "settlementCurrencyNotional", typeof(ResolvedFxNdf), typeof(CurrencyAmount));
			  agreedFxRate_Renamed = DirectMetaProperty.ofImmutable(this, "agreedFxRate", typeof(ResolvedFxNdf), typeof(FxRate));
			  observation_Renamed = DirectMetaProperty.ofImmutable(this, "observation", typeof(ResolvedFxNdf), typeof(FxIndexObservation));
			  paymentDate_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDate", typeof(ResolvedFxNdf), typeof(LocalDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "settlementCurrencyNotional", "agreedFxRate", "observation", "paymentDate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code settlementCurrencyNotional} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmount> settlementCurrencyNotional_Renamed;
		/// <summary>
		/// The meta-property for the {@code agreedFxRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxRate> agreedFxRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code observation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxIndexObservation> observation_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> paymentDate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "settlementCurrencyNotional", "agreedFxRate", "observation", "paymentDate");
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
			case 594670010: // settlementCurrencyNotional
			  return settlementCurrencyNotional_Renamed;
			case 1040357930: // agreedFxRate
			  return agreedFxRate_Renamed;
			case 122345516: // observation
			  return observation_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedFxNdf.Builder builder()
		{
		  return new ResolvedFxNdf.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedFxNdf);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code settlementCurrencyNotional} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmount> settlementCurrencyNotional()
		{
		  return settlementCurrencyNotional_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code agreedFxRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxRate> agreedFxRate()
		{
		  return agreedFxRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code observation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxIndexObservation> observation()
		{
		  return observation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> paymentDate()
		{
		  return paymentDate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 594670010: // settlementCurrencyNotional
			  return ((ResolvedFxNdf) bean).SettlementCurrencyNotional;
			case 1040357930: // agreedFxRate
			  return ((ResolvedFxNdf) bean).AgreedFxRate;
			case 122345516: // observation
			  return ((ResolvedFxNdf) bean).Observation;
			case -1540873516: // paymentDate
			  return ((ResolvedFxNdf) bean).PaymentDate;
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
	  /// The bean-builder for {@code ResolvedFxNdf}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedFxNdf>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyAmount settlementCurrencyNotional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxRate agreedFxRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxIndexObservation observation_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate paymentDate_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedFxNdf beanToCopy)
		{
		  this.settlementCurrencyNotional_Renamed = beanToCopy.SettlementCurrencyNotional;
		  this.agreedFxRate_Renamed = beanToCopy.AgreedFxRate;
		  this.observation_Renamed = beanToCopy.Observation;
		  this.paymentDate_Renamed = beanToCopy.PaymentDate;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 594670010: // settlementCurrencyNotional
			  return settlementCurrencyNotional_Renamed;
			case 1040357930: // agreedFxRate
			  return agreedFxRate_Renamed;
			case 122345516: // observation
			  return observation_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 594670010: // settlementCurrencyNotional
			  this.settlementCurrencyNotional_Renamed = (CurrencyAmount) newValue;
			  break;
			case 1040357930: // agreedFxRate
			  this.agreedFxRate_Renamed = (FxRate) newValue;
			  break;
			case 122345516: // observation
			  this.observation_Renamed = (FxIndexObservation) newValue;
			  break;
			case -1540873516: // paymentDate
			  this.paymentDate_Renamed = (LocalDate) newValue;
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

		public override ResolvedFxNdf build()
		{
		  return new ResolvedFxNdf(settlementCurrencyNotional_Renamed, agreedFxRate_Renamed, observation_Renamed, paymentDate_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the notional amount in the settlement currency, positive if receiving, negative if paying.
		/// <para>
		/// The amount is signed.
		/// A positive amount indicates the payment is to be received.
		/// A negative amount indicates the payment is to be paid.
		/// </para>
		/// <para>
		/// This must be specified in one of the two currencies of the forward.
		/// </para>
		/// </summary>
		/// <param name="settlementCurrencyNotional">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder settlementCurrencyNotional(CurrencyAmount settlementCurrencyNotional)
		{
		  JodaBeanUtils.notNull(settlementCurrencyNotional, "settlementCurrencyNotional");
		  this.settlementCurrencyNotional_Renamed = settlementCurrencyNotional;
		  return this;
		}

		/// <summary>
		/// Sets the FX rate agreed for the value date at the inception of the trade.
		/// <para>
		/// The settlement amount is based on the difference between this rate and the
		/// rate observed on the fixing date using the {@code index}.
		/// </para>
		/// <para>
		/// The forward is between the two currencies defined by the rate.
		/// </para>
		/// </summary>
		/// <param name="agreedFxRate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder agreedFxRate(FxRate agreedFxRate)
		{
		  JodaBeanUtils.notNull(agreedFxRate, "agreedFxRate");
		  this.agreedFxRate_Renamed = agreedFxRate;
		  return this;
		}

		/// <summary>
		/// Sets the FX index observation.
		/// <para>
		/// This defines the observation of the index used to settle the trade.
		/// The value of the trade is based on the difference between the actual rate and the agreed rate.
		/// </para>
		/// <para>
		/// An FX index is a daily rate of exchange between two currencies.
		/// Note that the order of the currencies in the index does not matter, as the
		/// conversion direction is fully defined by the currency of the reference amount.
		/// </para>
		/// </summary>
		/// <param name="observation">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder observation(FxIndexObservation observation)
		{
		  JodaBeanUtils.notNull(observation, "observation");
		  this.observation_Renamed = observation;
		  return this;
		}

		/// <summary>
		/// Sets the date that the forward settles.
		/// <para>
		/// On this date, the settlement amount will be exchanged.
		/// This date should be a valid business day.
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ResolvedFxNdf.Builder{");
		  buf.Append("settlementCurrencyNotional").Append('=').Append(JodaBeanUtils.ToString(settlementCurrencyNotional_Renamed)).Append(',').Append(' ');
		  buf.Append("agreedFxRate").Append('=').Append(JodaBeanUtils.ToString(agreedFxRate_Renamed)).Append(',').Append(' ');
		  buf.Append("observation").Append('=').Append(JodaBeanUtils.ToString(observation_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDate").Append('=').Append(JodaBeanUtils.ToString(paymentDate_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}