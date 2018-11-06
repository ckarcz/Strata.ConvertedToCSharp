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
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Sets = com.google.common.collect.Sets;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using Index = com.opengamma.strata.basics.index.Index;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using FxRateLookup = com.opengamma.strata.calc.runner.FxRateLookup;
	using Messages = com.opengamma.strata.collect.Messages;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// The rates lookup, used to select curves for pricing.
	/// <para>
	/// This provides access to discount curves and forward curves.
	/// </para>
	/// <para>
	/// The lookup implements <seealso cref="CalculationParameter"/> and is used by passing it
	/// as an argument to <seealso cref="CalculationRules"/>. It provides the link between the
	/// data that the function needs and the data that is available in <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultRatesMarketDataLookup implements RatesMarketDataLookup, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultRatesMarketDataLookup : RatesMarketDataLookup, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.currency.Currency, com.opengamma.strata.market.curve.CurveId> discountCurves;
		private readonly ImmutableMap<Currency, CurveId> discountCurves;
	  /// <summary>
	  /// The forward curves in the group, keyed by index.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "Map<? extends Index, CurveId>") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.index.Index, com.opengamma.strata.market.curve.CurveId> forwardCurves;
	  private readonly ImmutableMap<Index, CurveId> forwardCurves;
	  /// <summary>
	  /// The source of market data for quotes and other observable market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.data.ObservableSource observableSource;
	  private readonly ObservableSource observableSource;
	  /// <summary>
	  /// The lookup used to obtain {@code FxRateProvider}.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", alias = "fxLookup", overrideGet = true) private final com.opengamma.strata.calc.runner.FxRateLookup fxRateLookup;
	  private readonly FxRateLookup fxRateLookup;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a map of discount and forward curve identifiers.
	  /// <para>
	  /// The discount and forward curves refer to the curve identifier.
	  /// The curves themselves are provided in <seealso cref="ScenarioMarketData"/>
	  /// using <seealso cref="CurveId"/> as the identifier.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="discountCurveIds">  the discount curve identifiers, keyed by currency </param>
	  /// <param name="forwardCurveIds">  the forward curves identifiers, keyed by index </param>
	  /// <param name="obsSource">  the source of market data for FX, quotes and other observable market data </param>
	  /// <param name="fxLookup">  the lookup used to obtain FX rates </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
	  public static DefaultRatesMarketDataLookup of<T1>(IDictionary<Currency, CurveId> discountCurveIds, IDictionary<T1> forwardCurveIds, ObservableSource obsSource, FxRateLookup fxLookup) where T1 : com.opengamma.strata.basics.index.Index
	  {

		return new DefaultRatesMarketDataLookup(discountCurveIds, forwardCurveIds, obsSource, fxLookup);
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<Currency> DiscountCurrencies
	  {
		  get
		  {
			return discountCurves.Keys;
		  }
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getDiscountMarketDataIds(com.opengamma.strata.basics.currency.Currency currency)
	  public ImmutableSet<MarketDataId<object>> getDiscountMarketDataIds(Currency currency)
	  {
		CurveId id = discountCurves.get(currency);
		if (id == null)
		{
		  throw new System.ArgumentException(msgCurrencyNotFound(currency));
		}
		return ImmutableSet.of(id);
	  }

	  public ImmutableSet<Index> ForwardIndices
	  {
		  get
		  {
			return forwardCurves.Keys;
		  }
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public com.google.common.collect.ImmutableSet<com.opengamma.strata.data.MarketDataId<?>> getForwardMarketDataIds(com.opengamma.strata.basics.index.Index index)
	  public ImmutableSet<MarketDataId<object>> getForwardMarketDataIds(Index index)
	  {
		CurveId id = forwardCurves.get(index);
		if (id == null)
		{
		  throw new System.ArgumentException(msgIndexNotFound(index));
		}
		return ImmutableSet.of(id);
	  }

	  //-------------------------------------------------------------------------
	  public FunctionRequirements requirements<T1>(ISet<Currency> currencies, ISet<T1> indices) where T1 : com.opengamma.strata.basics.index.Index
	  {
		foreach (Currency currency in currencies)
		{
		  if (!discountCurves.Keys.Contains(currency))
		  {
			throw new System.ArgumentException(msgCurrencyNotFound(currency));
		  }
		}
		foreach (Index index in indices)
		{
		  if (!forwardCurves.Keys.Contains(index))
		  {
			throw new System.ArgumentException(msgIndexNotFound(index));
		  }
		}

		// keys for time-series
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<ObservableId> indexQuoteIds = indices.Select(IndexQuoteId.of).collect(toImmutableSet());

		// keys for forward curves
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<com.opengamma.strata.data.MarketDataId<?>> indexCurveIds = indices.stream().map(idx -> forwardCurves.get(idx)).collect(toImmutableSet());
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<MarketDataId<object>> indexCurveIds = indices.Select(idx => forwardCurves.get(idx)).collect(toImmutableSet());

		// keys for discount factors
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<com.opengamma.strata.data.MarketDataId<?>> discountFactorsIds = currencies.stream().map(ccy -> discountCurves.get(ccy)).collect(toImmutableSet());
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<MarketDataId<object>> discountFactorsIds = currencies.Select(ccy => discountCurves.get(ccy)).collect(toImmutableSet());

		return FunctionRequirements.builder().valueRequirements(Sets.union(indexCurveIds, discountFactorsIds)).timeSeriesRequirements(indexQuoteIds).outputCurrencies(currencies).observableSource(observableSource).build();
	  }

	  //-------------------------------------------------------------------------
	  public RatesProvider ratesProvider(MarketData marketData)
	  {
		return DefaultLookupRatesProvider.of(this, marketData);
	  }

	  public FxRateProvider fxRateProvider(MarketData marketData)
	  {
		return fxRateLookup.fxRateProvider(marketData);
	  }

	  //-------------------------------------------------------------------------
	  internal string msgCurrencyNotFound(Currency currency)
	  {
		return Messages.format("Rates lookup has no discount curve defined for currency '{}'", currency);
	  }

	  internal string msgIndexNotFound(Index index)
	  {
		return Messages.format("Rates lookup has no forward curve defined for index '{}'", index);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultRatesMarketDataLookup}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultRatesMarketDataLookup> META_BEAN = LightMetaBean.of(typeof(DefaultRatesMarketDataLookup), MethodHandles.lookup(), new string[] {"discountCurves", "forwardCurves", "observableSource", "fxRateLookup"}, ImmutableMap.of(), ImmutableMap.of(), null, null).withAlias("fxLookup", "fxRateLookup");

	  /// <summary>
	  /// The meta-bean for {@code DefaultRatesMarketDataLookup}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultRatesMarketDataLookup> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultRatesMarketDataLookup()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DefaultRatesMarketDataLookup<T1>(IDictionary<Currency, CurveId> discountCurves, IDictionary<T1> forwardCurves, ObservableSource observableSource, FxRateLookup fxRateLookup) where T1 : com.opengamma.strata.basics.index.Index
	  {
		JodaBeanUtils.notNull(discountCurves, "discountCurves");
		JodaBeanUtils.notNull(forwardCurves, "forwardCurves");
		JodaBeanUtils.notNull(observableSource, "observableSource");
		JodaBeanUtils.notNull(fxRateLookup, "fxRateLookup");
		this.discountCurves = ImmutableMap.copyOf(discountCurves);
		this.forwardCurves = ImmutableMap.copyOf(forwardCurves);
		this.observableSource = observableSource;
		this.fxRateLookup = fxRateLookup;
	  }

	  public override TypedMetaBean<DefaultRatesMarketDataLookup> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discount curves in the group, keyed by currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Currency, CurveId> DiscountCurves
	  {
		  get
		  {
			return discountCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the forward curves in the group, keyed by index. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Index, CurveId> ForwardCurves
	  {
		  get
		  {
			return forwardCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the source of market data for quotes and other observable market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override ObservableSource ObservableSource
	  {
		  get
		  {
			return observableSource;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the lookup used to obtain {@code FxRateProvider}. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override FxRateLookup FxRateLookup
	  {
		  get
		  {
			return fxRateLookup;
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
		  DefaultRatesMarketDataLookup other = (DefaultRatesMarketDataLookup) obj;
		  return JodaBeanUtils.equal(discountCurves, other.discountCurves) && JodaBeanUtils.equal(forwardCurves, other.forwardCurves) && JodaBeanUtils.equal(observableSource, other.observableSource) && JodaBeanUtils.equal(fxRateLookup, other.fxRateLookup);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(forwardCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observableSource);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fxRateLookup);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("DefaultRatesMarketDataLookup{");
		buf.Append("discountCurves").Append('=').Append(discountCurves).Append(',').Append(' ');
		buf.Append("forwardCurves").Append('=').Append(forwardCurves).Append(',').Append(' ');
		buf.Append("observableSource").Append('=').Append(observableSource).Append(',').Append(' ');
		buf.Append("fxRateLookup").Append('=').Append(JodaBeanUtils.ToString(fxRateLookup));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}