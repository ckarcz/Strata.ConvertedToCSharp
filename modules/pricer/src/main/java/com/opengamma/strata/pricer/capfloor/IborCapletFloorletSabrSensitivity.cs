using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
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
	using SabrParameterType = com.opengamma.strata.market.model.SabrParameterType;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Sensitivity of a caplet/floorlet to SABR model parameters.
	/// <para>
	/// Holds the sensitivity vales to grid points of the SABR parameters.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class IborCapletFloorletSabrSensitivity implements com.opengamma.strata.market.sensitivity.PointSensitivity, com.opengamma.strata.market.sensitivity.PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborCapletFloorletSabrSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final IborCapletFloorletVolatilitiesName volatilitiesName;
		private readonly IborCapletFloorletVolatilitiesName volatilitiesName;
	  /// <summary>
	  /// The time to expiry of the option as a year fraction.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final double expiry;
	  private readonly double expiry;
	  /// <summary>
	  /// The type of the sensitivity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.market.model.SabrParameterType sensitivityType;
	  private readonly SabrParameterType sensitivityType;
	  /// <summary>
	  /// The currency of the sensitivity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
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
	  /// <param name="sensitivityType">  the type of the sensitivity </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the sensitivity object </returns>
	  public static IborCapletFloorletSabrSensitivity of(IborCapletFloorletVolatilitiesName volatilitiesName, double expiry, SabrParameterType sensitivityType, Currency sensitivityCurrency, double sensitivity)
	  {

		return new IborCapletFloorletSabrSensitivity(volatilitiesName, expiry, sensitivityType, sensitivityCurrency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public IborCapletFloorletSabrSensitivity withCurrency(Currency currency)
	  {
		if (this.currency.Equals(currency))
		{
		  return this;
		}
		return new IborCapletFloorletSabrSensitivity(volatilitiesName, expiry, sensitivityType, currency, sensitivity);
	  }

	  public IborCapletFloorletSabrSensitivity withSensitivity(double sensitivity)
	  {
		return new IborCapletFloorletSabrSensitivity(volatilitiesName, expiry, sensitivityType, currency, sensitivity);
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is IborCapletFloorletSabrSensitivity)
		{
		  IborCapletFloorletSabrSensitivity otherSwpt = (IborCapletFloorletSabrSensitivity) other;
		  return ComparisonChain.start().compare(volatilitiesName, otherSwpt.volatilitiesName).compare(currency, otherSwpt.currency).compare(expiry, otherSwpt.expiry).compare(sensitivityType, otherSwpt.sensitivityType).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override IborCapletFloorletSabrSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return (IborCapletFloorletSabrSensitivity) PointSensitivity.this.convertedTo(resultCurrency, rateProvider);
	  }

	  //-------------------------------------------------------------------------
	  public override IborCapletFloorletSabrSensitivity multipliedBy(double factor)
	  {
		return new IborCapletFloorletSabrSensitivity(volatilitiesName, expiry, sensitivityType, currency, sensitivity * factor);
	  }

	  public IborCapletFloorletSabrSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new IborCapletFloorletSabrSensitivity(volatilitiesName, expiry, sensitivityType, currency, @operator(sensitivity));
	  }

	  public IborCapletFloorletSabrSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public IborCapletFloorletSabrSensitivity cloned()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborCapletFloorletSabrSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborCapletFloorletSabrSensitivity.Meta meta()
	  {
		return IborCapletFloorletSabrSensitivity.Meta.INSTANCE;
	  }

	  static IborCapletFloorletSabrSensitivity()
	  {
		MetaBean.register(IborCapletFloorletSabrSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private IborCapletFloorletSabrSensitivity(IborCapletFloorletVolatilitiesName volatilitiesName, double expiry, SabrParameterType sensitivityType, Currency currency, double sensitivity)
	  {
		JodaBeanUtils.notNull(volatilitiesName, "volatilitiesName");
		JodaBeanUtils.notNull(expiry, "expiry");
		this.volatilitiesName = volatilitiesName;
		this.expiry = expiry;
		this.sensitivityType = sensitivityType;
		this.currency = currency;
		this.sensitivity = sensitivity;
	  }

	  public override IborCapletFloorletSabrSensitivity.Meta metaBean()
	  {
		return IborCapletFloorletSabrSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborCapletFloorletVolatilitiesName VolatilitiesName
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
	  /// Gets the type of the sensitivity. </summary>
	  /// <returns> the value of the property </returns>
	  public SabrParameterType SensitivityType
	  {
		  get
		  {
			return sensitivityType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the sensitivity. </summary>
	  /// <returns> the value of the property </returns>
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
		  IborCapletFloorletSabrSensitivity other = (IborCapletFloorletSabrSensitivity) obj;
		  return JodaBeanUtils.equal(volatilitiesName, other.volatilitiesName) && JodaBeanUtils.equal(expiry, other.expiry) && JodaBeanUtils.equal(sensitivityType, other.sensitivityType) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(volatilitiesName);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivityType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("IborCapletFloorletSabrSensitivity{");
		buf.Append("volatilitiesName").Append('=').Append(volatilitiesName).Append(',').Append(' ');
		buf.Append("expiry").Append('=').Append(expiry).Append(',').Append(' ');
		buf.Append("sensitivityType").Append('=').Append(sensitivityType).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborCapletFloorletSabrSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  volatilitiesName_Renamed = DirectMetaProperty.ofImmutable(this, "volatilitiesName", typeof(IborCapletFloorletSabrSensitivity), typeof(IborCapletFloorletVolatilitiesName));
			  expiry_Renamed = DirectMetaProperty.ofImmutable(this, "expiry", typeof(IborCapletFloorletSabrSensitivity), Double.TYPE);
			  sensitivityType_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivityType", typeof(IborCapletFloorletSabrSensitivity), typeof(SabrParameterType));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(IborCapletFloorletSabrSensitivity), typeof(Currency));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(IborCapletFloorletSabrSensitivity), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "volatilitiesName", "expiry", "sensitivityType", "currency", "sensitivity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code volatilitiesName} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborCapletFloorletVolatilitiesName> volatilitiesName_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> expiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code sensitivityType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SabrParameterType> sensitivityType_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "volatilitiesName", "expiry", "sensitivityType", "currency", "sensitivity");
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
			case 1598929529: // sensitivityType
			  return sensitivityType_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 564403871: // sensitivity
			  return sensitivity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends IborCapletFloorletSabrSensitivity> builder()
		public override BeanBuilder<IborCapletFloorletSabrSensitivity> builder()
		{
		  return new IborCapletFloorletSabrSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborCapletFloorletSabrSensitivity);
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
		public MetaProperty<IborCapletFloorletVolatilitiesName> volatilitiesName()
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
		/// The meta-property for the {@code sensitivityType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SabrParameterType> sensitivityType()
		{
		  return sensitivityType_Renamed;
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
			  return ((IborCapletFloorletSabrSensitivity) bean).VolatilitiesName;
			case -1289159373: // expiry
			  return ((IborCapletFloorletSabrSensitivity) bean).Expiry;
			case 1598929529: // sensitivityType
			  return ((IborCapletFloorletSabrSensitivity) bean).SensitivityType;
			case 575402001: // currency
			  return ((IborCapletFloorletSabrSensitivity) bean).Currency;
			case 564403871: // sensitivity
			  return ((IborCapletFloorletSabrSensitivity) bean).Sensitivity;
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
	  /// The bean-builder for {@code IborCapletFloorletSabrSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<IborCapletFloorletSabrSensitivity>
	  {

		internal IborCapletFloorletVolatilitiesName volatilitiesName;
		internal double expiry;
		internal SabrParameterType sensitivityType;
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
			case 1598929529: // sensitivityType
			  return sensitivityType;
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
			  this.volatilitiesName = (IborCapletFloorletVolatilitiesName) newValue;
			  break;
			case -1289159373: // expiry
			  this.expiry = (double?) newValue.Value;
			  break;
			case 1598929529: // sensitivityType
			  this.sensitivityType = (SabrParameterType) newValue;
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

		public override IborCapletFloorletSabrSensitivity build()
		{
		  return new IborCapletFloorletSabrSensitivity(volatilitiesName, expiry, sensitivityType, currency, sensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("IborCapletFloorletSabrSensitivity.Builder{");
		  buf.Append("volatilitiesName").Append('=').Append(JodaBeanUtils.ToString(volatilitiesName)).Append(',').Append(' ');
		  buf.Append("expiry").Append('=').Append(JodaBeanUtils.ToString(expiry)).Append(',').Append(' ');
		  buf.Append("sensitivityType").Append('=').Append(JodaBeanUtils.ToString(sensitivityType)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}