using System;
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
	/// Defines the computation of a rate from a single Overnight index that is compounded daily.
	/// <para>
	/// An interest rate determined directly from an Overnight index with daily compounding.
	/// For example, a rate determined by compounding values from 'GBP-SONIA'.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class OvernightCompoundedRateComputation implements OvernightRateComputation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class OvernightCompoundedRateComputation : OvernightRateComputation, ImmutableBean
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
	  /// The fixing date associated with the start date of the accrual period.
	  /// <para>
	  /// This is also the first fixing date.
	  /// The overnight rate is observed from this date onwards.
	  /// </para>
	  /// <para>
	  /// In general, the fixing dates and accrual dates are the same for an overnight index.
	  /// However, in the case of a Tomorrow/Next index, the fixing period is one business day
	  /// before the accrual period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The fixing date associated with the end date of the accrual period.
	  /// <para>
	  /// The overnight rate is observed until this date.
	  /// </para>
	  /// <para>
	  /// In general, the fixing dates and accrual dates are the same for an overnight index.
	  /// However, in the case of a Tomorrow/Next index, the fixing period is one business day
	  /// before the accrual period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The number of business days before the end of the period that the rate is cut off.
	  /// <para>
	  /// When a rate cut-off applies, the final daily rate is determined this number of days
	  /// before the end of the period, with any subsequent days having the same rate.
	  /// </para>
	  /// <para>
	  /// The amount must be zero or positive.
	  /// A value of zero or one will have no effect on the standard calculation.
	  /// The fixing holiday calendar of the index is used to determine business days.
	  /// </para>
	  /// <para>
	  /// For example, a value of {@code 3} means that the rate observed on
	  /// {@code (periodEndDate - 3 business days)} is also to be used on
	  /// {@code (periodEndDate - 2 business days)} and {@code (periodEndDate - 1 business day)}.
	  /// </para>
	  /// <para>
	  /// If there are multiple accrual periods in the payment period, then this
	  /// should typically only be non-zero in the last accrual period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final int rateCutOffDays;
	  private readonly int rateCutOffDays;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance from an index and period dates
	  /// <para>
	  /// No rate cut-off applies.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="startDate">  the first date of the accrual period </param>
	  /// <param name="endDate">  the last date of the accrual period </param>
	  /// <param name="refData">  the reference data to use when resolving holiday calendars </param>
	  /// <returns> the rate computation </returns>
	  public static OvernightCompoundedRateComputation of(OvernightIndex index, LocalDate startDate, LocalDate endDate, ReferenceData refData)
	  {

		return of(index, startDate, endDate, 0, refData);
	  }

	  /// <summary>
	  /// Creates an instance from an index, period dates and rate cut-off.
	  /// <para>
	  /// Rate cut-off applies if the cut-off is 2 or greater.
	  /// A value of 0 or 1 should be used if no cut-off applies.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="startDate">  the first date of the accrual period </param>
	  /// <param name="endDate">  the last date of the accrual period </param>
	  /// <param name="rateCutOffDays">  the rate cut-off days offset, not negative </param>
	  /// <param name="refData">  the reference data to use when resolving holiday calendars </param>
	  /// <returns> the rate computation </returns>
	  public static OvernightCompoundedRateComputation of(OvernightIndex index, LocalDate startDate, LocalDate endDate, int rateCutOffDays, ReferenceData refData)
	  {

		return OvernightCompoundedRateComputation.builder().index(index).fixingCalendar(index.FixingCalendar.resolve(refData)).startDate(index.calculateFixingFromEffective(startDate, refData)).endDate(index.calculateFixingFromEffective(endDate, refData)).rateCutOffDays(rateCutOffDays).build();
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
	  /// The meta-bean for {@code OvernightCompoundedRateComputation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static OvernightCompoundedRateComputation.Meta meta()
	  {
		return OvernightCompoundedRateComputation.Meta.INSTANCE;
	  }

	  static OvernightCompoundedRateComputation()
	  {
		MetaBean.register(OvernightCompoundedRateComputation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static OvernightCompoundedRateComputation.Builder builder()
	  {
		return new OvernightCompoundedRateComputation.Builder();
	  }

	  private OvernightCompoundedRateComputation(OvernightIndex index, HolidayCalendar fixingCalendar, LocalDate startDate, LocalDate endDate, int rateCutOffDays)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(fixingCalendar, "fixingCalendar");
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		ArgChecker.notNegative(rateCutOffDays, "rateCutOffDays");
		this.index = index;
		this.fixingCalendar = fixingCalendar;
		this.startDate = startDate;
		this.endDate = endDate;
		this.rateCutOffDays = rateCutOffDays;
		validate();
	  }

	  public override OvernightCompoundedRateComputation.Meta metaBean()
	  {
		return OvernightCompoundedRateComputation.Meta.INSTANCE;
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
	  /// Gets the fixing date associated with the start date of the accrual period.
	  /// <para>
	  /// This is also the first fixing date.
	  /// The overnight rate is observed from this date onwards.
	  /// </para>
	  /// <para>
	  /// In general, the fixing dates and accrual dates are the same for an overnight index.
	  /// However, in the case of a Tomorrow/Next index, the fixing period is one business day
	  /// before the accrual period.
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
	  /// Gets the fixing date associated with the end date of the accrual period.
	  /// <para>
	  /// The overnight rate is observed until this date.
	  /// </para>
	  /// <para>
	  /// In general, the fixing dates and accrual dates are the same for an overnight index.
	  /// However, in the case of a Tomorrow/Next index, the fixing period is one business day
	  /// before the accrual period.
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
	  /// Gets the number of business days before the end of the period that the rate is cut off.
	  /// <para>
	  /// When a rate cut-off applies, the final daily rate is determined this number of days
	  /// before the end of the period, with any subsequent days having the same rate.
	  /// </para>
	  /// <para>
	  /// The amount must be zero or positive.
	  /// A value of zero or one will have no effect on the standard calculation.
	  /// The fixing holiday calendar of the index is used to determine business days.
	  /// </para>
	  /// <para>
	  /// For example, a value of {@code 3} means that the rate observed on
	  /// {@code (periodEndDate - 3 business days)} is also to be used on
	  /// {@code (periodEndDate - 2 business days)} and {@code (periodEndDate - 1 business day)}.
	  /// </para>
	  /// <para>
	  /// If there are multiple accrual periods in the payment period, then this
	  /// should typically only be non-zero in the last accrual period.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public int RateCutOffDays
	  {
		  get
		  {
			return rateCutOffDays;
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
		  OvernightCompoundedRateComputation other = (OvernightCompoundedRateComputation) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(fixingCalendar, other.fixingCalendar) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && (rateCutOffDays == other.rateCutOffDays);
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
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rateCutOffDays);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("OvernightCompoundedRateComputation{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("fixingCalendar").Append('=').Append(fixingCalendar).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("rateCutOffDays").Append('=').Append(JodaBeanUtils.ToString(rateCutOffDays));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightCompoundedRateComputation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(OvernightCompoundedRateComputation), typeof(OvernightIndex));
			  fixingCalendar_Renamed = DirectMetaProperty.ofImmutable(this, "fixingCalendar", typeof(OvernightCompoundedRateComputation), typeof(HolidayCalendar));
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(OvernightCompoundedRateComputation), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(OvernightCompoundedRateComputation), typeof(LocalDate));
			  rateCutOffDays_Renamed = DirectMetaProperty.ofImmutable(this, "rateCutOffDays", typeof(OvernightCompoundedRateComputation), Integer.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "fixingCalendar", "startDate", "endDate", "rateCutOffDays");
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
		/// The meta-property for the {@code rateCutOffDays} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> rateCutOffDays_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "fixingCalendar", "startDate", "endDate", "rateCutOffDays");
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
			case -92095804: // rateCutOffDays
			  return rateCutOffDays_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override OvernightCompoundedRateComputation.Builder builder()
		{
		  return new OvernightCompoundedRateComputation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(OvernightCompoundedRateComputation);
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

		/// <summary>
		/// The meta-property for the {@code rateCutOffDays} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> rateCutOffDays()
		{
		  return rateCutOffDays_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((OvernightCompoundedRateComputation) bean).Index;
			case 394230283: // fixingCalendar
			  return ((OvernightCompoundedRateComputation) bean).FixingCalendar;
			case -2129778896: // startDate
			  return ((OvernightCompoundedRateComputation) bean).StartDate;
			case -1607727319: // endDate
			  return ((OvernightCompoundedRateComputation) bean).EndDate;
			case -92095804: // rateCutOffDays
			  return ((OvernightCompoundedRateComputation) bean).RateCutOffDays;
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
	  /// The bean-builder for {@code OvernightCompoundedRateComputation}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<OvernightCompoundedRateComputation>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal OvernightIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal HolidayCalendar fixingCalendar_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal int rateCutOffDays_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(OvernightCompoundedRateComputation beanToCopy)
		{
		  this.index_Renamed = beanToCopy.Index;
		  this.fixingCalendar_Renamed = beanToCopy.FixingCalendar;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.rateCutOffDays_Renamed = beanToCopy.RateCutOffDays;
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
			case -92095804: // rateCutOffDays
			  return rateCutOffDays_Renamed;
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
			case -92095804: // rateCutOffDays
			  this.rateCutOffDays_Renamed = (int?) newValue.Value;
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

		public override OvernightCompoundedRateComputation build()
		{
		  return new OvernightCompoundedRateComputation(index_Renamed, fixingCalendar_Renamed, startDate_Renamed, endDate_Renamed, rateCutOffDays_Renamed);
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
		/// Sets the fixing date associated with the start date of the accrual period.
		/// <para>
		/// This is also the first fixing date.
		/// The overnight rate is observed from this date onwards.
		/// </para>
		/// <para>
		/// In general, the fixing dates and accrual dates are the same for an overnight index.
		/// However, in the case of a Tomorrow/Next index, the fixing period is one business day
		/// before the accrual period.
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
		/// Sets the fixing date associated with the end date of the accrual period.
		/// <para>
		/// The overnight rate is observed until this date.
		/// </para>
		/// <para>
		/// In general, the fixing dates and accrual dates are the same for an overnight index.
		/// However, in the case of a Tomorrow/Next index, the fixing period is one business day
		/// before the accrual period.
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

		/// <summary>
		/// Sets the number of business days before the end of the period that the rate is cut off.
		/// <para>
		/// When a rate cut-off applies, the final daily rate is determined this number of days
		/// before the end of the period, with any subsequent days having the same rate.
		/// </para>
		/// <para>
		/// The amount must be zero or positive.
		/// A value of zero or one will have no effect on the standard calculation.
		/// The fixing holiday calendar of the index is used to determine business days.
		/// </para>
		/// <para>
		/// For example, a value of {@code 3} means that the rate observed on
		/// {@code (periodEndDate - 3 business days)} is also to be used on
		/// {@code (periodEndDate - 2 business days)} and {@code (periodEndDate - 1 business day)}.
		/// </para>
		/// <para>
		/// If there are multiple accrual periods in the payment period, then this
		/// should typically only be non-zero in the last accrual period.
		/// </para>
		/// </summary>
		/// <param name="rateCutOffDays">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rateCutOffDays(int rateCutOffDays)
		{
		  ArgChecker.notNegative(rateCutOffDays, "rateCutOffDays");
		  this.rateCutOffDays_Renamed = rateCutOffDays;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("OvernightCompoundedRateComputation.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingCalendar").Append('=').Append(JodaBeanUtils.ToString(fixingCalendar_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("rateCutOffDays").Append('=').Append(JodaBeanUtils.ToString(rateCutOffDays_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}