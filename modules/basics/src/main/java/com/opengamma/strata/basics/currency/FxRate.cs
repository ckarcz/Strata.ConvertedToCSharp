using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
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

	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A single foreign exchange rate between two currencies, such as 'EUR/USD 1.25'.
	/// <para>
	/// This represents a rate of foreign exchange. The rate 'EUR/USD 1.25' consists of three
	/// elements - the base currency 'EUR', the counter currency 'USD' and the rate '1.25'.
	/// When performing a conversion a rate of '1.25' means that '1 EUR = 1.25 USD'.
	/// </para>
	/// <para>
	/// See <seealso cref="CurrencyPair"/> for the representation that does not contain a rate.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FxRate implements FxRateProvider, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxRate : FxRateProvider, ImmutableBean
	{

	  /// <summary>
	  /// Regular expression to parse the textual format.
	  /// </summary>
	  private static readonly Pattern REGEX_FORMAT = Pattern.compile("([A-Z]{3})[/]([A-Z]{3})[ ]([0-9+.-]+)");

	  /// <summary>
	  /// The currency pair.
	  /// The pair is formed of two parts, the base and the counter.
	  /// In the pair 'AAA/BBB' the base is 'AAA' and the counter is 'BBB'.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CurrencyPair pair;
	  private readonly CurrencyPair pair;
	  /// <summary>
	  /// The rate applicable to the currency pair.
	  /// One unit of the base currency is exchanged for this amount of the counter currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero", get = "private") private final double rate;
	  private readonly double rate;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from two currencies.
	  /// <para>
	  /// The first currency is the base and the second is the counter.
	  /// The two currencies may be the same, but if they are then the rate must be one.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="base">  the base currency </param>
	  /// <param name="counter">  the counter currency </param>
	  /// <param name="rate">  the conversion rate, greater than zero </param>
	  /// <returns> the FX rate </returns>
	  /// <exception cref="IllegalArgumentException"> if the rate is invalid </exception>
	  public static FxRate of(Currency @base, Currency counter, double rate)
	  {
		return new FxRate(CurrencyPair.of(@base, counter), rate);
	  }

	  /// <summary>
	  /// Obtains an instance from a currency pair.
	  /// <para>
	  /// The two currencies may be the same, but if they are then the rate must be one.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pair">  the currency pair </param>
	  /// <param name="rate">  the conversion rate, greater than zero </param>
	  /// <returns> the FX rate </returns>
	  /// <exception cref="IllegalArgumentException"> if the rate is invalid </exception>
	  public static FxRate of(CurrencyPair pair, double rate)
	  {
		return new FxRate(pair, rate);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses a rate from a string with format AAA/BBB RATE.
	  /// <para>
	  /// The parsed format is '${baseCurrency}/${counterCurrency} ${rate}'.
	  /// Currency parsing is case insensitive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rateStr">  the rate as a string AAA/BBB RATE </param>
	  /// <returns> the FX rate </returns>
	  /// <exception cref="IllegalArgumentException"> if the FX rate cannot be parsed </exception>
	  public static FxRate parse(string rateStr)
	  {
		ArgChecker.notNull(rateStr, "rateStr");
		Matcher matcher = REGEX_FORMAT.matcher(rateStr.ToUpper(Locale.ENGLISH));
		if (!matcher.matches())
		{
		  throw new System.ArgumentException("Invalid rate: " + rateStr);
		}
		try
		{
		  Currency @base = Currency.parse(matcher.group(1));
		  Currency counter = Currency.parse(matcher.group(2));
		  double rate = double.Parse(matcher.group(3));
		  return new FxRate(CurrencyPair.of(@base, counter), rate);
		}
		catch (Exception ex)
		{
		  throw new System.ArgumentException("Unable to parse rate: " + rateStr, ex);
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (pair.Base.Equals(pair.Counter) && rate != 1d)
		{
		  throw new System.ArgumentException("Conversion rate between identical currencies must be one");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the inverse rate.
	  /// <para>
	  /// The inverse rate has the same currencies but in reverse order.
	  /// The rate is the reciprocal of the original.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the inverse pair </returns>
	  public FxRate inverse()
	  {
		return new FxRate(pair.inverse(), 1d / rate);
	  }

	  /// <summary>
	  /// Gets the FX rate for the specified currency pair.
	  /// <para>
	  /// The rate returned is the rate from the base currency to the counter currency
	  /// as defined by this formula: {@code (1 * baseCurrency = fxRate * counterCurrency)}.
	  /// </para>
	  /// <para>
	  /// This will return the rate or inverse rate, or 1 if the two input currencies are the same.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseCurrency">  the base currency, to convert from </param>
	  /// <param name="counterCurrency">  the counter currency, to convert to </param>
	  /// <returns> the FX rate for the currency pair </returns>
	  /// <exception cref="IllegalArgumentException"> if no FX rate could be found </exception>
	  public double fxRate(Currency baseCurrency, Currency counterCurrency)
	  {
		if (baseCurrency.Equals(counterCurrency))
		{
		  return 1d;
		}
		if (baseCurrency.Equals(pair.Base) && counterCurrency.Equals(pair.Counter))
		{
		  return rate;
		}
		if (counterCurrency.Equals(pair.Base) && baseCurrency.Equals(pair.Counter))
		{
		  return 1d / rate;
		}
		throw new System.ArgumentException(Messages.format("No FX rate found for {}/{}", baseCurrency, counterCurrency));
	  }

	  /// <summary>
	  /// Derives an FX rate from two related FX rates.
	  /// <para>
	  /// Given two FX rates it is possible to derive another rate if they have a currency in common.
	  /// For example, given rates for EUR/GBP and EUR/CHF it is possible to derive rates for GBP/CHF.
	  /// The result will always have a currency pair in the conventional order.
	  /// </para>
	  /// <para>
	  /// The cross is only returned if the two pairs contains three currencies in total.
	  /// If the inputs are invalid, an exception is thrown.
	  /// <ul>
	  /// <li>AAA/BBB and BBB/CCC - valid, producing AAA/CCC
	  /// <li>AAA/BBB and CCC/BBB - valid, producing AAA/CCC
	  /// <li>AAA/BBB and BBB/AAA - invalid, exception thrown
	  /// <li>AAA/BBB and BBB/BBB - invalid, exception thrown
	  /// <li>AAA/BBB and CCC/DDD - invalid, exception thrown
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other rates </param>
	  /// <returns> a set of FX rates derived from these rates and the other rates </returns>
	  /// <exception cref="IllegalArgumentException"> if the cross rate cannot be calculated </exception>
	  public FxRate crossRate(FxRate other)
	  {
		return pair.cross(other.pair).map(cross => computeCross(this, other, cross)).orElseThrow(() => new System.ArgumentException(Messages.format("Unable to cross when no unique common currency: {} and {}", pair, other.pair)));
	  }

	  // computes the cross rate
	  private static FxRate computeCross(FxRate fx1, FxRate fx2, CurrencyPair crossPairAC)
	  {
		// aim is to convert AAA/BBB and BBB/CCC to AAA/CCC
		Currency currA = crossPairAC.Base;
		Currency currC = crossPairAC.Counter;
		// given the conventional cross rate pair, order the two rates to match
		bool crossBaseCurrencyInFx1 = fx1.pair.contains(currA);
		FxRate fxABorBA = crossBaseCurrencyInFx1 ? fx1 : fx2;
		FxRate fxBCorCB = crossBaseCurrencyInFx1 ? fx2 : fx1;
		// extract the rates, taking the inverse if the pair is in the inverse order
		double rateAB = fxABorBA.Pair.Base.Equals(currA) ? fxABorBA.rate : 1d / fxABorBA.rate;
		double rateBC = fxBCorCB.Pair.Counter.Equals(currC) ? fxBCorCB.rate : 1d / fxBCorCB.rate;
		return FxRate.of(crossPairAC, rateAB * rateBC);
	  }

	  /// <summary>
	  /// Returns an FX rate object representing the market convention rate between the two currencies.
	  /// <para>
	  /// If the currency pair is the market convention pair, this method returns {@code this}, otherwise
	  /// it returns an {@code FxRate} with the inverse currency pair and reciprocal rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an FX rate object representing the market convention rate between the two currencies </returns>
	  public FxRate toConventional()
	  {
		return pair.Conventional ? this : FxRate.of(pair.toConventional(), 1 / rate);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the formatted string version of the currency pair.
	  /// <para>
	  /// The format is '${baseCurrency}/${counterCurrency} ${rate}'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the formatted string </returns>
	  public override string ToString()
	  {
		return pair + " " + (DoubleMath.isMathematicalInteger(rate) ? Convert.ToString((long) rate) : Convert.ToString(rate));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxRate}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxRate.Meta meta()
	  {
		return FxRate.Meta.INSTANCE;
	  }

	  static FxRate()
	  {
		MetaBean.register(FxRate.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FxRate(CurrencyPair pair, double rate)
	  {
		JodaBeanUtils.notNull(pair, "pair");
		ArgChecker.notNegativeOrZero(rate, "rate");
		this.pair = pair;
		this.rate = rate;
		validate();
	  }

	  public override FxRate.Meta metaBean()
	  {
		return FxRate.Meta.INSTANCE;
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
	  /// Gets the rate applicable to the currency pair.
	  /// One unit of the base currency is exchanged for this amount of the counter currency. </summary>
	  /// <returns> the value of the property </returns>
	  private double Rate
	  {
		  get
		  {
			return rate;
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
		  FxRate other = (FxRate) obj;
		  return JodaBeanUtils.equal(pair, other.pair) && JodaBeanUtils.equal(rate, other.rate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(pair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rate);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxRate}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  pair_Renamed = DirectMetaProperty.ofImmutable(this, "pair", typeof(FxRate), typeof(CurrencyPair));
			  rate_Renamed = DirectMetaProperty.ofImmutable(this, "rate", typeof(FxRate), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "pair", "rate");
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
		/// The meta-property for the {@code rate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> rate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "pair", "rate");
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
			case 3493088: // rate
			  return rate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxRate> builder()
		public override BeanBuilder<FxRate> builder()
		{
		  return new FxRate.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxRate);
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
		/// The meta-property for the {@code rate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> rate()
		{
		  return rate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3433178: // pair
			  return ((FxRate) bean).Pair;
			case 3493088: // rate
			  return ((FxRate) bean).Rate;
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
	  /// The bean-builder for {@code FxRate}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxRate>
	  {

		internal CurrencyPair pair;
		internal double rate;

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
			case 3493088: // rate
			  return rate;
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
			case 3493088: // rate
			  this.rate = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FxRate build()
		{
		  return new FxRate(pair, rate);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("FxRate.Builder{");
		  buf.Append("pair").Append('=').Append(JodaBeanUtils.ToString(pair)).Append(',').Append(' ');
		  buf.Append("rate").Append('=').Append(JodaBeanUtils.ToString(rate));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}