using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
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
	/// Default metadata for a surface.
	/// <para>
	/// This implementation of <seealso cref="SurfaceMetadata"/> provides the surface name and nodes.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class DefaultSurfaceMetadata implements SurfaceMetadata, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DefaultSurfaceMetadata : SurfaceMetadata, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final SurfaceName surfaceName;
		private readonly SurfaceName surfaceName;
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
	  /// This type provides meaning to the y-values.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.market.ValueType yValueType;
	  private readonly ValueType yValueType;
	  /// <summary>
	  /// The x-value type, providing meaning to the z-values of the curve.
	  /// <para>
	  /// This type provides meaning to the z-values.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.market.ValueType zValueType;
	  private readonly ValueType zValueType;
	  /// <summary>
	  /// The additional surface information.
	  /// <para>
	  /// This stores additional information for the surface.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<SurfaceInfoType<?>, Object> info;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<SurfaceInfoType<object>, object> info;
	  /// <summary>
	  /// The metadata about the parameters.
	  /// <para>
	  /// If present, the parameter metadata should match the number of parameters on the surface.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional", overrideGet = true, type = "List<>", builderType = "List<? extends ParameterMetadata>") private final com.google.common.collect.ImmutableList<com.opengamma.strata.market.param.ParameterMetadata> parameterMetadata;
	  private readonly ImmutableList<ParameterMetadata> parameterMetadata;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the metadata.
	  /// <para>
	  /// No information will be available for the x-values, y-values, z-values or parameters.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <returns> the metadata </returns>
	  public static DefaultSurfaceMetadata of(string name)
	  {
		return of(SurfaceName.of(name));
	  }

	  /// <summary>
	  /// Creates the metadata.
	  /// <para>
	  /// No information will be available for the x-values, y-values, z-values or parameters.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <returns> the metadata </returns>
	  public static DefaultSurfaceMetadata of(SurfaceName name)
	  {
		return new DefaultSurfaceMetadata(name, ValueType.UNKNOWN, ValueType.UNKNOWN, ValueType.UNKNOWN, ImmutableMap.of(), null);
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean.
	  /// </summary>
	  /// <returns> the builder, not null </returns>
	  public static DefaultSurfaceMetadataBuilder builder()
	  {
		return new DefaultSurfaceMetadataBuilder();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.xValueType = ValueType.UNKNOWN;
		builder.yValueType = ValueType.UNKNOWN;
		builder.zValueType = ValueType.UNKNOWN;
	  }

	  //-------------------------------------------------------------------------
	  public override T getInfo<T>(SurfaceInfoType<T> type)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") T value = (T) info.get(type);
		  T value = (T) info.get(type);
		if (value == null)
		{
		  throw new System.ArgumentException(Messages.format("Surface info not found for type '{}'", type));
		}
		return value;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public <T> java.util.Optional<T> findInfo(SurfaceInfoType<T> type)
	  public Optional<T> findInfo<T>(SurfaceInfoType<T> type)
	  {
		return Optional.ofNullable((T) info.get(type));
	  }

	  //-------------------------------------------------------------------------
	  public DefaultSurfaceMetadata withInfo<T>(SurfaceInfoType<T> type, T value)
	  {
		return toBuilder().addInfo(type, value).build();
	  }

	  public DefaultSurfaceMetadata withParameterMetadata<T1>(IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
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
	  public DefaultSurfaceMetadataBuilder toBuilder()
	  {
		return new DefaultSurfaceMetadataBuilder(this);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultSurfaceMetadata}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DefaultSurfaceMetadata.Meta meta()
	  {
		return DefaultSurfaceMetadata.Meta.INSTANCE;
	  }

	  static DefaultSurfaceMetadata()
	  {
		MetaBean.register(DefaultSurfaceMetadata.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="surfaceName">  the value of the property, not null </param>
	  /// <param name="xValueType">  the value of the property, not null </param>
	  /// <param name="yValueType">  the value of the property, not null </param>
	  /// <param name="zValueType">  the value of the property, not null </param>
	  /// <param name="info">  the value of the property, not null </param>
	  /// <param name="parameterMetadata">  the value of the property </param>
	  internal DefaultSurfaceMetadata<T1, T2>(SurfaceName surfaceName, ValueType xValueType, ValueType yValueType, ValueType zValueType, IDictionary<T1> info, IList<T2> parameterMetadata) where T2 : com.opengamma.strata.market.param.ParameterMetadata
	  {
		JodaBeanUtils.notNull(surfaceName, "surfaceName");
		JodaBeanUtils.notNull(xValueType, "xValueType");
		JodaBeanUtils.notNull(yValueType, "yValueType");
		JodaBeanUtils.notNull(zValueType, "zValueType");
		JodaBeanUtils.notNull(info, "info");
		this.surfaceName = surfaceName;
		this.xValueType = xValueType;
		this.yValueType = yValueType;
		this.zValueType = zValueType;
		this.info = ImmutableMap.copyOf(info);
		this.parameterMetadata = (parameterMetadata != null ? ImmutableList.copyOf(parameterMetadata) : null);
	  }

	  public override DefaultSurfaceMetadata.Meta metaBean()
	  {
		return DefaultSurfaceMetadata.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the surface name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SurfaceName SurfaceName
	  {
		  get
		  {
			return surfaceName;
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
	  /// This type provides meaning to the y-values.
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
	  /// Gets the x-value type, providing meaning to the z-values of the curve.
	  /// <para>
	  /// This type provides meaning to the z-values.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueType ZValueType
	  {
		  get
		  {
			return zValueType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional surface information.
	  /// <para>
	  /// This stores additional information for the surface.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<SurfaceInfoType<?>, Object> getInfo()
	  public ImmutableMap<SurfaceInfoType<object>, object> Info
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
	  /// If present, the parameter metadata should match the number of parameters on the surface.
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
		  DefaultSurfaceMetadata other = (DefaultSurfaceMetadata) obj;
		  return JodaBeanUtils.equal(surfaceName, other.surfaceName) && JodaBeanUtils.equal(xValueType, other.xValueType) && JodaBeanUtils.equal(yValueType, other.yValueType) && JodaBeanUtils.equal(zValueType, other.zValueType) && JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(parameterMetadata, other.parameterMetadata);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(surfaceName);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(xValueType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yValueType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(zValueType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameterMetadata);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("DefaultSurfaceMetadata{");
		buf.Append("surfaceName").Append('=').Append(surfaceName).Append(',').Append(' ');
		buf.Append("xValueType").Append('=').Append(xValueType).Append(',').Append(' ');
		buf.Append("yValueType").Append('=').Append(yValueType).Append(',').Append(' ');
		buf.Append("zValueType").Append('=').Append(zValueType).Append(',').Append(' ');
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("parameterMetadata").Append('=').Append(JodaBeanUtils.ToString(parameterMetadata));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultSurfaceMetadata}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  surfaceName_Renamed = DirectMetaProperty.ofImmutable(this, "surfaceName", typeof(DefaultSurfaceMetadata), typeof(SurfaceName));
			  xValueType_Renamed = DirectMetaProperty.ofImmutable(this, "xValueType", typeof(DefaultSurfaceMetadata), typeof(ValueType));
			  yValueType_Renamed = DirectMetaProperty.ofImmutable(this, "yValueType", typeof(DefaultSurfaceMetadata), typeof(ValueType));
			  zValueType_Renamed = DirectMetaProperty.ofImmutable(this, "zValueType", typeof(DefaultSurfaceMetadata), typeof(ValueType));
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(DefaultSurfaceMetadata), (Type) typeof(ImmutableMap));
			  parameterMetadata_Renamed = DirectMetaProperty.ofImmutable(this, "parameterMetadata", typeof(DefaultSurfaceMetadata), (Type) typeof(System.Collections.IList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "surfaceName", "xValueType", "yValueType", "zValueType", "info", "parameterMetadata");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code surfaceName} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SurfaceName> surfaceName_Renamed;
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
		/// The meta-property for the {@code zValueType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueType> zValueType_Renamed;
		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<SurfaceInfoType<?>, Object>> info = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "info", DefaultSurfaceMetadata.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableMap<SurfaceInfoType<object>, object>> info_Renamed;
		/// <summary>
		/// The meta-property for the {@code parameterMetadata} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<java.util.List<com.opengamma.strata.market.param.ParameterMetadata>> parameterMetadata = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "parameterMetadata", DefaultSurfaceMetadata.class, (Class) java.util.List.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IList<ParameterMetadata>> parameterMetadata_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "surfaceName", "xValueType", "yValueType", "zValueType", "info", "parameterMetadata");
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
			case -1403077416: // surfaceName
			  return surfaceName_Renamed;
			case -868509005: // xValueType
			  return xValueType_Renamed;
			case -1065022510: // yValueType
			  return yValueType_Renamed;
			case -1261536015: // zValueType
			  return zValueType_Renamed;
			case 3237038: // info
			  return info_Renamed;
			case -1169106440: // parameterMetadata
			  return parameterMetadata_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends DefaultSurfaceMetadata> builder()
		public override BeanBuilder<DefaultSurfaceMetadata> builder()
		{
		  return new DefaultSurfaceMetadata.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DefaultSurfaceMetadata);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code surfaceName} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SurfaceName> surfaceName()
		{
		  return surfaceName_Renamed;
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
		/// The meta-property for the {@code zValueType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueType> zValueType()
		{
		  return zValueType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code info} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<SurfaceInfoType<?>, Object>> info()
		public MetaProperty<ImmutableMap<SurfaceInfoType<object>, object>> info()
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
			case -1403077416: // surfaceName
			  return ((DefaultSurfaceMetadata) bean).SurfaceName;
			case -868509005: // xValueType
			  return ((DefaultSurfaceMetadata) bean).XValueType;
			case -1065022510: // yValueType
			  return ((DefaultSurfaceMetadata) bean).YValueType;
			case -1261536015: // zValueType
			  return ((DefaultSurfaceMetadata) bean).ZValueType;
			case 3237038: // info
			  return ((DefaultSurfaceMetadata) bean).Info;
			case -1169106440: // parameterMetadata
			  return ((DefaultSurfaceMetadata) bean).parameterMetadata;
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
	  /// The bean-builder for {@code DefaultSurfaceMetadata}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<DefaultSurfaceMetadata>
	  {

		internal SurfaceName surfaceName;
		internal ValueType xValueType;
		internal ValueType yValueType;
		internal ValueType zValueType;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<SurfaceInfoType<?>, Object> info = com.google.common.collect.ImmutableMap.of();
		internal IDictionary<SurfaceInfoType<object>, object> info = ImmutableMap.of();
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
			case -1403077416: // surfaceName
			  return surfaceName;
			case -868509005: // xValueType
			  return xValueType;
			case -1065022510: // yValueType
			  return yValueType;
			case -1261536015: // zValueType
			  return zValueType;
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
			case -1403077416: // surfaceName
			  this.surfaceName = (SurfaceName) newValue;
			  break;
			case -868509005: // xValueType
			  this.xValueType = (ValueType) newValue;
			  break;
			case -1065022510: // yValueType
			  this.yValueType = (ValueType) newValue;
			  break;
			case -1261536015: // zValueType
			  this.zValueType = (ValueType) newValue;
			  break;
			case 3237038: // info
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.info = (java.util.Map<SurfaceInfoType<?>, Object>) newValue;
			  this.info = (IDictionary<SurfaceInfoType<object>, object>) newValue;
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

		public override DefaultSurfaceMetadata build()
		{
		  return new DefaultSurfaceMetadata(surfaceName, xValueType, yValueType, zValueType, info, parameterMetadata);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("DefaultSurfaceMetadata.Builder{");
		  buf.Append("surfaceName").Append('=').Append(JodaBeanUtils.ToString(surfaceName)).Append(',').Append(' ');
		  buf.Append("xValueType").Append('=').Append(JodaBeanUtils.ToString(xValueType)).Append(',').Append(' ');
		  buf.Append("yValueType").Append('=').Append(JodaBeanUtils.ToString(yValueType)).Append(',').Append(' ');
		  buf.Append("zValueType").Append('=').Append(JodaBeanUtils.ToString(zValueType)).Append(',').Append(' ');
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info)).Append(',').Append(' ');
		  buf.Append("parameterMetadata").Append('=').Append(JodaBeanUtils.ToString(parameterMetadata));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}