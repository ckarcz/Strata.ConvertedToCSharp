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
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A single fixing of an index that is observed by {@code IborAveragedRateComputation}.
	/// <para>
	/// The interest rate is determined for each reset period, with the weight used
	/// to create a weighted average.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class IborAveragedFixing implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborAveragedFixing : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.IborIndexObservation observation;
		private readonly IborIndexObservation observation;
	  /// <summary>
	  /// The fixed rate for the fixing date, optional.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// In certain circumstances two counterparties agree the rate of a fixing when the contract starts.
	  /// It is used in place of an observed fixing.
	  /// Other calculation elements, such as gearing or spread, still apply.
	  /// </para>
	  /// <para>
	  /// If the value not present, which is the normal case, then the rate is
	  /// observed via the normal fixing process.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> fixedRate;
	  private readonly double? fixedRate;
	  /// <summary>
	  /// The weight to apply to this fixing.
	  /// <para>
	  /// If the averaging is unweighted, then all weights must be one.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double weight;
	  private readonly double weight;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a {@code IborAveragedFixing} from the fixing date with a weight of 1.
	  /// </summary>
	  /// <param name="observation">  the Ibor observation </param>
	  /// <returns> the unweighted fixing information </returns>
	  public static IborAveragedFixing of(IborIndexObservation observation)
	  {
		return of(observation, null);
	  }

	  /// <summary>
	  /// Creates a {@code IborAveragedFixing} from the fixing date with a weight of 1.
	  /// </summary>
	  /// <param name="observation">  the Ibor observation </param>
	  /// <param name="fixedRate">  the fixed rate for the fixing date, optional, may be null </param>
	  /// <returns> the unweighted fixing information </returns>
	  public static IborAveragedFixing of(IborIndexObservation observation, double? fixedRate)
	  {
		return IborAveragedFixing.builder().observation(observation).fixedRate(fixedRate).build();
	  }

	  /// <summary>
	  /// Creates a {@code IborAveragedFixing} from the fixing date, calculating the weight
	  /// from the number of days in the reset period.
	  /// <para>
	  /// This implements the standard approach to average weights, which is to set each
	  /// weight to the actual number of days between the start and end of the reset period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the Ibor observation </param>
	  /// <param name="startDate">  the start date of the reset period </param>
	  /// <param name="endDate">  the end date of the reset period </param>
	  /// <returns> the weighted fixing information </returns>
	  public static IborAveragedFixing ofDaysInResetPeriod(IborIndexObservation observation, LocalDate startDate, LocalDate endDate)
	  {
		return ofDaysInResetPeriod(observation, startDate, endDate, null);
	  }

	  /// <summary>
	  /// Creates a {@code IborAveragedFixing} from the fixing date, calculating the weight
	  /// from the number of days in the reset period.
	  /// <para>
	  /// This implements the standard approach to average weights, which is to set each
	  /// weight to the actual number of days between the start and end of the reset period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the Ibor observation </param>
	  /// <param name="startDate">  the start date of the reset period </param>
	  /// <param name="endDate">  the end date of the reset period </param>
	  /// <param name="fixedRate">  the fixed rate for the fixing date, optional, may be null </param>
	  /// <returns> the weighted fixing information </returns>
	  public static IborAveragedFixing ofDaysInResetPeriod(IborIndexObservation observation, LocalDate startDate, LocalDate endDate, double? fixedRate)
	  {
		ArgChecker.notNull(observation, "observation");
		ArgChecker.notNull(startDate, "startDate");
		ArgChecker.notNull(endDate, "endDate");
		return IborAveragedFixing.builder().observation(observation).fixedRate(fixedRate).weight(endDate.toEpochDay() - startDate.toEpochDay()).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.weight(1d);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborAveragedFixing}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborAveragedFixing.Meta meta()
	  {
		return IborAveragedFixing.Meta.INSTANCE;
	  }

	  static IborAveragedFixing()
	  {
		MetaBean.register(IborAveragedFixing.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static IborAveragedFixing.Builder builder()
	  {
		return new IborAveragedFixing.Builder();
	  }

	  private IborAveragedFixing(IborIndexObservation observation, double? fixedRate, double weight)
	  {
		JodaBeanUtils.notNull(observation, "observation");
		this.observation = observation;
		this.fixedRate = fixedRate;
		this.weight = weight;
	  }

	  public override IborAveragedFixing.Meta metaBean()
	  {
		return IborAveragedFixing.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index observation to use to determine a rate for the reset period. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndexObservation Observation
	  {
		  get
		  {
			return observation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed rate for the fixing date, optional.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// In certain circumstances two counterparties agree the rate of a fixing when the contract starts.
	  /// It is used in place of an observed fixing.
	  /// Other calculation elements, such as gearing or spread, still apply.
	  /// </para>
	  /// <para>
	  /// If the value not present, which is the normal case, then the rate is
	  /// observed via the normal fixing process.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? FixedRate
	  {
		  get
		  {
			return fixedRate != null ? double?.of(fixedRate) : double?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the weight to apply to this fixing.
	  /// <para>
	  /// If the averaging is unweighted, then all weights must be one.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Weight
	  {
		  get
		  {
			return weight;
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
		  IborAveragedFixing other = (IborAveragedFixing) obj;
		  return JodaBeanUtils.equal(observation, other.observation) && JodaBeanUtils.equal(fixedRate, other.fixedRate) && JodaBeanUtils.equal(weight, other.weight);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(weight);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("IborAveragedFixing{");
		buf.Append("observation").Append('=').Append(observation).Append(',').Append(' ');
		buf.Append("fixedRate").Append('=').Append(fixedRate).Append(',').Append(' ');
		buf.Append("weight").Append('=').Append(JodaBeanUtils.ToString(weight));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborAveragedFixing}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  observation_Renamed = DirectMetaProperty.ofImmutable(this, "observation", typeof(IborAveragedFixing), typeof(IborIndexObservation));
			  fixedRate_Renamed = DirectMetaProperty.ofImmutable(this, "fixedRate", typeof(IborAveragedFixing), typeof(Double));
			  weight_Renamed = DirectMetaProperty.ofImmutable(this, "weight", typeof(IborAveragedFixing), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "observation", "fixedRate", "weight");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code observation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndexObservation> observation_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixedRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> fixedRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code weight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> weight_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "observation", "fixedRate", "weight");
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
			case 122345516: // observation
			  return observation_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -791592328: // weight
			  return weight_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override IborAveragedFixing.Builder builder()
		{
		  return new IborAveragedFixing.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborAveragedFixing);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code observation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndexObservation> observation()
		{
		  return observation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixedRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> fixedRate()
		{
		  return fixedRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code weight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> weight()
		{
		  return weight_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 122345516: // observation
			  return ((IborAveragedFixing) bean).Observation;
			case 747425396: // fixedRate
			  return ((IborAveragedFixing) bean).fixedRate;
			case -791592328: // weight
			  return ((IborAveragedFixing) bean).Weight;
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
	  /// The bean-builder for {@code IborAveragedFixing}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<IborAveragedFixing>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndexObservation observation_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double? fixedRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double weight_Renamed;

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
		internal Builder(IborAveragedFixing beanToCopy)
		{
		  this.observation_Renamed = beanToCopy.Observation;
		  this.fixedRate_Renamed = beanToCopy.fixedRate;
		  this.weight_Renamed = beanToCopy.Weight;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 122345516: // observation
			  return observation_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -791592328: // weight
			  return weight_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 122345516: // observation
			  this.observation_Renamed = (IborIndexObservation) newValue;
			  break;
			case 747425396: // fixedRate
			  this.fixedRate_Renamed = (double?) newValue;
			  break;
			case -791592328: // weight
			  this.weight_Renamed = (double?) newValue.Value;
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

		public override IborAveragedFixing build()
		{
		  return new IborAveragedFixing(observation_Renamed, fixedRate_Renamed, weight_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the Ibor index observation to use to determine a rate for the reset period. </summary>
		/// <param name="observation">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder observation(IborIndexObservation observation)
		{
		  JodaBeanUtils.notNull(observation, "observation");
		  this.observation_Renamed = observation;
		  return this;
		}

		/// <summary>
		/// Sets the fixed rate for the fixing date, optional.
		/// A 5% rate will be expressed as 0.05.
		/// <para>
		/// In certain circumstances two counterparties agree the rate of a fixing when the contract starts.
		/// It is used in place of an observed fixing.
		/// Other calculation elements, such as gearing or spread, still apply.
		/// </para>
		/// <para>
		/// If the value not present, which is the normal case, then the rate is
		/// observed via the normal fixing process.
		/// </para>
		/// </summary>
		/// <param name="fixedRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixedRate(double? fixedRate)
		{
		  this.fixedRate_Renamed = fixedRate;
		  return this;
		}

		/// <summary>
		/// Sets the weight to apply to this fixing.
		/// <para>
		/// If the averaging is unweighted, then all weights must be one.
		/// </para>
		/// </summary>
		/// <param name="weight">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder weight(double weight)
		{
		  this.weight_Renamed = weight;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("IborAveragedFixing.Builder{");
		  buf.Append("observation").Append('=').Append(JodaBeanUtils.ToString(observation_Renamed)).Append(',').Append(' ');
		  buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate_Renamed)).Append(',').Append(' ');
		  buf.Append("weight").Append('=').Append(JodaBeanUtils.ToString(weight_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}