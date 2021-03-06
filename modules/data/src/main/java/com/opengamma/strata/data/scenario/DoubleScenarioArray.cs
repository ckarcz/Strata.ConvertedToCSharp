﻿using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
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

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// A scenario array holding one {@code double} value for each scenario.
	/// <para>
	/// This contains a list of values, one value for each scenario.
	/// The calculation runner will not attempt to convert the currency of the values.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class DoubleScenarioArray implements ScenarioArray<double>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DoubleScenarioArray : ScenarioArray<double>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray values;
		private readonly DoubleArray values;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified array of values.
	  /// </summary>
	  /// <param name="values">  the values, one value for each scenario </param>
	  /// <returns> an instance with the specified values </returns>
	  public static DoubleScenarioArray of(DoubleArray values)
	  {
		return new DoubleScenarioArray(values);
	  }

	  /// <summary>
	  /// Obtains an instance from the specified list of values.
	  /// </summary>
	  /// <param name="values">  the values, one value for each scenario </param>
	  /// <returns> an instance with the specified values </returns>
	  public static DoubleScenarioArray of(IList<double> values)
	  {
		return new DoubleScenarioArray(DoubleArray.copyOf(values));
	  }

	  /// <summary>
	  /// Obtains an instance using a function to create the entries.
	  /// <para>
	  /// The function is passed the scenario index and returns the value for that index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="size">  the number of elements </param>
	  /// <param name="valueFunction">  the function used to obtain each value </param>
	  /// <returns> an instance initialized using the function </returns>
	  /// <exception cref="IllegalArgumentException"> is size is zero or less </exception>
	  public static DoubleScenarioArray of(int size, System.Func<int, double> valueFunction)
	  {
		ArgChecker.notNegativeOrZero(size, "size");
		return new DoubleScenarioArray(DoubleArray.of(size, valueFunction));
	  }

	  //-------------------------------------------------------------------------
	  public int ScenarioCount
	  {
		  get
		  {
			return values.size();
		  }
	  }

	  public double? get(int index)
	  {
		return values.get(index);
	  }

	  public override Stream<double> stream()
	  {
		return values.stream().boxed();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DoubleScenarioArray}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DoubleScenarioArray.Meta meta()
	  {
		return DoubleScenarioArray.Meta.INSTANCE;
	  }

	  static DoubleScenarioArray()
	  {
		MetaBean.register(DoubleScenarioArray.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DoubleScenarioArray(DoubleArray values)
	  {
		JodaBeanUtils.notNull(values, "values");
		this.values = values;
	  }

	  public override DoubleScenarioArray.Meta metaBean()
	  {
		return DoubleScenarioArray.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the calculated values, one per scenario. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray Values
	  {
		  get
		  {
			return values;
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
		  DoubleScenarioArray other = (DoubleScenarioArray) obj;
		  return JodaBeanUtils.equal(values, other.values);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(values);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("DoubleScenarioArray{");
		buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DoubleScenarioArray}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  values_Renamed = DirectMetaProperty.ofImmutable(this, "values", typeof(DoubleScenarioArray), typeof(DoubleArray));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "values");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code values} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> values_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "values");
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
			case -823812830: // values
			  return values_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends DoubleScenarioArray> builder()
		public override BeanBuilder<DoubleScenarioArray> builder()
		{
		  return new DoubleScenarioArray.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DoubleScenarioArray);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code values} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> values()
		{
		  return values_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -823812830: // values
			  return ((DoubleScenarioArray) bean).Values;
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
	  /// The bean-builder for {@code DoubleScenarioArray}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<DoubleScenarioArray>
	  {

		internal DoubleArray values;

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
			case -823812830: // values
			  return values;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -823812830: // values
			  this.values = (DoubleArray) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override DoubleScenarioArray build()
		{
		  return new DoubleScenarioArray(values);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("DoubleScenarioArray.Builder{");
		  buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}