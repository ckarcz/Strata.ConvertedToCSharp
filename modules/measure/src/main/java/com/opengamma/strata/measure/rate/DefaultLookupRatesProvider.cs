using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
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
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using DiscountFactors = com.opengamma.strata.pricer.DiscountFactors;
	using DiscountFxForwardRates = com.opengamma.strata.pricer.fx.DiscountFxForwardRates;
	using ForwardFxIndexRates = com.opengamma.strata.pricer.fx.ForwardFxIndexRates;
	using FxForwardRates = com.opengamma.strata.pricer.fx.FxForwardRates;
	using FxIndexRates = com.opengamma.strata.pricer.fx.FxIndexRates;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using OvernightIndexRates = com.opengamma.strata.pricer.rate.OvernightIndexRates;
	using PriceIndexValues = com.opengamma.strata.pricer.rate.PriceIndexValues;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// A rates provider based on a rates lookup.
	/// <para>
	/// This uses a <seealso cref="DefaultRatesMarketDataLookup"/> to provide a view on <seealso cref="MarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultLookupRatesProvider implements com.opengamma.strata.pricer.rate.RatesProvider, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultLookupRatesProvider : RatesProvider, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final DefaultRatesMarketDataLookup lookup;
		private readonly DefaultRatesMarketDataLookup lookup;
	  /// <summary>
	  /// The market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.MarketData marketData;
	  private readonly MarketData marketData;
	  /// <summary>
	  /// The FX rate provider.
	  /// </summary>
	  [NonSerialized]
	  private readonly FxRateProvider fxRateProvider; // derived

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a lookup and market data.
	  /// <para>
	  /// The lookup provides the mapping from currency to discount curve, and from
	  /// index to forward curve. The curves are in the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="lookup">  the lookup </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the rates provider </returns>
	  public static DefaultLookupRatesProvider of(DefaultRatesMarketDataLookup lookup, MarketData marketData)
	  {
		return new DefaultLookupRatesProvider(lookup, marketData);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private DefaultLookupRatesProvider(DefaultRatesMarketDataLookup lookup, com.opengamma.strata.data.MarketData marketData)
	  private DefaultLookupRatesProvider(DefaultRatesMarketDataLookup lookup, MarketData marketData)
	  {
		this.lookup = ArgChecker.notNull(lookup, "lookup");
		this.marketData = ArgChecker.notNull(marketData, "marketData");
		this.fxRateProvider = lookup.fxRateProvider(marketData);
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new DefaultLookupRatesProvider(lookup, marketData);
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return marketData.ValuationDate;
		  }
	  }

	  public ImmutableSet<Currency> DiscountCurrencies
	  {
		  get
		  {
			return lookup.DiscountCurrencies;
		  }
	  }

	  public ImmutableSet<IborIndex> IborIndices
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return lookup.ForwardIndices.Where(typeof(IborIndex).isInstance).Select(typeof(IborIndex).cast).collect(toImmutableSet());
		  }
	  }

	  public ImmutableSet<OvernightIndex> OvernightIndices
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return lookup.ForwardIndices.Where(typeof(OvernightIndex).isInstance).Select(typeof(OvernightIndex).cast).collect(toImmutableSet());
		  }
	  }

	  public ImmutableSet<PriceIndex> PriceIndices
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return lookup.ForwardIndices.Where(typeof(PriceIndex).isInstance).Select(typeof(PriceIndex).cast).collect(toImmutableSet());
		  }
	  }

	  public ImmutableSet<Index> TimeSeriesIndices
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return marketData.TimeSeriesIds.Where(typeof(IndexQuoteId).isInstance).Select(typeof(IndexQuoteId).cast).Select(id => id.Index).collect(toImmutableSet());
		  }
	  }

	  //-------------------------------------------------------------------------
	  public T data<T>(MarketDataId<T> key)
	  {
		return marketData.getValue(key);
	  }

	  //-------------------------------------------------------------------------
	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		return Stream.concat(lookup.DiscountCurves.values().stream(), lookup.ForwardCurves.values().stream()).filter(id => id.MarketDataName.Equals(name)).findFirst().flatMap(id => marketData.findValue(id)).map(v => name.MarketDataType.cast(v));
	  }

	  public LocalDateDoubleTimeSeries timeSeries(Index index)
	  {
		return marketData.getTimeSeries(IndexQuoteId.of(index));
	  }

	  //-------------------------------------------------------------------------
	  public double fxRate(Currency baseCurrency, Currency counterCurrency)
	  {
		return fxRateProvider.fxRate(baseCurrency, counterCurrency);
	  }

	  //-------------------------------------------------------------------------
	  public DiscountFactors discountFactors(Currency currency)
	  {
		CurveId curveId = lookup.DiscountCurves.get(currency);
		if (curveId == null)
		{
		  throw new MarketDataNotFoundException(lookup.msgCurrencyNotFound(currency));
		}
		Curve curve = marketData.getValue(curveId);
		return DiscountFactors.of(currency, ValuationDate, curve);
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
		FxRate fxRate = FxRate.of(currencyPair, fxRate(currencyPair));
		return DiscountFxForwardRates.of(currencyPair, fxRate, @base, counter);
	  };

	  //-------------------------------------------------------------------------
	  public IborIndexRates iborIndexRates(IborIndex index)
	  {
		CurveId curveId = lookup.ForwardCurves.get(index);
		if (curveId == null)
		{
		  throw new MarketDataNotFoundException(lookup.msgIndexNotFound(index));
		}
		Curve curve = marketData.getValue(curveId);
		return IborIndexRates.of(index, ValuationDate, curve, timeSeries(index));
	  }

	  //-------------------------------------------------------------------------
	  public OvernightIndexRates overnightIndexRates(OvernightIndex index)
	  {
		CurveId curveId = lookup.ForwardCurves.get(index);
		if (curveId == null)
		{
		  throw new MarketDataNotFoundException(lookup.msgIndexNotFound(index));
		}
		Curve curve = marketData.getValue(curveId);
		return OvernightIndexRates.of(index, ValuationDate, curve, timeSeries(index));
	  }

	  //-------------------------------------------------------------------------
	  public PriceIndexValues priceIndexValues(PriceIndex index)
	  {
		CurveId curveId = lookup.ForwardCurves.get(index);
		if (curveId == null)
		{
		  throw new MarketDataNotFoundException(lookup.msgIndexNotFound(index));
		}
		Curve curve = marketData.getValue(curveId);
		return PriceIndexValues.of(index, ValuationDate, curve, timeSeries(index));
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableRatesProvider toImmutableRatesProvider()
	  {
		// discount curves
		IDictionary<Currency, Curve> dscMap = new Dictionary<Currency, Curve>();
		foreach (Currency currency in lookup.DiscountCurrencies)
		{
		  CurveId curveId = lookup.DiscountCurves.get(currency);
		  if (curveId != null && marketData.containsValue(curveId))
		  {
			dscMap[currency] = marketData.getValue(curveId);
		  }
		}
		// forward curves
		IDictionary<Index, Curve> fwdMap = new Dictionary<Index, Curve>();
		foreach (Index index in lookup.ForwardIndices)
		{
		  CurveId curveId = lookup.ForwardCurves.get(index);
		  if (curveId != null && marketData.containsValue(curveId))
		  {
			fwdMap[index] = marketData.getValue(curveId);
		  }
		}
		// time-series
		IDictionary<Index, LocalDateDoubleTimeSeries> tsMap = new Dictionary<Index, LocalDateDoubleTimeSeries>();
		foreach (ObservableId id in marketData.TimeSeriesIds)
		{
		  if (id is IndexQuoteId)
		  {
			IndexQuoteId indexId = (IndexQuoteId) id;
			tsMap[indexId.Index] = marketData.getTimeSeries(id);
		  }
		}
		// build result
		return ImmutableRatesProvider.builder(ValuationDate).discountCurves(dscMap).indexCurves(fwdMap).timeSeries(tsMap).fxRateProvider(fxRateProvider).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultLookupRatesProvider}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultLookupRatesProvider> META_BEAN = LightMetaBean.of(typeof(DefaultLookupRatesProvider), MethodHandles.lookup(), new string[] {"lookup", "marketData"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code DefaultLookupRatesProvider}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultLookupRatesProvider> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultLookupRatesProvider()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override TypedMetaBean<DefaultLookupRatesProvider> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the lookup. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DefaultRatesMarketDataLookup Lookup
	  {
		  get
		  {
			return lookup;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public MarketData MarketData
	  {
		  get
		  {
			return marketData;
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
		  DefaultLookupRatesProvider other = (DefaultLookupRatesProvider) obj;
		  return JodaBeanUtils.equal(lookup, other.lookup) && JodaBeanUtils.equal(marketData, other.marketData);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lookup);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(marketData);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("DefaultLookupRatesProvider{");
		buf.Append("lookup").Append('=').Append(lookup).Append(',').Append(' ');
		buf.Append("marketData").Append('=').Append(JodaBeanUtils.ToString(marketData));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}