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
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A template for creating credit default swap trades.
	/// <para>
	/// This defines almost all the data necessary to create a credit default swap <seealso cref="CdsTrade"/>.
	/// The start and end of the trade are defined in terms of {@code AccrualStart} and {@code Tenor}.
	/// </para>
	/// <para>
	/// The legal entity ID, trade date, notional and fixed rate are required 
	/// to complete the template and create the trade.
	/// As such, it is often possible to get a market quote for a trade based on the template.
	/// The start date (if it is not the next day) and end date are computed from trade date 
	/// with the standard semi-annual roll convention. 
	/// </para>
	/// <para>
	/// A CDS is quoted in points upfront, par spread, or quoted spread. 
	/// For the latter two cases, the market quotes are passed as the fixed rate.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class TenorCdsTemplate implements CdsTemplate, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class TenorCdsTemplate : CdsTemplate, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final AccrualStart accrualStart;
		private readonly AccrualStart accrualStart;
	  /// <summary>
	  /// The tenor of the credit default swap.
	  /// <para>
	  /// This is the period to the protection end.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.Tenor tenor;
	  private readonly Tenor tenor;
	  /// <summary>
	  /// The market convention of the credit default swap.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CdsConvention convention;
	  private readonly CdsConvention convention;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a template based on the specified tenor and convention.
	  /// <para>
	  /// The protection end will be calculated based on standard semi-annual roll convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="accrualStart">  the accrual start </param>
	  /// <param name="tenor">  the tenor of the CDS </param>
	  /// <param name="convention">  the market convention </param>
	  /// <returns> the template </returns>
	  public static TenorCdsTemplate of(AccrualStart accrualStart, Tenor tenor, CdsConvention convention)
	  {
		return new TenorCdsTemplate(accrualStart, tenor, convention);
	  }

	  /// <summary>
	  /// Obtains a template based on the specified tenor and convention.
	  /// <para>
	  /// The start and end dates will be calculated based on standard semi-annual roll convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tenor">  the tenor of the CDS </param>
	  /// <param name="convention">  the market convention </param>
	  /// <returns> the template </returns>
	  public static TenorCdsTemplate of(Tenor tenor, CdsConvention convention)
	  {
		return of(AccrualStart.IMM_DATE, tenor, convention);
	  }

	  //-------------------------------------------------------------------------
	  public CdsTrade createTrade(StandardId legalEntityId, LocalDate tradeDate, BuySell buySell, double notional, double fixedRate, ReferenceData refData)
	  {

		return accrualStart.Equals(AccrualStart.IMM_DATE) ? convention.createTrade(legalEntityId, tradeDate, tenor, buySell, notional, fixedRate, refData) : convention.createTrade(legalEntityId, tradeDate, tradeDate.plusDays(1), tenor, buySell, notional, fixedRate, refData);
	  }

	  public CdsTrade createTrade(StandardId legalEntityId, LocalDate tradeDate, BuySell buySell, double notional, double fixedRate, AdjustablePayment upFrontFee, ReferenceData refData)
	  {

		return accrualStart.Equals(AccrualStart.IMM_DATE) ? convention.createTrade(legalEntityId, tradeDate, tenor, buySell, notional, fixedRate, upFrontFee, refData) : convention.createTrade(legalEntityId, tradeDate, tradeDate.plusDays(1), tenor, buySell, notional, fixedRate, upFrontFee, refData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code TenorCdsTemplate}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TenorCdsTemplate.Meta meta()
	  {
		return TenorCdsTemplate.Meta.INSTANCE;
	  }

	  static TenorCdsTemplate()
	  {
		MetaBean.register(TenorCdsTemplate.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private TenorCdsTemplate(AccrualStart accrualStart, Tenor tenor, CdsConvention convention)
	  {
		JodaBeanUtils.notNull(accrualStart, "accrualStart");
		JodaBeanUtils.notNull(tenor, "tenor");
		JodaBeanUtils.notNull(convention, "convention");
		this.accrualStart = accrualStart;
		this.tenor = tenor;
		this.convention = convention;
	  }

	  public override TenorCdsTemplate.Meta metaBean()
	  {
		return TenorCdsTemplate.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual start.
	  /// <para>
	  /// Whether the accrual start is the next day or the previous IMM date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public AccrualStart AccrualStart
	  {
		  get
		  {
			return accrualStart;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the tenor of the credit default swap.
	  /// <para>
	  /// This is the period to the protection end.
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
		  TenorCdsTemplate other = (TenorCdsTemplate) obj;
		  return JodaBeanUtils.equal(accrualStart, other.accrualStart) && JodaBeanUtils.equal(tenor, other.tenor) && JodaBeanUtils.equal(convention, other.convention);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualStart);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tenor);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(convention);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("TenorCdsTemplate{");
		buf.Append("accrualStart").Append('=').Append(accrualStart).Append(',').Append(' ');
		buf.Append("tenor").Append('=').Append(tenor).Append(',').Append(' ');
		buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code TenorCdsTemplate}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  accrualStart_Renamed = DirectMetaProperty.ofImmutable(this, "accrualStart", typeof(TenorCdsTemplate), typeof(AccrualStart));
			  tenor_Renamed = DirectMetaProperty.ofImmutable(this, "tenor", typeof(TenorCdsTemplate), typeof(Tenor));
			  convention_Renamed = DirectMetaProperty.ofImmutable(this, "convention", typeof(TenorCdsTemplate), typeof(CdsConvention));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "accrualStart", "tenor", "convention");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code accrualStart} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AccrualStart> accrualStart_Renamed;
		/// <summary>
		/// The meta-property for the {@code tenor} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Tenor> tenor_Renamed;
		/// <summary>
		/// The meta-property for the {@code convention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CdsConvention> convention_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "accrualStart", "tenor", "convention");
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
			case 1071260659: // accrualStart
			  return accrualStart_Renamed;
			case 110246592: // tenor
			  return tenor_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends TenorCdsTemplate> builder()
		public override BeanBuilder<TenorCdsTemplate> builder()
		{
		  return new TenorCdsTemplate.Builder();
		}

		public override Type beanType()
		{
		  return typeof(TenorCdsTemplate);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code accrualStart} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AccrualStart> accrualStart()
		{
		  return accrualStart_Renamed;
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
		public MetaProperty<CdsConvention> convention()
		{
		  return convention_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1071260659: // accrualStart
			  return ((TenorCdsTemplate) bean).AccrualStart;
			case 110246592: // tenor
			  return ((TenorCdsTemplate) bean).Tenor;
			case 2039569265: // convention
			  return ((TenorCdsTemplate) bean).Convention;
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
	  /// The bean-builder for {@code TenorCdsTemplate}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<TenorCdsTemplate>
	  {

		internal AccrualStart accrualStart;
		internal Tenor tenor;
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
			case 1071260659: // accrualStart
			  return accrualStart;
			case 110246592: // tenor
			  return tenor;
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
			case 1071260659: // accrualStart
			  this.accrualStart = (AccrualStart) newValue;
			  break;
			case 110246592: // tenor
			  this.tenor = (Tenor) newValue;
			  break;
			case 2039569265: // convention
			  this.convention = (CdsConvention) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override TenorCdsTemplate build()
		{
		  return new TenorCdsTemplate(accrualStart, tenor, convention);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("TenorCdsTemplate.Builder{");
		  buf.Append("accrualStart").Append('=').Append(JodaBeanUtils.ToString(accrualStart)).Append(',').Append(' ');
		  buf.Append("tenor").Append('=').Append(JodaBeanUtils.ToString(tenor)).Append(',').Append(' ');
		  buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------

	}

}