using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.google.common.@base.MoreObjects.firstNonNull;


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
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Index = com.opengamma.strata.basics.index.Index;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Defines the calculation of a fixed rate swap leg.
	/// <para>
	/// This defines the data necessary to calculate the amount payable on the leg.
	/// The amount is based on a fixed rate, which can vary over the lifetime of the leg.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FixedRateCalculation implements RateCalculation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FixedRateCalculation : RateCalculation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DayCount dayCount;
		private readonly DayCount dayCount;
	  /// <summary>
	  /// The interest rate to be paid.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// This defines the rate as an initial amount and a list of adjustments.
	  /// The rate is only permitted to change at accrual period boundaries.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.value.ValueSchedule rate;
	  private readonly ValueSchedule rate;
	  /// <summary>
	  /// The initial stub, optional.
	  /// <para>
	  /// The initial stub of a swap may have a different rate from the regular accrual periods.
	  /// This property allows the stub rate to be specified, either as a known amount or a rate.
	  /// If this property is not present, then the rate derived from the {@code rate} property applies during the stub.
	  /// If this property is present and there is no initial stub, it is ignored.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final FixedRateStubCalculation initialStub;
	  private readonly FixedRateStubCalculation initialStub;
	  /// <summary>
	  /// The final stub, optional.
	  /// <para>
	  /// The final stub of a swap may have a different rate from the regular accrual periods.
	  /// This property allows the stub rate to be specified, either as a known amount or a rate.
	  /// If this property is not present, then the rate derived from the {@code rate} property applies during the stub.
	  /// If this property is present and there is no initial stub, it is ignored.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final FixedRateStubCalculation finalStub;
	  private readonly FixedRateStubCalculation finalStub;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a rate calculation for the specified day count and rate.
	  /// <para>
	  /// The rate specified here does not vary during the life of the swap.
	  /// If this method provides insufficient control, use the <seealso cref="#builder() builder"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rate">  the rate </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the calculation </returns>
	  public static FixedRateCalculation of(double rate, DayCount dayCount)
	  {
		return FixedRateCalculation.builder().dayCount(dayCount).rate(ValueSchedule.of(rate)).build();
	  }

	  //-------------------------------------------------------------------------
	  public SwapLegType Type
	  {
		  get
		  {
			return SwapLegType.FIXED;
		  }
	  }

	  public void collectCurrencies(ImmutableSet.Builder<Currency> builder)
	  {
		// no currencies
	  }

	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		// no indices
	  }

	  public ImmutableList<RateAccrualPeriod> createAccrualPeriods(Schedule accrualSchedule, Schedule paymentSchedule, ReferenceData refData)
	  {

		// avoid null stub definitions if there are stubs
		FixedRateStubCalculation initialStub = firstNonNull(this.initialStub, FixedRateStubCalculation.NONE);
		FixedRateStubCalculation finalStub = firstNonNull(this.finalStub, FixedRateStubCalculation.NONE);
		Optional<SchedulePeriod> scheduleInitialStub = accrualSchedule.InitialStub;
		Optional<SchedulePeriod> scheduleFinalStub = accrualSchedule.FinalStub;
		// resolve data by schedule
		DoubleArray resolvedRates = rate.resolveValues(accrualSchedule);
		// build accrual periods
		ImmutableList.Builder<RateAccrualPeriod> accrualPeriods = ImmutableList.builder();
		for (int i = 0; i < accrualSchedule.size(); i++)
		{
		  SchedulePeriod period = accrualSchedule.getPeriod(i);
		  double yearFraction = period.yearFraction(dayCount, accrualSchedule);
		  // handle stubs
		  RateComputation rateComputation;
		  if (scheduleInitialStub.Present && scheduleInitialStub.get() == period)
		  {
			rateComputation = initialStub.createRateComputation(resolvedRates.get(i));
		  }
		  else if (scheduleFinalStub.Present && scheduleFinalStub.get() == period)
		  {
			rateComputation = finalStub.createRateComputation(resolvedRates.get(i));
		  }
		  else
		  {
			rateComputation = FixedRateComputation.of(resolvedRates.get(i));
		  }
		  accrualPeriods.add(new RateAccrualPeriod(period, yearFraction, rateComputation));
		}
		return accrualPeriods.build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedRateCalculation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FixedRateCalculation.Meta meta()
	  {
		return FixedRateCalculation.Meta.INSTANCE;
	  }

	  static FixedRateCalculation()
	  {
		MetaBean.register(FixedRateCalculation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FixedRateCalculation.Builder builder()
	  {
		return new FixedRateCalculation.Builder();
	  }

	  private FixedRateCalculation(DayCount dayCount, ValueSchedule rate, FixedRateStubCalculation initialStub, FixedRateStubCalculation finalStub)
	  {
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(rate, "rate");
		this.dayCount = dayCount;
		this.rate = rate;
		this.initialStub = initialStub;
		this.finalStub = finalStub;
	  }

	  public override FixedRateCalculation.Meta metaBean()
	  {
		return FixedRateCalculation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention.
	  /// <para>
	  /// This is used to convert schedule period dates to a numerical value.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the interest rate to be paid.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// This defines the rate as an initial amount and a list of adjustments.
	  /// The rate is only permitted to change at accrual period boundaries.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueSchedule Rate
	  {
		  get
		  {
			return rate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the initial stub, optional.
	  /// <para>
	  /// The initial stub of a swap may have a different rate from the regular accrual periods.
	  /// This property allows the stub rate to be specified, either as a known amount or a rate.
	  /// If this property is not present, then the rate derived from the {@code rate} property applies during the stub.
	  /// If this property is present and there is no initial stub, it is ignored.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<FixedRateStubCalculation> InitialStub
	  {
		  get
		  {
			return Optional.ofNullable(initialStub);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the final stub, optional.
	  /// <para>
	  /// The final stub of a swap may have a different rate from the regular accrual periods.
	  /// This property allows the stub rate to be specified, either as a known amount or a rate.
	  /// If this property is not present, then the rate derived from the {@code rate} property applies during the stub.
	  /// If this property is present and there is no initial stub, it is ignored.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<FixedRateStubCalculation> FinalStub
	  {
		  get
		  {
			return Optional.ofNullable(finalStub);
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
		  FixedRateCalculation other = (FixedRateCalculation) obj;
		  return JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(rate, other.rate) && JodaBeanUtils.equal(initialStub, other.initialStub) && JodaBeanUtils.equal(finalStub, other.finalStub);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(initialStub);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(finalStub);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("FixedRateCalculation{");
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("rate").Append('=').Append(rate).Append(',').Append(' ');
		buf.Append("initialStub").Append('=').Append(initialStub).Append(',').Append(' ');
		buf.Append("finalStub").Append('=').Append(JodaBeanUtils.ToString(finalStub));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedRateCalculation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(FixedRateCalculation), typeof(DayCount));
			  rate_Renamed = DirectMetaProperty.ofImmutable(this, "rate", typeof(FixedRateCalculation), typeof(ValueSchedule));
			  initialStub_Renamed = DirectMetaProperty.ofImmutable(this, "initialStub", typeof(FixedRateCalculation), typeof(FixedRateStubCalculation));
			  finalStub_Renamed = DirectMetaProperty.ofImmutable(this, "finalStub", typeof(FixedRateCalculation), typeof(FixedRateStubCalculation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "dayCount", "rate", "initialStub", "finalStub");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code rate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueSchedule> rate_Renamed;
		/// <summary>
		/// The meta-property for the {@code initialStub} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixedRateStubCalculation> initialStub_Renamed;
		/// <summary>
		/// The meta-property for the {@code finalStub} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixedRateStubCalculation> finalStub_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "dayCount", "rate", "initialStub", "finalStub");
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
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 3493088: // rate
			  return rate_Renamed;
			case 1233359378: // initialStub
			  return initialStub_Renamed;
			case 355242820: // finalStub
			  return finalStub_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FixedRateCalculation.Builder builder()
		{
		  return new FixedRateCalculation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FixedRateCalculation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueSchedule> rate()
		{
		  return rate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code initialStub} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixedRateStubCalculation> initialStub()
		{
		  return initialStub_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code finalStub} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixedRateStubCalculation> finalStub()
		{
		  return finalStub_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1905311443: // dayCount
			  return ((FixedRateCalculation) bean).DayCount;
			case 3493088: // rate
			  return ((FixedRateCalculation) bean).Rate;
			case 1233359378: // initialStub
			  return ((FixedRateCalculation) bean).initialStub;
			case 355242820: // finalStub
			  return ((FixedRateCalculation) bean).finalStub;
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
	  /// The bean-builder for {@code FixedRateCalculation}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FixedRateCalculation>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueSchedule rate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixedRateStubCalculation initialStub_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixedRateStubCalculation finalStub_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(FixedRateCalculation beanToCopy)
		{
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.rate_Renamed = beanToCopy.Rate;
		  this.initialStub_Renamed = beanToCopy.initialStub;
		  this.finalStub_Renamed = beanToCopy.finalStub;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 3493088: // rate
			  return rate_Renamed;
			case 1233359378: // initialStub
			  return initialStub_Renamed;
			case 355242820: // finalStub
			  return finalStub_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 3493088: // rate
			  this.rate_Renamed = (ValueSchedule) newValue;
			  break;
			case 1233359378: // initialStub
			  this.initialStub_Renamed = (FixedRateStubCalculation) newValue;
			  break;
			case 355242820: // finalStub
			  this.finalStub_Renamed = (FixedRateStubCalculation) newValue;
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

		public override FixedRateCalculation build()
		{
		  return new FixedRateCalculation(dayCount_Renamed, rate_Renamed, initialStub_Renamed, finalStub_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the day count convention.
		/// <para>
		/// This is used to convert schedule period dates to a numerical value.
		/// </para>
		/// </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the interest rate to be paid.
		/// A 5% rate will be expressed as 0.05.
		/// <para>
		/// This defines the rate as an initial amount and a list of adjustments.
		/// The rate is only permitted to change at accrual period boundaries.
		/// </para>
		/// </summary>
		/// <param name="rate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rate(ValueSchedule rate)
		{
		  JodaBeanUtils.notNull(rate, "rate");
		  this.rate_Renamed = rate;
		  return this;
		}

		/// <summary>
		/// Sets the initial stub, optional.
		/// <para>
		/// The initial stub of a swap may have a different rate from the regular accrual periods.
		/// This property allows the stub rate to be specified, either as a known amount or a rate.
		/// If this property is not present, then the rate derived from the {@code rate} property applies during the stub.
		/// If this property is present and there is no initial stub, it is ignored.
		/// </para>
		/// </summary>
		/// <param name="initialStub">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder initialStub(FixedRateStubCalculation initialStub)
		{
		  this.initialStub_Renamed = initialStub;
		  return this;
		}

		/// <summary>
		/// Sets the final stub, optional.
		/// <para>
		/// The final stub of a swap may have a different rate from the regular accrual periods.
		/// This property allows the stub rate to be specified, either as a known amount or a rate.
		/// If this property is not present, then the rate derived from the {@code rate} property applies during the stub.
		/// If this property is present and there is no initial stub, it is ignored.
		/// </para>
		/// </summary>
		/// <param name="finalStub">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder finalStub(FixedRateStubCalculation finalStub)
		{
		  this.finalStub_Renamed = finalStub;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("FixedRateCalculation.Builder{");
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("rate").Append('=').Append(JodaBeanUtils.ToString(rate_Renamed)).Append(',').Append(' ');
		  buf.Append("initialStub").Append('=').Append(JodaBeanUtils.ToString(initialStub_Renamed)).Append(',').Append(' ');
		  buf.Append("finalStub").Append('=').Append(JodaBeanUtils.ToString(finalStub_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}