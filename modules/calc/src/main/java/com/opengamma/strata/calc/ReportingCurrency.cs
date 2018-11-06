using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
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

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CalculationFunction = com.opengamma.strata.calc.runner.CalculationFunction;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// The reporting currency.
	/// <para>
	/// This is used to specify the currency that the result should be reporting in.
	/// The currency specified may be explicit, using <seealso cref="#of(Currency)"/>, or implicit
	/// using <seealso cref="#NATURAL"/>. The "natural" currency of a target is obtained from
	/// <seealso cref="CalculationFunction#naturalCurrency(CalculationTarget, ReferenceData)"/>.
	/// </para>
	/// <para>
	/// If the result is not associated with a currency, such as for "par rate", then the
	/// reporting currency will effectively be ignored.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ReportingCurrency implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ReportingCurrency : ImmutableBean
	{

	  /// <summary>
	  /// An instance requesting the "natural" currency of the target.
	  /// <para>
	  /// When converting calculation results, conversion will occur to the "natural" currency of the target.
	  /// The "natural" currency of a target is obtained
	  /// from <seealso cref="CalculationFunction#naturalCurrency(CalculationTarget, ReferenceData)"/>.
	  /// </para>
	  /// </summary>
	  public static readonly ReportingCurrency NATURAL = new ReportingCurrency(ReportingCurrencyType.NATURAL, null);
	  /// <summary>
	  /// An instance requesting no currency conversion.
	  /// <para>
	  /// Calculation results are normally converted to a single currency.
	  /// If this reporting currency is used, then no currency conversion will be performed.
	  /// </para>
	  /// </summary>
	  public static readonly ReportingCurrency NONE = new ReportingCurrency(ReportingCurrencyType.NONE, null);

	  /// <summary>
	  /// The type of reporting currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ReportingCurrencyType type;
	  private readonly ReportingCurrencyType type;
	  /// <summary>
	  /// The reporting currency.
	  /// <para>
	  /// This property will be set only if the type is 'Specific'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance requesting the specified currency.
	  /// <para>
	  /// When converting calculation results, conversion will occur to the specified currency.
	  /// This returns an instance with the type <seealso cref="ReportingCurrencyType#SPECIFIC"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the specific currency </param>
	  /// <returns> a reporting currency instance requesting the specified currency </returns>
	  public static ReportingCurrency of(Currency currency)
	  {
		return new ReportingCurrency(ReportingCurrencyType.SPECIFIC, currency);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the type is 'Specific'.
	  /// <para>
	  /// When converting calculation results, conversion will occur to the specific currency
	  /// returned by <seealso cref="#getCurrency()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if the type is 'Specific' </returns>
	  public bool Specific
	  {
		  get
		  {
			return (type == ReportingCurrencyType.SPECIFIC);
		  }
	  }

	  /// <summary>
	  /// Checks if the type is 'Natural'.
	  /// <para>
	  /// When converting calculation results, conversion will occur to the "natural" currency of the target.
	  /// The "natural" currency of a target is obtained
	  /// from <seealso cref="CalculationFunction#naturalCurrency(CalculationTarget, ReferenceData)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if the type is 'Natural' </returns>
	  public bool Natural
	  {
		  get
		  {
			return (type == ReportingCurrencyType.NATURAL);
		  }
	  }

	  /// <summary>
	  /// Checks if the type is 'None'.
	  /// <para>
	  /// Calculation results are normally converted to a single currency.
	  /// If this returns true than no currency conversion will be performed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if the type is 'None' </returns>
	  public bool None
	  {
		  get
		  {
			return (type == ReportingCurrencyType.NONE);
		  }
	  }

	  /// <summary>
	  /// Gets the currency if the type is 'Specific'.
	  /// <para>
	  /// If the type is 'Specific', this returns the currency.
	  /// Otherwise, this throws an exception.
	  /// As such, the type must be checked using #is
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency, only available if the type is 'Specific' </returns>
	  /// <exception cref="IllegalStateException"> if called on a failure result </exception>
	  public Currency Currency
	  {
		  get
		  {
			if (!Specific)
			{
			  throw new System.InvalidOperationException(Messages.format("No currency available for type '{}'", type));
			}
			return currency;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return type + (currency != null ? ":" + currency.ToString() : "");
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ReportingCurrency}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ReportingCurrency.Meta meta()
	  {
		return ReportingCurrency.Meta.INSTANCE;
	  }

	  static ReportingCurrency()
	  {
		MetaBean.register(ReportingCurrency.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ReportingCurrency(ReportingCurrencyType type, Currency currency)
	  {
		JodaBeanUtils.notNull(type, "type");
		this.type = type;
		this.currency = currency;
	  }

	  public override ReportingCurrency.Meta metaBean()
	  {
		return ReportingCurrency.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of reporting currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ReportingCurrencyType Type
	  {
		  get
		  {
			return type;
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
		  ReportingCurrency other = (ReportingCurrency) obj;
		  return JodaBeanUtils.equal(type, other.type) && JodaBeanUtils.equal(currency, other.currency);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(type);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ReportingCurrency}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  type_Renamed = DirectMetaProperty.ofImmutable(this, "type", typeof(ReportingCurrency), typeof(ReportingCurrencyType));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(ReportingCurrency), typeof(Currency));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "type", "currency");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code type} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ReportingCurrencyType> type_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "type", "currency");
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
			case 3575610: // type
			  return type_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ReportingCurrency> builder()
		public override BeanBuilder<ReportingCurrency> builder()
		{
		  return new ReportingCurrency.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ReportingCurrency);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code type} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ReportingCurrencyType> type()
		{
		  return type_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  return ((ReportingCurrency) bean).Type;
			case 575402001: // currency
			  return ((ReportingCurrency) bean).currency;
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
	  /// The bean-builder for {@code ReportingCurrency}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ReportingCurrency>
	  {

		internal ReportingCurrencyType type;
		internal Currency currency;

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
			case 3575610: // type
			  return type;
			case 575402001: // currency
			  return currency;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  this.type = (ReportingCurrencyType) newValue;
			  break;
			case 575402001: // currency
			  this.currency = (Currency) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ReportingCurrency build()
		{
		  return new ReportingCurrency(type, currency);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ReportingCurrency.Builder{");
		  buf.Append("type").Append('=').Append(JodaBeanUtils.ToString(type)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}