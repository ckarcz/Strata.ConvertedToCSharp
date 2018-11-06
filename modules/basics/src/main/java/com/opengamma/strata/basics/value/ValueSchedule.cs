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
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// A value that can vary over time.
	/// <para>
	/// This represents a single initial value and any adjustments over the lifetime of a trade.
	/// Adjustments may be specified in absolute or relative terms.
	/// </para>
	/// <para>
	/// The adjustments may be specified as individual steps or as a sequence of steps.
	/// An individual step is a change that occurs at a specific date, identified either by
	/// the date or the index within the schedule. A sequence of steps consists of a start date,
	/// end date and frequency, with the same change applying many times.
	/// All changes must occur on dates that are period boundaries in the specified schedule.
	/// </para>
	/// <para>
	/// It is possible to specify both individual steps and a sequence, however this is not recommended.
	/// It it is done, then the individual steps and sequence steps must resolve to different dates.
	/// </para>
	/// <para>
	/// The value is specified as a {@code double} with the context adding additional meaning.
	/// If the value represents an amount of money then the currency is specified separately.
	/// If the value represents a rate then a 5% rate is expressed as 0.05.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ValueSchedule implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ValueSchedule : ImmutableBean
	{

	  /// <summary>
	  /// A value schedule that always has the value zero.
	  /// </summary>
	  public static readonly ValueSchedule ALWAYS_0 = ValueSchedule.of(0);
	  /// <summary>
	  /// A value schedule that always has the value one.
	  /// </summary>
	  public static readonly ValueSchedule ALWAYS_1 = ValueSchedule.of(1);

	  /// <summary>
	  /// The initial value.
	  /// <para>
	  /// This is used for the lifetime of the trade unless specifically varied.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double initialValue;
	  private readonly double initialValue;
	  /// <summary>
	  /// The steps defining the change in the value.
	  /// <para>
	  /// Each step consists of a key locating the date of the change and the adjustment that occurs.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.util.List<ValueStep> steps;
	  private readonly IList<ValueStep> steps;
	  /// <summary>
	  /// The sequence of steps changing the value.
	  /// <para>
	  /// This allows a regular pattern of steps to be encoded.
	  /// All step dates must be unique, thus the list of steps must not contain any date implied by this sequence.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final ValueStepSequence stepSequence;
	  private readonly ValueStepSequence stepSequence;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a single value that does not change over time.
	  /// </summary>
	  /// <param name="value">  a single value that does not change over time </param>
	  /// <returns> the value schedule </returns>
	  public static ValueSchedule of(double value)
	  {
		return new ValueSchedule(value, ImmutableList.of(), null);
	  }

	  /// <summary>
	  /// Obtains an instance from an initial value and a list of changes.
	  /// <para>
	  /// Each step fully defines a single change in the value.
	  /// The date of each change can be specified as an absolute date or in relative terms.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="initialValue">  the initial value used for the first period </param>
	  /// <param name="steps">  the full definition of how the value changes over time </param>
	  /// <returns> the value schedule </returns>
	  public static ValueSchedule of(double initialValue, params ValueStep[] steps)
	  {
		return new ValueSchedule(initialValue, ImmutableList.copyOf(steps), null);
	  }

	  /// <summary>
	  /// Obtains an instance from an initial value and a list of changes.
	  /// <para>
	  /// Each step fully defines a single change in the value.
	  /// The date of each change can be specified as an absolute date or in relative terms.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="initialValue">  the initial value used for the first period </param>
	  /// <param name="steps">  the full definition of how the value changes over time </param>
	  /// <returns> the value schedule </returns>
	  public static ValueSchedule of(double initialValue, IList<ValueStep> steps)
	  {
		return new ValueSchedule(initialValue, ImmutableList.copyOf(steps), null);
	  }

	  /// <summary>
	  /// Obtains an instance from an initial value and a sequence of steps.
	  /// <para>
	  /// The sequence defines changes from one date to another date using a frequency.
	  /// For example, the value might change every year from 2011-06-01 to 2015-06-01.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="initialValue">  the initial value used for the first period </param>
	  /// <param name="stepSequence">  the full definition of how the value changes over time </param>
	  /// <returns> the value schedule </returns>
	  public static ValueSchedule of(double initialValue, ValueStepSequence stepSequence)
	  {
		return new ValueSchedule(initialValue, ImmutableList.of(), stepSequence);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Resolves the value and adjustments against a specific schedule.
	  /// <para>
	  /// This converts a schedule into a list of values, one for each schedule period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="schedule">  the schedule </param>
	  /// <returns> the values, one for each schedule period </returns>
	  public DoubleArray resolveValues(Schedule schedule)
	  {
		return resolveValues(schedule.Periods, schedule.RollConvention);
	  }

	  // resolve the values
	  private DoubleArray resolveValues(IList<SchedulePeriod> periods, RollConvention rollConv)
	  {
		// handle simple case where there are no steps
		if (steps.Count == 0 && stepSequence == null)
		{
		  return DoubleArray.filled(periods.Count, initialValue);
		}
		return resolveSteps(periods, rollConv);
	  }

	  // resolve the steps, broken into a separate method to aid inlining
	  private DoubleArray resolveSteps(IList<SchedulePeriod> periods, RollConvention rollConv)
	  {
		int size = periods.Count;
		double[] result = new double[size];
		IList<ValueStep> resolvedSteps = StepSequence.map(seq => seq.resolve(steps, rollConv)).orElse(steps);
		// expand ValueStep to array of adjustments matching the periods
		// the steps are not sorted, so use fixed size array to absorb incoming data
		ValueAdjustment[] expandedSteps = new ValueAdjustment[size];
		IList<ValueStep> invalidSteps = new List<ValueStep>();
		foreach (ValueStep step in resolvedSteps)
		{
		  int index = step.findIndex(periods);
		  if (index < 0)
		  {
			invalidSteps.Add(step);
			continue;
		  }
		  if (expandedSteps[index] != null && !expandedSteps[index].Equals(step.Value))
		  {
			throw new System.ArgumentException(Messages.format("Invalid ValueSchedule, two steps resolved to the same schedule period starting on {}, schedule defined as {}", periods[index].UnadjustedStartDate, this));
		  }
		  expandedSteps[index] = step.Value;
		}
		// apply each adjustment
		double value = initialValue;
		for (int i = 0; i < size; i++)
		{
		  if (expandedSteps[i] != null)
		  {
			value = expandedSteps[i].adjust(value);
		  }
		  result[i] = value;
		}
		// ensure that invalid steps cause no changes
		foreach (ValueStep step in invalidSteps)
		{
		  double baseValue = result[step.findPreviousIndex(periods)];
		  double adjusted = step.Value.adjust(baseValue);
		  if (adjusted != baseValue)
		  {
			throw new System.ArgumentException("ValueStep date does not match a period boundary: " + step.Date.get());
		  }
		}
		// return result
		return DoubleArray.ofUnsafe(result);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ValueSchedule}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ValueSchedule.Meta meta()
	  {
		return ValueSchedule.Meta.INSTANCE;
	  }

	  static ValueSchedule()
	  {
		MetaBean.register(ValueSchedule.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ValueSchedule.Builder builder()
	  {
		return new ValueSchedule.Builder();
	  }

	  private ValueSchedule(double initialValue, IList<ValueStep> steps, ValueStepSequence stepSequence)
	  {
		JodaBeanUtils.notNull(steps, "steps");
		this.initialValue = initialValue;
		this.steps = ImmutableList.copyOf(steps);
		this.stepSequence = stepSequence;
	  }

	  public override ValueSchedule.Meta metaBean()
	  {
		return ValueSchedule.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the initial value.
	  /// <para>
	  /// This is used for the lifetime of the trade unless specifically varied.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double InitialValue
	  {
		  get
		  {
			return initialValue;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the steps defining the change in the value.
	  /// <para>
	  /// Each step consists of a key locating the date of the change and the adjustment that occurs.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IList<ValueStep> Steps
	  {
		  get
		  {
			return steps;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the sequence of steps changing the value.
	  /// <para>
	  /// This allows a regular pattern of steps to be encoded.
	  /// All step dates must be unique, thus the list of steps must not contain any date implied by this sequence.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ValueStepSequence> StepSequence
	  {
		  get
		  {
			return Optional.ofNullable(stepSequence);
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
		  ValueSchedule other = (ValueSchedule) obj;
		  return JodaBeanUtils.equal(initialValue, other.initialValue) && JodaBeanUtils.equal(steps, other.steps) && JodaBeanUtils.equal(stepSequence, other.stepSequence);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(initialValue);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(steps);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(stepSequence);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("ValueSchedule{");
		buf.Append("initialValue").Append('=').Append(initialValue).Append(',').Append(' ');
		buf.Append("steps").Append('=').Append(steps).Append(',').Append(' ');
		buf.Append("stepSequence").Append('=').Append(JodaBeanUtils.ToString(stepSequence));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ValueSchedule}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  initialValue_Renamed = DirectMetaProperty.ofImmutable(this, "initialValue", typeof(ValueSchedule), Double.TYPE);
			  steps_Renamed = DirectMetaProperty.ofImmutable(this, "steps", typeof(ValueSchedule), (Type) typeof(System.Collections.IList));
			  stepSequence_Renamed = DirectMetaProperty.ofImmutable(this, "stepSequence", typeof(ValueSchedule), typeof(ValueStepSequence));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "initialValue", "steps", "stepSequence");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code initialValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> initialValue_Renamed;
		/// <summary>
		/// The meta-property for the {@code steps} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<java.util.List<ValueStep>> steps = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "steps", ValueSchedule.class, (Class) java.util.List.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IList<ValueStep>> steps_Renamed;
		/// <summary>
		/// The meta-property for the {@code stepSequence} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueStepSequence> stepSequence_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "initialValue", "steps", "stepSequence");
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
			case -418368371: // initialValue
			  return initialValue_Renamed;
			case 109761319: // steps
			  return steps_Renamed;
			case 2141410989: // stepSequence
			  return stepSequence_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ValueSchedule.Builder builder()
		{
		  return new ValueSchedule.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ValueSchedule);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code initialValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> initialValue()
		{
		  return initialValue_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code steps} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IList<ValueStep>> steps()
		{
		  return steps_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code stepSequence} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueStepSequence> stepSequence()
		{
		  return stepSequence_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -418368371: // initialValue
			  return ((ValueSchedule) bean).InitialValue;
			case 109761319: // steps
			  return ((ValueSchedule) bean).Steps;
			case 2141410989: // stepSequence
			  return ((ValueSchedule) bean).stepSequence;
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
	  /// The bean-builder for {@code ValueSchedule}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ValueSchedule>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double initialValue_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<ValueStep> steps_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueStepSequence stepSequence_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ValueSchedule beanToCopy)
		{
		  this.initialValue_Renamed = beanToCopy.InitialValue;
		  this.steps_Renamed = ImmutableList.copyOf(beanToCopy.Steps);
		  this.stepSequence_Renamed = beanToCopy.stepSequence;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -418368371: // initialValue
			  return initialValue_Renamed;
			case 109761319: // steps
			  return steps_Renamed;
			case 2141410989: // stepSequence
			  return stepSequence_Renamed;
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
			case -418368371: // initialValue
			  this.initialValue_Renamed = (double?) newValue.Value;
			  break;
			case 109761319: // steps
			  this.steps_Renamed = (IList<ValueStep>) newValue;
			  break;
			case 2141410989: // stepSequence
			  this.stepSequence_Renamed = (ValueStepSequence) newValue;
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

		public override ValueSchedule build()
		{
		  return new ValueSchedule(initialValue_Renamed, steps_Renamed, stepSequence_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the initial value.
		/// <para>
		/// This is used for the lifetime of the trade unless specifically varied.
		/// </para>
		/// </summary>
		/// <param name="initialValue">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder initialValue(double initialValue)
		{
		  this.initialValue_Renamed = initialValue;
		  return this;
		}

		/// <summary>
		/// Sets the steps defining the change in the value.
		/// <para>
		/// Each step consists of a key locating the date of the change and the adjustment that occurs.
		/// </para>
		/// </summary>
		/// <param name="steps">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder steps(IList<ValueStep> steps)
		{
		  JodaBeanUtils.notNull(steps, "steps");
		  this.steps_Renamed = steps;
		  return this;
		}

		/// <summary>
		/// Sets the {@code steps} property in the builder
		/// from an array of objects. </summary>
		/// <param name="steps">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder steps(params ValueStep[] steps)
		{
		  return this.steps(ImmutableList.copyOf(steps));
		}

		/// <summary>
		/// Sets the sequence of steps changing the value.
		/// <para>
		/// This allows a regular pattern of steps to be encoded.
		/// All step dates must be unique, thus the list of steps must not contain any date implied by this sequence.
		/// </para>
		/// </summary>
		/// <param name="stepSequence">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder stepSequence(ValueStepSequence stepSequence)
		{
		  this.stepSequence_Renamed = stepSequence;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("ValueSchedule.Builder{");
		  buf.Append("initialValue").Append('=').Append(JodaBeanUtils.ToString(initialValue_Renamed)).Append(',').Append(' ');
		  buf.Append("steps").Append('=').Append(JodaBeanUtils.ToString(steps_Renamed)).Append(',').Append(' ');
		  buf.Append("stepSequence").Append('=').Append(JodaBeanUtils.ToString(stepSequence_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}