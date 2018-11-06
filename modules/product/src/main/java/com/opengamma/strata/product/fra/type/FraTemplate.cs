using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra.type
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A template for creating a forward rate agreement (FRA) trade.
	/// <para>
	/// This defines almost all the data necessary to create a <seealso cref="FraTrade"/>.
	/// The trade date, notional and fixed rate are required to complete the template and create the trade.
	/// As such, it is often possible to get a market price for a trade based on the template.
	/// The market price is typically quoted as a bid/ask on the fixed rate.
	/// </para>
	/// <para>
	/// The template is defined by six dates.
	/// <ul>
	/// <li>Trade date, the date that the trade is agreed
	/// <li>Spot date, the base for date calculations, typically 2 business days after the trade date
	/// <li>Start date, the date on which the implied deposit starts, typically a number of months after the spot value date
	/// <li>End date, the date on which the implied deposit ends, typically a number of months after the spot value date
	/// <li>Fixing date, the date on which the index is to be observed, typically 2 business days before the start date
	/// <li>Payment date, the date on which payment is made, typically the same as the start date
	/// </ul>
	/// Some of these dates are specified by the convention embedded within this template.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FraTemplate implements com.opengamma.strata.product.TradeTemplate, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FraTemplate : TradeTemplate, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.Period periodToStart;
		private readonly Period periodToStart;
	  /// <summary>
	  /// The period between the spot value date and the end date.
	  /// <para>
	  /// In a FRA described as '2 x 5', the period to the end date is 5 months.
	  /// The difference between the start date and the end date typically matches the tenor of the index,
	  /// however this is not validated.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the period to start plus the tenor of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.Period periodToEnd;
	  private readonly Period periodToEnd;
	  /// <summary>
	  /// The underlying FRA convention.
	  /// <para>
	  /// This specifies the market convention of the FRA to be created.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FraConvention convention;
	  private readonly FraConvention convention;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.periodToEnd_Renamed == null && builder.convention_Renamed != null && builder.periodToStart_Renamed != null)
		{
		  builder.periodToEnd_Renamed = builder.periodToStart_Renamed.plus(builder.convention_Renamed.Index.Tenor.Period);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isFalse(periodToStart.Negative, "Period to start must not be negative");
		ArgChecker.isFalse(periodToEnd.Negative, "Period to end must not be negative");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a template based on the specified period and index.
	  /// <para>
	  /// The period from the spot date to the start date is specified.
	  /// The period from the spot date to the end date will be the period to start
	  /// plus the tenor of the index.
	  /// </para>
	  /// <para>
	  /// For example, a '2 x 5' FRA has a period to the start date of 2 months.
	  /// The index will be a 3 month index, such as 'USD-LIBOR-3M'.
	  /// The period to the end date will be the period to the start date plus the index tenor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="periodToStart">  the period between the spot date and the start date </param>
	  /// <param name="index">  the index that defines the market convention </param>
	  /// <returns> the template </returns>
	  public static FraTemplate of(Period periodToStart, IborIndex index)
	  {
		return of(periodToStart, periodToStart.plus(index.Tenor.Period), FraConvention.of(index));
	  }

	  /// <summary>
	  /// Obtains a template based on the specified periods and convention.
	  /// <para>
	  /// The periods from the spot date to the start date and to the end date are specified.
	  /// </para>
	  /// <para>
	  /// For example, a '2 x 5' FRA has a period to the start date of 2 months and
	  /// a period to the end date of 5 months.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="periodToStart">  the period between the spot date and the start date </param>
	  /// <param name="periodToEnd">  the period between the spot date and the end date </param>
	  /// <param name="convention">  the market convention </param>
	  /// <returns> the template </returns>
	  public static FraTemplate of(Period periodToStart, Period periodToEnd, FraConvention convention)
	  {
		ArgChecker.notNull(periodToStart, "periodToStart");
		ArgChecker.notNull(periodToEnd, "periodToEnd");
		ArgChecker.notNull(convention, "convention");
		return FraTemplate.builder().periodToStart(periodToStart).periodToEnd(periodToEnd).convention(convention).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a trade based on this template.
	  /// <para>
	  /// This returns a trade based on the specified date.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the FRA, the floating rate is received from the counterparty, with the fixed rate being paid.
	  /// If selling the FRA, the floating rate is paid to the counterparty, with the fixed rate being received.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="buySell">  the buy/sell flag, see <seealso cref="Fra#getBuySell()"/> </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  public FraTrade createTrade(LocalDate tradeDate, BuySell buySell, double notional, double fixedRate, ReferenceData refData)
	  {

		return convention.createTrade(tradeDate, periodToStart, periodToEnd, buySell, notional, fixedRate, refData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FraTemplate}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FraTemplate.Meta meta()
	  {
		return FraTemplate.Meta.INSTANCE;
	  }

	  static FraTemplate()
	  {
		MetaBean.register(FraTemplate.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FraTemplate.Builder builder()
	  {
		return new FraTemplate.Builder();
	  }

	  private FraTemplate(Period periodToStart, Period periodToEnd, FraConvention convention)
	  {
		JodaBeanUtils.notNull(periodToStart, "periodToStart");
		JodaBeanUtils.notNull(periodToEnd, "periodToEnd");
		JodaBeanUtils.notNull(convention, "convention");
		this.periodToStart = periodToStart;
		this.periodToEnd = periodToEnd;
		this.convention = convention;
		validate();
	  }

	  public override FraTemplate.Meta metaBean()
	  {
		return FraTemplate.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the period between the spot value date and the start date.
	  /// <para>
	  /// In a FRA described as '2 x 5', the period to the start date is 2 months.
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
	  /// Gets the period between the spot value date and the end date.
	  /// <para>
	  /// In a FRA described as '2 x 5', the period to the end date is 5 months.
	  /// The difference between the start date and the end date typically matches the tenor of the index,
	  /// however this is not validated.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the period to start plus the tenor of the index if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Period PeriodToEnd
	  {
		  get
		  {
			return periodToEnd;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying FRA convention.
	  /// <para>
	  /// This specifies the market convention of the FRA to be created.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FraConvention Convention
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
		  FraTemplate other = (FraTemplate) obj;
		  return JodaBeanUtils.equal(periodToStart, other.periodToStart) && JodaBeanUtils.equal(periodToEnd, other.periodToEnd) && JodaBeanUtils.equal(convention, other.convention);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periodToStart);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periodToEnd);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(convention);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("FraTemplate{");
		buf.Append("periodToStart").Append('=').Append(periodToStart).Append(',').Append(' ');
		buf.Append("periodToEnd").Append('=').Append(periodToEnd).Append(',').Append(' ');
		buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FraTemplate}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  periodToStart_Renamed = DirectMetaProperty.ofImmutable(this, "periodToStart", typeof(FraTemplate), typeof(Period));
			  periodToEnd_Renamed = DirectMetaProperty.ofImmutable(this, "periodToEnd", typeof(FraTemplate), typeof(Period));
			  convention_Renamed = DirectMetaProperty.ofImmutable(this, "convention", typeof(FraTemplate), typeof(FraConvention));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "periodToStart", "periodToEnd", "convention");
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
		/// The meta-property for the {@code periodToEnd} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Period> periodToEnd_Renamed;
		/// <summary>
		/// The meta-property for the {@code convention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FraConvention> convention_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "periodToStart", "periodToEnd", "convention");
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
			case -970442977: // periodToEnd
			  return periodToEnd_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FraTemplate.Builder builder()
		{
		  return new FraTemplate.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FraTemplate);
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
		/// The meta-property for the {@code periodToEnd} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Period> periodToEnd()
		{
		  return periodToEnd_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code convention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FraConvention> convention()
		{
		  return convention_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -574688858: // periodToStart
			  return ((FraTemplate) bean).PeriodToStart;
			case -970442977: // periodToEnd
			  return ((FraTemplate) bean).PeriodToEnd;
			case 2039569265: // convention
			  return ((FraTemplate) bean).Convention;
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
	  /// The bean-builder for {@code FraTemplate}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FraTemplate>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Period periodToStart_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Period periodToEnd_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FraConvention convention_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(FraTemplate beanToCopy)
		{
		  this.periodToStart_Renamed = beanToCopy.PeriodToStart;
		  this.periodToEnd_Renamed = beanToCopy.PeriodToEnd;
		  this.convention_Renamed = beanToCopy.Convention;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -574688858: // periodToStart
			  return periodToStart_Renamed;
			case -970442977: // periodToEnd
			  return periodToEnd_Renamed;
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
			case -970442977: // periodToEnd
			  this.periodToEnd_Renamed = (Period) newValue;
			  break;
			case 2039569265: // convention
			  this.convention_Renamed = (FraConvention) newValue;
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

		public override FraTemplate build()
		{
		  preBuild(this);
		  return new FraTemplate(periodToStart_Renamed, periodToEnd_Renamed, convention_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the period between the spot value date and the start date.
		/// <para>
		/// In a FRA described as '2 x 5', the period to the start date is 2 months.
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
		/// Sets the period between the spot value date and the end date.
		/// <para>
		/// In a FRA described as '2 x 5', the period to the end date is 5 months.
		/// The difference between the start date and the end date typically matches the tenor of the index,
		/// however this is not validated.
		/// </para>
		/// <para>
		/// When building, this will default to the period to start plus the tenor of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="periodToEnd">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periodToEnd(Period periodToEnd)
		{
		  JodaBeanUtils.notNull(periodToEnd, "periodToEnd");
		  this.periodToEnd_Renamed = periodToEnd;
		  return this;
		}

		/// <summary>
		/// Sets the underlying FRA convention.
		/// <para>
		/// This specifies the market convention of the FRA to be created.
		/// </para>
		/// </summary>
		/// <param name="convention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder convention(FraConvention convention)
		{
		  JodaBeanUtils.notNull(convention, "convention");
		  this.convention_Renamed = convention;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("FraTemplate.Builder{");
		  buf.Append("periodToStart").Append('=').Append(JodaBeanUtils.ToString(periodToStart_Renamed)).Append(',').Append(' ');
		  buf.Append("periodToEnd").Append('=').Append(JodaBeanUtils.ToString(periodToEnd_Renamed)).Append(',').Append(' ');
		  buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}