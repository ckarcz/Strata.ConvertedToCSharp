using System;
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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// A curve based on a single constant value.
	/// <para>
	/// This class defines a curve in terms of a single node point. 
	/// The resulting curve is a constant curve with the y-value of the node point.
	/// When queried, <seealso cref="#yValue(double)"/> always returns the constant value.
	/// The x-value is not significant in most use cases.
	/// See <seealso cref="ConstantCurve"/> for an alternative that does not have an x-value.
	/// </para>
	/// <para>
	/// The <seealso cref="#getXValues()"/> method returns the single x-value of the node.
	/// The <seealso cref="#getYValues()"/> method returns the single y-value of the node.
	/// The sensitivity is 1 and the first derivative is 0.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ConstantNodalCurve implements NodalCurve, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ConstantNodalCurve : NodalCurve, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CurveMetadata metadata;
		private readonly CurveMetadata metadata;
	  /// <summary>
	  /// The single x-value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final double xValue;
	  private readonly double xValue;
	  /// <summary>
	  /// The single y-value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final double yValue;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly double yValue_Renamed;
	  /// <summary>
	  /// The parameter metadata.
	  /// </summary>
	  [NonSerialized]
	  private readonly IList<ParameterMetadata> parameterMetadata; // derived, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a constant nodal curve with metadata.
	  /// <para>
	  /// The curve is defined by a single x and y value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="metadata">  the curve metadata </param>
	  /// <param name="xValue">  the x-value </param>
	  /// <param name="yValue">  the y-value </param>
	  /// <returns> the curve </returns>
	  public static ConstantNodalCurve of(CurveMetadata metadata, double xValue, double yValue)
	  {
		return new ConstantNodalCurve(metadata, xValue, yValue);
	  }

	  //-------------------------------------------------------------------------
	  // restricted constructor
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private ConstantNodalCurve(CurveMetadata metadata, double xValue, double yValue)
	  private ConstantNodalCurve(CurveMetadata metadata, double xValue, double yValue)
	  {
		JodaBeanUtils.notNull(metadata, "metadata");
		metadata.ParameterMetadata.ifPresent(@params =>
		{
		if (@params.size() != 1)
		{
			throw new System.ArgumentException("Length of parameter metadata must be 1");
		}
		});
		this.metadata = metadata;
		this.xValue = xValue;
		this.yValue_Renamed = yValue;
		this.parameterMetadata = ImmutableList.of(getParameterMetadata(0));
	  }

	  // resolve after deserialization
	  private object readResolve()
	  {
		return new ConstantNodalCurve(metadata, xValue, yValue_Renamed);
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
		ArgChecker.isTrue(parameterIndex == 0, "single parameter");
		return yValue_Renamed;
	  }

	  public ConstantNodalCurve withParameter(int parameterIndex, double newValue)
	  {
		ArgChecker.isTrue(parameterIndex == 0, "single parameter");
		return new ConstantNodalCurve(metadata, xValue, newValue);
	  }

	  public override ConstantNodalCurve withPerturbation(ParameterPerturbation perturbation)
	  {
		double perturbedValue = perturbation(0, yValue_Renamed, getParameterMetadata(0));
		return new ConstantNodalCurve(metadata, xValue, perturbedValue);
	  }

	  //-------------------------------------------------------------------------
	  public DoubleArray XValues
	  {
		  get
		  {
			return DoubleArray.of(xValue);
		  }
	  }

	  public DoubleArray YValues
	  {
		  get
		  {
			return DoubleArray.of(yValue_Renamed);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public double yValue(double x)
	  {
		return yValue_Renamed;
	  }

	  public UnitParameterSensitivity yValueParameterSensitivity(double x)
	  {
		return createParameterSensitivity(DoubleArray.of(1d));
	  }

	  public double firstDerivative(double x)
	  {
		return 0d;
	  }

	  //-------------------------------------------------------------------------
	  public ConstantNodalCurve withMetadata(CurveMetadata metadata)
	  {
		return new ConstantNodalCurve(metadata, xValue, yValue_Renamed);
	  }

	  public ConstantNodalCurve withYValues(DoubleArray yValues)
	  {
		ArgChecker.isTrue(yValues.size() == 1, "Invalid number of parameters, only one allowed");
		return new ConstantNodalCurve(metadata, xValue, yValues.get(0));
	  }

	  public ConstantNodalCurve withValues(DoubleArray xValues, DoubleArray yValues)
	  {
		ArgChecker.isTrue(xValues.size() == 1 && yValues.size() == 1, "Invalid number of parameters, only one allowed");
		return new ConstantNodalCurve(metadata, xValues.get(0), yValues.get(0));
	  }

	  //-------------------------------------------------------------------------
	  public ConstantNodalCurve withNode(double x, double y, ParameterMetadata paramMetadata)
	  {
		ArgChecker.isTrue(x == xValue, "x should be equal to the existing x-value");
		CurveMetadata md = metadata.withParameterMetadata(ImmutableList.of(paramMetadata));
		return new ConstantNodalCurve(md, x, y);
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
	  /// The meta-bean for {@code ConstantNodalCurve}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ConstantNodalCurve.Meta meta()
	  {
		return ConstantNodalCurve.Meta.INSTANCE;
	  }

	  static ConstantNodalCurve()
	  {
		MetaBean.register(ConstantNodalCurve.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ConstantNodalCurve.Builder builder()
	  {
		return new ConstantNodalCurve.Builder();
	  }

	  public override ConstantNodalCurve.Meta metaBean()
	  {
		return ConstantNodalCurve.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve metadata.
	  /// <para>
	  /// The metadata will have a single parameter metadata.
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
	  /// Gets the single x-value. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public double XValue
	  {
		  get
		  {
			return xValue;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the single y-value. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public double YValue
	  {
		  get
		  {
			return yValue_Renamed;
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
		  ConstantNodalCurve other = (ConstantNodalCurve) obj;
		  return JodaBeanUtils.equal(metadata, other.metadata) && JodaBeanUtils.equal(xValue, other.xValue) && JodaBeanUtils.equal(yValue_Renamed, other.yValue_Renamed);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(metadata);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(xValue);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yValue_Renamed);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("ConstantNodalCurve{");
		buf.Append("metadata").Append('=').Append(metadata).Append(',').Append(' ');
		buf.Append("xValue").Append('=').Append(xValue).Append(',').Append(' ');
		buf.Append("yValue").Append('=').Append(JodaBeanUtils.ToString(yValue_Renamed));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ConstantNodalCurve}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  metadata_Renamed = DirectMetaProperty.ofImmutable(this, "metadata", typeof(ConstantNodalCurve), typeof(CurveMetadata));
			  xValue_Renamed = DirectMetaProperty.ofImmutable(this, "xValue", typeof(ConstantNodalCurve), Double.TYPE);
			  yValue_Renamed = DirectMetaProperty.ofImmutable(this, "yValue", typeof(ConstantNodalCurve), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "metadata", "xValue", "yValue");
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
		/// The meta-property for the {@code xValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> xValue_Renamed;
		/// <summary>
		/// The meta-property for the {@code yValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> yValue_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "metadata", "xValue", "yValue");
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
			case -777049127: // xValue
			  return xValue_Renamed;
			case -748419976: // yValue
			  return yValue_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ConstantNodalCurve.Builder builder()
		{
		  return new ConstantNodalCurve.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ConstantNodalCurve);
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
		/// The meta-property for the {@code xValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> xValue()
		{
		  return xValue_Renamed;
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
			  return ((ConstantNodalCurve) bean).Metadata;
			case -777049127: // xValue
			  return ((ConstantNodalCurve) bean).XValue;
			case -748419976: // yValue
			  return ((ConstantNodalCurve) bean).YValue;
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
	  /// The bean-builder for {@code ConstantNodalCurve}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ConstantNodalCurve>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveMetadata metadata_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double xValue_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double yValue_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ConstantNodalCurve beanToCopy)
		{
		  this.metadata_Renamed = beanToCopy.Metadata;
		  this.xValue_Renamed = beanToCopy.XValue;
		  this.yValue_Renamed = beanToCopy.YValue;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  return metadata_Renamed;
			case -777049127: // xValue
			  return xValue_Renamed;
			case -748419976: // yValue
			  return yValue_Renamed;
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
			case -777049127: // xValue
			  this.xValue_Renamed = (double?) newValue.Value;
			  break;
			case -748419976: // yValue
			  this.yValue_Renamed = (double?) newValue.Value;
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

		public override ConstantNodalCurve build()
		{
		  return new ConstantNodalCurve(metadata_Renamed, xValue_Renamed, yValue_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the curve metadata.
		/// <para>
		/// The metadata will have a single parameter metadata.
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
		/// Sets the single x-value. </summary>
		/// <param name="xValue">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder xValue(double xValue)
		{
		  JodaBeanUtils.notNull(xValue, "xValue");
		  this.xValue_Renamed = xValue;
		  return this;
		}

		/// <summary>
		/// Sets the single y-value. </summary>
		/// <param name="yValue">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder yValue(double yValue)
		{
		  JodaBeanUtils.notNull(yValue, "yValue");
		  this.yValue_Renamed = yValue;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("ConstantNodalCurve.Builder{");
		  buf.Append("metadata").Append('=').Append(JodaBeanUtils.ToString(metadata_Renamed)).Append(',').Append(' ');
		  buf.Append("xValue").Append('=').Append(JodaBeanUtils.ToString(xValue_Renamed)).Append(',').Append(' ');
		  buf.Append("yValue").Append('=').Append(JodaBeanUtils.ToString(yValue_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}