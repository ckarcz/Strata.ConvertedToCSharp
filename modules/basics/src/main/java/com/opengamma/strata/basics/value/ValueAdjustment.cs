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

	/// <summary>
	/// An adjustment to a value, describing how to change one value into another.
	/// <para>
	/// A base value, represented as a {@code double}, can be transformed into another value
	/// by specifying the result (absolute) or the calculation (relative).
	/// </para>
	/// <para>
	/// <table class="border 1px solid black;border-collapse:collapse">
	/// <tr>
	/// <th>Type</th><th>baseValue</th><th>modifyingValue</th><th>Calculation</th>
	/// </tr><tr>
	/// <td>Replace</td><td>200</td><td>220</td><td>{@code result = modifyingValue = 220}</td>
	/// </tr><tr>
	/// <td>DeltaAmount</td><td>200</td><td>20</td><td>{@code result = baseValue + modifyingValue = (200 + 20) = 220}</td>
	/// </tr><tr>
	/// <td>DeltaMultiplier</td><td>200</td><td>0.1</td>
	/// <td>{@code result = baseValue + baseValue * modifyingValue = (200 + 200 * 0.1) = 220}</td>
	/// </tr><tr>
	/// <td>Multiplier</td><td>200</td><td>1.1</td><td>{@code result = baseValue * modifyingValue = (200 * 1.1) = 220}</td>
	/// </tr>
	/// </table>
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ValueAdjustment implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ValueAdjustment : ImmutableBean
	{

	  /// <summary>
	  /// An instance that makes no adjustment to the value.
	  /// </summary>
	  public static readonly ValueAdjustment NONE = ValueAdjustment.ofDeltaAmount(0);

	  /// <summary>
	  /// The value used to modify the base value.
	  /// This value is given meaning by the associated type.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double modifyingValue;
	  private readonly double modifyingValue;
	  /// <summary>
	  /// The type of adjustment to make.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ValueAdjustmentType type;
	  private readonly ValueAdjustmentType type;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that replaces the base value.
	  /// <para>
	  /// The base value is ignored when calculating the result.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="replacementValue">  the replacement value to use as the result of the adjustment </param>
	  /// <returns> the adjustment, capturing the replacement value </returns>
	  public static ValueAdjustment ofReplace(double replacementValue)
	  {
		return new ValueAdjustment(replacementValue, ValueAdjustmentType.REPLACE);
	  }

	  /// <summary>
	  /// Obtains an instance specifying an amount to add to the base value.
	  /// <para>
	  /// The result will be {@code (baseValue + deltaAmount)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="deltaAmount">  the amount to be added to the base value </param>
	  /// <returns> the adjustment, capturing the delta amount </returns>
	  public static ValueAdjustment ofDeltaAmount(double deltaAmount)
	  {
		return new ValueAdjustment(deltaAmount, ValueAdjustmentType.DELTA_AMOUNT);
	  }

	  /// <summary>
	  /// Obtains an instance specifying a multiplication factor, adding it to the base value.
	  /// <para>
	  /// The result will be {@code (baseValue + baseValue * modifyingValue)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="deltaMultiplier">  the multiplication factor to apply to the base amount
	  ///   with the result added to the base amount </param>
	  /// <returns> the adjustment, capturing the delta multiplier </returns>
	  public static ValueAdjustment ofDeltaMultiplier(double deltaMultiplier)
	  {
		return new ValueAdjustment(deltaMultiplier, ValueAdjustmentType.DELTA_MULTIPLIER);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance specifying a multiplication factor to apply to the base value.
	  /// <para>
	  /// The result will be {@code (baseValue * modifyingValue)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="multiplier">  the multiplication factor to apply to the base amount </param>
	  /// <returns> the adjustment </returns>
	  public static ValueAdjustment ofMultiplier(double multiplier)
	  {
		return new ValueAdjustment(multiplier, ValueAdjustmentType.MULTIPLIER);
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("ValueAdjustment[result = ");
		switch (type.innerEnumValue)
		{
		  case com.opengamma.strata.basics.value.ValueAdjustmentType.InnerEnum.DELTA_AMOUNT:
			if (this == NONE)
			{
			  buf.Append("input");
			}
			else
			{
			  buf.Append("input + ").Append(modifyingValue);
			}
			break;
		  case com.opengamma.strata.basics.value.ValueAdjustmentType.InnerEnum.DELTA_MULTIPLIER:
			buf.Append("input + input * ").Append(modifyingValue);
			break;
		  case com.opengamma.strata.basics.value.ValueAdjustmentType.InnerEnum.MULTIPLIER:
			buf.Append("input * ").Append(modifyingValue);
			break;
		  case com.opengamma.strata.basics.value.ValueAdjustmentType.InnerEnum.REPLACE:
		  default:
			buf.Append(modifyingValue);
			break;
		}
		buf.Append(']');
		return buf.ToString();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adjusts the base value based on the criteria of this adjustment.
	  /// <para>
	  /// For example, if this adjustment represents a 10% decrease, then the
	  /// result will be the base value minus 10%.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseValue">  the base, or previous, value to be adjusted </param>
	  /// <returns> the calculated result </returns>
	  public double adjust(double baseValue)
	  {
		return type.adjust(baseValue, modifyingValue);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ValueAdjustment}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ValueAdjustment.Meta meta()
	  {
		return ValueAdjustment.Meta.INSTANCE;
	  }

	  static ValueAdjustment()
	  {
		MetaBean.register(ValueAdjustment.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ValueAdjustment(double modifyingValue, ValueAdjustmentType type)
	  {
		JodaBeanUtils.notNull(type, "type");
		this.modifyingValue = modifyingValue;
		this.type = type;
	  }

	  public override ValueAdjustment.Meta metaBean()
	  {
		return ValueAdjustment.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the value used to modify the base value.
	  /// This value is given meaning by the associated type. </summary>
	  /// <returns> the value of the property </returns>
	  public double ModifyingValue
	  {
		  get
		  {
			return modifyingValue;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of adjustment to make. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueAdjustmentType Type
	  {
		  get
		  {
			return type;
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
		  ValueAdjustment other = (ValueAdjustment) obj;
		  return JodaBeanUtils.equal(modifyingValue, other.modifyingValue) && JodaBeanUtils.equal(type, other.type);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(modifyingValue);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(type);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ValueAdjustment}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  modifyingValue_Renamed = DirectMetaProperty.ofImmutable(this, "modifyingValue", typeof(ValueAdjustment), Double.TYPE);
			  type_Renamed = DirectMetaProperty.ofImmutable(this, "type", typeof(ValueAdjustment), typeof(ValueAdjustmentType));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "modifyingValue", "type");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code modifyingValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> modifyingValue_Renamed;
		/// <summary>
		/// The meta-property for the {@code type} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueAdjustmentType> type_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "modifyingValue", "type");
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
			case 503432553: // modifyingValue
			  return modifyingValue_Renamed;
			case 3575610: // type
			  return type_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ValueAdjustment> builder()
		public override BeanBuilder<ValueAdjustment> builder()
		{
		  return new ValueAdjustment.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ValueAdjustment);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code modifyingValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> modifyingValue()
		{
		  return modifyingValue_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code type} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueAdjustmentType> type()
		{
		  return type_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 503432553: // modifyingValue
			  return ((ValueAdjustment) bean).ModifyingValue;
			case 3575610: // type
			  return ((ValueAdjustment) bean).Type;
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
	  /// The bean-builder for {@code ValueAdjustment}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ValueAdjustment>
	  {

		internal double modifyingValue;
		internal ValueAdjustmentType type;

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
			case 503432553: // modifyingValue
			  return modifyingValue;
			case 3575610: // type
			  return type;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 503432553: // modifyingValue
			  this.modifyingValue = (double?) newValue.Value;
			  break;
			case 3575610: // type
			  this.type = (ValueAdjustmentType) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ValueAdjustment build()
		{
		  return new ValueAdjustment(modifyingValue, type);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ValueAdjustment.Builder{");
		  buf.Append("modifyingValue").Append('=').Append(JodaBeanUtils.ToString(modifyingValue)).Append(',').Append(' ');
		  buf.Append("type").Append('=').Append(JodaBeanUtils.ToString(type));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}