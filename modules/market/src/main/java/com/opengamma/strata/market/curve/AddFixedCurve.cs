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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// A curve formed from two curves, the fixed curve and the spread curve.
	/// <para>
	/// The spread curve is the primary curve, providing the metadata, parameters, sensitivity and perturbation.
	/// The fixed curve only affects the shape of the curve via the <seealso cref="#yValue(double)"/>
	/// and <seealso cref="#firstDerivative(double)"/> methods. The fixed curve is not exposed in the parameters.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class AddFixedCurve implements Curve, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class AddFixedCurve : Curve, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Curve fixedCurve;
		private readonly Curve fixedCurve;
	  /// <summary>
	  /// The spread curve. Also called the variable curve.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Curve spreadCurve;
	  private readonly Curve spreadCurve;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a curve as the sum of a fixed curve and a spread curve.
	  /// </summary>
	  /// <param name="fixedCurve">  the fixed curve </param>
	  /// <param name="spreadCurve">  the spread curve </param>
	  /// <returns> the curve </returns>
	  public static AddFixedCurve of(Curve fixedCurve, Curve spreadCurve)
	  {
		return new AddFixedCurve(fixedCurve, spreadCurve);
	  }

	  //-------------------------------------------------------------------------
	  public CurveMetadata Metadata
	  {
		  get
		  {
			return spreadCurve.Metadata;
		  }
	  }

	  public AddFixedCurve withMetadata(CurveMetadata metadata)
	  {
		return new AddFixedCurve(fixedCurve, spreadCurve.withMetadata(metadata));
	  }

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return spreadCurve.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return spreadCurve.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return spreadCurve.getParameterMetadata(parameterIndex);
	  }

	  public AddFixedCurve withParameter(int parameterIndex, double newValue)
	  {
		return new AddFixedCurve(fixedCurve, spreadCurve.withParameter(parameterIndex, newValue));
	  }

	  public override AddFixedCurve withPerturbation(ParameterPerturbation perturbation)
	  {
		return new AddFixedCurve(fixedCurve, spreadCurve.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double yValue(double x)
	  {
		return fixedCurve.yValue(x) + spreadCurve.yValue(x);
	  }

	  public UnitParameterSensitivity yValueParameterSensitivity(double x)
	  {
		return spreadCurve.yValueParameterSensitivity(x);
	  }

	  public double firstDerivative(double x)
	  {
		return fixedCurve.firstDerivative(x) + spreadCurve.firstDerivative(x);
	  }

	  //-------------------------------------------------------------------------
	  public override ImmutableList<Curve> split()
	  {
		return ImmutableList.of(fixedCurve, spreadCurve);
	  }

	  public override AddFixedCurve withUnderlyingCurve(int curveIndex, Curve curve)
	  {
		if (curveIndex == 0)
		{
		  return new AddFixedCurve(curve, spreadCurve);
		}
		if (curveIndex == 1)
		{
		  return new AddFixedCurve(fixedCurve, curve);
		}
		throw new System.ArgumentException("curveIndex is outside the range");
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code AddFixedCurve}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static AddFixedCurve.Meta meta()
	  {
		return AddFixedCurve.Meta.INSTANCE;
	  }

	  static AddFixedCurve()
	  {
		MetaBean.register(AddFixedCurve.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private AddFixedCurve(Curve fixedCurve, Curve spreadCurve)
	  {
		JodaBeanUtils.notNull(fixedCurve, "fixedCurve");
		JodaBeanUtils.notNull(spreadCurve, "spreadCurve");
		this.fixedCurve = fixedCurve;
		this.spreadCurve = spreadCurve;
	  }

	  public override AddFixedCurve.Meta metaBean()
	  {
		return AddFixedCurve.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed curve. Also called base or shape curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve FixedCurve
	  {
		  get
		  {
			return fixedCurve;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the spread curve. Also called the variable curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve SpreadCurve
	  {
		  get
		  {
			return spreadCurve;
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
		  AddFixedCurve other = (AddFixedCurve) obj;
		  return JodaBeanUtils.equal(fixedCurve, other.fixedCurve) && JodaBeanUtils.equal(spreadCurve, other.spreadCurve);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spreadCurve);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("AddFixedCurve{");
		buf.Append("fixedCurve").Append('=').Append(fixedCurve).Append(',').Append(' ');
		buf.Append("spreadCurve").Append('=').Append(JodaBeanUtils.ToString(spreadCurve));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code AddFixedCurve}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  fixedCurve_Renamed = DirectMetaProperty.ofImmutable(this, "fixedCurve", typeof(AddFixedCurve), typeof(Curve));
			  spreadCurve_Renamed = DirectMetaProperty.ofImmutable(this, "spreadCurve", typeof(AddFixedCurve), typeof(Curve));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "fixedCurve", "spreadCurve");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code fixedCurve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> fixedCurve_Renamed;
		/// <summary>
		/// The meta-property for the {@code spreadCurve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> spreadCurve_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "fixedCurve", "spreadCurve");
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
			case 1682092507: // fixedCurve
			  return fixedCurve_Renamed;
			case 2130054972: // spreadCurve
			  return spreadCurve_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends AddFixedCurve> builder()
		public override BeanBuilder<AddFixedCurve> builder()
		{
		  return new AddFixedCurve.Builder();
		}

		public override Type beanType()
		{
		  return typeof(AddFixedCurve);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code fixedCurve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Curve> fixedCurve()
		{
		  return fixedCurve_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code spreadCurve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Curve> spreadCurve()
		{
		  return spreadCurve_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1682092507: // fixedCurve
			  return ((AddFixedCurve) bean).FixedCurve;
			case 2130054972: // spreadCurve
			  return ((AddFixedCurve) bean).SpreadCurve;
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
	  /// The bean-builder for {@code AddFixedCurve}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<AddFixedCurve>
	  {

		internal Curve fixedCurve;
		internal Curve spreadCurve;

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
			case 1682092507: // fixedCurve
			  return fixedCurve;
			case 2130054972: // spreadCurve
			  return spreadCurve;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1682092507: // fixedCurve
			  this.fixedCurve = (Curve) newValue;
			  break;
			case 2130054972: // spreadCurve
			  this.spreadCurve = (Curve) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override AddFixedCurve build()
		{
		  return new AddFixedCurve(fixedCurve, spreadCurve);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("AddFixedCurve.Builder{");
		  buf.Append("fixedCurve").Append('=').Append(JodaBeanUtils.ToString(fixedCurve)).Append(',').Append(' ');
		  buf.Append("spreadCurve").Append('=').Append(JodaBeanUtils.ToString(spreadCurve));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}