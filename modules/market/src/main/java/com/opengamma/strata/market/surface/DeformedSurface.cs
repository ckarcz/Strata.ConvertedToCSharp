using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// The deformed surface.
	/// <para>
	/// The deformation is applied to {@code Surface}, and defined in terms of {@code Function}, which returns z-value and 
	/// sensitivities to the nodes of the original surface.
	/// </para>
	/// <para>
	/// Typical application of this class is to represent a surface constructed via model calibration to interpolated 
	/// market data, where the market data points and interpolation are stored in {@code originalSurface}, 
	/// and {@code deformationFunction} defines the constructed surface.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class DeformedSurface implements Surface, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DeformedSurface : Surface, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final SurfaceMetadata metadata;
		private readonly SurfaceMetadata metadata;
	  /// <summary>
	  /// The original surface.
	  /// <para>
	  /// The underlying surface which receives the deformation defined by {@code deformationFunction}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Surface originalSurface;
	  private readonly Surface originalSurface;
	  /// <summary>
	  /// The deformation function.
	  /// <para>
	  /// The deformation to the original surface is define by this function.
	  /// The function takes {@code DoublesPair} of x-value and y-value, then returns {@code ValueDerivatives} 
	  /// which contains z-value for the specified x,y values, and node sensitivities to the original surface.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final System.Func<com.opengamma.strata.collect.tuple.DoublesPair, com.opengamma.strata.basics.value.ValueDerivatives> deformationFunction;
	  private readonly System.Func<DoublesPair, ValueDerivatives> deformationFunction;

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return originalSurface.ParameterCount;
		  }
	  }

	  public DeformedSurface withMetadata(SurfaceMetadata metadata)
	  {
		return new DeformedSurface(metadata, originalSurface, deformationFunction);
	  }

	  public double getParameter(int parameterIndex)
	  {
		return originalSurface.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return originalSurface.getParameterMetadata(parameterIndex);
	  }

	  public Surface withParameter(int parameterIndex, double newValue)
	  {
		throw new System.ArgumentException("deformationFunction must be redefined with the new value");
	  }

	  //-------------------------------------------------------------------------
	  public double zValue(double x, double y)
	  {
		return deformationFunction.apply(DoublesPair.of(x, y)).Value;
	  }

	  public UnitParameterSensitivity zValueParameterSensitivity(double x, double y)
	  {
		return Metadata.ParameterMetadata.Present ? UnitParameterSensitivity.of(Metadata.SurfaceName, Metadata.ParameterMetadata.get(), deformationFunction.apply(DoublesPair.of(x, y)).Derivatives) : UnitParameterSensitivity.of(Metadata.SurfaceName, deformationFunction.apply(DoublesPair.of(x, y)).Derivatives);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance.
	  /// </summary>
	  /// <param name="metadata">  the surface metadata </param>
	  /// <param name="originalSurface">  the original surface </param>
	  /// <param name="deformationFunction">  the deformation function </param>
	  /// <returns> the surface </returns>
	  public static DeformedSurface of(SurfaceMetadata metadata, Surface originalSurface, System.Func<DoublesPair, ValueDerivatives> deformationFunction)
	  {

		return DeformedSurface.builder().metadata(metadata).originalSurface(originalSurface).deformationFunction(deformationFunction).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DeformedSurface}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DeformedSurface.Meta meta()
	  {
		return DeformedSurface.Meta.INSTANCE;
	  }

	  static DeformedSurface()
	  {
		MetaBean.register(DeformedSurface.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static DeformedSurface.Builder builder()
	  {
		return new DeformedSurface.Builder();
	  }

	  private DeformedSurface(SurfaceMetadata metadata, Surface originalSurface, System.Func<DoublesPair, ValueDerivatives> deformationFunction)
	  {
		JodaBeanUtils.notNull(metadata, "metadata");
		JodaBeanUtils.notNull(originalSurface, "originalSurface");
		JodaBeanUtils.notNull(deformationFunction, "deformationFunction");
		this.metadata = metadata;
		this.originalSurface = originalSurface;
		this.deformationFunction = deformationFunction;
	  }

	  public override DeformedSurface.Meta metaBean()
	  {
		return DeformedSurface.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the surface metadata.
	  /// <para>
	  /// The metadata includes an optional list of parameter metadata.
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
	  /// Gets the original surface.
	  /// <para>
	  /// The underlying surface which receives the deformation defined by {@code deformationFunction}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Surface OriginalSurface
	  {
		  get
		  {
			return originalSurface;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the deformation function.
	  /// <para>
	  /// The deformation to the original surface is define by this function.
	  /// The function takes {@code DoublesPair} of x-value and y-value, then returns {@code ValueDerivatives}
	  /// which contains z-value for the specified x,y values, and node sensitivities to the original surface.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public System.Func<DoublesPair, ValueDerivatives> DeformationFunction
	  {
		  get
		  {
			return deformationFunction;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  DeformedSurface other = (DeformedSurface) obj;
		  return JodaBeanUtils.equal(metadata, other.metadata) && JodaBeanUtils.equal(originalSurface, other.originalSurface) && JodaBeanUtils.equal(deformationFunction, other.deformationFunction);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(metadata);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(originalSurface);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(deformationFunction);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("DeformedSurface{");
		buf.Append("metadata").Append('=').Append(metadata).Append(',').Append(' ');
		buf.Append("originalSurface").Append('=').Append(originalSurface).Append(',').Append(' ');
		buf.Append("deformationFunction").Append('=').Append(JodaBeanUtils.ToString(deformationFunction));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DeformedSurface}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  metadata_Renamed = DirectMetaProperty.ofImmutable(this, "metadata", typeof(DeformedSurface), typeof(SurfaceMetadata));
			  originalSurface_Renamed = DirectMetaProperty.ofImmutable(this, "originalSurface", typeof(DeformedSurface), typeof(Surface));
			  deformationFunction_Renamed = DirectMetaProperty.ofImmutable(this, "deformationFunction", typeof(DeformedSurface), (Type) typeof(System.Func));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "metadata", "originalSurface", "deformationFunction");
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
		/// The meta-property for the {@code originalSurface} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Surface> originalSurface_Renamed;
		/// <summary>
		/// The meta-property for the {@code deformationFunction} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<System.Func<com.opengamma.strata.collect.tuple.DoublesPair, com.opengamma.strata.basics.value.ValueDerivatives>> deformationFunction = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "deformationFunction", DeformedSurface.class, (Class) System.Func.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<System.Func<DoublesPair, ValueDerivatives>> deformationFunction_Renamed = DirectMetaProperty.ofImmutable(this, "deformationFunction", typeof(DeformedSurface), (Type) typeof(System.Func));
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "metadata", "originalSurface", "deformationFunction");
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
			case 1982430620: // originalSurface
			  return originalSurface_Renamed;
			case -360086200: // deformationFunction
			  return deformationFunction_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override DeformedSurface.Builder builder()
		{
		  return new DeformedSurface.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DeformedSurface);
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
		/// The meta-property for the {@code originalSurface} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Surface> originalSurface()
		{
		  return originalSurface_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code deformationFunction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<System.Func<DoublesPair, ValueDerivatives>> deformationFunction()
		{
		  return deformationFunction_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  return ((DeformedSurface) bean).Metadata;
			case 1982430620: // originalSurface
			  return ((DeformedSurface) bean).OriginalSurface;
			case -360086200: // deformationFunction
			  return ((DeformedSurface) bean).DeformationFunction;
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
	  /// The bean-builder for {@code DeformedSurface}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<DeformedSurface>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SurfaceMetadata metadata_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Surface originalSurface_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal System.Func<DoublesPair, ValueDerivatives> deformationFunction_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(DeformedSurface beanToCopy)
		{
		  this.metadata_Renamed = beanToCopy.Metadata;
		  this.originalSurface_Renamed = beanToCopy.OriginalSurface;
		  this.deformationFunction_Renamed = beanToCopy.DeformationFunction;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  return metadata_Renamed;
			case 1982430620: // originalSurface
			  return originalSurface_Renamed;
			case -360086200: // deformationFunction
			  return deformationFunction_Renamed;
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
			case -450004177: // metadata
			  this.metadata_Renamed = (SurfaceMetadata) newValue;
			  break;
			case 1982430620: // originalSurface
			  this.originalSurface_Renamed = (Surface) newValue;
			  break;
			case -360086200: // deformationFunction
			  this.deformationFunction_Renamed = (System.Func<DoublesPair, ValueDerivatives>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override DeformedSurface build()
		{
		  return new DeformedSurface(metadata_Renamed, originalSurface_Renamed, deformationFunction_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the surface metadata.
		/// <para>
		/// The metadata includes an optional list of parameter metadata.
		/// </para>
		/// </summary>
		/// <param name="metadata">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder metadata(SurfaceMetadata metadata)
		{
		  JodaBeanUtils.notNull(metadata, "metadata");
		  this.metadata_Renamed = metadata;
		  return this;
		}

		/// <summary>
		/// Sets the original surface.
		/// <para>
		/// The underlying surface which receives the deformation defined by {@code deformationFunction}.
		/// </para>
		/// </summary>
		/// <param name="originalSurface">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder originalSurface(Surface originalSurface)
		{
		  JodaBeanUtils.notNull(originalSurface, "originalSurface");
		  this.originalSurface_Renamed = originalSurface;
		  return this;
		}

		/// <summary>
		/// Sets the deformation function.
		/// <para>
		/// The deformation to the original surface is define by this function.
		/// The function takes {@code DoublesPair} of x-value and y-value, then returns {@code ValueDerivatives}
		/// which contains z-value for the specified x,y values, and node sensitivities to the original surface.
		/// </para>
		/// </summary>
		/// <param name="deformationFunction">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder deformationFunction(System.Func<DoublesPair, ValueDerivatives> deformationFunction)
		{
		  JodaBeanUtils.notNull(deformationFunction, "deformationFunction");
		  this.deformationFunction_Renamed = deformationFunction;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("DeformedSurface.Builder{");
		  buf.Append("metadata").Append('=').Append(JodaBeanUtils.ToString(metadata_Renamed)).Append(',').Append(' ');
		  buf.Append("originalSurface").Append('=').Append(JodaBeanUtils.ToString(originalSurface_Renamed)).Append(',').Append(' ');
		  buf.Append("deformationFunction").Append('=').Append(JodaBeanUtils.ToString(deformationFunction_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}