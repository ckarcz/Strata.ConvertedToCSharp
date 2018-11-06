using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Index = com.opengamma.strata.basics.index.Index;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using PriceIndexObservation = com.opengamma.strata.basics.index.PriceIndexObservation;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Defines the computation of inflation figures from a price index with interpolation.
	/// <para>
	/// A price index is typically published monthly and has a delay before publication.
	/// The rate observed by this instance will be based on four observations of the index,
	/// two relative to the accrual start date and two relative to the accrual end date.
	/// Linear interpolation based on the number of days of the payment month is used
	/// to find the appropriate value for each pair of observations.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class InflationInterpolatedRateComputation implements RateComputation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class InflationInterpolatedRateComputation : RateComputation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.PriceIndexObservation startObservation;
		private readonly PriceIndexObservation startObservation;
	  /// <summary>
	  /// The observation for interpolation at the start.
	  /// <para>
	  /// The inflation rate is the ratio between the interpolated start and end observations.
	  /// The month is typically one month after the month of the start observation.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.PriceIndexObservation startSecondObservation;
	  private readonly PriceIndexObservation startSecondObservation;
	  /// <summary>
	  /// The observation at the end.
	  /// <para>
	  /// The inflation rate is the ratio between the interpolated start and end observations.
	  /// The end month is typically three months before the end of the period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.PriceIndexObservation endObservation;
	  private readonly PriceIndexObservation endObservation;
	  /// <summary>
	  /// The observation for interpolation at the end.
	  /// <para>
	  /// The inflation rate is the ratio between the interpolated start and end observations.
	  /// The month is typically one month after the month of the end observation.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.PriceIndexObservation endSecondObservation;
	  private readonly PriceIndexObservation endSecondObservation;
	  /// <summary>
	  /// The positive weight used when interpolating.
	  /// <para>
	  /// Given two price index observations, typically in adjacent months, the weight is used
	  /// to determine the adjusted index value. The value is given by the formula
	  /// {@code (weight * price_index_1 + (1 - weight) * price_index_2)}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double weight;
	  private readonly double weight;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance from an index, reference start month and reference end month.
	  /// <para>
	  /// The second start/end observations will be one month later than the start/end month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="referenceStartMonth">  the reference start month </param>
	  /// <param name="referenceEndMonth">  the reference end month </param>
	  /// <param name="weight">  the weight </param>
	  /// <returns> the inflation rate computation </returns>
	  public static InflationInterpolatedRateComputation of(PriceIndex index, YearMonth referenceStartMonth, YearMonth referenceEndMonth, double weight)
	  {

		return new InflationInterpolatedRateComputation(PriceIndexObservation.of(index, referenceStartMonth), PriceIndexObservation.of(index, referenceStartMonth.plusMonths(1)), PriceIndexObservation.of(index, referenceEndMonth), PriceIndexObservation.of(index, referenceEndMonth.plusMonths(1)), weight);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isTrue(startObservation.Index.Equals(endObservation.Index), "All observations must be for the same index");
		ArgChecker.isTrue(startObservation.Index.Equals(startSecondObservation.Index), "All observations must be for the same index");
		ArgChecker.isTrue(startObservation.Index.Equals(endSecondObservation.Index), "All observations must be for the same index");
		ArgChecker.inOrderNotEqual(startObservation.FixingMonth, startSecondObservation.FixingMonth, "startObservation", "startSecondObservation");
		ArgChecker.inOrderOrEqual(startSecondObservation.FixingMonth, endObservation.FixingMonth, "startSecondObservation", "endObservation");
		ArgChecker.inOrderNotEqual(endObservation.FixingMonth, endSecondObservation.FixingMonth, "endObservation", "endSecondObservation");
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Price index.
	  /// </summary>
	  /// <returns> the Price index </returns>
	  public PriceIndex Index
	  {
		  get
		  {
			return startObservation.Index;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		builder.add(Index);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationInterpolatedRateComputation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static InflationInterpolatedRateComputation.Meta meta()
	  {
		return InflationInterpolatedRateComputation.Meta.INSTANCE;
	  }

	  static InflationInterpolatedRateComputation()
	  {
		MetaBean.register(InflationInterpolatedRateComputation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private InflationInterpolatedRateComputation(PriceIndexObservation startObservation, PriceIndexObservation startSecondObservation, PriceIndexObservation endObservation, PriceIndexObservation endSecondObservation, double weight)
	  {
		JodaBeanUtils.notNull(startObservation, "startObservation");
		JodaBeanUtils.notNull(startSecondObservation, "startSecondObservation");
		JodaBeanUtils.notNull(endObservation, "endObservation");
		JodaBeanUtils.notNull(endSecondObservation, "endSecondObservation");
		ArgChecker.notNegative(weight, "weight");
		this.startObservation = startObservation;
		this.startSecondObservation = startSecondObservation;
		this.endObservation = endObservation;
		this.endSecondObservation = endSecondObservation;
		this.weight = weight;
		validate();
	  }

	  public override InflationInterpolatedRateComputation.Meta metaBean()
	  {
		return InflationInterpolatedRateComputation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the observation at the start.
	  /// <para>
	  /// The inflation rate is the ratio between the interpolated start and end observations.
	  /// The start month is typically three months before the start of the period.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PriceIndexObservation StartObservation
	  {
		  get
		  {
			return startObservation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the observation for interpolation at the start.
	  /// <para>
	  /// The inflation rate is the ratio between the interpolated start and end observations.
	  /// The month is typically one month after the month of the start observation.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PriceIndexObservation StartSecondObservation
	  {
		  get
		  {
			return startSecondObservation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the observation at the end.
	  /// <para>
	  /// The inflation rate is the ratio between the interpolated start and end observations.
	  /// The end month is typically three months before the end of the period.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PriceIndexObservation EndObservation
	  {
		  get
		  {
			return endObservation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the observation for interpolation at the end.
	  /// <para>
	  /// The inflation rate is the ratio between the interpolated start and end observations.
	  /// The month is typically one month after the month of the end observation.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PriceIndexObservation EndSecondObservation
	  {
		  get
		  {
			return endSecondObservation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the positive weight used when interpolating.
	  /// <para>
	  /// Given two price index observations, typically in adjacent months, the weight is used
	  /// to determine the adjusted index value. The value is given by the formula
	  /// {@code (weight * price_index_1 + (1 - weight) * price_index_2)}.
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
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  InflationInterpolatedRateComputation other = (InflationInterpolatedRateComputation) obj;
		  return JodaBeanUtils.equal(startObservation, other.startObservation) && JodaBeanUtils.equal(startSecondObservation, other.startSecondObservation) && JodaBeanUtils.equal(endObservation, other.endObservation) && JodaBeanUtils.equal(endSecondObservation, other.endSecondObservation) && JodaBeanUtils.equal(weight, other.weight);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startObservation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startSecondObservation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endObservation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endSecondObservation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(weight);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("InflationInterpolatedRateComputation{");
		buf.Append("startObservation").Append('=').Append(startObservation).Append(',').Append(' ');
		buf.Append("startSecondObservation").Append('=').Append(startSecondObservation).Append(',').Append(' ');
		buf.Append("endObservation").Append('=').Append(endObservation).Append(',').Append(' ');
		buf.Append("endSecondObservation").Append('=').Append(endSecondObservation).Append(',').Append(' ');
		buf.Append("weight").Append('=').Append(JodaBeanUtils.ToString(weight));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationInterpolatedRateComputation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  startObservation_Renamed = DirectMetaProperty.ofImmutable(this, "startObservation", typeof(InflationInterpolatedRateComputation), typeof(PriceIndexObservation));
			  startSecondObservation_Renamed = DirectMetaProperty.ofImmutable(this, "startSecondObservation", typeof(InflationInterpolatedRateComputation), typeof(PriceIndexObservation));
			  endObservation_Renamed = DirectMetaProperty.ofImmutable(this, "endObservation", typeof(InflationInterpolatedRateComputation), typeof(PriceIndexObservation));
			  endSecondObservation_Renamed = DirectMetaProperty.ofImmutable(this, "endSecondObservation", typeof(InflationInterpolatedRateComputation), typeof(PriceIndexObservation));
			  weight_Renamed = DirectMetaProperty.ofImmutable(this, "weight", typeof(InflationInterpolatedRateComputation), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "startObservation", "startSecondObservation", "endObservation", "endSecondObservation", "weight");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code startObservation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PriceIndexObservation> startObservation_Renamed;
		/// <summary>
		/// The meta-property for the {@code startSecondObservation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PriceIndexObservation> startSecondObservation_Renamed;
		/// <summary>
		/// The meta-property for the {@code endObservation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PriceIndexObservation> endObservation_Renamed;
		/// <summary>
		/// The meta-property for the {@code endSecondObservation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PriceIndexObservation> endSecondObservation_Renamed;
		/// <summary>
		/// The meta-property for the {@code weight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> weight_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "startObservation", "startSecondObservation", "endObservation", "endSecondObservation", "weight");
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
			case -1098347926: // startObservation
			  return startObservation_Renamed;
			case 1287141078: // startSecondObservation
			  return startSecondObservation_Renamed;
			case 82210897: // endObservation
			  return endObservation_Renamed;
			case 1209389949: // endSecondObservation
			  return endSecondObservation_Renamed;
			case -791592328: // weight
			  return weight_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends InflationInterpolatedRateComputation> builder()
		public override BeanBuilder<InflationInterpolatedRateComputation> builder()
		{
		  return new InflationInterpolatedRateComputation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(InflationInterpolatedRateComputation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code startObservation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PriceIndexObservation> startObservation()
		{
		  return startObservation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startSecondObservation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PriceIndexObservation> startSecondObservation()
		{
		  return startSecondObservation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endObservation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PriceIndexObservation> endObservation()
		{
		  return endObservation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endSecondObservation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PriceIndexObservation> endSecondObservation()
		{
		  return endSecondObservation_Renamed;
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
			case -1098347926: // startObservation
			  return ((InflationInterpolatedRateComputation) bean).StartObservation;
			case 1287141078: // startSecondObservation
			  return ((InflationInterpolatedRateComputation) bean).StartSecondObservation;
			case 82210897: // endObservation
			  return ((InflationInterpolatedRateComputation) bean).EndObservation;
			case 1209389949: // endSecondObservation
			  return ((InflationInterpolatedRateComputation) bean).EndSecondObservation;
			case -791592328: // weight
			  return ((InflationInterpolatedRateComputation) bean).Weight;
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
	  /// The bean-builder for {@code InflationInterpolatedRateComputation}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<InflationInterpolatedRateComputation>
	  {

		internal PriceIndexObservation startObservation;
		internal PriceIndexObservation startSecondObservation;
		internal PriceIndexObservation endObservation;
		internal PriceIndexObservation endSecondObservation;
		internal double weight;

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
			case -1098347926: // startObservation
			  return startObservation;
			case 1287141078: // startSecondObservation
			  return startSecondObservation;
			case 82210897: // endObservation
			  return endObservation;
			case 1209389949: // endSecondObservation
			  return endSecondObservation;
			case -791592328: // weight
			  return weight;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1098347926: // startObservation
			  this.startObservation = (PriceIndexObservation) newValue;
			  break;
			case 1287141078: // startSecondObservation
			  this.startSecondObservation = (PriceIndexObservation) newValue;
			  break;
			case 82210897: // endObservation
			  this.endObservation = (PriceIndexObservation) newValue;
			  break;
			case 1209389949: // endSecondObservation
			  this.endSecondObservation = (PriceIndexObservation) newValue;
			  break;
			case -791592328: // weight
			  this.weight = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override InflationInterpolatedRateComputation build()
		{
		  return new InflationInterpolatedRateComputation(startObservation, startSecondObservation, endObservation, endSecondObservation, weight);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("InflationInterpolatedRateComputation.Builder{");
		  buf.Append("startObservation").Append('=').Append(JodaBeanUtils.ToString(startObservation)).Append(',').Append(' ');
		  buf.Append("startSecondObservation").Append('=').Append(JodaBeanUtils.ToString(startSecondObservation)).Append(',').Append(' ');
		  buf.Append("endObservation").Append('=').Append(JodaBeanUtils.ToString(endObservation)).Append(',').Append(' ');
		  buf.Append("endSecondObservation").Append('=').Append(JodaBeanUtils.ToString(endSecondObservation)).Append(',').Append(' ');
		  buf.Append("weight").Append('=').Append(JodaBeanUtils.ToString(weight));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}