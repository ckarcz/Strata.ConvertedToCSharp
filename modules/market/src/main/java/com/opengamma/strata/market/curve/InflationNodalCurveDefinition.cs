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
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Provides the definition of how to calibrate a curve for inflation, optionally including seasonality.
	/// <para>
	/// This allows a "normal" curve definition to be combined with the last fixing,
	/// optionally adding seasonality.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", metaScope = "private") final class InflationNodalCurveDefinition implements NodalCurveDefinition, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class InflationNodalCurveDefinition : NodalCurveDefinition, ImmutableBean
	{

	  /// <summary>
	  /// The no seasonality definition.
	  /// </summary>
	  private static readonly SeasonalityDefinition NO_SEASONALITY_DEFINITION = SeasonalityDefinition.of(DoubleArray.filled(12, 1d), ShiftType.SCALED);

	  /// <summary>
	  /// The curve name.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final NodalCurveDefinition curveWithoutFixingDefinition;
	  private readonly NodalCurveDefinition curveWithoutFixingDefinition;
	  /// <summary>
	  /// Last fixing date.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.YearMonth lastFixingMonth;
	  private readonly YearMonth lastFixingMonth;
	  /// <summary>
	  /// Last fixing value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double lastFixingValue;
	  private readonly double lastFixingValue;
	  /// <summary>
	  /// The seasonality definition associated to the curve.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final SeasonalityDefinition seasonalityDefinition;
	  private readonly SeasonalityDefinition seasonalityDefinition;

	  //-------------------------------------------------------------------------
	  // restricted constructor
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor InflationNodalCurveDefinition(NodalCurveDefinition curveWithoutFixing, java.time.YearMonth lastFixingMonth, double lastFixingValue, SeasonalityDefinition seasonalityDefinition)
	  internal InflationNodalCurveDefinition(NodalCurveDefinition curveWithoutFixing, YearMonth lastFixingMonth, double lastFixingValue, SeasonalityDefinition seasonalityDefinition)
	  {

		this.curveWithoutFixingDefinition = curveWithoutFixing;
		this.lastFixingMonth = lastFixingMonth;
		this.lastFixingValue = lastFixingValue;
		if (seasonalityDefinition == null)
		{
		  this.seasonalityDefinition = NO_SEASONALITY_DEFINITION;
		}
		else
		{
		  this.seasonalityDefinition = seasonalityDefinition;
		}
	  }

	  public CurveName Name
	  {
		  get
		  {
			return curveWithoutFixingDefinition.Name;
		  }
	  }

	  public ValueType YValueType
	  {
		  get
		  {
			return curveWithoutFixingDefinition.YValueType;
		  }
	  }

	  public ImmutableList<CurveNode> Nodes
	  {
		  get
		  {
			return curveWithoutFixingDefinition.Nodes;
		  }
	  }

	  public NodalCurveDefinition filtered(LocalDate valuationDate, ReferenceData refData)
	  {
		return curveWithoutFixingDefinition.filtered(valuationDate, refData);
	  }

	  public CurveMetadata metadata(LocalDate valuationDate, ReferenceData refData)
	  {
		return curveWithoutFixingDefinition.metadata(valuationDate, refData);
	  }

	  public NodalCurve curve(LocalDate valuationDate, CurveMetadata metadata, DoubleArray parameters)
	  {
		NodalCurve curveWithoutFixing = curveWithoutFixingDefinition.curve(valuationDate, metadata, parameters);
		return InflationNodalCurve.of(curveWithoutFixing, valuationDate, lastFixingMonth, lastFixingValue, seasonalityDefinition);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationNodalCurveDefinition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static MetaBean meta()
	  {
		return InflationNodalCurveDefinition.Meta.INSTANCE;
	  }

	  static InflationNodalCurveDefinition()
	  {
		MetaBean.register(InflationNodalCurveDefinition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override MetaBean metaBean()
	  {
		return InflationNodalCurveDefinition.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public NodalCurveDefinition CurveWithoutFixingDefinition
	  {
		  get
		  {
			return curveWithoutFixingDefinition;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets last fixing date. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public YearMonth LastFixingMonth
	  {
		  get
		  {
			return lastFixingMonth;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets last fixing value. </summary>
	  /// <returns> the value of the property </returns>
	  public double LastFixingValue
	  {
		  get
		  {
			return lastFixingValue;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the seasonality definition associated to the curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SeasonalityDefinition SeasonalityDefinition
	  {
		  get
		  {
			return seasonalityDefinition;
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
		  InflationNodalCurveDefinition other = (InflationNodalCurveDefinition) obj;
		  return JodaBeanUtils.equal(curveWithoutFixingDefinition, other.curveWithoutFixingDefinition) && JodaBeanUtils.equal(lastFixingMonth, other.lastFixingMonth) && JodaBeanUtils.equal(lastFixingValue, other.lastFixingValue) && JodaBeanUtils.equal(seasonalityDefinition, other.seasonalityDefinition);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curveWithoutFixingDefinition);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastFixingMonth);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastFixingValue);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(seasonalityDefinition);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("InflationNodalCurveDefinition{");
		buf.Append("curveWithoutFixingDefinition").Append('=').Append(curveWithoutFixingDefinition).Append(',').Append(' ');
		buf.Append("lastFixingMonth").Append('=').Append(lastFixingMonth).Append(',').Append(' ');
		buf.Append("lastFixingValue").Append('=').Append(lastFixingValue).Append(',').Append(' ');
		buf.Append("seasonalityDefinition").Append('=').Append(JodaBeanUtils.ToString(seasonalityDefinition));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationNodalCurveDefinition}.
	  /// </summary>
	  private sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  curveWithoutFixingDefinition = DirectMetaProperty.ofImmutable(this, "curveWithoutFixingDefinition", typeof(InflationNodalCurveDefinition), typeof(NodalCurveDefinition));
			  lastFixingMonth = DirectMetaProperty.ofImmutable(this, "lastFixingMonth", typeof(InflationNodalCurveDefinition), typeof(YearMonth));
			  lastFixingValue = DirectMetaProperty.ofImmutable(this, "lastFixingValue", typeof(InflationNodalCurveDefinition), Double.TYPE);
			  seasonalityDefinition = DirectMetaProperty.ofImmutable(this, "seasonalityDefinition", typeof(InflationNodalCurveDefinition), typeof(SeasonalityDefinition));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "curveWithoutFixingDefinition", "lastFixingMonth", "lastFixingValue", "seasonalityDefinition");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code curveWithoutFixingDefinition} property.
		/// </summary>
		internal MetaProperty<NodalCurveDefinition> curveWithoutFixingDefinition;
		/// <summary>
		/// The meta-property for the {@code lastFixingMonth} property.
		/// </summary>
		internal MetaProperty<YearMonth> lastFixingMonth;
		/// <summary>
		/// The meta-property for the {@code lastFixingValue} property.
		/// </summary>
		internal MetaProperty<double> lastFixingValue;
		/// <summary>
		/// The meta-property for the {@code seasonalityDefinition} property.
		/// </summary>
		internal MetaProperty<SeasonalityDefinition> seasonalityDefinition;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "curveWithoutFixingDefinition", "lastFixingMonth", "lastFixingValue", "seasonalityDefinition");
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
			case -249814055: // curveWithoutFixingDefinition
			  return curveWithoutFixingDefinition;
			case -1842439587: // lastFixingMonth
			  return lastFixingMonth;
			case -1834546866: // lastFixingValue
			  return lastFixingValue;
			case 1835044115: // seasonalityDefinition
			  return seasonalityDefinition;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends InflationNodalCurveDefinition> builder()
		public override BeanBuilder<InflationNodalCurveDefinition> builder()
		{
		  return new InflationNodalCurveDefinition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(InflationNodalCurveDefinition);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -249814055: // curveWithoutFixingDefinition
			  return ((InflationNodalCurveDefinition) bean).CurveWithoutFixingDefinition;
			case -1842439587: // lastFixingMonth
			  return ((InflationNodalCurveDefinition) bean).LastFixingMonth;
			case -1834546866: // lastFixingValue
			  return ((InflationNodalCurveDefinition) bean).LastFixingValue;
			case 1835044115: // seasonalityDefinition
			  return ((InflationNodalCurveDefinition) bean).SeasonalityDefinition;
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
	  /// The bean-builder for {@code InflationNodalCurveDefinition}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<InflationNodalCurveDefinition>
	  {

		internal NodalCurveDefinition curveWithoutFixingDefinition;
		internal YearMonth lastFixingMonth;
		internal double lastFixingValue;
		internal SeasonalityDefinition seasonalityDefinition;

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
			case -249814055: // curveWithoutFixingDefinition
			  return curveWithoutFixingDefinition;
			case -1842439587: // lastFixingMonth
			  return lastFixingMonth;
			case -1834546866: // lastFixingValue
			  return lastFixingValue;
			case 1835044115: // seasonalityDefinition
			  return seasonalityDefinition;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -249814055: // curveWithoutFixingDefinition
			  this.curveWithoutFixingDefinition = (NodalCurveDefinition) newValue;
			  break;
			case -1842439587: // lastFixingMonth
			  this.lastFixingMonth = (YearMonth) newValue;
			  break;
			case -1834546866: // lastFixingValue
			  this.lastFixingValue = (double?) newValue.Value;
			  break;
			case 1835044115: // seasonalityDefinition
			  this.seasonalityDefinition = (SeasonalityDefinition) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override InflationNodalCurveDefinition build()
		{
		  return new InflationNodalCurveDefinition(curveWithoutFixingDefinition, lastFixingMonth, lastFixingValue, seasonalityDefinition);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("InflationNodalCurveDefinition.Builder{");
		  buf.Append("curveWithoutFixingDefinition").Append('=').Append(JodaBeanUtils.ToString(curveWithoutFixingDefinition)).Append(',').Append(' ');
		  buf.Append("lastFixingMonth").Append('=').Append(JodaBeanUtils.ToString(lastFixingMonth)).Append(',').Append(' ');
		  buf.Append("lastFixingValue").Append('=').Append(JodaBeanUtils.ToString(lastFixingValue)).Append(',').Append(' ');
		  buf.Append("seasonalityDefinition").Append('=').Append(JodaBeanUtils.ToString(seasonalityDefinition));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}