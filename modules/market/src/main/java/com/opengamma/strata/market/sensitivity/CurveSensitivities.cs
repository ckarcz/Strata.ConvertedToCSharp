using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


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

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivitiesBuilder = com.opengamma.strata.market.param.CurrencyParameterSensitivitiesBuilder;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using AttributeType = com.opengamma.strata.product.AttributeType;
	using PortfolioItem = com.opengamma.strata.product.PortfolioItem;
	using PortfolioItemInfo = com.opengamma.strata.product.PortfolioItemInfo;
	using PortfolioItemSummary = com.opengamma.strata.product.PortfolioItemSummary;
	using PortfolioItemType = com.opengamma.strata.product.PortfolioItemType;
	using ProductType = com.opengamma.strata.product.ProductType;

	/// <summary>
	/// Sensitivity to a set of curves, used to pass risk into calculations.
	/// <para>
	/// Sometimes it is useful to pass in a representation of risk rather than explicitly
	/// listing the current portfolio of trades and/or positions.
	/// This target is designed to allow this.
	/// </para>
	/// <para>
	/// A map of sensitivities is provided, allowing both delta and gamma to be included if desired.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CurveSensitivities implements com.opengamma.strata.product.PortfolioItem, com.opengamma.strata.basics.currency.FxConvertible<CurveSensitivities>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CurveSensitivities : PortfolioItem, FxConvertible<CurveSensitivities>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.PortfolioItemInfo info;
		private readonly PortfolioItemInfo info;
	  /// <summary>
	  /// The sensitivities, keyed by type.
	  /// <para>
	  /// The map allows sensitivity to different types to be expressed.
	  /// For example, there might be both delta and gamma sensitivity.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<CurveSensitivitiesType, com.opengamma.strata.market.param.CurrencyParameterSensitivities> typedSensitivities;
	  private readonly ImmutableMap<CurveSensitivitiesType, CurrencyParameterSensitivities> typedSensitivities;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an empty instance.
	  /// </summary>
	  /// <returns> the empty sensitivities instance </returns>
	  public static CurveSensitivities empty()
	  {
		return new CurveSensitivities(PortfolioItemInfo.empty(), ImmutableMap.of());
	  }

	  /// <summary>
	  /// Returns a builder that can be used to create an instance of {@code CurveSensitivities}.
	  /// <para>
	  /// The builder takes into account the parameter metadata when creating the sensitivity map.
	  /// As such, the parameter metadata added to the builder must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="info">  the additional information </param>
	  /// <returns> the builder </returns>
	  public static CurveSensitivitiesBuilder builder(PortfolioItemInfo info)
	  {
		return new CurveSensitivitiesBuilder(info);
	  }

	  /// <summary>
	  /// Obtains an instance from a single set of sensitivities.
	  /// </summary>
	  /// <param name="info">  the additional information </param>
	  /// <param name="type">  the type of the sensitivities </param>
	  /// <param name="sensitivities">  the sensitivities </param>
	  /// <returns> the sensitivities instance </returns>
	  public static CurveSensitivities of(PortfolioItemInfo info, CurveSensitivitiesType type, CurrencyParameterSensitivities sensitivities)
	  {

		return new CurveSensitivities(info, ImmutableMap.of(type, sensitivities));
	  }

	  /// <summary>
	  /// Obtains an instance from a map of sensitivities.
	  /// </summary>
	  /// <param name="info">  the additional information </param>
	  /// <param name="typedSensitivities">  the map of sensitivities by type </param>
	  /// <returns> the sensitivities instance </returns>
	  public static CurveSensitivities of(PortfolioItemInfo info, IDictionary<CurveSensitivitiesType, CurrencyParameterSensitivities> typedSensitivities)
	  {

		return new CurveSensitivities(info, ImmutableMap.copyOf(typedSensitivities));
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets a sensitivity instance by type, throwing an exception if not found.
	  /// </summary>
	  /// <param name="type">  the type to get </param>
	  /// <returns> the sensitivity </returns>
	  /// <exception cref="IllegalArgumentException"> if the type is not found </exception>
	  public CurrencyParameterSensitivities getTypedSensitivity(CurveSensitivitiesType type)
	  {
		CurrencyParameterSensitivities sens = typedSensitivities.get(type);
		if (sens == null)
		{
		  throw new System.ArgumentException("Unable to find sensitivities: " + type);
		}
		return sens;
	  }

	  /// <summary>
	  /// Finds a sensitivity instance by type, returning empty if not found.
	  /// </summary>
	  /// <param name="type">  the type to find </param>
	  /// <returns> the sensitivity, empty if not found </returns>
	  public Optional<CurrencyParameterSensitivities> findTypedSensitivity(CurveSensitivitiesType type)
	  {
		return Optional.ofNullable(typedSensitivities.get(type));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Merges this set of sensitivities with another set.
	  /// <para>
	  /// This returns a new curve sensitivities with a combined map of typed sensitivities.
	  /// Any sensitivities of the same type will be combined as though using
	  /// <seealso cref="CurrencyParameterSensitivities#mergedWith(CurrencyParameterSensitivities)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other parameter sensitivities </param>
	  /// <returns> an instance based on this one, with the other instance added </returns>
	  public CurveSensitivities mergedWith(IDictionary<CurveSensitivitiesType, CurrencyParameterSensitivities> other)
	  {
		// this uses a collector to merge all the instances at once which is more efficient than reduction
		// because it creates a single CurrencyParameterSensitivitiesBuilder
		ImmutableMap<CurveSensitivitiesType, CurrencyParameterSensitivities> combinedSens = MapStream.concat(MapStream.of(typedSensitivities), MapStream.of(other)).toMapGrouping(mergeSensitivities());
		return new CurveSensitivities(info, combinedSens);
	  }

	  // collector to merge sensitivities
	  private static Collector<CurrencyParameterSensitivities, CurrencyParameterSensitivitiesBuilder, CurrencyParameterSensitivities> mergeSensitivities()
	  {

//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Collector.of(CurrencyParameterSensitivities.builder, CurrencyParameterSensitivitiesBuilder::add, (l, r) => l.add(r.build()), CurrencyParameterSensitivitiesBuilder::build);
	  }

	  /// <summary>
	  /// Combines this set of sensitivities with another set.
	  /// <para>
	  /// This returns a new curve sensitivities with a combined map of typed sensitivities.
	  /// Any sensitivities of the same type will be combined as though using
	  /// <seealso cref="CurrencyParameterSensitivities#mergedWith(CurrencyParameterSensitivities)"/>.
	  /// The identifier and attributes of this instance will take precedence.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other parameter sensitivities </param>
	  /// <returns> an instance based on this one, with the other instance added </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"rawtypes", "unchecked"}) public CurveSensitivities mergedWith(CurveSensitivities other)
	  public CurveSensitivities mergedWith(CurveSensitivities other)
	  {
		PortfolioItemInfo combinedInfo = info;
		if (!info.Id.Present && other.info.Id.Present)
		{
		  combinedInfo = combinedInfo.withId(other.info.Id.get());
		}
		foreach (AttributeType attrType in other.info.AttributeTypes)
		{
		  if (!combinedInfo.AttributeTypes.contains(attrType))
		  {
			combinedInfo = combinedInfo.withAttribute(attrType, other.info.getAttribute(attrType));
		  }
		}
		return new CurveSensitivities(combinedInfo, mergedWith(other.typedSensitivities).TypedSensitivities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks and adjusts the market data names.
	  /// <para>
	  /// The supplied function is invoked for each market data name in this sensitivities.
	  /// A typical use case would be to convert index names to curve names valid for an underlying system.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="nameFn">  the function for checking and adjusting the name </param>
	  /// <returns> the adjusted sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the function throws an exception </exception>
	  public CurveSensitivities withMarketDataNames<T1>(System.Func<T1> nameFn)
	  {
		return new CurveSensitivities(info, MapStream.of(typedSensitivities).mapValues(sens => sens.withMarketDataNames(nameFn)).toMap());
	  }

	  /// <summary>
	  /// Checks and adjusts the parameter metadata.
	  /// <para>
	  /// The supplied function is invoked for each parameter metadata in this sensitivities.
	  /// If the function returns the same metadata for two different inputs, the sensitivity value will be summed.
	  /// A typical use case would be to normalize parameter metadata tenors to be valid for an underlying system.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mdFn">  the function for checking and adjusting the metadata </param>
	  /// <returns> the adjusted sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the function throws an exception </exception>
	  public CurveSensitivities withParameterMetadatas(System.Func<ParameterMetadata, ParameterMetadata> mdFn)
	  {
		return new CurveSensitivities(info, MapStream.of(typedSensitivities).mapValues(sens => sens.withParameterMetadatas(mdFn)).toMap());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts the sensitivities in this instance to an equivalent in the specified currency.
	  /// <para>
	  /// Any FX conversion that is required will use rates from the provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the sensitivity object expressed in terms of the result currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  public CurveSensitivities convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return new CurveSensitivities(info, MapStream.of(typedSensitivities).mapValues(v => v.convertedTo(resultCurrency, rateProvider)).toMap());
	  }

	  public PortfolioItemSummary summarize()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		string typesStr = typedSensitivities.Keys.Select(CurveSensitivitiesType::toString).OrderBy(c => c).collect(joining(", ", "CurveSensitivities[", "]"));
		return PortfolioItemSummary.of(Id.orElse(null), PortfolioItemType.SENSITIVITIES, ProductType.SENSITIVITIES, typedSensitivities.values().stream().flatMap(s => s.Sensitivities.stream()).map(s => s.Currency).collect(toImmutableSet()), typesStr);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurveSensitivities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CurveSensitivities.Meta meta()
	  {
		return CurveSensitivities.Meta.INSTANCE;
	  }

	  static CurveSensitivities()
	  {
		MetaBean.register(CurveSensitivities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CurveSensitivities(PortfolioItemInfo info, IDictionary<CurveSensitivitiesType, CurrencyParameterSensitivities> typedSensitivities)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(typedSensitivities, "typedSensitivities");
		this.info = info;
		this.typedSensitivities = ImmutableMap.copyOf(typedSensitivities);
	  }

	  public override CurveSensitivities.Meta metaBean()
	  {
		return CurveSensitivities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional information.
	  /// <para>
	  /// This allows additional information to be attached to the sensitivities.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PortfolioItemInfo Info
	  {
		  get
		  {
			return info;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the sensitivities, keyed by type.
	  /// <para>
	  /// The map allows sensitivity to different types to be expressed.
	  /// For example, there might be both delta and gamma sensitivity.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<CurveSensitivitiesType, CurrencyParameterSensitivities> TypedSensitivities
	  {
		  get
		  {
			return typedSensitivities;
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
		  CurveSensitivities other = (CurveSensitivities) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(typedSensitivities, other.typedSensitivities);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(typedSensitivities);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("CurveSensitivities{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("typedSensitivities").Append('=').Append(JodaBeanUtils.ToString(typedSensitivities));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurveSensitivities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(CurveSensitivities), typeof(PortfolioItemInfo));
			  typedSensitivities_Renamed = DirectMetaProperty.ofImmutable(this, "typedSensitivities", typeof(CurveSensitivities), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "typedSensitivities");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PortfolioItemInfo> info_Renamed;
		/// <summary>
		/// The meta-property for the {@code typedSensitivities} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<CurveSensitivitiesType, com.opengamma.strata.market.param.CurrencyParameterSensitivities>> typedSensitivities = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "typedSensitivities", CurveSensitivities.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<CurveSensitivitiesType, CurrencyParameterSensitivities>> typedSensitivities_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "typedSensitivities");
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
			case 3237038: // info
			  return info_Renamed;
			case 153032499: // typedSensitivities
			  return typedSensitivities_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CurveSensitivities> builder()
		public override BeanBuilder<CurveSensitivities> builder()
		{
		  return new CurveSensitivities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CurveSensitivities);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code info} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PortfolioItemInfo> info()
		{
		  return info_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code typedSensitivities} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<CurveSensitivitiesType, CurrencyParameterSensitivities>> typedSensitivities()
		{
		  return typedSensitivities_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return ((CurveSensitivities) bean).Info;
			case 153032499: // typedSensitivities
			  return ((CurveSensitivities) bean).TypedSensitivities;
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
	  /// The bean-builder for {@code CurveSensitivities}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CurveSensitivities>
	  {

		internal PortfolioItemInfo info;
		internal IDictionary<CurveSensitivitiesType, CurrencyParameterSensitivities> typedSensitivities = ImmutableMap.of();

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
			case 3237038: // info
			  return info;
			case 153032499: // typedSensitivities
			  return typedSensitivities;
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
			case 3237038: // info
			  this.info = (PortfolioItemInfo) newValue;
			  break;
			case 153032499: // typedSensitivities
			  this.typedSensitivities = (IDictionary<CurveSensitivitiesType, CurrencyParameterSensitivities>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CurveSensitivities build()
		{
		  return new CurveSensitivities(info, typedSensitivities);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("CurveSensitivities.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info)).Append(',').Append(' ');
		  buf.Append("typedSensitivities").Append('=').Append(JodaBeanUtils.ToString(typedSensitivities));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}