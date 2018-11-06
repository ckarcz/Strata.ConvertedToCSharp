using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A sequence of steps that vary a value over time.
	/// <para>
	/// A financial value, such as the notional or interest rate, may vary over time.
	/// This class represents a sequence of changes in the value within <seealso cref="ValueSchedule"/>.
	/// </para>
	/// <para>
	/// The sequence is defined by a start date, end date and frequency.
	/// The adjustment at each step is defined using <seealso cref="ValueAdjustment"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ValueStepSequence implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ValueStepSequence : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate firstStepDate;
		private readonly LocalDate firstStepDate;
	  /// <summary>
	  /// The last date in the sequence.
	  /// <para>
	  /// This sequence will change the value on this date, but not after.
	  /// This must be one of the unadjusted dates in the schedule period schedule.
	  /// </para>
	  /// <para>
	  /// For example, consider a 5 year swap from 2012-02-01 to 2017-02-01 with 6 month frequency.
	  /// The date '2015-02-01' is an unadjusted schedule period boundary, and so may be specified here.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate lastStepDate;
	  private readonly LocalDate lastStepDate;
	  /// <summary>
	  /// The frequency of the sequence.
	  /// <para>
	  /// This sequence will change the value on each date between the start and end defined by this frequency.
	  /// The frequency is interpreted relative to the frequency of a <seealso cref="Schedule"/>.
	  /// It must be equal or greater than the related schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.Frequency frequency;
	  private readonly Frequency frequency;
	  /// <summary>
	  /// The adjustment representing the change that occurs at each step.
	  /// <para>
	  /// The adjustment type must not be 'Replace'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ValueAdjustment adjustment;
	  private readonly ValueAdjustment adjustment;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the dates, frequency and change.
	  /// </summary>
	  /// <param name="firstStepDate">  the first date of the sequence </param>
	  /// <param name="lastStepDate">  the last date of the sequence </param>
	  /// <param name="frequency">  the frequency of changes </param>
	  /// <param name="adjustment">  the adjustment at each step </param>
	  /// <returns> the varying step </returns>
	  public static ValueStepSequence of(LocalDate firstStepDate, LocalDate lastStepDate, Frequency frequency, ValueAdjustment adjustment)
	  {

		return new ValueStepSequence(firstStepDate, lastStepDate, frequency, adjustment);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderOrEqual(firstStepDate, lastStepDate, "firstStepDate", "lastStepDate");
		ArgChecker.isTrue(adjustment.Type != ValueAdjustmentType.REPLACE, "ValueAdjustmentType must not be 'Replace'");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Resolves the sequence to a list of steps.
	  /// </summary>
	  /// <param name="existingSteps">  the existing list of steps </param>
	  /// <param name="rollConv">  the roll convention </param>
	  /// <returns> the steps </returns>
	  internal IList<ValueStep> resolve(IList<ValueStep> existingSteps, RollConvention rollConv)
	  {
		ImmutableList.Builder<ValueStep> steps = ImmutableList.builder();
		steps.addAll(existingSteps);
		LocalDate prev = firstStepDate;
		LocalDate date = firstStepDate;
		while (!date.isAfter(lastStepDate))
		{
		  steps.add(ValueStep.of(date, adjustment));
		  prev = date;
		  date = rollConv.next(date, frequency);
		}
		if (!prev.Equals(lastStepDate))
		{
		  throw new System.ArgumentException(Messages.format("ValueStepSequence lastStepDate did not match frequency '{}' using roll convention '{}', {} != {}", frequency, rollConv, lastStepDate, prev));
		}
		return steps.build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ValueStepSequence}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ValueStepSequence.Meta meta()
	  {
		return ValueStepSequence.Meta.INSTANCE;
	  }

	  static ValueStepSequence()
	  {
		MetaBean.register(ValueStepSequence.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ValueStepSequence(LocalDate firstStepDate, LocalDate lastStepDate, Frequency frequency, ValueAdjustment adjustment)
	  {
		JodaBeanUtils.notNull(firstStepDate, "firstStepDate");
		JodaBeanUtils.notNull(lastStepDate, "lastStepDate");
		JodaBeanUtils.notNull(frequency, "frequency");
		JodaBeanUtils.notNull(adjustment, "adjustment");
		this.firstStepDate = firstStepDate;
		this.lastStepDate = lastStepDate;
		this.frequency = frequency;
		this.adjustment = adjustment;
		validate();
	  }

	  public override ValueStepSequence.Meta metaBean()
	  {
		return ValueStepSequence.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the first date in the sequence.
	  /// <para>
	  /// This sequence will change the value on this date, but not before.
	  /// This must be one of the unadjusted dates in the schedule period schedule.
	  /// </para>
	  /// <para>
	  /// For example, consider a 5 year swap from 2012-02-01 to 2017-02-01 with 6 month frequency.
	  /// The date '2013-02-01' is an unadjusted schedule period boundary, and so may be specified here.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate FirstStepDate
	  {
		  get
		  {
			return firstStepDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the last date in the sequence.
	  /// <para>
	  /// This sequence will change the value on this date, but not after.
	  /// This must be one of the unadjusted dates in the schedule period schedule.
	  /// </para>
	  /// <para>
	  /// For example, consider a 5 year swap from 2012-02-01 to 2017-02-01 with 6 month frequency.
	  /// The date '2015-02-01' is an unadjusted schedule period boundary, and so may be specified here.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate LastStepDate
	  {
		  get
		  {
			return lastStepDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the frequency of the sequence.
	  /// <para>
	  /// This sequence will change the value on each date between the start and end defined by this frequency.
	  /// The frequency is interpreted relative to the frequency of a <seealso cref="Schedule"/>.
	  /// It must be equal or greater than the related schedule.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Frequency Frequency
	  {
		  get
		  {
			return frequency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the adjustment representing the change that occurs at each step.
	  /// <para>
	  /// The adjustment type must not be 'Replace'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueAdjustment Adjustment
	  {
		  get
		  {
			return adjustment;
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
		  ValueStepSequence other = (ValueStepSequence) obj;
		  return JodaBeanUtils.equal(firstStepDate, other.firstStepDate) && JodaBeanUtils.equal(lastStepDate, other.lastStepDate) && JodaBeanUtils.equal(frequency, other.frequency) && JodaBeanUtils.equal(adjustment, other.adjustment);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(firstStepDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastStepDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(frequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(adjustment);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("ValueStepSequence{");
		buf.Append("firstStepDate").Append('=').Append(firstStepDate).Append(',').Append(' ');
		buf.Append("lastStepDate").Append('=').Append(lastStepDate).Append(',').Append(' ');
		buf.Append("frequency").Append('=').Append(frequency).Append(',').Append(' ');
		buf.Append("adjustment").Append('=').Append(JodaBeanUtils.ToString(adjustment));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ValueStepSequence}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  firstStepDate_Renamed = DirectMetaProperty.ofImmutable(this, "firstStepDate", typeof(ValueStepSequence), typeof(LocalDate));
			  lastStepDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastStepDate", typeof(ValueStepSequence), typeof(LocalDate));
			  frequency_Renamed = DirectMetaProperty.ofImmutable(this, "frequency", typeof(ValueStepSequence), typeof(Frequency));
			  adjustment_Renamed = DirectMetaProperty.ofImmutable(this, "adjustment", typeof(ValueStepSequence), typeof(ValueAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "firstStepDate", "lastStepDate", "frequency", "adjustment");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code firstStepDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> firstStepDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code lastStepDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> lastStepDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code frequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> frequency_Renamed;
		/// <summary>
		/// The meta-property for the {@code adjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueAdjustment> adjustment_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "firstStepDate", "lastStepDate", "frequency", "adjustment");
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
			case -1025397910: // firstStepDate
			  return firstStepDate_Renamed;
			case -292412080: // lastStepDate
			  return lastStepDate_Renamed;
			case -70023844: // frequency
			  return frequency_Renamed;
			case 1977085293: // adjustment
			  return adjustment_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ValueStepSequence> builder()
		public override BeanBuilder<ValueStepSequence> builder()
		{
		  return new ValueStepSequence.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ValueStepSequence);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code firstStepDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> firstStepDate()
		{
		  return firstStepDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lastStepDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> lastStepDate()
		{
		  return lastStepDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code frequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> frequency()
		{
		  return frequency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code adjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueAdjustment> adjustment()
		{
		  return adjustment_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1025397910: // firstStepDate
			  return ((ValueStepSequence) bean).FirstStepDate;
			case -292412080: // lastStepDate
			  return ((ValueStepSequence) bean).LastStepDate;
			case -70023844: // frequency
			  return ((ValueStepSequence) bean).Frequency;
			case 1977085293: // adjustment
			  return ((ValueStepSequence) bean).Adjustment;
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
	  /// The bean-builder for {@code ValueStepSequence}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ValueStepSequence>
	  {

		internal LocalDate firstStepDate;
		internal LocalDate lastStepDate;
		internal Frequency frequency;
		internal ValueAdjustment adjustment;

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
			case -1025397910: // firstStepDate
			  return firstStepDate;
			case -292412080: // lastStepDate
			  return lastStepDate;
			case -70023844: // frequency
			  return frequency;
			case 1977085293: // adjustment
			  return adjustment;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1025397910: // firstStepDate
			  this.firstStepDate = (LocalDate) newValue;
			  break;
			case -292412080: // lastStepDate
			  this.lastStepDate = (LocalDate) newValue;
			  break;
			case -70023844: // frequency
			  this.frequency = (Frequency) newValue;
			  break;
			case 1977085293: // adjustment
			  this.adjustment = (ValueAdjustment) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ValueStepSequence build()
		{
		  return new ValueStepSequence(firstStepDate, lastStepDate, frequency, adjustment);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ValueStepSequence.Builder{");
		  buf.Append("firstStepDate").Append('=').Append(JodaBeanUtils.ToString(firstStepDate)).Append(',').Append(' ');
		  buf.Append("lastStepDate").Append('=').Append(JodaBeanUtils.ToString(lastStepDate)).Append(',').Append(' ');
		  buf.Append("frequency").Append('=').Append(JodaBeanUtils.ToString(frequency)).Append(',').Append(' ');
		  buf.Append("adjustment").Append('=').Append(JodaBeanUtils.ToString(adjustment));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}