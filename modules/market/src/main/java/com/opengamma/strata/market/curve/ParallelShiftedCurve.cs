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

	using LabelParameterMetadata = com.opengamma.strata.market.param.LabelParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// A curve with a parallel shift applied to its y-values.
	/// <para>
	/// This class decorates another curve and applies an adjustment to the y-values when they are queried.
	/// The shift is either absolute or relative.
	/// </para>
	/// <para>
	/// When the shift is absolute the shift amount is added to the y-value.
	/// </para>
	/// <para>
	/// When the shift is relative the y-value is scaled by the shift amount.
	/// The shift amount is interpreted as a percentage.
	/// For example, a shift amount of 0.1 is a shift of +10% which multiplies the value by 1.1.
	/// A shift amount of -0.2 is a shift of -20% which multiplies the value by 0.8.
	/// </para>
	/// <para>
	/// The parameters consist of the parameters of the underlying curve, followed by the shift.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ParallelShiftedCurve implements Curve, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ParallelShiftedCurve : Curve, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Curve underlyingCurve;
		private readonly Curve underlyingCurve;
	  /// <summary>
	  /// The type of shift to apply to the y-values of the curve.
	  /// The amount of the shift is determined by {@code #getShiftAmount()}.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ShiftType shiftType;
	  private readonly ShiftType shiftType;
	  /// <summary>
	  /// The amount by which y-values are shifted.
	  /// The meaning of this amount is determined by {@code #getShiftType()}.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final double shiftAmount;
	  private readonly double shiftAmount;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a curve based on an underlying curve with a fixed amount added to the Y values.
	  /// </summary>
	  /// <param name="curve">  the underlying curve </param>
	  /// <param name="shiftAmount">  the amount added to the Y values of the curve </param>
	  /// <returns> a curve based on an underlying curve with a fixed amount added to the Y values. </returns>
	  public static ParallelShiftedCurve absolute(Curve curve, double shiftAmount)
	  {
		return new ParallelShiftedCurve(curve, ShiftType.ABSOLUTE, shiftAmount);
	  }

	  /// <summary>
	  /// Returns a curve based on an underlying curve with a scaling applied to the Y values.
	  /// <para>
	  /// The shift amount is interpreted as a percentage. For example, a shift amount of 0.1 is a
	  /// shift of +10% which multiplies the value by 1.1. A shift amount of -0.2 is a shift of -20%
	  /// which multiplies the value by 0.8
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curve">  the underlying curve </param>
	  /// <param name="shiftAmount">  the percentage by which the Y values are scaled </param>
	  /// <returns> a curve based on an underlying curve with a scaling applied to the Y values. </returns>
	  public static ParallelShiftedCurve relative(Curve curve, double shiftAmount)
	  {
		return new ParallelShiftedCurve(curve, ShiftType.RELATIVE, shiftAmount);
	  }

	  /// <summary>
	  /// Returns a curve based on an underlying curve with a parallel shift applied to the Y values.
	  /// </summary>
	  /// <param name="curve">  the underlying curve </param>
	  /// <param name="shiftType">  the type of shift which specifies how the shift amount is applied to the Y values </param>
	  /// <param name="shiftAmount">  the magnitude of the shift </param>
	  /// <returns> a curve based on an underlying curve with a parallel shift applied to the Y values </returns>
	  public static ParallelShiftedCurve of(Curve curve, ShiftType shiftType, double shiftAmount)
	  {
		return new ParallelShiftedCurve(curve, shiftType, shiftAmount);
	  }

	  //-------------------------------------------------------------------------
	  public CurveMetadata Metadata
	  {
		  get
		  {
			return underlyingCurve.Metadata;
		  }
	  }

	  public ParallelShiftedCurve withMetadata(CurveMetadata metadata)
	  {
		return new ParallelShiftedCurve(underlyingCurve.withMetadata(metadata), shiftType, shiftAmount);
	  }

	  public override CurveName Name
	  {
		  get
		  {
			return underlyingCurve.Name;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return underlyingCurve.ParameterCount + 1;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		if (parameterIndex == underlyingCurve.ParameterCount)
		{
		  return shiftAmount;
		}
		return underlyingCurve.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		if (parameterIndex == underlyingCurve.ParameterCount)
		{
		  return LabelParameterMetadata.of(shiftType + "Shift");
		}
		return underlyingCurve.getParameterMetadata(parameterIndex);
	  }

	  public ParallelShiftedCurve withParameter(int parameterIndex, double newValue)
	  {
		if (parameterIndex == underlyingCurve.ParameterCount)
		{
		  return new ParallelShiftedCurve(underlyingCurve, shiftType, newValue);
		}
		return new ParallelShiftedCurve(underlyingCurve.withParameter(parameterIndex, newValue), shiftType, shiftAmount);
	  }

	  public override ParallelShiftedCurve withPerturbation(ParameterPerturbation perturbation)
	  {
		Curve bumpedCurve = underlyingCurve.withPerturbation(perturbation);
		int shiftIndex = underlyingCurve.ParameterCount;
		double bumpedShift = perturbation(shiftIndex, shiftAmount, getParameterMetadata(shiftIndex));
		return new ParallelShiftedCurve(bumpedCurve, shiftType, bumpedShift);
	  }

	  //-------------------------------------------------------------------------
	  public double yValue(double x)
	  {
		return shiftType.applyShift(underlyingCurve.yValue(x), shiftAmount);
	  }

	  public UnitParameterSensitivity yValueParameterSensitivity(double x)
	  {
		return underlyingCurve.yValueParameterSensitivity(x);
	  }

	  public double firstDerivative(double x)
	  {
		double firstDerivative = underlyingCurve.firstDerivative(x);
		switch (shiftType.innerEnumValue)
		{
		  case ShiftType.InnerEnum.ABSOLUTE:
			// If all Y values have been shifted the same amount the derivative is unaffected
			return firstDerivative;
		  case ShiftType.InnerEnum.RELATIVE:
			// If all Y values have been scaled by the same factor the first derivative is scaled in the same way
			return shiftType.applyShift(firstDerivative, shiftAmount);
		  default:
			throw new System.ArgumentException("Unsupported shift type " + shiftType);
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ParallelShiftedCurve}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ParallelShiftedCurve.Meta meta()
	  {
		return ParallelShiftedCurve.Meta.INSTANCE;
	  }

	  static ParallelShiftedCurve()
	  {
		MetaBean.register(ParallelShiftedCurve.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ParallelShiftedCurve(Curve underlyingCurve, ShiftType shiftType, double shiftAmount)
	  {
		JodaBeanUtils.notNull(underlyingCurve, "underlyingCurve");
		JodaBeanUtils.notNull(shiftType, "shiftType");
		JodaBeanUtils.notNull(shiftAmount, "shiftAmount");
		this.underlyingCurve = underlyingCurve;
		this.shiftType = shiftType;
		this.shiftAmount = shiftAmount;
	  }

	  public override ParallelShiftedCurve.Meta metaBean()
	  {
		return ParallelShiftedCurve.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve UnderlyingCurve
	  {
		  get
		  {
			return underlyingCurve;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of shift to apply to the y-values of the curve.
	  /// The amount of the shift is determined by {@code #getShiftAmount()}. </summary>
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
	  /// Gets the amount by which y-values are shifted.
	  /// The meaning of this amount is determined by {@code #getShiftType()}. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public double ShiftAmount
	  {
		  get
		  {
			return shiftAmount;
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
		  ParallelShiftedCurve other = (ParallelShiftedCurve) obj;
		  return JodaBeanUtils.equal(underlyingCurve, other.underlyingCurve) && JodaBeanUtils.equal(shiftType, other.shiftType) && JodaBeanUtils.equal(shiftAmount, other.shiftAmount);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlyingCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftAmount);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("ParallelShiftedCurve{");
		buf.Append("underlyingCurve").Append('=').Append(underlyingCurve).Append(',').Append(' ');
		buf.Append("shiftType").Append('=').Append(shiftType).Append(',').Append(' ');
		buf.Append("shiftAmount").Append('=').Append(JodaBeanUtils.ToString(shiftAmount));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ParallelShiftedCurve}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  underlyingCurve_Renamed = DirectMetaProperty.ofImmutable(this, "underlyingCurve", typeof(ParallelShiftedCurve), typeof(Curve));
			  shiftType_Renamed = DirectMetaProperty.ofImmutable(this, "shiftType", typeof(ParallelShiftedCurve), typeof(ShiftType));
			  shiftAmount_Renamed = DirectMetaProperty.ofImmutable(this, "shiftAmount", typeof(ParallelShiftedCurve), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "underlyingCurve", "shiftType", "shiftAmount");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code underlyingCurve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> underlyingCurve_Renamed;
		/// <summary>
		/// The meta-property for the {@code shiftType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ShiftType> shiftType_Renamed;
		/// <summary>
		/// The meta-property for the {@code shiftAmount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> shiftAmount_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "underlyingCurve", "shiftType", "shiftAmount");
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
			case -839394414: // underlyingCurve
			  return underlyingCurve_Renamed;
			case 893345500: // shiftType
			  return shiftType_Renamed;
			case -1043480710: // shiftAmount
			  return shiftAmount_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ParallelShiftedCurve> builder()
		public override BeanBuilder<ParallelShiftedCurve> builder()
		{
		  return new ParallelShiftedCurve.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ParallelShiftedCurve);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code underlyingCurve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Curve> underlyingCurve()
		{
		  return underlyingCurve_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code shiftType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ShiftType> shiftType()
		{
		  return shiftType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code shiftAmount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> shiftAmount()
		{
		  return shiftAmount_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -839394414: // underlyingCurve
			  return ((ParallelShiftedCurve) bean).UnderlyingCurve;
			case 893345500: // shiftType
			  return ((ParallelShiftedCurve) bean).ShiftType;
			case -1043480710: // shiftAmount
			  return ((ParallelShiftedCurve) bean).ShiftAmount;
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
	  /// The bean-builder for {@code ParallelShiftedCurve}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ParallelShiftedCurve>
	  {

		internal Curve underlyingCurve;
		internal ShiftType shiftType;
		internal double shiftAmount;

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
			case -839394414: // underlyingCurve
			  return underlyingCurve;
			case 893345500: // shiftType
			  return shiftType;
			case -1043480710: // shiftAmount
			  return shiftAmount;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -839394414: // underlyingCurve
			  this.underlyingCurve = (Curve) newValue;
			  break;
			case 893345500: // shiftType
			  this.shiftType = (ShiftType) newValue;
			  break;
			case -1043480710: // shiftAmount
			  this.shiftAmount = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ParallelShiftedCurve build()
		{
		  return new ParallelShiftedCurve(underlyingCurve, shiftType, shiftAmount);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("ParallelShiftedCurve.Builder{");
		  buf.Append("underlyingCurve").Append('=').Append(JodaBeanUtils.ToString(underlyingCurve)).Append(',').Append(' ');
		  buf.Append("shiftType").Append('=').Append(JodaBeanUtils.ToString(shiftType)).Append(',').Append(' ');
		  buf.Append("shiftAmount").Append('=').Append(JodaBeanUtils.ToString(shiftAmount));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}