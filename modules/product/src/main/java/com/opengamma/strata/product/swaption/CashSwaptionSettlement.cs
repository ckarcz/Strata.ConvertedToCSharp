﻿using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swaption
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

	using SettlementType = com.opengamma.strata.product.common.SettlementType;

	/// <summary>
	/// Defines the settlement type and settlement method of swaptions.
	/// <para>
	/// The settlement type is <seealso cref="SettlementType#CASH"/>, This means that a cash amount is paid
	/// by the short party to the long party at the exercise date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CashSwaptionSettlement implements SwaptionSettlement, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CashSwaptionSettlement : SwaptionSettlement, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate settlementDate;
		private readonly LocalDate settlementDate;

	  /// <summary>
	  /// The cash settlement method.
	  /// <para>
	  /// The settlement rate of the cash settled swaption is specified by respective cash settlement methods.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CashSwaptionSettlementMethod method;
	  private readonly CashSwaptionSettlementMethod method;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the settlement date and method.
	  /// </summary>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="method">  the settlement method </param>
	  /// <returns> the settlement </returns>
	  public static CashSwaptionSettlement of(LocalDate settlementDate, CashSwaptionSettlementMethod method)
	  {
		return new CashSwaptionSettlement(settlementDate, method);
	  }

	  //-------------------------------------------------------------------------
	  public SettlementType SettlementType
	  {
		  get
		  {
			return SettlementType.CASH;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CashSwaptionSettlement}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CashSwaptionSettlement.Meta meta()
	  {
		return CashSwaptionSettlement.Meta.INSTANCE;
	  }

	  static CashSwaptionSettlement()
	  {
		MetaBean.register(CashSwaptionSettlement.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CashSwaptionSettlement(LocalDate settlementDate, CashSwaptionSettlementMethod method)
	  {
		JodaBeanUtils.notNull(settlementDate, "settlementDate");
		JodaBeanUtils.notNull(method, "method");
		this.settlementDate = settlementDate;
		this.method = method;
	  }

	  public override CashSwaptionSettlement.Meta metaBean()
	  {
		return CashSwaptionSettlement.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the settlement date.
	  /// <para>
	  /// The payoff of the option is settled at this date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate SettlementDate
	  {
		  get
		  {
			return settlementDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the cash settlement method.
	  /// <para>
	  /// The settlement rate of the cash settled swaption is specified by respective cash settlement methods.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CashSwaptionSettlementMethod Method
	  {
		  get
		  {
			return method;
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
		  CashSwaptionSettlement other = (CashSwaptionSettlement) obj;
		  return JodaBeanUtils.equal(settlementDate, other.settlementDate) && JodaBeanUtils.equal(method, other.method);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(method);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("CashSwaptionSettlement{");
		buf.Append("settlementDate").Append('=').Append(settlementDate).Append(',').Append(' ');
		buf.Append("method").Append('=').Append(JodaBeanUtils.ToString(method));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CashSwaptionSettlement}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  settlementDate_Renamed = DirectMetaProperty.ofImmutable(this, "settlementDate", typeof(CashSwaptionSettlement), typeof(LocalDate));
			  method_Renamed = DirectMetaProperty.ofImmutable(this, "method", typeof(CashSwaptionSettlement), typeof(CashSwaptionSettlementMethod));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "settlementDate", "method");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code settlementDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> settlementDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code method} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CashSwaptionSettlementMethod> method_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "settlementDate", "method");
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
			case -295948169: // settlementDate
			  return settlementDate_Renamed;
			case -1077554975: // method
			  return method_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CashSwaptionSettlement> builder()
		public override BeanBuilder<CashSwaptionSettlement> builder()
		{
		  return new CashSwaptionSettlement.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CashSwaptionSettlement);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code settlementDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> settlementDate()
		{
		  return settlementDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code method} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CashSwaptionSettlementMethod> method()
		{
		  return method_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -295948169: // settlementDate
			  return ((CashSwaptionSettlement) bean).SettlementDate;
			case -1077554975: // method
			  return ((CashSwaptionSettlement) bean).Method;
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
	  /// The bean-builder for {@code CashSwaptionSettlement}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CashSwaptionSettlement>
	  {

		internal LocalDate settlementDate;
		internal CashSwaptionSettlementMethod method;

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
			case -295948169: // settlementDate
			  return settlementDate;
			case -1077554975: // method
			  return method;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -295948169: // settlementDate
			  this.settlementDate = (LocalDate) newValue;
			  break;
			case -1077554975: // method
			  this.method = (CashSwaptionSettlementMethod) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CashSwaptionSettlement build()
		{
		  return new CashSwaptionSettlement(settlementDate, method);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("CashSwaptionSettlement.Builder{");
		  buf.Append("settlementDate").Append('=').Append(JodaBeanUtils.ToString(settlementDate)).Append(',').Append(' ');
		  buf.Append("method").Append('=').Append(JodaBeanUtils.ToString(method));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}