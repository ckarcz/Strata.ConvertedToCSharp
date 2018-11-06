using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// A set of FX rates between two currencies containing rates for multiple scenarios.
	/// <para>
	/// This represents rates of foreign exchange. The rate 'EUR/USD 1.25' consists of three
	/// elements - the base currency 'EUR', the counter currency 'USD' and the rate '1.25'.
	/// When performing a conversion a rate of '1.25' means that '1 EUR = 1.25 USD'.
	/// </para>
	/// <para>
	/// The <seealso cref="FxRate"/> class represents a single rate for a currency pair. This class is
	/// intended as an efficient way of storing multiple rates for the same currency pair
	/// for use in multiple scenarios.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= FxRate </seealso>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FxRateScenarioArray implements ScenarioArray<com.opengamma.strata.basics.currency.FxRate>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxRateScenarioArray : ScenarioArray<FxRate>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyPair pair;
		private readonly CurrencyPair pair;

	  /// <summary>
	  /// The rates applicable to the currency pair.
	  /// One unit of the base currency is exchanged for this amount of the counter currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", get = "private") private final com.opengamma.strata.collect.array.DoubleArray rates;
	  private readonly DoubleArray rates;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an array of FX rates for a currency pair.
	  /// <para>
	  /// The rates are the rates from the base currency to the counter currency
	  /// as defined by this formula: {@code (1 * baseCurrency = fxRate * counterCurrency)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="rates">  the FX rates for the currency pair </param>
	  /// <returns> an array of FX rates for a currency pair </returns>
	  public static FxRateScenarioArray of(CurrencyPair currencyPair, DoubleArray rates)
	  {
		return new FxRateScenarioArray(currencyPair, rates);
	  }

	  /// <summary>
	  /// Returns an array of FX rates for a currency pair.
	  /// <para>
	  /// The rates are the rates from the base currency to the counter currency
	  /// as defined by this formula: {@code (1 * baseCurrency = fxRate * counterCurrency)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="base">  the base currency of the pair </param>
	  /// <param name="counter">  the counter currency of the pair </param>
	  /// <param name="rates">  the FX rates for the currency pair </param>
	  /// <returns> an array of FX rates for a currency pair </returns>
	  public static FxRateScenarioArray of(Currency @base, Currency counter, DoubleArray rates)
	  {
		return new FxRateScenarioArray(CurrencyPair.of(@base, counter), rates);
	  }

	  //-------------------------------------------------------------------------
	  public int ScenarioCount
	  {
		  get
		  {
			return rates.size();
		  }
	  }

	  /// <summary>
	  /// Returns the FX rate for a scenario.
	  /// </summary>
	  /// <param name="scenarioIndex">  the index of the scenario </param>
	  /// <returns> the FX rate for the specified scenario </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public FxRate get(int scenarioIndex)
	  {
		return FxRate.of(pair, rates.get(scenarioIndex));
	  }

	  public override Stream<FxRate> stream()
	  {
		return rates.stream().mapToObj(rate => FxRate.of(pair, rate));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the FX rate for the specified currency pair and scenario index.
	  /// <para>
	  /// The rate returned is the rate from the base currency to the counter currency
	  /// as defined by this formula: {@code (1 * baseCurrency = fxRate * counterCurrency)}.
	  /// </para>
	  /// <para>
	  /// This will return the rate or inverse rate, or 1 if the two input currencies are the same.
	  /// </para>
	  /// <para>
	  /// This method is more efficient than <seealso cref="#get(int)"/> as it doesn't create an instance
	  /// of <seealso cref="FxRate"/> for every invocation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseCurrency">  the base currency, to convert from </param>
	  /// <param name="counterCurrency">  the counter currency, to convert to </param>
	  /// <param name="scenarioIndex">  the index of the scenario for which rates are required </param>
	  /// <returns> the FX rate for the currency pair </returns>
	  /// <exception cref="IllegalArgumentException"> if no FX rate could be found </exception>
	  public double fxRate(Currency baseCurrency, Currency counterCurrency, int scenarioIndex)
	  {
		if (baseCurrency.Equals(pair.Base) && counterCurrency.Equals(pair.Counter))
		{
		  return rates.get(scenarioIndex);
		}
		if (counterCurrency.Equals(pair.Base) && baseCurrency.Equals(pair.Counter))
		{
		  return 1d / rates.get(scenarioIndex);
		}
		throw new System.ArgumentException("Unknown rate: " + baseCurrency + "/" + counterCurrency);
	  }

	  /// <summary>
	  /// Converts an amount in a currency to an amount in a different currency using this rate.
	  /// <para>
	  /// The from and to currencies must be the same as this rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amounts">  the amounts in {@code fromCurrency} to convert </param>
	  /// <param name="fromCurrency">  the currency of the amounts </param>
	  /// <param name="toCurrency">  the currency into which the amount should be converted </param>
	  /// <returns> the amount converted into {@code toCurrency} </returns>
	  /// <exception cref="IllegalArgumentException"> if one or both input currencies are not in part of this rate </exception>
	  public DoubleArray convert(DoubleArray amounts, Currency fromCurrency, Currency toCurrency)
	  {
		if (fromCurrency.Equals(pair.Base) && toCurrency.Equals(pair.Counter))
		{
		  return amounts.multipliedBy(rates);
		}
		if (toCurrency.Equals(pair.Base) && fromCurrency.Equals(pair.Counter))
		{
		  return rates.mapWithIndex((i, v) => amounts.get(i) / v);
		}
		throw new System.ArgumentException("Unknown rate: " + fromCurrency + "/" + toCurrency);
	  }

	  /// <summary>
	  /// Derives a set of FX rates from these rates and another set of rates.
	  /// <para>
	  /// For example, given rates for EUR/GBP and EUR/CHF it is possible to derive rates for GBP/CHF.
	  /// </para>
	  /// <para>
	  /// There must be exactly one currency in common between the two currency pairs and
	  /// each pair must contain two different currencies. The other rates must have the same scenario count
	  /// as these rates.
	  /// </para>
	  /// <para>
	  /// The returned object contains rates for converting between the two currencies which only appear in
	  /// one set of rates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other rates </param>
	  /// <returns> a set of FX rates derived from these rates and the other rates </returns>
	  public FxRateScenarioArray crossRates(FxRateScenarioArray other)
	  {
		return pair.cross(other.pair).map(cross => computeCross(other, cross)).orElseThrow(() => new System.ArgumentException(Messages.format("Unable to cross when no unique common currency: {} and {}", pair, other.pair)));
	  }

	  private FxRateScenarioArray computeCross(FxRateScenarioArray other, CurrencyPair crossPairAC)
	  {
		// aim is to convert AAA/BBB and BBB/CCC to AAA/CCC
		Currency currA = crossPairAC.Base;
		Currency currC = crossPairAC.Counter;
		// given the conventional cross rate pair, order the two rates to match
		bool crossBaseCurrencyInFx1 = pair.contains(currA);
		FxRateScenarioArray fxABorBA = crossBaseCurrencyInFx1 ? this : other;
		FxRateScenarioArray fxBCorCB = crossBaseCurrencyInFx1 ? other : this;
		// extract the rates, taking the inverse if the pair is in the inverse order
		DoubleArray ratesAB = fxABorBA.Pair.Base.Equals(currA) ? fxABorBA.rates : fxABorBA.rates.map(v => 1 / v);
		DoubleArray ratesBC = fxBCorCB.Pair.Counter.Equals(currC) ? fxBCorCB.rates : fxBCorCB.rates.map(v => 1 / v);
		return FxRateScenarioArray.of(crossPairAC, ratesAB.multipliedBy(ratesBC));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (pair.Base.Equals(pair.Counter) && !rates.All(v => v == 1d))
		{
		  throw new System.ArgumentException("Conversion rate between identical currencies must be one");
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxRateScenarioArray}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxRateScenarioArray.Meta meta()
	  {
		return FxRateScenarioArray.Meta.INSTANCE;
	  }

	  static FxRateScenarioArray()
	  {
		MetaBean.register(FxRateScenarioArray.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FxRateScenarioArray(CurrencyPair pair, DoubleArray rates)
	  {
		JodaBeanUtils.notNull(pair, "pair");
		JodaBeanUtils.notNull(rates, "rates");
		this.pair = pair;
		this.rates = rates;
		validate();
	  }

	  public override FxRateScenarioArray.Meta metaBean()
	  {
		return FxRateScenarioArray.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair.
	  /// The pair is formed of two parts, the base and the counter.
	  /// In the pair 'AAA/BBB' the base is 'AAA' and the counter is 'BBB'. </summary>
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
	  /// Gets the rates applicable to the currency pair.
	  /// One unit of the base currency is exchanged for this amount of the counter currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  private DoubleArray Rates
	  {
		  get
		  {
			return rates;
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
		  FxRateScenarioArray other = (FxRateScenarioArray) obj;
		  return JodaBeanUtils.equal(pair, other.pair) && JodaBeanUtils.equal(rates, other.rates);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(pair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rates);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("FxRateScenarioArray{");
		buf.Append("pair").Append('=').Append(pair).Append(',').Append(' ');
		buf.Append("rates").Append('=').Append(JodaBeanUtils.ToString(rates));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxRateScenarioArray}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  pair_Renamed = DirectMetaProperty.ofImmutable(this, "pair", typeof(FxRateScenarioArray), typeof(CurrencyPair));
			  rates_Renamed = DirectMetaProperty.ofImmutable(this, "rates", typeof(FxRateScenarioArray), typeof(DoubleArray));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "pair", "rates");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code pair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> pair_Renamed;
		/// <summary>
		/// The meta-property for the {@code rates} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> rates_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "pair", "rates");
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
			case 3433178: // pair
			  return pair_Renamed;
			case 108285843: // rates
			  return rates_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxRateScenarioArray> builder()
		public override BeanBuilder<FxRateScenarioArray> builder()
		{
		  return new FxRateScenarioArray.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxRateScenarioArray);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code pair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> pair()
		{
		  return pair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rates} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> rates()
		{
		  return rates_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3433178: // pair
			  return ((FxRateScenarioArray) bean).Pair;
			case 108285843: // rates
			  return ((FxRateScenarioArray) bean).Rates;
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
	  /// The bean-builder for {@code FxRateScenarioArray}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxRateScenarioArray>
	  {

		internal CurrencyPair pair;
		internal DoubleArray rates;

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
			case 3433178: // pair
			  return pair;
			case 108285843: // rates
			  return rates;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3433178: // pair
			  this.pair = (CurrencyPair) newValue;
			  break;
			case 108285843: // rates
			  this.rates = (DoubleArray) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FxRateScenarioArray build()
		{
		  return new FxRateScenarioArray(pair, rates);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("FxRateScenarioArray.Builder{");
		  buf.Append("pair").Append('=').Append(JodaBeanUtils.ToString(pair)).Append(',').Append(' ');
		  buf.Append("rates").Append('=').Append(JodaBeanUtils.ToString(rates));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}