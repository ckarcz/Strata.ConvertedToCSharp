using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.explain
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
	using StringConvert = org.joda.convert.StringConvert;

	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// A map of explanatory values.
	/// <para>
	/// This is a loosely defined data structure that allows an explanation of a calculation to be represented.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ExplainMap implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ExplainMap : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<ExplainKey<?>, Object> map;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		private readonly ImmutableMap<ExplainKey<object>, object> map;

	  /// <summary>
	  /// Creates an instance from a populated map.
	  /// </summary>
	  /// <param name="map">  the map </param>
	  /// <returns> the explanatory map </returns>
	  public static ExplainMap of<T1>(IDictionary<T1> map)
	  {
		return new ExplainMap(map);
	  }

	  /// <summary>
	  /// Returns a builder for creating the map.
	  /// </summary>
	  /// <returns> the builder </returns>
	  public static ExplainMapBuilder builder()
	  {
		return new ExplainMapBuilder();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets a value by key.
	  /// </summary>
	  /// @param <R>  the type of the key </param>
	  /// <param name="key">  the key to lookup </param>
	  /// <returns> the value associated with the key </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <R> java.util.Optional<R> get(ExplainKey<R> key)
	  public Optional<R> get<R>(ExplainKey<R> key)
	  {
		return (Optional<R>) Optional.ofNullable(map.get(key));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the explanation as a string.
	  /// <para>
	  /// This returns a multi-line string containing the string form of the entries.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the explanation as a string </returns>
	  public string explanationString()
	  {
		StringBuilder buf = new StringBuilder(1024);
		buf.Append("ExplainMap ");
		explanationString(buf, "");
		buf.Append(Environment.NewLine);
		return buf.ToString();
	  }

	  // append the explanation with indent
	  private void explanationString(StringBuilder buf, string indent)
	  {
		buf.Append("{").Append(Environment.NewLine);
		string entryIndent = indent + "  ";
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<ExplainKey<?>, Object> entry : map.entrySet())
		foreach (KeyValuePair<ExplainKey<object>, object> entry in map.entrySet())
		{
		  buf.Append(entryIndent).Append(entry.Key).Append(" = ");
		  if (entry.Value is System.Collections.IList)
		  {
			// list
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") java.util.List<ExplainMap> list = (java.util.List<ExplainMap>) entry.getValue();
			IList<ExplainMap> list = (IList<ExplainMap>) entry.Value;
			explanationString(buf, entryIndent, list);
		  }
		  else
		  {
			// single entry
			try
			{
			  buf.Append(StringConvert.INSTANCE.convertToString(entry.Value));
			}
			catch (Exception)
			{
			  buf.Append(entry.Value);
			}
		  }
		  buf.Append(',').Append(Environment.NewLine);
		}
		if (!map.entrySet().Empty)
		{
		  buf.Remove(buf.lastIndexOf(","), 1);
		}
		buf.Append(indent).Append("}");
	  }

	  // append a list of entries
	  private void explanationString(StringBuilder buf, string indent, IList<ExplainMap> list)
	  {
		if (list.Count == 0)
		{
		  buf.Append("[]");
		}
		else
		{
		  buf.Append("[");
		  foreach (ExplainMap child in list)
		  {
			child.explanationString(buf, indent);
			buf.Append(',');
		  }
		  buf[buf.Length - 1] = ']';
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ExplainMap}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ExplainMap.Meta meta()
	  {
		return ExplainMap.Meta.INSTANCE;
	  }

	  static ExplainMap()
	  {
		MetaBean.register(ExplainMap.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ExplainMap<T1>(IDictionary<T1> map)
	  {
		JodaBeanUtils.notNull(map, "map");
		this.map = ImmutableMap.copyOf(map);
	  }

	  public override ExplainMap.Meta metaBean()
	  {
		return ExplainMap.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the map of explanatory values. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<ExplainKey<?>, Object> getMap()
	  public ImmutableMap<ExplainKey<object>, object> Map
	  {
		  get
		  {
			return map;
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
		  ExplainMap other = (ExplainMap) obj;
		  return JodaBeanUtils.equal(map, other.map);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(map);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("ExplainMap{");
		buf.Append("map").Append('=').Append(JodaBeanUtils.ToString(map));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ExplainMap}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  map_Renamed = DirectMetaProperty.ofImmutable(this, "map", typeof(ExplainMap), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "map");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code map} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<ExplainKey<?>, Object>> map = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "map", ExplainMap.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<ExplainKey<object>, object>> map_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "map");
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
			case 107868: // map
			  return map_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ExplainMap> builder()
		public override BeanBuilder<ExplainMap> builder()
		{
		  return new ExplainMap.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ExplainMap);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code map} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<ExplainKey<?>, Object>> map()
		public MetaProperty<ImmutableMap<ExplainKey<object>, object>> map()
		{
		  return map_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 107868: // map
			  return ((ExplainMap) bean).Map;
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
	  /// The bean-builder for {@code ExplainMap}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ExplainMap>
	  {

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<ExplainKey<?>, Object> map = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<ExplainKey<object>, object> map = ImmutableMap.of();

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
			case 107868: // map
			  return map;
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
			case 107868: // map
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.map = (java.util.Map<ExplainKey<?>, Object>) newValue;
			  this.map = (IDictionary<ExplainKey<object>, object>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ExplainMap build()
		{
		  return new ExplainMap(map);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("ExplainMap.Builder{");
		  buf.Append("map").Append('=').Append(JodaBeanUtils.ToString(map));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}