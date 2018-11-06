using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSortedMap = com.google.common.collect.ImmutableSortedMap;
	using Sets = com.google.common.collect.Sets;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// An array of multi-currency amounts.
	/// <para>
	/// This represents an array of <seealso cref="MultiCurrencyAmount"/>.
	/// Internally, it stores the data using a map of <seealso cref="Currency"/> to <seealso cref="DoubleArray"/>,
	/// which uses less memory than a {@code List<MultiCurrencyAmount>}.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class MultiCurrencyAmountArray implements FxConvertible<CurrencyAmountArray>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class MultiCurrencyAmountArray : FxConvertible<CurrencyAmountArray>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNegative") private final int size;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private readonly int size_Renamed;

	  /// <summary>
	  /// The currency values, keyed by currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableSortedMap<Currency, com.opengamma.strata.collect.array.DoubleArray> values;
	  private readonly ImmutableSortedMap<Currency, DoubleArray> values;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified multi-currency amounts.
	  /// </summary>
	  /// <param name="amounts">  the amounts </param>
	  /// <returns> an instance with the specified amounts </returns>
	  public static MultiCurrencyAmountArray of(params MultiCurrencyAmount[] amounts)
	  {
		return of(Arrays.asList(amounts));
	  }

	  /// <summary>
	  /// Obtains an instance from the specified multi-currency amounts.
	  /// </summary>
	  /// <param name="amounts">  the amounts </param>
	  /// <returns> an instance with the specified amounts </returns>
	  public static MultiCurrencyAmountArray of(IList<MultiCurrencyAmount> amounts)
	  {
		int size = amounts.Count;
		Dictionary<Currency, double[]> valueMap = new Dictionary<Currency, double[]>();
		for (int i = 0; i < size; i++)
		{
		  MultiCurrencyAmount multiCurrencyAmount = amounts[i];
		  foreach (CurrencyAmount currencyAmount in multiCurrencyAmount.Amounts)
		  {
			double[] currencyValues = valueMap.computeIfAbsent(currencyAmount.Currency, ccy => new double[size]);
			currencyValues[i] = currencyAmount.Amount;
		  }
		}
		IDictionary<Currency, DoubleArray> doubleArrayMap = MapStream.of(valueMap).mapValues(v => DoubleArray.ofUnsafe(v)).toMap();
		return new MultiCurrencyAmountArray(size, doubleArrayMap);
	  }

	  /// <summary>
	  /// Obtains an instance using a function to create the entries.
	  /// <para>
	  /// The function is passed the index and returns the {@code MultiCurrencyAmount} for that index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="size">  the number of elements, at least size one </param>
	  /// <param name="valueFunction">  the function used to obtain each value </param>
	  /// <returns> an instance initialized using the function </returns>
	  /// <exception cref="IllegalArgumentException"> is size is zero or less </exception>
	  public static MultiCurrencyAmountArray of(int size, System.Func<int, MultiCurrencyAmount> valueFunction)
	  {
		IDictionary<Currency, double[]> map = new Dictionary<Currency, double[]>();
		for (int i = 0; i < size; i++)
		{
		  MultiCurrencyAmount mca = valueFunction(i);
		  foreach (CurrencyAmount ca in mca.Amounts)
		  {
			double[] array = map.computeIfAbsent(ca.Currency, c => new double[size]);
			array[i] = ca.Amount;
		  }
		}
		return new MultiCurrencyAmountArray(size, MapStream.of(map).mapValues(array => DoubleArray.ofUnsafe(array)).toMap());
	  }

	  /// <summary>
	  /// Obtains an instance from a map of amounts.
	  /// <para>
	  /// Each currency is associated with an array of amounts.
	  /// All the arrays must have the same number of elements.
	  /// </para>
	  /// <para>
	  /// If the map is empty the returned array will have a size of zero. To create an empty array
	  /// with a non-zero size use one of the other {@code of} methods.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="values">  map of currencies to values </param>
	  /// <returns> an instance containing the values from the map </returns>
	  public static MultiCurrencyAmountArray of(IDictionary<Currency, DoubleArray> values)
	  {
		values.Values.Aggregate((a1, a2) => checkSize(a1, a2));
		// All of the values must have the same size so use the size of the first
		int size = values.Count == 0 ? 0 : values.Values.GetEnumerator().next().size();
		return new MultiCurrencyAmountArray(size, values);
	  }

	  /// <summary>
	  /// Checks the size of the arrays are the same and throws an exception if not.
	  /// </summary>
	  /// <param name="array1">  an array </param>
	  /// <param name="array2">  an array </param>
	  /// <returns> array1 </returns>
	  /// <exception cref="IllegalArgumentException"> if the array sizes are not equal </exception>
	  private static DoubleArray checkSize(DoubleArray array1, DoubleArray array2)
	  {
		if (array1.size() != array2.size())
		{
		  throw new System.ArgumentException(Messages.format("Arrays must have the same size but found sizes {} and {}", array1.size(), array2.size()));
		}
		return array1;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private MultiCurrencyAmountArray(int size, java.util.Map<Currency, com.opengamma.strata.collect.array.DoubleArray> values)
	  private MultiCurrencyAmountArray(int size, IDictionary<Currency, DoubleArray> values)
	  {
		this.values = ImmutableSortedMap.copyOf(values);
		this.size_Renamed = size;
	  }

	  // validate when deserializing
	  private object readResolve()
	  {
		values.values().Aggregate((a1, a2) => checkSize(a1, a2));
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the set of currencies for which this object contains values.
	  /// </summary>
	  /// <returns> the set of currencies for which this object contains values </returns>
	  public ISet<Currency> Currencies
	  {
		  get
		  {
			return values.Keys;
		  }
	  }

	  /// <summary>
	  /// Gets the values for the specified currency, throws an exception if there are no values for the currency.
	  /// </summary>
	  /// <param name="currency">  the currency for which values are required </param>
	  /// <returns> the values for the specified currency, throws an exception if there are none </returns>
	  /// <exception cref="IllegalArgumentException"> if there are no values for the currency </exception>
	  public DoubleArray getValues(Currency currency)
	  {
		DoubleArray currencyValues = values.get(currency);
		if (currencyValues == null)
		{
		  throw new System.ArgumentException("No values available for " + currency);
		}
		return currencyValues;
	  }

	  /// <summary>
	  /// Gets the size of the array.
	  /// </summary>
	  /// <returns> the array size </returns>
	  public int size()
	  {
		return size_Renamed;
	  }

	  /// <summary>
	  /// Gets the amount at the specified index.
	  /// </summary>
	  /// <param name="index">  the zero-based index to retrieve </param>
	  /// <returns> the amount at the specified index </returns>
	  public MultiCurrencyAmount get(int index)
	  {
		IList<CurrencyAmount> currencyAmounts = values.Keys.Select(ccy => CurrencyAmount.of(ccy, values.get(ccy).get(index))).ToList();
		return MultiCurrencyAmount.of(currencyAmounts);
	  }

	  /// <summary>
	  /// Returns a stream of the amounts.
	  /// </summary>
	  /// <returns> a stream of the amounts </returns>
	  public Stream<MultiCurrencyAmount> stream()
	  {
		return IntStream.range(0, size_Renamed).mapToObj(this.get);
	  }

	  public CurrencyAmountArray convertedTo(Currency resultCurrency, FxRateProvider fxRateProvider)
	  {
		double[] singleCurrencyValues = new double[size_Renamed];
		foreach (KeyValuePair<Currency, DoubleArray> entry in values.entrySet())
		{
		  Currency currency = entry.Key;
		  DoubleArray currencyValues = entry.Value;
		  for (int i = 0; i < size_Renamed; i++)
		  {
			singleCurrencyValues[i] += currencyValues.get(i) * fxRateProvider.fxRate(currency, resultCurrency);
		  }
		}
		return CurrencyAmountArray.of(resultCurrency, DoubleArray.ofUnsafe(singleCurrencyValues));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a new array containing the values from this array added to the values in the other array.
	  /// <para>
	  /// The amounts are added to the matching element in this array.
	  /// The arrays must have the same size.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  another array of multiple currency values. </param>
	  /// <returns> a new array containing the values from this array added to the values in the other array </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public MultiCurrencyAmountArray plus(MultiCurrencyAmountArray other)
	  {
		if (other.size() != size_Renamed)
		{
		  throw new System.ArgumentException(Messages.format("Sizes must be equal, this size is {}, other size is {}", size_Renamed, other.size()));
		}
		IDictionary<Currency, DoubleArray> addedValues = Stream.concat(values.entrySet().stream(), other.values.entrySet().stream()).collect(toMap(e => e.Key, e => e.Value, (arr1, arr2) => arr1.plus(arr2)));
		return MultiCurrencyAmountArray.of(addedValues);
	  }

	  /// <summary>
	  /// Returns a new array containing the values from this array with the values from the amount added.
	  /// <para>
	  /// The amount is added to each element in this array.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to add </param>
	  /// <returns> a new array containing the values from this array added to the values in the other array </returns>
	  public MultiCurrencyAmountArray plus(MultiCurrencyAmount amount)
	  {
		ImmutableMap.Builder<Currency, DoubleArray> builder = ImmutableMap.builder();
		foreach (Currency currency in Sets.union(values.Keys, amount.Currencies))
		{
		  DoubleArray array = values.get(currency);
		  if (array == null)
		  {
			builder.put(currency, DoubleArray.filled(size_Renamed, amount.getAmount(currency).Amount));
		  }
		  else if (!amount.contains(currency))
		  {
			builder.put(currency, array);
		  }
		  else
		  {
			builder.put(currency, array.plus(amount.getAmount(currency).Amount));
		  }
		}
		return MultiCurrencyAmountArray.of(builder.build());
	  }

	  /// <summary>
	  /// Returns a new array containing the values from this array with the values from the other array subtracted.
	  /// <para>
	  /// The amounts are subtracted from the matching element in this array.
	  /// The arrays must have the same size.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  another array of multiple currency values. </param>
	  /// <returns> a new array containing the values from this array added with the values from the other array subtracted </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public MultiCurrencyAmountArray minus(MultiCurrencyAmountArray other)
	  {
		if (other.size() != size_Renamed)
		{
		  throw new System.ArgumentException(Messages.format("Sizes must be equal, this size is {}, other size is {}", size_Renamed, other.size()));
		}
		ImmutableMap.Builder<Currency, DoubleArray> builder = ImmutableMap.builder();
		foreach (Currency currency in Sets.union(values.Keys, other.values.Keys))
		{
		  DoubleArray array = values.get(currency);
		  DoubleArray otherArray = other.values.get(currency);
		  if (otherArray == null)
		  {
			builder.put(currency, array);
		  }
		  else if (array == null)
		  {
			builder.put(currency, otherArray.multipliedBy(-1));
		  }
		  else
		  {
			builder.put(currency, array.minus(otherArray));
		  }
		}
		return of(builder.build());
	  }

	  /// <summary>
	  /// Returns a new array containing the values from this array with the values from the amount subtracted.
	  /// <para>
	  /// The amount is subtracted from each element in this array.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to subtract </param>
	  /// <returns> a new array containing the values from this array with the values from the amount subtracted </returns>
	  public MultiCurrencyAmountArray minus(MultiCurrencyAmount amount)
	  {
		ImmutableMap.Builder<Currency, DoubleArray> builder = ImmutableMap.builder();
		foreach (Currency currency in Sets.union(values.Keys, amount.Currencies))
		{
		  DoubleArray array = values.get(currency);
		  if (array == null)
		  {
			builder.put(currency, DoubleArray.filled(size_Renamed, -amount.getAmount(currency).Amount));
		  }
		  else if (!amount.contains(currency))
		  {
			builder.put(currency, array);
		  }
		  else
		  {
			builder.put(currency, array.minus(amount.getAmount(currency).Amount));
		  }
		}
		return MultiCurrencyAmountArray.of(builder.build());
	  }

	  /// <summary>
	  /// Returns a multi currency amount array representing the total of the input arrays.
	  /// <para>
	  /// If the input contains the same currency more than once, the amounts are added together.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="arrays">  the amount arrays </param>
	  /// <returns> the total amounts </returns>
	  public static MultiCurrencyAmountArray total(IEnumerable<CurrencyAmountArray> arrays)
	  {
		return Guavate.stream(arrays).collect(toMultiCurrencyAmountArray());
	  }

	  /// <summary>
	  /// Returns a collector which creates a multi currency amount array by combining a stream of
	  /// currency amount arrays.
	  /// <para>
	  /// The arrays in the stream must all have the same length.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the collector </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static java.util.stream.Collector<CurrencyAmountArray, ?, MultiCurrencyAmountArray> toMultiCurrencyAmountArray()
	  public static Collector<CurrencyAmountArray, ?, MultiCurrencyAmountArray> toMultiCurrencyAmountArray()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Collector.of<CurrencyAmountArray, IDictionary<Currency, CurrencyAmountArray>, MultiCurrencyAmountArray>(Hashtable::new, (map, ca) => map.merge(ca.Currency, ca, CurrencyAmountArray::plus), (map1, map2) =>
		{
		map2.values().forEach((ca2) => map1.merge(ca2.Currency, ca2, CurrencyAmountArray::plus));
		return map1;
		}, map =>
		{
		IDictionary<Currency, DoubleArray> currencyArrayMap = MapStream.of(map).mapValues(caa => caa.Values).toMap();
		return MultiCurrencyAmountArray.of(currencyArrayMap);
	}, UNORDERED);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code MultiCurrencyAmountArray}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static MultiCurrencyAmountArray.Meta meta()
	  {
		return MultiCurrencyAmountArray.Meta.INSTANCE;
	  }

	  static MultiCurrencyAmountArray()
	  {
		MetaBean.register(MultiCurrencyAmountArray.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override MultiCurrencyAmountArray.Meta metaBean()
	  {
		return MultiCurrencyAmountArray.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the size of this array. </summary>
	  /// <returns> the value of the property </returns>
	  public int Size
	  {
		  get
		  {
			return size_Renamed;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency values, keyed by currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableSortedMap<Currency, DoubleArray> Values
	  {
		  get
		  {
			return values;
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
		  MultiCurrencyAmountArray other = (MultiCurrencyAmountArray) obj;
		  return (size_Renamed == other.size_Renamed) && JodaBeanUtils.equal(values, other.values);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(size_Renamed);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(values);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("MultiCurrencyAmountArray{");
		buf.Append("size").Append('=').Append(size_Renamed).Append(',').Append(' ');
		buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code MultiCurrencyAmountArray}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  size_Renamed = DirectMetaProperty.ofImmutable(this, "size", typeof(MultiCurrencyAmountArray), Integer.TYPE);
			  values_Renamed = DirectMetaProperty.ofImmutable(this, "values", typeof(MultiCurrencyAmountArray), (Type) typeof(ImmutableSortedMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "size", "values");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code size} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> size_Renamed;
		/// <summary>
		/// The meta-property for the {@code values} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableSortedMap<Currency, com.opengamma.strata.collect.array.DoubleArray>> values = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "values", MultiCurrencyAmountArray.class, (Class) com.google.common.collect.ImmutableSortedMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableSortedMap<Currency, DoubleArray>> values_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "size", "values");
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
			case 3530753: // size
			  return size_Renamed;
			case -823812830: // values
			  return values_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends MultiCurrencyAmountArray> builder()
		public override BeanBuilder<MultiCurrencyAmountArray> builder()
		{
		  return new MultiCurrencyAmountArray.Builder();
		}

		public override Type beanType()
		{
		  return typeof(MultiCurrencyAmountArray);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code size} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> size()
		{
		  return size_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code values} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableSortedMap<Currency, DoubleArray>> values()
		{
		  return values_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3530753: // size
			  return ((MultiCurrencyAmountArray) bean).Size;
			case -823812830: // values
			  return ((MultiCurrencyAmountArray) bean).Values;
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
	  /// The bean-builder for {@code MultiCurrencyAmountArray}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<MultiCurrencyAmountArray>
	  {

		internal int size;
		internal SortedDictionary<Currency, DoubleArray> values = ImmutableSortedMap.of();

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
			case 3530753: // size
			  return size;
			case -823812830: // values
			  return values;
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
			case 3530753: // size
			  this.size = (int?) newValue.Value;
			  break;
			case -823812830: // values
			  this.values = (SortedDictionary<Currency, DoubleArray>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override MultiCurrencyAmountArray build()
		{
		  return new MultiCurrencyAmountArray(size, values);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("MultiCurrencyAmountArray.Builder{");
		  buf.Append("size").Append('=').Append(JodaBeanUtils.ToString(size)).Append(',').Append(' ');
		  buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}