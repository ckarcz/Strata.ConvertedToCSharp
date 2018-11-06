using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A template for creating credit default swap trades.
	/// <para>
	/// This defines almost all the data necessary to create a credit default swap <seealso cref="CdsTrade"/>.
	/// The start and end of the trade are specified by {@code LocalDate}.
	/// Use <seealso cref="TenorCdsTemplate"/> for standard CDS trades.
	/// </para>
	/// <para>
	/// The legal entity ID, trade date, notional and fixed rate are required to complete the template and create the trade.
	/// As such, it is often possible to get a market quote for a trade based on the template.
	/// </para>
	/// <para>
	/// A CDS is quoted in points upfront, par spread, or quoted spread. 
	/// For the latter two cases, the market quotes are passed as the fixed rate.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class DatesCdsTemplate implements CdsTemplate, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DatesCdsTemplate : CdsTemplate, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
		private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date.
	  /// <para>
	  /// The end date of the underling CDS product.
	  /// This date can be modified following the rule in {@code convention}. 
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The market convention of the credit default swap.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CdsConvention convention;
	  private readonly CdsConvention convention;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a template based on the specified dates and convention.
	  /// </summary>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="convention">  the convention </param>
	  /// <returns> the template </returns>
	  public static DatesCdsTemplate of(LocalDate startDate, LocalDate endDate, CdsConvention convention)
	  {
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
		return new DatesCdsTemplate(startDate, endDate, convention);
	  }

	  //-------------------------------------------------------------------------
	  public CdsTrade createTrade(StandardId legalEntityId, LocalDate tradeDate, BuySell buySell, double notional, double fixedRate, ReferenceData refData)
	  {

		ArgChecker.inOrderNotEqual(tradeDate, endDate, "tradeDate", "endDate");
		return convention.createTrade(legalEntityId, tradeDate, startDate, endDate, buySell, notional, fixedRate, refData);
	  }

	  public CdsTrade createTrade(StandardId legalEntityId, LocalDate tradeDate, BuySell buySell, double notional, double fixedRate, AdjustablePayment upFrontFee, ReferenceData refData)
	  {

		ArgChecker.inOrderNotEqual(tradeDate, endDate, "tradeDate", "endDate");
		return convention.createTrade(legalEntityId, tradeDate, startDate, endDate, buySell, notional, fixedRate, upFrontFee, refData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DatesCdsTemplate}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DatesCdsTemplate.Meta meta()
	  {
		return DatesCdsTemplate.Meta.INSTANCE;
	  }

	  static DatesCdsTemplate()
	  {
		MetaBean.register(DatesCdsTemplate.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DatesCdsTemplate(LocalDate startDate, LocalDate endDate, CdsConvention convention)
	  {
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		JodaBeanUtils.notNull(convention, "convention");
		this.startDate = startDate;
		this.endDate = endDate;
		this.convention = convention;
	  }

	  public override DatesCdsTemplate.Meta metaBean()
	  {
		return DatesCdsTemplate.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date.
	  /// <para>
	  /// The start date of the underling CDS product.
	  /// This date can be modified following the rule in {@code convention}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the end date.
	  /// <para>
	  /// The end date of the underling CDS product.
	  /// This date can be modified following the rule in {@code convention}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return endDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market convention of the credit default swap. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CdsConvention Convention
	  {
		  get
		  {
			return convention;
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
		  DatesCdsTemplate other = (DatesCdsTemplate) obj;
		  return JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(convention, other.convention);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(convention);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("DatesCdsTemplate{");
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DatesCdsTemplate}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(DatesCdsTemplate), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(DatesCdsTemplate), typeof(LocalDate));
			  convention_Renamed = DirectMetaProperty.ofImmutable(this, "convention", typeof(DatesCdsTemplate), typeof(CdsConvention));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "startDate", "endDate", "convention");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code convention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CdsConvention> convention_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "startDate", "endDate", "convention");
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
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends DatesCdsTemplate> builder()
		public override BeanBuilder<DatesCdsTemplate> builder()
		{
		  return new DatesCdsTemplate.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DatesCdsTemplate);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code convention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CdsConvention> convention()
		{
		  return convention_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  return ((DatesCdsTemplate) bean).StartDate;
			case -1607727319: // endDate
			  return ((DatesCdsTemplate) bean).EndDate;
			case 2039569265: // convention
			  return ((DatesCdsTemplate) bean).Convention;
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
	  /// The bean-builder for {@code DatesCdsTemplate}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<DatesCdsTemplate>
	  {

		internal LocalDate startDate;
		internal LocalDate endDate;
		internal CdsConvention convention;

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
			case -2129778896: // startDate
			  return startDate;
			case -1607727319: // endDate
			  return endDate;
			case 2039569265: // convention
			  return convention;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  this.startDate = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate = (LocalDate) newValue;
			  break;
			case 2039569265: // convention
			  this.convention = (CdsConvention) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override DatesCdsTemplate build()
		{
		  return new DatesCdsTemplate(startDate, endDate, convention);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("DatesCdsTemplate.Builder{");
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate)).Append(',').Append(' ');
		  buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------

	}

}