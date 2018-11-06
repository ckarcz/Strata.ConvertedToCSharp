using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
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

	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;

	/// <summary>
	/// A single step in the variation of a value over time.
	/// <para>
	/// A financial value, such as the notional or interest rate, may vary over time.
	/// This class represents a single change in the value within <seealso cref="ValueSchedule"/>.
	/// </para>
	/// <para>
	/// The date of the change is either specified explicitly, or in relative terms via an index.
	/// The adjustment to the value can also be specified absolutely, or in relative terms.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ValueStep implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ValueStep : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<int> periodIndex;
		private readonly int? periodIndex;
	  /// <summary>
	  /// The date of the schedule period boundary at which the change occurs.
	  /// <para>
	  /// This property is used to define the date that the step occurs in absolute terms.
	  /// This must be one of the unadjusted dates in the schedule period schedule.
	  /// This is an unadjusted date and calculation period business day adjustments will apply.
	  /// </para>
	  /// <para>
	  /// For example, consider a 5 year swap from 2012-02-01 to 2017-02-01 with 6 month frequency.
	  /// The date '2013-02-01' is an unadjusted schedule period boundary, and so may be specified here.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.LocalDate date;
	  private readonly LocalDate date;
	  /// <summary>
	  /// The value representing the change that occurs.
	  /// <para>
	  /// The adjustment can be an absolute value, or various kinds of relative values.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ValueAdjustment value;
	  private readonly ValueAdjustment value;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that applies at the specified schedule period index.
	  /// <para>
	  /// This factory is used to define the date that the step occurs in relative terms.
	  /// The date is identified by specifying the zero-based index of the schedule period boundary.
	  /// The change will occur at the start of the specified period.
	  /// Thus an index of zero is the start of the first period or initial stub.
	  /// The index must be one or greater, as a change is not permitted at the start of the first period.
	  /// </para>
	  /// <para>
	  /// For example, consider a 5 year swap from 2012-02-01 to 2017-02-01 with 6 month frequency.
	  /// A zero-based index of '2' would refer to start of the 3rd period, which would be 2013-02-01.
	  /// </para>
	  /// <para>
	  /// The value may be absolute or relative, as per <seealso cref="ValueAdjustment"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="periodIndex">  the index of the period of the value change </param>
	  /// <param name="value">  the adjustment to make to the value </param>
	  /// <returns> the varying step </returns>
	  public static ValueStep of(int periodIndex, ValueAdjustment value)
	  {
		return new ValueStep(periodIndex, null, value);
	  }

	  /// <summary>
	  /// Obtains an instance that applies at the specified date.
	  /// <para>
	  /// This factory obtains a step that causes the value to change at the specified date.
	  /// </para>
	  /// <para>
	  /// The value may be absolute or relative, as per <seealso cref="ValueAdjustment"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the start date of the value change </param>
	  /// <param name="value">  the adjustment to make to the value </param>
	  /// <returns> the varying step </returns>
	  public static ValueStep of(LocalDate date, ValueAdjustment value)
	  {
		return new ValueStep(null, date, value);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the index of this value step in the schedule.
	  /// </summary>
	  /// <param name="periods">  the list of schedule periods </param>
	  /// <returns> the index of the schedule period </returns>
	  internal int findIndex(IList<SchedulePeriod> periods)
	  {
		// either periodIndex or date is non-null, not both
		if (periodIndex != null)
		{
		  // index based
		  if (periodIndex >= periods.Count)
		  {
			throw new System.ArgumentException("ValueStep index is beyond last schedule period");
		  }
		  return periodIndex.Value;
		}
		else
		{
		  // date based, match one of the unadjusted period boundaries
		  for (int i = 0; i < periods.Count; i++)
		  {
			SchedulePeriod period = periods[i];
			if (period.UnadjustedStartDate.Equals(date))
			{
			  return i;
			}
		  }
		  // try adjusted boundaries instead of unadjusted ones
		  for (int i = 0; i < periods.Count; i++)
		  {
			SchedulePeriod period = periods[i];
			if (period.StartDate.Equals(date))
			{
			  return i;
			}
		  }
		  return -1;
		}
	  }

	  /// <summary>
	  /// Finds the index of the previous value step in the schedule.
	  /// <para>
	  /// This will only be called on a date-based step.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="periods">  the list of schedule periods, at least size 1, only date-based </param>
	  /// <returns> the index of the schedule period </returns>
	  internal int findPreviousIndex(IList<SchedulePeriod> periods)
	  {
		SchedulePeriod firstPeriod = periods[0];
		if (date.isBefore(firstPeriod.UnadjustedStartDate))
		{
		  throw new System.ArgumentException("ValueStep date is before the start of the schedule: " + date + " < " + firstPeriod.UnadjustedStartDate);
		}
		for (int i = 1; i < periods.Count; i++)
		{
		  SchedulePeriod period = periods[i];
		  if (period.UnadjustedStartDate.isAfter(date))
		  {
			return i - 1;
		  }
		}
		SchedulePeriod lastPeriod = periods[periods.Count - 1];
		if (date.isAfter(lastPeriod.UnadjustedEndDate))
		{
		  throw new System.ArgumentException("ValueStep date is after the end of the schedule: " + date + " > " + lastPeriod.UnadjustedEndDate);
		}
		return periods.Count - 1;
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (periodIndex == null && date == null)
		{
		  throw new System.ArgumentException("Either the 'periodIndex' or 'date' must be set");
		}
		if (periodIndex != null)
		{
		  if (date != null)
		  {
			throw new System.ArgumentException("Either the 'periodIndex' or 'date' must be set, not both");
		  }
		  if (periodIndex < 1)
		  {
			throw new System.ArgumentException("The 'periodIndex' must not be zero or negative");
		  }
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ValueStep}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ValueStep.Meta meta()
	  {
		return ValueStep.Meta.INSTANCE;
	  }

	  static ValueStep()
	  {
		MetaBean.register(ValueStep.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ValueStep.Builder builder()
	  {
		return new ValueStep.Builder();
	  }

	  private ValueStep(int? periodIndex, LocalDate date, ValueAdjustment value)
	  {
		JodaBeanUtils.notNull(value, "value");
		this.periodIndex = periodIndex;
		this.date = date;
		this.value = value;
		validate();
	  }

	  public override ValueStep.Meta metaBean()
	  {
		return ValueStep.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index of the schedule period boundary at which the change occurs.
	  /// <para>
	  /// This property is used to define the date that the step occurs in relative terms.
	  /// The date is identified by specifying the zero-based index of the schedule period boundary.
	  /// The change will occur at the start of the specified period.
	  /// Thus an index of zero is the start of the first period or initial stub.
	  /// The index must be one or greater, as a change is not permitted at the start of the first period.
	  /// </para>
	  /// <para>
	  /// For example, consider a 5 year swap from 2012-02-01 to 2017-02-01 with 6 month frequency.
	  /// A zero-based index of '2' would refer to start of the 3rd period, which would be 2013-02-01.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public int? PeriodIndex
	  {
		  get
		  {
			return periodIndex != null ? int?.of(periodIndex) : int?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date of the schedule period boundary at which the change occurs.
	  /// <para>
	  /// This property is used to define the date that the step occurs in absolute terms.
	  /// This must be one of the unadjusted dates in the schedule period schedule.
	  /// This is an unadjusted date and calculation period business day adjustments will apply.
	  /// </para>
	  /// <para>
	  /// For example, consider a 5 year swap from 2012-02-01 to 2017-02-01 with 6 month frequency.
	  /// The date '2013-02-01' is an unadjusted schedule period boundary, and so may be specified here.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<LocalDate> Date
	  {
		  get
		  {
			return Optional.ofNullable(date);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the value representing the change that occurs.
	  /// <para>
	  /// The adjustment can be an absolute value, or various kinds of relative values.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueAdjustment Value
	  {
		  get
		  {
			return value;
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
		  ValueStep other = (ValueStep) obj;
		  return JodaBeanUtils.equal(periodIndex, other.periodIndex) && JodaBeanUtils.equal(date, other.date) && JodaBeanUtils.equal(value, other.value);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periodIndex);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(date);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(value);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("ValueStep{");
		buf.Append("periodIndex").Append('=').Append(periodIndex).Append(',').Append(' ');
		buf.Append("date").Append('=').Append(date).Append(',').Append(' ');
		buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ValueStep}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  periodIndex_Renamed = DirectMetaProperty.ofImmutable(this, "periodIndex", typeof(ValueStep), typeof(Integer));
			  date_Renamed = DirectMetaProperty.ofImmutable(this, "date", typeof(ValueStep), typeof(LocalDate));
			  value_Renamed = DirectMetaProperty.ofImmutable(this, "value", typeof(ValueStep), typeof(ValueAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "periodIndex", "date", "value");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code periodIndex} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> periodIndex_Renamed;
		/// <summary>
		/// The meta-property for the {@code date} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> date_Renamed;
		/// <summary>
		/// The meta-property for the {@code value} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueAdjustment> value_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "periodIndex", "date", "value");
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
			case -980601967: // periodIndex
			  return periodIndex_Renamed;
			case 3076014: // date
			  return date_Renamed;
			case 111972721: // value
			  return value_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ValueStep.Builder builder()
		{
		  return new ValueStep.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ValueStep);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code periodIndex} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> periodIndex()
		{
		  return periodIndex_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code date} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> date()
		{
		  return date_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code value} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueAdjustment> value()
		{
		  return value_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -980601967: // periodIndex
			  return ((ValueStep) bean).periodIndex;
			case 3076014: // date
			  return ((ValueStep) bean).date;
			case 111972721: // value
			  return ((ValueStep) bean).Value;
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
	  /// The bean-builder for {@code ValueStep}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ValueStep>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal int? periodIndex_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate date_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueAdjustment value_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ValueStep beanToCopy)
		{
		  this.periodIndex_Renamed = beanToCopy.periodIndex;
		  this.date_Renamed = beanToCopy.date;
		  this.value_Renamed = beanToCopy.Value;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -980601967: // periodIndex
			  return periodIndex_Renamed;
			case 3076014: // date
			  return date_Renamed;
			case 111972721: // value
			  return value_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -980601967: // periodIndex
			  this.periodIndex_Renamed = (int?) newValue;
			  break;
			case 3076014: // date
			  this.date_Renamed = (LocalDate) newValue;
			  break;
			case 111972721: // value
			  this.value_Renamed = (ValueAdjustment) newValue;
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

		public override ValueStep build()
		{
		  return new ValueStep(periodIndex_Renamed, date_Renamed, value_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the index of the schedule period boundary at which the change occurs.
		/// <para>
		/// This property is used to define the date that the step occurs in relative terms.
		/// The date is identified by specifying the zero-based index of the schedule period boundary.
		/// The change will occur at the start of the specified period.
		/// Thus an index of zero is the start of the first period or initial stub.
		/// The index must be one or greater, as a change is not permitted at the start of the first period.
		/// </para>
		/// <para>
		/// For example, consider a 5 year swap from 2012-02-01 to 2017-02-01 with 6 month frequency.
		/// A zero-based index of '2' would refer to start of the 3rd period, which would be 2013-02-01.
		/// </para>
		/// </summary>
		/// <param name="periodIndex">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periodIndex(int? periodIndex)
		{
		  this.periodIndex_Renamed = periodIndex;
		  return this;
		}

		/// <summary>
		/// Sets the date of the schedule period boundary at which the change occurs.
		/// <para>
		/// This property is used to define the date that the step occurs in absolute terms.
		/// This must be one of the unadjusted dates in the schedule period schedule.
		/// This is an unadjusted date and calculation period business day adjustments will apply.
		/// </para>
		/// <para>
		/// For example, consider a 5 year swap from 2012-02-01 to 2017-02-01 with 6 month frequency.
		/// The date '2013-02-01' is an unadjusted schedule period boundary, and so may be specified here.
		/// </para>
		/// </summary>
		/// <param name="date">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder date(LocalDate date)
		{
		  this.date_Renamed = date;
		  return this;
		}

		/// <summary>
		/// Sets the value representing the change that occurs.
		/// <para>
		/// The adjustment can be an absolute value, or various kinds of relative values.
		/// </para>
		/// </summary>
		/// <param name="value">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder value(ValueAdjustment value)
		{
		  JodaBeanUtils.notNull(value, "value");
		  this.value_Renamed = value;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("ValueStep.Builder{");
		  buf.Append("periodIndex").Append('=').Append(JodaBeanUtils.ToString(periodIndex_Renamed)).Append(',').Append(' ');
		  buf.Append("date").Append('=').Append(JodaBeanUtils.ToString(date_Renamed)).Append(',').Append(' ');
		  buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}