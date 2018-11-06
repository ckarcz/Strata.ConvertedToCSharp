using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Defines the computation of an averaged daily rate for a single Overnight index.
	/// <para>
	/// An interest rate determined directly from an Overnight index by averaging the value
	/// of each day's rate over the period strictly between the start date and end date. 
	/// </para>
	/// <para>
	/// The start date and end date can be non-business days. 
	/// The average is taken on calendar days between the start and end dates.
	/// </para>
	/// <para>
	/// If a day in the period is not a business day on the fixing calendar of the Overnight index, 
	/// the overnight rate fixed on the previous business day is used.
	/// </para>
	/// <para>
	/// For example, a rate determined averaging values from 'USD-FED-FUND'.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class OvernightAveragedDailyRateComputation implements OvernightRateComputation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class OvernightAveragedDailyRateComputation : OvernightRateComputation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.OvernightIndex index;
		private readonly OvernightIndex index;
	  /// <summary>
	  /// The resolved calendar that the index uses.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.HolidayCalendar fixingCalendar;
	  private readonly HolidayCalendar fixingCalendar;
	  /// <summary>
	  /// The start date of the accrual period.
	  /// <para>
	  /// This is not necessarily a valid business day.
	  /// In this case, the first fixing date is the previous business day of the start date on {@code fixingCalendar}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date of the accrual period.
	  /// <para>
	  /// This is not necessarily a valid business day.
	  /// In this case, the last fixing date is the previous business day of the end date on {@code fixingCalendar}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance from an index and accrual period dates
	  /// <para>
	  /// The dates represent the accrual period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="startDate">  the first date of the accrual period </param>
	  /// <param name="endDate">  the last date of the accrual period </param>
	  /// <param name="refData">  the reference data to use when resolving holiday calendars </param>
	  /// <returns> the rate computation </returns>
	  public static OvernightAveragedDailyRateComputation of(OvernightIndex index, LocalDate startDate, LocalDate endDate, ReferenceData refData)
	  {

		return OvernightAveragedDailyRateComputation.builder().index(index).fixingCalendar(index.FixingCalendar.resolve(refData)).startDate(startDate).endDate(endDate).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightAveragedDailyRateComputation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static OvernightAveragedDailyRateComputation.Meta meta()
	  {
		return OvernightAveragedDailyRateComputation.Meta.INSTANCE;
	  }

	  static OvernightAveragedDailyRateComputation()
	  {
		MetaBean.register(OvernightAveragedDailyRateComputation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static OvernightAveragedDailyRateComputation.Builder builder()
	  {
		return new OvernightAveragedDailyRateComputation.Builder();
	  }

	  private OvernightAveragedDailyRateComputation(OvernightIndex index, HolidayCalendar fixingCalendar, LocalDate startDate, LocalDate endDate)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(fixingCalendar, "fixingCalendar");
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		this.index = index;
		this.fixingCalendar = fixingCalendar;
		this.startDate = startDate;
		this.endDate = endDate;
		validate();
	  }

	  public override OvernightAveragedDailyRateComputation.Meta metaBean()
	  {
		return OvernightAveragedDailyRateComputation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Overnight index.
	  /// <para>
	  /// The rate to be paid is based on this index.
	  /// It will be a well known market index such as 'GBP-SONIA'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public OvernightIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the resolved calendar that the index uses. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public HolidayCalendar FixingCalendar
	  {
		  get
		  {
			return fixingCalendar;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of the accrual period.
	  /// <para>
	  /// This is not necessarily a valid business day.
	  /// In this case, the first fixing date is the previous business day of the start date on {@code fixingCalendar}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the end date of the accrual period.
	  /// <para>
	  /// This is not necessarily a valid business day.
	  /// In this case, the last fixing date is the previous business day of the end date on {@code fixingCalendar}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return endDate;
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
		  OvernightAveragedDailyRateComputation other = (OvernightAveragedDailyRateComputation) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(fixingCalendar, other.fixingCalendar) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingCalendar);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("OvernightAveragedDailyRateComputation{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("fixingCalendar").Append('=').Append(fixingCalendar).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightAveragedDailyRateComputation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(OvernightAveragedDailyRateComputation), typeof(OvernightIndex));
			  fixingCalendar_Renamed = DirectMetaProperty.ofImmutable(this, "fixingCalendar", typeof(OvernightAveragedDailyRateComputation), typeof(HolidayCalendar));
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(OvernightAveragedDailyRateComputation), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(OvernightAveragedDailyRateComputation), typeof(LocalDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "fixingCalendar", "startDate", "endDate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<OvernightIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingCalendar} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<HolidayCalendar> fixingCalendar_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "fixingCalendar", "startDate", "endDate");
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
			case 100346066: // index
			  return index_Renamed;
			case 394230283: // fixingCalendar
			  return fixingCalendar_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override OvernightAveragedDailyRateComputation.Builder builder()
		{
		  return new OvernightAveragedDailyRateComputation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(OvernightAveragedDailyRateComputation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<OvernightIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingCalendar} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<HolidayCalendar> fixingCalendar()
		{
		  return fixingCalendar_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((OvernightAveragedDailyRateComputation) bean).Index;
			case 394230283: // fixingCalendar
			  return ((OvernightAveragedDailyRateComputation) bean).FixingCalendar;
			case -2129778896: // startDate
			  return ((OvernightAveragedDailyRateComputation) bean).StartDate;
			case -1607727319: // endDate
			  return ((OvernightAveragedDailyRateComputation) bean).EndDate;
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
	  /// The bean-builder for {@code OvernightAveragedDailyRateComputation}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<OvernightAveragedDailyRateComputation>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal OvernightIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal HolidayCalendar fixingCalendar_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(OvernightAveragedDailyRateComputation beanToCopy)
		{
		  this.index_Renamed = beanToCopy.Index;
		  this.fixingCalendar_Renamed = beanToCopy.FixingCalendar;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return index_Renamed;
			case 394230283: // fixingCalendar
			  return fixingCalendar_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index_Renamed = (OvernightIndex) newValue;
			  break;
			case 394230283: // fixingCalendar
			  this.fixingCalendar_Renamed = (HolidayCalendar) newValue;
			  break;
			case -2129778896: // startDate
			  this.startDate_Renamed = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (LocalDate) newValue;
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

		public override OvernightAveragedDailyRateComputation build()
		{
		  return new OvernightAveragedDailyRateComputation(index_Renamed, fixingCalendar_Renamed, startDate_Renamed, endDate_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the Overnight index.
		/// <para>
		/// The rate to be paid is based on this index.
		/// It will be a well known market index such as 'GBP-SONIA'.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(OvernightIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the resolved calendar that the index uses. </summary>
		/// <param name="fixingCalendar">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingCalendar(HolidayCalendar fixingCalendar)
		{
		  JodaBeanUtils.notNull(fixingCalendar, "fixingCalendar");
		  this.fixingCalendar_Renamed = fixingCalendar;
		  return this;
		}

		/// <summary>
		/// Sets the start date of the accrual period.
		/// <para>
		/// This is not necessarily a valid business day.
		/// In this case, the first fixing date is the previous business day of the start date on {@code fixingCalendar}.
		/// </para>
		/// </summary>
		/// <param name="startDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDate(LocalDate startDate)
		{
		  JodaBeanUtils.notNull(startDate, "startDate");
		  this.startDate_Renamed = startDate;
		  return this;
		}

		/// <summary>
		/// Sets the end date of the accrual period.
		/// <para>
		/// This is not necessarily a valid business day.
		/// In this case, the last fixing date is the previous business day of the end date on {@code fixingCalendar}.
		/// </para>
		/// </summary>
		/// <param name="endDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDate(LocalDate endDate)
		{
		  JodaBeanUtils.notNull(endDate, "endDate");
		  this.endDate_Renamed = endDate;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("OvernightAveragedDailyRateComputation.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingCalendar").Append('=').Append(JodaBeanUtils.ToString(fixingCalendar_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}