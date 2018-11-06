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
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// An FX rate conversion for the notional amount of a swap leg.
	/// <para>
	/// Interest rate swaps are based on a notional amount of money.
	/// The notional can be specified in a currency other than that of the swap leg,
	/// with an FX conversion applied at each payment period boundary.
	/// </para>
	/// <para>
	/// The two currencies involved are the swap leg currency and the reference currency.
	/// The swap leg currency is, in most cases, the currency that payment will occur in.
	/// The reference currency is the currency in which the notional is actually defined.
	/// ISDA refers to the payment currency as the <i>variable currency</i> and the reference
	/// currency as the <i>constant currency</i>.
	/// </para>
	/// <para>
	/// Defined by the 2006 ISDA definitions article 10.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FxReset implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxReset : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.FxIndexObservation observation;
		private readonly FxIndexObservation observation;
	  /// <summary>
	  /// The currency of the notional amount defined in the contract.
	  /// <para>
	  /// This is the currency of notional amount as defined in the contract.
	  /// The amount will be converted from this reference currency to the swap leg currency
	  /// when calculating the value of the leg.
	  /// </para>
	  /// <para>
	  /// The reference currency must be one of the two currencies of the index.
	  /// </para>
	  /// <para>
	  /// The reference currency is also known as the <i>constant currency</i>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency referenceCurrency;
	  private readonly Currency referenceCurrency;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the observation and reference currency.
	  /// </summary>
	  /// <param name="observation">  the FX index observation </param>
	  /// <param name="referenceCurrency">  the reference currency </param>
	  /// <returns> the FX reset </returns>
	  /// <exception cref="IllegalArgumentException"> if the currency is not one of those in the index </exception>
	  public static FxReset of(FxIndexObservation observation, Currency referenceCurrency)
	  {
		return new FxReset(observation, referenceCurrency);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		FxIndex index = observation.Index;
		if (!index.CurrencyPair.contains(referenceCurrency))
		{
		  throw new System.ArgumentException(Messages.format("Reference currency {} must be one of those in the FxIndex {}", referenceCurrency, index));
		}
	  }

	  //-----------------------------------------------------------------------
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

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxReset}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxReset.Meta meta()
	  {
		return FxReset.Meta.INSTANCE;
	  }

	  static FxReset()
	  {
		MetaBean.register(FxReset.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FxReset(FxIndexObservation observation, Currency referenceCurrency)
	  {
		JodaBeanUtils.notNull(observation, "observation");
		JodaBeanUtils.notNull(referenceCurrency, "referenceCurrency");
		this.observation = observation;
		this.referenceCurrency = referenceCurrency;
		validate();
	  }

	  public override FxReset.Meta metaBean()
	  {
		return FxReset.Meta.INSTANCE;
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
	  /// <summary>
	  /// Gets the currency of the notional amount defined in the contract.
	  /// <para>
	  /// This is the currency of notional amount as defined in the contract.
	  /// The amount will be converted from this reference currency to the swap leg currency
	  /// when calculating the value of the leg.
	  /// </para>
	  /// <para>
	  /// The reference currency must be one of the two currencies of the index.
	  /// </para>
	  /// <para>
	  /// The reference currency is also known as the <i>constant currency</i>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency ReferenceCurrency
	  {
		  get
		  {
			return referenceCurrency;
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
		  FxReset other = (FxReset) obj;
		  return JodaBeanUtils.equal(observation, other.observation) && JodaBeanUtils.equal(referenceCurrency, other.referenceCurrency);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(referenceCurrency);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("FxReset{");
		buf.Append("observation").Append('=').Append(observation).Append(',').Append(' ');
		buf.Append("referenceCurrency").Append('=').Append(JodaBeanUtils.ToString(referenceCurrency));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxReset}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  observation_Renamed = DirectMetaProperty.ofImmutable(this, "observation", typeof(FxReset), typeof(FxIndexObservation));
			  referenceCurrency_Renamed = DirectMetaProperty.ofImmutable(this, "referenceCurrency", typeof(FxReset), typeof(Currency));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "observation", "referenceCurrency");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code observation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxIndexObservation> observation_Renamed;
		/// <summary>
		/// The meta-property for the {@code referenceCurrency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> referenceCurrency_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "observation", "referenceCurrency");
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
			case 122345516: // observation
			  return observation_Renamed;
			case 727652476: // referenceCurrency
			  return referenceCurrency_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxReset> builder()
		public override BeanBuilder<FxReset> builder()
		{
		  return new FxReset.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxReset);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code observation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxIndexObservation> observation()
		{
		  return observation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code referenceCurrency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> referenceCurrency()
		{
		  return referenceCurrency_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 122345516: // observation
			  return ((FxReset) bean).Observation;
			case 727652476: // referenceCurrency
			  return ((FxReset) bean).ReferenceCurrency;
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
	  /// The bean-builder for {@code FxReset}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxReset>
	  {

		internal FxIndexObservation observation;
		internal Currency referenceCurrency;

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
			case 122345516: // observation
			  return observation;
			case 727652476: // referenceCurrency
			  return referenceCurrency;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 122345516: // observation
			  this.observation = (FxIndexObservation) newValue;
			  break;
			case 727652476: // referenceCurrency
			  this.referenceCurrency = (Currency) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FxReset build()
		{
		  return new FxReset(observation, referenceCurrency);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("FxReset.Builder{");
		  buf.Append("observation").Append('=').Append(JodaBeanUtils.ToString(observation)).Append(',').Append(' ');
		  buf.Append("referenceCurrency").Append('=').Append(JodaBeanUtils.ToString(referenceCurrency));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}