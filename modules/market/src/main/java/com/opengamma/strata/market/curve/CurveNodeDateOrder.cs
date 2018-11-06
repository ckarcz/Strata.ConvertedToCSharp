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
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// The date order rules to apply to a pair of curve nodes.
	/// <para>
	/// In any curve, two nodes may not have the same date. In addition, it is typically
	/// desirable to ensure that there is a minimum gap between two nodes, such as 7 days.
	/// An instance of {@code CurveNodeDateOrder} specifies the minimum gap and what to do if the clash occurs.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CurveNodeDateOrder implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CurveNodeDateOrder : ImmutableBean
	{

	  /// <summary>
	  /// The default instance, that throws an exception if the node is on the same date
	  /// or before another node.
	  /// </summary>
	  public static readonly CurveNodeDateOrder DEFAULT = new CurveNodeDateOrder(1, CurveNodeClashAction.EXCEPTION);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The minimum gap between two curve nodes, measured in calendar days.
	  /// A gap of one day is the smallest allowed.
	  /// A clash occurs if the period between the two nodes is less than the minimum.
	  /// The gap applies to the node before this one and the node after this one.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final int minGapInDays;
	  private readonly int minGapInDays;
	  /// <summary>
	  /// The action to perform if a clash occurs.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CurveNodeClashAction action;
	  private readonly CurveNodeClashAction action;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the minimum gap, allowing reordering flag and clash action.
	  /// </summary>
	  /// <param name="minGapInDays">  the minimum gap between this node and the previous node in days, one or greater </param>
	  /// <param name="action">  the action to perform if a clash occurs </param>
	  /// <returns> an instance specifying a fixed date </returns>
	  public static CurveNodeDateOrder of(int minGapInDays, CurveNodeClashAction action)
	  {
		return new CurveNodeDateOrder(minGapInDays, action);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (minGapInDays < 1)
		{
		  throw new System.ArgumentException(Messages.format("Minimum gap must be at least one day, but was {}", minGapInDays));
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurveNodeDateOrder}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CurveNodeDateOrder.Meta meta()
	  {
		return CurveNodeDateOrder.Meta.INSTANCE;
	  }

	  static CurveNodeDateOrder()
	  {
		MetaBean.register(CurveNodeDateOrder.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CurveNodeDateOrder(int minGapInDays, CurveNodeClashAction action)
	  {
		JodaBeanUtils.notNull(action, "action");
		this.minGapInDays = minGapInDays;
		this.action = action;
		validate();
	  }

	  public override CurveNodeDateOrder.Meta metaBean()
	  {
		return CurveNodeDateOrder.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the minimum gap between two curve nodes, measured in calendar days.
	  /// A gap of one day is the smallest allowed.
	  /// A clash occurs if the period between the two nodes is less than the minimum.
	  /// The gap applies to the node before this one and the node after this one. </summary>
	  /// <returns> the value of the property </returns>
	  public int MinGapInDays
	  {
		  get
		  {
			return minGapInDays;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the action to perform if a clash occurs. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveNodeClashAction Action
	  {
		  get
		  {
			return action;
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
		  CurveNodeDateOrder other = (CurveNodeDateOrder) obj;
		  return (minGapInDays == other.minGapInDays) && JodaBeanUtils.equal(action, other.action);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(minGapInDays);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(action);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("CurveNodeDateOrder{");
		buf.Append("minGapInDays").Append('=').Append(minGapInDays).Append(',').Append(' ');
		buf.Append("action").Append('=').Append(JodaBeanUtils.ToString(action));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurveNodeDateOrder}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  minGapInDays_Renamed = DirectMetaProperty.ofImmutable(this, "minGapInDays", typeof(CurveNodeDateOrder), Integer.TYPE);
			  action_Renamed = DirectMetaProperty.ofImmutable(this, "action", typeof(CurveNodeDateOrder), typeof(CurveNodeClashAction));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "minGapInDays", "action");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code minGapInDays} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> minGapInDays_Renamed;
		/// <summary>
		/// The meta-property for the {@code action} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveNodeClashAction> action_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "minGapInDays", "action");
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
			case 1925599072: // minGapInDays
			  return minGapInDays_Renamed;
			case -1422950858: // action
			  return action_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CurveNodeDateOrder> builder()
		public override BeanBuilder<CurveNodeDateOrder> builder()
		{
		  return new CurveNodeDateOrder.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CurveNodeDateOrder);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code minGapInDays} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> minGapInDays()
		{
		  return minGapInDays_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code action} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveNodeClashAction> action()
		{
		  return action_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1925599072: // minGapInDays
			  return ((CurveNodeDateOrder) bean).MinGapInDays;
			case -1422950858: // action
			  return ((CurveNodeDateOrder) bean).Action;
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
	  /// The bean-builder for {@code CurveNodeDateOrder}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CurveNodeDateOrder>
	  {

		internal int minGapInDays;
		internal CurveNodeClashAction action;

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
			case 1925599072: // minGapInDays
			  return minGapInDays;
			case -1422950858: // action
			  return action;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1925599072: // minGapInDays
			  this.minGapInDays = (int?) newValue.Value;
			  break;
			case -1422950858: // action
			  this.action = (CurveNodeClashAction) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CurveNodeDateOrder build()
		{
		  return new CurveNodeDateOrder(minGapInDays, action);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("CurveNodeDateOrder.Builder{");
		  buf.Append("minGapInDays").Append('=').Append(JodaBeanUtils.ToString(minGapInDays)).Append(',').Append(' ');
		  buf.Append("action").Append('=').Append(JodaBeanUtils.ToString(action));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}