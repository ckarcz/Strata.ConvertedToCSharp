using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
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

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Country = com.opengamma.strata.basics.location.Country;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;

	/// <summary>
	/// A price index implementation based on an immutable set of rules.
	/// <para>
	/// A standard immutable implementation of <seealso cref="PriceIndex"/>.
	/// </para>
	/// <para>
	/// In most cases, applications should refer to indices by name, using <seealso cref="PriceIndex#of(String)"/>.
	/// The named index will typically be resolved to an instance of this class.
	/// As such, it is recommended to use the {@code PriceIndex} interface in application
	/// code rather than directly referring to this class.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutablePriceIndex implements PriceIndex, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutablePriceIndex : PriceIndex, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final String name;
		private readonly string name;
	  /// <summary>
	  /// The region of the index.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.location.Country region;
	  private readonly Country region;
	  /// <summary>
	  /// The currency of the index.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
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
	  /// The publication frequency of the index.
	  /// Most price indices are published monthly, but some are published quarterly.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.schedule.Frequency publicationFrequency;
	  private readonly Frequency publicationFrequency;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.active_Renamed = true;
	  }

	  public FloatingRateName FloatingRateName
	  {
		  get
		  {
			return FloatingRateName.of(name);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is ImmutablePriceIndex)
		{
		  return name.Equals(((ImmutablePriceIndex) obj).name);
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
	  /// The meta-bean for {@code ImmutablePriceIndex}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutablePriceIndex.Meta meta()
	  {
		return ImmutablePriceIndex.Meta.INSTANCE;
	  }

	  static ImmutablePriceIndex()
	  {
		MetaBean.register(ImmutablePriceIndex.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutablePriceIndex.Builder builder()
	  {
		return new ImmutablePriceIndex.Builder();
	  }

	  private ImmutablePriceIndex(string name, Country region, Currency currency, bool active, Frequency publicationFrequency)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(region, "region");
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(publicationFrequency, "publicationFrequency");
		this.name = name;
		this.region = region;
		this.currency = currency;
		this.active = active;
		this.publicationFrequency = publicationFrequency;
	  }

	  public override ImmutablePriceIndex.Meta metaBean()
	  {
		return ImmutablePriceIndex.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index name, such as 'GB-HICP'. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the region of the index. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Country Region
	  {
		  get
		  {
			return region;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the index. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
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
	  /// Gets the publication frequency of the index.
	  /// Most price indices are published monthly, but some are published quarterly. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Frequency PublicationFrequency
	  {
		  get
		  {
			return publicationFrequency;
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
	  /// The meta-bean for {@code ImmutablePriceIndex}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutablePriceIndex), typeof(string));
			  region_Renamed = DirectMetaProperty.ofImmutable(this, "region", typeof(ImmutablePriceIndex), typeof(Country));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(ImmutablePriceIndex), typeof(Currency));
			  active_Renamed = DirectMetaProperty.ofImmutable(this, "active", typeof(ImmutablePriceIndex), Boolean.TYPE);
			  publicationFrequency_Renamed = DirectMetaProperty.ofImmutable(this, "publicationFrequency", typeof(ImmutablePriceIndex), typeof(Frequency));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "region", "currency", "active", "publicationFrequency");
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
		/// The meta-property for the {@code region} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Country> region_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code active} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> active_Renamed;
		/// <summary>
		/// The meta-property for the {@code publicationFrequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> publicationFrequency_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "region", "currency", "active", "publicationFrequency");
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
			case -934795532: // region
			  return region_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case -1422950650: // active
			  return active_Renamed;
			case -1407208304: // publicationFrequency
			  return publicationFrequency_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutablePriceIndex.Builder builder()
		{
		  return new ImmutablePriceIndex.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutablePriceIndex);
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
		/// The meta-property for the {@code region} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Country> region()
		{
		  return region_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code active} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> active()
		{
		  return active_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code publicationFrequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> publicationFrequency()
		{
		  return publicationFrequency_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((ImmutablePriceIndex) bean).Name;
			case -934795532: // region
			  return ((ImmutablePriceIndex) bean).Region;
			case 575402001: // currency
			  return ((ImmutablePriceIndex) bean).Currency;
			case -1422950650: // active
			  return ((ImmutablePriceIndex) bean).Active;
			case -1407208304: // publicationFrequency
			  return ((ImmutablePriceIndex) bean).PublicationFrequency;
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
	  /// The bean-builder for {@code ImmutablePriceIndex}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutablePriceIndex>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Country region_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool active_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency publicationFrequency_Renamed;

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
		internal Builder(ImmutablePriceIndex beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.region_Renamed = beanToCopy.Region;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.active_Renamed = beanToCopy.Active;
		  this.publicationFrequency_Renamed = beanToCopy.PublicationFrequency;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case -934795532: // region
			  return region_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case -1422950650: // active
			  return active_Renamed;
			case -1407208304: // publicationFrequency
			  return publicationFrequency_Renamed;
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
			case -934795532: // region
			  this.region_Renamed = (Country) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case -1422950650: // active
			  this.active_Renamed = (bool?) newValue.Value;
			  break;
			case -1407208304: // publicationFrequency
			  this.publicationFrequency_Renamed = (Frequency) newValue;
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

		public override ImmutablePriceIndex build()
		{
		  return new ImmutablePriceIndex(name_Renamed, region_Renamed, currency_Renamed, active_Renamed, publicationFrequency_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the index name, such as 'GB-HICP'. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the region of the index. </summary>
		/// <param name="region">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder region(Country region)
		{
		  JodaBeanUtils.notNull(region, "region");
		  this.region_Renamed = region;
		  return this;
		}

		/// <summary>
		/// Sets the currency of the index. </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
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
		/// Sets the publication frequency of the index.
		/// Most price indices are published monthly, but some are published quarterly. </summary>
		/// <param name="publicationFrequency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder publicationFrequency(Frequency publicationFrequency)
		{
		  JodaBeanUtils.notNull(publicationFrequency, "publicationFrequency");
		  this.publicationFrequency_Renamed = publicationFrequency;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("ImmutablePriceIndex.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("region").Append('=').Append(JodaBeanUtils.ToString(region_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("active").Append('=').Append(JodaBeanUtils.ToString(active_Renamed)).Append(',').Append(' ');
		  buf.Append("publicationFrequency").Append('=').Append(JodaBeanUtils.ToString(publicationFrequency_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}