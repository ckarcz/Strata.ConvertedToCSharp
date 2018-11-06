using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Joiner = com.google.common.@base.Joiner;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// A matrix of foreign exchange rates.
	/// <para>
	/// This provides a matrix of foreign exchange rates, such that the rate can be queried for any available pair.
	/// For example, if the matrix contains the currencies 'USD', 'EUR' and 'GBP', then six rates can be queried,
	/// 'EUR/USD', 'GBP/USD', 'EUR/GBP' and the three inverse rates.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private", constructorScope = "package") public final class FxMatrix implements FxRateProvider, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxMatrix : FxRateProvider, ImmutableBean
	{

	  /// <summary>
	  /// An empty FX matrix containing neither currencies nor rates.
	  /// </summary>
	  private static readonly FxMatrix EMPTY = builder().build();

	  /// <summary>
	  /// The map between the currencies and their position within the
	  /// {@code rates} array. Generally the position reflects the order
	  /// in which the currencies were added, so the first currency added
	  /// will be assigned 0, the second 1 etc.
	  /// <para>
	  /// An ImmutableMap is used so that the currencies are correctly
	  /// ordered when the <seealso cref="#toString()"/> method is called.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", get = "") private final com.google.common.collect.ImmutableMap<Currency, int> currencies;
	  private readonly ImmutableMap<Currency, int> currencies;
	  /// <summary>
	  /// The matrix with all the exchange rates. Each row represents the
	  /// rates required to convert a unit of particular currency to all
	  /// other currencies in the matrix.
	  /// <para>
	  /// If currencies c1 and c2 are assigned indexes i and j respectively
	  /// in the {@code currencies} map, then the entry [i][j] is such that
	  /// 1 unit of currency c1 is worth {@code rates[i][j]} units of
	  /// currency c2.
	  /// </para>
	  /// <para>
	  /// If {@code currencies.get(EUR)} = 0 and {@code currencies.get(USD)} = 1,
	  /// then the element {@code rates[0][1]} is likely to be around
	  /// 1.40 and {@code rates[1][0]} around 0.7142. The rate {@code rates[1][0]}
	  /// will be computed from fxRate[0][1] when the object is constructed
	  /// by the builder. All the element of the matrix are meaningful and coherent.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleMatrix rates;
	  private readonly DoubleMatrix rates;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an empty FX matrix.
	  /// <para>
	  /// The result contains no currencies or rates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an empty matrix </returns>
	  public static FxMatrix empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an instance containing a single FX rate.
	  /// <para>
	  /// This is most likely to be used in testing.
	  /// </para>
	  /// <para>
	  /// An invocation of the method with {@code FxMatrix.of(CurrencyPair.of(GBP, USD), 1.6)}
	  /// indicates that 1 pound sterling is worth 1.6 US dollars.
	  /// The matrix can also be queried for the reverse rate, from USD to GBP.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair to be added </param>
	  /// <param name="rate">  the FX rate between the base currency of the pair and the
	  ///   counter currency. The rate indicates the value of one unit of the base
	  ///   currency in terms of the counter currency. </param>
	  /// <returns> a matrix containing the single FX rate </returns>
	  public static FxMatrix of(CurrencyPair currencyPair, double rate)
	  {
		return FxMatrix.of(currencyPair.Base, currencyPair.Counter, rate);
	  }

	  /// <summary>
	  /// Obtains an instance containing a single FX rate.
	  /// <para>
	  /// This is most likely to be used in testing.
	  /// </para>
	  /// <para>
	  /// An invocation of the method with {@code FxMatrix.of(GBP, USD, 1.6)}
	  /// indicates that 1 pound sterling is worth 1.6 US dollars.
	  /// The matrix can also be queried for the reverse rate, from USD to GBP.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ccy1">  the first currency of the pair </param>
	  /// <param name="ccy2">  the second currency of the pair </param>
	  /// <param name="rate">  the FX rate between the first currency and the second currency.
	  ///   The rate indicates the value of one unit of the first currency in terms
	  ///   of the second currency. </param>
	  /// <returns> a matrix containing the single FX rate </returns>
	  public static FxMatrix of(Currency ccy1, Currency ccy2, double rate)
	  {
		return (new FxMatrixBuilder()).addRate(ccy1, ccy2, rate).build();
	  }

	  /// <summary>
	  /// Creates a builder that can be used to build instances of {@code FxMatrix}.
	  /// </summary>
	  /// <returns> a new builder </returns>
	  public static FxMatrixBuilder builder()
	  {
		return new FxMatrixBuilder();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a {@code Collector} that allows a {@code Map.Entry} of currency pair to rate
	  /// to be streamed and collected into a new {@code FxMatrix}.
	  /// </summary>
	  /// <returns> a collector for creating an {@code FxMatrix} from a stream </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static java.util.stream.Collector<? super java.util.Map.Entry<CurrencyPair, double>, FxMatrixBuilder, FxMatrix> entriesToFxMatrix()
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Collector<object, FxMatrixBuilder, FxMatrix> entriesToFxMatrix()
	  {
		return collector((builder, entry) => builder.addRate(entry.Key, entry.Value));
	  }

	  /// <summary>
	  /// Creates a {@code Collector} that allows a collection of pairs each containing
	  /// a currency pair and a rate to be streamed and collected into a new {@code FxMatrix}.
	  /// </summary>
	  /// <returns> a collector for creating an {@code FxMatrix} from a stream </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static java.util.stream.Collector<? super com.opengamma.strata.collect.tuple.Pair<CurrencyPair, double>, FxMatrixBuilder, FxMatrix> pairsToFxMatrix()
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Collector<object, FxMatrixBuilder, FxMatrix> pairsToFxMatrix()
	  {
		return collector((builder, pair) => builder.addRate(pair.First, pair.Second));
	  }

	  private static Collector<T, FxMatrixBuilder, FxMatrix> collector<T>(System.Action<FxMatrixBuilder, T> accumulator)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Collector.of(FxMatrix.builder, accumulator, FxMatrixBuilder::merge, FxMatrixBuilder::build);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the set of currencies held within this matrix.
	  /// </summary>
	  /// <returns> the currencies in this matrix </returns>
	  public ImmutableSet<Currency> Currencies
	  {
		  get
		  {
			return currencies.Keys;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the FX rate for the specified currency pair.
	  /// <para>
	  /// The rate returned is the rate from the base currency to the counter currency
	  /// as defined by this formula: {@code (1 * baseCurrency = fxRate * counterCurrency)}.
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
		int? index1 = currencies.get(baseCurrency);
		int? index2 = currencies.get(counterCurrency);
		if (index1 != null && index2 != null)
		{
		  return rates.get(index1.Value, index2.Value);
		}
		else
		{
		  throw new System.ArgumentException(Messages.format("No FX rate found for {}/{}, matrix only contains rates for {}", baseCurrency, counterCurrency, currencies.Keys));
		}
	  }

	  /// <summary>
	  /// Converts a {@code CurrencyAmount} into an amount in the specified
	  /// currency using the rates in this matrix.
	  /// </summary>
	  /// <param name="amount">  the {@code CurrencyAmount} to be converted </param>
	  /// <param name="targetCurrency">  the currency to convert the {@code CurrencyAmount} to </param>
	  /// <returns> the amount converted to the requested currency </returns>
	  public CurrencyAmount convert(CurrencyAmount amount, Currency targetCurrency)
	  {
		ArgChecker.notNull(amount, "amount");
		ArgChecker.notNull(targetCurrency, "targetCurrency");
		// Only do conversion if we need to
		Currency originalCurrency = amount.Currency;
		if (originalCurrency.Equals(targetCurrency))
		{
		  return amount;
		}
		return CurrencyAmount.of(targetCurrency, convert(amount.Amount, originalCurrency, targetCurrency));
	  }

	  /// <summary>
	  /// Converts a {@code MultipleCurrencyAmount} into an amount in the
	  /// specified currency using the rates in this matrix.
	  /// </summary>
	  /// <param name="amount">  the {@code MultipleCurrencyAmount} to be converted </param>
	  /// <param name="targetCurrency">  the currency to convert all entries to </param>
	  /// <returns> the total amount in the requested currency </returns>
	  public CurrencyAmount convert(MultiCurrencyAmount amount, Currency targetCurrency)
	  {
		ArgChecker.notNull(amount, "amount");
		ArgChecker.notNull(targetCurrency, "targetCurrency");

		// We could do this using the currency amounts but to
		// avoid creating extra objects we'll use doubles
		double total = amount.Amounts.Select(ca => convert(ca.Amount, ca.Currency, targetCurrency)).Sum();
		return CurrencyAmount.of(targetCurrency, total);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Merges the entries from the other matrix into this one.
	  /// <para>
	  /// The other matrix should have at least one currency in common with this one.
	  /// The additional currencies from the other matrix are added one by one and
	  /// the exchange rate data created is coherent with some data in this matrix.
	  /// </para>
	  /// <para>
	  /// Note that if the other matrix has more than one currency in common with
	  /// this one, and the rates for pairs of those currencies are different to
	  /// the equivalents in this matrix, then the rates between the additional
	  /// currencies is this matrix will differ from those in the original.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the matrix to be merged into this one </param>
	  /// <returns> a new matrix containing the rates from this matrix
	  ///   plus any rates for additional currencies from the other matrix </returns>
	  public FxMatrix merge(FxMatrix other)
	  {
		return toBuilder().merge(other.toBuilder()).build();
	  }

	  /// <summary>
	  /// Creates a new builder using the data from this matrix to
	  /// create a set of initial entries.
	  /// </summary>
	  /// <returns> a new builder containing the data from this matrix </returns>
	  public FxMatrixBuilder toBuilder()
	  {
		return new FxMatrixBuilder(currencies, rates.toArray());
	  }

	  public override string ToString()
	  {
		return "FxMatrix[" + Joiner.on(", ").join(Currencies) + " : " + Stream.of(rates.toArrayUnsafe()).map(Arrays.toString).collect(Collectors.joining(",")) + "]";
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxMatrix}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxMatrix.Meta meta()
	  {
		return FxMatrix.Meta.INSTANCE;
	  }

	  static FxMatrix()
	  {
		MetaBean.register(FxMatrix.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="currencies">  the value of the property, not null </param>
	  /// <param name="rates">  the value of the property, not null </param>
	  internal FxMatrix(IDictionary<Currency, int> currencies, DoubleMatrix rates)
	  {
		JodaBeanUtils.notNull(currencies, "currencies");
		JodaBeanUtils.notNull(rates, "rates");
		this.currencies = ImmutableMap.copyOf(currencies);
		this.rates = rates;
	  }

	  public override FxMatrix.Meta metaBean()
	  {
		return FxMatrix.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the matrix with all the exchange rates. Each row represents the
	  /// rates required to convert a unit of particular currency to all
	  /// other currencies in the matrix.
	  /// <para>
	  /// If currencies c1 and c2 are assigned indexes i and j respectively
	  /// in the {@code currencies} map, then the entry [i][j] is such that
	  /// 1 unit of currency c1 is worth {@code rates[i][j]} units of
	  /// currency c2.
	  /// </para>
	  /// <para>
	  /// If {@code currencies.get(EUR)} = 0 and {@code currencies.get(USD)} = 1,
	  /// then the element {@code rates[0][1]} is likely to be around
	  /// 1.40 and {@code rates[1][0]} around 0.7142. The rate {@code rates[1][0]}
	  /// will be computed from fxRate[0][1] when the object is constructed
	  /// by the builder. All the element of the matrix are meaningful and coherent.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleMatrix Rates
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
		  FxMatrix other = (FxMatrix) obj;
		  return JodaBeanUtils.equal(currencies, other.currencies) && JodaBeanUtils.equal(rates, other.rates);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencies);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rates);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxMatrix}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currencies_Renamed = DirectMetaProperty.ofImmutable(this, "currencies", typeof(FxMatrix), (Type) typeof(ImmutableMap));
			  rates_Renamed = DirectMetaProperty.ofImmutable(this, "rates", typeof(FxMatrix), typeof(DoubleMatrix));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currencies", "rates");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code currencies} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<Currency, int>> currencies = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "currencies", FxMatrix.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Currency, int>> currencies_Renamed;
		/// <summary>
		/// The meta-property for the {@code rates} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleMatrix> rates_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currencies", "rates");
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
			case -1089470353: // currencies
			  return currencies_Renamed;
			case 108285843: // rates
			  return rates_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxMatrix> builder()
		public override BeanBuilder<FxMatrix> builder()
		{
		  return new FxMatrix.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxMatrix);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code currencies} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Currency, int>> currencies()
		{
		  return currencies_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rates} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleMatrix> rates()
		{
		  return rates_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1089470353: // currencies
			  return ((FxMatrix) bean).currencies;
			case 108285843: // rates
			  return ((FxMatrix) bean).Rates;
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
	  /// The bean-builder for {@code FxMatrix}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxMatrix>
	  {

		internal IDictionary<Currency, int> currencies = ImmutableMap.of();
		internal DoubleMatrix rates;

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
			case -1089470353: // currencies
			  return currencies;
			case 108285843: // rates
			  return rates;
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
			case -1089470353: // currencies
			  this.currencies = (IDictionary<Currency, int>) newValue;
			  break;
			case 108285843: // rates
			  this.rates = (DoubleMatrix) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FxMatrix build()
		{
		  return new FxMatrix(currencies, rates);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("FxMatrix.Builder{");
		  buf.Append("currencies").Append('=').Append(JodaBeanUtils.ToString(currencies)).Append(',').Append(' ');
		  buf.Append("rates").Append('=').Append(JodaBeanUtils.ToString(rates));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}