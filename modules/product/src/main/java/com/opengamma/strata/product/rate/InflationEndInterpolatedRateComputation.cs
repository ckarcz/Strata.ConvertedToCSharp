using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Defines the computation of inflation figures from a price index with interpolation
	/// where the start index value is known.
	/// <para>
	/// A typical application of this rate computation is payments of a capital indexed bond,
	/// where the reference start month is the start month of the bond rather than start month
	/// of the payment period.
	/// </para>
	/// <para>
	/// A price index is typically published monthly and has a delay before publication.
	/// The rate observed by this instance will be based on the specified start index value
	/// and two index observations relative to the end month.
	/// Linear interpolation based on the number of days of the payment month is used
	/// to find the appropriate value.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class InflationEndInterpolatedRateComputation implements RateComputation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class InflationEndInterpolatedRateComputation : RateComputation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double startIndexValue;
		private readonly double startIndexValue;
	  /// <summary>
	  /// The observation at the end.
	  /// <para>
	  /// The inflation rate is the ratio between the start index value and the interpolated end observations.
	  /// The end month is typically three months before the end of the period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.PriceIndexObservation endObservation;
	  private readonly PriceIndexObservation endObservation;
	  /// <summary>
	  /// The observation for interpolation at the end.
	  /// <para>
	  /// The inflation rate is the ratio between the start index value and the interpolated end observations.
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
	  /// Creates an instance from an index, start index value and reference end month.
	  /// <para>
	  /// The second end observations will be one month later than the end month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="startIndexValue">  the start index value </param>
	  /// <param name="referenceEndMonth">  the reference end month </param>
	  /// <param name="weight">  the weight </param>
	  /// <returns> the inflation rate computation </returns>
	  public static InflationEndInterpolatedRateComputation of(PriceIndex index, double startIndexValue, YearMonth referenceEndMonth, double weight)
	  {

		return new InflationEndInterpolatedRateComputation(startIndexValue, PriceIndexObservation.of(index, referenceEndMonth), PriceIndexObservation.of(index, referenceEndMonth.plusMonths(1)), weight);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isTrue(endObservation.Index.Equals(endSecondObservation.Index), "Both observations must be for the same index");
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
			return endObservation.Index;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		builder.add(Index);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationEndInterpolatedRateComputation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static InflationEndInterpolatedRateComputation.Meta meta()
	  {
		return InflationEndInterpolatedRateComputation.Meta.INSTANCE;
	  }

	  static InflationEndInterpolatedRateComputation()
	  {
		MetaBean.register(InflationEndInterpolatedRateComputation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private InflationEndInterpolatedRateComputation(double startIndexValue, PriceIndexObservation endObservation, PriceIndexObservation endSecondObservation, double weight)
	  {
		ArgChecker.notNegativeOrZero(startIndexValue, "startIndexValue");
		JodaBeanUtils.notNull(endObservation, "endObservation");
		JodaBeanUtils.notNull(endSecondObservation, "endSecondObservation");
		ArgChecker.notNegative(weight, "weight");
		this.startIndexValue = startIndexValue;
		this.endObservation = endObservation;
		this.endSecondObservation = endSecondObservation;
		this.weight = weight;
		validate();
	  }

	  public override InflationEndInterpolatedRateComputation.Meta metaBean()
	  {
		return InflationEndInterpolatedRateComputation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start index value.
	  /// <para>
	  /// The published index value of the start month.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double StartIndexValue
	  {
		  get
		  {
			return startIndexValue;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the observation at the end.
	  /// <para>
	  /// The inflation rate is the ratio between the start index value and the interpolated end observations.
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
	  /// The inflation rate is the ratio between the start index value and the interpolated end observations.
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
		  InflationEndInterpolatedRateComputation other = (InflationEndInterpolatedRateComputation) obj;
		  return JodaBeanUtils.equal(startIndexValue, other.startIndexValue) && JodaBeanUtils.equal(endObservation, other.endObservation) && JodaBeanUtils.equal(endSecondObservation, other.endSecondObservation) && JodaBeanUtils.equal(weight, other.weight);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startIndexValue);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endObservation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endSecondObservation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(weight);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("InflationEndInterpolatedRateComputation{");
		buf.Append("startIndexValue").Append('=').Append(startIndexValue).Append(',').Append(' ');
		buf.Append("endObservation").Append('=').Append(endObservation).Append(',').Append(' ');
		buf.Append("endSecondObservation").Append('=').Append(endSecondObservation).Append(',').Append(' ');
		buf.Append("weight").Append('=').Append(JodaBeanUtils.ToString(weight));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationEndInterpolatedRateComputation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  startIndexValue_Renamed = DirectMetaProperty.ofImmutable(this, "startIndexValue", typeof(InflationEndInterpolatedRateComputation), Double.TYPE);
			  endObservation_Renamed = DirectMetaProperty.ofImmutable(this, "endObservation", typeof(InflationEndInterpolatedRateComputation), typeof(PriceIndexObservation));
			  endSecondObservation_Renamed = DirectMetaProperty.ofImmutable(this, "endSecondObservation", typeof(InflationEndInterpolatedRateComputation), typeof(PriceIndexObservation));
			  weight_Renamed = DirectMetaProperty.ofImmutable(this, "weight", typeof(InflationEndInterpolatedRateComputation), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "startIndexValue", "endObservation", "endSecondObservation", "weight");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code startIndexValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> startIndexValue_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "startIndexValue", "endObservation", "endSecondObservation", "weight");
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
			case -1656407615: // startIndexValue
			  return startIndexValue_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends InflationEndInterpolatedRateComputation> builder()
		public override BeanBuilder<InflationEndInterpolatedRateComputation> builder()
		{
		  return new InflationEndInterpolatedRateComputation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(InflationEndInterpolatedRateComputation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code startIndexValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> startIndexValue()
		{
		  return startIndexValue_Renamed;
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
			case -1656407615: // startIndexValue
			  return ((InflationEndInterpolatedRateComputation) bean).StartIndexValue;
			case 82210897: // endObservation
			  return ((InflationEndInterpolatedRateComputation) bean).EndObservation;
			case 1209389949: // endSecondObservation
			  return ((InflationEndInterpolatedRateComputation) bean).EndSecondObservation;
			case -791592328: // weight
			  return ((InflationEndInterpolatedRateComputation) bean).Weight;
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
	  /// The bean-builder for {@code InflationEndInterpolatedRateComputation}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<InflationEndInterpolatedRateComputation>
	  {

		internal double startIndexValue;
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
			case -1656407615: // startIndexValue
			  return startIndexValue;
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
			case -1656407615: // startIndexValue
			  this.startIndexValue = (double?) newValue.Value;
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

		public override InflationEndInterpolatedRateComputation build()
		{
		  return new InflationEndInterpolatedRateComputation(startIndexValue, endObservation, endSecondObservation, weight);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("InflationEndInterpolatedRateComputation.Builder{");
		  buf.Append("startIndexValue").Append('=').Append(JodaBeanUtils.ToString(startIndexValue)).Append(',').Append(' ');
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