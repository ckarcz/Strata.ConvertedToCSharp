using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer
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
	/// Point sensitivity to the zero rate curve.
	/// <para>
	/// Holds the sensitivity to the zero rate curve at a specific date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ZeroRateSensitivity implements com.opengamma.strata.market.sensitivity.PointSensitivity, com.opengamma.strata.market.sensitivity.PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ZeroRateSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency curveCurrency;
		private readonly Currency curveCurrency;
	  /// <summary>
	  /// The time that was queried, expressed as a year fraction.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double yearFraction;
	  private readonly double yearFraction;
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
	  /// Obtains an instance from the curve currency, date and value.
	  /// <para>
	  /// The currency representing the curve is used also for the sensitivity currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency of the curve and sensitivity </param>
	  /// <param name="yearFraction">  the year fraction that was looked up on the curve </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static ZeroRateSensitivity of(Currency currency, double yearFraction, double sensitivity)
	  {
		return new ZeroRateSensitivity(currency, yearFraction, currency, sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance from the curve currency, date, sensitivity currency and value.
	  /// <para>
	  /// The currency representing the curve is used also for the sensitivity currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveCurrency">  the currency of the curve </param>
	  /// <param name="yearFraction">  the year fraction that was looked up on the curve </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static ZeroRateSensitivity of(Currency curveCurrency, double yearFraction, Currency sensitivityCurrency, double sensitivity)
	  {
		return new ZeroRateSensitivity(curveCurrency, yearFraction, sensitivityCurrency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public ZeroRateSensitivity withCurrency(Currency currency)
	  {
		if (this.currency.Equals(currency))
		{
		  return this;
		}
		return new ZeroRateSensitivity(curveCurrency, yearFraction, currency, sensitivity);
	  }

	  public ZeroRateSensitivity withSensitivity(double sensitivity)
	  {
		return new ZeroRateSensitivity(curveCurrency, yearFraction, currency, sensitivity);
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is ZeroRateSensitivity)
		{
		  ZeroRateSensitivity otherZero = (ZeroRateSensitivity) other;
		  return ComparisonChain.start().compare(curveCurrency, otherZero.curveCurrency).compare(currency, otherZero.currency).compare(yearFraction, otherZero.yearFraction).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override ZeroRateSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return (ZeroRateSensitivity) PointSensitivity.this.convertedTo(resultCurrency, rateProvider);
	  }

	  //-------------------------------------------------------------------------
	  public override ZeroRateSensitivity multipliedBy(double factor)
	  {
		return new ZeroRateSensitivity(curveCurrency, yearFraction, currency, sensitivity * factor);
	  }

	  public ZeroRateSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new ZeroRateSensitivity(curveCurrency, yearFraction, currency, @operator(sensitivity));
	  }

	  public ZeroRateSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public ZeroRateSensitivity cloned()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ZeroRateSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ZeroRateSensitivity.Meta meta()
	  {
		return ZeroRateSensitivity.Meta.INSTANCE;
	  }

	  static ZeroRateSensitivity()
	  {
		MetaBean.register(ZeroRateSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ZeroRateSensitivity(Currency curveCurrency, double yearFraction, Currency currency, double sensitivity)
	  {
		JodaBeanUtils.notNull(curveCurrency, "curveCurrency");
		JodaBeanUtils.notNull(currency, "currency");
		this.curveCurrency = curveCurrency;
		this.yearFraction = yearFraction;
		this.currency = currency;
		this.sensitivity = sensitivity;
	  }

	  public override ZeroRateSensitivity.Meta metaBean()
	  {
		return ZeroRateSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the curve for which the sensitivity is computed. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency CurveCurrency
	  {
		  get
		  {
			return curveCurrency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time that was queried, expressed as a year fraction. </summary>
	  /// <returns> the value of the property </returns>
	  public double YearFraction
	  {
		  get
		  {
			return yearFraction;
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
		  ZeroRateSensitivity other = (ZeroRateSensitivity) obj;
		  return JodaBeanUtils.equal(curveCurrency, other.curveCurrency) && JodaBeanUtils.equal(yearFraction, other.yearFraction) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curveCurrency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yearFraction);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("ZeroRateSensitivity{");
		buf.Append("curveCurrency").Append('=').Append(curveCurrency).Append(',').Append(' ');
		buf.Append("yearFraction").Append('=').Append(yearFraction).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ZeroRateSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  curveCurrency_Renamed = DirectMetaProperty.ofImmutable(this, "curveCurrency", typeof(ZeroRateSensitivity), typeof(Currency));
			  yearFraction_Renamed = DirectMetaProperty.ofImmutable(this, "yearFraction", typeof(ZeroRateSensitivity), Double.TYPE);
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(ZeroRateSensitivity), typeof(Currency));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(ZeroRateSensitivity), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "curveCurrency", "yearFraction", "currency", "sensitivity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code curveCurrency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> curveCurrency_Renamed;
		/// <summary>
		/// The meta-property for the {@code yearFraction} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> yearFraction_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "curveCurrency", "yearFraction", "currency", "sensitivity");
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
			case 1303639584: // curveCurrency
			  return curveCurrency_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 564403871: // sensitivity
			  return sensitivity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ZeroRateSensitivity> builder()
		public override BeanBuilder<ZeroRateSensitivity> builder()
		{
		  return new ZeroRateSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ZeroRateSensitivity);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code curveCurrency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> curveCurrency()
		{
		  return curveCurrency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yearFraction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> yearFraction()
		{
		  return yearFraction_Renamed;
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
			case 1303639584: // curveCurrency
			  return ((ZeroRateSensitivity) bean).CurveCurrency;
			case -1731780257: // yearFraction
			  return ((ZeroRateSensitivity) bean).YearFraction;
			case 575402001: // currency
			  return ((ZeroRateSensitivity) bean).Currency;
			case 564403871: // sensitivity
			  return ((ZeroRateSensitivity) bean).Sensitivity;
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
	  /// The bean-builder for {@code ZeroRateSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ZeroRateSensitivity>
	  {

		internal Currency curveCurrency;
		internal double yearFraction;
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
			case 1303639584: // curveCurrency
			  return curveCurrency;
			case -1731780257: // yearFraction
			  return yearFraction;
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
			case 1303639584: // curveCurrency
			  this.curveCurrency = (Currency) newValue;
			  break;
			case -1731780257: // yearFraction
			  this.yearFraction = (double?) newValue.Value;
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

		public override ZeroRateSensitivity build()
		{
		  return new ZeroRateSensitivity(curveCurrency, yearFraction, currency, sensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ZeroRateSensitivity.Builder{");
		  buf.Append("curveCurrency").Append('=').Append(JodaBeanUtils.ToString(curveCurrency)).Append(',').Append(' ');
		  buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}