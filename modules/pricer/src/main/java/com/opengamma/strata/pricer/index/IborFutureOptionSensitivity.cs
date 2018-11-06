using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
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
	/// Point sensitivity to an implied volatility for a Ibor future option model.
	/// <para>
	/// Holds the sensitivity to a specific volatility point.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class IborFutureOptionSensitivity implements com.opengamma.strata.market.sensitivity.PointSensitivity, com.opengamma.strata.market.sensitivity.PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborFutureOptionSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final IborFutureOptionVolatilitiesName volatilitiesName;
		private readonly IborFutureOptionVolatilitiesName volatilitiesName;
	  /// <summary>
	  /// The time to expiry of the option as a year fraction.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final double expiry;
	  private readonly double expiry;
	  /// <summary>
	  /// The fixing date of the underlying future.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate fixingDate;
	  private readonly LocalDate fixingDate;
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
	  /// Obtains an instance.
	  /// </summary>
	  /// <param name="volatilitiesName">  the name of the volatilities </param>
	  /// <param name="expiry">  the expiry date-time of the option as a year fraction </param>
	  /// <param name="fixingDate">  the fixing date of the underlying future </param>
	  /// <param name="strikePrice">  the strike price of the option </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static IborFutureOptionSensitivity of(IborFutureOptionVolatilitiesName volatilitiesName, double expiry, LocalDate fixingDate, double strikePrice, double futurePrice, Currency sensitivityCurrency, double sensitivity)
	  {

		return new IborFutureOptionSensitivity(volatilitiesName, expiry, fixingDate, strikePrice, futurePrice, sensitivityCurrency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public IborFutureOptionSensitivity withCurrency(Currency currency)
	  {
		if (this.currency.Equals(currency))
		{
		  return this;
		}
		return new IborFutureOptionSensitivity(volatilitiesName, expiry, fixingDate, strikePrice, futurePrice, currency, sensitivity);
	  }

	  public IborFutureOptionSensitivity withSensitivity(double sensitivity)
	  {
		return new IborFutureOptionSensitivity(volatilitiesName, expiry, fixingDate, strikePrice, futurePrice, currency, sensitivity);
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is IborFutureOptionSensitivity)
		{
		  IborFutureOptionSensitivity otherOption = (IborFutureOptionSensitivity) other;
		  return ComparisonChain.start().compare(volatilitiesName, otherOption.volatilitiesName).compare(currency, otherOption.currency).compare(expiry, otherOption.expiry).compare(fixingDate, otherOption.fixingDate).compare(strikePrice, otherOption.strikePrice).compare(futurePrice, otherOption.futurePrice).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override IborFutureOptionSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return (IborFutureOptionSensitivity) PointSensitivity.this.convertedTo(resultCurrency, rateProvider);
	  }

	  //-------------------------------------------------------------------------
	  public override IborFutureOptionSensitivity multipliedBy(double factor)
	  {
		return new IborFutureOptionSensitivity(volatilitiesName, expiry, fixingDate, strikePrice, futurePrice, currency, sensitivity * factor);
	  }

	  public IborFutureOptionSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new IborFutureOptionSensitivity(volatilitiesName, expiry, fixingDate, strikePrice, futurePrice, currency, @operator(sensitivity));
	  }

	  public IborFutureOptionSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public IborFutureOptionSensitivity cloned()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborFutureOptionSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborFutureOptionSensitivity.Meta meta()
	  {
		return IborFutureOptionSensitivity.Meta.INSTANCE;
	  }

	  static IborFutureOptionSensitivity()
	  {
		MetaBean.register(IborFutureOptionSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private IborFutureOptionSensitivity(IborFutureOptionVolatilitiesName volatilitiesName, double expiry, LocalDate fixingDate, double strikePrice, double futurePrice, Currency currency, double sensitivity)
	  {
		JodaBeanUtils.notNull(volatilitiesName, "volatilitiesName");
		JodaBeanUtils.notNull(expiry, "expiry");
		JodaBeanUtils.notNull(fixingDate, "fixingDate");
		JodaBeanUtils.notNull(currency, "currency");
		this.volatilitiesName = volatilitiesName;
		this.expiry = expiry;
		this.fixingDate = fixingDate;
		this.strikePrice = strikePrice;
		this.futurePrice = futurePrice;
		this.currency = currency;
		this.sensitivity = sensitivity;
	  }

	  public override IborFutureOptionSensitivity.Meta metaBean()
	  {
		return IborFutureOptionSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborFutureOptionVolatilitiesName VolatilitiesName
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
	  /// Gets the fixing date of the underlying future. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate FixingDate
	  {
		  get
		  {
			return fixingDate;
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
		  IborFutureOptionSensitivity other = (IborFutureOptionSensitivity) obj;
		  return JodaBeanUtils.equal(volatilitiesName, other.volatilitiesName) && JodaBeanUtils.equal(expiry, other.expiry) && JodaBeanUtils.equal(fixingDate, other.fixingDate) && JodaBeanUtils.equal(strikePrice, other.strikePrice) && JodaBeanUtils.equal(futurePrice, other.futurePrice) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(volatilitiesName);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikePrice);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(futurePrice);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("IborFutureOptionSensitivity{");
		buf.Append("volatilitiesName").Append('=').Append(volatilitiesName).Append(',').Append(' ');
		buf.Append("expiry").Append('=').Append(expiry).Append(',').Append(' ');
		buf.Append("fixingDate").Append('=').Append(fixingDate).Append(',').Append(' ');
		buf.Append("strikePrice").Append('=').Append(strikePrice).Append(',').Append(' ');
		buf.Append("futurePrice").Append('=').Append(futurePrice).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborFutureOptionSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  volatilitiesName_Renamed = DirectMetaProperty.ofImmutable(this, "volatilitiesName", typeof(IborFutureOptionSensitivity), typeof(IborFutureOptionVolatilitiesName));
			  expiry_Renamed = DirectMetaProperty.ofImmutable(this, "expiry", typeof(IborFutureOptionSensitivity), Double.TYPE);
			  fixingDate_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDate", typeof(IborFutureOptionSensitivity), typeof(LocalDate));
			  strikePrice_Renamed = DirectMetaProperty.ofImmutable(this, "strikePrice", typeof(IborFutureOptionSensitivity), Double.TYPE);
			  futurePrice_Renamed = DirectMetaProperty.ofImmutable(this, "futurePrice", typeof(IborFutureOptionSensitivity), Double.TYPE);
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(IborFutureOptionSensitivity), typeof(Currency));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(IborFutureOptionSensitivity), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "volatilitiesName", "expiry", "fixingDate", "strikePrice", "futurePrice", "currency", "sensitivity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code volatilitiesName} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborFutureOptionVolatilitiesName> volatilitiesName_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> expiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> fixingDate_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "volatilitiesName", "expiry", "fixingDate", "strikePrice", "futurePrice", "currency", "sensitivity");
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
			case 1255202043: // fixingDate
			  return fixingDate_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends IborFutureOptionSensitivity> builder()
		public override BeanBuilder<IborFutureOptionSensitivity> builder()
		{
		  return new IborFutureOptionSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborFutureOptionSensitivity);
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
		public MetaProperty<IborFutureOptionVolatilitiesName> volatilitiesName()
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
		/// The meta-property for the {@code fixingDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> fixingDate()
		{
		  return fixingDate_Renamed;
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
			  return ((IborFutureOptionSensitivity) bean).VolatilitiesName;
			case -1289159373: // expiry
			  return ((IborFutureOptionSensitivity) bean).Expiry;
			case 1255202043: // fixingDate
			  return ((IborFutureOptionSensitivity) bean).FixingDate;
			case 50946231: // strikePrice
			  return ((IborFutureOptionSensitivity) bean).StrikePrice;
			case -518499002: // futurePrice
			  return ((IborFutureOptionSensitivity) bean).FuturePrice;
			case 575402001: // currency
			  return ((IborFutureOptionSensitivity) bean).Currency;
			case 564403871: // sensitivity
			  return ((IborFutureOptionSensitivity) bean).Sensitivity;
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
	  /// The bean-builder for {@code IborFutureOptionSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<IborFutureOptionSensitivity>
	  {

		internal IborFutureOptionVolatilitiesName volatilitiesName;
		internal double expiry;
		internal LocalDate fixingDate;
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
			case 1255202043: // fixingDate
			  return fixingDate;
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
			  this.volatilitiesName = (IborFutureOptionVolatilitiesName) newValue;
			  break;
			case -1289159373: // expiry
			  this.expiry = (double?) newValue.Value;
			  break;
			case 1255202043: // fixingDate
			  this.fixingDate = (LocalDate) newValue;
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

		public override IborFutureOptionSensitivity build()
		{
		  return new IborFutureOptionSensitivity(volatilitiesName, expiry, fixingDate, strikePrice, futurePrice, currency, sensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("IborFutureOptionSensitivity.Builder{");
		  buf.Append("volatilitiesName").Append('=').Append(JodaBeanUtils.ToString(volatilitiesName)).Append(',').Append(' ');
		  buf.Append("expiry").Append('=').Append(JodaBeanUtils.ToString(expiry)).Append(',').Append(' ');
		  buf.Append("fixingDate").Append('=').Append(JodaBeanUtils.ToString(fixingDate)).Append(',').Append(' ');
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