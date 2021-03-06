﻿using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.common
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using Strike = com.opengamma.strata.market.option.Strike;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Surface node metadata for a generic volatility surface node with a specific period to expiry and strike.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class GenericVolatilitySurfacePeriodParameterMetadata implements com.opengamma.strata.market.param.ParameterMetadata, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class GenericVolatilitySurfacePeriodParameterMetadata : ParameterMetadata, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final java.time.Period period;
		private readonly Period period;
	  /// <summary>
	  /// The strike of the surface node.
	  /// <para>
	  /// This is the strike that the node on the surface is defined as.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.option.Strike strike;
	  private readonly Strike strike;
	  /// <summary>
	  /// The label that describes the node.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", overrideGet = true) private final String label;
	  private readonly string label;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates node metadata using period and strike.
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="strike">  the strike </param>
	  /// <returns> node metadata  </returns>
	  public static GenericVolatilitySurfacePeriodParameterMetadata of(Period period, Strike strike)
	  {

		string label = Pair.of(period, strike.Label).ToString();
		return new GenericVolatilitySurfacePeriodParameterMetadata(period, strike, label);
	  }

	  /// <summary>
	  /// Creates node using period, strike and label.
	  /// </summary>
	  /// <param name="period">  the period </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="label">  the label to use </param>
	  /// <returns> the metadata </returns>
	  public static GenericVolatilitySurfacePeriodParameterMetadata of(Period period, Strike strike, string label)
	  {

		return new GenericVolatilitySurfacePeriodParameterMetadata(period, strike, label);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (string.ReferenceEquals(builder.label, null) && builder.strike != null)
		{
		  builder.label = Pair.of(builder.period, builder.strike.Label).ToString();
		}
	  }

	  public Pair<Period, Strike> Identifier
	  {
		  get
		  {
			return Pair.of(period, strike);
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code GenericVolatilitySurfacePeriodParameterMetadata}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static GenericVolatilitySurfacePeriodParameterMetadata.Meta meta()
	  {
		return GenericVolatilitySurfacePeriodParameterMetadata.Meta.INSTANCE;
	  }

	  static GenericVolatilitySurfacePeriodParameterMetadata()
	  {
		MetaBean.register(GenericVolatilitySurfacePeriodParameterMetadata.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private GenericVolatilitySurfacePeriodParameterMetadata(Period period, Strike strike, string label)
	  {
		JodaBeanUtils.notNull(strike, "strike");
		JodaBeanUtils.notEmpty(label, "label");
		this.period = period;
		this.strike = strike;
		this.label = label;
	  }

	  public override GenericVolatilitySurfacePeriodParameterMetadata.Meta metaBean()
	  {
		return GenericVolatilitySurfacePeriodParameterMetadata.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the period of the surface node.
	  /// <para>
	  /// This is the period to expiry that the node on the surface is defined as.
	  /// There is not necessarily a direct relationship with a date from an underlying instrument.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public Period Period
	  {
		  get
		  {
			return period;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the strike of the surface node.
	  /// <para>
	  /// This is the strike that the node on the surface is defined as.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Strike Strike
	  {
		  get
		  {
			return strike;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the label that describes the node. </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public string Label
	  {
		  get
		  {
			return label;
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
		  GenericVolatilitySurfacePeriodParameterMetadata other = (GenericVolatilitySurfacePeriodParameterMetadata) obj;
		  return JodaBeanUtils.equal(period, other.period) && JodaBeanUtils.equal(strike, other.strike) && JodaBeanUtils.equal(label, other.label);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(period);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strike);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(label);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("GenericVolatilitySurfacePeriodParameterMetadata{");
		buf.Append("period").Append('=').Append(period).Append(',').Append(' ');
		buf.Append("strike").Append('=').Append(strike).Append(',').Append(' ');
		buf.Append("label").Append('=').Append(JodaBeanUtils.ToString(label));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code GenericVolatilitySurfacePeriodParameterMetadata}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  period_Renamed = DirectMetaProperty.ofImmutable(this, "period", typeof(GenericVolatilitySurfacePeriodParameterMetadata), typeof(Period));
			  strike_Renamed = DirectMetaProperty.ofImmutable(this, "strike", typeof(GenericVolatilitySurfacePeriodParameterMetadata), typeof(Strike));
			  label_Renamed = DirectMetaProperty.ofImmutable(this, "label", typeof(GenericVolatilitySurfacePeriodParameterMetadata), typeof(string));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "period", "strike", "label");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code period} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Period> period_Renamed;
		/// <summary>
		/// The meta-property for the {@code strike} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Strike> strike_Renamed;
		/// <summary>
		/// The meta-property for the {@code label} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> label_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "period", "strike", "label");
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
			case -991726143: // period
			  return period_Renamed;
			case -891985998: // strike
			  return strike_Renamed;
			case 102727412: // label
			  return label_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends GenericVolatilitySurfacePeriodParameterMetadata> builder()
		public override BeanBuilder<GenericVolatilitySurfacePeriodParameterMetadata> builder()
		{
		  return new GenericVolatilitySurfacePeriodParameterMetadata.Builder();
		}

		public override Type beanType()
		{
		  return typeof(GenericVolatilitySurfacePeriodParameterMetadata);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code period} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Period> period()
		{
		  return period_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strike} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Strike> strike()
		{
		  return strike_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code label} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> label()
		{
		  return label_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -991726143: // period
			  return ((GenericVolatilitySurfacePeriodParameterMetadata) bean).Period;
			case -891985998: // strike
			  return ((GenericVolatilitySurfacePeriodParameterMetadata) bean).Strike;
			case 102727412: // label
			  return ((GenericVolatilitySurfacePeriodParameterMetadata) bean).Label;
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
	  /// The bean-builder for {@code GenericVolatilitySurfacePeriodParameterMetadata}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<GenericVolatilitySurfacePeriodParameterMetadata>
	  {

		internal Period period;
		internal Strike strike;
		internal string label;

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
			case -991726143: // period
			  return period;
			case -891985998: // strike
			  return strike;
			case 102727412: // label
			  return label;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -991726143: // period
			  this.period = (Period) newValue;
			  break;
			case -891985998: // strike
			  this.strike = (Strike) newValue;
			  break;
			case 102727412: // label
			  this.label = (string) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override GenericVolatilitySurfacePeriodParameterMetadata build()
		{
		  preBuild(this);
		  return new GenericVolatilitySurfacePeriodParameterMetadata(period, strike, label);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("GenericVolatilitySurfacePeriodParameterMetadata.Builder{");
		  buf.Append("period").Append('=').Append(JodaBeanUtils.ToString(period)).Append(',').Append(' ');
		  buf.Append("strike").Append('=').Append(JodaBeanUtils.ToString(strike)).Append(',').Append(' ');
		  buf.Append("label").Append('=').Append(JodaBeanUtils.ToString(label));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}