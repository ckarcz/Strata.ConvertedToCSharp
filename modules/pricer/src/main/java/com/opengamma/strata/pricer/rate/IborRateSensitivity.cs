using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
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
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Point sensitivity to a rate from an Ibor index curve.
	/// <para>
	/// Holds the sensitivity to the <seealso cref="IborIndex"/> curve at a fixing date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class IborRateSensitivity implements com.opengamma.strata.market.sensitivity.PointSensitivity, com.opengamma.strata.market.sensitivity.PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborRateSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.IborIndexObservation observation;
		private readonly IborIndexObservation observation;
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
	  /// Obtains an instance from the observation and sensitivity value.
	  /// <para>
	  /// The currency is defaulted from the index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static IborRateSensitivity of(IborIndexObservation observation, double sensitivity)
	  {
		return new IborRateSensitivity(observation, observation.Index.Currency, sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance from the observation and sensitivity value,
	  /// specifying the currency of the value.
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static IborRateSensitivity of(IborIndexObservation observation, Currency sensitivityCurrency, double sensitivity)
	  {

		return new IborRateSensitivity(observation, sensitivityCurrency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index that the sensitivity refers to.
	  /// </summary>
	  /// <returns> the Ibor index </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return observation.Index;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public IborRateSensitivity withCurrency(Currency currency)
	  {
		if (this.currency.Equals(currency))
		{
		  return this;
		}
		return new IborRateSensitivity(observation, currency, sensitivity);
	  }

	  public IborRateSensitivity withSensitivity(double sensitivity)
	  {
		return new IborRateSensitivity(observation, currency, sensitivity);
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is IborRateSensitivity)
		{
		  IborRateSensitivity otherIbor = (IborRateSensitivity) other;
		  return ComparisonChain.start().compare(Index.ToString(), otherIbor.Index.ToString()).compare(currency, otherIbor.currency).compare(observation.FixingDate, otherIbor.observation.FixingDate).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override IborRateSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return (IborRateSensitivity) PointSensitivity.this.convertedTo(resultCurrency, rateProvider);
	  }

	  //-------------------------------------------------------------------------
	  public override IborRateSensitivity multipliedBy(double factor)
	  {
		return new IborRateSensitivity(observation, currency, sensitivity * factor);
	  }

	  public IborRateSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new IborRateSensitivity(observation, currency, @operator(sensitivity));
	  }

	  public IborRateSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public IborRateSensitivity cloned()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborRateSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborRateSensitivity.Meta meta()
	  {
		return IborRateSensitivity.Meta.INSTANCE;
	  }

	  static IborRateSensitivity()
	  {
		MetaBean.register(IborRateSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private IborRateSensitivity(IborIndexObservation observation, Currency currency, double sensitivity)
	  {
		JodaBeanUtils.notNull(observation, "observation");
		JodaBeanUtils.notNull(currency, "currency");
		this.observation = observation;
		this.currency = currency;
		this.sensitivity = sensitivity;
	  }

	  public override IborRateSensitivity.Meta metaBean()
	  {
		return IborRateSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index observation.
	  /// <para>
	  /// This includes the index and fixing date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndexObservation Observation
	  {
		  get
		  {
			return observation;
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
		  IborRateSensitivity other = (IborRateSensitivity) obj;
		  return JodaBeanUtils.equal(observation, other.observation) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("IborRateSensitivity{");
		buf.Append("observation").Append('=').Append(observation).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborRateSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  observation_Renamed = DirectMetaProperty.ofImmutable(this, "observation", typeof(IborRateSensitivity), typeof(IborIndexObservation));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(IborRateSensitivity), typeof(Currency));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(IborRateSensitivity), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "observation", "currency", "sensitivity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code observation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndexObservation> observation_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "observation", "currency", "sensitivity");
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
			case 575402001: // currency
			  return currency_Renamed;
			case 564403871: // sensitivity
			  return sensitivity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends IborRateSensitivity> builder()
		public override BeanBuilder<IborRateSensitivity> builder()
		{
		  return new IborRateSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborRateSensitivity);
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
		public MetaProperty<IborIndexObservation> observation()
		{
		  return observation_Renamed;
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
			  return ((IborRateSensitivity) bean).Observation;
			case 575402001: // currency
			  return ((IborRateSensitivity) bean).Currency;
			case 564403871: // sensitivity
			  return ((IborRateSensitivity) bean).Sensitivity;
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
	  /// The bean-builder for {@code IborRateSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<IborRateSensitivity>
	  {

		internal IborIndexObservation observation;
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
			  this.observation = (IborIndexObservation) newValue;
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

		public override IborRateSensitivity build()
		{
		  return new IborRateSensitivity(observation, currency, sensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("IborRateSensitivity.Builder{");
		  buf.Append("observation").Append('=').Append(JodaBeanUtils.ToString(observation)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}