﻿using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// Defines a known fixed rate of interest.
	/// <para>
	/// An interest rate that is specified in the contract or has subsequently been determined.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FixedRateComputation implements RateComputation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FixedRateComputation : RateComputation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double rate;
		private readonly double rate;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="rate">  the fixed rate </param>
	  /// <returns> the fixed rate computation </returns>
	  public static FixedRateComputation of(double rate)
	  {
		return new FixedRateComputation(rate);
	  }

	  //-------------------------------------------------------------------------
	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		// no indices to add
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedRateComputation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FixedRateComputation.Meta meta()
	  {
		return FixedRateComputation.Meta.INSTANCE;
	  }

	  static FixedRateComputation()
	  {
		MetaBean.register(FixedRateComputation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FixedRateComputation(double rate)
	  {
		this.rate = rate;
	  }

	  public override FixedRateComputation.Meta metaBean()
	  {
		return FixedRateComputation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed rate to be paid.
	  /// A 5% rate will be expressed as 0.05. </summary>
	  /// <returns> the value of the property </returns>
	  public double Rate
	  {
		  get
		  {
			return rate;
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
		  FixedRateComputation other = (FixedRateComputation) obj;
		  return JodaBeanUtils.equal(rate, other.rate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("FixedRateComputation{");
		buf.Append("rate").Append('=').Append(JodaBeanUtils.ToString(rate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedRateComputation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  rate_Renamed = DirectMetaProperty.ofImmutable(this, "rate", typeof(FixedRateComputation), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "rate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code rate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> rate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "rate");
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
			case 3493088: // rate
			  return rate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FixedRateComputation> builder()
		public override BeanBuilder<FixedRateComputation> builder()
		{
		  return new FixedRateComputation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FixedRateComputation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code rate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> rate()
		{
		  return rate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3493088: // rate
			  return ((FixedRateComputation) bean).Rate;
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
	  /// The bean-builder for {@code FixedRateComputation}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FixedRateComputation>
	  {

		internal double rate;

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
			case 3493088: // rate
			  return rate;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3493088: // rate
			  this.rate = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FixedRateComputation build()
		{
		  return new FixedRateComputation(rate);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("FixedRateComputation.Builder{");
		  buf.Append("rate").Append('=').Append(JodaBeanUtils.ToString(rate));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}