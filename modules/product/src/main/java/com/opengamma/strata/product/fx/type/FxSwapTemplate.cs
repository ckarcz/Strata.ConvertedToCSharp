using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx.type
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
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A template for creating an FX swap trade.
	/// <para>
	/// This defines almost all the data necessary to create a <seealso cref="FxSwapTrade"/>.
	/// The trade date, notional, FX rate and forward points are required to complete the template and create the trade.
	/// As such, it is often possible to get a market price for a trade based on the template.
	/// </para>
	/// <para>
	/// The convention is defined by four dates.
	/// <ul>
	/// <li>Trade date, the date that the trade is agreed
	/// <li>Spot date, the base for date calculations, typically 2 business days in the
	///  joint calendar of both currencies after the trade date
	/// <li>Near date, the date on which the near leg of the swap is exchanged,
	///  typically equal to the spot date
	/// <li>Far date, the date on which the far leg of the swap is exchanged,
	///  typically a number of months or years after the spot date
	/// </ul>
	/// Some of these dates are specified by the convention embedded within this template.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FxSwapTemplate implements com.opengamma.strata.product.TradeTemplate, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxSwapTemplate : TradeTemplate, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.Period periodToNear;
		private readonly Period periodToNear;
	  /// <summary>
	  /// The period between the spot value date and the far date.
	  /// <para>
	  /// For example, a '3M x 6M' FX swap has a period from spot to the far date of 6 months
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.Period periodToFar;
	  private readonly Period periodToFar;
	  /// <summary>
	  /// The underlying FX Swap convention.
	  /// <para>
	  /// This specifies the market convention of the FX Swap to be created.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FxSwapConvention convention;
	  private readonly FxSwapConvention convention;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a template based on the specified period and convention.
	  /// <para>
	  /// The near date is equal to the spot date.
	  /// The period from the spot date to the far date is specified
	  /// </para>
	  /// <para>
	  /// For example, a '6M' FX swap has a near leg on the spot date and a period from spot to the far date of 6 months
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="periodToFar">  the period between the spot date and the far date </param>
	  /// <param name="convention">  the market convention </param>
	  /// <returns> the template </returns>
	  public static FxSwapTemplate of(Period periodToFar, FxSwapConvention convention)
	  {
		return FxSwapTemplate.builder().periodToNear(Period.ZERO).periodToFar(periodToFar).convention(convention).build();
	  }

	  /// <summary>
	  /// Obtains a template based on the specified periods and convention.
	  /// <para>
	  /// Both the period from the spot date to the near date and far date are specified.
	  /// </para>
	  /// <para>
	  /// For example, a '3M x 6M' FX swap has a period from spot to the start date of 3 months and 
	  /// a period from spot to the far date of 6 months
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="periodToNear">  the period between the spot date and the near date </param>
	  /// <param name="periodToFar">  the period between the spot date and the far date </param>
	  /// <param name="convention">  the market convention </param>
	  /// <returns> the template </returns>
	  public static FxSwapTemplate of(Period periodToNear, Period periodToFar, FxSwapConvention convention)
	  {
		return FxSwapTemplate.builder().periodToNear(periodToNear).periodToFar(periodToFar).convention(convention).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isFalse(periodToNear.Negative, "Period to start must not be negative");
		ArgChecker.isFalse(periodToFar.Negative, "Period to end must not be negative");
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
	  /// This returns a trade based on the specified date.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the FX Swap, the amount in the first currency of the pair is received
	  /// in the near leg and paid in the far leg, while the second currency is paid in the
	  /// near leg and received in the far leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the first currency of the currency pair </param>
	  /// <param name="nearFxRate">  the FX rate for the near leg </param>
	  /// <param name="forwardPoints">  the FX points to be added to the FX rate at the far leg </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  public FxSwapTrade createTrade(LocalDate tradeDate, BuySell buySell, double notional, double nearFxRate, double forwardPoints, ReferenceData refData)
	  {

		return convention.createTrade(tradeDate, periodToNear, periodToFar, buySell, notional, nearFxRate, forwardPoints, refData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSwapTemplate}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxSwapTemplate.Meta meta()
	  {
		return FxSwapTemplate.Meta.INSTANCE;
	  }

	  static FxSwapTemplate()
	  {
		MetaBean.register(FxSwapTemplate.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FxSwapTemplate.Builder builder()
	  {
		return new FxSwapTemplate.Builder();
	  }

	  private FxSwapTemplate(Period periodToNear, Period periodToFar, FxSwapConvention convention)
	  {
		JodaBeanUtils.notNull(periodToNear, "periodToNear");
		JodaBeanUtils.notNull(periodToFar, "periodToFar");
		JodaBeanUtils.notNull(convention, "convention");
		this.periodToNear = periodToNear;
		this.periodToFar = periodToFar;
		this.convention = convention;
		validate();
	  }

	  public override FxSwapTemplate.Meta metaBean()
	  {
		return FxSwapTemplate.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the period between the spot value date and the near date.
	  /// <para>
	  /// For example, a '3M x 6M' FX swap has a period from spot to the near date of 3 months
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Period PeriodToNear
	  {
		  get
		  {
			return periodToNear;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the period between the spot value date and the far date.
	  /// <para>
	  /// For example, a '3M x 6M' FX swap has a period from spot to the far date of 6 months
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Period PeriodToFar
	  {
		  get
		  {
			return periodToFar;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying FX Swap convention.
	  /// <para>
	  /// This specifies the market convention of the FX Swap to be created.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxSwapConvention Convention
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
		  FxSwapTemplate other = (FxSwapTemplate) obj;
		  return JodaBeanUtils.equal(periodToNear, other.periodToNear) && JodaBeanUtils.equal(periodToFar, other.periodToFar) && JodaBeanUtils.equal(convention, other.convention);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periodToNear);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periodToFar);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(convention);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("FxSwapTemplate{");
		buf.Append("periodToNear").Append('=').Append(periodToNear).Append(',').Append(' ');
		buf.Append("periodToFar").Append('=').Append(periodToFar).Append(',').Append(' ');
		buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSwapTemplate}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  periodToNear_Renamed = DirectMetaProperty.ofImmutable(this, "periodToNear", typeof(FxSwapTemplate), typeof(Period));
			  periodToFar_Renamed = DirectMetaProperty.ofImmutable(this, "periodToFar", typeof(FxSwapTemplate), typeof(Period));
			  convention_Renamed = DirectMetaProperty.ofImmutable(this, "convention", typeof(FxSwapTemplate), typeof(FxSwapConvention));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "periodToNear", "periodToFar", "convention");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code periodToNear} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Period> periodToNear_Renamed;
		/// <summary>
		/// The meta-property for the {@code periodToFar} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Period> periodToFar_Renamed;
		/// <summary>
		/// The meta-property for the {@code convention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxSwapConvention> convention_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "periodToNear", "periodToFar", "convention");
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
			case -18701724: // periodToNear
			  return periodToNear_Renamed;
			case -970442405: // periodToFar
			  return periodToFar_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FxSwapTemplate.Builder builder()
		{
		  return new FxSwapTemplate.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxSwapTemplate);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code periodToNear} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Period> periodToNear()
		{
		  return periodToNear_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code periodToFar} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Period> periodToFar()
		{
		  return periodToFar_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code convention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxSwapConvention> convention()
		{
		  return convention_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -18701724: // periodToNear
			  return ((FxSwapTemplate) bean).PeriodToNear;
			case -970442405: // periodToFar
			  return ((FxSwapTemplate) bean).PeriodToFar;
			case 2039569265: // convention
			  return ((FxSwapTemplate) bean).Convention;
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
	  /// The bean-builder for {@code FxSwapTemplate}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FxSwapTemplate>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Period periodToNear_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Period periodToFar_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxSwapConvention convention_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(FxSwapTemplate beanToCopy)
		{
		  this.periodToNear_Renamed = beanToCopy.PeriodToNear;
		  this.periodToFar_Renamed = beanToCopy.PeriodToFar;
		  this.convention_Renamed = beanToCopy.Convention;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -18701724: // periodToNear
			  return periodToNear_Renamed;
			case -970442405: // periodToFar
			  return periodToFar_Renamed;
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
			case -18701724: // periodToNear
			  this.periodToNear_Renamed = (Period) newValue;
			  break;
			case -970442405: // periodToFar
			  this.periodToFar_Renamed = (Period) newValue;
			  break;
			case 2039569265: // convention
			  this.convention_Renamed = (FxSwapConvention) newValue;
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

		public override FxSwapTemplate build()
		{
		  return new FxSwapTemplate(periodToNear_Renamed, periodToFar_Renamed, convention_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the period between the spot value date and the near date.
		/// <para>
		/// For example, a '3M x 6M' FX swap has a period from spot to the near date of 3 months
		/// </para>
		/// </summary>
		/// <param name="periodToNear">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periodToNear(Period periodToNear)
		{
		  JodaBeanUtils.notNull(periodToNear, "periodToNear");
		  this.periodToNear_Renamed = periodToNear;
		  return this;
		}

		/// <summary>
		/// Sets the period between the spot value date and the far date.
		/// <para>
		/// For example, a '3M x 6M' FX swap has a period from spot to the far date of 6 months
		/// </para>
		/// </summary>
		/// <param name="periodToFar">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periodToFar(Period periodToFar)
		{
		  JodaBeanUtils.notNull(periodToFar, "periodToFar");
		  this.periodToFar_Renamed = periodToFar;
		  return this;
		}

		/// <summary>
		/// Sets the underlying FX Swap convention.
		/// <para>
		/// This specifies the market convention of the FX Swap to be created.
		/// </para>
		/// </summary>
		/// <param name="convention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder convention(FxSwapConvention convention)
		{
		  JodaBeanUtils.notNull(convention, "convention");
		  this.convention_Renamed = convention;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("FxSwapTemplate.Builder{");
		  buf.Append("periodToNear").Append('=').Append(JodaBeanUtils.ToString(periodToNear_Renamed)).Append(',').Append(' ');
		  buf.Append("periodToFar").Append('=').Append(JodaBeanUtils.ToString(periodToFar_Renamed)).Append(',').Append(' ');
		  buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}