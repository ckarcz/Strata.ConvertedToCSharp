﻿using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.trade
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

	/// <summary>
	/// Describes a column in a trade report.
	/// <para>
	/// Processing of the fields is intentionally delayed so that the fields can be interpreted
	/// in the context of the calculation results, and errors are delayed until the report is run.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class TradeReportColumn implements org.joda.beans.ImmutableBean
	public sealed class TradeReportColumn : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final String header;
		private readonly string header;
	  /// <summary>
	  /// The reference to a value to display in this column.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final String value;
	  private readonly string value;
	  /// <summary>
	  /// Whether to ignore failures, or report the errors.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final boolean ignoreFailures;
	  private readonly bool ignoreFailures;

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code TradeReportColumn}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TradeReportColumn.Meta meta()
	  {
		return TradeReportColumn.Meta.INSTANCE;
	  }

	  static TradeReportColumn()
	  {
		MetaBean.register(TradeReportColumn.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static TradeReportColumn.Builder builder()
	  {
		return new TradeReportColumn.Builder();
	  }

	  private TradeReportColumn(string header, string value, bool ignoreFailures)
	  {
		JodaBeanUtils.notNull(header, "header");
		this.header = header;
		this.value = value;
		this.ignoreFailures = ignoreFailures;
	  }

	  public override TradeReportColumn.Meta metaBean()
	  {
		return TradeReportColumn.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the column header. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public string Header
	  {
		  get
		  {
			return header;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reference to a value to display in this column. </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<string> Value
	  {
		  get
		  {
			return Optional.ofNullable(value);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether to ignore failures, or report the errors. </summary>
	  /// <returns> the value of the property </returns>
	  public bool IgnoreFailures
	  {
		  get
		  {
			return ignoreFailures;
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
		  TradeReportColumn other = (TradeReportColumn) obj;
		  return JodaBeanUtils.equal(header, other.header) && JodaBeanUtils.equal(value, other.value) && (ignoreFailures == other.ignoreFailures);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(header);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(value);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(ignoreFailures);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("TradeReportColumn{");
		buf.Append("header").Append('=').Append(header).Append(',').Append(' ');
		buf.Append("value").Append('=').Append(value).Append(',').Append(' ');
		buf.Append("ignoreFailures").Append('=').Append(JodaBeanUtils.ToString(ignoreFailures));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code TradeReportColumn}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  header_Renamed = DirectMetaProperty.ofImmutable(this, "header", typeof(TradeReportColumn), typeof(string));
			  value_Renamed = DirectMetaProperty.ofImmutable(this, "value", typeof(TradeReportColumn), typeof(string));
			  ignoreFailures_Renamed = DirectMetaProperty.ofImmutable(this, "ignoreFailures", typeof(TradeReportColumn), Boolean.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "header", "value", "ignoreFailures");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code header} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> header_Renamed;
		/// <summary>
		/// The meta-property for the {@code value} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> value_Renamed;
		/// <summary>
		/// The meta-property for the {@code ignoreFailures} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> ignoreFailures_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "header", "value", "ignoreFailures");
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
			case -1221270899: // header
			  return header_Renamed;
			case 111972721: // value
			  return value_Renamed;
			case -335122405: // ignoreFailures
			  return ignoreFailures_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override TradeReportColumn.Builder builder()
		{
		  return new TradeReportColumn.Builder();
		}

		public override Type beanType()
		{
		  return typeof(TradeReportColumn);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code header} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> header()
		{
		  return header_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code value} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> value()
		{
		  return value_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code ignoreFailures} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> ignoreFailures()
		{
		  return ignoreFailures_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1221270899: // header
			  return ((TradeReportColumn) bean).Header;
			case 111972721: // value
			  return ((TradeReportColumn) bean).value;
			case -335122405: // ignoreFailures
			  return ((TradeReportColumn) bean).IgnoreFailures;
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
	  /// The bean-builder for {@code TradeReportColumn}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<TradeReportColumn>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string header_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string value_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool ignoreFailures_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(TradeReportColumn beanToCopy)
		{
		  this.header_Renamed = beanToCopy.Header;
		  this.value_Renamed = beanToCopy.value;
		  this.ignoreFailures_Renamed = beanToCopy.IgnoreFailures;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1221270899: // header
			  return header_Renamed;
			case 111972721: // value
			  return value_Renamed;
			case -335122405: // ignoreFailures
			  return ignoreFailures_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1221270899: // header
			  this.header_Renamed = (string) newValue;
			  break;
			case 111972721: // value
			  this.value_Renamed = (string) newValue;
			  break;
			case -335122405: // ignoreFailures
			  this.ignoreFailures_Renamed = (bool?) newValue.Value;
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

		public override TradeReportColumn build()
		{
		  return new TradeReportColumn(header_Renamed, value_Renamed, ignoreFailures_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the column header. </summary>
		/// <param name="header">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder header(string header)
		{
		  JodaBeanUtils.notNull(header, "header");
		  this.header_Renamed = header;
		  return this;
		}

		/// <summary>
		/// Sets the reference to a value to display in this column. </summary>
		/// <param name="value">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder value(string value)
		{
		  this.value_Renamed = value;
		  return this;
		}

		/// <summary>
		/// Sets whether to ignore failures, or report the errors. </summary>
		/// <param name="ignoreFailures">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder ignoreFailures(bool ignoreFailures)
		{
		  this.ignoreFailures_Renamed = ignoreFailures;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("TradeReportColumn.Builder{");
		  buf.Append("header").Append('=').Append(JodaBeanUtils.ToString(header_Renamed)).Append(',').Append(' ');
		  buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value_Renamed)).Append(',').Append(' ');
		  buf.Append("ignoreFailures").Append('=').Append(JodaBeanUtils.ToString(ignoreFailures_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}