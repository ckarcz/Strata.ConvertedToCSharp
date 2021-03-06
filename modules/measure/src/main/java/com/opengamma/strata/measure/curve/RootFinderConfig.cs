﻿using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.curve
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Configuration for the root finder used when calibrating curves.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class RootFinderConfig implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RootFinderConfig : ImmutableBean
	{

	  /// <summary>
	  /// The default absolute tolerance for the root finder. </summary>
	  public const double DEFAULT_ABSOLUTE_TOLERANCE = 1e-9;

	  /// <summary>
	  /// The default relative tolerance for the root finder. </summary>
	  public const double DEFAULT_RELATIVE_TOLERANCE = 1e-9;

	  /// <summary>
	  /// The default maximum number of steps for the root finder. </summary>
	  public const int DEFAULT_MAXIMUM_STEPS = 1000;

	  /// <summary>
	  /// The standard configuration. </summary>
	  private static readonly RootFinderConfig STANDARD = new RootFinderConfig(DEFAULT_ABSOLUTE_TOLERANCE, DEFAULT_RELATIVE_TOLERANCE, DEFAULT_MAXIMUM_STEPS);

	  /// <summary>
	  /// The absolute tolerance for the root finder. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double absoluteTolerance;
	  private readonly double absoluteTolerance;

	  /// <summary>
	  /// The relative tolerance for the root finder. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double relativeTolerance;
	  private readonly double relativeTolerance;

	  /// <summary>
	  /// The maximum number of steps for the root finder. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final int maximumSteps;
	  private readonly int maximumSteps;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns standard root finder configuration, using the {@code DEFAULT} constants from this class.
	  /// </summary>
	  /// <returns> the standard root finder configuration, using the {@code DEFAULT} constants from this class </returns>
	  public static RootFinderConfig standard()
	  {
		return STANDARD;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.absoluteTolerance(DEFAULT_ABSOLUTE_TOLERANCE);
		builder.relativeTolerance(DEFAULT_RELATIVE_TOLERANCE);
		builder.maximumSteps(DEFAULT_MAXIMUM_STEPS);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RootFinderConfig}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RootFinderConfig.Meta meta()
	  {
		return RootFinderConfig.Meta.INSTANCE;
	  }

	  static RootFinderConfig()
	  {
		MetaBean.register(RootFinderConfig.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static RootFinderConfig.Builder builder()
	  {
		return new RootFinderConfig.Builder();
	  }

	  private RootFinderConfig(double absoluteTolerance, double relativeTolerance, int maximumSteps)
	  {
		ArgChecker.notNegativeOrZero(absoluteTolerance, "absoluteTolerance");
		ArgChecker.notNegativeOrZero(relativeTolerance, "relativeTolerance");
		ArgChecker.notNegativeOrZero(maximumSteps, "maximumSteps");
		this.absoluteTolerance = absoluteTolerance;
		this.relativeTolerance = relativeTolerance;
		this.maximumSteps = maximumSteps;
	  }

	  public override RootFinderConfig.Meta metaBean()
	  {
		return RootFinderConfig.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the absolute tolerance for the root finder. </summary>
	  /// <returns> the value of the property </returns>
	  public double AbsoluteTolerance
	  {
		  get
		  {
			return absoluteTolerance;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the relative tolerance for the root finder. </summary>
	  /// <returns> the value of the property </returns>
	  public double RelativeTolerance
	  {
		  get
		  {
			return relativeTolerance;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the maximum number of steps for the root finder. </summary>
	  /// <returns> the value of the property </returns>
	  public int MaximumSteps
	  {
		  get
		  {
			return maximumSteps;
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
		  RootFinderConfig other = (RootFinderConfig) obj;
		  return JodaBeanUtils.equal(absoluteTolerance, other.absoluteTolerance) && JodaBeanUtils.equal(relativeTolerance, other.relativeTolerance) && (maximumSteps == other.maximumSteps);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(absoluteTolerance);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(relativeTolerance);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(maximumSteps);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("RootFinderConfig{");
		buf.Append("absoluteTolerance").Append('=').Append(absoluteTolerance).Append(',').Append(' ');
		buf.Append("relativeTolerance").Append('=').Append(relativeTolerance).Append(',').Append(' ');
		buf.Append("maximumSteps").Append('=').Append(JodaBeanUtils.ToString(maximumSteps));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RootFinderConfig}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  absoluteTolerance_Renamed = DirectMetaProperty.ofImmutable(this, "absoluteTolerance", typeof(RootFinderConfig), Double.TYPE);
			  relativeTolerance_Renamed = DirectMetaProperty.ofImmutable(this, "relativeTolerance", typeof(RootFinderConfig), Double.TYPE);
			  maximumSteps_Renamed = DirectMetaProperty.ofImmutable(this, "maximumSteps", typeof(RootFinderConfig), Integer.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "absoluteTolerance", "relativeTolerance", "maximumSteps");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code absoluteTolerance} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> absoluteTolerance_Renamed;
		/// <summary>
		/// The meta-property for the {@code relativeTolerance} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> relativeTolerance_Renamed;
		/// <summary>
		/// The meta-property for the {@code maximumSteps} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> maximumSteps_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "absoluteTolerance", "relativeTolerance", "maximumSteps");
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
			case -402212778: // absoluteTolerance
			  return absoluteTolerance_Renamed;
			case 1517353633: // relativeTolerance
			  return relativeTolerance_Renamed;
			case 715849959: // maximumSteps
			  return maximumSteps_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override RootFinderConfig.Builder builder()
		{
		  return new RootFinderConfig.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RootFinderConfig);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code absoluteTolerance} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> absoluteTolerance()
		{
		  return absoluteTolerance_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code relativeTolerance} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> relativeTolerance()
		{
		  return relativeTolerance_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code maximumSteps} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> maximumSteps()
		{
		  return maximumSteps_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -402212778: // absoluteTolerance
			  return ((RootFinderConfig) bean).AbsoluteTolerance;
			case 1517353633: // relativeTolerance
			  return ((RootFinderConfig) bean).RelativeTolerance;
			case 715849959: // maximumSteps
			  return ((RootFinderConfig) bean).MaximumSteps;
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
	  /// The bean-builder for {@code RootFinderConfig}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<RootFinderConfig>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double absoluteTolerance_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double relativeTolerance_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal int maximumSteps_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(RootFinderConfig beanToCopy)
		{
		  this.absoluteTolerance_Renamed = beanToCopy.AbsoluteTolerance;
		  this.relativeTolerance_Renamed = beanToCopy.RelativeTolerance;
		  this.maximumSteps_Renamed = beanToCopy.MaximumSteps;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -402212778: // absoluteTolerance
			  return absoluteTolerance_Renamed;
			case 1517353633: // relativeTolerance
			  return relativeTolerance_Renamed;
			case 715849959: // maximumSteps
			  return maximumSteps_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -402212778: // absoluteTolerance
			  this.absoluteTolerance_Renamed = (double?) newValue.Value;
			  break;
			case 1517353633: // relativeTolerance
			  this.relativeTolerance_Renamed = (double?) newValue.Value;
			  break;
			case 715849959: // maximumSteps
			  this.maximumSteps_Renamed = (int?) newValue.Value;
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

		public override RootFinderConfig build()
		{
		  return new RootFinderConfig(absoluteTolerance_Renamed, relativeTolerance_Renamed, maximumSteps_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the absolute tolerance for the root finder. </summary>
		/// <param name="absoluteTolerance">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder absoluteTolerance(double absoluteTolerance)
		{
		  ArgChecker.notNegativeOrZero(absoluteTolerance, "absoluteTolerance");
		  this.absoluteTolerance_Renamed = absoluteTolerance;
		  return this;
		}

		/// <summary>
		/// Sets the relative tolerance for the root finder. </summary>
		/// <param name="relativeTolerance">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder relativeTolerance(double relativeTolerance)
		{
		  ArgChecker.notNegativeOrZero(relativeTolerance, "relativeTolerance");
		  this.relativeTolerance_Renamed = relativeTolerance;
		  return this;
		}

		/// <summary>
		/// Sets the maximum number of steps for the root finder. </summary>
		/// <param name="maximumSteps">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder maximumSteps(int maximumSteps)
		{
		  ArgChecker.notNegativeOrZero(maximumSteps, "maximumSteps");
		  this.maximumSteps_Renamed = maximumSteps;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("RootFinderConfig.Builder{");
		  buf.Append("absoluteTolerance").Append('=').Append(JodaBeanUtils.ToString(absoluteTolerance_Renamed)).Append(',').Append(' ');
		  buf.Append("relativeTolerance").Append('=').Append(JodaBeanUtils.ToString(relativeTolerance_Renamed)).Append(',').Append(' ');
		  buf.Append("maximumSteps").Append('=').Append(JodaBeanUtils.ToString(maximumSteps_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}