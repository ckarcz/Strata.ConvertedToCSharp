using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A template for creating cross-currency Ibor-Ibor swap trades.
	/// <para>
	/// This defines almost all the data necessary to create a Ibor-Ibor cross-currency <seealso cref="SwapTrade"/>.
	/// The trade date, notional and spread are required to complete the template and create the trade.
	/// As such, it is often possible to get a market price for a trade based on the template.
	/// The market price is typically quoted as a spread on one leg.
	/// </para>
	/// <para>
	/// The template references four dates.
	/// <ul>
	/// <li>Trade date, the date that the trade is agreed
	/// <li>Spot date, the base for date calculations, typically 2 business days after the trade date
	/// <li>Start date, the date on which accrual starts
	/// <li>End date, the date on which accrual ends
	/// </ul>
	/// Some of these dates are specified by the convention embedded within this template.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class XCcyIborIborSwapTemplate implements com.opengamma.strata.product.TradeTemplate, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class XCcyIborIborSwapTemplate : TradeTemplate, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.Period periodToStart;
		private readonly Period periodToStart;
	  /// <summary>
	  /// The tenor of the swap.
	  /// <para>
	  /// This is the period from the first accrual date to the last accrual date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.Tenor tenor;
	  private readonly Tenor tenor;
	  /// <summary>
	  /// The market convention of the swap.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final XCcyIborIborSwapConvention convention;
	  private readonly XCcyIborIborSwapConvention convention;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isFalse(periodToStart.Negative, "Period to start must not be negative");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a template based on the specified tenor and convention.
	  /// <para>
	  /// The swap will start on the spot date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tenor">  the tenor of the swap </param>
	  /// <param name="convention">  the market convention </param>
	  /// <returns> the template </returns>
	  public static XCcyIborIborSwapTemplate of(Tenor tenor, XCcyIborIborSwapConvention convention)
	  {
		return of(Period.ZERO, tenor, convention);
	  }

	  /// <summary>
	  /// Obtains a template based on the specified period, tenor and convention.
	  /// <para>
	  /// The period from the spot date to the start date is specified.
	  /// The tenor from the start date to the end date is also specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="periodToStart">  the period between the spot date and the start date </param>
	  /// <param name="tenor">  the tenor of the swap </param>
	  /// <param name="convention">  the market convention </param>
	  /// <returns> the template </returns>
	  public static XCcyIborIborSwapTemplate of(Period periodToStart, Tenor tenor, XCcyIborIborSwapConvention convention)
	  {
		return XCcyIborIborSwapTemplate.builder().periodToStart(periodToStart).tenor(tenor).convention(convention).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair of the template.
	  /// </summary>
	  /// <returns> the currency pair </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return convention.CurrencyPair;
		  }
	  }

	  /// <summary>
	  /// Creates a trade based on this template.
	  /// <para>
	  /// This returns a trade based on the specified trade date.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the swap, the rate of the flat leg is received, with the rate of the spread leg being paid.
	  /// If selling the swap, the opposite occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notionalSpreadLeg">  the notional amount for the spread leg </param>
	  /// <param name="notionalFlatLeg">  the notional amount for the flat leg </param>
	  /// <param name="spread">  the spread, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  public SwapTrade createTrade(LocalDate tradeDate, BuySell buySell, double notionalSpreadLeg, double notionalFlatLeg, double spread, ReferenceData refData)
	  {

		return convention.createTrade(tradeDate, periodToStart, tenor, buySell, notionalSpreadLeg, notionalFlatLeg, spread, refData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code XCcyIborIborSwapTemplate}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static XCcyIborIborSwapTemplate.Meta meta()
	  {
		return XCcyIborIborSwapTemplate.Meta.INSTANCE;
	  }

	  static XCcyIborIborSwapTemplate()
	  {
		MetaBean.register(XCcyIborIborSwapTemplate.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static XCcyIborIborSwapTemplate.Builder builder()
	  {
		return new XCcyIborIborSwapTemplate.Builder();
	  }

	  private XCcyIborIborSwapTemplate(Period periodToStart, Tenor tenor, XCcyIborIborSwapConvention convention)
	  {
		JodaBeanUtils.notNull(periodToStart, "periodToStart");
		JodaBeanUtils.notNull(tenor, "tenor");
		JodaBeanUtils.notNull(convention, "convention");
		this.periodToStart = periodToStart;
		this.tenor = tenor;
		this.convention = convention;
		validate();
	  }

	  public override XCcyIborIborSwapTemplate.Meta metaBean()
	  {
		return XCcyIborIborSwapTemplate.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the period between the spot value date and the start date.
	  /// <para>
	  /// This is often zero, but can be greater if the swap if <i>forward starting</i>.
	  /// This must not be negative.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Period PeriodToStart
	  {
		  get
		  {
			return periodToStart;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the tenor of the swap.
	  /// <para>
	  /// This is the period from the first accrual date to the last accrual date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Tenor Tenor
	  {
		  get
		  {
			return tenor;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market convention of the swap. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public XCcyIborIborSwapConvention Convention
	  {
		  get
		  {
			return convention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  XCcyIborIborSwapTemplate other = (XCcyIborIborSwapTemplate) obj;
		  return JodaBeanUtils.equal(periodToStart, other.periodToStart) && JodaBeanUtils.equal(tenor, other.tenor) && JodaBeanUtils.equal(convention, other.convention);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periodToStart);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tenor);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(convention);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("XCcyIborIborSwapTemplate{");
		buf.Append("periodToStart").Append('=').Append(periodToStart).Append(',').Append(' ');
		buf.Append("tenor").Append('=').Append(tenor).Append(',').Append(' ');
		buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code XCcyIborIborSwapTemplate}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  periodToStart_Renamed = DirectMetaProperty.ofImmutable(this, "periodToStart", typeof(XCcyIborIborSwapTemplate), typeof(Period));
			  tenor_Renamed = DirectMetaProperty.ofImmutable(this, "tenor", typeof(XCcyIborIborSwapTemplate), typeof(Tenor));
			  convention_Renamed = DirectMetaProperty.ofImmutable(this, "convention", typeof(XCcyIborIborSwapTemplate), typeof(XCcyIborIborSwapConvention));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "periodToStart", "tenor", "convention");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code periodToStart} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Period> periodToStart_Renamed;
		/// <summary>
		/// The meta-property for the {@code tenor} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Tenor> tenor_Renamed;
		/// <summary>
		/// The meta-property for the {@code convention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<XCcyIborIborSwapConvention> convention_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "periodToStart", "tenor", "convention");
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
			case -574688858: // periodToStart
			  return periodToStart_Renamed;
			case 110246592: // tenor
			  return tenor_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override XCcyIborIborSwapTemplate.Builder builder()
		{
		  return new XCcyIborIborSwapTemplate.Builder();
		}

		public override Type beanType()
		{
		  return typeof(XCcyIborIborSwapTemplate);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code periodToStart} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Period> periodToStart()
		{
		  return periodToStart_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code tenor} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Tenor> tenor()
		{
		  return tenor_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code convention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<XCcyIborIborSwapConvention> convention()
		{
		  return convention_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -574688858: // periodToStart
			  return ((XCcyIborIborSwapTemplate) bean).PeriodToStart;
			case 110246592: // tenor
			  return ((XCcyIborIborSwapTemplate) bean).Tenor;
			case 2039569265: // convention
			  return ((XCcyIborIborSwapTemplate) bean).Convention;
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
	  /// The bean-builder for {@code XCcyIborIborSwapTemplate}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<XCcyIborIborSwapTemplate>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Period periodToStart_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Tenor tenor_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal XCcyIborIborSwapConvention convention_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(XCcyIborIborSwapTemplate beanToCopy)
		{
		  this.periodToStart_Renamed = beanToCopy.PeriodToStart;
		  this.tenor_Renamed = beanToCopy.Tenor;
		  this.convention_Renamed = beanToCopy.Convention;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -574688858: // periodToStart
			  return periodToStart_Renamed;
			case 110246592: // tenor
			  return tenor_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -574688858: // periodToStart
			  this.periodToStart_Renamed = (Period) newValue;
			  break;
			case 110246592: // tenor
			  this.tenor_Renamed = (Tenor) newValue;
			  break;
			case 2039569265: // convention
			  this.convention_Renamed = (XCcyIborIborSwapConvention) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override XCcyIborIborSwapTemplate build()
		{
		  return new XCcyIborIborSwapTemplate(periodToStart_Renamed, tenor_Renamed, convention_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the period between the spot value date and the start date.
		/// <para>
		/// This is often zero, but can be greater if the swap if <i>forward starting</i>.
		/// This must not be negative.
		/// </para>
		/// </summary>
		/// <param name="periodToStart">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periodToStart(Period periodToStart)
		{
		  JodaBeanUtils.notNull(periodToStart, "periodToStart");
		  this.periodToStart_Renamed = periodToStart;
		  return this;
		}

		/// <summary>
		/// Sets the tenor of the swap.
		/// <para>
		/// This is the period from the first accrual date to the last accrual date.
		/// </para>
		/// </summary>
		/// <param name="tenor">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder tenor(Tenor tenor)
		{
		  JodaBeanUtils.notNull(tenor, "tenor");
		  this.tenor_Renamed = tenor;
		  return this;
		}

		/// <summary>
		/// Sets the market convention of the swap. </summary>
		/// <param name="convention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder convention(XCcyIborIborSwapConvention convention)
		{
		  JodaBeanUtils.notNull(convention, "convention");
		  this.convention_Renamed = convention;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("XCcyIborIborSwapTemplate.Builder{");
		  buf.Append("periodToStart").Append('=').Append(JodaBeanUtils.ToString(periodToStart_Renamed)).Append(',').Append(' ');
		  buf.Append("tenor").Append('=').Append(JodaBeanUtils.ToString(tenor_Renamed)).Append(',').Append(' ');
		  buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}