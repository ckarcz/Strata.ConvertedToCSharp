using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;

	/// <summary>
	/// An immutable floating rate index name, such as Libor, Euribor or US Fed Fund.
	/// <para>
	/// This is the standard immutable implementation of <seealso cref="FloatingRateName"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "package") public final class ImmutableFloatingRateName implements FloatingRateName, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableFloatingRateName : FloatingRateName, ImmutableBean
	{

	  /// <summary>
	  /// Special suffix that can be used to distinguish averaged indices.
	  /// </summary>
	  private const string AVERAGE_SUFFIX = "-AVG";

	  /// <summary>
	  /// The external name, typically from FpML, such as 'GBP-LIBOR-BBA'.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final String externalName;
	  private readonly string externalName;
	  /// <summary>
	  /// The root of the name of the index, such as 'GBP-LIBOR', to which the tenor is appended.
	  /// This name matches that used by <seealso cref="IborIndex"/> or <seealso cref="OvernightIndex"/>.
	  /// Typically, multiple {@code FloatingRateName} names map to one Ibor or Overnight index.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final String indexName;
	  private readonly string indexName;
	  /// <summary>
	  /// The type of the index.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final FloatingRateType type;
	  private readonly FloatingRateType type;
	  /// <summary>
	  /// The fixing date offset, in days, optional.
	  /// This is used when a floating rate name implies a non-standard fixing date offset.
	  /// This is only used for Ibor Indices, and currently only for DKK CIBOR.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<int> fixingDateOffsetDays;
	  private readonly int? fixingDateOffsetDays;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified external name, index name and type.
	  /// </summary>
	  /// <param name="externalName">  the unique name </param>
	  /// <param name="indexName">  the name of the index </param>
	  /// <param name="type">  the type - Ibor, Overnight or Price </param>
	  /// <returns> the name </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
	  public static ImmutableFloatingRateName of(string externalName, string indexName, FloatingRateType type)
	  {
		return new ImmutableFloatingRateName(externalName, indexName, type, null);
	  }

	  /// <summary>
	  /// Obtains an instance from the specified external name, index name and type.
	  /// </summary>
	  /// <param name="externalName">  the unique name </param>
	  /// <param name="indexName">  the name of the index </param>
	  /// <param name="type">  the type - Ibor, Overnight or Price </param>
	  /// <param name="fixingDateOffsetDays">  the fixing date offset, in days, negative to use the standard </param>
	  /// <returns> the name </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
	  public static ImmutableFloatingRateName of(string externalName, string indexName, FloatingRateType type, int fixingDateOffsetDays)
	  {

		return new ImmutableFloatingRateName(externalName, indexName, type, fixingDateOffsetDays >= 0 ? fixingDateOffsetDays : null);
	  }

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return externalName;
		  }
	  }

	  public ISet<Tenor> Tenors
	  {
		  get
		  {
			if (!type.Ibor)
			{
			  return ImmutableSet.of();
			}
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return IborIndex.extendedEnum().lookupAll().values().Where(index => index.Name.StartsWith(indexName)).Where(index => index.Active).Select(index => index.Tenor).OrderBy(c => c).collect(toImmutableSet());
		  }
	  }

	  public FloatingRateName normalized()
	  {
		if (type.Ibor && indexName.EndsWith("-", StringComparison.Ordinal))
		{
		  return FloatingRateName.of(indexName.Substring(0, indexName.Length - 1));
		}
		return FloatingRateName.of(indexName);
	  }

	  //-------------------------------------------------------------------------
	  public IborIndex toIborIndex(Tenor tenor)
	  {
		if (!type.Ibor)
		{
		  throw new System.InvalidOperationException("Incorrect index type, expected Ibor: " + externalName);
		}
		return IborIndex.of(indexName + tenor.normalized().ToString());
	  }

	  public override DaysAdjustment toIborIndexFixingOffset()
	  {
		DaysAdjustment @base = FloatingRateName.this.toIborIndexFixingOffset();
		if (fixingDateOffsetDays == null)
		{
		  return @base;
		}
		if (fixingDateOffsetDays == 0)
		{
		  return DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(PRECEDING, @base.ResultCalendar));
		}
		return @base.toBuilder().days(fixingDateOffsetDays.Value).build().normalized();
	  }

	  public OvernightIndex toOvernightIndex()
	  {
		if (!type.Overnight)
		{
		  throw new System.InvalidOperationException("Incorrect index type, expected Overnight: " + externalName);
		}
		if (indexName.EndsWith(AVERAGE_SUFFIX, StringComparison.Ordinal))
		{
		  return OvernightIndex.of(indexName.Substring(0, indexName.Length - 4));
		}
		return OvernightIndex.of(indexName);
	  }

	  public PriceIndex toPriceIndex()
	  {
		if (!type.Price)
		{
		  throw new System.InvalidOperationException("Incorrect index type, expected Price: " + externalName);
		}
		return PriceIndex.of(indexName);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is ImmutableFloatingRateName)
		{
		  return externalName.Equals(((ImmutableFloatingRateName) obj).externalName);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return externalName.GetHashCode();
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
	  /// The meta-bean for {@code ImmutableFloatingRateName}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableFloatingRateName.Meta meta()
	  {
		return ImmutableFloatingRateName.Meta.INSTANCE;
	  }

	  static ImmutableFloatingRateName()
	  {
		MetaBean.register(ImmutableFloatingRateName.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  internal static ImmutableFloatingRateName.Builder builder()
	  {
		return new ImmutableFloatingRateName.Builder();
	  }

	  private ImmutableFloatingRateName(string externalName, string indexName, FloatingRateType type, int? fixingDateOffsetDays)
	  {
		JodaBeanUtils.notEmpty(externalName, "externalName");
		JodaBeanUtils.notEmpty(indexName, "indexName");
		JodaBeanUtils.notNull(type, "type");
		this.externalName = externalName;
		this.indexName = indexName;
		this.type = type;
		this.fixingDateOffsetDays = fixingDateOffsetDays;
	  }

	  public override ImmutableFloatingRateName.Meta metaBean()
	  {
		return ImmutableFloatingRateName.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the external name, typically from FpML, such as 'GBP-LIBOR-BBA'. </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public string ExternalName
	  {
		  get
		  {
			return externalName;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the root of the name of the index, such as 'GBP-LIBOR', to which the tenor is appended.
	  /// This name matches that used by <seealso cref="IborIndex"/> or <seealso cref="OvernightIndex"/>.
	  /// Typically, multiple {@code FloatingRateName} names map to one Ibor or Overnight index. </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public string IndexName
	  {
		  get
		  {
			return indexName;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of the index. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FloatingRateType Type
	  {
		  get
		  {
			return type;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixing date offset, in days, optional.
	  /// This is used when a floating rate name implies a non-standard fixing date offset.
	  /// This is only used for Ibor Indices, and currently only for DKK CIBOR. </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public int? FixingDateOffsetDays
	  {
		  get
		  {
			return fixingDateOffsetDays != null ? int?.of(fixingDateOffsetDays) : int?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  internal Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableFloatingRateName}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  externalName_Renamed = DirectMetaProperty.ofImmutable(this, "externalName", typeof(ImmutableFloatingRateName), typeof(string));
			  indexName_Renamed = DirectMetaProperty.ofImmutable(this, "indexName", typeof(ImmutableFloatingRateName), typeof(string));
			  type_Renamed = DirectMetaProperty.ofImmutable(this, "type", typeof(ImmutableFloatingRateName), typeof(FloatingRateType));
			  fixingDateOffsetDays_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDateOffsetDays", typeof(ImmutableFloatingRateName), typeof(Integer));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "externalName", "indexName", "type", "fixingDateOffsetDays");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code externalName} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> externalName_Renamed;
		/// <summary>
		/// The meta-property for the {@code indexName} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> indexName_Renamed;
		/// <summary>
		/// The meta-property for the {@code type} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FloatingRateType> type_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDateOffsetDays} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> fixingDateOffsetDays_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "externalName", "indexName", "type", "fixingDateOffsetDays");
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
			case -1386121994: // externalName
			  return externalName_Renamed;
			case -807707011: // indexName
			  return indexName_Renamed;
			case 3575610: // type
			  return type_Renamed;
			case -594001179: // fixingDateOffsetDays
			  return fixingDateOffsetDays_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableFloatingRateName.Builder builder()
		{
		  return new ImmutableFloatingRateName.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableFloatingRateName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code externalName} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> externalName()
		{
		  return externalName_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code indexName} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> indexName()
		{
		  return indexName_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code type} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FloatingRateType> type()
		{
		  return type_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDateOffsetDays} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> fixingDateOffsetDays()
		{
		  return fixingDateOffsetDays_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1386121994: // externalName
			  return ((ImmutableFloatingRateName) bean).ExternalName;
			case -807707011: // indexName
			  return ((ImmutableFloatingRateName) bean).IndexName;
			case 3575610: // type
			  return ((ImmutableFloatingRateName) bean).Type;
			case -594001179: // fixingDateOffsetDays
			  return ((ImmutableFloatingRateName) bean).fixingDateOffsetDays;
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
	  /// The bean-builder for {@code ImmutableFloatingRateName}.
	  /// </summary>
	  internal sealed class Builder : DirectFieldsBeanBuilder<ImmutableFloatingRateName>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string externalName_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string indexName_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FloatingRateType type_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal int? fixingDateOffsetDays_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ImmutableFloatingRateName beanToCopy)
		{
		  this.externalName_Renamed = beanToCopy.ExternalName;
		  this.indexName_Renamed = beanToCopy.IndexName;
		  this.type_Renamed = beanToCopy.Type;
		  this.fixingDateOffsetDays_Renamed = beanToCopy.fixingDateOffsetDays;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1386121994: // externalName
			  return externalName_Renamed;
			case -807707011: // indexName
			  return indexName_Renamed;
			case 3575610: // type
			  return type_Renamed;
			case -594001179: // fixingDateOffsetDays
			  return fixingDateOffsetDays_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1386121994: // externalName
			  this.externalName_Renamed = (string) newValue;
			  break;
			case -807707011: // indexName
			  this.indexName_Renamed = (string) newValue;
			  break;
			case 3575610: // type
			  this.type_Renamed = (FloatingRateType) newValue;
			  break;
			case -594001179: // fixingDateOffsetDays
			  this.fixingDateOffsetDays_Renamed = (int?) newValue;
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

		public override ImmutableFloatingRateName build()
		{
		  return new ImmutableFloatingRateName(externalName_Renamed, indexName_Renamed, type_Renamed, fixingDateOffsetDays_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the external name, typically from FpML, such as 'GBP-LIBOR-BBA'. </summary>
		/// <param name="externalName">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder externalName(string externalName)
		{
		  JodaBeanUtils.notEmpty(externalName, "externalName");
		  this.externalName_Renamed = externalName;
		  return this;
		}

		/// <summary>
		/// Sets the root of the name of the index, such as 'GBP-LIBOR', to which the tenor is appended.
		/// This name matches that used by <seealso cref="IborIndex"/> or <seealso cref="OvernightIndex"/>.
		/// Typically, multiple {@code FloatingRateName} names map to one Ibor or Overnight index. </summary>
		/// <param name="indexName">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder indexName(string indexName)
		{
		  JodaBeanUtils.notEmpty(indexName, "indexName");
		  this.indexName_Renamed = indexName;
		  return this;
		}

		/// <summary>
		/// Sets the type of the index. </summary>
		/// <param name="type">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder type(FloatingRateType type)
		{
		  JodaBeanUtils.notNull(type, "type");
		  this.type_Renamed = type;
		  return this;
		}

		/// <summary>
		/// Sets the fixing date offset, in days, optional.
		/// This is used when a floating rate name implies a non-standard fixing date offset.
		/// This is only used for Ibor Indices, and currently only for DKK CIBOR. </summary>
		/// <param name="fixingDateOffsetDays">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingDateOffsetDays(int? fixingDateOffsetDays)
		{
		  this.fixingDateOffsetDays_Renamed = fixingDateOffsetDays;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ImmutableFloatingRateName.Builder{");
		  buf.Append("externalName").Append('=').Append(JodaBeanUtils.ToString(externalName_Renamed)).Append(',').Append(' ');
		  buf.Append("indexName").Append('=').Append(JodaBeanUtils.ToString(indexName_Renamed)).Append(',').Append(' ');
		  buf.Append("type").Append('=').Append(JodaBeanUtils.ToString(type_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDateOffsetDays").Append('=').Append(JodaBeanUtils.ToString(fixingDateOffsetDays_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}