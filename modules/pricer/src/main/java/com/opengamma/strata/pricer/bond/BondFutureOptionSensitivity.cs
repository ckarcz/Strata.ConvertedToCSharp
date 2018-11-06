using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
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
	/// Point sensitivity to an implied volatility for a bond future option model.
	/// <para>
	/// Holds the sensitivity to a specific volatility point.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class BondFutureOptionSensitivity implements com.opengamma.strata.market.sensitivity.PointSensitivity, com.opengamma.strata.market.sensitivity.PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class BondFutureOptionSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final BondFutureVolatilitiesName volatilitiesName;
		private readonly BondFutureVolatilitiesName volatilitiesName;
	  /// <summary>
	  /// The expiry date-time of the option.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final double expiry;
	  private readonly double expiry;
	  /// <summary>
	  /// The expiry date of the underlying future.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate futureExpiryDate;
	  private readonly LocalDate futureExpiryDate;
	  /// <summary>
	  /// The option strike price.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double strikePrice;
	  private readonly double strikePrice;
	  /// <summary>
	  /// The underlying future price.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double futurePrice;
	  private readonly double futurePrice;
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
	  /// Obtains an instance based on the security ID.
	  /// </summary>
	  /// <param name="volatilitiesName">  the name of the volatilities </param>
	  /// <param name="expiry">  the time to expiry of the option as a year fraction </param>
	  /// <param name="futureExpiryDate">  the expiry date of the underlying future </param>
	  /// <param name="strikePrice">  the strike price of the option </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static BondFutureOptionSensitivity of(BondFutureVolatilitiesName volatilitiesName, double expiry, LocalDate futureExpiryDate, double strikePrice, double futurePrice, Currency sensitivityCurrency, double sensitivity)
	  {

		return new BondFutureOptionSensitivity(volatilitiesName, expiry, futureExpiryDate, strikePrice, futurePrice, sensitivityCurrency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public BondFutureOptionSensitivity withCurrency(Currency currency)
	  {
		if (this.currency.Equals(currency))
		{
		  return this;
		}
		return new BondFutureOptionSensitivity(volatilitiesName, expiry, futureExpiryDate, strikePrice, futurePrice, currency, sensitivity);
	  }

	  public BondFutureOptionSensitivity withSensitivity(double sensitivity)
	  {
		return new BondFutureOptionSensitivity(volatilitiesName, expiry, futureExpiryDate, strikePrice, futurePrice, currency, sensitivity);
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is BondFutureOptionSensitivity)
		{
		  BondFutureOptionSensitivity otherOption = (BondFutureOptionSensitivity) other;
		  return ComparisonChain.start().compare(volatilitiesName.ToString(), otherOption.volatilitiesName.ToString()).compare(expiry, otherOption.expiry).compare(futureExpiryDate, otherOption.futureExpiryDate).compare(strikePrice, otherOption.strikePrice).compare(futurePrice, otherOption.futurePrice).compare(currency, otherOption.currency).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override BondFutureOptionSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return (BondFutureOptionSensitivity) PointSensitivity.this.convertedTo(resultCurrency, rateProvider);
	  }

	  //-------------------------------------------------------------------------
	  public override BondFutureOptionSensitivity multipliedBy(double factor)
	  {
		return new BondFutureOptionSensitivity(volatilitiesName, expiry, futureExpiryDate, strikePrice, futurePrice, currency, sensitivity * factor);
	  }

	  public BondFutureOptionSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new BondFutureOptionSensitivity(volatilitiesName, expiry, futureExpiryDate, strikePrice, futurePrice, currency, @operator(sensitivity));
	  }

	  public BondFutureOptionSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public BondFutureOptionSensitivity cloned()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BondFutureOptionSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BondFutureOptionSensitivity.Meta meta()
	  {
		return BondFutureOptionSensitivity.Meta.INSTANCE;
	  }

	  static BondFutureOptionSensitivity()
	  {
		MetaBean.register(BondFutureOptionSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private BondFutureOptionSensitivity(BondFutureVolatilitiesName volatilitiesName, double expiry, LocalDate futureExpiryDate, double strikePrice, double futurePrice, Currency currency, double sensitivity)
	  {
		JodaBeanUtils.notNull(volatilitiesName, "volatilitiesName");
		JodaBeanUtils.notNull(expiry, "expiry");
		JodaBeanUtils.notNull(futureExpiryDate, "futureExpiryDate");
		JodaBeanUtils.notNull(currency, "currency");
		this.volatilitiesName = volatilitiesName;
		this.expiry = expiry;
		this.futureExpiryDate = futureExpiryDate;
		this.strikePrice = strikePrice;
		this.futurePrice = futurePrice;
		this.currency = currency;
		this.sensitivity = sensitivity;
	  }

	  public override BondFutureOptionSensitivity.Meta metaBean()
	  {
		return BondFutureOptionSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BondFutureVolatilitiesName VolatilitiesName
	  {
		  get
		  {
			return volatilitiesName;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry date-time of the option. </summary>
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
	  /// Gets the expiry date of the underlying future. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate FutureExpiryDate
	  {
		  get
		  {
			return futureExpiryDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the option strike price. </summary>
	  /// <returns> the value of the property </returns>
	  public double StrikePrice
	  {
		  get
		  {
			return strikePrice;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying future price. </summary>
	  /// <returns> the value of the property </returns>
	  public double FuturePrice
	  {
		  get
		  {
			return futurePrice;
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
		  BondFutureOptionSensitivity other = (BondFutureOptionSensitivity) obj;
		  return JodaBeanUtils.equal(volatilitiesName, other.volatilitiesName) && JodaBeanUtils.equal(expiry, other.expiry) && JodaBeanUtils.equal(futureExpiryDate, other.futureExpiryDate) && JodaBeanUtils.equal(strikePrice, other.strikePrice) && JodaBeanUtils.equal(futurePrice, other.futurePrice) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(volatilitiesName);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(futureExpiryDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikePrice);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(futurePrice);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("BondFutureOptionSensitivity{");
		buf.Append("volatilitiesName").Append('=').Append(volatilitiesName).Append(',').Append(' ');
		buf.Append("expiry").Append('=').Append(expiry).Append(',').Append(' ');
		buf.Append("futureExpiryDate").Append('=').Append(futureExpiryDate).Append(',').Append(' ');
		buf.Append("strikePrice").Append('=').Append(strikePrice).Append(',').Append(' ');
		buf.Append("futurePrice").Append('=').Append(futurePrice).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code BondFutureOptionSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  volatilitiesName_Renamed = DirectMetaProperty.ofImmutable(this, "volatilitiesName", typeof(BondFutureOptionSensitivity), typeof(BondFutureVolatilitiesName));
			  expiry_Renamed = DirectMetaProperty.ofImmutable(this, "expiry", typeof(BondFutureOptionSensitivity), Double.TYPE);
			  futureExpiryDate_Renamed = DirectMetaProperty.ofImmutable(this, "futureExpiryDate", typeof(BondFutureOptionSensitivity), typeof(LocalDate));
			  strikePrice_Renamed = DirectMetaProperty.ofImmutable(this, "strikePrice", typeof(BondFutureOptionSensitivity), Double.TYPE);
			  futurePrice_Renamed = DirectMetaProperty.ofImmutable(this, "futurePrice", typeof(BondFutureOptionSensitivity), Double.TYPE);
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(BondFutureOptionSensitivity), typeof(Currency));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(BondFutureOptionSensitivity), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "volatilitiesName", "expiry", "futureExpiryDate", "strikePrice", "futurePrice", "currency", "sensitivity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code volatilitiesName} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BondFutureVolatilitiesName> volatilitiesName_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> expiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code futureExpiryDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> futureExpiryDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikePrice} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> strikePrice_Renamed;
		/// <summary>
		/// The meta-property for the {@code futurePrice} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> futurePrice_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "volatilitiesName", "expiry", "futureExpiryDate", "strikePrice", "futurePrice", "currency", "sensitivity");
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
			case -1119821404: // futureExpiryDate
			  return futureExpiryDate_Renamed;
			case 50946231: // strikePrice
			  return strikePrice_Renamed;
			case -518499002: // futurePrice
			  return futurePrice_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 564403871: // sensitivity
			  return sensitivity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends BondFutureOptionSensitivity> builder()
		public override BeanBuilder<BondFutureOptionSensitivity> builder()
		{
		  return new BondFutureOptionSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BondFutureOptionSensitivity);
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
		public MetaProperty<BondFutureVolatilitiesName> volatilitiesName()
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
		/// The meta-property for the {@code futureExpiryDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> futureExpiryDate()
		{
		  return futureExpiryDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikePrice} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> strikePrice()
		{
		  return strikePrice_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code futurePrice} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> futurePrice()
		{
		  return futurePrice_Renamed;
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
			  return ((BondFutureOptionSensitivity) bean).VolatilitiesName;
			case -1289159373: // expiry
			  return ((BondFutureOptionSensitivity) bean).Expiry;
			case -1119821404: // futureExpiryDate
			  return ((BondFutureOptionSensitivity) bean).FutureExpiryDate;
			case 50946231: // strikePrice
			  return ((BondFutureOptionSensitivity) bean).StrikePrice;
			case -518499002: // futurePrice
			  return ((BondFutureOptionSensitivity) bean).FuturePrice;
			case 575402001: // currency
			  return ((BondFutureOptionSensitivity) bean).Currency;
			case 564403871: // sensitivity
			  return ((BondFutureOptionSensitivity) bean).Sensitivity;
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
	  /// The bean-builder for {@code BondFutureOptionSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<BondFutureOptionSensitivity>
	  {

		internal BondFutureVolatilitiesName volatilitiesName;
		internal double expiry;
		internal LocalDate futureExpiryDate;
		internal double strikePrice;
		internal double futurePrice;
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
			case -1119821404: // futureExpiryDate
			  return futureExpiryDate;
			case 50946231: // strikePrice
			  return strikePrice;
			case -518499002: // futurePrice
			  return futurePrice;
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
			  this.volatilitiesName = (BondFutureVolatilitiesName) newValue;
			  break;
			case -1289159373: // expiry
			  this.expiry = (double?) newValue.Value;
			  break;
			case -1119821404: // futureExpiryDate
			  this.futureExpiryDate = (LocalDate) newValue;
			  break;
			case 50946231: // strikePrice
			  this.strikePrice = (double?) newValue.Value;
			  break;
			case -518499002: // futurePrice
			  this.futurePrice = (double?) newValue.Value;
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

		public override BondFutureOptionSensitivity build()
		{
		  return new BondFutureOptionSensitivity(volatilitiesName, expiry, futureExpiryDate, strikePrice, futurePrice, currency, sensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("BondFutureOptionSensitivity.Builder{");
		  buf.Append("volatilitiesName").Append('=').Append(JodaBeanUtils.ToString(volatilitiesName)).Append(',').Append(' ');
		  buf.Append("expiry").Append('=').Append(JodaBeanUtils.ToString(expiry)).Append(',').Append(' ');
		  buf.Append("futureExpiryDate").Append('=').Append(JodaBeanUtils.ToString(futureExpiryDate)).Append(',').Append(' ');
		  buf.Append("strikePrice").Append('=').Append(JodaBeanUtils.ToString(strikePrice)).Append(',').Append(' ');
		  buf.Append("futurePrice").Append('=').Append(JodaBeanUtils.ToString(futurePrice)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}