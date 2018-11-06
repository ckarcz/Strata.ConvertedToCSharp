using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// A curve based on a parameterized function.
	/// <para>
	/// This class defines a curve in terms of a function and its parameters.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ParameterizedFunctionalCurve implements Curve, org.joda.beans.ImmutableBean
	public sealed class ParameterizedFunctionalCurve : Curve, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CurveMetadata metadata;
		private readonly CurveMetadata metadata;
	  /// <summary>
	  /// The array of parameters for the curve function.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray parameters;
	  private readonly DoubleArray parameters;
	  /// <summary>
	  /// The y-value function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns y-value.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final System.Func<com.opengamma.strata.collect.array.DoubleArray, double, double> valueFunction;
	  private readonly System.Func<DoubleArray, double, double> valueFunction;
	  /// <summary>
	  /// The derivative function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns the first derivative of y-value with respective to x, 
	  /// i.e., the gradient of the curve.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final System.Func<com.opengamma.strata.collect.array.DoubleArray, double, double> derivativeFunction;
	  private readonly System.Func<DoubleArray, double, double> derivativeFunction;
	  /// <summary>
	  /// The parameter sensitivity function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns the sensitivities of y-value to the parameters.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final System.Func<com.opengamma.strata.collect.array.DoubleArray, double, com.opengamma.strata.collect.array.DoubleArray> sensitivityFunction;
	  private readonly System.Func<DoubleArray, double, DoubleArray> sensitivityFunction;
	  /// <summary>
	  /// The parameter metadata.
	  /// </summary>
	  [NonSerialized]
	  private readonly IList<ParameterMetadata> parameterMetadata; // derived, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance.
	  /// </summary>
	  /// <param name="metadata">  the metadata </param>
	  /// <param name="parameters">  the parameters </param>
	  /// <param name="valueFunction">  the value function </param>
	  /// <param name="derivativeFunction">  the derivative function </param>
	  /// <param name="sensitivityFunction">  the parameter sensitivity function </param>
	  /// <returns> the instance </returns>
	  public static ParameterizedFunctionalCurve of(CurveMetadata metadata, DoubleArray parameters, System.Func<DoubleArray, double, double> valueFunction, System.Func<DoubleArray, double, double> derivativeFunction, System.Func<DoubleArray, double, DoubleArray> sensitivityFunction)
	  {

		return ParameterizedFunctionalCurve.builder().metadata(metadata).parameters(parameters).valueFunction(valueFunction).derivativeFunction(derivativeFunction).sensitivityFunction(sensitivityFunction).build();
	  }

	  // restricted constructor
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private ParameterizedFunctionalCurve(CurveMetadata metadata, com.opengamma.strata.collect.array.DoubleArray parameters, System.Func<com.opengamma.strata.collect.array.DoubleArray, double, double> valueFunction, System.Func<com.opengamma.strata.collect.array.DoubleArray, double, double> derivativeFunction, System.Func<com.opengamma.strata.collect.array.DoubleArray, double, com.opengamma.strata.collect.array.DoubleArray> sensitivityFunction)
	  private ParameterizedFunctionalCurve(CurveMetadata metadata, DoubleArray parameters, System.Func<DoubleArray, double, double> valueFunction, System.Func<DoubleArray, double, double> derivativeFunction, System.Func<DoubleArray, double, DoubleArray> sensitivityFunction)
	  {

		JodaBeanUtils.notNull(metadata, "metadata");
		JodaBeanUtils.notNull(parameters, "parameters");
		JodaBeanUtils.notNull(valueFunction, "valueFunction");
		JodaBeanUtils.notNull(derivativeFunction, "derivativeFunction");
		JodaBeanUtils.notNull(sensitivityFunction, "sensitivityFunction");
		this.metadata = metadata;
		this.parameters = parameters;
		this.valueFunction = valueFunction;
		this.derivativeFunction = derivativeFunction;
		this.sensitivityFunction = sensitivityFunction;
		this.parameterMetadata = IntStream.range(0, ParameterCount).mapToObj(i => getParameterMetadata(i)).collect(toImmutableList());
	  }

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return parameters.size();
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return parameters.get(parameterIndex);
	  }

	  public ParameterizedFunctionalCurve withParameter(int parameterIndex, double newValue)
	  {
		return withParameters(parameters.with(parameterIndex, newValue));
	  }

	  public override ParameterizedFunctionalCurve withPerturbation(ParameterPerturbation perturbation)
	  {
		int size = parameters.size();
		DoubleArray perturbedValues = DoubleArray.of(size, i => perturbation(i, parameters.get(i), getParameterMetadata(i)));
		return withParameters(perturbedValues);
	  }

	  //-------------------------------------------------------------------------
	  public double yValue(double x)
	  {
		return valueFunction.apply(parameters, x);
	  }

	  public UnitParameterSensitivity yValueParameterSensitivity(double x)
	  {
		return createParameterSensitivity(sensitivityFunction.apply(parameters, x));
	  }

	  public double firstDerivative(double x)
	  {
		return derivativeFunction.apply(parameters, x);
	  }

	  //-------------------------------------------------------------------------
	  public ParameterizedFunctionalCurve withMetadata(CurveMetadata metadata)
	  {
		return new ParameterizedFunctionalCurve(metadata, parameters, valueFunction, derivativeFunction, sensitivityFunction);
	  }

	  /// <summary>
	  /// Returns a copy of the curve with all of the parameters altered. 
	  /// <para>
	  /// This instance is immutable and unaffected by this method call.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameters">  the new parameters </param>
	  /// <returns> the curve with the parameters altered </returns>
	  public ParameterizedFunctionalCurve withParameters(DoubleArray parameters)
	  {
		ArgChecker.isTrue(parameters.size() == this.parameters.size(), "the new parameters size must be the same as the initial parameter size");
		return new ParameterizedFunctionalCurve(metadata, parameters, valueFunction, derivativeFunction, sensitivityFunction);
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
	  /// The meta-bean for {@code ParameterizedFunctionalCurve}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ParameterizedFunctionalCurve.Meta meta()
	  {
		return ParameterizedFunctionalCurve.Meta.INSTANCE;
	  }

	  static ParameterizedFunctionalCurve()
	  {
		MetaBean.register(ParameterizedFunctionalCurve.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ParameterizedFunctionalCurve.Builder builder()
	  {
		return new ParameterizedFunctionalCurve.Builder();
	  }

	  public override ParameterizedFunctionalCurve.Meta metaBean()
	  {
		return ParameterizedFunctionalCurve.Meta.INSTANCE;
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
	  /// Gets the array of parameters for the curve function. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray Parameters
	  {
		  get
		  {
			return parameters;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the y-value function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns y-value.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public System.Func<DoubleArray, double, double> ValueFunction
	  {
		  get
		  {
			return valueFunction;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the derivative function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns the first derivative of y-value with respective to x,
	  /// i.e., the gradient of the curve.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public System.Func<DoubleArray, double, double> DerivativeFunction
	  {
		  get
		  {
			return derivativeFunction;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameter sensitivity function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns the sensitivities of y-value to the parameters.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public System.Func<DoubleArray, double, DoubleArray> SensitivityFunction
	  {
		  get
		  {
			return sensitivityFunction;
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
		  ParameterizedFunctionalCurve other = (ParameterizedFunctionalCurve) obj;
		  return JodaBeanUtils.equal(metadata, other.metadata) && JodaBeanUtils.equal(parameters, other.parameters) && JodaBeanUtils.equal(valueFunction, other.valueFunction) && JodaBeanUtils.equal(derivativeFunction, other.derivativeFunction) && JodaBeanUtils.equal(sensitivityFunction, other.sensitivityFunction);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(metadata);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameters);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valueFunction);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(derivativeFunction);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivityFunction);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("ParameterizedFunctionalCurve{");
		buf.Append("metadata").Append('=').Append(metadata).Append(',').Append(' ');
		buf.Append("parameters").Append('=').Append(parameters).Append(',').Append(' ');
		buf.Append("valueFunction").Append('=').Append(valueFunction).Append(',').Append(' ');
		buf.Append("derivativeFunction").Append('=').Append(derivativeFunction).Append(',').Append(' ');
		buf.Append("sensitivityFunction").Append('=').Append(JodaBeanUtils.ToString(sensitivityFunction));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ParameterizedFunctionalCurve}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  metadata_Renamed = DirectMetaProperty.ofImmutable(this, "metadata", typeof(ParameterizedFunctionalCurve), typeof(CurveMetadata));
			  parameters_Renamed = DirectMetaProperty.ofImmutable(this, "parameters", typeof(ParameterizedFunctionalCurve), typeof(DoubleArray));
			  valueFunction_Renamed = DirectMetaProperty.ofImmutable(this, "valueFunction", typeof(ParameterizedFunctionalCurve), (Type) typeof(System.Func));
			  derivativeFunction_Renamed = DirectMetaProperty.ofImmutable(this, "derivativeFunction", typeof(ParameterizedFunctionalCurve), (Type) typeof(System.Func));
			  sensitivityFunction_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivityFunction", typeof(ParameterizedFunctionalCurve), (Type) typeof(System.Func));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "metadata", "parameters", "valueFunction", "derivativeFunction", "sensitivityFunction");
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
		/// The meta-property for the {@code parameters} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> parameters_Renamed;
		/// <summary>
		/// The meta-property for the {@code valueFunction} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<System.Func<com.opengamma.strata.collect.array.DoubleArray, double, double>> valueFunction = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "valueFunction", ParameterizedFunctionalCurve.class, (Class) System.Func.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<System.Func<DoubleArray, double, double>> valueFunction_Renamed = DirectMetaProperty.ofImmutable(this, "valueFunction", typeof(ParameterizedFunctionalCurve), (Type) typeof(System.Func));
		/// <summary>
		/// The meta-property for the {@code derivativeFunction} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<System.Func<com.opengamma.strata.collect.array.DoubleArray, double, double>> derivativeFunction = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "derivativeFunction", ParameterizedFunctionalCurve.class, (Class) System.Func.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<System.Func<DoubleArray, double, double>> derivativeFunction_Renamed = DirectMetaProperty.ofImmutable(this, "derivativeFunction", typeof(ParameterizedFunctionalCurve), (Type) typeof(System.Func));
		/// <summary>
		/// The meta-property for the {@code sensitivityFunction} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<System.Func<com.opengamma.strata.collect.array.DoubleArray, double, com.opengamma.strata.collect.array.DoubleArray>> sensitivityFunction = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "sensitivityFunction", ParameterizedFunctionalCurve.class, (Class) System.Func.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<System.Func<DoubleArray, double, DoubleArray>> sensitivityFunction_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivityFunction", typeof(ParameterizedFunctionalCurve), (Type) typeof(System.Func));
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "metadata", "parameters", "valueFunction", "derivativeFunction", "sensitivityFunction");
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
			case 458736106: // parameters
			  return parameters_Renamed;
			case 636119145: // valueFunction
			  return valueFunction_Renamed;
			case 1663351423: // derivativeFunction
			  return derivativeFunction_Renamed;
			case -1353652329: // sensitivityFunction
			  return sensitivityFunction_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ParameterizedFunctionalCurve.Builder builder()
		{
		  return new ParameterizedFunctionalCurve.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ParameterizedFunctionalCurve);
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
		/// The meta-property for the {@code parameters} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> parameters()
		{
		  return parameters_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code valueFunction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<System.Func<DoubleArray, double, double>> valueFunction()
		{
		  return valueFunction_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code derivativeFunction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<System.Func<DoubleArray, double, double>> derivativeFunction()
		{
		  return derivativeFunction_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code sensitivityFunction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<System.Func<DoubleArray, double, DoubleArray>> sensitivityFunction()
		{
		  return sensitivityFunction_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  return ((ParameterizedFunctionalCurve) bean).Metadata;
			case 458736106: // parameters
			  return ((ParameterizedFunctionalCurve) bean).Parameters;
			case 636119145: // valueFunction
			  return ((ParameterizedFunctionalCurve) bean).ValueFunction;
			case 1663351423: // derivativeFunction
			  return ((ParameterizedFunctionalCurve) bean).DerivativeFunction;
			case -1353652329: // sensitivityFunction
			  return ((ParameterizedFunctionalCurve) bean).SensitivityFunction;
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
	  /// The bean-builder for {@code ParameterizedFunctionalCurve}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ParameterizedFunctionalCurve>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveMetadata metadata_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DoubleArray parameters_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal System.Func<DoubleArray, double, double> valueFunction_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal System.Func<DoubleArray, double, double> derivativeFunction_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal System.Func<DoubleArray, double, DoubleArray> sensitivityFunction_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ParameterizedFunctionalCurve beanToCopy)
		{
		  this.metadata_Renamed = beanToCopy.Metadata;
		  this.parameters_Renamed = beanToCopy.Parameters;
		  this.valueFunction_Renamed = beanToCopy.ValueFunction;
		  this.derivativeFunction_Renamed = beanToCopy.DerivativeFunction;
		  this.sensitivityFunction_Renamed = beanToCopy.SensitivityFunction;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -450004177: // metadata
			  return metadata_Renamed;
			case 458736106: // parameters
			  return parameters_Renamed;
			case 636119145: // valueFunction
			  return valueFunction_Renamed;
			case 1663351423: // derivativeFunction
			  return derivativeFunction_Renamed;
			case -1353652329: // sensitivityFunction
			  return sensitivityFunction_Renamed;
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
			  this.metadata_Renamed = (CurveMetadata) newValue;
			  break;
			case 458736106: // parameters
			  this.parameters_Renamed = (DoubleArray) newValue;
			  break;
			case 636119145: // valueFunction
			  this.valueFunction_Renamed = (System.Func<DoubleArray, double, double>) newValue;
			  break;
			case 1663351423: // derivativeFunction
			  this.derivativeFunction_Renamed = (System.Func<DoubleArray, double, double>) newValue;
			  break;
			case -1353652329: // sensitivityFunction
			  this.sensitivityFunction_Renamed = (System.Func<DoubleArray, double, DoubleArray>) newValue;
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

		public override ParameterizedFunctionalCurve build()
		{
		  return new ParameterizedFunctionalCurve(metadata_Renamed, parameters_Renamed, valueFunction_Renamed, derivativeFunction_Renamed, sensitivityFunction_Renamed);
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
		/// Sets the array of parameters for the curve function. </summary>
		/// <param name="parameters">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder parameters(DoubleArray parameters)
		{
		  JodaBeanUtils.notNull(parameters, "parameters");
		  this.parameters_Renamed = parameters;
		  return this;
		}

		/// <summary>
		/// Sets the y-value function.
		/// <para>
		/// The function takes {@code parameters} and x-value, then returns y-value.
		/// </para>
		/// </summary>
		/// <param name="valueFunction">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder valueFunction(System.Func<DoubleArray, double, double> valueFunction)
		{
		  JodaBeanUtils.notNull(valueFunction, "valueFunction");
		  this.valueFunction_Renamed = valueFunction;
		  return this;
		}

		/// <summary>
		/// Sets the derivative function.
		/// <para>
		/// The function takes {@code parameters} and x-value, then returns the first derivative of y-value with respective to x,
		/// i.e., the gradient of the curve.
		/// </para>
		/// </summary>
		/// <param name="derivativeFunction">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder derivativeFunction(System.Func<DoubleArray, double, double> derivativeFunction)
		{
		  JodaBeanUtils.notNull(derivativeFunction, "derivativeFunction");
		  this.derivativeFunction_Renamed = derivativeFunction;
		  return this;
		}

		/// <summary>
		/// Sets the parameter sensitivity function.
		/// <para>
		/// The function takes {@code parameters} and x-value, then returns the sensitivities of y-value to the parameters.
		/// </para>
		/// </summary>
		/// <param name="sensitivityFunction">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder sensitivityFunction(System.Func<DoubleArray, double, DoubleArray> sensitivityFunction)
		{
		  JodaBeanUtils.notNull(sensitivityFunction, "sensitivityFunction");
		  this.sensitivityFunction_Renamed = sensitivityFunction;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("ParameterizedFunctionalCurve.Builder{");
		  buf.Append("metadata").Append('=').Append(JodaBeanUtils.ToString(metadata_Renamed)).Append(',').Append(' ');
		  buf.Append("parameters").Append('=').Append(JodaBeanUtils.ToString(parameters_Renamed)).Append(',').Append(' ');
		  buf.Append("valueFunction").Append('=').Append(JodaBeanUtils.ToString(valueFunction_Renamed)).Append(',').Append(' ');
		  buf.Append("derivativeFunction").Append('=').Append(JodaBeanUtils.ToString(derivativeFunction_Renamed)).Append(',').Append(' ');
		  buf.Append("sensitivityFunction").Append('=').Append(JodaBeanUtils.ToString(sensitivityFunction_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}