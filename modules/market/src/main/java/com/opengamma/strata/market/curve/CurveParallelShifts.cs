using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioPerturbation = com.opengamma.strata.data.scenario.ScenarioPerturbation;

	/// <summary>
	/// Perturbation which applies a parallel shift to a curve.
	/// <para>
	/// The shift can be absolute or relative.
	/// An absolute shift adds the shift amount to each point on the curve.
	/// A relative shift applies a scaling to each point on the curve.
	/// </para>
	/// <para>
	/// For example, a relative shift of 0.1 (10%) multiplies each value on the curve by 1.1, and a shift of -0.2 (-20%)
	/// multiplies the value by 0.8. So for relative shifts the shifted value is {@code (value x (1 + shift))}.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CurveParallelShifts implements com.opengamma.strata.data.scenario.ScenarioPerturbation<Curve>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CurveParallelShifts : ScenarioPerturbation<Curve>, ImmutableBean
	{

	  /// <summary>
	  /// Logger. </summary>
	  private static readonly Logger log = LoggerFactory.getLogger(typeof(CurveParallelShifts));

	  /// <summary>
	  /// The type of shift to apply to the y-values of the curve.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ShiftType shiftType;
	  private readonly ShiftType shiftType;
	  /// <summary>
	  /// The amount by which the y-values are shifted.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray shiftAmounts;
	  private readonly DoubleArray shiftAmounts;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a shift that adds a fixed amount to the value at every node in the curve.
	  /// </summary>
	  /// <param name="shiftAmounts">  the amount to add to each node value in the curve </param>
	  /// <returns> a shift that adds a fixed amount to the value at every node in the curve </returns>
	  public static CurveParallelShifts absolute(params double[] shiftAmounts)
	  {
		return new CurveParallelShifts(ShiftType.ABSOLUTE, DoubleArray.copyOf(shiftAmounts));
	  }

	  /// <summary>
	  /// Creates a shift that multiplies the values at each curve node by a scaling factor.
	  /// <para>
	  /// The shift amount is a decimal percentage. For example, a shift amount of 0.1 is a
	  /// shift of +10% which multiplies the value by 1.1. A shift amount of -0.2 is a shift
	  /// of -20% which multiplies the value by 0.8.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="shiftAmounts">  the factor to multiply the value at each curve node by </param>
	  /// <returns> a shift that multiplies the values at each curve node by a scaling factor </returns>
	  public static CurveParallelShifts relative(params double[] shiftAmounts)
	  {
		return new CurveParallelShifts(ShiftType.RELATIVE, DoubleArray.copyOf(shiftAmounts));
	  }

	  //-------------------------------------------------------------------------
	  public MarketDataBox<Curve> applyTo(MarketDataBox<Curve> curve, ReferenceData refData)
	  {
		return curve.mapWithIndex(ScenarioCount, this.applyShift);
	  }

	  private Curve applyShift(Curve curve, int scenarioIndex)
	  {
		double shiftAmount = shiftAmounts.get(scenarioIndex);
		log.debug("Applying {} parallel shift of {} to curve '{}'", shiftType, shiftAmount, curve.Name);
		return ParallelShiftedCurve.of(curve, shiftType, shiftAmount);
	  }

	  public int ScenarioCount
	  {
		  get
		  {
			return shiftAmounts.size();
		  }
	  }

	  public Type<Curve> MarketDataType
	  {
		  get
		  {
			return typeof(Curve);
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurveParallelShifts}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CurveParallelShifts.Meta meta()
	  {
		return CurveParallelShifts.Meta.INSTANCE;
	  }

	  static CurveParallelShifts()
	  {
		MetaBean.register(CurveParallelShifts.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CurveParallelShifts(ShiftType shiftType, DoubleArray shiftAmounts)
	  {
		JodaBeanUtils.notNull(shiftType, "shiftType");
		JodaBeanUtils.notNull(shiftAmounts, "shiftAmounts");
		this.shiftType = shiftType;
		this.shiftAmounts = shiftAmounts;
	  }

	  public override CurveParallelShifts.Meta metaBean()
	  {
		return CurveParallelShifts.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of shift to apply to the y-values of the curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ShiftType ShiftType
	  {
		  get
		  {
			return shiftType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the amount by which the y-values are shifted. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray ShiftAmounts
	  {
		  get
		  {
			return shiftAmounts;
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
		  CurveParallelShifts other = (CurveParallelShifts) obj;
		  return JodaBeanUtils.equal(shiftType, other.shiftType) && JodaBeanUtils.equal(shiftAmounts, other.shiftAmounts);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftAmounts);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("CurveParallelShifts{");
		buf.Append("shiftType").Append('=').Append(shiftType).Append(',').Append(' ');
		buf.Append("shiftAmounts").Append('=').Append(JodaBeanUtils.ToString(shiftAmounts));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurveParallelShifts}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  shiftType_Renamed = DirectMetaProperty.ofImmutable(this, "shiftType", typeof(CurveParallelShifts), typeof(ShiftType));
			  shiftAmounts_Renamed = DirectMetaProperty.ofImmutable(this, "shiftAmounts", typeof(CurveParallelShifts), typeof(DoubleArray));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "shiftType", "shiftAmounts");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code shiftType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ShiftType> shiftType_Renamed;
		/// <summary>
		/// The meta-property for the {@code shiftAmounts} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> shiftAmounts_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "shiftType", "shiftAmounts");
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
			case 893345500: // shiftType
			  return shiftType_Renamed;
			case 2011836473: // shiftAmounts
			  return shiftAmounts_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CurveParallelShifts> builder()
		public override BeanBuilder<CurveParallelShifts> builder()
		{
		  return new CurveParallelShifts.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CurveParallelShifts);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code shiftType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ShiftType> shiftType()
		{
		  return shiftType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code shiftAmounts} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> shiftAmounts()
		{
		  return shiftAmounts_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 893345500: // shiftType
			  return ((CurveParallelShifts) bean).ShiftType;
			case 2011836473: // shiftAmounts
			  return ((CurveParallelShifts) bean).ShiftAmounts;
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
	  /// The bean-builder for {@code CurveParallelShifts}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CurveParallelShifts>
	  {

		internal ShiftType shiftType;
		internal DoubleArray shiftAmounts;

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
			case 893345500: // shiftType
			  return shiftType;
			case 2011836473: // shiftAmounts
			  return shiftAmounts;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 893345500: // shiftType
			  this.shiftType = (ShiftType) newValue;
			  break;
			case 2011836473: // shiftAmounts
			  this.shiftAmounts = (DoubleArray) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CurveParallelShifts build()
		{
		  return new CurveParallelShifts(shiftType, shiftAmounts);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("CurveParallelShifts.Builder{");
		  buf.Append("shiftType").Append('=').Append(JodaBeanUtils.ToString(shiftType)).Append(',').Append(' ');
		  buf.Append("shiftAmounts").Append('=').Append(JodaBeanUtils.ToString(shiftAmounts));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}