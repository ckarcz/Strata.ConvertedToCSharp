﻿using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit
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

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;

	/// <summary>
	/// A trade in a CDS index used for credit curve calibration. 
	/// <para>
	/// The CDS index trade and market quote are stored in this class.
	/// <seealso cref="CdsIndexTrade"/> and {@code ResolvedCdsIndexTrade} should be used for pricing.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CdsIndexCalibrationTrade implements com.opengamma.strata.product.Trade, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CdsIndexCalibrationTrade : Trade, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CdsIndexTrade underlyingTrade;
		private readonly CdsIndexTrade underlyingTrade;
	  /// <summary>
	  /// The CDS index quote.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CdsQuote quote;
	  private readonly CdsQuote quote;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="quote">  the quote </param>
	  /// <returns> the instance </returns>
	  public static CdsIndexCalibrationTrade of(CdsIndexTrade trade, CdsQuote quote)
	  {
		return new CdsIndexCalibrationTrade(trade, quote);
	  }

	  public TradeInfo Info
	  {
		  get
		  {
			return underlyingTrade.Info;
		  }
	  }

	  public CdsIndexCalibrationTrade withInfo(TradeInfo info)
	  {
		return new CdsIndexCalibrationTrade(underlyingTrade.withInfo(info), quote);
	  }

	  public PortfolioItemSummary summarize()
	  {
		string description = "CDS Index calibration trade";
		Currency currency = underlyingTrade.Product.Currency;
		return SummarizerUtils.summary(this, ProductType.CALIBRATION, description, currency);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CdsIndexCalibrationTrade}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CdsIndexCalibrationTrade.Meta meta()
	  {
		return CdsIndexCalibrationTrade.Meta.INSTANCE;
	  }

	  static CdsIndexCalibrationTrade()
	  {
		MetaBean.register(CdsIndexCalibrationTrade.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CdsIndexCalibrationTrade(CdsIndexTrade underlyingTrade, CdsQuote quote)
	  {
		JodaBeanUtils.notNull(underlyingTrade, "underlyingTrade");
		JodaBeanUtils.notNull(quote, "quote");
		this.underlyingTrade = underlyingTrade;
		this.quote = quote;
	  }

	  public override CdsIndexCalibrationTrade.Meta metaBean()
	  {
		return CdsIndexCalibrationTrade.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying CDS index trade. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CdsIndexTrade UnderlyingTrade
	  {
		  get
		  {
			return underlyingTrade;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the CDS index quote. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CdsQuote Quote
	  {
		  get
		  {
			return quote;
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
		  CdsIndexCalibrationTrade other = (CdsIndexCalibrationTrade) obj;
		  return JodaBeanUtils.equal(underlyingTrade, other.underlyingTrade) && JodaBeanUtils.equal(quote, other.quote);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlyingTrade);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(quote);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("CdsIndexCalibrationTrade{");
		buf.Append("underlyingTrade").Append('=').Append(underlyingTrade).Append(',').Append(' ');
		buf.Append("quote").Append('=').Append(JodaBeanUtils.ToString(quote));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CdsIndexCalibrationTrade}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  underlyingTrade_Renamed = DirectMetaProperty.ofImmutable(this, "underlyingTrade", typeof(CdsIndexCalibrationTrade), typeof(CdsIndexTrade));
			  quote_Renamed = DirectMetaProperty.ofImmutable(this, "quote", typeof(CdsIndexCalibrationTrade), typeof(CdsQuote));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "underlyingTrade", "quote");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code underlyingTrade} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CdsIndexTrade> underlyingTrade_Renamed;
		/// <summary>
		/// The meta-property for the {@code quote} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CdsQuote> quote_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "underlyingTrade", "quote");
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
			case -823800825: // underlyingTrade
			  return underlyingTrade_Renamed;
			case 107953788: // quote
			  return quote_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CdsIndexCalibrationTrade> builder()
		public override BeanBuilder<CdsIndexCalibrationTrade> builder()
		{
		  return new CdsIndexCalibrationTrade.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CdsIndexCalibrationTrade);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code underlyingTrade} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CdsIndexTrade> underlyingTrade()
		{
		  return underlyingTrade_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code quote} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CdsQuote> quote()
		{
		  return quote_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -823800825: // underlyingTrade
			  return ((CdsIndexCalibrationTrade) bean).UnderlyingTrade;
			case 107953788: // quote
			  return ((CdsIndexCalibrationTrade) bean).Quote;
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
	  /// The bean-builder for {@code CdsIndexCalibrationTrade}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CdsIndexCalibrationTrade>
	  {

		internal CdsIndexTrade underlyingTrade;
		internal CdsQuote quote;

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
			case -823800825: // underlyingTrade
			  return underlyingTrade;
			case 107953788: // quote
			  return quote;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -823800825: // underlyingTrade
			  this.underlyingTrade = (CdsIndexTrade) newValue;
			  break;
			case 107953788: // quote
			  this.quote = (CdsQuote) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CdsIndexCalibrationTrade build()
		{
		  return new CdsIndexCalibrationTrade(underlyingTrade, quote);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("CdsIndexCalibrationTrade.Builder{");
		  buf.Append("underlyingTrade").Append('=').Append(JodaBeanUtils.ToString(underlyingTrade)).Append(',').Append(' ');
		  buf.Append("quote").Append('=').Append(JodaBeanUtils.ToString(quote));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------

	}

}