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
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using Messages = com.opengamma.strata.collect.Messages;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Point sensitivity to a forward rate of an FX rate for a currency pair.
	/// <para>
	/// Holds the sensitivity to the curves associated with <seealso cref="CurrencyPair"/> at a reference date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FxForwardSensitivity implements com.opengamma.strata.market.sensitivity.PointSensitivity, com.opengamma.strata.market.sensitivity.PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxForwardSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyPair currencyPair;
		private readonly CurrencyPair currencyPair;
	  /// <summary>
	  /// The reference currency.
	  /// <para>
	  /// This is the base currency of the FX conversion that occurs using the currency pair.
	  /// The reference currency must be one of the two currencies of the currency pair.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency referenceCurrency;
	  private readonly Currency referenceCurrency;
	  /// <summary>
	  /// The date to query the rate for.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate referenceDate;
	  private readonly LocalDate referenceDate;
	  /// <summary>
	  /// The currency of the sensitivity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The value of the sensitivity.
	  /// This is the amount that is converted from the base currency to the counter currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final double sensitivity;
	  private readonly double sensitivity;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from currency pair, reference currency, reference date and sensitivity value.
	  /// <para>
	  /// The sensitivity currency is defaulted to be a currency of the currency pair that is not the reference currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="referenceCurrency">  the reference currency </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static FxForwardSensitivity of(CurrencyPair currencyPair, Currency referenceCurrency, LocalDate referenceDate, double sensitivity)
	  {
		bool inverse = referenceCurrency.Equals(currencyPair.Counter);
		CurrencyPair pair = inverse ? currencyPair.inverse() : currencyPair;
		Currency sensitivityCurrency = pair.Counter;
		return new FxForwardSensitivity(currencyPair, referenceCurrency, referenceDate, sensitivityCurrency, sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance from currency pair, reference currency, reference date
	  /// sensitivity currency and sensitivity value.
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="referenceCurrency">  the reference currency </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static FxForwardSensitivity of(CurrencyPair currencyPair, Currency referenceCurrency, LocalDate referenceDate, Currency sensitivityCurrency, double sensitivity)
	  {
		return new FxForwardSensitivity(currencyPair, referenceCurrency, referenceDate, sensitivityCurrency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (!currencyPair.contains(referenceCurrency))
		{
		  throw new System.ArgumentException(Messages.format("Reference currency {} must be one of those in the currency pair {}", referenceCurrency, currencyPair));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency counter to the reference currency.
	  /// <para>
	  /// The currency pair contains two currencies. One is the reference currency.
	  /// This method returns the other.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the counter currency </returns>
	  public Currency ReferenceCounterCurrency
	  {
		  get
		  {
			bool inverse = referenceCurrency.Equals(currencyPair.Base);
			return inverse ? currencyPair.Counter : currencyPair.Base;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public FxForwardSensitivity withCurrency(Currency currency)
	  {
		if (this.currency.Equals(currency))
		{
		  return this;
		}
		return new FxForwardSensitivity(currencyPair, referenceCurrency, referenceDate, currency, sensitivity);
	  }

	  public FxForwardSensitivity withSensitivity(double sensitivity)
	  {
		return new FxForwardSensitivity(currencyPair, referenceCurrency, referenceDate, currency, sensitivity);
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is FxForwardSensitivity)
		{
		  FxForwardSensitivity otherFx = (FxForwardSensitivity) other;
		  return ComparisonChain.start().compare(currencyPair.ToString(), otherFx.currencyPair.ToString()).compare(currency, otherFx.currency).compare(referenceCurrency, otherFx.referenceCurrency).compare(referenceDate, otherFx.referenceDate).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override FxForwardSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return (FxForwardSensitivity) PointSensitivity.this.convertedTo(resultCurrency, rateProvider);
	  }

	  //-------------------------------------------------------------------------
	  public override FxForwardSensitivity multipliedBy(double factor)
	  {
		return new FxForwardSensitivity(currencyPair, referenceCurrency, referenceDate, currency, sensitivity * factor);
	  }

	  public FxForwardSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new FxForwardSensitivity(currencyPair, referenceCurrency, referenceDate, currency, @operator(sensitivity));
	  }

	  public FxForwardSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public FxForwardSensitivity cloned()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxForwardSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxForwardSensitivity.Meta meta()
	  {
		return FxForwardSensitivity.Meta.INSTANCE;
	  }

	  static FxForwardSensitivity()
	  {
		MetaBean.register(FxForwardSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FxForwardSensitivity(CurrencyPair currencyPair, Currency referenceCurrency, LocalDate referenceDate, Currency currency, double sensitivity)
	  {
		JodaBeanUtils.notNull(currencyPair, "currencyPair");
		JodaBeanUtils.notNull(referenceCurrency, "referenceCurrency");
		JodaBeanUtils.notNull(referenceDate, "referenceDate");
		JodaBeanUtils.notNull(currency, "currency");
		this.currencyPair = currencyPair;
		this.referenceCurrency = referenceCurrency;
		this.referenceDate = referenceDate;
		this.currency = currency;
		this.sensitivity = sensitivity;
		validate();
	  }

	  public override FxForwardSensitivity.Meta metaBean()
	  {
		return FxForwardSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair for which the sensitivity is computed. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return currencyPair;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reference currency.
	  /// <para>
	  /// This is the base currency of the FX conversion that occurs using the currency pair.
	  /// The reference currency must be one of the two currencies of the currency pair.
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
	  /// Gets the date to query the rate for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ReferenceDate
	  {
		  get
		  {
			return referenceDate;
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
	  /// Gets the value of the sensitivity.
	  /// This is the amount that is converted from the base currency to the counter currency. </summary>
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
		  FxForwardSensitivity other = (FxForwardSensitivity) obj;
		  return JodaBeanUtils.equal(currencyPair, other.currencyPair) && JodaBeanUtils.equal(referenceCurrency, other.referenceCurrency) && JodaBeanUtils.equal(referenceDate, other.referenceDate) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencyPair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(referenceCurrency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(referenceDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("FxForwardSensitivity{");
		buf.Append("currencyPair").Append('=').Append(currencyPair).Append(',').Append(' ');
		buf.Append("referenceCurrency").Append('=').Append(referenceCurrency).Append(',').Append(' ');
		buf.Append("referenceDate").Append('=').Append(referenceDate).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxForwardSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(FxForwardSensitivity), typeof(CurrencyPair));
			  referenceCurrency_Renamed = DirectMetaProperty.ofImmutable(this, "referenceCurrency", typeof(FxForwardSensitivity), typeof(Currency));
			  referenceDate_Renamed = DirectMetaProperty.ofImmutable(this, "referenceDate", typeof(FxForwardSensitivity), typeof(LocalDate));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(FxForwardSensitivity), typeof(Currency));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(FxForwardSensitivity), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currencyPair", "referenceCurrency", "referenceDate", "currency", "sensitivity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code currencyPair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> currencyPair_Renamed;
		/// <summary>
		/// The meta-property for the {@code referenceCurrency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> referenceCurrency_Renamed;
		/// <summary>
		/// The meta-property for the {@code referenceDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> referenceDate_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currencyPair", "referenceCurrency", "referenceDate", "currency", "sensitivity");
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
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 727652476: // referenceCurrency
			  return referenceCurrency_Renamed;
			case 1600456089: // referenceDate
			  return referenceDate_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 564403871: // sensitivity
			  return sensitivity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxForwardSensitivity> builder()
		public override BeanBuilder<FxForwardSensitivity> builder()
		{
		  return new FxForwardSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxForwardSensitivity);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code currencyPair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> currencyPair()
		{
		  return currencyPair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code referenceCurrency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> referenceCurrency()
		{
		  return referenceCurrency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code referenceDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> referenceDate()
		{
		  return referenceDate_Renamed;
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
			case 1005147787: // currencyPair
			  return ((FxForwardSensitivity) bean).CurrencyPair;
			case 727652476: // referenceCurrency
			  return ((FxForwardSensitivity) bean).ReferenceCurrency;
			case 1600456089: // referenceDate
			  return ((FxForwardSensitivity) bean).ReferenceDate;
			case 575402001: // currency
			  return ((FxForwardSensitivity) bean).Currency;
			case 564403871: // sensitivity
			  return ((FxForwardSensitivity) bean).Sensitivity;
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
	  /// The bean-builder for {@code FxForwardSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxForwardSensitivity>
	  {

		internal CurrencyPair currencyPair;
		internal Currency referenceCurrency;
		internal LocalDate referenceDate;
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
			case 1005147787: // currencyPair
			  return currencyPair;
			case 727652476: // referenceCurrency
			  return referenceCurrency;
			case 1600456089: // referenceDate
			  return referenceDate;
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
			case 1005147787: // currencyPair
			  this.currencyPair = (CurrencyPair) newValue;
			  break;
			case 727652476: // referenceCurrency
			  this.referenceCurrency = (Currency) newValue;
			  break;
			case 1600456089: // referenceDate
			  this.referenceDate = (LocalDate) newValue;
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

		public override FxForwardSensitivity build()
		{
		  return new FxForwardSensitivity(currencyPair, referenceCurrency, referenceDate, currency, sensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("FxForwardSensitivity.Builder{");
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair)).Append(',').Append(' ');
		  buf.Append("referenceCurrency").Append('=').Append(JodaBeanUtils.ToString(referenceCurrency)).Append(',').Append(' ');
		  buf.Append("referenceDate").Append('=').Append(JodaBeanUtils.ToString(referenceDate)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}