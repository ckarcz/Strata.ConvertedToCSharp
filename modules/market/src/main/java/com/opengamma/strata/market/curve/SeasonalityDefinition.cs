﻿using System;
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
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Provides the definition of seasonality for a price index curve.
	/// <para>
	/// The seasonality is describe by a adjustment type and the month on month adjustments.
	/// The adjustment type is usually <seealso cref="ShiftType#SCALED"/> (multiplicative) or <seealso cref="ShiftType#ABSOLUTE"/> (additive).
	/// The month on month adjustment is an array of length 12 with the first element being the
	/// adjustment from January to February, the second element being the adjustment from February to March,
	/// and so on to the 12th element being the adjustment from  December to January.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class SeasonalityDefinition implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SeasonalityDefinition : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray seasonalityMonthOnMonth;
		private readonly DoubleArray seasonalityMonthOnMonth;
	  /// <summary>
	  /// The shift type applied to the unadjusted value and the adjustment.
	  /// (value, seasonality) -> adjustmentType.applyShift(value, seasonality).
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ShiftType adjustmentType;
	  private readonly ShiftType adjustmentType;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of the seasonality.
	  /// </summary>
	  /// <param name="seasonalityMonthOnMonth">  the month-on-month seasonality </param>
	  /// <param name="adjustmentType">  the adjustment type </param>
	  /// <returns> the instance </returns>
	  public static SeasonalityDefinition of(DoubleArray seasonalityMonthOnMonth, ShiftType adjustmentType)
	  {
		ArgChecker.isTrue(seasonalityMonthOnMonth.size() == 12, "seasonality must be of length 12");
		return new SeasonalityDefinition(seasonalityMonthOnMonth, adjustmentType);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SeasonalityDefinition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SeasonalityDefinition.Meta meta()
	  {
		return SeasonalityDefinition.Meta.INSTANCE;
	  }

	  static SeasonalityDefinition()
	  {
		MetaBean.register(SeasonalityDefinition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private SeasonalityDefinition(DoubleArray seasonalityMonthOnMonth, ShiftType adjustmentType)
	  {
		JodaBeanUtils.notNull(seasonalityMonthOnMonth, "seasonalityMonthOnMonth");
		JodaBeanUtils.notNull(adjustmentType, "adjustmentType");
		this.seasonalityMonthOnMonth = seasonalityMonthOnMonth;
		this.adjustmentType = adjustmentType;
	  }

	  public override SeasonalityDefinition.Meta metaBean()
	  {
		return SeasonalityDefinition.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the month on month adjustment.
	  /// <para>
	  /// This is an array of length 12, with the first element being the adjustment from
	  /// January to February, the second element being the adjustment from February to March,
	  /// and so on to the 12th element being the adjustment from December to January.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray SeasonalityMonthOnMonth
	  {
		  get
		  {
			return seasonalityMonthOnMonth;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the shift type applied to the unadjusted value and the adjustment.
	  /// (value, seasonality) -> adjustmentType.applyShift(value, seasonality). </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ShiftType AdjustmentType
	  {
		  get
		  {
			return adjustmentType;
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
		  SeasonalityDefinition other = (SeasonalityDefinition) obj;
		  return JodaBeanUtils.equal(seasonalityMonthOnMonth, other.seasonalityMonthOnMonth) && JodaBeanUtils.equal(adjustmentType, other.adjustmentType);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(seasonalityMonthOnMonth);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(adjustmentType);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("SeasonalityDefinition{");
		buf.Append("seasonalityMonthOnMonth").Append('=').Append(seasonalityMonthOnMonth).Append(',').Append(' ');
		buf.Append("adjustmentType").Append('=').Append(JodaBeanUtils.ToString(adjustmentType));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SeasonalityDefinition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  seasonalityMonthOnMonth_Renamed = DirectMetaProperty.ofImmutable(this, "seasonalityMonthOnMonth", typeof(SeasonalityDefinition), typeof(DoubleArray));
			  adjustmentType_Renamed = DirectMetaProperty.ofImmutable(this, "adjustmentType", typeof(SeasonalityDefinition), typeof(ShiftType));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "seasonalityMonthOnMonth", "adjustmentType");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code seasonalityMonthOnMonth} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> seasonalityMonthOnMonth_Renamed;
		/// <summary>
		/// The meta-property for the {@code adjustmentType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ShiftType> adjustmentType_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "seasonalityMonthOnMonth", "adjustmentType");
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
			case -731183871: // seasonalityMonthOnMonth
			  return seasonalityMonthOnMonth_Renamed;
			case -1002343865: // adjustmentType
			  return adjustmentType_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends SeasonalityDefinition> builder()
		public override BeanBuilder<SeasonalityDefinition> builder()
		{
		  return new SeasonalityDefinition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SeasonalityDefinition);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code seasonalityMonthOnMonth} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> seasonalityMonthOnMonth()
		{
		  return seasonalityMonthOnMonth_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code adjustmentType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ShiftType> adjustmentType()
		{
		  return adjustmentType_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -731183871: // seasonalityMonthOnMonth
			  return ((SeasonalityDefinition) bean).SeasonalityMonthOnMonth;
			case -1002343865: // adjustmentType
			  return ((SeasonalityDefinition) bean).AdjustmentType;
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
	  /// The bean-builder for {@code SeasonalityDefinition}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<SeasonalityDefinition>
	  {

		internal DoubleArray seasonalityMonthOnMonth;
		internal ShiftType adjustmentType;

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
			case -731183871: // seasonalityMonthOnMonth
			  return seasonalityMonthOnMonth;
			case -1002343865: // adjustmentType
			  return adjustmentType;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -731183871: // seasonalityMonthOnMonth
			  this.seasonalityMonthOnMonth = (DoubleArray) newValue;
			  break;
			case -1002343865: // adjustmentType
			  this.adjustmentType = (ShiftType) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override SeasonalityDefinition build()
		{
		  return new SeasonalityDefinition(seasonalityMonthOnMonth, adjustmentType);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("SeasonalityDefinition.Builder{");
		  buf.Append("seasonalityMonthOnMonth").Append('=').Append(JodaBeanUtils.ToString(seasonalityMonthOnMonth)).Append(',').Append(' ');
		  buf.Append("adjustmentType").Append('=').Append(JodaBeanUtils.ToString(adjustmentType));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}