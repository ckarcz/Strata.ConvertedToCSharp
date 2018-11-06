using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;

	/// <summary>
	/// A bill, resolved for pricing.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(constructorScope = "package") public class ResolvedBill implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public class ResolvedBill : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.SecurityId securityId;
		private readonly SecurityId securityId;
	  /// <summary>
	  /// The notional payment of the bill notional, the amount must be positive.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Payment notional;
	  private readonly Payment notional;
	  /// <summary>
	  /// The day count convention applicable.
	  /// <para>
	  /// The conversion from dates to a numerical value is made based on this day count.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// Yield convention.
	  /// <para>
	  /// The convention defines how to convert from yield to price and inversely.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final BillYieldConvention yieldConvention;
	  private readonly BillYieldConvention yieldConvention;
	  /// <summary>
	  /// The legal entity identifier.
	  /// <para>
	  /// This identifier is used for the legal entity that issues the bill.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.LegalEntityId legalEntityId;
	  private readonly LegalEntityId legalEntityId;
	  /// <summary>
	  /// The number of days between valuation date and settlement date.
	  /// <para>
	  /// It is usually one business day for US and UK bills and two days for Euroland government bills.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment settlementDateOffset;
	  private readonly DaysAdjustment settlementDateOffset;

	  /// <summary>
	  /// Returns the currency of the bill.
	  /// </summary>
	  /// <returns> the currency </returns>
	  public virtual Currency Currency
	  {
		  get
		  {
			return notional.Currency;
		  }
	  }

	  /// <summary>
	  /// Computes the price from the yield at a given settlement date.
	  /// </summary>
	  /// <param name="yield">  the yield </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <returns> the price </returns>
	  public virtual double priceFromYield(double yield, LocalDate settlementDate)
	  {
		double accrualFactor = dayCount.relativeYearFraction(settlementDate, notional.Date);
		return yieldConvention.priceFromYield(yield, accrualFactor);
	  }

	  /// <summary>
	  /// Computes the yield from the price at a given settlement date.
	  /// </summary>
	  /// <param name="price">  the price </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <returns> the yield </returns>
	  public virtual double yieldFromPrice(double price, LocalDate settlementDate)
	  {
		double accrualFactor = dayCount.relativeYearFraction(settlementDate, notional.Date);
		return yieldConvention.yieldFromPrice(price, accrualFactor);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedBill}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedBill.Meta meta()
	  {
		return ResolvedBill.Meta.INSTANCE;
	  }

	  static ResolvedBill()
	  {
		MetaBean.register(ResolvedBill.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedBill.Builder builder()
	  {
		return new ResolvedBill.Builder();
	  }

	  /// <summary>
	  /// Restricted constructor. </summary>
	  /// <param name="builder">  the builder to copy from, not null </param>
	  protected internal ResolvedBill(ResolvedBill.Builder builder)
	  {
		JodaBeanUtils.notNull(builder.securityId_Renamed, "securityId");
		JodaBeanUtils.notNull(builder.notional_Renamed, "notional");
		JodaBeanUtils.notNull(builder.dayCount_Renamed, "dayCount");
		JodaBeanUtils.notNull(builder.yieldConvention_Renamed, "yieldConvention");
		JodaBeanUtils.notNull(builder.legalEntityId_Renamed, "legalEntityId");
		JodaBeanUtils.notNull(builder.settlementDateOffset_Renamed, "settlementDateOffset");
		this.securityId = builder.securityId_Renamed;
		this.notional = builder.notional_Renamed;
		this.dayCount = builder.dayCount_Renamed;
		this.yieldConvention = builder.yieldConvention_Renamed;
		this.legalEntityId = builder.legalEntityId_Renamed;
		this.settlementDateOffset = builder.settlementDateOffset_Renamed;
	  }

	  public override ResolvedBill.Meta metaBean()
	  {
		return ResolvedBill.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public virtual SecurityId SecurityId
	  {
		  get
		  {
			return securityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional payment of the bill notional, the amount must be positive. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public virtual Payment Notional
	  {
		  get
		  {
			return notional;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention applicable.
	  /// <para>
	  /// The conversion from dates to a numerical value is made based on this day count.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public virtual DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets yield convention.
	  /// <para>
	  /// The convention defines how to convert from yield to price and inversely.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public virtual BillYieldConvention YieldConvention
	  {
		  get
		  {
			return yieldConvention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legal entity identifier.
	  /// <para>
	  /// This identifier is used for the legal entity that issues the bill.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public virtual LegalEntityId LegalEntityId
	  {
		  get
		  {
			return legalEntityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of days between valuation date and settlement date.
	  /// <para>
	  /// It is usually one business day for US and UK bills and two days for Euroland government bills.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public virtual DaysAdjustment SettlementDateOffset
	  {
		  get
		  {
			return settlementDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public virtual Builder toBuilder()
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
		  ResolvedBill other = (ResolvedBill) obj;
		  return JodaBeanUtils.equal(securityId, other.securityId) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(yieldConvention, other.yieldConvention) && JodaBeanUtils.equal(legalEntityId, other.legalEntityId) && JodaBeanUtils.equal(settlementDateOffset, other.settlementDateOffset);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(securityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yieldConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementDateOffset);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("ResolvedBill{");
		int len = buf.Length;
		ToString(buf);
		if (buf.Length > len)
		{
		  buf.Length = buf.Length - 2;
		}
		buf.Append('}');
		return buf.ToString();
	  }

	  protected internal virtual void ToString(StringBuilder buf)
	  {
		buf.Append("securityId").Append('=').Append(JodaBeanUtils.ToString(securityId)).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional)).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount)).Append(',').Append(' ');
		buf.Append("yieldConvention").Append('=').Append(JodaBeanUtils.ToString(yieldConvention)).Append(',').Append(' ');
		buf.Append("legalEntityId").Append('=').Append(JodaBeanUtils.ToString(legalEntityId)).Append(',').Append(' ');
		buf.Append("settlementDateOffset").Append('=').Append(JodaBeanUtils.ToString(settlementDateOffset)).Append(',').Append(' ');
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedBill}.
	  /// </summary>
	  public class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal virtual void InitializeInstanceFields()
		  {
			  securityId_Renamed = DirectMetaProperty.ofImmutable(this, "securityId", typeof(ResolvedBill), typeof(SecurityId));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(ResolvedBill), typeof(Payment));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(ResolvedBill), typeof(DayCount));
			  yieldConvention_Renamed = DirectMetaProperty.ofImmutable(this, "yieldConvention", typeof(ResolvedBill), typeof(BillYieldConvention));
			  legalEntityId_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityId", typeof(ResolvedBill), typeof(LegalEntityId));
			  settlementDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "settlementDateOffset", typeof(ResolvedBill), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "securityId", "notional", "dayCount", "yieldConvention", "legalEntityId", "settlementDateOffset");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code securityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityId> securityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code notional} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Payment> notional_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code yieldConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BillYieldConvention> yieldConvention_Renamed;
		/// <summary>
		/// The meta-property for the {@code legalEntityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LegalEntityId> legalEntityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code settlementDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> settlementDateOffset_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "securityId", "notional", "dayCount", "yieldConvention", "legalEntityId", "settlementDateOffset");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		protected internal Meta()
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
			case 1574023291: // securityId
			  return securityId_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1895216418: // yieldConvention
			  return yieldConvention_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedBill.Builder builder()
		{
		  return new ResolvedBill.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedBill);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code securityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityId> securityId()
		{
		  return securityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code notional} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Payment> notional()
		{
		  return notional_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yieldConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BillYieldConvention> yieldConvention()
		{
		  return yieldConvention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code legalEntityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LegalEntityId> legalEntityId()
		{
		  return legalEntityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code settlementDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> settlementDateOffset()
		{
		  return settlementDateOffset_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return ((ResolvedBill) bean).SecurityId;
			case 1585636160: // notional
			  return ((ResolvedBill) bean).Notional;
			case 1905311443: // dayCount
			  return ((ResolvedBill) bean).DayCount;
			case -1895216418: // yieldConvention
			  return ((ResolvedBill) bean).YieldConvention;
			case 866287159: // legalEntityId
			  return ((ResolvedBill) bean).LegalEntityId;
			case 135924714: // settlementDateOffset
			  return ((ResolvedBill) bean).SettlementDateOffset;
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
	  /// The bean-builder for {@code ResolvedBill}.
	  /// </summary>
	  public class Builder : DirectFieldsBeanBuilder<ResolvedBill>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityId securityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Payment notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BillYieldConvention yieldConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LegalEntityId legalEntityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment settlementDateOffset_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		protected internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		protected internal Builder(ResolvedBill beanToCopy)
		{
		  this.securityId_Renamed = beanToCopy.SecurityId;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.yieldConvention_Renamed = beanToCopy.YieldConvention;
		  this.legalEntityId_Renamed = beanToCopy.LegalEntityId;
		  this.settlementDateOffset_Renamed = beanToCopy.SettlementDateOffset;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return securityId_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1895216418: // yieldConvention
			  return yieldConvention_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  this.securityId_Renamed = (SecurityId) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (Payment) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case -1895216418: // yieldConvention
			  this.yieldConvention_Renamed = (BillYieldConvention) newValue;
			  break;
			case 866287159: // legalEntityId
			  this.legalEntityId_Renamed = (LegalEntityId) newValue;
			  break;
			case 135924714: // settlementDateOffset
			  this.settlementDateOffset_Renamed = (DaysAdjustment) newValue;
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

		public override ResolvedBill build()
		{
		  return new ResolvedBill(this);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the security identifier.
		/// <para>
		/// This identifier uniquely identifies the security within the system.
		/// </para>
		/// </summary>
		/// <param name="securityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public virtual Builder securityId(SecurityId securityId)
		{
		  JodaBeanUtils.notNull(securityId, "securityId");
		  this.securityId_Renamed = securityId;
		  return this;
		}

		/// <summary>
		/// Sets the notional payment of the bill notional, the amount must be positive. </summary>
		/// <param name="notional">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public virtual Builder notional(Payment notional)
		{
		  JodaBeanUtils.notNull(notional, "notional");
		  this.notional_Renamed = notional;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention applicable.
		/// <para>
		/// The conversion from dates to a numerical value is made based on this day count.
		/// </para>
		/// </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public virtual Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets yield convention.
		/// <para>
		/// The convention defines how to convert from yield to price and inversely.
		/// </para>
		/// </summary>
		/// <param name="yieldConvention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public virtual Builder yieldConvention(BillYieldConvention yieldConvention)
		{
		  JodaBeanUtils.notNull(yieldConvention, "yieldConvention");
		  this.yieldConvention_Renamed = yieldConvention;
		  return this;
		}

		/// <summary>
		/// Sets the legal entity identifier.
		/// <para>
		/// This identifier is used for the legal entity that issues the bill.
		/// </para>
		/// </summary>
		/// <param name="legalEntityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public virtual Builder legalEntityId(LegalEntityId legalEntityId)
		{
		  JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		  this.legalEntityId_Renamed = legalEntityId;
		  return this;
		}

		/// <summary>
		/// Sets the number of days between valuation date and settlement date.
		/// <para>
		/// It is usually one business day for US and UK bills and two days for Euroland government bills.
		/// </para>
		/// </summary>
		/// <param name="settlementDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public virtual Builder settlementDateOffset(DaysAdjustment settlementDateOffset)
		{
		  JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		  this.settlementDateOffset_Renamed = settlementDateOffset;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("ResolvedBill.Builder{");
		  int len = buf.Length;
		  ToString(buf);
		  if (buf.Length > len)
		  {
			buf.Length = buf.Length - 2;
		  }
		  buf.Append('}');
		  return buf.ToString();
		}

		protected internal virtual void ToString(StringBuilder buf)
		{
		  buf.Append("securityId").Append('=').Append(JodaBeanUtils.ToString(securityId_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("yieldConvention").Append('=').Append(JodaBeanUtils.ToString(yieldConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("legalEntityId").Append('=').Append(JodaBeanUtils.ToString(legalEntityId_Renamed)).Append(',').Append(' ');
		  buf.Append("settlementDateOffset").Append('=').Append(JodaBeanUtils.ToString(settlementDateOffset_Renamed)).Append(',').Append(' ');
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}