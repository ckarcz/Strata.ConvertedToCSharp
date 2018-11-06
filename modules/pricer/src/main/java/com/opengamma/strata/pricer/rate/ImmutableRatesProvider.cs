using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
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
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using DiscountFxForwardRates = com.opengamma.strata.pricer.fx.DiscountFxForwardRates;
	using ForwardFxIndexRates = com.opengamma.strata.pricer.fx.ForwardFxIndexRates;
	using FxForwardRates = com.opengamma.strata.pricer.fx.FxForwardRates;
	using FxIndexRates = com.opengamma.strata.pricer.fx.FxIndexRates;

	/// <summary>
	/// The default immutable rates provider, used to calculate analytic measures.
	/// <para>
	/// This provides the environmental information against which pricing occurs.
	/// This includes FX rates, discount factors and forward curves.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class ImmutableRatesProvider implements RatesProvider, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableRatesProvider : RatesProvider, ImmutableBean
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The valuation date.
	  /// All curves and other data items in this provider are calibrated for this date.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate valuationDate;
	  private readonly LocalDate valuationDate;
	  /// <summary>
	  /// The provider of foreign exchange rates.
	  /// Conversions where both currencies are the same always succeed.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.FxRateProvider fxRateProvider;
	  private readonly FxRateProvider fxRateProvider;
	  /// <summary>
	  /// The discount curves, defaulted to an empty map.
	  /// The curve data, predicting the future, associated with each currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.currency.Currency, com.opengamma.strata.market.curve.Curve> discountCurves;
	  private readonly ImmutableMap<Currency, Curve> discountCurves;
	  /// <summary>
	  /// The forward curves, defaulted to an empty map.
	  /// The curve data, predicting the future, associated with each index.
	  /// This is used for Ibor, Overnight and Price indices.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.index.Index, com.opengamma.strata.market.curve.Curve> indexCurves;
	  private readonly ImmutableMap<Index, Curve> indexCurves;
	  /// <summary>
	  /// The time-series, defaulted to an empty map.
	  /// The historic data associated with each index.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.index.Index, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries> timeSeries;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly ImmutableMap<Index, LocalDateDoubleTimeSeries> timeSeries_Renamed;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.fxRateProvider = FxMatrix.empty();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines a number of rates providers.
	  /// <para>
	  /// If the two providers have curves or time series for the same currency or index, 
	  /// an <seealso cref="IllegalAccessException"/> is thrown.
	  /// The FxRateProviders is not populated with the given provider; no attempt is done on merging the embedded FX providers.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fx">  the FX provider for the resulting rate provider </param>
	  /// <param name="providers">  the rates providers to be merged </param>
	  /// <returns> the combined rates provider </returns>
	  public static ImmutableRatesProvider combined(FxRateProvider fx, params ImmutableRatesProvider[] providers)
	  {
		ArgChecker.isTrue(providers.Length > 0, "at least one provider requested");
		ImmutableRatesProvider merged = ImmutableRatesProvider.builder(providers[0].ValuationDate).build();
		foreach (ImmutableRatesProvider provider in providers)
		{
		  merged = merged.combinedWith(provider, fx);
		}
		return merged;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a builder specifying the valuation date.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the builder </returns>
	  public static ImmutableRatesProviderBuilder builder(LocalDate valuationDate)
	  {
		return new ImmutableRatesProviderBuilder(valuationDate);
	  }

	  /// <summary>
	  /// Converts this instance to a builder allowing changes to be made.
	  /// </summary>
	  /// <returns> the builder </returns>
	  public ImmutableRatesProviderBuilder toBuilder()
	  {
		return (new ImmutableRatesProviderBuilder(valuationDate)).fxRateProvider(fxRateProvider).discountCurves(discountCurves).indexCurves(indexCurves).timeSeries(timeSeries_Renamed);
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<Currency> DiscountCurrencies
	  {
		  get
		  {
			return discountCurves.Keys;
		  }
	  }

	  public ImmutableSet<IborIndex> IborIndices
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return indexCurves.Keys.Where(typeof(IborIndex).isInstance).Select(typeof(IborIndex).cast).collect(toImmutableSet());
		  }
	  }

	  public ImmutableSet<OvernightIndex> OvernightIndices
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return indexCurves.Keys.Where(typeof(OvernightIndex).isInstance).Select(typeof(OvernightIndex).cast).collect(toImmutableSet());
		  }
	  }

	  public ImmutableSet<PriceIndex> PriceIndices
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return indexCurves.Keys.Where(typeof(PriceIndex).isInstance).Select(typeof(PriceIndex).cast).collect(toImmutableSet());
		  }
	  }

	  public ImmutableSet<Index> TimeSeriesIndices
	  {
		  get
		  {
			return timeSeries_Renamed.Keys;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		if (name is CurveName)
		{
		  return Stream.concat(discountCurves.values().stream(), indexCurves.values().stream()).filter(c => c.Name.Equals(name)).map(v => name.MarketDataType.cast(v)).findFirst();
		}
		return null;
	  }

	  //-------------------------------------------------------------------------
	  public T data<T>(MarketDataId<T> id)
	  {
		throw new System.ArgumentException("Unknown identifier: " + id.ToString());
	  }

	  //-------------------------------------------------------------------------
	  public LocalDateDoubleTimeSeries timeSeries(Index index)
	  {
		return timeSeries_Renamed.getOrDefault(index, LocalDateDoubleTimeSeries.empty());
	  }

	  // finds the index curve
	  private Curve indexCurve(Index index)
	  {
		Curve curve = indexCurves.get(index);
		if (curve == null)
		{
		  throw new System.ArgumentException("Unable to find index curve: " + index);
		}
		return curve;
	  }

	  //-------------------------------------------------------------------------
	  public double fxRate(Currency baseCurrency, Currency counterCurrency)
	  {
		return fxRateProvider.fxRate(baseCurrency, counterCurrency);
	  }

	  //-------------------------------------------------------------------------
	  public DiscountFactors discountFactors(Currency currency)
	  {
		Curve curve = discountCurves.get(currency);
		if (curve == null)
		{
		  throw new System.ArgumentException("Unable to find discount curve: " + currency);
		}
		return DiscountFactors.of(currency, valuationDate, curve);
	  }

	  //-------------------------------------------------------------------------
	  public FxIndexRates fxIndexRates(FxIndex index)
	  {
		LocalDateDoubleTimeSeries fixings = timeSeries(index);
		FxForwardRates fxForwardRates = this.fxForwardRates(index.CurrencyPair);
		return ForwardFxIndexRates.of(index, fxForwardRates, fixings);
	  }

	  //-------------------------------------------------------------------------
	  public FxForwardRates fxForwardRates(CurrencyPair currencyPair)
	  {
		DiscountFactors @base = discountFactors(currencyPair.Base);
		DiscountFactors counter = discountFactors(currencyPair.Counter);
		return DiscountFxForwardRates.of(currencyPair, fxRateProvider, @base, counter);
	  };

	  //-------------------------------------------------------------------------
	  public IborIndexRates iborIndexRates(IborIndex index)
	  {
		LocalDateDoubleTimeSeries fixings = timeSeries(index);
		Curve curve = indexCurve(index);
		return IborIndexRates.of(index, valuationDate, curve, fixings);
	  }

	  public OvernightIndexRates overnightIndexRates(OvernightIndex index)
	  {
		LocalDateDoubleTimeSeries fixings = timeSeries(index);
		Curve curve = indexCurve(index);
		return OvernightIndexRates.of(index, valuationDate, curve, fixings);
	  }

	  public PriceIndexValues priceIndexValues(PriceIndex index)
	  {
		LocalDateDoubleTimeSeries fixings = timeSeries(index);
		Curve curve = indexCurve(index);
		return PriceIndexValues.of(index, valuationDate, curve, fixings);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this provider with another.
	  /// <para> 
	  /// If the two providers have curves or time series for the same currency or index,
	  /// an <seealso cref="IllegalAccessException"/> is thrown. No attempt is made to combine the
	  /// FX providers, instead one is supplied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other rates provider </param>
	  /// <param name="fxProvider">  the FX rate provider to use </param>
	  /// <returns> the combined provider </returns>
	  public ImmutableRatesProvider combinedWith(ImmutableRatesProvider other, FxRateProvider fxProvider)
	  {
		ImmutableRatesProviderBuilder merged = other.toBuilder();
		// discount
		ImmutableMap<Currency, Curve> dscMap1 = discountCurves;
		ImmutableMap<Currency, Curve> dscMap2 = other.discountCurves;
		foreach (KeyValuePair<Currency, Curve> entry in dscMap1.entrySet())
		{
		  ArgChecker.isTrue(!dscMap2.containsKey(entry.Key), "conflict on discount curve, currency '{}' appears twice in the providers", entry.Key);
		  merged.discountCurve(entry.Key, entry.Value);
		}
		// forward
		ImmutableMap<Index, Curve> indexMap1 = indexCurves;
		ImmutableMap<Index, Curve> indexMap2 = other.indexCurves;
		foreach (KeyValuePair<Index, Curve> entry in indexMap1.entrySet())
		{
		  ArgChecker.isTrue(!indexMap2.containsKey(entry.Key), "conflict on index curve, index '{}' appears twice in the providers", entry.Key);
		  merged.indexCurve(entry.Key, entry.Value);
		}
		// time series
		IDictionary<Index, LocalDateDoubleTimeSeries> tsMap1 = timeSeries_Renamed;
		IDictionary<Index, LocalDateDoubleTimeSeries> tsMap2 = other.timeSeries_Renamed;
		foreach (KeyValuePair<Index, LocalDateDoubleTimeSeries> entry in tsMap1.SetOfKeyValuePairs())
		{
		  ArgChecker.isTrue(!tsMap2.ContainsKey(entry.Key), "conflict on time series, index '{}' appears twice in the providers", entry.Key);
		  merged.timeSeries(entry.Key, entry.Value);
		}
		merged.fxRateProvider(fxProvider);
		return merged.build();
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableRatesProvider toImmutableRatesProvider()
	  {
		return this;
	  }

	  /// <summary>
	  /// Returns a map containing all the curves, keyed by curve name.
	  /// <para>
	  /// No checks are performed to see if one curve name is mapped to two different curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the map of curves </returns>
	  public IDictionary<CurveName, Curve> Curves
	  {
		  get
		  {
			// use a HashMap to avoid errors due to duplicates
			IDictionary<CurveName, Curve> curves = new Dictionary<CurveName, Curve>();
			discountCurves.values().forEach(curve => curves.put(curve.Name, curve));
			indexCurves.values().forEach(curve => curves.put(curve.Name, curve));
			return curves;
		  }
	  }

	  /// <summary>
	  /// Returns a map containing all the curves, keyed by curve identifier.
	  /// <para>
	  /// No checks are performed to see if one curve name is mapped to two different curves.
	  /// </para>
	  /// <para>
	  /// This method is useful when transforming a rates provider to <seealso cref="MarketData"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupName">  the curve group name </param>
	  /// <returns> the map of curves, keyed by {@code CurveId}. </returns>
	  public IDictionary<CurveId, Curve> getCurves(CurveGroupName groupName)
	  {
		// use a HashMap to avoid errors due to duplicates
		IDictionary<CurveId, Curve> curves = new Dictionary<CurveId, Curve>();
		discountCurves.values().forEach(curve => curves.put(CurveId.of(groupName, curve.Name), curve));
		indexCurves.values().forEach(curve => curves.put(CurveId.of(groupName, curve.Name), curve));
		return curves;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableRatesProvider}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableRatesProvider.Meta meta()
	  {
		return ImmutableRatesProvider.Meta.INSTANCE;
	  }

	  static ImmutableRatesProvider()
	  {
		MetaBean.register(ImmutableRatesProvider.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="valuationDate">  the value of the property, not null </param>
	  /// <param name="fxRateProvider">  the value of the property, not null </param>
	  /// <param name="discountCurves">  the value of the property, not null </param>
	  /// <param name="indexCurves">  the value of the property, not null </param>
	  /// <param name="timeSeries">  the value of the property, not null </param>
	  internal ImmutableRatesProvider(LocalDate valuationDate, FxRateProvider fxRateProvider, IDictionary<Currency, Curve> discountCurves, IDictionary<Index, Curve> indexCurves, IDictionary<Index, LocalDateDoubleTimeSeries> timeSeries)
	  {
		JodaBeanUtils.notNull(valuationDate, "valuationDate");
		JodaBeanUtils.notNull(fxRateProvider, "fxRateProvider");
		JodaBeanUtils.notNull(discountCurves, "discountCurves");
		JodaBeanUtils.notNull(indexCurves, "indexCurves");
		JodaBeanUtils.notNull(timeSeries, "timeSeries");
		this.valuationDate = valuationDate;
		this.fxRateProvider = fxRateProvider;
		this.discountCurves = ImmutableMap.copyOf(discountCurves);
		this.indexCurves = ImmutableMap.copyOf(indexCurves);
		this.timeSeries_Renamed = ImmutableMap.copyOf(timeSeries);
	  }

	  public override ImmutableRatesProvider.Meta metaBean()
	  {
		return ImmutableRatesProvider.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date.
	  /// All curves and other data items in this provider are calibrated for this date. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the provider of foreign exchange rates.
	  /// Conversions where both currencies are the same always succeed. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxRateProvider FxRateProvider
	  {
		  get
		  {
			return fxRateProvider;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discount curves, defaulted to an empty map.
	  /// The curve data, predicting the future, associated with each currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Currency, Curve> DiscountCurves
	  {
		  get
		  {
			return discountCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the forward curves, defaulted to an empty map.
	  /// The curve data, predicting the future, associated with each index.
	  /// This is used for Ibor, Overnight and Price indices. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Index, Curve> IndexCurves
	  {
		  get
		  {
			return indexCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time-series, defaulted to an empty map.
	  /// The historic data associated with each index. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Index, LocalDateDoubleTimeSeries> TimeSeries
	  {
		  get
		  {
			return timeSeries_Renamed;
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
		  ImmutableRatesProvider other = (ImmutableRatesProvider) obj;
		  return JodaBeanUtils.equal(valuationDate, other.valuationDate) && JodaBeanUtils.equal(fxRateProvider, other.fxRateProvider) && JodaBeanUtils.equal(discountCurves, other.discountCurves) && JodaBeanUtils.equal(indexCurves, other.indexCurves) && JodaBeanUtils.equal(timeSeries_Renamed, other.timeSeries_Renamed);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fxRateProvider);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(indexCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeSeries_Renamed);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("ImmutableRatesProvider{");
		buf.Append("valuationDate").Append('=').Append(valuationDate).Append(',').Append(' ');
		buf.Append("fxRateProvider").Append('=').Append(fxRateProvider).Append(',').Append(' ');
		buf.Append("discountCurves").Append('=').Append(discountCurves).Append(',').Append(' ');
		buf.Append("indexCurves").Append('=').Append(indexCurves).Append(',').Append(' ');
		buf.Append("timeSeries").Append('=').Append(JodaBeanUtils.ToString(timeSeries_Renamed));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableRatesProvider}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  valuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDate", typeof(ImmutableRatesProvider), typeof(LocalDate));
			  fxRateProvider_Renamed = DirectMetaProperty.ofImmutable(this, "fxRateProvider", typeof(ImmutableRatesProvider), typeof(FxRateProvider));
			  discountCurves_Renamed = DirectMetaProperty.ofImmutable(this, "discountCurves", typeof(ImmutableRatesProvider), (Type) typeof(ImmutableMap));
			  indexCurves_Renamed = DirectMetaProperty.ofImmutable(this, "indexCurves", typeof(ImmutableRatesProvider), (Type) typeof(ImmutableMap));
			  timeSeries_Renamed = DirectMetaProperty.ofImmutable(this, "timeSeries", typeof(ImmutableRatesProvider), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "valuationDate", "fxRateProvider", "discountCurves", "indexCurves", "timeSeries");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code valuationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> valuationDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code fxRateProvider} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxRateProvider> fxRateProvider_Renamed;
		/// <summary>
		/// The meta-property for the {@code discountCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.currency.Currency, com.opengamma.strata.market.curve.Curve>> discountCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "discountCurves", ImmutableRatesProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Currency, Curve>> discountCurves_Renamed;
		/// <summary>
		/// The meta-property for the {@code indexCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.index.Index, com.opengamma.strata.market.curve.Curve>> indexCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "indexCurves", ImmutableRatesProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Index, Curve>> indexCurves_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeSeries} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.index.Index, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries>> timeSeries = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "timeSeries", ImmutableRatesProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Index, LocalDateDoubleTimeSeries>> timeSeries_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "valuationDate", "fxRateProvider", "discountCurves", "indexCurves", "timeSeries");
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
			case 113107279: // valuationDate
			  return valuationDate_Renamed;
			case -1499624221: // fxRateProvider
			  return fxRateProvider_Renamed;
			case -624113147: // discountCurves
			  return discountCurves_Renamed;
			case 886361302: // indexCurves
			  return indexCurves_Renamed;
			case 779431844: // timeSeries
			  return timeSeries_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ImmutableRatesProvider> builder()
		public override BeanBuilder<ImmutableRatesProvider> builder()
		{
		  return new ImmutableRatesProvider.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableRatesProvider);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code valuationDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> valuationDate()
		{
		  return valuationDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fxRateProvider} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxRateProvider> fxRateProvider()
		{
		  return fxRateProvider_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code discountCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Currency, Curve>> discountCurves()
		{
		  return discountCurves_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code indexCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Index, Curve>> indexCurves()
		{
		  return indexCurves_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeSeries} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Index, LocalDateDoubleTimeSeries>> timeSeries()
		{
		  return timeSeries_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 113107279: // valuationDate
			  return ((ImmutableRatesProvider) bean).ValuationDate;
			case -1499624221: // fxRateProvider
			  return ((ImmutableRatesProvider) bean).FxRateProvider;
			case -624113147: // discountCurves
			  return ((ImmutableRatesProvider) bean).DiscountCurves;
			case 886361302: // indexCurves
			  return ((ImmutableRatesProvider) bean).IndexCurves;
			case 779431844: // timeSeries
			  return ((ImmutableRatesProvider) bean).TimeSeries;
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
	  /// The bean-builder for {@code ImmutableRatesProvider}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ImmutableRatesProvider>
	  {

		internal LocalDate valuationDate;
		internal FxRateProvider fxRateProvider;
		internal IDictionary<Currency, Curve> discountCurves = ImmutableMap.of();
		internal IDictionary<Index, Curve> indexCurves = ImmutableMap.of();
		internal IDictionary<Index, LocalDateDoubleTimeSeries> timeSeries = ImmutableMap.of();

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
			case 113107279: // valuationDate
			  return valuationDate;
			case -1499624221: // fxRateProvider
			  return fxRateProvider;
			case -624113147: // discountCurves
			  return discountCurves;
			case 886361302: // indexCurves
			  return indexCurves;
			case 779431844: // timeSeries
			  return timeSeries;
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
			case 113107279: // valuationDate
			  this.valuationDate = (LocalDate) newValue;
			  break;
			case -1499624221: // fxRateProvider
			  this.fxRateProvider = (FxRateProvider) newValue;
			  break;
			case -624113147: // discountCurves
			  this.discountCurves = (IDictionary<Currency, Curve>) newValue;
			  break;
			case 886361302: // indexCurves
			  this.indexCurves = (IDictionary<Index, Curve>) newValue;
			  break;
			case 779431844: // timeSeries
			  this.timeSeries = (IDictionary<Index, LocalDateDoubleTimeSeries>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ImmutableRatesProvider build()
		{
		  return new ImmutableRatesProvider(valuationDate, fxRateProvider, discountCurves, indexCurves, timeSeries);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("ImmutableRatesProvider.Builder{");
		  buf.Append("valuationDate").Append('=').Append(JodaBeanUtils.ToString(valuationDate)).Append(',').Append(' ');
		  buf.Append("fxRateProvider").Append('=').Append(JodaBeanUtils.ToString(fxRateProvider)).Append(',').Append(' ');
		  buf.Append("discountCurves").Append('=').Append(JodaBeanUtils.ToString(discountCurves)).Append(',').Append(' ');
		  buf.Append("indexCurves").Append('=').Append(JodaBeanUtils.ToString(indexCurves)).Append(',').Append(' ');
		  buf.Append("timeSeries").Append('=').Append(JodaBeanUtils.ToString(timeSeries));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}