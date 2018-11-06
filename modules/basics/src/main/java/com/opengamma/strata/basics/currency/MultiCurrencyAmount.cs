using System;
using System.Collections;
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ImmutableSortedMap = com.google.common.collect.ImmutableSortedMap;
	using ImmutableSortedSet = com.google.common.collect.ImmutableSortedSet;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using MapStream = com.opengamma.strata.collect.MapStream;

	/// <summary>
	/// A map of currency amounts keyed by currency.
	/// <para>
	/// This is a container holding multiple <seealso cref="CurrencyAmount"/> instances.
	/// The amounts do not necessarily have the same worth or value in each currency.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class MultiCurrencyAmount implements FxConvertible<CurrencyAmount>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class MultiCurrencyAmount : FxConvertible<CurrencyAmount>, ImmutableBean
	{
	  // the choice of a set as the internal storage is driven by serialization concerns
	  // the ideal storage form would be Map<Currency, CurrencyAmount> but this
	  // would duplicate the currency in the serialized form
	  // a set was chosen as a suitable middle ground

	  /// <summary>
	  /// An empty instance.
	  /// </summary>
	  private static readonly MultiCurrencyAmount EMPTY = new MultiCurrencyAmount(ImmutableSortedSet.of());

	  /// <summary>
	  /// The set of currency amounts.
	  /// Each currency will occur only once, as per a map keyed by currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableSortedSet<CurrencyAmount> amounts;
	  private readonly ImmutableSortedSet<CurrencyAmount> amounts;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an empty {@code MultiCurrencyAmount}.
	  /// </summary>
	  /// <returns> the empty instance </returns>
	  public static MultiCurrencyAmount empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an instance from a currency and amount.
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="amount">  the amount </param>
	  /// <returns> the amount </returns>
	  public static MultiCurrencyAmount of(Currency currency, double amount)
	  {
		ArgChecker.notNull(currency, "currency");
		return new MultiCurrencyAmount(ImmutableSortedSet.of(CurrencyAmount.of(currency, amount)));
	  }

	  /// <summary>
	  /// Obtains an instance from an array of {@code CurrencyAmount} objects.
	  /// <para>
	  /// It is an error for the input to contain the same currency twice.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amounts">  the amounts </param>
	  /// <returns> the amount </returns>
	  public static MultiCurrencyAmount of(params CurrencyAmount[] amounts)
	  {
		ArgChecker.notNull(amounts, "amounts");
		if (amounts.Length == 0)
		{
		  return EMPTY;
		}
		return of(Arrays.asList(amounts));
	  }

	  /// <summary>
	  /// Obtains an instance from a list of {@code CurrencyAmount} objects.
	  /// <para>
	  /// It is an error for the input to contain the same currency twice.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amounts">  the amounts </param>
	  /// <returns> the amount </returns>
	  public static MultiCurrencyAmount of(IEnumerable<CurrencyAmount> amounts)
	  {
		ArgChecker.noNulls(amounts, "amounts");
		IDictionary<Currency, CurrencyAmount> map = new Dictionary<Currency, CurrencyAmount>();
		foreach (CurrencyAmount amount in amounts)
		{
		  if (map.put(amount.Currency, amount) != null)
		  {
			throw new System.ArgumentException("Currency is duplicated: " + amount.Currency);
		  }
		}
		return new MultiCurrencyAmount(ImmutableSortedSet.copyOf(map.Values));
	  }

	  /// <summary>
	  /// Obtains an instance from a map of currency to amount.
	  /// </summary>
	  /// <param name="map">  the map of currency to amount </param>
	  /// <returns> the amount </returns>
	  public static MultiCurrencyAmount of(IDictionary<Currency, double> map)
	  {
		ArgChecker.noNulls(map, "map");
		return MapStream.of(map).map(CurrencyAmount.of).collect(MultiCurrencyAmount.collectorInternal());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the total of a list of {@code CurrencyAmount} objects.
	  /// <para>
	  /// If the input contains the same currency more than once, the amounts are added together.
	  /// For example, an input of (EUR 100, EUR 200, CAD 100) would result in (EUR 300, CAD 100).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amounts">  the amounts </param>
	  /// <returns> the amount </returns>
	  public static MultiCurrencyAmount total(IEnumerable<CurrencyAmount> amounts)
	  {
		ArgChecker.notNull(amounts, "amounts");
		return Guavate.stream(amounts).collect(toMultiCurrencyAmount());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a collector that can be used to create a multi-currency amount from a stream of amounts.
	  /// <para>
	  /// If the input contains the same currency more than once, the amounts are added together.
	  /// For example, an input of (EUR 100, EUR 200, CAD 100) would result in (EUR 300, CAD 100).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the collector </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static java.util.stream.Collector<CurrencyAmount, ?, MultiCurrencyAmount> toMultiCurrencyAmount()
	  public static Collector<CurrencyAmount, ?, MultiCurrencyAmount> toMultiCurrencyAmount()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Collector.of<CurrencyAmount, IDictionary<Currency, CurrencyAmount>, MultiCurrencyAmount>(Hashtable::new, (map, ca) => map.merge(ArgChecker.notNull(ca, "amount").Currency, ca, CurrencyAmount::plus), (map1, map2) =>
		{
		map2.values().forEach((ca2) => map1.merge(ca2.Currency, ca2, CurrencyAmount::plus));
		return map1;
		}, map => new MultiCurrencyAmount(ImmutableSortedSet.copyOf(map.values())), UNORDERED);
	  }

	  /// <summary>
	  /// Returns a collector that can be used to create a multi-currency amount
	  /// from a stream of amounts where each amount has a different currency.
	  /// <para>
	  /// Each amount in the stream must have a different currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the collector </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static java.util.stream.Collector<CurrencyAmount, ?, MultiCurrencyAmount> collectorInternal()
	  private static Collector<CurrencyAmount, ?, MultiCurrencyAmount> collectorInternal()
	  {
		// this method must not be exposed publicly as misuse creates an instance with invalid state
		// it exists because when used internally it offers better performance than collector()
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collectors.collectingAndThen(Guavate.toImmutableSortedSet(), MultiCurrencyAmount::new);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance where the input is already validated.
	  /// </summary>
	  /// <param name="amounts">  the set of amounts </param>
	  private MultiCurrencyAmount(ImmutableSortedSet<CurrencyAmount> amounts)
	  {
		this.amounts = amounts;
	  }

	  /// <summary>
	  /// Validate against duplicate currencies.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		long currencyCount = amounts.Select(ArgChecker.notNullItem).Select(CurrencyAmount::getCurrency).Distinct().Count();
		if (currencyCount < amounts.size())
		{
		  throw new System.ArgumentException("Duplicate currency not allowed: " + amounts);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the set of stored currencies.
	  /// </summary>
	  /// <returns> the set of currencies in this amount </returns>
	  public ImmutableSet<Currency> Currencies
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return amounts.Select(CurrencyAmount::getCurrency).collect(Guavate.toImmutableSet());
		  }
	  }

	  /// <summary>
	  /// Gets the number of stored amounts.
	  /// </summary>
	  /// <returns> the number of amounts </returns>
	  public int size()
	  {
		return amounts.size();
	  }

	  /// <summary>
	  /// Checks if this multi-amount contains an amount for the specified currency.
	  /// </summary>
	  /// <param name="currency">  the currency to find </param>
	  /// <returns> true if this amount contains a value for the currency </returns>
	  public bool contains(Currency currency)
	  {
		ArgChecker.notNull(currency, "currency");
		return amounts.Any(ca => ca.Currency.Equals(currency));
	  }

	  /// <summary>
	  /// Gets the {@code CurrencyAmount} for the specified currency, throwing an exception if not found.
	  /// </summary>
	  /// <param name="currency">  the currency to find an amount for </param>
	  /// <returns> the amount </returns>
	  /// <exception cref="IllegalArgumentException"> if the currency is not found </exception>
	  public CurrencyAmount getAmount(Currency currency)
	  {
		ArgChecker.notNull(currency, "currency");
		return amounts.Where(ca => ca.Currency.Equals(currency)).First().orElseThrow(() => new System.ArgumentException("Unknown currency " + currency));
	  }

	  /// <summary>
	  /// Gets the {@code CurrencyAmount} for the specified currency, returning zero if not found.
	  /// </summary>
	  /// <param name="currency">  the currency to find an amount for </param>
	  /// <returns> the amount </returns>
	  public CurrencyAmount getAmountOrZero(Currency currency)
	  {
		ArgChecker.notNull(currency, "currency");
		return amounts.Where(ca => ca.Currency.Equals(currency)).First().orElseGet(() => CurrencyAmount.zero(currency));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this {@code MultiCurrencyAmount} with the specified amount added.
	  /// <para>
	  /// This adds the specified amount to this monetary amount, returning a new object.
	  /// If the currency is already present, the amount is added to the existing amount.
	  /// If the currency is not yet present, the currency-amount is added to the map.
	  /// The addition uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency to add to </param>
	  /// <param name="amountToAdd">  the amount to add </param>
	  /// <returns> an amount based on this with the specified amount added </returns>
	  public MultiCurrencyAmount plus(Currency currency, double amountToAdd)
	  {
		return plus(CurrencyAmount.of(currency, amountToAdd));
	  }

	  /// <summary>
	  /// Returns a copy of this {@code MultiCurrencyAmount} with the specified amount added.
	  /// <para>
	  /// This adds the specified amount to this monetary amount, returning a new object.
	  /// If the currency is already present, the amount is added to the existing amount.
	  /// If the currency is not yet present, the currency-amount is added to the map.
	  /// The addition uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountToAdd">  the amount to add </param>
	  /// <returns> an amount based on this with the specified amount added </returns>
	  public MultiCurrencyAmount plus(CurrencyAmount amountToAdd)
	  {
		ArgChecker.notNull(amountToAdd, "amountToAdd");
		return Stream.concat(amounts.stream(), Stream.of(amountToAdd)).collect(toMultiCurrencyAmount());
	  }

	  /// <summary>
	  /// Returns a copy of this {@code MultiCurrencyAmount} with the specified amount added.
	  /// <para>
	  /// This adds the specified amount to this monetary amount, returning a new object.
	  /// If the currency is already present, the amount is added to the existing amount.
	  /// If the currency is not yet present, the currency-amount is added to the map.
	  /// The addition uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountToAdd">  the amount to add </param>
	  /// <returns> an amount based on this with the specified amount added </returns>
	  public MultiCurrencyAmount plus(MultiCurrencyAmount amountToAdd)
	  {
		ArgChecker.notNull(amountToAdd, "amountToAdd");
		return Stream.concat(amounts.stream(), amountToAdd.stream()).collect(toMultiCurrencyAmount());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this {@code MultiCurrencyAmount} with the specified amount subtracted.
	  /// <para>
	  /// This subtracts the specified amount from this monetary amount, returning a new object.
	  /// If the currency is already present, the amount is subtracted from the existing amount.
	  /// If the currency is not yet present, the negated amount is included.
	  /// The subtraction uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency to subtract from </param>
	  /// <param name="amountToAdd">  the amount to subtract </param>
	  /// <returns> an amount based on this with the specified amount subtracted </returns>
	  public MultiCurrencyAmount minus(Currency currency, double amountToAdd)
	  {
		return plus(CurrencyAmount.of(currency, -amountToAdd));
	  }

	  /// <summary>
	  /// Returns a copy of this {@code MultiCurrencyAmount} with the specified amount subtracted.
	  /// <para>
	  /// This subtracts the specified amount from this monetary amount, returning a new object.
	  /// If the currency is already present, the amount is subtracted from the existing amount.
	  /// If the currency is not yet present, the negated amount is included.
	  /// The subtraction uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountToSubtract">  the amount to subtract </param>
	  /// <returns> an amount based on this with the specified amount subtracted </returns>
	  public MultiCurrencyAmount minus(CurrencyAmount amountToSubtract)
	  {
		ArgChecker.notNull(amountToSubtract, "amountToSubtract");
		return plus(amountToSubtract.negated());
	  }

	  /// <summary>
	  /// Returns a copy of this {@code MultiCurrencyAmount} with the specified amount subtracted.
	  /// <para>
	  /// This subtracts the specified amount from this monetary amount, returning a new object.
	  /// If the currency is already present, the amount is subtracted from the existing amount.
	  /// If the currency is not yet present, the negated amount is included.
	  /// The subtraction uses standard {@code double} arithmetic.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amountToSubtract">  the amount to subtract </param>
	  /// <returns> an amount based on this with the specified amount subtracted </returns>
	  public MultiCurrencyAmount minus(MultiCurrencyAmount amountToSubtract)
	  {
		ArgChecker.notNull(amountToSubtract, "amountToSubtract");
		return plus(amountToSubtract.negated());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this {@code MultiCurrencyAmount} with all the amounts multiplied by the factor.
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="factor">  the multiplicative factor </param>
	  /// <returns> an amount based on this with all the amounts multiplied by the factor </returns>
	  public MultiCurrencyAmount multipliedBy(double factor)
	  {
		return mapAmounts(a => a * factor);
	  }

	  /// <summary>
	  /// Returns a copy of this {@code CurrencyAmount} with the amount negated.
	  /// <para>
	  /// This takes this amount and negates it. If any amount is 0.0 or -0.0 the negated amount is 0.0.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an amount based on this with the amount negated </returns>
	  public MultiCurrencyAmount negated()
	  {
		// Zero is treated as a special case to avoid creating -0.0 which produces surprising equality behaviour
		return mapAmounts(a => a == 0d ? 0d : -a);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a stream over the currency amounts.
	  /// <para>
	  /// This provides access to the entire set of amounts.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a stream over the individual amounts </returns>
	  public Stream<CurrencyAmount> stream()
	  {
		return amounts.stream();
	  }

	  /// <summary>
	  /// Applies an operation to the amounts.
	  /// <para>
	  /// This is generally used to apply a mathematical operation to the amounts.
	  /// For example, the operator could multiply the amounts by a constant, or take the inverse.
	  /// <pre>
	  ///   multiplied = base.mapAmount(value -> value * 3);
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  the operator to be applied to the amounts </param>
	  /// <returns> a copy of this amount with the mapping applied to the original amounts </returns>
	  public MultiCurrencyAmount mapAmounts(System.Func<double, double> mapper)
	  {
		ArgChecker.notNull(mapper, "mapper");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return amounts.Select(ca => ca.mapAmount(mapper)).collect(MultiCurrencyAmount.collectorInternal());
	  }

	  /// <summary>
	  /// Applies an operation to the currency amounts.
	  /// <para>
	  /// The operator is called once for each currency in this amount.
	  /// The operator may return an amount with a different currency.
	  /// The result will be the total of the altered amounts.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="operator">  the operator to be applied to the amounts </param>
	  /// <returns> a copy of this amount with the mapping applied to the original amounts </returns>
	  public MultiCurrencyAmount mapCurrencyAmounts(System.Func<CurrencyAmount, CurrencyAmount> @operator)
	  {
		ArgChecker.notNull(@operator, "operator");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return amounts.Select(ca => @operator(ca)).collect(MultiCurrencyAmount.toMultiCurrencyAmount());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this amount to an equivalent amount the specified currency.
	  /// <para>
	  /// The result will be expressed in terms of the given currency.
	  /// If conversion is needed, the provider will be used to supply the FX rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the converted instance, which should be expressed in the specified currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  public CurrencyAmount convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		if (amounts.size() == 1)
		{
		  return amounts.first().convertedTo(resultCurrency, rateProvider);
		}
		double total = 0d;
		foreach (CurrencyAmount amount in amounts)
		{
		  total += rateProvider.convert(amount.Amount, amount.Currency, resultCurrency);
		}
		return CurrencyAmount.of(resultCurrency, total);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this {@code MultiCurrencyAmount} to a map keyed by currency.
	  /// </summary>
	  /// <returns> the amounts in a map keyed by currency </returns>
	  public ImmutableSortedMap<Currency, double> toMap()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return amounts.collect(Guavate.toImmutableSortedMap(CurrencyAmount::getCurrency, CurrencyAmount::getAmount));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the amount as a string.
	  /// <para>
	  /// The format includes each currency-amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency amount </returns>
	  public override string ToString()
	  {
		return amounts.ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code MultiCurrencyAmount}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static MultiCurrencyAmount.Meta meta()
	  {
		return MultiCurrencyAmount.Meta.INSTANCE;
	  }

	  static MultiCurrencyAmount()
	  {
		MetaBean.register(MultiCurrencyAmount.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private MultiCurrencyAmount(SortedSet<CurrencyAmount> amounts)
	  {
		JodaBeanUtils.notNull(amounts, "amounts");
		this.amounts = ImmutableSortedSet.copyOfSorted(amounts);
		validate();
	  }

	  public override MultiCurrencyAmount.Meta metaBean()
	  {
		return MultiCurrencyAmount.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the set of currency amounts.
	  /// Each currency will occur only once, as per a map keyed by currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableSortedSet<CurrencyAmount> Amounts
	  {
		  get
		  {
			return amounts;
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
		  MultiCurrencyAmount other = (MultiCurrencyAmount) obj;
		  return JodaBeanUtils.equal(amounts, other.amounts);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(amounts);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code MultiCurrencyAmount}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  amounts_Renamed = DirectMetaProperty.ofImmutable(this, "amounts", typeof(MultiCurrencyAmount), (Type) typeof(ImmutableSortedSet));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "amounts");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code amounts} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSortedSet<CurrencyAmount>> amounts = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "amounts", MultiCurrencyAmount.class, (Class) com.google.common.collect.ImmutableSortedSet.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableSortedSet<CurrencyAmount>> amounts_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "amounts");
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
			case -879772901: // amounts
			  return amounts_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends MultiCurrencyAmount> builder()
		public override BeanBuilder<MultiCurrencyAmount> builder()
		{
		  return new MultiCurrencyAmount.Builder();
		}

		public override Type beanType()
		{
		  return typeof(MultiCurrencyAmount);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code amounts} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableSortedSet<CurrencyAmount>> amounts()
		{
		  return amounts_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -879772901: // amounts
			  return ((MultiCurrencyAmount) bean).Amounts;
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
	  /// The bean-builder for {@code MultiCurrencyAmount}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<MultiCurrencyAmount>
	  {

		internal SortedSet<CurrencyAmount> amounts = ImmutableSortedSet.of();

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
			case -879772901: // amounts
			  return amounts;
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
			case -879772901: // amounts
			  this.amounts = (SortedSet<CurrencyAmount>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override MultiCurrencyAmount build()
		{
		  return new MultiCurrencyAmount(amounts);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("MultiCurrencyAmount.Builder{");
		  buf.Append("amounts").Append('=').Append(JodaBeanUtils.ToString(amounts));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}