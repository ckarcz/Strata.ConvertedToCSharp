using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
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
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Point sensitivity to a swaption implied parameter point.
	/// <para>
	/// Holds the sensitivity to the swaption grid point.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class SwaptionSensitivity implements com.opengamma.strata.market.sensitivity.PointSensitivity, com.opengamma.strata.market.sensitivity.PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SwaptionSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final SwaptionVolatilitiesName volatilitiesName;
		private readonly SwaptionVolatilitiesName volatilitiesName;
	  /// <summary>
	  /// The time to expiry of the option as a year fraction.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final double expiry;
	  private readonly double expiry;
	  /// <summary>
	  /// The underlying swap tenor.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double tenor;
	  private readonly double tenor;
	  /// <summary>
	  /// The swaption strike rate.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double strike;
	  private readonly double strike;
	  /// <summary>
	  /// The underlying swap forward rate.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double forward;
	  private readonly double forward;
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
	  /// Obtains an instance from the specified elements.
	  /// </summary>
	  /// <param name="volatilitiesName">  the name of the volatilities </param>
	  /// <param name="expiry">  the time to expiry of the option as a year fraction </param>
	  /// <param name="tenor">  the underlying swap tenor </param>
	  /// <param name="strike">  the swaption strike rate </param>
	  /// <param name="forward">  the underlying swap forward rate </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static SwaptionSensitivity of(SwaptionVolatilitiesName volatilitiesName, double expiry, double tenor, double strike, double forward, Currency sensitivityCurrency, double sensitivity)
	  {

		return new SwaptionSensitivity(volatilitiesName, expiry, tenor, strike, forward, sensitivityCurrency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public SwaptionSensitivity withCurrency(Currency currency)
	  {
		if (this.currency.Equals(currency))
		{
		  return this;
		}
		return new SwaptionSensitivity(volatilitiesName, expiry, tenor, strike, forward, currency, sensitivity);
	  }

	  public SwaptionSensitivity withSensitivity(double value)
	  {
		return new SwaptionSensitivity(volatilitiesName, expiry, tenor, strike, forward, currency, value);
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is SwaptionSensitivity)
		{
		  SwaptionSensitivity otherSwpt = (SwaptionSensitivity) other;
		  return ComparisonChain.start().compare(volatilitiesName, otherSwpt.volatilitiesName).compare(currency, otherSwpt.currency).compare(expiry, otherSwpt.expiry).compare(tenor, otherSwpt.tenor).compare(strike, otherSwpt.strike).compare(forward, otherSwpt.forward).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override SwaptionSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return (SwaptionSensitivity) PointSensitivity.this.convertedTo(resultCurrency, rateProvider);
	  }

	  //-------------------------------------------------------------------------
	  public override SwaptionSensitivity multipliedBy(double factor)
	  {
		return new SwaptionSensitivity(volatilitiesName, expiry, tenor, strike, forward, currency, sensitivity * factor);
	  }

	  public SwaptionSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new SwaptionSensitivity(volatilitiesName, expiry, tenor, strike, forward, currency, @operator(sensitivity));
	  }

	  public SwaptionSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public SwaptionSensitivity cloned()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SwaptionSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SwaptionSensitivity.Meta meta()
	  {
		return SwaptionSensitivity.Meta.INSTANCE;
	  }

	  static SwaptionSensitivity()
	  {
		MetaBean.register(SwaptionSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private SwaptionSensitivity(SwaptionVolatilitiesName volatilitiesName, double expiry, double tenor, double strike, double forward, Currency currency, double sensitivity)
	  {
		JodaBeanUtils.notNull(volatilitiesName, "volatilitiesName");
		JodaBeanUtils.notNull(expiry, "expiry");
		JodaBeanUtils.notNull(currency, "currency");
		this.volatilitiesName = volatilitiesName;
		this.expiry = expiry;
		this.tenor = tenor;
		this.strike = strike;
		this.forward = forward;
		this.currency = currency;
		this.sensitivity = sensitivity;
	  }

	  public override SwaptionSensitivity.Meta metaBean()
	  {
		return SwaptionSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SwaptionVolatilitiesName VolatilitiesName
	  {
		  get
		  {
			return volatilitiesName;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time to expiry of the option as a year fraction. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public double Expiry
	  {
		  get
		  {
			return expiry;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying swap tenor. </summary>
	  /// <returns> the value of the property </returns>
	  public double Tenor
	  {
		  get
		  {
			return tenor;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the swaption strike rate. </summary>
	  /// <returns> the value of the property </returns>
	  public double Strike
	  {
		  get
		  {
			return strike;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying swap forward rate. </summary>
	  /// <returns> the value of the property </returns>
	  public double Forward
	  {
		  get
		  {
			return forward;
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
		  SwaptionSensitivity other = (SwaptionSensitivity) obj;
		  return JodaBeanUtils.equal(volatilitiesName, other.volatilitiesName) && JodaBeanUtils.equal(expiry, other.expiry) && JodaBeanUtils.equal(tenor, other.tenor) && JodaBeanUtils.equal(strike, other.strike) && JodaBeanUtils.equal(forward, other.forward) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(volatilitiesName);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tenor);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strike);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(forward);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("SwaptionSensitivity{");
		buf.Append("volatilitiesName").Append('=').Append(volatilitiesName).Append(',').Append(' ');
		buf.Append("expiry").Append('=').Append(expiry).Append(',').Append(' ');
		buf.Append("tenor").Append('=').Append(tenor).Append(',').Append(' ');
		buf.Append("strike").Append('=').Append(strike).Append(',').Append(' ');
		buf.Append("forward").Append('=').Append(forward).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SwaptionSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  volatilitiesName_Renamed = DirectMetaProperty.ofImmutable(this, "volatilitiesName", typeof(SwaptionSensitivity), typeof(SwaptionVolatilitiesName));
			  expiry_Renamed = DirectMetaProperty.ofImmutable(this, "expiry", typeof(SwaptionSensitivity), Double.TYPE);
			  tenor_Renamed = DirectMetaProperty.ofImmutable(this, "tenor", typeof(SwaptionSensitivity), Double.TYPE);
			  strike_Renamed = DirectMetaProperty.ofImmutable(this, "strike", typeof(SwaptionSensitivity), Double.TYPE);
			  forward_Renamed = DirectMetaProperty.ofImmutable(this, "forward", typeof(SwaptionSensitivity), Double.TYPE);
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(SwaptionSensitivity), typeof(Currency));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(SwaptionSensitivity), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "volatilitiesName", "expiry", "tenor", "strike", "forward", "currency", "sensitivity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code volatilitiesName} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwaptionVolatilitiesName> volatilitiesName_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> expiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code tenor} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> tenor_Renamed;
		/// <summary>
		/// The meta-property for the {@code strike} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> strike_Renamed;
		/// <summary>
		/// The meta-property for the {@code forward} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> forward_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "volatilitiesName", "expiry", "tenor", "strike", "forward", "currency", "sensitivity");
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
			case 2100884654: // volatilitiesName
			  return volatilitiesName_Renamed;
			case -1289159373: // expiry
			  return expiry_Renamed;
			case 110246592: // tenor
			  return tenor_Renamed;
			case -891985998: // strike
			  return strike_Renamed;
			case -677145915: // forward
			  return forward_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 564403871: // sensitivity
			  return sensitivity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends SwaptionSensitivity> builder()
		public override BeanBuilder<SwaptionSensitivity> builder()
		{
		  return new SwaptionSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SwaptionSensitivity);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code volatilitiesName} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwaptionVolatilitiesName> volatilitiesName()
		{
		  return volatilitiesName_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiry} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> expiry()
		{
		  return expiry_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code tenor} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> tenor()
		{
		  return tenor_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strike} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> strike()
		{
		  return strike_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code forward} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> forward()
		{
		  return forward_Renamed;
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
			case 2100884654: // volatilitiesName
			  return ((SwaptionSensitivity) bean).VolatilitiesName;
			case -1289159373: // expiry
			  return ((SwaptionSensitivity) bean).Expiry;
			case 110246592: // tenor
			  return ((SwaptionSensitivity) bean).Tenor;
			case -891985998: // strike
			  return ((SwaptionSensitivity) bean).Strike;
			case -677145915: // forward
			  return ((SwaptionSensitivity) bean).Forward;
			case 575402001: // currency
			  return ((SwaptionSensitivity) bean).Currency;
			case 564403871: // sensitivity
			  return ((SwaptionSensitivity) bean).Sensitivity;
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
	  /// The bean-builder for {@code SwaptionSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<SwaptionSensitivity>
	  {

		internal SwaptionVolatilitiesName volatilitiesName;
		internal double expiry;
		internal double tenor;
		internal double strike;
		internal double forward;
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
			case 2100884654: // volatilitiesName
			  return volatilitiesName;
			case -1289159373: // expiry
			  return expiry;
			case 110246592: // tenor
			  return tenor;
			case -891985998: // strike
			  return strike;
			case -677145915: // forward
			  return forward;
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
			case 2100884654: // volatilitiesName
			  this.volatilitiesName = (SwaptionVolatilitiesName) newValue;
			  break;
			case -1289159373: // expiry
			  this.expiry = (double?) newValue.Value;
			  break;
			case 110246592: // tenor
			  this.tenor = (double?) newValue.Value;
			  break;
			case -891985998: // strike
			  this.strike = (double?) newValue.Value;
			  break;
			case -677145915: // forward
			  this.forward = (double?) newValue.Value;
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

		public override SwaptionSensitivity build()
		{
		  return new SwaptionSensitivity(volatilitiesName, expiry, tenor, strike, forward, currency, sensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("SwaptionSensitivity.Builder{");
		  buf.Append("volatilitiesName").Append('=').Append(JodaBeanUtils.ToString(volatilitiesName)).Append(',').Append(' ');
		  buf.Append("expiry").Append('=').Append(JodaBeanUtils.ToString(expiry)).Append(',').Append(' ');
		  buf.Append("tenor").Append('=').Append(JodaBeanUtils.ToString(tenor)).Append(',').Append(' ');
		  buf.Append("strike").Append('=').Append(JodaBeanUtils.ToString(strike)).Append(',').Append(' ');
		  buf.Append("forward").Append('=').Append(JodaBeanUtils.ToString(forward)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}