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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using ObjDoublePair = com.opengamma.strata.collect.tuple.ObjDoublePair;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;
	using BoundSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.BoundSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;

	/// <summary>
	/// A surface based on interpolation between a number of nodal points.
	/// <para>
	/// This class defines a surface in terms of a fixed number of nodes, referred to as <i>parameters</i>.
	/// </para>
	/// <para>
	/// Each node has an x-value and a y-value.
	/// The interface is focused on finding the z-value for a given x-value and y-value.
	/// An interpolator is used to find z-values for x-values and y-values between two nodes.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class InterpolatedNodalSurface implements NodalSurface, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class InterpolatedNodalSurface : NodalSurface, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final SurfaceMetadata metadata;
		private readonly SurfaceMetadata metadata;
	  /// <summary>
	  /// The array of x-values, one for each point.
	  /// <para>
	  /// This array will contains at least two elements.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.collect.array.DoubleArray xValues;
	  private readonly DoubleArray xValues;
	  /// <summary>
	  /// The array of y-values, one for each point.
	  /// <para>
	  /// This array will contains at least two elements and be of the same length as x-values.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.collect.array.DoubleArray yValues;
	  private readonly DoubleArray yValues;
	  /// <summary>
	  /// The array of z-values, one for each point.
	  /// <para>
	  /// This array will contains at least two elements and be of the same length as x-values.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.collect.array.DoubleArray zValues;
	  private readonly DoubleArray zValues;
	  /// <summary>
	  /// The underlying interpolator.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator interpolator;
	  private readonly SurfaceInterpolator interpolator;
	  /// <summary>
	  /// The bound interpolator.
	  /// </summary>
	  [NonSerialized]
	  private readonly BoundSurfaceInterpolator boundInterpolator; // derived and cached, not a property
	  /// <summary>
	  /// The parameter metadata.
	  /// </summary>
	  [NonSerialized]
	  private readonly IList<ParameterMetadata> parameterMetadata; // derived, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an interpolated surface with metadata.
	  /// <para>
	  /// The value arrays must be sorted, by x-values then y-values.
	  /// An exception is thrown if they are not sorted.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="metadata">  the surface metadata </param>
	  /// <param name="xValues">  the x-values, must be sorted from low to high </param>
	  /// <param name="yValues">  the y-values, must be sorted from low to high within x </param>
	  /// <param name="zValues">  the z-values </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <returns> the surface </returns>
	  public static InterpolatedNodalSurface of(SurfaceMetadata metadata, DoubleArray xValues, DoubleArray yValues, DoubleArray zValues, SurfaceInterpolator interpolator)
	  {

		return new InterpolatedNodalSurface(metadata, xValues, yValues, zValues, interpolator);
	  }

	  /// <summary>
	  /// Creates an interpolated surface with metadata, where the values are not sorted.
	  /// <para>
	  /// The value arrays will be sorted, by x-values then y-values.
	  /// Both the z-values and parameter metadata will be sorted along with the x and y values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="metadata">  the surface metadata </param>
	  /// <param name="xValues">  the x-values </param>
	  /// <param name="yValues">  the y-values </param>
	  /// <param name="zValues">  the z-values </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <returns> the surface </returns>
	  public static InterpolatedNodalSurface ofUnsorted(SurfaceMetadata metadata, DoubleArray xValues, DoubleArray yValues, DoubleArray zValues, SurfaceInterpolator interpolator)
	  {

		return new InterpolatedNodalSurface(metadata, xValues, yValues, zValues, interpolator, true);
	  }

	  //-------------------------------------------------------------------------
	  // restricted constructor
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private InterpolatedNodalSurface(SurfaceMetadata metadata, com.opengamma.strata.collect.array.DoubleArray xValues, com.opengamma.strata.collect.array.DoubleArray yValues, com.opengamma.strata.collect.array.DoubleArray zValues, com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator interpolator)
	  private InterpolatedNodalSurface(SurfaceMetadata metadata, DoubleArray xValues, DoubleArray yValues, DoubleArray zValues, SurfaceInterpolator interpolator)
	  {

		validateInputs(metadata, xValues, yValues, zValues, interpolator);
		for (int i = 1; i < xValues.size(); i++)
		{
		  if (xValues.get(i) < xValues.get(i - 1))
		  {
			throw new System.ArgumentException("Array of x-values must be sorted");
		  }
		  if (xValues.get(i) == xValues.get(i - 1) && yValues.get(i) <= yValues.get(i - 1))
		  {
			throw new System.ArgumentException("Array of y-values must be sorted and unique within x-values");
		  }
		}
		this.metadata = metadata;
		this.xValues = xValues;
		this.yValues = yValues;
		this.zValues = zValues;
		this.interpolator = interpolator;
		this.boundInterpolator = interpolator.bind(xValues, yValues, zValues);
		this.parameterMetadata = IntStream.range(0, ParameterCount).mapToObj(i => metadata.getParameterMetadata(i)).collect(toImmutableList());
	  }

	  // constructor that sorts (artificial boolean flag)
	  private InterpolatedNodalSurface(SurfaceMetadata metadata, DoubleArray xValues, DoubleArray yValues, DoubleArray zValues, SurfaceInterpolator interpolator, bool sort)
	  {

		validateInputs(metadata, xValues, yValues, zValues, interpolator);
		// sort inputs
		IDictionary<DoublesPair, ObjDoublePair<ParameterMetadata>> sorted = new SortedDictionary<DoublesPair, ObjDoublePair<ParameterMetadata>>();
		for (int i = 0; i < xValues.size(); i++)
		{
		  ParameterMetadata pm = metadata.getParameterMetadata(i);
		  sorted[DoublesPair.of(xValues.get(i), yValues.get(i))] = ObjDoublePair.of(pm, zValues.get(i));
		}
		double[] sortedX = new double[sorted.Count];
		double[] sortedY = new double[sorted.Count];
		double[] sortedZ = new double[sorted.Count];
		ParameterMetadata[] sortedPm = new ParameterMetadata[sorted.Count];
		int pos = 0;
		foreach (KeyValuePair<DoublesPair, ObjDoublePair<ParameterMetadata>> entry in sorted.SetOfKeyValuePairs())
		{
		  sortedX[pos] = entry.Key.First;
		  sortedY[pos] = entry.Key.Second;
		  sortedZ[pos] = entry.Value.Second;
		  sortedPm[pos] = entry.Value.First;
		  pos++;
		}
		// assign
		SurfaceMetadata sortedMetadata = metadata.withParameterMetadata(Arrays.asList(sortedPm));
		this.metadata = sortedMetadata;
		this.xValues = DoubleArray.ofUnsafe(sortedX);
		this.yValues = DoubleArray.ofUnsafe(sortedY);
		this.zValues = DoubleArray.ofUnsafe(sortedZ);
		IDictionary<DoublesPair, double> pairs = new Dictionary<DoublesPair, double>();
		for (int i = 0; i < xValues.size(); i++)
		{
		  pairs[DoublesPair.of(xValues.get(i), yValues.get(i))] = zValues.get(i);
		}
		this.interpolator = interpolator;
		this.boundInterpolator = interpolator.bind(this.xValues, this.yValues, this.zValues);
		this.parameterMetadata = IntStream.range(0, ParameterCount).mapToObj(i => sortedMetadata.getParameterMetadata(i)).collect(toImmutableList());
	  }

	  // basic validation
	  private void validateInputs(SurfaceMetadata metadata, DoubleArray xValues, DoubleArray yValues, DoubleArray zValues, SurfaceInterpolator interpolator)
	  {

		ArgChecker.notNull(metadata, "metadata");
		ArgChecker.notNull(xValues, "times");
		ArgChecker.notNull(yValues, "values");
		ArgChecker.notNull(interpolator, "interpolator");
		if (xValues.size() < 2)
		{
		  throw new System.ArgumentException("Length of x-values must be at least 2");
		}
		if (xValues.size() != yValues.size())
		{
		  throw new System.ArgumentException("Length of x-values and y-values must match");
		}
		if (xValues.size() != zValues.size())
		{
		  throw new System.ArgumentException("Length of x-values and z-values must match");
		}
		metadata.ParameterMetadata.ifPresent(@params =>
		{
		if (xValues.size() != @params.size())
		{
			throw new System.ArgumentException("Length of x-values and parameter metadata must match when metadata present");
		}
		});
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new InterpolatedNodalSurface(metadata, xValues, yValues, zValues, interpolator);
	  }

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return zValues.size();
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return zValues.get(parameterIndex);
	  }

	  public InterpolatedNodalSurface withParameter(int parameterIndex, double newValue)
	  {
		return withZValues(zValues.with(parameterIndex, newValue));
	  }

	  public override InterpolatedNodalSurface withPerturbation(ParameterPerturbation perturbation)
	  {
		int size = zValues.size();
		DoubleArray perturbedValues = DoubleArray.of(size, i => perturbation(i, zValues.get(i), getParameterMetadata(i)));
		return withZValues(perturbedValues);
	  }

	  //-------------------------------------------------------------------------
	  public double zValue(double x, double y)
	  {
		return boundInterpolator.interpolate(x, y);
	  }

	  public UnitParameterSensitivity zValueParameterSensitivity(double x, double y)
	  {
		DoubleArray sensitivityValues = boundInterpolator.parameterSensitivity(x, y);
		return createParameterSensitivity(sensitivityValues);
	  }

	  //-------------------------------------------------------------------------
	  public InterpolatedNodalSurface withMetadata(SurfaceMetadata metadata)
	  {
		return new InterpolatedNodalSurface(metadata, xValues, yValues, zValues, interpolator);
	  }

	  public InterpolatedNodalSurface withZValues(DoubleArray zValues)
	  {
		return new InterpolatedNodalSurface(metadata, xValues, yValues, zValues, interpolator);
	  }

	  //-------------------------------------------------------------------------
	  public override UnitParameterSensitivity createParameterSensitivity(DoubleArray sensitivities)
	  {
		return UnitParameterSensitivity.of(Name, parameterMetadata, sensitivities);
	  }

	  public override CurrencyParameterSensitivity createParameterSensitivity(Currency currency, DoubleArray sensitivities)
	  {
		return CurrencyParameterSensitivity.of(Name, parameterMetadata, currency, sensitivities);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code InterpolatedNodalSurface}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static InterpolatedNodalSurface.Meta meta()
	  {
		return InterpolatedNodalSurface.Meta.INSTANCE;
	  }

	  static InterpolatedNodalSurface()
	  {
		MetaBean.register(InterpolatedNodalSurface.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static InterpolatedNodalSurface.Builder builder()
	  {
		return new InterpolatedNodalSurface.Builder();
	  }

	  public override InterpolatedNodalSurface.Meta metaBean()
	  {
		return InterpolatedNodalSurface.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the surface metadata.
	  /// <para>
	  /// The metadata includes an optional list of parameter metadata.
	  /// If present, the size of the parameter metadata list will match the number of parameters of this surface.
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
	  /// Gets the array of x-values, one for each point.
	  /// <para>
	  /// This array will contains at least two elements.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray XValues
	  {
		  get
		  {
			return xValues;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the array of y-values, one for each point.
	  /// <para>
	  /// This array will contains at least two elements and be of the same length as x-values.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray YValues
	  {
		  get
		  {
			return yValues;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the array of z-values, one for each point.
	  /// <para>
	  /// This array will contains at least two elements and be of the same length as x-values.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray ZValues
	  {
		  get
		  {
			return zValues;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying interpolator. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SurfaceInterpolator Interpolator
	  {
		  get
		  {
			return interpolator;
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
		  InterpolatedNodalSurface other = (InterpolatedNodalSurface) obj;
		  return JodaBeanUtils.equal(metadata, other.metadata) && JodaBeanUtils.equal(xValues, other.xValues) && JodaBeanUtils.equal(yValues, other.yValues) && JodaBeanUtils.equal(zValues, other.zValues) && JodaBeanUtils.equal(interpolator, other.interpolator);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(metadata);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(xValues);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yValues);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(zValues);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(interpolator);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("InterpolatedNodalSurface{");
		buf.Append("metadata").Append('=').Append(metadata).Append(',').Append(' ');
		buf.Append("xValues").Append('=').Append(xValues).Append(',').Append(' ');
		buf.Append("yValues").Append('=').Append(yValues).Append(',').Append(' ');
		buf.Append("zValues").Append('=').Append(zValues).Append(',').Append(' ');
		buf.Append("interpolator").Append('=').Append(JodaBeanUtils.ToString(interpolator));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code InterpolatedNodalSurface}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  metadata_Renamed = DirectMetaProperty.ofImmutable(this, "metadata", typeof(InterpolatedNodalSurface), typeof(SurfaceMetadata));
			  xValues_Renamed = DirectMetaProperty.ofImmutable(this, "xValues", typeof(InterpolatedNodalSurface), typeof(DoubleArray));
			  yValues_Renamed = DirectMetaProperty.ofImmutable(this, "yValues", typeof(InterpolatedNodalSurface), typeof(DoubleArray));
			  zValues_Renamed = DirectMetaProperty.ofImmutable(this, "zValues", typeof(InterpolatedNodalSurface), typeof(DoubleArray));
			  interpolator_Renamed = DirectMetaProperty.ofImmutable(this, "interpolator", typeof(InterpolatedNodalSurface), typeof(SurfaceInterpolator));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "metadata", "xValues", "yValues", "zValues", "interpolator");
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
		/// The meta-property for the {@code xValues} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> xValues_Renamed;
		/// <summary>
		/// The meta-property for the {@code yValues} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> yValues_Renamed;
		/// <summary>
		/// The meta-property for the {@code zValues} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> zValues_Renamed;
		/// <summary>
		/// The meta-property for the {@code interpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SurfaceInterpolator> interpolator_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "metadata", "xValues", "yValues", "zValues", "interpolator");
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
			case 1681280954: // xValues
			  return xValues_Renamed;
			case -1726182661: // yValues
			  return yValues_Renamed;
			case -838678980: // zValues
			  return zValues_Renamed;
			case 2096253127: // interpolator
			  return interpolator_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override InterpolatedNodalSurface.Builder builder()
		{
		  return new InterpolatedNodalSurface.Builder();
		}

		public override Type beanType()
		{
		  return typeof(InterpolatedNodalSurface);
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
		/// The meta-property for the {@code xValues} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> xValues()
		{
		  return xValues_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yValues} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> yValues()
		{
		  return yValues_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code zValues} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> zValues()
		{
		  return zValues_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code interpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SurfaceInterpolator> interpolator()
		{
		  return interpolator_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  return ((InterpolatedNodalSurface) bean).Metadata;
			case 1681280954: // xValues
			  return ((InterpolatedNodalSurface) bean).XValues;
			case -1726182661: // yValues
			  return ((InterpolatedNodalSurface) bean).YValues;
			case -838678980: // zValues
			  return ((InterpolatedNodalSurface) bean).ZValues;
			case 2096253127: // interpolator
			  return ((InterpolatedNodalSurface) bean).Interpolator;
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
	  /// The bean-builder for {@code InterpolatedNodalSurface}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<InterpolatedNodalSurface>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SurfaceMetadata metadata_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DoubleArray xValues_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DoubleArray yValues_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DoubleArray zValues_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SurfaceInterpolator interpolator_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(InterpolatedNodalSurface beanToCopy)
		{
		  this.metadata_Renamed = beanToCopy.Metadata;
		  this.xValues_Renamed = beanToCopy.XValues;
		  this.yValues_Renamed = beanToCopy.YValues;
		  this.zValues_Renamed = beanToCopy.ZValues;
		  this.interpolator_Renamed = beanToCopy.Interpolator;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  return metadata_Renamed;
			case 1681280954: // xValues
			  return xValues_Renamed;
			case -1726182661: // yValues
			  return yValues_Renamed;
			case -838678980: // zValues
			  return zValues_Renamed;
			case 2096253127: // interpolator
			  return interpolator_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  this.metadata_Renamed = (SurfaceMetadata) newValue;
			  break;
			case 1681280954: // xValues
			  this.xValues_Renamed = (DoubleArray) newValue;
			  break;
			case -1726182661: // yValues
			  this.yValues_Renamed = (DoubleArray) newValue;
			  break;
			case -838678980: // zValues
			  this.zValues_Renamed = (DoubleArray) newValue;
			  break;
			case 2096253127: // interpolator
			  this.interpolator_Renamed = (SurfaceInterpolator) newValue;
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

		public override InterpolatedNodalSurface build()
		{
		  return new InterpolatedNodalSurface(metadata_Renamed, xValues_Renamed, yValues_Renamed, zValues_Renamed, interpolator_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the surface metadata.
		/// <para>
		/// The metadata includes an optional list of parameter metadata.
		/// If present, the size of the parameter metadata list will match the number of parameters of this surface.
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
		/// Sets the array of x-values, one for each point.
		/// <para>
		/// This array will contains at least two elements.
		/// </para>
		/// </summary>
		/// <param name="xValues">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder xValues(DoubleArray xValues)
		{
		  JodaBeanUtils.notNull(xValues, "xValues");
		  this.xValues_Renamed = xValues;
		  return this;
		}

		/// <summary>
		/// Sets the array of y-values, one for each point.
		/// <para>
		/// This array will contains at least two elements and be of the same length as x-values.
		/// </para>
		/// </summary>
		/// <param name="yValues">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder yValues(DoubleArray yValues)
		{
		  JodaBeanUtils.notNull(yValues, "yValues");
		  this.yValues_Renamed = yValues;
		  return this;
		}

		/// <summary>
		/// Sets the array of z-values, one for each point.
		/// <para>
		/// This array will contains at least two elements and be of the same length as x-values.
		/// </para>
		/// </summary>
		/// <param name="zValues">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder zValues(DoubleArray zValues)
		{
		  JodaBeanUtils.notNull(zValues, "zValues");
		  this.zValues_Renamed = zValues;
		  return this;
		}

		/// <summary>
		/// Sets the underlying interpolator. </summary>
		/// <param name="interpolator">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder interpolator(SurfaceInterpolator interpolator)
		{
		  JodaBeanUtils.notNull(interpolator, "interpolator");
		  this.interpolator_Renamed = interpolator;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("InterpolatedNodalSurface.Builder{");
		  buf.Append("metadata").Append('=').Append(JodaBeanUtils.ToString(metadata_Renamed)).Append(',').Append(' ');
		  buf.Append("xValues").Append('=').Append(JodaBeanUtils.ToString(xValues_Renamed)).Append(',').Append(' ');
		  buf.Append("yValues").Append('=').Append(JodaBeanUtils.ToString(yValues_Renamed)).Append(',').Append(' ');
		  buf.Append("zValues").Append('=').Append(JodaBeanUtils.ToString(zValues_Renamed)).Append(',').Append(' ');
		  buf.Append("interpolator").Append('=').Append(JodaBeanUtils.ToString(interpolator_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}