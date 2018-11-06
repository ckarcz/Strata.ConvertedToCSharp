using System;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Provides FX rates from market data.
	/// <para>
	/// This decorates an instance of <seealso cref="MarketData"/> to provide FX rates.
	/// </para>
	/// <para>
	/// The FX rates provided are obtained via a triangulation process.
	/// To find the FX rate for the currency pair AAA/BBB:
	/// <ol>
	/// <li>If the rate AAA/BBB is available, return it
	/// <li>Find the triangulation currency of AAA (TTA), try to return rate from AAA/TTA and TTA/BBB
	/// <li>Find the triangulation currency of BBB (TTB), try to return rate from AAA/TTB and TTB/BBB
	/// <li>Find the triangulation currency of AAA (TTA) and BBB (TTB), try to return rate from AAA/TTA, TTA/TTB and TTB/BBB
	/// </ol>
	/// The triangulation currency can also be specified, which is useful if all
	/// FX rates are supplied relative to a currency other than USD.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class MarketDataFxRateProvider implements com.opengamma.strata.basics.currency.FxRateProvider, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class MarketDataFxRateProvider : FxRateProvider, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final MarketData marketData;
		private readonly MarketData marketData;
	  /// <summary>
	  /// The source of market data for FX rates.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ObservableSource fxRatesSource;
	  private readonly ObservableSource fxRatesSource;
	  /// <summary>
	  /// The triangulation currency to use.
	  /// <para>
	  /// If specified, this currency is used to triangulate FX rates in preference to the standard approach.
	  /// This would be useful if all FX rates are supplied relative to a currency other than USD.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.currency.Currency triangulationCurrency;
	  private readonly Currency triangulationCurrency;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance which takes FX rates from the market data.
	  /// </summary>
	  /// <param name="marketData">  market data used for looking up FX rates </param>
	  /// <returns> the provider </returns>
	  public static MarketDataFxRateProvider of(MarketData marketData)
	  {
		return of(marketData, ObservableSource.NONE);
	  }

	  /// <summary>
	  /// Obtains an instance which takes FX rates from the market data,
	  /// specifying the source of FX rates.
	  /// <para>
	  /// The source of FX rates is rarely needed, as most applications only need one set of FX rates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  market data used for looking up FX rates </param>
	  /// <param name="fxRatesSource">  the source of market data for FX rates </param>
	  /// <returns> the provider </returns>
	  public static MarketDataFxRateProvider of(MarketData marketData, ObservableSource fxRatesSource)
	  {
		return new MarketDataFxRateProvider(marketData, fxRatesSource, null);
	  }

	  /// <summary>
	  /// Obtains an instance which takes FX rates from the market data,
	  /// specifying the source of FX rates.
	  /// <para>
	  /// The source of FX rates is rarely needed, as most applications only need one set of FX rates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  market data used for looking up FX rates </param>
	  /// <param name="fxRatesSource">  the source of market data for FX rates </param>
	  /// <param name="triangulationCurrency">  the triangulation currency to use </param>
	  /// <returns> the provider </returns>
	  public static MarketDataFxRateProvider of(MarketData marketData, ObservableSource fxRatesSource, Currency triangulationCurrency)
	  {

		ArgChecker.notNull(triangulationCurrency, "triangulationCurrency");
		return new MarketDataFxRateProvider(marketData, fxRatesSource, triangulationCurrency);
	  }

	  //-------------------------------------------------------------------------
	  public double fxRate(Currency baseCurrency, Currency counterCurrency)
	  {
		if (baseCurrency.Equals(counterCurrency))
		{
		  return 1;
		}
		// Try direct pair
		Optional<FxRate> rate = marketData.findValue(FxRateId.of(baseCurrency, counterCurrency, fxRatesSource));
		if (rate.Present)
		{
		  return rate.get().fxRate(baseCurrency, counterCurrency);
		}
		// try specified triangulation currency
		if (triangulationCurrency != null)
		{
		  Optional<FxRate> rateBase1 = marketData.findValue(FxRateId.of(baseCurrency, triangulationCurrency, fxRatesSource));
		  Optional<FxRate> rateBase2 = marketData.findValue(FxRateId.of(triangulationCurrency, counterCurrency, fxRatesSource));
		  if (rateBase1.Present && rateBase2.Present)
		  {
			return rateBase1.get().crossRate(rateBase2.get()).fxRate(baseCurrency, counterCurrency);
		  }
		}
		// Try triangulation on base currency
		Currency triangularBaseCcy = baseCurrency.TriangulationCurrency;
		Optional<FxRate> rateBase1 = marketData.findValue(FxRateId.of(baseCurrency, triangularBaseCcy, fxRatesSource));
		Optional<FxRate> rateBase2 = marketData.findValue(FxRateId.of(triangularBaseCcy, counterCurrency, fxRatesSource));
		if (rateBase1.Present && rateBase2.Present)
		{
		  return rateBase1.get().crossRate(rateBase2.get()).fxRate(baseCurrency, counterCurrency);
		}
		// Try triangulation on counter currency
		Currency triangularCounterCcy = counterCurrency.TriangulationCurrency;
		Optional<FxRate> rateCounter1 = marketData.findValue(FxRateId.of(baseCurrency, triangularCounterCcy, fxRatesSource));
		Optional<FxRate> rateCounter2 = marketData.findValue(FxRateId.of(triangularCounterCcy, counterCurrency, fxRatesSource));
		if (rateCounter1.Present && rateCounter2.Present)
		{
		  return rateCounter1.get().crossRate(rateCounter2.get()).fxRate(baseCurrency, counterCurrency);
		}
		// Double triangulation
		if (rateBase1.Present && rateCounter2.Present)
		{
		  Optional<FxRate> rateTriangular2 = marketData.findValue(FxRateId.of(triangularBaseCcy, triangularCounterCcy, fxRatesSource));
		  if (rateTriangular2.Present)
		  {
			return rateBase1.get().crossRate(rateTriangular2.get()).crossRate(rateCounter2.get()).fxRate(baseCurrency, counterCurrency);
		  }
		}
		if (fxRatesSource.Equals(ObservableSource.NONE))
		{
		  throw new MarketDataNotFoundException(Messages.format("No FX rate market data for {}/{}", baseCurrency, counterCurrency));
		}
		throw new MarketDataNotFoundException(Messages.format("No FX rate market data for {}/{} using source '{}'", baseCurrency, counterCurrency, fxRatesSource));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code MarketDataFxRateProvider}.
	  /// </summary>
	  private static readonly TypedMetaBean<MarketDataFxRateProvider> META_BEAN = LightMetaBean.of(typeof(MarketDataFxRateProvider), MethodHandles.lookup(), new string[] {"marketData", "fxRatesSource", "triangulationCurrency"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code MarketDataFxRateProvider}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<MarketDataFxRateProvider> meta()
	  {
		return META_BEAN;
	  }

	  static MarketDataFxRateProvider()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private MarketDataFxRateProvider(MarketData marketData, ObservableSource fxRatesSource, Currency triangulationCurrency)
	  {
		JodaBeanUtils.notNull(marketData, "marketData");
		JodaBeanUtils.notNull(fxRatesSource, "fxRatesSource");
		this.marketData = marketData;
		this.fxRatesSource = fxRatesSource;
		this.triangulationCurrency = triangulationCurrency;
	  }

	  public override TypedMetaBean<MarketDataFxRateProvider> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data that provides the FX rates. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public MarketData MarketData
	  {
		  get
		  {
			return marketData;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the source of market data for FX rates. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableSource FxRatesSource
	  {
		  get
		  {
			return fxRatesSource;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the triangulation currency to use.
	  /// <para>
	  /// If specified, this currency is used to triangulate FX rates in preference to the standard approach.
	  /// This would be useful if all FX rates are supplied relative to a currency other than USD.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<Currency> TriangulationCurrency
	  {
		  get
		  {
			return Optional.ofNullable(triangulationCurrency);
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
		  MarketDataFxRateProvider other = (MarketDataFxRateProvider) obj;
		  return JodaBeanUtils.equal(marketData, other.marketData) && JodaBeanUtils.equal(fxRatesSource, other.fxRatesSource) && JodaBeanUtils.equal(triangulationCurrency, other.triangulationCurrency);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(marketData);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fxRatesSource);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(triangulationCurrency);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("MarketDataFxRateProvider{");
		buf.Append("marketData").Append('=').Append(marketData).Append(',').Append(' ');
		buf.Append("fxRatesSource").Append('=').Append(fxRatesSource).Append(',').Append(' ');
		buf.Append("triangulationCurrency").Append('=').Append(JodaBeanUtils.ToString(triangulationCurrency));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}