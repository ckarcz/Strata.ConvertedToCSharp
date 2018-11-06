using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Point sensitivity to a forward rate of an FX rate for an FX index.
	/// <para>
	/// Holds the sensitivity to the <seealso cref="FxIndex"/> curve at a fixing date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FxIndexSensitivity implements com.opengamma.strata.market.sensitivity.PointSensitivity, com.opengamma.strata.market.sensitivity.PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxIndexSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.FxIndexObservation observation;
		private readonly FxIndexObservation observation;
	  /// <summary>
	  /// The reference currency.
	  /// <para>
	  /// This is the base currency of the FX conversion that occurs using the index.
	  /// The reference currency must be one of the two currencies of the index.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency referenceCurrency;
	  private readonly Currency referenceCurrency;
	  /// <summary>
	  /// The currency of the sensitivity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The value of the sensitivity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final double sensitivity;
	  private readonly double sensitivity;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the observation, reference currency and sensitivity value.
	  /// <para>
	  /// The sensitivity currency is defaulted to be the counter currency of queried currency pair.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <param name="referenceCurrency">  the reference currency </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static FxIndexSensitivity of(FxIndexObservation observation, Currency referenceCurrency, double sensitivity)
	  {
		CurrencyPair obsPair = observation.CurrencyPair;
		bool inverse = referenceCurrency.Equals(obsPair.Counter);
		CurrencyPair queriedPair = inverse ? obsPair.inverse() : obsPair;
		Currency sensiCurrency = queriedPair.Counter;
		return new FxIndexSensitivity(observation, referenceCurrency, sensiCurrency, sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance from the observation, reference currency and sensitivity value,
	  /// specifying the currency of the value.
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <param name="referenceCurrency">  the reference currency </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static FxIndexSensitivity of(FxIndexObservation observation, Currency referenceCurrency, Currency sensitivityCurrency, double sensitivity)
	  {

		return new FxIndexSensitivity(observation, referenceCurrency, sensitivityCurrency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this sensitivity to an {@code FxForwardSensitivity}.
	  /// <para>
	  /// The time series, fixing date and FX index are lost by this conversion.
	  /// Instead, maturity date and currency pair are contained in <seealso cref="FxForwardSensitivity"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the FX forward sensitivity </returns>
	  public FxForwardSensitivity toFxForwardSensitivity()
	  {
		return FxForwardSensitivity.of(observation.CurrencyPair, referenceCurrency, observation.MaturityDate, currency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the FX index that the sensitivity refers to.
	  /// </summary>
	  /// <returns> the FX index </returns>
	  public FxIndex Index
	  {
		  get
		  {
			return observation.Index;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public FxIndexSensitivity withCurrency(Currency currency)
	  {
		if (this.currency.Equals(currency))
		{
		  return this;
		}
		return new FxIndexSensitivity(observation, referenceCurrency, currency, sensitivity);
	  }

	  public FxIndexSensitivity withSensitivity(double sensitivity)
	  {
		return new FxIndexSensitivity(observation, referenceCurrency, currency, sensitivity);
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is FxIndexSensitivity)
		{
		  FxIndexSensitivity otherFx = (FxIndexSensitivity) other;
		  return ComparisonChain.start().compare(Index.ToString(), otherFx.Index.ToString()).compare(currency, otherFx.currency).compare(referenceCurrency, otherFx.referenceCurrency).compare(observation.FixingDate, otherFx.observation.FixingDate).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override FxIndexSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return (FxIndexSensitivity) PointSensitivity.this.convertedTo(resultCurrency, rateProvider);
	  }

	  //-------------------------------------------------------------------------
	  public override FxIndexSensitivity multipliedBy(double factor)
	  {
		return new FxIndexSensitivity(observation, referenceCurrency, currency, sensitivity * factor);
	  }

	  public FxIndexSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new FxIndexSensitivity(observation, referenceCurrency, currency, @operator(sensitivity));
	  }

	  public FxIndexSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public FxIndexSensitivity cloned()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxIndexSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxIndexSensitivity.Meta meta()
	  {
		return FxIndexSensitivity.Meta.INSTANCE;
	  }

	  static FxIndexSensitivity()
	  {
		MetaBean.register(FxIndexSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FxIndexSensitivity(FxIndexObservation observation, Currency referenceCurrency, Currency currency, double sensitivity)
	  {
		JodaBeanUtils.notNull(observation, "observation");
		JodaBeanUtils.notNull(referenceCurrency, "referenceCurrency");
		JodaBeanUtils.notNull(currency, "currency");
		this.observation = observation;
		this.referenceCurrency = referenceCurrency;
		this.currency = currency;
		this.sensitivity = sensitivity;
	  }

	  public override FxIndexSensitivity.Meta metaBean()
	  {
		return FxIndexSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the FX rate observation.
	  /// <para>
	  /// This includes the index and fixing date.
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
	  /// Gets the reference currency.
	  /// <para>
	  /// This is the base currency of the FX conversion that occurs using the index.
	  /// The reference currency must be one of the two currencies of the index.
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
	  /// <summary>
	  /// Gets the currency of the sensitivity. </summary>
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
	  /// Gets the value of the sensitivity. </summary>
	  /// <returns> the value of the property </returns>
	  public double Sensitivity
	  {
		  get
		  {
			return sensitivity;
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
		  FxIndexSensitivity other = (FxIndexSensitivity) obj;
		  return JodaBeanUtils.equal(observation, other.observation) && JodaBeanUtils.equal(referenceCurrency, other.referenceCurrency) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(referenceCurrency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("FxIndexSensitivity{");
		buf.Append("observation").Append('=').Append(observation).Append(',').Append(' ');
		buf.Append("referenceCurrency").Append('=').Append(referenceCurrency).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxIndexSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  observation_Renamed = DirectMetaProperty.ofImmutable(this, "observation", typeof(FxIndexSensitivity), typeof(FxIndexObservation));
			  referenceCurrency_Renamed = DirectMetaProperty.ofImmutable(this, "referenceCurrency", typeof(FxIndexSensitivity), typeof(Currency));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(FxIndexSensitivity), typeof(Currency));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(FxIndexSensitivity), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "observation", "referenceCurrency", "currency", "sensitivity");
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
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code sensitivity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> sensitivity_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "observation", "referenceCurrency", "currency", "sensitivity");
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
			case 575402001: // currency
			  return currency_Renamed;
			case 564403871: // sensitivity
			  return sensitivity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxIndexSensitivity> builder()
		public override BeanBuilder<FxIndexSensitivity> builder()
		{
		  return new FxIndexSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxIndexSensitivity);
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

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code sensitivity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> sensitivity()
		{
		  return sensitivity_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 122345516: // observation
			  return ((FxIndexSensitivity) bean).Observation;
			case 727652476: // referenceCurrency
			  return ((FxIndexSensitivity) bean).ReferenceCurrency;
			case 575402001: // currency
			  return ((FxIndexSensitivity) bean).Currency;
			case 564403871: // sensitivity
			  return ((FxIndexSensitivity) bean).Sensitivity;
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
	  /// The bean-builder for {@code FxIndexSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxIndexSensitivity>
	  {

		internal FxIndexObservation observation;
		internal Currency referenceCurrency;
		internal Currency currency;
		internal double sensitivity;

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
			case 575402001: // currency
			  return currency;
			case 564403871: // sensitivity
			  return sensitivity;
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
			case 575402001: // currency
			  this.currency = (Currency) newValue;
			  break;
			case 564403871: // sensitivity
			  this.sensitivity = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FxIndexSensitivity build()
		{
		  return new FxIndexSensitivity(observation, referenceCurrency, currency, sensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("FxIndexSensitivity.Builder{");
		  buf.Append("observation").Append('=').Append(JodaBeanUtils.ToString(observation)).Append(',').Append(' ');
		  buf.Append("referenceCurrency").Append('=').Append(JodaBeanUtils.ToString(referenceCurrency)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}