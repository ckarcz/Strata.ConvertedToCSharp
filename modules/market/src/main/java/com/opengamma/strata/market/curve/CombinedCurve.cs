using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// A curve formed from two curves, the base curve and the spread curve.
	/// <para>
	/// The parameters of this curve are the combination of the base curve parameters and spread curve parameters.
	/// The node sensitivities are calculated in terms of the nodes on the base curve and spread curve.
	/// </para>
	/// <para>
	/// If one of the two curves must be fixed, use <seealso cref="AddFixedCurve"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CombinedCurve implements Curve, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CombinedCurve : Curve, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Curve baseCurve;
		private readonly Curve baseCurve;
	  /// <summary>
	  /// The spread curve. 
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Curve spreadCurve;
	  private readonly Curve spreadCurve;
	  /// <summary>
	  /// The curve metadata.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CurveMetadata metadata;
	  private readonly CurveMetadata metadata;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a curve as the sum of a base curve and a spread curve with a specified curve metadata.
	  /// </summary>
	  /// <param name="baseCurve">  the base curve </param>
	  /// <param name="spreadCurve">  the spread curve </param>
	  /// <param name="metadata">  the metadata </param>
	  /// <returns> the combined curve </returns>
	  public static CombinedCurve of(Curve baseCurve, Curve spreadCurve, CurveMetadata metadata)
	  {

		return new CombinedCurve(baseCurve, spreadCurve, metadata);
	  }

	  /// <summary>
	  /// Creates a curve as the sum of a base curve and a spread curve.
	  /// <para>
	  /// The metadata of the combined curve will be created form the base curve and spread curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseCurve">  the base curve </param>
	  /// <param name="spreadCurve">  the spread curve </param>
	  /// <returns> the combined curve </returns>
	  public static CombinedCurve of(Curve baseCurve, Curve spreadCurve)
	  {
		CurveMetadata baseMetadata = baseCurve.Metadata;
		CurveMetadata spreadMetadata = spreadCurve.Metadata;

		IList<ParameterMetadata> paramMeta = Stream.concat(IntStream.range(0, baseCurve.ParameterCount).mapToObj(i => baseCurve.getParameterMetadata(i)), IntStream.range(0, spreadCurve.ParameterCount).mapToObj(i => spreadCurve.getParameterMetadata(i))).collect(toImmutableList());

		DefaultCurveMetadataBuilder metadataBuilder = DefaultCurveMetadata.builder().curveName(baseCurve.Name.Name + "+" + spreadMetadata.CurveName.Name).xValueType(baseMetadata.XValueType).yValueType(baseMetadata.YValueType).parameterMetadata(paramMeta);

		if (baseMetadata.findInfo(CurveInfoType.DAY_COUNT).Present)
		{
		  metadataBuilder.addInfo(CurveInfoType.DAY_COUNT, baseMetadata.getInfo(CurveInfoType.DAY_COUNT));
		}

		return of(baseCurve, spreadCurve, metadataBuilder.build());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private CombinedCurve(Curve baseCurve, Curve spreadCurve, CurveMetadata metadata)
	  private CombinedCurve(Curve baseCurve, Curve spreadCurve, CurveMetadata metadata)
	  {

		JodaBeanUtils.notNull(baseCurve, "baseCurve");
		JodaBeanUtils.notNull(spreadCurve, "spreadCurve");
		JodaBeanUtils.notNull(metadata, "metadata");

		CurveMetadata baseMetadata = baseCurve.Metadata;
		CurveMetadata spreadMetadata = spreadCurve.Metadata;
		// check value type
		if (!baseMetadata.XValueType.Equals(metadata.XValueType))
		{
		  throw new System.ArgumentException(Messages.format("xValueType is {} in baseCurve, but {} in CombinedCurve", baseMetadata.XValueType, metadata.XValueType));
		}
		if (!spreadMetadata.XValueType.Equals(metadata.XValueType))
		{
		  throw new System.ArgumentException(Messages.format("xValueType is {} in spreadCurve, but {} in CombinedCurve", spreadMetadata.XValueType, metadata.XValueType));
		}
		if (!baseMetadata.YValueType.Equals(metadata.YValueType))
		{
		  throw new System.ArgumentException(Messages.format("yValueType is {} in baseCurve, but {} in CombinedCurve", baseMetadata.YValueType, metadata.YValueType));
		}
		if (!spreadMetadata.YValueType.Equals(metadata.YValueType))
		{
		  throw new System.ArgumentException(Messages.format("yValueType is {} in spreadCurve, but {} in CombinedCurve", spreadMetadata.YValueType, metadata.YValueType));
		}
		// check day count
		Optional<DayCount> dccOpt = metadata.findInfo(CurveInfoType.DAY_COUNT);
		if (dccOpt.Present)
		{
		  DayCount dcc = dccOpt.get();
		  if (!baseMetadata.findInfo(CurveInfoType.DAY_COUNT).Present || !baseMetadata.getInfo(CurveInfoType.DAY_COUNT).Equals(dcc))
		  {
			throw new System.ArgumentException(Messages.format("DayCount in baseCurve should be {}", dcc));
		  }
		  if (!spreadMetadata.findInfo(CurveInfoType.DAY_COUNT).Present || !spreadMetadata.getInfo(CurveInfoType.DAY_COUNT).Equals(dcc))
		  {
			throw new System.ArgumentException(Messages.format("DayCount in spreadCurve should be {}", dcc));
		  }
		}

		this.metadata = metadata;
		this.baseCurve = baseCurve;
		this.spreadCurve = spreadCurve;
	  }

	  //-------------------------------------------------------------------------
	  public CombinedCurve withMetadata(CurveMetadata metadata)
	  {
		return new CombinedCurve(baseCurve, spreadCurve, metadata);
	  }

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return baseCurve.ParameterCount + spreadCurve.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		if (parameterIndex < baseCurve.ParameterCount)
		{
		  return baseCurve.getParameter(parameterIndex);
		}
		return spreadCurve.getParameter(parameterIndex - baseCurve.ParameterCount);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		if (parameterIndex < baseCurve.ParameterCount)
		{
		  return baseCurve.getParameterMetadata(parameterIndex);
		}
		return spreadCurve.getParameterMetadata(parameterIndex - baseCurve.ParameterCount);
	  }

	  public CombinedCurve withParameter(int parameterIndex, double newValue)
	  {
		if (parameterIndex < baseCurve.ParameterCount)
		{
		  return new CombinedCurve(baseCurve.withParameter(parameterIndex, newValue), spreadCurve, metadata);
		}
		return new CombinedCurve(baseCurve, spreadCurve.withParameter(parameterIndex - baseCurve.ParameterCount, newValue), metadata);
	  }

	  public override CombinedCurve withPerturbation(ParameterPerturbation perturbation)
	  {

		Curve newBaseCurve = baseCurve.withPerturbation((idx, value, meta) => perturbation(idx, baseCurve.getParameter(idx), baseCurve.getParameterMetadata(idx)));
		int offset = baseCurve.ParameterCount;
		Curve newSpreadCurve = spreadCurve.withPerturbation((idx, value, meta) => perturbation(idx + offset, spreadCurve.getParameter(idx), spreadCurve.getParameterMetadata(idx)));

		IList<ParameterMetadata> newParamMeta = Stream.concat(IntStream.range(0, newBaseCurve.ParameterCount).mapToObj(i => newBaseCurve.getParameterMetadata(i)), IntStream.range(0, newSpreadCurve.ParameterCount).mapToObj(i => newSpreadCurve.getParameterMetadata(i))).collect(toImmutableList());

		return CombinedCurve.of(newBaseCurve, newSpreadCurve, metadata.withParameterMetadata(newParamMeta));
	  }

	  //-------------------------------------------------------------------------
	  public double yValue(double x)
	  {
		return baseCurve.yValue(x) + spreadCurve.yValue(x);
	  }

	  public UnitParameterSensitivity yValueParameterSensitivity(double x)
	  {
		UnitParameterSensitivity baseSens = baseCurve.yValueParameterSensitivity(x);
		UnitParameterSensitivity spreadSens = spreadCurve.yValueParameterSensitivity(x);
		return UnitParameterSensitivity.combine(Name, baseSens, spreadSens);
	  }

	  public double firstDerivative(double x)
	  {
		return baseCurve.firstDerivative(x) + spreadCurve.firstDerivative(x);
	  }

	  //-------------------------------------------------------------------------
	  public override UnitParameterSensitivity createParameterSensitivity(DoubleArray sensitivities)
	  {
		UnitParameterSensitivity baseSens = baseCurve.createParameterSensitivity(sensitivities.subArray(0, baseCurve.ParameterCount));
		UnitParameterSensitivity spreadSens = spreadCurve.createParameterSensitivity(sensitivities.subArray(baseCurve.ParameterCount, sensitivities.size()));
		return UnitParameterSensitivity.combine(Name, baseSens, spreadSens);
	  }

	  public override CurrencyParameterSensitivity createParameterSensitivity(Currency currency, DoubleArray sensitivities)
	  {
		CurrencyParameterSensitivity baseSensi = baseCurve.createParameterSensitivity(currency, sensitivities.subArray(0, baseCurve.ParameterCount));
		CurrencyParameterSensitivity spreadSensi = spreadCurve.createParameterSensitivity(currency, sensitivities.subArray(baseCurve.ParameterCount, sensitivities.size()));
		return CurrencyParameterSensitivity.combine(Name, baseSensi, spreadSensi);
	  }

	  //-------------------------------------------------------------------------
	  public override ImmutableList<Curve> split()
	  {
		return ImmutableList.of(baseCurve, spreadCurve);
	  }

	  public override CombinedCurve withUnderlyingCurve(int curveIndex, Curve curve)
	  {
		if (curveIndex == 0)
		{
		  return new CombinedCurve(curve, spreadCurve, metadata);
		}
		if (curveIndex == 1)
		{
		  return new CombinedCurve(baseCurve, curve, metadata);
		}
		throw new System.ArgumentException("curveIndex is outside the range");
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CombinedCurve}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CombinedCurve.Meta meta()
	  {
		return CombinedCurve.Meta.INSTANCE;
	  }

	  static CombinedCurve()
	  {
		MetaBean.register(CombinedCurve.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override CombinedCurve.Meta metaBean()
	  {
		return CombinedCurve.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the base curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve BaseCurve
	  {
		  get
		  {
			return baseCurve;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the spread curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve SpreadCurve
	  {
		  get
		  {
			return spreadCurve;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve metadata. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveMetadata Metadata
	  {
		  get
		  {
			return metadata;
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
		  CombinedCurve other = (CombinedCurve) obj;
		  return JodaBeanUtils.equal(baseCurve, other.baseCurve) && JodaBeanUtils.equal(spreadCurve, other.spreadCurve) && JodaBeanUtils.equal(metadata, other.metadata);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(baseCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spreadCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(metadata);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("CombinedCurve{");
		buf.Append("baseCurve").Append('=').Append(baseCurve).Append(',').Append(' ');
		buf.Append("spreadCurve").Append('=').Append(spreadCurve).Append(',').Append(' ');
		buf.Append("metadata").Append('=').Append(JodaBeanUtils.ToString(metadata));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CombinedCurve}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  baseCurve_Renamed = DirectMetaProperty.ofImmutable(this, "baseCurve", typeof(CombinedCurve), typeof(Curve));
			  spreadCurve_Renamed = DirectMetaProperty.ofImmutable(this, "spreadCurve", typeof(CombinedCurve), typeof(Curve));
			  metadata_Renamed = DirectMetaProperty.ofImmutable(this, "metadata", typeof(CombinedCurve), typeof(CurveMetadata));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "baseCurve", "spreadCurve", "metadata");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code baseCurve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> baseCurve_Renamed;
		/// <summary>
		/// The meta-property for the {@code spreadCurve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> spreadCurve_Renamed;
		/// <summary>
		/// The meta-property for the {@code metadata} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveMetadata> metadata_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "baseCurve", "spreadCurve", "metadata");
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
			case -1842240354: // baseCurve
			  return baseCurve_Renamed;
			case 2130054972: // spreadCurve
			  return spreadCurve_Renamed;
			case -450004177: // metadata
			  return metadata_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CombinedCurve> builder()
		public override BeanBuilder<CombinedCurve> builder()
		{
		  return new CombinedCurve.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CombinedCurve);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code baseCurve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Curve> baseCurve()
		{
		  return baseCurve_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code spreadCurve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Curve> spreadCurve()
		{
		  return spreadCurve_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code metadata} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveMetadata> metadata()
		{
		  return metadata_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1842240354: // baseCurve
			  return ((CombinedCurve) bean).BaseCurve;
			case 2130054972: // spreadCurve
			  return ((CombinedCurve) bean).SpreadCurve;
			case -450004177: // metadata
			  return ((CombinedCurve) bean).Metadata;
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
	  /// The bean-builder for {@code CombinedCurve}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CombinedCurve>
	  {

		internal Curve baseCurve;
		internal Curve spreadCurve;
		internal CurveMetadata metadata;

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
			case -1842240354: // baseCurve
			  return baseCurve;
			case 2130054972: // spreadCurve
			  return spreadCurve;
			case -450004177: // metadata
			  return metadata;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1842240354: // baseCurve
			  this.baseCurve = (Curve) newValue;
			  break;
			case 2130054972: // spreadCurve
			  this.spreadCurve = (Curve) newValue;
			  break;
			case -450004177: // metadata
			  this.metadata = (CurveMetadata) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CombinedCurve build()
		{
		  return new CombinedCurve(baseCurve, spreadCurve, metadata);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("CombinedCurve.Builder{");
		  buf.Append("baseCurve").Append('=').Append(JodaBeanUtils.ToString(baseCurve)).Append(',').Append(' ');
		  buf.Append("spreadCurve").Append('=').Append(JodaBeanUtils.ToString(spreadCurve)).Append(',').Append(' ');
		  buf.Append("metadata").Append('=').Append(JodaBeanUtils.ToString(metadata));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}