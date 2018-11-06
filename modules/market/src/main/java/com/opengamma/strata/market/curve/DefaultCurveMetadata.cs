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
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Messages = com.opengamma.strata.collect.Messages;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Default metadata for a curve.
	/// <para>
	/// This implementation of <seealso cref="CurveMetadata"/> provides the curve name and nodes.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class DefaultCurveMetadata implements CurveMetadata, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DefaultCurveMetadata : CurveMetadata, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CurveName curveName;
		private readonly CurveName curveName;
	  /// <summary>
	  /// The x-value type, providing meaning to the x-values of the curve.
	  /// <para>
	  /// This type provides meaning to the x-values. For example, the x-value might
	  /// represent a year fraction, as represented using <seealso cref="ValueType#YEAR_FRACTION"/>.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.market.ValueType xValueType;
	  private readonly ValueType xValueType;
	  /// <summary>
	  /// The y-value type, providing meaning to the y-values of the curve.
	  /// <para>
	  /// This type provides meaning to the y-values. For example, the y-value might
	  /// represent a zero rate, as represented using <seealso cref="ValueType#ZERO_RATE"/>.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.market.ValueType yValueType;
	  private readonly ValueType yValueType;
	  /// <summary>
	  /// The additional curve information.
	  /// <para>
	  /// This stores additional information for the curve.
	  /// </para>
	  /// <para>
	  /// The most common information is the <seealso cref="CurveInfoType#DAY_COUNT day count"/>
	  /// and <seealso cref="CurveInfoType#JACOBIAN curve calibration Jacobian"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<CurveInfoType<?>, Object> info;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<CurveInfoType<object>, object> info;
	  /// <summary>
	  /// The metadata about the parameters.
	  /// <para>
	  /// If present, the parameter metadata will match the number of parameters on the curve.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional", overrideGet = true, type = "List<>", builderType = "List<? extends ParameterMetadata>") private final com.google.common.collect.ImmutableList<com.opengamma.strata.market.param.ParameterMetadata> parameterMetadata;
	  private readonly ImmutableList<ParameterMetadata> parameterMetadata;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the metadata.
	  /// <para>
	  /// No information will be available for the x-values, y-values or parameters.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <returns> the metadata </returns>
	  public static DefaultCurveMetadata of(string name)
	  {
		return of(CurveName.of(name));
	  }

	  /// <summary>
	  /// Creates the metadata.
	  /// <para>
	  /// No information will be available for the x-values, y-values or parameters.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <returns> the metadata </returns>
	  public static DefaultCurveMetadata of(CurveName name)
	  {
		return new DefaultCurveMetadata(name, ValueType.UNKNOWN, ValueType.UNKNOWN, ImmutableMap.of(), null);
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean.
	  /// </summary>
	  /// <returns> the builder, not null </returns>
	  public static DefaultCurveMetadataBuilder builder()
	  {
		return new DefaultCurveMetadataBuilder();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.xValueType = ValueType.UNKNOWN;
		builder.yValueType = ValueType.UNKNOWN;
	  }

	  //-------------------------------------------------------------------------
	  public override T getInfo<T>(CurveInfoType<T> type)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") T value = (T) info.get(type);
		  T value = (T) info.get(type);
		if (value == null)
		{
		  throw new System.ArgumentException(Messages.format("Curve info not found for type '{}'", type));
		}
		return value;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> java.util.Optional<T> findInfo(CurveInfoType<T> type)
	  public Optional<T> findInfo<T>(CurveInfoType<T> type)
	  {
		return Optional.ofNullable((T) info.get(type));
	  }

	  //-------------------------------------------------------------------------
	  public DefaultCurveMetadata withInfo<T>(CurveInfoType<T> type, T value)
	  {
		return toBuilder().addInfo(type, value).build();
	  }

	  public DefaultCurveMetadata withParameterMetadata<T1>(IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
	  {
		if (parameterMetadata == null)
		{
		  return this.parameterMetadata != null ? toBuilder().clearParameterMetadata().build() : this;
		}
		return toBuilder().parameterMetadata(parameterMetadata).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a mutable builder initialized with the state of this bean.
	  /// </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public DefaultCurveMetadataBuilder toBuilder()
	  {
		return new DefaultCurveMetadataBuilder(this);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultCurveMetadata}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DefaultCurveMetadata.Meta meta()
	  {
		return DefaultCurveMetadata.Meta.INSTANCE;
	  }

	  static DefaultCurveMetadata()
	  {
		MetaBean.register(DefaultCurveMetadata.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="curveName">  the value of the property, not null </param>
	  /// <param name="xValueType">  the value of the property, not null </param>
	  /// <param name="yValueType">  the value of the property, not null </param>
	  /// <param name="info">  the value of the property, not null </param>
	  /// <param name="parameterMetadata">  the value of the property </param>
	  internal DefaultCurveMetadata<T1, T2>(CurveName curveName, ValueType xValueType, ValueType yValueType, IDictionary<T1> info, IList<T2> parameterMetadata) where T2 : com.opengamma.strata.market.param.ParameterMetadata
	  {
		JodaBeanUtils.notNull(curveName, "curveName");
		JodaBeanUtils.notNull(xValueType, "xValueType");
		JodaBeanUtils.notNull(yValueType, "yValueType");
		JodaBeanUtils.notNull(info, "info");
		this.curveName = curveName;
		this.xValueType = xValueType;
		this.yValueType = yValueType;
		this.info = ImmutableMap.copyOf(info);
		this.parameterMetadata = (parameterMetadata != null ? ImmutableList.copyOf(parameterMetadata) : null);
	  }

	  public override DefaultCurveMetadata.Meta metaBean()
	  {
		return DefaultCurveMetadata.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveName CurveName
	  {
		  get
		  {
			return curveName;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the x-value type, providing meaning to the x-values of the curve.
	  /// <para>
	  /// This type provides meaning to the x-values. For example, the x-value might
	  /// represent a year fraction, as represented using <seealso cref="ValueType#YEAR_FRACTION"/>.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueType XValueType
	  {
		  get
		  {
			return xValueType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the y-value type, providing meaning to the y-values of the curve.
	  /// <para>
	  /// This type provides meaning to the y-values. For example, the y-value might
	  /// represent a zero rate, as represented using <seealso cref="ValueType#ZERO_RATE"/>.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueType YValueType
	  {
		  get
		  {
			return yValueType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional curve information.
	  /// <para>
	  /// This stores additional information for the curve.
	  /// </para>
	  /// <para>
	  /// The most common information is the <seealso cref="CurveInfoType#DAY_COUNT day count"/>
	  /// and <seealso cref="CurveInfoType#JACOBIAN curve calibration Jacobian"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<CurveInfoType<?>, Object> getInfo()
	  public ImmutableMap<CurveInfoType<object>, object> Info
	  {
		  get
		  {
			return info;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the metadata about the parameters.
	  /// <para>
	  /// If present, the parameter metadata will match the number of parameters on the curve.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<IList<ParameterMetadata>> ParameterMetadata
	  {
		  get
		  {
			return Optional.ofNullable(parameterMetadata);
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
		  DefaultCurveMetadata other = (DefaultCurveMetadata) obj;
		  return JodaBeanUtils.equal(curveName, other.curveName) && JodaBeanUtils.equal(xValueType, other.xValueType) && JodaBeanUtils.equal(yValueType, other.yValueType) && JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(parameterMetadata, other.parameterMetadata);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curveName);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(xValueType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yValueType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameterMetadata);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("DefaultCurveMetadata{");
		buf.Append("curveName").Append('=').Append(curveName).Append(',').Append(' ');
		buf.Append("xValueType").Append('=').Append(xValueType).Append(',').Append(' ');
		buf.Append("yValueType").Append('=').Append(yValueType).Append(',').Append(' ');
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("parameterMetadata").Append('=').Append(JodaBeanUtils.ToString(parameterMetadata));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultCurveMetadata}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  curveName_Renamed = DirectMetaProperty.ofImmutable(this, "curveName", typeof(DefaultCurveMetadata), typeof(CurveName));
			  xValueType_Renamed = DirectMetaProperty.ofImmutable(this, "xValueType", typeof(DefaultCurveMetadata), typeof(ValueType));
			  yValueType_Renamed = DirectMetaProperty.ofImmutable(this, "yValueType", typeof(DefaultCurveMetadata), typeof(ValueType));
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(DefaultCurveMetadata), (Type) typeof(ImmutableMap));
			  parameterMetadata_Renamed = DirectMetaProperty.ofImmutable(this, "parameterMetadata", typeof(DefaultCurveMetadata), (Type) typeof(System.Collections.IList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "curveName", "xValueType", "yValueType", "info", "parameterMetadata");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code curveName} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveName> curveName_Renamed;
		/// <summary>
		/// The meta-property for the {@code xValueType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueType> xValueType_Renamed;
		/// <summary>
		/// The meta-property for the {@code yValueType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueType> yValueType_Renamed;
		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<CurveInfoType<?>, Object>> info = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "info", DefaultCurveMetadata.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<CurveInfoType<object>, object>> info_Renamed;
		/// <summary>
		/// The meta-property for the {@code parameterMetadata} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<java.util.List<com.opengamma.strata.market.param.ParameterMetadata>> parameterMetadata = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "parameterMetadata", DefaultCurveMetadata.class, (Class) java.util.List.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IList<ParameterMetadata>> parameterMetadata_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "curveName", "xValueType", "yValueType", "info", "parameterMetadata");
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
			case 771153946: // curveName
			  return curveName_Renamed;
			case -868509005: // xValueType
			  return xValueType_Renamed;
			case -1065022510: // yValueType
			  return yValueType_Renamed;
			case 3237038: // info
			  return info_Renamed;
			case -1169106440: // parameterMetadata
			  return parameterMetadata_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends DefaultCurveMetadata> builder()
		public override BeanBuilder<DefaultCurveMetadata> builder()
		{
		  return new DefaultCurveMetadata.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DefaultCurveMetadata);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code curveName} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveName> curveName()
		{
		  return curveName_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code xValueType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueType> xValueType()
		{
		  return xValueType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yValueType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueType> yValueType()
		{
		  return yValueType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code info} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<CurveInfoType<?>, Object>> info()
		public MetaProperty<ImmutableMap<CurveInfoType<object>, object>> info()
		{
		  return info_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code parameterMetadata} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IList<ParameterMetadata>> parameterMetadata()
		{
		  return parameterMetadata_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 771153946: // curveName
			  return ((DefaultCurveMetadata) bean).CurveName;
			case -868509005: // xValueType
			  return ((DefaultCurveMetadata) bean).XValueType;
			case -1065022510: // yValueType
			  return ((DefaultCurveMetadata) bean).YValueType;
			case 3237038: // info
			  return ((DefaultCurveMetadata) bean).Info;
			case -1169106440: // parameterMetadata
			  return ((DefaultCurveMetadata) bean).parameterMetadata;
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
	  /// The bean-builder for {@code DefaultCurveMetadata}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<DefaultCurveMetadata>
	  {

		internal CurveName curveName;
		internal ValueType xValueType;
		internal ValueType yValueType;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<CurveInfoType<?>, Object> info = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<CurveInfoType<object>, object> info = ImmutableMap.of();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends com.opengamma.strata.market.param.ParameterMetadata> parameterMetadata;
		internal IList<ParameterMetadata> parameterMetadata;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 771153946: // curveName
			  return curveName;
			case -868509005: // xValueType
			  return xValueType;
			case -1065022510: // yValueType
			  return yValueType;
			case 3237038: // info
			  return info;
			case -1169106440: // parameterMetadata
			  return parameterMetadata;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 771153946: // curveName
			  this.curveName = (CurveName) newValue;
			  break;
			case -868509005: // xValueType
			  this.xValueType = (ValueType) newValue;
			  break;
			case -1065022510: // yValueType
			  this.yValueType = (ValueType) newValue;
			  break;
			case 3237038: // info
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.info = (java.util.Map<CurveInfoType<?>, Object>) newValue;
			  this.info = (IDictionary<CurveInfoType<object>, object>) newValue;
			  break;
			case -1169106440: // parameterMetadata
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.parameterMetadata = (java.util.List<? extends com.opengamma.strata.market.param.ParameterMetadata>) newValue;
			  this.parameterMetadata = (IList<ParameterMetadata>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override DefaultCurveMetadata build()
		{
		  return new DefaultCurveMetadata(curveName, xValueType, yValueType, info, parameterMetadata);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("DefaultCurveMetadata.Builder{");
		  buf.Append("curveName").Append('=').Append(JodaBeanUtils.ToString(curveName)).Append(',').Append(' ');
		  buf.Append("xValueType").Append('=').Append(JodaBeanUtils.ToString(xValueType)).Append(',').Append(' ');
		  buf.Append("yValueType").Append('=').Append(JodaBeanUtils.ToString(yValueType)).Append(',').Append(' ');
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info)).Append(',').Append(' ');
		  buf.Append("parameterMetadata").Append('=').Append(JodaBeanUtils.ToString(parameterMetadata));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}