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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using BoundCurveInterpolator = com.opengamma.strata.market.curve.interpolator.BoundCurveInterpolator;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// A curve based on interpolation between a number of nodal points.
	/// <para>
	/// This class defines a curve in terms of a fixed number of nodes, referred to as <i>parameters</i>.
	/// </para>
	/// <para>
	/// Each node has an x-value and a y-value.
	/// The interface is focused on finding the y-value for a given x-value.
	/// An interpolator is used to find y-values for x-values between two nodes.
	/// Two extrapolators are used to find y-values, one when the x-value is to the left
	/// of the first node, and one where the x-value is to the right of the last node.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class InterpolatedNodalCurve implements NodalCurve, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class InterpolatedNodalCurve : NodalCurve, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CurveMetadata metadata;
		private readonly CurveMetadata metadata;
	  /// <summary>
	  /// The array of x-values, one for each point.
	  /// <para>
	  /// This array will contains at least two elements and be of the same length as y-values.
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
	  /// The interpolator.
	  /// This is used for x-values between the smallest and largest known x-value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator interpolator;
	  private readonly CurveInterpolator interpolator;
	  /// <summary>
	  /// The extrapolator for x-values on the left, defaulted to 'Flat".
	  /// This is used for x-values smaller than the smallest known x-value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator extrapolatorLeft;
	  private readonly CurveExtrapolator extrapolatorLeft;
	  /// <summary>
	  /// The extrapolator for x-values on the right, defaulted to 'Flat".
	  /// This is used for x-values larger than the largest known x-value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator extrapolatorRight;
	  private readonly CurveExtrapolator extrapolatorRight;
	  /// <summary>
	  /// The bound interpolator.
	  /// </summary>
	  [NonSerialized]
	  private readonly BoundCurveInterpolator boundInterpolator; // derived and cached, not a property
	  /// <summary>
	  /// The parameter metadata.
	  /// </summary>
	  [NonSerialized]
	  private readonly IList<ParameterMetadata> parameterMetadata; // derived, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an interpolated curve with metadata.
	  /// </summary>
	  /// <param name="metadata">  the curve metadata </param>
	  /// <param name="xValues">  the x-values </param>
	  /// <param name="yValues">  the y-values </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <returns> the curve </returns>
	  public static InterpolatedNodalCurve of(CurveMetadata metadata, DoubleArray xValues, DoubleArray yValues, CurveInterpolator interpolator)
	  {

		return InterpolatedNodalCurve.builder().metadata(metadata).xValues(xValues).yValues(yValues).interpolator(interpolator).build();
	  }

	  /// <summary>
	  /// Creates an interpolated curve with metadata.
	  /// </summary>
	  /// <param name="metadata">  the curve metadata </param>
	  /// <param name="xValues">  the x-values </param>
	  /// <param name="yValues">  the y-values </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <param name="extrapolatorLeft">  the extrapolator for extrapolating off the left-hand end of the curve </param>
	  /// <param name="extrapolatorRight">  the extrapolator for extrapolating off the right-hand end of the curve </param>
	  /// <returns> the curve </returns>
	  public static InterpolatedNodalCurve of(CurveMetadata metadata, DoubleArray xValues, DoubleArray yValues, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight)
	  {

		return InterpolatedNodalCurve.builder().metadata(metadata).xValues(xValues).yValues(yValues).interpolator(interpolator).extrapolatorLeft(extrapolatorLeft).extrapolatorRight(extrapolatorRight).build();
	  }

	  //-------------------------------------------------------------------------
	  // restricted constructor
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private InterpolatedNodalCurve(CurveMetadata metadata, com.opengamma.strata.collect.array.DoubleArray xValues, com.opengamma.strata.collect.array.DoubleArray yValues, com.opengamma.strata.market.curve.interpolator.CurveInterpolator interpolator, com.opengamma.strata.market.curve.interpolator.CurveExtrapolator extrapolatorLeft, com.opengamma.strata.market.curve.interpolator.CurveExtrapolator extrapolatorRight)
	  private InterpolatedNodalCurve(CurveMetadata metadata, DoubleArray xValues, DoubleArray yValues, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight)
	  {

		JodaBeanUtils.notNull(metadata, "metadata");
		JodaBeanUtils.notNull(xValues, "times");
		JodaBeanUtils.notNull(yValues, "values");
		JodaBeanUtils.notNull(interpolator, "interpolator");
		JodaBeanUtils.notNull(extrapolatorLeft, "extrapolatorLeft");
		JodaBeanUtils.notNull(extrapolatorRight, "extrapolatorRight");
		if (xValues.size() < 2)
		{
		  throw new System.ArgumentException("Length of x-values must be at least 2");
		}
		if (xValues.size() != yValues.size())
		{
		  throw new System.ArgumentException("Length of x-values and y-values must match");
		}
		metadata.ParameterMetadata.ifPresent(@params =>
		{
		if (xValues.size() != @params.size())
		{
			throw new System.ArgumentException("Length of x-values and parameter metadata must match when metadata present");
		}
		});
		for (int i = 1; i < xValues.size(); i++)
		{
		  if (xValues.get(i) <= xValues.get(i - 1))
		  {
			throw new System.ArgumentException("Array of x-values must be sorted and unique");
		  }
		}
		this.metadata = metadata;
		this.xValues = xValues;
		this.yValues = yValues;
		this.extrapolatorLeft = extrapolatorLeft;
		this.interpolator = interpolator;
		this.extrapolatorRight = extrapolatorRight;
		this.boundInterpolator = interpolator.bind(xValues, yValues, extrapolatorLeft, extrapolatorRight);
		this.parameterMetadata = IntStream.range(0, ParameterCount).mapToObj(i => getParameterMetadata(i)).collect(toImmutableList());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.extrapolatorLeft_Renamed = CurveExtrapolators.FLAT;
		builder.extrapolatorRight_Renamed = CurveExtrapolators.FLAT;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new InterpolatedNodalCurve(metadata, xValues, yValues, interpolator, extrapolatorLeft, extrapolatorRight);
	  }

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return yValues.size();
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return yValues.get(parameterIndex);
	  }

	  public InterpolatedNodalCurve withParameter(int parameterIndex, double newValue)
	  {
		return withYValues(yValues.with(parameterIndex, newValue));
	  }

	  public override InterpolatedNodalCurve withPerturbation(ParameterPerturbation perturbation)
	  {
		int size = yValues.size();
		DoubleArray perturbedValues = DoubleArray.of(size, i => perturbation(i, yValues.get(i), getParameterMetadata(i)));
		return withYValues(perturbedValues);
	  }

	  //-------------------------------------------------------------------------
	  public double yValue(double x)
	  {
		return boundInterpolator.interpolate(x);
	  }

	  public UnitParameterSensitivity yValueParameterSensitivity(double x)
	  {
		return createParameterSensitivity(boundInterpolator.parameterSensitivity(x));
	  }

	  public double firstDerivative(double x)
	  {
		return boundInterpolator.firstDerivative(x);
	  }

	  //-------------------------------------------------------------------------
	  public InterpolatedNodalCurve withMetadata(CurveMetadata metadata)
	  {
		return new InterpolatedNodalCurve(metadata, xValues, yValues, interpolator, extrapolatorLeft, extrapolatorRight);
	  }

	  public InterpolatedNodalCurve withYValues(DoubleArray yValues)
	  {
		return new InterpolatedNodalCurve(metadata, xValues, yValues, interpolator, extrapolatorLeft, extrapolatorRight);
	  }

	  public InterpolatedNodalCurve withValues(DoubleArray xValues, DoubleArray yValues)
	  {
		return new InterpolatedNodalCurve(metadata, xValues, yValues, interpolator, extrapolatorLeft, extrapolatorRight);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a new curve with an additional node, specifying the parameter metadata.
	  /// <para>
	  /// The result will contain the specified node.
	  /// If the x-value equals an existing x-value, the y-value will be changed.
	  /// If the x-value does not equal an existing x-value, the node will be added.
	  /// </para>
	  /// <para>
	  /// The result will only contain the specified parameter metadata if this curve also has parameter meta-data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the new x-value </param>
	  /// <param name="y">  the new y-value </param>
	  /// <param name="paramMetadata">  the new parameter metadata </param>
	  /// <returns> the updated curve </returns>
	  public InterpolatedNodalCurve withNode(double x, double y, ParameterMetadata paramMetadata)
	  {
		int index = Arrays.binarySearch(xValues.toArrayUnsafe(), x);
		if (index >= 0)
		{
		  CurveMetadata md = metadata.ParameterMetadata.map(@params =>
		  {
		  IList<ParameterMetadata> extended = new List<ParameterMetadata>(@params);
		  extended[index] = paramMetadata;
		  return metadata.withParameterMetadata(extended);
		  }).orElse(metadata);
		  DoubleArray yUpdated = yValues.with(index, y);
		  return new InterpolatedNodalCurve(md, xValues, yUpdated, interpolator, extrapolatorLeft, extrapolatorRight);
		}
		int insertion = -(index + 1);
		DoubleArray xExtended = xValues.subArray(0, insertion).concat(x).concat(xValues.subArray(insertion));
		DoubleArray yExtended = yValues.subArray(0, insertion).concat(y).concat(yValues.subArray(insertion));
		// add to existing metadata, or do nothing if no existing metadata
		CurveMetadata md = metadata.ParameterMetadata.map(@params =>
		{
		IList<ParameterMetadata> extended = new List<ParameterMetadata>(@params);
		extended.Insert(insertion, paramMetadata);
		return metadata.withParameterMetadata(extended);
		}).orElse(metadata);
		return new InterpolatedNodalCurve(md, xExtended, yExtended, interpolator, extrapolatorLeft, extrapolatorRight);
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
	  /// The meta-bean for {@code InterpolatedNodalCurve}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static InterpolatedNodalCurve.Meta meta()
	  {
		return InterpolatedNodalCurve.Meta.INSTANCE;
	  }

	  static InterpolatedNodalCurve()
	  {
		MetaBean.register(InterpolatedNodalCurve.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static InterpolatedNodalCurve.Builder builder()
	  {
		return new InterpolatedNodalCurve.Builder();
	  }

	  public override InterpolatedNodalCurve.Meta metaBean()
	  {
		return InterpolatedNodalCurve.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve metadata.
	  /// <para>
	  /// The metadata includes an optional list of parameter metadata.
	  /// If present, the size of the parameter metadata list will match the number of parameters of this curve.
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
	  /// Gets the array of x-values, one for each point.
	  /// <para>
	  /// This array will contains at least two elements and be of the same length as y-values.
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
	  /// Gets the interpolator.
	  /// This is used for x-values between the smallest and largest known x-value. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveInterpolator Interpolator
	  {
		  get
		  {
			return interpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the extrapolator for x-values on the left, defaulted to 'Flat".
	  /// This is used for x-values smaller than the smallest known x-value. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator ExtrapolatorLeft
	  {
		  get
		  {
			return extrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the extrapolator for x-values on the right, defaulted to 'Flat".
	  /// This is used for x-values larger than the largest known x-value. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator ExtrapolatorRight
	  {
		  get
		  {
			return extrapolatorRight;
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
		  InterpolatedNodalCurve other = (InterpolatedNodalCurve) obj;
		  return JodaBeanUtils.equal(metadata, other.metadata) && JodaBeanUtils.equal(xValues, other.xValues) && JodaBeanUtils.equal(yValues, other.yValues) && JodaBeanUtils.equal(interpolator, other.interpolator) && JodaBeanUtils.equal(extrapolatorLeft, other.extrapolatorLeft) && JodaBeanUtils.equal(extrapolatorRight, other.extrapolatorRight);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(metadata);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(xValues);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yValues);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(interpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(extrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(extrapolatorRight);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("InterpolatedNodalCurve{");
		buf.Append("metadata").Append('=').Append(metadata).Append(',').Append(' ');
		buf.Append("xValues").Append('=').Append(xValues).Append(',').Append(' ');
		buf.Append("yValues").Append('=').Append(yValues).Append(',').Append(' ');
		buf.Append("interpolator").Append('=').Append(interpolator).Append(',').Append(' ');
		buf.Append("extrapolatorLeft").Append('=').Append(extrapolatorLeft).Append(',').Append(' ');
		buf.Append("extrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(extrapolatorRight));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code InterpolatedNodalCurve}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  metadata_Renamed = DirectMetaProperty.ofImmutable(this, "metadata", typeof(InterpolatedNodalCurve), typeof(CurveMetadata));
			  xValues_Renamed = DirectMetaProperty.ofImmutable(this, "xValues", typeof(InterpolatedNodalCurve), typeof(DoubleArray));
			  yValues_Renamed = DirectMetaProperty.ofImmutable(this, "yValues", typeof(InterpolatedNodalCurve), typeof(DoubleArray));
			  interpolator_Renamed = DirectMetaProperty.ofImmutable(this, "interpolator", typeof(InterpolatedNodalCurve), typeof(CurveInterpolator));
			  extrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "extrapolatorLeft", typeof(InterpolatedNodalCurve), typeof(CurveExtrapolator));
			  extrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "extrapolatorRight", typeof(InterpolatedNodalCurve), typeof(CurveExtrapolator));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "metadata", "xValues", "yValues", "interpolator", "extrapolatorLeft", "extrapolatorRight");
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
		/// The meta-property for the {@code interpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> interpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code extrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> extrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code extrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> extrapolatorRight_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "metadata", "xValues", "yValues", "interpolator", "extrapolatorLeft", "extrapolatorRight");
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
			case 2096253127: // interpolator
			  return interpolator_Renamed;
			case 1271703994: // extrapolatorLeft
			  return extrapolatorLeft_Renamed;
			case 773779145: // extrapolatorRight
			  return extrapolatorRight_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override InterpolatedNodalCurve.Builder builder()
		{
		  return new InterpolatedNodalCurve.Builder();
		}

		public override Type beanType()
		{
		  return typeof(InterpolatedNodalCurve);
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
		/// The meta-property for the {@code interpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> interpolator()
		{
		  return interpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code extrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> extrapolatorLeft()
		{
		  return extrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code extrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> extrapolatorRight()
		{
		  return extrapolatorRight_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  return ((InterpolatedNodalCurve) bean).Metadata;
			case 1681280954: // xValues
			  return ((InterpolatedNodalCurve) bean).XValues;
			case -1726182661: // yValues
			  return ((InterpolatedNodalCurve) bean).YValues;
			case 2096253127: // interpolator
			  return ((InterpolatedNodalCurve) bean).Interpolator;
			case 1271703994: // extrapolatorLeft
			  return ((InterpolatedNodalCurve) bean).ExtrapolatorLeft;
			case 773779145: // extrapolatorRight
			  return ((InterpolatedNodalCurve) bean).ExtrapolatorRight;
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
	  /// The bean-builder for {@code InterpolatedNodalCurve}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<InterpolatedNodalCurve>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveMetadata metadata_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DoubleArray xValues_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DoubleArray yValues_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveInterpolator interpolator_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator extrapolatorLeft_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator extrapolatorRight_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(InterpolatedNodalCurve beanToCopy)
		{
		  this.metadata_Renamed = beanToCopy.Metadata;
		  this.xValues_Renamed = beanToCopy.XValues;
		  this.yValues_Renamed = beanToCopy.YValues;
		  this.interpolator_Renamed = beanToCopy.Interpolator;
		  this.extrapolatorLeft_Renamed = beanToCopy.ExtrapolatorLeft;
		  this.extrapolatorRight_Renamed = beanToCopy.ExtrapolatorRight;
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
			case 2096253127: // interpolator
			  return interpolator_Renamed;
			case 1271703994: // extrapolatorLeft
			  return extrapolatorLeft_Renamed;
			case 773779145: // extrapolatorRight
			  return extrapolatorRight_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  this.metadata_Renamed = (CurveMetadata) newValue;
			  break;
			case 1681280954: // xValues
			  this.xValues_Renamed = (DoubleArray) newValue;
			  break;
			case -1726182661: // yValues
			  this.yValues_Renamed = (DoubleArray) newValue;
			  break;
			case 2096253127: // interpolator
			  this.interpolator_Renamed = (CurveInterpolator) newValue;
			  break;
			case 1271703994: // extrapolatorLeft
			  this.extrapolatorLeft_Renamed = (CurveExtrapolator) newValue;
			  break;
			case 773779145: // extrapolatorRight
			  this.extrapolatorRight_Renamed = (CurveExtrapolator) newValue;
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

		public override InterpolatedNodalCurve build()
		{
		  return new InterpolatedNodalCurve(metadata_Renamed, xValues_Renamed, yValues_Renamed, interpolator_Renamed, extrapolatorLeft_Renamed, extrapolatorRight_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the curve metadata.
		/// <para>
		/// The metadata includes an optional list of parameter metadata.
		/// If present, the size of the parameter metadata list will match the number of parameters of this curve.
		/// </para>
		/// </summary>
		/// <param name="metadata">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder metadata(CurveMetadata metadata)
		{
		  JodaBeanUtils.notNull(metadata, "metadata");
		  this.metadata_Renamed = metadata;
		  return this;
		}

		/// <summary>
		/// Sets the array of x-values, one for each point.
		/// <para>
		/// This array will contains at least two elements and be of the same length as y-values.
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
		/// Sets the interpolator.
		/// This is used for x-values between the smallest and largest known x-value. </summary>
		/// <param name="interpolator">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder interpolator(CurveInterpolator interpolator)
		{
		  JodaBeanUtils.notNull(interpolator, "interpolator");
		  this.interpolator_Renamed = interpolator;
		  return this;
		}

		/// <summary>
		/// Sets the extrapolator for x-values on the left, defaulted to 'Flat".
		/// This is used for x-values smaller than the smallest known x-value. </summary>
		/// <param name="extrapolatorLeft">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder extrapolatorLeft(CurveExtrapolator extrapolatorLeft)
		{
		  JodaBeanUtils.notNull(extrapolatorLeft, "extrapolatorLeft");
		  this.extrapolatorLeft_Renamed = extrapolatorLeft;
		  return this;
		}

		/// <summary>
		/// Sets the extrapolator for x-values on the right, defaulted to 'Flat".
		/// This is used for x-values larger than the largest known x-value. </summary>
		/// <param name="extrapolatorRight">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder extrapolatorRight(CurveExtrapolator extrapolatorRight)
		{
		  JodaBeanUtils.notNull(extrapolatorRight, "extrapolatorRight");
		  this.extrapolatorRight_Renamed = extrapolatorRight;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("InterpolatedNodalCurve.Builder{");
		  buf.Append("metadata").Append('=').Append(JodaBeanUtils.ToString(metadata_Renamed)).Append(',').Append(' ');
		  buf.Append("xValues").Append('=').Append(JodaBeanUtils.ToString(xValues_Renamed)).Append(',').Append(' ');
		  buf.Append("yValues").Append('=').Append(JodaBeanUtils.ToString(yValues_Renamed)).Append(',').Append(' ');
		  buf.Append("interpolator").Append('=').Append(JodaBeanUtils.ToString(interpolator_Renamed)).Append(',').Append(' ');
		  buf.Append("extrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(extrapolatorLeft_Renamed)).Append(',').Append(' ');
		  buf.Append("extrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(extrapolatorRight_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}