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

	using Preconditions = com.google.common.@base.Preconditions;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// A curve based on a single constant value.
	/// <para>
	/// This class defines a curve in terms of a single parameter, the constant value.
	/// When queried, <seealso cref="#yValue(double)"/> always returns the constant value.
	/// The sensitivity is 1 and the first derivative is 0.
	/// </para>
	/// <para>
	/// The curve has one parameter, the value of the constant.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ConstantCurve implements Curve, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ConstantCurve : Curve, ImmutableBean
	{

	  /// <summary>
	  /// Sensitivity does not vary.
	  /// </summary>
	  private static readonly DoubleArray SENSITIVITY = DoubleArray.of(1d);

	  /// <summary>
	  /// The curve metadata.
	  /// <para>
	  /// The metadata will not normally have parameter metadata.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CurveMetadata metadata;
	  private readonly CurveMetadata metadata;
	  /// <summary>
	  /// The single y-value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double yValue;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly double yValue_Renamed;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a constant curve with a specific value.
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="yValue">  the constant y-value </param>
	  /// <returns> the curve </returns>
	  public static ConstantCurve of(string name, double yValue)
	  {
		return of(CurveName.of(name), yValue);
	  }

	  /// <summary>
	  /// Creates a constant curve with a specific value.
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="yValue">  the constant y-value </param>
	  /// <returns> the curve </returns>
	  public static ConstantCurve of(CurveName name, double yValue)
	  {
		return new ConstantCurve(DefaultCurveMetadata.of(name), yValue);
	  }

	  /// <summary>
	  /// Creates a constant curve with a specific value.
	  /// </summary>
	  /// <param name="metadata">  the curve metadata </param>
	  /// <param name="yValue">  the constant y-value </param>
	  /// <returns> the curve </returns>
	  public static ConstantCurve of(CurveMetadata metadata, double yValue)
	  {
		return new ConstantCurve(metadata, yValue);
	  }

	  //-------------------------------------------------------------------------
	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new ConstantCurve(metadata, yValue_Renamed);
	  }

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return 1;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		Preconditions.checkElementIndex(parameterIndex, 1);
		return yValue_Renamed;
	  }

	  public ConstantCurve withParameter(int parameterIndex, double newValue)
	  {
		Preconditions.checkElementIndex(parameterIndex, 1);
		return new ConstantCurve(metadata, newValue);
	  }

	  public override ConstantCurve withPerturbation(ParameterPerturbation perturbation)
	  {
		return new ConstantCurve(metadata, perturbation(0, yValue_Renamed, getParameterMetadata(0)));
	  }

	  //-------------------------------------------------------------------------
	  public double yValue(double x)
	  {
		return yValue_Renamed;
	  }

	  public UnitParameterSensitivity yValueParameterSensitivity(double x)
	  {
		ImmutableList<ParameterMetadata> paramMeta = ImmutableList.of(getParameterMetadata(0));
		return UnitParameterSensitivity.of(metadata.CurveName, paramMeta, SENSITIVITY);
	  }

	  public double firstDerivative(double x)
	  {
		return 0d;
	  }

	  //-------------------------------------------------------------------------
	  public ConstantCurve withMetadata(CurveMetadata metadata)
	  {
		return new ConstantCurve(metadata.withParameterMetadata(null), yValue_Renamed);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ConstantCurve}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ConstantCurve.Meta meta()
	  {
		return ConstantCurve.Meta.INSTANCE;
	  }

	  static ConstantCurve()
	  {
		MetaBean.register(ConstantCurve.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ConstantCurve(CurveMetadata metadata, double yValue)
	  {
		JodaBeanUtils.notNull(metadata, "metadata");
		this.metadata = metadata;
		this.yValue_Renamed = yValue;
	  }

	  public override ConstantCurve.Meta metaBean()
	  {
		return ConstantCurve.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve metadata.
	  /// <para>
	  /// The metadata will not normally have parameter metadata.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveMetadata Metadata
	  {
		  get
		  {
			return metadata;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the single y-value. </summary>
	  /// <returns> the value of the property </returns>
	  public double YValue
	  {
		  get
		  {
			return yValue_Renamed;
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
		  ConstantCurve other = (ConstantCurve) obj;
		  return JodaBeanUtils.equal(metadata, other.metadata) && JodaBeanUtils.equal(yValue_Renamed, other.yValue_Renamed);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(metadata);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yValue_Renamed);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("ConstantCurve{");
		buf.Append("metadata").Append('=').Append(metadata).Append(',').Append(' ');
		buf.Append("yValue").Append('=').Append(JodaBeanUtils.ToString(yValue_Renamed));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ConstantCurve}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  metadata_Renamed = DirectMetaProperty.ofImmutable(this, "metadata", typeof(ConstantCurve), typeof(CurveMetadata));
			  yValue_Renamed = DirectMetaProperty.ofImmutable(this, "yValue", typeof(ConstantCurve), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "metadata", "yValue");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code metadata} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveMetadata> metadata_Renamed;
		/// <summary>
		/// The meta-property for the {@code yValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> yValue_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "metadata", "yValue");
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
			case -450004177: // metadata
			  return metadata_Renamed;
			case -748419976: // yValue
			  return yValue_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ConstantCurve> builder()
		public override BeanBuilder<ConstantCurve> builder()
		{
		  return new ConstantCurve.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ConstantCurve);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code metadata} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveMetadata> metadata()
		{
		  return metadata_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> yValue()
		{
		  return yValue_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  return ((ConstantCurve) bean).Metadata;
			case -748419976: // yValue
			  return ((ConstantCurve) bean).YValue;
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
	  /// The bean-builder for {@code ConstantCurve}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ConstantCurve>
	  {

		internal CurveMetadata metadata;
		internal double yValue;

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
			case -450004177: // metadata
			  return metadata;
			case -748419976: // yValue
			  return yValue;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  this.metadata = (CurveMetadata) newValue;
			  break;
			case -748419976: // yValue
			  this.yValue = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ConstantCurve build()
		{
		  return new ConstantCurve(metadata, yValue);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ConstantCurve.Builder{");
		  buf.Append("metadata").Append('=').Append(JodaBeanUtils.ToString(metadata)).Append(',').Append(' ');
		  buf.Append("yValue").Append('=').Append(JodaBeanUtils.ToString(yValue));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}