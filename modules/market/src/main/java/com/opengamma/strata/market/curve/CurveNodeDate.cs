using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// The date of the curve node.
	/// <para>
	/// A {@code CurveNodeDate} provides a flexible mechanism of defining the date of the curve node.
	/// It may be associated with the end date, the last fixing date, or specified exactly.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CurveNodeDate implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CurveNodeDate : ImmutableBean
	{

	  /// <summary>
	  /// An instance defining the curve node date as the end date of the trade.
	  /// </summary>
	  public static readonly CurveNodeDate END = new CurveNodeDate(CurveNodeDateType.END, null);
	  /// <summary>
	  /// An instance defining the curve node date as the last fixing date date of the trade.
	  /// Used only for instruments referencing an Ibor index.
	  /// </summary>
	  public static readonly CurveNodeDate LAST_FIXING = new CurveNodeDate(CurveNodeDateType.LAST_FIXING, null);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The method by which the date of the node is calculated, defaulted to 'End'.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final CurveNodeDateType type;
	  private readonly CurveNodeDateType type;
	  /// <summary>
	  /// The fixed date to be used on the node, only used when the type is 'Fixed'.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final java.time.LocalDate date;
	  private readonly LocalDate date;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance specifying a fixed date.
	  /// <para>
	  /// This returns an instance with the type <seealso cref="CurveNodeDateType#FIXED"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the specific date </param>
	  /// <returns> an instance specifying a fixed date </returns>
	  public static CurveNodeDate of(LocalDate date)
	  {
		return new CurveNodeDate(CurveNodeDateType.FIXED, date);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.type = CurveNodeDateType.END;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (type.Equals(CurveNodeDateType.FIXED))
		{
		  ArgChecker.isTrue(date != null, "Date must be present when type is 'Fixed'");
		}
		else
		{
		  ArgChecker.isTrue(date == null, "Date must not be present unless type is 'Fixed'");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the type is 'End'.
	  /// </summary>
	  /// <returns> true if the type is 'End' </returns>
	  public bool End
	  {
		  get
		  {
			return (type == CurveNodeDateType.END);
		  }
	  }

	  /// <summary>
	  /// Checks if the type is 'LastFixing'.
	  /// </summary>
	  /// <returns> true if the type is 'LastFixing' </returns>
	  public bool LastFixing
	  {
		  get
		  {
			return (type == CurveNodeDateType.LAST_FIXING);
		  }
	  }

	  /// <summary>
	  /// Checks if the type is 'Fixed'.
	  /// </summary>
	  /// <returns> true if the type is 'Fixed' </returns>
	  public bool Fixed
	  {
		  get
		  {
			return (type == CurveNodeDateType.FIXED);
		  }
	  }

	  /// <summary>
	  /// Gets the node date if the type is 'Fixed'.
	  /// <para>
	  /// If the type is 'Fixed', this returns the node date.
	  /// Otherwise, this throws an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the node date, only available if the type is 'Fixed' </returns>
	  /// <exception cref="IllegalStateException"> if called when the type is not fixed </exception>
	  public LocalDate Date
	  {
		  get
		  {
			if (!Fixed)
			{
			  throw new System.InvalidOperationException(Messages.format("No date available for type '{}'", type));
			}
			return date;
		  }
	  }

	  /// <summary>
	  /// Calculates the appropriate date for the node.
	  /// </summary>
	  /// <param name="endDateSupplier">  the supplier invoked to get the end date </param>
	  /// <param name="lastFixingDateSupplier">  the supplier invoked to get the last fixing date </param>
	  /// <returns> the calculated date </returns>
	  public LocalDate calculate(System.Func<LocalDate> endDateSupplier, System.Func<LocalDate> lastFixingDateSupplier)
	  {
		switch (type.innerEnumValue)
		{
		  case com.opengamma.strata.market.curve.CurveNodeDateType.InnerEnum.FIXED:
			return date;
		  case com.opengamma.strata.market.curve.CurveNodeDateType.InnerEnum.END:
			return endDateSupplier();
		  case com.opengamma.strata.market.curve.CurveNodeDateType.InnerEnum.LAST_FIXING:
			return lastFixingDateSupplier();
		}
		throw new System.InvalidOperationException("Unknown curve node type");
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurveNodeDate}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CurveNodeDate.Meta meta()
	  {
		return CurveNodeDate.Meta.INSTANCE;
	  }

	  static CurveNodeDate()
	  {
		MetaBean.register(CurveNodeDate.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CurveNodeDate(CurveNodeDateType type, LocalDate date)
	  {
		this.type = type;
		this.date = date;
		validate();
	  }

	  public override CurveNodeDate.Meta metaBean()
	  {
		return CurveNodeDate.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the method by which the date of the node is calculated, defaulted to 'End'. </summary>
	  /// <returns> the value of the property </returns>
	  public CurveNodeDateType Type
	  {
		  get
		  {
			return type;
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
		  CurveNodeDate other = (CurveNodeDate) obj;
		  return JodaBeanUtils.equal(type, other.type) && JodaBeanUtils.equal(date, other.date);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(type);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(date);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("CurveNodeDate{");
		buf.Append("type").Append('=').Append(type).Append(',').Append(' ');
		buf.Append("date").Append('=').Append(JodaBeanUtils.ToString(date));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurveNodeDate}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  type_Renamed = DirectMetaProperty.ofImmutable(this, "type", typeof(CurveNodeDate), typeof(CurveNodeDateType));
			  date_Renamed = DirectMetaProperty.ofImmutable(this, "date", typeof(CurveNodeDate), typeof(LocalDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "type", "date");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code type} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveNodeDateType> type_Renamed;
		/// <summary>
		/// The meta-property for the {@code date} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> date_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "type", "date");
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
			case 3575610: // type
			  return type_Renamed;
			case 3076014: // date
			  return date_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CurveNodeDate> builder()
		public override BeanBuilder<CurveNodeDate> builder()
		{
		  return new CurveNodeDate.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CurveNodeDate);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code type} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveNodeDateType> type()
		{
		  return type_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code date} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> date()
		{
		  return date_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  return ((CurveNodeDate) bean).Type;
			case 3076014: // date
			  return ((CurveNodeDate) bean).date;
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
	  /// The bean-builder for {@code CurveNodeDate}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CurveNodeDate>
	  {

		internal CurveNodeDateType type;
		internal LocalDate date;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  return type;
			case 3076014: // date
			  return date;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  this.type = (CurveNodeDateType) newValue;
			  break;
			case 3076014: // date
			  this.date = (LocalDate) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CurveNodeDate build()
		{
		  return new CurveNodeDate(type, date);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("CurveNodeDate.Builder{");
		  buf.Append("type").Append('=').Append(JodaBeanUtils.ToString(type)).Append(',').Append(' ');
		  buf.Append("date").Append('=').Append(JodaBeanUtils.ToString(date));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}