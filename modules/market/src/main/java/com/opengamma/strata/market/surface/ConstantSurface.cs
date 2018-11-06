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
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Preconditions = com.google.common.@base.Preconditions;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// A surface based on a single constant value.
	/// <para>
	/// This class defines a surface in terms of a single parameter, the constant value.
	/// When queried, <seealso cref="#zValue(double, double)"/> always returns the constant value.
	/// The sensitivity is 1 and the first derivative is 0.
	/// </para>
	/// <para>
	/// The surface has one parameter, the value of the constant.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ConstantSurface implements Surface, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ConstantSurface : Surface, ImmutableBean
	{

	  /// <summary>
	  /// Sensitivity does not vary.
	  /// </summary>
	  private static readonly DoubleArray SENSITIVITY = DoubleArray.of(1d);

	  /// <summary>
	  /// The surface metadata.
	  /// <para>
	  /// The metadata will have not have parameter metadata.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final SurfaceMetadata metadata;
	  private readonly SurfaceMetadata metadata;
	  /// <summary>
	  /// The single z-value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double zValue;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly double zValue_Renamed;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a constant surface with a specific value.
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="zValue">  the constant z-value </param>
	  /// <returns> the surface </returns>
	  public static ConstantSurface of(string name, double zValue)
	  {
		return of(SurfaceName.of(name), zValue);
	  }

	  /// <summary>
	  /// Creates a constant surface with a specific value.
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="zValue">  the constant z-value </param>
	  /// <returns> the surface </returns>
	  public static ConstantSurface of(SurfaceName name, double zValue)
	  {
		return new ConstantSurface(DefaultSurfaceMetadata.of(name), zValue);
	  }

	  /// <summary>
	  /// Creates a constant surface with a specific value.
	  /// </summary>
	  /// <param name="metadata">  the surface metadata </param>
	  /// <param name="zValue">  the constant z-value </param>
	  /// <returns> the surface </returns>
	  public static ConstantSurface of(SurfaceMetadata metadata, double zValue)
	  {
		return new ConstantSurface(metadata, zValue);
	  }

	  //-------------------------------------------------------------------------
	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new ConstantSurface(metadata, zValue_Renamed);
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
		return zValue_Renamed;
	  }

	  public ConstantSurface withParameter(int parameterIndex, double newValue)
	  {
		Preconditions.checkElementIndex(parameterIndex, 1);
		return new ConstantSurface(metadata, newValue);
	  }

	  public override ConstantSurface withPerturbation(ParameterPerturbation perturbation)
	  {
		return new ConstantSurface(metadata, perturbation(0, zValue_Renamed, getParameterMetadata(0)));
	  }

	  //-------------------------------------------------------------------------
	  public double zValue(double x, double y)
	  {
		return zValue_Renamed;
	  }

	  public UnitParameterSensitivity zValueParameterSensitivity(double x, double y)
	  {
		return createParameterSensitivity(SENSITIVITY);
	  }

	  //-------------------------------------------------------------------------
	  public ConstantSurface withMetadata(SurfaceMetadata metadata)
	  {
		return new ConstantSurface(metadata.withParameterMetadata(null), zValue_Renamed);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ConstantSurface}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ConstantSurface.Meta meta()
	  {
		return ConstantSurface.Meta.INSTANCE;
	  }

	  static ConstantSurface()
	  {
		MetaBean.register(ConstantSurface.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ConstantSurface(SurfaceMetadata metadata, double zValue)
	  {
		JodaBeanUtils.notNull(metadata, "metadata");
		this.metadata = metadata;
		this.zValue_Renamed = zValue;
	  }

	  public override ConstantSurface.Meta metaBean()
	  {
		return ConstantSurface.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the surface metadata.
	  /// <para>
	  /// The metadata will have not have parameter metadata.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SurfaceMetadata Metadata
	  {
		  get
		  {
			return metadata;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the single z-value. </summary>
	  /// <returns> the value of the property </returns>
	  public double ZValue
	  {
		  get
		  {
			return zValue_Renamed;
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
		  ConstantSurface other = (ConstantSurface) obj;
		  return JodaBeanUtils.equal(metadata, other.metadata) && JodaBeanUtils.equal(zValue_Renamed, other.zValue_Renamed);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(metadata);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(zValue_Renamed);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("ConstantSurface{");
		buf.Append("metadata").Append('=').Append(metadata).Append(',').Append(' ');
		buf.Append("zValue").Append('=').Append(JodaBeanUtils.ToString(zValue_Renamed));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ConstantSurface}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  metadata_Renamed = DirectMetaProperty.ofImmutable(this, "metadata", typeof(ConstantSurface), typeof(SurfaceMetadata));
			  zValue_Renamed = DirectMetaProperty.ofImmutable(this, "zValue", typeof(ConstantSurface), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "metadata", "zValue");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code metadata} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SurfaceMetadata> metadata_Renamed;
		/// <summary>
		/// The meta-property for the {@code zValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> zValue_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "metadata", "zValue");
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
			case -719790825: // zValue
			  return zValue_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ConstantSurface> builder()
		public override BeanBuilder<ConstantSurface> builder()
		{
		  return new ConstantSurface.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ConstantSurface);
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
		public MetaProperty<SurfaceMetadata> metadata()
		{
		  return metadata_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code zValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> zValue()
		{
		  return zValue_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  return ((ConstantSurface) bean).Metadata;
			case -719790825: // zValue
			  return ((ConstantSurface) bean).ZValue;
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
	  /// The bean-builder for {@code ConstantSurface}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ConstantSurface>
	  {

		internal SurfaceMetadata metadata;
		internal double zValue;

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
			case -719790825: // zValue
			  return zValue;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  this.metadata = (SurfaceMetadata) newValue;
			  break;
			case -719790825: // zValue
			  this.zValue = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ConstantSurface build()
		{
		  return new ConstantSurface(metadata, zValue);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ConstantSurface.Builder{");
		  buf.Append("metadata").Append('=').Append(JodaBeanUtils.ToString(metadata)).Append(',').Append(' ');
		  buf.Append("zValue").Append('=').Append(JodaBeanUtils.ToString(zValue));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}