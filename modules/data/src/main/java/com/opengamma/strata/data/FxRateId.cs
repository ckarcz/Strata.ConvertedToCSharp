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
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Identifies the market data for an FX rate.
	/// <para>
	/// The currency pair in a rate ID is always the market convention currency pair. If an ID is
	/// created using a non-conventional pair, the pair is inverted. This has no effect on the
	/// <seealso cref="FxRate"/> identified by the ID as it can handle both currency pairs that can be
	/// created from the two currencies.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light", cacheHashCode = true) public final class FxRateId implements MarketDataId<com.opengamma.strata.basics.currency.FxRate>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxRateId : MarketDataId<FxRate>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyPair pair;
		private readonly CurrencyPair pair;
	  /// <summary>
	  /// The source of observable market data.
	  /// This is used when looking up the underlying market quotes for the rate.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ObservableSource observableSource;
	  private readonly ObservableSource observableSource;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance representing the FX rate for a currency pair.
	  /// </summary>
	  /// <param name="currencyPair">  a currency pair </param>
	  /// <returns> an ID for the FX rate for the currency pair </returns>
	  public static FxRateId of(CurrencyPair currencyPair)
	  {
		return new FxRateId(currencyPair, ObservableSource.NONE);
	  }

	  /// <summary>
	  /// Obtains an instance representing the FX rate for a currency pair.
	  /// </summary>
	  /// <param name="base">  the base currency of the pair </param>
	  /// <param name="counter">  the counter currency of the pair </param>
	  /// <returns> an ID for the FX rate for the currency pair </returns>
	  public static FxRateId of(Currency @base, Currency counter)
	  {
		return new FxRateId(CurrencyPair.of(@base, counter), ObservableSource.NONE);
	  }

	  /// <summary>
	  /// Obtains an instance representing the FX rate for a currency pair, specifying the source.
	  /// </summary>
	  /// <param name="currencyPair">  a currency pair </param>
	  /// <param name="observableSource">  the source of the observable market data used to create the rate </param>
	  /// <returns> an ID for the FX rate for the currency pair </returns>
	  public static FxRateId of(CurrencyPair currencyPair, ObservableSource observableSource)
	  {
		return new FxRateId(currencyPair, observableSource);
	  }

	  /// <summary>
	  /// Obtains an instance representing the FX rate for a currency pair, specifying the source.
	  /// </summary>
	  /// <param name="base">  the base currency of the pair </param>
	  /// <param name="counter">  the counter currency of the pair </param>
	  /// <param name="observableSource">  the source of the observable market data used to create the rate </param>
	  /// <returns> an ID for the FX rate for the currency pair </returns>
	  public static FxRateId of(Currency @base, Currency counter, ObservableSource observableSource)
	  {
		return new FxRateId(CurrencyPair.of(@base, counter), observableSource);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private FxRateId(com.opengamma.strata.basics.currency.CurrencyPair currencyPair, ObservableSource observableSource)
	  private FxRateId(CurrencyPair currencyPair, ObservableSource observableSource)
	  {
		ArgChecker.notNull(currencyPair, "currencyPair");
		ArgChecker.notNull(observableSource, "observableSource");
		this.pair = currencyPair.toConventional();
		this.observableSource = observableSource;
	  }

	  //-------------------------------------------------------------------------
	  public Type<FxRate> MarketDataType
	  {
		  get
		  {
			return typeof(FxRate);
		  }
	  }

	  public override string ToString()
	  {
		return (new StringBuilder(32)).Append("FxRateId:").Append(pair).Append(observableSource.Equals(ObservableSource.NONE) ? "" : "/" + observableSource).ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxRateId}.
	  /// </summary>
	  private static readonly TypedMetaBean<FxRateId> META_BEAN = LightMetaBean.of(typeof(FxRateId), MethodHandles.lookup(), new string[] {"pair", "observableSource"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code FxRateId}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<FxRateId> meta()
	  {
		return META_BEAN;
	  }

	  static FxRateId()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The cached hash code, using the racy single-check idiom.
	  /// </summary>
	  [NonSerialized]
	  private int cacheHashCode;

	  public override TypedMetaBean<FxRateId> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair that is required.
	  /// For example, 'GBP/USD'. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyPair Pair
	  {
		  get
		  {
			return pair;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the source of observable market data.
	  /// This is used when looking up the underlying market quotes for the rate. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableSource ObservableSource
	  {
		  get
		  {
			return observableSource;
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
		  FxRateId other = (FxRateId) obj;
		  return JodaBeanUtils.equal(pair, other.pair) && JodaBeanUtils.equal(observableSource, other.observableSource);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = cacheHashCode;
		if (hash == 0)
		{
		  hash = this.GetType().GetHashCode();
		  hash = hash * 31 + JodaBeanUtils.GetHashCode(pair);
		  hash = hash * 31 + JodaBeanUtils.GetHashCode(observableSource);
		  cacheHashCode = hash;
		}
		return hash;
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}