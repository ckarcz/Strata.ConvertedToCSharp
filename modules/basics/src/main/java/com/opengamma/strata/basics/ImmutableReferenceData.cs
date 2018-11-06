using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
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

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// An immutable set of reference data
	/// <para>
	/// This is the standard immutable implementation of <seealso cref="ReferenceData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ImmutableReferenceData implements ReferenceData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableReferenceData : ReferenceData, ImmutableBean
	{

	  /// <summary>
	  /// The empty instance.
	  /// </summary>
	  private static readonly ImmutableReferenceData EMPTY = new ImmutableReferenceData(ImmutableMap.of());

	  /// <summary>
	  /// The typed reference data values by identifier.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Map<? extends ReferenceDataId<?>, ?>") private final com.google.common.collect.ImmutableMap<ReferenceDataId<?>, Object> values;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<ReferenceDataId<object>, object> values;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a map of reference data.
	  /// <para>
	  /// Each entry in the map is a single piece of reference data, keyed by the matching identifier.
	  /// For example, a <seealso cref="HolidayCalendarId"/> associated with a <seealso cref="HolidayCalendar"/>.
	  /// The caller must ensure that the each entry in the map corresponds with the parameterized
	  /// type on the identifier.
	  /// </para>
	  /// <para>
	  /// The resulting {@code ImmutableReferenceData} instance does not include the
	  /// <seealso cref="ReferenceData#minimal() minimal"/> set of reference data that is essential for pricing.
	  /// To include the minimal set, use <seealso cref="ReferenceData#of(Map)"/> or <seealso cref="#combinedWith(ReferenceData)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="values">  the reference data values </param>
	  /// <returns> the reference data instance </returns>
	  /// <exception cref="ClassCastException"> if a value does not match the parameterized type associated with the identifier </exception>
	  public static ImmutableReferenceData of<T1>(IDictionary<T1> values) where T1 : ReferenceDataId<T1>
	  {
		// validation handles case where value does not match identifier
		values.forEach((id, value) => validateEntry(id, value));
		return new ImmutableReferenceData(values);
	  }

	  /// <summary>
	  /// Obtains an instance from a single reference data entry.
	  /// <para>
	  /// This returns an instance containing a single entry based on the specified identifier and value.
	  /// This is primarily of interest to test cases.
	  /// </para>
	  /// <para>
	  /// The resulting {@code ImmutableReferenceData} instance does not include the
	  /// <seealso cref="ReferenceData#minimal() minimal"/> set of reference data that is essential for pricing.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the reference data </param>
	  /// <param name="id">  the identifier </param>
	  /// <param name="value">  the reference data values </param>
	  /// <returns> the reference data instance </returns>
	  /// <exception cref="ClassCastException"> if a value does not match the parameterized type associated with the identifier </exception>
	  public static ImmutableReferenceData of<T>(ReferenceDataId<T> id, T value)
	  {
		// validation handles edge case where input by raw or polluted types
		validateEntry(id, value);
		return new ImmutableReferenceData(ImmutableMap.of(id, value));
	  }

	  // validates a single entry
	  private static void validateEntry<T1>(ReferenceDataId<T1> id, object value)
	  {
		if (!id.ReferenceDataType.IsInstanceOfType(value))
		{
		  if (value == null)
		  {
			throw new System.ArgumentException(Messages.format("Value for identifier '{}' must not be null", id));
		  }
		  throw new System.InvalidCastException(Messages.format("Value for identifier '{}' does not implement expected type '{}': '{}'", id, id.ReferenceDataType.Name, value));
		}
	  }

	  /// <summary>
	  /// Obtains an instance containing no reference data.
	  /// </summary>
	  /// <returns> empty reference data </returns>
	  public static ImmutableReferenceData empty()
	  {
		return EMPTY;
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> T queryValueOrNull(ReferenceDataId<T> id)
	  public T queryValueOrNull<T>(ReferenceDataId<T> id)
	  {
		// no type check against id.getReferenceDataType() as checked in factory
		return (T) values.get(id);
	  }

	  public override ReferenceData combinedWith(ReferenceData other)
	  {
		if (other is ImmutableReferenceData)
		{
		  ImmutableReferenceData otherData = (ImmutableReferenceData) other;
		  // hash map so that keys can overlap, with this instance taking priority
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> combined = new java.util.HashMap<>();
		  IDictionary<ReferenceDataId<object>, object> combined = new Dictionary<ReferenceDataId<object>, object>();
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		  combined.putAll(otherData.values);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		  combined.putAll(this.values);
		  return new ImmutableReferenceData(combined);
		}
		return ReferenceData.this.combinedWith(other);
	  }

	  // this should be a private method on the interface
	  // extracted to aid inlining performance
	  internal static string msgValueNotFound<T1>(ReferenceDataId<T1> id)
	  {
		return Messages.format("Reference data not found for identifier '{}' of type '{}'", id, id.GetType().Name);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableReferenceData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableReferenceData.Meta meta()
	  {
		return ImmutableReferenceData.Meta.INSTANCE;
	  }

	  static ImmutableReferenceData()
	  {
		MetaBean.register(ImmutableReferenceData.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ImmutableReferenceData<T1>(IDictionary<T1> values) where T1 : ReferenceDataId<T1>
	  {
		JodaBeanUtils.notNull(values, "values");
		this.values = ImmutableMap.copyOf(values);
	  }

	  public override ImmutableReferenceData.Meta metaBean()
	  {
		return ImmutableReferenceData.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the typed reference data values by identifier. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<ReferenceDataId<?>, Object> getValues()
	  public ImmutableMap<ReferenceDataId<object>, object> Values
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
		  ImmutableReferenceData other = (ImmutableReferenceData) obj;
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
		buf.Append("ImmutableReferenceData{");
		buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableReferenceData}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  values_Renamed = DirectMetaProperty.ofImmutable(this, "values", typeof(ImmutableReferenceData), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "values");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code values} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<ReferenceDataId<?>, Object>> values = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "values", ImmutableReferenceData.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<ReferenceDataId<object>, object>> values_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ImmutableReferenceData> builder()
		public override BeanBuilder<ImmutableReferenceData> builder()
		{
		  return new ImmutableReferenceData.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableReferenceData);
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
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<ReferenceDataId<?>, Object>> values()
		public MetaProperty<ImmutableMap<ReferenceDataId<object>, object>> values()
		{
		  return values_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -823812830: // values
			  return ((ImmutableReferenceData) bean).Values;
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
	  /// The bean-builder for {@code ImmutableReferenceData}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ImmutableReferenceData>
	  {

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<? extends ReferenceDataId<?>, ?> values = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<ReferenceDataId<object>, ?> values = ImmutableMap.of();

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

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -823812830: // values
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.values = (java.util.Map<? extends ReferenceDataId<?>, ?>) newValue;
			  this.values = (IDictionary<ReferenceDataId<object>, ?>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ImmutableReferenceData build()
		{
		  return new ImmutableReferenceData(values);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("ImmutableReferenceData.Builder{");
		  buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}