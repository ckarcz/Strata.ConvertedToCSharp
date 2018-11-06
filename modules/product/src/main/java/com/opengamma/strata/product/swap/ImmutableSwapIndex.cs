using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
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

	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;

	/// <summary>
	/// A swap index implementation based on an immutable set of rules.
	/// <para>
	/// A standard immutable implementation of <seealso cref="SwapIndex"/> that defines the swap trade template,
	/// including the swap convention and tenor.
	/// </para>
	/// <para>
	/// In most cases, applications should refer to indices by name, using <seealso cref="SwapIndex#of(String)"/>.
	/// The named index will typically be resolved to an instance of this class.
	/// As such, it is recommended to use the {@code SwapIndex} interface in application
	/// code rather than directly referring to this class.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableSwapIndex implements SwapIndex, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableSwapIndex : SwapIndex, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", overrideGet = true) private final String name;
		private readonly string name;
	  /// <summary>
	  /// Whether the index is active, defaulted to true.
	  /// <para>
	  /// Over time some indices become inactive and are no longer produced.
	  /// If this occurs, this flag will be set to false.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final boolean active;
	  private readonly bool active;
	  /// <summary>
	  /// The fixing time.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalTime fixingTime;
	  private readonly LocalTime fixingTime;
	  /// <summary>
	  /// The time-zone of the fixing time.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.ZoneId fixingZone;
	  private readonly ZoneId fixingZone;
	  /// <summary>
	  /// The template for creating Fixed-Ibor swap.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.swap.type.FixedIborSwapTemplate template;
	  private readonly FixedIborSwapTemplate template;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name, time and template.
	  /// </summary>
	  /// <param name="name">  the index name </param>
	  /// <param name="fixingTime">  the fixing time </param>
	  /// <param name="fixingZone">  the time-zone of the fixing time </param>
	  /// <param name="template">  the swap template </param>
	  /// <returns> the index </returns>
	  public static ImmutableSwapIndex of(string name, LocalTime fixingTime, ZoneId fixingZone, FixedIborSwapTemplate template)
	  {
		return new ImmutableSwapIndex(name, true, fixingTime, fixingZone, template);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.active_Renamed = true;
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is ImmutableSwapIndex)
		{
		  return name.Equals(((ImmutableSwapIndex) obj).name);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return name.GetHashCode();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the name of the index.
	  /// </summary>
	  /// <returns> the name of the index </returns>
	  public override string ToString()
	  {
		return Name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableSwapIndex}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableSwapIndex.Meta meta()
	  {
		return ImmutableSwapIndex.Meta.INSTANCE;
	  }

	  static ImmutableSwapIndex()
	  {
		MetaBean.register(ImmutableSwapIndex.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableSwapIndex.Builder builder()
	  {
		return new ImmutableSwapIndex.Builder();
	  }

	  private ImmutableSwapIndex(string name, bool active, LocalTime fixingTime, ZoneId fixingZone, FixedIborSwapTemplate template)
	  {
		JodaBeanUtils.notEmpty(name, "name");
		JodaBeanUtils.notNull(fixingTime, "fixingTime");
		JodaBeanUtils.notNull(fixingZone, "fixingZone");
		JodaBeanUtils.notNull(template, "template");
		this.name = name;
		this.active = active;
		this.fixingTime = fixingTime;
		this.fixingZone = fixingZone;
		this.template = template;
	  }

	  public override ImmutableSwapIndex.Meta metaBean()
	  {
		return ImmutableSwapIndex.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index name. </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the index is active, defaulted to true.
	  /// <para>
	  /// Over time some indices become inactive and are no longer produced.
	  /// If this occurs, this flag will be set to false.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public bool Active
	  {
		  get
		  {
			return active;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixing time. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalTime FixingTime
	  {
		  get
		  {
			return fixingTime;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time-zone of the fixing time. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZoneId FixingZone
	  {
		  get
		  {
			return fixingZone;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the template for creating Fixed-Ibor swap. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FixedIborSwapTemplate Template
	  {
		  get
		  {
			return template;
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

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableSwapIndex}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableSwapIndex), typeof(string));
			  active_Renamed = DirectMetaProperty.ofImmutable(this, "active", typeof(ImmutableSwapIndex), Boolean.TYPE);
			  fixingTime_Renamed = DirectMetaProperty.ofImmutable(this, "fixingTime", typeof(ImmutableSwapIndex), typeof(LocalTime));
			  fixingZone_Renamed = DirectMetaProperty.ofImmutable(this, "fixingZone", typeof(ImmutableSwapIndex), typeof(ZoneId));
			  template_Renamed = DirectMetaProperty.ofImmutable(this, "template", typeof(ImmutableSwapIndex), typeof(FixedIborSwapTemplate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "active", "fixingTime", "fixingZone", "template");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code active} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> active_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalTime> fixingTime_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingZone} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZoneId> fixingZone_Renamed;
		/// <summary>
		/// The meta-property for the {@code template} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixedIborSwapTemplate> template_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "active", "fixingTime", "fixingZone", "template");
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
			case 3373707: // name
			  return name_Renamed;
			case -1422950650: // active
			  return active_Renamed;
			case 1255686170: // fixingTime
			  return fixingTime_Renamed;
			case 1255870713: // fixingZone
			  return fixingZone_Renamed;
			case -1321546630: // template
			  return template_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableSwapIndex.Builder builder()
		{
		  return new ImmutableSwapIndex.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableSwapIndex);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code active} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> active()
		{
		  return active_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalTime> fixingTime()
		{
		  return fixingTime_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingZone} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZoneId> fixingZone()
		{
		  return fixingZone_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code template} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixedIborSwapTemplate> template()
		{
		  return template_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((ImmutableSwapIndex) bean).Name;
			case -1422950650: // active
			  return ((ImmutableSwapIndex) bean).Active;
			case 1255686170: // fixingTime
			  return ((ImmutableSwapIndex) bean).FixingTime;
			case 1255870713: // fixingZone
			  return ((ImmutableSwapIndex) bean).FixingZone;
			case -1321546630: // template
			  return ((ImmutableSwapIndex) bean).Template;
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
	  /// The bean-builder for {@code ImmutableSwapIndex}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableSwapIndex>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool active_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalTime fixingTime_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZoneId fixingZone_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixedIborSwapTemplate template_Renamed;

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
		internal Builder(ImmutableSwapIndex beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.active_Renamed = beanToCopy.Active;
		  this.fixingTime_Renamed = beanToCopy.FixingTime;
		  this.fixingZone_Renamed = beanToCopy.FixingZone;
		  this.template_Renamed = beanToCopy.Template;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case -1422950650: // active
			  return active_Renamed;
			case 1255686170: // fixingTime
			  return fixingTime_Renamed;
			case 1255870713: // fixingZone
			  return fixingZone_Renamed;
			case -1321546630: // template
			  return template_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name_Renamed = (string) newValue;
			  break;
			case -1422950650: // active
			  this.active_Renamed = (bool?) newValue.Value;
			  break;
			case 1255686170: // fixingTime
			  this.fixingTime_Renamed = (LocalTime) newValue;
			  break;
			case 1255870713: // fixingZone
			  this.fixingZone_Renamed = (ZoneId) newValue;
			  break;
			case -1321546630: // template
			  this.template_Renamed = (FixedIborSwapTemplate) newValue;
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

		public override ImmutableSwapIndex build()
		{
		  return new ImmutableSwapIndex(name_Renamed, active_Renamed, fixingTime_Renamed, fixingZone_Renamed, template_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the index name. </summary>
		/// <param name="name">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  JodaBeanUtils.notEmpty(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets whether the index is active, defaulted to true.
		/// <para>
		/// Over time some indices become inactive and are no longer produced.
		/// If this occurs, this flag will be set to false.
		/// </para>
		/// </summary>
		/// <param name="active">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder active(bool active)
		{
		  this.active_Renamed = active;
		  return this;
		}

		/// <summary>
		/// Sets the fixing time. </summary>
		/// <param name="fixingTime">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingTime(LocalTime fixingTime)
		{
		  JodaBeanUtils.notNull(fixingTime, "fixingTime");
		  this.fixingTime_Renamed = fixingTime;
		  return this;
		}

		/// <summary>
		/// Sets the time-zone of the fixing time. </summary>
		/// <param name="fixingZone">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingZone(ZoneId fixingZone)
		{
		  JodaBeanUtils.notNull(fixingZone, "fixingZone");
		  this.fixingZone_Renamed = fixingZone;
		  return this;
		}

		/// <summary>
		/// Sets the template for creating Fixed-Ibor swap. </summary>
		/// <param name="template">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder template(FixedIborSwapTemplate template)
		{
		  JodaBeanUtils.notNull(template, "template");
		  this.template_Renamed = template;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("ImmutableSwapIndex.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("active").Append('=').Append(JodaBeanUtils.ToString(active_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingTime").Append('=').Append(JodaBeanUtils.ToString(fixingTime_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingZone").Append('=').Append(JodaBeanUtils.ToString(fixingZone_Renamed)).Append(',').Append(' ');
		  buf.Append("template").Append('=').Append(JodaBeanUtils.ToString(template_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}